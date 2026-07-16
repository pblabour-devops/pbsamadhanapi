using pbsamadhannetcoreapi.CommonUtiliteis;
using pbsamadhannetcoreapi.Models;
using pbsamadhannetcoreapi.Repositories.Implementations;
using pbsamadhannetcoreapi.Services.Implementations;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;
using EFCore.BulkExtensions;
using pbsamadhannetcoreapi.ViewModels;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace pbsamadhannetcoreapi.Services
{
    public class Licence_MotorTransportService : ILicence_MotorTransportService
    {
        private readonly IGenericRepository<Licence_MotorTransport> _iGR_Licence_MotorTransport;
        private readonly IApplicationManagementService<Licence_MotorTransport> _iApplicationMamnagementService;
        private readonly IThirdPartyInegrationsService _iThirdPartyInegrationsService;
        private readonly AppDbContext _context;
        private readonly IGeneric_SP_Repository _iGeneric_SP_Repository;
        private readonly IProjectSiteService _iProjectSiteService;
        private IAuthService _iAuthService;
        private readonly IGenericRepository<DistrictLgd> _iGR_DistrictLgd;
        private readonly IGenericRepository<Licence_Motor_Transport_AmendmentDataHistories> _iGR_Licence_Motor_Transport_AmendmentHistories;
        public Licence_MotorTransportService(IGenericRepository<Licence_MotorTransport> iGR_Licence_MotorTransport,
             IApplicationManagementService<Licence_MotorTransport> IApplicationMamnagementService,
             IThirdPartyInegrationsService iThirdPartyInegrationsService,
             AppDbContext context,
             IGeneric_SP_Repository iGeneric_SP_Repository,
             IAuthService authService,
             IProjectSiteService iProjectSiteService,
             IGenericRepository<DistrictLgd> iGR_DistrictLgd,
             IGenericRepository<Licence_Motor_Transport_AmendmentDataHistories> iGR_Licence_Motor_Transport_AmendmentHistories)
        {
            _iGR_Licence_MotorTransport = iGR_Licence_MotorTransport;
            _iApplicationMamnagementService = IApplicationMamnagementService;
            _iThirdPartyInegrationsService = iThirdPartyInegrationsService;
            _context = context;
            _iGeneric_SP_Repository = iGeneric_SP_Repository;
            _iProjectSiteService = iProjectSiteService;
            _iAuthService = authService;
            _iGR_DistrictLgd = iGR_DistrictLgd;
            _iGR_Licence_Motor_Transport_AmendmentHistories = iGR_Licence_Motor_Transport_AmendmentHistories;
        }

        public async Task<GenericFormModel<Licence_MotorTransport>> GetMotorTransportLicenceGeneralDetail(long id, long projectSiteId, Int64 identity)
        {
            GenericFormModel<Licence_MotorTransport> genericFormModel = new GenericFormModel<Licence_MotorTransport>();
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
                    var parentWithChildObject = await _iGR_Licence_MotorTransport
                       .GetAsync(x => x.AppRefId == id,  //Conditions         
                         null,                //Orders
                         x => x.Application, x => x.Application.ApplicationAction)      //Includes
                       .ConfigureAwait(false);

                    genericFormModel.FormModel = parentWithChildObject.FirstOrDefault();
                    genericFormModel.IsEditAllowed = identity == 0 ? true :genericFormModel.FormModel.Application.IsAllowEdit;
                    genericFormModel.IsLocked = identity == 0 ? false : genericFormModel.FormModel.Application.IsLocked;
                    genericFormModel.ApplicationLifeCycleStatusType = genericFormModel.FormModel.Application.ApplicationLifeCycleStatusType;

                    var siteId = await _iProjectSiteService.GetProjectSiteRefIdByAppId(id);
                    var projectProfileInfo = await _iProjectSiteService.GetProjectSiteDetail(siteId.ResponseDataModel);
                    genericFormModel.FormModel.AlcCircleRefId = projectProfileInfo.FormModel.AlcCircleRefId;
                }

                else //New Record
                {
                    genericFormModel.FormModel = new Licence_MotorTransport();
                    genericFormModel.IsEditAllowed = true;
                    genericFormModel.IsLocked = false;
                    genericFormModel.ApplicationLifeCycleStatusType = ApplicationLifeCycleStatusTypeEnum.NOT_SUBMITTED;
                }
                genericFormModel.EnumTemplateLists = new List<EnumListTemplate>();
                genericFormModel.EnumTemplateLists.Add(new EnumListTemplate()
                {
                    SelectListTypeCode = "MotorTransportNameAddressTypeEnum",
                    SelectListItems = EnumOps.GetEnumAsSelectList<MotorTransportNameAddressTypeEnum>()

                });


                genericFormModel.AppFormStepsList = new List<AppFormStepsInfo>();
                genericFormModel.AppFormStepsList = await _iApplicationMamnagementService.GetAppFormStepperInfo(id, ApplicationTypeEnum.MOTOR_TRANSPORT, (genericFormModel.FormModel != null ? genericFormModel.FormModel.Id : id), "GD");
            }
            catch (Exception ex)
            {
                genericFormModel.HasError = true;
                genericFormModel.ErrorDesc = ex.Message;
            }
            return genericFormModel;

        }

        public async Task<GenericServiceResultTemplate> AddUpdate_GeneralDetail(Licence_MotorTransport formModel, string id)
        {
            GenericServiceResultTemplate genericServiceResultTemplate = new GenericServiceResultTemplate();
            try
            {
                genericServiceResultTemplate.HasException = false;
                genericServiceResultTemplate.CustomeValidationResult = CustomeValidator<Licence_MotorTransport>.ValidateModel_AllProperties(formModel);
                genericServiceResultTemplate.ApplicationInitiateResponse = new ApplicationInitiateResponseViewModel();
                genericServiceResultTemplate.ApplicationInitiateResponse.EntityKeyId = formModel.Id;
                genericServiceResultTemplate.ApplicationInitiateResponse.AppId = formModel.AppRefId;
                if (genericServiceResultTemplate.CustomeValidationResult.IsValid)
                {
                    Licence_MotorTransport previousMTLicenseData = null;
                    List<Licence_Motor_Transport_AmendmentDataHistories> savedAmendmentDataHistories = null;
                    {
                        if (formModel.Id != 0) //Existing record
                        {
                            previousMTLicenseData = _context.Licence_MotorTransport.Where(x => x.Id == formModel.Id).AsNoTracking().FirstOrDefault();
                            savedAmendmentDataHistories = await _context.Licence_Motor_Transport_AmendmentDataHistories.Where(x =>
                                !x.IsLocked && x.AppRefId == previousMTLicenseData.AppRefId).ToListAsync();
                            //Update last modified date
                            _iGR_Licence_MotorTransport.Update(formModel);
                            await _iGR_Licence_MotorTransport.SavechangeAsync();
                            await _iApplicationMamnagementService.UpdateAppLastModifiedDate(formModel.AppRefId);

                            // Update Circle
                            var projectSiteDetail = await _iApplicationMamnagementService.GetProjectSiteByAppRefId(genericServiceResultTemplate.ApplicationInitiateResponse.AppId);
                            await _iProjectSiteService.SetProjectSiteCircle(projectSiteDetail.FormModel.ProjectSiteId, 0, 0,formModel.AlcCircleRefId);

                        }
                        else //New record
                        {
                            previousMTLicenseData = _context.Licence_MotorTransport.Where(x => x.AppRefId == formModel.OldAppRefId).AsNoTracking().FirstOrDefault();
                            if(previousMTLicenseData != null)
                            {
                                savedAmendmentDataHistories = await _context.Licence_Motor_Transport_AmendmentDataHistories.Where(x =>
                             !x.IsLocked && x.AppRefId == previousMTLicenseData.AppRefId).ToListAsync();
                            }
                            
                           
                            formModel = GenericModelOps<Licence_MotorTransport>.SetNullAllNevigationProperties(formModel);
                            genericServiceResultTemplate.ApplicationInitiateResponse = await _iApplicationMamnagementService.InitiateApplication(ApplicationTypeEnum.MOTOR_TRANSPORT, formModel, formModel.ProjectSiteRefId, formModel.ApplicationPurposeType, id, formModel.IPin, formModel.InvestPunjab_AppId, false, 0, 0, null, null, formModel.ProjectSiteVersion);
                            if (genericServiceResultTemplate.ApplicationInitiateResponse.IsApplicationCreated == true)
                            {

                                var projectSiteDetail = await _iApplicationMamnagementService.GetProjectSiteByAppRefId(genericServiceResultTemplate.ApplicationInitiateResponse.AppId);
                                await _iProjectSiteService.SetProjectSiteCircle(projectSiteDetail.FormModel.ProjectSiteId, 0, 0,formModel.AlcCircleRefId);
                                //await _iThirdPartyInegrationsService.ShareStatusToBusinessFirst(genericServiceResultTemplate.ApplicationInitiateResponse.AppId, ApplicationTypeEnum.PRINCIPAL_EMPLOYER, AppActionTypeEnum.APP_SAVE_DRAFT);
                            }
                            else if (genericServiceResultTemplate.ApplicationInitiateResponse.IsApplicationCreated == false)
                            {
                                genericServiceResultTemplate.HasException = true;
                                genericServiceResultTemplate.Exceptions = new Exception();
                            }


                        }
                        
                        if (formModel.ApplicationPurposeType == ApplicationPurposeTypeEnum.RENEWAL_LICENCE || formModel.ApplicationPurposeType == ApplicationPurposeTypeEnum.AMENDMENT_LICENCE && previousMTLicenseData != null)
                        {
                            List<Licence_Motor_Transport_AmendmentDataHistories> amendmentDataHistories = new List<Licence_Motor_Transport_AmendmentDataHistories>();

                            if (savedAmendmentDataHistories.Any(x => x.FieldName == "LicenceForYear"))
                            {
                                if (savedAmendmentDataHistories.Where(x => x.FieldName == "LicenceForYear").Select(x => x.PreviousValue).FirstOrDefault() == formModel.LicenceForYear.ToString())
                                {
                                    _context.Licence_Motor_Transport_AmendmentDataHistories.Remove(savedAmendmentDataHistories.Where(x => x.FieldName == "LicenceForYear").FirstOrDefault());
                                }
                                else
                                {
                                    var amendmentHistory = savedAmendmentDataHistories.Where(x => x.FieldName == "LicenceForYear").FirstOrDefault();
                                    amendmentHistory.ModifiedValue = formModel.LicenceForYear.ToString();
                                    _context.Update<Licence_Motor_Transport_AmendmentDataHistories>(amendmentHistory);
                                }
                                await _context.SaveChangesAsync();
                            }
                            else
                            {
                                if (previousMTLicenseData.LicenceForYear != formModel.LicenceForYear)
                                {
                                    amendmentDataHistories.Add(new Licence_Motor_Transport_AmendmentDataHistories()
                                    {
                                        AppRefId = formModel.AppRefId,
                                        FieldName = "LicenceForYear",
                                        PreviousValue = previousMTLicenseData.LicenceForYear.ToString(),
                                        ModifiedValue = formModel.LicenceForYear.ToString(),
                                        SectionCode = "GD", //ConstructionBuildingDetail
                                        ModifiedOn = DateTime.Now,
                                        ModifiedCounter = formModel.ModifiedCounter
                                    });
                                }
                            }

                            if (savedAmendmentDataHistories.Any(x => x.FieldName == "TransportUndertakingName"))
                            {
                                if (savedAmendmentDataHistories.Where(x => x.FieldName == "TransportUndertakingName").Select(x => x.PreviousValue).FirstOrDefault() == formModel.TransportUndertakingName)
                                {
                                    _context.Licence_Motor_Transport_AmendmentDataHistories.Remove(savedAmendmentDataHistories.Where(x => x.FieldName == "TransportUndertakingName").FirstOrDefault());
                                }
                                else
                                {
                                    var amendmentHistory = savedAmendmentDataHistories.Where(x => x.FieldName == "TransportUndertakingName").FirstOrDefault();
                                    amendmentHistory.ModifiedValue = formModel.TransportUndertakingName.ToString();
                                    _context.Update<Licence_Motor_Transport_AmendmentDataHistories>(amendmentHistory);
                                }
                                await _context.SaveChangesAsync();
                            }
                            else
                            {
                                if (previousMTLicenseData.TransportUndertakingName != formModel.TransportUndertakingName)
                                {
                                    amendmentDataHistories.Add(new Licence_Motor_Transport_AmendmentDataHistories()
                                    {
                                        AppRefId = formModel.AppRefId,
                                        FieldName = "TransportUndertakingName",
                                        PreviousValue = previousMTLicenseData.TransportUndertakingName.ToString(),
                                        ModifiedValue = formModel.TransportUndertakingName.ToString(),
                                        SectionCode = "GD", //ConstructionBuildingDetail
                                        ModifiedOn = DateTime.Now,
                                        ModifiedCounter = formModel.ModifiedCounter
                                    });
                                }
                            }

                            if (savedAmendmentDataHistories.Any(x => x.FieldName == "CommunicationAddress"))
                            {
                                if (savedAmendmentDataHistories.Where(x => x.FieldName == "CommunicationAddress").Select(x => x.PreviousValue).FirstOrDefault() == formModel.CommunicationAddress)
                                {
                                    _context.Licence_Motor_Transport_AmendmentDataHistories.Remove(savedAmendmentDataHistories.Where(x => x.FieldName == "CommunicationAddress").FirstOrDefault());
                                }
                                else
                                {
                                    var amendmentHistory = savedAmendmentDataHistories.Where(x => x.FieldName == "CommunicationAddress").FirstOrDefault();
                                    amendmentHistory.ModifiedValue = formModel.CommunicationAddress.ToString();
                                    _context.Update<Licence_Motor_Transport_AmendmentDataHistories>(amendmentHistory);
                                }
                                await _context.SaveChangesAsync();
                            }
                            else
                            {
                                if (previousMTLicenseData.CommunicationAddress != formModel.CommunicationAddress)
                                {
                                    amendmentDataHistories.Add(new Licence_Motor_Transport_AmendmentDataHistories()
                                    {
                                        AppRefId = formModel.AppRefId,
                                        FieldName = "CommunicationAddress",
                                        PreviousValue = previousMTLicenseData.CommunicationAddress.ToString(),
                                        ModifiedValue = formModel.CommunicationAddress.ToString(),
                                        SectionCode = "GD", //ConstructionSiteDetail
                                        ModifiedOn = DateTime.Now,
                                        ModifiedCounter = formModel.ModifiedCounter
                                    });
                                }
                            }

                            if (savedAmendmentDataHistories.Any(x => x.FieldName == "CommunicationAddress_PinCode"))
                            {
                                if (savedAmendmentDataHistories.Where(x => x.FieldName == "CommunicationAddress_PinCode").Select(x => x.PreviousValue).FirstOrDefault() == formModel.CommunicationAddress_PinCode)
                                {
                                    _context.Licence_Motor_Transport_AmendmentDataHistories.Remove(savedAmendmentDataHistories.Where(x => x.FieldName == "CommunicationAddress_PinCode").FirstOrDefault());
                                }
                                else
                                {
                                    var amendmentHistory = savedAmendmentDataHistories.Where(x => x.FieldName == "CommunicationAddress_PinCode").FirstOrDefault();
                                    amendmentHistory.ModifiedValue = formModel.CommunicationAddress_PinCode.ToString();
                                    _context.Update<Licence_Motor_Transport_AmendmentDataHistories>(amendmentHistory);
                                }
                                await _context.SaveChangesAsync();
                            }
                            else
                            {
                                if (previousMTLicenseData.CommunicationAddress_PinCode != formModel.CommunicationAddress_PinCode)
                                {
                                    amendmentDataHistories.Add(new Licence_Motor_Transport_AmendmentDataHistories()
                                    {
                                        AppRefId = formModel.AppRefId,
                                        FieldName = "CommunicationAddress_PinCode",
                                        PreviousValue = previousMTLicenseData.CommunicationAddress_PinCode.ToString(),
                                        ModifiedValue = formModel.CommunicationAddress_PinCode.ToString(),
                                        SectionCode = "GD", //ConstructionSiteDetail
                                        ModifiedOn = DateTime.Now,
                                        ModifiedCounter = formModel.ModifiedCounter
                                    });
                                }
                            }

                            if (savedAmendmentDataHistories.Any(x => x.FieldName == "TransportServiceName"))
                            {
                                if (savedAmendmentDataHistories.Where(x => x.FieldName == "TransportServiceName").Select(x => x.PreviousValue).FirstOrDefault() == formModel.TransportServiceName)
                                {
                                    _context.Licence_Motor_Transport_AmendmentDataHistories.Remove(savedAmendmentDataHistories.Where(x => x.FieldName == "TransportServiceName").FirstOrDefault());
                                }
                                else
                                {
                                    var amendmentHistory = savedAmendmentDataHistories.Where(x => x.FieldName == "TransportServiceName").FirstOrDefault();
                                    amendmentHistory.ModifiedValue = formModel.TransportServiceName.ToString();
                                    _context.Update<Licence_Motor_Transport_AmendmentDataHistories>(amendmentHistory);
                                }
                                await _context.SaveChangesAsync();
                            }
                            else
                            {
                                if (previousMTLicenseData.TransportServiceName != formModel.TransportServiceName)
                                {
                                    amendmentDataHistories.Add(new Licence_Motor_Transport_AmendmentDataHistories()
                                    {
                                        AppRefId = formModel.AppRefId,
                                        FieldName = "TransportServiceName",
                                        PreviousValue = previousMTLicenseData.TransportServiceName.ToString(),
                                        ModifiedValue = formModel.TransportServiceName.ToString(),
                                        SectionCode = "VD", //ConstructionSiteDetail
                                        ModifiedOn = DateTime.Now,
                                        ModifiedCounter = formModel.ModifiedCounter
                                    });
                                }
                            }

                            if (savedAmendmentDataHistories.Any(x => x.FieldName == "TotalRoutes"))
                            {
                                if (savedAmendmentDataHistories.Where(x => x.FieldName == "TotalRoutes").Select(x => x.PreviousValue).FirstOrDefault() == formModel.TotalRoutes.ToString())
                                {
                                    _context.Licence_Motor_Transport_AmendmentDataHistories.Remove(savedAmendmentDataHistories.Where(x => x.FieldName == "TotalRoutes").FirstOrDefault());
                                }
                                else
                                {
                                    var amendmentHistory = savedAmendmentDataHistories.Where(x => x.FieldName == "TotalRoutes").FirstOrDefault();
                                    amendmentHistory.ModifiedValue = formModel.TotalRoutes.ToString();
                                    _context.Update<Licence_Motor_Transport_AmendmentDataHistories>(amendmentHistory);
                                }
                                await _context.SaveChangesAsync();
                            }
                            else
                            {
                                if (previousMTLicenseData.TotalRoutes != formModel.TotalRoutes)
                                {
                                    amendmentDataHistories.Add(new Licence_Motor_Transport_AmendmentDataHistories()
                                    {
                                        AppRefId = formModel.AppRefId,
                                        FieldName = "TotalRoutes",
                                        PreviousValue = previousMTLicenseData.TotalRoutes.ToString(),
                                        ModifiedValue = formModel.TotalRoutes.ToString(),
                                        SectionCode = "VD", //ContractLabourDetail
                                        ModifiedOn = DateTime.Now,
                                        ModifiedCounter = formModel.ModifiedCounter
                                    });
                                }
                            }

                            if (savedAmendmentDataHistories.Any(x => x.FieldName == "TotalRouteMileage"))
                            {
                                if (savedAmendmentDataHistories.Where(x => x.FieldName == "TotalRouteMileage").Select(x => x.PreviousValue).FirstOrDefault() == formModel.TotalRouteMileage.ToString())
                                {
                                    _context.Licence_Motor_Transport_AmendmentDataHistories.Remove(savedAmendmentDataHistories.Where(x => x.FieldName == "TotalRouteMileage").FirstOrDefault());
                                }
                                else
                                {
                                    var amendmentHistory = savedAmendmentDataHistories.Where(x => x.FieldName == "TotalRouteMileage").FirstOrDefault();
                                    amendmentHistory.ModifiedValue = formModel.TotalRouteMileage.ToString();
                                    _context.Update<Licence_Motor_Transport_AmendmentDataHistories>(amendmentHistory);
                                }
                                await _context.SaveChangesAsync();
                            }
                            else
                            {
                                if (previousMTLicenseData.TotalRouteMileage != formModel.TotalRouteMileage)
                                {
                                    amendmentDataHistories.Add(new Licence_Motor_Transport_AmendmentDataHistories()
                                    {
                                        AppRefId = formModel.AppRefId,
                                        FieldName = "TotalRouteMileage",
                                        PreviousValue = previousMTLicenseData.TotalRouteMileage.ToString(),
                                        ModifiedValue = formModel.TotalRouteMileage.ToString(),
                                        SectionCode = "VD", //ManagerDetail
                                        ModifiedOn = DateTime.Now,
                                        ModifiedCounter = formModel.ModifiedCounter
                                    });
                                }
                            }


                            if (savedAmendmentDataHistories.Any(x => x.FieldName == "TotalVehicles"))
                            {
                                if (savedAmendmentDataHistories.Where(x => x.FieldName == "TotalVehicles").Select(x => x.PreviousValue).FirstOrDefault() == formModel.TotalVehicles.ToString())
                                {
                                    _context.Licence_Motor_Transport_AmendmentDataHistories.Remove(savedAmendmentDataHistories.Where(x => x.FieldName == "TotalVehicles").FirstOrDefault());
                                }
                                else
                                {
                                    var amendmentHistory = savedAmendmentDataHistories.Where(x => x.FieldName == "TotalVehicles").FirstOrDefault();
                                    amendmentHistory.ModifiedValue = formModel.TotalVehicles.ToString();
                                    _context.Update<Licence_Motor_Transport_AmendmentDataHistories>(amendmentHistory);
                                }
                                await _context.SaveChangesAsync();
                            }
                            else
                            {
                                if (previousMTLicenseData.TotalVehicles != formModel.TotalVehicles)
                                {
                                    amendmentDataHistories.Add(new Licence_Motor_Transport_AmendmentDataHistories()
                                    {
                                        AppRefId = formModel.AppRefId,
                                        FieldName = "TotalVehicles",
                                        PreviousValue = previousMTLicenseData.TotalVehicles.ToString(),
                                        ModifiedValue = formModel.TotalVehicles.ToString(),
                                        SectionCode = "VD", //ManagerDetail
                                        ModifiedOn = DateTime.Now,
                                        ModifiedCounter = formModel.ModifiedCounter
                                    });
                                }
                            }

                            if (savedAmendmentDataHistories.Any(x => x.FieldName == "MaxTransportWorkers"))
                            {
                                if (savedAmendmentDataHistories.Where(x => x.FieldName == "MaxTransportWorkers").Select(x => x.PreviousValue).FirstOrDefault() == formModel.MaxTransportWorkers.ToString())
                                {
                                    _context.Licence_Motor_Transport_AmendmentDataHistories.Remove(savedAmendmentDataHistories.Where(x => x.FieldName == "MaxTransportWorkers").FirstOrDefault());
                                }
                                else
                                {
                                    var amendmentHistory = savedAmendmentDataHistories.Where(x => x.FieldName == "MaxTransportWorkers").FirstOrDefault();
                                    amendmentHistory.ModifiedValue = formModel.MaxTransportWorkers.ToString();
                                    _context.Update<Licence_Motor_Transport_AmendmentDataHistories>(amendmentHistory);
                                }
                                await _context.SaveChangesAsync();
                            }
                            else
                            {
                                if (previousMTLicenseData.MaxTransportWorkers != formModel.MaxTransportWorkers)
                                {
                                    amendmentDataHistories.Add(new Licence_Motor_Transport_AmendmentDataHistories()
                                    {
                                        AppRefId = formModel.AppRefId,
                                        FieldName = "MaxTransportWorkers",
                                        PreviousValue = previousMTLicenseData.MaxTransportWorkers.ToString(),
                                        ModifiedValue = formModel.MaxTransportWorkers.ToString(),
                                        SectionCode = "GD", //ManagerDetail
                                        ModifiedOn = DateTime.Now,
                                        ModifiedCounter = formModel.ModifiedCounter
                                    });
                                }
                            }

                            if (savedAmendmentDataHistories.Any(x => x.FieldName == "NameAddressType"))
                            {
                                if (savedAmendmentDataHistories.Where(x => x.FieldName == "NameAddressType").Select(x => x.PreviousValue).FirstOrDefault() == formModel.NameAddressType.ToString())
                                {
                                    _context.Licence_Motor_Transport_AmendmentDataHistories.Remove(savedAmendmentDataHistories.Where(x => x.FieldName == "NameAddressType").FirstOrDefault());
                                }
                                else
                                {
                                    var amendmentHistory = savedAmendmentDataHistories.Where(x => x.FieldName == "NameAddressType").FirstOrDefault();
                                    amendmentHistory.ModifiedValue = formModel.NameAddressType.ToString();
                                    _context.Update<Licence_Motor_Transport_AmendmentDataHistories>(amendmentHistory);
                                }
                                await _context.SaveChangesAsync();
                            }
                            else
                            {
                                if (previousMTLicenseData.NameAddressType != formModel.NameAddressType)
                                {
                                    amendmentDataHistories.Add(new Licence_Motor_Transport_AmendmentDataHistories()
                                    {
                                        AppRefId = formModel.AppRefId,
                                        FieldName = "NameAddressType",
                                        PreviousValue = previousMTLicenseData.NameAddressType.ToString(),
                                        ModifiedValue = formModel.NameAddressType.ToString(),
                                        SectionCode = "GD", //ManagerDetail
                                        ModifiedOn = DateTime.Now,
                                        ModifiedCounter = formModel.ModifiedCounter
                                    });
                                }
                            }
                            if (savedAmendmentDataHistories.Any(x => x.FieldName == "NameAddressType_Name"))
                            {
                                if (savedAmendmentDataHistories.Where(x => x.FieldName == "NameAddressType_Name").Select(x => x.PreviousValue).FirstOrDefault() == formModel.NameAddressType_Name)
                                {
                                    _context.Licence_Motor_Transport_AmendmentDataHistories.Remove(savedAmendmentDataHistories.Where(x => x.FieldName == "NameAddressType_Name").FirstOrDefault());
                                }
                                else
                                {
                                    var amendmentHistory = savedAmendmentDataHistories.Where(x => x.FieldName == "NameAddressType_Name").FirstOrDefault();
                                    amendmentHistory.ModifiedValue = formModel.NameAddressType_Name.ToString();
                                    _context.Update<Licence_Motor_Transport_AmendmentDataHistories>(amendmentHistory);
                                }
                                await _context.SaveChangesAsync();
                            }
                            else
                            {
                                if (previousMTLicenseData.NameAddressType_Name != formModel.NameAddressType_Name)
                                {
                                    amendmentDataHistories.Add(new Licence_Motor_Transport_AmendmentDataHistories()
                                    {
                                        AppRefId = formModel.AppRefId,
                                        FieldName = "NameAddressType_Name",
                                        PreviousValue = previousMTLicenseData.NameAddressType_Name.ToString(),
                                        ModifiedValue = formModel.NameAddressType_Name.ToString(),
                                        SectionCode = "GD", //ManagerDetail
                                        ModifiedOn = DateTime.Now,
                                        ModifiedCounter = formModel.ModifiedCounter
                                    });
                                }
                            }

                            if (savedAmendmentDataHistories.Any(x => x.FieldName == "NameAddressType_Email"))
                            {
                                if (savedAmendmentDataHistories.Where(x => x.FieldName == "NameAddressType_Email").Select(x => x.PreviousValue).FirstOrDefault() == formModel.NameAddressType_Email)
                                {
                                    _context.Licence_Motor_Transport_AmendmentDataHistories.Remove(savedAmendmentDataHistories.Where(x => x.FieldName == "NameAddressType_Email").FirstOrDefault());
                                }
                                else
                                {
                                    var amendmentHistory = savedAmendmentDataHistories.Where(x => x.FieldName == "NameAddressType_Email").FirstOrDefault();
                                    amendmentHistory.ModifiedValue = formModel.NameAddressType_Email.ToString();
                                    _context.Update<Licence_Motor_Transport_AmendmentDataHistories>(amendmentHistory);
                                }
                                await _context.SaveChangesAsync();
                            }
                            else
                            {
                                if (previousMTLicenseData.NameAddressType_Email != formModel.NameAddressType_Email)
                                {
                                    amendmentDataHistories.Add(new Licence_Motor_Transport_AmendmentDataHistories()
                                    {
                                        AppRefId = formModel.AppRefId,
                                        FieldName = "NameAddressType_Email",
                                        PreviousValue = previousMTLicenseData.NameAddressType_Email.ToString(),
                                        ModifiedValue = formModel.NameAddressType_Email.ToString(),
                                        SectionCode = "GD", //ManagerDetail
                                        ModifiedOn = DateTime.Now,
                                        ModifiedCounter = formModel.ModifiedCounter
                                    });
                                }
                            }

                            if (savedAmendmentDataHistories.Any(x => x.FieldName == "NameAddressType_Mobile"))
                            {
                                if (savedAmendmentDataHistories.Where(x => x.FieldName == "NameAddressType_Mobile").Select(x => x.PreviousValue).FirstOrDefault() == formModel.NameAddressType_Mobile)
                                {
                                    _context.Licence_Motor_Transport_AmendmentDataHistories.Remove(savedAmendmentDataHistories.Where(x => x.FieldName == "NameAddressType_Mobile").FirstOrDefault());
                                }
                                else
                                {
                                    var amendmentHistory = savedAmendmentDataHistories.Where(x => x.FieldName == "NameAddressType_Mobile").FirstOrDefault();
                                    amendmentHistory.ModifiedValue = formModel.NameAddressType_Mobile.ToString();
                                    _context.Update<Licence_Motor_Transport_AmendmentDataHistories>(amendmentHistory);
                                }
                                await _context.SaveChangesAsync();
                            }
                            else
                            {
                                if (previousMTLicenseData.NameAddressType_Mobile != formModel.NameAddressType_Mobile)
                                {
                                    amendmentDataHistories.Add(new Licence_Motor_Transport_AmendmentDataHistories()
                                    {
                                        AppRefId = formModel.AppRefId,
                                        FieldName = "NameAddressType_Mobile",
                                        PreviousValue = previousMTLicenseData.NameAddressType_Mobile.ToString(),
                                        ModifiedValue = formModel.NameAddressType_Mobile.ToString(),
                                        SectionCode = "GD", //ManagerDetail
                                        ModifiedOn = DateTime.Now,
                                        ModifiedCounter = formModel.ModifiedCounter
                                    });
                                }
                            }

                            if (savedAmendmentDataHistories.Any(x => x.FieldName == "NameAddressType_Address"))
                            {
                                if (savedAmendmentDataHistories.Where(x => x.FieldName == "NameAddressType_Address").Select(x => x.PreviousValue).FirstOrDefault() == formModel.NameAddressType_Address)
                                {
                                    _context.Licence_Motor_Transport_AmendmentDataHistories.Remove(savedAmendmentDataHistories.Where(x => x.FieldName == "NameAddressType_Address").FirstOrDefault());
                                }
                                else
                                {
                                    var amendmentHistory = savedAmendmentDataHistories.Where(x => x.FieldName == "NameAddressType_Address").FirstOrDefault();
                                    amendmentHistory.ModifiedValue = formModel.NameAddressType_Address.ToString();
                                    _context.Update<Licence_Motor_Transport_AmendmentDataHistories>(amendmentHistory);
                                }
                                await _context.SaveChangesAsync();
                            }
                            else
                            {
                                if (previousMTLicenseData.NameAddressType_Address != formModel.NameAddressType_Address)
                                {
                                    amendmentDataHistories.Add(new Licence_Motor_Transport_AmendmentDataHistories()
                                    {
                                        AppRefId = formModel.AppRefId,
                                        FieldName = "NameAddressType_Address",
                                        PreviousValue = previousMTLicenseData.NameAddressType_Address.ToString(),
                                        ModifiedValue = formModel.NameAddressType_Address.ToString(),
                                        SectionCode = "GD", //ManagerDetail
                                        ModifiedOn = DateTime.Now,
                                        ModifiedCounter = formModel.ModifiedCounter
                                    });
                                }
                            }

                            if (savedAmendmentDataHistories.Any(x => x.FieldName == "IsCompanyRegUnderCompaniesAct"))
                            {
                                if (savedAmendmentDataHistories.Where(x => x.FieldName == "IsCompanyRegUnderCompaniesAct").Select(x => x.PreviousValue).FirstOrDefault() == formModel.IsCompanyRegUnderCompaniesAct.ToString())
                                {
                                    _context.Licence_Motor_Transport_AmendmentDataHistories.Remove(savedAmendmentDataHistories.Where(x => x.FieldName == "IsCompanyRegUnderCompaniesAct").FirstOrDefault());
                                }
                                else
                                {
                                    var amendmentHistory = savedAmendmentDataHistories.Where(x => x.FieldName == "IsCompanyRegUnderCompaniesAct").FirstOrDefault();
                                    amendmentHistory.ModifiedValue = formModel.IsCompanyRegUnderCompaniesAct.ToString();
                                    _context.Update<Licence_Motor_Transport_AmendmentDataHistories>(amendmentHistory);
                                }
                                await _context.SaveChangesAsync();
                            }
                            else
                            {
                                if (previousMTLicenseData.IsCompanyRegUnderCompaniesAct != formModel.IsCompanyRegUnderCompaniesAct)
                                {
                                    amendmentDataHistories.Add(new Licence_Motor_Transport_AmendmentDataHistories()
                                    {
                                        AppRefId = formModel.AppRefId,
                                        FieldName = "IsCompanyRegUnderCompaniesAct",
                                        PreviousValue = previousMTLicenseData.IsCompanyRegUnderCompaniesAct.ToString(),
                                        ModifiedValue = formModel.IsCompanyRegUnderCompaniesAct.ToString(),
                                        SectionCode = "GD", //ManagerDetail
                                        ModifiedOn = DateTime.Now,
                                        ModifiedCounter = formModel.ModifiedCounter
                                    });
                                }
                            }

                            if (savedAmendmentDataHistories.Any(x => x.FieldName == "Director_Name"))
                            {
                                if (savedAmendmentDataHistories.Where(x => x.FieldName == "Director_Name").Select(x => x.PreviousValue).FirstOrDefault() == formModel.Director_Name)
                                {
                                    _context.Licence_Motor_Transport_AmendmentDataHistories.Remove(savedAmendmentDataHistories.Where(x => x.FieldName == "Director_Name").FirstOrDefault());
                                }
                                else
                                {
                                    var amendmentHistory = savedAmendmentDataHistories.Where(x => x.FieldName == "Director_Name").FirstOrDefault();
                                    amendmentHistory.ModifiedValue = formModel.Director_Name.ToString();
                                    _context.Update<Licence_Motor_Transport_AmendmentDataHistories>(amendmentHistory);
                                }
                                await _context.SaveChangesAsync();
                            }
                            else
                            {
                                if (previousMTLicenseData.Director_Name != formModel.Director_Name)
                                {
                                    amendmentDataHistories.Add(new Licence_Motor_Transport_AmendmentDataHistories()
                                    {
                                        AppRefId = formModel.AppRefId,
                                        FieldName = "Director_Name",
                                        PreviousValue = previousMTLicenseData.Director_Name.ToString(),
                                        ModifiedValue = formModel.Director_Name.ToString(),
                                        SectionCode = "DD", //ManagerDetail
                                        ModifiedOn = DateTime.Now,
                                        ModifiedCounter = formModel.ModifiedCounter
                                    });
                                }
                            }

                            if (savedAmendmentDataHistories.Any(x => x.FieldName == "Director_Email"))
                            {
                                if (savedAmendmentDataHistories.Where(x => x.FieldName == "Director_Email").Select(x => x.PreviousValue).FirstOrDefault() == formModel.Director_Email)
                                {
                                    _context.Licence_Motor_Transport_AmendmentDataHistories.Remove(savedAmendmentDataHistories.Where(x => x.FieldName == "Director_Email").FirstOrDefault());
                                }
                                else
                                {
                                    var amendmentHistory = savedAmendmentDataHistories.Where(x => x.FieldName == "Director_Email").FirstOrDefault();
                                    amendmentHistory.ModifiedValue = formModel.Director_Email.ToString();
                                    _context.Update<Licence_Motor_Transport_AmendmentDataHistories>(amendmentHistory);
                                }
                                await _context.SaveChangesAsync();
                            }
                            else
                            {
                                if (previousMTLicenseData.Director_Email != formModel.Director_Email)
                                {
                                    amendmentDataHistories.Add(new Licence_Motor_Transport_AmendmentDataHistories()
                                    {
                                        AppRefId = formModel.AppRefId,
                                        FieldName = "Director_Email",
                                        PreviousValue = previousMTLicenseData.Director_Email.ToString(),
                                        ModifiedValue = formModel.Director_Email.ToString(),
                                        SectionCode = "DD", //ManagerDetail
                                        ModifiedOn = DateTime.Now,
                                        ModifiedCounter = formModel.ModifiedCounter
                                    });
                                }
                            }

                            if (savedAmendmentDataHistories.Any(x => x.FieldName == "Director_Mobile"))
                            {
                                if (savedAmendmentDataHistories.Where(x => x.FieldName == "Director_Mobile").Select(x => x.PreviousValue).FirstOrDefault() == formModel.Director_Mobile)
                                {
                                    _context.Licence_Motor_Transport_AmendmentDataHistories.Remove(savedAmendmentDataHistories.Where(x => x.FieldName == "Director_Mobile").FirstOrDefault());
                                }
                                else
                                {
                                    var amendmentHistory = savedAmendmentDataHistories.Where(x => x.FieldName == "Director_Mobile").FirstOrDefault();
                                    amendmentHistory.ModifiedValue = formModel.Director_Mobile.ToString();
                                    _context.Update<Licence_Motor_Transport_AmendmentDataHistories>(amendmentHistory);
                                }
                                await _context.SaveChangesAsync();
                            }
                            else
                            {
                                if (previousMTLicenseData.Director_Mobile != formModel.Director_Mobile)
                                {
                                    amendmentDataHistories.Add(new Licence_Motor_Transport_AmendmentDataHistories()
                                    {
                                        AppRefId = formModel.AppRefId,
                                        FieldName = "Director_Mobile",
                                        PreviousValue = previousMTLicenseData.Director_Mobile.ToString(),
                                        ModifiedValue = formModel.Director_Mobile.ToString(),
                                        SectionCode = "DD", //ManagerDetail
                                        ModifiedOn = DateTime.Now,
                                        ModifiedCounter = formModel.ModifiedCounter
                                    });
                                }
                            }

                            if (savedAmendmentDataHistories.Any(x => x.FieldName == "Director_Address"))
                            {
                                if (savedAmendmentDataHistories.Where(x => x.FieldName == "Director_Address").Select(x => x.PreviousValue).FirstOrDefault() == formModel.Director_Address)
                                {
                                    _context.Licence_Motor_Transport_AmendmentDataHistories.Remove(savedAmendmentDataHistories.Where(x => x.FieldName == "Director_Address").FirstOrDefault());
                                }
                                else
                                {
                                    var amendmentHistory = savedAmendmentDataHistories.Where(x => x.FieldName == "Director_Address").FirstOrDefault();
                                    amendmentHistory.ModifiedValue = formModel.Director_Address.ToString();
                                    _context.Update<Licence_Motor_Transport_AmendmentDataHistories>(amendmentHistory);
                                }
                                await _context.SaveChangesAsync();
                            }
                            else
                            {
                                if (previousMTLicenseData.Director_Address != formModel.Director_Address)
                                {
                                    amendmentDataHistories.Add(new Licence_Motor_Transport_AmendmentDataHistories()
                                    {
                                        AppRefId = formModel.AppRefId,
                                        FieldName = "Director_Address",
                                        PreviousValue = previousMTLicenseData.Director_Address.ToString(),
                                        ModifiedValue = formModel.Director_Address.ToString(),
                                        SectionCode = "DD", //ManagerDetail
                                        ModifiedOn = DateTime.Now,
                                        ModifiedCounter = formModel.ModifiedCounter
                                    });
                                }
                            }


                            if (amendmentDataHistories.Count() > 0)
                            {
                                await _context.BulkInsertAsync<Licence_Motor_Transport_AmendmentDataHistories>(amendmentDataHistories);
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

        public async Task<GenericFormModel<Licence_MotorTransportViewModel>> GetMotorTransportDetail(long id)
        {
            GenericFormModel<Licence_MotorTransportViewModel> genericFormModel = new GenericFormModel<Licence_MotorTransportViewModel>();
            try
            {
                genericFormModel.ListTemplateLists = new List<ListTemplate>();
                genericFormModel.FormModel = new Licence_MotorTransportViewModel();
                genericFormModel.FormModel.GeneralDetail = new Licence_MotorTransport();
                genericFormModel.FormModel.Licence_Motor_Transport_AmendmentDataHistories = new List<Licence_Motor_Transport_AmendmentDataHistories>();
                var parentWithChildObject = await _iGR_Licence_MotorTransport
                       .GetAsync(x => x.AppRefId == id,
                         null,
                         x => x.Application)
                       .ConfigureAwait(false);

                var oldparentWithChildObject = await _iGR_Licence_Motor_Transport_AmendmentHistories
                      .GetAsync(x => x.AppRefId == id,
                        null)
                      .ConfigureAwait(false);

                //Initialization of Enum types of lists
                genericFormModel.EnumTemplateLists = new List<EnumListTemplate>();
                genericFormModel.EnumTemplateLists.Add(new EnumListTemplate()
                {
                    SelectListTypeCode = "MotorTransportNameAddressTypeEnum",
                    SelectListItems = EnumOps.GetEnumAsSelectList<MotorTransportNameAddressTypeEnum>()
                });

                genericFormModel.FormModel.GeneralDetail = parentWithChildObject.FirstOrDefault();
                genericFormModel.FormModel.Licence_Motor_Transport_AmendmentDataHistories = oldparentWithChildObject.ToList();
                genericFormModel.FormModel.IsLocked = parentWithChildObject.FirstOrDefault().Application.IsLocked;
                genericFormModel.FormModel.IsFeeApplicable = parentWithChildObject.FirstOrDefault().Application.IsFeeApplicable;
                genericFormModel.ApplicationLifeCycleStatusType = parentWithChildObject.FirstOrDefault().Application.ApplicationLifeCycleStatusType;
                var districtlist = _context.Districts.Where(x => x.DistrictLgdId == parentWithChildObject.FirstOrDefault().CommunicationAddress_DistrictLgdId).ToList();
                genericFormModel.FormModel.GeneralDetail.DistrictName = districtlist.FirstOrDefault().DistrictName;
                var tehsilname = _context.Tehsils.Where(x => x.TehsilLgdId == parentWithChildObject.FirstOrDefault().CommunicationAddress_TehsilLgdId).FirstOrDefault();
                genericFormModel.FormModel.GeneralDetail.TehsilName = tehsilname.TehsilName;
                genericFormModel.AppFormStepsList = new List<AppFormStepsInfo>();
                genericFormModel.AppFormStepsList = await _iApplicationMamnagementService.GetAppFormStepperInfo(genericFormModel.FormModel.GeneralDetail.AppRefId, ApplicationTypeEnum.MOTOR_TRANSPORT, (genericFormModel.FormModel != null ? genericFormModel.FormModel.GeneralDetail.Id : id), "LOCK");
            }

            catch (Exception ex)
            {
                genericFormModel.HasError = true;
                genericFormModel.ErrorDesc = ex.Message;
            }
            return genericFormModel;
        }

        public async Task<GenericServiceResultTemplate> LockApplication(Int64 appRefId, int appActionType, string remarks)
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

                    if (await _iApplicationMamnagementService.LockApplication(appRefId, appActionType, remarks, ApplicationTypeEnum.MOTOR_TRANSPORT) != "")
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
