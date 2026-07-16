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
using System.Text.Json.Serialization;
using Newtonsoft.Json;

namespace pbsamadhannetcoreapi.Services.Implementations
{
    public class ProjectSiteService : IProjectSiteService
    {
        private readonly IGenericRepository<ProjectSite> _iGR_ProjectSite;
        private readonly IApplicationManagementService<ProjectSite> _iApplicationMamnagementService;
        private readonly IProjectSiteRepository _iProjectSiteRepository;
        private readonly IGeneric_SP_Repository _iGeneric_SP_Repository;
        private readonly AppDbContext _context;
        private readonly IGenericRepository<ProjectSiteLog> _iGR_ProjectSiteLog;
        public ProjectSiteService(IGenericRepository<ProjectSite> iGR_ProjectSite,
         IApplicationManagementService<ProjectSite> IApplicationMamnagementService,
         AppDbContext context,
         IProjectSiteRepository iProjectSiteRepository, 
         IGeneric_SP_Repository iGeneric_SP_Repository,
         IGenericRepository<ProjectSiteLog> iGR_ProjectSiteLog)
        {
            _iGR_ProjectSite = iGR_ProjectSite;
            _iApplicationMamnagementService = IApplicationMamnagementService;
            _iProjectSiteRepository = iProjectSiteRepository;
            _iGeneric_SP_Repository =  iGeneric_SP_Repository;
            _context = context;
            _iGR_ProjectSiteLog = iGR_ProjectSiteLog;
        }
        public async Task<GenericFormModel<ProjectSite>> GetProjectSiteDetail(long id)
        {
            GenericFormModel<ProjectSite> genericFormModel = new GenericFormModel<ProjectSite>();
            try
            {
                genericFormModel.ListTemplateLists = new List<ListTemplate>();

                // Check if this is an existing record
                if (id != 0) //Existing Record
                {
                    //Get Form data
                    genericFormModel.FormModel = _iGR_ProjectSite.GetById(id);

                    genericFormModel.IsEditAllowed = false;
                    //if form model is available
                    if (genericFormModel.FormModel != null)
                    {
                        genericFormModel.IsEditAllowed = true;
                    }
                }
                else //New Record
                {
                    genericFormModel.FormModel = new ProjectSite();

                    //Allow user to edit form
                    genericFormModel.IsEditAllowed = true;
                }
            }
            catch (Exception ex)
            {
            }
            return genericFormModel;
        }
        public async Task<GenericServiceResultTemplate> AddUpdate_ProjectSiteDetail(ProjectSite formModel, string userId)
        {
            GenericServiceResultTemplate genericServiceResultTemplate = new GenericServiceResultTemplate();
            try
            {
                formModel.UserRefId = userId;
                genericServiceResultTemplate.HasException = false;
                genericServiceResultTemplate.CustomeValidationResult = CustomeValidator<ProjectSite>.ValidateModel_AllProperties(formModel);
                if (genericServiceResultTemplate.CustomeValidationResult.IsValid)
                {
                    if (formModel.ProjectSiteId != 0) //Existing record
                    {
                        formModel.LastModifiedDate = DateTime.Now;
                        _iGR_ProjectSite.Update(formModel);
                        await _iGR_ProjectSite.SavechangeAsync();
                    }
                    else //New record
                    {
                        formModel = GenericModelOps<ProjectSite>.SetNullAllNevigationProperties(formModel);
                        formModel.Createddate = formModel.LastModifiedDate = DateTime.Now;
                        _iGR_ProjectSite.Insert(formModel);
                        await _iGR_ProjectSite.SavechangeAsync();
                        long projectSiteId = formModel.ProjectSiteId;
                        // Insert Into ProjectSiteLogs
                        ProjectSiteLog projectSiteLog = new ProjectSiteLog()
                        {
                            ProjectSiteRefId = projectSiteId,
                            EstablishmentName = formModel.EstablishmentName,
                            Address = formModel.Address,
                            VillageOrTown = formModel.VillageOrTown,
                            TehsilRefId = Convert.ToInt64(formModel.TehsilRefId),
                            DistrictRefId = Convert.ToInt64(formModel.DistrictRefId),
                            PinCode = formModel.PinCode,
                            IsActive = true,
                            IsDeleted = false,
                            Createddate = DateTime.Now,
                            LastModifiedDate = DateTime.Now,
                            UserRefId = formModel.UserRefId,
                            LabourCircleRefId = formModel.LabourCircleRefId,
                            FactoryCircleRefId = formModel.FactoryCircleRefId,
                            ApplicantAadharNumber = formModel.ApplicantAadharNumber,
                            ApplicantAadharAttachment = formModel.ApplicantAadharAttachment,
                            ApplicantPanNumber = formModel.ApplicantPanNumber,
                            ApplicantPanAttachment = formModel.ApplicantPanAttachment,
                            CompanyPanNumber = formModel.CompanyPanNumber,
                            CompanyPanAttachment = formModel.CompanyPanAttachment,
                            ProjectPurpose = formModel.ProjectPurpose,
                            ContactPersonFirstName = formModel.ContactPersonFirstName,
                            ContactPersonMiddleName = formModel.ContactPersonMiddleName,
                            ContactPersonLastName = formModel.ContactPersonLastName,
                            ContactPersonEmail = formModel.ContactPersonEmail,
                            ContactPersonMobileNo = formModel.ContactPersonMobileNo,
                            AlternateMobileNo = "NA",
                            AlternateEmail = "NA",
                            ProjectSiteVersion = formModel.ProjectSiteVersion
                        };
                        await _context.ProjectSiteLogs.AddAsync(projectSiteLog);
                        _context.SaveChanges();
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

        public async Task<GenericFormModel<List<ProjectProfileViewModel>>> Get_CurrentUserAllProjectSites(string UserId)
        {
            GenericFormModel<List<ProjectProfileViewModel>> genericFormModel = new GenericFormModel<List<ProjectProfileViewModel>>();
            try
            {
                List<StoreProcedureParm> storeProcedureParms = new List<StoreProcedureParm>()
                {
                    new StoreProcedureParm (){ ParmName="UserRefId", ParmValue=UserId, isNumber=false}
                };
                genericFormModel.FormModel = await _iGeneric_SP_Repository.CallStoreProcedureReaderAsync<ProjectProfileViewModel>("sp_GetProjectSitesByUserId", storeProcedureParms);
            }
            catch (Exception ex)
            {
                genericFormModel.HasError = true;
                genericFormModel.ErrorDesc = ex.Message;
            }
            return genericFormModel;
        }
        public async Task<GenericResponseTemplateModel<ProjectSitesViewModel>> Get_ProjectSiteByProjectSiteId(Int64 projectSiteId, Int64 appRefId, int projectSiteVersion)
        {
            GenericResponseTemplateModel<ProjectSitesViewModel> genericResponse = new GenericResponseTemplateModel<ProjectSitesViewModel>();
            try
            {
                List<StoreProcedureParm> storeProcedureParms = new List<StoreProcedureParm>()
                {
                    new StoreProcedureParm (){ ParmName="ProjectSiteId", ParmValue=projectSiteId.ToString(), isNumber=true},
                    new StoreProcedureParm (){ ParmName="AppRefId", ParmValue=appRefId.ToString(), isNumber=true},
                    new StoreProcedureParm (){ ParmName="ProjectSiteVersion", ParmValue=projectSiteVersion.ToString(), isNumber=true}
                };
                var projectSites = await _iGeneric_SP_Repository.CallStoreProcedureReaderAsync<ProjectSitesViewModel>("sp_GetProjectSitesByProjectSiteId", storeProcedureParms);
                if (projectSites.Count() > 0)
                {
                    genericResponse.ResponseDataModel = projectSites.FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                genericResponse.HasError = true;
                genericResponse.ErrorDesc = ex.Message;
            }
            return genericResponse;
        }

        public async Task<GenericResponseTemplateModel<ProjectSiteViewModel>> CreateProjectSite(ProjectSite requestData, bool isIpinMapping, string iPin)
        {
            GenericResponseTemplateModel<ProjectSiteViewModel> genericServiceResultTemplate = new GenericResponseTemplateModel<ProjectSiteViewModel>() { ResponseDataModel= null, ErrorDesc="", HasError=false};
            try
            {
                var validations = CustomeValidator<ProjectSite>.ValidateModel_AllProperties(requestData);
                if (!validations.IsValid)
                {
                    
                    var kk = validations.CustomeValidationErrorList;
                }
                _iGR_ProjectSite.Insert(requestData);
                await _iGR_ProjectSite.SavechangeAsync();

                ProjectSiteLog projectSiteLog = new ProjectSiteLog();
                var projectSiteString = JsonConvert.SerializeObject(requestData, Formatting.Indented, new JsonSerializerSettings()
                                            {
                                                ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore
                                            }
                                        );
               
                projectSiteLog = JsonConvert.DeserializeObject<ProjectSiteLog>(projectSiteString);
                projectSiteLog.ProjectSiteRefId = requestData.ProjectSiteId;
                _context.ProjectSiteLogs.Add(projectSiteLog);
                _context.SaveChanges();

                if (isIpinMapping)
                {
                    ProjectSite_IpinMapping mapping = new ProjectSite_IpinMapping() 
                    {
                        IPin=iPin,
                        ProjectSiteRefId=requestData.ProjectSiteId
                    };
                    await _context.AddAsync<ProjectSite_IpinMapping>(mapping);
                    await _context.SaveChangesAsync();
                }
                genericServiceResultTemplate.ResponseDataModel = new ProjectSiteViewModel();
                genericServiceResultTemplate.ResponseDataModel.ProjectSiteId = requestData.ProjectSiteId;
                genericServiceResultTemplate.ResponseDataModel.ProjectSiteVersion = requestData.ProjectSiteVersion;
            }
            catch (Exception ex)
            {
                genericServiceResultTemplate.HasError = true;
                genericServiceResultTemplate.ErrorDesc = ex.InnerException.Message;
                throw ex;
            }
            return genericServiceResultTemplate;
        }

        public async Task<GenericResponseTemplateModel<Int64>> GetProjectSiteRefIdByAppId(Int64 appId)
        {
            GenericResponseTemplateModel<Int64> genericServiceResultTemplate = new GenericResponseTemplateModel<Int64>() { ResponseDataModel = 0, ErrorDesc = "", HasError = false };
            try
            {
                genericServiceResultTemplate.ResponseDataModel = _context.Applications.Where(x => x.AppId == appId && x.IsDeleted == false).Select(x => x.ProjectSiteRefId).FirstOrDefault();
            }
            catch (Exception ex)
            {
                genericServiceResultTemplate.HasError = true;
                genericServiceResultTemplate.ErrorDesc = ex.Message;
            }
            return genericServiceResultTemplate;
        }

        public async Task<GenericResponseTemplateModel<Int64>> GetProjectSiteRefIdByIpin(string iPin)
        {
            GenericResponseTemplateModel<Int64> genericServiceResultTemplate = new GenericResponseTemplateModel<Int64>() { ResponseDataModel = 0, ErrorDesc = "", HasError = false };
            try
            {
                genericServiceResultTemplate.ResponseDataModel = await _context.ProjectSite_IpinMappings.Where(x => x.IPin == iPin).Select(x => x.ProjectSiteRefId).FirstOrDefaultAsync();
            }
            catch (Exception ex)
            {
                genericServiceResultTemplate.HasError = true;
                genericServiceResultTemplate.ErrorDesc = ex.Message;
            }
            return genericServiceResultTemplate;
        }

        public async Task<GenericServiceResultTemplate> SetProjectSiteCircle(Int64 projectSiteRefId, Int64 factoryCircleId, Int64 labourCircleId,Int64 alcCircleId)
        {
            GenericServiceResultTemplate genericServiceResultTemplate = new GenericServiceResultTemplate();
            try
            {
               var projectSite = _context.ProjectSites.Where(x => x.ProjectSiteId == projectSiteRefId).FirstOrDefault();
                if (projectSite != null)
                {
                    if (factoryCircleId > 0)
                    {
                        projectSite.FactoryCircleRefId = factoryCircleId;
                    }
                    if (labourCircleId> 0)
                    {
                        projectSite.LabourCircleRefId= labourCircleId;
                    }
                    if (alcCircleId > 0)
                    {
                        projectSite.AlcCircleRefId = alcCircleId;
                    }
                    _context.Update<ProjectSite>(projectSite);
                    await _context.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                genericServiceResultTemplate.HasException = true;
                genericServiceResultTemplate.Exceptions = ex;
            }
            return genericServiceResultTemplate;
        }

        public async Task<GenericServiceResultTemplate> Update_ProjectSiteLogDetails(ProjectSiteEstablishmentBasicDetailsViewModel requestData)
        {
            GenericServiceResultTemplate genericServiceResultTemplate = new GenericServiceResultTemplate();
            try
            {
                var projectSiteLog = await _context.ProjectSiteLogs.Where(x => x.ProjectSiteRefId == requestData.ProjectSiteRefId 
                                                && x.ProjectSiteVersion == requestData.ProjectSiteVersion).FirstOrDefaultAsync();

                if (projectSiteLog != null)
                {
                    projectSiteLog.EstablishmentName = requestData.EstablishmentName;
                    projectSiteLog.Address = requestData.Address;
                    projectSiteLog.ContactPersonFirstName = requestData.ContactPersonFirstName;
                    projectSiteLog.ContactPersonMiddleName = requestData.ContactPersonMiddleName;
                    projectSiteLog.ContactPersonLastName = requestData.ContactPersonLastName;
                    projectSiteLog.ContactPersonMobileNo = requestData.ContactPersonMobileNo;
                    projectSiteLog.ContactPersonEmail = requestData.ContactPersonEmail;
                    projectSiteLog.ProjectPurpose = requestData.ProjectPurpose;

                    await _context.SaveChangesAsync();

                    // Application Update
                    var appData = _context.Applications.Where(x => x.AppId == requestData.AppRefId && x.IsDeleted == false).FirstOrDefault();
                    if(appData!= null)
                    {
                        appData.ProjectSiteVersion = requestData.ProjectSiteVersion;
                        _context.Update<Application>(appData);
                        await _context.SaveChangesAsync();
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
    }
}
