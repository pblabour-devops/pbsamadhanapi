using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using pbsamadhannetcoreapi.CommonUtiliteis;
using pbsamadhannetcoreapi.Models;
using pbsamadhannetcoreapi.Services.Implementations;
using pbsamadhannetcoreapi.ViewModels;

namespace pbsamadhannetcoreapi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProjectSiteController : ControllerBase
    {
        private IProjectSiteService _iProjectSiteService;
        private readonly UserManager<User> _userManager;
        private IAuthService _iAuthService;
        public ProjectSiteController(IProjectSiteService iprojectSiteService, UserManager<User> userManager, IAuthService iAuthService)
        {
            _iProjectSiteService = iprojectSiteService;
            _userManager = userManager;
            _iAuthService = iAuthService;
        }

        #region Project Site
        [HttpGet, Route("getprojectsiteDetails")]
        public async Task<IActionResult> Get_ProjectSiteDetail([FromQuery] Int64 id)
        {
            GenericFormModel<ProjectSite> genericFormModel = await _iProjectSiteService.GetProjectSiteDetail(id);
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

        [Route("addupdate_projectsitedetails")]
        [HttpPost]
        public async Task<IActionResult> AddUpdate_ProjectSiteDetail([FromBody] ProjectSite requestData)
        {
            var userClaims = await _iAuthService.DecryptLoggedInUserClaims(User.Claims);
            GenericServiceResultTemplate genericServiceResultTemplate = await _iProjectSiteService.AddUpdate_ProjectSiteDetail(requestData, userClaims.UserId);
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

        [HttpGet, Route("getcurrentuserallprojectsites")]
        public async Task<IActionResult> Get_CurrentUserAllProjectSites()
        {
            var userClaims = await _iAuthService.DecryptLoggedInUserClaims(User.Claims);
            GenericFormModel<List<ProjectProfileViewModel>> genericFormModel = await _iProjectSiteService.Get_CurrentUserAllProjectSites(userClaims.UserId);
            if (genericFormModel.HasError)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, genericFormModel.ErrorDesc);
            }
            return StatusCode(StatusCodes.Status200OK, genericFormModel);
        }

        [HttpGet, Route("getProjectsitesByProfileSiteId")]
        public async Task<IActionResult> Get_ProjectsitesByProfileSiteId([FromQuery] Int64 projectSiteId, Int64 appRefId, int projectSiteVersion)
        {
            GenericResponseTemplateModel<ProjectSitesViewModel> genericResponse = await _iProjectSiteService.Get_ProjectSiteByProjectSiteId(projectSiteId, appRefId, projectSiteVersion);
            if (genericResponse.HasError)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, genericResponse.ErrorDesc);
            }
            return StatusCode(StatusCodes.Status200OK, genericResponse);
        }

        [Route("update_ProjectSiteLogDetails")]
        [HttpPost]
        public async Task<IActionResult> Update_ProjectSiteLogDetails([FromBody] ProjectSiteEstablishmentBasicDetailsViewModel requestData)
        {
            GenericServiceResultTemplate genericServiceResultTemplate = await _iProjectSiteService.Update_ProjectSiteLogDetails(requestData);
            if (genericServiceResultTemplate.HasException)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, genericServiceResultTemplate.Exceptions.Message);
            }
            return StatusCode(StatusCodes.Status200OK, genericServiceResultTemplate);
        }
        #endregion
    }
}
