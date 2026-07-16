using pbsamadhannetcoreapi.Models;
using System.Threading.Tasks;

namespace pbsamadhannetcoreapi.Services.Implementations
{
    public interface IComplaintService
    {
        Task<GenericFormModel<object>> Get_ComplaintsCategories();
        Task<GenericFormModel<WorkerDetail>> GetWorkerDetails(long id, long projectSiteId);
        Task<GenericServiceResultTemplate> CreateAppComplaintTypeMapping(AppComplaintTypeMapping requestData);
        Task<GenericFormModel<Complaint_EmployerORContractorDetail>> Get_EmployerOrContractorDetails(long id);
        Task<GenericFormModel<Complaint_EstablishmentDetail>> Get_EstablishmentDetails(long id);
        Task<GenericFormModel<Complaint_GratuityClaim>> Get_GratuityClaimDetails(long id);
        Task<GenericFormModel<Complaint_MaternityBenefitComplaint>> Get_MaternityBenefitsComplaintDetails(long id);
        Task<GenericFormModel<Complaint_Claim_CodeOnWage>> Get_ClaimUnderCodeOnWagesDetails(long id);
        Task<GenericFormModel<Complaint_MinimumWagesNotPaid>> Get_MinimumWagesNotPaidDetails(long id);
        Task<GenericFormModel<Complaint_MinimumWagesNotPaidPeriodAmount>> Get_MinimumWagesNotPaidPeriodAmountDetails(long id);
    }
}
