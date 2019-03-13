using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Plugin.FirebasePushNotification.Abstractions;

namespace RCM.Mobile.Droid
{
    public class PushNotificationHandler : IPushNotificationHandler
    {
        public void OnError(string error)
        {
            
        }

        public void OnOpened(NotificationResponse response)
        {
            
        }

        public void OnReceived(IDictionary<string, object> parameters)
        {
            
        }
    }
}