using EFCore.BulkExtensions;
using Microsoft.EntityFrameworkCore;
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

namespace pbsamadhannetcoreapi.Services
{
    public class Licence_ContractLabourService : ILicence_ContractLabourService
    {
        private readonly IGenericRepository<Licence_ContractLabour_GeneralDetail> _iGR_Licence_ContractLabour_GeneralDetail;
        //private readonly IGenericRepository<Licence_CL_PE_Contrator> _iGR_Licence_CL_PE_ContractorDetail;
        private readonly IApplicationManagementService<Licence_ContractLabour_GeneralDetail> _iApplicationMamnagementService;
        //private readonly IGenericRepository<BuildingPlanHUDPaymentDetail> _iGR_AppFeeDetail;
        //private readonly IPaymentManagerRepository _iPaymentManagerRepository;
        //private readonly IPaymentManagerService _iPaymentManagerService;
        private readonly IThirdPartyInegrationsService _iThirdPartyInegrationsService;
        private readonly AppDbContext _context;
        private readonly IGeneric_SP_Repository _iGeneric_SP_Repository;
        private readonly IProjectSiteService _iProjectSiteService;
        //private readonly IGenericRepository<DistrictLgd> _iGR_DistrictLgd;
        private readonly IGenericRepository<Licence_ContractLabour_AmendmentDataHistory> _iGR_Licence_ContractLabour_AmendmentHistories;
        public Licence_ContractLabourService(
            IGenericRepository<Licence_ContractLabour_GeneralDetail> iGR_Licence_ContractLabour_GeneralDetail,
            //IGenericRepository<Licence_CL_PE_Contrator> iGR_Licence_CL_PE_ContractorDetail,
            IApplicationManagementService<Licence_ContractLabour_GeneralDetail> IApplicationMamnagementService,
            IThirdPartyInegrationsService iThirdPartyInegrationsService,
            AppDbContext context,
            IGeneric_SP_Repository iGeneric_SP_Repository,
            //IGenericRepository<DistrictLgd> iGR_DistrictLgd, IZoneRepository iZoneRepository,
            IProjectSiteService iProjectSiteService,
            IGenericRepository<Licence_ContractLabour_AmendmentDataHistory> iGR_Licence_ContractLabour_AmendmentHistories)
        {
            _iGR_Licence_ContractLabour_GeneralDetail = iGR_Licence_ContractLabour_GeneralDetail;
            //_iGR_Licence_CL_PE_ContractorDetail = iGR_Licence_CL_PE_ContractorDetail;
            _iApplicationMamnagementService = IApplicationMamnagementService;
            _iThirdPartyInegrationsService = iThirdPartyInegrationsService;
            _context = context;
            _iGeneric_SP_Repository = iGeneric_SP_Repository;
            _iProjectSiteService = iProjectSiteService;
            //_iGR_DistrictLgd = iGR_DistrictLgd;
            _iGR_Licence_ContractLabour_AmendmentHistories = iGR_Licence_ContractLabour_AmendmentHistories;
        }

        public async Task<GenericFormModel<Licence_ContractLabour_GeneralDetail>> GetContractLabourGeneralDetail(long id, long projectSiteId)
        {
            GenericFormModel<Licence_ContractLabour_GeneralDetail> genericFormModel = new GenericFormModel<Licence_ContractLabour_GeneralDetail>();
            try
            {
                if (id != 0) //Existing Record
                {
                    var parentWithChildObject = await _iGR_Licence_ContractLabour_GeneralDetail
                       .GetAsync(x => x.AppRefId == id,
                         null,
                         x => x.Application, x => x.Application.ApplicationAction)      //Includes
                       .ConfigureAwait(false);
                    var contratorDetails = await _iGR_Licence_ContractLabour_GeneralDetail
                       .GetAsync(x => x.AppRefId == id,
                         null)
                       .ConfigureAwait(false);
                    genericFormModel.FormModel = parentWithChildObject.FirstOrDefault();
                    genericFormModel.IsEditAllowed = genericFormModel.FormModel.Application.IsAllowEdit;
                    genericFormModel.IsLocked = genericFormModel.FormModel.Application.IsLocked;
                    genericFormModel.ApplicationLifeCycleStatusType = genericFormModel.FormModel.Application.ApplicationLifeCycleStatusType;
                    genericFormModel.FormModel.ApplicationPurposeType = genericFormModel.FormModel.Application.ApplicationPurposeType;
                    genericFormModel.FormModel.LegacyLicenceNumber = genericFormModel.FormModel.Application.Legacy_LicenceNo;

                    var siteId = await _iProjectSiteService.GetProjectSiteRefIdByAppId(id);
                    var projectProfileInfo = await _iProjectSiteService.GetProjectSiteDetail(siteId.ResponseDataModel);
                    genericFormModel.FormModel.AlcCircleRefId = projectProfileInfo.FormModel.AlcCircleRefId;

                }

                else //New Record
                {
                    genericFormModel.FormModel = new Licence_ContractLabour_GeneralDetail();

                    //Allow user to edit form
                    genericFormModel.IsEditAllowed = true;
                    genericFormModel.IsLocked = false;
                    genericFormModel.ApplicationLifeCycleStatusType = ApplicationLifeCycleStatusTypeEnum.NOT_SUBMITTED;
                }

                //Initialization of Enum types of lists

                genericFormModel.EnumTemplateLists = new List<EnumListTemplate>();
                genericFormModel.EnumTemplateLists.Add(new EnumListTemplate()
                {
                    SelectListTypeCode = "ApplicationPurposeTypeEnum",
                    SelectListItems = EnumOps.GetEnumAsSelectList<ApplicationPurposeTypeEnum>()
                });

                genericFormModel.AppFormStepsList = new List<AppFormStepsInfo>();
                genericFormModel.AppFormStepsList = await _iApplicationMamnagementService.GetAppFormStepperInfo(id, ApplicationTypeEnum.CONTRACT_LABOUR, (genericFormModel.FormModel != null ? genericFormModel.FormModel.ContractLabourId : id), "GD");
            }
            catch (Exception ex)
            {
                genericFormModel.HasError = true;
                genericFormModel.ErrorDesc = ex.Message;
            }
            return genericFormModel;

        }


        public async Task<GenericResponseTemplateModel<Licence_CL_PE_GeneralDetail_ViewModel>> GetPrincipalEmployerDetails(string licencenumber)
        {
            GenericResponseTemplateModel<Licence_CL_PE_GeneralDetail_ViewModel> genericFormModel = new GenericResponseTemplateModel<Licence_CL_PE_GeneralDetail_ViewModel>();
            try
            {
                List<StoreProcedureParm> storeProcedureParms = new List<StoreProcedureParm>()
                {
                     new StoreProcedureParm (){ ParmName="LicenceNumber", ParmValue=licencenumber.ToString(), isNumber=false}
                };
                var pedetails = await _iGeneric_SP_Repository.CallStoreProcedureReaderAsync<Licence_CL_PE_GeneralDetail_ViewModel>("sp_GetPrincipalEmployerDetailByLicence", storeProcedureParms);
                genericFormModel.ResponseDataModel = pedetails.FirstOrDefault();
            }
            catch (Exception ex)
            {
                genericFormModel.HasError = true;
                genericFormModel.ErrorDesc = ex.Message;
            }
            return genericFormModel;
        }

        public async Task<GenericFormModel<List<Licence_CL_PE_Contrator>>>Get_PE_Contractor_List(Int64 generalDetailRefId)
        {
            GenericFormModel<List<Licence_CL_PE_Contrator>> genericFormModel = new GenericFormModel<List<Licence_CL_PE_Contrator>>();
            try
            {

                var contractorlist = _context.Licence_CL_PE_Contrator.Where(x => x.Licence_CL_PE_GeneralDetailRefId == generalDetailRefId).ToList();
                genericFormModel.FormModel = new List<Licence_CL_PE_Contrator>();
                genericFormModel.FormModel = contractorlist;
            }
            catch (Exception ex)
            {
                genericFormModel.HasError = true;
                genericFormModel.ErrorDesc = ex.Message;
            }
            return genericFormModel;
        }

        public async Task<GenericResponseTemplateModel<Licence_CL_PE_Contrator>> Get_PE_Contractor_List_byId(Int64 id)
        {
            GenericResponseTemplateModel<Licence_CL_PE_Contrator> genericFormModel = new GenericResponseTemplateModel<Licence_CL_PE_Contrator>();
            try
            {


                var pecontractordetails = _context.Licence_CL_PE_Contrator.Where(x => x.Id == id).FirstOrDefault();
                //List<StoreProcedureParm> storeProcedureParms = new List<StoreProcedureParm>()
                //{
                //     new StoreProcedureParm (){ ParmName="Id", ParmValue=id.ToString(), isNumber=true}
                //};
                //var pecontractordetails = await _iGeneric_SP_Repository.CallStoreProcedureReaderAsync<Licence_CL_PE_Contrator>("sp_GetContractorListbyID", storeProcedureParms);
                genericFormModel.ResponseDataModel = pecontractordetails;
            }
            catch (Exception ex)
            {
                genericFormModel.HasError = true;
                genericFormModel.ErrorDesc = ex.Message;
            }
            return genericFormModel;
        }
        public async Task<GenericServiceResultTemplate> AddUpdate_GeneralDetail(Licence_ContractLabour_GeneralDetail formModel, string id)
        {
            GenericServiceResultTemplate genericServiceResultTemplate = new GenericServiceResultTemplate();
            try
            {
                genericServiceResultTemplate.HasException = false;
                genericServiceResultTemplate.CustomeValidationResult = CustomeValidator<Licence_ContractLabour_GeneralDetail>.ValidateModel_AllProperties(formModel);
                genericServiceResultTemplate.ApplicationInitiateResponse = new ApplicationInitiateResponseViewModel();
                genericServiceResultTemplate.ApplicationInitiateResponse.EntityKeyId = formModel.ContractLabourId;
                genericServiceResultTemplate.ApplicationInitiateResponse.AppId = formModel.AppRefId;
                if (genericServiceResultTemplate.CustomeValidationResult.IsValid)
                {
                    Licence_ContractLabour_GeneralDetail previousPELicenseData = null;
                    List<Licence_ContractLabour_AmendmentDataHistory> savedAmendmentDataHistories = null;
                    {
                        if (formModel.ContractLabourId != 0) //Existing record
                        {
                            previousPELicenseData = _context.Licence_ContractLabour_GeneralDetails.Where(x => x.ContractLabourId == formModel.ContractLabourId).AsNoTracking().FirstOrDefault();
                            savedAmendmentDataHistories = await _context.Licence_ContractLabour_AmendmentDataHistories.Where(x =>
                                !x.IsLocked && x.AppRefId == previousPELicenseData.AppRefId &&
                                x.ModifiedCounter == previousPELicenseData.ModifiedCounter &&
                                (x.FieldName == "TotalWorker")).ToListAsync();
                            _iGR_Licence_ContractLabour_GeneralDetail.Update(formModel);
                            await _iGR_Licence_ContractLabour_GeneralDetail.SavechangeAsync();

                            //Update last modified date
                            await _iApplicationMamnagementService.UpdateAppLastModifiedDate(formModel.AppRefId);
                            genericServiceResultTemplate.ApplicationInitiateResponse = new ApplicationInitiateResponseViewModel();
                            genericServiceResultTemplate.ApplicationInitiateResponse.EntityKeyId = formModel.ContractLabourId;
                            genericServiceResultTemplate.ApplicationInitiateResponse.AppId = formModel.AppRefId;

                            //Update Circle
                            var projectSiteDetail = await _iApplicationMamnagementService.GetProjectSiteByAppRefId(genericServiceResultTemplate.ApplicationInitiateResponse.AppId);
                            await _iProjectSiteService.SetProjectSiteCircle(projectSiteDetail.FormModel.ProjectSiteId, 0, 0, formModel.AlcCircleRefId);

                            var isStatusShared = await _iThirdPartyInegrationsService.HasShareStatusToBusinessFirst(genericServiceResultTemplate.ApplicationInitiateResponse.AppId);
                            if (isStatusShared != null && !isStatusShared.HasError && !isStatusShared.ResponseDataModel)
                            {
                                //Update Native Appid by IPIN
                                await _iThirdPartyInegrationsService.UpdateBusinessFirstRequestNativeAppIdByIPin(formModel.IPin, formModel.InvestPunjab_AppId, genericServiceResultTemplate.ApplicationInitiateResponse.AppId, ApplicationTypeEnum.CONTRACT_LABOUR, formModel.ApplicationPurposeType);

                            }
                        }
                        else //New record
                        {

                            formModel = GenericModelOps<Licence_ContractLabour_GeneralDetail>.SetNullAllNevigationProperties(formModel);
                            genericServiceResultTemplate.ApplicationInitiateResponse = await _iApplicationMamnagementService.InitiateApplication(ApplicationTypeEnum.CONTRACT_LABOUR, formModel, formModel.ProjectSiteRefId, formModel.ApplicationPurposeType, id, formModel.IPin, formModel.InvestPunjab_AppId, false, 0, 0, null, null, formModel.ProjectSiteVersion);

                            if (genericServiceResultTemplate.ApplicationInitiateResponse.IsApplicationCreated == true)
                            {

                                //update LabourCircle Id in ProjectSite Table
                                var projectSiteDetail = await _iApplicationMamnagementService.GetProjectSiteByAppRefId(genericServiceResultTemplate.ApplicationInitiateResponse.AppId);
                                await _iProjectSiteService.SetProjectSiteCircle(projectSiteDetail.FormModel.ProjectSiteId, 0, 0, formModel.AlcCircleRefId);


                                //Update Native Appid by IPIN
                                var temp = await _iThirdPartyInegrationsService.UpdateBusinessFirstRequestNativeAppIdByIPin(formModel.IPin, formModel.InvestPunjab_AppId, genericServiceResultTemplate.ApplicationInitiateResponse.AppId, ApplicationTypeEnum.CONTRACT_LABOUR, formModel.ApplicationPurposeType);

                                //Share status to Invest Punjab Portal
                                await _iThirdPartyInegrationsService.ShareStatusToBusinessFirst(genericServiceResultTemplate.ApplicationInitiateResponse.AppId, ApplicationTypeEnum.CONTRACT_LABOUR, AppActionTypeEnum.APP_SAVE_DRAFT);
                            }
                            else if (genericServiceResultTemplate.ApplicationInitiateResponse.IsApplicationCreated == false)
                            {
                                genericServiceResultTemplate.HasException = true;
                                genericServiceResultTemplate.Exceptions = new Exception();
                            }


                        }
                        var application = _context.Applications.Where(x => x.AppId == formModel.AppRefId && x.IsDeleted == false).FirstOrDefault();

                        if (application.ApplicationPurposeType == ApplicationPurposeTypeEnum.AMENDMENT_LICENCE && previousPELicenseData != null)
                        {
                            List<Licence_ContractLabour_AmendmentDataHistory> amendmentDataHistories = new List<Licence_ContractLabour_AmendmentDataHistory>();

                            if (savedAmendmentDataHistories.Any(x => x.FieldName == "TotalWorker"))
                            {
                                if (savedAmendmentDataHistories.Where(x => x.FieldName == "TotalWorker").Select(x => x.PreviousValue).FirstOrDefault() == formModel.MaximumNumberOfEmployee.ToString())
                                {
                                    _context.Licence_ContractLabour_AmendmentDataHistories.Remove(savedAmendmentDataHistories.Where(x => x.FieldName == "TotalWorker").FirstOrDefault());
                                }
                                else
                                {
                                    var amendmentHistory = savedAmendmentDataHistories.Where(x => x.FieldName == "TotalWorker").FirstOrDefault();
                                    amendmentHistory.ModifiedValue = formModel.MaximumNumberOfEmployee.ToString();
                                    _context.Update<Licence_ContractLabour_AmendmentDataHistory>(amendmentHistory);
                                }
                                await _context.SaveChangesAsync();
                            }
                            else
                            {
                                if (previousPELicenseData.MaximumNumberOfEmployee != formModel.MaximumNumberOfEmployee)
                                {
                                    amendmentDataHistories.Add(new Licence_ContractLabour_AmendmentDataHistory()
                                    {
                                        AppRefId = formModel.AppRefId,
                                        FieldName = "TotalWorker",
                                        PreviousValue = previousPELicenseData.MaximumNumberOfEmployee.ToString(),
                                        ModifiedValue = formModel.MaximumNumberOfEmployee.ToString(),
                                        SectionCode = "TW", //ConstructionBuildingDetail
                                        ModifiedOn = DateTime.Now,
                                        ModifiedCounter = formModel.ModifiedCounter
                                    });
                                }
                            }



                            if (savedAmendmentDataHistories.Any(x => x.FieldName == "Manager_Name"))
                            {
                                if (savedAmendmentDataHistories.Where(x => x.FieldName == "Manager_Name").Select(x => x.PreviousValue).FirstOrDefault() == formModel.AgentOrManagerName)
                                {
                                    _context.Licence_ContractLabour_AmendmentDataHistories.Remove(savedAmendmentDataHistories.Where(x => x.FieldName == "Manager_Name").FirstOrDefault());
                                }
                                else
                                {
                                    var amendmentHistory = savedAmendmentDataHistories.Where(x => x.FieldName == "Manager_Name").FirstOrDefault();
                                    amendmentHistory.ModifiedValue = formModel.AgentOrManagerName.ToString();
                                    _context.Update<Licence_ContractLabour_AmendmentDataHistory>(amendmentHistory);
                                }
                                await _context.SaveChangesAsync();
                            }
                            else
                            {
                                if (previousPELicenseData.AgentOrManagerName != formModel.AgentOrManagerName)
                                {
                                    amendmentDataHistories.Add(new Licence_ContractLabour_AmendmentDataHistory()
                                    {
                                        AppRefId = formModel.AppRefId,
                                        FieldName = "Manager_Name",
                                        PreviousValue = previousPELicenseData.AgentOrManagerName.ToString(),
                                        ModifiedValue = formModel.AgentOrManagerName.ToString(),
                                        SectionCode = "MD", //ManagerDetail
                                        ModifiedOn = DateTime.Now,
                                        ModifiedCounter = formModel.ModifiedCounter
                                    });
                                }
                            }


                            if (savedAmendmentDataHistories.Any(x => x.FieldName == "Manager_Address"))
                            {
                                if (savedAmendmentDataHistories.Where(x => x.FieldName == "Manager_Address").Select(x => x.PreviousValue).FirstOrDefault() == formModel.AgentOrManagerAddress)
                                {
                                    _context.Licence_ContractLabour_AmendmentDataHistories.Remove(savedAmendmentDataHistories.Where(x => x.FieldName == "Manager_Address").FirstOrDefault());
                                }
                                else
                                {
                                    var amendmentHistory = savedAmendmentDataHistories.Where(x => x.FieldName == "Manager_Address").FirstOrDefault();
                                    amendmentHistory.ModifiedValue = formModel.AgentOrManagerAddress.ToString();
                                    _context.Update<Licence_ContractLabour_AmendmentDataHistory>(amendmentHistory);
                                }
                                await _context.SaveChangesAsync();
                            }
                            else
                            {
                                if (previousPELicenseData.AgentOrManagerAddress != formModel.AgentOrManagerAddress)
                                {
                                    amendmentDataHistories.Add(new Licence_ContractLabour_AmendmentDataHistory()
                                    {
                                        AppRefId = formModel.AppRefId,
                                        FieldName = "Manager_Address",
                                        PreviousValue = previousPELicenseData.AgentOrManagerAddress.ToString(),
                                        ModifiedValue = formModel.AgentOrManagerAddress.ToString(),
                                        SectionCode = "MD", //ManagerDetail
                                        ModifiedOn = DateTime.Now,
                                        ModifiedCounter = formModel.ModifiedCounter
                                    });
                                }
                            }

                            if (amendmentDataHistories.Count() > 0)
                            {
                                await _context.BulkInsertAsync<Licence_ContractLabour_AmendmentDataHistory>(amendmentDataHistories);
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

        public async Task<GenericFormModel<Licence_Contract_LabourViewModel>> Get_Licence_ContractLabour_Detail(long id)
        {
            GenericFormModel<Licence_Contract_LabourViewModel> genericFormModel = new GenericFormModel<Licence_Contract_LabourViewModel>();
            try
            {
                genericFormModel.ListTemplateLists = new List<ListTemplate>();
                genericFormModel.FormModel = new Licence_Contract_LabourViewModel();
                genericFormModel.FormModel.GeneralDetail = new Licence_ContractLabour_GeneralDetail();
                genericFormModel.FormModel.Licence_ContractLabour_AmendmentDataHistories = new List<Licence_ContractLabour_AmendmentDataHistory>();

                var parentWithChildObject = await _iGR_Licence_ContractLabour_GeneralDetail
                       .GetAsync(x => x.AppRefId == id,
                         null,
                         x => x.Application, x => x.Application.ApplicationAction)
                       .ConfigureAwait(false);

                var oldparentWithChildObject = await _iGR_Licence_ContractLabour_AmendmentHistories
                      .GetAsync(x => x.AppRefId == id,
                        null)
                      .ConfigureAwait(false);

                genericFormModel.FormModel.GeneralDetail = parentWithChildObject.FirstOrDefault();
                genericFormModel.FormModel.Licence_ContractLabour_AmendmentDataHistories = oldparentWithChildObject.ToList();
                genericFormModel.FormModel.IsLocked = parentWithChildObject.FirstOrDefault().Application.IsLocked;
                genericFormModel.FormModel.IsFeeApplicable = parentWithChildObject.FirstOrDefault().Application.IsFeeApplicable;
                genericFormModel.ApplicationLifeCycleStatusType = parentWithChildObject.FirstOrDefault().Application.ApplicationLifeCycleStatusType;
                genericFormModel.FormModel.AppActionType = parentWithChildObject.FirstOrDefault().Application.ApplicationAction.AppActionType;

                genericFormModel.AppFormStepsList = new List<AppFormStepsInfo>();
                genericFormModel.AppFormStepsList = await _iApplicationMamnagementService.GetAppFormStepperInfo(genericFormModel.FormModel.GeneralDetail.AppRefId, ApplicationTypeEnum.CONTRACT_LABOUR, (genericFormModel.FormModel != null ? genericFormModel.FormModel.GeneralDetail.ContractLabourId : id), "LOCK");

                var siteId = await _iProjectSiteService.GetProjectSiteRefIdByAppId(id);
                var projectProfileInfo = await _iProjectSiteService.GetProjectSiteDetail(siteId.ResponseDataModel);
                genericFormModel.FormModel.GeneralDetail.AlcCircleRefId = projectProfileInfo.FormModel.AlcCircleRefId;
                genericFormModel.FormModel.GeneralDetail.DistrictRefId = projectProfileInfo.FormModel.DistrictRefId;

            }

            catch (Exception ex)
            {
                genericFormModel.HasError = true;
                genericFormModel.ErrorDesc = ex.Message;
            }
            return genericFormModel;
        }



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

        public async Task<GenericResponseTemplateModel<List<ContractLabourLicenceValidityViewModel>>> GetLicenceValidity(string licencenumber)
        {
            GenericResponseTemplateModel<List<ContractLabourLicenceValidityViewModel>> genericFormModel = new GenericResponseTemplateModel<List<ContractLabourLicenceValidityViewModel>>();
            try
            {
                List<StoreProcedureParm> storeProcedureParms = new List<StoreProcedureParm>()
        {
             new StoreProcedureParm (){ ParmName="LicenceNumber", ParmValue=licencenumber.ToString(), isNumber=false}
        };
                var pedetails = await _iGeneric_SP_Repository.CallStoreProcedureReaderAsync<ContractLabourLicenceValidityViewModel>("sp_GetCLLicenceValidity", storeProcedureParms);
                genericFormModel.ResponseDataModel = pedetails;
            }
            catch (Exception ex)
            {
                genericFormModel.HasError = true;
                genericFormModel.ErrorDesc = ex.Message;
            }
            return genericFormModel;
        }
    }
}