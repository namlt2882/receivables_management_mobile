using Newtonsoft.Json.Linq;
using RCM.Mobile.Helpers;
using RCM.Mobile.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace RCM.Mobile.Services
{
    public interface IUtilityService
    {
        //Task<CustomerBasket> GetBasketAsync(string guidUser, string token);
        //Task<CustomerBasket> UpdateBasketAsync(CustomerBasket customerBasket, string token);
        //Task CheckoutAsync(BasketCheckout basketCheckout, string token);
        //Task ClearBasketAsync(string guidUser, string token);
        Task<string> GetServerTime(string token);
    }
    public class UtilityService : IUtilityService
    {
        private const string ApiUrlBase = "/Utility";
        private readonly IRequestProvider _requestProvider;
        private readonly ISettingsService _settingsService;

        public UtilityService(IRequestProvider requestProvider, ISettingsService settingsService)
        {
            _requestProvider = requestProvider;
            _settingsService = settingsService;

        }

        public async Task<string> GetServerTime(string token)
        {
            var uri = UriHelper.CombineUri(_settingsService.EndPoint(), ApiUrlBase+ "/GetServerDay");
            return await _requestProvider.GetAsync<string>(uri, token);
        }
    }
}
