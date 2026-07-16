using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using pbsamadhannetcoreapi.Models;
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
    [CustomFillters.AuthorizeAttribute("LB1N,DEVTEAM,HELPDESK")]
    public class ToDoManagerController : ControllerBase
    {

        private IToDoManagerService _iToDoManagerService;
        public ToDoManagerController(IToDoManagerService iToDoManagerService)
        {
            _iToDoManagerService = iToDoManagerService;
        }
        [HttpGet, Route("getUserWiseActivities")]
        public async Task<IActionResult> getUserWiseActivities()
        {
            GenericResponseTemplateModel<List<ToDoUserWiseActivityViewModel>> genericFormModel = await _iToDoManagerService.GetUserWiseActivities();
            if (genericFormModel.HasError)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, genericFormModel.ErrorDesc);
            }
            return StatusCode(StatusCodes.Status200OK, genericFormModel);
        }

        [HttpGet, Route("getActivitiesByRootActivityId")]
        public async Task<IActionResult> GetActivitiesByRootActivityId([FromQuery] DataTableParamsViewModel dataTableParams)
        {
            GenericResponseTemplateModel<List<ToDoActivityLogViewModel>> genericFormModel = await _iToDoManagerService.GetActivitiesByRootActivityId(dataTableParams);
            if (genericFormModel.HasError)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, genericFormModel.ErrorDesc);
            }
            return StatusCode(StatusCodes.Status200OK, genericFormModel);
        }

        [HttpGet, Route("getActivitiesWiseSteps")]
        public async Task<IActionResult> GetActivitiesWiseSteps(string rootActivityRefId)
        {
            GenericResponseTemplateModel<List<ToDoActivityWiseStepViewModel>> genericFormModel = await _iToDoManagerService.GetActivitiesWiseSteps(rootActivityRefId);
            if (genericFormModel.HasError)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, genericFormModel.ErrorDesc);
            }
            return StatusCode(StatusCodes.Status200OK, genericFormModel);
        }

        [HttpGet, Route("getAssignedTickets")]
        public async Task<IActionResult> GetAssignedTickets()
        {
            GenericResponseTemplateModel<List<ToDoTicketDetailViewModel>> genericFormModel = await _iToDoManagerService.GetAssignedTicketsByManpowerUserId("75878B38-A32E-45A2-963F-E002EE4C4416",ToDoTicketStatusTypeEnum.OPEN);
            if (genericFormModel.HasError)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, genericFormModel.ErrorDesc);
            }
            return StatusCode(StatusCodes.Status200OK, genericFormModel);
        }

        [HttpGet, Route("getActivityMappingList")]
        public async Task<IActionResult> GetActivityMappingList()
        {
            GenericResponseTemplateModel<List<ToDoApplicationActivityMaping>> genericFormModel = await _iToDoManagerService.GetActivityMappingList();
            if (genericFormModel.HasError)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, genericFormModel.ErrorDesc);
            }
            return StatusCode(StatusCodes.Status200OK, genericFormModel);
        }

    }
}
