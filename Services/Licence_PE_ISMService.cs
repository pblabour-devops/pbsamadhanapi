using pbsamadhannetcoreapi.CommonUtiliteis;
using pbsamadhannetcoreapi.Models;
using pbsamadhannetcoreapi.Repositories.Implementations;
using pbsamadhannetcoreapi.Services.Implementations;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System;
using EFCore.BulkExtensions;
using pbsamadhannetcoreapi.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace pbsamadhannetcoreapi.Services
{
    public class Licence_PE_ISMService : ILicence_PE_ISMService
    {
        private readonly IGenericRepository<Licence_PE_ISM_GeneralDetail> _iGR_Licence_PE_ISM_GeneralDetail;
        private readonly IProjectSiteService _iProjectSiteService;
        private readonly IApplicationManagementService<Licence_PE_ISM_GeneralDetail> _iApplicationMamnagementService;
        private readonly IThirdPartyInegrationsService _iThirdPartyInegrationsService;
        private readonly IGeneric_SP_Repository _iGeneric_SP_Repository;
        private readonly AppDbContext _context;
        private readonly IGenericRepository<Licence_PE_ISM_Contrator> _iGR_Licence_PE_ISM_ContractorDetail;
        private readonly IGenericRepository<Licence_PE_ISM_AmendmentDataHistories> _iGR_Licence_ISM_AmendmentHistories;

        public Licence_PE_ISMService(
           IGenericRepository<Licence_PE_ISM_GeneralDetail> iGR_Licence_PE_ISM_GeneralDetail,
           IProjectSiteService iProjectSiteService,
           IApplicationManagementService<Licence_PE_ISM_GeneralDetail> iApplicationMamnagementService,
           IThirdPartyInegrationsService iThirdPartyInegrationsService,
           IGeneric_SP_Repository iGeneric_SP_Repository,
           AppDbContext context,
           IGenericRepository<Licence_PE_ISM_Contrator> iGR_Licence_PE_ISM_ContractorDetail,
           IGenericRepository<Licence_PE_ISM_AmendmentDataHistories> iGR_Licence_PE_ISM_AmendmentHistories)
        {
            _iGR_Licence_PE_ISM_GeneralDetail = iGR_Licence_PE_ISM_GeneralDetail;
            _iProjectSiteService = iProjectSiteService;
            _iApplicationMamnagementService = iApplicationMamnagementService;
            _iThirdPartyInegrationsService = iThirdPartyInegrationsService;
            _iGeneric_SP_Repository = iGeneric_SP_Repository;
            _context = context;
            _iGR_Licence_PE_ISM_ContractorDetail = iGR_Licence_PE_ISM_ContractorDetail;
            _iGR_Licence_ISM_AmendmentHistories = iGR_Licence_PE_ISM_AmendmentHistories;
        }

        public async Task<GenericFormModel<Licence_PE_ISM_GeneralDetail>> GetISMGeneralDetail(long id, long projectSiteId)
        {
            GenericFormModel<Licence_PE_ISM_GeneralDetail> genericFormModel = new GenericFormModel<Licence_PE_ISM_GeneralDetail>();
            try
            {
                if (id != 0) //Existing Record
                {
                    var parentWithChildObject = await _iGR_Licence_PE_ISM_GeneralDetail
                       .GetAsync(x => x.AppRefId == id,  //Conditions         
                         null,                //Orders
                         x => x.Application, x => x.Application.ApplicationAction, x => x.Licence_PE_ISM_Contrators)      //Includes
                       .ConfigureAwait(false);
                    var contratorDetails = await _iGR_Licence_PE_ISM_GeneralDetail
                       .GetAsync(x => x.AppRefId == id,  //Conditions         
                         null,                //Orders
                         x => x.Licence_PE_ISM_Contrators)      //Includes
                       .ConfigureAwait(false);
                    genericFormModel.FormModel = parentWithChildObject.FirstOrDefault();
                    genericFormModel.IsEditAllowed = genericFormModel.FormModel.Application.IsAllowEdit;
                    genericFormModel.IsLocked = genericFormModel.FormModel.Application.IsLocked;
                    genericFormModel.ApplicationLifeCycleStatusType = genericFormModel.FormModel.Application.ApplicationLifeCycleStatusType;
                    genericFormModel.FormModel.ApplicationPurposeType = genericFormModel.FormModel.Application.ApplicationPurposeType;

                    genericFormModel.FormModel.Legacy_LicenceNo = genericFormModel.FormModel.Application.Legacy_LicenceNo;
                    var siteId = await _iProjectSiteService.GetProjectSiteRefIdByAppId(id);
                    var projectProfileInfo = await _iProjectSiteService.GetProjectSiteDetail(siteId.ResponseDataModel);
                    genericFormModel.FormModel.AlcCircleRefId = projectProfileInfo.FormModel.AlcCircleRefId;

                }

                else //New Record
                {
                    genericFormModel.FormModel = new Licence_PE_ISM_GeneralDetail();

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
                genericFormModel.AppFormStepsList = await _iApplicationMamnagementService.GetAppFormStepperInfo(id, ApplicationTypeEnum.ISM_PRINCIPAL_EMPLOYER, (genericFormModel.FormModel != null ? genericFormModel.FormModel.Id : id), "GD");

            }
            catch (Exception ex)
            {
                genericFormModel.HasError = true;
                genericFormModel.ErrorDesc = ex.Message;
            }
            return genericFormModel;

        }
        public async Task<GenericFormModel<Licence_PE_ISM_ContractorDetailViewModel>> Get_ISM_Contractor_General_Detail(Int64 id, Int64 projectSiteId)
        {
            GenericFormModel<Licence_PE_ISM_ContractorDetailViewModel> genericFormModel = new GenericFormModel<Licence_PE_ISM_ContractorDetailViewModel>();
            try
            {
                genericFormModel.ListTemplateLists = new List<ListTemplate>();

                var parentWithChildObject = await _iGR_Licence_PE_ISM_GeneralDetail
                    .GetAsync(x => x.AppRefId == id,  //Conditions         
                        null,                //Orders
                       x => x.Application, x => x.Application.ApplicationAction, x => x.Licence_PE_ISM_Contrators)
                    .ConfigureAwait(false);

                genericFormModel.FormModel = new Licence_PE_ISM_ContractorDetailViewModel();
                genericFormModel.IsEditAllowed = parentWithChildObject.FirstOrDefault().Application.IsAllowEdit;
                genericFormModel.IsLocked = parentWithChildObject.FirstOrDefault().Application.IsLocked;
                genericFormModel.FormModel.id = parentWithChildObject.FirstOrDefault().Id;
                genericFormModel.FormModel.ApplicationPurposeType = parentWithChildObject.FirstOrDefault().Application.ApplicationPurposeType;
                genericFormModel.AppFormStepsList = new List<AppFormStepsInfo>();
                genericFormModel.AppFormStepsList = await _iApplicationMamnagementService.GetAppFormStepperInfo(id, ApplicationTypeEnum.ISM_PRINCIPAL_EMPLOYER, (genericFormModel.FormModel != null ? parentWithChildObject.FirstOrDefault().Id : id), "CD");
            }
            catch (Exception ex)
            {
                genericFormModel.HasError = true;
                genericFormModel.ErrorDesc = ex.Message;
                throw ex;
            }
            return genericFormModel;
        }
        public async Task<GenericFormModel<List<Licence_PE_ISM_Contrator>>> Get_ISM_Contractor_List(Int64 appRefId)
        {
            GenericFormModel<List<Licence_PE_ISM_Contrator>> genericFormModel = new GenericFormModel<List<Licence_PE_ISM_Contrator>>();
            try
            {
                var id = _context.Licence_PE_ISM_GeneralDetails.Where(x => x.AppRefId == appRefId).FirstOrDefault().Id;
                var emps = _context.Licence_PE_ISM_Contrators.Where(x => x.Licence_PE_ISM_GeneralDetailId == id).ToList();

                genericFormModel.FormModel = new List<Licence_PE_ISM_Contrator>();
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
                var emp = _context.Licence_PE_ISM_Contrators.Where(x => x.Id == id).FirstOrDefault();
                _context.Licence_PE_ISM_Contrators.RemoveRange(emp);
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

        public async Task<GenericFormModel<Licence_PE_ISMViewModel>> Get_Licence_ISM_Detail(long id)
        {
            GenericFormModel<Licence_PE_ISMViewModel> genericFormModel = new GenericFormModel<Licence_PE_ISMViewModel>();
            try
            {
                genericFormModel.ListTemplateLists = new List<ListTemplate>();
                genericFormModel.FormModel = new Licence_PE_ISMViewModel();
                genericFormModel.FormModel.GeneralDetail = new Licence_PE_ISM_GeneralDetail();
                genericFormModel.FormModel.Licence_ISM_AmendmentDataHistories = new List<Licence_PE_ISM_AmendmentDataHistories>();

                var parentWithChildObject = await _iGR_Licence_PE_ISM_GeneralDetail
                       .GetAsync(x => x.AppRefId == id,
                         null,
                         x => x.Application, x => x.Licence_PE_ISM_Contrators, x => x.Application.ApplicationAction)
                       .ConfigureAwait(false);

                var oldparentWithChildObject = await _iGR_Licence_ISM_AmendmentHistories
                      .GetAsync(x => x.AppRefId == id,
                        null)
                      .ConfigureAwait(false);

                genericFormModel.FormModel.GeneralDetail = parentWithChildObject.FirstOrDefault();
                genericFormModel.FormModel.Licence_ISM_AmendmentDataHistories = oldparentWithChildObject.ToList();
                genericFormModel.FormModel.IsLocked = parentWithChildObject.FirstOrDefault().Application.IsLocked;
                genericFormModel.FormModel.IsFeeApplicable = parentWithChildObject.FirstOrDefault().Application.IsFeeApplicable;
                genericFormModel.ApplicationLifeCycleStatusType = parentWithChildObject.FirstOrDefault().Application.ApplicationLifeCycleStatusType;
                genericFormModel.FormModel.AppActionType = parentWithChildObject.FirstOrDefault().Application.ApplicationAction.AppActionType;

                genericFormModel.AppFormStepsList = new List<AppFormStepsInfo>();
                genericFormModel.AppFormStepsList = await _iApplicationMamnagementService.GetAppFormStepperInfo(genericFormModel.FormModel.GeneralDetail.AppRefId, ApplicationTypeEnum.ISM_PRINCIPAL_EMPLOYER, (genericFormModel.FormModel != null ? genericFormModel.FormModel.GeneralDetail.Id : id), "LOCK");

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

    }
}
