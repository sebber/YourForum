using System;
using System.Linq;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using YourForum.Core.Data;
using YourForum.Core.Services;

namespace YourForum.Web.Features.Home
{
    
    public class HomeController : ForumController
    {
        private readonly IMediator _mediator;

        public HomeController(IMediator mediator) => _mediator = mediator;

        public async Task<IActionResult> Index(Index.Query query)
        {
            query.ForumId = Forum.Id;

            var result = await _mediator.Send(query);

            return View(result);
        }
    }
}
