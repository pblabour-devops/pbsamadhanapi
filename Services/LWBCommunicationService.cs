using Dapper;
using DocumentFormat.OpenXml.Drawing.Charts;
using DocumentFormat.OpenXml.InkML;
using DocumentFormat.OpenXml.Spreadsheet;
using EFCore.BulkExtensions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Primitives;
using Microsoft.IdentityModel.Tokens;
using Microsoft.VisualBasic.FileIO;
using Newtonsoft.Json;
using Org.BouncyCastle.Asn1.Ocsp;
using pbsamadhannetcoreapi.CommonUtiliteis;
using pbsamadhannetcoreapi.Models;
using pbsamadhannetcoreapi.Repositories.Implementations;
using pbsamadhannetcoreapi.Services.Implementations;
using pbsamadhannetcoreapi.ViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data;
using System.Data.SqlClient;
using System.IdentityModel.Tokens.Jwt;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Security.Policy;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Text.Json;
using System.Globalization;
using ClosedXML.Excel;
using MiniValidation;
using DocumentFormat.OpenXml.Wordprocessing;
using pbsamadhannetcoreapi.CommonUtiliteis.CustomeAttributes;


namespace pbsamadhannetcoreapi.Services
{
    public class LWBCommunicationService : ILWBCommunicationService
    {
        private readonly AppDbContext _context;
        private readonly IGeneric_SP_Repository _iGeneric_SP_Repository;
        private readonly IDbConnection _db;
        private readonly ICommonApisService _iCommonApisService;
        public IConfiguration Configuration { get; }
        private readonly IAuthService _iAuthService;
        public LWBCommunicationService(AppDbContext context, IConfiguration configuration, IOptions<CustomAppConfigsModel> appSettings, IGeneric_SP_Repository iGeneric_SP_Repository, IAuthService iAuthService, ICommonApisService iCommonApisService)
        {
            _context = context;
            Configuration = configuration;
            _iGeneric_SP_Repository = iGeneric_SP_Repository;
            _db = new SqlConnection(configuration.GetConnectionString("SQLServerConnection_Logger"));
            _iAuthService = iAuthService;
            _iCommonApisService = iCommonApisService;
        }

        public async Task<GenericResponseTemplateModel<string>> GetAuthToken(string rawToken)
        {
            GenericResponseTemplateModel<string> genericFormModel = new GenericResponseTemplateModel<string>();
            try
            {
                var encryptionConfigs = Configuration.GetSection("EncryptionConfigs");
                var encryptionKey = encryptionConfigs.GetSection("MobileAppDataEncryptionKey").Value;
                var ivKey = encryptionConfigs.GetSection("MobileAppDataIVKey").Value;

                var decryptedPayload = RijndaelManagedCryptoHandler.DecryptFromPlainTextToBase64(rawToken, encryptionKey, ivKey);
                var serializedData = JsonConvert.DeserializeObject<RawTokenSerializedDataViewModel>(decryptedPayload);

                if (serializedData.ProjectId == "LWB01")
                {
                    var userDetailPayload = new
                    {
                        projectId = serializedData.ProjectId,
                        dataCode = serializedData.DataCode,
                        tokenTime = DateTime.Now,
                        expireOn = DateTime.Now.AddSeconds(30)
                    };
                    string userDetailPayloadJsonText = JsonConvert.SerializeObject(userDetailPayload);

                    genericFormModel.ResponseDataModel = RijndaelManagedCryptoHandler.EncryptFromPlainTextToBase64(userDetailPayloadJsonText, encryptionKey, ivKey);
                    //authTokenViewModel.Data = encryptedPayload;
                }
                else
                {
                    genericFormModel.HasError = true;
                    genericFormModel.ErrorDesc = "Invalid Token";
                }

            }
            catch (Exception ex)
            {
                genericFormModel.HasError = true;
                genericFormModel.ErrorDesc = ex.Message;
            }
            return genericFormModel;
        }


        public async Task<GenericResponseTemplateModel<List<LWBApplicationDetailsViewModel>>> GetApplicationDetails(string licenceNumber, string authToken)
        {
            GenericResponseTemplateModel<List<LWBApplicationDetailsViewModel>> genericFormModel = new GenericResponseTemplateModel<List<LWBApplicationDetailsViewModel>>();
            try
            {
                var encryptionConfigs = Configuration.GetSection("EncryptionConfigs");
                var encryptionKey = encryptionConfigs.GetSection("MobileAppDataEncryptionKey").Value;
                var ivKey = encryptionConfigs.GetSection("MobileAppDataIVKey").Value;

                var decryptedPayload = RijndaelManagedCryptoHandler.DecryptFromPlainTextToBase64(authToken, encryptionKey, ivKey);
                var serializedData = JsonConvert.DeserializeObject<AuthTokenSerializedDataViewModel>(decryptedPayload);

                if (serializedData.ProjectId == "LWB01")
                {
                    if (serializedData.expireOn > DateTime.Now)
                    {
                        List<StoreProcedureParm> storeProcedureParms = new List<StoreProcedureParm>()
                            {
                                new StoreProcedureParm (){ ParmName="licenceNumber", ParmValue=licenceNumber.ToString(), isNumber=false}
                            };
                        genericFormModel.ResponseDataModel = await _iGeneric_SP_Repository.CallStoreProcedureReaderAsync<LWBApplicationDetailsViewModel>("sp_GetLWBEstablishmentDetails", storeProcedureParms);

                        if (genericFormModel.ResponseDataModel.Count == 0)
                        {
                            genericFormModel.HasError = true;
                            genericFormModel.ErrorDesc = "Data not found";
                        }
                    }
                    else
                    {
                        genericFormModel.HasError = true;
                        genericFormModel.ErrorDesc = "Token time expired";
                    }

                }
                else
                {
                    genericFormModel.HasError = true;
                    genericFormModel.ErrorDesc = "Invalid Token";
                }
                return genericFormModel;
            }
            catch (Exception ex)
            {
                genericFormModel.HasError = true;
                genericFormModel.ErrorDesc = ex.Message;
            }
            return genericFormModel;
        }

        public async Task<GenericServiceResultTemplate> ErrorLogsParams(ErrorLogParamsViewModel formModel)
        {
            GenericServiceResultTemplate genericServiceResultTemplate = new GenericServiceResultTemplate();

            try
            {
                var query = @"INSERT INTO ErrorLogs( ErrRoute,ErrDesc,ErrException, ErrIP, ErrDate, ProjectModuleId,Username,  RequestParameter)

               VALUES( @ErrRoute, @ErrDesc,  @ErrException, @ErrIP, @ErrDate,@ProjectModuleId,@Username, @RequestParameter)";

                var parameters = new
                {
                    formModel.ErrRoute,
                    formModel.ErrDesc,
                    formModel.ErrException,
                    formModel.ErrIP,
                    ErrDate = DateTime.Now,
                    formModel.ProjectModuleId,
                    formModel.Username,
                    formModel.RequestParameter
                };

                var command = new CommandDefinition(
                                commandText: query,
                                parameters: parameters,
                                commandType: CommandType.Text,
                                commandTimeout: 60
                                );

                await _db.ExecuteAsync(command);
               
            }
            catch (Exception ex)
            {
                genericServiceResultTemplate.HasException = true;
                genericServiceResultTemplate.Exceptions = ex;
            }

            return genericServiceResultTemplate;
        }
        public async Task<GenericResponseTemplateModel<string>> GetRedirectTokenWelfareBoard(string userId, string licenceNumber, int pwbCessCollectionId, int type)
        {
            GenericResponseTemplateModel<string> genericFormModel = new GenericResponseTemplateModel<string>();
            List<EstablishmentAndUserDetailViewModel> establishmentAndUserDetails = new List<EstablishmentAndUserDetailViewModel>();
            try
            {
                var userDetails = await _iAuthService.GetUserLoginDeatilByUserId(userId);
                var profileDetails =  await _iAuthService.GetUserProfileByUserId(userId);
                Guid newGuid = Guid.NewGuid();

                if(licenceNumber != null) {
                    List<StoreProcedureParm> storeProcedureParms = new List<StoreProcedureParm>()
                        {
                          new StoreProcedureParm() { ParmName = "licenceNumber", ParmValue = licenceNumber.ToString(), isNumber = false }
                        };
                     establishmentAndUserDetails = await _iGeneric_SP_Repository.CallStoreProcedureReaderAsync<EstablishmentAndUserDetailViewModel>("sp_GetEstablishmentAndUserDetailsByLicenceNumber", storeProcedureParms);
                }
                



                var encryptionConfigs = Configuration.GetSection("OtherPortelUrl");
                var returnFromLWBPortal = encryptionConfigs.GetSection("EPortalRetURL").Value;
                // var returnFromLWBPortal = Configuration.GetSection("OtherPortelUrl").GetSection("EPortalRetURL").Value;
                var key = Configuration.GetSection("EncryptionConfigs").GetSection("BPToken").Value;
                string DT = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                List<StoreProcedureParm> storeProcedureParms2 = new List<StoreProcedureParm>()
                    {
                        new StoreProcedureParm (){ ParmName="UserId", ParmValue=userDetails.UserId.ToString(), isNumber=false},
                        new StoreProcedureParm (){ ParmName="Token", ParmValue=newGuid.ToString(), isNumber=false},
                        new StoreProcedureParm (){ ParmName="SwitchPortal", ParmValue="Welfare", isNumber=false},
                        new StoreProcedureParm (){ ParmName="RouteActivity", ParmValue="Out", isNumber=false}
                    };
                await _iGeneric_SP_Repository.CallStoreProcedureReaderScalerAsync("dbo.sp_Legacy_InsertUserPortalSwitchLogs", storeProcedureParms2);
                string userList = null;
                //var userList = (userDetails.UserId + "|" + userDetails.UserName + "|" + userDetails.RoleName + "|" + DT + "|" + "defaultpage" + "|NA|" + "https://pblabour.gov.in/ePortal" + "|" + newGuid);
                if (establishmentAndUserDetails.Count() > 0)
                {

                     //userList = (establishmentAndUserDetails.FirstOrDefault().LicenceNumber + "|" + pwbCessCollectionId);

                    //userList = (establishmentAndUserDetails.FirstOrDefault().LicenceNumber + "|" + pwbCessCollectionId);

                    userList = (userDetails.UserId + "|" + userDetails.UserName + "|" + userDetails.RoleName + "|" + DT + "|" + "defaultpage" + "|NA|" + "https://pblabour.gov.in/ePortal" + "|" + newGuid + "|" + establishmentAndUserDetails.FirstOrDefault().MobileNo + "|" + establishmentAndUserDetails.FirstOrDefault().Email + "|" + profileDetails.FirstName + "|" + profileDetails.LastName + "|" + establishmentAndUserDetails.FirstOrDefault().LicenceNumber + "|" + establishmentAndUserDetails.FirstOrDefault().EstablishmentName + "|" + establishmentAndUserDetails.FirstOrDefault().Address + "|" + establishmentAndUserDetails.FirstOrDefault().FactoryCircleRefId + "|" + establishmentAndUserDetails.FirstOrDefault().AlcCircleRefId + "|" + establishmentAndUserDetails.FirstOrDefault().LabourCircleRefId + "|" + establishmentAndUserDetails.FirstOrDefault().ProjectPurpose + "|" + establishmentAndUserDetails.FirstOrDefault().AlternateEmail + "|" + establishmentAndUserDetails.FirstOrDefault().AlternateMobileNo + "|" + establishmentAndUserDetails.FirstOrDefault().InvestPunjab_Ipin + "|" + establishmentAndUserDetails.FirstOrDefault().DistrictRefId + "|" + establishmentAndUserDetails.FirstOrDefault().TehsilRefId + "|" + pwbCessCollectionId + "|" + type);
                }
                else
                {

                    userList = (userDetails.UserId + "|" + userDetails.UserName + "|" + userDetails.RoleName + "|" + DT + "|" + "defaultpage" + "|NA|" + "https://pblabour.gov.in/ePortal" + "|" + newGuid + "|" + profileDetails.MobileNo + "|" + profileDetails.Email + "|" + profileDetails.FirstName + "|" + profileDetails.LastName + "|" + type);

                }

                var redirectUrl = Configuration.GetSection("ThirdPartyIntegrationConfigs").GetSection("OtherPortelUrl").GetSection("LWBSRedirectURL").Value;
                string encurl = MPRRedirectEncry.Encrypt(userList, Configuration.GetSection("EncryptionConfigs").GetSection("BOCWToken").Value);
                 var url = redirectUrl + "?msg=" + MPRRedirectEncry.EncryptingURL(encurl, Configuration.GetSection("EncryptionConfigs").GetSection("BOCWToken").Value);
                genericFormModel.ResponseDataModel = url;

            }
            catch (Exception ex)
            {
                genericFormModel.HasError = true;
                genericFormModel.ErrorDesc = ex.Message;
            }
            return genericFormModel;
        }

        public async Task<GenericServiceResultTemplate> InsertLWBMaster(
     LWBFormRequestViewModel formModel,
     IFormFileCollection files)
        {
            var genericResponseTemplateModel = new GenericServiceResultTemplate();
            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                Application application = null;
                LWB_FundMaster fundMaster = null;

                if (formModel.Id == 0)
                {
                    application = new Application
                    {
                        ApplicationType = ApplicationTypeEnum.LABOUR_SERVICES,
                        ApplicationPurposeType = formModel.ServiceType == 1
                            ? ApplicationPurposeTypeEnum.LWB_CONTRIBUTION
                            : ApplicationPurposeTypeEnum.UNPAID_WAGES,
                        PublicAppRefNum = "",
                        IterationCount = 1,
                        CreatedOnDate = DateTime.Now,
                        LastModifiedOnDate = DateTime.Now,
                        IsEnabled = true,
                        IsDeleted = false,
                        IsDigitalSignatureRequired = false,
                        IsDigitalSignatureVerified = false,
                        IsLocked = false,
                        IsAllowEdit = true,
                        IsFeeApplicable = false,
                        ApplicationLifeCycleStatusType = 0,
                        ApplicationLifeCycleLastStatusOn = DateTime.Now,
                        PaymentBatchCounter = 1,
                        Legacy_IsMigrated = false,
                        Legacy_AppId = 0,
                        Legacy_AppFormId = 0,
                        Legacy_NAR = "N/A",
                        InvestPunjab_Ipin = "0",
                        InvestPunjab_AppId = 0,
                        Legacy_LicenceNo = formModel.LicenceNumber,
                        ProjectSiteVersion = 1,
                        IsIPIntegrated = false,
                        IsTimeLineFlow = false,
                        ProjectSiteRefId = formModel.ProjectSiteRefId,
                        DigitalSignatureRefId = null,
                        DepartmentRefID = 1
                    };

                    _context.Applications.Add(application);
                   var  appPurposeTypeCode = application.ApplicationPurposeType == ApplicationPurposeTypeEnum.LWB_CONTRIBUTION ? "F" : application.ApplicationPurposeType == ApplicationPurposeTypeEnum.UNPAID_WAGES ? "U" : "A";
                    application.PublicAppRefNum = "LS" + appPurposeTypeCode + application.IterationCount.ToString() + DateTime.Now.ToString("yy") + DateTime.Now.ToString("MM") + DateTime.Now.ToString("dd") + application.AppId.ToString("D8");
                    _context.Entry(application).State = EntityState.Modified;
                    await _context.SaveChangesAsync();

                    fundMaster = new LWB_FundMaster
                    {
                        LicenceNo = formModel.LicenceNumber,
                        LWBSlabType = (LWBSlabTypeEnum)Convert.ToInt32(formModel.TimeSlot),
                        Year = formModel.FinancialYear,
                        LWBAadhaarVerficationStatusType =
                            LWBAadhaarverificationStatusTypeEnum.PENDING_MODEL_VALIDATION,
                        FileName = formModel.AppDocId,
                        AppRefId = application.AppId,
                        Type = formModel.ServiceType,
                        CreatedOn = DateTime.Now
                    };

                    _context.LWB_FundMaster.Add(fundMaster);
                    await _context.SaveChangesAsync();
                }


                if (application == null)
                {
                    application = _context.LWB_FundMaster.Where(x => x.Id == formModel.Id).Select(x => x.Application).FirstOrDefault();
                    fundMaster = _context.LWB_FundMaster.Where(x => x.Id == formModel.Id).FirstOrDefault();
                }
                if (files != null && files.Any())
                {


                    if (formModel.DocId == null || application == null || fundMaster == null)
                    {
                        genericResponseTemplateModel.HasException = true;
                        return genericResponseTemplateModel;
                    }

                    string docId = formModel.DocId.ToString();
                    string appId = application.AppId.ToString();
                    string appDocId = formModel.AppDocId.ToString();

                    string rootPath = Path.Combine(
                        Directory.GetCurrentDirectory(),
                        "wwwroot",
                        "AppFiles");

                    foreach (var file in files)
                    {
                        var data = await _iCommonApisService.LogFileInfo(
                            application.AppId,
                            Convert.ToInt64(docId),
                            Convert.ToInt64(appDocId),
                            Path.GetExtension(file.FileName));

                        if (data.HasError || data.ResponseDataModel == null)
                            continue;

                        string savedFilePath = Path.Combine(
                            rootPath,
                            data.ResponseDataModel.FileName);

                        using (var fs = new FileStream(
                            savedFilePath,
                            FileMode.Create))
                        {
                            await file.CopyToAsync(fs);
                        }
                      
                        if (formModel.ServiceType == 1)
                        {

                            

                            List<LWB_Employees_Fund> employees = new List<LWB_Employees_Fund>();

                            try
                            {
                                if (!System.IO.File.Exists(savedFilePath))
                                    throw new Exception($"File not found: {savedFilePath}");
                                var allLines = await File.ReadAllLinesAsync(savedFilePath);

                                Console.WriteLine($"Lines: {allLines.Length}");
                                Console.WriteLine($"Employees: {employees.Count}");

                                using (var reader = new StreamReader(savedFilePath))
                                {
                                    // Read Header
                                    var headerLine = await reader.ReadLineAsync();

                                    if (string.IsNullOrWhiteSpace(headerLine))
                                        throw new Exception("CSV file is empty.");

                                    var csvHeaders = headerLine
                                        .Split(',')
                                        .Select(x => x.Trim())
                                        .ToList();

                                    var modelHeaders = typeof(LWBCsvEmployeeModel)
                                        .GetProperties()
                                        .Select(x => x.Name)
                                        .ToList();

                                    // Check Column Count
                                    List<string> validationErrors = new List<string>();

                                    // Missing Headers
                                    var missingHeaders = modelHeaders
                                        .Except(csvHeaders, StringComparer.OrdinalIgnoreCase)
                                        .ToList();

                                    if (missingHeaders.Any())
                                    {
                                        validationErrors.Add($"Missing Columns: {string.Join(", ", missingHeaders)}");
                                    }

                                    // Extra Headers
                                    var extraHeaders = csvHeaders
                                        .Except(modelHeaders, StringComparer.OrdinalIgnoreCase)
                                        .ToList();

                                    if (extraHeaders.Any())
                                    {
                                        validationErrors.Add(
                                            $"Unexpected Columns: {string.Join(", ", extraHeaders)}");
                                    }

                                    // Only check order when there are no missing/extra columns
                                    if (!missingHeaders.Any() && !extraHeaders.Any())
                                    {
                                        for (int i = 0; i < modelHeaders.Count; i++)
                                        {
                                            if (!string.Equals(
                                                    modelHeaders[i],
                                                    csvHeaders[i],
                                                    StringComparison.OrdinalIgnoreCase))
                                            {
                                                validationErrors.Add(
                                                    $"Column {i + 1}: Expected '{modelHeaders[i]}' but found '{csvHeaders[i]}'");
                                            }
                                        }
                                    }

                                    if (validationErrors.Any())
                                    {
                                        await transaction.RollbackAsync();
                                        genericResponseTemplateModel.Exceptions = new Exception(
                                            "Invalid CSV template.\n\n" +
                                            string.Join("\n", validationErrors) +
                                            "\n\nPlease download and use the latest CSV template."
                                        );

                                        return genericResponseTemplateModel;
                                    }

                                    // Read Data Rows
                                    while (!reader.EndOfStream)
                                    {
                                        var line = await reader.ReadLineAsync();

                                        if (string.IsNullOrWhiteSpace(line))
                                            continue;

                                        if (line.Trim() == "\"\"")
                                            continue;

                                        var columns = line.Split(',');

                                        if (columns.Length < modelHeaders.Count)
                                            continue;

                                        if (string.IsNullOrWhiteSpace(columns[0]))
                                            continue;

                                        employees.Add(new LWB_Employees_Fund
                                        {
                                            FundMasterRefId = fundMaster.Id,
                                            Name = columns[0]?.Trim(),
                                            Aadhaar = columns[3]?.Trim(),
                                            VerificationFormatType = AadharVerificationFormatTypeEnum.PENDING,
                                            VerificationDemographicType = AadharVerificationDemographicTypeEnum.PENDING,
                                            Description = "",
                                            FatherOrHusbandName = columns[1]?.Trim(),
                                            Gender = columns[2]?.Trim(),
                                            Mobile = columns[4]?.Trim(),
                                            BankAccountNo = columns[5]?.Trim(),
                                            BankName = columns[6]?.Trim(),
                                            IFSCCode = columns[7]?.Trim(),
                                            ESICNo = columns[8]?.Trim(),
                                            PFNo = columns[9]?.Trim(),
                                            DOB = string.IsNullOrWhiteSpace(columns[10]) ? (DateTime?)null : Convert.ToDateTime(columns[10].Trim()),
                                            DOJ = string.IsNullOrWhiteSpace(columns[11]) ? (DateTime?)null : Convert.ToDateTime(columns[11].Trim()),
                                            DOR = string.IsNullOrWhiteSpace(columns[12]) ? (DateTime?)null : Convert.ToDateTime(columns[12].Trim()),
                                            Location = columns[13]?.Trim(),
                                            Nationality = columns[14]?.Trim(),
                                            Apr = (LeaveTypeEnum)Convert.ToInt32(columns[16]?.Trim()),
                                            May = (LeaveTypeEnum)Convert.ToInt32(columns[17]?.Trim()),
                                            Jun = (LeaveTypeEnum)Convert.ToInt32(columns[18]?.Trim()),
                                            Jul = (LeaveTypeEnum)Convert.ToInt32(columns[19]?.Trim()),
                                            Aug = (LeaveTypeEnum)Convert.ToInt32(columns[20]?.Trim()),
                                            Sep = (LeaveTypeEnum)Convert.ToInt32(columns[21]?.Trim()),
                                            Oct = (LeaveTypeEnum)Convert.ToInt32(columns[22]?.Trim()),
                                            Nov = (LeaveTypeEnum)Convert.ToInt32(columns[23]?.Trim()),
                                            Dec = (LeaveTypeEnum)Convert.ToInt32(columns[24]?.Trim()),
                                            Jan = (LeaveTypeEnum)Convert.ToInt32(columns[25]?.Trim()),
                                            Feb = (LeaveTypeEnum)Convert.ToInt32(columns[26]?.Trim()),
                                            Mar = (LeaveTypeEnum)Convert.ToInt32(columns[27]?.Trim())

                                        });
                                    }
                                }

                                if (employees.Any())
                                {
                                    await _context.BulkInsertAsync(employees);
                                }
                               
                            }
                            catch (Exception ex)
                            {
                                genericResponseTemplateModel.HasException = true;
                                genericResponseTemplateModel.Exceptions = ex;
                            }

                        }

                        if (formModel.ServiceType == 2)
                        {
                            if (formModel.Id > 0)
                            {
                                _context.LWB_Employees_UnpaidWages.RemoveRange(
                                    _context.LWB_Employees_UnpaidWages
                                            .Where(x => x.FundMasterRefId == formModel.Id));

                                var fundData = await _context.LWB_FundMaster
                                                             .FirstOrDefaultAsync(x => x.Id == formModel.Id);

                                if (fundData != null)
                                {
                                    fundData.LWBAadhaarVerficationStatusType =
                                        LWBAadhaarverificationStatusTypeEnum.PENDING_MODEL_VALIDATION;
                                }

                                await _context.SaveChangesAsync();
                            }
                            List<LWB_Employees_UnpaidWages> employees = new List<LWB_Employees_UnpaidWages>();
                            try
                            {
                                if (!System.IO.File.Exists(savedFilePath))
                                    throw new Exception($"File not found: {savedFilePath}");
                                var allLines = await File.ReadAllLinesAsync(savedFilePath);

                                Console.WriteLine($"Lines: {allLines.Length}");
                                Console.WriteLine($"Employees: {employees.Count}");

                                using (var reader = new StreamReader(savedFilePath))
                                {
                                    // Read Header
                                    var headerLine = await reader.ReadLineAsync();

                                    if (string.IsNullOrWhiteSpace(headerLine))
                                        throw new Exception("CSV file is empty.");

                                    var csvHeaders = headerLine
                                        .Split(',')
                                        .Select(x => x.Trim())
                                        .ToList();

                                    var modelHeaders = typeof(UnpaidWagesCsvEmployeeModel)
                                        .GetProperties()
                                        .Select(x => x.Name)
                                        .ToList();

                                    // Check Column Count
                                    List<string> validationErrors = new List<string>();

                                    // Missing Headers
                                    var missingHeaders = modelHeaders
                                        .Except(csvHeaders, StringComparer.OrdinalIgnoreCase)
                                        .ToList();

                                    if (missingHeaders.Any())
                                    {
                                        validationErrors.Add($"Missing Columns: {string.Join(", ", missingHeaders)}");
                                    }

                                    // Extra Headers
                                    var extraHeaders = csvHeaders
                                        .Except(modelHeaders, StringComparer.OrdinalIgnoreCase)
                                        .ToList();

                                    if (extraHeaders.Any())
                                    {
                                        validationErrors.Add(
                                            $"Unexpected Columns: {string.Join(", ", extraHeaders)}");
                                    }

                                    // Only check order when there are no missing/extra columns
                                    if (!missingHeaders.Any() && !extraHeaders.Any())
                                    {
                                        for (int i = 0; i < modelHeaders.Count; i++)
                                        {
                                            if (!string.Equals(
                                                    modelHeaders[i],
                                                    csvHeaders[i],
                                                    StringComparison.OrdinalIgnoreCase))
                                            {
                                                validationErrors.Add(
                                                    $"Column {i + 1}: Expected '{modelHeaders[i]}' but found '{csvHeaders[i]}'");
                                            }
                                        }
                                    }

                                    if (validationErrors.Any())
                                    {
                                        await transaction.RollbackAsync();
                                        genericResponseTemplateModel.Exceptions = new Exception(
                                            "Invalid CSV template.\n\n" +
                                            string.Join("\n", validationErrors) +
                                            "\n\nPlease download and use the latest CSV template."
                                        );

                                        return genericResponseTemplateModel;
                                    }

                                    // Read Data Rows
                                    while (!reader.EndOfStream)
                                    {
                                        var line = await reader.ReadLineAsync();

                                        if (string.IsNullOrWhiteSpace(line))
                                            continue;

                                        if (line.Trim() == "\"\"")
                                            continue;

                                        var columns = line.Split(',');

                                        if (columns.Length < modelHeaders.Count)
                                            continue;

                                        if (string.IsNullOrWhiteSpace(columns[0]))
                                            continue;
                                        var encryptionConfigs = Configuration.GetSection("EncryptionConfigs");
                                        var encryptionKey = encryptionConfigs.GetSection("MobileAppDataEncryptionKey").Value;
                                        var ivKey = encryptionConfigs.GetSection("MobileAppDataIVKey").Value;
                                        employees.Add(new LWB_Employees_UnpaidWages
                                        {
                                            FundMasterRefId = fundMaster.Id,
                                            EmpIdInEstablishment = columns[0]?.Trim(),
                                            Name = columns[1]?.Trim(),
                                            UniqueKey = RijndaelManagedCryptoHandler.EncryptFromPlainTextToBase64(columns[2]?.Trim(), encryptionKey, ivKey),
                                            FatherName = columns[3]?.Trim(),
                                            DateOfJoining = string.IsNullOrWhiteSpace(columns[4]) ? (DateTime?)null : Convert.ToDateTime(columns[4].Trim()),
                                            DateOfResign = string.IsNullOrWhiteSpace(columns[5]) ? (DateTime?)null : Convert.ToDateTime(columns[5].Trim()),
                                            Mobile = columns[6]?.Trim(),
                                            BankAccountNo = columns[7]?.Trim(),
                                            BankName = columns[8]?.Trim(),
                                            IFSCCode = columns[9]?.Trim(),
                                            Amount = Convert.ToDecimal(columns[10]?.Trim()),
                                            Remarks = columns[11]?.Trim(),
                                            Description = "",
                                            VerificationFormatType = AadharVerificationFormatTypeEnum.PENDING,
                                            VerificationDemographicType = AadharVerificationDemographicTypeEnum.PENDING
                                        });
                                    }
                                }

                                if (employees.Any())
                                {
                                    await _context.BulkInsertAsync(employees);
                                }
                            }
                            catch (Exception ex)
                            {
                                genericResponseTemplateModel.HasException = true;
                                genericResponseTemplateModel.Exceptions = ex;
                            }

                        }
                        await transaction.CommitAsync();
                    }
                }
            }
            catch (Exception ex)
            {
                genericResponseTemplateModel.HasException = true;
                genericResponseTemplateModel.Exceptions = ex;
            }

            return genericResponseTemplateModel;
        }


        public async Task<GenericFormModel<LWB_FundMaster>> GetLWBDetails(long id)
        {
            GenericFormModel<LWB_FundMaster> genericFormModel = new GenericFormModel<LWB_FundMaster>();
            try
            {
                if (id != 0) //Existing Record
                {

                    genericFormModel.FormModel = _context.LWB_FundMaster.Where(x => x.Id == id).FirstOrDefault();

                    //var parentWithChildObject = await _iGR_Licence_Factory_GeneralDetail
                    //   .GetAsync(x => x.AppRefId == id,  //Conditions         
                    //     null,                //Orders          
                    //     x => x.Application, x => x.Application.ApplicationAction)      //Includes
                    //   .ConfigureAwait(false);
                    //genericFormModel.FormModel = parentWithChildObject.FirstOrDefault()

                }
            }
            catch (Exception ex)
            {
                genericFormModel.HasError = true;
                genericFormModel.ErrorDesc = ex.Message;
                throw ex;
            }
            return genericFormModel;
        }

        public async Task<GenericFormModel<List<LWB_Employees_Fund>>> GetLWBEmployeeDetails(long id)
        {
            GenericFormModel<List<LWB_Employees_Fund>> genericFormModel = new GenericFormModel<List<LWB_Employees_Fund>>();
            try
            {
                if (id != 0) //Existing Record
                {

                    genericFormModel.FormModel = _context.LWBFundEmployees.Where(x => x.FundMasterRefId == id).ToList();

                    //var parentWithChildObject = await _iGR_Licence_Factory_GeneralDetail
                    //   .GetAsync(x => x.AppRefId == id,  //Conditions         
                    //     null,                //Orders          
                    //     x => x.Application, x => x.Application.ApplicationAction)      //Includes
                    //   .ConfigureAwait(false);
                    //genericFormModel.FormModel = parentWithChildObject.FirstOrDefault()

                }
            }
            catch (Exception ex)
            {
                genericFormModel.HasError = true;
                genericFormModel.ErrorDesc = ex.Message;
                throw ex;
            }
            return genericFormModel;
        }

        public async Task<GenericFormModel<List<LWB_Employees_UnpaidWages>>> GetUnpaidWagesErrorEmployeeDetails(long id)
        {
            GenericFormModel<List<LWB_Employees_UnpaidWages>> genericFormModel = new GenericFormModel<List<LWB_Employees_UnpaidWages>>();
            try
            {
                if (id != 0) // Existing Record
                {
                    var encryptionConfigs = Configuration.GetSection("EncryptionConfigs");
                    var encryptionKey = encryptionConfigs.GetSection("MobileAppDataEncryptionKey").Value;
                    var ivKey = encryptionConfigs.GetSection("MobileAppDataIVKey").Value;

                    genericFormModel.FormModel = _context.LWB_Employees_UnpaidWages
                                                         .Where(x => x.FundMasterRefId == id)
                                                         .ToList();

                    foreach (var item in genericFormModel.FormModel)
                    {
                        item.UniqueKey = RijndaelManagedCryptoHandler.DecryptFromPlainTextToBase64(
                                            item.UniqueKey,
                                            encryptionKey,
                                            ivKey);
                    }
                }
            }
            catch (Exception ex)
            {
                genericFormModel.HasError = true;
                genericFormModel.ErrorDesc = ex.Message;
                throw ex;
            }
            return genericFormModel;
        }

        public async Task<GenericFormModel<List<LWB_LIdAndUnpaidWages>>> GetLWBUnpaidWagesPaymentEmployeeDetails(Int64 fundMasterRefId)
        {
            GenericFormModel<List<LWB_LIdAndUnpaidWages>> genericFormModel = new GenericFormModel<List<LWB_LIdAndUnpaidWages>>();
            try
            {
                if (fundMasterRefId != 0) //Existing Record
                {
                    var encryptionConfigs = Configuration.GetSection("EncryptionConfigs");
                    var encryptionKey = encryptionConfigs.GetSection("MobileAppDataEncryptionKey").Value;
                    var ivKey = encryptionConfigs.GetSection("MobileAppDataIVKey").Value;
                    genericFormModel.FormModel = _context.LWB_LIdAndUnpaidWages.Where(x => x.FundMasterRefId == fundMasterRefId).ToList();
                  
                    //var parentWithChildObject = await _iGR_Licence_Factory_GeneralDetail
                    //   .GetAsync(x => x.AppRefId == id,  //Conditions         
                    //     null,                //Orders          
                    //     x => x.Application, x => x.Application.ApplicationAction)      //Includes
                    //   .ConfigureAwait(false);
                    //genericFormModel.FormModel = parentWithChildObject.FirstOrDefault()

                }
            }
            catch (Exception ex)
            {
                genericFormModel.HasError = true;
                genericFormModel.ErrorDesc = ex.Message;
                throw ex;
            }
            return genericFormModel;
        }

        public async Task<GenericFormModel<int>> VerifySlotAndLicenceNumber(string financialYear, string timeSlot, string licenceNumber)
        {
            GenericFormModel<int> genericFormModel = new GenericFormModel<int>();
            try
            {
                var existingRecord = _context.LWB_FundMaster.Where(x => x.LicenceNo == licenceNumber && x.LWBSlabType == (LWBSlabTypeEnum)Convert.ToInt32(timeSlot) && x.Year == financialYear).FirstOrDefault();
                if (existingRecord != null)
                {
                    genericFormModel.FormModel = Convert.ToInt32(existingRecord.Id);
                }
                else
                {
                    genericFormModel.FormModel = 0;
                }
            }
            catch (Exception ex)
            {
                genericFormModel.HasError = true;
                genericFormModel.ErrorDesc = ex.Message;
                throw ex;
            }
            return genericFormModel;
        }
    }
}

