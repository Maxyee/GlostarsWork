/*User total picture counter, how many photos they have in their profile */
/*---------------Julhas---------------*/
        public IHttpActionResult GetUserPictures(string userId, int count)
        {
            if (count > 0)
            {
                count = count - 1;
            }
            var response = new ApiResponseModel();
            var appUser = _context.Users.FirstOrDefault(w => w.Id == userId);

            try
            {
                if (appUser == null)
                {
                    response.ResponseCode = ResponseCodes.Failed;
                    response.Message = "User not found";
                    return Ok(response);
                }

                var model = new UserPicturesViewModel
                {
                    UserId = userId
                };

                Debug.Assert(appUser != null, "appUser != null");


                var totalCompetitonPic = 0;
                var totalmutualFollowerPics = 0;
                var totalpublicPictures = 0;
                var allPictureCount = appUser.Pictures.Count;

                var competitionPics =
                    appUser.Pictures.Where(w => w.IsCompeting).OrderByDescending(w => w.Uploaded).Skip(count*10)
                        .Take(10);


                model.CompetitionPictures = competitionPics.Select(MvCtoApiPicture).ToList();
                totalCompetitonPic = (count*10) + model.CompetitionPictures.Count();


                if (!model.CompetitionPictures.Any())
                {
                    totalCompetitonPic = appUser.Pictures.Count(w => w.IsCompeting);
                }


                var mutualFollowerPics =
                    appUser.Pictures.Where(x => x.Privacy == "friends" && !x.IsCompeting)
                        .OrderByDescending(w => w.Uploaded)
                        .Skip(count*10)
                        .Take(10);

                model.MutualFollowerPictures = mutualFollowerPics.Select(MvCtoApiPicture).ToList();
                totalmutualFollowerPics = (count*10) + model.MutualFollowerPictures.Count();


                if (!model.MutualFollowerPictures.Any())
                {
                    totalmutualFollowerPics = appUser.Pictures.Count(x => x.Privacy == "friends" && !x.IsCompeting);
                }

                var publicPictures =
                    appUser.Pictures.Where(w => !w.IsCompeting && w.Privacy == "public")
                        .OrderByDescending(w => w.Uploaded)
                        .Skip(count*10)
                        .Take(10);
                model.PublicPictures = publicPictures.Select(MvCtoApiPicture).ToList();
                totalpublicPictures = (count*10) + model.PublicPictures.Count();


                if (!model.PublicPictures.Any())
                {
                    totalpublicPictures = appUser.Pictures.Count(w => !w.IsCompeting && w.Privacy == "public");
                }


                response.ResponseCode = ResponseCodes.Successful;
                response.Message = "Pictures successfully retrieved";
                response.ResultPayload = new
                {
                    allPictureCount,
                    totalCompetitonPic,
                    totalmutualFollowerPics,
                    totalpublicPictures,
                    model
                };
                return Ok(response);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }
/*---------------Julhas---------------*/    
