using MediatR;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace YourForum.Web.Features.Authentication
{
    public class AuthenticationController : ForumController
    {
        private readonly IMediator _mediator;

        public AuthenticationController(IMediator mediator) =>
            _mediator = mediator;

        public IActionResult SignIn() =>
            View();

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SignIn(SignIn.Command command)
        {
            await _mediator.Send(command);

            return RedirectToAction("Index", "Home", new { forumId = RouteData.Values["forumId"] });
        }

        [Authorize]
        public IActionResult SignOut()
        {
            HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            return RedirectToAction("SignIn");
        }
    }
}
