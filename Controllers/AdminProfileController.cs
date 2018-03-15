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
            var data = Db.UserReports.ToList();
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
	}
}