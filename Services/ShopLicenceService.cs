using EFCore.BulkExtensions;
using pbsamadhannetcoreapi.CommonUtiliteis;
using pbsamadhannetcoreapi.Models;
using pbsamadhannetcoreapi.Repositories.Implementations;
using pbsamadhannetcoreapi.Services.Implementations;
using pbsamadhannetcoreapi.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace pbsamadhannetcoreapi.Services
{
    public class ShopLicenceService : IShopLicenceService
    {
        private readonly IGenericRepository<ShopLicence_GeneralDetail> _iGR_ShopLicence_GeneralDetail;
        private readonly IApplicationManagementService<ShopLicence_GeneralDetail> _iApplicationMamnagementService;
        private readonly IThirdPartyInegrationsService _iThirdPartyInegrationsService;
        private readonly IGeneric_SP_Repository _iGeneric_SP_Repository;
        private readonly IProjectSiteService _iProjectSiteService;
        private readonly IGenericRepository<ShopLicence_EmployeeDetail> _iGR_ShopLicence_EmployeeDetail;
        private readonly AppDbContext _context;
        private IApplicationManagementRepository<ShopLicence_GeneralDetail> _iApplicationMamnagementRepository;
        public ShopLicenceService(IGenericRepository<ShopLicence_GeneralDetail> iGR_ShopLicence_GeneralDetail,
             IApplicationManagementService<ShopLicence_GeneralDetail> IApplicationMamnagementService,
             IThirdPartyInegrationsService iThirdPartyInegrationsService,
             IGeneric_SP_Repository iGeneric_SP_Repository,
             IProjectSiteService iProjectSiteService,
             IGenericRepository<ShopLicence_EmployeeDetail> iGR_ShopLicence_EmployeeDetail,
             AppDbContext context,
             IApplicationManagementRepository<ShopLicence_GeneralDetail> iApplicationMamnagementRepository)
        {
            _iGR_ShopLicence_GeneralDetail = iGR_ShopLicence_GeneralDetail;
            _iApplicationMamnagementService = IApplicationMamnagementService;
            _iThirdPartyInegrationsService = iThirdPartyInegrationsService;
            _iGeneric_SP_Repository = iGeneric_SP_Repository;
            _iProjectSiteService = iProjectSiteService;
            _iGR_ShopLicence_EmployeeDetail = iGR_ShopLicence_EmployeeDetail;
            _context = context;
            _iApplicationMamnagementRepository = iApplicationMamnagementRepository;
        }

        #region General Details
        public async Task<GenericFormModel<ShopLicence_GeneralDetail>> GetShopLicenceGeneralDetail(long id, long projectSiteId)
        {
            GenericFormModel<ShopLicence_GeneralDetail> genericFormModel = new GenericFormModel<ShopLicence_GeneralDetail>();
            try
            {
                genericFormModel.ListTemplateLists = new List<ListTemplate>();
                if (id != 0) //Existing Record
                {
                    var parentWithChildObject = await _iGR_ShopLicence_GeneralDetail
                       .GetAsync(x => x.AppRefId == id,  //Conditions         
                         null,                //Orders
                         x=>x.ShopLicence_EmployeeDetails,
                         x => x.Application, x => x.Application.ApplicationAction)      //Includes
                       .ConfigureAwait(false);
                    genericFormModel.FormModel = parentWithChildObject.FirstOrDefault();
                    //genericFormModel.FormModel.EmployeeList = parentWithChildObject.FirstOrDefault().ShopLicence_EmployeeDetails.ToList();
                    genericFormModel.IsEditAllowed = genericFormModel.FormModel.Application.IsAllowEdit;
                    genericFormModel.IsLocked = genericFormModel.FormModel.Application.IsLocked;
                    genericFormModel.ApplicationLifeCycleStatusType = genericFormModel.FormModel.Application.ApplicationLifeCycleStatusType;

                    var siteId = await _iProjectSiteService.GetProjectSiteRefIdByAppId(id);
                    var projectProfileInfo = await _iProjectSiteService.GetProjectSiteDetail(siteId.ResponseDataModel);
                    genericFormModel.FormModel.LabourCircleRefId = projectProfileInfo.FormModel.LabourCircleRefId;

                }
                
                else //New Record
                {
                    genericFormModel.FormModel = new ShopLicence_GeneralDetail();

                    //Allow user to edit form
                    genericFormModel.IsEditAllowed = true;
                    genericFormModel.IsLocked = false;
                    genericFormModel.ApplicationLifeCycleStatusType = ApplicationLifeCycleStatusTypeEnum.NOT_SUBMITTED;
                    genericFormModel.FormModel.LabourCircleRefId = 0;
                }
                genericFormModel.FormModel.ProjectSiteRefId = projectSiteId;

                //Initialization of Enum types of lists
                
                genericFormModel.EnumTemplateLists = new List<EnumListTemplate>();
                genericFormModel.EnumTemplateLists.Add(new EnumListTemplate()
                {
                    SelectListTypeCode = "ClosingDayTypeEnum",
                    SelectListItems = EnumOps.GetEnumAsSelectList<ClosingDayTypeEnum>()
                });

                genericFormModel.EnumTemplateLists.Add(new EnumListTemplate()
                {
                    SelectListTypeCode = "EstablishmentConstitutionTypeEnum",
                    SelectListItems = EnumOps.GetEnumAsSelectList<EstablishmentConstitutionTypeEnum>()
                });

                genericFormModel.EnumTemplateLists.Add(new EnumListTemplate()
                {
                    SelectListTypeCode = "ShopTypeEnum",
                    SelectListItems = EnumOps.GetEnumAsSelectList<ShopTypeEnum>()
                });
                
                genericFormModel.AppFormStepsList = new List<AppFormStepsInfo>();
                genericFormModel.AppFormStepsList = await _iApplicationMamnagementService.GetAppFormStepperInfo(id, ApplicationTypeEnum.SHOP_LICENCE, (genericFormModel.FormModel != null ? genericFormModel.FormModel.ShopLicenceId : id), "GD");
            }
            catch (Exception ex)
            {
                genericFormModel.HasError = true;
                genericFormModel.ErrorDesc = ex.Message;
                throw ex;
            }
            return genericFormModel;
        }

        //***** SHIFTTED TO CRUD 

        //public async Task<GenericServiceResultTemplate> AddUpdate_ShopLicenceDetail(ShopLicence_GeneralDetail formModel, string id)
        //{
        //    GenericServiceResultTemplate genericServiceResultTemplate = new GenericServiceResultTemplate();
        //    try
        //    {
        //        genericServiceResultTemplate.HasException = false;
        //        genericServiceResultTemplate.CustomeValidationResult = CustomeValidator<ShopLicence_GeneralDetail>.ValidateModel_AllProperties(formModel);
        //        if (genericServiceResultTemplate.CustomeValidationResult.IsValid)
        //        {
        //            if (formModel.ShopLicenceId != 0) //Existing record
        //            {
        //                _iGR_ShopLicence_GeneralDetail.Update(formModel);
        //                await _iGR_ShopLicence_GeneralDetail.SavechangeAsync();

        //                //Update last modified date
        //                await _iApplicationMamnagementService.UpdateAppLastModifiedDate(formModel.AppRefId);
        //                genericServiceResultTemplate.ApplicationInitiateResponse = new ApplicationInitiateResponseViewModel();
        //                genericServiceResultTemplate.ApplicationInitiateResponse.EntityKeyId = formModel.ShopLicenceId;
        //                genericServiceResultTemplate.ApplicationInitiateResponse.AppId = formModel.AppRefId;

        //                // Update Circle
        //                await _iProjectSiteService.SetProjectSiteCircle(formModel.ProjectSiteRefId, 0, formModel.LabourCircleRefId,0);

        //                //if (formModel.EmployeeList != null)
        //                //{
        //                //    formModel.EmployeeList = formModel.EmployeeList.Select(x => { x.ShopLicenceRefId = formModel.ShopLicenceId; return x; }).ToList();
        //                //    var existingEmployees = _context.ShopLicence_EmployeeDetails.Where(x => x.ShopLicenceRefId == formModel.ShopLicenceId).ToList();
        //                //    if (existingEmployees.Count() > 0)
        //                //    {
        //                //        await _context.BulkDeleteAsync<ShopLicence_EmployeeDetail>(existingEmployees);
        //                //    }
        //                //    await _context.BulkInsertAsync<ShopLicence_EmployeeDetail>(formModel.EmployeeList.ToList());

        //                //    await _iApplicationMamnagementRepository.SwitchApplicationLifeCycleType(formModel.AppRefId, ApplicationLifeCycleStatusTypeEnum.NOT_SUBMITTED);
        //                //}

        //                // Check has status share to business first
        //                var isStatusShared = await _iThirdPartyInegrationsService.HasShareStatusToBusinessFirst(genericServiceResultTemplate.ApplicationInitiateResponse.AppId);
        //                if(isStatusShared!=null && !isStatusShared.HasError && !isStatusShared.ResponseDataModel)
        //                {
        //                    // Update Native Appid by IPIN
        //                    await _iThirdPartyInegrationsService.UpdateBusinessFirstRequestNativeAppIdByIPin(formModel.IPin, formModel.InvestPunjab_AppId, genericServiceResultTemplate.ApplicationInitiateResponse.AppId, ApplicationTypeEnum.SHOP_LICENCE, formModel.ApplicationPurposeType);

        //                    // Share status to Invest Punjab Portal
        //                    await _iThirdPartyInegrationsService.ShareStatusToBusinessFirst(genericServiceResultTemplate.ApplicationInitiateResponse.AppId, ApplicationTypeEnum.SHOP_LICENCE, AppActionTypeEnum.APP_SAVE_DRAFT);
        //                }

        //            }
        //            else //New record
        //            {
        //                //Set null all navigation properties from model
        //                formModel = GenericModelOps<ShopLicence_GeneralDetail>.SetNullAllNevigationProperties(formModel);

        //                genericServiceResultTemplate.ApplicationInitiateResponse = await _iApplicationMamnagementService.InitiateApplication(ApplicationTypeEnum.SHOP_LICENCE, formModel, formModel.ProjectSiteRefId, formModel.ApplicationPurposeType, id, formModel.IPin, formModel.InvestPunjab_AppId, false, 0,0, null,null, formModel.ProjectSiteVersion);

        //                if(genericServiceResultTemplate.ApplicationInitiateResponse.IsApplicationCreated== true)
        //                {
        //                    //var shopLicenceId = _context.ShopLicence_GeneralDetails.Where(x => x.AppRefId == genericServiceResultTemplate.ApplicationInitiateResponse.AppId).FirstOrDefault();
        //                    //formModel.EmployeeList = formModel.EmployeeList.Select(x => { x.ShopLicenceRefId = genericServiceResultTemplate.ApplicationInitiateResponse.EntityKeyId; return x; }).ToList();
        //                    //await _context.BulkInsertAsync<ShopLicence_EmployeeDetail>(formModel.EmployeeList.ToList());

        //                    // update LabourCircle Id in ProjectSite Table
        //                    var projectSiteDetail = await _iApplicationMamnagementService.GetProjectSiteByAppRefId(genericServiceResultTemplate.ApplicationInitiateResponse.AppId);
        //                    await _iProjectSiteService.SetProjectSiteCircle(projectSiteDetail.FormModel.ProjectSiteId, 0, formModel.LabourCircleRefId,0);

        //                    // Update Native Appid by IPIN
        //                    await _iThirdPartyInegrationsService.UpdateBusinessFirstRequestNativeAppIdByIPin(formModel.IPin, formModel.InvestPunjab_AppId, genericServiceResultTemplate.ApplicationInitiateResponse.AppId, ApplicationTypeEnum.SHOP_LICENCE, formModel.ApplicationPurposeType);

        //                    // Share status to Invest Punjab Portal
        //                    await _iThirdPartyInegrationsService.ShareStatusToBusinessFirst(genericServiceResultTemplate.ApplicationInitiateResponse.AppId, ApplicationTypeEnum.SHOP_LICENCE, AppActionTypeEnum.APP_SAVE_DRAFT);
        //                }
        //                else if (genericServiceResultTemplate.ApplicationInitiateResponse.IsApplicationCreated == false)
        //                {
        //                    genericServiceResultTemplate.HasException = true;
        //                    genericServiceResultTemplate.Exceptions = new Exception();
        //                }
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        genericServiceResultTemplate.HasException = true;
        //        genericServiceResultTemplate.Exceptions = ex;
        //        throw ex;
        //    }
        //    return genericServiceResultTemplate;
        //}

        #endregion General Details


        public async Task<GenericFormModel<EmployeeDetailViewModel>> GetShopLicenceEmlpyeeDetail(Int64 id, Int64 projectSiteId)
        {
            GenericFormModel<EmployeeDetailViewModel> genericFormModel = new GenericFormModel<EmployeeDetailViewModel>();
            try
            {
                genericFormModel.ListTemplateLists = new List<ListTemplate>();

                var parentWithChildObject = await _iGR_ShopLicence_GeneralDetail
                    .GetAsync(x => x.AppRefId == id,  //Conditions         
                        null,                //Orders
                       x => x.Application, x => x.Application.ApplicationAction)      //Includes
                    .ConfigureAwait(false);

                //List<StoreProcedureParm> storeProcedureParms = new List<StoreProcedureParm>()
                //{
                //    new StoreProcedureParm (){ ParmName="AppRefId", ParmValue=id.ToString(), isNumber=true},
                //};
                //var emps = await _iGeneric_SP_Repository.CallStoreProcedureReaderAsync<ShopLicence_EmployeeDetail>("sp_ShopLicence_GetAllEmployees", storeProcedureParms);

                genericFormModel.FormModel = new EmployeeDetailViewModel();
                genericFormModel.FormModel.EmployeesList = new List<ShopLicence_EmployeeDetail>();
                genericFormModel.FormModel.IsHavingEmployee = parentWithChildObject.FirstOrDefault().IsHavingEmployee;
                //genericFormModel.FormModel.EmployeesList = emps;
                genericFormModel.IsEditAllowed = parentWithChildObject.FirstOrDefault().Application.IsAllowEdit;
                genericFormModel.IsLocked = parentWithChildObject.FirstOrDefault().Application.IsLocked;
                genericFormModel.ApplicationLifeCycleStatusType = parentWithChildObject.FirstOrDefault().Application.ApplicationLifeCycleStatusType;
                
                genericFormModel.AppFormStepsList = new List<AppFormStepsInfo>();
                genericFormModel.AppFormStepsList = await _iApplicationMamnagementService.GetAppFormStepperInfo(id, ApplicationTypeEnum.SHOP_LICENCE, (genericFormModel.FormModel != null ? parentWithChildObject.FirstOrDefault().ShopLicenceId : id), "ED");
            }
            catch (Exception ex)
            {
                genericFormModel.HasError = true;
                genericFormModel.ErrorDesc = ex.Message;
                throw ex;
            }
            return genericFormModel;
        }


        //***** SHIFTTED TO CRUD 
        //public async Task<GenericServiceResultTemplate> AddUpdate_ShopLicenceEmployeeDetail(EmployeeDetailViewModel formModel)
        //{
        //    GenericServiceResultTemplate genericServiceResultTemplate = new GenericServiceResultTemplate();
        //    try
        //    {
        //        genericServiceResultTemplate.HasException = false;
        //        var shopData = await _iGR_ShopLicence_GeneralDetail.GetAsync(x => x.ShopLicenceId == formModel.ShopLicenceId).ConfigureAwait(false);
        //        shopData.FirstOrDefault().IsHavingEmployee = formModel.IsHavingEmployee;
        //        _iGR_ShopLicence_GeneralDetail.Update(shopData.FirstOrDefault());
        //        await _iGR_ShopLicence_GeneralDetail.SavechangeAsync();

        //        //Update last modified date
        //        await _iApplicationMamnagementService.UpdateAppLastModifiedDate(formModel.AppRefId);
        //        genericServiceResultTemplate.ApplicationInitiateResponse = new ApplicationInitiateResponseViewModel();
        //        genericServiceResultTemplate.ApplicationInitiateResponse.EntityKeyId = formModel.ShopLicenceId;
        //        genericServiceResultTemplate.ApplicationInitiateResponse.AppId = formModel.AppRefId;
        //    }
        //    catch (Exception ex)
        //    {
        //        genericServiceResultTemplate.HasException = true;
        //        genericServiceResultTemplate.Exceptions = ex;
        //        throw ex;
        //    }
        //    return genericServiceResultTemplate;
        //}



        #region Shop Licence Detail
        public async Task<GenericFormModel<ShopLicenceViewModel>> GetShopLicenceDetail(long id)
        {
            GenericFormModel<ShopLicenceViewModel> genericFormModel = new GenericFormModel<ShopLicenceViewModel>();
            try
            {
                genericFormModel.ListTemplateLists = new List<ListTemplate>();
                genericFormModel.FormModel = new ShopLicenceViewModel();
                genericFormModel.FormModel.GeneralDetail = new ShopLicence_GeneralDetail();

                var parentWithChildObject = await _iGR_ShopLicence_GeneralDetail
                       .GetAsync(x => x.AppRefId == id,  //Conditions         
                         null,                //Orders          
                         x => x.Application, x => x.ShopLicence_EmployeeDetails)
                       .ConfigureAwait(false);

                //Initialization of Enum types of lists
                genericFormModel.EnumTemplateLists = new List<EnumListTemplate>();
                genericFormModel.EnumTemplateLists.Add(new EnumListTemplate()
                {
                    SelectListTypeCode = "ClosingDayTypeEnum",
                    SelectListItems = EnumOps.GetEnumAsSelectList<ClosingDayTypeEnum>()
                });

                genericFormModel.EnumTemplateLists.Add(new EnumListTemplate()
                {
                    SelectListTypeCode = "EstablishmentConstitutionTypeEnum",
                    SelectListItems = EnumOps.GetEnumAsSelectList<EstablishmentConstitutionTypeEnum>()
                });

                genericFormModel.EnumTemplateLists.Add(new EnumListTemplate()
                {
                    SelectListTypeCode = "ShopTypeEnum",
                    SelectListItems = EnumOps.GetEnumAsSelectList<ShopTypeEnum>()
                });

                genericFormModel.FormModel.GeneralDetail = parentWithChildObject.FirstOrDefault();

                genericFormModel.FormModel.IsLocked = parentWithChildObject.FirstOrDefault().Application.IsLocked;
                genericFormModel.FormModel.IsFeeApplicable = parentWithChildObject.FirstOrDefault().Application.IsFeeApplicable;
                genericFormModel.ApplicationLifeCycleStatusType = parentWithChildObject.FirstOrDefault().Application.ApplicationLifeCycleStatusType;

                genericFormModel.AppFormStepsList = new List<AppFormStepsInfo>();
                genericFormModel.AppFormStepsList = await _iApplicationMamnagementService.GetAppFormStepperInfo(genericFormModel.FormModel.GeneralDetail.AppRefId, ApplicationTypeEnum.SHOP_LICENCE, (genericFormModel.FormModel != null ? genericFormModel.FormModel.GeneralDetail.ShopLicenceId : id), "LOCK");

                var siteId = await _iProjectSiteService.GetProjectSiteRefIdByAppId(id);
                var projectProfileInfo = await _iProjectSiteService.GetProjectSiteDetail(siteId.ResponseDataModel);
                genericFormModel.FormModel.GeneralDetail.LabourCircleRefId = projectProfileInfo.FormModel.LabourCircleRefId;
                genericFormModel.FormModel.GeneralDetail.DistrictRefId = projectProfileInfo.FormModel.DistrictRefId;
            }
            catch (Exception ex)
            {
                genericFormModel.HasError = true;
                genericFormModel.ErrorDesc = ex.Message;
                throw ex;
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

                    if (await _iApplicationMamnagementService.LockApplication(appRefId, AppActionType, remarks, ApplicationTypeEnum.SHOP_LICENCE) !="")
                    {
                        genericServiceResultTemplate.ApplicationInitiateResponse.IsApplicationCreated = true;
                        // Share status to Invest Punjab Portal
                       // await _iThirdPartyInegrationsService.ShareStatusToBusinessFirst(appRefId, ApplicationTypeEnum.SHOP_LICENCE, (AppActionTypeEnum)AppActionType);
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

        #region Find Duplicates
        public async Task<GenericFormModel<IntReturn>> FindDuplicateGST(string gstNumber, Int64 shopLicenceId)
        {
            GenericFormModel<IntReturn> genericFormModel = new GenericFormModel<IntReturn>();
            try
            {
                List<StoreProcedureParm> storeProcedureParms = new List<StoreProcedureParm>()
                {
                    new StoreProcedureParm (){ ParmName="GstNumber", ParmValue=gstNumber, isNumber=false},
                    new StoreProcedureParm (){ ParmName="ShopLicenceId", ParmValue=shopLicenceId.ToString(), isNumber=true}
                };
                genericFormModel.FormModel = await _iGeneric_SP_Repository.CallStoreProcedureReaderScalerAsync("sp_ShopLicence_FindDuplicateGst", storeProcedureParms);
            }
            catch (Exception ex)
            {
                genericFormModel.HasError = true;
                genericFormModel.ErrorDesc = ex.Message;
            }
            return genericFormModel;
        }
        public async Task<GenericFormModel<IntReturn>> FindDuplicatePanNo(string panOrTanNumber, Int64 shopLicenceId)
        {
            GenericFormModel<IntReturn> genericFormModel = new GenericFormModel<IntReturn>();
            try
            {
                List<StoreProcedureParm> storeProcedureParms = new List<StoreProcedureParm>()
                {
                    new StoreProcedureParm (){ ParmName="PanNumber", ParmValue=panOrTanNumber, isNumber=false},
                    new StoreProcedureParm (){ ParmName="ShopLicenceId", ParmValue=shopLicenceId.ToString(), isNumber=true}
                };
                genericFormModel.FormModel = await _iGeneric_SP_Repository.CallStoreProcedureReaderScalerAsync("sp_ShopLicence_FindDuplicatePAN", storeProcedureParms);
            }
            catch (Exception ex)
            {
                genericFormModel.HasError = true;
                genericFormModel.ErrorDesc = ex.Message;
            }
            return genericFormModel;
        }
        public async Task<GenericResponseTemplateModel<AdhaarVerifyViewModel>> FindDuplicateAadharNumber(string aadharNumber, Int64 projectSiteRefId)
        {
            GenericResponseTemplateModel<AdhaarVerifyViewModel> genericFormModel = new GenericResponseTemplateModel<AdhaarVerifyViewModel>();
            try
            {
                List<StoreProcedureParm> storeProcedureParms = new List<StoreProcedureParm>()
                {
                    new StoreProcedureParm (){ ParmName="AadharNumber", ParmValue=aadharNumber, isNumber=false},
                    new StoreProcedureParm (){ ParmName="ProjectSiteRefId", ParmValue=projectSiteRefId.ToString(), isNumber=true}
                };
                var username = await _iGeneric_SP_Repository.CallStoreProcedureReaderAsync<AdhaarVerifyViewModel>("sp_ShopLicence_FindDuplicateAadhar", storeProcedureParms);
                genericFormModel.ResponseDataModel = username.FirstOrDefault();
            }
            catch (Exception ex)
            {
                genericFormModel.HasError = true;
                genericFormModel.ErrorDesc = ex.Message;
            }
            return genericFormModel;
        }

        public async Task<GenericFormModel<List<ShopEmployeeDetailViewModel>>> GetShopLicenceEmlpyeesList(Int64 id, string searchCode, int pageNo, int pageSize, string sortColumn, string sortOrder, string filterArray)
        {
            GenericFormModel<List<ShopEmployeeDetailViewModel>> genericFormModel = new GenericFormModel<List<ShopEmployeeDetailViewModel>>();
            try
            {
                //var parentWithChildObject = await _iGR_ShopLicence_GeneralDetail
                //    .GetAsync(x => x.AppRefId == id,  //Conditions         
                //        null,                //Orders
                //        x => x.ShopLicence_EmployeeDetails)      //Includes
                //    .ConfigureAwait(false);
                List<StoreProcedureParm> storeProcedureParms = new List<StoreProcedureParm>()
                {
                    new StoreProcedureParm (){ ParmName="AppRefId", ParmValue=id.ToString(), isNumber=true},
                    new StoreProcedureParm (){isNumber=false, ParmName="SearchCode",ParmValue= searchCode==null ? "": searchCode},
                    new StoreProcedureParm (){isNumber=true, ParmName="PageNo", ParmValue=pageNo.ToString()},
                    new StoreProcedureParm (){isNumber=true, ParmName="PageSize", ParmValue=pageSize.ToString()},
                    new StoreProcedureParm (){isNumber=false, ParmName="SortColumn", ParmValue=sortColumn.ToString()},
                    new StoreProcedureParm (){isNumber=false, ParmName="SortOrder", ParmValue = sortOrder=="1" ? "ASC" : "DESC" }
                };
                var emps = await _iGeneric_SP_Repository.CallStoreProcedureReaderAsync<ShopEmployeeDetailViewModel>("sp_ShopLicence_GetAllEmployees", storeProcedureParms);

                genericFormModel.FormModel = new List<ShopEmployeeDetailViewModel>();
                genericFormModel.FormModel = emps;
            }
            catch (Exception ex)
            {
                genericFormModel.HasError = true;
                genericFormModel.ErrorDesc = ex.Message;
            }
            return genericFormModel;
        }
        #endregion

        public async Task<GenericResponseTemplateModel<bool>> RemoveEmployee(Int64 id)
        {
            GenericResponseTemplateModel<bool> genericFormModel = new GenericResponseTemplateModel<bool>();
            try
            {
                var emp = _context.ShopLicence_EmployeeDetails.Where(x => x.EmployeeDetailId == id).FirstOrDefault();
                _context.ShopLicence_EmployeeDetails.RemoveRange(emp);
                _context.SaveChanges();
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
