using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using YourForum.Core.Data;
using YourForum.Core.Models;

namespace YourForum.Web.Features.Accounts
{
    public class Create
    {
        public class Command : IRequest<string>
        {
            [IgnoreMap]
            public string Id { get; set; }
            public int TenantId { get; set; }
            public string UserName { get; set; }
            public string Email { get; set; }
            [IgnoreMap]
            public string Password { get; set; }
        }

        public class Handler : AsyncRequestHandler<Command, string>
        {
            private readonly UserManager<Account> _db;

            public Handler(UserManager<Account> db) => _db = db;

            protected override async Task<string> HandleCore(Command message)
            {
                var account = Mapper.Map<Command, Account>(message);

                account.Id = message.Id;

                var result = await _db.CreateAsync(account, message.Password);

                if (result.Succeeded)
                {
                    return account.Id;
                }

                return "failed";
            }
        }
    }
}
