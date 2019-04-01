using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using RCM.Mobile.Views;
using Prism;
using Prism.Ioc;
using RCM.Mobile.ViewModels;
using RCM.Mobile.Services;
using Plugin.FirebasePushNotification;
using Prism.Navigation;
using Xamarin.Forms.Internals;
using System.Diagnostics;
using Prism.Services;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]
namespace RCM.Mobile
{
    public partial class App : IReloadable
    {

        /* 
         * The Xamarin Forms XAML Previewer in Visual Studio uses System.Activator.CreateInstance.
         * This imposes a limitation in which the App class must have a default constructor. 
         * App(IPlatformInitializer initializer = null) cannot be handled by the Activator.
         */
        public App() : this(null) { }

        public App(IPlatformInitializer initializer) : base(initializer) { }

        public void OnLoaded()
        {

        }

        protected override void OnInitialized()
        {
            Xamarin.Forms.Internals.Log.Listeners.Add(new DelegateLogListener((arg1, arg2) => Debug.WriteLine(arg2)));
#if DEBUG
            HotReloader.Current.Start(this);
#endif
            
            InitializeComponent();
            var settingsService = Container.Resolve<ISettingsService>();
            var firebaseTokenService = Container.Resolve<IFirebaseTokenService>();
            var navigationService = Container.Resolve<INavigationService>();
            var pageDialogService = Container.Resolve<IPageDialogService>();
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
                settingsService.FirebaseToken = CrossFirebasePushNotification.Current.Token;
            };
            CrossFirebasePushNotification.Current.OnNotificationOpened += (s, p) =>
            {
                //pageDialogService.DisplayAlertAsync("Open", "", "OK");
                //NavigationService.NavigateAsync("NavigationPage/ReceivableListPage");
                
            };
            CrossFirebasePushNotification.Current.OnNotificationReceived += (s, p) =>
            {
            };
            CrossFirebasePushNotification.Current.OnNotificationAction += (s, p) =>
            {
                //pageDialogService.DisplayAlertAsync("Action","","OK");
                //NavigationService.NavigateAsync("NavigationPage/ReceivableListPage");
                
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
                NavigationService.NavigateAsync("NavigationPage/NotificationListPage");
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

            containerRegistry.RegisterForNavigation<AssignedReceivablesPage>("AssignedReceivablesPage");
            containerRegistry.RegisterForNavigation<LoginPage>("LoginPage");
            containerRegistry.RegisterForNavigation<MainPage>("MainPage");
            //Notification
            containerRegistry.RegisterForNavigation<NotificationListPage>("NotificationListPage");
            containerRegistry.RegisterForNavigation<NotificationPage>("NotificationPage");
            //Receivable
            containerRegistry.RegisterForNavigation<ReceivableDetailPage>("ReceivableDetailPage");
            containerRegistry.RegisterForNavigation<ReceivableListPage>("ReceivableListPage");
            containerRegistry.RegisterForNavigation<ReceivableTaskListPage>("ReceivableTaskListPage");
            containerRegistry.RegisterForNavigation<TaskDetailPage>("TaskDetailPage");
            containerRegistry.RegisterForNavigation<TaskPage>("TaskPage");
            //Service
            containerRegistry.Register<IAuthService, AuthService>();
            containerRegistry.Register<IFirebaseTokenService, FirebaseTokenService>();
            containerRegistry.Register<INotificationService, NotificationService>();
            containerRegistry.Register<IRequestProvider, RequestProvider>();
            containerRegistry.Register<IReceivableService, ReceivableService>();
            containerRegistry.Register<ISettingsService, SettingsService>();
            containerRegistry.Register<ITaskService, TaskService>();
            containerRegistry.Register<IUtilityService, UtilityService>();
            //containerRegistry.Register<IHubConnectionService, HubConnectionService>();
            ///Page
            containerRegistry.RegisterForNavigation<AssignedReceivablesPage, AssignedReceivablesPageViewModel>();
            containerRegistry.RegisterForNavigation<LoginPage, LoginPageViewModel>();
            containerRegistry.RegisterForNavigation<MainPage, MainPageViewModel>();

            containerRegistry.RegisterForNavigation<NotificationListPage, NotificationListPageViewModel>();
            containerRegistry.RegisterForNavigation<NotificationPage, NotificationPageViewModel>();

            containerRegistry.RegisterForNavigation<ReceivableDetailPage, ReceivableDetailPageViewModel>();
            containerRegistry.RegisterForNavigation<ReceivableListPage, ReceivableListPageViewModel>();
            containerRegistry.RegisterForNavigation<ReceivableTaskListPage, ReceivableTaskListPageViewModel>();
            containerRegistry.RegisterForNavigation<TaskDetailPage, TaskDetailPageViewModel>();

        }
    }

}
