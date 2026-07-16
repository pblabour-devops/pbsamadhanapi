using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using pbsamadhannetcoreapi.CommonUtiliteis;
using pbsamadhannetcoreapi.Models;
using pbsamadhannetcoreapi.Repositories;
using pbsamadhannetcoreapi.Repositories.Implementations;
using pbsamadhannetcoreapi.ViewModels;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Storage;
namespace pbsamadhannetcoreapi.Services.Implementations
{
    public class BusinessEntityService //: IBusinessEntityService
    {
        //private readonly IGenericRepository<BusinessEntity> _iGR_BusinessEntity;
        //private readonly IApplicationManagementService<BusinessEntity> _iApplicationMamnagementService;
        //public BusinessEntityService(IGenericRepository<BusinessEntity> iGR_BusinessEntity,
        // IApplicationManagementService<BusinessEntity> IApplicationMamnagementService,
        // AppDbContext context)
        //{
        //    _iGR_BusinessEntity = iGR_BusinessEntity;
        //    _iApplicationMamnagementService = IApplicationMamnagementService;
        //}

        //public async Task<GenericFormModel<BusinessEntity>> GetBusinessEntityDetail(long id)
        //{
        //    GenericFormModel<BusinessEntity> genericFormModel = new GenericFormModel<BusinessEntity>();
        //    try
        //    {
        //        genericFormModel.ListTemplateLists = new List<ListTemplate>();

        //        // Check if this is an existing record
        //        if (id != 0) //Existing Record
        //        {
        //            //Get Form data
        //            genericFormModel.FormModel = _iGR_BusinessEntity.GetById(id);

        //            genericFormModel.IsEditAllowed = false;
        //            //if form model is available
        //            if (genericFormModel.FormModel != null)
        //            {
        //                genericFormModel.IsEditAllowed = true;
        //            }
        //        }
        //        else //New Record
        //        {
        //            genericFormModel.FormModel = new BusinessEntity();

        //            //Allow user to edit form
        //            genericFormModel.IsEditAllowed = true;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //    }
        //    return genericFormModel;
        //}
        //public async Task<GenericServiceResultTemplate> AddUpdate_BusinessEntityDetail(string requestData)
        //{
        //    GenericServiceResultTemplate genericServiceResultTemplate = new GenericServiceResultTemplate();
        //    try
        //    {
        //        BusinessEntity formModel = new BusinessEntity();
        //        formModel = Newtonsoft.Json.JsonConvert.DeserializeObject<BusinessEntity>(requestData);
        //        genericServiceResultTemplate.HasException = false;
        //        genericServiceResultTemplate.CustomeValidationResult = CustomeValidator<BusinessEntity>.ValidateModel_AllProperties(formModel);
        //        if (genericServiceResultTemplate.CustomeValidationResult.IsValid)
        //        {
        //            if (formModel.BusinessEntityId != 0) //Existing record
        //            {
        //                //formModel.LastModifiedOnDate = DateTime.Now;
        //                _iGR_BusinessEntity.Update(formModel);
        //                //Save changes
        //                _iGR_BusinessEntity.Savechange();

        //                //Update last modified date
        //                await _iApplicationMamnagementService.UpdateAppLastModifiedDate(formModel.BusinessEntityId);
        //                genericServiceResultTemplate.ApplicationInitiateResponse = new ApplicationInitiateResponseViewModel();
        //                genericServiceResultTemplate.ApplicationInitiateResponse.EntityKeyId = formModel.BusinessEntityId;
        //            }
        //            else //New record
        //            {
        //                formModel = GenericModelOps<BusinessEntity>.SetNullAllNevigationProperties(formModel);
        //                _iGR_BusinessEntity.Insert(formModel);
        //                _iGR_BusinessEntity.Savechange();
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        genericServiceResultTemplate.HasException = true;
        //        genericServiceResultTemplate.Exceptions = ex;
        //    }
        //    return genericServiceResultTemplate;
        //}
    }
}
