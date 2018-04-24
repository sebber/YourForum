using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using YourForum.Core.Data;
using YourForum.Core.Models;

namespace YourForum.Web.Features.Accounts
{
    public class Index
    {
        public class Query : IRequest<Result>
        {
            public int ForumId { get; set; }
        }

        public class Result
        {
            public List<Model> Accounts { get; set; }
        }

        public class Model
        {
            public string Id { get; set; }
            public string UserName { get; set; }
            public string Email { get; set; }
            public DateTime? DateCreated { get; set; }
        }

        public class Handler : AsyncRequestHandler<Query, Result>
        {
            private readonly YourForumContext _db;

            public Handler(YourForumContext db) => _db = db;

            protected override async Task<Result> HandleCore(Query request)
            {
                var accounts = await _db.Users
                    .Where(u => u.TenantId == request.ForumId)
                    .OrderBy(u => u.DateCreated)
                    .ProjectTo<Model>()
                    .ToListAsync();

                return new Result
                {
                    Accounts = accounts
                };
            }
        }
    }
}
