using Microsoft.AspNetCore.SignalR.Client;
using Prism.Services;
using RCM.Mobile.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace RCM.Mobile.Services
{
    //public class HubConnectionService : IHubConnectionService
    //{
    //    private readonly HubConnection _connection;
    //    private IPageDialogService _dialogService;
    //    private ISettingsService _settingsService;

    //    public HubConnectionService(ISettingsService settingsService, IPageDialogService dialogService)
    //    {
    //        _settingsService = settingsService;
    //        _dialogService = dialogService;
    //        _connection = new HubConnectionBuilder()
    //        .WithUrl(GlobalSetting.DefaultUri + "/centerHub?access_token=" + _settingsService.AuthAccessToken)
    //        .Build();
       
    //        _connection.On<Notification>("Notify", async (notification) =>
    //        {
    //            await _dialogService.DisplayAlertAsync($"{ notification.Title}", $"{notification.Body}", "OK");
    //        });
    //        _connection.StartAsync();
    //    }
    //    //public async Task<bool> StartAsync()
    //    //{
    //    //    await _connection.StartAsync();
    //    //    if (_connection.State == HubConnectionState.Connected)
    //    //    {
    //    //        return true;
    //    //    }
    //    //    return false;
    //    //}
    //    public void TurnOnNotificationService()
    //    {
    //        _dialogService.DisplayAlertAsync($"Notification","Turn on", "OK");
           
    //    }
    //}
    //public interface IHubConnectionService
    //{
    //    void TurnOnNotificationService();
    //}
}
