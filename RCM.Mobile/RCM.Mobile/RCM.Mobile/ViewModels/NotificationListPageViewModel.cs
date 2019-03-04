using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading.Tasks;
using Prism.Navigation;
using Prism.Services;
using RCM.Mobile.Models;
using RCM.Mobile.Services;

namespace RCM.Mobile.ViewModels
{
    public class NotificationListPageViewModel : BaseAuthenticatedViewModel
    {
        private INotificationService _notificationService;

        public NotificationListPageViewModel(
            ISettingsService settingsService,
            IPageDialogService dialogService,
            INavigationService navigationService,
            INotificationService notificationService
            ) : base(settingsService, dialogService, navigationService)
        {
            _notificationService = notificationService;
            Title = "Notifications";

        }

        private ObservableCollection<Notification> _notifications;
        public ObservableCollection<Notification> Notifications
        {
            get { return _notifications; }
            set { SetProperty(ref _notifications, value);RaisePropertyChanged("Notifications"); }
        }
        //public ObservableCollection<Notification> Notifications { get; set; }
        public override async void OnNavigatedTo(INavigationParameters parameters)
        {
            base.OnNavigatedTo(parameters);
             await InitAsync();
        }
        private async Task InitAsync()
        {
            //Task.FromResult(await _notificationService.GetNotificationsAsync(_settingsService.AuthAccessToken));
            var collection = await _notificationService.GetNotificationsAsync(_settingsService.AuthAccessToken);
            Notifications = new ObservableCollection<Notification>();
            foreach (var item in collection)
            {
                Notifications.Add(item);
            }
        }

    }
}

