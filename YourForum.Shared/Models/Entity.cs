using System;

namespace YourForum.Core.Models
{
    public interface Entity
    {
        int Id { get; set; }

        DateTime? DateCreated { get; set; }

        DateTime? DateModified { get; set; }
    }
}
