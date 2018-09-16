        public async Task<IHttpActionResult> GetUserSocialinfo(string usermail)
        {
           var response = new ApiResponseModel();

           var user = _db.FacebookInfos.FirstOrDefault(x => x.FacebookEmail == usermail);

           string resultvalue = "";

           if(user != null)
           {
               resultvalue = "This user is in the social media table";
           }
           else
           {
               resultvalue = "This user is not in the social media table";
           }

           response.ResponseCode = ResponseCodes.Successful;
           response.ResultPayload = new
           {
               userSocialMedia = resultvalue
           };
           return Ok(response);
        }