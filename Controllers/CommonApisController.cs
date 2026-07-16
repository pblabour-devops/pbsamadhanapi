using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Primitives;
using Newtonsoft.Json;
using Org.BouncyCastle.Asn1.Ocsp;
using pbsamadhannetcoreapi.CommonUtiliteis;
using pbsamadhannetcoreapi.Models;
using pbsamadhannetcoreapi.Services;
using pbsamadhannetcoreapi.Services.Implementations;
using pbsamadhannetcoreapi.ViewModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace pbsamadhannetcoreapi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    //[Authorize]
    public class CommonApisController : Controller
    {
        private readonly ICommonApisService _iCommonApisService;
        private readonly IApplicationManagementService<dynamic> _iApplicationMamnagementService;
        private IHttpContextAccessor _httpContextAccessor;
        public CommonApisController(ICommonApisService iCommonApisService,
            IApplicationManagementService<dynamic> iApplicationMamnagementService,
            IHttpContextAccessor httpContextAccessor)
        {
            _iCommonApisService = iCommonApisService;
            _iApplicationMamnagementService = iApplicationMamnagementService;
            _httpContextAccessor = httpContextAccessor;
        }

        [HttpGet, Route("gettehsilsbydistrictrefid")]
        [CustomFillters.AuthorizeAttribute("INDL")]
        public async Task<IActionResult> GetTehsilsByDistrictRefId([FromQuery] Int64 id)
        {
            List<GenericListTemplate> tehsils = new List<GenericListTemplate>();
            try
            {
                tehsils = await _iCommonApisService.GetTehsilsByDistrictRefId(id);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
            return StatusCode(StatusCodes.Status200OK, tehsils);
        }

        [HttpPost]
        [Route("uploadappdocs")]
        [CustomFillters.AuthorizeAttribute("INDL")]
        public async Task<IActionResult> UploadAppDocs()
        {
            bool isSomethingWrong = false;
            try
            {
                var files = Request.Form.Files;
                if (files.Count() > 0)
                {
                    StringValues docIds, appIds;
                    isSomethingWrong = Request.Headers.TryGetValue("docid", out docIds);
                    isSomethingWrong = Request.Headers.TryGetValue("appid", out appIds);
                    if (docIds.Count() == 0 || appIds.Count() == 0)
                    {
                        isSomethingWrong = true;
                    }
                    else
                    {
                        isSomethingWrong = false;
                        var filepath = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "TempFileUpload")).Root;

                        string DocId = docIds[0];
                        string AppId = appIds[0];
                        foreach (var file in files)
                        {
                            filepath = filepath + AppId + "_" + DocId + file.Name.Substring(file.FileName.LastIndexOf('.'));
                            using (FileStream fs = System.IO.File.Create(filepath))
                            {
                                file.CopyTo(fs);
                                fs.Flush();
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                isSomethingWrong = true;
            }
            if (isSomethingWrong)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, false);
            }
            return StatusCode(StatusCodes.Status200OK, true);
        }

        [HttpGet]
        [Route("getAppAddendumDocs")]
        public async Task<IActionResult> GetAppAddendumDocs([FromQuery] Int64 id)
        {
            GenericResponseTemplateModel<List<ApplicationAddendumDocument>> genericServiceResultTemplate = new GenericResponseTemplateModel<List<ApplicationAddendumDocument>>() { ErrorDesc = "", HasError = false };
            try
            {
                genericServiceResultTemplate = await _iCommonApisService.GetApplicationAddendumDocuments(id);
            }
            catch (Exception ex)
            {
                genericServiceResultTemplate.ResponseDataModel = null;
                genericServiceResultTemplate.HasError = true;
                genericServiceResultTemplate.ErrorDesc = ex.Message;
            }
            if (genericServiceResultTemplate.HasError)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, genericServiceResultTemplate);
            }
            return StatusCode(StatusCodes.Status200OK, genericServiceResultTemplate);
        }

        [HttpGet]
        [Route("determineAddendumUploadOptionShown")]
        public async Task<IActionResult> DetermineAddendumUploadOptionShown([FromQuery] Int64 id)
        {
            GenericResponseTemplateModel<bool> genericServiceResultTemplate = new GenericResponseTemplateModel<bool>() { ErrorDesc = "", HasError = false };
            try
            {
                genericServiceResultTemplate = await _iCommonApisService.DetermineAddendumUploadOptionShown(id);
            }
            catch (Exception ex)
            {
                genericServiceResultTemplate.ResponseDataModel = false;
                genericServiceResultTemplate.HasError = true;
                genericServiceResultTemplate.ErrorDesc = ex.Message;
            }
            if (genericServiceResultTemplate.HasError)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, genericServiceResultTemplate);
            }
            return StatusCode(StatusCodes.Status200OK, genericServiceResultTemplate);
        }

        [HttpPost]
        [Route("UploadAppAddendum")]
        public async Task<IActionResult> UploadAppAddendum()
        {
            bool isSomethingWrong = false;
            try
            {
                var files = Request.Form.Files;
                if (files.Count() > 0)
                {
                    StringValues titles, appIds;
                    //isSomethingWrong = Request.Headers.TryGetValue("docid", out docIds);
                    isSomethingWrong = Request.Headers.TryGetValue("appid", out appIds);
                    isSomethingWrong = Request.Headers.TryGetValue("title", out titles);
                    if (appIds.Count() == 0)
                    {
                        isSomethingWrong = true;
                    }
                    else
                    {
                        isSomethingWrong = false;
                        string appId = appIds[0];
                        string title = titles[0];
                        //string DocId = docIds[0];

                        foreach (var file in files)
                        {
                            Guid guid = Guid.NewGuid();
                            var filepath = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "AppAddendums")).Root;
                            var fileName = appId + "_" + guid.ToString() + file.Name.Substring(file.FileName.LastIndexOf('.'));
                            filepath = filepath + fileName;


                            ApplicationAddendumDocument obj = new ApplicationAddendumDocument()
                            {
                                AppRefId = Convert.ToInt64(appId),
                                AttachmentName = fileName,
                                LastModifiedDate = DateTime.Now,
                                Title = title
                            };
                            var serResp = await _iCommonApisService.LogApplicationAddendumDocument(obj);
                            if(!serResp.HasError && serResp.ResponseDataModel)
                            {
                                using (FileStream fs = System.IO.File.Create(filepath))
                                {
                                    file.CopyTo(fs);
                                    fs.Flush();
                                }
                            }
                            else
                            {
                                isSomethingWrong = true;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                isSomethingWrong = true;
            }
            if (isSomethingWrong)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, false);
            }
            return StatusCode(StatusCodes.Status200OK, true);
        }

        [HttpGet, Route("lockuploadfiles")]
        public async Task<IActionResult> Get_LockUploadFiles([FromQuery] Int64 id, string appDocIds)
        {
            bool isSomethingWentWrong = false;
            try
            {
                //var tempFilePath = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "TempFileUpload")).Root;
                //string[] filePaths = Directory.GetFiles(tempFilePath, id + "_*");
                //if (filePaths.Count() == 0)
                //{
                //    isSomethingWentWrong = true;
                //}
                //else
                //{
                //    var appFilePath = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "AppFiles")).Root;
                //    string docId, fileExt;
                //    string[] alreadyFiles;
                //    string newFileName;
                //    foreach (var file in filePaths)
                //    {
                //        fileExt = file.Substring(file.LastIndexOf('.'));
                //        docId = file.Substring(file.LastIndexOf('_') + 1);
                //        docId = docId.Substring(0, docId.LastIndexOf('.'));
                //        alreadyFiles = Directory.GetFiles(appFilePath, id + "_" + docId + "*");
                //        newFileName = id + "_" + docId + "_" + (alreadyFiles.Count() + 1) + fileExt;
                //        Directory.Move(file, appFilePath + newFileName);

                //        isSomethingWentWrong = !await _iCommonApisService.SaveAppFileInfo(Convert.ToInt64(id), Convert.ToInt64(docId), newFileName);
                //    }
                //}

                var appDocIdsArray = JsonConvert.DeserializeObject<Int64[]>(appDocIds);
                //isSomethingWentWrong = !await _iCommonApisService.SaveAppFileInfo(Convert.ToInt64(id), Convert.ToInt64(docId), newFileName);
                isSomethingWentWrong = !await _iCommonApisService.LockAppDocFiles(id, appDocIdsArray);
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

        [HttpGet, Route("initiateappdocuments")]
        public async Task<IActionResult> Get_InitiateAppDocuments([FromQuery] Int64 id, bool deleteTempFiles)
        {

            List<AppFileUploadInfoViewModel> filesDetail = new List<AppFileUploadInfoViewModel>();
            bool isSomethingWentWrong = false;
            try
            {
                filesDetail = await _iCommonApisService.InitiateAppFileInfo(id, deleteTempFiles);

                //1. Delete existing temp files
                //var tempFilePath = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "TempFileUpload")).Root;
                //DirectoryInfo dir = new DirectoryInfo(tempFilePath);
                //foreach (FileInfo fi in dir.GetFiles(id + "_*"))
                //{
                //    fi.Delete();
                //}
                //2. Initiate already uploaded files
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
                isSomethingWentWrong = true;
            }
            if (isSomethingWentWrong)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, false);
            }
            return StatusCode(StatusCodes.Status200OK, filesDetail);
        }

        [HttpGet]
        [Route("download")]
        public async Task<FileStream> Download([FromQuery] string file)
        {
            var appFilePath = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "AppFiles")).Root;
            var filePath = Path.Combine(appFilePath, file);

            return new FileStream(filePath, FileMode.Open, FileAccess.Read);
        }

        [HttpGet]
        [Route("downloadlicense")]
        public async Task<FileStream> DownloadLicense([FromQuery] string fileName, ApplicationTypeEnum applicationType)
        {
            var appFilePath = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "License", "AppForm_" + applicationType.ToString())).Root;
            var filePath = Path.Combine(appFilePath, fileName+".pdf");

            return new FileStream(filePath, FileMode.Open, FileAccess.Read);
        }
        [HttpGet]
        [Route("downloadLegacyLicense")]
        public async Task<FileStream> DownloadLegacyLicense([FromQuery] string fileName)
        {
            var appFilePath = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "License", "LegacyLicences")).Root;
            var filePath = Path.Combine(appFilePath, fileName);

            return new FileStream(filePath, FileMode.Open, FileAccess.Read);
        }

        [HttpGet]
        [Route("getapplicationstepper")]
        public async Task<IActionResult> GetApplicationStepper(Int64 entityParentKeyId, Int64 appId, ApplicationTypeEnum applicationType)
        {
            GenericFormModel<dynamic> formModel = await _iCommonApisService.GetApplicationStepper(entityParentKeyId, appId, applicationType);
            if(formModel==null)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, null);
            }
            return StatusCode(StatusCodes.Status200OK, formModel);
        }

        #region Get District
        [HttpGet, Route("getalldistrict")]
        public async Task<IActionResult> GetAllDistrict()
        {
            GenericFormModel<List<DistrictViewModel>> genericFormModel = await _iCommonApisService.GetAllDistrict();
            if (genericFormModel.HasError)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, genericFormModel.ErrorDesc);
            }
            return StatusCode(StatusCodes.Status200OK, genericFormModel);
        }
        #endregion Get District

        [HttpGet, Route("getAllNICCodes")]
        public async Task<IActionResult> GetAllNICCodes()
        {
            GenericFormModel<List<NICCode>> genericFormModel = await _iCommonApisService.GetAllNICCodes();
            if (genericFormModel.HasError)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, genericFormModel.ErrorDesc);
            }
            return StatusCode(StatusCodes.Status200OK, genericFormModel);
        }

        #region Initiate AppDocument with RoleId
        [HttpGet, Route("initiateappdocumentwithroleid")]
        public async Task<IActionResult> Get_InitiateAppDocumentsWithRoleId([FromQuery] Int64 id, bool deleteTempFiles, string userId, int currentActionCode, int allowedActionCode, Int64 applicationaType, bool isTimeLineFlow, Int64 applicationActionLogId)
        {
            List<AppFileUploadInfoViewModel> filesDetail = new List<AppFileUploadInfoViewModel>();
            bool isSomethingWentWrong = false;
            try
            {
                filesDetail = await _iCommonApisService.InitiateAppFileInfoWithRoleId(id, deleteTempFiles, userId, currentActionCode, allowedActionCode, applicationaType, isTimeLineFlow, applicationActionLogId);
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
        #endregion

        [HttpPost]
        [Route("UploadDocument")]
        public async Task<IActionResult> UploadDocument()
        {
            GenericResponseTemplateModel<FileUploadResponse> genericResponse = new GenericResponseTemplateModel<FileUploadResponse>() { ErrorDesc="", HasError=false };
            try
            {
                var files = Request.Form.Files;
                if (files.Count() > 0)
                {
                    StringValues docIds, appIds, appDocIds;
                    genericResponse.HasError = Request.Headers.TryGetValue("docid", out docIds);
                    genericResponse.HasError = Request.Headers.TryGetValue("appid", out appIds);
                    genericResponse.HasError = Request.Headers.TryGetValue("appdocid", out appDocIds);
                    
                    if (docIds.Count() == 0 || appIds.Count() == 0)
                    {
                        genericResponse.HasError = true;
                    }
                    else
                    {
                        string DocId = docIds[0];
                        string AppId = appIds[0];
                        string AppDocId = appDocIds[0];

                        var filepath = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "AppFiles")).Root;
                        string fileName = "";
                       
                        foreach (var file in files)
                        {
                            //fileName = AppId + "_" + DocId + "_" + AppDocId + file.Name.Substring(file.FileName.LastIndexOf('.'));
                            //filepath = filepath + fileName;
                            genericResponse = await _iCommonApisService.LogFileInfo(Convert.ToInt64(AppId), Convert.ToInt64(DocId), Convert.ToInt64(AppDocId), file.Name.Substring(file.FileName.LastIndexOf('.')));
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

        #region Establishment Card Info
        [HttpGet, Route("getestablishmentcardinfo")]
        public async Task<IActionResult> GetEstablishmentCardInfo(Int64 appRefId)
        {
            GenericResponseTemplateModel<EstablishmentCardInfoViewModel> genericFormModel = await _iCommonApisService.GetEstablishmentCardDetails(appRefId);
            if (genericFormModel.HasError)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, genericFormModel.ErrorDesc);
            }
            return StatusCode(StatusCodes.Status200OK, genericFormModel);
        }
        #endregion

        #region Application Current Status
        [HttpGet, Route("getApplicationStatusByAppRefId")]
        public async Task<IActionResult> GetApplicationStatusByAppRefId(Int64 appRefId, ApplicationTypeEnum applicationType)
        {
            GenericResponseTemplateModel<ApplicationAction> genericFormModel = await _iApplicationMamnagementService.GetCurrentStatusByAppRefId(appRefId, applicationType);
            if (genericFormModel.HasError)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, genericFormModel.ErrorDesc);
            }
            return StatusCode(StatusCodes.Status200OK, genericFormModel);
        }
        #endregion

        #region Industry User Dashboard URL
        [HttpGet, Route("getIndustryUserDashboardURLByUserId")]
        public async Task<IActionResult> GetIndustryUserDashboardURLByUserId(string userId)
        {
            GenericResponseTemplateModel<string> genericFormModel = await _iApplicationMamnagementService.GetIndustryUserDashboardUrl(userId);
            if (genericFormModel.HasError)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, genericFormModel.ErrorDesc);
            }
            return StatusCode(StatusCodes.Status200OK, genericFormModel);
        }
        #endregion

        [HttpPost, Route("ValidateAndSeedCsvData")]
        public async Task<IActionResult> ValidateAndSeedCsvData([FromBody] UploadCsvRequestViewModel requestData)
        {
            GenericResponseTemplateModel<UploadCsvResponseViewModel> genericFormModel = await _iCommonApisService.ValidateAndSeedCsvData<ShopLicence_EmployeeDetail>(requestData.Data, requestData.EntityKey, requestData.EntityKeyValue);
            if (genericFormModel.HasError)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, genericFormModel.ErrorDesc);
            }
            return StatusCode(StatusCodes.Status200OK, genericFormModel);
        }

        #region Duplicate PAN, Aadhar & GST
        [HttpGet, Route("findDuplicateGST")]
        public async Task<IActionResult> FindDuplicateGST([FromQuery] string gstNumber, Int64 projectSiteRefId, int applicationType)
        {
            GenericResponseTemplateModel<IntReturn> genericResponseTemplateModel = await _iCommonApisService.FindDuplicateGST(gstNumber, projectSiteRefId, applicationType);
            if (genericResponseTemplateModel.HasError)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, genericResponseTemplateModel.ErrorDesc);
            }
            return StatusCode(StatusCodes.Status200OK, genericResponseTemplateModel);
        }

        [HttpGet, Route("findDuplicatePan")]
        public async Task<IActionResult> FindDuplicatePAN([FromQuery] string panOrTanNumber, Int64 projectSiteRefId, int applicationType)
        {
            GenericResponseTemplateModel<IntReturn> genericResponseTemplateModel = await _iCommonApisService.FindDuplicatePanNo(panOrTanNumber, projectSiteRefId, applicationType);
            if (genericResponseTemplateModel.HasError)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, genericResponseTemplateModel.ErrorDesc);
            }
            return StatusCode(StatusCodes.Status200OK, genericResponseTemplateModel);
        }
        #endregion

        #region Report Updated Date
        [HttpGet, Route("getReportUpdatedDate")]
        public async Task<IActionResult> GetReportUpdatedDate()
        {
            GenericResponseTemplateModel<DateTime> genericResponseTemplateModel = await _iCommonApisService.GetReportUpdatedDate();
            if (genericResponseTemplateModel.HasError)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, genericResponseTemplateModel.ErrorDesc);
            }
            return StatusCode(StatusCodes.Status200OK, genericResponseTemplateModel);
        }
        #endregion

        #region Activity Logs
        [HttpGet, Route("createActivityLogs")]
        public async Task<IActionResult> CreateActivityLogs([FromQuery] UserActivityLog requestData)
        {
            GenericServiceResultTemplate genericServiceResultTemplate = await _iCommonApisService.CreateActivityLogs(requestData);
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
        #endregion
        [HttpGet, Route("getInformationByLicenceNo")]
        public async Task<IActionResult> GetInformationByLicenceNo(string licenceNo)
        {
            GenericResponseTemplateModel<InformationViewModel> genericFormModel = await _iCommonApisService.GetInformationByLicenceNo(licenceNo);
            if (genericFormModel.HasError)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, genericFormModel.ErrorDesc);
            }
            return StatusCode(StatusCodes.Status200OK, genericFormModel);
        }

        [HttpGet, Route("getClientId")]
        public async Task<IActionResult> GetClientId()
        {
            bool tryUseXForwardHeader = true;
            string ip = null;
            if (tryUseXForwardHeader)
                ip = GetHeaderValueAs<string>("X-Forwarded-For").SplitCsv().FirstOrDefault();
            // RemoteIpAddress is always null in DNX RC1 Update1 (bug).
            if (ip.IsNullOrWhitespace() && _httpContextAccessor.HttpContext?.Connection?.RemoteIpAddress != null)
                ip = _httpContextAccessor.HttpContext.Connection.RemoteIpAddress.ToString();

            if (ip.IsNullOrWhitespace())
                ip = GetHeaderValueAs<string>("REMOTE_ADDR");

            // _httpContextAccessor.HttpContext?.Request?.Host this is the local host.

            if (ip.IsNullOrWhitespace())
                throw new Exception("Unable to determine caller's IP.");

            return StatusCode(StatusCodes.Status200OK,new {ip = ip });
        }

        public T GetHeaderValueAs<T>(string headerName)
        {
            StringValues values;

            if (_httpContextAccessor.HttpContext?.Request?.Headers?.TryGetValue(headerName, out values) ?? false)
            {
                string rawValues = values.ToString();   // writes out as Csv when there are multiple.

                if (!rawValues.IsNullOrWhitespace())
                    return (T)Convert.ChangeType(values.ToString(), typeof(T));
            }
            return default(T);
        }

        [HttpGet, Route("getWhatsNewInPortal")]
        public async Task<IActionResult> GetWhatsNewInPortal([FromQuery] string roleName)
        {
            GenericResponseTemplateModel<List<WhatsNewInPortal>> genericFormModel = await _iCommonApisService.GetWhatsNewInPortal(roleName);
            if (genericFormModel.HasError)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, genericFormModel.ErrorDesc);
            }
            return StatusCode(StatusCodes.Status200OK, genericFormModel);
        }

        [Route("addUpdateWhatsNewInPortalDetails")]
        [HttpPost]
        public async Task<IActionResult> AddUpdateWhatsNewInPortal([FromBody] WhatsNewInPortal requestData)
        {
            GenericServiceResultTemplate genericServiceResultTemplate = await _iCommonApisService.AddUpdateWhatsNewInPortal(requestData);
            if (genericServiceResultTemplate.HasException)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, genericServiceResultTemplate.Exceptions.Message);
            }
            return StatusCode(StatusCodes.Status200OK, genericServiceResultTemplate);
        }
        [HttpGet, Route("validateGst")]
        public async Task<IActionResult> ValidateGst([FromQuery] string gstNumber)
        {
            GenericResponseTemplateModel<ValidateGstResponseViewModel> genericFormModel = await _iCommonApisService.ValidateGst(gstNumber);
            if (genericFormModel.HasError)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, genericFormModel.ErrorDesc);
            }
            return StatusCode(StatusCodes.Status200OK, genericFormModel);
        }
    }
}



