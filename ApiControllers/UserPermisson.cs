    public dynamic UserPermission(UserPrivacyModelView permission)
        {
            string appUserId = User.Identity.GetUserId();
            string p = permission.Permission;
            var appUser = _context.Users.First(x => x.Id == appUserId);
            var permissionUser = _context.Users.First(x => x.Id == permission.PermissionWhomUserId);
            
            if (permissionUser != null)
            {
                var isUserInPermissionList = _context.UserPrivacys.Any(x => x.PermissionByUserId == appUserId && x.PermissionWhomUserId == permission.PermissionWhomUserId);

                if (!isUserInPermissionList)
                {
                    _context.UserPrivacys.Add(new UserPrivacy()
                    {
                        PermissionByUserId = appUserId,
                        PermissionWhomUserId = permission.PermissionWhomUserId,
                        Permission = p,
                        Time = DateTime.Now
                    });
                    _context.SaveChanges();
                
                    return new
                    {
                        res = true,
                        msg = "This User added to your "+ p +" Permission List"
                    };
                }
                else if (isUserInPermissionList)
                {
                    var permissionOver = _context.UserPrivacys.First(x => x.PermissionByUserId == appUserId && x.PermissionWhomUserId == permission.PermissionWhomUserId);

                    permissionOver.Permission = p;
                    _context.SaveChanges();

                    return new
                    {
                        res = true,
                        msg = "This user successfully changed to "+p+" privacy"
                    };
                }
                else
                {
                    return new
                    {
                        res = true,
                        msg = "This user already in "+p+" Permission List"
                    };
                }
            }
            return new
            {
                res = false,
                msg = "No found for give permission"
            };
        }
    }