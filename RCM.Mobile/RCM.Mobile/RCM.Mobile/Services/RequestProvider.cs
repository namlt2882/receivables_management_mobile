﻿using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Microsoft.CSharp;
using RCM.Mobile.Models;
using System.Collections.ObjectModel;
using Task = System.Threading.Tasks.Task;
using Prism.Services;

namespace RCM.Mobile.Services
{
    public class ServiceAuthenticationException : Exception
    {
        public string Content { get; }

        public ServiceAuthenticationException()
        {
        }

        public ServiceAuthenticationException(string content)
        {
            Content = content;
        }
    }
    public class HttpRequestExceptionEx : HttpRequestException
    {
        public System.Net.HttpStatusCode HttpCode { get; }
        public HttpRequestExceptionEx(System.Net.HttpStatusCode code) : this(code, null, null)
        {
        }

        public HttpRequestExceptionEx(System.Net.HttpStatusCode code, string message) : this(code, message, null)
        {
        }

        public HttpRequestExceptionEx(System.Net.HttpStatusCode code, string message, Exception inner) : base(message,
            inner)
        {
            HttpCode = code;
        }

    }

    public interface IRequestProvider
    {
        Task<TResult> GetAsync<TResult>(string uri, string token = "");
        Task<List<Receivable>> ReceivableListPostAsync(string uri, List<int> receivableIdList, string token = "");
        Task<List<Receivable>> ReceivableListPostAsync(string uri, string token = "");
        Task<TResult> PostAsync<TResult>(string uri, TResult data, string token = "", string header = "");
        Task<JObject> PostAsyncStringResultAsync<TResult>(string uri, TResult data, string token = "", string header = "");
        Task<TResult> PutAsync<TResult>(string uri, TResult data, string token = "", string header = "");
        Task CancelTask(string uri, string token = "", string header = "");
        Task<bool> UpdateTask(string uri, UpdateTaskModel data, string token = "", string header = "");
        Task<string> PostFirebaseToken(string uri, string firebaseToken, string token = "", string header = "");
        Task PutFirebaseToken(string uri, string firebaseToken, string token = "", string header = "");
        Task DeleteAsync(string uri, string token = "");
    }

    public class RequestProvider : IRequestProvider
    {
        private readonly JsonSerializerSettings _serializerSettings;
        private readonly IPageDialogService _dialogService;
        public RequestProvider(IPageDialogService dialogService)
        {
            _dialogService = dialogService;
            _serializerSettings = new JsonSerializerSettings
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver(),
                DateTimeZoneHandling = DateTimeZoneHandling.Utc,
                NullValueHandling = NullValueHandling.Ignore
            };
            _serializerSettings.Converters.Add(new StringEnumConverter());
        }

        public async Task<TResult> GetAsync<TResult>(string uri, string token = "")
        {
            HttpClient httpClient = CreateHttpClient(token);
            HttpResponseMessage response = await httpClient.GetAsync(uri);

            await HandleResponse(response);
            string serialized = await response.Content.ReadAsStringAsync();

            TResult result = await Task.Run(() =>
                JsonConvert.DeserializeObject<TResult>(serialized, _serializerSettings));

            return result;
        }

        public async Task<TResult> PostAsync<TResult>(string uri, TResult data, string token = "", string header = "")
        {
            HttpClient httpClient = CreateHttpClient(token);

            if (!string.IsNullOrEmpty(header))
            {
                AddHeaderParameter(httpClient, header);
            }

            var content = new StringContent(JsonConvert.SerializeObject(data));
            content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            HttpResponseMessage response = await httpClient.PostAsync(uri, content);

            await HandleResponse(response);
            string serialized = await response.Content.ReadAsStringAsync();

            TResult result = await Task.Run(() =>
                JsonConvert.DeserializeObject<TResult>(serialized, _serializerSettings));

            return result;
        }

        //FirebaseToken
        public async Task<string> PostFirebaseToken(string uri, string firebaseToken, string token = "", string header = "")
        {
            HttpClient httpClient = CreateHttpClient(token);

            if (!string.IsNullOrEmpty(header))
            {
                AddHeaderParameter(httpClient, header);
            }

            var content = new StringContent(JsonConvert.SerializeObject(""));
            content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            HttpResponseMessage response = await httpClient.PostAsync($"{uri}?firebaseToken=" + firebaseToken
            , content);
            await HandleResponse(response);
            string serialized = await response.Content.ReadAsStringAsync();

            string result = await Task.Run(() =>
                JsonConvert.DeserializeObject<string>(serialized, _serializerSettings));
            return result;
        }
        public async Task PutFirebaseToken(string uri, string firebaseToken, string token = "", string header = "")
        {
            HttpClient httpClient = CreateHttpClient(token);

            if (!string.IsNullOrEmpty(header))
            {
                AddHeaderParameter(httpClient, header);
            }

            var content = new StringContent(JsonConvert.SerializeObject(""));
            content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            HttpResponseMessage response = await httpClient.PutAsync($"{uri}?firebaseToken=" + firebaseToken
            , content);
            await HandleResponse(response);

        }

        public async Task<TResult> PutAsync<TResult>(string uri, TResult data, string token = "", string header = "")
        {
            HttpClient httpClient = CreateHttpClient(token);

            if (!string.IsNullOrEmpty(header))
            {
                AddHeaderParameter(httpClient, header);
            }

            var content = new StringContent(JsonConvert.SerializeObject(data));
            content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            HttpResponseMessage response = await httpClient.PutAsync(uri, content);

            await HandleResponse(response);
            string serialized = await response.Content.ReadAsStringAsync();

            TResult result = await Task.Run(() =>
                JsonConvert.DeserializeObject<TResult>(serialized, _serializerSettings));
            return result;
        }

        public async Task DeleteAsync(string uri, string token = "")
        {
            HttpClient httpClient = CreateHttpClient(token);
            await httpClient.DeleteAsync(uri);
        }

        private HttpClient CreateHttpClient(string token = "")
        {
            var httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            if (!string.IsNullOrEmpty(token))
            {
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }
            return httpClient;
        }

        private void AddHeaderParameter(HttpClient httpClient, string parameter)
        {
            if (httpClient == null)
                return;

            if (string.IsNullOrEmpty(parameter))
                return;

            httpClient.DefaultRequestHeaders.Add(parameter, Guid.NewGuid().ToString());
        }

        private async Task HandleResponse(HttpResponseMessage response)
        {
            if (!response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();

                if (response.StatusCode == HttpStatusCode.Forbidden ||
                    response.StatusCode == HttpStatusCode.Unauthorized)
                {
                    await _dialogService.DisplayAlertAsync(response.StatusCode.ToString(), "", "OK");
                    //throw new ServiceAuthenticationException(content);
                }
                await _dialogService.DisplayAlertAsync(response.StatusCode.ToString(), "", "OK");
                //throw new HttpRequestExceptionEx(response.StatusCode, content);
            }
        }

        public async Task<JObject> PostAsyncStringResultAsync<TResult>(string uri, TResult data, string token = "", string header = "")
        {
            HttpClient httpClient = CreateHttpClient(token);

            if (!string.IsNullOrEmpty(header))
            {
                AddHeaderParameter(httpClient, header);
            }

            var content = new StringContent(JsonConvert.SerializeObject(data));
            content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            HttpResponseMessage response = await httpClient.PostAsync(uri, content);

            //await HandleResponse(response);
            //return await response.Content.ReadAsStringAsync(); ;
            var result = response.Content.ReadAsStringAsync().Result;
            JObject jwtDynamic = JsonConvert.DeserializeObject<dynamic>(result);
            return jwtDynamic;
        }
        public async Task<List<Receivable>> ReceivableListPostAsync(string uri, string token = "")
        {
            HttpClient httpClient = CreateHttpClient(token);

            var content = new StringContent(JsonConvert.SerializeObject(""));
            content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            HttpResponseMessage response = await httpClient.PostAsync(uri, content);
            await HandleResponse(response);
            if (!response.IsSuccessStatusCode)
            {
                return new List<Receivable>();
            }
            string serialized = await response.Content.ReadAsStringAsync();

            List<Receivable> result = await Task.Run(() =>
                JsonConvert.DeserializeObject<List<Receivable>>(serialized, _serializerSettings));

            return result;
        }
        public async Task<List<Receivable>> ReceivableListPostAsync(string uri, List<int> receivableIdList, string token = "")
        {
            HttpClient httpClient = CreateHttpClient(token);
            var content = new StringContent(JsonConvert.SerializeObject(receivableIdList));
            content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            HttpResponseMessage response = await httpClient.PostAsync(uri, content);

            await HandleResponse(response);
            if (!response.IsSuccessStatusCode)
            {
                return new List<Receivable>();
            }
            string serialized = await response.Content.ReadAsStringAsync();

            List<Receivable> result = await Task.Run(() =>
                JsonConvert.DeserializeObject<List<Receivable>>(serialized, _serializerSettings));

            return result;
        }

        public async Task<bool> UpdateTask(string uri, UpdateTaskModel data, string token = "", string header = "")
        {
            HttpClient httpClient = CreateHttpClient(token);

            if (!string.IsNullOrEmpty(header))
            {
                AddHeaderParameter(httpClient, header);
            }
            #region File

            //var content = new StringContent(JsonConvert.SerializeObject(data));
            //content.Headers.ContentType = new MediaTypeHeaderValue("multipart/form-data");
            HttpContent file = new StreamContent(data.File.StreamSource);
            file.Headers.ContentDisposition = new System.Net.Http.Headers.ContentDispositionHeaderValue("form-data") { Name = "file", FileName = data.File.FileName };
            file.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/octet-stream");
            #endregion
            var formData = new MultipartFormDataContent();
            formData.Add(file);
            formData.Add(new StringContent(data.Id.ToString()), "Id");
            formData.Add(new StringContent(data.Note != null ? data.Note : ""), "Note");
            HttpResponseMessage response = await httpClient.PostAsync(uri, formData);
            await HandleResponse(response);
            if (response.StatusCode == HttpStatusCode.OK)
            {
                return true;
            }
            return false;
        }

        public async Task CancelTask(string uri, string token = "", string header = "")
        {
            HttpClient httpClient = CreateHttpClient(token);

            if (!string.IsNullOrEmpty(header))
            {
                AddHeaderParameter(httpClient, header);
            }
            HttpResponseMessage response = await httpClient.PutAsync(uri, null);
            await HandleResponse(response);
        }
    }
}
