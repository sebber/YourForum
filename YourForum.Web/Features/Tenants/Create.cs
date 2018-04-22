using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MediatR;
using AutoMapper;
using YourForum.Web.Data;
using YourForum.Web.Models;

namespace YourForum.Web.Features.Tenants
{
    public class Create
    {
        public class Command : IRequest<int>
        {
            [IgnoreMap]
            public int Id { get; set; }
            public string Name { get; set; }
        }

        public class Handler : AsyncRequestHandler<Command, int>
        {
            private readonly YourForumContext _db;

            public Handler(YourForumContext db) => _db = db;

            protected override async Task<int> HandleCore(Command message)
            {
                var tenant = Mapper.Map<Command, Tenant>(message);
                tenant.Id = message.Id;

                _db.Tenants.Add(tenant);

                await _db.SaveChangesAsync();

                return tenant.Id;
            }
        }
    }
}
