using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace pbsamadhannetcoreapi.Models
{
	public class Deemed_ProcessEngineLog
	{
		[Key]
		public Int64 DeemedProcessEngineId { get; set; }
        public DeemedProcessEngineTypeEnum DeemedProcessEngineType { get; set; }
        public DateTime ProcessStartsOn { get; set; }
		public DateTime? ProcessEndsOn { get; set; }
        public int TotalTargetApplications { get; set; }
        public string Description { get; set; }
        public virtual List<Deemed_ProcessFilesLog> Deemed_ProcessFilesLogs { get; set; }
	}

	public class Deemed_ProcessFilesLog
    {
		[Key]
        public Int64 DeemedId { get; set; }

		[Required(ErrorMessage = "Application ref id is required..!")]
		[ForeignKey("Application")]
		public Int64 AppRefId { get; set; }
		public virtual Application Application { get; set; }

		[Required(ErrorMessage = "DeemedProcessEngineRefId ref id is required..!")]
		[ForeignKey("Deemed_ProcessEngineLog")]
		public Int64 DeemedProcessEngineRefId { get; set; }
		public virtual Deemed_ProcessEngineLog Deemed_ProcessEngineLog { get; set; }

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

		[Required(ErrorMessage = "DeemedInTime is required..!")]
		public int DeemedInTime { get; set; }

		[Required(ErrorMessage = "MaxDeemedTime is required..!")]
		public int MaxDeemedTime { get; set; }

		[Required(ErrorMessage = "DeemedTimeType is required..!")]
		[StringLength(5, ErrorMessage = "DeemedTimeType max length is 5..!")]
		public string DeemedTimeType { get; set; }

		[Required(ErrorMessage = "DeemedProcessStatusType is required..!")]
		public DeemedProcessStatusTypeEnum DeemedProcessStatusType { get; set; }

		public string DeemedProcessRemarks { get; set; }

		[StringLength(100, ErrorMessage = "CertificateFileName max length is 100..!")]
		public string CertificateFileName { get; set; }
		public DateTime? FileDeemedDate { get; set; }

		[Required(ErrorMessage = "Officer_UserRefId is required..!")]
		[StringLength(450, ErrorMessage = "Officer_UserRefId max length is 450..!")]
		public string Officer_UserRefId { get; set; }

		[Required(ErrorMessage = "Officer_RoleRefId is required..!")]
		[StringLength(450, ErrorMessage = "Officer_RoleRefId max length is 450..!")]
		public string Officer_RoleRefId { get; set; }

		[Required(ErrorMessage = "Officer_ProfileRefId is required..!")]
		public Int64 Officer_ProfileRefId { get; set; }
	}

    public class Dormant_ProcessEngineLog
    {
        [Key]
        public Int64 DormantProcessEngineId { get; set; }
        //public DeemedProcessEngineTypeEnum DormantProcessEngineType { get; set; }
        public DateTime ProcessStartsOn { get; set; }
        public DateTime? ProcessEndsOn { get; set; }
        public int TotalTargetApplications { get; set; }
        public string Description { get; set; }
        public virtual List<Dormant_ProcessFilesLog> Dormant_ProcessFilesLogs { get; set; }
    }

    public class Dormant_ProcessFilesLog
    {
        [Key]
        public Int64 DormantId { get; set; }

        [Required(ErrorMessage = "Application ref id is required..!")]
        [ForeignKey("Application")]
        public Int64 AppRefId { get; set; }
        public virtual Application Application { get; set; }

        [Required(ErrorMessage = "DormantProcessEngineRefId ref id is required..!")]
        [ForeignKey("Dormant_ProcessEngineLog")]
        public Int64 DormantProcessEngineRefId { get; set; }
        public virtual Dormant_ProcessEngineLog Dormant_ProcessEngineLog { get; set; }

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

        [Required(ErrorMessage = "DormantInTime is required..!")]
        public int DormantInTime { get; set; }

        [Required(ErrorMessage = "MaxDeemedTime is required..!")]
        public int MaxDormantTime { get; set; }

        [Required(ErrorMessage = "DormantTimeType is required..!")]
        [StringLength(5, ErrorMessage = "DormantTimeType max length is 5..!")]
        public string DormantTimeType { get; set; }

        [Required(ErrorMessage = "DormantProcessStatusType is required..!")]
        public DormantProcessStatusTypeEnum DormantProcessStatusType { get; set; }
        public string DormantProcessRemarks { get; set; }
        public DateTime? FileDormantDate { get; set; }

        [Required(ErrorMessage = "Officer_UserRefId is required..!")]
        [StringLength(450, ErrorMessage = "Officer_UserRefId max length is 450..!")]
        public string Officer_UserRefId { get; set; }

        [Required(ErrorMessage = "Officer_RoleRefId is required..!")]
        [StringLength(450, ErrorMessage = "Officer_RoleRefId max length is 450..!")]
        public string Officer_RoleRefId { get; set; }

        [Required(ErrorMessage = "Officer_ProfileRefId is required..!")]
        public Int64 Officer_ProfileRefId { get; set; }

        [Required(ErrorMessage = "Islegacy is required..!")]
        public int IsLegacy { get; set; }
        [Required(ErrorMessage = "Legacy_AppFormId is required..!")]
        public int Legacy_AppFormId { get; set; }
        [Required(ErrorMessage = "Legacy_NAR is required..!")]
        public string Legacy_NAR { get; set; }

        [Required(ErrorMessage = "ApplicationActionLogRefId is required..!")]
        public Int64 ApplicationActionLogRefId { get; set; }
    }

    public class Dormant_FileNotification
    {
        [Key]
        public Int64 Id { get; set; }

        [Required(ErrorMessage = "Application ref id is required..!")]
        [ForeignKey("Application")]
        public Int64 AppRefId { get; set; }
        public virtual Application Application { get; set; }

        [Required(ErrorMessage = "ApplicationActionLogRefId is required..!")]
        public Int64 ApplicationActionLogRefId { get; set; }

        [Required(ErrorMessage = "Iteration count is required..!")]
        public Int64 IterationCount { get; set; }

        [Required(ErrorMessage = "Notification date id is required..!")]
        public DateTime NotificationDate { get; set; }
    }


    public class All_Act_Deemed_ProcessEngineLogs
    {
        [Key]
        public Int64 AllActDeemedProcessEngineId { get; set; }
        public DeemedProcessEngineTypeEnum DeemedProcessEngineType { get; set; }
        public DateTime ProcessStartsOn { get; set; }
        public DateTime? ProcessEndsOn { get; set; }
        public int TotalTargetApplications { get; set; }
        public string Description { get; set; }
        public virtual List<All_Act_Deemed_ProcessFilesLogs> All_Act_Deemed_ProcessFilesLogs { get; set; }
    }

    public class All_Act_Deemed_ProcessFilesLogs
    {
        [Key]
        public Int64 DeemedId { get; set; }

        [Required(ErrorMessage = "Application ref id is required..!")]
        [ForeignKey("Application")]
        public Int64 AppRefId { get; set; }
        public virtual Application Application { get; set; }

        [Required(ErrorMessage = "DeemedProcessEngineRefId ref id is required..!")]
        [ForeignKey("All_Act_Deemed_ProcessEngineLogs")]
        public Int64 AllActDeemedProcessEngineRefId { get; set; }
        public virtual All_Act_Deemed_ProcessEngineLogs All_Act_Deemed_ProcessEngineLogs { get; set; }

        [Required(ErrorMessage = "ApplicationType is required..!")]
        public ApplicationTypeEnum ApplicationType { get; set; }

        [Required(ErrorMessage = "SubmissionDate is required..!")]
        public DateTime SubmissionDate { get; set; }

        [Required(ErrorMessage = "TotalTime is required..!")]
        public string TotalTime { get; set; }

        [Required(ErrorMessage = "TotalHolidaysTime is required..!")]
        public string TotalHolidaysTime { get; set; }

        [Required(ErrorMessage = "TotalWeekEndsTime is required..!")]
        public string TotalWeekEndsTime { get; set; }

        [Required(ErrorMessage = "TotalObjectionTime is required..!")]
        public string TotalObjectionTime { get; set; }

        [Required(ErrorMessage = "DeemedInTime is required..!")]
        public string DeemedInTime { get; set; }

        [Required(ErrorMessage = "MaxDeemedTime is required..!")]
        public string MaxDeemedTime { get; set; }

        [Required(ErrorMessage = "DeemedTimeType is required..!")]
        [StringLength(5, ErrorMessage = "DeemedTimeType max length is 5..!")]
        public string DeemedTimeType { get; set; }

        [Required(ErrorMessage = "DeemedProcessStatusType is required..!")]
        public DeemedProcessStatusTypeEnum DeemedProcessStatusType { get; set; }

        public string DeemedProcessRemarks { get; set; }

        [StringLength(100, ErrorMessage = "CertificateFileName max length is 100..!")]
        public string CertificateFileName { get; set; }
        public DateTime? FileDeemedDate { get; set; }

        [Required(ErrorMessage = "Officer_UserRefId is required..!")]
        [StringLength(450, ErrorMessage = "Officer_UserRefId max length is 450..!")]
        public string Officer_UserRefId { get; set; }

        [Required(ErrorMessage = "Officer_RoleRefId is required..!")]
        [StringLength(450, ErrorMessage = "Officer_RoleRefId max length is 450..!")]
        public string Officer_RoleRefId { get; set; }

        [Required(ErrorMessage = "Officer_ProfileRefId is required..!")]
        public Int64 Officer_ProfileRefId { get; set; }

        public DateTime DeemedStartsDate { get; set; }

        public bool IsFeeApplicable { get; set; }

        [Required(ErrorMessage = "TotalDormantTime is required..!")]
        public string TotalDormantTime { get; set; }
    }

}
