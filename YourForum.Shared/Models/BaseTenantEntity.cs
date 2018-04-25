using System;
using System.Collections.Generic;
using System.Text;

namespace YourForum.Core.Models
{
    abstract public class BaseTenantEntity : BaseEntity, ITenantEntity
    {
        public int TenantId { get; set; }
        public Tenant Tenant { get; set; }
    }
}
