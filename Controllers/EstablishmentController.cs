using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using pbsamadhannetcoreapi.CommonUtiliteis;
using pbsamadhannetcoreapi.Models;
using pbsamadhannetcoreapi.Services;
using pbsamadhannetcoreapi.Services.Implementations;
using pbsamadhannetcoreapi.ViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace pbsamadhannetcoreapi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [CustomFillters.AuthorizeAttribute("INDL")]

    public class EstablishmentController : ControllerBase
    {
        private IEstablishmentService _iEstablishmentService;
        private IAuthService _iAuthService;
        public EstablishmentController(IEstablishmentService establishmentService, IAuthService iAuthService)
        {
            _iEstablishmentService = establishmentService;
            _iAuthService = iAuthService;
        }

        #region General Details
        [HttpGet, Route("getgeneraldetail")]
        public async Task<IActionResult> Get_GeneralDetail([FromQuery] Int64 id, Int64 projectSiteId)
        {
            GenericFormModel<Establishment_GeneralDetail> genericFormModel = await _iEstablishmentService.GetEstablishmentGeneralDetail(id, projectSiteId);
            if (genericFormModel.HasError)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, genericFormModel.ErrorDesc);
            }
            return StatusCode(StatusCodes.Status200OK, genericFormModel);
        }

        [Route("addupdate_generaldetails")]
        [HttpPost]
        public async Task<IActionResult> AddUpdate_GeneralDetail([FromBody] Establishment_GeneralDetail requestData)
        {
            var userClaims = await _iAuthService.DecryptLoggedInUserClaims(User.Claims);
            GenericServiceResultTemplate genericServiceResultTemplate = await _iEstablishmentService.AddUpdate_GeneralDetail(requestData, userClaims.UserName);
            if (genericServiceResultTemplate.HasException)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, genericServiceResultTemplate.Exceptions.Message);
            }
            else if (!genericServiceResultTemplate.CustomeValidationResult.IsValid)
            {
                return StatusCode(StatusCodes.Status400BadRequest, genericServiceResultTemplate.CustomeValidationResult.ValidationErrorMessages);
            }
            return StatusCode(StatusCodes.Status200OK, genericServiceResultTemplate);
        }
        #endregion

        #region Employer Details
        [HttpGet, Route("getemployerdetail")]
        public async Task<IActionResult> Get_EmployerDetail([FromQuery] Int64 id)
        {
            GenericFormModel<Establishment_EmployerDetail> genericFormModel = await _iEstablishmentService.GetEstablishmentEmployerDetail(id);
            if (genericFormModel.HasError)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, genericFormModel.ErrorDesc);
            }
            return StatusCode(StatusCodes.Status200OK, genericFormModel);
        }

        [Route("addupdate_employerdetails")]
        [HttpPost]
        public async Task<IActionResult> AddUpdate_EmployerDetail([FromBody] Establishment_EmployerDetail requestData)
        {
            GenericServiceResultTemplate genericServiceResultTemplate = await _iEstablishmentService.AddUpdate_EmployerDetail(requestData);
            if (genericServiceResultTemplate.HasException)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, genericServiceResultTemplate.Exceptions.Message);
            }
            else if (!genericServiceResultTemplate.CustomeValidationResult.IsValid)
            {
                return StatusCode(StatusCodes.Status400BadRequest, genericServiceResultTemplate.CustomeValidationResult.ValidationErrorMessages);
            }
            return StatusCode(StatusCodes.Status200OK, genericServiceResultTemplate);
        }
        #endregion

        #region Contractor Details
        [HttpGet, Route("getcontractordetail")]
        public async Task<IActionResult> Get_ContractorDetail([FromQuery] Int64 id)
        {
            GenericFormModel<List<Establishment_ContractorDetail>> genericFormModel = await _iEstablishmentService.GetEstablishmentContractorDetail(id);
            if (genericFormModel.HasError)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, genericFormModel.ErrorDesc);
            }
            return StatusCode(StatusCodes.Status200OK, genericFormModel);
        }

        [Route("addupdate_contractordetails")]
        [HttpPost]
        public async Task<IActionResult> AddUpdate_ContractorDetail([FromBody] Establishment_ContractorDetail requestData)
        {
            GenericServiceResultTemplate genericServiceResultTemplate = await _iEstablishmentService.AddUpdate_ContractorDetail(requestData);
            if (genericServiceResultTemplate.HasException)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, genericServiceResultTemplate.Exceptions.Message);
            }
            else if (!genericServiceResultTemplate.CustomeValidationResult.IsValid)
            {
                return StatusCode(StatusCodes.Status400BadRequest, genericServiceResultTemplate.CustomeValidationResult.ValidationErrorMessages);
            }
            return StatusCode(StatusCodes.Status200OK, genericServiceResultTemplate);
        }

        [HttpGet, Route("deletecontractordetails")]
        public async Task<IActionResult> Delete_ContractorDetail([FromQuery] Int64 establishment_ContractorDetailId)
        {
            GenericServiceResultTemplate genericFormModel = await _iEstablishmentService.DeleteContractorDetail(establishment_ContractorDetailId);
            if (genericFormModel.HasException)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, genericFormModel.Exceptions);
            }
            return StatusCode(StatusCodes.Status200OK, genericFormModel);
        }
        #endregion

        #region Employer Details
        [HttpGet, Route("getmigrantworkerdetail")]
        public async Task<IActionResult> Get_MigrantWorkerDetail([FromQuery] Int64 id)
        {
            GenericFormModel<List<Establishment_Migrantworker>> genericFormModel = await _iEstablishmentService.GetMigrantWorkerDetail(id);
            if (genericFormModel.FormModel == null && id != 0)
            {
                GenericServiceResultTemplate genericServiceResultTemplate = new GenericServiceResultTemplate();
                genericServiceResultTemplate.CustomeValidationResult = new CustomeValidationResult();
                genericServiceResultTemplate.CustomeValidationResult.IsValid = false;
                genericServiceResultTemplate.CustomeValidationResult.ValidationErrorMessages = "Invalid establishment identity";
                return StatusCode(StatusCodes.Status400BadRequest, genericServiceResultTemplate.CustomeValidationResult.ValidationErrorMessages);
            }
            return Ok(genericFormModel);
        }

        [Route("addupdate_migrantworkerdetails")]
        [HttpPost]
        public async Task<IActionResult> AddUpdate_MigrantWorkerDetail([FromBody] Establishment_Migrantworker requestData)
        {
            GenericServiceResultTemplate genericServiceResultTemplate = await _iEstablishmentService.AddUpdate_MigrantWorkerDetail(requestData);
            if (genericServiceResultTemplate.HasException)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, genericServiceResultTemplate.Exceptions.Message);
            }
            else if (!genericServiceResultTemplate.CustomeValidationResult.IsValid)
            {
                return StatusCode(StatusCodes.Status400BadRequest, genericServiceResultTemplate.CustomeValidationResult.ValidationErrorMessages);
            }
            return StatusCode(StatusCodes.Status200OK, genericServiceResultTemplate);
        }

        [HttpGet, Route("deletemigrantworkerdetails")]
        public async Task<IActionResult> Delete_MigrantWorkerDetail([FromQuery] Int64 establishmentMigrantworkerId)
        {
            GenericServiceResultTemplate genericFormModel = await _iEstablishmentService.DeleteMigrantWorkerDetail(establishmentMigrantworkerId);
            if (genericFormModel.HasException)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, genericFormModel.Exceptions);
            }
            return StatusCode(StatusCodes.Status200OK, genericFormModel);
        }
        #endregion

        #region Establishment Details

        [HttpGet, Route("getdetail")]
        public async Task<IActionResult> Get_Detail([FromQuery] Int64 id, Int64 psid)
        {
            GenericFormModel<EstablishmentDetailViewModel> genericFormModel = await _iEstablishmentService.GetEstablishmentDetail(id);
            if (genericFormModel.HasError)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, genericFormModel.ErrorDesc);
            }
            return StatusCode(StatusCodes.Status200OK, genericFormModel);
            }
        #endregion Establishment Details

        #region Lock Application

        [Route("lockapplication")]
        [HttpPost]
        public async Task<IActionResult> LockApplication([FromBody] ApplicationLockParmsViewModel applicationLockParms)
        {
            GenericServiceResultTemplate genericServiceResultTemplate = await _iEstablishmentService.LockApplication(applicationLockParms.AppId, applicationLockParms.AppActionType, applicationLockParms.Remarks);
            if (genericServiceResultTemplate.HasException)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, genericServiceResultTemplate.Exceptions.Message);
            }
            else if (!genericServiceResultTemplate.CustomeValidationResult.IsValid)
            {
                return StatusCode(StatusCodes.Status400BadRequest, genericServiceResultTemplate.CustomeValidationResult.ValidationErrorMessages);
            }
            return StatusCode(StatusCodes.Status200OK, genericServiceResultTemplate);
        }
        #endregion Lock Application

        [HttpGet, Route("findDuplicateGST")]
        public async Task<IActionResult> FindDuplicateGST([FromQuery] string gstNumber, Int64 establishmentId)
        {
            GenericFormModel<IntReturn> genericFormModel = await _iEstablishmentService.FindDuplicateGST(gstNumber, establishmentId);
            if (genericFormModel.HasError)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, genericFormModel.ErrorDesc);
            }
            return StatusCode(StatusCodes.Status200OK, genericFormModel);
        }
    }
}
