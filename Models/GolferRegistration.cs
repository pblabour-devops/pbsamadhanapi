using System;
using System.ComponentModel.DataAnnotations;

namespace pbsamadhannetcoreapi.Models
{
    public class GolferRegistration
    {
        [Key]
        public Int64 Id { get; set; }

        [Required(ErrorMessage = "NameOfTheParticipants type is required..!")]
        public string NameOfTheParticipants { get; set; }

        [Required(ErrorMessage = "Address is required..!")]
        [StringLength(200, ErrorMessage = "The max length of Address is 200 characters..!")]
        public string Address { get; set; }

        [RegularExpression("^([0-9]+)$", ErrorMessage = "Mobile number can only have numbers, space and special characters (, - / ()) !")]
        [StringLength(10, ErrorMessage = "Mobile number must be 10-20 characters long !", MinimumLength = 10)]
        public string MobileNo { get; set; }

        [Required(ErrorMessage = "Email is required..!"), EmailAddress(ErrorMessage = "Invalid email address..!"), StringLength(50, ErrorMessage = "The max length of email is 100 characters..!")]
        public string Email { get; set; }

        [Required(ErrorMessage = "MemberOfChdClub is required..!")]
        public string AreYouMemberOfChdClub { get; set; }

        [Required(ErrorMessage = "MembershipType is required..!")]
        [StringLength(100, ErrorMessage = "The max length of MembershipType is 100 characters..!")]
        public string MembershipType { get; set; }

        [Required(ErrorMessage = "MembershipNumber is required..!")]
        [StringLength(50, ErrorMessage = "The max length of MembershipNumber is 50 characters..!")]
        public string MembershipNumber { get; set; }

        [Required(ErrorMessage = "Handicap is required..!")]
        public int Handicap { get; set; }

        [Required(ErrorMessage = "IsFeePaid is required..!")]
        public string IsFeePaid { get; set; }

        [Required(ErrorMessage = "RegistrationDate is required..!")]
        public DateTime RegistrationDate { get; set; }

        [Required(ErrorMessage = "Signature is required..!")]
        public string Signature { get; set; }
    }
}
