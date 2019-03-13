using RCM.Mobile.Helpers;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace RCM.Mobile.Services
{
    public interface IFirebaseTokenService
    {
        Task<string> AddFirebaseToken(string userToken, string firebaseToken);
        Task UpdateFirebaseToken(string userToken, string firebaseToken);
        Task DeleteFirebaseToken(string userToken);
    }

    public class FirebaseTokenService : IFirebaseTokenService
    {
        private const string ApiUrlBase = "/FirebaseToken";
        private readonly IRequestProvider _requestProvider;

        public FirebaseTokenService(IRequestProvider requestProvider)
        {
            _requestProvider = requestProvider;
        }

      
        public async Task<string> AddFirebaseToken(string userToken, string firebaseToken)
        {
            var uri = UriHelper.CombineUri(GlobalSetting.DefaultEndpoint, ApiUrlBase);
            return await _requestProvider.PostFirebaseToken(uri,firebaseToken,userToken);
        }

        public async Task DeleteFirebaseToken(string userToken)
        {
            var uri = UriHelper.CombineUri(GlobalSetting.DefaultEndpoint, ApiUrlBase);
            await _requestProvider.DeleteAsync(uri, userToken);
        }

        public async Task UpdateFirebaseToken(string userToken, string firebaseToken)
        {
            var uri = UriHelper.CombineUri(GlobalSetting.DefaultEndpoint, ApiUrlBase);
             await _requestProvider.PutFirebaseToken(uri, firebaseToken, userToken);
        }
    }
}
