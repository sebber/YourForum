using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;

namespace YourForum.Core.Infrastructure
{
    public class TenantProvider
    {
        public readonly IHttpContextAccessor _accessor;

        public TenantProvider(IHttpContextAccessor accessor)
        {
            _accessor = accessor;
        }

        public int GetTenantId()
        {
            var segments = _accessor?.HttpContext?.Request?.Path.Value?.Split('/') ?? new string[0];
            if (segments.Length > 1 && int.TryParse(segments[1], out int forumId))
                return forumId;

            return 0;
        }
    }
}
