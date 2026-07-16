using pbsamadhannetcoreapi.Models;
using pbsamadhannetcoreapi.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace pbsamadhannetcoreapi.Services.Implementations
{
    public interface ICircleManager
    {
        Task<GenericFormModel<List<CircleManagerViewModel>>> Get_FactoryCircleByDistricRefId(int DistrictRefId);
        Task<GenericFormModel<List<LabourCircleViewModel>>> Get_LabourCircleByDistricRefId(int districtRefId);
        Task<GenericResponseTemplateModel<List<TransferUserInfoViewModel>>> Get_LabourCircleOfficersByAppRefId(Int64 id);
        Task<GenericResponseTemplateModel<VerifyAppCircleVersionRespViewModel>> VerifyAppCircleVersionUpdate(Int64 projectSiteRefId, string roleCode, ApplicationTypeEnum applicationType);
        Task<GenericResponseTemplateModel<List<TransferFactoryCircleUserInfoViewModel>>> Get_FactoryCircleOfficersByAppRefId(Int64 id, string roleName);

        Task<GenericResponseTemplateModel<List<TransferALCCircleUserInfoViewModel>>> Get_ALCCircleOfficersByAppRefId(Int64 id, string roleName);
        Task<GenericResponseTemplateModel<string>> GetCircleDetailsByCircleId(Int64 circleId, CircleTypeEnum circleType);

        Task<GenericFormModel<List<ALCCircleManagerViewModel>>> Get_AlcCircleByDistricRefId(int DistrictRefId);
    }
}
