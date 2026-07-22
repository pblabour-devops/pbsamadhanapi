using pbsamadhannetcoreapi.Models;
using System.Collections.Generic;

namespace pbsamadhannetcoreapi.ViewModels
{
    public class ComplaintDetailViewModel
    {
        public WorkerDetail WorkerDetail { get; set; }

        public ICollection<Complaint_EmployerORContractorDetail> Complaint_EmployerORContractorDetails { get; set; }

        public Complaint_WorkplaceDetail Complaint_WorkplaceDetails { get; set; }

        public Complaint_EstablishmentDetail Complaint_EstablishmentDetails { get; set; }
        public Complaint_GratuityClaim Complaint_GratuityClaims { get; set; }
    }
}
