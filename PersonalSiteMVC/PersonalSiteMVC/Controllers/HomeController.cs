using PersonalSiteMVC.UI.Models;
using System;
using System.Net.Mail;
using System.Net;
using System.Web.Mvc;

namespace PersonalSiteMVC.UI.Controllers
{
    public class HomeController : Controller
    {
        //[HttpGet]
        public ActionResult Index()
        {
            return View();
        }

        //[HttpGet]
        //[Authorize]
        public ActionResult Resume()
        {
            //ViewBag.Message = "Your app description page.";

            return View();
        }


        //[HttpGet]
        public ActionResult Links()
        {
            return View();
        }

        //[HttpGet]
        public ActionResult Portfolio()
        {
            return View();
        }

        //GET Request -- When the user clicks a link, go to the View and show the form TO them
        [HttpGet]
        public ActionResult Contact()
        {

            return View();
        }



        //POST Request -- When the user fills out the form, we can collect the data FROM them
        [HttpPost]
        [ValidateAntiForgeryToken] // Check if the form in our View was used to submit the cvm object
        public ActionResult ContactAjax(ContactViewModel cvm)
        {
            //Check that the ModelState is valid (they are sending us valid information for our ContactViewModel)
            if (!ModelState.IsValid)
            {
                //Send them back to the form with the completed inputs
                return View(cvm);
            }

            //Build the message that will be emailed to us
            string message = $"You have received an email from {cvm.Name} with a subject of {cvm.Subject}. Please respond " +
                $"to {cvm.Email} with your response to the following message: <br />{cvm.Message}";

            string justinMessage = $"You have received an email from {cvm.Name}. {(string.IsNullOrEmpty(cvm.Subject) ? "No Subject Provided" : cvm.Subject)}.<br/>" +
                $"Please respond to {cvm.Email} with your response to the following message: <br />{cvm.Message}";


            //MailMessage - What sends the email
            MailMessage mm = new MailMessage(

                //FROM - Who sends the email
                "admin@gidonny.com",

                //TO - Where you want the email sent
                "gidonnyramos@outlook.com",

                //SUBJECT
                cvm.Subject,

                //BODY of the email
                justinMessage

                );

            //MailMessage properties
            //Allow HTML formatting in the email
            mm.IsBodyHtml = true;

            //Set any emails from our contact form as High Priority
            mm.Priority = MailPriority.High;

            //Update the ReplyTo List with the sender's email
            //This will allow us to reply to them (instead of replying to our SmarterASP.net email user)
            mm.ReplyToList.Add(cvm.Email);

            //SmtpClient - SMTP (Secure Mail Transfer Protocol)
            //This is the info from the host that allows us to send the email.
            SmtpClient client = new SmtpClient("mail.gidonny.com");

            //Client Credentials - Login information for your email user
            client.Credentials = new NetworkCredential("admin@gidonny.com", "Password");

            //Try to send the email
            //It's possible that something in our configuration is wrong (typos, incorrect passwords, etc.)
            //OR that the webserver is down (SmarterASP). So, we can use a try/catch to help handle this.

            try
            {
                //Attempt to send the email
                client.Send(mm);
            }
            catch (Exception ex)
            {
                //Format an error message for the user
                ViewBag.CustomerMessage = $"We're sorry, but your request could not be completed at this time. Please " +
                    $"try again later. Error Message: <br />{ex.StackTrace}";

                //Send them back to the View with their completed form data
                return View(cvm);
            }

            //If we made it this far, the email has been sent.
            //Send the user to a confirmation view along with their message

            return View("EmailConfirmation", cvm);
        }

    }
}
