using pbsamadhannetcoreapi.Models;
using pbsamadhannetcoreapi.Repositories.Implementations;
using pbsamadhannetcoreapi.Services.Implementations;
using System.Threading.Tasks;
using System;
using System.Linq;
using pbsamadhannetcoreapi.Repositories;
using pbsamadhannetcoreapi.ViewModels;
using System.Data;
using pbsamadhannetcoreapi.CommonUtiliteis;
using System.Collections.Generic;
using DocumentFormat.OpenXml.Office2010.Excel;
using Microsoft.Extensions.FileProviders;
using System.IO;

namespace pbsamadhannetcoreapi.Services
{
    public class MiscellaneousService : IMiscellaneousService
    {
        private readonly AppDbContext _context;
        private readonly IGenericRepository<Establishment_EPFO_Logs> _iGR_Establishment_EPFO;
        private readonly IGeneric_SP_Repository _iGeneric_SP_Repository;
        public MiscellaneousService(AppDbContext context, IGenericRepository<Establishment_EPFO_Logs> iGR_Establishment_EPFO, IGeneric_SP_Repository iGeneric_SP_Repository)
        {
            _context = context;
            _iGR_Establishment_EPFO = iGR_Establishment_EPFO;
            _iGeneric_SP_Repository = iGeneric_SP_Repository;
        }

        public async Task<GenericResponseTemplateModel<Establishment_EPFO>> GetEstablishmentEpfoDetails(string establishmentId)
        {
            GenericResponseTemplateModel<Establishment_EPFO> genericServiceResultTemplate = new GenericResponseTemplateModel<Establishment_EPFO>()
            {
                HasError = false,
                ErrorDesc = "",
            };
            try
            {
                genericServiceResultTemplate.ResponseDataModel = _context.Establishment_EPFO.Where(x => x.EstablishmentId == establishmentId).FirstOrDefault();
            }
            catch (Exception ex)
            {
                genericServiceResultTemplate.HasError = true;
                genericServiceResultTemplate.ErrorDesc = ex.Message;
            }
            return genericServiceResultTemplate;
        }

        public async Task<GenericResponseTemplateModel<List<Establishment_EPFO_Logs>>> GetEstablishmentEpfoLogDetailbyId(string establishmentRefId)
        {
            GenericResponseTemplateModel<List<Establishment_EPFO_Logs>> genericServiceResultTemplate = new GenericResponseTemplateModel<List<Establishment_EPFO_Logs>>()
            {
                HasError = false,
                ErrorDesc = "",
            };
            try
            {

                genericServiceResultTemplate.ResponseDataModel = _context.Establishment_EPFO_Logs.Where(x => x.EstablishmentRefId == establishmentRefId && x.HasSent == true).ToList();
            }
            catch (Exception ex)
            {
                genericServiceResultTemplate.HasError = true;
                genericServiceResultTemplate.ErrorDesc = ex.Message;
            }
            return genericServiceResultTemplate;
        }

        public async Task<GenericServiceResultTemplate> UpdateEstablishmentDataLogs(Establishment_EPFO_Logs formModel)
        {
            GenericServiceResultTemplate genericServiceResultTemplate = new GenericServiceResultTemplate();
            try
            {
                genericServiceResultTemplate.HasException = false;
                genericServiceResultTemplate.CustomeValidationResult = CustomeValidator<Establishment_EPFO_Logs>.ValidateModel_AllProperties(formModel);
                if (genericServiceResultTemplate.CustomeValidationResult.IsValid)
                {
                    var establishment_EPFO_Logs =  _context.Establishment_EPFO_Logs.Where(x => x.EstablishmentEPFOLogsId == formModel.EstablishmentEPFOLogsId).FirstOrDefault();
                    establishment_EPFO_Logs.SentRemarks = formModel.SentRemarks;
                    establishment_EPFO_Logs.SentDate = DateTime.Now;
                    establishment_EPFO_Logs.HasSent = true;
                    _context.SaveChanges();
                    
                    var establishmentEPFOLogsId = formModel.EstablishmentEPFOLogsId;
                }

                // Update ApplicationDisputeMapping Table. 
                var disputemapping = _context.ApplicationDisputeNoMapping.Where(x => x.EstablishmentId == formModel.EstablishmentRefId && x.EstablishmentEPFOLogsId == formModel.EstablishmentEPFOLogsId).ToList();
                foreach (var dispute in disputemapping)
                {
                    dispute.IsNotSent = 1;
                    dispute.SentDateTime = DateTime.Now;
                    _context.SaveChanges();
                }

                var tempFilePath = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "TempFiles")).Root;

                if (File.Exists(tempFilePath + formModel.PdfNameGUID + ".pdf"))
                {
                    var licenseDirName = "AppForm_Notice";
                    var licenceRootDirUrl = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "License")).Root.ToString();
                    Directory.CreateDirectory(licenceRootDirUrl + licenseDirName);
                    var licenseFilePath = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "License", licenseDirName)).Root;

                    if (File.Exists(licenseFilePath + formModel.EstablishmentRefId + formModel.EstablishmentEPFOLogsId + ".pdf"))
                    {
                        File.Delete(licenseFilePath + formModel.EstablishmentRefId + formModel.EstablishmentEPFOLogsId + ".pdf");
                    }
                    File.Move(tempFilePath + formModel.PdfNameGUID + ".pdf", licenseFilePath + formModel.EstablishmentRefId + formModel.EstablishmentEPFOLogsId + ".pdf");
                }

            }
            catch (Exception ex)
            {
                genericServiceResultTemplate.HasException = true;
                genericServiceResultTemplate.Exceptions = ex;
            }
            return genericServiceResultTemplate;
        }



        public async Task<GenericServiceResultTemplate> UpdateEstablishmentReamrks(Establishment_EPFO_Logs formModel)
        {
            GenericServiceResultTemplate genericServiceResultTemplate = new GenericServiceResultTemplate();
            try
            {
                genericServiceResultTemplate.HasException = false;
                genericServiceResultTemplate.CustomeValidationResult = CustomeValidator<Establishment_EPFO_Logs>.ValidateModel_AllProperties(formModel);
                if (genericServiceResultTemplate.CustomeValidationResult.IsValid)
                {
                    formModel = GenericModelOps<Establishment_EPFO_Logs>.SetNullAllNevigationProperties(formModel);
                    formModel.SentDate = DateTime.Now;
                    formModel.HasSent = true;
                    _iGR_Establishment_EPFO.Insert(formModel);
                    await _iGR_Establishment_EPFO.SavechangeAsync();
                    var establishmentEPFOLogsId = formModel.EstablishmentEPFOLogsId;
                }

            }
            catch (Exception ex)
            {
                genericServiceResultTemplate.HasException = true;
                genericServiceResultTemplate.Exceptions = ex;
            }
            return genericServiceResultTemplate;
        }

        public async Task<GenericResponseTemplateModel<List<Establishment_EPFO_Report>>> GetEstablishmentEPFOReport(string fromdate, string todate, string id, string searchCode, int pageNo, int pageSize, string sortColumn, string sortOrder, string filterArray)
        {
            GenericResponseTemplateModel<List<Establishment_EPFO_Report>> genericServiceResultTemplate = new GenericResponseTemplateModel<List<Establishment_EPFO_Report>>()
            {
                HasError = false,
                ErrorDesc = "",
            };
            try
            {
                List<StoreProcedureParm> storeProcedureParms = new List<StoreProcedureParm>();
                storeProcedureParms = new List<StoreProcedureParm>()
                {
                   new StoreProcedureParm (){ParmName="FromDate", ParmValue=fromdate.ToString(), isNumber=false},
                    new StoreProcedureParm (){ParmName="ToDate", ParmValue=todate.ToString(), isNumber=false},
                    new StoreProcedureParm (){ParmName="UserRefId", ParmValue=id.ToString(), isNumber=false},
                    new StoreProcedureParm (){ParmName="SearchCode",ParmValue= searchCode==null ? "": searchCode ,isNumber=false},
                    new StoreProcedureParm (){ ParmName="PageNo", ParmValue=pageNo.ToString() , isNumber=true},
                    new StoreProcedureParm (){ParmName="PageSize", ParmValue=pageSize.ToString() , isNumber=true},
                    new StoreProcedureParm (){ParmName="SortColumn", ParmValue=sortColumn.ToString(), isNumber=false},
                    new StoreProcedureParm (){ParmName="SortOrder", ParmValue = sortOrder=="1" ? "ASC" : "DESC", isNumber=false}
                };
                genericServiceResultTemplate.ResponseDataModel = await _iGeneric_SP_Repository.CallStoreProcedureReaderAsync<Establishment_EPFO_Report>("dbo.sp_Get_EPFO_Sent_Notice_Report", storeProcedureParms);
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