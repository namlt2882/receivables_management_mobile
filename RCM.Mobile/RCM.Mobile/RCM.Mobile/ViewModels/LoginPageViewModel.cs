using Prism.Navigation;
using Prism.Services;
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

        public LoginPageViewModel(INavigationService navigationService, 
            IAuthService authService, 
            ISettingsService settingsService, 
            IPageDialogService dialogService)
            : base(navigationService)
        {
            _authService = authService;
            _settingsService = settingsService;
            _dialogService = dialogService;
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
        public Command Login
        {
            get
            {
                return new Command(async () =>
                {

                    var jwtDynamic = await _authService.Login(new AuthModel { UserName = UserName, Password = Password });
                    if (jwtDynamic.HasValues)
                    {
                        _settingsService.AuthAccessToken = jwtDynamic.Value<string>("access_token");
                        _settingsService.AuthUserName = jwtDynamic.Value<string>("username");
                        _settingsService.AccessTokenExpirationDate = DateTime.UtcNow.AddMinutes(95);

                        var mainPage = Application.Current.MainPage as NavigationPage;
                        if (mainPage != null)
                        {
                            //mainPage = new NavigationPage(new MainPage());
                            //if (mainPage.Navigation.NavigationStack.Count == 1)
                            //{
                            //    mainPage.Navigation.RemovePage(
                            //    mainPage.Navigation.NavigationStack[mainPage.Navigation.NavigationStack.Count - 1]);
                            //}
                            await NavigationService.GoBackToRootAsync();
                            await NavigationService.NavigateAsync("NavigationPage/MainPage");
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
                });
            }
        }
    }
}
