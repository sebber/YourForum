﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using YourForum.Core.Data;

namespace YourForum.Web.Features.Home
{
    public class Index
    {
        public class Query : IRequest<Result>
        {
        }

        public class Result
        {
            public Forum Forum { get; set; }
        }

        public class Forum
        {
            public int Id { get; set; }
            public string Name { get; set; }
            public DateTime? DateCreated { get; set; }
        }

        public class Handler : AsyncRequestHandler<Query, Result>
        {
            private readonly YourForumContext _db;

            public Handler(YourForumContext db) => _db = db;

            protected override async Task<Result> HandleCore(Query request)
            {
                var tenant = await _db.Tenants
                    .ProjectTo<Forum>()
                    .SingleOrDefaultAsync();

                return new Result
                {
                    Forum = tenant
                };
            }
        }
    }
}
