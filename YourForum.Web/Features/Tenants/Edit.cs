using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using YourForum.Web.Data;

namespace YourForum.Web.Features.Tenants
{
    public class Edit
    {
        public class Query : IRequest<Command>
        {
            public int? Id { get; set; }
        }

        public class QueryValidator : AbstractValidator<Query>
        {
            public QueryValidator()
            {
                RuleFor(m => m.Id).NotNull();
            }
        }

        public class QueryHandler : AsyncRequestHandler<Query, Command>
        {
            private readonly YourForumContext _db;

            public QueryHandler(YourForumContext db) => _db = db;

            protected override Task<Command> HandleCore(Query message) =>
            _db.Tenants
               .Where(t => t.Id == message.Id)
               .ProjectTo<Command>()
               .SingleOrDefaultAsync();
        }

        public class Command : IRequest
        {
            [Display(Name = "Number")]
            public int Id { get; set; }
            public string Name { get; set; }
        }

        public class CommandValidator : AbstractValidator<Command>
        {
            public CommandValidator()
            {
                RuleFor(m => m.Name).NotNull().Length(3, 250);
            }
        }

        public class CommandHandler : AsyncRequestHandler<Command>
        {
            private readonly YourForumContext _db;

            public CommandHandler(YourForumContext db) => _db = db;

            protected override async Task HandleCore(Command message)
            {
                var tenant = await _db.Tenants.FindAsync(message.Id);

                Mapper.Map(message, tenant);
            }
        }
    }
}
