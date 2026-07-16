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
using DocumentFormat.OpenXml.Wordprocessing;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace pbsamadhannetcoreapi.Services
{
    public class Licence_TradeUnionService : ILicence_TradeUnionService
    {
        private readonly IGenericRepository<Licence_TradeUnion> _iGR_Licence_TradeUnion;
        private readonly IApplicationManagementService<Licence_TradeUnion> _iApplicationMamnagementService;
        private readonly IThirdPartyInegrationsService _iThirdPartyInegrationsService;
        private readonly AppDbContext _context;
        private readonly IGeneric_SP_Repository _iGeneric_SP_Repository;
        private readonly IProjectSiteService _iProjectSiteService;
        private IAuthService _iAuthService;
        private readonly IGenericRepository<DistrictLgd> _iGR_DistrictLgd;
        private readonly IGenericRepository<Licence_TradeUnion_AmendmentDataHistories> _iGR_Licence_Trade_Union_AmendmentHistories;
        private readonly IGenericRepository<Licence_TradeUnion_Officer> _iGR_Licence_Trade_Union_Officer;
        public Licence_TradeUnionService(IGenericRepository<Licence_TradeUnion> iGR_Licence_TradeUnion,
             IApplicationManagementService<Licence_TradeUnion> IApplicationMamnagementService,
             IThirdPartyInegrationsService iThirdPartyInegrationsService,
             AppDbContext context,
             IGeneric_SP_Repository iGeneric_SP_Repository,
             IAuthService authService,
             IProjectSiteService iProjectSiteService,
             IGenericRepository<DistrictLgd> iGR_DistrictLgd,
                 IGenericRepository<Licence_TradeUnion_AmendmentDataHistories> iGR_Licence_Trade_Union_AmendmentHistories,
                 IGenericRepository<Licence_TradeUnion_Officer> iGR_Licence_Trade_Union_Officer)
        {
            _iGR_Licence_TradeUnion = iGR_Licence_TradeUnion;
            _iApplicationMamnagementService = IApplicationMamnagementService;
            _iThirdPartyInegrationsService = iThirdPartyInegrationsService;
            _context = context;
            _iGeneric_SP_Repository = iGeneric_SP_Repository;
            _iProjectSiteService = iProjectSiteService;
            _iAuthService = authService;
            _iGR_DistrictLgd = iGR_DistrictLgd;
            _iGR_Licence_Trade_Union_AmendmentHistories = iGR_Licence_Trade_Union_AmendmentHistories;
            _iGR_Licence_Trade_Union_Officer = iGR_Licence_Trade_Union_Officer;
        }

        public async Task<GenericFormModel<Licence_TradeUnion>> GetTradeUnionLicenceGeneralDetail(long id, long oldAppRefId, long projectSiteId)
        {
            GenericFormModel<Licence_TradeUnion> genericFormModel = new GenericFormModel<Licence_TradeUnion>();
            try
            {
                genericFormModel.ListTemplateLists = new List<ListTemplate>();

                //Initialization of list templates
                genericFormModel.ListTemplateLists.Add(new ListTemplate()
                {
                    ListTypeCode = "Districts",
                    ListItems = _iGR_DistrictLgd.GetAll().Select(x => new GenericListTemplate() { ID = x.DistrictLgdId, Text = x.DistrictName }).ToList()
                });
                if (id != 0 && oldAppRefId == 0) //Existing Record
                {

                    genericFormModel.FormModel = _context.Licence_TradeUnion.Where(x => x.AppRefId == id).Include(x => x.Application).FirstOrDefault();
                    genericFormModel.IsEditAllowed = genericFormModel.FormModel.Application.IsAllowEdit;
                    genericFormModel.IsLocked = genericFormModel.FormModel.Application.IsLocked;
                    genericFormModel.ApplicationLifeCycleStatusType = genericFormModel.FormModel.Application.ApplicationLifeCycleStatusType;

                    var siteId = await _iProjectSiteService.GetProjectSiteRefIdByAppId(id);
                    var projectProfileInfo = await _iProjectSiteService.GetProjectSiteDetail(siteId.ResponseDataModel);
                    genericFormModel.FormModel.LabourCircleRefId = projectProfileInfo.FormModel.LabourCircleRefId;
                }

                else //New Record OR Amendment
                {
                    if (oldAppRefId > 0)
                    {

                        genericFormModel.FormModel = _context.Licence_TradeUnion.Where(x => x.AppRefId == oldAppRefId).Include(x => x.Application).FirstOrDefault();
                        genericFormModel.FormModel.Application.ApplicationPurposeType = ApplicationPurposeTypeEnum.AMENDMENT_LICENCE;
                        genericFormModel.FormModel.Application.IsLocked = false;
                        genericFormModel.FormModel.Application.IsAllowEdit = true;
                        genericFormModel.IsEditAllowed = true;
                        genericFormModel.IsLocked = false;
                        genericFormModel.ApplicationLifeCycleStatusType = ApplicationLifeCycleStatusTypeEnum.NOT_SUBMITTED;
                    }
                    else
                    {

                        genericFormModel.FormModel = new Licence_TradeUnion();

                        genericFormModel.IsEditAllowed = true;
                        genericFormModel.IsLocked = false;
                        genericFormModel.ApplicationLifeCycleStatusType = ApplicationLifeCycleStatusTypeEnum.NOT_SUBMITTED;
                    }
                }

                genericFormModel.AppFormStepsList = new List<AppFormStepsInfo>();
                genericFormModel.AppFormStepsList = await _iApplicationMamnagementService.GetAppFormStepperInfo(id, ApplicationTypeEnum.TRADE_UNION, (genericFormModel.FormModel != null ? genericFormModel.FormModel.TradeUnionId : id), "GD");
            }
            catch (Exception ex)
            {
                genericFormModel.HasError = true;
                genericFormModel.ErrorDesc = ex.Message;
            }
            return genericFormModel;

        }



        public async Task<GenericFormModel<Licence_Trade_Union_ViewModels>> GetOfficerDetail(Int64 id, Int64 projectSiteId)
        {
            GenericFormModel<Licence_Trade_Union_ViewModels> genericFormModel = new GenericFormModel<Licence_Trade_Union_ViewModels>();
            try
            {
                genericFormModel.ListTemplateLists = new List<ListTemplate>();

                var parentWithChildObject = await _iGR_Licence_TradeUnion
                    .GetAsync(x => x.AppRefId == id,  //Conditions         
                        null,                //Orders
                       x => x.Application, x => x.Application.ApplicationAction)      //Includes
                    .ConfigureAwait(false);

                genericFormModel.FormModel = new Licence_Trade_Union_ViewModels();
                genericFormModel.IsEditAllowed = parentWithChildObject.FirstOrDefault().Application.IsAllowEdit;
                genericFormModel.IsLocked = parentWithChildObject.FirstOrDefault().Application.IsLocked;
                genericFormModel.ApplicationLifeCycleStatusType = parentWithChildObject.FirstOrDefault().Application.ApplicationLifeCycleStatusType;

                genericFormModel.AppFormStepsList = new List<AppFormStepsInfo>();
                genericFormModel.AppFormStepsList = await _iApplicationMamnagementService.GetAppFormStepperInfo(id, ApplicationTypeEnum.TRADE_UNION, (genericFormModel.FormModel != null ? parentWithChildObject.FirstOrDefault().TradeUnionId : id), "CD");
            }
            catch (Exception ex)
            {
                genericFormModel.HasError = true;
                genericFormModel.ErrorDesc = ex.Message;
                throw ex;
            }
            return genericFormModel;
        }



        public async Task<GenericFormModel<List<Licence_TradeUnion_Officer>>> Get_TradeUnion_Officer_List(Int64 appRefId, Int64 oldAppRefId, Int64 applicationPurposeType)
        {
            GenericFormModel<List<Licence_TradeUnion_Officer>> genericFormModel = new GenericFormModel<List<Licence_TradeUnion_Officer>>();
            try
            {


                var tradeUnionId_new = _context.Licence_TradeUnion.Where(x => x.AppRefId == appRefId).FirstOrDefault().TradeUnionId;
                var emps = _context.Licence_TradeUnion_Officer.Where(x => x.TradeUnionRefId == tradeUnionId_new).ToList();

                if (emps.Count() == 0 && oldAppRefId != 0)
                {
                    var tradeUnionId = _context.Licence_TradeUnion.Where(x => x.AppRefId == oldAppRefId).FirstOrDefault().TradeUnionId;
                    var emps1 = _context.Licence_TradeUnion_Officer.Where(x => x.TradeUnionRefId == tradeUnionId).ToList();
                    foreach (var item in emps1)
                    {
                        emps.Add(new Licence_TradeUnion_Officer()
                        {

                            Designation = item.Designation,
                            OfficerName = item.OfficerName,
                            Address = item.Address,
                            Phone = item.Phone,
                            Age = item.Age,
                            Occupation = item.Occupation,
                            ModifiedCounter = 1,
                            TradeUnionRefId = tradeUnionId_new
                        });
                    }


                    //emps = emps1.Select(x => { x.AppRefId = appRefId; x.TradeUnionRefId = tradeUnionId_new; return x; }).ToList();

                    _context.BulkInsert<Licence_TradeUnion_Officer>(emps);
                    _context.SaveChanges();

                }

                genericFormModel.FormModel = new List<Licence_TradeUnion_Officer>();
                genericFormModel.FormModel = emps;
            }
            catch (Exception ex)
            {
                genericFormModel.HasError = true;
                genericFormModel.ErrorDesc = ex.Message;
            }
            return genericFormModel;
        }


        public async Task<GenericResponseTemplateModel<bool>> RemoveOfficer(Int64 id)
        {
            GenericResponseTemplateModel<bool> genericFormModel = new GenericResponseTemplateModel<bool>();
            try
            {
                var emp = _context.Licence_TradeUnion_Officer.Where(x => x.OfficerId == id).FirstOrDefault();
                _context.Licence_TradeUnion_Officer.RemoveRange(emp);
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


        #region Trade Licence Detail
        public async Task<GenericFormModel<Licence_Trade_Union_ViewModels>> Get_Tade_Union_Detail(long id)
        {
            GenericFormModel<Licence_Trade_Union_ViewModels> genericFormModel = new GenericFormModel<Licence_Trade_Union_ViewModels>();
            try
            {
                genericFormModel.ListTemplateLists = new List<ListTemplate>();
                genericFormModel.FormModel = new Licence_Trade_Union_ViewModels();
                genericFormModel.FormModel.GeneralDetail = new Licence_TradeUnion();

                var parentWithChildObject = await _iGR_Licence_TradeUnion
                       .GetAsync(x => x.AppRefId == id,
                         null,
                         x => x.Application, x => x.Licence_TradeUnion_Officer)
                       .ConfigureAwait(false);

                genericFormModel.FormModel.GeneralDetail = parentWithChildObject.FirstOrDefault();

                genericFormModel.FormModel.IsLocked = parentWithChildObject.FirstOrDefault().Application.IsLocked;
                genericFormModel.FormModel.IsFeeApplicable = parentWithChildObject.FirstOrDefault().Application.IsFeeApplicable;
                genericFormModel.ApplicationLifeCycleStatusType = parentWithChildObject.FirstOrDefault().Application.ApplicationLifeCycleStatusType;

                genericFormModel.AppFormStepsList = new List<AppFormStepsInfo>();
                genericFormModel.AppFormStepsList = await _iApplicationMamnagementService.GetAppFormStepperInfo(genericFormModel.FormModel.GeneralDetail.AppRefId, ApplicationTypeEnum.TRADE_UNION, (genericFormModel.FormModel != null ? genericFormModel.FormModel.GeneralDetail.TradeUnionId : id), "LOCK");

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

        public async Task<GenericResponseTemplateModel<List<TradeUnionLicenceValidateViewModel>>> ValidateLicenceNumber(string licenceNo)
        {
            GenericResponseTemplateModel<List<TradeUnionLicenceValidateViewModel>> genericFormModel = new GenericResponseTemplateModel<List<TradeUnionLicenceValidateViewModel>>();

            try
            {
                List<StoreProcedureParm> storeProcedureParms = new List<StoreProcedureParm>()
        {
new StoreProcedureParm (){ ParmName="LicenceNo", ParmValue=licenceNo, isNumber=false }
         };

                genericFormModel.ResponseDataModel = await _iGeneric_SP_Repository.CallStoreProcedureReaderAsync<TradeUnionLicenceValidateViewModel>("dbo.sp_ValidateLicenceNumber", storeProcedureParms);
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