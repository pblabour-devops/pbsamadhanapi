using Microsoft.Extensions.Configuration;
using EFCore.BulkExtensions;
using Newtonsoft.Json;
using pbsamadhannetcoreapi.CommonUtiliteis;
using pbsamadhannetcoreapi.Models;
using pbsamadhannetcoreapi.Repositories.Implementations;
using pbsamadhannetcoreapi.Services.Implementations;
using pbsamadhannetcoreapi.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace pbsamadhannetcoreapi.Services
{
    public class BuildingPlanHUDService : IBuildingPlanHUDService
    {
        private readonly IGenericRepository<BuildingPlanHUD_GeneralDetail> _iGR_BuildingPlanHUD_GeneralDetail;
        private readonly IApplicationManagementService<BuildingPlanHUD_GeneralDetail> _iApplicationMamnagementService;
        private readonly IGenericRepository<BuildingPlanHUDPaymentDetail> _iGR_AppFeeDetail;
        private readonly IPaymentManagerRepository _iPaymentManagerRepository;
        private readonly IPaymentManagerService _iPaymentManagerService;
        private readonly IBuildingPlanHUDRepository _iBuildingPlanHUDRepository;
        private readonly IThirdPartyInegrationsService _iThirdPartyInegrationsService;
        private readonly IGenericRepository<BuildingPlanHUD_RTB_Mapping> _iGR_BuildingPlanHUD_RTB_Mapping;
        private readonly AppDbContext _context;
        private IAuthService _iAuthService;
        private IConfiguration _iConfiguration { get; }
        public BuildingPlanHUDService(IGenericRepository<BuildingPlanHUD_GeneralDetail> iGR_BuildingPlanHUD_GeneralDetail,
             IApplicationManagementService<BuildingPlanHUD_GeneralDetail> IApplicationMamnagementService,
             IGenericRepository<BuildingPlanHUDPaymentDetail> iGR_AppFeeDetail,
             IPaymentManagerRepository iPaymentManagerRepository,
             IPaymentManagerService iPaymentManagerService,
             IBuildingPlanHUDRepository iBuildingPlanHUDRepository,
             IThirdPartyInegrationsService iThirdPartyInegrationsService,
             IGenericRepository<BuildingPlanHUD_RTB_Mapping> iGR_BuildingPlanHUD_RTB_Mapping,
             AppDbContext context,
             IAuthService authService,
             IConfiguration iConfiguration)
        {
            _iGR_BuildingPlanHUD_GeneralDetail = iGR_BuildingPlanHUD_GeneralDetail;
            _iApplicationMamnagementService = IApplicationMamnagementService;
            _iGR_AppFeeDetail = iGR_AppFeeDetail;
            _iPaymentManagerRepository = iPaymentManagerRepository;
            _iPaymentManagerService = iPaymentManagerService;
            _iBuildingPlanHUDRepository = iBuildingPlanHUDRepository;
            _iThirdPartyInegrationsService = iThirdPartyInegrationsService;
            _iGR_BuildingPlanHUD_RTB_Mapping = iGR_BuildingPlanHUD_RTB_Mapping;
            _context = context;
            _iAuthService = authService;
            _iConfiguration = iConfiguration;
        }
        public async Task<GenericFormModel<BuildingPlanHUD_GeneralDetail>> GetBuildingPlanHUDGeneralDetail(long id, long projectSiteId)
        {
            GenericFormModel<BuildingPlanHUD_GeneralDetail> genericFormModel = new GenericFormModel<BuildingPlanHUD_GeneralDetail>();
            try
            {
                genericFormModel.ListTemplateLists = new List<ListTemplate>();
                if (id != 0) //Existing Record
                {
                    var parentWithChildObject = await _iGR_BuildingPlanHUD_GeneralDetail
                       .GetAsync(x => x.AppRefId == id,  //Conditions         
                         null,                //Orders          
                         x => x.Application, x => x.Application.ApplicationAction)      //Includes
                       .ConfigureAwait(false);
                    genericFormModel.FormModel = parentWithChildObject.FirstOrDefault();

                    genericFormModel.IsEditAllowed = genericFormModel.FormModel.Application.IsAllowEdit;
                    genericFormModel.IsLocked = genericFormModel.FormModel.Application.IsLocked;
                    genericFormModel.ApplicationLifeCycleStatusType = genericFormModel.FormModel.Application.ApplicationLifeCycleStatusType;
                    genericFormModel.FormModel.IPin =Convert.ToInt64(genericFormModel.FormModel.Application.InvestPunjab_Ipin);
                    genericFormModel.FormModel.InvestPunjab_AppId = genericFormModel.FormModel.Application.InvestPunjab_AppId;
                }
                
                else //New Record
                {
                    genericFormModel.FormModel = new BuildingPlanHUD_GeneralDetail();

                    //Allow user to edit form
                    genericFormModel.IsEditAllowed = true;
                    genericFormModel.IsLocked = false;
                    genericFormModel.ApplicationLifeCycleStatusType = ApplicationLifeCycleStatusTypeEnum.NOT_SUBMITTED;
                }
                genericFormModel.FormModel.ProjectSiteRefId = projectSiteId;

                //Initialization of Enum types of lists

                genericFormModel.EnumTemplateLists = new List<EnumListTemplate>();
                genericFormModel.EnumTemplateLists.Add(new EnumListTemplate()
                {
                    SelectListTypeCode = "BuidingPlanHUDApprovalTypeEnum",
                    SelectListItems = EnumOps.GetEnumAsSelectList<BuidingPlanHUDApprovalTypeEnum>()
                });

                genericFormModel.EnumTemplateLists.Add(new EnumListTemplate()
                {
                    SelectListTypeCode = "BuildingTypeEnum",
                    SelectListItems = EnumOps.GetEnumAsSelectList<BuildingTypeEnum>()
                });

                genericFormModel.EnumTemplateLists.Add(new EnumListTemplate()
                {
                    SelectListTypeCode = "InspectionTypeEnum",
                    SelectListItems = EnumOps.GetEnumAsSelectList<InspectionTypeEnum>()
                });

                genericFormModel.EnumTemplateLists.Add(new EnumListTemplate()
                {
                    SelectListTypeCode = "ProjectTypeEnum",
                    SelectListItems = EnumOps.GetEnumAsSelectList<ProjectTypeEnum>()
                });

                genericFormModel.EnumTemplateLists.Add(new EnumListTemplate()
                {
                    SelectListTypeCode = "IndustryColorCodeByPPCBTypeEnum",
                    SelectListItems = EnumOps.GetEnumAsSelectList<IndustryColorCodeByPPCBTypeEnum>()
                });

                genericFormModel.EnumTemplateLists.Add(new EnumListTemplate()
                {
                    SelectListTypeCode = "BuildingPlanApprovalAuthorityTypeEnum",
                    SelectListItems = EnumOps.GetEnumAsSelectList<BuildingPlanApprovalAuthorityTypeEnum>()
                });


                genericFormModel.ListTemplateLists = new List<ListTemplate>();

                //Initialization of list templates
                var competentPersonList = await _iAuthService.GetOfficerDetailsByRoleName("CPPR");

                genericFormModel.FormModel.CompetentPersonList = competentPersonList.ListData;

                var empaneledArchitectsList = await _iAuthService.GetOfficerDetailsByRoleName("ARCH");
                genericFormModel.FormModel.EmpaneledArchitectsList = empaneledArchitectsList.ListData;

                var empaneledEngineersList = await _iAuthService.GetOfficerDetailsByRoleName("ENGR");
                genericFormModel.FormModel.EmpaneledEngineersList = empaneledEngineersList.ListData;

                genericFormModel.AppFormStepsList = new List<AppFormStepsInfo>();
                genericFormModel.AppFormStepsList = await _iApplicationMamnagementService.GetAppFormStepperInfo(id, ApplicationTypeEnum.BUILDING_PLAN_HUD, (genericFormModel.FormModel != null ? genericFormModel.FormModel.BuildingPlanHUDId : id), "GD");
            }
            catch (Exception ex)
            {
                genericFormModel.HasError = true;
                genericFormModel.ErrorDesc = ex.Message;
            }
            return genericFormModel;
        }
        public async Task<GenericServiceResultTemplate> AddUpdate_GeneralDetail(BuildingPlanHUD_GeneralDetail formModel, string id)
        {
            GenericServiceResultTemplate genericServiceResultTemplate = new GenericServiceResultTemplate();
            try
            {
                genericServiceResultTemplate.HasException = false;
                genericServiceResultTemplate.CustomeValidationResult = CustomeValidator<BuildingPlanHUD_GeneralDetail>.ValidateModel_AllProperties(formModel);
                if (genericServiceResultTemplate.CustomeValidationResult.IsValid)
                {
                    if (formModel.BuildingPlanHUDId != 0) //Existing record
                    {
                        //formModel.LastModifiedOnDate = DateTime.Now;
                        _iGR_BuildingPlanHUD_GeneralDetail.Update(formModel);
                        //Save changes
                        await _iGR_BuildingPlanHUD_GeneralDetail.SavechangeAsync();

                        //Update last modified date
                        await _iApplicationMamnagementService.UpdateAppLastModifiedDate(formModel.AppRefId);
                        genericServiceResultTemplate.ApplicationInitiateResponse = new ApplicationInitiateResponseViewModel();
                        genericServiceResultTemplate.ApplicationInitiateResponse.EntityKeyId = formModel.BuildingPlanHUDId;
                        genericServiceResultTemplate.ApplicationInitiateResponse.AppId = formModel.AppRefId;


                        if (formModel.IsUnderRightToBusinessAct == "1") // Update table with response json received from invest punjab api
                        {
                           
                            var rtb_mapping = await _iGR_BuildingPlanHUD_RTB_Mapping.GetAsync(x => x.AppRefId == formModel.AppRefId).ConfigureAwait(false);
                            if (formModel.PrincipalApproval_RBA_Details != null)
                            {
                                if (rtb_mapping.FirstOrDefault() != null)
                                {
                                    rtb_mapping.FirstOrDefault().ResponseJson = JsonConvert.SerializeObject(formModel.PrincipalApproval_RBA_Details);
                                    _iGR_BuildingPlanHUD_RTB_Mapping.Update(rtb_mapping.FirstOrDefault());
                                    await _iGR_BuildingPlanHUD_RTB_Mapping.SavechangeAsync();
                                }
                                else
                                {
                                    BuildingPlanHUD_RTB_Mapping buildingPlanHUD_RTB = new BuildingPlanHUD_RTB_Mapping()
                                    {
                                        ProjectIdentificationNo = formModel.ProjectIdentificationNo,
                                        AppIdRightToBusinessAct = formModel.AppIdRightToBusinessAct,
                                        AppRefId = genericServiceResultTemplate.ApplicationInitiateResponse.AppId
                                    };
                                    buildingPlanHUD_RTB.ResponseJson = JsonConvert.SerializeObject(formModel.PrincipalApproval_RBA_Details);
                                    _iGR_BuildingPlanHUD_RTB_Mapping.Insert(buildingPlanHUD_RTB);
                                    await _iGR_BuildingPlanHUD_RTB_Mapping.SavechangeAsync();
                                }
                            }
                        }
                    }
                    else //New record
                    {
                        //Set null all navigation properties from model
                        formModel = GenericModelOps<BuildingPlanHUD_GeneralDetail>.SetNullAllNevigationProperties(formModel);
                        genericServiceResultTemplate.ApplicationInitiateResponse = await _iApplicationMamnagementService.InitiateApplication(ApplicationTypeEnum.BUILDING_PLAN_HUD, formModel, formModel.ProjectSiteRefId, formModel.ApplicationPurposeType, id, formModel.IPin,formModel.InvestPunjab_AppId, false, 0, 0, null, null, formModel.ProjectSiteVersion);

                        if(genericServiceResultTemplate.ApplicationInitiateResponse.IsApplicationCreated== true)
                        {
                            // Update Native Appid by IPIN
                            var temp = await _iThirdPartyInegrationsService.UpdateBusinessFirstRequestNativeAppIdByIPin(formModel.IPin, formModel.InvestPunjab_AppId, genericServiceResultTemplate.ApplicationInitiateResponse.AppId, ApplicationTypeEnum.BUILDING_PLAN_HUD, formModel.ApplicationPurposeType);

                            // Share status to Invest Punjab Portal
                            await _iThirdPartyInegrationsService.ShareStatusToBusinessFirst(genericServiceResultTemplate.ApplicationInitiateResponse.AppId, ApplicationTypeEnum.BUILDING_PLAN_HUD, AppActionTypeEnum.APP_SAVE_DRAFT);

                            if(formModel.IsUnderRightToBusinessAct == "1") // Update table with response json received from invest punjab api
                            {
                                BuildingPlanHUD_RTB_Mapping buildingPlanHUD_RTB = new BuildingPlanHUD_RTB_Mapping()
                                {
                                        ProjectIdentificationNo = formModel.ProjectIdentificationNo,
                                        AppIdRightToBusinessAct = formModel.AppIdRightToBusinessAct,
                                        AppRefId = genericServiceResultTemplate.ApplicationInitiateResponse.AppId
                                };

                                if (formModel.PrincipalApproval_RBA_Details != null)
                                {
                                    buildingPlanHUD_RTB.ResponseJson = JsonConvert.SerializeObject(formModel.PrincipalApproval_RBA_Details);
                                    _iGR_BuildingPlanHUD_RTB_Mapping.Insert(buildingPlanHUD_RTB);
                                    await _iGR_BuildingPlanHUD_RTB_Mapping.SavechangeAsync();
                                }
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
            }
            return genericServiceResultTemplate;
        }

        #region BuildingPlanHUD Details
        public async Task<GenericFormModel<BuildingPlanHUDViewModel>> GetBuildingPlanHUDDetail(long id)
        {
            GenericFormModel<BuildingPlanHUDViewModel> genericFormModel = new GenericFormModel<BuildingPlanHUDViewModel>();
            try
            {
                genericFormModel.ListTemplateLists = new List<ListTemplate>();
                genericFormModel.FormModel = new BuildingPlanHUDViewModel();
                genericFormModel.FormModel.GeneralDetail = new BuildingPlanHUD_GeneralDetail();

                var parentWithChildObject = await _iGR_BuildingPlanHUD_GeneralDetail
                        .GetAsync(x => x.AppRefId == id,  //Conditions         
                          null,                //Orders          
                          x => x.Application)      //Includes
                        .ConfigureAwait(false);

                //Initialization of Enum types of lists
                genericFormModel.EnumTemplateLists = new List<EnumListTemplate>();
                genericFormModel.EnumTemplateLists.Add(new EnumListTemplate()
                {
                    SelectListTypeCode = "BuidingPlanHUDApprovalTypeEnum",
                    SelectListItems = EnumOps.GetEnumAsSelectList<BuidingPlanHUDApprovalTypeEnum>()
                });

                genericFormModel.EnumTemplateLists.Add(new EnumListTemplate()
                {
                    SelectListTypeCode = "IndustryColorCodeByPPCBTypeEnum",
                    SelectListItems = EnumOps.GetEnumAsSelectList<IndustryColorCodeByPPCBTypeEnum>()
                });

                genericFormModel.FormModel.GeneralDetail = parentWithChildObject.FirstOrDefault();
               
                genericFormModel.FormModel.IsLocked = parentWithChildObject.FirstOrDefault().Application.IsLocked;
                genericFormModel.FormModel.IsFeeApplicable = parentWithChildObject.FirstOrDefault().Application.IsFeeApplicable;
                genericFormModel.ApplicationLifeCycleStatusType = parentWithChildObject.FirstOrDefault().Application.ApplicationLifeCycleStatusType;

                genericFormModel.AppFormStepsList = new List<AppFormStepsInfo>();
                genericFormModel.AppFormStepsList = await _iApplicationMamnagementService.GetAppFormStepperInfo(id, ApplicationTypeEnum.BUILDING_PLAN_HUD, (genericFormModel.FormModel != null ? genericFormModel.FormModel.GeneralDetail.BuildingPlanHUDId : id), "LOCK");
            }
            catch (Exception ex)
            {
                genericFormModel.HasError = true;
                genericFormModel.ErrorDesc = ex.Message;
            }
            return genericFormModel;
        }
        #endregion BuildingPlanHUD Details

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

                    if (await _iApplicationMamnagementService.LockApplication(appRefId, AppActionType, remarks, ApplicationTypeEnum.BUILDING_PLAN_HUD)!="")
                    {
                        genericServiceResultTemplate.ApplicationInitiateResponse.IsApplicationCreated = true;
                        // Share status to Invest Punjab Portal
                        // await _iThirdPartyInegrationsService.ShareStatusToBusinessFirst(appRefId, ApplicationTypeEnum.BUILDING_PLAN_HUD, (AppActionTypeEnum)AppActionType);
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
            }
            return genericServiceResultTemplate;
        }
        #endregion Lock Application

        #region Building Plan HUD Raise Fee
        public async Task<GenericFormModel<RaiseFeeParmsViewModel>> GetBuildingPlanHUDRaiseFee(Int64 appRefId, ApplicationTypeEnum applicationType, Int64 paymentBatchCounter)
        {
            GenericFormModel<RaiseFeeParmsViewModel> genericFormModel = new GenericFormModel<RaiseFeeParmsViewModel>();
            try
            {
                genericFormModel.FormModel = new RaiseFeeParmsViewModel();
                Int64 feeHeaderId = 0;
                FeesHeader feesHeader = new FeesHeader();
               
                if (applicationType == ApplicationTypeEnum.BUILDING_PLAN_HUD)
                {
                    feeHeaderId = 19;
                    feesHeader = await GetFeeHeader(feeHeaderId);
                    var calculatedFeeDetail = await _iPaymentManagerService.CalculateFeeAmountHeaderwise(appRefId, applicationType, feeHeaderId, ApplicationPurposeTypeEnum.GRANT_LICENCE);
                    genericFormModel.FormModel.FeeCalculatorInfoParms = new List<FeeCalculatorInfoParmsViewModel>();
                    genericFormModel.FormModel.FeeCalculatorInfoParms.Add(new FeeCalculatorInfoParmsViewModel()
                    {
                        AmountCalculated = calculatedFeeDetail.CalculatedAmount,
                        AppRefId = appRefId,
                        Description = calculatedFeeDetail.Description,
                        FeeHeaderId = feesHeader.FeeHeaderId,
                        FeeHeaderTitle = feesHeader.FeeHeaderTitle,
                        IsDeduductible = false,
                        PaymentDetailId = 0,
                        PaymentBatchCounter = 0,
                        IsTreasuryPayment = false,
                        NonTreasuryCode = _iConfiguration.GetSection("PaymentGatewayConfigs").GetSection("NonTreasuryCodes").GetSection("BOCWCess").Value
                    });

                    feeHeaderId = 20;
                    feesHeader = await GetFeeHeader(feeHeaderId);
                    calculatedFeeDetail = await _iPaymentManagerService.CalculateFeeAmountHeaderwise(appRefId, applicationType, feeHeaderId, ApplicationPurposeTypeEnum.GRANT_LICENCE);
                    genericFormModel.FormModel.FeeCalculatorInfoParms.Add(new FeeCalculatorInfoParmsViewModel()
                    {
                        AmountCalculated = 0,
                        AppRefId = appRefId,
                        Description = calculatedFeeDetail.Description,
                        FeeHeaderId = feesHeader.FeeHeaderId,
                        FeeHeaderTitle = feesHeader.FeeHeaderTitle,
                        IsDeduductible = false,
                        PaymentDetailId = 0,
                        PaymentBatchCounter = 0,
                        IsTreasuryPayment = true,
                        NonTreasuryCode = null
                    });

                    feeHeaderId = 21;
                    feesHeader = await GetFeeHeader(feeHeaderId);
                    calculatedFeeDetail = await _iPaymentManagerService.CalculateFeeAmountHeaderwise(appRefId, applicationType, feeHeaderId, ApplicationPurposeTypeEnum.GRANT_LICENCE);
                    genericFormModel.FormModel.FeeCalculatorInfoParms.Add(new FeeCalculatorInfoParmsViewModel()
                    {
                        AmountCalculated = 0,
                        AppRefId = appRefId,
                        Description = calculatedFeeDetail.Description,
                        FeeHeaderId = feesHeader.FeeHeaderId,
                        FeeHeaderTitle = feesHeader.FeeHeaderTitle,
                        IsDeduductible = false,
                        PaymentDetailId = 0,
                        PaymentBatchCounter = 0,
                        IsTreasuryPayment = true,
                        NonTreasuryCode = null
                    });
                    feeHeaderId = 22;
                    feesHeader = await GetFeeHeader(feeHeaderId);
                    calculatedFeeDetail = await _iPaymentManagerService.CalculateFeeAmountHeaderwise(appRefId, applicationType, feeHeaderId, ApplicationPurposeTypeEnum.GRANT_LICENCE);
                    genericFormModel.FormModel.FeeCalculatorInfoParms.Add(new FeeCalculatorInfoParmsViewModel()
                    {
                        AmountCalculated = 0,
                        AppRefId = appRefId,
                        Description = calculatedFeeDetail.Description,
                        FeeHeaderId = feesHeader.FeeHeaderId,
                        FeeHeaderTitle = feesHeader.FeeHeaderTitle,
                        IsDeduductible = false,
                        PaymentDetailId = 0,
                        PaymentBatchCounter = 0,
                        IsTreasuryPayment = false,
                        NonTreasuryCode = ""
                    });
                    feeHeaderId = 23;
                    feesHeader = await GetFeeHeader(feeHeaderId);
                    calculatedFeeDetail = await _iPaymentManagerService.CalculateFeeAmountHeaderwise(appRefId, applicationType, feeHeaderId, ApplicationPurposeTypeEnum.GRANT_LICENCE);
                    genericFormModel.FormModel.FeeCalculatorInfoParms.Add(new FeeCalculatorInfoParmsViewModel()
                    {
                        AmountCalculated = 0,
                        AppRefId = appRefId,
                        Description = calculatedFeeDetail.Description,
                        FeeHeaderId = feesHeader.FeeHeaderId,
                        FeeHeaderTitle = feesHeader.FeeHeaderTitle,
                        IsDeduductible = false,
                        PaymentDetailId = 0,
                        PaymentBatchCounter = 0,
                        IsTreasuryPayment = true,
                        NonTreasuryCode = null
                    });
                    feeHeaderId = 24;
                    feesHeader = await GetFeeHeader(feeHeaderId);
                    calculatedFeeDetail = await _iPaymentManagerService.CalculateFeeAmountHeaderwise(appRefId, applicationType, feeHeaderId, ApplicationPurposeTypeEnum.GRANT_LICENCE);
                    genericFormModel.FormModel.FeeCalculatorInfoParms.Add(new FeeCalculatorInfoParmsViewModel()
                    {
                        AmountCalculated = 0,
                        AppRefId = appRefId,
                        Description = calculatedFeeDetail.Description,
                        FeeHeaderId = feesHeader.FeeHeaderId,
                        FeeHeaderTitle = feesHeader.FeeHeaderTitle,
                        IsDeduductible = false,
                        PaymentDetailId = 0,
                        PaymentBatchCounter = 0,
                        IsTreasuryPayment = true,
                        NonTreasuryCode = null
                    });
                    feeHeaderId = 25;
                    feesHeader = await GetFeeHeader(feeHeaderId);
                    calculatedFeeDetail = await _iPaymentManagerService.CalculateFeeAmountHeaderwise(appRefId, applicationType, feeHeaderId, ApplicationPurposeTypeEnum.GRANT_LICENCE);
                    genericFormModel.FormModel.FeeCalculatorInfoParms.Add(new FeeCalculatorInfoParmsViewModel()
                    {
                        AmountCalculated = 0,
                        AppRefId = appRefId,
                        Description = calculatedFeeDetail.Description,
                        FeeHeaderId = feesHeader.FeeHeaderId,
                        FeeHeaderTitle = feesHeader.FeeHeaderTitle,
                        IsDeduductible = false,
                        PaymentDetailId = 0,
                        PaymentBatchCounter = 0,
                        IsTreasuryPayment = true,
                        NonTreasuryCode = null
                    });
                    feeHeaderId = 26;
                    feesHeader = await GetFeeHeader(feeHeaderId);
                    calculatedFeeDetail = await _iPaymentManagerService.CalculateFeeAmountHeaderwise(appRefId, applicationType, feeHeaderId, ApplicationPurposeTypeEnum.GRANT_LICENCE);
                    genericFormModel.FormModel.FeeCalculatorInfoParms.Add(new FeeCalculatorInfoParmsViewModel()
                    {
                        AmountCalculated = 0,
                        AppRefId = appRefId,
                        Description = calculatedFeeDetail.Description,
                        FeeHeaderId = feesHeader.FeeHeaderId,
                        FeeHeaderTitle = feesHeader.FeeHeaderTitle,
                        IsDeduductible = false,
                        PaymentDetailId = 0,
                        PaymentBatchCounter = 0,
                        IsTreasuryPayment = true,
                        NonTreasuryCode = null
                    });
                    feeHeaderId = 27;
                    feesHeader = await GetFeeHeader(feeHeaderId);
                    calculatedFeeDetail = await _iPaymentManagerService.CalculateFeeAmountHeaderwise(appRefId, applicationType, feeHeaderId, ApplicationPurposeTypeEnum.GRANT_LICENCE);
                    genericFormModel.FormModel.FeeCalculatorInfoParms.Add(new FeeCalculatorInfoParmsViewModel()
                    {
                        AmountCalculated = 0,
                        AppRefId = appRefId,
                        Description = calculatedFeeDetail.Description,
                        FeeHeaderId = feesHeader.FeeHeaderId,
                        FeeHeaderTitle = feesHeader.FeeHeaderTitle,
                        IsDeduductible = false,
                        PaymentDetailId = 0,
                        PaymentBatchCounter = 0,
                        IsTreasuryPayment = false,
                        NonTreasuryCode = _iConfiguration.GetSection("PaymentGatewayConfigs").GetSection("NonTreasuryCodes").GetSection("SIF").Value
                    });
                    feeHeaderId = 28;
                    feesHeader = await GetFeeHeader(feeHeaderId);
                    calculatedFeeDetail = await _iPaymentManagerService.CalculateFeeAmountHeaderwise(appRefId, applicationType, feeHeaderId, ApplicationPurposeTypeEnum.GRANT_LICENCE);
                    genericFormModel.FormModel.FeeCalculatorInfoParms.Add(new FeeCalculatorInfoParmsViewModel()
                    {
                        AmountCalculated = 0,
                        AppRefId = appRefId,
                        Description = calculatedFeeDetail.Description,
                        FeeHeaderId = feesHeader.FeeHeaderId,
                        FeeHeaderTitle = feesHeader.FeeHeaderTitle,
                        IsDeduductible = false,
                        PaymentDetailId = 0,
                        PaymentBatchCounter = 0,
                        IsTreasuryPayment = true,
                        NonTreasuryCode = null
                    });

                    feeHeaderId = 29;
                    feesHeader = await GetFeeHeader(feeHeaderId);
                    calculatedFeeDetail = await _iPaymentManagerService.CalculateFeeAmountHeaderwise(appRefId, applicationType, feeHeaderId, ApplicationPurposeTypeEnum.GRANT_LICENCE);
                    genericFormModel.FormModel.FeeCalculatorInfoParms.Add(new FeeCalculatorInfoParmsViewModel()
                    {
                        AmountCalculated = 0,
                        AppRefId = appRefId,
                        Description = calculatedFeeDetail.Description,
                        FeeHeaderId = feesHeader.FeeHeaderId,
                        FeeHeaderTitle = feesHeader.FeeHeaderTitle,
                        IsDeduductible = false,
                        PaymentDetailId = 0,
                        PaymentBatchCounter = 0,
                        IsTreasuryPayment = true,
                        NonTreasuryCode = null
                    });
                }

                genericFormModel.FormModel.FeeCalculatorInfoParms = genericFormModel.FormModel.FeeCalculatorInfoParms.OrderBy(x => x.IsTreasuryPayment).ToList();

                genericFormModel.AppFormStepsList = new List<AppFormStepsInfo>();
                genericFormModel.AppFormStepsList = null;
            }
            catch (Exception ex)
            {
                genericFormModel.HasError = true;
                genericFormModel.ErrorDesc = ex.Message;
            }
            return genericFormModel;
        }

        public async Task<GenericFormModel<RaiseFeeParmsViewModel>> VerifyBuildingPlanHUDRaiseFee(Int64 appRefId, ApplicationTypeEnum applicationType, int paymentBatchCounter)
        {
            GenericFormModel<RaiseFeeParmsViewModel> genericFormModel = new GenericFormModel<RaiseFeeParmsViewModel>();
            try
            {
                paymentBatchCounter = _context.Applications.Where(x => x.AppId == appRefId && x.IsDeleted == false).FirstOrDefault().PaymentBatchCounter;
                var parentObject = _context.BuildingPlanHUDPaymentDetails.Where(x => x.AppRefId == appRefId && x.PaymentBatchCounter == paymentBatchCounter);

                genericFormModel.FormModel = new RaiseFeeParmsViewModel();
                Int64 feeHeaderId = 0;
                FeesHeader feesHeader = new FeesHeader();

                if (applicationType == ApplicationTypeEnum.BUILDING_PLAN_HUD)
                {
                    feeHeaderId = 19;
                    feesHeader = await GetFeeHeader(feeHeaderId);
                    var calculatedFeeDetail = await _iPaymentManagerService.CalculateFeeAmountHeaderwise(appRefId, applicationType, feeHeaderId, ApplicationPurposeTypeEnum.GRANT_LICENCE);
                    genericFormModel.FormModel.FeeCalculatorInfoParms = new List<FeeCalculatorInfoParmsViewModel>();
                    genericFormModel.FormModel.FeeCalculatorInfoParms.Add(new FeeCalculatorInfoParmsViewModel()
                    {
                        //AmountCalculated = calculatedFeeDetail.CalculatedAmount,
                        AmountCalculated = parentObject.Where(x => x.FeeHeaderRefId == 19).Select(x => x.AmountRaised).FirstOrDefault(),
                        AppRefId = appRefId,
                        Description = calculatedFeeDetail.Description,
                        FeeHeaderId = feesHeader.FeeHeaderId,
                        FeeHeaderTitle = feesHeader.FeeHeaderTitle,
                        IsDeduductible = false,
                        PaymentBatchCounter = paymentBatchCounter,
                        IsTreasuryPayment = false,
                        NonTreasuryCode = parentObject.Where(x => x.FeeHeaderRefId == 19).Select(x => x.NonTreasuryCode).FirstOrDefault()
                    });

                    feeHeaderId = 20;
                    feesHeader = await GetFeeHeader(feeHeaderId);
                    calculatedFeeDetail = await _iPaymentManagerService.CalculateFeeAmountHeaderwise(appRefId, applicationType, feeHeaderId, ApplicationPurposeTypeEnum.GRANT_LICENCE);
                    genericFormModel.FormModel.FeeCalculatorInfoParms.Add(new FeeCalculatorInfoParmsViewModel()
                    {
                        AmountCalculated = parentObject.Where(x => x.FeeHeaderRefId == 20).Select(x => x.AmountRaised).FirstOrDefault(),
                        AppRefId = appRefId,
                        Description = calculatedFeeDetail.Description,
                        FeeHeaderId = feesHeader.FeeHeaderId,
                        FeeHeaderTitle = feesHeader.FeeHeaderTitle,
                        IsDeduductible = false,
                        PaymentDetailId = parentObject.Where(x => x.FeeHeaderRefId == 20).Select(x => x.PaymentDetailId).FirstOrDefault(),
                        PaymentBatchCounter = paymentBatchCounter,
                        IsTreasuryPayment = true,
                        NonTreasuryCode = parentObject.Where(x => x.FeeHeaderRefId == 20).Select(x => x.NonTreasuryCode).FirstOrDefault()

                    });

                    feeHeaderId = 21;
                    feesHeader = await GetFeeHeader(feeHeaderId);
                    calculatedFeeDetail = await _iPaymentManagerService.CalculateFeeAmountHeaderwise(appRefId, applicationType, feeHeaderId, ApplicationPurposeTypeEnum.GRANT_LICENCE);
                    genericFormModel.FormModel.FeeCalculatorInfoParms.Add(new FeeCalculatorInfoParmsViewModel()
                    {
                        AmountCalculated = parentObject.Where(x => x.FeeHeaderRefId == 21).Select(x => x.AmountRaised).FirstOrDefault(),
                        AppRefId = appRefId,
                        Description = calculatedFeeDetail.Description,
                        FeeHeaderId = feesHeader.FeeHeaderId,
                        FeeHeaderTitle = feesHeader.FeeHeaderTitle,
                        IsDeduductible = false,
                        PaymentDetailId = parentObject.Where(x => x.FeeHeaderRefId == 21).Select(x => x.PaymentDetailId).FirstOrDefault(),
                        PaymentBatchCounter = paymentBatchCounter,
                        IsTreasuryPayment = true,
                        NonTreasuryCode = parentObject.Where(x => x.FeeHeaderRefId == 21).Select(x => x.NonTreasuryCode).FirstOrDefault()
                    });
                    feeHeaderId = 22;
                    feesHeader = await GetFeeHeader(feeHeaderId);
                    calculatedFeeDetail = await _iPaymentManagerService.CalculateFeeAmountHeaderwise(appRefId, applicationType, feeHeaderId, ApplicationPurposeTypeEnum.GRANT_LICENCE);
                    genericFormModel.FormModel.FeeCalculatorInfoParms.Add(new FeeCalculatorInfoParmsViewModel()
                    {
                        AmountCalculated = parentObject.Where(x => x.FeeHeaderRefId == 22).Select(x => x.AmountRaised).FirstOrDefault(),
                        AppRefId = appRefId,
                        Description = calculatedFeeDetail.Description,
                        FeeHeaderId = feesHeader.FeeHeaderId,
                        FeeHeaderTitle = feesHeader.FeeHeaderTitle,
                        IsDeduductible = false,
                        PaymentDetailId = parentObject.Where(x => x.FeeHeaderRefId == 22).Select(x => x.PaymentDetailId).FirstOrDefault(),
                        PaymentBatchCounter = paymentBatchCounter,
                        IsTreasuryPayment = false,
                        NonTreasuryCode = parentObject.Where(x => x.FeeHeaderRefId == 22).Select(x => x.NonTreasuryCode).FirstOrDefault()
                    });
                    feeHeaderId = 23;
                    feesHeader = await GetFeeHeader(feeHeaderId);
                    calculatedFeeDetail = await _iPaymentManagerService.CalculateFeeAmountHeaderwise(appRefId, applicationType, feeHeaderId, ApplicationPurposeTypeEnum.GRANT_LICENCE);
                    genericFormModel.FormModel.FeeCalculatorInfoParms.Add(new FeeCalculatorInfoParmsViewModel()
                    {
                        AmountCalculated = parentObject.Where(x => x.FeeHeaderRefId == 23).Select(x => x.AmountRaised).FirstOrDefault(),
                        AppRefId = appRefId,
                        Description = calculatedFeeDetail.Description,
                        FeeHeaderId = feesHeader.FeeHeaderId,
                        FeeHeaderTitle = feesHeader.FeeHeaderTitle,
                        IsDeduductible = false,
                        PaymentDetailId = parentObject.Where(x => x.FeeHeaderRefId == 23).Select(x => x.PaymentDetailId).FirstOrDefault(),
                        PaymentBatchCounter = paymentBatchCounter,
                        IsTreasuryPayment = true,
                        NonTreasuryCode = parentObject.Where(x => x.FeeHeaderRefId == 23).Select(x => x.NonTreasuryCode).FirstOrDefault()
                    });
                    feeHeaderId = 24;
                    feesHeader = await GetFeeHeader(feeHeaderId);
                    calculatedFeeDetail = await _iPaymentManagerService.CalculateFeeAmountHeaderwise(appRefId, applicationType, feeHeaderId, ApplicationPurposeTypeEnum.GRANT_LICENCE);
                    genericFormModel.FormModel.FeeCalculatorInfoParms.Add(new FeeCalculatorInfoParmsViewModel()
                    {
                        AmountCalculated = parentObject.Where(x => x.FeeHeaderRefId == 24).Select(x => x.AmountRaised).FirstOrDefault(),
                        AppRefId = appRefId,
                        Description = calculatedFeeDetail.Description,
                        FeeHeaderId = feesHeader.FeeHeaderId,
                        FeeHeaderTitle = feesHeader.FeeHeaderTitle,
                        IsDeduductible = false,
                        PaymentDetailId = parentObject.Where(x => x.FeeHeaderRefId == 24).Select(x => x.PaymentDetailId).FirstOrDefault(),
                        PaymentBatchCounter = paymentBatchCounter,
                        IsTreasuryPayment = true,
                        NonTreasuryCode = parentObject.Where(x => x.FeeHeaderRefId == 24).Select(x => x.NonTreasuryCode).FirstOrDefault()
                    });
                    feeHeaderId = 25;
                    feesHeader = await GetFeeHeader(feeHeaderId);
                    calculatedFeeDetail = await _iPaymentManagerService.CalculateFeeAmountHeaderwise(appRefId, applicationType, feeHeaderId, ApplicationPurposeTypeEnum.GRANT_LICENCE);
                    genericFormModel.FormModel.FeeCalculatorInfoParms.Add(new FeeCalculatorInfoParmsViewModel()
                    {
                        AmountCalculated = parentObject.Where(x => x.FeeHeaderRefId == 25).Select(x => x.AmountRaised).FirstOrDefault(),
                        AppRefId = appRefId,
                        Description = calculatedFeeDetail.Description,
                        FeeHeaderId = feesHeader.FeeHeaderId,
                        FeeHeaderTitle = feesHeader.FeeHeaderTitle,
                        IsDeduductible = false,
                        PaymentDetailId = parentObject.Where(x => x.FeeHeaderRefId == 25).Select(x => x.PaymentDetailId).FirstOrDefault(),
                        PaymentBatchCounter = paymentBatchCounter,
                        IsTreasuryPayment = true,
                        NonTreasuryCode = parentObject.Where(x => x.FeeHeaderRefId == 25).Select(x => x.NonTreasuryCode).FirstOrDefault()
                    });
                    feeHeaderId = 26;
                    feesHeader = await GetFeeHeader(feeHeaderId);
                    calculatedFeeDetail = await _iPaymentManagerService.CalculateFeeAmountHeaderwise(appRefId, applicationType, feeHeaderId, ApplicationPurposeTypeEnum.GRANT_LICENCE);
                    genericFormModel.FormModel.FeeCalculatorInfoParms.Add(new FeeCalculatorInfoParmsViewModel()
                    {
                        AmountCalculated = parentObject.Where(x => x.FeeHeaderRefId == 26).Select(x => x.AmountRaised).FirstOrDefault(),
                        AppRefId = appRefId,
                        Description = calculatedFeeDetail.Description,
                        FeeHeaderId = feesHeader.FeeHeaderId,
                        FeeHeaderTitle = feesHeader.FeeHeaderTitle,
                        IsDeduductible = false,
                        PaymentDetailId = parentObject.Where(x => x.FeeHeaderRefId == 26).Select(x => x.PaymentDetailId).FirstOrDefault(),
                        PaymentBatchCounter = paymentBatchCounter,
                        IsTreasuryPayment = true,
                        NonTreasuryCode = parentObject.Where(x => x.FeeHeaderRefId == 26).Select(x => x.NonTreasuryCode).FirstOrDefault()
                    });
                    feeHeaderId = 27;
                    feesHeader = await GetFeeHeader(feeHeaderId);
                    calculatedFeeDetail = await _iPaymentManagerService.CalculateFeeAmountHeaderwise(appRefId, applicationType, feeHeaderId, ApplicationPurposeTypeEnum.GRANT_LICENCE);
                    genericFormModel.FormModel.FeeCalculatorInfoParms.Add(new FeeCalculatorInfoParmsViewModel()
                    {
                        AmountCalculated = parentObject.Where(x => x.FeeHeaderRefId == 27).Select(x => x.AmountRaised).FirstOrDefault(),
                        AppRefId = appRefId,
                        Description = calculatedFeeDetail.Description,
                        FeeHeaderId = feesHeader.FeeHeaderId,
                        FeeHeaderTitle = feesHeader.FeeHeaderTitle,
                        IsDeduductible = false,
                        PaymentDetailId = parentObject.Where(x => x.FeeHeaderRefId == 27).Select(x => x.PaymentDetailId).FirstOrDefault(),
                        PaymentBatchCounter = paymentBatchCounter,
                        IsTreasuryPayment = false,
                        NonTreasuryCode = parentObject.Where(x => x.FeeHeaderRefId == 27).Select(x => x.NonTreasuryCode).FirstOrDefault()
                    });
                    feeHeaderId = 28;
                    feesHeader = await GetFeeHeader(feeHeaderId);
                    calculatedFeeDetail = await _iPaymentManagerService.CalculateFeeAmountHeaderwise(appRefId, applicationType, feeHeaderId, ApplicationPurposeTypeEnum.GRANT_LICENCE);
                    genericFormModel.FormModel.FeeCalculatorInfoParms.Add(new FeeCalculatorInfoParmsViewModel()
                    {
                        AmountCalculated = parentObject.Where(x => x.FeeHeaderRefId == 28).Select(x => x.AmountRaised).FirstOrDefault(),
                        AppRefId = appRefId,
                        Description = calculatedFeeDetail.Description,
                        FeeHeaderId = feesHeader.FeeHeaderId,
                        FeeHeaderTitle = feesHeader.FeeHeaderTitle,
                        IsDeduductible = false,
                        PaymentDetailId = parentObject.Where(x => x.FeeHeaderRefId == 28).Select(x => x.PaymentDetailId).FirstOrDefault(),
                        PaymentBatchCounter = paymentBatchCounter,
                        IsTreasuryPayment = true,
                        NonTreasuryCode = parentObject.Where(x => x.FeeHeaderRefId == 28).Select(x => x.NonTreasuryCode).FirstOrDefault()
                    });
                    feeHeaderId = 29;
                    feesHeader = await GetFeeHeader(feeHeaderId);
                    calculatedFeeDetail = await _iPaymentManagerService.CalculateFeeAmountHeaderwise(appRefId, applicationType, feeHeaderId, ApplicationPurposeTypeEnum.GRANT_LICENCE);
                    genericFormModel.FormModel.FeeCalculatorInfoParms.Add(new FeeCalculatorInfoParmsViewModel()
                    {
                        AmountCalculated = parentObject.Where(x => x.FeeHeaderRefId == 29).Select(x => x.AmountRaised).FirstOrDefault(),
                        AppRefId = appRefId,
                        Description = calculatedFeeDetail.Description,
                        FeeHeaderId = feesHeader.FeeHeaderId,
                        FeeHeaderTitle = feesHeader.FeeHeaderTitle,
                        IsDeduductible = false,
                        PaymentDetailId = parentObject.Where(x => x.FeeHeaderRefId == 29).Select(x => x.PaymentDetailId).FirstOrDefault(),
                        PaymentBatchCounter = paymentBatchCounter,
                        IsTreasuryPayment = true,
                        NonTreasuryCode = parentObject.Where(x => x.FeeHeaderRefId == 29).Select(x => x.NonTreasuryCode).FirstOrDefault()
                    });
                }
                genericFormModel.FormModel.FeeCalculatorInfoParms = genericFormModel.FormModel.FeeCalculatorInfoParms.OrderBy(x => x.IsTreasuryPayment).ToList();
                genericFormModel.AppFormStepsList = new List<AppFormStepsInfo>();
                genericFormModel.AppFormStepsList = null;
            }
            catch (Exception ex)
            {
                genericFormModel.HasError = true;
                genericFormModel.ErrorDesc = ex.Message;
            }
            return genericFormModel;
        }

        public async Task<GenericResponseTemplateModel<ApplicationActionViewModel>> AddUpdate_BuildingPlanHUDRaiseFee(RaiseFeeParmsViewModel requestData)
        {
            GenericResponseTemplateModel<ApplicationActionViewModel> genericServiceResultTemplate = new GenericResponseTemplateModel<ApplicationActionViewModel>();
            try
            {
                genericServiceResultTemplate.HasError = false;
                //genericServiceResultTemplate.CustomeValidationResult = CustomeValidator<List<FeeCalculatorInfoParmsViewModel>>.ValidateModel_AllProperties(requestData);
                //if (genericServiceResultTemplate.CustomeValidationResult.IsValid)
                //{
                    if (requestData.FeeCalculatorInfoParms.Count > 0)
                    {
                        List<BuildingPlanHUDPaymentDetail> paymentDetailList = new List<BuildingPlanHUDPaymentDetail>();
                        int paymentBatchCounter = requestData.FeeCalculatorInfoParms.FirstOrDefault().PaymentBatchCounter;

                        if (paymentBatchCounter == 0)
                        {
                            if(requestData.IsForVerification == false) 
                            { 
                                var res = await _iApplicationMamnagementService.IncreasePaymentBatchCounter(requestData.FeeCalculatorInfoParms.FirstOrDefault().AppRefId);
                                paymentBatchCounter = res.ResponseDataModel;
                            }
                        }

                        foreach (var item in requestData.FeeCalculatorInfoParms)
                        {
                            paymentDetailList.Add(new BuildingPlanHUDPaymentDetail()
                            {
                                AmountRaised = item.AmountCalculated,
                                AmountAlreadyPaid = 0,
                                AppRefId = item.AppRefId,
                                FeeHeaderRefId = item.FeeHeaderId,
                                IsFeeApplicable = true,
                                PaymentBatchCounter = paymentBatchCounter,
                                Createddate = DateTime.Now,
                                LastModifiedDate = DateTime.Now,
                                PaymentDetailId= item.PaymentDetailId,
                                NonTreasuryCode = item.NonTreasuryCode
                            });
                        }
                        genericServiceResultTemplate = await _iBuildingPlanHUDRepository.AddUpdate_RaiseFee(paymentDetailList, requestData.IsForVerification, requestData.Remarks, requestData.IsTimeLineFlow);
                    }
                //}
            }
            catch (Exception ex)
            {
                genericServiceResultTemplate.HasError = true;
                genericServiceResultTemplate.ErrorDesc = ex.Message;
            }
            return genericServiceResultTemplate;
        }

        public async Task<FeesHeader> GetFeeHeader(Int64 feeHeaderId)
        {
            return await _iPaymentManagerRepository.GetFeeHeader(feeHeaderId);
        }


        public async Task<GenericFormModel<List<BuildingPlanHUDPaymentDetail>>> Get_RaisedFeeList(Int64 appRefId, Int64 paymentBatchCounter)
        {
            GenericFormModel<List<BuildingPlanHUDPaymentDetail>> genericFormModel = new GenericFormModel<List<BuildingPlanHUDPaymentDetail>>();
            try
            {
                genericFormModel =  await _iBuildingPlanHUDRepository.Get_RaisedFeeList(appRefId, paymentBatchCounter);
                if(!genericFormModel.HasError && genericFormModel.FormModel.Count() > 0)
                {
                    genericFormModel.FormModel = genericFormModel.FormModel.Select(x => 
                    { 
                        x.AmountPayable = x.AmountRaised - x.AmountAlreadyPaid;
                        x.Description = (x.FeeHeaderRefId == 19 ? "1 % of building cost as per CA certificate." : x.FeeHeaderRefId == 20 ? "Rs 60 per square meter of covered area." : "");
                        return x; 
                    }).ToList();

                    //genericFormModel.FormModel[genericFormModel.FormModel.FindIndex(x => x.FeeHeaderRefId == 19)].Description = "1 % of building cost as per CA certificate";
                    //genericFormModel.FormModel[genericFormModel.FormModel.FindIndex(x => x.FeeHeaderRefId == 20)].Description = "Rs 60 per square meter of covered area.";


                }
                //genericFormModel.AppFormStepsList = new List<AppFormStepsInfo>();
                genericFormModel.AppFormStepsList = null;
            }
            catch (Exception ex)
            {
                genericFormModel.HasError = true;
                genericFormModel.ErrorDesc = ex.Message;
            }
            return genericFormModel;
        }

        public async Task<GenericResponseTemplateModel<bool>> Update_RaisedFeeDetail(BuildingPlanHUDPaymentDetailViewModal requestData)
        {
            GenericResponseTemplateModel<bool> genericServiceResultTemplate = new GenericResponseTemplateModel<bool>();
            try
            {
                genericServiceResultTemplate.HasError = false;
                genericServiceResultTemplate = await _iBuildingPlanHUDRepository.Update_RaisedFeeDetail(requestData);
                genericServiceResultTemplate.ResponseDataModel = true;
            }
            catch (Exception ex)
            {
                genericServiceResultTemplate.HasError = true;
                genericServiceResultTemplate.ErrorDesc = ex.Message;
            }
            return genericServiceResultTemplate;
        }

        public async Task<GenericResponseTemplateModel<bool>> UpdateActionLogIdInRaisedFee(UpdateActionLogInRaisedFeeParmsViewModel requestData)
        {
            GenericResponseTemplateModel<bool> genericServiceResultTemplate = new GenericResponseTemplateModel<bool>();
            try
            {
                genericServiceResultTemplate.HasError = false;
                var payments = _context.BuildingPlanHUDPaymentDetails.Where(x => x.AppRefId == requestData.AppRefId && x.PaymentBatchCounter == requestData.PaymentBatchCounter).ToList();
                payments = payments.Select(x => { x.ApplicationActionLogId = requestData.ApplicationActionLogId; return x; }).ToList();
                await _context.BulkUpdateAsync<BuildingPlanHUDPaymentDetail>(payments);
                genericServiceResultTemplate.ResponseDataModel = true;
            }
            catch (Exception ex)
            {
                genericServiceResultTemplate.HasError = true;
                genericServiceResultTemplate.ErrorDesc = ex.Message;
            }
            return genericServiceResultTemplate;
        }

        #endregion 
    }
}
