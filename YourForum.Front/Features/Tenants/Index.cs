using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using YourForum.Core.Data;

namespace YourForum.Front.Features.Tenants
{
    public class Index
    {
        public class Query : IRequest<Result>
        {

        }

        public class Result
        {
            public List<Tenant> Tenants { get; set; }

            public class Tenant
            {
                public int Id { get; set; }
                public string Name { get; set; }
                public DateTime? DateCreated { get; set; }
            }
        }

        public class Handler : AsyncRequestHandler<Query, Result>
        {
            private readonly YourForumContext _db;

            public Handler(YourForumContext db) => _db = db;

            protected override async Task<Result> HandleCore(Query request)
            {
                var tenants = await _db.Tenants
                    .OrderBy(t => t.DateCreated)
                    .ProjectTo<Result.Tenant>()
                    .ToListAsync();

                return new Result
                {
                    Tenants = tenants
                };
            }
        }
    }
}
