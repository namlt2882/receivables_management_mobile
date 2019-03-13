using RCM.Mobile.Helpers;
using RCM.Mobile.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading.Tasks;

namespace RCM.Mobile.Services
{
    public class TaskService : ITaskService
    {
        private const string ApiUrlBase = "/Task";
        private readonly IRequestProvider _requestProvider;

        public TaskService(IRequestProvider requestProvider)
        {
            _requestProvider = requestProvider;
        }
        public async Task<ObservableCollection<RCM.Mobile.ViewModels.Task>> GetAssignedTaskAsync(string token)
        {
            var uri = UriHelper.CombineUri(GlobalSetting.DefaultEndpoint, ApiUrlBase + "/GetAssignedTask");
            return await _requestProvider.GetAsync<ObservableCollection<RCM.Mobile.ViewModels.Task>>(uri, token);
        }
    }
    public interface ITaskService
    {
        Task<ObservableCollection<RCM.Mobile.ViewModels.Task>> GetAssignedTaskAsync(string token);
    }
}
