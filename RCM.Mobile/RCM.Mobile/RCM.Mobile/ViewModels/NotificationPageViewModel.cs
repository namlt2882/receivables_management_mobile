using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using Prism.Navigation;
using Prism.Services;
using RCM.Mobile.Models;
using RCM.Mobile.Services;
namespace RCM.Mobile.ViewModels
{

    public class NotificationPageViewModel : BaseAuthenticatedViewModel
    {
        private readonly INotificationService _notificationService;
        

        public NotificationPageViewModel(
            INavigationService navigationService, 
            ISettingsService settingsService, 
            IPageDialogService dialogService,
            INotificationService notificationService)
            : base(settingsService, dialogService, navigationService)
        {
            Title = "Notification Page";
        }
        private Notification _notification;
        public Notification Notification
        {
            get => _notification;
            set
            {
                _notification = value;
                RaisePropertyChanged("Notification");
            }
        }
       

    }
}
