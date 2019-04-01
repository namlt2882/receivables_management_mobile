using RCM.Mobile.Helpers;
using RCM.Mobile.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading.Tasks;
using Task = System.Threading.Tasks.Task;

namespace RCM.Mobile.Services
{
    public class ReceivableService : IReceivableService
    {
        private const string ApiUrlBase = "/Receivable";
        private readonly IRequestProvider _requestProvider;
        private readonly ISettingsService _settingsService;

        public ReceivableService(IRequestProvider requestProvider, ISettingsService settingsService)
        {
            _requestProvider = requestProvider;
            _settingsService = settingsService;
        }

        public async Task CloseReceivable(string token, ReceivableCloseModel receivableCM)
        {
            var uri = UriHelper.CombineUri(_settingsService.EndPoint(), ApiUrlBase + "/CloseReceivable");
            await _requestProvider.PutAsync(uri, receivableCM, token);
        }

        public async Task<Receivable> GetAssignedReceivableAsync(string token, int receivableId)
        {
            var uri = UriHelper.CombineUri(_settingsService.EndPoint(), ApiUrlBase + $"/GetAssignedReceivable/{receivableId}");
            return await _requestProvider.GetAsync<Receivable>(uri, token);
        }

        public async Task<List<Receivable>> GetAssignedReceivablesAsync(string token, bool isHistory)
        {
            var uri = UriHelper.CombineUri(_settingsService.EndPoint(), ApiUrlBase + "/GetAssignedReceivables/"+isHistory);
            return await _requestProvider.ReceivableListPostAsync(uri, token);
        }
        public async Task<List<Receivable>> GetAssignedReceivablesAsync(string token, List<int> receivableIdList)
        {
            var uri = UriHelper.CombineUri(_settingsService.EndPoint(), ApiUrlBase + "/GetAssignedReceivables");
            return await _requestProvider.ReceivableListPostAsync(uri, receivableIdList, token);
        }
    }
    public interface IReceivableService
    {
        Task<List<Receivable>> GetAssignedReceivablesAsync(string token, List<int> receivableIdList);
        Task<List<Receivable>> GetAssignedReceivablesAsync(string token, bool isHistory);
        Task<Receivable> GetAssignedReceivableAsync(string token, int receivableId);
        Task CloseReceivable(string token, ReceivableCloseModel receivableCM);
    }
}
