using Newtonsoft.Json;
using pbsamadhannetcoreapi.CommonUtiliteis;
using pbsamadhannetcoreapi.Models;
using pbsamadhannetcoreapi.Services.Implementations;
using pbsamadhannetcoreapi.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace pbsamadhannetcoreapi.Services
{
    public class CrudService : ICrudService
    {
        private readonly IToDoManagerService _iToDoManagerService;
        public CrudService(IToDoManagerService iToDoManagerService)
        {
            _iToDoManagerService = iToDoManagerService;
        }
        public async Task<CRUD_CreateUpdateOperationResponse> CreateUpdate(object requestData, User user)
        {
            CRUD_CreateUpdateOperationResponse resp = new CRUD_CreateUpdateOperationResponse();

            // STEP: 1 - Pre-Requisites

            bool is_preRequisites_set_1_meets = true;
            PropertyFinderRespViewModel propertyFinderResp = ReflectionOps.GeValueByPropName(requestData, "toDoActivityCategoryType");
            ToDoActivityCategoryTypeEnum toDoActivityCategoryType = ToDoActivityCategoryTypeEnum.DEFAULT;
            ApplicationTypeEnum applicationType = ApplicationTypeEnum.DEFAULT;
            ApplicationPurposeTypeEnum applicationPurposeType = ApplicationPurposeTypeEnum.DEFAULT;
            Int64 projectSiteRefId = 0;
            Int64 investPunjab_AppId = 0;
            Int64 iPin = 0;
            Int64 appRefId = 0;
            Int64 entityKeyId = 0;
            int projectSiteVersion = 0;
            if (propertyFinderResp.HasPropName)
            {
                Enum.TryParse(propertyFinderResp.PropValue, out toDoActivityCategoryType);
            }
            else
            {
                is_preRequisites_set_1_meets = false;
            }

            if (toDoActivityCategoryType != ToDoActivityCategoryTypeEnum.APPLICATION_FORM_LOCK)
            {
                propertyFinderResp = ReflectionOps.GeValueByPropName(requestData, "ApplicationType");
                if (propertyFinderResp.HasPropName)
                {
                    Enum.TryParse(propertyFinderResp.PropValue, out applicationType);
                }
                else
                {
                    is_preRequisites_set_1_meets = false;
                }
            }

            if (toDoActivityCategoryType != ToDoActivityCategoryTypeEnum.APPLICATION_FORM_LOCK)
            {
                propertyFinderResp = ReflectionOps.GeValueByPropName(requestData, "ApplicationPurposeType");
                if (propertyFinderResp.HasPropName)
                {
                    Enum.TryParse(propertyFinderResp.PropValue, out applicationPurposeType);
                }
                else
                {
                    is_preRequisites_set_1_meets = false;
                }
            }
            propertyFinderResp = ReflectionOps.GeValueByPropName(requestData, "ToDoActivityModeType");
            ToDoActivityModeTypeEnum toDoActivityModeType = ToDoActivityModeTypeEnum.DEFAULT;
            if (propertyFinderResp.HasPropName)
            {
                Enum.TryParse(propertyFinderResp.PropValue, out toDoActivityModeType);
            }
            else
            {
                is_preRequisites_set_1_meets = false;
            }



            propertyFinderResp = ReflectionOps.GeValueByPropName(requestData, "AppRefId");

            if (propertyFinderResp.HasPropName)
            {
                appRefId = Convert.ToInt64(propertyFinderResp.PropValue);
            }
            else
            {
                is_preRequisites_set_1_meets = false;
            }

            if (toDoActivityCategoryType != ToDoActivityCategoryTypeEnum.APPLICATION_FORM_LOCK)
            {
                propertyFinderResp = ReflectionOps.GeValueByPropName(requestData, "InvestPunjab_AppId");

                if (propertyFinderResp.HasPropName)
                {
                    investPunjab_AppId = Convert.ToInt64(propertyFinderResp.PropValue);
                }
                else
                {
                    is_preRequisites_set_1_meets = false;
                }
            }

            if (toDoActivityCategoryType != ToDoActivityCategoryTypeEnum.APPLICATION_FORM_LOCK)
            {
                propertyFinderResp = ReflectionOps.GeValueByPropName(requestData, "IPin");

                if (propertyFinderResp.HasPropName)
                {
                    iPin = Convert.ToInt64(propertyFinderResp.PropValue);
                }
                else
                {
                    is_preRequisites_set_1_meets = false;
                }
            }

            if (toDoActivityCategoryType != ToDoActivityCategoryTypeEnum.APPLICATION_FORM_LOCK)
            {
                propertyFinderResp = ReflectionOps.GeValueByPropName(requestData, "ProjectSiteRefId");

                if (propertyFinderResp.HasPropName)
                {
                    projectSiteRefId = Convert.ToInt64(propertyFinderResp.PropValue);
                }
                else
                {
                    is_preRequisites_set_1_meets = false;
                }
            }

            if (toDoActivityCategoryType != ToDoActivityCategoryTypeEnum.APPLICATION_FORM_LOCK)
            {
                propertyFinderResp = ReflectionOps.GeValueByPropName(requestData, "ProjectSiteVersion");

                if (propertyFinderResp.HasPropName)
                {
                    projectSiteVersion = Convert.ToInt32(propertyFinderResp.PropValue);
                }
                else
                {
                    is_preRequisites_set_1_meets = false;
                }
            }

            propertyFinderResp = ReflectionOps.GeValueByPropName(requestData, "RootActivityRefId");
            string rootActivityRefId = "";
            if (propertyFinderResp.HasPropName)
            {
                rootActivityRefId = propertyFinderResp.PropValue.ToString();
                if (rootActivityRefId == "")
                {
                    rootActivityRefId = DateTimeOffset.Now.ToUnixTimeSeconds().ToString();
                }
            }
            else
            {
                rootActivityRefId = DateTimeOffset.Now.ToUnixTimeSeconds().ToString();
                //is_preRequisites_set_1_meets = false;
            }

            ApplicationTypeEnum applicationTypeClone = applicationType;
            ApplicationPurposeTypeEnum applicationPurposeTypeClone = applicationPurposeType;

            if (appRefId > 0)
            {
                var application = await _iToDoManagerService.GetApplication(appRefId);
                applicationType = (toDoActivityCategoryType == ToDoActivityCategoryTypeEnum.APPLICATION_FORM_LOCK) ? 0 : application.ResponseDataModel.ApplicationType;
                applicationPurposeType = (toDoActivityCategoryType == ToDoActivityCategoryTypeEnum.APPLICATION_FORM_LOCK) ? 0 : application.ResponseDataModel.ApplicationPurposeType;
                investPunjab_AppId = application.ResponseDataModel.InvestPunjab_AppId;
                iPin = Convert.ToInt64(application.ResponseDataModel.InvestPunjab_Ipin);
                projectSiteRefId = application.ResponseDataModel.ProjectSiteRefId;
                projectSiteVersion = application.ResponseDataModel.ProjectSiteVersion;

                applicationTypeClone = application.ResponseDataModel.ApplicationType;
                applicationPurposeTypeClone = application.ResponseDataModel.ApplicationPurposeType;
            }

            if (is_preRequisites_set_1_meets)
            {
                List<ToDoApplicationActivityMaping> toDoActivityList = await _iToDoManagerService.GetToDoApplicationActivityMapingList(applicationType, applicationPurposeType, toDoActivityModeType, toDoActivityCategoryType);
                bool isAnyStepHasException = false;
                foreach (var toDoActivity in toDoActivityList)
                {
                    string failedMessage = "";
                    ToDoActivityCompleteTypeEnum toDoActivityCompleteType = ToDoActivityCompleteTypeEnum.SUCCEEDED;
                    switch (toDoActivity.ToDoCodeType)
                    {
                        case ToDoCodeTypeEnum.INITIATE_APPLICATION:
                            try
                            {
                                propertyFinderResp = ReflectionOps.GeValueByPropName(requestData, "OldAppRefId");
                                Int64 OldAppRefId = 0;
                                if (propertyFinderResp.HasPropName)
                                {
                                    OldAppRefId = Convert.ToInt64(propertyFinderResp.PropValue.ToString());
                                }


                                propertyFinderResp = ReflectionOps.GeValueByPropName(requestData, "oldLicenceNo");
                                var oldLicenceNo = "";
                                if (propertyFinderResp.HasPropName)
                                {
                                    oldLicenceNo = propertyFinderResp.PropValue.ToString();
                                }

                                appRefId = await _iToDoManagerService.InitiateApplication(new InitiateApplicationParmsViewModel()
                                {
                                    ApplicationPurposeType = applicationPurposeType,
                                    ApplicationType = applicationType,
                                    InvestPunjab_AppId = investPunjab_AppId,
                                    IPin = iPin,
                                    Legacy_AppFormId = 0,
                                    Legacy_AppId = OldAppRefId,
                                    Legacy_IsMigrated = false,
                                    Legacy_LicenceNo = oldLicenceNo,
                                    Legacy_NAR = null,
                                    ProjectSiteRefId = projectSiteRefId,
                                    ProjectSiteVersion = projectSiteVersion,
                                    RootActivityRefId = rootActivityRefId,
                                }, rootActivityRefId, toDoActivity.ToDoCodeType);

                                failedMessage = "";
                                toDoActivityCompleteType = ToDoActivityCompleteTypeEnum.SUCCEEDED;
                            }
                            catch(Exception ex)
                            {
                                failedMessage = "Activity INITIATE_APPLICATION: " + JsonConvert.SerializeObject(ex);
                                toDoActivityCompleteType = ToDoActivityCompleteTypeEnum.FAILED_WITH_ERRORS;
                                isAnyStepHasException = true;
                                throw ex;
                            }
                            finally
                            {
                                await _iToDoManagerService.CreateActivityLog(new ToDoActivityLog()
                                {
                                    ActivityFailedMessage = failedMessage,
                                    ApplicationPurposeType = applicationPurposeTypeClone,
                                    ApplicationType = applicationTypeClone,
                                    AppRefId = appRefId,
                                    RootActivityRefId = rootActivityRefId,
                                    TimeStemp = DateTime.Now,
                                    ToDoActivityCompleteType = toDoActivityCompleteType,
                                    ToDoActivityMapingVersion = toDoActivity.Version,
                                    ToDoActivityModeType = toDoActivityModeType,
                                    ToDoCodeType = toDoActivity.ToDoCodeType,
                                    UserRefId = user.Id,
                                    InvestPunjab_AppId = investPunjab_AppId,
                                    InvestPunjab_Ipin = iPin,
                                    ToDoActivityCategoryType = toDoActivityCategoryType
                                });
                            }
                            break;

                        case ToDoCodeTypeEnum.MASTER_ENTRY_NEW:

                            try
                            {

                                if (appRefId == 0)
                                {
                                    failedMessage = "Activity MASTER_ENTRY: AppRefId is not available";
                                    toDoActivityCompleteType = ToDoActivityCompleteTypeEnum.FAILED_MISSING_PARAMETERS;
                                    isAnyStepHasException = true;
                                }
                                else
                                {
                                    PropertyInfo propertyInfo = requestData.GetType().GetProperty("AppRefId");
                                    propertyInfo.SetValue(requestData, appRefId);
                                    entityKeyId = await _iToDoManagerService.Master_Seed(requestData, rootActivityRefId, toDoActivity.ToDoCodeType);

                                    failedMessage = "";
                                    toDoActivityCompleteType = ToDoActivityCompleteTypeEnum.SUCCEEDED;
                                }
                                
                            }
                            catch (Exception ex)
                            {
                                failedMessage = "Activity MASTER_ENTRY: " + JsonConvert.SerializeObject(ex);
                                toDoActivityCompleteType = ToDoActivityCompleteTypeEnum.FAILED_WITH_ERRORS;
                                isAnyStepHasException = true;
                                throw ex;
                            }
                            finally
                            {
                                await _iToDoManagerService.CreateActivityLog(new ToDoActivityLog()
                                {
                                    ActivityFailedMessage = failedMessage,
                                    ApplicationPurposeType = applicationPurposeTypeClone,
                                    ApplicationType = applicationTypeClone,
                                    AppRefId = appRefId,
                                    RootActivityRefId = rootActivityRefId,
                                    TimeStemp = DateTime.Now,
                                    ToDoActivityCompleteType = toDoActivityCompleteType,
                                    ToDoActivityMapingVersion = toDoActivity.Version,
                                    ToDoActivityModeType = toDoActivityModeType,
                                    ToDoCodeType = toDoActivity.ToDoCodeType,
                                    UserRefId = user.Id,
                                    InvestPunjab_AppId = investPunjab_AppId,
                                    InvestPunjab_Ipin = iPin,
                                    ToDoActivityCategoryType = toDoActivityCategoryType
                                });
                            }
                            break;

                        case ToDoCodeTypeEnum.INITIATE_APPLICATION_ACTION:
                            try
                            {
                                if (appRefId == 0)
                                {
                                    failedMessage = "Activity INITIATE_APPLICATION_ACTION: AppRefId is not available";
                                    toDoActivityCompleteType = ToDoActivityCompleteTypeEnum.FAILED_MISSING_PARAMETERS;
                                    isAnyStepHasException = true;
                                }
                                else
                                {
                                    await _iToDoManagerService.SeedInitiateAppAction(appRefId, user, rootActivityRefId, toDoActivity.ToDoCodeType, applicationType);

                                    failedMessage = "";
                                    toDoActivityCompleteType = ToDoActivityCompleteTypeEnum.SUCCEEDED;
                                }

                            }
                            catch (Exception ex)
                            {
                                failedMessage = "Activity INITIATE_APPLICATION_ACTION: " + JsonConvert.SerializeObject(ex);
                                toDoActivityCompleteType = ToDoActivityCompleteTypeEnum.FAILED_WITH_ERRORS;
                                isAnyStepHasException = true;
                                throw ex;
                            }
                            finally
                            {
                                await _iToDoManagerService.CreateActivityLog(new ToDoActivityLog()
                                {
                                    ActivityFailedMessage = failedMessage,
                                    ApplicationPurposeType = applicationPurposeTypeClone,
                                    ApplicationType = applicationTypeClone,
                                    AppRefId = appRefId,
                                    RootActivityRefId = rootActivityRefId,
                                    TimeStemp = DateTime.Now,
                                    ToDoActivityCompleteType = toDoActivityCompleteType,
                                    ToDoActivityMapingVersion = toDoActivity.Version,
                                    ToDoActivityModeType = toDoActivityModeType,
                                    ToDoCodeType = toDoActivity.ToDoCodeType,
                                    UserRefId = user.Id,
                                    InvestPunjab_AppId = investPunjab_AppId,
                                    InvestPunjab_Ipin = iPin,
                                    ToDoActivityCategoryType = toDoActivityCategoryType
                                });
                            }
                            break;
                            
                        case ToDoCodeTypeEnum.SHARE_STATUS_WITH_INVEST_PUNJAB:
                            try
                            {

                                if (appRefId == 0)
                                {
                                    failedMessage = "Activity SHARE_STATUS_WITH_INVEST_PUNJAB: AppRefId is not available";
                                    toDoActivityCompleteType = ToDoActivityCompleteTypeEnum.FAILED_MISSING_PARAMETERS;
                                    isAnyStepHasException = true;
                                }
                                else
                                {
                                    await _iToDoManagerService.ShareStatusWithInvestPunjab(appRefId, rootActivityRefId, toDoActivity.ToDoCodeType, toDoActivityModeType);
                                    failedMessage = "";
                                    toDoActivityCompleteType = ToDoActivityCompleteTypeEnum.SUCCEEDED;
                                }

                            }
                            catch (Exception ex)
                            {
                                failedMessage = "Activity SHARE_STATUS_WITH_INVEST_PUNJAB: " + JsonConvert.SerializeObject(ex);
                                toDoActivityCompleteType = ToDoActivityCompleteTypeEnum.FAILED_WITH_ERRORS;
                                isAnyStepHasException = true;
                                throw ex;
                            }
                            finally
                            {
                                await _iToDoManagerService.CreateActivityLog(new ToDoActivityLog()
                                {
                                    ActivityFailedMessage = failedMessage,
                                    ApplicationPurposeType = applicationPurposeTypeClone,
                                    ApplicationType = applicationTypeClone,
                                    AppRefId = appRefId,
                                    RootActivityRefId = rootActivityRefId,
                                    TimeStemp = DateTime.Now,
                                    ToDoActivityCompleteType = toDoActivityCompleteType,
                                    ToDoActivityMapingVersion = toDoActivity.Version,
                                    ToDoActivityModeType = toDoActivityModeType,
                                    ToDoCodeType = toDoActivity.ToDoCodeType,
                                    UserRefId = user.Id,
                                    InvestPunjab_AppId = investPunjab_AppId,
                                    InvestPunjab_Ipin = iPin,
                                    ToDoActivityCategoryType = toDoActivityCategoryType
                                });
                            }
                            break;

                        case ToDoCodeTypeEnum.UPDATE_CIRCLEID_IN_PROJECT_SITE:
                            try
                            {

                                if (projectSiteRefId == 0 || projectSiteVersion==0)
                                {
                                    failedMessage = "Activity UPDATE_CIRCLEID_IN_PROJECT_SITE: projectSiteRefId="+ projectSiteRefId.ToString() + ", projectSiteVersion="+ projectSiteVersion.ToString() + "  is not available";
                                    toDoActivityCompleteType = ToDoActivityCompleteTypeEnum.FAILED_MISSING_PARAMETERS;
                                    isAnyStepHasException = true;
                                }
                                else
                                {
                                    Int64 circleRefId = 0;
                                    Int64 alCircleId = 0;
                                    CircleTypeEnum circleType = CircleTypeEnum.LABOUR_CIRCLE;
                                    if(applicationType == ApplicationTypeEnum.SHOP_LICENCE || applicationType == ApplicationTypeEnum.TRADE_UNION)
                                    {
                                        circleType = CircleTypeEnum.LABOUR_CIRCLE;
                                        propertyFinderResp = ReflectionOps.GeValueByPropName(requestData, "LabourCircleRefId");
                                        if (propertyFinderResp.HasPropName)
                                        {
                                            circleRefId = Convert.ToInt64(propertyFinderResp.PropValue.ToString());
                                        }
                                        else
                                        {
                                            circleRefId = 0;
                                        }
                                    }
                                    else if (applicationType == ApplicationTypeEnum.FACTORY_LICENCE 
                                          || applicationType == ApplicationTypeEnum.BUILDING_PLAN_PROPOSED
                                          || applicationType == ApplicationTypeEnum.BUILDING_PLAN_EXISTING
                                          || applicationType == ApplicationTypeEnum.BUILDING_PLAN_ADDITION_AMENDMENT
                                          || applicationType == ApplicationTypeEnum.BUILDING_PLAN_PSIEC)
                                    {
                                        circleType = CircleTypeEnum.FACTORY_CIRCLE;
                                        propertyFinderResp = ReflectionOps.GeValueByPropName(requestData, "FactoryCircleRefId");
                                        if (propertyFinderResp.HasPropName)
                                        {
                                            circleRefId = Convert.ToInt64(propertyFinderResp.PropValue.ToString());
                                        }
                                        else
                                        {
                                            circleRefId = 0;
                                        }
                                    }
                                    else if (applicationType == ApplicationTypeEnum.PRINCIPAL_EMPLOYER 
                                        || applicationType == ApplicationTypeEnum.CONTRACT_LABOUR
                                        || applicationType == ApplicationTypeEnum.ISM_CONTRACT_LABOUR
                                        || applicationType == ApplicationTypeEnum.ISM_PRINCIPAL_EMPLOYER)
                                    {
                                        circleType = CircleTypeEnum.ALC_CIRCLE;
                                        propertyFinderResp = ReflectionOps.GeValueByPropName(requestData, "AlcCircleRefId");
                                        if (propertyFinderResp.HasPropName)
                                        {
                                            circleRefId = Convert.ToInt64(propertyFinderResp.PropValue.ToString());
                                        }
                                        else
                                        {
                                            circleRefId = 0;
                                        }
                                    }

                                    if (circleRefId == 0)
                                    {
                                        failedMessage = "Activity UPDATE_CIRCLEID_IN_PROJECT_SITE: CircleRefId  is not available";
                                        toDoActivityCompleteType = ToDoActivityCompleteTypeEnum.FAILED_MISSING_PARAMETERS;
                                        isAnyStepHasException = true;
                                    }
                                    else
                                    {

                                        await _iToDoManagerService.UpdateCircleRefIdInProjectSite(projectSiteRefId, projectSiteVersion, circleRefId, circleType, rootActivityRefId, toDoActivity.ToDoCodeType);

                                        if (applicationType == ApplicationTypeEnum.TRADE_UNION)
                                        {
                                            alCircleId = await _iToDoManagerService.GET_ALCCircleId_By_LabourCircleId(circleRefId);
                                            await _iToDoManagerService.UpdateCircleRefIdInProjectSite(projectSiteRefId, projectSiteVersion, alCircleId, CircleTypeEnum.ALC_CIRCLE, rootActivityRefId, toDoActivity.ToDoCodeType);
                                        }

                                        failedMessage = "";
                                        toDoActivityCompleteType = ToDoActivityCompleteTypeEnum.SUCCEEDED;
                                    }
                                   
                                }

                            }
                            catch (Exception ex)
                            {
                                failedMessage = "Activity UPDATE_CIRCLEID_IN_PROJECT_SITE: " + JsonConvert.SerializeObject(ex);
                                toDoActivityCompleteType = ToDoActivityCompleteTypeEnum.FAILED_WITH_ERRORS;
                                isAnyStepHasException = true;
                                throw ex;
                            }
                            finally
                            {
                                await _iToDoManagerService.CreateActivityLog(new ToDoActivityLog()
                                {
                                    ActivityFailedMessage = failedMessage,
                                    ApplicationPurposeType = applicationPurposeTypeClone,
                                    ApplicationType = applicationTypeClone,
                                    AppRefId = appRefId,
                                    RootActivityRefId = rootActivityRefId,
                                    TimeStemp = DateTime.Now,
                                    ToDoActivityCompleteType = toDoActivityCompleteType,
                                    ToDoActivityMapingVersion = toDoActivity.Version,
                                    ToDoActivityModeType = toDoActivityModeType,
                                    ToDoCodeType = toDoActivity.ToDoCodeType,
                                    UserRefId = user.Id,
                                    InvestPunjab_AppId = investPunjab_AppId,
                                    InvestPunjab_Ipin = iPin,
                                    ToDoActivityCategoryType = toDoActivityCategoryType
                                });
                            }
                            break;

                        case ToDoCodeTypeEnum.UPDATE_NATIVE_APPID_IN_BF_REQLOG:
                            try
                            {

                                if (appRefId == 0 || iPin==0 || investPunjab_AppId==0)
                                {
                                    failedMessage = "Activity UPDATE_NATIVE_APPID_IN_BF_REQLOG: appRefId="+appRefId.ToString()+" , iPin="+ iPin.ToString()+ ", investPunjab_AppId=" + investPunjab_AppId.ToString() + " is not available";
                                    toDoActivityCompleteType = ToDoActivityCompleteTypeEnum.FAILED_MISSING_PARAMETERS;
                                    isAnyStepHasException = true;
                                }
                                else
                                {

                                    await _iToDoManagerService.UpdateBusinessFirstRequestNativeAppIdByIPin(iPin, investPunjab_AppId, appRefId, applicationType, applicationPurposeType, rootActivityRefId, toDoActivity.ToDoCodeType);

                                    failedMessage = "";
                                    toDoActivityCompleteType = ToDoActivityCompleteTypeEnum.SUCCEEDED;

                                }

                            }
                            catch (Exception ex)
                            {
                                failedMessage = "Activity UPDATE_NATIVE_APPID_IN_BF_REQLOG: " + JsonConvert.SerializeObject(ex);
                                toDoActivityCompleteType = ToDoActivityCompleteTypeEnum.FAILED_WITH_ERRORS;
                                isAnyStepHasException = true;
                                throw ex;
                            }
                            finally
                            {
                                await _iToDoManagerService.CreateActivityLog(new ToDoActivityLog()
                                {
                                    ActivityFailedMessage = failedMessage,
                                    ApplicationPurposeType = applicationPurposeTypeClone,
                                    ApplicationType = applicationTypeClone,
                                    AppRefId = appRefId,
                                    RootActivityRefId = rootActivityRefId,
                                    TimeStemp = DateTime.Now,
                                    ToDoActivityCompleteType = toDoActivityCompleteType,
                                    ToDoActivityMapingVersion = toDoActivity.Version,
                                    ToDoActivityModeType = toDoActivityModeType,
                                    ToDoCodeType = toDoActivity.ToDoCodeType,
                                    UserRefId = user.Id,
                                    InvestPunjab_AppId = investPunjab_AppId,
                                    InvestPunjab_Ipin = iPin,
                                    ToDoActivityCategoryType = toDoActivityCategoryType
                                });
                            }
                            break;

                        case ToDoCodeTypeEnum.MASTER_ENTRY_UPDATE:
                            try
                            {
                                if (appRefId == 0)
                                {
                                    failedMessage = "Activity MASTER_ENTRY_UPDATE: appRefId=" + appRefId.ToString() + " is not available";
                                    toDoActivityCompleteType = ToDoActivityCompleteTypeEnum.FAILED_MISSING_PARAMETERS;
                                    isAnyStepHasException = true;
                                }
                                else
                                {
                                    entityKeyId = await _iToDoManagerService.Master_Update(requestData, rootActivityRefId, toDoActivity.ToDoCodeType);
                                    failedMessage = "";
                                    toDoActivityCompleteType = ToDoActivityCompleteTypeEnum.SUCCEEDED;
                                }
                            }
                            catch (Exception ex)
                            {
                                failedMessage = "Activity MASTER_ENTRY_UPDATE: " + JsonConvert.SerializeObject(ex);
                                toDoActivityCompleteType = ToDoActivityCompleteTypeEnum.FAILED_WITH_ERRORS;
                                isAnyStepHasException = true;
                                throw ex;
                            }
                            finally
                            {
                                await _iToDoManagerService.CreateActivityLog(new ToDoActivityLog()
                                {
                                    ActivityFailedMessage = failedMessage,
                                    ApplicationPurposeType = applicationPurposeTypeClone,
                                    ApplicationType = applicationTypeClone,
                                    AppRefId = appRefId,
                                    RootActivityRefId = rootActivityRefId,
                                    TimeStemp = DateTime.Now,
                                    ToDoActivityCompleteType = toDoActivityCompleteType,
                                    ToDoActivityMapingVersion = toDoActivity.Version,
                                    ToDoActivityModeType = toDoActivityModeType,
                                    ToDoCodeType = toDoActivity.ToDoCodeType,
                                    UserRefId = user.Id,
                                    InvestPunjab_AppId = investPunjab_AppId,
                                    InvestPunjab_Ipin = iPin,
                                    ToDoActivityCategoryType = toDoActivityCategoryType
                                });
                            }
                            break;

                        case ToDoCodeTypeEnum.UPDATE_LAST_MODIFIED_DATE_IN_APPLICATION:
                            try
                            {
                                if (appRefId == 0)
                                {
                                    failedMessage = "Activity UPDATE_LAST_MODIFIED_DATE_IN_APPLICATION: appRefId=" + appRefId.ToString() + " is not available";
                                    toDoActivityCompleteType = ToDoActivityCompleteTypeEnum.FAILED_MISSING_PARAMETERS;
                                    isAnyStepHasException = true;
                                }
                                else
                                {
                                    await _iToDoManagerService.UpdateLastModifiedDateInApplication(appRefId, rootActivityRefId, toDoActivity.ToDoCodeType);
                                    failedMessage = "";
                                    toDoActivityCompleteType = ToDoActivityCompleteTypeEnum.SUCCEEDED;
                                }
                            }
                            catch (Exception ex)
                            {
                                failedMessage = "Activity UPDATE_LAST_MODIFIED_DATE_IN_APPLICATION: " + JsonConvert.SerializeObject(ex);
                                toDoActivityCompleteType = ToDoActivityCompleteTypeEnum.FAILED_WITH_ERRORS;
                                isAnyStepHasException = true;
                                throw ex;
                            }
                            finally
                            {
                                await _iToDoManagerService.CreateActivityLog(new ToDoActivityLog()
                                {
                                    ActivityFailedMessage = failedMessage,
                                    ApplicationPurposeType = applicationPurposeTypeClone,
                                    ApplicationType = applicationTypeClone,
                                    AppRefId = appRefId,
                                    RootActivityRefId = rootActivityRefId,
                                    TimeStemp = DateTime.Now,
                                    ToDoActivityCompleteType = toDoActivityCompleteType,
                                    ToDoActivityMapingVersion = toDoActivity.Version,
                                    ToDoActivityModeType = toDoActivityModeType,
                                    ToDoCodeType = toDoActivity.ToDoCodeType,
                                    UserRefId = user.Id,
                                    InvestPunjab_AppId = investPunjab_AppId,
                                    InvestPunjab_Ipin = iPin,
                                    ToDoActivityCategoryType = toDoActivityCategoryType
                                });
                            }
                            break;

                        case ToDoCodeTypeEnum.SET_ISDIRTY_FLAG:
                            try
                            {
                                if (appRefId == 0)
                                {
                                    failedMessage = "Activity SET_ISDIRTY_FLAG: appRefId=" + appRefId.ToString() + " is not available";
                                    toDoActivityCompleteType = ToDoActivityCompleteTypeEnum.FAILED_MISSING_PARAMETERS;
                                    isAnyStepHasException = true;
                                }
                                else
                                {
                                    await _iToDoManagerService.SetIsDirtyFlag(appRefId, true, rootActivityRefId, toDoActivity.ToDoCodeType);
                                    failedMessage = "";
                                    toDoActivityCompleteType = ToDoActivityCompleteTypeEnum.SUCCEEDED;
                                }
                            }
                            catch (Exception ex)
                            {
                                failedMessage = "Activity SET_ISDIRTY_FLAG: " + JsonConvert.SerializeObject(ex);
                                toDoActivityCompleteType = ToDoActivityCompleteTypeEnum.FAILED_WITH_ERRORS;
                                isAnyStepHasException = true;
                                throw ex;
                            }
                            finally
                            {
                                await _iToDoManagerService.CreateActivityLog(new ToDoActivityLog()
                                {
                                    ActivityFailedMessage = failedMessage,
                                    ApplicationPurposeType = applicationPurposeTypeClone,
                                    ApplicationType = applicationTypeClone,
                                    AppRefId = appRefId,
                                    RootActivityRefId = rootActivityRefId,
                                    TimeStemp = DateTime.Now,
                                    ToDoActivityCompleteType = toDoActivityCompleteType,
                                    ToDoActivityMapingVersion = toDoActivity.Version,
                                    ToDoActivityModeType = toDoActivityModeType,
                                    ToDoCodeType = toDoActivity.ToDoCodeType,
                                    UserRefId = user.Id,
                                    InvestPunjab_AppId = investPunjab_AppId,
                                    InvestPunjab_Ipin = iPin,
                                    ToDoActivityCategoryType = toDoActivityCategoryType
                                });
                            }
                            break;

                        case ToDoCodeTypeEnum.SET_LOCK_APPLICATION:
                            try
                            {
                                if (appRefId == 0)
                                {
                                    failedMessage = "Activity SET_LOCK_APPLICATION: appRefId=" + appRefId.ToString() + " is not available";
                                    toDoActivityCompleteType = ToDoActivityCompleteTypeEnum.FAILED_MISSING_PARAMETERS;
                                    isAnyStepHasException = true;
                                }
                                else
                                {
                                    await _iToDoManagerService.SetApplicationLock(appRefId, rootActivityRefId, toDoActivity.ToDoCodeType);
                                    failedMessage = "";
                                    toDoActivityCompleteType = ToDoActivityCompleteTypeEnum.SUCCEEDED;
                                }
                            }
                            catch (Exception ex)
                            {
                                failedMessage = "Activity SET_LOCK_APPLICATION: " + JsonConvert.SerializeObject(ex);
                                toDoActivityCompleteType = ToDoActivityCompleteTypeEnum.FAILED_WITH_ERRORS;
                                isAnyStepHasException = true;
                                throw ex;
                            }
                            finally
                            {
                                await _iToDoManagerService.CreateActivityLog(new ToDoActivityLog()
                                {
                                    ActivityFailedMessage = failedMessage,
                                    ApplicationPurposeType = applicationPurposeTypeClone,
                                    ApplicationType = applicationTypeClone,
                                    AppRefId = appRefId,
                                    RootActivityRefId = rootActivityRefId,
                                    TimeStemp = DateTime.Now,
                                    ToDoActivityCompleteType = toDoActivityCompleteType,
                                    ToDoActivityMapingVersion = toDoActivity.Version,
                                    ToDoActivityModeType = toDoActivityModeType,
                                    ToDoCodeType = toDoActivity.ToDoCodeType,
                                    UserRefId = user.Id,
                                    InvestPunjab_AppId = investPunjab_AppId,
                                    InvestPunjab_Ipin = iPin,
                                    ToDoActivityCategoryType = toDoActivityCategoryType
                                });
                            }
                            break;

                        case ToDoCodeTypeEnum.SET_APPLICATION_LIFECYCLE:
                            try
                            {
                                if (appRefId == 0)
                                {
                                    failedMessage = "Activity SET_APPLICATION_LIFECYCLE: appRefId=" + appRefId.ToString() + " is not available";
                                    toDoActivityCompleteType = ToDoActivityCompleteTypeEnum.FAILED_MISSING_PARAMETERS;
                                    isAnyStepHasException = true;
                                }
                                else
                                {
                                    ApplicationLifeCycleStatusTypeEnum appLifeCycleStatusType = ApplicationLifeCycleStatusTypeEnum.NOT_SUBMITTED;
                                    if (applicationTypeClone == ApplicationTypeEnum.SHOP_LICENCE)
                                    {
                                        appLifeCycleStatusType = ApplicationLifeCycleStatusTypeEnum.IN_PROCESS;
                                    }
                                        
                                    if (toDoActivityCategoryType == ToDoActivityCategoryTypeEnum.WITHDRAW_APPLICATION)
                                    {
                                        appLifeCycleStatusType = ApplicationLifeCycleStatusTypeEnum.WITHDRAW_APPLICATION;
                                    }
                                    await _iToDoManagerService.SetApplicationLifeCycle(appRefId, appLifeCycleStatusType, rootActivityRefId, toDoActivity.ToDoCodeType);
                                    failedMessage = "";
                                    toDoActivityCompleteType = ToDoActivityCompleteTypeEnum.SUCCEEDED;
                                }
                            }
                            catch (Exception ex)
                            {
                                failedMessage = "Activity SET_APPLICATION_LIFECYCLE: " + JsonConvert.SerializeObject(ex);
                                toDoActivityCompleteType = ToDoActivityCompleteTypeEnum.FAILED_WITH_ERRORS;
                                isAnyStepHasException = true;
                                throw ex;
                            }
                            finally
                            {
                                await _iToDoManagerService.CreateActivityLog(new ToDoActivityLog()
                                {
                                    ActivityFailedMessage = failedMessage,
                                    ApplicationPurposeType = applicationPurposeTypeClone,
                                    ApplicationType = applicationTypeClone,
                                    AppRefId = appRefId,
                                    RootActivityRefId = rootActivityRefId,
                                    TimeStemp = DateTime.Now,
                                    ToDoActivityCompleteType = toDoActivityCompleteType,
                                    ToDoActivityMapingVersion = toDoActivity.Version,
                                    ToDoActivityModeType = toDoActivityModeType,
                                    ToDoCodeType = toDoActivity.ToDoCodeType,
                                    UserRefId = user.Id,
                                    InvestPunjab_AppId = investPunjab_AppId,
                                    InvestPunjab_Ipin = iPin,
                                    ToDoActivityCategoryType = toDoActivityCategoryType
                                });
                            }
                            break;

                        case ToDoCodeTypeEnum.SEND_APPLICATION_TO_LANDING_OFFICER:
                            try
                            {
                                bool isThisStepHasLessParms = false;
                                StringBuilder errorMessage = new StringBuilder("");
                                propertyFinderResp = ReflectionOps.GeValueByPropName(requestData, "AppActionType");
                                AppActionTypeEnum appActionType = AppActionTypeEnum.DEFAULT;
                                if (propertyFinderResp.HasPropName)
                                {
                                    Enum.TryParse(propertyFinderResp.PropValue, out appActionType);
                                }
                                else
                                {
                                    errorMessage.Append("AppActionType KEY is missing");
                                    isThisStepHasLessParms = true;

                                }
                                
                                if (appActionType == AppActionTypeEnum.DEFAULT)
                                {
                                    errorMessage.Append(" AppActionType INVALID value");
                                    isThisStepHasLessParms = true;
                                }

                                propertyFinderResp = ReflectionOps.GeValueByPropName(requestData, "Remarks");
                                string remarks ="";
                                if (propertyFinderResp.HasPropName)
                                {
                                    remarks = propertyFinderResp.PropValue;
                                }
                                else
                                {
                                    errorMessage.Append(" Remarks KEY is missing");
                                    isThisStepHasLessParms = true;
                                }

                                if (appRefId == 0)
                                {
                                    errorMessage.Append(" AppRefId INVALID value");
                                    isThisStepHasLessParms = true;
                                }

                                if(!isThisStepHasLessParms)
                                {
                                    await _iToDoManagerService.SendApplicationToLandingOfficer(appRefId, applicationType, appActionType, remarks, rootActivityRefId, toDoActivity.ToDoCodeType);
                                    failedMessage = "";
                                    toDoActivityCompleteType = ToDoActivityCompleteTypeEnum.SUCCEEDED;
                                }
                                else
                                {
                                    failedMessage = "Activity SEND_APPLICATION_TO_LANDING_OFFICER: "+ errorMessage.ToString();
                                    toDoActivityCompleteType = ToDoActivityCompleteTypeEnum.FAILED_MISSING_PARAMETERS;
                                    isAnyStepHasException = true;
                                }

                            }
                            catch (Exception ex)
                            {
                                failedMessage = "Activity SEND_APPLICATION_TO_LANDING_OFFICER: " + JsonConvert.SerializeObject(ex);
                                toDoActivityCompleteType = ToDoActivityCompleteTypeEnum.FAILED_WITH_ERRORS;
                                isAnyStepHasException = true;
                                throw ex;
                            }
                            finally
                            {
                                await _iToDoManagerService.CreateActivityLog(new ToDoActivityLog()
                                {
                                    ActivityFailedMessage = failedMessage,
                                    ApplicationPurposeType = applicationPurposeTypeClone,
                                    ApplicationType = applicationTypeClone,
                                    AppRefId = appRefId,
                                    RootActivityRefId = rootActivityRefId,
                                    TimeStemp = DateTime.Now,
                                    ToDoActivityCompleteType = toDoActivityCompleteType,
                                    ToDoActivityMapingVersion = toDoActivity.Version,
                                    ToDoActivityModeType = toDoActivityModeType,
                                    ToDoCodeType = toDoActivity.ToDoCodeType,
                                    UserRefId = user.Id,
                                    InvestPunjab_AppId = investPunjab_AppId,
                                    InvestPunjab_Ipin = iPin,
                                    ToDoActivityCategoryType = toDoActivityCategoryType
                                });
                            }
                            break;

                        case ToDoCodeTypeEnum.SEND_APPLICATION_TO_LANDING_OFFICER_OBJECTION_RESOLVED:
                            try
                            {
                                bool isThisStepHasLessParms = false;
                                StringBuilder errorMessage = new StringBuilder("");
                                propertyFinderResp = ReflectionOps.GeValueByPropName(requestData, "AppActionType");
                                AppActionTypeEnum appActionType = AppActionTypeEnum.DEFAULT;
                                if (propertyFinderResp.HasPropName)
                                {
                                    Enum.TryParse(propertyFinderResp.PropValue, out appActionType);
                                }
                                else
                                {
                                    errorMessage.Append("AppActionType KEY is missing");
                                    isThisStepHasLessParms = true;

                                }

                                if (appActionType == AppActionTypeEnum.DEFAULT)
                                {
                                    errorMessage.Append(" AppActionType INVALID value");
                                    isThisStepHasLessParms = true;
                                }

                                propertyFinderResp = ReflectionOps.GeValueByPropName(requestData, "Remarks");
                                string remarks="";
                                if (propertyFinderResp.HasPropName)
                                {
                                    remarks = propertyFinderResp.PropValue;
                                }
                                else
                                {
                                    errorMessage.Append(" Remarks KEY is missing");
                                    isThisStepHasLessParms = true;
                                }

                                if (appRefId == 0)
                                {
                                    errorMessage.Append(" AppRefId INVALID value");
                                    isThisStepHasLessParms = true;
                                }

                                if (!isThisStepHasLessParms)
                                {
                                    await _iToDoManagerService.SendApplicationToLandingOfficerAfterObjectionResolved(appRefId, AppActionTypeEnum.OBJECTION_RESOLVED_APPLICATION_RESUBMITTED, remarks, rootActivityRefId, toDoActivity.ToDoCodeType);
                                    failedMessage = "";
                                    toDoActivityCompleteType = ToDoActivityCompleteTypeEnum.SUCCEEDED;
                                }
                                else
                                {
                                    failedMessage = "Activity SEND_APPLICATION_TO_LANDING_OFFICER_OBJECTION_RESOLVED: " + errorMessage.ToString();
                                    toDoActivityCompleteType = ToDoActivityCompleteTypeEnum.FAILED_MISSING_PARAMETERS;
                                    isAnyStepHasException = true;
                                }

                            }
                            catch (Exception ex)
                            {
                                failedMessage = "Activity SEND_APPLICATION_TO_LANDING_OFFICER_OBJECTION_RESOLVED: " + JsonConvert.SerializeObject(ex);
                                toDoActivityCompleteType = ToDoActivityCompleteTypeEnum.FAILED_WITH_ERRORS;
                                isAnyStepHasException = true;
                                throw ex;
                            }
                            finally
                            {
                                await _iToDoManagerService.CreateActivityLog(new ToDoActivityLog()
                                {
                                    ActivityFailedMessage = failedMessage,
                                    ApplicationPurposeType = applicationPurposeTypeClone,
                                    ApplicationType = applicationTypeClone,
                                    AppRefId = appRefId,
                                    RootActivityRefId = rootActivityRefId,
                                    TimeStemp = DateTime.Now,
                                    ToDoActivityCompleteType = toDoActivityCompleteType,
                                    ToDoActivityMapingVersion = toDoActivity.Version,
                                    ToDoActivityModeType = toDoActivityModeType,
                                    ToDoCodeType = toDoActivity.ToDoCodeType,
                                    UserRefId = user.Id,
                                    InvestPunjab_AppId = investPunjab_AppId,
                                    InvestPunjab_Ipin = iPin,
                                    ToDoActivityCategoryType = toDoActivityCategoryType
                                });
                            }
                            break;

                        case ToDoCodeTypeEnum.SET_ISHAVING_EMPLOYEE_FLAG:
                            try
                            {

                                bool isThisStepHasLessParms = false;
                                StringBuilder errorMessage = new StringBuilder("");

                                propertyFinderResp = ReflectionOps.GeValueByPropName(requestData, "IsHavingEmployee");
                                int isHavingEmployee = -1;
                                if (propertyFinderResp.HasPropName)
                                {
                                    isHavingEmployee = Convert.ToInt32(propertyFinderResp.PropValue);
                                }
                                else
                                {
                                    isThisStepHasLessParms = true;
                                    errorMessage.Append("IsHavingEmployee is missing.");
                                }
                                if (!isThisStepHasLessParms && (isHavingEmployee < 0 || isHavingEmployee > 1))
                                {
                                    errorMessage.Append("IsHavingEmployee has invalid value = " + isHavingEmployee.ToString() +". ");
                                }


                                propertyFinderResp = ReflectionOps.GeValueByPropName(requestData, "ShopLicenceId");
                                Int64 shopLicenceId = -1;
                                if (propertyFinderResp.HasPropName)
                                {
                                    shopLicenceId = Convert.ToInt64(propertyFinderResp.PropValue);
                                }
                                else
                                {
                                    isThisStepHasLessParms = true;
                                    errorMessage.Append("ShopLicenceId is missing. ");
                                }
                                if (!isThisStepHasLessParms && shopLicenceId <= 0 )
                                {
                                    isThisStepHasLessParms = true;
                                    errorMessage.Append("ShopLicenceId has invalid value = " + shopLicenceId.ToString() + ". ");
                                }

                                if (!isAnyStepHasException)
                                {
                                    await _iToDoManagerService.SET_ISHAVING_EMPLOYEE_FLAG(shopLicenceId, isHavingEmployee, rootActivityRefId, toDoActivity.ToDoCodeType);
                                    failedMessage = "";
                                    toDoActivityCompleteType = ToDoActivityCompleteTypeEnum.SUCCEEDED;
                                }
                                else
                                {
                                    failedMessage = "Activity SET_ISHAVING_EMPLOYEE_FLAG: " + errorMessage.ToString();
                                    toDoActivityCompleteType = ToDoActivityCompleteTypeEnum.FAILED_MISSING_PARAMETERS;
                                    isAnyStepHasException = true;
                                }
                            }
                            catch (Exception ex)
                            {
                                failedMessage = "Activity SET_ISHAVING_EMPLOYEE_FLAG: " + JsonConvert.SerializeObject(ex);
                                toDoActivityCompleteType = ToDoActivityCompleteTypeEnum.FAILED_WITH_ERRORS;
                                isAnyStepHasException = true;
                                throw ex;
                            }
                            finally
                            {
                                await _iToDoManagerService.CreateActivityLog(new ToDoActivityLog()
                                {
                                    ActivityFailedMessage = failedMessage,
                                    ApplicationPurposeType = applicationPurposeTypeClone,
                                    ApplicationType = applicationTypeClone,
                                    AppRefId = appRefId,
                                    RootActivityRefId = rootActivityRefId,
                                    TimeStemp = DateTime.Now,
                                    ToDoActivityCompleteType = toDoActivityCompleteType,
                                    ToDoActivityMapingVersion = toDoActivity.Version,
                                    ToDoActivityModeType = toDoActivityModeType,
                                    ToDoCodeType = toDoActivity.ToDoCodeType,
                                    UserRefId = user.Id,
                                    InvestPunjab_AppId = investPunjab_AppId,
                                    InvestPunjab_Ipin = iPin,
                                    ToDoActivityCategoryType = toDoActivityCategoryType
                                });
                            }
                            break;
                            
                        case ToDoCodeTypeEnum.SET_REG_REN_AMD_DATES:
                            try
                            {
                                if (applicationPurposeType == 0)
                                {
                                    failedMessage = "Activity SET_REG_REN_AMD_DATES: applicationPurposeType=" + applicationPurposeType.ToString() + " is not available";
                                    toDoActivityCompleteType = ToDoActivityCompleteTypeEnum.FAILED_MISSING_PARAMETERS;
                                    isAnyStepHasException = true;
                                }
                                else
                                {
                                    await _iToDoManagerService.SET_REG_REN_AMD_DATES(requestData, rootActivityRefId, toDoActivity.ToDoCodeType);
                                    failedMessage = "";
                                    toDoActivityCompleteType = ToDoActivityCompleteTypeEnum.SUCCEEDED;
                                }
                            }
                            catch (Exception ex)
                            {
                                failedMessage = "Activity SET_REG_REN_AMD_DATES: " + JsonConvert.SerializeObject(ex);
                                toDoActivityCompleteType = ToDoActivityCompleteTypeEnum.FAILED_WITH_ERRORS;
                                isAnyStepHasException = true;
                                throw ex;
                            }
                            finally
                            {
                                await _iToDoManagerService.CreateActivityLog(new ToDoActivityLog()
                                {
                                    ActivityFailedMessage = failedMessage,
                                    ApplicationPurposeType = applicationPurposeTypeClone,
                                    ApplicationType = applicationTypeClone,
                                    AppRefId = appRefId,
                                    RootActivityRefId = rootActivityRefId,
                                    TimeStemp = DateTime.Now,
                                    ToDoActivityCompleteType = toDoActivityCompleteType,
                                    ToDoActivityMapingVersion = toDoActivity.Version,
                                    ToDoActivityModeType = toDoActivityModeType,
                                    ToDoCodeType = toDoActivity.ToDoCodeType,
                                    UserRefId = user.Id,
                                    InvestPunjab_AppId = investPunjab_AppId,
                                    InvestPunjab_Ipin = iPin,
                                    ToDoActivityCategoryType = toDoActivityCategoryType
                                });
                            }
                            break;

                        case ToDoCodeTypeEnum.SET_AMENDMENT_HISTORY_COUNTER:
                            try
                            {
                                if (appRefId == 0)
                                {
                                    failedMessage = "Activity SET_AMENDMENT_HISTORY_COUNTER: appRefId=" + appRefId.ToString() + " is not available";
                                    toDoActivityCompleteType = ToDoActivityCompleteTypeEnum.FAILED_MISSING_PARAMETERS;
                                    isAnyStepHasException = true;
                                }
                                else
                                {
                                    await _iToDoManagerService.SET_AMENDMENT_HISTORY_COUNTER(appRefId, rootActivityRefId, toDoActivity.ToDoCodeType);
                                    failedMessage = "";
                                    toDoActivityCompleteType = ToDoActivityCompleteTypeEnum.SUCCEEDED;
                                }
                            }
                            catch (Exception ex)
                            {
                                failedMessage = "Activity SET_AMENDMENT_HISTORY_COUNTER: " + JsonConvert.SerializeObject(ex);
                                toDoActivityCompleteType = ToDoActivityCompleteTypeEnum.FAILED_WITH_ERRORS;
                                isAnyStepHasException = true;
                                throw ex;
                            }
                            finally
                            {
                                await _iToDoManagerService.CreateActivityLog(new ToDoActivityLog()
                                {
                                    ActivityFailedMessage = failedMessage,
                                    ApplicationPurposeType = applicationPurposeTypeClone,
                                    ApplicationType = applicationTypeClone,
                                    AppRefId = appRefId,
                                    RootActivityRefId = rootActivityRefId,
                                    TimeStemp = DateTime.Now,
                                    ToDoActivityCompleteType = toDoActivityCompleteType,
                                    ToDoActivityMapingVersion = toDoActivity.Version,
                                    ToDoActivityModeType = toDoActivityModeType,
                                    ToDoCodeType = toDoActivity.ToDoCodeType,
                                    UserRefId = user.Id,
                                    InvestPunjab_AppId = investPunjab_AppId,
                                    InvestPunjab_Ipin = iPin,
                                    ToDoActivityCategoryType = toDoActivityCategoryType
                                });
                            }
                            break;

                        case ToDoCodeTypeEnum.PREPARE_DATA_AS_PER_TEMP_REG_FLAG:
                            try
                            {
                                var isTempReg = await IsFactoryHasTempRegistrationFlag(requestData);
                                if (isTempReg.ResponseDataModel.IsTempRegistered == 1 && isTempReg.ResponseDataModel.isTempRegistrationVerified)
                                {
                                    if (appRefId == 0)
                                    {
                                        failedMessage = "Activity PREPARE_DATA_AS_PER_TEMP_REG_FLAG: appRefId=" + appRefId.ToString() + " is not available";
                                        toDoActivityCompleteType = ToDoActivityCompleteTypeEnum.FAILED_MISSING_PARAMETERS;
                                        isAnyStepHasException = true;
                                    }
                                    else
                                    {
                                        propertyFinderResp = ReflectionOps.GeValueByPropName(requestData, "TempRegistrationNumber");
                                        string tempRegistrationNumber = "";
                                        if (propertyFinderResp.HasPropName)
                                        {
                                            tempRegistrationNumber = propertyFinderResp.PropValue;
                                        }

                                        if (tempRegistrationNumber == null)
                                        {
                                            failedMessage = "Activity PREPARE_DATA_AS_PER_TEMP_REG_FLAG: TempRegistrationNumber is not available";
                                            toDoActivityCompleteType = ToDoActivityCompleteTypeEnum.FAILED_MISSING_PARAMETERS;
                                            isAnyStepHasException = true;
                                        }
                                        else
                                        {
                                            await _iToDoManagerService.PREPARE_DATA_AS_PER_TEMP_REG_FLAG(appRefId, entityKeyId, tempRegistrationNumber, rootActivityRefId, toDoActivity.ToDoCodeType);
                                            failedMessage = "";
                                            toDoActivityCompleteType = ToDoActivityCompleteTypeEnum.SUCCEEDED;
                                        }
                                    }
                                }
                                else
                                {
                                    failedMessage = "Activity PREPARE_DATA_AS_PER_TEMP_REG_FLAG: Temporary registration is not applicable.";
                                    toDoActivityCompleteType = ToDoActivityCompleteTypeEnum.SKIPPED;
                                }
                            }
                            catch (Exception ex)
                            {
                                failedMessage = "Activity PREPARE_DATA_AS_PER_TEMP_REG_FLAG: " + JsonConvert.SerializeObject(ex);
                                toDoActivityCompleteType = ToDoActivityCompleteTypeEnum.FAILED_WITH_ERRORS;
                                isAnyStepHasException = true;
                                throw ex;
                            }
                            finally
                            {
                                await _iToDoManagerService.CreateActivityLog(new ToDoActivityLog()
                                {
                                    ActivityFailedMessage = failedMessage,
                                    ApplicationPurposeType = applicationPurposeTypeClone,
                                    ApplicationType = applicationTypeClone,
                                    AppRefId = appRefId,
                                    RootActivityRefId = rootActivityRefId,
                                    TimeStemp = DateTime.Now,
                                    ToDoActivityCompleteType = toDoActivityCompleteType,
                                    ToDoActivityMapingVersion = toDoActivity.Version,
                                    ToDoActivityModeType = toDoActivityModeType,
                                    ToDoCodeType = toDoActivity.ToDoCodeType,
                                    UserRefId = user.Id,
                                    InvestPunjab_AppId = investPunjab_AppId,
                                    InvestPunjab_Ipin = iPin,
                                    ToDoActivityCategoryType = toDoActivityCategoryType
                                });
                            }
                            break;

                        case ToDoCodeTypeEnum.PREPARE_AND_SET_AMENDMENT_HISTORY_DATA:
                            try
                            {
                                if (appRefId == 0)
                                {
                                    failedMessage = "Activity PREPARE_AND_SET_AMENDMENT_HISTORY_DATA: appRefId=" + appRefId.ToString() + " is not available";
                                    toDoActivityCompleteType = ToDoActivityCompleteTypeEnum.FAILED_MISSING_PARAMETERS;
                                    isAnyStepHasException = true;
                                }
                                else
                                {
                                    await _iToDoManagerService.PREPARE_AND_SET_AMENDMENT_HISTORY_DATA(requestData, appRefId, rootActivityRefId, toDoActivity.ToDoCodeType);
                                    failedMessage = "";
                                    toDoActivityCompleteType = ToDoActivityCompleteTypeEnum.SUCCEEDED;
                                }
                            }
                            catch (Exception ex)
                            {
                                failedMessage = "Activity PREPARE_AND_SET_AMENDMENT_HISTORY_DATA: " + JsonConvert.SerializeObject(ex);
                                toDoActivityCompleteType = ToDoActivityCompleteTypeEnum.FAILED_WITH_ERRORS;
                                isAnyStepHasException = true;
                                throw ex;
                            }
                            finally
                            {
                                await _iToDoManagerService.CreateActivityLog(new ToDoActivityLog()
                                {
                                    ActivityFailedMessage = failedMessage,
                                    ApplicationPurposeType = applicationPurposeTypeClone,
                                    ApplicationType = applicationTypeClone,
                                    AppRefId = appRefId,
                                    RootActivityRefId = rootActivityRefId,
                                    TimeStemp = DateTime.Now,
                                    ToDoActivityCompleteType = toDoActivityCompleteType,
                                    ToDoActivityMapingVersion = toDoActivity.Version,
                                    ToDoActivityModeType = toDoActivityModeType,
                                    ToDoCodeType = toDoActivity.ToDoCodeType,
                                    UserRefId = user.Id,
                                    InvestPunjab_AppId = investPunjab_AppId,
                                    InvestPunjab_Ipin = iPin,
                                    ToDoActivityCategoryType = toDoActivityCategoryType
                                });
                            }
                            break;

                        case ToDoCodeTypeEnum.SET_IS_FEE_APPLICABLE_FLAG:
                            try
                            {
                                
                                if (appRefId == 0)
                                {
                                    failedMessage = "Activity SET_IS_FEE_APPLICABLE_FLAG: appRefId=" + appRefId.ToString() + " is not available";
                                    toDoActivityCompleteType = ToDoActivityCompleteTypeEnum.FAILED_MISSING_PARAMETERS;
                                    isAnyStepHasException = true;
                                }
                                else
                                {
                                    bool isFeeApplicable = true;
                                    if(applicationType == ApplicationTypeEnum.FACTORY_LICENCE)
                                    {
                                        var isTempReg = await IsFactoryHasTempRegistrationFlag(requestData);
                                        if (isTempReg.ResponseDataModel.IsTempRegistered==1)
                                        {
                                            isFeeApplicable = false;
                                        }
                                    }
                                    else if(applicationType == ApplicationTypeEnum.BUILDING_PLAN_PROPOSED || applicationType == ApplicationTypeEnum.BUILDING_PLAN_EXISTING || applicationType == ApplicationTypeEnum.BUILDING_PLAN_ADDITION_AMENDMENT)
                                    {
                                        isFeeApplicable = false;
                                    }

                                    await _iToDoManagerService.SET_IS_FEE_APPLICABLE_FLAG(appRefId, isFeeApplicable, rootActivityRefId, toDoActivity.ToDoCodeType);
                                    failedMessage = "";
                                    toDoActivityCompleteType = ToDoActivityCompleteTypeEnum.SUCCEEDED;
                                }
                            }
                            catch (Exception ex)
                            {
                                failedMessage = "Activity SET_IS_FEE_APPLICABLE_FLAG: " + JsonConvert.SerializeObject(ex);
                                toDoActivityCompleteType = ToDoActivityCompleteTypeEnum.FAILED_WITH_ERRORS;
                                isAnyStepHasException = true;
                                throw ex;
                            }
                            finally
                            {
                                await _iToDoManagerService.CreateActivityLog(new ToDoActivityLog()
                                {
                                    ActivityFailedMessage = failedMessage,
                                    ApplicationPurposeType = applicationPurposeTypeClone,
                                    ApplicationType = applicationTypeClone,
                                    AppRefId = appRefId,
                                    RootActivityRefId = rootActivityRefId,
                                    TimeStemp = DateTime.Now,
                                    ToDoActivityCompleteType = toDoActivityCompleteType,
                                    ToDoActivityMapingVersion = toDoActivity.Version,
                                    ToDoActivityModeType = toDoActivityModeType,
                                    ToDoCodeType = toDoActivity.ToDoCodeType,
                                    UserRefId = user.Id,
                                    InvestPunjab_AppId = investPunjab_AppId,
                                    InvestPunjab_Ipin = iPin,
                                    ToDoActivityCategoryType = toDoActivityCategoryType
                                });
                            }
                            break;

                        case ToDoCodeTypeEnum.SEND_NOTIFICATION:
                            try
                            {
                                if (appRefId == 0)
                                {
                                    failedMessage = "Activity SEND_NOTIFICATION: appRefId=" + appRefId.ToString() + " is not available";
                                    toDoActivityCompleteType = ToDoActivityCompleteTypeEnum.FAILED_MISSING_PARAMETERS;
                                    isAnyStepHasException = true;
                                }
                                else
                                {
                                    await _iToDoManagerService.SEND_NOTIFICATION(appRefId, rootActivityRefId, toDoActivity.ToDoCodeType);
                                    failedMessage = "";
                                    toDoActivityCompleteType = ToDoActivityCompleteTypeEnum.SUCCEEDED;
                                }
                            }
                            catch (Exception ex)
                            {
                                failedMessage = "Activity SEND_NOTIFICATION: " + JsonConvert.SerializeObject(ex);
                                toDoActivityCompleteType = ToDoActivityCompleteTypeEnum.FAILED_WITH_ERRORS;
                                isAnyStepHasException = true;
                                throw ex;
                            }
                            finally
                            {
                                await _iToDoManagerService.CreateActivityLog(new ToDoActivityLog()
                                {
                                    ActivityFailedMessage = failedMessage,
                                    ApplicationPurposeType = applicationPurposeTypeClone,
                                    ApplicationType = applicationTypeClone,
                                    AppRefId = appRefId,
                                    RootActivityRefId = rootActivityRefId,
                                    TimeStemp = DateTime.Now,
                                    ToDoActivityCompleteType = toDoActivityCompleteType,
                                    ToDoActivityMapingVersion = toDoActivity.Version,
                                    ToDoActivityModeType = toDoActivityModeType,
                                    ToDoCodeType = toDoActivity.ToDoCodeType,
                                    UserRefId = user.Id,
                                    InvestPunjab_AppId = investPunjab_AppId,
                                    InvestPunjab_Ipin = iPin,
                                    ToDoActivityCategoryType = toDoActivityCategoryType
                                });
                            }
                            break;

                        case ToDoCodeTypeEnum.ADD_PAYMENT_RAISED_FEE:
                            try
                            {
                                if (appRefId == 0)
                                {
                                    failedMessage = "Activity ADD_PAYMENT_RAISED_FEE: appRefId=" + appRefId.ToString() + " is not available";
                                    toDoActivityCompleteType = ToDoActivityCompleteTypeEnum.FAILED_MISSING_PARAMETERS;
                                    isAnyStepHasException = true;
                                }
                                else
                                {
                                    await _iToDoManagerService.ADD_PAYMENT_RAISED_FEE(requestData, appRefId, rootActivityRefId, toDoActivity.ToDoCodeType);
                                    failedMessage = "";
                                    toDoActivityCompleteType = ToDoActivityCompleteTypeEnum.SUCCEEDED;
                                }
                            }
                            catch (Exception ex)
                            {
                                failedMessage = "Activity ADD_PAYMENT_RAISED_FEE: " + JsonConvert.SerializeObject(ex);
                                toDoActivityCompleteType = ToDoActivityCompleteTypeEnum.FAILED_WITH_ERRORS;
                                isAnyStepHasException = true;
                                throw ex;
                            }
                            finally
                            {
                                await _iToDoManagerService.CreateActivityLog(new ToDoActivityLog()
                                {
                                    ActivityFailedMessage = failedMessage,
                                    ApplicationPurposeType = applicationPurposeType,
                                    ApplicationType = applicationType,
                                    AppRefId = appRefId,
                                    RootActivityRefId = rootActivityRefId,
                                    TimeStemp = DateTime.Now,
                                    ToDoActivityCompleteType = toDoActivityCompleteType,
                                    ToDoActivityMapingVersion = toDoActivity.Version,
                                    ToDoActivityModeType = toDoActivityModeType,
                                    ToDoCodeType = toDoActivity.ToDoCodeType,
                                    UserRefId = user.Id,
                                    InvestPunjab_AppId = investPunjab_AppId,
                                    InvestPunjab_Ipin = iPin,
                                    ToDoActivityCategoryType = toDoActivityCategoryType
                                });
                            }
                            break;

                        case ToDoCodeTypeEnum.INCREASE_PAYMENT_BATCH_COUNTER:
                            try
                            {
                                if (appRefId == 0)
                                {
                                    failedMessage = "Activity INCREASE_PAYMENT_BATCH_COUNTER: appRefId=" + appRefId.ToString() + " is not available";
                                    toDoActivityCompleteType = ToDoActivityCompleteTypeEnum.FAILED_MISSING_PARAMETERS;
                                    isAnyStepHasException = true;
                                }
                                else
                                {
                                    await _iToDoManagerService.INCREASE_PAYMENT_BATCH_COUNTER(appRefId, rootActivityRefId, toDoActivity.ToDoCodeType);
                                    failedMessage = "";
                                    toDoActivityCompleteType = ToDoActivityCompleteTypeEnum.SUCCEEDED;
                                }
                            }
                            catch (Exception ex)
                            {
                                failedMessage = "Activity INCREASE_PAYMENT_BATCH_COUNTER: " + JsonConvert.SerializeObject(ex);
                                toDoActivityCompleteType = ToDoActivityCompleteTypeEnum.FAILED_WITH_ERRORS;
                                isAnyStepHasException = true;
                                throw ex;
                            }
                            finally
                            {
                                await _iToDoManagerService.CreateActivityLog(new ToDoActivityLog()
                                {
                                    ActivityFailedMessage = failedMessage,
                                    ApplicationPurposeType = applicationPurposeType,
                                    ApplicationType = applicationType,
                                    AppRefId = appRefId,
                                    RootActivityRefId = rootActivityRefId,
                                    TimeStemp = DateTime.Now,
                                    ToDoActivityCompleteType = toDoActivityCompleteType,
                                    ToDoActivityMapingVersion = toDoActivity.Version,
                                    ToDoActivityModeType = toDoActivityModeType,
                                    ToDoCodeType = toDoActivity.ToDoCodeType,
                                    UserRefId = user.Id,
                                    InvestPunjab_AppId = investPunjab_AppId,
                                    InvestPunjab_Ipin = iPin,
                                    ToDoActivityCategoryType = toDoActivityCategoryType
                                });
                            }
                            break;

                        case ToDoCodeTypeEnum.SET_ACTION_LOG_ID:
                            try
                            {
                                if (appRefId == 0)
                                {
                                    failedMessage = "Activity SET_ACTION_LOG_ID: appRefId=" + appRefId.ToString() + " is not available";
                                    toDoActivityCompleteType = ToDoActivityCompleteTypeEnum.FAILED_MISSING_PARAMETERS;
                                    isAnyStepHasException = true;
                                }
                                else
                                {
                                    await _iToDoManagerService.SET_ACTION_LOG_ID(appRefId, rootActivityRefId, toDoActivity.ToDoCodeType);
                                    failedMessage = "";
                                    toDoActivityCompleteType = ToDoActivityCompleteTypeEnum.SUCCEEDED;
                                }
                            }
                            catch (Exception ex)
                            {
                                failedMessage = "Activity SET_ACTION_LOG_ID: " + JsonConvert.SerializeObject(ex);
                                toDoActivityCompleteType = ToDoActivityCompleteTypeEnum.FAILED_WITH_ERRORS;
                                isAnyStepHasException = true;
                                throw ex;
                            }
                            finally
                            {
                                await _iToDoManagerService.CreateActivityLog(new ToDoActivityLog()
                                {
                                    ActivityFailedMessage = failedMessage,
                                    ApplicationPurposeType = applicationPurposeType,
                                    ApplicationType = applicationType,
                                    AppRefId = appRefId,
                                    RootActivityRefId = rootActivityRefId,
                                    TimeStemp = DateTime.Now,
                                    ToDoActivityCompleteType = toDoActivityCompleteType,
                                    ToDoActivityMapingVersion = toDoActivity.Version,
                                    ToDoActivityModeType = toDoActivityModeType,
                                    ToDoCodeType = toDoActivity.ToDoCodeType,
                                    UserRefId = user.Id,
                                    InvestPunjab_AppId = investPunjab_AppId,
                                    InvestPunjab_Ipin = iPin,
                                    ToDoActivityCategoryType = toDoActivityCategoryType
                                });
                            }
                            break;

                        case ToDoCodeTypeEnum.UPDATE_PAYMENT_RAISED_FEE:
                            try
                            {
                                if (appRefId == 0)
                                {
                                    failedMessage = "Activity UPDATE_PAYMENT_RAISED_FEE: appRefId=" + appRefId.ToString() + " is not available";
                                    toDoActivityCompleteType = ToDoActivityCompleteTypeEnum.FAILED_MISSING_PARAMETERS;
                                    isAnyStepHasException = true;
                                }
                                else
                                {
                                    await _iToDoManagerService.UPDATE_PAYMENT_RAISED_FEE(requestData, rootActivityRefId, toDoActivity.ToDoCodeType);
                                    failedMessage = "";
                                    toDoActivityCompleteType = ToDoActivityCompleteTypeEnum.SUCCEEDED;
                                }
                            }
                            catch (Exception ex)
                            {
                                failedMessage = "Activity UPDATE_PAYMENT_RAISED_FEE: " + JsonConvert.SerializeObject(ex);
                                toDoActivityCompleteType = ToDoActivityCompleteTypeEnum.FAILED_WITH_ERRORS;
                                isAnyStepHasException = true;
                                throw ex;
                            }
                            finally
                            {
                                await _iToDoManagerService.CreateActivityLog(new ToDoActivityLog()
                                {
                                    ActivityFailedMessage = failedMessage,
                                    ApplicationPurposeType = applicationPurposeType,
                                    ApplicationType = applicationType,
                                    AppRefId = appRefId,
                                    RootActivityRefId = rootActivityRefId,
                                    TimeStemp = DateTime.Now,
                                    ToDoActivityCompleteType = toDoActivityCompleteType,
                                    ToDoActivityMapingVersion = toDoActivity.Version,
                                    ToDoActivityModeType = toDoActivityModeType,
                                    ToDoCodeType = toDoActivity.ToDoCodeType,
                                    UserRefId = user.Id,
                                    InvestPunjab_AppId = investPunjab_AppId,
                                    InvestPunjab_Ipin = iPin,
                                    ToDoActivityCategoryType = toDoActivityCategoryType
                                });
                            }
                            break;

                        case ToDoCodeTypeEnum.WITHDRAW_APPLICATION:
                            try
                            {
                                bool isThisStepHasLessParms = false;
                                StringBuilder errorMessage = new StringBuilder("");
                                propertyFinderResp = ReflectionOps.GeValueByPropName(requestData, "Remarks");
                                string remarks = "";
                                if (propertyFinderResp.HasPropName)
                                {
                                    remarks = propertyFinderResp.PropValue;
                                }
                                else
                                {
                                    errorMessage.Append(" Remarks KEY is missing");
                                    isThisStepHasLessParms = true;
                                }

                                if (appRefId == 0)
                                {
                                    errorMessage.Append(" AppRefId INVALID value");
                                    isThisStepHasLessParms = true;
                                }

                                if (!isThisStepHasLessParms)
                                {
                                    await _iToDoManagerService.WITHDRAW_APPLICATION(appRefId, remarks, rootActivityRefId, toDoActivity.ToDoCodeType);
                                    failedMessage = "";
                                    toDoActivityCompleteType = ToDoActivityCompleteTypeEnum.SUCCEEDED;
                                }
                                else
                                {
                                    failedMessage = "Activity WITHDRAW_APPLICATION: " + errorMessage.ToString();
                                    toDoActivityCompleteType = ToDoActivityCompleteTypeEnum.FAILED_MISSING_PARAMETERS;
                                    isAnyStepHasException = true;
                                }
                            }
                            catch (Exception ex)
                            {
                                failedMessage = "Activity WITHDRAW_APPLICATION: " + JsonConvert.SerializeObject(ex);
                                toDoActivityCompleteType = ToDoActivityCompleteTypeEnum.FAILED_WITH_ERRORS;
                                isAnyStepHasException = true;
                                throw ex;
                            }
                            finally
                            {
                                await _iToDoManagerService.CreateActivityLog(new ToDoActivityLog()
                                {
                                    ActivityFailedMessage = failedMessage,
                                    ApplicationPurposeType = applicationPurposeType,
                                    ApplicationType = applicationType,
                                    AppRefId = appRefId,
                                    RootActivityRefId = rootActivityRefId,
                                    TimeStemp = DateTime.Now,
                                    ToDoActivityCompleteType = toDoActivityCompleteType,
                                    ToDoActivityMapingVersion = toDoActivity.Version,
                                    ToDoActivityModeType = toDoActivityModeType,
                                    ToDoCodeType = toDoActivity.ToDoCodeType,
                                    UserRefId = user.Id,
                                    InvestPunjab_AppId = investPunjab_AppId,
                                    InvestPunjab_Ipin = iPin,
                                    ToDoActivityCategoryType = toDoActivityCategoryType
                                });
                            }
                            break;

                        case ToDoCodeTypeEnum.SEED_TIME_LINE_FLOW_DATA:
                            try
                            {
                                if (appRefId == 0)
                                {
                                    failedMessage = "Activity SEED_TIME_LINE_FLOW_DATA: appRefId=" + appRefId.ToString() + " is not available";
                                    toDoActivityCompleteType = ToDoActivityCompleteTypeEnum.FAILED_MISSING_PARAMETERS;
                                    isAnyStepHasException = true;
                                }
                                else
                                {
                                    await _iToDoManagerService.SEED_TIME_LINE_FLOW_DATA(appRefId, rootActivityRefId, toDoActivity.ToDoCodeType);

                                    failedMessage = "";
                                    toDoActivityCompleteType = ToDoActivityCompleteTypeEnum.SUCCEEDED;
                                }
                            }
                            catch (Exception ex)
                            {
                                failedMessage = "Activity SEED_TIME_LINE_FLOW_DATA: " + JsonConvert.SerializeObject(ex);
                                toDoActivityCompleteType = ToDoActivityCompleteTypeEnum.FAILED_WITH_ERRORS;
                                isAnyStepHasException = true;
                                throw ex;
                            }
                            finally
                            {
                                await _iToDoManagerService.CreateActivityLog(new ToDoActivityLog()
                                {
                                    ActivityFailedMessage = failedMessage,
                                    ApplicationPurposeType = applicationPurposeType,
                                    ApplicationType = applicationType,
                                    AppRefId = appRefId,
                                    RootActivityRefId = rootActivityRefId,
                                    TimeStemp = DateTime.Now,
                                    ToDoActivityCompleteType = toDoActivityCompleteType,
                                    ToDoActivityMapingVersion = toDoActivity.Version,
                                    ToDoActivityModeType = toDoActivityModeType,
                                    ToDoCodeType = toDoActivity.ToDoCodeType,
                                    UserRefId = user.Id,
                                    InvestPunjab_AppId = investPunjab_AppId,
                                    InvestPunjab_Ipin = iPin,
                                    ToDoActivityCategoryType = toDoActivityCategoryType
                                });
                            }
                            break;
                    }
                }

                if (isAnyStepHasException)
                {
                    resp.HasExceptions = true;
                    resp.ErrorDesc = "Operation Completed With Errors";
                    resp.RootActivityRefId = rootActivityRefId;
                    await _iToDoManagerService.RaiseTicket(user.Id, rootActivityRefId, applicationTypeClone, applicationPurposeTypeClone, toDoActivityCategoryType, iPin, investPunjab_AppId);

                }
                else
                {
                    resp.AppId = appRefId;
                    resp.EntityKeyId = entityKeyId;
                    resp.HasExceptions = false;
                    resp.ErrorDesc = "Operation Completed";
                }
            }
            else
            {
                resp.HasExceptions = true;
                resp.ErrorDesc = "Failed in PreRequisites SET-1";
                //Failed in PreRequisites Set 1
            }
            return resp;
        }

        public async Task<CRUD_CreateUpdateOperationResponse> OfficerCreateUpdate(object requestData, User user)
        {
            CRUD_CreateUpdateOperationResponse resp = new CRUD_CreateUpdateOperationResponse();
            bool is_preRequisites_set_1_meets = true;
            PropertyFinderRespViewModel propertyFinderResp = ReflectionOps.GeValueByPropName(requestData, "toDoActivityCategoryType");
            ToDoActivityCategoryTypeEnum toDoActivityCategoryType = ToDoActivityCategoryTypeEnum.DEFAULT;
            ApplicationTypeEnum applicationType = ApplicationTypeEnum.DEFAULT;
            ApplicationPurposeTypeEnum applicationPurposeType = ApplicationPurposeTypeEnum.DEFAULT;
            Int64 projectSiteRefId = 0;
            Int64 investPunjab_AppId = 0;
            Int64 iPin = 0;
            Int64 appRefId = 0;
            Int64 entityKeyId = 0;
            int projectSiteVersion = 0;
            if (propertyFinderResp.HasPropName)
            {
                Enum.TryParse(propertyFinderResp.PropValue, out toDoActivityCategoryType);
            }
            else
            {
                is_preRequisites_set_1_meets = false;
            }

            if (toDoActivityCategoryType != ToDoActivityCategoryTypeEnum.APPLICATION_FORM_LOCK)
            {
                propertyFinderResp = ReflectionOps.GeValueByPropName(requestData, "ApplicationType");
                if (propertyFinderResp.HasPropName)
                {
                    Enum.TryParse(propertyFinderResp.PropValue, out applicationType);
                }
                else
                {
                    is_preRequisites_set_1_meets = false;
                }
            }

            if (toDoActivityCategoryType != ToDoActivityCategoryTypeEnum.APPLICATION_FORM_LOCK)
            {
                propertyFinderResp = ReflectionOps.GeValueByPropName(requestData, "ApplicationPurposeType");
                if (propertyFinderResp.HasPropName)
                {
                    Enum.TryParse(propertyFinderResp.PropValue, out applicationPurposeType);
                }
                else
                {
                    is_preRequisites_set_1_meets = false;
                }
            }
            propertyFinderResp = ReflectionOps.GeValueByPropName(requestData, "ToDoActivityModeType");
            ToDoActivityModeTypeEnum toDoActivityModeType = ToDoActivityModeTypeEnum.DEFAULT;
            if (propertyFinderResp.HasPropName)
            {
                Enum.TryParse(propertyFinderResp.PropValue, out toDoActivityModeType);
            }
            else
            {
                is_preRequisites_set_1_meets = false;
            }

            propertyFinderResp = ReflectionOps.GeValueByPropName(requestData, "AppRefId");
            if (propertyFinderResp.HasPropName)
            {
                appRefId = Convert.ToInt64(propertyFinderResp.PropValue);
            }
            else
            {
                is_preRequisites_set_1_meets = false;
            }

            if (toDoActivityCategoryType != ToDoActivityCategoryTypeEnum.APPLICATION_FORM_LOCK)
            {
                propertyFinderResp = ReflectionOps.GeValueByPropName(requestData, "InvestPunjab_AppId");

                if (propertyFinderResp.HasPropName)
                {
                    investPunjab_AppId = Convert.ToInt64(propertyFinderResp.PropValue);
                }
                else
                {
                    is_preRequisites_set_1_meets = false;
                }
            }

            if (toDoActivityCategoryType != ToDoActivityCategoryTypeEnum.APPLICATION_FORM_LOCK)
            {
                propertyFinderResp = ReflectionOps.GeValueByPropName(requestData, "IPin");

                if (propertyFinderResp.HasPropName)
                {
                    iPin = Convert.ToInt64(propertyFinderResp.PropValue);
                }
                else
                {
                    is_preRequisites_set_1_meets = false;
                }
            }

            if (toDoActivityCategoryType != ToDoActivityCategoryTypeEnum.APPLICATION_FORM_LOCK)
            {
                propertyFinderResp = ReflectionOps.GeValueByPropName(requestData, "ProjectSiteRefId");

                if (propertyFinderResp.HasPropName)
                {
                    projectSiteRefId = Convert.ToInt64(propertyFinderResp.PropValue);
                }
                else
                {
                    is_preRequisites_set_1_meets = false;
                }
            }

            if (toDoActivityCategoryType != ToDoActivityCategoryTypeEnum.APPLICATION_FORM_LOCK)
            {
                propertyFinderResp = ReflectionOps.GeValueByPropName(requestData, "ProjectSiteVersion");

                if (propertyFinderResp.HasPropName)
                {
                    projectSiteVersion = Convert.ToInt32(propertyFinderResp.PropValue);
                }
                else
                {
                    is_preRequisites_set_1_meets = false;
                }
            }

            propertyFinderResp = ReflectionOps.GeValueByPropName(requestData, "RootActivityRefId");
            string rootActivityRefId = "";
            if (propertyFinderResp.HasPropName)
            {
                rootActivityRefId = propertyFinderResp.PropValue.ToString();
                if (rootActivityRefId == "")
                {
                    rootActivityRefId = DateTimeOffset.Now.ToUnixTimeSeconds().ToString();
                }
            }
            else
            {
                rootActivityRefId = DateTimeOffset.Now.ToUnixTimeSeconds().ToString();
                //is_preRequisites_set_1_meets = false;
            }

            ApplicationTypeEnum applicationTypeClone = applicationType;
            ApplicationPurposeTypeEnum applicationPurposeTypeClone = applicationPurposeType;

            if (appRefId > 0)
            {
                var application = await _iToDoManagerService.GetApplication(appRefId);
                applicationType = (toDoActivityCategoryType == ToDoActivityCategoryTypeEnum.APPLICATION_FORM_LOCK) ? 0 : application.ResponseDataModel.ApplicationType;
                applicationPurposeType = (toDoActivityCategoryType == ToDoActivityCategoryTypeEnum.APPLICATION_FORM_LOCK) ? 0 : application.ResponseDataModel.ApplicationPurposeType;
                investPunjab_AppId = application.ResponseDataModel.InvestPunjab_AppId;
                iPin = Convert.ToInt64(application.ResponseDataModel.InvestPunjab_Ipin);
                projectSiteRefId = application.ResponseDataModel.ProjectSiteRefId;
                projectSiteVersion = application.ResponseDataModel.ProjectSiteVersion;

                applicationTypeClone = application.ResponseDataModel.ApplicationType;
                applicationPurposeTypeClone = application.ResponseDataModel.ApplicationPurposeType;
            }

            if (is_preRequisites_set_1_meets)
            {
                List<ToDoApplicationActivityMaping> toDoActivityList = await _iToDoManagerService.GetToDoApplicationActivityMapingList(applicationType, applicationPurposeType, toDoActivityModeType, toDoActivityCategoryType);
                bool isAnyStepHasException = false;
                foreach (var toDoActivity in toDoActivityList)
                {
                    string failedMessage = "";
                    ToDoActivityCompleteTypeEnum toDoActivityCompleteType = ToDoActivityCompleteTypeEnum.SUCCEEDED;
                    switch (toDoActivity.ToDoCodeType)
                    {
                        case ToDoCodeTypeEnum.SHARE_STATUS_WITH_INVEST_PUNJAB:
                            try
                            {

                                if (appRefId == 0)
                                {
                                    failedMessage = "Activity SHARE_STATUS_WITH_INVEST_PUNJAB: AppRefId is not available";
                                    toDoActivityCompleteType = ToDoActivityCompleteTypeEnum.FAILED_MISSING_PARAMETERS;
                                    isAnyStepHasException = true;
                                }
                                else
                                {
                                    await _iToDoManagerService.ShareStatusWithInvestPunjab(appRefId, rootActivityRefId, toDoActivity.ToDoCodeType, toDoActivityModeType);
                                    failedMessage = "";
                                    toDoActivityCompleteType = ToDoActivityCompleteTypeEnum.SUCCEEDED;
                                }

                            }
                            catch (Exception ex)
                            {
                                failedMessage = "Activity SHARE_STATUS_WITH_INVEST_PUNJAB: " + JsonConvert.SerializeObject(ex);
                                toDoActivityCompleteType = ToDoActivityCompleteTypeEnum.FAILED_WITH_ERRORS;
                                isAnyStepHasException = true;
                                throw ex;
                            }
                            finally
                            {
                                await _iToDoManagerService.CreateActivityLog(new ToDoActivityLog()
                                {
                                    ActivityFailedMessage = failedMessage,
                                    ApplicationPurposeType = applicationPurposeTypeClone,
                                    ApplicationType = applicationTypeClone,
                                    AppRefId = appRefId,
                                    RootActivityRefId = rootActivityRefId,
                                    TimeStemp = DateTime.Now,
                                    ToDoActivityCompleteType = toDoActivityCompleteType,
                                    ToDoActivityMapingVersion = toDoActivity.Version,
                                    ToDoActivityModeType = toDoActivityModeType,
                                    ToDoCodeType = toDoActivity.ToDoCodeType,
                                    UserRefId = user.Id,
                                    InvestPunjab_AppId = investPunjab_AppId,
                                    InvestPunjab_Ipin = iPin,
                                    ToDoActivityCategoryType = toDoActivityCategoryType
                                });
                            }
                            break;

                        case ToDoCodeTypeEnum.SET_ISDIRTY_FLAG:
                            try
                            {
                                if (appRefId == 0)
                                {
                                    failedMessage = "Activity SET_ISDIRTY_FLAG: appRefId=" + appRefId.ToString() + " is not available";
                                    toDoActivityCompleteType = ToDoActivityCompleteTypeEnum.FAILED_MISSING_PARAMETERS;
                                    isAnyStepHasException = true;
                                }
                                else
                                {
                                    await _iToDoManagerService.SetIsDirtyFlag(appRefId, true, rootActivityRefId, toDoActivity.ToDoCodeType);
                                    failedMessage = "";
                                    toDoActivityCompleteType = ToDoActivityCompleteTypeEnum.SUCCEEDED;
                                }
                            }
                            catch (Exception ex)
                            {
                                failedMessage = "Activity SET_ISDIRTY_FLAG: " + JsonConvert.SerializeObject(ex);
                                toDoActivityCompleteType = ToDoActivityCompleteTypeEnum.FAILED_WITH_ERRORS;
                                isAnyStepHasException = true;
                                throw ex;
                            }
                            finally
                            {
                                await _iToDoManagerService.CreateActivityLog(new ToDoActivityLog()
                                {
                                    ActivityFailedMessage = failedMessage,
                                    ApplicationPurposeType = applicationPurposeTypeClone,
                                    ApplicationType = applicationTypeClone,
                                    AppRefId = appRefId,
                                    RootActivityRefId = rootActivityRefId,
                                    TimeStemp = DateTime.Now,
                                    ToDoActivityCompleteType = toDoActivityCompleteType,
                                    ToDoActivityMapingVersion = toDoActivity.Version,
                                    ToDoActivityModeType = toDoActivityModeType,
                                    ToDoCodeType = toDoActivity.ToDoCodeType,
                                    UserRefId = user.Id,
                                    InvestPunjab_AppId = investPunjab_AppId,
                                    InvestPunjab_Ipin = iPin,
                                    ToDoActivityCategoryType = toDoActivityCategoryType
                                });
                            }
                            break;

                        case ToDoCodeTypeEnum.SET_APPLICATION_LIFECYCLE:
                            try
                            {
                                if (appRefId == 0)
                                {
                                    failedMessage = "Activity SET_APPLICATION_LIFECYCLE: appRefId=" + appRefId.ToString() + " is not available";
                                    toDoActivityCompleteType = ToDoActivityCompleteTypeEnum.FAILED_MISSING_PARAMETERS;
                                    isAnyStepHasException = true;
                                }
                                else
                                {
                                    ApplicationLifeCycleStatusTypeEnum appLifeCycleStatusType = ApplicationLifeCycleStatusTypeEnum.NOT_SUBMITTED;
                                    if (applicationTypeClone == ApplicationTypeEnum.SHOP_LICENCE)
                                    {
                                        appLifeCycleStatusType = ApplicationLifeCycleStatusTypeEnum.IN_PROCESS;
                                    }

                                    if (toDoActivityCategoryType == ToDoActivityCategoryTypeEnum.WITHDRAW_APPLICATION)
                                    {
                                        appLifeCycleStatusType = ApplicationLifeCycleStatusTypeEnum.WITHDRAW_APPLICATION;
                                    }
                                    await _iToDoManagerService.SetApplicationLifeCycle(appRefId, appLifeCycleStatusType, rootActivityRefId, toDoActivity.ToDoCodeType);
                                    failedMessage = "";
                                    toDoActivityCompleteType = ToDoActivityCompleteTypeEnum.SUCCEEDED;
                                }
                            }
                            catch (Exception ex)
                            {
                                failedMessage = "Activity SET_APPLICATION_LIFECYCLE: " + JsonConvert.SerializeObject(ex);
                                toDoActivityCompleteType = ToDoActivityCompleteTypeEnum.FAILED_WITH_ERRORS;
                                isAnyStepHasException = true;
                                throw ex;
                            }
                            finally
                            {
                                await _iToDoManagerService.CreateActivityLog(new ToDoActivityLog()
                                {
                                    ActivityFailedMessage = failedMessage,
                                    ApplicationPurposeType = applicationPurposeTypeClone,
                                    ApplicationType = applicationTypeClone,
                                    AppRefId = appRefId,
                                    RootActivityRefId = rootActivityRefId,
                                    TimeStemp = DateTime.Now,
                                    ToDoActivityCompleteType = toDoActivityCompleteType,
                                    ToDoActivityMapingVersion = toDoActivity.Version,
                                    ToDoActivityModeType = toDoActivityModeType,
                                    ToDoCodeType = toDoActivity.ToDoCodeType,
                                    UserRefId = user.Id,
                                    InvestPunjab_AppId = investPunjab_AppId,
                                    InvestPunjab_Ipin = iPin,
                                    ToDoActivityCategoryType = toDoActivityCategoryType
                                });
                            }
                            break;

                        case ToDoCodeTypeEnum.SEND_NOTIFICATION:
                            try
                            {
                                if (appRefId == 0)
                                {
                                    failedMessage = "Activity SEND_NOTIFICATION: appRefId=" + appRefId.ToString() + " is not available";
                                    toDoActivityCompleteType = ToDoActivityCompleteTypeEnum.FAILED_MISSING_PARAMETERS;
                                    isAnyStepHasException = true;
                                }
                                else
                                {
                                    await _iToDoManagerService.SEND_NOTIFICATION(appRefId, rootActivityRefId, toDoActivity.ToDoCodeType);
                                    failedMessage = "";
                                    toDoActivityCompleteType = ToDoActivityCompleteTypeEnum.SUCCEEDED;
                                }
                            }
                            catch (Exception ex)
                            {
                                failedMessage = "Activity SEND_NOTIFICATION: " + JsonConvert.SerializeObject(ex);
                                toDoActivityCompleteType = ToDoActivityCompleteTypeEnum.FAILED_WITH_ERRORS;
                                isAnyStepHasException = true;
                                throw ex;
                            }
                            finally
                            {
                                await _iToDoManagerService.CreateActivityLog(new ToDoActivityLog()
                                {
                                    ActivityFailedMessage = failedMessage,
                                    ApplicationPurposeType = applicationPurposeTypeClone,
                                    ApplicationType = applicationTypeClone,
                                    AppRefId = appRefId,
                                    RootActivityRefId = rootActivityRefId,
                                    TimeStemp = DateTime.Now,
                                    ToDoActivityCompleteType = toDoActivityCompleteType,
                                    ToDoActivityMapingVersion = toDoActivity.Version,
                                    ToDoActivityModeType = toDoActivityModeType,
                                    ToDoCodeType = toDoActivity.ToDoCodeType,
                                    UserRefId = user.Id,
                                    InvestPunjab_AppId = investPunjab_AppId,
                                    InvestPunjab_Ipin = iPin,
                                    ToDoActivityCategoryType = toDoActivityCategoryType
                                });
                            }
                            break;

                        case ToDoCodeTypeEnum.ADD_PAYMENT_RAISED_FEE:
                            try
                            {
                                if (appRefId == 0)
                                {
                                    failedMessage = "Activity ADD_PAYMENT_RAISED_FEE: appRefId=" + appRefId.ToString() + " is not available";
                                    toDoActivityCompleteType = ToDoActivityCompleteTypeEnum.FAILED_MISSING_PARAMETERS;
                                    isAnyStepHasException = true;
                                }
                                else
                                {
                                    await _iToDoManagerService.ADD_PAYMENT_RAISED_FEE(requestData, appRefId, rootActivityRefId, toDoActivity.ToDoCodeType);
                                    failedMessage = "";
                                    toDoActivityCompleteType = ToDoActivityCompleteTypeEnum.SUCCEEDED;
                                }
                            }
                            catch (Exception ex)
                            {
                                failedMessage = "Activity ADD_PAYMENT_RAISED_FEE: " + JsonConvert.SerializeObject(ex);
                                toDoActivityCompleteType = ToDoActivityCompleteTypeEnum.FAILED_WITH_ERRORS;
                                isAnyStepHasException = true;
                                throw ex;
                            }
                            finally
                            {
                                await _iToDoManagerService.CreateActivityLog(new ToDoActivityLog()
                                {
                                    ActivityFailedMessage = failedMessage,
                                    ApplicationPurposeType = applicationPurposeType,
                                    ApplicationType = applicationType,
                                    AppRefId = appRefId,
                                    RootActivityRefId = rootActivityRefId,
                                    TimeStemp = DateTime.Now,
                                    ToDoActivityCompleteType = toDoActivityCompleteType,
                                    ToDoActivityMapingVersion = toDoActivity.Version,
                                    ToDoActivityModeType = toDoActivityModeType,
                                    ToDoCodeType = toDoActivity.ToDoCodeType,
                                    UserRefId = user.Id,
                                    InvestPunjab_AppId = investPunjab_AppId,
                                    InvestPunjab_Ipin = iPin,
                                    ToDoActivityCategoryType = toDoActivityCategoryType
                                });
                            }
                            break;

                        case ToDoCodeTypeEnum.INCREASE_PAYMENT_BATCH_COUNTER:
                            try
                            {
                                if (appRefId == 0)
                                {
                                    failedMessage = "Activity INCREASE_PAYMENT_BATCH_COUNTER: appRefId=" + appRefId.ToString() + " is not available";
                                    toDoActivityCompleteType = ToDoActivityCompleteTypeEnum.FAILED_MISSING_PARAMETERS;
                                    isAnyStepHasException = true;
                                }
                                else
                                {
                                    await _iToDoManagerService.INCREASE_PAYMENT_BATCH_COUNTER(appRefId, rootActivityRefId, toDoActivity.ToDoCodeType);
                                    failedMessage = "";
                                    toDoActivityCompleteType = ToDoActivityCompleteTypeEnum.SUCCEEDED;
                                }
                            }
                            catch (Exception ex)
                            {
                                failedMessage = "Activity INCREASE_PAYMENT_BATCH_COUNTER: " + JsonConvert.SerializeObject(ex);
                                toDoActivityCompleteType = ToDoActivityCompleteTypeEnum.FAILED_WITH_ERRORS;
                                isAnyStepHasException = true;
                                throw ex;
                            }
                            finally
                            {
                                await _iToDoManagerService.CreateActivityLog(new ToDoActivityLog()
                                {
                                    ActivityFailedMessage = failedMessage,
                                    ApplicationPurposeType = applicationPurposeType,
                                    ApplicationType = applicationType,
                                    AppRefId = appRefId,
                                    RootActivityRefId = rootActivityRefId,
                                    TimeStemp = DateTime.Now,
                                    ToDoActivityCompleteType = toDoActivityCompleteType,
                                    ToDoActivityMapingVersion = toDoActivity.Version,
                                    ToDoActivityModeType = toDoActivityModeType,
                                    ToDoCodeType = toDoActivity.ToDoCodeType,
                                    UserRefId = user.Id,
                                    InvestPunjab_AppId = investPunjab_AppId,
                                    InvestPunjab_Ipin = iPin,
                                    ToDoActivityCategoryType = toDoActivityCategoryType
                                });
                            }
                            break;

                        case ToDoCodeTypeEnum.SET_ACTION_LOG_ID:
                            try
                            {
                                if (appRefId == 0)
                                {
                                    failedMessage = "Activity SET_ACTION_LOG_ID: appRefId=" + appRefId.ToString() + " is not available";
                                    toDoActivityCompleteType = ToDoActivityCompleteTypeEnum.FAILED_MISSING_PARAMETERS;
                                    isAnyStepHasException = true;
                                }
                                else
                                {
                                    await _iToDoManagerService.SET_ACTION_LOG_ID(appRefId, rootActivityRefId, toDoActivity.ToDoCodeType);
                                    failedMessage = "";
                                    toDoActivityCompleteType = ToDoActivityCompleteTypeEnum.SUCCEEDED;
                                }
                            }
                            catch (Exception ex)
                            {
                                failedMessage = "Activity SET_ACTION_LOG_ID: " + JsonConvert.SerializeObject(ex);
                                toDoActivityCompleteType = ToDoActivityCompleteTypeEnum.FAILED_WITH_ERRORS;
                                isAnyStepHasException = true;
                                throw ex;
                            }
                            finally
                            {
                                await _iToDoManagerService.CreateActivityLog(new ToDoActivityLog()
                                {
                                    ActivityFailedMessage = failedMessage,
                                    ApplicationPurposeType = applicationPurposeType,
                                    ApplicationType = applicationType,
                                    AppRefId = appRefId,
                                    RootActivityRefId = rootActivityRefId,
                                    TimeStemp = DateTime.Now,
                                    ToDoActivityCompleteType = toDoActivityCompleteType,
                                    ToDoActivityMapingVersion = toDoActivity.Version,
                                    ToDoActivityModeType = toDoActivityModeType,
                                    ToDoCodeType = toDoActivity.ToDoCodeType,
                                    UserRefId = user.Id,
                                    InvestPunjab_AppId = investPunjab_AppId,
                                    InvestPunjab_Ipin = iPin,
                                    ToDoActivityCategoryType = toDoActivityCategoryType
                                });
                            }
                            break;

                        case ToDoCodeTypeEnum.UPDATE_PAYMENT_RAISED_FEE:
                            try
                            {
                                if (appRefId == 0)
                                {
                                    failedMessage = "Activity UPDATE_PAYMENT_RAISED_FEE: appRefId=" + appRefId.ToString() + " is not available";
                                    toDoActivityCompleteType = ToDoActivityCompleteTypeEnum.FAILED_MISSING_PARAMETERS;
                                    isAnyStepHasException = true;
                                }
                                else
                                {
                                    await _iToDoManagerService.UPDATE_PAYMENT_RAISED_FEE(requestData, rootActivityRefId, toDoActivity.ToDoCodeType);
                                    failedMessage = "";
                                    toDoActivityCompleteType = ToDoActivityCompleteTypeEnum.SUCCEEDED;
                                }
                            }
                            catch (Exception ex)
                            {
                                failedMessage = "Activity UPDATE_PAYMENT_RAISED_FEE: " + JsonConvert.SerializeObject(ex);
                                toDoActivityCompleteType = ToDoActivityCompleteTypeEnum.FAILED_WITH_ERRORS;
                                isAnyStepHasException = true;
                                throw ex;
                            }
                            finally
                            {
                                await _iToDoManagerService.CreateActivityLog(new ToDoActivityLog()
                                {
                                    ActivityFailedMessage = failedMessage,
                                    ApplicationPurposeType = applicationPurposeType,
                                    ApplicationType = applicationType,
                                    AppRefId = appRefId,
                                    RootActivityRefId = rootActivityRefId,
                                    TimeStemp = DateTime.Now,
                                    ToDoActivityCompleteType = toDoActivityCompleteType,
                                    ToDoActivityMapingVersion = toDoActivity.Version,
                                    ToDoActivityModeType = toDoActivityModeType,
                                    ToDoCodeType = toDoActivity.ToDoCodeType,
                                    UserRefId = user.Id,
                                    InvestPunjab_AppId = investPunjab_AppId,
                                    InvestPunjab_Ipin = iPin,
                                    ToDoActivityCategoryType = toDoActivityCategoryType
                                });
                            }
                            break;
                    }
                }
                if (isAnyStepHasException)
                {
                    resp.HasExceptions = true;
                    resp.ErrorDesc = "Operation Completed With Errors";
                    resp.RootActivityRefId = rootActivityRefId;
                    await _iToDoManagerService.RaiseTicket(user.Id, rootActivityRefId, applicationTypeClone, applicationPurposeTypeClone, toDoActivityCategoryType, iPin, investPunjab_AppId);

                }
                else
                {
                    resp.AppId = appRefId;
                    resp.EntityKeyId = entityKeyId;
                    resp.HasExceptions = false;
                    resp.ErrorDesc = "Operation Completed";
                }
            }
            else
            {
                resp.HasExceptions = true;
                resp.ErrorDesc = "Failed in PreRequisites SET-1";
            }
            return resp;
        }

        public async Task<GenericResponseTemplateModel<FactoryHasTempRegistrationFlagViewModel>> IsFactoryHasTempRegistrationFlag(object requestData)
        {
            GenericResponseTemplateModel<FactoryHasTempRegistrationFlagViewModel> genericServiceResultTemplate = new GenericResponseTemplateModel<FactoryHasTempRegistrationFlagViewModel>();
            genericServiceResultTemplate.ResponseDataModel = new FactoryHasTempRegistrationFlagViewModel();
            PropertyFinderRespViewModel propertyFinderResp = ReflectionOps.GeValueByPropName(requestData, "ToDoActivityCategoryType");
            propertyFinderResp = ReflectionOps.GeValueByPropName(requestData, "IsTempRegistered");
            if (propertyFinderResp.HasPropName)
            {
                genericServiceResultTemplate.ResponseDataModel.IsTempRegistered = Convert.ToInt32(propertyFinderResp.PropValue);
            }
            propertyFinderResp = ReflectionOps.GeValueByPropName(requestData, "IsTempRegistrationVerified");
            if (propertyFinderResp.HasPropName)
            {
                genericServiceResultTemplate.ResponseDataModel.isTempRegistrationVerified = Convert.ToBoolean(propertyFinderResp.PropValue);
            }
            return genericServiceResultTemplate;
        }
    }
}
