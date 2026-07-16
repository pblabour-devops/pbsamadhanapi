using EFCore.BulkExtensions;
using Microsoft.EntityFrameworkCore;
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
    public class BocwLicenceService : IBocwLicenceService
    {
        private readonly IGenericRepository<Licence_BocwAct_GeneralDetail> _iGR_BocwLicence_GeneralDetail;
        private readonly IGenericRepository<Licence_BocwAct_ContractorDetail> _iGR_BocwLicence_ContractorDetail;
        private readonly IApplicationManagementService<Licence_BocwAct_GeneralDetail> _iApplicationMamnagementService;
        private readonly IGenericRepository<DistrictLgd> _iGR_DistrictLgd;
        private readonly IThirdPartyInegrationsService _iThirdPartyInegrationsService;
        private readonly IGeneric_SP_Repository _iGeneric_SP_Repository;
        private readonly IProjectSiteService _iProjectSiteService;
        private readonly AppDbContext _context;
        private readonly ISystem_O_CommunicationService _iSystem_O_CommunicationService;

        public BocwLicenceService(IGenericRepository<Licence_BocwAct_GeneralDetail> iGR_BocwLicence_GeneralDetail,
             IGenericRepository<Licence_BocwAct_ContractorDetail> iGR_BocwLicence_ContractorDetail,
             IApplicationManagementService<Licence_BocwAct_GeneralDetail> IApplicationMamnagementService,
             IGenericRepository<DistrictLgd> iGR_DistrictLgd, IZoneRepository iZoneRepository,
             IThirdPartyInegrationsService iThirdPartyInegrationsService,
             IGeneric_SP_Repository iGeneric_SP_Repository,
             IProjectSiteService iProjectSiteService,
             AppDbContext context,
             ISystem_O_CommunicationService iSystem_O_CommunicationService
            )
        {
            _iThirdPartyInegrationsService = iThirdPartyInegrationsService;
            _iGeneric_SP_Repository = iGeneric_SP_Repository;
            _iProjectSiteService = iProjectSiteService;
            _iGR_BocwLicence_GeneralDetail = iGR_BocwLicence_GeneralDetail;
            _iGR_BocwLicence_ContractorDetail = iGR_BocwLicence_ContractorDetail;
            _iGR_DistrictLgd = iGR_DistrictLgd;
            _context = context;
            _iApplicationMamnagementService = IApplicationMamnagementService;
            _iSystem_O_CommunicationService = iSystem_O_CommunicationService;
        }

        public async Task<GenericFormModel<Licence_BocwAct_GeneralDetail>> GetBocwLicenceGeneralDetail(long id, long projectSiteId)
        {
            GenericFormModel<Licence_BocwAct_GeneralDetail> genericFormModel = new GenericFormModel<Licence_BocwAct_GeneralDetail>();
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
                    var parentWithChildObject = await _iGR_BocwLicence_GeneralDetail
                       .GetAsync(x => x.AppRefId == id,  //Conditions         
                         null,                //Orders
                         x => x.Application, x => x.Application.ApplicationAction, x => x.Licence_BocwAct_ContractorDetail)      //Includes
                       .ConfigureAwait(false);
                    var contratorDetails = await _iGR_BocwLicence_GeneralDetail
                       .GetAsync(x => x.AppRefId == id,  //Conditions         
                         null,                //Orders
                         x => x.Licence_BocwAct_ContractorDetail)               //Includes
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
                    genericFormModel.FormModel = new Licence_BocwAct_GeneralDetail();

                    //Allow user to edit form
                    genericFormModel.IsEditAllowed = true;
                    genericFormModel.IsLocked = false;
                    genericFormModel.ApplicationLifeCycleStatusType = ApplicationLifeCycleStatusTypeEnum.NOT_SUBMITTED;
                    genericFormModel.FormModel.AlcCircleRefId = 0;
                }
                genericFormModel.FormModel.ProjectSiteRefId = projectSiteId;

                //Initialization of Enum types of lists

                genericFormModel.EnumTemplateLists = new List<EnumListTemplate>();
                genericFormModel.EnumTemplateLists.Add(new EnumListTemplate()
                {
                    SelectListTypeCode = "ConstructionBuildingTypeEnum",
                    SelectListItems = EnumOps.GetEnumAsSelectList<ConstructionBuildingTypeEnum>()
                });

                genericFormModel.EnumTemplateLists.Add(new EnumListTemplate()
                {
                    SelectListTypeCode = "BocwEngagedWorkerSlabTypeEnum",
                    SelectListItems = EnumOps.GetEnumAsSelectList<BocwEngagedWorkerSlabTypeEnum>()
                });

                genericFormModel.EnumTemplateLists.Add(new EnumListTemplate()
                {
                    SelectListTypeCode = "BocwRegisteredTypeEnum",
                    SelectListItems = EnumOps.GetEnumAsSelectList<BocwRegisteredTypeEnum>()
                });

                genericFormModel.EnumTemplateLists.Add(new EnumListTemplate()
                {
                    SelectListTypeCode = "EngagedAnyContractorTypeEnum",
                    SelectListItems = EnumOps.GetEnumAsSelectList<EngagedAnyContractorTypeEnum>()
                });

                genericFormModel.EnumTemplateLists.Add(new EnumListTemplate()
                {
                    SelectListTypeCode = "ApplicationPurposeTypeEnum",
                    SelectListItems = EnumOps.GetEnumAsSelectList<ApplicationPurposeTypeEnum>()
                });

                genericFormModel.EnumTemplateLists.Add(new EnumListTemplate()
                {
                    SelectListTypeCode = "BOCWActCircleTypeEnum",
                    SelectListItems = EnumOps.GetEnumAsSelectList<BOCWActCircleTypeEnum>()
                });

                genericFormModel.AppFormStepsList = new List<AppFormStepsInfo>();
                genericFormModel.AppFormStepsList = await _iApplicationMamnagementService.GetAppFormStepperInfo(id, ApplicationTypeEnum.BOCW_ESTABLISHMENT_ACT, (genericFormModel.FormModel != null ? genericFormModel.FormModel.BocwEstablishmentRegistrationId : id), "GD");
            }
            catch (Exception ex)
            {
                genericFormModel.HasError = true;
                genericFormModel.ErrorDesc = ex.Message;
            }
            return genericFormModel;

        }



        public async Task<GenericServiceResultTemplate> AddUpdate_OccupierAndManagerDetail(Licence_BocwAct_GeneralDetail formModel, string id)
        {
            GenericServiceResultTemplate genericServiceResultTemplate = new GenericServiceResultTemplate();
            try
            {
                genericServiceResultTemplate.HasException = false;
                genericServiceResultTemplate.CustomeValidationResult = CustomeValidator<Licence_BocwAct_GeneralDetail>.ValidateModel_AllProperties(formModel);
                genericServiceResultTemplate.ApplicationInitiateResponse = new ApplicationInitiateResponseViewModel();
                genericServiceResultTemplate.ApplicationInitiateResponse.EntityKeyId = formModel.BocwEstablishmentRegistrationId;
                genericServiceResultTemplate.ApplicationInitiateResponse.AppId = formModel.AppRefId;
                if (genericServiceResultTemplate.CustomeValidationResult.IsValid)
                {
                    Licence_BocwAct_GeneralDetail previousBocwActLicenseData = null;
                    List<Licence_Bocw_AmendmentDataHistories> savedAmendmentDataHistories = null;
                    if (formModel.BocwEstablishmentRegistrationId != 0) //Existing record
                    {

                        if (formModel.BocwRegisteredType == BocwRegisteredTypeEnum.CONTRACTOR)
                        {
                            dynamic data = JsonConvert.DeserializeObject<dynamic>(formModel.ContractorDetails);

                            Licence_BocwAct_ContractorDetail contractorData = new Licence_BocwAct_ContractorDetail
                            {
                                ContractorName = data.contractorName,
                                ContractorAddress = data.contractorAddress,
                                NatureOfConstructionWork = data.natureOfConstructionWork,
                                maxNoOfContractorOnAnyDay = data.maxNoOfContractorOnAnyDay,
                                Contractor_Work_CommencementDate = ParseDateTime(data.contractor_Work_CommencementDate),
                                Contractor_Work_CompletionDate = ParseDateTime(data.contractor_Work_CompletionDate),
                                BocwEstablishmentRegistrationRefId = genericServiceResultTemplate.ApplicationInitiateResponse.EntityKeyId,
                            };
                            //-----Date Time Format Change----
                            DateTime? ParseDateTime(dynamic value)
                            {
                                if (value == null)
                                {
                                    return null;
                                }
                                return DateTime.Parse(value.ToString());
                            }
                            _iGR_BocwLicence_ContractorDetail.Insert(contractorData);
                            await _context.SaveChangesAsync();

                            previousBocwActLicenseData = _context.Licence_BocwAct_GeneralDetails.Where(x => x.BocwEstablishmentRegistrationId == formModel.BocwEstablishmentRegistrationId).AsNoTracking().FirstOrDefault();
                            savedAmendmentDataHistories = await _context.Licence_Bocw_AmendmentDataHistories.Where(x =>
                            !x.IsLocked &&
                            x.ModifiedCounter == previousBocwActLicenseData.ModifiedCounter &&
                            (x.FieldName == "No_of_Worker")).ToListAsync();

                            if (savedAmendmentDataHistories.Count() > 0)
                            {
                                formModel.ModifiedCounter = previousBocwActLicenseData.ModifiedCounter + 1;
                            }
                            else
                            {
                                formModel.ModifiedCounter = previousBocwActLicenseData.ModifiedCounter;
                            }
                            _iGR_BocwLicence_GeneralDetail.Update(formModel);
                            await _iGR_BocwLicence_GeneralDetail.SavechangeAsync();
                            //Update last modified date
                            await _iApplicationMamnagementService.UpdateAppLastModifiedDate(formModel.AppRefId);
                            genericServiceResultTemplate.ApplicationInitiateResponse = new ApplicationInitiateResponseViewModel();
                            genericServiceResultTemplate.ApplicationInitiateResponse.EntityKeyId = formModel.BocwEstablishmentRegistrationId;
                            genericServiceResultTemplate.ApplicationInitiateResponse.AppId = formModel.AppRefId;

                        }
                        else
                        {
                            previousBocwActLicenseData = _context.Licence_BocwAct_GeneralDetails.Where(x => x.BocwEstablishmentRegistrationId == formModel.BocwEstablishmentRegistrationId).AsNoTracking().FirstOrDefault();
                            savedAmendmentDataHistories = await _context.Licence_Bocw_AmendmentDataHistories.Where(x =>
                            !x.IsLocked &&
                            x.ModifiedCounter == previousBocwActLicenseData.ModifiedCounter &&
                            (x.FieldName == "No_of_Worker")).ToListAsync();

                            if (savedAmendmentDataHistories.Count() > 0)
                            {
                                formModel.ModifiedCounter = previousBocwActLicenseData.ModifiedCounter + 1;
                            }
                            else
                            {
                                formModel.ModifiedCounter = previousBocwActLicenseData.ModifiedCounter;
                            }
                            _iGR_BocwLicence_GeneralDetail.Update(formModel);
                            await _iGR_BocwLicence_GeneralDetail.SavechangeAsync();
                            //Update last modified date
                            await _iApplicationMamnagementService.UpdateAppLastModifiedDate(formModel.AppRefId);
                            genericServiceResultTemplate.ApplicationInitiateResponse = new ApplicationInitiateResponseViewModel();
                            genericServiceResultTemplate.ApplicationInitiateResponse.EntityKeyId = formModel.BocwEstablishmentRegistrationId;
                            genericServiceResultTemplate.ApplicationInitiateResponse.AppId = formModel.AppRefId;


                        }
                        // Update Circle
                        await _iProjectSiteService.SetProjectSiteCircle(formModel.ProjectSiteRefId, 0, 0, formModel.AlcCircleRefId);

                        var isStatusShared = await _iThirdPartyInegrationsService.HasShareStatusToBusinessFirst(genericServiceResultTemplate.ApplicationInitiateResponse.AppId);
                        if (isStatusShared != null && !isStatusShared.HasError && !isStatusShared.ResponseDataModel)
                        {
                            // Update Native Appid by IPIN
                            await _iThirdPartyInegrationsService.UpdateBusinessFirstRequestNativeAppIdByIPin(formModel.IPin, formModel.InvestPunjab_AppId, genericServiceResultTemplate.ApplicationInitiateResponse.AppId, ApplicationTypeEnum.SHOP_LICENCE, formModel.ApplicationPurposeType);

                            // Share status to Invest Punjab Portal
                            //await _iThirdPartyInegrationsService.ShareStatusToBusinessFirst(genericServiceResultTemplate.ApplicationInitiateResponse.AppId, ApplicationTypeEnum.SHOP_LICENCE, AppActionTypeEnum.APP_SAVE_DRAFT);
                        }

                    }
                    else //New record
                    {
                        if (formModel.BocwRegisteredType == BocwRegisteredTypeEnum.CONTRACTOR)
                        {

                            formModel = GenericModelOps<Licence_BocwAct_GeneralDetail>.SetNullAllNevigationProperties(formModel);
                            genericServiceResultTemplate.ApplicationInitiateResponse = await _iApplicationMamnagementService.InitiateApplication(ApplicationTypeEnum.BOCW_ESTABLISHMENT_ACT, formModel, formModel.ProjectSiteRefId, formModel.ApplicationPurposeType, id, formModel.IPin, formModel.InvestPunjab_AppId, false, 0, 0, null, null, formModel.ProjectSiteVersion);

                            dynamic data = JsonConvert.DeserializeObject<dynamic>(formModel.ContractorDetails);

                            Licence_BocwAct_ContractorDetail contractorData = new Licence_BocwAct_ContractorDetail
                            {
                                ContractorName = data.contractorName,
                                ContractorAddress = data.contractorAddress,
                                NatureOfConstructionWork = data.natureOfConstructionWork,
                                maxNoOfContractorOnAnyDay = data.maxNoOfContractorOnAnyDay,
                                Contractor_Work_CommencementDate = ParseDateTime(data.contractor_Work_CommencementDate),
                                Contractor_Work_CompletionDate = ParseDateTime(data.contractor_Work_CompletionDate),
                                BocwEstablishmentRegistrationRefId = genericServiceResultTemplate.ApplicationInitiateResponse.EntityKeyId,
                            };
                            //-----Date Time Format Change----
                            DateTime? ParseDateTime(dynamic value)
                            {
                                if (value == null)
                                {
                                    return null;
                                }
                                return DateTime.Parse(value.ToString());
                            }
                            _iGR_BocwLicence_ContractorDetail.Insert(contractorData);
                            await _context.SaveChangesAsync();


                            if (genericServiceResultTemplate.ApplicationInitiateResponse.IsApplicationCreated == true)
                            {
                                // Update Native Appid by IPIN
                                var temp = await _iThirdPartyInegrationsService.UpdateBusinessFirstRequestNativeAppIdByIPin(formModel.IPin, formModel.InvestPunjab_AppId, genericServiceResultTemplate.ApplicationInitiateResponse.AppId, ApplicationTypeEnum.BOCW_ESTABLISHMENT_ACT, formModel.ApplicationPurposeType);

                                // Share status to Invest Punjab Portal
                                await _iThirdPartyInegrationsService.ShareStatusToBusinessFirst(genericServiceResultTemplate.ApplicationInitiateResponse.AppId, ApplicationTypeEnum.BOCW_ESTABLISHMENT_ACT, AppActionTypeEnum.APP_SAVE_DRAFT);
                            }
                            else if (genericServiceResultTemplate.ApplicationInitiateResponse.IsApplicationCreated == false)
                            {
                                genericServiceResultTemplate.HasException = true;
                                genericServiceResultTemplate.Exceptions = new Exception();
                            }
                        }
                        else
                        {
                            formModel = GenericModelOps<Licence_BocwAct_GeneralDetail>.SetNullAllNevigationProperties(formModel);
                            genericServiceResultTemplate.ApplicationInitiateResponse = await _iApplicationMamnagementService.InitiateApplication(ApplicationTypeEnum.BOCW_ESTABLISHMENT_ACT, formModel, formModel.ProjectSiteRefId, formModel.ApplicationPurposeType, id, formModel.IPin, formModel.InvestPunjab_AppId, false, 0, 0, null, null, formModel.ProjectSiteVersion);

                            if (genericServiceResultTemplate.ApplicationInitiateResponse.IsApplicationCreated == true)
                            {
                                // Update Native Appid by IPIN
                                var temp = await _iThirdPartyInegrationsService.UpdateBusinessFirstRequestNativeAppIdByIPin(formModel.IPin, formModel.InvestPunjab_AppId, genericServiceResultTemplate.ApplicationInitiateResponse.AppId, ApplicationTypeEnum.BOCW_ESTABLISHMENT_ACT, formModel.ApplicationPurposeType);

                                // Share status to Invest Punjab Portal
                                await _iThirdPartyInegrationsService.ShareStatusToBusinessFirst(genericServiceResultTemplate.ApplicationInitiateResponse.AppId, ApplicationTypeEnum.BOCW_ESTABLISHMENT_ACT, AppActionTypeEnum.APP_SAVE_DRAFT);
                            }
                            else if (genericServiceResultTemplate.ApplicationInitiateResponse.IsApplicationCreated == false)
                            {
                                genericServiceResultTemplate.HasException = true;
                                genericServiceResultTemplate.Exceptions = new Exception();
                            }
                        }

                    }
                    var application = _context.Applications.Where(x => x.AppId == formModel.AppRefId && x.IsDeleted == false).FirstOrDefault();

                    if (application.ApplicationPurposeType == ApplicationPurposeTypeEnum.AMENDMENT_LICENCE && previousBocwActLicenseData != null)
                    {
                        List<Licence_Bocw_AmendmentDataHistories> amendmentDataHistories = new List<Licence_Bocw_AmendmentDataHistories>();

                        if (savedAmendmentDataHistories.Any(x => x.FieldName == "ConstructionBuildingDesc"))
                        {
                            if (savedAmendmentDataHistories.Where(x => x.FieldName == "ConstructionBuildingDesc").Select(x => x.PreviousValue).FirstOrDefault() == formModel.ConstructionBuildingDesc)
                            {
                                _context.Licence_Bocw_AmendmentDataHistories.Remove(savedAmendmentDataHistories.Where(x => x.FieldName == "ConstructionBuildingDesc").FirstOrDefault());
                            }
                            else
                            {
                                var amendmentHistory = savedAmendmentDataHistories.Where(x => x.FieldName == "ConstructionBuildingDesc").FirstOrDefault();
                                amendmentHistory.ModifiedValue = formModel.ConstructionBuildingDesc.ToString();
                                _context.Update<Licence_Bocw_AmendmentDataHistories>(amendmentHistory);
                            }
                            await _context.SaveChangesAsync();
                        }
                        else
                        {
                            if (previousBocwActLicenseData.ConstructionBuildingDesc != formModel.ConstructionBuildingDesc)
                            {
                                amendmentDataHistories.Add(new Licence_Bocw_AmendmentDataHistories()
                                {
                                    AppRefId = formModel.AppRefId,
                                    FieldName = "ConstructionBuildingDesc",
                                    PreviousValue = previousBocwActLicenseData.ConstructionBuildingDesc.ToString(),
                                    ModifiedValue = formModel.ConstructionBuildingDesc.ToString(),
                                    SectionCode = "CBD", //ConstructionBuildingDetail
                                    ModifiedOn = DateTime.Now,
                                    ModifiedCounter = formModel.ModifiedCounter
                                });
                            }
                        }

                        if (savedAmendmentDataHistories.Any(x => x.FieldName == "ConstructionSite_Name"))
                        {
                            if (savedAmendmentDataHistories.Where(x => x.FieldName == "ConstructionSite_Name").Select(x => x.PreviousValue).FirstOrDefault() == formModel.ConstructionSite_Name)
                            {
                                _context.Licence_Bocw_AmendmentDataHistories.Remove(savedAmendmentDataHistories.Where(x => x.FieldName == "ConstructionSite_Name").FirstOrDefault());
                            }
                            else
                            {
                                var amendmentHistory = savedAmendmentDataHistories.Where(x => x.FieldName == "ConstructionSite_Name").FirstOrDefault();
                                amendmentHistory.ModifiedValue = formModel.ConstructionSite_Name.ToString();
                                _context.Update<Licence_Bocw_AmendmentDataHistories>(amendmentHistory);
                            }
                            await _context.SaveChangesAsync();
                        }
                        else
                        {
                            if (previousBocwActLicenseData.ConstructionSite_Name != formModel.ConstructionSite_Name)
                            {
                                amendmentDataHistories.Add(new Licence_Bocw_AmendmentDataHistories()
                                {
                                    AppRefId = formModel.AppRefId,
                                    FieldName = "ConstructionSite_Name",
                                    PreviousValue = previousBocwActLicenseData.ConstructionSite_Name.ToString(),
                                    ModifiedValue = formModel.ConstructionSite_Name.ToString(),
                                    SectionCode = "CSD", //ConstructionSiteDetail
                                    ModifiedOn = DateTime.Now,
                                    ModifiedCounter = formModel.ModifiedCounter
                                });
                            }
                        }

                        if (savedAmendmentDataHistories.Any(x => x.FieldName == "ConstructionSite_Address"))
                        {
                            if (savedAmendmentDataHistories.Where(x => x.FieldName == "ConstructionSite_Address").Select(x => x.PreviousValue).FirstOrDefault() == formModel.ConstructionSite_Address)
                            {
                                _context.Licence_Bocw_AmendmentDataHistories.Remove(savedAmendmentDataHistories.Where(x => x.FieldName == "ConstructionSite_Address").FirstOrDefault());
                            }
                            else
                            {
                                var amendmentHistory = savedAmendmentDataHistories.Where(x => x.FieldName == "ConstructionSite_Address").FirstOrDefault();
                                amendmentHistory.ModifiedValue = formModel.ConstructionSite_Address.ToString();
                                _context.Update<Licence_Bocw_AmendmentDataHistories>(amendmentHistory);
                            }
                            await _context.SaveChangesAsync();
                        }
                        else
                        {
                            if (previousBocwActLicenseData.ConstructionSite_Address != formModel.ConstructionSite_Address)
                            {
                                amendmentDataHistories.Add(new Licence_Bocw_AmendmentDataHistories()
                                {
                                    AppRefId = formModel.AppRefId,
                                    FieldName = "ConstructionSite_Address",
                                    PreviousValue = previousBocwActLicenseData.ConstructionSite_Address.ToString(),
                                    ModifiedValue = formModel.ConstructionSite_Address.ToString(),
                                    SectionCode = "CSD", //ConstructionSiteDetail
                                    ModifiedOn = DateTime.Now,
                                    ModifiedCounter = formModel.ModifiedCounter
                                });
                            }
                        }

                        if (savedAmendmentDataHistories.Any(x => x.FieldName == "ConstructionSite_PinCode"))
                        {
                            if (savedAmendmentDataHistories.Where(x => x.FieldName == "ConstructionSite_PinCode").Select(x => x.PreviousValue).FirstOrDefault() == formModel.ConstructionSite_PinCode)
                            {
                                _context.Licence_Bocw_AmendmentDataHistories.Remove(savedAmendmentDataHistories.Where(x => x.FieldName == "ConstructionSite_PinCode").FirstOrDefault());
                            }
                            else
                            {
                                var amendmentHistory = savedAmendmentDataHistories.Where(x => x.FieldName == "ConstructionSite_PinCode").FirstOrDefault();
                                amendmentHistory.ModifiedValue = formModel.ConstructionSite_PinCode.ToString();
                                _context.Update<Licence_Bocw_AmendmentDataHistories>(amendmentHistory);
                            }
                            await _context.SaveChangesAsync();
                        }
                        else
                        {
                            if (previousBocwActLicenseData.ConstructionSite_PinCode != formModel.ConstructionSite_PinCode)
                            {
                                amendmentDataHistories.Add(new Licence_Bocw_AmendmentDataHistories()
                                {
                                    AppRefId = formModel.AppRefId,
                                    FieldName = "ConstructionSite_PinCode",
                                    PreviousValue = previousBocwActLicenseData.ConstructionSite_PinCode.ToString(),
                                    ModifiedValue = formModel.ConstructionSite_PinCode.ToString(),
                                    SectionCode = "CSD", //ConstructionSiteDetail
                                    ModifiedOn = DateTime.Now,
                                    ModifiedCounter = formModel.ModifiedCounter
                                });
                            }
                        }

                        if (savedAmendmentDataHistories.Any(x => x.FieldName == "ContractLabourLicenceNumber"))
                        {
                            if (savedAmendmentDataHistories.Where(x => x.FieldName == "ContractLabourLicenceNumber").Select(x => x.PreviousValue).FirstOrDefault() == formModel.ContractLabourLicenceNumber)
                            {
                                _context.Licence_Bocw_AmendmentDataHistories.Remove(savedAmendmentDataHistories.Where(x => x.FieldName == "ContractLabourLicenceNumber").FirstOrDefault());
                            }
                            else
                            {
                                var amendmentHistory = savedAmendmentDataHistories.Where(x => x.FieldName == "ContractLabourLicenceNumber").FirstOrDefault();
                                amendmentHistory.ModifiedValue = formModel.ContractLabourLicenceNumber.ToString();
                                _context.Update<Licence_Bocw_AmendmentDataHistories>(amendmentHistory);
                            }
                            await _context.SaveChangesAsync();
                        }
                        else
                        {
                            if (previousBocwActLicenseData.ContractLabourLicenceNumber != formModel.ContractLabourLicenceNumber)
                            {
                                amendmentDataHistories.Add(new Licence_Bocw_AmendmentDataHistories()
                                {
                                    AppRefId = formModel.AppRefId,
                                    FieldName = "ContractLabourLicenceNumber",
                                    PreviousValue = previousBocwActLicenseData.ContractLabourLicenceNumber.ToString(),
                                    ModifiedValue = formModel.ContractLabourLicenceNumber.ToString(),
                                    SectionCode = "CLD", //ContractLabourDetail
                                    ModifiedOn = DateTime.Now,
                                    ModifiedCounter = formModel.ModifiedCounter
                                });
                            }
                        }

                        if (savedAmendmentDataHistories.Any(x => x.FieldName == "ManagerAddress"))
                        {
                            if (savedAmendmentDataHistories.Where(x => x.FieldName == "ManagerAddress").Select(x => x.PreviousValue).FirstOrDefault() == formModel.ManagerAddress)
                            {
                                _context.Licence_Bocw_AmendmentDataHistories.Remove(savedAmendmentDataHistories.Where(x => x.FieldName == "ManagerAddress").FirstOrDefault());
                            }
                            else
                            {
                                var amendmentHistory = savedAmendmentDataHistories.Where(x => x.FieldName == "ManagerAddress").FirstOrDefault();
                                amendmentHistory.ModifiedValue = formModel.ManagerAddress.ToString();
                                _context.Update<Licence_Bocw_AmendmentDataHistories>(amendmentHistory);
                            }
                            await _context.SaveChangesAsync();
                        }
                        else
                        {
                            if (previousBocwActLicenseData.ManagerAddress != formModel.ManagerAddress)
                            {
                                amendmentDataHistories.Add(new Licence_Bocw_AmendmentDataHistories()
                                {
                                    AppRefId = formModel.AppRefId,
                                    FieldName = "ManagerAddress",
                                    PreviousValue = previousBocwActLicenseData.ManagerAddress.ToString(),
                                    ModifiedValue = formModel.ManagerAddress.ToString(),
                                    SectionCode = "MD", //ManagerDetail
                                    ModifiedOn = DateTime.Now,
                                    ModifiedCounter = formModel.ModifiedCounter
                                });
                            }
                        }


                        if (savedAmendmentDataHistories.Any(x => x.FieldName == "ManagerName"))
                        {
                            if (savedAmendmentDataHistories.Where(x => x.FieldName == "ManagerName").Select(x => x.PreviousValue).FirstOrDefault() == formModel.ManagerName)
                            {
                                _context.Licence_Bocw_AmendmentDataHistories.Remove(savedAmendmentDataHistories.Where(x => x.FieldName == "ManagerName").FirstOrDefault());
                            }
                            else
                            {
                                var amendmentHistory = savedAmendmentDataHistories.Where(x => x.FieldName == "ManagerName").FirstOrDefault();
                                amendmentHistory.ModifiedValue = formModel.ManagerName.ToString();
                                _context.Update<Licence_Bocw_AmendmentDataHistories>(amendmentHistory);
                            }
                            await _context.SaveChangesAsync();
                        }
                        else
                        {
                            if (previousBocwActLicenseData.ManagerName != formModel.ManagerName)
                            {
                                amendmentDataHistories.Add(new Licence_Bocw_AmendmentDataHistories()
                                {
                                    AppRefId = formModel.AppRefId,
                                    FieldName = "ManagerName",
                                    PreviousValue = previousBocwActLicenseData.ManagerName.ToString(),
                                    ModifiedValue = formModel.ManagerName.ToString(),
                                    SectionCode = "MD", //ManagerDetail
                                    ModifiedOn = DateTime.Now,
                                    ModifiedCounter = formModel.ModifiedCounter
                                });
                            }
                        }

                        if (savedAmendmentDataHistories.Any(x => x.FieldName == "ManagerEmail"))
                        {
                            if (savedAmendmentDataHistories.Where(x => x.FieldName == "ManagerEmail").Select(x => x.PreviousValue).FirstOrDefault() == formModel.ManagerEmail)
                            {
                                _context.Licence_Bocw_AmendmentDataHistories.Remove(savedAmendmentDataHistories.Where(x => x.FieldName == "ManagerEmail").FirstOrDefault());
                            }
                            else
                            {
                                var amendmentHistory = savedAmendmentDataHistories.Where(x => x.FieldName == "ManagerEmail").FirstOrDefault();
                                amendmentHistory.ModifiedValue = formModel.ManagerEmail.ToString();
                                _context.Update<Licence_Bocw_AmendmentDataHistories>(amendmentHistory);
                            }
                            await _context.SaveChangesAsync();
                        }
                        else
                        {
                            if (previousBocwActLicenseData.ManagerEmail != formModel.ManagerEmail)
                            {
                                amendmentDataHistories.Add(new Licence_Bocw_AmendmentDataHistories()
                                {
                                    AppRefId = formModel.AppRefId,
                                    FieldName = "ManagerEmail",
                                    PreviousValue = previousBocwActLicenseData.ManagerEmail.ToString(),
                                    ModifiedValue = formModel.ManagerEmail.ToString(),
                                    SectionCode = "MD", //ManagerDetail
                                    ModifiedOn = DateTime.Now,
                                    ModifiedCounter = formModel.ModifiedCounter
                                });
                            }
                        }

                        if (savedAmendmentDataHistories.Any(x => x.FieldName == "ManagerMobile"))
                        {
                            if (savedAmendmentDataHistories.Where(x => x.FieldName == "ManagerMobile").Select(x => x.PreviousValue).FirstOrDefault() == formModel.ManagerMobile)
                            {
                                _context.Licence_Bocw_AmendmentDataHistories.Remove(savedAmendmentDataHistories.Where(x => x.FieldName == "ManagerMobile").FirstOrDefault());
                            }
                            else
                            {
                                var amendmentHistory = savedAmendmentDataHistories.Where(x => x.FieldName == "ManagerMobile").FirstOrDefault();
                                amendmentHistory.ModifiedValue = formModel.ManagerMobile.ToString();
                                _context.Update<Licence_Bocw_AmendmentDataHistories>(amendmentHistory);
                            }
                            await _context.SaveChangesAsync();
                        }
                        else
                        {
                            if (previousBocwActLicenseData.ManagerMobile != formModel.ManagerMobile)
                            {
                                amendmentDataHistories.Add(new Licence_Bocw_AmendmentDataHistories()
                                {
                                    AppRefId = formModel.AppRefId,
                                    FieldName = "ManagerMobile",
                                    PreviousValue = previousBocwActLicenseData.ManagerMobile.ToString(),
                                    ModifiedValue = formModel.ManagerMobile.ToString(),
                                    SectionCode = "MD", //ManagerDetail
                                    ModifiedOn = DateTime.Now,
                                    ModifiedCounter = formModel.ModifiedCounter
                                });
                            }
                        }


                        if (savedAmendmentDataHistories.Any(x => x.FieldName == "PrincipalEmployerName"))
                        {
                            if (savedAmendmentDataHistories.Where(x => x.FieldName == "PrincipalEmployerName").Select(x => x.PreviousValue).FirstOrDefault() == formModel.PrincipalEmployerName)
                            {
                                _context.Licence_Bocw_AmendmentDataHistories.Remove(savedAmendmentDataHistories.Where(x => x.FieldName == "PrincipalEmployerName").FirstOrDefault());
                            }
                            else
                            {
                                var amendmentHistory = savedAmendmentDataHistories.Where(x => x.FieldName == "PrincipalEmployerName").FirstOrDefault();
                                amendmentHistory.ModifiedValue = formModel.PrincipalEmployerName.ToString();
                                _context.Update<Licence_Bocw_AmendmentDataHistories>(amendmentHistory);
                            }
                            await _context.SaveChangesAsync();
                        }
                        else
                        {
                            if (previousBocwActLicenseData.PrincipalEmployerName != formModel.PrincipalEmployerName)
                            {
                                amendmentDataHistories.Add(new Licence_Bocw_AmendmentDataHistories()
                                {
                                    AppRefId = formModel.AppRefId,
                                    FieldName = "PrincipalEmployerName",
                                    PreviousValue = previousBocwActLicenseData.PrincipalEmployerName.ToString(),
                                    ModifiedValue = formModel.PrincipalEmployerName.ToString(),
                                    SectionCode = "PED", //PrincipalEmployerDetail
                                    ModifiedOn = DateTime.Now,
                                    ModifiedCounter = formModel.ModifiedCounter
                                });
                            }
                        }

                        if (savedAmendmentDataHistories.Any(x => x.FieldName == "PrincipalEmployerFatherName"))
                        {
                            if (savedAmendmentDataHistories.Where(x => x.FieldName == "PrincipalEmployerFatherName").Select(x => x.PreviousValue).FirstOrDefault() == formModel.PrincipalEmployerFatherName)
                            {
                                _context.Licence_Bocw_AmendmentDataHistories.Remove(savedAmendmentDataHistories.Where(x => x.FieldName == "PrincipalEmployerFatherName").FirstOrDefault());
                            }
                            else
                            {
                                var amendmentHistory = savedAmendmentDataHistories.Where(x => x.FieldName == "PrincipalEmployerFatherName").FirstOrDefault();
                                amendmentHistory.ModifiedValue = formModel.PrincipalEmployerFatherName.ToString();
                                _context.Update<Licence_Bocw_AmendmentDataHistories>(amendmentHistory);
                            }
                            await _context.SaveChangesAsync();
                        }
                        else
                        {
                            if (previousBocwActLicenseData.PrincipalEmployerFatherName != formModel.PrincipalEmployerFatherName)
                            {
                                amendmentDataHistories.Add(new Licence_Bocw_AmendmentDataHistories()
                                {
                                    AppRefId = formModel.AppRefId,
                                    FieldName = "PrincipalEmployerFatherName",
                                    PreviousValue = previousBocwActLicenseData.PrincipalEmployerFatherName.ToString(),
                                    ModifiedValue = formModel.PrincipalEmployerFatherName.ToString(),
                                    SectionCode = "PED",   //PrincipalEmployerDetail
                                    ModifiedOn = DateTime.Now,
                                    ModifiedCounter = formModel.ModifiedCounter
                                });
                            }
                        }

                        if (savedAmendmentDataHistories.Any(x => x.FieldName == "PrincipalEmployerEmail"))
                        {
                            if (savedAmendmentDataHistories.Where(x => x.FieldName == "PrincipalEmployerEmail").Select(x => x.PreviousValue).FirstOrDefault() == formModel.PrincipalEmployerEmail)
                            {
                                _context.Licence_Bocw_AmendmentDataHistories.Remove(savedAmendmentDataHistories.Where(x => x.FieldName == "PrincipalEmployerEmail").FirstOrDefault());
                            }
                            else
                            {
                                var amendmentHistory = savedAmendmentDataHistories.Where(x => x.FieldName == "PrincipalEmployerEmail").FirstOrDefault();
                                amendmentHistory.ModifiedValue = formModel.PrincipalEmployerEmail.ToString();
                                _context.Update<Licence_Bocw_AmendmentDataHistories>(amendmentHistory);
                            }
                            await _context.SaveChangesAsync();
                        }
                        else
                        {
                            if (previousBocwActLicenseData.PrincipalEmployerEmail != formModel.PrincipalEmployerEmail)
                            {
                                amendmentDataHistories.Add(new Licence_Bocw_AmendmentDataHistories()
                                {
                                    AppRefId = formModel.AppRefId,
                                    FieldName = "PrincipalEmployerEmail",
                                    PreviousValue = previousBocwActLicenseData.PrincipalEmployerEmail.ToString(),
                                    ModifiedValue = formModel.PrincipalEmployerEmail.ToString(),
                                    SectionCode = "PED",  //PrincipalEmployerDetail
                                    ModifiedOn = DateTime.Now,
                                    ModifiedCounter = formModel.ModifiedCounter
                                });
                            }
                        }

                        if (savedAmendmentDataHistories.Any(x => x.FieldName == "PrincipalEmployerMobileNo"))
                        {
                            if (savedAmendmentDataHistories.Where(x => x.FieldName == "PrincipalEmployerMobileNo").Select(x => x.PreviousValue).FirstOrDefault() == formModel.PrincipalEmployerMobileNo)
                            {
                                _context.Licence_Bocw_AmendmentDataHistories.Remove(savedAmendmentDataHistories.Where(x => x.FieldName == "PrincipalEmployerMobileNo").FirstOrDefault());
                            }
                            else
                            {
                                var amendmentHistory = savedAmendmentDataHistories.Where(x => x.FieldName == "PrincipalEmployerMobileNo").FirstOrDefault();
                                amendmentHistory.ModifiedValue = formModel.PrincipalEmployerMobileNo.ToString();
                                _context.Update<Licence_Bocw_AmendmentDataHistories>(amendmentHistory);
                            }
                            await _context.SaveChangesAsync();
                        }
                        else
                        {
                            if (previousBocwActLicenseData.PrincipalEmployerMobileNo != formModel.PrincipalEmployerMobileNo)
                            {
                                amendmentDataHistories.Add(new Licence_Bocw_AmendmentDataHistories()
                                {
                                    AppRefId = formModel.AppRefId,
                                    FieldName = "PrincipalEmployerMobileNo",
                                    PreviousValue = previousBocwActLicenseData.PrincipalEmployerMobileNo.ToString(),
                                    ModifiedValue = formModel.PrincipalEmployerMobileNo.ToString(),
                                    SectionCode = "PED",  //PrincipalEmployerDetail
                                    ModifiedOn = DateTime.Now,
                                    ModifiedCounter = formModel.ModifiedCounter
                                });
                            }
                        }

                        if (savedAmendmentDataHistories.Any(x => x.FieldName == "PrincipalEmployerAddress"))
                        {
                            if (savedAmendmentDataHistories.Where(x => x.FieldName == "PrincipalEmployerAddress").Select(x => x.PreviousValue).FirstOrDefault() == formModel.PrincipalEmployerAddress)
                            {
                                _context.Licence_Bocw_AmendmentDataHistories.Remove(savedAmendmentDataHistories.Where(x => x.FieldName == "PrincipalEmployerAddress").FirstOrDefault());
                            }
                            else
                            {
                                var amendmentHistory = savedAmendmentDataHistories.Where(x => x.FieldName == "PrincipalEmployerAddress").FirstOrDefault();
                                amendmentHistory.ModifiedValue = formModel.PrincipalEmployerMobileNo.ToString();
                                _context.Update<Licence_Bocw_AmendmentDataHistories>(amendmentHistory);
                            }
                            await _context.SaveChangesAsync();
                        }
                        else
                        {
                            if (previousBocwActLicenseData.PrincipalEmployerAddress != formModel.PrincipalEmployerAddress)
                            {
                                amendmentDataHistories.Add(new Licence_Bocw_AmendmentDataHistories()
                                {
                                    AppRefId = formModel.AppRefId,
                                    FieldName = "PrincipalEmployerAddress",
                                    PreviousValue = previousBocwActLicenseData.PrincipalEmployerAddress.ToString(),
                                    ModifiedValue = formModel.PrincipalEmployerAddress.ToString(),
                                    SectionCode = "PED",  //PrincipalEmployerDetail
                                    ModifiedOn = DateTime.Now,
                                    ModifiedCounter = formModel.ModifiedCounter
                                });
                            }
                        }

                        if (savedAmendmentDataHistories.Any(x => x.FieldName == "No_of_Worker"))
                        {
                            if (Convert.ToInt64(savedAmendmentDataHistories.Where(x => x.FieldName == "No_of_Worker").Select(x => x.PreviousValue).FirstOrDefault()) == formModel.MaximumNoOfWorkers)
                            {
                                _context.Licence_Bocw_AmendmentDataHistories.Remove(savedAmendmentDataHistories.Where(x => x.FieldName == "No_of_Worker").FirstOrDefault());
                            }
                            else
                            {
                                var amendmentHistory = savedAmendmentDataHistories.Where(x => x.FieldName == "No_of_Worker").FirstOrDefault();
                                amendmentHistory.ModifiedValue = formModel.MaximumNoOfWorkers.ToString();
                                _context.Update<Licence_Bocw_AmendmentDataHistories>(amendmentHistory);
                            }
                            await _context.SaveChangesAsync();
                        }
                        else
                        {
                            if (previousBocwActLicenseData.MaximumNoOfWorkers != formModel.MaximumNoOfWorkers)
                            {
                                amendmentDataHistories.Add(new Licence_Bocw_AmendmentDataHistories()
                                {
                                    AppRefId = formModel.AppRefId,
                                    FieldName = "No_of_Worker",
                                    PreviousValue = previousBocwActLicenseData.MaximumNoOfWorkers.ToString(),
                                    ModifiedValue = formModel.MaximumNoOfWorkers.ToString(),
                                    SectionCode = "WorkerNo", //WorkerDetail
                                    ModifiedOn = DateTime.Now,
                                    ModifiedCounter = formModel.ModifiedCounter
                                });
                            }
                        }

                        if (amendmentDataHistories.Count() > 0)
                        {
                            await _context.BulkInsertAsync<Licence_Bocw_AmendmentDataHistories>(amendmentDataHistories);
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


        public async Task<GenericFormModel<BocwContractorDetailViewModel>> GetBocwContractorCommanDetail(Int64 id, Int64 projectSiteId)
        {
            GenericFormModel<BocwContractorDetailViewModel> genericFormModel = new GenericFormModel<BocwContractorDetailViewModel>();
            try
            {
                genericFormModel.ListTemplateLists = new List<ListTemplate>();

                var parentWithChildObject = await _iGR_BocwLicence_GeneralDetail
                    .GetAsync(x => x.AppRefId == id,  //Conditions         
                        null,                //Orders
                       x => x.Application, x => x.Application.ApplicationAction)      //Includes
                    .ConfigureAwait(false);

                genericFormModel.FormModel = new BocwContractorDetailViewModel();
                genericFormModel.FormModel.BocwContractorList = new List<Licence_BocwAct_ContractorDetail>();
                genericFormModel.FormModel.EngagedAnyContractorType = parentWithChildObject.FirstOrDefault().EngagedAnyContractorType;
                genericFormModel.FormModel.BocwEngagedWorkerSlabType = parentWithChildObject.FirstOrDefault().BocwEngagedWorkerSlabType;
                genericFormModel.FormModel.Work_CommencementDate = parentWithChildObject.FirstOrDefault().Work_CommencementDate;
                genericFormModel.FormModel.Work_CompletionDate = parentWithChildObject.FirstOrDefault().Work_CompletionDate;
                genericFormModel.FormModel.EngagedAnyContractorType = parentWithChildObject.FirstOrDefault().EngagedAnyContractorType;
                genericFormModel.FormModel.Totalworker = parentWithChildObject.FirstOrDefault().MaximumNoOfWorkers;

                genericFormModel.IsEditAllowed = parentWithChildObject.FirstOrDefault().Application.IsAllowEdit;
                genericFormModel.IsLocked = parentWithChildObject.FirstOrDefault().Application.IsLocked;

                genericFormModel.AppFormStepsList = new List<AppFormStepsInfo>();
                genericFormModel.AppFormStepsList = await _iApplicationMamnagementService.GetAppFormStepperInfo(id, ApplicationTypeEnum.BOCW_ESTABLISHMENT_ACT, (genericFormModel.FormModel != null ? parentWithChildObject.FirstOrDefault().BocwEstablishmentRegistrationId : id), "CD");
            }
            catch (Exception ex)
            {
                genericFormModel.HasError = true;
                genericFormModel.ErrorDesc = ex.Message;
            }
            return genericFormModel;
        }


        public async Task<GenericFormModel<List<Licence_BocwAct_ContractorDetail>>> GetBocwLicenceContractorList(Int64 id)
        {
            GenericFormModel<List<Licence_BocwAct_ContractorDetail>> genericFormModel = new GenericFormModel<List<Licence_BocwAct_ContractorDetail>>();
            try
            {

                var bocwid = _context.Licence_BocwAct_GeneralDetails.Where(x => x.AppRefId == id).FirstOrDefault();
                var emps = _context.Licence_BocwAct_ContractorDetails.Where(x => x.BocwEstablishmentRegistrationRefId == bocwid.BocwEstablishmentRegistrationId).ToList();
                genericFormModel.FormModel = new List<Licence_BocwAct_ContractorDetail>();
                genericFormModel.FormModel = emps;
            }
            catch (Exception ex)
            {
                genericFormModel.HasError = true;
                genericFormModel.ErrorDesc = ex.Message;
            }
            return genericFormModel;
        }

        public async Task<GenericServiceResultTemplate> AddUpdateBocwContractorDetail(BocwContractorDetailViewModel formModel)
        {
            GenericServiceResultTemplate genericServiceResultTemplate = new GenericServiceResultTemplate();
            try
            {
                genericServiceResultTemplate.HasException = false;
                genericServiceResultTemplate.CustomeValidationResult = CustomeValidator<BocwContractorDetailViewModel>.ValidateModel_AllProperties(formModel);
                var contractexist = await _iGR_BocwLicence_ContractorDetail.GetAsync(x => x.BocwEstablishmentRegistrationRefId == formModel.BocwEstablishmentRegistrationId).ConfigureAwait(false);
                if (contractexist.Count() > 0) /*--Existing Rocord*/
                {
                    var bocwData = await _iGR_BocwLicence_GeneralDetail.GetAsync(x => x.BocwEstablishmentRegistrationId == formModel.BocwEstablishmentRegistrationId).ConfigureAwait(false);
                    bocwData.FirstOrDefault().EngagedAnyContractorType = formModel.EngagedAnyContractorType;
                    bocwData.FirstOrDefault().MaximumNoOfWorkers = formModel.Totalworker;
                    _iGR_BocwLicence_GeneralDetail.Update(bocwData.FirstOrDefault());
                    await _iGR_BocwLicence_GeneralDetail.SavechangeAsync();

                    if (contractexist.Any())
                    {
                        await _context.BulkDeleteAsync(contractexist.ToList());
                    }

                    await _context.BulkInsertAsync<Licence_BocwAct_ContractorDetail>(formModel.BocwContractorList.ToList());

                    //Update last modified date
                    await _iApplicationMamnagementService.UpdateAppLastModifiedDate(formModel.AppRefId);
                    genericServiceResultTemplate.ApplicationInitiateResponse = new ApplicationInitiateResponseViewModel();
                    genericServiceResultTemplate.ApplicationInitiateResponse.EntityKeyId = formModel.BocwEstablishmentRegistrationId;
                    genericServiceResultTemplate.ApplicationInitiateResponse.AppId = formModel.AppRefId;
                }
                else /*--new Rocord*/
                {
                    var bocwData = await _iGR_BocwLicence_GeneralDetail.GetAsync(x => x.BocwEstablishmentRegistrationId == formModel.BocwEstablishmentRegistrationId).ConfigureAwait(false);
                    bocwData.FirstOrDefault().EngagedAnyContractorType = formModel.EngagedAnyContractorType;
                    bocwData.FirstOrDefault().MaximumNoOfWorkers = formModel.Totalworker;
                    _iGR_BocwLicence_GeneralDetail.Update(bocwData.FirstOrDefault());
                    await _iGR_BocwLicence_GeneralDetail.SavechangeAsync();
                    await _context.BulkInsertAsync<Licence_BocwAct_ContractorDetail>(formModel.BocwContractorList.ToList());

                    //Update last modified date
                    await _iApplicationMamnagementService.UpdateAppLastModifiedDate(formModel.AppRefId);
                    genericServiceResultTemplate.ApplicationInitiateResponse = new ApplicationInitiateResponseViewModel();
                    genericServiceResultTemplate.ApplicationInitiateResponse.EntityKeyId = formModel.BocwEstablishmentRegistrationId;
                    genericServiceResultTemplate.ApplicationInitiateResponse.AppId = formModel.AppRefId;
                    var application = _context.Applications.Where(x => x.AppId == formModel.AppRefId && x.IsDeleted == false).FirstOrDefault();
                    if (application.ApplicationPurposeType == ApplicationPurposeTypeEnum.AMENDMENT_LICENCE)
                    {
                        List<Licence_Bocw_AmendmentDataHistories> amendmentDataHistories = new List<Licence_Bocw_AmendmentDataHistories>();

                        if (bocwData.FirstOrDefault().MaximumNoOfWorkers != formModel.Totalworker)
                        {
                            amendmentDataHistories.Add(new Licence_Bocw_AmendmentDataHistories()
                            {
                                AppRefId = formModel.AppRefId,
                                FieldName = "No_of_Worker",
                                PreviousValue = bocwData.FirstOrDefault().MaximumNoOfWorkers.ToString(),
                                ModifiedValue = formModel.Totalworker.ToString(),
                                SectionCode = "WorkerNo", //WorkerDetail
                                ModifiedOn = DateTime.Now,
                                ModifiedCounter = 1
                            });
                        }

                        if (amendmentDataHistories.Count() > 0)
                        {
                            await _context.BulkInsertAsync<Licence_Bocw_AmendmentDataHistories>(amendmentDataHistories);
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

        #region BOCW Licence Detail
        public async Task<GenericFormModel<BocwLicenceViewModel>> GetBocwLicenceDetail(long id)
        {
            GenericFormModel<BocwLicenceViewModel> genericFormModel = new GenericFormModel<BocwLicenceViewModel>();
            try
            {
                genericFormModel.ListTemplateLists = new List<ListTemplate>();
                genericFormModel.FormModel = new BocwLicenceViewModel();
                genericFormModel.FormModel.GeneralDetail = new Licence_BocwAct_GeneralDetail();
                genericFormModel.FormModel.OldGeneralDetail = new Licence_BocwAct_GeneralDetail();

                var parentWithChildObject = await _iGR_BocwLicence_GeneralDetail
                       .GetAsync(x => x.AppRefId == id,
                         null,
                         x => x.Application, x => x.Licence_BocwAct_ContractorDetail, x => x.DistrictLgd, x => x.TehsilLgd)
                       .ConfigureAwait(false);

                var nativeAppRefId = _context.Applications.Where(x => x.AppId == id && x.IsDeleted == false).FirstOrDefault().Legacy_AppId;

                var oldparentWithChildObject = await _iGR_BocwLicence_GeneralDetail
                      .GetAsync(x => x.AppRefId == nativeAppRefId,
                        null,
                        x => x.Application, x => x.Licence_BocwAct_ContractorDetail, x => x.DistrictLgd, x => x.TehsilLgd)
                      .ConfigureAwait(false);

                //Initialization of Enum types of lists
                genericFormModel.EnumTemplateLists = new List<EnumListTemplate>();
                genericFormModel.EnumTemplateLists.Add(new EnumListTemplate()
                {
                    SelectListTypeCode = "ConstructionBuildingTypeEnum",
                    SelectListItems = EnumOps.GetEnumAsSelectList<ConstructionBuildingTypeEnum>()
                });

                genericFormModel.EnumTemplateLists.Add(new EnumListTemplate()
                {
                    SelectListTypeCode = "BocwEngagedWorkerSlabTypeEnum",
                    SelectListItems = EnumOps.GetEnumAsSelectList<BocwEngagedWorkerSlabTypeEnum>()
                });

                genericFormModel.EnumTemplateLists.Add(new EnumListTemplate()
                {
                    SelectListTypeCode = "BocwRegisteredTypeEnum",
                    SelectListItems = EnumOps.GetEnumAsSelectList<BocwRegisteredTypeEnum>()
                });

                genericFormModel.EnumTemplateLists.Add(new EnumListTemplate()
                {
                    SelectListTypeCode = "EngagedAnyContractorTypeEnum",
                    SelectListItems = EnumOps.GetEnumAsSelectList<EngagedAnyContractorTypeEnum>()
                });

                genericFormModel.EnumTemplateLists.Add(new EnumListTemplate()
                {
                    SelectListTypeCode = "ApplicationPurposeTypeEnum",
                    SelectListItems = EnumOps.GetEnumAsSelectList<ApplicationPurposeTypeEnum>()
                });

                genericFormModel.EnumTemplateLists.Add(new EnumListTemplate()
                {
                    SelectListTypeCode = "BOCWActCircleTypeEnum",
                    SelectListItems = EnumOps.GetEnumAsSelectList<BOCWActCircleTypeEnum>()
                });


                genericFormModel.FormModel.GeneralDetail = parentWithChildObject.FirstOrDefault();
                genericFormModel.FormModel.OldGeneralDetail = oldparentWithChildObject.FirstOrDefault();
                genericFormModel.FormModel.IsLocked = parentWithChildObject.FirstOrDefault().Application.IsLocked;
                genericFormModel.FormModel.IsFeeApplicable = parentWithChildObject.FirstOrDefault().Application.IsFeeApplicable;
                genericFormModel.ApplicationLifeCycleStatusType = parentWithChildObject.FirstOrDefault().Application.ApplicationLifeCycleStatusType;

                genericFormModel.AppFormStepsList = new List<AppFormStepsInfo>();
                genericFormModel.AppFormStepsList = await _iApplicationMamnagementService.GetAppFormStepperInfo(genericFormModel.FormModel.GeneralDetail.AppRefId, ApplicationTypeEnum.BOCW_ESTABLISHMENT_ACT, (genericFormModel.FormModel != null ? genericFormModel.FormModel.GeneralDetail.BocwEstablishmentRegistrationId : id), "LOCK");


                genericFormModel.FormModel.ConstructionBuildingTypeDesc = EnumOps.GetEnumDescriptionName<ConstructionBuildingTypeEnum>(Enum.GetName(typeof(ConstructionBuildingTypeEnum), parentWithChildObject.FirstOrDefault().ConstructionBuildingType));
                genericFormModel.FormModel.BocwEngagedWorkerSlabType = EnumOps.GetEnumDescriptionName<BocwEngagedWorkerSlabTypeEnum>(Enum.GetName(typeof(BocwEngagedWorkerSlabTypeEnum), parentWithChildObject.FirstOrDefault().BocwEngagedWorkerSlabType));
                genericFormModel.FormModel.BocwRegisteredType = EnumOps.GetEnumDescriptionName<BocwRegisteredTypeEnum>(Enum.GetName(typeof(BocwRegisteredTypeEnum), parentWithChildObject.FirstOrDefault().BocwRegisteredType));
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

                    if (await _iApplicationMamnagementService.LockApplication(appRefId, AppActionType, remarks, ApplicationTypeEnum.BOCW_ESTABLISHMENT_ACT) != "")
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

        public async Task<GenericResponseTemplateModel<bool>> RemoveContractor(Int64 id, int totalWorkers)
        {
            GenericResponseTemplateModel<bool> genericFormModel = new GenericResponseTemplateModel<bool>();
            try
            {
                var emp = _context.Licence_BocwAct_ContractorDetails.Where(x => x.BocwEstablishmentContractorDetailId == id).FirstOrDefault();
                _context.Licence_BocwAct_ContractorDetails.RemoveRange(emp);
                _context.SaveChanges();

                var bocwGeneralData = await _iGR_BocwLicence_GeneralDetail.GetAsync(x => x.BocwEstablishmentRegistrationId == emp.BocwEstablishmentRegistrationRefId).ConfigureAwait(false);
                bocwGeneralData.FirstOrDefault().MaximumNoOfWorkers = (bocwGeneralData.FirstOrDefault().MaximumNoOfWorkers - totalWorkers);
                _iGR_BocwLicence_GeneralDetail.Update(bocwGeneralData.FirstOrDefault());
                await _iGR_BocwLicence_GeneralDetail.SavechangeAsync();


            }
            catch (Exception ex)
            {
                genericFormModel.HasError = true;
                genericFormModel.ErrorDesc = ex.Message;
                throw ex;
            }
            return genericFormModel;
        }
        #endregion Lock Application
    }
}
