using Dapper;
using pbsamadhannetcoreapi.Models;
using pbsamadhannetcoreapi.Repositories.Implementations;
using pbsamadhannetcoreapi.Services.Implementations;
using pbsamadhannetcoreapi.ViewModels;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace pbsamadhannetcoreapi.Services
{
    public class System_O_CommunicationService : ISystem_O_CommunicationService
    {
        private readonly IDapperRepository _iDapperRepository;

        public System_O_CommunicationService(IDapperRepository iDapperRepository)
        {
            _iDapperRepository = iDapperRepository;
        }

        public async Task<GenericResponseTemplateModel<bool>> HasServiceAlreadyApplied(long tokenNumber, int appFormId)
        {
            GenericResponseTemplateModel<bool> genericServiceResultTemplate = new GenericResponseTemplateModel<bool>() { HasError = false, ErrorDesc = null, ResponseDataModel = false };
            try
            {
                var licence = await _iDapperRepository.Get<Int32>("SELECT COUNT(*) FROM AppPBIPActions WHERE TokenNumber = @tokenNumber AND AppFormId = @appFormId AND StatusId NOT IN (5 , 210 )", new { TokenNumber = tokenNumber.ToString(), AppFormId = appFormId }, CommandType.Text).ConfigureAwait(false);
                genericServiceResultTemplate.ResponseDataModel = (licence[0] == 1);
            }
            catch (Exception ex)
            {
                genericServiceResultTemplate.HasError = true;
                genericServiceResultTemplate.ErrorDesc = ex.Message;
            }
            return genericServiceResultTemplate;
        }

        public async Task<GenericResponseTemplateModel<bool>> IsServiceAlreadyInObjection(long tokenNumber, int appFormId)
        {
            GenericResponseTemplateModel<bool> genericServiceResultTemplate = new GenericResponseTemplateModel<bool>() { HasError = false, ErrorDesc = null, ResponseDataModel = false };
            try
            {
                var licence = await _iDapperRepository.Get<Int32>("SELECT COUNT(*) FROM AppPBIPActions WHERE StatusId = 9 AND TokenNumber = @tokenNumber AND AppFormId = @appFormId", new { StatusId = 9, TokenNumber = tokenNumber, AppFormId = appFormId }, CommandType.Text).ConfigureAwait(false);
                genericServiceResultTemplate.ResponseDataModel = (licence[0] == 1);

            }
            catch (Exception ex)
            {
                genericServiceResultTemplate.HasError = true;
                genericServiceResultTemplate.ErrorDesc = ex.Message;
            }
            return genericServiceResultTemplate;
        }

        public async Task<GenericResponseTemplateModel<bool>> IsServiceAlreadyInProcess(long tokenNumber, int appFormId)
        {
            GenericResponseTemplateModel<bool> genericServiceResultTemplate = new GenericResponseTemplateModel<bool>() { HasError = false, ErrorDesc = null, ResponseDataModel = false };
            try
            {
                var licence = await _iDapperRepository.Get<Int32>("SELECT COUNT(*) FROM AppPBIPActions WHERE TokenNumber = @tokenNumber AND AppFormId = @appFormId AND StatusId NOT IN (1,5,15,210,9)", new { TokenNumber = tokenNumber, AppFormId = appFormId }, CommandType.Text).ConfigureAwait(false);
                genericServiceResultTemplate.ResponseDataModel = (licence[0] == 1);
            }
            catch (Exception ex)
            {
                genericServiceResultTemplate.HasError = true;
                genericServiceResultTemplate.ErrorDesc = ex.Message;
            }
            return genericServiceResultTemplate;
        }
        public async Task<GenericResponseTemplateModel<Int64>> DoesIpinHasNativeUser(long iPin)
        {
            GenericResponseTemplateModel<Int64> genericServiceResultTemplate = new GenericResponseTemplateModel<Int64>() { HasError = false, ErrorDesc = null, ResponseDataModel = 0 };
            try
            {
                var licence = await _iDapperRepository.Get<Int64>("SELECT TOP 1 AppId FROM Applications WHERE TokenNumber =CONVERT(varchar, @IPin)", new { IPin = iPin }, CommandType.Text).ConfigureAwait(false);
                if(licence.Count() > 0)
                {
                    genericServiceResultTemplate.ResponseDataModel = licence[0];
                }
                else
                {
                    licence = await _iDapperRepository.Get<Int64>("SELECT TOP 1 E_LAB_AppId FROM BF_Integration_Addi_Info WHERE iPin = CONVERT(varchar,@IPin) AND E_LAB_AppId IS NOT NULL", new { IPin = iPin }, CommandType.Text).ConfigureAwait(false);
                    if (licence.Count() > 0)
                    {
                        genericServiceResultTemplate.ResponseDataModel = licence[0];
                    }
                }
            }
            catch (Exception ex)
            {
                genericServiceResultTemplate.HasError = true;
                genericServiceResultTemplate.ErrorDesc = ex.Message;
            }
            return genericServiceResultTemplate;
        }

        public async Task<GenericResponseTemplateModel<bool>> CreateServiceMigrationLog(Int64 appId, Int64 appFormId, string nar)
        {
            GenericResponseTemplateModel<bool> genericServiceResultTemplate = new GenericResponseTemplateModel<bool>() { HasError = false, ErrorDesc = null, ResponseDataModel = false };
            try
            {
                var licence = await _iDapperRepository.AddupdateData("INSERT INTO ServiceMigrationDataLogs (AppId, AppFormId, NAR, MigrationDate) VALUES(@AppId,@AppFormId,@NAR, @MigrationDate)", new { AppId = appId, AppFormId = appFormId, NAR = nar, MigrationDate = DateTime.Now }, CommandType.Text).ConfigureAwait(false);
                genericServiceResultTemplate.ResponseDataModel = true;
            }
            catch (Exception ex)
            {
                genericServiceResultTemplate.HasError = true;
                genericServiceResultTemplate.ErrorDesc = ex.Message;
            }
            return genericServiceResultTemplate;
        }

        public async Task<GenericResponseTemplateModel<List<ShopLicence_EmployeeDetailsViewModel>>> IsHavingEmployee(Int64 appId)
        {
            GenericResponseTemplateModel<List<ShopLicence_EmployeeDetailsViewModel>> genericServiceResultTemplate = new GenericResponseTemplateModel<List<ShopLicence_EmployeeDetailsViewModel>>() { HasError = false, ErrorDesc = null, ResponseDataModel = null};
            try
            {
                var employeesDetails = await _iDapperRepository.Get<ShopLicence_EmployeeDetailsViewModel>("SELECT * FROM Employees WHERE AppId =@appId", new { AppId = appId }, CommandType.Text).ConfigureAwait(false);
                genericServiceResultTemplate.ResponseDataModel = employeesDetails.ToList();
            }
            catch (Exception ex)
            {
                genericServiceResultTemplate.HasError = true;
                genericServiceResultTemplate.ErrorDesc = ex.Message;
            }
            return genericServiceResultTemplate;
        }

        public async Task<GenericResponseTemplateModel<Sys_O_AppId_TokenInfoViewModel>> DoesOldLicenceNoExists(string licenceNo, ApplicationTypeEnum applicationType)
        {
            GenericResponseTemplateModel<Sys_O_AppId_TokenInfoViewModel> genericServiceResultTemplate = new GenericResponseTemplateModel<Sys_O_AppId_TokenInfoViewModel>() { HasError = false, ErrorDesc = null, ResponseDataModel = null };
            try
            {
                var licence = await _iDapperRepository.Get<Sys_O_AppId_TokenInfoViewModel>("SELECT AP.TokenNumber, AP.AppId FROM AppClearanceIssueds AC INNER JOIN AppPBIPActions AP ON AC.AppId = AP.AppId WHERE AC.LicenceNo = @licenceNo", new { LicenceNo = licenceNo }, CommandType.Text).ConfigureAwait(false);

                if(licence.Count() == 0)
                {
                    if(applicationType == ApplicationTypeEnum.FACTORY_LICENCE)
                    {
                        licence = await _iDapperRepository.Get<Sys_O_AppId_TokenInfoViewModel>("SELECT AP.TokenNumber, AP.AppId FROM FactoryLicenses AC INNER JOIN AppPBIPActions AP ON AC.AppId = AP.AppId WHERE AC.FactoryLicenseNumber = @licenceNo", new { LicenceNo = licenceNo }, CommandType.Text).ConfigureAwait(false);
                    }
                }


                if (licence.Count() > 0)
                {
                    genericServiceResultTemplate.ResponseDataModel = licence[0];
                }
            }
            catch (Exception ex)
            {
                genericServiceResultTemplate.HasError = true;
                genericServiceResultTemplate.ErrorDesc = ex.Message;
            }
            return genericServiceResultTemplate;
        }

        public async Task<GenericResponseTemplateModel<Sys_O_AppId_TokenInfoViewModel>> GetLegacyAppIdFromIPin(Int64 investPunjab_Ipin, Int64 investPunjab_AppId, ApplicationTypeEnum applicationType)
        {
            GenericResponseTemplateModel<Sys_O_AppId_TokenInfoViewModel> genericServiceResultTemplate = new GenericResponseTemplateModel<Sys_O_AppId_TokenInfoViewModel>() { HasError = false, ErrorDesc = null, ResponseDataModel = null };
            try
            {
                var licence = await _iDapperRepository.Get<Sys_O_AppId_TokenInfoViewModel>("SELECT TOP 1 AC.AppId,AC.TokenNumber from BF_Integration_Addi_Info BF INNER JOIN AppPBIPActions AC ON BF.E_LAB_AppId = AC.AppId WHERE BF.iPin = @IPin and BF.AppId = @AppId ORDER BY AC.TDATE DESC", new { IPin = investPunjab_Ipin, AppId = investPunjab_AppId }, CommandType.Text).ConfigureAwait(false);

                if (licence.Count() > 0)
                {
                    genericServiceResultTemplate.ResponseDataModel = licence[0];
                }
            }

            catch (Exception ex)
            {
                genericServiceResultTemplate.HasError = true;
                genericServiceResultTemplate.ErrorDesc = ex.Message;
            }
            return genericServiceResultTemplate;
        }

        public async Task<GenericResponseTemplateModel<bool>> DeRegisterFactoryByFactoryBacklog(DeRegsiterFactoryRequestViewModel deRegsiterFactoryRequest, string fileName)
        {
            GenericResponseTemplateModel<bool> genericServiceResultTemplate = new GenericResponseTemplateModel<bool>() { HasError = false, ErrorDesc = null, ResponseDataModel = false };
            try
            {
                var licence = await _iDapperRepository.AddupdateData("[dbo].[sp_Dashboard_DeRegisterFactory]", new { AppId = deRegsiterFactoryRequest.appId, AppFormId = deRegsiterFactoryRequest.appFormId, NAR = deRegsiterFactoryRequest.nar, SenderId = deRegsiterFactoryRequest.id, Remarks = deRegsiterFactoryRequest.remarks, DeRegNo = deRegsiterFactoryRequest.deregistrationnumber, applicantuserid = deRegsiterFactoryRequest.applicantuserid, applicantprofileid = deRegsiterFactoryRequest.applicantprofileid, FileName = fileName }, CommandType.StoredProcedure);
            }
            catch (Exception ex)
            {
                genericServiceResultTemplate.HasError = true;
                genericServiceResultTemplate.ErrorDesc = ex.Message;
            }
            return genericServiceResultTemplate;
        }


        public async Task<GenericResponseTemplateModel<Sys_O_AppId_OldDataViewModel>> OldDataAppClerance(string licenceNo)
        {
            GenericResponseTemplateModel<Sys_O_AppId_OldDataViewModel> genericServiceResultTemplate = new GenericResponseTemplateModel<Sys_O_AppId_OldDataViewModel>() { HasError = false, ErrorDesc = null, ResponseDataModel = null };
            try
            {
                var employeesDetails = await _iDapperRepository.Get<Sys_O_AppId_OldDataViewModel>("SELECT * FROM AppClearanceIssueds WHERE LicenceNo =@LicenceNo", new { LicenceNo = licenceNo }, CommandType.Text).ConfigureAwait(false);
                genericServiceResultTemplate.ResponseDataModel = employeesDetails.FirstOrDefault();
            }
            catch (Exception ex)
            {
                genericServiceResultTemplate.HasError = true;
                genericServiceResultTemplate.ErrorDesc = ex.Message;
            }
            return genericServiceResultTemplate;
        }

        public async Task<GenericResponseTemplateModel<List<Licence_CL_PE_Contractor_ViewModel>>> GetContractorList(Int64 appId, Int64 appFormId, string nar)
        {
            GenericResponseTemplateModel<List<Licence_CL_PE_Contractor_ViewModel>> genericServiceResultTemplate = new GenericResponseTemplateModel<List<Licence_CL_PE_Contractor_ViewModel>>() { HasError = false, ErrorDesc = null, ResponseDataModel = null };
            try
            {
                var contractors = await _iDapperRepository.Get<Licence_CL_PE_Contractor_ViewModel>("SELECT * FROM EstRegContractors WHERE AppId = @AppId AND AppFormId = @AppFormId AND NAR = @NAR", new { AppId = appId, AppFormId = appFormId, NAR = nar, }, CommandType.Text).ConfigureAwait(false);
                genericServiceResultTemplate.ResponseDataModel = contractors.ToList();
            }
            catch (Exception ex)
            {
                genericServiceResultTemplate.HasError = true;
                genericServiceResultTemplate.ErrorDesc = ex.Message;
            }
            return genericServiceResultTemplate;
        }

        public async Task<GenericResponseTemplateModel<List<InterstatePEContractLabour_ViewModel>>> GetISMContractorList(Int64 appId, Int64 appFormId, string nar)
        {
            GenericResponseTemplateModel<List<InterstatePEContractLabour_ViewModel>> genericServiceResultTemplate = new GenericResponseTemplateModel<List<InterstatePEContractLabour_ViewModel>>() { HasError = false, ErrorDesc = null, ResponseDataModel = null };
            try
            {
                var contractors = await _iDapperRepository.Get<InterstatePEContractLabour_ViewModel>("SELECT * FROM InterstatePEContractLabours WHERE AppId = @AppId AND AppFormId = @AppFormId AND NAR = @NAR", new { AppId = appId, AppFormId = appFormId, NAR = nar, }, CommandType.Text).ConfigureAwait(false);
                genericServiceResultTemplate.ResponseDataModel = contractors.ToList();
            }
            catch (Exception ex)
            {
                genericServiceResultTemplate.HasError = true;
                genericServiceResultTemplate.ErrorDesc = ex.Message;
            }
            return genericServiceResultTemplate;
        }



        public async Task<GenericResponseTemplateModel<List<Licence_TradeUnion_Officer_ViewModel>>> GetTradeUnionOfficerList(Int64 tradeUnionId)
        {
            GenericResponseTemplateModel<List<Licence_TradeUnion_Officer_ViewModel>> genericServiceResultTemplate = new GenericResponseTemplateModel<List<Licence_TradeUnion_Officer_ViewModel>>() { HasError = false, ErrorDesc = null, ResponseDataModel = null };
            try
            {
                var officer = await _iDapperRepository.Get<Licence_TradeUnion_Officer_ViewModel>("SELECT * FROM TradeUnionOfficers WHERE TradeUnionId = @TradeUnionId", new { TradeUnionId = tradeUnionId }, CommandType.Text).ConfigureAwait(false);
                genericServiceResultTemplate.ResponseDataModel = officer.ToList();
            }
            catch (Exception ex)
            {
                genericServiceResultTemplate.HasError = true;
                genericServiceResultTemplate.ErrorDesc = ex.Message;
            }
            return genericServiceResultTemplate;
        }


    }
}
