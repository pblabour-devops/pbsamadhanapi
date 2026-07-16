using Dapper;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using pbsamadhannetcoreapi.CommonUtiliteis;
using pbsamadhannetcoreapi.Models;
using pbsamadhannetcoreapi.Repositories.Implementations;
using pbsamadhannetcoreapi.Services.Implementations;
using pbsamadhannetcoreapi.ViewModels;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using System.Net.Http;
using System.Text;
using Org.BouncyCastle.Asn1.Ocsp;
using static Org.BouncyCastle.Math.EC.ECCurve;
using Org.BouncyCastle.Ocsp;
using DocumentFormat.OpenXml.Office2013.Excel;
using System.IO;
using Microsoft.Extensions.FileProviders;
using System.Security.Cryptography;
using DocumentFormat.OpenXml.Spreadsheet;
using EFCore.BulkExtensions;
using pbsamadhannetcoreapi.Repositories;
using DocumentFormat.OpenXml.Drawing.Charts;
using DocumentFormat.OpenXml.Office2010.Excel;
using static ClosedXML.Excel.XLPredefinedFormat;

namespace pbsamadhannetcoreapi.Services
{
    public class GolferRegistrationService : IGolferRegistrationService
    {
        private readonly AppDbContext _context;
        public GolferRegistrationService(AppDbContext context)
        {
            _context = context;
        }



        public async Task<GenericResponseTemplateModel<bool>> CheckMobileNo(string mobileNo)
        {
            GenericResponseTemplateModel<bool> genericServiceResultTemplate = new GenericResponseTemplateModel<bool>() { ResponseDataModel=false};
            try
            {
                var isAlreadyRegistered = await _context.GolferRegistrations.Where(x => x.MobileNo == mobileNo).FirstOrDefaultAsync();
                if (isAlreadyRegistered != null)
                {
                    genericServiceResultTemplate.ResponseDataModel = true;
                }
            }
            catch (Exception ex)
            {
                genericServiceResultTemplate.HasError = false;
                genericServiceResultTemplate.ErrorDesc = ex.Message;
            }
            return genericServiceResultTemplate;
        }

        public async Task<GenericResponseTemplateModel<string>> AddGolferRegistrationDetails(GolferRegistration formModel)
        {
            GenericResponseTemplateModel<string> genericServiceResultTemplate = new GenericResponseTemplateModel<string>();
            try
            {
                if (formModel != null)
                {
                    await _context.AddAsync(formModel);
                    await _context.SaveChangesAsync();
                    
                }
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
