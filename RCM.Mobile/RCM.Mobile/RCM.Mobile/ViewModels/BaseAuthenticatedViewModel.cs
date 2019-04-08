using Prism.Navigation;
using Plugin.Connectivity;

using Prism.Services;
using RCM.Mobile.Models;
using RCM.Mobile.Services;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace RCM.Mobile.ViewModels
{
    public class BaseAuthenticatedViewModel : ViewModelBase
    {
        //private readonly HubConnection _connection;
        protected IPageDialogService _dialogService;
        protected ISettingsService _settingsService;
        private bool _networkConntection;
        public bool NetworkConntection
        {
            get { return _networkConntection; }
            set { SetProperty(ref _networkConntection, value); }
        }
        //private void CheckWifiOnStart()
        //{
        //    NetworkConntection = CrossConnectivity.Current.IsConnected;
        //    if (!NetworkConntection)
        //    {

        //    }
        //}

        //private void CheckWifiContinously()
        //{
        //    throw new NotImplementedException();
        //}

        public BaseAuthenticatedViewModel(ISettingsService settingsService, IPageDialogService dialogService, INavigationService navigationService) : base(navigationService)
        {
            _settingsService = settingsService;
            _dialogService = dialogService;
            //CheckWifiOnStart();
            //CheckWifiContinously();
            //_connection = new HubConnectionBuilder()
            //.WithUrl(GlobalSetting.DefaultUri + "/centerHub?access_token=" + _settingsService.AuthAccessToken)
            //.Build();

            //_connection.On<Notification>("Notify", async (notification) =>
            //{
            //    await _dialogService.DisplayAlertAsync($"{ notification.Title}", $"{notification.Body}", "OK");
            //});
            //_connection.StartAsync();
        }
    }
}
