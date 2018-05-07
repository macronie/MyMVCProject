using MyMVCProject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
 using System.Web.Security;



namespace MyMVCProject.Controllers
{
    public class UserController : Controller
    {
        //Registration Action

        [HttpGet]
        public ActionResult Registration()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Registration(User user)
        {
            bool Status = false;
            string message = "";

            //Model validation
            if (ModelState.IsValid)
            {
                var emailIsExist = IsEmailExist(user.EmailID);
                if (emailIsExist)
                {
                    ModelState.AddModelError("EmailExists", "Email already exists.");
                    return View();
                }
                //Password Hashing 
                user.Password = Crypto.Hash(user.Password);
                user.ConfirmPassword = Crypto.Hash(user.ConfirmPassword);

                //save to DB
                using (MyDataBaseEntities dc = new MyDataBaseEntities())
                {
                    {
                        dc.Users.Add(user);
                        dc.SaveChanges();
                    }
                }
            }
            else
            {
                message = "Invalid Request";

            }
            ViewBag.Message = message;
            ViewBag.Status = Status;
            return View(user);
        }
        //check for email in db already exist
        public bool IsEmailExist(string email)
        {
            using (MyDataBaseEntities dc = new MyDataBaseEntities())
            {
                var activeEmail = dc.Users.Where(a => a.EmailID == email).FirstOrDefault();
                return activeEmail != null;
            }

        }

        //Login Action
        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }

        //Login Post
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(UserLogin login, string ReturnUrl)
        {
            string message = "";
            using (MyDataBaseEntities dc = new MyDataBaseEntities())
            {
                var activeEmail = dc.Users.Where(a => a.EmailID == login.EmailID).FirstOrDefault();
                if (activeEmail != null)
                {
                    int timeout = login.RememberMe ? 525600 : 20; // 525600 min = 1 year
                    var ticket = new FormsAuthenticationTicket(login.EmailID, login.RememberMe, timeout);
                    string encrypted = FormsAuthentication.Encrypt(ticket);
                    var cookie = new HttpCookie(FormsAuthentication.FormsCookieName, encrypted);
                    cookie.Expires = DateTime.Now.AddMinutes(timeout);
                    cookie.HttpOnly = true;
                    Response.Cookies.Add(cookie);
                    if (string.Compare(Crypto.Hash(login.Password), activeEmail.Password) == 0)
                    {
                        if (Url.IsLocalUrl(ReturnUrl))
                        {
                            return Redirect(ReturnUrl);
                        }
                    
                    }
                    else
                    {
                        return RedirectToAction("Index", "Home");

                    }
                    

                }
                else
                {
                    message = "Invalid credential provided";
                }
            }
            ViewBag.Message = message;

            return View();
        }

        //Logout
        [HttpPost]
        [Authorize]
        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Login", "User");

        }

    }
}