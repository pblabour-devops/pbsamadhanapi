using pbsamadhannetcoreapi.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace pbsamadhannetcoreapi.ViewModels
{
    public class BocwContractorDetailViewModel
    {
        public EngagedAnyContractorTypeEnum EngagedAnyContractorType { get; set; }
        public Int64 BocwEstablishmentRegistrationId { get; set; }
        public Int64 AppRefId { get; set; }
        public int Totalworker { get; set; }
        public BocwEngagedWorkerSlabTypeEnum BocwEngagedWorkerSlabType { get; set; }
        public DateTime Work_CommencementDate { get; set; }
        public DateTime Work_CompletionDate { get; set; }
        public List<Licence_BocwAct_ContractorDetail> BocwContractorList { get; set; }
    }

    public class BocwContractorListViewModel
    {
        public Int64 BocwEstablishmentContractorDetailId { get; set; }

        public Int64? BocwEstablishmentRegistrationRefId { get; set; }

        public string ContractorName { get; set; }

        public string ContractorAddress { get; set; }

        public string NatureOfConstructionWork { get; set; }

        public int maxNoOfContractorOnAnyDay { get; set; }

        public DateTime Contractor_Work_CommencementDate { get; set; }

        public DateTime Contractor_Work_CompletionDate { get; set; }
    }

    public class BocwLicenceViewModel
    {
        public Licence_BocwAct_GeneralDetail GeneralDetail { get; set; }

        public Licence_BocwAct_GeneralDetail OldGeneralDetail { get; set; }
        public bool IsLocked { get; set; }
        public bool IsFeeApplicable { get; set; }
        public string ConstructionBuildingTypeDesc { get; set; }
        public string BocwEngagedWorkerSlabType { get; set; }
        public string BocwRegisteredType { get; set; }

    }
}