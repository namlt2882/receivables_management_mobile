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
        private readonly ISettingsService _settingsService;

        public TaskService(IRequestProvider requestProvider, ISettingsService settingsService)
        {
            _requestProvider = requestProvider;
            _settingsService = settingsService;
        }
        public async Task<List<Models.Task>> GetCollectorAssignedTasks(string token)
        {
            var uri = UriHelper.CombineUri(_settingsService.EndPoint(), ApiUrlBase + "/GetCollectorAssignedTasks");
            return await _requestProvider.GetAsync<List<Models.Task>>(uri, token);
        }
        public async Task<List<Models.Task>> GetAssignedTaskByDay(string token, int day)
        {
            var uri = UriHelper.CombineUri(_settingsService.EndPoint(), ApiUrlBase + $"/GetAssignedTaskByDay/{day}");
            return await _requestProvider.GetAsync<List<Models.Task>>(uri, token);
        }
        public async Task<List<DateTime>> GetCollectorCalendarTasks(string token)
        {
            var uri = UriHelper.CombineUri(_settingsService.EndPoint(), ApiUrlBase + "/GetCollectorCalendarTasks");
            return await _requestProvider.GetAsync<List<DateTime>>(uri, token);
        }
        public async System.Threading.Tasks.Task<bool> Update(string token, UpdateTaskModel updateTaskModel)
        {
            var uri = UriHelper.CombineUri(_settingsService.EndPoint(), ApiUrlBase + "/UpdateTask");
            return await _requestProvider.UpdateTask(uri, updateTaskModel, token);
        }

        public async Task<List<Models.Task>> GetAssignedTaskByReceivableAndDay(string token, int day, int receivableId)
        {
            var uri = UriHelper.CombineUri(_settingsService.EndPoint(), ApiUrlBase + $"/GetAssignedTaskByReceivableAndDay/{day}/{receivableId}");
            return await _requestProvider.GetAsync<List<Models.Task>>(uri, token);
        }

        public async Task<List<Models.Task>> GetCompletedTaskByReceivableId(string token, int receivableId)
        {
            var uri = UriHelper.CombineUri(_settingsService.EndPoint(), ApiUrlBase + $"/GetCompletedTaskByReceivableId/{receivableId}");
            return await _requestProvider.GetAsync<List<Models.Task>>(uri, token);
        }
        public async Task<List<Models.Task>> GetTodoTaskByReceivableId(string token, int receivableId)
        {
            var uri = UriHelper.CombineUri(_settingsService.EndPoint(), ApiUrlBase + $"/GetTodoTaskByReceivableId/{receivableId}");
            return await _requestProvider.GetAsync<List<Models.Task>>(uri, token);
        }
        public async System.Threading.Tasks.Task Cancel(string token, int id)
        {
            var uri = UriHelper.CombineUri(_settingsService.EndPoint(), ApiUrlBase + "/Cancel/"+id);
             await _requestProvider.CancelTask(uri, token);
        }
    }
    public interface ITaskService
    {
        Task<List<Models.Task>> GetCollectorAssignedTasks(string token);
        Task<List<Models.Task>> GetAssignedTaskByDay(string token, int day);
        Task<List<Models.Task>> GetAssignedTaskByReceivableAndDay(string token, int day, int receivableId);
        Task<List<Models.Task>> GetCompletedTaskByReceivableId(string token, int receivableId);
        Task<List<Models.Task>> GetTodoTaskByReceivableId(string token, int receivableId);
        Task<List<DateTime>> GetCollectorCalendarTasks(string token);
        System.Threading.Tasks.Task<bool> Update(string token, UpdateTaskModel updateTaskModel);
        System.Threading.Tasks.Task Cancel(string token, int id);
    }
}
