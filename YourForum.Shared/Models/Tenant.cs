using System;

namespace YourForum.Core.Models
{
    public class Tenant : BaseEntity, Entity
    {
        public string Name { get; set; }
    }
}
