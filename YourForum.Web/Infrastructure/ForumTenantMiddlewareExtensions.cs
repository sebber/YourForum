using Microsoft.AspNetCore.Builder;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace YourForum.Web.Infrastructure
{
    public static class ForumTenantMiddlewareExtensions
    {
        public static IApplicationBuilder UseForumTenant(
            this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<ForumTenantMiddleware>();
        }
    }
}
