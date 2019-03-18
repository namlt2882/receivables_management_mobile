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

        public async Task CloseReceivable(string token, ReceivableCloseModel receivableCM)
        {
            var uri = UriHelper.CombineUri(GlobalSetting.DefaultEndpoint, ApiUrlBase + "/CloseReceivable");
            await _requestProvider.PutAsync(uri, receivableCM, token);
        }

        public async Task<Receivable> GetAssignedReceivableAsync(string token, int receivableId)
        {
            var uri = UriHelper.CombineUri(GlobalSetting.DefaultEndpoint, ApiUrlBase + $"/GetAssignedReceivable/{receivableId}");
            return await _requestProvider.GetAsync<Receivable>(uri, token);
        }

        public async Task<ObservableCollection<Receivable>> GetAssignedReceivablesAsync(string token, List<int> receivableIdList)
        {
            var uri = UriHelper.CombineUri(GlobalSetting.DefaultEndpoint, ApiUrlBase + "/GetAssignedReceivables");
            return await _requestProvider.ReceivableListPostAsync(uri, receivableIdList, token);
        }
    }
    public interface IReceivableService
    {
        Task<ObservableCollection<Receivable>> GetAssignedReceivablesAsync(string token, List<int> receivableIdList);
        Task<Receivable> GetAssignedReceivableAsync(string token, int receivableId);
        Task CloseReceivable(string token, ReceivableCloseModel receivableCM);


    }
}
