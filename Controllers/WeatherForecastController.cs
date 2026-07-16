using ClosedXML.Excel;
using Dapper;
using DocumentFormat.OpenXml.Spreadsheet;
using iTextSharp.text;
using iTextSharp.text.pdf;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using pbsamadhannetcoreapi.CommonUtiliteis;
using pbsamadhannetcoreapi.CommonUtiliteis.RSA;
using pbsamadhannetcoreapi.Models;
using pbsamadhannetcoreapi.Repositories;
using pbsamadhannetcoreapi.Repositories.Implementations;
using pbsamadhannetcoreapi.Services;
using pbsamadhannetcoreapi.Services.Implementations;
using pbsamadhannetcoreapi.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data;
using System.IdentityModel.Tokens.Jwt;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Web;
//using UIDAI;

namespace pbsamadhannetcoreapi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly ILogger<WeatherForecastController> _logger;
        private readonly IThirdPartyInegrationsService _dapper;
        private readonly ISystem_O_CommunicationService _iSystem_O_CommunicationService;
        private readonly INotificationManagerService _iNotificationManagerService;
        private readonly AppDbContext _context;
        private readonly IAppTimeLineManagerService _AppTimeLineManagerService;
        private readonly IGeneric_SP_Repository _iGeneric_SP_Repository;
        private readonly UserManager<User> _userManager;
        public IConfiguration _iConfiguration { get; }
        public WeatherForecastController(ILogger<WeatherForecastController> logger, 
            IThirdPartyInegrationsService dapper, 
            ISystem_O_CommunicationService iSystem_O_CommunicationService,
            INotificationManagerService iNotificationManagerService,
            AppDbContext context,
            IAppTimeLineManagerService appTimeLineManagerService,
             IGeneric_SP_Repository iGeneric_SP_Repository, IConfiguration iConfiguration,
             UserManager<User> userManager)
        {
            _logger = logger;
            _dapper = dapper;
            _iSystem_O_CommunicationService = iSystem_O_CommunicationService;
            _iNotificationManagerService = iNotificationManagerService;
            _context = context;
            _AppTimeLineManagerService = appTimeLineManagerService;
            _iGeneric_SP_Repository = iGeneric_SP_Repository;
            _userManager = userManager;
            _iConfiguration = iConfiguration;
        }

        [HttpGet]
        public async Task<IEnumerable<WeatherForecast>> Get()
        {
            //// Intentionally cause a DivideByZeroException
            //int numerator = 10;
            //int denominator = 0;
            //int result = numerator / denominator;

            //ExcelOperations<TestExcelFileFormat>.GenerateExcelUsingModel("worksheet1");
            //DigitalSignatureOperations.StempDigitalSignatureToFile();
            //string salt = SecurityHelper.GenerateSalt(16);

            //string hash = SecurityHelper.HashPassword("636a1ae3b5341025bb65b75823c6c95d24d68c13055570bf694412aac56dbdf0", salt, 1000, 32);

            //hash = SecurityHelper.HashPassword("abc", salt, 1000, 32);

            //Passord --Industry@123
            //636a1ae3b5341025bb65b75823c6c95d24d68c13055570bf694412aac56dbdf0
            //Salt
            //rI93o3GC0KZOhH7A+TX+8g==
            //Hash
            //mHwh59uDN9MAGOBITRYJkR3FWK62fgqAjBKhFUePd8A=

            //var data = new
            //{
            //    to = "dSFywDe-fEw:APA91bF_v20eTvhfdpPqxG3UVESPr9PYTF1KfI4c2kWeVck1y2riplXLghwnny1yVrwXdPZpxWb0-5u1CC9E92VYu9cLnxkKMUo89YGVKy0ByQPNGYc50bOPcm3Lqbo0oU4ZAbDiqICh",
            //    data = new
            //    {
            //        message = "Test Massage",
            //        name = "Pb Labour",
            //        userId = "1",
            //        status = true
            //    }
            //};


            //var serializer = JsonConvert.SerializeObject(data);
            //Byte[] byteArray= Encoding.UTF8.GetBytes(serializer);


            //string server_api_key = "AAAAwJTm94Y:APA91bEzttjbjk9vvcL-20cqtdOHDzlC8SHdIWVGc0Co2BS2EKoGj5CBb3Q1qC8twM-rONm38941Lc6Ly_wpBNH8DL1dv3JXW1O_1GqzyaxnhuRdzDLWD-ocaZHlKwg2j62TcBtR3LHV";
            //string sender_id = "827131885446";


            //WebRequest webRequest = WebRequest.Create("https://fcm.googleapis.com/fcm/send");
            //webRequest.Method = "post";
            //webRequest.ContentType = "application/json";
            //webRequest.Headers.Add($"Authorization: key={server_api_key}");
            //webRequest.Headers.Add($"Sender: id={sender_id}");

            //webRequest.ContentLength = byteArray.Length;
            //Stream stream = webRequest.GetRequestStream();
            //stream.Write(byteArray, 0, byteArray.Length);
            //stream.Close();

            //WebResponse webResponse = webRequest.GetResponse();
            //stream = webResponse.GetResponseStream();
            //StreamReader streamReader = new StreamReader(stream);

            //string responseFromServer = streamReader.ReadToEnd();
            //streamReader.Close();
            //stream.Close();
            //webResponse.Close();

            //var result = await _dapper.Get<TestDapperData>("[dbo].[sp_TestDapper]", new { });
            //var data = await _dapper.GetMultipleResultSets<SqlMapper.GridReader>("[dbo].[SP_TestMultipleRecordSet]", new { IPin = "220360909", ServiceCode = 6 }) ;
            //var clearence = data.ReadFirst<ProcessImd_ClearenceViewModel>();
            //var documemts = data.Read<ProcessImd_ClearencesDocumemtViewModel>();
            //var logs = data.Read<ProcessImd_ClearencesLogViewModel>();

            //var fees = data.Read<ProcessImd_ClearencesFeeViewModel>();

            // var kk = await _dapper.ImportAndSeedData("210958009", 5);

            // _iSystem_O_CommunicationService.CreateServiceMigrationLog(282060, 5, "SCF");

            //TemplateNewCaseLendedToOfficerNotificationViewModel vm = new TemplateNewCaseLendedToOfficerNotificationViewModel()
            //{
            //    OfficerName = "Panday",
            //    ApplicationPurposeTitle = EnumOps.GetEnumDescriptionName<ApplicationPurposeTypeEnum>(ApplicationPurposeTypeEnum.AMENDMENT_LICENCE.ToString()),
            //    ApplicationTypeTitle="dsfsdf",
            //    EstablishmentName="sdfssfsfsdsdf",
            //    PublicApplicationRefNo="123456TTT"
            //};

            //var emailTemplateText = await _iNotificationManagerService.RenderToStringAsync("EmailTemplates/_templateNewCaseLendedToOfficerNotification", vm);
            //var emailSendinfResponse = await _iNotificationManagerService.SendEmailSMTP(vm.OfficerName, "pravesh8822@gmail.com ", "Department of Labour: New application received (" + vm.PublicApplicationRefNo +")", emailTemplateText);

            //string exstingStr = "123";
            //if (exstingStr != null && exstingStr.Trim().Length > 0)
            //{
            //    exstingStr = exstingStr.Replace(" ", ",");
            //    string nicCodes = "[";
            //    foreach (var item in exstingStr.Split(','))
            //    {
            //        nicCodes = nicCodes + "\"" + item.Trim() + "\"" + ",";
            //    }


            //    nicCodes = nicCodes.Substring(0, nicCodes.Length - 1);
            //    nicCodes = nicCodes + "]";
            //}
            //else
            //{

            //}


            //["011","013"]

            //_context.Configuration.LazyLoadingEnabled = false



            // var tt = await _iGR_Student.GetAsync(x => x.Id == 1, null, x => x.StudentSubjects).ConfigureAwait(false);

            //var kk = _context.StudentSubject.Where(x => x.Student.Id == 2).AsNoTracking().ToList();

            //var kk = _context.Students.Where(x => x.Id == 1).Select(x => { x.StudentSubjects.FirstOrDefault().AsNoTracking().ToList(); return x; }).
            // var kk = tt.Select(x => x.StudentSubjects).FirstOrDefault();
            //var formModel = GenericModelOps<StudentSubject>.SetNullAllNevigationProperties(kk);


            ////variables
            //string pathin = @"C:\Users\User\Documents\dummy.pdf";
            //string pathout = @"C:\Users\User\Documents\semped.pdf"; ;

            ////create PdfReader object to read from the existing document
            //using (PdfReader reader = new PdfReader(pathin))
            ////create PdfStamper object to write to get the pages from reader 
            //using (PdfStamper stamper = new PdfStamper(reader, new FileStream(pathout, FileMode.Create)))
            //{
            //    //select two pages from the original document
            //    reader.SelectPages("1-2");

            //    //gettins the page size in order to substract from the iTextSharp coordinates
            //    var pageSize = reader.GetPageSize(1);

            //    // PdfContentByte from stamper to add content to the pages over the original content
            //    PdfContentByte pbover = stamper.GetOverContent(1);

            //    //add content to the page using ColumnText
            //    iTextSharp.text.Font font = FontFactory.GetFont("Alice", BaseFont.IDENTITY_H, 16);

            //    //setting up the X and Y coordinates of the document
            //    int x = 20;
            //    int y = 22;

            //    y = (int)(pageSize.Height - y);

            //    ColumnText.ShowTextAligned(pbover, Element.ALIGN_CENTER, new Phrase("Hellooooooooooo ooooooooooooo xxxxxxxxxxxx", font), x, y, 0);
            //}



            //List<EstablishmentConstitutionTypeEnum> toDotasks = new List<EstablishmentConstitutionTypeEnum>();
            //toDotasks.Add(EstablishmentConstitutionTypeEnum.COOPERATIVE);

            //foreach (var item in toDotasks)
            //{
            //    Console.WriteLine(item);
            //}


            //var data = RijndaelManagedCryptoHandler.EncryptFromPlainTextToBase64("{\"iPin\":250137916,\"appId\":2503504129,\"serviceCode\":5,\"categoryType\":\"WD\"", "94c8e24f-6c5e-4d", "70k6e44y-9z5q-9q");
            //var origibal = RijndaelManagedCryptoHandler.DecryptFromPlainTextToBase64(data, "94c8e24f-6c5e-4d", "70k6e44y-9z5q-9q");



            // await _AppTimeLineManagerService.SeedTimeLineData(147256);

            //int i = 1;
            //var appActionExt = _context.ApplicationActionExtensions.Where(x => x.AppActionRefId == 147279).FirstOrDefault();
            //var kk = appActionExt.GetType().GetProperty("Receiver_UserRefId_" + i.ToString()).GetValue(appActionExt);


            //var receiverBackups = JsonConvert.DeserializeObject<List<ReceiverBackupViewModel>>("[{\"Receiver_ProfileRefId\":5,\"Receiver_UserRefId\":\"7EF39BF3-1EB0-4F79-8246-94A25207E225\",\"ReceiverRoleId\":\"38D54781-39A2-4AF6-87EE-3102431EAB16\",\"Receiver_ActionOn\":\""+ DateTime.Now + "\",\"Receiver_Remarks\":\"SC-Completed\",\"IsCurrentReceiver\":true},{\"Receiver_ProfileRefId\":306,\"Receiver_UserRefId\":\"9F30336C-55AF-40F8-BF4D-AD1F527F31A5\",\"ReceiverRoleId\":\"A9CE9875-D23D-4AA2-8271-6A8799EEF0E3\",\"Receiver_ActionOn\":\"" + DateTime.Now + "\",\"Receiver_Remarks\":\"Sir, Please do the needful\",\"IsCurrentReceiver\":false},{\"Receiver_ProfileRefId\":0,\"Receiver_UserRefId\":\"\",\"ReceiverRoleId\":\"\",\"Receiver_ActionOn\":\"" + DateTime.Now + "\",\"Receiver_Remarks\":\"\",\"IsCurrentReceiver\":false},{\"Receiver_ProfileRefId\":305,\"Receiver_UserRefId\":\"c75c0092-e0fe-4459-a469-646e311a7be2\",\"ReceiverRoleId\":\"592add3e-f992-4983-a8ca-21ddc090bda0\",\"Receiver_ActionOn\":\"" + DateTime.Now + "\",\"Receiver_Remarks\":\"SC - Completed\",\"IsCurrentReceiver\":true}]");
            //receiverBackups = receiverBackups.Where(x => (x.Receiver_UserRefId != "9F30336C-55AF-40F8-BF4D-AD1F527F31A5")).ToList();
            //receiverBackups = receiverBackups.Where(x => (x.Receiver_UserRefId != null)).ToList();
            //receiverBackups = receiverBackups.Where(x => (x.Receiver_UserRefId != String.Empty)).ToList();
            //receiverBackups = receiverBackups.Where(x => (x.IsCurrentReceiver)).ToList();

            //.Where(x => (x.Receiver_UserRefId != "9F30336C-55AF-40F8-BF4D-AD1F527F31A5")).ToList();



            //.Where(x => (x.Receiver_UserRefId != null)).ToList()
            //        .Where(x => (x.Receiver_UserRefId != "9F30336C-55AF-40F8-BF4D-AD1F527F31A5" && !x.IsCurrentReceiver)).ToList()
            //        .Where(x => (x.Receiver_UserRefId != String.Empty)).ToList();


            //appActionExtensionLog.AppActionLogRefId = applicationActionlogs.ApplicationActionLogId



            //await _AppTimeLineManagerService.SeedTimeLineData(455065, 0);



            //var id = _context.EmpUniqueId.Where(x => x.IsVerified == 1).ToList();
            //foreach (var item in id)
            //{
            //    var k = AadharOps.ValidateVerhoeff(item.UniqueID);
            //    if (k == true)
            //    {
            //        item.IsVerified = 2;
            //    }
            //    else
            //    {
            //        item.IsVerified = 3;
            //    }
            //    _context.SaveChanges();
            //}



            // await _AppTimeLineManagerService.SeedTimeLineData(437451, 0);

            //try
            //{
            //    int numerator = 10;
            //    int denominator = 0;
            //    int result = numerator / denominator;
            //}
            //catch (Exception ex)
            //{
            //    throw ex;
            //}


            //var result = await _userManager.PasswordValidators[0].ValidateAsync(_userManager, null, "");

            //if (result.Succeeded)
            //{
            //    var kk = new List<string> { "Password meets policy requirements." };
            //}
            //else
            //{
            //    // Return a list of all errors
            //    var kk = result.Errors.Select(e => e.Description).ToList();
            //}

            //var publicKeyPath = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), "CommonUtiliteis", "RSA", "Keys")).Root + "Version_1_pub.key_L2LWB.pem";

            //var kk = JWTOperations.PrepareTokenForPartnerPortal(new List<TokenClaimPairViewModel>()
            //{
            //    new TokenClaimPairViewModel() { ClaimName="UserName", ClaimValue="Leo.Mohali"},
            //    new TokenClaimPairViewModel() { ClaimName="UserId", ClaimValue="97be9692-8955-40f5-a63c-818cc706deee"},
            //    new TokenClaimPairViewModel() { ClaimName="UserFullName", ClaimValue="Ajay Kumar"},

            //    new TokenClaimPairViewModel() { ClaimName="RoleId", ClaimValue="60c208bd-100d-442a-bb3b-a677e9a95877"},
            //    new TokenClaimPairViewModel() { ClaimName="RoleCode", ClaimValue="ADDF"},
            //    new TokenClaimPairViewModel() { ClaimName="RoleName", ClaimValue="Labour Inspector"},
            //    new TokenClaimPairViewModel() { ClaimName="ProfileId", ClaimValue="59"},

            //    new TokenClaimPairViewModel() { ClaimName="LabourCircleId", ClaimValue="10071"},
            //    new TokenClaimPairViewModel() { ClaimName="FactoryCircleId", ClaimValue=null},
            //    new TokenClaimPairViewModel() { ClaimName="AlcCircleId", ClaimValue=null},


            //}, publicKeyPath,
            //_iConfiguration.GetSection("EncryptionConfigs").GetSection("HandshakeKey_L2LWB").Value,
            //_iConfiguration.GetSection("EncryptionConfigs").GetSection("SigningKey_L2LWB").Value,
            //_iConfiguration.GetSection("EncryptionConfigs").GetSection("SharedIssuerCode_L2LWB").Value,
            //_iConfiguration.GetSection("EncryptionConfigs").GetSection("SharedAudienceCode_L2LWB").Value);


            //var privateKeyPath = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), "CommonUtiliteis", "RSA", "Keys")).Root + "Version_1_pri.key_L2LWB.pem";

            //var isValid=  JWTOperations.ValidateParterPortalToken(kk,
            //    _iConfiguration.GetSection("EncryptionConfigs").GetSection("SharedIssuerCode_L2LWB").Value,
            //    _iConfiguration.GetSection("EncryptionConfigs").GetSection("SharedAudienceCode_L2LWB").Value,
            //    _iConfiguration.GetSection("EncryptionConfigs").GetSection("SigningKey_L2LWB").Value,
            //    privateKeyPath,
            //    _iConfiguration.GetSection("EncryptionConfigs").GetSection("HandshakeKey_L2LWB").Value);



            //            var requestData = RijndaelManagedCryptoHandler.EncryptFromPlainTextToBase64("{\"userName\" : \"alc.mohali\", \"password\":\"Test123#\"}",
            //                    _iConfiguration.GetSection("EncryptionConfigs").GetSection("MyOfficeEncryptionKey").Value,
            //                    _iConfiguration.GetSection("EncryptionConfigs").GetSection("MyOfficeIVKey").Value);

            //            var requestData12 = RijndaelManagedCryptoHandler.DecryptFromPlainTextToBase64("n812+hN8rY7qC6/LqlkI2kLAGCSfgjRdyCXTl/hEp4gIZOmVPRnGoXpUbiqo0ClY4+8/yWXy8fG9m+0+cHsXsFR5ksUa/4S1Tgyry3QROYs="
            //,
            //                    _iConfiguration.GetSection("EncryptionConfigs").GetSection("MyOfficeEncryptionKey").Value,
            //                    _iConfiguration.GetSection("EncryptionConfigs").GetSection("MyOfficeIVKey").Value);


            var rng = new Random();
            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateTime.Now.AddDays(index),
                TemperatureC = rng.Next(-20, 55),
                Summary = Summaries[rng.Next(Summaries.Length)]
            })
            .ToArray();
        }




        //public async Task<string> GenerateUniquePaymentGatewayTransactionId(Int64 appRefId)
        //{
        //    StringBuilder UniquePaymentGatewayTransactionId = new StringBuilder();
        //    string[] alphaCodes = { "0", "Z", "A", "X", "H", "V", "K", "T", "C", "R", "Q", "L" };
        //    var appRefIdAsChars = (appRefId.ToString()).ToCharArray();
        //    foreach (var item in appRefIdAsChars)
        //    {
        //        UniquePaymentGatewayTransactionId.Append(alphaCodes[Convert.ToInt32(item.ToString())]);
        //    }
        //    UniquePaymentGatewayTransactionId.Append("_");
        //    List<StoreProcedureParm> storeProcedureParms = new List<StoreProcedureParm>()
        //        {
        //            new StoreProcedureParm (){ ParmName="AppRefId", ParmValue= appRefId.ToString(), isNumber=true}
        //        };
        //    List<AppFeeTransaction> allTransactions = await _iGeneric_SP_Repository.CallStoreProcedureReaderAsync<AppFeeTransaction>("dbo.sp_Payments_Get_All_App_Fee_Transactions", storeProcedureParms);
        //    UniquePaymentGatewayTransactionId.Append("T" + (allTransactions.Count() + 1).ToString());
        //    int lengthRequiredDiff = 15 - (UniquePaymentGatewayTransactionId.ToString().Length);
        //    if (lengthRequiredDiff > 0)
        //    {
        //        string zeros = "QTXYFBAMKDWHLPI";
        //        UniquePaymentGatewayTransactionId = UniquePaymentGatewayTransactionId.Replace("_", zeros.Substring(0, lengthRequiredDiff + 1));
        //    }
        //    else
        //    {
        //        UniquePaymentGatewayTransactionId = UniquePaymentGatewayTransactionId.Replace("_", "0");
        //    }


        //    return UniquePaymentGatewayTransactionId.ToString();
        //}




    }
    
}
