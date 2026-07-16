using pbsamadhannetcoreapi.Models;
using System.Threading.Tasks;
using System;
using pbsamadhannetcoreapi.ViewModels;
using System.Collections.Generic;

namespace pbsamadhannetcoreapi.Services.Implementations
{
    public interface ILicence_TradeUnionService
    {
        Task<GenericFormModel<Licence_TradeUnion>> GetTradeUnionLicenceGeneralDetail(Int64 id, Int64 oldAppRefId, Int64 projectSiteId);

        Task<GenericFormModel<Licence_Trade_Union_ViewModels>> GetOfficerDetail(Int64 id, Int64 projectSiteId);

        Task<GenericFormModel<List<Licence_TradeUnion_Officer>>> Get_TradeUnion_Officer_List(Int64 appRefId, Int64 oldAppRefId, Int64 applicationPurposeType);

        Task<GenericResponseTemplateModel<bool>> RemoveOfficer(Int64 id);



        Task<GenericFormModel<Licence_Trade_Union_ViewModels>> Get_Tade_Union_Detail(long id);

        Task<GenericResponseTemplateModel<List<TradeUnionLicenceValidateViewModel>>> ValidateLicenceNumber(string licenceNo);

    }
}