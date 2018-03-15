using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Common.CommandTrees;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using glostars.Models;
using glostars.Models.AdminPanelReport;
using Microsoft.AspNet.SignalR.Messaging;

namespace glostars.Controllers
{
     public class AdminReportController : Controller
    {
        public ApplicationDbContext Db = new ApplicationDbContext();

        // Admin process is starts from here ---> developed by Eyamin
        [HttpGet]
        public ActionResult Login()
        {
            if (Session["AdminIsLogedIn"] != null)
            {
                return RedirectToAction("Dashboard", "AdminProfile");
            }
            return View();
        }

        [HttpPost]
        public ActionResult Login(AdminLoginData data)
        {
            if (ModelState.IsValid)
            {
                var edata = Db.Admins.FirstOrDefault(x => x.Email == data.Email && x.Password == data.Password);
                
                var timedata = new AdminLoginData
                {
                    Email = data.Email,
                    Password = data.Password,
                };
                Db.AdminLoginDatas.Add(timedata);
                Db.SaveChanges();

                
                if (edata != null)
                {
                    Session["AdminIsLogedIn"] = true;
                    return RedirectToAction("Dashboard", "AdminProfile");
                }
            }
            ViewBag.Error = "Wrong username and password given";
            return View();
        }


        public ActionResult Logout()
        {
            Session["AdminIsLogedIn"] = null;
            return RedirectToAction("Login", "AdminReport");
        }
	}
}