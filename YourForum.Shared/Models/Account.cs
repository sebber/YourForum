using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace YourForum.Core.Models
{
    public class Account : IdentityUser<int>, ITenantEntity
    {
        public int TenantId { get; set; }
        public Tenant Tenant { get; set; }
        
        public DateTime? DateCreated { get; set; }
        public DateTime? DateModified { get; set; }
    }
}
