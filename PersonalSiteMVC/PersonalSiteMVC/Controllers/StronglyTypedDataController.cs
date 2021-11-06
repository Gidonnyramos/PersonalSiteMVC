using System;
using System.Collections.Generic;
using System.Web.Mvc;

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
            //When a class has validation attributes, that validation should be checked BEFORE attempting to process any data.
            if (!ModelState.IsValid)
            {
                //something the user typed or didn't type is causing issues...let's send them the form back with all of their values they typed in to each field.
                return View(cvm);
            }

            //Assemble the message itself - This is what we will see in the body of the message being sent from the site.
            string message = $"You have received an email from {cvm.Name}:<br/>" +
                $"Subject: {(string.IsNullOrEmpty(cvm.Subject) ? "No Subject Provided" : cvm.Subject)}<br/>" +
                $"Email: {cvm.Email}<br/>" +
                $"Message:<br/>{cvm.Message}";

            //Then, we can assemble a MailMessage to be staged and used in the SMTPClient when we send the message.
            MailMessage mm = new MailMessage(
                //From
                ConfigurationManager.AppSettings["EmailUser"].ToString(),
                //To - this assumes forwarding by the host...what email do we want this message to send to
                ConfigurationManager.AppSettings["EmailTo"].ToString(),
                //Subject
                cvm.Subject,
                //Body
                message
                );

            //MailMessage properties - configuring the object
            //Allow HTML formatting in the email
            mm.IsBodyHtml = true;

            //if you want to mark these emails with high priority
            mm.Priority = MailPriority.High;

            //respond to user's email rather than our SMTP Client email(smarterasp.net)
            mm.ReplyToList.Add(cvm.Email);

            //SMTP Client object - this is what actually allows us to send the message
            SmtpClient client = new SmtpClient(ConfigurationManager.AppSettings["EmailClient"].ToString())
            {

                //Add in client credentials (username and password from smarterasp.net)
                Credentials = new NetworkCredential(ConfigurationManager.AppSettings["EmailUser"].ToString(), ConfigurationManager.AppSettings["EmailPass"].ToString())
            };

            //Use a try/catch to send the object
            //It is possible that the mail server is down or we may have configuration issues, so we to encapsulate our code to send this message in a try/catch.
            try
            {
                //attempt to send the message
                client.Send(mm);
            }
            catch (Exception ex)
            {
                //If something goes wrong we will populate a ViewBag message for the user so they know it didn't send
                ViewBag.CustomerMessage = $"We're sorry your request could not be completed at this time.<br/>" +
                    $"Please try again later or contact the site admin for further support.<br/>" +
                    $"Stack Trace: {ex.StackTrace}";

                //send the view back with what they have written into the inputs
                return View(cvm);
            }
            //Return the confirmation view - if all goes well return a confirmation view to the user that the email was sent
            return View("EmailConfirmation", cvm);
        }
    }//end class
}//end namespace