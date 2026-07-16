using pbsamadhannetcoreapi.CommonUtiliteis;
using pbsamadhannetcoreapi.Models;
using pbsamadhannetcoreapi.Repositories.Implementations;
using pbsamadhannetcoreapi.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
namespace pbsamadhannetcoreapi.Services.Implementations
{
    public class ContractLabourService : IContractLabourService
    {
        private readonly IGenericRepository<Contractor_GeneralDetail> _iGR_Contractor_GeneralDetail;
        private readonly IGenericRepository<Contractor_PrincipalEmployer> _iGR_Contractor_PrincipalEmployer;
        private readonly IZoneRepository _iZoneRepository;
        private readonly IGenericRepository<DistrictLgd> _iGR_DistrictLgd;
        private readonly IApplicationManagementService<Contractor_GeneralDetail> _iApplicationMamnagementService;
        private readonly IGenericRepository<ContractLabourAndEstablishmentMapping> _iGR_ContractLabourAndEstablishmentMapping;
        public ContractLabourService(IGenericRepository<Contractor_GeneralDetail> iGR_ContractorGeneralDetail,
            IGenericRepository<Contractor_PrincipalEmployer> iGR_ContractorPrincipalEmployer,
            IZoneRepository iZoneRepository, IApplicationManagementService<Contractor_GeneralDetail> IApplicationMamnagementService,
            IGenericRepository<DistrictLgd> iGR_DistrictLgd,
            IGenericRepository<ContractLabourAndEstablishmentMapping> iGR_ContractLabourAndEstablishmentMapping)
        {
            _iGR_Contractor_GeneralDetail = iGR_ContractorGeneralDetail;
            _iGR_Contractor_PrincipalEmployer = iGR_ContractorPrincipalEmployer;
            _iZoneRepository = iZoneRepository;
            _iApplicationMamnagementService = IApplicationMamnagementService;
            _iGR_DistrictLgd = iGR_DistrictLgd;
            _iGR_ContractLabourAndEstablishmentMapping = iGR_ContractLabourAndEstablishmentMapping;
        }

        #region General Details
        public async Task<GenericFormModel<Contractor_GeneralDetail>> GetContractLabourGeneralDetail(long id)
        {
            GenericFormModel<Contractor_GeneralDetail> genericFormModel = new GenericFormModel<Contractor_GeneralDetail>();
            try
            {
                genericFormModel.ListTemplateLists = new List<ListTemplate>();

                //Initialization of list templates
                genericFormModel.ListTemplateLists.Add(new ListTemplate()
                {
                    ListTypeCode = "Districts",
                    ListItems = _iGR_DistrictLgd.GetAll().Select(x => new GenericListTemplate() { ID = x.DistrictLgdId, Text = x.DistrictName }).ToList()
                });
                if (id != 0) //Existing Record
                {
                    //Get Form data
                    var parentWithChildObject = await _iGR_Contractor_GeneralDetail
                       .GetAsync(x => x.ContractLabourId == id,  //Conditions         
                         null,                //Orders          
                         x => x.Application)      //Includes
                       .ConfigureAwait(false);

                    genericFormModel.FormModel = parentWithChildObject.FirstOrDefault();

                    genericFormModel.IsEditAllowed = genericFormModel.FormModel.Application.IsAllowEdit;
                    genericFormModel.IsLocked = genericFormModel.FormModel.Application.IsLocked;
                    genericFormModel.ApplicationLifeCycleStatusType = genericFormModel.FormModel.Application.ApplicationLifeCycleStatusType;

                    //if form model is available
                    if (genericFormModel.FormModel != null)
                    {
                        var tehsils = await _iZoneRepository.GetTehsilsByDistrictRefId(genericFormModel.FormModel.DistrictRefId);
                        genericFormModel.ListTemplateLists.Add(new ListTemplate()
                        {
                            ListTypeCode = "Comm_Tehsils",
                            ListItems = tehsils.Select(x => new GenericListTemplate() { ID = x.ID, Text = x.Text }).ToList()
                        });
                    }
                }
                else //New Record
                {
                    genericFormModel.FormModel = new Contractor_GeneralDetail();

                    //Allow user to edit form
                    genericFormModel.IsEditAllowed = true;
                    genericFormModel.IsLocked = false;
                    genericFormModel.ApplicationLifeCycleStatusType = ApplicationLifeCycleStatusTypeEnum.IN_PROCESS;
                }
                //genericFormModel.FormModel.EstablishmentRefId = establishmentRefId;

                genericFormModel.AppFormStepsList = new List<AppFormStepsInfo>();
                genericFormModel.AppFormStepsList = await _iApplicationMamnagementService.GetAppFormStepperInfo(genericFormModel.FormModel.AppRefId, ApplicationTypeEnum.CONTRACT_LABOUR, (genericFormModel.FormModel != null ? genericFormModel.FormModel.ContractLabourId : id), "GD");
            }
            catch (Exception ex)
            {
                genericFormModel.HasError = true;
                genericFormModel.ErrorDesc = ex.Message;
            }
            return genericFormModel;
        }
        public async Task<GenericServiceResultTemplate> AddUpdate_GeneralDetail(Contractor_GeneralDetail formModel, string userName)
        {
            GenericServiceResultTemplate genericServiceResultTemplate = new GenericServiceResultTemplate();
            try
            {
                genericServiceResultTemplate.HasException = false;
                genericServiceResultTemplate.CustomeValidationResult = CustomeValidator<Contractor_GeneralDetail>.ValidateModel_AllProperties(formModel);
                if (genericServiceResultTemplate.CustomeValidationResult.IsValid)
                {
                    if (formModel.ContractLabourId != 0) //Existing record
                    {
                        //formModel.LastModifiedOnDate = DateTime.Now;
                        _iGR_Contractor_GeneralDetail.Update(formModel);
                        //Save changes
                        await _iGR_Contractor_GeneralDetail.SavechangeAsync();

                        //Update last modified date
                        await _iApplicationMamnagementService.UpdateAppLastModifiedDate(formModel.AppRefId);
                        genericServiceResultTemplate.ApplicationInitiateResponse = new ApplicationInitiateResponseViewModel();
                        //genericServiceResultTemplate.ApplicationInitiateResponse.EntityKeyId = formModel.EstablishmentRefId;
                    }
                    else //New record
                    {
                        //Set null all navigation properties from model
                        formModel = GenericModelOps<Contractor_GeneralDetail>.SetNullAllNevigationProperties(formModel);
                        genericServiceResultTemplate.ApplicationInitiateResponse = await _iApplicationMamnagementService.InitiateApplication(ApplicationTypeEnum.CONTRACT_LABOUR, formModel, formModel.ProjectSiteRefId, ApplicationPurposeTypeEnum.GRANT_LICENCE, userName,0,formModel.InvestPunjab_AppId, false, 0, 0, null, null, formModel.ProjectSiteVersion);
                        if (genericServiceResultTemplate.ApplicationInitiateResponse.IsApplicationCreated == false)
                        {
                            genericServiceResultTemplate.HasException = true;
                            genericServiceResultTemplate.Exceptions = new Exception();
                        }
                        else
                        {
                            if (formModel.ApplicationPurposeType == ApplicationPurposeTypeEnum.GRANT_LICENCE)
                            {
                                ContractLabourAndEstablishmentMapping contractLabourAndEstablishment = new ContractLabourAndEstablishmentMapping()
                                {
                                    EstablishmentRefId = formModel.EstablishmentRefId,
                                    ContractLabourRefId = genericServiceResultTemplate.ApplicationInitiateResponse.EntityKeyId
                                };
                                _iGR_ContractLabourAndEstablishmentMapping.Insert(contractLabourAndEstablishment);
                                await _iGR_ContractLabourAndEstablishmentMapping.SavechangeAsync();
                            }
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

        #region Principal Employer Details
        public async Task<GenericFormModel<Contractor_PrincipalEmployer>> GetPricipalEmployerDetail(Int64 id)
        {
            GenericFormModel<Contractor_PrincipalEmployer> genericFormModel = new GenericFormModel<Contractor_PrincipalEmployer>();
            try
            {
                genericFormModel.ListTemplateLists = new List<ListTemplate>();

                //Initialization of list templates
                genericFormModel.ListTemplateLists.Add(new ListTemplate()
                {
                    ListTypeCode = "Districts",
                    ListItems = _iGR_DistrictLgd.GetAll().Select(x => new GenericListTemplate() { ID = x.DistrictLgdId, Text = x.DistrictName }).ToList()
                });
                // Check if this is an existing record
                if (id != 0) //Existing Record
                {
                    var parentWithChildObject = await _iGR_Contractor_GeneralDetail
                        .GetAsync(x => x.ContractLabourId == id,  //Conditions         
                          null,                //Orders          
                          x => x.Contractor_PrincipalEmployer, x => x.Application)      //Includes
                        .ConfigureAwait(false);
                    genericFormModel.FormModel = parentWithChildObject.FirstOrDefault().Contractor_PrincipalEmployer;

                    genericFormModel.IsEditAllowed = parentWithChildObject.FirstOrDefault().Application.IsAllowEdit;
                    genericFormModel.IsLocked = parentWithChildObject.FirstOrDefault().Application.IsLocked;
                    genericFormModel.ApplicationLifeCycleStatusType = parentWithChildObject.FirstOrDefault().Application.ApplicationLifeCycleStatusType;
                    //if form model is available
                    if (genericFormModel.FormModel != null)
                    {
                        //Get Tehsils
                        var tehsils = await _iZoneRepository.GetTehsilsByDistrictRefId(genericFormModel.FormModel.DistrictRefId);
                        genericFormModel.ListTemplateLists.Add(new ListTemplate()
                        {
                            ListTypeCode = "Tehsils",
                            ListItems = tehsils.Select(x => new GenericListTemplate() { ID = x.ID, Text = x.Text }).ToList()
                        });
                    }
                }
                else //New Record
                {
                    genericFormModel.FormModel = new Contractor_PrincipalEmployer();

                    //Allow user to edit form
                    genericFormModel.IsEditAllowed = true;
                    genericFormModel.IsLocked = false;
                    genericFormModel.ApplicationLifeCycleStatusType = ApplicationLifeCycleStatusTypeEnum.IN_PROCESS;
                }
                genericFormModel.AppFormStepsList = await _iApplicationMamnagementService.GetAppFormStepperInfo(genericFormModel.FormModel.Contractor_GeneralDetail.AppRefId, ApplicationTypeEnum.CONTRACT_LABOUR, (genericFormModel.FormModel != null ? genericFormModel.FormModel.ContractorRefId : id), "PE");
            }
            catch (Exception ex)
            {
                genericFormModel.HasError = true;
                genericFormModel.ErrorDesc = ex.Message;
            }
            return genericFormModel;
        }
        public async Task<GenericServiceResultTemplate> AddUpdate_PricipalEmployerDetail(Contractor_PrincipalEmployer formModel)
        {
            GenericServiceResultTemplate genericServiceResultTemplate = new GenericServiceResultTemplate();
            try
            {
                genericServiceResultTemplate.HasException = false;
                genericServiceResultTemplate.CustomeValidationResult = CustomeValidator<Contractor_PrincipalEmployer>.ValidateModel_AllProperties(formModel);
                if (genericServiceResultTemplate.CustomeValidationResult.IsValid)
                {
                    formModel = GenericModelOps<Contractor_PrincipalEmployer>.SetNullAllNevigationProperties(formModel);
                    _iGR_Contractor_PrincipalEmployer.Insert(formModel);
                    await _iGR_Contractor_PrincipalEmployer.SavechangeAsync();
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

        #region Contract Labour Details
        public async Task<GenericFormModel<ContractLabourViewModels>> GetContractLabourDetail(long id)
        {
            GenericFormModel<ContractLabourViewModels> genericFormModel = new GenericFormModel<ContractLabourViewModels>();
            try
            {
                genericFormModel.ListTemplateLists = new List<ListTemplate>();
                genericFormModel.FormModel = new ContractLabourViewModels();
                genericFormModel.FormModel.GeneralDetail = new Contractor_GeneralDetail();
                genericFormModel.FormModel.PrincipalEmployerDetail = new Contractor_PrincipalEmployer();

                var parentWithChildObject = await _iGR_Contractor_GeneralDetail
                        .GetAsync(x => x.ContractLabourId == id,  //Conditions         
                          null,                //Orders          
                          x => x.Contractor_PrincipalEmployer, x => x.Application)      //Includes
                        .ConfigureAwait(false);

                genericFormModel.FormModel.GeneralDetail = parentWithChildObject.FirstOrDefault();
                genericFormModel.FormModel.PrincipalEmployerDetail = parentWithChildObject.FirstOrDefault().Contractor_PrincipalEmployer;

                genericFormModel.FormModel.IsLocked = parentWithChildObject.FirstOrDefault().Application.IsLocked;
                genericFormModel.FormModel.IsFeeApplicable = parentWithChildObject.FirstOrDefault().Application.IsFeeApplicable;
                genericFormModel.ApplicationLifeCycleStatusType = parentWithChildObject.FirstOrDefault().Application.ApplicationLifeCycleStatusType;

                genericFormModel.AppFormStepsList = new List<AppFormStepsInfo>();
                genericFormModel.AppFormStepsList = await _iApplicationMamnagementService.GetAppFormStepperInfo(genericFormModel.FormModel.GeneralDetail.AppRefId, ApplicationTypeEnum.CONTRACT_LABOUR, (genericFormModel.FormModel != null ? genericFormModel.FormModel.GeneralDetail.ContractLabourId : id), "LOCK");
            }
            catch (Exception ex)
            {
                genericFormModel.HasError = true;
                genericFormModel.ErrorDesc = ex.Message;
            }
            return genericFormModel;
        }
        #endregion Contract Labour Details

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

                    if (await _iApplicationMamnagementService.LockApplication(appRefId, AppActionType, remarks, ApplicationTypeEnum.CONTRACT_LABOUR) != "")
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
        #endregion Lock Application
    }
}
