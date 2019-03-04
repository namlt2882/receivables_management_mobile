using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using Prism.Services;
using RCM.Mobile.Services;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace RCM.Mobile.ViewModels
{
    public class MainPageViewModel : BaseAuthenticatedViewModel
    {
        public MainPageViewModel(INavigationService navigationService, ISettingsService settingsService, IPageDialogService dialogService)
            : base(settingsService, dialogService, navigationService)
        {
            Title = "Main Page";
        }
        public Command Noti
        {
            get
            {
                return new Command(async () =>
                {
                    await NavigationService.NavigateAsync("NotificationListPage");
                });
            }
        }
    }
}
