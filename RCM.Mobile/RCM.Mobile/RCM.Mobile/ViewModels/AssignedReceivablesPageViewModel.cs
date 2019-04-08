using Prism.Navigation;
using Prism.Services;
using RCM.Mobile.Helpers;
using RCM.Mobile.Models;
using RCM.Mobile.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using Telerik.XamarinForms.DataControls.ListView.Commands;
using Xamarin.Forms;

namespace RCM.Mobile.ViewModels
{
    public class AssignedReceivablesPageViewModel : BaseAuthenticatedViewModel
    {
        private IReceivableService _receivableService;

        public AssignedReceivablesPageViewModel(
            ISettingsService settingsService,
            IPageDialogService dialogService,
            INavigationService navigationService,
            IReceivableService receivableService
            ) : base(settingsService, dialogService, navigationService)
        {
            _receivableService = receivableService;
            Title = "Assigned Receivables";

        }
        private List<Status> _status;
        public List<Status> Status
        {
            get { return _status; }
            set { SetProperty(ref _status, value); RaisePropertyChanged("Status"); }
        }
        private List<int> _receivableIdList;
        public List<int> ReceivableIdList
        {
            get { return _receivableIdList; }
            set { SetProperty(ref _receivableIdList, value); RaisePropertyChanged("ReceivableIdList"); }
        }
        private ObservableCollection<Receivable> _receivables;
        public ObservableCollection<Receivable> Receivables
        {
            get { return _receivables; }
            set { SetProperty(ref _receivables, value); RaisePropertyChanged("Receivables"); }
        }
        public override async void OnNavigatedTo(INavigationParameters parameters)
        {
            List<int> receivableIdList;
            if (parameters.ContainsKey("receivableIdList"))
            {
                receivableIdList = parameters.GetValue<List<int>>("receivableIdList");
                ReceivableIdList = receivableIdList;
            }
            var collection = await _receivableService.GetAssignedReceivablesAsync(_settingsService.AuthAccessToken, ReceivableIdList);
            Receivables = new ObservableCollection<Receivable>();
            foreach (var item in collection)
            {
                Receivables.Add(item);
            }
            Status = Constant.STATUSES;
            base.OnNavigatedTo(parameters);
        }
        public Command TapReceivable
        {
            get
            {
                return new Command<ItemTapCommandContext>(async (_) =>
                {
                    var receivable = _.Item as Receivable;
                    var navigationParams = new NavigationParameters();
                    navigationParams.Add("receivableId", receivable.Id);
                    await NavigationService.NavigateAsync("ReceivableDetailPage", navigationParams);
                });
            }
        }
    }
}
