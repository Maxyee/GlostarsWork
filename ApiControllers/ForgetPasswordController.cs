using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Mime;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Http.Cors;
using System.Web.Script.Serialization;
using glostars.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using Newtonsoft.Json;

namespace glostars.Controllers.Api
{
    [EnableCors("*","*","*")]
    [System.Web.Mvc.Authorize]
    [System.Web.Mvc.RoutePrefix("api/forgetpassword")]
    public class ForgetPasswordController : ApiController
    {
        private const string LocalLoginProvider = "Local";
        private readonly ApplicationDbContext _db = new ApplicationDbContext();
        private ApplicationUserManager _userManager;

        public ForgetPasswordController()
        {
            
        }

        public ForgetPasswordController(ApplicationUserManager userManager, ISecureDataFormat<AuthenticationTicket> accessTokeFormat)
        {
            UserManager = userManager;
            AccessTokeFormat = accessTokeFormat;
        }

        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set { _userManager = value; }
        }

        public ISecureDataFormat<AuthenticationTicket> AccessTokeFormat { get; private set; }

        private IAuthenticationManager Authentication
        {
            get { return HttpContext.Current.GetOwinContext().Authentication; }
        }

        [HttpGet]
        public async Task<HttpResponseMessage> EmailSendCode(string email)
        {
            //ApplicationUser user = _db.Users.Find(email);
            var user = _db.Users.FirstOrDefault(x => x.Email == email);
            if (user == null)
            {                
                var responseMsg = new HttpResponseMessage
                {
                    Content = new StringContent(new JavaScriptSerializer().Serialize(new
                    {
                        result = false,
                        message = "Email Doesn't exist in Database."

                    }), Encoding.UTF8, "application/json")
                };
                return responseMsg;
            }
            else
            {
                Random random = new Random();
                int x = random.Next(1001, 9999);

                user.SecurityToken = x;
                _db.SaveChanges();
                Debug.WriteLine("Active Code : " + x);

                string code = UserManager.GenerateEmailConfirmationToken(user.Id);
                UserManager.SendEmail(user.Id,"Forget Password Verification Code",
                    "Forget Your password ? Please use this code for active your account.<h1>" + x + "</h1>");
               
                var responseMsg = new HttpResponseMessage
                {
                    Content = new StringContent(new JavaScriptSerializer().Serialize(new
                    {
                        result = true,
                        message = "Sucessfully sent email with verfication code."

                    }), Encoding.UTF8, "application/json")
                };
                return responseMsg;
                
            }
        }

        [HttpGet]
        public async Task<HttpResponseMessage> SendCodeAgainEmail(string email)
        {
            
            Random random = new Random();
            int x = random.Next(1001, 9999);

            //ApplicationUser user = _db.Users.Find(email);
            var user = _db.Users.FirstOrDefault(j => j.Email == email);
            if (user == null)
            {
              
                var responseMsg = new HttpResponseMessage
                {
                    Content = new StringContent(new JavaScriptSerializer().Serialize(new
                    {
                        result = false,
                        message = "No user found."

                    }), Encoding.UTF8, "application/json")
                };
                return responseMsg;
            }

            user.SecurityToken = x;
            _db.SaveChanges();
            Debug.WriteLine("Active code: " + x);

            string code = UserManager.GenerateEmailConfirmationToken(user.Id);
            UserManager.SendEmail(user.Id, "Forget Password Verification Code",
                    "Forget Your password ? Please use this code for active your account.<h1>" + x + "</h1>");
            

            var responseMsgsuccess = new HttpResponseMessage
            {
                Content = new StringContent(new JavaScriptSerializer().Serialize(new
                {
                    result = true,
                    message = "Sucessfully sent email with verfication code again."

                }), Encoding.UTF8, "application/json")
            };
            return responseMsgsuccess;
        }

        [HttpPost]
        public async Task<HttpResponseMessage> Password(string email, string password)
        {
            var model = _db.Users.FirstOrDefault(x => x.Email == email);
            if (model == null)
            {
                var responseMsg = new HttpResponseMessage
                {
                    Content = new StringContent(new JavaScriptSerializer().Serialize(new
                    {
                        result = false,
                        message = "email not found"

                    }), Encoding.UTF8, "application/json")
                };
                return responseMsg;
            }
            string token = UserManager.GeneratePasswordResetToken(model.Id);
            IdentityResult identityResult = UserManager.ResetPasswordAsync(model.Id, token, password).Result;

            var responseMsgsuccess = new HttpResponseMessage
            {
                Content = new StringContent(new JavaScriptSerializer().Serialize(new
                {
                    result = true,
                    message = "Successfully changed the password"

                }), Encoding.UTF8, "application/json")
            };
            return responseMsgsuccess;
        }


        [HttpGet]
        public async Task<HttpResponseMessage> ForgetConfirm(string email, int confirmCode)
        {
            var user = _db.Users.FirstOrDefault(x => x.Email == email);
            if (user == null)
            {
                var responseMsg = new HttpResponseMessage
                {
                    Content = new StringContent(new JavaScriptSerializer().Serialize(new
                    {
                        result = false,
                        message = "email not found"

                    }), Encoding.UTF8, "application/json")
                };
                return responseMsg;
            }

            if (confirmCode > 1000 && confirmCode <= 9999)
            {
                if (user.SecurityToken == confirmCode)
                {
                    user.SecurityToken = 0;
                    _db.SaveChanges();

                    //return JsonConvert.SerializeObject(new
                    //{
                    //    result = true,
                    //    Msg = "Confarmation Code Matched Successfully"
                    //});

                    var responseMsg = new HttpResponseMessage
                    {
                        Content = new StringContent(new JavaScriptSerializer().Serialize(new
                        {
                            result = true,
                            message = "Confarmation Code Matched Successfully"

                        }), Encoding.UTF8, "application/json")
                    };
                    return responseMsg;
                }
                var responseMsgfail = new HttpResponseMessage
                {
                    Content = new StringContent(new JavaScriptSerializer().Serialize(new
                    {
                        result = false,
                        message = "Wrong Code"

                    }), Encoding.UTF8, "application/json")
                };
                return responseMsgfail;

            }
            var responseMsgfailed = new HttpResponseMessage
            {
                Content = new StringContent(new JavaScriptSerializer().Serialize(new
                {
                    result = false,
                    message = "Wrong Code"

                }), Encoding.UTF8, "application/json")
            };
            return responseMsgfailed;
        }
    }
}
