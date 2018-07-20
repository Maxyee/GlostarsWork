                var permissionmeUser = _db.UserPrivacys.FirstOrDefault(x => x.PermissionByUserId == MyId && x.PermissionWhomUserId == userId);
                var permissionheUser = _db.UserPrivacys.FirstOrDefault(x => x.PermissionByUserId == userId && x.PermissionWhomUserId == MyId);

                //var x = permissionUser.Permission
                string meUser = "";
                string heUser = "";

                
                if (permissionmeUser != null && permissionheUser != null)
                {
                    permissionmeUser.Permission = permissionheUser.Permission;
                    meUser = permissionmeUser.Permission;
                    permissionheUser.Permission = permissionmeUser.Permission;
                    heUser = permissionheUser.Permission;
                }
                
                if (permissionmeUser == null && permissionheUser != null)
                {
                    meUser = "Network";
                    heUser = permissionheUser.Permission;
                }

                if (permissionmeUser != null && permissionheUser == null)
                {
                    meUser = permissionmeUser.Permission;
                    heUser = "Network";
                }

                if (permissionmeUser == null && permissionheUser == null)
                {
                    meUser = "";
                    heUser = "";
                }