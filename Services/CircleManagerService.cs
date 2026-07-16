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
    public class CircleManagerService : ICircleManager
    {
        private readonly IGeneric_SP_Repository _iGeneric_SP_Repository;
        private readonly AppDbContext _context;
        public CircleManagerService(IGeneric_SP_Repository iGeneric_SP_Repository, AppDbContext context)
        {
            _iGeneric_SP_Repository = iGeneric_SP_Repository;
            _context = context;
        }

        #region Factory Circle
        public async Task<GenericFormModel<List<CircleManagerViewModel>>> Get_FactoryCircleByDistricRefId(int districtRefId)
        {
            GenericFormModel<List<CircleManagerViewModel>> genericFormModel = new GenericFormModel<List<CircleManagerViewModel>>();
            try
            {
                List<StoreProcedureParm> storeProcedureParms = new List<StoreProcedureParm>()
                {
                    new StoreProcedureParm (){ ParmName="DistricRefId", ParmValue=districtRefId.ToString(), isNumber=true}
                };
                genericFormModel.FormModel = await _iGeneric_SP_Repository.CallStoreProcedureReaderAsync<CircleManagerViewModel>("sp_Circle_GetFactoryCircleByDistricRefId", storeProcedureParms);
            }
            catch (Exception ex)
            {
                genericFormModel.HasError = true;
                genericFormModel.ErrorDesc = ex.Message;
            }
            return genericFormModel;
        }
        #endregion Factory Circle

        #region Labour Circle
        public async Task<GenericFormModel<List<LabourCircleViewModel>>> Get_LabourCircleByDistricRefId(int districtRefId)
        {
            GenericFormModel<List<LabourCircleViewModel>> genericFormModel = new GenericFormModel<List<LabourCircleViewModel>>();
            try
            {
                List<StoreProcedureParm> storeProcedureParms = new List<StoreProcedureParm>()
                {
                    new StoreProcedureParm (){ ParmName="districtRefId", ParmValue=districtRefId.ToString(), isNumber=true}
                };
                genericFormModel.FormModel = await _iGeneric_SP_Repository.CallStoreProcedureReaderAsync<LabourCircleViewModel>("sp_Circle_GetLabourCircleByALCCircleRefId", storeProcedureParms);
            }
            catch (Exception ex)
            {
                genericFormModel.HasError = true;
                genericFormModel.ErrorDesc = ex.Message;
            }
            return genericFormModel;
        }
        #endregion Labour Circle

        public async Task<GenericResponseTemplateModel<List<TransferUserInfoViewModel>>> Get_LabourCircleOfficersByAppRefId(Int64 id)
        {
            GenericResponseTemplateModel<List<TransferUserInfoViewModel>> genericFormModel = new GenericResponseTemplateModel<List<TransferUserInfoViewModel>>();
            try
            {
                List<StoreProcedureParm> storeProcedureParms = new List<StoreProcedureParm>()
                {
                    new StoreProcedureParm (){ ParmName="appRefId", ParmValue=id.ToString(), isNumber=true}
                };
                genericFormModel.ResponseDataModel = await _iGeneric_SP_Repository.CallStoreProcedureReaderAsync<TransferUserInfoViewModel>("sp_Circle_GetLabourCircleOfficersByAppRefId", storeProcedureParms);
            }
            catch (Exception ex)
            {
                genericFormModel.HasError = true;
                genericFormModel.ErrorDesc = ex.Message;
            }
            return genericFormModel;
        }

        public async Task<GenericResponseTemplateModel<VerifyAppCircleVersionRespViewModel>> VerifyAppCircleVersionUpdate(Int64 projectSiteRefId, string roleCode, ApplicationTypeEnum applicationType)
        {
            GenericResponseTemplateModel<VerifyAppCircleVersionRespViewModel> genericFormModel = new GenericResponseTemplateModel<VerifyAppCircleVersionRespViewModel>();
            try
            {
                if (applicationType == ApplicationTypeEnum.BUILDING_PLAN_HUD)
                {
                    genericFormModel.ResponseDataModel = new VerifyAppCircleVersionRespViewModel()
                    {
                        IsAlreadyUpdated = true
                    };
                }
                else
                {
                    var projectSite = _context.ProjectSites.Where(x => x.ProjectSiteId == projectSiteRefId).FirstOrDefault();
                    int version = 2;
                    if (applicationType == ApplicationTypeEnum.SHOP_LICENCE)
                    {
                        version = _context.LabourCircles.Where(x => x.LabourCircleId == projectSite.LabourCircleRefId).Select(x => x.Version).FirstOrDefault();
                    }
                    else if (applicationType == ApplicationTypeEnum.FACTORY_LICENCE)
                    {
                        version = _context.FactoryCircles.Where(x => x.FactoryCircleId == projectSite.FactoryCircleRefId).Select(x => x.Version).FirstOrDefault();
                    }
                    else if (applicationType == ApplicationTypeEnum.CONTRACT_LABOUR || applicationType == ApplicationTypeEnum.PRINCIPAL_EMPLOYER)
                    {
                        version = _context.ALCCircles.Where(x => x.ALCCircleId == projectSite.AlcCircleRefId).Select(x => x.Version).FirstOrDefault();
                    }
                    else if (applicationType == ApplicationTypeEnum.MOTOR_TRANSPORT)
                    {
                        version = _context.ALCCircles.Where(x => x.ALCCircleId == projectSite.AlcCircleRefId).Select(x => x.Version).FirstOrDefault();
                    }
                    genericFormModel.ResponseDataModel = new VerifyAppCircleVersionRespViewModel();

                    if (version == 1)
                    {
                        genericFormModel.ResponseDataModel.IsAlreadyUpdated = false;
                    }
                    else
                    {
                        genericFormModel.ResponseDataModel.IsAlreadyUpdated = true;
                    }

                    if (!genericFormModel.ResponseDataModel.IsAlreadyUpdated)
                    {
                        List<StoreProcedureParm> storeProcedureParms = new List<StoreProcedureParm>()
                        {
                            new StoreProcedureParm (){ ParmName="districtRefId", ParmValue=projectSite.DistrictRefId.ToString(), isNumber=true},
                            new StoreProcedureParm (){ ParmName="roleName", ParmValue=roleCode, isNumber=false}
                        };


                        List<LatestCircleInfoViewModel> circleList = new List<LatestCircleInfoViewModel>();
                        if (applicationType == ApplicationTypeEnum.SHOP_LICENCE)
                        {
                            circleList = await _iGeneric_SP_Repository.CallStoreProcedureReaderAsync<LatestCircleInfoViewModel>("sp_Circle_GetLatestLabourCircleByDistrictRefIdAndRoleWise", storeProcedureParms);
                        }
                        else if (applicationType == ApplicationTypeEnum.FACTORY_LICENCE)
                        {
                            circleList = await _iGeneric_SP_Repository.CallStoreProcedureReaderAsync<LatestCircleInfoViewModel>("sp_Circle_GetLatestFactoryCircleByDistrictRefIdAndRoleWise", storeProcedureParms);
                        }
                        else if (applicationType == ApplicationTypeEnum.MOTOR_TRANSPORT || applicationType == ApplicationTypeEnum.PRINCIPAL_EMPLOYER || applicationType == ApplicationTypeEnum.CONTRACT_LABOUR || applicationType == ApplicationTypeEnum.ISM_PRINCIPAL_EMPLOYER)
                        {
                            circleList = await _iGeneric_SP_Repository.CallStoreProcedureReaderAsync<LatestCircleInfoViewModel>("sp_Circle_GetLatestAlcCircleByDistrictRefIdAndRoleWise", storeProcedureParms);
                        }

                        genericFormModel.ResponseDataModel.LatestCircles = new List<LatestCircleInfoViewModel>();
                        genericFormModel.ResponseDataModel.LatestCircles = circleList;
                    }
                }
            }
            catch (Exception ex)
            {
                genericFormModel.HasError = true;
                genericFormModel.ErrorDesc = ex.Message;
            }
            return genericFormModel;
        }

        public async Task<GenericResponseTemplateModel<List<TransferFactoryCircleUserInfoViewModel>>> Get_FactoryCircleOfficersByAppRefId(Int64 id, string roleName)
        {
            GenericResponseTemplateModel<List<TransferFactoryCircleUserInfoViewModel>> genericFormModel = new GenericResponseTemplateModel<List<TransferFactoryCircleUserInfoViewModel>>();
            try
            {
                List<StoreProcedureParm> storeProcedureParms = new List<StoreProcedureParm>()
                {
                    new StoreProcedureParm (){ ParmName="appRefId", ParmValue=id.ToString(), isNumber=true},
                    new StoreProcedureParm (){ ParmName="roleName", ParmValue=roleName.ToString(), isNumber=false},
                };
                genericFormModel.ResponseDataModel = await _iGeneric_SP_Repository.CallStoreProcedureReaderAsync<TransferFactoryCircleUserInfoViewModel>("sp_Circle_GetFactoryCircleOfficersByAppRefId", storeProcedureParms);
            }
            catch (Exception ex)
            {
                genericFormModel.HasError = true;
                genericFormModel.ErrorDesc = ex.Message;
            }
            return genericFormModel;
        }

        public async Task<GenericResponseTemplateModel<List<TransferALCCircleUserInfoViewModel>>> Get_ALCCircleOfficersByAppRefId(Int64 id, string roleName)
        {
            GenericResponseTemplateModel<List<TransferALCCircleUserInfoViewModel>> genericFormModel = new GenericResponseTemplateModel<List<TransferALCCircleUserInfoViewModel>>();
            try
            {
                List<StoreProcedureParm> storeProcedureParms = new List<StoreProcedureParm>()
                {
                    new StoreProcedureParm (){ ParmName="appRefId", ParmValue=id.ToString(), isNumber=true},
                    new StoreProcedureParm (){ ParmName="roleName", ParmValue=roleName.ToString(), isNumber=false},
                };
                genericFormModel.ResponseDataModel = await _iGeneric_SP_Repository.CallStoreProcedureReaderAsync<TransferALCCircleUserInfoViewModel>("sp_Circle_GetAlcCircleOfficersByAppRefId", storeProcedureParms);
            }
            catch (Exception ex)
            {
                genericFormModel.HasError = true;
                genericFormModel.ErrorDesc = ex.Message;
            }
            return genericFormModel;
        }


        public async Task<GenericResponseTemplateModel<string>> GetCircleDetailsByCircleId(Int64 circleId, CircleTypeEnum circleType)
        {
            GenericResponseTemplateModel<string> genericFormModel = new GenericResponseTemplateModel<string>();
            try
            {
                if (circleType == CircleTypeEnum.FACTORY_CIRCLE)
                {
                    genericFormModel.ResponseDataModel = _context.FactoryCircles.Where(x => x.FactoryCircleId == circleId && x.Version == 2).Select(x => x.FactoryCircleName).FirstOrDefault();
                }
                else if (circleType == CircleTypeEnum.LABOUR_CIRCLE)
                {
                    genericFormModel.ResponseDataModel = _context.LabourCircles.Where(x => x.LabourCircleId == circleId && x.Version == 2).Select(x => x.LabourCircleName).FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                genericFormModel.HasError = true;
                genericFormModel.ErrorDesc = ex.Message;
            }
            return genericFormModel;
        }

        #region ALLC Circle
        public async Task<GenericFormModel<List<ALCCircleManagerViewModel>>> Get_AlcCircleByDistricRefId(int districtRefId)
        {
            GenericFormModel<List<ALCCircleManagerViewModel>> genericFormModel = new GenericFormModel<List<ALCCircleManagerViewModel>>();
            try
            {
                List<StoreProcedureParm> storeProcedureParms = new List<StoreProcedureParm>()
                {
                    new StoreProcedureParm (){ ParmName="DistricRefId", ParmValue=districtRefId.ToString(), isNumber=true}
                };
                genericFormModel.FormModel = await _iGeneric_SP_Repository.CallStoreProcedureReaderAsync<ALCCircleManagerViewModel>("sp_Circle_GetAlcCircleByDistricRefId", storeProcedureParms);
            }
            catch (Exception ex)
            {
                genericFormModel.HasError = true;
                genericFormModel.ErrorDesc = ex.Message;
            }
            return genericFormModel;
        }
        #endregion Factory Circle
    }
}
