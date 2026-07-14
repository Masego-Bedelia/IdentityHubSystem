using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using IdentityHub.Data;
using IdentityHub.Core.Entities;

namespace IdentityHub.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ApplicationsController : ControllerBase
    {
        private readonly IdentityDbContext _context;

        public ApplicationsController(IdentityDbContext context)
        {
            _context = context;
        }

        // GET: api/Applications
        [HttpGet]
        public async Task<ActionResult<IEnumerable<RegisteredApplication>>> GetApplications()
        {
            return await _context.RegisteredApplications.ToListAsync();
        }

        [HttpPost("register")]
        public async Task<ActionResult<RegisteredApplication>> RegisterApplication(RegisteredApplication app)
        {
          
            app.AppId = Guid.NewGuid();

          
            if (string.IsNullOrWhiteSpace(app.ClientId) || app.ClientId == "string")
            {
                app.ClientId = Guid.NewGuid().ToString("N").Substring(0, 12);
            }
            else
            {
                if (await _context.RegisteredApplications.AnyAsync(a => a.ClientId == app.ClientId))
                {
                    return BadRequest(new { error = $"The ClientId '{app.ClientId}' is already in use." });
                }
            }

           
            if (string.IsNullOrWhiteSpace(app.ClientSecretHash) || app.ClientSecretHash == "string")
            {
                app.ClientSecretHash = Guid.NewGuid().ToString("B"); 
            }

            if (string.IsNullOrWhiteSpace(app.AllowedScopes) || app.AllowedScopes == "string")
            {
                app.AllowedScopes = "openid profile email";
            }

            _context.RegisteredApplications.Add(app);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetApplications), new { id = app.AppId }, app);
        }

    }
}
