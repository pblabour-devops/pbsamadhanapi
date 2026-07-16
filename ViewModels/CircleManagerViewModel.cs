using pbsamadhannetcoreapi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace pbsamadhannetcoreapi.ViewModels
{
    public class CircleManagerViewModel
    {
        public Int64 FactoryCircleId { get; set; }
        public string FactoryCircleName { get; set; }
        public string JuridictionArea { get; set; }
    }

    public class LabourCircleViewModel
    {
        public Int64 LabourCircleId { get; set; }
        public string LabourCircleName { get; set; }
        public string JuridcitionArea { get; set; }
        public string ALCCircleName { get; set; }
        public Int64 ALCCircleRefId { get; set; }
        public string OfficerName { get; set; }
        public string Designation { get; set; }
        public string MobileNo { get; set; }
        public string Email { get; set; }
    }
    public class TransferUserInfoViewModel
    {
        public string UserRefId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public Int64 UserProfileRefId { get; set; }
        public string RoleId { get; set; }
        //public string UserName { get; set; }
        public Int64 LabourCircleId { get; set; }
        public string LabourCircleName { get; set; }
        public string JuridcitionArea { get; set; }

    }

    public class VerifyAppCircleVersionRespViewModel
    {
        public bool IsAlreadyUpdated { get; set; }
        public List<LatestCircleInfoViewModel> LatestCircles { get; set; }

    }
    public class LatestCircleInfoViewModel
    {
        public Int64 CircleId { get; set; }
        public CircleTypeEnum CircleType { get; set; }
        public string CircleName { get; set; }
        public string JuridcitionArea { get; set; }
        public string OfficerName { get; set; }
        public string RoleDesc { get; set; }
        public string UserId { get; set; }
        public Int64 UserProfileId { get; set; }
        public string RoleId { get; set; }
        public Int64 AppRefId { get; set; }
        public Int64 ProjectSiteRefId { get; set; }
        public string Sender_UserRefId { get; set; }
        public Int64 Sender_UserProfileRefId { get; set; }
    }

    public class TransferFactoryCircleUserInfoViewModel
    {
        public string UserRefId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string RoleDesc { get; set; }
        public Int64 UserProfileRefId { get; set; }
        public string RoleId { get; set; }
        public Int64 FactoryCircleId { get; set; }
        public string FactoryCircleName { get; set; }
        public string JuridictionArea { get; set; }
    }


    public class TransferALCCircleUserInfoViewModel
    {
        public string UserRefId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string RoleDesc { get; set; }
        public Int64 UserProfileRefId { get; set; }
        public string RoleId { get; set; }
        public Int64 ALCCircleId { get; set; }
        public string ALCCircleName { get; set; }
        public string JuridictionArea { get; set; }
    }

    public class ALCCircleManagerViewModel
    {
        public Int64 ALCCircleId { get; set; }
        public string ALCCircleName { get; set; }
        public string JuridictionArea { get; set; }
    }

}
