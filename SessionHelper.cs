using DBE.ENERGY.Core.Entities;
using DBE.ENERGY.Core.Interfaces;
using DBE.ENERGY.Web.Extensions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System;

namespace DBE.ENERGY.Web.Helper
{
    public class SessionHelper : ISessionHelper
    {
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly IConfiguration _configuration;

        public SessionHelper(IHttpContextAccessor contextAccessor, IConfiguration configuration)
        {
            _contextAccessor = contextAccessor;
            _configuration = configuration;
        }
        public UserEntity User
        {
            get { return Get<UserEntity>("User"); }
            set { Set("User", value); }
        }

        public string BasePath {
            get { return _configuration.GetValue<string>("BasePath"); }
            set { }
        }

        public string BUILD_VERSION
        {
            get
            {
                return System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString();
            }
        }

        public string BUILD_DATE
        {
            get
            {
                Version version = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version;

                //Add the number of days (build)
                DateTime result = DateTime.Parse("1/1/2000");

                result = result.AddDays(version.Build);

                //Add the number of seconds since midnight (revision) multiplied by 2

                result = result.AddSeconds(version.Revision * 2);

                return result.ToString("dd.MM.yyyy HH:mm");
            }
        }

        public bool IsPrivilegedUser()
        {
            throw new NotImplementedException();
        }



        /// <summary> Gets. </summary>
        /// <typeparam name="T"> Generic type parameter. </typeparam>
        /// <param name="key"> The key. </param>
        /// <returns> . </returns>
        private T Get<T>(string key)
        {
            object o = _contextAccessor.HttpContext.Session.GetObject<T>(key);
            if (o is T)
            {
                return (T)o;
            }

            return default(T);
        }

        /// <summary> Sets. </summary>
        /// <typeparam name="T"> Generic type parameter. </typeparam>
        /// <param name="key">  The key. </param>
        /// <param name="item"> The item. </param>
        private void Set<T>(string key, T item)
        {
            _contextAccessor.HttpContext.Session.SetObject(key, item);
        }


    }
}
