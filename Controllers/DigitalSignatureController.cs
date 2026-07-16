using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using pbsamadhannetcoreapi.Models;
using pbsamadhannetcoreapi.Services.Implementations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace pbsamadhannetcoreapi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    //[Authorize(Roles ="INDL")]

    public class DigitalSignatureController : ControllerBase
    {
        private readonly UserManager<User> _userManager;
        private readonly IDigitalSignatureService _iDigitalSignatureService;
        public DigitalSignatureController(UserManager<User> userManager, IDigitalSignatureService iDigitalSignatureService)
        {
            _userManager = userManager;
            _iDigitalSignatureService = iDigitalSignatureService;
        }

        [Route("register")]
        [HttpPost]
        public async Task<IActionResult> Register([FromForm] string requestData)
        {
            //ClaimsPrincipal currentUser = this.User;
            //var currentUserName = currentUser.FindFirst(ClaimTypes.NameIdentifier).Value;
            //User user = await _userManager.FindByNameAsync(currentUserName);
            
            var user = await _userManager.FindByNameAsync(User.Identity.Name);
            await _iDigitalSignatureService.Regiater(requestData, user);
            //user.UserProfileMapping.UserProfile.DigitalSignature;

            //GenericServiceResultTemplate genericServiceResultTemplate = await _iEstablishmentService.AddUpdate_GeneralDetail(requestData);
            //if (genericServiceResultTemplate.HasException)
            //{
            //    return StatusCode(StatusCodes.Status500InternalServerError, genericServiceResultTemplate.Exceptions.Message);
            //}
            //else if (!genericServiceResultTemplate.CustomeValidationResult.IsValid)
            //{
            //    return StatusCode(StatusCodes.Status400BadRequest, genericServiceResultTemplate.CustomeValidationResult.ValidationErrorMessages);
            //}
            return StatusCode(StatusCodes.Status200OK);
        }

        [Route("verify")]
        [HttpPost]
        public async Task<IActionResult> Verify([FromForm] string requestData)
        {

            await _iDigitalSignatureService.Verify(null, requestData);
            //ClaimsPrincipal currentUser = this.User;
            //var currentUserName = currentUser.FindFirst(ClaimTypes.NameIdentifier).Value;
            //User user = await _userManager.FindByNameAsync(currentUserName);

            //var user = await _userManager.FindByNameAsync(User.Identity.Name);
            //await _iDigitalSignatureService.Regiater(requestData, user);
            //user.UserProfileMapping.UserProfile.DigitalSignature;

            //GenericServiceResultTemplate genericServiceResultTemplate = await _iEstablishmentService.AddUpdate_GeneralDetail(requestData);
            //if (genericServiceResultTemplate.HasException)
            //{
            //    return StatusCode(StatusCodes.Status500InternalServerError, genericServiceResultTemplate.Exceptions.Message);
            //}
            //else if (!genericServiceResultTemplate.CustomeValidationResult.IsValid)
            //{
            //    return StatusCode(StatusCodes.Status400BadRequest, genericServiceResultTemplate.CustomeValidationResult.ValidationErrorMessages);
            //}
            return StatusCode(StatusCodes.Status200OK);
        }

        [Route("getuidpid")]
        [HttpGet]
        public async Task<IActionResult> Get_UidPid([FromQuery] Int64 id)
        {
            //var user = await _userManager.FindByNameAsync(User.Identity.Name);
            //string uidPid = await _iDigitalSignatureService.Get_UidPid(user);

            //if(uidPid != null)
            {
                return Ok("hghghhhghg");
            }
            //return BadRequest("No digital signature found");
        }
    }
}
