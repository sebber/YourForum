using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using YourForum.Core.Data;
using YourForum.Core.Models;

namespace YourForum.Web.Features.Accounts
{
    public class Details
    {
        public class Query : IRequest<Response>
        {
            public int Id { get; set; }
        }

        public class Response
        {
            public string Username { get; set; }
            public string Email { get; set; }
        }

        public class Handler : AsyncRequestHandler<Query, Response>
        {
            private readonly YourForumContext _db;

            public Handler(YourForumContext db) => _db = db;

            protected override async Task<Response> HandleCore(Query request) =>
                await _db.Accounts
                    .Where(t => t.Id == request.Id)
                    .ProjectTo<Response>()
                    .SingleOrDefaultAsync();
        }
    }
}
