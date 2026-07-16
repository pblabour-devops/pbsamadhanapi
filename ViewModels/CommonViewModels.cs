using Newtonsoft.Json.Linq;
using pbsamadhannetcoreapi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace pbsamadhannetcoreapi.ViewModels
{
    public class TransactionalStoreProcedureResponseViewModel
    {
        public bool IsExcutedSuccessfully { get; set; }
    }
    public class FileUploadResponse
    {
        public Int64 Id { get; set; }
        public string FileName { get; set; }
    }

    //public class TestDapperData
    //{
    //    public int AppDocumentId  { get; set; }
    //    public string FileName  { get; set; }
    //}

    public class TimeFormatViewModal
    {
        public int hour { get; set; }
        public int minute { get; set; }
        public int second { get; set; }
    }

    public class CsvMismatchInfoViewModel
    {
        public string ColumnName { get; set; }
        public CsvColumnMismatchTypeEnum CsvColumnMismatchType { get; set; }
    }
    public class UploadCsvResponseViewModel
    {
        public UploadCsvErrorTypeEnum UploadCsvErrorType { get; set; }
        public List<CsvMismatchInfoViewModel> CsvMismatchList { get; set; }
        public bool IsValidData { get; set; }
        public List<string> ModelColumnList { get; set; }
        public List<UploadCsvIncorrectDataViewModal> Rows { get; set; }
    }

    public class UploadCsvIncorrectDataViewModal
    {
        public JToken Row { get; set; }
        public string ValidationErrors { get; set; }
        public int ExcelSheetRowIndex { get; set; }
    }


    public class UploadCsvRequestViewModel
    {
        public string Data { get; set; }
        public string EntityKey { get; set; }
        public Int64 EntityKeyValue { get; set; }
    }
    public class UploadExcelDuplicateColViewModel
    {
        public string PropertyName { get; set; }
        public bool AllowDuplicateInSheet { get; set; }
        public string CandidateColumnsCommaSeparated { get; set; }
        public string ErrorMessage { get; set; }
    }
    public class UploadExcelDuplicateValDetailViewModel
    {
        public string PropertyName { get; set; }
        public List<string> DuplicateValueList { get; set; }
    }
    public class DateComponentViewModel
    {
        public int year { get; set; }
        public int month { get; set; }
        public int day { get; set; }
    }

    public class InformationViewModel
    {
        public List<PaymentDetailsByLicenceNoViewModel> FeeDetailsList { get; set; }
    }

    public class PaymentDetailsByLicenceNoViewModel
    {
        public string LicenceNo { get; set; }
        public string ApplicationType { get; set; }
        public Decimal PaymentAmount { get; set; }
        public string PaymentHeadName { get; set; }
        public string PaymentDate { get; set; }
    }
    public class XhrRequestDataSetViewModel
    {
        public string RequestData { get; set; }

    }
    public class XhrRequestDataPartsViewModel
    {
        //Request Id
        public string I { get; set; }
        //Data Part
        public string D { get; set; }

        //Keys
        public string K { get; set; }

        //Time
        public string T { get; set; }

        //Client Address
        public string C { get; set; }

        //Locations
        public string L { get; set; }

        //UserName
        public string U { get; set; }

        //ProfileId
        public string P { get; set; }
    }
    public class XhrRequestDataViewModel
    {
        public string RequestData { get; set; }
        //public string DesignatedModel { get; set; }
    }
    public class XhrRequestDataParmsViewModel
    {
        public string Data { get; set; }
        public string DesignatedModel { get; set; }
    }
    public class PropertyFinderRespViewModel
    {
        public bool HasPropName { get; set; }
        public string PropValue { get; set; }
    }

    public class AppSetting_EncryptionConfigs_ViewModel
    {
        public string  TokenDataEncryptionKey { get; set; }
        public string TokenDataIVKey { get; set; }
    }
    public class ThirdPartyTokenVerificationRespViewModel
    {
        public string TokenEncryptedKey { get; set; }
        public string TokenIVKey { get; set; }
        public bool IsValidated { get; set; }
        public string ViolationMsg { get; set; }
    }
    public class TokenClaimPairViewModel
    {
        public string ClaimName { get; set; }
        public string ClaimValue { get; set; }
    }
    public class ValidateGstResponseViewModel
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public ValidateGstResponseDataViewModel Data { get; set; }
    }
    public class ValidateGstResponseDataViewModel
    {
        public string Gstin { get; set; }
        public string TradeName { get; set; }
        public string LegalName { get; set; }
        public string Address { get; set; }
        public string Status { get; set; }
        public string Approving_Auth { get; set; }
        public string Ward { get; set; }
        public string District { get; set; }
    }
}
