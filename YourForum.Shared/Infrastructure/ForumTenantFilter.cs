using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YourForum.Core.Data;

namespace YourForum.Core.Infrastructure
{
    public class ForumTenantFilter : IAsyncActionFilter
    {
        private readonly YourForumContext _db;

        public ForumTenantFilter(YourForumContext db)
        {
            _db = db;
        }

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            if (int.TryParse(context.RouteData.Values["forumId"].ToString(), out int forumId))
            {
                var forum = await _db.Tenants.Where(t => t.Id == forumId).SingleOrDefaultAsync();

                if (forum != null)
                {
                    context.HttpContext.Items.Add("tenant", forum);
                    await next();
                }
                else
                {
                    context.HttpContext.Response.StatusCode = 404;
                    return;
                }
            }
        }
    }
}
