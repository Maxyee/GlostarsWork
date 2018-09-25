                var blockfollowercounter = user.FollowerList.Count(a => _db.Blocks.Any(x =>
                                (((x.BlockedWhomUserId == a.Id) || (x.BlockedByUserId == a.Id )) && x.IsBlock)));

                var afterblockfollowercounter = user.FollowerList.Count - blockfollowercounter;
                //-----------------------------------------------------------------------------------------------------------------------------


                //Block following list count -------------------------------------------------------------------------------------------------
                var blockfollowingcounter = user.FollowingList.Count(a=> _db.Blocks.Any(x =>
                                (((x.BlockedWhomUserId == a.Id) || (x.BlockedByUserId == a.Id)) && x.IsBlock)));

                var afterblockfollowingcounter = user.FollowingList.Count - blockfollowingcounter;