using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.Identity;
using System.Web.Mvc;
using System.Web.Security;
using BusinessLogic;
using Common;
using System.Net.Mail;
using System.Configuration;
using System.Web.Configuration;

namespace SecurityWebsite.Controllers
{
    public class AccountsController : Controller
    {
        // GET: Accoutns
        [HttpGet]
        public ActionResult Login()
        {
            /*
            Configuration config = WebConfigurationManager.OpenWebConfiguration(Request.ApplicationPath);
            ConfigurationSection section = config.GetSection("connectionStrings");
            if (!section.SectionInformation.IsProtected)
            {
                section.SectionInformation.ProtectSection("RsaProtectedConfigurationProvider");
                config.Save();
            }

            Configuration config = WebConfigurationManager.OpenWebConfiguration(Request.ApplicationPath);
            ConfigurationSection section = config.GetSection("connectionStrings");
            if (section.SectionInformation.IsProtected)
            {
                section.SectionInformation.UnprotectSection();
                config.Save();
            }
            */

            if (User.Identity.IsAuthenticated == false)
            {
                return View();
            }
            else
            {
                return RedirectToAction("Index", "Musics");
            }
        }
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(string email,string password)
        {
            UsersBL ul = new UsersBL();

            if (new UsersBL().Login(email, password) == false)
            {
                try
                {
                    if (ul.CheckAccountBlocked(email) == false)
                    {
                        int currentAttempts = 3 - ul.ValidationAttempts(email);

                        TempData["errormessage"] = "Incorrect password entered, you have " + currentAttempts + " tries left before your account is blocked!";
                        return View();
                    }
                    else
                    {
                        TempData["errormessage"] = "Account is currently blocked, contact administrator for access.";
                        return View();
                    }
                }
                catch(Exception ex)
                {
                    Logger.LogMessage("", Request.Path, "Error: " + ex.Message);
                    TempData["errormessage"] = "Incorrect email has been entered.";
                    return View();
                }
            }
            else
            {
                if (ul.CheckAccountBlocked(email) == false)
                {
                    // users needs to be logged in
                    FormsAuthentication.SetAuthCookie(email, true);
                    return RedirectToAction("Index", "Musics");
                }
                else
                {
                    TempData["errormessage"] = "Account is currently blocked, contact administrator for access.";
                    return View();
                }
            }        
        }

        public ActionResult SignOut()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Login");
        }

        [HttpGet]
        public ActionResult Register()
        {
            if (User.Identity.IsAuthenticated == false)
            {
                return View();
            }
            else {
                return RedirectToAction("Index", "Musics");
            }
        }
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Register(User u)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    new UsersBL().Register(u.Email, u.Password, u.FirstName, u.LastName, u.Mobile);
                    Logger.LogMessage("", Request.Path, u.Email + " has been registered successfully");
                    TempData["message"] = "Registered Succesfully";

                }
            }
            catch (Exception ex)
            {
                if(ex.Message == "Email is already taken.") {
                    Logger.LogMessage("", Request.Path, "Error: " + ex.Message);
                    TempData["errormessage"] = ex.Message;
                }
                TempData["errormessage"] = "Something went wrong. Please try again later.";
            }
            return View(u);
        }

        [HttpGet]
        public ActionResult ForgotPassword()
        {
            if (User.Identity.IsAuthenticated == false)
            {
                return View();
            }
            else
            {
                return RedirectToAction("Index", "Musics");
            }
        }

        [HttpPost]
        //[AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult ForgotPassword(string email)
        {
            UsersBL ul = new UsersBL();
            try
            { 
                ul.SetRecoveryCode(email);
                string userCode = ul.GetRecoveryCode(email);

                MailMessage mail = new MailMessage();
                mail.From = new MailAddress("musicsharingrecovery@gmail.com");
                mail.To.Add(email);

                mail.Subject = "Music Sharing Application";
                mail.Body = "+++++++++ Music Sharing Application +++++++++" +
                            "<br />" +
                            "<br />Copy and paste the below recovery code into the website to reset your account " +
                            "<br />" +
                            "<br />Your recovery code : " + userCode +
                            "<br />Your email : " + email +
                             "<br />" +
                            "<br />Sent at " + DateTime.Now.ToString("M/d/yyyy") +
                            "<br />Contact a website administrator if this wasn`t you!" +
                            "<br />" +
                            "<br />+++++++++ THIS IS AN AUTOMATED MESSAGE +++++++++";
                mail.IsBodyHtml = true;

                SmtpClient client = new SmtpClient();
                client.EnableSsl = true;
                client.Host = "smtp.gmail.com";
                client.Port = 587;
                client.EnableSsl = true;
                client.DeliveryMethod = System.Net.Mail.SmtpDeliveryMethod.Network;
                client.Credentials = new System.Net.NetworkCredential("musicsharingrecovery@gmail.com", "IOAN(&$%");
                client.Timeout = 2000000;
                client.Send(mail);
                TempData["message"] = "Recovery code has been sent to the specified email.";

                return View("ResetPassword");
            }
            catch (Exception ex)
            {
                Logger.LogMessage("", Request.Path, "Error: " + ex.Message);
                TempData["errormessage"] = "Invalid email";
            }
            return View();

        }

        [HttpGet]
        public ActionResult ResetPassword()
        {
            if (User.Identity.IsAuthenticated == false)
            {
                return View();
            }
            else
            {
                return RedirectToAction("Index", "Musics");
            };
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ResetPassword(string email,string code, string password)
        {
            try
            {
                UsersBL ul = new UsersBL();
                string userCode = ul.GetRecoveryCode(email);

                if (userCode == code)
                {
                    if (password != null)
                    {
                        ul.ResetPassword(email, password);
                        TempData["message"] = "Account has been reset!";
                        Logger.LogMessage("", Request.Path, "Password has been reseted");
                        return View("Login");
                    }
                    else
                    {
                        TempData["errormessage"] = "Password cannot be left empty!";
                    }
                }
                else
                {
                    TempData["errormessage"] = "Code is incorrect!";
                }

                return View();
            }
            catch (Exception ex)
            {
                Logger.LogMessage("", Request.Path, "Error: " + ex.Message);
                TempData["errormessage"] = "The recovery code and/or email have been entered incorrectly.";
                return View();
            }
        }

    }
}