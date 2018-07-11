    [Authorize]
    [HttpPost]
    public dynamic UserPermissionInsider(UserPrivacyModelView permission)
    {
        string appUserId = User.Identity.GetUserId();
        
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
                    Permission = "Insider",
                    Time = DateTime.Now
                });
                _context.SaveChanges();

                return new
                {
                    res = true,
                    msg = "This User added to your Insider Permission List"
                };
            }
            else
            {
                return new
                {
                    res = true,
                    msg = "This user already in Insider Permission List"
                };
            }
        }
        return new
        {
            res = false,
            msg = "No found for give permission"
        };
    }

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
}