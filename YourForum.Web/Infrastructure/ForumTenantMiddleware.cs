using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using YourForum.Web.Data;
using YourForum.Web.Models;

namespace YourForum.Web.Infrastructure
{
    public class ForumTenantMiddleware
    {
        private readonly RequestDelegate _next;

        public ForumTenantMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context, IMediator mediator)
        {
            string requestPath = context.Request.Path.Value;

            if (requestPath.StartsWith("/forums/"))
            {
                var segments = context.Request.Path.Value.Split("/");

                if (int.TryParse(segments[2], out int forumId))
                {
                    var tenant = await mediator.Send(new ForumTenant.Query
                    {
                        Id = forumId
                    });

                    context.Items.Add("tenant", tenant);
                }
            }

            await _next(context);
        }

        public class ForumTenant
        {
            public class Query : IRequest<Tenant>
            {
                public int Id { get; set; }
            }

            public class Handler : AsyncRequestHandler<Query, Tenant>
            {
                private readonly YourForumContext _db;

                public Handler(YourForumContext db) => _db = db;

                protected override async Task<Tenant> HandleCore(Query request)
                {
                    var tenant = await _db.Tenants
                        .Where(t => t.Id == request.Id)
                        .SingleOrDefaultAsync();

                    return tenant;
                }
            }
        }
    }
}
