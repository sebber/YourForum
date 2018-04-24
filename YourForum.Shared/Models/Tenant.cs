using System;

namespace YourForum.Core.Models
{
    public class Tenant : BaseEntity, IEntity
    {
        public string Name { get; set; }
    }
}
