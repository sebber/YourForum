using System;
using Microsoft.AspNetCore.Mvc;
using YourForum.Web.Models;

namespace YourForum.Web.Areas.Forums
{
    [Area("Forums")]
    abstract public class ForumController : Controller
    {
        public Tenant Forum => HttpContext?.Items["tenant"] as Tenant;
    }
}
