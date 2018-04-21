using System;
namespace YourForum.Web.Models
{
    abstract public class BaseEntity : Entity
    {
        public int Id { get; set; }
        public DateTime? DateCreated { get; set; }
        public DateTime? DateModified { get; set; }
    }
}
