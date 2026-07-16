using Microsoft.AspNetCore.Mvc;
using pbsamadhannetcoreapi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace pbsamadhannetcoreapi.Services.Implementations
{
    public interface IPdfOprationsService
    {
        Task<CertificateGenerateServiceResultTemplate> GenerateCertificate(Int64 appRefId, ApplicationTypeEnum applicationType, string userName, bool isDeemedCalling);

        Task<NoticeGenerateServiceResultTemplate> GenerateEPFONoticeCertificate(Establishment_EPFO_Logs formModel, string userName);
        Task<GeneratePdfServiceResultTemplate> GenerateInspectionPdf(Int64 inspectionRefId, int inspectionType);

        Task<GeneratePdfServiceResultTemplate> GenerateInspectionViolationPdf(Int64 inspectionRefId, int inspectionType);
        Task<CertificateGenerateServiceResultTemplate> Generate_Form_V(Int64 appRefid, Int64 id, string userName,int isFormVGenerate);

        Task<CertificateGenerateServiceResultTemplate> AllActGenerateCertificate(Int64 appRefId, ApplicationTypeEnum applicationType, string userName, bool isDeemedCalling);

    

    }
}
