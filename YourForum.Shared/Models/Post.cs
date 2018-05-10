using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace YourForum.Core.Models
{
    public class Post : BaseTenantEntity, ITenantEntity
    {
        public string Subject { get; set; }

        public string Body { get; set; }

        public Account Author { get; set; }
        public int AuthorId { get; set; }

        public Post Parent { get; set; }
        public int ParentId { get; set; }

        public bool IsThreadPost() => ParentId >= 0;

        [InverseProperty("Parent")]
        public List<Post> Replies { get; set; }
    }
}
