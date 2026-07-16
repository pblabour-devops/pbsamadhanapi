using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using pbsamadhannetcoreapi.CommonUtiliteis;
using pbsamadhannetcoreapi.Models;
using pbsamadhannetcoreapi.Services;
using pbsamadhannetcoreapi.Services.Implementations;
using pbsamadhannetcoreapi.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace pbsamadhannetcoreapi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PdfOprationsController : ControllerBase
    {
        private IPdfOprationsService _IPdfOprations;
        private IAuthService _iAuthService;
        public PdfOprationsController(IPdfOprationsService IPdfOprations, IAuthService iAuthService)
        {
            _IPdfOprations = IPdfOprations;
            _iAuthService = iAuthService;
        }

        [HttpGet, Route("generatecertificate")]
        [CustomFillters.AuthorizeAttribute(AuthConfigs.AllLabourOfficialRoles)]
        public async Task<IActionResult> GenerateCertificate([FromQuery] Int64 appRefId, ApplicationTypeEnum applicationType)
        {
            var userClaims = await _iAuthService.DecryptLoggedInUserClaims(User.Claims);
            CertificateGenerateServiceResultTemplate genericFormModel = await _IPdfOprations.GenerateCertificate(appRefId, applicationType, userClaims.UserName, false);
            if (genericFormModel.HasException)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, genericFormModel.Exceptions);
            }
            return StatusCode(StatusCodes.Status200OK, genericFormModel);
        }


        [HttpPost, Route("generateepfonotice")]
        public async Task<IActionResult> GenerateEPFONotice([FromBody] Establishment_EPFO_Logs requestData)
        {
            var userClaims = await _iAuthService.DecryptLoggedInUserClaims(User.Claims);
            NoticeGenerateServiceResultTemplate genericFormModel = await _IPdfOprations.GenerateEPFONoticeCertificate(requestData, userClaims.UserName);
            if (genericFormModel.HasException)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, genericFormModel.Exceptions);
            }
            return StatusCode(StatusCodes.Status200OK, genericFormModel);
        }

        [HttpGet, Route("generateInspectionPdf")]
        [CustomFillters.AuthorizeAttribute(AuthConfigs.AllLabourOfficialRoles)]
        public async Task<IActionResult> GenerateInspectionPdf([FromQuery] Int64 inspectionRefId, int inspectionType)
        {
            var userClaims = await _iAuthService.DecryptLoggedInUserClaims(User.Claims);
            GeneratePdfServiceResultTemplate genericFormModel = await _IPdfOprations.GenerateInspectionPdf(inspectionRefId, inspectionType);
            if (genericFormModel.HasException)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, genericFormModel.Exceptions);
            }
            return StatusCode(StatusCodes.Status200OK, genericFormModel);
        }

        [HttpGet, Route("generateInspectionViolationPdf")]
        [CustomFillters.AuthorizeAttribute(AuthConfigs.AllLabourOfficialRoles)]
        public async Task<IActionResult> GenerateInspectionViolationPdf([FromQuery] Int64 inspectionRefId, int inspectionType)
            {
            GeneratePdfServiceResultTemplate genericFormModel = await _IPdfOprations.GenerateInspectionViolationPdf(inspectionRefId, inspectionType);
            if (genericFormModel.HasException)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, genericFormModel.Exceptions);
            }
            return StatusCode(StatusCodes.Status200OK, genericFormModel);
        }
        [HttpGet, Route("generateformV")]
        public async Task<IActionResult> Generate_Form_V([FromQuery] Int64 appRefid, Int64 id, int isFormVGenerate)
        {
            var userClaims = await _iAuthService.DecryptLoggedInUserClaims(User.Claims);
            CertificateGenerateServiceResultTemplate genericFormModel = await _IPdfOprations.Generate_Form_V(appRefid, id, userClaims.UserName, isFormVGenerate);
            if (genericFormModel.HasException)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, genericFormModel.Exceptions);
            }
            return StatusCode(StatusCodes.Status200OK, genericFormModel);
        }
  }
}
