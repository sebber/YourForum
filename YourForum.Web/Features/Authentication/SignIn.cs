using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using YourForum.Core.Data;
using YourForum.Core.Models;
using YourForum.Core.Services;

namespace YourForum.Web.Features.Authentication
{
    public class SignIn
    {
        public class Command : IRequest
        {
            public string Email { get; set; }
            public string Password { get; set; }
        }

        public class Handler : AsyncRequestHandler<Command>
        {
            private readonly YourForumContext _db;
            private readonly IPasswordService _passwordService;
            private readonly IHttpContextAccessor _httpContextAccessor;

            public Handler(
                IHttpContextAccessor httpContextAccessor,
                YourForumContext db, 
                IPasswordService passwordService)
            {
                _httpContextAccessor = httpContextAccessor;
                _db = db;
                _passwordService = passwordService;
            }

            protected override async Task HandleCore(Command message)
            {
                var account = await _db.Accounts.SingleOrDefaultAsync(a => a.Email.ToLower().Equals(message.Email.ToLower()));

                if (account != null && _passwordService.VerifyPassword(account.Password, message.Password))
                {
                    var claims = new List<Claim>
                    {
                        new Claim(ClaimTypes.Name, account.Username),
                        new Claim(ClaimTypes.Email, account.Email),
                        new Claim("Id", account.Id.ToString()),
                        new Claim("LastModified", account.DateModified.ToString())
                    };

                    var claimsIdentity = new ClaimsIdentity(
                        claims,
                        CookieAuthenticationDefaults.AuthenticationScheme);

                    await  _httpContextAccessor.HttpContext.SignInAsync(
                            CookieAuthenticationDefaults.AuthenticationScheme,
                            new ClaimsPrincipal(claimsIdentity));
                }
            }
        }
    }
}
