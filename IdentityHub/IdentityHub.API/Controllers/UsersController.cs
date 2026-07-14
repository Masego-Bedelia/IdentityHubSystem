using IdentityHub.Core.Entities;
using IdentityHub.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using BC = BCrypt.Net.BCrypt;

namespace IdentityHub.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IdentityDbContext _context;

        public UsersController(IdentityDbContext context)
        {
            _context = context;
        }

        // POST: api/Users/register
        [HttpPost("register")]
        public async Task<IActionResult> RegisterUser(string email, string password)
        {
            if (await _context.Users.AnyAsync(u => u.Email == email))
                return BadRequest("User already exists.");

            var user = new User
            {
                Email = email,
                PasswordHash = BC.HashPassword(password), 
                IsActive = true
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return Ok(new { message = "User registered successfully", userId = user.UserId });
        }

        // POST: api/Users/assign-role
        [HttpPost("assign-role")]
        public async Task<IActionResult> AssignRoleToUser(Guid userId, Guid appId, string roleName)
        {
            // 1. Ensure the Role exists for this specific App
            var role = await _context.ApplicationRoles
                .FirstOrDefaultAsync(r => r.AppId == appId && r.RoleName == roleName);

            if (role == null)
            {
                role = new ApplicationRole { AppId = appId, RoleName = roleName };
                _context.ApplicationRoles.Add(role);
                await _context.SaveChangesAsync();
            }

            // 2. Map the User to the App with this Role
            var permission = new UserAppPermission
            {
                UserId = userId,
                PermissionId = appId,
                RoleId = role.RoleId
            };

            _context.UserAppPermissions.Add(permission);
            await _context.SaveChangesAsync();

            return Ok("Role assigned successfully.");
        }
        [HttpPost("login")]
        public async Task<IActionResult> Login(string email, string password)
        {
            // 1. Find user and their permissions
            var user = await _context.Users
                .Include(u => u.Permissions)
                .ThenInclude(p => p.Role)
                .FirstOrDefaultAsync(u => u.Email == email);

            if (user == null || !BC.Verify(password, user.PasswordHash))
                return Unauthorized("Invalid credentials.");

            // 2. Prepare the Token Claims (The "Data" inside the token)
            var claims = new List<Claim>
    {
        new Claim(ClaimTypes.Name, user.Email),
        new Claim(ClaimTypes.NameIdentifier, user.UserId.ToString()),
        new Claim("IdentityHub_User", "true")
    };

            // 3. Add App-Specific Roles to the Token
            foreach (var perm in user.Permissions)
            {
                claims.Add(new Claim("app_access", perm.AppId.ToString()));
                claims.Add(new Claim(ClaimTypes.Role, perm.Role.RoleName));
            }

            // 4. Create the Token
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("YOUR_VERY_SECRET_KEY_1234567890_MUST_BE_LONG"));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: "IdentityHub",
                audience: "IdentityHub.Applications",
                claims: claims,
                expires: DateTime.Now.AddHours(8),
                signingCredentials: creds
            );

            return Ok(new
            {
                token = new JwtSecurityTokenHandler().WriteToken(token),
                expiration = token.ValidTo
            });
        }
    }
}
