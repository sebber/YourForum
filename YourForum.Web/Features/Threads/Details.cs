using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using YourForum.Core.Data;

namespace YourForum.Web.Features.Threads
{
    public class Details
    {
        public class Query : IRequest<Response>
        {
            public int Id { get; set; }
        }

        public class Response
        {
            public Post Thread { get; set; }

            public List<Post> Replies { get; set; }

            public class Post
            {
                public int Id { get; set; }
                public string Subject { get; set; }
                public string Body { get; set; }
                public DateTime DateCreated { get; set; }
                public DateTime DateModified { get; set; }

                public Author Author { get; set; }
            }

            public class Author
            {
                public int Id { get; set; }
                public string Username { get; set; }
            }
        }

        public class Handler : AsyncRequestHandler<Query, Response>
        {
            private readonly YourForumContext _db;

            public Handler(YourForumContext db) => _db = db;

            protected override async Task<Response> HandleCore(Query query)
            {
                var thread = await _db.Posts
                    .Include(p => p.Author)
                    .ProjectTo<Response.Post>()
                    .SingleOrDefaultAsync(p => p.Id == query.Id);

                var replies = await _db.Posts
                    .Include(p => p.Author)
                    .Where(t => t.ParentId == query.Id)
                    .OrderBy(u => u.DateCreated)
                    .ProjectTo<Response.Post>()
                    .ToListAsync();

                return new Response
                {
                    Thread = thread,
                    Replies = replies
                };
            }
        }
    }
}
