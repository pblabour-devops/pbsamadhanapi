using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using pbsamadhannetcoreapi.CommonUtiliteis;
using pbsamadhannetcoreapi.Models;
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
    [Authorize]
    public class BuildingPlanController : ControllerBase
    {
        private IBuildingPlanService _iBuildingPlanService;
        private IAuthService _iAuthService;
        public BuildingPlanController(IBuildingPlanService iBuildingPlanService, IAuthService iAuthService)
        {
            _iBuildingPlanService = iBuildingPlanService;
            _iAuthService = iAuthService;
        }

        #region Building Plan 

        [HttpGet, Route("getbuildingplandetail")]
        [CustomFillters.AuthorizeAttribute("INDL")]
        public async Task<IActionResult> Get_GeneralDetail([FromQuery] Int64 id, Int64 projectSiteId)
        {
            GenericFormModel<BuildingPlan> genericFormModel = await _iBuildingPlanService.GetBuildingPlanDetail(id, projectSiteId);
            if (genericFormModel.HasError)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, genericFormModel.ErrorDesc);
            }
            return StatusCode(StatusCodes.Status200OK, genericFormModel);
        }

        [Route("addupdate_buildingplandetail")]
        [HttpPost]
        [CustomFillters.AuthorizeAttribute("INDL")]
        public async Task<IActionResult> AddUpdate_GeneralDetail([FromBody] BuildingPlan requestData)
        {
            var userClaims = await _iAuthService.DecryptLoggedInUserClaims(User.Claims);
            GenericServiceResultTemplate genericServiceResultTemplate = await _iBuildingPlanService.AddUpdate_BuildingPlanDetail(requestData, userClaims.UserName);
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

        #endregion Building Plan

        #region Building Plan AreaDetail

        [HttpGet, Route("getBuildingPlanAreaDetail")]
        [CustomFillters.AuthorizeAttribute("INDL")]
        public async Task<IActionResult> Get_BuildingPlan_AreaDetail([FromQuery] Int64 id)
        {
            GenericFormModel<List<BuildingPlan_AreaDetail>> genericFormModel = await _iBuildingPlanService.GetBuildingPlanAreaDetail(id);
            if (genericFormModel.FormModel == null && id != 0)
            {
                GenericServiceResultTemplate genericServiceResultTemplate = new GenericServiceResultTemplate();
                genericServiceResultTemplate.CustomeValidationResult = new CustomeValidationResult();
                genericServiceResultTemplate.CustomeValidationResult.IsValid = false;
                genericServiceResultTemplate.CustomeValidationResult.ValidationErrorMessages = "Invalid building plan identity";
                return StatusCode(StatusCodes.Status400BadRequest, genericServiceResultTemplate.CustomeValidationResult.ValidationErrorMessages);
            }
            return Ok(genericFormModel);
        }

        [Route("addupdateBuildingPlanAreaDetail")]
        [HttpPost]
        [CustomFillters.AuthorizeAttribute("INDL")]
        public async Task<IActionResult> AddUpdate_BuildingPlan_AreaDetail([FromBody] BuildingPlan_AreaDetail requestData)
        {
            GenericServiceResultTemplate genericServiceResultTemplate = await _iBuildingPlanService.AddUpdateBuildingPlanAreaDetail(requestData);
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

        #endregion Building Plan AreaDetail

        #region Building Plan Details

        [HttpGet, Route("getdetail")]
        public async Task<IActionResult> Get_Detail([FromQuery] Int64 id, Int64 psid)
        {
            GenericFormModel<BuildingPlanViewModels> genericFormModel = await _iBuildingPlanService.GetBuildingPlanDetail(id);
            if (genericFormModel.HasError)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, genericFormModel.ErrorDesc);
            }
            return StatusCode(StatusCodes.Status200OK, genericFormModel);
        }
        #endregion Building Plan Details

        #region Lock Application

        [Route("lockapplication")]
        [HttpPost]
        public async Task<IActionResult> LockApplication([FromBody] ApplicationLockParmsViewModel applicationLockParms)
        {
            GenericServiceResultTemplate genericServiceResultTemplate = await _iBuildingPlanService.LockApplication(applicationLockParms.AppId, applicationLockParms.AppActionType, applicationLockParms.Remarks);
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
    }
}
