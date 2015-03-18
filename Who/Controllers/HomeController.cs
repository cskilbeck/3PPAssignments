using System;
using System.Web.Mvc;
using Who.Models;
using Microsoft.VisualBasic.FileIO;
using System.IO;

namespace Who.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index(string id)
        {
            if (id == null)
            {
                return Redirect("/Regions");
            }
            else
            {
                AccountSet a = new AccountSet();
                try
                {
                    a.Load(id);
                }
                catch (Exception e)
                {
                    a.Error = e.ToString();
                }
                return View(a);
            }
        }
    }
}
