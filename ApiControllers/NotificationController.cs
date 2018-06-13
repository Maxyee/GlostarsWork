                var userOldNotification =
                    _context.Notifications.Where(x => x.OriginatedById == user.Id && x.PictureId == model.PhotoId);

                if (userOldNotification.Any())
                {
                    _context.Notifications.RemoveRange(userOldNotification);
                    _context.SaveChanges();
                }


                if (pictureToRate != null && myId != pictureToRate.User_Id)
                    // if i'm not rating my own picture, then create a notification
                {
                    var noti = _context.Notifications.Add(new Notification
                    {
                        UserId = pictureToRate.User_Id,
                        OriginatedById = myId,
                        Seen = false,
                        Checked = false,
                        PictureId = pictureToRate.Id,
                        Date = DateTime.Now,
                        Description = "starred your picture"
                    });
                    try
                    {
                        _context.SaveChanges();

                        if (model.NumOfStars > 0)
                        {
                            var list = new List<string>();
                            list.Add(pictureToRate.User_Id);
                            GlostarsHub.AddPictureNotification(list,
                                JsonConvert.SerializeObject(MVCNotifToApiNotif(noti,"has starred your photo"), Formatting.Indented,
                                    new JsonSerializerSettings
                                    {
                                        ContractResolver = new CamelCasePropertyNamesContractResolver()
                                    }));
                                //Real Time Update
                        }

                        response.ResponseCode = ResponseCodes.Successful;
                        response.Message = "Successfully give rating";
                        response.ResultPayload = new
                        {
                            picId = model.PhotoId,
                            userId = User.Identity.GetUserId(),
                            userRating = model.NumOfStars,
                            totalRating = pictureToRate.TotalStars
                        };
                    }
                    catch (Exception e)
                    {
                        return InternalServerError(e);
                    }
                }