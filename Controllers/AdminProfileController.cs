using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Common.CommandTrees;
using System.Linq;
using System.Net.Mime;
using System.Web;
using System.Web.Mvc;
using glostars.Models;
using glostars.Models.AdminPanelReport;
using Microsoft.Ajax.Utilities;

namespace glostars.Controllers
{

    [Filters.AutorizeAdmin]
    public class AdminProfileController : Controller
    {
        //
        // GET: /AdminProfile/
        public ApplicationDbContext Db = new ApplicationDbContext();

        public ActionResult Dashboard()
        {

            var lastlogin = Db.AdminLoginDatas.OrderByDescending(e => e.ID).Skip(1).Take(1).FirstOrDefault();
            ViewBag.LastData = lastlogin.Date;

            return View();
        }

        public ActionResult ReportList()
        {       
            var data = Db.UserReports.Where(x=>x.Action == false).ToList();
            ViewBag.Alldata = data;

            var pictable = Db.Pictures.ToList();
            //var extension = from pic in pictable
            //                where 
            ViewBag.AllPicture = pictable;
            //var userdata = Db.UserReports.Where(x=>x.)

            //var allusers = Db.Users.ToList();

            //var findingname = from m in allusers
            //                  where 
            return View();
        }

        public ActionResult History()
        {
            var data = Db.UserReports.Where(x => x.Action == true).ToList();
            ViewBag.Alldata = data;

            var pictable = Db.Pictures.ToList();
            ViewBag.AllPicture = pictable;

            return View();
        }

        public ActionResult Settings()
        {
            return View();
        }

        //method for changing action of a report
        [HttpPost]
        public ActionResult Action(int reportid, bool action)
        {
            int id = reportid;
            //bool result = action;

            var usrrpt = Db.UserReports.ToList();

            var query = (from m in usrrpt
                where m.ID == id
                select m).First();

            query.Action = action;
            Db.SaveChanges();
            //var user = new UserReport() {ID = reportid, Action = action};

            //if (id != null)
            //{
            //    Db.UserReports.Attach(user);
            //    Db.Entry(user).Property(x=> x.)
            //}
            
            return RedirectToAction("ReportList");
        }

        public ActionResult FalseAction(int reportid, bool action)
        {
            int id = reportid;
            //bool result = action;

            var usrrpt = Db.UserReports.ToList();

            var query = (from m in usrrpt
                         where m.ID == id
                         select m).First();

            query.Action = action;
            Db.SaveChanges();
            //var user = new UserReport() {ID = reportid, Action = action};

            //if (id != null)
            //{
            //    Db.UserReports.Attach(user);
            //    Db.Entry(user).Property(x=> x.)
            //}

            return RedirectToAction("History");
        }
	}
}