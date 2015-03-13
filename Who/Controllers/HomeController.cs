using System.Diagnostics;
using System.Web.Mvc;
using Who.Models;
using Microsoft.VisualBasic.FileIO;

namespace Who.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            AccountSet a = new AccountSet();
            try
            {
                using (TextFieldParser parser = new TextFieldParser("D:\\temp.csv"))
                {
                    parser.TextFieldType = FieldType.Delimited;
                    parser.SetDelimiters(",");
                    while (!parser.EndOfData)
                    {
                        string[] values = parser.ReadFields();
                        for (int i = values.Length; i < 4; ++i)
                        {
                            values[i] = "";
                        }
                        a.Accounts.Add(new AccountSet.Account
                        {
                            Name = values[0],
                            DAM = values[1],
                            AM = values[2],
                            RM = values[3]
                        });
                    }
                } 
            }
            catch (System.IO.FileNotFoundException)
            {
                a.Error = "Data file not found";
            }
            catch (System.IO.IOException)
            {
                a.Error = "IO Error loading data file";
            }
            return View(a);
        }
    }
}
