using EFCore.BulkExtensions;
using iTextSharp.text;
using iTextSharp.text.pdf;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;
using Newtonsoft.Json;
using pbsamadhannetcoreapi.CommonUtiliteis;
using pbsamadhannetcoreapi.Models;
using pbsamadhannetcoreapi.Repositories.Implementations;
using pbsamadhannetcoreapi.Services.Implementations;
using pbsamadhannetcoreapi.ViewModels;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace pbsamadhannetcoreapi.Services
{
    public class FactoryLicenceService : IFactoryLicenceService
    {
        private readonly IGenericRepository<Licence_Factory_GeneralDetail> _iGR_Licence_Factory_GeneralDetail;
        private readonly IGenericRepository<Licence_Factory_OccupierAndManagerDetail> _iGR_Licence_Factory_OccupierAndManagerDetail;
        private readonly IApplicationManagementService<Licence_Factory_GeneralDetail> _iApplicationMamnagementService;
        private readonly IApplicationManagementService<Licence_Factory_QuestionnaireDetail> _iApplicationMamnagementService_QuestionnaireDetail;
        private readonly IGenericRepository<BuildingPlanHUDPaymentDetail> _iGR_AppFeeDetail;
        private readonly IPaymentManagerRepository _iPaymentManagerRepository;
        private readonly IPaymentManagerService _iPaymentManagerService;
        private readonly IBuildingPlanHUDRepository _iBuildingPlanHUDRepository;
        private readonly IThirdPartyInegrationsService _iThirdPartyInegrationsService;
        private readonly IGenericRepository<BuildingPlanHUD_RTB_Mapping> _iGR_BuildingPlanHUD_RTB_Mapping;
        private readonly AppDbContext _context;
        private readonly IGenericRepository<Licence_Factory_QuestionnaireDetail> _iGR_Licence_Factory_QuestionnaireDetail;
        private readonly IGeneric_SP_Repository _iGeneric_SP_Repository;
        private readonly IProjectSiteService _iProjectSiteService;
        private IAuthService _iAuthService;
        public FactoryLicenceService(IGenericRepository<Licence_Factory_GeneralDetail> iGR_Licence_Factory_GeneralDetail,
             IGenericRepository<Licence_Factory_OccupierAndManagerDetail> iGR_Licence_Factory_OccupierAndManagerDetail,
             IApplicationManagementService<Licence_Factory_GeneralDetail> IApplicationMamnagementService,
             IGenericRepository<BuildingPlanHUDPaymentDetail> iGR_AppFeeDetail,
             IPaymentManagerRepository iPaymentManagerRepository,
             IPaymentManagerService iPaymentManagerService,
             IBuildingPlanHUDRepository iBuildingPlanHUDRepository,
             IThirdPartyInegrationsService iThirdPartyInegrationsService,
             IGenericRepository<BuildingPlanHUD_RTB_Mapping> iGR_BuildingPlanHUD_RTB_Mapping,
             AppDbContext context,
             IGenericRepository<Licence_Factory_QuestionnaireDetail> iGR_Licence_Factory_QuestionnaireDetail,
             IApplicationManagementService<Licence_Factory_QuestionnaireDetail> iApplicationMamnagementService_QuestionnaireDetail,
             IGeneric_SP_Repository iGeneric_SP_Repository,
             IProjectSiteService iProjectSiteService,
             IAuthService authService)
        {
            _iGR_Licence_Factory_GeneralDetail = iGR_Licence_Factory_GeneralDetail;
            _iGR_Licence_Factory_OccupierAndManagerDetail = iGR_Licence_Factory_OccupierAndManagerDetail;
            _iApplicationMamnagementService = IApplicationMamnagementService;
            _iGR_AppFeeDetail = iGR_AppFeeDetail;
            _iPaymentManagerRepository = iPaymentManagerRepository;
            _iPaymentManagerService = iPaymentManagerService;
            _iBuildingPlanHUDRepository = iBuildingPlanHUDRepository;
            _iThirdPartyInegrationsService = iThirdPartyInegrationsService;
            _iGR_BuildingPlanHUD_RTB_Mapping = iGR_BuildingPlanHUD_RTB_Mapping;
            _context = context;
            _iGR_Licence_Factory_QuestionnaireDetail = iGR_Licence_Factory_QuestionnaireDetail;
            _iApplicationMamnagementService_QuestionnaireDetail = iApplicationMamnagementService_QuestionnaireDetail;
            _iGeneric_SP_Repository = iGeneric_SP_Repository;
            _iProjectSiteService = iProjectSiteService;
            _iAuthService = authService;
        }

        #region Factory General Details
        public async Task<GenericFormModel<Licence_Factory_GeneralDetail>> GetFactoryLicenceGeneralDetail(long id, long projectSiteId)
        {
            GenericFormModel<Licence_Factory_GeneralDetail> genericFormModel = new GenericFormModel<Licence_Factory_GeneralDetail>();
            try
            {
                if (id != 0) //Existing Record
                {

                    genericFormModel.FormModel = _context.Licence_Factory_GeneralDetails.Where(x => x.AppRefId == id).Include(x=>x.Application).FirstOrDefault();
                    
                    //var parentWithChildObject = await _iGR_Licence_Factory_GeneralDetail
                    //   .GetAsync(x => x.AppRefId == id,  //Conditions         
                    //     null,                //Orders          
                    //     x => x.Application, x => x.Application.ApplicationAction)      //Includes
                    //   .ConfigureAwait(false);
                    //genericFormModel.FormModel = parentWithChildObject.FirstOrDefault();

                    genericFormModel.IsEditAllowed = genericFormModel.FormModel.Application.IsAllowEdit;
                    genericFormModel.IsLocked = genericFormModel.FormModel.Application.IsLocked;
                    genericFormModel.ApplicationLifeCycleStatusType = genericFormModel.FormModel.Application.ApplicationLifeCycleStatusType;
                    genericFormModel.FormModel.IPin = Convert.ToInt64(genericFormModel.FormModel.Application.InvestPunjab_Ipin);
                    genericFormModel.FormModel.InvestPunjab_AppId = genericFormModel.FormModel.Application.InvestPunjab_AppId;

                    genericFormModel.FormModel.ApplicationPurposeType = genericFormModel.FormModel.Application.ApplicationPurposeType;

                    if (genericFormModel.FormModel.Application.ApplicationPurposeType == ApplicationPurposeTypeEnum.GRANT_LICENCE)
                    {
                        genericFormModel.FormModel.RenewalFromDate_Json = null;
                        genericFormModel.FormModel.RenewalFromDate = null;

                        genericFormModel.FormModel.OldLicenceValidUpTo_Json = null;
                        genericFormModel.FormModel.OldLicenceValidUpTo = null;
                        var dateComponents = new DateComponentViewModel()
                        {
                            year = genericFormModel.FormModel.RegistrationDate.Value.Year,
                            day = genericFormModel.FormModel.RegistrationDate.Value.Day,
                            month = genericFormModel.FormModel.RegistrationDate.Value.Month
                        };

                        genericFormModel.EnumTemplateLists = new List<EnumListTemplate>();
                        genericFormModel.EnumTemplateLists.Add(new EnumListTemplate()
                        {
                            SelectListTypeCode = "BuildingPlanStabilityAuthorityTypeEnum",
                            SelectListItems = EnumOps.GetEnumAsSelectList<BuildingPlanStabilityAuthorityTypeEnum>()
                        });

                        //Initialization of list templates
                        var competentPersonList = await _iAuthService.GetOfficerDetailsByRoleName("CPPR");

                        genericFormModel.FormModel.CompetentPersonList = competentPersonList.ListData;

                        var empaneledEngineersList = await _iAuthService.GetOfficerDetailsByRoleName("ENGR");
                        genericFormModel.FormModel.EmpaneledEngineersList = empaneledEngineersList.ListData;

                        genericFormModel.FormModel.RegistrationDate_Json = dateComponents;
                    }
                    else if (genericFormModel.FormModel.Application.ApplicationPurposeType == ApplicationPurposeTypeEnum.RENEWAL_LICENCE)
                    {
                        if (genericFormModel.FormModel.Application.Legacy_LicenceNo != null)
                        {
                            List<StoreProcedureParm> storeProcedureParms = new List<StoreProcedureParm>()
                            {
                                new StoreProcedureParm (){ ParmName="LicenceNumber", ParmValue=genericFormModel.FormModel.Application.Legacy_LicenceNo.ToString(), isNumber=false}
                            };
                            var allClearancesWithSlabs = await _iGeneric_SP_Repository.CallStoreProcedureReaderAsync<GetAllClearanceDateSlabsViewModel>("sp_GetAllClearanceDateSlabsByLicenceNumber", storeProcedureParms);

                            if (allClearancesWithSlabs != null && allClearancesWithSlabs.Count() > 0)
                            {
                                genericFormModel.FormModel.RenewalFromDate = allClearancesWithSlabs.FirstOrDefault().ClearanceExpiredOn.AddDays(1);
                                genericFormModel.FormModel.OldLicenceValidUpTo = allClearancesWithSlabs.FirstOrDefault().ClearanceExpiredOn;
                            }
                        }

                        var dateComponents = new DateComponentViewModel()
                        {
                            year = genericFormModel.FormModel.RenewalFromDate.Value.Year,
                            day = genericFormModel.FormModel.RenewalFromDate.Value.Day,
                            month = genericFormModel.FormModel.RenewalFromDate.Value.Month
                        };

                        genericFormModel.FormModel.RenewalFromDate_Json = dateComponents;

                        dateComponents = new DateComponentViewModel()
                        {
                            year = genericFormModel.FormModel.OldLicenceValidUpTo.Value.Year,
                            day = genericFormModel.FormModel.OldLicenceValidUpTo.Value.Day,
                            month = genericFormModel.FormModel.OldLicenceValidUpTo.Value.Month
                        };

                       genericFormModel.FormModel.OldLicenceValidUpTo_Json = dateComponents;

                        genericFormModel.FormModel.RegistrationDate = null;
                        genericFormModel.FormModel.RegistrationDate_Json = null;
                    }
                    else
                    {
                        genericFormModel.FormModel.RenewalFromDate_Json = null;
                        genericFormModel.FormModel.RenewalFromDate = null;

                        if (genericFormModel.FormModel.Application.Legacy_LicenceNo != null)
                        {
                            List<StoreProcedureParm> storeProcedureParms = new List<StoreProcedureParm>()
                            {
                                new StoreProcedureParm (){ ParmName="LicenceNumber", ParmValue=genericFormModel.FormModel.Application.Legacy_LicenceNo.ToString(), isNumber=false}
                            };
                            var allClearancesWithSlabs = await _iGeneric_SP_Repository.CallStoreProcedureReaderAsync<GetAllClearanceDateSlabsViewModel>("sp_GetAllClearanceDateSlabsByLicenceNumber", storeProcedureParms);

                            if (allClearancesWithSlabs != null && allClearancesWithSlabs.Count() > 0)
                            {
                                //genericFormModel.FormModel.RenewalFromDate = allClearancesWithSlabs.FirstOrDefault().ClearanceExpiredOn.AddDays(1);
                                genericFormModel.FormModel.OldLicenceValidUpTo = allClearancesWithSlabs.FirstOrDefault().ClearanceExpiredOn;
                            }
                        }

                        var dateComponents = new DateComponentViewModel()
                        {
                            year = genericFormModel.FormModel.OldLicenceValidUpTo.Value.Year,
                            day = genericFormModel.FormModel.OldLicenceValidUpTo.Value.Day,
                            month = genericFormModel.FormModel.OldLicenceValidUpTo.Value.Month
                        };

                        genericFormModel.FormModel.OldLicenceValidUpTo_Json = dateComponents;

                        //genericFormModel.FormModel.RegistrationDate = null;
                        //genericFormModel.FormModel.RegistrationDate_Json = null;
                    }

                    var siteId = await _iProjectSiteService.GetProjectSiteRefIdByAppId(id);
                    var projectProfileInfo = await _iProjectSiteService.GetProjectSiteDetail(siteId.ResponseDataModel);
                    genericFormModel.FormModel.FactoryCircleRefId = projectProfileInfo.FormModel.FactoryCircleRefId;
                }

                else //New Record
                {
                    genericFormModel.FormModel = new Licence_Factory_GeneralDetail();

                    genericFormModel.IsEditAllowed = true;
                    genericFormModel.IsLocked = false;
                    genericFormModel.ApplicationLifeCycleStatusType = ApplicationLifeCycleStatusTypeEnum.NOT_SUBMITTED;


                    genericFormModel.FormModel.RenewalFromDate_Json = null;
                    genericFormModel.FormModel.RenewalFromDate = null;

                    genericFormModel.FormModel.OldLicenceValidUpTo_Json = null;
                    genericFormModel.FormModel.OldLicenceValidUpTo = null;

                    genericFormModel.FormModel.RegistrationDate_Json = null;
                    genericFormModel.FormModel.RegistrationDate = null;

                    genericFormModel.FormModel.ProjectSiteRefId = projectSiteId;
                    genericFormModel.ListTemplateLists = new List<ListTemplate>();

                    genericFormModel.ListTemplateLists = new List<ListTemplate>();

                    genericFormModel.EnumTemplateLists = new List<EnumListTemplate>();
                    genericFormModel.EnumTemplateLists.Add(new EnumListTemplate()
                    {
                        SelectListTypeCode = "BuildingPlanStabilityAuthorityTypeEnum",
                        SelectListItems = EnumOps.GetEnumAsSelectList<BuildingPlanStabilityAuthorityTypeEnum>()
                    });

                    //Initialization of list templates
                    var competentPersonList = await _iAuthService.GetOfficerDetailsByRoleName("CPPR");

                    genericFormModel.FormModel.CompetentPersonList = competentPersonList.ListData;

                    var empaneledEngineersList = await _iAuthService.GetOfficerDetailsByRoleName("ENGR");
                    genericFormModel.FormModel.EmpaneledEngineersList = empaneledEngineersList.ListData;

                    // Get Competent Person Details By Role
                    //var competentPersonList = await _iAuthService.GetOfficerDetailsByRoleName("CPPR");
                    //genericFormModel.FormModel.CompetentPersonList = competentPersonList.ListData;
                }
                genericFormModel.AppFormStepsList = new List<AppFormStepsInfo>();
                genericFormModel.AppFormStepsList = await _iApplicationMamnagementService.GetAppFormStepperInfo(id, ApplicationTypeEnum.FACTORY_LICENCE, (genericFormModel.FormModel != null ? genericFormModel.FormModel.FactoryLicenceId : id), "GD");
            }
            catch (Exception ex)
            {
                genericFormModel.HasError = true;
                genericFormModel.ErrorDesc = ex.Message;
                throw ex;
            }
            return genericFormModel;
        }
        public async Task<GenericServiceResultTemplate> AddUpdate_GeneralDetail(Licence_Factory_GeneralDetail formModel, string id)
        {
            GenericServiceResultTemplate genericServiceResultTemplate = new GenericServiceResultTemplate();
            try
            {
                genericServiceResultTemplate.HasException = false;
                genericServiceResultTemplate.CustomeValidationResult = CustomeValidator<Licence_Factory_GeneralDetail>.ValidateModel_AllProperties(formModel);
                if (genericServiceResultTemplate.CustomeValidationResult.IsValid)
                {
                    Licence_Factory_GeneralDetail previousFactoryLicenseData = null;
                    List<Licence_Factory_AmendmentDataHistory> savedAmendmentDataHistories = null;

                    if (formModel.ApplicationPurposeType == ApplicationPurposeTypeEnum.GRANT_LICENCE)
                    {
                        formModel.RenewalFromDate = null;
                        formModel.OldLicenceValidUpTo = null;
                        formModel.RegistrationDate = new DateTime(formModel.RegistrationDate_Json.year, formModel.RegistrationDate_Json.month, formModel.RegistrationDate_Json.day);
                    }
                    else if (formModel.ApplicationPurposeType == ApplicationPurposeTypeEnum.RENEWAL_LICENCE)
                    {
                        formModel.RenewalFromDate = new DateTime(formModel.RenewalFromDate_Json.year, formModel.RenewalFromDate_Json.month, formModel.RenewalFromDate_Json.day);
                        formModel.OldLicenceValidUpTo = new DateTime(formModel.OldLicenceValidUpTo_Json.year, formModel.OldLicenceValidUpTo_Json.month, formModel.OldLicenceValidUpTo_Json.day);
                        formModel.RegistrationDate = null;
                    }
                    else
                    {   
                        formModel.RenewalFromDate = null;
                        formModel.OldLicenceValidUpTo = new DateTime(formModel.OldLicenceValidUpTo_Json.year, formModel.OldLicenceValidUpTo_Json.month, formModel.OldLicenceValidUpTo_Json.day);
                        formModel.RegistrationDate = null;
                    }

                    if (formModel.FactoryLicenceId != 0) //Existing record
                    {
                        previousFactoryLicenseData = _context.Licence_Factory_GeneralDetails.Where(x => x.FactoryLicenceId == formModel.FactoryLicenceId).AsNoTracking().FirstOrDefault();
                        savedAmendmentDataHistories = await _context.Licence_Factory_AmendmentDataHistories.Where(x => 
                        !x.IsLocked && 
                        x.ModifiedCounter == previousFactoryLicenseData.ModifiedCounter && 
                        (x.FieldName == "PowerKW_Installed" || x.FieldName== "Workers_MaxDuringYear")).ToListAsync();

                        if (savedAmendmentDataHistories.Count() > 0)
                        {
                            formModel.ModifiedCounter = previousFactoryLicenseData.ModifiedCounter + 1;
                        }
                        else
                        {
                            formModel.ModifiedCounter = previousFactoryLicenseData.ModifiedCounter;
                        }

                        _iGR_Licence_Factory_GeneralDetail.Update(formModel);
                        //Save changes
                        await _iGR_Licence_Factory_GeneralDetail.SavechangeAsync();
                        //Update last modified date
                        await _iApplicationMamnagementService.UpdateAppLastModifiedDate(formModel.AppRefId);
                        genericServiceResultTemplate.ApplicationInitiateResponse = new ApplicationInitiateResponseViewModel();
                        genericServiceResultTemplate.ApplicationInitiateResponse.EntityKeyId = formModel.FactoryLicenceId;
                        genericServiceResultTemplate.ApplicationInitiateResponse.AppId = formModel.AppRefId;

                        // Update factory circle
                        await _iProjectSiteService.SetProjectSiteCircle(formModel.ProjectSiteRefId, formModel.FactoryCircleRefId, 0,0);
                    }
                    else //New record
                    {
                        //Set null all navigation properties from model
                        formModel = GenericModelOps<Licence_Factory_GeneralDetail>.SetNullAllNevigationProperties(formModel);
                        formModel.ModifiedCounter = 1;
                        genericServiceResultTemplate.ApplicationInitiateResponse = await _iApplicationMamnagementService.InitiateApplication(ApplicationTypeEnum.FACTORY_LICENCE, formModel, formModel.ProjectSiteRefId, formModel.ApplicationPurposeType, id, formModel.IPin,formModel.InvestPunjab_AppId, false, 0, 0, null, null, formModel.ProjectSiteVersion);

                        if (genericServiceResultTemplate.ApplicationInitiateResponse.IsApplicationCreated == true)
                        {

                            // update LabourCircle Id in ProjectSite Table
                            var projectSiteDetail = await _iApplicationMamnagementService.GetProjectSiteByAppRefId(genericServiceResultTemplate.ApplicationInitiateResponse.AppId);
                            await _iProjectSiteService.SetProjectSiteCircle(projectSiteDetail.FormModel.ProjectSiteId, formModel.FactoryCircleRefId, 0,0);

                            // Update Native Appid by IPIN
                            var temp = await _iThirdPartyInegrationsService.UpdateBusinessFirstRequestNativeAppIdByIPin(formModel.IPin, formModel.InvestPunjab_AppId, genericServiceResultTemplate.ApplicationInitiateResponse.AppId, ApplicationTypeEnum.FACTORY_LICENCE, formModel.ApplicationPurposeType);

                            // Share status to Invest Punjab Portal
                            await _iThirdPartyInegrationsService.ShareStatusToBusinessFirst(genericServiceResultTemplate.ApplicationInitiateResponse.AppId, ApplicationTypeEnum.FACTORY_LICENCE, AppActionTypeEnum.APP_SAVE_DRAFT);
                        }
                        else if (genericServiceResultTemplate.ApplicationInitiateResponse.IsApplicationCreated == false)
                        {
                            genericServiceResultTemplate.HasException = true;
                            genericServiceResultTemplate.Exceptions = new Exception();
                        }

                        if(formModel.IsTempRegistered == 1 && formModel.IsTempRegistrationVerified == true) // If Temporary licence is verified
                        {
                            // Step- 1. Call API
                            GenericResponseTemplateModel<Factory_TemporaryLicenceDetailsViewModel> genericFormModel  = await _iThirdPartyInegrationsService.GetTemporaryLicenseDetails(formModel.TempRegistrationNumber);
                            
                            // Step -2. Data save in occupier & manager details table
                            Licence_Factory_OccupierAndManagerDetail licence_Factory_OccupierAndManagerDetail = new Licence_Factory_OccupierAndManagerDetail()
                            {
                                ManagerFullName = genericFormModel.ResponseDataModel.ManagerFullName,
                                ManagerFatherName = genericFormModel.ResponseDataModel.ManagerFatherName,
                                ManagerFullAddress = genericFormModel.ResponseDataModel.ManagerFullAddress,
                                ManagerMobile = genericFormModel.ResponseDataModel.ManagerMobile,
                                ManagerEmail = genericFormModel.ResponseDataModel.ManagerEmail,
                                ManagerResidentialAddress = genericFormModel.ResponseDataModel.ManagerFullAddress,
                                OccupierFullName = genericFormModel.ResponseDataModel.OccupierFullName,
                                OccupierFatherName = genericFormModel.ResponseDataModel.OccupierFatherName,
                                OccupierFullAddress = genericFormModel.ResponseDataModel.OccupierFullAddress,
                                OccupierMobile = genericFormModel.ResponseDataModel.OccupierMobile,
                                OccupierEmail = genericFormModel.ResponseDataModel.OccupierEmail,
                                OccupierResidentialAddress = genericFormModel.ResponseDataModel.OccupierFullAddress,
                                OwnerName = genericFormModel.ResponseDataModel.OwnerName,
                                OwnerPremisesAddress = genericFormModel.ResponseDataModel.OwnerPremisesAddress,
                                StabilityCertificateNumber = "NA",
                                StabilityCertificateDate = DateTime.Now,
                                StabilityDOFNumber = genericFormModel.ResponseDataModel.ReferenceNumberBuildingConst,
                                Checklist_IsBuildingPlanApproved = "0",
                                Checklist_IsLabourWelfareFundPaid = "0",
                                Checklist_IsAnnualReturnFiled = "0",
                                Checklist_IsStabilityCertificateAttached = "0",
                                ModifiedCounter = 1,
                                FactoryLicenceRefId = genericServiceResultTemplate.ApplicationInitiateResponse.EntityKeyId
                            };
                            _iGR_Licence_Factory_OccupierAndManagerDetail.Insert(licence_Factory_OccupierAndManagerDetail);
                            await _iGR_Licence_Factory_OccupierAndManagerDetail.SavechangeAsync();

                            // step - 3. Data save in AppFeeDetails
                            AppFeeDetail appFeeDetail = new AppFeeDetail()
                            {
                                AppRefId = genericServiceResultTemplate.ApplicationInitiateResponse.AppId,
                                FeeHeaderRefId = 31,
                                Amount = genericFormModel.ResponseDataModel.TXN_AMOUNT,
                                CalculatedOn = genericFormModel.ResponseDataModel.PaymentDate,
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
                                TransactionInitializationDate = genericFormModel.ResponseDataModel.PaymentDate,
                                PaymentGatewayType = PaymentGatewayTypeEnum.IFMS,
                                PaymentModeType = PaymentModeTypeEnum.ONLINE,
                                PaymentTreasuryType = PaymentTreasuryTypeEnum.TREASURY,
                                PaymentGatewayTargetUrl = "NA",
                                PaymentGatewayApiMethodType = PaymentGatewayApiMethodTypeEnum.POST,
                                UniquePaymentGatewayTransactionId = genericFormModel.ResponseDataModel.ME_TXN_REF_NO,
                                IsWebRequestCycleCompleted = true,
                                RequestBodyData = genericFormModel.ResponseDataModel.ReqMsg,
                                ResponseBodyData = genericFormModel.ResponseDataModel.ResMsg,
                                ResponseReceivedOn = genericFormModel.ResponseDataModel.PaymentDate,
                                ResponseMessage = genericFormModel.ResponseDataModel.ResMsg,
                                TransactionFinalStatusType = TransactionFinalStatusTypeEnum.SUCCEED,
                                AmountCalculated = genericFormModel.ResponseDataModel.TXN_AMOUNT,
                                PaymentBatchCounter = 1,
                                PaymentPartCounter = 1,
                                BankTransactionRefNumber1 = genericFormModel.ResponseDataModel.BANK_REFERENCE_NO,
                                BankTransactionRefNumber1_Type = BankTransactionReferenceTypeEnum.BANK_REF_NUMBER,
                                BankTransactionRefNumber2 = genericFormModel.ResponseDataModel.BANK_REFERENCE_NO,
                                BankTransactionRefNumber2_Type = BankTransactionReferenceTypeEnum.BANK_REF_NUMBER,
                                BankSettlementOn = genericFormModel.ResponseDataModel.PaymentDate,
                                AppRefId = genericServiceResultTemplate.ApplicationInitiateResponse.AppId
                            };
                            await _context.AppFeeTransactions.AddAsync(appFeeTransaction);
                            await _context.SaveChangesAsync();

                            //step - 5. Insert into SuccessMapping Table
                            AppPaymentSuccessTransactionMapping appPaymentSuccessTransactionMapping = new AppPaymentSuccessTransactionMapping()
                            {
                                AppRefId = genericServiceResultTemplate.ApplicationInitiateResponse.AppId,
                                AppFeeTransactionRefId = appFeeTransaction.AppFeeTransactionId,
                                PaymentBatchCounter = 1,
                                PaymentPartCounter = 1,
                            };
                            await _context.AppPaymentSuccessTransactionMappings.AddAsync(appPaymentSuccessTransactionMapping);
                            await _context.SaveChangesAsync();
                        }
                    }

                    if(formModel.ApplicationPurposeType == ApplicationPurposeTypeEnum.RENEWAL_LICENCE || formModel.ApplicationPurposeType == ApplicationPurposeTypeEnum.AMENDMENT_LICENCE && previousFactoryLicenseData!=null)
                    {
                        List<Licence_Factory_AmendmentDataHistory> amendmentDataHistories = new List<Licence_Factory_AmendmentDataHistory>();

                        if(savedAmendmentDataHistories.Any(x=>x.FieldName == "Workers_MaxDuringYear"))
                        {
                            if(Convert.ToInt64(savedAmendmentDataHistories.Where(x => x.FieldName == "Workers_MaxDuringYear").Select(x=>x.PreviousValue).FirstOrDefault()) == formModel.Workers_MaxDuringYear)
                            {
                                _context.Licence_Factory_AmendmentDataHistories.Remove(savedAmendmentDataHistories.Where(x => x.FieldName == "Workers_MaxDuringYear").FirstOrDefault());
                            }
                            else
                            {
                                var amdHistory = savedAmendmentDataHistories.Where(x => x.FieldName == "Workers_MaxDuringYear").FirstOrDefault();
                                amdHistory.ModifiedValue = formModel.Workers_MaxDuringYear.ToString();
                                _context.Update<Licence_Factory_AmendmentDataHistory>(amdHistory);
                            }
                            await _context.SaveChangesAsync();
                        }
                        else
                        {
                            if (previousFactoryLicenseData.Workers_MaxDuringYear != formModel.Workers_MaxDuringYear)
                            {
                                amendmentDataHistories.Add(new Licence_Factory_AmendmentDataHistory()
                                {
                                    AppRefId = formModel.AppRefId,
                                    FieldName = "Workers_MaxDuringYear",
                                    PreviousValue = previousFactoryLicenseData.Workers_MaxDuringYear.ToString(),
                                    ModifiedValue = formModel.Workers_MaxDuringYear.ToString(),
                                    SectionCode = "MP", //MANPOWER SECTION
                                    ModifiedOn = DateTime.Now,
                                    ModifiedCounter = formModel.ModifiedCounter
                                });
                            }
                        }

                        if (savedAmendmentDataHistories.Any(x => x.FieldName == "PowerKW_Installed"))
                        {
                            if (Convert.ToInt64(savedAmendmentDataHistories.Where(x => x.FieldName == "PowerKW_Installed").Select(x => x.PreviousValue).FirstOrDefault()) == formModel.PowerKW_Installed)
                            {
                                _context.Licence_Factory_AmendmentDataHistories.Remove(savedAmendmentDataHistories.Where(x => x.FieldName == "PowerKW_Installed").FirstOrDefault());
                            }
                            else
                            {
                                var amdHistory = savedAmendmentDataHistories.Where(x => x.FieldName == "PowerKW_Installed").FirstOrDefault();
                                amdHistory.ModifiedValue = formModel.PowerKW_Installed.ToString();
                                _context.Update<Licence_Factory_AmendmentDataHistory>(amdHistory);
                            }
                            await _context.SaveChangesAsync();
                        }
                        else
                        {
                            if (previousFactoryLicenseData.PowerKW_Installed != formModel.PowerKW_Installed)
                            {
                                amendmentDataHistories.Add(new Licence_Factory_AmendmentDataHistory()
                                {
                                    AppRefId = formModel.AppRefId,
                                    FieldName = "PowerKW_Installed",
                                    PreviousValue = previousFactoryLicenseData.PowerKW_Installed.ToString(),
                                    ModifiedValue = formModel.PowerKW_Installed.ToString(),
                                    SectionCode = "KW", //KILOWATT SECTION
                                    ModifiedOn = DateTime.Now,
                                    ModifiedCounter = formModel.ModifiedCounter
                                });
                            }
                        }
                        if (amendmentDataHistories.Count() > 0)
                        {
                            await _context.BulkInsertAsync<Licence_Factory_AmendmentDataHistory>(amendmentDataHistories);
                        }
                    }
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
        #endregion 

        #region Occupier & Manager Details
        public async Task<GenericFormModel<Licence_Factory_OccupierAndManagerDetail>> GetOccupierAndManagerDetail(Int64 id)
        {
            GenericFormModel<Licence_Factory_OccupierAndManagerDetail> genericFormModel = new GenericFormModel<Licence_Factory_OccupierAndManagerDetail>();
            try
            {
                Int64 appRefId = 0;
                if (id != 0) //Existing Record
                {
                    var parentWithChildObject = await _iGR_Licence_Factory_GeneralDetail
                        .GetAsync(x => x.FactoryLicenceId == id,  //Conditions         
                          null,                //Orders          
                          x => x.Licence_Factory_OccupierAndManagerDetail, x => x.Application)      //Includes
                        .ConfigureAwait(false);
                    genericFormModel.FormModel = parentWithChildObject.FirstOrDefault().Licence_Factory_OccupierAndManagerDetail;

                    genericFormModel.IsEditAllowed = parentWithChildObject.FirstOrDefault().Application.IsAllowEdit;
                    genericFormModel.IsLocked = parentWithChildObject.FirstOrDefault().Application.IsLocked;
                    genericFormModel.ApplicationLifeCycleStatusType = parentWithChildObject.FirstOrDefault().Application.ApplicationLifeCycleStatusType;
                    appRefId = parentWithChildObject.FirstOrDefault().AppRefId;
                }
                else //New Record
                {
                    genericFormModel.FormModel = new Licence_Factory_OccupierAndManagerDetail();

                    //Allow user to edit form
                    genericFormModel.IsEditAllowed = true;
                    genericFormModel.IsLocked = false;
                    genericFormModel.ApplicationLifeCycleStatusType = ApplicationLifeCycleStatusTypeEnum.IN_PROCESS;
                }
                genericFormModel.AppFormStepsList = await _iApplicationMamnagementService.GetAppFormStepperInfo(appRefId, ApplicationTypeEnum.FACTORY_LICENCE, (genericFormModel.FormModel != null ? genericFormModel.FormModel.FactoryLicenceRefId : id), "OD");
            }
            catch (Exception ex)
            {
                genericFormModel.HasError = true;
                genericFormModel.ErrorDesc = ex.Message;
                throw ex;
            }
            return genericFormModel;
        }
        public async Task<GenericServiceResultTemplate> AddUpdate_OccupierAndManagerDetail(Licence_Factory_OccupierAndManagerDetail formModel)
        {
            GenericServiceResultTemplate genericServiceResultTemplate = new GenericServiceResultTemplate();
            try
            {
                genericServiceResultTemplate.HasException = false;
                genericServiceResultTemplate.CustomeValidationResult = CustomeValidator<Licence_Factory_OccupierAndManagerDetail>.ValidateModel_AllProperties(formModel);
                genericServiceResultTemplate.ApplicationInitiateResponse = new ApplicationInitiateResponseViewModel();
                genericServiceResultTemplate.ApplicationInitiateResponse.EntityKeyId = formModel.FactoryLicenceRefId;
                genericServiceResultTemplate.ApplicationInitiateResponse.AppId= formModel.AppRefId;
                if (genericServiceResultTemplate.CustomeValidationResult.IsValid)
                {

                    List<Licence_Factory_AmendmentDataHistory> savedAmendmentDataHistories = null;
                    Licence_Factory_OccupierAndManagerDetail previousFactoryLicenseData = null;

                    if (formModel.OccupierAndManagerDetailId != 0) //Existing record
                    {
                        previousFactoryLicenseData = _context.Licence_Factory_OccupierAndManagerDetails.Where(x => x.OccupierAndManagerDetailId == formModel.OccupierAndManagerDetailId).AsNoTracking().FirstOrDefault();
                        savedAmendmentDataHistories = await _context.Licence_Factory_AmendmentDataHistories.Where(x =>
                        !x.IsLocked &&
                        x.ModifiedCounter == previousFactoryLicenseData.ModifiedCounter &&
                        (x.FieldName == "ManagerFullName" || x.FieldName == "ManagerFatherName" || x.FieldName == "ManagerEmail" || x.FieldName == "ManagerMobile")).ToListAsync();

                        if (savedAmendmentDataHistories.Count() == 0)
                        {
                            formModel.ModifiedCounter = previousFactoryLicenseData.ModifiedCounter + 1;
                        }
                        else
                        {
                            formModel.ModifiedCounter = previousFactoryLicenseData.ModifiedCounter;
                        }

                        _iGR_Licence_Factory_OccupierAndManagerDetail.Update(formModel);
                        await _iGR_Licence_Factory_OccupierAndManagerDetail.SavechangeAsync();

                        //Update last modified date
                        await _iApplicationMamnagementService.UpdateAppLastModifiedDate(formModel.AppRefId);
                    }
                    else //New record
                    {
                        //Set null all navigation properties from model
                        formModel = GenericModelOps<Licence_Factory_OccupierAndManagerDetail>.SetNullAllNevigationProperties(formModel);
                        formModel.ModifiedCounter = 1;
                        _iGR_Licence_Factory_OccupierAndManagerDetail.Insert(formModel);
                        await _iGR_Licence_Factory_OccupierAndManagerDetail.SavechangeAsync();
                    }

                    var application = _context.Applications.Where(x => x.AppId == formModel.AppRefId && x.IsDeleted == false).FirstOrDefault();

                    if (application.ApplicationPurposeType  == ApplicationPurposeTypeEnum.RENEWAL_LICENCE || application.ApplicationPurposeType == ApplicationPurposeTypeEnum.AMENDMENT_LICENCE && previousFactoryLicenseData != null)
                    {
                        List<Licence_Factory_AmendmentDataHistory> amendmentDataHistories = new List<Licence_Factory_AmendmentDataHistory>();

                        if (savedAmendmentDataHistories.Any(x => x.FieldName == "ManagerFullName"))
                        {
                            if (savedAmendmentDataHistories.Where(x => x.FieldName == "ManagerFullName").Select(x => x.PreviousValue).FirstOrDefault() == formModel.ManagerFullName)
                            {
                                _context.Licence_Factory_AmendmentDataHistories.Remove(savedAmendmentDataHistories.Where(x => x.FieldName == "ManagerFullName").FirstOrDefault());
                            }
                            else
                            {
                                var amdHistory = savedAmendmentDataHistories.Where(x => x.FieldName == "ManagerFullName").FirstOrDefault();
                                amdHistory.ModifiedValue = formModel.ManagerFullName.ToString();
                                _context.Update<Licence_Factory_AmendmentDataHistory>(amdHistory);
                            }
                            await _context.SaveChangesAsync();
                        }
                        else
                        {
                            if (previousFactoryLicenseData.ManagerFullName != formModel.ManagerFullName)
                            {
                                amendmentDataHistories.Add(new Licence_Factory_AmendmentDataHistory()
                                {
                                    AppRefId = formModel.AppRefId,
                                    FieldName = "ManagerFullName",
                                    PreviousValue = previousFactoryLicenseData.ManagerFullName.ToString(),
                                    ModifiedValue = formModel.ManagerFullName.ToString(),
                                    SectionCode = "MD", //Manager Detail
                                    ModifiedOn = DateTime.Now,
                                    ModifiedCounter = formModel.ModifiedCounter
                                });
                            }
                        }

                        if (savedAmendmentDataHistories.Any(x => x.FieldName == "ManagerFatherName"))
                        {
                            if (savedAmendmentDataHistories.Where(x => x.FieldName == "ManagerFatherName").Select(x => x.PreviousValue).FirstOrDefault() == formModel.ManagerFatherName)
                            {
                                _context.Licence_Factory_AmendmentDataHistories.Remove(savedAmendmentDataHistories.Where(x => x.FieldName == "ManagerFatherName").FirstOrDefault());
                            }
                            else
                            {
                                var amdHistory = savedAmendmentDataHistories.Where(x => x.FieldName == "ManagerFatherName").FirstOrDefault();
                                amdHistory.ModifiedValue = formModel.ManagerFatherName.ToString();
                                _context.Update<Licence_Factory_AmendmentDataHistory>(amdHistory);
                            }
                            await _context.SaveChangesAsync();
                        }
                        else
                        {
                            if (previousFactoryLicenseData.ManagerFatherName != formModel.ManagerFatherName)
                            {
                                amendmentDataHistories.Add(new Licence_Factory_AmendmentDataHistory()
                                {
                                    AppRefId = formModel.AppRefId,
                                    FieldName = "ManagerFatherName",
                                    PreviousValue = previousFactoryLicenseData.ManagerFatherName.ToString(),
                                    ModifiedValue = formModel.ManagerFatherName.ToString(),
                                    SectionCode = "MD", //Manager Detail
                                    ModifiedOn = DateTime.Now,
                                    ModifiedCounter = formModel.ModifiedCounter
                                });
                            }
                        }

                        if (savedAmendmentDataHistories.Any(x => x.FieldName == "ManagerEmail"))
                        {
                            if (savedAmendmentDataHistories.Where(x => x.FieldName == "ManagerEmail").Select(x => x.PreviousValue).FirstOrDefault() == formModel.ManagerEmail)
                            {
                                _context.Licence_Factory_AmendmentDataHistories.Remove(savedAmendmentDataHistories.Where(x => x.FieldName == "ManagerEmail").FirstOrDefault());
                            }
                            else
                            {
                                var amdHistory = savedAmendmentDataHistories.Where(x => x.FieldName == "ManagerEmail").FirstOrDefault();
                                amdHistory.ModifiedValue = formModel.ManagerEmail.ToString();
                                _context.Update<Licence_Factory_AmendmentDataHistory>(amdHistory);
                            }
                            await _context.SaveChangesAsync();
                        }
                        else
                        {
                            if (previousFactoryLicenseData.ManagerEmail != formModel.ManagerEmail)
                            {
                                amendmentDataHistories.Add(new Licence_Factory_AmendmentDataHistory()
                                {
                                    AppRefId = formModel.AppRefId,
                                    FieldName = "ManagerEmail",
                                    PreviousValue = previousFactoryLicenseData.ManagerEmail.ToString(),
                                    ModifiedValue = formModel.ManagerEmail.ToString(),
                                    SectionCode = "MD", //Manager Detail
                                    ModifiedOn = DateTime.Now,
                                    ModifiedCounter = formModel.ModifiedCounter
                                });
                            }
                        }

                        if (savedAmendmentDataHistories.Any(x => x.FieldName == "ManagerMobile"))
                        {
                            if (savedAmendmentDataHistories.Where(x => x.FieldName == "ManagerMobile").Select(x => x.PreviousValue).FirstOrDefault() == formModel.ManagerMobile)
                            {
                                _context.Licence_Factory_AmendmentDataHistories.Remove(savedAmendmentDataHistories.Where(x => x.FieldName == "ManagerMobile").FirstOrDefault());
                            }
                            else
                            {
                                var amdHistory = savedAmendmentDataHistories.Where(x => x.FieldName == "ManagerMobile").FirstOrDefault();
                                amdHistory.ModifiedValue = formModel.ManagerMobile.ToString();
                                _context.Update<Licence_Factory_AmendmentDataHistory>(amdHistory);
                            }
                            await _context.SaveChangesAsync();
                        }
                        else
                        {
                            if (previousFactoryLicenseData.ManagerMobile != formModel.ManagerMobile)
                            {
                                amendmentDataHistories.Add(new Licence_Factory_AmendmentDataHistory()
                                {
                                    AppRefId = formModel.AppRefId,
                                    FieldName = "ManagerMobile",
                                    PreviousValue = previousFactoryLicenseData.ManagerMobile.ToString(),
                                    ModifiedValue = formModel.ManagerMobile.ToString(),
                                    SectionCode = "MD", //Manager Detail
                                    ModifiedOn = DateTime.Now,
                                    ModifiedCounter = formModel.ModifiedCounter
                                });
                            }
                        }

                        // Occupier Details
                        if (savedAmendmentDataHistories.Any(x => x.FieldName == "OccupierFullName"))
                        {
                            if (savedAmendmentDataHistories.Where(x => x.FieldName == "OccupierFullName").Select(x => x.PreviousValue).FirstOrDefault() == formModel.OccupierFullName)
                            {
                                _context.Licence_Factory_AmendmentDataHistories.Remove(savedAmendmentDataHistories.Where(x => x.FieldName == "OccupierFullName").FirstOrDefault());
                            }
                            else
                            {
                                var amdHistory = savedAmendmentDataHistories.Where(x => x.FieldName == "OccupierFullName").FirstOrDefault();
                                amdHistory.ModifiedValue = formModel.OccupierFullName.ToString();
                                _context.Update<Licence_Factory_AmendmentDataHistory>(amdHistory);
                            }
                            await _context.SaveChangesAsync();
                        }
                        else
                        {
                            if (previousFactoryLicenseData.OccupierFullName != formModel.OccupierFullName)
                            {
                                amendmentDataHistories.Add(new Licence_Factory_AmendmentDataHistory()
                                {
                                    AppRefId = formModel.AppRefId,
                                    FieldName = "OccupierFullName",
                                    PreviousValue = previousFactoryLicenseData.OccupierFullName.ToString(),
                                    ModifiedValue = formModel.OccupierFullName.ToString(),
                                    SectionCode = "OD", //Occupier Details
                                    ModifiedOn = DateTime.Now,
                                    ModifiedCounter = formModel.ModifiedCounter
                                });
                            }
                        }

                        if (savedAmendmentDataHistories.Any(x => x.FieldName == "OccupierFatherName"))
                        {
                            if (savedAmendmentDataHistories.Where(x => x.FieldName == "OccupierFatherName").Select(x => x.PreviousValue).FirstOrDefault() == formModel.OccupierFatherName)
                            {
                                _context.Licence_Factory_AmendmentDataHistories.Remove(savedAmendmentDataHistories.Where(x => x.FieldName == "OccupierFatherName").FirstOrDefault());
                            }
                            else
                            {
                                var amdHistory = savedAmendmentDataHistories.Where(x => x.FieldName == "OccupierFatherName").FirstOrDefault();
                                amdHistory.ModifiedValue = formModel.OccupierFatherName.ToString();
                                _context.Update<Licence_Factory_AmendmentDataHistory>(amdHistory);
                            }
                            await _context.SaveChangesAsync();
                        }
                        else
                        {
                            if (previousFactoryLicenseData.OccupierFatherName != formModel.OccupierFatherName)
                            {
                                amendmentDataHistories.Add(new Licence_Factory_AmendmentDataHistory()
                                {
                                    AppRefId = formModel.AppRefId,
                                    FieldName = "OccupierFatherName",
                                    PreviousValue = previousFactoryLicenseData.OccupierFatherName.ToString(),
                                    ModifiedValue = formModel.OccupierFatherName.ToString(),
                                    SectionCode = "OD", //Manager Detail
                                    ModifiedOn = DateTime.Now,
                                    ModifiedCounter = formModel.ModifiedCounter
                                });
                            }
                        }

                        if (savedAmendmentDataHistories.Any(x => x.FieldName == "OccupierEmail"))
                        {
                            if (savedAmendmentDataHistories.Where(x => x.FieldName == "OccupierEmail").Select(x => x.PreviousValue).FirstOrDefault() == formModel.OccupierEmail)
                            {
                                _context.Licence_Factory_AmendmentDataHistories.Remove(savedAmendmentDataHistories.Where(x => x.FieldName == "OccupierEmail").FirstOrDefault());
                            }
                            else
                            {
                                var amdHistory = savedAmendmentDataHistories.Where(x => x.FieldName == "OccupierEmail").FirstOrDefault();
                                amdHistory.ModifiedValue = formModel.OccupierEmail.ToString();
                                _context.Update<Licence_Factory_AmendmentDataHistory>(amdHistory);
                            }
                            await _context.SaveChangesAsync();
                        }
                        else
                        {
                            if (previousFactoryLicenseData.OccupierEmail != formModel.OccupierEmail)
                            {
                                amendmentDataHistories.Add(new Licence_Factory_AmendmentDataHistory()
                                {
                                    AppRefId = formModel.AppRefId,
                                    FieldName = "OccupierEmail",
                                    PreviousValue = previousFactoryLicenseData.OccupierEmail.ToString(),
                                    ModifiedValue = formModel.OccupierEmail.ToString(),
                                    SectionCode = "OD", //Manager Detail
                                    ModifiedOn = DateTime.Now,
                                    ModifiedCounter = formModel.ModifiedCounter
                                });
                            }
                        }

                        if (savedAmendmentDataHistories.Any(x => x.FieldName == "OccupierMobile"))
                        {
                            if (savedAmendmentDataHistories.Where(x => x.FieldName == "OccupierMobile").Select(x => x.PreviousValue).FirstOrDefault() == formModel.OccupierMobile)
                            {
                                _context.Licence_Factory_AmendmentDataHistories.Remove(savedAmendmentDataHistories.Where(x => x.FieldName == "OccupierMobile").FirstOrDefault());
                            }
                            else
                            {
                                var amdHistory = savedAmendmentDataHistories.Where(x => x.FieldName == "OccupierMobile").FirstOrDefault();
                                amdHistory.ModifiedValue = formModel.OccupierMobile.ToString();
                                _context.Update<Licence_Factory_AmendmentDataHistory>(amdHistory);
                            }
                            await _context.SaveChangesAsync();
                        }
                        else
                        {
                            if (previousFactoryLicenseData.OccupierMobile != formModel.OccupierMobile)
                            {
                                amendmentDataHistories.Add(new Licence_Factory_AmendmentDataHistory()
                                {
                                    AppRefId = formModel.AppRefId,
                                    FieldName = "OccupierMobile",
                                    PreviousValue = previousFactoryLicenseData.OccupierMobile.ToString(),
                                    ModifiedValue = formModel.OccupierMobile.ToString(),
                                    SectionCode = "OD", //Occupier Detail
                                    ModifiedOn = DateTime.Now,
                                    ModifiedCounter = formModel.ModifiedCounter
                                });
                            }
                        }

                        if (amendmentDataHistories.Count() > 0)
                        {
                            await _context.BulkInsertAsync<Licence_Factory_AmendmentDataHistory>(amendmentDataHistories);
                        }
                    }
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

        #endregion

        #region Factory Licence Details
        public async Task<GenericFormModel<FactoryLicenceViewModel>> GetFactoryLicenceDetail(long id)
        {
            GenericFormModel<FactoryLicenceViewModel> genericFormModel = new GenericFormModel<FactoryLicenceViewModel>();
            try
            {
                genericFormModel.ListTemplateLists = new List<ListTemplate>();
                genericFormModel.FormModel = new FactoryLicenceViewModel();
                genericFormModel.FormModel.GeneralDetail = new Licence_Factory_GeneralDetail();
                genericFormModel.FormModel.Licence_Factory_AmendmentDataHistory = new List<Licence_Factory_AmendmentDataHistory>();

                var parentWithChildObject = await _iGR_Licence_Factory_GeneralDetail
                        .GetAsync(x => x.AppRefId == id,  //Conditions         
                          null,                //Orders
                          x => x.Licence_Factory_OccupierAndManagerDetail,
                          x => x.Application)      //Includes
                        .ConfigureAwait(false);

                var amendmentHistory = _context.Licence_Factory_AmendmentDataHistories.Where(x => x.AppRefId == id).ToList();

                genericFormModel.FormModel.GeneralDetail = parentWithChildObject.FirstOrDefault();
                genericFormModel.FormModel.Licence_Factory_AmendmentDataHistory = amendmentHistory;

                genericFormModel.FormModel.IsLocked = parentWithChildObject.FirstOrDefault().Application.IsLocked;
                genericFormModel.FormModel.IsFeeApplicable = parentWithChildObject.FirstOrDefault().Application.IsFeeApplicable;
                genericFormModel.ApplicationLifeCycleStatusType = parentWithChildObject.FirstOrDefault().Application.ApplicationLifeCycleStatusType;

                genericFormModel.AppFormStepsList = new List<AppFormStepsInfo>();
                genericFormModel.AppFormStepsList = await _iApplicationMamnagementService.GetAppFormStepperInfo(id, ApplicationTypeEnum.FACTORY_LICENCE, (genericFormModel.FormModel != null ? genericFormModel.FormModel.GeneralDetail.FactoryLicenceId : id), "LOCK");

                var siteId = await _iProjectSiteService.GetProjectSiteRefIdByAppId(id);
                var projectProfileInfo = await _iProjectSiteService.GetProjectSiteDetail(siteId.ResponseDataModel);
                genericFormModel.FormModel.GeneralDetail.FactoryCircleRefId = projectProfileInfo.FormModel.FactoryCircleRefId;
                genericFormModel.FormModel.GeneralDetail.DistrictRefId = projectProfileInfo.FormModel.DistrictRefId;
            }
            catch (Exception ex)
            {
                genericFormModel.HasError = true;
                genericFormModel.ErrorDesc = ex.Message;
            }
            return genericFormModel;
        }
        #endregion 

        #region Lock Application
        public async Task<GenericServiceResultTemplate> LockApplication(Int64 appRefId, int AppActionType, string remarks)
        {
            GenericServiceResultTemplate genericServiceResultTemplate = new GenericServiceResultTemplate();
            try
            {
                genericServiceResultTemplate.HasException = false;
                genericServiceResultTemplate.CustomeValidationResult = new CustomeValidationResult();
                genericServiceResultTemplate.CustomeValidationResult.IsValid = true;
                genericServiceResultTemplate.ApplicationInitiateResponse = new ApplicationInitiateResponseViewModel();
                if (appRefId != 0)
                {
                    var resp = await _iApplicationMamnagementService.LockApplication(appRefId, AppActionType, remarks, ApplicationTypeEnum.FACTORY_LICENCE);
                    if (resp != "" && resp != "NAN")
                    {
                        genericServiceResultTemplate.ApplicationInitiateResponse.IsApplicationCreated = true;

                        var generalDetails = _context.Licence_Factory_GeneralDetails.Where(x => x.AppRefId == appRefId).FirstOrDefault();
                        var ocupierDetails = _context.Licence_Factory_OccupierAndManagerDetails.Where(x => x.FactoryLicenceRefId == generalDetails.FactoryLicenceId).FirstOrDefault();

                        var amenmentHistories = await _context.Licence_Factory_AmendmentDataHistories.Where(x => x.AppRefId == appRefId &&
                        x.ModifiedCounter == generalDetails.ModifiedCounter ||
                        x.ModifiedCounter == ocupierDetails.ModifiedCounter).ToListAsync();
                        amenmentHistories = amenmentHistories.Select(x => { x.IsLocked = true; return x; }).ToList();

                        await _context.BulkUpdateAsync<Licence_Factory_AmendmentDataHistory>(amenmentHistories);
                        // Share status to Invest Punjab Portal
                        // await _iThirdPartyInegrationsService.ShareStatusToBusinessFirst(appRefId, ApplicationTypeEnum.FACTORY_LICENCE, (AppActionTypeEnum)AppActionType);
                    }
                    else if (resp == "NAN")
                    {
                        genericServiceResultTemplate.ApplicationInitiateResponse.IsApplicationCreated = true;
                        genericServiceResultTemplate.CustomeValidationResult.IsValid = true;
                    }
                    else
                    {
                        genericServiceResultTemplate.ApplicationInitiateResponse.IsApplicationCreated = false;
                        genericServiceResultTemplate.CustomeValidationResult.IsValid = false;
                    }
                }
                else
                {
                    genericServiceResultTemplate.CustomeValidationResult.IsValid = false;
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
        #endregion Lock Application

        public async Task<GenericServiceResultTemplate> AddUpdate_Questionnairedetails(Licence_Factory_QuestionnaireDetail formModel, string id)
        {
            GenericServiceResultTemplate genericServiceResultTemplate = new GenericServiceResultTemplate();
            try
            {
                genericServiceResultTemplate.HasException = false;
                genericServiceResultTemplate.CustomeValidationResult = CustomeValidator<Licence_Factory_QuestionnaireDetail>.ValidateModel_AllProperties(formModel);
                if (genericServiceResultTemplate.CustomeValidationResult.IsValid)
                {
                    if (formModel.QuestionnaireDetailId != 0) //Existing record
                    {
                        _iGR_Licence_Factory_QuestionnaireDetail.Update(formModel);
                        await _iGR_Licence_Factory_QuestionnaireDetail.SavechangeAsync();

                        //Update last modified date
                        await _iApplicationMamnagementService.UpdateAppLastModifiedDate(formModel.AppRefId);
                        genericServiceResultTemplate.ApplicationInitiateResponse = new ApplicationInitiateResponseViewModel();
                        genericServiceResultTemplate.ApplicationInitiateResponse.EntityKeyId = formModel.QuestionnaireDetailId;
                        genericServiceResultTemplate.ApplicationInitiateResponse.AppId = formModel.AppRefId;

                        // Check has status share to business first
                        var isStatusShared = await _iThirdPartyInegrationsService.HasShareStatusToBusinessFirst(genericServiceResultTemplate.ApplicationInitiateResponse.AppId);
                        if (isStatusShared != null && !isStatusShared.HasError && !isStatusShared.ResponseDataModel)
                        {
                            // Update Native Appid by IPIN
                            await _iThirdPartyInegrationsService.UpdateBusinessFirstRequestNativeAppIdByIPin(formModel.IPin, formModel.InvestPunjab_AppId, genericServiceResultTemplate.ApplicationInitiateResponse.AppId, ApplicationTypeEnum.FACTORY_LICENCE, formModel.ApplicationPurposeType);

                            // Share status to Invest Punjab Portal
                            await _iThirdPartyInegrationsService.ShareStatusToBusinessFirst(genericServiceResultTemplate.ApplicationInitiateResponse.AppId, ApplicationTypeEnum.FACTORY_LICENCE, AppActionTypeEnum.APP_SAVE_DRAFT);
                        }

                    }
                    else //New record
                    {
                        //Set null all navigation properties from model
                        formModel = GenericModelOps<Licence_Factory_QuestionnaireDetail>.SetNullAllNevigationProperties(formModel);
                        genericServiceResultTemplate.ApplicationInitiateResponse = await _iApplicationMamnagementService_QuestionnaireDetail.InitiateApplication(ApplicationTypeEnum.FACTORY_LICENCE, formModel, formModel.ProjectSiteRefId, formModel.ApplicationPurposeType, id, formModel.IPin, formModel.InvestPunjab_AppId, false, 0, 0, null, null, formModel.ProjectSiteVersion);

                        if (genericServiceResultTemplate.ApplicationInitiateResponse.IsApplicationCreated == true)
                        {
                            // Update Native Appid by IPIN
                            await _iThirdPartyInegrationsService.UpdateBusinessFirstRequestNativeAppIdByIPin(formModel.IPin, formModel.InvestPunjab_AppId, genericServiceResultTemplate.ApplicationInitiateResponse.AppId, ApplicationTypeEnum.FACTORY_LICENCE, formModel.ApplicationPurposeType);

                            // Share status to Invest Punjab Portal
                            await _iThirdPartyInegrationsService.ShareStatusToBusinessFirst(genericServiceResultTemplate.ApplicationInitiateResponse.AppId, ApplicationTypeEnum.FACTORY_LICENCE, AppActionTypeEnum.APP_SAVE_DRAFT);

                            

                            if(formModel.IsTempRegistered == 1 && formModel.IsTempRegistrationVerified)
                            {
                                var application = await _context.Applications.Where(x => x.AppId == genericServiceResultTemplate.ApplicationInitiateResponse.AppId && x.IsDeleted == false).FirstOrDefaultAsync();
                                application.IsFeeApplicable = false;
                                _context.Applications.Update(application);
                                await _context.SaveChangesAsync();

                                GenericResponseTemplateModel<Factory_TemporaryLicenceDetailsViewModel> genericFormModel = await _iThirdPartyInegrationsService.GetTemporaryLicenseDetails(formModel.TempRegistrationNumber);

                                string exstingStr = genericFormModel.ResponseDataModel.NICCode;
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
                                    OldLicenceNo = genericFormModel.ResponseDataModel.OLNo == null ? "NA" : genericFormModel.ResponseDataModel.OLNo,
                                    OldLicenceValidUpTo = genericFormModel.ResponseDataModel.OLValidUpto,
                                    OldLicenceTotalEmployees = genericFormModel.ResponseDataModel.OLEmps,
                                    OldLicenceFactoryKiloWatt = genericFormModel.ResponseDataModel.OLKiloWatt,
                                    RegistrationDate = genericFormModel.ResponseDataModel.RegistrationDate,
                                    RenewalFromDate = genericFormModel.ResponseDataModel.ApplicationDate,
                                    NoOfYears = genericFormModel.ResponseDataModel.LicenceForNoOfYear,
                                    ManufacturingProcess_Last12Months = "NA",
                                    ManufacturingProcess_Next12Months = "NA",
                                    NationalIndustrialClassificationCode = exstingStr,
                                    MfgProducts_Last12Month = genericFormModel.ResponseDataModel.MFProc != null ? genericFormModel.ResponseDataModel.MFProc : "NA",
                                    Workers_MaxDuringYear = genericFormModel.ResponseDataModel.MaximumNumberEmployeeInYear,
                                    Workers_MaxLast12Month = genericFormModel.ResponseDataModel.MaximumNumberEmployeeLastYear,
                                    Workers_OrdinarilyEmployed = genericFormModel.ResponseDataModel.OrdinarilyEmployed,
                                    PowerKW_Installed = genericFormModel.ResponseDataModel.InstalledPower,
                                    PowerKW_MaxProposed = genericFormModel.ResponseDataModel.MaximumPowerUsed,
                                    ModifiedCounter = 1,
                                    AppRefId = genericServiceResultTemplate.ApplicationInitiateResponse.AppId,
                                };

                                await _context.Licence_Factory_GeneralDetails.AddAsync(formData);
                                await _context.SaveChangesAsync();

                                Licence_Factory_OccupierAndManagerDetail occupierDetails = new Licence_Factory_OccupierAndManagerDetail()
                                {
                                    ManagerFullName = genericFormModel.ResponseDataModel.ManagerFullName,
                                    ManagerFatherName = genericFormModel.ResponseDataModel.ManagerFatherName,
                                    ManagerFullAddress = genericFormModel.ResponseDataModel.ManagerFullAddress,
                                    ManagerMobile = genericFormModel.ResponseDataModel.ManagerMobile == null ? "NA" : genericFormModel.ResponseDataModel.ManagerMobile,
                                    ManagerEmail = genericFormModel.ResponseDataModel.ManagerEmail == null ? "NA" : genericFormModel.ResponseDataModel.ManagerEmail,
                                    ManagerResidentialAddress = genericFormModel.ResponseDataModel.ManagerFullAddress,
                                    OccupierFullName = genericFormModel.ResponseDataModel.OccupierFullName,
                                    OccupierFatherName = genericFormModel.ResponseDataModel.OccupierFatherName,
                                    OccupierFullAddress = genericFormModel.ResponseDataModel.OccupierFullAddress,
                                    OccupierMobile = genericFormModel.ResponseDataModel.OccupierMobile == null ? "NA" : genericFormModel.ResponseDataModel.OccupierMobile,
                                    OccupierEmail = genericFormModel.ResponseDataModel.OccupierEmail == null ? "NA" : genericFormModel.ResponseDataModel.OccupierEmail,
                                    OccupierResidentialAddress = genericFormModel.ResponseDataModel.OccupierFullAddress,
                                    OwnerName = genericFormModel.ResponseDataModel.OwnerName,
                                    OwnerPremisesAddress = genericFormModel.ResponseDataModel.OwnerPremisesAddress,
                                    StabilityCertificateNumber = "NA",
                                    StabilityCertificateDate = genericFormModel.ResponseDataModel.RegistrationDate,
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
                            }
                            
                        }
                        else if (genericServiceResultTemplate.ApplicationInitiateResponse.IsApplicationCreated == false)
                        {
                            genericServiceResultTemplate.HasException = true;
                            genericServiceResultTemplate.Exceptions = new Exception();
                        }
                    }
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

        public async Task<GenericFormModel<AnnualReturnWelfareFundViewModel>> VerifyWelfareFundAndReturn(string licenceNumber, Int64 appRefId)
        {
            GenericFormModel<AnnualReturnWelfareFundViewModel> genericFormModel = new GenericFormModel<AnnualReturnWelfareFundViewModel>();
            try
            {
                List<StoreProcedureParm> storeProcedureParms = new List<StoreProcedureParm>()
                {
                    new StoreProcedureParm (){ ParmName="LicenceNumber", ParmValue=licenceNumber, isNumber=false},
                    new StoreProcedureParm (){ ParmName="AppRefId", ParmValue=appRefId.ToString(), isNumber=true}
                };
               var welfareFundReturnDetails  = await _iGeneric_SP_Repository.CallStoreProcedureReaderAsync<AnnualReturnWelfareFundViewModel>("sp_CheckAnnualReturnAndWelfareFundDetails", storeProcedureParms);

                genericFormModel.FormModel = welfareFundReturnDetails.FirstOrDefault();
            }
            catch (Exception ex)
            {
                genericFormModel.HasError = true;
                genericFormModel.ErrorDesc = ex.Message;
                throw ex;
            }
            return genericFormModel;
        }

        public async Task<GeneratePdfServiceResultTemplate> GenerateApplicationFormPdf(Int64 appRefId)
        {
            GeneratePdfServiceResultTemplate genericFormModel = new GeneratePdfServiceResultTemplate();
            try
            {
                Guid guId = Guid.NewGuid();
                genericFormModel.PdfNameGUID = guId.ToString();
                genericFormModel.HasException = false;
                var SaveDirectoryPath = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "TempFiles")).Root + guId.ToString() + ".pdf";
                iTextSharp.text.Document documents = new iTextSharp.text.Document(iTextSharp.text.PageSize.A4, 5, 5, 30, 30);

                #region ClassObjects
                pdfFonts classFonts = new pdfFonts();
                #endregion

                #region CreateObjects
                PdfWriter owriter = PdfWriter.GetInstance(documents, new FileStream(SaveDirectoryPath, FileMode.Create));
                PDFPageEvents pageEvent = new PDFPageEvents();
                owriter.PageEvent = pageEvent;
                owriter.SetEncryption(PdfWriter.STRENGTH128BITS, "", "", PdfWriter.AllowPrinting);
                #endregion

                #region Regester Font Style
                var fontName = "Alice";
                Font font = FontFactory.GetFont(fontName, BaseFont.IDENTITY_H, 16);
                Font fontSmall = FontFactory.GetFont(fontName, BaseFont.IDENTITY_H, 12);
                Font fontBOLD = FontFactory.GetFont(fontName, BaseFont.IDENTITY_H, 16, iTextSharp.text.Font.BOLD);
                #endregion
                
                documents.Open();
                Font verdanaHeader = FontFactory.GetFont("Arial", 9, Font.BOLD);
                Font verdana = FontFactory.GetFont("Arial", 9, BaseColor.BLACK);
                Font verdanaBoldItalic = FontFactory.GetFont("Arial", 8, Font.BOLDITALIC);
                Font verdanasmall = FontFactory.GetFont("Arial", 8, Font.NORMAL);
                verdanasmall.Color = BaseColor.WHITE;
                Font verdanaRightHeader = FontFactory.GetFont("Arial", 15, Font.BOLD);
                verdanaRightHeader.Color = BaseColor.WHITE;
                Font verdanaRightHeader1 = FontFactory.GetFont("Arial", 12, Font.BOLD);
                verdanaRightHeader1.Color = BaseColor.WHITE;
                Font verdanaRightHeader2 = FontFactory.GetFont("Arial", 9, Font.BOLD);
                verdanaRightHeader2.Color = BaseColor.WHITE;
                Font monotype = FontFactory.GetFont("Arial", 13, Font.BOLD);
                Font monotypeHeader = FontFactory.GetFont("Arial", 15, Font.BOLD);
                Font impact = new Font(fontSmall.BaseFont, 12);
                Font impactHeader = new Font(fontSmall.BaseFont, 12);
                impactHeader.Color = BaseColor.WHITE;
                BaseFont bfTimes = BaseFont.CreateFont(BaseFont.TIMES_ROMAN, BaseFont.CP1252, false);
                Font timesRoman = FontFactory.GetFont("Arial", 10, BaseColor.BLACK);
                Font verdanaBold = FontFactory.GetFont("Arial", 9, Font.BOLD);

                PdfPTable factoryAnnexure = new PdfPTable(2);
                factoryAnnexure.DefaultCell.Border = 0;
                factoryAnnexure.DefaultCell.Padding = 2;

                BaseColor baseColor = new BaseColor(0, 128, 128); // RGB values for header background color

                #region PDF Content
                factoryAnnexure.AddCell(new PdfPCell(new Phrase("Form-2 Factory License under Punjab Factory Rules 1952.The Factories Act, 1948", verdanaHeader)) { Padding = 20, Colspan = 7, Border = 0, Left = 40, BackgroundColor = baseColor });

                // Fetch data from the database
                var application = _context.Applications.Where(x => x.AppId == appRefId && x.IsDeleted == false).Include(x=> x.ProjectSites).FirstOrDefault();
                var factoryLicence = _context.Licence_Factory_GeneralDetails
                                    .Where(x => x.AppRefId == appRefId)
                                    .Include(x => x.Licence_Factory_OccupierAndManagerDetail)
                                    .FirstOrDefault();

                var applicantDetails = _context.UserProfileMapping.Where(x => x.UserRefId == application.ProjectSites.UserRefId & x.IsActive == true).Include(x => x.UserProfile).FirstOrDefault();
                var feeDetails = _context.AppFeeTransactions.Where(x => x.AppRefId == appRefId && x.TransactionFinalStatusType == TransactionFinalStatusTypeEnum.SUCCEED).FirstOrDefault();

                factoryAnnexure.AddCell(new PdfPCell(new Phrase("Establishment Name :")) { Padding = 1, Border = 1, PaddingTop = 20 });
                factoryAnnexure.AddCell(new PdfPCell(new Phrase(application.ProjectSites.EstablishmentName?.ToString() ?? "")) { Padding = 1, Border = 1, PaddingTop = 20 });

                factoryAnnexure.AddCell(new PdfPCell(new Phrase("Establishment Address :")) { Padding = 1, Border = 1 });
                factoryAnnexure.AddCell(new PdfPCell(new Phrase(application.ProjectSites.Address?.ToString() ?? "")) { Padding = 1, Border = 1 });

                factoryAnnexure.AddCell(new PdfPCell(new Phrase("File No :")) { Padding = 1, Border = 1 });
                factoryAnnexure.AddCell(new PdfPCell(new Phrase(application.PublicAppRefNum?.ToString() ?? "")) { Padding = 1, Border = 1 });

                factoryAnnexure.AddCell(new PdfPCell(new Phrase("Applicant Name :")) { Padding = 1, Border = 1 });
                factoryAnnexure.AddCell(new PdfPCell(new Phrase(applicantDetails.UserProfile.FirstName + ' ' + applicantDetails.UserProfile.FirstName.ToString() ?? "")) { Padding = 1, Border = 1 });

                factoryAnnexure.AddCell(new PdfPCell(new Phrase("Mobile No :")) { Padding = 1, Border = 1 });
                factoryAnnexure.AddCell(new PdfPCell(new Phrase(applicantDetails.UserProfile.MobileNo?.ToString() ?? "")) { Padding = 1, Border = 1 });

                factoryAnnexure.AddCell(new PdfPCell(new Phrase("Email :")) { Padding = 1, Border = 1 });
                factoryAnnexure.AddCell(new PdfPCell(new Phrase(applicantDetails.UserProfile.Email?.ToString() ?? "")) { Padding = 1, Border = 1 });

                factoryAnnexure.AddCell(new PdfPCell(new Phrase("Processing Fee Paid :")) { Padding = 1, Border = 1 });
                var amount = feeDetails.AmountCalculated != 0 ? Convert.ToDecimal(feeDetails.AmountCalculated).ToString() : "";
                factoryAnnexure.AddCell(new PdfPCell(new Phrase(amount)) { Padding = 1, Border = 1 });


                factoryAnnexure.AddCell(new PdfPCell(new Phrase("Transaction Date :")) { Padding = 1, Border = 1 });
                factoryAnnexure.AddCell(new PdfPCell(new Phrase(feeDetails.TransactionInitializationDate.ToString() ?? "")) { Padding = 1, Border = 1 });

                // Iterate over properties using reflection
                var properties = factoryLicence.GetType().GetProperties();

                foreach (var item in properties)
                {
                    // Get the name and value of the property
                    string propertyName = item.Name;
                    object propertyValue = item.GetValue(factoryLicence);

                    // Skip properties that should be excluded
                    if (IsPropertyExcluded(propertyName, application.ApplicationPurposeType))
                    {
                        continue;
                    }

                    // Transform boolean values to "True" or "False"
                    if (item.PropertyType == typeof(bool))
                    {
                        if (propertyValue is bool)
                        {
                            propertyValue = (bool)propertyValue ? "Yes" : "No";
                        }
                        else if (propertyValue is string && (string)propertyValue == "1" || (int)propertyValue == 1)
                        {
                            propertyValue = "Yes";
                        }
                        else if (propertyValue is string && (string)propertyValue == "0" || (int)propertyValue == 0)
                        {
                            propertyValue = "No";
                        }
                    }

                    // Add cell to the table dynamically
                    factoryAnnexure.AddCell(new PdfPCell(new Phrase(propertyName + ":")) { Padding = 1, Border = 1 });
                    factoryAnnexure.AddCell(new PdfPCell(new Phrase(propertyValue?.ToString() ?? "")) { Padding = 1, Border = 1 });
                }

                // Include properties from the Licence_Factory_OccupierAndManagerDetail
                if (factoryLicence.Licence_Factory_OccupierAndManagerDetail != null)
                {
                    var occupierProperties = factoryLicence.Licence_Factory_OccupierAndManagerDetail.GetType().GetProperties();

                    foreach (var occupierItem in occupierProperties)
                    {
                        // Get the name and value of the property
                        string occupierPropertyName = occupierItem.Name;
                        object occupierPropertyValue = occupierItem.GetValue(factoryLicence.Licence_Factory_OccupierAndManagerDetail);

                        // Skip properties that should be excluded
                        if (IsPropertyExcluded(occupierPropertyName, application.ApplicationPurposeType))
                        {
                            continue;
                        }

                        // Add cell to the table dynamically
                        factoryAnnexure.AddCell(new PdfPCell(new Phrase(occupierPropertyName + ":")) { Padding = 1, Border = 1 });
                        factoryAnnexure.AddCell(new PdfPCell(new Phrase(occupierPropertyValue?.ToString() ?? "")) { Padding = 1, Border = 1 });
                    }
                }


                // Signature of occupier & manager
                var signature = _context.ApplicationDocuments
                             .Where(x => (x.AppRefId == appRefId && x.DocumentRefId == 20005) || (x.AppRefId == appRefId && x.DocumentRefId == 50016))
                             .OrderByDescending(x => x.AppDocId)
                             .Select(x => new
                             {
                                 AttachmentName = x.AttachmentName,
                                 FilePath = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "AppFiles")).Root + x.AttachmentName,
                                 DocumentRefId = x.DocumentRefId
                             })
                             .ToList();

                var occupierSign = signature.FirstOrDefault(x => x.DocumentRefId == 20005); // Occupier
                var managerSign = signature.FirstOrDefault(x => x.DocumentRefId == 50016);  // Manager

                var occupierSigFilePath = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "AppFiles")).Root + occupierSign.AttachmentName;

                iTextSharp.text.Image occupierSignature = iTextSharp.text.Image.GetInstance(new FileStream(occupierSigFilePath, FileMode.OpenOrCreate, FileAccess.Read));
                occupierSignature.ScaleToFit(120f, 120f);
                occupierSignature.SetAbsolutePosition(630f, 150f);
                documents.Add(occupierSignature);

                var managerSigFilePath = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "AppFiles")).Root + managerSign.AttachmentName;

                iTextSharp.text.Image managerSignature = iTextSharp.text.Image.GetInstance(new FileStream(managerSigFilePath, FileMode.OpenOrCreate, FileAccess.Read));
                managerSignature.SetAbsolutePosition(630f, 300f);
                documents.Add(managerSignature);

                factoryAnnexure.AddCell(new PdfPCell(new Phrase("Signature of Occupier")) { Padding = 1, Border = 1 });
                factoryAnnexure.AddCell(new PdfPCell(occupierSignature) { Border = 0, Colspan = 2, PaddingTop = 0 });

                factoryAnnexure.AddCell(new PdfPCell(new Phrase("Signature of Manager")) { Padding = 1, Border = 1 });
                factoryAnnexure.AddCell(new PdfPCell(managerSignature) { Border = 0, Colspan = 2, PaddingTop = 0, HorizontalAlignment = PdfPCell.ALIGN_RIGHT });

                documents.Add(factoryAnnexure);

                #endregion

                documents.NewPage();
                documents.Close();
                PDFPageEvents pageeventsobj = new PDFPageEvents();


                string pdfFilePath = SaveDirectoryPath;
                Byte[] bytes = File.ReadAllBytes(SaveDirectoryPath);
                String file = Convert.ToBase64String(bytes);
                genericFormModel.PdfContent = file;
                genericFormModel.FileNo = application.PublicAppRefNum;

                // Construct the file path
                string filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "TempFiles", guId + ".pdf");

                // Check if the file exists before attempting to delete
                if (File.Exists(filePath))
                {
                    try
                    {
                        // Delete the file
                        File.Delete(filePath);
                    }
                    catch (IOException ex)
                    {
                        genericFormModel.HasException = true;
                        genericFormModel.Exceptions = ex.Message;
                    }
                }
            }
            catch (Exception ex)
            {
                genericFormModel.HasException = true;
                genericFormModel.Exceptions = ex.Message;
                throw ex;
            }
            return genericFormModel;
        }

        // Function to determine if a property should be Excluded in the PDF
        private bool IsPropertyExcluded(string propertyName, ApplicationPurposeTypeEnum applicationPurposeType)
        {
            // List of property names to be excluded
            List<string> excludedProperties = new List<string>();

            if (applicationPurposeType == ApplicationPurposeTypeEnum.GRANT_LICENCE)
            {
                excludedProperties.AddRange(new List<string> {
                "FactoryLicenceId", "OldLicenceValidUpTo", "OldLicenceNo", "AppRefId",
                "OldLicenceFactoryKiloWatt", "OldLicenceTotalEmployees", "RenewalFromDate",
                "ModifiedCounter", "CompetentPersonUserId","Application","Licence_Factory_OccupierAndManagerDetail","ProjectSiteRefId",
                "ApplicationPurposeType","InvestPunjab_Ipin","InvestPunjab_AppId","CompetentPersonList","RenewalFromDate_Json","RegistrationDate_Json",
                "OldLicenceValidUpTo_Json","FactoryCircleRefId","DistrictRefId","IPin"
                });
            }
            else if (applicationPurposeType == ApplicationPurposeTypeEnum.RENEWAL_LICENCE || applicationPurposeType == ApplicationPurposeTypeEnum.AMENDMENT_LICENCE)
            {
                excludedProperties.AddRange(new List<string> {
                "FactoryLicenceId", "AppRefId", "ModifiedCounter", "CompetentPersonUserId","Application","Licence_Factory_OccupierAndManagerDetail",
                "ProjectSiteRefId","ApplicationPurposeType","InvestPunjab_Ipin","InvestPunjab_AppId","CompetentPersonList","RenewalFromDate_Json",
                "RegistrationDate_Json","OldLicenceValidUpTo_Json","FactoryCircleRefId","DistrictRefId","IPin"
                });
            }

            excludedProperties.AddRange(new List<string> {
                "Licence_Factory_GeneralDetail", 
                "AppRefId",
                "FactoryLicenceRefId"
             });

            // Check if the property name is in the list of excluded properties
            return excludedProperties.Contains(propertyName);
        }
    }
}