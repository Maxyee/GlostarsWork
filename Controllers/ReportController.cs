using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using glostars.Models;
using glostars.Models.AdminPanelReport;
using Microsoft.AspNet.Identity;

namespace glostars.Controllers
{
    [Authorize]
    public class ReportController : Controller
    {
        public ApplicationDbContext Db = new ApplicationDbContext();
        //
        // GET: /Report/
        public ActionResult Report()
        {
            return View();
        }

        public ActionResult Spam()
        {
            return View();
        }

        public ActionResult Inappropriate(int id, string userid)
        {
            var picid = id;
            //var uploaduserid = userid;

            ViewBag.PictureId = picid;
            ViewBag.UploadUserId = userid;

            return View();
        }

        public ActionResult Final(int id, string userid)
        {
            var picid = id;
            //var uploaduserid = userid;

            ViewBag.PictureId = picid;
            ViewBag.UploadUserId = userid;

            
            return View();
        }

        public ActionResult Reported()
        {
            return View();
        }

        public ActionResult Selfinjury(int id, string userid)
        {
            var picid = id;
            //var uploaduserid = userid;

            ViewBag.PictureId = picid;
            ViewBag.UploadUserId = userid;

            
            return View();
        }

        public ActionResult Harassment(int id, string userid)
        {
            var picid = id;
            //var uploaduserid = userid;

            ViewBag.PictureId = picid;
            ViewBag.UploadUserId = userid;

            
            return View();
        }

        public ActionResult Saleorpromotion(int id, string userid)
        {
            var picid = id;
            //var uploaduserid = userid;

            ViewBag.PictureId = picid;
            ViewBag.UploadUserId = userid;

            
            return View();
        }

        public ActionResult Promotionoffirearms(int id, string userid)
        {
            var picid = id;
            //var uploaduserid = userid;

            ViewBag.PictureId = picid;
            ViewBag.UploadUserId = userid;

            
            return View();
        }

        public ActionResult Violence(int id, string userid)
        {
            var picid = id;
            //var uploaduserid = userid;

            ViewBag.PictureId = picid;
            ViewBag.UploadUserId = userid;

           
            return View();
        }

        public ActionResult Speech(int id, string userid)
        {
            var picid = id;
            //var uploaduserid = userid;

            ViewBag.PictureId = picid;
            ViewBag.UploadUserId = userid;

            
            return View();
        }

        public ActionResult Intellectual(int id, string userid)
        {
            var picid = id;
            //var uploaduserid = userid;

            ViewBag.PictureId = picid;
            ViewBag.UploadUserId = userid;

            
            return View();
        }

        public ActionResult Donotlike(int id, string userid)
        {
            var picid = id;
            //var uploaduserid = userid;

            ViewBag.PictureId = picid;
            ViewBag.UploadUserId = userid;

            
            return View();
        }

        public ActionResult Piccontroller(int id, string userid)
        {
            var picid = id;
            var uploaduserid = userid;
            //var uploadusername = username;

            ViewBag.PictureId = picid;
            ViewBag.UploadUserId = uploaduserid;
            //ViewBag.UploadUserName = uploadusername;
           
            return View("Spam");
        }

        [HttpPost]
        public ActionResult ReportSubmit(int picid, string uploaduserid, string reportedusername, string reporteduserid, string reportedcategory)
        {
            var uploaduser = Db.Users.ToList();
            var linquploadusername = (from m in uploaduser
                                  where m.Id == uploaduserid
                                  select m.Name).SingleOrDefault();

            var uploadedusername = linquploadusername;

            var model = new UserReport
            {
                ReportCategory = reportedcategory,
                ReportedUserId = reporteduserid,
                ReportedUserName = reportedusername,
                UploadedUserName = uploadedusername,
                UploadedUserId = uploaduserid,
                PictureId = picid
            };

            Db.UserReports.Add(model);
            Db.SaveChanges();

            return RedirectToAction("Reported");
        }

        [HttpPost]
        public ActionResult ReportOption(int picid, string uploadedby, string reportedby, string reportcategory)
        {
            //var username = User.Identity.GetUserName();
            if (reportcategory == "Self injury")
            {
                return RedirectToAction("Selfinjury");
            }
            else if (reportcategory == "Harassment or bullying")
            {
                return RedirectToAction("Harassment");
            }
            else if (reportcategory == "Sale or promotion of drugs")
            {
                return RedirectToAction("Saleorpromotion");
            }
            else if (reportcategory == "Sale or promotion of firearms")
            {
                return RedirectToAction("Promotionoffirearms");
            }
            else if (reportcategory == "Nudity or pornography")
            {
                return RedirectToAction("Final");
            }
            else if (reportcategory == "Violence or harm")
            {
                return RedirectToAction("Violence");
            }
            else if (reportcategory == "Hate speech or symbols")
            {
                return RedirectToAction("Speech");
            }
            else if (reportcategory == "Intellectual property violation")
            {
                return RedirectToAction("Intellectual");
            }
            else if (reportcategory == "I just don't like it")
            {
                return RedirectToAction("Donotlike");
            }

            return View("Spam");
        }
	}
}