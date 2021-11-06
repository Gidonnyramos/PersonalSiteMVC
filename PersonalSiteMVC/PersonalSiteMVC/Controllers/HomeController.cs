using PersonalSiteMVC.Ui.MVC.Models; //Added for access to the ContactViewModel class
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Web;
using System.Web.Mvc;

namespace PersonalSiteMVC.Controllers
{
    public class HomeController : Controller
    {
        //http://domain/Controller/Action/optionalParams
        public ActionResult Index()
        {
            //I could write logic to access data from the database here and display it in this individual view. The path for this view is the domain/Home/Index
            return View();
        }

       

        //The path for About would be Domain/Home/About
        public ActionResult Resume()
        {
            //ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Links()
        {
            //ViewBag.Message = "Your contact page.";

            return View();
        }


        public ActionResult Portfolio()
        {
            //ViewBag.Message = "Your contact page.";

            return View();
        }

        public ActionResult Contact()
        {
            //ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}