using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace pbsamadhannetcoreapi.Models
{
	public class AutoApprove_ProcessEngineLog
    {
		[Key]
		public Int64 AutoApproveProcessEngineId { get; set; }
        public DateTime ProcessStartsOn { get; set; }
		public DateTime? ProcessEndsOn { get; set; }
        public int TotalTargetApplications { get; set; }
        public string Description { get; set; }
	}

	public class AutoApprove_ProcessFilesLog
    {
		[Key]
        public Int64 AutoApproveId { get; set; }

		[Required(ErrorMessage = "Application ref id is required..!")]
		[ForeignKey("Application")]
		public Int64 AppRefId { get; set; }
		public virtual Application Application { get; set; }

		[Required(ErrorMessage = "AutoApproveProcessEngineRefId ref id is required..!")]
		[ForeignKey("AutoApprove_ProcessEngineLog")]
		public Int64 AutoApproveProcessEngineRefId { get; set; }


		[Required(ErrorMessage = "ApplicationType is required..!")]
		public ApplicationTypeEnum ApplicationType { get; set; }

		[Required(ErrorMessage = "SubmissionDate is required..!")]
        public DateTime SubmissionDate { get; set; }

        [Required(ErrorMessage = "TotalTime is required..!")]
        public int TotalTime { get; set; }

        [Required(ErrorMessage = "TotalHolidaysTime is required..!")]
        public int TotalHolidaysTime { get; set; }

        [Required(ErrorMessage = "TotalWeekEndsTime is required..!")]
        public int TotalWeekEndsTime { get; set; }

        [Required(ErrorMessage = "TotalObjectionTime is required..!")]
        public int TotalObjectionTime { get; set; }

        [Required(ErrorMessage = "AutoApproveInTime is required..!")]
        public int AutoApproveInTime { get; set; }

        [Required(ErrorMessage = "MaxAutoApproveTime is required..!")]
        public int MaxAutoApproveTime { get; set; }

        [Required(ErrorMessage = "AutoApproveTimeType is required..!")]
        [StringLength(5, ErrorMessage = "AutoApproveTimeType max length is 5..!")]
        public string AutoApproveTimeType { get; set; }

        [Required(ErrorMessage = "AutoApproveProcessStatusType is required..!")]
		public AutoApproveProcessStatusTypeEnum AutoApproveProcessStatusType { get; set; }

        [StringLength(100, ErrorMessage = "CertificateFileName max length is 100..!")]
        public string CertificateFileName { get; set; }
        public DateTime? FileAutoApproveDate { get; set; }
        public string AutoApproveProcessRemarks { get; set; }

		[Required(ErrorMessage = "Officer_UserRefId is required..!")]
		[StringLength(450, ErrorMessage = "Officer_UserRefId max length is 450..!")]
		public string Officer_UserRefId { get; set; }

		[Required(ErrorMessage = "Officer_RoleRefId is required..!")]
		[StringLength(450, ErrorMessage = "Officer_RoleRefId max length is 450..!")]
		public string Officer_RoleRefId { get; set; }

		[Required(ErrorMessage = "Officer_ProfileRefId is required..!")]
		public Int64 Officer_ProfileRefId { get; set; }
	}
}
