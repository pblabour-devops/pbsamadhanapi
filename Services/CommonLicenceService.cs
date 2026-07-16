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
using System.Linq.Expressions;
using Microsoft.AspNetCore.Hosting;
using System.IO;
using System.IO.Compression;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using pbsamadhannetcoreapi.Middlewares;
using static pbsamadhannetcoreapi.Controllers.AuthController;
using System.Text;
using System.Drawing.Imaging;
using System.Drawing;
using Microsoft.Extensions.Configuration;


namespace pbsamadhannetcoreapi.Services.Implementations
{
    public class CommonLicenceService : ICommonLicenceService
    {
        private readonly IGenericRepository<CommonLicence_GeneralDetail> _iGR_CommonLicence_GeneralDetail;
        private readonly IGenericRepository<CommonLicence_ContractorDetail> _iGR_CommonLicence_ContractorDetail;
        private readonly IGenericRepository<DistrictLgd> _iGR_DistrictLgd;
        private readonly IZoneRepository _iZoneRepository;
        private readonly IApplicationManagementService<CommonLicence_GeneralDetail> _iApplicationMamnagementService;
        private readonly IGeneric_SP_Repository _iGeneric_SP_Repository;
        private readonly IGenericRepository<CommonLicences_SelectedLicenceMapping> _iGR_CommonLicences_SelectedLicenceMapping;
        private IAuthService _iAuthService;
        public IConfiguration Configuration { get; }
        public CommonLicenceService(IGenericRepository<CommonLicence_GeneralDetail> iGR_CommonLicence_GeneralDetail,
            IGenericRepository<CommonLicence_ContractorDetail> iGR_CommonLicence_ContractorDetail,
            IGenericRepository<DistrictLgd> iGR_DistrictLgd, IZoneRepository iZoneRepository,
            IApplicationManagementService<CommonLicence_GeneralDetail> IApplicationMamnagementService,
            AppDbContext context,
            IGeneric_SP_Repository iGeneric_SP_Repository,
            IGenericRepository<CommonLicences_SelectedLicenceMapping> iGR_CommonLicences_SelectedLicenceMapping,
            IAuthService authService,
            IConfiguration configuration
            )
        {
            _iGR_CommonLicence_GeneralDetail = iGR_CommonLicence_GeneralDetail;
            _iGR_CommonLicence_ContractorDetail = iGR_CommonLicence_ContractorDetail;
            _iGR_DistrictLgd = iGR_DistrictLgd;
            _iZoneRepository = iZoneRepository;
            _iApplicationMamnagementService = IApplicationMamnagementService;
            _iGeneric_SP_Repository = iGeneric_SP_Repository;
            _iGR_CommonLicences_SelectedLicenceMapping = iGR_CommonLicences_SelectedLicenceMapping;
            _iAuthService = authService;
            Configuration = configuration;
        }
        #region General Details
        public async Task<GenericFormModel<CommonLicence_GeneralDetail>> GetCommonLicenceGeneralDetail(Int64 id, Int64 projectSiteRefId)
        {
            GenericFormModel<CommonLicence_GeneralDetail> genericFormModel = new GenericFormModel<CommonLicence_GeneralDetail>();
            try
            {
                genericFormModel.ListTemplateLists = new List<ListTemplate>();

                //Initialization of list templates
                genericFormModel.ListTemplateLists.Add(new ListTemplate()
                {
                    ListTypeCode = "Districts",
                    ListItems = _iGR_DistrictLgd.GetAll().Select(x => new GenericListTemplate() { ID = x.DistrictLgdId, Text = x.DistrictName }).ToList()
                });
                if (id != 0) //Existing Record
                {
                    //Get Form data

                    var parentWithChildObject = await _iGR_CommonLicence_GeneralDetail
                       .GetAsync(x => x.CommonLicenceId == id,  //Conditions         
                         null,                //Orders          
                         x => x.Application, x=>x.Application.ApplicationAction)      //Includes
                       .ConfigureAwait(false);
                    genericFormModel.FormModel = parentWithChildObject.FirstOrDefault();

                    genericFormModel.IsEditAllowed = genericFormModel.FormModel.Application.IsAllowEdit;
                    genericFormModel.IsLocked = genericFormModel.FormModel.Application.IsLocked;
                    genericFormModel.ApplicationLifeCycleStatusType = genericFormModel.FormModel.Application.ApplicationLifeCycleStatusType;
                    
                    //if form model is available
                    if (genericFormModel.FormModel != null)
                    {
                            var tehsils = await _iZoneRepository.GetTehsilsByDistrictRefId(genericFormModel.FormModel.OccupierOrPE_DistrictRefId);
                            genericFormModel.ListTemplateLists.Add(new ListTemplate()
                            {
                                ListTypeCode = "Comm_Tehsils",
                                ListItems = tehsils.Select(x => new GenericListTemplate() { ID = x.ID, Text = x.Text}).ToList()
                            });
                    }
                }
                else //New Record
                {
                    genericFormModel.FormModel = new CommonLicence_GeneralDetail();
                    
                    //Allow user to edit form
                    genericFormModel.IsEditAllowed = true;
                    genericFormModel.IsLocked = false; 
                    genericFormModel.ApplicationLifeCycleStatusType = ApplicationLifeCycleStatusTypeEnum.IN_PROCESS;
                }
                //genericFormModel.FormModel.EstablishmentRefId = establishmentRefId;

                genericFormModel.AppFormStepsList = new List<AppFormStepsInfo>();
                genericFormModel.AppFormStepsList = await _iApplicationMamnagementService.GetAppFormStepperInfo(genericFormModel.FormModel.AppRefId, ApplicationTypeEnum.COMMON_LICENCE_OSH, (genericFormModel.FormModel != null ? genericFormModel.FormModel.CommonLicenceId : id), "GD");
            }
            catch (Exception ex)
            {
                genericFormModel.HasError = true;
                genericFormModel.ErrorDesc = ex.Message;
            }
            return genericFormModel;
         }
        public async Task<GenericServiceResultTemplate> AddUpdate_GeneralDetail(CommonLicence_GeneralDetail formModel, string userName)
        {
            GenericServiceResultTemplate genericServiceResultTemplate = new GenericServiceResultTemplate();
            try
            {
                genericServiceResultTemplate.HasException = false;
                genericServiceResultTemplate.CustomeValidationResult = CustomeValidator<CommonLicence_GeneralDetail>.ValidateModel_AllProperties(formModel);
                if (genericServiceResultTemplate.CustomeValidationResult.IsValid)
                {
                    if (formModel.CommonLicenceId != 0) //Existing record
                    {
                        //formModel.LastModifiedOnDate = DateTime.Now;
                        _iGR_CommonLicence_GeneralDetail.Update(formModel);
                        //Save changes
                        await _iGR_CommonLicence_GeneralDetail.SavechangeAsync();

                        //Update last modified date
                        await _iApplicationMamnagementService.UpdateAppLastModifiedDate(formModel.AppRefId);
                        genericServiceResultTemplate.ApplicationInitiateResponse = new ApplicationInitiateResponseViewModel();
                        genericServiceResultTemplate.ApplicationInitiateResponse.EntityKeyId = formModel.EstablishmentRefId;
                    }
                    else //New record
                    {
                        //Set null all navigation properties from model
                        formModel = GenericModelOps<CommonLicence_GeneralDetail>.SetNullAllNevigationProperties(formModel);
                        genericServiceResultTemplate.ApplicationInitiateResponse = await _iApplicationMamnagementService.InitiateApplication(ApplicationTypeEnum.COMMON_LICENCE_OSH, formModel, formModel.ProjectSiteRefId, formModel.ApplicationPurposeType, userName,0,formModel.InvestPunjab_AppId, false, 0,0, null, null, formModel.ProjectSiteVersion);
                        if (genericServiceResultTemplate.ApplicationInitiateResponse.IsApplicationCreated == false)
                        {
                            genericServiceResultTemplate.HasException = true;
                            genericServiceResultTemplate.Exceptions = new Exception();
                        }
                        
                        else
                        {
                            CommonLicences_SelectedLicenceMapping commonSelectedLicence = new CommonLicences_SelectedLicenceMapping()
                            {
                                IsFactory = formModel.IsFactory,
                                IsPrincipalEmployer = formModel.IsEngagementOfContractor,
                                IsBeediOrCigar = formModel.IsBeediAndCigar,
                                CommonLicenceRefId = genericServiceResultTemplate.ApplicationInitiateResponse.EntityKeyId
                            };
                            _iGR_CommonLicences_SelectedLicenceMapping.Insert(commonSelectedLicence);
                            await _iGR_CommonLicences_SelectedLicenceMapping.SavechangeAsync();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                genericServiceResultTemplate.HasException = true;
                genericServiceResultTemplate.Exceptions = ex;
            }
            return genericServiceResultTemplate;
        }
        #endregion General Details

        #region Contractor Details
        public async Task<GenericFormModel<CommonLicence_ContractorDetail>> GetCommonLicenceContractorDetail(long id)
        {
            GenericFormModel<CommonLicence_ContractorDetail> genericFormModel = new GenericFormModel<CommonLicence_ContractorDetail>();
            try
            {
                genericFormModel.ListTemplateLists = new List<ListTemplate>();

                //Initialization of list templates
                genericFormModel.ListTemplateLists.Add(new ListTemplate()
                {
                    ListTypeCode = "Districts",
                    ListItems = _iGR_DistrictLgd.GetAll().Select(x => new GenericListTemplate() { ID = x.DistrictLgdId, Text = x.DistrictName }).ToList()
                });
                // Check if this is an existing record
                if (id != 0) //Existing Record
                {
                    var parentWithChildObject = await _iGR_CommonLicence_GeneralDetail
                        .GetAsync(x => x.CommonLicenceId == id,  //Conditions         
                          null,                //Orders          
                          x => x.CommonLicence_ContractorDetail, x => x.Application)      //Includes
                        .ConfigureAwait(false);
                    genericFormModel.FormModel = parentWithChildObject.FirstOrDefault().CommonLicence_ContractorDetail;

                    genericFormModel.IsEditAllowed = parentWithChildObject.FirstOrDefault().Application.IsAllowEdit;
                    genericFormModel.IsLocked = parentWithChildObject.FirstOrDefault().Application.IsLocked;
                    genericFormModel.ApplicationLifeCycleStatusType = parentWithChildObject.FirstOrDefault().Application.ApplicationLifeCycleStatusType;
                }
                else //New Record
                {
                    genericFormModel.FormModel = new CommonLicence_ContractorDetail();

                    //Allow user to edit form
                    genericFormModel.IsEditAllowed = true;
                    genericFormModel.IsLocked = false;
                    genericFormModel.ApplicationLifeCycleStatusType = ApplicationLifeCycleStatusTypeEnum.IN_PROCESS;
                }
                genericFormModel.AppFormStepsList = await _iApplicationMamnagementService.GetAppFormStepperInfo(genericFormModel.FormModel.CommonLicence_GeneralDetail.AppRefId, ApplicationTypeEnum.COMMON_LICENCE_OSH, (genericFormModel.FormModel != null ? genericFormModel.FormModel.CommonLicenceRefId : id), "CD");
            }
            catch (Exception ex)
            {
                genericFormModel.HasError = true;
                genericFormModel.ErrorDesc = ex.Message;
            }
            return genericFormModel;
        }
        public async Task<GenericServiceResultTemplate> AddUpdate_ContractorDetail(CommonLicence_ContractorDetail formModel)
        {
            GenericServiceResultTemplate genericServiceResultTemplate = new GenericServiceResultTemplate();
            try
            {
                genericServiceResultTemplate.HasException = false;
                genericServiceResultTemplate.CustomeValidationResult = CustomeValidator<CommonLicence_ContractorDetail>.ValidateModel_AllProperties(formModel);
                genericServiceResultTemplate.ApplicationInitiateResponse = new ApplicationInitiateResponseViewModel();
                genericServiceResultTemplate.ApplicationInitiateResponse.EntityKeyId = formModel.CommonLicenceRefId;
                if (genericServiceResultTemplate.CustomeValidationResult.IsValid)
                {
                    if (formModel.CommonLicence_ContractorDetailId != 0) //Existing record
                    {
                        _iGR_CommonLicence_ContractorDetail.Update(formModel);
                        await _iGR_CommonLicence_ContractorDetail.SavechangeAsync();

                        //Update last modified date
                        await _iApplicationMamnagementService.UpdateAppLastModifiedDate(_iGR_CommonLicence_GeneralDetail.GetById(formModel.CommonLicenceRefId).AppRefId);
                    }
                    else //New record
                    {
                        //Set null all navigation properties from model
                        formModel = GenericModelOps<CommonLicence_ContractorDetail>.SetNullAllNevigationProperties(formModel);
                        _iGR_CommonLicence_ContractorDetail.Insert(formModel);
                        await _iGR_CommonLicence_ContractorDetail.SavechangeAsync();
                    }
                }
            }
            catch (Exception ex)
            {
                genericServiceResultTemplate.HasException = true;
                genericServiceResultTemplate.Exceptions = ex;
            }
            return genericServiceResultTemplate;
        }
        #endregion Contractor Details

        #region Common Licence Details
        public async Task<GenericFormModel<CommonLicenceViewModels>> GetCommonLicenceDetail(long id)
        {
            GenericFormModel<CommonLicenceViewModels> genericFormModel = new GenericFormModel<CommonLicenceViewModels>();
            try
            {
                genericFormModel.ListTemplateLists = new List<ListTemplate>();
                genericFormModel.FormModel = new CommonLicenceViewModels();
                genericFormModel.FormModel.GeneralDetail = new CommonLicence_GeneralDetail();
                genericFormModel.FormModel.ContractorDetail = new CommonLicence_ContractorDetail();

                var parentWithChildObject = await _iGR_CommonLicence_GeneralDetail
                        .GetAsync(x => x.CommonLicenceId == id,  //Conditions         
                          null,                //Orders          
                          x => x.CommonLicence_ContractorDetail, x => x.Application)      //Includes
                        .ConfigureAwait(false);

                genericFormModel.FormModel.GeneralDetail = parentWithChildObject.FirstOrDefault();
                genericFormModel.FormModel.ContractorDetail = parentWithChildObject.FirstOrDefault().CommonLicence_ContractorDetail;

                genericFormModel.FormModel.IsLocked = parentWithChildObject.FirstOrDefault().Application.IsLocked;
                genericFormModel.FormModel.IsFeeApplicable = parentWithChildObject.FirstOrDefault().Application.IsFeeApplicable;
                genericFormModel.ApplicationLifeCycleStatusType = parentWithChildObject.FirstOrDefault().Application.ApplicationLifeCycleStatusType;

                genericFormModel.AppFormStepsList = new List<AppFormStepsInfo>();
                genericFormModel.AppFormStepsList = await _iApplicationMamnagementService.GetAppFormStepperInfo(genericFormModel.FormModel.GeneralDetail.AppRefId, ApplicationTypeEnum.COMMON_LICENCE_OSH, (genericFormModel.FormModel != null ? genericFormModel.FormModel.GeneralDetail.CommonLicenceId : id), "LOCK");
            }
            catch (Exception ex)
            {
                genericFormModel.HasError = true;
                genericFormModel.ErrorDesc = ex.Message;
            }
            return genericFormModel;
        }
        #endregion Common Licence Details

        #region Lock Application
        public async Task<GenericServiceResultTemplate> LockApplication(Int64 appRefId, int AppActionType, string remarks)
        {
            GenericServiceResultTemplate genericServiceResultTemplate = new GenericServiceResultTemplate();
            try
            {
                genericServiceResultTemplate.HasException = false;
                genericServiceResultTemplate.CustomeValidationResult = new CustomeValidationResult();
                genericServiceResultTemplate.CustomeValidationResult.IsValid = true;
                genericServiceResultTemplate.ApplicationInitiateResponse = new ApplicationInitiateResponseViewModel();
                if (appRefId != 0)
                {

                    if (await _iApplicationMamnagementService.LockApplication(appRefId, AppActionType, remarks, ApplicationTypeEnum.COMMON_LICENCE_OSH) !="")
                    {
                        genericServiceResultTemplate.ApplicationInitiateResponse.IsApplicationCreated = true;
                    }
                    else
                    {
                        genericServiceResultTemplate.ApplicationInitiateResponse.IsApplicationCreated = false;
                        genericServiceResultTemplate.CustomeValidationResult.IsValid = false;
                    }
                }
                else
                {
                    genericServiceResultTemplate.CustomeValidationResult.IsValid = false;
                }
            }
            catch (Exception ex)
            {
                genericServiceResultTemplate.HasException = true;
                genericServiceResultTemplate.Exceptions = ex;
            }
            return genericServiceResultTemplate;
        }
        #endregion Lock Application


        public async Task<GenericResponseTemplateModel<CaptchaResultViewModel>> GetCaptchaImage()
        {
            GenericResponseTemplateModel<CaptchaResultViewModel> genericFormModel = new GenericResponseTemplateModel<CaptchaResultViewModel>();
            try
            {

                int width = 320;
                int height = 57;
                var captchaCd = GenerateCaptchaCode();
                var result = GenerateCaptchaImage(width, height, captchaCd);

                Stream s = new MemoryStream(result.CaptchaByteData);

                var img = JsonConvert.SerializeObject(s, Formatting.Indented, new MemoryStreamJsonConverter());
                string retObj =  RijndaelManagedCryptoHandler.EncryptFromPlainTextToBase64(captchaCd.ToString(), Configuration.GetSection("EncryptionConfigs").GetSection("MobileAppDataEncryptionKey").Value, Configuration.GetSection("EncryptionConfigs").GetSection("MobileAppDataIVKey").Value);

                CaptchaResultViewModel captchadetail = new CaptchaResultViewModel();
                captchadetail.CaptchaCode = retObj;
                captchadetail.CaptchaImg = img;
                captchadetail.CaptchaToken = await _iAuthService.GenerateCaptchaJwtToken(retObj);
                captchadetail.Timestamp = DateTime.Now;

                genericFormModel.ResponseDataModel = captchadetail;

                  
              
            }
            catch (Exception ex)
            {
                genericFormModel.HasError = true;
                genericFormModel.ErrorDesc = ex.Message;
            }
            return genericFormModel;
        }


        public static string GenerateCaptchaCode()
        {
            const string Letters = "2346789ABCDEFGHJKLMNPRTUVWXYZ";
            Random rand = new Random();
            int maxRand = Letters.Length - 1;

            StringBuilder sb = new StringBuilder();

            for (int i = 0; i < 6; i++)
            {
                int index = rand.Next(maxRand);
                sb.Append(Letters[index]);
            }

            return sb.ToString();
        }

        public static CaptchaCodeViewModel GenerateCaptchaImage(int width, int height, string captchaCode)
        {
            using (Bitmap baseMap = new Bitmap(width, height))
            using (Graphics graph = Graphics.FromImage(baseMap))
            {
                Random rand = new Random();
                graph.Clear(Color.FromArgb(255, 250, 250));

                DrawCaptchaCode();
                DrawDisorderLine();

                MemoryStream ms = new MemoryStream();

                baseMap.Save(ms, ImageFormat.Png);

                return new CaptchaCodeViewModel { CaptchaCode = captchaCode, CaptchaByteData = ms.ToArray(), Timestamp = DateTime.Now };

                int GetFontSize(int imageWidth, int captchCodeCount)
                {
                    var averageSize = imageWidth / captchCodeCount;

                    return Convert.ToInt32(averageSize);
                }

                Color GetRandomDeepColor()
                {
                    int redlow = 160, greenLow = 100, blueLow = 160;
                    return Color.FromArgb(rand.Next(redlow), rand.Next(greenLow), rand.Next(blueLow));
                }

                void DrawCaptchaCode()
                {
                    SolidBrush fontBrush = new SolidBrush(Color.Black);
                    int fontSize = GetFontSize(width, captchaCode.Length);
                    Font font = new Font(FontFamily.GenericSerif, fontSize, FontStyle.Bold, GraphicsUnit.Pixel);
                    for (int i = 0; i < captchaCode.Length; i++)
                    {
                        fontBrush.Color = GetRandomDeepColor();

                        int shiftPx = fontSize / 6;

                        float x = i * fontSize + rand.Next(-shiftPx, shiftPx) + rand.Next(-shiftPx, shiftPx);
                        int maxY = height - fontSize;
                        if (maxY < 0) maxY = 0;
                        float y = rand.Next(0, maxY);

                        graph.DrawString(captchaCode[i].ToString(), font, fontBrush, x, y);
                    }
                }

                void DrawDisorderLine()
                {
                    Pen linePen = new Pen(new SolidBrush(Color.Black), 1);
                    for (int i = 0; i < rand.Next(3, 5); i++)
                    {
                        linePen.Color = GetRandomDeepColor();

                        Point startPoint = new Point(rand.Next(0, width), rand.Next(0, height));
                        Point endPoint = new Point(rand.Next(0, width), rand.Next(0, height));
                        graph.DrawLine(linePen, startPoint, endPoint);

                    }
                }

            }
        }


    }
}
