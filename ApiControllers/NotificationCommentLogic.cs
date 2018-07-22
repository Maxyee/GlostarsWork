            public async Task<IHttpActionResult> APiMethodNameGuessit(CommentsBindingModel model)
            {
            var result = new ApiResponseModel();

            IdentityUser user = await UserManager.FindByIdAsync(RequestContext.Principal.Identity.GetUserId());
            var appUser = _context.Users.FirstOrDefault(w => w.Id == user.Id);

            //finding all commented user for specific picture
            var iscommentedUser = _context.Comments.Any(d => d.PictureId == model.PhotoId);

            var commentedUsers = _context.Comments.Where(m => m.PictureId == model.PhotoId);

            var uploadedPhoto = _context.Pictures.Find(model.PhotoId);
            if (uploadedPhoto == null)
            {
                result.Message = "No Picture Found!!";
                result.ResponseCode = ResponseCodes.Failed;
                result.ResultPayload = null;
                return Ok(result);
            }

            try
            {
                var comment = new Models.Comment();
                comment.CommentTime = DateTime.Now;
                comment.CommentMessage = model.CommentText;
                comment.PictureId = model.PhotoId;
                comment.CommentUserNameId = appUser.Id; // conditional access of the appUser variable
                var c = _context.Comments.Add(comment);

                if (User.Identity.GetUserId() == _context.Pictures.Find(model.PhotoId).User_Id)
                {

                    _context.SaveChanges();

                    GlostarsHub.UpdatePictureInfo(JsonConvert.SerializeObject(MvCtoApiPicture(comment.Picture),
                        Formatting.Indented,
                        new JsonSerializerSettings { ContractResolver = new CamelCasePropertyNamesContractResolver() }));
                    //Real Time Update
                        
                }
                else
                {
                    if (iscommentedUser == false)
                    {
                        var noti = _context.Notifications.Add(new Notification
                        {
                            UserId = _context.Pictures.Find(model.PhotoId).User_Id,
                            OriginatedById = User.Identity.GetUserId(),
                            Seen = false,
                            Checked = false,
                            PictureId = model.PhotoId,
                            Date = DateTime.Now,
                            Description = "commented on your picture"
                        });

                        var list = new List<string>();
                        list.Add(_context.Pictures.Find(model.PhotoId).User_Id);


                        GlostarsHub.AddPictureNotification(list,
                            JsonConvert.SerializeObject(MVCNotifToApiNotif(noti, "has commented on your photo"), Formatting.Indented,
                                new JsonSerializerSettings { ContractResolver = new CamelCasePropertyNamesContractResolver() }));
                        //Real Time Update

                        _context.SaveChanges();
                    }
                    else
                    {

                        foreach (var commentedUser in commentedUsers)
                        {
                            if (commentedUser.CommentUserNameId == _context.Pictures.Find(model.PhotoId).User_Id)
                            {
                                var noti = _context.Notifications.Add(new Notification
                                {
                                    UserId = _context.Pictures.Find(model.PhotoId).User_Id,
                                    OriginatedById = User.Identity.GetUserId(),
                                    Seen = false,
                                    Checked = false,
                                    PictureId = model.PhotoId,
                                    Date = DateTime.Now,
                                    Description = "commented on your picture"
                                });

                                var list = new List<string>();
                                list.Add(_context.Pictures.Find(model.PhotoId).User_Id);

                                GlostarsHub.AddPictureNotification(list,
                                JsonConvert.SerializeObject(MVCNotifToApiNotif(noti, "has commented on your photo"), Formatting.Indented,
                                new JsonSerializerSettings { ContractResolver = new CamelCasePropertyNamesContractResolver() }));

                                _context.SaveChanges();
                            }
                            else
                            {
                                var noti = _context.Notifications.Add(new Notification
                                {
                                    UserId = commentedUser.CommentUserNameId,
                                    OriginatedById = User.Identity.GetUserId(),
                                    Seen = false,
                                    Checked = false,
                                    PictureId = model.PhotoId,
                                    Date = DateTime.Now,
                                    Description = "commented on the picture which you already commented"
                                });

                                var list = new List<string>();
                                list.Add(commentedUser.CommentUserNameId);

                                GlostarsHub.AddPictureNotification(list,
                                JsonConvert.SerializeObject(MVCNotifToApiNotif(noti, "has commented on the picture which you already commented"), Formatting.Indented,
                                new JsonSerializerSettings { ContractResolver = new CamelCasePropertyNamesContractResolver() }));

                                _context.SaveChanges();
                            }

                        }

                        GlostarsHub.UpdatePictureInfo(JsonConvert.SerializeObject(MvCtoApiPicture(comment.Picture),
                        Formatting.Indented,
                        new JsonSerializerSettings { ContractResolver = new CamelCasePropertyNamesContractResolver() }));
                    }
                }

                    

                result.ResponseCode = ResponseCodes.Successful;
                result.ResultPayload = new
                {
                    commentId = c.CommentId,
                    c.CommentMessage,
                    commenterUserName = appUser.UserName,
                    CommentUserNameId = appUser.Id,
                    commentTime = c.CommentTime.ToUniversalTime(),
                    profilePicUrl = appUser.GetProfilePictureThumb(Sizes.big),
                    ProfilePicUrlMedium = appUser.GetProfilePictureThumb(Sizes.medium),
                    ProfilePicUrlMini = appUser.GetProfilePictureThumb(Sizes.mini),
                    ProfilePicUrlSmall = appUser.GetProfilePictureThumb(Sizes.small),
                    firstName = appUser.Name,
                    lastName = appUser.LastName,
                    PictureId = model.PhotoId
                };
                result.Message = "Comment posted successfully";
                return Ok(result);
            }
                catch (Exception ex)
                {
                    return InternalServerError(ex);
                }
            }


            //this code has some problem but i dont to delete it thats why i upload it into the github