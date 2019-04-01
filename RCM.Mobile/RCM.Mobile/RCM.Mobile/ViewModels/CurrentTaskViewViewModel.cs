using System;
using System.Collections.Generic;
using System.Text;
using Prism.Navigation;
using Prism.Services;
using RCM.Mobile.Services;

namespace RCM.Mobile.ViewModels
{
    public class CurrentTaskViewViewModel : BaseAuthenticatedViewModel
    {
        public CurrentTaskViewViewModel(ISettingsService settingsService, IPageDialogService dialogService, INavigationService navigationService) : base(settingsService, dialogService, navigationService)
        {
        }
    }
}
