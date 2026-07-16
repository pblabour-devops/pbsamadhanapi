using pbsamadhannetcoreapi.Models;
using pbsamadhannetcoreapi.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace pbsamadhannetcoreapi.Services.Implementations
{
    public interface IAppTimeLineManagerService
    {
        Task<GenericResponseTemplateModel<bool>> SeedTimeLineData(Int64 appRefId, Int64 appActionLogRefId);
        Task<bool> ProcessTimeLineWise(ElapsedApplicationViewModel elapsedApplication);
        Task<bool> UpdateAppProcessedTypePhaseCounter(Int64 id);
        Task<bool> AddAppProcessLogType(ApplicationProcessPhaseLog appProcessPhaseLogs);
    }
}
