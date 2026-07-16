using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using pbsamadhannetcoreapi.ViewModels;
using pbsamadhannetcoreapi.Models;
using Newtonsoft.Json;
using pbsamadhannetcoreapi.Services.Implementations;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using pbsamadhannetcoreapi.CommonUtiliteis;

namespace pbsamadhannetcoreapi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
   
    public class CrudController : ControllerBase
    {
        private readonly ICrudService _iCrudService;
        private readonly UserManager<User> _userManager;
        private IAuthService _iAuthService;
        public CrudController(ICrudService iCrudService, UserManager<User> userManager, IAuthService iAuthService)
        {
            _iCrudService = iCrudService;
            _userManager = userManager;
            _iAuthService = iAuthService;
        }

        [Route("CreateUpdate")]
        [HttpPost]
        //[CustomFillters.AuthorizeAttribute("INDL")]
        public async Task<IActionResult> CreateUpdate([FromBody] XhrRequestDataParmsViewModel requestData)
        {
            var user = new User();
            var userClaims = await _iAuthService.DecryptLoggedInUserClaims(User.Claims);
            if (userClaims.UserName != null)
            {
                user = await _userManager.FindByNameAsync(userClaims.UserName);
            }
            else
            {
                user = await _userManager.FindByNameAsync("NoUser");
            }
            var designatedObject = JsonConvert.DeserializeObject(requestData.Data, Type.GetType(requestData.DesignatedModel));
            CRUD_CreateUpdateOperationResponse resp  = await _iCrudService.CreateUpdate(designatedObject, user);
            resp.IsCrudService = true;
            if (resp.HasExceptions)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, resp);
            }
            //else if (!crudResp.CustomeValidationResult.IsValid)
            //{
            //    return StatusCode(StatusCodes.Status400BadRequest, genericServiceResultTemplate.CustomeValidationResult.ValidationErrorMessages);
            //}
            return StatusCode(StatusCodes.Status200OK, resp);
        }


        [Route("officerCreateUpdate")]
        [HttpPost]
        [CustomFillters.AuthorizeAttribute(AuthConfigs.AllLabourOfficialRoles)]
        public async Task<IActionResult> OfficerCreateUpdate([FromBody] XhrRequestDataParmsViewModel requestData)
        {
            var user = new User();
            var userClaims = await _iAuthService.DecryptLoggedInUserClaims(User.Claims);
            if (userClaims.UserName != null)
            {
                user = await _userManager.FindByNameAsync(userClaims.UserName);
            }
            else
            {
                user = await _userManager.FindByNameAsync("NoUser");
            }
            var designatedObject = JsonConvert.DeserializeObject(requestData.Data, Type.GetType(requestData.DesignatedModel));
            CRUD_CreateUpdateOperationResponse resp = await _iCrudService.OfficerCreateUpdate(designatedObject, user);
            resp.IsCrudService = true;
            if (resp.HasExceptions)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, resp);
            }
            return StatusCode(StatusCodes.Status200OK, resp);
        }
    }
}
