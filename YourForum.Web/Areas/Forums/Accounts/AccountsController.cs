using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace YourForum.Web.Areas.Forums.Accounts
{
    public class AccountsController : ForumController
    {
        private readonly IMediator _mediator;

        public AccountsController(IMediator mediator) => _mediator = mediator;

        public async Task<IActionResult> Index(Index.Query query)
        {
            query.ForumId = Forum.Id;

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
            command.TenantId = Forum.Id;

            await _mediator.Send(command);

            return RedirectToAction(nameof(Index));
        }
    }
}