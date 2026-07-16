using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using pbsamadhannetcoreapi.CommonUtiliteis;
using pbsamadhannetcoreapi.Models;
using pbsamadhannetcoreapi.Repositories;
using pbsamadhannetcoreapi.Repositories.Implementations;
using pbsamadhannetcoreapi.ViewModels;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Storage;
using System.Linq.Expressions;
using Microsoft.AspNetCore.Hosting;
using System.IO;
using System.IO.Compression;
using Microsoft.AspNetCore.Http;

namespace pbsamadhannetcoreapi.Services.Implementations
{
    public class EstablishmentService : IEstablishmentService
    {
        private readonly IGenericRepository<Establishment_GeneralDetail> _iGR_Establishment_GeneralDetail;
        private readonly IGenericRepository<Establishment_EmployerDetail> _iGR_Establishment_EmployerDetail;
        private readonly IGenericRepository<Establishment_ContractorDetail> _iGR_Establishment_ContractorDetail;
        private readonly IGenericRepository<Establishment_Migrantworker> _iGR_Establishment_Migrantworker;
        private readonly IGenericRepository<DistrictLgd> _iGR_DistrictLgd;
        private readonly IZoneRepository _iZoneRepository;
        private readonly IEstablishmentRepository _iEstablishmentRepository;
        private readonly IApplicationManagementService<Establishment_GeneralDetail> _iApplicationMamnagementService;
        private readonly IGeneric_SP_Repository _iGeneric_SP_Repository;
        public EstablishmentService(IGenericRepository<Establishment_GeneralDetail> iGR_Establishment_GeneralDetail,
            IGenericRepository<DistrictLgd> iGR_DistrictLgd, IZoneRepository iZoneRepository,
            IGenericRepository<Establishment_EmployerDetail> iGR_Establishment_EmployerDetail,
            IGenericRepository<Establishment_ContractorDetail> iGR_Establishment_ContractorDetail,
            IGenericRepository<Establishment_Migrantworker> iGR_Establishment_Migrantworker,
            IEstablishmentRepository iEstablishmentRepository,
            IApplicationManagementService<Establishment_GeneralDetail> IApplicationMamnagementService,
            AppDbContext context,
            IGeneric_SP_Repository iGeneric_SP_Repository)
        {
            _iGR_Establishment_GeneralDetail = iGR_Establishment_GeneralDetail;
            _iGR_Establishment_EmployerDetail = iGR_Establishment_EmployerDetail;
            _iGR_Establishment_ContractorDetail = iGR_Establishment_ContractorDetail;
            _iGR_Establishment_Migrantworker = iGR_Establishment_Migrantworker;
            _iGR_DistrictLgd = iGR_DistrictLgd;
            _iZoneRepository = iZoneRepository;
            _iEstablishmentRepository = iEstablishmentRepository;
            _iApplicationMamnagementService = IApplicationMamnagementService;
            _iGeneric_SP_Repository = iGeneric_SP_Repository;
        }
        #region General Details
        public async Task<GenericFormModel<Establishment_GeneralDetail>> GetEstablishmentGeneralDetail(long id, Int64 projectSiteId)
        {
            GenericFormModel<Establishment_GeneralDetail> genericFormModel = new GenericFormModel<Establishment_GeneralDetail>();
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
                    //Get Form data
                    //genericFormModel.FormModel = _iGR_Establishment_GeneralDetail.GetById(id);

                    var parentWithChildObject = await _iGR_Establishment_GeneralDetail
                       .GetAsync(x => x.EstablishmentId == id,  //Conditions         
                         null,                //Orders          
                         x => x.Application, x=>x.Application.ApplicationAction)      //Includes
                       .ConfigureAwait(false);
                    genericFormModel.FormModel = parentWithChildObject.FirstOrDefault();

                    genericFormModel.IsEditAllowed = genericFormModel.FormModel.Application.IsAllowEdit;
                    genericFormModel.IsLocked = genericFormModel.FormModel.Application.IsLocked;
                    genericFormModel.ApplicationLifeCycleStatusType = genericFormModel.FormModel.Application.ApplicationLifeCycleStatusType;
                    
                    //if form model is available
                    if (genericFormModel.FormModel != null)
                    {
                        //Get Tehsils
                        //var tehsils = await _iZoneRepository.GetTehsilsByDistrictRefId(genericFormModel.FormModel.Estb_DistrictRefId);
                        //genericFormModel.ListTemplateLists.Add(new ListTemplate()
                        //{
                        //    ListTypeCode = "Estb_Tehsils",
                        //    ListItems = tehsils.Select(x => new GenericListTemplate() { ID = x.TehsilLgdId, Text = x.TehsilName }).ToList()
                        //});

                        ////Check if communication and establihment Districts are same, if so then load same Tehsils for both types of addess
                        //if (genericFormModel.FormModel.Estb_DistrictRefId == genericFormModel.FormModel.Comm_DistrictRefId)
                        //{
                        //    genericFormModel.ListTemplateLists.Add(new ListTemplate()
                        //    {
                        //        ListTypeCode = "Comm_Tehsils",
                        //        ListItems = genericFormModel.ListTemplateLists.First(x => x.ListTypeCode == "Estb_Tehsils").ListItems
                        //    });
                        //}
                        //else // 
                        //{
                            var tehsils = await _iZoneRepository.GetTehsilsByDistrictRefId(genericFormModel.FormModel.Comm_DistrictRefId);
                            genericFormModel.ListTemplateLists.Add(new ListTemplate()
                            {
                                ListTypeCode = "Comm_Tehsils",
                                ListItems = tehsils.Select(x => new GenericListTemplate() { ID = x.ID, Text = x.Text}).ToList()
                            });
                        //}
                    }
                }
                else //New Record
                {
                    genericFormModel.FormModel = new Establishment_GeneralDetail();

                    //Allow user to edit form
                    genericFormModel.IsEditAllowed = true;
                    genericFormModel.IsLocked = false; 
                    genericFormModel.ApplicationLifeCycleStatusType = ApplicationLifeCycleStatusTypeEnum.IN_PROCESS;
                }
                genericFormModel.FormModel.ProjectSiteRefId = projectSiteId;

                //Initialization of Enum types of lists

                genericFormModel.EnumTemplateLists = new List<EnumListTemplate>();
                genericFormModel.EnumTemplateLists.Add(new EnumListTemplate()
                {
                    SelectListTypeCode = "EstablishmentTypeEnum",
                    SelectListItems = EnumOps.GetEnumAsSelectList<EstablishmentTypeEnum>()
                });

                genericFormModel.EnumTemplateLists.Add(new EnumListTemplate()
                {
                    SelectListTypeCode = "EstablishmentConstitutionTypeEnum",
                    SelectListItems = EnumOps.GetEnumAsSelectList<EstablishmentConstitutionTypeEnum>()
                });

                genericFormModel.EnumTemplateLists.Add(new EnumListTemplate()
                {
                    SelectListTypeCode = "EstablishmentBuildingTypeEnum",
                    SelectListItems = EnumOps.GetEnumAsSelectList<EstablishmentBuildingTypeEnum>()
                });

                genericFormModel.AppFormStepsList = new List<AppFormStepsInfo>();
                genericFormModel.AppFormStepsList = await _iApplicationMamnagementService.GetAppFormStepperInfo(genericFormModel.FormModel.AppRefId, ApplicationTypeEnum.REG_ESTB_OSH, (genericFormModel.FormModel != null ? genericFormModel.FormModel.EstablishmentId : id),"GD");
            }
            catch (Exception ex)
            {
                genericFormModel.HasError = true;
                genericFormModel.ErrorDesc = ex.Message;
            }
            return genericFormModel;
         }
        public async Task<GenericServiceResultTemplate> AddUpdate_GeneralDetail(Establishment_GeneralDetail formModel, string userName)
            {
            GenericServiceResultTemplate genericServiceResultTemplate = new GenericServiceResultTemplate();
            try
            {
                genericServiceResultTemplate.HasException = false;
                genericServiceResultTemplate.CustomeValidationResult = CustomeValidator<Establishment_GeneralDetail>.ValidateModel_AllProperties(formModel);
                if (genericServiceResultTemplate.CustomeValidationResult.IsValid)
                {
                    if (formModel.EstablishmentId != 0) //Existing record
                    {
                        //formModel.LastModifiedOnDate = DateTime.Now;
                        _iGR_Establishment_GeneralDetail.Update(formModel);
                        //Save changes
                        await _iGR_Establishment_GeneralDetail.SavechangeAsync();

                        //Update last modified date
                        await _iApplicationMamnagementService.UpdateAppLastModifiedDate(formModel.AppRefId);
                        genericServiceResultTemplate.ApplicationInitiateResponse = new ApplicationInitiateResponseViewModel();
                        genericServiceResultTemplate.ApplicationInitiateResponse.EntityKeyId = formModel.EstablishmentId;
                    }
                    else //New record
                    {
                        //Set null all navigation properties from model
                        formModel = GenericModelOps<Establishment_GeneralDetail>.SetNullAllNevigationProperties(formModel);
                        genericServiceResultTemplate.ApplicationInitiateResponse = await _iApplicationMamnagementService.InitiateApplication(ApplicationTypeEnum.REG_ESTB_OSH, formModel, formModel.ProjectSiteRefId, formModel.ApplicationPurposeType, userName, 0,formModel.InvestPunjab_AppId, false, 0,0, null, null, formModel.ProjectSiteVersion);
                        if (genericServiceResultTemplate.ApplicationInitiateResponse.IsApplicationCreated == false)
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

        #region Employer Details
        public async Task<GenericFormModel<Establishment_EmployerDetail>> GetEstablishmentEmployerDetail(long id)
        {
            GenericFormModel<Establishment_EmployerDetail> genericFormModel = new GenericFormModel<Establishment_EmployerDetail>();
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
                    var parentWithChildObject = await _iGR_Establishment_GeneralDetail
                        .GetAsync(x =>x.EstablishmentId == id,  //Conditions         
                          null,                //Orders          
                          x => x.Establishment_EmployerDetail, x=>x.Application)      //Includes
                        .ConfigureAwait(false);
                    genericFormModel.FormModel = parentWithChildObject.FirstOrDefault().Establishment_EmployerDetail;

                    genericFormModel.IsEditAllowed = parentWithChildObject.FirstOrDefault().Application.IsAllowEdit;
                    genericFormModel.IsLocked = parentWithChildObject.FirstOrDefault().Application.IsLocked;
                    genericFormModel.ApplicationLifeCycleStatusType = parentWithChildObject.FirstOrDefault().Application.ApplicationLifeCycleStatusType;
                    //if form model is available
                    if (genericFormModel.FormModel != null)
                    {
                        //Get Tehsils
                        var tehsils = await _iZoneRepository.GetTehsilsByDistrictRefId(genericFormModel.FormModel.Employer_DistrictRefId);
                        genericFormModel.ListTemplateLists.Add(new ListTemplate()
                        {
                            ListTypeCode = "Tehsils",
                            ListItems = tehsils.Select(x => new GenericListTemplate() { ID = x.ID, Text = x.Text }).ToList()
                        });
                    }
                }
                else //New Record
                {
                    genericFormModel.FormModel = new Establishment_EmployerDetail();

                    //Allow user to edit form
                    genericFormModel.IsEditAllowed = true;
                    genericFormModel.IsLocked = false;
                    genericFormModel.ApplicationLifeCycleStatusType = ApplicationLifeCycleStatusTypeEnum.IN_PROCESS;
                }
                genericFormModel.AppFormStepsList = await _iApplicationMamnagementService.GetAppFormStepperInfo(genericFormModel.FormModel.Establishment_GeneralDetail.AppRefId, ApplicationTypeEnum.REG_ESTB_OSH, (genericFormModel.FormModel != null ? genericFormModel.FormModel.EstablishmentRefId : id),"ED");
            }
            catch (Exception ex)
            {
                genericFormModel.HasError = true;
                genericFormModel.ErrorDesc = ex.Message;
            }
            return genericFormModel;
        }
        public async Task<GenericServiceResultTemplate> AddUpdate_EmployerDetail(Establishment_EmployerDetail formModel)
        {
            GenericServiceResultTemplate genericServiceResultTemplate = new GenericServiceResultTemplate();
            try
            {
                genericServiceResultTemplate.HasException = false;
                genericServiceResultTemplate.CustomeValidationResult = CustomeValidator<Establishment_EmployerDetail>.ValidateModel_AllProperties(formModel);
                genericServiceResultTemplate.ApplicationInitiateResponse = new ApplicationInitiateResponseViewModel();
                genericServiceResultTemplate.ApplicationInitiateResponse.EntityKeyId = formModel.EstablishmentRefId;
                if (genericServiceResultTemplate.CustomeValidationResult.IsValid)
                {
                    if (formModel.Establishment_EmployerDetailId != 0) //Existing record
                    {
                        //formModel.LastModifiedOnDate = DateTime.Now;
                        _iGR_Establishment_EmployerDetail.Update(formModel);
                        //Save changes
                        await _iGR_Establishment_EmployerDetail.SavechangeAsync();

                        //Update last modified date
                        await _iApplicationMamnagementService.UpdateAppLastModifiedDate(_iGR_Establishment_GeneralDetail.GetById(formModel.EstablishmentRefId).AppRefId);
                        //genericServiceResultTemplate.ApplicationInitiateResponse = new ApplicationInitiateResponseViewModel();
                        //genericServiceResultTemplate.ApplicationInitiateResponse.EntityKeyId = formModel.EstablishmentRefId;
                    }
                    else //New record
                    {
                        //Set null all navigation properties from model
                        formModel = GenericModelOps<Establishment_EmployerDetail>.SetNullAllNevigationProperties(formModel);
                        _iGR_Establishment_EmployerDetail.Insert(formModel);
                        await _iGR_Establishment_EmployerDetail.SavechangeAsync();

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

        #region Contractor Details
        public async Task<GenericFormModel<List<Establishment_ContractorDetail>>> GetEstablishmentContractorDetail(long id)
        {
            GenericFormModel<List<Establishment_ContractorDetail>> genericFormModel = new GenericFormModel<List<Establishment_ContractorDetail>>();
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
                    //Get Form data
                    //genericFormModel.FormModel = _iGR_Establishment_GeneralDetail.GetById(id).Establishment_ContractorDetail;
                    var parentWithChildObject = await _iGR_Establishment_GeneralDetail
                        .GetAsync(x => x.EstablishmentId == id,  //Conditions         
                          null,                //Orders          
                          x => x.Establishment_ContractorsDetail, x=>x.Application)      //Includes
                        .ConfigureAwait(false);
                    genericFormModel.FormModel = parentWithChildObject.FirstOrDefault().Establishment_ContractorsDetail.ToList();

                    genericFormModel.IsEditAllowed = parentWithChildObject.FirstOrDefault().Application.IsAllowEdit;
                    genericFormModel.IsLocked = parentWithChildObject.FirstOrDefault().Application.IsLocked;
                    genericFormModel.ApplicationLifeCycleStatusType = parentWithChildObject.FirstOrDefault().Application.ApplicationLifeCycleStatusType;
                    //if form model is available

                    //if (genericFormModel.FormModel != null)
                    //{
                    //    //Get Tehsils
                    //    var tehsils = await _iZoneRepository.GetTehsilsByDistrictRefId(genericFormModel.FormModel.Contractor_DistrictRefId);
                    //    genericFormModel.ListTemplateLists.Add(new ListTemplate()
                    //    {
                    //        ListTypeCode = "Tehsils",
                    //        ListItems = tehsils.Select(x => new GenericListTemplate() { ID = x.TehsilLgdId, Text = x.TehsilName }).ToList()
                    //    });
                    //}
                }
                else //New Record
                {
                    genericFormModel.FormModel = new List<Establishment_ContractorDetail>();

                    //Allow user to edit form
                    genericFormModel.IsEditAllowed = true;
                    genericFormModel.IsLocked = false;
                    genericFormModel.ApplicationLifeCycleStatusType = ApplicationLifeCycleStatusTypeEnum.IN_PROCESS;
                }
                genericFormModel.AppFormStepsList = await _iApplicationMamnagementService.GetAppFormStepperInfo(0, ApplicationTypeEnum.REG_ESTB_OSH, (genericFormModel.FormModel.Count!=0 ? genericFormModel.FormModel.FirstOrDefault().EstablishmentRefId : id),"CD");
            }
            catch (Exception ex)
            {
                genericFormModel.HasError = true;
                genericFormModel.ErrorDesc = ex.Message;
            }
            return genericFormModel;
        }
        public async Task<GenericServiceResultTemplate> AddUpdate_ContractorDetail(Establishment_ContractorDetail formModel)
        {
            GenericServiceResultTemplate genericServiceResultTemplate = new GenericServiceResultTemplate();
            try
            {
                //Establishment_ContractorDetail formModel = new Establishment_ContractorDetail();
                //formModel = Newtonsoft.Json.JsonConvert.DeserializeObject<Establishment_ContractorDetail>(requestData);
                genericServiceResultTemplate.HasException = false;
                genericServiceResultTemplate.CustomeValidationResult = CustomeValidator<Establishment_ContractorDetail>.ValidateModel_AllProperties(formModel);
                if (genericServiceResultTemplate.CustomeValidationResult.IsValid)
                {
                    //if (formModel.Establishment_ContractorDetailId != 0) //Existing record
                    //{
                    //    //formModel.LastModifiedOnDate = DateTime.Now;
                    //    _iGR_Establishment_ContractorDetail.Update(formModel);
                    //    //Save changes
                    //    await _iGR_Establishment_ContractorDetail.SavechangeAsync();

                    //    //Update last modified date
                    //    await _iApplicationMamnagementService.UpdateAppLastModifiedDate(_iGR_Establishment_GeneralDetail.GetById(formModel.EstablishmentRefId).AppRefId);
                    //    genericServiceResultTemplate.ApplicationInitiateResponse = new ApplicationInitiateResponseViewModel();
                    //    genericServiceResultTemplate.ApplicationInitiateResponse.EntityKeyId = formModel.EstablishmentRefId;
                    //}
                    //else //New record
                    //{
                        //Set null all navigation properties from model
                        formModel = GenericModelOps<Establishment_ContractorDetail>.SetNullAllNevigationProperties(formModel);
                        _iGR_Establishment_ContractorDetail.Insert(formModel);
                        await _iGR_Establishment_ContractorDetail.SavechangeAsync();
                    //}
                }
            }
            catch (Exception ex)
            {
                genericServiceResultTemplate.HasException = true;
                genericServiceResultTemplate.Exceptions = ex;
            }
            return genericServiceResultTemplate;
        }
        public async Task<GenericServiceResultTemplate> DeleteContractorDetail(long establishment_ContractorDetailId)
        {
            GenericServiceResultTemplate genericServiceResultTemplate = new GenericServiceResultTemplate();
            genericServiceResultTemplate.HasException = false;
            try
            {
                if (establishment_ContractorDetailId != 0)
                {
                    _iGR_Establishment_ContractorDetail.Delete(establishment_ContractorDetailId);
                    await _iGR_Establishment_ContractorDetail.SavechangeAsync();
                }
            }
            catch (Exception ex)
            {
                genericServiceResultTemplate.HasException = true;
                genericServiceResultTemplate.Exceptions = ex;
            }
            return genericServiceResultTemplate;
        }
        #endregion Contractor Details

        #region Migrant Worker Details
        public async Task<GenericFormModel<List<Establishment_Migrantworker>>> GetMigrantWorkerDetail(Int64 id)
        {
            GenericFormModel<List<Establishment_Migrantworker>> genericFormModel = new GenericFormModel<List<Establishment_Migrantworker>>();
            try
            {
                genericFormModel.ListTemplateLists = new List<ListTemplate>();
                genericFormModel.ListTemplateLists.Add(new ListTemplate()
                {
                    ListTypeCode = "Districts",
                    ListItems = _iGR_DistrictLgd.GetAll().Select(x => new GenericListTemplate() { ID = x.DistrictLgdId, Text = x.DistrictName }).ToList()
                });
                if (id != 0) //Existing Record
                {
                    var parentWithChildObject = await _iGR_Establishment_GeneralDetail
                         .GetAsync(x => x.EstablishmentId == id,  //Conditions         
                           null,                //Orders          
                           x => x.Establishment_Migrantworkers, x => x.Application)      //Includes
                         .ConfigureAwait(false);
                    genericFormModel.FormModel = parentWithChildObject.FirstOrDefault().Establishment_Migrantworkers.ToList();
                    genericFormModel.IsEditAllowed = parentWithChildObject.FirstOrDefault().Application.IsAllowEdit;
                    genericFormModel.IsLocked = parentWithChildObject.FirstOrDefault().Application.IsLocked;
                    genericFormModel.ApplicationLifeCycleStatusType = parentWithChildObject.FirstOrDefault().Application.ApplicationLifeCycleStatusType;
                }
                else //New Record
                {
                    genericFormModel.FormModel = new List<Establishment_Migrantworker>();
                    genericFormModel.IsEditAllowed = true;
                    genericFormModel.IsLocked = false;
                    genericFormModel.ApplicationLifeCycleStatusType = ApplicationLifeCycleStatusTypeEnum.IN_PROCESS;
                }
                genericFormModel.AppFormStepsList = await _iApplicationMamnagementService.GetAppFormStepperInfo(0, ApplicationTypeEnum.REG_ESTB_OSH, (genericFormModel.FormModel.Count()>0 ? genericFormModel.FormModel.FirstOrDefault().EstablishmentRefId : id), "MW");
            }
            catch (Exception ex)
            {
                genericFormModel.HasError = true;
                genericFormModel.ErrorDesc = ex.Message;
            }
            return genericFormModel;
        }
        public async Task<GenericServiceResultTemplate> AddUpdate_MigrantWorkerDetail(Establishment_Migrantworker formModel)
        {
            GenericServiceResultTemplate genericServiceResultTemplate = new GenericServiceResultTemplate();
            try
            {
                genericServiceResultTemplate.HasException = false;
                genericServiceResultTemplate.CustomeValidationResult = CustomeValidator<Establishment_Migrantworker>.ValidateModel_AllProperties(formModel);
                if (genericServiceResultTemplate.CustomeValidationResult.IsValid)
                {
                    formModel = GenericModelOps<Establishment_Migrantworker>.SetNullAllNevigationProperties(formModel);
                    _iGR_Establishment_Migrantworker.Insert(formModel);
                    await _iGR_Establishment_Migrantworker.SavechangeAsync();
                }
            }
            catch (Exception ex)
            {
                genericServiceResultTemplate.HasException = true;
                genericServiceResultTemplate.Exceptions = ex;
            }
            return genericServiceResultTemplate;
        }
        public async Task<GenericServiceResultTemplate> DeleteMigrantWorkerDetail(long establishmentMigrantworkerId)
        {
            GenericServiceResultTemplate genericServiceResultTemplate = new GenericServiceResultTemplate();
            genericServiceResultTemplate.HasException = false;
            try
            {
                if (establishmentMigrantworkerId != 0)
                {
                    _iGR_Establishment_Migrantworker.Delete(establishmentMigrantworkerId);
                    await _iGR_Establishment_Migrantworker.SavechangeAsync();
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

        #region Establishment Details
        public async Task<GenericFormModel<EstablishmentDetailViewModel>> GetEstablishmentDetail(long id)
        {
            GenericFormModel<EstablishmentDetailViewModel> genericFormModel = new GenericFormModel<EstablishmentDetailViewModel>();
            try
            {
                genericFormModel.ListTemplateLists = new List<ListTemplate>();
                genericFormModel.FormModel = new EstablishmentDetailViewModel();
                genericFormModel.FormModel.GeneralDetail = new Establishment_GeneralDetail();
                genericFormModel.FormModel.EmployerDetail= new Establishment_EmployerDetail();
                genericFormModel.FormModel.ContractorDetail = new Establishment_ContractorDetail();

                var parentWithChildObject = await _iGR_Establishment_GeneralDetail
                        .GetAsync(x => x.EstablishmentId == id,  //Conditions         
                          null,                //Orders          
                          x => x.Establishment_EmployerDetail, x=>x.Establishment_ContractorsDetail, x=>x.Application)      //Includes
                        .ConfigureAwait(false);
                
                genericFormModel.FormModel.GeneralDetail = parentWithChildObject.FirstOrDefault();
                genericFormModel.FormModel.EmployerDetail = parentWithChildObject.FirstOrDefault().Establishment_EmployerDetail;
                genericFormModel.FormModel.ContractorDetail = parentWithChildObject.FirstOrDefault().Establishment_ContractorsDetail.FirstOrDefault();
               
                genericFormModel.FormModel.IsLocked = parentWithChildObject.FirstOrDefault().Application.IsLocked;
                genericFormModel.FormModel.IsFeeApplicable = parentWithChildObject.FirstOrDefault().Application.IsFeeApplicable;
                genericFormModel.ApplicationLifeCycleStatusType = parentWithChildObject.FirstOrDefault().Application.ApplicationLifeCycleStatusType;

                genericFormModel.AppFormStepsList = new List<AppFormStepsInfo>();
                genericFormModel.AppFormStepsList = await _iApplicationMamnagementService.GetAppFormStepperInfo(genericFormModel.FormModel.GeneralDetail.AppRefId, ApplicationTypeEnum.REG_ESTB_OSH, (genericFormModel.FormModel != null ? genericFormModel.FormModel.GeneralDetail.EstablishmentId : id),"LOCK");
            }
            catch (Exception ex)
            {
                genericFormModel.HasError = true;
                genericFormModel.ErrorDesc = ex.Message;
            }
            return genericFormModel;
        }
        #endregion Establishment Details

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
                //genericServiceResultTemplate.ApplicationInitiateResponse.EntityKeyId = formModel.EstablishmentRefId;
                if (appRefId != 0)
                {

                    if (await _iApplicationMamnagementService.LockApplication(appRefId, AppActionType, remarks, ApplicationTypeEnum.REG_ESTB_OSH) != "")
                    {
                        genericServiceResultTemplate.ApplicationInitiateResponse.IsApplicationCreated = true;
                    }
                    else
                    {
                        genericServiceResultTemplate.ApplicationInitiateResponse.IsApplicationCreated = false;
                        genericServiceResultTemplate.CustomeValidationResult.IsValid = false;
                    }

                    //Establishment_GeneralDetail establishment  = _iGR_Establishment_GeneralDetail.GetById(id);
                    //if (establishment != null)
                    //{
                        
                    //}
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

        public async Task<GenericFormModel<IntReturn>> FindDuplicateGST(string gstNumber, Int64 establishmentId)
        {
            GenericFormModel<IntReturn> genericFormModel = new GenericFormModel<IntReturn>();
            try
            {
                List<StoreProcedureParm> storeProcedureParms = new List<StoreProcedureParm>()
                {
                    new StoreProcedureParm (){ ParmName="GstNumber", ParmValue=gstNumber, isNumber=false},
                    new StoreProcedureParm (){ ParmName="EstablishmentId", ParmValue=establishmentId.ToString(), isNumber=true}
                };
                genericFormModel.FormModel = await _iGeneric_SP_Repository.CallStoreProcedureReaderScalerAsync("sp_Establishment_FindDuplicateGST", storeProcedureParms);
            }
            catch (Exception ex)
            {
                genericFormModel.HasError = true;
                genericFormModel.ErrorDesc = ex.Message;
            }
            return genericFormModel;
        }
        #endregion Lock Application
    }
}
