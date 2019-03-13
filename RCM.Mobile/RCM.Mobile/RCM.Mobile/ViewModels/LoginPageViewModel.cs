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

                                //CrossFirebasePushNotification.Current.Subscribe(_settingsService.AuthUserName);

                                await NavigationService.NavigateAsync("RCM.Mobile:///MainPage");

                                //await NavigationService.NavigateAsync("myapp:///NavigationPage/MainPage");
                                /////////////// PROBLEM NEED TO SOLVE
                                //NavigationPage.SetHasNavigationBar(mainPage, false);
                                ////mainPage = new NavigationPage(new MainPage());
                                ////if (mainPage.Navigation.NavigationStack.Count == 1)
                                ////{
                                ////    mainPage.Navigation.RemovePage(
                                ////    mainPage.Navigation.NavigationStack[mainPage.Navigation.NavigationStack.Count - 1]);
                                ////}
                                ////await NavigationService.GoBackToRootAsync();
                                //var navigationParams = new NavigationParameters();
                                //navigationParams.Add("PreviousPage", Constant.Login);

                                //await NavigationService.NavigateAsync("NavigationPage/MainPage", navigationParams);
                                //mainPage.Navigation.RemovePage(
                                //    mainPage.Navigation.NavigationStack[0]);
                            }
                        }
                        else
                        {
                            await _dialogService.DisplayAlertAsync("Login fail", "Invalid username or password", "OK");
                        }
                        //_settingsService.AccessTokenExpirationDate = DateTime.UtcNow.AddMinutes(double.Parse(jwtDynamic.Value<string>("expires_in")));
                        //var mainPage = Application.Current.MainPage as MainPage;
                        //if (mainPage != null)
                        //{
                        //    mainPage.Navigation.RemovePage(
                        //        mainPage.Navigation.NavigationStack[mainPage.Navigation.NavigationStack.Count - 2]);
                        //}
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
