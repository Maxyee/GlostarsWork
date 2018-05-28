[System.Web.Http.HttpGet]
public async Task<IHttpActionResult> NameOftheMethod(int CommentValue)
{
    var result = new ApiResponseModel();
    var comment = Db.Comments.Find(CommentValue);

    var commentpicturerealtime = Db.Pictures.FirstOrDefault(w => w.Id == comment.PictureId);

    var s = User.Identity.GetUserId();

    if (comment != null)
    {
        if (comment.CommentUserNameId != s && comment.Picture.User_Id != s)
        {
            result.ResponseCode = ResponseCodes.Successful;
            result.ResultPayload = new
            {
                res = false,
                msg = "Invalid User..."
            };
            return Ok(result);
        }
        Db.Comments.Remove(comment);
        Db.SaveChanges();

        /*For Real time Delete the comment....Julhas*/
        GlostarsHub.UpdatePictureInfo(JsonConvert.SerializeObject(MvCtoApiPicture(commentpicturerealtime),
            Formatting.Indented,
            new JsonSerializerSettings { ContractResolver = new CamelCasePropertyNamesContractResolver() }));
        /*For Real time Delete the comment....Julhas*/

        result.ResponseCode = ResponseCodes.Successful;
        result.ResultPayload = new
        {
            res = true,
            msg = "Comment Removed Successfully"
        };

        return Ok(result);
    }
    result.ResponseCode = ResponseCodes.Successful;
    result.ResultPayload = new
    {
        res = false,
        msg = "No Comment Found..."
    };
    return Ok(result);
}