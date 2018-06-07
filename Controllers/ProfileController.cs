    /*-----------------------------Julhas Code starts here-------------------------------------*/
        //After blocking a user / profile of that user should not be shown .

        /// <summary>
        ///     Profile of User
        /// </summary>
        /// <param name="Id"></param>
        [AllowAnonymous]
        public ActionResult Profile(String Id)
        {
            ApplicationUser Profile = _context.Users.FirstOrDefault(w => w.Id == Id);
            String MyId = User.Identity.GetUserId();
            var IsMyFollowerOrMe = MyId == Id || Profile.FollowerList.Any(w => w.Id == MyId);
            ApplicationUser meUser = _context.Users.Find(MyId);


            var appUser = _context.Users.First(x=>x.Id==MyId);
            var blockUser = _context.Users.First(x => x.Id == Id);

            var isUserInBlockList = _context.Blocks.Any(x => x.BlockedByUserId == MyId && x.BlockedWhomUserId==Id && x.IsBlock);

            if (isUserInBlockList)
            {
                return View("Index", "Home");
            }
            else
            {
                ViewBag.PicutresViewModel = new UserPicturesViewModel(Profile, meUser, IsMyFollowerOrMe);
                var recogprofile = Db.RecognitionInProfiles.FirstOrDefault(x => x.EmailId == Profile.Email);
                ViewBag.isrecognition = recogprofile != null;
                if (ViewBag.isrecognition)
                {
                    if (string.IsNullOrEmpty(recogprofile.Weekly) && string.IsNullOrEmpty(recogprofile.Monthly) &&
                        string.IsNullOrEmpty(recogprofile.Exhibition) && string.IsNullOrEmpty(recogprofile.Grand))
                    {
                        ViewBag.isrecognition = false;
                    }
                }
                ViewBag.recogprofiledata = recogprofile;
                return View(Profile);
            } 
        }
    /*-------------------------------Julhas Code ends here-----------------------------------------*/