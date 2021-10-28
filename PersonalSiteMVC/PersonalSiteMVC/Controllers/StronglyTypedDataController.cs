using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using IntroToMVC.Models;//added for access to the FamilyMemberViewModel and ContactViewModel
using System.Configuration; //This gives me access to objects in the web.config and AppSecretKeys.config
using System.Net;//This allows us to access/use the NetworkCredential object
using System.Net.Mail;//This allows us to use the SMTPClient object to send our message

namespace PersonalSiteMVC.Controllers
{
    public class StronglyTypedDataController : Controller
    {
        // GET: StronglyTypedData
        public ActionResult Index()
        {
            return View();
        }

      
        //Get action - Contact - This is what renders the form to the UI
        public ActionResult Contact()
        {
            return View();
        }

        //Post action - Contact - This is what submits the information for the contact form. In the post action will be our functionality to send a message via an SMTP Client.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Contact(ContactViewModel cvm)
        {
         
            if (!ModelState.IsValid)
            {
               
                return View(cvm);
            }

            
            string message = $"You have received an email from {cvm.Name}:<br/>" +
                $"Subject: {(string.IsNullOrEmpty(cvm.Subject) ? "No Subject Provided" : cvm.Subject)}<br/>" +
                $"Email: {cvm.Email}<br/>" +
                $"Message:<br/>{cvm.Message}";

          
            MailMessage mm = new MailMessage(
                
                ConfigurationManager.AppSettings["EmailUser"].ToString(),
              
                ConfigurationManager.AppSettings["EmailTo"].ToString(),
             
                cvm.Subject,
          
                message
                );

            mm.IsBodyHtml = true;

         
            mm.Priority = MailPriority.High;

         
            mm.ReplyToList.Add(cvm.Email);

          
            SmtpClient client = new SmtpClient(ConfigurationManager.AppSettings["EmailClient"].ToString());

           
            client.Credentials = new NetworkCredential(ConfigurationManager.AppSettings["EmailUser"].ToString(), ConfigurationManager.AppSettings["EmailPass"].ToString());

            try
            {
             
                client.Send(mm);
            }
            catch (Exception ex)
            {
                
                ViewBag.CustomerMessage = $"We're sorry your request could not be completed at this time.<br/>" +
                    $"Please try again later or contact the site admin for further support.<br/>" +
                    $"Stack Trace: {ex.StackTrace}";

                return View(cvm);
            }
         
            return View("EmailConfirmation", cvm);
        }
    }//end class
}//end namespace