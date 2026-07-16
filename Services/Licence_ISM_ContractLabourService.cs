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
    public class Licence_ISM_ContractLabourService : ILicence_ISM_ContractLabourService
    {
        private readonly IGenericRepository<Licence_ISM_ContractLabour_GeneralDetail> _iGR_Licence_ISM_ContractLabour_GeneralDetail;
        private readonly IApplicationManagementService<Licence_ISM_ContractLabour_GeneralDetail> _iApplicationMamnagementService;
        private readonly IThirdPartyInegrationsService _iThirdPartyInegrationsService;
        private readonly AppDbContext _context;
        private readonly IGeneric_SP_Repository _iGeneric_SP_Repository;
        private readonly IProjectSiteService _iProjectSiteService;
        private readonly IGenericRepository<Licence_ISM_ContractLabour_AmendmentDataHistory> _iGR_Licence_ISM_ContractLabour_AmendmentHistories;
        public Licence_ISM_ContractLabourService(
            IGenericRepository<Licence_ISM_ContractLabour_GeneralDetail> iGR_Licence_ISM_ContractLabour_GeneralDetail,
            IApplicationManagementService<Licence_ISM_ContractLabour_GeneralDetail> IApplicationMamnagementService,
            IThirdPartyInegrationsService iThirdPartyInegrationsService,
            AppDbContext context,
            IGeneric_SP_Repository iGeneric_SP_Repository,
            IProjectSiteService iProjectSiteService,
            IGenericRepository<Licence_ContractLabour_AmendmentDataHistory> iGR_Licence_ContractLabour_AmendmentHistories)
        {
            _iGR_Licence_ISM_ContractLabour_GeneralDetail = iGR_Licence_ISM_ContractLabour_GeneralDetail;
            _iApplicationMamnagementService = IApplicationMamnagementService;
            _iThirdPartyInegrationsService = iThirdPartyInegrationsService;
            _context = context;
            _iGeneric_SP_Repository = iGeneric_SP_Repository;
            _iProjectSiteService = iProjectSiteService;
            _iGR_Licence_ISM_ContractLabour_AmendmentHistories = _iGR_Licence_ISM_ContractLabour_AmendmentHistories;
        }

        public async Task<GenericFormModel<Licence_ISM_ContractLabour_GeneralDetail>> GetISMContractLabourGeneralDetail(long id, long projectSiteId)
        {
            GenericFormModel<Licence_ISM_ContractLabour_GeneralDetail> genericFormModel = new GenericFormModel<Licence_ISM_ContractLabour_GeneralDetail>();
            try
            {
                if (id != 0) //Existing Record
                {
                    var parentWithChildObject = await _iGR_Licence_ISM_ContractLabour_GeneralDetail
                       .GetAsync(x => x.AppRefId == id,
                         null,
                         x => x.Application, x => x.Application.ApplicationAction)      //Includes
                       .ConfigureAwait(false);

                    var contratorDetails = await _iGR_Licence_ISM_ContractLabour_GeneralDetail
                       .GetAsync(x => x.AppRefId == id,
                         null)
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
                    genericFormModel.FormModel = new Licence_ISM_ContractLabour_GeneralDetail();

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
                genericFormModel.AppFormStepsList = await _iApplicationMamnagementService.GetAppFormStepperInfo(id, ApplicationTypeEnum.ISM_CONTRACT_LABOUR, (genericFormModel.FormModel != null ? genericFormModel.FormModel.ISMContractLabourId : id), "GD");
            }
            catch (Exception ex)
            {
                genericFormModel.HasError = true;
                genericFormModel.ErrorDesc = ex.Message;
            }
            return genericFormModel;

        }


        public async Task<GenericResponseTemplateModel<Licence_CL_PE_GeneralDetail_ViewModel>> GetISMPrincipalEmployerDetails(string licencenumber)
        {
            GenericResponseTemplateModel<Licence_CL_PE_GeneralDetail_ViewModel> genericFormModel = new GenericResponseTemplateModel<Licence_CL_PE_GeneralDetail_ViewModel>();
            try
            {
                List<StoreProcedureParm> storeProcedureParms = new List<StoreProcedureParm>()
                {
                     new StoreProcedureParm (){ ParmName="LicenceNumber", ParmValue=licencenumber.ToString(), isNumber=false}
                };
                var pedetails = await _iGeneric_SP_Repository.CallStoreProcedureReaderAsync<Licence_CL_PE_GeneralDetail_ViewModel>("sp_GetISMPrincipalEmployerDetailByLicence", storeProcedureParms);
                genericFormModel.ResponseDataModel = pedetails.FirstOrDefault();
            }
            catch (Exception ex)
            {
                genericFormModel.HasError = true;
                genericFormModel.ErrorDesc = ex.Message;
            }
            return genericFormModel;
        }

        public async Task<GenericFormModel<List<Licence_PE_ISM_Contrator>>> Get_ISM_PE_Contractor_List(Int64 generalDetailRefId)
        {
            GenericFormModel<List<Licence_PE_ISM_Contrator>> genericFormModel = new GenericFormModel<List<Licence_PE_ISM_Contrator>>();
            try
            {

                var contractorlist = _context.Licence_PE_ISM_Contrators.Where(x => x.Licence_PE_ISM_GeneralDetailId == generalDetailRefId).ToList();
                genericFormModel.FormModel = new List<Licence_PE_ISM_Contrator>();
                genericFormModel.FormModel = contractorlist;
            }
            catch (Exception ex)
            {
                genericFormModel.HasError = true;
                genericFormModel.ErrorDesc = ex.Message;
            }
            return genericFormModel;
        }

        public async Task<GenericResponseTemplateModel<Licence_PE_ISM_Contrator>> Get_ISM_PE_Contractor_List_byId(Int64 id)
        {
            GenericResponseTemplateModel<Licence_PE_ISM_Contrator> genericFormModel = new GenericResponseTemplateModel<Licence_PE_ISM_Contrator>();
            try
            {


                var pecontractordetails = _context.Licence_PE_ISM_Contrators.Where(x => x.Id == id).FirstOrDefault();
                genericFormModel.ResponseDataModel = pecontractordetails;
            }
            catch (Exception ex)
            {
                genericFormModel.HasError = true;
                genericFormModel.ErrorDesc = ex.Message;
            }
            return genericFormModel;
        }

        public async Task<GenericFormModel<Licence_ISM_Contract_LabourViewModel>> Get_Licence_ISM_ContractLabour_Detail(long id)
        {
            GenericFormModel<Licence_ISM_Contract_LabourViewModel> genericFormModel = new GenericFormModel<Licence_ISM_Contract_LabourViewModel>();
            try
            {
                genericFormModel.ListTemplateLists = new List<ListTemplate>();
                genericFormModel.FormModel = new Licence_ISM_Contract_LabourViewModel();
                genericFormModel.FormModel.GeneralDetail = new Licence_ISM_ContractLabour_GeneralDetail();
                genericFormModel.FormModel.Licence_ISM_ContractLabour_AmendmentDataHistories = new List<Licence_ISM_ContractLabour_AmendmentDataHistory>();

                var parentWithChildObject = await _iGR_Licence_ISM_ContractLabour_GeneralDetail
                       .GetAsync(x => x.AppRefId == id,
                         null,
                         x => x.Application, x => x.Application.ApplicationAction)
                       .ConfigureAwait(false);

                //var oldparentWithChildObject = await _iGR_Licence_ISM_ContractLabour_AmendmentHistories
                //  .GetAsync(x => x.AppRefId == id,
                //    null)
                //  .ConfigureAwait(false);

                var oldparentWithChildObject = _context.Licence_ISM_ContractLabour_AmendmentDataHistories.Where(x => x.AppRefId == id).ToList();

                genericFormModel.FormModel.GeneralDetail = parentWithChildObject.FirstOrDefault();
                genericFormModel.FormModel.Licence_ISM_ContractLabour_AmendmentDataHistories = oldparentWithChildObject.ToList();
                genericFormModel.FormModel.IsLocked = parentWithChildObject.FirstOrDefault().Application.IsLocked;
                genericFormModel.FormModel.IsFeeApplicable = parentWithChildObject.FirstOrDefault().Application.IsFeeApplicable;
                genericFormModel.ApplicationLifeCycleStatusType = parentWithChildObject.FirstOrDefault().Application.ApplicationLifeCycleStatusType;
                genericFormModel.FormModel.AppActionType = parentWithChildObject.FirstOrDefault().Application.ApplicationAction.AppActionType;

                genericFormModel.AppFormStepsList = new List<AppFormStepsInfo>();
                genericFormModel.AppFormStepsList = await _iApplicationMamnagementService.GetAppFormStepperInfo(genericFormModel.FormModel.GeneralDetail.AppRefId, ApplicationTypeEnum.ISM_CONTRACT_LABOUR, (genericFormModel.FormModel != null ? genericFormModel.FormModel.GeneralDetail.ISMContractLabourId : id), "LOCK");

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