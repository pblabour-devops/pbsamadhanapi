using EFCore.BulkExtensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.FileProviders;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using pbsamadhannetcoreapi.CommonUtiliteis;
using pbsamadhannetcoreapi.CommonUtiliteis.CustomeAttributes;
using pbsamadhannetcoreapi.Models;
using pbsamadhannetcoreapi.Repositories;
using pbsamadhannetcoreapi.Repositories.Implementations;
using pbsamadhannetcoreapi.Services.Implementations;
using pbsamadhannetcoreapi.ViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace pbsamadhannetcoreapi.Services
{
    public class CommonApisService : ICommonApisService
    {
        private readonly IZoneRepository _iZoneRepository;
        private readonly IGenericRepository<ApplicationDocument> _iGR_AppDocument;
        private readonly IGeneric_SP_Repository _iGeneric_SP_Repository;
        private readonly IApplicationManagementService<AppFileUploadInfoViewModel> _iApplicationMamnagementService;
        private readonly AppDbContext _context;
        private readonly IGenericRepository<UserActivityLog> _iGR_UserActivityLog;
        private IAuthService _iAuthService;
        private IConfiguration _iConfiguration { get; }
        public CommonApisService(IZoneRepository iZoneRepository,
            IGenericRepository<ApplicationDocument> iGR_AppDocument,
            IGeneric_SP_Repository iGeneric_SP_Repository,
            IApplicationManagementService<AppFileUploadInfoViewModel> iApplicationMamnagementService,
            AppDbContext context,
            IGenericRepository<UserActivityLog> iGR_UserActivityLog,
            IAuthService authService,
            IConfiguration iConfiguration)
        {
            _iZoneRepository = iZoneRepository;
            _iGR_AppDocument = iGR_AppDocument;
            _iGeneric_SP_Repository = iGeneric_SP_Repository;
            _iApplicationMamnagementService = iApplicationMamnagementService;
            _context = context;
            _iGR_UserActivityLog = iGR_UserActivityLog;
            _iAuthService = authService;
            _iConfiguration = iConfiguration;
        }
        public async Task<List<GenericListTemplate>> GetTehsilsByDistrictRefId(long DistrictRefId)
        {
            return await _iZoneRepository.GetTehsilsByDistrictRefId(DistrictRefId);
        }

        public async Task<bool> SaveAppFileInfo(Int64 AppId, Int64 DocId, string fileName)
        {
            try
            {
                ApplicationDocument appDocument = new ApplicationDocument()
                {
                    AppRefId = AppId,
                    AttachmentName = fileName,
                    DocumentRefId = DocId,
                    IsUploaded = true,
                    LastModifiedDate = DateTime.Now,
                    Uploadeddate = DateTime.Now
                };

                _iGR_AppDocument.Insert(appDocument);
                await _iGR_AppDocument.SavechangeAsync();
            }
            catch (Exception ex)
            {
                return false;
            }
            return true;
        }
        public async Task<bool> LockAppDocFiles(Int64 appId, Int64[] appDocIds)
        {
            try
            {
                var parentObject = await _iGR_AppDocument.GetAsync(x => appDocIds.Contains(x.AppDocId)).ConfigureAwait(false);
                var appDocs = parentObject.Select(x => { x.IsLocked = true; return x; }).ToList();
                await _context.BulkUpdateAsync<ApplicationDocument>(appDocs);

                parentObject = await _iGR_AppDocument.GetAsync(x => x.AppRefId == appId && !x.IsLocked).ConfigureAwait(false);
                await _context.BulkDeleteAsync<ApplicationDocument>(parentObject.ToList());
            }
            catch (Exception ex)
            {
                return false;
            }
            return true;
        }

        public async Task<List<AppFileUploadInfoViewModel>> InitiateAppFileInfo(Int64 AppId, bool deleteTempFiles)
        {
            List<AppFileUploadInfoViewModel> filesDetail = new List<AppFileUploadInfoViewModel>();

            try
            {
                //1. Delete existing temp files
                //if (deleteTempFiles)
                //{
                //    var tempFilePath = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "TempFileUpload")).Root;
                //    DirectoryInfo dir = new DirectoryInfo(tempFilePath);
                //    foreach (FileInfo fi in dir.GetFiles(AppId + "_*"))
                //    {
                //        fi.Delete();
                //    }
                //}
                //2. Initiate already uploaded files
                filesDetail = await _iApplicationMamnagementService.InitiateAppFileInfo(AppId);
                //List<StoreProcedureParm> storeProcedureParms = new List<StoreProcedureParm>()
                //{
                //    new StoreProcedureParm (){ ParmName="AppId", ParmValue= AppId.ToString(), isNumber=true }
                //};
                //filesDetail = await _iGeneric_SP_Repository.CallStoreProcedureReaderAsync<AppFileUploadInfoViewModel>("dbo.sp_Get_AppType_Allowed_File_Files", storeProcedureParms);

                //if (filesDetail.Count() > 0)
                //{
                //    foreach (var file in filesDetail)
                //    {
                //        if (file.AlreadyUploaded != null)
                //        {
                //            string[] alreadyUploadedFileInfo = file.AlreadyUploaded.Split(',');
                //            if (alreadyUploadedFileInfo.Length > 0)
                //            {
                //                file.AlreadyUploadedInfo = new List<ViewModels.AppFileAlreadyFileUploadedViewModel>();
                //                foreach (var alreadyFile in alreadyUploadedFileInfo)
                //                {
                //                    string[] fileInfo = alreadyFile.Split('=');
                //                    file.AlreadyUploadedInfo.Add(new ViewModels.AppFileAlreadyFileUploadedViewModel()
                //                    {
                //                        FileName = fileInfo[0],
                //                        FileUploadOn = Convert.ToDateTime(fileInfo[1])
                //                    });
                //                }
                //            }
                //        }
                //    }
                //}
            }
            catch (Exception ex)
            {

            }
            return filesDetail;
        }
        public async Task<GenericFormModel<dynamic>> GetApplicationStepper(Int64 entityParentKeyId, long appId, ApplicationTypeEnum ApplicationType)
        {
            GenericFormModel<dynamic> formModel = new GenericFormModel<dynamic>();
            formModel.AppFormStepsList = new List<AppFormStepsInfo>();
            try
            {
                formModel.AppFormStepsList = await _iApplicationMamnagementService.GetAppFormStepperInfo(appId, ApplicationType, entityParentKeyId, "DOC");

            }
            catch (Exception ex)
            {
                return null;
            }
            return formModel;
        }
        public async Task<GenericFormModel<List<DistrictViewModel>>> GetAllDistrict()
        {
            GenericFormModel<List<DistrictViewModel>> genericFormModel = new GenericFormModel<List<DistrictViewModel>>();
            try
            {
                genericFormModel.FormModel = await _iGeneric_SP_Repository.CallStoreProcedureReaderAsyncWithoutParams<DistrictViewModel>("sp_GetDistrictDetails");
            }
            catch (Exception ex)
            {
                genericFormModel.HasError = true;
                genericFormModel.ErrorDesc = ex.Message;
            }
            return genericFormModel;
        }

        public async Task<GenericFormModel<List<NICCode>>> GetAllNICCodes()
        {
            GenericFormModel<List<NICCode>> genericFormModel = new GenericFormModel<List<NICCode>>();
            try
            {
                genericFormModel.FormModel = await _iGeneric_SP_Repository.CallStoreProcedureReaderAsyncWithoutParams<NICCode>("sp_Get_All_NIC_Codes");
            }
            catch (Exception ex)
            {
                genericFormModel.HasError = true;
                genericFormModel.ErrorDesc = ex.Message;
            }
            return genericFormModel;
        }

        public async Task<List<AppFileUploadInfoViewModel>> InitiateAppFileInfoWithRoleId(Int64 appId, bool deleteTempFiles, string userId, int currentActionCode, int allowedActionCode, Int64 applicationaType, bool isTimeLineFlow, Int64 applicationActionLogId)
        {
            List<AppFileUploadInfoViewModel> filesDetail = new List<AppFileUploadInfoViewModel>();

            try
            {
                //1. Delete existing temp files
                if (deleteTempFiles)
                {
                    var tempFilePath = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "TempFileUpload")).Root;
                    DirectoryInfo dir = new DirectoryInfo(tempFilePath);
                    foreach (FileInfo fi in dir.GetFiles(appId + "_*"))
                    {
                        fi.Delete();
                    }
                }
                //2. Initiate already uploaded files
                filesDetail = await _iApplicationMamnagementService.InitiateAppFileInfoWithRoleId(appId, userId, currentActionCode, allowedActionCode, applicationaType, isTimeLineFlow, applicationActionLogId);
            }
            catch (Exception ex)
            {

            }
            return filesDetail;
        }
        public async Task<GenericResponseTemplateModel<FileUploadResponse>> LogFileInfo(Int64 AppId, Int64 DocId, Int64 appDocId, string fileName)
        {
            GenericResponseTemplateModel<FileUploadResponse> genericResponseTemplate = new GenericResponseTemplateModel<FileUploadResponse>() { ErrorDesc = null, HasError = false };
            try
            {

                if (appDocId != 0)
                {
                    var foundDocument = _iGR_AppDocument.GetById(appDocId);
                    foundDocument.LastModifiedDate = DateTime.Now;

                    _iGR_AppDocument.Update(foundDocument);
                    await _iGR_AppDocument.SavechangeAsync();
                    genericResponseTemplate.ResponseDataModel = new FileUploadResponse()
                    {
                        FileName = foundDocument.AttachmentName,
                        Id = foundDocument.AppDocId
                    };
                }
                else
                {
                    ApplicationDocument appDocument = new ApplicationDocument()
                    {
                        AppRefId = AppId,
                        AttachmentName = fileName,
                        DocumentRefId = DocId,
                        IsUploaded = true,
                        LastModifiedDate = DateTime.Now,
                        Uploadeddate = DateTime.Now
                    };
                    _iGR_AppDocument.Insert(appDocument);
                    await _iGR_AppDocument.SavechangeAsync();


                    fileName = AppId + "_" + DocId + "_" + appDocument.AppDocId + fileName;
                    var foundDocument = _iGR_AppDocument.GetById(appDocument.AppDocId);
                    foundDocument.AttachmentName = fileName;

                    _iGR_AppDocument.Update(foundDocument);
                    await _iGR_AppDocument.SavechangeAsync();


                    genericResponseTemplate.ResponseDataModel = new FileUploadResponse()
                    {
                        FileName = foundDocument.AttachmentName,
                        Id = appDocument.AppDocId
                    };
                }
            }
            catch (Exception ex)
            {
                genericResponseTemplate.ResponseDataModel = new FileUploadResponse()
                {
                    FileName = "",
                    Id = 0
                };
                genericResponseTemplate.HasError = true;
                genericResponseTemplate.ErrorDesc = ex.Message;
                return genericResponseTemplate;
            }
            return genericResponseTemplate;
        }

        public async Task<GenericResponseTemplateModel<EstablishmentCardInfoViewModel>> GetEstablishmentCardDetails(Int64 AppRefId)
        {
            GenericResponseTemplateModel<EstablishmentCardInfoViewModel> genericFormModel = new GenericResponseTemplateModel<EstablishmentCardInfoViewModel>();
            try
            {
                List<StoreProcedureParm> storeProcedureParms = new List<StoreProcedureParm>()
                {
                    new StoreProcedureParm (){ ParmName="AppRefId", ParmValue=AppRefId.ToString(), isNumber=true}
                };
                var EstablishmentDetails = await _iGeneric_SP_Repository.CallStoreProcedureReaderAsync<EstablishmentCardInfoViewModel>("sp_GetEstablishmentBasicDetailsByAppRefId", storeProcedureParms);
                genericFormModel.ResponseDataModel = EstablishmentDetails.FirstOrDefault();
            }
            catch (Exception ex)
            {
                genericFormModel.HasError = true;
                genericFormModel.ErrorDesc = ex.Message;
            }
            return genericFormModel;
        }

        public async Task<GenericResponseTemplateModel<bool>> LogApplicationAddendumDocument(ApplicationAddendumDocument obj)
        {
            GenericResponseTemplateModel<bool> genericServiceResultTemplate = new GenericResponseTemplateModel<bool>() { ResponseDataModel = true, ErrorDesc = "", HasError = false };
            try
            {
                await _context.ApplicationAddendumDocuments.AddAsync(obj);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                genericServiceResultTemplate.ResponseDataModel = false;
                genericServiceResultTemplate.HasError = true;
                genericServiceResultTemplate.ErrorDesc = ex.InnerException.Message;
            }
            return genericServiceResultTemplate;
        }

        public async Task<GenericResponseTemplateModel<List<ApplicationAddendumDocument>>> GetApplicationAddendumDocuments(Int64 appRefId)
        {
            GenericResponseTemplateModel<List<ApplicationAddendumDocument>> genericServiceResultTemplate = new GenericResponseTemplateModel<List<ApplicationAddendumDocument>>() { ErrorDesc = "", HasError = false };
            try
            {
                genericServiceResultTemplate.ResponseDataModel = _context.ApplicationAddendumDocuments.Where(x => x.AppRefId == appRefId).ToList();
            }
            catch (Exception ex)
            {
                genericServiceResultTemplate.ResponseDataModel = null;
                genericServiceResultTemplate.HasError = true;
                genericServiceResultTemplate.ErrorDesc = ex.InnerException.Message;
            }
            return genericServiceResultTemplate;
        }
        public async Task<GenericResponseTemplateModel<bool>> DetermineAddendumUploadOptionShown(Int64 appRefId)
        {
            GenericResponseTemplateModel<bool> genericServiceResultTemplate = new GenericResponseTemplateModel<bool>() { ErrorDesc = "", HasError = false };
            try
            {
                List<StoreProcedureParm> storeProcedureParms = new List<StoreProcedureParm>()
                {
                    new StoreProcedureParm (){ ParmName="AppRefId", ParmValue=appRefId.ToString(), isNumber=true}
                };
                var data = await _iGeneric_SP_Repository.CallStoreProcedureReaderScalerAsync("sp_DetermineAddendumUploadOptionShown", storeProcedureParms);
                genericServiceResultTemplate.ResponseDataModel = Convert.ToBoolean(data.Value);
                //genericServiceResultTemplate.ResponseDataModel = _context.Applications.Any(x => x.AppId == appRefId && x.IsDeleted == false && (x.ApplicationLifeCycleStatusType == ApplicationLifeCycleStatusTypeEnum.IN_PROCESS || x.ApplicationLifeCycleStatusType == ApplicationLifeCycleStatusTypeEnum.IN_OBJECTION));
            }
            catch (Exception ex)
            {
                genericServiceResultTemplate.ResponseDataModel = false;
                genericServiceResultTemplate.HasError = true;
                genericServiceResultTemplate.ErrorDesc = ex.InnerException.Message;
            }
            return genericServiceResultTemplate;
        }


        public async Task<GenericResponseTemplateModel<UploadCsvResponseViewModel>> ValidateAndSeedCsvData<T>(string data, string entityKey, Int64 entityKeyValue) where T : class
        {
            GenericResponseTemplateModel<UploadCsvResponseViewModel> genericFormModel = new GenericResponseTemplateModel<UploadCsvResponseViewModel>()
            {
                ErrorDesc = "",
                HasError = false,
                ResponseDataModel = new UploadCsvResponseViewModel()
                {
                    CsvMismatchList = new List<CsvMismatchInfoViewModel>(),
                    //Data = "",
                    IsValidData = true,
                    UploadCsvErrorType = UploadCsvErrorTypeEnum.ALL_OK,
                    Rows = new List<UploadCsvIncorrectDataViewModal>()
                },
            };
            try
            {
                List<UploadExcelDuplicateColViewModel> uploadExcelDuplicateCols = new List<UploadExcelDuplicateColViewModel>();

                JObject parsed = JObject.Parse(data);
                JArray SheetDataArray = (JArray)parsed["sheetData"];
                var sheetData = SheetDataArray.ToList();

                var firstItemOfArray = SheetDataArray.FirstOrDefault();

                if (firstItemOfArray == null)
                {
                    genericFormModel = new GenericResponseTemplateModel<UploadCsvResponseViewModel>()
                    {
                        ErrorDesc = "",
                        HasError = false,
                        ResponseDataModel = new UploadCsvResponseViewModel()
                        {
                            CsvMismatchList = new List<CsvMismatchInfoViewModel>(),
                            //Data = "",
                            IsValidData = false,
                            UploadCsvErrorType = UploadCsvErrorTypeEnum.EMPTY_SHEET,
                            Rows = new List<UploadCsvIncorrectDataViewModal>(),
                            ModelColumnList = new List<string>()
                        }
                    };
                }

                else
                {
                    var jsonInfo = firstItemOfArray.ToObject<Dictionary<string, string>>();
                    var suppliedKeys = jsonInfo.Keys.ToArray();

                    var modelKeys = typeof(T).GetRuntimeProperties();

                    //var modelKeys = json.SheetData.FirstOrDefault().GetType().GetProperties();

                    List<string> modelRequiredFields = new List<string>();

                    foreach (PropertyInfo property in modelKeys)
                    {
                        var keyAttribute = Attribute.GetCustomAttribute(property, typeof(KeyAttribute))
                            as KeyAttribute;

                        var notMapped = Attribute.GetCustomAttribute(property, typeof(NotMappedAttribute))
                            as NotMappedAttribute;

                        var foreignMapped = Attribute.GetCustomAttribute(property, typeof(ForeignKeyAttribute))
                            as ForeignKeyAttribute;

                        var reqMapped = Attribute.GetCustomAttribute(property, typeof(RequiredAttribute))
                            as RequiredAttribute;

                        var excelUniqueColumn = Attribute.GetCustomAttribute(property, typeof(CustomeAttribute_ExcelUniqueColumn))
                            as CustomeAttribute_ExcelUniqueColumn;
                        if (excelUniqueColumn != null)
                        {
                            uploadExcelDuplicateCols.Add(new UploadExcelDuplicateColViewModel()
                            {
                                PropertyName = property.Name,
                                AllowDuplicateInSheet = excelUniqueColumn.AllowDuplicateInSheet,
                                CandidateColumnsCommaSeparated = excelUniqueColumn.CandidateColumnsCommaSeparated,
                                ErrorMessage = excelUniqueColumn.ErrorMessage
                            });
                        }


                        if (reqMapped != null && keyAttribute == null && notMapped == null && foreignMapped == null)
                        {
                            modelRequiredFields.Add(property.Name);
                        }
                    }

                    //Missing Props
                    var misingProps = modelRequiredFields.Where(x => !suppliedKeys.Contains(x)).ToList();
                    var extraProps = suppliedKeys.Where(x => !modelRequiredFields.Contains(x)).ToList();

                    if (misingProps.Count() != 0 || extraProps.Count() != 0)
                    {
                        genericFormModel = new GenericResponseTemplateModel<UploadCsvResponseViewModel>()
                        {
                            ErrorDesc = "",
                            HasError = false,
                            ResponseDataModel = new UploadCsvResponseViewModel()
                            {
                                CsvMismatchList = new List<CsvMismatchInfoViewModel>(),
                                //Data = "",
                                IsValidData = false,
                                UploadCsvErrorType = UploadCsvErrorTypeEnum.COLUMN_MISMATCH,
                                ModelColumnList = modelRequiredFields
                            }
                        };

                        foreach (var item in misingProps)
                        {
                            genericFormModel.ResponseDataModel.CsvMismatchList.Add(new CsvMismatchInfoViewModel()
                            {
                                ColumnName = item,
                                CsvColumnMismatchType = CsvColumnMismatchTypeEnum.MISSED
                            });
                        }
                        foreach (var item in extraProps)
                        {
                            genericFormModel.ResponseDataModel.CsvMismatchList.Add(new CsvMismatchInfoViewModel()
                            {
                                ColumnName = item,
                                CsvColumnMismatchType = CsvColumnMismatchTypeEnum.EXTRA
                            });
                        }
                    }
                    else
                    {
                        if (sheetData.Count() > 0)
                        {
                            int i = 0;
                            StringBuilder error = new StringBuilder();
                            List<T> dataObjList = new List<T>();

                            List<UploadExcelDuplicateValDetailViewModel> duplicateInfoList = new List<UploadExcelDuplicateValDetailViewModel>();


                            if (uploadExcelDuplicateCols != null && uploadExcelDuplicateCols.Count() > 0)
                            {
                                foreach (var item in uploadExcelDuplicateCols)
                                {
                                    if (!item.AllowDuplicateInSheet)
                                    {
                                        var duplicates = sheetData.GroupBy(x => x[item.PropertyName]).Where(x => x.Count() > 1).Select(x => x.Key.ToString()).ToList();
                                        if (duplicates != null)
                                        {
                                            duplicateInfoList.Add(new UploadExcelDuplicateValDetailViewModel()
                                            {
                                                PropertyName = item.PropertyName,
                                                DuplicateValueList = duplicates

                                            });
                                        }
                                    }
                                }
                            }

                            foreach (var row in sheetData)
                            {
                                T obj = (T)Activator.CreateInstance(typeof(T));
                                foreach (var col in suppliedKeys)
                                {
                                    var sheetRowData = row.ToObject<Dictionary<string, string>>();
                                    try
                                    {
                                        var isEnum = obj.GetType().GetProperty(col).PropertyType.IsEnum;
                                        if (isEnum)
                                        {
                                            var enumType = Type.GetType(obj.GetType().GetProperty(col).PropertyType.FullName);
                                            GenericModelOps<T>.TrySetProperty(obj, col, Enum.Parse(enumType, sheetRowData[col]));
                                        }

                                        else if (obj.GetType().GetProperty(col).PropertyType == typeof(DateTime))
                                        {
                                            GenericModelOps<T>.TrySetProperty(obj, col, Convert.ToDateTime(sheetRowData[col]));
                                        }
                                        else
                                        {
                                            GenericModelOps<T>.TrySetProperty(obj, col, sheetRowData[col]);
                                        }
                                    }
                                    catch (Exception ex)
                                    {
                                        //if (obj.GetType().GetProperty(col).PropertyType != typeof(DateTime))
                                        //{/
                                        GenericModelOps<T>.TrySetProperty(obj, col, null);
                                        //}
                                    }
                                }
                                dataObjList.Add(obj);
                            }

                            var json = JsonConvert.DeserializeObject<List<T>>(JsonConvert.SerializeObject(dataObjList));
                            foreach (var item in json)
                            {
                                var varificationResult = CustomeValidator<T>.ValidateModel_AllProperties(item);
                                int j = 1;
                                error.Clear();
                                error.Append("<ul>");


                                var isDuplicate = false;
                                var duplicateErrorMsg = "";
                                foreach (var duplicateItem in duplicateInfoList)
                                {
                                    if (duplicateItem.DuplicateValueList.Contains(item.GetType().GetProperty(duplicateItem.PropertyName).GetValue(item).ToString()))
                                    {
                                        isDuplicate = true;
                                        duplicateErrorMsg = uploadExcelDuplicateCols.Where(x => x.PropertyName == duplicateItem.PropertyName).Select(x => x.ErrorMessage).FirstOrDefault();
                                    }
                                }

                                //json.SheetData[i].IsValidRow = true;

                                var allDateTypeProps = item.GetType().GetProperties().Where(x => x.PropertyType == typeof(DateTime)).ToList();
                                bool areAllDatesAreCorrect = true;
                                foreach (var dateTypeProp in allDateTypeProps)
                                {
                                    var dateVal = dateTypeProp.GetValue(item);
                                    if (Convert.ToDateTime(dateVal) == DateTime.MinValue)
                                    {
                                        areAllDatesAreCorrect = false;

                                        error.Append("<li>" + j.ToString() + ". Invalid " + dateTypeProp.Name + "..!</li>");
                                        j++;
                                    }
                                }

                                if (!varificationResult.IsValid || !areAllDatesAreCorrect || isDuplicate)
                                {
                                    //json.SheetData[i].IsValidRow = false;
                                    if (varificationResult.CustomeValidationErrorList != null)
                                    {
                                        foreach (var errorItem in varificationResult.CustomeValidationErrorList)
                                        {
                                            error.Append("<li>" + j.ToString() + ". " + errorItem.ErrorMessage + "</li>");
                                            j++;
                                        }
                                    }
                                    if (isDuplicate)
                                    {
                                        error.Append("<li>" + j.ToString() + ". " + duplicateErrorMsg + "</li>");
                                        j++;
                                    }
                                    genericFormModel.ResponseDataModel.Rows.Add(new UploadCsvIncorrectDataViewModal
                                    {
                                        Row = sheetData[i],
                                        ExcelSheetRowIndex = i,
                                        ValidationErrors = error.ToString()
                                    });
                                    //json.SheetData[i].ValidationError = error.ToString();
                                }
                                i++;
                                error.Append("</ul>");
                            }

                            if (genericFormModel.ResponseDataModel.Rows.Count() > 0)
                            {
                                genericFormModel.ResponseDataModel.UploadCsvErrorType = UploadCsvErrorTypeEnum.DATA_VALIDATION;
                                genericFormModel.ResponseDataModel.IsValidData = false;
                                genericFormModel.ResponseDataModel.ModelColumnList = modelRequiredFields;
                            }
                            else
                            {
                                if (typeof(T).Name == "ShopLicence_EmployeeDetail")
                                {
                                    var emps = _context.ShopLicence_EmployeeDetails.Where(x => x.ShopLicenceRefId == entityKeyValue).ToList();
                                    if (emps.Count() > 0)
                                    {
                                        _context.ShopLicence_EmployeeDetails.RemoveRange(emps);
                                        _context.SaveChanges();
                                    }
                                }

                                json = json.Select(x => { x.GetType().GetProperty(entityKey).SetValue(x, entityKeyValue); return x; }).ToList();
                                await _context.BulkInsertAsync<T>(json);
                                await _context.SaveChangesAsync();
                            }
                            //if (json.SheetData.Any(x => !x.IsValidRow))
                            //{
                            //    genericFormModel.ResponseDataModel = new UploadCsvResponseViewModel()
                            //    {
                            //        IsValidData = false,
                            //        Data = JsonConvert.SerializeObject(json)
                            //    };
                            //}
                            //else //All Good and insert data
                            //{
                            //    json.SheetData = json.SheetData.Select(x => { x.ShopLicenceRefId = 10041; return x; }).ToList();
                            //    await _context.BulkInsertAsync<ShopLicence_EmployeeDetail>(json.SheetData);
                            //    await _context.SaveChangesAsync();

                            //}
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                genericFormModel.HasError = true;
                genericFormModel.ErrorDesc = ex.Message;
            }
            return genericFormModel;
        }

        public async Task<GenericResponseTemplateModel<IntReturn>> FindDuplicateGST(string gstNumber, Int64 projectSiteRefId, int applicationType)
        {
            GenericResponseTemplateModel<IntReturn> genericResponseTemplateModel = new GenericResponseTemplateModel<IntReturn>();
            try
            {
                List<StoreProcedureParm> storeProcedureParms = new List<StoreProcedureParm>()
                {
                    new StoreProcedureParm (){ ParmName="GstNumber", ParmValue=gstNumber, isNumber=false},
                    new StoreProcedureParm (){ ParmName="ApplicationType", ParmValue=applicationType.ToString(), isNumber=true},
                    new StoreProcedureParm (){ ParmName="ProjectSiteRefId", ParmValue=projectSiteRefId.ToString(), isNumber=true}
                };
                genericResponseTemplateModel.ResponseDataModel = await _iGeneric_SP_Repository.CallStoreProcedureReaderScalerAsync("sp_FindDuplicateGst", storeProcedureParms);
            }
            catch (Exception ex)
            {
                genericResponseTemplateModel.HasError = true;
                genericResponseTemplateModel.ErrorDesc = ex.Message;
            }
            return genericResponseTemplateModel;
        }

        public async Task<GenericResponseTemplateModel<IntReturn>> FindDuplicatePanNo(string panOrTanNumber, Int64 projectSiteRefId, int applicationType)
        {
            GenericResponseTemplateModel<IntReturn> genericResponseTemplateModel = new GenericResponseTemplateModel<IntReturn>();
            try
            {
                List<StoreProcedureParm> storeProcedureParms = new List<StoreProcedureParm>()
                {
                    new StoreProcedureParm (){ ParmName="PanOrTanNumber", ParmValue=panOrTanNumber, isNumber=false},
                    new StoreProcedureParm (){ ParmName="ApplicationType", ParmValue=applicationType.ToString(), isNumber=true},
                    new StoreProcedureParm (){ ParmName="ProjectSiteRefId", ParmValue=projectSiteRefId.ToString(), isNumber=true}
                };
                genericResponseTemplateModel.ResponseDataModel = await _iGeneric_SP_Repository.CallStoreProcedureReaderScalerAsync("sp_FindDuplicatePanOrTan", storeProcedureParms);
            }
            catch (Exception ex)
            {
                genericResponseTemplateModel.HasError = true;
                genericResponseTemplateModel.ErrorDesc = ex.Message;
            }
            return genericResponseTemplateModel;
        }

        public async Task<GenericResponseTemplateModel<DateTime>> GetReportUpdatedDate()
        {
            GenericResponseTemplateModel<DateTime> genericResponseTemplateModel = new GenericResponseTemplateModel<DateTime>();
            try
            {
                genericResponseTemplateModel.ResponseDataModel = _context.MISDashboardEngineStatus.Select(x => x.LastUpdatedOn).FirstOrDefault();
            }
            catch (Exception ex)
            {
                genericResponseTemplateModel.HasError = true;
                genericResponseTemplateModel.ErrorDesc = ex.Message;
            }
            return genericResponseTemplateModel;
        }

        public async Task<GenericServiceResultTemplate> CreateActivityLogs(UserActivityLog formModel)
        {
            GenericServiceResultTemplate genericServiceResultTemplate = new GenericServiceResultTemplate();
            try
            {
                genericServiceResultTemplate.HasException = false;
                genericServiceResultTemplate.CustomeValidationResult = CustomeValidator<UserActivityLog>.ValidateModel_AllProperties(formModel);
                if (genericServiceResultTemplate.CustomeValidationResult.IsValid)
                {
                    formModel.LoginTime = DateTime.Now;
                    _iGR_UserActivityLog.Insert(formModel);
                    await _iGR_UserActivityLog.SavechangeAsync();
                }
            }
            catch (Exception ex)
            {
                genericServiceResultTemplate.HasException = true;
                genericServiceResultTemplate.Exceptions = ex;
            }
            return genericServiceResultTemplate;
        }

        public async Task<GenericResponseTemplateModel<InformationViewModel>> GetInformationByLicenceNo(string licenceNo)
        {
            GenericResponseTemplateModel<InformationViewModel> genericFormModel = new GenericResponseTemplateModel<InformationViewModel>();
            try
            {
                List<StoreProcedureParm> storeProcedureParms = new List<StoreProcedureParm>()
                {
                    new StoreProcedureParm (){ ParmName="LicenceNo", ParmValue=licenceNo.ToString(), isNumber=false}
                };
                var resp = await _iGeneric_SP_Repository.CallStoreProcedureReaderAsync<PaymentDetailsByLicenceNoViewModel>("sp_GetPaymentDetailsByLicenceNo", storeProcedureParms);

                var informationViewModel = new InformationViewModel
                {
                    FeeDetailsList = resp
                };
                genericFormModel.ResponseDataModel = informationViewModel;
            }
            catch (Exception ex)
            {
                genericFormModel.HasError = true;
                genericFormModel.ErrorDesc = ex.Message;
            }
            return genericFormModel;
        }

        public async Task<GenericResponseTemplateModel<List<WhatsNewInPortal>>> GetWhatsNewInPortal(string roleName)
        {
            GenericResponseTemplateModel<List<WhatsNewInPortal>> genericRespModel = new GenericResponseTemplateModel<List<WhatsNewInPortal>>();
            try
            {
                genericRespModel.ResponseDataModel = await _context.WhatsNewInPortal.Where(x => ("," + x.Role + ",").Contains("," + roleName + ",") && x.ExpiryDate >= DateTime.Now ).OrderByDescending(x => x.Id).ToListAsync();

            }
            catch (Exception ex)
            {
                genericRespModel.HasError = true;
                genericRespModel.ErrorDesc = ex.Message;
            }
            return genericRespModel;
        }

        public async Task<GenericServiceResultTemplate> AddUpdateWhatsNewInPortal(WhatsNewInPortal requestData)
        {
            GenericServiceResultTemplate genericServiceResultTemplate = new GenericServiceResultTemplate();
            try
            {
                if (requestData != null)
                {
                    await _context.AddAsync(requestData);
                    await _context.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                genericServiceResultTemplate.HasException = true;
                genericServiceResultTemplate.Exceptions = ex;
            }
            return genericServiceResultTemplate;
        }
        public async Task<GenericResponseTemplateModel<ValidateGstResponseViewModel>> ValidateGst(string gstNumber)
        {
            GenericResponseTemplateModel<ValidateGstResponseViewModel> genericRespModel = new GenericResponseTemplateModel<ValidateGstResponseViewModel>();
            try
            {
                string apiUrl = _iConfiguration.GetSection("ThirdPartyIntegrationConfigs").GetSection("GstIntegration").GetSection("GstVarificationPostUrl").Value;
                using (var httpClient = new HttpClient())
                {
                    httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                    var requestBody= "{\"gstNo\":\""+ gstNumber + "\",\"projectModuleTypeId\":17}";
                    var encRequestBody = RijndaelManagedCryptoHandler.EncryptFromPlainTextToBase64(requestBody,
                        _iConfiguration.GetSection("ThirdPartyIntegrationConfigs").GetSection("GstIntegration").GetSection("EncryptionKey").Value,
                        _iConfiguration.GetSection("ThirdPartyIntegrationConfigs").GetSection("GstIntegration").GetSection("EncryptionIVKey").Value);

                    StringContent content = new StringContent("{\"body\":\""+ encRequestBody + "\"}",
                        Encoding.UTF8, "application/json");

                    using (var response = await httpClient.PostAsync(apiUrl, content))
                    {
                        var encryptedResp = await response.Content.ReadAsStringAsync();
                        if(response.StatusCode == System.Net.HttpStatusCode.OK)
                        {
                            var plainResponse = RijndaelManagedCryptoHandler.DecryptFromPlainTextToBase64(JsonConvert.DeserializeObject<string>(encryptedResp.ToString()),
                                _iConfiguration.GetSection("ThirdPartyIntegrationConfigs").GetSection("GstIntegration").GetSection("EncryptionKey").Value,
                                _iConfiguration.GetSection("ThirdPartyIntegrationConfigs").GetSection("GstIntegration").GetSection("EncryptionIVKey").Value);
                            genericRespModel.ResponseDataModel = JsonConvert.DeserializeObject<ValidateGstResponseViewModel>(plainResponse);
                            genericRespModel.HasError = false;
                            genericRespModel.ErrorDesc = "";
                        }
                        else
                        {
                            genericRespModel.HasError = true;
                            genericRespModel.ErrorDesc = "Error in calling api..!";
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                genericRespModel.HasError = true;
                genericRespModel.ErrorDesc = ex.Message;
            }
            return genericRespModel;
        }
    }
}
