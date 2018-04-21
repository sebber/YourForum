using System;
namespace YourForum.Web.Models
{
    public class Tenant : BaseEntity, Entity
    {
        public string Name { get; set; }
    }
}
