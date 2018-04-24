using System;
using Microsoft.AspNetCore.Mvc;
using YourForum.Core.Models;

namespace YourForum.Web.Features
{
    abstract public class ForumController : Controller
    {
        public Tenant Forum => HttpContext?.Items["tenant"] as Tenant;
    }
}
