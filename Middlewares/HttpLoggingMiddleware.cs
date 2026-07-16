//using pbsamadhannetcoreapi.CommonUtilities;
//using pbsamadhannetcoreapi.ViewModels;
using DocumentFormat.OpenXml.Presentation;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Primitives;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using pbsamadhannetcoreapi.CommonUtiliteis;
using pbsamadhannetcoreapi.CommonUtiliteis.RSA;
using pbsamadhannetcoreapi.Models;
using pbsamadhannetcoreapi.ViewModels;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace pbsamadhannetcoreapi.Middlewares
{
    public class HttpLoggingMiddleware
    {
        private readonly ILogger _logger;
        private readonly RequestDelegate _next;
        private IConfiguration _iConfiguration { get; }
        private IServiceScopeFactory _iServiceScopeFactory;
        private IHttpContextAccessor _httpContextAccessor;
        public HttpLoggingMiddleware(RequestDelegate next, 
            ILogger<HttpLoggingMiddleware> logger, 
            IConfiguration iConfiguration,
            IServiceScopeFactory iServiceScopeFactory,
            IHttpContextAccessor httpContextAccessor)
        {
            _iConfiguration = iConfiguration;
            _next = next;
            _logger = logger;
            _iServiceScopeFactory = iServiceScopeFactory;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task Invoke(HttpContext context)
        {
            //Copy  pointer to the original response body stream
            var originalBodyStream = context.Response.Body;

            //List<string> requestsToBeAvoided = new List<string>();
            //requestsToBeAvoided.Add("signalrhub");
            //requestsToBeAvoided.Add("ServiceGateway");

            //if (context.Request.Path.Value.Contains("signalrhub") 
            //    || context.Request.Path.Value.Contains("ServiceGateway") 
            //    || context.Request.Path.Value.Contains("UploadDocument"))// requestsToBeAvoided.Any(x=>x.Contains(context.Request.Path.Value)))
            //{
            //    //request.Body.Position = 0;
            //}
            //else
            //{
                await GetRequestAsTextAsync(context, context.Request);
            //}

            
            //Get incoming request
            //var request = await GetRequestAsTextAsync(context.Request);
            //Log it
            //_logger.LogInformation(request);


            //Create a new memory stream and use it for the temp reponse body
            await using var responseBody = new MemoryStream();
            context.Response.Body = responseBody;

            //Continue down the Middleware pipeline
            await _next(context);

            //Format the response from the server
            var response = await GetResponseAsTextAsync(context.Response);
            ////Log it
            //_logger.LogInformation(response);

            //Copy the contents of the new memory stream, which contains the response to the original stream, which is then returned to the client.
            await responseBody.CopyToAsync(originalBodyStream);
        }


        private async Task GetRequestAsTextAsync(HttpContext context, HttpRequest request)
        {
            //var kk = request.Headers.Where(x => x.Key == "Referer").FirstOrDefault();

            request.EnableBuffering();

            // using var streamReader = new StreamReader(request.Body, leaveOpen: true);
            //var requestBody = await streamReader.ReadToEndAsync();
            StreamReader streamReader;
            string requestBody = "";

            int maxPayloadAgeInMinutes = Convert.ToInt32(_iConfiguration.GetSection("EncryptionConfigs").GetSection("ClientToServerPayloadAgeInMinutes").Value);
            if (request.Method == "GET")
            {
                requestBody = request.QueryString.Value.Replace("?", "");
                // || request.Path.Value.Contains("ServiceGateway")

                if (request.Path.Value.Contains("signalrhub") || 
                    request.Path.Value.Contains("ServiceGateway") || 
                    request.Path.Value.Contains("verifyLicenseNumber") || 
                    request.Path.Value.Contains("sendRandomizationNotificationToFactoryByRandomizationId") || 
                    request.Path.Value.Contains("sendInspectionNotificationToLabourWingByFactoryCircleId") || 
                    request.Path.Value.Contains("seedApplicationData") || 
                    request.Path.Value.Contains("sendInspectionNotificationToIndustryUserByFactoryCircleId") || 
                    request.Path.Value.Contains("initiateLegacyAppNotification") || 
                    request.Path.Value.Contains("sentLegacyNotificationByPhoneNo" )|| 
                    request.Path.Value.Contains("getAuthToken") || 
                    request.Path.Value.Contains("getEncryption") || 
                    request.Path.Value.Contains("getApplicationDetails") || 
                    request.Path.Value.Contains("hrms_GetHRMSCodeAndActWiseData") || 
                    request.Path.Value.Contains("withdrawApplication") || 
                    request.Path.Value.Contains("get_ReportByAuthority") || 
                    request.Path.Value.Contains("get_ReportByService") || 
                    request.Path.Value.Contains("ThirdPartyIntegrations/seedTimelineWiseAction") || 
                    request.Path.Value.Contains("esclationApplications") || 
                    request.Path.Value.Contains("deemedAllActCertificate") || 
                    request.Path.Value.Contains("validateLoginFromPartnerPortal") ||
                    request.Path.Value.Contains("ValidateViaMyOffice") ||
                    request.Path.Value.Contains("ValidateViaBocwMobileApp"))

                {
                    //request.QueryString = new QueryString(requestBody);
                }
                else
                {
                    //if(request.Path.Value.Contains("getStabiltyAcknoweldgementReceipt"))
                    //{
                    //    int i = 0;
                    //}
                    var collection = HttpUtility.ParseQueryString(requestBody);
                    var queryStrToJson = collection.AllKeys.ToDictionary(y => y, y => collection[y]);
                    if (queryStrToJson.Count() > 0)
                    {
                        //requestBody = JsonConvert.SerializeObject(collection.AllKeys.ToDictionary(y => y, y => collection[y]));
                        string queryString = "";
                        if (queryStrToJson["requestData"] != null)
                        {
                            if (Convert.ToBoolean(_iConfiguration.GetSection("EncryptionConfigs").GetSection("IsXhrEncryptionEnabled").Value))
                            {
                                var requestParts = JsonConvert.DeserializeObject<XhrRequestDataPartsViewModel>(queryStrToJson["requestData"]);
                                var privateKeyPath = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), "CommonUtiliteis", "RSA", "Keys")).Root + "Version_2_pri.key_L2L.pem";
                                var keys = RsaHelper.Decrypt(requestParts.K.Replace(" ","+"), privateKeyPath);

                                string reqCreatedOnStr = RijndaelManagedCryptoHandler.DecryptFromPlainTextToBase64(requestParts.T, keys.Split("|")[0], keys.Split("|")[1]);
                                if (DateTime.Now > Convert.ToDateTime(reqCreatedOnStr).AddMinutes(maxPayloadAgeInMinutes))
                                {
                                    context.Response.StatusCode = StatusCodes.Status408RequestTimeout;
                                    context.Response.ContentType = "application/json";
                                    await context.Response.WriteAsync("Request expired..!");
                                }
                                else
                                {

                                    string requestId = RijndaelManagedCryptoHandler.DecryptFromPlainTextToBase64(requestParts.I, keys.Split("|")[0], keys.Split("|")[1]);
                                    string sourceUrl = RijndaelManagedCryptoHandler.DecryptFromPlainTextToBase64(requestParts.C, keys.Split("|")[0], keys.Split("|")[1]);
                                    string decryptedLocations = RijndaelManagedCryptoHandler.DecryptFromPlainTextToBase64(requestParts.L, keys.Split("|")[0], keys.Split("|")[1]);
                                    string decryptedData = RijndaelManagedCryptoHandler.DecryptFromPlainTextToBase64(requestParts.D.Replace(" ", "+"), keys.Split("|")[0], keys.Split("|")[1]);
                                    string decryptedUserName = RijndaelManagedCryptoHandler.DecryptFromPlainTextToBase64(requestParts.U, keys.Split("|")[0], keys.Split("|")[1]);
                                    string decryptedProfileId = RijndaelManagedCryptoHandler.DecryptFromPlainTextToBase64(requestParts.P, keys.Split("|")[0], keys.Split("|")[1]);
                                    //string decryptedData = RijndaelManagedCryptoHandler.DecryptFromPlainTextToBase64(requestParts.D.Replace(" ", "+"), keys.Split("|")[0], keys.Split("|")[1]);
                                    var ipInfo = GetClientIP();
                                    await LogActivity(new UserApiActivityLog()
                                    {
                                        DestinationUrl = request.Path,
                                        IP = ipInfo.Result,
                                        Location = decryptedLocations.Split("|")[0] + "," + decryptedLocations.Split("|")[1],
                                        ProfileRefId = Convert.ToInt64(decryptedProfileId),
                                        RequestData = decryptedData,
                                        RequestId = requestId,
                                        RequestType = request.Method,
                                        ResponseData = null,
                                        SourceUrl = sourceUrl,
                                        TimeStemp = DateTime.Now,
                                        UserName = decryptedUserName
                                    });
                                    //string decryptedData = RijndaelManagedCryptoHandler.DecryptFromPlainTextToBase64(requestDataSet, _iConfiguration.GetSection("EncryptionConfigs").GetSection("FormDataEncryptionKey").Value, _iConfiguration.GetSection("EncryptionConfigs").GetSection("FormDataIVKey").Value);
                                    if (decryptedData != "null")
                                    {
                                        JObject parsed = JObject.Parse(decryptedData);
                                        var allParmsJsons = parsed.Children().ToList();
                                        StringBuilder sb = new StringBuilder("?");
                                        foreach (var item in allParmsJsons)
                                        {
                                            sb.Append(item.ToString().Replace("\"", "").Replace(":", "=").Replace(" ", "").Trim()).Append("&");
                                        }

                                        queryString = sb.ToString().TrimEnd('&');  // Remove the trailing '&'
                                    }
                                }
                            }
                            else
                            {
                                queryString = RijndaelManagedCryptoHandler.JsonToQueryString(queryStrToJson["d"]);
                            }
                        }
                        else
                        {
                            int a = 0;
                        }
                        request.QueryString = new QueryString(queryString);
                    }
                }
            }
            else
            {
                if (request.Path.Value.Contains("UploadDocument") || request.Path.Value.Contains("UploadAppAddendum") || request.Path.Value.Contains("signalrhub") || request.Path.Value.Contains("DeemedProcess/generateCertificates") || request.Path.Value.Contains("responseTerminal_IFMS") || request.Path.Value.Contains("responseTerminal_IFMS_NON_TREASURY") || request.Path.Value.Contains("setIsDirtyFlagForcefully") || request.Path.Value.Contains("getAppSubmissionDateByIpinAppId") || request.Path.Value.Contains("insert_complianceLogs") || request.Path.Value.Contains("AutoApproveProcess/generateCertificates") || request.Path.Value.Contains("hrms_GetHRMSCodeAndActWiseData") || request.Path.Value.Contains("hrms_GetHRMSCodeAndActWiseData_FactoryWing") || request.Path.Value.Contains("hrms_GetHRMSCodeAndActWiseData_ALCWing") || request.Path.Value.Contains("hrms_GetHRMSCodeAndActWiseData_LabourWing") || request.Path.Value.Contains("getTotalPendingApplications") || request.Path.Value.Contains("getTotalPendingApplicationsByServiceCode") || request.Path.Value.Contains("getApplicationCurrentStatusByIpinAndAppId") || request.Path.Value.Contains("getUpdatedApplicationsBetweenDates") || request.Path.Value.Contains("dormantApplications") || request.Path.Value.Contains("LogError") || request.Path.Value.Contains("getBFApplicationsLogs") || request.Path.Value.Contains("insertLWBMaster") || request.Path.Value.Contains("responseTerminal_HDFC"))
                {
                    // No need to decrypt data in case of document upload api call
                    request.Body.Position = 0;
                }
                else if (request.Path.Value.Contains("seed_BusinessFirstApprovedFiles"))
                {
                    streamReader = new StreamReader(request.Body, leaveOpen: true);
                    requestBody = await streamReader.ReadToEndAsync();
                    var requestObject = JsonConvert.DeserializeObject<XhrRequestDataViewModel>(requestBody);
                    if (requestObject != null)
                    {
                        string decryptedData = requestObject.RequestData;
                        if (Convert.ToBoolean(_iConfiguration.GetSection("EncryptionConfigs").GetSection("IsXhrEncryptionEnabled").Value))
                        {
                            decryptedData = RijndaelManagedCryptoHandler.DecryptFromPlainTextToBase64(requestObject.RequestData, _iConfiguration.GetSection("EncryptionConfigs").GetSection("FormDataEncryptionKey").Value, _iConfiguration.GetSection("EncryptionConfigs").GetSection("FormDataIVKey").Value);
                        }
                        var requestParms = JsonConvert.DeserializeObject<XhrRequestDataParmsViewModel>(decryptedData);
                        var designatedObject = JsonConvert.DeserializeObject(requestParms.Data, Type.GetType(requestParms.DesignatedModel));
                        Stream stream = new MemoryStream(Encoding.Default.GetBytes(JsonConvert.SerializeObject(designatedObject)));
                        request.Body = stream;
                    }
                    request.Body.Position = 0;
                }
                //else
                //{
                //    streamReader = new StreamReader(request.Body, leaveOpen: true);
                //    requestBody = await streamReader.ReadToEndAsync();
                //    var requestObject = JsonConvert.DeserializeObject<XhrRequestDataSetViewModel>(requestBody); //JsonConvert.DeserializeObject<XhrRequestDataViewModel>(requestBody);
                //    if (requestObject != null)
                //    {
                //        var requestParts = JsonConvert.DeserializeObject<XhrRequestDataPartsViewModel>(requestObject.RequestData);
                //        //string decryptedRequestData = requestObject.RequestData;
                //        string decryptedData = requestObject.RequestData;
                //        if (Convert.ToBoolean(_iConfiguration.GetSection("EncryptionConfigs").GetSection("IsXhrEncryptionEnabled").Value))
                //        {
                //            var keys = RsaHelper.Decrypt(requestParts.TokenPart);
                //            decryptedData = RijndaelManagedCryptoHandler.DecryptFromPlainTextToBase64(requestParts.DataPart, keys.Split("|")[0], keys.Split("|")[1]);
                //            //decryptedData = RijndaelManagedCryptoHandler.DecryptFromPlainTextToBase64(requestParts.EncryptedData, _iConfiguration.GetSection("EncryptionConfigs").GetSection("FormDataEncryptionKey").Value, _iConfiguration.GetSection("EncryptionConfigs").GetSection("FormDataIVKey").Value);
                //        }
                //        var requestParms = JsonConvert.DeserializeObject<XhrRequestDataParmsViewModel>(decryptedData);
                //        var designatedObject = JsonConvert.DeserializeObject(requestParms.Data, Type.GetType(requestParms.DesignatedModel));
                //        Stream stream = new MemoryStream(Encoding.Default.GetBytes(JsonConvert.SerializeObject(designatedObject)));
                //        request.Body = stream;
                //    }
                //    request.Body.Position = 0;
                //}
                else
                {
                    streamReader = new StreamReader(request.Body, leaveOpen: true);
                    requestBody = await streamReader.ReadToEndAsync();
                    var requestObject = JsonConvert.DeserializeObject<XhrRequestDataSetViewModel>(requestBody); //JsonConvert.DeserializeObject<XhrRequestDataViewModel>(requestBody);
                    if (requestObject != null)
                    {
                        var requestParts = JsonConvert.DeserializeObject<XhrRequestDataPartsViewModel>(requestObject.RequestData);
                        //string decryptedRequestData = requestObject.RequestData;
                        string decryptedData = requestObject.RequestData;
                        if (Convert.ToBoolean(_iConfiguration.GetSection("EncryptionConfigs").GetSection("IsXhrEncryptionEnabled").Value))
                        {
                            var privateKeyPath = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), "CommonUtiliteis", "RSA", "Keys")).Root + "Version_2_pri.key_L2L.pem";
                            var keys = RsaHelper.Decrypt(requestParts.K, privateKeyPath);

                            string reqCreatedOnStr = RijndaelManagedCryptoHandler.DecryptFromPlainTextToBase64(requestParts.T, keys.Split("|")[0], keys.Split("|")[1]);
                            if (DateTime.Now > Convert.ToDateTime(reqCreatedOnStr).AddMinutes(maxPayloadAgeInMinutes))
                            {
                                context.Response.StatusCode = StatusCodes.Status408RequestTimeout;
                                context.Response.ContentType = "application/json";
                                await context.Response.WriteAsync("Request expired..!");
                            }

                            string requestId = RijndaelManagedCryptoHandler.DecryptFromPlainTextToBase64(requestParts.I, keys.Split("|")[0], keys.Split("|")[1]);
                            string sourceUrl = RijndaelManagedCryptoHandler.DecryptFromPlainTextToBase64(requestParts.C, keys.Split("|")[0], keys.Split("|")[1]);
                            string decryptedLocations = RijndaelManagedCryptoHandler.DecryptFromPlainTextToBase64(requestParts.L, keys.Split("|")[0], keys.Split("|")[1]);
                            string decryptedUserName = RijndaelManagedCryptoHandler.DecryptFromPlainTextToBase64(requestParts.U, keys.Split("|")[0], keys.Split("|")[1]);
                            string decryptedProfileId = RijndaelManagedCryptoHandler.DecryptFromPlainTextToBase64(requestParts.P, keys.Split("|")[0], keys.Split("|")[1]);
                            string decryptedDataPart = RijndaelManagedCryptoHandler.DecryptFromPlainTextToBase64(requestParts.D, keys.Split("|")[0], keys.Split("|")[1]);
                            var ipInfo = GetClientIP();
                            await LogActivity(new UserApiActivityLog()
                            {
                                DestinationUrl = request.Path,
                                IP = ipInfo.Result,
                                Location = decryptedLocations.Split("|")[0] + "," + decryptedLocations.Split("|")[1],
                                ProfileRefId = Convert.ToInt64(decryptedProfileId),
                                RequestData = decryptedDataPart,
                                RequestId = requestId,
                                RequestType = request.Method,
                                ResponseData = null,
                                SourceUrl = sourceUrl,
                                TimeStemp = DateTime.Now,
                                UserName = decryptedUserName
                            });


                            decryptedData = RijndaelManagedCryptoHandler.DecryptFromPlainTextToBase64(requestParts.D, keys.Split("|")[0], keys.Split("|")[1]);
                            //decryptedData = RijndaelManagedCryptoHandler.DecryptFromPlainTextToBase64(requestParts.EncryptedData, _iConfiguration.GetSection("EncryptionConfigs").GetSection("FormDataEncryptionKey").Value, _iConfiguration.GetSection("EncryptionConfigs").GetSection("FormDataIVKey").Value);
                        }

                        Stream stream = null;
                        if (request.Path.Value.Contains("Crud"))
                        {
                            stream = new MemoryStream(Encoding.Default.GetBytes(decryptedData));
                        }
                        else
                        {
                            var requestParms = JsonConvert.DeserializeObject<XhrRequestDataParmsViewModel>(decryptedData);
                            var designatedObject = JsonConvert.DeserializeObject(requestParms.Data, Type.GetType(requestParms.DesignatedModel));
                            stream = new MemoryStream(Encoding.Default.GetBytes(JsonConvert.SerializeObject(designatedObject)));
                        }

                        request.Body = stream;
                    }
                    request.Body.Position = 0;
                }

            }
        }
        
        private async Task<bool> LogActivity(UserApiActivityLog activity)
        {
            using (var scope = _iServiceScopeFactory.CreateScope())
            {
                try
                {
                    var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
                    await dbContext.UserApiActivityLogs.AddAsync(activity);
                    await dbContext.SaveChangesAsync();
                    return true;
                }
                catch (Exception ex)

                {
                    return false;
                }
            }
        }

        private async Task<string> GetClientIP()
        {
            bool tryUseXForwardHeader = true;
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
                ip = "Unable to determine caller's IP.";
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


        private async Task<string> GetResponseAsTextAsync(HttpResponse response)
        {
            response.Body.Seek(0, SeekOrigin.Begin);
            //Create stream reader to write entire stream
            var text = await new StreamReader(response.Body).ReadToEndAsync();
            response.Body.Seek(0, SeekOrigin.Begin);

            return text;
        }

       
    }
}
