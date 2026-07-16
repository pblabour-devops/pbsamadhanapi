using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using pbsamadhannetcoreapi.Models;
using pbsamadhannetcoreapi.Services.Implementations;
using pbsamadhannetcoreapi.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Primitives;
using System.IO;
using Newtonsoft.Json;
using pbsamadhannetcoreapi.CommonUtiliteis;


namespace pbsamadhannetcoreapi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InspectionController : ControllerBase
    {
        public IInspectionService _iInspectionService;
        private readonly UserManager<User> _userManager;
        private IAuthService _iAuthService;
        public InspectionController(IInspectionService iInspectionService, UserManager<User> userManager, IAuthService iAuthService)
        {
            _iInspectionService = iInspectionService;
            _userManager = userManager;
            _iAuthService = iAuthService;
        }

        [HttpGet, Route("GetInspection_Randomization")]
        public async Task<IActionResult> GetInspection_Randomization([FromQuery] Int64 id, string searchCode, int pageNo, int pageSize, string sortColumn, string sortOrder, string filterArray)
        {
            var userClaims = await _iAuthService.DecryptLoggedInUserClaims(User.Claims);
            GenericResponseTemplateModel<List<Inspection_RandomizeDashboardDataViewModel>> genericFormModel = await _iInspectionService.GetInspection_Randomization(userClaims.UserId, id,searchCode, pageNo, pageSize, sortColumn, sortOrder, filterArray);

            if (genericFormModel.HasError)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, genericFormModel.ErrorDesc);
            }
            return StatusCode(StatusCodes.Status200OK, genericFormModel);
        }

        [HttpGet, Route("get_inspectionsByRandomization")]
        public async Task<IActionResult> Get_InspectionsByRandomization([FromQuery] Int64 id, Int64 factoryRefId, string roleName,string userRefId)
            {
            GenericResponseTemplateModel<List<InspectioninfoViewModel>> genericFormModel = await _iInspectionService.Get_InspectionsByRandomization(id, factoryRefId, roleName, userRefId);
            if (genericFormModel.HasError)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, genericFormModel.ErrorDesc);
            }
            return StatusCode(StatusCodes.Status200OK, genericFormModel);
        }

        [HttpGet, Route("GetInspection_LabourCircleUserDetails")]
        public async Task<IActionResult> GetInspection_LabourCircleUserDetails([FromQuery] Int64 id, Int64 labourWingProfileRefId)
        {
            GenericFormModel<LabourCircleUserDetailsViewModel> genericFormModel = await _iInspectionService.GetInspection_LabourCircleUserDetails(id, labourWingProfileRefId);
            if (genericFormModel.HasError)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, genericFormModel.ErrorDesc);
            }
            return StatusCode(StatusCodes.Status200OK, genericFormModel);
        }

        [HttpGet, Route("GetForm_Factory_Part_I_General")]
        public async Task<IActionResult> GetForm_Factory_Part_I_General([FromQuery] Int64 id)
        {
            GenericFormModel<Inspection_Form_Factory_Part_I_General> genericFormModel = await _iInspectionService.GetIncpectionFormFactoryPartIGeneralDetail(id);
            if (genericFormModel.HasError)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, genericFormModel.ErrorDesc);
            }
            return StatusCode(StatusCodes.Status200OK, genericFormModel);
        }
        [HttpGet, Route("GetForm_Factory_Part_II_FactoryDetail")]
        public async Task<IActionResult> GetForm_Factory_Part_II_FactoryDetail([FromQuery] Int64 id)
        {
            GenericFormModel<Inspection_Form_Factory_Part_II_FactoryDetail> genericFormModel = await _iInspectionService.GetIncpectionFormFactoryPartIIFactoryDetail(id);
            if (genericFormModel.HasError)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, genericFormModel.ErrorDesc);
            }
            return StatusCode(StatusCodes.Status200OK, genericFormModel);
        }

        [HttpGet, Route("GetForm_Factory_Part_III_InspectionReport")]
        public async Task<IActionResult> GetForm_Factory_Part_III_InspectionReport([FromQuery] Int64 id)
        {
            GenericFormModel<Inspection_Form_Factory_Part_III_InspectionReport> genericFormModel = await _iInspectionService.GetIncpectionFormFactoryPartIIIInspectionReport(id);
            if (genericFormModel.HasError)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, genericFormModel.ErrorDesc);
            }
            return StatusCode(StatusCodes.Status200OK, genericFormModel);
        }

        [Route("getForm_Factory_Part_III_Dangerousoperation")]
        [HttpGet]
        public async Task<IActionResult> getForm_Factory_Part_III_Dangerousoperation([FromQuery] Int64 id)
        {
            GenericFormModel<Inspection_Form_Factory_Part_III_DangerousOperation> genericFormModel = await _iInspectionService.GetForm_Factory_Part_III_Dangerousoperation(id);
            if (genericFormModel.HasError)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, genericFormModel.ErrorDesc);
            }
            return StatusCode(StatusCodes.Status200OK, genericFormModel);
        }


        [HttpGet, Route("GetForm_Factory_Part_III_MusterRoll")]
        public async Task<IActionResult> GetForm_Factory_Part_III_MusterRoll([FromQuery] Int64 id)
        {
            GenericResponseTemplateModel<List<Inspection_Form_Factory_Part_III_MusterRoll>> genericFormModel = await _iInspectionService.GetIncpectionFormFactoryPartIIIMusterRoll(id);

            if (genericFormModel.HasError)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, genericFormModel.ErrorDesc);
            }
            return StatusCode(StatusCodes.Status200OK, genericFormModel);
        }

        [HttpGet, Route("GetForm_Factory_Part_III_Health")]
        public async Task<IActionResult> GetForm_Factory_Part_III_Health([FromQuery] Int64 id)
        {
            GenericFormModel<Inspection_Form_Factory_Part_III_Health> genericFormModel = await _iInspectionService.GetIncpectionFormFactoryPartIIIHealth(id);

            if (genericFormModel.HasError)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, genericFormModel.ErrorDesc);
            }
            return StatusCode(StatusCodes.Status200OK, genericFormModel);
        }

        [HttpGet, Route("GetForm_Factory_Part_III_Safety")]
        public async Task<IActionResult> GetForm_Factory_Part_III_Safety([FromQuery] Int64 id)
        {
            GenericFormModel<Inspection_Form_Factory_Part_III_Safety> genericFormModel = await _iInspectionService.GetIncpectionFormFactoryPartIIISafety(id);

            if (genericFormModel.HasError)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, genericFormModel.ErrorDesc);
            }
            return StatusCode(StatusCodes.Status200OK, genericFormModel);
        }

        [HttpGet, Route("GetForm_Factory_Part_III_Welfare")]
        public async Task<IActionResult> GetForm_Factory_Part_III_Welfare([FromQuery] Int64 id)
        {
            GenericFormModel<Inspection_Form_Factory_Part_III_Welfare> genericFormModel = await _iInspectionService.GetIncpectionGetFormFactoryPartIIIWelfare(id);

            if (genericFormModel.HasError)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, genericFormModel.ErrorDesc);
            }
            return StatusCode(StatusCodes.Status200OK, genericFormModel);
        }

        [HttpGet, Route("GetForm_Factory_Part_III_General")]
        public async Task<IActionResult> GetForm_Factory_Part_III_General([FromQuery] Int64 id)
        {
            GenericFormModel<Inspection_Form_Factory_Part_III_General> genericFormModel = await _iInspectionService.GetIncpectionGetFormFactoryPartIIIGeneral(id);

            if (genericFormModel.HasError)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, genericFormModel.ErrorDesc);
            }
            return StatusCode(StatusCodes.Status200OK, genericFormModel);
        }

        [HttpGet, Route("GetForm_Factory_Part_III_MajorAccidentHazard")]
        public async Task<IActionResult> GetForm_Factory_Part_III_MajorAccidentHazard([FromQuery] Int64 id)
        {
            GenericFormModel<Inspection_Form_Factory_Part_III_MajorAccidentHazard> genericFormModel = await _iInspectionService.GetIncpectionGetFormFactoryPartIIIMajorAccidentHazard(id);

            if (genericFormModel.HasError)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, genericFormModel.ErrorDesc);
            }
            return StatusCode(StatusCodes.Status200OK, genericFormModel);
        }
        
        [Route("AddUpdateForm_Factory_Part_I_General")]
        [HttpPost]
        public async Task<IActionResult> AddUpdateForm_Factory_Part_I_General([FromBody] Inspection_Form_Factory_Part_I_General requestData)
        {
            GenericServiceResultTemplate genericServiceResultTemplate = await _iInspectionService.AddUpdate_IncpectionFormFactoryPartIGeneralDetail(requestData);
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

        [Route("AddUpdateForm_Factory_Part_II_FactoryDetail")]
        [HttpPost]
        public async Task<IActionResult> AddUpdateForm_Factory_Part_II_FactoryDetail([FromBody] Inspection_Form_Factory_Part_II_FactoryDetail requestData)
        {
            GenericServiceResultTemplate genericServiceResultTemplate = await _iInspectionService.AddUpdate_IncpectionFormFactoryPartIIFactoryDetail(requestData);
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

        [Route("AddUpdateForm_Factory_Part_III_InspectionReport")]
        [HttpPost]
        public async Task<IActionResult> AddUpdateForm_Factory_Part_III_InspectionReport([FromBody] Inspection_Form_Factory_Part_III_InspectionReport requestData)
        {
            GenericServiceResultTemplate genericServiceResultTemplate = await _iInspectionService.AddUpdate_IncpectionFormFactoryPartIIIInspectionReport(requestData);
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

        [Route("AddUpdateForm_Factory_Part_III_MusterRoll")]
        [HttpPost]
        public async Task<IActionResult> AddUpdateForm_Factory_Part_III_MusterRoll([FromBody] Inspection_Form_Factory_Part_III_MusterRoll requestData)
        {
            GenericServiceResultTemplate genericServiceResultTemplate = await _iInspectionService.AddUpdate_IncpectionFormFactoryPartIIIMusterRoll(requestData);
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

        [Route("AddUpdateForm_Factory_Part_III_Health")]
        [HttpPost]
        public async Task<IActionResult> AddUpdateForm_Factory_Part_III_Health([FromBody] Inspection_Form_Factory_Part_III_Health requestData)
        {
            GenericServiceResultTemplate genericServiceResultTemplate = await _iInspectionService.AddUpdate_InspectionFormFactoryPartIIIHealth(requestData);
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

        [Route("AddUpdateForm_Factory_Part_III_Safety")]
        [HttpPost]
        public async Task<IActionResult> AddUpdateForm_Factory_Part_III_Safety([FromBody] Inspection_Form_Factory_Part_III_Safety requestData)
        {
            GenericServiceResultTemplate genericServiceResultTemplate = await _iInspectionService.AddUpdate_InspectionFormFactoryPartIIISafety(requestData);
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

        [Route("AddUpdateForm_Factory_Part_III_Welfare")]
        [HttpPost]
        public async Task<IActionResult> AddUpdateForm_Factory_Part_III_Welfare([FromBody] Inspection_Form_Factory_Part_III_Welfare requestData)
        {
            GenericServiceResultTemplate genericServiceResultTemplate = await _iInspectionService.AddUpdate_IncpectionFormFactoryPartIIIWelfare(requestData);
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

        [Route("AddUpdateForm_Factory_Part_III_General")]
        [HttpPost]
        public async Task<IActionResult> AddUpdateForm_Factory_Part_III_General([FromBody] Inspection_Form_Factory_Part_III_General requestData)
        {
            GenericServiceResultTemplate genericServiceResultTemplate = await _iInspectionService.AddUpdate_IncpectionFormFactoryPartIIIGeneral(requestData);
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

        [Route("AddUpdateForm_Factory_Part_III_MajorAccidentHazard")]
        [HttpPost]
        public async Task<IActionResult> AddUpdateForm_Factory_Part_III_MajorAccidentHazard([FromBody] Inspection_Form_Factory_Part_III_MajorAccidentHazard requestData)
        {
            GenericServiceResultTemplate genericServiceResultTemplate = await _iInspectionService.AddUpdate_IncpectionFormFactoryPartIIIMajorAccidentHazard(requestData);
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

        [Route("addUpdateForm_Factory_Part_III_Dangerousoperation")]
        [HttpPost]
        public async Task<IActionResult> AddUpdateForm_Factory_Part_III_Dangerousoperation([FromBody] Inspection_Form_Factory_Part_III_DangerousOperation requestData)
        {
            GenericServiceResultTemplate genericServiceResultTemplate = await _iInspectionService.AddUpdateForm_Factory_Part_III_Dangerousoperation(requestData);
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

        [HttpGet, Route("Update_Inspection_MasterUsingInpectionId")]
        public async Task<IActionResult> Update_Inspection_MasterUsingInpectionId([FromQuery] Int64 inspectionId, Boolean hasAssignedLabourCircle, Int64 labourCircleRefId,Int64 labourWingProfileRefId)
        {
            GenericResponseTemplateModel<bool> genericFormModel  = await _iInspectionService.Update_InspectionMasterUsingInpectionId(inspectionId,hasAssignedLabourCircle, labourCircleRefId, labourWingProfileRefId);
            if (genericFormModel.HasError)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, genericFormModel.ErrorDesc);
            }
            return StatusCode(StatusCodes.Status200OK, genericFormModel);
        }

        [HttpGet, Route("Transfer_Inspection_MasterUsingInspectionId")]
        public async Task<IActionResult> Transfer_Inspection_MasterUsingInspectionId([FromQuery] Int64 inspectionId, Boolean hasAssignedLabourCircle, Int64 circleRefId, string senderUserId, string senderRoleId, string senderrRoleName, string receiverUserId, string receiverRoleId, Int64 receiverProfileId, string remarks, int inspectionType)
        {
            GenericResponseTemplateModel<bool> genericFormModel = await _iInspectionService.Transfer_Inspection_MasterUsingInspectionId(inspectionId, hasAssignedLabourCircle, circleRefId, senderUserId, senderRoleId, senderrRoleName, receiverUserId, receiverRoleId, receiverProfileId, remarks, inspectionType);
            if (genericFormModel.HasError)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, genericFormModel.ErrorDesc);
            }
            return StatusCode(StatusCodes.Status200OK, genericFormModel);
        }

        [Route("lock_AssignmentInfo")]
        [HttpPost]
        public async Task<IActionResult> lock_AssignmentInfo([FromBody] Inspection_LockInfo requestData)
        {
            GenericServiceResultTemplate genericServiceResultTemplate = await _iInspectionService.Lock_InspectionInfo(requestData);
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


        [HttpGet, Route("RemoveForm_Factory_Part_III_MusterRoll")]
        public async Task<IActionResult> RemoveForm_Factory_Part_III_MusterRoll([FromQuery] Int64 id)
        {
            GenericResponseTemplateModel<bool> genericFormModel = await _iInspectionService.RemoveFormFactoryPartIIIMusterRollByMusterId(id);
            if (genericFormModel.HasError)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, genericFormModel.ErrorDesc);
            }
            return StatusCode(StatusCodes.Status200OK, genericFormModel);
        }

        [HttpGet, Route("get_Inspections_FactoryPerformaStepStatus")]
        public async Task<IActionResult> Get_Inspections_FactoryPerformaStepStatus([FromQuery] Int64 inspectionRefId)
        {
            GenericResponseTemplateModel<List<GetInspections_FactoryPerformaStepStatusViewModel>> genericFormModel = await _iInspectionService.Get_Inspections_FactoryPerformaStepStatus(inspectionRefId);
            if (genericFormModel.HasError)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, genericFormModel.ErrorDesc);
            }
            return StatusCode(StatusCodes.Status200OK, genericFormModel);
        }


        //[Route("lock_AssignmentInfo")]
        //[HttpPost]
        //public async Task<IActionResult> LockForm([FromBody] Inspection_LockInfo requestData)
        //{
        //    GenericServiceResultTemplate genericServiceResultTemplate = await _iInspectionService.Lock_InspectionInfo(requestData);
        //    if (genericServiceResultTemplate.HasException)
        //    {
        //        return StatusCode(StatusCodes.Status500InternalServerError, genericServiceResultTemplate.Exceptions.Message);
        //    }
        //    else if (!genericServiceResultTemplate.CustomeValidationResult.IsValid)
        //    {
        //        return StatusCode(StatusCodes.Status400BadRequest, genericServiceResultTemplate.CustomeValidationResult.ValidationErrorMessages);
        //    }
        //    return StatusCode(StatusCodes.Status200OK, genericServiceResultTemplate);
        //}

        [HttpGet, Route("GetInspection_MasterDataByRandomizationIdWithStabblishmentNameAddress")]
        public async Task<IActionResult> GetInspection_MasterDataByRandomizationIdWithStabblishmentNameAddress([FromQuery] Int64 id, string userId)
        {
            GenericFormModel<List<Inspection_MasterDataByRandomizationIdWithStabblishmentNameAddressViewModel>> genericFormModel = await _iInspectionService.GetInspection_MasterDataByRandomizationIdWithStabblishmentNameAddress(id,userId);
            if (genericFormModel.HasError)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, genericFormModel.ErrorDesc);
            }
            return StatusCode(StatusCodes.Status200OK, genericFormModel);
        }

        #region Labour Inspections
        [HttpGet, Route("GetForm_Labour_Part_I_General")]
        public async Task<IActionResult> GetForm_Labour_Part_I_General([FromQuery] Int64 id)
        {
            GenericFormModel<Inspection_Form_Labour_Part_I_General> genericFormModel = await _iInspectionService.GetInspectionFormLabourPartIGeneralDetail(id);
            if (genericFormModel.HasError)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, genericFormModel.ErrorDesc);
            }
            return StatusCode(StatusCodes.Status200OK, genericFormModel);
        }

        [Route("AddUpdateForm_Labour_Part_I_General")]
        [HttpPost]
        public async Task<IActionResult> AddUpdateForm_Labour_Part_I_General([FromBody] Inspection_Form_Labour_Part_I_General requestData)
        {
            GenericServiceResultTemplate genericServiceResultTemplate = await _iInspectionService.AddUpdate_InspectionFormLabourPartIGeneralDetail(requestData);
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

        [HttpGet, Route("GetForm_Labour_Part_II_FactoryDetail")]
        public async Task<IActionResult> GetForm_Labour_Part_II_FactoryDetail([FromQuery] Int64 id)
        {
            GenericFormModel<Inspection_Form_Labour_Part_II_FactoryDetail> genericFormModel = await _iInspectionService.GetInspectionFormLabourPartIIFactoryDetail(id);
            if (genericFormModel.HasError)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, genericFormModel.ErrorDesc);
            }
            return StatusCode(StatusCodes.Status200OK, genericFormModel);
        }

        [Route("AddUpdateForm_Labour_Part_II_FactoryDetail")]
        [HttpPost]
        public async Task<IActionResult> AddUpdateForm_Labour_Part_II_FactoryDetail([FromBody] Inspection_Form_Labour_Part_II_FactoryDetail requestData)
        {
            GenericServiceResultTemplate genericServiceResultTemplate = await _iInspectionService.AddUpdate_InspectionFormLabourPartIIFactoryDetail(requestData);
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

        [HttpGet, Route("GetForm_Labour_Part_III_MusterRoll")]
        public async Task<IActionResult> GetForm_Labour_Part_III_MusterRoll([FromQuery] Int64 id)
        {
            GenericResponseTemplateModel<List<Inspection_Form_Labour_Part_III_MusterRoll>> genericFormModel = await _iInspectionService.GetInspectionFormLabourPartIIIMusterRoll(id);

            if (genericFormModel.HasError)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, genericFormModel.ErrorDesc);
            }
            return StatusCode(StatusCodes.Status200OK, genericFormModel);
        }
        [HttpGet, Route("RemoveForm_Labour_Part_III_MusterRoll")]
        public async Task<IActionResult> RemoveForm_Labour_Part_III_MusterRoll([FromQuery] Int64 id)
        {
            GenericResponseTemplateModel<bool> genericFormModel = await _iInspectionService.RemoveFormLabourPartIIIMusterRollByMusterId(id)
;
            if (genericFormModel.HasError)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, genericFormModel.ErrorDesc);
            }
            return StatusCode(StatusCodes.Status200OK, genericFormModel);
        }

        [Route("AddUpdateForm_Labour_Part_III_MusterRoll")]
        [HttpPost]
        public async Task<IActionResult> AddUpdateForm_Labour_Part_III_MusterRoll([FromBody] Inspection_Form_Labour_Part_III_MusterRoll requestData)
        {
            GenericServiceResultTemplate genericServiceResultTemplate = await _iInspectionService.AddUpdate_InspectionFormLabourPartIIIMusterRoll(requestData);
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

        [HttpGet, Route("getForm_Labour_Part_III_EqualEnumerationAct")]
        public async Task<IActionResult> GetForm_Labour_Part_III_EqualEnumerationAct([FromQuery] Int64 id)
        {
            GenericFormModel<Inspection_Form_Labour_Part_III_EqualEnumerationAct> genericFormModel = await _iInspectionService.GetInspectionFormLabourPartIIIEqualEnumerationAct(id);
            if (genericFormModel.HasError)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, genericFormModel.ErrorDesc);
            }
            return StatusCode(StatusCodes.Status200OK, genericFormModel);
        }

        [Route("addUpdateForm_Labour_Part_III_EqualEnumerationAct")]
        [HttpPost]
        public async Task<IActionResult> AddUpdateForm_Labour_Part_III_EqualEnumerationAct([FromBody] Inspection_Form_Labour_Part_III_EqualEnumerationAct requestData)
        {
            GenericServiceResultTemplate genericServiceResultTemplate = await _iInspectionService.AddUpdate_InspectionFormLabourPartIIIEqualEnumerationAct(requestData);
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

        [HttpGet, Route("getForm_Labour_Part_III_MinimumWagesAct")]
        public async Task<IActionResult> GetForm_Labour_Part_III_MinimumWagesAct([FromQuery] Int64 id)
        {
            GenericFormModel<Inspection_Form_Labour_III_MinimumWageAct> genericFormModel = await _iInspectionService.GetInspectionFormLabourPartIIIMinimumWagesAct(id);
            if (genericFormModel.HasError)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, genericFormModel.ErrorDesc);
            }
            return StatusCode(StatusCodes.Status200OK, genericFormModel);
        }

        [Route("addUpdateForm_Labour_Part_III_MinimumWagesAct")]
        [HttpPost]
        public async Task<IActionResult> AddUpdateForm_Labour_Part_III_MinimumWagesAct([FromBody] Inspection_Form_Labour_III_MinimumWageAct requestData)
        {
            GenericServiceResultTemplate genericServiceResultTemplate = await _iInspectionService.AddUpdate_InspectionFormLabourPartIIIMinimumWagesAct(requestData);
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

        [HttpGet, Route("getForm_Labour_Part_III_PaymentWagesAct")]
        public async Task<IActionResult> GetForm_Labour_Part_III_PaymentWagesAct([FromQuery] Int64 id)
        {
            GenericFormModel<Inspection_Form_Labour_III_PaymentWagesAct> genericFormModel = await _iInspectionService.GetInspectionFormLabourPartIIIPaymentWagesAct(id);
            if (genericFormModel.HasError)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, genericFormModel.ErrorDesc);
            }
            return StatusCode(StatusCodes.Status200OK, genericFormModel);
        }

        [Route("addUpdateForm_Labour_Part_III_PaymentWagesAct")]
        [HttpPost]
        public async Task<IActionResult> AddUpdateForm_Labour_Part_III_PaymentWagesAct([FromBody] Inspection_Form_Labour_III_PaymentWagesAct requestData)
        {
            GenericServiceResultTemplate genericServiceResultTemplate = await _iInspectionService.AddUpdate_InspectionFormLabourPartIIIPaymentWagesAct(requestData);
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

        [HttpGet, Route("getForm_Labour_Part_III_PaymentBonusAct_StatutoryReport")]
        public async Task<IActionResult> GetForm_Labour_Part_III_PaymentBonusAct_StatutoryReport([FromQuery] Int64 id)
        {
            GenericFormModel<Inspection_Form_Labour_III_PaymentBonusAct_StatutoryReport> genericFormModel = await _iInspectionService.GetInspectionFormLabourPartIIIPaymentBonusAct_StatutoryReport(id);
            if (genericFormModel.HasError)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, genericFormModel.ErrorDesc);
            }
            return StatusCode(StatusCodes.Status200OK, genericFormModel);
        }

        [Route("addUpdateForm_Labour_Part_III_PaymentBonusAct_StatutoryReport")]
        [HttpPost]
        public async Task<IActionResult> AddUpdateForm_Labour_Part_III_PaymentBonusAct_StatutoryReport([FromBody] Inspection_Form_Labour_III_PaymentBonusAct_StatutoryReport requestData)
        {
            GenericServiceResultTemplate genericServiceResultTemplate = await _iInspectionService.AddUpdate_InspectionFormLabourPartIIIPaymentBonusAct_StatutoryReport(requestData);
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

        [HttpGet, Route("getForm_Labour_Part_III_PaymentBonusAct_Part_B")]
        public async Task<IActionResult> GetForm_Labour_Part_III_PaymentBonusAct_Part_B([FromQuery] Int64 id)
        {
            GenericFormModel<List<Inspection_Form_Labour_III_PaymentBonusAct_Part_B_Attachment>> genericFormModel = await _iInspectionService.GetInspectionFormLabourPartIIIPaymentBonusAct_Part_B_Attachment(id);
            if (genericFormModel.HasError)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, genericFormModel.ErrorDesc);
            }
            return StatusCode(StatusCodes.Status200OK, genericFormModel);
        }

        [Route("addUpdateForm_Labour_Part_III_PaymentBonusAct_Part_B")]
        [HttpPost]
        public async Task<IActionResult> AddUpdateForm_Labour_Part_III_PaymentBonusAct_Part_B([FromBody] Inspection_Form_Labour_III_PaymentBonusAct_Part_B_Attachment requestData)
        {
            GenericServiceResultTemplate genericServiceResultTemplate = await _iInspectionService.AddUpdate_InspectionFormLabourPartIIIPaymentBonusAct_Part_B_Attachment(requestData);
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

        [HttpGet, Route("getForm_Labour_Part_III_ChildAndAdolescentLabourAct")]
        public async Task<IActionResult> GetForm_Labour_Part_III_ChildAndAdolescentLabourAct([FromQuery] Int64 id)
        {
            GenericFormModel<Inspection_Form_Labour_III_ChildAndAdolescentLabourAct> genericFormModel = await _iInspectionService.GetInspectionFormLabourPartIIIChildAndAdolescentLabourAct(id);
            if (genericFormModel.HasError)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, genericFormModel.ErrorDesc);
            }
            return StatusCode(StatusCodes.Status200OK, genericFormModel);
        }

        [Route("addUpdateForm_Labour_Part_III_ChildAndAdolescentLabourAct")]
        [HttpPost]
        public async Task<IActionResult> AddUpdateForm_Labour_Part_III_ChildAndAdolescentLabourAct([FromBody] Inspection_Form_Labour_III_ChildAndAdolescentLabourAct requestData)
        {
            GenericServiceResultTemplate genericServiceResultTemplate = await _iInspectionService.AddUpdate_InspectionFormLabourPartIIIChildAndAdolescentLabourAct(requestData);
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

        [HttpGet, Route("getForm_Labour_Part_III_NationalAndFestivalHolidays")]
        public async Task<IActionResult> GetForm_Labour_Part_III_NationalAndFestivalHolidays([FromQuery] Int64 id)
        {
            GenericFormModel<Inspection_Form_Labour_III_NationalAndFestivalHolidays> genericFormModel = await _iInspectionService.GetInspectionFormLabourPartIIINationalAndFestivalHolidays(id);
            if (genericFormModel.HasError)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, genericFormModel.ErrorDesc);
            }
            return StatusCode(StatusCodes.Status200OK, genericFormModel);
        }

        [Route("addUpdateForm_Labour_Part_III_NationalAndFestivalHolidays")]
        [HttpPost]
        public async Task<IActionResult> AddUpdateForm_Labour_Part_III_NationalAndFestivalHolidays([FromBody] Inspection_Form_Labour_III_NationalAndFestivalHolidays requestData)
        {
            GenericServiceResultTemplate genericServiceResultTemplate = await _iInspectionService.AddUpdate_InspectionFormLabourPartIIINationalAndFestivalHolidays(requestData);
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

        [HttpGet, Route("getForm_Labour_Part_III_MaternityBenefitAct")]
        public async Task<IActionResult> GetForm_Labour_Part_III_MaternityBenefitAct([FromQuery] Int64 id)
        {
            GenericFormModel<Inspection_Form_Labour_III_MaternityBenefitAct> genericFormModel = await _iInspectionService.GetInspectionFormLabourPartIIIMaternityBenefitAct(id);
            if (genericFormModel.HasError)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, genericFormModel.ErrorDesc);
            }
            return StatusCode(StatusCodes.Status200OK, genericFormModel);
        }

        [Route("addUpdateForm_Labour_Part_III_MaternityBenefitAct")]
        [HttpPost]
        public async Task<IActionResult> AddUpdateForm_Labour_Part_III_MaternityBenefitAct([FromBody] Inspection_Form_Labour_III_MaternityBenefitAct requestData)
        {
            GenericServiceResultTemplate genericServiceResultTemplate = await _iInspectionService.AddUpdate_InspectionFormLabourPartIIIMaternityBenefitAct(requestData);
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

        [HttpGet, Route("getForm_Labour_Part_III_ContractLabourAct")]
        public async Task<IActionResult> GetForm_Labour_Part_III_ContractLabourAct([FromQuery] Int64 id)
        {
            GenericFormModel<Inspection_Form_Labour_III_ContractLabourAct> genericFormModel = await _iInspectionService.GetInspectionFormLabourPartIIIContractLabourAct(id);
            if (genericFormModel.HasError)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, genericFormModel.ErrorDesc);
            }
            return StatusCode(StatusCodes.Status200OK, genericFormModel);
        }

        [Route("addUpdateForm_Labour_Part_III_ContractLabourAct")]
        [HttpPost]
        public async Task<IActionResult> AddUpdateForm_Labour_Part_III_ContractLabourAct([FromBody] Inspection_Form_Labour_III_ContractLabourAct requestData)
        {
            GenericServiceResultTemplate genericServiceResultTemplate = await _iInspectionService.AddUpdate_InspectionFormLabourPartIIIContractLabourAct(requestData);
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

        [HttpGet, Route("getForm_Labour_Part_III_InterStateMigrantWorkmenAct")]
        public async Task<IActionResult> GetForm_Labour_Part_III_InterStateMigrantWorkmenAct([FromQuery] Int64 id)
        {
            GenericFormModel<Inspection_Form_Labour_III_InterStateMigrantWorkmenAct> genericFormModel = await _iInspectionService.GetInspectionFormLabourPartIIIInterStateMigrantWorkmenAct(id);
            if (genericFormModel.HasError)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, genericFormModel.ErrorDesc);
            }
            return StatusCode(StatusCodes.Status200OK, genericFormModel);
        }

        [Route("addUpdateForm_Labour_Part_III_InterStateMigrantWorkmenAct")]
        [HttpPost]
        public async Task<IActionResult> AddUpdateForm_Labour_Part_III_InterStateMigrantWorkmenAct([FromBody] Inspection_Form_Labour_III_InterStateMigrantWorkmenAct requestData)
        {
            GenericServiceResultTemplate genericServiceResultTemplate = await _iInspectionService.AddUpdate_InspectionFormLabourPartIIIInterStateMigrantWorkmenAct(requestData);
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

        [HttpGet, Route("getForm_Labour_Part_III_LabourWelfareFund_Act")]
        public async Task<IActionResult> GetForm_Labour_Part_III_LabourWelfareFund_Act([FromQuery] Int64 id)
        {
            GenericFormModel<Inspection_Form_Labour_III_LabourWelfareFund_Act> genericFormModel = await _iInspectionService.GetInspectionFormLabourPartIIILabourWelfareFund_Act(id);
            if (genericFormModel.HasError)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, genericFormModel.ErrorDesc);
            }
            return StatusCode(StatusCodes.Status200OK, genericFormModel);
        }

        [Route("addUpdateForm_Labour_Part_III_LabourWelfareFund_Act")]
        [HttpPost]
        public async Task<IActionResult> AddUpdateForm_Labour_Part_III_LabourWelfareFund_Act([FromBody] Inspection_Form_Labour_III_LabourWelfareFund_Act requestData)
        {
            GenericServiceResultTemplate genericServiceResultTemplate = await _iInspectionService.AddUpdate_InspectionFormLabourPartIIILabourWelfareFund_Act(requestData);
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

        [HttpGet, Route("getForm_Labour_Part_III_GratuityAct")]
        public async Task<IActionResult> GetForm_Labour_Part_III_GratuityAct([FromQuery] Int64 id)
        {
            GenericFormModel<Inspection_Form_Labour_III_GratuityAct> genericFormModel = await _iInspectionService.GetInspectionFormLabourPartIIIGratuityAct(id);
            if (genericFormModel.HasError)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, genericFormModel.ErrorDesc);
            }
            return StatusCode(StatusCodes.Status200OK, genericFormModel);
        }

        [Route("addUpdateForm_Labour_Part_III_GratuityAct")]
        [HttpPost]
        public async Task<IActionResult> AddUpdateForm_Labour_Part_III_GratuityAct([FromBody] Inspection_Form_Labour_III_GratuityAct requestData)
        {
            GenericServiceResultTemplate genericServiceResultTemplate = await _iInspectionService.AddUpdate_InspectionFormLabourPartIIIGratuityAct(requestData);
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

        [HttpGet, Route("getForm_Labour_Part_III_IndustrialEmploymentAct")]
        public async Task<IActionResult> GetForm_Labour_Part_III_IndustrialEmploymentAct([FromQuery] Int64 id)
        {
            GenericFormModel<Inspection_Form_Labour_III_IndustrialEmploymentAct> genericFormModel = await _iInspectionService.GetInspectionFormLabourPartIIIIndustrialEmploymentAct(id);
            if (genericFormModel.HasError)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, genericFormModel.ErrorDesc);
            }
            return StatusCode(StatusCodes.Status200OK, genericFormModel);
        }

        [Route("addUpdateForm_Labour_Part_III_IndustrialEmploymentAct")]
        [HttpPost]
        public async Task<IActionResult> AddUpdateForm_Labour_Part_III_IndustrialEmploymentAct([FromBody] Inspection_Form_Labour_III_IndustrialEmploymentAct requestData)
        {
            GenericServiceResultTemplate genericServiceResultTemplate = await _iInspectionService.AddUpdate_InspectionFormLabourPartIIIIndustrialEmploymentAct(requestData);
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

        [HttpGet, Route("getForm_Labour_Part_III_BOCW_Act")]
        public async Task<IActionResult> GetForm_Labour_Part_III_BOCW_Act([FromQuery] Int64 id)
        {
            GenericFormModel<Inspection_Form_Labour_III_BOCW_Act> genericFormModel = await _iInspectionService.GetInspectionFormLabourPartIIIBOCW_Act(id);
            if (genericFormModel.HasError)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, genericFormModel.ErrorDesc);
            }
            return StatusCode(StatusCodes.Status200OK, genericFormModel);
        }

        [Route("addUpdateForm_Labour_Part_III_BOCW_Act")]
        [HttpPost]
        public async Task<IActionResult> AddUpdateForm_Labour_Part_III_BOCW_Act([FromBody] Inspection_Form_Labour_III_BOCW_Act requestData)
        {
            GenericServiceResultTemplate genericServiceResultTemplate = await _iInspectionService.AddUpdate_InspectionFormLabourPartIIIBOCW_Act(requestData);
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

        [HttpGet, Route("getForm_Labour_Part_III_ShopAct")]
        public async Task<IActionResult> GetForm_Labour_Part_III_ShopAct([FromQuery] Int64 id)
        {
            GenericFormModel<Inspection_Form_Labour_III_ShopAct> genericFormModel = await _iInspectionService.GetInspectionFormLabourPartIIIShopAct(id);
            if (genericFormModel.HasError)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, genericFormModel.ErrorDesc);
            }
            return StatusCode(StatusCodes.Status200OK, genericFormModel);
        }

        [Route("addUpdateForm_Labour_Part_III_ShopAct")]
        [HttpPost]
        public async Task<IActionResult> AddUpdateForm_Labour_Part_III_ShopAct([FromBody] Inspection_Form_Labour_III_ShopAct requestData)
        {
            GenericServiceResultTemplate genericServiceResultTemplate = await _iInspectionService.AddUpdate_InspectionFormLabourPartIIIShopAct(requestData);
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

        [HttpGet, Route("getForm_Labour_Part_III_Observations")]
        public async Task<IActionResult> GetForm_Labour_Part_III_Observations([FromQuery] Int64 id)
        {
            GenericFormModel<Inspection_Form_Labour_III_Observations> genericFormModel = await _iInspectionService.GetInspectionFormLabourPartIIIObservations(id);
            if (genericFormModel.HasError)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, genericFormModel.ErrorDesc);
            }
            return StatusCode(StatusCodes.Status200OK, genericFormModel);
        }

        [Route("addUpdateForm_Labour_Part_III_Observations")]
        [HttpPost]
        public async Task<IActionResult> AddUpdateForm_Labour_Part_III_Observations([FromBody] Inspection_Form_Labour_III_Observations requestData)
        {
            GenericServiceResultTemplate genericServiceResultTemplate = await _iInspectionService.AddUpdate_InspectionFormLabourPartIIIObservations(requestData);
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

        [HttpGet, Route("get_Inspections_LabourPerformaStepStatus")]
        public async Task<IActionResult> get_Inspections_LabourPerformaStepStatus([FromQuery] Int64 inspectionRefId)
        {
            GenericResponseTemplateModel<List<GetInspections_FactoryPerformaStepStatusViewModel>> genericFormModel = await _iInspectionService.Get_Inspections_LabourPerformaStepStatus(inspectionRefId);
            if (genericFormModel.HasError)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, genericFormModel.ErrorDesc);
            }
            return StatusCode(StatusCodes.Status200OK, genericFormModel);
        }
        #endregion



        [Route("lock_Inspection")]
        [HttpPost]
        public async Task<IActionResult> lock_Inspection([FromBody] Inpection_LockViewModel requestData)
        {
            GenericServiceResultTemplate genericServiceResultTemplate = await _iInspectionService.Lock_Inspection(requestData);
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

        [HttpGet, Route("getInspectionEstablishmentDetails")]
        public async Task<IActionResult> GetInspectionEstablishmentDetails(string licenceNo)
        {
            GenericResponseTemplateModel<InspectionEstablishmentBasicDetailsViewModel> genericFormModel = await _iInspectionService.GetInspectionEstablishmentBasicDetails(licenceNo);
            if (genericFormModel.HasError)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, genericFormModel.ErrorDesc);
            }
            return StatusCode(StatusCodes.Status200OK, genericFormModel);
        }


        [HttpGet, Route("getInspectionFactoryDetailsByLicenceNo")]
        public async Task<IActionResult> GetInspectionFactoryDetailsByLicenceNo(string licenceNo)
        {
            GenericResponseTemplateModel<InspectionFactoryDetailsViewModel> genericFormModel = await _iInspectionService.GetInspectionFactoryDetailsByLicenceNo(licenceNo);
            if (genericFormModel.HasError)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, genericFormModel.ErrorDesc);
            }
            return StatusCode(StatusCodes.Status200OK, genericFormModel);
        }

        [HttpGet, Route("getLabourCircleOfficersByInspectionRefId")]
        public async Task<IActionResult> Get_LabourCircleOfficersByInspectionRefId([FromQuery] string userRefId, string roleName)
        {
            GenericResponseTemplateModel<List<InspectionTransferInfoViewModel>> genericFormModel = await _iInspectionService.GetLabourCircleOfficersByInspectionRefId( userRefId,  roleName);

            if (genericFormModel.HasError)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, genericFormModel.ErrorDesc);
            }
            return StatusCode(StatusCodes.Status200OK, genericFormModel);
        }
        
        [HttpGet, Route("getDistrictRefIdByUserId")]
        public async Task<IActionResult> GetDistrictRefIdByUserId(string userId)
        {
            GenericResponseTemplateModel<Int64> genericFormModel = await _iInspectionService.GetDistrictRefIdByUserId(userId);
            if (genericFormModel.HasError)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, genericFormModel.ErrorDesc);
            }
            return StatusCode(StatusCodes.Status200OK, genericFormModel);
        }

        [HttpGet, Route("getFactoryCircleIdByLabourCircleId")]
        public async Task<IActionResult> GetFactoryCircleIdByLabourCircleId(Int64 labourCircleRefId)
        {
            GenericResponseTemplateModel<GetFactoryCirlceViewModel> genericFormModel = await _iInspectionService.GetFactoryCircleIdByLabourCircleId(labourCircleRefId);
            if (genericFormModel.HasError)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, genericFormModel.ErrorDesc);
            }
            return StatusCode(StatusCodes.Status200OK, genericFormModel);
        }

        [HttpGet, Route("getInspectionOperationalStatus")]
        public async Task<IActionResult> Get_Inspection_OperationalStatus(Int64 inspectionRefId, string roleName)
        {
            GenericResponseTemplateModel<Inpection_LockViewModel> genericFormModel = await _iInspectionService.Get_Inspection_OperationalStatus(inspectionRefId, roleName);
            if (genericFormModel.HasError)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, genericFormModel.ErrorDesc);
            }
            return StatusCode(StatusCodes.Status200OK, genericFormModel);
        }

        [HttpGet, Route("inspection_getLabourCircleByalcCircleRefId")]
        public async Task<IActionResult> GetInspection_LabourCircleByDistricRefId([FromQuery] int districtRefId)
        {
            GenericFormModel<List<InspectionLabourCircleViewModel>> genericFormModel = await _iInspectionService.GetInspection_LabourCircleByDistricRefId(districtRefId);
            if (genericFormModel.HasError)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, genericFormModel.ErrorDesc);
            }
            return StatusCode(StatusCodes.Status200OK, genericFormModel);
        }

        [Route("update_ContactDetails")]
        [HttpPost]
        public async Task<IActionResult> update_ContactDetails(UpdateAlternateContactDetailsViewModel formModel)
            {
            GenericServiceResultTemplate genericServiceResultTemplate = await _iInspectionService.Update_ContactDetails(formModel);
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

        [HttpGet, Route("get_ContactDetails")]
        public async Task<IActionResult> Get_ContactDetails(Int64 appId, bool isLegacy)
        {
            GenericResponseTemplateModel<AlternateContactDetailsViewModel> genericFormModel = await _iInspectionService.Get_ContactDetails(appId, isLegacy);
            if (genericFormModel.HasError)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, genericFormModel.ErrorDesc);
            }
            return StatusCode(StatusCodes.Status200OK, genericFormModel);
        }

        [HttpGet, Route("verifyLicenceNumber")]
        public async Task<IActionResult> VerifyLicenceNumber([FromQuery] string licenceNumber)
        {
            GenericResponseTemplateModel<IntReturn> genericResponseTemplateModel = await _iInspectionService.VerifyLicenceNumber(licenceNumber);
            if (genericResponseTemplateModel.HasError)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, genericResponseTemplateModel.ErrorDesc);
            }
            return StatusCode(StatusCodes.Status200OK, genericResponseTemplateModel);
        }

        [HttpGet, Route("verifyLicenceNumberWithUser")]
        public async Task<IActionResult> VerifyLicenceNumberWithUser([FromQuery] string licenceNumber, string userRefId)
        {
            GenericResponseTemplateModel<LicenceNumberDetailsViewModel> genericResponseTemplateModel = await _iInspectionService.VerifyLicenceNumberWithUser(licenceNumber, userRefId);
            if (genericResponseTemplateModel.HasError)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, genericResponseTemplateModel.ErrorDesc);
            }
            return StatusCode(StatusCodes.Status200OK, genericResponseTemplateModel);
        }

        [HttpGet, Route("get_inspectionsDataByUserId")]
        public async Task<IActionResult> Get_inspectionsDataByUserId([FromQuery] string searchCode, int pageNo, int pageSize, string sortColumn, string sortOrder, string filterArray, string userRefId, int investpunjab_ipin, string roleName)
        {
            GenericResponseTemplateModel<List<InspectionComplianceinfoViewModel>> genericFormModel = await _iInspectionService.Get_inspectionsDataByUserId(searchCode, pageNo, pageSize, sortColumn, sortOrder, filterArray, userRefId, investpunjab_ipin, roleName);
            if (genericFormModel.HasError)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, genericFormModel.ErrorDesc);
            }
            return StatusCode(StatusCodes.Status200OK, genericFormModel);
        }

        [HttpGet, Route("getInspectionViolationData")]
        public async Task<IActionResult> GetInspectionViolationData([FromQuery] Int64 inspectionRefId, int inspectionType)
        {
            GenericResponseTemplateModel<InspectionSectionWiseKeyValuePair> genericResponseTemplateModel = await _iInspectionService.GetInspectionViolationData(inspectionRefId, inspectionType);
            if (genericResponseTemplateModel.HasError)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, genericResponseTemplateModel.ErrorDesc);
            }
            return StatusCode(StatusCodes.Status200OK, genericResponseTemplateModel);
        }

        [HttpGet, Route("getprocessinspectiondetail")]
        public async Task<IActionResult> Get_ProcessInspectionDetail(string id, int currentActionCode, int inspectionType)
        {
            GenericFormModel<List<RoleWiseAllowedActionCodeViewModel>> genericFormModel = await _iInspectionService.GetProcessInspectionDetail(id, currentActionCode, inspectionType);
            if (genericFormModel.HasError)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, genericFormModel.ErrorDesc);
            }
            return StatusCode(StatusCodes.Status200OK, genericFormModel);
        }

        [HttpGet, Route("getuserbyinspectionactioncode")]
        public async Task<IActionResult> GetUserByInspectionActionCode(string id, int actionCode, Int64 inspectionRefId, int inspectionType)
        {
            //var user = await _userManager.FindByNameAsync(User.Identity.Name);

            GenericFormModel<List<ProcessApplicationUsersViewModel>> genericFormModel = await _iInspectionService.GetUserByInspectionActionCode(actionCode, id, inspectionRefId, inspectionType);
            if (genericFormModel.HasError)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, genericFormModel.ErrorDesc);
            }
            return StatusCode(StatusCodes.Status200OK, genericFormModel);
        }

        [Route("addprocessinspectiondetails")]
        [HttpPost]
        public async Task<IActionResult> AddUpdate_ProcessInspectionDetail([FromBody] InspectionActionViewModel requestData)
        {
            //var user = await _userManager.FindByNameAsync(User.Identity.Name);
            var resp = await _iInspectionService.RecordInspectionAction(requestData, requestData.UserId);
            if (resp.HasError)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, resp.ErrorDesc);
            }
            else
            {
                // Share status to Invest Punjab Portal
                //await _iThirdPartyInegrationsService.ShareStatusToBusinessFirst(genericServiceResultTemplate.ApplicationInitiateResponse.AppId, ApplicationTypeEnum.BUILDING_PLAN_HUD, (AppActionTypeEnum)requestData.AppActionType);
            }

            return StatusCode(StatusCodes.Status200OK, resp);
        }

        [HttpGet, Route("getInspectionLogsByInspectionId")]
        public async Task<IActionResult> GetInspectionLogsByInspectionId(Int64 inspectionRefId, int inspectionType)
        {
            GenericResponseTemplateModel<List<GetInspectionNotingLogsViewModel>> genericFormModel = await _iInspectionService.GetInspectionLogsByInspectionId(inspectionRefId, inspectionType);
            if (genericFormModel.HasError)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, genericFormModel.ErrorDesc);
            }
            return StatusCode(StatusCodes.Status200OK, genericFormModel);
        }

        [HttpGet, Route("initiateinspectiondocuments")]
        public async Task<IActionResult> Get_InitiateInspectionDocuments([FromQuery] Int64 id, bool deleteTempFiles, int inspectionType)
        {

            List<AppFileUploadInfoViewModel> filesDetail = new List<AppFileUploadInfoViewModel>();
            bool isSomethingWentWrong = false;
            try
            {
                filesDetail = await _iInspectionService.InitiateInspectionFileInfo(id, deleteTempFiles, inspectionType);

               
            }
            catch (Exception ex)
            {
                isSomethingWentWrong = true;
            }
            if (isSomethingWentWrong)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, false);
            }
            return StatusCode(StatusCodes.Status200OK, filesDetail);
        }

        [HttpGet, Route("initiateinspectiondocumentwithroleid")]
        public async Task<IActionResult> Get_InitiateAppDocumentsWithRoleId([FromQuery] Int64 id, bool deleteTempFiles, string userId, int currentActionCode, int allowedActionCode, Int64 inspectionType)
        {
            List<AppFileUploadInfoViewModel> filesDetail = new List<AppFileUploadInfoViewModel>();
            bool isSomethingWentWrong = false;
            try
            {
                filesDetail = await _iInspectionService.InitiateInspectionFileInfoWithRoleId(id, deleteTempFiles, userId, currentActionCode, allowedActionCode, inspectionType);
            }
            catch (Exception ex)
            {
                isSomethingWentWrong = true;
            }
            if (isSomethingWentWrong)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, false);
            }
            return StatusCode(StatusCodes.Status200OK, filesDetail);
        }

        [HttpPost]
        [Route("UploadDocument")]
        public async Task<IActionResult> UploadDocument()
        {
            GenericResponseTemplateModel<FileUploadResponse> genericResponse = new GenericResponseTemplateModel<FileUploadResponse>() { ErrorDesc = "", HasError = false };
            try
            {
                var files = Request.Form.Files;
                if (files.Count() > 0)
                {
                    StringValues docIds, inspectionIds, appDocIds, inspectiontypes;
                    genericResponse.HasError = Request.Headers.TryGetValue("docid", out docIds);
                    genericResponse.HasError = Request.Headers.TryGetValue("inspectionid", out inspectionIds);
                    genericResponse.HasError = Request.Headers.TryGetValue("appdocid", out appDocIds);
                    genericResponse.HasError = Request.Headers.TryGetValue("inspectiontype", out inspectiontypes);

                    if (docIds.Count() == 0 || inspectionIds.Count() == 0)
                    {
                        genericResponse.HasError = true;
                    }
                    else
                    {
                        string DocId = docIds[0];
                        string InspectionId = inspectionIds[0];
                        string AppDocId = appDocIds[0];
                        string InspectionType = inspectiontypes[0];

                        var filepath = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "ComplianceReport")).Root;
                        string fileName = "";

                        foreach (var file in files)
                        {
                            //fileName = AppId + "_" + DocId + "_" + AppDocId + file.Name.Substring(file.FileName.LastIndexOf('.'));
                            //filepath = filepath + fileName;
                            genericResponse = await _iInspectionService.LogFileInfo(Convert.ToInt64(InspectionId), Convert.ToInt64(DocId), Convert.ToInt64(AppDocId), file.Name.Substring(file.FileName.LastIndexOf('.')),Convert.ToInt32(InspectionType));
                            if (!genericResponse.HasError)
                            {
                                filepath = filepath + genericResponse.ResponseDataModel.FileName;
                                using (FileStream fs = System.IO.File.Create(filepath))
                                {
                                    file.CopyTo(fs);
                                    fs.Flush();
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                genericResponse.HasError = true;
                genericResponse.ResponseDataModel = new FileUploadResponse()
                {
                    FileName = "",
                    Id = 0
                };
                genericResponse.ErrorDesc = ex.Message;
            }
            if (genericResponse.HasError)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, genericResponse);
            }
            return StatusCode(StatusCodes.Status200OK, genericResponse);
        }

        [HttpGet, Route("lockuploadfiles")]
        public async Task<IActionResult> Get_LockUploadFiles([FromQuery] Int64 id, string appDocIds)
        {
            bool isSomethingWentWrong = false;
            try
            {
                var appDocIdsArray = JsonConvert.DeserializeObject<Int64[]>(appDocIds);
                //isSomethingWentWrong = !await _iCommonApisService.SaveAppFileInfo(Convert.ToInt64(id), Convert.ToInt64(docId), newFileName);
                isSomethingWentWrong = !await _iInspectionService.LockAppDocFiles(id, appDocIdsArray);
            }
            catch (Exception ex)
            {
                isSomethingWentWrong = true;
            }
            if (isSomethingWentWrong)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, false);
            }
            return StatusCode(StatusCodes.Status200OK, true);
        }

        [Route("insert_complianceLogs")]
        [HttpPost]
        public async Task<IActionResult> insert_complianceLogs([FromQuery] int inspectionType)
        {
            GenericServiceResultTemplate genericServiceResultTemplate = await _iInspectionService.Insert_complianceLogs(inspectionType);
            if (genericServiceResultTemplate.HasException)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, genericServiceResultTemplate.Exceptions.Message);
            }
            return StatusCode(StatusCodes.Status200OK, genericServiceResultTemplate);
        }

        [HttpGet, Route("send_otp")]
        public async Task<IActionResult> SendOTPToUser(string mobileNumber, string userId, string licenceNumber, string newUserName)
        {
            GenericResponseTemplateModel<string> serviceGatewayResponse = await _iInspectionService.SendOTPToUser(mobileNumber, userId,licenceNumber,newUserName);
            if (serviceGatewayResponse.HasError)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, serviceGatewayResponse.ErrorDesc);
            }
            return StatusCode(StatusCodes.Status200OK, serviceGatewayResponse);

        }

        [HttpGet, Route("verify_otp")]
        public async Task<IActionResult> VerifyOTP(int otp, string encryptedData, string userId, string licenceNumber)
        {
            GenericResponseTemplateModel<bool> serviceGatewayResponse = await _iInspectionService.VerifyOTP(otp, encryptedData, userId, licenceNumber);
            if (serviceGatewayResponse.HasError)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, serviceGatewayResponse.ErrorDesc);
            }
            return StatusCode(StatusCodes.Status200OK, serviceGatewayResponse);

        }

        [HttpGet, Route("getInspectionTranferLogs")]
        public async Task<IActionResult> GetInspectionTransferLogs(Int64 inspectionRefId, int inspectionType)
        {
            GenericResponseTemplateModel<List<InspectionTransferLogsViewModel>> genericFormModel = await _iInspectionService.GetInspectionTransferLogs(inspectionRefId, inspectionType);
            if (genericFormModel.HasError)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, genericFormModel.ErrorDesc);
            }
            return StatusCode(StatusCodes.Status200OK, genericFormModel);
        }

        [HttpGet, Route("randomization-initialization")]
        public async Task<IActionResult> RadomizationInitialization([FromQuery] int month, int year, string userRefId)
        {
            GenericFormModel<List<RandomizationInitializationViewModel>> genericFormModel = await _iInspectionService.RadomizationInitialization(month, year, userRefId);
            if (genericFormModel.HasError)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, genericFormModel.ErrorDesc);
            }
            return StatusCode(StatusCodes.Status200OK, genericFormModel);
        }

        [HttpGet, Route("getOldInspectionId")]
        public async Task<IActionResult> GetOldInspectionId([FromQuery] Int64 inspectionRefId)
        {
            GenericFormModel<OldInspectionMapping> genericFormModel = await _iInspectionService.GetOldInspectionId(inspectionRefId);
            if (genericFormModel.HasError)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, genericFormModel.ErrorDesc);
            }
            return StatusCode(StatusCodes.Status200OK, genericFormModel);
        }

        [HttpGet, Route("getInspectionFactoryDetailsByLicenceNo_Allotment")]
        public async Task<IActionResult> GetInspectionFactoryDetailsByLicenceNo_Allotment(string licenceNo)
        {
            GenericResponseTemplateModel<InspectionFactoryAllotmentDetailsViewModel> genericFormModel = await _iInspectionService.GetInspectionFactoryDetailsByLicenceNo_Allotment(licenceNo);
            if (genericFormModel.HasError)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, genericFormModel.ErrorDesc);
            }
            return StatusCode(StatusCodes.Status200OK, genericFormModel);
        }

        [Route("insert_allotmentinspections")]
        [HttpPost]
        public async Task<IActionResult> Insert_AllotmentInspections([FromBody] InspectionAllotmentRequestViewModel requestData)
        {
            GenericServiceResultTemplate genericServiceResultTemplate = await _iInspectionService.Insert_AllotmentInspections(requestData);
            if (genericServiceResultTemplate.HasException)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, genericServiceResultTemplate.Exceptions.Message);
            }
            return StatusCode(StatusCodes.Status200OK, genericServiceResultTemplate);
        }

        [HttpGet, Route("verifyCurrentMonthyear")]
        public async Task<IActionResult> verifyCurrentMonthYear([FromQuery] int month ,int year)
        {
            GenericResponseTemplateModel<IntReturn> genericResponseTemplateModel = await _iInspectionService.VerifyCurrentMonthYear(month, year);
            if (genericResponseTemplateModel.HasError)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, genericResponseTemplateModel.ErrorDesc);
            }
            return StatusCode(StatusCodes.Status200OK, genericResponseTemplateModel);
        }
    }


}
