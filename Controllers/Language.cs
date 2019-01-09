public ActionResult Change(LanguageModel model)
{
    //fetch the language and set the global application language to user's preference
    var lang = model.SelectedLanguage;
    System.Web.HttpContext.Current.Session["lang"] = lang;
    if (model != null)
    {
        
        Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture(model.SelectedLanguage.ToString());
        Thread.CurrentThread.CurrentUICulture = new CultureInfo(model.SelectedLanguage.ToString());
        HttpCookie cookie = new HttpCookie("language");
        cookie.Value = model.SelectedLanguage.ToString();
        Response.Cookies.Add(cookie);

        Uri prevUrl = Request.UrlReferrer;
        if (prevUrl != null)
        {
            return Redirect(prevUrl.ToString());
        }

        //redirect to the page from which this request was posted
        return RedirectToAction("Index", "Home");
    }
    else
    {
        return RedirectToAction("Index", "Home");
    }
    
}