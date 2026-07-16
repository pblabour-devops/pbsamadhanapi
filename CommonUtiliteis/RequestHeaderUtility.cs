using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace pbsamadhannetcoreapi.CommonUtiliteis
{
    public class RequestHeaderUtility
    {
        private IHttpContextAccessor _httpContextAccessor;
        public RequestHeaderUtility(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }
        public string GetRequestIP(bool tryUseXForwardHeader = true)
        {
            string ip = null;
            if (tryUseXForwardHeader)
                ip = GetHeaderValueAs<string>("X-Forwarded-For").SplitCsv().FirstOrDefault();
            // RemoteIpAddress is always null in DNX RC1 Update1 (bug).
            if (ip.IsNullOrWhitespace() && _httpContextAccessor.HttpContext?.Connection?.RemoteIpAddress != null)
                ip = _httpContextAccessor.HttpContext.Connection.RemoteIpAddress.ToString();

            if (ip.IsNullOrWhitespace())
                ip = GetHeaderValueAs<string>("REMOTE_ADDR");

            // _httpContextAccessor.HttpContext?.Request?.Host this is the local host.

            if (ip.IsNullOrWhitespace())
                throw new Exception("Unable to determine caller's IP.");

            return ip;
        }

        public T GetHeaderValueAs<T>(string headerName)
        {
            StringValues values;

            if (_httpContextAccessor.HttpContext?.Request?.Headers?.TryGetValue(headerName, out values) ?? false)
            {
                string rawValues = values.ToString();   // writes out as Csv when there are multiple.

                if (!rawValues.IsNullOrWhitespace())
                    return (T)Convert.ChangeType(values.ToString(), typeof(T));
            }
            return default(T);
        }
    }
}
