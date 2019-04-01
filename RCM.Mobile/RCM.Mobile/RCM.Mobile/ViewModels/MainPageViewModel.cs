using Plugin.FirebasePushNotification;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using Prism.Services;
using RCM.Mobile.Helpers;
using RCM.Mobile.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace RCM.Mobile.ViewModels
{
    public class MainPageViewModel : BaseAuthenticatedViewModel
    {
        private IFirebaseTokenService _firebaseTokenService;
        private ITaskService _taskService;
        private INotificationService _notificationService;
        private IUtilityService _utilityService;
        public MainPageViewModel(INavigationService navigationService, ISettingsService settingsService, IPageDialogService dialogService, IFirebaseTokenService firebaseTokenService, ITaskService taskService, INotificationService notificationService, IUtilityService utilityService)
            : base(settingsService, dialogService, navigationService)
        {
            _firebaseTokenService = firebaseTokenService;
            _taskService = taskService;
            _notificationService = notificationService;
            _utilityService = utilityService;
            Title = "";
            this.Menus = new ObservableCollection<string>()
            {
                "Receivable",
                "Task",
                "Setting",
                "Something funny",
                "Logout",
            };
            
            this.AccountName = settingsService.AuthUserName;
            
            Init();
        }

        private ObservableCollection<Models.Task> _tasks;
        public ObservableCollection<Models.Task> Tasks
        {
            get { return _tasks; }
            set { SetProperty(ref _tasks, value); RaisePropertyChanged("Tasks"); }
        }

        private bool _hasNewNotification;
        public bool HasNewNotification
        {
            get { return _hasNewNotification; }
            set { SetProperty(ref _hasNewNotification, value); RaisePropertyChanged("HasNewNotification"); }
        }
        private bool _notHasNewNotification;
        public bool NotHasNewNotification
        {
            get { return _notHasNewNotification; }
            set { SetProperty(ref _notHasNewNotification, value); RaisePropertyChanged("NotHasNewNotification"); }
        }

        private string _accountName;
        public string AccountName
        {
            get { return _accountName; }
            set { SetProperty(ref _accountName, value); }
        }
        public ObservableCollection<string> Menus { get; private set; }
        private string selectedMenu;

        public string SelectedMenu
        {
            get
            {
                return this.selectedMenu;
            }
            set
            {
                if (this.selectedMenu != value)
                {
                    this.selectedMenu = value;
                    this.RaisePropertyChanged();
                    this.OnMenuChangedAsync();
                }
            }
        }

        public override void OnNavigatedTo(INavigationParameters parameters)
        {
            //var collection = await _taskService.GetCollectorAssignedTasks(_settingsService.AuthAccessToken);
            //Tasks = new ObservableCollection<Models.Task>();
            //foreach (var item in collection)
            //{
            //    Tasks.Add(item);
            //}
            Init();
            base.OnNavigatedTo(parameters);
        }
        private async void Init()
        {
            HasNewNotification = await _notificationService.HasNotifications(token: _settingsService.AuthAccessToken);
            NotHasNewNotification = !HasNewNotification;
            _settingsService.ServerDay = await _utilityService.GetServerTime(_settingsService.AuthAccessToken);
            await _firebaseTokenService.UpdateFirebaseToken(_settingsService.AuthAccessToken, CrossFirebasePushNotification.Current.Token);
        } 
        private async System.Threading.Tasks.Task OnMenuChangedAsync()
        {
            switch (SelectedMenu)
            {
                case "Task":
                    await NavigationService.NavigateAsync("NavigationPage/TaskPage");
                    break;
                case "Calendar":
                    await NavigationService.NavigateAsync("NavigationPage/Calendar");
                    break;
                case "Receivable":
                    await NavigationService.NavigateAsync("NavigationPage/ReceivableListPage");
                    break;
                case "Logout":
                    await _firebaseTokenService.DeleteFirebaseToken(_settingsService.AuthAccessToken);
                    _settingsService.AccessTokenExpirationDate = DateTime.UtcNow;
                    _settingsService.AuthAccessToken = "";
                    //CrossFirebasePushNotification.Current.Unsubscribe(_settingsService.AuthUserName);
                    await NavigationService.NavigateAsync("RCM.Mobile:///LoginPage");
                    break;
            }
        }
        public Command Notification
        {
            get
            {
                return new Command(async () =>
                {
                    await NavigationService.NavigateAsync("NavigationPage/NotificationListPage");
                });
            }
        }
    }
}
