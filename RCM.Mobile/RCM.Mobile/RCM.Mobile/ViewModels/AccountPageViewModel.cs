using System;
using Prism.Navigation;
using Prism.Services;
using RCM.Mobile.Helpers;
using RCM.Mobile.Models;
using RCM.Mobile.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using Telerik.XamarinForms.DataControls.ListView.Commands;
using Xamarin.Forms;

namespace RCM.Mobile.ViewModels
{
    public class AccountPageViewModel : BaseAuthenticatedViewModel
    {
        private IFirebaseTokenService _firebaseTokenService;

        public AccountPageViewModel(
            ISettingsService settingsService,
            IPageDialogService dialogService,
            IFirebaseTokenService firebaseTokenService,
            INavigationService navigationService,
            IReceivableService receivableService
            ) : base(settingsService, dialogService, navigationService)
        {
            _firebaseTokenService = firebaseTokenService;
            Title = "Account";
            this.AccountName = settingsService.AuthUserName;
        }
        private string _accountName;
        public string AccountName
        {
            get { return _accountName; }
            set { SetProperty(ref _accountName, value); }
        }
        private string _ip;
        public string IP
        {
            get { return _ip; }
            set { SetProperty(ref _ip, value); }
        }

        public Command ChangeIP
        {
            get
            {
                return new Command(async () =>
               {
                   if (await _dialogService.DisplayAlertAsync("Message", "Your want to change IP?", "Ok", "Cancel"))
                       _settingsService.IPAddress = IP;
               });
            }
        }
        public Command Logout
        {
            get
            {
                return new Command(async () =>
                {
                    await _firebaseTokenService.DeleteFirebaseToken(_settingsService.AuthAccessToken);
                    _settingsService.AccessTokenExpirationDate = DateTime.UtcNow;
                    _settingsService.AuthAccessToken = "";
                    //CrossFirebasePushNotification.Current.Unsubscribe(_settingsService.AuthUserName);
                    await NavigationService.NavigateAsync("RCM.Mobile:///LoginPage");
                });
            }
        }
    }
}
