using Newtonsoft.Json;
using pbsamadhannetcoreapi.CommonUtiliteis;
using pbsamadhannetcoreapi.Models;
using pbsamadhannetcoreapi.Repositories;
using pbsamadhannetcoreapi.Repositories.Implementations;
using pbsamadhannetcoreapi.Services.Implementations;
using pbsamadhannetcoreapi.ViewModels;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace pbsamadhannetcoreapi.Services
{
    public class Licence_Factory_NightShiftService : ILicence_Factory_NightShiftService
    {
        private readonly IGeneric_SP_Repository _iGeneric_SP_Repository;    
        private readonly AppDbContext _context;
        private readonly IGenericRepository<Licence_Factory_NightShift_Approval> _iGR_Licence_Factory_NightShift_Approval;
        private readonly IApplicationManagementService<Licence_Factory_NightShift_Approval> _iApplicationMamnagementService;
        private readonly IProjectSiteService _iProjectSiteService;
        private readonly IThirdPartyInegrationsService _iThirdPartyInegrationsService;
        private readonly ISystem_O_CommunicationService _iSystem_O_CommunicationService;
        public Licence_Factory_NightShiftService(AppDbContext context,
            IGenericRepository<Licence_Factory_NightShift_Approval> iGR_Licence_Factory_NightShift_Approval,
            IApplicationManagementService<Licence_Factory_NightShift_Approval> IApplicationMamnagementService,
            IProjectSiteService iProjectSiteService,
            IThirdPartyInegrationsService iThirdPartyInegrationsService,
            IGeneric_SP_Repository iGeneric_SP_Repository,
            ISystem_O_CommunicationService iSystem_O_CommunicationService
            )
        {
            _context = context;
            _iGR_Licence_Factory_NightShift_Approval = iGR_Licence_Factory_NightShift_Approval;
            _iApplicationMamnagementService = IApplicationMamnagementService;
            _iProjectSiteService = iProjectSiteService;
            _iThirdPartyInegrationsService = iThirdPartyInegrationsService;
            _iGeneric_SP_Repository = iGeneric_SP_Repository;
            _iSystem_O_CommunicationService = iSystem_O_CommunicationService;
        }

        #region Night Shift General Details
        public async Task<GenericFormModel<Licence_Factory_NightShift_Approval>> GetGeneralDetail(Int64 id, Int64 projectSiteId)
        {
            GenericFormModel<Licence_Factory_NightShift_Approval> genericFormModel = new GenericFormModel<Licence_Factory_NightShift_Approval>();
            try
            {
                genericFormModel.FormModel = new Licence_Factory_NightShift_Approval();
                genericFormModel.FormModel.NightShift_ChecklistPoints = new List<Licence_Factory_NightShift_ChecklistPoint>();

                //genericServiceResultTemplate.ResponseDataModel = genericServiceResultTemplate.ResponseDataModel.Select(x => { x.IsSelected = false; return x; }).ToList();
                if (id != 0) //Existing Record
                {
                    var parentWithChildObject = await _iGR_Licence_Factory_NightShift_Approval
                       .GetAsync(x => x.AppRefId == id,  //Conditions         
                         null,                //Orders
                                              //x => x.Licence_Factory_NightShift_ChecklistPoints,
                         x => x.Application, x => x.Application.ApplicationAction)      //Includes
                       .ConfigureAwait(false);
                    genericFormModel.FormModel = parentWithChildObject.FirstOrDefault();
                    //genericFormModel.FormModel.EmployeeList = parentWithChildObject.FirstOrDefault().FactoryLicence_EmployeeDetails.ToList();
                    genericFormModel.IsEditAllowed = genericFormModel.FormModel.Application.IsAllowEdit;
                    genericFormModel.IsLocked = genericFormModel.FormModel.Application.IsLocked;
                    genericFormModel.ApplicationLifeCycleStatusType = genericFormModel.FormModel.Application.ApplicationLifeCycleStatusType;

                    var selectedIds = JsonConvert.DeserializeObject<List<Int64>>(parentWithChildObject.FirstOrDefault().CheckListJson);
                    genericFormModel.FormModel.NightShift_ChecklistPoints = _context.Licence_Factory_NightShift_ChecklistPoints.Where(x => x.IsEnabled == true).ToList();
                    genericFormModel.FormModel.NightShift_ChecklistPoints = genericFormModel.FormModel.NightShift_ChecklistPoints.Select(x => { x.IsSelected = (selectedIds.Contains(x.ChecklistID)); return x; }).ToList();
                }

                else //New Record
                {
                    //genericFormModel.FormModel = new Licence_Factory_NightShift_Approval();

                    //Allow user to edit form
                    genericFormModel.FormModel.NightShift_ChecklistPoints = _context.Licence_Factory_NightShift_ChecklistPoints.Where(x => x.IsEnabled == true).ToList();
                    genericFormModel.IsEditAllowed = true;
                    genericFormModel.IsLocked = false;
                    genericFormModel.ApplicationLifeCycleStatusType = ApplicationLifeCycleStatusTypeEnum.NOT_SUBMITTED;
                }
                genericFormModel.FormModel.ProjectSiteRefId = projectSiteId;

                genericFormModel.AppFormStepsList = new List<AppFormStepsInfo>();
                genericFormModel.AppFormStepsList = await _iApplicationMamnagementService.GetAppFormStepperInfo(id, ApplicationTypeEnum.WOMEN_NIGHT_SHIFT_FACTORY, (genericFormModel.FormModel != null ? genericFormModel.FormModel.Id : id), "GD");
            }
            catch (Exception ex)
            {
                genericFormModel.HasError = true;
                genericFormModel.ErrorDesc = ex.Message;
            }
            return genericFormModel;
        }
        public async Task<GenericServiceResultTemplate> AddUpdate_GeneralDetail(Licence_Factory_NightShift_Approval formModel, string id)
        {
            GenericServiceResultTemplate genericServiceResultTemplate = new GenericServiceResultTemplate();
            try
            {
                genericServiceResultTemplate.HasException = false;
                genericServiceResultTemplate.CustomeValidationResult = CustomeValidator<Licence_Factory_NightShift_Approval>.ValidateModel_AllProperties(formModel);
                if (genericServiceResultTemplate.CustomeValidationResult.IsValid)
                {
                    if (formModel.Id != 0) //Existing record
                    {
                        _iGR_Licence_Factory_NightShift_Approval.Update(formModel);
                        await _iGR_Licence_Factory_NightShift_Approval.SavechangeAsync();

                        //Update last modified date
                        await _iApplicationMamnagementService.UpdateAppLastModifiedDate(formModel.AppRefId);
                        genericServiceResultTemplate.ApplicationInitiateResponse = new ApplicationInitiateResponseViewModel();
                        genericServiceResultTemplate.ApplicationInitiateResponse.EntityKeyId = formModel.Id;
                        genericServiceResultTemplate.ApplicationInitiateResponse.AppId = formModel.AppRefId;


                        // Check has status share to business first
                        var isStatusShared = await _iThirdPartyInegrationsService.HasShareStatusToBusinessFirst(genericServiceResultTemplate.ApplicationInitiateResponse.AppId);
                        if (isStatusShared != null && !isStatusShared.HasError && !isStatusShared.ResponseDataModel)
                        {
                            // Update Native Appid by IPIN
                            await _iThirdPartyInegrationsService.UpdateBusinessFirstRequestNativeAppIdByIPin(formModel.IPin, genericServiceResultTemplate.ApplicationInitiateResponse.AppId, formModel.InvestPunjab_AppId, ApplicationTypeEnum.WOMEN_NIGHT_SHIFT_FACTORY, formModel.ApplicationPurposeType);

                            // Share status to Invest Punjab Portal
                            await _iThirdPartyInegrationsService.ShareStatusToBusinessFirst(genericServiceResultTemplate.ApplicationInitiateResponse.AppId, ApplicationTypeEnum.WOMEN_NIGHT_SHIFT_FACTORY, AppActionTypeEnum.APP_SAVE_DRAFT);
                        }
                    }
                    else //New record
                    {
                        //Get Old Portel Data AppId , NAR , Licence Number

                        var serResp = await _iSystem_O_CommunicationService.OldDataAppClerance(formModel.LicenceNumber);

                        //Set null all navigation properties from model
                        formModel = GenericModelOps<Licence_Factory_NightShift_Approval>.SetNullAllNevigationProperties(formModel);
                        genericServiceResultTemplate.ApplicationInitiateResponse = await _iApplicationMamnagementService.InitiateApplication(ApplicationTypeEnum.WOMEN_NIGHT_SHIFT_FACTORY, formModel, formModel.ProjectSiteRefId, formModel.ApplicationPurposeType, id, formModel.IPin, formModel.InvestPunjab_AppId, false, 0, 0, null, formModel.LicenceNumber, formModel.ProjectSiteVersion);



                        if (genericServiceResultTemplate.ApplicationInitiateResponse.IsApplicationCreated == true)
                        {
                            // Update Native Appid by IPIN
                            await _iThirdPartyInegrationsService.UpdateBusinessFirstRequestNativeAppIdByIPin(formModel.IPin, formModel.InvestPunjab_AppId, genericServiceResultTemplate.ApplicationInitiateResponse.AppId, ApplicationTypeEnum.WOMEN_NIGHT_SHIFT_FACTORY, formModel.ApplicationPurposeType);
                            // Share status to Invest Punjab Portal
                            await _iThirdPartyInegrationsService.ShareStatusToBusinessFirst(genericServiceResultTemplate.ApplicationInitiateResponse.AppId, ApplicationTypeEnum.WOMEN_NIGHT_SHIFT_FACTORY, AppActionTypeEnum.APP_SAVE_DRAFT);
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
        #endregion

        #region Licence Factory Night Shift Detail

        public async Task<GenericFormModel<Licence_Factory_NightShiftViewModel>> GetLicenceFactoryNightShiftDetail(long id)
        {
            GenericFormModel<Licence_Factory_NightShiftViewModel> genericFormModel = new GenericFormModel<Licence_Factory_NightShiftViewModel>();
            try
            {
                genericFormModel.ListTemplateLists = new List<ListTemplate>();
                genericFormModel.FormModel = new Licence_Factory_NightShiftViewModel();
                genericFormModel.FormModel.GeneralDetail = new Licence_Factory_NightShift_Approval();

                var parentWithChildObject = await _iGR_Licence_Factory_NightShift_Approval
                       .GetAsync(x => x.AppRefId == id,  //Conditions         
                         null,                //Orders          
                         x => x.Application)
                       .ConfigureAwait(false);

                genericFormModel.FormModel.GeneralDetail = parentWithChildObject.FirstOrDefault();
                genericFormModel.FormModel.GeneralDetail.NightShift_ChecklistPoints = new List<Licence_Factory_NightShift_ChecklistPoint>();
                genericFormModel.FormModel.GeneralDetail.NightShift_ChecklistPoints = _context.Licence_Factory_NightShift_ChecklistPoints.Where(x => x.IsEnabled == true).ToList();

                genericFormModel.FormModel.IsLocked = parentWithChildObject.FirstOrDefault().Application.IsLocked;
                genericFormModel.FormModel.IsFeeApplicable = parentWithChildObject.FirstOrDefault().Application.IsFeeApplicable;
                genericFormModel.ApplicationLifeCycleStatusType = parentWithChildObject.FirstOrDefault().Application.ApplicationLifeCycleStatusType;

                genericFormModel.AppFormStepsList = new List<AppFormStepsInfo>();
                genericFormModel.AppFormStepsList = await _iApplicationMamnagementService.GetAppFormStepperInfo(genericFormModel.FormModel.GeneralDetail.AppRefId, ApplicationTypeEnum.WOMEN_NIGHT_SHIFT_FACTORY, (genericFormModel.FormModel != null ? genericFormModel.FormModel.GeneralDetail.Id : id), "LOCK");

                var siteId = await _iProjectSiteService.GetProjectSiteRefIdByAppId(id);
                var projectProfileInfo = await _iProjectSiteService.GetProjectSiteDetail(siteId.ResponseDataModel);
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

                    if (await _iApplicationMamnagementService.LockApplication(appRefId, AppActionType, remarks, ApplicationTypeEnum.WOMEN_NIGHT_SHIFT_FACTORY) != "")
                    {
                        genericServiceResultTemplate.ApplicationInitiateResponse.IsApplicationCreated = true;
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


        public async Task<GenericResponseTemplateModel<List<GetlastClearanceViewModel>>> GetLastClearances(string licenceNo , Int64 projectSiteRefId, int projectSiteVersion)
        {
            GenericResponseTemplateModel<List<GetlastClearanceViewModel>> genericResponseTemplateModel = new GenericResponseTemplateModel<List<GetlastClearanceViewModel>>();
            try
            {
                List<StoreProcedureParm> storeProcedureParms = new List<StoreProcedureParm>()
                {
                        new StoreProcedureParm (){ ParmName="LicenceNo", ParmValue= licenceNo.ToString(), isNumber = false},
                        new StoreProcedureParm (){ ParmName="ProjectSiteRefId", ParmValue= projectSiteRefId.ToString(), isNumber = true},
                        new StoreProcedureParm (){ ParmName="ProjectSiteVersion", ParmValue= projectSiteVersion.ToString(), isNumber = true},
           };
                var Clearance = await _iGeneric_SP_Repository.CallStoreProcedureReaderAsync<GetlastClearanceViewModel>("sp_GetLastClerance", storeProcedureParms);
                genericResponseTemplateModel.ResponseDataModel = Clearance;
            }
            catch (Exception ex)
            {
                genericResponseTemplateModel.HasError = true;
                genericResponseTemplateModel.ErrorDesc = ex.Message;
            }
            return genericResponseTemplateModel;
        }


        #endregion Lock Application



    }
}