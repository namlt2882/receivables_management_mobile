using System;
using System.Collections.Generic;
using System.Text;
using Prism.Navigation;
using Prism.Services;
using RCM.Mobile.Services;
namespace RCM.Mobile.ViewModels
{
    public class SettingViewModel : BaseAuthenticatedViewModel
    {
        public SettingViewModel(INavigationService navigationService, ISettingsService settingsService, IPageDialogService dialogService)
            : base(settingsService, dialogService, navigationService)
        {
            Title = "Setting Page";
        }

    }
}
