public ActionResult Facebook()
{
    var fb = new FacebookClient();
    var loginUrl = fb.GetLoginUrl(new
    {
        client_id = appId,
        client_secret = appSecret,
        redirect_uri = RedirectUri.AbsoluteUri,
        response_type = "code",
        scope = "email" // Add other permissions as needed
    });

    return Redirect(loginUrl.AbsoluteUri);
}