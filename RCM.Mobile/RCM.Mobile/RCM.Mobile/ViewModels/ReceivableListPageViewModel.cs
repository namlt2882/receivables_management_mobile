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
            Title = "Your receivable";

        }

        private ObservableCollection<Receivable> _receivables;
        public ObservableCollection<Receivable> Receivables
        {
            get { return _receivables; }
            set { SetProperty(ref _receivables, value); RaisePropertyChanged("Receivables"); }
        }
        //public ObservableCollection<Receivable> Receivables { get; set; }
        public override async void OnNavigatedTo(INavigationParameters parameters)
        {
            var collection  = new ObservableCollection<Receivable>();
            if (parameters.Count > 0)
            {
                var receivableIdList = parameters.GetValue<List<int>>("receivableIdList");
                collection = await _receivableService.GetAssignedReceivablesAsync(_settingsService.AuthAccessToken, receivableIdList);
            }
            else
            {
                collection = await _receivableService.GetAssignedReceivablesAsync(_settingsService.AuthAccessToken, new List<int>());
            }
            Receivables = new ObservableCollection<Receivable>();
            foreach (var item in collection)
            {
                Receivables.Add(item);
            }
            base.OnNavigatedTo(parameters);
        }

        public Command TapReceivable
        {
            get
            {
                return new Command<ItemTapCommandContext>(async (_) =>
                {
                    var item = _.Item as Receivable;
                    var receivable = Receivables[Receivables.IndexOf(item)];
                    var navigationParams = new NavigationParameters();
                    navigationParams.Add("receivableId", receivable.Id);
                    await NavigationService.NavigateAsync("ReceivableDetailPage", navigationParams);
                });
            }
        }
    }
}
