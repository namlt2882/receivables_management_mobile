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
        public MainPageViewModel(INavigationService navigationService, ISettingsService settingsService, IPageDialogService dialogService, IFirebaseTokenService firebaseTokenService)
            : base(settingsService, dialogService, navigationService)
        {
            _firebaseTokenService = firebaseTokenService;
            Title = "";
            this.Menus = new ObservableCollection<string>()
            {
                "Receivable",
                "Tasks",
                //"Notification",
                "Setting",
                "Something funny",
                "Logout",

            };
            this.AccountName = settingsService.AuthUserName;
        }

        private string _hasNewNotification;
        public string HasNewNotification
        {
            get { return _hasNewNotification; }
            set { SetProperty(ref _hasNewNotification, value); }
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
            //if (parameters.ContainsKey("PreviousPage"))
            //{
            //    var previousPage = parameters["PreviousPage"] as string;
            //    if (String.IsNullOrEmpty(previousPage))
            //        if (previousPage.Equals(Constant.Login))
            //        {
            //            var mainPage = Application.Current.MainPage;
            //            mainPage.Navigation.RemovePage(
            //                            mainPage.Navigation.NavigationStack[0]);
            //        }
            //}

            //NavigationPage.SetHasNavigationBar(Application.Current.MainPage, false);
            base.OnNavigatedTo(parameters);
        }
        private async System.Threading.Tasks.Task OnMenuChangedAsync()
        {
            switch (SelectedMenu)
            {
                //case "Notification":
                //    await NavigationService.NavigateAsync("NavigationPage/NotificationListPage");
                //    break;
                case "Receivable":
                    await NavigationService.NavigateAsync("NavigationPage/ReceivableListPage");
                    break;
                case "Logout":
                    //await _firebaseTokenService.DeleteFirebaseToken(_settingsService.AuthAccessToken);
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
        //public Command Receivable
        //{
        //    get
        //    {
        //        return new Command(async () =>
        //        {
        //            await NavigationService.NavigateAsync("ReceivableListPage");
        //        });
        //    }
        //}
    }
}
