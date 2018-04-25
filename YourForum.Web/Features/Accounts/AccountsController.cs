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

        public async Task<IActionResult> Index(Index.Query query)
        {
            var result = await _mediator.Send(query);

            return View(result);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Create.Command command)
        {
            await _mediator.Send(command);

            return RedirectToAction(nameof(Index));
        }
    }
}