using RCM.Mobile.Helpers;
using RCM.Mobile.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace RCM.Mobile.Services
{
    public class ContactService : IContactService
    {
        private const string ApiUrlBase = "/Contact";
        private readonly IRequestProvider _requestProvider;
        private readonly ISettingsService _settingsService;

        public ContactService(IRequestProvider requestProvider, ISettingsService settingsService)
        {
            _requestProvider = requestProvider;
            _settingsService = settingsService;
        }
        public async Task<List<Contact>> GetDebtors(string token, string name)
        {
            var uri = UriHelper.CombineUri(_settingsService.EndPoint(), ApiUrlBase + $"/GetDebtors/{name}");
            return await _requestProvider.GetAsync<List<Contact>>(uri, token);
        }
    }
    public interface IContactService
    {
        Task<List<Contact>> GetDebtors(string token, string name);
    }
}
