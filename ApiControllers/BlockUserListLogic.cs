        public async Task<IHttpActionResult>MethodNameById(string userId, int page)
        {
 
            if (page == null)
            {
                page = 0;
            }
            var response = new ApiResponseModel();

            var appUser = _context.Users.First(x => x.Id == userId);

            if (appUser != null)
            {
                //LinQ
                var blockUserList = from m in _context.Blocks.ToList()
                    where m.BlockedByUserId == userId
                                    select new { Id = m.BlockedWhomUserId, Name = m.BlockedWhomUser.Name, Lastname = m.BlockedWhomUser.LastName, ProfilemediumPath = m.BlockedWhomUser.GetProfilePictureThumb(Sizes.big)};

                var userBlockList = blockUserList.Skip(page*10).Take(10);

                response.ResponseCode = ResponseCodes.Successful;
                response.ResultPayload = new
                {
                    userBlockList
                };
                return Ok(response);
                
                
            }

            response.Message = "User not found";
            response.ResponseCode = ResponseCodes.Failed;
            response.ResultPayload = null;
            return Ok(response);

        }