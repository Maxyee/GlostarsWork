using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Common.CommandTrees;
using System.Linq;
using System.Net.Mime;
using System.Web;
using System.Web.Mvc;
using glostars.Models;
using glostars.Models.AdminPanelReport;

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
            //var time = Db.AdminLoginDatas.Where(x=>x.Date)
            return View();
        }

        public ActionResult ReportList()
        {       
            var data = Db.UserReports.ToList();
            ViewBag.Alldata = data;
            return View();
        }

        public ActionResult History()
        {
            return View();
        }

        public ActionResult Settings()
        {
            return View();
        }
	}
}