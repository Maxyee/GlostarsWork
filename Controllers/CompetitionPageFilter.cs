public ActionResult Competition(int? page)
{

    String MyId = User.Identity.GetUserId();
    
    var pageSize = 48;
    var pageNumber = (page ?? 1);
    var picsPaged =
        _context.Pictures.Where(w => (w.IsCompeting && (!_context.Blocks.Any(x => ((x.BlockedWhomUserId == w.User_Id) || (x.BlockedByUserId == w.User_Id || x.BlockedWhomUserId == MyId)) && x.IsBlock))) || (w.IsCompeting && w.User_Id == MyId))
            .OrderByDescending(w => w.Uploaded)
            .ToList()
            .ToPagedList(pageNumber, pageSize);
    return View(picsPaged);
}