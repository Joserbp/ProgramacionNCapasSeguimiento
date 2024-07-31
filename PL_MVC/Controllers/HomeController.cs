using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PL_MVC.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            ServiceReferencePaises.CountryInfoServiceSoapTypeClient paises = new ServiceReferencePaises.CountryInfoServiceSoapTypeClient("CountryInfoServiceSoap");

            paises.FullCountryInfoAllCountries();

            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}