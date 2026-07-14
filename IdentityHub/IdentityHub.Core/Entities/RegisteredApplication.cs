//using System;
//using System.Collections.Generic;
//using System.ComponentModel.DataAnnotations;
//using System.Text;

//namespace IdentityHub.Core.Entities
//{
//    public class RegisteredApplication
//    {
//        [Key]
//        public Guid AppId { get; set; } = Guid.NewGuid();
//        [Required, StringLength(100)]
//        public string ClientName { get; set; }
//        [Required, StringLength(50)]
//        public string ClientId { get; set; } // Used by Angular Template
//        public string ClientSecretHash { get; set; } // For secure API calls
//        public string AllowedScopes { get; set; } // e.g. "openid profile email"
//        public string RedirectUri { get; set; } // Security whitelist
//        public bool IsActive { get; set; } = true;
//    }
//}
using System;
using System.ComponentModel.DataAnnotations;

namespace IdentityHub.Core.Entities
{
    public class RegisteredApplication
    {
        [Key]
        public Guid AppId { get; set; } = Guid.NewGuid();

        [Required, StringLength(100)]
        public string ClientName { get; set; } = string.Empty;

        [StringLength(50)]
        public string? ClientId { get; set; }

        public string? ClientSecretHash { get; set; }

        public string? AllowedScopes { get; set; }

        [Required]
        public string RedirectUri { get; set; } = string.Empty;

        public bool IsActive { get; set; } = true;
    }
}

