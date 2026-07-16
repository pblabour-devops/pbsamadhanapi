using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace pbsamadhannetcoreapi.ViewModels
{


    public class ProfileFormViewModel
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public AddressFormViewModel Address { get; set; }
        public  int CountryId { get; set; }
    }

    public class AddressFormViewModel
    {
        public int Street { get; set; }
        public string City { get; set; }
        public string Pincode { get; set; }
    }



}
