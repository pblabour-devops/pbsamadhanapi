using Castle.Core.Configuration;
using DocumentFormat.OpenXml.Wordprocessing;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Org.BouncyCastle.Asn1.Ocsp;
using pbsamadhannetcoreapi.CommonUtiliteis;
using pbsamadhannetcoreapi.Models;
using pbsamadhannetcoreapi.Services.Implementations;
using pbsamadhannetcoreapi.ViewModels;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Threading.Tasks;

namespace pbsamadhannetcoreapi.CustomFillters
{
    public class AuthorizeAttribute : TypeFilterAttribute
    {
        public AuthorizeAttribute(string claim) : base(typeof(AuthorizeFilter))
        {
            Arguments = new object[] { claim };
        }
    }

    public class AuthorizeFilter : IAuthorizationFilter
    {
        readonly string[] _claim;
        public AuthorizeFilter(string claim)
        {

            _claim = claim.Split(",").ToArray();
        }
        public void OnAuthorization(AuthorizationFilterContext context)
        {

            //var location = new Uri($"{context.HttpContext.Request.Scheme}://{context.HttpContext.Request.Host}{context.HttpContext.Request.Path}{context.HttpContext.Request.QueryString}");

            //var url = location.AbsoluteUri;

            //string refererUrl = context.HttpContext.Request.Headers["Referer"].ToString();
            //string originUrl = context.HttpContext.Request.Headers["Origin"].ToString();
            var isAuthenticated = context.HttpContext.User.Identity.IsAuthenticated;
            var claimsIndentity = context.HttpContext.User.Identity as ClaimsIdentity;

            if (isAuthenticated)
            {
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
                var dynamicKeys = context.HttpContext.User.Claims.Where(x => x.Type == "SignatureToken").FirstOrDefault().Value;
                var decyptedText = RijndaelManagedCryptoHandler.DecryptFromPlainTextToBase64(context.HttpContext.User.Claims.Where(x => x.Type == "ServerSecret").FirstOrDefault().Value,
                dynamicKeys.Split("|")[0],
                dynamicKeys.Split("|")[1]);
                LoggedUserInfoViewModel tokenClaims = JsonConvert.DeserializeObject<LoggedUserInfoViewModel>(decyptedText);
                if (context.RouteData.Values.Where(x => x.Key == "action").FirstOrDefault().Value.ToString() == "AddUpdate_ProcessApplicationDetail")
                {
                    var streamReader = new StreamReader(context.HttpContext.Request.Body);
                    var requestBodyStream = streamReader.ReadToEnd();
                    var requestBody = JsonConvert.DeserializeObject<ApplicationActionViewModel>(requestBodyStream);
                    context.HttpContext.Request.Body.Position = 0;
                    if (requestBody.AppActionType == (int)AppActionTypeEnum.RAISED_FEE_PAID_OFFLINE && tokenClaims.RoleName == "INDL")
                    {
                        flagClaim = true;
                    }
                    else
                    {
                        foreach (var item in _claim)
                        {
                            if (item == tokenClaims.RoleName)
                                flagClaim = true;
                        }
                    }
                }
                else
                {
                    var customAppConfigs = JsonConvert.DeserializeObject<CustomAppConfigsModel>(config["CustomAppConfigs"].ToString());
                    if (!customAppConfigs.OtpBypassUserRoles.Split(",").Any(x => x == tokenClaims.RoleName))
                    {
                        using (SqlConnection con = new SqlConnection("Data Source=10.44.86.91;Initial Catalog=PbLabourCoreNetDb;Integrated Security=SSPI;User ID=labour;Password=Db$La@0ur91#!T3@mX9Qp;Trusted_Connection=False;Persist Security Info=True;MultipleActiveResultSets=True"))
                        {
                            con.Open();
                            string query = @"SELECT COUNT(*) FROM User2FactorVerifications WHERE UserRefId = @userId AND ResponseId = @loginResponseId AND User2FactorVerificationType = 1";

                            SqlCommand cmd = new SqlCommand(query, con);
                            cmd.Parameters.AddWithValue("@userId", tokenClaims.UserId);
                            cmd.Parameters.AddWithValue("@loginResponseId", tokenClaims.LoginResponseId);

                            int count = (int)cmd.ExecuteScalar();
                            if (count>0)
                            {
                                foreach (var item in _claim)
                                {
                                    if (item == tokenClaims.RoleName)
                                        flagClaim = true;
                                }
                            }
                            else
                            {
                                flagClaim = false;
                            }
                        }
                    }
                    else
                    {
                        foreach (var item in _claim)
                        {
                            if (item == tokenClaims.RoleName)
                                flagClaim = true;
                        }
                    }
                }
                if (!flagClaim)
                {
                    context.HttpContext.Response.StatusCode = (int)HttpStatusCode.Unauthorized; //Set HTTP 401   
                    context.Result = new ObjectResult("Access Denied: You do not have permission.")
                    {
                        StatusCode = StatusCodes.Status401Unauthorized
                    };
                }
            }
            else
            {
                context.Result = new ObjectResult("Access Denied: You are not authenticated.")
                {
                    StatusCode = StatusCodes.Status403Forbidden
                };
            }
            //context.Result = new ObjectResult("")
            //{
            //    StatusCode = StatusCodes.Status200OK
            //};
        }
    }
}  

