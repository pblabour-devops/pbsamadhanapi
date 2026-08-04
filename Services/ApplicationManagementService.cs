using DocumentFormat.OpenXml.Vml.Spreadsheet;
using DocumentFormat.OpenXml.Wordprocessing;
using EFCore.BulkExtensions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Newtonsoft.Json;
using pbsamadhannetcoreapi.CommonUtiliteis;
using pbsamadhannetcoreapi.Models;
using pbsamadhannetcoreapi.Repositories.Implementations;
using pbsamadhannetcoreapi.Services.Implementations;
using pbsamadhannetcoreapi.ViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography.Xml;
using System.Threading.Tasks;

namespace pbsamadhannetcoreapi.Services
{
    public class ApplicationManagementService<T> : IApplicationManagementService<T> where T : class
    {
        private IApplicationManagementRepository<T> _iApplicationMamnagementRepository;
        private readonly IGenericRepository<Establishment_GeneralDetail> _iGR_Establishment_GeneralDetail;
        private readonly IGeneric_SP_Repository _iGeneric_SP_Repository;
        private readonly AppDbContext _context;
        private readonly IGenericRepository<ApplicationAction> _iGR_ApplicationAction;
        private readonly IGenericRepository<ApplicationActionLog> _iGR_ApplicationActionLog;
        private readonly UserManager<User> _userManager;
        private readonly INotificationManagerService _iNotificationManagerService;
        private readonly IGenericRepository<CommonLicence_GeneralDetail> _iGR_CommonLicence_GeneralDetail;
        private readonly IGenericRepository<Contractor_GeneralDetail> _iGR_Contractor_GeneralDetail;
        private readonly IAuthService _iAuthService;
        private readonly IGenericRepository<Application> _iGR_Application;
        private readonly IGenericRepository<BuildingPlan> _iGR_BuildingPlanDetail;
        private readonly IGenericRepository<BuildingPlan_AreaDetail> _iGR_BuildingPlan_AreaDetail;
        private readonly IGenericRepository<BuildingPlanHUD_GeneralDetail> _iGR_BuildingPlanHUD_GeneralDetail;
        private readonly IGenericRepository<RoleWiseAllowedActionCode> _iGR_RoleWiseAllowedActionCode;
        private readonly IInvestPunjabShareStatusService _iInvestPunjabShareStatusService;
        private readonly IGenericRepository<ShopLicence_GeneralDetail> _iGR_ShopLicence_GeneralDetail;
        private readonly IGenericRepository<ApplicationActionCode> _iGR_ApplicationActionCode;
        private readonly IGenericRepository<BuildingPlanFactory_GeneralDetail> _iGR_BuildingPlanFactory_GeneralDetail;
        private readonly IGenericRepository<Licence_Factory_GeneralDetail> _iGR_Licence_Factory_GeneralDetail;
        private readonly IGenericRepository<Licence_Shop_NightShift_Approval> _iGR_Licence_Shop_NightShift_Approval;
        private readonly IGenericRepository<Licence_Factory_NightShift_Approval> _iGR_Licence_Factory_NightShift_Approval;
        private readonly IGenericRepository<Licence_ContractLabour_GeneralDetail> _iGR_Licence_ContractLabour_GeneralDetail;
        private IHttpContextAccessor _iAccessor;
        private readonly IGenericRepository<BuildingPlanFactory_Declaration_Stability_Certificate> _iGR_BuildingPlanFactory_Declaration_Stability_Certificate;
        public IConfiguration Configuration { get; }
        private readonly IGenericRepository<Licence_BocwAct_GeneralDetail> _iGR_Licence_BocwAct_GeneralDetail;
        private readonly IGenericRepository<Licence_MotorTransport> _iGR_Licence_MotorTransport;

        private readonly IGenericRepository<Licence_Proposed_BuildingPlan_GeneralDetail> _iGR_Licence_Proposed_BuildingPlan_GeneralDetail;
        private readonly IGenericRepository<Licence_Existing_BuildingPlan_GeneralDetail> _iGR_Licence_Existing_BuildingPlan_GeneralDetail;
        private readonly IGenericRepository<Licence_Addition_Amendment_BuildingPlan_GeneralDetail> _iGR_Licence_Addition_Amendment_BuildingPlan_GeneralDetail;
        private readonly IGenericRepository<Licence_PE_ISM_GeneralDetail> _iGR_Licence_PE_ISM_GeneralDetail;
        private readonly IGenericRepository<Licence_CL_PE_GeneralDetail> _iGR_Licence_Principal_Employer_GeneralDetail;
        private readonly IAppTimeLineManagerService _iAppTimeLineManagerService;
        private readonly IGenericRepository<Licence_BuildingPlan_PSIEC_GeneralDetail> _iGR_Licence_BuildingPlan_PSIEC_GeneralDetail;
        private readonly IGenericRepository<Licence_TradeUnion> _iGR_Licence_Trade_Union_GeneralDetail;
        private readonly IGenericRepository<Licence_ISM_ContractLabour_GeneralDetail> _iGR_Licence_ISM_ContractLabour_GeneralDetail;
        private readonly IAppTimeLineManagerService _AppTimeLineManagerService;
        private IServiceScopeFactory _iServiceScopeFactory;
        public ApplicationManagementService(
            IApplicationManagementRepository<T> iApplicationMamnagementRepository,
            IGenericRepository<Establishment_GeneralDetail> iGR_Establishment_GeneralDetail, 
            IGeneric_SP_Repository iGeneric_SP_Repository,
            AppDbContext context,
            IGenericRepository<ApplicationAction> iGR_ApplicationAction,
            IGenericRepository<ApplicationActionLog> iGR_ApplicationActionLog,
            UserManager<User> userManager,
            INotificationManagerService iNotificationManagerService,
            IGenericRepository<CommonLicence_GeneralDetail> iGR_CommonLicence_GeneralDetail,
            IGenericRepository<Contractor_GeneralDetail> iGR_Contractor_GeneralDetail,
            IAuthService iAuthService,
            IGenericRepository<Application> iGR_Application,
            IGenericRepository<BuildingPlan> iGR_BuildingPlanDetail,
            IGenericRepository<BuildingPlan_AreaDetail> iGR_BuildingPlan_AreaDetail,
            IGenericRepository<BuildingPlanHUD_GeneralDetail> iGR_BuildingPlanHUD_GeneralDetail,
            IGenericRepository<RoleWiseAllowedActionCode> iGR_RoleWiseAllowedActionCode,
            IInvestPunjabShareStatusService iInvestPunjabShareStatusService,
            IConfiguration configuration,
            IGenericRepository<ShopLicence_GeneralDetail> iGR_ShopLicence_GeneralDetail,
            IGenericRepository<ApplicationActionCode> iGR_ApplicationActionCode,
            IGenericRepository<BuildingPlanFactory_GeneralDetail> iGR_BuildingPlanFactory_GeneralDetail,
            IGenericRepository<Licence_Factory_GeneralDetail> iGR_Licence_Factory_GeneralDetail,
            IGenericRepository<Licence_Shop_NightShift_Approval> iGR_Licence_Shop_NightShift_Approval,
            IGenericRepository<Licence_ContractLabour_GeneralDetail> iGR_Licence_ContractLabour_GeneralDetail,
            IHttpContextAccessor iAccessor,
            IGenericRepository<BuildingPlanFactory_Declaration_Stability_Certificate> iGR_BuildingPlanFactory_Declaration_Stability_Certificate,
            IGenericRepository<Licence_Factory_NightShift_Approval> iGR_Licence_Factory_NightShift_Approval,
            IGenericRepository<Licence_BocwAct_GeneralDetail> iGR_Licence_BocwAct_GeneralDetail,
            IGenericRepository<Licence_MotorTransport> iGR_Licence_MotorTransport,
            IGenericRepository<Licence_Proposed_BuildingPlan_GeneralDetail> iGR_Licence_Proposed_BuildingPlan_GeneralDetail,
            IGenericRepository<Licence_Existing_BuildingPlan_GeneralDetail> iGR_Licence_Existing_BuildingPlan_GeneralDetail,
            IGenericRepository<Licence_Addition_Amendment_BuildingPlan_GeneralDetail> iGR_Licence_Addition_Amendment_BuildingPlan_GeneralDetail,
            IGenericRepository<Licence_PE_ISM_GeneralDetail> iGR_Licence_PE_ISM_GeneralDetail,
            IGenericRepository<Licence_CL_PE_GeneralDetail> iGR_Licence_Principal_Employer_GeneralDetail,
             IAppTimeLineManagerService iAppTimeLineManagerService,
             IGenericRepository<Licence_BuildingPlan_PSIEC_GeneralDetail> iGR_Licence_BuildingPlan_PSIEC_GeneralDetail,
             IGenericRepository<Licence_TradeUnion> iGR_Licence_Trade_Union_GeneralDetail,
             IGenericRepository<Licence_ISM_ContractLabour_GeneralDetail> iGR_Licence_ISM_ContractLabour_GeneralDetail,
             IAppTimeLineManagerService appTimeLineManagerService,
            IServiceScopeFactory iServiceScopeFactory
           )
        {
            _iApplicationMamnagementRepository = iApplicationMamnagementRepository;
            _iGR_Establishment_GeneralDetail = iGR_Establishment_GeneralDetail;
            _iGeneric_SP_Repository = iGeneric_SP_Repository;
            _context = context;
            _iGR_ApplicationAction = iGR_ApplicationAction;
            _iGR_ApplicationActionLog = iGR_ApplicationActionLog;
            _userManager = userManager;
            _iNotificationManagerService = iNotificationManagerService;
            _iGR_CommonLicence_GeneralDetail = iGR_CommonLicence_GeneralDetail;
            _iGR_Contractor_GeneralDetail = iGR_Contractor_GeneralDetail;
            _iAuthService = iAuthService;
            _iGR_Application = iGR_Application;
            _iGR_BuildingPlanDetail = iGR_BuildingPlanDetail;
            _iGR_BuildingPlan_AreaDetail = iGR_BuildingPlan_AreaDetail;
            _iGR_BuildingPlanHUD_GeneralDetail = iGR_BuildingPlanHUD_GeneralDetail;
            _iGR_RoleWiseAllowedActionCode = iGR_RoleWiseAllowedActionCode;
            _iInvestPunjabShareStatusService = iInvestPunjabShareStatusService;
            Configuration = configuration;
            _iGR_ShopLicence_GeneralDetail = iGR_ShopLicence_GeneralDetail;
            _iGR_ApplicationActionCode = iGR_ApplicationActionCode;
            _iGR_BuildingPlanFactory_GeneralDetail = iGR_BuildingPlanFactory_GeneralDetail;
            _iGR_Licence_Factory_GeneralDetail = iGR_Licence_Factory_GeneralDetail;
            _iGR_Licence_Shop_NightShift_Approval = iGR_Licence_Shop_NightShift_Approval;
            _iAccessor = iAccessor;
            _iGR_Licence_ContractLabour_GeneralDetail = iGR_Licence_ContractLabour_GeneralDetail;
            _iGR_BuildingPlanFactory_Declaration_Stability_Certificate = iGR_BuildingPlanFactory_Declaration_Stability_Certificate;
            _iGR_Licence_Factory_NightShift_Approval = iGR_Licence_Factory_NightShift_Approval;
            _iGR_Licence_BocwAct_GeneralDetail = iGR_Licence_BocwAct_GeneralDetail;
            _iGR_Licence_MotorTransport = iGR_Licence_MotorTransport;
            _iGR_Licence_Proposed_BuildingPlan_GeneralDetail = iGR_Licence_Proposed_BuildingPlan_GeneralDetail;
            _iGR_Licence_Existing_BuildingPlan_GeneralDetail = iGR_Licence_Existing_BuildingPlan_GeneralDetail;
            _iGR_Licence_Addition_Amendment_BuildingPlan_GeneralDetail = iGR_Licence_Addition_Amendment_BuildingPlan_GeneralDetail;
            _iGR_Licence_PE_ISM_GeneralDetail = iGR_Licence_PE_ISM_GeneralDetail;
            _iGR_Licence_Principal_Employer_GeneralDetail = iGR_Licence_Principal_Employer_GeneralDetail;
            _iAppTimeLineManagerService = iAppTimeLineManagerService;
            _iGR_Licence_BuildingPlan_PSIEC_GeneralDetail = iGR_Licence_BuildingPlan_PSIEC_GeneralDetail;
            _iGR_Licence_Trade_Union_GeneralDetail = iGR_Licence_Trade_Union_GeneralDetail;
            _iGR_Licence_ISM_ContractLabour_GeneralDetail = iGR_Licence_ISM_ContractLabour_GeneralDetail;
            _AppTimeLineManagerService = appTimeLineManagerService;
            _iServiceScopeFactory = iServiceScopeFactory;

        }

        public async Task<List<AppFormStepsInfo>> GetAppFormStepperInfo(Int64 appRefId, ApplicationTypeEnum applicationType, Int64 entityParentKeyId, string stepCode)
        {
            List<AppFormStepsInfo> appFormSteps = new List<AppFormStepsInfo>();
            dynamic parentWithChildObject = null;
            bool isParentTableHasData = false;
            //Int64 appRefId = 0;
            string detailPageUiComponentUrl = "";
            if (applicationType == ApplicationTypeEnum.REG_ESTB_OSH)
            {
                parentWithChildObject = await _iGR_Establishment_GeneralDetail
                        .GetAsync(x => x.EstablishmentId == entityParentKeyId,  //Conditions         
                          null,                //Orders          
                          x => x.Establishment_EmployerDetail, x => x.Application, x => x.Establishment_ContractorsDetail, x => x.Establishment_Migrantworkers)      //Includes
                        .ConfigureAwait(false);
                List<Establishment_GeneralDetail> parentObject = parentWithChildObject;
                detailPageUiComponentUrl = "/establishment/detail";
                if (parentObject != null && parentObject.Count() > 0 && parentObject.FirstOrDefault().Application != null)
                {
                    appRefId = parentObject.FirstOrDefault().AppRefId;
                    isParentTableHasData = true;
                }

                appFormSteps.Add(new AppFormStepsInfo()
                {
                    StepTitle = "General Details",
                    EntityParentKeyId = entityParentKeyId,
                    IsFilled = parentObject.FirstOrDefault() == null ? false : true,
                    IsLink = true,
                    UiPageComponentPath = "/establishment/addupdategeneraldetail",
                    StepCode = "GD",
                    ApplicationType = applicationType,
                    AppRefId = appRefId,
                    IsCommonStep = false,
                    UiNextPageComponentPath = "/establishment/addupdateemployerdetail",
                    ProjectSiteRefId = parentObject.FirstOrDefault() == null ? 0 : parentObject.FirstOrDefault().Application.ProjectSiteRefId
                });

                appFormSteps.Add(new AppFormStepsInfo()
                {
                    StepTitle = "Employer Details",
                    EntityParentKeyId = entityParentKeyId,
                    IsFilled = parentObject.FirstOrDefault() == null ? false : parentObject.FirstOrDefault().Establishment_EmployerDetail == null ? false : true,
                    IsLink = entityParentKeyId == 0 ? false : true,
                    UiPageComponentPath = "/establishment/addupdateemployerdetail",
                    StepCode = "ED",
                    ApplicationType = applicationType,
                    AppRefId = appRefId,
                    IsCommonStep = false,
                    UiNextPageComponentPath = "/establishment/addupdatecontractordetail",
                    ProjectSiteRefId = parentObject.FirstOrDefault() == null ? 0 : parentObject.FirstOrDefault().Application.ProjectSiteRefId
                });
                bool IsEmployingInterStateMigrantWorkers = (parentObject.FirstOrDefault() != null && parentObject.FirstOrDefault().IsEmployingInterStateMigrantWorkers);


                appFormSteps.Add(new AppFormStepsInfo()
                {
                    StepTitle = "Contractor Details",
                    EntityParentKeyId = entityParentKeyId,
                    IsFilled = parentObject.FirstOrDefault() == null ? false : parentObject.FirstOrDefault().Establishment_ContractorsDetail.Count == 0 ? false : true,
                    IsLink = entityParentKeyId == 0 ? false : true,
                    UiPageComponentPath = "/establishment/addupdatecontractordetail",
                    StepCode = "CD",
                    ApplicationType = applicationType,
                    AppRefId = appRefId,
                    IsCommonStep = false,
                    UiNextPageComponentPath = IsEmployingInterStateMigrantWorkers ? "/establishment/addupdatemigrantdetail" : "/shared/appdocuments",
                    ProjectSiteRefId = parentObject.FirstOrDefault() == null ? 0 : parentObject.FirstOrDefault().Application.ProjectSiteRefId
                });
                if (IsEmployingInterStateMigrantWorkers)
                {
                    appFormSteps.Add(new AppFormStepsInfo()
                    {
                        StepTitle = "Inter-State Migrant Workers",
                        EntityParentKeyId = entityParentKeyId,
                        IsFilled = parentObject.FirstOrDefault() == null ? false : (parentObject.FirstOrDefault().Establishment_Migrantworkers == null || parentObject.FirstOrDefault().Establishment_Migrantworkers.Count() == 0) ? false : true,
                        IsLink = entityParentKeyId == 0 ? false : true,
                        UiPageComponentPath = "/establishment/addupdatemigrantdetail",
                        StepCode = "MW",
                        ApplicationType = applicationType,
                        AppRefId = appRefId,
                        IsCommonStep = false,
                        UiNextPageComponentPath = "/shared/appdocuments",
                        ProjectSiteRefId = parentObject.FirstOrDefault() == null ? 0 : parentObject.FirstOrDefault().Application.ProjectSiteRefId
                    });
                }
            }

            #region For samadhaan
            else if (applicationType == ApplicationTypeEnum.SAMADHAN_COMPLAINTS)
            {
                parentWithChildObject = await _context.Applications
                                   .Include(x => x.WorkerDetail)    
                                   .Include(x => x.Complaint_EmployerORContractorDetails)
                                   .Include(x => x.Complaint_GratuityClaims)
                                   .Include(x => x.Complaint_Claim_CodeOnWages)
                                   .Include(x => x.Complaint_MaternityBenefitComplaints).Where(x => x.AppId == appRefId).FirstOrDefaultAsync(); 
                var mappedComplaintCategoryIds = await _context.AppComplaintTypeMappings.Where(x => x.AppRefId == appRefId).Select(x => x.ComplaintsCategoryRefId).Distinct().ToListAsync();
                var ComplaintsCategories = await _context.ComplaintsCategories.ToListAsync();

                detailPageUiComponentUrl = "/samadhaan/details";

                if (parentWithChildObject != null)
                {
                    entityParentKeyId = parentWithChildObject.WorkerDetail.Id;
                    isParentTableHasData = true;
                }
                appFormSteps.Add(new AppFormStepsInfo()
                {
                    StepTitle = "Worker Details",
                    EntityParentKeyId = entityParentKeyId,
                    IsFilled = parentWithChildObject != null,
                    IsLink = true,
                    UiPageComponentPath = "/samadhaan/worker-details",
                    StepCode = "WD",
                    ApplicationType = applicationType,
                    AppRefId = appRefId,
                    IsCommonStep = false,
                    UiNextPageComponentPath = "/samadhaan/employer-details",
                    RootActivityRefId = "0",
                    ToDoActivityCategoryType = ToDoActivityCategoryTypeEnum.DEFAULT,
                    ToDoActivityModeType = ToDoActivityModeTypeEnum.DEFAULT
                });

                appFormSteps.Add(new AppFormStepsInfo()
                {
                    StepTitle = "Employer Establishment Details",
                    EntityParentKeyId = entityParentKeyId,
                    IsFilled = false,
                    IsLink = entityParentKeyId == 0 ? false : true,
                    UiPageComponentPath = "/samadhaan/employer-details",
                    StepCode = "EED",
                    ApplicationType = applicationType,
                    AppRefId = appRefId,
                    IsCommonStep = false,
                    UiNextPageComponentPath = "samadhaan/gratuity-claims",
                    RootActivityRefId = "0",
                    ToDoActivityCategoryType = ToDoActivityCategoryTypeEnum.DEFAULT,
                    ToDoActivityModeType = ToDoActivityModeTypeEnum.DEFAULT
                });

                //{
                appFormSteps.Add(new AppFormStepsInfo()
                {
                    StepTitle = "Gratuity Claims",
                    EntityParentKeyId = entityParentKeyId,
                    IsFilled = false,
                    IsLink = entityParentKeyId == 0 ? false : true,
                    UiPageComponentPath = "/samadhaan/gratuity-claims",
                    StepCode = "GC",
                    ApplicationType = applicationType,
                    AppRefId = appRefId,
                    IsCommonStep = false,
                    UiNextPageComponentPath = "samadhaan/wages",
                    RootActivityRefId = "0",
                    ToDoActivityCategoryType = ToDoActivityCategoryTypeEnum.DEFAULT,
                    ToDoActivityModeType = ToDoActivityModeTypeEnum.DEFAULT
                });

                appFormSteps.Add(new AppFormStepsInfo()
                {
                    StepTitle = "Claim Under Code On Wages",
                    EntityParentKeyId = entityParentKeyId,
                    IsFilled = false,
                    IsLink = entityParentKeyId == 0 ? false : true,
                    UiPageComponentPath = "/samadhaan/wages",
                    StepCode = "CCOW",
                    ApplicationType = applicationType,
                    AppRefId = appRefId,
                    IsCommonStep = false,
                    UiNextPageComponentPath = "/samadhaan/mb-complaint",
                    RootActivityRefId = "0",
                    ToDoActivityCategoryType = ToDoActivityCategoryTypeEnum.DEFAULT,
                    ToDoActivityModeType = ToDoActivityModeTypeEnum.DEFAULT
                });
                //}

                appFormSteps.Add(new AppFormStepsInfo()
                {
                    StepTitle = "Meternity Benefits Complaints",
                    EntityParentKeyId = entityParentKeyId,
                    IsFilled = false,
                    IsLink = entityParentKeyId == 0 ? false : true,
                    UiPageComponentPath = "/samadhaan/mb-complaint",
                    StepCode = "MBC",
                    ApplicationType = applicationType,
                    AppRefId = appRefId,
                    IsCommonStep = false,
                    UiNextPageComponentPath = "/samadhaan/recovery-of-money",
                    RootActivityRefId = "0",
                    ToDoActivityCategoryType = ToDoActivityCategoryTypeEnum.DEFAULT,
                    ToDoActivityModeType = ToDoActivityModeTypeEnum.DEFAULT
                });

                appFormSteps.Add(new AppFormStepsInfo()
                {
                    StepTitle = "Recovery Of Money Under Section 59(1) of IR Code",
                    EntityParentKeyId = entityParentKeyId,
                    IsFilled = false,
                    IsLink = entityParentKeyId == 0 ? false : true,
                    UiPageComponentPath = "/samadhaan/recovery-of-money",
                    StepCode = "RM",
                    ApplicationType = applicationType,
                    AppRefId = appRefId,
                    IsCommonStep = false,
                    UiNextPageComponentPath = detailPageUiComponentUrl,
                    RootActivityRefId = "0",
                    ToDoActivityCategoryType = ToDoActivityCategoryTypeEnum.DEFAULT,
                    ToDoActivityModeType = ToDoActivityModeTypeEnum.DEFAULT
                });


                //appFormSteps.Add(new AppFormStepsInfo()
                //{
                //    StepTitle = "Review",
                //    EntityParentKeyId = entityParentKeyId,
                //    IsFilled = false,
                //    IsLink = entityParentKeyId == 0 ? false : true,
                //    UiPageComponentPath = "/samadhaan/review",
                //    StepCode = "ROD",
                //    ApplicationType = applicationType,
                //    AppRefId = appRefId,
                //    IsCommonStep = false,
                //    UiNextPageComponentPath = detailPageUiComponentUrl,
                //});
            }

            #endregion

            //Common Steps
            object obj = parentWithChildObject;
            bool isLocked = false;
            bool isFeeApplicable = false;
            ApplicationLifeCycleStatusTypeEnum applicationLifeCycleStatusType;
            if (obj != null && (!(obj is System.Collections.ICollection col) || col.Count > 0))
            {
                if (obj.GetType().GetProperties().Count(x => x.Name == "IsLocked") > 0)
                {
                    isLocked = Convert.ToBoolean(obj.GetType().GetProperty("IsLocked").GetValue(obj));
                    isFeeApplicable = Convert.ToBoolean(obj.GetType().GetProperty("IsFeeApplicable").GetValue(obj));
                    applicationLifeCycleStatusType = (ApplicationLifeCycleStatusTypeEnum)(obj.GetType().GetProperty("ApplicationLifeCycleStatusType").GetValue(obj));
                }
                else
                {
                    isLocked = (Enumerable.FirstOrDefault(parentWithChildObject).Application.IsLocked);
                    isFeeApplicable = Enumerable.FirstOrDefault(parentWithChildObject).Application.IsFeeApplicable;
                    applicationLifeCycleStatusType = Enumerable.FirstOrDefault(parentWithChildObject).Application.ApplicationLifeCycleStatusType;
                }
            }
            else if (applicationType == ApplicationTypeEnum.LABOUR_SERVICES)
            {
                isParentTableHasData = true;
                isLocked = isFeeApplicable = true;
                applicationLifeCycleStatusType = ApplicationLifeCycleStatusTypeEnum.NOT_SUBMITTED;
            }   
            else
            {
               
                isLocked = isFeeApplicable = false;
                applicationLifeCycleStatusType = ApplicationLifeCycleStatusTypeEnum.NOT_SUBMITTED;
            }
            if (applicationType != ApplicationTypeEnum.LABOUR_SERVICES)
            {
                appFormSteps.Add(new AppFormStepsInfo()
                {
                    StepTitle = "Confirm & Lock",
                    EntityParentKeyId = entityParentKeyId,
                    IsFilled = !isParentTableHasData ? false : isLocked,
                    IsLink = false,
                    UiPageComponentPath = detailPageUiComponentUrl,
                    StepCode = "LOCK",
                    ApplicationType = applicationType,
                    AppRefId = appRefId,
                    IsCommonStep = true
                });
            }
            //if (isParentTableHasData && Enumerable.FirstOrDefault(parentWithChildObject).Application.IsDigitalSignatureRequired)
            //{
            //    appFormSteps.Add(new AppFormStepsInfo()
            //    {
            //        StepTitle = "Verify Digital Signature",
            //        EntityParentKeyId = entityParentKeyId,
            //        IsFilled = !isParentTableHasData ? false : Enumerable.FirstOrDefault(parentWithChildObject).Application.IsDigitalSignatureVerified,
            //        IsLink = false,
            //        UiPageComponentPath = "/digitalsignature/sign_application",
            //        StepCode = "DEG_SIG",
            //        ApplicationType = applicationType,
            //        AppRefId = appRefId,
            //        IsCommonStep = true
            //    });
            //}

            if (isParentTableHasData && isFeeApplicable
                && (applicationLifeCycleStatusType == ApplicationLifeCycleStatusTypeEnum.IN_OBJECTION_WITH_BALANCE_FEE
                    || applicationLifeCycleStatusType == ApplicationLifeCycleStatusTypeEnum.NOT_SUBMITTED)
                )
            {
                appFormSteps.Add(new AppFormStepsInfo()
                {
                    StepTitle = "Make Payment & Submit",
                    EntityParentKeyId = entityParentKeyId,
                    IsFilled = false,
                    IsLink = false,
                    UiPageComponentPath = "/payments/appfeecalculator",
                    StepCode = "PAYMNT",
                    ApplicationType = applicationType,
                    AppRefId = appRefId,
                    IsCommonStep = true
                });
            }

            appFormSteps.Where(x => x.StepCode == stepCode).Select(x => { x.IsCurrentStep = true; return x; }).ToList();

            //Lock step enable disable
            if (appFormSteps.Where(x=>x.IsCommonStep==false && x.IsFilled == false).ToList().Count() == 0 && appFormSteps.Where(x=>x.StepCode=="DOC" && x.IsFilled).Any())
            {
                appFormSteps.Where(x => x.StepCode == "LOCK").Select(x => { x.IsLink = true; return x; }).ToList();
            }

            //Digital Signature
            if ((appFormSteps.Where(x => x.StepCode=="LOCK" && x.IsFilled).ToList().Count() == 1) && appFormSteps.Where(x=>x.StepCode== "DEG_SIG").Any())
            {
                appFormSteps.Where(x => x.StepCode == "DEG_SIG").Select(x => { x.IsLink = true; return x; }).ToList();
            }

            //Make Payment
            if (appFormSteps.Where(x => x.StepCode == "LOCK" && x.IsFilled).ToList().Count() == 1 && appFormSteps.Where(x => x.StepCode == "PAYMNT").Any())
            {
                if(appFormSteps.Where(x => x.StepCode == "DEG_SIG" && x.IsFilled).Any())
                {
                    appFormSteps.Where(x => x.StepCode == "PAYMNT").Select(x => { x.IsLink = true; return x; }).ToList();
                }
                else if (!appFormSteps.Where(x => x.StepCode == "DEG_SIG").Any())
                {
                    appFormSteps.Where(x => x.StepCode == "PAYMNT").Select(x => { x.IsLink = true; return x; }).ToList();
                }
            }
            else if(applicationType == ApplicationTypeEnum.LABOUR_SERVICES)
            {
                appFormSteps.Where(x => x.StepCode == "PAYMNT").Select(x => { x.IsLink = true; return x; }).ToList();

            }
            return appFormSteps;
        }

        public async Task<ApplicationInitiateResponseViewModel> InitiateApplication(ApplicationTypeEnum applicationType, T entityType, Int64 projectSiteRefId, ApplicationPurposeTypeEnum applicationPurposeType, string id, Int64 iPin, Int64 investPunjab_AppId, bool legacy_IsMigrated, Int64 legacy_AppId, Int64 Legacy_AppFormId, string legacy_NAR, string legacy_LicenceNo, int projectSiteVersion)
        {
            ApplicationInitiateResponseViewModel applicationInitiateResponse;
            //Step 1: Prepare application object
            //Application application = new Application()
            //{
            //    ApplicationType = applicationType,
            //    ApplicationPurposeType = applicationPurposeType,
            //    CreatedOnDate = DateTime.Now,
            //    LastModifiedOnDate = DateTime.Now,
            //    IsDeleted = false,
            //    IsEnabled = true,
            //    IsDigitalSignatureRequired = false,
            //    IsDigitalSignatureVerified = false,
            //    DigitalSignatureRefId = null,
            //    IsAllowEdit = true,
            //    IsLocked = false,
            //    ApplicationLifeCycleStatusType = ApplicationLifeCycleStatusTypeEnum.NOT_SUBMITTED,
            //    ApplicationLifeCycleLastStatusOn = DateTime.Now,
            //    PaymentBatchCounter = 1,
            //    Legacy_AppId = legacy_AppId,
            //    Legacy_IsMigrated = legacy_IsMigrated,
            //    Legacy_AppFormId = Legacy_AppFormId,
            //    Legacy_NAR = legacy_NAR,
            //    InvestPunjab_Ipin = iPin.ToString(),
            //    InvestPunjab_AppId = investPunjab_AppId,
            //    Legacy_LicenceNo = legacy_LicenceNo,
            //    ProjectSiteVersion = projectSiteVersion,
            //    IsIPIntegrated = true,
            //    IsTimeLineFlow = true
            //};

            bool isFeeApplicable = false;
            int departmentRefID;
            bool IsTimeLineFlow = true;
            bool IsIPIntegrated = true;
            if (applicationType == ApplicationTypeEnum.REG_ESTB_OSH || applicationType == ApplicationTypeEnum.CONTRACT_LABOUR_OSH || applicationType == ApplicationTypeEnum.COMMON_LICENCE_OSH)
            {
                isFeeApplicable = true;
                departmentRefID = 2;
            }
            else if(applicationType == ApplicationTypeEnum.BUILDING_PLAN_OSH)
            {
                isFeeApplicable = true;
                departmentRefID = 3;
            }
            else if (applicationType == ApplicationTypeEnum.BUILDING_PLAN_HUD || applicationType == ApplicationTypeEnum.BUILDING_PLAN || applicationType == ApplicationTypeEnum.FACTORY_LICENCE || applicationType == ApplicationTypeEnum.CONTRACT_LABOUR)
            {
                isFeeApplicable = true;
                departmentRefID = 3;
            }
            else if (applicationType == ApplicationTypeEnum.SHOP_LICENCE || applicationType == ApplicationTypeEnum.WOMEN_NIGHT_SHIFT_SHOP || applicationType == ApplicationTypeEnum.BP_DECLARATION_STABILITY_CERTIFICATE || applicationType == ApplicationTypeEnum.WOMEN_NIGHT_SHIFT_FACTORY)
            {
                isFeeApplicable = false;
                departmentRefID = 1;
            }
            else if (applicationType == ApplicationTypeEnum.BOCW_ESTABLISHMENT_ACT || applicationType == ApplicationTypeEnum.PRINCIPAL_EMPLOYER || applicationType == ApplicationTypeEnum.CONTRACT_LABOUR || applicationType == ApplicationTypeEnum.ISM_PRINCIPAL_EMPLOYER || applicationType == ApplicationTypeEnum.MOTOR_TRANSPORT || applicationType == ApplicationTypeEnum.TRADE_UNION || applicationType == ApplicationTypeEnum.ISM_CONTRACT_LABOUR)
            {
                isFeeApplicable = true;
                departmentRefID = 1;
            }
            else if (applicationType == ApplicationTypeEnum.BUILDING_PLAN_PROPOSED || applicationType == ApplicationTypeEnum.BUILDING_PLAN_EXISTING || applicationType == ApplicationTypeEnum.BUILDING_PLAN_ADDITION_AMENDMENT)
            {
                isFeeApplicable = false;
                departmentRefID = 3;
            }
            else if (applicationType == ApplicationTypeEnum.BUILDING_PLAN_PSIEC)
            {
                isFeeApplicable = false;
                departmentRefID = 3;
            }
            else if (applicationType == ApplicationTypeEnum.OSH_FORM_1_Registration)
            {
                isFeeApplicable = false;
                departmentRefID = 3;
            }
            else
            {
                isFeeApplicable = false;
                departmentRefID = 1;
            }

            if (applicationType == ApplicationTypeEnum.MOTOR_TRANSPORT || applicationType == ApplicationTypeEnum.TRADE_UNION)
            {
                IsIPIntegrated = false;
                IsTimeLineFlow = false;
            }

            var user = await _userManager.Users.Include(x => x.UserRoles).Include(x => x.UserProfileMapping).Include(x => x.UserProfileMapping.UserProfile).FirstOrDefaultAsync(x => x.Id == id);

            applicationInitiateResponse = await _iApplicationMamnagementRepository.InitiateApplication(entityType, projectSiteRefId, user, iPin, Legacy_AppFormId, legacy_NAR, legacy_IsMigrated, investPunjab_AppId, legacy_LicenceNo, legacy_AppId, (int)applicationType, (int)applicationPurposeType, projectSiteVersion, isFeeApplicable);

            if (applicationInitiateResponse.IsApplicationCreated)
            {
                Application application = await _context.Applications.Where(x => x.AppId == applicationInitiateResponse.AppId && x.IsDeleted == false).FirstOrDefaultAsync();
                if (applicationType == ApplicationTypeEnum.FACTORY_LICENCE && applicationPurposeType == ApplicationPurposeTypeEnum.GRANT_LICENCE)
                {
                    var factoryLicence = _context.Licence_Factory_GeneralDetails.Where(x => x.AppRefId == applicationInitiateResponse.AppId);
                    if (factoryLicence.FirstOrDefault().IsTempRegistered == 1 && factoryLicence.FirstOrDefault().IsTempRegistrationVerified == true)
                    {
                        application.IsFeeApplicable = false;
                        _context.Entry(application).State = EntityState.Modified;
                        await _context.SaveChangesAsync();
                    }
                }

                if (applicationType == ApplicationTypeEnum.BOCW_ESTABLISHMENT_ACT && applicationPurposeType == ApplicationPurposeTypeEnum.GRANT_LICENCE)
                {
                    var bocwLicence = _context.Licence_BocwAct_GeneralDetails.Where(x => x.AppRefId == applicationInitiateResponse.AppId);
                    if (bocwLicence.FirstOrDefault().BocwEngagedWorkerSlabType == BocwEngagedWorkerSlabTypeEnum.BELOW_10)
                    {
                        application.IsFeeApplicable = false;
                        _context.Entry(application).State = EntityState.Modified;
                        await _context.SaveChangesAsync();
                    }
                }
            }
            

            return applicationInitiateResponse;
        }

        public async Task<bool> UpdateAppLastModifiedDate(long AppId)
        {
            return await _iApplicationMamnagementRepository.UpdateAppLastModifiedDate(AppId);
        }

        public async Task<string> LockApplication(Int64 AppId, int AppActionType, string remarks, ApplicationTypeEnum applicationType)
        {
            var receiverUserId =await _iApplicationMamnagementRepository.LockApplication(AppId, AppActionType, remarks);


            var app = _context.Applications.Where(x => x.AppId == AppId && x.IsDeleted == false).FirstOrDefault();

            int investPunjabStatusCode = AppActionType;


            if (app.ApplicationType == ApplicationTypeEnum.FACTORY_LICENCE && app.ApplicationPurposeType == ApplicationPurposeTypeEnum.GRANT_LICENCE && app.IsFeeApplicable == false)
            {
                investPunjabStatusCode = 3;
                AppActionType = 405;
                //if(Convert.ToBoolean(Configuration.GetSection("AppProcessSetting").GetSection("TimeLineFileProcessingEnabled").Value))
                //{
                //    await _iAppTimeLineManagerService.SeedTimeLineData(AppId);
                //}

                //await _iAppTimeLineManagerService.SeedTimeLineData(AppId,0);

                if (app.IsTimeLineFlow)
                {
                    await _iAppTimeLineManagerService.SeedTimeLineData(AppId, 0);
                }

            }

            if (receiverUserId != "NAN" && receiverUserId != "")
            {
                if (applicationType != ApplicationTypeEnum.MOTOR_TRANSPORT)
                {
                    //var businessFirstRequestLog = await _context.BusinessFirst_RequestLogs.Where(x => x.NativeAppId == AppId && x.IsEnabled == true).FirstOrDefaultAsync();
                    var application = await _context.Applications.Where(x => x.AppId == AppId && x.IsDeleted == false).Include(x=> x.ProjectSites).FirstOrDefaultAsync();
                    var licenceInfo = await _context.ApplicationLicenceNoMapping.Where(x => x.AppRefId == AppId).FirstOrDefaultAsync();
                    Int64 appActionLogId = await _context.ApplicationActionLogs.Where(x => x.ApplicationRefId == AppId).OrderByDescending(x => x.ApplicationActionLogId).Select(x => x.ApplicationActionLogId).FirstOrDefaultAsync().ConfigureAwait(false);

                    if (application != null)
                    {
                        var statusDescription = await _context.ApplicationActionCodes.Where(x => x.ActionCode == AppActionType).FirstOrDefaultAsync();
                        List<StoreProcedureParm> storeProcedureParms = new List<StoreProcedureParm>()
                        {
                            new StoreProcedureParm (){ ParmName="UserId", ParmValue= receiverUserId + "," +  application.ProjectSites.UserRefId, isNumber=false }
                        };
                        var user = await _iGeneric_SP_Repository.CallStoreProcedureReaderAsync<SenderReceiverDetailViewModal>("dbo.sp_GetSenderReceiverDetail", storeProcedureParms);

                        string clearanceFile = "NA";
                        string receiverDesignation = user.Where(x => x.UserId == receiverUserId).Select(x => x.NormalizedName).FirstOrDefault();


                        if (applicationType == ApplicationTypeEnum.BP_DECLARATION_STABILITY_CERTIFICATE)
                        {
                            var userClaims = await _iAuthService.DecryptLoggedInUserClaims(_iAccessor.HttpContext.User.Claims);
                            using (var scope = _iServiceScopeFactory.CreateScope())
                            {
                                try
                                {

                                    var _iPdfOprations = scope.ServiceProvider.GetRequiredService<IPdfOprationsService>();
                                    CertificateGenerateServiceResultTemplate certResp = await _iPdfOprations.GenerateCertificate(AppId, ApplicationTypeEnum.BP_DECLARATION_STABILITY_CERTIFICATE, userClaims.UserName, false);
                                    var licenseDirName = "AppForm_BP_DECLARATION_STABILITY_CERTIFICATE";
                                    clearanceFile = Configuration.GetSection("ThirdPartyIntegrationConfigs").GetSection("WebHostRootUrl").Value + "License/" + licenseDirName + "/" + application.PublicAppRefNum + ".pdf";
                                    receiverDesignation = "Director of Factories, Punjab";
                                }
                                catch (Exception ex)

                                {

                                }

                            }


                            
                        }

                        InvestPunjabShareStatusParmsViewModel statusReq = new InvestPunjabShareStatusParmsViewModel();
                        statusReq.AppRefId = AppId;
                        statusReq.ApplicationType = applicationType;
                        statusReq.IPin = Convert.ToInt64(application.InvestPunjab_Ipin);
                        statusReq.InvestPunjab_AppId = application.InvestPunjab_AppId;
                        //statusReq.StatusId = (formModel.AppActionType == 404 || formModel.AppActionType == 102) ? 9 : formModel.AppActionType == 200 ? 5 : formModel.AppActionType == 500 ? 15 : formModel.AppActionType;
                        statusReq.StatusId = investPunjabStatusCode;
                        statusReq.StatusDesc = statusDescription.ActionName;
                        statusReq.Comments = statusDescription.ActionName;
                        statusReq.SenderName = user.Where(x => x.UserId == application.ProjectSites.UserRefId).Select(x => x.FirstName).FirstOrDefault() + ' ' + user.Where(x => x.UserId == application.ProjectSites.UserRefId).Select(x => x.LastName).FirstOrDefault();
                        statusReq.SenderDesignation = user.Where(x => x.UserId == application.ProjectSites.UserRefId).Select(x => x.NormalizedName).FirstOrDefault();
                        statusReq.ReceiverName = user.Where(x => x.UserId == receiverUserId).Select(x => x.FirstName).FirstOrDefault() + ' ' + user.Where(x => x.UserId == receiverUserId).Select(x => x.LastName).FirstOrDefault();
                        statusReq.ReceiverDesignation = receiverDesignation;
                        statusReq.ClearanceIssuedOn = DateTime.Now;
                        statusReq.ClearanceExpiredOn = new DateTime(DateTime.Now.Year, 12, 31);
                        statusReq.LicenseNo = licenceInfo == null ? "NA" : licenceInfo.LicenceNumber.ToString();
                        statusReq.ClearanceFile = clearanceFile;
                        statusReq.StatusDate = DateTime.Now;
                        statusReq.IntegrationSource = "LABOUR";
                        statusReq.IsdeemedApproval = "false";
                        statusReq.appActionLogId = appActionLogId;
                        statusReq.appActionType = (AppActionTypeEnum) AppActionType;
                        var response = await _iInvestPunjabShareStatusService.ShareStatusToBusinessFirst(statusReq);
                    }
                }

            }
            return receiverUserId;
        }

        public async Task<bool> DetermineAreAppDocumentsUploaded(Int64 appId, ApplicationTypeEnum applicationType)
        {
            bool areAllMandatoryUploaded = true;
            List<AppFileUploadInfoViewModel> documentsInfo = await InitiateAppFileInfo(appId);
            if (documentsInfo.Count > 0)
            {
                foreach (var item in documentsInfo)
                {
                    if (!item.IsOptional && (item.AlreadyUploadedInfo == null || item.AlreadyUploadedInfo.Count == 0))
                    {
                        areAllMandatoryUploaded = false;
                    }
                }
            }
            else
            {
                areAllMandatoryUploaded = false;
            }
            return areAllMandatoryUploaded;
        }

        public async Task<List<AppFileUploadInfoViewModel>> InitiateAppFileInfo(long AppId)
        {
            List<AppFileUploadInfoViewModel> filesDetail = new List<AppFileUploadInfoViewModel>();
            try
            {
                List<StoreProcedureParm> storeProcedureParms = new List<StoreProcedureParm>()
                {
                    new StoreProcedureParm (){ ParmName="AppId", ParmValue= AppId.ToString(), isNumber=true }
                };
                //filesDetail = await _iGeneric_SP_Repository.CallStoreProcedureReaderAsync<AppFileUploadInfoViewModel>("dbo.sp_Get_AppType_Allowed_File_Files", storeProcedureParms);
                filesDetail = await _iGeneric_SP_Repository.CallStoreProcedureReaderAsync<AppFileUploadInfoViewModel>("dbo.sp_Get_AppType_Allowed_File_Files", storeProcedureParms);

                if (filesDetail.Count() > 0)
                {
                    foreach (var file in filesDetail)
                    {
                        if (file.AlreadyUploaded != null)
                        {
                            string[] alreadyUploadedFileInfo = file.AlreadyUploaded.Split(',');
                            if (alreadyUploadedFileInfo.Length > 0)
                            {
                                file.AlreadyUploadedInfo = new List<AppFileAlreadyFileUploadedViewModel>();
                                foreach (var alreadyFile in alreadyUploadedFileInfo)
                                {
                                    string[] fileInfo = alreadyFile.Split('=');
                                    file.AlreadyUploadedInfo.Add(new AppFileAlreadyFileUploadedViewModel()
                                    {
                                        FileName = fileInfo[0],
                                        FileUploadOn = Convert.ToDateTime(fileInfo[1]),
                                        IsLocked = Convert.ToBoolean(Convert.ToInt16(fileInfo[2]))
                                    });
                                }
                            }
                        }
                    }
                }
                if (AppId > 0)
                {
                    var application = _context.Applications.Where(x => x.AppId == AppId && x.IsDeleted == false).Include(x=>x.ProjectSites).FirstOrDefault();
                    if (application.ApplicationType == ApplicationTypeEnum.BUILDING_PLAN_HUD)
                    {
                        var buildingPlanHud = _context.BuildingPlanHUD_GeneralDetails.Where(x => x.AppRefId == AppId).FirstOrDefault();
                        if (buildingPlanHud.IsUnderRightToBusinessAct == "1")
                        {
                            filesDetail.Where(x => x.DocumentId == 30).Select(x => { x.IsOptional = false; return x; }).ToList();
                        }
                        if (buildingPlanHud.IsBuildingHeightMoreThen15Meter == "1")
                        {
                            filesDetail.Where(x => x.DocumentId == 38).Select(x => { x.IsOptional = false; return x; }).ToList();
                        }
                        if (buildingPlanHud.IsGasOrFuelPipeLinePassWithin150Meter == "1")
                        {
                            filesDetail.Where(x => x.DocumentId == 30005).Select(x => { x.IsOptional = false; return x; }).ToList();
                        }
                        if (buildingPlanHud.IsHistoricalSiteIsLocatedWithin100Meter == "1")
                        {
                            filesDetail.Where(x => x.DocumentId == 30007).Select(x => { x.IsOptional = false; return x; }).ToList();
                        }

                        if (application.ProjectSites.DistrictRefId==608)
                        {
                            filesDetail.Where(x => x.DocumentId == 30010).Select(x => { x.IsOptional = false; return x; }).ToList();
                        }
                    }

                    else if (application.ApplicationType == ApplicationTypeEnum.FACTORY_LICENCE && application.ApplicationPurposeType == ApplicationPurposeTypeEnum.GRANT_LICENCE)
                    {
                        var factoryLicence = _context.Licence_Factory_GeneralDetails.Where(x => x.AppRefId == AppId).FirstOrDefault();
                        if (factoryLicence.IsStabilityApproved == 0) //From competent person - DocId= 32 - Form B (Structural Stability Certificate)
                        {
                            filesDetail.Where(x => x.DocumentId == 32).Select(x => { x.IsOptional = false; return x; }).ToList();
                        }
                        if (factoryLicence.IsBuildingPlanApproved == 0) // Building Plan From Another authority - DocId = 50017
                        {
                            filesDetail.Where(x => x.DocumentId == 50017).Select(x => { x.IsOptional = false; return x; }).ToList();
                        }
                        if (factoryLicence.HaveYouMadeChangesInBuildingPlan == 0) // Building Plan From Another authority - DocId = 50018
                        {
                            filesDetail.Where(x => x.DocumentId == 50018).Select(x => { x.IsOptional = false; return x; }).ToList();
                        }
                    }

                    else if (application.ApplicationType == ApplicationTypeEnum.BP_DECLARATION_STABILITY_CERTIFICATE)
                    {
                        var declarationStabilityCertificates = _context.BuildingPlanFactory_Declaration_Stability_Certificates.Where(x => x.AppRefId == AppId).FirstOrDefault();
                        if (declarationStabilityCertificates.IsCompetentPersonSubmittedAnyChanges == "1") // Undertaking given by employer 
                        {
                            filesDetail.Where(x => x.DocumentId == 52).Select(x => { x.IsOptional = false; return x; }).ToList();
                        }
                    }
                }

                filesDetail = filesDetail.OrderBy(x => x.IsOptional).ToList();
            }
            catch (Exception ex)
            {

            }
            return filesDetail;
        }

        public async Task<List<AppFileUploadInfoViewModel>> InitiateAppAddendumFileInfo(long AppId)
        {
            List<AppFileUploadInfoViewModel> filesDetail = new List<AppFileUploadInfoViewModel>();
            try
            {
                List<StoreProcedureParm> storeProcedureParms = new List<StoreProcedureParm>()
                {
                    new StoreProcedureParm (){ ParmName="AppId", ParmValue= AppId.ToString(), isNumber=true }
                };
                filesDetail = await _iGeneric_SP_Repository.CallStoreProcedureReaderAsync<AppFileUploadInfoViewModel>("dbo.sp_Get_AppType_Allowed_File_Files", storeProcedureParms);

                if (filesDetail.Count() > 0)
                {
                    foreach (var file in filesDetail)
                    {
                        if (file.AlreadyUploaded != null)
                        {
                            string[] alreadyUploadedFileInfo = file.AlreadyUploaded.Split(',');
                            if (alreadyUploadedFileInfo.Length > 0)
                            {
                                file.AlreadyUploadedInfo = new List<AppFileAlreadyFileUploadedViewModel>();
                                foreach (var alreadyFile in alreadyUploadedFileInfo)
                                {
                                    string[] fileInfo = alreadyFile.Split('=');
                                    file.AlreadyUploadedInfo.Add(new AppFileAlreadyFileUploadedViewModel()
                                    {
                                        FileName = fileInfo[0],
                                        FileUploadOn = Convert.ToDateTime(fileInfo[1]),
                                        IsLocked = Convert.ToBoolean(Convert.ToInt16(fileInfo[2]))
                                    });
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {

            }
            return filesDetail;
        }

        //************* W A R N I N G **************
        //************* W A R N I N G **************
        //************* W A R N I N G **************
        //************* W A R N I N G **************
        //************* W A R N I N G **************
        // ******** API IS CALLING FROM DEEMED PROCESS ********


        #region RecordApplicationAction Backup - 19-Aug-2025
        //public async Task<GenericResponseTemplateModel<RecordActionResponseViewModel>> RecordApplicationAction(ApplicationActionViewModel formModel, string userId)
        //{
        //    GenericResponseTemplateModel<RecordActionResponseViewModel> resp = new GenericResponseTemplateModel<RecordActionResponseViewModel>();
        //    try
        //    {
        //        var applicationAction = await _context.ApplicationActions.Where(x => x.ApplicationRefId == formModel.AppRefId).FirstOrDefaultAsync();
        //        var app = await _context.Applications.Where(x => x.AppId == formModel.AppRefId).FirstOrDefaultAsync();
        //        List<StoreProcedureParm> storeProcedureParms = new List<StoreProcedureParm>()
        //        {
        //            new StoreProcedureParm (){ ParmName="UserId", ParmValue= formModel.Receiver_UserRefId.Replace("_",",") + "," + userId, isNumber=false }
        //        };
        //        var user = await _iGeneric_SP_Repository.CallStoreProcedureReaderAsync<SenderReceiverDetailViewModal>("dbo.sp_GetSenderReceiverDetail", storeProcedureParms);

        //        CheckListDataViewModel ChecklistData = null;
        //        if (formModel.CheckListFormJson != null && formModel.CheckListFormJson.Length > 0)
        //        {
        //            ChecklistData = JsonConvert.DeserializeObject<CheckListDataViewModel>(formModel.CheckListFormJson);
        //        }

        //        int totalDays = GetWorkingDays(applicationAction.ActionDate, DateTimeOffset.UtcNow.Date);
        //        int holidays = _context.Holidays.Where(x => x.HolidayDate >= applicationAction.ActionDate && x.HolidayDate <= DateTimeOffset.UtcNow.Date && x.IsWeekend == false).Count();
        //        int totalWorkingDays = 0;
        //        if (holidays > 0)
        //        {
        //            totalWorkingDays = (totalDays - holidays);
        //        }
        //        else
        //        {
        //            totalWorkingDays = totalDays;
        //        }

        //        var end = DateTime.Now;
        //        var start = applicationAction.ActionDate;
        //        var weekend = new[] { DayOfWeek.Saturday, DayOfWeek.Sunday };

        //        var totalWorkingHours = Enumerable.Range(0, Convert.ToInt32(end.Subtract(start).TotalHours))
        //             .Count(offset => !weekend.Contains(start.AddHours(offset).DayOfWeek));

        //        if (applicationAction != null)
        //        {
        //            applicationAction.ActionTakenDaysCount = totalWorkingDays;
        //            applicationAction.ActionTakenHoursCount = Convert.ToInt64(totalWorkingHours);
        //            applicationAction.AppActionType = formModel.AppActionType;
        //            applicationAction.ActionDate = DateTime.Now;

        //            var appActionExt = new ApplicationActionExtension();
        //            if (app.ApplicationType == ApplicationTypeEnum.BUILDING_PLAN_PSIEC)
        //            {
        //                List<ReceiverBackupViewModel> receiverBackups = new List<ReceiverBackupViewModel>()
        //                { new ReceiverBackupViewModel()
        //                    {
        //                        Receiver_ProfileRefId = applicationAction.Receiver_ProfileRefId,
        //                        Receiver_UserRefId = applicationAction.Receiver_UserRefId,
        //                        ReceiverRoleId = applicationAction.ReceiverRoleId,
        //                        Receiver_ActionOn = applicationAction.ActionDate,
        //                        Receiver_Remarks = applicationAction.Remarks,
        //                        AppActionType = (AppActionTypeEnum) applicationAction.AppActionType,
        //                        SenderRoleId = applicationAction.SenderRoleId,
        //                        Sender_ProfileRefId = applicationAction.Sender_ProfileRefId,
        //                        Sender_UserRefId = applicationAction.Sender_UserRefId,
        //                        ActionTakenModeType = applicationAction.ActionTakenModeType,
        //                        IsCurrentReceiver = applicationAction.Receiver_UserRefId != formModel.UserId,
        //                        ReceiverRepeatedCount = 1
        //                    }
        //                };

        //                applicationAction.ActionDate = DateTime.Now;

        //                appActionExt = _context.ApplicationActionExtensions.Where(x => x.AppActionRefId == applicationAction.AppActionId).FirstOrDefault();
        //                if (appActionExt != null)
        //                {
        //                    int i = 1;
        //                    foreach (var item in typeof(ApplicationActionExtension).GetProperties().Where(x => x.Name.Contains("Receiver_UserRefId_")))
        //                    {

        //                        var extReceiver = new ReceiverBackupViewModel()
        //                        {
        //                            Receiver_ProfileRefId = Convert.ToInt64(appActionExt.GetType().GetProperty("Receiver_ProfileRefId_" + i.ToString()).GetValue(appActionExt)),
        //                            Receiver_UserRefId = Convert.ToString(appActionExt.GetType().GetProperty("Receiver_UserRefId_" + i.ToString()).GetValue(appActionExt)),
        //                            ReceiverRoleId = Convert.ToString(appActionExt.GetType().GetProperty("ReceiverRoleId_" + i.ToString()).GetValue(appActionExt)),
        //                            Receiver_ActionOn = Convert.ToDateTime(appActionExt.GetType().GetProperty("Receiver_ActionOn_" + i.ToString()).GetValue(appActionExt)),
        //                            Receiver_Remarks = Convert.ToString(appActionExt.GetType().GetProperty("Receiver_Remarks_" + i.ToString()).GetValue(appActionExt)),

        //                            AppActionType = (AppActionTypeEnum)appActionExt.GetType().GetProperty("AppActionType_" + i.ToString()).GetValue(appActionExt),
        //                            SenderRoleId = Convert.ToString(appActionExt.GetType().GetProperty("SenderRoleId_" + i.ToString()).GetValue(appActionExt)),
        //                            Sender_ProfileRefId = Convert.ToInt64(appActionExt.GetType().GetProperty("Sender_ProfileRefId_" + i.ToString()).GetValue(appActionExt)),
        //                            Sender_UserRefId = Convert.ToString(appActionExt.GetType().GetProperty("Sender_UserRefId_" + i.ToString()).GetValue(appActionExt)),

        //                            ActionTakenModeType = (ActionTakenModeTypeEnum)(appActionExt.GetType().GetProperty("ActionTakenModeType_" + i.ToString()).GetValue(appActionExt)),


        //                            //AppActionLogRefId = Convert.ToInt64(appActionExt.GetType().GetProperty("AppActionLogRefId_" + i.ToString()).GetValue(appActionExt)),
        //                            ActionTakenHoursCount = Convert.ToInt64(appActionExt.GetType().GetProperty("ActionTakenHoursCount_" + i.ToString()).GetValue(appActionExt)),
        //                            ActionTakenDaysCount = Convert.ToInt64(appActionExt.GetType().GetProperty("ActionTakenDaysCount_" + i.ToString()).GetValue(appActionExt)),
        //                            IsDocumentUploaded = Convert.ToBoolean(appActionExt.GetType().GetProperty("IsDocumentUploaded_" + i.ToString()).GetValue(appActionExt)),
        //                            AppDocumentRefId = Convert.ToInt64(appActionExt.GetType().GetProperty("AppDocumentRefId_" + i.ToString()).GetValue(appActionExt)),

        //                            //IsCurrentReceiver = applicationAction.Receiver_UserRefId != formModel.UserId
        //                        };
        //                        extReceiver.IsCurrentReceiver = extReceiver.Receiver_UserRefId != formModel.UserId;

        //                        extReceiver.ReceiverRepeatedCount = receiverBackups.Count(x => x.Receiver_UserRefId == Convert.ToString(appActionExt.GetType().GetProperty("Receiver_UserRefId_" + i.ToString()).GetValue(appActionExt))) + 1;

        //                        receiverBackups.Add(extReceiver);
        //                        i++;
        //                    }
        //                }


        //                var splitedUserIds = formModel.Receiver_UserRefId.Split("_");
        //                foreach (var item in splitedUserIds)
        //                {
        //                    receiverBackups.Add(
        //                        new ReceiverBackupViewModel()
        //                        {
        //                            Receiver_ProfileRefId = user.Where(x => x.UserId == item).Select(x => x.UserProfileId).FirstOrDefault(),
        //                            Receiver_UserRefId = item,
        //                            ReceiverRoleId = user.Where(x => x.UserId == item).Select(x => x.RoleId).FirstOrDefault(),
        //                            Receiver_ActionOn = applicationAction.ActionDate,
        //                            Receiver_Remarks = formModel.Remarks,

        //                            AppActionType = (AppActionTypeEnum)formModel.AppActionType,
        //                            Sender_ProfileRefId = user.Where(x => x.UserId == userId).Select(x => x.UserProfileId).FirstOrDefault(),
        //                            Sender_UserRefId = userId,
        //                            SenderRoleId = user.Where(x => x.UserId.ToLower() == userId.ToLower()).Select(x => x.RoleId).FirstOrDefault(),
        //                            ActionTakenModeType = formModel.ActionTakenModeType,
        //                            IsCurrentReceiver = true,
        //                            ReceiverRepeatedCount = receiverBackups.Count(x => x.Receiver_UserRefId == item) + 1
        //                        });
        //                }

        //                // var kk = JsonConvert.SerializeObject(receiverBackups);
        //                //receiverBackups = receiverBackups.Where(x => (x.Receiver_UserRefId != formModel.UserId)).ToList();
        //                receiverBackups = receiverBackups.Where(x => (x.Receiver_UserRefId != null)).ToList();
        //                receiverBackups = receiverBackups.Where(x => (x.Receiver_UserRefId != String.Empty)).ToList();
        //                receiverBackups = receiverBackups.Where(x => (x.IsCurrentReceiver)).ToList();

        //                List<ReceiverBackupViewModel> receiverBackups_Filtered = new List<ReceiverBackupViewModel>();

        //                foreach (var item in receiverBackups)
        //                {
        //                    if (receiverBackups_Filtered.Count(x => x.Receiver_UserRefId == item.Receiver_UserRefId) == 0)
        //                    {
        //                        int maxReceiverRepeatedCount = receiverBackups.Where(x => x.Receiver_UserRefId == item.Receiver_UserRefId).Select(x => x.ReceiverRepeatedCount).Max();
        //                        receiverBackups_Filtered.Add(receiverBackups.Where(x => x.Receiver_UserRefId == item.Receiver_UserRefId && x.ReceiverRepeatedCount == maxReceiverRepeatedCount).FirstOrDefault());
        //                    }
        //                }


        //                receiverBackups = receiverBackups_Filtered;


        //                applicationAction.Receiver_ProfileRefId = receiverBackups.FirstOrDefault().Receiver_ProfileRefId;
        //                applicationAction.Receiver_UserRefId = receiverBackups.FirstOrDefault().Receiver_UserRefId;
        //                applicationAction.ReceiverRoleId = receiverBackups.FirstOrDefault().ReceiverRoleId;
        //                applicationAction.ActionDate = Convert.ToDateTime(receiverBackups.FirstOrDefault().Receiver_ActionOn);
        //                applicationAction.Remarks = receiverBackups.FirstOrDefault().Receiver_Remarks;

        //                applicationAction.Sender_ProfileRefId = receiverBackups.FirstOrDefault().Sender_ProfileRefId;
        //                applicationAction.Sender_UserRefId = receiverBackups.FirstOrDefault().Sender_UserRefId;
        //                applicationAction.SenderRoleId = receiverBackups.FirstOrDefault().SenderRoleId;
        //                applicationAction.AppActionType = (int)receiverBackups.FirstOrDefault().AppActionType;


        //                receiverBackups.RemoveAt(0);

        //                ApplicationActionExtension updatedAppActionExt = new ApplicationActionExtension();
        //                int j = 0;
        //                foreach (var item in receiverBackups)
        //                {
        //                    j++;
        //                    updatedAppActionExt.GetType().GetProperty("Receiver_UserRefId_" + j.ToString()).SetValue(updatedAppActionExt, item.Receiver_UserRefId);
        //                    updatedAppActionExt.GetType().GetProperty("Receiver_ProfileRefId_" + j.ToString()).SetValue(updatedAppActionExt, item.Receiver_ProfileRefId);
        //                    updatedAppActionExt.GetType().GetProperty("ReceiverRoleId_" + j.ToString()).SetValue(updatedAppActionExt, item.ReceiverRoleId);
        //                    updatedAppActionExt.GetType().GetProperty("Receiver_ActionOn_" + j.ToString()).SetValue(updatedAppActionExt, item.Receiver_ActionOn);
        //                    updatedAppActionExt.GetType().GetProperty("Receiver_Remarks_" + j.ToString()).SetValue(updatedAppActionExt, item.Receiver_Remarks);


        //                    updatedAppActionExt.GetType().GetProperty("Sender_ProfileRefId_" + j.ToString()).SetValue(updatedAppActionExt, item.Sender_ProfileRefId);
        //                    updatedAppActionExt.GetType().GetProperty("Sender_UserRefId_" + j.ToString()).SetValue(updatedAppActionExt, item.Sender_UserRefId);
        //                    updatedAppActionExt.GetType().GetProperty("SenderRoleId_" + j.ToString()).SetValue(updatedAppActionExt, item.SenderRoleId);
        //                    updatedAppActionExt.GetType().GetProperty("AppActionType_" + j.ToString()).SetValue(updatedAppActionExt, item.AppActionType);


        //                    updatedAppActionExt.GetType().GetProperty("ActionTakenModeType_" + j.ToString()).SetValue(updatedAppActionExt, item.ActionTakenModeType);

        //                    //updatedAppActionExt.GetType().GetProperty("AppActionLogRefId_" + j.ToString()).SetValue(updatedAppActionExt, item.AppActionLogRefId);
        //                    updatedAppActionExt.GetType().GetProperty("ActionTakenHoursCount_" + j.ToString()).SetValue(updatedAppActionExt, item.ActionTakenHoursCount);
        //                    updatedAppActionExt.GetType().GetProperty("ActionTakenDaysCount_" + j.ToString()).SetValue(updatedAppActionExt, item.ActionTakenDaysCount);
        //                    updatedAppActionExt.GetType().GetProperty("IsDocumentUploaded_" + j.ToString()).SetValue(updatedAppActionExt, item.IsDocumentUploaded);
        //                    updatedAppActionExt.GetType().GetProperty("AppDocumentRefId_" + j.ToString()).SetValue(updatedAppActionExt, item.AppDocumentRefId);
        //                }

        //                if (appActionExt != null)
        //                {
        //                    _context.ApplicationActionExtensions.RemoveRange(appActionExt);
        //                    _context.SaveChanges();
        //                }


        //                if (j > 0)
        //                {
        //                    updatedAppActionExt.AppActionRefId = applicationAction.AppActionId;
        //                    updatedAppActionExt.CurrentActionTakenUserRefId = formModel.UserId;
        //                    updatedAppActionExt.CurrentActionTakenCode = (AppActionTypeEnum)formModel.AppActionType;
        //                    await _context.ApplicationActionExtensions.AddAsync(updatedAppActionExt);
        //                    await _context.SaveChangesAsync();
        //                }
        //            }



        //            //int i = 0;
        //            //var splitedUserIds = formModel.Receiver_UserRefId.Split("_");
        //            //bool hasSenderEliminated = false;
        //            //foreach (var item in splitedUserIds) 
        //            //{
        //            //    //List<StoreProcedureParm> storeProcedureParms = new List<StoreProcedureParm>()
        //            //    //{
        //            //    //    new StoreProcedureParm (){ ParmName="UserId", ParmValue= item + "," + userId, isNumber=false }
        //            //    //};
        //            //    //var user = await _iGeneric_SP_Repository.CallStoreProcedureReaderAsync<SenderReceiverDetailViewModal>("dbo.sp_GetSenderReceiverDetail", storeProcedureParms);
        //            //    ReceiverBackupViewModel receiverBackup = new ReceiverBackupViewModel();  

        //            //    if (i == 0) 
        //            //    {
        //            //        receiverBackup.Receiver_ProfileRefId = applicationAction.Receiver_ProfileRefId;
        //            //        receiverBackup.Receiver_UserRefId = applicationAction.Receiver_UserRefId;
        //            //        receiverBackup.ReceiverRoleId = applicationAction.ReceiverRoleId;

        //            //        applicationAction.Receiver_ProfileRefId = user.Where(x => x.UserId == item).Select(x => x.UserProfileId).FirstOrDefault();  //user.Where(x => x.UserId == formModel.UserId).Select(x => x.UserProfileId).FirstOrDefault();
        //            //        applicationAction.Receiver_UserRefId = item;
        //            //        applicationAction.ReceiverRoleId = user.Where(x => x.UserId == item).Select(x => x.RoleId).FirstOrDefault();
        //            //    }
        //            //    //else
        //            //    //{
        //            //        var appActionExt = _context.ApplicationActionExtensions.Where(x=>x.AppActionRefId == applicationAction.AppActionId).FirstOrDefault();
        //            //        if(appActionExt == null && splitedUserIds.Length>1)
        //            //        {
        //            //            appActionExt = new ApplicationActionExtension()
        //            //            {
        //            //                Receiver_ProfileRefId_1 = user.Where(x => x.UserId == item).Select(x => x.UserProfileId).FirstOrDefault(), 
        //            //                Receiver_UserRefId_1 = item,
        //            //                ReceiverRoleId_1 = user.Where(x => x.UserId == item).Select(x => x.RoleId).FirstOrDefault(),
        //            //                AppActionRefId = applicationAction.AppActionId
        //            //            };
        //            //            await _context.ApplicationActionExtensions.AddAsync(appActionExt);
        //            //            await _context.SaveChangesAsync();
        //            //        }




        //            //        else
        //            //        {
        //            //            if (i < 2 && !hasSenderEliminated)
        //            //            {
        //            //                for (int k = 0; k < splitedUserIds.Length-1; k++)
        //            //                {
        //            //                    if (appActionExt.GetType().GetProperty("Receiver_UserRefId_" + i.ToString()).GetValue(appActionExt).ToString() == formModel.UserId)
        //            //                    {
        //            //                        if (receiverBackup.Receiver_UserRefId == formModel.UserId)
        //            //                        {
        //            //                            appActionExt.GetType().GetProperty("Receiver_UserRefId_" + i.ToString()).SetValue(appActionExt, null);
        //            //                            appActionExt.GetType().GetProperty("Receiver_ProfileRefId_" + i.ToString()).SetValue(appActionExt, 0);
        //            //                            appActionExt.GetType().GetProperty("ReceiverRoleId" + i.ToString()).SetValue(appActionExt, null);
        //            //                        }
        //            //                        else
        //            //                        {
        //            //                            appActionExt.GetType().GetProperty("Receiver_UserRefId_" + i.ToString()).SetValue(appActionExt, receiverBackup.Receiver_UserRefId);
        //            //                            appActionExt.GetType().GetProperty("Receiver_ProfileRefId_" + i.ToString()).SetValue(appActionExt, receiverBackup.Receiver_ProfileRefId);
        //            //                            appActionExt.GetType().GetProperty("ReceiverRoleId" + i.ToString()).SetValue(appActionExt, receiverBackup.ReceiverRoleId);
        //            //                        }
        //            //                    }
        //            //                }
        //            //                hasSenderEliminated = true;
        //            //            }


        //            //            if (appActionExt.GetType().GetProperty("Receiver_UserRefId_" + i.ToString()).GetValue(appActionExt) == null)
        //            //            {
        //            //                appActionExt.GetType().GetProperty("Receiver_UserRefId_" + i.ToString()).SetValue(appActionExt, item);
        //            //                appActionExt.GetType().GetProperty("Receiver_ProfileRefId_" + i.ToString()).SetValue(appActionExt, user.Where(x => x.UserId == item).Select(x => x.UserProfileId).FirstOrDefault());
        //            //                appActionExt.GetType().GetProperty("ReceiverRoleId" + i.ToString()).SetValue(appActionExt, user.Where(x => x.UserId == item).Select(x => x.RoleId).FirstOrDefault());
        //            //            }

        //            //            _context.Update<ApplicationActionExtension>(appActionExt);
        //            //            _context.SaveChanges();
        //            //        }
        //            //    //}
        //            //    i++;
        //            //}


        //            //applicationAction.Sender_ProfileRefId = user.Where(x => x.UserId == userId).Select(x => x.UserProfileId).FirstOrDefault();
        //            //applicationAction.Sender_UserRefId = userId;
        //            //applicationAction.SenderRoleId = user.Where(x => x.UserId.ToLower() == userId.ToLower()).Select(x => x.RoleId).FirstOrDefault();

        //            //applicationAction.Remarks = formModel.Remarks;
        //            applicationAction.IsDocumentUploaded = formModel.IsDocumentUploaded;
        //            applicationAction.AppDocumentRefId = formModel.AppDocumentRefId;
        //            applicationAction.Checklist_Json = formModel.CheckListFormJson;
                    
        //            if (ChecklistData != null)
        //            {
        //                applicationAction.Checklist_IsAllAgreed = !ChecklistData.CheckListNodes.Exists(x => x.IsVerified == false);
        //                applicationAction.Checklist_FieldObjections = ChecklistData.CheckListNodes.Where(x => !x.IsVerified && !x.IsDocument).Count();
        //                applicationAction.Checklist_DocObjections = ChecklistData.CheckListNodes.Where(x => !x.IsVerified && x.IsDocument).Count();
        //            }
        //            else
        //            {
        //                applicationAction.Checklist_IsAllAgreed = true;
        //                applicationAction.Checklist_FieldObjections = 0;
        //                applicationAction.Checklist_DocObjections = 0;
        //            }
        //            if (formModel.AppActionType == 206)
        //            {
        //                applicationAction.IpAddress = "Deemed";
        //                applicationAction.Latitude = "Deemed";
        //                applicationAction.Longitude = "Deemed";
        //            }
        //            else if (formModel.AppActionType == 208)
        //            {
        //                applicationAction.IpAddress = "Auto Approve";
        //                applicationAction.Latitude = "Auto Approve";
        //                applicationAction.Longitude = "Auto Approve";
        //            }
        //            else if(formModel.AppActionType == 411)
        //            {
        //                applicationAction.IpAddress = "Dormant";
        //                applicationAction.Latitude = "Dormant";
        //                applicationAction.Longitude = "Dormant";
        //            }
        //            else if (formModel.UserId == "C79F57CB-40BA-48EA-8336-327DC1B78701")
        //            {
        //                applicationAction.IpAddress = "Auto Action";
        //                applicationAction.Latitude = "Auto Action";
        //                applicationAction.Longitude = "Auto Action";
        //            }
        //            else
        //            {
        //                if (formModel.IpAddress == null)
        //                {
        //                    applicationAction.IpAddress = "Auto Action";
        //                    applicationAction.Latitude = "Auto Action";
        //                    applicationAction.Longitude = "Auto Action";
        //                }
        //                else
        //                {
        //                    applicationAction.IpAddress = formModel.IpAddress.ToString();
        //                    applicationAction.Latitude = formModel.Latitude.ToString();
        //                    applicationAction.Longitude = formModel.Longitude.ToString();
        //                }
                            
        //            }
        //            applicationAction.IsDeleted = false;
        //            applicationAction.ActionTakenModeType = formModel.ActionTakenModeType;
        //            _iGR_ApplicationAction.Update(applicationAction);
        //            await _iGR_ApplicationAction.SavechangeAsync();
        //        }

        //        else
        //        {
        //            //_iGR_ApplicationAction.Insert(formModel);
        //            //await _iGR_ApplicationAction.SavechangeAsync();
        //        }

        //        ApplicationActionLog applicationActionlogs = new ApplicationActionLog()
        //        {
        //            ActionDate = applicationAction.ActionDate,
        //            ActionTakenDaysCount = applicationAction.ActionTakenDaysCount,
        //            ActionTakenHoursCount = applicationAction.ActionTakenHoursCount,
        //            AppActionType = applicationAction.AppActionType,

        //            Receiver_ProfileRefId = applicationAction.Receiver_ProfileRefId,
        //            Receiver_UserRefId = applicationAction.Receiver_UserRefId,

        //            Sender_ProfileRefId = applicationAction.Sender_ProfileRefId,
        //            Sender_UserRefId = applicationAction.Sender_UserRefId,
        //            Remarks = applicationAction.Remarks,
        //            ReceiverRoleId = applicationAction.ReceiverRoleId,
        //            ApplicationRefId = applicationAction.ApplicationRefId,
        //            SenderRoleId = applicationAction.SenderRoleId,

        //            Checklist_Json = applicationAction.Checklist_Json,
        //            Checklist_IsAllAgreed = applicationAction.Checklist_IsAllAgreed,
        //            Checklist_FieldObjections = applicationAction.Checklist_FieldObjections,
        //            Checklist_DocObjections = applicationAction.Checklist_DocObjections,
        //            IsDocumentUploaded = applicationAction.IsDocumentUploaded,
        //            AppDocumentRefId = applicationAction.AppDocumentRefId,
        //            ActionTakenModeType = applicationAction.ActionTakenModeType,
        //            IpAddress = applicationAction.IpAddress,
        //            Latitude = applicationAction.Latitude,
        //            Longitude = applicationAction.Longitude,
        //            IsDeleted = false
        //        };

        //        _iGR_ApplicationActionLog.Insert(applicationActionlogs);
        //        await _iGR_ApplicationActionLog.SavechangeAsync();

        //        var appActionExt1 = _context.ApplicationActionExtensions.Where(x => x.AppActionRefId == applicationAction.AppActionId).FirstOrDefault();
        //        if (appActionExt1 != null)
        //        {
        //            ApplicationActionExtensionLog appActionExtensionLog = new ApplicationActionExtensionLog();
        //            appActionExtensionLog = JsonConvert.DeserializeObject<ApplicationActionExtensionLog>(JsonConvert.SerializeObject(appActionExt1));
        //            appActionExtensionLog.CurrentAppActionLogRefId = applicationActionlogs.ApplicationActionLogId;



        //            //{
        //            //    AppActionLogRefId = applicationActionlogs.ApplicationActionLogId,
        //            //    Receiver_UserRefId_1 = appActionExt1.Receiver_UserRefId_1,
        //            //    Receiver_ProfileRefId_1 = appActionExt1.Receiver_ProfileRefId_1,
        //            //    ReceiverRoleId_1 = appActionExt1.ReceiverRoleId_1,
        //            //    Receiver_ActionOn_1 = appActionExt1.Receiver_ActionOn_1,
        //            //    Receiver_Remarks_1 = appActionExt1.Receiver_Remarks_1,
        //            //    AppActionType_1 = appActionExt1.AppActionType_1,
        //            //    SenderRoleId_1 = appActionExt1.SenderRoleId_1,
        //            //    Sender_ProfileRefId_1 = appActionExt1.Sender_ProfileRefId_1,
        //            //    Sender_UserRefId_1 = appActionExt1.Sender_UserRefId_1,


        //            //    Receiver_UserRefId_2 = appActionExt1.Receiver_UserRefId_2,
        //            //    Receiver_ProfileRefId_2 = appActionExt1.Receiver_ProfileRefId_2,
        //            //    ReceiverRoleId_2 = appActionExt1.ReceiverRoleId_2,
        //            //    Receiver_ActionOn_2 = appActionExt1.Receiver_ActionOn_2,
        //            //    Receiver_Remarks_2 = appActionExt1.Receiver_Remarks_2,
        //            //    AppActionType_2 = appActionExt1.AppActionType_2,
        //            //    SenderRoleId_2 = appActionExt1.SenderRoleId_2,
        //            //    Sender_ProfileRefId_2 = appActionExt1.Sender_ProfileRefId_2,
        //            //    Sender_UserRefId_2 = appActionExt1.Sender_UserRefId_2,

        //            //    Receiver_UserRefId_3 = appActionExt1.Receiver_UserRefId_2,
        //            //    Receiver_ProfileRefId_3 = appActionExt1.Receiver_ProfileRefId_2,
        //            //    ReceiverRoleId_2 = appActionExt1.ReceiverRoleId_2,
        //            //    Receiver_ActionOn_2 = appActionExt1.Receiver_ActionOn_2,
        //            //    Receiver_Remarks_2 = appActionExt1.Receiver_Remarks_2,
        //            //    AppActionType_2 = appActionExt1.AppActionType_2,
        //            //    SenderRoleId_2 = appActionExt1.SenderRoleId_2,
        //            //    Sender_ProfileRefId_2 = appActionExt1.Sender_ProfileRefId_2,
        //            //    Sender_UserRefId_2 = appActionExt1.Sender_UserRefId_2,

        //            //};

        //            await _context.ApplicationActionExtensionLogs.AddAsync(appActionExtensionLog);
        //            await _context.SaveChangesAsync();
        //        }
                
        //        resp.ResponseDataModel = new RecordActionResponseViewModel()
        //        {
        //            AppActionId = applicationAction.AppActionId,
        //            ApplicationActionLogId = applicationActionlogs.ApplicationActionLogId
        //        };

        //        // Update ApplicationActionLogId In BuildingPlanHUDPaymentDetail Table. 
        //        if (applicationAction.AppActionType == 401)
        //        {
        //            var paymentList = _context.BuildingPlanHUDPaymentDetails.Where(x => x.AppRefId == formModel.AppRefId).ToList();
        //            foreach (var payment in paymentList)
        //            {
        //                payment.ApplicationActionLogId = applicationActionlogs.ApplicationActionLogId;
        //                _context.SaveChanges();
        //            }
        //        }

        //        bool allOk = true;
        //        //Handle Approvals and rejections
        //        string licenseFilePath = "NA";
        //        string licenseDirName = "";
        //        var application = await _iGR_Application.GetAsync(x => x.AppId == formModel.AppRefId,null, x=>x.ProjectSites).ConfigureAwait(false);
        //        if ((formModel.AppActionType == 200 || formModel.AppActionType == 206) && application.FirstOrDefault().ApplicationType != ApplicationTypeEnum.BUILDING_PLAN_HUD)//Approved
        //        {
        //            //Application application
        //            if (formModel.AppActionType == 206)
        //            {
        //                allOk = await _iApplicationMamnagementRepository.SwitchApplicationLifeCycleType(formModel.AppRefId, ApplicationLifeCycleStatusTypeEnum.DEEMED_COMPLETED);
        //            }
        //            else if (formModel.AppActionType == 208)
        //            {
        //                allOk = await _iApplicationMamnagementRepository.SwitchApplicationLifeCycleType(formModel.AppRefId, ApplicationLifeCycleStatusTypeEnum.AUTO_APPROVEL_COMPLETED);
        //            }
        //            else
        //            {
        //                allOk = await _iApplicationMamnagementRepository.SwitchApplicationLifeCycleType(formModel.AppRefId, ApplicationLifeCycleStatusTypeEnum.APPROVED);
        //            }


        //            //GenerateLicenceNoViewModel licenceNo = await _iApplicationMamnagementRepository.GenerateLicenceNo(formModel.AppRefId, application.FirstOrDefault().ApplicationType);

        //            var tempFilePath = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "TempFiles")).Root;

        //            if (File.Exists(tempFilePath + formModel.PdfNameGUID + ".pdf") && allOk)
        //            {
        //                licenseDirName = "AppForm_" + application.FirstOrDefault().ApplicationType.ToString();
        //                var licenceRootDirUrl = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "License")).Root.ToString();
        //                Directory.CreateDirectory(licenceRootDirUrl + licenseDirName);
        //                licenseFilePath = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "License", licenseDirName)).Root;

        //                formModel.PublicAppRefNum = _context.Applications.Where(x => x.AppId == formModel.AppRefId && x.IsDeleted == false).FirstOrDefault().PublicAppRefNum;

        //                if (File.Exists(licenseFilePath + formModel.PublicAppRefNum + ".pdf"))
        //                {
        //                    File.Delete(licenseFilePath + formModel.PublicAppRefNum + ".pdf");
        //                }
        //                File.Move(tempFilePath + formModel.PdfNameGUID + ".pdf", licenseFilePath + formModel.PublicAppRefNum + ".pdf");
        //            }
        //            else
        //            {
        //                allOk = false;
        //            }

        //            if (allOk && application.FirstOrDefault().ApplicationType == ApplicationTypeEnum.FACTORY_LICENCE)
        //            {
        //                Licence_Factory_AdditionalDetail additionalInfo = new Licence_Factory_AdditionalDetail()
        //                {
        //                    AppRefId = application.FirstOrDefault().AppId,
        //                    FactoryHazardousCategoryType = formModel.FactoryHazardousCategoryType,
        //                    FactorySectionCategoryType = formModel.FactorySectionCategoryType,
        //                    FactorySessionCategoryType = formModel.FactorySessionCategoryType,
        //                    LabourCircleRefId = formModel.LabourCircleRefId,
        //                    FactoryCategoryType = formModel.FactoryCategoryType
        //                };

        //                storeProcedureParms = new List<StoreProcedureParm>()
        //                {
        //                    new StoreProcedureParm (){ ParmName="AppRefId", ParmValue= additionalInfo.AppRefId.ToString(), isNumber=true},
        //                    new StoreProcedureParm (){ ParmName="FactoryHazardousCategoryType", ParmValue= ((int)additionalInfo.FactoryHazardousCategoryType).ToString(), isNumber=true},
        //                    new StoreProcedureParm (){ ParmName="FactorySectionCategoryType", ParmValue= ((int)additionalInfo.FactorySectionCategoryType).ToString(), isNumber=true },
        //                    new StoreProcedureParm (){ ParmName="FactorySessionCategoryType", ParmValue= ((int)additionalInfo.FactorySessionCategoryType).ToString(), isNumber=true},
        //                    new StoreProcedureParm (){ ParmName="LabourCircleRefId", ParmValue= additionalInfo.LabourCircleRefId.ToString(), isNumber=true},
        //                    new StoreProcedureParm (){ ParmName="FactoryCategoryType", ParmValue= ((int)additionalInfo.FactoryCategoryType).ToString(), isNumber=true}

        //                };
        //                var val = await _iGeneric_SP_Repository.CallStoreProcedureReaderScalerAsync("dbo.sp_Licence_Add_Factory_AdditionalDetails", storeProcedureParms);
        //            }

        //        }

        //        else if ((formModel.AppActionType == 402) && application.FirstOrDefault().ApplicationType != ApplicationTypeEnum.BUILDING_PLAN_HUD)//Raised Additional Fee
        //        {
        //            if (allOk && application.FirstOrDefault().ApplicationType == ApplicationTypeEnum.FACTORY_LICENCE)
        //            {
        //                decimal raisedFee = 0;
        //                if(formModel.ExistingPowerKW_Installed != formModel.PowerKW_Installed || formModel.ExistingWorkers_MaxDuringYear != formModel.Workers_MaxDuringYear)
        //                {
        //                    raisedFee = formModel.RaisedFeeAmount + 100;
        //                }
        //                else
        //                {
        //                    raisedFee = formModel.RaisedFeeAmount;
        //                }
        //                AppFeeDetail appFeeDetail = new AppFeeDetail()
        //                {
        //                    Amount = raisedFee,
        //                    AppRefId = formModel.AppRefId,
        //                    CalculatedOn = DateTime.Now,
        //                    DedicatedDDOCode = null,
        //                    DedicatedTreasurCode = null,
        //                    Description = formModel.RaisedFeeReason,
        //                    FeeHeaderRefId = 30,
        //                    HasDedicatedTreasuryCode = false,
        //                    IsDeduductible = false,
        //                    PaymentPartCounter = 1
        //                };

        //                var res = await IncreasePaymentBatchCounter(appFeeDetail.AppRefId);
        //                appFeeDetail.PaymentBatchCounter = res.ResponseDataModel;
        //                await _context.AppFeeDetails.AddAsync(appFeeDetail);
        //                await _context.SaveChangesAsync();


        //                // If any field change, insert into Factory & Amendment History Table
        //                if (formModel.ExistingWorkers_MaxDuringYear != formModel.Workers_MaxDuringYear || formModel.ExistingPowerKW_Installed != formModel.PowerKW_Installed)
        //                {
        //                    // Update Factory Details
        //                    var factoryLicence = _context.Licence_Factory_GeneralDetails.Where(x => x.AppRefId == formModel.AppRefId).FirstOrDefault();
        //                    //Licence_Factory_GeneralDetail factory_GeneralDetail = new Licence_Factory_GeneralDetail()
        //                    //{
        //                    //    Workers_MaxDuringYear = formModel.Workers_MaxDuringYear,
        //                    //    PowerKW_Installed = formModel.PowerKW_Installed
        //                    //};
        //                    factoryLicence.Workers_MaxDuringYear = formModel.Workers_MaxDuringYear;
        //                    factoryLicence.PowerKW_Installed = formModel.PowerKW_Installed;
        //                    //_iGR_Licence_Factory_GeneralDetail.Update(factory_GeneralDetail);
        //                    //await _iGR_Licence_Factory_GeneralDetail.SavechangeAsync();
        //                    _context.Licence_Factory_GeneralDetails.Update(factoryLicence);
        //                    _context.SaveChanges();

        //                    // Insert Into Log Table
        //                    Licence_Factory_AmendmentDataHistory amendmentDataHistory = new Licence_Factory_AmendmentDataHistory();
        //                    if(formModel.ExistingWorkers_MaxDuringYear != formModel.Workers_MaxDuringYear)
        //                    {
        //                        amendmentDataHistory = new Licence_Factory_AmendmentDataHistory()
        //                        {
        //                            AppRefId = formModel.AppRefId,
        //                            FieldName = "Workers_MaxDuringYear",
        //                            PreviousValue = formModel.ExistingWorkers_MaxDuringYear.ToString(),
        //                            ModifiedValue = formModel.Workers_MaxDuringYear.ToString(),
        //                            SectionCode = "MP",
        //                            ModifiedOn = DateTime.Now,
        //                            ModifiedCounter = appFeeDetail.PaymentBatchCounter
        //                        };
        //                        await _context.Licence_Factory_AmendmentDataHistories.AddAsync(amendmentDataHistory);
        //                    }
        //                    if(formModel.ExistingPowerKW_Installed != formModel.PowerKW_Installed)
        //                    {
        //                        amendmentDataHistory = new Licence_Factory_AmendmentDataHistory()
        //                        {
        //                            AppRefId = formModel.AppRefId,
        //                            FieldName = "PowerKW_Installed",
        //                            PreviousValue = formModel.ExistingPowerKW_Installed.ToString(),
        //                            ModifiedValue = formModel.PowerKW_Installed.ToString(),
        //                            SectionCode = "KW",
        //                            ModifiedOn = DateTime.Now,
        //                            ModifiedCounter = appFeeDetail.PaymentBatchCounter
        //                        };
        //                        await _context.Licence_Factory_AmendmentDataHistories.AddAsync(amendmentDataHistory);
        //                    }
        //                    await _context.SaveChangesAsync();
        //                }

        //                allOk = await _iApplicationMamnagementRepository.SwitchApplicationLifeCycleType(formModel.AppRefId, ApplicationLifeCycleStatusTypeEnum.IN_OBJECTION_WITH_BALANCE_FEE);
        //            }
        //            else if (allOk && (application.FirstOrDefault().ApplicationType == ApplicationTypeEnum.PRINCIPAL_EMPLOYER || application.FirstOrDefault().ApplicationType == ApplicationTypeEnum.ISM_PRINCIPAL_EMPLOYER))
        //            {
        //                decimal raisedFee = 0;
        //                raisedFee = formModel.RaisedFeeAmount;

        //                AppFeeDetail appFeeDetail = new AppFeeDetail()
        //                {
        //                    Amount = raisedFee,
        //                    AppRefId = formModel.AppRefId,
        //                    CalculatedOn = DateTime.Now,
        //                    DedicatedDDOCode = null,
        //                    DedicatedTreasurCode = null,
        //                    Description = formModel.RaisedFeeReason,
        //                    FeeHeaderRefId = 30,
        //                    HasDedicatedTreasuryCode = false,
        //                    IsDeduductible = false,
        //                    PaymentPartCounter = 1
        //                };

        //                var res = await IncreasePaymentBatchCounter(appFeeDetail.AppRefId);
        //                appFeeDetail.PaymentBatchCounter = res.ResponseDataModel;
        //                await _context.AppFeeDetails.AddAsync(appFeeDetail);
        //                await _context.SaveChangesAsync();

        //                allOk = await _iApplicationMamnagementRepository.SwitchApplicationLifeCycleType(formModel.AppRefId, ApplicationLifeCycleStatusTypeEnum.IN_OBJECTION_WITH_BALANCE_FEE);
        //            }
        //            else if (allOk && application.FirstOrDefault().ApplicationType == ApplicationTypeEnum.CONTRACT_LABOUR)
        //            {
        //                decimal raisedFee = formModel.RaisedFeeAmount;
        //                AppFeeDetail raisedFeeDetail = new AppFeeDetail()
        //                {
        //                    Amount = raisedFee,
        //                    AppRefId = formModel.AppRefId,
        //                    CalculatedOn = DateTime.Now,
        //                    DedicatedDDOCode = null,
        //                    DedicatedTreasurCode = null,
        //                    Description = formModel.RaisedFeeReason,
        //                    FeeHeaderRefId = 30,
        //                    HasDedicatedTreasuryCode = false,
        //                    IsDeduductible = false,
        //                    PaymentPartCounter = 1
        //                };

        //                decimal securityRaisedFee = formModel.SecurityRaisedFeeAmount;
        //                AppFeeDetail securityFeeDetail = new AppFeeDetail()
        //                {
        //                    Amount = securityRaisedFee,
        //                    AppRefId = formModel.AppRefId,
        //                    CalculatedOn = DateTime.Now,
        //                    DedicatedDDOCode = null,
        //                    DedicatedTreasurCode = null,
        //                    Description = formModel.RaisedFeeReason,
        //                    FeeHeaderRefId = 15,
        //                    HasDedicatedTreasuryCode = false,
        //                    IsDeduductible = false,
        //                    PaymentPartCounter = 1
        //                };

        //                var res = await IncreasePaymentBatchCounter(formModel.AppRefId);
        //                raisedFeeDetail.PaymentBatchCounter = res.ResponseDataModel;
        //                securityFeeDetail.PaymentBatchCounter = res.ResponseDataModel;

        //                await _context.AppFeeDetails.AddAsync(raisedFeeDetail);
        //                await _context.AppFeeDetails.AddAsync(securityFeeDetail);
        //                await _context.SaveChangesAsync();

        //                allOk = await _iApplicationMamnagementRepository.SwitchApplicationLifeCycleType(formModel.AppRefId, ApplicationLifeCycleStatusTypeEnum.IN_OBJECTION_WITH_BALANCE_FEE);
        //            }
        //        }

        //        else if ((formModel.AppActionType == 412 && application.FirstOrDefault().ApplicationType == ApplicationTypeEnum.BUILDING_PLAN_PSIEC)
        //            || (formModel.AppActionType == 102 && application.FirstOrDefault().ApplicationType == ApplicationTypeEnum.BUILDING_PLAN_PSIEC))//Raised Fee PSIEC-FactoryWing
        //        {
        //            if (allOk)
        //            {
        //                AppActionTypeEnum appActionType= AppActionTypeEnum.DEFAULT;
        //                if (formModel.AppActionType == 412)
        //                {
        //                    List<AppFeeDetail> appFeeDetail = new List<AppFeeDetail>();
        //                    appFeeDetail.Add(new AppFeeDetail()
        //                    {
        //                        Amount = formModel.PsiecCessAmount,
        //                        AppRefId = formModel.AppRefId,
        //                        CalculatedOn = DateTime.Now,
        //                        DedicatedDDOCode = null,
        //                        DedicatedTreasurCode = null,
        //                        Description = formModel.Remarks,
        //                        FeeHeaderRefId = 19,
        //                        HasDedicatedTreasuryCode = false,
        //                        IsDeduductible = false,
        //                        PaymentPartCounter = 1
        //                    });
        //                    appFeeDetail.Add(new AppFeeDetail()
        //                    {
        //                        Amount = formModel.PsiecProcessingFeeAmount,
        //                        AppRefId = formModel.AppRefId,
        //                        CalculatedOn = DateTime.Now,
        //                        DedicatedDDOCode = null,
        //                        DedicatedTreasurCode = null,
        //                        Description = formModel.Remarks,
        //                        FeeHeaderRefId = 39,
        //                        HasDedicatedTreasuryCode = false,
        //                        IsDeduductible = false,
        //                        PaymentPartCounter = 1
        //                    });

        //                    var res = await IncreasePaymentBatchCounter(formModel.AppRefId);
        //                    appFeeDetail = appFeeDetail.Select(x => { x.PaymentBatchCounter = res.ResponseDataModel; return x; }).ToList();
        //                    await _context.BulkInsertAsync<AppFeeDetail>(appFeeDetail);
        //                    appActionType = AppActionTypeEnum.FEE_RAISED;
        //                }
        //                else if (formModel.AppActionType == 102)
        //                {
        //                    appActionType = AppActionTypeEnum.FORWARD_FOR_OBJECTION;

        //                }
        //                TimeLine_DepartmentWiseFinalAction dfa = new TimeLine_DepartmentWiseFinalAction()
        //                {
        //                    AppRefId = formModel.AppRefId,
        //                    DeptCodeType = DepartmentCodeTypeEnum.LABOUR,
        //                    AppActionType = appActionType,
        //                    AppActionLogRefId = applicationActionlogs.ApplicationActionLogId
        //                };
        //                await _context.AddAsync<TimeLine_DepartmentWiseFinalAction>(dfa);
        //                await _context.SaveChangesAsync();
        //            } 
        //        }

        //        else if (application.FirstOrDefault().ApplicationType == ApplicationTypeEnum.BUILDING_PLAN_HUD && (formModel.AppActionType == 200 || formModel.AppActionType == 204)) // Approved -Building Plan Hud
        //        {
        //            allOk = await _iApplicationMamnagementRepository.SwitchApplicationLifeCycleType(formModel.AppRefId, ApplicationLifeCycleStatusTypeEnum.APPROVED);
        //            var appDocument = await _context.ApplicationDocuments.Where(x => x.AppDocId == formModel.AppDocumentRefId).FirstOrDefaultAsync();
        //            if (appDocument != null)
        //            {
        //                var tempFilePath = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "AppFiles")).Root;

        //                if (File.Exists(tempFilePath + appDocument.AttachmentName) && allOk)
        //                {
        //                    licenseDirName = "AppForm_" + application.FirstOrDefault().ApplicationType.ToString();
        //                    var licenceRootDirUrl = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "License")).Root.ToString();
        //                    Directory.CreateDirectory(licenceRootDirUrl + licenseDirName);
        //                    licenseFilePath = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "License", licenseDirName)).Root;

        //                    formModel.PublicAppRefNum = application.FirstOrDefault().PublicAppRefNum;

        //                    if (File.Exists(licenseFilePath + formModel.PublicAppRefNum + ".pdf"))
        //                    {
        //                        File.Delete(licenseFilePath + formModel.PublicAppRefNum + ".pdf");
        //                    }
        //                    File.Copy(tempFilePath + appDocument.AttachmentName, licenseFilePath + formModel.PublicAppRefNum + ".pdf");
        //                }
        //                else
        //                {
        //                    allOk = false;
        //                }

        //                if (allOk)
        //                {

        //                    var licenceNo = "DOFPB" + application.FirstOrDefault().ProjectSiteRefId.ToString("D8");

        //                    ApplicationLicenceNoMapping applicationLicenceNo = new ApplicationLicenceNoMapping()
        //                    {
        //                        AppRefId = formModel.AppRefId,
        //                        LicenceNumber = licenceNo
        //                    };

        //                    await _context.ApplicationLicenceNoMapping.AddAsync(applicationLicenceNo);
        //                    await _context.SaveChangesAsync();
        //                }
        //            }
        //        }
        //        else if (formModel.AppActionType == 201) // Rejected
        //        {
        //            allOk = await _iApplicationMamnagementRepository.SwitchApplicationLifeCycleType(formModel.AppRefId, ApplicationLifeCycleStatusTypeEnum.REJECTED);
        //            // Sent SMS Notification To User
        //            var smsTemplateText = $"Dear Applicant,Your Application No. {application.FirstOrDefault().PublicAppRefNum} is Rejected from Department of Labour-PB LBAOUR";
        //            var templateId = "1407172725588507473";
        //            await _iNotificationManagerService.InitiateNotification(application.FirstOrDefault().ProjectSites.UserRefId,
        //            new List<NotificationViewModel>()
        //            {
        //                new NotificationViewModel(){Body=smsTemplateText, NotificationMode=NotificationModeTypeEnum.MOBILE,NotificationPurpose=NotificationPurposeTypeEnum.INFO, Title=templateId}
        //            });
        //        }
        //        else if (formModel.AppActionType == 404 || formModel.AppActionType == 407) // Objection
        //        {
        //            allOk = await _iApplicationMamnagementRepository.SwitchApplicationLifeCycleType(formModel.AppRefId, ApplicationLifeCycleStatusTypeEnum.IN_OBJECTION);

        //            // Sent SMS Notification To User
        //            var smsTemplateText = $"Department of labour raised Objection on your Application No. {application.FirstOrDefault().PublicAppRefNum} . Please login at pbindustries.gov.in for resolve it.-PB  LABOUR";
        //            var templateId = "1407172725521534794";
        //            await _iNotificationManagerService.InitiateNotification(application.FirstOrDefault().ProjectSites.UserRefId,
        //            new List<NotificationViewModel>()
        //            {
        //                new NotificationViewModel(){Body=smsTemplateText, NotificationMode=NotificationModeTypeEnum.MOBILE,NotificationPurpose=NotificationPurposeTypeEnum.INFO, Title=templateId}
        //            });

        //            // Sent Email Notification 
        //            //var receiverInfo = await _iAuthService.GetUserProfileByUserId(applicationAction.Receiver_UserRefId);
        //            //var application = _context.Applications.Where(x => x.AppId == applicationAction.ApplicationRefId).FirstOrDefault();
        //            //var projectSite = _context.ProjectSites.Where(x => x.ProjectSiteId == application.ProjectSiteRefId).FirstOrDefault();
        //            //TemplateResolveObjectionCaseLendedToOfficerNotificationViewModel mailTemplate = new TemplateResolveObjectionCaseLendedToOfficerNotificationViewModel()
        //            //{
        //            //    OfficerName = receiverInfo.FirstName + " " + receiverInfo.LastName,
        //            //    ApplicationPurposeTitle = EnumOps.GetEnumDescriptionName<ApplicationPurposeTypeEnum>(application.ApplicationPurposeType.ToString()),
        //            //    ApplicationTypeTitle = EnumOps.GetEnumDescriptionName<ApplicationTypeEnum>(application.ApplicationType.ToString()),
        //            //    EstablishmentName = projectSite.EstablishmentName,
        //            //    PublicApplicationRefNo = application.PublicAppRefNum,
        //            //    StatusDescription = applicationAction.Remarks,
        //            //    InvestPunjab_Ipin = application.InvestPunjab_Ipin
        //            //};
        //            //var emailTemplateText = await _iNotificationManagerService.RenderToStringAsync("EmailTemplates/_templateObjectionResolvedAndCaseLendedToOfficerNotification", mailTemplate);
        //            //var emailSendinfResponse = await _iNotificationManagerService.SendEmailSMTP(mailTemplate.OfficerName, receiverInfo.Email, "Department of Labour: The department has raised few objections, kindly examine the application form and resolved the objections. (" + mailTemplate.InvestPunjab_Ipin + ")", emailTemplateText);
        //        }
        //        else if (formModel.AppActionType == 202) // Deregistered
        //        {
        //            allOk = await _iApplicationMamnagementRepository.SwitchApplicationLifeCycleType(formModel.AppRefId, ApplicationLifeCycleStatusTypeEnum.DEREGISTERED);
        //        }
        //        else if (formModel.AppActionType == 411) // Dormant
        //        {
        //            allOk = await _iApplicationMamnagementRepository.SwitchApplicationLifeCycleType(formModel.AppRefId, ApplicationLifeCycleStatusTypeEnum.APPLICATION_DORMANT);
        //        }
        //        else if (formModel.AppActionType == 212) // Decline
        //        {
        //            allOk = await _iApplicationMamnagementRepository.SwitchApplicationLifeCycleType(formModel.AppRefId, ApplicationLifeCycleStatusTypeEnum.APPLICATION_DECLINED);
        //        }
        //        if (allOk)
        //        {
        //            var actionTimeLine = _context.AppActionTimeLines.Where(x => x.AppActionLogRefId == formModel.ApplicationActionLogId).ToList();
        //            if(actionTimeLine != null)
        //            {
        //                actionTimeLine = actionTimeLine.Select(x => { x.IsProcessed = true; return x; }).ToList();
        //                await _context.BulkInsertOrUpdateAsync(actionTimeLine);
        //            }
        //            //if (formModel.AppActionType == (int)AppActionTypeEnum.SCRUTINY_COMPLETED && (applicationActionlogs.SenderRoleId == "5b70c7bd-b591-4300-a34c-56b4bddc9416" || applicationActionlogs.SenderRoleId == "ad7c34a9-e2b5-4131-984a-c3b03ff9b4aa"))
        //            //{
        //            //    await _iAppTimeLineManagerService.AddAppProcessLogType(new ApplicationProcessPhaseLog
        //            //    {
        //            //        ApplicationProcessPhaseLogType = ApplicationProcessPhaseLogTypeEnum.OBSERVATION_PHASE,
        //            //        AppRefId = formModel.AppRefId,
        //            //        LastModifiedOn = DateTime.Now,
        //            //        PhaseCounter = 1,
        //            //    });
        //            //}
                    
        //            if (application.FirstOrDefault().IsTimeLineFlow)
        //            {
        //                await _iAppTimeLineManagerService.SeedTimeLineData(formModel.AppRefId);
        //            }

                    

        //            if (formModel.AppDocumentRefId > 0)
        //            {
        //                var appDocument = _context.ApplicationDocuments.Where(x => x.AppDocId == formModel.AppDocumentRefId).FirstOrDefault();
        //                appDocument.IsLocked = true;
        //                _context.ApplicationDocuments.Update(appDocument);
        //                await _context.SaveChangesAsync();
        //            }

        //            var licenceInfo = await _context.ApplicationLicenceNoMapping.Where(x => x.AppRefId == formModel.AppRefId).FirstOrDefaultAsync();
        //            var statusDescription = await _context.ApplicationActionCodes.Where(x => x.ActionCode == formModel.AppActionType).FirstOrDefaultAsync();
        //            Int64 appActionLogId = await _context.ApplicationActionLogs.Where(x => x.ApplicationRefId == formModel.AppRefId).OrderByDescending(x => x.ApplicationActionLogId).Select(x => x.ApplicationActionLogId).FirstOrDefaultAsync().ConfigureAwait(false);

        //            InvestPunjabShareStatusParmsViewModel statusReq = new InvestPunjabShareStatusParmsViewModel();

        //            statusReq.AppRefId = formModel.AppRefId;
        //            statusReq.ApplicationType = application.FirstOrDefault().ApplicationType;
        //            statusReq.IPin = Convert.ToInt64(application.FirstOrDefault().InvestPunjab_Ipin);
        //            statusReq.InvestPunjab_AppId = application.FirstOrDefault().InvestPunjab_AppId;
        //            //statusReq.StatusId = (formModel.AppActionType == 404 || formModel.AppActionType == 102) ? 9 : formModel.AppActionType == 200 ? 5 : formModel.AppActionType == 500 ? 15 : formModel.AppActionType;
        //            statusReq.StatusId = formModel.AppActionType;
        //            statusReq.StatusDesc = statusDescription.ActionPublicName;
        //            //statusReq.Comments = (formModel.AppActionType == 404 || formModel.AppActionType == 102 || formModel.AppActionType == 201 || formModel.AppActionType == 401) ?  formModel.Remarks: statusDescription.ActionName;
        //            statusReq.Comments = (formModel.AppActionType == 404 || formModel.AppActionType == 401 || formModel.AppActionType == 402 || formModel.AppActionType == 301 || formModel.AppActionType == 201 || formModel.AppActionType == 202 || formModel.AppActionType == 200 || formModel.AppActionType == 407 || formModel.AppActionType == 212) ? formModel.Remarks : statusDescription.ActionName;
        //            statusReq.SenderName = user.Where(x => x.UserId == userId).Select(x => x.FirstName).FirstOrDefault() + ' ' + user.Where(x => x.UserId == userId).Select(x => x.LastName).FirstOrDefault();
        //            statusReq.SenderDesignation = user.Where(x => x.UserId == userId).Select(x => x.NormalizedName).FirstOrDefault();
        //            int i = 0;
        //            foreach (var item in formModel.Receiver_UserRefId.Split("_"))
        //            {
        //                if (i > 0)
        //                {
        //                    statusReq.ReceiverName = statusReq.ReceiverName + " and ";
        //                    statusReq.ReceiverDesignation = statusReq.ReceiverDesignation + " and ";
        //                }
        //                statusReq.ReceiverName = user.Where(x => x.UserId == item).Select(x => x.FirstName).FirstOrDefault() + ' ' + user.Where(x => x.UserId == item).Select(x => x.LastName).FirstOrDefault();
        //                statusReq.ReceiverDesignation = user.Where(x => x.UserId == item).Select(x => x.NormalizedName).FirstOrDefault();
        //                i++;
        //            }

        //            statusReq.ClearanceIssuedOn = DateTime.Now;
        //            statusReq.ClearanceExpiredOn = (application.FirstOrDefault().ApplicationType == ApplicationTypeEnum.SHOP_LICENCE ? (DateTime?)null : new DateTime(DateTime.Now.Year, 12, 31));
        //            //new DateTime(DateTime.Now.Year, 12, 31);
        //            statusReq.LicenseNo = licenceInfo == null ? "NA" : licenceInfo.LicenceNumber.ToString();
        //            statusReq.ClearanceFile = (application.FirstOrDefault().ApplicationType == ApplicationTypeEnum.BUILDING_PLAN_HUD && (formModel.AppActionType == 200 || formModel.AppActionType == 204)) || formModel.AppActionType==200 || formModel.AppActionType == 206 ? Configuration.GetSection("ThirdPartyIntegrationConfigs").GetSection("WebHostRootUrl").Value + "License/"+ licenseDirName + "/" + formModel.PublicAppRefNum + ".pdf" : licenseFilePath;
        //            statusReq.StatusDate = DateTime.Now;
        //            statusReq.IntegrationSource = "LABOUR";
        //            statusReq.IsdeemedApproval = "false";
        //            statusReq.appActionType = (AppActionTypeEnum) formModel.AppActionType;
        //            statusReq.appActionLogId = appActionLogId;
        //            var response = await _iInvestPunjabShareStatusService.ShareStatusToBusinessFirst(statusReq);

        //            //var receiverInfo = await _iAuthService.GetUserProfileByUserId(applicationAction.Receiver_UserRefId);
        //            //var application = _context.Applications.Where(x => x.AppId == applicationAction.ApplicationRefId).FirstOrDefault();
        //            var projectSite = _context.ProjectSites.Where(x => x.ProjectSiteId == application.FirstOrDefault().ProjectSiteRefId).FirstOrDefault();

        //            foreach (var item in formModel.Receiver_UserRefId.Split("_"))
        //            {
        //                if (formModel.AppActionType == 200 || formModel.AppActionType == 206) //Approved Deemed
        //                {
        //                    // Sent Email Notification 
        //                    TemplateResolveObjectionCaseLendedToOfficerNotificationViewModel mailTemplate = new TemplateResolveObjectionCaseLendedToOfficerNotificationViewModel()
        //                    {
        //                        OfficerName = user.Where(x => x.UserId == item).Select(x => x.FirstName).FirstOrDefault() + ' ' + user.Where(x => x.UserId == item).Select(x => x.LastName).FirstOrDefault(),
        //                        ApplicationPurposeTitle = EnumOps.GetEnumDescriptionName<ApplicationPurposeTypeEnum>(application.FirstOrDefault().ApplicationPurposeType.ToString()),
        //                        ApplicationTypeTitle = EnumOps.GetEnumDescriptionName<ApplicationTypeEnum>(application.FirstOrDefault().ApplicationType.ToString()),
        //                        EstablishmentName = projectSite.EstablishmentName,
        //                        PublicApplicationRefNo = application.FirstOrDefault().PublicAppRefNum,
        //                        StatusDescription = applicationAction.Remarks
        //                    };
        //                    var emailTemplateText = await _iNotificationManagerService.RenderToStringAsync("EmailTemplates/_templateObjectionResolvedAndCaseLendedToOfficerNotification", mailTemplate);
        //                    //var emailSendinfResponse = await _iNotificationManagerService.SendEmailSMTP(mailTemplate.OfficerName, user.Where(x => x.UserId == formModel.Receiver_UserRefId).Select(x => x.Email).FirstOrDefault(), "Department of Labour: Your application has been approved. (" + mailTemplate.PublicApplicationRefNo + ")", emailTemplateText);

        //                    // Sent SMS Notification To User
        //                    var smsTemplateText = $"We are pleased to inform you that, your Application No. {application.FirstOrDefault().PublicAppRefNum} is got Approved from Department of Labour.-PB Labour";
        //                    var templateId = "1407172725558167954";
        //                    await _iNotificationManagerService.InitiateNotification(projectSite.UserRefId,
        //                    new List<NotificationViewModel>()
        //                    {
        //                    new NotificationViewModel(){Body=smsTemplateText, NotificationMode=NotificationModeTypeEnum.MOBILE,NotificationPurpose=NotificationPurposeTypeEnum.INFO, Title=templateId}
        //                    });
        //                }
        //                else if (formModel.AppActionType == 404) // Objection
        //                {
        //                    TemplateResolveObjectionCaseLendedToOfficerNotificationViewModel mailTemplate = new TemplateResolveObjectionCaseLendedToOfficerNotificationViewModel()
        //                    {
        //                        OfficerName = user.Where(x => x.UserId == item).Select(x => x.FirstName).FirstOrDefault() + ' ' + user.Where(x => x.UserId == item).Select(x => x.LastName).FirstOrDefault(),
        //                        ApplicationPurposeTitle = EnumOps.GetEnumDescriptionName<ApplicationPurposeTypeEnum>(application.FirstOrDefault().ApplicationPurposeType.ToString()),
        //                        ApplicationTypeTitle = EnumOps.GetEnumDescriptionName<ApplicationTypeEnum>(application.FirstOrDefault().ApplicationType.ToString()),
        //                        EstablishmentName = projectSite.EstablishmentName,
        //                        PublicApplicationRefNo = application.FirstOrDefault().PublicAppRefNum,
        //                        StatusDescription = applicationAction.Remarks,
        //                        InvestPunjab_Ipin = application.FirstOrDefault().InvestPunjab_Ipin
        //                    };
        //                    var emailTemplateText = await _iNotificationManagerService.RenderToStringAsync("EmailTemplates/_templateObjectionResolvedAndCaseLendedToOfficerNotification", mailTemplate);
        //                    //var emailSendinfResponse = await _iNotificationManagerService.SendEmailSMTP(mailTemplate.OfficerName, user.Where(x => x.UserId == formModel.Receiver_UserRefId).Select(x => x.Email).FirstOrDefault(), "Department of Labour: The department has raised few objections, kindly examine the application form and resolved the objections. (" + mailTemplate.InvestPunjab_Ipin + ")", emailTemplateText);

        //                    // Sent SMS Notification To User
        //                    var smsTemplateText = $"Department of labour raised Objection on your Application No. {application.FirstOrDefault().PublicAppRefNum} . Please login at pbindustries.gov.in for resolve it.-PB  LABOUR";
        //                    var templateId = "1407172725521534794";
        //                    await _iNotificationManagerService.InitiateNotification(application.FirstOrDefault().ProjectSites.UserRefId,
        //                    new List<NotificationViewModel>()
        //                    {
        //                new NotificationViewModel(){Body=smsTemplateText, NotificationMode=NotificationModeTypeEnum.MOBILE,NotificationPurpose=NotificationPurposeTypeEnum.INFO, Title=templateId}
        //                    });
        //                }
        //                else if (formModel.AppActionType == 103 || formModel.AppActionType == 400 || formModel.AppActionType == 9) // Forwarded For further action
        //                {
        //                    TemplateResolveObjectionCaseLendedToOfficerNotificationViewModel mailTemplate = new TemplateResolveObjectionCaseLendedToOfficerNotificationViewModel()
        //                    {
        //                        OfficerName = user.Where(x => x.UserId == item).Select(x => x.FirstName).FirstOrDefault() + ' ' + user.Where(x => x.UserId == item).Select(x => x.LastName).FirstOrDefault(),
        //                        ApplicationPurposeTitle = EnumOps.GetEnumDescriptionName<ApplicationPurposeTypeEnum>(application.FirstOrDefault().ApplicationPurposeType.ToString()),
        //                        ApplicationTypeTitle = EnumOps.GetEnumDescriptionName<ApplicationTypeEnum>(application.FirstOrDefault().ApplicationType.ToString()),
        //                        EstablishmentName = projectSite.EstablishmentName,
        //                        PublicApplicationRefNo = application.FirstOrDefault().PublicAppRefNum,
        //                        StatusDescription = applicationAction.Remarks,
        //                        InvestPunjab_Ipin = application.FirstOrDefault().InvestPunjab_Ipin
        //                    };
        //                    var emailTemplateText = await _iNotificationManagerService.RenderToStringAsync("EmailTemplates/_templateObjectionResolvedAndCaseLendedToOfficerNotification", mailTemplate);
        //                    // var emailSendinfResponse = await _iNotificationManagerService.SendEmailSMTP(mailTemplate.OfficerName, user.Where(x => x.UserId == formModel.Receiver_UserRefId).Select(x => x.Email).FirstOrDefault(), "Department of Labour: An application received. (" + mailTemplate.PublicApplicationRefNo + ")", emailTemplateText);
        //                }
        //            }

                    






        //            //else
        //            //{
        //            //    //transaction.Rollback();
        //            //    resp.HasError = true;
        //            //    resp.ErrorDesc = "Something wrong with approval files";
        //            //}
        //            resp.HasError = false;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        //transaction.Rollback();
        //        resp.HasError = true;
        //        resp.ErrorDesc = ex.Message;
        //        throw ex;
        //    }
        //    //}
        //    return resp;
        //}
        #endregion

        public async Task<GenericResponseTemplateModel<RecordActionResponseViewModel>> RecordApplicationAction(ApplicationActionViewModel formModel, string userId)
        {
            GenericResponseTemplateModel<RecordActionResponseViewModel> resp = new GenericResponseTemplateModel<RecordActionResponseViewModel>();
            try
                {
                    var applications = await _context.Applications.Where(x => x.AppId == formModel.AppRefId).FirstOrDefaultAsync();
                    var applicationAction = await _context.ApplicationActions.Where(x => x.ApplicationRefId == formModel.AppRefId).FirstOrDefaultAsync();

                    if (applications.ApplicationType == ApplicationTypeEnum.BUILDING_PLAN_PSIEC)
                    {
                        await RecordApplicationActionPSIEC(formModel, userId);
                    }
                    else
                    {
                        List<StoreProcedureParm> storeProcedureParms = new List<StoreProcedureParm>()
                        {
                            new StoreProcedureParm (){ ParmName="UserId", ParmValue= formModel.Receiver_UserRefId + "," + userId, isNumber=false }
                        };
                        var user = await _iGeneric_SP_Repository.CallStoreProcedureReaderAsync<SenderReceiverDetailViewModal>("dbo.sp_GetSenderReceiverDetail", storeProcedureParms);
                        CheckListDataViewModel ChecklistData = null;
                        if (formModel.CheckListFormJson != null && formModel.CheckListFormJson.Length > 0)
                        {
                            ChecklistData = JsonConvert.DeserializeObject<CheckListDataViewModel>(formModel.CheckListFormJson);
                        }
                        int totalDays = GetWorkingDays(applicationAction.ActionDate, DateTimeOffset.UtcNow.Date);
                        int holidays = _context.Holidays.Where(x => x.HolidayDate >= applicationAction.ActionDate && x.HolidayDate <= DateTimeOffset.UtcNow.Date && x.IsWeekend == false).Count();
                        int totalWorkingDays = 0;
                        if (holidays > 0)
                        {
                            totalWorkingDays = (totalDays - holidays);
                        }
                        else
                        {
                            totalWorkingDays = totalDays;
                        }
                        var end = DateTime.Now;
                        var start = applicationAction.ActionDate;
                        var weekend = new[] { DayOfWeek.Saturday, DayOfWeek.Sunday };
                        var totalWorkingHours = Enumerable.Range(0, Convert.ToInt32(end.Subtract(start).TotalHours))
                             .Count(offset => !weekend.Contains(start.AddHours(offset).DayOfWeek));
                        if (applicationAction != null)
                        {
                            applicationAction.ActionTakenDaysCount = totalWorkingDays;
                            applicationAction.ActionTakenHoursCount = Convert.ToInt64(totalWorkingHours);
                            applicationAction.ActionDate = DateTime.Now;
                            applicationAction.AppActionType = formModel.AppActionType;

                            applicationAction.Receiver_ProfileRefId = user.Where(x => x.UserId.ToLower() == formModel.Receiver_UserRefId.ToLower()).Select(x => x.UserProfileId).FirstOrDefault();  //user.Where(x => x.UserId == formModel.UserId).Select(x => x.UserProfileId).FirstOrDefault();
                            applicationAction.Receiver_UserRefId = formModel.Receiver_UserRefId;

                            applicationAction.Sender_ProfileRefId = user.Where(x => x.UserId == userId).Select(x => x.UserProfileId).FirstOrDefault();
                            applicationAction.Sender_UserRefId = userId;
                            applicationAction.Remarks = formModel.Remarks;
                            applicationAction.IsDocumentUploaded = formModel.IsDocumentUploaded;
                            applicationAction.AppDocumentRefId = formModel.AppDocumentRefId;

                            applicationAction.Checklist_Json = formModel.CheckListFormJson;
                            applicationAction.ReceiverRoleId = user.Where(x => x.UserId.ToLower() == formModel.Receiver_UserRefId.ToLower()).Select(x => x.RoleId).FirstOrDefault();
                            applicationAction.SenderRoleId = user.Where(x => x.UserId.ToLower() == userId.ToLower()).Select(x => x.RoleId).FirstOrDefault();
                            applicationAction.ActionTakenModeType = formModel.ActionTakenModeType != null ? formModel.ActionTakenModeType : 0;
                        if (ChecklistData != null)
                            {
                                applicationAction.Checklist_IsAllAgreed = !ChecklistData.CheckListNodes.Exists(x => x.IsVerified == false);
                                applicationAction.Checklist_FieldObjections = ChecklistData.CheckListNodes.Where(x => !x.IsVerified && !x.IsDocument).Count();
                                applicationAction.Checklist_DocObjections = ChecklistData.CheckListNodes.Where(x => !x.IsVerified && x.IsDocument).Count();
                            }
                            else
                            {
                                applicationAction.Checklist_IsAllAgreed = true;
                                applicationAction.Checklist_FieldObjections = 0;
                                applicationAction.Checklist_DocObjections = 0;
                            }
                        //if (formModel.AppActionType == 206 || formModel.AppActionType == 214)
                            if (formModel.AppActionType == 206)
                            {
                                applicationAction.IpAddress = "Deemed";
                                applicationAction.Latitude = "Deemed";
                                applicationAction.Longitude = "Deemed";
                            }
                            else if (formModel.AppActionType == 208)
                            {
                                applicationAction.IpAddress = "Auto Approve";
                                applicationAction.Latitude = "Auto Approve";
                                applicationAction.Longitude = "Auto Approve";
                            }
                            else if (formModel.AppActionType == 411)
                            {
                                applicationAction.IpAddress = "Dormant";
                                applicationAction.Latitude = "Dormant";
                                applicationAction.Longitude = "Dormant";
                            }
                            else if (formModel.UserId == "C79F57CB-40BA-48EA-8336-327DC1B78701")
                            {
                                applicationAction.IpAddress = "Auto Action";
                                applicationAction.Latitude = "Auto Action";
                                applicationAction.Longitude = "Auto Action";
                            }
                            else
                            {
                                if (formModel.IpAddress == null)
                                {
                                    applicationAction.IpAddress = "Auto Action";
                                    applicationAction.Latitude = "Auto Action";
                                    applicationAction.Longitude = "Auto Action";
                                }
                                else
                                {
                                    applicationAction.IpAddress = formModel.IpAddress.ToString();
                                    applicationAction.Latitude = formModel.Latitude.ToString();
                                    applicationAction.Longitude = formModel.Longitude.ToString();
                                }

                            }
                            applicationAction.IsDeleted = false;
                            _iGR_ApplicationAction.Update(applicationAction);
                            await _iGR_ApplicationAction.SavechangeAsync();
                        }
                        else
                        {
                            //_iGR_ApplicationAction.Insert(formModel);
                            //await _iGR_ApplicationAction.SavechangeAsync();
                        }
                        ApplicationActionLog applicationActionlogs = new ApplicationActionLog()
                        {
                            ActionDate = applicationAction.ActionDate,
                            ActionTakenDaysCount = applicationAction.ActionTakenDaysCount,
                            ActionTakenHoursCount = applicationAction.ActionTakenHoursCount,
                            AppActionType = applicationAction.AppActionType,

                            Receiver_ProfileRefId = applicationAction.Receiver_ProfileRefId,
                            Receiver_UserRefId = applicationAction.Receiver_UserRefId,

                            Sender_ProfileRefId = applicationAction.Sender_ProfileRefId,
                            Sender_UserRefId = applicationAction.Sender_UserRefId,
                            Remarks = applicationAction.Remarks,
                            ReceiverRoleId = applicationAction.ReceiverRoleId,
                            ApplicationRefId = applicationAction.ApplicationRefId,
                            SenderRoleId = applicationAction.SenderRoleId,

                            Checklist_Json = applicationAction.Checklist_Json,
                            Checklist_IsAllAgreed = applicationAction.Checklist_IsAllAgreed,
                            Checklist_FieldObjections = applicationAction.Checklist_FieldObjections,
                            Checklist_DocObjections = applicationAction.Checklist_DocObjections,
                            IsDocumentUploaded = applicationAction.IsDocumentUploaded,
                            AppDocumentRefId = applicationAction.AppDocumentRefId,

                            IpAddress = applicationAction.IpAddress,
                            Latitude = applicationAction.Latitude,
                            Longitude = applicationAction.Longitude,
                            IsDeleted = false
                        };
                        _iGR_ApplicationActionLog.Insert(applicationActionlogs);
                        await _iGR_ApplicationActionLog.SavechangeAsync();
                        resp.ResponseDataModel = new RecordActionResponseViewModel()
                        {
                            AppActionId = applicationAction.AppActionId,
                            ApplicationActionLogId = applicationActionlogs.ApplicationActionLogId
                        };
                        if (applicationAction.AppActionType == 411)
                        {
                            AppActivationLogs activationLogs = new AppActivationLogs();
                            activationLogs.AppActionLogRefId = applicationActionlogs.ApplicationActionLogId;
                            activationLogs.ApplicationStatus = ApplicationActivationStatusTypeEnum.DEACTIVATED;
                            activationLogs.AppRefId = applicationAction.ApplicationRefId;
                            activationLogs.ActionDate = DateTime.Now;

                           await  _context.AppActivationLogs.AddAsync(activationLogs);
                            await _context.SaveChangesAsync();

                        }
                        // Update ApplicationActionLogId In BuildingPlanHUDPaymentDetail Table. 
                        if (applicationAction.AppActionType == 401)
                        {
                            var paymentList = _context.BuildingPlanHUDPaymentDetails.Where(x => x.AppRefId == formModel.AppRefId).ToList();
                            foreach (var payment in paymentList)
                            {
                                payment.ApplicationActionLogId = applicationActionlogs.ApplicationActionLogId;
                                _context.SaveChanges();
                            }
                        }
                        bool allOk = true;
                        //Handle Approvals and rejections
                        string licenseFilePath = "NA";
                        string licenseDirName = "";
                        var application = await _iGR_Application.GetAsync(x => x.AppId == formModel.AppRefId, null, x => x.ProjectSites).ConfigureAwait(false);
                    if ((formModel.AppActionType == 200 || formModel.AppActionType == 206) && application.FirstOrDefault().ApplicationType != ApplicationTypeEnum.BUILDING_PLAN_HUD)//Approved
                    {
                        //Application application
                        if (formModel.AppActionType == 206)
                        {
                            allOk = await _iApplicationMamnagementRepository.SwitchApplicationLifeCycleType(formModel.AppRefId, ApplicationLifeCycleStatusTypeEnum.DEEMED_COMPLETED);
                        }
                        //else if (formModel.AppActionType == 214)
                        //{
                        //    allOk = await _iApplicationMamnagementRepository.SwitchApplicationLifeCycleType(formModel.AppRefId, ApplicationLifeCycleStatusTypeEnum.DEEMED_WITH_FEE_PENDING);
                        //}
                        else if (formModel.AppActionType == 208)
                        {
                            allOk = await _iApplicationMamnagementRepository.SwitchApplicationLifeCycleType(formModel.AppRefId, ApplicationLifeCycleStatusTypeEnum.AUTO_APPROVEL_COMPLETED);
                        }
                        else if (formModel.AppActionType == 411)
                        {
                            allOk = await _iApplicationMamnagementRepository.SwitchApplicationLifeCycleType(formModel.AppRefId, ApplicationLifeCycleStatusTypeEnum.APPLICATION_DORMANT);
                        }
                        else
                        {
                            allOk = await _iApplicationMamnagementRepository.SwitchApplicationLifeCycleType(formModel.AppRefId, ApplicationLifeCycleStatusTypeEnum.APPROVED);
                        }

                        var deemeedfile = _context.All_Act_Deemed_ProcessFilesLogs.Where(x => x.AppRefId == formModel.AppRefId && x.DeemedProcessStatusType == DeemedProcessStatusTypeEnum.PENDING).FirstOrDefault();
                        if (deemeedfile != null && formModel.AppActionType == 200)
                        {
                            deemeedfile.DeemedProcessStatusType = DeemedProcessStatusTypeEnum.COMPLETED_BY_OFFICER;
                            _context.All_Act_Deemed_ProcessFilesLogs.Update(deemeedfile);
                            await _context.SaveChangesAsync();
                        }

                        //GenerateLicenceNoViewModel licenceNo = await _iApplicationMamnagementRepository.GenerateLicenceNo(formModel.AppRefId, application.FirstOrDefault().ApplicationType);

                        var tempFilePath = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "TempFiles")).Root;

                        if (File.Exists(tempFilePath + formModel.PdfNameGUID + ".pdf") && allOk)
                        {
                            licenseDirName = "AppForm_" + application.FirstOrDefault().ApplicationType.ToString();
                            var licenceRootDirUrl = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "License")).Root.ToString();
                            Directory.CreateDirectory(licenceRootDirUrl + licenseDirName);
                            licenseFilePath = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "License", licenseDirName)).Root;

                            formModel.PublicAppRefNum = _context.Applications.Where(x => x.AppId == formModel.AppRefId).FirstOrDefault().PublicAppRefNum;

                            if (File.Exists(licenseFilePath + formModel.PublicAppRefNum + ".pdf"))
                            {
                                File.Delete(licenseFilePath + formModel.PublicAppRefNum + ".pdf");
                            }
                            File.Move(tempFilePath + formModel.PdfNameGUID + ".pdf", licenseFilePath + formModel.PublicAppRefNum + ".pdf");
                        }
                        else
                        {
                            allOk = false;
                        }

                        if (allOk && application.FirstOrDefault().ApplicationType == ApplicationTypeEnum.FACTORY_LICENCE)
                        {
                            Licence_Factory_AdditionalDetail additionalInfo = new Licence_Factory_AdditionalDetail()
                            {
                                AppRefId = application.FirstOrDefault().AppId,
                                FactoryHazardousCategoryType = formModel.FactoryHazardousCategoryType,
                                FactorySectionCategoryType = formModel.FactorySectionCategoryType,
                                FactorySessionCategoryType = formModel.FactorySessionCategoryType,
                                LabourCircleRefId = formModel.LabourCircleRefId,
                                FactoryCategoryType = formModel.FactoryCategoryType
                            };

                            storeProcedureParms = new List<StoreProcedureParm>()
                            {
                                new StoreProcedureParm (){ ParmName="AppRefId", ParmValue= additionalInfo.AppRefId.ToString(), isNumber=true},
                                new StoreProcedureParm (){ ParmName="FactoryHazardousCategoryType", ParmValue= ((int)additionalInfo.FactoryHazardousCategoryType).ToString(), isNumber=true},
                                new StoreProcedureParm (){ ParmName="FactorySectionCategoryType", ParmValue= ((int)additionalInfo.FactorySectionCategoryType).ToString(), isNumber=true },
                                new StoreProcedureParm (){ ParmName="FactorySessionCategoryType", ParmValue= ((int)additionalInfo.FactorySessionCategoryType).ToString(), isNumber=true},
                                new StoreProcedureParm (){ ParmName="LabourCircleRefId", ParmValue= additionalInfo.LabourCircleRefId.ToString(), isNumber=true},
                                new StoreProcedureParm (){ ParmName="FactoryCategoryType", ParmValue= ((int)additionalInfo.FactoryCategoryType).ToString(), isNumber=true}

                            };
                            var val = await _iGeneric_SP_Repository.CallStoreProcedureReaderScalerAsync("dbo.sp_Licence_Add_Factory_AdditionalDetails", storeProcedureParms);
                        }

                    }
                    else if ((formModel.AppActionType == 402) && application.FirstOrDefault().ApplicationType != ApplicationTypeEnum.BUILDING_PLAN_HUD)//Raised Additional Fee
                    {
                        if (allOk && application.FirstOrDefault().ApplicationType == ApplicationTypeEnum.FACTORY_LICENCE)
                        {
                            decimal raisedFee = 0;
                            if (formModel.ExistingPowerKW_Installed != formModel.PowerKW_Installed || formModel.ExistingWorkers_MaxDuringYear != formModel.Workers_MaxDuringYear)
                            {
                                raisedFee = formModel.RaisedFeeAmount + 100;
                            }
                            else
                            {
                                raisedFee = formModel.RaisedFeeAmount;
                            }
                            AppFeeDetail appFeeDetail = new AppFeeDetail()
                            {
                                Amount = raisedFee,
                                AppRefId = formModel.AppRefId,
                                CalculatedOn = DateTime.Now,
                                DedicatedDDOCode = null,
                                DedicatedTreasurCode = null,
                                Description = formModel.RaisedFeeReason,
                                FeeHeaderRefId = 30,
                                HasDedicatedTreasuryCode = false,
                                IsDeduductible = false,
                                PaymentPartCounter = 1
                            };

                            var res = await IncreasePaymentBatchCounter(appFeeDetail.AppRefId);
                            appFeeDetail.PaymentBatchCounter = res.ResponseDataModel;
                            await _context.AppFeeDetails.AddAsync(appFeeDetail);
                            await _context.SaveChangesAsync();


                            // If any field change, insert into Factory & Amendment History Table
                            if (formModel.ExistingWorkers_MaxDuringYear != formModel.Workers_MaxDuringYear || formModel.ExistingPowerKW_Installed != formModel.PowerKW_Installed)
                            {
                                // Update Factory Details
                                var factoryLicence = _context.Licence_Factory_GeneralDetails.Where(x => x.AppRefId == formModel.AppRefId).FirstOrDefault();
                                //Licence_Factory_GeneralDetail factory_GeneralDetail = new Licence_Factory_GeneralDetail()
                                //{
                                //    Workers_MaxDuringYear = formModel.Workers_MaxDuringYear,
                                //    PowerKW_Installed = formModel.PowerKW_Installed
                                //};
                                factoryLicence.Workers_MaxDuringYear = formModel.Workers_MaxDuringYear;
                                factoryLicence.PowerKW_Installed = formModel.PowerKW_Installed;
                                //_iGR_Licence_Factory_GeneralDetail.Update(factory_GeneralDetail);
                                //await _iGR_Licence_Factory_GeneralDetail.SavechangeAsync();
                                _context.Licence_Factory_GeneralDetails.Update(factoryLicence);
                                _context.SaveChanges();

                                // Insert Into Log Table
                                Licence_Factory_AmendmentDataHistory amendmentDataHistory = new Licence_Factory_AmendmentDataHistory();
                                if (formModel.ExistingWorkers_MaxDuringYear != formModel.Workers_MaxDuringYear)
                                {
                                    amendmentDataHistory = new Licence_Factory_AmendmentDataHistory()
                                    {
                                        AppRefId = formModel.AppRefId,
                                        FieldName = "Workers_MaxDuringYear",
                                        PreviousValue = formModel.ExistingWorkers_MaxDuringYear.ToString(),
                                        ModifiedValue = formModel.Workers_MaxDuringYear.ToString(),
                                        SectionCode = "MP",
                                        ModifiedOn = DateTime.Now,
                                        ModifiedCounter = appFeeDetail.PaymentBatchCounter
                                    };
                                    await _context.Licence_Factory_AmendmentDataHistories.AddAsync(amendmentDataHistory);
                                }
                                if (formModel.ExistingPowerKW_Installed != formModel.PowerKW_Installed)
                                {
                                    amendmentDataHistory = new Licence_Factory_AmendmentDataHistory()
                                    {
                                        AppRefId = formModel.AppRefId,
                                        FieldName = "PowerKW_Installed",
                                        PreviousValue = formModel.ExistingPowerKW_Installed.ToString(),
                                        ModifiedValue = formModel.PowerKW_Installed.ToString(),
                                        SectionCode = "KW",
                                        ModifiedOn = DateTime.Now,
                                        ModifiedCounter = appFeeDetail.PaymentBatchCounter
                                    };
                                    await _context.Licence_Factory_AmendmentDataHistories.AddAsync(amendmentDataHistory);
                                }
                                await _context.SaveChangesAsync();
                            }

                            allOk = await _iApplicationMamnagementRepository.SwitchApplicationLifeCycleType(formModel.AppRefId, ApplicationLifeCycleStatusTypeEnum.IN_OBJECTION_WITH_BALANCE_FEE);
                        }
                        else if (allOk && application.FirstOrDefault().ApplicationType == ApplicationTypeEnum.PRINCIPAL_EMPLOYER)
                        {
                            decimal raisedFee = 0;
                            raisedFee = formModel.RaisedFeeAmount;

                            AppFeeDetail appFeeDetail = new AppFeeDetail()
                            {
                                Amount = raisedFee,
                                AppRefId = formModel.AppRefId,
                                CalculatedOn = DateTime.Now,
                                DedicatedDDOCode = null,
                                DedicatedTreasurCode = null,
                                Description = formModel.RaisedFeeReason,
                                FeeHeaderRefId = 30,
                                HasDedicatedTreasuryCode = false,
                                IsDeduductible = false,
                                PaymentPartCounter = 1
                            };

                            var res = await IncreasePaymentBatchCounter(appFeeDetail.AppRefId);
                            appFeeDetail.PaymentBatchCounter = res.ResponseDataModel;
                            await _context.AppFeeDetails.AddAsync(appFeeDetail);
                            await _context.SaveChangesAsync();

                            allOk = await _iApplicationMamnagementRepository.SwitchApplicationLifeCycleType(formModel.AppRefId, ApplicationLifeCycleStatusTypeEnum.IN_OBJECTION_WITH_BALANCE_FEE);
                        }
                        else if (allOk && application.FirstOrDefault().ApplicationType == ApplicationTypeEnum.CONTRACT_LABOUR || application.FirstOrDefault().ApplicationType == ApplicationTypeEnum.ISM_CONTRACT_LABOUR)
                        {
                            decimal raisedFee = formModel.RaisedFeeAmount;
                            AppFeeDetail raisedFeeDetail = new AppFeeDetail()
                            {
                                Amount = raisedFee,
                                AppRefId = formModel.AppRefId,
                                CalculatedOn = DateTime.Now,
                                DedicatedDDOCode = null,
                                DedicatedTreasurCode = null,
                                Description = formModel.RaisedFeeReason,
                                FeeHeaderRefId = 30,
                                HasDedicatedTreasuryCode = false,
                                IsDeduductible = false,
                                PaymentPartCounter = 1
                            };

                            decimal securityRaisedFee = formModel.SecurityRaisedFeeAmount;
                            AppFeeDetail securityFeeDetail = new AppFeeDetail()
                            {
                                Amount = securityRaisedFee,
                                AppRefId = formModel.AppRefId,
                                CalculatedOn = DateTime.Now,
                                DedicatedDDOCode = null,
                                DedicatedTreasurCode = null,
                                Description = formModel.RaisedFeeReason,
                                FeeHeaderRefId = 47,
                                HasDedicatedTreasuryCode = false,
                                IsDeduductible = false,
                                PaymentPartCounter = 1
                            };

                            var res = await IncreasePaymentBatchCounter(formModel.AppRefId);
                            raisedFeeDetail.PaymentBatchCounter = res.ResponseDataModel;
                            securityFeeDetail.PaymentBatchCounter = res.ResponseDataModel;

                            await _context.AppFeeDetails.AddAsync(raisedFeeDetail);
                            await _context.AppFeeDetails.AddAsync(securityFeeDetail);
                            await _context.SaveChangesAsync();

                            allOk = await _iApplicationMamnagementRepository.SwitchApplicationLifeCycleType(formModel.AppRefId, ApplicationLifeCycleStatusTypeEnum.IN_OBJECTION_WITH_BALANCE_FEE);
                        }
                    }
                    else if (application.FirstOrDefault().ApplicationType == ApplicationTypeEnum.BUILDING_PLAN_HUD && (formModel.AppActionType == 200 || formModel.AppActionType == 204 || formModel.AppActionType == 206)) // Approved -Building Plan Hud
                    {
                        //if (formModel.AppActionType == 214)
                        //{
                        //    allOk = await _iApplicationMamnagementRepository.SwitchApplicationLifeCycleType(formModel.AppRefId, ApplicationLifeCycleStatusTypeEnum.DEEMED_WITH_FEE_PENDING);
                        //}
                        if (formModel.AppActionType == 206)
                        {
                            allOk = await _iApplicationMamnagementRepository.SwitchApplicationLifeCycleType(formModel.AppRefId, ApplicationLifeCycleStatusTypeEnum.DEEMED_COMPLETED);
                        }
                        else if (formModel.AppActionType == 200 || formModel.AppActionType == 204)
                        {
                            allOk = await _iApplicationMamnagementRepository.SwitchApplicationLifeCycleType(formModel.AppRefId, ApplicationLifeCycleStatusTypeEnum.APPROVED);
                        }
                        var appDocument = await _context.ApplicationDocuments.Where(x => x.AppDocId == formModel.AppDocumentRefId).FirstOrDefaultAsync();
                        if (appDocument != null)
                        {
                            var tempFilePath = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "AppFiles")).Root;

                            if (File.Exists(tempFilePath + appDocument.AttachmentName) && allOk)
                            {
                                licenseDirName = "AppForm_" + application.FirstOrDefault().ApplicationType.ToString();
                                var licenceRootDirUrl = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "License")).Root.ToString();
                                Directory.CreateDirectory(licenceRootDirUrl + licenseDirName);
                                licenseFilePath = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "License", licenseDirName)).Root;

                                formModel.PublicAppRefNum = application.FirstOrDefault().PublicAppRefNum;

                                if (File.Exists(licenseFilePath + formModel.PublicAppRefNum + ".pdf"))
                                {
                                    File.Delete(licenseFilePath + formModel.PublicAppRefNum + ".pdf");
                                }
                                File.Copy(tempFilePath + appDocument.AttachmentName, licenseFilePath + formModel.PublicAppRefNum + ".pdf");
                            }
                            else
                            {
                                //Application application
                                if (formModel.AppActionType == 206)
                                {
                                    allOk = await _iApplicationMamnagementRepository.SwitchApplicationLifeCycleType(formModel.AppRefId, ApplicationLifeCycleStatusTypeEnum.DEEMED_COMPLETED);
                                }
                                else if (formModel.AppActionType == 208)
                                {
                                    allOk = await _iApplicationMamnagementRepository.SwitchApplicationLifeCycleType(formModel.AppRefId, ApplicationLifeCycleStatusTypeEnum.AUTO_APPROVEL_COMPLETED);
                                }
                                else if (formModel.AppActionType == 214)
                                {
                                    allOk = await _iApplicationMamnagementRepository.SwitchApplicationLifeCycleType(formModel.AppRefId, ApplicationLifeCycleStatusTypeEnum.DEEMED_WITH_FEE_PENDING);
                                }
                                else
                                {
                                    allOk = await _iApplicationMamnagementRepository.SwitchApplicationLifeCycleType(formModel.AppRefId, ApplicationLifeCycleStatusTypeEnum.APPROVED);
                                }

                                allOk = false;
                            }

                            if (allOk)
                            {

                                var licenceNo = "DOFPB" + application.FirstOrDefault().ProjectSiteRefId.ToString("D8");

                                ApplicationLicenceNoMapping applicationLicenceNo = new ApplicationLicenceNoMapping()
                                {
                                    AppRefId = formModel.AppRefId,
                                    LicenceNumber = licenceNo
                                };

                                await _context.ApplicationLicenceNoMapping.AddAsync(applicationLicenceNo);
                                await _context.SaveChangesAsync();
                            }
                        }
                        else
                        {
                            var tempFilePath = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "TempFiles")).Root;

                            if (File.Exists(tempFilePath + formModel.PdfNameGUID + ".pdf") && allOk)
                            {
                                licenseDirName = "AppForm_" + application.FirstOrDefault().ApplicationType.ToString();
                                var licenceRootDirUrl = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "License")).Root.ToString();
                                Directory.CreateDirectory(licenceRootDirUrl + licenseDirName);
                                licenseFilePath = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "License", licenseDirName)).Root;

                                formModel.PublicAppRefNum = application.FirstOrDefault().PublicAppRefNum;

                                if (File.Exists(licenseFilePath + formModel.PublicAppRefNum + ".pdf"))
                                {
                                    File.Delete(licenseFilePath + formModel.PublicAppRefNum + ".pdf");
                                }
                                File.Copy(tempFilePath + formModel.PdfNameGUID + ".pdf", licenseFilePath + formModel.PublicAppRefNum + ".pdf");
                                allOk = true;
                            }
                            if (allOk)
                            {

                                var licenceNo = "DOFPB" + application.FirstOrDefault().ProjectSiteRefId.ToString("D8");

                                ApplicationLicenceNoMapping applicationLicenceNo = new ApplicationLicenceNoMapping()
                                {
                                    AppRefId = formModel.AppRefId,
                                    LicenceNumber = licenceNo
                                };

                                await _context.ApplicationLicenceNoMapping.AddAsync(applicationLicenceNo);
                                await _context.SaveChangesAsync();
                            }
                        }

                        var deemeedfile = _context.All_Act_Deemed_ProcessFilesLogs.Where(x => x.AppRefId == formModel.AppRefId && x.DeemedProcessStatusType == DeemedProcessStatusTypeEnum.PENDING).FirstOrDefault();
                        if (deemeedfile != null && formModel.AppActionType == 200)
                        {
                            deemeedfile.DeemedProcessStatusType = DeemedProcessStatusTypeEnum.COMPLETED_BY_OFFICER;
                            _context.All_Act_Deemed_ProcessFilesLogs.Update(deemeedfile);
                            await _context.SaveChangesAsync();
                        }
                    }
                    else if (formModel.AppActionType == 201) // Rejected
                    {
                        allOk = await _iApplicationMamnagementRepository.SwitchApplicationLifeCycleType(formModel.AppRefId, ApplicationLifeCycleStatusTypeEnum.REJECTED);
                        // Sent SMS Notification To User
                        var smsTemplateText = $"Dear Applicant,Your Application No. {application.FirstOrDefault().PublicAppRefNum} is Rejected from Department of Labour-PB LBAOUR";
                        var templateId = "1407172725588507473";
                        await _iNotificationManagerService.InitiateNotification(application.FirstOrDefault().ProjectSites.UserRefId,
                        new List<NotificationViewModel>()
                        {
                            new NotificationViewModel(){Body=smsTemplateText, NotificationMode=NotificationModeTypeEnum.MOBILE,NotificationPurpose=NotificationPurposeTypeEnum.INFO, Title=templateId}
                        });
                    }
                    else if (formModel.AppActionType == 404 || formModel.AppActionType == 407) // Objection
                    {
                        allOk = await _iApplicationMamnagementRepository.SwitchApplicationLifeCycleType(formModel.AppRefId, ApplicationLifeCycleStatusTypeEnum.IN_OBJECTION);

                        // Sent SMS Notification To User
                        var smsTemplateText = $"Department of labour raised Objection on your Application No. {application.FirstOrDefault().PublicAppRefNum} . Please login at pbindustries.gov.in for resolve it.-PB  LABOUR";
                        var templateId = "1407172725521534794";
                        await _iNotificationManagerService.InitiateNotification(application.FirstOrDefault().ProjectSites.UserRefId,
                        new List<NotificationViewModel>()
                        {
                            new NotificationViewModel(){Body=smsTemplateText, NotificationMode=NotificationModeTypeEnum.MOBILE,NotificationPurpose=NotificationPurposeTypeEnum.INFO, Title=templateId}
                        });

                        // Sent Email Notification 
                        //var receiverInfo = await _iAuthService.GetUserProfileByUserId(applicationAction.Receiver_UserRefId);
                        //var application = _context.Applications.Where(x => x.AppId == applicationAction.ApplicationRefId).FirstOrDefault();
                        //var projectSite = _context.ProjectSites.Where(x => x.ProjectSiteId == application.ProjectSiteRefId).FirstOrDefault();
                        //TemplateResolveObjectionCaseLendedToOfficerNotificationViewModel mailTemplate = new TemplateResolveObjectionCaseLendedToOfficerNotificationViewModel()
                        //{
                        //    OfficerName = receiverInfo.FirstName + " " + receiverInfo.LastName,
                        //    ApplicationPurposeTitle = EnumOps.GetEnumDescriptionName<ApplicationPurposeTypeEnum>(application.ApplicationPurposeType.ToString()),
                        //    ApplicationTypeTitle = EnumOps.GetEnumDescriptionName<ApplicationTypeEnum>(application.ApplicationType.ToString()),
                        //    EstablishmentName = projectSite.EstablishmentName,
                        //    PublicApplicationRefNo = application.PublicAppRefNum,
                        //    StatusDescription = applicationAction.Remarks,
                        //    InvestPunjab_Ipin = application.InvestPunjab_Ipin
                        //};
                        //var emailTemplateText = await _iNotificationManagerService.RenderToStringAsync("EmailTemplates/_templateObjectionResolvedAndCaseLendedToOfficerNotification", mailTemplate);
                        //var emailSendinfResponse = await _iNotificationManagerService.SendEmailSMTP(mailTemplate.OfficerName, receiverInfo.Email, "Department of Labour: The department has raised few objections, kindly examine the application form and resolved the objections. (" + mailTemplate.InvestPunjab_Ipin + ")", emailTemplateText);
                    }
                    else if (formModel.AppActionType == 202) // Deregistered
                    {
                        allOk = await _iApplicationMamnagementRepository.SwitchApplicationLifeCycleType(formModel.AppRefId, ApplicationLifeCycleStatusTypeEnum.DEREGISTERED);
                    }
                    else if (formModel.AppActionType == 411) // Dormant
                    {
                        allOk = await _iApplicationMamnagementRepository.SwitchApplicationLifeCycleType(formModel.AppRefId, ApplicationLifeCycleStatusTypeEnum.APPLICATION_DORMANT);
                    }
                    else if (formModel.AppActionType == 212) // Decline
                    {
                        allOk = await _iApplicationMamnagementRepository.SwitchApplicationLifeCycleType(formModel.AppRefId, ApplicationLifeCycleStatusTypeEnum.APPLICATION_DECLINED);
                    }
                        if (allOk)
                        {
                            var actionTimeLine = _context.AppActionTimeLines.Where(x => x.AppActionLogRefId == formModel.ApplicationActionLogId).ToList();
                            if (actionTimeLine != null)
                            {
                                actionTimeLine = actionTimeLine.Select(x => { x.IsProcessed = true; return x; }).ToList();
                                await _context.BulkInsertOrUpdateAsync(actionTimeLine);
                            }
                            if (application.FirstOrDefault().IsTimeLineFlow)
                            {
                                await _iAppTimeLineManagerService.SeedTimeLineData(formModel.AppRefId,0);
                            }

                            var deemeedfile = _context.All_Act_Deemed_ProcessFilesLogs.Where(x => x.AppRefId == formModel.AppRefId && x.DeemedProcessStatusType == DeemedProcessStatusTypeEnum.PENDING).FirstOrDefault();
                            if (deemeedfile != null && formModel.AppActionType != 200)
                            {
                                deemeedfile.DeemedProcessStatusType = DeemedProcessStatusTypeEnum.ACTION_BY_OFFICER;
                                _context.All_Act_Deemed_ProcessFilesLogs.Update(deemeedfile);
                                await _context.SaveChangesAsync();
                            }

                        if (formModel.AppDocumentRefId > 0)
                            {
                                var appDocument = _context.ApplicationDocuments.Where(x => x.AppDocId == formModel.AppDocumentRefId).FirstOrDefault();
                                appDocument.IsLocked = true;
                                _context.ApplicationDocuments.Update(appDocument);
                                await _context.SaveChangesAsync();
                            }
                            var licenceInfo = await _context.ApplicationLicenceNoMapping.Where(x => x.AppRefId == formModel.AppRefId).FirstOrDefaultAsync();
                            var statusDescription = await _context.ApplicationActionCodes.Where(x => x.ActionCode == formModel.AppActionType).FirstOrDefaultAsync();
                            Int64 appActionLogId = await _context.ApplicationActionLogs.Where(x => x.ApplicationRefId == formModel.AppRefId).OrderByDescending(x => x.ApplicationActionLogId).Select(x => x.ApplicationActionLogId).FirstOrDefaultAsync().ConfigureAwait(false);

                            ////Licence Path
                            //var licencepath = "NA";
                            //if(formModel.AppActionType == 214 || formModel.AppActionType == 215 || formModel.AppActionType == 216)
                            //{
                            //    var encryptedAppId = RijndaelManagedCryptoHandler.EncryptFromPlainTextToBase64(formModel.AppRefId.ToString(), Configuration.GetSection("EncryptionConfigs").GetSection("MobileAppDataEncryptionKey").Value, Configuration.GetSection("EncryptionConfigs").GetSection("MobileAppDataIVKey").Value);
                            //    licencepath = Configuration.GetSection("ThirdPartyIntegrationConfigs").GetSection("WebHostUIRootUrl").Value + "download-approval?msg=" + encryptedAppId;
                            //}
                            //else
                            //{
                            //    licencepath = (application.FirstOrDefault().ApplicationType == ApplicationTypeEnum.BUILDING_PLAN_HUD && (formModel.AppActionType == 200 || formModel.AppActionType == 204)) || formModel.AppActionType == 200 || formModel.AppActionType == 206 ? Configuration.GetSection("ThirdPartyIntegrationConfigs").GetSection("WebHostRootUrl").Value + "License/" + licenseDirName + "/" + formModel.PublicAppRefNum + ".pdf" : licenseFilePath;
                            //}
                            InvestPunjabShareStatusParmsViewModel statusReq = new InvestPunjabShareStatusParmsViewModel();

                            statusReq.AppRefId = formModel.AppRefId;
                            statusReq.ApplicationType = application.FirstOrDefault().ApplicationType;
                            statusReq.IPin = Convert.ToInt64(application.FirstOrDefault().InvestPunjab_Ipin);
                            statusReq.InvestPunjab_AppId = application.FirstOrDefault().InvestPunjab_AppId;
                            statusReq.StatusId = formModel.AppActionType;
                            statusReq.StatusDesc = statusDescription.ActionPublicName;
                            statusReq.Comments = (formModel.AppActionType == 404 || formModel.AppActionType == 401 || formModel.AppActionType == 402 || formModel.AppActionType == 301 || formModel.AppActionType == 201 || formModel.AppActionType == 202 || formModel.AppActionType == 200 || formModel.AppActionType == 407 || formModel.AppActionType == 212) ? formModel.Remarks : statusDescription.ActionName;
                            statusReq.SenderName = user.Where(x => x.UserId == userId).Select(x => x.FirstName).FirstOrDefault() + ' ' + user.Where(x => x.UserId == userId).Select(x => x.LastName).FirstOrDefault();
                            statusReq.SenderDesignation = user.Where(x => x.UserId == userId).Select(x => x.NormalizedName).FirstOrDefault();
                            statusReq.ReceiverName = user.Where(x => x.UserId == formModel.Receiver_UserRefId).Select(x => x.FirstName).FirstOrDefault() + ' ' + user.Where(x => x.UserId == formModel.Receiver_UserRefId).Select(x => x.LastName).FirstOrDefault();
                            statusReq.ReceiverDesignation = user.Where(x => x.UserId == formModel.Receiver_UserRefId).Select(x => x.NormalizedName).FirstOrDefault();

                            statusReq.ClearanceIssuedOn = DateTime.Now;
                            statusReq.ClearanceExpiredOn = (application.FirstOrDefault().ApplicationType == ApplicationTypeEnum.SHOP_LICENCE ? (DateTime?)null : new DateTime(DateTime.Now.Year, 12, 31));
                            statusReq.LicenseNo = licenceInfo == null ? "NA" : licenceInfo.LicenceNumber.ToString();
                            statusReq.ClearanceFile = (application.FirstOrDefault().ApplicationType == ApplicationTypeEnum.BUILDING_PLAN_HUD && (formModel.AppActionType == 200 || formModel.AppActionType == 204)) || formModel.AppActionType == 200 || formModel.AppActionType == 206 ? Configuration.GetSection("ThirdPartyIntegrationConfigs").GetSection("WebHostRootUrl").Value + "License/" + licenseDirName + "/" + formModel.PublicAppRefNum + ".pdf" : licenseFilePath;
                            statusReq.StatusDate = DateTime.Now;
                            statusReq.IntegrationSource = "LABOUR";
                            statusReq.IsdeemedApproval = (formModel.AppActionType == 205 || formModel.AppActionType == 206) ? "true" : "false";
                            statusReq.appActionType = (AppActionTypeEnum)formModel.AppActionType;
                            statusReq.appActionLogId = appActionLogId;
                            var response = await _iInvestPunjabShareStatusService.ShareStatusToBusinessFirst(statusReq);
                            var projectSite = _context.ProjectSites.Where(x => x.ProjectSiteId == application.FirstOrDefault().ProjectSiteRefId).FirstOrDefault();
                            if (formModel.AppActionType == 200 || formModel.AppActionType == 206) //Approved Deemed
                            {
                                // Sent Email Notification 
                                TemplateResolveObjectionCaseLendedToOfficerNotificationViewModel mailTemplate = new TemplateResolveObjectionCaseLendedToOfficerNotificationViewModel()
                                {
                                    OfficerName = user.Where(x => x.UserId == formModel.Receiver_UserRefId).Select(x => x.FirstName).FirstOrDefault() + ' ' + user.Where(x => x.UserId == formModel.Receiver_UserRefId).Select(x => x.LastName).FirstOrDefault(),
                                    ApplicationPurposeTitle = EnumOps.GetEnumDescriptionName<ApplicationPurposeTypeEnum>(application.FirstOrDefault().ApplicationPurposeType.ToString()),
                                    ApplicationTypeTitle = EnumOps.GetEnumDescriptionName<ApplicationTypeEnum>(application.FirstOrDefault().ApplicationType.ToString()),
                                    EstablishmentName = projectSite.EstablishmentName,
                                    PublicApplicationRefNo = application.FirstOrDefault().PublicAppRefNum,
                                    StatusDescription = applicationAction.Remarks
                                };
                                var emailTemplateText = await _iNotificationManagerService.RenderToStringAsync("EmailTemplates/_templateObjectionResolvedAndCaseLendedToOfficerNotification", mailTemplate);
                                //var emailSendinfResponse = await _iNotificationManagerService.SendEmailSMTP(mailTemplate.OfficerName, user.Where(x => x.UserId == formModel.Receiver_UserRefId).Select(x => x.Email).FirstOrDefault(), "Department of Labour: Your application has been approved. (" + mailTemplate.PublicApplicationRefNo + ")", emailTemplateText);

                                // Sent SMS Notification To User
                                var smsTemplateText = $"We are pleased to inform you that, your Application No. {application.FirstOrDefault().PublicAppRefNum} is got Approved from Department of Labour.-PB Labour";
                                var templateId = "1407172725558167954";
                                await _iNotificationManagerService.InitiateNotification(projectSite.UserRefId,
                                new List<NotificationViewModel>()
                                {
                                new NotificationViewModel(){Body=smsTemplateText, NotificationMode=NotificationModeTypeEnum.MOBILE,NotificationPurpose=NotificationPurposeTypeEnum.INFO, Title=templateId}
                                });
                            }
                            else if (formModel.AppActionType == 404) // Objection
                            {
                                TemplateResolveObjectionCaseLendedToOfficerNotificationViewModel mailTemplate = new TemplateResolveObjectionCaseLendedToOfficerNotificationViewModel()
                                {
                                    OfficerName = user.Where(x => x.UserId == formModel.Receiver_UserRefId).Select(x => x.FirstName).FirstOrDefault() + ' ' + user.Where(x => x.UserId == formModel.Receiver_UserRefId).Select(x => x.LastName).FirstOrDefault(),
                                    ApplicationPurposeTitle = EnumOps.GetEnumDescriptionName<ApplicationPurposeTypeEnum>(application.FirstOrDefault().ApplicationPurposeType.ToString()),
                                    ApplicationTypeTitle = EnumOps.GetEnumDescriptionName<ApplicationTypeEnum>(application.FirstOrDefault().ApplicationType.ToString()),
                                    EstablishmentName = projectSite.EstablishmentName,
                                    PublicApplicationRefNo = application.FirstOrDefault().PublicAppRefNum,
                                    StatusDescription = applicationAction.Remarks,
                                    InvestPunjab_Ipin = application.FirstOrDefault().InvestPunjab_Ipin
                                };
                                var emailTemplateText = await _iNotificationManagerService.RenderToStringAsync("EmailTemplates/_templateObjectionResolvedAndCaseLendedToOfficerNotification", mailTemplate);
                                //var emailSendinfResponse = await _iNotificationManagerService.SendEmailSMTP(mailTemplate.OfficerName, user.Where(x => x.UserId == formModel.Receiver_UserRefId).Select(x => x.Email).FirstOrDefault(), "Department of Labour: The department has raised few objections, kindly examine the application form and resolved the objections. (" + mailTemplate.InvestPunjab_Ipin + ")", emailTemplateText);

                                // Sent SMS Notification To User
                                var smsTemplateText = $"Department of labour raised Objection on your Application No. {application.FirstOrDefault().PublicAppRefNum} . Please login at pbindustries.gov.in for resolve it.-PB  LABOUR";
                                var templateId = "1407172725521534794";
                                await _iNotificationManagerService.InitiateNotification(application.FirstOrDefault().ProjectSites.UserRefId,
                                new List<NotificationViewModel>()
                                {
                                    new NotificationViewModel(){Body=smsTemplateText, NotificationMode=NotificationModeTypeEnum.MOBILE,NotificationPurpose=NotificationPurposeTypeEnum.INFO, Title=templateId}
                                });
                            }
                            else if (formModel.AppActionType == 103 || formModel.AppActionType == 400 || formModel.AppActionType == 9) // Forwarded For further action
                            {
                                TemplateResolveObjectionCaseLendedToOfficerNotificationViewModel mailTemplate = new TemplateResolveObjectionCaseLendedToOfficerNotificationViewModel()
                                {
                                    OfficerName = user.Where(x => x.UserId == formModel.Receiver_UserRefId).Select(x => x.FirstName).FirstOrDefault() + ' ' + user.Where(x => x.UserId == formModel.Receiver_UserRefId).Select(x => x.LastName).FirstOrDefault(),
                                    ApplicationPurposeTitle = EnumOps.GetEnumDescriptionName<ApplicationPurposeTypeEnum>(application.FirstOrDefault().ApplicationPurposeType.ToString()),
                                    ApplicationTypeTitle = EnumOps.GetEnumDescriptionName<ApplicationTypeEnum>(application.FirstOrDefault().ApplicationType.ToString()),
                                    EstablishmentName = projectSite.EstablishmentName,
                                    PublicApplicationRefNo = application.FirstOrDefault().PublicAppRefNum,
                                    StatusDescription = applicationAction.Remarks,
                                    InvestPunjab_Ipin = application.FirstOrDefault().InvestPunjab_Ipin
                                };
                                var emailTemplateText = await _iNotificationManagerService.RenderToStringAsync("EmailTemplates/_templateObjectionResolvedAndCaseLendedToOfficerNotification", mailTemplate);
                            }

                            //else
                            //{
                            //    //transaction.Rollback();
                            //    resp.HasError = true;
                            //    resp.ErrorDesc = "Something wrong with approval files";
                            //}
                            resp.HasError = false;
                        }
                    }
                }
                catch (Exception ex)
                {
                    //transaction.Rollback();
                    resp.HasError = true;
                    resp.ErrorDesc = ex.Message;
                    throw ex;
                }
            return resp;
        }


        public async Task<GenericResponseTemplateModel<bool>> UpgradeApplicationCircle(LatestCircleInfoViewModel requestData)
        {
            GenericResponseTemplateModel<bool> resp = new GenericResponseTemplateModel<bool>();
            try
            {
                var applicationAction = await _context.ApplicationActions.Where(x => x.ApplicationRefId == requestData.AppRefId).FirstOrDefaultAsync();

                int totalDays = GetWorkingDays(applicationAction.ActionDate, DateTimeOffset.UtcNow.Date);
                int holidays = _context.Holidays.Where(x => x.HolidayDate >= applicationAction.ActionDate && x.HolidayDate <= DateTimeOffset.UtcNow.Date && x.IsWeekend == false).Count();
                int totalWorkingDays = 0;
                if (holidays > 0)
                {
                    totalWorkingDays = (totalDays - holidays);
                }
                else
                {
                    totalWorkingDays = totalDays;
                }

                var end = DateTime.Now;
                var start = applicationAction.ActionDate;
                var weekend = new[] { DayOfWeek.Saturday, DayOfWeek.Sunday };

                var totalWorkingHours = Enumerable.Range(0, Convert.ToInt32(end.Subtract(start).TotalHours))
                     .Count(offset => !weekend.Contains(start.AddHours(offset).DayOfWeek));

                if (applicationAction != null)
                {
                    applicationAction.ActionTakenDaysCount = totalWorkingDays;
                    applicationAction.ActionTakenHoursCount = Convert.ToInt64(totalWorkingHours);
                    applicationAction.ActionDate = DateTime.Now;

                    applicationAction.Receiver_ProfileRefId = requestData.UserProfileId;
                    applicationAction.Receiver_UserRefId = requestData.UserId;
                    applicationAction.ReceiverRoleId = requestData.RoleId;

                    applicationAction.Remarks = "File transferd from old circle/jurisdiction system to new system.";

                    _iGR_ApplicationAction.Update(applicationAction);
                    await _iGR_ApplicationAction.SavechangeAsync();


                    var user = await _userManager.Users.Include(x => x.UserRoles).Where(x => x.Id == requestData.UserId).ToListAsync();

                    ApplicationActionLog applicationActionlogs = new ApplicationActionLog()
                    {
                        ActionDate = applicationAction.ActionDate,
                        ActionTakenDaysCount = applicationAction.ActionTakenDaysCount,
                        ActionTakenHoursCount = applicationAction.ActionTakenHoursCount,
                        AppActionType = (int)AppActionTypeEnum.FILE_TRANSFERRED,

                        Receiver_ProfileRefId = applicationAction.Receiver_ProfileRefId,
                        Receiver_UserRefId = applicationAction.Receiver_UserRefId,
                        ReceiverRoleId = applicationAction.ReceiverRoleId,

                        Sender_UserRefId = requestData.Sender_UserRefId,
                        SenderRoleId = user.FirstOrDefault().UserRoles.FirstOrDefault().RoleId,
                        Sender_ProfileRefId = requestData.Sender_UserProfileRefId,

                        Remarks = applicationAction.Remarks,

                        ApplicationRefId = applicationAction.ApplicationRefId,

                        Checklist_Json = applicationAction.Checklist_Json,
                        Checklist_IsAllAgreed = applicationAction.Checklist_IsAllAgreed,
                        Checklist_FieldObjections = applicationAction.Checklist_FieldObjections,
                        Checklist_DocObjections = applicationAction.Checklist_DocObjections,
                        IsDocumentUploaded = applicationAction.IsDocumentUploaded,
                        AppDocumentRefId = applicationAction.AppDocumentRefId,
                        IpAddress = "Manual",
                        Latitude = "Manual",
                        Longitude = "Manual"
                    };

                    _iGR_ApplicationActionLog.Insert(applicationActionlogs);
                    await _iGR_ApplicationActionLog.SavechangeAsync();


                    var application = _context.Applications.Include(x => x.ProjectSites).Where(x => x.AppId == requestData.AppRefId && x.IsDeleted == false).FirstOrDefault();

                    CircleUpgradationHistoryLog circleUpgradationHistoryLog = new CircleUpgradationHistoryLog()
                    {
                        AppRefId = requestData.AppRefId,
                        CircleType = requestData.CircleType,
                        CreatedOn = DateTime.Now,
                        NewCircleId = requestData.CircleId,
                        NewUserProfileId = requestData.UserProfileId,
                        NewUserRefId = requestData.UserId,
                        NewUserRoleId = requestData.RoleId,
                        OldUserProfileId = requestData.Sender_UserProfileRefId,
                        OldUserRefId = requestData.Sender_UserRefId,
                        OldUserRoleId = user.FirstOrDefault().UserRoles.FirstOrDefault().RoleId,
                    };


                    if (requestData.CircleType == CircleTypeEnum.LABOUR_CIRCLE)
                    {
                        circleUpgradationHistoryLog.OldCircleId = application.ProjectSites.LabourCircleRefId;
                        application.ProjectSites.LabourCircleRefId = requestData.CircleId;
                    }
                    else if (requestData.CircleType == CircleTypeEnum.FACTORY_CIRCLE)
                    {
                        circleUpgradationHistoryLog.OldCircleId = application.ProjectSites.FactoryCircleRefId;
                        application.ProjectSites.FactoryCircleRefId = requestData.CircleId;
                    }


                    _context.ProjectSites.Update(application.ProjectSites);
                    _context.SaveChanges();

                    _context.Add<CircleUpgradationHistoryLog>(circleUpgradationHistoryLog);
                    _context.SaveChanges();

                    //await _iAppTimeLineManagerService.SeedTimeLineData(application.AppId, 0);

                    if (application.IsTimeLineFlow)
                    {
                        await _iAppTimeLineManagerService.SeedTimeLineData(application.AppId, 0);
                    }
                }
            }
            catch (Exception ex)
            {
                resp.HasError = true;
                resp.ErrorDesc = ex.Message;
            }
            return resp;
        }

        public int GetWorkingDays(DateTime from, DateTime to)
        {
            var totalDays = 0;
            for (var date = from; date <= to; date = date.AddDays(1))
            {
                if (date.DayOfWeek != DayOfWeek.Saturday
                    && date.DayOfWeek != DayOfWeek.Sunday)
                    totalDays++;
            }
            return totalDays;
        }
        public async Task<GenericFormModel<List<RoleWiseAllowedActionCodeViewModel>>> GetProcessApplicationDetail(string userId, int currentActionCode, ApplicationTypeEnum applicationType)
        {
            GenericFormModel<List<RoleWiseAllowedActionCodeViewModel>> genericFormModel = new GenericFormModel<List<RoleWiseAllowedActionCodeViewModel>>();
            try
            {
                List<StoreProcedureParm> storeProcedureParms = new List<StoreProcedureParm>()
                {
                    new StoreProcedureParm (){ ParmName="UserId", ParmValue=userId.ToString(), isNumber=false},
                    new StoreProcedureParm (){ ParmName="CurrentActionCode", ParmValue=currentActionCode.ToString(), isNumber=true},
                    new StoreProcedureParm (){ ParmName="ApplicationType", ParmValue = EnumOps.GetEnumValue<ApplicationTypeEnum>(applicationType.ToString()), isNumber=true}
                };
                genericFormModel.FormModel = await _iGeneric_SP_Repository.CallStoreProcedureReaderAsync<RoleWiseAllowedActionCodeViewModel>("sp_RoleWiseAllowedActionCodes", storeProcedureParms);
            }
            catch (Exception ex)
            {
                genericFormModel.HasError = true;
                genericFormModel.ErrorDesc = ex.Message;
            }
            return genericFormModel;
        }
        public async Task<GenericResponseTemplateModel<ProcessApplicationUsersDetailViewModel>> GetUserByActionCode(int actionCode, string userId, Int64 appRefId, ApplicationTypeEnum applicationType, string precheckCode)
        {
            GenericResponseTemplateModel<ProcessApplicationUsersDetailViewModel> genericFormModel = new GenericResponseTemplateModel<ProcessApplicationUsersDetailViewModel>()
            {
                ResponseDataModel = new ProcessApplicationUsersDetailViewModel() { IsPreCheckPassed = true }
            };
            try
            {
                List<StoreProcedureParm> storeProcedureParms = null;
                if (precheckCode!="null" && precheckCode != String.Empty && precheckCode != null)
                {
                    storeProcedureParms = new List<StoreProcedureParm>()
                    {
                        new StoreProcedureParm (){ ParmName="ActionCode", ParmValue=actionCode.ToString(), isNumber=true},
                        new StoreProcedureParm (){ ParmName="UserRefId", ParmValue=userId.ToString(), isNumber=false},
                        new StoreProcedureParm (){ ParmName="AppRefId", ParmValue=appRefId.ToString(), isNumber=true},
                        new StoreProcedureParm (){ ParmName="PrecheckCode", ParmValue = precheckCode.ToString(), isNumber=false}
                    };
                    var preCheckResp = await _iGeneric_SP_Repository.CallStoreProcedureReaderAsync<PreCheckViewModel>("sp_TimeLine_AppPreCheckCode", storeProcedureParms);
                    genericFormModel.ResponseDataModel.IsPreCheckPassed = preCheckResp.FirstOrDefault().IsPreCheckPassed;
                }

                if (genericFormModel.ResponseDataModel.IsPreCheckPassed)
                {
                    Application application = _context.Applications.Where(x => x.AppId == appRefId && x.IsDeleted == false).FirstOrDefault();
                    storeProcedureParms = new List<StoreProcedureParm>()
                {
                        new StoreProcedureParm (){ ParmName="ActionCode", ParmValue=actionCode.ToString(), isNumber=true},
                        new StoreProcedureParm (){ ParmName="UserId", ParmValue=userId.ToString(), isNumber=false},
                        new StoreProcedureParm (){ ParmName="AppRefId", ParmValue=appRefId.ToString(), isNumber=true},
                        new StoreProcedureParm (){ ParmName="ApplicationType", ParmValue=EnumOps.GetEnumValue<ApplicationTypeEnum>(applicationType.ToString()), isNumber=true},
                        new StoreProcedureParm (){ ParmName="DefaultRoleName", ParmValue=null, isNumber=false}

                };
                    if (application.IsTimeLineFlow)
                    {
                        genericFormModel.ResponseDataModel.ProcessApplicationUsers = await _iGeneric_SP_Repository.CallStoreProcedureReaderAsync<ProcessApplicationUsersViewModel>("sp_TimeLine_GetNextUserByAction", storeProcedureParms);
                    }
                    else
                    {
                        storeProcedureParms = storeProcedureParms.Where(x => x.ParmName != "DefaultRoleName").ToList();
                        genericFormModel.ResponseDataModel.ProcessApplicationUsers = await _iGeneric_SP_Repository.CallStoreProcedureReaderAsync<ProcessApplicationUsersViewModel>("sp_GetNextUserByAction", storeProcedureParms);
                    }
                }

                
            }
            catch (Exception ex)
            {
                genericFormModel.HasError = true;
                genericFormModel.ErrorDesc = ex.Message;
            }
            return genericFormModel;
        }

        public async Task<GenericResponseTemplateModel<List<NotingLogsViewModel>>> GetApplicationNotingLogsByAppId(Int64 appRefId, string userId)
        {
            GenericResponseTemplateModel<List<NotingLogsViewModel>> genericFormModel = new GenericResponseTemplateModel<List<NotingLogsViewModel>>();
            try
            {
                List<StoreProcedureParm> storeProcedureParms = new List<StoreProcedureParm>()
                {
                    new StoreProcedureParm (){ ParmName="AppRefId", ParmValue=appRefId.ToString(), isNumber=true},
                    new StoreProcedureParm (){ ParmName="UserId", ParmValue=userId.ToString(), isNumber=false}
                };
                genericFormModel.ResponseDataModel = await _iGeneric_SP_Repository.CallStoreProcedureReaderAsync<NotingLogsViewModel>("sp_GetApplicationNotingLogs", storeProcedureParms);
            }
            catch (Exception ex)
            {
                genericFormModel.HasError = true;
                genericFormModel.ErrorDesc = ex.Message;
            }
            return genericFormModel;
        }

        public async Task<GenericServiceResultTemplate> DeleteTempCreatedLicense(ApplicationActionViewModel formModel)
        {
            GenericServiceResultTemplate genericServiceResultTemplate = new GenericServiceResultTemplate();
            try
            {
                var tempFilePath = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "TempFiles")).Root;
                File.Delete(tempFilePath + formModel.PdfNameGUID + ".pdf");
                genericServiceResultTemplate.HasException = false;
                genericServiceResultTemplate.Exceptions = null;
            }
            catch (Exception ex)
            {
                genericServiceResultTemplate.HasException = true;
                genericServiceResultTemplate.Exceptions = ex;
            }
            return genericServiceResultTemplate;
        }
        public async Task<GenericServiceResultTemplate> RecordAppFeeSuccessTransactionEntry(Int64 appId, Int64 appFeeTransactionRefId, int paymentPartCounter, int paymentBatchCounter, int hasAllPartsPaid)
        {
            GenericServiceResultTemplate genericServiceResultTemplate = new GenericServiceResultTemplate();
            try
            {
                List<StoreProcedureParm> storeProcedureParms = new List<StoreProcedureParm>()
                {
                    new StoreProcedureParm (){ ParmName="appId", ParmValue=appId.ToString(), isNumber=true},
                    new StoreProcedureParm (){ ParmName="appFeeTransactionRefId", ParmValue=appFeeTransactionRefId.ToString(), isNumber=true},
                    new StoreProcedureParm (){ ParmName="paymentPartCounter", ParmValue=paymentPartCounter.ToString(), isNumber=true},
                    new StoreProcedureParm (){ ParmName="paymentBatchCounter", ParmValue=paymentBatchCounter.ToString(), isNumber=true},
                    new StoreProcedureParm (){ ParmName="hasAllPartsPaid", ParmValue=hasAllPartsPaid.ToString(), isNumber=true}
                };
                await _iGeneric_SP_Repository.CallStoreProcedureReaderAsync<LendingOfficerDetailsViewModel>("sp_Application_Record_AppFee_Success_Transaction_Entry", storeProcedureParms);
            }
            catch (Exception ex)
            {
                genericServiceResultTemplate.HasException = true;
                genericServiceResultTemplate.Exceptions = ex;
            }
            return genericServiceResultTemplate;
        }
        public async Task<UserProfile> GetApplicantProfileDetailByAppId(Int64 appRefId)
        {
            var parentWithChildObject = await _iGR_Application
                        .GetAsync(x => x.AppId == appRefId,  //Conditions         
                          null,                //Orders          
                          x => x.ProjectSites)      //Includes
                        .ConfigureAwait(false);
            return await _iAuthService.GetUserProfileByUserId(parentWithChildObject.FirstOrDefault().ProjectSites.UserRefId);
        }

        #region Application details for pdf operation
        public async Task<GenericListModel<ApplicationFullDetailsViewModel>> GetEstablishmentApplicationCertificateDetailsByAppRefId(Int64 appRefId)
        {
            GenericListModel<ApplicationFullDetailsViewModel> genericListModel = new GenericListModel<ApplicationFullDetailsViewModel>();
            try
            {
                List<StoreProcedureParm> storeProcedureParms = new List<StoreProcedureParm>()
                {
                    new StoreProcedureParm (){ ParmName="AppRefId", ParmValue=appRefId.ToString(), isNumber=true}
                };
                genericListModel.ListData = await _iGeneric_SP_Repository.CallStoreProcedureReaderAsync<ApplicationFullDetailsViewModel>("sp_GetEstablishmentPdfDetailsByAppRefId", storeProcedureParms);
            }
            catch (Exception ex)
            {
                genericListModel.HasError = true;
                genericListModel.ErrorDesc = ex.Message;
            }
            return genericListModel;
        }
        public async Task<GenericListModel<ApplicationFullDetailsViewModel>> GetCommonLicenceApplicationCertificateDetailsByAppRefId(Int64 appRefId)
        {
            GenericListModel<ApplicationFullDetailsViewModel> genericListModel = new GenericListModel<ApplicationFullDetailsViewModel>();
            try
            {
                List<StoreProcedureParm> storeProcedureParms = new List<StoreProcedureParm>()
                {
                    new StoreProcedureParm (){ ParmName="AppRefId", ParmValue=appRefId.ToString(), isNumber=true}
                };
                genericListModel.ListData = await _iGeneric_SP_Repository.CallStoreProcedureReaderAsync<ApplicationFullDetailsViewModel>("sp_GetCommonLicencePdfDetailsByAppRefId", storeProcedureParms);
            }
            catch (Exception ex)
            {
                genericListModel.HasError = true;
                genericListModel.ErrorDesc = ex.Message;
            }
            return genericListModel;
        }
        public async Task<GenericListModel<ContractLabourPdfDetailsViewModel>> GetContractLabourApplicationCertificateDetailsByAppRefId(Int64 appRefId)
        {
            GenericListModel<ContractLabourPdfDetailsViewModel> genericListModel = new GenericListModel<ContractLabourPdfDetailsViewModel>();
            try
            {
                List<StoreProcedureParm> storeProcedureParms = new List<StoreProcedureParm>()
                {
                    new StoreProcedureParm (){ ParmName="AppRefId", ParmValue=appRefId.ToString(), isNumber=true}
                };
                genericListModel.ListData = await _iGeneric_SP_Repository.CallStoreProcedureReaderAsync<ContractLabourPdfDetailsViewModel>("sp_GetContractLabourPdfDetailsByAppRefId", storeProcedureParms);
            }
            catch (Exception ex)
            {
                genericListModel.HasError = true;
                genericListModel.ErrorDesc = ex.Message;
            }
            return genericListModel;
        }
        public async Task<GenericListModel<BuildingPlanHudPdfDetailsViewModel>> GetBuildingPlanHUDCertificateDetailsByAppRefId(Int64 appRefId)
        {
            GenericListModel<BuildingPlanHudPdfDetailsViewModel> genericListModel = new GenericListModel<BuildingPlanHudPdfDetailsViewModel>();
            try
            {
                List<StoreProcedureParm> storeProcedureParms = new List<StoreProcedureParm>()
                {
                    new StoreProcedureParm (){ ParmName="AppRefId", ParmValue=appRefId.ToString(), isNumber=true}
                };
                genericListModel.ListData = await _iGeneric_SP_Repository.CallStoreProcedureReaderAsync<BuildingPlanHudPdfDetailsViewModel>("sp_GetBuildingPlanHudPdfDetailsByAppRefId", storeProcedureParms);
            }
            catch (Exception ex)
            {
                genericListModel.HasError = true;
                genericListModel.ErrorDesc = ex.Message;
            }
            return genericListModel;
        }
        public async Task<GenericListModel<ShopLicencePdfDetailsViewModel>> GetShopLicenceCertificateDetailsByAppRefId(Int64 appRefId)
        {
            GenericListModel<ShopLicencePdfDetailsViewModel> genericListModel = new GenericListModel<ShopLicencePdfDetailsViewModel>();
            try
            {
                List<StoreProcedureParm> storeProcedureParms = new List<StoreProcedureParm>()
                {
                    new StoreProcedureParm (){ ParmName="AppRefId", ParmValue=appRefId.ToString(), isNumber=true}
                };
                genericListModel.ListData = await _iGeneric_SP_Repository.CallStoreProcedureReaderAsync<ShopLicencePdfDetailsViewModel>("sp_GetShopLicencePdfDetailsByAppRefId", storeProcedureParms);
            }
            catch (Exception ex)
            {
                genericListModel.HasError = true;
                genericListModel.ErrorDesc = ex.Message;
            }
            return genericListModel;
        }
        #endregion

        public async Task<List<AppFileUploadInfoViewModel>> InitiateAppFileInfoWithRoleId(long AppId, string userId, int currentActionCode, int allowedActionCode, Int64 applicationaType, bool isTimeLineFlow, Int64 applicationActionLogId)
        {
            List<AppFileUploadInfoViewModel> filesDetail = new List<AppFileUploadInfoViewModel>();
            try
            {
                if (isTimeLineFlow)
                {
                    List<StoreProcedureParm> storeProcedureParms = new List<StoreProcedureParm>()
                    {
                        new StoreProcedureParm (){ ParmName="AppId", ParmValue= AppId.ToString(), isNumber=true },
                        new StoreProcedureParm (){ ParmName="AllowedActionCode", ParmValue= allowedActionCode.ToString(), isNumber=true},
                        new StoreProcedureParm (){ ParmName="ApplicationActionLogId", ParmValue= applicationActionLogId.ToString(), isNumber=true},
                        new StoreProcedureParm (){ ParmName="UserRefId", ParmValue= userId, isNumber=false}
                    };

                    filesDetail = await _iGeneric_SP_Repository.CallStoreProcedureReaderAsync<AppFileUploadInfoViewModel>("dbo.sp_TimeLine_InitiateAppFileInfoWithRoleId", storeProcedureParms);
                }
                else
                {
                    List<StoreProcedureParm> storeProcedureParms = new List<StoreProcedureParm>()
                    {
                        new StoreProcedureParm (){ ParmName="AppId", ParmValue= AppId.ToString(), isNumber=true },
                        new StoreProcedureParm (){ ParmName="UserId", ParmValue= userId.ToString(), isNumber=false },
                        new StoreProcedureParm (){ ParmName="CurrentActionCode", ParmValue= currentActionCode.ToString(), isNumber=true},
                        new StoreProcedureParm (){ ParmName="AllowedActionCode", ParmValue= allowedActionCode.ToString(), isNumber=true},
                        new StoreProcedureParm (){ ParmName="ApplicationaType", ParmValue= applicationaType.ToString(), isNumber=true}
                    };

                    filesDetail = await _iGeneric_SP_Repository.CallStoreProcedureReaderAsync<AppFileUploadInfoViewModel>("dbo.sp_InitiateAppFileInfoWithRoleId", storeProcedureParms);
                }

                if (filesDetail.Count() > 0)
                {
                    foreach (var file in filesDetail)
                    {
                        if (file.AlreadyUploaded != null)
                        {
                            string[] alreadyUploadedFileInfo = file.AlreadyUploaded.Split(',');
                            if (alreadyUploadedFileInfo.Length > 0)
                            {
                                file.AlreadyUploadedInfo = new List<ViewModels.AppFileAlreadyFileUploadedViewModel>();
                                foreach (var alreadyFile in alreadyUploadedFileInfo)
                                {
                                    string[] fileInfo = alreadyFile.Split('=');
                                    file.AlreadyUploadedInfo.Add(new ViewModels.AppFileAlreadyFileUploadedViewModel()
                                    {
                                        FileName = fileInfo[0],
                                        FileUploadOn = Convert.ToDateTime(fileInfo[1])
                                    });
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {

            }
            return filesDetail;
        }

        public async Task<GenericFormModel<ProjectSiteViewModel>> GetProjectSiteByAppRefId(Int64 appRefId)
        {
            GenericFormModel<ProjectSiteViewModel> genericFormModel = new GenericFormModel<ProjectSiteViewModel>();
            try
            {
                List<StoreProcedureParm> storeProcedureParms = new List<StoreProcedureParm>()
                {
                    new StoreProcedureParm (){ ParmName="AppRefId", ParmValue=appRefId.ToString(), isNumber=true}
                };
                var projectSiteList = await _iGeneric_SP_Repository.CallStoreProcedureReaderAsync<ProjectSiteViewModel>("sp_GetProjectSiteIdByAppRefId", storeProcedureParms);
                if(projectSiteList!=null && projectSiteList.Count() > 0)
                {
                    genericFormModel.FormModel = projectSiteList.FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                genericFormModel.HasError = true;
                genericFormModel.ErrorDesc = ex.Message;
            }
            return genericFormModel;
        }

        public async Task<GenericResponseTemplateModel<ApplicationAction>> GetCurrentStatusByAppRefId(Int64 appRefId, ApplicationTypeEnum applicationType)
        {
            GenericResponseTemplateModel<ApplicationAction> genericServiceResultTemplate = new GenericResponseTemplateModel<ApplicationAction>()
            {
                HasError = false,
                ErrorDesc = "",
            };
            try
            {
                if(applicationType == ApplicationTypeEnum.BUILDING_PLAN_PSIEC)
                {
                    var parentWithChildObject = await _context.ApplicationAction_ParallelProcesses.Where(x => x.ApplicationRefId == appRefId && x.IsAlive == true).OrderByDescending(x=>x.AppActionParallelProcessId).FirstOrDefaultAsync();
                    genericServiceResultTemplate.ResponseDataModel = JsonConvert.DeserializeObject<ApplicationAction>(JsonConvert.SerializeObject(parentWithChildObject));
                }
                else
                {
                    var parentWithChildObject = await _iGR_ApplicationAction.GetAsync(x => x.ApplicationRefId == appRefId).ConfigureAwait(false);
                    genericServiceResultTemplate.ResponseDataModel = parentWithChildObject.FirstOrDefault();
                }
                
                if (genericServiceResultTemplate.ResponseDataModel != null)
                {
                    var actionTypeResp = await _iGR_ApplicationActionCode.GetAsync(x => x.ActionCode == genericServiceResultTemplate.ResponseDataModel.AppActionType).ConfigureAwait(false);
                    genericServiceResultTemplate.ResponseDataModel.ActionPublicName = actionTypeResp.FirstOrDefault().ActionPublicName;
                }
            }
            catch (Exception ex)
            {
                genericServiceResultTemplate.HasError = true;
                genericServiceResultTemplate.ErrorDesc = ex.Message;
            }
            return genericServiceResultTemplate;
        }

        public async Task<GenericResponseTemplateModel<ApplicationActionCode>> GetCurrentStatusCodeByAppActionType (Int64 actionCode)
        {
            GenericResponseTemplateModel<ApplicationActionCode> genericServiceResultTemplate = new GenericResponseTemplateModel<ApplicationActionCode>()
            {
                HasError = false,
                ErrorDesc = "",
            };
            try
            {
                var parentWithChildObject = await _iGR_ApplicationActionCode.GetAsync(x => x.ActionCode == actionCode).ConfigureAwait(false);  // ApplicationActionCodes.Where(x => x.ActionCode == actionCode).FirstOrDefault();
                genericServiceResultTemplate.ResponseDataModel = parentWithChildObject.FirstOrDefault();
            }
            catch (Exception ex)
            {
                genericServiceResultTemplate.HasError = true;
                genericServiceResultTemplate.ErrorDesc = ex.Message;
            }
            return genericServiceResultTemplate;
        }

        public async Task<GenericResponseTemplateModel<int>> IncreasePaymentBatchCounter(Int64 appId)
        {
            GenericResponseTemplateModel<int> genericResponseTemplate = new GenericResponseTemplateModel<int>() {ErrorDesc="", HasError=false, ResponseDataModel=0 };
            try
            {
                var application = await _iGR_Application.GetAsync(x => x.AppId == appId).ConfigureAwait(false);
                //payment application.FirstOrDefault().PaymentBatchCounter;
                application.FirstOrDefault().PaymentBatchCounter = application.FirstOrDefault().PaymentBatchCounter + 1;
                _iGR_Application.Update(application.FirstOrDefault());
                await _iGR_Application.SavechangeAsync();
                genericResponseTemplate.ResponseDataModel = application.FirstOrDefault().PaymentBatchCounter;
            }
            catch (Exception ex)
            {
                genericResponseTemplate.HasError = true;
                genericResponseTemplate.ErrorDesc= ex.Message;
            }
            return genericResponseTemplate;
        }

        public async Task<GenericResponseTemplateModel<string>> GetIndustryUserDashboardUrl(string userId)
        {
            GenericResponseTemplateModel<string> genericResponseTemplate = new GenericResponseTemplateModel<string>() { ErrorDesc = "", HasError = false, ResponseDataModel = "" };
            try
            {
                genericResponseTemplate.ResponseDataModel = Configuration.GetSection("ThirdPartyIntegrationConfigs").GetSection("InvestPunjab").GetSection("BackToInvesterDashboardURL").Value;
                //var requestString = await _context.BusinessFirst_RequestLogs.Where(x => x.NativeUserId == userId).Select(x => x.RequestString).FirstOrDefaultAsync();
                //string[] stringData = requestString.Split("|");
                //if(stringData != null)
                //{
                //    genericResponseTemplate.ResponseDataModel = stringData[6] + '|' + stringData[7];
                //}
                //else
                //{
                //    genericResponseTemplate.ResponseDataModel = null;
                //}
            }
            catch (Exception ex)
            {
                genericResponseTemplate.HasError = true;
                genericResponseTemplate.ErrorDesc = ex.Message;
            }
            return genericResponseTemplate;
        }

        public async Task<GenericResponseTemplateModel<ApplicationAction>> GetCurrentStatusByNativeAppId(Int64 nativeAppId)
        {
            GenericResponseTemplateModel<ApplicationAction> genericServiceResultTemplate = new GenericResponseTemplateModel<ApplicationAction>()
            {
                HasError = false,
                ErrorDesc = "",
            };
            try
            {
                var parentWithChildObject = await _iGR_ApplicationAction.GetAsync(x => x.ApplicationRefId == nativeAppId).ConfigureAwait(false);
                genericServiceResultTemplate.ResponseDataModel = parentWithChildObject.FirstOrDefault();
            }
            catch (Exception ex)
            {
                genericServiceResultTemplate.HasError = true;
                genericServiceResultTemplate.ErrorDesc = ex.Message;
            }
            return genericServiceResultTemplate;
        }

        public async Task<GenericServiceResultTemplate> TransferApplicationAction(ApplicationTransferParmsViewModal formModel)
        {
            GenericServiceResultTemplate genericServiceResultTemplate = new GenericServiceResultTemplate();
            //using (IDbContextTransaction transaction = _context.Database.BeginTransaction())
            //{
            try
            {
                var applicationAction = await _context.ApplicationActions.Where(x => x.ApplicationRefId == formModel.AppRefId).FirstOrDefaultAsync();

                int totalDays = GetWorkingDays(applicationAction.ActionDate, DateTimeOffset.UtcNow.Date);
                int holidays = _context.Holidays.Where(x => x.HolidayDate >= applicationAction.ActionDate && x.HolidayDate <= DateTimeOffset.UtcNow.Date && x.IsWeekend == false).Count();
                int totalWorkingDays = 0;
                if (holidays > 0)
                {
                    totalWorkingDays = (totalDays - holidays);
                }
                else
                {
                    totalWorkingDays = totalDays;
                }

                var end = DateTime.Now;
                var start = applicationAction.ActionDate;
                var weekend = new[] { DayOfWeek.Saturday, DayOfWeek.Sunday };

                var totalWorkingHours = Enumerable.Range(0, Convert.ToInt32(end.Subtract(start).TotalHours))
                     .Count(offset => !weekend.Contains(start.AddHours(offset).DayOfWeek));

                if (applicationAction != null)
                {
                    applicationAction.ActionTakenDaysCount = totalWorkingDays;
                    applicationAction.ActionTakenHoursCount = Convert.ToInt64(totalWorkingHours);
                    applicationAction.ActionDate = DateTime.Now;

                    applicationAction.Receiver_ProfileRefId = formModel.Receiver_UserProfileRefId;
                    applicationAction.Receiver_UserRefId = formModel.Receiver_UserRefId;
                    applicationAction.ReceiverRoleId = formModel.Receiver_RoleRefId;

                    applicationAction.IpAddress = formModel.IpAddress.ToString();
                    applicationAction.Latitude = formModel.Latitude.ToString();
                    applicationAction.Longitude = formModel.Longitude.ToString();

                    //applicationAction.Remarks = formModel.Remarks;

                    _iGR_ApplicationAction.Update(applicationAction);
                    await _iGR_ApplicationAction.SavechangeAsync();

                    
                    var user = await _userManager.Users.Include(x => x.UserRoles).Where(x => x.Id == formModel.Sender_UserRefId).ToListAsync();

                    ApplicationActionLog applicationActionlogs = new ApplicationActionLog()
                    {
                        ActionDate = applicationAction.ActionDate,
                        ActionTakenDaysCount = applicationAction.ActionTakenDaysCount,
                        ActionTakenHoursCount = applicationAction.ActionTakenHoursCount,
                        AppActionType = (int)AppActionTypeEnum.FILE_TRANSFERRED,

                        Receiver_ProfileRefId = applicationAction.Receiver_ProfileRefId,
                        Receiver_UserRefId = applicationAction.Receiver_UserRefId,
                        ReceiverRoleId = applicationAction.ReceiverRoleId,

                        Sender_UserRefId = formModel.Sender_UserRefId,
                        SenderRoleId = user.FirstOrDefault().UserRoles.FirstOrDefault().RoleId,
                        Sender_ProfileRefId = formModel.Sender_UserProfileRefId,

                        Remarks = formModel.Remarks,

                        ApplicationRefId = applicationAction.ApplicationRefId,

                        Checklist_Json = applicationAction.Checklist_Json,
                        Checklist_IsAllAgreed = applicationAction.Checklist_IsAllAgreed,
                        Checklist_FieldObjections = applicationAction.Checklist_FieldObjections,
                        Checklist_DocObjections = applicationAction.Checklist_DocObjections,
                        IsDocumentUploaded = applicationAction.IsDocumentUploaded,
                        AppDocumentRefId = applicationAction.AppDocumentRefId,

                        IpAddress = formModel.IpAddress.ToString(),
                        Latitude = formModel.Latitude.ToString(),
                        Longitude = formModel.Longitude.ToString()
                    };

                    _iGR_ApplicationActionLog.Insert(applicationActionlogs);
                    await _iGR_ApplicationActionLog.SavechangeAsync();



                    var application = _context.Applications.Where(x => x.AppId == formModel.AppRefId && x.IsDeleted == false).FirstOrDefault();
                    /*project site update*/
                    var projectSite = _context.ProjectSites.Where(x => x.ProjectSiteId == application.ProjectSiteRefId).FirstOrDefault();
                    if (projectSite != null)
                    {
                        if (formModel.FactoryCircleId > 0)
                        {
                            projectSite.FactoryCircleRefId = formModel.FactoryCircleId;
                        }
                        if (formModel.LabourCircleId > 0)
                        {
                            projectSite.LabourCircleRefId = formModel.LabourCircleId;
                        }
                        if (formModel.AlcCircleId > 0)
                        {
                            projectSite.AlcCircleRefId = formModel.AlcCircleId;
                        }
                        _context.Update<ProjectSite>(projectSite);
                        await _context.SaveChangesAsync();
                    }
                    /*project site logs update*/
                    var projectSitelogs = _context.ProjectSiteLogs.Where(x => x.ProjectSiteRefId == projectSite.ProjectSiteId && x.ProjectSiteVersion == projectSite.ProjectSiteVersion).FirstOrDefault();
                    if (projectSitelogs != null)
                    {
                        if (formModel.FactoryCircleId > 0)
                        {
                            projectSitelogs.FactoryCircleRefId = formModel.FactoryCircleId;
                        }
                        if (formModel.LabourCircleId > 0)
                        {
                            projectSitelogs.LabourCircleRefId = formModel.LabourCircleId;
                        }
                        if (formModel.AlcCircleId > 0)
                        {
                            projectSitelogs.AlcCircleRefId = formModel.AlcCircleId;
                        }
                        _context.Update<ProjectSiteLog>(projectSitelogs);
                        await _context.SaveChangesAsync();
                    }

                    //await _iAppTimeLineManagerService.SeedTimeLineData(application.AppId, 0);
                    if (application.IsTimeLineFlow)
                    {
                        await _iAppTimeLineManagerService.SeedTimeLineData(application.AppId, 0);
                    }
                }
            }
            catch (Exception ex)
            {
                //transaction.Rollback();
                genericServiceResultTemplate.HasException = true;
                genericServiceResultTemplate.Exceptions = ex;
            }
            //}
            return genericServiceResultTemplate;
        }


        public async Task<GenericResponseTemplateModel<string>> GetLicenceNoDetailsByProjectSiteRefId(Int64 appId)
        {
            GenericResponseTemplateModel<string> genericResponseTemplate = new GenericResponseTemplateModel<string>() { ErrorDesc = "", HasError = false, ResponseDataModel = "" };
            try
            {
                List<StoreProcedureParm> storeProcedureParms = new List<StoreProcedureParm>()
                {
                    new StoreProcedureParm (){ ParmName="AppRefId", ParmValue=appId.ToString(), isNumber=true}
                };
                var licResp = await _iGeneric_SP_Repository.CallStoreProcedureReaderAsync<LicenceNoViewModel>("sp_GetLicenceDetailsByProjectSiteRefId", storeProcedureParms);
                if (licResp != null)
                {
                    genericResponseTemplate.ResponseDataModel = licResp.FirstOrDefault().LicenceNo;
                }
            }
            catch (Exception ex)
            {
                genericResponseTemplate.HasError = true;
                genericResponseTemplate.ErrorDesc = ex.Message;
            }
            return genericResponseTemplate;
        }

        public async Task<GenericFormModel<DofLicenceDetailsViewModel>> GetDofDetailsByLicenceNo(string licenceNo)
        {
            GenericFormModel<DofLicenceDetailsViewModel> genericFormModel = new GenericFormModel<DofLicenceDetailsViewModel>();
            try
            {
                List<StoreProcedureParm> storeProcedureParms = new List<StoreProcedureParm>()
                {
                    new StoreProcedureParm (){ ParmName="LicenceNo", ParmValue=licenceNo.ToString(), isNumber=false}
                };
                var resp = await _iGeneric_SP_Repository.CallStoreProcedureReaderAsync<DofLicenceDetailsViewModel>("GetDofDetailsByLicenceNo", storeProcedureParms);
                if (resp != null && resp.Count() > 0)
                {
                    genericFormModel.FormModel = resp.FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                genericFormModel.HasError = true;
                genericFormModel.ErrorDesc = ex.Message;
            }
            return genericFormModel;
        }

        public async Task<GenericFormModel<DofLicenceDetailsViewModel>> GetDofDetailsByDOFNo(string dofNumber)
        {
            GenericFormModel<DofLicenceDetailsViewModel> genericFormModel = new GenericFormModel<DofLicenceDetailsViewModel>();
            try
            {
                List<StoreProcedureParm> storeProcedureParms = new List<StoreProcedureParm>()
                {
                    new StoreProcedureParm (){ ParmName="DOFNumber", ParmValue=dofNumber.ToString(), isNumber=false}
                };
                var resp = await _iGeneric_SP_Repository.CallStoreProcedureReaderAsync<DofLicenceDetailsViewModel>("sp_GetDofDetailsByDOFNo", storeProcedureParms);
                if (resp != null && resp.Count() > 0)
                {
                    genericFormModel.FormModel = resp.FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                genericFormModel.HasError = true;
                genericFormModel.ErrorDesc = ex.Message;
            }
            return genericFormModel;
        }

        public async Task<GenericResponseTemplateModel<ApplicationAdditionalDetailsViewModel>> GetApplicationAdditionalDetails(Int64 appRefId, ApplicationTypeEnum applicationType)
        {
            GenericResponseTemplateModel<ApplicationAdditionalDetailsViewModel> genericServiceResultTemplate = new GenericResponseTemplateModel<ApplicationAdditionalDetailsViewModel>()
            {
                HasError = false,
                ErrorDesc = "",
            };
            try
            {
                genericServiceResultTemplate.ResponseDataModel = new ApplicationAdditionalDetailsViewModel();

                var parentWithChildObject = await _iGR_Licence_Factory_GeneralDetail
                       .GetAsync(x => x.AppRefId == appRefId,  //Conditions         
                         null,                //Orders
                         x => x.Application)      //Includes
                       .ConfigureAwait(false);

                if (parentWithChildObject.FirstOrDefault().Application.ApplicationType == ApplicationTypeEnum.FACTORY_LICENCE && (parentWithChildObject.FirstOrDefault().Application.ApplicationPurposeType == ApplicationPurposeTypeEnum.RENEWAL_LICENCE || parentWithChildObject.FirstOrDefault().Application.ApplicationPurposeType == ApplicationPurposeTypeEnum.AMENDMENT_LICENCE))
                {
                    List<StoreProcedureParm> storeProcedureParms = new List<StoreProcedureParm>();
                    storeProcedureParms = new List<StoreProcedureParm>()
                    {
                        new StoreProcedureParm (){ ParmName="LicenceNo", ParmValue=parentWithChildObject.FirstOrDefault().OldLicenceNo, isNumber=false},
                    };
                    var respWelfareFund = await _iGeneric_SP_Repository.CallStoreProcedureReaderAsync<WelfareFundDetailsViewModel>("dbo.sp_GetWelfareFundDetailsByLicenceNo", storeProcedureParms);
                    if (respWelfareFund.Count() > 0)
                    {
                        genericServiceResultTemplate.ResponseDataModel.WelfareFundDetails = new List<WelfareFundDetailsViewModel>();
                        genericServiceResultTemplate.ResponseDataModel.WelfareFundDetails = respWelfareFund;
                    }

                    storeProcedureParms = new List<StoreProcedureParm>()
                    {
                        new StoreProcedureParm (){ ParmName="LicenceNo", ParmValue=parentWithChildObject.FirstOrDefault().OldLicenceNo, isNumber=false},
                    };
                    var respAnnualReturn = await _iGeneric_SP_Repository.CallStoreProcedureReaderAsync<AnnualReturnDetailsViewModel>("dbo.sp_GetAnnualReturnDetailsByLicenceNo", storeProcedureParms);
                    if (respAnnualReturn.Count() > 0)
                    {
                        genericServiceResultTemplate.ResponseDataModel.AnnualReturnDetails = new List<AnnualReturnDetailsViewModel>();
                        genericServiceResultTemplate.ResponseDataModel.AnnualReturnDetails = respAnnualReturn;
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

        public async Task<GenericResponseTemplateModel<ApplicationSpecificDataViewModel>> GetApplicationSpecificData(Int64 appRefId, ApplicationTypeEnum applicationType)
        {
            GenericResponseTemplateModel<ApplicationSpecificDataViewModel> genericServiceResultTemplate = new GenericResponseTemplateModel<ApplicationSpecificDataViewModel>()
            {
                HasError = false,
                ErrorDesc = "",
            };
            try
            {
                genericServiceResultTemplate.ResponseDataModel = new ApplicationSpecificDataViewModel();

                if (applicationType == ApplicationTypeEnum.FACTORY_LICENCE)
                {
                    var factoryLicence = await _context.Licence_Factory_GeneralDetails.Where(x => x.AppRefId == appRefId).FirstOrDefaultAsync();
                    genericServiceResultTemplate.ResponseDataModel.FactoryLicenceSpecificData = new FactoryLicenceSpecificDataViewModel
                    {
                        ExistingWorkers_MaxDuringYear = factoryLicence.Workers_MaxDuringYear,
                        ExistingPowerKW_Installed = factoryLicence.PowerKW_Installed
                    };
                }
            }
            catch (Exception ex)
            {
                genericServiceResultTemplate.HasError = true;
                genericServiceResultTemplate.ErrorDesc = ex.Message;
            }
            return genericServiceResultTemplate;
        }

        public async Task<GenericResponseTemplateModel<List<ApplicationClearencesCondition>>> GetApplicationClearencesConditions(long appRefId)
        {
            GenericResponseTemplateModel<List<ApplicationClearencesCondition>> genericServiceResultTemplate = new GenericResponseTemplateModel<List<ApplicationClearencesCondition>>()
            {
                HasError = false,
                ErrorDesc = "",
            };
            try
            {
                //genericServiceResultTemplate.ResponseDataModel = await _context.ApplicationClearencesConditions.Where(x => x.AppRefId == appRefId).ToListAsync();
            }
            catch (Exception ex)
            {
                genericServiceResultTemplate.HasError = true;
                genericServiceResultTemplate.ErrorDesc = ex.Message;
            }
            return genericServiceResultTemplate;
        }

        public async Task<GenericResponseTemplateModel<ShopLicenceDetailsForNightShiftPdfViewModel>> getShopLicenceLegacyDetail(string licenceNo)
        {
            GenericResponseTemplateModel<ShopLicenceDetailsForNightShiftPdfViewModel> genericServiceResultTemplate = new GenericResponseTemplateModel<ShopLicenceDetailsForNightShiftPdfViewModel>();
            try
            {
                List<StoreProcedureParm> storeProcedureParms = new List<StoreProcedureParm>()
                {
                    new StoreProcedureParm (){ ParmName="LicenceNo", ParmValue=licenceNo.ToString(), isNumber=false}
                };
                var resp = await _iGeneric_SP_Repository.CallStoreProcedureReaderAsync<ShopLicenceDetailsForNightShiftPdfViewModel>("GetShopLicenceDetailsForNightShiftByLicenceNo", storeProcedureParms);
                if(resp != null)
                {
                    genericServiceResultTemplate.ResponseDataModel = resp.FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                genericServiceResultTemplate.HasError = true;
                genericServiceResultTemplate.ErrorDesc = ex.Message;
            }
            return genericServiceResultTemplate;
        }

        public async Task<GenericResponseTemplateModel<FactoryLicenceDetailsForNightShiftPdfViewModel>> getFactoryLicenceLegacyDetail(string licenceNo)
        {
            GenericResponseTemplateModel<FactoryLicenceDetailsForNightShiftPdfViewModel> genericServiceResultTemplate = new GenericResponseTemplateModel<FactoryLicenceDetailsForNightShiftPdfViewModel>();
            try
            {
                List<StoreProcedureParm> storeProcedureParms = new List<StoreProcedureParm>()
                {
                    new StoreProcedureParm (){ ParmName="LicenceNo", ParmValue=licenceNo.ToString(), isNumber=false}
                };
                var resp = await _iGeneric_SP_Repository.CallStoreProcedureReaderAsync<FactoryLicenceDetailsForNightShiftPdfViewModel>("sp_GetFactoryLicenceDetailsForNightShiftByLicenceNo", storeProcedureParms);
                if (resp != null)
                {
                    genericServiceResultTemplate.ResponseDataModel = resp.FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                genericServiceResultTemplate.HasError = true;
                genericServiceResultTemplate.ErrorDesc = ex.Message;
            }
            return genericServiceResultTemplate;
        }


        public async Task<GenericFormModel<List<RoleWiseAllowedActionCodeViewModel>>> GetTimeLineWiseAllowedAction(Int64 appActionLogRefId, string userRefId, Int64 appRefId)
        {
            GenericFormModel<List<RoleWiseAllowedActionCodeViewModel>> genericFormModel = new GenericFormModel<List<RoleWiseAllowedActionCodeViewModel>>();
            try
            {
                List<StoreProcedureParm> storeProcedureParms = new List<StoreProcedureParm>()
                {
                    new StoreProcedureParm (){ ParmName="AppActionLogRefId", ParmValue=appActionLogRefId.ToString(), isNumber=true},
                    new StoreProcedureParm (){ ParmName="UserRefId", ParmValue=userRefId, isNumber=false},
                    new StoreProcedureParm (){ ParmName="AppRefId", ParmValue=appRefId.ToString(), isNumber=true}
                };
                genericFormModel.FormModel = await _iGeneric_SP_Repository.CallStoreProcedureReaderAsync<RoleWiseAllowedActionCodeViewModel>("sp_TimeLineWiseAllowedActionCodes", storeProcedureParms);

                var application = await _context.Applications.Where(x => x.AppId == appRefId).FirstOrDefaultAsync();
                if (genericFormModel.FormModel.Count() == 0 && application.IsTimeLineFlow)
                {
                    await _iAppTimeLineManagerService.SeedTimeLineData(appRefId, appActionLogRefId);
                }
            }
            catch (Exception ex)
            {
                genericFormModel.HasError = true;
                genericFormModel.ErrorDesc = ex.Message;
            }
            return genericFormModel;
        }

        public async Task<GenericFormModel<List<AppActionTime_MutualProcessFlagViewModel>>> GetTimeLineOpenMutualAction(Int64 appRefId, string userRefId)
        {
            GenericFormModel<List<AppActionTime_MutualProcessFlagViewModel>> genericFormModel = new GenericFormModel<List<AppActionTime_MutualProcessFlagViewModel>>();
            try
            {
                List<StoreProcedureParm> storeProcedureParms = new List<StoreProcedureParm>()
                {
                    new StoreProcedureParm (){ ParmName="AppRefId", ParmValue=appRefId.ToString(), isNumber=true},
                    new StoreProcedureParm (){ ParmName="UserRefId", ParmValue=userRefId, isNumber=false}
                };
                genericFormModel.FormModel = await _iGeneric_SP_Repository.CallStoreProcedureReaderAsync<AppActionTime_MutualProcessFlagViewModel>("sp_TimeLine_OpenMutualAction", storeProcedureParms);
            }
            catch (Exception ex)
            {
                genericFormModel.HasError = true;
                genericFormModel.ErrorDesc = ex.Message;
            }
            return genericFormModel;
        }

        //public async Task<GenericResponseTemplateModel<RecordActionResponseViewModel>> RecordApplicationActionPSIEC(ApplicationActionViewModel formModel, string userId)
        //{
        //    GenericResponseTemplateModel<RecordActionResponseViewModel> resp = new GenericResponseTemplateModel<RecordActionResponseViewModel>();
        //    try
        //    {
        //        var applicationAction = await _context.ApplicationActions.Where(x => x.ApplicationRefId == formModel.AppRefId).FirstOrDefaultAsync();
        //        List<StoreProcedureParm> storeProcedureParms = new List<StoreProcedureParm>()
        //        {
        //            new StoreProcedureParm (){ ParmName="UserId", ParmValue= formModel.Receiver_UserRefId.Replace("_",",") + "," + userId, isNumber=false }
        //        };
        //        var user = await _iGeneric_SP_Repository.CallStoreProcedureReaderAsync<SenderReceiverDetailViewModal>("dbo.sp_GetSenderReceiverDetail", storeProcedureParms);

        //        CheckListDataViewModel ChecklistData = null;
        //        if (formModel.CheckListFormJson != null && formModel.CheckListFormJson.Length > 0)
        //        {
        //            ChecklistData = JsonConvert.DeserializeObject<CheckListDataViewModel>(formModel.CheckListFormJson);
        //        }

        //        int totalDays = GetWorkingDays(applicationAction.ActionDate, DateTimeOffset.UtcNow.Date);
        //        int holidays = _context.Holidays.Where(x => x.HolidayDate >= applicationAction.ActionDate && x.HolidayDate <= DateTimeOffset.UtcNow.Date && x.IsWeekend == false).Count();
        //        int totalWorkingDays = 0;
        //        if (holidays > 0)
        //        {
        //            totalWorkingDays = (totalDays - holidays);
        //        }
        //        else
        //        {
        //            totalWorkingDays = totalDays;
        //        }

        //        var end = DateTime.Now;
        //        var start = applicationAction.ActionDate;
        //        var weekend = new[] { DayOfWeek.Saturday, DayOfWeek.Sunday };

        //        var totalWorkingHours = Enumerable.Range(0, Convert.ToInt32(end.Subtract(start).TotalHours))
        //             .Count(offset => !weekend.Contains(start.AddHours(offset).DayOfWeek));

        //        if (applicationAction != null)
        //        {
        //            applicationAction.ActionTakenDaysCount = totalWorkingDays;
        //            applicationAction.ActionTakenHoursCount = Convert.ToInt64(totalWorkingHours);
        //            List<ReceiverBackupViewModel> receiverBackups = new List<ReceiverBackupViewModel>()
        //            { new ReceiverBackupViewModel()
        //                {
        //                    Receiver_ProfileRefId = applicationAction.Receiver_ProfileRefId,
        //                    Receiver_UserRefId = applicationAction.Receiver_UserRefId,
        //                    ReceiverRoleId = applicationAction.ReceiverRoleId,
        //                    Receiver_ActionOn = applicationAction.ActionDate,
        //                    Receiver_Remarks = applicationAction.Remarks,
        //                    AppActionType = (AppActionTypeEnum) applicationAction.AppActionType,
        //                    SenderRoleId = applicationAction.SenderRoleId,
        //                    Sender_ProfileRefId = applicationAction.Sender_ProfileRefId,
        //                    Sender_UserRefId = applicationAction.Sender_UserRefId,
        //                    ActionTakenModeType = applicationAction.ActionTakenModeType,
        //                    IsCurrentReceiver = applicationAction.Receiver_UserRefId != formModel.UserId,
        //                    ReceiverRepeatedCount = 1
        //                }
        //            };

        //            applicationAction.ActionDate = DateTime.Now;
        //            var appActionExt = _context.ApplicationActionExtensions.Where(x => x.AppActionRefId == applicationAction.AppActionId).FirstOrDefault();
        //            if (appActionExt != null)
        //            {
        //                int i = 1;
        //                foreach (var item in typeof(ApplicationActionExtension).GetProperties().Where(x => x.Name.Contains("Receiver_UserRefId_")))
        //                {

        //                    var extReceiver = new ReceiverBackupViewModel()
        //                    {
        //                        Receiver_ProfileRefId = Convert.ToInt64(appActionExt.GetType().GetProperty("Receiver_ProfileRefId_" + i.ToString()).GetValue(appActionExt)),
        //                        Receiver_UserRefId = Convert.ToString(appActionExt.GetType().GetProperty("Receiver_UserRefId_" + i.ToString()).GetValue(appActionExt)),
        //                        ReceiverRoleId = Convert.ToString(appActionExt.GetType().GetProperty("ReceiverRoleId_" + i.ToString()).GetValue(appActionExt)),
        //                        Receiver_ActionOn = Convert.ToDateTime(appActionExt.GetType().GetProperty("Receiver_ActionOn_" + i.ToString()).GetValue(appActionExt)),
        //                        Receiver_Remarks = Convert.ToString(appActionExt.GetType().GetProperty("Receiver_Remarks_" + i.ToString()).GetValue(appActionExt)),

        //                        AppActionType = (AppActionTypeEnum)appActionExt.GetType().GetProperty("AppActionType_" + i.ToString()).GetValue(appActionExt),
        //                        SenderRoleId = Convert.ToString(appActionExt.GetType().GetProperty("SenderRoleId_" + i.ToString()).GetValue(appActionExt)),
        //                        Sender_ProfileRefId = Convert.ToInt64(appActionExt.GetType().GetProperty("Sender_ProfileRefId_" + i.ToString()).GetValue(appActionExt)),
        //                        Sender_UserRefId = Convert.ToString(appActionExt.GetType().GetProperty("Sender_UserRefId_" + i.ToString()).GetValue(appActionExt)),

        //                        ActionTakenModeType = (ActionTakenModeTypeEnum)(appActionExt.GetType().GetProperty("ActionTakenModeType_" + i.ToString()).GetValue(appActionExt)),


        //                        //AppActionLogRefId = Convert.ToInt64(appActionExt.GetType().GetProperty("AppActionLogRefId_" + i.ToString()).GetValue(appActionExt)),
        //                        ActionTakenHoursCount = Convert.ToInt64(appActionExt.GetType().GetProperty("ActionTakenHoursCount_" + i.ToString()).GetValue(appActionExt)),
        //                        ActionTakenDaysCount = Convert.ToInt64(appActionExt.GetType().GetProperty("ActionTakenDaysCount_" + i.ToString()).GetValue(appActionExt)),
        //                        IsDocumentUploaded = Convert.ToBoolean(appActionExt.GetType().GetProperty("IsDocumentUploaded_" + i.ToString()).GetValue(appActionExt)),
        //                        AppDocumentRefId = Convert.ToInt64(appActionExt.GetType().GetProperty("AppDocumentRefId_" + i.ToString()).GetValue(appActionExt)),

        //                        //IsCurrentReceiver = applicationAction.Receiver_UserRefId != formModel.UserId
        //                    };
        //                    extReceiver.IsCurrentReceiver = extReceiver.Receiver_UserRefId != formModel.UserId;

        //                    extReceiver.ReceiverRepeatedCount = receiverBackups.Count(x => x.Receiver_UserRefId == Convert.ToString(appActionExt.GetType().GetProperty("Receiver_UserRefId_" + i.ToString()).GetValue(appActionExt))) + 1;

        //                    receiverBackups.Add(extReceiver);
        //                    i++;
        //                }
        //            }
        //            var splitedUserIds = formModel.Receiver_UserRefId.Split("_");
        //            foreach (var item in splitedUserIds)
        //            {
        //                receiverBackups.Add(
        //                    new ReceiverBackupViewModel()
        //                    {
        //                        Receiver_ProfileRefId = user.Where(x => x.UserId == item).Select(x => x.UserProfileId).FirstOrDefault(),
        //                        Receiver_UserRefId = item,
        //                        ReceiverRoleId = user.Where(x => x.UserId == item).Select(x => x.RoleId).FirstOrDefault(),
        //                        Receiver_ActionOn = applicationAction.ActionDate,
        //                        Receiver_Remarks = formModel.Remarks,

        //                        AppActionType = (AppActionTypeEnum)formModel.AppActionType,
        //                        Sender_ProfileRefId = user.Where(x => x.UserId == userId).Select(x => x.UserProfileId).FirstOrDefault(),
        //                        Sender_UserRefId = userId,
        //                        SenderRoleId = user.Where(x => x.UserId.ToLower() == userId.ToLower()).Select(x => x.RoleId).FirstOrDefault(),
        //                        ActionTakenModeType = formModel.ActionTakenModeType,
        //                        IsCurrentReceiver = true,
        //                        ReceiverRepeatedCount = receiverBackups.Count(x => x.Receiver_UserRefId == item) + 1
        //                    });
        //            }

        //            // var kk = JsonConvert.SerializeObject(receiverBackups);
        //            //receiverBackups = receiverBackups.Where(x => (x.Receiver_UserRefId != formModel.UserId)).ToList();
        //            receiverBackups = receiverBackups.Where(x => (x.Receiver_UserRefId != null)).ToList();
        //            receiverBackups = receiverBackups.Where(x => (x.Receiver_UserRefId != String.Empty)).ToList();
        //            receiverBackups = receiverBackups.Where(x => (x.IsCurrentReceiver)).ToList();

        //            List<ReceiverBackupViewModel> receiverBackups_Filtered = new List<ReceiverBackupViewModel>();

        //            foreach (var item in receiverBackups)
        //            {
        //                if (receiverBackups_Filtered.Count(x => x.Receiver_UserRefId == item.Receiver_UserRefId) == 0)
        //                {
        //                    int maxReceiverRepeatedCount = receiverBackups.Where(x => x.Receiver_UserRefId == item.Receiver_UserRefId).Select(x => x.ReceiverRepeatedCount).Max();
        //                    receiverBackups_Filtered.Add(receiverBackups.Where(x => x.Receiver_UserRefId == item.Receiver_UserRefId && x.ReceiverRepeatedCount == maxReceiverRepeatedCount).FirstOrDefault());
        //                }
        //            }

        //            receiverBackups = receiverBackups_Filtered;
        //            applicationAction.Receiver_ProfileRefId = receiverBackups.FirstOrDefault().Receiver_ProfileRefId;
        //            applicationAction.Receiver_UserRefId = receiverBackups.FirstOrDefault().Receiver_UserRefId;
        //            applicationAction.ReceiverRoleId = receiverBackups.FirstOrDefault().ReceiverRoleId;
        //            applicationAction.ActionDate = Convert.ToDateTime(receiverBackups.FirstOrDefault().Receiver_ActionOn);
        //            applicationAction.Remarks = receiverBackups.FirstOrDefault().Receiver_Remarks;
        //            applicationAction.Sender_ProfileRefId = receiverBackups.FirstOrDefault().Sender_ProfileRefId;
        //            applicationAction.Sender_UserRefId = receiverBackups.FirstOrDefault().Sender_UserRefId;
        //            applicationAction.SenderRoleId = receiverBackups.FirstOrDefault().SenderRoleId;
        //            applicationAction.AppActionType = (int)receiverBackups.FirstOrDefault().AppActionType;
        //            receiverBackups.RemoveAt(0);
        //            ApplicationActionExtension updatedAppActionExt = new ApplicationActionExtension();
        //            int j = 0;
        //            foreach (var item in receiverBackups)
        //            {
        //                j++;
        //                updatedAppActionExt.GetType().GetProperty("Receiver_UserRefId_" + j.ToString()).SetValue(updatedAppActionExt, item.Receiver_UserRefId);
        //                updatedAppActionExt.GetType().GetProperty("Receiver_ProfileRefId_" + j.ToString()).SetValue(updatedAppActionExt, item.Receiver_ProfileRefId);
        //                updatedAppActionExt.GetType().GetProperty("ReceiverRoleId_" + j.ToString()).SetValue(updatedAppActionExt, item.ReceiverRoleId);
        //                updatedAppActionExt.GetType().GetProperty("Receiver_ActionOn_" + j.ToString()).SetValue(updatedAppActionExt, item.Receiver_ActionOn);
        //                updatedAppActionExt.GetType().GetProperty("Receiver_Remarks_" + j.ToString()).SetValue(updatedAppActionExt, item.Receiver_Remarks);


        //                updatedAppActionExt.GetType().GetProperty("Sender_ProfileRefId_" + j.ToString()).SetValue(updatedAppActionExt, item.Sender_ProfileRefId);
        //                updatedAppActionExt.GetType().GetProperty("Sender_UserRefId_" + j.ToString()).SetValue(updatedAppActionExt, item.Sender_UserRefId);
        //                updatedAppActionExt.GetType().GetProperty("SenderRoleId_" + j.ToString()).SetValue(updatedAppActionExt, item.SenderRoleId);
        //                updatedAppActionExt.GetType().GetProperty("AppActionType_" + j.ToString()).SetValue(updatedAppActionExt, item.AppActionType);


        //                updatedAppActionExt.GetType().GetProperty("ActionTakenModeType_" + j.ToString()).SetValue(updatedAppActionExt, item.ActionTakenModeType);

        //                //updatedAppActionExt.GetType().GetProperty("AppActionLogRefId_" + j.ToString()).SetValue(updatedAppActionExt, item.AppActionLogRefId);
        //                updatedAppActionExt.GetType().GetProperty("ActionTakenHoursCount_" + j.ToString()).SetValue(updatedAppActionExt, item.ActionTakenHoursCount);
        //                updatedAppActionExt.GetType().GetProperty("ActionTakenDaysCount_" + j.ToString()).SetValue(updatedAppActionExt, item.ActionTakenDaysCount);
        //                updatedAppActionExt.GetType().GetProperty("IsDocumentUploaded_" + j.ToString()).SetValue(updatedAppActionExt, item.IsDocumentUploaded);
        //                updatedAppActionExt.GetType().GetProperty("AppDocumentRefId_" + j.ToString()).SetValue(updatedAppActionExt, item.AppDocumentRefId);
        //            }


        //            if (appActionExt != null)
        //            {
        //                _context.ApplicationActionExtensions.RemoveRange(appActionExt);
        //                _context.SaveChanges();
        //            }

        //            if (j > 0)
        //            {
        //                updatedAppActionExt.AppActionRefId = applicationAction.AppActionId;
        //                updatedAppActionExt.CurrentActionTakenUserRefId = formModel.UserId;
        //                updatedAppActionExt.CurrentActionTakenCode = (AppActionTypeEnum)formModel.AppActionType;
        //                await _context.ApplicationActionExtensions.AddAsync(updatedAppActionExt);
        //                await _context.SaveChangesAsync();
        //            }



        //            //int i = 0;
        //            //var splitedUserIds = formModel.Receiver_UserRefId.Split("_");
        //            //bool hasSenderEliminated = false;
        //            //foreach (var item in splitedUserIds) 
        //            //{
        //            //    //List<StoreProcedureParm> storeProcedureParms = new List<StoreProcedureParm>()
        //            //    //{
        //            //    //    new StoreProcedureParm (){ ParmName="UserId", ParmValue= item + "," + userId, isNumber=false }
        //            //    //};
        //            //    //var user = await _iGeneric_SP_Repository.CallStoreProcedureReaderAsync<SenderReceiverDetailViewModal>("dbo.sp_GetSenderReceiverDetail", storeProcedureParms);
        //            //    ReceiverBackupViewModel receiverBackup = new ReceiverBackupViewModel();  

        //            //    if (i == 0) 
        //            //    {
        //            //        receiverBackup.Receiver_ProfileRefId = applicationAction.Receiver_ProfileRefId;
        //            //        receiverBackup.Receiver_UserRefId = applicationAction.Receiver_UserRefId;
        //            //        receiverBackup.ReceiverRoleId = applicationAction.ReceiverRoleId;

        //            //        applicationAction.Receiver_ProfileRefId = user.Where(x => x.UserId == item).Select(x => x.UserProfileId).FirstOrDefault();  //user.Where(x => x.UserId == formModel.UserId).Select(x => x.UserProfileId).FirstOrDefault();
        //            //        applicationAction.Receiver_UserRefId = item;
        //            //        applicationAction.ReceiverRoleId = user.Where(x => x.UserId == item).Select(x => x.RoleId).FirstOrDefault();
        //            //    }
        //            //    //else
        //            //    //{
        //            //        var appActionExt = _context.ApplicationActionExtensions.Where(x=>x.AppActionRefId == applicationAction.AppActionId).FirstOrDefault();
        //            //        if(appActionExt == null && splitedUserIds.Length>1)
        //            //        {
        //            //            appActionExt = new ApplicationActionExtension()
        //            //            {
        //            //                Receiver_ProfileRefId_1 = user.Where(x => x.UserId == item).Select(x => x.UserProfileId).FirstOrDefault(), 
        //            //                Receiver_UserRefId_1 = item,
        //            //                ReceiverRoleId_1 = user.Where(x => x.UserId == item).Select(x => x.RoleId).FirstOrDefault(),
        //            //                AppActionRefId = applicationAction.AppActionId
        //            //            };
        //            //            await _context.ApplicationActionExtensions.AddAsync(appActionExt);
        //            //            await _context.SaveChangesAsync();
        //            //        }




        //            //        else
        //            //        {
        //            //            if (i < 2 && !hasSenderEliminated)
        //            //            {
        //            //                for (int k = 0; k < splitedUserIds.Length-1; k++)
        //            //                {
        //            //                    if (appActionExt.GetType().GetProperty("Receiver_UserRefId_" + i.ToString()).GetValue(appActionExt).ToString() == formModel.UserId)
        //            //                    {
        //            //                        if (receiverBackup.Receiver_UserRefId == formModel.UserId)
        //            //                        {
        //            //                            appActionExt.GetType().GetProperty("Receiver_UserRefId_" + i.ToString()).SetValue(appActionExt, null);
        //            //                            appActionExt.GetType().GetProperty("Receiver_ProfileRefId_" + i.ToString()).SetValue(appActionExt, 0);
        //            //                            appActionExt.GetType().GetProperty("ReceiverRoleId" + i.ToString()).SetValue(appActionExt, null);
        //            //                        }
        //            //                        else
        //            //                        {
        //            //                            appActionExt.GetType().GetProperty("Receiver_UserRefId_" + i.ToString()).SetValue(appActionExt, receiverBackup.Receiver_UserRefId);
        //            //                            appActionExt.GetType().GetProperty("Receiver_ProfileRefId_" + i.ToString()).SetValue(appActionExt, receiverBackup.Receiver_ProfileRefId);
        //            //                            appActionExt.GetType().GetProperty("ReceiverRoleId" + i.ToString()).SetValue(appActionExt, receiverBackup.ReceiverRoleId);
        //            //                        }
        //            //                    }
        //            //                }
        //            //                hasSenderEliminated = true;
        //            //            }


        //            //            if (appActionExt.GetType().GetProperty("Receiver_UserRefId_" + i.ToString()).GetValue(appActionExt) == null)
        //            //            {
        //            //                appActionExt.GetType().GetProperty("Receiver_UserRefId_" + i.ToString()).SetValue(appActionExt, item);
        //            //                appActionExt.GetType().GetProperty("Receiver_ProfileRefId_" + i.ToString()).SetValue(appActionExt, user.Where(x => x.UserId == item).Select(x => x.UserProfileId).FirstOrDefault());
        //            //                appActionExt.GetType().GetProperty("ReceiverRoleId" + i.ToString()).SetValue(appActionExt, user.Where(x => x.UserId == item).Select(x => x.RoleId).FirstOrDefault());
        //            //            }

        //            //            _context.Update<ApplicationActionExtension>(appActionExt);
        //            //            _context.SaveChanges();
        //            //        }
        //            //    //}
        //            //    i++;
        //            //}


        //            //applicationAction.Sender_ProfileRefId = user.Where(x => x.UserId == userId).Select(x => x.UserProfileId).FirstOrDefault();
        //            //applicationAction.Sender_UserRefId = userId;
        //            //applicationAction.SenderRoleId = user.Where(x => x.UserId.ToLower() == userId.ToLower()).Select(x => x.RoleId).FirstOrDefault();

        //            //applicationAction.Remarks = formModel.Remarks;
        //            applicationAction.IsDocumentUploaded = formModel.IsDocumentUploaded;
        //            applicationAction.AppDocumentRefId = formModel.AppDocumentRefId;
        //            applicationAction.Checklist_Json = formModel.CheckListFormJson;

        //            if (ChecklistData != null)
        //            {
        //                applicationAction.Checklist_IsAllAgreed = !ChecklistData.CheckListNodes.Exists(x => x.IsVerified == false);
        //                applicationAction.Checklist_FieldObjections = ChecklistData.CheckListNodes.Where(x => !x.IsVerified && !x.IsDocument).Count();
        //                applicationAction.Checklist_DocObjections = ChecklistData.CheckListNodes.Where(x => !x.IsVerified && x.IsDocument).Count();
        //            }
        //            else
        //            {
        //                applicationAction.Checklist_IsAllAgreed = true;
        //                applicationAction.Checklist_FieldObjections = 0;
        //                applicationAction.Checklist_DocObjections = 0;
        //            }
        //            if (formModel.AppActionType == 206)
        //            {
        //                applicationAction.IpAddress = "Deemed";
        //                applicationAction.Latitude = "Deemed";
        //                applicationAction.Longitude = "Deemed";
        //            }
        //            else if (formModel.AppActionType == 208)
        //            {
        //                applicationAction.IpAddress = "Auto Approve";
        //                applicationAction.Latitude = "Auto Approve";
        //                applicationAction.Longitude = "Auto Approve";
        //            }
        //            else if (formModel.AppActionType == 411)
        //            {
        //                applicationAction.IpAddress = "Dormant";
        //                applicationAction.Latitude = "Dormant";
        //                applicationAction.Longitude = "Dormant";
        //            }
        //            else if (formModel.UserId == "C79F57CB-40BA-48EA-8336-327DC1B78701")
        //            {
        //                applicationAction.IpAddress = "Auto Action";
        //                applicationAction.Latitude = "Auto Action";
        //                applicationAction.Longitude = "Auto Action";
        //            }
        //            else
        //            {
        //                if (formModel.IpAddress == null)
        //                {
        //                    applicationAction.IpAddress = "Auto Action";
        //                    applicationAction.Latitude = "Auto Action";
        //                    applicationAction.Longitude = "Auto Action";
        //                }
        //                else
        //                {
        //                    applicationAction.IpAddress = formModel.IpAddress.ToString();
        //                    applicationAction.Latitude = formModel.Latitude.ToString();
        //                    applicationAction.Longitude = formModel.Longitude.ToString();
        //                }

        //            }
        //            applicationAction.IsDeleted = false;
        //            applicationAction.ActionTakenModeType = formModel.ActionTakenModeType;
        //            _iGR_ApplicationAction.Update(applicationAction);
        //            await _iGR_ApplicationAction.SavechangeAsync();
        //        }

        //        else
        //        {
        //            //_iGR_ApplicationAction.Insert(formModel);
        //            //await _iGR_ApplicationAction.SavechangeAsync();
        //        }

        //        ApplicationActionLog applicationActionlogs = new ApplicationActionLog()
        //        {
        //            ActionDate = applicationAction.ActionDate,
        //            ActionTakenDaysCount = applicationAction.ActionTakenDaysCount,
        //            ActionTakenHoursCount = applicationAction.ActionTakenHoursCount,
        //            AppActionType = applicationAction.AppActionType,

        //            Receiver_ProfileRefId = applicationAction.Receiver_ProfileRefId,
        //            Receiver_UserRefId = applicationAction.Receiver_UserRefId,

        //            Sender_ProfileRefId = applicationAction.Sender_ProfileRefId,
        //            Sender_UserRefId = applicationAction.Sender_UserRefId,
        //            Remarks = applicationAction.Remarks,
        //            ReceiverRoleId = applicationAction.ReceiverRoleId,
        //            ApplicationRefId = applicationAction.ApplicationRefId,
        //            SenderRoleId = applicationAction.SenderRoleId,

        //            Checklist_Json = applicationAction.Checklist_Json,
        //            Checklist_IsAllAgreed = applicationAction.Checklist_IsAllAgreed,
        //            Checklist_FieldObjections = applicationAction.Checklist_FieldObjections,
        //            Checklist_DocObjections = applicationAction.Checklist_DocObjections,
        //            IsDocumentUploaded = applicationAction.IsDocumentUploaded,
        //            AppDocumentRefId = applicationAction.AppDocumentRefId,
        //            ActionTakenModeType = applicationAction.ActionTakenModeType,
        //            IpAddress = applicationAction.IpAddress,
        //            Latitude = applicationAction.Latitude,
        //            Longitude = applicationAction.Longitude,
        //            IsDeleted = false
        //        };

        //        _iGR_ApplicationActionLog.Insert(applicationActionlogs);
        //        await _iGR_ApplicationActionLog.SavechangeAsync();

        //        var appActionExt1 = _context.ApplicationActionExtensions.Where(x => x.AppActionRefId == applicationAction.AppActionId).FirstOrDefault();
        //        if (appActionExt1 != null)
        //        {
        //            ApplicationActionExtensionLog appActionExtensionLog = new ApplicationActionExtensionLog();
        //            appActionExtensionLog = JsonConvert.DeserializeObject<ApplicationActionExtensionLog>(JsonConvert.SerializeObject(appActionExt1));
        //            appActionExtensionLog.CurrentAppActionLogRefId = applicationActionlogs.ApplicationActionLogId;



        //            //{
        //            //    AppActionLogRefId = applicationActionlogs.ApplicationActionLogId,
        //            //    Receiver_UserRefId_1 = appActionExt1.Receiver_UserRefId_1,
        //            //    Receiver_ProfileRefId_1 = appActionExt1.Receiver_ProfileRefId_1,
        //            //    ReceiverRoleId_1 = appActionExt1.ReceiverRoleId_1,
        //            //    Receiver_ActionOn_1 = appActionExt1.Receiver_ActionOn_1,
        //            //    Receiver_Remarks_1 = appActionExt1.Receiver_Remarks_1,
        //            //    AppActionType_1 = appActionExt1.AppActionType_1,
        //            //    SenderRoleId_1 = appActionExt1.SenderRoleId_1,
        //            //    Sender_ProfileRefId_1 = appActionExt1.Sender_ProfileRefId_1,
        //            //    Sender_UserRefId_1 = appActionExt1.Sender_UserRefId_1,


        //            //    Receiver_UserRefId_2 = appActionExt1.Receiver_UserRefId_2,
        //            //    Receiver_ProfileRefId_2 = appActionExt1.Receiver_ProfileRefId_2,
        //            //    ReceiverRoleId_2 = appActionExt1.ReceiverRoleId_2,
        //            //    Receiver_ActionOn_2 = appActionExt1.Receiver_ActionOn_2,
        //            //    Receiver_Remarks_2 = appActionExt1.Receiver_Remarks_2,
        //            //    AppActionType_2 = appActionExt1.AppActionType_2,
        //            //    SenderRoleId_2 = appActionExt1.SenderRoleId_2,
        //            //    Sender_ProfileRefId_2 = appActionExt1.Sender_ProfileRefId_2,
        //            //    Sender_UserRefId_2 = appActionExt1.Sender_UserRefId_2,

        //            //    Receiver_UserRefId_3 = appActionExt1.Receiver_UserRefId_2,
        //            //    Receiver_ProfileRefId_3 = appActionExt1.Receiver_ProfileRefId_2,
        //            //    ReceiverRoleId_2 = appActionExt1.ReceiverRoleId_2,
        //            //    Receiver_ActionOn_2 = appActionExt1.Receiver_ActionOn_2,
        //            //    Receiver_Remarks_2 = appActionExt1.Receiver_Remarks_2,
        //            //    AppActionType_2 = appActionExt1.AppActionType_2,
        //            //    SenderRoleId_2 = appActionExt1.SenderRoleId_2,
        //            //    Sender_ProfileRefId_2 = appActionExt1.Sender_ProfileRefId_2,
        //            //    Sender_UserRefId_2 = appActionExt1.Sender_UserRefId_2,

        //            //};

        //            await _context.ApplicationActionExtensionLogs.AddAsync(appActionExtensionLog);
        //            await _context.SaveChangesAsync();
        //        }

        //        resp.ResponseDataModel = new RecordActionResponseViewModel()
        //        {
        //            AppActionId = applicationAction.AppActionId,
        //            ApplicationActionLogId = applicationActionlogs.ApplicationActionLogId
        //        };

        //        // Update ApplicationActionLogId In BuildingPlanHUDPaymentDetail Table. 
        //        if (applicationAction.AppActionType == 401)
        //        {
        //            var paymentList = _context.BuildingPlanHUDPaymentDetails.Where(x => x.AppRefId == formModel.AppRefId).ToList();
        //            foreach (var payment in paymentList)
        //            {
        //                payment.ApplicationActionLogId = applicationActionlogs.ApplicationActionLogId;
        //                _context.SaveChanges();
        //            }
        //        }

        //        bool allOk = true;
        //        //Handle Approvals and rejections
        //        string licenseFilePath = "NA";
        //        string licenseDirName = "";
        //        var application = await _iGR_Application.GetAsync(x => x.AppId == formModel.AppRefId, null, x => x.ProjectSites).ConfigureAwait(false);
        //        if ((formModel.AppActionType == 200 || formModel.AppActionType == 206) && application.FirstOrDefault().ApplicationType != ApplicationTypeEnum.BUILDING_PLAN_HUD)//Approved
        //        {
        //            //Application application
        //            if (formModel.AppActionType == 206)
        //            {
        //                allOk = await _iApplicationMamnagementRepository.SwitchApplicationLifeCycleType(formModel.AppRefId, ApplicationLifeCycleStatusTypeEnum.DEEMED_COMPLETED);
        //            }
        //            else if (formModel.AppActionType == 214)
        //            {
        //                allOk = await _iApplicationMamnagementRepository.SwitchApplicationLifeCycleType(formModel.AppRefId, ApplicationLifeCycleStatusTypeEnum.DEEMED_WITH_FEE_PENDING);
        //            }
        //            else if (formModel.AppActionType == 208)
        //            {
        //                allOk = await _iApplicationMamnagementRepository.SwitchApplicationLifeCycleType(formModel.AppRefId, ApplicationLifeCycleStatusTypeEnum.AUTO_APPROVEL_COMPLETED);
        //            }
        //            else
        //            {
        //                allOk = await _iApplicationMamnagementRepository.SwitchApplicationLifeCycleType(formModel.AppRefId, ApplicationLifeCycleStatusTypeEnum.APPROVED);
        //            }


        //            //GenerateLicenceNoViewModel licenceNo = await _iApplicationMamnagementRepository.GenerateLicenceNo(formModel.AppRefId, application.FirstOrDefault().ApplicationType);

        //            var tempFilePath = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "TempFiles")).Root;

        //            if (File.Exists(tempFilePath + formModel.PdfNameGUID + ".pdf") && allOk)
        //            {
        //                licenseDirName = "AppForm_" + application.FirstOrDefault().ApplicationType.ToString();
        //                var licenceRootDirUrl = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "License")).Root.ToString();
        //                Directory.CreateDirectory(licenceRootDirUrl + licenseDirName);
        //                licenseFilePath = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "License", licenseDirName)).Root;

        //                formModel.PublicAppRefNum = _context.Applications.Where(x => x.AppId == formModel.AppRefId && x.IsDeleted == false).FirstOrDefault().PublicAppRefNum;

        //                if (File.Exists(licenseFilePath + formModel.PublicAppRefNum + ".pdf"))
        //                {
        //                    File.Delete(licenseFilePath + formModel.PublicAppRefNum + ".pdf");
        //                }
        //                File.Move(tempFilePath + formModel.PdfNameGUID + ".pdf", licenseFilePath + formModel.PublicAppRefNum + ".pdf");
        //            }
        //            else
        //            {
        //                allOk = false;
        //            }

        //            if (allOk && application.FirstOrDefault().ApplicationType == ApplicationTypeEnum.FACTORY_LICENCE)
        //            {
        //                Licence_Factory_AdditionalDetail additionalInfo = new Licence_Factory_AdditionalDetail()
        //                {
        //                    AppRefId = application.FirstOrDefault().AppId,
        //                    FactoryHazardousCategoryType = formModel.FactoryHazardousCategoryType,
        //                    FactorySectionCategoryType = formModel.FactorySectionCategoryType,
        //                    FactorySessionCategoryType = formModel.FactorySessionCategoryType,
        //                    LabourCircleRefId = formModel.LabourCircleRefId,
        //                    FactoryCategoryType = formModel.FactoryCategoryType
        //                };

        //                storeProcedureParms = new List<StoreProcedureParm>()
        //                {
        //                    new StoreProcedureParm (){ ParmName="AppRefId", ParmValue= additionalInfo.AppRefId.ToString(), isNumber=true},
        //                    new StoreProcedureParm (){ ParmName="FactoryHazardousCategoryType", ParmValue= ((int)additionalInfo.FactoryHazardousCategoryType).ToString(), isNumber=true},
        //                    new StoreProcedureParm (){ ParmName="FactorySectionCategoryType", ParmValue= ((int)additionalInfo.FactorySectionCategoryType).ToString(), isNumber=true },
        //                    new StoreProcedureParm (){ ParmName="FactorySessionCategoryType", ParmValue= ((int)additionalInfo.FactorySessionCategoryType).ToString(), isNumber=true},
        //                    new StoreProcedureParm (){ ParmName="LabourCircleRefId", ParmValue= additionalInfo.LabourCircleRefId.ToString(), isNumber=true},
        //                    new StoreProcedureParm (){ ParmName="FactoryCategoryType", ParmValue= ((int)additionalInfo.FactoryCategoryType).ToString(), isNumber=true}

        //                };
        //                var val = await _iGeneric_SP_Repository.CallStoreProcedureReaderScalerAsync("dbo.sp_Licence_Add_Factory_AdditionalDetails", storeProcedureParms);
        //            }

        //        }

        //        else if ((formModel.AppActionType == 402) && application.FirstOrDefault().ApplicationType != ApplicationTypeEnum.BUILDING_PLAN_HUD)//Raised Additional Fee
        //        {
        //            if (allOk && application.FirstOrDefault().ApplicationType == ApplicationTypeEnum.FACTORY_LICENCE)
        //            {
        //                decimal raisedFee = 0;
        //                if (formModel.ExistingPowerKW_Installed != formModel.PowerKW_Installed || formModel.ExistingWorkers_MaxDuringYear != formModel.Workers_MaxDuringYear)
        //                {
        //                    raisedFee = formModel.RaisedFeeAmount + 100;
        //                }
        //                else
        //                {
        //                    raisedFee = formModel.RaisedFeeAmount;
        //                }
        //                AppFeeDetail appFeeDetail = new AppFeeDetail()
        //                {
        //                    Amount = raisedFee,
        //                    AppRefId = formModel.AppRefId,
        //                    CalculatedOn = DateTime.Now,
        //                    DedicatedDDOCode = null,
        //                    DedicatedTreasurCode = null,
        //                    Description = formModel.RaisedFeeReason,
        //                    FeeHeaderRefId = 30,
        //                    HasDedicatedTreasuryCode = false,
        //                    IsDeduductible = false,
        //                    PaymentPartCounter = 1
        //                };

        //                var res = await IncreasePaymentBatchCounter(appFeeDetail.AppRefId);
        //                appFeeDetail.PaymentBatchCounter = res.ResponseDataModel;
        //                await _context.AppFeeDetails.AddAsync(appFeeDetail);
        //                await _context.SaveChangesAsync();


        //                // If any field change, insert into Factory & Amendment History Table
        //                if (formModel.ExistingWorkers_MaxDuringYear != formModel.Workers_MaxDuringYear || formModel.ExistingPowerKW_Installed != formModel.PowerKW_Installed)
        //                {
        //                    // Update Factory Details
        //                    var factoryLicence = _context.Licence_Factory_GeneralDetails.Where(x => x.AppRefId == formModel.AppRefId).FirstOrDefault();
        //                    //Licence_Factory_GeneralDetail factory_GeneralDetail = new Licence_Factory_GeneralDetail()
        //                    //{
        //                    //    Workers_MaxDuringYear = formModel.Workers_MaxDuringYear,
        //                    //    PowerKW_Installed = formModel.PowerKW_Installed
        //                    //};
        //                    factoryLicence.Workers_MaxDuringYear = formModel.Workers_MaxDuringYear;
        //                    factoryLicence.PowerKW_Installed = formModel.PowerKW_Installed;
        //                    //_iGR_Licence_Factory_GeneralDetail.Update(factory_GeneralDetail);
        //                    //await _iGR_Licence_Factory_GeneralDetail.SavechangeAsync();
        //                    _context.Licence_Factory_GeneralDetails.Update(factoryLicence);
        //                    _context.SaveChanges();

        //                    // Insert Into Log Table
        //                    Licence_Factory_AmendmentDataHistory amendmentDataHistory = new Licence_Factory_AmendmentDataHistory();
        //                    if (formModel.ExistingWorkers_MaxDuringYear != formModel.Workers_MaxDuringYear)
        //                    {
        //                        amendmentDataHistory = new Licence_Factory_AmendmentDataHistory()
        //                        {
        //                            AppRefId = formModel.AppRefId,
        //                            FieldName = "Workers_MaxDuringYear",
        //                            PreviousValue = formModel.ExistingWorkers_MaxDuringYear.ToString(),
        //                            ModifiedValue = formModel.Workers_MaxDuringYear.ToString(),
        //                            SectionCode = "MP",
        //                            ModifiedOn = DateTime.Now,
        //                            ModifiedCounter = appFeeDetail.PaymentBatchCounter
        //                        };
        //                        await _context.Licence_Factory_AmendmentDataHistories.AddAsync(amendmentDataHistory);
        //                    }
        //                    if (formModel.ExistingPowerKW_Installed != formModel.PowerKW_Installed)
        //                    {
        //                        amendmentDataHistory = new Licence_Factory_AmendmentDataHistory()
        //                        {
        //                            AppRefId = formModel.AppRefId,
        //                            FieldName = "PowerKW_Installed",
        //                            PreviousValue = formModel.ExistingPowerKW_Installed.ToString(),
        //                            ModifiedValue = formModel.PowerKW_Installed.ToString(),
        //                            SectionCode = "KW",
        //                            ModifiedOn = DateTime.Now,
        //                            ModifiedCounter = appFeeDetail.PaymentBatchCounter
        //                        };
        //                        await _context.Licence_Factory_AmendmentDataHistories.AddAsync(amendmentDataHistory);
        //                    }
        //                    await _context.SaveChangesAsync();
        //                }

        //                allOk = await _iApplicationMamnagementRepository.SwitchApplicationLifeCycleType(formModel.AppRefId, ApplicationLifeCycleStatusTypeEnum.IN_OBJECTION_WITH_BALANCE_FEE);
        //            }
        //            else if (allOk && (application.FirstOrDefault().ApplicationType == ApplicationTypeEnum.PRINCIPAL_EMPLOYER || application.FirstOrDefault().ApplicationType == ApplicationTypeEnum.ISM_PRINCIPAL_EMPLOYER))
        //            {
        //                decimal raisedFee = 0;
        //                raisedFee = formModel.RaisedFeeAmount;

        //                AppFeeDetail appFeeDetail = new AppFeeDetail()
        //                {
        //                    Amount = raisedFee,
        //                    AppRefId = formModel.AppRefId,
        //                    CalculatedOn = DateTime.Now,
        //                    DedicatedDDOCode = null,
        //                    DedicatedTreasurCode = null,
        //                    Description = formModel.RaisedFeeReason,
        //                    FeeHeaderRefId = 30,
        //                    HasDedicatedTreasuryCode = false,
        //                    IsDeduductible = false,
        //                    PaymentPartCounter = 1
        //                };

        //                var res = await IncreasePaymentBatchCounter(appFeeDetail.AppRefId);
        //                appFeeDetail.PaymentBatchCounter = res.ResponseDataModel;
        //                await _context.AppFeeDetails.AddAsync(appFeeDetail);
        //                await _context.SaveChangesAsync();

        //                allOk = await _iApplicationMamnagementRepository.SwitchApplicationLifeCycleType(formModel.AppRefId, ApplicationLifeCycleStatusTypeEnum.IN_OBJECTION_WITH_BALANCE_FEE);
        //            }
        //            else if (allOk && application.FirstOrDefault().ApplicationType == ApplicationTypeEnum.CONTRACT_LABOUR)
        //            {
        //                decimal raisedFee = formModel.RaisedFeeAmount;
        //                AppFeeDetail raisedFeeDetail = new AppFeeDetail()
        //                {
        //                    Amount = raisedFee,
        //                    AppRefId = formModel.AppRefId,
        //                    CalculatedOn = DateTime.Now,
        //                    DedicatedDDOCode = null,
        //                    DedicatedTreasurCode = null,
        //                    Description = formModel.RaisedFeeReason,
        //                    FeeHeaderRefId = 30,
        //                    HasDedicatedTreasuryCode = false,
        //                    IsDeduductible = false,
        //                    PaymentPartCounter = 1
        //                };

        //                decimal securityRaisedFee = formModel.SecurityRaisedFeeAmount;
        //                AppFeeDetail securityFeeDetail = new AppFeeDetail()
        //                {
        //                    Amount = securityRaisedFee,
        //                    AppRefId = formModel.AppRefId,
        //                    CalculatedOn = DateTime.Now,
        //                    DedicatedDDOCode = null,
        //                    DedicatedTreasurCode = null,
        //                    Description = formModel.RaisedFeeReason,
        //                    FeeHeaderRefId = 15,
        //                    HasDedicatedTreasuryCode = false,
        //                    IsDeduductible = false,
        //                    PaymentPartCounter = 1
        //                };

        //                var res = await IncreasePaymentBatchCounter(formModel.AppRefId);
        //                raisedFeeDetail.PaymentBatchCounter = res.ResponseDataModel;
        //                securityFeeDetail.PaymentBatchCounter = res.ResponseDataModel;

        //                await _context.AppFeeDetails.AddAsync(raisedFeeDetail);
        //                await _context.AppFeeDetails.AddAsync(securityFeeDetail);
        //                await _context.SaveChangesAsync();

        //                allOk = await _iApplicationMamnagementRepository.SwitchApplicationLifeCycleType(formModel.AppRefId, ApplicationLifeCycleStatusTypeEnum.IN_OBJECTION_WITH_BALANCE_FEE);
        //            }
        //        }

        //        else if ((formModel.AppActionType == 412 && application.FirstOrDefault().ApplicationType == ApplicationTypeEnum.BUILDING_PLAN_PSIEC)
        //            || (formModel.AppActionType == 102 && application.FirstOrDefault().ApplicationType == ApplicationTypeEnum.BUILDING_PLAN_PSIEC))//Raised Fee PSIEC-FactoryWing
        //        {
        //            if (allOk)
        //            {
        //                AppActionTypeEnum appActionType = AppActionTypeEnum.DEFAULT;
        //                if (formModel.AppActionType == 412)
        //                {
        //                    List<AppFeeDetail> appFeeDetail = new List<AppFeeDetail>();
        //                    appFeeDetail.Add(new AppFeeDetail()
        //                    {
        //                        Amount = formModel.PsiecCessAmount,
        //                        AppRefId = formModel.AppRefId,
        //                        CalculatedOn = DateTime.Now,
        //                        DedicatedDDOCode = null,
        //                        DedicatedTreasurCode = null,
        //                        Description = formModel.Remarks,
        //                        FeeHeaderRefId = 19,
        //                        HasDedicatedTreasuryCode = false,
        //                        IsDeduductible = false,
        //                        PaymentPartCounter = 1
        //                    });
        //                    appFeeDetail.Add(new AppFeeDetail()
        //                    {
        //                        Amount = formModel.PsiecProcessingFeeAmount,
        //                        AppRefId = formModel.AppRefId,
        //                        CalculatedOn = DateTime.Now,
        //                        DedicatedDDOCode = null,
        //                        DedicatedTreasurCode = null,
        //                        Description = formModel.Remarks,
        //                        FeeHeaderRefId = 39,
        //                        HasDedicatedTreasuryCode = false,
        //                        IsDeduductible = false,
        //                        PaymentPartCounter = 1
        //                    });

        //                    var res = await IncreasePaymentBatchCounter(formModel.AppRefId);
        //                    appFeeDetail = appFeeDetail.Select(x => { x.PaymentBatchCounter = res.ResponseDataModel; return x; }).ToList();
        //                    await _context.BulkInsertAsync<AppFeeDetail>(appFeeDetail);
        //                    appActionType = AppActionTypeEnum.FEE_RAISED;


        //                    // Insert into raise Fee table- 
        //                    IList<Payments_RaisedFee> listData = new List<Payments_RaisedFee>();
        //                    listData.Add(new Payments_RaisedFee
        //                    {
        //                        AppRefId = formModel.AppRefId,
        //                        FeeHeaderRefId = 19,
        //                        PaymentBatchCounter = formModel.PaymentBatchCounter,
        //                        AmountRaised = formModel.PsiecCessAmount,
        //                        AmountAlreadyPaid = 0,
        //                        IsFeeApplicable = true,
        //                        Createddate = DateTime.UtcNow,
        //                        LastModifiedDate = DateTime.UtcNow,
        //                        ApplicationActionLogId = 0,
        //                        NonTreasuryCode = "NA",
        //                        Description = "NA",
        //                        HasDedicatedTreasuryCode = true,
        //                        DedicatedTreasurCode = "NA",
        //                        DedicatedDDOCode = "NA"
        //                    });

        //                    listData.Add(new Payments_RaisedFee
        //                    {
        //                        AppRefId = formModel.AppRefId,
        //                        FeeHeaderRefId = 39,
        //                        PaymentBatchCounter = formModel.PaymentBatchCounter,
        //                        AmountRaised = formModel.PsiecProcessingFeeAmount,
        //                        AmountAlreadyPaid = 0,
        //                        IsFeeApplicable = true,
        //                        Createddate = DateTime.UtcNow,
        //                        LastModifiedDate = DateTime.UtcNow,
        //                        ApplicationActionLogId = 0,
        //                        NonTreasuryCode = "NA",
        //                        Description = "NA",
        //                        HasDedicatedTreasuryCode = true,
        //                        DedicatedTreasurCode = "NA",
        //                        DedicatedDDOCode = "NA"
        //                    });
        //                    await _context.BulkInsertAsync(listData);

        //                }
        //                else if (formModel.AppActionType == 102)
        //                {
        //                    appActionType = AppActionTypeEnum.FORWARD_FOR_OBJECTION;

        //                }
        //                TimeLine_DepartmentWiseFinalAction dfa = new TimeLine_DepartmentWiseFinalAction()
        //                {
        //                    AppRefId = formModel.AppRefId,
        //                    DeptCodeType = DepartmentCodeTypeEnum.LABOUR,
        //                    AppActionType = appActionType,
        //                    AppActionLogRefId = applicationActionlogs.ApplicationActionLogId
        //                };
        //                await _context.AddAsync<TimeLine_DepartmentWiseFinalAction>(dfa);
        //                await _context.SaveChangesAsync();
        //            }
        //        }

        //        else if (application.FirstOrDefault().ApplicationType == ApplicationTypeEnum.BUILDING_PLAN_HUD && (formModel.AppActionType == 200 || formModel.AppActionType == 204)) // Approved -Building Plan Hud
        //        {
        //            allOk = await _iApplicationMamnagementRepository.SwitchApplicationLifeCycleType(formModel.AppRefId, ApplicationLifeCycleStatusTypeEnum.APPROVED);
        //            var appDocument = await _context.ApplicationDocuments.Where(x => x.AppDocId == formModel.AppDocumentRefId).FirstOrDefaultAsync();
        //            if (appDocument != null)
        //            {
        //                var tempFilePath = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "AppFiles")).Root;

        //                if (File.Exists(tempFilePath + appDocument.AttachmentName) && allOk)
        //                {
        //                    licenseDirName = "AppForm_" + application.FirstOrDefault().ApplicationType.ToString();
        //                    var licenceRootDirUrl = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "License")).Root.ToString();
        //                    Directory.CreateDirectory(licenceRootDirUrl + licenseDirName);
        //                    licenseFilePath = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "License", licenseDirName)).Root;

        //                    formModel.PublicAppRefNum = application.FirstOrDefault().PublicAppRefNum;

        //                    if (File.Exists(licenseFilePath + formModel.PublicAppRefNum + ".pdf"))
        //                    {
        //                        File.Delete(licenseFilePath + formModel.PublicAppRefNum + ".pdf");
        //                    }
        //                    File.Copy(tempFilePath + appDocument.AttachmentName, licenseFilePath + formModel.PublicAppRefNum + ".pdf");
        //                }
        //                else
        //                {
        //                    allOk = false;
        //                }

        //                if (allOk)
        //                {

        //                    var licenceNo = "DOFPB" + application.FirstOrDefault().ProjectSiteRefId.ToString("D8");

        //                    ApplicationLicenceNoMapping applicationLicenceNo = new ApplicationLicenceNoMapping()
        //                    {
        //                        AppRefId = formModel.AppRefId,
        //                        LicenceNumber = licenceNo
        //                    };

        //                    await _context.ApplicationLicenceNoMapping.AddAsync(applicationLicenceNo);
        //                    await _context.SaveChangesAsync();
        //                }
        //            }
        //        }
        //        else if (formModel.AppActionType == 201) // Rejected
        //        {
        //            allOk = await _iApplicationMamnagementRepository.SwitchApplicationLifeCycleType(formModel.AppRefId, ApplicationLifeCycleStatusTypeEnum.REJECTED);
        //            // Sent SMS Notification To User
        //            var smsTemplateText = $"Dear Applicant,Your Application No. {application.FirstOrDefault().PublicAppRefNum} is Rejected from Department of Labour-PB LBAOUR";
        //            var templateId = "1407172725588507473";
        //            await _iNotificationManagerService.InitiateNotification(application.FirstOrDefault().ProjectSites.UserRefId,
        //            new List<NotificationViewModel>()
        //            {
        //                new NotificationViewModel(){Body=smsTemplateText, NotificationMode=NotificationModeTypeEnum.MOBILE,NotificationPurpose=NotificationPurposeTypeEnum.INFO, Title=templateId}
        //            });
        //        }
        //        else if (formModel.AppActionType == 404 || formModel.AppActionType == 407) // Objection
        //        {
        //            allOk = await _iApplicationMamnagementRepository.SwitchApplicationLifeCycleType(formModel.AppRefId, ApplicationLifeCycleStatusTypeEnum.IN_OBJECTION);

        //            // Sent SMS Notification To User
        //            var smsTemplateText = $"Department of labour raised Objection on your Application No. {application.FirstOrDefault().PublicAppRefNum} . Please login at pbindustries.gov.in for resolve it.-PB  LABOUR";
        //            var templateId = "1407172725521534794";
        //            await _iNotificationManagerService.InitiateNotification(application.FirstOrDefault().ProjectSites.UserRefId,
        //            new List<NotificationViewModel>()
        //            {
        //                new NotificationViewModel(){Body=smsTemplateText, NotificationMode=NotificationModeTypeEnum.MOBILE,NotificationPurpose=NotificationPurposeTypeEnum.INFO, Title=templateId}
        //            });

        //            // Sent Email Notification 
        //            //var receiverInfo = await _iAuthService.GetUserProfileByUserId(applicationAction.Receiver_UserRefId);
        //            //var application = _context.Applications.Where(x => x.AppId == applicationAction.ApplicationRefId).FirstOrDefault();
        //            //var projectSite = _context.ProjectSites.Where(x => x.ProjectSiteId == application.ProjectSiteRefId).FirstOrDefault();
        //            //TemplateResolveObjectionCaseLendedToOfficerNotificationViewModel mailTemplate = new TemplateResolveObjectionCaseLendedToOfficerNotificationViewModel()
        //            //{
        //            //    OfficerName = receiverInfo.FirstName + " " + receiverInfo.LastName,
        //            //    ApplicationPurposeTitle = EnumOps.GetEnumDescriptionName<ApplicationPurposeTypeEnum>(application.ApplicationPurposeType.ToString()),
        //            //    ApplicationTypeTitle = EnumOps.GetEnumDescriptionName<ApplicationTypeEnum>(application.ApplicationType.ToString()),
        //            //    EstablishmentName = projectSite.EstablishmentName,
        //            //    PublicApplicationRefNo = application.PublicAppRefNum,
        //            //    StatusDescription = applicationAction.Remarks,
        //            //    InvestPunjab_Ipin = application.InvestPunjab_Ipin
        //            //};
        //            //var emailTemplateText = await _iNotificationManagerService.RenderToStringAsync("EmailTemplates/_templateObjectionResolvedAndCaseLendedToOfficerNotification", mailTemplate);
        //            //var emailSendinfResponse = await _iNotificationManagerService.SendEmailSMTP(mailTemplate.OfficerName, receiverInfo.Email, "Department of Labour: The department has raised few objections, kindly examine the application form and resolved the objections. (" + mailTemplate.InvestPunjab_Ipin + ")", emailTemplateText);
        //        }
        //        else if (formModel.AppActionType == 202) // Deregistered
        //        {
        //            allOk = await _iApplicationMamnagementRepository.SwitchApplicationLifeCycleType(formModel.AppRefId, ApplicationLifeCycleStatusTypeEnum.DEREGISTERED);
        //        }
        //        else if (formModel.AppActionType == 411) // Dormant
        //        {
        //            allOk = await _iApplicationMamnagementRepository.SwitchApplicationLifeCycleType(formModel.AppRefId, ApplicationLifeCycleStatusTypeEnum.APPLICATION_DORMANT);
        //        }
        //        else if (formModel.AppActionType == 212) // Decline
        //        {
        //            allOk = await _iApplicationMamnagementRepository.SwitchApplicationLifeCycleType(formModel.AppRefId, ApplicationLifeCycleStatusTypeEnum.APPLICATION_DECLINED);
        //        }
        //        if (allOk)
        //        {
        //            var actionTimeLine = _context.AppActionTimeLines.Where(x => x.AppActionLogRefId == formModel.ApplicationActionLogId).ToList();
        //            if (actionTimeLine != null)
        //            {
        //                actionTimeLine = actionTimeLine.Select(x => { x.IsProcessed = true; return x; }).ToList();
        //                await _context.BulkInsertOrUpdateAsync(actionTimeLine);
        //            }
        //            //if (formModel.AppActionType == (int)AppActionTypeEnum.SCRUTINY_COMPLETED && (applicationActionlogs.SenderRoleId == "5b70c7bd-b591-4300-a34c-56b4bddc9416" || applicationActionlogs.SenderRoleId == "ad7c34a9-e2b5-4131-984a-c3b03ff9b4aa"))
        //            //{
        //            //    await _iAppTimeLineManagerService.AddAppProcessLogType(new ApplicationProcessPhaseLog
        //            //    {
        //            //        ApplicationProcessPhaseLogType = ApplicationProcessPhaseLogTypeEnum.OBSERVATION_PHASE,
        //            //        AppRefId = formModel.AppRefId,
        //            //        LastModifiedOn = DateTime.Now,
        //            //        PhaseCounter = 1,
        //            //    });
        //            //}

        //            if (application.FirstOrDefault().IsTimeLineFlow)
        //            {
        //                await _iAppTimeLineManagerService.SeedTimeLineData(formModel.AppRefId);
        //            }



        //            if (formModel.AppDocumentRefId > 0)
        //            {
        //                var appDocument = _context.ApplicationDocuments.Where(x => x.AppDocId == formModel.AppDocumentRefId).FirstOrDefault();
        //                appDocument.IsLocked = true;
        //                _context.ApplicationDocuments.Update(appDocument);
        //                await _context.SaveChangesAsync();
        //            }

        //            var licenceInfo = await _context.ApplicationLicenceNoMapping.Where(x => x.AppRefId == formModel.AppRefId).FirstOrDefaultAsync();
        //            var statusDescription = await _context.ApplicationActionCodes.Where(x => x.ActionCode == formModel.AppActionType).FirstOrDefaultAsync();
        //            Int64 appActionLogId = await _context.ApplicationActionLogs.Where(x => x.ApplicationRefId == formModel.AppRefId).OrderByDescending(x => x.ApplicationActionLogId).Select(x => x.ApplicationActionLogId).FirstOrDefaultAsync().ConfigureAwait(false);

        //            InvestPunjabShareStatusParmsViewModel statusReq = new InvestPunjabShareStatusParmsViewModel();

        //            statusReq.AppRefId = formModel.AppRefId;
        //            statusReq.ApplicationType = application.FirstOrDefault().ApplicationType;
        //            statusReq.IPin = Convert.ToInt64(application.FirstOrDefault().InvestPunjab_Ipin);
        //            statusReq.InvestPunjab_AppId = application.FirstOrDefault().InvestPunjab_AppId;
        //            //statusReq.StatusId = (formModel.AppActionType == 404 || formModel.AppActionType == 102) ? 9 : formModel.AppActionType == 200 ? 5 : formModel.AppActionType == 500 ? 15 : formModel.AppActionType;
        //            statusReq.StatusId = formModel.AppActionType;
        //            statusReq.StatusDesc = statusDescription.ActionPublicName;
        //            //statusReq.Comments = (formModel.AppActionType == 404 || formModel.AppActionType == 102 || formModel.AppActionType == 201 || formModel.AppActionType == 401) ?  formModel.Remarks: statusDescription.ActionName;
        //            statusReq.Comments = (formModel.AppActionType == 404 || formModel.AppActionType == 401 || formModel.AppActionType == 402 || formModel.AppActionType == 301 || formModel.AppActionType == 201 || formModel.AppActionType == 202 || formModel.AppActionType == 200 || formModel.AppActionType == 407 || formModel.AppActionType == 212) ? formModel.Remarks : statusDescription.ActionName;
        //            statusReq.SenderName = user.Where(x => x.UserId == userId).Select(x => x.FirstName).FirstOrDefault() + ' ' + user.Where(x => x.UserId == userId).Select(x => x.LastName).FirstOrDefault();
        //            statusReq.SenderDesignation = user.Where(x => x.UserId == userId).Select(x => x.NormalizedName).FirstOrDefault();
        //            int i = 0;
        //            foreach (var item in formModel.Receiver_UserRefId.Split("_"))
        //            {
        //                if (i > 0)
        //                {
        //                    statusReq.ReceiverName = statusReq.ReceiverName + " and ";
        //                    statusReq.ReceiverDesignation = statusReq.ReceiverDesignation + " and ";
        //                }
        //                statusReq.ReceiverName = user.Where(x => x.UserId == item).Select(x => x.FirstName).FirstOrDefault() + ' ' + user.Where(x => x.UserId == item).Select(x => x.LastName).FirstOrDefault();
        //                statusReq.ReceiverDesignation = user.Where(x => x.UserId == item).Select(x => x.NormalizedName).FirstOrDefault();
        //                i++;
        //            }

        //            statusReq.ClearanceIssuedOn = DateTime.Now;
        //            statusReq.ClearanceExpiredOn = (application.FirstOrDefault().ApplicationType == ApplicationTypeEnum.SHOP_LICENCE ? (DateTime?)null : new DateTime(DateTime.Now.Year, 12, 31));
        //            //new DateTime(DateTime.Now.Year, 12, 31);
        //            statusReq.LicenseNo = licenceInfo == null ? "NA" : licenceInfo.LicenceNumber.ToString();
        //            statusReq.ClearanceFile = (application.FirstOrDefault().ApplicationType == ApplicationTypeEnum.BUILDING_PLAN_HUD && (formModel.AppActionType == 200 || formModel.AppActionType == 204)) || formModel.AppActionType == 200 || formModel.AppActionType == 206 ? Configuration.GetSection("ThirdPartyIntegrationConfigs").GetSection("WebHostRootUrl").Value + "License/" + licenseDirName + "/" + formModel.PublicAppRefNum + ".pdf" : licenseFilePath;
        //            statusReq.StatusDate = DateTime.Now;
        //            statusReq.IntegrationSource = "LABOUR";
        //            statusReq.IsdeemedApproval = "false";
        //            statusReq.appActionType = (AppActionTypeEnum)formModel.AppActionType;
        //            statusReq.appActionLogId = appActionLogId;
        //            var response = await _iInvestPunjabShareStatusService.ShareStatusToBusinessFirst(statusReq);

        //            //var receiverInfo = await _iAuthService.GetUserProfileByUserId(applicationAction.Receiver_UserRefId);
        //            //var application = _context.Applications.Where(x => x.AppId == applicationAction.ApplicationRefId).FirstOrDefault();
        //            var projectSite = _context.ProjectSites.Where(x => x.ProjectSiteId == application.FirstOrDefault().ProjectSiteRefId).FirstOrDefault();

        //            foreach (var item in formModel.Receiver_UserRefId.Split("_"))
        //            {
        //                if (formModel.AppActionType == 200 || formModel.AppActionType == 206) //Approved Deemed
        //                {
        //                    // Sent Email Notification 
        //                    TemplateResolveObjectionCaseLendedToOfficerNotificationViewModel mailTemplate = new TemplateResolveObjectionCaseLendedToOfficerNotificationViewModel()
        //                    {
        //                        OfficerName = user.Where(x => x.UserId == item).Select(x => x.FirstName).FirstOrDefault() + ' ' + user.Where(x => x.UserId == item).Select(x => x.LastName).FirstOrDefault(),
        //                        ApplicationPurposeTitle = EnumOps.GetEnumDescriptionName<ApplicationPurposeTypeEnum>(application.FirstOrDefault().ApplicationPurposeType.ToString()),
        //                        ApplicationTypeTitle = EnumOps.GetEnumDescriptionName<ApplicationTypeEnum>(application.FirstOrDefault().ApplicationType.ToString()),
        //                        EstablishmentName = projectSite.EstablishmentName,
        //                        PublicApplicationRefNo = application.FirstOrDefault().PublicAppRefNum,
        //                        StatusDescription = applicationAction.Remarks
        //                    };
        //                    var emailTemplateText = await _iNotificationManagerService.RenderToStringAsync("EmailTemplates/_templateObjectionResolvedAndCaseLendedToOfficerNotification", mailTemplate);
        //                    //var emailSendinfResponse = await _iNotificationManagerService.SendEmailSMTP(mailTemplate.OfficerName, user.Where(x => x.UserId == formModel.Receiver_UserRefId).Select(x => x.Email).FirstOrDefault(), "Department of Labour: Your application has been approved. (" + mailTemplate.PublicApplicationRefNo + ")", emailTemplateText);

        //                    // Sent SMS Notification To User
        //                    var smsTemplateText = $"We are pleased to inform you that, your Application No. {application.FirstOrDefault().PublicAppRefNum} is got Approved from Department of Labour.-PB Labour";
        //                    var templateId = "1407172725558167954";
        //                    await _iNotificationManagerService.InitiateNotification(projectSite.UserRefId,
        //                    new List<NotificationViewModel>()
        //                    {
        //                    new NotificationViewModel(){Body=smsTemplateText, NotificationMode=NotificationModeTypeEnum.MOBILE,NotificationPurpose=NotificationPurposeTypeEnum.INFO, Title=templateId}
        //                    });
        //                }
        //                else if (formModel.AppActionType == 404) // Objection
        //                {
        //                    TemplateResolveObjectionCaseLendedToOfficerNotificationViewModel mailTemplate = new TemplateResolveObjectionCaseLendedToOfficerNotificationViewModel()
        //                    {
        //                        OfficerName = user.Where(x => x.UserId == item).Select(x => x.FirstName).FirstOrDefault() + ' ' + user.Where(x => x.UserId == item).Select(x => x.LastName).FirstOrDefault(),
        //                        ApplicationPurposeTitle = EnumOps.GetEnumDescriptionName<ApplicationPurposeTypeEnum>(application.FirstOrDefault().ApplicationPurposeType.ToString()),
        //                        ApplicationTypeTitle = EnumOps.GetEnumDescriptionName<ApplicationTypeEnum>(application.FirstOrDefault().ApplicationType.ToString()),
        //                        EstablishmentName = projectSite.EstablishmentName,
        //                        PublicApplicationRefNo = application.FirstOrDefault().PublicAppRefNum,
        //                        StatusDescription = applicationAction.Remarks,
        //                        InvestPunjab_Ipin = application.FirstOrDefault().InvestPunjab_Ipin
        //                    };
        //                    var emailTemplateText = await _iNotificationManagerService.RenderToStringAsync("EmailTemplates/_templateObjectionResolvedAndCaseLendedToOfficerNotification", mailTemplate);
        //                    //var emailSendinfResponse = await _iNotificationManagerService.SendEmailSMTP(mailTemplate.OfficerName, user.Where(x => x.UserId == formModel.Receiver_UserRefId).Select(x => x.Email).FirstOrDefault(), "Department of Labour: The department has raised few objections, kindly examine the application form and resolved the objections. (" + mailTemplate.InvestPunjab_Ipin + ")", emailTemplateText);

        //                    // Sent SMS Notification To User
        //                    var smsTemplateText = $"Department of labour raised Objection on your Application No. {application.FirstOrDefault().PublicAppRefNum} . Please login at pbindustries.gov.in for resolve it.-PB  LABOUR";
        //                    var templateId = "1407172725521534794";
        //                    await _iNotificationManagerService.InitiateNotification(application.FirstOrDefault().ProjectSites.UserRefId,
        //                    new List<NotificationViewModel>()
        //                    {
        //                new NotificationViewModel(){Body=smsTemplateText, NotificationMode=NotificationModeTypeEnum.MOBILE,NotificationPurpose=NotificationPurposeTypeEnum.INFO, Title=templateId}
        //                    });
        //                }
        //                else if (formModel.AppActionType == 103 || formModel.AppActionType == 400 || formModel.AppActionType == 9) // Forwarded For further action
        //                {
        //                    TemplateResolveObjectionCaseLendedToOfficerNotificationViewModel mailTemplate = new TemplateResolveObjectionCaseLendedToOfficerNotificationViewModel()
        //                    {
        //                        OfficerName = user.Where(x => x.UserId == item).Select(x => x.FirstName).FirstOrDefault() + ' ' + user.Where(x => x.UserId == item).Select(x => x.LastName).FirstOrDefault(),
        //                        ApplicationPurposeTitle = EnumOps.GetEnumDescriptionName<ApplicationPurposeTypeEnum>(application.FirstOrDefault().ApplicationPurposeType.ToString()),
        //                        ApplicationTypeTitle = EnumOps.GetEnumDescriptionName<ApplicationTypeEnum>(application.FirstOrDefault().ApplicationType.ToString()),
        //                        EstablishmentName = projectSite.EstablishmentName,
        //                        PublicApplicationRefNo = application.FirstOrDefault().PublicAppRefNum,
        //                        StatusDescription = applicationAction.Remarks,
        //                        InvestPunjab_Ipin = application.FirstOrDefault().InvestPunjab_Ipin
        //                    };
        //                    var emailTemplateText = await _iNotificationManagerService.RenderToStringAsync("EmailTemplates/_templateObjectionResolvedAndCaseLendedToOfficerNotification", mailTemplate);
        //                    // var emailSendinfResponse = await _iNotificationManagerService.SendEmailSMTP(mailTemplate.OfficerName, user.Where(x => x.UserId == formModel.Receiver_UserRefId).Select(x => x.Email).FirstOrDefault(), "Department of Labour: An application received. (" + mailTemplate.PublicApplicationRefNo + ")", emailTemplateText);
        //                }
        //            }
        //            //else
        //            //{
        //            //    //transaction.Rollback();
        //            //    resp.HasError = true;
        //            //    resp.ErrorDesc = "Something wrong with approval files";
        //            //}
        //            resp.HasError = false;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        //transaction.Rollback();
        //        resp.HasError = true;
        //        resp.ErrorDesc = ex.Message;
        //        throw ex;
        //    }
        //    //}
        //    return resp;
        //}


        public async Task<GenericResponseTemplateModel<RecordActionResponseViewModel>> RecordApplicationActionPSIEC(ApplicationActionViewModel formModel, string userId)
        {
            GenericResponseTemplateModel<RecordActionResponseViewModel> resp = new GenericResponseTemplateModel<RecordActionResponseViewModel>();
            try
            {
                var applicationAction = await _context.ApplicationAction_ParallelProcesses.Where(x => 
                    x.ApplicationRefId == formModel.AppRefId && 
                    x.AppActionType == (int) formModel.PreviousActionType && 
                    x.AppActionParallelProcessId == formModel.ApplicationActionLogId &&
                    x.IsAlive).FirstOrDefaultAsync();

                ApplicationAction_ParallelProcess appActPP = new ApplicationAction_ParallelProcess();
                List<StoreProcedureParm> storeProcedureParms = new List<StoreProcedureParm>()
                {
                    new StoreProcedureParm (){ ParmName="UserId", ParmValue= formModel.Receiver_UserRefId.Replace("_",",") + "," + userId, isNumber=false }
                };
                var user = await _iGeneric_SP_Repository.CallStoreProcedureReaderAsync<SenderReceiverDetailViewModal>("dbo.sp_GetSenderReceiverDetail", storeProcedureParms);

                CheckListDataViewModel ChecklistData = null;
                if (formModel.CheckListFormJson != null && formModel.CheckListFormJson.Length > 0)
                {
                    ChecklistData = JsonConvert.DeserializeObject<CheckListDataViewModel>(formModel.CheckListFormJson);
                }

                int totalDays = GetWorkingDays(applicationAction.ActionDate, DateTimeOffset.UtcNow.Date);
                int holidays = _context.Holidays.Where(x => x.HolidayDate >= applicationAction.ActionDate && x.HolidayDate <= DateTimeOffset.UtcNow.Date && x.IsWeekend == false).Count();
                int totalWorkingDays = 0;
                if (holidays > 0)
                {
                    totalWorkingDays = (totalDays - holidays);
                }
                else
                {
                    totalWorkingDays = totalDays;
                }

                var end = DateTime.Now;
                var start = applicationAction.ActionDate;
                var weekend = new[] { DayOfWeek.Saturday, DayOfWeek.Sunday };

                var totalWorkingHours = Enumerable.Range(0, Convert.ToInt32(end.Subtract(start).TotalHours))
                     .Count(offset => !weekend.Contains(start.AddHours(offset).DayOfWeek));

                if (applicationAction != null)
                {
                    applicationAction.ActionTakenDaysCount = totalWorkingDays;
                    applicationAction.ActionTakenHoursCount = Convert.ToInt64(totalWorkingHours);
                    
                    var splitedUserIds = formModel.Receiver_UserRefId.Split("_");

                    foreach (var item in splitedUserIds)
                    {
                        var appAct = _context.ApplicationAction_ParallelProcesses.Where(x => x.ApplicationRefId == formModel.AppRefId && x.Receiver_UserRefId == item && x.IsAlive).FirstOrDefault();
                        if (appAct != null)
                        {
                            appAct.IsAlive = false;
                            _context.ApplicationAction_ParallelProcesses.Update(appAct);
                            _context.SaveChanges();
                        }
                    }


                    foreach (var item in splitedUserIds)
                    {
                        appActPP = new ApplicationAction_ParallelProcess() 
                        {
                            AppActionType = formModel.AppActionType,

                            Sender_UserRefId = userId,
                            Sender_ProfileRefId = user.Where(x => x.UserId == userId).Select(x => x.UserProfileId).FirstOrDefault(),
                            SenderRoleId = user.Where(x => x.UserId.ToLower() == userId.ToLower()).Select(x => x.RoleId).FirstOrDefault(),

                            Receiver_UserRefId = item,
                            Receiver_ProfileRefId = user.Where(x => x.UserId.ToLower() == item.ToLower()).Select(x => x.UserProfileId).FirstOrDefault(),
                            ReceiverRoleId = user.Where(x => x.UserId.ToLower() == item.ToLower()).Select(x => x.RoleId).FirstOrDefault(),

                            ActionDate = DateTime.Now,
                            ActionTakenDaysCount = totalWorkingDays,
                            ActionTakenHoursCount = Convert.ToInt64(totalWorkingHours),
                            Remarks = formModel.Remarks,
                            Checklist_Json = null,
                            Checklist_IsAllAgreed = false,
                            Checklist_FieldObjections = 0,
                            Checklist_DocObjections = 0,
                            IsDocumentUploaded = formModel.IsDocumentUploaded,
                            AppDocumentRefId = formModel.AppDocumentRefId,
                            IpAddress = formModel.IpAddress.ToString(),
                            Latitude = formModel.Latitude.ToString(),
                            Longitude = formModel.Longitude.ToString(),
                            ActionTakenModeType = formModel.ActionTakenModeType == null? ActionTakenModeTypeEnum.USER: formModel.ActionTakenModeType,
                            IsDeleted = false,
                            IsAlive = true,
                            ApplicationRefId = formModel.AppRefId
                        };

                        await _context.ApplicationAction_ParallelProcesses.AddAsync(appActPP);
                        await _context.SaveChangesAsync();

                    }

                    applicationAction.IsAlive = false;
                    _context.ApplicationAction_ParallelProcesses.Update(appActPP);
                    await _context.SaveChangesAsync();


                    
                }

                bool allOk = true;
                string licenseFilePath = "NA";
                string licenseDirName = "";
                var application = await _iGR_Application.GetAsync(x => x.AppId == formModel.AppRefId, null, x => x.ProjectSites).ConfigureAwait(false);
                if ((formModel.AppActionType == 200 || formModel.AppActionType == 206) && application.FirstOrDefault().ApplicationType != ApplicationTypeEnum.BUILDING_PLAN_HUD)//Approved
                {
                    if (formModel.AppActionType == 206)
                    {
                        allOk = await _iApplicationMamnagementRepository.SwitchApplicationLifeCycleType(formModel.AppRefId, ApplicationLifeCycleStatusTypeEnum.DEEMED_COMPLETED);
                    }
                    else if (formModel.AppActionType == 214)
                    {
                        allOk = await _iApplicationMamnagementRepository.SwitchApplicationLifeCycleType(formModel.AppRefId, ApplicationLifeCycleStatusTypeEnum.DEEMED_WITH_FEE_PENDING);
                    }
                    else if (formModel.AppActionType == 208)
                    {
                        allOk = await _iApplicationMamnagementRepository.SwitchApplicationLifeCycleType(formModel.AppRefId, ApplicationLifeCycleStatusTypeEnum.AUTO_APPROVEL_COMPLETED);
                    }
                    else
                    {
                        allOk = await _iApplicationMamnagementRepository.SwitchApplicationLifeCycleType(formModel.AppRefId, ApplicationLifeCycleStatusTypeEnum.APPROVED);
                    }

                    var tempFilePath = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "TempFiles")).Root;
                    if (File.Exists(tempFilePath + formModel.PdfNameGUID + ".pdf") && allOk)
                    {
                        licenseDirName = "AppForm_" + application.FirstOrDefault().ApplicationType.ToString();
                        var licenceRootDirUrl = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "License")).Root.ToString();
                        Directory.CreateDirectory(licenceRootDirUrl + licenseDirName);
                        licenseFilePath = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "License", licenseDirName)).Root;

                        formModel.PublicAppRefNum = _context.Applications.Where(x => x.AppId == formModel.AppRefId && x.IsDeleted == false).FirstOrDefault().PublicAppRefNum;

                        if (File.Exists(licenseFilePath + formModel.PublicAppRefNum + ".pdf"))
                        {
                            File.Delete(licenseFilePath + formModel.PublicAppRefNum + ".pdf");
                        }
                        File.Move(tempFilePath + formModel.PdfNameGUID + ".pdf", licenseFilePath + formModel.PublicAppRefNum + ".pdf");
                    }
                }

                else if ((formModel.AppActionType == 412 && application.FirstOrDefault().ApplicationType == ApplicationTypeEnum.BUILDING_PLAN_PSIEC)
                    || (formModel.AppActionType == 102 && application.FirstOrDefault().ApplicationType == ApplicationTypeEnum.BUILDING_PLAN_PSIEC))//Raised Fee PSIEC-FactoryWing
                {
                    if (allOk)
                    {
                        AppActionTypeEnum appActionType = AppActionTypeEnum.DEFAULT;
                        if (formModel.AppActionType == 412)
                        {
                            List<AppFeeDetail> appFeeDetail = new List<AppFeeDetail>();

                            if(application.FirstOrDefault().ApplicationType == ApplicationTypeEnum.BUILDING_PLAN_PSIEC)
                            {
                                appFeeDetail.Add(new AppFeeDetail()
                                {
                                    Amount = formModel.PsiecCessAmount,
                                    AppRefId = formModel.AppRefId,
                                    CalculatedOn = DateTime.Now,
                                    DedicatedDDOCode = null,
                                    DedicatedTreasurCode = null,
                                    Description = formModel.Remarks,
                                    FeeHeaderRefId = 48,
                                    HasDedicatedTreasuryCode = false,
                                    IsDeduductible = false,
                                    PaymentPartCounter = 1
                                });
                            }
                            else
                            {
                                appFeeDetail.Add(new AppFeeDetail()
                                {
                                    Amount = formModel.PsiecCessAmount,
                                    AppRefId = formModel.AppRefId,
                                    CalculatedOn = DateTime.Now,
                                    DedicatedDDOCode = null,
                                    DedicatedTreasurCode = null,
                                    Description = formModel.Remarks,
                                    FeeHeaderRefId = 19,
                                    HasDedicatedTreasuryCode = false,
                                    IsDeduductible = false,
                                    PaymentPartCounter = 1
                                });
                            }

                            appFeeDetail.Add(new AppFeeDetail()
                            {
                                Amount = formModel.PsiecProcessingFeeAmount,
                                AppRefId = formModel.AppRefId,
                                CalculatedOn = DateTime.Now,
                                DedicatedDDOCode = null,
                                DedicatedTreasurCode = null,
                                Description = formModel.Remarks,
                                FeeHeaderRefId = 39,
                                HasDedicatedTreasuryCode = false,
                                IsDeduductible = false,
                                PaymentPartCounter = 1
                            });

                            var res = await IncreasePaymentBatchCounter(formModel.AppRefId);
                            appFeeDetail = appFeeDetail.Select(x => { x.PaymentBatchCounter = res.ResponseDataModel; return x; }).ToList();
                            await _context.BulkInsertAsync<AppFeeDetail>(appFeeDetail);
                            appActionType = AppActionTypeEnum.FEE_RAISED;


                            // Insert into raise Fee table- 
                            IList<Payments_RaisedFee> listData = new List<Payments_RaisedFee>();
                            if(application.FirstOrDefault().ApplicationType == ApplicationTypeEnum.BUILDING_PLAN_PSIEC)
                            {
                                listData.Add(new Payments_RaisedFee
                                {
                                    AppRefId = formModel.AppRefId,
                                    FeeHeaderRefId = 48,
                                    PaymentBatchCounter = formModel.PaymentBatchCounter,
                                    AmountRaised = formModel.PsiecCessAmount,
                                    AmountAlreadyPaid = 0,
                                    IsFeeApplicable = true,
                                    Createddate = DateTime.UtcNow,
                                    LastModifiedDate = DateTime.UtcNow,
                                    ApplicationActionLogId = 0,
                                    NonTreasuryCode = "NA",
                                    Description = "NA",
                                    HasDedicatedTreasuryCode = true,
                                    DedicatedTreasurCode = "NA",
                                    DedicatedDDOCode = "NA"
                                });
                            }
                            else
                            {
                                listData.Add(new Payments_RaisedFee
                                {
                                    AppRefId = formModel.AppRefId,
                                    FeeHeaderRefId = 19,
                                    PaymentBatchCounter = formModel.PaymentBatchCounter,
                                    AmountRaised = formModel.PsiecCessAmount,
                                    AmountAlreadyPaid = 0,
                                    IsFeeApplicable = true,
                                    Createddate = DateTime.UtcNow,
                                    LastModifiedDate = DateTime.UtcNow,
                                    ApplicationActionLogId = 0,
                                    NonTreasuryCode = "NA",
                                    Description = "NA",
                                    HasDedicatedTreasuryCode = true,
                                    DedicatedTreasurCode = "NA",
                                    DedicatedDDOCode = "NA"
                                });
                            }

                            listData.Add(new Payments_RaisedFee
                            {
                                AppRefId = formModel.AppRefId,
                                FeeHeaderRefId = 39,
                                PaymentBatchCounter = formModel.PaymentBatchCounter,
                                AmountRaised = formModel.PsiecProcessingFeeAmount,
                                AmountAlreadyPaid = 0,
                                IsFeeApplicable = true,
                                Createddate = DateTime.UtcNow,
                                LastModifiedDate = DateTime.UtcNow,
                                ApplicationActionLogId = 0,
                                NonTreasuryCode = "NA",
                                Description = "NA",
                                HasDedicatedTreasuryCode = true,
                                DedicatedTreasurCode = "NA",
                                DedicatedDDOCode = "NA"
                            });
                            await _context.BulkInsertAsync(listData);
                        }
                        else if (formModel.AppActionType == 102)
                        {
                            appActionType = AppActionTypeEnum.FORWARD_FOR_OBJECTION;

                        }
                        //TimeLine_DepartmentWiseFinalAction dfa = new TimeLine_DepartmentWiseFinalAction()
                        //{
                        //    AppRefId = formModel.AppRefId,
                        //    DeptCodeType = DepartmentCodeTypeEnum.LABOUR,
                        //    AppActionType = appActionType,
                        //    AppActionLogRefId = applicationActionlogs.ApplicationActionLogId
                        //};
                        //await _context.AddAsync<TimeLine_DepartmentWiseFinalAction>(dfa);
                        //await _context.SaveChangesAsync();
                    }
                }

                else if (formModel.AppActionType == 201) // Rejected
                {
                    allOk = await _iApplicationMamnagementRepository.SwitchApplicationLifeCycleType(formModel.AppRefId, ApplicationLifeCycleStatusTypeEnum.REJECTED);
                    // Sent SMS Notification To User
                    var smsTemplateText = $"Dear Applicant,Your Application No. {application.FirstOrDefault().PublicAppRefNum} is Rejected from Department of Labour-PB LBAOUR";
                    var templateId = "1407172725588507473";
                    await _iNotificationManagerService.InitiateNotification(application.FirstOrDefault().ProjectSites.UserRefId,
                    new List<NotificationViewModel>()
                    {
                        new NotificationViewModel(){Body=smsTemplateText, NotificationMode=NotificationModeTypeEnum.MOBILE,NotificationPurpose=NotificationPurposeTypeEnum.INFO, Title=templateId}
                    });
                }
                else if (formModel.AppActionType == 404 || formModel.AppActionType == 407) // Objection
                {
                    allOk = await _iApplicationMamnagementRepository.SwitchApplicationLifeCycleType(formModel.AppRefId, ApplicationLifeCycleStatusTypeEnum.IN_OBJECTION);

                    // Sent SMS Notification To User
                    var smsTemplateText = $"Department of labour raised Objection on your Application No. {application.FirstOrDefault().PublicAppRefNum} . Please login at pbindustries.gov.in for resolve it.-PB  LABOUR";
                    var templateId = "1407172725521534794";
                    await _iNotificationManagerService.InitiateNotification(application.FirstOrDefault().ProjectSites.UserRefId,
                    new List<NotificationViewModel>()
                    {
                        new NotificationViewModel(){Body=smsTemplateText, NotificationMode=NotificationModeTypeEnum.MOBILE,NotificationPurpose=NotificationPurposeTypeEnum.INFO, Title=templateId}
                    });
                }
                else if (formModel.AppActionType == 202) // Deregistered
                {
                    allOk = await _iApplicationMamnagementRepository.SwitchApplicationLifeCycleType(formModel.AppRefId, ApplicationLifeCycleStatusTypeEnum.DEREGISTERED);
                }
                else if (formModel.AppActionType == 411) // Dormant
                {
                    allOk = await _iApplicationMamnagementRepository.SwitchApplicationLifeCycleType(formModel.AppRefId, ApplicationLifeCycleStatusTypeEnum.APPLICATION_DORMANT);
                }
                else if (formModel.AppActionType == 212) // Decline
                {
                    allOk = await _iApplicationMamnagementRepository.SwitchApplicationLifeCycleType(formModel.AppRefId, ApplicationLifeCycleStatusTypeEnum.APPLICATION_DECLINED);
                }
                if (allOk)
                {
                    var actionTimeLine = _context.AppActionTimeLines.Where(x => x.AppActionLogRefId == formModel.ApplicationActionLogId).ToList();
                    if (actionTimeLine != null)
                    {
                        actionTimeLine = actionTimeLine.Select(x => { x.IsProcessed = true; return x; }).ToList();
                        await _context.BulkInsertOrUpdateAsync(actionTimeLine);
                    }

                    if (application.FirstOrDefault().IsTimeLineFlow)
                    {
                        await _iAppTimeLineManagerService.SeedTimeLineData(formModel.AppRefId, appActPP.AppActionParallelProcessId);
                    }

                    if (formModel.AppDocumentRefId > 0)
                    {
                        var appDocument = _context.ApplicationDocuments.Where(x => x.AppDocId == formModel.AppDocumentRefId).FirstOrDefault();
                        appDocument.IsLocked = true;
                        _context.ApplicationDocuments.Update(appDocument);
                        await _context.SaveChangesAsync();
                    }

                    var licenceInfo = await _context.ApplicationLicenceNoMapping.Where(x => x.AppRefId == formModel.AppRefId).FirstOrDefaultAsync();
                    var statusDescription = await _context.ApplicationActionCodes.Where(x => x.ActionCode == formModel.AppActionType).FirstOrDefaultAsync();
                    //Int64 appActionLogId = await _context.ApplicationActionLogs.Where(x => x.ApplicationRefId == formModel.AppRefId).OrderByDescending(x => x.ApplicationActionLogId).Select(x => x.ApplicationActionLogId).FirstOrDefaultAsync().ConfigureAwait(false);

                    InvestPunjabShareStatusParmsViewModel statusReq = new InvestPunjabShareStatusParmsViewModel();

                    statusReq.AppRefId = formModel.AppRefId;
                    statusReq.ApplicationType = application.FirstOrDefault().ApplicationType;
                    statusReq.IPin = Convert.ToInt64(application.FirstOrDefault().InvestPunjab_Ipin);
                    statusReq.InvestPunjab_AppId = application.FirstOrDefault().InvestPunjab_AppId;
                    statusReq.StatusId = formModel.AppActionType;
                    statusReq.StatusDesc = statusDescription.ActionPublicName;
                    statusReq.Comments = (formModel.AppActionType == 404 || formModel.AppActionType == 401 || formModel.AppActionType == 402 || formModel.AppActionType == 301 || formModel.AppActionType == 201 || formModel.AppActionType == 202 || formModel.AppActionType == 200 || formModel.AppActionType == 407 || formModel.AppActionType == 212) ? formModel.Remarks : statusDescription.ActionName;
                    statusReq.SenderName = user.Where(x => x.UserId == userId).Select(x => x.FirstName).FirstOrDefault() + ' ' + user.Where(x => x.UserId == userId).Select(x => x.LastName).FirstOrDefault();
                    statusReq.SenderDesignation = user.Where(x => x.UserId == userId).Select(x => x.NormalizedName).FirstOrDefault();
                    int i = 0;
                    foreach (var item in formModel.Receiver_UserRefId.Split("_"))
                    {
                        if (i > 0)
                        {
                            statusReq.ReceiverName = statusReq.ReceiverName + " and ";
                            statusReq.ReceiverDesignation = statusReq.ReceiverDesignation + " and ";
                        }
                        statusReq.ReceiverName = user.Where(x => x.UserId == item).Select(x => x.FirstName).FirstOrDefault() + ' ' + user.Where(x => x.UserId == item).Select(x => x.LastName).FirstOrDefault();
                        statusReq.ReceiverDesignation = user.Where(x => x.UserId == item).Select(x => x.NormalizedName).FirstOrDefault();
                        i++;
                    }

                    statusReq.ClearanceIssuedOn = DateTime.Now;
                    statusReq.ClearanceExpiredOn = (application.FirstOrDefault().ApplicationType == ApplicationTypeEnum.SHOP_LICENCE ? (DateTime?)null : new DateTime(DateTime.Now.Year, 12, 31));
                    statusReq.LicenseNo = licenceInfo == null ? "NA" : licenceInfo.LicenceNumber.ToString();
                    statusReq.ClearanceFile = formModel.AppActionType == 200 || formModel.AppActionType == 206 ? Configuration.GetSection("ThirdPartyIntegrationConfigs").GetSection("WebHostRootUrl").Value + "License/" + licenseDirName + application.FirstOrDefault().PublicAppRefNum + ".pdf" : licenseFilePath;
                    statusReq.StatusDate = DateTime.Now;
                    statusReq.IntegrationSource = "LABOUR";
                    statusReq.IsdeemedApproval = "false";
                    statusReq.appActionType = (AppActionTypeEnum)formModel.AppActionType;
                    statusReq.appActionLogId = appActPP.AppActionParallelProcessId;

                    var response = await _iInvestPunjabShareStatusService.ShareStatusToBusinessFirst(statusReq);
                    var projectSite = _context.ProjectSites.Where(x => x.ProjectSiteId == application.FirstOrDefault().ProjectSiteRefId).FirstOrDefault();

                    foreach (var item in formModel.Receiver_UserRefId.Split("_"))
                    {
                        if (formModel.AppActionType == 200 || formModel.AppActionType == 206) //Approved Deemed
                        {
                            // Sent Email Notification 
                            TemplateResolveObjectionCaseLendedToOfficerNotificationViewModel mailTemplate = new TemplateResolveObjectionCaseLendedToOfficerNotificationViewModel()
                            {
                                OfficerName = user.Where(x => x.UserId == item).Select(x => x.FirstName).FirstOrDefault() + ' ' + user.Where(x => x.UserId == item).Select(x => x.LastName).FirstOrDefault(),
                                ApplicationPurposeTitle = EnumOps.GetEnumDescriptionName<ApplicationPurposeTypeEnum>(application.FirstOrDefault().ApplicationPurposeType.ToString()),
                                ApplicationTypeTitle = EnumOps.GetEnumDescriptionName<ApplicationTypeEnum>(application.FirstOrDefault().ApplicationType.ToString()),
                                EstablishmentName = projectSite.EstablishmentName,
                                PublicApplicationRefNo = application.FirstOrDefault().PublicAppRefNum,
                                StatusDescription = applicationAction.Remarks
                            };
                            var emailTemplateText = await _iNotificationManagerService.RenderToStringAsync("EmailTemplates/_templateObjectionResolvedAndCaseLendedToOfficerNotification", mailTemplate);

                            // Sent SMS Notification To User
                            var smsTemplateText = $"We are pleased to inform you that, your Application No. {application.FirstOrDefault().PublicAppRefNum} is got Approved from Department of Labour.-PB Labour";
                            var templateId = "1407172725558167954";
                            await _iNotificationManagerService.InitiateNotification(projectSite.UserRefId,
                            new List<NotificationViewModel>()
                            {
                            new NotificationViewModel(){Body=smsTemplateText, NotificationMode=NotificationModeTypeEnum.MOBILE,NotificationPurpose=NotificationPurposeTypeEnum.INFO, Title=templateId}
                            });
                        }
                        else if (formModel.AppActionType == 404) // Objection
                        {
                            TemplateResolveObjectionCaseLendedToOfficerNotificationViewModel mailTemplate = new TemplateResolveObjectionCaseLendedToOfficerNotificationViewModel()
                            {
                                OfficerName = user.Where(x => x.UserId == item).Select(x => x.FirstName).FirstOrDefault() + ' ' + user.Where(x => x.UserId == item).Select(x => x.LastName).FirstOrDefault(),
                                ApplicationPurposeTitle = EnumOps.GetEnumDescriptionName<ApplicationPurposeTypeEnum>(application.FirstOrDefault().ApplicationPurposeType.ToString()),
                                ApplicationTypeTitle = EnumOps.GetEnumDescriptionName<ApplicationTypeEnum>(application.FirstOrDefault().ApplicationType.ToString()),
                                EstablishmentName = projectSite.EstablishmentName,
                                PublicApplicationRefNo = application.FirstOrDefault().PublicAppRefNum,
                                StatusDescription = applicationAction.Remarks,
                                InvestPunjab_Ipin = application.FirstOrDefault().InvestPunjab_Ipin
                            };
                            var emailTemplateText = await _iNotificationManagerService.RenderToStringAsync("EmailTemplates/_templateObjectionResolvedAndCaseLendedToOfficerNotification", mailTemplate);

                            // Sent SMS Notification To User
                            var smsTemplateText = $"Department of labour raised Objection on your Application No. {application.FirstOrDefault().PublicAppRefNum} . Please login at pbindustries.gov.in for resolve it.-PB  LABOUR";
                            var templateId = "1407172725521534794";
                            await _iNotificationManagerService.InitiateNotification(application.FirstOrDefault().ProjectSites.UserRefId,
                            new List<NotificationViewModel>()
                            {
                        new NotificationViewModel(){Body=smsTemplateText, NotificationMode=NotificationModeTypeEnum.MOBILE,NotificationPurpose=NotificationPurposeTypeEnum.INFO, Title=templateId}
                            });
                        }
                        else if (formModel.AppActionType == 103 || formModel.AppActionType == 400 || formModel.AppActionType == 9) // Forwarded For further action
                        {
                            TemplateResolveObjectionCaseLendedToOfficerNotificationViewModel mailTemplate = new TemplateResolveObjectionCaseLendedToOfficerNotificationViewModel()
                            {
                                OfficerName = user.Where(x => x.UserId == item).Select(x => x.FirstName).FirstOrDefault() + ' ' + user.Where(x => x.UserId == item).Select(x => x.LastName).FirstOrDefault(),
                                ApplicationPurposeTitle = EnumOps.GetEnumDescriptionName<ApplicationPurposeTypeEnum>(application.FirstOrDefault().ApplicationPurposeType.ToString()),
                                ApplicationTypeTitle = EnumOps.GetEnumDescriptionName<ApplicationTypeEnum>(application.FirstOrDefault().ApplicationType.ToString()),
                                EstablishmentName = projectSite.EstablishmentName,
                                PublicApplicationRefNo = application.FirstOrDefault().PublicAppRefNum,
                                StatusDescription = applicationAction.Remarks,
                                InvestPunjab_Ipin = application.FirstOrDefault().InvestPunjab_Ipin
                            };
                            var emailTemplateText = await _iNotificationManagerService.RenderToStringAsync("EmailTemplates/_templateObjectionResolvedAndCaseLendedToOfficerNotification", mailTemplate);
                        }
                    }
                    resp.HasError = false;
                }
            }
            catch (Exception ex)
            {
                resp.HasError = true;
                resp.ErrorDesc = ex.Message;
                throw ex;
            }
            return resp;
        }


        public async Task<GenericResponseTemplateModel<RecordActionResponseViewModel>> RecordOfflineRaiseFeeAction(ApplicationActionViewModel formModel, string userId)
        {
            GenericResponseTemplateModel<RecordActionResponseViewModel> resp = new GenericResponseTemplateModel<RecordActionResponseViewModel>();
            try
            {
                var applications = await _context.Applications.Where(x => x.AppId == formModel.AppRefId).FirstOrDefaultAsync();
                var applicationAction = await _context.ApplicationActions.Where(x => x.ApplicationRefId == formModel.AppRefId).FirstOrDefaultAsync();
                if (applications.ApplicationType == ApplicationTypeEnum.BUILDING_PLAN_PSIEC)
                {
                    await RecordApplicationActionPSIEC(formModel, userId);
                }
                else
                {
                    List<StoreProcedureParm> storeProcedureParms = new List<StoreProcedureParm>()
                    {
                        new StoreProcedureParm (){ ParmName="UserId", ParmValue= formModel.Receiver_UserRefId + "," + userId, isNumber=false }
                    };
                    var user = await _iGeneric_SP_Repository.CallStoreProcedureReaderAsync<SenderReceiverDetailViewModal>("dbo.sp_GetSenderReceiverDetail", storeProcedureParms);

                    CheckListDataViewModel ChecklistData = null;
                    if (formModel.CheckListFormJson != null && formModel.CheckListFormJson.Length > 0)
                    {
                        ChecklistData = JsonConvert.DeserializeObject<CheckListDataViewModel>(formModel.CheckListFormJson);
                    }

                    int totalDays = GetWorkingDays(applicationAction.ActionDate, DateTimeOffset.UtcNow.Date);
                    int holidays = _context.Holidays.Where(x => x.HolidayDate >= applicationAction.ActionDate && x.HolidayDate <= DateTimeOffset.UtcNow.Date && x.IsWeekend == false).Count();
                    int totalWorkingDays = 0;
                    if (holidays > 0)
                    {
                        totalWorkingDays = (totalDays - holidays);
                    }
                    else
                    {
                        totalWorkingDays = totalDays;
                    }

                    var end = DateTime.Now;
                    var start = applicationAction.ActionDate;
                    var weekend = new[] { DayOfWeek.Saturday, DayOfWeek.Sunday };
                    var totalWorkingHours = Enumerable.Range(0, Convert.ToInt32(end.Subtract(start).TotalHours)).Count(offset => !weekend.Contains(start.AddHours(offset).DayOfWeek));
                    if (applicationAction != null)
                    {
                        applicationAction.ActionTakenDaysCount = totalWorkingDays;
                        applicationAction.ActionTakenHoursCount = Convert.ToInt64(totalWorkingHours);
                        applicationAction.ActionDate = DateTime.Now;
                        applicationAction.AppActionType = formModel.AppActionType;
                        applicationAction.Receiver_ProfileRefId = user.Where(x => x.UserId.ToLower() == formModel.Receiver_UserRefId.ToLower()).Select(x => x.UserProfileId).FirstOrDefault();  //user.Where(x => x.UserId == formModel.UserId).Select(x => x.UserProfileId).FirstOrDefault();
                        applicationAction.Receiver_UserRefId = formModel.Receiver_UserRefId;
                        applicationAction.Sender_ProfileRefId = user.Where(x => x.UserId == userId).Select(x => x.UserProfileId).FirstOrDefault();
                        applicationAction.Sender_UserRefId = userId;
                        applicationAction.Remarks = formModel.Remarks;
                        applicationAction.IsDocumentUploaded = formModel.IsDocumentUploaded;
                        applicationAction.AppDocumentRefId = formModel.AppDocumentRefId;
                        applicationAction.Checklist_Json = formModel.CheckListFormJson;
                        applicationAction.ReceiverRoleId = user.Where(x => x.UserId.ToLower() == formModel.Receiver_UserRefId.ToLower()).Select(x => x.RoleId).FirstOrDefault();
                        applicationAction.SenderRoleId = user.Where(x => x.UserId.ToLower() == userId.ToLower()).Select(x => x.RoleId).FirstOrDefault();
                        applicationAction.ActionTakenModeType = formModel.ActionTakenModeType != null ? formModel.ActionTakenModeType : 0;
                        if (ChecklistData != null)
                        {
                            applicationAction.Checklist_IsAllAgreed = !ChecklistData.CheckListNodes.Exists(x => x.IsVerified == false);
                            applicationAction.Checklist_FieldObjections = ChecklistData.CheckListNodes.Where(x => !x.IsVerified && !x.IsDocument).Count();
                            applicationAction.Checklist_DocObjections = ChecklistData.CheckListNodes.Where(x => !x.IsVerified && x.IsDocument).Count();
                        }
                        else
                        {
                            applicationAction.Checklist_IsAllAgreed = true;
                            applicationAction.Checklist_FieldObjections = 0;
                            applicationAction.Checklist_DocObjections = 0;
                        }
                        if (formModel.AppActionType == 206)
                        {
                            applicationAction.IpAddress = "Deemed";
                            applicationAction.Latitude = "Deemed";
                            applicationAction.Longitude = "Deemed";
                        }
                        else if (formModel.AppActionType == 208)
                        {
                            applicationAction.IpAddress = "Auto Approve";
                            applicationAction.Latitude = "Auto Approve";
                            applicationAction.Longitude = "Auto Approve";
                        }
                        else if (formModel.AppActionType == 411)
                        {
                            applicationAction.IpAddress = "Dormant";
                            applicationAction.Latitude = "Dormant";
                            applicationAction.Longitude = "Dormant";
                        }
                        else if (formModel.UserId == "C79F57CB-40BA-48EA-8336-327DC1B78701")
                        {
                            applicationAction.IpAddress = "Auto Action";
                            applicationAction.Latitude = "Auto Action";
                            applicationAction.Longitude = "Auto Action";
                        }
                        else
                        {
                            if (formModel.IpAddress == null)
                            {
                                applicationAction.IpAddress = "Auto Action";
                                applicationAction.Latitude = "Auto Action";
                                applicationAction.Longitude = "Auto Action";
                            }
                            else
                            {
                                applicationAction.IpAddress = formModel.IpAddress.ToString();
                                applicationAction.Latitude = formModel.Latitude.ToString();
                                applicationAction.Longitude = formModel.Longitude.ToString();
                            }
                        }
                        applicationAction.IsDeleted = false;
                        _iGR_ApplicationAction.Update(applicationAction);
                        await _iGR_ApplicationAction.SavechangeAsync();
                    }
                    else
                    {
                        //_iGR_ApplicationAction.Insert(formModel);
                        //await _iGR_ApplicationAction.SavechangeAsync();
                    }
                    ApplicationActionLog applicationActionlogs = new ApplicationActionLog()
                    {
                        ActionDate = applicationAction.ActionDate,
                        ActionTakenDaysCount = applicationAction.ActionTakenDaysCount,
                        ActionTakenHoursCount = applicationAction.ActionTakenHoursCount,
                        AppActionType = applicationAction.AppActionType,
                        Receiver_ProfileRefId = applicationAction.Receiver_ProfileRefId,
                        Receiver_UserRefId = applicationAction.Receiver_UserRefId,
                        Sender_ProfileRefId = applicationAction.Sender_ProfileRefId,
                        Sender_UserRefId = applicationAction.Sender_UserRefId,
                        Remarks = applicationAction.Remarks,
                        ReceiverRoleId = applicationAction.ReceiverRoleId,
                        ApplicationRefId = applicationAction.ApplicationRefId,
                        SenderRoleId = applicationAction.SenderRoleId,
                        Checklist_Json = applicationAction.Checklist_Json,
                        Checklist_IsAllAgreed = applicationAction.Checklist_IsAllAgreed,
                        Checklist_FieldObjections = applicationAction.Checklist_FieldObjections,
                        Checklist_DocObjections = applicationAction.Checklist_DocObjections,
                        IsDocumentUploaded = applicationAction.IsDocumentUploaded,
                        AppDocumentRefId = applicationAction.AppDocumentRefId,
                        IpAddress = applicationAction.IpAddress,
                        Latitude = applicationAction.Latitude,
                        Longitude = applicationAction.Longitude,
                        IsDeleted = false
                    };
                    _iGR_ApplicationActionLog.Insert(applicationActionlogs);
                    await _iGR_ApplicationActionLog.SavechangeAsync();
                    resp.ResponseDataModel = new RecordActionResponseViewModel()
                    {
                        AppActionId = applicationAction.AppActionId,
                        ApplicationActionLogId = applicationActionlogs.ApplicationActionLogId
                    };

                    // Update ApplicationActionLogId In BuildingPlanHUDPaymentDetail Table. 
                    if (applicationAction.AppActionType == 401)
                    {
                        var paymentList = _context.BuildingPlanHUDPaymentDetails.Where(x => x.AppRefId == formModel.AppRefId).ToList();
                        foreach (var payment in paymentList)
                        {
                            payment.ApplicationActionLogId = applicationActionlogs.ApplicationActionLogId;
                            _context.SaveChanges();
                        }
                    }
                    bool allOk = true;

                    string licenseFilePath = "NA";
                    string licenseDirName = "";
                    var application = await _iGR_Application.GetAsync(x => x.AppId == formModel.AppRefId, null, x => x.ProjectSites).ConfigureAwait(false);
                    if (formModel.AppActionType == 9)
                    {
                        allOk = await _iApplicationMamnagementRepository.SwitchApplicationLifeCycleType(formModel.AppRefId, ApplicationLifeCycleStatusTypeEnum.IN_PROCESS);
                    }
                    if (allOk)
                    {
                        var actionTimeLine = _context.AppActionTimeLines.Where(x => x.AppActionLogRefId == formModel.ApplicationActionLogId).ToList();
                        if (actionTimeLine != null)
                        {
                            actionTimeLine = actionTimeLine.Select(x => { x.IsProcessed = true; return x; }).ToList();
                            await _context.BulkInsertOrUpdateAsync(actionTimeLine);
                        }
                        if (application.FirstOrDefault().IsTimeLineFlow)
                        {
                            await _iAppTimeLineManagerService.SeedTimeLineData(formModel.AppRefId, 0);
                        }

                        if (formModel.AppDocumentRefId > 0)
                        {
                            var appDocument = _context.ApplicationDocuments.Where(x => x.AppDocId == formModel.AppDocumentRefId).FirstOrDefault();
                            appDocument.IsLocked = true;
                            _context.ApplicationDocuments.Update(appDocument);
                            await _context.SaveChangesAsync();
                        }
                        var licenceInfo = await _context.ApplicationLicenceNoMapping.Where(x => x.AppRefId == formModel.AppRefId).FirstOrDefaultAsync();
                        var statusDescription = await _context.ApplicationActionCodes.Where(x => x.ActionCode == formModel.AppActionType).FirstOrDefaultAsync();
                        Int64 appActionLogId = await _context.ApplicationActionLogs.Where(x => x.ApplicationRefId == formModel.AppRefId).OrderByDescending(x => x.ApplicationActionLogId).Select(x => x.ApplicationActionLogId).FirstOrDefaultAsync().ConfigureAwait(false);

                        InvestPunjabShareStatusParmsViewModel statusReq = new InvestPunjabShareStatusParmsViewModel();
                        statusReq.AppRefId = formModel.AppRefId;
                        statusReq.ApplicationType = application.FirstOrDefault().ApplicationType;
                        statusReq.IPin = Convert.ToInt64(application.FirstOrDefault().InvestPunjab_Ipin);
                        statusReq.InvestPunjab_AppId = application.FirstOrDefault().InvestPunjab_AppId;
                        statusReq.StatusId = formModel.AppActionType;
                        statusReq.StatusDesc = statusDescription.ActionPublicName;
                        statusReq.Comments = (formModel.AppActionType == 404 || formModel.AppActionType == 401 || formModel.AppActionType == 402 || formModel.AppActionType == 301 || formModel.AppActionType == 201 || formModel.AppActionType == 202 || formModel.AppActionType == 200 || formModel.AppActionType == 407 || formModel.AppActionType == 212) ? formModel.Remarks : statusDescription.ActionName;
                        statusReq.SenderName = user.Where(x => x.UserId == userId).Select(x => x.FirstName).FirstOrDefault() + ' ' + user.Where(x => x.UserId == userId).Select(x => x.LastName).FirstOrDefault();
                        statusReq.SenderDesignation = user.Where(x => x.UserId == userId).Select(x => x.NormalizedName).FirstOrDefault();
                        statusReq.ReceiverName = user.Where(x => x.UserId == formModel.Receiver_UserRefId).Select(x => x.FirstName).FirstOrDefault() + ' ' + user.Where(x => x.UserId == formModel.Receiver_UserRefId).Select(x => x.LastName).FirstOrDefault();
                        statusReq.ReceiverDesignation = user.Where(x => x.UserId == formModel.Receiver_UserRefId).Select(x => x.NormalizedName).FirstOrDefault();
                        statusReq.ClearanceIssuedOn = DateTime.Now;
                        statusReq.ClearanceExpiredOn = (application.FirstOrDefault().ApplicationType == ApplicationTypeEnum.SHOP_LICENCE ? (DateTime?)null : new DateTime(DateTime.Now.Year, 12, 31));
                        statusReq.LicenseNo = licenceInfo == null ? "NA" : licenceInfo.LicenceNumber.ToString();
                        statusReq.ClearanceFile = (application.FirstOrDefault().ApplicationType == ApplicationTypeEnum.BUILDING_PLAN_HUD && (formModel.AppActionType == 200 || formModel.AppActionType == 204)) || formModel.AppActionType == 200 || formModel.AppActionType == 206 ? Configuration.GetSection("ThirdPartyIntegrationConfigs").GetSection("WebHostRootUrl").Value + "License/" + licenseDirName + "/" + formModel.PublicAppRefNum + ".pdf" : licenseFilePath;
                        statusReq.StatusDate = DateTime.Now;
                        statusReq.IntegrationSource = "LABOUR";
                        statusReq.IsdeemedApproval = (formModel.AppActionType == 205 || formModel.AppActionType == 206) ? "true" : "false";
                        statusReq.appActionType = (AppActionTypeEnum)formModel.AppActionType;
                        statusReq.appActionLogId = appActionLogId;
                        var response = await _iInvestPunjabShareStatusService.ShareStatusToBusinessFirst(statusReq);
                        var projectSite = _context.ProjectSites.Where(x => x.ProjectSiteId == application.FirstOrDefault().ProjectSiteRefId).FirstOrDefault();

                        // Sent Notification
                        TemplateResolveObjectionCaseLendedToOfficerNotificationViewModel mailTemplate = new TemplateResolveObjectionCaseLendedToOfficerNotificationViewModel()
                        {
                            OfficerName = user.Where(x => x.UserId == formModel.Receiver_UserRefId).Select(x => x.FirstName).FirstOrDefault() + ' ' + user.Where(x => x.UserId == formModel.Receiver_UserRefId).Select(x => x.LastName).FirstOrDefault(),
                            ApplicationPurposeTitle = EnumOps.GetEnumDescriptionName<ApplicationPurposeTypeEnum>(application.FirstOrDefault().ApplicationPurposeType.ToString()),
                            ApplicationTypeTitle = EnumOps.GetEnumDescriptionName<ApplicationTypeEnum>(application.FirstOrDefault().ApplicationType.ToString()),
                            EstablishmentName = projectSite.EstablishmentName,
                            PublicApplicationRefNo = application.FirstOrDefault().PublicAppRefNum,
                            StatusDescription = applicationAction.Remarks,
                            InvestPunjab_Ipin = application.FirstOrDefault().InvestPunjab_Ipin
                        };
                        var emailTemplateText = await _iNotificationManagerService.RenderToStringAsync("EmailTemplates/_templateObjectionResolvedAndCaseLendedToOfficerNotification", mailTemplate);
                        resp.HasError = false;
                    }
                }
            }
            catch (Exception ex)
            {
                resp.HasError = true;
                resp.ErrorDesc = ex.Message;
                throw ex;
            }
            return resp;
        }
    }
}


