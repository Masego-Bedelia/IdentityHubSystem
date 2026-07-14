using IdentityHub.Core.Entities;
using System.ComponentModel.DataAnnotations;

public class UserAppPermission
{
    [Key]
    public Guid PermissionId { get; set; }

    public Guid UserId { get; set; }
    public User User { get; set; }
    public Guid AppId { get; set; }
    public Guid RoleId { get; set; }
    public ApplicationRole Role { get; set; }

}
