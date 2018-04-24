using System;

namespace YourForum.Core.Models
{
    abstract public class BaseEntity : IEntity
    {
        public int Id { get; set; }
        public DateTime? DateCreated { get; set; }
        public DateTime? DateModified { get; set; }
    }
}
