using ClosedXML.Excel;
using Dapper;
using DocumentFormat.OpenXml.Bibliography;
using DocumentFormat.OpenXml.Drawing.Spreadsheet;
using DocumentFormat.OpenXml.Office2013.Drawing.ChartStyle;
using DocumentFormat.OpenXml.Presentation;
using DocumentFormat.OpenXml.Spreadsheet;
using DocumentFormat.OpenXml.Wordprocessing;
using EFCore.BulkExtensions;
using Hangfire.Logging;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.FileProviders;
using Newtonsoft.Json;
using Org.BouncyCastle.Asn1.Ocsp;
using Org.BouncyCastle.Ocsp;
using pbsamadhannetcoreapi.CommonUtiliteis;
using pbsamadhannetcoreapi.CommonUtiliteis.RSA;
using pbsamadhannetcoreapi.Models;
using pbsamadhannetcoreapi.Repositories.Implementations;
using pbsamadhannetcoreapi.Services.Implementations;
using pbsamadhannetcoreapi.ViewModels;
using RestSharp;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Policy;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using static Org.BouncyCastle.Math.EC.ECCurve;


namespace pbsamadhannetcoreapi.Services
{
    public class ThirdPartyInegrationsService : IThirdPartyInegrationsService
    {
        private IAuthService _iAuthService;
        private readonly IGenericRepository<BusinessFirst_RequestLog> _iGR_BusinessFirst_RequestLog;
        public IConfiguration Configuration { get; }
        private readonly AppDbContext _context;
        private UserManager<User> _userManager;
        private IProjectSiteService _iProjectSiteService;
        private readonly IGeneric_SP_Repository _iGeneric_SP_Repository;
        private readonly IApplicationManagementService<dynamic> _iApplicationMamnagementService;
        private readonly IGenericRepository<BusinessFirstShareStatusLog> _iGR_BusinessFirstShareStatusLog;
        private readonly IGenericRepository<ApplicationAction> _iGR_ApplicationAction;
        private readonly ISystem_O_CommunicationService _iSystem_O_CommunicationService;
        private readonly IDapperRepository _iDapperRepository;
        private readonly IApplicationManagementService<ShopLicence_GeneralDetail> _iApplicationMamnagement_Shop_Service;
        private readonly IGenericRepository<ApplicationDocument> _iGR_AppDocument;
        private readonly INotificationManagerService _iNotificationManagerService;
        private readonly IGenericRepository<BuildingPlanHUD_RTB_Mapping> _iGR_BuildingPlanHUD_RTB_Mapping;
        private readonly IApplicationManagementService<Licence_Factory_GeneralDetail> _iApplicationMamnagement_Factory_Service;
        private readonly IGenericRepository<BusinessFirst_ApprovedFileSeeding> _iGR_BusinessFirst_ApprovedFileSeeding;
        private readonly IApplicationManagementService<Licence_ContractLabour_GeneralDetail> _iApplicationMamnagement_ContractLabour_Service;
        private readonly IApplicationManagementService<Licence_CL_PE_GeneralDetail> _iApplicationMamnagement_CL_PE_Service;
        private readonly IApplicationManagementService<Licence_Proposed_BuildingPlan_GeneralDetail> _iApplicationMamnagement_Proposed_BuildingPlan_Service;
        private IToDoManagerService _iToDoManagerService;
        private readonly IApplicationManagementService<Licence_Existing_BuildingPlan_GeneralDetail> _iApplicationMamnagement_Existing_BuildingPlan_Service;
        private readonly IApplicationManagementService<Licence_Addition_Amendment_BuildingPlan_GeneralDetail> _iApplicationMamnagement_Addition_Amendment_BuildingPlan_Service;
        private readonly IApplicationManagementService<Licence_MotorTransport> _iApplicationMamnagement_Licence_MotorTransport_Service;
        private readonly IApplicationManagementService<Licence_BocwAct_GeneralDetail> _iApplicationMamnagement_Licence_BocwAct_GeneralDetail;
        private readonly IApplicationManagementService<Licence_TradeUnion> _iApplicationMamnagement_Licence_TradeUnion;
        private readonly IApplicationManagementService<Licence_PE_ISM_GeneralDetail> _iApplicationMamnagement_Licence_ISM_PrincipalEmployer;
        private readonly IApplicationManagementService<Licence_ISM_ContractLabour_GeneralDetail> _iApplicationMamnagement_Licence_ISM_ContractLabour;
    

        public ThirdPartyInegrationsService(IAuthService authService,
            IGenericRepository<BusinessFirst_RequestLog> iGR_BusinessFirst_RequestLog,
            IConfiguration configuration, AppDbContext context,
            UserManager<User> userManager,
            IProjectSiteService iprojectSiteService,
            IGeneric_SP_Repository iGeneric_SP_Repository,
            IApplicationManagementService<dynamic> iApplicationMamnagementService,
            IGenericRepository<BusinessFirstShareStatusLog> iGR_BusinessFirstShareStatusLog,
            IGenericRepository<ApplicationAction> iGR_ApplicationAction,
            ISystem_O_CommunicationService iSystem_O_CommunicationService,
            IDapperRepository iDapperRepository,
            IApplicationManagementService<ShopLicence_GeneralDetail> iApplicationMamnagement_Shop_Service,
            IGenericRepository<ApplicationDocument> iGR_AppDocument,
            INotificationManagerService iNotificationManagerService,
            IGenericRepository<BuildingPlanHUD_RTB_Mapping> iGR_BuildingPlanHUD_RTB_Mapping,
            IApplicationManagementService<Licence_Factory_GeneralDetail> iApplicationMamnagement_Factory_Service,
            IGenericRepository<BusinessFirst_ApprovedFileSeeding> iGR_BusinessFirst_ApprovedFileSeeding,
            IApplicationManagementService<Licence_ContractLabour_GeneralDetail> iApplicationMamnagement_ContractLabour_Service,
            IApplicationManagementService<Licence_Proposed_BuildingPlan_GeneralDetail> iApplicationMamnagement_Proposed_BuildingPlan_Service,
            IApplicationManagementService<Licence_Existing_BuildingPlan_GeneralDetail> iApplicationMamnagement_Existing_BuildingPlan_Service,
            IApplicationManagementService<Licence_Addition_Amendment_BuildingPlan_GeneralDetail> iApplicationMamnagement_Addition_Amendment_BuildingPlan_Service,
            IApplicationManagementService<Licence_MotorTransport> iApplicationMamnagement_Licence_MotorTransport_Service,
            IApplicationManagementService<Licence_CL_PE_GeneralDetail> iApplicationMamnagement_CL_PE_Service,
            IApplicationManagementService<Licence_BocwAct_GeneralDetail> iApplicationMamnagement_Licence_BocwAct_GeneralDetail,
            IApplicationManagementService<Licence_TradeUnion> iApplicationMamnagement_Licence_TradeUnion,
            IApplicationManagementService<Licence_PE_ISM_GeneralDetail> iApplicationMamnagement_Licence_ISM_PrincipalEmployer,
            IApplicationManagementService<Licence_ISM_ContractLabour_GeneralDetail> iApplicationMamnagement_Licence_ISM_ContractLabour,
            IToDoManagerService iToDoManagerService)
        {
            _iAuthService = authService;
            _iGR_BusinessFirst_RequestLog = iGR_BusinessFirst_RequestLog;
            Configuration = configuration;
            _context = context;
            _userManager = userManager;
            _iProjectSiteService = iprojectSiteService;
            _iGeneric_SP_Repository = iGeneric_SP_Repository;
            _iApplicationMamnagementService = iApplicationMamnagementService;
            _iGR_BusinessFirstShareStatusLog = iGR_BusinessFirstShareStatusLog;
            _iGR_BuildingPlanHUD_RTB_Mapping = iGR_BuildingPlanHUD_RTB_Mapping;
            _iGR_ApplicationAction = iGR_ApplicationAction;
            _iSystem_O_CommunicationService = iSystem_O_CommunicationService;
            _iDapperRepository = iDapperRepository;
            _iApplicationMamnagement_Shop_Service = iApplicationMamnagement_Shop_Service;
            _iGR_AppDocument = iGR_AppDocument;
            _iNotificationManagerService = iNotificationManagerService;
            _iApplicationMamnagement_Factory_Service = iApplicationMamnagement_Factory_Service;
            _iGR_BusinessFirst_ApprovedFileSeeding = iGR_BusinessFirst_ApprovedFileSeeding;
            _iApplicationMamnagement_ContractLabour_Service = iApplicationMamnagement_ContractLabour_Service;
            _iApplicationMamnagement_CL_PE_Service = iApplicationMamnagement_CL_PE_Service;
            _iApplicationMamnagement_Proposed_BuildingPlan_Service = iApplicationMamnagement_Proposed_BuildingPlan_Service;
            _iToDoManagerService = iToDoManagerService;
            _iApplicationMamnagement_Existing_BuildingPlan_Service = iApplicationMamnagement_Existing_BuildingPlan_Service;
            _iApplicationMamnagement_Addition_Amendment_BuildingPlan_Service = iApplicationMamnagement_Addition_Amendment_BuildingPlan_Service;
            _iApplicationMamnagement_Licence_MotorTransport_Service = iApplicationMamnagement_Licence_MotorTransport_Service;
            _iApplicationMamnagement_Licence_BocwAct_GeneralDetail = iApplicationMamnagement_Licence_BocwAct_GeneralDetail;
            _iApplicationMamnagement_Licence_TradeUnion = iApplicationMamnagement_Licence_TradeUnion;
            _iApplicationMamnagement_Licence_ISM_PrincipalEmployer = iApplicationMamnagement_Licence_ISM_PrincipalEmployer;
            _iApplicationMamnagement_Licence_ISM_ContractLabour = iApplicationMamnagement_Licence_ISM_ContractLabour;
        }
        public async Task<ServiceGatewayResponseViewModel> ServiceGateway(string msg, HttpRequest httpRequest)
        {

            ServiceGatewayResponseViewModel serviceGatewayResponse = new ServiceGatewayResponseViewModel() { HasException = false };
            try
            {
                string[] queryParmArray = msg.Split("|");
                if (queryParmArray.Length < 7)
                {
                    serviceGatewayResponse.HasLessParameters = true;
                    return serviceGatewayResponse;
                }


                GenericResponseTemplateModel<List<ToDoTicketDetailViewModel>> resp = await _iToDoManagerService.GetOpenedTicketsByUserId(queryParmArray[0]);

                if (resp.ResponseDataModel != null &&resp.ResponseDataModel.Count() > 0 )
                {
                    serviceGatewayResponse.CanApply = false;
                    serviceGatewayResponse.HasOpenedTickets = true;
                    serviceGatewayResponse.ToDoTickets = resp.ResponseDataModel;
                    return serviceGatewayResponse;
                }

                ServiceGatewayRequestJsonDataViewModel serviceGatewayRequestJsonData = JsonConvert.DeserializeObject<ServiceGatewayRequestJsonDataViewModel>(queryParmArray[5]);

                if (serviceGatewayRequestJsonData.ServiceCode == 62 && serviceGatewayRequestJsonData.CategoryType == "FT")
                {
                    List<StoreProcedureParm> storeProcedureParms = new List<StoreProcedureParm>()
                    {
                       new StoreProcedureParm (){ ParmName="LicenceNumber", ParmValue = serviceGatewayRequestJsonData.FileNo.ToString(), isNumber = false},
                    };
                    var alreadyInProcess = await _iGeneric_SP_Repository.CallStoreProcedureReaderAsync<CheckAlreadyInProcessViewTypeViewModel>("sp_CheckAlreadyInProcess", storeProcedureParms);
                    if (alreadyInProcess.Count() == 0)
                    {
                        var checkApplicationType = await GetActTypeByLicenceNumber(serviceGatewayRequestJsonData.FileNo);
                        if (checkApplicationType.ResponseDataModel.FirstOrDefault()?.ApplicationType == 70)
                        {
                            List<StoreProcedureParm> storeProcedureParms1 = new List<StoreProcedureParm>()
                            {
                                new StoreProcedureParm (){ ParmName="LicenceNo", ParmValue = serviceGatewayRequestJsonData.FileNo.ToString(), isNumber = false},
                                new StoreProcedureParm (){ ParmName="ProjectSiteRefId", ParmValue= "", isNumber = false},
                                new StoreProcedureParm (){ ParmName="ProjectSiteVersion", ParmValue= "", isNumber = false},
                            };
                            var lastClearance = await _iGeneric_SP_Repository.CallStoreProcedureReaderAsync<GetlastClearanceViewModel>("sp_GetLastClerance", storeProcedureParms1);

                            if (lastClearance != null && lastClearance.Any())
                            {
                                var lastdate = lastClearance.First().ClearanceExpiredOn;
                                if (lastdate < DateTime.Today)
                                {
                                    serviceGatewayResponse.CanApply = false;
                                    serviceGatewayResponse.RequestDeniedReason = "Your license number is expired, so you are not eligible to apply for this service. .........!";
                                    return serviceGatewayResponse;
                                }
                            }
                        }
                        else
                        {
                            serviceGatewayResponse.CanApply = false;
                            serviceGatewayResponse.RequestDeniedReason = "Your license number belongs to another category, so you are not eligible to apply for this service. .........!";
                            return serviceGatewayResponse;
                        }
                    }
                    else
                    {
                        serviceGatewayResponse.CanApply = false;
                        serviceGatewayResponse.RequestDeniedReason = "This service has already been applied. (CHK-6)";
                        return serviceGatewayResponse;
                    }
                }

                BusinessFirst_RequestLog requestLog = new BusinessFirst_RequestLog()
                {
                    AppId = Convert.ToInt64(serviceGatewayRequestJsonData.AppId),
                    IPin = Convert.ToInt64(serviceGatewayRequestJsonData.iPin),
                    UserId = queryParmArray[0],
                    ServiceCode = serviceGatewayRequestJsonData.ServiceCode,
                    CategoryTypeId = serviceGatewayRequestJsonData.CategoryTypeId,
                    RequestCount = 1,
                    NativeAppId = 0,
                    CreatedDate = DateTime.Now,
                    LastModifiedDate = DateTime.Now,
                    RequestString = msg,
                    IsEnabled = true,
                    NativeUserId = null
                };

                if (serviceGatewayRequestJsonData.ServiceCode == 74 && serviceGatewayRequestJsonData.CategoryType != "FT")
                {
                    Application application = await _context.Applications.Where(x => x.InvestPunjab_Ipin == serviceGatewayRequestJsonData.iPin.ToString()
                                                    && x.InvestPunjab_AppId.ToString() == serviceGatewayRequestJsonData.AppId).FirstOrDefaultAsync();
                    serviceGatewayRequestJsonData.ServiceCode = (int)application.ApplicationType;
                    serviceGatewayRequestJsonData.ActID = (int)application.ApplicationType;
                    requestLog.ServiceCode = (int)application.ApplicationType;
                }



                // Verify Licence no exist & for same service type
                if (serviceGatewayRequestJsonData.FileNo != null)
                {
                    var applicationTypeDetailsIP = FindServiceCodeMappingWithApplicationType(serviceGatewayRequestJsonData.ServiceCode);
                    var isFound = await _context.ApplicationLicenceNoMapping.Include(x => x.Application).Where(x => x.LicenceNumber == serviceGatewayRequestJsonData.FileNo.ToString() 
                    && (x.Application.ApplicationType != ApplicationTypeEnum.WOMEN_NIGHT_SHIFT_FACTORY || x.Application.ApplicationType != ApplicationTypeEnum.WOMEN_NIGHT_SHIFT_SHOP)).FirstOrDefaultAsync();
                    if (isFound != null)
                    {
                        string applicationType = applicationTypeDetailsIP.ApplicationType.ToString() == "WOMEN_NIGHT_SHIFT_FACTORY" ? "FACTORY_LICENCE"
                                         : applicationTypeDetailsIP.ApplicationType.ToString() == "WOMEN_NIGHT_SHIFT_SHOP" ? "SHOP_LICENCE"
                                         : applicationTypeDetailsIP.ApplicationType.ToString();

                        string exitsapplicationType = isFound.Application.ApplicationType.ToString() == "WOMEN_NIGHT_SHIFT_FACTORY" ? "FACTORY_LICENCE"
                                        : isFound.Application.ApplicationType.ToString() == "WOMEN_NIGHT_SHIFT_SHOP" ? "SHOP_LICENCE"
                                        : isFound.Application.ApplicationType.ToString();


                        if (applicationType != exitsapplicationType)
                        {
                            serviceGatewayResponse.CanApply = false;
                            serviceGatewayResponse.RequestDeniedReason = "Licence no not belong to applied service type. Please enter the correct licence no..!";
                            return serviceGatewayResponse;
                        }
                    }
                    else // Find licence no and service type in old db
                    {
                        var licenceDetais = await _iDapperRepository.Get<int>("SELECT TOP 1 AppFormId FROM AppClearanceIssueds WHERE LicenceNo = @LicenceNo ", new { LicenceNo = serviceGatewayRequestJsonData.FileNo.ToString() }, CommandType.Text).ConfigureAwait(false);
                        var applicationTypeDetailsOLd = FindServiceCodeMappingWithApplicationType(licenceDetais.FirstOrDefault());
                        if (licenceDetais != null && applicationTypeDetailsOLd != null)
                        {
                            string applicationType = applicationTypeDetailsIP.ApplicationType.ToString() == "WOMEN_NIGHT_SHIFT_FACTORY" ? "FACTORY_LICENCE"
                      : applicationTypeDetailsIP.ApplicationType.ToString() == "WOMEN_NIGHT_SHIFT_SHOP" ? "SHOP_LICENCE"
                      : applicationTypeDetailsIP.ApplicationType.ToString();

                            string exitsapplicationType = applicationTypeDetailsOLd.ApplicationType.ToString() == "WOMEN_NIGHT_SHIFT_FACTORY" ? "FACTORY_LICENCE"
                          : applicationTypeDetailsOLd.ApplicationType.ToString() == "WOMEN_NIGHT_SHIFT_SHOP" ? "SHOP_LICENCE"
                          : applicationTypeDetailsOLd.ApplicationType.ToString();

                            if (applicationType != exitsapplicationType)
                            {
                                serviceGatewayResponse.CanApply = false;
                                serviceGatewayResponse.RequestDeniedReason = "Licence no not belong to applied service type. Please enter the correct licence no..!";
                                return serviceGatewayResponse;
                            }
                        }
                    }

                }

                GenericResponseTemplateModel<BusinessFirstRequestLogResponseViewModel> genericServiceResultTemplate = await LogBusinessFirstRequest(requestLog);
                bool serviceIsAlreadyAppliedORInProcess = true;
                int[] appType_Registration = new int[] { 3, 4, 5, 6, 17, 22, 35, 61, 62, 63, 70, 71, 72, 73, 74, 75, 76,81,19,101 };
                int[] appType_Renewal = new int[] { 7, 12, 23 ,20 };
                int[] appType_Amendment = new int[] { 8, 11, 13, 15, 18, 24, 36 ,21 };
                int[] labour_services = new int[] { 1001 };



                if (!genericServiceResultTemplate.HasError)
                {
                    #region Commented Code

                    //if (genericServiceResultTemplate.ResponseDataModel.NativeAppId>0 && serviceGatewayRequestJsonData.CategoryTypeId==1) 
                    //{
                    //    serviceGatewayResponse.HasException = true;
                    //    serviceGatewayResponse.ExceptionMessage = "Duplicate service..!";
                    //}
                    //else 
                    //if(genericServiceResultTemplate.ResponseDataModel.NativeUserId == null)
                    //{
                    //    GenericResponseTemplateModel<BusinessFirstSCAFViewModel> genericResponseTemplateModel =  await GetBusinessFirstCafDataByIPin(requestLog.IPin);

                    //    if(genericResponseTemplateModel.ResponseDataModel == null)
                    //    {
                    //        serviceGatewayResponse.HasException = true;
                    //        serviceGatewayResponse.ExceptionMessage = "Issue in get CAF data";
                    //    }
                    //    else
                    //    {
                    //        Guid guid = Guid.NewGuid();
                    //        User user = new User()
                    //        {
                    //            AccessFailedCount = 0,
                    //            ConcurrencyStamp = "",
                    //            Email = "NoEmail@some.com",
                    //            EmailConfirmed = true,
                    //            Id = guid.ToString(),
                    //            LockoutEnabled = false,
                    //            LockoutEnd = null,
                    //            NormalizedEmail = "NoEmail@some.com",
                    //            NormalizedUserName = genericResponseTemplateModel.ResponseDataModel.AppUser.Username,
                    //            PasswordHash = "mHwh59uDN9MAGOBITRYJkR3FWK62fgqAjBKhFUePd8A=",
                    //            PhoneNumber = "0000000000",
                    //            PhoneNumberConfirmed = true,
                    //            SecurityStamp = "rI93o3GC0KZOhH7A+TX+8g==",
                    //            TwoFactorEnabled = false,
                    //            UserName = genericResponseTemplateModel.ResponseDataModel.AppUser.Username,
                    //            IsEnabled = false,
                    //            IsTestUser = false
                    //        };

                    //        await _userManager.CreateAsync(user);


                    //        List<StoreProcedureParm> storeProcedureParms = new List<StoreProcedureParm>()
                    //        {
                    //            new StoreProcedureParm (){ ParmName="RoleId", ParmValue="592add3e-f992-4983-a8ca-21ddc090bda0", isNumber=false},
                    //            new StoreProcedureParm (){ ParmName="UserId", ParmValue=user.Id, isNumber=false}
                    //        };

                    //UserProfile userProfile = new UserProfile()
                    //{
                    //    FirstName = genericResponseTemplateModel.ResponseDataModel.AppUser.FirstName,
                    //    MiddleName = genericResponseTemplateModel.ResponseDataModel.AppUser.MiddleName,
                    //    LastName = genericResponseTemplateModel.ResponseDataModel.AppUser.LastName,
                    //    FatherName = "NA",
                    //    MobileNo = genericResponseTemplateModel.ResponseDataModel.AppUser.PhoneNumber.Substring(genericResponseTemplateModel.ResponseDataModel.AppUser.PhoneNumber.Length - 10),
                    //    AlternateMobileNo = genericResponseTemplateModel.ResponseDataModel.AppUser.PhoneNumber.Substring(genericResponseTemplateModel.ResponseDataModel.AppUser.PhoneNumber.Length - 10),
                    //    Email = genericResponseTemplateModel.ResponseDataModel.AppUser.Email,
                    //    AlternateEmail = genericResponseTemplateModel.ResponseDataModel.AppUser.Email,
                    //    TehsilId = genericResponseTemplateModel.ResponseDataModel.CafInfo.SiteLGDTehId,
                    //    DistrictId = genericResponseTemplateModel.ResponseDataModel.CafInfo.SiteLGDDistId,
                    //    State = "Punjab",
                    //    PinCode = genericResponseTemplateModel.ResponseDataModel.CafInfo.ComPin,
                    //    Signature = "NA",
                    //    ProfilePhoto = "NA",
                    //    IsActive = true,
                    //    IsDeleted = false,
                    //    Createddate = DateTime.Now,
                    //    LastModifiedDate = DateTime.Now
                    //};


                    //        await _iGeneric_SP_Repository.CallStoreProcedureReaderScalerAsync("dbo.sp_MapUserWithRole", storeProcedureParms);

                    //        UserProfile userProfile = new UserProfile()
                    //        {
                    //            FirstName = genericResponseTemplateModel.ResponseDataModel.AppUser.FirstName,
                    //            MiddleName = genericResponseTemplateModel.ResponseDataModel.AppUser.MiddleName,
                    //            LastName = genericResponseTemplateModel.ResponseDataModel.AppUser.LastName,
                    //            FatherName = "NA",
                    //            MobileNo = genericResponseTemplateModel.ResponseDataModel.AppUser.PhoneNumber.Substring(genericResponseTemplateModel.ResponseDataModel.AppUser.PhoneNumber.Length - 10),
                    //            AlternateMobileNo = genericResponseTemplateModel.ResponseDataModel.AppUser.PhoneNumber.Substring(genericResponseTemplateModel.ResponseDataModel.AppUser.PhoneNumber.Length - 10),
                    //            Email = genericResponseTemplateModel.ResponseDataModel.AppUser.Email,
                    //            AlternateEmail = genericResponseTemplateModel.ResponseDataModel.AppUser.Email,
                    //            TehsilId = genericResponseTemplateModel.ResponseDataModel.AppUser.LGDTehsilId,
                    //            DistrictId = genericResponseTemplateModel.ResponseDataModel.AppUser.LGDDistrictId,
                    //            State = "Punjab",
                    //            PinCode = genericResponseTemplateModel.ResponseDataModel.CafInfo.ComPin,
                    //            Signature = "NA",
                    //            ProfilePhoto = "NA",
                    //            IsActive = true,
                    //            IsDeleted = false,
                    //            Createddate = DateTime.Now,
                    //            LastModifiedDate = DateTime.Now
                    //        };

                    //        var userProfileResponse = await _iAuthService.CreateNewUserProfile(userProfile);
                    //        await _iAuthService.MapUserWithProfile(user.Id, userProfile.UserProfileId);

                    //        //var factoryCircleId = _context.FactoryCircles.Where(x => x.DistrictLgdRefId== genericResponseTemplateModel.ResponseDataModel.AppUser.LGDDistrictId).Select(x=> x.FactoryCircleId).FirstOrDefault();
                    //        //var alcCircleId = _context.ALCCircles.Where(x=> x.DistrictLgdRefId== genericResponseTemplateModel.ResponseDataModel.AppUser.LGDDistrictId).Select(x => x.ALCCircleId).FirstOrDefault();
                    //        //var labourCircleId = _context.LabourCircles.Where(x => x.ALCCircleRefId == alcCircleId).Select(x => x.LabourCircleId).FirstOrDefault();

                    //        var factoryCircleId = 22;
                    //        var alcCircleId = 10002;
                    //        var labourCircleId = 10001;

                    //        ProjectSite projectSite = new ProjectSite()
                    //        {
                    //            EstablishmentName = genericResponseTemplateModel.ResponseDataModel.CafInfo.EstbName,
                    //            Address = genericResponseTemplateModel.ResponseDataModel.CafInfo.ComAddress,
                    //            VillageOrTown = genericResponseTemplateModel.ResponseDataModel.CafInfo.ComVlgName,
                    //            TehsilRefId = genericResponseTemplateModel.ResponseDataModel.CafInfo.SiteLGDTehId,
                    //            DistrictRefId = genericResponseTemplateModel.ResponseDataModel.CafInfo.SiteLGDDistId,
                    //            PinCode = genericResponseTemplateModel.ResponseDataModel.CafInfo.ComPin,
                    //            IsActive = true,
                    //            IsDeleted = false,
                    //            Createddate = DateTime.Now,
                    //            LastModifiedDate = DateTime.Now,
                    //            UserRefId = user.Id,
                    //            LabourCircleRefId = labourCircleId,
                    //            FactoryCircleRefId = factoryCircleId
                    //        };

                    //       // await _iProjectSiteService.AddUpdate_ProjectSiteDetail(projectSite, user.Id);

                    //       var projectSiteIdCreateResp = await _iProjectSiteService.CreateProjectSite(projectSite);


                    //        // Update BusinessFirst_RequestLog - UserId

                    //        await UpdateBusinessFirstRequestNativeUserNativeByIPin(requestLog.IPin, user.Id);

                    //        LoggedUserInfoViewModel loggedUserInfoViewModel = await _iAuthService.GetUserLoginDeatilByUserId(user.Id);
                    //        if (loggedUserInfoViewModel != null)
                    //        {
                    //            serviceGatewayResponse.CanApply = true;
                    //            serviceGatewayResponse.IPin = requestLog.IPin;
                    //            serviceGatewayResponse.NativeUserId = user.Id;
                    //            serviceGatewayResponse.NativeAppId = genericServiceResultTemplate.ResponseDataModel.NativeAppId;
                    //            serviceGatewayResponse.ServiceCode = requestLog.ServiceCode;
                    //            serviceGatewayResponse.ProjectSiteRefId = projectSiteIdCreateResp.ResponseDataModel;
                    //            serviceGatewayResponse.SymmetricKey = await _iAuthService.GenerateNewJwtToken(loggedUserInfoViewModel);
                    //            serviceGatewayResponse.CategoryTypeId = serviceGatewayRequestJsonData.CategoryTypeId;
                    //        }
                    //        else
                    //        {

                    //        }
                    //    }
                    //}
                    //else
                    //{
                    //    LoggedUserInfoViewModel loggedUserInfoViewModel = await _iAuthService.GetUserLoginDeatilByUserId(genericServiceResultTemplate.ResponseDataModel.NativeUserId);
                    //    if (loggedUserInfoViewModel != null)
                    //    {
                    //        Int64 projectSiteId = 0;
                    //        if (genericServiceResultTemplate.ResponseDataModel.NativeAppId > 0)
                    //        {
                    //            var projectSiteIdResp = await _iProjectSiteService.GetProjectSiteRefIdByAppId(genericServiceResultTemplate.ResponseDataModel.NativeAppId);
                    //            projectSiteId = projectSiteIdResp.ResponseDataModel;
                    //        }
                    //        else
                    //        {
                    //            var projectSiteIdResp = await _iProjectSiteService.GetProjectSiteRefIdByUserId(genericServiceResultTemplate.ResponseDataModel.NativeUserId);
                    //            projectSiteId = projectSiteIdResp.ResponseDataModel;
                    //        }

                    //        serviceGatewayResponse.CanApply = true;
                    //        serviceGatewayResponse.IPin = requestLog.IPin;
                    //        serviceGatewayResponse.NativeUserId = genericServiceResultTemplate.ResponseDataModel.NativeUserId;
                    //        serviceGatewayResponse.NativeAppId = genericServiceResultTemplate.ResponseDataModel.NativeAppId;
                    //        serviceGatewayResponse.ServiceCode = requestLog.ServiceCode;
                    //        serviceGatewayResponse.ProjectSiteRefId = projectSiteId;
                    //        serviceGatewayResponse.SymmetricKey = await _iAuthService.GenerateNewJwtToken(loggedUserInfoViewModel);
                    //        serviceGatewayResponse.CategoryTypeId = serviceGatewayRequestJsonData.CategoryTypeId;
                    //    }
                    //}
                    //}
                    //else // Show Error to user
                    //{
                    //    serviceGatewayResponse.HasException = true;
                    //    serviceGatewayResponse.ExceptionMessage = "Issue in log request";
                    //}

                    //if (serviceGatewayResponse.CanApply)
                    //{

                    #endregion

                    //CHK-1 -- Check application Type
                    if (serviceGatewayRequestJsonData.CategoryType == "OR" || serviceGatewayRequestJsonData.CategoryType == "VW" || serviceGatewayRequestJsonData.CategoryType == "ED" || serviceGatewayRequestJsonData.CategoryType == "DP")
                    {
                        var Sys_N_ServiceCodeInfo = FindServiceCodeMappingWithApplicationType(serviceGatewayRequestJsonData.ServiceCode);

                        var alreadyInProcess = await IsNativeAppIdApplicationIsAlreadyInprocess(serviceGatewayRequestJsonData.iPin, Sys_N_ServiceCodeInfo.ApplicationType, Sys_N_ServiceCodeInfo.ApplicationPurposeType);
                        if (alreadyInProcess.ResponseDataModel)
                        {
                            serviceGatewayResponse = await OpenServiceApplicationForm(genericServiceResultTemplate, requestLog, serviceGatewayRequestJsonData, false, "", true, true, httpRequest);
                        }
                        else
                        {
                            //DoesIPinBelongToYS_N
                            var belongToSysN = await DoesIPinBelongToSYS_N(serviceGatewayRequestJsonData.iPin, serviceGatewayRequestJsonData.AppId, Sys_N_ServiceCodeInfo.ApplicationType, Sys_N_ServiceCodeInfo.ApplicationPurposeType);
                            if (belongToSysN.ResponseDataModel != null)
                            {
                                genericServiceResultTemplate.ResponseDataModel.NativeAppId = belongToSysN.ResponseDataModel.AppId;
                                serviceGatewayResponse = await OpenServiceApplicationForm(genericServiceResultTemplate, requestLog, serviceGatewayRequestJsonData, false, "", true, true, httpRequest);
                            }
                            else
                            {
                                var applicationTypeDetails = FindServiceCodeMappingWithApplicationType(serviceGatewayRequestJsonData.ServiceCode);
                                var Sys_O_AppId_TokenNumberInfo = await _iSystem_O_CommunicationService.GetLegacyAppIdFromIPin(serviceGatewayRequestJsonData.iPin, Convert.ToInt64(serviceGatewayRequestJsonData.AppId), applicationTypeDetails.ApplicationType);
                                var isfileinobjection = await _iSystem_O_CommunicationService.IsServiceAlreadyInObjection(long.Parse(Sys_O_AppId_TokenNumberInfo.ResponseDataModel.TokenNumber), applicationTypeDetails.ServiceCode);

                                if (serviceGatewayRequestJsonData.CategoryType == "OR" && isfileinobjection.ResponseDataModel == true && applicationTypeDetails.ApplicationType == ApplicationTypeEnum.FACTORY_LICENCE)
                                {

                                    var LicenceNumberList = await _iDapperRepository.Get<string>("SELECT TOP 1 LicenceNo FROM AppClearanceIssueds WHERE AppFormId IN (6,7,8) AND AppId = @Appid AND LicenceNo IS NOT NULL AND LicenceNo != '' ORDER BY TDate DESC", new { Appid = Sys_O_AppId_TokenNumberInfo.ResponseDataModel.AppId }, CommandType.Text).ConfigureAwait(false);

                                    var LicenceNumber = LicenceNumberList.FirstOrDefault();
                                    if (LicenceNumber == null)
                                    {
                                        LicenceNumber = null;
                                    }

                                    var processImdResp = await ImportAndSeedData(Sys_O_AppId_TokenNumberInfo.ResponseDataModel.AppId, serviceGatewayRequestJsonData.iPin.ToString(), requestLog.AppId, serviceGatewayRequestJsonData.ServiceCode, Sys_O_AppId_TokenNumberInfo.ResponseDataModel.TokenNumber, LicenceNumber, true);
                                    if (processImdResp.ResponseDataModel != null)
                                    {
                                        requestLog.NativeAppId = processImdResp.ResponseDataModel.AppRefId;
                                        genericServiceResultTemplate.ResponseDataModel.NativeAppId = processImdResp.ResponseDataModel.AppRefId;
                                        serviceGatewayResponse = await OpenServiceApplicationForm(genericServiceResultTemplate, requestLog, serviceGatewayRequestJsonData, false, "", true, true, httpRequest);
                                    }
                                    else
                                    {
                                        serviceGatewayResponse.CanApply = false;
                                        serviceGatewayResponse.RequestDeniedReason = "Issue in opening form (CHK-7)..!";
                                    }
                                }

                                else
                                {
                                    serviceGatewayResponse.CanApply = false;
                                    serviceGatewayResponse.RequestDeniedReason = "Request belongs to Sys_O";
                                    serviceGatewayResponse.IsRequestToLegacyApp = true;
                                    if (appType_Registration.Contains(serviceGatewayRequestJsonData.ServiceCode))
                                    {
                                       // serviceGatewayResponse.LegacyAppUrl = Configuration.GetSection("thirdpartyintegrationconfigs").GetSection("legacyapplicationurls").GetSection("registrationurl").Value;
                                    }
                                    else
                                    {
                                       // serviceGatewayResponse.LegacyAppUrl = Configuration.GetSection("thirdpartyintegrationconfigs").GetSection("legacyapplicationurls").GetSection("amendmenturl").Value;
                                    }
                                }
                            }
                        }
                    }
                    else if (appType_Registration.Contains(serviceGatewayRequestJsonData.ServiceCode)) // CHK-1 -- Registration
                    {
                        if(serviceGatewayRequestJsonData.ServiceCode != 3 && serviceGatewayRequestJsonData.ServiceCode != 11 && serviceGatewayRequestJsonData.ServiceCode != 4 && serviceGatewayRequestJsonData.ServiceCode != 12 && serviceGatewayRequestJsonData.ServiceCode != 13
                            && serviceGatewayRequestJsonData.ServiceCode != 17 && serviceGatewayRequestJsonData.ServiceCode != 18 && serviceGatewayRequestJsonData.ServiceCode != 19 && serviceGatewayRequestJsonData.ServiceCode != 20
                            && serviceGatewayRequestJsonData.ServiceCode != 21 && serviceGatewayRequestJsonData.ServiceCode != 35 && serviceGatewayRequestJsonData.ServiceCode != 36)
                        {
                            var serResp = await _iSystem_O_CommunicationService.HasServiceAlreadyApplied(serviceGatewayRequestJsonData.iPin, serviceGatewayRequestJsonData.ServiceCode);
                            if (!serResp.HasError)
                            {
                                //CHK-2 -- Is application already applied in SYS-O
                                if (serResp.ResponseDataModel)
                                {
                                    serviceGatewayResponse.CanApply = false;
                                    serviceGatewayResponse.RequestDeniedReason = "This service has already been applied..!";
                                }
                                else
                                {
                                    serviceIsAlreadyAppliedORInProcess = false;
                                }
                            }
                            else
                            {
                                serviceGatewayResponse.CanApply = false;
                                serviceGatewayResponse.RequestDeniedReason = "Issue in applying service (CHK-2)";
                            }
                        }
                         else if (labour_services.Contains(serviceGatewayRequestJsonData.ServiceCode) ) // Labour services
                    {
                            serviceIsAlreadyAppliedORInProcess = false;
                    }
                        else
                        {
                            serviceIsAlreadyAppliedORInProcess = false;
                        }
                            

                       
                    }
                    else if (appType_Renewal.Contains(serviceGatewayRequestJsonData.ServiceCode) || appType_Amendment.Contains(serviceGatewayRequestJsonData.ServiceCode)) // CHK-1 -- Renewal OR Amendment
                    {


                        if (serviceGatewayRequestJsonData.ServiceCode != 3 && serviceGatewayRequestJsonData.ServiceCode != 11 && serviceGatewayRequestJsonData.ServiceCode != 4 && serviceGatewayRequestJsonData.ServiceCode != 12 && serviceGatewayRequestJsonData.ServiceCode != 13
                            && serviceGatewayRequestJsonData.ServiceCode != 17 && serviceGatewayRequestJsonData.ServiceCode != 18 && serviceGatewayRequestJsonData.ServiceCode != 19 && serviceGatewayRequestJsonData.ServiceCode != 20
                            && serviceGatewayRequestJsonData.ServiceCode != 21 && serviceGatewayRequestJsonData.ServiceCode != 35 && serviceGatewayRequestJsonData.ServiceCode != 36)
                        {
                            var serResp = await _iSystem_O_CommunicationService.IsServiceAlreadyInProcess(serviceGatewayRequestJsonData.iPin, serviceGatewayRequestJsonData.ServiceCode);

                            if (!serResp.HasError)
                            {
                                //CHK-3 -- Is application already in-process in SYS-O
                                if (serResp.ResponseDataModel)
                                {
                                    serviceGatewayResponse.CanApply = false;
                                    serviceGatewayResponse.RequestDeniedReason = "This service already in process..!";
                                }
                                else
                                {
                                    serviceIsAlreadyAppliedORInProcess = false;
                                }
                            }
                            else
                            {
                                serviceGatewayResponse.CanApply = false;
                                serviceGatewayResponse.RequestDeniedReason = "Issue in applying service (CHK-3)";
                            }
                        }
                        else
                        {
                            serviceIsAlreadyAppliedORInProcess = false;
                        }


                        
                    }
                    else if (labour_services.Contains(serviceGatewayRequestJsonData.ServiceCode)) // Labour services
                    {
                        serviceIsAlreadyAppliedORInProcess = false;
                    }
                    else // CHK-1 -- Invalid service code
                    {
                        serviceGatewayResponse.CanApply = false;
                        serviceGatewayResponse.RequestDeniedReason = "Unknown application type";
                    }
                }
                else // Show Error to user
                {
                    serviceGatewayResponse.CanApply = false;
                    serviceGatewayResponse.RequestDeniedReason = "Issue in log request";
                }

                if (!serviceGatewayResponse.HasException && !serviceIsAlreadyAppliedORInProcess)
                {
                    if (serviceGatewayRequestJsonData.ServiceCode == 1001)
                    {
                        var user = _context.Users.Where(x => x.Id == requestLog.UserId).FirstOrDefault();
                        if (user != null)
                        {
                            genericServiceResultTemplate.ResponseDataModel.NativeUserId = user.Id;
                            serviceGatewayResponse = await OpenServiceApplicationForm(genericServiceResultTemplate, requestLog, serviceGatewayRequestJsonData, false, "", false, true, httpRequest);
                        }
                        else
                        {
                            var createUserServiceResp = await CreateNewUser(serviceGatewayRequestJsonData.iPin, null);
                            if (createUserServiceResp.HasError)
                            {
                                serviceGatewayResponse.CanApply = false;
                                serviceGatewayResponse.RequestDeniedReason = "Issue in applying service. " + createUserServiceResp.ErrorDesc + " (CHK-7)";
                            }
                        }
                    }
                    else
                    {
                        var reqLog = await _iGR_BusinessFirst_RequestLog.GetAsync(x => x.IPin == serviceGatewayRequestJsonData.iPin && x.NativeUserId != null && x.ServiceCode == requestLog.ServiceCode).ConfigureAwait(false);
                        //CHK-4 -- Check is iPin and Native UserId exists in request log
                        var licenceInfo = _context.ApplicationLicenceNoMapping.Where(x => x.LicenceNumber == serviceGatewayRequestJsonData.FileNo).OrderByDescending(x => x.ApplicationLicenceNoMappingId).FirstOrDefault();

                        if ((reqLog.Count() > 0 && reqLog.Any(x => x.NativeAppId > 0)) || (licenceInfo != null && reqLog.Any(x => x.ServiceCode == serviceGatewayRequestJsonData.ServiceCode))) // CHK-4 -- exists in SYS-N
                        {
                            var applicationTypeDetails = FindServiceCodeMappingWithApplicationType(serviceGatewayRequestJsonData.ServiceCode);

                            var alreadyInProcess = await IsNativeAppIdApplicationIsAlreadyInprocess(serviceGatewayRequestJsonData.iPin, applicationTypeDetails.ApplicationType, applicationTypeDetails.ApplicationPurposeType);


                            if (alreadyInProcess.ResponseDataModel) // CHK-6     
                            {
                                // Check application Approved
                                var isGrantApproval = await DoesIPinHasGrantApproval(serviceGatewayRequestJsonData.iPin, applicationTypeDetails.ApplicationType, applicationTypeDetails.ApplicationPurposeType);

                                Application appInProcess = null;
                                if (isGrantApproval.ResponseDataModel == null)
                                {
                                    var rsep = await GetNotSubmittedApplication(serviceGatewayRequestJsonData.iPin, applicationTypeDetails.ApplicationType, applicationTypeDetails.ApplicationPurposeType);
                                    appInProcess = rsep.ResponseDataModel;
                                }

                                if (isGrantApproval.ResponseDataModel != null)  //&& applicationTypeDetails.ApplicationPurposeType!=ApplicationPurposeTypeEnum.GRANT_LICENCE
                                {
                                    serviceGatewayResponse = await OpenServiceApplicationForm(genericServiceResultTemplate, requestLog, serviceGatewayRequestJsonData, false, "", true, false, httpRequest);
                                }
                                else if (appInProcess != null)
                                {
                                    serviceGatewayResponse.CanApply = false;
                                    serviceGatewayResponse.RequestDeniedReason = "This service has already been applied/approved. (CHK-6)";
                                    serviceGatewayResponse.ServiceCode = (int)applicationTypeDetails.ApplicationType;
                                    serviceGatewayResponse.NativeAppId = appInProcess.AppId;
                                }
                                else
                                {
                                    serviceGatewayResponse.CanApply = false;
                                    serviceGatewayResponse.RequestDeniedReason = "This service has already been applied/approved. (CHK-6)";
                                    serviceGatewayResponse.ServiceCode = (int)applicationTypeDetails.ApplicationType;
                                }
                            }
                            else // CHK-6 -- Open application form
                            {
                                if (serviceGatewayRequestJsonData.ServiceCode == 61) // Night Shift (Shop)
                                {

                                    var Sys_N_ServiceCodeInfo = FindServiceCodeMappingWithApplicationType(serviceGatewayRequestJsonData.ServiceCode);

                                    // Find Licence no
                                    List<StoreProcedureParm> storeProcedureParms = new List<StoreProcedureParm>()
                                {
                                    new StoreProcedureParm (){ ParmName="LicenseNo", ParmValue=serviceGatewayRequestJsonData.FileNo.ToString(), isNumber=false},
                                    new StoreProcedureParm (){ ParmName="ApplicationType", ParmValue= ((int)Sys_N_ServiceCodeInfo.ApplicationType).ToString(), isNumber=true}
                                };
                                    var sameAppType = await _iGeneric_SP_Repository.CallStoreProcedureReaderAsync<IntReturn>("dbo.sp_DetermineLicenceNoHasSameApplicationType", storeProcedureParms);
                                    if (serviceGatewayRequestJsonData.CategoryType == "FT" && _context.Licence_Shop_NightShift_Approvals.Where(x => x.LicenceNumber == serviceGatewayRequestJsonData.FileNo).Count() > 0)
                                    {
                                        // Show message - 
                                        serviceGatewayResponse.CanApply = false;
                                        serviceGatewayResponse.RequestDeniedReason = "Provided Licence no does not match with selected service type. Please check and try again..!";

                                    }
                                    else if (sameAppType.Select(x => x.Value).FirstOrDefault() == 0)
                                    {
                                        serviceGatewayResponse.CanApply = false;
                                        serviceGatewayResponse.RequestDeniedReason = "You have already applied this service under the provided licence no. Please check and try again..!";
                                    }
                                    else
                                    {
                                        serviceGatewayResponse = await OpenServiceApplicationForm(genericServiceResultTemplate, requestLog, serviceGatewayRequestJsonData, false, "", true, false, httpRequest);
                                    }
                                }
                                if (serviceGatewayRequestJsonData.ServiceCode == 62) // Night Shift (Factory)
                                {

                                    var Sys_N_ServiceCodeInfo = FindServiceCodeMappingWithApplicationType(serviceGatewayRequestJsonData.ServiceCode);

                                    // Find Licence no
                                    List<StoreProcedureParm> storeProcedureParms = new List<StoreProcedureParm>()
                                {
                                    new StoreProcedureParm (){ ParmName="LicenseNo", ParmValue=serviceGatewayRequestJsonData.FileNo.ToString(), isNumber=false},
                                    new StoreProcedureParm (){ ParmName="ApplicationType", ParmValue= ((int)Sys_N_ServiceCodeInfo.ApplicationType).ToString(), isNumber=true}
                                };
                                    var sameAppType = await _iGeneric_SP_Repository.CallStoreProcedureReaderAsync<IntReturn>("dbo.sp_DetermineLicenceNoHasSameApplicationType", storeProcedureParms);
                                    if (serviceGatewayRequestJsonData.CategoryType == "FT" &&
                                        _context.Applications.Where(x => x.Legacy_LicenceNo == serviceGatewayRequestJsonData.FileNo && x.IsDeleted == false).FirstOrDefault().ApplicationType != ApplicationTypeEnum.FACTORY_LICENCE &&
                                        _context.Applications.Where(x => x.InvestPunjab_Ipin == serviceGatewayRequestJsonData.iPin.ToString() && x.IsDeleted == false && x.ApplicationType == ApplicationTypeEnum.WOMEN_NIGHT_SHIFT_FACTORY && x.ApplicationLifeCycleStatusType != ApplicationLifeCycleStatusTypeEnum.APPROVED).Count() > 0)
                                    {
                                        // Show message - 
                                        serviceGatewayResponse.CanApply = false;
                                        serviceGatewayResponse.RequestDeniedReason = "Provided Licence no does not match with selected service type and service already has been applied. Please check and try again..!";

                                    }
                                    else if (sameAppType.Select(x => x.Value).FirstOrDefault() == 0)
                                    {
                                        serviceGatewayResponse.CanApply = false;
                                        serviceGatewayResponse.RequestDeniedReason = "You have already applied this service under the provided licence no. Please check and try again..!";
                                    }
                                    else
                                    {
                                        serviceGatewayResponse = await OpenServiceApplicationForm(genericServiceResultTemplate, requestLog, serviceGatewayRequestJsonData, false, "", true, false, httpRequest);
                                    }
                                }
                                else
                                {
                                    if (appType_Renewal.Contains(serviceGatewayRequestJsonData.ServiceCode) || appType_Amendment.Contains(serviceGatewayRequestJsonData.ServiceCode)) // CHK-1 -- Renewal OR Amendment
                                    {

                                        var foundApplications = _context.Applications.Where(x => x.InvestPunjab_Ipin == serviceGatewayRequestJsonData.iPin.ToString() && x.IsDeleted == false
                                           && x.ApplicationType == applicationTypeDetails.ApplicationType
                                           && (x.ApplicationLifeCycleStatusType == ApplicationLifeCycleStatusTypeEnum.APPROVED || x.ApplicationLifeCycleStatusType == ApplicationLifeCycleStatusTypeEnum.WITHDRAW_APPLICATION || x.ApplicationLifeCycleStatusType == ApplicationLifeCycleStatusTypeEnum.REJECTED)
                                           ).OrderByDescending(x => x.AppId).ToList();

                                        Application application = new Application();

                                        if (foundApplications.Where(x => x.ApplicationLifeCycleStatusType == ApplicationLifeCycleStatusTypeEnum.APPROVED).Count() > 0)
                                        {
                                            application = foundApplications.Where(x => x.ApplicationLifeCycleStatusType == ApplicationLifeCycleStatusTypeEnum.APPROVED).FirstOrDefault();
                                        }
                                        else
                                        {
                                            application = foundApplications.FirstOrDefault();
                                        }


                                        if (application == null && licenceInfo != null)
                                        {
                                            application = _context.Applications.Where(x => x.AppId == licenceInfo.AppRefId && x.IsDeleted == false).OrderByDescending(x => x.AppId).FirstOrDefault();
                                        }

                                        Int64 projectSiteId = application.ProjectSiteRefId;
                                        var projectSiteResp = await _iProjectSiteService.GetProjectSiteRefIdByIpin(serviceGatewayRequestJsonData.iPin.ToString());
                                        if (projectSiteResp.ResponseDataModel > 0)
                                        {
                                            projectSiteId = projectSiteResp.ResponseDataModel;
                                        }
                                        else if (projectSiteResp.ResponseDataModel == 0)
                                        {

                                            GenericResponseTemplateModel<BusinessFirstSCAFViewModel> genericResponseTemplateModel = await GetBusinessFirstCafDataByIPin(requestLog.IPin);

                                            var factoryCircleId = 22;
                                            var labourCircleId = 10001;

                                            var comLGDDistId = Convert.ToInt64(genericResponseTemplateModel.ResponseDataModel.CafInfo.ComLGDDistId);
                                            var factoryCircle = _context.FactoryCircles.Where(x => x.DistrictLgdRefId == (comLGDDistId == 737 ? 43 : comLGDDistId) && x.Version == 2).FirstOrDefault();


                                            ProjectSite projectSites = new ProjectSite()
                                            {
                                                EstablishmentName = genericResponseTemplateModel.ResponseDataModel.CafInfo.EstbName,
                                                Address = genericResponseTemplateModel.ResponseDataModel.CafInfo.SiteAddress == null ? "" : genericResponseTemplateModel.ResponseDataModel.CafInfo.SiteAddress,
                                                VillageOrTown = genericResponseTemplateModel.ResponseDataModel.CafInfo.SiteVlgName == null ? "NA" : genericResponseTemplateModel.ResponseDataModel.CafInfo.SiteVlgName,
                                                TehsilRefId = Convert.ToInt64(genericResponseTemplateModel.ResponseDataModel.CafInfo.SiteLGDTehId),
                                                DistrictRefId = Convert.ToInt64(genericResponseTemplateModel.ResponseDataModel.CafInfo.SiteLGDDistId),
                                                PinCode = genericResponseTemplateModel.ResponseDataModel.CafInfo.SitePin == null ? "000000" : genericResponseTemplateModel.ResponseDataModel.CafInfo.SitePin,
                                                IsActive = true,
                                                IsDeleted = false,
                                                Createddate = DateTime.Now,
                                                LastModifiedDate = DateTime.Now,
                                                UserRefId = requestLog.UserId,
                                                LabourCircleRefId = labourCircleId,
                                                FactoryCircleRefId = factoryCircle.FactoryCircleId,
                                                ApplicantAadharNumber = genericResponseTemplateModel.ResponseDataModel.CafInfo.AadharNum,
                                                ApplicantAadharAttachment = genericResponseTemplateModel.ResponseDataModel.CafInfo.SC_Aadhar,
                                                ApplicantPanNumber = genericResponseTemplateModel.ResponseDataModel.CafInfo.ApplicantPan,
                                                ApplicantPanAttachment = genericResponseTemplateModel.ResponseDataModel.CafInfo.ApplicantPanAttachment,
                                                CompanyPanNumber = genericResponseTemplateModel.ResponseDataModel.CafInfo.CompanyPan,
                                                CompanyPanAttachment = genericResponseTemplateModel.ResponseDataModel.CafInfo.CompanyPanAttachment,
                                                ProjectPurpose = genericResponseTemplateModel.ResponseDataModel.CafInfo.ProjectPurpose,
                                                ContactPersonFirstName = genericResponseTemplateModel.ResponseDataModel.CafInfo.First_Name,
                                                ContactPersonMiddleName = genericResponseTemplateModel.ResponseDataModel.CafInfo.Middle_Name == null ? "" : genericResponseTemplateModel.ResponseDataModel.CafInfo.Middle_Name,
                                                ContactPersonLastName = genericResponseTemplateModel.ResponseDataModel.CafInfo.Last_Name == null ? "." : genericResponseTemplateModel.ResponseDataModel.CafInfo.Last_Name,
                                                ContactPersonEmail = genericResponseTemplateModel.ResponseDataModel.CafInfo.Email,
                                                ContactPersonMobileNo = genericResponseTemplateModel.ResponseDataModel.CafInfo.MobileNo.Substring(genericResponseTemplateModel.ResponseDataModel.CafInfo.MobileNo.Length - 10),
                                                AlternateMobileNo = genericResponseTemplateModel.ResponseDataModel.CafInfo.MobileNo.Substring(genericResponseTemplateModel.ResponseDataModel.CafInfo.MobileNo.Length - 10),
                                                AlternateEmail = genericResponseTemplateModel.ResponseDataModel.CafInfo.AltEmail == "" ? genericResponseTemplateModel.ResponseDataModel.CafInfo.Email : genericResponseTemplateModel.ResponseDataModel.CafInfo.AltEmail,
                                                ProjectSiteVersion = 1
                                            };
                                            var projectSiteDetails = await _iProjectSiteService.CreateProjectSite(projectSites, true, requestLog.IPin.ToString());
                                            projectSiteId = projectSiteDetails.ResponseDataModel.ProjectSiteId;

                                        }

                                        if (application != null)
                                        {
                                            var projectSite = _context.ProjectSites.Where(x => x.ProjectSiteId == application.ProjectSiteRefId).FirstOrDefault();
                                            if (application.ApplicationType == ApplicationTypeEnum.FACTORY_LICENCE)
                                            {
                                                var factGeneralDetails = _context.Licence_Factory_GeneralDetails.Where(x => x.AppRefId == application.AppId).FirstOrDefault();
                                                //if(application.ApplicationPurposeType == ApplicationPurposeTypeEnum.RENEWAL_LICENCE) 
                                                //{
                                                //    factGeneralDetails.OldLicenceValidUpTo = new DateTime(factGeneralDetails.RenewalFromDate.Value.Year + factGeneralDetails.NoOfYears -1, 12, 31);
                                                //}
                                                //var factOccupierDetails = _context.Licence_Factory_OccupierAndManagerDetails.Where(x => x.FactoryLicenceRefId == factGeneralDetails.FactoryLicenceId).FirstOrDefault();
                                                //var docs = _context.ApplicationDocuments.Where(x=>x.AppRefId == application.AppId).ToList();
                                                var Sys_N_ServiceCodeInfo = FindServiceCodeMappingWithApplicationType(serviceGatewayRequestJsonData.ServiceCode);
                                                Int64 oldFactoryRefId = factGeneralDetails.FactoryLicenceId;

                                                var statusResp = await _iApplicationMamnagement_Factory_Service.InitiateApplication(ApplicationTypeEnum.FACTORY_LICENCE, factGeneralDetails, application.ProjectSiteRefId, Sys_N_ServiceCodeInfo.ApplicationPurposeType, projectSite.UserRefId, serviceGatewayRequestJsonData.iPin, Convert.ToInt64(serviceGatewayRequestJsonData.AppId), false, application.AppId, serviceGatewayRequestJsonData.ServiceCode, "NA", serviceGatewayRequestJsonData.FileNo, application.ProjectSiteVersion == 0 ? 1 : application.ProjectSiteVersion);
                                                var newAppId = statusResp.AppId;

                                                var latestFactoryDetails = _context.Licence_Factory_GeneralDetails.Where(x => x.FactoryLicenceId == statusResp.EntityKeyId).FirstOrDefault();
                                                //latestFactoryDetails.OldLicenceValidUpTo = new DateTime(factGeneralDetails.AmmendmentDate != null ? factGeneralDetails.AmmendmentDate.Value.Year :
                                                //    ((factGeneralDetails.RenewalFromDate == null ? factGeneralDetails.RegistrationDate.Value.Year : factGeneralDetails.RenewalFromDate.Value.Year)
                                                //    + factGeneralDetails.NoOfYears - 1), 12, 31);


                                                if (application.ApplicationPurposeType == ApplicationPurposeTypeEnum.GRANT_LICENCE)
                                                {
                                                    latestFactoryDetails.OldLicenceValidUpTo = new DateTime((factGeneralDetails.RegistrationDate.Value.Year + (factGeneralDetails.NoOfYears - 1)), 12, 31);
                                                }

                                                if (application.ApplicationPurposeType == ApplicationPurposeTypeEnum.RENEWAL_LICENCE)
                                                {
                                                    latestFactoryDetails.OldLicenceValidUpTo = new DateTime((factGeneralDetails.RenewalFromDate.Value.Year + (factGeneralDetails.NoOfYears - 1)), 12, 31);
                                                }

                                                if (application.ApplicationPurposeType == ApplicationPurposeTypeEnum.AMENDMENT_LICENCE)
                                                {
                                                    latestFactoryDetails.OldLicenceValidUpTo = factGeneralDetails.OldLicenceValidUpTo;
                                                }

                                                if (appType_Renewal.Contains(serviceGatewayRequestJsonData.ServiceCode))
                                                {
                                                    latestFactoryDetails.RenewalFromDate = latestFactoryDetails.OldLicenceValidUpTo.Value.AddDays(1);
                                                    var licenceNo = _context.ApplicationLicenceNoMapping.Where(x => x.AppRefId == application.AppId).Select(x => x.LicenceNumber).FirstOrDefault();
                                                    latestFactoryDetails.OldLicenceNo = licenceNo;
                                                    latestFactoryDetails.OldLicenceTotalEmployees = (int)(factGeneralDetails.Workers_OrdinarilyEmployed);
                                                    latestFactoryDetails.OldLicenceFactoryKiloWatt = (int)(factGeneralDetails.PowerKW_Installed);
                                                    latestFactoryDetails.Workers_MaxLast12Month = (int)(factGeneralDetails.Workers_MaxDuringYear);
                                                }

                                                var factOccupierDetails = _context.Licence_Factory_OccupierAndManagerDetails.Where(x => x.FactoryLicenceRefId == oldFactoryRefId).FirstOrDefault();
                                                var docs = _context.ApplicationDocuments.Where(x => x.AppRefId == application.AppId).ToList();


                                                // Update Native Appid by IPIN
                                                var temp = await UpdateBusinessFirstRequestNativeAppIdByIPin(serviceGatewayRequestJsonData.iPin, Convert.ToInt64(serviceGatewayRequestJsonData.AppId), newAppId, ApplicationTypeEnum.FACTORY_LICENCE, Sys_N_ServiceCodeInfo.ApplicationPurposeType);

                                                if (factOccupierDetails != null)
                                                {
                                                    factOccupierDetails.FactoryLicenceRefId = statusResp.EntityKeyId;
                                                    factOccupierDetails.OccupierAndManagerDetailId = 0;
                                                    await _context.Licence_Factory_OccupierAndManagerDetails.AddAsync(factOccupierDetails);
                                                }

                                                docs = docs.Select(x => { x.LastModifiedDate = DateTime.Now; x.AppRefId = newAppId; return x; }).ToList();
                                                _context.BulkInsert<ApplicationDocument>(docs);

                                                await ShareStatusToBusinessFirst(newAppId, application.ApplicationType, AppActionTypeEnum.APP_SAVE_DRAFT);

                                                requestLog.NativeAppId = newAppId;
                                                genericServiceResultTemplate.ResponseDataModel.NativeAppId = newAppId;
                                                serviceGatewayResponse = await OpenServiceApplicationForm(genericServiceResultTemplate, requestLog, serviceGatewayRequestJsonData, false, "", true, true, httpRequest);
                                            }
                                            else if (application.ApplicationType == ApplicationTypeEnum.SHOP_LICENCE)
                                            {
                                                var shopGeneralDetails = _context.ShopLicence_GeneralDetails.Where(x => x.AppRefId == application.AppId).FirstOrDefault();
                                                var shopEmployeeDetails = _context.ShopLicence_EmployeeDetails.Where(x => x.ShopLicenceRefId == shopGeneralDetails.ShopLicenceId).ToList();
                                                var docs = _context.ApplicationDocuments.Where(x => x.AppRefId == application.AppId).ToList();
                                                var Sys_N_ServiceCodeInfo = FindServiceCodeMappingWithApplicationType(serviceGatewayRequestJsonData.ServiceCode);
                                                ApplicationInitiateResponseViewModel statusResp = new ApplicationInitiateResponseViewModel();
                                                Int64 nAppId = 0;
                                                Int64 entityKeyId = 0;

                                                statusResp = await _iApplicationMamnagement_Shop_Service.InitiateApplication(ApplicationTypeEnum.SHOP_LICENCE, shopGeneralDetails, application.ProjectSiteRefId, Sys_N_ServiceCodeInfo.ApplicationPurposeType, projectSite.UserRefId, serviceGatewayRequestJsonData.iPin, Convert.ToInt64(serviceGatewayRequestJsonData.AppId), false, application.AppId, serviceGatewayRequestJsonData.ServiceCode, "NA", serviceGatewayRequestJsonData.FileNo, application.ProjectSiteVersion == 0 ? 1 : application.ProjectSiteVersion);
                                                nAppId = statusResp.AppId;
                                                entityKeyId = statusResp.EntityKeyId;

                                                // Update Native Appid by IPIN
                                                var temp = await UpdateBusinessFirstRequestNativeAppIdByIPin(serviceGatewayRequestJsonData.iPin, Convert.ToInt64(serviceGatewayRequestJsonData.AppId), nAppId, ApplicationTypeEnum.SHOP_LICENCE, Sys_N_ServiceCodeInfo.ApplicationPurposeType);

                                                if (shopGeneralDetails.IsHavingEmployee == 1)
                                                {
                                                    shopEmployeeDetails = shopEmployeeDetails.Select(x => { x.ShopLicenceRefId = entityKeyId; x.EmployeeDetailId = 0; return x; }).ToList();
                                                    //shopEmployeeDetails.ShopLicenceRefId = statusResp.EntityKeyId;
                                                    //shopEmployeeDetails.EmployeeDetailId = 0;
                                                    //await _context.ShopLicence_EmployeeDetails.(shopEmployeeDetails);

                                                    var existingEmps = _context.ShopLicence_EmployeeDetails.Where(x => x.ShopLicenceRefId == entityKeyId).ToList();
                                                    if (existingEmps.Count() > 0)
                                                    {
                                                        _context.BulkDelete<ShopLicence_EmployeeDetail>(existingEmps);
                                                        _context.SaveChanges();
                                                    }
                                                    _context.BulkInsert<ShopLicence_EmployeeDetail>(shopEmployeeDetails);
                                                    _context.SaveChanges();
                                                }

                                                docs = docs.Select(x => { x.LastModifiedDate = DateTime.Now; x.AppRefId = nAppId; return x; }).ToList();
                                                _context.BulkInsert<ApplicationDocument>(docs);
                                                _context.SaveChanges();


                                                await ShareStatusToBusinessFirst(nAppId, application.ApplicationType, AppActionTypeEnum.APP_SAVE_DRAFT);

                                                requestLog.NativeAppId = nAppId;
                                                genericServiceResultTemplate.ResponseDataModel.NativeAppId = nAppId;
                                                serviceGatewayResponse = await OpenServiceApplicationForm(genericServiceResultTemplate, requestLog, serviceGatewayRequestJsonData, false, "", true, true, httpRequest);
                                            }
                                            else if (application.ApplicationType == ApplicationTypeEnum.PRINCIPAL_EMPLOYER)
                                            {

                                                var contractordetails = await GetCLPEContractorListNew(serviceGatewayRequestJsonData.FileNo);
                                                var principalEmployerGeneralDetails = _context.Licence_CL_PE_GeneralDetail.Where(x => x.AppRefId == application.AppId).FirstOrDefault();
                                                var sys_N_ServiceCodeInfo = FindServiceCodeMappingWithApplicationType(serviceGatewayRequestJsonData.ServiceCode);
                                                Int64 oldPrincipalEmployerRegistrationRefId = principalEmployerGeneralDetails.Id;

                                                var statusResp = await _iApplicationMamnagement_CL_PE_Service.InitiateApplication(ApplicationTypeEnum.PRINCIPAL_EMPLOYER, principalEmployerGeneralDetails, application.ProjectSiteRefId, sys_N_ServiceCodeInfo.ApplicationPurposeType, projectSite.UserRefId, serviceGatewayRequestJsonData.iPin, Convert.ToInt64(serviceGatewayRequestJsonData.AppId), false, application.AppId, serviceGatewayRequestJsonData.ServiceCode, "NA", serviceGatewayRequestJsonData.FileNo, application.ProjectSiteVersion == 0 ? 1 : application.ProjectSiteVersion);
                                                var newAppId = statusResp.AppId;
                                                _context.SaveChanges();



                                                var contractors = _context.Licence_CL_PE_Contrator.Where(x => x.Licence_CL_PE_GeneralDetailRefId == oldPrincipalEmployerRegistrationRefId).ToList();
                                                var totalMaxLabourWorkers = contractors.Sum(x => x.MaxLabourWorkerEmployed);
                                                totalMaxLabourWorkers = contractordetails.FormModel.Sum(x => x.MaxLabourWorkerEmployed);
                                                Licence_CL_PE_GeneralDetail previousPELicenseData = null;
                                                previousPELicenseData = _context.Licence_CL_PE_GeneralDetail.Where(x => x.AppRefId == newAppId).FirstOrDefault();
                                                previousPELicenseData.TotalWorker = totalMaxLabourWorkers;
                                                _context.Licence_CL_PE_GeneralDetail.Update(previousPELicenseData);
                                                await _context.SaveChangesAsync();

                                                if (contractordetails.FormModel.Count() > 0)
                                                {

                                                    var existingcontractors = _context.Licence_CL_PE_Contrator.Where(x => x.Licence_CL_PE_GeneralDetailRefId == statusResp.EntityKeyId).ToList();
                                                    if (existingcontractors.Count() > 0)
                                                    {
                                                        _context.BulkDelete<Licence_CL_PE_Contrator>(existingcontractors);
                                                        _context.SaveChanges();
                                                    }


                                                    foreach (var item in contractordetails.FormModel)
                                                    {
                                                        existingcontractors.Add(new Licence_CL_PE_Contrator()
                                                        {

                                                            Name = item.Name,
                                                            Address = item.Address,
                                                            NatureOfWork = item.NatureOfWork,
                                                            MaxLabourWorkerEmployed = item.MaxLabourWorkerEmployed,
                                                            DateOfCommencement = item.DateOfCommencement,
                                                            DateOfTermination = item.DateOfTermination,
                                                            ModifiedCounter = 1,
                                                            Licence_CL_PE_GeneralDetailRefId = statusResp.EntityKeyId
                                                        });
                                                    }

                                                    _context.BulkInsert<Licence_CL_PE_Contrator>(existingcontractors);
                                                    _context.SaveChanges();
                                                }
                                                var docs = _context.ApplicationDocuments.Where(x => x.AppRefId == application.AppId).ToList();


                                                // Update Native Appid by IPIN
                                                var temp = await UpdateBusinessFirstRequestNativeAppIdByIPin(serviceGatewayRequestJsonData.iPin, Convert.ToInt64(serviceGatewayRequestJsonData.AppId), newAppId, ApplicationTypeEnum.PRINCIPAL_EMPLOYER, sys_N_ServiceCodeInfo.ApplicationPurposeType);


                                                docs = docs.Select(x => { x.LastModifiedDate = DateTime.Now; x.AppRefId = newAppId; return x; }).ToList();
                                                _context.BulkInsert<ApplicationDocument>(docs);

                                                await ShareStatusToBusinessFirst(newAppId, application.ApplicationType, AppActionTypeEnum.APP_SAVE_DRAFT);

                                                requestLog.NativeAppId = newAppId;
                                                genericServiceResultTemplate.ResponseDataModel.NativeAppId = newAppId;
                                                serviceGatewayResponse = await OpenServiceApplicationForm(genericServiceResultTemplate, requestLog, serviceGatewayRequestJsonData, false, "", true, true, httpRequest);


                                            }
                                            else if (application.ApplicationType == ApplicationTypeEnum.CONTRACT_LABOUR)
                                            {

                                                var contractLabourGeneralDetails = _context.Licence_ContractLabour_GeneralDetails.Where(x => x.AppRefId == application.AppId).FirstOrDefault();
                                                var sys_N_ServiceCodeInfo = FindServiceCodeMappingWithApplicationType(serviceGatewayRequestJsonData.ServiceCode);
                                                Int64 oldPrincipalEmployerRegistrationRefId = contractLabourGeneralDetails.ContractLabourId;
                                                if (serviceGatewayRequestJsonData.ServiceCode == 12)
                                                {
                                                    contractLabourGeneralDetails.LicenceForYear = contractLabourGeneralDetails.LicenceForYear + 1;
                                                }

                                                var statusResp = await _iApplicationMamnagement_ContractLabour_Service.InitiateApplication(ApplicationTypeEnum.CONTRACT_LABOUR, contractLabourGeneralDetails, projectSiteId, sys_N_ServiceCodeInfo.ApplicationPurposeType, projectSite.UserRefId, serviceGatewayRequestJsonData.iPin, Convert.ToInt64(serviceGatewayRequestJsonData.AppId), false, application.AppId, serviceGatewayRequestJsonData.ServiceCode, "NA", serviceGatewayRequestJsonData.FileNo, application.ProjectSiteVersion == 0 ? 1 : application.ProjectSiteVersion);
                                                var newAppId = statusResp.AppId;
                                                _context.SaveChanges();
                                                var docs = _context.ApplicationDocuments.Where(x => x.AppRefId == application.AppId).ToList();


                                                // Update Native Appid by IPIN
                                                var temp = await UpdateBusinessFirstRequestNativeAppIdByIPin(serviceGatewayRequestJsonData.iPin, Convert.ToInt64(serviceGatewayRequestJsonData.AppId), newAppId, ApplicationTypeEnum.CONTRACT_LABOUR, sys_N_ServiceCodeInfo.ApplicationPurposeType);


                                                docs = docs.Select(x => { x.LastModifiedDate = DateTime.Now; x.AppRefId = newAppId; return x; }).ToList();
                                                _context.BulkInsert<ApplicationDocument>(docs);

                                                await ShareStatusToBusinessFirst(newAppId, application.ApplicationType, AppActionTypeEnum.APP_SAVE_DRAFT);

                                                requestLog.NativeAppId = newAppId;
                                                genericServiceResultTemplate.ResponseDataModel.NativeAppId = newAppId;
                                                serviceGatewayResponse = await OpenServiceApplicationForm(genericServiceResultTemplate, requestLog, serviceGatewayRequestJsonData, false, "", true, true, httpRequest);

                                            }
                                            else if (application.ApplicationType == ApplicationTypeEnum.BOCW_ESTABLISHMENT_ACT)
                                            {

                                                var bocwEstablishmentGeneralDetails = _context.Licence_BocwAct_GeneralDetails.Where(x => x.AppRefId == application.AppId).FirstOrDefault();
                                                var sys_N_ServiceCodeInfo = FindServiceCodeMappingWithApplicationType(serviceGatewayRequestJsonData.ServiceCode);
                                                Int64 oldBocwEstablishmentRegistrationRefId = bocwEstablishmentGeneralDetails.BocwEstablishmentRegistrationId;


                                                var statusResp = await _iApplicationMamnagement_Licence_BocwAct_GeneralDetail.InitiateApplication(ApplicationTypeEnum.BOCW_ESTABLISHMENT_ACT, bocwEstablishmentGeneralDetails, application.ProjectSiteRefId, sys_N_ServiceCodeInfo.ApplicationPurposeType, projectSite.UserRefId, serviceGatewayRequestJsonData.iPin, Convert.ToInt64(serviceGatewayRequestJsonData.AppId), false, application.AppId, serviceGatewayRequestJsonData.ServiceCode, "NA", serviceGatewayRequestJsonData.FileNo, application.ProjectSiteVersion == 0 ? 1 : application.ProjectSiteVersion);
                                                var newAppId = statusResp.AppId;
                                                _context.SaveChanges();


                                                var contractors = _context.Licence_BocwAct_ContractorDetails.Where(x => x.BocwEstablishmentRegistrationRefId == oldBocwEstablishmentRegistrationRefId).ToList();

                                                if (contractors.Count() > 1)
                                                {
                                                    contractors = contractors.Select(x => { x.BocwEstablishmentRegistrationRefId = statusResp.EntityKeyId; return x; }).ToList();

                                                    var existingcontractors = _context.Licence_BocwAct_ContractorDetails.Where(x => x.BocwEstablishmentRegistrationRefId == statusResp.EntityKeyId).ToList();
                                                    if (existingcontractors.Count() > 0)
                                                    {
                                                        _context.BulkDelete<Licence_BocwAct_ContractorDetail>(existingcontractors);
                                                        _context.SaveChanges();
                                                    }
                                                    _context.BulkInsert<Licence_BocwAct_ContractorDetail>(contractors);
                                                    _context.SaveChanges();
                                                }

                                                var docs = _context.ApplicationDocuments.Where(x => x.AppRefId == application.AppId).ToList();


                                                // Update Native Appid by IPIN
                                                var temp = await UpdateBusinessFirstRequestNativeAppIdByIPin(serviceGatewayRequestJsonData.iPin, Convert.ToInt64(serviceGatewayRequestJsonData.AppId), newAppId, ApplicationTypeEnum.BOCW_ESTABLISHMENT_ACT, sys_N_ServiceCodeInfo.ApplicationPurposeType);


                                                docs = docs.Select(x => { x.LastModifiedDate = DateTime.Now; x.AppRefId = newAppId; return x; }).ToList();
                                                _context.BulkInsert<ApplicationDocument>(docs);

                                                //await ShareStatusToBusinessFirst(newAppId, application.ApplicationType, AppActionTypeEnum.APP_SAVE_DRAFT);

                                                requestLog.NativeAppId = newAppId;
                                                genericServiceResultTemplate.ResponseDataModel.NativeAppId = newAppId;
                                                serviceGatewayResponse = await OpenServiceApplicationForm(genericServiceResultTemplate, requestLog, serviceGatewayRequestJsonData, false, "", true, true, httpRequest);

                                            }
                                            else if (application.ApplicationType == ApplicationTypeEnum.ISM_PRINCIPAL_EMPLOYER)
                                            {

                                                var ismprincipalEmployerGeneralDetails = _context.Licence_PE_ISM_GeneralDetails.Where(x => x.AppRefId == application.AppId).FirstOrDefault();
                                                var sys_N_ServiceCodeInfo = FindServiceCodeMappingWithApplicationType(serviceGatewayRequestJsonData.ServiceCode);
                                                Int64 oldismPrincipalEmployerRegistrationRefId = ismprincipalEmployerGeneralDetails.Id;

                                                var statusResp = await _iApplicationMamnagement_Licence_ISM_PrincipalEmployer.InitiateApplication(ApplicationTypeEnum.ISM_PRINCIPAL_EMPLOYER, ismprincipalEmployerGeneralDetails, application.ProjectSiteRefId, sys_N_ServiceCodeInfo.ApplicationPurposeType, projectSite.UserRefId, serviceGatewayRequestJsonData.iPin, Convert.ToInt64(serviceGatewayRequestJsonData.AppId), false, application.AppId, serviceGatewayRequestJsonData.ServiceCode, "NA", serviceGatewayRequestJsonData.FileNo, application.ProjectSiteVersion == 0 ? 1 : application.ProjectSiteVersion);
                                                var newAppId = statusResp.AppId;
                                                _context.SaveChanges();



                                                var contractors = _context.Licence_PE_ISM_Contrators.Where(x => x.Licence_PE_ISM_GeneralDetailId == oldismPrincipalEmployerRegistrationRefId).ToList();
                                                var totalMaxLabourWorkers = contractors.Sum(x => x.MaxLabourWorkerEmployed);

                                                Licence_PE_ISM_GeneralDetail previousISMPELicenseData = null;
                                                previousISMPELicenseData = _context.Licence_PE_ISM_GeneralDetails.Where(x => x.AppRefId == newAppId).FirstOrDefault();
                                                previousISMPELicenseData.TotalWorker = totalMaxLabourWorkers;
                                                _context.Licence_PE_ISM_GeneralDetails.Update(previousISMPELicenseData);
                                                await _context.SaveChangesAsync();

                                                if (contractors.Count() > 1)
                                                {

                                                    var existingcontractors = _context.Licence_PE_ISM_Contrators.Where(x => x.Licence_PE_ISM_GeneralDetailId == statusResp.EntityKeyId).ToList();
                                                    if (existingcontractors.Count() > 0)
                                                    {
                                                        _context.BulkDelete<Licence_PE_ISM_Contrator>(existingcontractors);
                                                        _context.SaveChanges();
                                                    }


                                                    foreach (var item in contractors)
                                                    {
                                                        existingcontractors.Add(new Licence_PE_ISM_Contrator()
                                                        {

                                                            Name = item.Name,
                                                            Address = item.Address,
                                                            NatureOfWork = item.NatureOfWork,
                                                            MaxLabourWorkerEmployed = item.MaxLabourWorkerEmployed,
                                                            DateOfCommencement = item.DateOfCommencement,
                                                            DateOfTermination = item.DateOfTermination,
                                                            ModifiedCounter = 1,
                                                            Licence_PE_ISM_GeneralDetailId = statusResp.EntityKeyId
                                                        });
                                                    }

                                                    _context.BulkInsert<Licence_PE_ISM_Contrator>(existingcontractors);
                                                    _context.SaveChanges();
                                                }
                                                var docs = _context.ApplicationDocuments.Where(x => x.AppRefId == application.AppId).ToList();


                                                // Update Native Appid by IPIN
                                                var temp = await UpdateBusinessFirstRequestNativeAppIdByIPin(serviceGatewayRequestJsonData.iPin, Convert.ToInt64(serviceGatewayRequestJsonData.AppId), newAppId, ApplicationTypeEnum.ISM_PRINCIPAL_EMPLOYER, sys_N_ServiceCodeInfo.ApplicationPurposeType);


                                                docs = docs.Select(x => { x.LastModifiedDate = DateTime.Now; x.AppRefId = newAppId; return x; }).ToList();
                                                _context.BulkInsert<ApplicationDocument>(docs);

                                                await ShareStatusToBusinessFirst(newAppId, application.ApplicationType, AppActionTypeEnum.APP_SAVE_DRAFT);

                                                requestLog.NativeAppId = newAppId;
                                                genericServiceResultTemplate.ResponseDataModel.NativeAppId = newAppId;
                                                serviceGatewayResponse = await OpenServiceApplicationForm(genericServiceResultTemplate, requestLog, serviceGatewayRequestJsonData, false, "", true, true, httpRequest);


                                            }
                                            else if (application.ApplicationType == ApplicationTypeEnum.ISM_CONTRACT_LABOUR)
                                            {

                                                var contractLabourGeneralDetails = _context.Licence_ISM_ContractLabour_GeneralDetails.Where(x => x.AppRefId == application.AppId).FirstOrDefault();
                                                var sys_N_ServiceCodeInfo = FindServiceCodeMappingWithApplicationType(serviceGatewayRequestJsonData.ServiceCode);
                                                Int64 oldISMCLRegistrationRefId = contractLabourGeneralDetails.ISMContractLabourId;


                                                var statusResp = await _iApplicationMamnagement_Licence_ISM_ContractLabour.InitiateApplication(ApplicationTypeEnum.ISM_CONTRACT_LABOUR, contractLabourGeneralDetails, application.ProjectSiteRefId, sys_N_ServiceCodeInfo.ApplicationPurposeType, projectSite.UserRefId, serviceGatewayRequestJsonData.iPin, Convert.ToInt64(serviceGatewayRequestJsonData.AppId), false, application.AppId, serviceGatewayRequestJsonData.ServiceCode, "NA", serviceGatewayRequestJsonData.FileNo, application.ProjectSiteVersion == 0 ? 1 : application.ProjectSiteVersion);
                                                var newAppId = statusResp.AppId;
                                                _context.SaveChanges();
                                                var docs = _context.ApplicationDocuments.Where(x => x.AppRefId == application.AppId).ToList();


                                                // Update Native Appid by IPIN
                                                var temp = await UpdateBusinessFirstRequestNativeAppIdByIPin(serviceGatewayRequestJsonData.iPin, Convert.ToInt64(serviceGatewayRequestJsonData.AppId), newAppId, ApplicationTypeEnum.ISM_CONTRACT_LABOUR, sys_N_ServiceCodeInfo.ApplicationPurposeType);


                                                docs = docs.Select(x => { x.LastModifiedDate = DateTime.Now; x.AppRefId = newAppId; return x; }).ToList();
                                                _context.BulkInsert<ApplicationDocument>(docs);

                                                await ShareStatusToBusinessFirst(newAppId, application.ApplicationType, AppActionTypeEnum.APP_SAVE_DRAFT);

                                                requestLog.NativeAppId = newAppId;
                                                genericServiceResultTemplate.ResponseDataModel.NativeAppId = newAppId;
                                                serviceGatewayResponse = await OpenServiceApplicationForm(genericServiceResultTemplate, requestLog, serviceGatewayRequestJsonData, false, "", true, true, httpRequest);

                                            }
                                        }
                                        else
                                        {
                                            serviceGatewayResponse = await OpenServiceApplicationForm(genericServiceResultTemplate, requestLog, serviceGatewayRequestJsonData, false, "", true, false, httpRequest);
                                        }
                                    }
                                    else
                                    {
                                        serviceGatewayResponse = await OpenServiceApplicationForm(genericServiceResultTemplate, requestLog, serviceGatewayRequestJsonData, false, "", true, false, httpRequest);
                                    }
                                }
                            }
                        }

                        else // CHK-4 -- does not exist in SYS-N
                        {
                            var applicationTypeDetails = FindServiceCodeMappingWithApplicationType(serviceGatewayRequestJsonData.ServiceCode);
                            if (serviceGatewayRequestJsonData.ServiceCode == 3 || serviceGatewayRequestJsonData.ServiceCode == 11 || serviceGatewayRequestJsonData.ServiceCode == 4 || serviceGatewayRequestJsonData.ServiceCode == 12
                                || serviceGatewayRequestJsonData.ServiceCode == 13 || serviceGatewayRequestJsonData.ServiceCode == 17 || serviceGatewayRequestJsonData.ServiceCode == 18 || serviceGatewayRequestJsonData.ServiceCode == 19
                                || serviceGatewayRequestJsonData.ServiceCode == 20 || serviceGatewayRequestJsonData.ServiceCode == 21 || serviceGatewayRequestJsonData.ServiceCode == 35 || serviceGatewayRequestJsonData.ServiceCode == 36)
                            {
                                var createUserServiceResp = await CreateNewUser(serviceGatewayRequestJsonData.iPin, null);
                                if (createUserServiceResp.HasError)
                                {
                                    serviceGatewayResponse.CanApply = false;
                                    serviceGatewayResponse.RequestDeniedReason = "Issue in applying service. " + createUserServiceResp.ErrorDesc + " (CHK-7)";
                                }
                                else
                                {
                                    genericServiceResultTemplate.ResponseDataModel.NativeUserId = createUserServiceResp.ResponseDataModel.UserId;
                                    serviceGatewayResponse = await OpenServiceApplicationForm(genericServiceResultTemplate, requestLog, serviceGatewayRequestJsonData, false, "", false, true, httpRequest);
                                }
                            }
                            else
                            {

                                if (applicationTypeDetails.ApplicationPurposeType == ApplicationPurposeTypeEnum.AMENDMENT_LICENCE || applicationTypeDetails.ApplicationPurposeType == ApplicationPurposeTypeEnum.RENEWAL_LICENCE) // CHK-7
                                {
                                    // verify Old License. ************************** PENDING **************************

                                    var Sys_O_AppId_TokenInfo = await _iSystem_O_CommunicationService.DoesOldLicenceNoExists(serviceGatewayRequestJsonData.FileNo, applicationTypeDetails.ApplicationType);
                                    if (Sys_O_AppId_TokenInfo.ResponseDataModel != null)
                                    {
                                        var processImdResp = await ImportAndSeedData(Sys_O_AppId_TokenInfo.ResponseDataModel.AppId, serviceGatewayRequestJsonData.iPin.ToString(), requestLog.AppId, serviceGatewayRequestJsonData.ServiceCode, Sys_O_AppId_TokenInfo.ResponseDataModel.TokenNumber, serviceGatewayRequestJsonData.FileNo, false);
                                        if (processImdResp.ResponseDataModel != null)
                                        {
                                            requestLog.NativeAppId = processImdResp.ResponseDataModel.AppRefId;
                                            genericServiceResultTemplate.ResponseDataModel.NativeAppId = processImdResp.ResponseDataModel.AppRefId;
                                            serviceGatewayResponse = await OpenServiceApplicationForm(genericServiceResultTemplate, requestLog, serviceGatewayRequestJsonData, false, "", true, true, httpRequest);
                                        }
                                        else
                                        {
                                            serviceGatewayResponse.CanApply = false;
                                            serviceGatewayResponse.RequestDeniedReason = "Issue in opening form (CHK-7)..!";
                                        }
                                    }
                                    else
                                    {
                                        serviceGatewayResponse.CanApply = false;
                                        serviceGatewayResponse.RequestDeniedReason = "Licence no does not exixt..!";
                                    }
                                }
                                else // CHK-7 -- Create new user and open application form
                                {
                                    var createUserServiceResp = await CreateNewUser(serviceGatewayRequestJsonData.iPin, null);
                                    if (createUserServiceResp.HasError)
                                    {
                                        serviceGatewayResponse.CanApply = false;
                                        serviceGatewayResponse.RequestDeniedReason = "Issue in applying service. " + createUserServiceResp.ErrorDesc + " (CHK-7)";
                                    }
                                    else
                                    {
                                        genericServiceResultTemplate.ResponseDataModel.NativeUserId = createUserServiceResp.ResponseDataModel.UserId;
                                        serviceGatewayResponse = await OpenServiceApplicationForm(genericServiceResultTemplate, requestLog, serviceGatewayRequestJsonData, false, "", false, true, httpRequest);
                                    }
                                }
                            }
                            //var serResp = await _iSystem_O_CommunicationService.DoesIpinHasNativeUser(serviceGatewayRequestJsonData.iPin);
                            //if (!serResp.HasError)
                            //{
                            //    //CHK-5 -- Check is iPin and Native UserId exists SYS-O
                            //    if (serResp.ResponseDataModel > 0)
                            //    {
                            //        var processImdResp = await ImportAndSeedData(serResp.ResponseDataModel, serviceGatewayRequestJsonData.iPin.ToString(),requestLog.AppId, serviceGatewayRequestJsonData.ServiceCode,"", serviceGatewayRequestJsonData.FileNo);
                            //        if (processImdResp.ResponseDataModel != null)
                            //        {
                            //            requestLog.NativeAppId = processImdResp.ResponseDataModel.AppRefId;
                            //            serviceGatewayResponse = await OpenServiceApplicationForm(genericServiceResultTemplate, requestLog, serviceGatewayRequestJsonData, false, "", true, true);
                            //        }
                            //        else
                            //        {
                            //            serviceGatewayResponse.CanApply = false;
                            //            serviceGatewayResponse.RequestDeniedReason = "Issue in opening form (CHK-5)..!";
                            //        }
                            //    }
                            //    else
                            //    {
                            //        var applicationTypeDetails = FindServiceCodeMappingWithApplicationType(serviceGatewayRequestJsonData.ServiceCode);
                            //        if(applicationTypeDetails.ApplicationPurposeType== ApplicationPurposeTypeEnum.AMENDMENT_LICENCE || applicationTypeDetails.ApplicationPurposeType == ApplicationPurposeTypeEnum.RENEWAL_LICENCE) // CHK-7
                            //        {
                            //            // verify Old License. ************************** PENDING **************************

                            //           var Sys_O_AppId_TokenInfo = await _iSystem_O_CommunicationService.DoesOldLicenceNoExists(serviceGatewayRequestJsonData.FileNo);
                            //            if (Sys_O_AppId_TokenInfo.ResponseDataModel != null)
                            //            {
                            //                var processImdResp = await ImportAndSeedData(Sys_O_AppId_TokenInfo.ResponseDataModel.AppId, serviceGatewayRequestJsonData.iPin.ToString(), requestLog.AppId, serviceGatewayRequestJsonData.ServiceCode, Sys_O_AppId_TokenInfo.ResponseDataModel.TokenNumber, serviceGatewayRequestJsonData.FileNo);
                            //                if (processImdResp.ResponseDataModel != null)
                            //                {
                            //                    requestLog.NativeAppId = processImdResp.ResponseDataModel.AppRefId;
                            //                    genericServiceResultTemplate.ResponseDataModel.NativeAppId = processImdResp.ResponseDataModel.AppRefId;
                            //                    serviceGatewayResponse = await OpenServiceApplicationForm(genericServiceResultTemplate, requestLog, serviceGatewayRequestJsonData, false, "", true, true);
                            //                }
                            //                else
                            //                {
                            //                    serviceGatewayResponse.CanApply = false;
                            //                    serviceGatewayResponse.RequestDeniedReason = "Issue in opening form (CHK-7)..!";
                            //                }
                            //            }
                            //            else
                            //            {
                            //                serviceGatewayResponse.CanApply = false;
                            //                serviceGatewayResponse.RequestDeniedReason = "Licence no does not exixt..!";
                            //            }
                            //        }
                            //        else // CHK-7 -- Create new user and open application form
                            //        {
                            //            var createUserServiceResp = await CreateNewUser(serviceGatewayRequestJsonData.iPin, null);
                            //            if (createUserServiceResp.HasError)
                            //            {
                            //                serviceGatewayResponse.CanApply = false;
                            //                serviceGatewayResponse.RequestDeniedReason = "Issue in applying service. "+ createUserServiceResp.ErrorDesc + " (CHK-7)";
                            //            }
                            //            else
                            //            {
                            //                genericServiceResultTemplate.ResponseDataModel.NativeUserId = createUserServiceResp.ResponseDataModel.UserId;
                            //                serviceGatewayResponse = await OpenServiceApplicationForm(genericServiceResultTemplate, requestLog, serviceGatewayRequestJsonData, false, "", false, true);
                            //            }
                            //        }
                            //    }
                            //}
                            //else
                            //{
                            //    serviceGatewayResponse.CanApply = false;
                            //    serviceGatewayResponse.RequestDeniedReason = "Issue in applying service (CHK-5)";
                            //}
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                serviceGatewayResponse.HasException = true;
                serviceGatewayResponse.ExceptionMessage = ex.Message;
                throw ex;
            }
            return serviceGatewayResponse;
        }
        public async Task<GenericResponseTemplateModel<BusinessFirstRequestLogResponseViewModel>> LogBusinessFirstRequest(BusinessFirst_RequestLog requestLog)
        {
            GenericResponseTemplateModel<BusinessFirstRequestLogResponseViewModel> genericServiceResultTemplate = new GenericResponseTemplateModel<BusinessFirstRequestLogResponseViewModel>();
            try
            {
                //&& x.AppId == requestLog.AppId  && x.CategoryTypeId == requestLog.CategoryTypeId
                genericServiceResultTemplate.HasError = false;
                //var parentWithChildObjects = await _iGR_BusinessFirst_RequestLog.GetAsync(x => x.IPin == requestLog.IPin && x.ServiceCode == requestLog.ServiceCode).ConfigureAwait(false);
                var parentWithChildObjects = await _iGR_BusinessFirst_RequestLog.GetAsync(x => x.IPin == requestLog.IPin).ConfigureAwait(false);
                if (parentWithChildObjects.Count() > 0) //Existing record
                {
                    var nativeUserId = parentWithChildObjects.Where(x => x.NativeUserId != null).Select(x => x.NativeUserId).FirstOrDefault();
                    var nativeAppId = parentWithChildObjects.Where(x => x.ServiceCode == requestLog.ServiceCode && x.NativeAppId != 0 && x.AppId == requestLog.AppId).Select(x => x.NativeAppId).FirstOrDefault();
                    requestLog.NativeUserId = nativeUserId;
                    requestLog.NativeAppId = nativeAppId;

                    // Handling Reject Case
                    if (nativeAppId != 0)
                    {
                        requestLog.NativeUserId = nativeUserId;
                        var statusResp = await _iApplicationMamnagementService.GetCurrentStatusByAppRefId(nativeAppId, (ApplicationTypeEnum) requestLog.ServiceCode);

                        if (statusResp.ResponseDataModel.AppActionType != (int)AppActionTypeEnum.APP_REJECT)
                        {
                            var logs = parentWithChildObjects.Where(x => x.AppId == requestLog.AppId).Select(x => { x.LastModifiedDate = DateTime.Now; x.IsEnabled = false; x.NativeUserId = nativeUserId; x.NativeAppId = nativeAppId; return x; }).ToList();
                            await _context.BulkUpdateAsync<BusinessFirst_RequestLog>(logs);

                            requestLog.RequestCount = parentWithChildObjects.Count() + 1;
                            requestLog.NativeUserId = nativeUserId;
                            requestLog.NativeAppId = nativeAppId;
                        }

                        else if (statusResp.ResponseDataModel.AppActionType != (int)AppActionTypeEnum.APPLICATION_DORMANT)
                        {
                            var logs = parentWithChildObjects.Where(x => x.AppId == requestLog.AppId).Select(x => { x.LastModifiedDate = DateTime.Now; x.IsEnabled = false; x.NativeUserId = nativeUserId; x.NativeAppId = nativeAppId; return x; }).ToList();
                            await _context.BulkUpdateAsync<BusinessFirst_RequestLog>(logs);

                            requestLog.RequestCount = parentWithChildObjects.Count() + 1;
                            requestLog.NativeUserId = nativeUserId;
                            requestLog.NativeAppId = nativeAppId;
                        }
                    }
                }

                _iGR_BusinessFirst_RequestLog.Insert(requestLog);
                await _iGR_BusinessFirst_RequestLog.SavechangeAsync();

                genericServiceResultTemplate.ResponseDataModel = new BusinessFirstRequestLogResponseViewModel();
                genericServiceResultTemplate.ResponseDataModel.BusinessFirst_RequestLogId = requestLog.BusinessFirst_RequestLogId;
                //if(requestLog.NativeAppId > 0)
                //{
                //    genericServiceResultTemplate.ResponseDataModel.NativeAppId = requestLog.NativeAppId;
                //}
                //else
                //{
                //    genericServiceResultTemplate.ResponseDataModel.NativeAppId = 300179;
                //}
                genericServiceResultTemplate.ResponseDataModel.NativeAppId = requestLog.NativeAppId;
                genericServiceResultTemplate.ResponseDataModel.NativeUserId = requestLog.NativeUserId;
            }
            catch (Exception ex)
            {
                genericServiceResultTemplate.HasError = true;
                genericServiceResultTemplate.ErrorDesc = ex.Message;
                throw ex;
            }
            return genericServiceResultTemplate;
        }

        //public async Task<GenericResponseTemplateModel<BusinessFirstSCAFViewModel>> GetBusinessFirstCafDataByIPin(Int64 iPin)
        //{
        //    GenericResponseTemplateModel<BusinessFirstSCAFViewModel> genericResponseTemplateModel = new GenericResponseTemplateModel<BusinessFirstSCAFViewModel>() { HasError = false, ErrorDesc = "" };
        //    try
        //    {
        //        string url = Configuration.GetSection("ThirdPartyIntegrationConfigs").GetSection("InvestPunjab").GetSection("GetCafDataUrl").Value;
        //        using (var httpClient = new HttpClient())
        //        {
        //            StringContent content = new StringContent(JsonConvert.SerializeObject(new { iPin = iPin }), Encoding.UTF8, "application/json");

        //            using (var response = await httpClient.PostAsync(url, content))
        //            {
        //                string apiResponse = await response.Content.ReadAsStringAsync();
        //                genericResponseTemplateModel.ResponseDataModel = JsonConvert.DeserializeObject<BusinessFirstSCAFViewModel>(apiResponse);
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        genericResponseTemplateModel.HasError = true;
        //        genericResponseTemplateModel.ErrorDesc = ex.Message;
        //        throw ex;
        //    }

        //    return genericResponseTemplateModel;
        //}

        public async Task<GenericResponseTemplateModel<BusinessFirstSCAFViewModel>> GetBusinessFirstCafDataByIPin(Int64 iPin)
        {
            GenericResponseTemplateModel<BusinessFirstSCAFViewModel> genericResponseTemplateModel = new GenericResponseTemplateModel<BusinessFirstSCAFViewModel>() { HasError = false, ErrorDesc = "" };
            try
            {
                string tokenUrl = Configuration.GetSection("ThirdPartyIntegrationConfigs").GetSection("InvestPunjab").GetSection("GetBusinessFirstTokenUrl").Value;
                string integrationKey = Configuration.GetSection("ThirdPartyIntegrationConfigs").GetSection("InvestPunjab").GetSection("InvestPunjabApiTokenIntegrationKey").Value;
                InvestPunjabApiTokenViewModel investPunjabApiToken = null;
                using (var httpClient = new HttpClient())
                {
                    var tokenRequestData = new
                    {
                        IntegrationKey = integrationKey
                    };
                    StringContent content = new StringContent(JsonConvert.SerializeObject(tokenRequestData), Encoding.UTF8, "application/json");

                    using (var response = await httpClient.PostAsync(tokenUrl, content))
                    {
                        string apiResponse = await response.Content.ReadAsStringAsync();
                        investPunjabApiToken = JsonConvert.DeserializeObject<InvestPunjabApiTokenViewModel>(apiResponse);
                    }
                }
                if (investPunjabApiToken != null && investPunjabApiToken.Token != null)
                {
                    string url = Configuration.GetSection("ThirdPartyIntegrationConfigs").GetSection("InvestPunjab").GetSection("GetCafDataUrl").Value;
                    using (var httpClient = new HttpClient())
                    {
                        httpClient.DefaultRequestHeaders.Add("authorization", investPunjabApiToken.Token);
                        httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                        StringContent content = new StringContent(JsonConvert.SerializeObject(new { iPin = iPin }), Encoding.UTF8, "application/json");

                        using (var response = await httpClient.PostAsync(url, content))
                        {
                            string apiResponse = await response.Content.ReadAsStringAsync();
                            genericResponseTemplateModel.ResponseDataModel = JsonConvert.DeserializeObject<BusinessFirstSCAFViewModel>(apiResponse);
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                genericResponseTemplateModel.HasError = true;
                genericResponseTemplateModel.ErrorDesc = ex.Message;
            }

            return genericResponseTemplateModel;
        }

        public async Task<GenericResponseTemplateModel<bool>> UpdateBusinessFirstRequestNativeUserNativeByIPin(Int64 iPin, string nativeUserId)
        {
            GenericResponseTemplateModel<bool> genericServiceResultTemplate = new GenericResponseTemplateModel<bool>()
            {
                HasError = false,
                ErrorDesc = "",
                ResponseDataModel = true
            };
            try
            {
                genericServiceResultTemplate.HasError = false;
                var parentWithChildObjects = await _iGR_BusinessFirst_RequestLog.GetAsync(x => x.IPin == iPin).ConfigureAwait(false);
                if (parentWithChildObjects.Count() > 0) //Existing record
                {
                    var logs = parentWithChildObjects.Select(x => { x.LastModifiedDate = DateTime.Now; x.NativeUserId = nativeUserId; return x; }).ToList();
                    await _context.BulkUpdateAsync<BusinessFirst_RequestLog>(logs);
                }
            }
            catch (Exception ex)
            {
                genericServiceResultTemplate.HasError = true;
                genericServiceResultTemplate.ErrorDesc = ex.Message;
                throw ex;
            }
            return genericServiceResultTemplate;
        }

        public async Task<GenericResponseTemplateModel<bool>> UpdateBusinessFirstRequestNativeAppIdByIPin(Int64 iPin, Int64 appId, Int64 nativeAppId, ApplicationTypeEnum applicationType, ApplicationPurposeTypeEnum applicationPurposeType)
        {
            GenericResponseTemplateModel<bool> genericServiceResultTemplate = new GenericResponseTemplateModel<bool>()
            {
                HasError = false,
                ErrorDesc = "",
                ResponseDataModel = true
            };
            try
            {
                var foundService = FindServiceCodeWithApplicationType_Purpose(applicationType, applicationPurposeType);


                genericServiceResultTemplate.HasError = false;
                var parentWithChildObjects = await _iGR_BusinessFirst_RequestLog.GetAsync(x => x.IPin == iPin && x.AppId == appId && x.ServiceCode == foundService.ServiceCode).ConfigureAwait(false);
                if (parentWithChildObjects.Count() > 0) //Existing record
                {
                    var logs = parentWithChildObjects.Select(x => { x.LastModifiedDate = DateTime.Now; x.NativeAppId = nativeAppId; return x; }).ToList();
                    await _context.BulkUpdateAsync<BusinessFirst_RequestLog>(logs);
                }
            }
            catch (Exception ex)
            {
                genericServiceResultTemplate.HasError = true;
                genericServiceResultTemplate.ErrorDesc = ex.Message;
                throw ex;
            }
            return genericServiceResultTemplate;
        }

        public async Task<GenericResponseTemplateModel<string>> ShareStatusToBusinessFirst(Int64 AppRefId, ApplicationTypeEnum applicationType, AppActionTypeEnum appActionType)
        {
            GenericResponseTemplateModel<string> genericServiceResultTemplate = new GenericResponseTemplateModel<string>()
            {
                HasError = false,
                ErrorDesc = "",
                ResponseDataModel = ""
            };
            try
            {
                //var iPinResp =  await GetIPinByAppRefId(AppRefId);
                //var countResp = await GetShareStatusLogCountByAppRefId(AppRefId);
                //var statusResp = await _iApplicationMamnagementService.GetCurrentStatusByAppRefId(AppRefId);
                //var senderProfileResp = _iAuthService.GetUserProfileByProfileId(statusResp.ResponseDataModel.Sender_ProfileRefId);
                //var receiverProfileResp = _iAuthService.GetUserProfileByProfileId(statusResp.ResponseDataModel.Receiver_ProfileRefId);
                //var senderRoleResp = _iAuthService.GetUserRoleByRoleId(statusResp.ResponseDataModel.SenderRoleId);
                //var receiverRoleResp = _iAuthService.GetUserRoleByRoleId(statusResp.ResponseDataModel.ReceiverRoleId);
                ////var statusDescResp = await _iApplicationMamnagementService.GetCurrentStatusCodeByAppActionType(statusResp.ResponseDataModel.AppActionType);

                //var licenceInfo = _context.ApplicationLicenceNoMapping.Where(x => x.AppRefId == AppRefId).FirstOrDefault();


                List<StoreProcedureParm> storeProcedureParms2 = new List<StoreProcedureParm>()
                {
                    new StoreProcedureParm (){ ParmName="AppRefId", ParmValue= AppRefId.ToString(), isNumber=true },
                    new StoreProcedureParm (){ ParmName="IsDirty", ParmValue= "1", isNumber=false }
                };
                var resp1 = await _iGeneric_SP_Repository.CallStoreProcedureReaderScalerAsync("dbo.sp_SetApplicationIsDirtyStatus", storeProcedureParms2);


                List<StoreProcedureParm> storeProcedureParms1 = new List<StoreProcedureParm>()
                {
                    new StoreProcedureParm (){ ParmName="appRefId", ParmValue=AppRefId.ToString(), isNumber=true}
                };
                var resp = await _iGeneric_SP_Repository.CallStoreProcedureReaderAsync<GetShareStatusRequestDetailsViewModel>("dbo.sp_GetShareStatusRequestDetails", storeProcedureParms1);

                if (resp != null)
                {
                    // Call Stored Procedure
                    List<StoreProcedureParm> storeProcedureParms = new List<StoreProcedureParm>()
                    {
                        new StoreProcedureParm (){ ParmName="appRefId", ParmValue=AppRefId.ToString(), isNumber=true}
                    };
                    var totalTakenTimeInfo = await _iGeneric_SP_Repository.CallStoreProcedureReaderAsync<GetTotalTakenTimeByDepartmentViewModel>("dbo.sp_GetTotalTakenTimeByDepartment", storeProcedureParms);

                    var clearanceIssuedOn = "NA";
                    var clearanceExpiredOn = "NA";
                    if (resp.FirstOrDefault().AppActionType == 200)
                    {
                        clearanceIssuedOn = resp.FirstOrDefault().ActionDate.ToString("yyyy-MM-dd HH:mm:ss") ?? "NA";
                        clearanceExpiredOn = resp.FirstOrDefault().ClearanceExpiredOn.ToString("yyyy-MM-dd HH:mm:ss") ?? "NA";
                    }
                    
                    var requestData = new
                    {
                        iPin = resp.FirstOrDefault().InvestPunjab_Ipin,
                        AppId = resp.FirstOrDefault().InvestPunjab_AppId,
                        statusId = resp.FirstOrDefault().AppActionType,
                        statusDesc = resp.FirstOrDefault().Remarks,
                        comments = resp.FirstOrDefault().Remarks,
                        senderName = resp.FirstOrDefault().SenderName,
                        senderDesignation = resp.FirstOrDefault().SenderRoleName,
                        receiverName = resp.FirstOrDefault().ReceiverName,
                        receiverDesignation = resp.FirstOrDefault().ReceiverRoleName,
                        clearanceIssuedOn = clearanceIssuedOn,
                        clearanceExpiredOn = clearanceExpiredOn,
                        //licenseNo = licenceInfo == null ? "NA" : licenceInfo.LicenceNumber.ToString(),
                        licenseNo = resp.FirstOrDefault().LicenceNumber,
                        //clearanceFile = resp.FirstOrDefault().AppActionType != 200 ? "NA" : resp.FirstOrDefault().ClearanceFile,
                        clearanceFile = (resp.FirstOrDefault().AppActionType == 200 || resp.FirstOrDefault().AppActionType == 206) ? resp.FirstOrDefault().ClearanceFile : "NA",
                        statusDate = resp.FirstOrDefault().ActionDate,
                        integrationSource = "LABOUR",
                        deemedApproval = "false",
                        departmentTakenTotalTime = totalTakenTimeInfo.FirstOrDefault().TotalTime,
                        timeType = totalTakenTimeInfo.FirstOrDefault().TimeType,
                        appActionLogId = resp.FirstOrDefault().AppActionLogId,
                    };

                    BusinessFirstShareStatusLog businessFirstShareStatusLog = new BusinessFirstShareStatusLog()
                    {
                        ApplicationType = applicationType,
                        AppActionType = (AppActionTypeEnum)resp.FirstOrDefault().AppActionType,
                        ApiURL = Configuration.GetSection("ThirdPartyIntegrationConfigs").GetSection("InvestPunjab").GetSection("ShareStatusApiUrl").Value,
                        StatusSentOn = DateTime.Now,
                        RequestJSON = JsonConvert.SerializeObject(requestData),
                        IsRequestCompeleted = false,
                        RequestCompletionOn = null,
                        ResponseJson = "",
                        TriedCount = resp.FirstOrDefault().TriedCount + 1,
                        AppRefId = AppRefId,
                        SyncStatusProcessEngineRefId = null
                    };

                    _context.BusinessFirstShareStatusLogs.Add(businessFirstShareStatusLog);
                    _context.SaveChanges();

                    //_iGR_BusinessFirstShareStatusLog.Insert(businessFirstShareStatusLog);
                    //await _iGR_BusinessFirstShareStatusLog.SavechangeAsync();

                    // Post Request data to Invest punjab
                    string tokenUrl = Configuration.GetSection("ThirdPartyIntegrationConfigs").GetSection("InvestPunjab").GetSection("GetBusinessFirstTokenUrl").Value;
                    string integrationKey = Configuration.GetSection("ThirdPartyIntegrationConfigs").GetSection("InvestPunjab").GetSection("InvestPunjabApiTokenIntegrationKey").Value;
                    InvestPunjabApiTokenViewModel investPunjabApiToken = null;
                    using (var httpClient = new HttpClient())
                    {
                        var tokenRequestData = new
                        {
                            IntegrationKey = integrationKey
                        };

                        StringContent content = new StringContent(JsonConvert.SerializeObject(tokenRequestData), Encoding.UTF8, "application/json");

                        using (var response = await httpClient.PostAsync(tokenUrl, content))
                        {
                            string apiResponse = await response.Content.ReadAsStringAsync();
                            investPunjabApiToken = JsonConvert.DeserializeObject<InvestPunjabApiTokenViewModel>(apiResponse);
                        }
                    }

                    if (investPunjabApiToken != null && investPunjabApiToken.Token != null)
                    {
                        string apiUrl = Configuration.GetSection("ThirdPartyIntegrationConfigs").GetSection("InvestPunjab").GetSection("ShareStatusApiUrl").Value;
                        using (var httpClient = new HttpClient())
                        {
                            httpClient.DefaultRequestHeaders.Add("authorization", investPunjabApiToken.Token);
                            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                            StringContent content = new StringContent(JsonConvert.SerializeObject(requestData), Encoding.UTF8, "application/json");

                            using (var response = await httpClient.PostAsync(apiUrl, content))
                            {
                                businessFirstShareStatusLog.RequestCompletionOn = DateTime.Now;
                                businessFirstShareStatusLog.IsRequestCompeleted = true;
                                businessFirstShareStatusLog.ResponseJson = await response.Content.ReadAsStringAsync();
                                _iGR_BusinessFirstShareStatusLog.Update(businessFirstShareStatusLog);
                                await _iGR_BusinessFirstShareStatusLog.SavechangeAsync();
                                genericServiceResultTemplate.ResponseDataModel = businessFirstShareStatusLog.ResponseJson;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                genericServiceResultTemplate.HasError = true;
                genericServiceResultTemplate.ErrorDesc = ex.Message;
                genericServiceResultTemplate.ResponseDataModel = ex.Message;
            }
            return genericServiceResultTemplate;
        }
        public async Task<GenericResponseTemplateModel<Application>> GetIPinByAppRefId(Int64 appRefId)
        {
            GenericResponseTemplateModel<Application> genericServiceResultTemplate = new GenericResponseTemplateModel<Application>()
            {
                HasError = false,
                ErrorDesc = "",
            };
            try
            {
                genericServiceResultTemplate.ResponseDataModel = _context.Applications.Where(x => x.AppId == appRefId && x.IsDeleted == false).FirstOrDefault();
            }
            catch (Exception ex)
            {
                genericServiceResultTemplate.HasError = true;
                genericServiceResultTemplate.ErrorDesc = ex.Message;
                throw ex;
            }
            return genericServiceResultTemplate;
        }

        public async Task<GenericResponseTemplateModel<Int32>> GetShareStatusLogCountByAppRefId(Int64 appRefId)
        {
            GenericResponseTemplateModel<Int32> genericServiceResultTemplate = new GenericResponseTemplateModel<Int32>()
            {
                HasError = false,
                ErrorDesc = "",
            };
            try
            {
                genericServiceResultTemplate.ResponseDataModel = _context.BusinessFirstShareStatusLogs.Count(x => x.AppRefId == appRefId);
            }
            catch (Exception ex)
            {
                genericServiceResultTemplate.HasError = true;
                genericServiceResultTemplate.ErrorDesc = ex.Message;
                throw ex;
            }
            return genericServiceResultTemplate;
        }

        public int GetMappedBusinessFirstActionCode(int actionCode)
        {
            if (actionCode == 1) //   Application Saved as draft
            {
                return 1;
            }
            else if (actionCode == 2) //   Application Saved As Lock And Fees Pending
            {
                return 2;
            }
            else if (actionCode == 3) //  Application Submitted And Fee Paid Online
            {
                return 3;
            }
            else if (actionCode == 6) //  Objection Resolved & Application Re - Submitted
            {
                return 10;
            }
            else if (actionCode == 200) // Approved
            {
                return 5;
            }
            else if (actionCode == 201) // Rejected
            {
                return 15;
            }
            else if (actionCode == 202) // De-Registered
            {
                return 16;
            }
            else if (actionCode == 103 || actionCode == 100 || actionCode == 102) // Forwarded for further action // Marked Back // Forwarded With Objections
            {
                return 8;
            }
            else if (actionCode == 404) // Objection Raised And Sent To Applicant
            {
                return 9;
            }
            else if (actionCode == 101 || actionCode == 104) // Forwarded for Approval // Recommendation for Approval
            {
                return 12;
            }
            else if (actionCode == 400) // Raise Fees
            {
                return 400;
            }
            else if (actionCode == 8) //  Raised Fees Paid(BuildinPlan - HUD)
            {
                return 7;
            }
            return 0;

            //else if (actionCode == 5) //   Application Submitted(Application Fee Not Applicable)
            //{
            //    return 1;
            //}

            //else if (actionCode == 7) //   Application locked but digital signature is pending
            //{
            //    return 1;
            //}
        }

        public async Task<GenericResponseTemplateModel<ImportDataViewModel>> ImportDataFromELabour(Int64 iPin, ApplicationTypeEnum applicationType)
        {
            GenericResponseTemplateModel<ImportDataViewModel> genericResponseTemplateModel = new GenericResponseTemplateModel<ImportDataViewModel>() { HasError = false, ErrorDesc = "" };
            try
            {
                string url = Configuration.GetSection("ThirdPartyIntegrationConfigs").GetSection("PbLabour").GetSection("ImportDataUrl").Value;
                using (var httpClient = new HttpClient())
                {
                    var requestData = new
                    {
                        iPin = iPin,
                        applicationType = applicationType
                    };

                    StringContent content = new StringContent(JsonConvert.SerializeObject(requestData), Encoding.UTF8, "application/json");

                    using (var response = await httpClient.PostAsync(url, content))
                    {
                        string apiResponse = await response.Content.ReadAsStringAsync();
                        genericResponseTemplateModel.ResponseDataModel = JsonConvert.DeserializeObject<ImportDataViewModel>(apiResponse);
                    }
                }
            }
            catch (Exception ex)
            {
                genericResponseTemplateModel.HasError = true;
                genericResponseTemplateModel.ErrorDesc = ex.Message;
                throw ex;
            }
            return genericResponseTemplateModel;
        }

        public async Task<GenericResponseTemplateModel<bool>> IsNativeAppIdApplicationIsAlreadyInprocess(Int64 iPin, ApplicationTypeEnum applicationType, ApplicationPurposeTypeEnum applicationPurposeType)
        {
            GenericResponseTemplateModel<bool> genericServiceResultTemplate = new GenericResponseTemplateModel<bool>()
            {
                HasError = false,
                ErrorDesc = "",
                ResponseDataModel = false
            };
            try
            {
                var nativeAppId = _context.BusinessFirst_RequestLogs.Where(x => x.IPin == iPin && x.NativeAppId != 0).Select(x => x.NativeAppId).ToArray();
                if (nativeAppId.Length == 0)
                {
                    genericServiceResultTemplate.ResponseDataModel = false;
                }
                else
                {
                    //genericServiceResultTemplate.ResponseDataModel = _context.Applications.Any(x => nativeAppId.Contains(x.AppId) && x.ApplicationType == applicationType && x.ApplicationPurposeType == applicationPurposeType && (x.ApplicationLifeCycleStatusType == ApplicationLifeCycleStatusTypeEnum.IN_PROCESS || x.ApplicationLifeCycleStatusType == ApplicationLifeCycleStatusTypeEnum.IN_OBJECTION || x.ApplicationLifeCycleStatusType == ApplicationLifeCycleStatusTypeEnum.APPROVED));

                    if (applicationPurposeType == ApplicationPurposeTypeEnum.GRANT_LICENCE)
                    {
                        genericServiceResultTemplate.ResponseDataModel = _context.Applications.Any(x => nativeAppId.Contains(x.AppId) && x.IsDeleted == false && x.ApplicationType == applicationType && x.ApplicationPurposeType == applicationPurposeType); //&& !x.IsAllowEdit
                    }
                    else
                    {
                        genericServiceResultTemplate.ResponseDataModel = _context.Applications.Any(x => nativeAppId.Contains(x.AppId) 
                        && x.IsDeleted == false
                        && x.ApplicationType == applicationType
                        && x.ApplicationPurposeType == applicationPurposeType
                        && (x.ApplicationLifeCycleStatusType == ApplicationLifeCycleStatusTypeEnum.NOT_SUBMITTED || x.ApplicationLifeCycleStatusType == ApplicationLifeCycleStatusTypeEnum.IN_PROCESS || x.ApplicationLifeCycleStatusType == ApplicationLifeCycleStatusTypeEnum.IN_OBJECTION)); //&& !x.IsAllowEdit
                    }
                }
            }
            catch (Exception ex)
            {
                genericServiceResultTemplate.HasError = true;
                genericServiceResultTemplate.ErrorDesc = ex.Message;
                throw ex;
            }
            return genericServiceResultTemplate;
        }

        public async Task<ServiceGatewayResponseViewModel> OpenServiceApplicationForm(
            GenericResponseTemplateModel<BusinessFirstRequestLogResponseViewModel> genericServiceResultTemplate,
            BusinessFirst_RequestLog requestLog,
            ServiceGatewayRequestJsonDataViewModel serviceGatewayRequestJsonData,
            bool isOtpVerificationRequired,
            string contactSnapshot,
            bool isProjectProfileUpdate,
            bool isAppRefIdToBeSet, HttpRequest httpRequest)
        {
            ServiceGatewayResponseViewModel serviceGatewayResponse = new ServiceGatewayResponseViewModel() { HasException = false };
            LoggedUserInfoViewModel loggedUserInfoViewModel = await _iAuthService.GetUserLoginDeatilByUserId(requestLog.UserId);
            if (loggedUserInfoViewModel != null)
            {
                Int64 projectSiteId = 0;
                int projectSiteVersion = 0;
                ApplicationLifeCycleStatusTypeEnum applicationlifecycle = ApplicationLifeCycleStatusTypeEnum.NOT_SUBMITTED;
                var projectSiteIdResp = await _iProjectSiteService.GetProjectSiteRefIdByIpin(requestLog.IPin.ToString());
                projectSiteId = projectSiteIdResp.ResponseDataModel;
                if (projectSiteId > 0 && isProjectProfileUpdate)
                {
                    try
                    {
                        ProjectSite projectSite = _context.ProjectSites.Where(x => x.ProjectSiteId == projectSiteId).FirstOrDefault();
                        //bool isProjectSiteLogToBeUpdate = false;
                        bool isProjectSiteVersionToBeUpdatd = false;

                        GenericResponseTemplateModel<BusinessFirstSCAFViewModel> genericResponseTemplateModel = await GetBusinessFirstCafDataByIPin(requestLog.IPin);
                        if (genericResponseTemplateModel.ResponseDataModel.CafInfo.SitePin == null)
                        {
                            genericResponseTemplateModel.ResponseDataModel.CafInfo.SitePin = "000000";
                        }

                        //address
                        if (!string.IsNullOrWhiteSpace(genericResponseTemplateModel.ResponseDataModel?.CafInfo?.SiteAddress))
                        {
                            if (projectSite.Address.Trim().ToLower() != genericResponseTemplateModel.ResponseDataModel.CafInfo.SiteAddress.Trim().ToLower() ||
                                projectSite.ProjectPurpose.Trim().ToLower() != genericResponseTemplateModel.ResponseDataModel.CafInfo.ProjectPurpose.Trim().ToLower() ||
                                projectSite.EstablishmentName.Trim().ToLower() != genericResponseTemplateModel.ResponseDataModel.CafInfo.EstbName.Trim().ToLower() ||
                                projectSite.ContactPersonFirstName.Trim().ToLower() != genericResponseTemplateModel.ResponseDataModel.CafInfo.First_Name.Trim().ToLower())
                            {
                                isProjectSiteVersionToBeUpdatd = true;
                            }
                        }



                        var application = await _context.Applications.Where(x => x.ProjectSiteRefId == projectSite.ProjectSiteId && x.IsDeleted == false && x.ProjectSiteVersion == projectSite.ProjectSiteVersion).OrderByDescending(x => x.AppId).FirstOrDefaultAsync();
                        if (application != null)
                        {
                            if (application.InvestPunjab_Ipin == requestLog.IPin.ToString())
                            {
                                projectSiteVersion = projectSite.ProjectSiteVersion;
                            }
                            else
                            {
                                projectSiteVersion = projectSite.ProjectSiteVersion + 1;
                                isProjectSiteVersionToBeUpdatd = true;
                            }
                        }
                        else
                        {
                            projectSiteVersion = projectSite.ProjectSiteVersion;
                        }


                        if (isProjectSiteVersionToBeUpdatd)
                        {
                          
                            projectSite.EstablishmentName = genericResponseTemplateModel.ResponseDataModel.CafInfo.EstbName;
                            projectSite.Address = genericResponseTemplateModel.ResponseDataModel.CafInfo.SiteAddress == null ? "" : genericResponseTemplateModel.ResponseDataModel.CafInfo.SiteAddress;
                            projectSite.VillageOrTown = genericResponseTemplateModel.ResponseDataModel.CafInfo.SiteVlgName == null ? "" : genericResponseTemplateModel.ResponseDataModel.CafInfo.SiteVlgName;
                            projectSite.TehsilRefId = Convert.ToInt64(genericResponseTemplateModel.ResponseDataModel.CafInfo.SiteLGDTehId);
                            projectSite.DistrictRefId = Convert.ToInt64(genericResponseTemplateModel.ResponseDataModel.CafInfo.SiteLGDDistId);
                            projectSite.PinCode = genericResponseTemplateModel.ResponseDataModel.CafInfo.SitePin == null ? "000000" : genericResponseTemplateModel.ResponseDataModel.CafInfo.SitePin;
                            projectSite.LastModifiedDate = DateTime.Now;
                            projectSite.UserRefId = loggedUserInfoViewModel.UserId;
                            projectSite.ApplicantAadharNumber = genericResponseTemplateModel.ResponseDataModel.CafInfo.AadharNum;
                            projectSite.ApplicantAadharAttachment = genericResponseTemplateModel.ResponseDataModel.CafInfo.SC_Aadhar;
                            projectSite.ApplicantPanNumber = genericResponseTemplateModel.ResponseDataModel.CafInfo.ApplicantPan;
                            projectSite.ApplicantPanAttachment = genericResponseTemplateModel.ResponseDataModel.CafInfo.ApplicantPanAttachment;
                            projectSite.CompanyPanNumber = genericResponseTemplateModel.ResponseDataModel.CafInfo.CompanyPan;
                            projectSite.CompanyPanAttachment = genericResponseTemplateModel.ResponseDataModel.CafInfo.CompanyPanAttachment;
                            projectSite.ProjectPurpose = genericResponseTemplateModel.ResponseDataModel.CafInfo.ProjectPurpose;
                            projectSite.ContactPersonFirstName = genericResponseTemplateModel.ResponseDataModel.CafInfo.First_Name;
                            projectSite.ContactPersonMiddleName = genericResponseTemplateModel.ResponseDataModel.CafInfo.Middle_Name == null ? "" : genericResponseTemplateModel.ResponseDataModel.CafInfo.Middle_Name;
                            projectSite.ContactPersonLastName = genericResponseTemplateModel.ResponseDataModel.CafInfo.Last_Name == null ? "." : genericResponseTemplateModel.ResponseDataModel.CafInfo.Last_Name;
                            projectSite.ContactPersonEmail = genericResponseTemplateModel.ResponseDataModel.CafInfo.Email;
                            projectSite.ContactPersonMobileNo = genericResponseTemplateModel.ResponseDataModel.CafInfo.MobileNo.Substring(genericResponseTemplateModel.ResponseDataModel.CafInfo.MobileNo.Length - 10);
                            projectSite.AlternateMobileNo = "NA";
                            projectSite.AlternateEmail = "NA";
                            projectSite.ProjectSiteId = projectSiteId;
                            var userRefId = projectSite.UserRefId;
                            projectSite.UserRefId = userRefId;
                            projectSite.ProjectSiteVersion = projectSiteVersion + 1;
                            _context.ProjectSites.Update(projectSite);
                            _context.SaveChanges();


                            var projectSiteLog = new ProjectSiteLog();
                            projectSiteLog.ProjectSiteRefId = projectSite.ProjectSiteId;
                            projectSiteLog.EstablishmentName = projectSite.EstablishmentName;
                            projectSiteLog.Address = projectSite.Address;
                            projectSiteLog.VillageOrTown = projectSite.VillageOrTown;
                            projectSiteLog.TehsilRefId = Convert.ToInt64(projectSite.TehsilRefId);
                            projectSiteLog.DistrictRefId = Convert.ToInt64(projectSite.DistrictRefId);
                            projectSiteLog.PinCode = projectSite.PinCode;
                            projectSiteLog.IsActive = true;
                            projectSiteLog.IsDeleted = false;
                            projectSiteLog.Createddate = DateTime.Now;
                            projectSiteLog.LastModifiedDate = DateTime.Now;
                            projectSiteLog.UserRefId = projectSite.UserRefId;
                            projectSiteLog.LabourCircleRefId = projectSite.LabourCircleRefId;
                            projectSiteLog.FactoryCircleRefId = projectSite.FactoryCircleRefId;
                            projectSiteLog.ApplicantAadharNumber = projectSite.ApplicantAadharNumber;
                            projectSiteLog.ApplicantAadharAttachment = projectSite.ApplicantAadharAttachment;
                            projectSiteLog.ApplicantPanNumber = projectSite.ApplicantPanNumber;
                            projectSiteLog.ApplicantPanAttachment = projectSite.ApplicantPanAttachment;
                            projectSiteLog.CompanyPanNumber = projectSite.CompanyPanNumber;
                            projectSiteLog.CompanyPanAttachment = projectSite.CompanyPanAttachment;
                            projectSiteLog.ProjectPurpose = projectSite.ProjectPurpose;
                            projectSiteLog.ContactPersonFirstName = projectSite.ContactPersonFirstName;
                            projectSiteLog.ContactPersonMiddleName = projectSite.ContactPersonMiddleName;
                            projectSiteLog.ContactPersonLastName = projectSite.ContactPersonLastName == null ? "." : projectSite.ContactPersonLastName;
                            projectSiteLog.ContactPersonEmail = projectSite.ContactPersonEmail;
                            projectSiteLog.ContactPersonMobileNo = projectSite.ContactPersonMobileNo;
                            projectSiteLog.AlternateMobileNo = "NA";
                            projectSiteLog.AlternateEmail = "NA";
                            projectSiteLog.ProjectSiteVersion = projectSite.ProjectSiteVersion;
                            await _context.ProjectSiteLogs.AddAsync(projectSiteLog);
                            _context.SaveChanges();
                        }
                        projectSiteVersion = projectSite.ProjectSiteVersion;

                      Application app =  _context.Applications.Where(x => x.InvestPunjab_Ipin == requestLog.IPin.ToString() && x.InvestPunjab_AppId == requestLog.AppId && x.IsDeleted == false).FirstOrDefault();
                      if(app != null)
                      {
                        app.ProjectSiteVersion = projectSiteVersion;
                        _context.Applications.Update(app);
                        _context.SaveChanges();
                       }
                      
                    }
                    catch (Exception dd)
                    {
                        throw dd;
                    }
                }
                else if (projectSiteId == 0)
                {
                    GenericResponseTemplateModel<BusinessFirstSCAFViewModel> genericResponseTemplateModel = await GetBusinessFirstCafDataByIPin(requestLog.IPin);

                    var factoryCircleId = 22;
                    var labourCircleId = 10001;

                    var comLGDDistId = Convert.ToInt64(genericResponseTemplateModel.ResponseDataModel.CafInfo.ComLGDDistId);
                    var factoryCircle = _context.FactoryCircles.Where(x => x.DistrictLgdRefId == (comLGDDistId == 737 ? 43 : comLGDDistId) && x.Version == 2).FirstOrDefault();

                    //737 - Malerkotla ---> 43 Sangrur

                    ProjectSite projectSite = new ProjectSite()
                    {
                        EstablishmentName = genericResponseTemplateModel.ResponseDataModel.CafInfo.EstbName,
                        Address = genericResponseTemplateModel.ResponseDataModel.CafInfo.SiteAddress == null ? "" : genericResponseTemplateModel.ResponseDataModel.CafInfo.SiteAddress,
                        VillageOrTown = genericResponseTemplateModel.ResponseDataModel.CafInfo.SiteVlgName == null ? "NA" : genericResponseTemplateModel.ResponseDataModel.CafInfo.SiteVlgName,
                        TehsilRefId = Convert.ToInt64(genericResponseTemplateModel.ResponseDataModel.CafInfo.SiteLGDTehId),
                        DistrictRefId = Convert.ToInt64(genericResponseTemplateModel.ResponseDataModel.CafInfo.SiteLGDDistId),
                        PinCode = genericResponseTemplateModel.ResponseDataModel.CafInfo.SitePin == null ? "000000" : genericResponseTemplateModel.ResponseDataModel.CafInfo.SitePin,
                        IsActive = true,
                        IsDeleted = false,
                        Createddate = DateTime.Now,
                        LastModifiedDate = DateTime.Now,
                        UserRefId = loggedUserInfoViewModel.UserId,
                        LabourCircleRefId = labourCircleId,
                        FactoryCircleRefId = factoryCircle.FactoryCircleId,
                        ApplicantAadharNumber = genericResponseTemplateModel.ResponseDataModel.CafInfo.AadharNum,
                        ApplicantAadharAttachment = genericResponseTemplateModel.ResponseDataModel.CafInfo.SC_Aadhar,
                        ApplicantPanNumber = genericResponseTemplateModel.ResponseDataModel.CafInfo.ApplicantPan,
                        ApplicantPanAttachment = genericResponseTemplateModel.ResponseDataModel.CafInfo.ApplicantPanAttachment,
                        CompanyPanNumber = genericResponseTemplateModel.ResponseDataModel.CafInfo.CompanyPan,
                        CompanyPanAttachment = genericResponseTemplateModel.ResponseDataModel.CafInfo.CompanyPanAttachment,
                        ProjectPurpose = genericResponseTemplateModel.ResponseDataModel.CafInfo.ProjectPurpose,
                        ContactPersonFirstName = genericResponseTemplateModel.ResponseDataModel.CafInfo.First_Name,
                        ContactPersonMiddleName = genericResponseTemplateModel.ResponseDataModel.CafInfo.Middle_Name == null ? "" : genericResponseTemplateModel.ResponseDataModel.CafInfo.Middle_Name,
                        ContactPersonLastName = genericResponseTemplateModel.ResponseDataModel.CafInfo.Last_Name == null ? "." : genericResponseTemplateModel.ResponseDataModel.CafInfo.Last_Name,
                        ContactPersonEmail = genericResponseTemplateModel.ResponseDataModel.CafInfo.Email,
                        ContactPersonMobileNo = genericResponseTemplateModel.ResponseDataModel.CafInfo.MobileNo.Substring(genericResponseTemplateModel.ResponseDataModel.CafInfo.MobileNo.Length - 10),
                        AlternateMobileNo = genericResponseTemplateModel.ResponseDataModel.CafInfo.MobileNo.Substring(genericResponseTemplateModel.ResponseDataModel.CafInfo.MobileNo.Length - 10),
                        AlternateEmail = genericResponseTemplateModel.ResponseDataModel.CafInfo.AltEmail == "" ? genericResponseTemplateModel.ResponseDataModel.CafInfo.Email : genericResponseTemplateModel.ResponseDataModel.CafInfo.AltEmail,
                        ProjectSiteVersion = projectSiteVersion == 0 ? 1 : projectSiteVersion
                    };
                    var projectSiteDetails = await _iProjectSiteService.CreateProjectSite(projectSite, true, requestLog.IPin.ToString());
                    projectSiteId = projectSiteDetails.ResponseDataModel.ProjectSiteId;
                    projectSiteVersion = projectSiteDetails.ResponseDataModel.ProjectSiteVersion;
                }

                if(serviceGatewayRequestJsonData.CategoryType == "OR")
                {
                    applicationlifecycle = _context.Applications.Where(x => x.InvestPunjab_AppId == requestLog.AppId).FirstOrDefault().ApplicationLifeCycleStatusType;
                }

                if (projectSiteIdResp.HasError)
                {
                    serviceGatewayResponse.CanApply = false;
                    serviceGatewayResponse.RequestDeniedReason = "Issue in open application form (Error code: 100)";
                }
                else if (projectSiteIdResp.ResponseDataModel == 0)
                {
                    serviceGatewayResponse.CanApply = false;
                    serviceGatewayResponse.RequestDeniedReason = "Issue in open application form (Error code: 101)";
                }
                else
                {
                    serviceGatewayResponse.CanApply = true;
                    serviceGatewayResponse.IPin = requestLog.IPin;
                    serviceGatewayResponse.NativeUserId = genericServiceResultTemplate.ResponseDataModel.NativeUserId;
                    serviceGatewayResponse.NativeAppId = genericServiceResultTemplate.ResponseDataModel.NativeAppId; //isAppRefIdToBeSet ? genericServiceResultTemplate.ResponseDataModel.NativeAppId: 0;
                    serviceGatewayResponse.ServiceCode = requestLog.ServiceCode;
                    serviceGatewayResponse.ProjectSiteRefId = projectSiteId;
                    
                    var generateTokenResp = await _iAuthService.GenerateNewJwtToken(loggedUserInfoViewModel, httpRequest);
                    serviceGatewayResponse.SymmetricKey = generateTokenResp.Token;
                    serviceGatewayResponse.EncryptionKey = generateTokenResp.EncryptionKey;
                    serviceGatewayResponse.IVKey = generateTokenResp.IVKey;
                    serviceGatewayResponse.CategoryTypeId = serviceGatewayRequestJsonData.CategoryTypeId;
                    serviceGatewayResponse.IsOtpVerificationRequired = isOtpVerificationRequired;
                    serviceGatewayResponse.ContactSnapshot = contactSnapshot;
                    serviceGatewayResponse.InvestPunjab_AppId = requestLog.AppId;
                    serviceGatewayResponse.IsEntityKeysToKeepSame = isAppRefIdToBeSet;
                    serviceGatewayResponse.LicenceNo = serviceGatewayRequestJsonData.FileNo;
                    serviceGatewayResponse.ToDoActivityCategoryType = ToDoActivityCategoryTypeEnum.APPLICATION_FORM_MAIN_PART_SAVE;
                    serviceGatewayResponse.RootActivityRefId = DateTimeOffset.Now.ToUnixTimeSeconds().ToString();
                    var appType = FindServiceCodeMappingWithApplicationType(serviceGatewayRequestJsonData.ServiceCode);
                    serviceGatewayResponse.ToDoActivityModeType = serviceGatewayResponse.ToDoActivityModeType = serviceGatewayRequestJsonData.ServiceCode == 1001 ? ToDoActivityModeTypeEnum.DEFAULT
                        : (serviceGatewayRequestJsonData.CategoryType == "FT" && appType.ApplicationPurposeType == ApplicationPurposeTypeEnum.GRANT_LICENCE) ? ToDoActivityModeTypeEnum.ADD_NEW_MODE
                        : ((appType.ApplicationPurposeType == ApplicationPurposeTypeEnum.GRANT_LICENCE || appType.ApplicationPurposeType == ApplicationPurposeTypeEnum.RENEWAL_LICENCE || appType.ApplicationPurposeType == ApplicationPurposeTypeEnum.AMENDMENT_LICENCE) && applicationlifecycle == ApplicationLifeCycleStatusTypeEnum.IN_OBJECTION_WITH_BALANCE_FEE) ? ToDoActivityModeTypeEnum.RESOLVE_OBJECTION_BALANCE_FEE_MODE
                        : (appType.ApplicationPurposeType == ApplicationPurposeTypeEnum.RENEWAL_LICENCE || appType.ApplicationPurposeType == ApplicationPurposeTypeEnum.AMENDMENT_LICENCE || serviceGatewayRequestJsonData.CategoryType == "ED") ? ToDoActivityModeTypeEnum.EDIT_MODE
                        : ToDoActivityModeTypeEnum.RESOLVE_OBJECTION_MODE;
                    serviceGatewayResponse.ProjectSiteVersion = _context.ProjectSites.Where(x => x.ProjectSiteId == projectSiteId).Select(x => x.ProjectSiteVersion).FirstOrDefault();
                }
            }

            return serviceGatewayResponse;
        }


        public ServiceCodeApplicationTypeMapperViewModel FindServiceCodeMappingWithApplicationType(int serviceCode)
        {
            List<ServiceCodeApplicationTypeMapperViewModel> serviceCodeApplicationTypeMapper = new List<ServiceCodeApplicationTypeMapperViewModel>();

            serviceCodeApplicationTypeMapper.Add(new ServiceCodeApplicationTypeMapperViewModel() { ServiceCode = 6, ApplicationType = ApplicationTypeEnum.FACTORY_LICENCE, ApplicationPurposeType = ApplicationPurposeTypeEnum.GRANT_LICENCE });
            serviceCodeApplicationTypeMapper.Add(new ServiceCodeApplicationTypeMapperViewModel() { ServiceCode = 7, ApplicationType = ApplicationTypeEnum.FACTORY_LICENCE, ApplicationPurposeType = ApplicationPurposeTypeEnum.RENEWAL_LICENCE });
            serviceCodeApplicationTypeMapper.Add(new ServiceCodeApplicationTypeMapperViewModel() { ServiceCode = 8, ApplicationType = ApplicationTypeEnum.FACTORY_LICENCE, ApplicationPurposeType = ApplicationPurposeTypeEnum.AMENDMENT_LICENCE });

            serviceCodeApplicationTypeMapper.Add(new ServiceCodeApplicationTypeMapperViewModel() { ServiceCode = 5, ApplicationType = ApplicationTypeEnum.SHOP_LICENCE, ApplicationPurposeType = ApplicationPurposeTypeEnum.GRANT_LICENCE });
            serviceCodeApplicationTypeMapper.Add(new ServiceCodeApplicationTypeMapperViewModel() { ServiceCode = 15, ApplicationType = ApplicationTypeEnum.SHOP_LICENCE, ApplicationPurposeType = ApplicationPurposeTypeEnum.AMENDMENT_LICENCE });

            serviceCodeApplicationTypeMapper.Add(new ServiceCodeApplicationTypeMapperViewModel() { ServiceCode = 61, ApplicationType = ApplicationTypeEnum.WOMEN_NIGHT_SHIFT_SHOP, ApplicationPurposeType = ApplicationPurposeTypeEnum.GRANT_LICENCE });
            serviceCodeApplicationTypeMapper.Add(new ServiceCodeApplicationTypeMapperViewModel() { ServiceCode = 62, ApplicationType = ApplicationTypeEnum.WOMEN_NIGHT_SHIFT_FACTORY, ApplicationPurposeType = ApplicationPurposeTypeEnum.GRANT_LICENCE });

            serviceCodeApplicationTypeMapper.Add(new ServiceCodeApplicationTypeMapperViewModel() { ServiceCode = 3, ApplicationType = ApplicationTypeEnum.PRINCIPAL_EMPLOYER, ApplicationPurposeType = ApplicationPurposeTypeEnum.GRANT_LICENCE });
            serviceCodeApplicationTypeMapper.Add(new ServiceCodeApplicationTypeMapperViewModel() { ServiceCode = 11, ApplicationType = ApplicationTypeEnum.PRINCIPAL_EMPLOYER, ApplicationPurposeType = ApplicationPurposeTypeEnum.AMENDMENT_LICENCE });

            serviceCodeApplicationTypeMapper.Add(new ServiceCodeApplicationTypeMapperViewModel() { ServiceCode = 4, ApplicationType = ApplicationTypeEnum.CONTRACT_LABOUR, ApplicationPurposeType = ApplicationPurposeTypeEnum.GRANT_LICENCE });
            serviceCodeApplicationTypeMapper.Add(new ServiceCodeApplicationTypeMapperViewModel() { ServiceCode = 12, ApplicationType = ApplicationTypeEnum.CONTRACT_LABOUR, ApplicationPurposeType = ApplicationPurposeTypeEnum.RENEWAL_LICENCE });
            serviceCodeApplicationTypeMapper.Add(new ServiceCodeApplicationTypeMapperViewModel() { ServiceCode = 13, ApplicationType = ApplicationTypeEnum.CONTRACT_LABOUR, ApplicationPurposeType = ApplicationPurposeTypeEnum.AMENDMENT_LICENCE });

            serviceCodeApplicationTypeMapper.Add(new ServiceCodeApplicationTypeMapperViewModel() { ServiceCode = 35, ApplicationType = ApplicationTypeEnum.BOCW_ESTABLISHMENT_ACT, ApplicationPurposeType = ApplicationPurposeTypeEnum.GRANT_LICENCE });
            serviceCodeApplicationTypeMapper.Add(new ServiceCodeApplicationTypeMapperViewModel() { ServiceCode = 36, ApplicationType = ApplicationTypeEnum.BOCW_ESTABLISHMENT_ACT, ApplicationPurposeType = ApplicationPurposeTypeEnum.AMENDMENT_LICENCE });

            serviceCodeApplicationTypeMapper.Add(new ServiceCodeApplicationTypeMapperViewModel() { ServiceCode = 17, ApplicationType = ApplicationTypeEnum.ISM_PRINCIPAL_EMPLOYER, ApplicationPurposeType = ApplicationPurposeTypeEnum.GRANT_LICENCE });
            serviceCodeApplicationTypeMapper.Add(new ServiceCodeApplicationTypeMapperViewModel() { ServiceCode = 18, ApplicationType = ApplicationTypeEnum.ISM_PRINCIPAL_EMPLOYER, ApplicationPurposeType = ApplicationPurposeTypeEnum.AMENDMENT_LICENCE });

            serviceCodeApplicationTypeMapper.Add(new ServiceCodeApplicationTypeMapperViewModel() { ServiceCode = 22, ApplicationType = ApplicationTypeEnum.MOTOR_TRANSPORT, ApplicationPurposeType = ApplicationPurposeTypeEnum.GRANT_LICENCE });
            serviceCodeApplicationTypeMapper.Add(new ServiceCodeApplicationTypeMapperViewModel() { ServiceCode = 23, ApplicationType = ApplicationTypeEnum.MOTOR_TRANSPORT, ApplicationPurposeType = ApplicationPurposeTypeEnum.RENEWAL_LICENCE });
            serviceCodeApplicationTypeMapper.Add(new ServiceCodeApplicationTypeMapperViewModel() { ServiceCode = 24, ApplicationType = ApplicationTypeEnum.MOTOR_TRANSPORT, ApplicationPurposeType = ApplicationPurposeTypeEnum.AMENDMENT_LICENCE });

            serviceCodeApplicationTypeMapper.Add(new ServiceCodeApplicationTypeMapperViewModel() { ServiceCode = 63, ApplicationType = ApplicationTypeEnum.BUILDING_PLAN_HUD, ApplicationPurposeType = ApplicationPurposeTypeEnum.GRANT_LICENCE });
            serviceCodeApplicationTypeMapper.Add(new ServiceCodeApplicationTypeMapperViewModel() { ServiceCode = 70, ApplicationType = ApplicationTypeEnum.FACTORY_LICENCE, ApplicationPurposeType = ApplicationPurposeTypeEnum.GRANT_LICENCE });
            serviceCodeApplicationTypeMapper.Add(new ServiceCodeApplicationTypeMapperViewModel() { ServiceCode = 76, ApplicationType = ApplicationTypeEnum.BP_DECLARATION_STABILITY_CERTIFICATE, ApplicationPurposeType = ApplicationPurposeTypeEnum.GRANT_LICENCE });

            serviceCodeApplicationTypeMapper.Add(new ServiceCodeApplicationTypeMapperViewModel() { ServiceCode = 71, ApplicationType = ApplicationTypeEnum.BUILDING_PLAN_PROPOSED, ApplicationPurposeType = ApplicationPurposeTypeEnum.GRANT_LICENCE });
            serviceCodeApplicationTypeMapper.Add(new ServiceCodeApplicationTypeMapperViewModel() { ServiceCode = 72, ApplicationType = ApplicationTypeEnum.BUILDING_PLAN_EXISTING, ApplicationPurposeType = ApplicationPurposeTypeEnum.GRANT_LICENCE });
            serviceCodeApplicationTypeMapper.Add(new ServiceCodeApplicationTypeMapperViewModel() { ServiceCode = 73, ApplicationType = ApplicationTypeEnum.BUILDING_PLAN_ADDITION_AMENDMENT, ApplicationPurposeType = ApplicationPurposeTypeEnum.GRANT_LICENCE });
            serviceCodeApplicationTypeMapper.Add(new ServiceCodeApplicationTypeMapperViewModel() { ServiceCode = 74, ApplicationType = ApplicationTypeEnum.BUILDING_PLAN_UNDER_FACTORY_ACT, ApplicationPurposeType = ApplicationPurposeTypeEnum.GRANT_LICENCE });

            serviceCodeApplicationTypeMapper.Add(new ServiceCodeApplicationTypeMapperViewModel() { ServiceCode = 81, ApplicationType = ApplicationTypeEnum.BUILDING_PLAN_PSIEC, ApplicationPurposeType = ApplicationPurposeTypeEnum.GRANT_LICENCE });


            serviceCodeApplicationTypeMapper.Add(new ServiceCodeApplicationTypeMapperViewModel() { ServiceCode = 25, ApplicationType = ApplicationTypeEnum.TRADE_UNION, ApplicationPurposeType = ApplicationPurposeTypeEnum.GRANT_LICENCE });
            serviceCodeApplicationTypeMapper.Add(new ServiceCodeApplicationTypeMapperViewModel() { ServiceCode = 26, ApplicationType = ApplicationTypeEnum.TRADE_UNION, ApplicationPurposeType = ApplicationPurposeTypeEnum.AMENDMENT_LICENCE });

            serviceCodeApplicationTypeMapper.Add(new ServiceCodeApplicationTypeMapperViewModel() { ServiceCode = 19, ApplicationType = ApplicationTypeEnum.ISM_CONTRACT_LABOUR, ApplicationPurposeType = ApplicationPurposeTypeEnum.GRANT_LICENCE });
            serviceCodeApplicationTypeMapper.Add(new ServiceCodeApplicationTypeMapperViewModel() { ServiceCode = 20, ApplicationType = ApplicationTypeEnum.ISM_CONTRACT_LABOUR, ApplicationPurposeType = ApplicationPurposeTypeEnum.RENEWAL_LICENCE });
            serviceCodeApplicationTypeMapper.Add(new ServiceCodeApplicationTypeMapperViewModel() { ServiceCode = 21, ApplicationType = ApplicationTypeEnum.ISM_CONTRACT_LABOUR, ApplicationPurposeType = ApplicationPurposeTypeEnum.AMENDMENT_LICENCE });

            serviceCodeApplicationTypeMapper.Add(new ServiceCodeApplicationTypeMapperViewModel() { ServiceCode = 101, ApplicationType = ApplicationTypeEnum.OSH_FORM_1_Registration, ApplicationPurposeType = ApplicationPurposeTypeEnum.GRANT_LICENCE });


            return serviceCodeApplicationTypeMapper.Where(x => x.ServiceCode == serviceCode).FirstOrDefault();
        }

        public ServiceCodeApplicationTypeMapperViewModel FindServiceCodeWithApplicationType_Purpose(ApplicationTypeEnum applicationType, ApplicationPurposeTypeEnum applicationPurposeType)
        {
            List<ServiceCodeApplicationTypeMapperViewModel> serviceCodeApplicationTypeMapper = new List<ServiceCodeApplicationTypeMapperViewModel>();

            serviceCodeApplicationTypeMapper.Add(new ServiceCodeApplicationTypeMapperViewModel() { ServiceCode = 6, ApplicationType = ApplicationTypeEnum.FACTORY_LICENCE, ApplicationPurposeType = ApplicationPurposeTypeEnum.GRANT_LICENCE });
            serviceCodeApplicationTypeMapper.Add(new ServiceCodeApplicationTypeMapperViewModel() { ServiceCode = 7, ApplicationType = ApplicationTypeEnum.FACTORY_LICENCE, ApplicationPurposeType = ApplicationPurposeTypeEnum.RENEWAL_LICENCE });
            serviceCodeApplicationTypeMapper.Add(new ServiceCodeApplicationTypeMapperViewModel() { ServiceCode = 8, ApplicationType = ApplicationTypeEnum.FACTORY_LICENCE, ApplicationPurposeType = ApplicationPurposeTypeEnum.AMENDMENT_LICENCE });

            serviceCodeApplicationTypeMapper.Add(new ServiceCodeApplicationTypeMapperViewModel() { ServiceCode = 5, ApplicationType = ApplicationTypeEnum.SHOP_LICENCE, ApplicationPurposeType = ApplicationPurposeTypeEnum.GRANT_LICENCE });
            serviceCodeApplicationTypeMapper.Add(new ServiceCodeApplicationTypeMapperViewModel() { ServiceCode = 15, ApplicationType = ApplicationTypeEnum.SHOP_LICENCE, ApplicationPurposeType = ApplicationPurposeTypeEnum.AMENDMENT_LICENCE });

            serviceCodeApplicationTypeMapper.Add(new ServiceCodeApplicationTypeMapperViewModel() { ServiceCode = 4, ApplicationType = ApplicationTypeEnum.CONTRACT_LABOUR, ApplicationPurposeType = ApplicationPurposeTypeEnum.GRANT_LICENCE });
            serviceCodeApplicationTypeMapper.Add(new ServiceCodeApplicationTypeMapperViewModel() { ServiceCode = 12, ApplicationType = ApplicationTypeEnum.CONTRACT_LABOUR, ApplicationPurposeType = ApplicationPurposeTypeEnum.RENEWAL_LICENCE });
            serviceCodeApplicationTypeMapper.Add(new ServiceCodeApplicationTypeMapperViewModel() { ServiceCode = 13, ApplicationType = ApplicationTypeEnum.CONTRACT_LABOUR, ApplicationPurposeType = ApplicationPurposeTypeEnum.AMENDMENT_LICENCE });


            serviceCodeApplicationTypeMapper.Add(new ServiceCodeApplicationTypeMapperViewModel() { ServiceCode = 35, ApplicationType = ApplicationTypeEnum.BOCW_ESTABLISHMENT_ACT, ApplicationPurposeType = ApplicationPurposeTypeEnum.GRANT_LICENCE });
            serviceCodeApplicationTypeMapper.Add(new ServiceCodeApplicationTypeMapperViewModel() { ServiceCode = 36, ApplicationType = ApplicationTypeEnum.BOCW_ESTABLISHMENT_ACT, ApplicationPurposeType = ApplicationPurposeTypeEnum.AMENDMENT_LICENCE });

            serviceCodeApplicationTypeMapper.Add(new ServiceCodeApplicationTypeMapperViewModel() { ServiceCode = 17, ApplicationType = ApplicationTypeEnum.ISM_PRINCIPAL_EMPLOYER, ApplicationPurposeType = ApplicationPurposeTypeEnum.GRANT_LICENCE });
            serviceCodeApplicationTypeMapper.Add(new ServiceCodeApplicationTypeMapperViewModel() { ServiceCode = 18, ApplicationType = ApplicationTypeEnum.ISM_PRINCIPAL_EMPLOYER, ApplicationPurposeType = ApplicationPurposeTypeEnum.AMENDMENT_LICENCE });

            serviceCodeApplicationTypeMapper.Add(new ServiceCodeApplicationTypeMapperViewModel() { ServiceCode = 22, ApplicationType = ApplicationTypeEnum.MOTOR_TRANSPORT, ApplicationPurposeType = ApplicationPurposeTypeEnum.GRANT_LICENCE });
            serviceCodeApplicationTypeMapper.Add(new ServiceCodeApplicationTypeMapperViewModel() { ServiceCode = 23, ApplicationType = ApplicationTypeEnum.MOTOR_TRANSPORT, ApplicationPurposeType = ApplicationPurposeTypeEnum.RENEWAL_LICENCE });
            serviceCodeApplicationTypeMapper.Add(new ServiceCodeApplicationTypeMapperViewModel() { ServiceCode = 24, ApplicationType = ApplicationTypeEnum.MOTOR_TRANSPORT, ApplicationPurposeType = ApplicationPurposeTypeEnum.AMENDMENT_LICENCE });

            serviceCodeApplicationTypeMapper.Add(new ServiceCodeApplicationTypeMapperViewModel() { ServiceCode = 63, ApplicationType = ApplicationTypeEnum.BUILDING_PLAN_HUD, ApplicationPurposeType = ApplicationPurposeTypeEnum.GRANT_LICENCE });

            serviceCodeApplicationTypeMapper.Add(new ServiceCodeApplicationTypeMapperViewModel() { ServiceCode = 61, ApplicationType = ApplicationTypeEnum.WOMEN_NIGHT_SHIFT_SHOP, ApplicationPurposeType = ApplicationPurposeTypeEnum.GRANT_LICENCE });
            serviceCodeApplicationTypeMapper.Add(new ServiceCodeApplicationTypeMapperViewModel() { ServiceCode = 61, ApplicationType = ApplicationTypeEnum.WOMEN_NIGHT_SHIFT_SHOP, ApplicationPurposeType = ApplicationPurposeTypeEnum.AMENDMENT_LICENCE });
            serviceCodeApplicationTypeMapper.Add(new ServiceCodeApplicationTypeMapperViewModel() { ServiceCode = 76, ApplicationType = ApplicationTypeEnum.BP_DECLARATION_STABILITY_CERTIFICATE, ApplicationPurposeType = ApplicationPurposeTypeEnum.GRANT_LICENCE });

            serviceCodeApplicationTypeMapper.Add(new ServiceCodeApplicationTypeMapperViewModel() { ServiceCode = 62, ApplicationType = ApplicationTypeEnum.WOMEN_NIGHT_SHIFT_FACTORY, ApplicationPurposeType = ApplicationPurposeTypeEnum.GRANT_LICENCE });
            serviceCodeApplicationTypeMapper.Add(new ServiceCodeApplicationTypeMapperViewModel() { ServiceCode = 62, ApplicationType = ApplicationTypeEnum.WOMEN_NIGHT_SHIFT_FACTORY, ApplicationPurposeType = ApplicationPurposeTypeEnum.AMENDMENT_LICENCE });
            serviceCodeApplicationTypeMapper.Add(new ServiceCodeApplicationTypeMapperViewModel() { ServiceCode = 76, ApplicationType = ApplicationTypeEnum.BP_DECLARATION_STABILITY_CERTIFICATE, ApplicationPurposeType = ApplicationPurposeTypeEnum.GRANT_LICENCE });

            serviceCodeApplicationTypeMapper.Add(new ServiceCodeApplicationTypeMapperViewModel() { ServiceCode = 71, ApplicationType = ApplicationTypeEnum.BUILDING_PLAN_PROPOSED, ApplicationPurposeType = ApplicationPurposeTypeEnum.GRANT_LICENCE });
            serviceCodeApplicationTypeMapper.Add(new ServiceCodeApplicationTypeMapperViewModel() { ServiceCode = 72, ApplicationType = ApplicationTypeEnum.BUILDING_PLAN_EXISTING, ApplicationPurposeType = ApplicationPurposeTypeEnum.GRANT_LICENCE });
            serviceCodeApplicationTypeMapper.Add(new ServiceCodeApplicationTypeMapperViewModel() { ServiceCode = 73, ApplicationType = ApplicationTypeEnum.BUILDING_PLAN_ADDITION_AMENDMENT, ApplicationPurposeType = ApplicationPurposeTypeEnum.GRANT_LICENCE });
            serviceCodeApplicationTypeMapper.Add(new ServiceCodeApplicationTypeMapperViewModel() { ServiceCode = 74, ApplicationType = ApplicationTypeEnum.BUILDING_PLAN_UNDER_FACTORY_ACT, ApplicationPurposeType = ApplicationPurposeTypeEnum.GRANT_LICENCE });

            serviceCodeApplicationTypeMapper.Add(new ServiceCodeApplicationTypeMapperViewModel() { ServiceCode = 3, ApplicationType = ApplicationTypeEnum.PRINCIPAL_EMPLOYER, ApplicationPurposeType = ApplicationPurposeTypeEnum.GRANT_LICENCE });
            serviceCodeApplicationTypeMapper.Add(new ServiceCodeApplicationTypeMapperViewModel() { ServiceCode = 11, ApplicationType = ApplicationTypeEnum.PRINCIPAL_EMPLOYER, ApplicationPurposeType = ApplicationPurposeTypeEnum.AMENDMENT_LICENCE });

            serviceCodeApplicationTypeMapper.Add(new ServiceCodeApplicationTypeMapperViewModel() { ServiceCode = 81, ApplicationType = ApplicationTypeEnum.BUILDING_PLAN_PSIEC, ApplicationPurposeType = ApplicationPurposeTypeEnum.GRANT_LICENCE });

            serviceCodeApplicationTypeMapper.Add(new ServiceCodeApplicationTypeMapperViewModel() { ServiceCode = 19, ApplicationType = ApplicationTypeEnum.ISM_CONTRACT_LABOUR, ApplicationPurposeType = ApplicationPurposeTypeEnum.GRANT_LICENCE });
            serviceCodeApplicationTypeMapper.Add(new ServiceCodeApplicationTypeMapperViewModel() { ServiceCode = 20, ApplicationType = ApplicationTypeEnum.ISM_CONTRACT_LABOUR, ApplicationPurposeType = ApplicationPurposeTypeEnum.RENEWAL_LICENCE });
            serviceCodeApplicationTypeMapper.Add(new ServiceCodeApplicationTypeMapperViewModel() { ServiceCode = 21, ApplicationType = ApplicationTypeEnum.ISM_CONTRACT_LABOUR, ApplicationPurposeType = ApplicationPurposeTypeEnum.AMENDMENT_LICENCE });

            serviceCodeApplicationTypeMapper.Add(new ServiceCodeApplicationTypeMapperViewModel() { ServiceCode = 101, ApplicationType = ApplicationTypeEnum.OSH_FORM_1_Registration, ApplicationPurposeType = ApplicationPurposeTypeEnum.GRANT_LICENCE });

            return serviceCodeApplicationTypeMapper.Where(x => x.ApplicationType == applicationType && x.ApplicationPurposeType == applicationPurposeType).FirstOrDefault();
        }

        public async Task<GenericResponseTemplateModel<CreateUserProcessResponseViewModel>> CreateNewUser(Int64 iPin, string lagacyUserId)
        {
            GenericResponseTemplateModel<CreateUserProcessResponseViewModel> serviceGatewayResponse = new GenericResponseTemplateModel<CreateUserProcessResponseViewModel>() { ErrorDesc = null, HasError = false, ResponseDataModel = null };
            try
            {
                GenericResponseTemplateModel<BusinessFirstSCAFViewModel> genericResponseTemplateModel = await GetBusinessFirstCafDataByIPin(iPin);

                if (genericResponseTemplateModel.ResponseDataModel == null)
                {
                    serviceGatewayResponse.HasError = true;
                    serviceGatewayResponse.ErrorDesc = "Issue in get CAF data";
                }
                else
                {
                    Guid guid;
                    if (lagacyUserId != null && genericResponseTemplateModel.ResponseDataModel.AppUser.AppUserID == lagacyUserId)
                    {
                        guid = Guid.Parse(lagacyUserId);
                    }
                    else if (lagacyUserId != null && genericResponseTemplateModel.ResponseDataModel.AppUser.AppUserID != lagacyUserId)
                    {
                        guid = Guid.Parse(genericResponseTemplateModel.ResponseDataModel.AppUser.AppUserID);
                    }
                    else if (genericResponseTemplateModel.ResponseDataModel.AppUser.AppUserID != null)
                    {
                        guid = Guid.Parse(genericResponseTemplateModel.ResponseDataModel.AppUser.AppUserID);
                    }
                    else
                    {
                        guid = Guid.NewGuid();
                    }

                    var userDetailByUserName = await _iAuthService.GetUserFullDetailByUserName(genericResponseTemplateModel.ResponseDataModel.AppUser.Username);
                    if (userDetailByUserName.ListData.Count() == 0)
                    {
                        //var userDetail = await _iAuthService.GetUserLoginDeatilByUserId(guid.ToString());

                        var userCounts = _context.Users.Count(x => x.Id == guid.ToString());
                        //if (userDetail==null || !userDetail.IsUserRegistered)
                        if (userCounts == 0)
                        {
                            User user = new User()
                            {
                                AccessFailedCount = 0,
                                ConcurrencyStamp = "",
                                Email = "NoEmail@some.com",
                                EmailConfirmed = true,
                                Id = guid.ToString(),
                                LockoutEnabled = false,
                                LockoutEnd = null,
                                NormalizedEmail = "NoEmail@some.com",
                                NormalizedUserName = genericResponseTemplateModel.ResponseDataModel.AppUser.Username,
                                PasswordHash = "mHwh59uDN9MAGOBITRYJkR3FWK62fgqAjBKhFUePd8A=",
                                PhoneNumber = "0000000000",
                                PhoneNumberConfirmed = true,
                                SecurityStamp = "rI93o3GC0KZOhH7A+TX+8g==",
                                TwoFactorEnabled = false,
                                UserName = genericResponseTemplateModel.ResponseDataModel.AppUser.Username,
                                IsEnabled = false,
                                IsTestUser = false
                            };

                            await _context.AddAsync<User>(user);
                            await _context.SaveChangesAsync();
                            //await _userManager.CreateAsync(user);


                            List<StoreProcedureParm> storeProcedureParms = new List<StoreProcedureParm>()
                            {
                                new StoreProcedureParm (){ ParmName="RoleId", ParmValue="592add3e-f992-4983-a8ca-21ddc090bda0", isNumber=false},
                                new StoreProcedureParm (){ ParmName="UserId", ParmValue=user.Id, isNumber=false}
                            };

                            await _iGeneric_SP_Repository.CallStoreProcedureReaderScalerAsync("dbo.sp_MapUserWithRole", storeProcedureParms);

                            if (genericResponseTemplateModel.ResponseDataModel.CafInfo.ComPin == null)
                            {
                                genericResponseTemplateModel.ResponseDataModel.CafInfo.ComPin = "000000";
                            }


                            UserProfile userProfile = new UserProfile()
                            {
                                FirstName = genericResponseTemplateModel.ResponseDataModel.AppUser.FirstName,
                                MiddleName = genericResponseTemplateModel.ResponseDataModel.AppUser.MiddleName,
                                LastName = genericResponseTemplateModel.ResponseDataModel.AppUser.LastName,
                                FatherName = "NA",
                                MobileNo = genericResponseTemplateModel.ResponseDataModel.AppUser.PhoneNumber.Substring(genericResponseTemplateModel.ResponseDataModel.AppUser.PhoneNumber.Length - 10),
                                AlternateMobileNo = genericResponseTemplateModel.ResponseDataModel.AppUser.PhoneNumber.Substring(genericResponseTemplateModel.ResponseDataModel.AppUser.PhoneNumber.Length - 10),
                                Email = genericResponseTemplateModel.ResponseDataModel.AppUser.Email,
                                AlternateEmail = genericResponseTemplateModel.ResponseDataModel.AppUser.Email,
                                TehsilId = genericResponseTemplateModel.ResponseDataModel.CafInfo.SiteLGDTehId,
                                DistrictId = genericResponseTemplateModel.ResponseDataModel.CafInfo.SiteLGDDistId,
                                State = "Punjab",
                                PinCode = genericResponseTemplateModel.ResponseDataModel.CafInfo.ComPin == null ? "000000" : genericResponseTemplateModel.ResponseDataModel.CafInfo.ComPin.Substring(genericResponseTemplateModel.ResponseDataModel.CafInfo.ComPin.Length - 6),
                                Signature = "NA",
                                ProfilePhoto = "NA",
                                IsActive = true,
                                IsDeleted = false,
                                Createddate = DateTime.Now,
                                LastModifiedDate = DateTime.Now
                            };

                            var userProfileResponse = await _iAuthService.CreateNewUserProfile(userProfile);
                            await _iAuthService.MapUserWithProfile(user.Id, userProfile.UserProfileId);

                            //var factoryCircleId = 22;
                            //var alcCircleId = 10002;
                            //var labourCircleId = 10001;

                            //ProjectSite projectSite = new ProjectSite()
                            //{
                            //    EstablishmentName = genericResponseTemplateModel.ResponseDataModel.CafInfo.EstbName,
                            //    Address = genericResponseTemplateModel.ResponseDataModel.CafInfo.ComAddress,
                            //    VillageOrTown = genericResponseTemplateModel.ResponseDataModel.CafInfo.ComVlgName,
                            //    TehsilRefId = genericResponseTemplateModel.ResponseDataModel.CafInfo.SiteLGDTehId,
                            //    DistrictRefId = genericResponseTemplateModel.ResponseDataModel.CafInfo.SiteLGDDistId,
                            //    PinCode = genericResponseTemplateModel.ResponseDataModel.CafInfo.ComPin,
                            //    IsActive = true,
                            //    IsDeleted = false,
                            //    Createddate = DateTime.Now,
                            //    LastModifiedDate = DateTime.Now,
                            //    UserRefId = user.Id,
                            //    LabourCircleRefId = labourCircleId,
                            //    FactoryCircleRefId = factoryCircleId
                            //};

                            //var projectSiteIdCreateResp = await _iProjectSiteService.CreateProjectSite(projectSite, true, iPin.ToString());

                            // Update BusinessFirst_RequestLog - UserId
                            await UpdateBusinessFirstRequestNativeUserNativeByIPin(iPin, user.Id);
                            var ContactSnapshotByteCode = Encoding.UTF8.GetBytes(userProfile.MobileNo + "|" + userProfile.Email);
                            serviceGatewayResponse.ResponseDataModel = new CreateUserProcessResponseViewModel()
                            {
                                UserId = user.Id,
                                //ProjectSiteId = projectSiteIdCreateResp.ResponseDataModel,
                                ContactSnapshot = Convert.ToBase64String(ContactSnapshotByteCode)
                            };
                        }
                        else
                        {
                            var userDetail = await _iAuthService.GetUserLoginDeatilByUserId(guid.ToString());
                            //var projectSiteDetail = await _iProjectSiteService.GetProjectSiteRefIdByUserId(userDetail.UserId);
                            var userProfileDetail = await _iAuthService.GetUserProfileByProfileId(userDetail.UserProfileId);
                            var ContactSnapshotByteCode = Encoding.UTF8.GetBytes(userProfileDetail.MobileNo + "|" + userProfileDetail.Email);
                            await UpdateBusinessFirstRequestNativeUserNativeByIPin(iPin, userDetail.UserId);
                            serviceGatewayResponse.ResponseDataModel = new CreateUserProcessResponseViewModel()
                            {
                                UserId = userDetail.UserId,
                                //ProjectSiteId = projectSiteDetail.ResponseDataModel,
                                ContactSnapshot = Convert.ToBase64String(ContactSnapshotByteCode)
                            };
                        }
                    }
                    else
                    {
                        var userDetail = await _iAuthService.GetUserLoginDeatilByUserId(guid.ToString());
                        if (userDetail.IsUserRegistered)
                        {
                            //var projectSiteDetail = await _iProjectSiteService.GetProjectSiteRefIdByIpin(userDetail.UserId);
                            await UpdateBusinessFirstRequestNativeUserNativeByIPin(iPin, userDetail.UserId);
                            var userProfileDetail = await _iAuthService.GetUserProfileByProfileId(userDetail.UserProfileId);
                            var ContactSnapshotByteCode = Encoding.UTF8.GetBytes(userProfileDetail.MobileNo + "|" + userProfileDetail.Email);
                            serviceGatewayResponse.ResponseDataModel = new CreateUserProcessResponseViewModel()
                            {
                                UserId = userDetail.UserId,
                                //ProjectSiteId = projectSiteDetail.ResponseDataModel,
                                ContactSnapshot = Convert.ToBase64String(ContactSnapshotByteCode)
                            };
                        }
                        else
                        {
                            serviceGatewayResponse.HasError = true;
                            serviceGatewayResponse.ErrorDesc = "Username already in use..!";
                        }
                    }

                    if (!serviceGatewayResponse.HasError)
                    {
                        var projectSiteDetail = await _iProjectSiteService.GetProjectSiteRefIdByIpin(iPin.ToString());
                        if (projectSiteDetail.ResponseDataModel == 0)
                        {
                            var factoryCircleId = _context.FactoryCircles.Where(x => x.DistrictLgdRefId == genericResponseTemplateModel.ResponseDataModel.CafInfo.SiteLGDDistId).Select(x => x.FactoryCircleId).FirstOrDefault();
                            var alcCircleId = 10002;
                            var labourCircleId = 10001;

                            if (genericResponseTemplateModel.ResponseDataModel.CafInfo.ComPin == null)
                            {
                                genericResponseTemplateModel.ResponseDataModel.CafInfo.ComPin = "000000";
                            }

                            ProjectSite projectSite = new ProjectSite()
                            {
                                EstablishmentName = genericResponseTemplateModel.ResponseDataModel.CafInfo.EstbName,
                                Address = genericResponseTemplateModel.ResponseDataModel.CafInfo.SiteAddress == null ? "-" : genericResponseTemplateModel.ResponseDataModel.CafInfo.SiteAddress,
                                VillageOrTown = genericResponseTemplateModel.ResponseDataModel.CafInfo.ComVlgName == null ? "-" : genericResponseTemplateModel.ResponseDataModel.CafInfo.ComVlgName,
                                TehsilRefId = genericResponseTemplateModel.ResponseDataModel.CafInfo.SiteLGDTehId,
                                DistrictRefId = genericResponseTemplateModel.ResponseDataModel.CafInfo.SiteLGDDistId,
                                PinCode = genericResponseTemplateModel.ResponseDataModel.CafInfo.ComPin == null ? "000000" : genericResponseTemplateModel.ResponseDataModel.CafInfo.ComPin.Substring(genericResponseTemplateModel.ResponseDataModel.CafInfo.ComPin.Length - 6),
                                IsActive = true,
                                IsDeleted = false,
                                Createddate = DateTime.Now,
                                LastModifiedDate = DateTime.Now,
                                UserRefId = serviceGatewayResponse.ResponseDataModel.UserId,
                                LabourCircleRefId = labourCircleId,
                                FactoryCircleRefId = factoryCircleId,
                                ApplicantAadharNumber = genericResponseTemplateModel.ResponseDataModel.CafInfo.AadharNum,
                                ApplicantAadharAttachment = genericResponseTemplateModel.ResponseDataModel.CafInfo.SC_Aadhar,
                                ApplicantPanNumber = genericResponseTemplateModel.ResponseDataModel.CafInfo.ApplicantPan,
                                ApplicantPanAttachment = genericResponseTemplateModel.ResponseDataModel.CafInfo.ApplicantPanAttachment,
                                CompanyPanNumber = genericResponseTemplateModel.ResponseDataModel.CafInfo.CompanyPan,
                                CompanyPanAttachment = genericResponseTemplateModel.ResponseDataModel.CafInfo.CompanyPanAttachment,
                                ProjectPurpose = genericResponseTemplateModel.ResponseDataModel.CafInfo.ProjectPurpose,

                                ContactPersonFirstName = genericResponseTemplateModel.ResponseDataModel.CafInfo.First_Name,
                                ContactPersonMiddleName = genericResponseTemplateModel.ResponseDataModel.CafInfo.Middle_Name == null ? "" : genericResponseTemplateModel.ResponseDataModel.CafInfo.Middle_Name,
                                ContactPersonLastName = genericResponseTemplateModel.ResponseDataModel.CafInfo.Last_Name == null ? "." : genericResponseTemplateModel.ResponseDataModel.CafInfo.Last_Name,
                                ContactPersonEmail = genericResponseTemplateModel.ResponseDataModel.CafInfo.Email,
                                ContactPersonMobileNo = genericResponseTemplateModel.ResponseDataModel.CafInfo.MobileNo.Substring(genericResponseTemplateModel.ResponseDataModel.CafInfo.MobileNo.Length - 10),
                                AlternateEmail = "nomail@nomail.com",
                                AlternateMobileNo = "0000000000",
                                ProjectSiteVersion = 1
                            };
                            var projectSiteIdCreateResp = await _iProjectSiteService.CreateProjectSite(projectSite, true, iPin.ToString());
                            if (!projectSiteIdCreateResp.HasError)
                            {
                                serviceGatewayResponse.ResponseDataModel.ProjectSiteId = projectSiteIdCreateResp.ResponseDataModel.ProjectSiteId;
                                serviceGatewayResponse.ResponseDataModel.ProjectSiteVersion = projectSiteIdCreateResp.ResponseDataModel.ProjectSiteVersion;

                                //// Insert Into ProjectSiteLogs
                                //ProjectSiteLog projectSiteLog = new ProjectSiteLog()
                                //{
                                //    EstablishmentName = projectSite.EstablishmentName,
                                //    Address = projectSite.Address,
                                //    VillageOrTown = projectSite.VillageOrTown,
                                //    TehsilRefId = Convert.ToInt64(projectSite.TehsilLgd),
                                //    DistrictRefId = Convert.ToInt64(projectSite.DistrictLgd),
                                //    PinCode = projectSite.PinCode,
                                //    IsActive = true,
                                //    IsDeleted = false,
                                //    Createddate = DateTime.Now,
                                //    LastModifiedDate = DateTime.Now,
                                //    UserRefId = projectSite.UserRefId,
                                //    LabourCircleRefId = projectSite.LabourCircleRefId,
                                //    FactoryCircleRefId = projectSite.FactoryCircleRefId,
                                //    ApplicantAadharNumber = projectSite.ApplicantAadharNumber,
                                //    ApplicantAadharAttachment = projectSite.ApplicantAadharAttachment,
                                //    ApplicantPanNumber = projectSite.ApplicantPanNumber,
                                //    ApplicantPanAttachment = projectSite.ApplicantPanAttachment,
                                //    CompanyPanNumber = projectSite.CompanyPanNumber,
                                //    CompanyPanAttachment = projectSite.CompanyPanAttachment,
                                //    ProjectPurpose = projectSite.ProjectPurpose,
                                //    ContactPersonFirstName = projectSite.ContactPersonFirstName,
                                //    ContactPersonMiddleName = projectSite.ContactPersonMiddleName,
                                //    ContactPersonLastName = projectSite.ContactPersonLastName,
                                //    ContactPersonEmail = projectSite.ContactPersonEmail,
                                //    ContactPersonMobileNo = projectSite.ContactPersonMobileNo,
                                //    AlternateMobileNo = "NA",
                                //    AlternateEmail = "NA",
                                //    ProjectSiteVersion = projectSite.ProjectSiteVersion + 1
                                //};
                                //await _context.ProjectSiteLogs.AddAsync(projectSiteLog);
                                //_context.SaveChanges();
                            }
                            else
                            {
                                serviceGatewayResponse.HasError = true;
                                serviceGatewayResponse.ErrorDesc = projectSiteIdCreateResp.ErrorDesc;
                            }
                        }
                        else
                        {
                            serviceGatewayResponse.ResponseDataModel.ProjectSiteId = projectSiteDetail.ResponseDataModel;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                serviceGatewayResponse.HasError = true;
                serviceGatewayResponse.ErrorDesc = ex.Message;
                throw ex;
            }
            return serviceGatewayResponse;
        }

        public async Task<GenericResponseTemplateModel<ImportAndSeedDataResponseViewModel>> ImportAndSeedData(Int64 sys_O_AppId, string iPin, Int64 investPunjab_AppId, int serviceCode, string sys_o_token, string licenceNo, bool logsAlsoToBeSeeded)
        {
            GenericResponseTemplateModel<ImportAndSeedDataResponseViewModel> serviceGatewayResponse = new GenericResponseTemplateModel<ImportAndSeedDataResponseViewModel>() { ErrorDesc = null, HasError = false, ResponseDataModel = null };
            List<ProcessImd_ClearencesDataViewModel> foundClearences = new List<ProcessImd_ClearencesDataViewModel>();
            Int64 newAppId = 0;
            string AmendmentRenewalAppFormIds = "";
            CreateUserProcessResponseViewModel createUserProcessResponse = new CreateUserProcessResponseViewModel();
            try
            {
                //1. Get Registration Data
                #region Mapped All Registration NAR
                string reg_NAR = "";
                int reg_ServiceCode = 0;
                if (serviceCode == 2)
                {
                    reg_NAR = "BPF";
                }
                else if (serviceCode == 3 || serviceCode == 11)
                {
                    if (serviceCode == 3)
                    {
                        reg_NAR = "PEF";
                        reg_ServiceCode = 3;
                    }
                    else if (serviceCode == 11)
                    {
                        reg_NAR = "PEA";
                        reg_ServiceCode = 11;
                    }
                }
                else if (serviceCode == 4 || serviceCode == 12 || serviceCode == 13)
                {
                    reg_NAR = "CLF";
                }
                else if (serviceCode == 5 || serviceCode == 15)
                {
                    reg_NAR = "SCF";
                    reg_ServiceCode = 5;
                }
                else if (serviceCode == 6 || serviceCode == 7 || serviceCode == 8)
                {
                    reg_NAR = "FLM";
                    reg_ServiceCode = 6;
                    if (serviceCode == 7)
                    {
                        reg_NAR = "FLR";
                        reg_ServiceCode = 7;
                    }
                    else if (serviceCode == 8)
                    {
                        reg_NAR = "FLA";
                        reg_ServiceCode = 8;
                    }
                }
                else if (serviceCode == 17)
                {
                    reg_NAR = "IPR";
                }
                else if (serviceCode == 22)
                {
                    reg_NAR = "MTL";
                }
                else if (serviceCode == 25)
                {
                    reg_NAR = "TUR";
                }
                else if (serviceCode == 35)
                {
                    reg_NAR = "BOF";
                }

                #endregion
                //, Sys_O_AppId = sys_O_AppId
                var data = await _iDapperRepository.GetMultipleResultSets<SqlMapper.GridReader>("[dbo].[sp_GetProcess_IMD_Registration_Data]", new { sysOAppId = sys_O_AppId, IPin = sys_o_token != "" ? sys_o_token : iPin, ServiceCode = reg_ServiceCode, NAR = reg_NAR, LicenceNo = licenceNo });
                var service = data.Read<ProcessImd_ServiceViewModel>();
                var documemts = data.Read<ProcessImd_ClearencesDocumemtViewModel>();
                var logs = data.Read<ProcessImd_ClearencesLogViewModel>();
                var fees = data.Read<ProcessImd_ClearencesFeeViewModel>();
                var applicantDetails = data.Read<ProcessImd_ClearencesApplicantDetailsViewModel>();

                #region Registration Master Data
                ProcessImd_ClearencesDataViewModel ClearenceNar = new ProcessImd_ClearencesDataViewModel();

                if (serviceCode == 2) // Building Plan
                {
                    AmendmentRenewalAppFormIds = "";
                    var licence = await _iDapperRepository.Get<ProcessImd_ClearencesBuildingPlanViewModel>("[dbo].[sp_GetProcess_IMD_GetMasterData]", new { sysOAppId = sys_O_AppId, IPin = iPin, AppFormId = 2, NAR = "BPF" }, CommandType.StoredProcedure).ConfigureAwait(false);
                    ClearenceNar.BuildingPlan_Master = licence.FirstOrDefault();
                }

                else if (serviceCode == 3 || serviceCode == 11) // Principal Employer
                {
                    AmendmentRenewalAppFormIds = "11";
                    var licence = await _iDapperRepository.Get<ProcessImd_ClearencesEstablishmentRegistrationViewModel>("[dbo].[sp_GetProcess_IMD_GetMasterData]", new { sysOAppId = sys_O_AppId, IPin = iPin, AppFormId = 3, NAR = "PEF", LicenceNo = licenceNo }, CommandType.StoredProcedure).ConfigureAwait(false);
                    ClearenceNar.PrincipalEmployer_Master = licence.FirstOrDefault();
                }

                else if (serviceCode == 4 || serviceCode == 12 || serviceCode == 13) // Contract Labour
                {
                    AmendmentRenewalAppFormIds = "4";
                    var licence = await _iDapperRepository.Get<ProcessImd_ClearencesContractLabourViewModel>("[dbo].[sp_GetProcess_IMD_GetMasterData]", new { sysOAppId = sys_O_AppId, IPin = sys_o_token != "" ? sys_o_token : iPin, AppFormId = 4, NAR = "CLF", LicenceNo = licenceNo }, CommandType.StoredProcedure).ConfigureAwait(false);

                    ClearenceNar.ContractLabour_Master = licence.FirstOrDefault();
                }

                else if (serviceCode == 5 || serviceCode == 15) // Shop
                {
                    AmendmentRenewalAppFormIds = "15";
                    var licence = await _iDapperRepository.Get<ProcessImd_ClearencesShopLicenceViewModel>("[dbo].[sp_GetProcess_IMD_GetMasterData]", new { sysOAppId = sys_O_AppId, IPin = sys_o_token != "" ? sys_o_token : iPin, AppFormId = 5, NAR = "SCF", LicenceNo = licenceNo }, CommandType.StoredProcedure).ConfigureAwait(false);
                    ClearenceNar.Shop_Master = licence.FirstOrDefault();
                }
                else if (serviceCode == 6 || serviceCode == 7 || serviceCode == 8) // Factory
                {
                    AmendmentRenewalAppFormIds = "7,8";
                    var licence = await _iDapperRepository.Get<ProcessImd_ClearencesFactoryLicenceViewModel>("[dbo].[sp_GetProcess_IMD_GetMasterData]", new { sysOAppId = sys_O_AppId, IPin = sys_o_token != "" ? sys_o_token : iPin, AppFormId = 6, NAR = "FLM", LicenceNo = licenceNo }, CommandType.StoredProcedure).ConfigureAwait(false);
                    ClearenceNar.Factory_Master = licence.LastOrDefault();
                }
                else if (serviceCode == 35) // BOCW 
                {
                    AmendmentRenewalAppFormIds = "36";
                    var licence = await _iDapperRepository.Get<ProcessImd_ClearencesBocwRegistrationViewModel>("[dbo].[sp_GetProcess_IMD_GetMasterData]", new { sysOAppId = sys_O_AppId, IPin = iPin, AppFormId = 35, NAR = "BOF" }, CommandType.StoredProcedure).ConfigureAwait(false);
                    ClearenceNar.BOCW_Master = licence.FirstOrDefault();
                }
                else if (serviceCode == 25) // Trade Union 
                {
                    AmendmentRenewalAppFormIds = "";
                    var licence = await _iDapperRepository.Get<ProcessImd_ClearencesTradeUnionViewModel>("[dbo].[sp_GetProcess_IMD_GetMasterData]", new { sysOAppId = sys_O_AppId, IPin = iPin, AppFormId = 25, NAR = "TUR" }, CommandType.StoredProcedure).ConfigureAwait(false);
                    ClearenceNar.TradeUnion_Master = licence.FirstOrDefault();
                }
                else if (serviceCode == 17)
                {
                    AmendmentRenewalAppFormIds = "18";
                    var licence = await _iDapperRepository.Get<ProcessImd_ClearencesInterstatePrincipalEmployersViewModel>("[dbo].[sp_GetProcess_IMD_GetMasterData]", new { sysOAppId = sys_O_AppId, IPin = iPin, AppFormId = 17, NAR = "IPR" }, CommandType.StoredProcedure).ConfigureAwait(false);
                    ClearenceNar.InterstatePrincipalEmployer_Master = licence.FirstOrDefault();
                }
                else if (serviceCode == 22)
                {
                    AmendmentRenewalAppFormIds = "23,24";
                    var licence = await _iDapperRepository.Get<ProcessImd_ClearencesMotorTransportViewModel>("[dbo].[sp_GetProcess_IMD_GetMasterData]", new { sysOAppId = sys_O_AppId, IPin = iPin, AppFormId = 22, NAR = "MTL" }, CommandType.StoredProcedure).ConfigureAwait(false);
                    ClearenceNar.MotorTransport_Master = licence.FirstOrDefault();
                }

                foundClearences.Add(new ProcessImd_ClearencesDataViewModel()
                {
                    ServiceData = service.FirstOrDefault(),
                    ClearencesFee = fees.ToList(),
                    ServiceLogs = logs.ToList(),
                    Documents = documemts.ToList(),
                    Shop_Master = ClearenceNar.Shop_Master,
                    Factory_Master = ClearenceNar.Factory_Master,
                    ContractLabour_Master = ClearenceNar.ContractLabour_Master,
                    ApplicantDetails = applicantDetails.FirstOrDefault()
                });

                #endregion

                //2. Get all NAR's (Renewals)
                var allNar = await _iDapperRepository.Get<NarDataViewModel>("[dbo].[sp_Process_IMD_FindAllNAR]", new { IPin = sys_o_token != "" ? sys_o_token : iPin, ServiceCodes = AmendmentRenewalAppFormIds }, CommandType.StoredProcedure);

                foreach (var item in allNar)
                {
                    data = await _iDapperRepository.GetMultipleResultSets<SqlMapper.GridReader>("[dbo].[sp_GetProcess_IMD_Registration_Data]", new { sysOAppId = sys_O_AppId, IPin = sys_o_token != "" ? sys_o_token : iPin, ServiceCode = item.AppFormId, NAR = item.NAR, LicenceNo = licenceNo });
                    service = data.Read<ProcessImd_ServiceViewModel>();
                    documemts = data.Read<ProcessImd_ClearencesDocumemtViewModel>();
                    logs = data.Read<ProcessImd_ClearencesLogViewModel>();
                    fees = data.Read<ProcessImd_ClearencesFeeViewModel>();
                    applicantDetails = data.Read<ProcessImd_ClearencesApplicantDetailsViewModel>();
                    ClearenceNar = new ProcessImd_ClearencesDataViewModel()
                    {
                        ServiceData = service.FirstOrDefault(),
                        ClearencesFee = fees.ToList(),
                        //ServiceLogs = logs.ToList(),
                        Documents = documemts.ToList(),
                        ApplicantDetails = applicantDetails.FirstOrDefault()
                    };

                    // Renewal & Amendment *******
                    if (serviceCode == 12 || serviceCode == 13) // Contract Labour
                    {
                        var contractLabour = data.Read<ProcessImd_ClearencesContractLabourViewModel>();
                        ClearenceNar.ContractLabour_Master = contractLabour.FirstOrDefault();
                    }

                    else if (serviceCode == 6 || serviceCode == 7 || serviceCode == 8) // Factory
                    {
                        //var factoryLicence = data.Read<ProcessImd_ClearencesFactoryLicenceViewModel>();
                        var factoryLicence = await _iDapperRepository.Get<ProcessImd_ClearencesFactoryLicenceViewModel>("[dbo].[sp_GetProcess_IMD_GetMasterData]", new { sysOAppId = sys_O_AppId, IPin = iPin, AppFormId = item.AppFormId, NAR = item.NAR, LicenceNo = licenceNo }, CommandType.StoredProcedure).ConfigureAwait(false);
                        ClearenceNar.Factory_Master = factoryLicence.LastOrDefault();
                    }
                    else if (serviceCode == 23 || serviceCode == 24) // Motor Transport
                    {
                        var motorTransport = data.Read<ProcessImd_ClearencesMotorTransportViewModel>();
                        ClearenceNar.MotorTransport_Master = motorTransport.FirstOrDefault();
                    }
                    else if (serviceCode == 25 || serviceCode == 26) // Trade Union
                    {
                        var tradeUnion = data.Read<ProcessImd_ClearencesTradeUnionViewModel>();
                        ClearenceNar.TradeUnion_Master = tradeUnion.FirstOrDefault();
                    }

                    else if (serviceCode == 17 || serviceCode == 18) // Migrant 
                    {
                        var interStatePE = data.Read<ProcessImd_ClearencesInterstatePrincipalEmployersViewModel>();
                        ClearenceNar.InterstatePrincipalEmployer_Master = interStatePE.FirstOrDefault();
                    }

                    else if (serviceCode == 3 || serviceCode == 11) // Principal Emp
                    {
                        var pe = await _iDapperRepository.Get<ProcessImd_ClearencesEstablishmentRegistrationViewModel>("[dbo].[sp_GetProcess_IMD_GetMasterData]", new { sysOAppId = sys_O_AppId, IPin = iPin, AppFormId = item.AppFormId, NAR = item.NAR, LicenceNo = licenceNo }, CommandType.StoredProcedure).ConfigureAwait(false);
                        ClearenceNar.PrincipalEmployer_Master = pe.FirstOrDefault();
                    }


                    else if (item.AppFormId == 15) // Shop
                    {
                        //var shop = data.Read<ProcessImd_ClearencesShopLicenceViewModel>();
                        var shop = await _iDapperRepository.Get<ProcessImd_ClearencesShopLicenceViewModel>("[dbo].[sp_GetProcess_IMD_GetMasterData]", new { sysOAppId = sys_O_AppId, IPin = iPin, AppFormId = item.AppFormId, NAR = item.NAR, LicenceNo = licenceNo }, CommandType.StoredProcedure).ConfigureAwait(false);
                        ClearenceNar.Shop_Master = shop.FirstOrDefault();
                    }
                    else if (serviceCode == 36) // BOCW
                    {
                        var bocw = data.Read<ProcessImd_ClearencesBocwRegistrationViewModel>();
                        ClearenceNar.BOCW_Master = bocw.FirstOrDefault();
                    }

                    foundClearences.Add(ClearenceNar);

                    // await CopyLegacyFiles(newAppId, legacyFiles);
                }

                if (foundClearences.Count() > 0)
                {
                    var latestMasterData = new ProcessImd_ClearencesDataViewModel();
                    var userRes = await CreateNewUser(Convert.ToInt64(iPin), foundClearences.Where(x => x.ApplicantDetails != null).FirstOrDefault().ApplicantDetails.UserId);
                    if (!userRes.HasError)
                    {
                        if (foundClearences != null && foundClearences.Count() > 0)
                        {
                            Int64 legacyAppId = 0;
                            Int64 legacyAppFormId = 0;
                            string legacyNAR = "";

                            if (serviceCode == 5 || serviceCode == 15)
                            {
                                latestMasterData = foundClearences.LastOrDefault(x => x.Shop_Master != null);
                                if (latestMasterData != null)
                                {
                                    string exstingStr = latestMasterData.Shop_Master.NICCode;
                                    string nicCodes = "";
                                    if (exstingStr != null && exstingStr.Trim().Length > 0)
                                    {
                                        exstingStr = exstingStr.Replace(" ", ",");
                                        nicCodes = "[";
                                        foreach (var item in exstingStr.Split(','))
                                        {
                                            nicCodes = nicCodes + "\"" + item.Trim() + "\"" + ",";
                                        }


                                        nicCodes = nicCodes.Substring(0, nicCodes.Length - 1);
                                        nicCodes = nicCodes + "]";
                                    }
                                    else
                                    {
                                        nicCodes = "0";
                                    }


                                    string[] timeParts = latestMasterData.Shop_Master.OHours.Split(':');

                                    //int hour = int.Parse(timeParts[0]);
                                    //int minute = int.Parse(timeParts[1]);
                                    //int second = 0; // Since the input format doesn't include seconds

                                    var timeObject = new
                                    {
                                        hour = 9,
                                        minute = 0,
                                        second = 0
                                    };
                                    string jsonOHours = JsonConvert.SerializeObject(timeObject);

                                    timeParts = latestMasterData.Shop_Master.CHours.Split(':');

                                    //hour = int.Parse(timeParts[0]);
                                    //minute = int.Parse(timeParts[1]);
                                    //second = 0; // Since the input format doesn't include seconds

                                    timeObject = new
                                    {
                                        hour = 6,
                                        minute = 0,
                                        second = 0
                                    };
                                    string jsonCHours = JsonConvert.SerializeObject(timeObject);

                                    ShopLicence_GeneralDetail formData = new ShopLicence_GeneralDetail()
                                    {
                                        ClosingDay = ClosingDayTypeEnum.SUNDAY,
                                        OpeningHoursOfEstablishment = jsonOHours,
                                        ClosingHoursOfEstablishment = jsonCHours,
                                        OwnerName = latestMasterData.Shop_Master.OName == null ? "NA" : latestMasterData.Shop_Master.OName,
                                        OwnerFatherOrHusbandName = latestMasterData.Shop_Master.OFHName == null ? "NA" : latestMasterData.Shop_Master.OFHName,
                                        AadharNumber = latestMasterData.Shop_Master.AadharNo == null ? "00000000000" : latestMasterData.Shop_Master.AadharNo,
                                        EstablishmentConstitutionType = EstablishmentConstitutionTypeEnum.PROPRIETORSHIP,
                                        ManagerName = latestMasterData.Shop_Master.MgrName == null ? "NA" : latestMasterData.Shop_Master.MgrName,
                                        ShopType = ShopTypeEnum.SHOP,
                                        NationalIndustrialClassificationCode = latestMasterData.Shop_Master.NICCode == null ? "0" : nicCodes,
                                        IsHavingEmployee = Convert.ToInt32(latestMasterData.Shop_Master.HasEmp),
                                        AppRefId = 0
                                    };
                                    var Sys_N_ServiceCodeInfo = FindServiceCodeMappingWithApplicationType(serviceCode);
                                    var statusResp = await _iApplicationMamnagement_Shop_Service.InitiateApplication(ApplicationTypeEnum.SHOP_LICENCE, formData, userRes.ResponseDataModel.ProjectSiteId, Sys_N_ServiceCodeInfo.ApplicationPurposeType, userRes.ResponseDataModel.UserId, Convert.ToInt64(iPin), investPunjab_AppId, true, latestMasterData.Shop_Master.AppId, serviceCode, latestMasterData.Shop_Master.NAR, latestMasterData.ServiceData.LicenseNo, userRes.ResponseDataModel.ProjectSiteVersion == 0 ? 1 : userRes.ResponseDataModel.ProjectSiteVersion);
                                    newAppId = statusResp.AppId;

                                    // Get Employee Details 

                                    var employeeDetails = await _iSystem_O_CommunicationService.IsHavingEmployee(latestMasterData.Shop_Master.AppId);
                                    if (employeeDetails != null)
                                    {
                                        ShopLicence_EmployeeDetail employee = null;
                                        foreach (var item in employeeDetails.ResponseDataModel)
                                        {
                                            employee = new ShopLicence_EmployeeDetail()
                                            {
                                                Name = item.EmpName == null ? "0" : item.EmpName,
                                                FatherOrHusbandName = item.EmpFHName == null ? "NA" : item.EmpFHName,
                                                Gender = item.Gender == null ? "NA" : item.Gender,
                                                DateOfBirth = item.Date_of_Birth,
                                                MobileNumber = item.MobileNumber == null ? "NA" : item.MobileNumber,
                                                PanNumber = item.PanNumber == null ? "NA" : item.PanNumber,
                                                AadharNumber = item.AadharNumber == null ? "0" : item.AadharNumber,
                                                JoiningDate = employeeDetails.ResponseDataModel[0].JoiningDate,
                                                ClosingDayType = ClosingDayTypeEnum.SUNDAY,
                                                WorkingHoursFrom = item.WorkingHoursFrom.TimeOfDay.ToString(),
                                                WorkingHoursTo = item.WorkingHoursTo.TimeOfDay.ToString(),
                                                IntervalFrom = item.IntervalFrom.TimeOfDay.ToString(),
                                                IntervalTo = item.IntervalTo.TimeOfDay.ToString(),
                                                ShopLicenceRefId = formData.ShopLicenceId
                                            };
                                            await _context.ShopLicence_EmployeeDetails.AddAsync(employee);
                                            await _context.SaveChangesAsync();
                                        }
                                    }

                                    // Document

                                    List<ProcessImd_LegacyDocumentInfoViewModel> legacyFiles = new List<ProcessImd_LegacyDocumentInfoViewModel>();
                                    legacyFiles.Add(new ProcessImd_LegacyDocumentInfoViewModel()
                                    {
                                        SYS_N_DocId = 10003,
                                        Legacy_FileName = @"images/" + latestMasterData.Shop_Master.ProofShopEstd
                                    });

                                    legacyFiles.Add(new ProcessImd_LegacyDocumentInfoViewModel()
                                    {
                                        SYS_N_DocId = 10004,
                                        Legacy_FileName = @"images/" + latestMasterData.Shop_Master.OwnerProof
                                    });

                                    legacyFiles.Add(new ProcessImd_LegacyDocumentInfoViewModel()
                                    {
                                        SYS_N_DocId = 10005,
                                        Legacy_FileName = @"images/" + latestMasterData.Shop_Master.PhotoInteriorOne
                                    });

                                    legacyFiles.Add(new ProcessImd_LegacyDocumentInfoViewModel()
                                    {
                                        SYS_N_DocId = 10006,
                                        Legacy_FileName = @"images/" + latestMasterData.Shop_Master.PhotoFront
                                    });

                                    legacyFiles.Add(new ProcessImd_LegacyDocumentInfoViewModel()
                                    {
                                        SYS_N_DocId = 20005,
                                        Legacy_FileName = @"images/" + latestMasterData.Shop_Master.ApplicantSign
                                    });

                                    legacyFiles.Add(new ProcessImd_LegacyDocumentInfoViewModel()
                                    {
                                        SYS_N_DocId = 50,
                                        Legacy_FileName = @"images/" + latestMasterData.Shop_Master.OtherDocs
                                    });
                                    await CopyLegacyFiles(newAppId, legacyFiles);
                                }

                                legacyAppId = latestMasterData.Shop_Master.AppId;
                                legacyAppFormId = latestMasterData.Shop_Master.AppFormId;
                                legacyNAR = latestMasterData.Shop_Master.NAR;
                            }
                            else if (serviceCode == 6 || serviceCode == 7 || serviceCode == 8)
                            {
                                latestMasterData = foundClearences.LastOrDefault(x => x.Factory_Master != null);
                                if (latestMasterData != null)
                                {
                                    string exstingStr = latestMasterData.Factory_Master.NICCode;
                                    string nicCodes = "";
                                    if (exstingStr != null && exstingStr.Trim().Length > 0)
                                    {
                                        exstingStr = exstingStr.Replace(" ", ",");
                                        nicCodes = "[";
                                        foreach (var item in exstingStr.Split(','))
                                        {
                                            nicCodes = nicCodes + "\"" + item.Trim() + "\"" + ",";
                                        }
                                        nicCodes = nicCodes.Substring(0, nicCodes.Length - 1);
                                        nicCodes = nicCodes + "]";
                                    }
                                    else
                                    {
                                        nicCodes = "0";
                                    }

                                    Licence_Factory_GeneralDetail formData = new Licence_Factory_GeneralDetail()
                                    {
                                        IsBuildingConstructedBefore29June2018 = 1,
                                        HaveYouMadeChangesInBuildingPlan = 0,
                                        OldLicenceNo = licenceNo != null ? licenceNo : "NA",
                                        //OldLicenceValidUpTo =   foundClearences.FirstOrDefault().ServiceData.ValidUpTo ,
                                        OldLicenceValidUpTo = (foundClearences.FirstOrDefault()?.ServiceData != null && foundClearences.FirstOrDefault()?.ServiceData.ValidUpTo != null) ? foundClearences.FirstOrDefault().ServiceData.ValidUpTo : (DateTime?)null,
                                        OldLicenceTotalEmployees = latestMasterData.Factory_Master.MaximumNumberEmployeeInYear,
                                        OldLicenceFactoryKiloWatt = latestMasterData.Factory_Master.InstalledPower,
                                        RegistrationDate = latestMasterData.Factory_Master.RegistrationDate,

                                        //RenewalFromDate  = latestMasterData.Factory_Master.ApplicationDate,
                                        //RenewalFromDate = new DateTime(foundClearences.FirstOrDefault().ServiceData.ValidUpTo.Year+ 1, 1, 1),
                                        RenewalFromDate = serviceCode == 8 ? null : ((foundClearences.FirstOrDefault()?.ServiceData != null && foundClearences.FirstOrDefault()?.ServiceData.ValidUpTo != null) ? new DateTime(foundClearences.FirstOrDefault().ServiceData.ValidUpTo.Year + 1, 1, 1) : (DateTime?)null),

                                        NoOfYears = latestMasterData.Factory_Master.LicenceForNoOfYear,
                                        ManufacturingProcess_Last12Months = "NA",
                                        ManufacturingProcess_Next12Months = "NA",
                                        NationalIndustrialClassificationCode = nicCodes,
                                        MfgProducts_Last12Month = latestMasterData.Factory_Master.MFProc != null ? latestMasterData.Factory_Master.MFProc : "NA",
                                        Workers_MaxDuringYear = latestMasterData.Factory_Master.MaximumNumberEmployeeInYear,
                                        Workers_MaxLast12Month = latestMasterData.Factory_Master.MaximumNumberEmployeeLastYear,
                                        Workers_OrdinarilyEmployed = latestMasterData.Factory_Master.OrdinarilyEmployed,
                                        PowerKW_Installed = latestMasterData.Factory_Master.InstalledPower,
                                        PowerKW_MaxProposed = latestMasterData.Factory_Master.MaximumPowerUsed,
                                        ModifiedCounter = 1,
                                        AppIdRightToBusinessAct = "NA",
                                        BuildingPlanDofNumber = "NA",
                                        CompetentPersonUserId = "NA",
                                        DateOfPrincipalApproval = "NA",
                                        IsBuildingPlanApproved = 2,
                                        IsBuildingPlanVerified = true,
                                        IsRBAVerified = true,
                                        IsStabilityApproved = BuildingPlanStabilityAuthorityTypeEnum.DIRECTOR_FACTORIES,
                                        IsStabilityPlanVerified = true,
                                        IsTempRegistered = 0,
                                        IsTempRegistrationVerified = true,
                                        IsUnderRightToBusinessAct = 0,
                                        ProjectIdentificationNo = "NA",
                                        StabilityPlanDofNumber = "NA",
                                        TempRegistrationNumber = "NA",
                                        CompetentPersonName = "NA",
                                        CompetentPersonContactNo = "9999999999",
                                        CompetentPersonEmail = "NA",
                                        EngineerName = "NA",
                                        EngineerContactNo = "9999999999",
                                        EngineerEmail = "NA",
                                        BuildingPlanApprovalDate = DateTime.Now,
                                        BuildingPlanStabilityApprovalDate = DateTime.Now
                                    };

                                    var Sys_N_ServiceCodeInfo = FindServiceCodeMappingWithApplicationType(serviceCode);
                                    var statusResp = await _iApplicationMamnagement_Factory_Service.InitiateApplication(ApplicationTypeEnum.FACTORY_LICENCE, formData, userRes.ResponseDataModel.ProjectSiteId, Sys_N_ServiceCodeInfo.ApplicationPurposeType, userRes.ResponseDataModel.UserId, Convert.ToInt64(iPin), investPunjab_AppId, true, latestMasterData.Factory_Master.AppId, serviceCode, latestMasterData.Factory_Master.NAR, (latestMasterData.ServiceData != null && latestMasterData.ServiceData.LicenseNo != null) ? foundClearences.FirstOrDefault().ServiceData.LicenseNo : null, userRes.ResponseDataModel.ProjectSiteVersion == 0 ? 1 : userRes.ResponseDataModel.ProjectSiteVersion);
                                    newAppId = statusResp.AppId;

                                    Licence_Factory_OccupierAndManagerDetail occupierDetails = new Licence_Factory_OccupierAndManagerDetail()
                                    {
                                        ManagerFullName = latestMasterData.Factory_Master.ManagerFullName,
                                        ManagerFatherName = latestMasterData.Factory_Master.ManagerFatherName,
                                        ManagerFullAddress = latestMasterData.Factory_Master.ManagerFullAddress,
                                        ManagerMobile = latestMasterData.Factory_Master.ManagerMobile == null ? "NA" : latestMasterData.Factory_Master.ManagerMobile,
                                        ManagerEmail = latestMasterData.Factory_Master.ManagerEmail == null ? "NA" : latestMasterData.Factory_Master.ManagerEmail,
                                        ManagerResidentialAddress = latestMasterData.Factory_Master.ManagerFullAddress,
                                        OccupierFullName = latestMasterData.Factory_Master.OccupierFullName,
                                        OccupierFatherName = latestMasterData.Factory_Master.OccupierFatherName,
                                        OccupierFullAddress = latestMasterData.Factory_Master.OccupierFullAddress,
                                        OccupierMobile = latestMasterData.Factory_Master.OccupierMobile == null ? "NA" : latestMasterData.Factory_Master.OccupierMobile,
                                        OccupierEmail = latestMasterData.Factory_Master.OccupierEmail == null ? "NA" : latestMasterData.Factory_Master.OccupierEmail,
                                        OccupierResidentialAddress = latestMasterData.Factory_Master.OccupierFullAddress,
                                        OwnerName = latestMasterData.Factory_Master.OwnerName,
                                        OwnerPremisesAddress = latestMasterData.Factory_Master.OwnerPremisesAddress,
                                        StabilityCertificateNumber = "NA",
                                        StabilityCertificateDate = latestMasterData.Factory_Master.RegistrationDate,
                                        StabilityDOFNumber = "NA",
                                        Checklist_IsBuildingPlanApproved = "1",
                                        Checklist_IsLabourWelfareFundPaid = "1",
                                        Checklist_IsAnnualReturnFiled = "1",
                                        Checklist_IsStabilityCertificateAttached = "1",
                                        FactoryLicenceRefId = formData.FactoryLicenceId,
                                        ModifiedCounter = 1
                                    };
                                    await _context.Licence_Factory_OccupierAndManagerDetails.AddAsync(occupierDetails);
                                    await _context.SaveChangesAsync();

                                    // Document
                                    List<ProcessImd_LegacyDocumentInfoViewModel> legacyFiles = new List<ProcessImd_LegacyDocumentInfoViewModel>();
                                    legacyFiles.Add(new ProcessImd_LegacyDocumentInfoViewModel()
                                    {
                                        SYS_N_DocId = 20005,
                                        Legacy_FileName = @"images/" + documemts.Where(x => x.DocId == 27).Select(x => x.DocumentFile).FirstOrDefault()
                                    });

                                    legacyFiles.Add(new ProcessImd_LegacyDocumentInfoViewModel()
                                    {
                                        SYS_N_DocId = 50013,
                                        Legacy_FileName = @"images/" + latestMasterData.Factory_Master.OLProof
                                    });

                                    legacyFiles.Add(new ProcessImd_LegacyDocumentInfoViewModel()
                                    {
                                        SYS_N_DocId = 50014,
                                        Legacy_FileName = @"images/" + documemts.Where(x => x.DocId == 72).Select(x => x.DocumentFile).FirstOrDefault()
                                    });

                                    legacyFiles.Add(new ProcessImd_LegacyDocumentInfoViewModel()
                                    {
                                        SYS_N_DocId = 40013,
                                        Legacy_FileName = @"images/" + documemts.Where(x => x.DocId == 73).Select(x => x.DocumentFile).FirstOrDefault()
                                    });

                                    legacyFiles.Add(new ProcessImd_LegacyDocumentInfoViewModel()
                                    {
                                        SYS_N_DocId = 50015,
                                        Legacy_FileName = @"images/" + documemts.Where(x => x.DocId == 168).Select(x => x.DocumentFile).FirstOrDefault()
                                    });

                                    legacyFiles.Add(new ProcessImd_LegacyDocumentInfoViewModel()
                                    {
                                        SYS_N_DocId = 32,
                                        Legacy_FileName = @"images/" + documemts.Where(x => x.DocId == 166).Select(x => x.DocumentFile).FirstOrDefault()
                                    });

                                    legacyFiles.Add(new ProcessImd_LegacyDocumentInfoViewModel()
                                    {
                                        SYS_N_DocId = 50016,
                                        Legacy_FileName = @"images/" + documemts.Where(x => x.DocId == 74).Select(x => x.DocumentFile).FirstOrDefault()
                                    });

                                    await CopyLegacyFiles(newAppId, legacyFiles);
                                }
                                legacyAppId = latestMasterData.Factory_Master.AppId;
                                legacyAppFormId = latestMasterData.Factory_Master.AppFormId;
                                legacyNAR = latestMasterData.Factory_Master.NAR;
                            }
                            else if (serviceCode == 4 || serviceCode == 12 || serviceCode == 13)
                            {
                                latestMasterData = foundClearences.LastOrDefault(x => x.ContractLabour_Master != null);
                                if (latestMasterData != null)
                                {
                                    Licence_ContractLabour_GeneralDetail formData = new Licence_ContractLabour_GeneralDetail()
                                    {
                                        IsCoopraticSociety = "1",
                                        ContractorName = latestMasterData.ContractLabour_Master.ContractorName,
                                        ContractorFatherName = latestMasterData.ContractLabour_Master.ContractorFatherName ?? "NA",
                                        ContractorAddress = latestMasterData.ContractLabour_Master.ContractorAddress ?? "NA",
                                        ContractorDOB = latestMasterData.ContractLabour_Master.DOB ?? DateTime.Today,
                                        TypeOfBusiness = latestMasterData.ContractLabour_Master.TypeOfBusiness,
                                        RegistrationCertificateNo = latestMasterData.ContractLabour_Master.RegistrationCertificateNo,
                                        RegistrationCertificateDate = latestMasterData.ContractLabour_Master.RegistrationCertificateDate,
                                        PrincipalEmployerName = latestMasterData.ContractLabour_Master.PrincipalEmployerName,
                                        PrincipalEmployerAddress = latestMasterData.ContractLabour_Master.PrincipalEmployerAddress ?? "NA",
                                        NatureOfWork = latestMasterData.ContractLabour_Master.WorkNatureString,
                                        ContractWork_CommencingDate = latestMasterData.ContractLabour_Master.ContractWork_CommencingDate,
                                        ContractWork_TerminationDate = latestMasterData.ContractLabour_Master.ContractWork_TerminationDate,
                                        AgentOrManagerName = latestMasterData.ContractLabour_Master.AgentOrManagerName ?? "NA",
                                        AgentOrManagerAddress = latestMasterData.ContractLabour_Master.AgentOrManagerAddress ?? "NA",
                                        MaximumNumberOfEmployee = latestMasterData.ContractLabour_Master.MaximumNumberEmployee,
                                        ContractorAge = latestMasterData.ContractLabour_Master.Age,
                                        LicenceForYear = latestMasterData.ContractLabour_Master.RegistrationFrom,
                                        ModifiedCounter = 1,
                                        GstNumber = "000000000",
                                        PanOrTanNumber = "ABCDD1234E"
                                    };

                                    var Sys_N_ServiceCodeInfo = FindServiceCodeMappingWithApplicationType(serviceCode);
                                    var statusResp = await _iApplicationMamnagement_ContractLabour_Service.InitiateApplication(ApplicationTypeEnum.CONTRACT_LABOUR, formData, userRes.ResponseDataModel.ProjectSiteId, Sys_N_ServiceCodeInfo.ApplicationPurposeType, userRes.ResponseDataModel.UserId, Convert.ToInt64(iPin), investPunjab_AppId, true, latestMasterData.ContractLabour_Master.AppId, serviceCode, latestMasterData.ContractLabour_Master.NAR, latestMasterData.ServiceData.LicenseNo, userRes.ResponseDataModel.ProjectSiteVersion == 0 ? 1 : userRes.ResponseDataModel.ProjectSiteVersion);
                                    newAppId = statusResp.AppId;

                                    // Document
                                    List<ProcessImd_LegacyDocumentInfoViewModel> legacyFiles = new List<ProcessImd_LegacyDocumentInfoViewModel>();
                                    legacyFiles.Add(new ProcessImd_LegacyDocumentInfoViewModel()
                                    {
                                        SYS_N_DocId = 20005,
                                        Legacy_FileName = @"images/" + documemts.Where(x => x.DocId == 27).Select(x => x.DocumentFile).FirstOrDefault()
                                    });

                                    legacyFiles.Add(new ProcessImd_LegacyDocumentInfoViewModel()
                                    {
                                        SYS_N_DocId = 50013,
                                        Legacy_FileName = @"images/" + latestMasterData.ContractLabour_Master.OLProof
                                    });

                                    legacyFiles.Add(new ProcessImd_LegacyDocumentInfoViewModel()
                                    {
                                        SYS_N_DocId = 50014,
                                        Legacy_FileName = @"images/" + documemts.Where(x => x.DocId == 72).Select(x => x.DocumentFile).FirstOrDefault()
                                    });

                                    legacyFiles.Add(new ProcessImd_LegacyDocumentInfoViewModel()
                                    {
                                        SYS_N_DocId = 40013,
                                        Legacy_FileName = @"images/" + documemts.Where(x => x.DocId == 73).Select(x => x.DocumentFile).FirstOrDefault()
                                    });
                                    await CopyLegacyFiles(newAppId, legacyFiles);
                                }
                                legacyAppId = latestMasterData.ContractLabour_Master.AppId;
                                legacyAppFormId = latestMasterData.ContractLabour_Master.AppFormId;
                                legacyNAR = latestMasterData.ContractLabour_Master.NAR;
                            }
                            else if (serviceCode == 3 || serviceCode == 11)
                            {
                                latestMasterData = foundClearences.LastOrDefault(x => x.PrincipalEmployer_Master != null);
                                legacyAppId = latestMasterData.PrincipalEmployer_Master.AppId;
                                legacyAppFormId = latestMasterData.PrincipalEmployer_Master.AppFormId;
                                legacyNAR = latestMasterData.PrincipalEmployer_Master.NAR;
                                if (latestMasterData != null)
                                {
                                    Licence_CL_PE_GeneralDetail formData = new Licence_CL_PE_GeneralDetail()
                                    {
                                        PE_Name = latestMasterData.PrincipalEmployer_Master.PrincipalEmpName,
                                        PE_FatherName = latestMasterData.PrincipalEmployer_Master.PrincipalFatherEmpName,
                                        PE_Mobile = latestMasterData.PrincipalEmployer_Master.PrincipalEmpMobileNo ?? "0000000000",
                                        PE_Email = latestMasterData.PrincipalEmployer_Master.PrincipalEmpEmailId ?? "No Email",
                                        PE_Address = latestMasterData.PrincipalEmployer_Master.PrincipalEmpAddress ?? "No Address",
                                        Manager_Name = latestMasterData.PrincipalEmployer_Master.ManagerName,
                                        Manager_Mobile = latestMasterData.PrincipalEmployer_Master.ManagerMobileNo ?? "0000000000",
                                        Manager_Email = latestMasterData.PrincipalEmployer_Master.ManagerEmailId ?? "No Email",
                                        Manager_Address = latestMasterData.PrincipalEmployer_Master.ManagerAddress ?? "No Address",
                                        NatureOfWork = latestMasterData.PrincipalEmployer_Master.WorkCarried,
                                        TotalWorker = latestMasterData.PrincipalEmployer_Master.TotalWorker,
                                        AppRefId = 1,
                                        ProjectSiteRefId = 1000,
                                        IPin = 1000,
                                        InvestPunjab_AppId = 1000,
                                        AlcCircleRefId = 1000,
                                    };

                                    var sys_N_ServiceCodeInfo = FindServiceCodeMappingWithApplicationType(serviceCode);
                                    var statusResp = await _iApplicationMamnagement_CL_PE_Service.InitiateApplication(ApplicationTypeEnum.PRINCIPAL_EMPLOYER, formData, userRes.ResponseDataModel.ProjectSiteId, sys_N_ServiceCodeInfo.ApplicationPurposeType, userRes.ResponseDataModel.UserId, Convert.ToInt64(iPin), investPunjab_AppId, true, latestMasterData.PrincipalEmployer_Master.AppId, serviceCode, latestMasterData.PrincipalEmployer_Master.NAR, (latestMasterData.ServiceData != null && latestMasterData.ServiceData.LicenseNo != null) ? foundClearences.FirstOrDefault().ServiceData.LicenseNo : null, 1);
                                    newAppId = statusResp.AppId;

                                    // Get Employee Details 
                                    var contractordetails = await _iSystem_O_CommunicationService.GetContractorList(legacyAppId, legacyAppFormId, legacyNAR);

                                    if (contractordetails != null)
                                    {
                                        Licence_CL_PE_Contrator contractorlist = null;
                                        foreach (var item in contractordetails.ResponseDataModel)
                                        {
                                            contractorlist = new Licence_CL_PE_Contrator()
                                            {
                                                Name = item.ContractorName,
                                                Address = item.ContractorAddress,
                                                NatureOfWork = item.WorkNature,
                                                MaxLabourWorkerEmployed = (int)item.ContractLabourEmployed,
                                                DateOfCommencement = item.ContractWork_CommencingDate,
                                                DateOfTermination = item.ContractWork_TerminationDate,
                                                Licence_CL_PE_GeneralDetailRefId = statusResp.EntityKeyId,
                                                ModifiedCounter = 0
                                            };
                                            await _context.Licence_CL_PE_Contrator.AddAsync(contractorlist);
                                            await _context.SaveChangesAsync();
                                        }
                                    }
                                }


                            }
                            else if (serviceCode == 2 && legacyNAR.Contains("BPC"))
                            {
                                latestMasterData = foundClearences.LastOrDefault(x => x.Factory_Master != null);
                                if (latestMasterData != null)
                                {
                                    Licence_Proposed_BuildingPlan_GeneralDetail formData = new Licence_Proposed_BuildingPlan_GeneralDetail()
                                    {
                                        //DispatchNo = "NA",
                                        //DispatchDate = latestMasterData.Proposed_BuildingPlan_Master.IssueDate,
                                        //IsBuildingConstructedBefore_01_Oct_2008 = latestMasterData.Proposed_BuildingPlan_Master.IsBuildingConstBefore2008,
                                        //ProposedBuildingCost = latestMasterData.Proposed_BuildingPlan_Master.BuildingCost,
                                        //CoveredAreaSqFt = 0,
                                        //NoOfConstructionWorkers = latestMasterData.Proposed_BuildingPlan_Master.BOCW_NoOfWorkers
                                    };

                                    var Sys_N_ServiceCodeInfo = FindServiceCodeMappingWithApplicationType(serviceCode);
                                    var statusResp = await _iApplicationMamnagement_Proposed_BuildingPlan_Service.InitiateApplication(ApplicationTypeEnum.BUILDING_PLAN_PROPOSED, formData, userRes.ResponseDataModel.ProjectSiteId, Sys_N_ServiceCodeInfo.ApplicationPurposeType, userRes.ResponseDataModel.UserId, Convert.ToInt64(iPin), investPunjab_AppId, true, latestMasterData.Factory_Master.AppId, serviceCode, latestMasterData.Factory_Master.NAR, (latestMasterData.ServiceData != null && latestMasterData.ServiceData.LicenseNo != null) ? foundClearences.FirstOrDefault().ServiceData.LicenseNo : null, userRes.ResponseDataModel.ProjectSiteVersion == 0 ? 1 : userRes.ResponseDataModel.ProjectSiteVersion);

                                    newAppId = statusResp.AppId;

                                    // Document
                                    List<ProcessImd_LegacyDocumentInfoViewModel> legacyFiles = new List<ProcessImd_LegacyDocumentInfoViewModel>();
                                    legacyFiles.Add(new ProcessImd_LegacyDocumentInfoViewModel()
                                    {
                                        SYS_N_DocId = 20005,
                                        Legacy_FileName = @"images/" + documemts.Where(x => x.DocId == 27).Select(x => x.DocumentFile).FirstOrDefault()
                                    });

                                    legacyFiles.Add(new ProcessImd_LegacyDocumentInfoViewModel()
                                    {
                                        SYS_N_DocId = 50013,
                                        Legacy_FileName = @"images/" + latestMasterData.Factory_Master.OLProof
                                    });

                                    legacyFiles.Add(new ProcessImd_LegacyDocumentInfoViewModel()
                                    {
                                        SYS_N_DocId = 50014,
                                        Legacy_FileName = @"images/" + documemts.Where(x => x.DocId == 72).Select(x => x.DocumentFile).FirstOrDefault()
                                    });

                                    legacyFiles.Add(new ProcessImd_LegacyDocumentInfoViewModel()
                                    {
                                        SYS_N_DocId = 40013,
                                        Legacy_FileName = @"images/" + documemts.Where(x => x.DocId == 73).Select(x => x.DocumentFile).FirstOrDefault()
                                    });

                                    legacyFiles.Add(new ProcessImd_LegacyDocumentInfoViewModel()
                                    {
                                        SYS_N_DocId = 50015,
                                        Legacy_FileName = @"images/" + documemts.Where(x => x.DocId == 168).Select(x => x.DocumentFile).FirstOrDefault()
                                    });

                                    legacyFiles.Add(new ProcessImd_LegacyDocumentInfoViewModel()
                                    {
                                        SYS_N_DocId = 32,
                                        Legacy_FileName = @"images/" + documemts.Where(x => x.DocId == 166).Select(x => x.DocumentFile).FirstOrDefault()
                                    });

                                    legacyFiles.Add(new ProcessImd_LegacyDocumentInfoViewModel()
                                    {
                                        SYS_N_DocId = 50016,
                                        Legacy_FileName = @"images/" + documemts.Where(x => x.DocId == 74).Select(x => x.DocumentFile).FirstOrDefault()
                                    });

                                    await CopyLegacyFiles(newAppId, legacyFiles);
                                }
                                legacyAppId = latestMasterData.Factory_Master.AppId;
                                legacyAppFormId = latestMasterData.Factory_Master.AppFormId;
                                legacyNAR = latestMasterData.Factory_Master.NAR;
                            }



                            var createServiceMigrationResp = await _iSystem_O_CommunicationService.CreateServiceMigrationLog(legacyAppId, legacyAppFormId, legacyNAR);
                            createUserProcessResponse.UserId = userRes.ResponseDataModel.UserId;

                            #region Clearence Files
                            List<LegacyApprovedClearenceMapping> legacyApprovedClearence = new List<LegacyApprovedClearenceMapping>();
                            foreach (var item in foundClearences)
                            {
                                if (item.ServiceData != null && item.ServiceData.IsApproved == 1)
                                {
                                    legacyApprovedClearence.Add(new LegacyApprovedClearenceMapping()
                                    {
                                        Legacy_AppId = item.ServiceData.AppId,
                                        Legacy_AppFormId = item.ServiceData.AppFormId,
                                        Legacy_NAR = item.ServiceData.NAR,
                                        ApprovedOn = item.ServiceData.ApprovedOn,
                                        AttachmentName = @"Licence/" + item.ServiceData.LicenseFile,
                                        Sys_N_AppRefId = newAppId,
                                        LegacyLicenceNo = item.ServiceData.LicenseNo
                                    });
                                }
                            }

                            if (legacyApprovedClearence.Count() > 0)
                            {
                                await CopyLegacyApprovedClearences(legacyApprovedClearence);
                            }

                            #endregion

                            //Logs
                            var latestServiceInProcess = foundClearences.LastOrDefault(x => x.ServiceData != null && x.ServiceData.IsApproved == 0 && x.ServiceLogs.Count() > 0);
                            var application = _context.Applications.Where(x => x.AppId == newAppId && x.IsDeleted == false).FirstOrDefault();
                            if (latestServiceInProcess != null)
                            {
                                string indlUserId = "";
                                Int64 indlProfileId = 0;

                                string indlReceiverUserId = "";
                                Int64 indlReceiverProfileId = 0;

                                ApplicationActionLog applicationActionlogs = null;
                                foreach (var item in latestServiceInProcess.ServiceLogs)
                                {
                                    indlUserId = "";
                                    indlProfileId = 0;

                                    indlReceiverUserId = "";
                                    indlReceiverProfileId = 0;

                                    if (item.SenderRoleId == "592add3e-f992-4983-a8ca-21ddc090bda0")
                                    {
                                        var applicationInfo = await _iApplicationMamnagementService.GetApplicantProfileDetailByAppId(newAppId);
                                        indlUserId = applicationInfo.UserProfileMapping.UserRefId;
                                        indlProfileId = applicationInfo.UserProfileId;
                                    }
                                    else if (item.ReceiverRoleId == "592add3e-f992-4983-a8ca-21ddc090bda0")
                                    {
                                        var applicationInfo = await _iApplicationMamnagementService.GetApplicantProfileDetailByAppId(newAppId);
                                        indlReceiverUserId = applicationInfo.UserProfileMapping.UserRefId;
                                        indlReceiverProfileId = applicationInfo.UserProfileId;
                                    }

                                    applicationActionlogs = new ApplicationActionLog()
                                    {
                                        ActionDate = item.StatusDate,
                                        ActionTakenDaysCount = 0,
                                        ActionTakenHoursCount = 0,
                                        AppActionType = GetSys_N_ActionCode(Convert.ToInt32(item.StatusId)),
                                        ApplicationRefId = newAppId,
                                        Receiver_ProfileRefId = indlReceiverProfileId != 0 ? indlReceiverProfileId : item.ReceiverProfileID,
                                        Receiver_UserRefId = indlReceiverUserId != "" ? indlReceiverUserId : item.ReceiverUserId,
                                        Sender_ProfileRefId = indlProfileId != 0 ? indlProfileId : item.SenderProfileID,
                                        Sender_UserRefId = indlUserId != "" ? indlUserId : item.SenderUserId,
                                        Remarks = item.StatusDesc,
                                        ReceiverRoleId = item.ReceiverRoleId,
                                        SenderRoleId = item.SenderRoleId,
                                        AppDocumentRefId = 0,
                                        IsDocumentUploaded = false,
                                        Legacy_IsMigrated = true,
                                        Legacy_AppId = item.AppId,
                                        Legacy_AppFormId = item.AppFormId,
                                        Legacy_NAR = item.NAR,
                                        Legacy_StatusId = Convert.ToInt32(item.StatusId)
                                    };

                                    await _context.ApplicationActionLogs.AddAsync(applicationActionlogs);
                                    await _context.SaveChangesAsync();
                                }

                                if (applicationActionlogs != null)
                                {
                                    ApplicationAction applicationAction = new ApplicationAction()
                                    {
                                        ActionDate = applicationActionlogs.ActionDate,
                                        ActionTakenDaysCount = 0,
                                        ActionTakenHoursCount = 0,
                                        AppActionType = applicationActionlogs.AppActionType,
                                        ApplicationRefId = newAppId,
                                        Receiver_ProfileRefId = applicationActionlogs.Receiver_ProfileRefId,
                                        Receiver_UserRefId = applicationActionlogs.Receiver_UserRefId,
                                        Sender_ProfileRefId = applicationActionlogs.Sender_ProfileRefId,
                                        Sender_UserRefId = applicationActionlogs.Sender_UserRefId,
                                        Remarks = applicationActionlogs.Remarks,
                                        ReceiverRoleId = applicationActionlogs.ReceiverRoleId,
                                        SenderRoleId = applicationActionlogs.SenderRoleId,
                                        AppDocumentRefId = 0,
                                        IsDocumentUploaded = false,
                                        Legacy_IsMigrated = true,
                                        Legacy_AppId = applicationActionlogs.Legacy_AppId,
                                        Legacy_AppFormId = applicationActionlogs.Legacy_AppFormId,
                                        Legacy_NAR = applicationActionlogs.Legacy_NAR,
                                        Legacy_StatusId = applicationActionlogs.Legacy_StatusId
                                    };

                                    await _context.ApplicationActions.AddAsync(applicationAction);
                                    await _context.SaveChangesAsync();
                                }
                            }

                            else
                            {
                                //var clearences = foundClearences.Select(x => x.ServiceLogs[0]).FirstOrDefault();
                                var clearences = foundClearences.FirstOrDefault();
                                var applicationInfo = await _iApplicationMamnagementService.GetApplicantProfileDetailByAppId(newAppId);
                                var indlReceiverUserId = applicationInfo.UserProfileMapping.UserRefId;
                                var indlReceiverProfileId = applicationInfo.UserProfileId;

                                var userDetails = await _iAuthService.GetUserRoleAndProfileDetailByUserId(userRes.ResponseDataModel.UserId);

                                if (logsAlsoToBeSeeded == true)
                                {
                                    var senderUserRefId = foundClearences.FirstOrDefault().ServiceLogs.LastOrDefault();
                                    var senderProfileDetails = await _iAuthService.GetUserProfileByUserRefId(senderUserRefId.SenderUserId);

                                    //var applogs = _context.ApplicationActionLogs.Where(x => x.ApplicationRefId == newAppId).ToList();
                                    //if (applogs.Any())
                                    //{
                                    //    _context.ApplicationActionLogs.RemoveRange(applogs);
                                    //    _context.SaveChanges();
                                    //}

                                    var appactions = _context.ApplicationActions.Where(x => x.ApplicationRefId == newAppId).FirstOrDefault();
                                    //if (appactions.Any())
                                    //{
                                    //    _context.ApplicationActions.RemoveRange(appactions);
                                    //    _context.SaveChanges();
                                    //}
                                    List<ApplicationActionLog> actionLogList = new List<ApplicationActionLog>();
                                    foreach (var item in foundClearences.FirstOrDefault().ServiceLogs)
                                    {
                                        ApplicationActionLog actionLog = new ApplicationActionLog();
                                        actionLog.ActionDate = item.StatusDate;
                                        actionLog.ActionTakenDaysCount = 0;
                                        actionLog.ActionTakenHoursCount = 0;

                                        actionLog.AppActionType = GetSys_N_ActionCode(Convert.ToInt32(item.StatusId));

                                        actionLog.ApplicationRefId = newAppId;

                                        actionLog.Receiver_ProfileRefId = userDetails == null ? 404 : userDetails.UserProfileId;
                                        actionLog.Receiver_UserRefId = userDetails.UserId;

                                        actionLog.Sender_ProfileRefId = senderProfileDetails?.UserProfileRefId ?? 404;
                                        actionLog.Sender_UserRefId = item.SenderUserId;

                                        actionLog.Remarks = "objection in form";
                                        actionLog.ReceiverRoleId = item.ReceiverRoleId == null ? userDetails.RoleId : item.ReceiverRoleId;
                                        actionLog.SenderRoleId = item.SenderRoleId == null ? userDetails.RoleId : item.SenderRoleId;
                                        actionLog.AppDocumentRefId = 0;
                                        actionLog.IsDocumentUploaded = false;
                                        actionLog.Legacy_IsMigrated = true;
                                        actionLog.Legacy_AppId = item.AppId;
                                        actionLog.Legacy_AppFormId = item.AppFormId;
                                        actionLog.Legacy_NAR = item.NAR;
                                        actionLog.Legacy_StatusId = Convert.ToInt32(item.StatusId);
                                        actionLog.IpAddress = "Manual";
                                        actionLog.Latitude = "Manual";
                                        actionLog.Longitude = "Manual";
                                        actionLogList.Add(actionLog);


                                    }
                                    await _context.BulkInsertAsync<ApplicationActionLog>(actionLogList);


                                    appactions.ActionDate = actionLogList.LastOrDefault().ActionDate;
                                        appactions.ActionTakenDaysCount = 0;
                                        appactions.ActionTakenHoursCount = 0;
                                        appactions.AppActionType = actionLogList.LastOrDefault().AppActionType; // GetSys_N_ActionCode(Convert.ToInt32(1));
                                        appactions.ApplicationRefId = actionLogList.LastOrDefault().ApplicationRefId;

                                        appactions.Receiver_ProfileRefId = userDetails == null ? 404 : userDetails.UserProfileId;
                                        appactions.Receiver_UserRefId = userDetails.UserId;

                                        appactions.Sender_ProfileRefId = actionLogList.LastOrDefault().Sender_ProfileRefId;
                                        appactions.Sender_UserRefId = actionLogList.LastOrDefault().Sender_UserRefId;

                                        appactions.Remarks = "Objection in Form";
                                        appactions.ReceiverRoleId = actionLogList.LastOrDefault().ReceiverRoleId;
                                        appactions.SenderRoleId = actionLogList.LastOrDefault().SenderRoleId;
                                        appactions.AppDocumentRefId = 0;
                                        appactions.IsDocumentUploaded = false;
                                        appactions.Legacy_IsMigrated = true;
                                        appactions.Legacy_AppId = actionLogList.LastOrDefault().Legacy_AppId;
                                        appactions.Legacy_AppFormId = actionLogList.LastOrDefault().Legacy_AppFormId;
                                        appactions.Legacy_NAR = actionLogList.LastOrDefault().Legacy_NAR;
                                        appactions.Legacy_StatusId = actionLogList.LastOrDefault().Legacy_StatusId;
                                        appactions.IpAddress = "Manual";
                                        appactions.Latitude = "Manual";
                                    appactions.Longitude = "Manual";
                                    
                                     _context.ApplicationActions.Update(appactions);
                                    await _context.SaveChangesAsync();

                                    // update application life cycle
                                    var appDetails = _context.Applications.Where(x => x.AppId == appactions.ApplicationRefId && x.IsDeleted == false).FirstOrDefault();
                                    appDetails.ApplicationLifeCycleStatusType = ApplicationLifeCycleStatusTypeEnum.IN_OBJECTION;
                                    appDetails.ApplicationLifeCycleLastStatusOn = DateTime.Now;
                                    _context.Update<Application>(appDetails);

                                    // Insert into AppFee Details

                                    AppFeeDetail appFeeDetail = new AppFeeDetail()
                                    {
                                        AppRefId = appactions.ApplicationRefId,
                                        FeeHeaderRefId = 3,
                                        Amount = foundClearences.FirstOrDefault().ClearencesFee.FirstOrDefault().TransactionAmount,
                                        CalculatedOn = foundClearences.FirstOrDefault().ClearencesFee.FirstOrDefault().TransactionDate,
                                        IsDeduductible = false,
                                        PaymentBatchCounter = 1,
                                        PaymentPartCounter = 0,
                                        HasDedicatedTreasuryCode = false,
                                        DedicatedTreasurCode = null,
                                        DedicatedDDOCode = null,
                                        Description = null,
                                    };
                                    await _context.AppFeeDetails.AddAsync(appFeeDetail);
                                    await _context.SaveChangesAsync();

                                    //_iGR_AppFeeDetail

                                    //Step - 4. Insert into AppFeeTransaction
                                    AppFeeTransaction appFeeTransaction = new AppFeeTransaction()
                                    {
                                        TransactionInitializationDate = foundClearences.FirstOrDefault().ClearencesFee.FirstOrDefault().PaymentDate,
                                        PaymentGatewayType = PaymentGatewayTypeEnum.IFMS,
                                        PaymentModeType = PaymentModeTypeEnum.ONLINE,
                                        PaymentTreasuryType = PaymentTreasuryTypeEnum.TREASURY,
                                        PaymentGatewayTargetUrl = "NA",
                                        PaymentGatewayApiMethodType = PaymentGatewayApiMethodTypeEnum.POST,
                                        UniquePaymentGatewayTransactionId = foundClearences.FirstOrDefault().ClearencesFee.FirstOrDefault().ME_TXN_REF_NO,
                                        IsWebRequestCycleCompleted = true,
                                        RequestBodyData = foundClearences.FirstOrDefault().ClearencesFee.FirstOrDefault().ReqMsg,
                                        ResponseBodyData = foundClearences.FirstOrDefault().ClearencesFee.FirstOrDefault().ResMsg,
                                        ResponseReceivedOn = foundClearences.FirstOrDefault().ClearencesFee.FirstOrDefault().PaymentDate,
                                        ResponseMessage = foundClearences.FirstOrDefault().ClearencesFee.FirstOrDefault().ResMsg,
                                        TransactionFinalStatusType = TransactionFinalStatusTypeEnum.SUCCEED,
                                        AmountCalculated = foundClearences.FirstOrDefault().ClearencesFee.FirstOrDefault().TransactionAmount,
                                        PaymentBatchCounter = 1,
                                        PaymentPartCounter = 1,
                                        BankTransactionRefNumber1 = foundClearences.FirstOrDefault().ClearencesFee.FirstOrDefault().BANK_REFERENCE_NO,
                                        BankTransactionRefNumber1_Type = BankTransactionReferenceTypeEnum.BANK_REF_NUMBER,
                                        BankTransactionRefNumber2 = foundClearences.FirstOrDefault().ClearencesFee.FirstOrDefault().BANK_REFERENCE_NO,
                                        BankTransactionRefNumber2_Type = BankTransactionReferenceTypeEnum.BANK_REF_NUMBER,
                                        BankSettlementOn = foundClearences.FirstOrDefault().ClearencesFee.FirstOrDefault().PaymentDate,
                                        AppRefId = appactions.ApplicationRefId
                                    };
                                    await _context.AppFeeTransactions.AddAsync(appFeeTransaction);
                                    await _context.SaveChangesAsync();

                                    //step - 5. Insert into SuccessMapping Table
                                    AppPaymentSuccessTransactionMapping appPaymentSuccessTransactionMapping = new AppPaymentSuccessTransactionMapping()
                                    {
                                        AppRefId = appactions.ApplicationRefId,
                                        AppFeeTransactionRefId = appFeeTransaction.AppFeeTransactionId,
                                        PaymentBatchCounter = 1,
                                        PaymentPartCounter = 1,
                                    };
                                    await _context.AppPaymentSuccessTransactionMappings.AddAsync(appPaymentSuccessTransactionMapping);
                                    await _context.SaveChangesAsync();
                                }
                                else
                                {
                                    if (!_context.ApplicationActions.Any(x => x.ApplicationRefId == newAppId))
                                    {
                                        ApplicationAction action = new ApplicationAction()
                                        {
                                            ActionDate = DateTime.Now,
                                            ActionTakenDaysCount = 0,
                                            ActionTakenHoursCount = 0,
                                            AppActionType = (int)AppActionTypeEnum.APP_SAVE_DRAFT, // GetSys_N_ActionCode(Convert.ToInt32(1)),
                                            ApplicationRefId = newAppId,

                                            Receiver_ProfileRefId = userDetails.UserProfileId,
                                            Receiver_UserRefId = userDetails.UserId,

                                            Sender_ProfileRefId = userDetails.UserProfileId,
                                            Sender_UserRefId = userDetails.UserId,

                                            Remarks = "Application saved as draft",
                                            ReceiverRoleId = userDetails.RoleId,
                                            SenderRoleId = userDetails.RoleId,
                                            AppDocumentRefId = 0,
                                            IsDocumentUploaded = false,
                                            Legacy_IsMigrated = true,
                                            Legacy_AppId = clearences.ServiceData.AppId,
                                            Legacy_AppFormId = serviceCode,
                                            Legacy_NAR = null,
                                            Legacy_StatusId = Convert.ToInt32(1),
                                            IpAddress = "Manual",
                                            Latitude = "Manual",
                                            Longitude = "Manual"
                                        };

                                        ApplicationActionLog actionLog = new ApplicationActionLog()
                                        {
                                            ActionDate = action.ActionDate,
                                            ActionTakenDaysCount = 0,
                                            ActionTakenHoursCount = 0,

                                            AppActionType = action.AppActionType,

                                            ApplicationRefId = newAppId,

                                            Receiver_ProfileRefId = action.Receiver_ProfileRefId,
                                            Receiver_UserRefId = action.Receiver_UserRefId,

                                            Sender_ProfileRefId = action.Sender_ProfileRefId,
                                            Sender_UserRefId = action.Sender_UserRefId,

                                            Remarks = action.Remarks,
                                            ReceiverRoleId = action.ReceiverRoleId,
                                            SenderRoleId = action.SenderRoleId,
                                            AppDocumentRefId = 0,
                                            IsDocumentUploaded = false,
                                            Legacy_IsMigrated = true,
                                            Legacy_AppId = action.Legacy_AppId,
                                            Legacy_AppFormId = action.Legacy_AppFormId,
                                            Legacy_NAR = action.Legacy_NAR,
                                            Legacy_StatusId = action.Legacy_StatusId,
                                            IpAddress = "Manual",
                                            Latitude = "Manual",
                                            Longitude = "Manual"
                                        };

                                        await _context.ApplicationActions.AddAsync(action);
                                        await _context.SaveChangesAsync();

                                        await _context.ApplicationActionLogs.AddAsync(actionLog);
                                        await _context.SaveChangesAsync();
                                    }
                                }
                            }

                            // Share status to Invest Punjab Portal
                            await ShareStatusToBusinessFirst(newAppId, application.ApplicationType, AppActionTypeEnum.APP_SAVE_DRAFT);
                            serviceGatewayResponse.ResponseDataModel = new ImportAndSeedDataResponseViewModel();
                            serviceGatewayResponse.ResponseDataModel.AppRefId = newAppId;
                            serviceGatewayResponse.ResponseDataModel.UserRefId = createUserProcessResponse.UserId;
                        }
                    }
                    else
                    {
                        serviceGatewayResponse.HasError = true;
                        serviceGatewayResponse.ErrorDesc = "Issue in applying service..! (Process-IMD, Error Code: 101)";
                    }

                }
            }
            catch (Exception ex)
            {
                serviceGatewayResponse.HasError = true;
                serviceGatewayResponse.ErrorDesc = ex.Message;
                throw ex;
            }
            return serviceGatewayResponse;
        }

        public async Task<GenericResponseTemplateModel<bool>> SeedMasterData_Shop(Int64 appRefId, ProcessImd_ClearencesShopLicenceViewModel data)
        {
            GenericResponseTemplateModel<bool> serviceGatewayResponse = new GenericResponseTemplateModel<bool>() { ErrorDesc = null, HasError = false, ResponseDataModel = true };

            try
            {
                ShopLicence_GeneralDetail shopLicence_GeneralDetail = new ShopLicence_GeneralDetail()
                {
                    ClosingDay = ClosingDayTypeEnum.SUNDAY,
                    OpeningHoursOfEstablishment = data.OHours,
                    ClosingHoursOfEstablishment = data.CHours,
                    OwnerName = data.OName,
                    OwnerFatherOrHusbandName = data.OFHName,
                    AadharNumber = data.AadharNo,
                    EstablishmentConstitutionType = EstablishmentConstitutionTypeEnum.PROPRIETORSHIP,
                    ManagerName = data.MgrName,
                    ShopType = ShopTypeEnum.SHOP,
                    NationalIndustrialClassificationCode = data.NICCode,
                    IsHavingEmployee = Convert.ToInt32(data.HasEmp),
                    AppRefId = appRefId
                };
                await _context.AddAsync<ShopLicence_GeneralDetail>(shopLicence_GeneralDetail);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                serviceGatewayResponse.HasError = true;
                serviceGatewayResponse.ErrorDesc = ex.Message;
                throw ex;
            }
            return serviceGatewayResponse;
        }

        public async Task<GenericResponseTemplateModel<bool>> CopyLegacyFiles(Int64 AppId, List<ProcessImd_LegacyDocumentInfoViewModel> legacyFiles)
        {
            GenericResponseTemplateModel<bool> serviceGatewayResponse = new GenericResponseTemplateModel<bool>() { ErrorDesc = null, HasError = false, ResponseDataModel = true };

            try
            {
                string legacySourceDocumentPath = Configuration.GetSection("ThirdPartyIntegrationConfigs").GetSection("ProcessImd_Configs").GetSection("legacySourceDocumentPath").Value;
                string targetAppDocPath = Configuration.GetSection("ThirdPartyIntegrationConfigs").GetSection("ProcessImd_Configs").GetSection("TargetAppDocPath").Value;
                ApplicationDocument appDocument;
                foreach (var item in legacyFiles)
                {
                    if (item.Legacy_FileName != null && item.Legacy_FileName.Trim().Length > 0)
                    {
                        if (File.Exists(legacySourceDocumentPath + item.Legacy_FileName))
                        {
                            appDocument = new ApplicationDocument()
                            {
                                AppRefId = AppId,
                                AttachmentName = item.Legacy_FileName,
                                DocumentRefId = item.SYS_N_DocId,
                                IsUploaded = true,
                                LastModifiedDate = DateTime.Now,
                                Uploadeddate = DateTime.Now,
                                IsLocked = true
                            };
                            _iGR_AppDocument.Insert(appDocument);
                            await _iGR_AppDocument.SavechangeAsync();

                            var filename = item.Legacy_FileName.Substring(item.Legacy_FileName.LastIndexOf('.'));
                            filename = AppId + "_" + item.SYS_N_DocId.ToString() + "_" + appDocument.AppDocId + filename;
                            var foundDocument = _iGR_AppDocument.GetById(appDocument.AppDocId);
                            foundDocument.AttachmentName = filename;

                            _iGR_AppDocument.Update(foundDocument);
                            await _iGR_AppDocument.SavechangeAsync();

                            File.Copy(legacySourceDocumentPath + item.Legacy_FileName, targetAppDocPath + filename, true);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                serviceGatewayResponse.HasError = true;
                serviceGatewayResponse.ErrorDesc = ex.Message;
                throw ex;
            }
            return serviceGatewayResponse;
        }

        public async Task<GenericResponseTemplateModel<bool>> CopyLegacyApprovedClearences(List<LegacyApprovedClearenceMapping> legacyFiles)
        {
            GenericResponseTemplateModel<bool> serviceGatewayResponse = new GenericResponseTemplateModel<bool>() { ErrorDesc = null, HasError = false, ResponseDataModel = true };

            try
            {
                string legacySourceDocumentPath = Configuration.GetSection("ThirdPartyIntegrationConfigs").GetSection("ProcessImd_Configs").GetSection("legacySourceDocumentPath").Value;
                string targetLegacyLicencePath = Configuration.GetSection("ThirdPartyIntegrationConfigs").GetSection("ProcessImd_Configs").GetSection("TargetLegacyLicencePath").Value;
                //ApplicationDocument appDocument;
                string oldFileName = "";
                int index = 0;
                foreach (var item in legacyFiles)
                {
                    if (item.AttachmentName != null && item.AttachmentName.Trim().Length > 0)
                    {
                        oldFileName = item.AttachmentName;

                        if (File.Exists(legacySourceDocumentPath + oldFileName))
                        {
                            var filename = item.Sys_N_AppRefId.ToString() + "_" + item.Legacy_AppId + "_" + item.Legacy_AppFormId + "_" + item.Legacy_NAR + oldFileName.Substring(oldFileName.LastIndexOf('.'));
                            legacyFiles[index].AttachmentName = filename;
                            _context.LegacyApprovedClearenceMappings.Add(item);
                            await _context.SaveChangesAsync();

                            File.Copy(legacySourceDocumentPath + oldFileName, targetLegacyLicencePath + filename, true);
                        }
                    }
                    index++;
                }
            }
            catch (Exception ex)
            {
                serviceGatewayResponse.HasError = true;
                serviceGatewayResponse.ErrorDesc = ex.Message;
                throw ex;
            }
            return serviceGatewayResponse;
        }

        public async Task<GenericResponseTemplateModel<PrincipalApproval_RBA_DetailsViewModel>> GetPrincipalApprovalUnderRBAByIPin(string iPin, string appId)
        {
            GenericResponseTemplateModel<PrincipalApproval_RBA_DetailsViewModel> genericResponseTemplateModel = new GenericResponseTemplateModel<PrincipalApproval_RBA_DetailsViewModel>() { HasError = false, ErrorDesc = "" };
            try
            {
                Int64 result;
                if (Int64.TryParse(iPin, out result) && Int64.TryParse(appId, out result))
                {
                    string url = Configuration.GetSection("ThirdPartyIntegrationConfigs").GetSection("InvestPunjab").GetSection("GetBusinessFirstTokenUrl").Value;
                    InvestPunjabApiTokenViewModel investPunjabApiToken = null;
                    using (var httpClient = new HttpClient())
                    {
                        var requestData = new
                        {
                            IntegrationKey = "UAT_LABOUR"
                        };

                        StringContent content = new StringContent(JsonConvert.SerializeObject(requestData), Encoding.UTF8, "application/json");

                        using (var response = await httpClient.PostAsync(url, content))
                        {
                            string apiResponse = await response.Content.ReadAsStringAsync();
                            investPunjabApiToken = JsonConvert.DeserializeObject<InvestPunjabApiTokenViewModel>(apiResponse);
                        }
                    }

                    if (investPunjabApiToken != null && investPunjabApiToken.Token != null)
                    {
                        url = Configuration.GetSection("ThirdPartyIntegrationConfigs").GetSection("InvestPunjab").GetSection("GetPrincipalApprovalDataUrl").Value;
                        using (var httpClient = new HttpClient())
                        {
                            //httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Authorization", investPunjabApiToken.Token);
                            httpClient.DefaultRequestHeaders.Add("Authorization", investPunjabApiToken.Token);
                            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                            //httpClient.DefaultRequestHeaders
                            var requestData = new
                            {
                                iPin = iPin,
                                AppId = appId
                            };

                            StringContent content = new StringContent(JsonConvert.SerializeObject(requestData), Encoding.UTF8, "application/json");

                            using (var response = await httpClient.PostAsync(url, content))
                            {
                                string apiResponse = await response.Content.ReadAsStringAsync();
                                genericResponseTemplateModel.ResponseDataModel = JsonConvert.DeserializeObject<PrincipalApproval_RBA_DetailsViewModel>(apiResponse);
                            }
                        }
                    }
                }
                else
                {
                    genericResponseTemplateModel.ResponseDataModel = new PrincipalApproval_RBA_DetailsViewModel()
                    {
                        success = false,
                        data = null
                    };
                }
            }
            catch (Exception ex)
            {

                genericResponseTemplateModel.HasError = true;
                genericResponseTemplateModel.ErrorDesc = ex.Message;
                throw ex;
            }
            return genericResponseTemplateModel;
        }

        public int GetSys_N_ActionCode(int legacyStatusCode)
        {
            if (legacyStatusCode == 5 || legacyStatusCode == 14 || legacyStatusCode == 6)
            {
                legacyStatusCode = 200;
            }
            else if (legacyStatusCode == 9)
            {
                legacyStatusCode = 404;
            }
            else if (legacyStatusCode == 15)
            {
                legacyStatusCode = 500;
            }
            else if (legacyStatusCode == 20)
            {
                legacyStatusCode = 3;
            }
            else if (legacyStatusCode == 32)
            {
                legacyStatusCode = 102;
            }
            else if (legacyStatusCode == 81 || legacyStatusCode == 31 || legacyStatusCode == 13 || legacyStatusCode == 8 || legacyStatusCode == 12)
            {
                legacyStatusCode = 103;
            }
            else if (legacyStatusCode == 10 || legacyStatusCode == 202 || legacyStatusCode == 19 || legacyStatusCode == 11)
            {
                legacyStatusCode = 6;
            }
            else if (legacyStatusCode == 4)
            {
                legacyStatusCode = 405;
            }
            else if (legacyStatusCode == 119  ||  legacyStatusCode == 206)
            {
                legacyStatusCode = 206;
            }
            else if (legacyStatusCode == 201)
            {
                legacyStatusCode = 402;
            }
            else if (legacyStatusCode == 16)
            {
                legacyStatusCode = 202;
            }
            else if (legacyStatusCode == 118)
            {
                legacyStatusCode = 205;
            }


            return legacyStatusCode;
        }


        public int GetSys_N_Document_Id(int legacyStatusCode)
        {
            if (legacyStatusCode == 99)
            {
                legacyStatusCode = 60037;
            }
            else if (legacyStatusCode == 100)
            {
                legacyStatusCode = 60038;
            }
            else if (legacyStatusCode == 103)
            {
                legacyStatusCode = 60039;
            }
            else if (legacyStatusCode == 101)
            {
                legacyStatusCode = 60040;
            }
            else if (legacyStatusCode == 104)
            {
                legacyStatusCode = 60041;
            }
            else if (legacyStatusCode == 102)
            {
                legacyStatusCode = 60042;
            }
            else if (legacyStatusCode == 107)
            {
                legacyStatusCode = 60036;
            }

            else if (legacyStatusCode == 108)
            {
                legacyStatusCode = 60043;
            }
            else if (legacyStatusCode == 130)
            {
                legacyStatusCode = 60044;
            }
            else if (legacyStatusCode == 131)
            {
                legacyStatusCode = 60045;
            }
            else if (legacyStatusCode == 132)
            {
                legacyStatusCode = 60046;
            }


            return legacyStatusCode;
        }

        public async Task<GenericResponseTemplateModel<bool>> IsServiceIsInObjection(Int64 iPin, ApplicationTypeEnum applicationType, ApplicationPurposeTypeEnum applicationPurposeType)
        {
            GenericResponseTemplateModel<bool> genericServiceResultTemplate = new GenericResponseTemplateModel<bool>()
            {
                HasError = false,
                ErrorDesc = "",
                ResponseDataModel = false
            };
            try
            {
                var nativeAppId = _context.BusinessFirst_RequestLogs.Where(x => x.IPin == iPin && x.NativeAppId != 0).Select(x => x.NativeAppId).ToArray();
                if (nativeAppId.Length == 0)
                {
                    genericServiceResultTemplate.ResponseDataModel = false;
                }
                else
                {
                    genericServiceResultTemplate.ResponseDataModel = _context.Applications.Any(x => nativeAppId.Contains(x.AppId) && x.IsDeleted == false && x.ApplicationType == applicationType && x.ApplicationPurposeType == applicationPurposeType && (x.ApplicationLifeCycleStatusType == ApplicationLifeCycleStatusTypeEnum.IN_OBJECTION));

                }
            }
            catch (Exception ex)
            {
                genericServiceResultTemplate.HasError = true;
                genericServiceResultTemplate.ErrorDesc = ex.Message;
                throw ex;
            }
            return genericServiceResultTemplate;
        }

        public async Task<GenericResponseTemplateModel<string>> VerifyOldLicenceNo(string licenceSnapshot)
        {
            GenericResponseTemplateModel<string> genericServiceResultTemplate = new GenericResponseTemplateModel<string>();
            try
            {
                byte[] data = Convert.FromBase64String(licenceSnapshot);
                string LicenceNo = Encoding.UTF8.GetString(data);

                // Verify Licence No in SYS-N
                var isVerified = _context.ApplicationLicenceNoMapping.Where(x => x.LicenceNumber == LicenceNo).FirstOrDefault();
                if (isVerified != null)
                {
                    genericServiceResultTemplate.HasError = true;
                    genericServiceResultTemplate.ErrorDesc = "You have already applied and availed the licence in another account..!";
                }
                else // Call SYS-O
                {
                    var isFound = await _iDapperRepository.Get<Int64>("SELECT LicenceNo FROM AppClearanceIssueds WHERE LicenceNo=CONVERT(VARCHAR, 'LicenceNo')", new { LicenceNo = LicenceNo }, CommandType.Text).ConfigureAwait(false);
                    if (isFound.Count() > 0)
                    {
                        genericServiceResultTemplate.ResponseDataModel = "";
                        await _iNotificationManagerService.SendSmsViaBOCWBoard("8858888811", "Hello, This is for test otp service..!");
                    }
                }

                // Send OTP
                //await _iNotificationManagerService.SendOtpViaBOCWBoard("8858888811","Hello, This is for test otp service..!");
            }
            catch (Exception ex)
            {
                genericServiceResultTemplate.HasError = true;
                genericServiceResultTemplate.ErrorDesc = ex.Message;
                throw ex;
            }

            return genericServiceResultTemplate;
        }

        public async Task<GenericResponseTemplateModel<BuildingPlanHUD_RTB_Mapping>> GetInPrincipalApprovalByAppRefId(Int64 appRefId)
        {
            GenericResponseTemplateModel<BuildingPlanHUD_RTB_Mapping> genericResponseTemplateModel = new GenericResponseTemplateModel<BuildingPlanHUD_RTB_Mapping>() { HasError = false, ErrorDesc = "" };
            try
            {
                var parentWithChildObject = await _iGR_BuildingPlanHUD_RTB_Mapping.GetAsync(x => x.AppRefId == appRefId).ConfigureAwait(false);
                genericResponseTemplateModel.ResponseDataModel = parentWithChildObject.FirstOrDefault();
            }
            catch (Exception ex)
            {
                genericResponseTemplateModel.HasError = true;
                genericResponseTemplateModel.ErrorDesc = ex.Message;
                throw ex;
            }

            return genericResponseTemplateModel;
        }

        public async Task<GenericResponseTemplateModel<bool>> HasShareStatusToBusinessFirst(Int64 nativeAppId)
        {
            GenericResponseTemplateModel<bool> genericServiceResultTemplate = new GenericResponseTemplateModel<bool>()
            {
                HasError = false,
                ErrorDesc = "",
                ResponseDataModel = true
            };
            try
            {
                var parentWithChildObject = await _iGR_BusinessFirstShareStatusLog.GetAsync(x => x.AppRefId == nativeAppId && x.IsRequestCompeleted == true).ConfigureAwait(false);
                if (parentWithChildObject.Count() > 0)
                {
                    genericServiceResultTemplate.ResponseDataModel = true;
                }
                else
                {
                    genericServiceResultTemplate.ResponseDataModel = false;
                }
            }
            catch (Exception ex)
            {
                genericServiceResultTemplate.HasError = true;
                genericServiceResultTemplate.ErrorDesc = ex.Message;
                throw ex;
            }
            return genericServiceResultTemplate;
        }
        public async Task<GenericResponseTemplateModel<bool>>  VerifyLicenseNumber(string value)
        {
            GenericResponseTemplateModel<bool> genericResponseTemplateModel = new GenericResponseTemplateModel<bool>() { HasError = false, ErrorDesc = "", ResponseDataModel = true };
            try
            {
                genericResponseTemplateModel.ResponseDataModel = _context.ApplicationLicenceNoMapping.Any(x => x.LicenceNumber.ToLower() == value.Trim().ToLower());
            }
            catch (Exception ex)
            {
                genericResponseTemplateModel.HasError = true;
                genericResponseTemplateModel.ErrorDesc = ex.Message;
                throw ex;
            }

            return genericResponseTemplateModel;
        }

        public async Task<GenericResponseTemplateModel<int>> GetRaisedFeePaymentPageCode(Int64 appRefId)
        {
            GenericResponseTemplateModel<int> genericResponseTemplateModel = new GenericResponseTemplateModel<int>() { HasError = false, ErrorDesc = "" };
            try
            {
                var appliation = _context.Applications.Where(x => x.AppId == appRefId && x.IsDeleted == false).FirstOrDefault();
                var paymentParts = _context.AppPaymentParts.Where(x => x.AppRefId == appRefId && x.PaymentBatchCounter == appliation.PaymentBatchCounter).ToList();
                if (paymentParts.Count() == 0)
                {
                    genericResponseTemplateModel.ResponseDataModel = 1;
                }
                else if (paymentParts.Count() > 0 && paymentParts.Any(x => x.TransactionFinalStatusType == TransactionFinalStatusTypeEnum.SUCCEED))
                {
                    genericResponseTemplateModel.ResponseDataModel = 2;
                }
                //else
                //{
                //    genericResponseTemplateModel.ResponseDataModel = 2;
                //}
            }
            catch (Exception ex)
            {
                genericResponseTemplateModel.HasError = true;
                genericResponseTemplateModel.ErrorDesc = ex.Message;
                throw ex;
            }

            return genericResponseTemplateModel;
        }

        public async Task<GenericResponseTemplateModel<Application>> DoesIPinBelongToSYS_N(Int64 iPin, string investPunjab_AppId, ApplicationTypeEnum applicationType, ApplicationPurposeTypeEnum applicationPurposeType)
        {
            GenericResponseTemplateModel<Application> genericResponseTemplateModel = new GenericResponseTemplateModel<Application>() { HasError = false, ErrorDesc = "", ResponseDataModel = null };
            try
            {
                genericResponseTemplateModel.ResponseDataModel = _context.Applications.Where(x => x.InvestPunjab_Ipin == iPin.ToString() && x.IsDeleted == false && x.InvestPunjab_AppId == Convert.ToInt64(investPunjab_AppId) && x.ApplicationType == (ApplicationTypeEnum)applicationType && x.ApplicationPurposeType == (ApplicationPurposeTypeEnum)applicationPurposeType).FirstOrDefault();
            }
            catch (Exception ex)
            {
                genericResponseTemplateModel.HasError = true;
                genericResponseTemplateModel.ErrorDesc = ex.Message;
                throw ex;
            }

            return genericResponseTemplateModel;
        }

        public async Task<GenericResponseTemplateModel<Application>> DoesIPinHasGrantApproval(Int64 iPin, ApplicationTypeEnum applicationType, ApplicationPurposeTypeEnum applicationPurposeType)
        {
            GenericResponseTemplateModel<Application> genericServiceResultTemplate = new GenericResponseTemplateModel<Application>()
            {
                HasError = false,
                ErrorDesc = "",
                ResponseDataModel = null
            };
            try
            {
                //genericServiceResultTemplate.ResponseDataModel = _context.Applications.Where(x => x.InvestPunjab_Ipin == iPin.ToString() && x.ApplicationPurposeType == applicationPurposeType).FirstOrDefault();
                //genericServiceResultTemplate.ResponseDataModel = _context.Applications.Where(x => x.InvestPunjab_Ipin == iPin.ToString() && x.ApplicationType == applicationType && x.ApplicationPurposeType == applicationPurposeType && (x.ApplicationLifeCycleStatusType == ApplicationLifeCycleStatusTypeEnum.APPROVED || x.ApplicationLifeCycleStatusType == ApplicationLifeCycleStatusTypeEnum.REJECTED)).FirstOrDefault();

                genericServiceResultTemplate.ResponseDataModel = _context.Applications.Where(x => x.InvestPunjab_Ipin == iPin.ToString() 
                && x.IsDeleted == false 
                && x.ApplicationType == applicationType 
                && x.ApplicationPurposeType == applicationPurposeType 
                && (x.ApplicationLifeCycleStatusType == ApplicationLifeCycleStatusTypeEnum.APPROVED 
                || x.ApplicationLifeCycleStatusType == ApplicationLifeCycleStatusTypeEnum.REJECTED 
                || x.ApplicationLifeCycleStatusType == ApplicationLifeCycleStatusTypeEnum.WITHDRAW_APPLICATION
                || x.ApplicationLifeCycleStatusType == ApplicationLifeCycleStatusTypeEnum.APPLICATION_DECLINED
                || x.ApplicationLifeCycleStatusType == ApplicationLifeCycleStatusTypeEnum.ACKNOWLEDGMENT_SUBMITTED_COMPLETED)).FirstOrDefault();
            }
            catch (Exception ex)
            {
                genericServiceResultTemplate.HasError = true;
                genericServiceResultTemplate.ErrorDesc = ex.Message;
                throw ex;
            }
            return genericServiceResultTemplate;
        }

        public async Task<GenericResponseTemplateModel<Factory_TemporaryLicenceDetailsViewModel>> GetTemporaryLicenseDetails(string tempRegistrationNumber)
        {
            GenericResponseTemplateModel<Factory_TemporaryLicenceDetailsViewModel> genericServiceResultTemplate = new GenericResponseTemplateModel<Factory_TemporaryLicenceDetailsViewModel>();
            try
            {
                var licence = await _iDapperRepository.Get<Factory_TemporaryLicenceDetailsViewModel>("[dbo].[sp_GetTemporaryLicenceDetails_MasterData]", new { TempRegistrationNumber = tempRegistrationNumber }, CommandType.StoredProcedure).ConfigureAwait(false);
                genericServiceResultTemplate.ResponseDataModel = licence.FirstOrDefault();
            }
            catch (Exception ex)
            {
                genericServiceResultTemplate.HasError = true;
                genericServiceResultTemplate.ErrorDesc = ex.Message;
                throw ex;
            }
            return genericServiceResultTemplate;
        }

        public async Task<GenericResponseTemplateModel<string>> GetDOFLicenseDetails(string dofNumber)
        {
            GenericResponseTemplateModel<string> genericServiceResultTemplate = new GenericResponseTemplateModel<string>();
            try
            {
                var licence = await _iDapperRepository.Get<string>("[dbo].[sp_GetDOFLicenceDetails_MasterData]", new { DOFNumber = dofNumber }, CommandType.StoredProcedure).ConfigureAwait(false);
                genericServiceResultTemplate.ResponseDataModel = licence.FirstOrDefault();
            }
            catch (Exception ex)
            {
                genericServiceResultTemplate.HasError = true;
                genericServiceResultTemplate.ErrorDesc = ex.Message;
                throw ex;
            }
            return genericServiceResultTemplate;
        }

        public async Task<GenericServiceResultTemplate> Seed_BusinessFirstApprovedFiles(BusinessFirst_ApprovedFileSeeding requestData)
        {
            GenericServiceResultTemplate genericServiceResultTemplate = new GenericServiceResultTemplate();
            try
            {
                genericServiceResultTemplate.HasException = false;
                genericServiceResultTemplate.CustomeValidationResult = CustomeValidator<BusinessFirst_ApprovedFileSeeding>.ValidateModel_AllProperties(requestData);
                if (genericServiceResultTemplate.CustomeValidationResult.IsValid)
                {
                    _iGR_BusinessFirst_ApprovedFileSeeding.Insert(requestData);
                    await _iGR_BusinessFirst_ApprovedFileSeeding.SavechangeAsync();
                }
            }
            catch (Exception ex)
            {
                genericServiceResultTemplate.HasException = true;
                genericServiceResultTemplate.Exceptions = ex;
                throw ex;
            }
            return genericServiceResultTemplate;
        }
        public async Task<GenericResponseTemplateModel<List<GetLicenceNumberAndActTypeViewModel>>> GetActTypeByLicenceNumber(string licenceNumber)
        {
            GenericResponseTemplateModel<List<GetLicenceNumberAndActTypeViewModel>> genericServiceResultTemplate = new GenericResponseTemplateModel<List<GetLicenceNumberAndActTypeViewModel>>();
            try
            {
                List<StoreProcedureParm> storeProcedureParms = new List<StoreProcedureParm>()
                {
                    new StoreProcedureParm (){ ParmName="LicenceNumber", ParmValue = licenceNumber, isNumber = false},
                };
                genericServiceResultTemplate.ResponseDataModel = await _iGeneric_SP_Repository.CallStoreProcedureReaderAsync<GetLicenceNumberAndActTypeViewModel>("sp_getacttypebylicencenumber", storeProcedureParms);
            }
            catch (Exception ex)
            {
                genericServiceResultTemplate.HasError = true;
                genericServiceResultTemplate.ErrorDesc = ex.Message;
                throw ex;
            }
            return genericServiceResultTemplate;
        }

        public async Task<GenericFormModel<GetRedirectUrlViewModel>> GetRedirectToOtherPortel(string UserId, string role, string type)
        {
            GenericFormModel<GetRedirectUrlViewModel> genericFormModel = new GenericFormModel<GetRedirectUrlViewModel>();
            try
            {
                if (role == "MPRC")
                {
                    type = "MPRAPP";
                }
                string retUrl = Configuration.GetSection("ThirdPartyIntegrationConfigs").GetSection("OtherPortelUrl").GetSection("EPortalRetURL").Value;
                string redirectUrl = "";
                string url = "";
                string datetime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                var Userdata = await _context.Users.Include(x => x.UserRoles).Include(x => x.UserProfileMapping).Include(x => x.UserProfileMapping.UserProfile).Where(x => x.UserProfileMapping.IsActive == true).FirstOrDefaultAsync(x => x.Id == UserId);
                UserCircleMapping circleMapping = await _context.UserCircleMappings.Where(x => x.UserRefId == Userdata.Id && x.Version == 2).FirstOrDefaultAsync();

                // Finding circle based on the role
                Int64 circle = 0;
                if (role == "LBIN" || role == "BWDH" || role == "BACO")
                {
                    circle = (Int64)circleMapping.LabourCircleRefId;
                }
                else if (role == "ALLC")
                {
                    circle = (Int64)circleMapping.AlcCircleRefId;
                }
                else if (role == "ADRF" || role == "DDRF")
                {
                    circle = (Int64)circleMapping.FactoryCircleRefId;
                }

                var user = Userdata.Id + "|" + Userdata.UserProfileMapping.UserProfile.FirstName + " " + Userdata.UserProfileMapping.UserProfile.LastName + "|" +
                role + "|" + datetime + "|" + type + "|" + circle + "|" + retUrl;

                // 1. Redirect to node project
                if (type == "MPR" || type == "MPRAPP")
                {
                    redirectUrl = Configuration.GetSection("ThirdPartyIntegrationConfigs").GetSection("OtherPortelUrl").GetSection("MPRRedirectURL").Value;
                    string encurl = MPRRedirectEncry.Encrypt(user, Configuration.GetSection("EncryptionConfigs").GetSection("BPToken").Value);
                    url = redirectUrl + "?msg=" + MPRRedirectEncry.EncryptingURL(encurl, Configuration.GetSection("EncryptionConfigs").GetSection("BPToken").Value) + "|" + "PLBP";
                }

                // 2. Redirect to BOCW project
                else if (type == "BOCW")
                {
                    redirectUrl = Configuration.GetSection("ThirdPartyIntegrationConfigs").GetSection("OtherPortelUrl").GetSection("BOCWRedirectURL").Value;
                    string encurl = MPRRedirectEncry.Encrypt(user, Configuration.GetSection("EncryptionConfigs").GetSection("BOCWToken").Value);
                    url = redirectUrl + "?msg=" + MPRRedirectEncry.EncryptingURL(encurl, Configuration.GetSection("EncryptionConfigs").GetSection("BOCWToken").Value);
                }

                // 3. Redirect to LWS project
                else if (type == "LWBS")
                {
                    var guid = Guid.NewGuid().ToString();
                    user = Userdata.Id + "|" + Userdata.UserProfileMapping.UserProfile.FirstName + " " + Userdata.UserProfileMapping.UserProfile.LastName + "|" +
                    role + "|" + datetime + "|" + type + "|" + circle + "|" + retUrl + "|" + guid;

                    List<StoreProcedureParm> storeProcedureParms = new List<StoreProcedureParm>()
                    {
                        new StoreProcedureParm (){ ParmName="UserId", ParmValue=Userdata.Id.ToString(), isNumber=false},
                        new StoreProcedureParm (){ ParmName="Token", ParmValue=guid.ToString(), isNumber=false},
                        new StoreProcedureParm (){ ParmName="SwitchPortal", ParmValue="Welfare", isNumber=false},
                        new StoreProcedureParm (){ ParmName="RouteActivity", ParmValue="Out", isNumber=false}
                    };
                    await _iGeneric_SP_Repository.CallStoreProcedureReaderScalerAsync("dbo.sp_Legacy_InsertUserPortalSwitchLogs", storeProcedureParms);

                    //var tt = _iDapperRepository.AddupdateData("INSERT INTO UserPortalSwitchLogs (UserId, ActivityDate, Token, SwitchPortal, RouteActivity) VALUES(@UserId,@ActivityDate,@Token, @SwitchPortal, @RouteActivity)", new { UserId = Userdata.Id, ActivityDate = DateTime.Now, Token = guid, SwitchPortal = "Welfare", RouteActivity = "Out" }, CommandType.Text).ConfigureAwait(false);

                    redirectUrl = Configuration.GetSection("ThirdPartyIntegrationConfigs").GetSection("OtherPortelUrl").GetSection("LWBSRedirectURL").Value;
                    string encurl = MPRRedirectEncry.Encrypt(user, Configuration.GetSection("EncryptionConfigs").GetSection("BOCWToken").Value);
                    url = redirectUrl + "?msg=" + MPRRedirectEncry.EncryptingURL(encurl, Configuration.GetSection("EncryptionConfigs").GetSection("BOCWToken").Value);
                }

                genericFormModel.FormModel = new GetRedirectUrlViewModel
                {
                    RedirectUrl = url
                };
            }
            catch (Exception ex)
            {
                genericFormModel.HasError = true;
                genericFormModel.ErrorDesc = ex.Message;
                throw ex;
            }
            return genericFormModel;
        }

        public async Task<GenericResponseTemplateModel<StabiltyAcknoweldgementReceiptViewModel>> GetStabiltyAcknoweldgementSlip(string msg)
        {
            GenericResponseTemplateModel<StabiltyAcknoweldgementReceiptViewModel> genericFormModel = new GenericResponseTemplateModel<StabiltyAcknoweldgementReceiptViewModel>();
            try
            {

                var AppId = msg.Replace(".pdf", "");
                List<StoreProcedureParm> storeProcedureParms = new List<StoreProcedureParm>()
                {
                     new StoreProcedureParm (){ ParmName="AppId", ParmValue=AppId.ToString(), isNumber=false}
                };
                var establishmentDetails = await _iGeneric_SP_Repository.CallStoreProcedureReaderAsync<StabiltyAcknoweldgementReceiptViewModel>("sp_GetStabiltyAcknowDetailByAppRefId", storeProcedureParms);
                genericFormModel.ResponseDataModel = establishmentDetails.FirstOrDefault();
            }
            catch (Exception ex)
            {
                genericFormModel.HasError = true;
                genericFormModel.ErrorDesc = ex.Message;
                throw ex;
            }
            return genericFormModel;
        }

        public async Task<GenericResponseTemplateModel<List<IPinInfoViewModel>>> GetLatestDirtyApplicationsDetails()
        {
            GenericResponseTemplateModel<List<IPinInfoViewModel>> genericServiceResultTemplate = new GenericResponseTemplateModel<List<IPinInfoViewModel>>();
            try
            {
                var data = await _iGeneric_SP_Repository.CallStoreProcedureReaderAsyncWithoutParams<GetAllDirtyApplicationsViewModel>("sp_GetAllDirtyApplications");

                List<IPinInfoViewModel> pinInfoList = new List<IPinInfoViewModel>();
                IPinInfoViewModel pinInfo = new IPinInfoViewModel();
                if (data.Count > 0)
                {
                    var distinctIpins = data.Select(x => x.IPin).Distinct().ToList();
                    foreach (var iPin in distinctIpins)
                    {
                        pinInfo = new IPinInfoViewModel();
                        pinInfo.IPin = Convert.ToInt64(iPin);
                        pinInfo.Applications = new List<ApplicationIdInfoViewModel>();
                        var distinctAppIds = data.Where(x => x.IPin == iPin).Select(x => x.ApplicationId).Distinct().ToList();
                        if (distinctAppIds.Count() > 0)
                        {
                            foreach (var appId in distinctAppIds)
                            {
                                pinInfo.Applications.Add(new ApplicationIdInfoViewModel()
                                {
                                    ApplicationId = appId,
                                    ApplicationLogs = JsonConvert.DeserializeObject<List<ApplicationLogViewModel>>(JsonConvert.SerializeObject(data.Where(x => x.IPin == iPin && x.ApplicationId == appId).ToList()))
                                });
                            }
                        }
                        pinInfoList.Add(pinInfo);
                    }
                }
                genericServiceResultTemplate.ResponseDataModel = pinInfoList;



                List<SharedAppLogDetailVewModel> sharedData = new List<SharedAppLogDetailVewModel>();
                foreach (var item in pinInfoList)
                {
                    SharedAppLogDetailVewModel appData = new SharedAppLogDetailVewModel()
                    {
                        IPin = item.IPin
                    };

                    foreach (var application in item.Applications)
                    {
                        appData.ApplicationId = application.ApplicationId;
                        appData.AppLogsIds = String.Join(",", application.ApplicationLogs.Select(x => x.appActionLogId).ToArray());
                        appData.IsLegacy = data.Where(x => x.ApplicationId == application.ApplicationId && Convert.ToInt64(x.IPin) == item.IPin).Select(x => x.IsLegacy).FirstOrDefault();
                    }
                    sharedData.Add(appData);
                }

                SharedIsDirtyFlagRecordLog recordLog = new SharedIsDirtyFlagRecordLog
                {
                    RequestReceivedOn = DateTime.Now,
                    ResponseJson = JsonConvert.SerializeObject(sharedData)
                };

                _context.SharedIsDirtyFlagRecordLogs.Add(recordLog);
                await _context.SaveChangesAsync();


                // Update ApplicationDirtyStatusMappings
                foreach (var item in data.Where(x => x.IsLegacy == 0).ToList())
                {
                    List<StoreProcedureParm> storeProcedureParms1 = new List<StoreProcedureParm>()
                    {
                        new StoreProcedureParm (){ ParmName="AppRefId", ParmValue= item.AppRefId.ToString(), isNumber=true },
                        new StoreProcedureParm (){ ParmName="IsDirty", ParmValue= "0", isNumber=false }
                    };
                    var resp = await _iGeneric_SP_Repository.CallStoreProcedureReaderScalerAsync("dbo.sp_SetApplicationIsDirtyStatus", storeProcedureParms1);
                }

                // Sys- o
                foreach (var item in data.Where(x => x.IsLegacy == 1).ToList())
                {
                    List<StoreProcedureParm> storeProcedureParms2 = new List<StoreProcedureParm>()
                    {
                        new StoreProcedureParm (){ ParmName="AppRefId", ParmValue= item.AppRefId.ToString(), isNumber=true },
                        new StoreProcedureParm (){ ParmName="AppFormId", ParmValue= item.AppFormId.ToString(), isNumber=true },
                        new StoreProcedureParm (){ ParmName="IsDirty", ParmValue= "0", isNumber=true }
                    };
                    var resp = await _iGeneric_SP_Repository.CallStoreProcedureReaderScalerAsync("dbo.sp_SetApplicationIsDirtyStatus_Legacy", storeProcedureParms2);
                }
            }
            catch (Exception ex)
            {
                genericServiceResultTemplate.HasError = true;
                genericServiceResultTemplate.ErrorDesc = ex.Message;
                throw ex;
            }
            return genericServiceResultTemplate;
        }

        public async Task<GenericResponseTemplateModel<List<SetIsDirtyFlagForcefullyViewModel>>> SetIsDirtyFlagForcefully(List<DirtyFlagRequestParamsViewModel> requestData)
        {
            GenericResponseTemplateModel<List<SetIsDirtyFlagForcefullyViewModel>> genericServiceResultTemplate = new GenericResponseTemplateModel<List<SetIsDirtyFlagForcefullyViewModel>>();
            try
            {
                if (requestData.Count() > 0)
                {
                    List<SetIsDirtyFlagForcefullyViewModel> respList = new List<SetIsDirtyFlagForcefullyViewModel>();
                    foreach (var item in requestData)
                    {
                        List<StoreProcedureParm> storeProcedureParm = new List<StoreProcedureParm>()
                        {
                            new StoreProcedureParm { ParmName = "IPin", ParmValue = item.IPin.ToString(), isNumber = true },
                            new StoreProcedureParm { ParmName = "ApplicationId", ParmValue = item.ApplicationId.ToString(), isNumber = true },
                            new StoreProcedureParm { ParmName = "ServiceCode", ParmValue = item.ServiceCode.ToString(), isNumber = true }
                        };

                        var resp = await _iGeneric_SP_Repository.CallStoreProcedureReaderScalerAsync("sp_SetIsDirtyFlagForcefully", storeProcedureParm);
                        respList.Add(new SetIsDirtyFlagForcefullyViewModel
                        {
                            ApplicationId = item.ApplicationId,
                            IPin = item.IPin,
                            IsSuccess = (resp.Value == 1),
                            ErrorCode = (resp.Value == 1 ? 0 : 1),
                            ErrorDescription = (resp.Value == 1 ? "Record Updated" : "Application Not Found..!"),
                            ServiceCode = item.ServiceCode
                        });
                        genericServiceResultTemplate.ResponseDataModel = respList;
                    }
                }
            }
            catch (Exception ex)
            {
                genericServiceResultTemplate.HasError = true;
                genericServiceResultTemplate.ErrorDesc = ex.Message;
                throw ex;
            }
            return genericServiceResultTemplate;
        }

        public async Task<GenericResponseTemplateModel<List<IPinInfoViewModel>>> GetBFApplicationsLogDetails(List<DirtyFlagRequestParamsViewModel> requestData)
        {
            GenericResponseTemplateModel<List<IPinInfoViewModel>> genericServiceResultTemplate = new GenericResponseTemplateModel<List<IPinInfoViewModel>>();
            try
            {
                List<IPinInfoViewModel> pinInfoList = new List<IPinInfoViewModel>();
                if (requestData.Count() > 0)
                {
                    List<SetIsDirtyFlagForcefullyViewModel> respList = new List<SetIsDirtyFlagForcefullyViewModel>();
                    foreach (var item in requestData)
                    {
                        List<StoreProcedureParm> storeProcedureParm = new List<StoreProcedureParm>()
                        {
                            new StoreProcedureParm { ParmName = "IPin", ParmValue = item.IPin.ToString(), isNumber = true },
                            new StoreProcedureParm { ParmName = "ApplicationId", ParmValue = item.ApplicationId.ToString(), isNumber = true },
                            new StoreProcedureParm { ParmName = "ServiceCode", ParmValue = item.ServiceCode.ToString(), isNumber = true }
                        };
                        var data = await _iGeneric_SP_Repository.CallStoreProcedureReaderAsync<GetAllDirtyApplicationsViewModel>("SP_Get_BF_Application_Logs", storeProcedureParm);


                        IPinInfoViewModel pinInfo = new IPinInfoViewModel();
                        if (data.Count > 0)
                        {
                            var distinctIpins = data.Select(x => x.IPin).Distinct().ToList();
                            foreach (var iPin in distinctIpins)
                            {
                                pinInfo = new IPinInfoViewModel();
                                pinInfo.IPin = Convert.ToInt64(iPin);
                                pinInfo.Applications = new List<ApplicationIdInfoViewModel>();
                                var distinctAppIds = data.Where(x => x.IPin == iPin).Select(x => x.ApplicationId).Distinct().ToList();
                                if (distinctAppIds.Count() > 0)
                                {
                                    foreach (var appId in distinctAppIds)
                                    {
                                        pinInfo.Applications.Add(new ApplicationIdInfoViewModel()
                                        {
                                            ApplicationId = appId,
                                            ApplicationLogs = JsonConvert.DeserializeObject<List<ApplicationLogViewModel>>(JsonConvert.SerializeObject(data.Where(x => x.IPin == iPin && x.ApplicationId == appId).ToList()))
                                        });
                                    }
                                }
                                pinInfoList.Add(pinInfo);
                            }

                        }

                    }
                    genericServiceResultTemplate.ResponseDataModel = pinInfoList;
                }


            }
            catch (Exception ex)
            {
                genericServiceResultTemplate.HasError = true;
                genericServiceResultTemplate.ErrorDesc = ex.Message;
                throw ex;
            }
            return genericServiceResultTemplate;
        }


        public async Task<GenericResponseTemplateModel<List<AppSubmissionResponseViewModel>>> GetAppSubmissionDateByIpinAppId(List<DirtyFlagRequestParamsViewModel> requestData)
        {
            GenericResponseTemplateModel<List<AppSubmissionResponseViewModel>> genericServiceResultTemplate = new GenericResponseTemplateModel<List<AppSubmissionResponseViewModel>>();
            try
            {
                if (requestData.Count() > 0)
                {
                    List<AppSubmissionResponseViewModel> respList = new List<AppSubmissionResponseViewModel>();
                    foreach (var item in requestData)
                    {
                        List<StoreProcedureParm> storeProcedureParm = new List<StoreProcedureParm>()
                        {
                            new StoreProcedureParm { ParmName = "IPin", ParmValue = item.IPin.ToString(), isNumber = true },
                            new StoreProcedureParm { ParmName = "ApplicationId", ParmValue = item.ApplicationId.ToString(), isNumber = true },
                            new StoreProcedureParm { ParmName = "ServiceCode", ParmValue = item.ServiceCode.ToString(), isNumber = true }
                        };

                        var resp = await _iGeneric_SP_Repository.CallStoreProcedureReaderAsync<AppSubmissionResponseViewModel>("sp_GetAppSubmissionDateByIpinAppId", storeProcedureParm);
                        respList.Add(resp.FirstOrDefault());
                        genericServiceResultTemplate.ResponseDataModel = respList;
                    }
                }
            }
            catch (Exception ex)
            {
                genericServiceResultTemplate.HasError = true;
                genericServiceResultTemplate.ErrorDesc = ex.Message;
                throw ex;
            }
            return genericServiceResultTemplate;
        }

        public async Task<GenericResponseTemplateModel<List<IPinInfoViewModel>>> SeedApplicationDataByLegacyAppFormId(int legacyAppFormId)
        {
            GenericResponseTemplateModel<List<IPinInfoViewModel>> genericServiceResultTemplate = new GenericResponseTemplateModel<List<IPinInfoViewModel>>();
            try
            {
                var distinctLegacyAppId = await _iDapperRepository.Get<string>("SELECT DISTINCT AppId FROM PbLabour.dbo.AppPBIPAction_Log WHERE AppFormId = @AppFormId and AppId IN(11019,11020,11021,11022,11027,11028,11030,11031,11034,11036,11045,11063,11117,13961,14898,16808,19160,19175,19251,20894,21078,21498,21499,21592,21623,21706,22131,25073,25074,25119,25527,25529,26715,27352,27356,27443,27711,27783,27832,27878,27915,27916,27976,27992,40996,125891) AND NAR='BPC'", new { AppFormId = legacyAppFormId }, CommandType.Text).ConfigureAwait(false);
                ApplicationAction action = null;

                User user = null;
                foreach (var appId in distinctLegacyAppId)
                {
                    //if (_context.ApplicationSeedingLogs.Where(x => x.Legacy_AppFormId == legacyAppFormId && x.Legacy_AppId == Convert.ToInt64(appId)).Select(x => x.SeedStatusDiscription).FirstOrDefault() != "All Steps Completed")
                    //{
                    using (var workbook = new XLWorkbook())
                    {
                        ApplicationSeedingLog seedingLogs = new ApplicationSeedingLog();
                        // Get all the logs from legacy portal
                        var allLogs = await _iDapperRepository.Get<LegacyLogsViewModel>("SELECT * FROM AppPBIPAction_Log WHERE AppFormId = @AppFormId AND AppId = @AppId AND NAR='BPC' ORDER BY AppPBIPActionLogId ASC", new { AppFormId = legacyAppFormId, AppId = appId }, CommandType.Text).ConfigureAwait(false);
                        var distinctNAR = allLogs.Select(x => x.NAR).Distinct();
                        Int64 newAppId = 0;
                        foreach (var nar in distinctNAR)
                        {
                            if (!_context.ApplicationSeedingLogs.Any(x =>
                                x.Legacy_AppId == Convert.ToInt64(appId) &&
                                x.Legacy_AppFormId == legacyAppFormId &&
                                x.Legacy_NAR == nar &&
                                x.SeedStatusDiscription == "All Steps Completed"))
                            {
                                try
                                {
                                    var seededInfo = await GetApplicationSeedingLogsByAppId(Convert.ToInt64(appId), legacyAppFormId, nar);
                                    if (seededInfo == null)
                                    {
                                        string qry = @"INSERT INTO [dbo].[ApplicationSeedingLogs]([Legacy_AppId],[Legacy_AppFormId],[Legacy_NAR],[IsMasterSeeded],[IsDocumentSeeded],[IsPaymentSeeded],[IsApplicationLogsSeeded],[IsApprovalsSeeded],[OverAllSeedStatus],[SeedStatusDiscription],[IsProfileSeeded],[IsProjectSiteSeeded],[IsUserSeeded],[NewAppId],[IsBusinessRequestLogStatusSeeded]) VALUES("
                                                        + appId
                                                       + "," + legacyAppFormId
                                                       + ",'" + nar + "'"
                                                       + ",0"
                                                       + ",0"
                                                       + ",0"
                                                       + ",0"
                                                       + ",0"
                                                       + ",0"
                                                       + ",0"
                                                       + ",''"
                                                       + ",0"
                                                       + ",0"
                                                       + ",0"
                                                       + ",0)";

                                        ExecuteQueryUsingAdoNet(qry);
                                    }

                                    seededInfo = await GetApplicationSeedingLogsByAppId(Convert.ToInt64(appId), legacyAppFormId, nar).ConfigureAwait(false);
                                    if (seededInfo != null && !seededInfo.OverAllSeedStatus)
                                    {
                                        #region User & Profile

                                        // Find UserId In old Database
                                        var applications = await _iDapperRepository.Get<LegacyApplicationViewModel>("SELECT AP.*, SD.DistrictLgdId, ST.LGDirTehsilCode FROM Applications AP INNER JOIN ShopTehsils ST ON AP.SiteAddTehsilId = ST.TehsilId INNER JOIN PbLabourCoreNetDb.dbo.districts SD ON ST.LGDistrictCode = SD.DistrictLgdId WHERE AppId = @AppId", new { AppId = appId }, CommandType.Text).ConfigureAwait(false);
                                        if (applications.Count() > 0 && applications != null)
                                        {
                                            var userDetails = await _iDapperRepository.Get<LegacyUserViewModel>("SELECT UserName, ClientPasswdHash, PasswordHash, SecurityStamp,PhoneNumber,Email FROM AspNetUsers WHERE Id = @ApplicationUserID", new { ApplicationUserID = applications.FirstOrDefault().ApplicationUserID }, CommandType.Text).ConfigureAwait(false);

                                            if (_context.Users.Count(x => x.Id == applications.FirstOrDefault().ApplicationUserID.ToString()) > 0) //User Found in new DB
                                            {
                                                // Update the SeedStatusType
                                                seedingLogs = await GetApplicationSeedingLogsByAppId(Convert.ToInt64(appId), legacyAppFormId, nar).ConfigureAwait(false);

                                                ExecuteQueryUsingAdoNet("UPDATE ApplicationSeedingLogs SET IsUserSeeded = 2 WHERE Id=" + seedingLogs.Id.ToString());
                                                ExecuteQueryUsingAdoNet("UPDATE ApplicationSeedingLogs SET IsProfileSeeded = 1 WHERE Id=" + seedingLogs.Id.ToString());
                                            }
                                            else //User Not Found in new DB
                                            {
                                                if (seededInfo != null && seededInfo.IsUserSeeded == SeedStatusTypeEnum.NO_STATUS) // Create New User
                                                {
                                                    user = new User()
                                                    {
                                                        AccessFailedCount = 0,
                                                        ConcurrencyStamp = "",
                                                        Email = userDetails.FirstOrDefault().Email,
                                                        EmailConfirmed = true,
                                                        Id = applications.FirstOrDefault().ApplicationUserID.ToString(),
                                                        LockoutEnabled = false,
                                                        LockoutEnd = null,
                                                        NormalizedEmail = applications.FirstOrDefault().Email,
                                                        NormalizedUserName = userDetails.FirstOrDefault().UserName,
                                                        PasswordHash = userDetails.FirstOrDefault().PasswordHash,
                                                        PhoneNumber = userDetails.FirstOrDefault().PhoneNumber,
                                                        PhoneNumberConfirmed = true,
                                                        SecurityStamp = userDetails.FirstOrDefault().SecurityStamp,
                                                        TwoFactorEnabled = false,
                                                        UserName = userDetails.FirstOrDefault().UserName,
                                                        IsEnabled = false,
                                                        IsTestUser = false
                                                    };
                                                    await _context.AddAsync<User>(user);
                                                    await _context.SaveChangesAsync();

                                                    List<StoreProcedureParm> storeProcedureParms = new List<StoreProcedureParm>()
                                                {
                                                    new StoreProcedureParm (){ ParmName="RoleId", ParmValue="592add3e-f992-4983-a8ca-21ddc090bda0", isNumber=false},
                                                    new StoreProcedureParm (){ ParmName="UserId", ParmValue=user.Id, isNumber=false}
                                                };
                                                    await _iGeneric_SP_Repository.CallStoreProcedureReaderScalerAsync("dbo.sp_MapUserWithRole", storeProcedureParms).ConfigureAwait(false);

                                                    seedingLogs = await GetApplicationSeedingLogsByAppId(Convert.ToInt64(appId), legacyAppFormId, nar).ConfigureAwait(false);
                                                    ExecuteQueryUsingAdoNet("UPDATE ApplicationSeedingLogs SET IsUserSeeded = 1 WHERE Id=" + seedingLogs.Id.ToString());

                                                }

                                                if (seededInfo != null && seededInfo.IsProfileSeeded == SeedStatusTypeEnum.NO_STATUS) // // Create New User-Profile
                                                {
                                                    UserProfile userProfile = new UserProfile()
                                                    {
                                                        FirstName = applications.FirstOrDefault().FirstName,
                                                        MiddleName = applications.FirstOrDefault().MiddleName,
                                                        LastName = applications.FirstOrDefault().LastName,
                                                        FatherName = "NA",
                                                        MobileNo = applications.FirstOrDefault().MobileNo.Substring(applications.FirstOrDefault().MobileNo.Length - 10),
                                                        AlternateMobileNo = applications.FirstOrDefault().MobileNo.Substring(applications.FirstOrDefault().MobileNo.Length - 10),
                                                        Email = applications.FirstOrDefault().Email,
                                                        AlternateEmail = applications.FirstOrDefault().Email,
                                                        TehsilId = (Int64)applications.FirstOrDefault().LGDirTehsilCode,
                                                        DistrictId = applications.FirstOrDefault().DistrictLgdId,
                                                        State = applications.FirstOrDefault().StateId.ToString(),
                                                        PinCode = applications.FirstOrDefault().PinCode == null ? "000000" : applications.FirstOrDefault().PinCode.Substring(applications.FirstOrDefault().PinCode.Length - 6),
                                                        Signature = "NA",
                                                        ProfilePhoto = "NA",
                                                        IsActive = true,
                                                        IsDeleted = false,
                                                        Createddate = DateTime.Now,
                                                        LastModifiedDate = DateTime.Now
                                                    };

                                                    var userProfileResponse = await _iAuthService.CreateNewUserProfile(userProfile);
                                                    await _iAuthService.MapUserWithProfile(user.Id, userProfile.UserProfileId).ConfigureAwait(false);

                                                    seedingLogs = await GetApplicationSeedingLogsByAppId(Convert.ToInt64(appId), legacyAppFormId, nar);
                                                    ExecuteQueryUsingAdoNet("UPDATE ApplicationSeedingLogs SET IsProfileSeeded = 1 WHERE Id=" + seedingLogs.Id.ToString());

                                                }
                                            }
                                        }
                                        else
                                        {
                                            seedingLogs = await GetApplicationSeedingLogsByAppId(Convert.ToInt64(appId), legacyAppFormId, nar);
                                            ExecuteQueryUsingAdoNet("UPDATE ApplicationSeedingLogs SET IsUserSeeded = 4 WHERE Id=" + seedingLogs.Id.ToString());
                                        }

                                        #endregion
                                        long projectSiteId = 0;
                                        Int64 projectSiteRefId = 0;
                                        var appdata = _context.Applications.Where(s => s.Legacy_AppId == Convert.ToInt64(appId) && s.IsDeleted == false).FirstOrDefault();
                                        //var projectsite = _context.ProjectSites.Where(s => s.EstablishmentName == applications.FirstOrDefault().EstdName);
                                        if (appdata != null)
                                        {
                                            projectSiteId = appdata.ProjectSiteRefId;
                                            projectSiteRefId = appdata.ProjectSiteRefId;
                                        }
                                        else
                                        {
                                            projectSiteId = 0;
                                        }

                                        bool isProjectSiteToBeCreated = false;

                                        //var ipin = "0";
                                        var investPunjabAppId = "0";
                                        var ipin = "0";
                                        //var ipin = await _iDapperRepository.Get<string>(
                                        //    "SELECT PbLabour.dbo.GetAppIdFromIntegrationInfo(@AppId, @ServiceCode, @NAR)",new { AppId = appId, ServiceCode = legacyAppFormId , NAR = nar }, CommandType.Text).ConfigureAwait(false);

                                        List<StoreProcedureParm> storeProcedureParm = new List<StoreProcedureParm>()
                                        {
                                        new StoreProcedureParm (){ ParmName="E_LAB_AppId", ParmValue=appId, isNumber=true},
                                        new StoreProcedureParm (){ ParmName="ServiceCode", ParmValue=legacyAppFormId.ToString(), isNumber=true},
                                        new StoreProcedureParm (){ ParmName="NAR", ParmValue=nar, isNumber=false}
                                        };


                                        var legacyipinAppId = await _iGeneric_SP_Repository.CallStoreProcedureReaderAsync<AppSubmissionResponseViewModel>("dbo.sp_GetLegacyIpinAppId", storeProcedureParm);
                                        investPunjabAppId = legacyipinAppId.FirstOrDefault().ApplicationId.ToString();
                                        ipin = legacyipinAppId.FirstOrDefault().Ipin.ToString();


                                        if (projectSiteId == 0)
                                        {
                                            isProjectSiteToBeCreated = true;
                                        }
                                        else
                                        {
                                            isProjectSiteToBeCreated = false;
                                        }

                                        #region Project Site Created
                                        if (isProjectSiteToBeCreated && applications.Count() > 0 && applications != null)
                                        {
                                            if (seededInfo != null && seededInfo.IsProjectSiteSeeded == SeedStatusTypeEnum.NO_STATUS)
                                            {
                                                var factoryCircleId = 22;
                                                var labourCircleId = 10001;

                                                ProjectSite projectSite = new ProjectSite();

                                                {
                                                    projectSite.EstablishmentName = applications.FirstOrDefault().EstdName;
                                                    projectSite.Address = (applications.FirstOrDefault().SiteAddress == null || applications.FirstOrDefault().SiteAddress == "") ? "NA" : applications.FirstOrDefault().SiteAddress;
                                                    projectSite.VillageOrTown = (applications.FirstOrDefault().SiteAddCity == null || applications.FirstOrDefault().SiteAddCity == "") ? "NA" : applications.FirstOrDefault().SiteAddCity;
                                                    projectSite.TehsilRefId = applications.FirstOrDefault().LGDirTehsilCode;
                                                    projectSite.DistrictRefId = applications.FirstOrDefault().DistrictLgdId;
                                                    projectSite.PinCode = (applications.FirstOrDefault().PinCode == null || applications.FirstOrDefault().PinCode == "") ? "000000" : applications.FirstOrDefault().PinCode.Substring(applications.FirstOrDefault().PinCode.Length - 6);
                                                    projectSite.IsActive = true;
                                                    projectSite.IsDeleted = false;
                                                    projectSite.Createddate = DateTime.Now;
                                                    projectSite.LastModifiedDate = DateTime.Now;
                                                    projectSite.UserRefId = applications.FirstOrDefault().ApplicationUserID.ToString();
                                                    projectSite.LabourCircleRefId = labourCircleId == null ? 0 : labourCircleId;
                                                    projectSite.FactoryCircleRefId = (applications.FirstOrDefault().FCId == null || applications.FirstOrDefault().FCId.ToString() == "") ? 0 : (int)applications.FirstOrDefault().FCId;
                                                    projectSite.AlcCircleRefId = (applications.FirstOrDefault().ALLCCircleId == null || applications.FirstOrDefault().ALLCCircleId.ToString() == "") ? 0 : (int)applications.FirstOrDefault().ALLCCircleId;
                                                    projectSite.ApplicantAadharNumber = applications.FirstOrDefault().AadhaarNo;
                                                    projectSite.ApplicantAadharAttachment = "NA";
                                                    projectSite.ApplicantPanNumber = "NA";

                                                    projectSite.ApplicantPanAttachment = "NA";
                                                    projectSite.CompanyPanNumber = "NA";
                                                    projectSite.CompanyPanAttachment = "NA";
                                                    projectSite.ProjectPurpose = applications.FirstOrDefault().ProjectPurpose;
                                                    projectSite.ContactPersonFirstName = applications.FirstOrDefault().FirstName;
                                                    projectSite.ContactPersonMiddleName = applications.FirstOrDefault().MiddleName;
                                                    projectSite.ContactPersonLastName = (applications.FirstOrDefault().LastName == null || applications.FirstOrDefault().LastName == "") ? "." : applications.FirstOrDefault().LastName;
                                                    projectSite.ContactPersonEmail = applications.FirstOrDefault().Email;
                                                    projectSite.ContactPersonMobileNo = applications.FirstOrDefault().MobileNo.Substring(applications.FirstOrDefault().MobileNo.Length - 10);
                                                    projectSite.AlternateEmail = "abc@gmail.com";
                                                    projectSite.AlternateMobileNo = "9999999999";
                                                }
                                                ;
                                                var projectSiteCreateResp = await _iProjectSiteService.CreateProjectSite(projectSite, true, ipin.ToString()).ConfigureAwait(false);
                                                projectSiteRefId = projectSiteCreateResp.ResponseDataModel.ProjectSiteId;

                                                seedingLogs = await GetApplicationSeedingLogsByAppId(Convert.ToInt64(appId), legacyAppFormId, nar).ConfigureAwait(false);
                                                ExecuteQueryUsingAdoNet("UPDATE ApplicationSeedingLogs SET IsProjectSiteSeeded = 1 WHERE Id=" + seedingLogs.Id.ToString());
                                            }
                                        }
                                        else
                                        {
                                            isProjectSiteToBeCreated = false;
                                            seedingLogs = await GetApplicationSeedingLogsByAppId(Convert.ToInt64(appId), legacyAppFormId, nar);
                                            ExecuteQueryUsingAdoNet("UPDATE ApplicationSeedingLogs SET IsProjectSiteSeeded = 2 WHERE Id=" + seedingLogs.Id.ToString());
                                        }

                                        #endregion




                                        #region Master data
                                        if (seededInfo == null || seededInfo.IsMasterSeeded != SeedStatusTypeEnum.NO_STATUS || applications.Count() <= 0 || applications == null)
                                        {
                                            var app = _context.Applications.Where(x => x.Legacy_AppId.ToString() == appId && x.IsDeleted == false && legacyAppFormId == legacyAppFormId && x.Legacy_NAR == nar).FirstOrDefault();
                                            if (app == null)
                                            {
                                                seedingLogs = await GetApplicationSeedingLogsByAppId(Convert.ToInt64(appId), legacyAppFormId, nar).ConfigureAwait(false);
                                                ExecuteQueryUsingAdoNet("UPDATE ApplicationSeedingLogs SET OverAllSeedStatus = 0,SeedStatusDiscription ='No new app available'  WHERE Id=" + seedingLogs.Id.ToString());
                                            }
                                            else
                                            {
                                                newAppId = app.AppId;
                                            }
                                        }
                                        else
                                        {

                          
                                            var FactoryLicenseNumberNAR = "";
                                            if (legacyAppFormId == 2 && nar.Contains("BPC")) // Proposed Building Plan
                                            {
                                                var masterData = await _iDapperRepository.Get<LegacyBuildingPlanMasterDataViewModel>("SELECT * FROM BuildingPlans WHERE AppId = @AppId", new { AppId = appId }, CommandType.Text).ConfigureAwait(true);
                                                Licence_Proposed_BuildingPlan_GeneralDetail formData = new Licence_Proposed_BuildingPlan_GeneralDetail()
                                                {
                                                    CompetentPersonListId = Convert.ToInt32(masterData.FirstOrDefault().CompetentPersonListID),
                                                    BuildingCost = Convert.ToDecimal(masterData.FirstOrDefault().BuildingCost) * 100000,
                                                    IsBuildingConstructedBefore_01_Oct_2008 = masterData.FirstOrDefault().IsBuildingConstBefore2008,
                                                    DispatchNo = masterData.FirstOrDefault().OLNo,
                                                    DispatchDate = masterData.FirstOrDefault().IssueDate,
                                                    RegistrationDate = masterData.FirstOrDefault().RegistrationDate,
                                                    CompetentPersonVisitDate = masterData.FirstOrDefault().CompPerVisitDate,
                                                    BOCW_NoOfWorkers = masterData.FirstOrDefault().bocw_numberworker,

                                                    CompetentPersonName = "Not available",
                                                    CompetentPersonContactNo = "Not available",
                                                    CompetentPersonEmail = "Not available",
                                                    ArchitectName = "Not available",
                                                    ArchitectEmail = "Not available",
                                                    ArchitectContactNo = "Not available",
                                                    EngineerName = "Not available",
                                                    EngineerEmail = "Not available",
                                                    EngineerContactNo = "Not available",
                                                    BuildingPlanApprovalAuthorityType = BuildingPlanApprovalAuthorityTypeEnum.COMPETENT_PERSON,
                                                };

                                                var Sys_N_ServiceCodeInfo = FindServiceCodeMappingWithApplicationType(71);
                                                var statusResp = await _iApplicationMamnagement_Proposed_BuildingPlan_Service.InitiateApplication(ApplicationTypeEnum.BUILDING_PLAN_PROPOSED, formData, projectSiteRefId, Sys_N_ServiceCodeInfo.ApplicationPurposeType, applications.FirstOrDefault().ApplicationUserID, Convert.ToInt64(ipin), Convert.ToInt64(investPunjabAppId), true, Convert.ToInt64(appId), legacyAppFormId, nar, null, 1);
                                                newAppId = statusResp.AppId;

                                                // Update seeding log after master creation
                                                seedingLogs = await GetApplicationSeedingLogsByAppId(Convert.ToInt64(appId), legacyAppFormId, nar);

                                                string query = "UPDATE ApplicationSeedingLogs SET NewAppId = " + newAppId + ", IsMasterSeeded = 1 WHERE Id = " + seedingLogs.Id.ToString();
                                                ExecuteQueryUsingAdoNet(query);

                                            }
                                            else if (legacyAppFormId == 2 && nar.Contains("BPS")) // Existing
                                            {
                                                var masterData = await _iDapperRepository.Get<LegacyBuildingPlanMasterDataViewModel>("SELECT * FROM BuildingPlans WHERE AppId = @AppId AND NAR LIKE 'BPS%'", new { AppId = appId }, CommandType.Text).ConfigureAwait(true);
                                                if (masterData.Count() == 0)
                                                {
                                                    masterData = await _iDapperRepository.Get<LegacyBuildingPlanMasterDataViewModel>("SELECT TOP 1 * FROM BuildingPlans_Log WHERE AppId = @AppId AND NAR LIKE 'BPS%'", new { AppId = appId }, CommandType.Text).ConfigureAwait(true);
                                                }
                                                else
                                                {
                                                    Licence_Existing_BuildingPlan_GeneralDetail formData = new Licence_Existing_BuildingPlan_GeneralDetail()
                                                    {
                                                        CompetentPersonListId = Convert.ToInt32(masterData.FirstOrDefault().CompetentPersonListID),
                                                        BuildingCost = Convert.ToDecimal(masterData.FirstOrDefault().BuildingCost) * 100000,
                                                        IsBuildingConstructedBefore_01_Oct_2008 = masterData.FirstOrDefault().IsBuildingConstBefore2008,
                                                        DispatchNo = masterData.FirstOrDefault().OLNo,
                                                        DispatchDate = masterData.FirstOrDefault().IssueDate,
                                                        RegistrationDate = masterData.FirstOrDefault().RegistrationDate,
                                                        CompetentPersonVisitDate = masterData.FirstOrDefault().CompPerVisitDate,
                                                        BOCW_NoOfWorkers = masterData.FirstOrDefault().bocw_numberworker,
                                                        CompetentPersonName = "Not available",
                                                        CompetentPersonContactNo = "Not available",
                                                        CompetentPersonEmail = "Not available",
                                                        ArchitectName = "Not available",
                                                        ArchitectEmail = "Not available",
                                                        ArchitectContactNo = "Not available",
                                                        EngineerName = "Not available",
                                                        EngineerEmail = "Not available",
                                                        EngineerContactNo = "Not available",
                                                        BuildingPlanApprovalAuthorityType = BuildingPlanApprovalAuthorityTypeEnum.COMPETENT_PERSON,
                                                    };

                                                    var Sys_N_ServiceCodeInfo = FindServiceCodeMappingWithApplicationType(72);
                                                    var statusResp = await _iApplicationMamnagement_Existing_BuildingPlan_Service.InitiateApplication(ApplicationTypeEnum.BUILDING_PLAN_EXISTING, formData, projectSiteRefId, Sys_N_ServiceCodeInfo.ApplicationPurposeType, applications.FirstOrDefault().ApplicationUserID, Convert.ToInt64(ipin), Convert.ToInt64(investPunjabAppId), true, Convert.ToInt64(appId), legacyAppFormId, nar, null, 1);
                                                    newAppId = statusResp.AppId;
                                                }

                                                // Update seeding log after master creation
                                                seedingLogs = await GetApplicationSeedingLogsByAppId(Convert.ToInt64(appId), legacyAppFormId, nar);

                                                string query = "UPDATE ApplicationSeedingLogs SET NewAppId = " + newAppId + ", IsMasterSeeded = 1 WHERE Id = " + seedingLogs.Id.ToString();
                                                ExecuteQueryUsingAdoNet(query);
                                            }
                                            else if (legacyAppFormId == 2 && (nar.Contains("BPA") || nar.Contains("BPM"))) // Addition/Amendment
                                            {
                                                var masterData = await _iDapperRepository.Get<LegacyBuildingPlanMasterDataViewModel>("SELECT * FROM BuildingPlans WHERE AppId = @AppId", new { AppId = appId }, CommandType.Text).ConfigureAwait(true);
                                                Licence_Addition_Amendment_BuildingPlan_GeneralDetail formData = new Licence_Addition_Amendment_BuildingPlan_GeneralDetail()
                                                {
                                                    CompetentPersonListId = Convert.ToInt32(masterData.FirstOrDefault().CompetentPersonListID),
                                                    BuildingCost = Convert.ToDecimal(masterData.FirstOrDefault().BuildingCost) * 100000,
                                                    IsBuildingConstructedBefore_01_Oct_2008 = masterData.FirstOrDefault().IsBuildingConstBefore2008,
                                                    DispatchNo = masterData.FirstOrDefault().OLNo,
                                                    DispatchDate = masterData.FirstOrDefault().IssueDate,
                                                    RegistrationDate = masterData.FirstOrDefault().RegistrationDate,
                                                    CompetentPersonVisitDate = masterData.FirstOrDefault().CompPerVisitDate,
                                                    BOCW_NoOfWorkers = masterData.FirstOrDefault().bocw_numberworker,
                                                    CompetentPersonName = "Not available",
                                                    CompetentPersonContactNo = "Not available",
                                                    CompetentPersonEmail = "Not available",
                                                    ArchitectName = "Not available",
                                                    ArchitectEmail = "Not available",
                                                    ArchitectContactNo = "Not available",
                                                    EngineerName = "Not available",
                                                    EngineerEmail = "Not available",
                                                    EngineerContactNo = "Not available",
                                                    BuildingPlanApprovalAuthorityType = BuildingPlanApprovalAuthorityTypeEnum.COMPETENT_PERSON,
                                                };

                                                var Sys_N_ServiceCodeInfo = FindServiceCodeMappingWithApplicationType(73);
                                                var statusResp = await _iApplicationMamnagement_Addition_Amendment_BuildingPlan_Service.InitiateApplication(ApplicationTypeEnum.BUILDING_PLAN_ADDITION_AMENDMENT, formData, projectSiteRefId, Sys_N_ServiceCodeInfo.ApplicationPurposeType, applications.FirstOrDefault().ApplicationUserID, Convert.ToInt64(ipin), Convert.ToInt64(investPunjabAppId), true, Convert.ToInt64(appId), legacyAppFormId, nar, null, 1);
                                                newAppId = statusResp.AppId;

                                                // Update seeding log after master creation
                                                seedingLogs = await GetApplicationSeedingLogsByAppId(Convert.ToInt64(appId), legacyAppFormId, nar);

                                                string query = "UPDATE ApplicationSeedingLogs SET NewAppId = " + newAppId + ", IsMasterSeeded = 1 WHERE Id = " + seedingLogs.Id.ToString();
                                                ExecuteQueryUsingAdoNet(query);
                                            }
                                            else if (legacyAppFormId == 22 || legacyAppFormId == 23 || legacyAppFormId == 24 && nar.Contains("MTL") || nar.Contains("MTR") || nar.Contains("MTA")) // MTL
                                            {
                                                var masterData = await _iDapperRepository.Get<LegacyMotorTransportMasterDataViewModel>("SELECT * FROM MotorTransports WHERE AppId = @AppId", new { AppId = appId }, CommandType.Text).ConfigureAwait(true);
                                                Licence_MotorTransport formData = new Licence_MotorTransport()
                                                {
                                                    TransportUndertakingName = masterData.FirstOrDefault().NameMotorTrans,
                                                    CommunicationAddress = masterData.FirstOrDefault().CommAddress,
                                                    CommunicationAddress_PinCode = masterData.FirstOrDefault().CommPincode,
                                                    CommunicationAddress_DistrictLgdId = 1,
                                                    CommunicationAddress_TehsilLgdId = 1,
                                                    TransportServiceName = masterData.FirstOrDefault().NameTransService,
                                                    TotalRoutes = Convert.ToInt32(masterData.FirstOrDefault().TotalNoOfRoutes),
                                                    TotalRouteMileage = Convert.ToInt32(masterData.FirstOrDefault().TotalMileage),
                                                    TotalVehicles = Convert.ToInt32(masterData.FirstOrDefault().TotalNoVehiclesPreceedingYear),
                                                    MaxTransportWorkers = Convert.ToInt32(masterData.FirstOrDefault().MaxWorkersLastYear),
                                                    NameAddressType = masterData.FirstOrDefault().WhichAuthorty == "General Manager" ? MotorTransportNameAddressTypeEnum.GENERAL_MANAGER : MotorTransportNameAddressTypeEnum.PROPRIETOR_PARTNERS,
                                                    NameAddressType_Name = masterData.FirstOrDefault().AuthorityFullName ?? "No Name",
                                                    NameAddressType_Email = masterData.FirstOrDefault().AuthorityEmail ?? "nomail@gmail.com",
                                                    NameAddressType_Mobile = masterData.FirstOrDefault().AuthorityMobile ?? "9999999999",
                                                    NameAddressType_Address = masterData.FirstOrDefault().AuthorityAddress ?? "No Address",
                                                    IsCompanyRegUnderCompaniesAct = Convert.ToInt32(masterData.FirstOrDefault().IsCompany),
                                                    Director_Name = masterData.FirstOrDefault().DirectorName ?? "No Name",
                                                    Director_Email = masterData.FirstOrDefault().DirectorEmail ?? "nomail@gmail.com",
                                                    Director_Mobile = masterData.FirstOrDefault().DirectorMobile ?? "9999999999",
                                                    Director_Address = masterData.FirstOrDefault().DirectorAddress ?? "No Address",
                                                    ModifiedCounter = 1,
                                                    LicenceForYear = masterData.FirstOrDefault().pdate.Year,
                                                };

                                                var Sys_N_ServiceCodeInfo = FindServiceCodeMappingWithApplicationType(legacyAppFormId);
                                                var statusResp = await _iApplicationMamnagement_Licence_MotorTransport_Service.InitiateApplication(ApplicationTypeEnum.MOTOR_TRANSPORT, formData, projectSiteRefId, Sys_N_ServiceCodeInfo.ApplicationPurposeType, applications.FirstOrDefault().ApplicationUserID, Convert.ToInt64(ipin), Convert.ToInt64(investPunjabAppId), true, Convert.ToInt64(appId), legacyAppFormId, nar, null, 1);
                                                newAppId = statusResp.AppId;

                                                // Update seeding log after master creation
                                                seedingLogs = await GetApplicationSeedingLogsByAppId(Convert.ToInt64(appId), legacyAppFormId, nar);

                                                string query = "UPDATE ApplicationSeedingLogs SET NewAppId = " + newAppId + ", IsMasterSeeded = 1 WHERE Id = " + seedingLogs.Id.ToString();
                                                ExecuteQueryUsingAdoNet(query);
                                            }
                                            else if (legacyAppFormId == 6 || legacyAppFormId == 7 || legacyAppFormId == 8 && nar.Contains("FLM") || nar.Contains("FLR") || nar.Contains("FLA")) // Factory Licence Act
                                            {
                                                var masterData = await _iDapperRepository.Get<ProcessImd_ClearencesFactoryLicenceViewModel>("SELECT * FROM FactoryLicenses WHERE AppId = @AppId", new { AppId = appId }, CommandType.Text).ConfigureAwait(true);
                                                //if(masterData.FirstOrDefault().FactoryLicenseNumber == null || masterData.FirstOrDefault().FactoryLicenseNumber == "")
                                                //{
                                                //   var masterData1 = await _iDapperRepository.Get<ProcessImd_ClearencesFactoryLicenceViewModel>("SELECT * FROM FactoryLicenseNARs WHERE AppId = @AppId AND FactoryLicenseNumber IS NOT NULL", new { AppId = appId }, CommandType.Text).ConfigureAwait(true);
                                                //    FactoryLicenseNumberNAR = masterData1.FirstOrDefault().FactoryLicenseNumber;
                                                //}
                                                Licence_Factory_GeneralDetail formData = new Licence_Factory_GeneralDetail()
                                                {
                                                    IsBuildingConstructedBefore29June2018 = 1,
                                                    HaveYouMadeChangesInBuildingPlan = 0,
                                                    //OldLicenceNo = masterData.FirstOrDefault().FactoryLicenseNumber == null ? FactoryLicenseNumberNAR : masterData.FirstOrDefault().FactoryLicenseNumber,
                                                    OldLicenceNo = masterData.FirstOrDefault().FactoryLicenseNumber == null ? "NA" : masterData.FirstOrDefault().FactoryLicenseNumber,
                                                    OldLicenceValidUpTo = masterData.FirstOrDefault().OLStartDate,
                                                    OldLicenceTotalEmployees = masterData.FirstOrDefault().OLEmps,
                                                    OldLicenceFactoryKiloWatt = masterData.FirstOrDefault().InstalledPower,
                                                    RegistrationDate = masterData.FirstOrDefault().RegistrationDate,
                                                    RenewalFromDate = masterData.FirstOrDefault().ApplicationDate,

                                                    NoOfYears = masterData.FirstOrDefault().LicenceForNoOfYear,
                                                    ManufacturingProcess_Last12Months = "NA",
                                                    ManufacturingProcess_Next12Months = "NA",
                                                    NationalIndustrialClassificationCode = masterData.FirstOrDefault().NICCode,
                                                    MfgProducts_Last12Month = "NA",
                                                    Workers_MaxDuringYear = masterData.FirstOrDefault().MaximumNumberEmployeeInYear,
                                                    Workers_MaxLast12Month = masterData.FirstOrDefault().MaximumNumberEmployeeLastYear,
                                                    Workers_OrdinarilyEmployed = masterData.FirstOrDefault().OrdinarilyEmployed,
                                                    PowerKW_Installed = masterData.FirstOrDefault().InstalledPower,
                                                    PowerKW_MaxProposed = masterData.FirstOrDefault().MaximumPowerUsed,
                                                    ModifiedCounter = 1,
                                                    AppIdRightToBusinessAct = "NA",
                                                    BuildingPlanDofNumber = "NA",
                                                    CompetentPersonUserId = "NA",
                                                    DateOfPrincipalApproval = "NA",
                                                    IsBuildingPlanApproved = 2,
                                                    IsBuildingPlanVerified = true,
                                                    IsRBAVerified = true,
                                                    IsStabilityApproved = BuildingPlanStabilityAuthorityTypeEnum.DIRECTOR_FACTORIES,
                                                    IsStabilityPlanVerified = true,
                                                    IsTempRegistered = 0,
                                                    IsTempRegistrationVerified = true,
                                                    IsUnderRightToBusinessAct = 0,
                                                    ProjectIdentificationNo = "NA",
                                                    StabilityPlanDofNumber = "NA",
                                                    TempRegistrationNumber = "NA",
                                                    CompetentPersonName = "NA",
                                                    CompetentPersonContactNo = "9999999999",
                                                    CompetentPersonEmail = "NA",
                                                    EngineerName = "NA",
                                                    EngineerContactNo = "9999999999",
                                                    EngineerEmail = "NA",
                                                    BuildingPlanApprovalDate = DateTime.Now,
                                                    BuildingPlanStabilityApprovalDate = DateTime.Now
                                                };

                                                var Sys_N_ServiceCodeInfo = FindServiceCodeMappingWithApplicationType(legacyAppFormId);
                                                var statusResp = await _iApplicationMamnagement_Factory_Service.InitiateApplication(ApplicationTypeEnum.FACTORY_LICENCE, formData, projectSiteRefId, Sys_N_ServiceCodeInfo.ApplicationPurposeType, applications.FirstOrDefault().ApplicationUserID, Convert.ToInt64(ipin), Convert.ToInt64(investPunjabAppId), true, Convert.ToInt64(appId), legacyAppFormId, nar, null, 1);
                                                newAppId = statusResp.AppId;


                                                Licence_Factory_OccupierAndManagerDetail occupierDetails = new Licence_Factory_OccupierAndManagerDetail()
                                                {
                                                    ManagerFullName = masterData.FirstOrDefault().ManagerFullName,
                                                    ManagerFatherName = masterData.FirstOrDefault().ManagerFatherName,
                                                    ManagerFullAddress = masterData.FirstOrDefault().ManagerFullAddress,
                                                    ManagerMobile = masterData.FirstOrDefault().ManagerMobile == null ? "NA" : masterData.FirstOrDefault().ManagerMobile,
                                                    ManagerEmail = masterData.FirstOrDefault().ManagerEmail == null ? "NA" : masterData.FirstOrDefault().ManagerEmail,
                                                    ManagerResidentialAddress = masterData.FirstOrDefault().ManagerFullAddress,
                                                    OccupierFullName = masterData.FirstOrDefault().OccupierFullName,
                                                    OccupierFatherName = masterData.FirstOrDefault().OccupierFatherName,
                                                    OccupierFullAddress = masterData.FirstOrDefault().OccupierFullAddress,
                                                    OccupierMobile = masterData.FirstOrDefault().OccupierMobile == null ? "NA" : masterData.FirstOrDefault().OccupierMobile,
                                                    OccupierEmail = masterData.FirstOrDefault().OccupierEmail == null ? "NA" : masterData.FirstOrDefault().OccupierEmail,
                                                    OccupierResidentialAddress = masterData.FirstOrDefault().OccupierFullAddress,
                                                    OwnerName = masterData.FirstOrDefault().OwnerName,
                                                    OwnerPremisesAddress = masterData.FirstOrDefault().OwnerPremisesAddress,
                                                    StabilityCertificateNumber = "NA",
                                                    StabilityCertificateDate = masterData.FirstOrDefault().RegistrationDate,
                                                    StabilityDOFNumber = "NA",
                                                    Checklist_IsBuildingPlanApproved = "1",
                                                    Checklist_IsLabourWelfareFundPaid = "1",
                                                    Checklist_IsAnnualReturnFiled = "1",
                                                    Checklist_IsStabilityCertificateAttached = "1",
                                                    FactoryLicenceRefId = formData.FactoryLicenceId,
                                                    ModifiedCounter = 1
                                                };
                                                await _context.Licence_Factory_OccupierAndManagerDetails.AddAsync(occupierDetails);
                                                await _context.SaveChangesAsync();

                                                // Update seeding log after master creation
                                                seedingLogs = await GetApplicationSeedingLogsByAppId(Convert.ToInt64(appId), legacyAppFormId, nar);

                                                string query = "UPDATE ApplicationSeedingLogs SET NewAppId = " + newAppId + ", IsMasterSeeded = 1 WHERE Id = " + seedingLogs.Id.ToString();
                                                ExecuteQueryUsingAdoNet(query);
                                            }
                                            else if (legacyAppFormId == 3 || legacyAppFormId == 11 && nar.Contains("PEF") || nar.Contains("PEA")) // Principal Employeer Act
                                            {
                                                var masterData = await _iDapperRepository.Get<ProcessImd_ClearencesEstablishmentRegistrationViewModel>("SELECT * FROM EstablishmentRegistrations WHERE AppId = @AppId AND NAR = @NAR", new { AppId = appId, NAR = nar }, CommandType.Text).ConfigureAwait(true);

                                                if (masterData.Count() == 0)
                                                {
                                                    masterData = await _iDapperRepository.Get<ProcessImd_ClearencesEstablishmentRegistrationViewModel>("SELECT * FROM EstablishmentRegistration_Log WHERE AppId = @AppId  AND NAR = @NAR ", new { AppId = appId, NAR = nar }, CommandType.Text).ConfigureAwait(true);

                                                    if (masterData.Count() == 0)
                                                    {
                                                        masterData = await _iDapperRepository.Get<ProcessImd_ClearencesEstablishmentRegistrationViewModel>("SELECT * FROM EstablishmentRegistration_Log WHERE AppId = @AppId  ", new { AppId = appId }, CommandType.Text).ConfigureAwait(true);
                                                    }

                                                }


                                                Licence_CL_PE_GeneralDetail formData = new Licence_CL_PE_GeneralDetail();
                                                if (masterData.Count() == 0)
                                                {
                                                    {
                                                        formData.PE_Name = "NA";
                                                        formData.PE_FatherName = "NA";
                                                        formData.PE_Mobile = "0000000000";
                                                        formData.PE_Email = "No Email";
                                                        formData.PE_Address = "No Address";
                                                        formData.Manager_Name = "NA";
                                                        formData.Manager_Mobile = "0000000000";
                                                        formData.Manager_Email = "No Email";
                                                        formData.Manager_Address = "No Address";
                                                        formData.NatureOfWork = "NA";
                                                        formData.TotalWorker = 0;

                                                    }
                                                    ;

                                                }
                                                else
                                                {
                                                    {
                                                        formData.PE_Name = masterData.FirstOrDefault().PrincipalEmpName;
                                                        formData.PE_FatherName = masterData.FirstOrDefault().PrincipalFatherEmpName ?? "NA";
                                                        formData.PE_Mobile = masterData.FirstOrDefault().PrincipalEmpMobileNo ?? "0000000000";
                                                        formData.PE_Email = masterData.FirstOrDefault().PrincipalEmpEmailId ?? "No Email";
                                                        formData.PE_Address = masterData.FirstOrDefault().PrincipalEmpAddress ?? "No Address";
                                                        formData.Manager_Name = masterData.FirstOrDefault().ManagerName ?? "NA";
                                                        formData.Manager_Mobile = masterData.FirstOrDefault().ManagerMobileNo ?? "0000000000";
                                                        formData.Manager_Email = masterData.FirstOrDefault().ManagerEmailId ?? "No Email";
                                                        formData.Manager_Address = masterData.FirstOrDefault().ManagerAddress ?? "No Address";
                                                        formData.NatureOfWork = masterData.FirstOrDefault().WorkCarried;
                                                        formData.TotalWorker = masterData.FirstOrDefault().TotalWorker;

                                                    }
                                                    ;

                                                }

                                                var validations = CustomeValidator<Licence_CL_PE_GeneralDetail>.ValidateModel_AllProperties(formData);
                                                if (!validations.IsValid)
                                                {
                                                    var kk = validations.CustomeValidationErrorList;
                                                }

                                                var Sys_N_ServiceCodeInfo = FindServiceCodeMappingWithApplicationType(legacyAppFormId);
                                                var statusResp = await _iApplicationMamnagement_CL_PE_Service.InitiateApplication(ApplicationTypeEnum.PRINCIPAL_EMPLOYER, formData, projectSiteRefId, Sys_N_ServiceCodeInfo.ApplicationPurposeType, applications.FirstOrDefault().ApplicationUserID, Convert.ToInt64(ipin), Convert.ToInt64(investPunjabAppId), true, Convert.ToInt64(appId), legacyAppFormId, nar, null, 1);
                                                newAppId = statusResp.AppId;

                                                // Get Employee Details 
                                                var contractordetails = await _iSystem_O_CommunicationService.GetContractorList(Convert.ToInt64(appId), legacyAppFormId, nar);

                                                if (contractordetails != null)
                                                {
                                                    Licence_CL_PE_Contrator contractorlist = null;
                                                    foreach (var item in contractordetails.ResponseDataModel)
                                                    {
                                                        contractorlist = new Licence_CL_PE_Contrator()
                                                        {
                                                            Name = item.ContractorName,
                                                            Address = item.ContractorAddress,
                                                            NatureOfWork = item.WorkNature ?? "NA",
                                                            MaxLabourWorkerEmployed = (int)item.ContractLabourEmployed,
                                                            DateOfCommencement = item.ContractWork_CommencingDate,
                                                            DateOfTermination = item.ContractWork_TerminationDate,
                                                            Licence_CL_PE_GeneralDetailRefId = statusResp.EntityKeyId,
                                                            ModifiedCounter = 0
                                                        };

                                                        validations = CustomeValidator<Licence_CL_PE_Contrator>.ValidateModel_AllProperties(contractorlist);
                                                        if (!validations.IsValid)
                                                        {
                                                            var kk = validations.CustomeValidationErrorList;
                                                        }

                                                        await _context.Licence_CL_PE_Contrator.AddAsync(contractorlist);
                                                        await _context.SaveChangesAsync();
                                                    }
                                                }

                                                // Update seeding log after master creation
                                                seedingLogs = await GetApplicationSeedingLogsByAppId(Convert.ToInt64(appId), legacyAppFormId, nar);

                                                string query = "UPDATE ApplicationSeedingLogs SET NewAppId = " + newAppId + ", IsMasterSeeded = 1 WHERE Id = " + seedingLogs.Id.ToString();
                                                ExecuteQueryUsingAdoNet(query);
                                            }
                                            else if (legacyAppFormId == 4 || legacyAppFormId == 12 || legacyAppFormId == 13 && nar.Contains("CLF") || nar.Contains("CLR") || nar.Contains("CLA")) // Contract Labour Act
                                            {
                                                var masterData = await _iDapperRepository.Get<ProcessImd_ClearencesContractLabourViewModel>("SELECT * FROM ContactLabours WHERE AppId = @AppId AND NAR = @NAR", new { AppId = appId, NAR = nar }, CommandType.Text).ConfigureAwait(true);

                                                if (masterData.Count() == 0)
                                                {
                                                    masterData = await _iDapperRepository.Get<ProcessImd_ClearencesContractLabourViewModel>("SELECT * FROM ContactLabourNARs WHERE AppId = @AppId  AND NAR = @NAR ", new { AppId = appId, NAR = nar }, CommandType.Text).ConfigureAwait(true);

                                                    if (masterData.Count() == 0)
                                                    {
                                                        masterData = await _iDapperRepository.Get<ProcessImd_ClearencesContractLabourViewModel>("SELECT * FROM ContactLabourNARs WHERE AppId = @AppId  ", new { AppId = appId }, CommandType.Text).ConfigureAwait(true);
                                                    }

                                                }


                                                Licence_ContractLabour_GeneralDetail formData = new Licence_ContractLabour_GeneralDetail();
                                                if (masterData.Count() == 0)
                                                {
                                                    {
                                                        formData.IsCoopraticSociety = "1";
                                                        formData.ContractorName = "NA";
                                                        formData.ContractorFatherName = "NA";
                                                        formData.ContractorAddress = "NA";
                                                        formData.ContractorDOB = DateTime.Today;
                                                        formData.TypeOfBusiness = "NA";
                                                        formData.RegistrationCertificateNo = "NA";
                                                        formData.RegistrationCertificateDate = DateTime.Today;
                                                        formData.PrincipalEmployerName = "NA";
                                                        formData.PrincipalEmployerAddress = "NA";
                                                        formData.NatureOfWork = "NA";
                                                        formData.ContractWork_CommencingDate = DateTime.Today;
                                                        formData.ContractWork_TerminationDate = DateTime.Today;
                                                        formData.AgentOrManagerName = "NA";
                                                        formData.AgentOrManagerAddress = "NA";
                                                        formData.MaximumNumberOfEmployee = 0;
                                                        formData.ContractorAge = 0;
                                                        formData.LicenceForYear = 0;
                                                        formData.ModifiedCounter = 1;
                                                        formData.GstNumber = "000000000";
                                                        formData.PanOrTanNumber = "ABCDD1234E";
                                                        formData.EstablishmentName = "NA";

                                                    }
                                                    ;

                                                }
                                                else
                                                {
                                                    {
                                                        formData.IsCoopraticSociety = "1";
                                                        formData.ContractorName = masterData.FirstOrDefault().ContractorName;
                                                        formData.ContractorFatherName = masterData.FirstOrDefault().ContractorFatherName ?? "NA";
                                                        formData.ContractorAddress = masterData.FirstOrDefault().ContractorAddress ?? "NA";
                                                        formData.ContractorDOB = masterData.FirstOrDefault().DOB ?? DateTime.Today;
                                                        formData.TypeOfBusiness = masterData.FirstOrDefault().TypeOfBusiness;
                                                        formData.RegistrationCertificateNo = masterData.FirstOrDefault().RegistrationCertificateNo ?? "NA";
                                                        formData.RegistrationCertificateDate = masterData.FirstOrDefault().RegistrationCertificateDate ?? DateTime.Today;
                                                        formData.PrincipalEmployerName = masterData.FirstOrDefault().PrincipalEmployerName ?? "NA";
                                                        formData.PrincipalEmployerAddress = masterData.FirstOrDefault().PrincipalEmployerAddress ?? "NA";
                                                        formData.NatureOfWork = masterData.FirstOrDefault().WorkNatureString;
                                                        formData.ContractWork_CommencingDate = masterData.FirstOrDefault().ContractWork_CommencingDate;
                                                        formData.ContractWork_TerminationDate = masterData.FirstOrDefault().ContractWork_TerminationDate;
                                                        formData.AgentOrManagerName = masterData.FirstOrDefault().AgentOrManagerName ?? "NA";
                                                        formData.AgentOrManagerAddress = masterData.FirstOrDefault().AgentOrManagerAddress ?? "NA";
                                                        formData.MaximumNumberOfEmployee = masterData.FirstOrDefault().MaximumNumberEmployee;
                                                        formData.ContractorAge = masterData.FirstOrDefault().Age;
                                                        formData.LicenceForYear = masterData.FirstOrDefault().RegistrationFrom;
                                                        formData.ModifiedCounter = 1;
                                                        formData.GstNumber = "000000000";
                                                        formData.PanOrTanNumber = "ABCDD1234E";
                                                        formData.EstablishmentName = masterData.FirstOrDefault().EstablishmentName ?? "NA";

                                                    }
                                                    ;

                                                }

                                                var validations = CustomeValidator<Licence_ContractLabour_GeneralDetail>.ValidateModel_AllProperties(formData);
                                                if (!validations.IsValid)
                                                {
                                                    var kk = validations.CustomeValidationErrorList;
                                                }

                                                var Sys_N_ServiceCodeInfo = FindServiceCodeMappingWithApplicationType(legacyAppFormId);
                                                var statusResp = await _iApplicationMamnagement_ContractLabour_Service.InitiateApplication(ApplicationTypeEnum.CONTRACT_LABOUR, formData, projectSiteRefId, Sys_N_ServiceCodeInfo.ApplicationPurposeType, applications.FirstOrDefault().ApplicationUserID, Convert.ToInt64(ipin), Convert.ToInt64(investPunjabAppId), true, Convert.ToInt64(appId), legacyAppFormId, nar, null, 1).ConfigureAwait(false);
                                                newAppId = statusResp.AppId;

                                                // Update seeding log after master creation
                                                seedingLogs = await GetApplicationSeedingLogsByAppId(Convert.ToInt64(appId), legacyAppFormId, nar).ConfigureAwait(false);

                                                string query = "UPDATE ApplicationSeedingLogs SET NewAppId = " + newAppId + ", IsMasterSeeded = 1 WHERE Id = " + seedingLogs.Id.ToString();
                                                ExecuteQueryUsingAdoNet(query);
                                            }
                                            else if (legacyAppFormId == 17 || legacyAppFormId == 18 && nar.Contains("IPR") || nar.Contains("IPA")) // Principal Employeer Act
                                            {
                                                var masterData = await _iDapperRepository.Get<ProcessImd_ClearencesInterstatePrincipalEmployersViewModel>("SELECT * FROM InterstatePrincipalEmployers WHERE AppId = @AppId AND NAR = @NAR", new { AppId = appId, NAR = nar }, CommandType.Text).ConfigureAwait(true);

                                                if (masterData.Count() == 0)
                                                {
                                                    masterData = await _iDapperRepository.Get<ProcessImd_ClearencesInterstatePrincipalEmployersViewModel>("SELECT * FROM InterstatePrincipalEmployer_Log WHERE AppId = @AppId  AND NAR = @NAR ", new { AppId = appId, NAR = nar }, CommandType.Text).ConfigureAwait(true);

                                                    if (masterData.Count() == 0)
                                                    {
                                                        masterData = await _iDapperRepository.Get<ProcessImd_ClearencesInterstatePrincipalEmployersViewModel>("SELECT * FROM InterstatePrincipalEmployer_Log WHERE AppId = @AppId  ", new { AppId = appId }, CommandType.Text).ConfigureAwait(true);
                                                    }
                                                }
                                                Licence_PE_ISM_GeneralDetail formData = new Licence_PE_ISM_GeneralDetail();
                                                if (masterData.Count() == 0)
                                                {
                                                    {
                                                        formData.PE_Name = "NA";
                                                        formData.PE_FatherName = "NA";
                                                        formData.PE_Mobile = "0000000000";
                                                        formData.PE_Email = "No Email";
                                                        formData.PE_Address = "No Address";
                                                        formData.Manager_Name = "NA";
                                                        formData.Manager_Mobile = "0000000000";
                                                        formData.Manager_Email = "No Email";
                                                        formData.Manager_Address = "No Address";
                                                        formData.NatureOfWork = "NA";
                                                        formData.TotalWorker = 0;
                                                    } ;

                                                }
                                                else
                                                {
                                                    {
                                                        formData.PE_Name = masterData.FirstOrDefault().PrincipalEmpName;
                                                        formData.PE_FatherName = masterData.FirstOrDefault().PrincipalFatherEmpName ?? "NA";
                                                        formData.PE_Mobile = masterData.FirstOrDefault().PrincipalEmpMobileNo ?? "0000000000";
                                                        formData.PE_Email = masterData.FirstOrDefault().PrincipalEmpEmailId ?? "No Email";
                                                        formData.PE_Address = masterData.FirstOrDefault().PrincipalEmpAddress ?? "No Address";
                                                        formData.Manager_Name = masterData.FirstOrDefault().ManagerName ?? "NA";
                                                        formData.Manager_Mobile = masterData.FirstOrDefault().ManagerMobileNo ?? "0000000000";
                                                        formData.Manager_Email = masterData.FirstOrDefault().ManagerEmailId ?? "No Email";
                                                        formData.Manager_Address = masterData.FirstOrDefault().ManagerAddress ?? "No Address";
                                                        formData.NatureOfWork = masterData.FirstOrDefault().WorkCarried;
                                                        formData.TotalWorker = 0;

                                                    }
                                                    ;

                                                }

                                                var validations = CustomeValidator<Licence_PE_ISM_GeneralDetail>.ValidateModel_AllProperties(formData);
                                                if (!validations.IsValid)
                                                {
                                                    var kk = validations.CustomeValidationErrorList;
                                                }

                                                var Sys_N_ServiceCodeInfo = FindServiceCodeMappingWithApplicationType(legacyAppFormId);
                                                var statusResp = await _iApplicationMamnagement_Licence_ISM_PrincipalEmployer.InitiateApplication(ApplicationTypeEnum.ISM_PRINCIPAL_EMPLOYER, formData, projectSiteRefId, Sys_N_ServiceCodeInfo.ApplicationPurposeType, applications.FirstOrDefault().ApplicationUserID, Convert.ToInt64(ipin), Convert.ToInt64(investPunjabAppId), true, Convert.ToInt64(appId), legacyAppFormId, nar, null, 1);
                                                newAppId = statusResp.AppId;

                                                // Get Employee Details 
                                                var contractordetails = await _iSystem_O_CommunicationService.GetISMContractorList(Convert.ToInt64(appId), legacyAppFormId, nar);

                                                if (contractordetails != null)
                                                {
                                                    Licence_PE_ISM_Contrator contractorlist = null;
                                                    foreach (var item in contractordetails.ResponseDataModel)
                                                    {
                                                        contractorlist = new Licence_PE_ISM_Contrator()
                                                        {
                                                            Name = item.ContractorName,
                                                            Address = item.ContractorAddress,
                                                            NatureOfWork = item.WorkNatureString ?? "NA",
                                                            MaxLabourWorkerEmployed = (int)item.MaximumNumberEmployee,
                                                            DateOfCommencement = item.ContractWork_CommencingDate,
                                                            DateOfTermination = item.ContractWork_TerminationDate,
                                                            Licence_PE_ISM_GeneralDetailId = statusResp.EntityKeyId,
                                                            ModifiedCounter = 0,
                                                            IsFormVGenerate = 1,
                                                            FormVGenerateDate = DateTime.Now
                                                        };

                                                        validations = CustomeValidator<Licence_PE_ISM_Contrator>.ValidateModel_AllProperties(contractorlist);
                                                        if (!validations.IsValid)
                                                        {
                                                            var kk = validations.CustomeValidationErrorList;
                                                        }

                                                        await _context.Licence_PE_ISM_Contrators.AddAsync(contractorlist);
                                                        await _context.SaveChangesAsync();
                                                    }
                                                }

                                                // Update seeding log after master creation
                                                seedingLogs = await GetApplicationSeedingLogsByAppId(Convert.ToInt64(appId), legacyAppFormId, nar);

                                                string query = "UPDATE ApplicationSeedingLogs SET NewAppId = " + newAppId + ", IsMasterSeeded = 1 WHERE Id = " + seedingLogs.Id.ToString();
                                                ExecuteQueryUsingAdoNet(query);
                                            }
                                            else if (legacyAppFormId == 19 || legacyAppFormId == 20 || legacyAppFormId == 21 && nar.Contains("ICL") || nar.Contains("ICR") || nar.Contains("ICA")) // ISM Contract Labour Act
                                            {
                                                var masterData = await _iDapperRepository.Get<ProcessImd_ClearencesInterstateContractLabourViewModel>("SELECT * FROM InterstateContractLabours WHERE AppId = @AppId AND NAR = @NAR", new { AppId = appId, NAR = nar }, CommandType.Text).ConfigureAwait(true);

                                                if (masterData.Count() == 0)
                                                {
                                                    masterData = await _iDapperRepository.Get<ProcessImd_ClearencesInterstateContractLabourViewModel>("SELECT * FROM InterstateContractLabour_Log WHERE AppId = @AppId  AND NAR = @NAR ", new { AppId = appId, NAR = nar }, CommandType.Text).ConfigureAwait(true);

                                                    if (masterData.Count() == 0)
                                                    {
                                                        masterData = await _iDapperRepository.Get<ProcessImd_ClearencesInterstateContractLabourViewModel>("SELECT * FROM InterstateContractLabour_Log WHERE AppId = @AppId  ", new { AppId = appId }, CommandType.Text).ConfigureAwait(true);
                                                    }

                                                }


                                                Licence_ISM_ContractLabour_GeneralDetail formData = new Licence_ISM_ContractLabour_GeneralDetail();
                                                if (masterData.Count() == 0)
                                                {
                                                    {
                                                        formData.IsCoopraticSociety = "1";
                                                        formData.ContractorName = "NA";
                                                        formData.ContractorFatherName = "NA";
                                                        formData.ContractorAddress = "NA";
                                                        formData.ContractorDOB = DateTime.Today;
                                                        formData.TypeOfBusiness = "NA";
                                                        formData.RegistrationCertificateNo = "NA";
                                                        formData.RegistrationCertificateDate = DateTime.Today;
                                                        formData.PrincipalEmployerName = "NA";
                                                        formData.PrincipalEmployerAddress = "NA";
                                                        formData.NatureOfWork = "NA";
                                                        formData.ContractWork_CommencingDate = DateTime.Today;
                                                        formData.ContractWork_TerminationDate = DateTime.Today;
                                                        formData.AgentOrManagerName = "NA";
                                                        formData.AgentOrManagerAddress = "NA";
                                                        formData.MaximumNumberOfEmployee = 0;
                                                        formData.ContractorAge = 0;
                                                        formData.LicenceForYear = "NA";
                                                        formData.ModifiedCounter = 1;
                                                        formData.GstNumber = "000000000";
                                                        formData.PanOrTanNumber = "ABCDD1234E";

                                                    }
                                                    ;

                                                }
                                                else
                                                {
                                                    {
                                                        formData.IsCoopraticSociety = "1";
                                                        formData.ContractorName = masterData.FirstOrDefault().ContractorName;
                                                        formData.ContractorFatherName = masterData.FirstOrDefault().ContractorFatherName ?? "NA";
                                                        formData.ContractorAddress = masterData.FirstOrDefault().ContractorAddress ?? "NA";
                                                        formData.ContractorDOB = masterData.FirstOrDefault().DOB ?? DateTime.Today;
                                                        formData.TypeOfBusiness = masterData.FirstOrDefault().TypeOfBusiness;
                                                        formData.RegistrationCertificateNo = masterData.FirstOrDefault().RegistrationCertificateNo ?? "NA";
                                                        formData.RegistrationCertificateDate = masterData.FirstOrDefault().RegistrationCertificateDate ?? DateTime.Today;
                                                        formData.PrincipalEmployerName = masterData.FirstOrDefault().PrincipalEmployerName ?? "NA";
                                                        formData.PrincipalEmployerAddress = masterData.FirstOrDefault().PrincipalEmployerAddress ?? "NA";
                                                        formData.NatureOfWork = masterData.FirstOrDefault().WorkNatureString;
                                                        formData.ContractWork_CommencingDate = masterData.FirstOrDefault().ContractWork_CommencingDate;
                                                        formData.ContractWork_TerminationDate = masterData.FirstOrDefault().ContractWork_TerminationDate;
                                                        formData.AgentOrManagerName = masterData.FirstOrDefault().AgentOrManagerName ?? "NA";
                                                        formData.AgentOrManagerAddress = masterData.FirstOrDefault().AgentOrManagerAddress ?? "NA";
                                                        formData.MaximumNumberOfEmployee = masterData.FirstOrDefault().MaximumNumberEmployee;
                                                        formData.ContractorAge = masterData.FirstOrDefault().Age;
                                                        formData.LicenceForYear = masterData.FirstOrDefault().RegistrationFrom;
                                                        formData.ModifiedCounter = 1;
                                                        formData.GstNumber = "000000000";
                                                        formData.PanOrTanNumber = "ABCDD1234E";

                                                    }
                                                    ;

                                                }

                                                var validations = CustomeValidator<Licence_ISM_ContractLabour_GeneralDetail>.ValidateModel_AllProperties(formData);
                                                if (!validations.IsValid)
                                                {
                                                    var kk = validations.CustomeValidationErrorList;
                                                }

                                                var Sys_N_ServiceCodeInfo = FindServiceCodeMappingWithApplicationType(legacyAppFormId);
                                                var statusResp = await _iApplicationMamnagement_Licence_ISM_ContractLabour.InitiateApplication(ApplicationTypeEnum.ISM_CONTRACT_LABOUR, formData, projectSiteRefId, Sys_N_ServiceCodeInfo.ApplicationPurposeType, applications.FirstOrDefault().ApplicationUserID, Convert.ToInt64(ipin), Convert.ToInt64(investPunjabAppId), true, Convert.ToInt64(appId), legacyAppFormId, nar, null, 1).ConfigureAwait(false);
                                                newAppId = statusResp.AppId;

                                                // Update seeding log after master creation
                                                seedingLogs = await GetApplicationSeedingLogsByAppId(Convert.ToInt64(appId), legacyAppFormId, nar).ConfigureAwait(false);

                                                string query = "UPDATE ApplicationSeedingLogs SET NewAppId = " + newAppId + ", IsMasterSeeded = 1 WHERE Id = " + seedingLogs.Id.ToString();
                                                ExecuteQueryUsingAdoNet(query);
                                            }
                                            else if (legacyAppFormId == 35 || legacyAppFormId == 36 && nar.Contains("BOF") || nar.Contains("BOA")) // BOCW Act
                                            {
                                                var masterData = await _iDapperRepository.Get<ProcessImd_ClearencesBocwRegistrationViewModel>("SELECT * FROM BOCWEstbRegistrations WHERE AppId = @AppId AND NAR = @NAR", new { AppId = appId, NAR = nar }, CommandType.Text).ConfigureAwait(true);

                                                if (masterData.Count() == 0)
                                                {
                                                    masterData = await _iDapperRepository.Get<ProcessImd_ClearencesBocwRegistrationViewModel>("SELECT * FROM BOCWEstbRegistration_log WHERE AppId = @AppId  AND NAR = @NAR ", new { AppId = appId, NAR = nar }, CommandType.Text).ConfigureAwait(true);

                                                    if (masterData.Count() == 0)
                                                    {
                                                        masterData = await _iDapperRepository.Get<ProcessImd_ClearencesBocwRegistrationViewModel>("SELECT * FROM BOCWEstbRegistration_log WHERE AppId = @AppId  ", new { AppId = appId }, CommandType.Text).ConfigureAwait(true);
                                                    }

                                                }

                                                var district = await _iDapperRepository.Get<ShopTehsilsViewModel>("SELECT * FROM ShopTehsils WHERE TehsilId =  @TehsilId", new { TehsilId = applications.FirstOrDefault()?.SiteAddTehsilId }, CommandType.Text).ConfigureAwait(true);
                                                

                

                                                Licence_BocwAct_GeneralDetail formData = new Licence_BocwAct_GeneralDetail();
                                                if (masterData.Count() == 0)
                                                {
                                                    {
                                                        formData.BOCWActCircleType = 0;
                                                        formData.ConstructionBuildingType = 0;
                                                        formData.ConstructionBuildingDesc = "NA";
                                                        formData.BocwEngagedWorkerSlabType = 0;
                                                        formData.BocwRegisteredType = 0;
                                                        formData.Work_CommencementDate = DateTime.Now;
                                                        formData.Work_CompletionDate = DateTime.Now;
                                                        formData.MaximumNoOfWorkers = 0;
                                                        formData.AlcCircleRefId = 0;
                                                        formData.FactoryCircleRefId = 0;
                                                        formData.ModifiedCounter = 0;

                                                        formData.ConstructionSite_Name = "NA";
                                                        formData.ConstructionSite_Address = "No Address";
                                                        formData.DistrictLgdRefId = 1;
                                                        formData.TehsilLgdRefId = 1;
                                                        formData.ConstructionSite_PinCode = "0";

                                                        formData.PrincipalEmployerName = "No Address";
                                                        formData.PrincipalEmployerFatherName = "NA";
                                                        formData.PrincipalEmployerAddress = "NA";
                                                        formData.PrincipalEmployerMobileNo = "No Email";
                                                        formData.PrincipalEmployerEmail = "No Address";
                                                        formData.EngagedAnyContractorType = 0;

                                                        formData.ContractLabourLicenceNumber = "0000000000";

                                                        formData.ManagerName = "No Email";
                                                        formData.ManagerAddress = "No Address";
                                                        formData.ManagerEmail = "NA";
                                                        formData.ManagerMobile = "00";

                                                    };

                                                }
                                                else
                                                {
                                                    {
          
                                                        formData.BOCWActCircleType = (BOCWActCircleTypeEnum)(masterData.FirstOrDefault().BOCWCircleType == "BOCWALLCID" ? 2 : 1);
                                                        formData.ConstructionBuildingType = (ConstructionBuildingTypeEnum)(masterData.FirstOrDefault().BOCWCircleType == "BOCWALLCID" ? 2 : 1);
                                                        formData.ConstructionBuildingDesc = "NA" ;
                                                        formData.BocwEngagedWorkerSlabType = (BocwEngagedWorkerSlabTypeEnum)(masterData.FirstOrDefault().MaximumNumberOfWorkers > 11 ? 2 : 1);
                                                        formData.BocwRegisteredType = (BocwRegisteredTypeEnum)1;
                                                        formData.Work_CommencementDate = masterData.FirstOrDefault().Commencement_Date;
                                                        formData.Work_CompletionDate = masterData.FirstOrDefault().Completion_Date;
                                                        formData.MaximumNoOfWorkers = masterData.FirstOrDefault().MaximumNumberOfWorkers;
                                                        formData.AlcCircleRefId = masterData.FirstOrDefault()?.BOCWCircleType == "BOCWALLCID" ? applications.FirstOrDefault()?.ALLCCircleId ?? 0 : 0;
                                                        formData.FactoryCircleRefId = masterData.FirstOrDefault()?.BOCWCircleType == "BOCWFCID" ? applications.FirstOrDefault()?.FCId ?? 0 : 0;
                                                        formData.ModifiedCounter = 1;

                                                        formData.ConstructionSite_Name = (masterData.FirstOrDefault().EstablishmentName == null || masterData.FirstOrDefault().EstablishmentName == "") ? "NA" : masterData.FirstOrDefault().EstablishmentName;
                                                        formData.ConstructionSite_Address = (masterData.FirstOrDefault().EstablishmentAddress == null || masterData.FirstOrDefault().EstablishmentAddress == "") ? "NA" : masterData.FirstOrDefault().EstablishmentAddress;
                                                        formData.DistrictLgdRefId = 1;
                                                        formData.TehsilLgdRefId = 1;
                                                        formData.ConstructionSite_PinCode = (applications.FirstOrDefault()?.SiteAddPinCode == null || applications.FirstOrDefault()?.SiteAddPinCode == "") ? "000000" : applications.FirstOrDefault()?.SiteAddPinCode;

                                                        formData.PrincipalEmployerName = (masterData.FirstOrDefault().PrincipalEmpName == null || masterData.FirstOrDefault().PrincipalEmpName == "") ? "NA" : masterData.FirstOrDefault().PrincipalEmpName;
                                                        formData.PrincipalEmployerFatherName = "NA";
                                                        formData.PrincipalEmployerAddress = (masterData.FirstOrDefault().PrincipalEmpAddress == null || masterData.FirstOrDefault().PrincipalEmpAddress == "") ? "NA" : masterData.FirstOrDefault().PrincipalEmpAddress;
                                                        formData.PrincipalEmployerMobileNo = (masterData.FirstOrDefault().EstbEmployerMobile == null || masterData.FirstOrDefault().EstbEmployerMobile == "") ? "9999999999" : masterData.FirstOrDefault().EstbEmployerMobile;
                                                        formData.PrincipalEmployerEmail = (masterData.FirstOrDefault().EstbEmployerEmail == null || masterData.FirstOrDefault().EstbEmployerEmail =="") ? "abc@gmail.com" : masterData.FirstOrDefault().EstbEmployerEmail;
                                                        formData.EngagedAnyContractorType = (EngagedAnyContractorTypeEnum)2;

                                                        formData.ContractLabourLicenceNumber = "NA";

                                                        formData.ManagerName = (masterData.FirstOrDefault().ManagerName == null || masterData.FirstOrDefault().ManagerName == "") ? "NA" : masterData.FirstOrDefault().ManagerName;
                                                        formData.ManagerAddress = (masterData.FirstOrDefault().ManagerAddress == null || masterData.FirstOrDefault().ManagerAddress == "") ? "NA" : masterData.FirstOrDefault().ManagerAddress;
                                                        formData.ManagerEmail = (masterData.FirstOrDefault().ManagerEmail == null || masterData.FirstOrDefault().ManagerEmail == "") ? "abc@gmail.com" : masterData.FirstOrDefault().ManagerEmail;
                                                        formData.ManagerMobile = (masterData.FirstOrDefault().ManagerMobile == null || masterData.FirstOrDefault().ManagerMobile == "") ? "9999999999" : masterData.FirstOrDefault().ManagerMobile;

                                                    }
                                                    ;

                                                }

                                                var validations = CustomeValidator<Licence_BocwAct_GeneralDetail>.ValidateModel_AllProperties(formData);
                                                if (!validations.IsValid)
                                                {
                                                    var kk = validations.CustomeValidationErrorList;
                                                }

                                                var Sys_N_ServiceCodeInfo = FindServiceCodeMappingWithApplicationType(legacyAppFormId);
                                                var statusResp = await _iApplicationMamnagement_Licence_BocwAct_GeneralDetail.InitiateApplication(ApplicationTypeEnum.BOCW_ESTABLISHMENT_ACT, formData, projectSiteRefId, Sys_N_ServiceCodeInfo.ApplicationPurposeType, applications.FirstOrDefault().ApplicationUserID, Convert.ToInt64(ipin), Convert.ToInt64(investPunjabAppId), true, Convert.ToInt64(appId), legacyAppFormId, nar, null, 1);
                                                newAppId = statusResp.AppId;

                                              
                                                // Update seeding log after master creation
                                                seedingLogs = await GetApplicationSeedingLogsByAppId(Convert.ToInt64(appId), legacyAppFormId, nar);

                                                string query = "UPDATE ApplicationSeedingLogs SET NewAppId = " + newAppId + ", IsMasterSeeded = 1 WHERE Id = " + seedingLogs.Id.ToString();
                                                ExecuteQueryUsingAdoNet(query);
                                            }
                                         
                                        }
                                        #endregion

                                        if (newAppId > 0)
                                        {
                                            #region Business First Request Log
                                            if (seededInfo != null && seededInfo.IsBusinessRequestLogStatusSeeded == SeedStatusTypeEnum.NO_STATUS)
                                            {
                                                var projectsiteid = _context.Applications.Where(x => x.AppId == newAppId && x.IsDeleted == false).FirstOrDefault().ProjectSiteRefId;
                                                var applicationuserId = _context.ProjectSites.Where(x => x.ProjectSiteId == projectsiteid).FirstOrDefault().UserRefId; 
                                                BusinessFirst_RequestLog businessFirst_Request = new BusinessFirst_RequestLog()
                                                {
                                                    AppId = Convert.ToInt64(investPunjabAppId),
                                                    IPin = Convert.ToInt64(ipin),
                                                    UserId = applicationuserId,
                                                    ServiceCode = legacyAppFormId,
                                                    CategoryTypeId = 1,
                                                    RequestCount = 1,
                                                    NativeAppId = newAppId,
                                                    CreatedDate = DateTime.Now,
                                                    LastModifiedDate = DateTime.Now,
                                                    RequestString = "",
                                                    IsEnabled = true,
                                                    NativeUserId = null
                                                };

                                                // Add the mapping to the context
                                                _context.BusinessFirst_RequestLogs.Add(businessFirst_Request);
                                                _context.SaveChanges();

                                                seedingLogs = await GetApplicationSeedingLogsByAppId(Convert.ToInt64(appId), legacyAppFormId, nar).ConfigureAwait(false);
                                                ExecuteQueryUsingAdoNet("UPDATE ApplicationSeedingLogs SET IsBusinessRequestLogStatusSeeded = 1 WHERE Id=" + seedingLogs.Id.ToString());

                                                
                                            }
                                            #endregion


                                            if (seededInfo != null && seededInfo.IsDocumentSeeded == SeedStatusTypeEnum.NO_STATUS)
                                            {
                                                #region Documents
                                                var legacyAppDocuments = await _iDapperRepository.Get<LegacyAppDocumentViewModel>("SELECT DISTINCT DocId,FileName,TDate FROM AppDocuments where AppId = @AppId AND NAR = @NAR UNION SELECT DISTINCT DocId,FileName,TDate FROM AppDocuments_Log where AppId = @AppId AND NAR = @NAR", new { AppId = appId, NAR = nar }, CommandType.Text).ConfigureAwait(true);
                                                foreach (var doc in legacyAppDocuments)
                                                {
                                                    // Check if DocId is null (nullable int) or if FileName is null or empty
                                                    if (doc.DocId == 0 || string.IsNullOrEmpty(doc.FileName))
                                                    {
                                                        continue;  // Skip the current iteration and move to the next document
                                                    }

                                                    ApplicationDocument appDocument = new ApplicationDocument()
                                                    {
                                                        AppRefId = newAppId,
                                                        AttachmentName = doc.FileName,
                                                        DocumentRefId = doc.DocId,
                                                        IsUploaded = true,
                                                        LastModifiedDate = doc.TDate,
                                                        Uploadeddate = doc.TDate,
                                                        IsLocked = true
                                                    };

                                                    _iGR_AppDocument.Insert(appDocument);
                                                    _iGR_AppDocument.Savechange();

                                                    string filename = string.Empty;

                                                    // Create the filename based on DocId and AppDocId
                                                    filename = doc.FileName.Substring(doc.FileName.LastIndexOf('.'));
                                                    filename = newAppId.ToString() + "_" + doc.DocId.ToString() + "_" + appDocument.AppDocId + filename;

                                                    var foundDocument = _iGR_AppDocument.GetById(appDocument.AppDocId);
                                                    foundDocument.AttachmentName = filename.ToLower();

                                                    _iGR_AppDocument.Update(foundDocument);
                                                    _iGR_AppDocument.Savechange();

                                                    LegacyDocumentMapping legacyDocumentMapping = new LegacyDocumentMapping()
                                                    {
                                                        Legacy_DocumentName = doc.FileName,
                                                        Legacy_DocumentId = doc.DocId,
                                                        NewDocumentName = filename,
                                                        Legacy_AppId = Convert.ToInt64(appId),
                                                        Legacy_AppFormId = legacyAppFormId,
                                                        Legacy_NAR = nar,
                                                        AppRefId = newAppId,
                                                        IsMoved = false,
                                                        IsApproval = false
                                                    };

                                                    // Add the mapping to the context
                                                    _context.LegacyDocumentMappings.Add(legacyDocumentMapping);
                                                    _context.SaveChanges();
                                                }

                                                seedingLogs = await GetApplicationSeedingLogsByAppId(Convert.ToInt64(appId), legacyAppFormId, nar).ConfigureAwait(false);
                                                ExecuteQueryUsingAdoNet("UPDATE ApplicationSeedingLogs SET IsDocumentSeeded = 1 WHERE Id=" + seedingLogs.Id.ToString());

                                                #endregion
                                            }

                                            if (seededInfo != null && seededInfo.IsPaymentSeeded == SeedStatusTypeEnum.NO_STATUS)
                                            {
                                                #region Payment

                                                List<StoreProcedureParm> storeProcedureParms = new List<StoreProcedureParm>()
                                            {
                                                new StoreProcedureParm (){ ParmName="AppId", ParmValue=appId, isNumber=true},
                                                new StoreProcedureParm (){ ParmName="NAR", ParmValue=nar, isNumber=false}
                                            };

                                                var legacyPayments = await _iGeneric_SP_Repository.CallStoreProcedureReaderAsync<LegacyAppPaymentDetailsViewModel>("dbo.sp_GetLegacyPaymentDetailByAppId", storeProcedureParms).ConfigureAwait(false);

                                                if (legacyPayments.Count() > 0)
                                                {
                                                    AppFeeDetail appFeeDetail = new AppFeeDetail()
                                                    {
                                                        AppRefId = newAppId,
                                                        FeeHeaderRefId = 35,
                                                        Amount = legacyPayments.FirstOrDefault().TransactionAmount,
                                                        CalculatedOn = legacyPayments.FirstOrDefault().TransactionDate,
                                                        IsDeduductible = false,
                                                        PaymentBatchCounter = 1,
                                                        PaymentPartCounter = 0,
                                                        HasDedicatedTreasuryCode = false,
                                                        DedicatedTreasurCode = null,
                                                        DedicatedDDOCode = null,
                                                        Description = null,
                                                    };
                                                    _context.AppFeeDetails.Add(appFeeDetail);
                                                    _context.SaveChanges();

                                                    //_iGR_AppFeeDetail

                                                    //Step - 4. Insert into AppFeeTransaction
                                                    AppFeeTransaction appFeeTransaction = new AppFeeTransaction()
                                                    {
                                                        TransactionInitializationDate = legacyPayments.FirstOrDefault().PaymentDate,
                                                        PaymentGatewayType = PaymentGatewayTypeEnum.IFMS,
                                                        PaymentModeType = PaymentModeTypeEnum.ONLINE,
                                                        PaymentTreasuryType = PaymentTreasuryTypeEnum.NON_TREASURY,
                                                        PaymentGatewayTargetUrl = "NA",
                                                        PaymentGatewayApiMethodType = PaymentGatewayApiMethodTypeEnum.POST,
                                                        UniquePaymentGatewayTransactionId = legacyPayments.FirstOrDefault().TransactionId == null ? legacyPayments.FirstOrDefault().BankTransactionRefNumber : legacyPayments.FirstOrDefault().TransactionId,
                                                        IsWebRequestCycleCompleted = true,
                                                        RequestBodyData = legacyPayments.FirstOrDefault().ReqMsg,
                                                        ResponseBodyData = legacyPayments.FirstOrDefault().ResMsg,
                                                        ResponseReceivedOn = legacyPayments.FirstOrDefault().PaymentDate,
                                                        ResponseMessage = legacyPayments.FirstOrDefault().ResMsg,
                                                        TransactionFinalStatusType = TransactionFinalStatusTypeEnum.SUCCEED,
                                                        AmountCalculated = legacyPayments.FirstOrDefault().TransactionAmount,
                                                        PaymentBatchCounter = 1,
                                                        PaymentPartCounter = 1,
                                                        BankTransactionRefNumber1 = legacyPayments.FirstOrDefault().BankTransactionRefNumber,
                                                        BankTransactionRefNumber1_Type = BankTransactionReferenceTypeEnum.BANK_REF_NUMBER,
                                                        BankTransactionRefNumber2 = legacyPayments.FirstOrDefault().BankTransactionRefNumber,
                                                        BankTransactionRefNumber2_Type = BankTransactionReferenceTypeEnum.BANK_REF_NUMBER,
                                                        BankSettlementOn = legacyPayments.FirstOrDefault().PaymentDate,
                                                        AppRefId = newAppId
                                                    };
                                                    _context.AppFeeTransactions.Add(appFeeTransaction);
                                                    _context.SaveChanges();

                                                    //step - 5. Insert into SuccessMapping Table
                                                    AppPaymentSuccessTransactionMapping appPaymentSuccessTransactionMapping = new AppPaymentSuccessTransactionMapping()
                                                    {
                                                        AppRefId = newAppId,
                                                        AppFeeTransactionRefId = appFeeTransaction.AppFeeTransactionId,
                                                        PaymentBatchCounter = 1,
                                                        PaymentPartCounter = 1,
                                                    };
                                                    _context.AppPaymentSuccessTransactionMappings.Add(appPaymentSuccessTransactionMapping);
                                                    _context.SaveChanges();

                                                    // Update seeding log after payment seeded
                                                    seedingLogs = await GetApplicationSeedingLogsByAppId(Convert.ToInt64(appId), legacyAppFormId, nar);
                                                    ExecuteQueryUsingAdoNet("UPDATE ApplicationSeedingLogs SET IsPaymentSeeded = 1 WHERE Id=" + seedingLogs.Id.ToString());
                                                }
                                                else // Fee Not Found In Db
                                                {
                                                    seedingLogs = await GetApplicationSeedingLogsByAppId(Convert.ToInt64(appId), legacyAppFormId, nar);
                                                    ExecuteQueryUsingAdoNet("UPDATE ApplicationSeedingLogs SET IsPaymentSeeded = 3 WHERE Id=" + seedingLogs.Id.ToString());

                                                }

                                                #endregion
                                            }

                                            if (seededInfo != null && (seededInfo.IsApplicationLogsSeeded == SeedStatusTypeEnum.NO_STATUS || seededInfo.IsApplicationLogsSeeded == SeedStatusTypeEnum.ERROR))
                                            {
                                                try
                                                {

                                                    ////remove application action logs
                                                    //var applogs = _context.ApplicationActionLogs.Where(x => x.ApplicationRefId == newAppId).ToList();
                                                    //if (applogs.Any())
                                                    //{
                                                    //    _context.ApplicationActionLogs.RemoveRange(applogs);
                                                    //    _context.SaveChanges();
                                                    //}


                                                    #region ApplicationLogs

                                                    List<ApplicationActionLog> actionLogList = new List<ApplicationActionLog>();

                                                    // Step 1 : Grouping the unique logs
                                                    var distinctLogs = allLogs.GroupBy(log => new { log.StatusId, log.StatusDate }).Select(group => group.First()).ToList();

                                                    // Step 1: Separate logs with StatusId = 1 and StatusId = 2
                                                    var statusId1And2Logs = distinctLogs.Where(log => log.StatusId == 1 || log.StatusId == 2).OrderBy(log => log.StatusId).ToList();

                                                    // Step 2: Separate logs with StatusId other than 1 and 2
                                                    var otherLogs = distinctLogs.Where(log => log.StatusId != 1 && log.StatusId != 2).ToList();

                                                    // Step 3: Combine the two lists, with logs having StatusId = 1 and 2 coming first
                                                    var orderedLogs = statusId1And2Logs.Concat(otherLogs).ToList();

                                                    foreach (var log in orderedLogs.Where(x => x.NAR == nar).ToList())
                                                    {
                                                        var receiverRoleId = await _iDapperRepository.Get<string>("SELECT RoleId FROM AspNetUserRoles WHERE UserId =@UserId", new { UserId = log.Receiver }, CommandType.Text).ConfigureAwait(false);
                                                        var senderRoleId = await _iDapperRepository.Get<string>("SELECT RoleId FROM AspNetUserRoles WHERE UserId =@UserId", new { UserId = log.Sender }, CommandType.Text).ConfigureAwait(false);
                                                        var senderProfile = await _iAuthService.GetUserProfileByUserRefId(log.Sender).ConfigureAwait(false);
                                                        var receiverProfile = await _iAuthService.GetUserProfileByUserRefId(log.Receiver).ConfigureAwait(false);

                                                        ApplicationActionLog actionLog = new ApplicationActionLog();
                                                        actionLog.ActionDate = log.StatusDate;
                                                        actionLog.ActionTakenDaysCount = 0;
                                                        actionLog.ActionTakenHoursCount = 0;
                                                        actionLog.AppActionType = GetSys_N_ActionCode(Convert.ToInt32(log.StatusId));
                                                        actionLog.ApplicationRefId = newAppId;
                                                        actionLog.Receiver_ProfileRefId = receiverProfile == null ? 404 : receiverProfile.UserProfileRefId;
                                                        actionLog.Receiver_UserRefId = log.Receiver;
                                                        actionLog.Sender_ProfileRefId = senderProfile == null ? 404 : senderProfile.UserProfileRefId;
                                                        actionLog.Sender_UserRefId = log.Sender;
                                                        actionLog.Remarks = log.StatusDesc;
                                                        actionLog.ReceiverRoleId = new long[] { 5, 9, 206 }.Contains(log.StatusId) ? "592add3e-f992-4983-a8ca-21ddc090bda0" : receiverRoleId.FirstOrDefault().ToString();
                                                        actionLog.SenderRoleId = new long[] { 1, 2, 3 }.Contains(log.StatusId) ? "592add3e-f992-4983-a8ca-21ddc090bda0" : senderRoleId.FirstOrDefault().ToString();
                                                        actionLog.AppDocumentRefId = 0;
                                                        actionLog.IsDocumentUploaded = false;
                                                        actionLog.Legacy_IsMigrated = true;
                                                        actionLog.Legacy_AppId = Convert.ToInt64(appId);
                                                        actionLog.Legacy_AppFormId = legacyAppFormId;
                                                        actionLog.Legacy_NAR = nar;
                                                        actionLog.Legacy_StatusId = Convert.ToInt32(log.StatusId);
                                                        actionLog.IpAddress = "Manual";
                                                        actionLog.Latitude = "Manual";
                                                        actionLog.Longitude = "Manual";
                                                        actionLogList.Add(actionLog);
                                                    }
                                                    _context.BulkInsert<ApplicationActionLog>(actionLogList);

                                                    var actionSenderProfile = await _iAuthService.GetUserProfileByUserRefId(actionLogList.LastOrDefault().Sender_UserRefId).ConfigureAwait(false);
                                                    var actionReceiverProfile = await _iAuthService.GetUserProfileByUserRefId(actionLogList.LastOrDefault().Receiver_UserRefId).ConfigureAwait(false);


                                                    if (actionSenderProfile == null)
                                                    {
                                                        List<StoreProcedureParm> storeProcedureParms = new List<StoreProcedureParm>()
                                                    {
                                                        new StoreProcedureParm (){ ParmName="AspNetUserId", ParmValue=actionLogList.LastOrDefault().Sender_UserRefId, isNumber=false}
                                                    };

                                                        var userdata = await _iGeneric_SP_Repository.CallStoreProcedureReaderScalerAsync("dbo.sp_SeedLegacyUsersToUpgradedPortal", storeProcedureParms).ConfigureAwait(false);
                                                        actionSenderProfile = await _iAuthService.GetUserProfileByUserRefId(actionLogList.LastOrDefault().Sender_UserRefId).ConfigureAwait(false);
                                                    }

                                                    if (actionReceiverProfile == null)
                                                    {
                                                        List<StoreProcedureParm> storeProcedureParms = new List<StoreProcedureParm>()
                                                    {
                                                        new StoreProcedureParm (){ ParmName="AspNetUserId", ParmValue=actionLogList.LastOrDefault().Receiver_UserRefId, isNumber=false}
                                                    };

                                                        var userdata = await _iGeneric_SP_Repository.CallStoreProcedureReaderScalerAsync("dbo.sp_SeedLegacyUsersToUpgradedPortal", storeProcedureParms).ConfigureAwait(false);
                                                        actionReceiverProfile = await _iAuthService.GetUserProfileByUserRefId(actionLogList.LastOrDefault().Receiver_UserRefId).ConfigureAwait(false);
                                                    }
                                                    var applicationAction = _context.ApplicationActions.Where(x => x.ApplicationRefId == newAppId).FirstOrDefault();
                                                    if (applicationAction != null)
                                                    {
                                                        applicationAction.ActionDate = actionLogList.LastOrDefault().ActionDate;
                                                        applicationAction.ActionTakenDaysCount = 0;
                                                        applicationAction.ActionTakenHoursCount = 0;
                                                        applicationAction.AppActionType = actionLogList.LastOrDefault().AppActionType;
                                                        applicationAction.ApplicationRefId = actionLogList.LastOrDefault().ApplicationRefId;
                                                        applicationAction.Receiver_ProfileRefId = actionLogList.LastOrDefault().Receiver_UserRefId == "0c54c0ad-a4c7-42b0-ab4a-4e524c2addf" ? 5 :
                                                                                actionLogList.LastOrDefault().Receiver_UserRefId == "1aee535c-4794-4e7b-acaa-e5a0acf4af33" ? 12 :
                                                                                actionReceiverProfile.UserProfileRefId;

                                                        applicationAction.Receiver_UserRefId = actionLogList.LastOrDefault().Receiver_UserRefId == "0c54c0ad-a4c7-42b0-ab4a-4e524c2addf" ? "C29E5FD2-8FB9-49AC-BCF1-7E06769FAAD9" :
                                                                                actionLogList.LastOrDefault().Receiver_UserRefId == "1aee535c-4794-4e7b-acaa-e5a0acf4af33" ? "C29E5FD2-8FB9-49AC-BCF1-7E06769FAAD9" :
                                                                                actionLogList.LastOrDefault().Receiver_UserRefId;

                                                        applicationAction.Sender_ProfileRefId = actionLogList.LastOrDefault().Sender_UserRefId == "0c54c0ad-a4c7-42b0-ab4a-4e524c2addf" ? 5 :
                                                                                actionLogList.LastOrDefault().Sender_UserRefId == "1aee535c-4794-4e7b-acaa-e5a0acf4af33" ? 12 :
                                                                                actionSenderProfile.UserProfileRefId;

                                                        applicationAction.Sender_UserRefId = actionLogList.LastOrDefault().Sender_UserRefId == "0c54c0ad-a4c7-42b0-ab4a-4e524c2addf" ? "C29E5FD2-8FB9-49AC-BCF1-7E06769FAAD9" :
                                                                            actionLogList.LastOrDefault().Sender_UserRefId == "1aee535c-4794-4e7b-acaa-e5a0acf4af33" ? "C29E5FD2-8FB9-49AC-BCF1-7E06769FAAD9" :
                                                                            actionLogList.LastOrDefault().Sender_UserRefId;

                                                        applicationAction.Remarks = actionLogList.LastOrDefault().Remarks;
                                                        applicationAction.ReceiverRoleId = actionLogList.LastOrDefault().ReceiverRoleId;
                                                        applicationAction.SenderRoleId = actionLogList.LastOrDefault().SenderRoleId;
                                                        applicationAction.AppDocumentRefId = 0;
                                                        applicationAction.IsDocumentUploaded = false;
                                                        applicationAction.Legacy_IsMigrated = true;
                                                        applicationAction.Legacy_AppId = actionLogList.LastOrDefault().Legacy_AppId;
                                                        applicationAction.Legacy_AppFormId = actionLogList.LastOrDefault().Legacy_AppFormId;
                                                        applicationAction.Legacy_NAR = actionLogList.LastOrDefault().Legacy_NAR;
                                                        applicationAction.Legacy_StatusId = actionLogList.LastOrDefault().Legacy_StatusId;
                                                        applicationAction.IpAddress = "Manual";
                                                        applicationAction.Latitude = "Manual";
                                                        applicationAction.Longitude = "Manual";

                                                        _context.ApplicationActions.Update(applicationAction);
                                                        _context.SaveChanges();

                                                    };
                                                  


                                                    //update application life cycle
                                                    var application = _context.Applications.Where(x => x.AppId == newAppId && x.IsDeleted == false).FirstOrDefault();
                                                    int appLifeCycleStatusType = _context.AppActionTypeMappings.Where(x => x.NewAppActionType == applicationAction.AppActionType).Select(x => x.ApplicationLifeCycleStatusType).FirstOrDefault();

                                                    application.ApplicationLifeCycleStatusType = (ApplicationLifeCycleStatusTypeEnum)appLifeCycleStatusType;
                                                    application.ApplicationLifeCycleLastStatusOn = actionLogList.LastOrDefault().ActionDate;
                                                    application.CreatedOnDate = actionLogList.FirstOrDefault().ActionDate;
                                                    application.IsIPIntegrated = ipin == "0" ? false:true;
                                                    application.IsTimeLineFlow = false;
                                                    _context.Update<Application>(application);
                                                    _context.SaveChanges();

                                                    // Update seeding log after logs seeded
                                                    seedingLogs = await GetApplicationSeedingLogsByAppId(Convert.ToInt64(appId), legacyAppFormId, nar);
                                                    ExecuteQueryUsingAdoNet("UPDATE ApplicationSeedingLogs SET IsApplicationLogsSeeded = 1 WHERE Id=" + seedingLogs.Id.ToString());
                                                    #endregion
                                                }
                                                catch (Exception ex1)
                                                {
                                                    seedingLogs = await GetApplicationSeedingLogsByAppId(Convert.ToInt64(appId), legacyAppFormId, nar);
                                                    ExecuteQueryUsingAdoNet("UPDATE ApplicationSeedingLogs SET IsApplicationLogsSeeded = 4 WHERE Id=" + seedingLogs.Id.ToString());
                                                    continue;
                                                }
                                            }

                                            if (seededInfo != null && seededInfo.IsApprovalsSeeded == SeedStatusTypeEnum.NO_STATUS)
                                            {
                                                #region Approvals
                                                var actionlogs = _context.ApplicationActions.Where(x => x.ApplicationRefId == newAppId).FirstOrDefault();
                                                if (actionlogs != null && (actionlogs.AppActionType == (int)AppActionTypeEnum.APP_APPROVED || actionlogs.AppActionType == (int)AppActionTypeEnum.DEEMED_COMPLETED))
                                                {
                                                    var legacyApprovals = await _iDapperRepository.Get<LegacyAppClearanceIssuedViewModel>("SELECT * FROM AppClearanceIssueds WHERE AppId = @AppId AND AppFormId = @AppFormId AND NAR = @NAR ORDER BY 1 DESC", new { AppId = appId, AppFormId = legacyAppFormId, NAR = nar }, CommandType.Text).ConfigureAwait(false);

                                                    if (legacyApprovals.Count() > 0)
                                                    {
                                                        var app = _context.Applications.Where(x => x.AppId == newAppId && x.IsDeleted == false).FirstOrDefault();

                                                        string qry = @"INSERT INTO [dbo].[LegacyDocumentMappings]([Legacy_DocumentName],[Legacy_DocumentId],[NewDocumentName],[Legacy_AppId],[Legacy_AppFormId],[Legacy_NAR],[AppRefId],[IsMoved],[IsApproval]) VALUES("
                                                                     + "'" + legacyApprovals.FirstOrDefault().LicencePath + "'"
                                                                     + ",'" + legacyApprovals.FirstOrDefault().DocId + "'"
                                                                     + ",'" + app.PublicAppRefNum + ".pdf" + "'"
                                                                     + "," + appId
                                                                     + "," + legacyAppFormId
                                                                     + ",'" + nar + "'"
                                                                     + "," + newAppId
                                                                     + ",0"
                                                                     + ",1)";

                                                        ExecuteQueryUsingAdoNet(qry);

                                                        ExecuteQueryUsingAdoNet("INSERT INTO ApplicationLicenceNoMapping (AppRefId, LicenceNumber) VALUES(" + newAppId + "," + "'" + legacyApprovals.FirstOrDefault().LicenceNo + "'" + ")");
                                                    }
                                                    else
                                                    {
                                                        seedingLogs = await GetApplicationSeedingLogsByAppId(Convert.ToInt64(appId), legacyAppFormId, nar).ConfigureAwait(false);
                                                        ExecuteQueryUsingAdoNet("UPDATE ApplicationSeedingLogs SET IsApprovalsSeeded = 3 WHERE Id=" + seedingLogs.Id.ToString());
                                                    }
                                                }
                                                else
                                                {

                                                }

                                                // Update seeding log after approval seeded
                                                seedingLogs = await GetApplicationSeedingLogsByAppId(Convert.ToInt64(appId), legacyAppFormId, nar).ConfigureAwait(false);
                                                ExecuteQueryUsingAdoNet("UPDATE ApplicationSeedingLogs SET IsApprovalsSeeded = 1 WHERE Id=" + seedingLogs.Id.ToString());

                                                #endregion
                                            }

                                            // All the steps of data seeding is completed
                                            seedingLogs = await GetApplicationSeedingLogsByAppId(Convert.ToInt64(appId), legacyAppFormId, nar).ConfigureAwait(false);
                                            int allStepsCompleted = 0;
                                            if ((seedingLogs.IsUserSeeded == SeedStatusTypeEnum.SEEDED || seedingLogs.IsUserSeeded == SeedStatusTypeEnum.NO_SEEDING_REQUIRED)
                                                && seedingLogs.IsProfileSeeded == SeedStatusTypeEnum.SEEDED
                                                && seedingLogs.IsProjectSiteSeeded == SeedStatusTypeEnum.SEEDED
                                                && seedingLogs.IsDocumentSeeded == SeedStatusTypeEnum.SEEDED
                                                && seedingLogs.IsPaymentSeeded == SeedStatusTypeEnum.SEEDED
                                                && seedingLogs.IsApplicationLogsSeeded == SeedStatusTypeEnum.SEEDED
                                                && seedingLogs.IsApprovalsSeeded == SeedStatusTypeEnum.SEEDED
                                                && seedingLogs.IsBusinessRequestLogStatusSeeded == SeedStatusTypeEnum.SEEDED)
                                            {
                                                allStepsCompleted = 1;
                                            }

                                            string query = "UPDATE ApplicationSeedingLogs SET OverAllSeedStatus = " + allStepsCompleted + ", SeedStatusDiscription = 'All Steps Completed' WHERE Id = " + seedingLogs.Id.ToString();
                                            ExecuteQueryUsingAdoNet(query);
                                        }
                                        else
                                        {
                                            seedingLogs = await GetApplicationSeedingLogsByAppId(Convert.ToInt64(appId), legacyAppFormId, nar);
                                            ExecuteQueryUsingAdoNet("UPDATE ApplicationSeedingLogs SET OverAllSeedStatus = 0,SeedStatusDiscription ='No new app available'  WHERE Id=" + seedingLogs.Id.ToString());
                                        }
                                    }
                                }
                                catch (Exception ex)
                                {
                                    var foundSeededInfo = await GetApplicationSeedingLogsByAppId(Convert.ToInt64(appId), legacyAppFormId, nar);
                                    var msg = ex.InnerException != null ? Regex.Replace(ex.InnerException.ToString(), @"'", "") : Regex.Replace(ex.Message, @"'", "");

                                    List<StoreProcedureParm> storeProcedureParms = new List<StoreProcedureParm>()
                                {
                                    new StoreProcedureParm (){ ParmName="Id", ParmValue= foundSeededInfo.Id.ToString(), isNumber=true },
                                    new StoreProcedureParm (){ ParmName="ExceptionText", ParmValue= msg, isNumber=false }
                                };
                                    var resp = await _iGeneric_SP_Repository.CallStoreProcedureReaderScalerAsync("dbo.sp_updateAppSeedingLogsError", storeProcedureParms);

                                    continue;
                                }
                            }
                        }
                    }
                    //}
                }
            }
            catch (Exception ex)
            {
                genericServiceResultTemplate.HasError = true;
                genericServiceResultTemplate.ErrorDesc = ex.Message;
                throw ex;
            }
            return genericServiceResultTemplate;
        }



        public async Task<GenericResponseTemplateModel<List<IPinInfoViewModel>>> SeedApplicationDataByLegacyAppIdId(int legacyAppFormId , Int64 appRefId)
        {
            GenericResponseTemplateModel<List<IPinInfoViewModel>> genericServiceResultTemplate = new GenericResponseTemplateModel<List<IPinInfoViewModel>>();
            try
            {
                var distinctLegacyAppId = await _iDapperRepository.Get<string>("SELECT DISTINCT AppId FROM PbLabour.dbo.AppPBIPAction_Log WHERE AppFormId = @AppFormId and AppId = @AppId", new { AppFormId = legacyAppFormId, AppId = appRefId }, CommandType.Text).ConfigureAwait(false);
                ApplicationAction action = null;

                User user = null;
                foreach (var appId in distinctLegacyAppId)
                {
                    //if (_context.ApplicationSeedingLogs.Where(x => x.Legacy_AppFormId == legacyAppFormId && x.Legacy_AppId == Convert.ToInt64(appId)).Select(x => x.SeedStatusDiscription).FirstOrDefault() != "All Steps Completed")
                    //{
                    using (var workbook = new XLWorkbook())
                    {
                        ApplicationSeedingLog seedingLogs = new ApplicationSeedingLog();
                        // Get all the logs from legacy portal
                        var allLogs = await _iDapperRepository.Get<LegacyLogsViewModel>("SELECT * FROM AppPBIPAction_Log WHERE AppFormId = @AppFormId AND AppId = @AppId ORDER BY AppPBIPActionLogId ASC", new { AppFormId = legacyAppFormId, AppId = appId }, CommandType.Text).ConfigureAwait(false);
                        var distinctNAR = allLogs.Select(x => x.NAR).Distinct();
                        Int64 newAppId = 0;
                        foreach (var nar in distinctNAR)
                        {
                            if (!_context.ApplicationSeedingLogs.Any(x =>
                                x.Legacy_AppId == Convert.ToInt64(appId) &&
                                x.Legacy_AppFormId == legacyAppFormId &&
                                x.Legacy_NAR == nar &&
                                x.SeedStatusDiscription == "All Steps Completed"))
                            {
                                try
                                {
                                    var seededInfo = await GetApplicationSeedingLogsByAppId(Convert.ToInt64(appId), legacyAppFormId, nar);
                                    if (seededInfo == null)
                                    {
                                        string qry = @"INSERT INTO [dbo].[ApplicationSeedingLogs]([Legacy_AppId],[Legacy_AppFormId],[Legacy_NAR],[IsMasterSeeded],[IsDocumentSeeded],[IsPaymentSeeded],[IsApplicationLogsSeeded],[IsApprovalsSeeded],[OverAllSeedStatus],[SeedStatusDiscription],[IsProfileSeeded],[IsProjectSiteSeeded],[IsUserSeeded],[NewAppId],[IsBusinessRequestLogStatusSeeded]) VALUES("
                                                        + appId
                                                       + "," + legacyAppFormId
                                                       + ",'" + nar + "'"
                                                       + ",0"
                                                       + ",0"
                                                       + ",0"
                                                       + ",0"
                                                       + ",0"
                                                       + ",0"
                                                       + ",0"
                                                       + ",''"
                                                       + ",0"
                                                       + ",0"
                                                       + ",0"
                                                       + ",0)";

                                        ExecuteQueryUsingAdoNet(qry);
                                    }

                                    seededInfo = await GetApplicationSeedingLogsByAppId(Convert.ToInt64(appId), legacyAppFormId, nar).ConfigureAwait(false);
                                    if (seededInfo != null && !seededInfo.OverAllSeedStatus)
                                    {
                                        #region User & Profile

                                        // Find UserId In old Database
                                        var applications = await _iDapperRepository.Get<LegacyApplicationViewModel>("SELECT AP.*, SD.DistrictLgdId, ST.LGDirTehsilCode FROM Applications AP INNER JOIN ShopTehsils ST ON AP.SiteAddTehsilId = ST.TehsilId INNER JOIN PbLabourCoreNetDb.dbo.districts SD ON ST.LGDistrictCode = SD.DistrictLgdId WHERE AppId = @AppId", new { AppId = appId }, CommandType.Text).ConfigureAwait(false);
                                        if (applications.Count() > 0 && applications != null)
                                        {
                                            var userDetails = await _iDapperRepository.Get<LegacyUserViewModel>("SELECT UserName, ClientPasswdHash, PasswordHash, SecurityStamp,PhoneNumber,Email FROM AspNetUsers WHERE Id = @ApplicationUserID", new { ApplicationUserID = applications.FirstOrDefault().ApplicationUserID }, CommandType.Text).ConfigureAwait(false);

                                            if (_context.Users.Count(x => x.Id == applications.FirstOrDefault().ApplicationUserID.ToString()) > 0) //User Found in new DB
                                            {
                                                // Update the SeedStatusType
                                                seedingLogs = await GetApplicationSeedingLogsByAppId(Convert.ToInt64(appId), legacyAppFormId, nar).ConfigureAwait(false);

                                                ExecuteQueryUsingAdoNet("UPDATE ApplicationSeedingLogs SET IsUserSeeded = 2 WHERE Id=" + seedingLogs.Id.ToString());
                                                ExecuteQueryUsingAdoNet("UPDATE ApplicationSeedingLogs SET IsProfileSeeded = 1 WHERE Id=" + seedingLogs.Id.ToString());
                                            }
                                            else //User Not Found in new DB
                                            {
                                                if (seededInfo != null && seededInfo.IsUserSeeded == SeedStatusTypeEnum.NO_STATUS) // Create New User
                                                {
                                                    user = new User()
                                                    {
                                                        AccessFailedCount = 0,
                                                        ConcurrencyStamp = "",
                                                        Email = userDetails.FirstOrDefault().Email,
                                                        EmailConfirmed = true,
                                                        Id = applications.FirstOrDefault().ApplicationUserID.ToString(),
                                                        LockoutEnabled = false,
                                                        LockoutEnd = null,
                                                        NormalizedEmail = applications.FirstOrDefault().Email,
                                                        NormalizedUserName = userDetails.FirstOrDefault().UserName,
                                                        PasswordHash = userDetails.FirstOrDefault().PasswordHash,
                                                        PhoneNumber = userDetails.FirstOrDefault().PhoneNumber,
                                                        PhoneNumberConfirmed = true,
                                                        SecurityStamp = userDetails.FirstOrDefault().SecurityStamp,
                                                        TwoFactorEnabled = false,
                                                        UserName = userDetails.FirstOrDefault().UserName,
                                                        IsEnabled = false,
                                                        IsTestUser = false
                                                    };
                                                    await _context.AddAsync<User>(user);
                                                    await _context.SaveChangesAsync();

                                                    List<StoreProcedureParm> storeProcedureParms = new List<StoreProcedureParm>()
                                                {
                                                    new StoreProcedureParm (){ ParmName="RoleId", ParmValue="592add3e-f992-4983-a8ca-21ddc090bda0", isNumber=false},
                                                    new StoreProcedureParm (){ ParmName="UserId", ParmValue=user.Id, isNumber=false}
                                                };
                                                    await _iGeneric_SP_Repository.CallStoreProcedureReaderScalerAsync("dbo.sp_MapUserWithRole", storeProcedureParms).ConfigureAwait(false);

                                                    seedingLogs = await GetApplicationSeedingLogsByAppId(Convert.ToInt64(appId), legacyAppFormId, nar).ConfigureAwait(false);
                                                    ExecuteQueryUsingAdoNet("UPDATE ApplicationSeedingLogs SET IsUserSeeded = 1 WHERE Id=" + seedingLogs.Id.ToString());

                                                }

                                                if (seededInfo != null && seededInfo.IsProfileSeeded == SeedStatusTypeEnum.NO_STATUS) // // Create New User-Profile
                                                {
                                                    UserProfile userProfile = new UserProfile()
                                                    {
                                                        FirstName = applications.FirstOrDefault().FirstName,
                                                        MiddleName = applications.FirstOrDefault().MiddleName,
                                                        LastName = applications.FirstOrDefault().LastName,
                                                        FatherName = "NA",
                                                        MobileNo = applications.FirstOrDefault().MobileNo.Substring(applications.FirstOrDefault().MobileNo.Length - 10),
                                                        AlternateMobileNo = applications.FirstOrDefault().MobileNo.Substring(applications.FirstOrDefault().MobileNo.Length - 10),
                                                        Email = applications.FirstOrDefault().Email,
                                                        AlternateEmail = applications.FirstOrDefault().Email,
                                                        TehsilId = (Int64)applications.FirstOrDefault().LGDirTehsilCode,
                                                        DistrictId = applications.FirstOrDefault().DistrictLgdId,
                                                        State = applications.FirstOrDefault().StateId.ToString(),
                                                        PinCode = applications.FirstOrDefault().PinCode == null ? "000000" : applications.FirstOrDefault().PinCode.Substring(applications.FirstOrDefault().PinCode.Length - 6),
                                                        Signature = "NA",
                                                        ProfilePhoto = "NA",
                                                        IsActive = true,
                                                        IsDeleted = false,
                                                        Createddate = DateTime.Now,
                                                        LastModifiedDate = DateTime.Now
                                                    };

                                                    var userProfileResponse = await _iAuthService.CreateNewUserProfile(userProfile);
                                                    await _iAuthService.MapUserWithProfile(user.Id, userProfile.UserProfileId).ConfigureAwait(false);

                                                    seedingLogs = await GetApplicationSeedingLogsByAppId(Convert.ToInt64(appId), legacyAppFormId, nar);
                                                    ExecuteQueryUsingAdoNet("UPDATE ApplicationSeedingLogs SET IsProfileSeeded = 1 WHERE Id=" + seedingLogs.Id.ToString());

                                                }
                                            }
                                        }
                                        else
                                        {
                                            seedingLogs = await GetApplicationSeedingLogsByAppId(Convert.ToInt64(appId), legacyAppFormId, nar);
                                            ExecuteQueryUsingAdoNet("UPDATE ApplicationSeedingLogs SET IsUserSeeded = 4 WHERE Id=" + seedingLogs.Id.ToString());
                                        }

                                        #endregion
                                        long projectSiteId = 0;
                                        Int64 projectSiteRefId = 0;
                                        var appdata = _context.Applications.Where(s => s.Legacy_AppId == Convert.ToInt64(appId) && s.IsDeleted == false).FirstOrDefault();
                                        //var projectsite = _context.ProjectSites.Where(s => s.EstablishmentName == applications.FirstOrDefault().EstdName);
                                        if (appdata != null)
                                        {
                                            projectSiteId = appdata.ProjectSiteRefId;
                                            projectSiteRefId = appdata.ProjectSiteRefId;
                                        }
                                        else
                                        {
                                            projectSiteId = 0;
                                        }

                                        bool isProjectSiteToBeCreated = false;

                                        //var ipin = "0";
                                        var investPunjabAppId = "0";
                                        var ipin = "0";
                                        //var ipin = await _iDapperRepository.Get<string>(
                                        //    "SELECT PbLabour.dbo.GetAppIdFromIntegrationInfo(@AppId, @ServiceCode, @NAR)",new { AppId = appId, ServiceCode = legacyAppFormId , NAR = nar }, CommandType.Text).ConfigureAwait(false);

                                        List<StoreProcedureParm> storeProcedureParm = new List<StoreProcedureParm>()
                                        {
                                        new StoreProcedureParm (){ ParmName="E_LAB_AppId", ParmValue=appId, isNumber=true},
                                        new StoreProcedureParm (){ ParmName="ServiceCode", ParmValue=legacyAppFormId.ToString(), isNumber=true},
                                        new StoreProcedureParm (){ ParmName="NAR", ParmValue=nar, isNumber=false}
                                        };


                                        var legacyipinAppId = await _iGeneric_SP_Repository.CallStoreProcedureReaderAsync<AppSubmissionResponseViewModel>("dbo.sp_GetLegacyIpinAppId", storeProcedureParm);
                                        investPunjabAppId = legacyipinAppId.FirstOrDefault().ApplicationId.ToString();
                                        ipin = legacyipinAppId.FirstOrDefault().Ipin.ToString();


                                        if (projectSiteId == 0)
                                        {
                                            isProjectSiteToBeCreated = true;
                                        }
                                        else
                                        {
                                            isProjectSiteToBeCreated = false;
                                        }

                                        #region Project Site Created
                                        if (isProjectSiteToBeCreated && applications.Count() > 0 && applications != null)
                                        {
                                            if (seededInfo != null && seededInfo.IsProjectSiteSeeded == SeedStatusTypeEnum.NO_STATUS)
                                            {
                                                var factoryCircleId = 22;
                                                var labourCircleId = 10001;

                                                ProjectSite projectSite = new ProjectSite();

                                                {
                                                    projectSite.EstablishmentName = applications.FirstOrDefault().EstdName;
                                                    projectSite.Address = applications.FirstOrDefault().SiteAddress;
                                                    projectSite.VillageOrTown = applications.FirstOrDefault().SiteAddCity;
                                                    projectSite.TehsilRefId = applications.FirstOrDefault().LGDirTehsilCode;
                                                    projectSite.DistrictRefId = applications.FirstOrDefault().DistrictLgdId;
                                                    projectSite.PinCode = (applications.FirstOrDefault().PinCode == null || applications.FirstOrDefault().PinCode == "") ? "000000" : applications.FirstOrDefault().PinCode.Substring(applications.FirstOrDefault().PinCode.Length - 6);
                                                    projectSite.IsActive = true;
                                                    projectSite.IsDeleted = false;
                                                    projectSite.Createddate = DateTime.Now;
                                                    projectSite.LastModifiedDate = DateTime.Now;
                                                    projectSite.UserRefId = applications.FirstOrDefault().ApplicationUserID.ToString();
                                                    projectSite.LabourCircleRefId = labourCircleId == null ? 0 : labourCircleId;
                                                    projectSite.FactoryCircleRefId = (applications.FirstOrDefault().FCId == null || applications.FirstOrDefault().FCId.ToString() == "") ? 0 : (int)applications.FirstOrDefault().FCId;
                                                    projectSite.AlcCircleRefId = (applications.FirstOrDefault().ALLCCircleId == null || applications.FirstOrDefault().ALLCCircleId.ToString() == "") ? 0 : (int)applications.FirstOrDefault().ALLCCircleId;
                                                    projectSite.ApplicantAadharNumber = applications.FirstOrDefault().AadhaarNo;
                                                    projectSite.ApplicantAadharAttachment = "NA";
                                                    projectSite.ApplicantPanNumber = "NA";

                                                    projectSite.ApplicantPanAttachment = "NA";
                                                    projectSite.CompanyPanNumber = "NA";
                                                    projectSite.CompanyPanAttachment = "NA";
                                                    projectSite.ProjectPurpose = applications.FirstOrDefault().ProjectPurpose;
                                                    projectSite.ContactPersonFirstName = applications.FirstOrDefault().FirstName;
                                                    projectSite.ContactPersonMiddleName = applications.FirstOrDefault().MiddleName;
                                                    projectSite.ContactPersonLastName = applications.FirstOrDefault().LastName == null ? "." : applications.FirstOrDefault().LastName;
                                                    projectSite.ContactPersonEmail = applications.FirstOrDefault().Email;
                                                    projectSite.ContactPersonMobileNo = applications.FirstOrDefault().MobileNo.Substring(applications.FirstOrDefault().MobileNo.Length - 10);
                                                    projectSite.AlternateEmail = "abc@gmail.com";
                                                    projectSite.AlternateMobileNo = "9999999999";
                                                }
                                                ;
                                                var projectSiteCreateResp = await _iProjectSiteService.CreateProjectSite(projectSite, true, ipin.ToString()).ConfigureAwait(false);
                                                projectSiteRefId = projectSiteCreateResp.ResponseDataModel.ProjectSiteId;

                                                seedingLogs = await GetApplicationSeedingLogsByAppId(Convert.ToInt64(appId), legacyAppFormId, nar).ConfigureAwait(false);
                                                ExecuteQueryUsingAdoNet("UPDATE ApplicationSeedingLogs SET IsProjectSiteSeeded = 1 WHERE Id=" + seedingLogs.Id.ToString());
                                            }
                                        }
                                        else
                                        {
                                            isProjectSiteToBeCreated = false;
                                            seedingLogs = await GetApplicationSeedingLogsByAppId(Convert.ToInt64(appId), legacyAppFormId, nar);
                                            ExecuteQueryUsingAdoNet("UPDATE ApplicationSeedingLogs SET IsProjectSiteSeeded = 2 WHERE Id=" + seedingLogs.Id.ToString());
                                        }

                                        #endregion




                                        #region Master data
                                        if (seededInfo == null || seededInfo.IsMasterSeeded != SeedStatusTypeEnum.NO_STATUS || applications.Count() <= 0 || applications == null)
                                        {
                                            var app = _context.Applications.Where(x => x.Legacy_AppId.ToString() == appId && x.IsDeleted == false && legacyAppFormId == legacyAppFormId && x.Legacy_NAR == nar).FirstOrDefault();
                                            if (app == null)
                                            {
                                                seedingLogs = await GetApplicationSeedingLogsByAppId(Convert.ToInt64(appId), legacyAppFormId, nar).ConfigureAwait(false);
                                                ExecuteQueryUsingAdoNet("UPDATE ApplicationSeedingLogs SET OverAllSeedStatus = 0,SeedStatusDiscription ='No new app available'  WHERE Id=" + seedingLogs.Id.ToString());
                                            }
                                            else
                                            {
                                                newAppId = app.AppId;
                                            }
                                        }
                                        else
                                        {


                                            var FactoryLicenseNumberNAR = "";
                                            if (legacyAppFormId == 2 && nar.Contains("BPC")) // Proposed Building Plan
                                            {
                                                var masterData = await _iDapperRepository.Get<LegacyBuildingPlanMasterDataViewModel>("SELECT * FROM BuildingPlans WHERE AppId = @AppId", new { AppId = appId }, CommandType.Text).ConfigureAwait(true);
                                                Licence_Proposed_BuildingPlan_GeneralDetail formData = new Licence_Proposed_BuildingPlan_GeneralDetail()
                                                {
                                                    CompetentPersonListId = Convert.ToInt32(masterData.FirstOrDefault().CompetentPersonListID),
                                                    BuildingCost = Convert.ToDecimal(masterData.FirstOrDefault().BuildingCost) * 100000,
                                                    IsBuildingConstructedBefore_01_Oct_2008 = masterData.FirstOrDefault().IsBuildingConstBefore2008,
                                                    DispatchNo = masterData.FirstOrDefault().OLNo,
                                                    DispatchDate = masterData.FirstOrDefault().IssueDate,
                                                    RegistrationDate = masterData.FirstOrDefault().RegistrationDate,
                                                    CompetentPersonVisitDate = masterData.FirstOrDefault().CompPerVisitDate,
                                                    BOCW_NoOfWorkers = masterData.FirstOrDefault().bocw_numberworker,

                                                    CompetentPersonName = "Not available",
                                                    CompetentPersonContactNo = "Not available",
                                                    CompetentPersonEmail = "Not available",
                                                    ArchitectName = "Not available",
                                                    ArchitectEmail = "Not available",
                                                    ArchitectContactNo = "Not available",
                                                    EngineerName = "Not available",
                                                    EngineerEmail = "Not available",
                                                    EngineerContactNo = "Not available",
                                                    BuildingPlanApprovalAuthorityType = BuildingPlanApprovalAuthorityTypeEnum.COMPETENT_PERSON,
                                                };

                                                var Sys_N_ServiceCodeInfo = FindServiceCodeMappingWithApplicationType(71);
                                                var statusResp = await _iApplicationMamnagement_Proposed_BuildingPlan_Service.InitiateApplication(ApplicationTypeEnum.BUILDING_PLAN_PROPOSED, formData, projectSiteRefId, Sys_N_ServiceCodeInfo.ApplicationPurposeType, applications.FirstOrDefault().ApplicationUserID, Convert.ToInt64(ipin), Convert.ToInt64(investPunjabAppId), true, Convert.ToInt64(appId), legacyAppFormId, nar, null, 1);
                                                newAppId = statusResp.AppId;

                                                // Update seeding log after master creation
                                                seedingLogs = await GetApplicationSeedingLogsByAppId(Convert.ToInt64(appId), legacyAppFormId, nar);

                                                string query = "UPDATE ApplicationSeedingLogs SET NewAppId = " + newAppId + ", IsMasterSeeded = 1 WHERE Id = " + seedingLogs.Id.ToString();
                                                ExecuteQueryUsingAdoNet(query);

                                            }
                                            else if (legacyAppFormId == 2 && nar.Contains("BPS")) // Existing
                                            {
                                                var masterData = await _iDapperRepository.Get<LegacyBuildingPlanMasterDataViewModel>("SELECT * FROM BuildingPlans WHERE AppId = @AppId AND NAR LIKE 'BPS%'", new { AppId = appId }, CommandType.Text).ConfigureAwait(true);
                                                if (masterData.Count() == 0)
                                                {
                                                    masterData = await _iDapperRepository.Get<LegacyBuildingPlanMasterDataViewModel>("SELECT TOP 1 * FROM BuildingPlans_Log WHERE AppId = @AppId AND NAR LIKE 'BPS%'", new { AppId = appId }, CommandType.Text).ConfigureAwait(true);
                                                }
                                                else
                                                {
                                                    Licence_Existing_BuildingPlan_GeneralDetail formData = new Licence_Existing_BuildingPlan_GeneralDetail()
                                                    {
                                                        CompetentPersonListId = Convert.ToInt32(masterData.FirstOrDefault().CompetentPersonListID),
                                                        BuildingCost = Convert.ToDecimal(masterData.FirstOrDefault().BuildingCost) * 100000,
                                                        IsBuildingConstructedBefore_01_Oct_2008 = masterData.FirstOrDefault().IsBuildingConstBefore2008,
                                                        DispatchNo = masterData.FirstOrDefault().OLNo,
                                                        DispatchDate = masterData.FirstOrDefault().IssueDate,
                                                        RegistrationDate = masterData.FirstOrDefault().RegistrationDate,
                                                        CompetentPersonVisitDate = masterData.FirstOrDefault().CompPerVisitDate,
                                                        BOCW_NoOfWorkers = masterData.FirstOrDefault().bocw_numberworker,
                                                        CompetentPersonName = "Not available",
                                                        CompetentPersonContactNo = "Not available",
                                                        CompetentPersonEmail = "Not available",
                                                        ArchitectName = "Not available",
                                                        ArchitectEmail = "Not available",
                                                        ArchitectContactNo = "Not available",
                                                        EngineerName = "Not available",
                                                        EngineerEmail = "Not available",
                                                        EngineerContactNo = "Not available",
                                                        BuildingPlanApprovalAuthorityType = BuildingPlanApprovalAuthorityTypeEnum.COMPETENT_PERSON,
                                                    };

                                                    var Sys_N_ServiceCodeInfo = FindServiceCodeMappingWithApplicationType(72);
                                                    var statusResp = await _iApplicationMamnagement_Existing_BuildingPlan_Service.InitiateApplication(ApplicationTypeEnum.BUILDING_PLAN_EXISTING, formData, projectSiteRefId, Sys_N_ServiceCodeInfo.ApplicationPurposeType, applications.FirstOrDefault().ApplicationUserID, Convert.ToInt64(ipin), Convert.ToInt64(investPunjabAppId), true, Convert.ToInt64(appId), legacyAppFormId, nar, null, 1);
                                                    newAppId = statusResp.AppId;
                                                }

                                                // Update seeding log after master creation
                                                seedingLogs = await GetApplicationSeedingLogsByAppId(Convert.ToInt64(appId), legacyAppFormId, nar);

                                                string query = "UPDATE ApplicationSeedingLogs SET NewAppId = " + newAppId + ", IsMasterSeeded = 1 WHERE Id = " + seedingLogs.Id.ToString();
                                                ExecuteQueryUsingAdoNet(query);
                                            }
                                            else if (legacyAppFormId == 2 && (nar.Contains("BPA") || nar.Contains("BPM"))) // Addition/Amendment
                                            {
                                                var masterData = await _iDapperRepository.Get<LegacyBuildingPlanMasterDataViewModel>("SELECT * FROM BuildingPlans WHERE AppId = @AppId", new { AppId = appId }, CommandType.Text).ConfigureAwait(true);
                                                Licence_Addition_Amendment_BuildingPlan_GeneralDetail formData = new Licence_Addition_Amendment_BuildingPlan_GeneralDetail()
                                                {
                                                    CompetentPersonListId = Convert.ToInt32(masterData.FirstOrDefault().CompetentPersonListID),
                                                    BuildingCost = Convert.ToDecimal(masterData.FirstOrDefault().BuildingCost) * 100000,
                                                    IsBuildingConstructedBefore_01_Oct_2008 = masterData.FirstOrDefault().IsBuildingConstBefore2008,
                                                    DispatchNo = masterData.FirstOrDefault().OLNo,
                                                    DispatchDate = masterData.FirstOrDefault().IssueDate,
                                                    RegistrationDate = masterData.FirstOrDefault().RegistrationDate,
                                                    CompetentPersonVisitDate = masterData.FirstOrDefault().CompPerVisitDate,
                                                    BOCW_NoOfWorkers = masterData.FirstOrDefault().bocw_numberworker,
                                                    CompetentPersonName = "Not available",
                                                    CompetentPersonContactNo = "Not available",
                                                    CompetentPersonEmail = "Not available",
                                                    ArchitectName = "Not available",
                                                    ArchitectEmail = "Not available",
                                                    ArchitectContactNo = "Not available",
                                                    EngineerName = "Not available",
                                                    EngineerEmail = "Not available",
                                                    EngineerContactNo = "Not available",
                                                    BuildingPlanApprovalAuthorityType = BuildingPlanApprovalAuthorityTypeEnum.COMPETENT_PERSON,
                                                };

                                                var Sys_N_ServiceCodeInfo = FindServiceCodeMappingWithApplicationType(73);
                                                var statusResp = await _iApplicationMamnagement_Addition_Amendment_BuildingPlan_Service.InitiateApplication(ApplicationTypeEnum.BUILDING_PLAN_ADDITION_AMENDMENT, formData, projectSiteRefId, Sys_N_ServiceCodeInfo.ApplicationPurposeType, applications.FirstOrDefault().ApplicationUserID, Convert.ToInt64(ipin), Convert.ToInt64(investPunjabAppId), true, Convert.ToInt64(appId), legacyAppFormId, nar, null, 1);
                                                newAppId = statusResp.AppId;

                                                // Update seeding log after master creation
                                                seedingLogs = await GetApplicationSeedingLogsByAppId(Convert.ToInt64(appId), legacyAppFormId, nar);

                                                string query = "UPDATE ApplicationSeedingLogs SET NewAppId = " + newAppId + ", IsMasterSeeded = 1 WHERE Id = " + seedingLogs.Id.ToString();
                                                ExecuteQueryUsingAdoNet(query);
                                            }
                                            else if (legacyAppFormId == 22 || legacyAppFormId == 23 || legacyAppFormId == 24 && nar.Contains("MTL") || nar.Contains("MTR") || nar.Contains("MTA")) // MTL
                                            {
                                                var masterData = await _iDapperRepository.Get<LegacyMotorTransportMasterDataViewModel>("SELECT * FROM MotorTransports WHERE AppId = @AppId", new { AppId = appId }, CommandType.Text).ConfigureAwait(true);
                                                Licence_MotorTransport formData = new Licence_MotorTransport()
                                                {
                                                    TransportUndertakingName = masterData.FirstOrDefault().NameMotorTrans,
                                                    CommunicationAddress = masterData.FirstOrDefault().CommAddress,
                                                    CommunicationAddress_PinCode = masterData.FirstOrDefault().CommPincode,
                                                    CommunicationAddress_DistrictLgdId = 1,
                                                    CommunicationAddress_TehsilLgdId = 1,
                                                    TransportServiceName = masterData.FirstOrDefault().NameTransService,
                                                    TotalRoutes = Convert.ToInt32(masterData.FirstOrDefault().TotalNoOfRoutes),
                                                    TotalRouteMileage = Convert.ToInt32(masterData.FirstOrDefault().TotalMileage),
                                                    TotalVehicles = Convert.ToInt32(masterData.FirstOrDefault().TotalNoVehiclesPreceedingYear),
                                                    MaxTransportWorkers = Convert.ToInt32(masterData.FirstOrDefault().MaxWorkersLastYear),
                                                    NameAddressType = masterData.FirstOrDefault().WhichAuthorty == "General Manager" ? MotorTransportNameAddressTypeEnum.GENERAL_MANAGER : MotorTransportNameAddressTypeEnum.PROPRIETOR_PARTNERS,
                                                    NameAddressType_Name = masterData.FirstOrDefault().AuthorityFullName ?? "No Name",
                                                    NameAddressType_Email = masterData.FirstOrDefault().AuthorityEmail ?? "nomail@gmail.com",
                                                    NameAddressType_Mobile = masterData.FirstOrDefault().AuthorityMobile ?? "9999999999",
                                                    NameAddressType_Address = masterData.FirstOrDefault().AuthorityAddress ?? "No Address",
                                                    IsCompanyRegUnderCompaniesAct = Convert.ToInt32(masterData.FirstOrDefault().IsCompany),
                                                    Director_Name = masterData.FirstOrDefault().DirectorName ?? "No Name",
                                                    Director_Email = masterData.FirstOrDefault().DirectorEmail ?? "nomail@gmail.com",
                                                    Director_Mobile = masterData.FirstOrDefault().DirectorMobile ?? "9999999999",
                                                    Director_Address = masterData.FirstOrDefault().DirectorAddress ?? "No Address",
                                                    ModifiedCounter = 1,
                                                    LicenceForYear = masterData.FirstOrDefault().pdate.Year,
                                                };

                                                var Sys_N_ServiceCodeInfo = FindServiceCodeMappingWithApplicationType(legacyAppFormId);
                                                var statusResp = await _iApplicationMamnagement_Licence_MotorTransport_Service.InitiateApplication(ApplicationTypeEnum.MOTOR_TRANSPORT, formData, projectSiteRefId, Sys_N_ServiceCodeInfo.ApplicationPurposeType, applications.FirstOrDefault().ApplicationUserID, Convert.ToInt64(ipin), Convert.ToInt64(investPunjabAppId), true, Convert.ToInt64(appId), legacyAppFormId, nar, null, 1);
                                                newAppId = statusResp.AppId;

                                                // Update seeding log after master creation
                                                seedingLogs = await GetApplicationSeedingLogsByAppId(Convert.ToInt64(appId), legacyAppFormId, nar);

                                                string query = "UPDATE ApplicationSeedingLogs SET NewAppId = " + newAppId + ", IsMasterSeeded = 1 WHERE Id = " + seedingLogs.Id.ToString();
                                                ExecuteQueryUsingAdoNet(query);
                                            }
                                            else if (legacyAppFormId == 6 || legacyAppFormId == 7 || legacyAppFormId == 8 && nar.Contains("FLM") || nar.Contains("FLR") || nar.Contains("FLA")) // Factory Licence Act
                                            {
                                                var masterData = await _iDapperRepository.Get<ProcessImd_ClearencesFactoryLicenceViewModel>("SELECT * FROM FactoryLicenses WHERE AppId = @AppId", new { AppId = appId }, CommandType.Text).ConfigureAwait(true);
                                                //if(masterData.FirstOrDefault().FactoryLicenseNumber == null || masterData.FirstOrDefault().FactoryLicenseNumber == "")
                                                //{
                                                //   var masterData1 = await _iDapperRepository.Get<ProcessImd_ClearencesFactoryLicenceViewModel>("SELECT * FROM FactoryLicenseNARs WHERE AppId = @AppId AND FactoryLicenseNumber IS NOT NULL", new { AppId = appId }, CommandType.Text).ConfigureAwait(true);
                                                //    FactoryLicenseNumberNAR = masterData1.FirstOrDefault().FactoryLicenseNumber;
                                                //}
                                                Licence_Factory_GeneralDetail formData = new Licence_Factory_GeneralDetail()
                                                {
                                                    IsBuildingConstructedBefore29June2018 = 1,
                                                    HaveYouMadeChangesInBuildingPlan = 0,
                                                    //OldLicenceNo = masterData.FirstOrDefault().FactoryLicenseNumber == null ? FactoryLicenseNumberNAR : masterData.FirstOrDefault().FactoryLicenseNumber,
                                                    OldLicenceNo = masterData.FirstOrDefault().FactoryLicenseNumber == null ? "NA" : masterData.FirstOrDefault().FactoryLicenseNumber,
                                                    OldLicenceValidUpTo = masterData.FirstOrDefault().OLStartDate,
                                                    OldLicenceTotalEmployees = masterData.FirstOrDefault().OLEmps,
                                                    OldLicenceFactoryKiloWatt = masterData.FirstOrDefault().InstalledPower,
                                                    RegistrationDate = masterData.FirstOrDefault().RegistrationDate,
                                                    RenewalFromDate = masterData.FirstOrDefault().ApplicationDate,

                                                    NoOfYears = masterData.FirstOrDefault().LicenceForNoOfYear,
                                                    ManufacturingProcess_Last12Months = "NA",
                                                    ManufacturingProcess_Next12Months = "NA",
                                                    NationalIndustrialClassificationCode = masterData.FirstOrDefault().NICCode,
                                                    MfgProducts_Last12Month = "NA",
                                                    Workers_MaxDuringYear = masterData.FirstOrDefault().MaximumNumberEmployeeInYear,
                                                    Workers_MaxLast12Month = masterData.FirstOrDefault().MaximumNumberEmployeeLastYear,
                                                    Workers_OrdinarilyEmployed = masterData.FirstOrDefault().OrdinarilyEmployed,
                                                    PowerKW_Installed = masterData.FirstOrDefault().InstalledPower,
                                                    PowerKW_MaxProposed = masterData.FirstOrDefault().MaximumPowerUsed,
                                                    ModifiedCounter = 1,
                                                    AppIdRightToBusinessAct = "NA",
                                                    BuildingPlanDofNumber = "NA",
                                                    CompetentPersonUserId = "NA",
                                                    DateOfPrincipalApproval = "NA",
                                                    IsBuildingPlanApproved = 2,
                                                    IsBuildingPlanVerified = true,
                                                    IsRBAVerified = true,
                                                    IsStabilityApproved = BuildingPlanStabilityAuthorityTypeEnum.DIRECTOR_FACTORIES,
                                                    IsStabilityPlanVerified = true,
                                                    IsTempRegistered = 0,
                                                    IsTempRegistrationVerified = true,
                                                    IsUnderRightToBusinessAct = 0,
                                                    ProjectIdentificationNo = "NA",
                                                    StabilityPlanDofNumber = "NA",
                                                    TempRegistrationNumber = "NA",
                                                    CompetentPersonName = "NA",
                                                    CompetentPersonContactNo = "9999999999",
                                                    CompetentPersonEmail = "NA",
                                                    EngineerName = "NA",
                                                    EngineerContactNo = "9999999999",
                                                    EngineerEmail = "NA",
                                                    BuildingPlanApprovalDate = DateTime.Now,
                                                    BuildingPlanStabilityApprovalDate = DateTime.Now
                                                };

                                                var Sys_N_ServiceCodeInfo = FindServiceCodeMappingWithApplicationType(legacyAppFormId);
                                                var statusResp = await _iApplicationMamnagement_Factory_Service.InitiateApplication(ApplicationTypeEnum.FACTORY_LICENCE, formData, projectSiteRefId, Sys_N_ServiceCodeInfo.ApplicationPurposeType, applications.FirstOrDefault().ApplicationUserID, Convert.ToInt64(ipin), Convert.ToInt64(investPunjabAppId), true, Convert.ToInt64(appId), legacyAppFormId, nar, null, 1);
                                                newAppId = statusResp.AppId;


                                                Licence_Factory_OccupierAndManagerDetail occupierDetails = new Licence_Factory_OccupierAndManagerDetail()
                                                {
                                                    ManagerFullName = masterData.FirstOrDefault().ManagerFullName,
                                                    ManagerFatherName = masterData.FirstOrDefault().ManagerFatherName,
                                                    ManagerFullAddress = masterData.FirstOrDefault().ManagerFullAddress,
                                                    ManagerMobile = masterData.FirstOrDefault().ManagerMobile == null ? "NA" : masterData.FirstOrDefault().ManagerMobile,
                                                    ManagerEmail = masterData.FirstOrDefault().ManagerEmail == null ? "NA" : masterData.FirstOrDefault().ManagerEmail,
                                                    ManagerResidentialAddress = masterData.FirstOrDefault().ManagerFullAddress,
                                                    OccupierFullName = masterData.FirstOrDefault().OccupierFullName,
                                                    OccupierFatherName = masterData.FirstOrDefault().OccupierFatherName,
                                                    OccupierFullAddress = masterData.FirstOrDefault().OccupierFullAddress,
                                                    OccupierMobile = masterData.FirstOrDefault().OccupierMobile == null ? "NA" : masterData.FirstOrDefault().OccupierMobile,
                                                    OccupierEmail = masterData.FirstOrDefault().OccupierEmail == null ? "NA" : masterData.FirstOrDefault().OccupierEmail,
                                                    OccupierResidentialAddress = masterData.FirstOrDefault().OccupierFullAddress,
                                                    OwnerName = masterData.FirstOrDefault().OwnerName,
                                                    OwnerPremisesAddress = masterData.FirstOrDefault().OwnerPremisesAddress,
                                                    StabilityCertificateNumber = "NA",
                                                    StabilityCertificateDate = masterData.FirstOrDefault().RegistrationDate,
                                                    StabilityDOFNumber = "NA",
                                                    Checklist_IsBuildingPlanApproved = "1",
                                                    Checklist_IsLabourWelfareFundPaid = "1",
                                                    Checklist_IsAnnualReturnFiled = "1",
                                                    Checklist_IsStabilityCertificateAttached = "1",
                                                    FactoryLicenceRefId = formData.FactoryLicenceId,
                                                    ModifiedCounter = 1
                                                };
                                                await _context.Licence_Factory_OccupierAndManagerDetails.AddAsync(occupierDetails);
                                                await _context.SaveChangesAsync();

                                                // Update seeding log after master creation
                                                seedingLogs = await GetApplicationSeedingLogsByAppId(Convert.ToInt64(appId), legacyAppFormId, nar);

                                                string query = "UPDATE ApplicationSeedingLogs SET NewAppId = " + newAppId + ", IsMasterSeeded = 1 WHERE Id = " + seedingLogs.Id.ToString();
                                                ExecuteQueryUsingAdoNet(query);
                                            }
                                            else if (legacyAppFormId == 3 || legacyAppFormId == 11 && nar.Contains("PEF") || nar.Contains("PEA")) // Principal Employeer Act
                                            {
                                                var masterData = await _iDapperRepository.Get<ProcessImd_ClearencesEstablishmentRegistrationViewModel>("SELECT * FROM EstablishmentRegistrations WHERE AppId = @AppId AND NAR = @NAR", new { AppId = appId, NAR = nar }, CommandType.Text).ConfigureAwait(true);

                                                if (masterData.Count() == 0)
                                                {
                                                    masterData = await _iDapperRepository.Get<ProcessImd_ClearencesEstablishmentRegistrationViewModel>("SELECT * FROM EstablishmentRegistration_Log WHERE AppId = @AppId  AND NAR = @NAR ", new { AppId = appId, NAR = nar }, CommandType.Text).ConfigureAwait(true);

                                                    if (masterData.Count() == 0)
                                                    {
                                                        masterData = await _iDapperRepository.Get<ProcessImd_ClearencesEstablishmentRegistrationViewModel>("SELECT * FROM EstablishmentRegistration_Log WHERE AppId = @AppId  ", new { AppId = appId }, CommandType.Text).ConfigureAwait(true);
                                                    }

                                                }


                                                Licence_CL_PE_GeneralDetail formData = new Licence_CL_PE_GeneralDetail();
                                                if (masterData.Count() == 0)
                                                {
                                                    {
                                                        formData.PE_Name = "NA";
                                                        formData.PE_FatherName = "NA";
                                                        formData.PE_Mobile = "0000000000";
                                                        formData.PE_Email = "No Email";
                                                        formData.PE_Address = "No Address";
                                                        formData.Manager_Name = "NA";
                                                        formData.Manager_Mobile = "0000000000";
                                                        formData.Manager_Email = "No Email";
                                                        formData.Manager_Address = "No Address";
                                                        formData.NatureOfWork = "NA";
                                                        formData.TotalWorker = 0;

                                                    }
                                                    ;

                                                }
                                                else
                                                {
                                                    {
                                                        formData.PE_Name = masterData.FirstOrDefault().PrincipalEmpName;
                                                        formData.PE_FatherName = masterData.FirstOrDefault().PrincipalFatherEmpName ?? "NA";
                                                        formData.PE_Mobile = masterData.FirstOrDefault().PrincipalEmpMobileNo ?? "0000000000";
                                                        formData.PE_Email = masterData.FirstOrDefault().PrincipalEmpEmailId ?? "No Email";
                                                        formData.PE_Address = masterData.FirstOrDefault().PrincipalEmpAddress ?? "No Address";
                                                        formData.Manager_Name = masterData.FirstOrDefault().ManagerName ?? "NA";
                                                        formData.Manager_Mobile = masterData.FirstOrDefault().ManagerMobileNo ?? "0000000000";
                                                        formData.Manager_Email = masterData.FirstOrDefault().ManagerEmailId ?? "No Email";
                                                        formData.Manager_Address = masterData.FirstOrDefault().ManagerAddress ?? "No Address";
                                                        formData.NatureOfWork = masterData.FirstOrDefault().WorkCarried;
                                                        formData.TotalWorker = masterData.FirstOrDefault().TotalWorker;

                                                    }
                                                    ;

                                                }

                                                var validations = CustomeValidator<Licence_CL_PE_GeneralDetail>.ValidateModel_AllProperties(formData);
                                                if (!validations.IsValid)
                                                {
                                                    var kk = validations.CustomeValidationErrorList;
                                                }

                                                var Sys_N_ServiceCodeInfo = FindServiceCodeMappingWithApplicationType(legacyAppFormId);
                                                var statusResp = await _iApplicationMamnagement_CL_PE_Service.InitiateApplication(ApplicationTypeEnum.PRINCIPAL_EMPLOYER, formData, projectSiteRefId, Sys_N_ServiceCodeInfo.ApplicationPurposeType, applications.FirstOrDefault().ApplicationUserID, Convert.ToInt64(ipin), Convert.ToInt64(investPunjabAppId), true, Convert.ToInt64(appId), legacyAppFormId, nar, null, 1);
                                                newAppId = statusResp.AppId;

                                                // Get Employee Details 
                                                var contractordetails = await _iSystem_O_CommunicationService.GetContractorList(Convert.ToInt64(appId), legacyAppFormId, nar);

                                                if (contractordetails != null)
                                                {
                                                    Licence_CL_PE_Contrator contractorlist = null;
                                                    foreach (var item in contractordetails.ResponseDataModel)
                                                    {
                                                        contractorlist = new Licence_CL_PE_Contrator()
                                                        {
                                                            Name = item.ContractorName,
                                                            Address = item.ContractorAddress,
                                                            NatureOfWork = item.WorkNature ?? "NA",
                                                            MaxLabourWorkerEmployed = (int)item.ContractLabourEmployed,
                                                            DateOfCommencement = item.ContractWork_CommencingDate,
                                                            DateOfTermination = item.ContractWork_TerminationDate,
                                                            Licence_CL_PE_GeneralDetailRefId = statusResp.EntityKeyId,
                                                            ModifiedCounter = 0
                                                        };

                                                        validations = CustomeValidator<Licence_CL_PE_Contrator>.ValidateModel_AllProperties(contractorlist);
                                                        if (!validations.IsValid)
                                                        {
                                                            var kk = validations.CustomeValidationErrorList;
                                                        }

                                                        await _context.Licence_CL_PE_Contrator.AddAsync(contractorlist);
                                                        await _context.SaveChangesAsync();
                                                    }
                                                }

                                                // Update seeding log after master creation
                                                seedingLogs = await GetApplicationSeedingLogsByAppId(Convert.ToInt64(appId), legacyAppFormId, nar);

                                                string query = "UPDATE ApplicationSeedingLogs SET NewAppId = " + newAppId + ", IsMasterSeeded = 1 WHERE Id = " + seedingLogs.Id.ToString();
                                                ExecuteQueryUsingAdoNet(query);
                                            }
                                            else if (legacyAppFormId == 4 || legacyAppFormId == 12 || legacyAppFormId == 13 && nar.Contains("CLF") || nar.Contains("CLR") || nar.Contains("CLA")) // Contract Labour Act
                                            {
                                                var masterData = await _iDapperRepository.Get<ProcessImd_ClearencesContractLabourViewModel>("SELECT * FROM ContactLabours WHERE AppId = @AppId AND NAR = @NAR", new { AppId = appId, NAR = nar }, CommandType.Text).ConfigureAwait(true);

                                                if (masterData.Count() == 0)
                                                {
                                                    masterData = await _iDapperRepository.Get<ProcessImd_ClearencesContractLabourViewModel>("SELECT * FROM ContactLabourNARs WHERE AppId = @AppId  AND NAR = @NAR ", new { AppId = appId, NAR = nar }, CommandType.Text).ConfigureAwait(true);

                                                    if (masterData.Count() == 0)
                                                    {
                                                        masterData = await _iDapperRepository.Get<ProcessImd_ClearencesContractLabourViewModel>("SELECT * FROM ContactLabourNARs WHERE AppId = @AppId  ", new { AppId = appId }, CommandType.Text).ConfigureAwait(true);
                                                    }

                                                }


                                                Licence_ContractLabour_GeneralDetail formData = new Licence_ContractLabour_GeneralDetail();
                                                if (masterData.Count() == 0)
                                                {
                                                    {
                                                        formData.IsCoopraticSociety = "1";
                                                        formData.ContractorName = "NA";
                                                        formData.ContractorFatherName = "NA";
                                                        formData.ContractorAddress = "NA";
                                                        formData.ContractorDOB = DateTime.Today;
                                                        formData.TypeOfBusiness = "NA";
                                                        formData.RegistrationCertificateNo = "NA";
                                                        formData.RegistrationCertificateDate = DateTime.Today;
                                                        formData.PrincipalEmployerName = "NA";
                                                        formData.PrincipalEmployerAddress = "NA";
                                                        formData.NatureOfWork = "NA";
                                                        formData.ContractWork_CommencingDate = DateTime.Today;
                                                        formData.ContractWork_TerminationDate = DateTime.Today;
                                                        formData.AgentOrManagerName = "NA";
                                                        formData.AgentOrManagerAddress = "NA";
                                                        formData.MaximumNumberOfEmployee = 0;
                                                        formData.ContractorAge = 0;
                                                        formData.LicenceForYear = 0;
                                                        formData.ModifiedCounter = 1;
                                                        formData.GstNumber = "000000000";
                                                        formData.PanOrTanNumber = "ABCDD1234E";

                                                    }
                                                    ;

                                                }
                                                else
                                                {
                                                    {
                                                        formData.IsCoopraticSociety = "1";
                                                        formData.ContractorName = masterData.FirstOrDefault().ContractorName;
                                                        formData.ContractorFatherName = masterData.FirstOrDefault().ContractorFatherName ?? "NA";
                                                        formData.ContractorAddress = masterData.FirstOrDefault().ContractorAddress ?? "NA";
                                                        formData.ContractorDOB = masterData.FirstOrDefault().DOB ?? DateTime.Today;
                                                        formData.TypeOfBusiness = masterData.FirstOrDefault().TypeOfBusiness;
                                                        formData.RegistrationCertificateNo = masterData.FirstOrDefault().RegistrationCertificateNo ?? "NA";
                                                        formData.RegistrationCertificateDate = masterData.FirstOrDefault().RegistrationCertificateDate ?? DateTime.Today;
                                                        formData.PrincipalEmployerName = masterData.FirstOrDefault().PrincipalEmployerName ?? "NA";
                                                        formData.PrincipalEmployerAddress = masterData.FirstOrDefault().PrincipalEmployerAddress ?? "NA";
                                                        formData.NatureOfWork = masterData.FirstOrDefault().WorkNatureString;
                                                        formData.ContractWork_CommencingDate = masterData.FirstOrDefault().ContractWork_CommencingDate;
                                                        formData.ContractWork_TerminationDate = masterData.FirstOrDefault().ContractWork_TerminationDate;
                                                        formData.AgentOrManagerName = masterData.FirstOrDefault().AgentOrManagerName ?? "NA";
                                                        formData.AgentOrManagerAddress = masterData.FirstOrDefault().AgentOrManagerAddress ?? "NA";
                                                        formData.MaximumNumberOfEmployee = masterData.FirstOrDefault().MaximumNumberEmployee;
                                                        formData.ContractorAge = masterData.FirstOrDefault().Age;
                                                        formData.LicenceForYear = masterData.FirstOrDefault().RegistrationFrom;
                                                        formData.ModifiedCounter = 1;
                                                        formData.GstNumber = "000000000";
                                                        formData.PanOrTanNumber = "ABCDD1234E";

                                                    }
                                                    ;

                                                }

                                                var validations = CustomeValidator<Licence_ContractLabour_GeneralDetail>.ValidateModel_AllProperties(formData);
                                                if (!validations.IsValid)
                                                {
                                                    var kk = validations.CustomeValidationErrorList;
                                                }

                                                var Sys_N_ServiceCodeInfo = FindServiceCodeMappingWithApplicationType(legacyAppFormId);
                                                var statusResp = await _iApplicationMamnagement_ContractLabour_Service.InitiateApplication(ApplicationTypeEnum.CONTRACT_LABOUR, formData, projectSiteRefId, Sys_N_ServiceCodeInfo.ApplicationPurposeType, applications.FirstOrDefault().ApplicationUserID, Convert.ToInt64(ipin), Convert.ToInt64(investPunjabAppId), true, Convert.ToInt64(appId), legacyAppFormId, nar, null, 1).ConfigureAwait(false);
                                                newAppId = statusResp.AppId;

                                                // Update seeding log after master creation
                                                seedingLogs = await GetApplicationSeedingLogsByAppId(Convert.ToInt64(appId), legacyAppFormId, nar).ConfigureAwait(false);

                                                string query = "UPDATE ApplicationSeedingLogs SET NewAppId = " + newAppId + ", IsMasterSeeded = 1 WHERE Id = " + seedingLogs.Id.ToString();
                                                ExecuteQueryUsingAdoNet(query);
                                            }
                                            else if (legacyAppFormId == 17 || legacyAppFormId == 18 && nar.Contains("IPR") || nar.Contains("IPA")) // Principal Employeer Act
                                            {
                                                var masterData = await _iDapperRepository.Get<ProcessImd_ClearencesInterstatePrincipalEmployersViewModel>("SELECT * FROM InterstatePrincipalEmployers WHERE AppId = @AppId AND NAR = @NAR", new { AppId = appId, NAR = nar }, CommandType.Text).ConfigureAwait(true);

                                                if (masterData.Count() == 0)
                                                {
                                                    masterData = await _iDapperRepository.Get<ProcessImd_ClearencesInterstatePrincipalEmployersViewModel>("SELECT * FROM InterstatePrincipalEmployer_Log WHERE AppId = @AppId  AND NAR = @NAR ", new { AppId = appId, NAR = nar }, CommandType.Text).ConfigureAwait(true);

                                                    if (masterData.Count() == 0)
                                                    {
                                                        masterData = await _iDapperRepository.Get<ProcessImd_ClearencesInterstatePrincipalEmployersViewModel>("SELECT * FROM InterstatePrincipalEmployer_Log WHERE AppId = @AppId  ", new { AppId = appId }, CommandType.Text).ConfigureAwait(true);
                                                    }
                                                }
                                                Licence_PE_ISM_GeneralDetail formData = new Licence_PE_ISM_GeneralDetail();
                                                if (masterData.Count() == 0)
                                                {
                                                    {
                                                        formData.PE_Name = "NA";
                                                        formData.PE_FatherName = "NA";
                                                        formData.PE_Mobile = "0000000000";
                                                        formData.PE_Email = "No Email";
                                                        formData.PE_Address = "No Address";
                                                        formData.Manager_Name = "NA";
                                                        formData.Manager_Mobile = "0000000000";
                                                        formData.Manager_Email = "No Email";
                                                        formData.Manager_Address = "No Address";
                                                        formData.NatureOfWork = "NA";
                                                        formData.TotalWorker = 0;
                                                    }
                                                    ;

                                                }
                                                else
                                                {
                                                    {
                                                        formData.PE_Name = masterData.FirstOrDefault().PrincipalEmpName;
                                                        formData.PE_FatherName = masterData.FirstOrDefault().PrincipalFatherEmpName ?? "NA";
                                                        formData.PE_Mobile = masterData.FirstOrDefault().PrincipalEmpMobileNo ?? "0000000000";
                                                        formData.PE_Email = masterData.FirstOrDefault().PrincipalEmpEmailId ?? "No Email";
                                                        formData.PE_Address = masterData.FirstOrDefault().PrincipalEmpAddress ?? "No Address";
                                                        formData.Manager_Name = masterData.FirstOrDefault().ManagerName ?? "NA";
                                                        formData.Manager_Mobile = masterData.FirstOrDefault().ManagerMobileNo ?? "0000000000";
                                                        formData.Manager_Email = masterData.FirstOrDefault().ManagerEmailId ?? "No Email";
                                                        formData.Manager_Address = masterData.FirstOrDefault().ManagerAddress ?? "No Address";
                                                        formData.NatureOfWork = masterData.FirstOrDefault().WorkCarried;
                                                        formData.TotalWorker = 0;

                                                    }
                                                    ;

                                                }

                                                var validations = CustomeValidator<Licence_PE_ISM_GeneralDetail>.ValidateModel_AllProperties(formData);
                                                if (!validations.IsValid)
                                                {
                                                    var kk = validations.CustomeValidationErrorList;
                                                }

                                                var Sys_N_ServiceCodeInfo = FindServiceCodeMappingWithApplicationType(legacyAppFormId);
                                                var statusResp = await _iApplicationMamnagement_Licence_ISM_PrincipalEmployer.InitiateApplication(ApplicationTypeEnum.ISM_PRINCIPAL_EMPLOYER, formData, projectSiteRefId, Sys_N_ServiceCodeInfo.ApplicationPurposeType, applications.FirstOrDefault().ApplicationUserID, Convert.ToInt64(ipin), Convert.ToInt64(investPunjabAppId), true, Convert.ToInt64(appId), legacyAppFormId, nar, null, 1);
                                                newAppId = statusResp.AppId;

                                                // Get Employee Details 
                                                var contractordetails = await _iSystem_O_CommunicationService.GetISMContractorList(Convert.ToInt64(appId), legacyAppFormId, nar);

                                                if (contractordetails != null)
                                                {
                                                    Licence_PE_ISM_Contrator contractorlist = null;
                                                    foreach (var item in contractordetails.ResponseDataModel)
                                                    {
                                                        contractorlist = new Licence_PE_ISM_Contrator()
                                                        {
                                                            Name = item.ContractorName,
                                                            Address = item.ContractorAddress,
                                                            NatureOfWork = item.WorkNatureString ?? "NA",
                                                            MaxLabourWorkerEmployed = (int)item.MaximumNumberEmployee,
                                                            DateOfCommencement = item.ContractWork_CommencingDate,
                                                            DateOfTermination = item.ContractWork_TerminationDate,
                                                            Licence_PE_ISM_GeneralDetailId = statusResp.EntityKeyId,
                                                            ModifiedCounter = 0,
                                                            IsFormVGenerate = 1,
                                                            FormVGenerateDate = DateTime.Now
                                                        };

                                                        validations = CustomeValidator<Licence_PE_ISM_Contrator>.ValidateModel_AllProperties(contractorlist);
                                                        if (!validations.IsValid)
                                                        {
                                                            var kk = validations.CustomeValidationErrorList;
                                                        }

                                                        await _context.Licence_PE_ISM_Contrators.AddAsync(contractorlist);
                                                        await _context.SaveChangesAsync();
                                                    }
                                                }

                                                // Update seeding log after master creation
                                                seedingLogs = await GetApplicationSeedingLogsByAppId(Convert.ToInt64(appId), legacyAppFormId, nar);

                                                string query = "UPDATE ApplicationSeedingLogs SET NewAppId = " + newAppId + ", IsMasterSeeded = 1 WHERE Id = " + seedingLogs.Id.ToString();
                                                ExecuteQueryUsingAdoNet(query);
                                            }
                                            else if (legacyAppFormId == 19 || legacyAppFormId == 20 || legacyAppFormId == 21 && nar.Contains("ICL") || nar.Contains("ICR") || nar.Contains("ICA")) // ISM Contract Labour Act
                                            {
                                                var masterData = await _iDapperRepository.Get<ProcessImd_ClearencesInterstateContractLabourViewModel>("SELECT * FROM InterstateContractLabours WHERE AppId = @AppId AND NAR = @NAR", new { AppId = appId, NAR = nar }, CommandType.Text).ConfigureAwait(true);

                                                if (masterData.Count() == 0)
                                                {
                                                    masterData = await _iDapperRepository.Get<ProcessImd_ClearencesInterstateContractLabourViewModel>("SELECT * FROM InterstateContractLabour_Log WHERE AppId = @AppId  AND NAR = @NAR ", new { AppId = appId, NAR = nar }, CommandType.Text).ConfigureAwait(true);

                                                    if (masterData.Count() == 0)
                                                    {
                                                        masterData = await _iDapperRepository.Get<ProcessImd_ClearencesInterstateContractLabourViewModel>("SELECT * FROM InterstateContractLabour_Log WHERE AppId = @AppId  ", new { AppId = appId }, CommandType.Text).ConfigureAwait(true);
                                                    }

                                                }


                                                Licence_ISM_ContractLabour_GeneralDetail formData = new Licence_ISM_ContractLabour_GeneralDetail();
                                                if (masterData.Count() == 0)
                                                {
                                                    {
                                                        formData.IsCoopraticSociety = "1";
                                                        formData.ContractorName = "NA";
                                                        formData.ContractorFatherName = "NA";
                                                        formData.ContractorAddress = "NA";
                                                        formData.ContractorDOB = DateTime.Today;
                                                        formData.TypeOfBusiness = "NA";
                                                        formData.RegistrationCertificateNo = "NA";
                                                        formData.RegistrationCertificateDate = DateTime.Today;
                                                        formData.PrincipalEmployerName = "NA";
                                                        formData.PrincipalEmployerAddress = "NA";
                                                        formData.NatureOfWork = "NA";
                                                        formData.ContractWork_CommencingDate = DateTime.Today;
                                                        formData.ContractWork_TerminationDate = DateTime.Today;
                                                        formData.AgentOrManagerName = "NA";
                                                        formData.AgentOrManagerAddress = "NA";
                                                        formData.MaximumNumberOfEmployee = 0;
                                                        formData.ContractorAge = 0;
                                                        formData.LicenceForYear = "NA";
                                                        formData.ModifiedCounter = 1;
                                                        formData.GstNumber = "000000000";
                                                        formData.PanOrTanNumber = "ABCDD1234E";

                                                    }
                                                    ;

                                                }
                                                else
                                                {
                                                    {
                                                        formData.IsCoopraticSociety = "1";
                                                        formData.ContractorName = masterData.FirstOrDefault().ContractorName;
                                                        formData.ContractorFatherName = masterData.FirstOrDefault().ContractorFatherName ?? "NA";
                                                        formData.ContractorAddress = masterData.FirstOrDefault().ContractorAddress ?? "NA";
                                                        formData.ContractorDOB = masterData.FirstOrDefault().DOB ?? DateTime.Today;
                                                        formData.TypeOfBusiness = masterData.FirstOrDefault().TypeOfBusiness;
                                                        formData.RegistrationCertificateNo = masterData.FirstOrDefault().RegistrationCertificateNo ?? "NA";
                                                        formData.RegistrationCertificateDate = masterData.FirstOrDefault().RegistrationCertificateDate ?? DateTime.Today;
                                                        formData.PrincipalEmployerName = masterData.FirstOrDefault().PrincipalEmployerName ?? "NA";
                                                        formData.PrincipalEmployerAddress = masterData.FirstOrDefault().PrincipalEmployerAddress ?? "NA";
                                                        formData.NatureOfWork = masterData.FirstOrDefault().WorkNatureString;
                                                        formData.ContractWork_CommencingDate = masterData.FirstOrDefault().ContractWork_CommencingDate;
                                                        formData.ContractWork_TerminationDate = masterData.FirstOrDefault().ContractWork_TerminationDate;
                                                        formData.AgentOrManagerName = masterData.FirstOrDefault().AgentOrManagerName ?? "NA";
                                                        formData.AgentOrManagerAddress = masterData.FirstOrDefault().AgentOrManagerAddress ?? "NA";
                                                        formData.MaximumNumberOfEmployee = masterData.FirstOrDefault().MaximumNumberEmployee;
                                                        formData.ContractorAge = masterData.FirstOrDefault().Age;
                                                        formData.LicenceForYear = masterData.FirstOrDefault().RegistrationFrom;
                                                        formData.ModifiedCounter = 1;
                                                        formData.GstNumber = "000000000";
                                                        formData.PanOrTanNumber = "ABCDD1234E";

                                                    }
                                                    ;

                                                }

                                                var validations = CustomeValidator<Licence_ISM_ContractLabour_GeneralDetail>.ValidateModel_AllProperties(formData);
                                                if (!validations.IsValid)
                                                {
                                                    var kk = validations.CustomeValidationErrorList;
                                                }

                                                var Sys_N_ServiceCodeInfo = FindServiceCodeMappingWithApplicationType(legacyAppFormId);
                                                var statusResp = await _iApplicationMamnagement_Licence_ISM_ContractLabour.InitiateApplication(ApplicationTypeEnum.ISM_CONTRACT_LABOUR, formData, projectSiteRefId, Sys_N_ServiceCodeInfo.ApplicationPurposeType, applications.FirstOrDefault().ApplicationUserID, Convert.ToInt64(ipin), Convert.ToInt64(investPunjabAppId), true, Convert.ToInt64(appId), legacyAppFormId, nar, null, 1).ConfigureAwait(false);
                                                newAppId = statusResp.AppId;

                                                // Update seeding log after master creation
                                                seedingLogs = await GetApplicationSeedingLogsByAppId(Convert.ToInt64(appId), legacyAppFormId, nar).ConfigureAwait(false);

                                                string query = "UPDATE ApplicationSeedingLogs SET NewAppId = " + newAppId + ", IsMasterSeeded = 1 WHERE Id = " + seedingLogs.Id.ToString();
                                                ExecuteQueryUsingAdoNet(query);
                                            }
                                            else if (legacyAppFormId == 35 || legacyAppFormId == 36 && nar.Contains("BOF") || nar.Contains("BOA")) // BOCW Act
                                            {
                                                var masterData = await _iDapperRepository.Get<ProcessImd_ClearencesBocwRegistrationViewModel>("SELECT * FROM BOCWEstbRegistrations WHERE AppId = @AppId AND NAR = @NAR", new { AppId = appId, NAR = nar }, CommandType.Text).ConfigureAwait(true);

                                                if (masterData.Count() == 0)
                                                {
                                                    masterData = await _iDapperRepository.Get<ProcessImd_ClearencesBocwRegistrationViewModel>("SELECT * FROM BOCWEstbRegistration_log WHERE AppId = @AppId  AND NAR = @NAR ", new { AppId = appId, NAR = nar }, CommandType.Text).ConfigureAwait(true);

                                                    if (masterData.Count() == 0)
                                                    {
                                                        masterData = await _iDapperRepository.Get<ProcessImd_ClearencesBocwRegistrationViewModel>("SELECT * FROM BOCWEstbRegistration_log WHERE AppId = @AppId  ", new { AppId = appId }, CommandType.Text).ConfigureAwait(true);
                                                    }

                                                }

                                                var district = await _iDapperRepository.Get<ShopTehsilsViewModel>("SELECT * FROM ShopTehsils WHERE TehsilId =  @TehsilId", new { TehsilId = applications.FirstOrDefault()?.SiteAddTehsilId }, CommandType.Text).ConfigureAwait(true);




                                                Licence_BocwAct_GeneralDetail formData = new Licence_BocwAct_GeneralDetail();
                                                if (masterData.Count() == 0)
                                                {
                                                    {
                                                        formData.BOCWActCircleType = 0;
                                                        formData.ConstructionBuildingType = 0;
                                                        formData.ConstructionBuildingDesc = "NA";
                                                        formData.BocwEngagedWorkerSlabType = 0;
                                                        formData.BocwRegisteredType = 0;
                                                        formData.Work_CommencementDate = DateTime.Now;
                                                        formData.Work_CompletionDate = DateTime.Now;
                                                        formData.MaximumNoOfWorkers = 0;
                                                        formData.AlcCircleRefId = 0;
                                                        formData.FactoryCircleRefId = 0;
                                                        formData.ModifiedCounter = 0;

                                                        formData.ConstructionSite_Name = "NA";
                                                        formData.ConstructionSite_Address = "No Address";
                                                        formData.DistrictLgdRefId = 1;
                                                        formData.TehsilLgdRefId = 1;
                                                        formData.ConstructionSite_PinCode = "0";

                                                        formData.PrincipalEmployerName = "No Address";
                                                        formData.PrincipalEmployerFatherName = "NA";
                                                        formData.PrincipalEmployerAddress = "NA";
                                                        formData.PrincipalEmployerMobileNo = "No Email";
                                                        formData.PrincipalEmployerEmail = "No Address";
                                                        formData.EngagedAnyContractorType = 0;

                                                        formData.ContractLabourLicenceNumber = "0000000000";

                                                        formData.ManagerName = "No Email";
                                                        formData.ManagerAddress = "No Address";
                                                        formData.ManagerEmail = "NA";
                                                        formData.ManagerMobile = "00";

                                                    }
                                                    ;

                                                }
                                                else
                                                {
                                                    {

                                                        formData.BOCWActCircleType = (BOCWActCircleTypeEnum)(masterData.FirstOrDefault().BOCWCircleType == "BOCWALLCID" ? 2 : 1);
                                                        formData.ConstructionBuildingType = (ConstructionBuildingTypeEnum)(masterData.FirstOrDefault().BOCWCircleType == "BOCWALLCID" ? 2 : 1);
                                                        formData.ConstructionBuildingDesc = "NA";
                                                        formData.BocwEngagedWorkerSlabType = (BocwEngagedWorkerSlabTypeEnum)(masterData.FirstOrDefault().MaximumNumberOfWorkers > 11 ? 2 : 1);
                                                        formData.BocwRegisteredType = (BocwRegisteredTypeEnum)1;
                                                        formData.Work_CommencementDate = masterData.FirstOrDefault().Commencement_Date;
                                                        formData.Work_CompletionDate = masterData.FirstOrDefault().Completion_Date;
                                                        formData.MaximumNoOfWorkers = masterData.FirstOrDefault().MaximumNumberOfWorkers;
                                                        formData.AlcCircleRefId = masterData.FirstOrDefault()?.BOCWCircleType == "BOCWALLCID" ? applications.FirstOrDefault()?.ALLCCircleId ?? 0 : 0;
                                                        formData.FactoryCircleRefId = masterData.FirstOrDefault()?.BOCWCircleType == "BOCWFCID" ? applications.FirstOrDefault()?.FCId ?? 0 : 0;
                                                        formData.ModifiedCounter = 1;

                                                        formData.ConstructionSite_Name = (masterData.FirstOrDefault().EstablishmentName == null || masterData.FirstOrDefault().EstablishmentName == "") ? "NA" : masterData.FirstOrDefault().EstablishmentName;
                                                        formData.ConstructionSite_Address = (masterData.FirstOrDefault().EstablishmentAddress == null || masterData.FirstOrDefault().EstablishmentAddress == "") ? "NA" : masterData.FirstOrDefault().EstablishmentAddress;
                                                        formData.DistrictLgdRefId = 1;
                                                        formData.TehsilLgdRefId = 1;
                                                        formData.ConstructionSite_PinCode = (applications.FirstOrDefault()?.SiteAddPinCode == null || applications.FirstOrDefault()?.SiteAddPinCode == "") ? "000000" : applications.FirstOrDefault()?.SiteAddPinCode;

                                                        formData.PrincipalEmployerName = (masterData.FirstOrDefault().PrincipalEmpName == null || masterData.FirstOrDefault().PrincipalEmpName == "") ? "NA" : masterData.FirstOrDefault().PrincipalEmpName;
                                                        formData.PrincipalEmployerFatherName = "NA";
                                                        formData.PrincipalEmployerAddress = (masterData.FirstOrDefault().PrincipalEmpAddress == null || masterData.FirstOrDefault().PrincipalEmpAddress == "") ? "NA" : masterData.FirstOrDefault().PrincipalEmpAddress;
                                                        formData.PrincipalEmployerMobileNo = (masterData.FirstOrDefault().EstbEmployerMobile == null || masterData.FirstOrDefault().EstbEmployerMobile == "") ? "9999999999" : masterData.FirstOrDefault().EstbEmployerMobile;
                                                        formData.PrincipalEmployerEmail = (masterData.FirstOrDefault().EstbEmployerEmail == null || masterData.FirstOrDefault().EstbEmployerEmail == "") ? "abc@gmail.com" : masterData.FirstOrDefault().EstbEmployerEmail;
                                                        formData.EngagedAnyContractorType = (EngagedAnyContractorTypeEnum)2;

                                                        formData.ContractLabourLicenceNumber = "NA";

                                                        formData.ManagerName = (masterData.FirstOrDefault().ManagerName == null || masterData.FirstOrDefault().ManagerName == "") ? "NA" : masterData.FirstOrDefault().ManagerName;
                                                        formData.ManagerAddress = (masterData.FirstOrDefault().ManagerAddress == null || masterData.FirstOrDefault().ManagerAddress == "") ? "NA" : masterData.FirstOrDefault().ManagerAddress;
                                                        formData.ManagerEmail = (masterData.FirstOrDefault().ManagerEmail == null || masterData.FirstOrDefault().ManagerEmail == "") ? "abc@gmail.com" : masterData.FirstOrDefault().ManagerEmail;
                                                        formData.ManagerMobile = (masterData.FirstOrDefault().ManagerMobile == null || masterData.FirstOrDefault().ManagerMobile == "") ? "9999999999" : masterData.FirstOrDefault().ManagerMobile;

                                                    }
                                                    ;

                                                }

                                                var validations = CustomeValidator<Licence_BocwAct_GeneralDetail>.ValidateModel_AllProperties(formData);
                                                if (!validations.IsValid)
                                                {
                                                    var kk = validations.CustomeValidationErrorList;
                                                }

                                                var Sys_N_ServiceCodeInfo = FindServiceCodeMappingWithApplicationType(legacyAppFormId);
                                                var statusResp = await _iApplicationMamnagement_Licence_BocwAct_GeneralDetail.InitiateApplication(ApplicationTypeEnum.BOCW_ESTABLISHMENT_ACT, formData, projectSiteRefId, Sys_N_ServiceCodeInfo.ApplicationPurposeType, applications.FirstOrDefault().ApplicationUserID, Convert.ToInt64(ipin), Convert.ToInt64(investPunjabAppId), true, Convert.ToInt64(appId), legacyAppFormId, nar, null, 1);
                                                newAppId = statusResp.AppId;


                                                // Update seeding log after master creation
                                                seedingLogs = await GetApplicationSeedingLogsByAppId(Convert.ToInt64(appId), legacyAppFormId, nar);

                                                string query = "UPDATE ApplicationSeedingLogs SET NewAppId = " + newAppId + ", IsMasterSeeded = 1 WHERE Id = " + seedingLogs.Id.ToString();
                                                ExecuteQueryUsingAdoNet(query);
                                            }

                                        }
                                        #endregion

                                        if (newAppId > 0)
                                        {
                                            #region Business First Request Log
                                            if (seededInfo != null && seededInfo.IsBusinessRequestLogStatusSeeded == SeedStatusTypeEnum.NO_STATUS)
                                            {
                                                var projectsiteid = _context.Applications.Where(x => x.AppId == newAppId && x.IsDeleted == false).FirstOrDefault().ProjectSiteRefId;
                                                var applicationuserId = _context.ProjectSites.Where(x => x.ProjectSiteId == projectsiteid).FirstOrDefault().UserRefId;
                                                BusinessFirst_RequestLog businessFirst_Request = new BusinessFirst_RequestLog()
                                                {
                                                    AppId = Convert.ToInt64(investPunjabAppId),
                                                    IPin = Convert.ToInt64(ipin),
                                                    UserId = applicationuserId,
                                                    ServiceCode = legacyAppFormId,
                                                    CategoryTypeId = 1,
                                                    RequestCount = 1,
                                                    NativeAppId = newAppId,
                                                    CreatedDate = DateTime.Now,
                                                    LastModifiedDate = DateTime.Now,
                                                    RequestString = "",
                                                    IsEnabled = true,
                                                    NativeUserId = null
                                                };

                                                // Add the mapping to the context
                                                _context.BusinessFirst_RequestLogs.Add(businessFirst_Request);
                                                _context.SaveChanges();

                                                seedingLogs = await GetApplicationSeedingLogsByAppId(Convert.ToInt64(appId), legacyAppFormId, nar).ConfigureAwait(false);
                                                ExecuteQueryUsingAdoNet("UPDATE ApplicationSeedingLogs SET IsBusinessRequestLogStatusSeeded = 1 WHERE Id=" + seedingLogs.Id.ToString());


                                            }
                                            #endregion


                                            if (seededInfo != null && seededInfo.IsDocumentSeeded == SeedStatusTypeEnum.NO_STATUS)
                                            {
                                                #region Documents
                                                var legacyAppDocuments = await _iDapperRepository.Get<LegacyAppDocumentViewModel>("SELECT DISTINCT DocId,FileName,TDate FROM AppDocuments where AppId = @AppId AND NAR = @NAR UNION SELECT DISTINCT DocId,FileName,TDate FROM AppDocuments_Log where AppId = @AppId AND NAR = @NAR", new { AppId = appId, NAR = nar }, CommandType.Text).ConfigureAwait(true);
                                                foreach (var doc in legacyAppDocuments)
                                                {
                                                    // Check if DocId is null (nullable int) or if FileName is null or empty
                                                    if (doc.DocId == 0 || string.IsNullOrEmpty(doc.FileName))
                                                    {
                                                        continue;  // Skip the current iteration and move to the next document
                                                    }

                                                    ApplicationDocument appDocument = new ApplicationDocument()
                                                    {
                                                        AppRefId = newAppId,
                                                        AttachmentName = doc.FileName,
                                                        DocumentRefId = doc.DocId,
                                                        IsUploaded = true,
                                                        LastModifiedDate = doc.TDate,
                                                        Uploadeddate = doc.TDate,
                                                        IsLocked = true
                                                    };

                                                    _iGR_AppDocument.Insert(appDocument);
                                                    _iGR_AppDocument.Savechange();

                                                    string filename = string.Empty;

                                                    // Create the filename based on DocId and AppDocId
                                                    filename = doc.FileName.Substring(doc.FileName.LastIndexOf('.'));
                                                    filename = newAppId.ToString() + "_" + doc.DocId.ToString() + "_" + appDocument.AppDocId + filename;

                                                    var foundDocument = _iGR_AppDocument.GetById(appDocument.AppDocId);
                                                    foundDocument.AttachmentName = filename.ToLower();

                                                    _iGR_AppDocument.Update(foundDocument);
                                                    _iGR_AppDocument.Savechange();

                                                    LegacyDocumentMapping legacyDocumentMapping = new LegacyDocumentMapping()
                                                    {
                                                        Legacy_DocumentName = doc.FileName,
                                                        Legacy_DocumentId = doc.DocId,
                                                        NewDocumentName = filename,
                                                        Legacy_AppId = Convert.ToInt64(appId),
                                                        Legacy_AppFormId = legacyAppFormId,
                                                        Legacy_NAR = nar,
                                                        AppRefId = newAppId,
                                                        IsMoved = false,
                                                        IsApproval = false
                                                    };

                                                    // Add the mapping to the context
                                                    _context.LegacyDocumentMappings.Add(legacyDocumentMapping);
                                                    _context.SaveChanges();
                                                }

                                                seedingLogs = await GetApplicationSeedingLogsByAppId(Convert.ToInt64(appId), legacyAppFormId, nar).ConfigureAwait(false);
                                                ExecuteQueryUsingAdoNet("UPDATE ApplicationSeedingLogs SET IsDocumentSeeded = 1 WHERE Id=" + seedingLogs.Id.ToString());

                                                #endregion
                                            }

                                            if (seededInfo != null && seededInfo.IsPaymentSeeded == SeedStatusTypeEnum.NO_STATUS)
                                            {
                                                #region Payment

                                                List<StoreProcedureParm> storeProcedureParms = new List<StoreProcedureParm>()
                                            {
                                                new StoreProcedureParm (){ ParmName="AppId", ParmValue=appId, isNumber=true},
                                                new StoreProcedureParm (){ ParmName="NAR", ParmValue=nar, isNumber=false}
                                            };

                                                var legacyPayments = await _iGeneric_SP_Repository.CallStoreProcedureReaderAsync<LegacyAppPaymentDetailsViewModel>("dbo.sp_GetLegacyPaymentDetailByAppId", storeProcedureParms).ConfigureAwait(false);

                                                if (legacyPayments.Count() > 0)
                                                {
                                                    AppFeeDetail appFeeDetail = new AppFeeDetail()
                                                    {
                                                        AppRefId = newAppId,
                                                        FeeHeaderRefId = 35,
                                                        Amount = legacyPayments.FirstOrDefault().TransactionAmount,
                                                        CalculatedOn = legacyPayments.FirstOrDefault().TransactionDate,
                                                        IsDeduductible = false,
                                                        PaymentBatchCounter = 1,
                                                        PaymentPartCounter = 0,
                                                        HasDedicatedTreasuryCode = false,
                                                        DedicatedTreasurCode = null,
                                                        DedicatedDDOCode = null,
                                                        Description = null,
                                                    };
                                                    _context.AppFeeDetails.Add(appFeeDetail);
                                                    _context.SaveChanges();

                                                    //_iGR_AppFeeDetail

                                                    //Step - 4. Insert into AppFeeTransaction
                                                    AppFeeTransaction appFeeTransaction = new AppFeeTransaction()
                                                    {
                                                        TransactionInitializationDate = legacyPayments.FirstOrDefault().PaymentDate,
                                                        PaymentGatewayType = PaymentGatewayTypeEnum.IFMS,
                                                        PaymentModeType = PaymentModeTypeEnum.ONLINE,
                                                        PaymentTreasuryType = PaymentTreasuryTypeEnum.NON_TREASURY,
                                                        PaymentGatewayTargetUrl = "NA",
                                                        PaymentGatewayApiMethodType = PaymentGatewayApiMethodTypeEnum.POST,
                                                        UniquePaymentGatewayTransactionId = legacyPayments.FirstOrDefault().TransactionId == null ? legacyPayments.FirstOrDefault().BankTransactionRefNumber : legacyPayments.FirstOrDefault().TransactionId,
                                                        IsWebRequestCycleCompleted = true,
                                                        RequestBodyData = legacyPayments.FirstOrDefault().ReqMsg,
                                                        ResponseBodyData = legacyPayments.FirstOrDefault().ResMsg,
                                                        ResponseReceivedOn = legacyPayments.FirstOrDefault().PaymentDate,
                                                        ResponseMessage = legacyPayments.FirstOrDefault().ResMsg,
                                                        TransactionFinalStatusType = TransactionFinalStatusTypeEnum.SUCCEED,
                                                        AmountCalculated = legacyPayments.FirstOrDefault().TransactionAmount,
                                                        PaymentBatchCounter = 1,
                                                        PaymentPartCounter = 1,
                                                        BankTransactionRefNumber1 = legacyPayments.FirstOrDefault().BankTransactionRefNumber,
                                                        BankTransactionRefNumber1_Type = BankTransactionReferenceTypeEnum.BANK_REF_NUMBER,
                                                        BankTransactionRefNumber2 = legacyPayments.FirstOrDefault().BankTransactionRefNumber,
                                                        BankTransactionRefNumber2_Type = BankTransactionReferenceTypeEnum.BANK_REF_NUMBER,
                                                        BankSettlementOn = legacyPayments.FirstOrDefault().PaymentDate,
                                                        AppRefId = newAppId
                                                    };
                                                    _context.AppFeeTransactions.Add(appFeeTransaction);
                                                    _context.SaveChanges();

                                                    //step - 5. Insert into SuccessMapping Table
                                                    AppPaymentSuccessTransactionMapping appPaymentSuccessTransactionMapping = new AppPaymentSuccessTransactionMapping()
                                                    {
                                                        AppRefId = newAppId,
                                                        AppFeeTransactionRefId = appFeeTransaction.AppFeeTransactionId,
                                                        PaymentBatchCounter = 1,
                                                        PaymentPartCounter = 1,
                                                    };
                                                    _context.AppPaymentSuccessTransactionMappings.Add(appPaymentSuccessTransactionMapping);
                                                    _context.SaveChanges();

                                                    // Update seeding log after payment seeded
                                                    seedingLogs = await GetApplicationSeedingLogsByAppId(Convert.ToInt64(appId), legacyAppFormId, nar);
                                                    ExecuteQueryUsingAdoNet("UPDATE ApplicationSeedingLogs SET IsPaymentSeeded = 1 WHERE Id=" + seedingLogs.Id.ToString());
                                                }
                                                else // Fee Not Found In Db
                                                {
                                                    seedingLogs = await GetApplicationSeedingLogsByAppId(Convert.ToInt64(appId), legacyAppFormId, nar);
                                                    ExecuteQueryUsingAdoNet("UPDATE ApplicationSeedingLogs SET IsPaymentSeeded = 3 WHERE Id=" + seedingLogs.Id.ToString());

                                                }

                                                #endregion
                                            }

                                            if (seededInfo != null && (seededInfo.IsApplicationLogsSeeded == SeedStatusTypeEnum.NO_STATUS || seededInfo.IsApplicationLogsSeeded == SeedStatusTypeEnum.ERROR))
                                            {
                                                try
                                                {

                                                    #region ApplicationLogs

                                                    List<ApplicationActionLog> actionLogList = new List<ApplicationActionLog>();

                                                    // Step 1 : Grouping the unique logs
                                                    var distinctLogs = allLogs.GroupBy(log => new { log.StatusId, log.StatusDate }).Select(group => group.First()).ToList();

                                                    // Step 1: Separate logs with StatusId = 1 and StatusId = 2
                                                    var statusId1And2Logs = distinctLogs.Where(log => log.StatusId == 1 || log.StatusId == 2).OrderBy(log => log.StatusId).ToList();

                                                    // Step 2: Separate logs with StatusId other than 1 and 2
                                                    var otherLogs = distinctLogs.Where(log => log.StatusId != 1 && log.StatusId != 2).ToList();

                                                    // Step 3: Combine the two lists, with logs having StatusId = 1 and 2 coming first
                                                    var orderedLogs = statusId1And2Logs.Concat(otherLogs).ToList();

                                                    foreach (var log in orderedLogs.Where(x => x.NAR == nar).ToList())
                                                    {
                                                        var receiverRoleId = await _iDapperRepository.Get<string>("SELECT RoleId FROM AspNetUserRoles WHERE UserId =@UserId", new { UserId = log.Receiver }, CommandType.Text).ConfigureAwait(false);
                                                        var senderRoleId = await _iDapperRepository.Get<string>("SELECT RoleId FROM AspNetUserRoles WHERE UserId =@UserId", new { UserId = log.Sender }, CommandType.Text).ConfigureAwait(false);
                                                        var senderProfile = await _iAuthService.GetUserProfileByUserRefId(log.Sender).ConfigureAwait(false);
                                                        var receiverProfile = await _iAuthService.GetUserProfileByUserRefId(log.Receiver).ConfigureAwait(false);

                                                        ApplicationActionLog actionLog = new ApplicationActionLog();
                                                        actionLog.ActionDate = log.StatusDate;
                                                        actionLog.ActionTakenDaysCount = 0;
                                                        actionLog.ActionTakenHoursCount = 0;
                                                        actionLog.AppActionType = GetSys_N_ActionCode(Convert.ToInt32(log.StatusId));
                                                        actionLog.ApplicationRefId = newAppId;
                                                        actionLog.Receiver_ProfileRefId = receiverProfile == null ? 404 : receiverProfile.UserProfileRefId;
                                                        actionLog.Receiver_UserRefId = log.Receiver;
                                                        actionLog.Sender_ProfileRefId = senderProfile == null ? 404 : senderProfile.UserProfileRefId;
                                                        actionLog.Sender_UserRefId = log.Sender;
                                                        actionLog.Remarks = log.StatusDesc;
                                                        actionLog.ReceiverRoleId = new long[] { 5, 9, 206 }.Contains(log.StatusId) ? "592add3e-f992-4983-a8ca-21ddc090bda0" : receiverRoleId.FirstOrDefault().ToString();
                                                        actionLog.SenderRoleId = new long[] { 1, 2, 3 }.Contains(log.StatusId) ? "592add3e-f992-4983-a8ca-21ddc090bda0" : senderRoleId.FirstOrDefault().ToString();
                                                        actionLog.AppDocumentRefId = 0;
                                                        actionLog.IsDocumentUploaded = false;
                                                        actionLog.Legacy_IsMigrated = true;
                                                        actionLog.Legacy_AppId = Convert.ToInt64(appId);
                                                        actionLog.Legacy_AppFormId = legacyAppFormId;
                                                        actionLog.Legacy_NAR = nar;
                                                        actionLog.Legacy_StatusId = Convert.ToInt32(log.StatusId);
                                                        actionLog.IpAddress = "Manual";
                                                        actionLog.Latitude = "Manual";
                                                        actionLog.Longitude = "Manual";
                                                        actionLogList.Add(actionLog);
                                                    }
                                                    _context.BulkInsert<ApplicationActionLog>(actionLogList);

                                                    var actionSenderProfile = await _iAuthService.GetUserProfileByUserRefId(actionLogList.LastOrDefault().Sender_UserRefId).ConfigureAwait(false);
                                                    var actionReceiverProfile = await _iAuthService.GetUserProfileByUserRefId(actionLogList.LastOrDefault().Receiver_UserRefId).ConfigureAwait(false);


                                                    if (actionSenderProfile == null)
                                                    {
                                                        List<StoreProcedureParm> storeProcedureParms = new List<StoreProcedureParm>()
                                                    {
                                                        new StoreProcedureParm (){ ParmName="AspNetUserId", ParmValue=actionLogList.LastOrDefault().Sender_UserRefId, isNumber=false}
                                                    };

                                                        var userdata = await _iGeneric_SP_Repository.CallStoreProcedureReaderScalerAsync("dbo.sp_SeedLegacyUsersToUpgradedPortal", storeProcedureParms).ConfigureAwait(false);
                                                        actionSenderProfile = await _iAuthService.GetUserProfileByUserRefId(actionLogList.LastOrDefault().Sender_UserRefId).ConfigureAwait(false);
                                                    }

                                                    if (actionReceiverProfile == null)
                                                    {
                                                        List<StoreProcedureParm> storeProcedureParms = new List<StoreProcedureParm>()
                                                    {
                                                        new StoreProcedureParm (){ ParmName="AspNetUserId", ParmValue=actionLogList.LastOrDefault().Receiver_UserRefId, isNumber=false}
                                                    };

                                                        var userdata = await _iGeneric_SP_Repository.CallStoreProcedureReaderScalerAsync("dbo.sp_SeedLegacyUsersToUpgradedPortal", storeProcedureParms).ConfigureAwait(false);
                                                        actionReceiverProfile = await _iAuthService.GetUserProfileByUserRefId(actionLogList.LastOrDefault().Receiver_UserRefId).ConfigureAwait(false);
                                                    }
                                                    var applicationAction = _context.ApplicationActions.Where(x => x.ApplicationRefId == newAppId).FirstOrDefault();
                                                    if (applicationAction != null)
                                                    {
                                                        applicationAction.ActionDate = actionLogList.LastOrDefault().ActionDate;
                                                        applicationAction.ActionTakenDaysCount = 0;
                                                        applicationAction.ActionTakenHoursCount = 0;
                                                        applicationAction.AppActionType = actionLogList.LastOrDefault().AppActionType;
                                                        applicationAction.ApplicationRefId = actionLogList.LastOrDefault().ApplicationRefId;
                                                        applicationAction.Receiver_ProfileRefId = actionLogList.LastOrDefault().Receiver_UserRefId == "0c54c0ad-a4c7-42b0-ab4a-4e524c2addf" ? 5 :
                                                                                actionLogList.LastOrDefault().Receiver_UserRefId == "1aee535c-4794-4e7b-acaa-e5a0acf4af33" ? 12 :
                                                                                actionReceiverProfile.UserProfileRefId;

                                                        applicationAction.Receiver_UserRefId = actionLogList.LastOrDefault().Receiver_UserRefId == "0c54c0ad-a4c7-42b0-ab4a-4e524c2addf" ? "C29E5FD2-8FB9-49AC-BCF1-7E06769FAAD9" :
                                                                                actionLogList.LastOrDefault().Receiver_UserRefId == "1aee535c-4794-4e7b-acaa-e5a0acf4af33" ? "C29E5FD2-8FB9-49AC-BCF1-7E06769FAAD9" :
                                                                                actionLogList.LastOrDefault().Receiver_UserRefId;

                                                        applicationAction.Sender_ProfileRefId = actionLogList.LastOrDefault().Sender_UserRefId == "0c54c0ad-a4c7-42b0-ab4a-4e524c2addf" ? 5 :
                                                                                actionLogList.LastOrDefault().Sender_UserRefId == "1aee535c-4794-4e7b-acaa-e5a0acf4af33" ? 12 :
                                                                                actionSenderProfile.UserProfileRefId;

                                                        applicationAction.Sender_UserRefId = actionLogList.LastOrDefault().Sender_UserRefId == "0c54c0ad-a4c7-42b0-ab4a-4e524c2addf" ? "C29E5FD2-8FB9-49AC-BCF1-7E06769FAAD9" :
                                                                            actionLogList.LastOrDefault().Sender_UserRefId == "1aee535c-4794-4e7b-acaa-e5a0acf4af33" ? "C29E5FD2-8FB9-49AC-BCF1-7E06769FAAD9" :
                                                                            actionLogList.LastOrDefault().Sender_UserRefId;

                                                        applicationAction.Remarks = actionLogList.LastOrDefault().Remarks;
                                                        applicationAction.ReceiverRoleId = actionLogList.LastOrDefault().ReceiverRoleId;
                                                        applicationAction.SenderRoleId = actionLogList.LastOrDefault().SenderRoleId;
                                                        applicationAction.AppDocumentRefId = 0;
                                                        applicationAction.IsDocumentUploaded = false;
                                                        applicationAction.Legacy_IsMigrated = true;
                                                        applicationAction.Legacy_AppId = actionLogList.LastOrDefault().Legacy_AppId;
                                                        applicationAction.Legacy_AppFormId = actionLogList.LastOrDefault().Legacy_AppFormId;
                                                        applicationAction.Legacy_NAR = actionLogList.LastOrDefault().Legacy_NAR;
                                                        applicationAction.Legacy_StatusId = actionLogList.LastOrDefault().Legacy_StatusId;
                                                        applicationAction.IpAddress = "Manual";
                                                        applicationAction.Latitude = "Manual";
                                                        applicationAction.Longitude = "Manual";

                                                        _context.ApplicationActions.Update(applicationAction);
                                                        _context.SaveChanges();

                                                    }
                                                    ;



                                                    //update application life cycle
                                                    var application = _context.Applications.Where(x => x.AppId == newAppId && x.IsDeleted == false).FirstOrDefault();
                                                    int appLifeCycleStatusType = _context.AppActionTypeMappings.Where(x => x.NewAppActionType == applicationAction.AppActionType).Select(x => x.ApplicationLifeCycleStatusType).FirstOrDefault();

                                                    application.ApplicationLifeCycleStatusType = (ApplicationLifeCycleStatusTypeEnum)appLifeCycleStatusType;
                                                    application.ApplicationLifeCycleLastStatusOn = actionLogList.LastOrDefault().ActionDate;
                                                    application.CreatedOnDate = actionLogList.FirstOrDefault().ActionDate;
                                                    application.IsTimeLineFlow = false;
                                                    _context.Update<Application>(application);
                                                    _context.SaveChanges();

                                                    // Update seeding log after logs seeded
                                                    seedingLogs = await GetApplicationSeedingLogsByAppId(Convert.ToInt64(appId), legacyAppFormId, nar);
                                                    ExecuteQueryUsingAdoNet("UPDATE ApplicationSeedingLogs SET IsApplicationLogsSeeded = 1 WHERE Id=" + seedingLogs.Id.ToString());
                                                    #endregion
                                                }
                                                catch (Exception ex1)
                                                {
                                                    seedingLogs = await GetApplicationSeedingLogsByAppId(Convert.ToInt64(appId), legacyAppFormId, nar);
                                                    ExecuteQueryUsingAdoNet("UPDATE ApplicationSeedingLogs SET IsApplicationLogsSeeded = 4 WHERE Id=" + seedingLogs.Id.ToString());
                                                    continue;
                                                }
                                            }

                                            if (seededInfo != null && seededInfo.IsApprovalsSeeded == SeedStatusTypeEnum.NO_STATUS)
                                            {
                                                #region Approvals
                                                var actionlogs = _context.ApplicationActions.Where(x => x.ApplicationRefId == newAppId).FirstOrDefault();
                                                if (actionlogs != null && (actionlogs.AppActionType == (int)AppActionTypeEnum.APP_APPROVED || actionlogs.AppActionType == (int)AppActionTypeEnum.DEEMED_COMPLETED))
                                                {
                                                    var legacyApprovals = await _iDapperRepository.Get<LegacyAppClearanceIssuedViewModel>("SELECT * FROM AppClearanceIssueds WHERE AppId = @AppId AND AppFormId = @AppFormId AND NAR = @NAR ORDER BY 1 DESC", new { AppId = appId, AppFormId = legacyAppFormId, NAR = nar }, CommandType.Text).ConfigureAwait(false);

                                                    if (legacyApprovals.Count() > 0)
                                                    {
                                                        var app = _context.Applications.Where(x => x.AppId == newAppId && x.IsDeleted == false).FirstOrDefault();

                                                        string qry = @"INSERT INTO [dbo].[LegacyDocumentMappings]([Legacy_DocumentName],[Legacy_DocumentId],[NewDocumentName],[Legacy_AppId],[Legacy_AppFormId],[Legacy_NAR],[AppRefId],[IsMoved],[IsApproval]) VALUES("
                                                                     + "'" + legacyApprovals.FirstOrDefault().LicencePath + "'"
                                                                     + ",'" + legacyApprovals.FirstOrDefault().DocId + "'"
                                                                     + ",'" + app.PublicAppRefNum + ".pdf" + "'"
                                                                     + "," + appId
                                                                     + "," + legacyAppFormId
                                                                     + ",'" + nar + "'"
                                                                     + "," + newAppId
                                                                     + ",0"
                                                                     + ",1)";

                                                        ExecuteQueryUsingAdoNet(qry);

                                                        ExecuteQueryUsingAdoNet("INSERT INTO ApplicationLicenceNoMapping (AppRefId, LicenceNumber) VALUES(" + newAppId + "," + "'" + legacyApprovals.FirstOrDefault().LicenceNo + "'" + ")");
                                                    }
                                                    else
                                                    {
                                                        seedingLogs = await GetApplicationSeedingLogsByAppId(Convert.ToInt64(appId), legacyAppFormId, nar).ConfigureAwait(false);
                                                        ExecuteQueryUsingAdoNet("UPDATE ApplicationSeedingLogs SET IsApprovalsSeeded = 3 WHERE Id=" + seedingLogs.Id.ToString());
                                                    }
                                                }
                                                else
                                                {

                                                }

                                                // Update seeding log after approval seeded
                                                seedingLogs = await GetApplicationSeedingLogsByAppId(Convert.ToInt64(appId), legacyAppFormId, nar).ConfigureAwait(false);
                                                ExecuteQueryUsingAdoNet("UPDATE ApplicationSeedingLogs SET IsApprovalsSeeded = 1 WHERE Id=" + seedingLogs.Id.ToString());

                                                #endregion
                                            }

                                            // All the steps of data seeding is completed
                                            seedingLogs = await GetApplicationSeedingLogsByAppId(Convert.ToInt64(appId), legacyAppFormId, nar).ConfigureAwait(false);
                                            int allStepsCompleted = 0;
                                            if ((seedingLogs.IsUserSeeded == SeedStatusTypeEnum.SEEDED || seedingLogs.IsUserSeeded == SeedStatusTypeEnum.NO_SEEDING_REQUIRED)
                                                && seedingLogs.IsProfileSeeded == SeedStatusTypeEnum.SEEDED
                                                && seedingLogs.IsProjectSiteSeeded == SeedStatusTypeEnum.SEEDED
                                                && seedingLogs.IsDocumentSeeded == SeedStatusTypeEnum.SEEDED
                                                && seedingLogs.IsPaymentSeeded == SeedStatusTypeEnum.SEEDED
                                                && seedingLogs.IsApplicationLogsSeeded == SeedStatusTypeEnum.SEEDED
                                                && seedingLogs.IsApprovalsSeeded == SeedStatusTypeEnum.SEEDED
                                                && seedingLogs.IsBusinessRequestLogStatusSeeded == SeedStatusTypeEnum.SEEDED)
                                            {
                                                allStepsCompleted = 1;
                                            }

                                            string query = "UPDATE ApplicationSeedingLogs SET OverAllSeedStatus = " + allStepsCompleted + ", SeedStatusDiscription = 'All Steps Completed' WHERE Id = " + seedingLogs.Id.ToString();
                                            ExecuteQueryUsingAdoNet(query);
                                        }
                                        else
                                        {
                                            seedingLogs = await GetApplicationSeedingLogsByAppId(Convert.ToInt64(appId), legacyAppFormId, nar);
                                            ExecuteQueryUsingAdoNet("UPDATE ApplicationSeedingLogs SET OverAllSeedStatus = 0,SeedStatusDiscription ='No new app available'  WHERE Id=" + seedingLogs.Id.ToString());
                                        }
                                    }
                                }
                                catch (Exception ex)
                                {
                                    var foundSeededInfo = await GetApplicationSeedingLogsByAppId(Convert.ToInt64(appId), legacyAppFormId, nar);
                                    var msg = ex.InnerException != null ? Regex.Replace(ex.InnerException.ToString(), @"'", "") : Regex.Replace(ex.Message, @"'", "");

                                    List<StoreProcedureParm> storeProcedureParms = new List<StoreProcedureParm>()
                                {
                                    new StoreProcedureParm (){ ParmName="Id", ParmValue= foundSeededInfo.Id.ToString(), isNumber=true },
                                    new StoreProcedureParm (){ ParmName="ExceptionText", ParmValue= msg, isNumber=false }
                                };
                                    var resp = await _iGeneric_SP_Repository.CallStoreProcedureReaderScalerAsync("dbo.sp_updateAppSeedingLogsError", storeProcedureParms);

                                    continue;
                                }
                            }
                        }
                    }
                    //}
                }
            }
            catch (Exception ex)
            {
                genericServiceResultTemplate.HasError = true;
                genericServiceResultTemplate.ErrorDesc = ex.Message;
                throw ex;
            }
            return genericServiceResultTemplate;
        }
        public async Task<IpinAndAppIdViewModel> GetIpinAndApplicationIdByAppIdAndAppFormId(Int64 appId, int legacyAppFormId)
        {
            IpinAndAppIdViewModel ipinAndAppId = new IpinAndAppIdViewModel();
            {
                try
                {
                    var resp = await _iDapperRepository.Get<IpinAndAppIdViewModel>("SELECT TOP 1 BF.iPin AS Ipin, BF.AppId AS ApplicationId FROM Applications AP INNER JOIN BF_Integration_Addi_Info BF ON AP.AppId = BF.E_LAB_AppId WHERE AP.AppId = @AppId AND BF.ServiceCode = @ServiceCode ORDER BY BF.PDate DESC", new { AppId = appId, ServiceCode = legacyAppFormId }, CommandType.Text).ConfigureAwait(true);
                    ipinAndAppId = resp.FirstOrDefault();
                }
                catch (Exception ex)
                {

                }
            }
            return ipinAndAppId;
        }

        public async Task<ApplicationSeedingLog> GetApplicationSeedingLogsByAppId(Int64 appId, int appFormId, string nar)
        {
            return await _context.ApplicationSeedingLogs.Where(x => x.Legacy_AppId == appId && x.Legacy_AppFormId == appFormId && x.Legacy_NAR == nar).FirstOrDefaultAsync();
        }
        private void ExecuteQueryUsingAdoNet(string query)
        {
            SqlConnection cn = new SqlConnection(Configuration.GetConnectionString("SQLServerConnection"));
            SqlCommand cmd = new SqlCommand(query, cn);
            cn.Open();
            cmd.ExecuteNonQuery();
            cn.Close();
        }

        public async Task<GenericResponseTemplateModel<dynamic>> GetHRMSCodeAndActWiseData(HRMSRequestParamsViewModel requestData)
        {
            GenericResponseTemplateModel<dynamic> genericServiceResultTemplate = new GenericResponseTemplateModel<dynamic>();
            try
            {
                //HRMSApplicationPurposeTypeViewModel hRMSApplicationPurposeTypeData = new HRMSApplicationPurposeTypeViewModel();
                if (requestData != null)
                {
                    string fromDateFormatted = requestData.fromDate.ToString("yyyy-MM-dd HH:mm:ss");
                    string toDateFormatted = requestData.toDate.ToString("yyyy-MM-dd HH:mm:ss");

                    List<StoreProcedureParm> storeProcedureParm = new List<StoreProcedureParm>()
                    {
                        new StoreProcedureParm { ParmName = "FromDate", ParmValue = fromDateFormatted, isNumber = false },
                        new StoreProcedureParm { ParmName = "ToDate", ParmValue = toDateFormatted, isNumber = false },
                        new StoreProcedureParm { ParmName = "HrmsCode", ParmValue = requestData.hrmsCode.ToString(), isNumber = true }
                    };

                    var roleIds = await _iGeneric_SP_Repository.CallStoreProcedureReaderAsync<HRMS_RoleIdByHRMSServiceCodeViewModel>("sp_HRMS_getRolesByHRMSCode", storeProcedureParm);

                    var allowedServiceCodes = _context.RoleWiseAllowedHRMSServiceCodes.Where(x => roleIds.Select(x => x.RoleId).ToArray().Contains(x.RoleId)).ToList();

                    var data = new List<HRMSDataViewModel>();
                    foreach (var item in allowedServiceCodes)
                    {
                        storeProcedureParm = new List<StoreProcedureParm>()
                        {
                        new StoreProcedureParm { ParmName = "FromDate", ParmValue = fromDateFormatted, isNumber = false },
                        new StoreProcedureParm { ParmName = "ToDate", ParmValue = toDateFormatted, isNumber = false },
                        //new StoreProcedureParm { ParmName = "serviceCode", ParmValue = requestData.FirstOrDefault().serviceCode.ToString(), isNumber = true },
                        new StoreProcedureParm { ParmName = "HrmsCode", ParmValue = requestData.hrmsCode.ToString(), isNumber = true },
                        new StoreProcedureParm { ParmName = "ApplicationType", ParmValue = item.ApplicationType.ToString(), isNumber = true },
                        new StoreProcedureParm { ParmName = "ApplicationPurposeType", ParmValue = item.ApplicationPurposeType.ToString(), isNumber = true },
                        };

                        data.AddRange(await _iGeneric_SP_Repository.CallStoreProcedureReaderAsync<HRMSDataViewModel>("sp_HRMS_GetHRMSCodeAndActWiseData", storeProcedureParm));
                    }
                    var distinctCircles = data.Select(x => x.CircleName).Distinct();
                    dynamic obj = new ExpandoObject();
                    obj.services = new List<dynamic>();
                    dynamic serviceObj = new ExpandoObject();
                    foreach (var item in allowedServiceCodes)
                    {
                        serviceObj = new ExpandoObject();
                        serviceObj.ServiceCode = item.HRMSServiceCode;
                        serviceObj.ServiceName = item.ServiceName;
                        serviceObj.data = new List<dynamic>();
                        if (roleIds.FirstOrDefault().RoleId == "5b70c7bd-b591-4300-a34c-56b4bddc9416" || roleIds.FirstOrDefault().RoleId == "ad7c34a9-e2b5-4131-984a-c3b03ff9b4aa")
                        {
                            if (item.HRMSServiceCode == 1007)
                            {
                                foreach (var circle in distinctCircles)
                                {
                                    serviceObj.data.Add(new
                                    {
                                        CircleName = circle,
                                        CountHeads = new
                                        {

                                            NumberOfInspectionsPendingInTheBeginningOfTheYear = data.Where(x => x.ApplicationType == item.ApplicationType && x.applicationPurposeType == item.ApplicationPurposeType && x.CircleName == circle).Select(x => x.TotalPendingInBeginning).FirstOrDefault(),
                                            NumberOfInspectionsAssignedInTheYear = data.Where(x => x.ApplicationType == item.ApplicationType && x.applicationPurposeType == item.ApplicationPurposeType && x.CircleName == circle).Select(x => x.TotalReceived).FirstOrDefault(),
                                            TotalNumberOfInspections = data.Where(x => x.ApplicationType == item.ApplicationType && x.applicationPurposeType == item.ApplicationPurposeType && x.CircleName == circle).Select(x => x.TotalPendingInBeginning).FirstOrDefault() + data.Where(x => x.ApplicationType == item.ApplicationType && x.applicationPurposeType == item.ApplicationPurposeType && x.CircleName == circle).Select(x => x.TotalReceived).FirstOrDefault(),
                                            NumberOfInspectionsConductedDuringTheYear = data.Where(x => x.ApplicationType == item.ApplicationType && x.applicationPurposeType == item.ApplicationPurposeType && x.CircleName == circle).Select(x => x.TotalApproved).FirstOrDefault(),
                                            NumberOfInspectionsPendingAtTheEndOfTheYear = data.Where(x => x.ApplicationType == item.ApplicationType && x.applicationPurposeType == item.ApplicationPurposeType && x.CircleName == circle).Select(x => x.TotalPending).FirstOrDefault()
                                        }
                                    });
                                }
                            }
                            if (item.HRMSServiceCode == 1004)
                            {

                                foreach (var circle in distinctCircles)
                                {
                                    serviceObj.data.Add(new
                                    {
                                        CircleName = circle,
                                        CountHeads = new
                                        {
                                            NumberOfApplicationsPendingInTheBeginningOfTheYear = data.Where(x => x.ApplicationType == item.ApplicationType && x.applicationPurposeType == item.ApplicationPurposeType && x.CircleName == circle).Select(x => x.TotalPendingInBeginning).FirstOrDefault(),
                                            ReceivedDuringTheYear = data.Where(x => x.ApplicationType == item.ApplicationType && x.applicationPurposeType == item.ApplicationPurposeType && x.CircleName == circle).Select(x => x.TotalReceived).FirstOrDefault(),
                                            TotalNumberOfCases = data.Where(x => x.ApplicationType == item.ApplicationType && x.applicationPurposeType == item.ApplicationPurposeType && x.CircleName == circle).Select(x => x.TotalPendingInBeginning).FirstOrDefault() + data.Where(x => x.ApplicationType == item.ApplicationType && x.applicationPurposeType == item.ApplicationPurposeType && x.CircleName == circle).Select(x => x.TotalReceived).FirstOrDefault(),
                                            NumberOfLicenceRenewedDuringTheYear = data.Where(x => x.ApplicationType == item.ApplicationType && x.applicationPurposeType == item.ApplicationPurposeType && x.CircleName == circle).Select(x => x.TotalApproved).FirstOrDefault(),
                                            ObjectionRaisedDuringTheYear = data.Where(x => x.ApplicationType == item.ApplicationType && x.applicationPurposeType == item.ApplicationPurposeType && x.CircleName == circle).Select(x => x.TotalObjectionRaised).FirstOrDefault(),
                                            FilesRejectedDuringTheYear = data.Where(x => x.ApplicationType == item.ApplicationType && x.applicationPurposeType == item.ApplicationPurposeType && x.CircleName == circle).Select(x => x.TotalRejected).FirstOrDefault(),
                                            NumberOfApplicationsPendingAtTheEndOfYear = data.Where(x => x.ApplicationType == item.ApplicationType && x.applicationPurposeType == item.ApplicationPurposeType && x.CircleName == circle).Select(x => x.TotalPending).FirstOrDefault()
                                        }
                                    });
                                }
                            }
                            if (item.HRMSServiceCode == 1005)
                            {

                                foreach (var circle in distinctCircles)
                                {
                                    serviceObj.data.Add(new
                                    {
                                        CircleName = circle,
                                        CountHeads = new
                                        {
                                            NumberOfApplicationsPendingInTheBeginningOfTheYear = data.Where(x => x.ApplicationType == item.ApplicationType && x.applicationPurposeType == item.ApplicationPurposeType && x.CircleName == circle).Select(x => x.TotalPendingInBeginning).FirstOrDefault(),
                                            ReceivedDuringTheYear = data.Where(x => x.ApplicationType == item.ApplicationType && x.applicationPurposeType == item.ApplicationPurposeType && x.CircleName == circle).Select(x => x.TotalReceived).FirstOrDefault(),
                                            TotalNumberOfCases = data.Where(x => x.ApplicationType == item.ApplicationType && x.applicationPurposeType == item.ApplicationPurposeType && x.CircleName == circle).Select(x => x.TotalPendingInBeginning).FirstOrDefault() + data.Where(x => x.ApplicationType == item.ApplicationType && x.applicationPurposeType == item.ApplicationPurposeType && x.CircleName == circle).Select(x => x.TotalReceived).FirstOrDefault(),
                                            NumberOfLicenceAmendedDuringTheYear = data.Where(x => x.ApplicationType == item.ApplicationType && x.applicationPurposeType == item.ApplicationPurposeType && x.CircleName == circle).Select(x => x.TotalApproved).FirstOrDefault(),
                                            ObjectionRaisedDuringTheYear = data.Where(x => x.ApplicationType == item.ApplicationType && x.applicationPurposeType == item.ApplicationPurposeType && x.CircleName == circle).Select(x => x.TotalObjectionRaised).FirstOrDefault(),
                                            FilesRejectedDuringTheYear = data.Where(x => x.ApplicationType == item.ApplicationType && x.applicationPurposeType == item.ApplicationPurposeType && x.CircleName == circle).Select(x => x.TotalRejected).FirstOrDefault(),
                                            NumberOfApplicationsPendingAtTheEndOfYear = data.Where(x => x.ApplicationType == item.ApplicationType && x.applicationPurposeType == item.ApplicationPurposeType && x.CircleName == circle).Select(x => x.TotalPending).FirstOrDefault()
                                        }
                                    });
                                }
                            }
                            else
                            {

                                foreach (var circle in distinctCircles)
                                {
                                    serviceObj.data.Add(new
                                    {
                                        CircleName = circle,
                                        CountHeads = new
                                        {
                                            NumberOfApplicationsPendingInTheBeginningOfTheYear = data.Where(x => x.ApplicationType == item.ApplicationType && x.applicationPurposeType == item.ApplicationPurposeType && x.CircleName == circle).Select(x => x.TotalPendingInBeginning).FirstOrDefault(),
                                            ReceivedDuringTheYear = data.Where(x => x.ApplicationType == item.ApplicationType && x.applicationPurposeType == item.ApplicationPurposeType && x.CircleName == circle).Select(x => x.TotalReceived).FirstOrDefault(),
                                            TotalNumberOfCases = data.Where(x => x.ApplicationType == item.ApplicationType && x.applicationPurposeType == item.ApplicationPurposeType && x.CircleName == circle).Select(x => x.TotalPendingInBeginning).FirstOrDefault() + data.Where(x => x.ApplicationType == item.ApplicationType && x.applicationPurposeType == item.ApplicationPurposeType && x.CircleName == circle).Select(x => x.TotalReceived).FirstOrDefault(),
                                            NumberOfRegistrationIssuedAndLicenceGrantedDuringTheYear = data.Where(x => x.ApplicationType == item.ApplicationType && x.applicationPurposeType == item.ApplicationPurposeType && x.CircleName == circle).Select(x => x.TotalApproved).FirstOrDefault(),
                                            ObjectionRaisedDuringTheYear = data.Where(x => x.ApplicationType == item.ApplicationType && x.applicationPurposeType == item.ApplicationPurposeType && x.CircleName == circle).Select(x => x.TotalObjectionRaised).FirstOrDefault(),
                                            FilesRejectedDuringTheYear = data.Where(x => x.ApplicationType == item.ApplicationType && x.applicationPurposeType == item.ApplicationPurposeType && x.CircleName == circle).Select(x => x.TotalRejected).FirstOrDefault(),
                                            NumberOfApplicationsPendingAtTheEndOfYear = data.Where(x => x.ApplicationType == item.ApplicationType && x.applicationPurposeType == item.ApplicationPurposeType && x.CircleName == circle).Select(x => x.TotalPending).FirstOrDefault()
                                        }
                                    });
                                }
                            }
                            obj.services.Add(serviceObj);
                        }

                        if (roleIds.FirstOrDefault().RoleId == "71abc5f6-bacc-47b4-bd98-bc4cc08c3088")
                        {
                            if (item.HRMSServiceCode == 2013)
                            {
                                foreach (var circle in distinctCircles)
                                {
                                    serviceObj.data.Add(new
                                    {
                                        CircleName = circle,
                                        CountHeads = new
                                        {
                                            PendingInTheBeginningOfTheYear = data.Where(x => x.ApplicationType == item.ApplicationType && x.applicationPurposeType == item.ApplicationPurposeType && x.CircleName == circle).Select(x => x.TotalPendingInBeginning).FirstOrDefault(),
                                            ApplicationsReceived = data.Where(x => x.ApplicationType == item.ApplicationType && x.applicationPurposeType == item.ApplicationPurposeType && x.CircleName == circle).Select(x => x.TotalReceived).FirstOrDefault(),
                                            ObjectionsRaised = data.Where(x => x.ApplicationType == item.ApplicationType && x.applicationPurposeType == item.ApplicationPurposeType && x.CircleName == circle).Select(x => x.TotalPendingInBeginning).FirstOrDefault() + data.Where(x => x.ApplicationType == item.ApplicationType && x.applicationPurposeType == item.ApplicationPurposeType && x.CircleName == circle).Select(x => x.TotalObjectionRaised).FirstOrDefault(),
                                            ApplicationsApproved = data.Where(x => x.ApplicationType == item.ApplicationType && x.applicationPurposeType == item.ApplicationPurposeType && x.CircleName == circle).Select(x => x.TotalApproved).FirstOrDefault(),
                                            PendingAtTheEndOfTheYear = data.Where(x => x.ApplicationType == item.ApplicationType && x.applicationPurposeType == item.ApplicationPurposeType && x.CircleName == circle).Select(x => x.TotalPending).FirstOrDefault()

                                        }
                                    });
                                }
                            }
                            if (item.HRMSServiceCode == 2014)
                            {
                                foreach (var circle in distinctCircles)
                                {
                                    serviceObj.data.Add(new
                                    {
                                        CircleName = circle,
                                        CountHeads = new
                                        {
                                            PendingInTheBeginningOfTheYear = data.Where(x => x.ApplicationType == item.ApplicationType && x.applicationPurposeType == item.ApplicationPurposeType && x.CircleName == circle).Select(x => x.TotalPendingInBeginning).FirstOrDefault(),
                                            ApplicationsReceived = data.Where(x => x.ApplicationType == item.ApplicationType && x.applicationPurposeType == item.ApplicationPurposeType && x.CircleName == circle).Select(x => x.TotalReceived).FirstOrDefault(),
                                            ObjectionsRaised = data.Where(x => x.ApplicationType == item.ApplicationType && x.applicationPurposeType == item.ApplicationPurposeType && x.CircleName == circle).Select(x => x.TotalPendingInBeginning).FirstOrDefault() + data.Where(x => x.ApplicationType == item.ApplicationType && x.applicationPurposeType == item.ApplicationPurposeType && x.CircleName == circle).Select(x => x.TotalObjectionRaised).FirstOrDefault(),
                                            ApplicationsApproved = data.Where(x => x.ApplicationType == item.ApplicationType && x.applicationPurposeType == item.ApplicationPurposeType && x.CircleName == circle).Select(x => x.TotalApproved).FirstOrDefault(),
                                            PendingAtTheEndOfTheYear = data.Where(x => x.ApplicationType == item.ApplicationType && x.applicationPurposeType == item.ApplicationPurposeType && x.CircleName == circle).Select(x => x.TotalPending).FirstOrDefault()

                                        }
                                    });
                                }
                            }
                            else
                            {

                                foreach (var circle in distinctCircles)
                                {
                                    serviceObj.data.Add(new
                                    {
                                        CircleName = circle,
                                        CountHeads = new
                                        {
                                            PendingInTheBeginningOfTheYear = data.Where(x => x.ApplicationType == item.ApplicationType && x.applicationPurposeType == item.ApplicationPurposeType && x.CircleName == circle).Select(x => x.TotalPendingInBeginning).FirstOrDefault(),
                                            Received = data.Where(x => x.ApplicationType == item.ApplicationType && x.applicationPurposeType == item.ApplicationPurposeType && x.CircleName == circle).Select(x => x.TotalReceived).FirstOrDefault(),
                                            ObjectionsRaised = data.Where(x => x.ApplicationType == item.ApplicationType && x.applicationPurposeType == item.ApplicationPurposeType && x.CircleName == circle).Select(x => x.TotalObjectionRaised).FirstOrDefault(),
                                            Approved = data.Where(x => x.ApplicationType == item.ApplicationType && x.applicationPurposeType == item.ApplicationPurposeType && x.CircleName == circle).Select(x => x.TotalApproved).FirstOrDefault(),
                                            PendingAtTheEndOfTheYear = data.Where(x => x.ApplicationType == item.ApplicationType && x.applicationPurposeType == item.ApplicationPurposeType && x.CircleName == circle).Select(x => x.TotalPending).FirstOrDefault(),
                                        }
                                    });
                                }
                            }
                            obj.services.Add(serviceObj);
                        }

                        if (roleIds.FirstOrDefault().RoleId == "60c208bd-100d-442a-bb3b-a677e9a95877")
                        {
                            if (item.HRMSServiceCode == 3002)
                            {
                                foreach (var circle in distinctCircles)
                                {
                                    serviceObj.data.Add(new
                                    {
                                        CircleName = circle,
                                        CountHeads = new
                                        {
                                            PendingInTheBeginningOfTheYear = data.Where(x => x.ApplicationType == item.ApplicationType && x.applicationPurposeType == item.ApplicationPurposeType && x.CircleName == circle).Select(x => x.TotalPendingInBeginning).FirstOrDefault(),
                                            Inspectionsassignedduringtheyear = data.Where(x => x.ApplicationType == item.ApplicationType && x.applicationPurposeType == item.ApplicationPurposeType && x.CircleName == circle).Select(x => x.TotalReceived).FirstOrDefault(),
                                            InspectionsSubmitted = data.Where(x => x.ApplicationType == item.ApplicationType && x.applicationPurposeType == item.ApplicationPurposeType && x.CircleName == circle).Select(x => x.TotalApproved).FirstOrDefault(),
                                            NoOfInspectionsPendingAtTheEndOfTheYear = data.Where(x => x.ApplicationType == item.ApplicationType && x.applicationPurposeType == item.ApplicationPurposeType && x.CircleName == circle).Select(x => x.TotalPending).FirstOrDefault()
                                        }
                                    });
                                }
                            }
                            if (item.HRMSServiceCode == 3003 || item.HRMSServiceCode == 3004 || item.HRMSServiceCode == 3005)
                            {
                                foreach (var circle in distinctCircles)
                                {
                                    serviceObj.data.Add(new
                                    {
                                        CircleName = circle,
                                        CountHeads = new
                                        {
                                            PendingInTheBeginningOfTheYear = data.Where(x => x.ApplicationType == item.ApplicationType && x.applicationPurposeType == item.ApplicationPurposeType && x.CircleName == circle).Select(x => x.TotalPendingInBeginning).FirstOrDefault(),
                                            ApplicationsReceived = data.Where(x => x.ApplicationType == item.ApplicationType && x.applicationPurposeType == item.ApplicationPurposeType && x.CircleName == circle).Select(x => x.TotalReceived).FirstOrDefault(),
                                            ObjectionsResolved = data.Where(x => x.ApplicationType == item.ApplicationType && x.applicationPurposeType == item.ApplicationPurposeType && x.CircleName == circle).Select(x => x.TotalReceived).FirstOrDefault(),
                                            ObjectionsRaised = data.Where(x => x.ApplicationType == item.ApplicationType && x.applicationPurposeType == item.ApplicationPurposeType && x.CircleName == circle).Select(x => x.TotalPendingInBeginning).FirstOrDefault() + data.Where(x => x.ApplicationType == item.ApplicationType && x.applicationPurposeType == item.ApplicationPurposeType && x.CircleName == circle).Select(x => x.TotalObjectionRaised).FirstOrDefault(),
                                            ApplicationsApproved = data.Where(x => x.ApplicationType == item.ApplicationType && x.applicationPurposeType == item.ApplicationPurposeType && x.CircleName == circle).Select(x => x.TotalApproved).FirstOrDefault(),
                                            PendingAtTheEndOfTheYear = data.Where(x => x.ApplicationType == item.ApplicationType && x.applicationPurposeType == item.ApplicationPurposeType && x.CircleName == circle).Select(x => x.TotalPending).FirstOrDefault()

                                        }
                                    });
                                }
                            }
                            else
                            {

                                foreach (var circle in distinctCircles)
                                {
                                    serviceObj.data.Add(new
                                    {
                                        CircleName = circle,
                                        CountHeads = new
                                        {
                                            PendingInTheBeginningOfTheYear = data.Where(x => x.ApplicationType == item.ApplicationType && x.applicationPurposeType == item.ApplicationPurposeType && x.CircleName == circle).Select(x => x.TotalPendingInBeginning).FirstOrDefault(),
                                            ApplicationsReceivedNew = data.Where(x => x.ApplicationType == item.ApplicationType && x.applicationPurposeType == item.ApplicationPurposeType && x.CircleName == circle).Select(x => x.TotalReceived).FirstOrDefault(),
                                            ObjectionsRaised = data.Where(x => x.ApplicationType == item.ApplicationType && x.applicationPurposeType == item.ApplicationPurposeType && x.CircleName == circle).Select(x => x.TotalObjectionRaised).FirstOrDefault(),
                                            ObjectionsResolved = data.Where(x => x.ApplicationType == item.ApplicationType && x.applicationPurposeType == item.ApplicationPurposeType && x.CircleName == circle).Select(x => x.TotalReceived).FirstOrDefault(),
                                            ApplicationsApproved = data.Where(x => x.ApplicationType == item.ApplicationType && x.applicationPurposeType == item.ApplicationPurposeType && x.CircleName == circle).Select(x => x.TotalApproved).FirstOrDefault(),
                                            ApplicationsAutoapproved = data.Where(x => x.ApplicationType == item.ApplicationType && x.applicationPurposeType == item.ApplicationPurposeType && x.CircleName == circle).Select(x => x.TotalApproved).FirstOrDefault(),
                                            NumberOfApplicationsPendingAtTheEndOfYear = data.Where(x => x.ApplicationType == item.ApplicationType && x.applicationPurposeType == item.ApplicationPurposeType && x.CircleName == circle).Select(x => x.TotalPending).FirstOrDefault(),
                                        }
                                    });
                                }
                            }
                            obj.services.Add(serviceObj);
                        }

                    }
                    List<StoreProcedureParm> storeProcedureParm2 = new List<StoreProcedureParm>()
                    {
                        new StoreProcedureParm { ParmName = "hrmsCode", ParmValue = requestData.hrmsCode.ToString(), isNumber = true }
                    };
                    var officerDetails = await _iGeneric_SP_Repository.CallStoreProcedureReaderAsync<HRMS_OfficerDetailsViewModel>("sp_HRMS_GetProfileDetailsByHRMSCode", storeProcedureParm2);

                    var distinctAppPurposeTypes = data.Select(x => x.applicationPurposeType).Distinct().ToList();

                    obj.OfficerName = officerDetails.FirstOrDefault().OfficerName;
                    obj.HrmsCode = requestData.hrmsCode;

                    genericServiceResultTemplate.ResponseDataModel = obj;
                }
            }
            catch (Exception ex)
            {
                genericServiceResultTemplate.HasError = true;
                genericServiceResultTemplate.ErrorDesc = ex.Message;
                throw ex;
            }
            return genericServiceResultTemplate;
        }

        public async Task<GenericResponseTemplateModel<dynamic>> GetHRMSCodeAndActWiseData_FactoryWing(HRMSRequestParamsViewModel requestData)
        {
            GenericResponseTemplateModel<dynamic> genericServiceResultTemplate = new GenericResponseTemplateModel<dynamic>();
            try
            {
                //HRMSApplicationPurposeTypeViewModel hRMSApplicationPurposeTypeData = new HRMSApplicationPurposeTypeViewModel();
                if (requestData != null)
                {
                    string fromDateFormatted = requestData.fromDate.ToString("yyyy-MM-dd HH:mm:ss");
                    string toDateFormatted = requestData.toDate.ToString("yyyy-MM-dd HH:mm:ss");

                    List<StoreProcedureParm> storeProcedureParm = new List<StoreProcedureParm>()
                    {
                        new StoreProcedureParm { ParmName = "FromDate", ParmValue = fromDateFormatted, isNumber = false },
                        new StoreProcedureParm { ParmName = "ToDate", ParmValue = toDateFormatted, isNumber = false },
                        new StoreProcedureParm { ParmName = "HrmsCode", ParmValue = requestData.hrmsCode.ToString(), isNumber = true }
                    };

                    var roleIds = await _iGeneric_SP_Repository.CallStoreProcedureReaderAsync<HRMS_RoleIdByHRMSServiceCodeViewModel>("sp_HRMS_getRolesByHRMSCode", storeProcedureParm);

                    var allowedServiceCodes = _context.RoleWiseAllowedHRMSServiceCodes.Where(x => roleIds.Select(x => x.RoleId).ToArray().Contains(x.RoleId)).ToList();

                    var data = new List<HRMSDataViewModel>();
                    foreach (var item in allowedServiceCodes)
                    {
                        storeProcedureParm = new List<StoreProcedureParm>()
                        {
                        new StoreProcedureParm { ParmName = "FromDate", ParmValue = fromDateFormatted, isNumber = false },
                        new StoreProcedureParm { ParmName = "ToDate", ParmValue = toDateFormatted, isNumber = false },
                        //new StoreProcedureParm { ParmName = "serviceCode", ParmValue = requestData.FirstOrDefault().serviceCode.ToString(), isNumber = true },
                        new StoreProcedureParm { ParmName = "HrmsCode", ParmValue = requestData.hrmsCode.ToString(), isNumber = true },
                        new StoreProcedureParm { ParmName = "ApplicationType", ParmValue = item.ApplicationType.ToString(), isNumber = true },
                        new StoreProcedureParm { ParmName = "ApplicationPurposeType", ParmValue = item.ApplicationPurposeType.ToString(), isNumber = true },
                        };

                        data.AddRange(await _iGeneric_SP_Repository.CallStoreProcedureReaderAsync<HRMSDataViewModel>("sp_HRMS_GetHRMSCodeAndActWiseData", storeProcedureParm));
                    }
                    var distinctCircles = data.Where(x => x.CircleName != null).Select(x => x.CircleName).Distinct().ToList();
                    dynamic obj = new ExpandoObject();
                    obj.services = new List<dynamic>();
                    obj.inspections = new List<dynamic>();
                    dynamic serviceObj = new ExpandoObject();
                    foreach (var item in allowedServiceCodes)
                    {
                        serviceObj = new ExpandoObject();
                        serviceObj.ServiceCode = item.HRMSServiceCode;
                        serviceObj.ServiceName = item.ServiceName;
                        serviceObj.data = new List<dynamic>();

                        if (item.HRMSServiceCode == 1006)
                        {
                            foreach (var circle in distinctCircles)
                            {
                                serviceObj.data.Add(new
                                {
                                    CircleName = circle,
                                    CountHeads = new
                                    {

                                        NumberOfInspectionsPendingInTheBeginningOfTheYear = data.Where(x => x.ApplicationType == item.ApplicationType && x.applicationPurposeType == item.ApplicationPurposeType && x.CircleName == circle).Select(x => x.TotalPendingInBeginning).FirstOrDefault(),
                                        NumberOfInspectionsAssignedInTheYear = data.Where(x => x.ApplicationType == item.ApplicationType && x.applicationPurposeType == item.ApplicationPurposeType && x.CircleName == circle).Select(x => x.TotalReceived).FirstOrDefault(),
                                        TotalNumberOfInspections = data.Where(x => x.ApplicationType == item.ApplicationType && x.applicationPurposeType == item.ApplicationPurposeType && x.CircleName == circle).Select(x => x.TotalPendingInBeginning).FirstOrDefault() + data.Where(x => x.ApplicationType == item.ApplicationType && x.applicationPurposeType == item.ApplicationPurposeType && x.CircleName == circle).Select(x => x.TotalReceived).FirstOrDefault(),
                                        NumberOfInspectionsConductedDuringTheYear = data.Where(x => x.ApplicationType == item.ApplicationType && x.applicationPurposeType == item.ApplicationPurposeType && x.CircleName == circle).Select(x => x.TotalApproved).FirstOrDefault(),
                                        NumberOfInspectionsPendingAtTheEndOfTheYear = data.Where(x => x.ApplicationType == item.ApplicationType && x.applicationPurposeType == item.ApplicationPurposeType && x.CircleName == circle).Select(x => x.TotalPending).FirstOrDefault()
                                    }
                                });
                            }
                            obj.inspections.Add(serviceObj);
                        }

                        if (item.HRMSServiceCode == 1004)
                        {

                            foreach (var circle in distinctCircles)
                            {
                                serviceObj.data.Add(new
                                {
                                    CircleName = circle,
                                    CountHeads = new
                                    {
                                        NumberOfApplicationsPendingInTheBeginningOfTheYear = data.Where(x => x.ApplicationType == item.ApplicationType && x.applicationPurposeType == item.ApplicationPurposeType && x.CircleName == circle).Select(x => x.TotalPendingInBeginning).FirstOrDefault(),
                                        ReceivedDuringTheYear = data.Where(x => x.ApplicationType == item.ApplicationType && x.applicationPurposeType == item.ApplicationPurposeType && x.CircleName == circle).Select(x => x.TotalReceived).FirstOrDefault(),
                                        TotalNumberOfCases = data.Where(x => x.ApplicationType == item.ApplicationType && x.applicationPurposeType == item.ApplicationPurposeType && x.CircleName == circle).Select(x => x.TotalPendingInBeginning).FirstOrDefault() + data.Where(x => x.ApplicationType == item.ApplicationType && x.applicationPurposeType == item.ApplicationPurposeType && x.CircleName == circle).Select(x => x.TotalReceived).FirstOrDefault(),
                                        // NumberOfLicenceRenewedDuringTheYear = data.Where(x => x.ApplicationType == item.ApplicationType && x.applicationPurposeType == item.ApplicationPurposeType && x.CircleName == circle).Select(x => x.TotalApproved).FirstOrDefault(),
                                        NumberOfRegistrationIssuedAndLicenceGrantedDuringTheYear = data.Where(x => x.ApplicationType == item.ApplicationType && x.applicationPurposeType == item.ApplicationPurposeType && x.CircleName == circle).Select(x => x.TotalApproved).FirstOrDefault(),
                                        ObjectionRaisedDuringTheYear = data.Where(x => x.ApplicationType == item.ApplicationType && x.applicationPurposeType == item.ApplicationPurposeType && x.CircleName == circle).Select(x => x.TotalObjectionRaised).FirstOrDefault(),
                                        FilesRejectedDuringTheYear = data.Where(x => x.ApplicationType == item.ApplicationType && x.applicationPurposeType == item.ApplicationPurposeType && x.CircleName == circle).Select(x => x.TotalRejected).FirstOrDefault(),
                                        NumberOfApplicationsPendingAtTheEndOfYear = data.Where(x => x.ApplicationType == item.ApplicationType && x.applicationPurposeType == item.ApplicationPurposeType && x.CircleName == circle).Select(x => x.TotalPending).FirstOrDefault()
                                    }
                                });
                            }
                            obj.services.Add(serviceObj);
                        }
                        if (item.HRMSServiceCode == 1005)
                        {

                            foreach (var circle in distinctCircles)
                            {
                                serviceObj.data.Add(new
                                {
                                    CircleName = circle,
                                    CountHeads = new
                                    {
                                        NumberOfApplicationsPendingInTheBeginningOfTheYear = data.Where(x => x.ApplicationType == item.ApplicationType && x.applicationPurposeType == item.ApplicationPurposeType && x.CircleName == circle).Select(x => x.TotalPendingInBeginning).FirstOrDefault(),
                                        ReceivedDuringTheYear = data.Where(x => x.ApplicationType == item.ApplicationType && x.applicationPurposeType == item.ApplicationPurposeType && x.CircleName == circle).Select(x => x.TotalReceived).FirstOrDefault(),
                                        TotalNumberOfCases = data.Where(x => x.ApplicationType == item.ApplicationType && x.applicationPurposeType == item.ApplicationPurposeType && x.CircleName == circle).Select(x => x.TotalPendingInBeginning).FirstOrDefault() + data.Where(x => x.ApplicationType == item.ApplicationType && x.applicationPurposeType == item.ApplicationPurposeType && x.CircleName == circle).Select(x => x.TotalReceived).FirstOrDefault(),
                                       // NumberOfLicenceAmendedDuringTheYear = data.Where(x => x.ApplicationType == item.ApplicationType && x.applicationPurposeType == item.ApplicationPurposeType && x.CircleName == circle).Select(x => x.TotalApproved).FirstOrDefault(),
                                        NumberOfRegistrationIssuedAndLicenceGrantedDuringTheYear = data.Where(x => x.ApplicationType == item.ApplicationType && x.applicationPurposeType == item.ApplicationPurposeType && x.CircleName == circle).Select(x => x.TotalApproved).FirstOrDefault(),
                                        ObjectionRaisedDuringTheYear = data.Where(x => x.ApplicationType == item.ApplicationType && x.applicationPurposeType == item.ApplicationPurposeType && x.CircleName == circle).Select(x => x.TotalObjectionRaised).FirstOrDefault(),
                                        FilesRejectedDuringTheYear = data.Where(x => x.ApplicationType == item.ApplicationType && x.applicationPurposeType == item.ApplicationPurposeType && x.CircleName == circle).Select(x => x.TotalRejected).FirstOrDefault(),
                                        NumberOfApplicationsPendingAtTheEndOfYear = data.Where(x => x.ApplicationType == item.ApplicationType && x.applicationPurposeType == item.ApplicationPurposeType && x.CircleName == circle).Select(x => x.TotalPending).FirstOrDefault()
                                    }
                                });
                            }
                            obj.services.Add(serviceObj);
                        }
                        if (item.HRMSServiceCode == 1003 || item.HRMSServiceCode == 2011)
                        {

                            foreach (var circle in distinctCircles)
                            {
                                serviceObj.data.Add(new
                                {
                                    CircleName = circle,
                                    CountHeads = new
                                    {
                                        NumberOfApplicationsPendingInTheBeginningOfTheYear = data.Where(x => x.ApplicationType == item.ApplicationType && x.applicationPurposeType == item.ApplicationPurposeType && x.CircleName == circle).Select(x => x.TotalPendingInBeginning).FirstOrDefault(),
                                        ReceivedDuringTheYear = data.Where(x => x.ApplicationType == item.ApplicationType && x.applicationPurposeType == item.ApplicationPurposeType && x.CircleName == circle).Select(x => x.TotalReceived).FirstOrDefault(),
                                        TotalNumberOfCases = data.Where(x => x.ApplicationType == item.ApplicationType && x.applicationPurposeType == item.ApplicationPurposeType && x.CircleName == circle).Select(x => x.TotalPendingInBeginning).FirstOrDefault() + data.Where(x => x.ApplicationType == item.ApplicationType && x.applicationPurposeType == item.ApplicationPurposeType && x.CircleName == circle).Select(x => x.TotalReceived).FirstOrDefault(),
                                        NumberOfRegistrationIssuedAndLicenceGrantedDuringTheYear = data.Where(x => x.ApplicationType == item.ApplicationType && x.applicationPurposeType == item.ApplicationPurposeType && x.CircleName == circle).Select(x => x.TotalApproved).FirstOrDefault(),
                                        ObjectionRaisedDuringTheYear = data.Where(x => x.ApplicationType == item.ApplicationType && x.applicationPurposeType == item.ApplicationPurposeType && x.CircleName == circle).Select(x => x.TotalObjectionRaised).FirstOrDefault(),
                                        FilesRejectedDuringTheYear = data.Where(x => x.ApplicationType == item.ApplicationType && x.applicationPurposeType == item.ApplicationPurposeType && x.CircleName == circle).Select(x => x.TotalRejected).FirstOrDefault(),
                                        NumberOfApplicationsPendingAtTheEndOfYear = data.Where(x => x.ApplicationType == item.ApplicationType && x.applicationPurposeType == item.ApplicationPurposeType && x.CircleName == circle).Select(x => x.TotalPending).FirstOrDefault()
                                    }
                                });
                            }
                            obj.services.Add(serviceObj);
                        }
                    }
                    List<StoreProcedureParm> storeProcedureParm2 = new List<StoreProcedureParm>()
                    {
                        new StoreProcedureParm { ParmName = "hrmsCode", ParmValue = requestData.hrmsCode.ToString(), isNumber = true }
                    };
                    var officerDetails = await _iGeneric_SP_Repository.CallStoreProcedureReaderAsync<HRMS_OfficerDetailsViewModel>("sp_HRMS_GetProfileDetailsByHRMSCode", storeProcedureParm2);

                    var distinctAppPurposeTypes = data.Select(x => x.applicationPurposeType).Distinct().ToList();

                    obj.OfficerName = officerDetails.FirstOrDefault().OfficerName;
                    obj.HrmsCode = requestData.hrmsCode;
                    obj.Type = "FactoryWing";

                    genericServiceResultTemplate.ResponseDataModel = obj;
                }
            }
            catch (Exception ex)
            {
                genericServiceResultTemplate.HasError = true;
                genericServiceResultTemplate.ErrorDesc = ex.Message;
                throw ex;
            }
            return genericServiceResultTemplate;
        }

        public async Task<GenericResponseTemplateModel<dynamic>> GetHRMSCodeAndActWiseData_ALCWing(HRMSRequestParamsViewModel requestData)
        {
            GenericResponseTemplateModel<dynamic> genericServiceResultTemplate = new GenericResponseTemplateModel<dynamic>();
            try
            {
                //HRMSApplicationPurposeTypeViewModel hRMSApplicationPurposeTypeData = new HRMSApplicationPurposeTypeViewModel();
                if (requestData != null)
                {
                    string fromDateFormatted = requestData.fromDate.ToString("yyyy-MM-ddTHH:mm:ssZ");
                    string toDateFormatted = requestData.toDate.ToString("yyyy-MM-ddTHH:mm:ssZ");

                    List<StoreProcedureParm> storeProcedureParm = new List<StoreProcedureParm>()
                    {
                        new StoreProcedureParm { ParmName = "FromDate", ParmValue = fromDateFormatted, isNumber = false },
                        new StoreProcedureParm { ParmName = "ToDate", ParmValue = toDateFormatted, isNumber = false },
                        new StoreProcedureParm { ParmName = "HrmsCode", ParmValue = requestData.hrmsCode.ToString(), isNumber = true }
                    };

                    var roleIds = "71abc5f6-bacc-47b4-bd98-bc4cc08c3088";
                    //await _iGeneric_SP_Repository.CallStoreProcedureReaderAsync<HRMS_RoleIdByHRMSServiceCodeViewModel>("sp_HRMS_getRolesByHRMSCode", storeProcedureParm);

                    var allowedServiceCodes = _context.RoleWiseAllowedHRMSServiceCodes.Where(x => x.RoleId == roleIds).ToList();

                    var data = new List<HRMSDataViewModel>();
                    foreach (var item in allowedServiceCodes)
                    {
                        storeProcedureParm = new List<StoreProcedureParm>()
                        {
                        new StoreProcedureParm { ParmName = "FromDate", ParmValue = fromDateFormatted, isNumber = false },
                        new StoreProcedureParm { ParmName = "ToDate", ParmValue = toDateFormatted, isNumber = false },
                        //new StoreProcedureParm { ParmName = "serviceCode", ParmValue = requestData.FirstOrDefault().serviceCode.ToString(), isNumber = true },
                        new StoreProcedureParm { ParmName = "HrmsCode", ParmValue = requestData.hrmsCode.ToString(), isNumber = true },
                        new StoreProcedureParm { ParmName = "ApplicationType", ParmValue = item.ApplicationType.ToString(), isNumber = true },
                        new StoreProcedureParm { ParmName = "ApplicationPurposeType", ParmValue = item.ApplicationPurposeType.ToString(), isNumber = true },
                        };

                        data.AddRange(await _iGeneric_SP_Repository.CallStoreProcedureReaderAsync<HRMSDataViewModel>("sp_HRMS_GetHRMSCodeAndActWiseData", storeProcedureParm));
                    }
                    var distinctCircles = data.Where(x => x.CircleName != null).Select(x => x.CircleName).Distinct().ToList();
                    dynamic obj = new ExpandoObject();
                    obj.services = new List<dynamic>();
                    obj.welfareSchemesBOCW = new List<dynamic>();
                    obj.welfareSchemesPLWB = new List<dynamic>();
                    dynamic serviceObj = new ExpandoObject();
                    foreach (var item in allowedServiceCodes)
                    {
                        serviceObj = new ExpandoObject();
                        serviceObj.actCode = item.ActCode;
                        serviceObj.actName = item.ServiceName;
                        serviceObj.serviceCode = item.HRMSServiceCode;
                        serviceObj.serviceName = "";
                        serviceObj.data = new List<dynamic>();

                        if (item.HRMSServiceCode == 2001)
                        {

                            foreach (var circle in distinctCircles)
                            {
                                serviceObj.data.Add(new
                                {
                                    CircleName = circle,
                                    CountHeads = new
                                    {
                                        PendingInTheBeginningOfTheYear = data.Where(x => x.ApplicationType == item.ApplicationType && x.applicationPurposeType == item.ApplicationPurposeType && x.CircleName == circle).Select(x => x.TotalPendingInBeginning).FirstOrDefault(),
                                        Received = data.Where(x => x.ApplicationType == item.ApplicationType && x.applicationPurposeType == item.ApplicationPurposeType && x.CircleName == circle).Select(x => x.TotalReceived).FirstOrDefault(),
                                        ObjectionsRaised = data.Where(x => x.ApplicationType == item.ApplicationType && x.applicationPurposeType == item.ApplicationPurposeType && x.CircleName == circle).Select(x => x.TotalObjectionRaised).FirstOrDefault(),
                                        Approved = data.Where(x => x.ApplicationType == item.ApplicationType && x.applicationPurposeType == item.ApplicationPurposeType && x.CircleName == circle).Select(x => x.TotalApproved).FirstOrDefault(),
                                        PendingAtTheEndOfTheYear = data.Where(x => x.ApplicationType == item.ApplicationType && x.applicationPurposeType == item.ApplicationPurposeType && x.CircleName == circle).Select(x => x.TotalPending).FirstOrDefault(),
                                    }
                                });
                            }
                            serviceObj.serviceName = "New Registration";
                            obj.services.Add(serviceObj);
                        }

                        if (item.HRMSServiceCode == 2002)
                        {
                            serviceObj.serviceName = "Amendment RC";
                            foreach (var circle in distinctCircles)
                            {
                                serviceObj.data.Add(new
                                {
                                    CircleName = circle,
                                    CountHeads = new
                                    {
                                        PendingInTheBeginningOfTheYear = data.Where(x => x.ApplicationType == item.ApplicationType && x.applicationPurposeType == item.ApplicationPurposeType && x.CircleName == circle).Select(x => x.TotalPendingInBeginning).FirstOrDefault(),
                                        Received = data.Where(x => x.ApplicationType == item.ApplicationType && x.applicationPurposeType == item.ApplicationPurposeType && x.CircleName == circle).Select(x => x.TotalReceived).FirstOrDefault(),
                                        ObjectionsRaised = data.Where(x => x.ApplicationType == item.ApplicationType && x.applicationPurposeType == item.ApplicationPurposeType && x.CircleName == circle).Select(x => x.TotalObjectionRaised).FirstOrDefault(),
                                        Approved = data.Where(x => x.ApplicationType == item.ApplicationType && x.applicationPurposeType == item.ApplicationPurposeType && x.CircleName == circle).Select(x => x.TotalApproved).FirstOrDefault(),
                                        PendingAtTheEndOfTheYear = data.Where(x => x.ApplicationType == item.ApplicationType && x.applicationPurposeType == item.ApplicationPurposeType && x.CircleName == circle).Select(x => x.TotalPending).FirstOrDefault(),
                                    }
                                });
                            }
                            obj.services.Add(serviceObj);
                        }

                        if (item.HRMSServiceCode == 2003)
                        {
                            serviceObj.serviceName = "License";
                            foreach (var circle in distinctCircles)
                            {
                                serviceObj.data.Add(new
                                {
                                    CircleName = circle,
                                    CountHeads = new
                                    {
                                        PendingInTheBeginningOfTheYear = data.Where(x => x.ApplicationType == item.ApplicationType && x.applicationPurposeType == item.ApplicationPurposeType && x.CircleName == circle).Select(x => x.TotalPendingInBeginning).FirstOrDefault(),
                                        Received = data.Where(x => x.ApplicationType == item.ApplicationType && x.applicationPurposeType == item.ApplicationPurposeType && x.CircleName == circle).Select(x => x.TotalReceived).FirstOrDefault(),
                                        ObjectionsRaised = data.Where(x => x.ApplicationType == item.ApplicationType && x.applicationPurposeType == item.ApplicationPurposeType && x.CircleName == circle).Select(x => x.TotalObjectionRaised).FirstOrDefault(),
                                        Approved = data.Where(x => x.ApplicationType == item.ApplicationType && x.applicationPurposeType == item.ApplicationPurposeType && x.CircleName == circle).Select(x => x.TotalApproved).FirstOrDefault(),
                                        PendingAtTheEndOfTheYear = data.Where(x => x.ApplicationType == item.ApplicationType && x.applicationPurposeType == item.ApplicationPurposeType && x.CircleName == circle).Select(x => x.TotalPending).FirstOrDefault(),
                                    }
                                });
                            }
                            obj.services.Add(serviceObj);
                        }

                        if (item.HRMSServiceCode == 2004)
                        {
                            serviceObj.serviceName = "Renewal";
                            foreach (var circle in distinctCircles)
                            {
                                serviceObj.data.Add(new
                                {
                                    CircleName = circle,
                                    CountHeads = new
                                    {
                                        PendingInTheBeginningOfTheYear = data.Where(x => x.ApplicationType == item.ApplicationType && x.applicationPurposeType == item.ApplicationPurposeType && x.CircleName == circle).Select(x => x.TotalPendingInBeginning).FirstOrDefault(),
                                        Received = data.Where(x => x.ApplicationType == item.ApplicationType && x.applicationPurposeType == item.ApplicationPurposeType && x.CircleName == circle).Select(x => x.TotalReceived).FirstOrDefault(),
                                        ObjectionsRaised = data.Where(x => x.ApplicationType == item.ApplicationType && x.applicationPurposeType == item.ApplicationPurposeType && x.CircleName == circle).Select(x => x.TotalObjectionRaised).FirstOrDefault(),
                                        Approved = data.Where(x => x.ApplicationType == item.ApplicationType && x.applicationPurposeType == item.ApplicationPurposeType && x.CircleName == circle).Select(x => x.TotalApproved).FirstOrDefault(),
                                        PendingAtTheEndOfTheYear = data.Where(x => x.ApplicationType == item.ApplicationType && x.applicationPurposeType == item.ApplicationPurposeType && x.CircleName == circle).Select(x => x.TotalPending).FirstOrDefault(),
                                    }
                                });
                            }
                            obj.services.Add(serviceObj);
                        }

                        if (item.HRMSServiceCode == 2005)
                        {
                            serviceObj.serviceName = "Amendment";
                            foreach (var circle in distinctCircles)
                            {
                                serviceObj.data.Add(new
                                {
                                    CircleName = circle,
                                    CountHeads = new
                                    {
                                        PendingInTheBeginningOfTheYear = data.Where(x => x.ApplicationType == item.ApplicationType && x.applicationPurposeType == item.ApplicationPurposeType && x.CircleName == circle).Select(x => x.TotalPendingInBeginning).FirstOrDefault(),
                                        Received = data.Where(x => x.ApplicationType == item.ApplicationType && x.applicationPurposeType == item.ApplicationPurposeType && x.CircleName == circle).Select(x => x.TotalReceived).FirstOrDefault(),
                                        ObjectionsRaised = data.Where(x => x.ApplicationType == item.ApplicationType && x.applicationPurposeType == item.ApplicationPurposeType && x.CircleName == circle).Select(x => x.TotalObjectionRaised).FirstOrDefault(),
                                        Approved = data.Where(x => x.ApplicationType == item.ApplicationType && x.applicationPurposeType == item.ApplicationPurposeType && x.CircleName == circle).Select(x => x.TotalApproved).FirstOrDefault(),
                                        PendingAtTheEndOfTheYear = data.Where(x => x.ApplicationType == item.ApplicationType && x.applicationPurposeType == item.ApplicationPurposeType && x.CircleName == circle).Select(x => x.TotalPending).FirstOrDefault(),
                                    }
                                });
                            }
                            obj.services.Add(serviceObj);
                        }

                        if (item.HRMSServiceCode == 2006)
                        {
                            serviceObj.serviceName = "New Registration";
                            foreach (var circle in distinctCircles)
                            {
                                serviceObj.data.Add(new
                                {
                                    CircleName = circle,
                                    CountHeads = new
                                    {
                                        PendingInTheBeginningOfTheYear = data.Where(x => x.ApplicationType == item.ApplicationType && x.applicationPurposeType == item.ApplicationPurposeType && x.CircleName == circle).Select(x => x.TotalPendingInBeginning).FirstOrDefault(),
                                        Received = data.Where(x => x.ApplicationType == item.ApplicationType && x.applicationPurposeType == item.ApplicationPurposeType && x.CircleName == circle).Select(x => x.TotalReceived).FirstOrDefault(),
                                        ObjectionsRaised = data.Where(x => x.ApplicationType == item.ApplicationType && x.applicationPurposeType == item.ApplicationPurposeType && x.CircleName == circle).Select(x => x.TotalObjectionRaised).FirstOrDefault(),
                                        Approved = data.Where(x => x.ApplicationType == item.ApplicationType && x.applicationPurposeType == item.ApplicationPurposeType && x.CircleName == circle).Select(x => x.TotalApproved).FirstOrDefault(),
                                        PendingAtTheEndOfTheYear = data.Where(x => x.ApplicationType == item.ApplicationType && x.applicationPurposeType == item.ApplicationPurposeType && x.CircleName == circle).Select(x => x.TotalPending).FirstOrDefault(),
                                    }
                                });
                            }
                            obj.services.Add(serviceObj);
                        }

                        if (item.HRMSServiceCode == 2007)
                        {
                            serviceObj.serviceName = "Amendment RC";
                            foreach (var circle in distinctCircles)
                            {
                                serviceObj.data.Add(new
                                {
                                    CircleName = circle,
                                    CountHeads = new
                                    {
                                        PendingInTheBeginningOfTheYear = data.Where(x => x.ApplicationType == item.ApplicationType && x.applicationPurposeType == item.ApplicationPurposeType && x.CircleName == circle).Select(x => x.TotalPendingInBeginning).FirstOrDefault(),
                                        Received = data.Where(x => x.ApplicationType == item.ApplicationType && x.applicationPurposeType == item.ApplicationPurposeType && x.CircleName == circle).Select(x => x.TotalReceived).FirstOrDefault(),
                                        ObjectionsRaised = data.Where(x => x.ApplicationType == item.ApplicationType && x.applicationPurposeType == item.ApplicationPurposeType && x.CircleName == circle).Select(x => x.TotalObjectionRaised).FirstOrDefault(),
                                        Approved = data.Where(x => x.ApplicationType == item.ApplicationType && x.applicationPurposeType == item.ApplicationPurposeType && x.CircleName == circle).Select(x => x.TotalApproved).FirstOrDefault(),
                                        PendingAtTheEndOfTheYear = data.Where(x => x.ApplicationType == item.ApplicationType && x.applicationPurposeType == item.ApplicationPurposeType && x.CircleName == circle).Select(x => x.TotalPending).FirstOrDefault(),
                                    }
                                });
                            }
                            obj.services.Add(serviceObj);
                        }

                        if (item.HRMSServiceCode == 2008)
                        {
                            serviceObj.serviceName = "License";
                            foreach (var circle in distinctCircles)
                            {
                                serviceObj.data.Add(new
                                {
                                    CircleName = circle,
                                    CountHeads = new
                                    {
                                        PendingInTheBeginningOfTheYear = data.Where(x => x.ApplicationType == item.ApplicationType && x.applicationPurposeType == item.ApplicationPurposeType && x.CircleName == circle).Select(x => x.TotalPendingInBeginning).FirstOrDefault(),
                                        Received = data.Where(x => x.ApplicationType == item.ApplicationType && x.applicationPurposeType == item.ApplicationPurposeType && x.CircleName == circle).Select(x => x.TotalReceived).FirstOrDefault(),
                                        ObjectionsRaised = data.Where(x => x.ApplicationType == item.ApplicationType && x.applicationPurposeType == item.ApplicationPurposeType && x.CircleName == circle).Select(x => x.TotalObjectionRaised).FirstOrDefault(),
                                        Approved = data.Where(x => x.ApplicationType == item.ApplicationType && x.applicationPurposeType == item.ApplicationPurposeType && x.CircleName == circle).Select(x => x.TotalApproved).FirstOrDefault(),
                                        PendingAtTheEndOfTheYear = data.Where(x => x.ApplicationType == item.ApplicationType && x.applicationPurposeType == item.ApplicationPurposeType && x.CircleName == circle).Select(x => x.TotalPending).FirstOrDefault(),
                                    }
                                });
                            }
                            obj.services.Add(serviceObj);
                        }

                        if (item.HRMSServiceCode == 2009)
                        {
                            serviceObj.serviceName = "Renewal";
                            foreach (var circle in distinctCircles)
                            {
                                serviceObj.data.Add(new
                                {
                                    CircleName = circle,
                                    CountHeads = new
                                    {
                                        PendingInTheBeginningOfTheYear = data.Where(x => x.ApplicationType == item.ApplicationType && x.applicationPurposeType == item.ApplicationPurposeType && x.CircleName == circle).Select(x => x.TotalPendingInBeginning).FirstOrDefault(),
                                        Received = data.Where(x => x.ApplicationType == item.ApplicationType && x.applicationPurposeType == item.ApplicationPurposeType && x.CircleName == circle).Select(x => x.TotalReceived).FirstOrDefault(),
                                        ObjectionsRaised = data.Where(x => x.ApplicationType == item.ApplicationType && x.applicationPurposeType == item.ApplicationPurposeType && x.CircleName == circle).Select(x => x.TotalObjectionRaised).FirstOrDefault(),
                                        Approved = data.Where(x => x.ApplicationType == item.ApplicationType && x.applicationPurposeType == item.ApplicationPurposeType && x.CircleName == circle).Select(x => x.TotalApproved).FirstOrDefault(),
                                        PendingAtTheEndOfTheYear = data.Where(x => x.ApplicationType == item.ApplicationType && x.applicationPurposeType == item.ApplicationPurposeType && x.CircleName == circle).Select(x => x.TotalPending).FirstOrDefault(),
                                    }
                                });
                            }
                            obj.services.Add(serviceObj);
                        }

                        if (item.HRMSServiceCode == 2010)
                        {
                            serviceObj.serviceName = "Amendment";
                            foreach (var circle in distinctCircles)
                            {
                                serviceObj.data.Add(new
                                {
                                    CircleName = circle,
                                    CountHeads = new
                                    {
                                        PendingInTheBeginningOfTheYear = data.Where(x => x.ApplicationType == item.ApplicationType && x.applicationPurposeType == item.ApplicationPurposeType && x.CircleName == circle).Select(x => x.TotalPendingInBeginning).FirstOrDefault(),
                                        Received = data.Where(x => x.ApplicationType == item.ApplicationType && x.applicationPurposeType == item.ApplicationPurposeType && x.CircleName == circle).Select(x => x.TotalReceived).FirstOrDefault(),
                                        ObjectionsRaised = data.Where(x => x.ApplicationType == item.ApplicationType && x.applicationPurposeType == item.ApplicationPurposeType && x.CircleName == circle).Select(x => x.TotalObjectionRaised).FirstOrDefault(),
                                        Approved = data.Where(x => x.ApplicationType == item.ApplicationType && x.applicationPurposeType == item.ApplicationPurposeType && x.CircleName == circle).Select(x => x.TotalApproved).FirstOrDefault(),
                                        PendingAtTheEndOfTheYear = data.Where(x => x.ApplicationType == item.ApplicationType && x.applicationPurposeType == item.ApplicationPurposeType && x.CircleName == circle).Select(x => x.TotalPending).FirstOrDefault(),
                                    }
                                });
                            }
                            obj.services.Add(serviceObj);
                        }

                        if (item.HRMSServiceCode == 2012)
                        {
                            serviceObj.serviceName = "New Registration";
                            foreach (var circle in distinctCircles)
                            {
                                serviceObj.data.Add(new
                                {
                                    CircleName = circle,
                                    CountHeads = new
                                    {
                                        PendingInTheBeginningOfTheYear = data.Where(x => x.ApplicationType == item.ApplicationType && x.applicationPurposeType == item.ApplicationPurposeType && x.CircleName == circle).Select(x => x.TotalPendingInBeginning).FirstOrDefault(),
                                        Received = data.Where(x => x.ApplicationType == item.ApplicationType && x.applicationPurposeType == item.ApplicationPurposeType && x.CircleName == circle).Select(x => x.TotalReceived).FirstOrDefault(),
                                        ObjectionsRaised = data.Where(x => x.ApplicationType == item.ApplicationType && x.applicationPurposeType == item.ApplicationPurposeType && x.CircleName == circle).Select(x => x.TotalObjectionRaised).FirstOrDefault(),
                                        Approved = data.Where(x => x.ApplicationType == item.ApplicationType && x.applicationPurposeType == item.ApplicationPurposeType && x.CircleName == circle).Select(x => x.TotalApproved).FirstOrDefault(),
                                        PendingAtTheEndOfTheYear = data.Where(x => x.ApplicationType == item.ApplicationType && x.applicationPurposeType == item.ApplicationPurposeType && x.CircleName == circle).Select(x => x.TotalPending).FirstOrDefault(),
                                    }
                                });
                            }
                            obj.services.Add(serviceObj);
                        }

                        if (item.HRMSServiceCode == 2013)
                        {
                            serviceObj.serviceName = "Amendment RC";
                            foreach (var circle in distinctCircles)
                            {
                                serviceObj.data.Add(new
                                {
                                    CircleName = circle,
                                    CountHeads = new
                                    {
                                        PendingInTheBeginningOfTheYear = data.Where(x => x.ApplicationType == item.ApplicationType && x.applicationPurposeType == item.ApplicationPurposeType && x.CircleName == circle).Select(x => x.TotalPendingInBeginning).FirstOrDefault(),
                                        Received = data.Where(x => x.ApplicationType == item.ApplicationType && x.applicationPurposeType == item.ApplicationPurposeType && x.CircleName == circle).Select(x => x.TotalReceived).FirstOrDefault(),
                                        ObjectionsRaised = data.Where(x => x.ApplicationType == item.ApplicationType && x.applicationPurposeType == item.ApplicationPurposeType && x.CircleName == circle).Select(x => x.TotalObjectionRaised).FirstOrDefault(),
                                        Approved = data.Where(x => x.ApplicationType == item.ApplicationType && x.applicationPurposeType == item.ApplicationPurposeType && x.CircleName == circle).Select(x => x.TotalApproved).FirstOrDefault(),
                                        PendingAtTheEndOfTheYear = data.Where(x => x.ApplicationType == item.ApplicationType && x.applicationPurposeType == item.ApplicationPurposeType && x.CircleName == circle).Select(x => x.TotalPending).FirstOrDefault(),
                                    }
                                });
                            }
                            obj.services.Add(serviceObj);
                        }


                        if (item.HRMSServiceCode == 2014)
                        {
                            serviceObj.serviceName = "Welfare schemes of PLWB";
                            foreach (var circle in distinctCircles)
                            {
                                serviceObj.data.Add(new
                                {
                                    CircleName = circle,
                                    CountHeads = new
                                    {
                                        pendingInTheBeginningOfTheYear = data.Where(x => x.ApplicationType == item.ApplicationType && x.applicationPurposeType == item.ApplicationPurposeType && x.CircleName == circle).Select(x => x.TotalPendingInBeginning).FirstOrDefault(),
                                        applicationsReceived = data.Where(x => x.ApplicationType == item.ApplicationType && x.applicationPurposeType == item.ApplicationPurposeType && x.CircleName == circle).Select(x => x.TotalReceived).FirstOrDefault(),
                                        objectionsRaised = data.Where(x => x.ApplicationType == item.ApplicationType && x.applicationPurposeType == item.ApplicationPurposeType && x.CircleName == circle).Select(x => x.TotalPendingInBeginning).FirstOrDefault() + data.Where(x => x.ApplicationType == item.ApplicationType && x.applicationPurposeType == item.ApplicationPurposeType && x.CircleName == circle).Select(x => x.TotalObjectionRaised).FirstOrDefault(),
                                        applicationsApproved = data.Where(x => x.ApplicationType == item.ApplicationType && x.applicationPurposeType == item.ApplicationPurposeType && x.CircleName == circle).Select(x => x.TotalApproved).FirstOrDefault(),
                                        pendingAtTheEndOfTheYear = data.Where(x => x.ApplicationType == item.ApplicationType && x.applicationPurposeType == item.ApplicationPurposeType && x.CircleName == circle).Select(x => x.TotalPending).FirstOrDefault()

                                    }
                                });
                            }
                            obj.welfareSchemesPLWB.Add(serviceObj);
                        }
                        if (item.HRMSServiceCode == 2015)
                        {
                          
                            List<ApplicationData> respData = new List<ApplicationData>();
                            // The curl command you want to execute
                            //string curlCommand = $"curl -X GET \"https://bocw.punjab.gov.in/bocwb/GetPendancyRecords/222284/2024-04-01T11:21:12.015Z/2025-03-06T11:21:12.015Z\" -H \"Content-Type: application/json\"";

                            // string curlCommand = $"curl -X GET \"https://bocw.punjab.gov.in/bocwb/GetPendancyRecords/222284/2024-04-01T11:21:12.015Z/2025-03-06T11:21:12.015Z\" -H \"Content-Type: application/json\"";


                            //string curlCommand = $"curl -X GET \"https://bocw.punjab.gov.in/bocwb/GetPendancyRecords/{requestData.hrmsCode}/{fromDateFormatted}/{toDateFormatted}\" -H \"Content-Type: application/json\" --ssl-no-revoke -x 127.0.0.1:8081";

                            // var startInfo = new ProcessStartInfo
                            // {
                            //     //FileName = "bash",
                            //     //Arguments = $"-c \"{curlCommand}\"",
                            //     //RedirectStandardOutput = true,
                            //     //UseShellExecute = false,
                            //     //CreateNoWindow = true

                            //     FileName = "curl",
                            //     Arguments = curlCommand,
                            //     RedirectStandardOutput = true,
                            //     RedirectStandardError = true,
                            //     UseShellExecute = false,
                            //     CreateNoWindow = true
                            // };
                            // try
                            // {
                            //     using (var process = Process.Start(startInfo))
                            //     {
                            //         using (var reader = process.StandardOutput)
                            //         {
                            //             string response = reader.ReadToEnd();
                            //             process.WaitForExit();

                            //             var responseObject = JsonConvert.DeserializeObject<ApiResponse>(response);
                            //             if (responseObject.Result.Data != null)
                            //             {
                            //                 respData = JsonConvert.DeserializeObject<List<ApplicationData>>(responseObject.Result.Data);
                            //             }

                            //             //foreach (var item in responseObject.Result.Data)
                            //             {
                            //                 //Console.WriteLine($"Circle: {item.Circle}, PendingAtTheEndOfTheYear: {item.PendingAtTheEndOfTheYear}");
                            //             }
                            //         }
                            //     }
                            // }
                            // catch (Exception ex)
                            // {
                            //     Console.WriteLine($"Error: {ex.Message}");
                            // }


                            using (var httpClient = new HttpClient())
                            {

                                using (var response = await httpClient.GetAsync("https://bocw.punjab.gov.in/bocwb/GetPendancyRecords/" + requestData.hrmsCode + "/" + fromDateFormatted + "/" + toDateFormatted))
                                {
                                    string apiResponse = await response.Content.ReadAsStringAsync();
                                    var responseObject = JsonConvert.DeserializeObject<ApiResponse>(apiResponse);
                                    if (responseObject.Result.Data != null)
                                    {
                                        respData = JsonConvert.DeserializeObject<List<ApplicationData>>(responseObject.Result.Data);
                                    }
                                }
                            }

                            serviceObj.serviceName = "Welfare Schemes of BOCW";
                            foreach (var circle in distinctCircles)
                            {
                                serviceObj.data.Add(new
                                {
                                    CircleName = circle,
                                    CountHeads = new
                                    {
                                        pendingInTheBeginningOfTheYear = (respData == null || respData.Count() == 0) ? 0 : respData.Where(x => x.ApplicationType == item.ApplicationType && x.ApplicationPurposeType == item.ApplicationPurposeType && x.Circle.Trim().ToLower() == circle.Trim().ToLower()).Select(x => x.PendingInTheBeginningOfTheYear).FirstOrDefault(),
                                        applicationsReceived = (respData == null || respData.Count() == 0) ? 0 : respData.Where(x => x.ApplicationType == item.ApplicationType && x.ApplicationPurposeType == item.ApplicationPurposeType && x.Circle.Trim().ToLower()== circle.Trim().ToLower()).Select(x => x.ApplicationsReceived).FirstOrDefault(),
                                        objectionsRaised = (respData == null || respData.Count() == 0) ? 0 : respData.Where(x => x.ApplicationType == item.ApplicationType && x.ApplicationPurposeType == item.ApplicationPurposeType && x.Circle.Trim().ToLower()== circle.Trim().ToLower()).Select(x => x.ObjectionsRaised).FirstOrDefault(),
                                        totalNoOfApprovedApplications = (respData == null || respData.Count() == 0) ? 0 : respData.Where(x => x.ApplicationType == item.ApplicationType && x.ApplicationPurposeType == item.ApplicationPurposeType && x.Circle.Trim().ToLower()== circle.Trim().ToLower()).Select(x => x.TotalNoofApprovedApplications).FirstOrDefault(),
                                        pendingAtTheEndOfTheYear = (respData == null || respData.Count() == 0) ? 0 : respData.Where(x => x.ApplicationType == item.ApplicationType && x.ApplicationPurposeType == item.ApplicationPurposeType && x.Circle.Trim().ToLower()== circle.Trim().ToLower()).Select(x => x.PendingAtTheEndOfTheYear).FirstOrDefault()
                                    }
                                });
                            }
                            obj.welfareSchemesBOCW.Add(serviceObj);
                        }
                    }

                    List<StoreProcedureParm> storeProcedureParm2 = new List<StoreProcedureParm>()
                    {
                        new StoreProcedureParm { ParmName = "hrmsCode", ParmValue = requestData.hrmsCode.ToString(), isNumber = true }
                    };
                    var officerDetails = await _iGeneric_SP_Repository.CallStoreProcedureReaderAsync<HRMS_OfficerDetailsViewModel>("sp_HRMS_GetProfileDetailsByHRMSCode", storeProcedureParm2);

                    var distinctAppPurposeTypes = data.Select(x => x.applicationPurposeType).Distinct().ToList();

                    obj.OfficerName = officerDetails.FirstOrDefault().OfficerName;
                    obj.HrmsCode = requestData.hrmsCode;
                    obj.Type = "ALCWing";

                    genericServiceResultTemplate.ResponseDataModel = obj;
                }
            }
            catch (Exception ex)
            {
                genericServiceResultTemplate.HasError = true;
                genericServiceResultTemplate.ErrorDesc = ex.Message;
                throw ex;
            }
            return genericServiceResultTemplate;
        }

        public async Task<GenericResponseTemplateModel<dynamic>> GetHRMSCodeAndActWiseData_LabourWing(HRMSRequestParamsViewModel requestData)
        {
            GenericResponseTemplateModel<dynamic> genericServiceResultTemplate = new GenericResponseTemplateModel<dynamic>();
            try
            {
                //HRMSApplicationPurposeTypeViewModel hRMSApplicationPurposeTypeData = new HRMSApplicationPurposeTypeViewModel();
                if (requestData != null)
                {
                    string fromDateFormatted = requestData.fromDate.ToString("yyyy-MM-ddTHH:mm:ssZ");
                    string toDateFormatted = requestData.toDate.ToString("yyyy-MM-ddTHH:mm:ssZ");

                     List<StoreProcedureParm> storeProcedureParm = new List<StoreProcedureParm>()
                    {
                        new StoreProcedureParm { ParmName = "FromDate", ParmValue = fromDateFormatted, isNumber = false },
                        new StoreProcedureParm { ParmName = "ToDate", ParmValue = toDateFormatted, isNumber = false },
                        new StoreProcedureParm { ParmName = "HrmsCode", ParmValue = requestData.hrmsCode.ToString(), isNumber = true }
                    };

                    // var roleIds = await _iGeneric_SP_Repository.CallStoreProcedureReaderAsync<HRMS_RoleIdByHRMSServiceCodeViewModel>("sp_HRMS_getRolesByHRMSCode", storeProcedureParm);

                    var roleId = "60c208bd-100d-442a-bb3b-a677e9a95877";

                    var allowedServiceCodes = _context.RoleWiseAllowedHRMSServiceCodes.Where(x => x.RoleId == roleId);

                    var data = new List<HRMSDataViewModel>();
                    foreach (var item in allowedServiceCodes)
                    {
                        storeProcedureParm = new List<StoreProcedureParm>()
                        {
                        new StoreProcedureParm { ParmName = "FromDate", ParmValue = fromDateFormatted, isNumber = false },
                        new StoreProcedureParm { ParmName = "ToDate", ParmValue = toDateFormatted, isNumber = false },
                        //new StoreProcedureParm { ParmName = "serviceCode", ParmValue = requestData.FirstOrDefault().serviceCode.ToString(), isNumber = true },
                        new StoreProcedureParm { ParmName = "HrmsCode", ParmValue = requestData.hrmsCode.ToString(), isNumber = true },
                        new StoreProcedureParm { ParmName = "ApplicationType", ParmValue = item.ApplicationType.ToString(), isNumber = true },
                        new StoreProcedureParm { ParmName = "ApplicationPurposeType", ParmValue = item.ApplicationPurposeType.ToString(), isNumber = true },
                        };

                        data.AddRange(await _iGeneric_SP_Repository.CallStoreProcedureReaderAsync<HRMSDataViewModel>("sp_HRMS_GetHRMSCodeAndActWiseData", storeProcedureParm));
                    }
                    var distinctCircles = data.Where(x => x.CircleName != null).Select(x => x.CircleName).Distinct().ToList();

                    DataViewModel lbinData = new DataViewModel();
                    
                    using (var httpClient = new HttpClient())
                    {

                        using (var response = await httpClient.GetAsync("https://bocw.punjab.gov.in/bocwb/GetPendancyRecords/" + requestData.hrmsCode + "/" + fromDateFormatted + "/" + toDateFormatted))
                        {
                            string apiResponse = await response.Content.ReadAsStringAsync();
                            var responseObject = JsonConvert.DeserializeObject<ApiResponse>(apiResponse);
                            if (responseObject.Result.Data != null)
                            {
                                lbinData = JsonConvert.DeserializeObject<DataViewModel>(responseObject.Result.Data);
                            }
                        }
                    }

                    dynamic obj = new ExpandoObject();
                    obj.services = new List<dynamic>();
                    obj.inspections = new List<dynamic>();
                    obj.registrationBOCW = new List<dynamic>();
                    obj.welfareSchemesBOCW = new List<dynamic>();
                    obj.welfareSchemesPLWB = new List<dynamic>();
                    dynamic serviceObj = new ExpandoObject();
                    foreach (var item in allowedServiceCodes)
                    {
                        serviceObj = new ExpandoObject();
                        serviceObj.actCode = item.ActCode;
                        serviceObj.actName = item.ServiceName;
                        serviceObj.serviceCode = item.HRMSServiceCode;
                        serviceObj.serviceName = "";
                        serviceObj.data = new List<dynamic>();

                        if (item.HRMSServiceCode == 3001)
                        {

                            foreach (var circle in distinctCircles)
                            {
                                var filteredData = data.FirstOrDefault(x =>
                                    x.ApplicationType == item.ApplicationType &&
                                    x.applicationPurposeType == item.ApplicationPurposeType &&
                                    x.CircleName == circle);

                                var totalReceived = filteredData?.TotalReceived ?? 0;
                                var objectionsResolved = filteredData?.ObjectionsResolved ?? 0;
                                serviceObj.data.Add(new
                                {
                                    CircleName = circle,
                                    CountHeads = new
                                    {
                                        PendingInTheBeginningOfTheYear = (data.Where(x => x.ApplicationType == item.ApplicationType && x.applicationPurposeType == item.ApplicationPurposeType && x.CircleName == circle).Select(x => x.TotalPendingInBeginning).FirstOrDefault()),
                                        ApplicationsReceivedNew = data.Where(x => x.ApplicationType == item.ApplicationType && x.applicationPurposeType == item.ApplicationPurposeType && x.CircleName == circle).Select(x => x.TotalReceived).FirstOrDefault(),
                                        ObjectionsRaised = data.Where(x => x.ApplicationType == item.ApplicationType && x.applicationPurposeType == item.ApplicationPurposeType && x.CircleName == circle).Select(x => x.TotalObjectionRaised).FirstOrDefault(),
                                        ObjectionsResolved = data.Where(x => x.ApplicationType == item.ApplicationType && x.applicationPurposeType == item.ApplicationPurposeType && x.CircleName == circle).Select(x => x.ObjectionsResolved).FirstOrDefault(),
                                        ApplicationsApproved = data.Where(x => x.ApplicationType == item.ApplicationType && x.applicationPurposeType == item.ApplicationPurposeType && x.CircleName == circle).Select(x => x.TotalApproved).FirstOrDefault(),
                                        ApplicationsAutoapproved = data.Where(x => x.ApplicationType == item.ApplicationType && x.applicationPurposeType == item.ApplicationPurposeType && x.CircleName == circle).Select(x => x.AutoApproved).FirstOrDefault(),
                                        PendingAtTheEndOfYear = data.Where(x => x.ApplicationType == item.ApplicationType && x.applicationPurposeType == item.ApplicationPurposeType && x.CircleName == circle).Select(x => x.TotalPending).FirstOrDefault(),
                                    }
                                });
                            }
                            serviceObj.serviceName = "New Registration";
                            obj.services.Add(serviceObj);
                        }

                        if (item.HRMSServiceCode == 3002)
                        {

                            foreach (var circle in distinctCircles)
                            {
                                serviceObj.data.Add(new
                                {
                                    CircleName = circle,
                                    CountHeads = new
                                    {

                                        pendinginthebeginningoftheyear = data.Where(x => x.ApplicationType == item.ApplicationType && x.applicationPurposeType == item.ApplicationPurposeType && x.CircleName == circle).Select(x => x.TotalPendingInBeginning).FirstOrDefault(),
                                        inspectionsAssignedDuringTheYear = data.Where(x => x.ApplicationType == item.ApplicationType && x.applicationPurposeType == item.ApplicationPurposeType && x.CircleName == circle).Select(x => x.TotalReceived).FirstOrDefault(),
                                        inspectionssubmitted = data.Where(x => x.ApplicationType == item.ApplicationType && x.applicationPurposeType == item.ApplicationPurposeType && x.CircleName == circle).Select(x => x.TotalApproved).FirstOrDefault(),
                                        numberofinspectionspendingattheendoftheyear = data.Where(x => x.ApplicationType == item.ApplicationType && x.applicationPurposeType == item.ApplicationPurposeType && x.CircleName == circle).Select(x => x.TotalPending).FirstOrDefault()
                                    }
                                });
                            }
                            serviceObj.serviceName = "Labour Inspections Under Factories ACT";
                            obj.inspections.Add(serviceObj);
                        }

                        if (item.HRMSServiceCode == 3003)
                        {
                            serviceObj.serviceName = "Welfare schemes of PLWB";

                            foreach (var circle in distinctCircles)
                            {
                                var filteredData = data.FirstOrDefault(x =>
                                    x.ApplicationType == item.ApplicationType &&
                                    x.applicationPurposeType == item.ApplicationPurposeType &&
                                    x.CircleName == circle);

                                var totalReceived = filteredData?.TotalReceived ?? 0;
                                var objectionsResolved = filteredData?.ObjectionsResolved ?? 0;

                                serviceObj.data.Add(new
                                {
                                    CircleName = circle,
                                    CountHeads = new
                                    {
                                        PendingInTheBeginningOfTheYear = filteredData?.TotalPendingInBeginning ?? 0,
                                        ApplicationsReceived = Math.Max(0, totalReceived - objectionsResolved),
                                        ObjectionsResolved = objectionsResolved,
                                        ObjectionsRaised = filteredData?.TotalObjectionRaised ?? 0,
                                        ApplicationsApproved = filteredData?.TotalApproved ?? 0,
                                        PendingAtTheEnd = filteredData?.TotalPending ?? 0
                                    }
                                });
                            }
                            obj.welfareSchemesPLWB.Add(serviceObj);
                        }



                        if (item.HRMSServiceCode == 3004)
                        {
                            serviceObj.serviceName = "Registration of Beneficiaries";
                            foreach (var circle in distinctCircles)
                            {
                                serviceObj.data.Add(new
                                {
                                    CircleName = circle,
                                    CountHeads = new
                                    {
                                        PendingInTheBeginningOfTheYear = (lbinData.Worker_Data==null || lbinData.Worker_Data.Count()==0) ? 0 : lbinData.Worker_Data.Where(x => x.ApplicationType == item.ApplicationType && x.ApplicationPurposeType == item.ApplicationPurposeType && x.Circle.Trim().ToLower()== circle.Trim().ToLower()).Select(x => x.PendingInTheBeginningOfTheYear).FirstOrDefault(),

                                        ApplicationsReceived = (lbinData.Worker_Data == null || lbinData.Worker_Data.Count() == 0) ? 0 : lbinData.Worker_Data.Where(x => x.ApplicationType == item.ApplicationType && x.ApplicationPurposeType == item.ApplicationPurposeType && x.Circle.Trim().ToLower()== circle.Trim().ToLower()).Select(x => x.ApplicationsReceived).FirstOrDefault(),
                                        ObjectionsResolved = (lbinData.Worker_Data == null || lbinData.Worker_Data.Count() == 0) ? 0 : lbinData.Worker_Data.Where(x => x.ApplicationType == item.ApplicationType && x.ApplicationPurposeType == item.ApplicationPurposeType && x.Circle.Trim().ToLower()== circle.Trim().ToLower()).Select(x => x.ObjectionsResolved).FirstOrDefault(),
                                        ObjectionsRaised = (lbinData.Worker_Data == null || lbinData.Worker_Data.Count() == 0) ? 0 : lbinData.Worker_Data.Where(x => x.ApplicationType == item.ApplicationType && x.ApplicationPurposeType == item.ApplicationPurposeType && x.Circle.Trim().ToLower()== circle.Trim().ToLower()).Select(x => x.ObjectionsRaised).FirstOrDefault(),
                                        totalNoOfRegisteredBenificiaries = (lbinData.Worker_Data == null || lbinData.Worker_Data.Count() == 0) ? 0 : lbinData.Worker_Data.Where(x => x.ApplicationType == item.ApplicationType && x.ApplicationPurposeType == item.ApplicationPurposeType && x.Circle.Trim().ToLower()== circle.Trim().ToLower()).Select(x => x.TotalNoofRegisteredBeneficiaries).FirstOrDefault(),
                                        PendingAtTheEnd = (lbinData.Worker_Data == null || lbinData.Worker_Data.Count() == 0) ? 0 : lbinData.Worker_Data.Where(x => x.ApplicationType == item.ApplicationType && x.ApplicationPurposeType == item.ApplicationPurposeType && x.Circle.Trim().ToLower()== circle.Trim().ToLower()).Select(x => x.PendingAtTheEndOfTheYear).FirstOrDefault()

                                    }
                                });
                            }
                            obj.registrationBOCW.Add(serviceObj);
                        }

                        if (item.HRMSServiceCode == 3005)
                        {
                            serviceObj.serviceName = "Welfare Schemes of BOCW Board";
                            foreach (var circle in distinctCircles)
                            {
                                serviceObj.data.Add(new
                                {
                                    CircleName = circle,
                                    CountHeads = new
                                    {
                                        PendingInTheBeginningOfTheYear = (lbinData.Scheme_Data == null || lbinData.Scheme_Data.Count() == 0) ? 0 : lbinData.Scheme_Data.Where(x => x.ApplicationType == item.ApplicationType && x.ApplicationPurposeType == item.ApplicationPurposeType && x.Circle.Trim().ToLower()== circle.Trim().ToLower()).Select(x => x.PendingInTheBeginningOfTheYear).FirstOrDefault(),
                                        ApplicationsReceived = (lbinData.Scheme_Data == null || lbinData.Scheme_Data.Count() == 0) ? 0 : lbinData.Scheme_Data.Where(x => x.ApplicationType == item.ApplicationType && x.ApplicationPurposeType == item.ApplicationPurposeType && x.Circle.Trim().ToLower()== circle.Trim().ToLower()).Select(x => x.ApplicationsReceived).FirstOrDefault(),
                                        ObjectionsResolved = (lbinData.Scheme_Data == null || lbinData.Scheme_Data.Count() == 0) ? 0 : lbinData.Scheme_Data.Where(x => x.ApplicationType == item.ApplicationType && x.ApplicationPurposeType == item.ApplicationPurposeType && x.Circle.Trim().ToLower()== circle.Trim().ToLower()).Select(x => x.ObjectionsResolved).FirstOrDefault(),
                                        ObjectionsRaised = (lbinData.Scheme_Data == null || lbinData.Scheme_Data.Count() == 0) ? 0 : lbinData.Scheme_Data.Where(x => x.ApplicationType == item.ApplicationType && x.ApplicationPurposeType == item.ApplicationPurposeType && x.Circle.Trim().ToLower()== circle.Trim().ToLower()).Select(x => x.ObjectionsRaised).FirstOrDefault() ,
                                        TotalNoOfApprovedSchemes = (lbinData.Scheme_Data == null || lbinData.Scheme_Data.Count() == 0) ? 0 : lbinData.Scheme_Data.Where(x => x.ApplicationType == item.ApplicationType && x.ApplicationPurposeType == item.ApplicationPurposeType && x.Circle.Trim().ToLower()== circle.Trim().ToLower()).Select(x => x.TotalNoofApprovedSchemes).FirstOrDefault(),
                                        PendingAtTheEndOfTheYear = (lbinData.Scheme_Data == null || lbinData.Scheme_Data.Count() == 0) ? 0 : lbinData.Scheme_Data.Where(x => x.ApplicationType == item.ApplicationType && x.ApplicationPurposeType == item.ApplicationPurposeType && x.Circle.Trim().ToLower()== circle.Trim().ToLower()).Select(x => x.PendingAtTheEndOfTheYear).FirstOrDefault()

                                    }
                                });
                            }
                            obj.welfareSchemesBOCW.Add(serviceObj);
                        }
                    }

                    List<StoreProcedureParm> storeProcedureParm2 = new List<StoreProcedureParm>()
                    {
                        new StoreProcedureParm { ParmName = "hrmsCode", ParmValue = requestData.hrmsCode.ToString(), isNumber = true }
                    };
                    var officerDetails = await _iGeneric_SP_Repository.CallStoreProcedureReaderAsync<HRMS_OfficerDetailsViewModel>("sp_HRMS_GetProfileDetailsByHRMSCode", storeProcedureParm2);

                    var distinctAppPurposeTypes = data.Select(x => x.applicationPurposeType).Distinct().ToList();

                    obj.OfficerName = officerDetails.FirstOrDefault().OfficerName;
                    obj.HrmsCode = requestData.hrmsCode;
                    obj.Type = "LabourWing";

                    genericServiceResultTemplate.ResponseDataModel = obj;
                }
            }
            catch (Exception ex)
            {
                genericServiceResultTemplate.HasError = true;
                genericServiceResultTemplate.ErrorDesc = ex.Message;
                throw ex;
            }
            return genericServiceResultTemplate;
        }

        public async Task<GenericResponseTemplateModel<WithdrawApplicationViewModel>> WithdrawApplication(string requestData)
        {
            GenericResponseTemplateModel<WithdrawApplicationViewModel> genericServiceResultTemplate = new GenericResponseTemplateModel<WithdrawApplicationViewModel>();
            try
            {
                string encData = RijndaelManagedCryptoHandler.DecryptFromPlainTextToBase64(requestData, "94c8e24f-6c5e-4d", "70k6e44y-9z5q-9q");

                WithdrawApplicationReqParamsViewModel data = JsonConvert.DeserializeObject<WithdrawApplicationReqParamsViewModel>(encData);
                if (requestData != null)
                {
                    BusinessFirst_Withdraw_RequestLog withdrawRequestLog = new BusinessFirst_Withdraw_RequestLog
                    {
                        IPin = data.IPin,
                        AppId = data.AppId,
                        ServiceCode = data.ServiceCode,
                        CategoryType = data.CategoryType,
                        RequestCount = 1,
                        CreatedDate = DateTime.Now,
                        LastModifiedDate = DateTime.Now
                    };
                    _context.BusinessFirst_Withdraw_RequestLogs.Add(withdrawRequestLog);
                    await _context.SaveChangesAsync();
                }

                Application application = _context.Applications.Include(x => x.ProjectSites).Where(x => x.InvestPunjab_Ipin == data.IPin.ToString() && x.IsDeleted == false && x.InvestPunjab_AppId == data.AppId).FirstOrDefault();
                genericServiceResultTemplate.ResponseDataModel = new WithdrawApplicationViewModel();
                if (application != null)
                {
                    genericServiceResultTemplate.ResponseDataModel.AppRefId = application.AppId;
                    genericServiceResultTemplate.ResponseDataModel.ProjectSiteRefId = application.ProjectSiteRefId;
                    genericServiceResultTemplate.ResponseDataModel.ProjectSiteVersion = application.ProjectSites.ProjectSiteVersion;
                    genericServiceResultTemplate.ResponseDataModel.ApplicationType = application.ApplicationType;
                    genericServiceResultTemplate.ResponseDataModel.IPin = data.IPin;
                    genericServiceResultTemplate.ResponseDataModel.AppId = data.AppId;
                    genericServiceResultTemplate.ResponseDataModel.ApplicationPurposeType = application.ApplicationPurposeType;
                }
                else
                {
                    genericServiceResultTemplate.ResponseDataModel.AppRefId = 0;
                    genericServiceResultTemplate.ResponseDataModel.ProjectSiteRefId = 0;
                    genericServiceResultTemplate.ResponseDataModel.ProjectSiteVersion = 0;
                    genericServiceResultTemplate.ResponseDataModel.ApplicationType = 0;
                    genericServiceResultTemplate.ResponseDataModel.IPin = data.IPin;
                    genericServiceResultTemplate.ResponseDataModel.AppId = data.AppId;
                    genericServiceResultTemplate.ResponseDataModel.ApplicationPurposeType = 0;
                }

            }
            catch (Exception ex)
            {
                genericServiceResultTemplate.HasError = true;
                genericServiceResultTemplate.ErrorDesc = ex.Message;
                throw ex;
            }
            return genericServiceResultTemplate;

        }

        public async Task<GenericResponseTemplateModel<List<PendingApplicationDetailsViewModel>>> GetTotalPendingApplicationsByDate()
        {
           GenericResponseTemplateModel<List<PendingApplicationDetailsViewModel>> genericServiceResultTemplate = new GenericResponseTemplateModel<List<PendingApplicationDetailsViewModel>>();
            try
            {
               genericServiceResultTemplate.ResponseDataModel = await _iGeneric_SP_Repository.CallStoreProcedureReaderAsyncWithoutParams<PendingApplicationDetailsViewModel>("sp_GetPendencyCountServiceWise");
            }
            catch (Exception ex)
            {
                genericServiceResultTemplate.HasError = true;
                genericServiceResultTemplate.ErrorDesc = ex.Message;
            }
            return genericServiceResultTemplate;
        }

        public async Task<GenericResponseTemplateModel<List<PendingApplicationDetailsByServiceCodeViewModel>>> GetTotalPendingApplicationsByServiceCode(PendingApplicationsByServiceCodeParamsViewModel requestData)
        {
            GenericResponseTemplateModel<List<PendingApplicationDetailsByServiceCodeViewModel>> genericServiceResultTemplate = new GenericResponseTemplateModel<List<PendingApplicationDetailsByServiceCodeViewModel>>();
            try
            {
                List<StoreProcedureParm> storeProcedureParm = new List<StoreProcedureParm>()
                {
                    new StoreProcedureParm { ParmName = "ServiceCode", ParmValue = requestData.serviceCode.ToString(), isNumber = true },
                    new StoreProcedureParm { ParmName = "PendencyType",ParmValue = ((int)requestData.pendencyType).ToString(), isNumber = true }
                };
                genericServiceResultTemplate.ResponseDataModel = await _iGeneric_SP_Repository.CallStoreProcedureReaderAsync<PendingApplicationDetailsByServiceCodeViewModel>("sp_GetPendingApplicationsByServiceCode", storeProcedureParm);
            }
            catch (Exception ex)
            {
                genericServiceResultTemplate.HasError = true;
                genericServiceResultTemplate.ErrorDesc = ex.Message;
                throw ex;
            }
            return genericServiceResultTemplate;
        }

        public async Task<GenericResponseTemplateModel<IpinAndApplicationIdInfoViewModel>> GetApplicationCurrentStatusByIpinAndAppId(ApplicationCurrentStatusParamsViewModel requestData)
        {
            var genericServiceResultTemplate = new GenericResponseTemplateModel<IpinAndApplicationIdInfoViewModel>();
            try
            {
                List<StoreProcedureParm> storeProcedureParm = new List<StoreProcedureParm>
                {
                    new StoreProcedureParm { ParmName = "Ipin", ParmValue = requestData.ipin.ToString(), isNumber = true },
                    new StoreProcedureParm { ParmName = "ApplicationId", ParmValue = requestData.applicationId.ToString(), isNumber = false },
                    new StoreProcedureParm { ParmName = "ServiceCode", ParmValue = requestData.serviceCode.ToString(), isNumber = true }
                };

                var data = await _iGeneric_SP_Repository.CallStoreProcedureReaderAsync<ApplicationCurrentStatusViewModel>("sp_GetApplicationLogsByIpinAndAppId", storeProcedureParm);

                var filteredData = data.Where(x => x.IPin == requestData.ipin && x.ApplicationId == requestData.applicationId).ToList();

                var applicationLogs = filteredData.Select(x => new ApplicationStatusLogsViewModel
                {
                    StatusId = x.StatusId,
                    StatusDesc = x.StatusDesc,
                    Comments = x.Comments,
                    SenderName = x.SenderName,
                    SenderDesignation = x.SenderDesignation,
                    ReceiverName = x.ReceiverName,
                    ReceiverDesignation = x.ReceiverDesignation,
                    ClearanceIssuedOn = x.ClearanceIssuedOn,
                    ClearanceExpiredOn = x.ClearanceExpiredOn,
                    LicenseNo = x.LicenseNo,
                    ClearanceFile = x.ClearanceFile,
                    StatusDate = x.StatusDate,
                    IntegrationSource = x.IntegrationSource,
                    DeemedApproval = x.DeemedApproval,
                    DepartmentTakenTotalTime = x.DepartmentTakenTotalTime,
                    TimeType = x.TimeType,
                    AppActionLogId = x.AppActionLogId
                }).ToList();

                var response = new IpinAndApplicationIdInfoViewModel
                {
                    IPin = requestData.ipin.ToString(),
                    ApplicationId = requestData.applicationId,
                    ApplicationLogs = applicationLogs
                };

                genericServiceResultTemplate.ResponseDataModel = response;
            }
            catch (Exception ex)
            {
                genericServiceResultTemplate.HasError = true;
                genericServiceResultTemplate.ErrorDesc = ex.Message;
                throw ex;
            }

            return genericServiceResultTemplate;
        }

        public async Task<GenericResponseTemplateModel<List<PendingApplicationDetailsByServiceCodeViewModel>>> GetUpdatedApplicationsBetweenDates(UpdatedApplicationsParamsViewModel requestData)
        {
            GenericResponseTemplateModel<List<PendingApplicationDetailsByServiceCodeViewModel>> genericServiceResultTemplate = new GenericResponseTemplateModel<List<PendingApplicationDetailsByServiceCodeViewModel>>();
            try
            {
                List<StoreProcedureParm> storeProcedureParm = new List<StoreProcedureParm>()
                {
                    new StoreProcedureParm { ParmName = "FromDate", ParmValue = requestData.fromDate.ToString(), isNumber = false },
                    new StoreProcedureParm { ParmName = "ToDate", ParmValue = requestData.toDate.ToString(), isNumber = false }
                };
                genericServiceResultTemplate.ResponseDataModel = await _iGeneric_SP_Repository.CallStoreProcedureReaderAsync<PendingApplicationDetailsByServiceCodeViewModel>("sp_GetUpdatedApplicationsBetweenDates", storeProcedureParm);
            }
            catch (Exception ex)
            {
                genericServiceResultTemplate.HasError = true;
                genericServiceResultTemplate.ErrorDesc = ex.Message;
                throw ex;
            }
            return genericServiceResultTemplate;
        }
        public async Task<GenericResponseTemplateModel<List<GetReportByDepartmentViewModel>>> GetReportByDepartment()
        {
            var genericServiceResultTemplate = new GenericResponseTemplateModel<List<GetReportByDepartmentViewModel>>()
            {
                HasError = false,
                ErrorDesc = "",
            };

            try
            {

                genericServiceResultTemplate.ResponseDataModel = await _iGeneric_SP_Repository.CallStoreProcedureReaderAsyncWithoutParams<GetReportByDepartmentViewModel>("dbo.sp_GetReportByDepartment");
            }
            catch (Exception ex)
            {
                genericServiceResultTemplate.HasError = true;
                genericServiceResultTemplate.ErrorDesc = ex.Message;
                throw ex;
            }

            return genericServiceResultTemplate;
        }

        public async Task<GenericResponseTemplateModel<List<GetReportByAuthorityCountsViewModel>>> GetReportByAuthority(GetReportByAuthorityViewModel requestData)
        {
            GenericResponseTemplateModel<List<GetReportByAuthorityCountsViewModel>> genericServiceResultTemplate = new GenericResponseTemplateModel<List<GetReportByAuthorityCountsViewModel>>()
            {
                HasError = false,
                ErrorDesc = "",
            };
            try
            {
                List<StoreProcedureParm> storeProcedureParms = new List<StoreProcedureParm>();
                storeProcedureParms = new List<StoreProcedureParm>()
        {

              new StoreProcedureParm() { ParmName = "departmentID", ParmValue = requestData.departmentID.ToString(), isNumber = true },
              new StoreProcedureParm() { ParmName = "authorityID", ParmValue = requestData.authorityID.ToString(), isNumber = true }

        };
                genericServiceResultTemplate.ResponseDataModel = await _iGeneric_SP_Repository.CallStoreProcedureReaderAsync<GetReportByAuthorityCountsViewModel>("dbo.sp_GetAuthorityWisePendencyCounts", storeProcedureParms);
            }
            catch (Exception ex)
            {
                genericServiceResultTemplate.HasError = true;
                genericServiceResultTemplate.ErrorDesc = ex.Message;
            }
            return genericServiceResultTemplate;
        }

        public async Task<GenericResponseTemplateModel<List<GetReportByServiceCountsViewModel>>> GetReportByService(GetReportByServiceViewModel requestData)
        {
            GenericResponseTemplateModel<List<GetReportByServiceCountsViewModel>> genericServiceResultTemplate = new GenericResponseTemplateModel<List<GetReportByServiceCountsViewModel>>()
            {
                HasError = false,
                ErrorDesc = "",
            };
            try
            {
                List<StoreProcedureParm> storeProcedureParms = new List<StoreProcedureParm>();
                storeProcedureParms = new List<StoreProcedureParm>()
        {

              new StoreProcedureParm (){ ParmName="departmentID", ParmValue=requestData.departmentID.ToString(), isNumber=true},
              new StoreProcedureParm (){ ParmName="authorityID", ParmValue=requestData.authorityID.ToString(), isNumber=true},
              new StoreProcedureParm (){ ParmName="serviceID", ParmValue=requestData.serviceID.ToString(), isNumber=true}

        };
                genericServiceResultTemplate.ResponseDataModel = await _iGeneric_SP_Repository.CallStoreProcedureReaderAsync<GetReportByServiceCountsViewModel>("dbo.sp_GetPendingApplicationsByServiceId", storeProcedureParms);
            }
            catch (Exception ex)
            {          
                genericServiceResultTemplate.HasError = true;
                genericServiceResultTemplate.ErrorDesc = ex.Message;
            }
            return genericServiceResultTemplate;
        }


        public async Task<GenericResponseTemplateModel<Application>> GetNotSubmittedApplication(Int64 iPin, ApplicationTypeEnum applicationType, ApplicationPurposeTypeEnum applicationPurposeType)
        {
            GenericResponseTemplateModel<Application> genericServiceResultTemplate = new GenericResponseTemplateModel<Application>()
            {
                HasError = false,
                ErrorDesc = "",
                ResponseDataModel = null
            };
            try
            {
                genericServiceResultTemplate.ResponseDataModel = _context.Applications.Where(x => x.InvestPunjab_Ipin == iPin.ToString() && x.IsDeleted == false && x.ApplicationType == applicationType
                && x.ApplicationPurposeType == applicationPurposeType && (x.ApplicationLifeCycleStatusType == ApplicationLifeCycleStatusTypeEnum.NOT_SUBMITTED)).FirstOrDefault();
            }
            catch (Exception ex)
            {
                genericServiceResultTemplate.HasError = true;
                genericServiceResultTemplate.ErrorDesc = ex.Message;
            }
            return genericServiceResultTemplate;
        }

        public async Task<GenericResponseTemplateModel<DownloadApprovalViewModel>> DownloadApproval(string msg)
        {
            GenericResponseTemplateModel<DownloadApprovalViewModel> genericServiceResultTemplate = new GenericResponseTemplateModel<DownloadApprovalViewModel>();
            try
            {
                genericServiceResultTemplate.ResponseDataModel = new DownloadApprovalViewModel();
                ApplicationAction applicationAction = await _context.ApplicationActions.Where(x => x.ApplicationRefId == Convert.ToInt64(msg)).FirstOrDefaultAsync();
                Application application = await _context.Applications.Where(x => x.AppId == Convert.ToInt64(msg)).FirstOrDefaultAsync();
                int days = (applicationAction.ActionDate - DateTime.Now).Days;
                string downloadBaseUrl = "https://pblabour.gov.in/ePortal_api/License";
                string appForm = "";
                if (application.ApplicationType == ApplicationTypeEnum.BUILDING_PLAN_HUD)
                {
                    appForm = "AppForm_BUILDING_PLAN_HUD";
                }
                else if (application.ApplicationType == ApplicationTypeEnum.BUILDING_PLAN_PROPOSED)
                {
                    appForm = "AppForm_BUILDING_PLAN_PROPOSED";
                }
                else if (application.ApplicationType == ApplicationTypeEnum.BUILDING_PLAN_EXISTING)
                {
                    appForm = "AppForm_BUILDING_PLAN_EXISTING";
                }
                else if (application.ApplicationType == ApplicationTypeEnum.BUILDING_PLAN_ADDITION_AMENDMENT)
                {
                    appForm = "AppForm_BUILDING_PLAN_ADDITION_AMENDMENT";
                }

                if (applicationAction.AppActionType == (int)AppActionTypeEnum.DEEMED_RAISED_FEE_PAID)
                {
                    genericServiceResultTemplate.ResponseDataModel.IsCertificateDownload = true;
                    genericServiceResultTemplate.ResponseDataModel.MessageText = "You can download cerificate by clicking following link..!";
                    genericServiceResultTemplate.ResponseDataModel.DaysLeft = 0;
                    genericServiceResultTemplate.ResponseDataModel.CertificatePath = downloadBaseUrl + "/" + appForm + "/" + application.PublicAppRefNum + ".pdf";
                }
                else if (applicationAction.AppActionType == (int)AppActionTypeEnum.DEEMED_WITH_FEE_PENDING && days>5)
                {
                    genericServiceResultTemplate.ResponseDataModel.IsCertificateDownload = true;
                    genericServiceResultTemplate.ResponseDataModel.MessageText = "You can download cerificate by clicking following link..!";
                    genericServiceResultTemplate.ResponseDataModel.DaysLeft = 0;
                    genericServiceResultTemplate.ResponseDataModel.CertificatePath = downloadBaseUrl + "/" + appForm + "/" + application.PublicAppRefNum + ".pdf";
                }
                else
                {
                    genericServiceResultTemplate.ResponseDataModel.IsCertificateDownload = false;
                    genericServiceResultTemplate.ResponseDataModel.MessageText = "You are not eligible due to some conditions are not fullfiled..!";
                    genericServiceResultTemplate.ResponseDataModel.DaysLeft = 0;
                    genericServiceResultTemplate.ResponseDataModel.CertificatePath = "";
                }
            }
            catch (Exception ex)
            {
                genericServiceResultTemplate.HasError = true;
                genericServiceResultTemplate.ErrorDesc = ex.Message;
            }
            return genericServiceResultTemplate;
        }

        public async Task<string> ValidateLoginFromPartnerPortal(string msg, HttpRequest httpRequest)
        {
            msg = msg.Replace(" ", "+");
            var privateKeyPath = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), "CommonUtiliteis", "RSA", "Keys")).Root + "Version_2_pri.key_L2L.pem";
            string decryptedMsg = RsaHelper.Decrypt(msg, privateKeyPath);
            decryptedMsg = decryptedMsg.Replace("\"", "");
            string[] queryData = decryptedMsg.Split('|');
            string userId = queryData[0];
            string loggedInTime = queryData[1];
            string roleName = queryData[2];
            string portalType = queryData[3];

            var username = await _context.Users.Where(x => x.Id == userId).Select(x=> x.UserName).FirstOrDefaultAsync();

            DateTime LinkDateTime;

            if (queryData != null)
            {
                if (DateTime.TryParse(queryData[1].ToString(), out LinkDateTime))
                {
                    TimeSpan timeDifference = DateTime.Now - LinkDateTime;
                    if (timeDifference.TotalMinutes > 1)
                    {
                        return RijndaelManagedCryptoHandler.EncryptFromPlainTextToBase64(JsonConvert.SerializeObject(new LoginResponseViewModel()
                        {
                            ErrorDesc = "Login request is expired..!",
                            ErrorCode = "Login request is expired..!",
                            HasError = true,
                            IsMobileOtpVerificationReq = true,
                            IsMobileOtpVerified = false,
                            MobileNoToBeSentOtp = "",
                            Token = null
                        }),
                        Configuration.GetSection("EncryptionConfigs").GetSection("TokenDataEncryptionKey").Value,
                        Configuration.GetSection("EncryptionConfigs").GetSection("TokenDataIVKey").Value);
                    }
                    else
                    {

                        var guid = Guid.NewGuid().ToString();
                        DynamicFormViewModel dynamicForm = new DynamicFormViewModel()
                        {
                            DynamicFormFields = new List<DynamicFormFieldsViewModel>()
                            {
                                new DynamicFormFieldsViewModel ()
                                {
                                  Key="",
                                  IsClientSideEncryption= false,
                                  KeyCode= "Username",
                                  Value=username.ToString(),
                                  IsHidden= false,
                                  Type= "",
                                  CaptionText = "",
                                },
                                new DynamicFormFieldsViewModel ()
                                {
                                  Key="",
                                  IsClientSideEncryption= false,
                                  KeyCode= "Password",
                                  Value= "frompartnerportal|"+guid,
                                  IsHidden= false,
                                  Type= "", 
                                  CaptionText = "",
                                },
                                new DynamicFormFieldsViewModel ()
                                {
                                  Key="",
                                  IsClientSideEncryption= false,
                                  KeyCode= "OriginalCaptcha",
                                  Value="xyz",
                                  IsHidden= false,
                                  Type= "",
                                  CaptionText = "",
                                },
                                new DynamicFormFieldsViewModel ()
                                {
                                  Key="",
                                  IsClientSideEncryption= false,
                                  KeyCode= "EnteredCaptcha",
                                  Value="xyz",
                                  IsHidden= false,
                                  Type= "",
                                  CaptionText = "",
                                },
                                new DynamicFormFieldsViewModel ()
                                {
                                  Key="",
                                  IsClientSideEncryption= false,
                                  KeyCode= "IsFromPartnerPortal",
                                  Value="Y",
                                  IsHidden= false,
                                  Type= "",
                                  CaptionText = "",
                                },
                            },
                            ClientId_OnCreation = guid,
                            ClientId_OnSubmission = guid,
                            FormCreatedOn = DateTime.Now.Year.ToString() + "-" + DateTime.Now.Month.ToString() + "-" + DateTime.Now.Day.ToString() + "-" + DateTime.Now.Hour.ToString() + "-" + DateTime.Now.Minute.ToString()
                        };
                        return await _iAuthService.ValidateLoginInputForm(dynamicForm,httpRequest);
                    }
                }
                
            }
            else
            {
                return RijndaelManagedCryptoHandler.EncryptFromPlainTextToBase64(JsonConvert.SerializeObject(new LoginResponseViewModel()
                {
                    ErrorDesc = "Invalid request. Please login again..!",
                    ErrorCode = "Invalid request. Please login again..!",
                    HasError = true,
                    IsMobileOtpVerificationReq = true,
                    IsMobileOtpVerified = false,
                    MobileNoToBeSentOtp = "",
                    Token = null
                }),
                        Configuration.GetSection("EncryptionConfigs").GetSection("TokenDataEncryptionKey").Value,
                        Configuration.GetSection("EncryptionConfigs").GetSection("TokenDataIVKey").Value);
            }

            return null;
        }

        public async Task<GenericFormModel<List<Licence_CL_PE_Contrator>>> GetCLPEContractorListNew(string licenceNumber)
        {
            GenericFormModel<List<Licence_CL_PE_Contrator>> genericFormModel = new GenericFormModel<List<Licence_CL_PE_Contrator>>();
            try
            {
                List<StoreProcedureParm> storeProcedureParms = new List<StoreProcedureParm>()
         {
     new StoreProcedureParm (){ ParmName="LicenceNumber", ParmValue=licenceNumber.ToString(), isNumber=false},
         };
                var emps = await _iGeneric_SP_Repository.CallStoreProcedureReaderAsync<Licence_CL_PE_Contrator>("sp_CL_PELicence_GetAllContractor_New", storeProcedureParms);

                genericFormModel.FormModel = new List<Licence_CL_PE_Contrator>();
                genericFormModel.FormModel = emps;
            }
            catch (Exception ex)
            {
                genericFormModel.HasError = true;
                genericFormModel.ErrorDesc = ex.Message;
            }
            return genericFormModel;
        }

        public async Task<GenericResponseTemplateModel<List<BOCWTransparencyViewModel>>> GetBOCWTransparencyData(TransperancyReportViewModel requestData)
        {
            GenericResponseTemplateModel<List<BOCWTransparencyViewModel>> genericResponseTemplateModel = new GenericResponseTemplateModel<List<BOCWTransparencyViewModel>>()
            {
                HasError = false,
                ErrorDesc = ""
            };

            try
            {
                string url = Configuration.GetSection("ThirdPartyIntegrationConfigs").GetSection("BOCW").GetSection("GetTransparencyDataUrl").Value;

                using (var httpClient = new HttpClient())
                {
                    var requestObj = new
                    {
                        fromDate = requestData.FromDate.ToString("yyyy-MM-dd"),
                        toDate = requestData.ToDate.ToString("yyyy-MM-dd")
                    };

                    StringContent content = new StringContent(JsonConvert.SerializeObject(requestObj), Encoding.UTF8, "application/json");

                    using (var response = await httpClient.PostAsync(url, content))
                    {
                        string apiResponse = await response.Content.ReadAsStringAsync();

                        dynamic responseObj = JsonConvert.DeserializeObject(apiResponse);

                        string data = responseObj.result.data.ToString();

                        genericResponseTemplateModel.ResponseDataModel = JsonConvert.DeserializeObject<List<BOCWTransparencyViewModel>>(data);
                    }
                }
            }
            catch (Exception ex)
            {
                genericResponseTemplateModel.HasError = true;
                genericResponseTemplateModel.ErrorDesc = ex.Message;
            }

            return genericResponseTemplateModel;
        }
    }
}