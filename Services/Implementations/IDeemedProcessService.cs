using pbsamadhannetcoreapi.Models;
using pbsamadhannetcoreapi.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace pbsamadhannetcoreapi.Services.Implementations
{
    public interface IDeemedProcessService
    {
        Task<GenericResponseTemplateModel<bool>> GenerateCertificates(DeemedActionParmsViewModel requestData);
        Task<GenericResponseTemplateModel<bool>> DormantApplications(DormantParmsViewModel requestData);

        Task<GenericFormModel<bool>> EsclationApplications();

        Task<GenericFormModel<bool>> DeemedAllActCertificates();

        Task<GenericResponseTemplateModel<List<All_Act_Deemed_ProcessEngineLogsViewModel>>> GetDeemedApplications(DateTime deemedDate , String type);
        Task<GenericResponseTemplateModel<List<DeemedApplicationTimeLineViewModel>>> GetDeemedTimeLineApplications();
        Task<GenericResponseTemplateModel<EscalationFileAndActWiseDataViewModel>> GetEscalationApplicationWiseData(Int64 escalationProcessEngineRefId, string userId);

    }
}
