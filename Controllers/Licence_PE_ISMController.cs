using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using pbsamadhannetcoreapi.Models;
using pbsamadhannetcoreapi.Services.Implementations;
using pbsamadhannetcoreapi.ViewModels;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;



namespace pbsamadhannetcoreapi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class Licence_PE_ISMController : ControllerBase
    {
        private ILicence_PE_ISMService _iLicence_PE_ISMService;
        private readonly UserManager<User> _userManager;
        private IThirdPartyInegrationsService _iThirdPartyInegrationsService;
        private IAuthService _iAuthService;
        public Licence_PE_ISMController(ILicence_PE_ISMService iLicence_PE_ISMService, UserManager<User> userManager, IThirdPartyInegrationsService iThirdPartyInegrationsService, IAuthService authService)
        {
            _iLicence_PE_ISMService = iLicence_PE_ISMService;
            _userManager = userManager;
            _iThirdPartyInegrationsService = iThirdPartyInegrationsService;
            _iAuthService = authService;
        }
        #region General Details
        [HttpGet, Route("getgeneraldetail")]
        [CustomFillters.AuthorizeAttribute("INDL")]
        public async Task<IActionResult> Get_GeneralDetail([FromQuery] Int64 id, Int64 projectSiteId)

        {
            GenericFormModel<Licence_PE_ISM_GeneralDetail> genericFormModel = await _iLicence_PE_ISMService.GetISMGeneralDetail(id, projectSiteId);
            if (genericFormModel.HasError)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, genericFormModel.ErrorDesc);
            }
            return StatusCode(StatusCodes.Status200OK, genericFormModel);
        }
        #endregion


        #region Get Contractor General Detail
        [HttpGet, Route("getContractorgeneralDetail")]
        [CustomFillters.AuthorizeAttribute("INDL")]
        public async Task<IActionResult> Get_ISM_Contractor_General_Detail([FromQuery] Int64 id, Int64 projectSiteId)
        {
            GenericFormModel<Licence_PE_ISM_ContractorDetailViewModel> genericFormModel = await _iLicence_PE_ISMService.Get_ISM_Contractor_General_Detail(id, projectSiteId);
            if (genericFormModel.HasError)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, genericFormModel.ErrorDesc);
            }
            return StatusCode(StatusCodes.Status200OK, genericFormModel);
        }
        #endregion


        #region Get Contractor List
        [HttpGet, Route("getContractorlist")]
        [CustomFillters.AuthorizeAttribute("INDL")]
        public async Task<IActionResult> Get_ISM_Contractor_List([FromQuery] Int64 appRefId)
        {
            GenericFormModel<List<Licence_PE_ISM_Contrator>> genericFormModel = await _iLicence_PE_ISMService.Get_ISM_Contractor_List(appRefId);
            if (genericFormModel.HasError)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, genericFormModel.ErrorDesc);
            }
            return StatusCode(StatusCodes.Status200OK, genericFormModel);
        }
        #endregion

        #region Remove Contractor
        [HttpGet, Route("removeContractor")]
        [CustomFillters.AuthorizeAttribute("INDL")]
        public async Task<IActionResult> RemoveContractor([FromQuery] Int64 id)
        {
            GenericResponseTemplateModel<bool> genericFormModel = await _iLicence_PE_ISMService.RemoveContractor(id);
            if (genericFormModel.HasError)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, genericFormModel.ErrorDesc);
            }
            return StatusCode(StatusCodes.Status200OK, genericFormModel);
        }
        #endregion

        #region Get Licence ISM CL PE Detail
        [HttpGet, Route("getLicenceISMdetail")]
        public async Task<IActionResult> Get_Licence_ISM_Detail([FromQuery] Int64 id)
        {
            GenericFormModel<Licence_PE_ISMViewModel> genericFormModel = await _iLicence_PE_ISMService.Get_Licence_ISM_Detail(id);

            if (genericFormModel.HasError)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, genericFormModel.ErrorDesc);
            }
            return StatusCode(StatusCodes.Status200OK, genericFormModel);
        }
        #endregion

    }
}
