using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace YourForum.Web.Features.Accounts
{
    public class AccountsController : ForumController
    {
        private readonly IMediator _mediator;

        public AccountsController(IMediator mediator) => _mediator = mediator;

        public async Task<IActionResult> Index(Index.Query query) => 
            View(await _mediator.Send(query));

        public async Task<IActionResult> Details(Details.Query query) =>
            View(await _mediator.Send(query));

        public async Task<IActionResult> Edit(Edit.Query query) =>
            View(await _mediator.Send(query));

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Edit.Command command)
        {
            await _mediator.Send(command);

            return RedirectToAction(nameof(Details), new { command.Id });
        }

        public IActionResult Create() =>
            View();        

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Create.Command command)
        {
            var response = await _mediator.Send(command);

            return RedirectToAction(nameof(Details), new { Id = response });
        }
    }
}