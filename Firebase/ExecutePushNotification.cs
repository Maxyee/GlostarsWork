public static async Task ExcutePushNotification(string title, string msg, string userMail, object data)
{
    var Client = new HttpClient();
    userMail = userMail.Replace("@", "_");

    //var myJson = "{\"to\"😕"/topics/"+ userMail + "\",\"data\":{\"returnurl\"😕""+ returnUrl + "\",\"body\"😕""+msg+"\",\"title\"😕""+ title + "\"}}";

    var firebaseNotificationInformation = new FireBaseNotificationViewModel
    {
        Notification = new FireBaseNotification
        {
            Title = title,
            Body = msg
        },
        Data = data
    };

    string jsonMessage = JsonConvert.SerializeObject(firebaseNotificationInformation);
    string myJson = jsonMessage;

    Client.DefaultRequestHeaders.Authorization =
        new AuthenticationHeaderValue("key", "=your key");
    Client.DefaultRequestHeaders.Add("Sender", "id=your Id");
    try
    {
        HttpResponseMessage res = await Client.PostAsync("https://fcm.googleapis.com/fcm/send",
            new StringContent(myJson, Encoding.UTF8, "application/json"));
        Console.WriteLine(res);
    }
    catch (Exception e)
    {
        Console.WriteLine(e.ToString());
    }
}