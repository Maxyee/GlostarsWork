// feed main logic starts from here ------>
var mutualFollowerActivites = Db.Activities.Where(
    a => ((a.User.FollowingList.Any(b => MyId == b.Id) && a.User.FollowerList.Any(b => MyId == b.Id))
        ||(a.User.FollowerList.Any(b => MyId == b.Id) && a.Picture.Privacy != "friends") 
        ||(a.UserId == MyId)) && !listPhoto.Contains(a.PictureId) && !a.Picture.IsCompeting)
    .OrderByDescending(q => q.Date)
    //.Skip(count*10)
    .Take(10)
    .ToList();
    
//feed main logic ends here -------------->