using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Mime;
using System.Web;
using System.Web.Http;
using System.Web.Http.Cors;
using projects.projects_Hub;
using projects.Helpers;
using projects.Models;
using projects.Models.Api;
using projects.Utilities;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;

namespace GiveProjectName.Controllers.Api
{
    [EnableCors("*","*","*")]
    [System.Web.Http.Authorize]
    [System.Web.Http.RoutePrefix("give/yourul/for/api/work")]
    public class EditImagesController : ApiController
    {
        private readonly ApplicationDbContext _context = new ApplicationDbContext();
           
        [System.Web.Http.HttpGet]
        [System.Web.Http.Route("give/url/for/api/data")]
        public IHttpActionResult MethodName(int id)
        {

            var response = new ApiResponseModel();
            var pictureid = _context.Pictures.FirstOrDefault(x=>x.Id == id);

            var model = _context.Editpictures.Where(x => x.PictureId == id);

            try
            {
                if (pictureid == null)
                {
                    response.ResponseCode = ResponseCodes.Failed;
                    response.Message = "Picture id not found";
                    return Ok(response);
                }

                var size = model.Count();

                var allpicture = new ArrayList();

                foreach (var data in model)
                {
                    allpicture.Add(data);
                }

                if (size > 0)
                {
                    response.ResponseCode = ResponseCodes.Successful;
                    response.Message = "Pictures successfully retrieved";
                    response.ResultPayload = new
                    {
                        allpicture,
                        allpicture.Count
                    };
                    return Ok(response);
                }
                else
                {
                    response.ResponseCode = ResponseCodes.Failed;
                    response.Message = "No Editable Pictures";
                    response.ResultPayload = new
                    {
                        allpicture,
                        allpicture.Count
                    };
                    return Ok(response);
                }

            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }

        }


       
        [System.Web.Http.HttpPost]
        [System.Web.Http.Route("give/your/api/address/for/doing/the/work")]
        public IHttpActionResult MethodName(int id)
        {
            var response = new ApiResponseModel();
            var editpicture = _context.Editpictures.FirstOrDefault(e => e.Id == id);

            if (User.Identity.GetUserId() == editpicture.User_Id || User.Identity.GetUserId() == editpicture.Picture.User_Id)
            {
                 ProjectHub.RemovePicture(editpicture.Id + ""); // Real time update
                _context.Editpictures.Remove(editpicture);
                _context.SaveChanges();

                var sm = new StorageManager();
                sm.RemoveBlob(BlobContainers.Pictures, editpicture.Id.ToString());
                sm.RemoveBlob(BlobContainers.PicturesThumbsMedium, editpicture.Id.ToString());
                sm.RemoveBlob(BlobContainers.PicturesThumbs,editpicture.Id.ToString());
            }
            else
            {
                response.Message = "Invalid User Id";
                response.ResultPayload = new
                {
                    result = false
                };
                return Ok(response);
            }
            response.Message = "Successfully Delete";
            response.ResultPayload = new
            {
                result = true
            };
            return Ok(response);
        }
    }
}