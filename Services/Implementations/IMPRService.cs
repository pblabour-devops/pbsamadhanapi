using pbsamadhannetcoreapi.Models;
using pbsamadhannetcoreapi.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace pbsamadhannetcoreapi.Services.Implementations
{
    public interface IMPRService
    {
        Task<GenericResponseTemplateModel<MPR_Factory>> GetMprFactoryDetail(Int64 id, string userId);
        Task<GenericServiceResultTemplate> AddUpdate_MprFactoryDetail(MPR_Factory formModel);
        Task<GenericResponseTemplateModel<bool>> CheckDuplicateMonthYear(long id, int month, int year, Int64 factoryCircleRefId, string uid);

        Task<GenericResponseTemplateModel<List<MPR_Labour>>> GetMprLabourDetail(string userId, int month, int year);
        Task<GenericServiceResultTemplate> InsertBlankJsonStepsAsync(InsertNullJson_ViewModel formModel);

        Task<GenericServiceResultTemplate> SaveStepDataAsync(SaveSteps_ViewModel formModel);

        Task<GenericServiceResultTemplate> InsertBlankJsonStepsAlcAsync(InsertNullJsonAlc_ViewModel formModel);

        Task<GenericServiceResultTemplate> SaveStepDataAlcAsync(SaveStepsAlc_ViewModel formModel);

        Task<GenericResponseTemplateModel<List<MPR_Alc>>> GetMprAlcDetail(string userId, int month, int year);

        Task<GenericResponseTemplateModel<List<MprDataViewModel>>> GetMprDataAppType(string userId, int year, int month, int applicationType);
    }
}

