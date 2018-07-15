        [Authorize]
        [HttpGet]
        public dynamic UserPermissionNetwork(string userId)
        {
           string appUserId = User.Identity.GetUserId();
           var appUser = _context.Users.First(s => s.Id == appUserId);
           var permissionUser = _context.Users.First(s => s.Id == userId);

           if (permissionUser != null)
           {
               var permission = _context.UserPrivacys.First(x => x.PermissionByUserId == appUserId && x.PermissionWhomUserId == userId);

               if (permission != null)
               {
                   permission.Permission = "Network";
                   _context.SaveChanges();

                   return new
                   {
                       res = true,
                       msg = "This user successfully changed to Network privacy"
                   };
               }
               else
               {
                   return new
                   {
                       res = true,
                       msg = "This user is already in Network privacy"
                   };
               }
           }
           return new
           {
               res = false,
               msg = "No user found for the Permission"
           };
        }