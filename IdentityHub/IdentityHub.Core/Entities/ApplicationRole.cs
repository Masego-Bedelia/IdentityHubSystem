using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace IdentityHub.Core.Entities
{
    public class ApplicationRole
    {
        [Key]
        public Guid RoleId { get; set; } = Guid.NewGuid();
        public Guid AppId { get; set; }
        [ForeignKey("AppId")]
        public virtual RegisteredApplication Application { get; set; }
        [Required]
        public string RoleName { get; set; } // e.g. "Admin", "HR_Manager"
    }
}
