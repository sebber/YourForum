using AutoMapper;
using FluentValidation;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using YourForum.Core.Data;
using YourForum.Core.Models;

namespace YourForum.Web.Features.Threads
{
    public class Create
    {
        public class Command : IRequest<int>
        {
            [IgnoreMap]
            public int Id { get; set; }
            public int AuthorId { get; set; }
            public string Subject { get; set; }
            public string Body { get; set; }
        }

        public class CommandValidator : AbstractValidator<Command>
        {
            public CommandValidator(YourForumContext db)
            {
                RuleFor(m => m.Subject).NotEmpty().Length(3, 250);
                RuleFor(m => m.Body).NotEmpty().Length(1, 5000);
            }
        }

        
        public class Handler : AsyncRequestHandler<Command, int>
        {
            private readonly YourForumContext _db;

            public Handler(YourForumContext db)
            {
                _db = db;
            }

            protected override async Task<int> HandleCore(Command message)
            {
                var thread = Mapper.Map<Command, Post>(message);

                thread.Id = message.Id;

                _db.Posts.Add(thread);

                await _db.SaveChangesAsync();

                return thread.Id;
            }
        }
    }
}
