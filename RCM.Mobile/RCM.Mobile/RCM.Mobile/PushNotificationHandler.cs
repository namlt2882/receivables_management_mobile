using Plugin.FirebasePushNotification.Abstractions;
using System;
using System.Collections.Generic;
using System.Text;

namespace RCM.Mobile
{
    public class PushNotificationHandler : IPushNotificationHandler
    {
        public void OnError(string error)
        {
            throw new NotImplementedException();
        }

        public void OnOpened(NotificationResponse response)
        {
            
        }

        public void OnReceived(IDictionary<string, object> parameters)
        {
            
        }
    }
}
