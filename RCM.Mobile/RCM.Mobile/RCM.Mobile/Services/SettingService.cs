using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace RCM.Mobile.Services
{
    public class SettingsService : ISettingsService
    {
        #region Setting Constants
        private const string AccessToken = "access_token";
        private const string UserName = "username";
        //private const string IdUseMocks = "use_mocks";
        private const string IdBase = "url_base";
        //private const string IdIdentityBase = "url_base";
        private const string AuthAccessTokenExpirationDate = "expires_in";
        //private const string IdGatewayMarketingBase = "url_marketing";
        //private const string IdGatewayShoppingBase = "url_shopping";
        //private const string IdUseFakeLocation = "use_fake_location";
        private const string IdLatitude = "latitude";
        private const string IdLongitude = "longitude";
        private const string IdAllowGpsLocation = "allow_gps_location";
        private readonly string AccessTokenDefault = string.Empty;
        //private readonly string IdTokenDefault = string.Empty;
        //private readonly bool UseMocksDefault = true;
        //private readonly bool UseFakeLocationDefault = false;
        private readonly bool AllowGpsLocationDefault = false;
        //private readonly double FakeLatitudeDefault = 47.604610d;
        //private readonly double FakeLongitudeDefault = -122.315752d;
        private readonly string UrlDefault = GlobalSetting.DefaultEndpoint;
        //private readonly string UrlIdentityDefault = GlobalSetting.Instance.BaseIdentityEndpoint;
        //private readonly string UrlGatewayMarketingDefault = GlobalSetting.Instance.BaseGatewayMarketingEndpoint;
        //private readonly string UrlGatewayShoppingDefault = GlobalSetting.Instance.BaseGatewayShoppingEndpoint;
        #endregion

        #region Settings Properties

        public string AuthAccessToken
        {
            get => GetValueOrDefault(AccessToken, AccessTokenDefault);
            set => AddOrUpdateValue(AccessToken, value);
        }

        public string AuthUserName
        {
            get => GetValueOrDefault(UserName, "");
            set => AddOrUpdateValue(UserName, value);
        }


        public DateTime AccessTokenExpirationDate
        {
            get => GetValueOrDefault(AuthAccessTokenExpirationDate, DateTime.UtcNow);
            set => AddOrUpdateValue(AuthAccessTokenExpirationDate, value);
        }
        public bool TokenIsExpired
        {
            get => DateTime.UtcNow.CompareTo(AccessTokenExpirationDate) > 0;
        }

        //public bool UseMocks
        //{
        //    get => GetValueOrDefault(IdUseMocks, UseMocksDefault);
        //    set => AddOrUpdateValue(IdUseMocks, value);
        //}

        public string IdentityEndpointBase
        {
            get => GetValueOrDefault(IdBase, UrlDefault);
            set => AddOrUpdateValue(IdBase, value);
        }
        //public string IdentityEndpointBase
        //{
        //    get => GetValueOrDefault(IdIdentityBase, UrlIdentityDefault);
        //    set => AddOrUpdateValue(IdIdentityBase, value);
        //}

        //public string GatewayShoppingEndpointBase
        //{
        //    get => GetValueOrDefault(IdGatewayShoppingBase, UrlGatewayShoppingDefault);
        //    set => AddOrUpdateValue(IdGatewayShoppingBase, value);
        //}

        //public string GatewayMarketingEndpointBase
        //{
        //    get => GetValueOrDefault(IdGatewayMarketingBase, UrlGatewayMarketingDefault);
        //    set => AddOrUpdateValue(IdGatewayMarketingBase, value);
        //}

        //public bool UseFakeLocation
        //{
        //    get => GetValueOrDefault(IdUseFakeLocation, UseFakeLocationDefault);
        //    set => AddOrUpdateValue(IdUseFakeLocation, value);
        //}

        public string Latitude
        {
            get => GetValueOrDefault(IdLatitude, "");
            set => AddOrUpdateValue(IdLatitude, value);
        }

        public string Longitude
        {
            get => GetValueOrDefault(IdLongitude, "");
            set => AddOrUpdateValue(IdLongitude, value);
        }

        public bool AllowGpsLocation
        {
            get => GetValueOrDefault(IdAllowGpsLocation, AllowGpsLocationDefault);
            set => AddOrUpdateValue(IdAllowGpsLocation, value);
        }

        #endregion

        #region Public Methods

        public Task AddOrUpdateValue(string key, bool value) => AddOrUpdateValueInternal(key, value);
        public Task AddOrUpdateValue(string key, string value) => AddOrUpdateValueInternal(key, value);
        public Task AddOrUpdateValue(string key, DateTime value) => AddOrUpdateValueInternal(key, value);
        public bool GetValueOrDefault(string key, bool defaultValue) => GetValueOrDefaultInternal(key, defaultValue);
        public string GetValueOrDefault(string key, string defaultValue) => GetValueOrDefaultInternal(key, defaultValue);
        public DateTime GetValueOrDefault(string key, DateTime defaultValue) => GetValueOrDefaultInternal(key, defaultValue);

        #endregion

        #region Internal Implementation

        async Task AddOrUpdateValueInternal<T>(string key, T value)
        {
            if (value == null)
            {
                await Remove(key);
            }

            Application.Current.Properties[key] = value;
            try
            {
                await Application.Current.SavePropertiesAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Unable to save: " + key, " Message: " + ex.Message);
            }
        }

        T GetValueOrDefaultInternal<T>(string key, T defaultValue = default(T))
        {
            object value = null;
            if (Application.Current.Properties.ContainsKey(key))
            {
                value = Application.Current.Properties[key];
            }
            return null != value ? (T)value : defaultValue;
        }

        async Task Remove(string key)
        {
            try
            {
                if (Application.Current.Properties[key] != null)
                {
                    Application.Current.Properties.Remove(key);
                    await Application.Current.SavePropertiesAsync();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Unable to remove: " + key, " Message: " + ex.Message);
            }
        }

        #endregion
    }
    public interface ISettingsService
    {
        string AuthAccessToken { get; set; }
        string AuthUserName { get; set; }
        DateTime AccessTokenExpirationDate { get; set; }
        //bool UseMocks { get; set; }
        string IdentityEndpointBase { get; set; }
        //string GatewayShoppingEndpointBase { get; set; }
        //string GatewayMarketingEndpointBase { get; set; }
        //bool UseFakeLocation { get; set; }
        string Latitude { get; set; }
        string Longitude { get; set; }
        bool AllowGpsLocation { get; set; }

        bool TokenIsExpired { get; }
        bool GetValueOrDefault(string key, bool defaultValue);
        string GetValueOrDefault(string key, string defaultValue);
        Task AddOrUpdateValue(string key, bool value);
        Task AddOrUpdateValue(string key, string value);
    }
}
