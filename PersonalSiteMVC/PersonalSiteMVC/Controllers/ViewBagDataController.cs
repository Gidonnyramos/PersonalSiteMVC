using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PersonalSiteMVC.UI.Models;//Gives us access to the ViewModels in our application.


namespace PersonalSiteMVC.Controllers
{
    public class ViewBagDataController : Controller
    {
        // GET: ViewBagData
        public ActionResult Index()
        {
            //ViewBag.Action = "Hello!";

            return View();
        }

        // GET: ViewBagData
        public ActionResult Contact()
        {
            return View();
        }
    }
}