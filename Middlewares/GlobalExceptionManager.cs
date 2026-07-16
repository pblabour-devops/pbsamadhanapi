using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using pbsamadhannetcoreapi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.HttpOverrides;
using System.Data;
using System.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Dapper;
using pbsamadhannetcoreapi.CommonUtiliteis;
using pbsamadhannetcoreapi.ViewModels;
using Newtonsoft.Json.Linq;
using System.Diagnostics;
using System.IO;

namespace pbsamadhannetcoreapi.Middlewares
{
    public class GlobalExceptionManager
    {
        private readonly RequestDelegate _nextMiddelware;
        private readonly IDbConnection _db;
        public GlobalExceptionManager(RequestDelegate nextMiddelware,IConfiguration config)
        {
            _db = new SqlConnection(config.GetConnectionString("SQLServerConnection_Logger"));
            _nextMiddelware = nextMiddelware;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _nextMiddelware(context);
            }
            catch(Exception ex)
            {
                var ipAddress = context.Request.HttpContext.Connection.RemoteIpAddress;
                context.Response.ContentType = "application/problem+json";
                context.Response.StatusCode = StatusCodes.Status500InternalServerError;

                LoggedUserInfoViewModel tokenClaims = null;
                //if (context.User.Identity.Name != null)
                //{
                    string jsonContent = "";
                    if (Debugger.IsAttached)
                    {
                        jsonContent = File.ReadAllText("appsettings.Development.json");
                    }
                    else
                    {
                        jsonContent = File.ReadAllText("appsettings.json");
                    }

                    JObject config = JObject.Parse(jsonContent);
                    var keys = JsonConvert.DeserializeObject<AppSetting_EncryptionConfigs_ViewModel>(config["EncryptionConfigs"].ToString());
                    bool flagClaim = false;

                    var serverSecret = context.User.Claims.Where(x => x.Type == "ServerSecret").FirstOrDefault().Value;
                if (serverSecret != null)
                {
                    var decyptedText = RijndaelManagedCryptoHandler.DecryptFromPlainTextToBase64(serverSecret,
                        keys.TokenDataEncryptionKey,
                        keys.TokenDataIVKey);
                    tokenClaims = JsonConvert.DeserializeObject<LoggedUserInfoViewModel>(decyptedText);
                }
                else
                {
                    tokenClaims = null;
                }
                    
                //}
                var query = "INSERT INTO ErrorLogs (ErrRoute, ErrDesc,ErrException, ErrIP, ErrDate, ProjectModuleId, Username,RequestParameter, UserId) VALUES(@ErrRoute, @ErrDesc, @ErrException, @ErrIP, @ErrDate, @ProjectModuleId, @Username,@RequestParameter, @UserId)";
                var parameters = new
                {
                    ErrRoute = context.Request.Path.Value,
                    ErrDesc = ex.Message,
                    ErrException = ex.ToString(),
                    ErrIP = ipAddress.ToString(),
                    ErrDate = DateTime.Now,
                    ProjectModuleId = 17,
                    Username = tokenClaims == null ? "Guest" : tokenClaims.UserName,
                    RequestParameter = context.Request.HttpContext.Request.QueryString.ToString(),
                    UserId = tokenClaims.UserId
                };
                
                await _db.ExecuteAsync(query, parameters, commandType: CommandType.Text);
            }
        }
    }
}
