using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace pbsamadhannetcoreapi.ViewModels
{
    public class DigitalSignatureViewModels
    {
    }


    public class DigitalSignatureCertificateAttributes
    {
        public string SelCertSubject { get; set; }
        public string CertThumbPrint { get; set; }
        public string PublicKey { get; set; }
        public string eMail { get; set; }
        public DateTime ValidFrom { get; set; }
        public DateTime ExpDate { get; set; }

    }
}
