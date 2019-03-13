using RCM.Mobile.Helpers;
using RCM.Mobile.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading.Tasks;

namespace RCM.Mobile.Services
{
    public class ReceivableService : IReceivableService
    {
        private const string ApiUrlBase = "/Receivable";
        private readonly IRequestProvider _requestProvider;

        public ReceivableService(IRequestProvider requestProvider)
        {
            _requestProvider = requestProvider;
        }
        public async Task<ObservableCollection<Receivable>> GetAssignedReceivableAsync(string token)
        {
            var uri = UriHelper.CombineUri(GlobalSetting.DefaultEndpoint, ApiUrlBase+ "/GetAssignedReceivable");
            return await _requestProvider.GetAsync<ObservableCollection<Receivable>>(uri, token);
        }
    }
    public interface IReceivableService
    {
        Task<ObservableCollection<Receivable>> GetAssignedReceivableAsync(string token);

    }
}
