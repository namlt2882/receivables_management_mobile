using RCM.Mobile.Helpers;
using RCM.Mobile.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading.Tasks;

namespace RCM.Mobile.Services
{
    public interface INotificationService
    {
        //Task<CustomerBasket> GetBasketAsync(string guidUser, string token);
        //Task<CustomerBasket> UpdateBasketAsync(CustomerBasket customerBasket, string token);
        //Task CheckoutAsync(BasketCheckout basketCheckout, string token);
        //Task ClearBasketAsync(string guidUser, string token);
        Task<ObservableCollection<Notification>> GetNotificationsAsync(string token);
        Task<bool> HasNotifications(string token);
        Task<Notification> GetNotificationByIdAsync(int id, string token);
        Task<bool> ToggleSeen(int id, string token);
        Task<bool> DeleteAsync(int id, string token);
    }

    public class NotificationService : INotificationService
    {
        private const string ApiUrlBase = "/Notification";
        private readonly IRequestProvider _requestProvider;
        private readonly ISettingsService _settingsService;

        public NotificationService(IRequestProvider requestProvider)
        {
            _requestProvider = requestProvider;
        }

        public async Task<bool> DeleteAsync(int id, string token)
        {
            var uri = UriHelper.CombineUri(_settingsService.EndPoint(), ApiUrlBase + $"?id={id}");
            try
            {
                await _requestProvider.DeleteAsync(uri, token);
            }
            //If the status of the notification we will get
            //a BadRequest HttpStatus
            catch (HttpRequestExceptionEx ex) when (ex.HttpCode == System.Net.HttpStatusCode.BadRequest)
            {
                return false;
            }
            return true;
        }

        public async Task<Notification> GetNotificationByIdAsync(int id, string token)
        {
            var uri = UriHelper.CombineUri(_settingsService.EndPoint(), ApiUrlBase + $"/{id}");
            return await _requestProvider.GetAsync<Notification>(uri, token);
        }

        public async Task<ObservableCollection<Notification>> GetNotificationsAsync(string token)
        {
            var uri = UriHelper.CombineUri(_settingsService.EndPoint(), ApiUrlBase);
            return await _requestProvider.GetAsync<ObservableCollection<Notification>>(uri, token);
        }

        public async Task<bool> HasNotifications(string token)
        {
            var uri = UriHelper.CombineUri(_settingsService.EndPoint(), ApiUrlBase+ "/HasNotifications");
            return await _requestProvider.GetAsync<bool>(uri, token);
        }

        public async Task<bool> ToggleSeen(int id, string token)
        {
            var uri = UriHelper.CombineUri(_settingsService.EndPoint(), ApiUrlBase + $"/ToggleSeen/{id}");
            try
            {
                await _requestProvider.PutAsync(uri, data: "", token: token);
            }
            //If the status of the notification we will get
            //a BadRequest HttpStatus
            catch (HttpRequestExceptionEx ex) when (ex.HttpCode == System.Net.HttpStatusCode.BadRequest)
            {
                return false;
            }
            return true;
        }
    }
}
