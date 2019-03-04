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
            ////////////////Hockey App
            //AppCenter.Start("android=1b825fb4-d069-4218-9adf-a7197b4513a3;"
            //         //+ "uwp={Your UWP App secret here};" +
            //         // "ios={Your iOS App secret here}"
            //         ,
            //         //typeof(Analytics), typeof(Crashes), 
            //         typeof(Push));
            ///////////////OneSignal

            InitializeComponent();
            var settingsService = Container.Resolve<ISettingsService>();

            if (!string.IsNullOrEmpty(settingsService.AuthAccessToken))
            {
                if (settingsService.TokenIsExpired)
                {
                    MainPage = new NavigationPage(new LoginPage());
                }
                else
                {
                    MainPage = new NavigationPage(new MainPage());
                }
            }
            else
            {
                MainPage = new NavigationPage(new LoginPage());
            }
        }

        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            //Navigation
            containerRegistry.RegisterForNavigation<NavigationPage>("NavigationPage");
            containerRegistry.RegisterForNavigation<MainPage>("MainPage");
            containerRegistry.RegisterForNavigation<LoginPage>("LoginPage");
            containerRegistry.RegisterForNavigation<NotificationListPage>("NotificationListPage");
            containerRegistry.RegisterForNavigation<NotificationPage>("NotificationPage");
            //Service
            containerRegistry.Register<IAuthService, AuthService>();
            containerRegistry.Register<INotificationService, NotificationService>();
            containerRegistry.Register<IRequestProvider, RequestProvider>();
            containerRegistry.Register<ISettingsService, SettingsService>();
            //containerRegistry.Register<IHubConnectionService, HubConnectionService>();
            ///Page
            containerRegistry.RegisterForNavigation<LoginPage, LoginPageViewModel>();
            containerRegistry.RegisterForNavigation<MainPage, MainPageViewModel>();
            containerRegistry.RegisterForNavigation<NotificationListPage, NotificationListPageViewModel>();
            containerRegistry.RegisterForNavigation<NotificationPage, NotificationPageViewModel>();
        }
    }

}
