using Newtonsoft.Json.Linq;
using RCM.Mobile.Helpers;
using RCM.Mobile.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace RCM.Mobile.Services
{
    public interface IAuthService
    {
        //Task<CustomerBasket> GetBasketAsync(string guidUser, string token);
        //Task<CustomerBasket> UpdateBasketAsync(CustomerBasket customerBasket, string token);
        //Task CheckoutAsync(BasketCheckout basketCheckout, string token);
        //Task ClearBasketAsync(string guidUser, string token);
        Task<JObject> Login(AuthModel authModel);
    }
    public class AuthService : IAuthService
    {
        private const string ApiUrlBase = "/Auth";
        private readonly IRequestProvider _requestProvider;
        private readonly ISettingsService _settingsService;

        public AuthService(IRequestProvider requestProvider, ISettingsService settingsService)
        {
            _requestProvider = requestProvider;
            _settingsService = settingsService;
        }

        public async Task<JObject> Login(AuthModel authModel)
        {
            var uri = UriHelper.CombineUri(_settingsService.EndPoint(), ApiUrlBase+"/Login");
            return await _requestProvider.PostAsyncStringResultAsync<AuthModel>(uri, authModel);
        }
    }
}
