using System;
using System.Collections.Generic;
using System.Text;

namespace YourForum.Core.Models
{
    public interface ITenantEntity : IEntity
    {
        int TenantId { get; set; }
        Tenant Tenant { get; set; }
    }
}
