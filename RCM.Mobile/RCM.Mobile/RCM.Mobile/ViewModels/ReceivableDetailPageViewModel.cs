using Prism.Navigation;
using Prism.Services;
using RCM.Mobile.Helpers;
using RCM.Mobile.Models;
using RCM.Mobile.Services;
using Xamarin.Forms;

namespace RCM.Mobile.ViewModels
{
    public class ReceivableDetailPageViewModel : BaseAuthenticatedViewModel
    {

        public IReceivableService _receivableService;

        public ReceivableDetailPageViewModel(
            ISettingsService settingsService,
            IPageDialogService dialogService,
            INavigationService navigationService,
            IReceivableService receivableService
            ) : base(settingsService, dialogService, navigationService)
        {
            _receivableService = receivableService;
            Title = "Receivable";

        }
        private bool _fromNotification;

        private bool FromNotification
        {
            get { return _fromNotification; }
            set { SetProperty(ref _fromNotification, value); RaisePropertyChanged("FromNotification"); }
        }
        private Receivable _receivable;
        public Receivable Receivable
        {
            get { return _receivable; }
            set { SetProperty(ref _receivable, value); RaisePropertyChanged("Receivable"); }
        }
        //public ObservableCollection<Receivable> Receivables { get; set; }
        public override async void OnNavigatedTo(INavigationParameters parameters)
        {
            var receivableId = parameters.GetValue<int>("receivableId");
            Receivable = await _receivableService.GetAssignedReceivableAsync(_settingsService.AuthAccessToken, receivableId);
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
    }
}
