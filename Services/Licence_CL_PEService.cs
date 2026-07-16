using DocumentFormat.OpenXml.Office2010.Excel;
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
    public class Licence_CL_PEService : ILicence_CL_PEService
    {
        private readonly IGenericRepository<Licence_CL_PE_GeneralDetail> _iGR_Licence_CL_PE_GeneralDetail;
        private readonly IGenericRepository<Licence_CL_PE_Contrator> _iGR_Licence_CL_PE_ContractorDetail;
        private readonly IApplicationManagementService<Licence_CL_PE_GeneralDetail> _iApplicationMamnagementService;
        private readonly IGenericRepository<BuildingPlanHUDPaymentDetail> _iGR_AppFeeDetail;
        private readonly IPaymentManagerRepository _iPaymentManagerRepository;
        private readonly IPaymentManagerService _iPaymentManagerService;
        private readonly IThirdPartyInegrationsService _iThirdPartyInegrationsService;
        private readonly AppDbContext _context;
        private readonly IGeneric_SP_Repository _iGeneric_SP_Repository;
        private readonly IProjectSiteService _iProjectSiteService;
        private readonly IGenericRepository<DistrictLgd> _iGR_DistrictLgd;
        private readonly IGenericRepository<Licence_PE_AmendmentDataHistories> _iGR_Licence_CL_PE_AmendmentHistories;
        public Licence_CL_PEService(
            IGenericRepository<Licence_CL_PE_GeneralDetail> iGR_Licence_CL_PE_GeneralDetail,
            IGenericRepository<Licence_CL_PE_Contrator> iGR_Licence_CL_PE_ContractorDetail,
            IApplicationManagementService<Licence_CL_PE_GeneralDetail> IApplicationMamnagementService,
            IThirdPartyInegrationsService iThirdPartyInegrationsService,
            AppDbContext context,
            IGeneric_SP_Repository iGeneric_SP_Repository,
            IGenericRepository<DistrictLgd> iGR_DistrictLgd, IZoneRepository iZoneRepository,
            IProjectSiteService iProjectSiteService,
            IGenericRepository<Licence_PE_AmendmentDataHistories> iGR_Licence_CL_PE_AmendmentHistories)
        {
            _iGR_Licence_CL_PE_GeneralDetail = iGR_Licence_CL_PE_GeneralDetail;
            _iGR_Licence_CL_PE_ContractorDetail = iGR_Licence_CL_PE_ContractorDetail;
            _iApplicationMamnagementService = IApplicationMamnagementService;
            _iThirdPartyInegrationsService = iThirdPartyInegrationsService;
            _context = context;
            _iGeneric_SP_Repository = iGeneric_SP_Repository;
            _iProjectSiteService = iProjectSiteService;
            _iGR_DistrictLgd = iGR_DistrictLgd;
            _iGR_Licence_CL_PE_AmendmentHistories = iGR_Licence_CL_PE_AmendmentHistories;
        }

        public async Task<GenericFormModel<Licence_CL_PE_GeneralDetail>> GetPEGeneralDetail(long id, long projectSiteId)
        {
            GenericFormModel<Licence_CL_PE_GeneralDetail> genericFormModel = new GenericFormModel<Licence_CL_PE_GeneralDetail>();
            try
            {
                if (id != 0) //Existing Record
                {
                    var parentWithChildObject = await _iGR_Licence_CL_PE_GeneralDetail
                       .GetAsync(x => x.AppRefId == id,  //Conditions         
                         null,                //Orders
                         x => x.Application, x => x.Application.ApplicationAction, x => x.Licence_CL_PE_Contrators)      //Includes
                       .ConfigureAwait(false);
                    var contratorDetails = await _iGR_Licence_CL_PE_GeneralDetail
                       .GetAsync(x => x.AppRefId == id,  //Conditions         
                         null,                //Orders
                         x => x.Licence_CL_PE_Contrators)      //Includes
                       .ConfigureAwait(false);
                    genericFormModel.FormModel = parentWithChildObject.FirstOrDefault();
                    genericFormModel.IsEditAllowed = genericFormModel.FormModel.Application.IsAllowEdit;
                    genericFormModel.IsLocked = genericFormModel.FormModel.Application.IsLocked;
                    genericFormModel.ApplicationLifeCycleStatusType = genericFormModel.FormModel.Application.ApplicationLifeCycleStatusType;
                    genericFormModel.FormModel.ApplicationPurposeType = genericFormModel.FormModel.Application.ApplicationPurposeType;

                    var siteId = await _iProjectSiteService.GetProjectSiteRefIdByAppId(id);
                    var projectProfileInfo = await _iProjectSiteService.GetProjectSiteDetail(siteId.ResponseDataModel);
                    genericFormModel.FormModel.AlcCircleRefId = projectProfileInfo.FormModel.AlcCircleRefId;

                }

                else //New Record
                {
                    genericFormModel.FormModel = new Licence_CL_PE_GeneralDetail();

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
                genericFormModel.AppFormStepsList = await _iApplicationMamnagementService.GetAppFormStepperInfo(id, ApplicationTypeEnum.PRINCIPAL_EMPLOYER, (genericFormModel.FormModel != null ? genericFormModel.FormModel.Id : id), "GD");
            }
            catch (Exception ex)
            {
                genericFormModel.HasError = true;
                genericFormModel.ErrorDesc = ex.Message;
            }
            return genericFormModel;

        }
        public async Task<GenericServiceResultTemplate> AddUpdate_GeneralDetail(Licence_CL_PE_GeneralDetail formModel, string id)
        {
            GenericServiceResultTemplate genericServiceResultTemplate = new GenericServiceResultTemplate();
            try
            {
                genericServiceResultTemplate.HasException = false;
                genericServiceResultTemplate.CustomeValidationResult = CustomeValidator<Licence_CL_PE_GeneralDetail>.ValidateModel_AllProperties(formModel);
                genericServiceResultTemplate.ApplicationInitiateResponse = new ApplicationInitiateResponseViewModel();
                genericServiceResultTemplate.ApplicationInitiateResponse.EntityKeyId = formModel.Id;
                genericServiceResultTemplate.ApplicationInitiateResponse.AppId = formModel.AppRefId;
                if (genericServiceResultTemplate.CustomeValidationResult.IsValid)
                { 
                   Licence_CL_PE_GeneralDetail previousPELicenseData = null;
                   List<Licence_PE_AmendmentDataHistories> savedAmendmentDataHistories = null;
                    {
                        if (formModel.Id != 0) //Existing record
                        {
                            previousPELicenseData = _context.Licence_CL_PE_GeneralDetail.Where(x => x.Id == formModel.Id).AsNoTracking().FirstOrDefault();
                            savedAmendmentDataHistories = await _context.Licence_PE_AmendmentDataHistories.Where(x =>
                                !x.IsLocked && x.AppRefId == previousPELicenseData.AppRefId &&
                                x.ModifiedCounter == previousPELicenseData.ModifiedCounter &&
                                (x.FieldName == "TotalWorker")).ToListAsync();
                            _iGR_Licence_CL_PE_GeneralDetail.Update(formModel);
                            await _iGR_Licence_CL_PE_GeneralDetail.SavechangeAsync();
                            //Update last modified date
                            await _iApplicationMamnagementService.UpdateAppLastModifiedDate(formModel.AppRefId);
                            genericServiceResultTemplate.ApplicationInitiateResponse = new ApplicationInitiateResponseViewModel();
                            genericServiceResultTemplate.ApplicationInitiateResponse.EntityKeyId = formModel.Id;
                            genericServiceResultTemplate.ApplicationInitiateResponse.AppId = formModel.AppRefId;

                            // Update Circle
                            var projectSiteDetail = await _iApplicationMamnagementService.GetProjectSiteByAppRefId(genericServiceResultTemplate.ApplicationInitiateResponse.AppId);
                            await _iProjectSiteService.SetProjectSiteCircle(projectSiteDetail.FormModel.ProjectSiteId, 0, formModel.AlcCircleRefId,0);

                            var isStatusShared = await _iThirdPartyInegrationsService.HasShareStatusToBusinessFirst(genericServiceResultTemplate.ApplicationInitiateResponse.AppId);
                            if (isStatusShared != null && !isStatusShared.HasError && !isStatusShared.ResponseDataModel)
                            {
                                // Update Native Appid by IPIN
                                await _iThirdPartyInegrationsService.UpdateBusinessFirstRequestNativeAppIdByIPin(formModel.IPin, formModel.InvestPunjab_AppId, genericServiceResultTemplate.ApplicationInitiateResponse.AppId, ApplicationTypeEnum.PRINCIPAL_EMPLOYER, formModel.ApplicationPurposeType);

                            }
                        }
                        else //New record
                        {

                            formModel = GenericModelOps<Licence_CL_PE_GeneralDetail>.SetNullAllNevigationProperties(formModel);
                            genericServiceResultTemplate.ApplicationInitiateResponse = await _iApplicationMamnagementService.InitiateApplication(ApplicationTypeEnum.PRINCIPAL_EMPLOYER, formModel, formModel.ProjectSiteRefId, formModel.ApplicationPurposeType, id, formModel.IPin, formModel.InvestPunjab_AppId, false, 0, 0, null, null, formModel.ProjectSiteVersion);

                            if (genericServiceResultTemplate.ApplicationInitiateResponse.IsApplicationCreated == true)
                            {

                                // update LabourCircle Id in ProjectSite Table
                                var projectSiteDetail = await _iApplicationMamnagementService.GetProjectSiteByAppRefId(genericServiceResultTemplate.ApplicationInitiateResponse.AppId);
                                await _iProjectSiteService.SetProjectSiteCircle(projectSiteDetail.FormModel.ProjectSiteId, 0, formModel.AlcCircleRefId,0);


                                // Update Native Appid by IPIN
                                var temp = await _iThirdPartyInegrationsService.UpdateBusinessFirstRequestNativeAppIdByIPin(formModel.IPin, formModel.InvestPunjab_AppId, genericServiceResultTemplate.ApplicationInitiateResponse.AppId, ApplicationTypeEnum.PRINCIPAL_EMPLOYER, formModel.ApplicationPurposeType);

                                // Share status to Invest Punjab Portal
                                await _iThirdPartyInegrationsService.ShareStatusToBusinessFirst(genericServiceResultTemplate.ApplicationInitiateResponse.AppId, ApplicationTypeEnum.PRINCIPAL_EMPLOYER, AppActionTypeEnum.APP_SAVE_DRAFT);
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
                            List<Licence_PE_AmendmentDataHistories> amendmentDataHistories = new List<Licence_PE_AmendmentDataHistories>();

                            if (savedAmendmentDataHistories.Any(x => x.FieldName == "PrincipalEmployerName"))
                            {
                                if (savedAmendmentDataHistories.Where(x => x.FieldName == "PrincipalEmployerName").Select(x => x.PreviousValue).FirstOrDefault() == formModel.PE_Name)
                                {
                                    _context.Licence_PE_AmendmentDataHistories.Remove(savedAmendmentDataHistories.Where(x => x.FieldName == "PrincipalEmployerName").FirstOrDefault());
                                }
                                else
                                {
                                    var amendmentHistory = savedAmendmentDataHistories.Where(x => x.FieldName == "PrincipalEmployerName").FirstOrDefault();
                                    amendmentHistory.ModifiedValue = formModel.PE_Name.ToString();
                                    _context.Update<Licence_PE_AmendmentDataHistories>(amendmentHistory);
                                }
                                await _context.SaveChangesAsync();
                            }
                            else
                            {
                                if (previousPELicenseData.PE_Name != formModel.PE_Name)
                                {
                                    amendmentDataHistories.Add(new Licence_PE_AmendmentDataHistories()
                                    {
                                        AppRefId = formModel.AppRefId,
                                        FieldName = "PrincipalEmployerName",
                                        PreviousValue = previousPELicenseData.PE_Name.ToString(),
                                        ModifiedValue = formModel.PE_Name.ToString(),
                                        SectionCode = "PED", //ConstructionBuildingDetail
                                        ModifiedOn = DateTime.Now,
                                        ModifiedCounter = formModel.ModifiedCounter
                                    });
                                }
                            }

                            if (savedAmendmentDataHistories.Any(x => x.FieldName == "PE_FatherName"))
                            {
                                if (savedAmendmentDataHistories.Where(x => x.FieldName == "PE_FatherName").Select(x => x.PreviousValue).FirstOrDefault() == formModel.PE_FatherName)
                                {
                                    _context.Licence_PE_AmendmentDataHistories.Remove(savedAmendmentDataHistories.Where(x => x.FieldName == "PE_FatherName").FirstOrDefault());
                                }
                                else
                                {
                                    var amendmentHistory = savedAmendmentDataHistories.Where(x => x.FieldName == "PE_FatherName").FirstOrDefault();
                                    amendmentHistory.ModifiedValue = formModel.PE_FatherName.ToString();
                                    _context.Update<Licence_PE_AmendmentDataHistories>(amendmentHistory);
                                }
                                await _context.SaveChangesAsync();
                            }
                            else
                            {
                                if (previousPELicenseData.PE_FatherName != formModel.PE_FatherName)
                                {
                                    amendmentDataHistories.Add(new Licence_PE_AmendmentDataHistories()
                                    {
                                        AppRefId = formModel.AppRefId,
                                        FieldName = "PE_FatherName",
                                        PreviousValue = previousPELicenseData.PE_FatherName.ToString(),
                                        ModifiedValue = formModel.PE_FatherName.ToString(),
                                        SectionCode = "PED", //ConstructionSiteDetail
                                        ModifiedOn = DateTime.Now,
                                        ModifiedCounter = formModel.ModifiedCounter
                                    });
                                }
                            }

                            if (savedAmendmentDataHistories.Any(x => x.FieldName == "PE_Mobile"))
                            {
                                if (savedAmendmentDataHistories.Where(x => x.FieldName == "PE_Mobile").Select(x => x.PreviousValue).FirstOrDefault() == formModel.PE_Mobile)
                                {
                                    _context.Licence_PE_AmendmentDataHistories.Remove(savedAmendmentDataHistories.Where(x => x.FieldName == "PE_Mobile").FirstOrDefault());
                                }
                                else
                                {
                                    var amendmentHistory = savedAmendmentDataHistories.Where(x => x.FieldName == "PE_Mobile").FirstOrDefault();
                                    amendmentHistory.ModifiedValue = formModel.PE_Mobile.ToString();
                                    _context.Update<Licence_PE_AmendmentDataHistories>(amendmentHistory);
                                }
                                await _context.SaveChangesAsync();
                            }
                            else
                            {
                                if (previousPELicenseData.PE_Mobile != formModel.PE_Mobile)
                                {
                                    amendmentDataHistories.Add(new Licence_PE_AmendmentDataHistories()
                                    {
                                        AppRefId = formModel.AppRefId,
                                        FieldName = "PE_Mobile",
                                        PreviousValue = previousPELicenseData.PE_Mobile.ToString(),
                                        ModifiedValue = formModel.PE_Mobile.ToString(),
                                        SectionCode = "PED", //ConstructionSiteDetail
                                        ModifiedOn = DateTime.Now,
                                        ModifiedCounter = formModel.ModifiedCounter
                                    });
                                }
                            }

                            if (savedAmendmentDataHistories.Any(x => x.FieldName == "PE_Email"))
                            {
                                if (savedAmendmentDataHistories.Where(x => x.FieldName == "PE_Email").Select(x => x.PreviousValue).FirstOrDefault() == formModel.PE_Email)
                                {
                                    _context.Licence_PE_AmendmentDataHistories.Remove(savedAmendmentDataHistories.Where(x => x.FieldName == "PE_Email").FirstOrDefault());
                                }
                                else
                                {
                                    var amendmentHistory = savedAmendmentDataHistories.Where(x => x.FieldName == "PE_Email").FirstOrDefault();
                                    amendmentHistory.ModifiedValue = formModel.PE_Email.ToString();
                                    _context.Update<Licence_PE_AmendmentDataHistories>(amendmentHistory);
                                }
                                await _context.SaveChangesAsync();
                            }
                            else
                            {
                                if (previousPELicenseData.PE_Email != formModel.PE_Email)
                                {
                                    amendmentDataHistories.Add(new Licence_PE_AmendmentDataHistories()
                                    {
                                        AppRefId = formModel.AppRefId,
                                        FieldName = "PE_Email",
                                        PreviousValue = previousPELicenseData.PE_Email.ToString(),
                                        ModifiedValue = formModel.PE_Email.ToString(),
                                        SectionCode = "PED", //ConstructionSiteDetail
                                        ModifiedOn = DateTime.Now,
                                        ModifiedCounter = formModel.ModifiedCounter
                                    });
                                }
                            }

                            if (savedAmendmentDataHistories.Any(x => x.FieldName == "PE_Address"))
                            {
                                if (savedAmendmentDataHistories.Where(x => x.FieldName == "PE_Address").Select(x => x.PreviousValue).FirstOrDefault() == formModel.PE_Address)
                                {
                                    _context.Licence_PE_AmendmentDataHistories.Remove(savedAmendmentDataHistories.Where(x => x.FieldName == "PE_Address").FirstOrDefault());
                                }
                                else
                                {
                                    var amendmentHistory = savedAmendmentDataHistories.Where(x => x.FieldName == "PE_Address").FirstOrDefault();
                                    amendmentHistory.ModifiedValue = formModel.PE_Address.ToString();
                                    _context.Update<Licence_PE_AmendmentDataHistories>(amendmentHistory);
                                }
                                await _context.SaveChangesAsync();
                            }
                            else
                            {
                                if (previousPELicenseData.PE_Address != formModel.PE_Address)
                                {
                                    amendmentDataHistories.Add(new Licence_PE_AmendmentDataHistories()
                                    {
                                        AppRefId = formModel.AppRefId,
                                        FieldName = "PE_Address",
                                        PreviousValue = previousPELicenseData.PE_Address.ToString(),
                                        ModifiedValue = formModel.PE_Address.ToString(),
                                        SectionCode = "PED", //ContractLabourDetail
                                        ModifiedOn = DateTime.Now,
                                        ModifiedCounter = formModel.ModifiedCounter
                                    });
                                }
                            }

                            if (savedAmendmentDataHistories.Any(x => x.FieldName == "Manager_Name"))
                            {
                                if (savedAmendmentDataHistories.Where(x => x.FieldName == "Manager_Name").Select(x => x.PreviousValue).FirstOrDefault() == formModel.Manager_Name)
                                {
                                    _context.Licence_PE_AmendmentDataHistories.Remove(savedAmendmentDataHistories.Where(x => x.FieldName == "Manager_Name").FirstOrDefault());
                                }
                                else
                                {
                                    var amendmentHistory = savedAmendmentDataHistories.Where(x => x.FieldName == "Manager_Name").FirstOrDefault();
                                    amendmentHistory.ModifiedValue = formModel.Manager_Name.ToString();
                                    _context.Update<Licence_PE_AmendmentDataHistories>(amendmentHistory);
                                }
                                await _context.SaveChangesAsync();
                            }
                            else
                            {
                                if (previousPELicenseData.Manager_Name != formModel.Manager_Name)
                                {
                                    amendmentDataHistories.Add(new Licence_PE_AmendmentDataHistories()
                                    {
                                        AppRefId = formModel.AppRefId,
                                        FieldName = "Manager_Name",
                                        PreviousValue = previousPELicenseData.Manager_Name.ToString(),
                                        ModifiedValue = formModel.Manager_Name.ToString(),
                                        SectionCode = "MD", //ManagerDetail
                                        ModifiedOn = DateTime.Now,
                                        ModifiedCounter = formModel.ModifiedCounter
                                    });
                                }
                            }


                            if (savedAmendmentDataHistories.Any(x => x.FieldName == "Manager_Mobile"))
                            {
                                if (savedAmendmentDataHistories.Where(x => x.FieldName == "Manager_Mobile").Select(x => x.PreviousValue).FirstOrDefault() == formModel.Manager_Mobile)
                                {
                                    _context.Licence_PE_AmendmentDataHistories.Remove(savedAmendmentDataHistories.Where(x => x.FieldName == "Manager_Mobile").FirstOrDefault());
                                }
                                else
                                {
                                    var amendmentHistory = savedAmendmentDataHistories.Where(x => x.FieldName == "Manager_Mobile").FirstOrDefault();
                                    amendmentHistory.ModifiedValue = formModel.Manager_Mobile.ToString();
                                    _context.Update<Licence_PE_AmendmentDataHistories>(amendmentHistory);
                                }
                                await _context.SaveChangesAsync();
                            }
                            else
                            {
                                if (previousPELicenseData.Manager_Mobile != formModel.Manager_Mobile)
                                {
                                    amendmentDataHistories.Add(new Licence_PE_AmendmentDataHistories()
                                    {
                                        AppRefId = formModel.AppRefId,
                                        FieldName = "Manager_Mobile",
                                        PreviousValue = previousPELicenseData.Manager_Mobile.ToString(),
                                        ModifiedValue = formModel.Manager_Mobile.ToString(),
                                        SectionCode = "MD", //ManagerDetail
                                        ModifiedOn = DateTime.Now,
                                        ModifiedCounter = formModel.ModifiedCounter
                                    });
                                }
                            }

                            if (savedAmendmentDataHistories.Any(x => x.FieldName == "Manager_Email"))
                            {
                                if (savedAmendmentDataHistories.Where(x => x.FieldName == "Manager_Email").Select(x => x.PreviousValue).FirstOrDefault() == formModel.Manager_Email)
                                {
                                    _context.Licence_PE_AmendmentDataHistories.Remove(savedAmendmentDataHistories.Where(x => x.FieldName == "Manager_Email").FirstOrDefault());
                                }
                                else
                                {
                                    var amendmentHistory = savedAmendmentDataHistories.Where(x => x.FieldName == "Manager_Email").FirstOrDefault();
                                    amendmentHistory.ModifiedValue = formModel.Manager_Email.ToString();
                                    _context.Update<Licence_PE_AmendmentDataHistories>(amendmentHistory);
                                }
                                await _context.SaveChangesAsync();
                            }
                            else
                            {
                                if (previousPELicenseData.Manager_Email != formModel.Manager_Email)
                                {
                                    amendmentDataHistories.Add(new Licence_PE_AmendmentDataHistories()
                                    {
                                        AppRefId = formModel.AppRefId,
                                        FieldName = "Manager_Email",
                                        PreviousValue = previousPELicenseData.Manager_Email.ToString(),
                                        ModifiedValue = formModel.Manager_Email.ToString(),
                                        SectionCode = "MD", //ManagerDetail
                                        ModifiedOn = DateTime.Now,
                                        ModifiedCounter = formModel.ModifiedCounter
                                    });
                                }
                            }

                            if (savedAmendmentDataHistories.Any(x => x.FieldName == "Manager_Address"))
                            {
                                if (savedAmendmentDataHistories.Where(x => x.FieldName == "Manager_Address").Select(x => x.PreviousValue).FirstOrDefault() == formModel.Manager_Address)
                                {
                                    _context.Licence_PE_AmendmentDataHistories.Remove(savedAmendmentDataHistories.Where(x => x.FieldName == "Manager_Address").FirstOrDefault());
                                }
                                else
                                {
                                    var amendmentHistory = savedAmendmentDataHistories.Where(x => x.FieldName == "Manager_Address").FirstOrDefault();
                                    amendmentHistory.ModifiedValue = formModel.Manager_Address.ToString();
                                    _context.Update<Licence_PE_AmendmentDataHistories>(amendmentHistory);
                                }
                                await _context.SaveChangesAsync();
                            }
                            else
                            {
                                if (previousPELicenseData.Manager_Address != formModel.Manager_Address)
                                {
                                    amendmentDataHistories.Add(new Licence_PE_AmendmentDataHistories()
                                    {
                                        AppRefId = formModel.AppRefId,
                                        FieldName = "Manager_Address",
                                        PreviousValue = previousPELicenseData.Manager_Address.ToString(),
                                        ModifiedValue = formModel.Manager_Address.ToString(),
                                        SectionCode = "MD", //ManagerDetail
                                        ModifiedOn = DateTime.Now,
                                        ModifiedCounter = formModel.ModifiedCounter
                                    });
                                }
                            }
                            if (savedAmendmentDataHistories.Any(x => x.FieldName == "NatureOfWork"))
                            {
                                if (savedAmendmentDataHistories.Where(x => x.FieldName == "NatureOfWork").Select(x => x.PreviousValue).FirstOrDefault() == formModel.NatureOfWork)
                                {
                                    _context.Licence_PE_AmendmentDataHistories.Remove(savedAmendmentDataHistories.Where(x => x.FieldName == "NatureOfWork").FirstOrDefault());
                                }
                                else
                                {
                                    var amendmentHistory = savedAmendmentDataHistories.Where(x => x.FieldName == "NatureOfWork").FirstOrDefault();
                                    amendmentHistory.ModifiedValue = formModel.NatureOfWork.ToString();
                                    _context.Update<Licence_PE_AmendmentDataHistories>(amendmentHistory);
                                }
                                await _context.SaveChangesAsync();
                            }
                            else
                            {
                                if (previousPELicenseData.NatureOfWork != formModel.NatureOfWork)
                                {
                                    amendmentDataHistories.Add(new Licence_PE_AmendmentDataHistories()
                                    {
                                        AppRefId = formModel.AppRefId,
                                        FieldName = "NatureOfWork",
                                        PreviousValue = previousPELicenseData.NatureOfWork.ToString(),
                                        ModifiedValue = formModel.NatureOfWork.ToString(),
                                        SectionCode = "MD", //ManagerDetail
                                        ModifiedOn = DateTime.Now,
                                        ModifiedCounter = formModel.ModifiedCounter
                                    });
                                }
                            }
                            if (savedAmendmentDataHistories.Any(x => x.FieldName == "Manager_Address"))
                            {
                                if (savedAmendmentDataHistories.Where(x => x.FieldName == "Manager_Address").Select(x => x.PreviousValue).FirstOrDefault() == formModel.Manager_Address)
                                {
                                    _context.Licence_PE_AmendmentDataHistories.Remove(savedAmendmentDataHistories.Where(x => x.FieldName == "Manager_Address").FirstOrDefault());
                                }
                                else
                                {
                                    var amendmentHistory = savedAmendmentDataHistories.Where(x => x.FieldName == "Manager_Address").FirstOrDefault();
                                    amendmentHistory.ModifiedValue = formModel.Manager_Address.ToString();
                                    _context.Update<Licence_PE_AmendmentDataHistories>(amendmentHistory);
                                }
                                await _context.SaveChangesAsync();
                            }
                            else
                            {
                                if (previousPELicenseData.Manager_Address != formModel.Manager_Address)
                                {
                                    amendmentDataHistories.Add(new Licence_PE_AmendmentDataHistories()
                                    {
                                        AppRefId = formModel.AppRefId,
                                        FieldName = "Manager_Address",
                                        PreviousValue = previousPELicenseData.Manager_Address.ToString(),
                                        ModifiedValue = formModel.Manager_Address.ToString(),
                                        SectionCode = "MD", //ManagerDetail
                                        ModifiedOn = DateTime.Now,
                                        ModifiedCounter = formModel.ModifiedCounter
                                    });
                                }
                            }
                           
                           
                            if (amendmentDataHistories.Count() > 0)
                            {
                                await _context.BulkInsertAsync<Licence_PE_AmendmentDataHistories>(amendmentDataHistories);
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


        public async Task<GenericFormModel<Licence_CL_PE_ContractorDetailViewModel>> Get_CL_PE_Contractor_General_Detail(Int64 id, Int64 projectSiteId)
        {
            GenericFormModel<Licence_CL_PE_ContractorDetailViewModel> genericFormModel = new GenericFormModel<Licence_CL_PE_ContractorDetailViewModel>();
            try
            {
                genericFormModel.ListTemplateLists = new List<ListTemplate>();

                var parentWithChildObject = await _iGR_Licence_CL_PE_GeneralDetail
                    .GetAsync(x => x.AppRefId == id,  //Conditions         
                        null,                //Orders
                       x => x.Application, x => x.Application.ApplicationAction, x => x.Licence_CL_PE_Contrators)
                    .ConfigureAwait(false);

                genericFormModel.FormModel = new Licence_CL_PE_ContractorDetailViewModel();
                genericFormModel.IsEditAllowed = parentWithChildObject.FirstOrDefault().Application.IsAllowEdit;
                genericFormModel.IsLocked = parentWithChildObject.FirstOrDefault().Application.IsLocked;
                genericFormModel.FormModel.id = parentWithChildObject.FirstOrDefault().Id;
                genericFormModel.FormModel.ApplicationPurposeType = parentWithChildObject.FirstOrDefault().Application.ApplicationPurposeType;
                genericFormModel.AppFormStepsList = new List<AppFormStepsInfo>();
                genericFormModel.AppFormStepsList = await _iApplicationMamnagementService.GetAppFormStepperInfo(id, ApplicationTypeEnum.PRINCIPAL_EMPLOYER, (genericFormModel.FormModel != null ? parentWithChildObject.FirstOrDefault().Id : id), "CD");
            }
            catch (Exception ex)
            {
                genericFormModel.HasError = true;
                genericFormModel.ErrorDesc = ex.Message;
                throw ex;
            }
            return genericFormModel;
        }

        public async Task<GenericServiceResultTemplate> AddUpdate_Licence_CL_PEContractorDetail(Licence_CL_PE_ContractorDetailViewModel formModel)
        {
            GenericServiceResultTemplate genericServiceResultTemplate = new GenericServiceResultTemplate();
            try
            {
                Licence_CL_PE_GeneralDetail previousPELicenseData = null;
                List<Licence_PE_AmendmentDataHistories> savedAmendmentDataHistories = null;
                genericServiceResultTemplate.HasException = false;
                var contractexist = await _iGR_Licence_CL_PE_ContractorDetail.GetAsync(x => x.Licence_CL_PE_GeneralDetailRefId == formModel.id).ConfigureAwait(false);
                if (contractexist.Count() > 0) /*--Existing Rocord*/
                {
                    previousPELicenseData = _context.Licence_CL_PE_GeneralDetail.Where(x => x.Id == formModel.id).AsNoTracking().FirstOrDefault();

                    savedAmendmentDataHistories = await _context.Licence_PE_AmendmentDataHistories.Where(x =>
                        !x.IsLocked && x.AppRefId == previousPELicenseData.AppRefId &&
                        x.ModifiedCounter == previousPELicenseData.ModifiedCounter &&
                        (x.FieldName == "TotalWorker")).ToListAsync();




                    var pegeneralData = await _iGR_Licence_CL_PE_GeneralDetail.GetAsync(x => x.Id == formModel.id).ConfigureAwait(false);
                    _iGR_Licence_CL_PE_GeneralDetail.Update(pegeneralData.FirstOrDefault());
                    await _iGR_Licence_CL_PE_GeneralDetail.SavechangeAsync();

                    //Update last modified date
                    await _iApplicationMamnagementService.UpdateAppLastModifiedDate(formModel.AppRefId);
                    genericServiceResultTemplate.ApplicationInitiateResponse = new ApplicationInitiateResponseViewModel();
                    genericServiceResultTemplate.ApplicationInitiateResponse.EntityKeyId = formModel.id;
                    genericServiceResultTemplate.ApplicationInitiateResponse.AppId = formModel.AppRefId;

                }
                else /*--new Rocord*/
                {


                    //Update last modified date
                    await _iApplicationMamnagementService.UpdateAppLastModifiedDate(formModel.AppRefId);
                    genericServiceResultTemplate.ApplicationInitiateResponse = new ApplicationInitiateResponseViewModel();
                    genericServiceResultTemplate.ApplicationInitiateResponse.EntityKeyId = formModel.id;
                    genericServiceResultTemplate.ApplicationInitiateResponse.AppId = formModel.AppRefId;
                }
                var application = _context.Applications.Where(x => x.AppId == formModel.AppRefId && x.IsDeleted == false).FirstOrDefault();

                if (application.ApplicationPurposeType == ApplicationPurposeTypeEnum.AMENDMENT_LICENCE && previousPELicenseData != null)
                {
                    List<Licence_PE_AmendmentDataHistories> amendmentDataHistories = new List<Licence_PE_AmendmentDataHistories>();

                  
                    if (amendmentDataHistories.Count() > 0)
                    {
                        await _context.BulkInsertAsync<Licence_PE_AmendmentDataHistories>(amendmentDataHistories);
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

        public async Task<GenericFormModel<List<Licence_CL_PE_Contrator>>> Get_CL_PE_Contractor_List(Int64 appRefId)
        {
            GenericFormModel<List<Licence_CL_PE_Contrator>> genericFormModel = new GenericFormModel<List<Licence_CL_PE_Contrator>>();
            try
            {
                List<StoreProcedureParm> storeProcedureParms = new List<StoreProcedureParm>()
                {
                    new StoreProcedureParm (){ ParmName="AppRefId", ParmValue=appRefId.ToString(), isNumber=true},
                };
                var emps = await _iGeneric_SP_Repository.CallStoreProcedureReaderAsync<Licence_CL_PE_Contrator>("sp_CL_PELicence_GetAllContractor", storeProcedureParms);

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

        public async Task<GenericResponseTemplateModel<bool>> RemoveContractor(Int64 id)
        {
            GenericResponseTemplateModel<bool> genericFormModel = new GenericResponseTemplateModel<bool>();
            try
            {
                var emp = _context.Licence_CL_PE_Contrator.Where(x => x.Id == id).FirstOrDefault();
                _context.Licence_CL_PE_Contrator.RemoveRange(emp);
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

        public async Task<GenericFormModel<Licence_CL_PEViewModel>> Get_Licence_CL_PE_Detail(long id)
        {
            GenericFormModel<Licence_CL_PEViewModel> genericFormModel = new GenericFormModel<Licence_CL_PEViewModel>();
            try
            {
                genericFormModel.ListTemplateLists = new List<ListTemplate>();
                genericFormModel.FormModel = new Licence_CL_PEViewModel();
                genericFormModel.FormModel.GeneralDetail = new Licence_CL_PE_GeneralDetail();
                genericFormModel.FormModel.Licence_CL_PE_AmendmentDataHistories = new List<Licence_PE_AmendmentDataHistories>();

                var parentWithChildObject = await _iGR_Licence_CL_PE_GeneralDetail
                       .GetAsync(x => x.AppRefId == id,
                         null,
                         x => x.Application, x => x.Licence_CL_PE_Contrators, x => x.Application.ApplicationAction)
                       .ConfigureAwait(false);

                var oldparentWithChildObject = await _iGR_Licence_CL_PE_AmendmentHistories
                      .GetAsync(x => x.AppRefId == id,
                        null)
                      .ConfigureAwait(false);

                genericFormModel.FormModel.GeneralDetail = parentWithChildObject.FirstOrDefault();
                genericFormModel.FormModel.Licence_CL_PE_AmendmentDataHistories = oldparentWithChildObject.ToList();
                genericFormModel.FormModel.IsLocked = parentWithChildObject.FirstOrDefault().Application.IsLocked;
                genericFormModel.FormModel.IsFeeApplicable = parentWithChildObject.FirstOrDefault().Application.IsFeeApplicable;
                genericFormModel.ApplicationLifeCycleStatusType = parentWithChildObject.FirstOrDefault().Application.ApplicationLifeCycleStatusType;
                genericFormModel.FormModel.AppActionType = parentWithChildObject.FirstOrDefault().Application.ApplicationAction.AppActionType;

                genericFormModel.AppFormStepsList = new List<AppFormStepsInfo>();
                genericFormModel.AppFormStepsList = await _iApplicationMamnagementService.GetAppFormStepperInfo(genericFormModel.FormModel.GeneralDetail.AppRefId, ApplicationTypeEnum.PRINCIPAL_EMPLOYER, (genericFormModel.FormModel != null ? genericFormModel.FormModel.GeneralDetail.Id : id), "LOCK");

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

                    if (await _iApplicationMamnagementService.LockApplication(appRefId, AppActionType, remarks, ApplicationTypeEnum.PRINCIPAL_EMPLOYER) != "")
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
    }
}