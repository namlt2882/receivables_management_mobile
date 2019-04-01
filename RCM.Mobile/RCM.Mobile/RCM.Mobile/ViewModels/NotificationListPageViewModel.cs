using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Prism.Navigation;
using Prism.Services;
using RCM.Mobile.Helpers;
using RCM.Mobile.Models;
using RCM.Mobile.Services;
using Telerik.XamarinForms.DataControls.ListView.Commands;
using Xamarin.Forms;

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
            set { SetProperty(ref _notifications, value); RaisePropertyChanged("Notifications"); }
        }
        //public ObservableCollection<Notification> Notifications { get; set; }
        public override async void OnNavigatedTo(INavigationParameters parameters)
        {
            base.OnNavigatedTo(parameters);
            await InitAsync();
        }

        public Command<Notification> ToggleNotification
        {
            get
            {
                return new Command<Notification>(async (noti) =>
                {
                    var notification = Notifications[Notifications.IndexOf(noti)];
                    notification.IsSeen = !notification.IsSeen;
                    await _notificationService.ToggleSeen(noti.Id, _settingsService.AuthAccessToken);
                    //await NavigationService.NavigateAsync("NavigationPage/NotificationListPage");
                });
            }
        }

        public Command TapNotification
        {
            get
            {
                return new Command<ItemTapCommandContext>(async (_) =>
                {
                    var item = _.Item as Notification;
                    var notification = Notifications[Notifications.IndexOf(item)];
                    if (!notification.IsSeen)
                    {
                        notification.IsSeen = !notification.IsSeen;
                        await _notificationService.ToggleSeen(item.Id, _settingsService.AuthAccessToken);
                    }
                    var navigationParams = new NavigationParameters();

                    switch (notification.Type)
                    {
                        case Constant.NOTIFICATION_TYPE_NEW_RECEIVABLE_CODE:
                            List<int> receivableIdList = JsonConvert.DeserializeObject<List<int>>(notification.NData);
                            navigationParams.Add("receivableIdList", receivableIdList);
                            await NavigationService.NavigateAsync("AssignedReceivablesPage", navigationParams);
                            break;
                        case Constant.NOTIFICATION_TYPE_ASSIGN_RECEIVABLE_CODE:
                            int receivableId = JsonConvert.DeserializeObject<int>(notification.NData);
                            navigationParams.Add("receivableIdList", new List<int>() { receivableId });
                            await NavigationService.NavigateAsync("AssignedReceivablesPage", navigationParams);
                            break;

                    }
                    //await NavigationService.NavigateAsync("NavigationPage/NotificationListPage");
                });
            }
        }
        public Command DeleteNotification
        {
            get
            {
                return new Command<Notification>(async (noti) =>
                {
                    if (await _notificationService.DeleteAsync(noti.Id, _settingsService.AuthAccessToken))
                    {
                        Notifications.Remove(noti);
                    }

                    //await NavigationService.NavigateAsync("NavigationPage/NotificationListPage");
                });
            }
        }
        private async System.Threading.Tasks.Task InitAsync()
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

