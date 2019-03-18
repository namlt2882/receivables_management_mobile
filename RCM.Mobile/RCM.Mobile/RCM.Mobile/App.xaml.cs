using Microsoft.AppCenter;
using Microsoft.AppCenter.Analytics;
using Microsoft.AppCenter.Crashes;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using RCM.Mobile.Views;
using Prism;
using Prism.Ioc;
using RCM.Mobile.ViewModels;
using RCM.Mobile.Services;
using Microsoft.AppCenter.Push;
using Plugin.FirebasePushNotification;
using Prism.Navigation;
using Xamarin.Forms.Internals;
using System.Diagnostics;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]
namespace RCM.Mobile
{
    public partial class App
    {

        /* 
         * The Xamarin Forms XAML Previewer in Visual Studio uses System.Activator.CreateInstance.
         * This imposes a limitation in which the App class must have a default constructor. 
         * App(IPlatformInitializer initializer = null) cannot be handled by the Activator.
         */
        public App() : this(null) { }

        public App(IPlatformInitializer initializer) : base(initializer) { }

        protected override void OnInitialized()
        {
            Xamarin.Forms.Internals.Log.Listeners.Add(new DelegateLogListener((arg1, arg2) => Debug.WriteLine(arg2)));
#if DEBUG
            HotReloader.Current.Start(this);
#endif
            ////////////////Hockey App
            //AppCenter.Start("android=1b825fb4-d069-4218-9adf-a7197b4513a3;"
            //         //+ "uwp={Your UWP App secret here};" +
            //         // "ios={Your iOS App secret here}"
            //         ,
            //         //typeof(Analytics), typeof(Crashes), 
            //         typeof(Push));

            InitializeComponent();
            var settingsService = Container.Resolve<ISettingsService>();
            var firebaseTokenService = Container.Resolve<IFirebaseTokenService>();
            if (!string.IsNullOrEmpty(settingsService.AuthAccessToken))
            {
                if (settingsService.TokenIsExpired)
                {
                    MainPage = new LoginPage();
                }
                else
                {
                    //CrossFirebasePushNotification.Current.Subscribe(settingsService.AuthUserName);
                    MainPage = new MainPage();
                }
            }
            else
            {
                MainPage = new LoginPage();
            }
            //CrossFirebasePushNotification.Current.RegisterForPushNotifications();

            CrossFirebasePushNotification.Current.OnTokenRefresh += (s, p) =>
            {
                if (!string.IsNullOrEmpty(settingsService.AuthAccessToken))
                {
                    if (!settingsService.TokenIsExpired)
                    {
                        firebaseTokenService.UpdateFirebaseToken(settingsService.AuthAccessToken, CrossFirebasePushNotification.Current.Token);
                    }
                }
                System.Diagnostics.Debug.WriteLine($"TOKEN : {p.Token}");
                settingsService.FirebaseToken = CrossFirebasePushNotification.Current.Token;
            };
#if DEBUG
            CrossFirebasePushNotification.Current.OnTokenRefresh += (s, p) =>
            {
                System.Diagnostics.Debug.WriteLine($"TOKEN : {p.Token}");
                settingsService.FirebaseToken = CrossFirebasePushNotification.Current.Token;
            };
            CrossFirebasePushNotification.Current.OnNotificationReceived += (s, p) =>
            {

                System.Diagnostics.Debug.WriteLine("Received");

            };

            CrossFirebasePushNotification.Current.OnNotificationOpened += (s, p) =>
            {
                System.Diagnostics.Debug.WriteLine("Opened");
                foreach (var data in p.Data)
                {
                    System.Diagnostics.Debug.WriteLine($"{data.Key} : {data.Value}");
                }
                if (!string.IsNullOrEmpty(p.Identifier))
                {
                    System.Diagnostics.Debug.WriteLine($"ActionId: {p.Identifier}");

                }

            };
            CrossFirebasePushNotification.Current.OnNotificationAction += (s, p) =>
            {
                System.Diagnostics.Debug.WriteLine("Action");

                if (!string.IsNullOrEmpty(p.Identifier))
                {
                    System.Diagnostics.Debug.WriteLine($"ActionId: {p.Identifier}");
                    foreach (var data in p.Data)
                    {
                        System.Diagnostics.Debug.WriteLine($"{data.Key} : {data.Value}");
                    }
                }
            };
            CrossFirebasePushNotification.Current.OnNotificationDeleted += (s, p) =>
            {

                System.Diagnostics.Debug.WriteLine("Deleted");

            };
#else
                settingsService.FirebaseToken = CrossFirebasePushNotification.Current.Token;
#endif
            Console.WriteLine("TOKEN: " + CrossFirebasePushNotification.Current.Token);

        }

        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            //Navigation
            containerRegistry.RegisterForNavigation<NavigationPage>("NavigationPage");
            containerRegistry.RegisterForNavigation<MainPage>("MainPage");
            containerRegistry.RegisterForNavigation<LoginPage>("LoginPage");
            //Notification
            containerRegistry.RegisterForNavigation<NotificationListPage>("NotificationListPage");
            containerRegistry.RegisterForNavigation<NotificationPage>("NotificationPage");
            //Receivable
            containerRegistry.RegisterForNavigation<ReceivableListPage>("ReceivableListPage");
            containerRegistry.RegisterForNavigation<ReceivableDetailPage>("ReceivableDetailPage");

            //Service
            containerRegistry.Register<IAuthService, AuthService>();
            containerRegistry.Register<IFirebaseTokenService, FirebaseTokenService>();
            containerRegistry.Register<INotificationService, NotificationService>();
            containerRegistry.Register<IRequestProvider, RequestProvider>();
            containerRegistry.Register<IReceivableService, ReceivableService>();
            containerRegistry.Register<ISettingsService, SettingsService>();
            //containerRegistry.Register<IHubConnectionService, HubConnectionService>();
            ///Page
            containerRegistry.RegisterForNavigation<LoginPage, LoginPageViewModel>();
            containerRegistry.RegisterForNavigation<MainPage, MainPageViewModel>();
            containerRegistry.RegisterForNavigation<NotificationListPage, NotificationListPageViewModel>();
            containerRegistry.RegisterForNavigation<NotificationPage, NotificationPageViewModel>();
            containerRegistry.RegisterForNavigation<ReceivableListPage, ReceivableListPageViewModel>();
            containerRegistry.RegisterForNavigation<ReceivableDetailPage, ReceivableDetailPageViewModel>();
            
        }
    }

}
