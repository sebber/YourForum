using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace YourForum.Web.Features.Threads
{
    public class ThreadsController : Controller
    {
        private readonly IMediator _mediator;

        public ThreadsController(IMediator mediator) =>
            _mediator = mediator;

        public async Task<IActionResult> Index(Index.Query query) =>
            View(await _mediator.Send(query));

        public async Task<IActionResult> Details(Details.Query query) =>
            View(await _mediator.Send(query));

        public IActionResult Create() =>
            View();

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Create.Command command)
        {
            var threadId = await _mediator.Send(command);

            return RedirectToAction(nameof(Details), nameof(ThreadsController), new { Id = threadId });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Reply(Reply.Command command)
        {
            var threadId = await _mediator.Send(command);

            return RedirectToAction(nameof(Details), nameof(ThreadsController), new { Id = command.ParentId });
        }
    }
}