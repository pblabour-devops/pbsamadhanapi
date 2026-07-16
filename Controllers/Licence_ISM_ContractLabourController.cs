using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using pbsamadhannetcoreapi.Models;
using pbsamadhannetcoreapi.Services.Implementations;
using pbsamadhannetcoreapi.ViewModels;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
namespace pbsamadhannetcoreapi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class Licence_ISM_ContractLabourController : ControllerBase
    {
        private ILicence_ISM_ContractLabourService _iLicence_ISM_ContractLabourService;
        private readonly UserManager<User> _userManager;
        private IThirdPartyInegrationsService _iThirdPartyInegrationsService;
        private IAuthService _iAuthService;
        public Licence_ISM_ContractLabourController(ILicence_ISM_ContractLabourService iLicence_ISM_ContractLabourService, UserManager<User> userManager, IThirdPartyInegrationsService iThirdPartyInegrationsService, IAuthService authService)
        {
            _iLicence_ISM_ContractLabourService = iLicence_ISM_ContractLabourService;
            _userManager = userManager;
            _iThirdPartyInegrationsService = iThirdPartyInegrationsService;
            _iAuthService = authService;
        }

        #region General Details
        [HttpGet, Route("getismgeneraldetail")]
        [CustomFillters.AuthorizeAttribute("INDL")]
        public async Task<IActionResult> Get_ISM_GeneralDetail([FromQuery] Int64 id, Int64 projectSiteId)

        {
            GenericFormModel<Licence_ISM_ContractLabour_GeneralDetail> genericFormModel = await _iLicence_ISM_ContractLabourService.GetISMContractLabourGeneralDetail(id, projectSiteId);
            if (genericFormModel.HasError)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, genericFormModel.ErrorDesc);
            }
            return StatusCode(StatusCodes.Status200OK, genericFormModel);
        }
        #endregion

        #region Get Principal Employer Details
        [HttpGet, Route("getismPrincipalEmployerDetails")]
        [CustomFillters.AuthorizeAttribute("INDL")]
        public async Task<IActionResult> GetISMPrincipalEmployerDetails([FromQuery] string licencenumber)
        {
            GenericResponseTemplateModel<Licence_CL_PE_GeneralDetail_ViewModel> genericFormModel = await _iLicence_ISM_ContractLabourService.GetISMPrincipalEmployerDetails(licencenumber);
            if (genericFormModel.HasError)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, genericFormModel.ErrorDesc);
            }
            return StatusCode(StatusCodes.Status200OK, genericFormModel);
        }
        #endregion


        #region Get PE Contractor List
        [HttpGet, Route("getismpeContractorlist")]
        [CustomFillters.AuthorizeAttribute("INDL")]
        public async Task<IActionResult> Get_ISM_PE_Contractor_List([FromQuery] Int64 generalDetailRefId)
        {
            GenericFormModel<List<Licence_PE_ISM_Contrator>> genericFormModel = await _iLicence_ISM_ContractLabourService.Get_ISM_PE_Contractor_List(generalDetailRefId);
            if (genericFormModel.HasError)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, genericFormModel.ErrorDesc);
            }
            return StatusCode(StatusCodes.Status200OK, genericFormModel);
        }

        [HttpGet, Route("getISMpeContractorlistbyid")]
        [CustomFillters.AuthorizeAttribute("INDL")]
        public async Task<IActionResult> Get_ISM_PE_Contractor_List_ById([FromQuery] Int64 id)
        {
            GenericResponseTemplateModel<Licence_PE_ISM_Contrator> genericFormModel = await _iLicence_ISM_ContractLabourService.Get_ISM_PE_Contractor_List_byId(id);
            if (genericFormModel.HasError)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, genericFormModel.ErrorDesc);
            }
            return StatusCode(StatusCodes.Status200OK, genericFormModel);
        }
        #endregion


        #region Get Licence Contractor Labour Detail
        [HttpGet, Route("getLicenceContractLabourdetail")]
        public async Task<IActionResult> Get_Licence_ISM_Contract_Labour_Detail([FromQuery] Int64 id, Int64 psid)
        {
            GenericFormModel<Licence_ISM_Contract_LabourViewModel> genericFormModel = await _iLicence_ISM_ContractLabourService.Get_Licence_ISM_ContractLabour_Detail(id);

            if (genericFormModel.HasError)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, genericFormModel.ErrorDesc);
            }
            return StatusCode(StatusCodes.Status200OK, genericFormModel);
        }
        #endregion



    }
}
