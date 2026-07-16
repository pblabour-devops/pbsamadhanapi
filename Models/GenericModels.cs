using Microsoft.AspNetCore.Mvc.Rendering;
using pbsamadhannetcoreapi.CommonUtiliteis;
using pbsamadhannetcoreapi.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace pbsamadhannetcoreapi.Models
{
    public class GenericFormModel<T>
    {
        public T FormModel { get; set; }
        public bool IsEditAllowed { get; set; }
        public bool IsLocked { get; set; }
        public List<EnumListTemplate> EnumTemplateLists  { get; set; }
        public List<ListTemplate> ListTemplateLists { get; set; }
        public List<AppFormStepsInfo> AppFormStepsList { get; set; }
        public bool HasError { get; set; }
        public string ErrorDesc { get; set; }
        public ApplicationLifeCycleStatusTypeEnum ApplicationLifeCycleStatusType { get; set; }
    }
    public class EnumListTemplate
    {
        public string SelectListTypeCode { get; set; }
        public SelectList SelectListItems { get; set; }
    }

    public class GenericListTemplate
    {
        public Int64 ID { get; set; }
        public string Text { get; set; }
    }

    public class ListTemplate
    {
        public string ListTypeCode { get; set; }
        public List<GenericListTemplate> ListItems { get; set; }
    }
    public class GenericServiceResultTemplate
    {
        public ApplicationInitiateResponseViewModel ApplicationInitiateResponse { get; set; }
        public CustomeValidationResult CustomeValidationResult { get; set; }
        public bool HasException { get; set; }
        public Exception Exceptions { get; set; }
    }
    public class AppFormStepsInfo
    {
        public string StepTitle { get; set; }
        public bool IsFilled { get; set; }
        public bool IsLink { get; set; }
        public string UiPageComponentPath { get; set; }
        public Int64 EntityParentKeyId { get; set; }
        public bool IsCurrentStep { get; set; }
        public bool IsCommonStep { get; set; }
        public string StepCode { get; set; }
        public string UiNextPageComponentPath { get; set; }
        public Int64 ProjectSiteRefId { get; set; }
        public ApplicationTypeEnum ApplicationType { get; set; }
        public Int64 AppRefId { get; set; }
        public int ProjectSiteVersion { get; set; }
        public string IPin { get; set; }
        public Int64 InvestPunjab_AppId { get; set; }
        public ApplicationPurposeTypeEnum ApplicationPurposeType { get; set; }
        public string RootActivityRefId { get; set; }
        public ToDoActivityCategoryTypeEnum ToDoActivityCategoryType { get; set; }
        public ToDoActivityModeTypeEnum ToDoActivityModeType { get; set; }
    }
    public class GenericListModel<T>
    {
        public List<T> ListData { get; set; }
        public bool HasError { get; set; }
        public string ErrorDesc { get; set; }
    }
    public class CertificateGenerateServiceResultTemplate
    {
        public string PdfNameGUID { get; set; }

        public bool HasException { get; set; }

        public string Exceptions { get; set; }
    }

    public class NoticeGenerateServiceResultTemplate
    {
        public string PdfNameGUID { get; set; }

        public Int64 EstablishmentEPFOLogsRefId { get; set; }

        public bool HasException { get; set; }

        public string Exceptions { get; set; }
    }
    public class GenericResponseTemplateModel<T>
    {
        public T ResponseDataModel { get; set; }
        public bool HasError { get; set; }
        public string ErrorDesc { get; set; }
    }

    public class GeneratePdfServiceResultTemplate
    {
        public string PdfNameGUID { get; set; }
        public bool HasException { get; set; }
        public string Exceptions { get; set; }
        public string PdfContent { get; set; }
        public string FileNo { get; set; }
    }

    public class CRUD_CreateUpdateOperationResponse
    {
        public CRUD_CreateUpdateOperationResponse()
        {
            HasExceptions = false;
            ErrorDesc = "";
        }
        public bool HasExceptions { get; set; }
        public string ErrorDesc { get; set; }

        public Int64 AppId { get; set; }
        public Int64 EntityKeyId { get; set; }
        public bool IsCrudService { get; set; }
        public string RootActivityRefId { get; set; }
    }
}
