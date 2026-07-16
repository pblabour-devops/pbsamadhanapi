using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;

namespace pbsamadhannetcoreapi.Models
{
    public class DistrictLgd
    {
        [Key]
        [Required]
        public Int64 DistrictLgdId { get; set; }

        [Required, StringLength(50)]
        public string DistrictName { get; set; }

        [Required, StringLength(5)]
        public string DistrictAliasName { get; set; }

        #region ForeignKeyReferences
        //public virtual ICollection<Establishment_GeneralDetail> Estb_DistrictLgds { get; set; }
        public virtual ICollection<Establishment_GeneralDetail> Comm_DistrictLgds { get; set; }
        public virtual ICollection<Establishment_EmployerDetail> Employer_DistrictLgds { get; set; }
        public virtual ICollection<Establishment_ContractorDetail> Contractor_DistrictLgds { get; set; }
        public virtual ICollection<TehsilLgd> TehsilLgds { get; set; }
        public virtual ICollection<Contractor_GeneralDetail> Contractor_GeneralDetail_DistrictLgds { get; set; }
        public virtual ICollection<Contractor_PrincipalEmployer> Contractor_PE_ESTB_DistrictLgds { get; set; }
        public virtual ICollection<Contractor_PrincipalEmployer> Contractor_PE_DistrictLgds { get; set; }
        //public virtual ICollection<Contractor_ContractLabour> Contractor_ContractLabour_DistrictLgds { get; set; }
        public virtual ICollection<FactoryCircle> FactoryCircles { get; set; }
        public virtual ICollection<ALCCircle> ALCCircles { get; set; }
        public virtual ICollection<ProjectSite> ProjectSites { get; set; }
        public virtual ICollection<DistrictLevelUserMapping> DistrictLevelUserMappings { get; set; }
        public virtual ICollection<Circle> Circles { get; set; }
        public virtual ICollection<OSH_Form_1_Registration_Factory> OSH_Form_1_Registration_Factories { get; set; }

        public virtual ICollection<OSH_Form_1_Registration_EmployerDetail> OSH_Form_1_Registration_EmployerDetail { get; set; }
        public virtual ICollection<OSH_Form_1_Registration_PrincipalEmployerDetail> OSH_Form_1_Registration_PrincipalEmployerDetail { get; set; }
        public virtual ICollection<OSH_Form_1_Registration_ContractorDetail> OSH_Form_1_Registration_ContractorDetail { get; set; }

        #region For samadhaan portal
        public virtual ICollection<WorkerDetail> PermanentWorkerDetails { get; set; }
        public virtual ICollection<WorkerDetail> CorrespondenceWorkerDetails { get; set; }
        public virtual ICollection<Complaint_EmployerORContractorDetail> Complaint_EmployerORContractorDetails { get; set; }
        public virtual ICollection<Complaint_WorkplaceDetail> Complaint_WorkplaceDetails { get; set; }
        public virtual ICollection<Complaint_EstablishmentDetail> Complaint_EstablishmentDetails { get; set; }


        #endregion



        #endregion ForeignKeyReferences
    }

    public class TehsilLgd
    {
        [Key]
        [Required]
        public Int64 TehsilLgdId { get; set; }

        [Required, StringLength(50)]
        public string TehsilName { get; set; }


        [ForeignKey("DistrictLgd")]
        public Int64 DistrictRefId { get; set; }
        public virtual DistrictLgd DistrictLgd { get; set; }

        #region ForeignKeyReferences
        //public virtual ICollection<Establishment_GeneralDetail> Estb_TehsilLgds { get; set; }
        public virtual ICollection<Establishment_GeneralDetail> Comm_TehsilLgds { get; set; }
        public virtual ICollection<Establishment_EmployerDetail> Employer_TehsilLgds { get; set; }
        public virtual ICollection<Establishment_ContractorDetail> Contractor_TehsilLgds { get; set; }
        public virtual ICollection<Contractor_GeneralDetail> Contractor_GeneralDetail_TehsilLgds { get; set; }
        public virtual ICollection<Contractor_PrincipalEmployer> Contractor_PE_ESTB_TehsilLgds { get; set; }
        public virtual ICollection<Contractor_PrincipalEmployer> Contractor_PE_TehsilLgds { get; set; }
        //public virtual ICollection<Contractor_ContractLabour> Contractor_ContractLabour_TehsilLgds { get; set; }
        public virtual ICollection<ProjectSite> ProjectSites { get; set; }
        public virtual ICollection<OSH_Form_1_Registration_Factory> OSH_Form_1_Registration_Factories { get; set; }

        public virtual ICollection<OSH_Form_1_Registration_EmployerDetail> OSH_Form_1_Registration_EmployerDetail { get; set; }
        public virtual ICollection<OSH_Form_1_Registration_PrincipalEmployerDetail> OSH_Form_1_Registration_PrincipalEmployerDetail { get; set; }
        public virtual ICollection<OSH_Form_1_Registration_ContractorDetail> OSH_Form_1_Registration_ContractorDetail { get; set; }
        #endregion ForeignKeyReferences
    }
}
