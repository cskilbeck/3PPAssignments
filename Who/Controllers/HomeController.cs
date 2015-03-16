using System.Diagnostics;
using System.Web.Mvc;
using Who.Models;
using Microsoft.VisualBasic.FileIO;
using System.IO;

namespace Who.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            AccountSet a = new AccountSet();
            try
            {
                using (TextFieldParser parser = new TextFieldParser(Server.MapPath("~") + "\\temp.csv"))
                {
                    parser.TextFieldType = FieldType.Delimited;
                    parser.SetDelimiters(",");
                    while (!parser.EndOfData)
                    {
                        string[] values = parser.ReadFields();

                        if (values[0] == "Account Name" &&
                            values[1] == "Manager Role" &&
                            values[2] == "Full Name (Microsoft Manager)" &&
                            values[3] != null &&
                            values[4] != null &&
                            values[5] != null)
                        {
                            a.Add(values[3], values[4], values[5]);
                        }
                    }
                } 
            }
            catch (System.IO.FileNotFoundException)
            {
                a.Error = "Data file not found in " + Directory.GetCurrentDirectory() + "@" + Server.MapPath("~");
            }
            catch (System.IO.IOException)
            {
                a.Error = "IO Error loading data file from " + Directory.GetCurrentDirectory();
            }
            return View(a);
        }
    }
}
