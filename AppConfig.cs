using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace pbsamadhannetcoreapi
{
    public class CustomAppConfigsModel
    {
        public JwtConfigs JwtConfigs { get; set; }
        public ConnectionStrings DbConnectionStrings { get; set; }
        public string OtpBypassUserRoles { get; set; }
    }
    public class JwtConfigs
    {
        public string Issuer { get; set; }
        public string Audience { get; set; }
        public string SecurityKey { get; set; }
    }
    public class ConnectionStrings
    {
        public string SQLServerConnection { get; set; }
    }
}
