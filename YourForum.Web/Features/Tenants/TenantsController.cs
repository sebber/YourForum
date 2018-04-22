using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace YourForum.Web.Features.Tenants
{
    public class TenantsController : Controller
    {
        private readonly IMediator _mediator;

        public TenantsController(IMediator mediator) => _mediator = mediator;

        public async Task<IActionResult> Index(Index.Query query)
        {
            var result = await _mediator.Send(query);

            return View(result.Tenants);
        }

        public IActionResult Create()
        {
            return View();
        }
    }
}