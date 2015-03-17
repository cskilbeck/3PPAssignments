using System.Web.Mvc;
using Who.Models;

namespace Who.Controllers
{
    public class RegionsController : Controller
    {
        public ActionResult Index()
        {
            RegionsSet r = new RegionsSet();
            return View(r);
        }
    }
}
