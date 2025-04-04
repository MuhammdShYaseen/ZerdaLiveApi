using FirebaseAdmin;
using FirebaseAdmin.Messaging;
using Google.Apis.Auth.OAuth2;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ZerdaLiveApi.FirebaseNotification
{
    public class SendCloudMessage
    {
        public SendCloudMessage()
        {
            /*FirebaseApp.Create(new AppOptions()
            {
                Credential = GoogleCredential.FromFile("aymanetv.json")
            });*/
        }
        public async void SendToOneUser(string registrationToken, Notification notification, Dictionary<string, string> MsgData)
        {
            var message = new Message()
            {
                Data = MsgData,
                Token = registrationToken,
                Notification = notification,
                Apns = new ApnsConfig
                {
                    Aps = new Aps
                    {
                        ContentAvailable = true,
                        MutableContent = true,
                    }
                }
            };
            string response = await FirebaseMessaging.DefaultInstance.SendAsync(message);
            Console.WriteLine("Successfully sent message: " + response);
        }

        public async void SendToAllUser(Notification notification, Dictionary<string, string> MsgData,String Topic)
        {
            var message = new Message()
            {
                Data = MsgData,
                Topic = Topic,
                Notification = notification,
                Apns = new ApnsConfig
                {
                    Aps = new Aps
                    {
                        ContentAvailable = true,
                        MutableContent = true 
                    }
                }
            };
            string response = await FirebaseMessaging.DefaultInstance.SendAsync(message);
            Console.WriteLine("Successfully sent message: " + response);
        }
    }
}