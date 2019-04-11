using Plugin.Connectivity;
using Plugin.FirebasePushNotification;
using Prism.Navigation;
using Prism.Services;
using RCM.Mobile.Helpers;
using RCM.Mobile.Models;
using RCM.Mobile.Services;
using RCM.Mobile.Views;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace RCM.Mobile.ViewModels
{

    public class LoginPageViewModel : ViewModelBase
    {
        private string username;
        private string password;
        private readonly IAuthService _authService;
        private readonly ISettingsService _settingsService;
        private IPageDialogService _dialogService;
        private IFirebaseTokenService _firebaseTokenService;
        public LoginPageViewModel(INavigationService navigationService,
            IAuthService authService,
            ISettingsService settingsService,
            IPageDialogService dialogService,
            IFirebaseTokenService firebaseTokenService)
            : base(navigationService)
        {
            _authService = authService;
            _settingsService = settingsService;
            _dialogService = dialogService;
            _firebaseTokenService = firebaseTokenService;
            //Title = "Login Page";
        }
        public string UserName
        {
            get => this.username;
            set
            {
                if (this.username == value)
                {
                    return;
                }

                this.username = value;
                RaisePropertyChanged();
            }
        }

        public string Password
        {
            get => this.password;
            set
            {
                if (this.password == value)
                {
                    return;
                }

                this.password = value;
                RaisePropertyChanged();
            }
        }
        public override void OnNavigatedTo(INavigationParameters parameters)
        {
            base.OnNavigatedTo(parameters);
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
        public Command Login
        {
            get
            {
                return new Command(async () =>
                {
                    if (CrossConnectivity.Current.IsConnected)
                    {

                        var jwtDynamic = await _authService.Login(new AuthModel { UserName = UserName, Password = Password });
                        //Count > 1 =>Login Success!
                        if (jwtDynamic.Count > 1)
                        {
                            _settingsService.AuthAccessToken = jwtDynamic.Value<string>("access_token");
                            _settingsService.AuthUserName = jwtDynamic.Value<string>("username");
                            _settingsService.AccessTokenExpirationDate = DateTime.Now.AddDays(99);
                            var a = await _firebaseTokenService.AddFirebaseToken(_settingsService.AuthAccessToken, CrossFirebasePushNotification.Current.Token);
                            var mainPage = Application.Current.MainPage;
                            if (mainPage != null)
                            {
                                await NavigationService.NavigateAsync("RCM.Mobile:///MainPage");
                            }
                        }
                        else
                        {
                            await _dialogService.DisplayAlertAsync("Login fail", "Invalid username or password", "OK");
                        }
                    }
                    else
                    {
                        await _dialogService.DisplayAlertAsync("No internet", "Please check your network connection!", "OK");
                    }

                });
            }
        }
    }
}
