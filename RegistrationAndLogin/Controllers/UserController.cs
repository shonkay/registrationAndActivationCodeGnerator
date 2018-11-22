using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using RegistrationAndLogin.Models;
using System.Net.Mail;
using System.Net;

namespace RegistrationAndLogin.Controllers
{
    public class UserController : Controller
    {
        //Registration Action
        [HttpGet]
        public ActionResult Registration()
        {
            return View();
        }

        //Registration POST action
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Registration([Bind(Exclude ="IsEmailVerified, ActivationCode")] User user)
        {
            bool Status = false;
            string message = "";

            //Modelvalidation
            if (ModelState.IsValid)
            {
            
                #region //Email is already Exist or not
                var isExist = IsEmailExist(user.EmailID);
                if (isExist)
                {
                    ModelState.AddModelError("EmailExist", "Email already exist");
                    return View(user);
                }
                #endregion

                #region Generate Activation Code
                user.ActivationCode = Guid.NewGuid();
                #endregion

                #region Password Hashing
                user.Password = Crypto.Hash(user.Password);
                user.ConfirmPassword = Crypto.Hash(user.ConfirmPassword);
                #endregion
                user.IsEmailVerified = false;

                #region  Sava data to Database
                using(MyDataBaseEntities dc = new MyDataBaseEntities())
                {
                    dc.Users.Add(user);
                    dc.SaveChanges();

                    //Send Email to User
                    sendVerificationLinkEmail(user.EmailID, user.ActivationCode.ToString());
                    message = "Registration successfully done. Account activation link has been sent" +
                        " to your email id:" + user.EmailID;
                    Status = true;
                }
                #endregion
            }
            else
            {
                message = "Invalid Request";
            }

            ViewBag.Message = message;
            ViewBag.Status = Status;
            return View(user);
        }

        //verify Email

        //verify Email LINK

        //login

        //login POST

        //logout

        [NonAction]
        public bool IsEmailExist(string emailID)
        {
            using (MyDataBaseEntities dc = new MyDataBaseEntities())
            {
                var v = dc.Users.Where(a => a.EmailID == emailID).ToList();
                return v.Count>0 ? true : false;
            }
        }

        [NonAction]
        public void sendVerificationLinkEmail(string emailID, string activationCode)
        {
            var verifyUrl = "/User/VerifyAccount/" + activationCode;
            var link = Request.Url.AbsoluteUri.Replace(Request.Url.PathAndQuery, verifyUrl);

            var fromEmail = new MailAddress("shonkayworld1234don@gmail.com", "Kayode(Software Developer)SBSC"); 
            var toEmail = new MailAddress(emailID);
            var fromEmailPassword = "1234oluwaseun"; // To be replaced with actual password
            string subject = "Your account is successfully created!";

            string body = "<br/><br/> Your account has been successfully created. Please click on the " +
                "below link to verify your account" + "<br/><br/> <a href = '" + link + "'>" + link + "<a/>";

            var smtp = new SmtpClient
            {
                Host = "smtp.gmail.com",
                Port = 587,
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(fromEmail.Address, fromEmailPassword)
            };

            using (var message = new MailMessage(fromEmail, toEmail)
            {
                Subject = subject,
                Body = body,
                IsBodyHtml = true
            })
                smtp.Send(message);
        }
    }
}