using AutoMapper;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Threading.Tasks;
using YourForum.Core.Data;
using YourForum.Core.Models;
using YourForum.Core.Services;

namespace YourForum.Web.Features.Accounts
{
    public class Create
    {
        public class Command : IRequest<int>
        {
            [IgnoreMap]
            public int Id { get; set; }
            public string Username { get; set; }
            public string Email { get; set; }
            public string Password { get; set; }
        }

        public class CommandValidator : AbstractValidator<Command>
        {
            public CommandValidator(YourForumContext db)
            {
                RuleFor(m => m.Username).NotEmpty().Length(3, 50);
                RuleFor(m => m.Email).NotEmpty().EmailAddress()
                    .Must((rootobject, email, context) => 
                        !string.IsNullOrWhiteSpace(email) && !db.Accounts.Any(a => a.Email.Equals(email.ToLower())));
                RuleFor(m => m.Password).NotEmpty().Length(6, 120);
            }

            /*
             * .Must(BeAValidEmail).WithMessage("Please specify a valid email");
            private bool BeAValidEmail(string email)
            {
                try { new MailAddress(email); }
                catch (Exception) { return false; }

                return true;
            }
            */
        }

        public class Handler : AsyncRequestHandler<Command, int>
        {
            private readonly YourForumContext _db;
            private readonly IPasswordService _passwordService;

            public Handler(YourForumContext db, IPasswordService passwordService)
            {
                _db = db;
                _passwordService = passwordService;
            }

            protected override async Task<int> HandleCore(Command message)
            {
                var account = Mapper.Map<Command, Account>(message);

                account.Id = message.Id;
                account.Password = _passwordService.HashPassword(message.Password);

                _db.Accounts.Add(account);

                await _db.SaveChangesAsync();

                return account.Id;
            }
        }
    }
}
