using Prism.Navigation;
using Prism.Services;
using RCM.Mobile.Helpers;
using RCM.Mobile.Models;
using RCM.Mobile.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading.Tasks;
using Telerik.XamarinForms.DataControls.ListView.Commands;
using Xamarin.Forms;

namespace RCM.Mobile.ViewModels
{

    public class ReceivableListPageViewModel : BaseAuthenticatedViewModel
    {
        private IReceivableService _receivableService;
        private IContactService _contactService;
        public ReceivableListPageViewModel(
            ISettingsService settingsService,
            IPageDialogService dialogService,
            INavigationService navigationService,
            IContactService contactService,
            IReceivableService receivableService
            ) : base(settingsService, dialogService, navigationService)
        {
            _receivableService = receivableService;
            _contactService = contactService;
            Title = "Receivable List";
        }

        private bool isBusy;
        public bool IsBusy
        {
            get { return isBusy; }
            set { SetProperty(ref isBusy, value); RaisePropertyChanged("IsBusy"); }
        }
        private ObservableCollection<Receivable> _receivables;
        public ObservableCollection<Receivable> Receivables
        {
            get { return _receivables; }
            set { SetProperty(ref _receivables, value); RaisePropertyChanged("Receivables"); }
        }
        private ObservableCollection<Contact> _debtors;
        public ObservableCollection<Contact> Debtors
        {
            get { return _debtors; }
            set { SetProperty(ref _debtors, value); }
        }

        private List<Status> _status;
        public List<Status> Status
        {
            get { return _status; }
            set { SetProperty(ref _status, value); RaisePropertyChanged("Status"); }
        }

        private ObservableCollection<Receivable> _receivableHistories;
        public ObservableCollection<Receivable> ReceivableHistories
        {
            get { return _receivableHistories; }
            set { SetProperty(ref _receivableHistories, value); RaisePropertyChanged("ReceivableHistories"); }
        }
        //public ObservableCollection<Receivable> Receivables { get; set; }
        public override async void OnNavigatedTo(INavigationParameters parameters)
        {
            IsBusy = true;
            Status = Constant.STATUSES;
            Receivables = new ObservableCollection<Receivable>();
            foreach (var item in await _receivableService.GetAssignedReceivablesAsync(_settingsService.AuthAccessToken, false))
            {
                Receivables.Add(item);
            }
            ReceivableHistories = new ObservableCollection<Receivable>();
            foreach (var item in await _receivableService.GetAssignedReceivablesAsync(_settingsService.AuthAccessToken, true))
            {
                ReceivableHistories.Add(item);
            }
            IsBusy = false;
            base.OnNavigatedTo(parameters);
        }

        public async System.Threading.Tasks.Task InitDebtorsAsync(string name)
        {
            Debtors = new ObservableCollection<Contact>();

            List<Contact> contacts = await _contactService.GetDebtors(_settingsService.AuthAccessToken, name);
            if (contacts.Count > 0)
            {
                foreach (var item in contacts)
                {
                    Debtors.Add(item);
                }
            }
        }
        public Command TapReceivable
        {
            get
            {
                return new Command<ItemTapCommandContext>(async (_) =>
                {
                    var receivable = _.Item as Receivable;
                    var navigationParams = new NavigationParameters
                    {
                        { "receivableId", receivable.Id }
                    };
                    await NavigationService.NavigateAsync("ReceivableDetailPage", navigationParams);
                });
            }
        }
    }
}
