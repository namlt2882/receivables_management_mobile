using Prism.Navigation;
using Prism.Services;
using RCM.Mobile.Helpers;
using RCM.Mobile.Models;
using RCM.Mobile.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Xamarin.Forms;
namespace RCM.Mobile.ViewModels
{
    public class ReceivableDetailPageViewModel : BaseAuthenticatedViewModel
    {

        public IReceivableService _receivableService;
        public ITaskService _taskService;

        public ReceivableDetailPageViewModel(
            ISettingsService settingsService,
            IPageDialogService dialogService,
            INavigationService navigationService,
            IReceivableService receivableService,
            ITaskService taskService
            ) : base(settingsService, dialogService, navigationService)
        {
            _receivableService = receivableService;
            _taskService = taskService;
            Title = "Receivable Detail";
            HasTask = false;
        }

        private List<Contact> _relatives;
        public List<Contact> Relatives
        {
            get { return _relatives; }
            set { SetProperty(ref _relatives, value); RaisePropertyChanged("Relatives"); }
        }

        private Receivable _receivable;
        public Receivable Receivable
        {
            get { return _receivable; }
            set { SetProperty(ref _receivable, value); RaisePropertyChanged("Receivable"); }
        }
        private bool _hasTask;
        public bool HasTask
        {
            get { return _hasTask; }
            set { SetProperty(ref _hasTask, value); RaisePropertyChanged("HasTask"); }
        }
        //public ObservableCollection<Receivable> Receivables { get; set; }
        public override async void OnNavigatedTo(INavigationParameters parameters)
        {
            HasTask = false;
            if (parameters.ContainsKey("receivableId"))
            {
                var receivableId = parameters.GetValue<int>("receivableId");
                Receivable = await _receivableService.GetAssignedReceivableAsync(_settingsService.AuthAccessToken, receivableId);
                _settingsService.ReceivableId = Receivable.Id;
            }
            //Default Current Day set is 0
            var tasks = await _taskService.GetAssignedTaskByReceivableAndDay(_settingsService.AuthAccessToken, 0, _settingsService.ReceivableId);
            if (tasks.Count > 0)
            {
                HasTask = true;
            }
            //Get Relatives
            Relatives = new List<Contact>();
            Receivable.Contacts.Skip(1).ToList().ForEach(c=>
            {
                Relatives.Add(c);
            });
            base.OnNavigatedTo(parameters);
        }

        //public override void OnNavigatedFrom(INavigationParameters parameters)
        //{
        //    base.OnNavigatedFrom(parameters);
        //}

        public Command<bool> UpdateReceivableStatus
        {
            get
            {
                return new Command<bool>(async (Finish) =>
                {
                    //await _receivableService.CloseReceivable(_settingsService.AuthAccessToken, new ReceivableCloseModel()
                    //{
                    //    Id = Receivable.Id,
                    //    isPayed = Finish
                    //});
                    if (Finish)
                    {
                        Receivable.CollectionProgressStatus = Constant.COLLECTION_STATUS_CLOSED_CODE;
                    }
                    else
                    {
                        Receivable.CollectionProgressStatus = Constant.COLLECTION_STATUS_CANCEL_CODE;
                    }
                });
            }
        }

        public Command PhoneCommand
        {
            get
            {
                return new Command(() =>
                {
                    Plugin.Messaging.CrossMessaging.Current.PhoneDialer.MakePhoneCall(Receivable.Contacts[0].Phone, Receivable.Contacts[0].Name);
                });
            }
        }
        public Command MessagingCommand
        {
            get
            {
                return new Command(() =>
                {
                    Plugin.Messaging.CrossMessaging.Current.SmsMessenger.SendSms(Receivable.Contacts[0].Phone, "");
                });
            }
        }
        public Command DirectionCommand
        {
            get
            {
                return new Command(() =>
                {
                    var uri = new Uri($"http://maps.google.com/maps?daddr={Receivable.Contacts[0].Address}&directionsmode=transit");
                    Device.OpenUri(uri);

                });
            }
        }

        public Command<string> PhoneRelativeCommand
        {
            get
            {
                return new Command<string>((phone) =>
                {
                    Plugin.Messaging.CrossMessaging.Current.PhoneDialer.MakePhoneCall(phone, "");
                });
            }
        }
        public Command<string> MessagingRelativeCommand
        {
            get
            {
                return new Command<string>((phone) =>
                {
                    Plugin.Messaging.CrossMessaging.Current.SmsMessenger.SendSms(phone, "");
                });
            }
        }
        public Command<string> DirectionRelativeCommand
        {
            get
            {
                return new Command<string>((address) =>
                {
                    var uri = new Uri($"http://maps.google.com/maps?daddr={address}&directionsmode=transit");
                    Device.OpenUri(uri);

                });
            }
        }
        public Command Edit
        {
            get
            {
                return new Command(async () =>
                {
                    if (Receivable.CollectionProgressStatus != Constant.COLLECTION_STATUS_COLLECTION_CODE)
                    {
                        await _dialogService.DisplayActionSheetAsync("Update Receivable Status", "OK", null, null);
                    }
                    else
                    {
                        var answer = await _dialogService.DisplayActionSheetAsync("Update Receivable Status", "Cancel", null, "Finish", "Stop");
                        switch (answer)
                        {
                            case "Finish":
                                await _receivableService.CloseReceivable(_settingsService.AuthAccessToken, new ReceivableCloseModel()
                                {
                                    Id = Receivable.Id,
                                    isPayed = true
                                });
                                Receivable.CollectionProgressStatus = Constant.COLLECTION_STATUS_CLOSED_CODE;
                                break;
                            case "Stop":
                                await _receivableService.CloseReceivable(_settingsService.AuthAccessToken, new ReceivableCloseModel()
                                {
                                    Id = Receivable.Id,
                                    isPayed = false
                                });
                                Receivable.CollectionProgressStatus = Constant.COLLECTION_STATUS_CANCEL_CODE;
                                break;
                        }
                    }
                });
            }
        }
        public Command TapToDayTasksCommand
        {
            get
            {
                return new Command(async () =>
                {
                    await NavigationService.NavigateAsync("ReceivableTaskListPage");
                });
            }
        }
    }
}
