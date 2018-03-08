using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Http.Cors;
using System.Web.Script.Serialization;
using glostars.Models;
using glostars.Models.AdminPanelReport;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;

namespace glostars.Controllers.Api
{
    [EnableCors("*", "*", "*")]
    [System.Web.Mvc.Authorize]
    [System.Web.Mvc.RoutePrefix("api/userreport")]
    public class UserReportController : ApiController
    {
        private const string LocalLoginProvider = "Local";
        private readonly ApplicationDbContext _db = new ApplicationDbContext();
        private ApplicationUserManager _userManager;

        public UserReportController()
        {
        }

        public UserReportController(ApplicationUserManager userManager,
            ISecureDataFormat<AuthenticationTicket> accessTokeFormat)
        {
            UserManager = userManager;
            AccessTokenFormat = accessTokeFormat;
        }

        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set { _userManager = value; }
        }

        public ISecureDataFormat<AuthenticationTicket> AccessTokenFormat { get; private set; }

        private IAuthenticationManager Authentication
        {
            get { return HttpContext.Current.GetOwinContext().Authentication; }
        }


        [HttpPost]
        [AllowAnonymous]
        public async Task<HttpResponseMessage> UserReport(UserReport model)
        {
            if (ModelState.IsValid)
            {
                if (model == null)
                {
                    var responseMsgtwo = new HttpResponseMessage
                    {
                        Content = new StringContent(new JavaScriptSerializer().Serialize(new
                        {
                            res = false,
                            msg = "Model value is null"
                        }), Encoding.UTF8, "application/json")
                    };
                    return responseMsgtwo;
                }
                else
                {
                    var userreport = new UserReport
                    {
                        ReportCategory = model.ReportCategory,
                        ReportedUserId = model.ReportedUserId,
                        ReportedUserName = model.ReportedUserName,
                        UploadedUserId = model.UploadedUserId,
                        UploadedUserName = model.UploadedUserName,
                        PictureId = model.PictureId,
                    };

                    _db.UserReports.Add(userreport);
                    _db.SaveChanges();

                    var responseMsg = new HttpResponseMessage
                    {
                        Content = new StringContent(new JavaScriptSerializer().Serialize(new
                        {
                            res = false,
                            msg = "User Report Added Successfully",
                            repcat = model.ReportCategory,
                            repusid = model.ReportedUserId,
                            repusnam = model.ReportedUserName,
                            upusid = model.UploadedUserId,
                            upusnam = model.UploadedUserName,
                            picid = model.PictureId
                        }), Encoding.UTF8, "application/json")
                    };
                    return responseMsg;
                }
                
            }
            else
            {
                var responseMsgtwo = new HttpResponseMessage
                {
                    Content = new StringContent(new JavaScriptSerializer().Serialize(new
                    {
                        res = false,
                        msg = "User Report Not Added"
                    }), Encoding.UTF8, "application/json")
                };
                return responseMsgtwo;
            }
        }
    }
}