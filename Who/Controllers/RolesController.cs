using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Http;
using Who.Models;

namespace Who.Controllers
{
    public class RolesController : Controller
    {
        public ActionResult Roles()
        {
            var r = new { };
            return View(r);
        }
    }
}
