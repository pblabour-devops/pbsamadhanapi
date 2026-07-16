using pbsamadhannetcoreapi.Models;
using pbsamadhannetcoreapi.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace pbsamadhannetcoreapi.Services.Implementations
{
    public interface ISystem_O_CommunicationService
    {
        Task<GenericResponseTemplateModel<Sys_O_AppId_OldDataViewModel>> OldDataAppClerance(string licenceNo);
        Task<GenericResponseTemplateModel<bool>> HasServiceAlreadyApplied(Int64 iPin, int serviceCode);
        Task<GenericResponseTemplateModel<bool>> IsServiceAlreadyInProcess(Int64 iPin, int serviceCode);
        Task<GenericResponseTemplateModel<bool>> IsServiceAlreadyInObjection(Int64 iPin, int serviceCode);
        Task<GenericResponseTemplateModel<Int64>> DoesIpinHasNativeUser(Int64 iPin);
        //Task<GenericResponseTemplateModel<bool>> FetchAndSeedAllData(Int64 iPin, int serviceCode);
        Task<GenericResponseTemplateModel<bool>> CreateServiceMigrationLog(Int64 appId, Int64 appFormId, string nar);
        Task<GenericResponseTemplateModel<List<ShopLicence_EmployeeDetailsViewModel>>> IsHavingEmployee(Int64 appId);
        Task<GenericResponseTemplateModel<Sys_O_AppId_TokenInfoViewModel>> DoesOldLicenceNoExists(string licenceNo, ApplicationTypeEnum applicationType);
        Task<GenericResponseTemplateModel<bool>> DeRegisterFactoryByFactoryBacklog(DeRegsiterFactoryRequestViewModel deRegsiterFactoryRequest, string fileName);
        Task<GenericResponseTemplateModel<Sys_O_AppId_TokenInfoViewModel>> GetLegacyAppIdFromIPin(Int64 investPunjab_Ipin, Int64 investPunjab_AppId, ApplicationTypeEnum applicationType);
        Task<GenericResponseTemplateModel<List<Licence_CL_PE_Contractor_ViewModel>>> GetContractorList(Int64 appId, Int64 appFormId, string nar);

        Task<GenericResponseTemplateModel<List<Licence_TradeUnion_Officer_ViewModel>>> GetTradeUnionOfficerList(Int64 tradeUnionId);
        Task<GenericResponseTemplateModel<List<InterstatePEContractLabour_ViewModel>>> GetISMContractorList(Int64 appId, Int64 appFormId, string nar);
    }
}
