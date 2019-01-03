public class PushNotificaitonService
{
    private static Uri FireBasePushNotificationURL = new Uri("https://fcm.googleapis.com/fcm/send");

    private static string ServerKey =
        "your api server key";

    private static string senderId = "your senderId";

    public static async Task<bool> SendPushNotification(string singleUser, string title, string body, object data)
    {
        bool sent = false;

        if (singleUser != null)
        {
            var firebaseNotificationInformation = new FireBaseNotificationViewModel()
            {
                Notification = new FireBaseNotification()
                {
                    Title = title,
                    Body = body
                },
                Data = data
            };
            
            string jsonMessage = JsonConvert.SerializeObject(firebaseNotificationInformation);

            var request = new HttpRequestMessage(HttpMethod.Post, FireBasePushNotificationURL);
            request.Headers.TryAddWithoutValidation("Authorization", "key=" + ServerKey);
            request.Headers.TryAddWithoutValidation("Sender", senderId);
            request.Content = new StringContent(jsonMessage, Encoding.UTF8, "application/json");

            //HttpResponseMessage result;
            using (var client = new HttpClient())
            {
                HttpResponseMessage result = await client.SendAsync(request);
                sent = sent || result.IsSuccessStatusCode;
            }
            
        }

        return sent;
        
    }



    
}