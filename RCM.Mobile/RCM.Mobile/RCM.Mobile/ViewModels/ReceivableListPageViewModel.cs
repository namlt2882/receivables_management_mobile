using Prism.Navigation;
using Prism.Services;
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

        public ReceivableListPageViewModel(
            ISettingsService settingsService,
            IPageDialogService dialogService,
            INavigationService navigationService,
            IReceivableService receivableService
            ) : base(settingsService, dialogService, navigationService)
        {
            _receivableService = receivableService;
            Title = "Receivable List";
        }

        private ObservableCollection<Receivable> _receivables;
        public ObservableCollection<Receivable> Receivables
        {
            get { return _receivables; }
            set { SetProperty(ref _receivables, value); RaisePropertyChanged("Receivables"); }
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
            Receivables = new ObservableCollection<Receivable>();
            foreach (var item in await _receivableService.GetAssignedReceivablesAsync(_settingsService.AuthAccessToken,  false))
            {
                Receivables.Add(item);
            }
            ReceivableHistories = new ObservableCollection<Receivable>();
            foreach (var item in await _receivableService.GetAssignedReceivablesAsync(_settingsService.AuthAccessToken,  true))
            {
                ReceivableHistories.Add(item);
            }
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
