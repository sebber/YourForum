using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using YourForum.Core.Data;
using YourForum.Core.Models;

namespace YourForum.Web.Features.Threads
{
    public class Index
    {
        public class Query : IRequest<Response>
        {
        }

        public class Response
        {
            public List<Thread> Threads { get; set; }

            public class Thread
            {
                public int Id { get; set; }
                public string Subject { get; set; }
                public string Body { get; set; }
                public DateTime DateCreated { get; set; }
                public int ReplyCount { get; set; }
            }
        }

        public class Handler : AsyncRequestHandler<Query, Response>
        {
            private readonly YourForumContext _db;

            public Handler(YourForumContext db) => _db = db;

            protected override async Task<Response> HandleCore(Query request)
            {
                var threads = await _db.Posts
                    .Where(t => t.ParentId <= 0)
                    .OrderBy(u => u.DateCreated)
                    .ProjectTo<Response.Thread>()
                    .ToListAsync();

                return new Response
                {
                    Threads = threads
                };
            }
        }
    }
}
