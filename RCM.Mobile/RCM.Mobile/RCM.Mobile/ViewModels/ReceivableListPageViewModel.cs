using Prism.Navigation;
using Prism.Services;
using RCM.Mobile.Models;
using RCM.Mobile.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading.Tasks;

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
            Title = "Receivables";

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
            base.OnNavigatedTo(parameters);
            await InitAsync();
        }
        private async System.Threading.Tasks.Task InitAsync()
        {
            //Task.FromResult(await _receivableService.GetReceivablesAsync(_settingsService.AuthAccessToken));
            var collection = await _receivableService.GetAssignedReceivableAsync(_settingsService.AuthAccessToken);
            Receivables = new ObservableCollection<Receivable>();
            foreach (var item in collection)
            {
                Receivables.Add(item);
            }
        }

    }
}
