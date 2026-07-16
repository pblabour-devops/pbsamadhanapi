using Microsoft.EntityFrameworkCore;
using pbsamadhannetcoreapi.CommonUtiliteis;
using pbsamadhannetcoreapi.Models;
using pbsamadhannetcoreapi.Repositories.Implementations;
using pbsamadhannetcoreapi.Services.Implementations;
using pbsamadhannetcoreapi.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EFCore.BulkExtensions;
using Microsoft.Extensions.FileProviders;
using System.IO;
using static pbsamadhannetcoreapi.CommonUtiliteis.ReflectionOps;
using Newtonsoft.Json;
using System.Text;
using System.Globalization;
using Microsoft.Extensions.Configuration;
using DocumentFormat.OpenXml.Bibliography;
using Org.BouncyCastle.Security.Certificates;
using DocumentFormat.OpenXml.Office2010.Excel;
using Microsoft.AspNetCore.Identity;
namespace pbsamadhannetcoreapi.Services
{

    public class InspectionService : IInspectionService
    {
        private readonly IGenericRepository<Inspection_Form_Factory_Part_I_General> _iGR_Inspection_Form_Factory_Part_I_General;
        private readonly IGenericRepository<Inspection_Form_Factory_Part_II_FactoryDetail> _iGR_Inspection_Form_Factory_Part_II_FactoryDetail;
        private readonly IGenericRepository<Inspection_Form_Factory_Part_III_InspectionReport> _iGR_Inspection_Form_Factory_Part_III_InspectionReport;
        private readonly IGenericRepository<Inspection_Form_Factory_Part_III_MusterRoll> _iGR_Inspection_Form_Factory_Part_III_MusterRoll;
        private readonly IGenericRepository<Inspection_Form_Factory_Part_III_Health> _iGR_Inspection_Form_Factory_Part_III_Health;
        private readonly IGenericRepository<Inspection_Form_Factory_Part_III_Safety> _iGR_Inspection_Form_Factory_Part_III_Safety;
        private readonly IGenericRepository<Inspection_Form_Factory_Part_III_Welfare> _iGR_Inspection_Form_Factory_Part_III_Welfare;
        private readonly IGenericRepository<Inspection_Form_Factory_Part_III_General> _iGR_Inspection_Form_Factory_Part_III_General;
        private readonly IGenericRepository<Inspection_Form_Factory_Part_III_MajorAccidentHazard> _iGR_Inspection_Form_Factory_Part_III_MajorAccidentHazard;
        private readonly IGenericRepository<Inspection_Form_Factory_Part_III_DangerousOperation> _iGR_Inspection_Form_Factory_Part_III_DangerousOperation;
        private readonly IGenericRepository<Inspection_Master> _iGR_Inspection_Master;
        private readonly IGenericRepository<InspectionTransferLogs> _iGR_InspectionTransferLogs;
        private readonly IGenericRepository<Inspection_Randomization> _iGR_Inspection_Randomization;
        private readonly IGenericRepository<Inspection_LockInfo> _iGR_Inspection_LockInfo;
        private readonly IApplicationManagementService<Inspection_Form_Factory_Part_I_General> _iApplicationMamnagementService;
        private readonly IGeneric_SP_Repository _iGeneric_SP_Repository;
        private readonly AppDbContext _context;
        private readonly IGenericRepository<InspectionDocument> _iGR_InspectionDocument;
        private readonly UserManager<User> _userManager;

        private IAuthService _iAuthService;
        private readonly IGenericRepository<Inspection_Form_Labour_Part_I_General> _iGR_Inspection_Form_Labour_Part_I_General;
        private readonly IGenericRepository<Inspection_Form_Labour_Part_II_FactoryDetail> _iGR_Inspection_Form_Labour_Part_II_FactoryDetail;
        private readonly IGenericRepository<Inspection_Form_Labour_Part_III_MusterRoll> _iGR_Inspection_Form_Labour_Part_III_MusterRoll;
        private readonly IGenericRepository<Inspection_Form_Labour_Part_III_EqualEnumerationAct> _iGR_Inspection_Form_Labour_Part_III_EqualEnumerationAct;
        private readonly IGenericRepository<Inspection_Form_Labour_III_MinimumWageAct> _iGR_Inspection_Form_Labour_III_MinimumWageAct;
        private readonly IGenericRepository<Inspection_Form_Labour_III_PaymentWagesAct> _iGR_Inspection_Form_Labour_III_PaymentWagesAct;
        private readonly IGenericRepository<Inspection_Form_Labour_III_PaymentBonusAct_StatutoryReport> _iGR_Inspection_Form_Labour_III_PaymentBonusAct_StatutoryReport;
        private readonly IGenericRepository<Inspection_Form_Labour_III_PaymentBonusAct_Part_B_Attachment> _iGR_Inspection_Form_Labour_III_PaymentBonusAct_Part_B_Attachment;
        private readonly IGenericRepository<Inspection_Form_Labour_III_ChildAndAdolescentLabourAct> _iGR_Inspection_Form_Labour_III_ChildAndAdolescentLabourAct;
        private readonly IGenericRepository<Inspection_Form_Labour_III_NationalAndFestivalHolidays> _iGR_Inspection_Form_Labour_III_NationalAndFestivalHolidays;
        private readonly IGenericRepository<Inspection_Form_Labour_III_MaternityBenefitAct> _iGR_Inspection_Form_Labour_III_MaternityBenefitAct;
        private readonly IGenericRepository<Inspection_Form_Labour_III_ContractLabourAct> _iGR_Inspection_Form_Labour_III_ContractLabourAct;
        private readonly IGenericRepository<Inspection_Form_Labour_III_InterStateMigrantWorkmenAct> _iGR_Inspection_Form_Labour_III_InterStateMigrantWorkmenAct;
        private readonly IGenericRepository<Inspection_Form_Labour_III_LabourWelfareFund_Act> _iGR_Inspection_Form_Labour_III_LabourWelfareFund_Act;
        private readonly IGenericRepository<Inspection_Form_Labour_III_GratuityAct> _iGR_Inspection_Form_Labour_III_GratuityAct;
        private readonly IGenericRepository<Inspection_Form_Labour_III_IndustrialEmploymentAct> _iGR_Inspection_Form_Labour_III_IndustrialEmploymentAct;
        private readonly IGenericRepository<Inspection_Form_Labour_III_BOCW_Act> _iGR_Inspection_Form_Labour_III_BOCW_Act;
        private readonly IGenericRepository<Inspection_Form_Labour_III_ShopAct> _iGR_Inspection_Form_Labour_III_ShopAct;
        private readonly IGenericRepository<Inspection_Form_Labour_III_Observations> _iGR_Inspection_Form_Labour_III_Observations;
        private readonly INotificationManagerService _iNotificationManagerService;
        private readonly IGenericRepository<InspectionComplianceLog> _iGR_InspectionComplianceLog;
        private readonly IGenericRepository<Inspection_ViolationComplianceReminder> _iGR_Inspection_ViolationComplianceReminder;
        private readonly IGenericRepository<InspectionUserMapping> _iGR_InspectionUserMapping;
        private readonly IGenericRepository<Inspection_UserMapping_Logs> _iGR_Inspection_UserMapping_Logs;
        private readonly ICircleManager _iCircleManager;
        private readonly IGenericRepository<Randomization_Initialization> _iGR_Randomization_Initialization;
        public IConfiguration Configuration { get; }
        
        public InspectionService(
            IGenericRepository<Inspection_Form_Factory_Part_I_General> iGR_Inspection_Form_Factory_Part_I_General,
            IGenericRepository<Inspection_Form_Factory_Part_II_FactoryDetail> iGR_Inspection_Form_Factory_Part_II_FactoryDetail,
            IGenericRepository<Inspection_Form_Factory_Part_III_InspectionReport> iGR_Inspection_Form_Factory_Part_III_InspectionReport,
            IGenericRepository<Inspection_Form_Factory_Part_III_MusterRoll> iGR_Inspection_Form_Factory_Part_III_MusterRoll,
            IGenericRepository<Inspection_Form_Factory_Part_III_Health> iGR_Inspection_Form_Factory_Part_III_Health,
            IGenericRepository<Inspection_Form_Factory_Part_III_Safety> iGR_Inspection_Form_Factory_Part_III_Safety,
            IGenericRepository<Inspection_Form_Factory_Part_III_Welfare> iGR_Inspection_Form_Factory_Part_III_Welfare,
            IGenericRepository<Inspection_Form_Factory_Part_III_General> iGR_Inspection_Form_Factory_Part_III_General,
            IGenericRepository<Inspection_Form_Factory_Part_III_MajorAccidentHazard> iGR_Inspection_Form_Factory_Part_III_MajorAccidentHazard,
            IGenericRepository<Inspection_Form_Factory_Part_III_DangerousOperation> iGR_Inspection_Form_Factory_Part_III_DangerousOperation,
             IGenericRepository<Inspection_Master> IGR_Inspection_Master,
             IGenericRepository<InspectionTransferLogs> IGR_InspectionTransferLogs,
             IGenericRepository<Inspection_Randomization> IGR_Inspection_Randomization,
             IGenericRepository<Inspection_LockInfo> IGR_Inspection_LockInfo,
             IGeneric_SP_Repository iGeneric_SP_Repository,
             IAuthService authService,
            IApplicationManagementService<Inspection_Form_Factory_Part_I_General> IApplicationMamnagementService,
            AppDbContext context,
            IGenericRepository<Inspection_Form_Labour_Part_I_General> iGR_Inspection_Form_Labour_Part_I_General,
            IGenericRepository<Inspection_Form_Labour_Part_II_FactoryDetail> iGR_Inspection_Form_Labour_Part_II_FactoryDetail,
            IGenericRepository<Inspection_Form_Labour_Part_III_MusterRoll> iGR_Inspection_Form_Labour_Part_III_MusterRoll,
            IGenericRepository<Inspection_Form_Labour_Part_III_EqualEnumerationAct> iGR_Inspection_Form_Labour_Part_III_EqualEnumerationAct,
            IGenericRepository<Inspection_Form_Labour_III_MinimumWageAct> iGR_Inspection_Form_Labour_III_MinimumWageAct,
            IGenericRepository<Inspection_Form_Labour_III_PaymentWagesAct> iGR_Inspection_Form_Labour_III_PaymentWagesAct,
            IGenericRepository<Inspection_Form_Labour_III_PaymentBonusAct_StatutoryReport> iGR_Inspection_Form_Labour_III_PaymentBonusAct_StatutoryReport,
            IGenericRepository<Inspection_Form_Labour_III_PaymentBonusAct_Part_B_Attachment> iGR_Inspection_Form_Labour_III_PaymentBonusAct_Part_B_Attachment,
            IGenericRepository<Inspection_Form_Labour_III_ChildAndAdolescentLabourAct> iGR_Inspection_Form_Labour_III_ChildAndAdolescentLabourAct,
            IGenericRepository<Inspection_Form_Labour_III_NationalAndFestivalHolidays> iGR_Inspection_Form_Labour_III_NationalAndFestivalHolidays,
            IGenericRepository<Inspection_Form_Labour_III_MaternityBenefitAct> iGR_Inspection_Form_Labour_III_MaternityBenefitAct,
            IGenericRepository<Inspection_Form_Labour_III_ContractLabourAct> iGR_Inspection_Form_Labour_III_ContractLabourAct,
            IGenericRepository<Inspection_Form_Labour_III_InterStateMigrantWorkmenAct> iGR_Inspection_Form_Labour_III_InterStateMigrantWorkmenAct,
            IGenericRepository<Inspection_Form_Labour_III_LabourWelfareFund_Act> iGR_Inspection_Form_Labour_III_LabourWelfareFund_Act,
            IGenericRepository<Inspection_Form_Labour_III_GratuityAct> iGR_Inspection_Form_Labour_III_GratuityAct,
            IGenericRepository<Inspection_Form_Labour_III_IndustrialEmploymentAct> iGR_Inspection_Form_Labour_III_IndustrialEmploymentAct,
            IGenericRepository<Inspection_Form_Labour_III_BOCW_Act> iGR_Inspection_Form_Labour_III_BOCW_Act,
            IGenericRepository<Inspection_Form_Labour_III_ShopAct> iGR_Inspection_Form_Labour_III_ShopAct,
            IGenericRepository<Inspection_Form_Labour_III_Observations> iGR_Inspection_Form_Labour_III_Observations,
            INotificationManagerService iNotificationManagerService,
            IGenericRepository<InspectionComplianceLog> iGR_InspectionComplianceLog,
            IGenericRepository<Inspection_ViolationComplianceReminder> iGR_Inspection_ViolationComplianceReminder,
            IGenericRepository<InspectionDocument> iGR_InspectionDocument,
            IGenericRepository<InspectionUserMapping> iGR_InspectionUserMapping,
            IGenericRepository<Inspection_UserMapping_Logs> iGR_Inspection_UserMapping_Logs,
            ICircleManager circleManager, 
            IConfiguration configuration,
            IGenericRepository<Randomization_Initialization> iGR_Randomization_Initialization,
            UserManager<User> userManager)
        {
            _iGR_Inspection_Form_Factory_Part_I_General = iGR_Inspection_Form_Factory_Part_I_General;
            _iGR_Inspection_Form_Factory_Part_II_FactoryDetail = iGR_Inspection_Form_Factory_Part_II_FactoryDetail;
            _iGR_Inspection_Form_Factory_Part_III_InspectionReport = iGR_Inspection_Form_Factory_Part_III_InspectionReport;
            _iGR_Inspection_Form_Factory_Part_III_MusterRoll = iGR_Inspection_Form_Factory_Part_III_MusterRoll;
            _iGR_Inspection_Form_Factory_Part_III_Health = iGR_Inspection_Form_Factory_Part_III_Health;
            _iGR_Inspection_Form_Factory_Part_III_Safety = iGR_Inspection_Form_Factory_Part_III_Safety;
            _iGR_Inspection_Form_Factory_Part_III_Welfare = iGR_Inspection_Form_Factory_Part_III_Welfare;
            _iGR_Inspection_Form_Factory_Part_III_General = iGR_Inspection_Form_Factory_Part_III_General;
            _iGR_Inspection_Form_Factory_Part_III_MajorAccidentHazard = iGR_Inspection_Form_Factory_Part_III_MajorAccidentHazard;
            _iGR_Inspection_Form_Factory_Part_III_DangerousOperation = iGR_Inspection_Form_Factory_Part_III_DangerousOperation;
            _iGR_Inspection_Master = IGR_Inspection_Master;
            _iGR_InspectionTransferLogs = IGR_InspectionTransferLogs;
            _iGR_Inspection_Randomization = IGR_Inspection_Randomization;
            _iGR_Inspection_LockInfo = IGR_Inspection_LockInfo;
            _iApplicationMamnagementService = IApplicationMamnagementService;
            _iGeneric_SP_Repository = iGeneric_SP_Repository;
            _iAuthService = authService;
            _context = context;
            _iGR_Inspection_Form_Labour_Part_I_General = iGR_Inspection_Form_Labour_Part_I_General;
            _iGR_Inspection_Form_Labour_Part_II_FactoryDetail = iGR_Inspection_Form_Labour_Part_II_FactoryDetail;
            _iGR_Inspection_Form_Labour_Part_III_MusterRoll = iGR_Inspection_Form_Labour_Part_III_MusterRoll;
            _iGR_Inspection_Form_Labour_Part_III_EqualEnumerationAct = iGR_Inspection_Form_Labour_Part_III_EqualEnumerationAct;
            _iGR_Inspection_Form_Labour_III_MinimumWageAct = iGR_Inspection_Form_Labour_III_MinimumWageAct;
            _iGR_Inspection_Form_Labour_III_PaymentWagesAct = iGR_Inspection_Form_Labour_III_PaymentWagesAct;
            _iGR_Inspection_Form_Labour_III_PaymentBonusAct_StatutoryReport = iGR_Inspection_Form_Labour_III_PaymentBonusAct_StatutoryReport;
            _iGR_Inspection_Form_Labour_III_PaymentBonusAct_Part_B_Attachment = iGR_Inspection_Form_Labour_III_PaymentBonusAct_Part_B_Attachment;
            _iGR_Inspection_Form_Labour_III_ChildAndAdolescentLabourAct = iGR_Inspection_Form_Labour_III_ChildAndAdolescentLabourAct;
            _iGR_Inspection_Form_Labour_III_NationalAndFestivalHolidays = iGR_Inspection_Form_Labour_III_NationalAndFestivalHolidays;
            _iGR_Inspection_Form_Labour_III_MaternityBenefitAct = iGR_Inspection_Form_Labour_III_MaternityBenefitAct;
            _iGR_Inspection_Form_Labour_III_ContractLabourAct = iGR_Inspection_Form_Labour_III_ContractLabourAct;
            _iGR_Inspection_Form_Labour_III_InterStateMigrantWorkmenAct = iGR_Inspection_Form_Labour_III_InterStateMigrantWorkmenAct;
            _iGR_Inspection_Form_Labour_III_LabourWelfareFund_Act = iGR_Inspection_Form_Labour_III_LabourWelfareFund_Act;
            _iGR_Inspection_Form_Labour_III_GratuityAct = iGR_Inspection_Form_Labour_III_GratuityAct;
            _iGR_Inspection_Form_Labour_III_IndustrialEmploymentAct = iGR_Inspection_Form_Labour_III_IndustrialEmploymentAct;
            _iGR_Inspection_Form_Labour_III_BOCW_Act = iGR_Inspection_Form_Labour_III_BOCW_Act;
            _iGR_Inspection_Form_Labour_III_ShopAct = iGR_Inspection_Form_Labour_III_ShopAct;
            _iGR_Inspection_Form_Labour_III_Observations = iGR_Inspection_Form_Labour_III_Observations;
            _iNotificationManagerService = iNotificationManagerService;
            _iGR_InspectionComplianceLog = iGR_InspectionComplianceLog;
            _iGR_Inspection_ViolationComplianceReminder = iGR_Inspection_ViolationComplianceReminder;
            _iGR_InspectionDocument = iGR_InspectionDocument;
            _iGR_InspectionUserMapping = iGR_InspectionUserMapping;
            _iGR_Inspection_UserMapping_Logs = iGR_Inspection_UserMapping_Logs;
            _iCircleManager = circleManager;
            Configuration = configuration;
            _iGR_Randomization_Initialization = iGR_Randomization_Initialization;
            _userManager = userManager;
        }
        #region General Details

        public async Task<GenericResponseTemplateModel<List<Inspection_RandomizeDashboardDataViewModel>>> GetInspection_Randomization(string userId, Int64 id, string searchCode, int pageNo, int pageSize, string sortColumn, string sortOrder, string filterArray)
            {
            GenericResponseTemplateModel<List<Inspection_RandomizeDashboardDataViewModel>> genericFormModel = new GenericResponseTemplateModel<List<Inspection_RandomizeDashboardDataViewModel>>();
            try
            {
                List<StoreProcedureParm> storeProcedureParms = new List<StoreProcedureParm>()
                {
                    new StoreProcedureParm (){ isNumber=false,ParmName="UserRefId", ParmValue=userId.ToString()},

                     new StoreProcedureParm (){isNumber=false, ParmName="SearchCode",ParmValue= searchCode==null ? "": searchCode},
                            new StoreProcedureParm (){isNumber=true, ParmName="PageNo", ParmValue=pageNo.ToString()},
                            new StoreProcedureParm (){isNumber=true, ParmName="PageSize", ParmValue=pageSize.ToString()},
                            new StoreProcedureParm (){isNumber=false, ParmName="SortColumn", ParmValue=sortColumn.ToString()},
                            new StoreProcedureParm (){isNumber=false, ParmName="SortOrder", ParmValue = sortOrder=="1" ? "ASC" : "DESC" }
                };
                genericFormModel.ResponseDataModel = await _iGeneric_SP_Repository.CallStoreProcedureReaderAsync<Inspection_RandomizeDashboardDataViewModel>("sp_Inspection_GetMonthYearFactoryCircleWiseDashboardData", storeProcedureParms);

            }
            catch (Exception ex)
            {
                genericFormModel.HasError = true;
                genericFormModel.ErrorDesc = ex.Message;
            }
            return genericFormModel;
        }
        public async Task<GenericResponseTemplateModel<List<InspectioninfoViewModel>>> Get_InspectionsByRandomization(Int64 id, Int64 factoryRefId,string roleName, string userRefId)
        {
            GenericResponseTemplateModel<List<InspectioninfoViewModel>> genericFormModel = new GenericResponseTemplateModel<List<InspectioninfoViewModel>>();
            try
            {
                List<StoreProcedureParm> storeProcedureParms = new List<StoreProcedureParm>()
                {
                    new StoreProcedureParm (){ ParmName="RandomizationId", ParmValue=id.ToString(),isNumber=true},
                    new StoreProcedureParm (){ ParmName="FactoryRefId", ParmValue=factoryRefId.ToString(),isNumber=true,},
                    new StoreProcedureParm (){ ParmName="RoleName", ParmValue=roleName.ToString(),isNumber=false,},
                    new StoreProcedureParm (){ ParmName="userRefId", ParmValue=userRefId.ToString(),isNumber=false,}
                };
                genericFormModel.ResponseDataModel = await _iGeneric_SP_Repository.CallStoreProcedureReaderAsync<InspectioninfoViewModel>("sp_GetInspectionsByRandomizationIdAndfactoryCircleId", storeProcedureParms);
                if (genericFormModel.ResponseDataModel.Count() > 0)
                {
                    genericFormModel.ResponseDataModel.Select(x => { x.EstablishmentName = x.EstbDetail.Split("|")[0]; x.EstablishmentAddress = x.EstbDetail.Split("|")[1]; x.ContactNumber = x.EstbDetail.Split("|")[2]; x.Email = x.EstbDetail.Split("|")[3]; return x; }).ToList();
                }

            }
            catch (Exception ex)
            {
                genericFormModel.HasError = true;
                genericFormModel.ErrorDesc = ex.Message;
            }
            return genericFormModel;
        }

        public async Task<GenericFormModel<LabourCircleUserDetailsViewModel>> GetInspection_LabourCircleUserDetails(Int64 labourCircleRefId, Int64 labourWingProfileRefId)
        {
            GenericFormModel<LabourCircleUserDetailsViewModel> genericFormModel = new GenericFormModel<LabourCircleUserDetailsViewModel>();
            try
            {
                genericFormModel.FormModel = (from up in _context.UserProfiles
                                              join upm in _context.UserProfileMapping on up.UserProfileId equals upm.UserProfileRefId
                                              join ucm in _context.UserCircleMappings on upm.UserRefId equals ucm.UserRefId
                                              where ucm.LabourCircleRefId == labourCircleRefId && up.UserProfileId == labourWingProfileRefId
                                              select new LabourCircleUserDetailsViewModel
                                              {
                                                  FullName = up.FirstName + " " + up.LastName,
                                                  Email = up.Email,
                                                  MobileNo = up.MobileNo,
                                                  LabourCircleRefId = (long)ucm.LabourCircleRefId
                                              }).FirstOrDefault();
            }
            catch (Exception ex)
            {
                genericFormModel.HasError = true;
                genericFormModel.ErrorDesc = ex.Message;
            }
            return genericFormModel;
        }
        public async Task<GenericFormModel<Inspection_Form_Factory_Part_I_General>> GetIncpectionFormFactoryPartIGeneralDetail(long id)
        {
            GenericFormModel<Inspection_Form_Factory_Part_I_General> genericFormModel = new GenericFormModel<Inspection_Form_Factory_Part_I_General>();
            try
            {
                genericFormModel.ListTemplateLists = new List<ListTemplate>();
                if (id != 0) //Existing Record
                {
                    var parentWithChildObject = await _iGR_Inspection_Form_Factory_Part_I_General
                       .GetAsync(x => x.InspectionRefId == id,  //Conditions         
                         null,                //Orders
                         x => x.Inspection_Master)      //Includes
                       .ConfigureAwait(false);
                    genericFormModel.FormModel = parentWithChildObject.FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                genericFormModel.HasError = true;
                genericFormModel.ErrorDesc = ex.Message;
            }
            return genericFormModel;
        }

        public async Task<GenericFormModel<Inspection_Form_Factory_Part_II_FactoryDetail>> GetIncpectionFormFactoryPartIIFactoryDetail(long id)
        {
            GenericFormModel<Inspection_Form_Factory_Part_II_FactoryDetail> genericFormModel = new GenericFormModel<Inspection_Form_Factory_Part_II_FactoryDetail>();
            try
            {
                genericFormModel.ListTemplateLists = new List<ListTemplate>();
                if (id != 0) //Existing Record
                {
                    var parentWithChildObject = await _iGR_Inspection_Form_Factory_Part_II_FactoryDetail
                       .GetAsync(x => x.InspectionRefId == id,  //Conditions         
                         null,                //Orders
                         x => x.Inspection_Master)      //Includes
                       .ConfigureAwait(false);
                    genericFormModel.FormModel = parentWithChildObject.FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                genericFormModel.HasError = true;
                genericFormModel.ErrorDesc = ex.Message;
            }
            return genericFormModel;
        }

        public async Task<GenericFormModel<Inspection_Form_Factory_Part_III_InspectionReport>> GetIncpectionFormFactoryPartIIIInspectionReport(long id)
        {
            GenericFormModel<Inspection_Form_Factory_Part_III_InspectionReport> genericFormModel = new GenericFormModel<Inspection_Form_Factory_Part_III_InspectionReport>();
            try
            {
                genericFormModel.ListTemplateLists = new List<ListTemplate>();
                if (id != 0) //Existing Record
                {
                    var parentWithChildObject = await _iGR_Inspection_Form_Factory_Part_III_InspectionReport
                       .GetAsync(x => x.InspectionRefId == id,  //Conditions         
                         null,                //Orders
                         x => x.Inspection_Master)      //Includes
                       .ConfigureAwait(false);
                    genericFormModel.FormModel = parentWithChildObject.FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                genericFormModel.HasError = true;
                genericFormModel.ErrorDesc = ex.Message;
            }
            return genericFormModel;
        }

        public async Task<GenericFormModel<Inspection_Form_Factory_Part_III_DangerousOperation>> GetForm_Factory_Part_III_Dangerousoperation(long id)
        {
            GenericFormModel<Inspection_Form_Factory_Part_III_DangerousOperation> genericFormModel = new GenericFormModel<Inspection_Form_Factory_Part_III_DangerousOperation>();
            try
            {
                genericFormModel.ListTemplateLists = new List<ListTemplate>();
                if (id != 0) //Existing Record
                {
                    var parentWithChildObject = await _iGR_Inspection_Form_Factory_Part_III_DangerousOperation
                       .GetAsync(x => x.InspectionRefId == id,  //Conditions         
                         null,                //Orders
                         x => x.Inspection_Master)      //Includes
                       .ConfigureAwait(false);
                    genericFormModel.FormModel = parentWithChildObject.FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                genericFormModel.HasError = true;
                genericFormModel.ErrorDesc = ex.Message;
            }
            return genericFormModel;
        }

        public async Task<GenericResponseTemplateModel<List<Inspection_Form_Factory_Part_III_MusterRoll>>> GetIncpectionFormFactoryPartIIIMusterRoll(long id)
        {
            GenericResponseTemplateModel<List<Inspection_Form_Factory_Part_III_MusterRoll>> genericServiceResultTemplate = new GenericResponseTemplateModel<List<Inspection_Form_Factory_Part_III_MusterRoll>>();
            try
            {

                if (id != 0) //Existing Record
                {
                    var parentWithChildObject = await _iGR_Inspection_Form_Factory_Part_III_MusterRoll
                       .GetAsync(x => x.InspectionRefId == id,  //Conditions         
                         null,                //Orders
                         x => x.Inspection_Master)      //Includes
                       .ConfigureAwait(false);
                    genericServiceResultTemplate.ResponseDataModel = parentWithChildObject.ToList();
                }
            }
            catch (Exception ex)
            {
                genericServiceResultTemplate.HasError = true;
                genericServiceResultTemplate.ErrorDesc = ex.Message;
            }
            return genericServiceResultTemplate;
        }

        public async Task<GenericFormModel<Inspection_Form_Factory_Part_III_Health>> GetIncpectionFormFactoryPartIIIHealth(long id)
        {
            GenericFormModel<Inspection_Form_Factory_Part_III_Health> genericFormModel = new GenericFormModel<Inspection_Form_Factory_Part_III_Health>();
            try
            {
                genericFormModel.ListTemplateLists = new List<ListTemplate>();
                if (id != 0) //Existing Record
                {
                    var parentWithChildObject = await _iGR_Inspection_Form_Factory_Part_III_Health
                       .GetAsync(x => x.InspectionRefId == id,  //Conditions         
                         null,                //Orders
                         x => x.Inspection_Master)      //Includes
                       .ConfigureAwait(false);
                    genericFormModel.FormModel = parentWithChildObject.FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                genericFormModel.HasError = true;
                genericFormModel.ErrorDesc = ex.Message;
            }
            return genericFormModel;
        }

        public async Task<GenericFormModel<Inspection_Form_Factory_Part_III_Safety>> GetIncpectionFormFactoryPartIIISafety(long id)
        {
            GenericFormModel<Inspection_Form_Factory_Part_III_Safety> genericFormModel = new GenericFormModel<Inspection_Form_Factory_Part_III_Safety>();
            try
            {
                genericFormModel.ListTemplateLists = new List<ListTemplate>();
                if (id != 0) //Existing Record
                {
                    var parentWithChildObject = await _iGR_Inspection_Form_Factory_Part_III_Safety
                       .GetAsync(x => x.InspectionRefId == id,  //Conditions         
                         null,                //Orders
                         x => x.Inspection_Master)      //Includes
                       .ConfigureAwait(false);
                    genericFormModel.FormModel = parentWithChildObject.FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                genericFormModel.HasError = true;
                genericFormModel.ErrorDesc = ex.Message;
            }
            return genericFormModel;
        }
        public async Task<GenericFormModel<Inspection_Form_Factory_Part_III_Welfare>> GetIncpectionGetFormFactoryPartIIIWelfare(long id)
        {
            GenericFormModel<Inspection_Form_Factory_Part_III_Welfare> genericFormModel = new GenericFormModel<Inspection_Form_Factory_Part_III_Welfare>();
            try
            {
                genericFormModel.ListTemplateLists = new List<ListTemplate>();
                if (id != 0) //Existing Record
                {
                    var parentWithChildObject = await _iGR_Inspection_Form_Factory_Part_III_Welfare
                       .GetAsync(x => x.InspectionRefId == id,  //Conditions         
                         null,                //Orders
                         x => x.Inspection_Master)      //Includes
                       .ConfigureAwait(false);
                    genericFormModel.FormModel = parentWithChildObject.FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                genericFormModel.HasError = true;
                genericFormModel.ErrorDesc = ex.Message;
            }
            return genericFormModel;
        }

        public async Task<GenericFormModel<Inspection_Form_Factory_Part_III_General>> GetIncpectionGetFormFactoryPartIIIGeneral(long id)
        {
            GenericFormModel<Inspection_Form_Factory_Part_III_General> genericFormModel = new GenericFormModel<Inspection_Form_Factory_Part_III_General>();
            try
            {
                genericFormModel.ListTemplateLists = new List<ListTemplate>();
                if (id != 0) //Existing Record
                {
                    var parentWithChildObject = await _iGR_Inspection_Form_Factory_Part_III_General
                       .GetAsync(x => x.InspectionRefId == id,  //Conditions         
                         null,                //Orders
                         x => x.Inspection_Master)      //Includes
                       .ConfigureAwait(false);
                    genericFormModel.FormModel = parentWithChildObject.FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                genericFormModel.HasError = true;
                genericFormModel.ErrorDesc = ex.Message;
            }
            return genericFormModel;
        }

        public async Task<GenericFormModel<Inspection_Form_Factory_Part_III_MajorAccidentHazard>> GetIncpectionGetFormFactoryPartIIIMajorAccidentHazard(long id)
        {
            GenericFormModel<Inspection_Form_Factory_Part_III_MajorAccidentHazard> genericFormModel = new GenericFormModel<Inspection_Form_Factory_Part_III_MajorAccidentHazard>();
            try
            {
                genericFormModel.ListTemplateLists = new List<ListTemplate>();
                if (id != 0) //Existing Record
                {
                    var parentWithChildObject = await _iGR_Inspection_Form_Factory_Part_III_MajorAccidentHazard
                       .GetAsync(x => x.InspectionRefId == id,  //Conditions         
                         null,                //Orders
                         x => x.Inspection_Master)      //Includes
                       .ConfigureAwait(false);
                    genericFormModel.FormModel = parentWithChildObject.FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                genericFormModel.HasError = true;
                genericFormModel.ErrorDesc = ex.Message;
            }
            return genericFormModel;
        }

        public async Task<GenericServiceResultTemplate> AddUpdate_IncpectionFormFactoryPartIGeneralDetail(Inspection_Form_Factory_Part_I_General formModel)
        {
            GenericServiceResultTemplate genericServiceResultTemplate = new GenericServiceResultTemplate();
            try
            {
                genericServiceResultTemplate.HasException = false;
                genericServiceResultTemplate.CustomeValidationResult = CustomeValidator<Inspection_Form_Factory_Part_I_General>.ValidateModel_AllProperties(formModel);

                if (formModel.Id == 0)
                {
                    _iGR_Inspection_Form_Factory_Part_I_General.Insert(formModel);
                    await _iGR_Inspection_Form_Factory_Part_I_General.SavechangeAsync();
                }
                else
                {
                    _context.Update<Inspection_Form_Factory_Part_I_General>(formModel);
                    await _iGR_Inspection_Form_Factory_Part_I_General.SavechangeAsync();
                }
            }
            catch (Exception ex)
            {
                genericServiceResultTemplate.HasException = true;
                genericServiceResultTemplate.Exceptions = ex;
            }
            return genericServiceResultTemplate;
        }

        public async Task<GenericServiceResultTemplate> AddUpdate_IncpectionFormFactoryPartIIFactoryDetail(Inspection_Form_Factory_Part_II_FactoryDetail formModel)
        {
            GenericServiceResultTemplate genericServiceResultTemplate = new GenericServiceResultTemplate();
            try
            {
                genericServiceResultTemplate.HasException = false;
                genericServiceResultTemplate.CustomeValidationResult = CustomeValidator<Inspection_Form_Factory_Part_II_FactoryDetail>.ValidateModel_AllProperties(formModel);
                if (formModel.Id == 0)
                {
                    _iGR_Inspection_Form_Factory_Part_II_FactoryDetail.Insert(formModel);
                    await _iGR_Inspection_Form_Factory_Part_II_FactoryDetail.SavechangeAsync();
                }
                else
                {
                    _context.Update<Inspection_Form_Factory_Part_II_FactoryDetail>(formModel);
                    await _iGR_Inspection_Form_Factory_Part_II_FactoryDetail.SavechangeAsync();
                }

            }
            catch (Exception ex)
            {
                genericServiceResultTemplate.HasException = true;
                genericServiceResultTemplate.Exceptions = ex;
            }
            return genericServiceResultTemplate;
        }

        public async Task<GenericServiceResultTemplate> AddUpdate_IncpectionFormFactoryPartIIIInspectionReport(Inspection_Form_Factory_Part_III_InspectionReport formModel)
        {
            GenericServiceResultTemplate genericServiceResultTemplate = new GenericServiceResultTemplate();
            try
            {
                genericServiceResultTemplate.HasException = false;
                genericServiceResultTemplate.CustomeValidationResult = CustomeValidator<Inspection_Form_Factory_Part_III_InspectionReport>.ValidateModel_AllProperties(formModel);
                if (formModel.Id == 0)
                {
                    _iGR_Inspection_Form_Factory_Part_III_InspectionReport.Insert(formModel);
                    await _iGR_Inspection_Form_Factory_Part_III_InspectionReport.SavechangeAsync();
                }
                else
                {
                    _context.Update<Inspection_Form_Factory_Part_III_InspectionReport>(formModel);
                    await _iGR_Inspection_Form_Factory_Part_III_InspectionReport.SavechangeAsync();
                }

            }
            catch (Exception ex)
            {
                genericServiceResultTemplate.HasException = true;
                genericServiceResultTemplate.Exceptions = ex;
            }
            return genericServiceResultTemplate;
        }

        public async Task<GenericServiceResultTemplate> AddUpdate_IncpectionFormFactoryPartIIIMusterRoll(Inspection_Form_Factory_Part_III_MusterRoll formModel)
        {
            GenericServiceResultTemplate genericServiceResultTemplate = new GenericServiceResultTemplate();
            try
            {
                genericServiceResultTemplate.HasException = false;
                genericServiceResultTemplate.CustomeValidationResult = CustomeValidator<Inspection_Form_Factory_Part_III_MusterRoll>.ValidateModel_AllProperties(formModel);

                _iGR_Inspection_Form_Factory_Part_III_MusterRoll.Insert(formModel);
                await _iGR_Inspection_Form_Factory_Part_III_MusterRoll.SavechangeAsync();

            }
            catch (Exception ex)
            {
                genericServiceResultTemplate.HasException = true;
                genericServiceResultTemplate.Exceptions = ex;
            }
            return genericServiceResultTemplate;
        }

        public async Task<GenericServiceResultTemplate> AddUpdate_InspectionFormFactoryPartIIIHealth(Inspection_Form_Factory_Part_III_Health formModel)
        {
            GenericServiceResultTemplate genericServiceResultTemplate = new GenericServiceResultTemplate();
            genericServiceResultTemplate.HasException = false;
            genericServiceResultTemplate.CustomeValidationResult = CustomeValidator<Inspection_Form_Factory_Part_III_Health>.ValidateModel_AllProperties(formModel);

            try
            {
                if (formModel.Id == 0)
                {
                    _iGR_Inspection_Form_Factory_Part_III_Health.Insert(formModel);
                    await _iGR_Inspection_Form_Factory_Part_III_Health.SavechangeAsync();
                }
                else
                {
                    _context.Update<Inspection_Form_Factory_Part_III_Health>(formModel);
                    await _iGR_Inspection_Form_Factory_Part_III_Health.SavechangeAsync();
                }
            }
            catch (Exception ex)
            {
                genericServiceResultTemplate.HasException = true;
                genericServiceResultTemplate.Exceptions = ex;
            }
            return genericServiceResultTemplate;
        }

        public async Task<GenericServiceResultTemplate> AddUpdate_InspectionFormFactoryPartIIISafety(Inspection_Form_Factory_Part_III_Safety formModel)
        {
            GenericServiceResultTemplate genericServiceResultTemplate = new GenericServiceResultTemplate();
            genericServiceResultTemplate.HasException = false;
            genericServiceResultTemplate.CustomeValidationResult = CustomeValidator<Inspection_Form_Factory_Part_III_Safety>.ValidateModel_AllProperties(formModel);

            try
            {
                if (formModel.Id == 0)
                {
                    _iGR_Inspection_Form_Factory_Part_III_Safety.Insert(formModel);
                    await _iGR_Inspection_Form_Factory_Part_III_Safety.SavechangeAsync();
                }
                else
                {
                    _context.Update<Inspection_Form_Factory_Part_III_Safety>(formModel);
                    await _iGR_Inspection_Form_Factory_Part_III_Safety.SavechangeAsync();
                }

            }
            catch (Exception ex)
            {
                genericServiceResultTemplate.HasException = true;
                genericServiceResultTemplate.Exceptions = ex;
            }
            return genericServiceResultTemplate;
        }

        public async Task<GenericServiceResultTemplate> AddUpdate_IncpectionFormFactoryPartIIIWelfare(Inspection_Form_Factory_Part_III_Welfare formModel)
        {
            GenericServiceResultTemplate genericServiceResultTemplate = new GenericServiceResultTemplate();
            genericServiceResultTemplate.HasException = false;
            genericServiceResultTemplate.CustomeValidationResult = CustomeValidator<Inspection_Form_Factory_Part_III_Welfare>.ValidateModel_AllProperties(formModel);

            try
            {
                if (formModel.Id == 0)
                {
                    _iGR_Inspection_Form_Factory_Part_III_Welfare.Insert(formModel);
                    await _iGR_Inspection_Form_Factory_Part_III_Welfare.SavechangeAsync();
                }
                else
                {
                    _context.Update<Inspection_Form_Factory_Part_III_Welfare>(formModel);
                    await _iGR_Inspection_Form_Factory_Part_III_Welfare.SavechangeAsync();
                }
            }
            catch (Exception ex)
            {
                genericServiceResultTemplate.HasException = true;
                genericServiceResultTemplate.Exceptions = ex;
            }
            return genericServiceResultTemplate;
        }

        public async Task<GenericServiceResultTemplate> AddUpdate_IncpectionFormFactoryPartIIIGeneral(Inspection_Form_Factory_Part_III_General formModel)
        {
            GenericServiceResultTemplate genericServiceResultTemplate = new GenericServiceResultTemplate();
            genericServiceResultTemplate.HasException = false;
            genericServiceResultTemplate.CustomeValidationResult = CustomeValidator<Inspection_Form_Factory_Part_III_General>.ValidateModel_AllProperties(formModel);

            try
            {
                if (formModel.Id == 0)
                {
                    _iGR_Inspection_Form_Factory_Part_III_General.Insert(formModel);
                    await _iGR_Inspection_Form_Factory_Part_III_General.SavechangeAsync();
                }
                else
                {
                    _context.Update<Inspection_Form_Factory_Part_III_General>(formModel);
                    await _iGR_Inspection_Form_Factory_Part_III_General.SavechangeAsync();
                }

            }
            catch (Exception ex)
            {
                genericServiceResultTemplate.HasException = true;
                genericServiceResultTemplate.Exceptions = ex;
            }
            return genericServiceResultTemplate;
        }

        public async Task<GenericServiceResultTemplate> AddUpdate_IncpectionFormFactoryPartIIIMajorAccidentHazard(Inspection_Form_Factory_Part_III_MajorAccidentHazard formModel)
        {
            GenericServiceResultTemplate genericServiceResultTemplate = new GenericServiceResultTemplate();
            genericServiceResultTemplate.HasException = false;
            genericServiceResultTemplate.CustomeValidationResult = CustomeValidator<Inspection_Form_Factory_Part_III_MajorAccidentHazard>.ValidateModel_AllProperties(formModel);

            try
            {
                if (formModel.Id == 0)
                {
                    _iGR_Inspection_Form_Factory_Part_III_MajorAccidentHazard.Insert(formModel);
                    await _iGR_Inspection_Form_Factory_Part_III_MajorAccidentHazard.SavechangeAsync();
                }
                else
                {
                    _context.Update<Inspection_Form_Factory_Part_III_MajorAccidentHazard>(formModel);
                    await _iGR_Inspection_Form_Factory_Part_III_MajorAccidentHazard.SavechangeAsync();
                }

            }
            catch (Exception ex)
            {
                genericServiceResultTemplate.HasException = true;
                genericServiceResultTemplate.Exceptions = ex;
            }
            return genericServiceResultTemplate;
        }

        public async Task<GenericResponseTemplateModel<bool>> RemoveFormLabourPartIIIMusterRollByMusterId(Int64 id)
        {
            GenericResponseTemplateModel<bool> genericFormModel = new GenericResponseTemplateModel<bool>();
            try
            {
                _iGR_Inspection_Form_Labour_Part_III_MusterRoll.Delete(id)
;
                _context.SaveChanges();
            }
            catch (Exception ex)
            {
                genericFormModel.HasError = true;
                genericFormModel.ErrorDesc = ex.Message;
            }
            return genericFormModel;
        }
        public async Task<GenericServiceResultTemplate> AddUpdateForm_Factory_Part_III_Dangerousoperation(Inspection_Form_Factory_Part_III_DangerousOperation formModel)
        {
            GenericServiceResultTemplate genericServiceResultTemplate = new GenericServiceResultTemplate();
            genericServiceResultTemplate.HasException = false;
            genericServiceResultTemplate.CustomeValidationResult = CustomeValidator<Inspection_Form_Factory_Part_III_DangerousOperation>.ValidateModel_AllProperties(formModel);

            try
            {
                if (formModel.Id == 0)
                {
                    _iGR_Inspection_Form_Factory_Part_III_DangerousOperation.Insert(formModel);
                    await _iGR_Inspection_Form_Factory_Part_III_DangerousOperation.SavechangeAsync();
                }
                else
                {
                    _context.Update<Inspection_Form_Factory_Part_III_DangerousOperation>(formModel);
                    await _iGR_Inspection_Form_Factory_Part_III_DangerousOperation.SavechangeAsync();
                }

            }
            catch (Exception ex)
            {
                genericServiceResultTemplate.HasException = true;
                genericServiceResultTemplate.Exceptions = ex;
            }
            return genericServiceResultTemplate;
        }

        public async Task<GenericResponseTemplateModel<bool>> Update_InspectionMasterUsingInpectionId(Int64 inspectionId, Boolean hasAssignedLabourCircle, Int64 labourCircleRefId, Int64 labourWingProfileRefId)
        {
            GenericResponseTemplateModel<bool> genericFormModel = new GenericResponseTemplateModel<bool>();
            try
            {
                if (inspectionId != 0)
                {
                    var inspectionMasters = await _iGR_Inspection_Master
                      .GetAsync(x => x.InspectionId == inspectionId,  //Conditions         
                        null)      //Includes
                      .ConfigureAwait(false);

                    if (inspectionMasters != null)
                    {
                        var inspMaster = inspectionMasters.FirstOrDefault();
                        inspMaster.HasAssignedLabourCircle = hasAssignedLabourCircle;
                        inspMaster.LabourCircleRefId = labourCircleRefId;
                        inspMaster.LabourWingProfileRefId = labourWingProfileRefId;

                        _context.Update<Inspection_Master>(inspMaster);
                        await _iGR_Inspection_Master.SavechangeAsync();
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

        public async Task<GenericResponseTemplateModel<bool>> Transfer_Inspection_MasterUsingInspectionId(Int64 inspectionId, Boolean hasAssignedLabourCircle, Int64 circleRefId, string senderUserId, string senderRoleId, string senderrRoleName, string receiverUserId, string receiverRoleId, Int64 receiverProfileId, string remarks, int inspectionType)
        {
            GenericResponseTemplateModel<bool> genericFormModel = new GenericResponseTemplateModel<bool>();
            try
            {
                if (inspectionId != 0)
                {
                    var inspectionMasters = await _iGR_Inspection_Master
                      .GetAsync(x => x.InspectionId == inspectionId,  //Conditions         
                        null)      //Includes
                      .ConfigureAwait(false);

                    Int64 senderCircleRefId = 0;

                    if (inspectionMasters != null)
                    {
                        var inspMaster = inspectionMasters.FirstOrDefault();
                        inspMaster.HasAssignedLabourCircle = hasAssignedLabourCircle;

                        if (senderrRoleName == "LBIN")
                        {
                            senderCircleRefId = _context.UserCircleMappings.Where(x => x.UserRefId == senderUserId && x.Version ==2).Select(x => x.LabourCircleRefId.Value).FirstOrDefault(); 
                            inspMaster.LabourCircleRefId = circleRefId;
                            inspMaster.HasAssignedLabourCircle = true;
                        }
                        else if (senderrRoleName == "ALLC")
                        {
                            inspMaster.ALCCircleRefId = circleRefId;
                            inspMaster.ALCUserRefId = receiverUserId;
                            inspMaster.ALCProfileRefId = receiverProfileId;
                        }
                        else if (senderrRoleName == "ADRF" || senderrRoleName == "DDRF")
                        {
                            senderCircleRefId = _context.UserCircleMappings.Where(x => x.UserRefId == senderUserId && x.Version == 2).Select(x => x.FactoryCircleRefId.Value).FirstOrDefault();
                            inspMaster.FactoryCircleId = circleRefId;
                            inspMaster.UserId = receiverUserId;
                            inspMaster.UserProfileId = receiverProfileId;
                        }

                        _context.Update<Inspection_Master>(inspMaster);
                        await _iGR_Inspection_Master.SavechangeAsync();
                    }

                    var senderProfileId =  _context.UserProfileMapping.Where(x => x.UserRefId == senderUserId && x.IsActive == true).Select(x => x.UserProfileRefId).FirstOrDefault();

                    InspectionTransferLogs inspectionTransfer = new InspectionTransferLogs();
                    {
                        inspectionTransfer.InspectionRefId = inspectionId;
                        inspectionTransfer.SenderUserRefId = senderUserId;
                        inspectionTransfer.SenderRoleId = senderRoleId;
                        inspectionTransfer.SenderProfileId = senderProfileId;
                        inspectionTransfer.ReceiverUserRefId = receiverUserId;
                        inspectionTransfer.ReceiverRoleId = receiverRoleId;
                        inspectionTransfer.ReceiverProfileId = receiverProfileId;
                        inspectionTransfer.LastModifiedOn = DateTime.Now;
                        inspectionTransfer.SenderCircleRefId = senderCircleRefId;
                        inspectionTransfer.ReceiverCircleRefId = circleRefId;
                        inspectionTransfer.Remarks = remarks;
                        inspectionTransfer.InspectionType = inspectionType;
                    }

                    await _context.InspectionTransferLogs.AddAsync(inspectionTransfer);
                    await _context.SaveChangesAsync();
                }
            }

            catch (Exception ex)
            {
                genericFormModel.HasError = true;
                genericFormModel.ErrorDesc = ex.Message;
            }
            return genericFormModel;
        }

        public async Task<GenericServiceResultTemplate> Lock_InspectionInfo(Inspection_LockInfo formModel)
        {
            GenericServiceResultTemplate genericServiceResultTemplate = new GenericServiceResultTemplate();
            genericServiceResultTemplate.HasException = false;
            genericServiceResultTemplate.CustomeValidationResult = CustomeValidator<Inspection_LockInfo>.ValidateModel_AllProperties(formModel);

            try
            {
                var userDetails = await _iAuthService.GetUserRoleAndProfileDetailByUserId(formModel.LockedBy_UserId);
                var res = await _iGR_Inspection_LockInfo
                      .GetAsync(x => x.RandomizationRefId == formModel.RandomizationRefId && x.LockedBy_UserId == formModel.LockedBy_UserId,   //Conditions         
                        null)      //Includes
                      .ConfigureAwait(false);
                if (res.Count() == 0) // Existing record
                {
                    formModel.LockedOn = DateTime.Now;
                    formModel.LockedBy_RoleId = userDetails.RoleId;
                    _iGR_Inspection_LockInfo.Insert(formModel);
                    await _iGR_Inspection_LockInfo.SavechangeAsync();
                }

                await _iNotificationManagerService.SendInspectionNotificationToIndustryUserByFactoryCircleId(formModel.RandomizationRefId, formModel.FactoryCircleRefId);
            }
            catch (Exception ex)
            {
                genericServiceResultTemplate.HasException = true;
                genericServiceResultTemplate.Exceptions = ex;
            }
            return genericServiceResultTemplate;
        }

        public async Task<GenericResponseTemplateModel<bool>> RemoveFormFactoryPartIIIMusterRollByMusterId(Int64 id)
        {
            GenericResponseTemplateModel<bool> genericFormModel = new GenericResponseTemplateModel<bool>();
            try
            {
                _iGR_Inspection_Form_Factory_Part_III_MusterRoll.Delete(id);
                _context.SaveChanges();
            }
            catch (Exception ex)
            {
                genericFormModel.HasError = true;
                genericFormModel.ErrorDesc = ex.Message;
            }
            return genericFormModel;
        }

        public async Task<GenericResponseTemplateModel<List<GetInspections_FactoryPerformaStepStatusViewModel>>> Get_Inspections_FactoryPerformaStepStatus(Int64 inspectionRefId)
        {
            GenericResponseTemplateModel<List<GetInspections_FactoryPerformaStepStatusViewModel>> genericFormModel = new GenericResponseTemplateModel<List<GetInspections_FactoryPerformaStepStatusViewModel>>();
            try
            {
                List<StoreProcedureParm> storeProcedureParms = new List<StoreProcedureParm>()
                {
                    new StoreProcedureParm (){ ParmName="InspectionRefId", ParmValue=inspectionRefId.ToString(),isNumber=true}
                };
                genericFormModel.ResponseDataModel = await _iGeneric_SP_Repository.CallStoreProcedureReaderAsync<GetInspections_FactoryPerformaStepStatusViewModel>("sp_Inspections_FactoryPerformaStepStatus", storeProcedureParms);

            }
            catch (Exception ex)
            {
                genericFormModel.HasError = true;
                genericFormModel.ErrorDesc = ex.Message;
            }
            return genericFormModel;
        }


        public async Task<GenericFormModel<List<Inspection_MasterDataByRandomizationIdWithStabblishmentNameAddressViewModel>>> GetInspection_MasterDataByRandomizationIdWithStabblishmentNameAddress(Int64 id, string userId)
        {
            GenericFormModel<List<Inspection_MasterDataByRandomizationIdWithStabblishmentNameAddressViewModel>> genericFormModel = new GenericFormModel<List<Inspection_MasterDataByRandomizationIdWithStabblishmentNameAddressViewModel>>();
            try
            {
                List<StoreProcedureParm> storeProcedureParms = new List<StoreProcedureParm>()
                {
                    new StoreProcedureParm (){ isNumber=true,ParmName="RandomizationId", ParmValue=id.ToString()},
                    new StoreProcedureParm (){ isNumber=false,ParmName="UserId", ParmValue=userId.ToString()}
                };
                genericFormModel.FormModel = await _iGeneric_SP_Repository.CallStoreProcedureReaderAsync<Inspection_MasterDataByRandomizationIdWithStabblishmentNameAddressViewModel>("sp_GetInspection_MasterDataByRandomizationIdWithStabblishmentNameAddress", storeProcedureParms);

            }
            catch (Exception ex)
            {
                genericFormModel.HasError = true;
                genericFormModel.ErrorDesc = ex.Message;
            }

            return genericFormModel;
            
        }
        #endregion General Details

        #region Labour Inspections

        public async Task<GenericFormModel<Inspection_Form_Labour_Part_I_General>> GetInspectionFormLabourPartIGeneralDetail(long id)
        {
            GenericFormModel<Inspection_Form_Labour_Part_I_General> genericFormModel = new GenericFormModel<Inspection_Form_Labour_Part_I_General>();
            try
            {
                genericFormModel.ListTemplateLists = new List<ListTemplate>();
                if (id != 0) //Existing Record
                {
                    var parentWithChildObject = await _iGR_Inspection_Form_Labour_Part_I_General
                       .GetAsync(x => x.InspectionRefId == id,  //Conditions         
                         null,                //Orders
                         x => x.Inspection_Master)      //Includes
                       .ConfigureAwait(false);
                    genericFormModel.FormModel = parentWithChildObject.FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                genericFormModel.HasError = true;
                genericFormModel.ErrorDesc = ex.Message;
            }
            return genericFormModel;
        }
        public async Task<GenericServiceResultTemplate> AddUpdate_InspectionFormLabourPartIGeneralDetail(Inspection_Form_Labour_Part_I_General formModel)
        {
            GenericServiceResultTemplate genericServiceResultTemplate = new GenericServiceResultTemplate();
            try
            {
                genericServiceResultTemplate.HasException = false;
                genericServiceResultTemplate.CustomeValidationResult = CustomeValidator<Inspection_Form_Labour_Part_I_General>.ValidateModel_AllProperties(formModel);

                if (formModel.Id == 0)
                {
                    _iGR_Inspection_Form_Labour_Part_I_General.Insert(formModel);
                    await _iGR_Inspection_Form_Labour_Part_I_General.SavechangeAsync();
                }
                else
                {
                    _context.Update<Inspection_Form_Labour_Part_I_General>(formModel);
                    await _iGR_Inspection_Form_Labour_Part_I_General.SavechangeAsync();
                }

                //var inspectionMasters = await _iGR_Inspection_Master
                //     .GetAsync(x => x.InspectionId == formModel.InspectionRefId,  //Conditions         
                //       null)      //Includes
                //     .ConfigureAwait(false);

                //if (inspectionMasters.Count() > 0)
                //{
                //    var inspMaster = inspectionMasters.FirstOrDefault();
                //    //inspMaster.HasProcessed = true;
                //    inspMaster.InspectionDoneOn_Labour_Wing = formModel.DateOfInspection;
                //    _context.Update<Inspection_Master>(inspMaster);
                //    await _iGR_Inspection_Master.SavechangeAsync();
                //}

            }
            catch (Exception ex)
            {
                genericServiceResultTemplate.HasException = true;
                genericServiceResultTemplate.Exceptions = ex;
            }
            return genericServiceResultTemplate;
        }
        public async Task<GenericFormModel<Inspection_Form_Labour_Part_II_FactoryDetail>> GetInspectionFormLabourPartIIFactoryDetail(long id)
        {
            GenericFormModel<Inspection_Form_Labour_Part_II_FactoryDetail> genericFormModel = new GenericFormModel<Inspection_Form_Labour_Part_II_FactoryDetail>();
            try
            {
                genericFormModel.ListTemplateLists = new List<ListTemplate>();
                if (id != 0) //Existing Record
                {
                    var parentWithChildObject = await _iGR_Inspection_Form_Labour_Part_II_FactoryDetail
                       .GetAsync(x => x.InspectionRefId == id,  //Conditions         
                         null,                //Orders
                         x => x.Inspection_Master)      //Includes
                       .ConfigureAwait(false);
                    genericFormModel.FormModel = parentWithChildObject.FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                genericFormModel.HasError = true;
                genericFormModel.ErrorDesc = ex.Message;
            }
            return genericFormModel;
        }
        public async Task<GenericServiceResultTemplate> AddUpdate_InspectionFormLabourPartIIFactoryDetail(Inspection_Form_Labour_Part_II_FactoryDetail formModel)
        {
            GenericServiceResultTemplate genericServiceResultTemplate = new GenericServiceResultTemplate();
            try
            {
                genericServiceResultTemplate.HasException = false;
                genericServiceResultTemplate.CustomeValidationResult = CustomeValidator<Inspection_Form_Labour_Part_II_FactoryDetail>.ValidateModel_AllProperties(formModel);

                if (formModel.Id == 0)
                {
                    _iGR_Inspection_Form_Labour_Part_II_FactoryDetail.Insert(formModel);
                    await _iGR_Inspection_Form_Labour_Part_II_FactoryDetail.SavechangeAsync();
                }
                else
                {
                    _context.Update<Inspection_Form_Labour_Part_II_FactoryDetail>(formModel);
                    await _iGR_Inspection_Form_Labour_Part_II_FactoryDetail.SavechangeAsync();
                }
            }
            catch (Exception ex)
            {
                genericServiceResultTemplate.HasException = true;
                genericServiceResultTemplate.Exceptions = ex;
            }
            return genericServiceResultTemplate;
        }
        public async Task<GenericResponseTemplateModel<List<Inspection_Form_Labour_Part_III_MusterRoll>>> GetInspectionFormLabourPartIIIMusterRoll(long id)
        {
            GenericResponseTemplateModel<List<Inspection_Form_Labour_Part_III_MusterRoll>> genericServiceResultTemplate = new GenericResponseTemplateModel<List<Inspection_Form_Labour_Part_III_MusterRoll>>();
            try
            {

                if (id != 0) //Existing Record
                {
                    var parentWithChildObject = await _iGR_Inspection_Form_Labour_Part_III_MusterRoll
                       .GetAsync(x => x.InspectionRefId == id,  //Conditions         
                         null,                //Orders
                         x => x.Inspection_Master)      //Includes
                       .ConfigureAwait(false);
                    genericServiceResultTemplate.ResponseDataModel = parentWithChildObject.ToList();
                }
            }
            catch (Exception ex)
            {
                genericServiceResultTemplate.HasError = true;
                genericServiceResultTemplate.ErrorDesc = ex.Message;
            }
            return genericServiceResultTemplate;
        }
        public async Task<GenericServiceResultTemplate> AddUpdate_InspectionFormLabourPartIIIMusterRoll(Inspection_Form_Labour_Part_III_MusterRoll formModel)
        {
            GenericServiceResultTemplate genericServiceResultTemplate = new GenericServiceResultTemplate();
            try
            {
                genericServiceResultTemplate.HasException = false;
                genericServiceResultTemplate.CustomeValidationResult = CustomeValidator<Inspection_Form_Labour_Part_III_MusterRoll>.ValidateModel_AllProperties(formModel);

                var res = await _iGR_Inspection_Form_Labour_Part_III_MusterRoll
                       .GetAsync(x => x.InspectionRefId == formModel.InspectionRefId,  //Conditions         
                         null,                //Orders
                         x => x.Inspection_Master)      //Includes
                       .ConfigureAwait(false);
                if (formModel.InspectionRefId != 0 && res.Count() > 0) // Existing record
                {
                    _context.Update<Inspection_Form_Labour_Part_III_MusterRoll>(formModel);
                    await _iGR_Inspection_Form_Labour_Part_III_MusterRoll.SavechangeAsync();
                }
                else
                {
                    _iGR_Inspection_Form_Labour_Part_III_MusterRoll.Insert(formModel);
                    await _iGR_Inspection_Form_Labour_Part_III_MusterRoll.SavechangeAsync();
                }
            }
            catch (Exception ex)
            {
                genericServiceResultTemplate.HasException = true;
                genericServiceResultTemplate.Exceptions = ex;
            }
            return genericServiceResultTemplate;
        }
        public async Task<GenericFormModel<Inspection_Form_Labour_Part_III_EqualEnumerationAct>> GetInspectionFormLabourPartIIIEqualEnumerationAct(Int64 id)
        {
            GenericFormModel<Inspection_Form_Labour_Part_III_EqualEnumerationAct> genericServiceResultTemplate = new GenericFormModel<Inspection_Form_Labour_Part_III_EqualEnumerationAct>();
            try
            {

                if (id != 0) //Existing Record
                {
                    var parentWithChildObject = await _iGR_Inspection_Form_Labour_Part_III_EqualEnumerationAct
                       .GetAsync(x => x.InspectionRefId == id,  //Conditions         
                         null,                //Orders
                         x => x.Inspection_Master)      //Includes
                       .ConfigureAwait(false);
                    genericServiceResultTemplate.FormModel = parentWithChildObject.FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                genericServiceResultTemplate.HasError = true;
                genericServiceResultTemplate.ErrorDesc = ex.Message;
            }
            return genericServiceResultTemplate;
        }
        public async Task<GenericServiceResultTemplate> AddUpdate_InspectionFormLabourPartIIIEqualEnumerationAct(Inspection_Form_Labour_Part_III_EqualEnumerationAct formModel)
        {
            GenericServiceResultTemplate genericServiceResultTemplate = new GenericServiceResultTemplate();
            try
            {
                genericServiceResultTemplate.HasException = false;
                genericServiceResultTemplate.CustomeValidationResult = CustomeValidator<Inspection_Form_Labour_Part_III_EqualEnumerationAct>.ValidateModel_AllProperties(formModel);
                if (formModel.Id == 0)
                {
                    _iGR_Inspection_Form_Labour_Part_III_EqualEnumerationAct.Insert(formModel);
                    await _iGR_Inspection_Form_Labour_Part_III_EqualEnumerationAct.SavechangeAsync();
                }
                else
                {
                    _context.Update<Inspection_Form_Labour_Part_III_EqualEnumerationAct>(formModel);
                    await _iGR_Inspection_Form_Labour_Part_III_EqualEnumerationAct.SavechangeAsync();
                }
            }
            catch (Exception ex)
            {
                genericServiceResultTemplate.HasException = true;
                genericServiceResultTemplate.Exceptions = ex;
            }
            return genericServiceResultTemplate;
        }
        public async Task<GenericFormModel<Inspection_Form_Labour_III_MinimumWageAct>> GetInspectionFormLabourPartIIIMinimumWagesAct(Int64 id)
        {
            GenericFormModel<Inspection_Form_Labour_III_MinimumWageAct> genericServiceResultTemplate = new GenericFormModel<Inspection_Form_Labour_III_MinimumWageAct>();
            try
            {

                if (id != 0) //Existing Record
                {
                    var parentWithChildObject = await _iGR_Inspection_Form_Labour_III_MinimumWageAct
                       .GetAsync(x => x.InspectionRefId == id,  //Conditions         
                         null,                //Orders
                         x => x.Inspection_Master)      //Includes
                       .ConfigureAwait(false);
                    genericServiceResultTemplate.FormModel = parentWithChildObject.FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                genericServiceResultTemplate.HasError = true;
                genericServiceResultTemplate.ErrorDesc = ex.Message;
            }
            return genericServiceResultTemplate;
        }
        public async Task<GenericServiceResultTemplate> AddUpdate_InspectionFormLabourPartIIIMinimumWagesAct(Inspection_Form_Labour_III_MinimumWageAct formModel)
        {
            GenericServiceResultTemplate genericServiceResultTemplate = new GenericServiceResultTemplate();
            try
            {
                genericServiceResultTemplate.HasException = false;
                genericServiceResultTemplate.CustomeValidationResult = CustomeValidator<Inspection_Form_Labour_III_MinimumWageAct>.ValidateModel_AllProperties(formModel);

                if (formModel.Id == 0)
                {
                    _iGR_Inspection_Form_Labour_III_MinimumWageAct.Insert(formModel);
                    await _iGR_Inspection_Form_Labour_III_MinimumWageAct.SavechangeAsync();
                }
                else
                {
                    _context.Update<Inspection_Form_Labour_III_MinimumWageAct>(formModel);
                    await _iGR_Inspection_Form_Labour_III_MinimumWageAct.SavechangeAsync();
                }
            }
            catch (Exception ex)
            {
                genericServiceResultTemplate.HasException = true;
                genericServiceResultTemplate.Exceptions = ex;
            }
            return genericServiceResultTemplate;
        }
        public async Task<GenericFormModel<Inspection_Form_Labour_III_PaymentWagesAct>> GetInspectionFormLabourPartIIIPaymentWagesAct(Int64 id)
        {
            GenericFormModel<Inspection_Form_Labour_III_PaymentWagesAct> genericServiceResultTemplate = new GenericFormModel<Inspection_Form_Labour_III_PaymentWagesAct>();
            try
            {

                if (id != 0) //Existing Record
                {
                    var parentWithChildObject = await _iGR_Inspection_Form_Labour_III_PaymentWagesAct
                       .GetAsync(x => x.InspectionRefId == id,  //Conditions         
                         null,                //Orders
                         x => x.Inspection_Master)      //Includes
                       .ConfigureAwait(false);
                    genericServiceResultTemplate.FormModel = parentWithChildObject.FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                genericServiceResultTemplate.HasError = true;
                genericServiceResultTemplate.ErrorDesc = ex.Message;
            }
            return genericServiceResultTemplate;
        }
        public async Task<GenericServiceResultTemplate> AddUpdate_InspectionFormLabourPartIIIPaymentWagesAct(Inspection_Form_Labour_III_PaymentWagesAct formModel)
        {
            GenericServiceResultTemplate genericServiceResultTemplate = new GenericServiceResultTemplate();
            try
            {
                genericServiceResultTemplate.HasException = false;
                genericServiceResultTemplate.CustomeValidationResult = CustomeValidator<Inspection_Form_Labour_III_PaymentWagesAct>.ValidateModel_AllProperties(formModel);

                if (formModel.Id == 0)
                {
                    _iGR_Inspection_Form_Labour_III_PaymentWagesAct.Insert(formModel);
                    await _iGR_Inspection_Form_Labour_III_PaymentWagesAct.SavechangeAsync();
                }
                else
                {
                    _context.Update<Inspection_Form_Labour_III_PaymentWagesAct>(formModel);
                    await _iGR_Inspection_Form_Labour_III_PaymentWagesAct.SavechangeAsync();
                }
            }
            catch (Exception ex)
            {
                genericServiceResultTemplate.HasException = true;
                genericServiceResultTemplate.Exceptions = ex;
            }
            return genericServiceResultTemplate;
        }
        public async Task<GenericFormModel<Inspection_Form_Labour_III_PaymentBonusAct_StatutoryReport>> GetInspectionFormLabourPartIIIPaymentBonusAct_StatutoryReport(Int64 id)
        {
            GenericFormModel<Inspection_Form_Labour_III_PaymentBonusAct_StatutoryReport> genericServiceResultTemplate = new GenericFormModel<Inspection_Form_Labour_III_PaymentBonusAct_StatutoryReport>();
            try
            {

                if (id != 0) //Existing Record
                {
                    var parentWithChildObject = await _iGR_Inspection_Form_Labour_III_PaymentBonusAct_StatutoryReport
                       .GetAsync(x => x.InspectionRefId == id,  //Conditions         
                         null,                //Orders
                         x => x.Inspection_Master)      //Includes
                       .ConfigureAwait(false);
                    genericServiceResultTemplate.FormModel = parentWithChildObject.FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                genericServiceResultTemplate.HasError = true;
                genericServiceResultTemplate.ErrorDesc = ex.Message;
            }
            return genericServiceResultTemplate;
        }
        public async Task<GenericServiceResultTemplate> AddUpdate_InspectionFormLabourPartIIIPaymentBonusAct_StatutoryReport(Inspection_Form_Labour_III_PaymentBonusAct_StatutoryReport formModel)
        {
            GenericServiceResultTemplate genericServiceResultTemplate = new GenericServiceResultTemplate();
            try
            {
                genericServiceResultTemplate.HasException = false;
                genericServiceResultTemplate.CustomeValidationResult = CustomeValidator<Inspection_Form_Labour_III_PaymentBonusAct_StatutoryReport>.ValidateModel_AllProperties(formModel);

                if (formModel.Id == 0)
                {
                    _iGR_Inspection_Form_Labour_III_PaymentBonusAct_StatutoryReport.Insert(formModel);
                    await _iGR_Inspection_Form_Labour_III_PaymentBonusAct_StatutoryReport.SavechangeAsync();
                }
                else
                {
                    _context.Update<Inspection_Form_Labour_III_PaymentBonusAct_StatutoryReport>(formModel);
                    await _iGR_Inspection_Form_Labour_III_PaymentBonusAct_StatutoryReport.SavechangeAsync();
                }
            }
            catch (Exception ex)
            {
                genericServiceResultTemplate.HasException = true;
                genericServiceResultTemplate.Exceptions = ex;
            }
            return genericServiceResultTemplate;
        }
        public async Task<GenericFormModel<List<Inspection_Form_Labour_III_PaymentBonusAct_Part_B_Attachment>>> GetInspectionFormLabourPartIIIPaymentBonusAct_Part_B_Attachment(Int64 id)
        {
            GenericFormModel<List<Inspection_Form_Labour_III_PaymentBonusAct_Part_B_Attachment>> genericServiceResultTemplate = new GenericFormModel<List<Inspection_Form_Labour_III_PaymentBonusAct_Part_B_Attachment>>();
            try
            {

                if (id != 0) //Existing Record
                {
                    var parentWithChildObject = await _iGR_Inspection_Form_Labour_III_PaymentBonusAct_Part_B_Attachment
                       .GetAsync(x => x.InspectionRefId == id,  //Conditions         
                         null,                //Orders
                         x => x.Inspection_Master)      //Includes
                       .ConfigureAwait(false);
                    genericServiceResultTemplate.FormModel = parentWithChildObject.ToList();
                }
            }
            catch (Exception ex)
            {
                genericServiceResultTemplate.HasError = true;
                genericServiceResultTemplate.ErrorDesc = ex.Message;
            }
            return genericServiceResultTemplate;
        }
        public async Task<GenericServiceResultTemplate> AddUpdate_InspectionFormLabourPartIIIPaymentBonusAct_Part_B_Attachment(Inspection_Form_Labour_III_PaymentBonusAct_Part_B_Attachment formModel)
        {
            GenericServiceResultTemplate genericServiceResultTemplate = new GenericServiceResultTemplate();
            try
            {
                genericServiceResultTemplate.HasException = false;
                genericServiceResultTemplate.CustomeValidationResult = CustomeValidator<Inspection_Form_Labour_III_PaymentBonusAct_Part_B_Attachment>.ValidateModel_AllProperties(formModel);

                if (formModel.Id == 0)
                {
                    _iGR_Inspection_Form_Labour_III_PaymentBonusAct_Part_B_Attachment.Insert(formModel);
                    await _iGR_Inspection_Form_Labour_III_PaymentBonusAct_Part_B_Attachment.SavechangeAsync();
                }
                else
                {
                    _context.Update<Inspection_Form_Labour_III_PaymentBonusAct_Part_B_Attachment>(formModel);
                    await _iGR_Inspection_Form_Labour_III_PaymentBonusAct_Part_B_Attachment.SavechangeAsync();
                }
            }
            catch (Exception ex)
            {
                genericServiceResultTemplate.HasException = true;
                genericServiceResultTemplate.Exceptions = ex;
            }
            return genericServiceResultTemplate;
        }
        public async Task<GenericFormModel<Inspection_Form_Labour_III_ChildAndAdolescentLabourAct>> GetInspectionFormLabourPartIIIChildAndAdolescentLabourAct(Int64 id)
        {
            GenericFormModel<Inspection_Form_Labour_III_ChildAndAdolescentLabourAct> genericServiceResultTemplate = new GenericFormModel<Inspection_Form_Labour_III_ChildAndAdolescentLabourAct>();
            try
            {

                if (id != 0) //Existing Record
                {
                    var parentWithChildObject = await _iGR_Inspection_Form_Labour_III_ChildAndAdolescentLabourAct
                       .GetAsync(x => x.InspectionRefId == id,  //Conditions         
                         null,                //Orders
                         x => x.Inspection_Master)      //Includes
                       .ConfigureAwait(false);
                    genericServiceResultTemplate.FormModel = parentWithChildObject.FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                genericServiceResultTemplate.HasError = true;
                genericServiceResultTemplate.ErrorDesc = ex.Message;
            }
            return genericServiceResultTemplate;
        }
        public async Task<GenericServiceResultTemplate> AddUpdate_InspectionFormLabourPartIIIChildAndAdolescentLabourAct(Inspection_Form_Labour_III_ChildAndAdolescentLabourAct formModel)
        {
            GenericServiceResultTemplate genericServiceResultTemplate = new GenericServiceResultTemplate();
            try
            {
                genericServiceResultTemplate.HasException = false;
                genericServiceResultTemplate.CustomeValidationResult = CustomeValidator<Inspection_Form_Labour_III_ChildAndAdolescentLabourAct>.ValidateModel_AllProperties(formModel);

                if (formModel.Id == 0)
                {
                    _iGR_Inspection_Form_Labour_III_ChildAndAdolescentLabourAct.Insert(formModel);
                    await _iGR_Inspection_Form_Labour_III_ChildAndAdolescentLabourAct.SavechangeAsync();
                }
                else
                {
                    _context.Update<Inspection_Form_Labour_III_ChildAndAdolescentLabourAct>(formModel);
                    await _iGR_Inspection_Form_Labour_III_ChildAndAdolescentLabourAct.SavechangeAsync();
                }
            }
            catch (Exception ex)
            {
                genericServiceResultTemplate.HasException = true;
                genericServiceResultTemplate.Exceptions = ex;
            }
            return genericServiceResultTemplate;
        }
        public async Task<GenericFormModel<Inspection_Form_Labour_III_NationalAndFestivalHolidays>> GetInspectionFormLabourPartIIINationalAndFestivalHolidays(Int64 id)
        {
            GenericFormModel<Inspection_Form_Labour_III_NationalAndFestivalHolidays> genericServiceResultTemplate = new GenericFormModel<Inspection_Form_Labour_III_NationalAndFestivalHolidays>();
            try
            {

                if (id != 0) //Existing Record
                {
                    var parentWithChildObject = await _iGR_Inspection_Form_Labour_III_NationalAndFestivalHolidays
                       .GetAsync(x => x.InspectionRefId == id,  //Conditions         
                         null,                //Orders
                         x => x.Inspection_Master)      //Includes
                       .ConfigureAwait(false);
                    genericServiceResultTemplate.FormModel = parentWithChildObject.FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                genericServiceResultTemplate.HasError = true;
                genericServiceResultTemplate.ErrorDesc = ex.Message;
            }
            return genericServiceResultTemplate;
        }
        public async Task<GenericServiceResultTemplate> AddUpdate_InspectionFormLabourPartIIINationalAndFestivalHolidays(Inspection_Form_Labour_III_NationalAndFestivalHolidays formModel)
        {
            GenericServiceResultTemplate genericServiceResultTemplate = new GenericServiceResultTemplate();
            try
            {
                genericServiceResultTemplate.HasException = false;
                genericServiceResultTemplate.CustomeValidationResult = CustomeValidator<Inspection_Form_Labour_III_NationalAndFestivalHolidays>.ValidateModel_AllProperties(formModel);

                if (formModel.Id == 0)
                {
                    _iGR_Inspection_Form_Labour_III_NationalAndFestivalHolidays.Insert(formModel);
                    await _iGR_Inspection_Form_Labour_III_NationalAndFestivalHolidays.SavechangeAsync();
                }
                else
                {
                    _context.Update<Inspection_Form_Labour_III_NationalAndFestivalHolidays>(formModel);
                    await _iGR_Inspection_Form_Labour_III_NationalAndFestivalHolidays.SavechangeAsync();
                }
            }
            catch (Exception ex)
            {
                genericServiceResultTemplate.HasException = true;
                genericServiceResultTemplate.Exceptions = ex;
            }
            return genericServiceResultTemplate;
        }
        public async Task<GenericFormModel<Inspection_Form_Labour_III_MaternityBenefitAct>> GetInspectionFormLabourPartIIIMaternityBenefitAct(Int64 id)
        {
            GenericFormModel<Inspection_Form_Labour_III_MaternityBenefitAct> genericServiceResultTemplate = new GenericFormModel<Inspection_Form_Labour_III_MaternityBenefitAct>();
            try
            {

                if (id != 0) //Existing Record
                {
                    var parentWithChildObject = await _iGR_Inspection_Form_Labour_III_MaternityBenefitAct
                       .GetAsync(x => x.InspectionRefId == id,  //Conditions         
                         null,                //Orders
                         x => x.Inspection_Master)      //Includes
                       .ConfigureAwait(false);
                    genericServiceResultTemplate.FormModel = parentWithChildObject.FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                genericServiceResultTemplate.HasError = true;
                genericServiceResultTemplate.ErrorDesc = ex.Message;
            }
            return genericServiceResultTemplate;
        }
        public async Task<GenericServiceResultTemplate> AddUpdate_InspectionFormLabourPartIIIMaternityBenefitAct(Inspection_Form_Labour_III_MaternityBenefitAct formModel)
        {
            GenericServiceResultTemplate genericServiceResultTemplate = new GenericServiceResultTemplate();
            try
            {
                genericServiceResultTemplate.HasException = false;
                genericServiceResultTemplate.CustomeValidationResult = CustomeValidator<Inspection_Form_Labour_III_MaternityBenefitAct>.ValidateModel_AllProperties(formModel);
                if (formModel.Id == 0)
                {
                    _iGR_Inspection_Form_Labour_III_MaternityBenefitAct.Insert(formModel);
                    await _iGR_Inspection_Form_Labour_III_MaternityBenefitAct.SavechangeAsync();
                }
                else
                {
                    _context.Update<Inspection_Form_Labour_III_MaternityBenefitAct>(formModel);
                    await _iGR_Inspection_Form_Labour_III_MaternityBenefitAct.SavechangeAsync();
                }
            }
            catch (Exception ex)
            {
                genericServiceResultTemplate.HasException = true;
                genericServiceResultTemplate.Exceptions = ex;
            }
            return genericServiceResultTemplate;
        }
        public async Task<GenericFormModel<Inspection_Form_Labour_III_ContractLabourAct>> GetInspectionFormLabourPartIIIContractLabourAct(Int64 id)
        {
            GenericFormModel<Inspection_Form_Labour_III_ContractLabourAct> genericServiceResultTemplate = new GenericFormModel<Inspection_Form_Labour_III_ContractLabourAct>();
            try
            {

                if (id != 0) //Existing Record
                {
                    var parentWithChildObject = await _iGR_Inspection_Form_Labour_III_ContractLabourAct
                       .GetAsync(x => x.InspectionRefId == id,  //Conditions         
                         null,                //Orders
                         x => x.Inspection_Master)      //Includes
                       .ConfigureAwait(false);
                    genericServiceResultTemplate.FormModel = parentWithChildObject.FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                genericServiceResultTemplate.HasError = true;
                genericServiceResultTemplate.ErrorDesc = ex.Message;
            }
            return genericServiceResultTemplate;
        }
        public async Task<GenericServiceResultTemplate> AddUpdate_InspectionFormLabourPartIIIContractLabourAct(Inspection_Form_Labour_III_ContractLabourAct formModel)
        {
            GenericServiceResultTemplate genericServiceResultTemplate = new GenericServiceResultTemplate();
            try
            {
                genericServiceResultTemplate.HasException = false;
                genericServiceResultTemplate.CustomeValidationResult = CustomeValidator<Inspection_Form_Labour_III_ContractLabourAct>.ValidateModel_AllProperties(formModel);
                if (formModel.Id == 0)
                {
                    _iGR_Inspection_Form_Labour_III_ContractLabourAct.Insert(formModel);
                    await _iGR_Inspection_Form_Labour_III_ContractLabourAct.SavechangeAsync();
                }
                else
                {
                    _context.Update<Inspection_Form_Labour_III_ContractLabourAct>(formModel);
                    await _iGR_Inspection_Form_Labour_III_ContractLabourAct.SavechangeAsync();
                }
            }
            catch (Exception ex)
            {
                genericServiceResultTemplate.HasException = true;
                genericServiceResultTemplate.Exceptions = ex;
            }
            return genericServiceResultTemplate;
        }
        public async Task<GenericFormModel<Inspection_Form_Labour_III_InterStateMigrantWorkmenAct>> GetInspectionFormLabourPartIIIInterStateMigrantWorkmenAct(Int64 id)
        {
            GenericFormModel<Inspection_Form_Labour_III_InterStateMigrantWorkmenAct> genericServiceResultTemplate = new GenericFormModel<Inspection_Form_Labour_III_InterStateMigrantWorkmenAct>();
            try
            {

                if (id != 0) //Existing Record
                {
                    var parentWithChildObject = await _iGR_Inspection_Form_Labour_III_InterStateMigrantWorkmenAct
                       .GetAsync(x => x.InspectionRefId == id,  //Conditions         
                         null,                //Orders
                         x => x.Inspection_Master)      //Includes
                       .ConfigureAwait(false);
                    genericServiceResultTemplate.FormModel = parentWithChildObject.FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                genericServiceResultTemplate.HasError = true;
                genericServiceResultTemplate.ErrorDesc = ex.Message;
            }
            return genericServiceResultTemplate;
        }
        public async Task<GenericServiceResultTemplate> AddUpdate_InspectionFormLabourPartIIIInterStateMigrantWorkmenAct(Inspection_Form_Labour_III_InterStateMigrantWorkmenAct formModel)
        {
            GenericServiceResultTemplate genericServiceResultTemplate = new GenericServiceResultTemplate();
            try
            {
                genericServiceResultTemplate.HasException = false;
                genericServiceResultTemplate.CustomeValidationResult = CustomeValidator<Inspection_Form_Labour_III_InterStateMigrantWorkmenAct>.ValidateModel_AllProperties(formModel);
                if (formModel.Id == 0)
                {
                    _iGR_Inspection_Form_Labour_III_InterStateMigrantWorkmenAct.Insert(formModel);
                    await _iGR_Inspection_Form_Labour_III_InterStateMigrantWorkmenAct.SavechangeAsync();
                }
                else
                {
                    _context.Update<Inspection_Form_Labour_III_InterStateMigrantWorkmenAct>(formModel);
                    await _iGR_Inspection_Form_Labour_III_InterStateMigrantWorkmenAct.SavechangeAsync();
                }
            }
            catch (Exception ex)
            {
                genericServiceResultTemplate.HasException = true;
                genericServiceResultTemplate.Exceptions = ex;
            }
            return genericServiceResultTemplate;
        }
        public async Task<GenericFormModel<Inspection_Form_Labour_III_LabourWelfareFund_Act>> GetInspectionFormLabourPartIIILabourWelfareFund_Act(Int64 id)
        {
            GenericFormModel<Inspection_Form_Labour_III_LabourWelfareFund_Act> genericServiceResultTemplate = new GenericFormModel<Inspection_Form_Labour_III_LabourWelfareFund_Act>();
            try
            {

                if (id != 0) //Existing Record
                {
                    var parentWithChildObject = await _iGR_Inspection_Form_Labour_III_LabourWelfareFund_Act
                       .GetAsync(x => x.InspectionRefId == id,  //Conditions         
                         null,                //Orders
                         x => x.Inspection_Master)      //Includes
                       .ConfigureAwait(false);
                    genericServiceResultTemplate.FormModel = parentWithChildObject.FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                genericServiceResultTemplate.HasError = true;
                genericServiceResultTemplate.ErrorDesc = ex.Message;
            }
            return genericServiceResultTemplate;
        }
        public async Task<GenericServiceResultTemplate> AddUpdate_InspectionFormLabourPartIIILabourWelfareFund_Act(Inspection_Form_Labour_III_LabourWelfareFund_Act formModel)
        {
            GenericServiceResultTemplate genericServiceResultTemplate = new GenericServiceResultTemplate();
            try
            {
                genericServiceResultTemplate.HasException = false;
                genericServiceResultTemplate.CustomeValidationResult = CustomeValidator<Inspection_Form_Labour_III_LabourWelfareFund_Act>.ValidateModel_AllProperties(formModel);
                if (formModel.Id == 0)
                {
                    _iGR_Inspection_Form_Labour_III_LabourWelfareFund_Act.Insert(formModel);
                    await _iGR_Inspection_Form_Labour_III_LabourWelfareFund_Act.SavechangeAsync();
                }
                else
                {
                    _context.Update<Inspection_Form_Labour_III_LabourWelfareFund_Act>(formModel);
                    await _iGR_Inspection_Form_Labour_III_LabourWelfareFund_Act.SavechangeAsync();
                }
            }
            catch (Exception ex)
            {
                genericServiceResultTemplate.HasException = true;
                genericServiceResultTemplate.Exceptions = ex;
            }
            return genericServiceResultTemplate;
        }
        public async Task<GenericFormModel<Inspection_Form_Labour_III_GratuityAct>> GetInspectionFormLabourPartIIIGratuityAct(Int64 id)
        {
            GenericFormModel<Inspection_Form_Labour_III_GratuityAct> genericServiceResultTemplate = new GenericFormModel<Inspection_Form_Labour_III_GratuityAct>();
            try
            {

                if (id != 0) //Existing Record
                {
                    var parentWithChildObject = await _iGR_Inspection_Form_Labour_III_GratuityAct
                       .GetAsync(x => x.InspectionRefId == id,  //Conditions         
                         null,                //Orders
                         x => x.Inspection_Master)      //Includes
                       .ConfigureAwait(false);
                    genericServiceResultTemplate.FormModel = parentWithChildObject.FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                genericServiceResultTemplate.HasError = true;
                genericServiceResultTemplate.ErrorDesc = ex.Message;
            }
            return genericServiceResultTemplate;
        }
        public async Task<GenericServiceResultTemplate> AddUpdate_InspectionFormLabourPartIIIGratuityAct(Inspection_Form_Labour_III_GratuityAct formModel)
        {
            GenericServiceResultTemplate genericServiceResultTemplate = new GenericServiceResultTemplate();
            try
            {
                genericServiceResultTemplate.HasException = false;
                genericServiceResultTemplate.CustomeValidationResult = CustomeValidator<Inspection_Form_Labour_III_GratuityAct>.ValidateModel_AllProperties(formModel);
                if (formModel.Id == 0)
                {
                    _iGR_Inspection_Form_Labour_III_GratuityAct.Insert(formModel);
                    await _iGR_Inspection_Form_Labour_III_GratuityAct.SavechangeAsync();
                }
                else
                {
                    _context.Update<Inspection_Form_Labour_III_GratuityAct>(formModel);
                    await _iGR_Inspection_Form_Labour_III_GratuityAct.SavechangeAsync();
                }
            }
            catch (Exception ex)
            {
                genericServiceResultTemplate.HasException = true;
                genericServiceResultTemplate.Exceptions = ex;
            }
            return genericServiceResultTemplate;
        }
        public async Task<GenericFormModel<Inspection_Form_Labour_III_IndustrialEmploymentAct>> GetInspectionFormLabourPartIIIIndustrialEmploymentAct(Int64 id)
        {
            GenericFormModel<Inspection_Form_Labour_III_IndustrialEmploymentAct> genericServiceResultTemplate = new GenericFormModel<Inspection_Form_Labour_III_IndustrialEmploymentAct>();
            try
            {

                if (id != 0) //Existing Record
                {
                    var parentWithChildObject = await _iGR_Inspection_Form_Labour_III_IndustrialEmploymentAct
                       .GetAsync(x => x.InspectionRefId == id,  //Conditions         
                         null,                //Orders
                         x => x.Inspection_Master)      //Includes
                       .ConfigureAwait(false);
                    genericServiceResultTemplate.FormModel = parentWithChildObject.FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                genericServiceResultTemplate.HasError = true;
                genericServiceResultTemplate.ErrorDesc = ex.Message;
            }
            return genericServiceResultTemplate;
        }
        public async Task<GenericServiceResultTemplate> AddUpdate_InspectionFormLabourPartIIIIndustrialEmploymentAct(Inspection_Form_Labour_III_IndustrialEmploymentAct formModel)
        {
            GenericServiceResultTemplate genericServiceResultTemplate = new GenericServiceResultTemplate();
            try
            {
                genericServiceResultTemplate.HasException = false;
                genericServiceResultTemplate.CustomeValidationResult = CustomeValidator<Inspection_Form_Labour_III_IndustrialEmploymentAct>.ValidateModel_AllProperties(formModel);
                if (formModel.Id == 0)
                {
                    _iGR_Inspection_Form_Labour_III_IndustrialEmploymentAct.Insert(formModel);
                    await _iGR_Inspection_Form_Labour_III_IndustrialEmploymentAct.SavechangeAsync();
                }
                else
                {
                    _context.Update<Inspection_Form_Labour_III_IndustrialEmploymentAct>(formModel);
                    await _iGR_Inspection_Form_Labour_III_IndustrialEmploymentAct.SavechangeAsync();
                }
            }
            catch (Exception ex)
            {
                genericServiceResultTemplate.HasException = true;
                genericServiceResultTemplate.Exceptions = ex;
            }
            return genericServiceResultTemplate;
        }
        public async Task<GenericFormModel<Inspection_Form_Labour_III_BOCW_Act>> GetInspectionFormLabourPartIIIBOCW_Act(Int64 id)
        {
            GenericFormModel<Inspection_Form_Labour_III_BOCW_Act> genericServiceResultTemplate = new GenericFormModel<Inspection_Form_Labour_III_BOCW_Act>();
            try
            {

                if (id != 0) //Existing Record
                {
                    var parentWithChildObject = await _iGR_Inspection_Form_Labour_III_BOCW_Act
                       .GetAsync(x => x.InspectionRefId == id,  //Conditions         
                         null,                //Orders
                         x => x.Inspection_Master)      //Includes
                       .ConfigureAwait(false);
                    genericServiceResultTemplate.FormModel = parentWithChildObject.FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                genericServiceResultTemplate.HasError = true;
                genericServiceResultTemplate.ErrorDesc = ex.Message;
            }
            return genericServiceResultTemplate;
        }
        public async Task<GenericServiceResultTemplate> AddUpdate_InspectionFormLabourPartIIIBOCW_Act(Inspection_Form_Labour_III_BOCW_Act formModel)
        {
            GenericServiceResultTemplate genericServiceResultTemplate = new GenericServiceResultTemplate();
            try
            {
                genericServiceResultTemplate.HasException = false;
                genericServiceResultTemplate.CustomeValidationResult = CustomeValidator<Inspection_Form_Labour_III_BOCW_Act>.ValidateModel_AllProperties(formModel);

                if (formModel.Id == 0)
                {
                    _iGR_Inspection_Form_Labour_III_BOCW_Act.Insert(formModel);
                    await _iGR_Inspection_Form_Labour_III_BOCW_Act.SavechangeAsync();
                }
                else
                {
                    _context.Update<Inspection_Form_Labour_III_BOCW_Act>(formModel);
                    await _iGR_Inspection_Form_Labour_III_BOCW_Act.SavechangeAsync();
                }
            }
            catch (Exception ex)
            {
                genericServiceResultTemplate.HasException = true;
                genericServiceResultTemplate.Exceptions = ex;
            }
            return genericServiceResultTemplate;
        }
        public async Task<GenericFormModel<Inspection_Form_Labour_III_ShopAct>> GetInspectionFormLabourPartIIIShopAct(Int64 id)
        {
            GenericFormModel<Inspection_Form_Labour_III_ShopAct> genericServiceResultTemplate = new GenericFormModel<Inspection_Form_Labour_III_ShopAct>();
            try
            {

                if (id != 0) //Existing Record
                {
                    var parentWithChildObject = await _iGR_Inspection_Form_Labour_III_ShopAct
                       .GetAsync(x => x.InspectionRefId == id,  //Conditions         
                         null,                //Orders
                         x => x.Inspection_Master)      //Includes
                       .ConfigureAwait(false);
                    genericServiceResultTemplate.FormModel = parentWithChildObject.FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                genericServiceResultTemplate.HasError = true;
                genericServiceResultTemplate.ErrorDesc = ex.Message;
            }
            return genericServiceResultTemplate;
        }
        public async Task<GenericServiceResultTemplate> AddUpdate_InspectionFormLabourPartIIIShopAct(Inspection_Form_Labour_III_ShopAct formModel)
        {
            GenericServiceResultTemplate genericServiceResultTemplate = new GenericServiceResultTemplate();
            try
            {
                genericServiceResultTemplate.HasException = false;
                genericServiceResultTemplate.CustomeValidationResult = CustomeValidator<Inspection_Form_Labour_III_ShopAct>.ValidateModel_AllProperties(formModel);

                if (formModel.Id == 0)
                {
                    _iGR_Inspection_Form_Labour_III_ShopAct.Insert(formModel);
                    await _iGR_Inspection_Form_Labour_III_ShopAct.SavechangeAsync();
                }
                else
                {
                    _context.Update<Inspection_Form_Labour_III_ShopAct>(formModel);
                    await _iGR_Inspection_Form_Labour_III_ShopAct.SavechangeAsync();
                }
            }
            catch (Exception ex)
            {
                genericServiceResultTemplate.HasException = true;
                genericServiceResultTemplate.Exceptions = ex;
            }
            return genericServiceResultTemplate;
        }
        public async Task<GenericFormModel<Inspection_Form_Labour_III_Observations>> GetInspectionFormLabourPartIIIObservations(Int64 id)
        {
            GenericFormModel<Inspection_Form_Labour_III_Observations> genericServiceResultTemplate = new GenericFormModel<Inspection_Form_Labour_III_Observations>();
            try
            {

                if (id != 0) //Existing Record
                {
                    var parentWithChildObject = await _iGR_Inspection_Form_Labour_III_Observations
                       .GetAsync(x => x.InspectionRefId == id,  //Conditions         
                         null,                //Orders
                         x => x.Inspection_Master)      //Includes
                       .ConfigureAwait(false);
                    genericServiceResultTemplate.FormModel = parentWithChildObject.FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                genericServiceResultTemplate.HasError = true;
                genericServiceResultTemplate.ErrorDesc = ex.Message;
            }
            return genericServiceResultTemplate;
        }
        public async Task<GenericServiceResultTemplate> AddUpdate_InspectionFormLabourPartIIIObservations(Inspection_Form_Labour_III_Observations formModel)
        {
            GenericServiceResultTemplate genericServiceResultTemplate = new GenericServiceResultTemplate();
            try
            {
                genericServiceResultTemplate.HasException = false;
                genericServiceResultTemplate.CustomeValidationResult = CustomeValidator<Inspection_Form_Labour_III_Observations>.ValidateModel_AllProperties(formModel);
                if (formModel.Id == 0)
                {
                    _iGR_Inspection_Form_Labour_III_Observations.Insert(formModel);
                    await _iGR_Inspection_Form_Labour_III_Observations.SavechangeAsync();
                }
                else
                {
                    _context.Update<Inspection_Form_Labour_III_Observations>(formModel);
                    await _iGR_Inspection_Form_Labour_III_Observations.SavechangeAsync();
                }
            }
            catch (Exception ex)
            {
                genericServiceResultTemplate.HasException = true;
                genericServiceResultTemplate.Exceptions = ex;
            }
            return genericServiceResultTemplate;
        }
        public async Task<GenericResponseTemplateModel<List<GetInspections_FactoryPerformaStepStatusViewModel>>> Get_Inspections_LabourPerformaStepStatus(Int64 inspectionRefId)
        {
            GenericResponseTemplateModel<List<GetInspections_FactoryPerformaStepStatusViewModel>> genericFormModel = new GenericResponseTemplateModel<List<GetInspections_FactoryPerformaStepStatusViewModel>>();
            try
            {
                List<StoreProcedureParm> storeProcedureParms = new List<StoreProcedureParm>()
                {
                    new StoreProcedureParm (){ ParmName="InspectionRefId", ParmValue=inspectionRefId.ToString(),isNumber=true}
                };
                genericFormModel.ResponseDataModel = await _iGeneric_SP_Repository.CallStoreProcedureReaderAsync<GetInspections_FactoryPerformaStepStatusViewModel>("sp_Inspections_LabourPerformaStepStatus ", storeProcedureParms);

            }
            catch (Exception ex)
            {
                genericFormModel.HasError = true;
                genericFormModel.ErrorDesc = ex.Message;
            }
            return genericFormModel;
        }

        #endregion

        public async Task<GenericServiceResultTemplate> Lock_Inspection(Inpection_LockViewModel formModel)
        {
            GenericServiceResultTemplate genericServiceResultTemplate = new GenericServiceResultTemplate();
            genericServiceResultTemplate.HasException = false;
            genericServiceResultTemplate.CustomeValidationResult = CustomeValidator<Inpection_LockViewModel>.ValidateModel_AllProperties(formModel);
            try
            {
                if (formModel.Email != "N/A" || formModel.MobileNo != "N/A")
                {

                    var contactDetailsModel = new UpdateAlternateContactDetailsViewModel
                    {
                        Email = formModel.Email,
                        MobileNo = formModel.MobileNo,
                        AppId = formModel.AppId,
                        IsLegacy = formModel.IsLegacy
                    };
                    var result = await Update_ContactDetails(contactDetailsModel);
                }
                Inspection_Master inspection_Master = _context.Inspection_Master.Where(x => x.InspectionId == formModel.InspectionRefId).FirstOrDefault();
                var userDetails = await _iAuthService.GetUserRoleAndProfileDetailByUserId(formModel.UserRefId);
                var roleName = _context.Roles.Where(x => x.Id == userDetails.RoleId).Select(x => x.Name).FirstOrDefault();
                var userRefId = _context.InspectionUserMappings.Where(x => x.LicenceNumber == inspection_Master.LicenceNumber).Select(x => x.UserRefId).FirstOrDefault();
                var officerRole = await _iAuthService.GetUserRoleByRoleId(userDetails.RoleId);

                var violationCount = 0;

                if (roleName == "ADRF" || roleName == "DDRF")
                {
                    var inspectionDate = _context.Inspection_Form_Factory_Part_I_General.Where(x => x.InspectionRefId == formModel.InspectionRefId).Select(x => x.DateOfInspection).FirstOrDefault();

                    {
                        // Assuming inspectionDate is of type DateTime
                        inspection_Master.InspectionDoneOn_Factory_Wing = formModel.InspectionDate ?? DateTime.MaxValue;
                        inspection_Master.InspectionSubmitOn_Factory_Wing = DateTime.Now;
                        inspection_Master.InspectionSubmited_Factory_Wing_ProfileId = userDetails.UserProfileId;
                        inspection_Master.InspectionSubmited_Factory_Wing_RoleId = userDetails.RoleId;
                        inspection_Master.InspectionSubmited_Factory_Wing_UserId = formModel.UserRefId;
                        inspection_Master.IsSubmitted_Factory_Wing = true;
                        inspection_Master.FactoryDeRegistrationNo = formModel.FactoryDeRegistrationNo;
                        inspection_Master.InspectionEstablishmentType = formModel.InspectionEstablishmentType;
                        inspection_Master.InspectionSubmited_Factory_Wing_FactoryExistenceType = formModel.InspectionFactoryExistenceType;
                        inspection_Master.Remarks = formModel.Remarks;
                    };

                    if (formModel.InspectionFactoryExistenceType == InspectionFactoryExistenceTypeEnum.OPEN)
                    {
                        var inspectionFormHealthData = await GetIncpectionFormFactoryPartIIIHealth(formModel.InspectionRefId);
                        var pairsHealth = ModelOps.GetTitleAndValues<Inspection_Form_Factory_Part_III_Health>(inspectionFormHealthData.FormModel);
                        violationCount = pairsHealth.Where(x => x.PropertyName.EndsWith("ViolationExist") && x.Value == "HAS_VIOLATION").Count();
                        if (violationCount == 0)
                        {
                            var inspectionFormSafetyData = await GetIncpectionFormFactoryPartIIISafety(formModel.InspectionRefId);
                            var pairsSafety = ModelOps.GetTitleAndValues<Inspection_Form_Factory_Part_III_Safety>(inspectionFormSafetyData.FormModel);
                            violationCount = pairsSafety.Where(x => x.PropertyName.EndsWith("ViolationExist") && x.Value == "HAS_VIOLATION").Count();
                        }
                        if (violationCount == 0)
                        {
                            var inspectionFormWelfareData = await GetIncpectionGetFormFactoryPartIIIWelfare(formModel.InspectionRefId);
                            var pairsWelfare = ModelOps.GetTitleAndValues<Inspection_Form_Factory_Part_III_Welfare>(inspectionFormWelfareData.FormModel);
                            violationCount = pairsWelfare.Where(x => x.PropertyName.EndsWith("ViolationExist") && x.Value == "HAS_VIOLATION").Count();
                        }
                        if (violationCount == 0)
                        {
                            var inspectionFormGeneralData = await GetIncpectionGetFormFactoryPartIIIGeneral(formModel.InspectionRefId);
                            var pairsGeneral = ModelOps.GetTitleAndValues<Inspection_Form_Factory_Part_III_General>(inspectionFormGeneralData.FormModel);
                            violationCount = pairsGeneral.Where(x => x.PropertyName.EndsWith("ViolationExist") && x.Value == "HAS_VIOLATION").Count();
                        }
                        if (violationCount == 0)
                        {
                            var inspectionFormMajorAccidentData = await GetIncpectionGetFormFactoryPartIIIMajorAccidentHazard(formModel.InspectionRefId);
                            var pairsMajorAccident = ModelOps.GetTitleAndValues<Inspection_Form_Factory_Part_III_MajorAccidentHazard>(inspectionFormMajorAccidentData.FormModel);
                            violationCount = pairsMajorAccident.Where(x => x.PropertyName.EndsWith("ViolationExist") && x.Value == "HAS_VIOLATION").Count();
                        }


                        if (violationCount > 0)
                        {


                            var receiverInfo = await _iAuthService.GetUserProfileByUserId(userRefId);

                            InspectionComplianceLog inspectionComplianceLog = new InspectionComplianceLog();
                            {
                                inspectionComplianceLog.AppActionType = 1;
                                inspectionComplianceLog.Sender_UserRefId = userDetails.UserId;
                                inspectionComplianceLog.Sender_ProfileRefId = userDetails.UserProfileId;
                                inspectionComplianceLog.Receiver_UserRefId = userRefId;
                                inspectionComplianceLog.Receiver_ProfileRefId = receiverInfo == null ? 0 : receiverInfo.UserProfileId;
                                inspectionComplianceLog.ActionDate = DateTime.Now;
                                inspectionComplianceLog.ActionTakenDaysCount = 0;
                                inspectionComplianceLog.ActionTakenHoursCount = 0;
                                inspectionComplianceLog.Remarks = "Violation Report Sent";
                                inspectionComplianceLog.SenderRoleId = userDetails.RoleId;
                                inspectionComplianceLog.ReceiverRoleId = "";
                                inspectionComplianceLog.IsDocumentUploaded = false;
                                inspectionComplianceLog.AppDocumentRefId = 0;
                                inspectionComplianceLog.IpAddress = "";
                                inspectionComplianceLog.Latitude = "";
                                inspectionComplianceLog.Longitude = "";
                                inspectionComplianceLog.InspectionType = 1;
                                inspectionComplianceLog.InspectionRefId = formModel.InspectionRefId;
                            }
                            _iGR_InspectionComplianceLog.Insert(inspectionComplianceLog);
                            await _iGR_InspectionComplianceLog.SavechangeAsync();

                            Inspection_ViolationComplianceReminder inspection_ViolationComplianceReminder = new Inspection_ViolationComplianceReminder();
                            {
                                inspection_ViolationComplianceReminder.StartDate = DateTime.Now;
                                inspection_ViolationComplianceReminder.EndDate = DateTime.Now.AddDays(30);
                                inspection_ViolationComplianceReminder.TimeInDays = 30;
                                inspection_ViolationComplianceReminder.TotalRemindersToBeSend = 1;
                                inspection_ViolationComplianceReminder.TotalRemindersSent = 0;
                                inspection_ViolationComplianceReminder.ReminderStatus = 1;
                                inspection_ViolationComplianceReminder.InspectionType = 1;
                                inspection_ViolationComplianceReminder.InspectionRefId = formModel.InspectionRefId;
                            }
                            _iGR_Inspection_ViolationComplianceReminder.Insert(inspection_ViolationComplianceReminder);
                            await _iGR_Inspection_ViolationComplianceReminder.SavechangeAsync();
                        }
                    }

                    List<StoreProcedureParm> storeProcedureParms = new List<StoreProcedureParm>()
                        {
                        new StoreProcedureParm (){ ParmName="LicenceNo", ParmValue=inspection_Master.LicenceNumber.ToString(), isNumber=false}
                        };
                    var establishmentDetails = await _iGeneric_SP_Repository.CallStoreProcedureReaderAsync<InspectionEstablishmentBasicDetailsViewModel>("sp_GetInspectionEstablishmentBasicDetails", storeProcedureParms);

                    var circleDetails = await _iCircleManager.GetCircleDetailsByCircleId(inspection_Master.FactoryCircleId, CircleTypeEnum.FACTORY_CIRCLE);
                    TemplateInspectionSubmittedNotificationViewModel templateInspectionSubmittedNotificationViewModel = new TemplateInspectionSubmittedNotificationViewModel();
                    templateInspectionSubmittedNotificationViewModel = new TemplateInspectionSubmittedNotificationViewModel()
                    {
                        InspectionType = 1,
                        OfficerName = userDetails.FullName,
                        RoleName = officerRole.NormalizedName,
                        IsViolationFound = violationCount,
                        InspectionDate = inspectionDate == null ? inspection_Master.InspectionDoneOn_Factory_Wing : inspectionDate,
                        EstablishmentName = establishmentDetails.FirstOrDefault().EstablishmentName,
                        EstablishmentAddress = establishmentDetails.FirstOrDefault().EstablishmentAddress,
                        LicenseNumber = establishmentDetails.FirstOrDefault().LicenceNo,
                        FactoryStatus = inspection_Master.InspectionSubmited_Factory_Wing_FactoryExistenceType.ToString(),
                        FactoryCircle = circleDetails.ResponseDataModel
                    };

                    var emailTemplateText = await _iNotificationManagerService.RenderToStringAsync("EmailTemplates/_templateInspectionSubmittedNotification", templateInspectionSubmittedNotificationViewModel);
                    await _iNotificationManagerService.InitiateNotification(userRefId,
                new List<NotificationViewModel>()
                {
                        new NotificationViewModel(){Body=emailTemplateText, NotificationMode=NotificationModeTypeEnum.EMAIL,NotificationPurpose=NotificationPurposeTypeEnum.INFO, Title ="Department of Labour: Inspection submitted(Factory-Wing)"}
                });


                }
                else if (roleName == "LBIN")
                {
                    var inspectionDate = _context.Inspection_Form_Labour_Part_I_General.Where(x => x.InspectionRefId == formModel.InspectionRefId).Select(x => x.DateOfInspection).FirstOrDefault();
                    {
                        inspection_Master.InspectionDoneOn_Labour_Wing = formModel.InspectionDate ?? DateTime.MaxValue;
                        inspection_Master.InspectionSubmitOn_Labour_Wing = DateTime.Now;
                        inspection_Master.InspectionSubmited_Labour_Wing_ProfileId = userDetails.UserProfileId;
                        inspection_Master.InspectionSubmited_Labour_Wing_RoleId = userDetails.RoleId;
                        inspection_Master.InspectionSubmited_Labour_Wing_UserId = formModel.UserRefId;
                        inspection_Master.IsSubmitted_Labour_Wing = true;
                        inspection_Master.FactoryDeRegistrationNo = formModel.FactoryDeRegistrationNo;
                        inspection_Master.InspectionEstablishmentType = formModel.InspectionEstablishmentType;
                        inspection_Master.InspectionSubmited_Labour_Wing_FactoryExistenceType = formModel.InspectionFactoryExistenceType;
                        inspection_Master.Remarks = formModel.Remarks;
                    };

                    if (formModel.InspectionFactoryExistenceType == InspectionFactoryExistenceTypeEnum.OPEN)
                    {

                        var inspectionFormEqualEnumerationAct = await GetInspectionFormLabourPartIIIEqualEnumerationAct(formModel.InspectionRefId);
                        var pairsEnumerationAct = ModelOps.GetTitleAndValues<Inspection_Form_Labour_Part_III_EqualEnumerationAct>(inspectionFormEqualEnumerationAct.FormModel);
                        violationCount = pairsEnumerationAct.Where(x => x.PropertyName.EndsWith("ViolationExist") && x.Value == "HAS_VIOLATION").Count();

                        if (violationCount == 0)
                        {
                            var inspectionFormLabourIIIMinimumWageAct = await GetInspectionFormLabourPartIIIMinimumWagesAct(formModel.InspectionRefId);
                            var pairsMinimumWageAct = ModelOps.GetTitleAndValues<Inspection_Form_Labour_III_MinimumWageAct>(inspectionFormLabourIIIMinimumWageAct.FormModel);
                            violationCount = pairsMinimumWageAct.Where(x => x.PropertyName.EndsWith("ViolationExist") && x.Value == "HAS_VIOLATION").Count();
                        }

                        if (violationCount == 0)
                        {
                            var inspectionFormLabourIIIPaymentWagesAct = await GetInspectionFormLabourPartIIIPaymentWagesAct(formModel.InspectionRefId);
                            var pairsPaymentWagesAct = ModelOps.GetTitleAndValues<Inspection_Form_Labour_III_PaymentWagesAct>(inspectionFormLabourIIIPaymentWagesAct.FormModel);
                            violationCount = pairsPaymentWagesAct.Where(x => x.PropertyName.EndsWith("ViolationExist") && x.Value == "HAS_VIOLATION").Count();
                        }

                        if (violationCount == 0)
                        {
                            var inspectionFormLabourIIIPaymentBonusActStatutoryReport = await GetInspectionFormLabourPartIIIPaymentBonusAct_StatutoryReport(formModel.InspectionRefId);
                            var pairsPaymentBonusActStatutoryReport = ModelOps.GetTitleAndValues<Inspection_Form_Labour_III_PaymentBonusAct_StatutoryReport>(inspectionFormLabourIIIPaymentBonusActStatutoryReport.FormModel);
                            var propNamesPaymentBonusActStatutoryReport = pairsPaymentBonusActStatutoryReport.Where(x => x.PropertyName.EndsWith("ViolationExist") && x.Value == "HAS_VIOLATION").Count();
                        }

                        if (violationCount == 0)
                        {
                            var inspectionFormLabourIIIChildAndAdolescentLabourAct = await GetInspectionFormLabourPartIIIChildAndAdolescentLabourAct(formModel.InspectionRefId);
                            var pairsPaymentChildAndAdolescentLabourAct = ModelOps.GetTitleAndValues<Inspection_Form_Labour_III_ChildAndAdolescentLabourAct>(inspectionFormLabourIIIChildAndAdolescentLabourAct.FormModel);
                            violationCount = pairsPaymentChildAndAdolescentLabourAct.Where(x => x.PropertyName.EndsWith("ViolationExist") && x.Value == "HAS_VIOLATION").Count();
                        }

                        if (violationCount == 0)
                        {
                            var inspectionFormLabourIIINationalAndFestivalHolidays = await GetInspectionFormLabourPartIIINationalAndFestivalHolidays(formModel.InspectionRefId);
                            var pairsNationalAndFestivalHolidays = ModelOps.GetTitleAndValues<Inspection_Form_Labour_III_NationalAndFestivalHolidays>(inspectionFormLabourIIINationalAndFestivalHolidays.FormModel);
                            violationCount = pairsNationalAndFestivalHolidays.Where(x => x.PropertyName.EndsWith("ViolationExist") && x.Value == "HAS_VIOLATION").Count();
                        }

                        if (violationCount == 0)
                        {
                            var inspectionFormLabourIIIMaternityBenefitAct = await GetInspectionFormLabourPartIIIMaternityBenefitAct(formModel.InspectionRefId);
                            var pairsMaternityBenefitAct = ModelOps.GetTitleAndValues<Inspection_Form_Labour_III_MaternityBenefitAct>(inspectionFormLabourIIIMaternityBenefitAct.FormModel);
                            violationCount = pairsMaternityBenefitAct.Where(x => x.PropertyName.EndsWith("ViolationExist") && x.Value == "HAS_VIOLATION").Count();
                        }


                        if (violationCount == 0)
                        {
                            var inspectionFormLabourIIIContractLabourAct = await GetInspectionFormLabourPartIIIContractLabourAct(formModel.InspectionRefId);
                            var pairsContractLabourAct = ModelOps.GetTitleAndValues<Inspection_Form_Labour_III_ContractLabourAct>(inspectionFormLabourIIIContractLabourAct.FormModel);
                            violationCount = pairsContractLabourAct.Where(x => x.PropertyName.EndsWith("ViolationExist") && x.Value == "HAS_VIOLATION").Count();
                        }

                        if (violationCount == 0)
                        {
                            var inspectionFormLabourIIIInterStateMigrantWorkmenAct = await GetInspectionFormLabourPartIIIInterStateMigrantWorkmenAct(formModel.InspectionRefId);
                            var pairsInterStateMigrantWorkmenAct = ModelOps.GetTitleAndValues<Inspection_Form_Labour_III_InterStateMigrantWorkmenAct>(inspectionFormLabourIIIInterStateMigrantWorkmenAct.FormModel);
                            violationCount = pairsInterStateMigrantWorkmenAct.Where(x => x.PropertyName.EndsWith("ViolationExist") && x.Value == "HAS_VIOLATION").Count();
                        }
                            if (violationCount == 0)
                        {
                            var inspectionFormLabourIIILabourWelfareFundAct = await GetInspectionFormLabourPartIIILabourWelfareFund_Act(formModel.InspectionRefId);
                            var pairsLabourWelfareFundAct = ModelOps.GetTitleAndValues<Inspection_Form_Labour_III_LabourWelfareFund_Act>(inspectionFormLabourIIILabourWelfareFundAct.FormModel);
                                violationCount = pairsLabourWelfareFundAct.Where(x => x.PropertyName.EndsWith("ViolationExist") && x.Value == "HAS_VIOLATION").Count();
                            }

                        if (violationCount == 0)
                        {
                            var inspectionFormLabourIIIGratuityAct = await GetInspectionFormLabourPartIIIGratuityAct(formModel.InspectionRefId);
                            var pairsGratuityAct = ModelOps.GetTitleAndValues<Inspection_Form_Labour_III_GratuityAct>(inspectionFormLabourIIIGratuityAct.FormModel);
                                violationCount = pairsGratuityAct.Where(x => x.PropertyName.EndsWith("ViolationExist") && x.Value == "HAS_VIOLATION").Count();
                            }

                        if (violationCount == 0)
                        {
                            var inspectionFormLabourIIIIndustrialEmploymentAct = await GetInspectionFormLabourPartIIIIndustrialEmploymentAct(formModel.InspectionRefId);
                            var pairsIndustrialEmploymentAct = ModelOps.GetTitleAndValues<Inspection_Form_Labour_III_IndustrialEmploymentAct>(inspectionFormLabourIIIIndustrialEmploymentAct.FormModel);
                                violationCount = pairsIndustrialEmploymentAct.Where(x => x.PropertyName.EndsWith("ViolationExist") && x.Value == "HAS_VIOLATION").Count();
                            }

                        if (violationCount > 0)
                        {

                            var receiverInfo = await _iAuthService.GetUserProfileByUserId(userRefId);

                            InspectionComplianceLog inspectionComplianceLog = new InspectionComplianceLog();
                            {
                                inspectionComplianceLog.AppActionType = 1;
                                inspectionComplianceLog.Sender_UserRefId = userDetails.UserId;
                                inspectionComplianceLog.Sender_ProfileRefId = userDetails.UserProfileId;
                                inspectionComplianceLog.Receiver_UserRefId = userRefId;
                                inspectionComplianceLog.Receiver_ProfileRefId = receiverInfo == null ? 0 : receiverInfo.UserProfileId;
                                inspectionComplianceLog.ActionDate = DateTime.Now;
                                inspectionComplianceLog.ActionTakenDaysCount = 0;
                                inspectionComplianceLog.ActionTakenHoursCount = 0;
                                inspectionComplianceLog.Remarks = "Violation Report Sent";
                                inspectionComplianceLog.SenderRoleId = userDetails.RoleId;
                                inspectionComplianceLog.ReceiverRoleId = "";
                                inspectionComplianceLog.IsDocumentUploaded = false;
                                inspectionComplianceLog.AppDocumentRefId = 0;
                                inspectionComplianceLog.IpAddress = "";
                                inspectionComplianceLog.Latitude = "";
                                inspectionComplianceLog.Longitude = "";
                                inspectionComplianceLog.InspectionType = 2;
                                inspectionComplianceLog.InspectionRefId = formModel.InspectionRefId;
                            }
                            _iGR_InspectionComplianceLog.Insert(inspectionComplianceLog);
                            await _iGR_InspectionComplianceLog.SavechangeAsync();

                            Inspection_ViolationComplianceReminder inspection_ViolationComplianceReminder = new Inspection_ViolationComplianceReminder();
                            {
                                inspection_ViolationComplianceReminder.StartDate = DateTime.Now;
                                inspection_ViolationComplianceReminder.EndDate = DateTime.Now.AddDays(30);
                                inspection_ViolationComplianceReminder.TimeInDays = 30;
                                inspection_ViolationComplianceReminder.TotalRemindersToBeSend = 1;
                                inspection_ViolationComplianceReminder.TotalRemindersSent = 0;
                                inspection_ViolationComplianceReminder.ReminderStatus = 1;
                                inspection_ViolationComplianceReminder.InspectionType = 2;
                                inspection_ViolationComplianceReminder.InspectionRefId = formModel.InspectionRefId;
                            }
                            _iGR_Inspection_ViolationComplianceReminder.Insert(inspection_ViolationComplianceReminder);
                            await _iGR_Inspection_ViolationComplianceReminder.SavechangeAsync();
                        }
                    }

                    List<StoreProcedureParm> storeProcedureParms = new List<StoreProcedureParm>()
                        {
                        new StoreProcedureParm (){ ParmName="LicenceNo", ParmValue=inspection_Master.LicenceNumber.ToString(), isNumber=false}
                        };
                    var establishmentDetails = await _iGeneric_SP_Repository.CallStoreProcedureReaderAsync<InspectionEstablishmentBasicDetailsViewModel>("sp_GetInspectionEstablishmentBasicDetails", storeProcedureParms);

                    var circleDetails = await _iCircleManager.GetCircleDetailsByCircleId(inspection_Master.LabourCircleRefId, CircleTypeEnum.LABOUR_CIRCLE);
                    TemplateInspectionSubmittedNotificationViewModel templateInspectionSubmittedNotificationViewModel = new TemplateInspectionSubmittedNotificationViewModel();
                    templateInspectionSubmittedNotificationViewModel = new TemplateInspectionSubmittedNotificationViewModel()
                    {
                        InspectionType = 2,
                        OfficerName = userDetails.FullName,
                        RoleName = officerRole.NormalizedName,
                        IsViolationFound = violationCount,
                        InspectionDate = inspectionDate == null ? inspection_Master.InspectionDoneOn_Factory_Wing : inspectionDate,
                        EstablishmentName = establishmentDetails.FirstOrDefault().EstablishmentName,
                        EstablishmentAddress = establishmentDetails.FirstOrDefault().EstablishmentAddress,
                        LicenseNumber = establishmentDetails.FirstOrDefault().LicenceNo,
                        FactoryStatus = inspection_Master.InspectionSubmited_Labour_Wing_FactoryExistenceType.ToString(),
                        FactoryCircle = circleDetails.ResponseDataModel
                    };

                    var emailTemplateText = await _iNotificationManagerService.RenderToStringAsync("EmailTemplates/_templateInspectionSubmittedNotification", templateInspectionSubmittedNotificationViewModel);
                    await _iNotificationManagerService.InitiateNotification(userRefId,
                    new List<NotificationViewModel>()
                    {
                            new NotificationViewModel(){Body=emailTemplateText, NotificationMode=NotificationModeTypeEnum.EMAIL,NotificationPurpose=NotificationPurposeTypeEnum.INFO, Title ="Department of Labour: Inspection submitted(Labour-Wing)"}
                    });
                }
                _iGR_Inspection_Master.Update(inspection_Master);
                await _iGR_Inspection_Master.SavechangeAsync();

                
            }
            catch (Exception ex)
            {
                genericServiceResultTemplate.HasException = true;
                genericServiceResultTemplate.Exceptions = ex;
            }
            return genericServiceResultTemplate;
        }

        public async Task<GenericResponseTemplateModel<InspectionEstablishmentBasicDetailsViewModel>> GetInspectionEstablishmentBasicDetails(string licenceNo)
        {
            GenericResponseTemplateModel<InspectionEstablishmentBasicDetailsViewModel> genericFormModel = new GenericResponseTemplateModel<InspectionEstablishmentBasicDetailsViewModel>();
            try
            {
                List<StoreProcedureParm> storeProcedureParms = new List<StoreProcedureParm>()
                {
                    new StoreProcedureParm (){ ParmName="LicenceNo", ParmValue=licenceNo.ToString(), isNumber=false}
                };
                var EstablishmentDetails = await _iGeneric_SP_Repository.CallStoreProcedureReaderAsync<InspectionEstablishmentBasicDetailsViewModel>("sp_GetInspectionEstablishmentBasicDetails", storeProcedureParms);
                genericFormModel.ResponseDataModel = EstablishmentDetails.FirstOrDefault();
            }
            catch (Exception ex)
            {
                genericFormModel.HasError = true;
                genericFormModel.ErrorDesc = ex.Message;
            }
            return genericFormModel;
        }
        

        public async Task<GenericResponseTemplateModel<InspectionFactoryDetailsViewModel>> GetInspectionFactoryDetailsByLicenceNo(string licenceNo )
        {
            GenericResponseTemplateModel<InspectionFactoryDetailsViewModel> genericFormModel = new GenericResponseTemplateModel<InspectionFactoryDetailsViewModel>();
            try
            {
                List<StoreProcedureParm> storeProcedureParms = new List<StoreProcedureParm>()
                {
                    new StoreProcedureParm (){ ParmName="LicenceNo", ParmValue=licenceNo.ToString(), isNumber=false}
                };
                var EstablishmentDetails = await _iGeneric_SP_Repository.CallStoreProcedureReaderAsync<InspectionFactoryDetailsViewModel>("sp_GetInspectionFactoryDetailsByLicenceNo", storeProcedureParms);
                    genericFormModel.ResponseDataModel = EstablishmentDetails.FirstOrDefault();

            }
            catch (Exception ex)
            {
                genericFormModel.HasError = true;
                genericFormModel.ErrorDesc = ex.Message;
            }
            return genericFormModel;
        }

        public async Task<GenericResponseTemplateModel<List<InspectionTransferInfoViewModel>>> GetLabourCircleOfficersByInspectionRefId(string userRefId, string roleName)
        {
            GenericResponseTemplateModel<List<InspectionTransferInfoViewModel>> genericFormModel = new GenericResponseTemplateModel<List<InspectionTransferInfoViewModel>>();
            try
            {
                List<StoreProcedureParm> storeProcedureParms = new List<StoreProcedureParm>()
                {
                    new StoreProcedureParm (){ ParmName="UserRefId", ParmValue=userRefId.ToString(), isNumber=false},
                    new StoreProcedureParm (){ ParmName="RoleName", ParmValue=roleName.ToString(), isNumber=false}
                };
                genericFormModel.ResponseDataModel = await _iGeneric_SP_Repository.CallStoreProcedureReaderAsync<InspectionTransferInfoViewModel>("sp_GetLabourCircleOfficersByInspectionRefId", storeProcedureParms);
            }
            catch (Exception ex)
            {
                genericFormModel.HasError = true;
                genericFormModel.ErrorDesc = ex.Message;
            }
            return genericFormModel;
        }
        
        public async Task<GenericResponseTemplateModel<Int64>> GetDistrictRefIdByUserId(string userId)
        {
            GenericResponseTemplateModel<Int64> genericFormModel = new GenericResponseTemplateModel<Int64>();
            try
            {
                genericFormModel.ResponseDataModel = _context.UserCircleMappings.Where(x => x.UserRefId == userId && x.Version == 2).Include(x => x.FactoryCircle).Select(x => x.FactoryCircle.DistrictLgdRefId).FirstOrDefault();
            }
            catch (Exception ex)
            {
                genericFormModel.HasError = true;
                genericFormModel.ErrorDesc = ex.Message;
            }
            return genericFormModel;
        }

        public async Task<GenericResponseTemplateModel<GetFactoryCirlceViewModel>> GetFactoryCircleIdByLabourCircleId(Int64 labourCircleRefId)
        {
            GenericResponseTemplateModel<GetFactoryCirlceViewModel> genericFormModel = new GenericResponseTemplateModel<GetFactoryCirlceViewModel>();
            try
            {
                List<StoreProcedureParm> storeProcedureParms = new List<StoreProcedureParm>()
                {
                    new StoreProcedureParm (){ ParmName="LabourCircleId", ParmValue=labourCircleRefId.ToString(), isNumber=true}
                };
                var EstablishmentDetails = await _iGeneric_SP_Repository.CallStoreProcedureReaderAsync<GetFactoryCirlceViewModel>("sp_GetFactoryCircleIdBylabourCirlceId", storeProcedureParms);
                genericFormModel.ResponseDataModel = EstablishmentDetails.FirstOrDefault();

            }
            catch (Exception ex)
            {
                genericFormModel.HasError = true;
                genericFormModel.ErrorDesc = ex.Message;
            }
            return genericFormModel;
        }

        public async Task<GenericResponseTemplateModel<Inpection_LockViewModel>> Get_Inspection_OperationalStatus(Int64 inspectionRefId, string roleName)
        {
            var genericFormModel = new GenericResponseTemplateModel<Inpection_LockViewModel>();

            try
            {
                var inspection_Master = _context.Inspection_Master
                                                .FirstOrDefault(x => x.InspectionId == inspectionRefId);


                var inspection_lockinfo = await _iGR_Inspection_LockInfo
                                                .GetAsync(x => x.RandomizationRefId == inspection_Master.RandomizationRefId)
                                                .ConfigureAwait(false);

                var lockInfo = inspection_lockinfo.FirstOrDefault();

                if (roleName == "LBIN")
                {
                    genericFormModel.ResponseDataModel = new Inpection_LockViewModel
                    {
                        FactoryDeRegistrationNo = string.IsNullOrEmpty(inspection_Master.FactoryDeRegistrationNo) ? "NA" : inspection_Master.FactoryDeRegistrationNo,
                        InspectionFactoryExistenceType = inspection_Master.InspectionSubmited_Labour_Wing_FactoryExistenceType,
                        InspectionEstablishmentType = inspection_Master.InspectionEstablishmentType,
                        Remarks = inspection_Master.Remarks,
                        InspectionRefId = inspection_Master.InspectionId,
                        InspectionDate = inspection_Master.InspectionDoneOn_Labour_Wing.Value,
                        UserRefId = inspection_Master.InspectionSubmited_Labour_Wing_UserId
                    };
                }
                
                else
                {
                    genericFormModel.ResponseDataModel = new Inpection_LockViewModel
                    {
                        FactoryDeRegistrationNo = string.IsNullOrEmpty(inspection_Master.FactoryDeRegistrationNo) ? "NA" : inspection_Master.FactoryDeRegistrationNo,
                        InspectionFactoryExistenceType = inspection_Master.InspectionSubmited_Factory_Wing_FactoryExistenceType,
                        InspectionEstablishmentType = inspection_Master.InspectionEstablishmentType,
                        Remarks = inspection_Master.Remarks,
                        InspectionRefId = inspection_Master.InspectionId,
                        InspectionDate = inspection_Master.InspectionDoneOn_Factory_Wing.Value,
                        UserRefId = inspection_Master.InspectionSubmited_Factory_Wing_UserId
                    };
                }

                if (genericFormModel.ResponseDataModel.InspectionDate == null)
                {
                    genericFormModel.HasError = true;
                    genericFormModel.ErrorDesc = "Inspection date is not set.";
                }
            }
            catch (Exception ex)
            {
                genericFormModel.HasError = true;
                genericFormModel.ErrorDesc = ex.Message;
            }

            return genericFormModel;
        }

        public async Task<GenericFormModel<List<InspectionLabourCircleViewModel>>> GetInspection_LabourCircleByDistricRefId(int districtRefId)
        {
            GenericFormModel<List<InspectionLabourCircleViewModel>> genericFormModel = new GenericFormModel<List<InspectionLabourCircleViewModel>>();
            try
            {
                List<StoreProcedureParm> storeProcedureParms = new List<StoreProcedureParm>()
                {
                    new StoreProcedureParm (){ ParmName="districtRefId", ParmValue=districtRefId.ToString(), isNumber=true}
                };
                genericFormModel.FormModel = await _iGeneric_SP_Repository.CallStoreProcedureReaderAsync<InspectionLabourCircleViewModel>("sp_Inspection_GetLabourCircleByDistrictRefId", storeProcedureParms);
            }
            catch (Exception ex)
            {
                genericFormModel.HasError = true;
                genericFormModel.ErrorDesc = ex.Message;
            }
            return genericFormModel;
        }

        public async Task<GenericServiceResultTemplate> Update_ContactDetails(UpdateAlternateContactDetailsViewModel formModel)
        {
            GenericServiceResultTemplate genericServiceResultTemplate = new GenericServiceResultTemplate();
            genericServiceResultTemplate.HasException = false;
            genericServiceResultTemplate.CustomeValidationResult = CustomeValidator<UpdateAlternateContactDetailsViewModel>.ValidateModel_AllProperties(formModel);
            try
            {
                List<StoreProcedureParm> storeProcedureParms = new List<StoreProcedureParm>()
                {
                    new StoreProcedureParm (){ ParmName="email", ParmValue=formModel.Email, isNumber=false},
                    new StoreProcedureParm (){ ParmName="mobileNo", ParmValue=formModel.MobileNo, isNumber=false},
                    new StoreProcedureParm (){ ParmName="appId", ParmValue=formModel.AppId.ToString(), isNumber=true},
                    new StoreProcedureParm (){ ParmName="isLegacy", ParmValue=formModel.IsLegacy.ToString(), isNumber=true}
                };
                var intReturn = await _iGeneric_SP_Repository.CallStoreProcedureReaderScalerAsync("sp_Update_AlternateContactDetails", storeProcedureParms);
            }
            catch (Exception ex)
            {
                genericServiceResultTemplate.HasException = true;
                genericServiceResultTemplate.Exceptions = ex;
            }
            return genericServiceResultTemplate;
        }

        public async Task<GenericResponseTemplateModel<AlternateContactDetailsViewModel>> Get_ContactDetails(Int64 appId, bool isLegacy)
        {
            var genericFormModel = new GenericResponseTemplateModel<AlternateContactDetailsViewModel>();

            try
            {
                List<StoreProcedureParm> storeProcedureParms = new List<StoreProcedureParm>()
                {
                    new StoreProcedureParm (){ ParmName="appId", ParmValue=appId.ToString(), isNumber=true},
                    new StoreProcedureParm (){ ParmName="isLegacy", ParmValue=isLegacy.ToString(), isNumber=true}
                };
                 var contactDetails = await _iGeneric_SP_Repository.CallStoreProcedureReaderAsync<AlternateContactDetailsViewModel>("sp_Get_AlternateContactDetails", storeProcedureParms);
                genericFormModel.ResponseDataModel = contactDetails.FirstOrDefault();
            }
            catch (Exception ex)
            {
                genericFormModel.HasError = true;
                genericFormModel.ErrorDesc = ex.Message;
            }

            return genericFormModel;
        }

        public async Task<GenericResponseTemplateModel<IntReturn>> VerifyLicenceNumber(string licenceNumber)
        {
            GenericResponseTemplateModel<IntReturn> genericResponseTemplateModel = new GenericResponseTemplateModel<IntReturn>();
            try
            {
                List<StoreProcedureParm> storeProcedureParms = new List<StoreProcedureParm>()
                {
                    new StoreProcedureParm (){ ParmName="licenceNumber", ParmValue=licenceNumber, isNumber=false},

                };
                genericResponseTemplateModel.ResponseDataModel = await _iGeneric_SP_Repository.CallStoreProcedureReaderScalerAsync("sp_VerifyLicenceNumberByUserId", storeProcedureParms);
            }
            catch (Exception ex)
            {
                genericResponseTemplateModel.HasError = true;
                genericResponseTemplateModel.ErrorDesc = ex.Message;
            }
            return genericResponseTemplateModel;
        }

        public async Task<GenericResponseTemplateModel<LicenceNumberDetailsViewModel>> VerifyLicenceNumberWithUser(string licenceNumber, string userRefId)
        {
            GenericResponseTemplateModel<LicenceNumberDetailsViewModel> genericResponseTemplateModel = new GenericResponseTemplateModel<LicenceNumberDetailsViewModel>();
            try
            {
                List<StoreProcedureParm> storeProcedureParms = new List<StoreProcedureParm>()
                {
                    new StoreProcedureParm (){ ParmName="licenceNumber", ParmValue=licenceNumber, isNumber=false},
                    new StoreProcedureParm (){ ParmName="userRefId", ParmValue=userRefId, isNumber=false},

                };
                var licenceNumberDetails = await _iGeneric_SP_Repository.CallStoreProcedureReaderAsync<LicenceNumberDetailsViewModel>("sp_VerifyLicenceNumberMappingWithUserId", storeProcedureParms);

                genericResponseTemplateModel.ResponseDataModel = licenceNumberDetails.FirstOrDefault();
            }
            catch (Exception ex)
            {
                genericResponseTemplateModel.HasError = true;
                genericResponseTemplateModel.ErrorDesc = ex.Message;
            }
            return genericResponseTemplateModel;
        }

        public async Task<GenericResponseTemplateModel<List<InspectionComplianceinfoViewModel>>> Get_inspectionsDataByUserId(string searchCode, int pageNo, int pageSize, string sortColumn, string sortOrder, string filterArray, string userRefId, int investpunjab_ipin, string roleName)
        {
            GenericResponseTemplateModel<List<InspectionComplianceinfoViewModel>> genericFormModel = new GenericResponseTemplateModel<List<InspectionComplianceinfoViewModel>>();
            try
            {
                var userIds = userRefId;
                if (roleName == "INDL")
                {
                    if (investpunjab_ipin > 0)
                    {
                        List<StoreProcedureParm> storeProcedureParms1 = new List<StoreProcedureParm>()
                        {
                        new StoreProcedureParm (){ ParmName="InvestPunjab_Ipin", ParmValue=investpunjab_ipin.ToString(),isNumber=true}
                        };

                        var userRefIdsAsPerIpin = await _iGeneric_SP_Repository.CallStoreProcedureReaderAsync<UserRefIdsAsPerIpinViewModel>("sp_FindmaximumUsersByIpin", storeProcedureParms1);

                        if (userRefIdsAsPerIpin != null && userRefIdsAsPerIpin.Any())
                        {
                            userIds = userRefId + "," + string.Join(",", userRefIdsAsPerIpin.Select(x => x.UserRefId.ToString()));
                        }
                    }
                }

                List<StoreProcedureParm> storeProcedureParms = new List<StoreProcedureParm>()
                {
                    new StoreProcedureParm (){ ParmName="userRefId", ParmValue=userIds.ToString(),isNumber=false},

                    new StoreProcedureParm (){isNumber=false, ParmName="SearchCode",ParmValue= searchCode==null ? "": searchCode},
                    new StoreProcedureParm (){isNumber=true, ParmName="PageNo", ParmValue=pageNo.ToString()},
                    new StoreProcedureParm (){isNumber=true, ParmName="PageSize", ParmValue=pageSize.ToString()},
                    new StoreProcedureParm (){isNumber=false, ParmName="SortColumn", ParmValue=sortColumn.ToString()},
                    new StoreProcedureParm (){isNumber=false, ParmName="SortOrder", ParmValue = sortOrder=="1" ? "ASC" : "DESC" }
                };
                genericFormModel.ResponseDataModel = await _iGeneric_SP_Repository.CallStoreProcedureReaderAsync<InspectionComplianceinfoViewModel>("sp_GetInspectionsByUserId", storeProcedureParms);
                if (genericFormModel.ResponseDataModel.Count() > 0)
                {
                    genericFormModel.ResponseDataModel.Select(x => { x.EstablishmentName = x.EstbDetail.Split("|")[0]; x.EstablishmentAddress = x.EstbDetail.Split("|")[1]; return x; }).ToList();
                }

            }
            catch (Exception ex)
            {
                genericFormModel.HasError = true;
                genericFormModel.ErrorDesc = ex.Message;
            }
            return genericFormModel;
        }

        public async Task<GenericResponseTemplateModel<InspectionSectionWiseKeyValuePair>> GetInspectionViolationData(Int64 inspectionRefId, int inspectionType)
        {
            GenericResponseTemplateModel<InspectionSectionWiseKeyValuePair> genericFormModel = new GenericResponseTemplateModel<InspectionSectionWiseKeyValuePair>();
            try
            {
                var inspectionMasterData = _context.Inspection_Master.Where(x => x.InspectionId == inspectionRefId).FirstOrDefault();
                var establishmentDetails = await GetInspectionEstablishmentBasicDetails(inspectionMasterData.LicenceNumber);

                InspectionSectionWiseKeyValuePair pairs = new InspectionSectionWiseKeyValuePair();
                if (inspectionType == 1)
                {
                    var factoryGeneralData = await GetIncpectionFormFactoryPartIGeneralDetail(inspectionRefId);
                    pairs.pairs_GeneralDetails = ModelOps.GetTitleAndValues<Inspection_Form_Factory_Part_I_General>(factoryGeneralData.FormModel);

                    var factoryData = await GetIncpectionFormFactoryPartIIFactoryDetail(inspectionRefId);
                    pairs.pairs_FactoryDetails = ModelOps.GetTitleAndValues<Inspection_Form_Factory_Part_II_FactoryDetail>(factoryData.FormModel);

                    var inspectionFormHealthData = await GetIncpectionFormFactoryPartIIIHealth(inspectionRefId);
                    var pairsHealth = ModelOps.GetTitleAndValues<Inspection_Form_Factory_Part_III_Health>(inspectionFormHealthData.FormModel);
                    var propNames = pairsHealth.Where(x => x.PropertyName.EndsWith("ViolationExist") && x.Value == "HAS_VIOLATION").Select(x => x.PropertyName.Replace("ViolationExist", "")).ToArray();
                    pairs.pairs_Health = new List<PropertyTitleValuePair>();
                    foreach (var propName in propNames)
                    {
                        pairs.pairs_Health.Add(new PropertyTitleValuePair()
                        {
                            PropertyName = propName,
                            Title = pairsHealth.Where(x => x.PropertyName == propName + "Selection").Select(x => x.Title).FirstOrDefault(),
                            Value = pairsHealth.Where(x => x.PropertyName == propName + "Remarks").Select(x => x.Value).FirstOrDefault()
                        });
                    }


                    var inspectionFormSafetyData = await GetIncpectionFormFactoryPartIIISafety(inspectionRefId);
                    var pairsSafety = ModelOps.GetTitleAndValues<Inspection_Form_Factory_Part_III_Safety>(inspectionFormSafetyData.FormModel);
                    var propNamesSafety = pairsSafety.Where(x => x.PropertyName.EndsWith("ViolationExist") && x.Value == "HAS_VIOLATION").Select(x => x.PropertyName.Replace("ViolationExist", "")).ToArray();

                    pairs.pairs_Safety = new List<PropertyTitleValuePair>();
                    foreach (var propName1 in propNamesSafety)
                    {
                        pairs.pairs_Safety.Add(new PropertyTitleValuePair()
                        {
                            PropertyName = propName1,
                            Title = pairsSafety.Where(x => x.PropertyName == propName1 + "Selection").Select(x => x.Title).FirstOrDefault(),
                            Value = pairsSafety.Where(x => x.PropertyName == propName1 + "Remarks").Select(x => x.Value).FirstOrDefault()
                        });
                    }



                    var inspectionFormWelfareData = await GetIncpectionGetFormFactoryPartIIIWelfare(inspectionRefId);
                    var pairsWelfare = ModelOps.GetTitleAndValues<Inspection_Form_Factory_Part_III_Welfare>(inspectionFormWelfareData.FormModel);
                    var propNamesWelfare = pairsWelfare.Where(x => x.PropertyName.EndsWith("ViolationExist") && x.Value == "HAS_VIOLATION").Select(x => x.PropertyName.Replace("ViolationExist", "")).ToArray();
                    pairs.pairs_Welfare = new List<PropertyTitleValuePair>();
                    foreach (var propName in propNamesWelfare)
                    {
                        pairs.pairs_Welfare.Add(new PropertyTitleValuePair()
                        {
                            PropertyName = propName,
                            Title = pairsWelfare.Where(x => x.PropertyName == propName + "Selection").Select(x => x.Title).FirstOrDefault(),
                            Value = pairsWelfare.Where(x => x.PropertyName == propName + "Remarks").Select(x => x.Value).FirstOrDefault()
                        });
                    }



                    var inspectionFormGeneralData = await GetIncpectionGetFormFactoryPartIIIGeneral(inspectionRefId);
                    var pairsGeneral = ModelOps.GetTitleAndValues<Inspection_Form_Factory_Part_III_General>(inspectionFormGeneralData.FormModel);
                    var propNamesGeneral = pairsGeneral.Where(x => x.PropertyName.EndsWith("ViolationExist") && x.Value == "HAS_VIOLATION").Select(x => x.PropertyName.Replace("ViolationExist", "")).ToArray();

                    pairs.pairs_General = new List<PropertyTitleValuePair>();
                    foreach (var propName in propNamesGeneral)
                    {
                        pairs.pairs_General.Add(new PropertyTitleValuePair()
                        {
                            PropertyName = propName,
                            Title = pairsGeneral.Where(x => x.PropertyName == propName + "Selection").Select(x => x.Title).FirstOrDefault(),
                            Value = pairsGeneral.Where(x => x.PropertyName == propName + "Remarks").Select(x => x.Value).FirstOrDefault()
                        });
                    }


                    var inspectionFormMajorAccidentData = await GetIncpectionGetFormFactoryPartIIIMajorAccidentHazard(inspectionRefId);
                    var pairsMajorAccident = ModelOps.GetTitleAndValues<Inspection_Form_Factory_Part_III_MajorAccidentHazard>(inspectionFormMajorAccidentData.FormModel);
                    var propNamesMajorAccident = pairsMajorAccident.Where(x => x.PropertyName.EndsWith("ViolationExist") && x.Value == "HAS_VIOLATION").Select(x => x.PropertyName.Replace("_ViolationExist", "")).ToArray();
                    pairs.pairs_Accident = new List<PropertyTitleValuePair>();
                    foreach (var propName in propNamesMajorAccident)
                    {
                        pairs.pairs_Accident.Add(new PropertyTitleValuePair()
                        {
                            PropertyName = propName,
                            Title = pairsMajorAccident.Where(x => x.PropertyName == propName ).Select(x => x.Title).FirstOrDefault(),
                            Value = pairsMajorAccident.Where(x => x.PropertyName == propName + "Remarks").Select(x => x.Value).FirstOrDefault()
                        });
                    }
                }
                else
                {
                    var inspectionFormEqualEnumerationAct = await GetInspectionFormLabourPartIIIEqualEnumerationAct(inspectionRefId);
                    var pairsEnumerationAct = ModelOps.GetTitleAndValues<Inspection_Form_Labour_Part_III_EqualEnumerationAct>(inspectionFormEqualEnumerationAct.FormModel);
                    var propNames = pairsEnumerationAct.Where(x => x.PropertyName.EndsWith("ViolationExist") && x.Value == "HAS_VIOLATION").Select(x => x.PropertyName.Replace("_ViolationExist", "")).ToArray();
                    pairs.pairs_EnumerationAct = new List<PropertyTitleValuePair>();
                    foreach (var propName in propNames)
                    {
                        pairs.pairs_EnumerationAct.Add(new PropertyTitleValuePair()
                        {
                            PropertyName = propName,
                            Title = pairsEnumerationAct.Where(x => x.PropertyName == propName).Select(x => x.Title).FirstOrDefault(),
                            Value = pairsEnumerationAct.Where(x => x.PropertyName == propName + "_Remarks").Select(x => x.Value).FirstOrDefault()
                        });
                    }

                    var inspectionFormLabourIIIMinimumWageAct = await GetInspectionFormLabourPartIIIMinimumWagesAct(inspectionRefId);
                    var pairsMinimumWageAct = ModelOps.GetTitleAndValues<Inspection_Form_Labour_III_MinimumWageAct>(inspectionFormLabourIIIMinimumWageAct.FormModel);
                    var propNamesMinimumWageAct = pairsMinimumWageAct.Where(x => x.PropertyName.EndsWith("ViolationExist") && x.Value == "HAS_VIOLATION").Select(x => x.PropertyName.Replace("_ViolationExist", "")).ToArray();
                    pairs.pairs_MinimumWageAct = new List<PropertyTitleValuePair>();
                    foreach (var propName in propNamesMinimumWageAct)
                    {
                        pairs.pairs_MinimumWageAct.Add(new PropertyTitleValuePair()
                        {
                            PropertyName = propName,
                            Title = pairsMinimumWageAct.Where(x => x.PropertyName == propName ).Select(x => x.Title).FirstOrDefault(),
                            Value = pairsMinimumWageAct.Where(x => x.PropertyName == propName + "_Remarks").Select(x => x.Value).FirstOrDefault()
                        });
                    }


                    var inspectionFormLabourIIIPaymentWagesAct = await GetInspectionFormLabourPartIIIPaymentWagesAct(inspectionRefId);
                    var pairsPaymentWagesAct = ModelOps.GetTitleAndValues<Inspection_Form_Labour_III_PaymentWagesAct>(inspectionFormLabourIIIPaymentWagesAct.FormModel);
                    var propNamesPaymentWagesAct = pairsPaymentWagesAct.Where(x => x.PropertyName.EndsWith("ViolationExist") && x.Value == "HAS_VIOLATION").Select(x => x.PropertyName.Replace("_ViolationExist", "")).ToArray();
                    pairs.pairs_PaymentWagesAct = new List<PropertyTitleValuePair>();
                    foreach (var propName in propNamesPaymentWagesAct)
                    {
                        pairs.pairs_PaymentWagesAct.Add(new PropertyTitleValuePair()
                        {
                            PropertyName = propName,
                            Title = pairsPaymentWagesAct.Where(x => x.PropertyName == propName ).Select(x => x.Title).FirstOrDefault(),
                            Value = pairsPaymentWagesAct.Where(x => x.PropertyName == propName + "_Remarks").Select(x => x.Value).FirstOrDefault()
                        });
                    }

                    var inspectionFormLabourIIIPaymentBonusActStatutoryReport = await GetInspectionFormLabourPartIIIPaymentBonusAct_StatutoryReport(inspectionRefId);
                    var pairsPaymentBonusActStatutoryReport = ModelOps.GetTitleAndValues<Inspection_Form_Labour_III_PaymentBonusAct_StatutoryReport>(inspectionFormLabourIIIPaymentBonusActStatutoryReport.FormModel);
                    var propNamesPaymentBonusActStatutoryReport = pairsPaymentBonusActStatutoryReport.Where(x => x.PropertyName.EndsWith("ViolationExist") && x.Value == "HAS_VIOLATION").Select(x => x.PropertyName.Replace("_ViolationExist", "")).ToArray();
                    pairs.pairs_StatutoryReport = new List<PropertyTitleValuePair>();
                    foreach (var propName in propNamesPaymentBonusActStatutoryReport)
                    {
                        pairs.pairs_StatutoryReport.Add(new PropertyTitleValuePair()
                        {
                            PropertyName = propName,
                            Title = pairsPaymentBonusActStatutoryReport.Where(x => x.PropertyName == propName ).Select(x => x.Title).FirstOrDefault(),
                            Value = pairsPaymentBonusActStatutoryReport.Where(x => x.PropertyName == propName + "_Remarks").Select(x => x.Value).FirstOrDefault()
                        });
                    }

                    var inspectionFormLabourIIIChildAndAdolescentLabourAct = await GetInspectionFormLabourPartIIIChildAndAdolescentLabourAct(inspectionRefId);
                    var pairsPaymentChildAndAdolescentLabourAct = ModelOps.GetTitleAndValues<Inspection_Form_Labour_III_ChildAndAdolescentLabourAct>(inspectionFormLabourIIIChildAndAdolescentLabourAct.FormModel);
                    var propNamesPaymentChildAndAdolescentLabourAct = pairsPaymentChildAndAdolescentLabourAct.Where(x => x.PropertyName.EndsWith("ViolationExist") && x.Value == "HAS_VIOLATION").Select(x => x.PropertyName.Replace("_ViolationExist", "")).ToArray();
                    pairs.pairs_AdolescentLabourAct = new List<PropertyTitleValuePair>();
                    foreach (var propName in propNamesPaymentChildAndAdolescentLabourAct)
                    {
                        pairs.pairs_AdolescentLabourAct.Add(new PropertyTitleValuePair()
                        {
                            PropertyName = propName,
                            Title = pairsPaymentChildAndAdolescentLabourAct.Where(x => x.PropertyName == propName).Select(x => x.Title).FirstOrDefault(),
                            Value = pairsPaymentChildAndAdolescentLabourAct.Where(x => x.PropertyName == propName + "_Remarks").Select(x => x.Value).FirstOrDefault()
                        });
                    }

                    var inspectionFormLabourIIINationalAndFestivalHolidays = await GetInspectionFormLabourPartIIINationalAndFestivalHolidays(inspectionRefId);
                    var pairsNationalAndFestivalHolidays = ModelOps.GetTitleAndValues<Inspection_Form_Labour_III_NationalAndFestivalHolidays>(inspectionFormLabourIIINationalAndFestivalHolidays.FormModel);
                    var propNamesNationalAndFestivalHolidays = pairsNationalAndFestivalHolidays.Where(x => x.PropertyName.EndsWith("ViolationExist") && x.Value == "HAS_VIOLATION").Select(x => x.PropertyName.Replace("_ViolationExist", "")).ToArray();
                    pairs.pairs_FestivalHolidays = new List<PropertyTitleValuePair>();
                    foreach (var propName in propNamesNationalAndFestivalHolidays)
                    {
                        pairs.pairs_FestivalHolidays.Add(new PropertyTitleValuePair()
                        {
                            PropertyName = propName,
                            Title = pairsNationalAndFestivalHolidays.Where(x => x.PropertyName == propName ).Select(x => x.Title).FirstOrDefault(),
                            Value = pairsNationalAndFestivalHolidays.Where(x => x.PropertyName == propName + "_Remarks").Select(x => x.Value).FirstOrDefault()
                        });
                    }

                    var inspectionFormLabourIIIMaternityBenefitAct = await GetInspectionFormLabourPartIIIMaternityBenefitAct(inspectionRefId);
                    var pairsMaternityBenefitAct = ModelOps.GetTitleAndValues<Inspection_Form_Labour_III_MaternityBenefitAct>(inspectionFormLabourIIIMaternityBenefitAct.FormModel);
                    var propNamesMaternityBenefitAct = pairsMaternityBenefitAct.Where(x => x.PropertyName.EndsWith("ViolationExist") && x.Value == "HAS_VIOLATION").Select(x => x.PropertyName.Replace("_ViolationExist", "")).ToArray();
                    pairs.pairs_MaternityBenefitAct = new List<PropertyTitleValuePair>();
                    foreach (var propName in propNamesMaternityBenefitAct)
                    {
                        pairs.pairs_MaternityBenefitAct.Add(new PropertyTitleValuePair()
                        {
                            PropertyName = propName,
                            Title = pairsMaternityBenefitAct.Where(x => x.PropertyName == propName).Select(x => x.Title).FirstOrDefault(),
                            Value = pairsMaternityBenefitAct.Where(x => x.PropertyName == propName + "_Remarks").Select(x => x.Value).FirstOrDefault()
                        });
                    }

                    var inspectionFormLabourIIIContractLabourAct = await GetInspectionFormLabourPartIIIContractLabourAct(inspectionRefId);
                    var pairsContractLabourAct = ModelOps.GetTitleAndValues<Inspection_Form_Labour_III_ContractLabourAct>(inspectionFormLabourIIIContractLabourAct.FormModel);
                    var propNamesContractLabourAct = pairsContractLabourAct.Where(x => x.PropertyName.EndsWith("ViolationExist") && x.Value == "HAS_VIOLATION").Select(x => x.PropertyName.Replace("_ViolationExist", "")).ToArray();
                    pairs.pairs_ContractLabourAct = new List<PropertyTitleValuePair>();
                    foreach (var propName in propNamesContractLabourAct)
                    {
                        pairs.pairs_ContractLabourAct.Add(new PropertyTitleValuePair()
                        {
                            PropertyName = propName,
                            Title = pairsContractLabourAct.Where(x => x.PropertyName == propName ).Select(x => x.Title).FirstOrDefault(),
                            Value = pairsContractLabourAct.Where(x => x.PropertyName == propName + "_Remarks").Select(x => x.Value).FirstOrDefault()
                        });
                    }

                    var inspectionFormLabourIIIInterStateMigrantWorkmenAct = await GetInspectionFormLabourPartIIIInterStateMigrantWorkmenAct(inspectionRefId);
                    var pairsInterStateMigrantWorkmenAct = ModelOps.GetTitleAndValues<Inspection_Form_Labour_III_InterStateMigrantWorkmenAct>(inspectionFormLabourIIIInterStateMigrantWorkmenAct.FormModel);
                    var propNamesInterStateMigrantWorkmenAct = pairsInterStateMigrantWorkmenAct.Where(x => x.PropertyName.EndsWith("ViolationExist") && x.Value == "HAS_VIOLATION").Select(x => x.PropertyName.Replace("_ViolationExist", "")).ToArray();
                    pairs.pairs_MigrantWorkmenAct = new List<PropertyTitleValuePair>();
                    foreach (var propName in propNamesInterStateMigrantWorkmenAct)
                    {
                        pairs.pairs_MigrantWorkmenAct.Add(new PropertyTitleValuePair()
                        {
                            PropertyName = propName,
                            Title = pairsInterStateMigrantWorkmenAct.Where(x => x.PropertyName == propName ).Select(x => x.Title).FirstOrDefault(),
                            Value = pairsInterStateMigrantWorkmenAct.Where(x => x.PropertyName == propName + "Remarks").Select(x => x.Value).FirstOrDefault()
                        });
                    }

                    var inspectionFormLabourIIILabourWelfareFundAct = await GetInspectionFormLabourPartIIILabourWelfareFund_Act(inspectionRefId);
                    var pairsLabourWelfareFundAct = ModelOps.GetTitleAndValues<Inspection_Form_Labour_III_LabourWelfareFund_Act>(inspectionFormLabourIIILabourWelfareFundAct.FormModel);
                    var propNamesLabourWelfareFundAct = pairsLabourWelfareFundAct.Where(x => x.PropertyName.EndsWith("ViolationExist") && x.Value == "HAS_VIOLATION").Select(x => x.PropertyName.Replace("_ViolationExist", "")).ToArray();
                    pairs.pairs_WelfareFundAct = new List<PropertyTitleValuePair>();
                    foreach (var propName in propNamesLabourWelfareFundAct)
                    {
                        pairs.pairs_WelfareFundAct.Add(new PropertyTitleValuePair()
                        {
                            PropertyName = propName,
                            Title = pairsLabourWelfareFundAct.Where(x => x.PropertyName == propName ).Select(x => x.Title).FirstOrDefault(),
                            Value = pairsLabourWelfareFundAct.Where(x => x.PropertyName == propName + "_Remarks").Select(x => x.Value).FirstOrDefault()
                        });
                    }

                    var inspectionFormLabourIIIGratuityAct = await GetInspectionFormLabourPartIIIGratuityAct(inspectionRefId);
                    var pairsGratuityAct = ModelOps.GetTitleAndValues<Inspection_Form_Labour_III_GratuityAct>(inspectionFormLabourIIIGratuityAct.FormModel);
                    var propNamesGratuityAct = pairsGratuityAct.Where(x => x.PropertyName.EndsWith("ViolationExist") && x.Value == "HAS_VIOLATION").Select(x => x.PropertyName.Replace("_ViolationExist", "")).ToArray();
                    pairs.pairs_GratuityAct = new List<PropertyTitleValuePair>();
                    foreach (var propName in propNamesGratuityAct)
                    {
                        pairs.pairs_GratuityAct.Add(new PropertyTitleValuePair()
                        {
                            PropertyName = propName,
                            Title = pairsGratuityAct.Where(x => x.PropertyName == propName ).Select(x => x.Title).FirstOrDefault(),
                            Value = pairsGratuityAct.Where(x => x.PropertyName == propName + "_Remarks").Select(x => x.Value).FirstOrDefault()
                        });
                    }

                    var inspectionFormLabourIIIIndustrialEmploymentAct = await GetInspectionFormLabourPartIIIIndustrialEmploymentAct(inspectionRefId);
                    var pairsIndustrialEmploymentAct = ModelOps.GetTitleAndValues<Inspection_Form_Labour_III_IndustrialEmploymentAct>(inspectionFormLabourIIIIndustrialEmploymentAct.FormModel);
                    var propIndustrialEmploymentAct = pairsIndustrialEmploymentAct.Where(x => x.PropertyName.EndsWith("ViolationExist") && x.Value == "HAS_VIOLATION").Select(x => x.PropertyName.Replace("_ViolationExist", "")).ToArray();
                    pairs.pairs_IndustrialEmploymentAct = new List<PropertyTitleValuePair>();
                    foreach (var propName in propIndustrialEmploymentAct)
                    {
                        pairs.pairs_IndustrialEmploymentAct.Add(new PropertyTitleValuePair()
                        {
                            PropertyName = propName,
                            Title = pairsIndustrialEmploymentAct.Where(x => x.PropertyName == propName ).Select(x => x.Title).FirstOrDefault(),
                            Value = pairsIndustrialEmploymentAct.Where(x => x.PropertyName == propName + "_Remarks").Select(x => x.Value).FirstOrDefault()
                        });
                    }

                    var inspectionFormBOCWAct = await GetInspectionFormLabourPartIIIBOCW_Act(inspectionRefId);
                    var pairsBOCWAct = ModelOps.GetTitleAndValues<Inspection_Form_Labour_III_BOCW_Act>(inspectionFormBOCWAct.FormModel);
                    var propBOCWAct = pairsBOCWAct.Where(x => x.PropertyName.EndsWith("ViolationExist") && x.Value == "HAS_VIOLATION").Select(x => x.PropertyName.Replace("_ViolationExist", "")).ToArray();
                    pairs.pairs_BOCWAct = new List<PropertyTitleValuePair>();
                    foreach (var propName in propBOCWAct)
                    {
                        pairs.pairs_BOCWAct.Add(new PropertyTitleValuePair()
                        {
                            PropertyName = propName,
                            Title = pairsBOCWAct.Where(x => x.PropertyName == propName ).Select(x => x.Title).FirstOrDefault(),
                            Value = pairsBOCWAct.Where(x => x.PropertyName == propName + "_Remarks").Select(x => x.Value).FirstOrDefault()
                        });
                    }

                    var inspectionFormObservationsAct = await GetInspectionFormLabourPartIIIObservations(inspectionRefId);
                    var pairsObservationsAct = ModelOps.GetTitleAndValues<Inspection_Form_Labour_III_Observations>(inspectionFormObservationsAct.FormModel);
                    var propObservationsAct = pairsObservationsAct.Where(x => x.PropertyName.EndsWith("ViolationExist") && x.Value == "HAS_VIOLATION").Select(x => x.PropertyName.Replace("_ViolationExist", "")).ToArray();
                    pairs.pairs_ObservationsAct = new List<PropertyTitleValuePair>();
                    foreach (var propName in propObservationsAct)
                    {
                        pairs.pairs_ObservationsAct.Add(new PropertyTitleValuePair()
                        {
                            PropertyName = propName,
                            Title = pairsObservationsAct.Where(x => x.PropertyName == propName).Select(x => x.Title).FirstOrDefault(),
                            Value = pairsObservationsAct.Where(x => x.PropertyName == propName + "_Remarks").Select(x => x.Value).FirstOrDefault()
                        });
                    }
                }
                genericFormModel.ResponseDataModel = pairs;
            }
            catch (Exception ex)
            {
                genericFormModel.HasError = true;
                genericFormModel.ErrorDesc = ex.Message;
            }
            return genericFormModel;
        }

        public async Task<GenericFormModel<List<RoleWiseAllowedActionCodeViewModel>>> GetProcessInspectionDetail(string userId, int currentActionCode, int inspectionType)
        {
            GenericFormModel<List<RoleWiseAllowedActionCodeViewModel>> genericFormModel = new GenericFormModel<List<RoleWiseAllowedActionCodeViewModel>>();
            try
            {
                List<StoreProcedureParm> storeProcedureParms = new List<StoreProcedureParm>()
                {
                    new StoreProcedureParm (){ ParmName="UserId", ParmValue=userId.ToString(), isNumber=false},
                    new StoreProcedureParm (){ ParmName="CurrentActionCode", ParmValue=currentActionCode.ToString(), isNumber=true},
                    new StoreProcedureParm (){ ParmName="InspectionType", ParmValue = inspectionType.ToString(), isNumber=true}
                };
                genericFormModel.FormModel = await _iGeneric_SP_Repository.CallStoreProcedureReaderAsync<RoleWiseAllowedActionCodeViewModel>("sp_Inspection_RoleWiseAllowedActionCodes", storeProcedureParms);
            }
            catch (Exception ex)
            {
                genericFormModel.HasError = true;
                genericFormModel.ErrorDesc = ex.Message;
            }
            return genericFormModel;
        }

        public async Task<GenericFormModel<List<ProcessApplicationUsersViewModel>>> GetUserByInspectionActionCode(int actionCode, string userId, Int64 inspectionRefId, int inspectionType)
        {
            GenericFormModel<List<ProcessApplicationUsersViewModel>> genericFormModel = new GenericFormModel<List<ProcessApplicationUsersViewModel>>();
            try
            {
                List<StoreProcedureParm> storeProcedureParms = new List<StoreProcedureParm>()
                {
                    new StoreProcedureParm (){ ParmName="ActionCode", ParmValue=actionCode.ToString(), isNumber=true},
                    new StoreProcedureParm (){ ParmName="UserId", ParmValue=userId.ToString(), isNumber=false},
                    new StoreProcedureParm (){ ParmName="InspectionRefId", ParmValue=inspectionRefId.ToString(), isNumber=true},
                    new StoreProcedureParm (){ ParmName="InspectionType", ParmValue=EnumOps.GetEnumValue<CircleTypeEnum>(inspectionType.ToString()), isNumber=true}
                };
                genericFormModel.FormModel = await _iGeneric_SP_Repository.CallStoreProcedureReaderAsync<ProcessApplicationUsersViewModel>("sp_Inspection_GetNextUserByAction", storeProcedureParms);
            }
            catch (Exception ex)
            {
                genericFormModel.HasError = true;
                genericFormModel.ErrorDesc = ex.Message;
            }
            return genericFormModel;
        }

        public async Task<GenericResponseTemplateModel<RecordActionResponseViewModel>> RecordInspectionAction(InspectionActionViewModel formModel, string userId)
        {
            GenericResponseTemplateModel<RecordActionResponseViewModel> resp = new GenericResponseTemplateModel<RecordActionResponseViewModel>();
            try
            {
                var inspectionAction = await _context.InspectionComplianceLogs.Where(x => x.InspectionRefId == formModel.InspectionRefId).OrderByDescending(x => x.InspectionComplianceLogId).FirstOrDefaultAsync();
                List<StoreProcedureParm> storeProcedureParms = new List<StoreProcedureParm>()
                {
                    new StoreProcedureParm (){ ParmName="UserId", ParmValue= formModel.Receiver_UserRefId + "," + userId, isNumber=false }
                };
                var user = await _iGeneric_SP_Repository.CallStoreProcedureReaderAsync<SenderReceiverDetailViewModal>("dbo.sp_GetSenderReceiverDetail", storeProcedureParms);

                int totalDays = GetWorkingDays(inspectionAction.ActionDate, DateTimeOffset.UtcNow.Date);
                int holidays = _context.Holidays.Where(x => x.HolidayDate >= inspectionAction.ActionDate && x.HolidayDate <= DateTimeOffset.UtcNow.Date && x.IsWeekend == false).Count();
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
                var start = inspectionAction.ActionDate;
                var weekend = new[] { DayOfWeek.Saturday, DayOfWeek.Sunday };

                //var totalWorkingHours = Enumerable.Range(0, Convert.ToInt32(end.Subtract(start).TotalHours))
                //     .Count(offset => !weekend.Contains(start.AddHours(offset).DayOfWeek));
                var totalWorkingHours = Enumerable.Range(0, Convert.ToInt32(end.Subtract(start).TotalHours))
                     .Count(offset => true);  



                InspectionComplianceLog inspectionComplianceLogs = new InspectionComplianceLog()
                {
                    ActionTakenDaysCount = totalWorkingDays,
                    ActionTakenHoursCount = Convert.ToInt64(totalWorkingHours),
                    ActionDate = DateTime.Now,
                    AppActionType = formModel.AppActionType,

                    Receiver_ProfileRefId = user.Where(x => x.UserId == formModel.Receiver_UserRefId).Select(x => x.UserProfileId).FirstOrDefault(), //user.Where(x => x.UserId == formModel.UserId).Select(x => x.UserProfileId).FirstOrDefault();
                    Receiver_UserRefId = formModel.Receiver_UserRefId,

                    Sender_ProfileRefId = user.Where(x => x.UserId == userId).Select(x => x.UserProfileId).FirstOrDefault(),
                    Sender_UserRefId = userId,
                    Remarks = formModel.Remarks,
                    IsDocumentUploaded = formModel.IsDocumentUploaded,
                    AppDocumentRefId = formModel.AppDocumentRefId,

                    ReceiverRoleId = user.Where(x => x.UserId == formModel.Receiver_UserRefId).Select(x => x.RoleId).FirstOrDefault(),
                    SenderRoleId = user.Where(x => x.UserId.ToLower() == userId.ToLower()).Select(x => x.RoleId).FirstOrDefault(),
                    IpAddress = formModel.IpAddress.ToString(),
                    Latitude = formModel.Latitude.ToString(),
                    Longitude = formModel.Longitude.ToString(),
                    InspectionRefId = formModel.InspectionRefId,
                    InspectionType = formModel.InspectionType

                };
                    _iGR_InspectionComplianceLog.Insert(inspectionComplianceLogs);
                    await _iGR_InspectionComplianceLog.SavechangeAsync();

                if (formModel.AppActionType == 404) 
                {
                    Inspection_ViolationComplianceReminder inspection_ViolationComplianceReminder = new Inspection_ViolationComplianceReminder();
                    {
                        inspection_ViolationComplianceReminder.StartDate = DateTime.Now;
                        inspection_ViolationComplianceReminder.EndDate = DateTime.Now.AddDays(formModel.AllowedDays);
                        inspection_ViolationComplianceReminder.TimeInDays = formModel.AllowedDays;
                        inspection_ViolationComplianceReminder.TotalRemindersToBeSend = 1;
                        inspection_ViolationComplianceReminder.TotalRemindersSent = 0;
                        inspection_ViolationComplianceReminder.ReminderStatus = 1;
                        inspection_ViolationComplianceReminder.InspectionType = formModel.InspectionType;
                        inspection_ViolationComplianceReminder.InspectionRefId = formModel.InspectionRefId;
                    }
                    _iGR_Inspection_ViolationComplianceReminder.Insert(inspection_ViolationComplianceReminder);
                    await _iGR_Inspection_ViolationComplianceReminder.SavechangeAsync();
                }

                if (formModel.AppActionType == 2 || formModel.AppActionType == 6)
                {
                    var inspection_ViolationComplianceReminder = _context.Inspection_ViolationComplianceReminders.Where(x => x.InspectionRefId == formModel.InspectionRefId).ToList();
                    foreach (var data in inspection_ViolationComplianceReminder)
                    {
                        data.ReminderStatus = 0;
                    }
                    _context.SaveChangesAsync();
                    
                }



                bool allOk = true;
                //Handle Approvals and rejectionss
                if (allOk)
                {

                    if (formModel.AppDocumentRefId > 0)
                    {
                        var appDocument = _context.InspectionDocuments.Where(x => x.AppDocId == formModel.AppDocumentRefId).FirstOrDefault();
                        appDocument.IsLocked = true;
                        _context.InspectionDocuments.Update(appDocument);
                        await _context.SaveChangesAsync();
                    }


                    resp.HasError = false;
                }
            }
            catch (Exception ex)
            {
                //transaction.Rollback();
                resp.HasError = true;
                resp.ErrorDesc = ex.Message;
                throw ex;
            }
            //}
            return resp;
        }

        public async Task<GenericResponseTemplateModel<List<GetInspectionNotingLogsViewModel>>> GetInspectionLogsByInspectionId(Int64 inspectionRefId, int inspectionType)
        {
            GenericResponseTemplateModel<List<GetInspectionNotingLogsViewModel>> genericServiceResultTemplate = new GenericResponseTemplateModel<List<GetInspectionNotingLogsViewModel>>();
            try
            {
                List<StoreProcedureParm> storeProcedureParms = new List<StoreProcedureParm>()
                {
                        new StoreProcedureParm() { ParmName = "InspectionRefId", ParmValue = inspectionRefId.ToString(), isNumber = true },
                        new StoreProcedureParm() { ParmName = "InspectionType", ParmValue = inspectionType.ToString(), isNumber = true },
                };
                var NotingLogs = await _iGeneric_SP_Repository.CallStoreProcedureReaderAsync<GetInspectionNotingLogsViewModel>("sp_GetInspectionNotingLogsByInspectionId", storeProcedureParms);
                genericServiceResultTemplate.ResponseDataModel = NotingLogs;
            }

            catch (Exception ex)
            {
                genericServiceResultTemplate.HasError = true;
                genericServiceResultTemplate.ErrorDesc = ex.Message;
            }
            return genericServiceResultTemplate;
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

        public async Task<List<AppFileUploadInfoViewModel>> InitiateInspectionFileInfo(Int64 inspectionId, bool deleteTempFiles, int inspectionType)
        {
            List<AppFileUploadInfoViewModel> filesDetail = new List<AppFileUploadInfoViewModel>();

            try
            {
                
                filesDetail = await InitiateInspectionFileInfo(inspectionId, inspectionType);
                
            }
            catch (Exception ex)
            {

            }
            return filesDetail;
        }

        public async Task<List<AppFileUploadInfoViewModel>> InitiateInspectionFileInfo(long inspectionId, int inspectionType)
        {
            List<AppFileUploadInfoViewModel> filesDetail = new List<AppFileUploadInfoViewModel>();
            try
            {
                List<StoreProcedureParm> storeProcedureParms = new List<StoreProcedureParm>()
                {
                    new StoreProcedureParm (){ ParmName="InspectionId", ParmValue= inspectionId.ToString(), isNumber=true },
                    new StoreProcedureParm (){ ParmName="InspectionType", ParmValue= inspectionType.ToString(), isNumber=true },

                };
                //filesDetail = await _iGeneric_SP_Repository.CallStoreProcedureReaderAsync<AppFileUploadInfoViewModel>("dbo.sp_Get_AppType_Allowed_File_Files", storeProcedureParms);
                filesDetail = await _iGeneric_SP_Repository.CallStoreProcedureReaderAsync<AppFileUploadInfoViewModel>("dbo.sp_Get_InspectionType_Allowed_File_Files", storeProcedureParms);

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
                filesDetail = filesDetail.OrderBy(x => x.IsOptional).ToList();
            }
            catch (Exception ex)
            {

            }
            return filesDetail;
        }

        public async Task<List<AppFileUploadInfoViewModel>> InitiateInspectionFileInfoWithRoleId(Int64 inspectionId, bool deleteTempFiles, string userId, int currentActionCode, int allowedActionCode, Int64 inspectionType)
        {
            List<AppFileUploadInfoViewModel> filesDetail = new List<AppFileUploadInfoViewModel>();

            try
            {
                //1. Delete existing temp files
                if (deleteTempFiles)
                {
                    var tempFilePath = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "TempFileUpload")).Root;
                    DirectoryInfo dir = new DirectoryInfo(tempFilePath);
                    foreach (FileInfo fi in dir.GetFiles(inspectionId + "_*"))
                    {
                        fi.Delete();
                    }
                }
                //2. Initiate already uploaded files
                filesDetail = await InitiateAppFileInfoWithRoleId(inspectionId, userId, currentActionCode, allowedActionCode, inspectionType);
            }
            catch (Exception ex)
            {

            }
            return filesDetail;
        }

        public async Task<List<AppFileUploadInfoViewModel>> InitiateAppFileInfoWithRoleId(long inspectionId, string userId, int currentActionCode, int allowedActionCode, Int64 inspectionType)
        {
            List<AppFileUploadInfoViewModel> filesDetail = new List<AppFileUploadInfoViewModel>();
            try
            {
                List<StoreProcedureParm> storeProcedureParms = new List<StoreProcedureParm>()
                {
                    new StoreProcedureParm (){ ParmName="InspectionId", ParmValue= inspectionId.ToString(), isNumber=true },
                    new StoreProcedureParm (){ ParmName="UserId", ParmValue= userId.ToString(), isNumber=false },
                    new StoreProcedureParm (){ ParmName="CurrentActionCode", ParmValue= currentActionCode.ToString(), isNumber=true},
                    new StoreProcedureParm (){ ParmName="AllowedActionCode", ParmValue= allowedActionCode.ToString(), isNumber=true},
                    new StoreProcedureParm (){ ParmName="inspectionType", ParmValue= inspectionType.ToString(), isNumber=true}
                };
                filesDetail = await _iGeneric_SP_Repository.CallStoreProcedureReaderAsync<AppFileUploadInfoViewModel>("dbo.sp_InitiateInspectionFileInfoWithRoleId", storeProcedureParms);

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

        public async Task<GenericResponseTemplateModel<FileUploadResponse>> LogFileInfo(Int64 inspectionId, Int64 DocId, Int64 appDocId, string fileName, int inspectionType)
        {
            GenericResponseTemplateModel<FileUploadResponse> genericResponseTemplate = new GenericResponseTemplateModel<FileUploadResponse>() { ErrorDesc = null, HasError = false };
            try
            {

                if (appDocId != 0)
                {
                    var foundDocument = _iGR_InspectionDocument.GetById(appDocId);
                    foundDocument.LastModifiedDate = DateTime.Now;

                    _iGR_InspectionDocument.Update(foundDocument);
                    await _iGR_InspectionDocument.SavechangeAsync();
                    genericResponseTemplate.ResponseDataModel = new FileUploadResponse()
                    {
                        FileName = foundDocument.AttachmentName,
                        Id = foundDocument.AppDocId
                    };
                }
                else
                {
                    InspectionDocument inspectionDocument = new InspectionDocument()
                    {
                        InspectionRefId = inspectionId,
                        AttachmentName = fileName,
                        DocumentRefId = DocId,
                        IsUploaded = true,
                        LastModifiedDate = DateTime.Now,
                        Uploadeddate = DateTime.Now,
                        InspectionType = inspectionType
                    };
                    _iGR_InspectionDocument.Insert(inspectionDocument);
                    await _iGR_InspectionDocument.SavechangeAsync();


                    fileName = inspectionId + "_" + DocId + "_" + inspectionType + "_" +  inspectionDocument.AppDocId + fileName;
                    var foundDocument = _iGR_InspectionDocument.GetById(inspectionDocument.AppDocId);
                    foundDocument.AttachmentName = fileName;

                    _iGR_InspectionDocument.Update(foundDocument);
                    await _iGR_InspectionDocument.SavechangeAsync();


                    genericResponseTemplate.ResponseDataModel = new FileUploadResponse()
                    {
                        FileName = foundDocument.AttachmentName,
                        Id = inspectionDocument.AppDocId
                    };
                }
            }
            catch (Exception ex)
            {
                genericResponseTemplate.ResponseDataModel = new FileUploadResponse()
                {
                    FileName = "",
                    Id = 0
                };
                genericResponseTemplate.HasError = true;
                genericResponseTemplate.ErrorDesc = ex.Message;
                return genericResponseTemplate;
            }
            return genericResponseTemplate;
        }

        public async Task<bool> LockAppDocFiles(Int64 inspectionId, Int64[] appDocIds)
        {
            try
            {
                var parentObject = await _iGR_InspectionDocument.GetAsync(x => appDocIds.Contains(x.AppDocId)).ConfigureAwait(false);
                var appDocs = parentObject.Select(x => { x.IsLocked = true; return x; }).ToList();
                await _context.BulkUpdateAsync<InspectionDocument>(appDocs);

                parentObject = await _iGR_InspectionDocument.GetAsync(x => x.InspectionRefId == inspectionId && !x.IsLocked).ConfigureAwait(false);
                await _context.BulkDeleteAsync<InspectionDocument>(parentObject.ToList());
            }
            catch (Exception ex)
            {
                return false;
            }
            return true;
        }

        public async Task<GenericServiceResultTemplate> Insert_complianceLogs(int inspectionType)
        {
            GenericServiceResultTemplate genericServiceResultTemplate = new GenericServiceResultTemplate();
            genericServiceResultTemplate.HasException = false;
            try
            {
                var violationCount = 0;
                if (inspectionType == 1)
                {
                    var excludedInspectionIds = _context.InspectionComplianceLogs
                                                        .Where(log => log.InspectionType == 1)
                                                        .Select(log => log.InspectionRefId)
                                                        .ToList();

                    var factoryInspections = _context.Inspection_Master.Where(x => x.IsSubmitted_Factory_Wing == true && x.InspectionSubmited_Factory_Wing_FactoryExistenceType == InspectionFactoryExistenceTypeEnum.OPEN && x.Inspection_Randomization.Month > 8 && x.Inspection_Randomization.Year == 2024 && !excludedInspectionIds.Contains(x.InspectionId)).Include(x => x.Inspection_Randomization).Select(x => x.InspectionId).ToList();


                    foreach (var inspection in factoryInspections)
                    {
                        Inspection_Master inspection_Master = _context.Inspection_Master.Where(x => x.InspectionId == inspection).FirstOrDefault();

                        var userDetails = await _iAuthService.GetUserRoleAndProfileDetailByUserId(inspection_Master.UserId);
                        var officerRole = await _iAuthService.GetUserRoleByRoleId(userDetails.RoleId);
                        var inspectionFormHealthData = await GetIncpectionFormFactoryPartIIIHealth(inspection);
                        var pairsHealth = ModelOps.GetTitleAndValues<Inspection_Form_Factory_Part_III_Health>(inspectionFormHealthData.FormModel);
                        violationCount = pairsHealth.Where(x => x.PropertyName.EndsWith("ViolationExist") && x.Value == "HAS_VIOLATION").Count();
                        if (violationCount == 0)
                        {
                            var inspectionFormSafetyData = await GetIncpectionFormFactoryPartIIISafety(inspection);
                            var pairsSafety = ModelOps.GetTitleAndValues<Inspection_Form_Factory_Part_III_Safety>(inspectionFormSafetyData.FormModel);
                            violationCount = pairsSafety.Where(x => x.PropertyName.EndsWith("ViolationExist") && x.Value == "HAS_VIOLATION").Count();
                        }
                        if (violationCount == 0)
                        {
                            var inspectionFormWelfareData = await GetIncpectionGetFormFactoryPartIIIWelfare(inspection);
                            var pairsWelfare = ModelOps.GetTitleAndValues<Inspection_Form_Factory_Part_III_Welfare>(inspectionFormWelfareData.FormModel);
                            violationCount = pairsWelfare.Where(x => x.PropertyName.EndsWith("ViolationExist") && x.Value == "HAS_VIOLATION").Count();
                        }
                        if (violationCount == 0)
                        {
                            var inspectionFormGeneralData = await GetIncpectionGetFormFactoryPartIIIGeneral(inspection);
                            var pairsGeneral = ModelOps.GetTitleAndValues<Inspection_Form_Factory_Part_III_General>(inspectionFormGeneralData.FormModel);
                            violationCount = pairsGeneral.Where(x => x.PropertyName.EndsWith("ViolationExist") && x.Value == "HAS_VIOLATION").Count();
                        }
                        if (violationCount == 0)
                        {
                            var inspectionFormMajorAccidentData = await GetIncpectionGetFormFactoryPartIIIMajorAccidentHazard(inspection);
                            var pairsMajorAccident = ModelOps.GetTitleAndValues<Inspection_Form_Factory_Part_III_MajorAccidentHazard>(inspectionFormMajorAccidentData.FormModel);
                            violationCount = pairsMajorAccident.Where(x => x.PropertyName.EndsWith("ViolationExist") && x.Value == "HAS_VIOLATION").Count();
                        }

                        var userRefId = _context.InspectionUserMappings.Where(x => x.LicenceNumber == inspection_Master.LicenceNumber).Select(x => x.UserRefId).FirstOrDefault();
                        if (violationCount > 0)
                        {
                            var receiverInfo = await _iAuthService.GetUserProfileByUserId(userRefId);

                            InspectionComplianceLog inspectionComplianceLog = new InspectionComplianceLog();
                            {
                                inspectionComplianceLog.AppActionType = 1;
                                inspectionComplianceLog.Sender_UserRefId = userDetails.UserId;
                                inspectionComplianceLog.Sender_ProfileRefId = userDetails.UserProfileId;
                                inspectionComplianceLog.Receiver_UserRefId = userRefId;
                                inspectionComplianceLog.Receiver_ProfileRefId = receiverInfo == null ? 0 : receiverInfo.UserProfileId;
                                inspectionComplianceLog.ActionDate = DateTime.Now;
                                inspectionComplianceLog.ActionTakenDaysCount = 0;
                                inspectionComplianceLog.ActionTakenHoursCount = 0;
                                inspectionComplianceLog.Remarks = "Violation Report Sent";
                                inspectionComplianceLog.SenderRoleId = userDetails.RoleId;
                                inspectionComplianceLog.ReceiverRoleId = "592add3e-f992-4983-a8ca-21ddc090bda0";
                                inspectionComplianceLog.IsDocumentUploaded = false;
                                inspectionComplianceLog.AppDocumentRefId = 0;
                                inspectionComplianceLog.IpAddress = "";
                                inspectionComplianceLog.Latitude = "";
                                inspectionComplianceLog.Longitude = "";
                                inspectionComplianceLog.InspectionType = 1;
                                inspectionComplianceLog.InspectionRefId = inspection;
                            }
                            _iGR_InspectionComplianceLog.Insert(inspectionComplianceLog);
                            await _iGR_InspectionComplianceLog.SavechangeAsync();

                            Inspection_ViolationComplianceReminder inspection_ViolationComplianceReminder = new Inspection_ViolationComplianceReminder();
                            {
                                inspection_ViolationComplianceReminder.StartDate = DateTime.Now;
                                inspection_ViolationComplianceReminder.EndDate = DateTime.Now.AddDays(30);
                                inspection_ViolationComplianceReminder.TimeInDays = 30;
                                inspection_ViolationComplianceReminder.TotalRemindersToBeSend = 1;
                                inspection_ViolationComplianceReminder.TotalRemindersSent = 0;
                                inspection_ViolationComplianceReminder.ReminderStatus = 1;
                                inspection_ViolationComplianceReminder.InspectionType = 1;
                                inspection_ViolationComplianceReminder.InspectionRefId = inspection;
                            }
                            _iGR_Inspection_ViolationComplianceReminder.Insert(inspection_ViolationComplianceReminder);
                            await _iGR_Inspection_ViolationComplianceReminder.SavechangeAsync();

                            List<StoreProcedureParm> storeProcedureParms = new List<StoreProcedureParm>()
                    {
                        new StoreProcedureParm (){ ParmName="LicenceNo", ParmValue=inspection_Master.LicenceNumber.ToString(), isNumber=false}
                    };
                            var establishmentDetails = await _iGeneric_SP_Repository.CallStoreProcedureReaderAsync<InspectionEstablishmentBasicDetailsViewModel>("sp_GetInspectionEstablishmentBasicDetails", storeProcedureParms);

                            var circleDetails = await _iCircleManager.GetCircleDetailsByCircleId(inspection_Master.FactoryCircleId, CircleTypeEnum.FACTORY_CIRCLE);
                            TemplateInspectionSubmittedNotificationViewModel templateInspectionSubmittedNotificationViewModel = new TemplateInspectionSubmittedNotificationViewModel();
                            templateInspectionSubmittedNotificationViewModel = new TemplateInspectionSubmittedNotificationViewModel()
                            {
                                InspectionType = 1,
                                OfficerName = userDetails.FullName,
                                RoleName = officerRole.NormalizedName,
                                IsViolationFound = violationCount,
                                InspectionDate = inspection_Master.Inspection_Form_Factory_Part_I_General == null ? inspection_Master.InspectionDoneOn_Factory_Wing : inspection_Master.Inspection_Form_Factory_Part_I_General.DateOfInspection,
                                EstablishmentName = establishmentDetails.FirstOrDefault().EstablishmentName,
                                EstablishmentAddress = establishmentDetails.FirstOrDefault().EstablishmentAddress,
                                LicenseNumber = establishmentDetails.FirstOrDefault().LicenceNo,
                                FactoryStatus = inspection_Master.InspectionSubmited_Factory_Wing_FactoryExistenceType.ToString(),
                                FactoryCircle = circleDetails.ResponseDataModel
                            };

                            var emailTemplateText = await _iNotificationManagerService.RenderToStringAsync("EmailTemplates/_templateInspectionSubmittedNotification", templateInspectionSubmittedNotificationViewModel);
                            await _iNotificationManagerService.InitiateNotification(userRefId,
                        new List<NotificationViewModel>()
                        {
                        new NotificationViewModel(){Body=emailTemplateText, NotificationMode=NotificationModeTypeEnum.EMAIL,NotificationPurpose=NotificationPurposeTypeEnum.INFO, Title ="Department of Labour: Inspection submitted(Factory-Wing)"}
                        });
                        }

                        

                    }


                }
                else if (inspectionType == 2)
                {
                    var excludedInspectionIds = _context.InspectionComplianceLogs
                                                       .Where(log => log.InspectionType == 2)
                                                       .Select(log => log.InspectionRefId)
                                                       .ToList();

                                       var labourInspections = _context.Inspection_Master.Where(x => x.IsSubmitted_Labour_Wing == true && x.InspectionSubmitOn_Labour_Wing > new DateTime(2024, 11, 1) && x.InspectionSubmited_Labour_Wing_FactoryExistenceType == InspectionFactoryExistenceTypeEnum.OPEN && x.Inspection_Randomization.Month > 8 && x.Inspection_Randomization.Year == 2024 && !excludedInspectionIds.Contains(x.InspectionId)).Include(x => x.Inspection_Randomization).Select(x => x.InspectionId).OrderByDescending(id => id).ToList();

                   
                    foreach (var inspection in labourInspections)
                    {
                        Inspection_Master inspection_Master = _context.Inspection_Master.Where(x => x.InspectionId == inspection).FirstOrDefault();
                        var labourOfficerId = _context.UserCircleMappings.Where(x => x.LabourCircleRefId == inspection_Master.LabourCircleRefId && x.Version == 2).Select(x => x.UserRefId).FirstOrDefault();
                        var userDetails = await _iAuthService.GetUserRoleAndProfileDetailByUserId(labourOfficerId);
                        var officerRole = await _iAuthService.GetUserRoleByRoleId(userDetails.RoleId);
                        var inspectionFormEqualEnumerationAct = await GetInspectionFormLabourPartIIIEqualEnumerationAct(inspection);
                        var pairsEnumerationAct = ModelOps.GetTitleAndValues<Inspection_Form_Labour_Part_III_EqualEnumerationAct>(inspectionFormEqualEnumerationAct.FormModel);
                        violationCount = pairsEnumerationAct.Where(x => x.PropertyName.EndsWith("ViolationExist") && x.Value == "HAS_VIOLATION").Count();

                        if (violationCount == 0)
                        {
                            var inspectionFormLabourIIIMinimumWageAct = await GetInspectionFormLabourPartIIIMinimumWagesAct(inspection);
                            var pairsMinimumWageAct = ModelOps.GetTitleAndValues<Inspection_Form_Labour_III_MinimumWageAct>(inspectionFormLabourIIIMinimumWageAct.FormModel);
                            violationCount = pairsMinimumWageAct.Where(x => x.PropertyName.EndsWith("ViolationExist") && x.Value == "HAS_VIOLATION").Count(); ;
                        }

                        if (violationCount == 0)
                        {
                            var inspectionFormLabourIIIPaymentWagesAct = await GetInspectionFormLabourPartIIIPaymentWagesAct(inspection);
                            var pairsPaymentWagesAct = ModelOps.GetTitleAndValues<Inspection_Form_Labour_III_PaymentWagesAct>(inspectionFormLabourIIIPaymentWagesAct.FormModel);
                            violationCount = pairsPaymentWagesAct.Where(x => x.PropertyName.EndsWith("ViolationExist") && x.Value == "HAS_VIOLATION").Count();
                        }

                        if (violationCount == 0)
                        {
                            var inspectionFormLabourIIIPaymentBonusActStatutoryReport = await GetInspectionFormLabourPartIIIPaymentBonusAct_StatutoryReport(inspection);
                            var pairsPaymentBonusActStatutoryReport = ModelOps.GetTitleAndValues<Inspection_Form_Labour_III_PaymentBonusAct_StatutoryReport>(inspectionFormLabourIIIPaymentBonusActStatutoryReport.FormModel);
                            violationCount = pairsPaymentBonusActStatutoryReport.Where(x => x.PropertyName.EndsWith("ViolationExist") && x.Value == "HAS_VIOLATION").Count();
                        }

                        if (violationCount == 0)
                        {
                            var inspectionFormLabourIIIChildAndAdolescentLabourAct = await GetInspectionFormLabourPartIIIChildAndAdolescentLabourAct(inspection);
                            var pairsPaymentChildAndAdolescentLabourAct = ModelOps.GetTitleAndValues<Inspection_Form_Labour_III_ChildAndAdolescentLabourAct>(inspectionFormLabourIIIChildAndAdolescentLabourAct.FormModel);
                            violationCount = pairsPaymentChildAndAdolescentLabourAct.Where(x => x.PropertyName.EndsWith("ViolationExist") && x.Value == "HAS_VIOLATION").Count();
                        }

                        if (violationCount == 0)
                        {
                            var inspectionFormLabourIIINationalAndFestivalHolidays = await GetInspectionFormLabourPartIIINationalAndFestivalHolidays(inspection);
                            var pairsNationalAndFestivalHolidays = ModelOps.GetTitleAndValues<Inspection_Form_Labour_III_NationalAndFestivalHolidays>(inspectionFormLabourIIINationalAndFestivalHolidays.FormModel);
                            violationCount = pairsNationalAndFestivalHolidays.Where(x => x.PropertyName.EndsWith("ViolationExist") && x.Value == "HAS_VIOLATION").Count();
                        }

                        if (violationCount == 0)
                        {
                            var inspectionFormLabourIIIMaternityBenefitAct = await GetInspectionFormLabourPartIIIMaternityBenefitAct(inspection);
                            var pairsMaternityBenefitAct = ModelOps.GetTitleAndValues<Inspection_Form_Labour_III_MaternityBenefitAct>(inspectionFormLabourIIIMaternityBenefitAct.FormModel);
                            violationCount = pairsMaternityBenefitAct.Where(x => x.PropertyName.EndsWith("ViolationExist") && x.Value == "HAS_VIOLATION").Count();
                        }


                        if (violationCount == 0)
                        {
                            var inspectionFormLabourIIIContractLabourAct = await GetInspectionFormLabourPartIIIContractLabourAct(inspection);
                            var pairsContractLabourAct = ModelOps.GetTitleAndValues<Inspection_Form_Labour_III_ContractLabourAct>(inspectionFormLabourIIIContractLabourAct.FormModel);
                            violationCount = pairsContractLabourAct.Where(x => x.PropertyName.EndsWith("ViolationExist") && x.Value == "HAS_VIOLATION").Count();
                        }
                            if (violationCount == 0)
                            {
                                var inspectionFormLabourIIIInterStateMigrantWorkmenAct = await GetInspectionFormLabourPartIIIInterStateMigrantWorkmenAct(inspection);
                                var pairsInterStateMigrantWorkmenAct = ModelOps.GetTitleAndValues<Inspection_Form_Labour_III_InterStateMigrantWorkmenAct>(inspectionFormLabourIIIInterStateMigrantWorkmenAct.FormModel);
                                violationCount = pairsInterStateMigrantWorkmenAct.Where(x => x.PropertyName.EndsWith("ViolationExist") && x.Value == "HAS_VIOLATION").Count();
                            }

                            if (violationCount == 0)
                            {
                                var inspectionFormLabourIIILabourWelfareFundAct = await GetInspectionFormLabourPartIIILabourWelfareFund_Act(inspection);
                                var pairsLabourWelfareFundAct = ModelOps.GetTitleAndValues<Inspection_Form_Labour_III_LabourWelfareFund_Act>(inspectionFormLabourIIILabourWelfareFundAct.FormModel);
                                violationCount = pairsLabourWelfareFundAct.Where(x => x.PropertyName.EndsWith("ViolationExist") && x.Value == "HAS_VIOLATION").Count();
                            }

                            if (violationCount == 0)
                            {
                                var inspectionFormLabourIIIGratuityAct = await GetInspectionFormLabourPartIIIGratuityAct(inspection);
                                var pairsGratuityAct = ModelOps.GetTitleAndValues<Inspection_Form_Labour_III_GratuityAct>(inspectionFormLabourIIIGratuityAct.FormModel);
                                violationCount = pairsGratuityAct.Where(x => x.PropertyName.EndsWith("ViolationExist") && x.Value == "HAS_VIOLATION").Count();
                            }

                            if (violationCount == 0)
                            {
                                var inspectionFormLabourIIIIndustrialEmploymentAct = await GetInspectionFormLabourPartIIIIndustrialEmploymentAct(inspection);
                                var pairsIndustrialEmploymentAct = ModelOps.GetTitleAndValues<Inspection_Form_Labour_III_IndustrialEmploymentAct>(inspectionFormLabourIIIIndustrialEmploymentAct.FormModel);
                                violationCount = pairsIndustrialEmploymentAct.Where(x => x.PropertyName.EndsWith("ViolationExist") && x.Value == "HAS_VIOLATION").Count();
                            }

                            var userRefId = _context.InspectionUserMappings.Where(x => x.LicenceNumber == inspection_Master.LicenceNumber).Select(x => x.UserRefId).FirstOrDefault();
                            if (violationCount > 0)
                            {
                                var receiverInfo = await _iAuthService.GetUserProfileByUserId(userRefId);

                                InspectionComplianceLog inspectionComplianceLog = new InspectionComplianceLog();
                                {
                                    inspectionComplianceLog.AppActionType = 1;
                                    inspectionComplianceLog.Sender_UserRefId = userDetails.UserId;
                                    inspectionComplianceLog.Sender_ProfileRefId = userDetails.UserProfileId;
                                    inspectionComplianceLog.Receiver_UserRefId = userRefId;
                                    inspectionComplianceLog.Receiver_ProfileRefId = receiverInfo == null ? 0 : receiverInfo.UserProfileId;
                                    inspectionComplianceLog.ActionDate = DateTime.Now;
                                    inspectionComplianceLog.ActionTakenDaysCount = 0;
                                    inspectionComplianceLog.ActionTakenHoursCount = 0;
                                    inspectionComplianceLog.Remarks = "Violation Report Sent";
                                    inspectionComplianceLog.SenderRoleId = userDetails.RoleId;
                                    inspectionComplianceLog.ReceiverRoleId = "592add3e-f992-4983-a8ca-21ddc090bda0";
                                    inspectionComplianceLog.IsDocumentUploaded = false;
                                    inspectionComplianceLog.AppDocumentRefId = 0;
                                    inspectionComplianceLog.IpAddress = "";
                                    inspectionComplianceLog.Latitude = "";
                                    inspectionComplianceLog.Longitude = "";
                                    inspectionComplianceLog.InspectionType = 2;
                                    inspectionComplianceLog.InspectionRefId = inspection;
                                }
                                _iGR_InspectionComplianceLog.Insert(inspectionComplianceLog);
                                await _iGR_InspectionComplianceLog.SavechangeAsync();

                                Inspection_ViolationComplianceReminder inspection_ViolationComplianceReminder = new Inspection_ViolationComplianceReminder();
                                {
                                    inspection_ViolationComplianceReminder.StartDate = DateTime.Now;
                                    inspection_ViolationComplianceReminder.EndDate = DateTime.Now.AddDays(30);
                                    inspection_ViolationComplianceReminder.TimeInDays = 30;
                                    inspection_ViolationComplianceReminder.TotalRemindersToBeSend = 1;
                                    inspection_ViolationComplianceReminder.TotalRemindersSent = 0;
                                    inspection_ViolationComplianceReminder.ReminderStatus = 1;
                                    inspection_ViolationComplianceReminder.InspectionType = 2;
                                    inspection_ViolationComplianceReminder.InspectionRefId = inspection;
                                }
                                _iGR_Inspection_ViolationComplianceReminder.Insert(inspection_ViolationComplianceReminder);
                                await _iGR_Inspection_ViolationComplianceReminder.SavechangeAsync();

                                List<StoreProcedureParm> storeProcedureParms = new List<StoreProcedureParm>()
                    {
                        new StoreProcedureParm (){ ParmName="LicenceNo", ParmValue=inspection_Master.LicenceNumber.ToString(), isNumber=false}
                    };
                                var establishmentDetails = await _iGeneric_SP_Repository.CallStoreProcedureReaderAsync<InspectionEstablishmentBasicDetailsViewModel>("sp_GetInspectionEstablishmentBasicDetails", storeProcedureParms);

                                var circleDetails = await _iCircleManager.GetCircleDetailsByCircleId(inspection_Master.LabourCircleRefId, CircleTypeEnum.LABOUR_CIRCLE);
                                TemplateInspectionSubmittedNotificationViewModel templateInspectionSubmittedNotificationViewModel = new TemplateInspectionSubmittedNotificationViewModel();
                                templateInspectionSubmittedNotificationViewModel = new TemplateInspectionSubmittedNotificationViewModel()
                                {
                                    InspectionType = 2,
                                    OfficerName = userDetails.FullName,
                                    RoleName = officerRole.NormalizedName,
                                    IsViolationFound = violationCount,
                                    InspectionDate = inspection_Master.Inspection_Form_Labour_Part_I_General == null ? inspection_Master.InspectionDoneOn_Labour_Wing : inspection_Master.Inspection_Form_Labour_Part_I_General.DateOfInspection,
                                    EstablishmentName = establishmentDetails.FirstOrDefault().EstablishmentName,
                                    EstablishmentAddress = establishmentDetails.FirstOrDefault().EstablishmentAddress,
                                    LicenseNumber = establishmentDetails.FirstOrDefault().LicenceNo,
                                    FactoryStatus = inspection_Master.InspectionSubmited_Labour_Wing_FactoryExistenceType.ToString(),
                                    FactoryCircle = circleDetails.ResponseDataModel
                                };

                                var emailTemplateText = await _iNotificationManagerService.RenderToStringAsync("EmailTemplates/_templateInspectionSubmittedNotification", templateInspectionSubmittedNotificationViewModel);
                                await _iNotificationManagerService.InitiateNotification(userRefId,
                            new List<NotificationViewModel>()
                            {
                        new NotificationViewModel(){Body=emailTemplateText, NotificationMode=NotificationModeTypeEnum.EMAIL,NotificationPurpose=NotificationPurposeTypeEnum.INFO,Title ="Department of Labour: Inspection submitted(Labour-Wing)"}
                            });
                            }
                        }

                    }   
            }
            catch (Exception ex)
            {
                genericServiceResultTemplate.HasException = true;
                genericServiceResultTemplate.Exceptions = ex;
            }
            return genericServiceResultTemplate;
        }

        public async Task<GenericResponseTemplateModel<string>> SendOTPToUser(string mobileNumber, string userId, string licenceNumber, string newUserName)
        {
            GenericResponseTemplateModel<string> genericResponseTemplateModel = new GenericResponseTemplateModel<string>();
            try
            {
                var otp = GenerateOTPCode();
                var templateId = "1407173683467097508";
                //var smsText = $"Your OTP code is: {otp}. It will expire in 5 minutes.";
                var smsText = $"Dear User, This {otp} is for verification to merge your License no({licenceNumber}) with user id({newUserName}) - PB LABOUR";
                await _iNotificationManagerService.InitiateNotification(userId,
                new List<NotificationViewModel>()
                {
                    new NotificationViewModel(){Body=smsText,
                        NotificationMode=NotificationModeTypeEnum.MOBILE,
                        NotificationPurpose=NotificationPurposeTypeEnum.OTP,
                        Title=templateId}
                });

                var otpPayload = new
                {
                    otp = otp,
                    mobileNumber = mobileNumber
                };
                var encryptionConfigs = Configuration.GetSection("EncryptionConfigs");
                var encryptionKey = encryptionConfigs.GetSection("MobileAppDataEncryptionKey").Value;
                var ivKey = encryptionConfigs.GetSection("MobileAppDataIVKey").Value;
                string otpPayloadJsonText = JsonConvert.SerializeObject(otpPayload);
                genericResponseTemplateModel.ResponseDataModel = RijndaelManagedCryptoHandler.EncryptFromPlainTextToBase64(otpPayloadJsonText, encryptionKey, ivKey);
            }

            catch (Exception ex)
            {
                genericResponseTemplateModel.HasError = true;
                genericResponseTemplateModel.ErrorDesc = ex.Message;
            }

            return genericResponseTemplateModel;
        }

        public static string GenerateOTPCode()
        {
            const string Letters = "012346789";
            Random rand = new Random();
            int maxRand = Letters.Length - 1;

            StringBuilder sb = new StringBuilder();

            for (int i = 0; i < 4; i++)
            {
                int index = rand.Next(maxRand);
                sb.Append(Letters[index]);
            }

            return sb.ToString();
        }
        public async Task<GenericResponseTemplateModel<bool>> VerifyOTP(int otp, string encryptedData, string userId, string licenceNumber)
        {
            GenericResponseTemplateModel<bool> genericResponseTemplateModel = new GenericResponseTemplateModel<bool>();
            try
            {
                var decryptedPayload = RijndaelManagedCryptoHandler.DecryptFromPlainTextToBase64(encryptedData, "94c8e24f-6c5e-4d", "70k6e44y-9z5q-9q");
                var serializedData = JsonConvert.DeserializeObject<EncryptedOTPViewModel>(decryptedPayload);

                if (otp == serializedData.OTP)
                {
                    var oldUserId = _context.InspectionUserMappings.Where(x => x.LicenceNumber == licenceNumber).Select(x => x.UserRefId).FirstOrDefault();
                    var inspectionuser = _context.InspectionUserMappings.Where(x => x.LicenceNumber == licenceNumber).ToList();
                    Inspection_UserMapping_Logs inspection_UserMapping_Logs = new Inspection_UserMapping_Logs();
                    {
                        inspection_UserMapping_Logs.LicenceNumber = licenceNumber;
                        inspection_UserMapping_Logs.OldUserRefId = oldUserId;
                        inspection_UserMapping_Logs.NewUserRefId = userId;
                        inspection_UserMapping_Logs.TimeStemp = DateTime.Now;
                    }
                    _iGR_Inspection_UserMapping_Logs.Insert(inspection_UserMapping_Logs);
                    await _iGR_Inspection_UserMapping_Logs.SavechangeAsync();

                    if (inspectionuser.Count() > 0)
                    {
                        inspectionuser = inspectionuser.Select(x => { x.UserRefId = userId; return x; }).ToList();
                        await _context.BulkUpdateAsync<InspectionUserMapping>(inspectionuser);
                    }

                    var complianceLogsReceving = _context.InspectionComplianceLogs.Where(x => x.Receiver_UserRefId == oldUserId).ToList();
                    if (complianceLogsReceving.Count() > 0)
                    {
                        complianceLogsReceving = complianceLogsReceving.Select(x => { x.Receiver_UserRefId = userId; return x; }).ToList();
                        await _context.BulkUpdateAsync<InspectionComplianceLog>(complianceLogsReceving);
                    }

                    var complianceLogsSender = _context.InspectionComplianceLogs.Where(x => x.Sender_UserRefId == oldUserId).ToList();
                    if (complianceLogsSender.Count() > 0)
                    {
                        complianceLogsSender = complianceLogsSender.Select(x => { x.Sender_UserRefId = userId; return x; }).ToList();
                        await _context.BulkUpdateAsync<InspectionComplianceLog>(complianceLogsSender);
                    }
                    genericResponseTemplateModel.ResponseDataModel = true;
                }
                else
                {
                    genericResponseTemplateModel.ResponseDataModel = false;
                }
            }

            catch (Exception ex)
            {
                genericResponseTemplateModel.HasError = true;
                genericResponseTemplateModel.ErrorDesc = ex.Message;
            }

            return genericResponseTemplateModel;
        }

        public async Task<GenericResponseTemplateModel<List<InspectionTransferLogsViewModel>>> GetInspectionTransferLogs(Int64 inspectionRefId, int inspectionType)
        {
            GenericResponseTemplateModel<List<InspectionTransferLogsViewModel>> genericServiceResultTemplate = new GenericResponseTemplateModel<List<InspectionTransferLogsViewModel>>();
            try
            {
                List<StoreProcedureParm> storeProcedureParms = new List<StoreProcedureParm>()
                {
                        new StoreProcedureParm() { ParmName = "InspectionRefId", ParmValue = inspectionRefId.ToString(), isNumber = true },
                        new StoreProcedureParm() { ParmName = "InspectionType", ParmValue = inspectionType.ToString(), isNumber = true },
                };
                var NotingLogs = await _iGeneric_SP_Repository.CallStoreProcedureReaderAsync<InspectionTransferLogsViewModel>("sp_GetInspectionTransferLogs", storeProcedureParms);
                genericServiceResultTemplate.ResponseDataModel = NotingLogs;
            }

            catch (Exception ex)
            {
                genericServiceResultTemplate.HasError = true;
                genericServiceResultTemplate.ErrorDesc = ex.Message;
            }
            return genericServiceResultTemplate;
        }

        public async Task<GenericFormModel<List<RandomizationInitializationViewModel>>> RadomizationInitialization(int month, int year, string userRefId)
        {
            GenericFormModel<List<RandomizationInitializationViewModel>> genericFormModel = new GenericFormModel<List<RandomizationInitializationViewModel>>();
            try
            {
                bool exists = await _context.Randomization_Initialization
                    .Where(r => r.Month == month && r.Year == year && r.UserRefId == userRefId)
                    .AnyAsync();

                if (!exists)
                {
                    Randomization_Initialization randomization_Initialization = new Randomization_Initialization
                    {
                        Month = month,
                        Year = year,
                        UserRefId = userRefId,
                        Timestemp = DateTime.Now, 
                        RandomizationInitializationStatus = RandomizationInitilaztionProcessStatusTypeEnum.IN_PROCESS
                    };

                    _iGR_Randomization_Initialization.Insert(randomization_Initialization);
                    await _context.SaveChangesAsync(); 
                }

                var randomizationData = await _context.Randomization_Initialization
                    .Where(r => r.Month == month && r.Year == year && r.UserRefId == userRefId)
                    .ToListAsync();

                if (randomizationData != null && randomizationData.Any())
                {
                    genericFormModel.FormModel = randomizationData.Select(r => new RandomizationInitializationViewModel
                    {
                        Month = r.Month,
                        Year = r.Year,
                        RandomizationStatus = (int)r.RandomizationInitializationStatus, 
                    }).ToList();
                }
                
            }
            catch (Exception ex)
            {
                genericFormModel.HasError = true;
                genericFormModel.ErrorDesc = ex.Message;
            }

            return genericFormModel;
        }

        public async Task<GenericFormModel<OldInspectionMapping>> GetOldInspectionId(Int64 inspectionRefId)
        {
            GenericFormModel<OldInspectionMapping> genericFormModel = new GenericFormModel<OldInspectionMapping>();
            try
            {
                var data = _context.OldInspectionMappings.Where(x => x.InspectionId == inspectionRefId).FirstOrDefault();
                genericFormModel.FormModel = data;

            }
            catch (Exception ex)
            {
                genericFormModel.HasError = true;
                genericFormModel.ErrorDesc = ex.Message;
            }

            return genericFormModel;
        }

        public async Task<GenericResponseTemplateModel<InspectionFactoryAllotmentDetailsViewModel>> GetInspectionFactoryDetailsByLicenceNo_Allotment(string licenceNo)
        {
            GenericResponseTemplateModel<InspectionFactoryAllotmentDetailsViewModel> genericFormModel = new GenericResponseTemplateModel<InspectionFactoryAllotmentDetailsViewModel>();
            try
            {
                List<StoreProcedureParm> storeProcedureParms = new List<StoreProcedureParm>()
                {
                    new StoreProcedureParm (){ ParmName="LicenceNo", ParmValue=licenceNo.ToString(), isNumber=false}
                };
                var EstablishmentDetails = await _iGeneric_SP_Repository.CallStoreProcedureReaderAsync<InspectionFactoryAllotmentDetailsViewModel>("sp_GetInspectionFactoryDetailsByLicenceNo_Allotment", storeProcedureParms);
                genericFormModel.ResponseDataModel = EstablishmentDetails.FirstOrDefault();

            }
            catch (Exception ex)
            {
                genericFormModel.HasError = true;
                genericFormModel.ErrorDesc = ex.Message;
            }
            return genericFormModel;
        }

        public async Task<GenericServiceResultTemplate> Insert_AllotmentInspections(InspectionAllotmentRequestViewModel requestData)
        {

            GenericServiceResultTemplate genericServiceResultTemplate = new GenericServiceResultTemplate();
            genericServiceResultTemplate.HasException = false;
            try
            {
                Inspection_AllotmentLogs inspection_Allotment = new Inspection_AllotmentLogs
                {
                    LicenceNumber = requestData.LicenceNumber,
                    UserRefId = requestData.SubmittedUserId,
                    RoleId = requestData.SubmittedRoleId,
                    ProfileId = requestData.SubmittedProfileId,
                    TimeStamp = DateTime.Now,
                    Month = requestData.Month,
                    Year = requestData.Year
                };
                _context.Inspection_AllotmentLogs.Add(inspection_Allotment);
                await _context.SaveChangesAsync();


                var profileDetails = await _iAuthService.GetUserProfileByUserId(requestData.UserId);
                Inspection_Randomization inspection_Randomization = new Inspection_Randomization
                {
                    Month = requestData.Month,
                    Year = requestData.Year,
                    ProcessInitiatedByType = InspectionRandomizationProcessInitiatedByTypeEnum.BY_USER,
                    Description = "",
                    Timestemp = DateTime.Now,
                    UserRefId = requestData.SubmittedUserId
                };

                _context.Inspection_Randomizations.Add(inspection_Randomization);
                await _context.SaveChangesAsync();

                Inspection_Master inspection_Master = new Inspection_Master
                {
                    LicenceNumber = requestData.LicenceNumber,
                    IsLegacy = Convert.ToBoolean(requestData.IsLegacy),
                    FactoryCircleId = requestData.FactoryCircleRefId,
                    AppId = requestData.AppRefId,
                    ApplicationType = ApplicationTypeEnum.FACTORY_LICENCE,
                    UserProfileId = profileDetails.UserProfileId,
                    HasAssignedLabourCircle = false,
                    LabourCircleRefId = 0,
                    RandomizationRefId = inspection_Randomization.RandomizationId,
                    FactoryHazardousCategoryType =(FactoryHazardousCategoryTypeEnum)requestData.FactoryHazardousCategoryType,
                    FactorySectionCategoryType =(FactorySectionCategoryTypeEnum)(Convert.ToInt32(requestData.FactorySectionCategoryType)),
                    FactorySessionCategoryType = (FactorySessionCategoryTypeEnum)requestData.FactorySessionCategoryType,
                    UserId = requestData.UserId,
                    AdfDdfNameOfLegacyRecord = "N/A",
                    DistrictRefId = requestData.DistrictId,
                    InspectionDoneOn_Factory_Wing = null,
                    InspectionDoneOn_Labour_Wing = null,
                    InspectionSubmitOn_Factory_Wing = null,
                    InspectionSubmitOn_Labour_Wing = null,
                    InspectionSubmited_Factory_Wing_ProfileId = 0,
                    InspectionSubmited_Factory_Wing_RoleId = "",
                    InspectionSubmited_Factory_Wing_UserId = "",
                    InspectionSubmited_Labour_Wing_ProfileId = 0,
                    InspectionSubmited_Labour_Wing_RoleId = "",
                    InspectionSubmited_Labour_Wing_UserId = "",
                    IsSubmitted_Factory_Wing = false,
                    IsSubmitted_Labour_Wing = false,
                    FactoryDeRegistrationNo = "",
                    InspectionEstablishmentType = 0,
                    InspectionSubmited_Factory_Wing_FactoryExistenceType = 0,
                    InspectionSubmited_Labour_Wing_FactoryExistenceType = 0,
                    Remarks = "",
                    ALCCircleRefId = 0,
                    ALCProfileRefId =0,
                    ALCUserRefId = ""
                };
                _context.Inspection_Master.Add(inspection_Master);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                genericServiceResultTemplate.HasException = true;
                genericServiceResultTemplate.Exceptions = ex;
            }
            return genericServiceResultTemplate;
        }

        public async Task<GenericResponseTemplateModel<IntReturn>> VerifyCurrentMonthYear(int month ,int year)
        {
            GenericResponseTemplateModel<IntReturn> genericResponseTemplateModel = new GenericResponseTemplateModel<IntReturn>();
            try
            {
                int isRandomized = _context.Inspection_Randomizations.Count(x => x.Year == year && x.Month == month) > 0 ? 1 : 0;

                genericResponseTemplateModel.ResponseDataModel = new IntReturn
                {
                    Value = isRandomized
                };
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
