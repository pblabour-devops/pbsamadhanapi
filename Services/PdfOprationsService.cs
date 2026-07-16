using iTextSharp.text;
using iTextSharp.text.pdf;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;
using Newtonsoft.Json;
using pbsamadhannetcoreapi.CommonUtiliteis;
using pbsamadhannetcoreapi.Models;
using pbsamadhannetcoreapi.Repositories;
using pbsamadhannetcoreapi.Repositories.Implementations;
using pbsamadhannetcoreapi.Services.Implementations;
using pbsamadhannetcoreapi.ViewModels;
using QRCoder;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;
using static pbsamadhannetcoreapi.CommonUtiliteis.ReflectionOps;
using Document = iTextSharp.text.Document;

namespace pbsamadhannetcoreapi.Services
{
    public class PdfOprationsService : IPdfOprationsService
    {
        private readonly IAuthService _iAuthService;
        private readonly IApplicationManagementService<ApplicationFullDetailsViewModel> _iApplicationManagementService;
        private readonly AppDbContext _context;
        private readonly IGenericRepository<Establishment_EPFO_Logs> _iGR_Establishment_EPFO;
        private readonly ICommonApisService _iCommonApisService;
        private readonly IInspectionService _iInspectionService;
        private readonly IGenericRepository<Licence_PE_ISM_GeneralDetail> _iGR_Licence_PE_ISM_GeneralDetail;
        private readonly IGenericRepository<Licence_CL_PE_GeneralDetail> _iGR_Licence_CL_PE_GeneralDetail;
        private readonly IGenericRepository<Licence_ContractLabour_GeneralDetail> _iGR_Licence_Contract_Labour_GeneralDetail;
        private readonly IGenericRepository<Licence_MotorTransport> _iGR_Licence_MotorTransport;
        private readonly IGenericRepository<Licence_TradeUnion> _iGR_Licence_TradeUnion;
        private readonly IGenericRepository<Licence_ISM_ContractLabour_GeneralDetail> _iGR_Licence_ISM_ContractLabour_GeneralDetail;
        private readonly IGeneric_SP_Repository _iGeneric_SP_Repository;
        public PdfOprationsService(IAuthService iAuthService,
            IApplicationManagementService<ApplicationFullDetailsViewModel> iApplicationManagementService,
            IGenericRepository<Establishment_EPFO_Logs> iGR_Establishment_EPFO,
            AppDbContext context,
            ICommonApisService iCommonApisService,
            IInspectionService iInspectionService,
            IGenericRepository<Licence_PE_ISM_GeneralDetail> iGR_Licence_PE_ISM_GeneralDetail,
            IGenericRepository<Licence_CL_PE_GeneralDetail> iGR_Licence_CL_PE_GeneralDetail,
            IGenericRepository<Licence_ContractLabour_GeneralDetail> iGR_Licence_Contract_Labour_GeneralDetail,
            IGenericRepository<Licence_MotorTransport> iGR_Licence_MotorTransport,
            IGenericRepository<Licence_TradeUnion> iGR_Licence_TradeUnion,
            IGenericRepository<Licence_ISM_ContractLabour_GeneralDetail> iGR_Licence_ISM_ContractLabour_GeneralDetail,
            IGeneric_SP_Repository iGeneric_SP_Repository)
        {
            _iAuthService = iAuthService;
            _iApplicationManagementService = iApplicationManagementService;
            _context = context;
            _iGR_Establishment_EPFO = iGR_Establishment_EPFO;
            _iCommonApisService = iCommonApisService;
            _iInspectionService = iInspectionService;
            _iGR_Licence_PE_ISM_GeneralDetail = iGR_Licence_PE_ISM_GeneralDetail;
            _iGR_Licence_CL_PE_GeneralDetail = iGR_Licence_CL_PE_GeneralDetail;
            _iGR_Licence_Contract_Labour_GeneralDetail = iGR_Licence_Contract_Labour_GeneralDetail;
            _iGR_Licence_MotorTransport = iGR_Licence_MotorTransport;
            _iGR_Licence_TradeUnion = iGR_Licence_TradeUnion;
            _iGR_Licence_ISM_ContractLabour_GeneralDetail = iGR_Licence_ISM_ContractLabour_GeneralDetail;
            _iGeneric_SP_Repository = iGeneric_SP_Repository;
        }


        //************* W A R N I N G **************
        //************* W A R N I N G **************
        //************* W A R N I N G **************
        //************* W A R N I N G **************
        //************* W A R N I N G **************
        // ******** API IS CALLING FROM DEEMED PROCESS ********
        public async Task<CertificateGenerateServiceResultTemplate> GenerateCertificate(Int64 appRefId, ApplicationTypeEnum applicationType, string userName, bool isDeemedCalling)
        {
            CertificateGenerateServiceResultTemplate genericFormModel = new CertificateGenerateServiceResultTemplate();
            try
            {
                GenerateLicenceNoViewModel licenceNo = await GenerateLicenceNo(appRefId);
                string CertificatePerformaURL = string.Empty;
                if (applicationType == ApplicationTypeEnum.REG_ESTB_OSH) // Establishment Registration
                {
                    genericFormModel = await Certificate_AppForm_1(appRefId, CertificatePerformaURL, applicationType, userName, licenceNo.LicenceNo);
                }
                else if (applicationType == ApplicationTypeEnum.CONTRACT_LABOUR_OSH) // Contract Labour Service
                {
                    genericFormModel = await Certificate_AppForm_1(appRefId, CertificatePerformaURL, applicationType, userName, licenceNo.LicenceNo);
                }
                else if (applicationType == ApplicationTypeEnum.COMMON_LICENCE_OSH) // Common Licence Service
                {
                    genericFormModel = await Certificate_AppForm_1(appRefId, CertificatePerformaURL, applicationType, userName, licenceNo.LicenceNo);
                }
                else if (applicationType == ApplicationTypeEnum.BUILDING_PLAN_HUD) // Building Plan HUD
                {
                    genericFormModel = await Certificate_AppForm_1(appRefId, CertificatePerformaURL, applicationType, userName, licenceNo.LicenceNo);
                }
                else if (applicationType == ApplicationTypeEnum.SHOP_LICENCE) // Shop
                {
                    genericFormModel = await Certificate_AppForm_6(appRefId, CertificatePerformaURL, applicationType, userName, licenceNo.LicenceNo, isDeemedCalling);
                }
                else if (applicationType == ApplicationTypeEnum.FACTORY_LICENCE) // Shop
                {
                    genericFormModel = await Certificate_AppForm_70(appRefId, CertificatePerformaURL, applicationType, userName, licenceNo.LicenceNo, isDeemedCalling);
                }
                else if (applicationType == ApplicationTypeEnum.WOMEN_NIGHT_SHIFT_SHOP) // Night Shift Shop
                {
                    genericFormModel = await Certificate_AppForm_61(appRefId, CertificatePerformaURL, applicationType, userName, licenceNo.LicenceNo, isDeemedCalling);
                }
                else if (applicationType == ApplicationTypeEnum.WOMEN_NIGHT_SHIFT_FACTORY)
                {
                    genericFormModel = await Certificate_AppForm_62(appRefId, CertificatePerformaURL, applicationType, userName, licenceNo.LicenceNo, isDeemedCalling);
                }
                else if (applicationType == ApplicationTypeEnum.BOCW_ESTABLISHMENT_ACT) // BOCW Licence Service
                {
                    genericFormModel = await Certificate_AppForm_35(appRefId, CertificatePerformaURL, applicationType, userName, licenceNo.LicenceNo, isDeemedCalling);
                }
                else if (applicationType == ApplicationTypeEnum.BUILDING_PLAN_PROPOSED) // Proposed Building Plan
                {
                    genericFormModel = await Certificate_AppForm_71(appRefId, CertificatePerformaURL, applicationType, userName, licenceNo.LicenceNo, isDeemedCalling);
                }
                else if (applicationType == ApplicationTypeEnum.BUILDING_PLAN_EXISTING) // Proposed Building Plan
                {
                    genericFormModel = await Certificate_AppForm_72(appRefId, CertificatePerformaURL, applicationType, userName, licenceNo.LicenceNo, isDeemedCalling);
                }
                else if (applicationType == ApplicationTypeEnum.BUILDING_PLAN_ADDITION_AMENDMENT) // Proposed Building Plan
                {
                    genericFormModel = await Certificate_AppForm_73(appRefId, CertificatePerformaURL, applicationType, userName, licenceNo.LicenceNo, isDeemedCalling);
                }
                else if (applicationType == ApplicationTypeEnum.ISM_PRINCIPAL_EMPLOYER) // ISM Principal Employer
                {
                    genericFormModel = await Certificate_AppForm_39(appRefId, CertificatePerformaURL, applicationType, userName, licenceNo.LicenceNo, isDeemedCalling);
                }
                else if (applicationType == ApplicationTypeEnum.PRINCIPAL_EMPLOYER) // Principal Employer
                {
                    genericFormModel = await Certificate_AppForm_37(appRefId, CertificatePerformaURL, applicationType, userName, licenceNo.LicenceNo, isDeemedCalling);
                }
                else if (applicationType == ApplicationTypeEnum.CONTRACT_LABOUR) // Contract Labour
                {
                    genericFormModel = await Certificate_AppForm_38(appRefId, CertificatePerformaURL, applicationType, userName, licenceNo.LicenceNo, isDeemedCalling);
                }
                else if (applicationType == ApplicationTypeEnum.MOTOR_TRANSPORT) // Contract Labour
                {
                    genericFormModel = await Certificate_AppForm_36(appRefId, CertificatePerformaURL, applicationType, userName, licenceNo.LicenceNo, isDeemedCalling);
                }
                else if (applicationType == ApplicationTypeEnum.TRADE_UNION) // Trade Union
                {
                    genericFormModel = await Certificate_AppForm_8(appRefId, CertificatePerformaURL, applicationType, userName, licenceNo.LicenceNo, isDeemedCalling);
                }
                else if (applicationType == ApplicationTypeEnum.ISM_CONTRACT_LABOUR) //ISM Contract Labour
                {
                    genericFormModel = await Certificate_AppForm_40(appRefId, CertificatePerformaURL, applicationType, userName, licenceNo.LicenceNo, isDeemedCalling);
                }
                else if (applicationType == ApplicationTypeEnum.BUILDING_PLAN_PSIEC) // PSIEC
                {
                    genericFormModel = await Certificate_AppForm_81(appRefId, CertificatePerformaURL, applicationType, userName, licenceNo.LicenceNo, isDeemedCalling);
                }
                else if (applicationType == ApplicationTypeEnum.BP_DECLARATION_STABILITY_CERTIFICATE) // Submission of stability
                {
                    genericFormModel = await Certificate_AppForm_76(appRefId, applicationType, userName, isDeemedCalling);
                }
            }
            catch (Exception ex)
            {
                genericFormModel.HasException = true;
                genericFormModel.Exceptions = ex.Message;
            }
            return genericFormModel;
        }

        public async Task<NoticeGenerateServiceResultTemplate> GenerateEPFONoticeCertificate(Establishment_EPFO_Logs formModel, string userName)
        {
            NoticeGenerateServiceResultTemplate genericFormModel = new NoticeGenerateServiceResultTemplate();
            try
            {
                GenerateDisputeNoViewModel generateDispute = await GenerateDisputeNo(formModel);
                string CertificatePerformaURL = string.Empty;

                genericFormModel = await Certificate_EPFO(formModel.EstablishmentRefId, userName, generateDispute.EstablishmentEPFOLogsRefId, generateDispute.DisputeNo);

            }
            catch (Exception ex)
            {
                genericFormModel.HasException = true;
                genericFormModel.Exceptions = ex.Message;
            }
            return genericFormModel;
        }

        public async Task<GeneratePdfServiceResultTemplate> GenerateInspectionPdf(Int64 inspectionRefId, int inspectionType)
        {
            GeneratePdfServiceResultTemplate genericFormModel = new GeneratePdfServiceResultTemplate();
            try
            {
                string CertificatePerformaURL = string.Empty;
                if (inspectionType == 1)
                {
                    genericFormModel = await Certificate_Inspection_FactoryWing(inspectionRefId, inspectionType);
                }
                if (inspectionType == 2)
                {
                    genericFormModel = await Certificate_Inspection_LabourWing(inspectionRefId, inspectionType);
                }
            }
            catch (Exception ex)
            {
                genericFormModel.HasException = true;
                genericFormModel.Exceptions = ex.Message;
            }
            return genericFormModel;
        }

        public async Task<GeneratePdfServiceResultTemplate> GenerateInspectionViolationPdf(Int64 inspectionRefId, int inspectionType)
        {
            GeneratePdfServiceResultTemplate genericFormModel = new GeneratePdfServiceResultTemplate();
            try
            {
                string CertificatePerformaURL = string.Empty;
                if (inspectionType == 1)
                {
                    genericFormModel = await Inspection_ViolationReport_FactoryWing(inspectionRefId, inspectionType);
                }
                if (inspectionType == 2)
                {
                    genericFormModel = await Inspection_ViolationReport_LabourWing(inspectionRefId, inspectionType);
                }
            }
            catch (Exception ex)
            {
                genericFormModel.HasException = true;
                genericFormModel.Exceptions = ex.Message;
            }
            return genericFormModel;
        }
        private async Task<CertificateGenerateServiceResultTemplate> Certificate_AppForm_1(Int64 appRefId, string CertificatePerformaURL, ApplicationTypeEnum applicationType, string userName, string licenceNo)
        {
            #region Commented Code
            //CertificateGenerateServiceResultTemplate genericFormModel = new CertificateGenerateServiceResultTemplate();
            //try
            //{
            //    Guid guId = Guid.NewGuid();
            //    genericFormModel.PdfNameGUID = guId.ToString();
            //    genericFormModel.HasException = false;
            //    var SaveDirectoryPath = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "TempFiles")).Root + guId.ToString() + ".pdf";
            //    iTextSharp.text.Document document = new iTextSharp.text.Document(iTextSharp.text.PageSize.A4.Rotate(), 0, 0, 0, 0);

            //    #region ClassObjects
            //    pdfFonts classFonts = new pdfFonts();
            //    #endregion

            //    #region CreateObjects
            //    PdfWriter owriter = PdfWriter.GetInstance(document, new FileStream(SaveDirectoryPath, FileMode.Create));
            //    PDFPageEvents pageEvent = new PDFPageEvents();
            //    owriter.PageEvent = pageEvent;
            //    owriter.SetEncryption(PdfWriter.STRENGTH128BITS, "", "", PdfWriter.AllowPrinting);
            //    #endregion

            //    document.Open();

            //    #region Get Certificate Templete

            //    iTextSharp.text.Image page;

            //    if (applicationType == ApplicationTypeEnum.REG_ESTB) // Establishment Registration
            //    {
            //        page = iTextSharp.text.Image.GetInstance("Content/images/CertificatePerforma_1.png");
            //    }
            //    else if (applicationType == ApplicationTypeEnum.CONTRACT_LABOUR) // Contract Labour Service
            //    {
            //        page = iTextSharp.text.Image.GetInstance("Content/images/CertificatePerforma_ContactLabour.png");
            //    }
            //    else if (applicationType == ApplicationTypeEnum.COMMON_LICENCE) // Common Licence Service
            //    {
            //        page = iTextSharp.text.Image.GetInstance("Content/images/CertificatePerforma_CommonLicense.png");
            //    }
            //    else if(applicationType == ApplicationTypeEnum.BUILDING_PLAN_HUD) // Building Plan HUD
            //    {
            //        page = iTextSharp.text.Image.GetInstance("Content/images/CertificatePerforma_BP_HUD.png");
            //    }
            //    else if (applicationType == ApplicationTypeEnum.SHOP_LICENCE) // Shop
            //    {
            //        page = iTextSharp.text.Image.GetInstance("Content/images/CertificatePerforma_Shop.jpg");
            //    }
            //    else
            //    {
            //        page = iTextSharp.text.Image.GetInstance("Content/images/CertificatePerforma_1.png");
            //    }

            //    page.Alignment = iTextSharp.text.Image.ALIGN_CENTER;
            //    page.ScaleToFit(843f, 843f);
            //    page.SetAbsolutePosition(0f, 0f);
            //    document.Add(page);
            //    #endregion

            //    #region QR Code Generate
            //    iTextSharp.text.Image qr = iTextSharp.text.Image.GetInstance("Content/images/qr.png");
            //    qr.ScaleToFit(80f, 80f);
            //    qr.SetAbsolutePosition(730f, 480f);
            //    document.Add(qr);
            //    #endregion

            //    PdfPTable ptablerightSideDetail = new PdfPTable(1);
            //    ptablerightSideDetail.DefaultCell.Border = 0;
            //    ptablerightSideDetail.DefaultCell.Padding = 0;

            //    #region Regester Font Style File
            //    var fontName = "Alice";
            //    if (!FontFactory.IsRegistered(fontName))
            //    {
            //        var fontPath = "Content/fonts/Alice-Regular.ttf";
            //        Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            //        FontFactory.Register(fontPath, fontName);
            //    }
            //    #endregion

            //    Font font = FontFactory.GetFont(fontName, BaseFont.IDENTITY_H, 16);
            //    Font fontSmall = FontFactory.GetFont(fontName, BaseFont.IDENTITY_H, 12);
            //    Font fontBOLD = FontFactory.GetFont(fontName, BaseFont.IDENTITY_H, 16, iTextSharp.text.Font.BOLD);

            //    #region Create content for certificate
            //    if(applicationType == ApplicationTypeEnum.REG_ESTB) // Establishment Registration
            //    {
            //        var appDetails = await _iApplicationManagementService.GetEstablishmentApplicationCertificateDetailsByAppRefId(appRefId);
            //        Paragraph para = new Paragraph();
            //        para.Add(new Chunk("Sh. ", font));
            //        para.Add(new Chunk("Manu Johar" + " , ", fontBOLD));
            //        para.Add(new Chunk("S/O, D/O, W/O:", font));
            //        para.Add(new Chunk("Romesh Chand Johar", fontBOLD));
            //        para.Add(new Chunk(" Occupier,  " + Chunk.NEWLINE, font));
            //        para.Add(new Chunk("M/s ", font));
            //        para.Add(new Chunk(" Abhishek Global ventures" + ", " + "Hadbast No. 245, Jaspal Banger Road, Ludhiana" + Chunk.NEWLINE, font));

            //        para.Add(new Chunk(Chunk.NEWLINE + "Registration Number :- ", font));
            //        para.Add(new Chunk("LDH0FL1234", fontBOLD));
            //        para.Add(new Chunk("  Fees Rs. ", font));
            //        para.Add(new Chunk("1000.00", fontBOLD));
            //        para.Add(new Chunk(" In words (", font));
            //        para.Add(new Chunk("One Thousand Rupee Only", fontBOLD));
            //        para.Add(new Chunk(") valid for ", font));
            //        para.Add(new Chunk("the premises described above for use as a factory employing not more than ", font));
            //        para.Add(new Chunk("100", fontBOLD));
            //        para.Add(new Chunk(" persons on any one day during the year and using motive power not exceeding  ", font));
            //        para.Add(new Chunk("50", fontBOLD));
            //        para.Add(new Chunk(" Kilo Watt subject to the provisions of The Factories Act, 1948 as amended from time to time and the rules made thereunder." + Chunk.NEWLINE, font));
            //        para.Add(new Chunk("This licence shall remain in force till the 31st day of December, " + "2022" + "." + Chunk.NEWLINE, font));
            //        ptablerightSideDetail.AddCell(new PdfPCell(para) { HorizontalAlignment = PdfPCell.ALIGN_CENTER, PaddingTop = 170, FixedHeight = 400, Border = 0 });
            //        ptablerightSideDetail.WidthPercentage = 80;
            //    }
            //    else if (applicationType == ApplicationTypeEnum.COMMON_LICENCE) // Common Licence Service
            //    {
            //        var appDetails = await _iApplicationManagementService.GetCommonLicenceApplicationCertificateDetailsByAppRefId(appRefId);
            //        Paragraph para = new Paragraph();
            //        para.Add(new Chunk(Chunk.NEWLINE + "Office of designated authority under section 119 of the Occupational Safety, Health abd Working Conditions Code 2020" + "." + Chunk.NEWLINE, font));
            //        para.Add(new Chunk(Chunk.NEWLINE + "License Number: ", font));
            //        para.Add(new Chunk("LDH0FL1234" + " , ", fontBOLD));
            //        para.Add(new Chunk("Date Of Issue : ", font));
            //        para.Add(new Chunk("12-07-2022", fontBOLD));

            //        para.Add(new Chunk(Chunk.NEWLINE + "A License Number is granted to Shree:- ", font));
            //        para.Add(new Chunk(appDetails.ListData[0].OccupierName + " , ", fontBOLD));
            //        para.Add(new Chunk(" of Establishment : ", font));
            //        para.Add(new Chunk(appDetails.ListData[0].EstablishmentName + ".", fontBOLD));
            //        para.Add(new Chunk("registered under the section-3 of the code having registration number : ", font));
            //        para.Add(new Chunk("LDH0FL1234", fontBOLD));
            //        para.Add(new Chunk(" as below.", font));

            //        para.Add(new Chunk(Chunk.NEWLINE + "A:- To run a factory in which manufacturing process :", font));
            //        para.Add(new Chunk(appDetails.ListData[0].ManufacturingProcess + ",", fontBOLD));
            //        para.Add(new Chunk(" shall be caried by employing not more than ", font));
            //        para.Add(new Chunk(appDetails.ListData[0].TotalNoWorkersToBeEmployedInLicence.ToString() + "workers" + ",", fontBOLD));
            //        para.Add(new Chunk(" and power not more than ", font));
            //        para.Add(new Chunk(appDetails.ListData[0].ElectricLoadConnectedInKilowatts.ToString() + "KW", fontBOLD));
            //        para.Add(new Chunk(" for the purpose of The Occupational Safety, Health And Working Conditions Code 2020 and rules made there under and whose plan approved by Chief Inspector_cum_Facilitator vide number", font));
            //        para.Add(new Chunk("This licence shall remain in force till the 31st day of December, " + appDetails.ListData[0].DateOfTerminationOfEmployementUnderEachContractor.Year.ToString() + "." + Chunk.NEWLINE, font));

            //        para.Add(new Chunk(Chunk.NEWLINE +  "Fees Rs. ", font));
            //        para.Add(new Chunk(appDetails.ListData[0].AmountCalculated.ToString()));
            //        para.Add(new Chunk(" In words ", font));
            //        string AmountInWord = ConvertAmount(double.Parse(appDetails.ListData[0].AmountCalculated.ToString()));
            //        para.Add(new Chunk((AmountInWord), font));

            //        para.Add(new Chunk(Chunk.NEWLINE + "B:- Engaggement of contract labour subject to the conditions annexed to this license as given below : ", font));
            //        para.Add(new Chunk(Chunk.NEWLINE + "Fees Rs. ", font));
            //        para.Add(new Chunk(appDetails.ListData[0].AmountCalculated.ToString()));
            //        para.Add(new Chunk(Chunk.NEWLINE + "Date of deposit ", font));
            //        para.Add(new Chunk(appDetails.ListData[0].AmountCalculated.ToString()));
            //        para.Add(new Chunk(Chunk.NEWLINE + "1- Maximum no of workers to be employed as contractor labour: ", font));
            //        para.Add(new Chunk(appDetails.ListData[0].NumberOfContractLabourToBeEmployed.ToString()));
            //        para.Add(new Chunk(Chunk.NEWLINE + "2- Nature of activity for which contractor labour shal be engaged: ", font));
            //        para.Add(new Chunk(appDetails.ListData[0].NatureOfWorkContractLabour));

            //        ptablerightSideDetail.AddCell(new PdfPCell(para) { HorizontalAlignment = PdfPCell.ALIGN_CENTER, PaddingTop = 170, FixedHeight = 400, Border = 0 });
            //        ptablerightSideDetail.WidthPercentage = 85;
            //    }
            //    else if (applicationType == ApplicationTypeEnum.CONTRACT_LABOUR) // Contract Labour Licence Service
            //    {
            //        var appDetails = await _iApplicationManagementService.GetContractLabourApplicationCertificateDetailsByAppRefId(appRefId);
            //        Paragraph para = new Paragraph();
            //        para.Add(new Chunk(Chunk.NEWLINE + "Registration Number:- ", font));
            //        para.Add(new Chunk("LDH0FL1234", fontBOLD));
            //        para.Add(new Chunk(" Registration Date:", font));
            //        para.Add(new Chunk(DateTime.Now.ToString(), fontBOLD));

            //        para.Add(new Chunk(Chunk.NEWLINE + "Slab Fee:- ", font));
            //        para.Add(new Chunk(appDetails.ListData[0].SlabFees + ".", fontBOLD));
            //        para.Add(new Chunk(" Total Security Fee:- ", font));
            //        para.Add(new Chunk(appDetails.ListData[0].TotalSecurityFee + ".", fontBOLD));

            //        para.Add(new Chunk(Chunk.NEWLINE + "Licence is granted to:- ", font));
            //        para.Add(new Chunk(appDetails.ListData[0].ContractorName + ".", fontBOLD));
            //        para.Add(new Chunk("  under Section 47(1) of the Occupational Safety, Health And Working Conditions Code 2020 subject to the conditions specified annexure.", font));

            //        para.Add(new Chunk(Chunk.NEWLINE + "2- The licence is for doing the work of  : ", font));
            //        para.Add(new Chunk( appDetails.ListData[0].NatureOfWork + ".", fontBOLD));
            //        para.Add(new Chunk(" in the establishment :- ", font));
            //        para.Add(new Chunk(appDetails.ListData[0].EstablishmentName + ".", fontBOLD));

            //        para.Add(new Chunk(Chunk.NEWLINE + "3- The amount of workmen employed, as contract labour is the establishment shall not, on any day, exceed : ", font));
            //        para.Add(new Chunk( appDetails.ListData[0].MaxContractLabourToBeEmployed + ".", fontBOLD));

            //        para.Add(new Chunk(Chunk.NEWLINE + "4- This licence shall remain in force till : ", font));
            //        para.Add(new Chunk( appDetails.ListData[0].DurationOfProposedContractWorkEnd + ".", fontBOLD));

            //        ptablerightSideDetail.AddCell(new PdfPCell(para) { HorizontalAlignment = PdfPCell.ALIGN_CENTER, PaddingTop = 170, FixedHeight = 400, Border = 0 });
            //        ptablerightSideDetail.WidthPercentage = 80;
            //    }
            //    else if (applicationType == ApplicationTypeEnum.BUILDING_PLAN_HUD) // Building Plan HUD
            //    {
            //        var appDetails = await _iApplicationManagementService.GetBuildingPlanHUDCertificateDetailsByAppRefId(appRefId);
            //        Paragraph para = new Paragraph();
            //        para.Add(new Chunk("Sh. ", font));
            //        para.Add(new Chunk(appDetails.ListData[0].EstablishmentName + " , ", fontBOLD));
            //        para.Add(new Chunk("Establishment Address:", font));
            //        para.Add(new Chunk(appDetails.ListData[0].EstablishmentAddress, fontBOLD));

            //        para.Add(new Chunk(Chunk.NEWLINE + "Registration Number:- ", font));
            //        para.Add(new Chunk("LDH0FL1234", fontBOLD));
            //        para.Add(new Chunk("  Building cost in Rs. ", font));
            //        para.Add(new Chunk( appDetails.ListData[0].BuildingCost.ToString(), fontBOLD));
            //        para.Add(new Chunk(" In words (", font));
            //        para.Add(new Chunk("", fontBOLD));
            //        para.Add(new Chunk(") Plot area in Sqft : ", font));
            //        para.Add(new Chunk( appDetails.ListData[0].PlotAreaSqFt.ToString(), fontBOLD));
            //        para.Add(new Chunk("Plot area in acre : ", fontBOLD));
            //        para.Add(new Chunk( appDetails.ListData[0].PlotAreaAcres.ToString() + "." + Chunk.NEWLINE, fontBOLD));
            //        para.Add(new Chunk("This licence shall remain in force till the 31st day of December, " + "2023" + "." + Chunk.NEWLINE, font));
            //        ptablerightSideDetail.AddCell(new PdfPCell(para) { HorizontalAlignment = PdfPCell.ALIGN_CENTER, PaddingTop = 170, FixedHeight = 400, Border = 0 });
            //        ptablerightSideDetail.WidthPercentage = 80;
            //    }
            //    else if (applicationType == ApplicationTypeEnum.SHOP_LICENCE)
            //    {
            //        var appDetails = await _iApplicationManagementService.GetShopLicenceCertificateDetailsByAppRefId(appRefId);
            //        Paragraph para = new Paragraph();
            //        para.Add(new Chunk("Sh. ", font));
            //        para.Add(new Chunk(appDetails.ListData[0].EstablishmentName + " , ", fontBOLD));
            //        para.Add(new Chunk("Establishment Address:", font));
            //        para.Add(new Chunk(appDetails.ListData[0].EstablishmentAddress, fontBOLD));

            //        para.Add(new Chunk(Chunk.NEWLINE + "Registration Number:- ", font));
            //        para.Add(new Chunk("LDH0FL1234", fontBOLD));
            //        para.Add(new Chunk("  Establishment Owner Name. ", font));
            //        para.Add(new Chunk(appDetails.ListData[0].OwnerName.ToString(), fontBOLD));
            //        para.Add(new Chunk(") Owner Father Name : ", font));
            //        para.Add(new Chunk(appDetails.ListData[0].OwnerFatherOrHusbandName.ToString(), fontBOLD));
            //        para.Add(new Chunk(" Pan Card No : ", fontBOLD));
            //        para.Add(new Chunk(appDetails.ListData[0].PANCard.ToString(), fontBOLD));

            //        para.Add(new Chunk(" Employee Name : ", fontBOLD));
            //        para.Add(new Chunk(appDetails.ListData[0].EmployeeName.ToString(), fontBOLD));
            //        para.Add(new Chunk(" Employee Father Name : ", fontBOLD));
            //        para.Add(new Chunk(appDetails.ListData[0].EmployeeFatherName.ToString(), fontBOLD));
            //        para.Add(new Chunk(" Gender : ", fontBOLD));
            //        para.Add(new Chunk(appDetails.ListData[0].Gender.ToString(), fontBOLD));
            //        para.Add(new Chunk(" DateOfBirth : ", fontBOLD));
            //        para.Add(new Chunk(appDetails.ListData[0].DateOfBirth.ToString(), fontBOLD));

            //        para.Add(new Chunk(" This licence shall remain in force till the 31st day of December, " + "2023" + "." + Chunk.NEWLINE, font));
            //        ptablerightSideDetail.AddCell(new PdfPCell(para) { HorizontalAlignment = PdfPCell.ALIGN_CENTER, PaddingTop = 170, FixedHeight = 400, Border = 0 });
            //        ptablerightSideDetail.WidthPercentage = 80;
            //    }
            //    #endregion

            //    #region Add officer Signature
            //    iTextSharp.text.Image signature = null; //iTextSharp.text.Image.GetInstance("Content/images/1.png");
            //    signature.ScaleToFit(120f, 120f);
            //    signature.SetAbsolutePosition(630f, 150f);
            //    document.Add(signature);
            //    #endregion

            //    #region Officer Name & Designation
            //    var userDetails = await _iAuthService.GetUserFullDetailByUserName(userName);
            //    PdfPTable ptablerightSideDetail1 = new PdfPTable(1);
            //    Paragraph para1 = new Paragraph();

            //    para1.Add(new Chunk(userDetails.ListData[0].UserFullName + Chunk.NEWLINE, fontSmall));
            //    para1.Add(new Chunk(userDetails.ListData[0].NormalizedName + Chunk.NEWLINE, fontSmall));
            //    para1.Add(new Chunk(userDetails.ListData[0].DepartmentName + Chunk.NEWLINE, fontSmall));
            //    para1.Add(new Chunk(userDetails.ListData[0].FactoryCircleName + Chunk.NEWLINE, fontSmall));
            //    para1.Add(new Chunk(userDetails.ListData[0].DistrictName + Chunk.NEWLINE, fontSmall));
            //    #endregion

            //    ptablerightSideDetail1.AddCell(new PdfPCell(para1) { PaddingTop = 60, PaddingLeft = 620, Border = 0 });
            //    ptablerightSideDetail1.WidthPercentage = 100;
            //    ptablerightSideDetail1.DefaultCell.FixedHeight = 10f;
            //    document.Add(ptablerightSideDetail);
            //    document.Add(ptablerightSideDetail1);

            //    #region Building plan HUD terms & conditions
            //    document.NewPage();
            //    PdfPTable termAndConditionAnnexure = new PdfPTable(1);
            //    ptablerightSideDetail.DefaultCell.Border = 0;
            //    ptablerightSideDetail.DefaultCell.Padding = 0;

            //    termAndConditionAnnexure.AddCell(new PdfPCell(new Phrase("Proposed Building plans of project named in subject received in this office. Report from DTP, (DTP Name) sought by the officer. On the basis of DTP report, the building plan for the purpose of Project Purpose have been scrutinized and approval for an area of (land area in acre) has been considered. The acceptance/approval is hereby granted subject to the following terms and conditions.", fontBOLD)) { Padding = 20, Colspan = 7, Border = 0, Left = 40 });

            //    termAndConditionAnnexure.AddCell(new PdfPCell(new Phrase("1. Building shall be constructed as per approved building Plans/Service plans and in compliance with the Building Rules, 2021 as amended from time to time.", font)) );
            //    termAndConditionAnnexure.AddCell(new PdfPCell(new Phrase("2. The Applicant shall obtain any other permission required under any other Act/law/guidelines from any other department/Authority at his own level before, during and completion of construction at the site.", font)));
            //    termAndConditionAnnexure.AddCell(new PdfPCell(new Phrase("3. Service plans shall be got approved from Competent Authority.", font)));
            //    termAndConditionAnnexure.AddCell(new PdfPCell(new Phrase("4. Plinth level shall be obtained from Competent Authority and copy of that shall be submitted in the office of Director of Factories, Punjab.", font)));
            //    termAndConditionAnnexure.AddCell(new PdfPCell(new Phrase("5. Conditions imposed by departments such as PPCB/Forest/Fire department/Defense/Irrigation/NHAI/PWD/AAI/HUD or any other department needs to be complied.", font)));
            //    termAndConditionAnnexure.AddCell(new PdfPCell(new Phrase("6. The applicant shall be responsible to adhere as per Chapter 4 of Fire and Life Safety of the National Building Code of India,2016", font)));
            //    termAndConditionAnnexure.AddCell(new PdfPCell(new Phrase("7. The Applicant shall abide by the Action Plan issued by Government regarding monitoring of demolition and construction activity vide memo number: 17/20/2016-IHGI/1112 dated 02-03-2017", font)));
            //    termAndConditionAnnexure.AddCell(new PdfPCell(new Phrase("8. Completion and Occupancy Certificate as required under Building Bye-Laws, 2021 shall be taken from the Competent Authority.", font)));
            //    termAndConditionAnnexure.AddCell(new PdfPCell(new Phrase("9. The provisions of Punjab Energy Conservation Code shall be applicable in accordance with notification number: 18/4/2016HGI/1856 dated 24-06-2016", font)));
            //    termAndConditionAnnexure.AddCell(new PdfPCell(new Phrase("10. The building shall be constructed as per structural drawings designed by registered Engineer/structural engineer.", font)));
            //    termAndConditionAnnexure.AddCell(new PdfPCell(new Phrase("11. Construction shall be carried under the supervision of Engineer/Structural Engineer and Architect as per Form A.", font)));
            //    termAndConditionAnnexure.AddCell(new PdfPCell(new Phrase("12. The Applicant shall be liable to pay the difference of amount, if any, found at any point of time by any department or concerned development Authority within 30 days from the date of demand.", font)));
            //    termAndConditionAnnexure.AddCell(new PdfPCell(new Phrase("13. Green belt shall be developed and maintained as approved in the plans. The planting of trees of indigenous species shall be mandatory as per the provisions under PUPD Building Rules, 2021. Plantation shall be commenced once construction is started.", font)));
            //    termAndConditionAnnexure.AddCell(new PdfPCell(new Phrase("14. Stability Certificate in Form 1-F shall be got accepted as per Rule 4 of Punjab Factory Rules,1952.", font)));
            //    termAndConditionAnnexure.AddCell(new PdfPCell(new Phrase("15. The Applicant shall ensure to get the registration/license under the Factory Act,1948 and rules made thereunder.", font)));

            //    termAndConditionAnnexure.AddCell(new PdfPCell(new Phrase("16. The concerned owners of Factory/Partners/Directors/Proprietor shall  be solely responsible for any kind of violations/daviations/diversions made by them in the building/machinery plans etc.", font)));
            //    termAndConditionAnnexure.AddCell(new PdfPCell(new Phrase("17. The owner will ensure that all the precautions are taken regarding safety measures, such as fire safety and other provisions of the Factory Act 1948 and Punjab Factory Rules 1952.", font)));
            //    termAndConditionAnnexure.AddCell(new PdfPCell(new Phrase("18. The Applicant will also ensure proper facilities for the workers as per the Factories Act, 1948 and Punjab Factory Rules 1952.", font)));
            //    termAndConditionAnnexure.AddCell(new PdfPCell(new Phrase("19. The Site for which building plans have been approved shall be registered under The Building and Other Construction Workers Act, 1996.", font)));
            //    termAndConditionAnnexure.AddCell(new PdfPCell(new Phrase("20. The Applicant will ensure that all workers shall be employed in construction activity shall be registered as “BENEFICIARY” with Punjab Building and Other Construction Workers Welfare Board.", font)));
            //    termAndConditionAnnexure.AddCell(new PdfPCell(new Phrase("21. Once manufacturing activity is commenced in factory the occupier of factory shall ensure that Labour Welfare Fund is contributed to Punjab Labour Welfare Board as per provisions of Punjab Labour Welfare Fund Act, 1965 and rules made there under so that they can apply for the various Welfare schemes.", font)));
            //    termAndConditionAnnexure.AddCell(new PdfPCell(new Phrase("22. Occupant load shall not exceed as per the undertaking. In case there will be a change in occupant load revised building plans shall be got approved from the Competent Authority.", font)));
            //    termAndConditionAnnexure.AddCell(new PdfPCell(new Phrase("23. The owner of building shall submit the Form-I and carried out the assessment of the site as required under The Building and other Construction Workers Welfare Cess Act, 1996 and rules made there under.", font)));
            //    termAndConditionAnnexure.AddCell(new PdfPCell(new Phrase("24. The Applicant shall install Roof Top Solar Water Heating System at the site as per PEDA guidelines.", font)));
            //    termAndConditionAnnexure.AddCell(new PdfPCell(new Phrase("25. The Applicant shall install Photo Voltaic Panels at the site as per PEDA guidelines.", font)));
            //    termAndConditionAnnexure.AddCell(new PdfPCell(new Phrase("26. The Applicant shall strictly abide by the provisions regarding sustainable waste Management under Building Rules, 2021 regarding which approval of Competent Authority is mandatory.", font)));
            //    termAndConditionAnnexure.AddCell(new PdfPCell(new Phrase("27. The Applicant shall make provisions of Rain water harvesting as per Building Rules, 2021 within the project area at his own cost.", font)));

            //    termAndConditionAnnexure.AddCell(new PdfPCell(new Phrase("28. During the construction of building, if the owner/Architect/Civil Engineer/Structural Engineer are changed, the applicant shall intimate the same to the Competent Authority that he/she is no longer responsible for the project from the date of actual dispatch of the letter. The information must be sent in seven days of occurrence of the change to the Competent Authority by the respective Owner/Architect/Civil Engineer/Structural Engineer. The Construction work shall have to be suspended until the new owner/Architect/Civil Engineer/Structural Engineer as the case may be undertaken the full responsibility of the project vide forms and documents submitted at the time of applying for erection/re- erection of the building within seven days of his taking over.", font)));
            //    termAndConditionAnnexure.AddCell(new PdfPCell(new Phrase("29. The Applicant shall abide by all the provisions of The Punjab Regional and Town Planning and Development Act, 1995 as amended from time to time and shall also abide by the provisions of Unified Zoning Regulations as amended from time to time.", font)));
            //    termAndConditionAnnexure.AddCell(new PdfPCell(new Phrase("30. The Applicant shall not use underground water for construction of development works in the notified area. The Applicant shall use surface water sources or treated water from nearby Sewage Treatment Plant", font)));
            //    termAndConditionAnnexure.AddCell(new PdfPCell(new Phrase("31. The Amount of Rs …….. is received as EDC and Rs …… as SIF", font)));
            //    termAndConditionAnnexure.AddCell(new PdfPCell(new Phrase("32. The Applicant shall be responsible for the ownership of the land pertaining to the site. If any dispute related to the ownership of the land arises at any point of time, this department shall not be responsible for the same.", font)));
            //    document.Add(termAndConditionAnnexure);

            //    #region Certificate Footer
            //    PdfPTable CertificateFooter = new PdfPTable(1);
            //    para1 = new Paragraph();

            //    para1.Add(new Chunk("Copy of building plans has been forwarded to the following for information and necessary action please :" + Chunk.NEWLINE, font));
            //    para1.Add(new Chunk("1.	Director Town and country planning, Punjab" + Chunk.NEWLINE, fontSmall));
            //    para1.Add(new Chunk("District Town Planning of concerned district" + Chunk.NEWLINE, fontSmall));
            //    para1.Add(new Chunk("Concerned Deputy Director/Assistant Director of Factories" + Chunk.NEWLINE, fontSmall));

            //    para1.Add(new Chunk("Office of Director of Factories" + Chunk.NEWLINE, font));
            //    para1.Add(new Chunk("Address : Model Welfare Centre" + Chunk.NEWLINE, font));
            //    para1.Add(new Chunk("Labour Bhawan Sector-64, S.A.S. Nagar" + Chunk.NEWLINE, font));
            //    para1.Add(new Chunk("Punjab, India" + Chunk.NEWLINE, font));
            //    para1.Add(new Chunk("Phone No" + "91-172-2702486" + Chunk.NEWLINE, font));

            //    CertificateFooter.AddCell(new PdfPCell(para1) { PaddingTop = 60, PaddingLeft = 100, Border = 0 });
            //    CertificateFooter.WidthPercentage = 100;
            //    CertificateFooter.DefaultCell.FixedHeight = 10f;
            //    document.Add(CertificateFooter);
            //    #endregion

            //    #endregion

            //    document.Close();
            //}
            //catch (Exception ex)
            //{
            //    genericFormModel.HasException = true;
            //    genericFormModel.Exceptions = ex.Message;
            //}
            //return genericFormModel;

            #endregion


            CertificateGenerateServiceResultTemplate genericFormModel = new CertificateGenerateServiceResultTemplate();
            try
            {
                Guid guId = Guid.NewGuid();
                genericFormModel.PdfNameGUID = guId.ToString();
                genericFormModel.HasException = false;
                var SaveDirectoryPath = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "TempFiles")).Root + guId.ToString() + ".pdf";
                iTextSharp.text.Document documents = new iTextSharp.text.Document(iTextSharp.text.PageSize.A4, 5, 5, 30, 30);

                #region ClassObjects
                pdfFonts classFonts = new pdfFonts();
                #endregion

                #region CreateObjects
                PdfWriter owriter = PdfWriter.GetInstance(documents, new FileStream(SaveDirectoryPath, FileMode.Create));
                PDFPageEvents pageEvent = new PDFPageEvents();
                owriter.PageEvent = pageEvent;
                owriter.SetEncryption(PdfWriter.STRENGTH128BITS, "", "", PdfWriter.AllowPrinting);
                #endregion

                #region Regester Font Style
                var fontName = "Alice";
                Font font = FontFactory.GetFont(fontName, BaseFont.IDENTITY_H, 16);
                Font fontSmall = FontFactory.GetFont(fontName, BaseFont.IDENTITY_H, 12);
                Font fontBOLD = FontFactory.GetFont(fontName, BaseFont.IDENTITY_H, 16, iTextSharp.text.Font.BOLD);
                #endregion

                documents.Open();
                Font verdanaHeader = FontFactory.GetFont("Arial", 9, Font.BOLD);
                Font verdana = FontFactory.GetFont("Arial", 9, BaseColor.BLACK);
                Font verdanaBoldItalic = FontFactory.GetFont("Arial", 8, Font.BOLDITALIC);
                Font verdanasmall = FontFactory.GetFont("Arial", 8, Font.NORMAL);
                verdanasmall.Color = BaseColor.WHITE;
                Font verdanaRightHeader = FontFactory.GetFont("Arial", 15, Font.BOLD);
                verdanaRightHeader.Color = BaseColor.WHITE;
                Font verdanaRightHeader1 = FontFactory.GetFont("Arial", 12, Font.BOLD);
                verdanaRightHeader1.Color = BaseColor.WHITE;
                Font verdanaRightHeader2 = FontFactory.GetFont("Arial", 9, Font.BOLD);
                verdanaRightHeader2.Color = BaseColor.WHITE;
                Font monotype = FontFactory.GetFont("Arial", 13, Font.BOLD);
                Font monotypeHeader = FontFactory.GetFont("Arial", 15, Font.BOLD);
                Font impact = new Font(fontSmall.BaseFont, 12);
                Font impactHeader = new Font(fontSmall.BaseFont, 12);
                impactHeader.Color = BaseColor.WHITE;
                BaseFont bfTimes = BaseFont.CreateFont(BaseFont.TIMES_ROMAN, BaseFont.CP1252, false);
                Font timesRoman = FontFactory.GetFont("Arial", 10, BaseColor.BLACK);
                Font verdanaBold = FontFactory.GetFont("Arial", 9, Font.BOLD);

                PdfPTable ptableShopAnnexure = new PdfPTable(7);
                ptableShopAnnexure.DefaultCell.Border = 0;
                ptableShopAnnexure.DefaultCell.Padding = 2;

                var application = _context.Applications.Where(x => x.AppId == appRefId && x.IsDeleted == false).Include(x => x.ProjectSites).FirstOrDefault();

                int sno = 1;

                #region PDF Header & Logo
                string logoImagePath = "D:/Projects/pbsamadhannetcoreapi/Content/images/govPunjab.png";

                // Create an Image instance for the logo
                Image logoImage = Image.GetInstance(logoImagePath);
                logoImage.ScaleToFit(100, 100);  // Adjust the size as needed

                // Add the logo before the content
                PdfPCell logoBeforeCell = new PdfPCell(logoImage)
                {
                    Colspan = 1,
                    Rowspan = 2,
                    Border = 0,
                    Padding = 10,
                };
                ptableShopAnnexure.AddCell(logoBeforeCell);

                // Create a Font for the header content with a different color
                BaseFont headerBaseFont = BaseFont.CreateFont(BaseFont.HELVETICA_BOLD, BaseFont.CP1252, BaseFont.NOT_EMBEDDED);
                Font headerFont = new Font(headerBaseFont, 12, Font.NORMAL, BaseColor.BLACK);

                // Add the content to the second cell in the header row
                PdfPCell headerCell = new PdfPCell(new Phrase("OFFICE OF DIRECTOR OF FACTORIES, PUNJAB LABOUR BHAWAN(MODEL WELFARE CENTRE),SECTOR - 64, S.A.S.NAGAR", headerFont))
                {
                    Padding = 20,
                    Colspan = 4,  // Adjust the colspan based on your layout
                    Border = 0,
                    Left = 40,
                };
                ptableShopAnnexure.AddCell(headerCell);

                // Add the logo after the content

                logoImagePath = "D:/Projects/pbsamadhannetcoreapi/Content/images/logoLabour.png";

                // Create an Image instance for the logo
                logoImage = Image.GetInstance(logoImagePath);
                logoImage.ScaleToFit(100, 100);  // Adjust the size as needed
                PdfPCell logoAfterCell = new PdfPCell(logoImage)
                {
                    Colspan = 1,
                    Rowspan = 2,
                    Border = 0,
                    Padding = 10,
                };
                ptableShopAnnexure.AddCell(logoAfterCell);
                #endregion

                PdfPCell emptyCell = new PdfPCell()
                {
                    Colspan = 3,  // Adjust the colspan based on your layout
                    Border = 0,
                };
                ptableShopAnnexure.AddCell(emptyCell);

                ptableShopAnnexure.AddCell(new PdfPCell(new Phrase("PART-1", verdanaHeader)) { Padding = 20, Colspan = 7, Border = 0, Left = 40 });
                ptableShopAnnexure.AddCell(new PdfPCell(new Phrase("To : " + application.ProjectSites.EstablishmentName, verdanaHeader)) { Padding = 20, Colspan = 7, Border = 0, Left = 40 });
                ptableShopAnnexure.AddCell(new PdfPCell(new Phrase("Address :" + application.ProjectSites.Address, verdanaHeader)) { Padding = 20, Colspan = 7, Border = 0, Left = 40 });
                ptableShopAnnexure.AddCell(new PdfPCell(new Phrase("DOF No :" + licenceNo, verdanaHeader)) { Padding = 20, Colspan = 7, Border = 0, Left = 40 });
                ptableShopAnnexure.AddCell(new PdfPCell(new Phrase("Dated :" + DateTime.Now, verdanaHeader)) { Padding = 20, Colspan = 7, Border = 0, Left = 40 });


                ptableShopAnnexure.AddCell(new PdfPCell(new Phrase((sno++).ToString(), verdana)) { Border = 1, BorderWidthTop = 1, BorderWidthBottom = 1, BorderWidthLeft = 1, BorderWidthRight = 1 });
                ptableShopAnnexure.AddCell(new PdfPCell(new Phrase("Building shall be constructed as per approved Building Plans/Service plans and in compliance with the Building Rules, 2021 as amended from time to time.", verdana)) { Border = 1, BorderWidthTop = 1, BorderWidthBottom = 1, BorderWidthRight = 1, Colspan = 6, Padding = 6 });

                ptableShopAnnexure.AddCell(new PdfPCell(new Phrase((sno++).ToString(), verdana)) { Border = 1, BorderWidthTop = 0, BorderWidthBottom = 1, BorderWidthLeft = 1, BorderWidthRight = 1 });
                ptableShopAnnexure.AddCell(new PdfPCell(new Phrase("The Applicant shall obtain any other permission required under any other Act/law/guidelines from any other department/Authority at his own level before, during and completion of construction at the site.", verdana)) { Border = 1, BorderWidthTop = 0, BorderWidthBottom = 1, BorderWidthRight = 1, Colspan = 6, Padding = 6 });

                ptableShopAnnexure.AddCell(new PdfPCell(new Phrase((sno++).ToString(), verdana)) { Border = 1, BorderWidthTop = 0, BorderWidthBottom = 1, BorderWidthLeft = 1, BorderWidthRight = 1 });
                ptableShopAnnexure.AddCell(new PdfPCell(new Phrase("Service plans shall be got approved from Competent Authority.", verdana)) { Border = 1, BorderWidthTop = 0, BorderWidthBottom = 1, BorderWidthRight = 1, Colspan = 6, Padding = 6 });

                ptableShopAnnexure.AddCell(new PdfPCell(new Phrase((sno++).ToString(), verdana)) { Border = 1, BorderWidthTop = 0, BorderWidthBottom = 1, BorderWidthLeft = 1, BorderWidthRight = 1 });
                ptableShopAnnexure.AddCell(new PdfPCell(new Phrase("Plinth level shall be obtained from Competent Authority and copy of that shall be submitted in the office of Director of Factories, Punjab.", verdana)) { Border = 1, BorderWidthTop = 0, BorderWidthBottom = 1, BorderWidthRight = 1, Colspan = 6, Padding = 6 });

                ptableShopAnnexure.AddCell(new PdfPCell(new Phrase((sno++).ToString(), verdana)) { Border = 1, BorderWidthTop = 0, BorderWidthBottom = 1, BorderWidthLeft = 1, BorderWidthRight = 1 });
                ptableShopAnnexure.AddCell(new PdfPCell(new Phrase("Conditions imposed by departments such as PPCB/Forest/Fire department/Defense/Irrigation/NHAI/PWD/AAI/HUD or any other department needs to be complied.", verdana)) { Border = 1, BorderWidthTop = 0, BorderWidthBottom = 1, BorderWidthRight = 1, Colspan = 6, Padding = 6 });

                ptableShopAnnexure.AddCell(new PdfPCell(new Phrase((sno++).ToString(), verdana)) { Border = 1, BorderWidthTop = 0, BorderWidthBottom = 1, BorderWidthLeft = 1, BorderWidthRight = 1 });
                ptableShopAnnexure.AddCell(new PdfPCell(new Phrase("The Applicant shall be responsible to adhere as per Chapter 4 of Fire and Life Safety of the National Building Code of India, 2016.", verdana)) { Border = 1, BorderWidthTop = 0, BorderWidthBottom = 1, BorderWidthRight = 1, Colspan = 6, Padding = 6 });

                ptableShopAnnexure.AddCell(new PdfPCell(new Phrase((sno++).ToString(), verdana)) { Border = 1, BorderWidthTop = 0, BorderWidthBottom = 1, BorderWidthLeft = 1, BorderWidthRight = 1 });
                ptableShopAnnexure.AddCell(new PdfPCell(new Phrase("The Applicant shall abide by the Action Plan issued by Government regarding monitoring of demolition and construction activity vide memo number: 17/20/2016-IHGI/1112 dated 02-03-2017.", verdana)) { Border = 1, BorderWidthTop = 0, BorderWidthBottom = 1, BorderWidthRight = 1, Colspan = 6, Padding = 6 });

                ptableShopAnnexure.AddCell(new PdfPCell(new Phrase((sno++).ToString(), verdana)) { Border = 1, BorderWidthTop = 0, BorderWidthBottom = 1, BorderWidthLeft = 1, BorderWidthRight = 1 });
                ptableShopAnnexure.AddCell(new PdfPCell(new Phrase("Completion and Occupancy Certificate as required under Building Bye-Laws, 2021 shall be taken from the Competent Authority.", verdana)) { Border = 1, BorderWidthTop = 0, BorderWidthBottom = 1, BorderWidthRight = 1, Colspan = 6, Padding = 6 });

                ptableShopAnnexure.AddCell(new PdfPCell(new Phrase((sno++).ToString(), verdana)) { Border = 1, BorderWidthTop = 0, BorderWidthBottom = 1, BorderWidthLeft = 1, BorderWidthRight = 1 });
                ptableShopAnnexure.AddCell(new PdfPCell(new Phrase("The provisions of Punjab Energy Conservation Code shall be applicable in accordance with notification number: 18/4/2016HGI/1856 dated 24-06-2016.", verdana)) { Border = 1, BorderWidthTop = 0, BorderWidthBottom = 1, BorderWidthRight = 1, Colspan = 6, Padding = 6 });

                ptableShopAnnexure.AddCell(new PdfPCell(new Phrase((sno++).ToString(), verdana)) { Border = 1, BorderWidthTop = 0, BorderWidthBottom = 1, BorderWidthLeft = 1, BorderWidthRight = 1 });
                ptableShopAnnexure.AddCell(new PdfPCell(new Phrase("The building shall be constructed as per structural drawings designed by registered Engineer/structural engineer.", verdana)) { Border = 1, BorderWidthTop = 0, BorderWidthBottom = 1, BorderWidthRight = 1, Colspan = 6, Padding = 6 });

                ptableShopAnnexure.AddCell(new PdfPCell(new Phrase((sno++).ToString(), verdana)) { Border = 1, BorderWidthTop = 0, BorderWidthBottom = 1, BorderWidthLeft = 1, BorderWidthRight = 1 });
                ptableShopAnnexure.AddCell(new PdfPCell(new Phrase("Construction shall be carried under the supervision of Engineer/Structural Engineer and Architect as per Form A.", verdana)) { Border = 1, BorderWidthTop = 0, BorderWidthBottom = 1, BorderWidthRight = 1, Colspan = 6, Padding = 6 });

                ptableShopAnnexure.AddCell(new PdfPCell(new Phrase((sno++).ToString(), verdana)) { Border = 1, BorderWidthTop = 0, BorderWidthBottom = 1, BorderWidthLeft = 1, BorderWidthRight = 1 });
                ptableShopAnnexure.AddCell(new PdfPCell(new Phrase("The Applicant shall be liable to pay the difference of amount, if any, found at any point of time by any department or concerned development Authority within 30 days from the date of demand.", verdana)) { Border = 1, BorderWidthTop = 0, BorderWidthBottom = 1, BorderWidthRight = 1, Colspan = 6, Padding = 6 });

                ptableShopAnnexure.AddCell(new PdfPCell(new Phrase((sno++).ToString(), verdana)) { Border = 1, BorderWidthTop = 0, BorderWidthBottom = 1, BorderWidthLeft = 1, BorderWidthRight = 1 });
                ptableShopAnnexure.AddCell(new PdfPCell(new Phrase("Green belt shall be developed and maintained as approved in the plans. The planting of trees of indigenous species shall be mandatory as per the provisions under PUPD Building Rules, 2021. Plantation shall be commenced once construction is started.", verdana)) { Border = 1, BorderWidthTop = 0, BorderWidthBottom = 1, BorderWidthRight = 1, Colspan = 6, Padding = 6 });

                ptableShopAnnexure.AddCell(new PdfPCell(new Phrase((sno++).ToString(), verdana)) { Border = 1, BorderWidthTop = 0, BorderWidthBottom = 1, BorderWidthLeft = 1, BorderWidthRight = 1 });
                ptableShopAnnexure.AddCell(new PdfPCell(new Phrase("Stability Certificate in Form I-F shall be got accepted as per Rule 4 of Punjab Factory Rules, 1952.", verdana)) { Border = 1, BorderWidthTop = 0, BorderWidthBottom = 1, BorderWidthRight = 1, Colspan = 6, Padding = 6 });


                ptableShopAnnexure.AddCell(new PdfPCell(new Phrase((sno++).ToString(), verdana)) { Border = 1, BorderWidthTop = 0, BorderWidthBottom = 1, BorderWidthLeft = 1, BorderWidthRight = 1 });
                ptableShopAnnexure.AddCell(new PdfPCell(new Phrase("15.	The Applicant shall ensure to get the registration/license under the Factories Act, 1948 and rules made thereunder.", verdana)) { Border = 1, BorderWidthTop = 0, BorderWidthBottom = 1, BorderWidthRight = 1, Colspan = 6, Padding = 6 });

                ptableShopAnnexure.AddCell(new PdfPCell(new Phrase((sno++).ToString(), verdana)) { Border = 1, BorderWidthTop = 0, BorderWidthBottom = 1, BorderWidthLeft = 1, BorderWidthRight = 1 });
                ptableShopAnnexure.AddCell(new PdfPCell(new Phrase("16.	The concerned owners of Factory/Partners/Directors/Proprietor shall be solely responsible for any kind of violations/ deviations/diversions made by them in the building/machinery plans etc.", verdana)) { Border = 1, BorderWidthTop = 0, BorderWidthBottom = 1, BorderWidthRight = 1, Colspan = 6, Padding = 6 });

                ptableShopAnnexure.AddCell(new PdfPCell(new Phrase((sno++).ToString(), verdana)) { Border = 1, BorderWidthTop = 0, BorderWidthBottom = 1, BorderWidthLeft = 1, BorderWidthRight = 1 });
                ptableShopAnnexure.AddCell(new PdfPCell(new Phrase("17.	The owner will ensure that all the precautions are taken regarding safety measures, such as fire safety and other provisions of the Factories Act 1948 and Punjab Factory Rules 1952.", verdana)) { Border = 1, BorderWidthTop = 0, BorderWidthBottom = 1, BorderWidthRight = 1, Colspan = 6, Padding = 6 });

                ptableShopAnnexure.AddCell(new PdfPCell(new Phrase((sno++).ToString(), verdana)) { Border = 1, BorderWidthTop = 0, BorderWidthBottom = 1, BorderWidthLeft = 1, BorderWidthRight = 1 });
                ptableShopAnnexure.AddCell(new PdfPCell(new Phrase("18.	The Applicant will also ensure proper facilities for the workers as per the Factories Act, 1948 and Punjab Factory Rules 1952.", verdana)) { Border = 1, BorderWidthTop = 0, BorderWidthBottom = 1, BorderWidthRight = 1, Colspan = 6, Padding = 6 });

                ptableShopAnnexure.AddCell(new PdfPCell(new Phrase((sno++).ToString(), verdana)) { Border = 1, BorderWidthTop = 0, BorderWidthBottom = 1, BorderWidthLeft = 1, BorderWidthRight = 1 });
                ptableShopAnnexure.AddCell(new PdfPCell(new Phrase("19.	The Site for which building plans have been approved shall be registered under The Building and Other Construction Workers Act, 1996.", verdana)) { Border = 1, BorderWidthTop = 0, BorderWidthBottom = 1, BorderWidthRight = 1, Colspan = 6, Padding = 6 });

                ptableShopAnnexure.AddCell(new PdfPCell(new Phrase((sno++).ToString(), verdana)) { Border = 1, BorderWidthTop = 0, BorderWidthBottom = 1, BorderWidthLeft = 1, BorderWidthRight = 1 });
                ptableShopAnnexure.AddCell(new PdfPCell(new Phrase("20.	The Applicant will ensure that all workers shall be employed in construction activity shall be registered as “BENEFICIARY” with Punjab Building and Other Construction Workers Welfare Board.", verdana)) { Border = 1, BorderWidthTop = 0, BorderWidthBottom = 1, BorderWidthRight = 1, Colspan = 6, Padding = 6 });

                ptableShopAnnexure.AddCell(new PdfPCell(new Phrase((sno++).ToString(), verdana)) { Border = 1, BorderWidthTop = 0, BorderWidthBottom = 1, BorderWidthLeft = 1, BorderWidthRight = 1 });
                ptableShopAnnexure.AddCell(new PdfPCell(new Phrase("21.	Once manufacturing activity is commenced in factory the occupier of factory shall ensure that Labour Welfare Fund is contributed to Punjab Labour Welfare Board as per provisions of Punjab Labour Welfare Fund Act, 1965 and rules made there under so that they can apply for the various Welfare schemes.", verdana)) { Border = 1, BorderWidthTop = 0, BorderWidthBottom = 1, BorderWidthRight = 1, Colspan = 6, Padding = 6 });

                ptableShopAnnexure.AddCell(new PdfPCell(new Phrase((sno++).ToString(), verdana)) { Border = 1, BorderWidthTop = 0, BorderWidthBottom = 1, BorderWidthLeft = 1, BorderWidthRight = 1 });
                ptableShopAnnexure.AddCell(new PdfPCell(new Phrase("22.	Occupant load shall not exceed as per the undertaking. In case there will be a change in occupant load revised building plans shall be got approved from the Competent Authority.", verdana)) { Border = 1, BorderWidthTop = 0, BorderWidthBottom = 1, BorderWidthRight = 1, Colspan = 6, Padding = 6 });

                ptableShopAnnexure.AddCell(new PdfPCell(new Phrase((sno++).ToString(), verdana)) { Border = 1, BorderWidthTop = 0, BorderWidthBottom = 1, BorderWidthLeft = 1, BorderWidthRight = 1 });
                ptableShopAnnexure.AddCell(new PdfPCell(new Phrase("Stability Certificate in Form I-F shall be got accepted as per Rule 4 of Punjab Factory Rules, 1952.", verdana)) { Border = 1, BorderWidthTop = 0, BorderWidthBottom = 1, BorderWidthRight = 1, Colspan = 6, Padding = 6 });

                ptableShopAnnexure.AddCell(new PdfPCell(new Phrase((sno++).ToString(), verdana)) { Border = 1, BorderWidthTop = 0, BorderWidthBottom = 1, BorderWidthLeft = 1, BorderWidthRight = 1 });
                ptableShopAnnexure.AddCell(new PdfPCell(new Phrase("Stability Certificate in Form I-F shall be got accepted as per Rule 4 of Punjab Factory Rules, 1952.", verdana)) { Border = 1, BorderWidthTop = 0, BorderWidthBottom = 1, BorderWidthRight = 1, Colspan = 6, Padding = 6 });

                ptableShopAnnexure.AddCell(new PdfPCell(new Phrase((sno++).ToString(), verdana)) { Border = 1, BorderWidthTop = 0, BorderWidthBottom = 1, BorderWidthLeft = 1, BorderWidthRight = 1 });
                ptableShopAnnexure.AddCell(new PdfPCell(new Phrase("Stability Certificate in Form I-F shall be got accepted as per Rule 4 of Punjab Factory Rules, 1952.", verdana)) { Border = 1, BorderWidthTop = 0, BorderWidthBottom = 1, BorderWidthRight = 1, Colspan = 6, Padding = 6 });

                ptableShopAnnexure.AddCell(new PdfPCell(new Phrase((sno++).ToString(), verdana)) { Border = 1, BorderWidthTop = 0, BorderWidthBottom = 1, BorderWidthLeft = 1, BorderWidthRight = 1 });
                ptableShopAnnexure.AddCell(new PdfPCell(new Phrase("Stability Certificate in Form I-F shall be got accepted as per Rule 4 of Punjab Factory Rules, 1952.", verdana)) { Border = 1, BorderWidthTop = 0, BorderWidthBottom = 1, BorderWidthRight = 1, Colspan = 6, Padding = 6 });

                ptableShopAnnexure.AddCell(new PdfPCell(new Phrase((sno++).ToString(), verdana)) { Border = 1, BorderWidthTop = 0, BorderWidthBottom = 1, BorderWidthLeft = 1, BorderWidthRight = 1 });
                ptableShopAnnexure.AddCell(new PdfPCell(new Phrase("Stability Certificate in Form I-F shall be got accepted as per Rule 4 of Punjab Factory Rules, 1952.", verdana)) { Border = 1, BorderWidthTop = 0, BorderWidthBottom = 1, BorderWidthRight = 1, Colspan = 6, Padding = 6 });

                ptableShopAnnexure.AddCell(new PdfPCell(new Phrase((sno++).ToString(), verdana)) { Border = 1, BorderWidthTop = 0, BorderWidthBottom = 1, BorderWidthLeft = 1, BorderWidthRight = 1 });
                ptableShopAnnexure.AddCell(new PdfPCell(new Phrase("Stability Certificate in Form I-F shall be got accepted as per Rule 4 of Punjab Factory Rules, 1952.", verdana)) { Border = 1, BorderWidthTop = 0, BorderWidthBottom = 1, BorderWidthRight = 1, Colspan = 6, Padding = 6 });

                ptableShopAnnexure.AddCell(new PdfPCell(new Phrase((sno++).ToString(), verdana)) { Border = 1, BorderWidthTop = 0, BorderWidthBottom = 1, BorderWidthLeft = 1, BorderWidthRight = 1 });
                ptableShopAnnexure.AddCell(new PdfPCell(new Phrase("Stability Certificate in Form I-F shall be got accepted as per Rule 4 of Punjab Factory Rules, 1952.", verdana)) { Border = 1, BorderWidthTop = 0, BorderWidthBottom = 1, BorderWidthRight = 1, Colspan = 6, Padding = 6 });

                ptableShopAnnexure.AddCell(new PdfPCell(new Phrase((sno++).ToString(), verdana)) { Border = 1, BorderWidthTop = 0, BorderWidthBottom = 1, BorderWidthLeft = 1, BorderWidthRight = 1 });
                ptableShopAnnexure.AddCell(new PdfPCell(new Phrase("Stability Certificate in Form I-F shall be got accepted as per Rule 4 of Punjab Factory Rules, 1952.", verdana)) { Border = 1, BorderWidthTop = 0, BorderWidthBottom = 1, BorderWidthRight = 1, Colspan = 6, Padding = 6 });

                ptableShopAnnexure.AddCell(new PdfPCell(new Phrase((sno++).ToString(), verdana)) { Border = 1, BorderWidthTop = 0, BorderWidthBottom = 1, BorderWidthLeft = 1, BorderWidthRight = 1 });
                ptableShopAnnexure.AddCell(new PdfPCell(new Phrase("Stability Certificate in Form I-F shall be got accepted as per Rule 4 of Punjab Factory Rules, 1952.", verdana)) { Border = 1, BorderWidthTop = 0, BorderWidthBottom = 1, BorderWidthRight = 1, Colspan = 6, Padding = 6 });





                documents.Add(ptableShopAnnexure);
                documents.NewPage();
                PdfPTable ptableShopDetailLicenceAppliedFormB = new PdfPTable(3);
                ptableShopDetailLicenceAppliedFormB.DefaultCell.Border = 0;
                ptableShopDetailLicenceAppliedFormB.DefaultCell.Padding = 2;
                ptableShopDetailLicenceAppliedFormB.SpacingBefore = 1000;
                documents.Add(ptableShopDetailLicenceAppliedFormB);
                documents.Close();
                PDFPageEvents pageeventsobj = new PDFPageEvents();
            }
            catch (Exception ex)
            {
                genericFormModel.HasException = true;
                genericFormModel.Exceptions = ex.Message;
            }
            return genericFormModel;
        }
        private async Task<CertificateGenerateServiceResultTemplate> Certificate_AppForm_6(Int64 appRefId, string CertificatePerformaURL, ApplicationTypeEnum applicationType, string userName, string licenceNo, bool isDeemedCalling)
        {
            CertificateGenerateServiceResultTemplate genericFormModel = new CertificateGenerateServiceResultTemplate();
            try
            {
                Guid guId = Guid.NewGuid();
                genericFormModel.PdfNameGUID = guId.ToString();
                genericFormModel.HasException = false;
                var SaveDirectoryPath = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "TempFiles")).Root + guId.ToString() + ".pdf";
                iTextSharp.text.Document documents = new iTextSharp.text.Document(iTextSharp.text.PageSize.A4, 5, 5, 30, 30);

                #region ClassObjects
                pdfFonts classFonts = new pdfFonts();
                #endregion

                #region CreateObjects
                PdfWriter owriter = PdfWriter.GetInstance(documents, new FileStream(SaveDirectoryPath, FileMode.Create));
                PDFPageEvents pageEvent = new PDFPageEvents();
                owriter.PageEvent = pageEvent;
                owriter.SetEncryption(PdfWriter.STRENGTH128BITS, "", "", PdfWriter.AllowPrinting);
                #endregion

                #region Regester Font Style
                var fontName = "Alice";
                //if (!FontFactory.IsRegistered(fontName))
                //{
                //    var fontPath = "Content/fonts/Alice-Regular.ttf";
                //    Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
                //    FontFactory.Register(fontPath, fontName);
                //}
                Font font = FontFactory.GetFont(fontName, BaseFont.IDENTITY_H, 16);
                Font fontSmall = FontFactory.GetFont(fontName, BaseFont.IDENTITY_H, 12);
                Font fontBOLD = FontFactory.GetFont(fontName, BaseFont.IDENTITY_H, 16, iTextSharp.text.Font.BOLD);
                //BaseFont customfontimpact = BaseFont.CreateFont(fontName, BaseFont.CP1252, BaseFont.EMBEDDED);
                #endregion



                var processshop = _context.ShopLicence_GeneralDetails.Where(x => x.AppRefId == appRefId).FirstOrDefault();
                var application = _context.Applications.Where(x => x.AppId == appRefId && x.IsDeleted == false).FirstOrDefault();
                var projectSite = _context.ProjectSiteLogs.Where(x => x.ProjectSiteRefId == application.ProjectSiteRefId && x.ProjectSiteVersion == application.ProjectSiteVersion).FirstOrDefault();
                var labourCircle = _context.LabourCircles.Where(x => x.LabourCircleId == projectSite.LabourCircleRefId).FirstOrDefault();
                var employee = _context.ShopLicence_EmployeeDetails.Where(x => x.ShopLicenceRefId == processshop.ShopLicenceId).Count();
                var applicationAction = _context.ApplicationActions.Where(x => x.ApplicationRefId == application.AppId).FirstOrDefault();
                var tehsilName = _context.Tehsils.Where(x => x.TehsilLgdId == projectSite.TehsilRefId).Select(x => x.TehsilName).FirstOrDefault();
                var districtName = _context.Districts.Where(x => x.DistrictLgdId == projectSite.DistrictRefId).Select(x => x.DistrictName).FirstOrDefault();

                documents.Open();
                Font verdanaHeader = FontFactory.GetFont("Arial", 9, Font.BOLD);
                Font verdana = FontFactory.GetFont("Arial", 9, BaseColor.BLACK);
                Font verdanaBoldItalic = FontFactory.GetFont("Arial", 8, Font.BOLDITALIC);
                Font verdanasmall = FontFactory.GetFont("Arial", 8, Font.NORMAL);
                verdanasmall.Color = BaseColor.WHITE;
                Font verdanaRightHeader = FontFactory.GetFont("Arial", 15, Font.BOLD);
                verdanaRightHeader.Color = BaseColor.WHITE;
                Font verdanaRightHeader1 = FontFactory.GetFont("Arial", 12, Font.BOLD);
                verdanaRightHeader1.Color = BaseColor.WHITE;
                Font verdanaRightHeader2 = FontFactory.GetFont("Arial", 9, Font.BOLD);
                verdanaRightHeader2.Color = BaseColor.WHITE;
                Font monotype = FontFactory.GetFont("Arial", 13, Font.BOLD);
                Font monotypeHeader = FontFactory.GetFont("Arial", 15, Font.BOLD);
                Font impact = new Font(fontSmall.BaseFont, 12);
                Font impactHeader = new Font(fontSmall.BaseFont, 12);
                impactHeader.Color = BaseColor.WHITE;
                BaseFont bfTimes = BaseFont.CreateFont(BaseFont.TIMES_ROMAN, BaseFont.CP1252, false);
                Font timesRoman = FontFactory.GetFont("Arial", 10, BaseColor.BLACK);
                Font verdanaBold = FontFactory.GetFont("Arial", 9, Font.BOLD);
                iTextSharp.text.Image pageLogo1;

                var shopPerforma = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "LicensePerformas")).Root + "CertificatePerforma_Shop.jpg";
                pageLogo1 = iTextSharp.text.Image.GetInstance(new FileStream(shopPerforma, FileMode.OpenOrCreate, FileAccess.Read));
                //pageLogo1 = iTextSharp.text.Image.GetInstance("~/Content/images/CertificatePerforma_Shop.jpg");
                pageLogo1.Alignment = iTextSharp.text.Image.ALIGN_CENTER | iTextSharp.text.Image.UNDERLYING;
                pageLogo1.ScaleToFit(800f, 800f);
                pageLogo1.SetAbsolutePosition(10f, 25f);
                pageLogo1.BorderWidth = 1;
                documents.Add(pageLogo1);
                PdfPTable ptableleftSideDetail = new PdfPTable(1);
                ptableleftSideDetail.DefaultCell.Border = 0;
                ptableleftSideDetail.DefaultCell.Padding = 0;
                ptableleftSideDetail.HorizontalAlignment = (Element.ALIGN_JUSTIFIED);
                ptableleftSideDetail.DefaultCell.VerticalAlignment = (Element.ALIGN_JUSTIFIED);
                ptableleftSideDetail.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
                ptableleftSideDetail.AddCell(new PdfPCell(new Phrase("", verdanaHeader)) { Border = 0, PaddingTop = 320 });
                ptableleftSideDetail.AddCell(new PdfPCell(new Phrase("DESCRIPTION OF THE LICENCED  PREMISES ", verdanaRightHeader1)) { Border = 0, PaddingTop = 8, PaddingLeft = -15 });
                ptableleftSideDetail.AddCell(new PdfPCell(new Phrase("Licence No : ", verdanaRightHeader1)) { Border = 0, PaddingTop = 8, PaddingLeft = -15 });
                ptableleftSideDetail.AddCell(new PdfPCell(new Phrase("Department of Labour", verdanaRightHeader1)) { Border = 0, PaddingTop = 195, PaddingLeft = -15 });
                ptableleftSideDetail.AddCell(new PdfPCell(new Phrase("      Govt. of Punjab ", verdanaRightHeader1)) { Border = 0, PaddingTop = 0, PaddingLeft = -15 });
                PdfPTable ptableleftSideDetailFormF = new PdfPTable(1);
                ptableleftSideDetailFormF.DefaultCell.Border = 0;
                ptableleftSideDetailFormF.DefaultCell.Padding = 0;
                ptableleftSideDetailFormF.HorizontalAlignment = (Element.ALIGN_JUSTIFIED);
                ptableleftSideDetailFormF.DefaultCell.VerticalAlignment = (Element.ALIGN_JUSTIFIED);
                ptableleftSideDetailFormF.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
                PdfPTable tblCurrLicDetail = new PdfPTable(1);
                Font verdanaBoldHeader = FontFactory.GetFont("Arial", 9, Font.BOLD);
                verdanaBoldHeader.Color = BaseColor.WHITE;
                tblCurrLicDetail.AddCell(new PdfPCell(new Phrase("", verdanaBoldHeader)) { Border = 0 });

                tblCurrLicDetail.AddCell(new PdfPCell(new Phrase("Reg. No : " + licenceNo, verdanaBoldHeader)) { Border = 0, PaddingTop = 8 });
                tblCurrLicDetail.AddCell(new PdfPCell(new Phrase("Issued Date : " + DateTime.Now.Day.ToString() + "-" + DateTime.Now.Date.ToString("MMM") + "-" + DateTime.Now.Year.ToString(), verdanaBoldHeader)) { Border = 0, PaddingTop = 8 });
                tblCurrLicDetail.AddCell(new PdfPCell(new Phrase("File No: " + application.PublicAppRefNum, verdanaBoldHeader)) { Border = 0, PaddingTop = 8, PaddingBottom = 8 });

                ptableleftSideDetailFormF.AddCell(new PdfPCell(tblCurrLicDetail) { Border = 0, FixedHeight = 100 });

                #region QR Code Generate
                var qrCodeDetails = "Establishment Name : " + projectSite.EstablishmentName + "|" + "Establishment Address : " + projectSite.Address + "|" + "IPIN : " + application.InvestPunjab_Ipin + "|" + "Contact No : " + projectSite.ContactPersonFirstName + " " + projectSite.ContactPersonLastName + "|" + projectSite.ContactPersonMobileNo + "|" + "Licence No : " + licenceNo + "|" + "File No : " + application.PublicAppRefNum;
                var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(application.PublicAppRefNum);
                string base64String = System.Convert.ToBase64String(plainTextBytes);
                QRCodeGenerator qrGenerator = new QRCodeGenerator();
                QRCodeGenerator.QRCode qrCode = qrGenerator.CreateQrCode(qrCodeDetails, QRCodeGenerator.ECCLevel.Q);

                using (System.Drawing.Bitmap bitMap = qrCode.GetGraphic(20))
                {
                    using (MemoryStream ms = new MemoryStream())
                    {
                        bitMap.Save(ms, ImageFormat.Jpeg);
                        System.Drawing.Image imgQRCode = System.Drawing.Image.FromStream(ms);
                        iTextSharp.text.Image imdf = iTextSharp.text.Image.GetInstance(imgQRCode, System.Drawing.Imaging.ImageFormat.Jpeg);
                        imdf.ScaleAbsoluteHeight(150f);
                        imdf.ScaleAbsoluteWidth(150f);
                        ptableleftSideDetailFormF.AddCell(new PdfPCell(imdf) { Border = 0, PaddingTop = 10, PaddingRight = 5 });
                    }
                }

                #endregion

                ptableleftSideDetailFormF.AddCell(new PdfPCell(new Phrase("Department of Labour", verdanaRightHeader1)) { Border = 0, PaddingTop = 120, PaddingLeft = 10 });
                ptableleftSideDetailFormF.AddCell(new PdfPCell(new Phrase("Govt. of Punjab ", verdanaRightHeader1)) { Border = 0, PaddingTop = 0, PaddingLeft = 25 });

                #region Officer Signature

                var userDetails = await _iAuthService.GetUserFullDetailByUserName(userName);
                string officerSigFilePath = "";
                if (!isDeemedCalling)
                {
                    if (userDetails.ListData.Count() == 0 || userDetails.ListData[0].Signature == null)
                    {
                        userDetails.ListData[0].Signature = "abc.jpg";
                    }
                    officerSigFilePath = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "OfficerSignatures")).Root + userDetails.ListData[0].Signature;
                    //pageLogo1 = iTextSharp.text.Image.GetInstance(new FileStream(officerSigFilePath, FileMode.OpenOrCreate, FileAccess.Read));
                }
                else
                {
                    officerSigFilePath = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "OfficerSignatures")).Root + "deemed_engine_signature.jpg";
                }
                iTextSharp.text.Image signature = iTextSharp.text.Image.GetInstance(new FileStream(officerSigFilePath, FileMode.OpenOrCreate, FileAccess.Read));
                signature.ScaleToFit(90f, 900f);
                signature.SetAbsolutePosition(630f, 150f);
                documents.Add(signature);

                var empSig = _context.ApplicationDocuments.Where(x => x.AppRefId == appRefId && x.DocumentRefId == 20005).OrderByDescending(x => x.AppDocId).FirstOrDefault();
                string empSignatureFileName = "abc.jpg";
                if (empSig != null)
                {
                    empSignatureFileName = empSig.AttachmentName;
                }

                var employerSigFilePath = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "AppFiles")).Root + empSignatureFileName;
                //pageLogo1 = iTextSharp.text.Image.GetInstance(new FileStream(officerSigFilePath, FileMode.OpenOrCreate, FileAccess.Read));

                iTextSharp.text.Image employerSignature = iTextSharp.text.Image.GetInstance(new FileStream(employerSigFilePath, FileMode.OpenOrCreate, FileAccess.Read));
                employerSignature.ScaleToFit(90f, 90f);
                employerSignature.SetAbsolutePosition(630f, 150f);
                documents.Add(employerSignature);


                #endregion

                #region Officer Name & Designation
                #endregion

                #region Shop Form F Reports Detail

                PdfPTable ptableShopFormFDetail = new PdfPTable(4);
                ptableShopFormFDetail.DefaultCell.Border = 1;
                ptableShopFormFDetail.DefaultCell.Padding = 2;
                PdfPCell pFormFDetailcellLine1 = new PdfPCell(new Paragraph(""));
                pFormFDetailcellLine1.BorderWidth = 0;
                pFormFDetailcellLine1.Colspan = 4;
                ptableShopFormFDetail.AddCell(pFormFDetailcellLine1);
                ptableShopFormFDetail.AddCell(new PdfPCell(new Phrase("FORM F", verdanaHeader)) { Border = 0, PaddingLeft = 10, PaddingTop = 35, HorizontalAlignment = PdfPCell.ALIGN_CENTER, Colspan = 4 });

                ptableShopFormFDetail.AddCell(new PdfPCell(new Phrase("REGISTRATION CERTIFICATE OF SHOPS & COMMERCIAL ESTABLISHMENTS UNDER THE PUNJAB SHOPS AND COMMERCIAL ESTABLISHMENTS ACT, 1958", impactHeader)) { Border = 0, PaddingTop = 30, PaddingBottom = 20, PaddingLeft = 25, PaddingRight = 25, VerticalAlignment = PdfPCell.ALIGN_MIDDLE, HorizontalAlignment = PdfPCell.ALIGN_CENTER, Colspan = 4 });

                ptableShopFormFDetail.AddCell(new PdfPCell(new Phrase("", verdanaHeader)) { Border = 0, PaddingLeft = 20, HorizontalAlignment = PdfPCell.ALIGN_CENTER, Colspan = 4 });

                ptableShopFormFDetail.AddCell(new PdfPCell(new Phrase("Statement of Registration of Establishment under Section 13 of", verdanaHeader)) { Border = 0, PaddingLeft = 20, HorizontalAlignment = PdfPCell.ALIGN_CENTER, Colspan = 4 });

                ptableShopFormFDetail.AddCell(new PdfPCell(new Phrase("The Punjab Shops and Commercial Establishments Act,1958", verdanaHeader)) { Border = 0, PaddingLeft = 20, HorizontalAlignment = PdfPCell.ALIGN_CENTER, Colspan = 4 });
                ptableShopFormFDetail.AddCell(new PdfPCell(new Phrase("[Rule 13 of the Punjab Shops and Commercial Establishments Rules, 1958]", verdanaHeader)) { Border = 0, PaddingLeft = 20, HorizontalAlignment = PdfPCell.ALIGN_CENTER, Colspan = 4 });
                ptableShopFormFDetail.AddCell(new PdfPCell(new Phrase("To", verdana)) { Border = 0, Colspan = 3, PaddingTop = 20 });
                ptableShopFormFDetail.AddCell(new PdfPCell(new Phrase("", verdana)) { Border = 0 });
                ptableShopFormFDetail.AddCell(new PdfPCell(new Phrase("The Inspector of Shops and Commercial Establishments,", verdana)) { Border = 0, Colspan = 3 });
                ptableShopFormFDetail.AddCell(new PdfPCell(new Phrase("", verdana)) { Border = 0 });
                ptableShopFormFDetail.AddCell(new PdfPCell(new Phrase(labourCircle.LabourCircleName, verdana)) { Border = 0, Colspan = 3 });
                ptableShopFormFDetail.AddCell(new PdfPCell(new Phrase("", verdana)) { Border = 0, Colspan = 3 });

                PdfPCell para = new PdfPCell(new Paragraph("I hereby submit this statement for registration of my establishment . The information furnished here under is correct to the best of my knowledge.", verdana));
                para.BorderWidth = 0;
                para.Colspan = 4;
                ptableShopFormFDetail.AddCell(para);

                ptableShopFormFDetail.AddCell(new PdfPCell(new Phrase("1. Name and Father's/Husband's Name of Employer:", verdanaHeader)) { Border = 0, Colspan = 2, Padding = 6 });
                ptableShopFormFDetail.AddCell(new PdfPCell(new Phrase(processshop.OwnerName + " S/o, D/o, W/o: " + processshop.OwnerFatherOrHusbandName, verdana)) { Border = 0, Colspan = 2, Padding = 6 });

                ptableShopFormFDetail.AddCell(new PdfPCell(new Phrase("2. Name of Manager , if any :", verdanaHeader)) { Border = 0, Colspan = 2, Padding = 6 });

                ptableShopFormFDetail.AddCell(new PdfPCell(new Phrase(processshop.ManagerName, verdana)) { Border = 0, Padding = 6, Colspan = 2 });


                ptableShopFormFDetail.AddCell(new PdfPCell(new Phrase("3. Name of the Establishment : ", verdanaHeader)) { Border = 0, Colspan = 2, Padding = 6 });
                ptableShopFormFDetail.AddCell(new PdfPCell(new Phrase(projectSite.EstablishmentName, verdana)) { Border = 0, Colspan = 2, Padding = 6 });

                ptableShopFormFDetail.AddCell(new PdfPCell(new Phrase("4. Full Postal Address of the Establishment :", verdanaHeader)) { Border = 0, Colspan = 2, Padding = 6 });
                //ptableShopFormFDetail.AddCell(new PdfPCell(new Phrase(projectSite.Address + ", " + projectSite.TehsilRefId + ", " + projectSite.PinCode, verdana)) { Border = 0, Colspan = 2, Padding = 6 });
                ptableShopFormFDetail.AddCell(new PdfPCell(new Phrase(projectSite.Address + ", " + tehsilName + ", " + districtName + ", " + projectSite.PinCode, verdana)) { Border = 0, Colspan = 2, Padding = 6 });
                ptableShopFormFDetail.AddCell(new PdfPCell(new Phrase("5.1: Nature of Business ", verdanaHeader)) { Border = 0, Padding = 6, Colspan = 2 });
                ptableShopFormFDetail.AddCell(new PdfPCell(new Phrase("5.2: Project Purpose :", verdanaHeader)) { Border = 0, Padding = 6, Colspan = 2 });

                if (processshop.NationalIndustrialClassificationCode != null)
                {
                    string input = processshop.NationalIndustrialClassificationCode;
                    List<string> values = new List<string>();
                    int startIndex = 0;
                    string nicCode = "";
                    string nicCodeDescription = "";

                    while (startIndex < input.Length)
                    {
                        int openQuoteIndex = input.IndexOf('"', startIndex);
                        if (openQuoteIndex == -1)
                            break;

                        int closeQuoteIndex = input.IndexOf('"', openQuoteIndex + 1);
                        if (closeQuoteIndex == -1)
                            break;

                        int valueLength = closeQuoteIndex - openQuoteIndex - 1;
                        if (valueLength > 0)
                        {
                            string extractedValue = input.Substring(openQuoteIndex + 1, valueLength);
                            values.Add(extractedValue);
                        }

                        startIndex = closeQuoteIndex + 1;
                    }
                    // Printing the extracted values
                    foreach (string value in values)
                    {
                        nicCode = value;
                        nicCodeDescription = _context.NICCodes.Where(n => n.NicCode == nicCode).Select(x => x.Description).FirstOrDefault();
                    }

                    if (nicCode != null)
                    {
                        ptableShopFormFDetail.AddCell(new PdfPCell(new Phrase(nicCode + "-" + nicCodeDescription, verdana)) { Border = 0, Padding = 6, Colspan = 2 });
                        ptableShopFormFDetail.AddCell(new PdfPCell(new Phrase(projectSite.ProjectPurpose, verdana)) { Border = 0, Padding = 6, Colspan = 2 });
                    }
                }

                ptableShopFormFDetail.AddCell(new PdfPCell(new Phrase("6. No. of Employees if any:", verdanaHeader)) { Border = 0, Colspan = 2, Padding = 6 });
                ptableShopFormFDetail.AddCell(new PdfPCell(new Phrase(employee.ToString(), verdana)) { Border = 0, Colspan = 2, Padding = 6 });

                ptableShopFormFDetail.AddCell(new PdfPCell(new Phrase("7. No and Date of Previous Registration Certificate surrendered:", verdanaHeader)) { Border = 0, Colspan = 2, Padding = 6 });
                ptableShopFormFDetail.AddCell(new PdfPCell(new Phrase("-----", verdana)) { Border = 0, Colspan = 2, Padding = 6 });

                ptableShopFormFDetail.AddCell(new PdfPCell(new Phrase("8. Date:", verdanaHeader)) { Border = 0, Colspan = 2, Padding = 6 });
                ptableShopFormFDetail.AddCell(new PdfPCell(new Phrase(DateTime.Now.ToString(), verdana)) { Border = 0, Colspan = 2, Padding = 6 });
                string ExpireOn = "";

                ExpireOn = "The Establishment mentioned above is hereby registered under Reg. No. " + application.PublicAppRefNum;
                ptableShopFormFDetail.AddCell(new PdfPCell(new Phrase(ExpireOn, verdana)) { Border = 0, Colspan = 3 });
                ptableShopFormFDetail.AddCell(new PdfPCell(new Phrase("", verdana)) { Border = 0, Colspan = 3 });

                ptableShopFormFDetail.AddCell(new PdfPCell(signature) { Border = 0, Colspan = 2, PaddingTop = 40 });
                ptableShopFormFDetail.AddCell(new PdfPCell(employerSignature) { Border = 0, Colspan = 2, PaddingTop = 40, HorizontalAlignment = PdfPCell.ALIGN_RIGHT });

                if (isDeemedCalling)
                {
                    ptableShopFormFDetail.AddCell(new PdfPCell(new Phrase("Labour Inspector", verdanaHeader)) { Border = 0, Colspan = 2 });
                }
                else
                {
                    ptableShopFormFDetail.AddCell(new PdfPCell(new Phrase("Signature of Inspector", verdanaHeader)) { Border = 0, Colspan = 2 });
                }

                ptableShopFormFDetail.AddCell(new PdfPCell(new Phrase("Signature of Employer", verdanaHeader)) { Border = 0, Colspan = 2, HorizontalAlignment = PdfPCell.ALIGN_RIGHT });

                ptableShopFormFDetail.AddCell(new PdfPCell(new Phrase("", verdana)) { Border = 0, Colspan = 4 });
                ptableShopFormFDetail.AddCell(new PdfPCell(new Phrase(userDetails.ListData[0].UserFullName, verdana)) { Border = 0, Colspan = 4 });
                ptableShopFormFDetail.AddCell(new PdfPCell(new Phrase("", verdana)) { Border = 0, Colspan = 4 });
                ptableShopFormFDetail.AddCell(new PdfPCell(new Phrase("Shop and Commercial Establishment", verdana)) { Border = 0, Colspan = 4 });
                ptableShopFormFDetail.AddCell(new PdfPCell(new Phrase("", verdana)) { Border = 0, Colspan = 4 });
                ptableShopFormFDetail.AddCell(new PdfPCell(new Phrase(labourCircle.LabourCircleName, verdana)) { Border = 0, Colspan = 4 });
                ptableShopFormFDetail.AddCell(new PdfPCell(new Phrase("", verdana)) { Border = 0, Colspan = 4 });





                #endregion

                #region Shop Form B Reports Detail
                PdfPTable ptableShopDetail = new PdfPTable(4);
                ptableShopDetail.DefaultCell.Border = 0;
                ptableShopDetail.DefaultCell.Padding = 2;
                PdfPCell pDetailcellLine1 = new PdfPCell(new Paragraph(""));
                pDetailcellLine1.BorderWidth = 0;
                pDetailcellLine1.Colspan = 4;
                ptableShopDetail.AddCell(pDetailcellLine1);
                ptableShopDetail.AddCell(new PdfPCell(new Phrase("FORM B", verdanaHeader)) { Border = 0, PaddingLeft = 10, PaddingTop = 35, HorizontalAlignment = PdfPCell.ALIGN_CENTER, Colspan = 4 });

                ptableShopDetail.AddCell(new PdfPCell(new Phrase("REGISTRATION CERTIFICATE OF SHOPS & COMMERCIAL ESTABLISHMENTS UNDER THE PUNJAB SHOPS AND COMMERCIAL ESTABLISHMENTS ACT, 1958", impactHeader)) { Border = 0, PaddingTop = 30, PaddingBottom = 20, PaddingLeft = 25, PaddingRight = 25, VerticalAlignment = PdfPCell.ALIGN_MIDDLE, HorizontalAlignment = PdfPCell.ALIGN_CENTER, Colspan = 4 });

                ptableShopDetail.AddCell(new PdfPCell(new Phrase("", verdanaHeader)) { Border = 0, PaddingLeft = 20, HorizontalAlignment = PdfPCell.ALIGN_CENTER, Colspan = 4 });
                ptableShopDetail.AddCell(new PdfPCell(new Phrase("Notice to be exhibited under the section 20(1) of the Shops and Commercial Establishments Act, 1958", verdanaHeader)) { Border = 0, PaddingLeft = 20, HorizontalAlignment = PdfPCell.ALIGN_CENTER, PaddingTop = 10, Colspan = 4 });
                ptableShopDetail.AddCell(new PdfPCell(new Phrase("The Punjab Shops and Commercial Establishments Act,1958", verdanaHeader)) { Border = 0, PaddingLeft = 20, HorizontalAlignment = PdfPCell.ALIGN_CENTER, Colspan = 4 });
                ptableShopDetail.AddCell(new PdfPCell(new Phrase("[Rule 4 of the Punjab Shops and Commercial Establishments Rules, 1958]", verdanaHeader)) { Border = 0, PaddingLeft = 0, HorizontalAlignment = PdfPCell.ALIGN_CENTER, Colspan = 4 });
                ptableShopDetail.AddCell(new PdfPCell(new Phrase(" ", verdana)) { Border = 0, Colspan = 4 });
                ptableShopDetail.AddCell(new PdfPCell(new Phrase("1. Close Day, if any:", verdanaBold)) { Border = 0, Padding = 6, Colspan = 2 });
                ptableShopDetail.AddCell(new PdfPCell(new Phrase(EnumOps.GetEnumDescriptionName<ClosingDayTypeEnum>(processshop.ClosingDay.ToString()), verdana)) { Border = 0, Padding = 6, Colspan = 2 });
                ptableShopDetail.AddCell(new PdfPCell(new Phrase("2. Opening Hours of the Establishment: ", verdanaBold)) { Border = 0, Padding = 6, Colspan = 2 });
                ptableShopDetail.AddCell(new PdfPCell(new Phrase(Convert24HrsTo12Hrs(processshop.OpeningHoursOfEstablishment), verdana)) { Border = 0, Padding = 6, Colspan = 2 });
                ptableShopDetail.AddCell(new PdfPCell(new Phrase("2.1. Closing Hours of the Establishment: ", verdanaBold)) { Border = 0, Padding = 6, Colspan = 2 });
                ptableShopDetail.AddCell(new PdfPCell(new Phrase(Convert24HrsTo12Hrs(processshop.ClosingHoursOfEstablishment), verdana)) { Border = 0, Padding = 6, Colspan = 2 });
                ptableShopDetail.AddCell(new PdfPCell(new Phrase("3. Name and Father's/Husband's Name of Employee:", verdanaBold)) { Border = 0, Padding = 6, Colspan = 2 });
                ptableShopDetail.AddCell(new PdfPCell(new Phrase(processshop.OwnerName + " D/o,S/o,W/o: " + processshop.OwnerFatherOrHusbandName, verdana)) { Border = 0, Padding = 6, Colspan = 2 });
                ptableShopDetail.AddCell(new PdfPCell(new Phrase("4. Name of the Manager , if any :", verdanaBold)) { Border = 0, Padding = 6, Colspan = 2 });
                if (processshop.ManagerName != null)
                {
                    ptableShopDetail.AddCell(new PdfPCell(new Phrase(processshop.ManagerName, verdana)) { Border = 0, Padding = 6, Colspan = 2 });
                }
                else
                {
                    ptableShopDetail.AddCell(new PdfPCell(new Phrase("-------", verdana)) { Border = 0, Padding = 6, Colspan = 2 });
                }
                ptableShopDetail.AddCell(new PdfPCell(new Phrase("5. Name of the Establishment :", verdanaBold)) { Border = 0, Padding = 6, Colspan = 2 });
                ptableShopDetail.AddCell(new PdfPCell(new Phrase(projectSite.EstablishmentName, verdana)) { Border = 0, Padding = 6, Colspan = 2 });
                ptableShopDetail.AddCell(new PdfPCell(new Phrase("6.1: Nature of the Business :", verdanaBold)) { Border = 0, Padding = 6, Colspan = 2 });
                ptableShopDetail.AddCell(new PdfPCell(new Phrase("6.1: Project Purpose :", verdanaBold)) { Border = 0, Padding = 6, Colspan = 2 });
                if (processshop.NationalIndustrialClassificationCode != null)
                {
                    string input = processshop.NationalIndustrialClassificationCode;
                    List<string> values = new List<string>();
                    int startIndex = 0;
                    string nicCode = "";
                    string nicCodeDescription = "";

                    while (startIndex < input.Length)
                    {
                        int openQuoteIndex = input.IndexOf('"', startIndex);
                        if (openQuoteIndex == -1)
                            break;

                        int closeQuoteIndex = input.IndexOf('"', openQuoteIndex + 1);
                        if (closeQuoteIndex == -1)
                            break;

                        int valueLength = closeQuoteIndex - openQuoteIndex - 1;
                        if (valueLength > 0)
                        {
                            string extractedValue = input.Substring(openQuoteIndex + 1, valueLength);
                            values.Add(extractedValue);
                        }

                        startIndex = closeQuoteIndex + 1;
                    }
                    // Printing the extracted values
                    foreach (string value in values)
                    {
                        nicCode = value;
                        nicCodeDescription = _context.NICCodes.Where(n => n.NicCode == nicCode).Select(x => x.Description).FirstOrDefault();
                    }

                    if (nicCode != null)
                    {
                        ptableShopDetail.AddCell(new PdfPCell(new Phrase(nicCode + "-" + nicCodeDescription, verdana)) { Border = 0, Padding = 6, Colspan = 2 });
                        ptableShopDetail.AddCell(new PdfPCell(new Phrase(projectSite.ProjectPurpose, verdana)) { Border = 0, Padding = 6, Colspan = 2 });
                    }
                }
                else
                {
                    ptableShopDetail.AddCell(new PdfPCell(new Phrase("Nature", verdana)) { Border = 0, Padding = 6, Colspan = 2 });
                }
                ptableShopDetail.AddCell(new PdfPCell(new Phrase("7. Full Address :", verdanaBold)) { Border = 0, Padding = 6, Colspan = 2 });
                ptableShopDetail.AddCell(new PdfPCell(new Phrase(projectSite.Address + ", " + tehsilName + ", " + districtName + "," + projectSite.PinCode, verdana)) { Border = 0, Padding = 6, Colspan = 2 });
                ptableShopDetail.AddCell(new PdfPCell(new Phrase("8. Employees if any ", verdanaBold)) { Border = 0, Padding = 6, Colspan = 2 });
                ptableShopDetail.AddCell(new PdfPCell(new Phrase(employee.ToString() + " (Annexure Attached)", verdana)) { Border = 0, Padding = 6, Colspan = 2 });
                ptableShopDetail.AddCell(new PdfPCell(new Phrase("9. Date of Declaration", verdanaBold)) { Border = 0, Padding = 6, Colspan = 2 });
                ptableShopDetail.AddCell(new PdfPCell(new Phrase(DateTime.Now.ToString(), verdana)) { Border = 0, Padding = 6, Colspan = 2 });

                ptableShopDetail.AddCell(new PdfPCell(signature) { Border = 0, Colspan = 2, PaddingTop = 40 });
                ptableShopDetail.AddCell(new PdfPCell(employerSignature) { Border = 0, Colspan = 2, PaddingTop = 40, HorizontalAlignment = PdfPCell.ALIGN_RIGHT });

                ptableShopDetail.AddCell(new PdfPCell(new Phrase("Signature of Inspector", verdanaHeader)) { Border = 0, Colspan = 2 });
                ptableShopDetail.AddCell(new PdfPCell(new Phrase("Signature of Employer", verdanaHeader)) { Border = 0, Colspan = 2, HorizontalAlignment = PdfPCell.ALIGN_RIGHT });

                ptableShopDetail.AddCell(new PdfPCell(new Phrase("", verdana)) { Border = 0, Colspan = 4 });
                ptableShopDetail.AddCell(new PdfPCell(new Phrase(userDetails.ListData[0].UserFullName, verdana)) { Border = 0, Colspan = 4 });
                ptableShopDetail.AddCell(new PdfPCell(new Phrase("", verdana)) { Border = 0, Colspan = 4 });
                ptableShopDetail.AddCell(new PdfPCell(new Phrase("Shop and Commercial Establishment", verdana)) { Border = 0, Colspan = 4 });
                ptableShopDetail.AddCell(new PdfPCell(new Phrase("", verdana)) { Border = 0, Colspan = 4 });
                ptableShopDetail.AddCell(new PdfPCell(new Phrase(labourCircle.LabourCircleName, verdana)) { Border = 0, Colspan = 4 });
                ptableShopDetail.AddCell(new PdfPCell(new Phrase("", verdana)) { Border = 0, Colspan = 4 });

                #endregion

                #region Footer
                PdfPTable ptablefooterLine = new PdfPTable(3);
                ptablefooterLine.DefaultCell.Border = 0;
                ptablefooterLine.DefaultCell.Padding = 8;
                ptablefooterLine.SpacingAfter = 10;
                PdfPCell pcellLinefooter = new PdfPCell(new Paragraph(""));
                pcellLinefooter.BorderWidthBottom = 1;
                pcellLinefooter.BorderWidthTop = 0;
                pcellLinefooter.BorderWidthRight = 0;
                pcellLinefooter.BorderWidthLeft = 0;
                pcellLinefooter.Colspan = 3;
                ptablefooterLine.AddCell(pcellLinefooter);

                Paragraph AppHeadingFooter = new Paragraph("This is system generated document Hence no signature required.", verdana);

                AppHeadingFooter.Alignment = Element.ALIGN_CENTER;
                #endregion

                PdfPTable ptableShopDetailLicenceAppliedFormF = new PdfPTable(3);
                ptableShopDetailLicenceAppliedFormF.DefaultCell.Border = 0;
                ptableShopDetailLicenceAppliedFormF.DefaultCell.Padding = 2;
                ptableShopDetailLicenceAppliedFormF.SpacingBefore = 1000;
                ptableShopDetailLicenceAppliedFormF.AddCell(new PdfPCell(ptableleftSideDetailFormF) { Border = 0, PaddingTop = 300, PaddingLeft = -25, PaddingRight = 40 });
                ptableShopDetailLicenceAppliedFormF.AddCell(new PdfPCell(ptableShopFormFDetail) { Border = 0, Colspan = 3 });
                documents.Add(ptableShopDetailLicenceAppliedFormF);
                documents.NewPage();
                PdfPTable ptableShopAnnexure = new PdfPTable(7);
                ptableShopAnnexure.DefaultCell.Border = 0;
                ptableShopAnnexure.DefaultCell.Padding = 2;
                int sno = 1;
                ptableShopAnnexure.AddCell(new PdfPCell(new Phrase("Annexure to be attached at the back side of Form F and Form B (SHOP ACT LICENSE)", verdanaHeader)) { Padding = 20, Colspan = 7, Border = 0, Left = 40 });
                ptableShopAnnexure.AddCell(new PdfPCell(new Phrase((sno++).ToString(), verdana)) { Border = 1, BorderWidthTop = 1, BorderWidthBottom = 1, BorderWidthLeft = 1, BorderWidthRight = 1 });
                ptableShopAnnexure.AddCell(new PdfPCell(new Phrase("The  employer should notify any change within seven days after the changes has taken place in the prescribed Form B or A to the concerned authority/labour inspector [section 13 (4)]", verdana)) { Border = 1, BorderWidthTop = 1, BorderWidthBottom = 1, BorderWidthRight = 1, Colspan = 6, Padding = 6 });

                ptableShopAnnexure.AddCell(new PdfPCell(new Phrase((sno++).ToString(), verdana)) { Border = 1, BorderWidthTop = 0, BorderWidthBottom = 1, BorderWidthLeft = 1, BorderWidthRight = 1 });
                ptableShopAnnexure.AddCell(new PdfPCell(new Phrase("Notice in form B under sub section (1) of section 20 should be exhibited by the employer in the establishment.", verdana)) { Border = 1, BorderWidthTop = 0, BorderWidthBottom = 1, BorderWidthRight = 1, Colspan = 6, Padding = 6 });

                ptableShopAnnexure.AddCell(new PdfPCell(new Phrase((sno++).ToString(), verdana)) { Border = 1, BorderWidthTop = 0, BorderWidthBottom = 1, BorderWidthLeft = 1, BorderWidthRight = 1 });
                ptableShopAnnexure.AddCell(new PdfPCell(new Phrase("Registers of attendance, wages and deduction should be maintained by the employer in Form C, B and E under Section 20.", verdana)) { Border = 1, BorderWidthTop = 0, BorderWidthBottom = 1, BorderWidthRight = 1, Colspan = 6, Padding = 6 });

                ptableShopAnnexure.AddCell(new PdfPCell(new Phrase((sno++).ToString(), verdana)) { Border = 1, BorderWidthTop = 0, BorderWidthBottom = 1, BorderWidthLeft = 1, BorderWidthRight = 1 });
                ptableShopAnnexure.AddCell(new PdfPCell(new Phrase("Register of deduction in form E should be maintained by the employer.", verdana)) { Border = 1, BorderWidthTop = 0, BorderWidthBottom = 1, BorderWidthRight = 1, Colspan = 6, Padding = 6 });

                ptableShopAnnexure.AddCell(new PdfPCell(new Phrase((sno++).ToString(), verdana)) { Border = 1, BorderWidthTop = 0, BorderWidthBottom = 1, BorderWidthLeft = 1, BorderWidthRight = 1 });
                ptableShopAnnexure.AddCell(new PdfPCell(new Phrase("The  women employee who  was entitled for maternity benefit , should be  paid to her immediately i.e. within one week after the date of delivery (section 31)", verdana)) { Border = 1, BorderWidthTop = 0, BorderWidthBottom = 1, BorderWidthRight = 1, Colspan = 6, Padding = 6 });

                ptableShopAnnexure.AddCell(new PdfPCell(new Phrase((sno++).ToString(), verdana)) { Border = 1, BorderWidthTop = 0, BorderWidthBottom = 1, BorderWidthLeft = 1, BorderWidthRight = 1 });
                ptableShopAnnexure.AddCell(new PdfPCell(new Phrase("The  establishment should kept neat, clean and free from dirt and refuses, sufficiently lighted and properly ventilated [rule 16)]", verdana)) { Border = 1, BorderWidthTop = 0, BorderWidthBottom = 1, BorderWidthRight = 1, Colspan = 6, Padding = 6 });

                ptableShopAnnexure.AddCell(new PdfPCell(new Phrase((sno++).ToString(), verdana)) { Border = 1, BorderWidthTop = 0, BorderWidthBottom = 1, BorderWidthLeft = 1, BorderWidthRight = 1 });
                ptableShopAnnexure.AddCell(new PdfPCell(new Phrase("There should be  effective means of drainage provided (in case of wet flooring).[rule 16 (2)]", verdana)) { Border = 1, BorderWidthTop = 0, BorderWidthBottom = 1, BorderWidthRight = 1, Colspan = 6, Padding = 6 });

                ptableShopAnnexure.AddCell(new PdfPCell(new Phrase((sno++).ToString(), verdana)) { Border = 1, BorderWidthTop = 0, BorderWidthBottom = 1, BorderWidthLeft = 1, BorderWidthRight = 1 });
                ptableShopAnnexure.AddCell(new PdfPCell(new Phrase("There should be sufficient supply of drinking water fit for human consumption stored in shelter place.[rule 16.3]", verdana)) { Border = 1, BorderWidthTop = 0, BorderWidthBottom = 1, BorderWidthRight = 1, Colspan = 6, Padding = 6 });

                ptableShopAnnexure.AddCell(new PdfPCell(new Phrase((sno++).ToString(), verdana)) { Border = 1, BorderWidthTop = 0, BorderWidthBottom = 1, BorderWidthLeft = 1, BorderWidthRight = 1 });
                ptableShopAnnexure.AddCell(new PdfPCell(new Phrase("Any dangerous parts of machinery while in motion should be securely fenced by safeguards and shutting devices for cutting of power in emergencies from running machinery. [rule 17]", verdana)) { Border = 1, BorderWidthTop = 0, BorderWidthBottom = 1, BorderWidthRight = 1, Colspan = 6, Padding = 6 });

                ptableShopAnnexure.AddCell(new PdfPCell(new Phrase((sno++).ToString(), verdana)) { Border = 1, BorderWidthTop = 0, BorderWidthBottom = 1, BorderWidthLeft = 1, BorderWidthRight = 1 });
                ptableShopAnnexure.AddCell(new PdfPCell(new Phrase("Tight fitting clothes should be provided to the workers employed on or near the moving part. [rule 17(3)]", verdana)) { Border = 1, BorderWidthTop = 0, BorderWidthBottom = 1, BorderWidthRight = 1, Colspan = 6, Padding = 6 });

                ptableShopAnnexure.AddCell(new PdfPCell(new Phrase((sno++).ToString(), verdana)) { Border = 1, BorderWidthTop = 0, BorderWidthBottom = 1, BorderWidthLeft = 1, BorderWidthRight = 1 });
                ptableShopAnnexure.AddCell(new PdfPCell(new Phrase("There should be  emergency exit in case of fire and fire extinguisher shall be provided [rule 18].", verdana)) { Border = 1, BorderWidthTop = 0, BorderWidthBottom = 1, BorderWidthRight = 1, Colspan = 6, Padding = 6 });

                ptableShopAnnexure.AddCell(new PdfPCell(new Phrase((sno++).ToString(), verdana)) { Border = 1, BorderWidthTop = 0, BorderWidthBottom = 1, BorderWidthLeft = 1, BorderWidthRight = 1 });
                ptableShopAnnexure.AddCell(new PdfPCell(new Phrase("There should be first aid box equipped with  prescribed contents i.e., iodine, bandage, burnol, etc, [rule 19].", verdana)) { Border = 1, BorderWidthTop = 0, BorderWidthBottom = 1, BorderWidthRight = 1, Colspan = 6, Padding = 6 });

                ptableShopAnnexure.AddCell(new PdfPCell(new Phrase((sno++).ToString(), verdana)) { Border = 1, BorderWidthTop = 0, BorderWidthBottom = 1, BorderWidthLeft = 1, BorderWidthRight = 1 });
                ptableShopAnnexure.AddCell(new PdfPCell(new Phrase("The premises of every establishment shall be properly ventilated so as to permit sufficient air and light into the premises.[rule 16].", verdana)) { Border = 1, BorderWidthTop = 0, BorderWidthBottom = 1, BorderWidthRight = 1, Colspan = 6, Padding = 6 });

                ptableShopAnnexure.AddCell(new PdfPCell(new Phrase((sno++).ToString(), verdana)) { Border = 1, BorderWidthTop = 0, BorderWidthBottom = 1, BorderWidthLeft = 1, BorderWidthRight = 1 });
                ptableShopAnnexure.AddCell(new PdfPCell(new Phrase("Any employer found violating any of the provisions of the Act or Rules will be punishable under section 26 of the Act.", verdana)) { Border = 1, BorderWidthTop = 0, BorderWidthBottom = 1, BorderWidthRight = 1, Colspan = 6, Padding = 6 });
                documents.Add(ptableShopAnnexure);
                documents.NewPage();
                documents.Add(pageLogo1);
                PdfPTable ptableShopDetailLicenceAppliedFormB = new PdfPTable(3);
                ptableShopDetailLicenceAppliedFormB.DefaultCell.Border = 0;
                ptableShopDetailLicenceAppliedFormB.DefaultCell.Padding = 2;
                ptableShopDetailLicenceAppliedFormB.SpacingBefore = 1000;
                ptableShopDetailLicenceAppliedFormB.AddCell(new PdfPCell(ptableleftSideDetailFormF) { Border = 0, PaddingTop = 300, PaddingLeft = -25, PaddingRight = 40 });
                ptableShopDetailLicenceAppliedFormB.AddCell(new PdfPCell(ptableShopDetail) { Border = 0, Colspan = 3 });
                documents.Add(ptableShopDetailLicenceAppliedFormB);
                documents.NewPage();
                documents.Add(ptableShopAnnexure);
                documents.NewPage();
                documents.Close();
                PDFPageEvents pageeventsobj = new PDFPageEvents();
            }
            catch (Exception ex)
            {
                genericFormModel.HasException = true;
                genericFormModel.Exceptions = ex.Message;
            }
            return genericFormModel;
        }
        private async Task<CertificateGenerateServiceResultTemplate> Certificate_AppForm_70(Int64 appRefId, string CertificatePerformaURL, ApplicationTypeEnum applicationType, string userName, string licenceNo, bool isDeemedCalling)
        {
            CertificateGenerateServiceResultTemplate genericFormModel = new CertificateGenerateServiceResultTemplate();
            //try
            //{
            //    Guid guId = Guid.NewGuid();
            //    genericFormModel.PdfNameGUID = guId.ToString();
            //    genericFormModel.HasException = false;
            //    int licenceValidUpto = 0;
            //    var SaveDirectoryPath = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "TempFiles")).Root + guId.ToString() + ".pdf";
            //    iTextSharp.text.Document documents = new iTextSharp.text.Document(iTextSharp.text.PageSize.A4, 5, 5, 30, 30);

            //    #region ClassObjects
            //    pdfFonts classFonts = new pdfFonts();
            //    #endregion

            //    #region CreateObjects
            //    PdfWriter owriter = PdfWriter.GetInstance(documents, new FileStream(SaveDirectoryPath, FileMode.Create));
            //    PDFPageEvents pageEvent = new PDFPageEvents();
            //    owriter.PageEvent = pageEvent;
            //    owriter.SetEncryption(PdfWriter.STRENGTH128BITS, "", "", PdfWriter.AllowPrinting);
            //    #endregion

            //    #region Regester Font Style
            //    var fontName = "Alice";
            //    Font font = FontFactory.GetFont(fontName, BaseFont.IDENTITY_H, 16);
            //    Font fontSmall = FontFactory.GetFont(fontName, BaseFont.IDENTITY_H, 12);
            //    Font fontBOLD = FontFactory.GetFont(fontName, BaseFont.IDENTITY_H, 16, iTextSharp.text.Font.BOLD);
            //    #endregion

            //    var factoryLicence = _context.Licence_Factory_GeneralDetails.Where(x => x.AppRefId == appRefId).FirstOrDefault();
            //    decimal highestPower = factoryLicence.PowerKW_Installed > factoryLicence.PowerKW_MaxProposed ? factoryLicence.PowerKW_Installed : factoryLicence.PowerKW_MaxProposed;
            //    Int64 highestWorkers = factoryLicence.Workers_MaxDuringYear > factoryLicence.Workers_OrdinarilyEmployed ? factoryLicence.Workers_MaxDuringYear : factoryLicence.Workers_OrdinarilyEmployed;

            //    var factoryLicenceOccupierDetails = _context.Licence_Factory_OccupierAndManagerDetails.Where(x => x.FactoryLicenceRefId == factoryLicence.FactoryLicenceId).FirstOrDefault();
            //    var application = _context.Applications.Where(x => x.AppId == appRefId).FirstOrDefault();
            //    var projectSite = _context.ProjectSites.Where(x => x.ProjectSiteId == application.ProjectSiteRefId).FirstOrDefault();
            //    var factoryCircle = _context.FactoryCircles.Where(x => x.FactoryCircleId == projectSite.FactoryCircleRefId).FirstOrDefault();
            //    var applicationAction = _context.ApplicationActions.Where(x => x.ApplicationRefId == application.AppId).FirstOrDefault();
            //    var feeDetails = _context.AppFeeDetails.Where(x => x.AppRefId == appRefId).FirstOrDefault();

            //    // Licence Validity Year
            //    if(application.ApplicationPurposeType == ApplicationPurposeTypeEnum.GRANT_LICENCE)
            //    {
            //        DateTime registrationDate = factoryLicence.RegistrationDate.HasValue ? factoryLicence.RegistrationDate.Value : DateTime.MinValue;
            //        DateTime noOfYear = registrationDate.AddYears(factoryLicence.NoOfYears);
            //        licenceValidUpto = noOfYear.Year;
            //    }
            //    else if(application.ApplicationPurposeType == ApplicationPurposeTypeEnum.RENEWAL_LICENCE)
            //    {
            //        DateTime renewalDate = factoryLicence.RenewalFromDate.HasValue ? factoryLicence.RenewalFromDate.Value : DateTime.MinValue;
            //        DateTime noOfYear = renewalDate.AddYears(factoryLicence.NoOfYears);
            //        licenceValidUpto = noOfYear.Year;

            //        // Get Dof Details
            //        var dofDetails = await _iApplicationManagementService.GetDofDetailsByLicenceNo(factoryLicence.OldLicenceNo);
            //    }
            //    else
            //    {
            //        DateTime amendmentDate = factoryLicence.OldLicenceValidUpTo.HasValue ? factoryLicence.OldLicenceValidUpTo.Value : DateTime.MinValue;
            //        licenceValidUpto = amendmentDate.Year;
            //    }

            //    documents.Open();
            //    Font verdanaHeader = FontFactory.GetFont("Arial", 9, Font.BOLD);
            //    Font verdana = FontFactory.GetFont("Arial", 9, BaseColor.BLACK);
            //    Font verdanaBoldItalic = FontFactory.GetFont("Arial", 8, Font.BOLDITALIC);
            //    Font verdanasmall = FontFactory.GetFont("Arial", 8, Font.NORMAL);
            //    verdanasmall.Color = BaseColor.WHITE;
            //    Font verdanaRightHeader = FontFactory.GetFont("Arial", 15, Font.BOLD);
            //    verdanaRightHeader.Color = BaseColor.WHITE;
            //    Font verdanaRightHeader1 = FontFactory.GetFont("Arial", 12, Font.BOLD);
            //    verdanaRightHeader1.Color = BaseColor.WHITE;
            //    Font verdanaRightHeader2 = FontFactory.GetFont("Arial", 9, Font.BOLD);
            //    verdanaRightHeader2.Color = BaseColor.WHITE;
            //    Font monotype = FontFactory.GetFont("Arial", 13, Font.BOLD);
            //    Font monotypeHeader = FontFactory.GetFont("Arial", 15, Font.BOLD);
            //    Font impact = new Font(fontSmall.BaseFont, 12);
            //    Font impactHeader = new Font(fontSmall.BaseFont, 12);
            //    impactHeader.Color = BaseColor.WHITE;
            //    BaseFont bfTimes = BaseFont.CreateFont(BaseFont.TIMES_ROMAN, BaseFont.CP1252, false);
            //    Font timesRoman = FontFactory.GetFont("Arial", 10, BaseColor.BLACK);
            //    Font verdanaBold = FontFactory.GetFont("Arial", 9, Font.BOLD);
            //    iTextSharp.text.Image pageLogo1;

            //    var shopPerforma = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "LicensePerformas")).Root + "CertificatePerforma_Factory.jpg";
            //    pageLogo1 = iTextSharp.text.Image.GetInstance(new FileStream(shopPerforma, FileMode.OpenOrCreate, FileAccess.Read));
            //    //pageLogo1 = iTextSharp.text.Image.GetInstance("~/Content/images/CertificatePerforma_Shop.jpg");
            //    pageLogo1.Alignment = iTextSharp.text.Image.ALIGN_CENTER | iTextSharp.text.Image.UNDERLYING;
            //    pageLogo1.ScaleToFit(800f, 800f);
            //    pageLogo1.SetAbsolutePosition(10f, 25f);
            //    pageLogo1.BorderWidth = 1;
            //    documents.Add(pageLogo1);
            //    PdfPTable ptableleftSideDetail = new PdfPTable(1);
            //    ptableleftSideDetail.DefaultCell.Border = 0;
            //    ptableleftSideDetail.DefaultCell.Padding = 0;
            //    ptableleftSideDetail.HorizontalAlignment = (Element.ALIGN_JUSTIFIED);
            //    ptableleftSideDetail.DefaultCell.VerticalAlignment = (Element.ALIGN_JUSTIFIED);
            //    ptableleftSideDetail.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
            //    ptableleftSideDetail.AddCell(new PdfPCell(new Phrase("", verdanaHeader)) { Border = 0, PaddingTop = 320 });
            //    ptableleftSideDetail.AddCell(new PdfPCell(new Phrase("DESCRIPTION OF THE LICENCED  PREMISES ", verdanaRightHeader1)) { Border = 0, PaddingTop = 8, PaddingLeft = -15 });
            //    ptableleftSideDetail.AddCell(new PdfPCell(new Phrase("Licence No : ", verdanaRightHeader1)) { Border = 0, PaddingTop = 8, PaddingLeft = -15 });
            //    ptableleftSideDetail.AddCell(new PdfPCell(new Phrase("Department of Labour", verdanaRightHeader1)) { Border = 0, PaddingTop = 195, PaddingLeft = -15 });
            //    ptableleftSideDetail.AddCell(new PdfPCell(new Phrase("Govt. of Punjab ", verdanaRightHeader1)) { Border = 0, PaddingTop = 0, PaddingLeft = -15 });
            //    PdfPTable ptableleftSideDetailFormF = new PdfPTable(1);
            //    ptableleftSideDetailFormF.DefaultCell.Border = 0;
            //    ptableleftSideDetailFormF.DefaultCell.Padding = 0;
            //    ptableleftSideDetailFormF.HorizontalAlignment = (Element.ALIGN_JUSTIFIED);
            //    ptableleftSideDetailFormF.DefaultCell.VerticalAlignment = (Element.ALIGN_JUSTIFIED);
            //    ptableleftSideDetailFormF.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
            //    PdfPTable tblCurrLicDetail = new PdfPTable(1);
            //    Font verdanaBoldHeader = FontFactory.GetFont("Arial", 9, Font.BOLD);
            //    verdanaBoldHeader.Color = BaseColor.WHITE;
            //    tblCurrLicDetail.AddCell(new PdfPCell(new Phrase("", verdanaBoldHeader)) { Border = 0 });

            //    tblCurrLicDetail.AddCell(new PdfPCell(new Phrase("DESCRIPTION OF THE ", impactHeader)) { Border = 0, PaddingTop = 8, PaddingLeft = 8 });
            //    tblCurrLicDetail.AddCell(new PdfPCell(new Phrase("LICENCED PREMISES", impactHeader)) { Border = 0, PaddingLeft = 8 });
            //    tblCurrLicDetail.AddCell(new PdfPCell(new Phrase("The licenced premises shown on Building Plan approved vide No. " + factoryLicence.BuildingPlanDofNumber, verdanaBoldHeader)) { Border = 0, PaddingLeft = 8 });
            //    tblCurrLicDetail.AddCell(new PdfPCell(new Phrase("is situated at the address given above and consist of building as per approved drawings", verdanaBoldHeader)) { Border = 0, PaddingTop = 8, PaddingLeft = 8 });

            //    ptableleftSideDetailFormF.AddCell(new PdfPCell(tblCurrLicDetail) { Border = 0, FixedHeight = 130 });

            //    #region QR Code Generate
            //    var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(application.PublicAppRefNum);
            //    string base64String = System.Convert.ToBase64String(plainTextBytes);
            //    QRCodeGenerator qrGenerator = new QRCodeGenerator();
            //    QRCodeGenerator.QRCode qrCode = qrGenerator.CreateQrCode(base64String, QRCodeGenerator.ECCLevel.Q);

            //    using (System.Drawing.Bitmap bitMap = qrCode.GetGraphic(20))
            //    {
            //        using (MemoryStream ms = new MemoryStream())
            //        {
            //            bitMap.Save(ms, ImageFormat.Jpeg);
            //            System.Drawing.Image imgQRCode = System.Drawing.Image.FromStream(ms);
            //            iTextSharp.text.Image imdf = iTextSharp.text.Image.GetInstance(imgQRCode, System.Drawing.Imaging.ImageFormat.Jpeg);
            //            imdf.ScaleAbsoluteHeight(150f);
            //            imdf.ScaleAbsoluteWidth(150f);
            //            ptableleftSideDetailFormF.AddCell(new PdfPCell(imdf) { Border = 0, PaddingTop = 30, PaddingLeft = 10 });
            //        }
            //    }
            //    #endregion

            //    ptableleftSideDetailFormF.AddCell(new PdfPCell(new Phrase("Directorate of Factories, Punjab", verdanaRightHeader1)) { Border = 0, PaddingTop = 40, PaddingLeft = 25 });
            //    ptableleftSideDetailFormF.AddCell(new PdfPCell(new Phrase("Department of Labour", verdanaRightHeader1)) { Border = 0, PaddingTop = 10, PaddingLeft = 10 });
            //    ptableleftSideDetailFormF.AddCell(new PdfPCell(new Phrase("Govt. of Punjab ", verdanaRightHeader1)) { Border = 0, PaddingTop = 0, PaddingLeft = 25 });

            //    #region Officer Signature
            //    var userDetails = await _iAuthService.GetUserFullDetailByUserName(userName);
            //    if (userDetails.ListData[0].Signature == null)
            //    {
            //        userDetails.ListData[0].Signature = "abc.jpg";
            //    }
            //    var officerSigFilePath = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "OfficerSignatures")).Root + userDetails.ListData[0].Signature;

            //    iTextSharp.text.Image signature = iTextSharp.text.Image.GetInstance(new FileStream(officerSigFilePath, FileMode.OpenOrCreate, FileAccess.Read));
            //    signature.ScaleToFit(120f, 120f);
            //    signature.SetAbsolutePosition(630f, 150f);
            //    documents.Add(signature);

            //    //var empSig = _context.ApplicationDocuments.Where(x => x.AppRefId == appRefId && x.DocumentRefId == 20005).OrderByDescending(x => x.AppDocId).FirstOrDefault();
            //    //string empSignatureFileName = "abc.jpg";
            //    //if (empSig != null)
            //    //{
            //    //    empSignatureFileName = empSig.AttachmentName;
            //    //}

            //    //var employerSigFilePath = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "AppFiles")).Root + empSignatureFileName;

            //    //iTextSharp.text.Image employerSignature = iTextSharp.text.Image.GetInstance(new FileStream(employerSigFilePath, FileMode.OpenOrCreate, FileAccess.Read));
            //    //employerSignature.ScaleToFit(120f, 120f);
            //    //employerSignature.SetAbsolutePosition(630f, 150f);
            //    //documents.Add(employerSignature);
            //    #endregion

            //    #region Factory Main Detail
            //    PdfPTable ptableShopFormFDetail = new PdfPTable(4);
            //    ptableShopFormFDetail.DefaultCell.Border = 1;
            //    ptableShopFormFDetail.DefaultCell.Padding = 2;
            //    PdfPCell pFormFDetailcellLine1 = new PdfPCell(new Paragraph(""));
            //    pFormFDetailcellLine1.BorderWidth = 0;
            //    pFormFDetailcellLine1.Colspan = 4;
            //    ptableShopFormFDetail.AddCell(pFormFDetailcellLine1);
            //    ptableShopFormFDetail.AddCell(new PdfPCell(new Phrase("FORM 4", verdanaHeader)) { Border = 0, PaddingLeft = 10, PaddingTop = 55, HorizontalAlignment = PdfPCell.ALIGN_CENTER, Colspan = 4 });
            //    ptableShopFormFDetail.AddCell(new PdfPCell(new Phrase("(Under Rule 8)", verdanaHeader)) { Border = 0, PaddingLeft = 10, PaddingTop = 5, HorizontalAlignment = PdfPCell.ALIGN_CENTER, Colspan = 4 });

            //    ptableShopFormFDetail.AddCell(new PdfPCell(new Phrase("REGISTRATION & LICENCE ", impactHeader)) { Border = 0, PaddingTop = 20, PaddingBottom = 2, PaddingLeft = 25, PaddingRight = 25, VerticalAlignment = PdfPCell.ALIGN_MIDDLE, HorizontalAlignment = PdfPCell.ALIGN_CENTER, Colspan = 4 });
            //    ptableShopFormFDetail.AddCell(new PdfPCell(new Phrase("TO WORK AS FACTORY UNDER ", impactHeader)) { Border = 0, PaddingTop = 0, PaddingBottom = 2, PaddingLeft = 25, PaddingRight = 25, VerticalAlignment = PdfPCell.ALIGN_MIDDLE, HorizontalAlignment = PdfPCell.ALIGN_CENTER, Colspan = 4 });
            //    ptableShopFormFDetail.AddCell(new PdfPCell(new Phrase("THE FACTORIES ACT, 1948", impactHeader)) { Border = 0, PaddingTop = 0, PaddingBottom = 20, PaddingLeft = 25, PaddingRight = 25, VerticalAlignment = PdfPCell.ALIGN_MIDDLE, HorizontalAlignment = PdfPCell.ALIGN_CENTER, Colspan = 4 });
            //    ptableShopFormFDetail.AddCell(new PdfPCell(new Phrase("", verdanaHeader)) { Border = 0, PaddingLeft = 20, HorizontalAlignment = PdfPCell.ALIGN_CENTER, Colspan = 4 });
            //    ptableShopFormFDetail.AddCell(new PdfPCell(new Phrase("Licence is hereby granted to", verdanaHeader)) { Border = 0, PaddingLeft = 20, HorizontalAlignment = PdfPCell.ALIGN_CENTER, Colspan = 4 });

            //    ptableShopFormFDetail.AddCell(new PdfPCell(new Phrase("Sh." + factoryLicenceOccupierDetails.OccupierFullName.ToString() + ", S/O,D/O,W/O: " + factoryLicenceOccupierDetails.OccupierFatherName.ToString() + " Occupier,", impact)) { Border = 0, Colspan = 12, Padding = 6, PaddingTop = 30 });

            //    ptableShopFormFDetail.AddCell(new PdfPCell(new Phrase("M/s " + projectSite.EstablishmentName.FirstOrDefault() + ',' + projectSite.Address, impact)) { Border = 0, Colspan = 12, Padding = 6 });

            //    ptableShopFormFDetail.AddCell(new PdfPCell(new Phrase("Registration Number :- " + licenceNo + " Fees Rs. " + feeDetails.Amount + " In words " + '(' + ConvertAmount(double.Parse(feeDetails.Amount.ToString())) + ')' + " valid " +
            //        "for the premises described above for use as a factory employing not more than " + highestWorkers + " persons on any one day during the year and using motive power not exceeding " + highestPower + " " +
            //        "Kilo Watt subject to the provisions of The Factories Act, 1948 as amended from time to time and the rules made there under.", impact))
            //    { Border = 0, Colspan = 12 });

            //    ptableShopFormFDetail.AddCell(new PdfPCell(new Phrase("This licence shall remain in force till the 31st day of December, " + licenceValidUpto, impact)) { Border = 0, Colspan = 12, Padding = 6 });

            //    ptableShopFormFDetail.AddCell(new PdfPCell(new Phrase("Dated :" + DateTime.Now.ToString(), verdanaHeader)) { Border = 0, Colspan = 12, PaddingTop = 100, Padding = 6 });

            //    ptableShopFormFDetail.AddCell(new PdfPCell(signature) { Border = 0, Colspan = 4, PaddingTop = 40, HorizontalAlignment = PdfPCell.ALIGN_RIGHT });
            //    ptableShopFormFDetail.AddCell(new PdfPCell(new Phrase("For chief Inspector of Factories,", verdanaHeader)) { Border = 0, Colspan = 4, HorizontalAlignment = PdfPCell.ALIGN_RIGHT });
            //    ptableShopFormFDetail.AddCell(new PdfPCell(new Phrase("Department of Labour, Punjab", verdanaHeader)) { Border = 0, Colspan = 4, HorizontalAlignment = PdfPCell.ALIGN_RIGHT });

            //    ptableShopFormFDetail.AddCell(new PdfPCell(new Phrase("Name : ", verdanaHeader)) { Border = 0, Colspan = 2, HorizontalAlignment = PdfPCell.ALIGN_RIGHT });
            //    ptableShopFormFDetail.AddCell(new PdfPCell(new Phrase(userDetails.ListData[0].UserFullName, verdana)) { Border = 0, Colspan = 2, HorizontalAlignment = PdfPCell.ALIGN_RIGHT });
            //    ptableShopFormFDetail.AddCell(new PdfPCell(new Phrase("Desig : ", verdanaHeader)) { Border = 0, Colspan = 2, HorizontalAlignment = PdfPCell.ALIGN_RIGHT });
            //    ptableShopFormFDetail.AddCell(new PdfPCell(new Phrase("Deputy Director of Factories", verdana)) { Border = 0, Colspan = 2, HorizontalAlignment = PdfPCell.ALIGN_RIGHT });
            //    ptableShopFormFDetail.AddCell(new PdfPCell(new Phrase("Circle Name : ", verdanaHeader)) { Border = 0, Colspan = 2, HorizontalAlignment = PdfPCell.ALIGN_RIGHT });
            //    ptableShopFormFDetail.AddCell(new PdfPCell(new Phrase(factoryCircle.FactoryCircleName, verdana)) { Border = 0, Colspan = 2, HorizontalAlignment = PdfPCell.ALIGN_RIGHT });
            //    ptableShopFormFDetail.AddCell(new PdfPCell(new Phrase("District Name : ", verdanaHeader)) { Border = 0, Colspan = 2, HorizontalAlignment = PdfPCell.ALIGN_RIGHT });
            //    ptableShopFormFDetail.AddCell(new PdfPCell(new Phrase(factoryCircle.FactoryCircleName, verdana)) { Border = 0, Colspan = 2, HorizontalAlignment = PdfPCell.ALIGN_RIGHT });
            //    ptableShopFormFDetail.AddCell(new PdfPCell(new Phrase("", verdana)) { Border = 0, Colspan = 4, HorizontalAlignment = PdfPCell.ALIGN_RIGHT });
            //    #endregion

            //    #region Footer
            //    PdfPTable ptablefooterLine = new PdfPTable(3);
            //    ptablefooterLine.DefaultCell.Border = 0;
            //    ptablefooterLine.DefaultCell.Padding = 8;
            //    ptablefooterLine.SpacingAfter = 10;
            //    PdfPCell pcellLinefooter = new PdfPCell(new Paragraph(""));
            //    pcellLinefooter.BorderWidthBottom = 1;
            //    pcellLinefooter.BorderWidthTop = 0;
            //    pcellLinefooter.BorderWidthRight = 0;
            //    pcellLinefooter.BorderWidthLeft = 0;
            //    pcellLinefooter.Colspan = 3;
            //    ptablefooterLine.AddCell(pcellLinefooter);

            //    Paragraph AppHeadingFooter = new Paragraph("This is system generated document Hence no signature required.", verdana);
            //    AppHeadingFooter.Alignment = Element.ALIGN_CENTER;
            //    #endregion

            //    PdfPTable ptableShopDetailLicenceAppliedFormF = new PdfPTable(3);
            //    ptableShopDetailLicenceAppliedFormF.DefaultCell.Border = 0;
            //    ptableShopDetailLicenceAppliedFormF.DefaultCell.Padding = 2;
            //    ptableShopDetailLicenceAppliedFormF.SpacingBefore = 1000;
            //    ptableShopDetailLicenceAppliedFormF.AddCell(new PdfPCell(ptableleftSideDetailFormF) { Border = 0, PaddingTop = 300, PaddingLeft = -25, PaddingRight = 40 });
            //    ptableShopDetailLicenceAppliedFormF.AddCell(new PdfPCell(ptableShopFormFDetail) { Border = 0, Colspan = 3 });
            //    documents.Add(ptableShopDetailLicenceAppliedFormF);
            //    documents.Close();
            //    PDFPageEvents pageeventsobj = new PDFPageEvents();
            //}
            //catch (Exception ex)
            //{
            //    genericFormModel.HasException = true;
            //    genericFormModel.Exceptions = ex.Message;
            //}



            try
            {
                Guid guId = Guid.NewGuid();
                genericFormModel.PdfNameGUID = guId.ToString();
                var sfilepath = "";
                sfilepath = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "TempFiles")).Root + guId.ToString() + ".pdf";
                FileInfo file = new FileInfo(sfilepath);
                if (file.Exists)
                {
                    file.Delete();
                }
                iTextSharp.text.Document documents = new iTextSharp.text.Document(iTextSharp.text.PageSize.A4, 5, 5, 30, 30);
                documents.Open();
                #region ClassObjects
                pdfFonts classFonts = new pdfFonts();
                #endregion

                #region CreateObjects
                PdfWriter owriter = PdfWriter.GetInstance(documents, new FileStream(sfilepath, FileMode.Create));
                PDFPageEvents pageEvent = new PDFPageEvents();
                owriter.PageEvent = pageEvent;
                owriter.SetEncryption(PdfWriter.STRENGTH128BITS, "", "", PdfWriter.AllowPrinting);
                #endregion

                var fontName = "Verdana";
                if (!FontFactory.IsRegistered(fontName))
                {
                    var fontPath = "Content/fonts/VERDANA.TTF";
                    FontFactory.Register(fontPath);
                }

                #region Tables
                var factoryLicence = _context.Licence_Factory_GeneralDetails.Where(x => x.AppRefId == appRefId).FirstOrDefault();
                decimal highestPower = factoryLicence.PowerKW_Installed > factoryLicence.PowerKW_MaxProposed ? factoryLicence.PowerKW_Installed : factoryLicence.PowerKW_MaxProposed;
                Int64 highestWorkers = factoryLicence.Workers_MaxDuringYear > factoryLicence.Workers_OrdinarilyEmployed ? factoryLicence.Workers_MaxDuringYear : factoryLicence.Workers_OrdinarilyEmployed;

                var factoryLicenceOccupierDetails = _context.Licence_Factory_OccupierAndManagerDetails.Where(x => x.FactoryLicenceRefId == factoryLicence.FactoryLicenceId).FirstOrDefault();
                var application = _context.Applications.Where(x => x.AppId == appRefId && x.IsDeleted == false).FirstOrDefault();
                var projectSite = _context.ProjectSiteLogs.Where(x => x.ProjectSiteRefId == application.ProjectSiteRefId && x.ProjectSiteVersion == application.ProjectSiteVersion).FirstOrDefault();
                var factoryCircle = _context.FactoryCircles.Where(x => x.FactoryCircleId == projectSite.FactoryCircleRefId).FirstOrDefault();
                var applicationAction = _context.ApplicationActions.Where(x => x.ApplicationRefId == application.AppId).FirstOrDefault();
                // var feeDetails = _context.AppFeeDetails.Where(x => x.AppRefId == appRefId).FirstOrDefault();
                //var feeDetails = _context.AppFeeDetails.Where(x => x.AppRefId == appRefId && x.PaymentBatchCounter>0).Sum(x => x.Amount);
                var feeDetails = _context.AppFeeTransactions.Where(x => x.AppRefId == appRefId && x.TransactionFinalStatusType == TransactionFinalStatusTypeEnum.SUCCEED).Sum(x => x.AmountCalculated);
                #endregion

                #region Licence Validity Year
                int licenceValidUpto = 0;
                DofLicenceDetailsViewModel dofDetails = new DofLicenceDetailsViewModel();
                if (application.ApplicationPurposeType == ApplicationPurposeTypeEnum.GRANT_LICENCE)
                {
                    DateTime registrationDate = factoryLicence.RegistrationDate.HasValue ? factoryLicence.RegistrationDate.Value : DateTime.MinValue;
                    DateTime noOfYear = registrationDate.AddYears(factoryLicence.NoOfYears);
                    licenceValidUpto = noOfYear.Year - 1;
                    var resp = await _iApplicationManagementService.GetDofDetailsByDOFNo(factoryLicence.BuildingPlanDofNumber);
                    dofDetails = resp.FormModel;
                }

                else if (application.ApplicationPurposeType == ApplicationPurposeTypeEnum.RENEWAL_LICENCE)
                {
                    DateTime renewalDate = factoryLicence.RenewalFromDate.HasValue ? factoryLicence.RenewalFromDate.Value : DateTime.MinValue;
                    DateTime noOfYear = renewalDate.AddYears(factoryLicence.NoOfYears);
                    licenceValidUpto = noOfYear.Year - 1;

                    // Get Dof Details
                    var resp = await _iApplicationManagementService.GetDofDetailsByLicenceNo(factoryLicence.OldLicenceNo);
                    dofDetails = resp.FormModel;
                }
                else
                {
                    DateTime amendmentDate = factoryLicence.OldLicenceValidUpTo.HasValue ? factoryLicence.OldLicenceValidUpTo.Value : DateTime.MinValue;
                    licenceValidUpto = amendmentDate.Year;
                }
                #endregion

                string fontpath1 = "Content/fonts/VERDANA.TTF";
                BaseFont customfont = BaseFont.CreateFont(fontpath1, BaseFont.CP1252, BaseFont.EMBEDDED);
                string fontpath2 = "Content/fonts/VERDANA.TTF";
                BaseFont customfontimpact = BaseFont.CreateFont(fontpath2, BaseFont.CP1252, BaseFont.EMBEDDED);
                documents.Open();
                Font verdanaHeader = FontFactory.GetFont("Arial", 9, Font.BOLD);
                Font verdana = FontFactory.GetFont("Arial", 8, BaseColor.BLACK);
                Font verdanaBoldItalic = FontFactory.GetFont("Arial", 8, Font.BOLDITALIC);
                Font verdanasmall = FontFactory.GetFont("Arial", 8, Font.NORMAL);
                verdanasmall.Color = BaseColor.WHITE;
                Font verdanaRightHeader = FontFactory.GetFont("Arial", 15, Font.BOLD);
                verdanaRightHeader.Color = BaseColor.WHITE;
                Font verdanaRightHeader1 = FontFactory.GetFont("Arial", 12, Font.BOLD);
                verdanaRightHeader1.Color = BaseColor.WHITE;
                Font arialOldLicenceDetails = FontFactory.GetFont("Arial", 10, Font.BOLD);
                arialOldLicenceDetails.Color = BaseColor.WHITE;
                Font monotype = new Font(customfont, 12);
                Font monotypeHeader = new Font(customfont, 20);
                Font impact = new Font(customfontimpact, 12);
                Font impactHeader = new Font(customfontimpact, 14, Font.BOLD);
                impactHeader.Color = BaseColor.WHITE;
                BaseFont bfTimes = BaseFont.CreateFont(BaseFont.TIMES_ROMAN, BaseFont.CP1252, false);
                Font timesRoman = FontFactory.GetFont("Arial", 10, BaseColor.BLACK);
                Font ArialFont = FontFactory.GetFont("Arial", 8, BaseColor.WHITE);
                Font SealFontArial = FontFactory.GetFont("Arial", 9, BaseColor.BLACK);
                Font verdanaBold = FontFactory.GetFont("Arial", 10, Font.BOLD);

                var factoryPerforma = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "LicensePerformas")).Root + "CertificatePerforma_Factory.jpg";
                iTextSharp.text.Image pageLogo1 = iTextSharp.text.Image.GetInstance(new FileStream(factoryPerforma, FileMode.OpenOrCreate, FileAccess.Read));

                pageLogo1.Alignment = iTextSharp.text.Image.ALIGN_CENTER | iTextSharp.text.Image.UNDERLYING;
                pageLogo1.ScaleToFit(800f, 800f);
                pageLogo1.SetAbsolutePosition(10f, 25f);
                pageLogo1.BorderWidth = 1;
                documents.Add(pageLogo1);
                PdfPTable ptableleftSideDetail = new PdfPTable(1);
                ptableleftSideDetail.DefaultCell.Border = 0;
                ptableleftSideDetail.DefaultCell.Padding = 0;
                ptableleftSideDetail.HorizontalAlignment = (Element.ALIGN_JUSTIFIED);
                ptableleftSideDetail.DefaultCell.VerticalAlignment = (Element.ALIGN_JUSTIFIED);
                ptableleftSideDetail.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
                ptableleftSideDetail.AddCell(new PdfPCell(new Phrase("", verdanaBold)) { Border = 0, PaddingTop = 10 });

                ptableleftSideDetail.AddCell(new PdfPCell(new Phrase("DESCRIPTION OF THE LICENCED  PREMISES ", verdanaRightHeader1)) { Border = 0, PaddingTop = 300 });
                Font verdanaBoldBP = new Font(BaseFont.CreateFont(BaseFont.TIMES_ROMAN, BaseFont.CP1252, false), 8, Font.BOLD | Font.UNDERLINE, BaseColor.WHITE);
                Phrase phrase = new Phrase();

                phrase.Add(new Chunk(Chunk.NEWLINE + "" + Chunk.NEWLINE + "The licenced premises shown on Building Plan approved vide No. ", verdanasmall));

                phrase.Add(new Chunk(factoryLicence.Licence_Factory_OccupierAndManagerDetail.StabilityDOFNumber, verdanaBoldBP));
                phrase.Add(new Chunk(" Dated ", verdanasmall));
                phrase.Add(new Chunk(factoryLicence.Licence_Factory_OccupierAndManagerDetail.StabilityCertificateDate.ToShortDateString() + Chunk.NEWLINE, verdanaBoldBP));

                //if (dofDetails != null)
                //{
                //    phrase.Add(new Chunk(dofDetails.DofNumber, verdanaBoldBP));
                //    phrase.Add(new Chunk(" Dated ", verdanasmall));
                //    phrase.Add(new Chunk(dofDetails.DofApprovalDate.ToShortDateString() + Chunk.NEWLINE, verdanaBoldBP));
                //}
                //else
                //{
                //    phrase.Add(new Chunk("NA", verdanaBoldBP));
                //    phrase.Add(new Chunk(" Dated ", verdanasmall));
                //    phrase.Add(new Chunk("NA" + Chunk.NEWLINE, verdanaBoldBP));
                //}

                phrase.Add(new Chunk(Chunk.NEWLINE + "" + Chunk.NEWLINE + "is situated at the address given above and consist of building as per approved drawings.", verdanasmall));
                ptableleftSideDetail.AddCell(new PdfPCell(phrase) { Border = 0, PaddingTop = 10, PaddingRight = 20 });

                #region QR Code Generate
                var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(application.PublicAppRefNum);
                string base64String = System.Convert.ToBase64String(plainTextBytes);
                QRCodeGenerator qrGenerator = new QRCodeGenerator();
                QRCodeGenerator.QRCode qrCode = qrGenerator.CreateQrCode(base64String, QRCodeGenerator.ECCLevel.Q);

                using (System.Drawing.Bitmap bitMap = qrCode.GetGraphic(20))
                {
                    using (MemoryStream ms = new MemoryStream())
                    {
                        bitMap.Save(ms, ImageFormat.Jpeg);
                        System.Drawing.Image imgQRCode = System.Drawing.Image.FromStream(ms);
                        iTextSharp.text.Image imdf = iTextSharp.text.Image.GetInstance(imgQRCode, System.Drawing.Imaging.ImageFormat.Jpeg);
                        imdf.ScaleAbsoluteHeight(120f);
                        imdf.ScaleAbsoluteWidth(120f);
                        ptableleftSideDetail.AddCell(new PdfPCell(imdf) { Border = 0, PaddingTop = 30, PaddingLeft = 0, PaddingRight = 0 });
                    }
                }
                #endregion

                ptableleftSideDetail.AddCell(new PdfPCell(new Phrase("" + Chunk.NEWLINE + "Directorate of Factories, Punjab" + Chunk.NEWLINE, verdanaRightHeader)) { Border = 0, PaddingTop = 30, PaddingRight = 25, HorizontalAlignment = PdfPCell.ALIGN_CENTER });
                ptableleftSideDetail.AddCell(new PdfPCell(new Phrase("" + Chunk.NEWLINE + "Department of Labour, Gov. of Punjab" + Chunk.NEWLINE, verdanaRightHeader1)) { Border = 0, PaddingTop = 15, PaddingRight = 25, HorizontalAlignment = PdfPCell.ALIGN_CENTER });

                PdfPTable ptablerightSideDetail = new PdfPTable(1);
                ptablerightSideDetail.DefaultCell.Border = 0;
                ptablerightSideDetail.DefaultCell.Padding = 0;

                ptablerightSideDetail.AddCell(new PdfPCell(new Phrase("", verdanaBold)) { Border = 0, Padding = 30 });
                ptablerightSideDetail.AddCell(new PdfPCell(new Phrase("FORM 4", verdanaHeader)) { Border = 0, PaddingLeft = 20, HorizontalAlignment = PdfPCell.ALIGN_CENTER });
                ptablerightSideDetail.AddCell(new PdfPCell(new Phrase("(Under Rule 8)", verdanaHeader)) { Border = 0, PaddingLeft = 20, HorizontalAlignment = PdfPCell.ALIGN_CENTER });

                if (application.ApplicationPurposeType == ApplicationPurposeTypeEnum.AMENDMENT_LICENCE)
                {
                    ptablerightSideDetail.AddCell(new PdfPCell(new Phrase("AMENDMENT OF FACTORY LICENCE TO WORK AS FACTORY UNDER  THE FACTORIES ACT, 1948", impactHeader)) { Border = 0, PaddingTop = 70, PaddingBottom = 20, PaddingLeft = 55, PaddingRight = 40, VerticalAlignment = PdfPCell.ALIGN_MIDDLE, HorizontalAlignment = PdfPCell.ALIGN_CENTER });
                }
                else
                {
                    ptablerightSideDetail.AddCell(new PdfPCell(new Phrase("REGISTRATION & LICENCE TO WORK AS FACTORY UNDER  THE FACTORIES ACT, 1948", impactHeader)) { Border = 0, PaddingTop = 70, PaddingBottom = 20, PaddingLeft = 55, PaddingRight = 40, VerticalAlignment = PdfPCell.ALIGN_MIDDLE, HorizontalAlignment = PdfPCell.ALIGN_CENTER });
                }

                ptablerightSideDetail.AddCell(new PdfPCell(new Phrase("", verdanaBold)) { Border = 0, Padding = 5 });
                ptablerightSideDetail.AddCell(new PdfPCell(new Phrase("Licence is hereby granted to", monotypeHeader)) { Border = 0, PaddingBottom = 20 });

                Paragraph para = new Paragraph();
                para.Add(new Chunk("Sh. ", timesRoman));
                para.Add(new Chunk(factoryLicence.Licence_Factory_OccupierAndManagerDetail.OccupierFullName, verdanaBold).SetUnderline(0.1f, -2f));
                para.Add(new Chunk("S/O,D/O,W/O :  " + Chunk.NEWLINE, timesRoman));
                para.Add(new Chunk(factoryLicence.Licence_Factory_OccupierAndManagerDetail.OccupierFatherName, verdanaBold).SetUnderline(0.1f, -2f));
                para.Add(new Chunk(" Occupier,  " + Chunk.NEWLINE, timesRoman));
                para.Add(new Chunk("M/s ", timesRoman));
                para.Add(new Chunk(projectSite.EstablishmentName + ", " + projectSite.Address + Chunk.NEWLINE, verdanaBold).SetUnderline(0.1f, -2f));
                para.Add(new Chunk(Chunk.NEWLINE + "Registration Number:- ", timesRoman));
                para.Add(new Chunk(licenceNo, verdanaBold).SetUnderline(0.1f, -2f));
                para.Add(new Chunk("  Fees Rs. ", timesRoman));

                if (feeDetails != null)
                {
                    para.Add(new Chunk(feeDetails.ToString(), verdanaBold).SetUnderline(0.1f, -2f));
                    para.Add(new Chunk(" In words (", timesRoman));
                    para.Add(new Chunk('(' + ConvertAmount(double.Parse(feeDetails.ToString())) + ')', verdanaBoldItalic).SetUnderline(0.1f, -2f));
                }
                else
                {
                    if (factoryLicence.IsTempRegistered == 1)
                    {
                        var tempLicenceFee = await _iCommonApisService.GetInformationByLicenceNo(factoryLicence.TempRegistrationNumber);
                        if (tempLicenceFee.ResponseDataModel.FeeDetailsList.Count() > 0)
                        {
                            para.Add(new Chunk(tempLicenceFee.ResponseDataModel.FeeDetailsList.FirstOrDefault().PaymentAmount.ToString(), verdanaBold).SetUnderline(0.1f, -2f));
                            para.Add(new Chunk(" In words (", timesRoman));
                            para.Add(new Chunk('(' + ConvertAmount(double.Parse(tempLicenceFee.ResponseDataModel.FeeDetailsList.FirstOrDefault().PaymentAmount.ToString())) + ')', verdanaBoldItalic).SetUnderline(0.1f, -2f));
                        }
                    }
                    else
                    {
                        para.Add(new Chunk("0", verdanaBold).SetUnderline(0.1f, -2f));
                        para.Add(new Chunk(" In words (", timesRoman));
                        para.Add(new Chunk("NA", verdanaBoldItalic).SetUnderline(0.1f, -2f));
                    }

                }
                para.Add(new Chunk(") valid for ", timesRoman));

                para.Add(new Chunk("the premises described above for use as a factory employing not more than ", timesRoman));
                para.Add(new Chunk(highestWorkers.ToString(), verdanaBold).SetUnderline(0.1f, -2f));

                para.Add(new Chunk(" persons on any one day during the year and using motive power not exceeding  ", timesRoman));
                para.Add(new Chunk(highestPower.ToString(), verdanaBold).SetUnderline(0.1f, -2f));
                para.Add(new Chunk(" Kilo Watt subject to the provisions of The Factories Act, 1948 as amended from time to time and the rules made thereunder.", timesRoman));

                para.SpacingAfter = 10.5f;
                para.SpacingBefore = 10.5f;

                PdfPCell shipment = new PdfPCell(para) { Border = 0, };
                shipment.SetLeading(1f, 2f);
                ptablerightSideDetail.AddCell(shipment);
                ptablerightSideDetail.AddCell(new PdfPCell() { Border = 0, BorderWidthBottom = 0, PaddingTop = 5, PaddingBottom = 5 });
                ptablerightSideDetail.AddCell(new PdfPCell(new Phrase("This licence shall remain in force till the 31st day of December, " + licenceValidUpto + Chunk.NEWLINE + "" + Chunk.NEWLINE + "" + Chunk.NEWLINE, timesRoman)) { Border = 0, PaddingBottom = 30 });


                var userDetails = await _iAuthService.GetUserFullDetailByUserName(userName);
                string officerSigFilePath = "";
                if (!isDeemedCalling)
                {
                    if (userDetails.ListData.Count() == 0 || userDetails.ListData[0].Signature == null)
                    {
                        userDetails.ListData[0].Signature = "abc.jpg";
                    }
                    officerSigFilePath = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "OfficerSignatures")).Root + userDetails.ListData[0].Signature;
                }
                else
                {
                    officerSigFilePath = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "OfficerSignatures")).Root + "deemed_engine_signature.jpg";
                }
                iTextSharp.text.Image signature = iTextSharp.text.Image.GetInstance(new FileStream(officerSigFilePath, FileMode.OpenOrCreate, FileAccess.Read));
                signature.ScaleToFit(120f, 120f);
                signature.SetAbsolutePosition(630f, 150f);
                //ptableleftSideDetail.AddCell(new PdfPCell(signature) { Border = 0, PaddingTop = 30, PaddingLeft = 0, PaddingRight = 0 });


                Paragraph SignSeal = new Paragraph();
                SignSeal.Add(new Chunk("Name: ", verdanaHeader));
                SignSeal.Add(new Chunk(userDetails.ListData[0].UserFullName, SealFontArial));
                SignSeal.Add(new Chunk(Chunk.NEWLINE + "Desig: ", verdanaHeader));
                SignSeal.Add(new Chunk("Deputy Director of Factories", SealFontArial));
                SignSeal.Add(new Chunk(Chunk.NEWLINE + "Circle Name: ", verdanaHeader));
                SignSeal.Add(new Chunk(factoryCircle.FactoryCircleName, SealFontArial));
                SignSeal.Add(new Chunk(Chunk.NEWLINE + "District Name: ", verdanaHeader));
                SignSeal.Add(new Chunk(factoryCircle.FactoryCircleName, SealFontArial));

                ptablerightSideDetail.AddCell(new PdfPCell(signature) { Border = 0, PaddingTop = 5, HorizontalAlignment = PdfPCell.ALIGN_RIGHT });

                ptablerightSideDetail.AddCell(new PdfPCell(new Phrase("For Chief Inspector of Factories, ", verdanaHeader)) { Border = 0, HorizontalAlignment = PdfPCell.ALIGN_LEFT, PaddingLeft = 150 });
                ptablerightSideDetail.AddCell(new PdfPCell(new Phrase("Department of Labour, Punjab.", verdanaHeader)) { Border = 0, HorizontalAlignment = PdfPCell.ALIGN_LEFT, PaddingLeft = 150 });
                ptablerightSideDetail.AddCell(new PdfPCell(SignSeal) { Border = 0, HorizontalAlignment = PdfPCell.ALIGN_LEFT, PaddingLeft = 150, PaddingBottom = 10 });

                ptablerightSideDetail.AddCell(new PdfPCell(new Phrase(" Dated " + DateTime.Now.ToShortDateString(), verdanaHeader)) { Border = 0, HorizontalAlignment = PdfPCell.ALIGN_LEFT, PaddingLeft = 148 });

                PdfPTable ptableShopDetailLicenceApplied = new PdfPTable(3);
                ptableShopDetailLicenceApplied.DefaultCell.Border = 0;
                ptableShopDetailLicenceApplied.DefaultCell.Padding = 2;
                ptableShopDetailLicenceApplied.SpacingBefore = 1000;
                ptableShopDetailLicenceApplied.AddCell(new PdfPCell(ptableleftSideDetail) { Border = 0 });
                ptableShopDetailLicenceApplied.AddCell(new PdfPCell(ptablerightSideDetail) { Border = 0, Colspan = 2 });
                documents.Add(ptableShopDetailLicenceApplied);
                documents.Close();
                PDFPageEvents pageeventsobj = new PDFPageEvents();

                ptablerightSideDetail.AddCell(new PdfPCell(para) { Border = 0, VerticalAlignment = PdfPCell.ALIGN_JUSTIFIED, HorizontalAlignment = PdfPCell.ALIGN_JUSTIFIED, FixedHeight = 400 });
            }
            catch (Exception ex)
            {
                genericFormModel.HasException = true;
                genericFormModel.Exceptions = ex.Message;
            }
            return genericFormModel;
        }
        private async Task<CertificateGenerateServiceResultTemplate> Certificate_AppForm_61(Int64 appRefId, string CertificatePerformaURL, ApplicationTypeEnum applicationType, string userName, string licenceNo, bool isDeemedCalling)
        {
            CertificateGenerateServiceResultTemplate genericFormModel = new CertificateGenerateServiceResultTemplate();
            try
            {
                Guid guId = Guid.NewGuid();
                genericFormModel.PdfNameGUID = guId.ToString();
                genericFormModel.HasException = false;
                var SaveDirectoryPath = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "TempFiles")).Root + guId.ToString() + ".pdf";
                iTextSharp.text.Document documents = new iTextSharp.text.Document(iTextSharp.text.PageSize.A4, 5, 5, 30, 30);

                #region ClassObjects
                pdfFonts classFonts = new pdfFonts();
                #endregion

                #region CreateObjects
                PdfWriter owriter = PdfWriter.GetInstance(documents, new FileStream(SaveDirectoryPath, FileMode.Create));
                PDFPageEvents pageEvent = new PDFPageEvents();
                owriter.PageEvent = pageEvent;
                owriter.SetEncryption(PdfWriter.STRENGTH128BITS, "", "", PdfWriter.AllowPrinting);
                #endregion

                #region Regester Font Style
                var fontName = "Alice";
                //if (!FontFactory.IsRegistered(fontName))
                //{
                //    var fontPath = "Content/fonts/Alice-Regular.ttf";
                //    Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
                //    FontFactory.Register(fontPath, fontName);
                //}
                Font font = FontFactory.GetFont(fontName, BaseFont.IDENTITY_H, 16);
                Font fontSmall = FontFactory.GetFont(fontName, BaseFont.IDENTITY_H, 12);
                Font fontBOLD = FontFactory.GetFont(fontName, BaseFont.IDENTITY_H, 16, iTextSharp.text.Font.BOLD);
                //BaseFont customfontimpact = BaseFont.CreateFont(fontName, BaseFont.CP1252, BaseFont.EMBEDDED);
                #endregion

                var formData = _context.Licence_Shop_NightShift_Approvals.Where(x => x.AppRefId == appRefId).FirstOrDefault();
                var licenceMappingInfo = _context.ApplicationLicenceNoMapping.Where(x => x.LicenceNumber == formData.LicenceNumber).FirstOrDefault();

                /******* PENDING FOR OLD DB *****/
                GenericResponseTemplateModel<ShopLicenceDetailsForNightShiftPdfViewModel> shopFormModelDetails = new GenericResponseTemplateModel<ShopLicenceDetailsForNightShiftPdfViewModel>();
                if (licenceMappingInfo == null)
                {
                    shopFormModelDetails = await _iApplicationManagementService.getShopLicenceLegacyDetail(formData.LicenceNumber);
                }
                else
                {
                    var shopLicenceDetails = _context.ShopLicence_GeneralDetails.Where(x => x.AppRefId == licenceMappingInfo.AppRefId).Include(x => x.ShopLicence_EmployeeDetails).FirstOrDefault();
                    //var application = _context.Applications.Where(x => x.AppId == licenceMappingInfo.AppRefId).Include(x => x.ProjectSites).FirstOrDefault();
                    var application = _context.Applications.Where(x => x.AppId == appRefId && x.IsDeleted == false).FirstOrDefault();
                    var projectSite = _context.ProjectSiteLogs.Where(x => x.ProjectSiteRefId == application.ProjectSiteRefId && x.ProjectSiteVersion == application.ProjectSiteVersion).FirstOrDefault();
                    var tehsilName = _context.Tehsils.Where(x => x.TehsilLgdId == projectSite.TehsilRefId).Select(x => x.TehsilName).FirstOrDefault();
                    shopFormModelDetails.ResponseDataModel = new ShopLicenceDetailsForNightShiftPdfViewModel();
                    shopFormModelDetails.ResponseDataModel.OName = shopLicenceDetails.OwnerName;
                    shopFormModelDetails.ResponseDataModel.OFHName = shopLicenceDetails.OwnerFatherOrHusbandName;
                    shopFormModelDetails.ResponseDataModel.MgrName = shopLicenceDetails.ManagerName;
                    shopFormModelDetails.ResponseDataModel.EstdName = projectSite.EstablishmentName;
                    shopFormModelDetails.ResponseDataModel.ProjectPurpose = projectSite.ProjectPurpose;
                    shopFormModelDetails.ResponseDataModel.SiteAddress = projectSite.Address;
                    shopFormModelDetails.ResponseDataModel.PinCode = int.Parse(projectSite.PinCode);
                    shopFormModelDetails.ResponseDataModel.TehsilName = tehsilName;
                    shopFormModelDetails.ResponseDataModel.TotalEmployees = shopLicenceDetails.ShopLicence_EmployeeDetails.Count();
                    shopFormModelDetails.ResponseDataModel.ModifiedTokenNumber = application.PublicAppRefNum;
                    shopFormModelDetails.ResponseDataModel.NationalIndustrialClassificationCode = shopLicenceDetails.NationalIndustrialClassificationCode;
                    shopFormModelDetails.ResponseDataModel.ClearanceIssuedOn = DateTime.Now;
                    shopFormModelDetails.ResponseDataModel.ApplicantSign = null;
                }

                documents.Open();
                Font verdanaHeader = FontFactory.GetFont("Arial", 9, Font.BOLD);
                Font verdana = FontFactory.GetFont("Arial", 9, BaseColor.BLACK);
                Font verdanaBoldItalic = FontFactory.GetFont("Arial", 8, Font.BOLDITALIC);
                Font verdanasmall = FontFactory.GetFont("Arial", 8, Font.NORMAL);
                verdanasmall.Color = BaseColor.WHITE;
                Font verdanaRightHeader = FontFactory.GetFont("Arial", 15, Font.BOLD);
                verdanaRightHeader.Color = BaseColor.WHITE;
                Font verdanaRightHeader1 = FontFactory.GetFont("Arial", 12, Font.BOLD);
                verdanaRightHeader1.Color = BaseColor.WHITE;
                Font verdanaRightHeader2 = FontFactory.GetFont("Arial", 9, Font.BOLD);
                verdanaRightHeader2.Color = BaseColor.WHITE;
                Font monotype = FontFactory.GetFont("Arial", 13, Font.BOLD);
                Font monotypeHeader = FontFactory.GetFont("Arial", 15, Font.BOLD);
                Font impact = new Font(fontSmall.BaseFont, 12);
                Font impactHeader = new Font(fontSmall.BaseFont, 12);
                impactHeader.Color = BaseColor.WHITE;
                BaseFont bfTimes = BaseFont.CreateFont(BaseFont.TIMES_ROMAN, BaseFont.CP1252, false);
                Font timesRoman = FontFactory.GetFont("Arial", 10, BaseColor.BLACK);
                Font verdanaBold = FontFactory.GetFont("Arial", 9, Font.BOLD);
                iTextSharp.text.Image pageLogo1;

                var shopPerforma = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "LicensePerformas")).Root + "CertificatePerforma_Shop.jpg";
                pageLogo1 = iTextSharp.text.Image.GetInstance(new FileStream(shopPerforma, FileMode.OpenOrCreate, FileAccess.Read));
                pageLogo1.Alignment = iTextSharp.text.Image.ALIGN_CENTER | iTextSharp.text.Image.UNDERLYING;
                pageLogo1.ScaleToFit(800f, 800f);
                pageLogo1.SetAbsolutePosition(10f, 25f);
                pageLogo1.BorderWidth = 1;
                documents.Add(pageLogo1);
                PdfPTable ptableleftSideDetail = new PdfPTable(1);
                ptableleftSideDetail.DefaultCell.Border = 0;
                ptableleftSideDetail.DefaultCell.Padding = 0;
                ptableleftSideDetail.HorizontalAlignment = (Element.ALIGN_JUSTIFIED);
                ptableleftSideDetail.DefaultCell.VerticalAlignment = (Element.ALIGN_JUSTIFIED);
                ptableleftSideDetail.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
                ptableleftSideDetail.AddCell(new PdfPCell(new Phrase("", verdanaHeader)) { Border = 0, PaddingTop = 320 });
                ptableleftSideDetail.AddCell(new PdfPCell(new Phrase("DESCRIPTION OF THE LICENCED  PREMISES ", verdanaRightHeader1)) { Border = 0, PaddingTop = 8, PaddingLeft = -15 });
                ptableleftSideDetail.AddCell(new PdfPCell(new Phrase("Licence No : ", verdanaRightHeader1)) { Border = 0, PaddingTop = 8, PaddingLeft = -15 });
                ptableleftSideDetail.AddCell(new PdfPCell(new Phrase("Department of Labour", verdanaRightHeader1)) { Border = 0, PaddingTop = 195, PaddingLeft = -15 });
                ptableleftSideDetail.AddCell(new PdfPCell(new Phrase("      Govt. of Punjab ", verdanaRightHeader1)) { Border = 0, PaddingTop = 0, PaddingLeft = -15 });
                PdfPTable ptableleftSideDetailFormF = new PdfPTable(1);
                ptableleftSideDetailFormF.DefaultCell.Border = 0;
                ptableleftSideDetailFormF.DefaultCell.Padding = 0;
                ptableleftSideDetailFormF.HorizontalAlignment = (Element.ALIGN_JUSTIFIED);
                ptableleftSideDetailFormF.DefaultCell.VerticalAlignment = (Element.ALIGN_JUSTIFIED);
                ptableleftSideDetailFormF.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
                PdfPTable tblCurrLicDetail = new PdfPTable(1);
                Font verdanaBoldHeader = FontFactory.GetFont("Arial", 9, Font.BOLD);
                verdanaBoldHeader.Color = BaseColor.WHITE;
                tblCurrLicDetail.AddCell(new PdfPCell(new Phrase("", verdanaBoldHeader)) { Border = 0 });

                tblCurrLicDetail.AddCell(new PdfPCell(new Phrase("Reg. No : " + licenceNo, verdanaBoldHeader)) { Border = 0, PaddingTop = 8 });
                tblCurrLicDetail.AddCell(new PdfPCell(new Phrase("Issued Date : " + DateTime.Now.Day.ToString() + "-" + DateTime.Now.Date.ToString("MMM") + "-" + DateTime.Now.Year.ToString(), verdanaBoldHeader)) { Border = 0, PaddingTop = 8 });
                tblCurrLicDetail.AddCell(new PdfPCell(new Phrase("File No: " + shopFormModelDetails.ResponseDataModel.ModifiedTokenNumber, verdanaBoldHeader)) { Border = 0, PaddingTop = 8, PaddingBottom = 8 });

                ptableleftSideDetailFormF.AddCell(new PdfPCell(tblCurrLicDetail) { Border = 0, FixedHeight = 100 });

                #region QR Code Generate
                var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(shopFormModelDetails.ResponseDataModel.ModifiedTokenNumber);
                string base64String = System.Convert.ToBase64String(plainTextBytes);
                QRCodeGenerator qrGenerator = new QRCodeGenerator();
                QRCodeGenerator.QRCode qrCode = qrGenerator.CreateQrCode(base64String, QRCodeGenerator.ECCLevel.Q);

                using (System.Drawing.Bitmap bitMap = qrCode.GetGraphic(20))
                {
                    using (MemoryStream ms = new MemoryStream())
                    {
                        bitMap.Save(ms, ImageFormat.Jpeg);
                        System.Drawing.Image imgQRCode = System.Drawing.Image.FromStream(ms);
                        iTextSharp.text.Image imdf = iTextSharp.text.Image.GetInstance(imgQRCode, System.Drawing.Imaging.ImageFormat.Jpeg);
                        imdf.ScaleAbsoluteHeight(150f);
                        imdf.ScaleAbsoluteWidth(150f);
                        ptableleftSideDetailFormF.AddCell(new PdfPCell(imdf) { Border = 0, PaddingTop = 10, PaddingRight = 5 });
                    }
                }

                #endregion

                ptableleftSideDetailFormF.AddCell(new PdfPCell(new Phrase("Department of Labour", verdanaRightHeader1)) { Border = 0, PaddingTop = 120, PaddingLeft = 10 });
                ptableleftSideDetailFormF.AddCell(new PdfPCell(new Phrase("Govt. of Punjab ", verdanaRightHeader1)) { Border = 0, PaddingTop = 0, PaddingLeft = 25 });

                #region Officer Signature

                var userDetails = await _iAuthService.GetUserFullDetailByUserName(userName);
                string officerSigFilePath = "";
                if (!isDeemedCalling)
                {
                    if (userDetails.ListData.Count() == 0 || userDetails.ListData[0].Signature == null)
                    {
                        userDetails.ListData[0].Signature = "abc.jpg";
                    }
                    string filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "OfficerSignatures", userDetails.ListData[0].Signature);
                    // Checking if the file exists
                    if (File.Exists(filePath))
                    {
                        officerSigFilePath = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "OfficerSignatures")).Root + userDetails.ListData[0].Signature;
                    }
                    else
                    {
                        // File doesn't exist
                        userDetails.ListData[0].Signature = "abc.jpg";
                        officerSigFilePath = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "OfficerSignatures")).Root + userDetails.ListData[0].Signature;
                    }

                }
                else
                {
                    officerSigFilePath = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "OfficerSignatures")).Root + "deemed_engine_signature.jpg";
                }

                iTextSharp.text.Image signature = iTextSharp.text.Image.GetInstance(new FileStream(officerSigFilePath, FileMode.OpenOrCreate, FileAccess.Read));
                signature.ScaleToFit(120f, 120f);
                signature.SetAbsolutePosition(630f, 150f);
                documents.Add(signature);
                var empSig = (dynamic)null;


                var appSigDoc = _context.ApplicationDocuments.Where(x => x.AppRefId == appRefId && x.DocumentRefId == 20005).OrderByDescending(x => x.AppDocId).FirstOrDefault();
                if (appSigDoc != null)
                {
                    empSig = appSigDoc;
                }
                else
                {
                    if (licenceMappingInfo != null)
                    {
                        empSig = _context.ApplicationDocuments.Where(x => x.AppRefId == licenceMappingInfo.AppRefId && x.DocumentRefId == 20005).OrderByDescending(x => x.AppDocId).FirstOrDefault();
                    }
                }

                string empSignatureFileName = "";
                string employerSigFilePath = "";
                //empSig != null ? empSig.AttachmentName : shopFormModelDetails.ResponseDataModel.ApplicantSign;
                if (empSig != null)
                {
                    string filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "AppFiles", empSig.AttachmentName);
                    if (File.Exists(filePath))
                    {
                        employerSigFilePath = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "AppFiles")).Root + empSig.AttachmentName;
                    }
                    else
                    {
                        // File doesn't exist
                        empSignatureFileName = "abc.jpg";
                        officerSigFilePath = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "AppFiles")).Root + userDetails.ListData[0].Signature;
                    }
                }
                else if (empSig == null)
                {
                    var userRefId = _context.Applications.Where(x => x.AppId == appRefId && x.IsDeleted == false).Include(x => x.ProjectSites).Select(x => x.ProjectSites.UserRefId).FirstOrDefault();
                    var userProfileSignature = _context.UserProfileMapping.Where(x => x.UserRefId == userRefId).Include(x => x.UserProfile).Select(x => x.UserProfile.Signature).FirstOrDefault();
                    employerSigFilePath = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "AppFiles")).Root + userProfileSignature;

                    if (!File.Exists(employerSigFilePath))
                    {
                        if (shopFormModelDetails != null)
                        {
                            userProfileSignature = shopFormModelDetails.ResponseDataModel.ApplicantSign;

                            employerSigFilePath = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Legacy_images")).Root + userProfileSignature;
                        }
                    }


                }
                else
                {
                    empSignatureFileName = "abc.jpg";
                    employerSigFilePath = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "AppFiles")).Root + empSignatureFileName;
                }


                //pageLogo1 = iTextSharp.text.Image.GetInstance(new FileStream(officerSigFilePath, FileMode.OpenOrCreate, FileAccess.Read));

                iTextSharp.text.Image employerSignature = iTextSharp.text.Image.GetInstance(new FileStream(employerSigFilePath, FileMode.OpenOrCreate, FileAccess.Read));
                employerSignature.ScaleToFit(120f, 120f);
                employerSignature.SetAbsolutePosition(630f, 150f);
                documents.Add(employerSignature);


                #endregion

                #region Shop Form F Reports Detail

                PdfPTable ptableShopFormFDetail = new PdfPTable(4);
                ptableShopFormFDetail.DefaultCell.Border = 1;
                ptableShopFormFDetail.DefaultCell.Padding = 2;
                PdfPCell pFormFDetailcellLine1 = new PdfPCell(new Paragraph(""));
                pFormFDetailcellLine1.BorderWidth = 0;
                pFormFDetailcellLine1.Colspan = 4;
                ptableShopFormFDetail.AddCell(pFormFDetailcellLine1);
                //ptableShopFormFDetail.AddCell(new PdfPCell(new Phrase("FORM F", verdanaHeader)) { Border = 0, PaddingLeft = 10, PaddingTop = 35, HorizontalAlignment = PdfPCell.ALIGN_CENTER, Colspan = 4 });

                ptableShopFormFDetail.AddCell(new PdfPCell(new Phrase("PERMISSION FOR WORKING WOMEN IN NIGHT SHIFT", impactHeader)) { Border = 0, PaddingTop = 90, PaddingBottom = 50, PaddingLeft = 25, PaddingRight = 25, VerticalAlignment = PdfPCell.ALIGN_MIDDLE, HorizontalAlignment = PdfPCell.ALIGN_CENTER, Colspan = 4 });
                ptableShopFormFDetail.AddCell(new PdfPCell(new Phrase("1. Name and Father's/Husband's Name of Employer:", verdanaHeader)) { Border = 0, PaddingTop = 80, Colspan = 2, Padding = 6 });
                ptableShopFormFDetail.AddCell(new PdfPCell(new Phrase(shopFormModelDetails.ResponseDataModel.OName + " S/o, D/o, W/o: " + shopFormModelDetails.ResponseDataModel.OFHName, verdana)) { Border = 0, Colspan = 2, Padding = 6 });

                ptableShopFormFDetail.AddCell(new PdfPCell(new Phrase("2. Name of Manager , if any :", verdanaHeader)) { Border = 0, Colspan = 2, Padding = 6 });

                ptableShopFormFDetail.AddCell(new PdfPCell(new Phrase(shopFormModelDetails.ResponseDataModel.MgrName, verdana)) { Border = 0, Padding = 6, Colspan = 2 });


                ptableShopFormFDetail.AddCell(new PdfPCell(new Phrase("3. Name of the Establishment : ", verdanaHeader)) { Border = 0, Colspan = 2, Padding = 6 });
                ptableShopFormFDetail.AddCell(new PdfPCell(new Phrase(shopFormModelDetails.ResponseDataModel.EstdName, verdana)) { Border = 0, Colspan = 2, Padding = 6 });

                ptableShopFormFDetail.AddCell(new PdfPCell(new Phrase("4. Full Postal Address of the Establishment :", verdanaHeader)) { Border = 0, Colspan = 2, Padding = 6 });
                ptableShopFormFDetail.AddCell(new PdfPCell(new Phrase(shopFormModelDetails.ResponseDataModel.SiteAddress + ", " + shopFormModelDetails.ResponseDataModel.TehsilName + ", " + shopFormModelDetails.ResponseDataModel.PinCode, verdana)) { Border = 0, Colspan = 2, Padding = 6 });
                ptableShopFormFDetail.AddCell(new PdfPCell(new Phrase("5.1: Nature of Business ", verdanaHeader)) { Border = 0, Padding = 6, Colspan = 2 });


                if (shopFormModelDetails.ResponseDataModel.NationalIndustrialClassificationCode != null && licenceMappingInfo != null)
                {
                    string input = shopFormModelDetails.ResponseDataModel.NationalIndustrialClassificationCode;
                    List<string> values = new List<string>();
                    int startIndex = 0;
                    string nicCode = "";
                    string nicCodeDescription = "";

                    while (startIndex < input.Length)
                    {
                        int openQuoteIndex = input.IndexOf('"', startIndex);
                        if (openQuoteIndex == -1)
                            break;

                        int closeQuoteIndex = input.IndexOf('"', openQuoteIndex + 1);
                        if (closeQuoteIndex == -1)
                            break;

                        int valueLength = closeQuoteIndex - openQuoteIndex - 1;
                        if (valueLength > 0)
                        {
                            string extractedValue = input.Substring(openQuoteIndex + 1, valueLength);
                            values.Add(extractedValue);
                        }

                        startIndex = closeQuoteIndex + 1;
                    }
                    // Printing the extracted values
                    foreach (string value in values)
                    {
                        nicCode = value;
                        nicCodeDescription = _context.NICCodes.Where(n => n.NicCode == nicCode).Select(x => x.Description).FirstOrDefault();
                    }

                    if (nicCode != null)
                    {
                        ptableShopFormFDetail.AddCell(new PdfPCell(new Phrase(nicCode + "-" + nicCodeDescription, verdana)) { Border = 0, Padding = 6, Colspan = 2 });
                    }
                }
                else
                {
                    ptableShopFormFDetail.AddCell(new PdfPCell(new Phrase(shopFormModelDetails.ResponseDataModel.NationalIndustrialClassificationCode, verdana)) { Border = 0, Padding = 6, Colspan = 2 });


                }
                ptableShopFormFDetail.AddCell(new PdfPCell(new Phrase("5.2: Project Purpose :", verdanaHeader)) { Border = 0, Padding = 6, Colspan = 2 });
                ptableShopFormFDetail.AddCell(new PdfPCell(new Phrase(shopFormModelDetails.ResponseDataModel.ProjectPurpose, verdana)) { Border = 0, Padding = 6, Colspan = 2 });



                ptableShopFormFDetail.AddCell(new PdfPCell(new Phrase("6. No. of Employees if any:", verdanaHeader)) { Border = 0, Colspan = 2, Padding = 6 });
                ptableShopFormFDetail.AddCell(new PdfPCell(new Phrase(shopFormModelDetails.ResponseDataModel.TotalEmployees.ToString(), verdana)) { Border = 0, Colspan = 2, Padding = 6 });

                ptableShopFormFDetail.AddCell(new PdfPCell(new Phrase("7. No and Date of Previous Registration Certificate Under Shop Act:", verdanaHeader)) { Border = 0, Colspan = 2, Padding = 6 });
                ptableShopFormFDetail.AddCell(new PdfPCell(new Phrase(formData.LicenceNumber + ", " + shopFormModelDetails.ResponseDataModel.ClearanceIssuedOn.ToString(), verdana)) { Border = 0, Colspan = 2, Padding = 6 });

                ptableShopFormFDetail.AddCell(new PdfPCell(new Phrase("8. Date of Permission Granted for Night Working Women in The Night Shift Act. :", verdanaHeader)) { Border = 0, Colspan = 2, Padding = 6 });
                ptableShopFormFDetail.AddCell(new PdfPCell(new Phrase(DateTime.Now.ToString(), verdana)) { Border = 0, Colspan = 2, Padding = 6 });

                ptableShopFormFDetail.AddCell(new PdfPCell(new Phrase("9. Granted for Night Working Women in The Night Shift Act Permission Valid Till :", verdanaHeader)) { Border = 0, Colspan = 2, Padding = 6 });
                ptableShopFormFDetail.AddCell(new PdfPCell(new Phrase("31-Dec-" + DateTime.Now.Year.ToString(), verdana)) { Border = 0, Colspan = 2, Padding = 6 });


                #endregion

                PdfPTable ptableShopDetail = new PdfPTable(4);
                ptableShopDetail.DefaultCell.Border = 0;
                ptableShopDetail.DefaultCell.Padding = 2;
                PdfPCell pDetailcellLine1 = new PdfPCell(new Paragraph(""));
                pDetailcellLine1.BorderWidth = 0;
                pDetailcellLine1.Colspan = 4;
                ptableShopDetail.AddCell(pDetailcellLine1);
                ptableShopDetail.AddCell(new PdfPCell(new Phrase("Note: The employer will abide by the undertaking given by him/her regarding all the above conditions mentioned in the permission granted for working women in the night shift " + shopFormModelDetails.ResponseDataModel.EstdName + ". If any of the conditions are violated or not followed as per the permission given, then legal action will be initiated against the employer.", verdanaHeader)) { Border = 0, PaddingLeft = 10, PaddingTop = 35, HorizontalAlignment = PdfPCell.ALIGN_JUSTIFIED, Colspan = 4 });


                ptableShopDetail.AddCell(new PdfPCell(signature) { Border = 0, Colspan = 2, PaddingTop = 40 });
                ptableShopDetail.AddCell(new PdfPCell(employerSignature) { Border = 0, Colspan = 2, PaddingTop = 40, HorizontalAlignment = PdfPCell.ALIGN_RIGHT });

                ptableShopDetail.AddCell(new PdfPCell(new Phrase("Labour Commissioner", verdanaHeader)) { Border = 0, Colspan = 3 });
                ptableShopDetail.AddCell(new PdfPCell(new Phrase("Signature of Employer", verdanaHeader)) { Border = 0, Colspan = 1, HorizontalAlignment = PdfPCell.ALIGN_LEFT });

                //ptableShopDetail.AddCell(new PdfPCell(new Phrase("", verdana)) { Border = 0, Colspan = 4 });
                ptableShopDetail.AddCell(new PdfPCell(new Phrase("Labour Bhawan Model Welfare Centre.", verdana)) { Border = 0, Colspan = 3 });
                ptableShopDetail.AddCell(new PdfPCell(new Phrase(shopFormModelDetails.ResponseDataModel.OName, verdana)) { Border = 0, Colspan = 1, HorizontalAlignment = PdfPCell.ALIGN_LEFT });


                ptableShopDetail.AddCell(new PdfPCell(new Phrase("Phase 10. Sector 64, ", verdana)) { Border = 0, Colspan = 3 });
                ptableShopDetail.AddCell(new PdfPCell(new Phrase(shopFormModelDetails.ResponseDataModel.SiteAddress + ", " + shopFormModelDetails.ResponseDataModel.TehsilName + ", " + shopFormModelDetails.ResponseDataModel.PinCode, verdana)) { Border = 0, Colspan = 1, HorizontalAlignment = PdfPCell.ALIGN_LEFT });

                ptableShopDetail.AddCell(new PdfPCell(new Phrase("Sahibzada Ajit Singh Nagar, Punjab. 160062", verdana)) { Border = 0, Colspan = 3, PaddingTop = 0 });
                ptableShopDetail.AddCell(new PdfPCell(new Phrase("", verdana)) { Border = 0, Colspan = 1 });

                ptableShopDetail.AddCell(pDetailcellLine1);
                ptableShopDetail.AddCell(new PdfPCell(new Phrase("This is a computer generated document. No signature is required..!", verdanaHeader)) { Border = 0, PaddingLeft = 10, PaddingTop = 35, HorizontalAlignment = PdfPCell.ALIGN_JUSTIFIED, Colspan = 4 });

                //#endregion

                #region Footer
                PdfPTable ptablefooterLine = new PdfPTable(3);
                ptablefooterLine.DefaultCell.Border = 0;
                ptablefooterLine.DefaultCell.Padding = 8;
                ptablefooterLine.SpacingAfter = 10;
                PdfPCell pcellLinefooter = new PdfPCell(new Paragraph(""));
                pcellLinefooter.BorderWidthBottom = 1;
                pcellLinefooter.BorderWidthTop = 0;
                pcellLinefooter.BorderWidthRight = 0;
                pcellLinefooter.BorderWidthLeft = 0;
                pcellLinefooter.Colspan = 3;
                ptablefooterLine.AddCell(pcellLinefooter);

                Paragraph AppHeadingFooter = new Paragraph("This is system generated document Hence no signature required.", verdana);

                AppHeadingFooter.Alignment = Element.ALIGN_CENTER;
                #endregion

                PdfPTable ptableShopDetailLicenceAppliedFormF = new PdfPTable(3);
                ptableShopDetailLicenceAppliedFormF.DefaultCell.Border = 0;
                ptableShopDetailLicenceAppliedFormF.DefaultCell.Padding = 2;
                ptableShopDetailLicenceAppliedFormF.SpacingBefore = 1000;
                ptableShopDetailLicenceAppliedFormF.AddCell(new PdfPCell(ptableleftSideDetailFormF) { Border = 0, PaddingTop = 300, PaddingLeft = -25, PaddingRight = 40 });
                ptableShopDetailLicenceAppliedFormF.AddCell(new PdfPCell(ptableShopFormFDetail) { Border = 0, Colspan = 3 });
                documents.Add(ptableShopDetailLicenceAppliedFormF);
                documents.NewPage();
                PdfPTable ptableShopAnnexure = new PdfPTable(7);
                ptableShopAnnexure.DefaultCell.Border = 0;
                ptableShopAnnexure.DefaultCell.Padding = 2;
                int sno = 1;
                ptableShopAnnexure.AddCell(new PdfPCell(new Phrase("The conditions for permission of working women on the night shift:", verdanaHeader)) { Padding = 20, Colspan = 7, Border = 0, Left = 40 });


                var selectedIds = JsonConvert.DeserializeObject<List<Int64>>(formData.CheckListJson);
                var checklistPoints = _context.Licence_Shop_NightShift_ChecklistPoints.Where(x => x.IsEnabled == true && selectedIds.Contains(x.ChecklistID)).ToList();
                ///*genericFormModel.FormModel.NightShift_ChecklistPoint*/s = genericFormModel.FormModel.NightShift_ChecklistPoints.Select(x => { x.IsSelected = (selectedIds.Contains(x.ChecklistID)); return x; }).ToList();

                foreach (var item in checklistPoints)
                {
                    ptableShopAnnexure.AddCell(new PdfPCell(new Phrase((sno++).ToString(), verdana)) { Border = 1, BorderWidthTop = 1, BorderWidthBottom = 1, BorderWidthLeft = 1, BorderWidthRight = 1 });
                    ptableShopAnnexure.AddCell(new PdfPCell(new Phrase(item.ChecklistDescription, verdana)) { Border = 1, BorderWidthTop = 1, BorderWidthBottom = 1, BorderWidthRight = 1, Colspan = 6, Padding = 6 });

                }

                documents.Add(ptableShopAnnexure);
                documents.NewPage();
                //documents.Add(pageLogo1);
                PdfPTable ptableShopDetailLicenceAppliedFormB = new PdfPTable(3);
                ptableShopDetailLicenceAppliedFormB.DefaultCell.Border = 0;
                ptableShopDetailLicenceAppliedFormB.DefaultCell.Padding = 2;
                ptableShopDetailLicenceAppliedFormB.SpacingBefore = 1000;
                //ptableShopDetailLicenceAppliedFormB.AddCell(new PdfPCell(ptableleftSideDetailFormF) { Border = 0, PaddingTop = 300, PaddingLeft = -25, PaddingRight = 40 });
                ptableShopDetailLicenceAppliedFormB.AddCell(new PdfPCell(ptableShopDetail) { Border = 0, Colspan = 3 });
                documents.Add(ptableShopDetailLicenceAppliedFormB);
                documents.NewPage();
                //documents.Add(ptableShopAnnexure);
                //documents.NewPage();
                documents.Close();
                PDFPageEvents pageeventsobj = new PDFPageEvents();
            }
            catch (Exception ex)
            {
                genericFormModel.HasException = true;
                genericFormModel.Exceptions = ex.Message;
            }
            return genericFormModel;
        }
        //private async Task<CertificateGenerateServiceResultTemplate> Certificate_AppForm_72(Int64 appRefId, string CertificatePerformaURL, ApplicationTypeEnum applicationType, string userName, string licenceNo, bool isDeemedCalling)
        //{
        //    CertificateGenerateServiceResultTemplate genericFormModel = new CertificateGenerateServiceResultTemplate();
        //    try
        //    {
        //        Guid guId = Guid.NewGuid();
        //        genericFormModel.PdfNameGUID = guId.ToString();
        //        genericFormModel.HasException = false;
        //        var SaveDirectoryPath = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "TempFiles")).Root + guId.ToString() + ".pdf";
        //        iTextSharp.text.Document documents = new iTextSharp.text.Document(iTextSharp.text.PageSize.A4, 5, 5, 30, 30);

        //        #region ClassObjects
        //        pdfFonts classFonts = new pdfFonts();
        //        #endregion

        //        #region CreateObjects
        //        PdfWriter owriter = PdfWriter.GetInstance(documents, new FileStream(SaveDirectoryPath, FileMode.Create));
        //        PDFPageEvents pageEvent = new PDFPageEvents();
        //        owriter.PageEvent = pageEvent;
        //        owriter.SetEncryption(PdfWriter.STRENGTH128BITS, "", "", PdfWriter.AllowPrinting);
        //        #endregion

        //        #region Regester Font Style
        //        var fontName = "Verdana";
        //        Font font = FontFactory.GetFont(fontName, BaseFont.IDENTITY_H, 16);
        //        Font fontSmall = FontFactory.GetFont(fontName, BaseFont.IDENTITY_H, 12);
        //        Font fontBOLD = FontFactory.GetFont(fontName, BaseFont.IDENTITY_H, 16, iTextSharp.text.Font.BOLD);
        //        //BaseFont customfontimpact = BaseFont.CreateFont(fontName, BaseFont.CP1252, BaseFont.EMBEDDED);
        //        #endregion

        //        #region Tables
        //        //var labourLicence = _context.Licence_ContractLabour_GeneralDetails.Where(x => x.AppRefId == appRefId).FirstOrDefault();
        //        //var application = _context.Applications.Where(x => x.AppId == appRefId).FirstOrDefault();
        //        //var projectSite = _context.ProjectSites.Where(x => x.ProjectSiteId == application.ProjectSiteRefId).FirstOrDefault();
        //        //var labourCircle = _context.LabourCircles.Where(x => x.LabourCircleId == projectSite.LabourCircleRefId).FirstOrDefault();
        //        //var applicationAction = _context.ApplicationActions.Where(x => x.ApplicationRefId == application.AppId).FirstOrDefault();
        //        //var feeDetails = _context.AppFeeDetails.Where(x => x.AppRefId == appRefId).FirstOrDefault();

        //        var result = (from labourLicence in _context.Licence_ContractLabour_GeneralDetails
        //                      join application in _context.Applications on labourLicence.AppRefId equals application.AppId
        //                      join projectSite in _context.ProjectSites on application.ProjectSiteRefId equals projectSite.ProjectSiteId
        //                      join labourCircle in _context.LabourCircles on projectSite.LabourCircleRefId equals labourCircle.LabourCircleId
        //                      join applicationAction in _context.ApplicationActions on application.AppId equals applicationAction.ApplicationRefId
        //                      join feeDetails in _context.AppFeeDetails on labourLicence.AppRefId equals feeDetails.AppRefId
        //                      where labourLicence.AppRefId == appRefId
        //                      select new
        //                      {
        //                          labourLicence = labourLicence,
        //                          application = application,
        //                          projectSite = projectSite,
        //                          labourCircle = labourCircle,
        //                          applicationAction = applicationAction,
        //                          feeDetails = feeDetails
        //                      }).FirstOrDefault();

        //        #endregion

        //        #region Licence Validity Year
        //        string licenceValidUpto = "";
        //        DateTime registrationDate = result.labourLicence.RegistrationCertificateDate.HasValue ? result.labourLicence.RegistrationCertificateDate.Value : DateTime.MinValue;
        //        int year = DateTime.Now.Year;
        //        licenceValidUpto = new DateTime(year, 12, 31).ToString();
        //        //if (application.ApplicationPurposeType == ApplicationPurposeTypeEnum.GRANT_LICENCE)
        //        //{
        //        //    DateTime registrationDate = labourLicence.RegistrationCertificateDate.HasValue ? labourLicence.RegistrationCertificateDate.Value : DateTime.MinValue;
        //        //    int year = DateTime.Now.Year;
        //        //    licenceValidUpto = new DateTime(year, 12, 31).ToString(); 
        //        //}

        //        #endregion

        //        string fontpath1 = "Content/fonts/VERDANA.TTF";
        //        BaseFont customfont = BaseFont.CreateFont(fontpath1, BaseFont.CP1252, BaseFont.EMBEDDED);
        //        string fontpath2 = "Content/fonts/VERDANA.TTF";
        //        BaseFont customfontimpact = BaseFont.CreateFont(fontpath2, BaseFont.CP1252, BaseFont.EMBEDDED);
        //        documents.Open();
        //        Font verdanaHeader = FontFactory.GetFont("Arial", 9, Font.BOLD);
        //        Font verdana = FontFactory.GetFont("Arial", 9, BaseColor.BLACK);
        //        Font verdanaBoldItalic = FontFactory.GetFont("Arial", 8, Font.BOLDITALIC);
        //        Font verdanasmall = FontFactory.GetFont("Arial", 8, Font.NORMAL);
        //        verdanasmall.Color = BaseColor.WHITE;
        //        Font verdanaRightHeader = FontFactory.GetFont("Arial", 15, Font.BOLD);
        //        verdanaRightHeader.Color = BaseColor.WHITE;
        //        Font verdanaRightHeader1 = FontFactory.GetFont("Arial", 12, Font.BOLD);
        //        verdanaRightHeader1.Color = BaseColor.WHITE;
        //        Font verdanaRightHeader2 = FontFactory.GetFont("Arial", 9, Font.BOLD);
        //        verdanaRightHeader2.Color = BaseColor.WHITE;
        //        Font monotype = FontFactory.GetFont("Arial", 13, Font.BOLD);
        //        Font monotypeHeader = FontFactory.GetFont("Arial", 15, Font.BOLD);
        //        Font impact = new Font(fontSmall.BaseFont, 12);
        //        Font impactHeader = new Font(fontSmall.BaseFont, 12);
        //        impactHeader.Color = BaseColor.WHITE;
        //        BaseFont bfTimes = BaseFont.CreateFont(BaseFont.TIMES_ROMAN, BaseFont.CP1252, false);
        //        Font timesRoman = FontFactory.GetFont("Arial", 10, BaseColor.BLACK);
        //        Font verdanaBold = FontFactory.GetFont("Arial", 9, Font.BOLD);
        //        iTextSharp.text.Image pageLogo1;

        //        var contract_LabourPerforma = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "LicensePerformas")).Root + "CertificatePerforma_Shop.jpg";
        //        pageLogo1 = iTextSharp.text.Image.GetInstance(new FileStream(contract_LabourPerforma, FileMode.OpenOrCreate, FileAccess.Read));
        //        //pageLogo1 = iTextSharp.text.Image.GetInstance("~/Content/images/CertificatePerforma_Shop.jpg");
        //        pageLogo1.Alignment = iTextSharp.text.Image.ALIGN_CENTER | iTextSharp.text.Image.UNDERLYING;
        //        pageLogo1.ScaleToFit(800f, 800f);
        //        pageLogo1.SetAbsolutePosition(10f, 25f);
        //        pageLogo1.BorderWidth = 1;
        //        documents.Add(pageLogo1);
        //        PdfPTable ptableleftSideDetail = new PdfPTable(1);
        //        ptableleftSideDetail.DefaultCell.Border = 0;
        //        ptableleftSideDetail.DefaultCell.Padding = 0;
        //        ptableleftSideDetail.HorizontalAlignment = (Element.ALIGN_JUSTIFIED);
        //        ptableleftSideDetail.DefaultCell.VerticalAlignment = (Element.ALIGN_JUSTIFIED);
        //        ptableleftSideDetail.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
        //        ptableleftSideDetail.AddCell(new PdfPCell(new Phrase("", verdanaHeader)) { Border = 0, PaddingTop = 0 });

        //        ptableleftSideDetail.AddCell(new PdfPCell(new Phrase("Licence No. : " + licenceNo.Replace("/", ""), verdanaRightHeader2)) { Border = 0, PaddingTop = 320, PaddingLeft = -25, });
        //        ptableleftSideDetail.AddCell(new PdfPCell(new Phrase("Licence Issued On : " + String.Format("{0:dd-MMM-yyyy}", DateTime.Now.ToShortDateString()), verdanaRightHeader2)) { Border = 0, PaddingTop = 8, PaddingLeft = -25 });
        //        ptableleftSideDetail.AddCell(new PdfPCell(new Phrase("Valid upto : " + licenceValidUpto.ToString(), verdanaRightHeader2)) { Border = 0, PaddingTop = 8, PaddingLeft = -25 });
        //        ptableleftSideDetail.AddCell(new PdfPCell(new Phrase("Department of Labour", verdanaRightHeader2)) { Border = 0, PaddingTop = 80, PaddingLeft = -15 });
        //        ptableleftSideDetail.AddCell(new PdfPCell(new Phrase("      Govt. of Punjab ", verdanaRightHeader2)) { Border = 0, PaddingTop = 0, PaddingLeft = -15 });


        //        PdfPTable ptableleftSideDetailFormF = new PdfPTable(1);
        //        ptableleftSideDetailFormF.DefaultCell.Border = 0;
        //        ptableleftSideDetailFormF.DefaultCell.Padding = 0;
        //        ptableleftSideDetailFormF.HorizontalAlignment = (Element.ALIGN_JUSTIFIED);
        //        ptableleftSideDetailFormF.DefaultCell.VerticalAlignment = (Element.ALIGN_JUSTIFIED);
        //        ptableleftSideDetailFormF.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
        //        PdfPTable tblCurrLicDetail = new PdfPTable(1);
        //        Font verdanaBoldHeader = FontFactory.GetFont("Arial", 9, Font.BOLD);
        //        verdanaBoldHeader.Color = BaseColor.WHITE;


        //        ptableleftSideDetailFormF.AddCell(new PdfPCell(tblCurrLicDetail) { Border = 0, FixedHeight = 100, PaddingTop = 8, PaddingBottom = 8 });

        //        #region QR Code Generate
        //        var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(result.application.PublicAppRefNum);
        //        string base64String = System.Convert.ToBase64String(plainTextBytes);
        //        QRCodeGenerator qrGenerator = new QRCodeGenerator();
        //        QRCodeGenerator.QRCode qrCode = qrGenerator.CreateQrCode(base64String, QRCodeGenerator.ECCLevel.Q);

        //        using (System.Drawing.Bitmap bitMap = qrCode.GetGraphic(20))
        //        {
        //            using (MemoryStream ms = new MemoryStream())
        //            {
        //                bitMap.Save(ms, ImageFormat.Jpeg);
        //                System.Drawing.Image imgQRCode = System.Drawing.Image.FromStream(ms);
        //                iTextSharp.text.Image imdf = iTextSharp.text.Image.GetInstance(imgQRCode, System.Drawing.Imaging.ImageFormat.Jpeg);
        //                imdf.ScaleAbsoluteHeight(120f);
        //                imdf.ScaleAbsoluteWidth(120f);
        //                ptableleftSideDetail.AddCell(new PdfPCell(imdf) { Border = 0, PaddingTop = 100, PaddingLeft = -12, PaddingRight = 5 });
        //            }
        //        }
        //        #endregion

        //        ptableleftSideDetailFormF.AddCell(new PdfPCell(new Phrase("", verdanaRightHeader1)) { Border = 0, PaddingTop = 120, PaddingLeft = 10 });
        //        ptableleftSideDetailFormF.AddCell(new PdfPCell(new Phrase("", verdanaRightHeader1)) { Border = 0, PaddingTop = 0, PaddingLeft = 25 });

        //        PdfPTable ptablerightSideDetail = new PdfPTable(1);
        //        ptablerightSideDetail.DefaultCell.Border = 0;
        //        ptablerightSideDetail.DefaultCell.Padding = 0;
        //        ptablerightSideDetail.AddCell(new PdfPCell(new Phrase("", verdanaBold)) { Border = 0, Padding = 0 });
        //        ptablerightSideDetail.AddCell(new PdfPCell(new Phrase("FORM-VI", verdanaHeader)) { Border = 0, PaddingLeft = 20, PaddingTop = 35, HorizontalAlignment = PdfPCell.ALIGN_CENTER });
        //        ptablerightSideDetail.AddCell(new PdfPCell(new Phrase("(See Rule-25 (1))", verdanaHeader)) { Border = 0, PaddingLeft = 20, HorizontalAlignment = PdfPCell.ALIGN_CENTER });
        //        ptablerightSideDetail.AddCell(new PdfPCell(new Phrase("LICENSING OF CONTRACTOR UNDER THE CONTRACT  LABOUR (R & A) ACT, 1970 ", impactHeader)) { Border = 0, PaddingTop = 20, PaddingBottom = 20, PaddingLeft = 55, PaddingRight = 40, VerticalAlignment = PdfPCell.ALIGN_MIDDLE, HorizontalAlignment = PdfPCell.ALIGN_CENTER });
        //        ptablerightSideDetail.AddCell(new PdfPCell(new Phrase("", monotype)) { Border = 0, PaddingBottom = 10, PaddingTop = 20, PaddingLeft = 80 });
        //        ptablerightSideDetail.AddCell(new PdfPCell(new Phrase("", verdanaBold)) { Border = 0, Padding = 5 });
        //        ptablerightSideDetail.AddCell(new PdfPCell(new Phrase("GOVERNMENT OF PUNJAB", monotypeHeader)) { Border = 0, PaddingTop = 45, PaddingBottom = 5, HorizontalAlignment = PdfPCell.ALIGN_CENTER });
        //        ptablerightSideDetail.AddCell(new PdfPCell(new Phrase("", verdanaBold)) { Border = 0, Padding = 5 });
        //        ptablerightSideDetail.AddCell(new PdfPCell(new Phrase(new Chunk("Licence", monotypeHeader).SetUnderline(0.1f, -2f))) { Border = 0, PaddingTop = 10, PaddingLeft = 120 });


        //        Paragraph para = new Paragraph();
        //        Paragraph para1 = new Paragraph();
        //        para1.Add(new Chunk("1. Licence is hereby granted to ", timesRoman));
        //        para1.Add(new Chunk(result.labourLicence.ContractorName, verdanaBold).SetUnderline(0.1f, -2f));
        //        para1.Add(new Chunk(" work under ", timesRoman));
        //        para1.Add(new Chunk(result.projectSite.EstablishmentName, verdanaBold).SetUnderline(0.1f, -2f));
        //        para1.Add(new Chunk(" under section 12(1) of the Contract Labour (Regulation and Abolition) Act, 1970, subject to the conditions specified in Annexure).", timesRoman));
        //        para1.Add(new Chunk("", timesRoman));
        //        para1.Add(new Chunk("", timesRoman));
        //        para1.Add(new Chunk("", timesRoman));
        //        para1.Add(new Chunk(Chunk.NEWLINE + "2. This licence is for doing the work of ", timesRoman));
        //        para1.Add(new Chunk(result.labourLicence.NatureOfWork, verdanaBold).SetUnderline(0.1f, -2f));
        //        para1.Add(new Chunk("(nature of work to be indicated) in the establishment of ", timesRoman));
        //        para1.Add(new Chunk(result.labourLicence.PrincipalEmployerName, verdanaBold).SetUnderline(0.1f, -2f));
        //        para1.Add(new Chunk(" (name of principal employer to be indicated) of ", timesRoman));
        //        para1.Add(new Chunk(result.labourLicence.PrincipalEmployerAddress, verdanaBold).SetUnderline(0.1f, -2f));
        //        para1.Add(new Chunk("(place of work to be indicated).", timesRoman));

        //        para1.Add(new Chunk("", timesRoman));
        //        para1.Add(new Chunk(Chunk.NEWLINE + "3. The Licence shall remain in force till ", timesRoman));
        //        para1.Add(new Chunk(licenceValidUpto.ToString(), verdanaBold).SetUnderline(0.1f, -2f));
        //        para1.Add(new Chunk(Chunk.NEWLINE + "4. The Licence valid for number of workers upto ", timesRoman));
        //        para1.Add(new Chunk(result.labourLicence.MaximumNumberOfEmployee.ToString(), timesRoman));


        //        PdfPCell shipment1 = new PdfPCell(para1);
        //        shipment1.SetLeading(1f, 2f);
        //        ptablerightSideDetail.AddCell(new PdfPCell(shipment1) { Border = 0, VerticalAlignment = PdfPCell.ALIGN_JUSTIFIED, HorizontalAlignment = PdfPCell.ALIGN_JUSTIFIED, FixedHeight = 300 });
        //        var appDetails = _context.Applications.Where(x => x.AppId == result.labourLicence.AppRefId).FirstOrDefault();

        //        PdfPTable myspacee = new PdfPTable(1);
        //        myspacee.SpacingAfter = 10;
        //        myspacee.AddCell(new PdfPCell(new Phrase("", verdanaBold)) { Border = 0, Padding = 15 });
        //        ptablerightSideDetail.AddCell(new PdfPCell(myspacee) { Border = 0, VerticalAlignment = PdfPCell.ALIGN_JUSTIFIED, HorizontalAlignment = PdfPCell.ALIGN_JUSTIFIED });
        //        PdfPTable myTable = new PdfPTable(4);
        //        myTable.SpacingAfter = 10;
        //        string signatures = "";

        //        var userDetails = await _iAuthService.GetUserFullDetailByUserName(userName);
        //        string officerSigFilePath = "";
        //        if (!isDeemedCalling)
        //        {
        //            if (userDetails.ListData.Count() == 0 || userDetails.ListData[0].Signature == null)
        //            {
        //                userDetails.ListData[0].Signature = "abc.jpg";
        //            }
        //            string filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "OfficerSignatures", userDetails.ListData[0].Signature);
        //            // Checking if the file exists
        //            if (File.Exists(filePath))
        //            {
        //                officerSigFilePath = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "OfficerSignatures")).Root + userDetails.ListData[0].Signature;
        //            }
        //            else
        //            {
        //                // File doesn't exist
        //                userDetails.ListData[0].Signature = "abc.jpg";
        //                officerSigFilePath = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "OfficerSignatures")).Root + userDetails.ListData[0].Signature;
        //            }

        //        }
        //        else
        //        {
        //            officerSigFilePath = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "OfficerSignatures")).Root + "deemed_engine_signature.jpg";
        //        }

        //        //Font SealFontArial = FontFactory.GetFont("Arial", 9, Color.BLACK);
        //        Paragraph SignSeal = new Paragraph();
        //        SignSeal.Add(new Chunk("Name: ", verdanaHeader));
        //        SignSeal.Add(new Chunk(userDetails.ListData[0].UserFullName, verdanaBold));
        //        SignSeal.Add(new Chunk(Chunk.NEWLINE + "Desig: ", verdanaHeader));
        //        SignSeal.Add(new Chunk("Assistant Labour commissioner", verdanaBold));
        //        SignSeal.Add(new Chunk(Chunk.NEWLINE + "Circle Name: ", verdanaHeader));
        //        SignSeal.Add(new Chunk(result.labourCircle.LabourCircleName, verdanaBold));
        //        SignSeal.Add(new Chunk(Chunk.NEWLINE + "District Name: ", verdanaHeader));
        //        if (result.labourCircle.LabourCircleName.Contains("-"))
        //        {
        //            SignSeal.Add(new Chunk(result.labourCircle.LabourCircleName.Substring(0, result.labourCircle.LabourCircleName.IndexOf("-")).Trim(), verdanaBold));
        //        }
        //        else
        //        {
        //            SignSeal.Add(new Chunk(result.labourCircle.LabourCircleName, verdanaBold));
        //        }
        //        ptablerightSideDetail.AddCell(new PdfPCell(new Phrase("Signature of seal of the Licensing Officer", verdanaHeader)) { Border = 0, HorizontalAlignment = PdfPCell.ALIGN_RIGHT, PaddingBottom = 10 });
        //        ptablerightSideDetail.AddCell(new PdfPCell(SignSeal) { Border = 0, HorizontalAlignment = PdfPCell.ALIGN_LEFT, PaddingLeft = 150, PaddingBottom = 10 });

        //        Font fontTNR = new Font(BaseFont.CreateFont(BaseFont.TIMES_ROMAN, BaseFont.CP1252, false), 10, Font.NORMAL | Font.ITALIC);
        //        ptablerightSideDetail.AddCell(new PdfPCell(new Phrase("This is a computer generated document to check & verify please login to https://pblabour.gov.in/Account/ApplicationStatus", fontTNR)) { Border = 1, BorderWidth = 1, Padding = 5 });
        //        PdfPTable ptableShopDetailLicenceApplied = new PdfPTable(3);
        //        ptableShopDetailLicenceApplied.DefaultCell.Border = 0;
        //        ptableShopDetailLicenceApplied.DefaultCell.Padding = 2;
        //        ptableShopDetailLicenceApplied.SpacingBefore = 1000;
        //        ptableShopDetailLicenceApplied.AddCell(new PdfPCell(ptableleftSideDetail) { Border = 0 });
        //        ptableShopDetailLicenceApplied.AddCell(new PdfPCell(ptablerightSideDetail) { Border = 0, Colspan = 2 });
        //        documents.Add(ptableShopDetailLicenceApplied);
        //        documents.NewPage();
        //        PdfPTable ptableLabourAnnexure = new PdfPTable(7);
        //        ptableLabourAnnexure.DefaultCell.Border = 0;
        //        ptableLabourAnnexure.DefaultCell.Padding = 2;
        //        int sno = 1;

        //        ptableLabourAnnexure.AddCell(new PdfPCell(new Phrase("ANNEXURE", verdanaHeader)) { Padding = 20, PaddingLeft = 220, Colspan = 7, Border = 0, Left = 100 });
        //        ptableLabourAnnexure.AddCell(new PdfPCell(new Phrase("The licence is subject to the following conditions :-", verdana)) { Padding = 20, Colspan = 7, Border = 0, Left = 40 });
        //        ptableLabourAnnexure.AddCell(new PdfPCell(new Phrase((sno++).ToString(), verdana)) { Border = 1, BorderWidthTop = 1, BorderWidthBottom = 1, BorderWidthLeft = 1, BorderWidthRight = 1 });
        //        ptableLabourAnnexure.AddCell(new PdfPCell(new Phrase("The licence shall be non-transferable.", verdana)) { Border = 1, BorderWidthTop = 1, BorderWidthBottom = 1, BorderWidthRight = 1, Colspan = 6, Padding = 6 });
        //        ptableLabourAnnexure.AddCell(new PdfPCell(new Phrase((sno++).ToString(), verdana)) { Border = 1, BorderWidthTop = 1, BorderWidthBottom = 1, BorderWidthLeft = 1, BorderWidthRight = 1 });
        //        ptableLabourAnnexure.AddCell(new PdfPCell(new Phrase("The number of workmen employed as contract labour in the establishment shall not, on any day exceed " + result.labourLicence.MaximumNumberOfEmployee, verdana)) { Border = 1, BorderWidthTop = 1, BorderWidthBottom = 1, BorderWidthRight = 1, Colspan = 6, Padding = 6 });
        //        ptableLabourAnnexure.AddCell(new PdfPCell(new Phrase((sno++).ToString(), verdana)) { Border = 1, BorderWidthTop = 0, BorderWidthBottom = 1, BorderWidthLeft = 1, BorderWidthRight = 1 });
        //        ptableLabourAnnexure.AddCell(new PdfPCell(new Phrase("Except as provided in the rules the fees paid for the grant or as the case may be, for renewal of the licence shall be non-refundable .", verdana)) { Border = 1, BorderWidthTop = 0, BorderWidthBottom = 1, BorderWidthRight = 1, Colspan = 6, Padding = 6 });
        //        ptableLabourAnnexure.AddCell(new PdfPCell(new Phrase((sno++).ToString(), verdana)) { Border = 1, BorderWidthTop = 0, BorderWidthBottom = 1, BorderWidthLeft = 1, BorderWidthRight = 1 });
        //        ptableLabourAnnexure.AddCell(new PdfPCell(new Phrase("The rates of wages payble to the workmen by the contractor shall not be less than the rates prescribed for the Schedule of employment under the Minimum Wages Act, 1948, where applicable, and where the rates have been fixed by agreement, settlement or award, not less than the  rates fixed.", verdana)) { Border = 1, BorderWidthTop = 0, BorderWidthBottom = 1, BorderWidthRight = 1, Colspan = 6, Padding = 6 });
        //        ptableLabourAnnexure.AddCell(new PdfPCell(new Phrase((sno++).ToString(), verdana)) { Border = 1, BorderWidthTop = 0, BorderWidthBottom = 1, BorderWidthLeft = 1, BorderWidthRight = 1 });
        //        ptableLabourAnnexure.AddCell(new PdfPCell(new Phrase("In case where the workmen employed by the contractor perform the same or similar kind of work as the workmen directly employed by the principal employer of the establishment, the wages rates, holidays, hours of work and other conditions of service of the workmen of the contractor shall be the same as applicable to the workmen directly employed by the principal employer of the establishment on the same or similar kind of work: Provided that in the case of any disagreement with regard to the type of work the same shall be decided by the [Deputy Chief Labour Commissioner (Central)] whose decision shall be final.", verdana)) { Border = 1, BorderWidthTop = 0, BorderWidthBottom = 1, BorderWidthRight = 1, Colspan = 6, Padding = 6 });
        //        ptableLabourAnnexure.AddCell(new PdfPCell(new Phrase("", verdana)) { Border = 1, BorderWidthTop = 0, BorderWidthBottom = 1, BorderWidthLeft = 1, BorderWidthRight = 1 });
        //        ptableLabourAnnexure.AddCell(new PdfPCell(new Phrase("  Provided that in the case of any disagreement with regard to the type of work the same shall be decided by the [Deputy Chief Labour Commissioner (Central)] whose decision shall be final.", verdana)) { Border = 1, BorderWidthTop = 0, BorderWidthBottom = 1, BorderWidthRight = 1, Colspan = 6, Padding = 6 });
        //        ptableLabourAnnexure.AddCell(new PdfPCell(new Phrase((sno++).ToString(), verdana)) { Border = 1, BorderWidthTop = 0, BorderWidthBottom = 1, BorderWidthLeft = 1, BorderWidthRight = 1 });
        //        ptableLabourAnnexure.AddCell(new PdfPCell(new Phrase("In other case the wages rates, holidays, hours of work and condistions of service of the workmen of the contractor shall be such as may be specified in this  behalf by the [Deputy Chief Labour Commissioner (Central)].", verdana)) { Border = 1, BorderWidthTop = 0, BorderWidthBottom = 1, BorderWidthRight = 1, Colspan = 6, Padding = 6 });
        //        ptableLabourAnnexure.AddCell(new PdfPCell(new Phrase((sno++).ToString(), verdana)) { Border = 1, BorderWidthTop = 0, BorderWidthBottom = 1, BorderWidthLeft = 1, BorderWidthRight = 1 });
        //        ptableLabourAnnexure.AddCell(new PdfPCell(new Phrase("In every establishment where 20  or more women are ordinarily employed as contract labour there shall be provided 2 rooms of reasonable dimension for the use of their children under the age of six years. One of such rooms would be used as a play room for the children and the other as bed room for the children. For this purpose the contractor shall supply adequate number of toys and games in the play room and sufficient number of cots and beddings in the sleeping rooms. The standard of construction and maintenance of the creches may be such as may be specified in this behalf by the [Deputy Chief Labour Commissioner (Central)].", verdana)) { Border = 1, BorderWidthTop = 0, BorderWidthBottom = 1, BorderWidthRight = 1, Colspan = 6, Padding = 6 });
        //        ptableLabourAnnexure.AddCell(new PdfPCell(new Phrase((sno++).ToString(), verdana)) { Border = 1, BorderWidthTop = 0, BorderWidthBottom = 1, BorderWidthLeft = 1, BorderWidthRight = 1 });
        //        ptableLabourAnnexure.AddCell(new PdfPCell(new Phrase("The license shall notify any change in the number of workmen or the condistions of work to the Licensing Officer.", verdana)) { Border = 1, BorderWidthTop = 0, BorderWidthBottom = 1, BorderWidthRight = 1, Colspan = 6, Padding = 6 });
        //        ptableLabourAnnexure.AddCell(new PdfPCell(new Phrase((sno++).ToString(), verdana)) { Border = 1, BorderWidthTop = 0, BorderWidthBottom = 1, BorderWidthLeft = 1, BorderWidthRight = 1 });
        //        ptableLabourAnnexure.AddCell(new PdfPCell(new Phrase("[A copy of the licence shall be displayed prominently at the premises where the contract work is being carried on ]", verdana)) { Border = 1, BorderWidthTop = 0, BorderWidthBottom = 1, BorderWidthRight = 1, Colspan = 6, Padding = 6 });
        //        documents.Add(ptableLabourAnnexure);
        //        documents.Close();
        //        PDFPageEvents pageeventsobj = new PDFPageEvents();
        //        //if (IsDownload)
        //        //{
        //        //    pageeventsobj.DownloadPDF(sfilepath);
        //        //    //Response.BufferOutput = true;
        //        //}
        //        ptablerightSideDetail.AddCell(new PdfPCell(para) { Border = 0, VerticalAlignment = PdfPCell.ALIGN_JUSTIFIED, HorizontalAlignment = PdfPCell.ALIGN_JUSTIFIED, FixedHeight = 400 });
        //    }
        //    catch (Exception ex)
        //    {
        //        genericFormModel.HasException = true;
        //        genericFormModel.Exceptions = ex.Message;
        //    }
        //    return genericFormModel;
        //}
        private async Task<CertificateGenerateServiceResultTemplate> Certificate_AppForm_35(Int64 appRefId, string CertificatePerformaURL, ApplicationTypeEnum applicationType, string userName, string licenceNo, bool isDeemedCalling)
        {
            CertificateGenerateServiceResultTemplate genericFormModel = new CertificateGenerateServiceResultTemplate();
            try
            {
                Guid guId = Guid.NewGuid();
                genericFormModel.PdfNameGUID = guId.ToString();
                var sfilepath = "";
                sfilepath = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "TempFiles")).Root + guId.ToString() + ".pdf";
                FileInfo file = new FileInfo(sfilepath);
                if (file.Exists)
                {
                    file.Delete();
                }
                iTextSharp.text.Document documents = new iTextSharp.text.Document(iTextSharp.text.PageSize.A4, 5, 5, 30, 30);
                documents.Open();
                #region ClassObjects
                pdfFonts classFonts = new pdfFonts();
                #endregion

                #region CreateObjects
                PdfWriter owriter = PdfWriter.GetInstance(documents, new FileStream(sfilepath, FileMode.Create));
                PDFPageEvents pageEvent = new PDFPageEvents();
                owriter.PageEvent = pageEvent;
                owriter.SetEncryption(PdfWriter.STRENGTH128BITS, "", "", PdfWriter.AllowPrinting);
                #endregion

                var fontName = "Verdana";
                if (!FontFactory.IsRegistered(fontName))
                {
                    var fontPath = "Content/fonts/VERDANA.TTF";
                    FontFactory.Register(fontPath);
                }



                #region Tables
                var bocwLicence = _context.Licence_BocwAct_GeneralDetails.Where(x => x.AppRefId == appRefId).FirstOrDefault();
                var bocwContractorDetails = _context.Licence_BocwAct_ContractorDetails.Where(x => x.BocwEstablishmentRegistrationRefId == bocwLicence.BocwEstablishmentRegistrationId).FirstOrDefault();
                var application = _context.Applications.Where(x => x.AppId == appRefId && x.IsDeleted == false).FirstOrDefault();
                var projectSite = _context.ProjectSites.Where(x => x.ProjectSiteId == application.ProjectSiteRefId).FirstOrDefault();
                var factoryCircle = _context.FactoryCircles.Where(x => x.FactoryCircleId == bocwLicence.FactoryCircleRefId).FirstOrDefault();
                var alccircle = _context.ALCCircles.Where(x => x.ALCCircleId == bocwLicence.AlcCircleRefId).FirstOrDefault();
                var applicationAction = _context.ApplicationActions.Where(x => x.ApplicationRefId == application.AppId).FirstOrDefault();
                var feeDetails = _context.AppFeeDetails.Where(x => x.AppRefId == appRefId).FirstOrDefault();
                #endregion

                #region Licence Validity Year
                int licenceValidUpto = 0;
                DofLicenceDetailsViewModel dofDetails = new DofLicenceDetailsViewModel();
                if (application.ApplicationPurposeType == ApplicationPurposeTypeEnum.GRANT_LICENCE)
                {
                    DateTime registrationDate = DateTime.MinValue;
                    licenceValidUpto = 1;
                }
                #endregion

                string fontpath1 = "Content/fonts/VERDANA.TTF";
                BaseFont customfont = BaseFont.CreateFont(fontpath1, BaseFont.CP1252, BaseFont.EMBEDDED);
                string fontpath2 = "Content/fonts/VERDANA.TTF";
                BaseFont customfontimpact = BaseFont.CreateFont(fontpath2, BaseFont.CP1252, BaseFont.EMBEDDED);
                documents.Open();
                Font verdanaHeader = FontFactory.GetFont("Arial", 9, Font.BOLD);
                Font footer = FontFactory.GetFont("Arial", 9, Font.BOLD, BaseColor.BLUE);
                Font verdana = FontFactory.GetFont("Arial", 8, BaseColor.BLACK);
                Font verdanaBoldItalic = FontFactory.GetFont("Arial", 8, Font.BOLDITALIC);
                Font verdanasmall = FontFactory.GetFont("Arial", 8, Font.NORMAL);
                verdanasmall.Color = BaseColor.WHITE;
                Font verdanaRightHeader = FontFactory.GetFont("Arial", 15, Font.BOLD);
                verdanaRightHeader.Color = BaseColor.WHITE;
                Font verdanaRightHeader1 = FontFactory.GetFont("Arial", 12, Font.BOLD);
                verdanaRightHeader1.Color = BaseColor.WHITE;
                Font arialOldLicenceDetails = FontFactory.GetFont("Arial", 10, Font.BOLD);
                arialOldLicenceDetails.Color = BaseColor.WHITE;
                Font monotypeHeader = new Font(customfont, 20);
                Font monotype = FontFactory.GetFont("Arial", 13, Font.BOLD);
                Font impact = new Font(customfontimpact, 12);
                Font impactHeader = new Font(customfontimpact, 11, Font.BOLD);
                impactHeader.Color = BaseColor.WHITE;
                BaseFont bfTimes = BaseFont.CreateFont(BaseFont.TIMES_ROMAN, BaseFont.CP1252, false);
                Font timesRoman = FontFactory.GetFont("Arial", 9, BaseColor.BLACK);
                Font ArialFont = FontFactory.GetFont("Arial", 8, BaseColor.WHITE);
                Font SealFontArial = FontFactory.GetFont("Arial", 9, BaseColor.BLACK);
                Font verdanaBold = FontFactory.GetFont("Arial", 10, Font.BOLD);
                Font verdanalic = FontFactory.GetFont("Arial", 9, Font.BOLD);
                verdanalic.Color = BaseColor.WHITE;
                var factoryPerforma = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "LicensePerformas")).Root + "CertificatePerforma_Bocw.jpg";
                iTextSharp.text.Image pageLogo1 = iTextSharp.text.Image.GetInstance(new FileStream(factoryPerforma, FileMode.OpenOrCreate, FileAccess.Read));

                pageLogo1.Alignment = iTextSharp.text.Image.ALIGN_CENTER | iTextSharp.text.Image.UNDERLYING;
                pageLogo1.ScaleToFit(800f, 800f);
                pageLogo1.SetAbsolutePosition(10f, 25f);
                pageLogo1.BorderWidth = 1;
                documents.Add(pageLogo1);
                PdfPTable ptableleftSideDetail = new PdfPTable(1);
                ptableleftSideDetail.DefaultCell.Border = 0;
                ptableleftSideDetail.DefaultCell.Padding = 0;
                ptableleftSideDetail.HorizontalAlignment = (Element.ALIGN_JUSTIFIED);
                ptableleftSideDetail.DefaultCell.VerticalAlignment = (Element.ALIGN_JUSTIFIED);
                ptableleftSideDetail.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
                ptableleftSideDetail.AddCell(new PdfPCell(new Phrase("", verdanaBold)) { Border = 0, PaddingTop = 10 });


                ptableleftSideDetail.AddCell(new PdfPCell(new Phrase("Registration No. : " + licenceNo.ToString(), verdanalic)) { Border = 0, PaddingTop = 300, PaddingLeft = -18 }); ;
                ptableleftSideDetail.AddCell(new PdfPCell(new Phrase("Registration Issued On : " + String.Format("{0:dd-MMM-yyyy}", DateTime.Now.ToShortDateString()), verdanalic)) { Border = 0, PaddingTop = 10, PaddingLeft = -18 });
                Font verdanaBoldBP = new Font(BaseFont.CreateFont(BaseFont.TIMES_ROMAN, BaseFont.CP1252, false), 8, Font.BOLD | Font.UNDERLINE, BaseColor.WHITE);
                Phrase phrase = new Phrase();

                ptableleftSideDetail.AddCell(new PdfPCell(phrase) { Border = 0, PaddingTop = 10, PaddingRight = 20 });

                #region QR Code Generate
                var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(application.PublicAppRefNum);
                string base64String = System.Convert.ToBase64String(plainTextBytes);
                QRCodeGenerator qrGenerator = new QRCodeGenerator();
                QRCodeGenerator.QRCode qrCode = qrGenerator.CreateQrCode(base64String, QRCodeGenerator.ECCLevel.Q);

                using (System.Drawing.Bitmap bitMap = qrCode.GetGraphic(20))
                {
                    using (MemoryStream ms = new MemoryStream())
                    {
                        bitMap.Save(ms, ImageFormat.Jpeg);
                        System.Drawing.Image imgQRCode = System.Drawing.Image.FromStream(ms);
                        iTextSharp.text.Image imdf = iTextSharp.text.Image.GetInstance(imgQRCode, System.Drawing.Imaging.ImageFormat.Jpeg);
                        imdf.ScaleAbsoluteHeight(120f);
                        imdf.ScaleAbsoluteWidth(120f);
                        ptableleftSideDetail.AddCell(new PdfPCell(imdf) { Border = 0, PaddingTop = 30, PaddingLeft = 0, PaddingRight = 0 });
                    }
                }
                #endregion

                ptableleftSideDetail.AddCell(new PdfPCell(new Phrase("" + Chunk.NEWLINE + "Department of Labour, Govt. of Punjab" + Chunk.NEWLINE, verdanaRightHeader1)) { Border = 0, PaddingTop = 15, PaddingRight = 25, HorizontalAlignment = PdfPCell.ALIGN_CENTER });

                PdfPTable ptablerightSideDetail = new PdfPTable(1);
                ptablerightSideDetail.DefaultCell.Border = 0;
                ptablerightSideDetail.DefaultCell.Padding = 0;

                //ptablerightSideDetail.AddCell(new PdfPCell(new Phrase("", verdanaBold)) { Border = 0, Padding = 4 });
                ptablerightSideDetail.AddCell(new PdfPCell(new Phrase("FORM-XIII", verdanaHeader)) { Border = 0, PaddingLeft = 20, PaddingTop = 10, HorizontalAlignment = PdfPCell.ALIGN_CENTER });
                ptablerightSideDetail.AddCell(new PdfPCell(new Phrase("[See Rule 223(1)]", verdanaHeader)) { Border = 0, PaddingLeft = 20, HorizontalAlignment = PdfPCell.ALIGN_CENTER });
                ptablerightSideDetail.AddCell(new PdfPCell(new Phrase("REGISTRATION CERTIFICATE EMPLOYING WORKERS FOR BUILDING AND OTHER CONSTRUCTION WORK UNDER BOCW ACT 1996 ", impactHeader)) { Border = 0, PaddingTop = 12, PaddingBottom = 20, PaddingLeft = 40, PaddingRight = 30, VerticalAlignment = PdfPCell.ALIGN_MIDDLE, HorizontalAlignment = PdfPCell.ALIGN_CENTER });
                ptablerightSideDetail.AddCell(new PdfPCell(new Phrase("CERTIFICATE OF REGISTRATION", monotype)) { Border = 0, PaddingBottom = 4, PaddingTop = 3, HorizontalAlignment = PdfPCell.ALIGN_CENTER });
                ptablerightSideDetail.AddCell(new PdfPCell(new Phrase("GOVERNMENT OF PUNJAB", monotype)) { Border = 0, PaddingBottom = 4, HorizontalAlignment = PdfPCell.ALIGN_CENTER });
                ptablerightSideDetail.AddCell(new PdfPCell(new Phrase("", verdanaBold)) { Border = 0, Padding = 1, PaddingTop = 6 });
                ptablerightSideDetail.AddCell(new PdfPCell(new Phrase(" Dated: " + DateTime.Now.ToShortDateString() + "                            Registration No. : " + licenceNo.ToString(), verdanaHeader)) { Border = 0 });

                Paragraph para1 = new Paragraph();
                para1.Add(new Chunk("        A Certificate of Registration is hereby granted under sub-section (3) of section 7 of the Building and Other Construction Work(Regulation of Employment and Conditions of Services) Act, 1996 and the rules made there under, to M/s. ", timesRoman));
                para1.Add(new Chunk(projectSite.EstablishmentName, verdanaBold));

                para1.Add(new Chunk(" having the following particulars subject to the conditions laid down in Annexure:-", timesRoman));
                para1.Add(new Chunk(Chunk.NEWLINE + "", verdanaBoldItalic));
                para1.Add(new Chunk(Chunk.NEWLINE + "1. Postal address/location where building or other construction work is to be carried on by the employer:  ", timesRoman));
                para1.Add(new Chunk(projectSite.Address, verdanaBold));
                para1.Add(new Chunk(Chunk.NEWLINE + "2. Name and addresses of the principal employer including location of building and other construction work:  ", timesRoman));
                para1.Add(new Chunk(bocwLicence.PrincipalEmployerName + " ," + bocwLicence.PrincipalEmployerAddress, verdanaBold));
                para1.Add(new Chunk(Chunk.NEWLINE + "3. Name and permanent address of Manager:  ", timesRoman));
                para1.Add(new Chunk(bocwLicence.ManagerName + " ," + bocwLicence.ManagerAddress, verdanaBold));

                //para1.Add(new Chunk(Chunk.NEWLINE + "4. Nature of work in which building workers are employed or are to be employed:  ", timesRoman));
                //para1.Add(new Chunk(bocwLicence.MaximumNoOfWorkers.ToString(), verdanaBold));
                para1.Add(new Chunk(Chunk.NEWLINE + "4. Maximum  number of building workers to be employed on any day by the employer:  ", timesRoman));
                para1.Add(new Chunk(bocwLicence.MaximumNoOfWorkers.ToString(), verdanaBold));
                para1.Add(new Chunk(Chunk.NEWLINE + "5.Probable date of commencement of work:  ", timesRoman));
                para1.Add(new Chunk(bocwLicence.Work_CommencementDate.ToShortDateString(), verdanaBold));
                para1.Add(new Chunk(Chunk.NEWLINE + "6.Probable date of completion of work:  ", timesRoman));
                para1.Add(new Chunk(bocwLicence.Work_CompletionDate.ToShortDateString(), verdanaBold));
                para1.Add(new Chunk(Chunk.NEWLINE + "7.Other particulars relevant to the employment of building workers: ", timesRoman));
                para1.Add(new Chunk("NA", verdanaBold));
                PdfPCell shipment1 = new PdfPCell(para1);
                shipment1.SetLeading(1f, 2f);
                ptablerightSideDetail.AddCell(new PdfPCell(shipment1) { Border = 0, HorizontalAlignment = PdfPCell.ALIGN_JUSTIFIED, VerticalAlignment = PdfPCell.ALIGN_JUSTIFIED, FixedHeight = 430 });



                var userDetails = await _iAuthService.GetUserFullDetailByUserName(userName);
                string officerSigFilePath = "";
                if (!isDeemedCalling)
                {

                    if (userDetails.ListData.Count() == 0 || userDetails.ListData[0].Signature == null)
                    {
                        userDetails.ListData[0].Signature = "abc.jpg";
                    }
                    officerSigFilePath = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "OfficerSignatures")).Root + userDetails.ListData[0].Signature;
                    iTextSharp.text.Image signature = iTextSharp.text.Image.GetInstance(new FileStream(officerSigFilePath, FileMode.OpenOrCreate, FileAccess.Read));
                    signature.ScaleToFit(120f, 90f);
                    signature.SetAbsolutePosition(600f, 120f);
                    ptablerightSideDetail.AddCell(new PdfPCell(signature) { Border = 0, HorizontalAlignment = PdfPCell.ALIGN_RIGHT });
                }

                Paragraph SignSeal = new Paragraph();
                //SignSeal.Add(new Chunk("Name: ", verdanaHeader));
                //SignSeal.Add(new Chunk(userDetails.ListData[0].UserFullName, SealFontArial));
                //SignSeal.Add(new Chunk(Chunk.NEWLINE + "Desig: ", verdanaHeader));
                SignSeal.Add(new Chunk(Chunk.NEWLINE + "Circle Name: ", verdanaHeader));
                SignSeal.Add(new Chunk(bocwLicence.BOCWActCircleType == BOCWActCircleTypeEnum.FACTORY_WING ? factoryCircle.FactoryCircleName : alccircle.ALCCircleName, SealFontArial));
                SignSeal.Add(new Chunk(Chunk.NEWLINE + "District Name: ", verdanaHeader));
                SignSeal.Add(new Chunk(bocwLicence.BOCWActCircleType == BOCWActCircleTypeEnum.FACTORY_WING ? factoryCircle.FactoryCircleName : alccircle.ALCCircleName, SealFontArial));
                SignSeal.Add(new Chunk(Chunk.NEWLINE + "Date: ", verdanaHeader));
                SignSeal.Add(new Chunk(DateTime.Now.ToShortDateString(), SealFontArial));
                SignSeal.Add(new Chunk(Chunk.NEWLINE + "Time: ", verdanaHeader));
                SignSeal.Add(new Chunk(DateTime.Now.ToShortTimeString(), SealFontArial));

                ptablerightSideDetail.AddCell(new PdfPCell(SignSeal) { Border = 0, HorizontalAlignment = PdfPCell.ALIGN_LEFT, PaddingLeft = 200 });
                //ptablerightSideDetail.AddCell(new PdfPCell(new Phrase(" Dated " + DateTime.Now.ToShortDateString(), verdanaHeader)) { Border = 0, HorizontalAlignment = PdfPCell.ALIGN_LEFT});

                ptablerightSideDetail.AddCell(new PdfPCell(new Phrase("This is a computer generated document to check & verify please login to https://pblabour.gov.in/Account/ApplicationStatus", footer)) { Border = 1, BorderWidth = 1, Padding = 8 });
                PdfPTable ptableShopDetailLicenceApplied = new PdfPTable(3);
                ptableShopDetailLicenceApplied.DefaultCell.Border = 0;
                ptableShopDetailLicenceApplied.DefaultCell.Padding = 2;
                ptableShopDetailLicenceApplied.SpacingBefore = 1000;
                ptableShopDetailLicenceApplied.AddCell(new PdfPCell(ptableleftSideDetail) { Border = 0 });
                ptableShopDetailLicenceApplied.AddCell(new PdfPCell(ptablerightSideDetail) { Border = 0, Colspan = 2 });
                documents.Add(ptableShopDetailLicenceApplied);
                documents.Close();
                PDFPageEvents pageeventsobj = new PDFPageEvents();

                //ptablerightSideDetail.AddCell(new PdfPCell(para) { Border = 0, VerticalAlignment = PdfPCell.ALIGN_JUSTIFIED, HorizontalAlignment = PdfPCell.ALIGN_JUSTIFIED, FixedHeight = 400 });
            }
            catch (Exception ex)
            {
                genericFormModel.HasException = true;
                genericFormModel.Exceptions = ex.Message;
            }
            return genericFormModel;
        }
        private async Task<CertificateGenerateServiceResultTemplate> Certificate_AppForm_62(Int64 appRefId, string CertificatePerformaURL, ApplicationTypeEnum applicationType, string userName, string licenceNo, bool isDeemedCalling)
        {
            CertificateGenerateServiceResultTemplate genericFormModel = new CertificateGenerateServiceResultTemplate();
            try
            {
                Guid guId = Guid.NewGuid();
                genericFormModel.PdfNameGUID = guId.ToString();
                genericFormModel.HasException = false;
                var SaveDirectoryPath = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "TempFiles")).Root + guId.ToString() + ".pdf";
                iTextSharp.text.Document documents = new iTextSharp.text.Document(iTextSharp.text.PageSize.A4, 5, 5, 30, 30);

                #region ClassObjects
                pdfFonts classFonts = new pdfFonts();
                #endregion

                #region CreateObjects
                PdfWriter owriter = PdfWriter.GetInstance(documents, new FileStream(SaveDirectoryPath, FileMode.Create));
                PDFPageEvents pageEvent = new PDFPageEvents();
                owriter.PageEvent = pageEvent;
                owriter.SetEncryption(PdfWriter.STRENGTH128BITS, "", "", PdfWriter.AllowPrinting);
                #endregion

                #region Regester Font Style
                var fontName = "Alice";
                Font font = FontFactory.GetFont(fontName, BaseFont.IDENTITY_H, 16);
                Font fontSmall = FontFactory.GetFont(fontName, BaseFont.IDENTITY_H, 12);
                Font fontBOLD = FontFactory.GetFont(fontName, BaseFont.IDENTITY_H, 16, iTextSharp.text.Font.BOLD);
                #endregion

                var formData = _context.Licence_Factory_NightShift_Approvals.Where(x => x.AppRefId == appRefId).FirstOrDefault();
                var licenceMappingInfo = _context.ApplicationLicenceNoMapping.Where(x => x.LicenceNumber == formData.LicenceNumber && x.Application.ApplicationType == ApplicationTypeEnum.FACTORY_LICENCE).Include(x => x.Application).FirstOrDefault();

                GenericResponseTemplateModel<FactoryLicenceDetailsForNightShiftPdfViewModel> factoryFormModelDetails = new GenericResponseTemplateModel<FactoryLicenceDetailsForNightShiftPdfViewModel>();
                if (licenceMappingInfo == null)
                {
                    factoryFormModelDetails = await _iApplicationManagementService.getFactoryLicenceLegacyDetail(formData.LicenceNumber);
                }
                else
                {
                    var factoryLicenceDetails = _context.Licence_Factory_GeneralDetails.Where(x => x.AppRefId == licenceMappingInfo.AppRefId).Include(x => x.Licence_Factory_OccupierAndManagerDetail).FirstOrDefault();
                    var application = _context.Applications.Where(x => x.AppId == licenceMappingInfo.AppRefId && x.IsDeleted == false).FirstOrDefault();
                    var projectSite = _context.ProjectSiteLogs.Where(x => x.ProjectSiteRefId == application.ProjectSiteRefId && x.ProjectSiteVersion == application.ProjectSiteVersion).FirstOrDefault();

                    factoryFormModelDetails.ResponseDataModel = new FactoryLicenceDetailsForNightShiftPdfViewModel();
                    factoryFormModelDetails.ResponseDataModel.RegNo = licenceNo;
                    factoryFormModelDetails.ResponseDataModel.IssueDate = DateTime.Now;
                    factoryFormModelDetails.ResponseDataModel.FileNo = factoryLicenceDetails.Application.PublicAppRefNum;
                    factoryFormModelDetails.ResponseDataModel.EName = factoryLicenceDetails.Licence_Factory_OccupierAndManagerDetail.OccupierFullName;
                    factoryFormModelDetails.ResponseDataModel.EFHName = factoryLicenceDetails.Licence_Factory_OccupierAndManagerDetail.OccupierFatherName;
                    factoryFormModelDetails.ResponseDataModel.NameOftheEstablishment = projectSite.EstablishmentName;
                    factoryFormModelDetails.ResponseDataModel.ManagerName = factoryLicenceDetails.Licence_Factory_OccupierAndManagerDetail.ManagerFullName;
                    factoryFormModelDetails.ResponseDataModel.PostalAddressEstablishment = projectSite.Address;
                    factoryFormModelDetails.ResponseDataModel.BusinessNature = factoryLicenceDetails.ManufacturingProcess_Next12Months;
                    factoryFormModelDetails.ResponseDataModel.ProjectPurpose = projectSite.ProjectPurpose;
                    factoryFormModelDetails.ResponseDataModel.TotalEmployees = factoryLicenceDetails.Workers_MaxDuringYear;
                    factoryFormModelDetails.ResponseDataModel.DateOfPreviousRegistration = factoryLicenceDetails.Application.LastModifiedOnDate;
                    factoryFormModelDetails.ResponseDataModel.PermissionGrantedForWomenWorkingNightShift = DateTime.Now;
                    factoryFormModelDetails.ResponseDataModel.ValidPermissionGrantedForWomenWorkingNightShift = DateTime.Now;
                }

                documents.Open();
                Font verdanaHeader = FontFactory.GetFont("Arial", 9, Font.BOLD);
                Font verdana = FontFactory.GetFont("Arial", 9, BaseColor.BLACK);
                Font verdanaBoldItalic = FontFactory.GetFont("Arial", 8, Font.BOLDITALIC);
                Font verdanasmall = FontFactory.GetFont("Arial", 8, Font.NORMAL);
                verdanasmall.Color = BaseColor.WHITE;
                Font verdanaRightHeader = FontFactory.GetFont("Arial", 15, Font.BOLD);
                verdanaRightHeader.Color = BaseColor.WHITE;
                Font verdanaRightHeader1 = FontFactory.GetFont("Arial", 12, Font.BOLD);
                verdanaRightHeader1.Color = BaseColor.WHITE;
                Font verdanaRightHeader2 = FontFactory.GetFont("Arial", 9, Font.BOLD);
                verdanaRightHeader2.Color = BaseColor.WHITE;
                Font monotype = FontFactory.GetFont("Arial", 13, Font.BOLD);
                Font monotypeHeader = FontFactory.GetFont("Arial", 15, Font.BOLD);
                Font impact = new Font(fontSmall.BaseFont, 12);
                Font impactHeader = new Font(fontSmall.BaseFont, 12);
                impactHeader.Color = BaseColor.WHITE;
                BaseFont bfTimes = BaseFont.CreateFont(BaseFont.TIMES_ROMAN, BaseFont.CP1252, false);
                Font timesRoman = FontFactory.GetFont("Arial", 10, BaseColor.BLACK);
                Font verdanaBold = FontFactory.GetFont("Arial", 9, Font.BOLD);
                iTextSharp.text.Image pageLogo1;

                var factoryPerforma = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "LicensePerformas")).Root + "CertificatePerforma_Factory.jpg";
                pageLogo1 = iTextSharp.text.Image.GetInstance(new FileStream(factoryPerforma, FileMode.OpenOrCreate, FileAccess.Read));
                pageLogo1.Alignment = iTextSharp.text.Image.ALIGN_CENTER | iTextSharp.text.Image.UNDERLYING;
                pageLogo1.ScaleToFit(800f, 800f);
                pageLogo1.SetAbsolutePosition(10f, 25f);
                pageLogo1.BorderWidth = 1;
                documents.Add(pageLogo1);
                PdfPTable ptableleftSideDetail = new PdfPTable(1);
                ptableleftSideDetail.DefaultCell.Border = 0;
                ptableleftSideDetail.DefaultCell.Padding = 0;
                ptableleftSideDetail.HorizontalAlignment = (Element.ALIGN_JUSTIFIED);
                ptableleftSideDetail.DefaultCell.VerticalAlignment = (Element.ALIGN_JUSTIFIED);
                ptableleftSideDetail.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
                ptableleftSideDetail.AddCell(new PdfPCell(new Phrase("", verdanaHeader)) { Border = 0, PaddingTop = 320 });
                ptableleftSideDetail.AddCell(new PdfPCell(new Phrase("DESCRIPTION OF THE LICENCED  PREMISES ", verdanaRightHeader1)) { Border = 0, PaddingTop = 8, PaddingLeft = -15 });
                ptableleftSideDetail.AddCell(new PdfPCell(new Phrase("Licence No : ", verdanaRightHeader1)) { Border = 0, PaddingTop = 8, PaddingLeft = -15 });
                ptableleftSideDetail.AddCell(new PdfPCell(new Phrase("Department of Labour", verdanaRightHeader1)) { Border = 0, PaddingTop = 195, PaddingLeft = -15 });
                ptableleftSideDetail.AddCell(new PdfPCell(new Phrase("      Govt. of Punjab ", verdanaRightHeader1)) { Border = 0, PaddingTop = 0, PaddingLeft = -15 });
                PdfPTable ptableleftSideDetailFormF = new PdfPTable(1);
                ptableleftSideDetailFormF.DefaultCell.Border = 0;
                ptableleftSideDetailFormF.DefaultCell.Padding = 0;
                ptableleftSideDetailFormF.HorizontalAlignment = (Element.ALIGN_JUSTIFIED);
                ptableleftSideDetailFormF.DefaultCell.VerticalAlignment = (Element.ALIGN_JUSTIFIED);
                ptableleftSideDetailFormF.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
                PdfPTable tblCurrLicDetail = new PdfPTable(1);
                Font verdanaBoldHeader = FontFactory.GetFont("Arial", 9, Font.BOLD);
                verdanaBoldHeader.Color = BaseColor.WHITE;
                tblCurrLicDetail.AddCell(new PdfPCell(new Phrase("", verdanaBoldHeader)) { Border = 0 });

                tblCurrLicDetail.AddCell(new PdfPCell(new Phrase("Reg. No : " + licenceNo, verdanaBoldHeader)) { Border = 0, PaddingTop = 8, PaddingLeft = 8 });
                tblCurrLicDetail.AddCell(new PdfPCell(new Phrase("Issued Date : " + DateTime.Now.Day.ToString() + "-" + DateTime.Now.Date.ToString("MMM") + "-" + DateTime.Now.Year.ToString(), verdanaBoldHeader)) { Border = 0, PaddingTop = 8, PaddingLeft = 8 });
                tblCurrLicDetail.AddCell(new PdfPCell(new Phrase("File No: " + factoryFormModelDetails.ResponseDataModel.FileNo, verdanaBoldHeader)) { Border = 0, PaddingTop = 8, PaddingBottom = 8, PaddingLeft = 8 });

                ptableleftSideDetailFormF.AddCell(new PdfPCell(tblCurrLicDetail) { Border = 0, FixedHeight = 100 });

                #region QR Code Generate
                var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(factoryFormModelDetails.ResponseDataModel.FileNo);
                string base64String = System.Convert.ToBase64String(plainTextBytes);
                QRCodeGenerator qrGenerator = new QRCodeGenerator();
                QRCodeGenerator.QRCode qrCode = qrGenerator.CreateQrCode(base64String, QRCodeGenerator.ECCLevel.Q);

                using (System.Drawing.Bitmap bitMap = qrCode.GetGraphic(20))
                {
                    using (MemoryStream ms = new MemoryStream())
                    {
                        bitMap.Save(ms, ImageFormat.Jpeg);
                        System.Drawing.Image imgQRCode = System.Drawing.Image.FromStream(ms);
                        iTextSharp.text.Image imdf = iTextSharp.text.Image.GetInstance(imgQRCode, System.Drawing.Imaging.ImageFormat.Jpeg);
                        imdf.ScaleAbsoluteHeight(150f);
                        imdf.ScaleAbsoluteWidth(150f);
                        ptableleftSideDetailFormF.AddCell(new PdfPCell(imdf) { Border = 0, PaddingTop = 10, PaddingLeft = 12 });
                    }
                }

                #endregion

                ptableleftSideDetailFormF.AddCell(new PdfPCell(new Phrase("Department of Labour", verdanaRightHeader1)) { Border = 0, PaddingTop = 120, PaddingLeft = 10 });
                ptableleftSideDetailFormF.AddCell(new PdfPCell(new Phrase("Govt. of Punjab ", verdanaRightHeader1)) { Border = 0, PaddingTop = 0, PaddingLeft = 25 });

                #region Employer Signature

                var empSig = _context.ApplicationDocuments.Where(x => x.AppRefId == appRefId && x.DocumentRefId == 20005).OrderByDescending(x => x.AppDocId).FirstOrDefault();
                string empSignatureFileName = "abc.jpg";
                //empSig != null ? empSig.AttachmentName : factoryFormModelDetails.ResponseDataModel.ApplicantSign;
                if (empSig != null)
                {
                    empSignatureFileName = empSig.AttachmentName;
                }
                var employerSigFilePath = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "AppFiles")).Root + empSignatureFileName;

                iTextSharp.text.Image employerSignature = iTextSharp.text.Image.GetInstance(new FileStream(employerSigFilePath, FileMode.OpenOrCreate, FileAccess.Read));
                employerSignature.ScaleToFit(120f, 120f);
                employerSignature.SetAbsolutePosition(630f, 150f);
                documents.Add(employerSignature);

                #endregion

                #region Factory Form F Reports Detail

                PdfPTable ptableFactoryFormFDetail = new PdfPTable(4);
                ptableFactoryFormFDetail.DefaultCell.Border = 1;
                ptableFactoryFormFDetail.DefaultCell.Padding = 2;
                PdfPCell pFormFDetailcellLine1 = new PdfPCell(new Paragraph(""));
                pFormFDetailcellLine1.BorderWidth = 0;
                pFormFDetailcellLine1.Colspan = 4;
                ptableFactoryFormFDetail.AddCell(pFormFDetailcellLine1);
                ptableFactoryFormFDetail.AddCell(new PdfPCell(new Phrase("PERMISSION FOR WORKING WOMEN IN NIGHT SHIFT UNDER  FACTORY\n( ACT. 1948)", impactHeader)) { Border = 0, PaddingTop = 100, PaddingBottom = 50, PaddingLeft = 27, PaddingRight = 25, VerticalAlignment = PdfPCell.ALIGN_MIDDLE, HorizontalAlignment = PdfPCell.ALIGN_CENTER, Colspan = 6 });
                ptableFactoryFormFDetail.AddCell(new PdfPCell(new Phrase("1. Name and Father's/Husband's Name of Employer:", verdanaHeader)) { Border = 0, PaddingTop = 80, Colspan = 2, Padding = 6 });
                ptableFactoryFormFDetail.AddCell(new PdfPCell(new Phrase(factoryFormModelDetails.ResponseDataModel.EName + " \nS/o : " + factoryFormModelDetails.ResponseDataModel.EFHName, verdana)) { Border = 0, Colspan = 2, Padding = 6 });

                ptableFactoryFormFDetail.AddCell(new PdfPCell(new Phrase("2.  Name of the Establishment :", verdanaHeader)) { Border = 0, Colspan = 2, Padding = 6 });

                ptableFactoryFormFDetail.AddCell(new PdfPCell(new Phrase(factoryFormModelDetails.ResponseDataModel.NameOftheEstablishment, verdana)) { Border = 0, Padding = 6, Colspan = 2 });


                ptableFactoryFormFDetail.AddCell(new PdfPCell(new Phrase("3. Name of Manager : ", verdanaHeader)) { Border = 0, Colspan = 2, Padding = 6 });
                ptableFactoryFormFDetail.AddCell(new PdfPCell(new Phrase(factoryFormModelDetails.ResponseDataModel.ManagerName, verdana)) { Border = 0, Colspan = 2, Padding = 6 });

                ptableFactoryFormFDetail.AddCell(new PdfPCell(new Phrase("4. Full Postal Address of the Establishment :", verdanaHeader)) { Border = 0, Colspan = 2, Padding = 6 });
                ptableFactoryFormFDetail.AddCell(new PdfPCell(new Phrase(factoryFormModelDetails.ResponseDataModel.PostalAddressEstablishment, verdana)) { Border = 0, Colspan = 2, Padding = 6 });
                ptableFactoryFormFDetail.AddCell(new PdfPCell(new Phrase("5.1: Nature of Business ", verdanaHeader)) { Border = 0, Padding = 6, Colspan = 2 });
                ptableFactoryFormFDetail.AddCell(new PdfPCell(new Phrase(factoryFormModelDetails.ResponseDataModel.BusinessNature, verdana)) { Border = 0, Colspan = 2, Padding = 6 });

                ptableFactoryFormFDetail.AddCell(new PdfPCell(new Phrase("5.2: Project Purpose :", verdanaHeader)) { Border = 0, Padding = 6, Colspan = 2 });
                ptableFactoryFormFDetail.AddCell(new PdfPCell(new Phrase(factoryFormModelDetails.ResponseDataModel.ProjectPurpose, verdana)) { Border = 0, Padding = 6, Colspan = 2 });

                ptableFactoryFormFDetail.AddCell(new PdfPCell(new Phrase("6. No. of Employees if any:", verdanaHeader)) { Border = 0, Colspan = 2, Padding = 6 });
                ptableFactoryFormFDetail.AddCell(new PdfPCell(new Phrase(factoryFormModelDetails.ResponseDataModel.TotalEmployees.ToString(), verdana)) { Border = 0, Colspan = 2, Padding = 6 });

                ptableFactoryFormFDetail.AddCell(new PdfPCell(new Phrase("7. No and Date of Previous Registration Certificate Under Factory Act:", verdanaHeader)) { Border = 0, Colspan = 2, Padding = 6 });
                ptableFactoryFormFDetail.AddCell(new PdfPCell(new Phrase(licenceNo + ", " + factoryFormModelDetails.ResponseDataModel.DateOfPreviousRegistration.ToString(), verdana)) { Border = 0, Colspan = 2, Padding = 6 });

                ptableFactoryFormFDetail.AddCell(new PdfPCell(new Phrase("8. Date of Permission Granted for Night Working Women in The Night Shift. :", verdanaHeader)) { Border = 0, Colspan = 2, Padding = 6 });
                ptableFactoryFormFDetail.AddCell(new PdfPCell(new Phrase(DateTime.Now.ToString(), verdana)) { Border = 0, Colspan = 2, Padding = 6 });

                ptableFactoryFormFDetail.AddCell(new PdfPCell(new Phrase("9. Granted for Night Working Women in The Night Shift Act Permission Valid Till :", verdanaHeader)) { Border = 0, Colspan = 2, Padding = 6 });
                ptableFactoryFormFDetail.AddCell(new PdfPCell(new Phrase("31-Dec-" + DateTime.Now.Year.ToString(), verdana)) { Border = 0, Colspan = 2, Padding = 6 });

                ptableFactoryFormFDetail.AddCell(new PdfPCell(new Phrase("The Establishment mentioned above is hereby registerd under Reg. No.  " + licenceNo, verdanaHeader)) { Border = 0, PaddingTop = 40, HorizontalAlignment = PdfPCell.ALIGN_JUSTIFIED, Colspan = 15 });

                #endregion

                #region Footer
                PdfPTable ptablefooterLine = new PdfPTable(3);
                ptablefooterLine.DefaultCell.Border = 0;
                ptablefooterLine.DefaultCell.Padding = 8;
                ptablefooterLine.SpacingAfter = 10;
                PdfPCell pcellLinefooter = new PdfPCell(new Paragraph(""));
                pcellLinefooter.BorderWidthBottom = 1;
                pcellLinefooter.BorderWidthTop = 0;
                pcellLinefooter.BorderWidthRight = 0;
                pcellLinefooter.BorderWidthLeft = 0;
                pcellLinefooter.Colspan = 3;
                ptablefooterLine.AddCell(pcellLinefooter);

                /* Paragraph AppHeadingFooter = new Paragraph("This is system generated document Hence no signature required.", verdana);

                 AppHeadingFooter.Alignment = Element.ALIGN_CENTER;*/
                #endregion

                PdfPTable ptableFactoryDetailLicenceAppliedFormF = new PdfPTable(3);
                ptableFactoryDetailLicenceAppliedFormF.DefaultCell.Border = 0;
                ptableFactoryDetailLicenceAppliedFormF.DefaultCell.Padding = 2;
                ptableFactoryDetailLicenceAppliedFormF.SpacingBefore = 1000;
                ptableFactoryDetailLicenceAppliedFormF.AddCell(new PdfPCell(ptableleftSideDetailFormF) { Border = 0, PaddingTop = 300, PaddingLeft = -25, PaddingRight = 40 });
                ptableFactoryDetailLicenceAppliedFormF.AddCell(new PdfPCell(ptableFactoryFormFDetail) { Border = 0, Colspan = 3 });
                documents.Add(ptableFactoryDetailLicenceAppliedFormF);
                documents.NewPage();
                PdfPTable ptableFactoryAnnexure = new PdfPTable(7);
                ptableFactoryAnnexure.DefaultCell.Border = 0;
                ptableFactoryAnnexure.DefaultCell.Padding = 2;
                int sno = 1;
                ptableFactoryAnnexure.AddCell(new PdfPCell(new Phrase("The conditions for permission of working women on the night shift:", verdanaHeader)) { Padding = 20, Colspan = 7, Border = 0, Left = 40 });

                var selectedIds = JsonConvert.DeserializeObject<List<Int64>>(formData.CheckListJson);
                var checklistPoints = _context.Licence_Factory_NightShift_ChecklistPoints.Where(x => x.IsEnabled == true && selectedIds.Contains(x.ChecklistID)).ToList();
                foreach (var item in checklistPoints)
                {
                    //ptableFactoryAnnexure.AddCell();
                    ptableFactoryAnnexure.AddCell(new PdfPCell(new Phrase(item.ChecklistDescription, verdana)) { Border = 1, BorderWidthTop = 1, BorderWidthBottom = 1, BorderWidthRight = 1, BorderWidthLeft = 1, Colspan = 14, Padding = 6 });

                }
                ptableFactoryAnnexure.AddCell(new PdfPCell(new Phrase("Note: The employer will abide by the undertaking given by him/her regarding all the above conditions mentioned in the permission granted for working women in the night shift " + factoryFormModelDetails.ResponseDataModel.NameOftheEstablishment + ". If any of the conditions are violated or not followed as per the permission given, then legal action will be initiated against the employer.", verdanaHeader)) { Border = 0, PaddingTop = 40, HorizontalAlignment = PdfPCell.ALIGN_JUSTIFIED, Colspan = 15 });

                ptableFactoryAnnexure.AddCell(new PdfPCell(new Phrase(" ", verdanaHeader)) { Border = 0, Colspan = 5, PaddingTop = 10, HorizontalAlignment = PdfPCell.ALIGN_LEFT });
                ptableFactoryAnnexure.AddCell(new PdfPCell(employerSignature) { Border = 0, Colspan = 5, PaddingTop = 10, HorizontalAlignment = PdfPCell.ALIGN_RIGHT });

                ptableFactoryAnnexure.AddCell(new PdfPCell(new Phrase("Labour Commissioner", verdanaHeader)) { Border = 0, Colspan = 5, HorizontalAlignment = PdfPCell.ALIGN_LEFT });
                ptableFactoryAnnexure.AddCell(new PdfPCell(new Phrase("Signature of Employer", verdanaHeader)) { Border = 0, Colspan = 5, HorizontalAlignment = PdfPCell.ALIGN_RIGHT });

                //ptableShopDetail.AddCell(new PdfPCell(new Phrase("", verdana)) { Border = 0, Colspan = 4 });
                ptableFactoryAnnexure.AddCell(new PdfPCell(new Phrase("Labour Bhawan Model Welfare Centre.", verdana)) { Border = 0, Colspan = 5, HorizontalAlignment = PdfPCell.ALIGN_LEFT });
                ptableFactoryAnnexure.AddCell(new PdfPCell(new Phrase(factoryFormModelDetails.ResponseDataModel.EName, verdana)) { Border = 0, Colspan = 3, HorizontalAlignment = PdfPCell.ALIGN_RIGHT });


                ptableFactoryAnnexure.AddCell(new PdfPCell(new Phrase("Phase 10. Sector 64, ", verdana)) { Border = 0, Colspan = 5, HorizontalAlignment = PdfPCell.ALIGN_LEFT });
                ptableFactoryAnnexure.AddCell(new PdfPCell(new Phrase(factoryFormModelDetails.ResponseDataModel.PostalAddressEstablishment, verdana)) { Border = 0, Colspan = 3, HorizontalAlignment = PdfPCell.ALIGN_RIGHT });

                ptableFactoryAnnexure.AddCell(new PdfPCell(new Phrase("Sahibzada Ajit Singh Nagar, Punjab. 160062", verdana)) { Border = 0, Colspan = 5, HorizontalAlignment = PdfPCell.ALIGN_LEFT });
                ptableFactoryAnnexure.AddCell(new PdfPCell(new Phrase("", verdana)) { Border = 0, Colspan = 3, HorizontalAlignment = PdfPCell.ALIGN_RIGHT });

                ptableFactoryAnnexure.AddCell(new PdfPCell(new Phrase("This is system generated document Hence no signature required.", verdanaHeader)) { Border = 0, PaddingTop = 40, HorizontalAlignment = PdfPCell.ALIGN_CENTER, Colspan = 15 });

                documents.Add(ptableFactoryAnnexure);
                documents.NewPage();
                PdfPTable ptableFactoryDetailLicenceAppliedFormB = new PdfPTable(3);
                ptableFactoryDetailLicenceAppliedFormB.DefaultCell.Border = 0;
                ptableFactoryDetailLicenceAppliedFormB.DefaultCell.Padding = 2;
                ptableFactoryDetailLicenceAppliedFormB.SpacingBefore = 1000;
                //ptableFactoryDetailLicenceAppliedFormB.AddCell(new PdfPCell(ptableFactoryDetail) { Border = 0, Colspan = 3 });
                documents.Add(ptableFactoryDetailLicenceAppliedFormB);
                documents.NewPage();
                documents.Close();
                PDFPageEvents pageeventsobj = new PDFPageEvents();
            }
            catch (Exception ex)
            {
                genericFormModel.HasException = true;
                genericFormModel.Exceptions = ex.Message;
            }
            return genericFormModel;
        }

        private async Task<CertificateGenerateServiceResultTemplate> Certificate_AppForm_71(Int64 appRefId, string CertificatePerformaURL, ApplicationTypeEnum applicationType, string userName, string licenceNo, bool isDeemedCalling)
        {
            CertificateGenerateServiceResultTemplate genericFormModel = new CertificateGenerateServiceResultTemplate();
            try
            {
                Guid guId = Guid.NewGuid();
                genericFormModel.PdfNameGUID = guId.ToString();
                genericFormModel.HasException = false;
                var SaveDirectoryPath = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "TempFiles")).Root + guId.ToString() + ".pdf";
                iTextSharp.text.Document documents = new iTextSharp.text.Document(iTextSharp.text.PageSize.A4, 5, 5, 30, 30);

                PdfWriter owriter = PdfWriter.GetInstance(documents, new FileStream(SaveDirectoryPath, FileMode.Create));
                PDFPageEvents pageEvent = new PDFPageEvents();
                owriter.PageEvent = pageEvent;
                owriter.SetEncryption(PdfWriter.STRENGTH128BITS, "", "", PdfWriter.AllowPrinting);

                string fontpath = "Content/fonts/Times-New-Roman.TTF";
                BaseFont baseFont = BaseFont.CreateFont(fontpath, BaseFont.CP1252, BaseFont.EMBEDDED);
                Font font = new Font(baseFont, 12);
                Font fontBold = new Font(baseFont, 12, Font.BOLD);
                Font smallFont = new Font(baseFont, 8);
                documents.Open();

                Paragraph paragraph1 = new Paragraph("Director of Factories, Punjab", fontBold)
                {
                    Alignment = Element.ALIGN_CENTER
                };
                documents.Add(paragraph1);

                Paragraph paragraph2 = new Paragraph("Kirat Bhawan(Model Welfare Centre) Near 10-11 Lights\nSec 64 (Phase 10),S.A.S Nagar (Mohali),Pin-160062", font)
                {
                    Alignment = Element.ALIGN_CENTER
                };
                documents.Add(paragraph2);
                documents.Add(new Paragraph("\n", font));

                var licenceDetails = _context.Applications.Where(x => x.AppId == appRefId && x.IsDeleted == false).Include(x => x.ProjectSites).FirstOrDefault();
                string dofNumber = "DOFPB" + licenceDetails.ProjectSites.ProjectSiteId.ToString().PadLeft(8, '0');
                var feeDetails = _context.Payments_RaisedFee.Where(x => x.AppRefId == appRefId).FirstOrDefault();

                if (licenceDetails?.ProjectSites != null)
                {
                    Paragraph establishmentParagraph = new Paragraph(licenceDetails.ProjectSites.EstablishmentName, font)
                    {
                        Alignment = Element.ALIGN_JUSTIFIED,
                        IndentationLeft = 10f,
                        IndentationRight = 10f
                    };
                    documents.Add(establishmentParagraph);

                    Paragraph addressParagraph = new Paragraph(licenceDetails.ProjectSites.Address, font)
                    {
                        Alignment = Element.ALIGN_JUSTIFIED,
                        IndentationLeft = 10f,
                        IndentationRight = 10f
                    };
                    documents.Add(addressParagraph);
                }

                if (dofNumber != null)
                {
                    Paragraph dofNoParagraph = new Paragraph(dofNumber, fontBold)
                    {
                        Alignment = Element.ALIGN_JUSTIFIED,
                        IndentationLeft = 10f,
                        IndentationRight = 10f
                    };
                    documents.Add(dofNoParagraph);

                    Paragraph dateParagraph = new Paragraph("Date :" + DateTime.Now, fontBold)
                    {
                        Alignment = Element.ALIGN_JUSTIFIED,
                        IndentationLeft = 10f,
                        IndentationRight = 10f
                    };
                    documents.Add(dateParagraph);

                    Paragraph fileNoParagraph = new Paragraph("File No :" + licenceDetails.PublicAppRefNum, fontBold)
                    {
                        Alignment = Element.ALIGN_JUSTIFIED,
                        IndentationLeft = 10f,
                        IndentationRight = 10f
                    };
                    documents.Add(fileNoParagraph);
                }

                Paragraph subject = new Paragraph("\nSubject: Acceptance of Proposed Factory Building Plans under section-6 Factories ACT 1948 and Rule 3-A of Punjab Factories Rules 1952.", fontBold)
                {
                    Alignment = Element.ALIGN_JUSTIFIED,
                    IndentationLeft = 10f,
                    IndentationRight = 10f
                };
                documents.Add(subject);

                // Construct the paragraph text with specific parts in bold
                string establishmentName = licenceDetails.ProjectSites.EstablishmentName;
                string address = licenceDetails.ProjectSites.Address;

                Chunk normalChunk1 = new Chunk("\nThe proposed building plans of ", font);
                Chunk boldChunk1 = new Chunk(establishmentName + ", ", fontBold); // Bold establishment name
                Chunk normalChunk2 = new Chunk(address + ", Punjab (India) ", fontBold);
                Chunk normalChunk3 = new Chunk("have been found correct in accordance with the provisions of (Rule 3-A) of the Punjab Factories Rule, 1952 & ", font);
                Chunk normalChunk4 = new Chunk("got accepted under Factories Act, 1948 by the Chief Inspector of Factories, Punjab, subject to the following conditions: -", font);

                Phrase paragraphPhrase = new Phrase();
                paragraphPhrase.Add(normalChunk1);
                paragraphPhrase.Add(boldChunk1);
                paragraphPhrase.Add(normalChunk2);
                paragraphPhrase.Add(normalChunk3);
                paragraphPhrase.Add(normalChunk4);

                Paragraph body = new Paragraph(paragraphPhrase)
                {
                    Alignment = Element.ALIGN_JUSTIFIED,
                    IndentationLeft = 10f,
                    IndentationRight = 10f
                };
                documents.Add(body);

                documents.Add(new Paragraph("\n", font));



                string[] conditions = new[]
                {
                    "Amended Factory Building Plans shall be submitted to Chief Inspector of Factories, Punjab in case any change in area, building, plant & machinery and manufacturing process in future.",
                    "Applicant shall obtain any other permission required under building bye-law of Housing Department of Punjab Land Authority or any other Act at his own.",
                    "The Building Cess Rs. " + (feeDetails.AmountRaised - feeDetails.AmountAlreadyPaid).ToString() + " has been deposited by the applicant under the Building & other construction worker's Welfare Cess Act, 1996. However, the applicant shall deposit the difference of amount, if any, found at any point of time.",
                    "The occupier shall submit form no. 1-F (Stability Certificate for building/revised building and after expiry of 5 years) after the completion of building and installation of plant and machinery duly certified by the Competent Person.",
                    "The occupier shall submit the application in Form No.2 to get Factory Licence after the completion of plant and machinery before start of manufacturing process.",
                    "The Concerned Party will make sure that all rules and regulations are followed and that NOCs/ approvals as required to be taken from other departments/ offices/ agencies be taken accordingly. The concerned party shall be solely responsible for any kind of violations/ deviations/ diversions made by it. Strict action shall be taken accordingly as per the rules.",
                    "Occupier shall be liable to obtain NOC from Fire Department"
                };

                int index = 1;
                foreach (var condition in conditions)
                {
                    Paragraph conditionParagraph = new Paragraph($"{index}. {condition}", font)
                    {
                        Alignment = Element.ALIGN_JUSTIFIED,
                        IndentationLeft = 10f,
                        IndentationRight = 10f
                    };
                    documents.Add(conditionParagraph);
                    index++;
                }

                Paragraph additionalText = new Paragraph("\nAdditional Director of Factories, Punjab\nFor Director of Factories (Chief Inspector of Factories), Punjab", fontBold)
                {
                    Alignment = Element.ALIGN_RIGHT,
                    SpacingBefore = 10f,
                    SpacingAfter = 10f,
                    IndentationLeft = 10f,
                    IndentationRight = 10f
                };
                documents.Add(additionalText);

                PdfPTable footerTable = new PdfPTable(1)
                {
                    TotalWidth = PageSize.A4.Width - 80,
                    LockedWidth = true,
                    SpacingBefore = 0,
                    SpacingAfter = 0
                };

                // Create a cell for the footer text
                PdfPCell footerCell = new PdfPCell(new Phrase("This letter is electronically generated, so does not require any signature.", smallFont))
                {
                    Border = PdfPCell.NO_BORDER,
                    HorizontalAlignment = Element.ALIGN_CENTER, // Center text horizontally
                    VerticalAlignment = Element.ALIGN_BOTTOM, // Align text at the bottom of the cell
                    PaddingLeft = 0,
                    PaddingRight = 0,
                    PaddingTop = 0,
                    PaddingBottom = 0
                };

                // Add the cell to the table
                footerTable.AddCell(footerCell);

                // Set the table to be at the bottom of the page
                footerTable.WriteSelectedRows(0, -1, 0, 40, owriter.DirectContent);

                documents.Close();
                return genericFormModel;
            }
            catch (Exception ex)
            {
                genericFormModel.HasException = true;
                genericFormModel.Exceptions = ex.Message;
                return genericFormModel;
            }
        }

        private async Task<CertificateGenerateServiceResultTemplate> Certificate_AppForm_72(Int64 appRefId, string CertificatePerformaURL, ApplicationTypeEnum applicationType, string userName, string licenceNo, bool isDeemedCalling)
        {
            CertificateGenerateServiceResultTemplate genericFormModel = new CertificateGenerateServiceResultTemplate();
            try
            {
                Guid guId = Guid.NewGuid();
                genericFormModel.PdfNameGUID = guId.ToString();
                genericFormModel.HasException = false;
                var SaveDirectoryPath = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "TempFiles")).Root + guId.ToString() + ".pdf";
                iTextSharp.text.Document documents = new iTextSharp.text.Document(iTextSharp.text.PageSize.A4, 5, 5, 30, 30);

                // Create a new PDF writer instance
                PdfWriter owriter = PdfWriter.GetInstance(documents, new FileStream(SaveDirectoryPath, FileMode.Create));
                PDFPageEvents pageEvent = new PDFPageEvents();
                owriter.PageEvent = pageEvent;
                owriter.SetEncryption(PdfWriter.STRENGTH128BITS, "", "", PdfWriter.AllowPrinting);

                // Register fonts
                string fontpath = "Content/fonts/Times-New-Roman.TTF";
                BaseFont baseFont = BaseFont.CreateFont(fontpath, BaseFont.CP1252, BaseFont.EMBEDDED);
                Font font = new Font(baseFont, 12);
                Font fontBold = new Font(baseFont, 12, Font.BOLD);
                Font smallFont = new Font(baseFont, 8); // Smaller font size for the footer

                // Open the document
                documents.Open();

                // Add text without padding
                Paragraph paragraph1 = new Paragraph("Director of Factories, Punjab", fontBold)
                {
                    Alignment = Element.ALIGN_CENTER
                };
                documents.Add(paragraph1);

                Paragraph paragraph2 = new Paragraph("Kirat Bhawan(Model Welfare Centre) Near 10-11 Lights\nSec 64 (Phase 10),S.A.S Nagar (Mohali),Pin-160062", font)
                {
                    Alignment = Element.ALIGN_CENTER
                };
                documents.Add(paragraph2);

                // Add a line break
                documents.Add(new Paragraph("\n", font));

                var licenceDetails = _context.Applications.Where(x => x.AppId == appRefId && x.IsDeleted == false).Include(x => x.ProjectSites).FirstOrDefault();
                string dofNumber = "DOFPB" + licenceDetails.ProjectSites.ProjectSiteId.ToString().PadLeft(8, '0');

                if (licenceDetails?.ProjectSites != null)
                {
                    Paragraph establishmentParagraph = new Paragraph(licenceDetails.ProjectSites.EstablishmentName, font)
                    {
                        Alignment = Element.ALIGN_JUSTIFIED,
                        IndentationLeft = 10f,   // Padding left
                        IndentationRight = 10f   // Padding right
                    };
                    documents.Add(establishmentParagraph);

                    Paragraph addressParagraph = new Paragraph(licenceDetails.ProjectSites.Address, font)
                    {
                        Alignment = Element.ALIGN_JUSTIFIED,
                        IndentationLeft = 10f,   // Padding left
                        IndentationRight = 10f   // Padding right
                    };
                    documents.Add(addressParagraph);
                }

                if (dofNumber != null)
                {
                    Paragraph dofNoParagraph = new Paragraph(dofNumber, fontBold)
                    {
                        Alignment = Element.ALIGN_JUSTIFIED,
                        IndentationLeft = 10f,   // Padding left
                        IndentationRight = 10f   // Padding right
                    };
                    documents.Add(dofNoParagraph);

                    Paragraph dateParagraph = new Paragraph("Date :" + DateTime.Now, fontBold)
                    {
                        Alignment = Element.ALIGN_JUSTIFIED,
                        IndentationLeft = 10f,   // Padding left
                        IndentationRight = 10f   // Padding right
                    };
                    documents.Add(dateParagraph);

                    Paragraph fileNoParagraph = new Paragraph("File No :" + licenceDetails.PublicAppRefNum, fontBold)
                    {
                        Alignment = Element.ALIGN_JUSTIFIED,
                        IndentationLeft = 10f,   // Padding left
                        IndentationRight = 10f   // Padding right
                    };
                    documents.Add(fileNoParagraph);
                }

                Paragraph subject = new Paragraph("\nSubject: Acceptance of Existing Factory Building Plans and Stability Certificate under section-6 of the Factories ACT, 1948 and Punjab Factories Rules 1952.", fontBold)
                {
                    Alignment = Element.ALIGN_JUSTIFIED,
                    IndentationLeft = 10f,   // Padding left
                    IndentationRight = 10f   // Padding right
                };
                documents.Add(subject);

                // Construct the paragraph text with specific parts in bold
                string establishmentName = licenceDetails.ProjectSites.EstablishmentName;
                string address = licenceDetails.ProjectSites.Address;

                Chunk normalChunk1 = new Chunk("\nThe Existing building plans and Stability Certificate of ", font);
                Chunk boldChunk1 = new Chunk(establishmentName + ", ", fontBold); // Bold establishment name
                Chunk normalChunk2 = new Chunk(address + ", Punjab (India) ", fontBold);
                Chunk normalChunk3 = new Chunk("have been found correct in accordance with the provisions of the Factories Act, 1948 and Punjab Factory Rules, 1952.Building Plan and Stability Certificate got accepted under Factories Act, 1948 and rules made there under subject to the following conditions: -", font);
                //Chunk normalChunk4 = new Chunk("got accepted under Factories Act, 1948 by the Chief Inspector of Factories, Punjab, subject to the following conditions: -", font);

                Phrase paragraphPhrase = new Phrase();
                paragraphPhrase.Add(normalChunk1);
                paragraphPhrase.Add(boldChunk1);
                paragraphPhrase.Add(normalChunk2);
                paragraphPhrase.Add(normalChunk3);

                // Add the Phrase to the Paragraph
                Paragraph body = new Paragraph(paragraphPhrase)
                {
                    Alignment = Element.ALIGN_JUSTIFIED,
                    IndentationLeft = 10f,   // Padding left
                    IndentationRight = 10f   // Padding right
                };
                documents.Add(body);

                // Add margin from the top before the list
                documents.Add(new Paragraph("\n", font)); // Adjust as needed for more space

                // Manually add numbered conditions
                string[] conditions = new[]
                {
                    "Amended Factory Building Plans shall be submitted in case of any change in area, building, plant & machinery and manufacturing process.",
                    "Cess has been deposited under The Building & Other Construction Workers' Welfare Cess Act, 1996. However, the applicant shall deposit the difference of amount, if any, found at any point of time.",
                    "Stability Certifcate in Form No. 1F shall be valid as per the provisions laid down in Rule 4 of Punjab Fcatory Rules, 1952.",
                    "If approval of Building Plans has not been obtained by the applicant from competent authority under the building by-laws i.e. local authority for areas within M.C Limit and Labour Department / HUD Department for areas outside M.C Limit, then the applicant shall be  bound to get the building plans approved by competent authority as per the building by - laws within the period of six months.",
                    "It is again clarified, this approval is only w.r.t the provisions of Factories Act, 1948 and rules made thereunder. This may not be considered an approval/waiver of approval of building plan under the building by-laws as explained in above para. The responsibility of approval of building plan lies entirely on the applicant, in case at the time of approval of this plan under the building by-laws any changes are made w.r.t to ventilation, skylight, breathing space, layout of machinery, provisions of canteen, occupational health center etc. then the applicant will have to submit revised plans under the Factories Act, 1948.",
                    "Occupier shall be liable to obtain NOC from Fire Department"
                };

                int index = 1;
                foreach (var condition in conditions)
                {
                    Paragraph conditionParagraph = new Paragraph($"{index}. {condition}", font)
                    {
                        Alignment = Element.ALIGN_JUSTIFIED,
                        IndentationLeft = 10f,   // Padding left
                        IndentationRight = 10f   // Padding right
                    };
                    documents.Add(conditionParagraph);
                    index++;
                }

                // Add the additional text before the footer
                Paragraph additionalText = new Paragraph("\nAdditional Director of Factories, Punjab\nFor Director of Factories (Chief Inspector of Factories), Punjab", fontBold)
                {
                    Alignment = Element.ALIGN_RIGHT, // Justify the text
                    SpacingBefore = 10f, // Margin-top before the text
                    SpacingAfter = 10f, // Margin-bottom after the text
                    IndentationLeft = 10f,   // Padding left
                    IndentationRight = 10f   // Padding right
                };
                documents.Add(additionalText);

                // Create a table for the footer with a single cell
                PdfPTable footerTable = new PdfPTable(1)
                {
                    TotalWidth = PageSize.A4.Width - 80, // Adjust width if needed
                    LockedWidth = true,
                    SpacingBefore = 0, // No space before the table
                    SpacingAfter = 0 // No space after the table
                };

                // Create a cell for the footer text
                PdfPCell footerCell = new PdfPCell(new Phrase("This letter is electronically generated, so does not require any signature.", smallFont))
                {
                    Border = PdfPCell.NO_BORDER,
                    HorizontalAlignment = Element.ALIGN_CENTER, // Center text horizontally
                    VerticalAlignment = Element.ALIGN_BOTTOM, // Align text at the bottom of the cell
                    PaddingLeft = 0,
                    PaddingRight = 0,
                    PaddingTop = 0,
                    PaddingBottom = 0
                };

                // Add the cell to the table
                footerTable.AddCell(footerCell);

                // Set the table to be at the bottom of the page
                footerTable.WriteSelectedRows(0, -1, 0, 40, owriter.DirectContent);

                documents.Close();
                return genericFormModel;
            }
            catch (Exception ex)
            {
                genericFormModel.HasException = true;
                genericFormModel.Exceptions = ex.Message;
                return genericFormModel;
            }
        }

        private async Task<CertificateGenerateServiceResultTemplate> Certificate_AppForm_73(Int64 appRefId, string CertificatePerformaURL, ApplicationTypeEnum applicationType, string userName, string licenceNo, bool isDeemedCalling)
        {
            CertificateGenerateServiceResultTemplate genericFormModel = new CertificateGenerateServiceResultTemplate();
            try
            {
                Guid guId = Guid.NewGuid();
                genericFormModel.PdfNameGUID = guId.ToString();
                genericFormModel.HasException = false;
                var SaveDirectoryPath = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "TempFiles")).Root + guId.ToString() + ".pdf";
                iTextSharp.text.Document documents = new iTextSharp.text.Document(iTextSharp.text.PageSize.A4, 5, 5, 30, 30);

                // Create a new PDF writer instance
                PdfWriter owriter = PdfWriter.GetInstance(documents, new FileStream(SaveDirectoryPath, FileMode.Create));
                PDFPageEvents pageEvent = new PDFPageEvents();
                owriter.PageEvent = pageEvent;
                owriter.SetEncryption(PdfWriter.STRENGTH128BITS, "", "", PdfWriter.AllowPrinting);

                // Register fonts
                string fontpath = "Content/fonts/Times-New-Roman.TTF";
                BaseFont baseFont = BaseFont.CreateFont(fontpath, BaseFont.CP1252, BaseFont.EMBEDDED);
                Font font = new Font(baseFont, 12);
                Font fontBold = new Font(baseFont, 12, Font.BOLD);
                Font smallFont = new Font(baseFont, 8); // Smaller font size for the footer

                // Open the document
                documents.Open();

                // Add text without padding
                Paragraph paragraph1 = new Paragraph("Director of Factories, Punjab", fontBold)
                {
                    Alignment = Element.ALIGN_CENTER
                };
                documents.Add(paragraph1);

                Paragraph paragraph2 = new Paragraph("Kirat Bhawan(Model Welfare Centre) Near 10-11 Lights\nSec 64 (Phase 10),S.A.S Nagar (Mohali),Pin-160062", font)
                {
                    Alignment = Element.ALIGN_CENTER
                };
                documents.Add(paragraph2);

                // Add a line break
                documents.Add(new Paragraph("\n", font));

                var licenceDetails = _context.Applications.Where(x => x.AppId == appRefId && x.IsDeleted == false).Include(x => x.ProjectSites).FirstOrDefault();
                string dofNumber = "DOFPB" + licenceDetails.ProjectSites.ProjectSiteId.ToString().PadLeft(8, '0');

                if (licenceDetails?.ProjectSites != null)
                {
                    Paragraph establishmentParagraph = new Paragraph(licenceDetails.ProjectSites.EstablishmentName, font)
                    {
                        Alignment = Element.ALIGN_JUSTIFIED,
                        IndentationLeft = 10f,   // Padding left
                        IndentationRight = 10f   // Padding right
                    };
                    documents.Add(establishmentParagraph);

                    Paragraph addressParagraph = new Paragraph(licenceDetails.ProjectSites.Address, font)
                    {
                        Alignment = Element.ALIGN_JUSTIFIED,
                        IndentationLeft = 10f,   // Padding left
                        IndentationRight = 10f   // Padding right
                    };
                    documents.Add(addressParagraph);
                }

                if (dofNumber != null)
                {
                    Paragraph dofNoParagraph = new Paragraph(dofNumber, fontBold)
                    {
                        Alignment = Element.ALIGN_JUSTIFIED,
                        IndentationLeft = 10f,   // Padding left
                        IndentationRight = 10f   // Padding right
                    };
                    documents.Add(dofNoParagraph);

                    Paragraph dateParagraph = new Paragraph("Date :" + DateTime.Now, fontBold)
                    {
                        Alignment = Element.ALIGN_JUSTIFIED,
                        IndentationLeft = 10f,   // Padding left
                        IndentationRight = 10f   // Padding right
                    };
                    documents.Add(dateParagraph);

                    Paragraph fileNoParagraph = new Paragraph("File No :" + licenceDetails.PublicAppRefNum, fontBold)
                    {
                        Alignment = Element.ALIGN_JUSTIFIED,
                        IndentationLeft = 10f,   // Padding left
                        IndentationRight = 10f   // Padding right
                    };
                    documents.Add(fileNoParagraph);
                }

                Paragraph subject = new Paragraph("\nSubject: Acceptance of Existing Factory Building Plans and Stability Certificate under section-6 of the Factories ACT, 1948 and Punjab Factories Rules 1952.", fontBold)
                {
                    Alignment = Element.ALIGN_JUSTIFIED,
                    IndentationLeft = 10f,   // Padding left
                    IndentationRight = 10f   // Padding right
                };
                documents.Add(subject);

                // Construct the paragraph text with specific parts in bold
                string establishmentName = licenceDetails.ProjectSites.EstablishmentName;
                string address = licenceDetails.ProjectSites.Address;

                Chunk normalChunk1 = new Chunk("\nThe Existing building plans and Stability Certificate of ", font);
                Chunk boldChunk1 = new Chunk(establishmentName + ", ", fontBold); // Bold establishment name
                Chunk normalChunk2 = new Chunk(address + ", Punjab (India) ", fontBold);
                Chunk normalChunk3 = new Chunk("have been found correct in accordance with the provisions of the Factories Act, 1948 and Punjab Factory Rules, 1952.Building Plan and Stability Certificate got accepted under Factories Act, 1948 and rules made there under subject to the following conditions: -", font);
                //Chunk normalChunk4 = new Chunk("got accepted under Factories Act, 1948 by the Chief Inspector of Factories, Punjab, subject to the following conditions: -", font);

                Phrase paragraphPhrase = new Phrase();
                paragraphPhrase.Add(normalChunk1);
                paragraphPhrase.Add(boldChunk1);
                paragraphPhrase.Add(normalChunk2);
                paragraphPhrase.Add(normalChunk3);

                // Add the Phrase to the Paragraph
                Paragraph body = new Paragraph(paragraphPhrase)
                {
                    Alignment = Element.ALIGN_JUSTIFIED,
                    IndentationLeft = 10f,   // Padding left
                    IndentationRight = 10f   // Padding right
                };
                documents.Add(body);

                // Add margin from the top before the list
                documents.Add(new Paragraph("\n", font)); // Adjust as needed for more space

                // Manually add numbered conditions
                string[] conditions = new[]
                {
                    "Amended Factory Building Plans shall be submitted in case of any change in area, building, plant & machinery and manufacturing process.",
                    "Cess has been deposited under The Building & Other Construction Workers' Welfare Cess Act, 1996. However, the applicant shall deposit the difference of amount, if any, found at any point of time.",
                    "Stability Certifcate in Form No. 1F shall be valid as per the provisions laid down in Rule 4 of Punjab Fcatory Rules, 1952.",
                    "If approval of Building Plans has not been obtained by the applicant from competent authority under the building by-laws i.e. local authority for areas within M.C Limit and Labour Department / HUD Department for areas outside M.C Limit, then the applicant shall be  bound to get the building plans approved by competent authority as per the building by - laws within the period of six months.",
                    "It is again clarified, this approval is only w.r.t the provisions of Factories Act, 1948 and rules made thereunder. This may not be considered an approval/waiver of approval of building plan under the building by-laws as explained in above para. The responsibility of approval of building plan lies entirely on the applicant, in case at the time of approval of this plan under the building by-laws any changes are made w.r.t to ventilation, skylight, breathing space, layout of machinery, provisions of canteen, occupational health center etc. then the applicant will have to submit revised plans under the Factories Act, 1948.",
                    "Occupier shall be liable to obtain NOC from Fire Department"
                };

                int index = 1;
                foreach (var condition in conditions)
                {
                    Paragraph conditionParagraph = new Paragraph($"{index}. {condition}", font)
                    {
                        Alignment = Element.ALIGN_JUSTIFIED,
                        IndentationLeft = 10f,   // Padding left
                        IndentationRight = 10f   // Padding right
                    };
                    documents.Add(conditionParagraph);
                    index++;
                }

                // Add the additional text before the footer
                Paragraph additionalText = new Paragraph("\nAdditional Director of Factories, Punjab\nFor Director of Factories (Chief Inspector of Factories), Punjab", fontBold)
                {
                    Alignment = Element.ALIGN_RIGHT, // Justify the text
                    SpacingBefore = 10f, // Margin-top before the text
                    SpacingAfter = 10f, // Margin-bottom after the text
                    IndentationLeft = 10f,   // Padding left
                    IndentationRight = 10f   // Padding right
                };
                documents.Add(additionalText);

                // Create a table for the footer with a single cell
                PdfPTable footerTable = new PdfPTable(1)
                {
                    TotalWidth = PageSize.A4.Width - 80, // Adjust width if needed
                    LockedWidth = true,
                    SpacingBefore = 0, // No space before the table
                    SpacingAfter = 0 // No space after the table
                };

                // Create a cell for the footer text
                PdfPCell footerCell = new PdfPCell(new Phrase("This letter is electronically generated, so does not require any signature.", smallFont))
                {
                    Border = PdfPCell.NO_BORDER,
                    HorizontalAlignment = Element.ALIGN_CENTER, // Center text horizontally
                    VerticalAlignment = Element.ALIGN_BOTTOM, // Align text at the bottom of the cell
                    PaddingLeft = 0,
                    PaddingRight = 0,
                    PaddingTop = 0,
                    PaddingBottom = 0
                };

                // Add the cell to the table
                footerTable.AddCell(footerCell);

                // Set the table to be at the bottom of the page
                footerTable.WriteSelectedRows(0, -1, 0, 40, owriter.DirectContent);

                documents.Close();
                return genericFormModel;
            }
            catch (Exception ex)
            {
                genericFormModel.HasException = true;
                genericFormModel.Exceptions = ex.Message;
                return genericFormModel;
            }
        }

        private async Task<CertificateGenerateServiceResultTemplate> Certificate_AppForm_8(Int64 appRefId, string CertificatePerformaURL, ApplicationTypeEnum applicationType, string userName, string licenceNo, bool isDeemedCalling)
        {
            CertificateGenerateServiceResultTemplate genericFormModel = new CertificateGenerateServiceResultTemplate();
            try
            {
                Guid guId = Guid.NewGuid();
                genericFormModel.PdfNameGUID = guId.ToString();
                var sfilepath = "";
                sfilepath = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "TempFiles")).Root + guId.ToString() + ".pdf";
                FileInfo file = new FileInfo(sfilepath);
                if (file.Exists)
                {
                    file.Delete();
                }
                iTextSharp.text.Document documents = new iTextSharp.text.Document(iTextSharp.text.PageSize.A4, 5, 5, 30, 30);
                documents.Open();
                #region ClassObjects
                pdfFonts classFonts = new pdfFonts();
                #endregion

                #region CreateObjects
                PdfWriter owriter = PdfWriter.GetInstance(documents, new FileStream(sfilepath, FileMode.Create));
                PDFPageEvents pageEvent = new PDFPageEvents();
                owriter.PageEvent = pageEvent;
                owriter.SetEncryption(PdfWriter.STRENGTH128BITS, "", "", PdfWriter.AllowPrinting);
                #endregion

                var fontName = "Verdana";
                if (!FontFactory.IsRegistered(fontName))
                {
                    var fontPath = "Content/fonts/VERDANA.TTF";
                    FontFactory.Register(fontPath);
                }

                #region Tables
                var parentWithChildObject = await _iGR_Licence_TradeUnion
                      .GetAsync(x => x.AppRefId == appRefId,  //Conditions         
                        null,                //Orders          
                        x => x.Application, x => x.Application.ApplicationAction, x => x.Application.AppFeeDetails)      //Includes
                      .ConfigureAwait(false);
                var genralDetail = parentWithChildObject.FirstOrDefault();
                var projectSite = _context.ProjectSites.Where(x => x.ProjectSiteId == genralDetail.Application.ProjectSiteRefId).FirstOrDefault();
                var alcCircle = _context.ALCCircles.Where(x => x.ALCCircleId == projectSite.AlcCircleRefId).FirstOrDefault();
                #endregion

                string formattedLastDate = "";

                if (genralDetail.Application.ApplicationPurposeType == ApplicationPurposeTypeEnum.GRANT_LICENCE)
                {
                    int approveldate = (int)genralDetail.LicenceForYear;
                    DateTime lastDateOfCurrentYear = new DateTime(approveldate, 12, 31);
                    formattedLastDate = lastDateOfCurrentYear.ToString("dd MMMM yyyy");
                }
                else if (genralDetail.Application.ApplicationPurposeType == ApplicationPurposeTypeEnum.AMENDMENT_LICENCE)
                {

                    int approveldate = (int)genralDetail.LicenceForYear;
                    DateTime lastDateOfCurrentYear = new DateTime(approveldate, 12, 31);
                    formattedLastDate = lastDateOfCurrentYear.ToString("dd MMMM yyyy");
                }


                string fontpath1 = "Content/fonts/VERDANA.TTF";
                BaseFont customfont = BaseFont.CreateFont(fontpath1, BaseFont.CP1252, BaseFont.EMBEDDED);
                string fontpath2 = "Content/fonts/VERDANA.TTF";
                BaseFont customfontimpact = BaseFont.CreateFont(fontpath2, BaseFont.CP1252, BaseFont.EMBEDDED);
                documents.Open();
                Font verdanaHeader = FontFactory.GetFont("Arial", 9, Font.BOLD);
                Font footer = FontFactory.GetFont("Arial", 9, Font.BOLD, BaseColor.BLUE);
                Font verdana = FontFactory.GetFont("Arial", 8, BaseColor.BLACK);
                Font verdanaBoldItalic = FontFactory.GetFont("Arial", 8, Font.BOLDITALIC);
                Font verdanasmall = FontFactory.GetFont("Arial", 8, Font.NORMAL);
                verdanasmall.Color = BaseColor.WHITE;
                Font verdanaRightHeader = FontFactory.GetFont("Arial", 15, Font.BOLD);
                verdanaRightHeader.Color = BaseColor.WHITE;
                Font verdanaRightHeader1 = FontFactory.GetFont("Arial", 12, Font.BOLD);
                verdanaRightHeader1.Color = BaseColor.WHITE;
                Font arialOldLicenceDetails = FontFactory.GetFont("Arial", 10, Font.BOLD);
                arialOldLicenceDetails.Color = BaseColor.WHITE;
                Font monotypeHeader = new Font(customfont, 20);
                Font monotype = FontFactory.GetFont("Arial", 13, Font.BOLD);
                Font impact = new Font(customfontimpact, 12);
                Font impactHeader = new Font(customfontimpact, 11, Font.BOLD);
                impactHeader.Color = BaseColor.WHITE;
                BaseFont bfTimes = BaseFont.CreateFont(BaseFont.TIMES_ROMAN, BaseFont.CP1252, false);
                Font timesRoman = FontFactory.GetFont("Arial", 10, BaseColor.BLACK);
                Font ArialFont = FontFactory.GetFont("Arial", 8, BaseColor.WHITE);
                Font SealFontArial = FontFactory.GetFont("Arial", 9, BaseColor.BLACK);
                Font verdanaBold = FontFactory.GetFont("Arial", 10, Font.BOLD);
                Font verdanalic = FontFactory.GetFont("Arial", 9, Font.BOLD);
                verdanalic.Color = BaseColor.WHITE;
                var factoryPerforma = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "LicensePerformas")).Root + "CertificatePerforma_MotorTransport.jpg";
                iTextSharp.text.Image pageLogo1 = iTextSharp.text.Image.GetInstance(new FileStream(factoryPerforma, FileMode.OpenOrCreate, FileAccess.Read));

                pageLogo1.Alignment = iTextSharp.text.Image.ALIGN_CENTER | iTextSharp.text.Image.UNDERLYING;
                pageLogo1.ScaleToFit(800f, 800f);
                pageLogo1.SetAbsolutePosition(10f, 25f);
                pageLogo1.BorderWidth = 1;
                documents.Add(pageLogo1);
                PdfPTable ptableleftSideDetail = new PdfPTable(1);
                ptableleftSideDetail.DefaultCell.Border = 0;
                ptableleftSideDetail.DefaultCell.Padding = 0;
                ptableleftSideDetail.HorizontalAlignment = (Element.ALIGN_JUSTIFIED);
                ptableleftSideDetail.DefaultCell.VerticalAlignment = (Element.ALIGN_JUSTIFIED);
                ptableleftSideDetail.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
                ptableleftSideDetail.AddCell(new PdfPCell(new Phrase("", verdanaBold)) { Border = 0, PaddingTop = 10 });



                ptableleftSideDetail.AddCell(new PdfPCell(new Phrase("Licence No. : " + licenceNo.ToString(), verdanalic)) { Border = 0, PaddingTop = 300, PaddingLeft = -18 }); ;
                ptableleftSideDetail.AddCell(new PdfPCell(new Phrase("Licence Issued On : " + String.Format("{0:dd-MMM-yyyy}", DateTime.Now.ToShortDateString()), verdanalic)) { Border = 0, PaddingTop = 10, PaddingLeft = -18 });
                //ptableleftSideDetail.AddCell(new PdfPCell(new Phrase("Valid upto  : " + formattedLastDate, verdanalic)) { Border = 0, PaddingTop = 10, PaddingLeft = -18 });

                Font verdanaBoldBP = new Font(BaseFont.CreateFont(BaseFont.TIMES_ROMAN, BaseFont.CP1252, false), 8, Font.BOLD | Font.UNDERLINE, BaseColor.WHITE);
                Phrase phrase = new Phrase();

                ptableleftSideDetail.AddCell(new PdfPCell(phrase) { Border = 0, PaddingTop = 10, PaddingRight = 20 });

                #region QR Code Generate
                var qrCodeDetails = "Establishment Name : " + projectSite.EstablishmentName + "|" + "Establishment Address : " + projectSite.Address + "|" + "Contact No : " + projectSite.ContactPersonFirstName + " " + projectSite.ContactPersonLastName + "|" + projectSite.ContactPersonMobileNo + "|" + "Licence No : " + licenceNo + "|" + "File No : " + genralDetail.Application.PublicAppRefNum;
                //string base64String = System.Convert.ToBase64String(plainTextBytes);
                QRCodeGenerator qrGenerator = new QRCodeGenerator();
                QRCodeGenerator.QRCode qrCode = qrGenerator.CreateQrCode(qrCodeDetails, QRCodeGenerator.ECCLevel.Q);

                using (System.Drawing.Bitmap bitMap = qrCode.GetGraphic(20))
                {
                    using (MemoryStream ms = new MemoryStream())
                    {
                        bitMap.Save(ms, ImageFormat.Jpeg);
                        System.Drawing.Image imgQRCode = System.Drawing.Image.FromStream(ms);
                        iTextSharp.text.Image imdf = iTextSharp.text.Image.GetInstance(imgQRCode, System.Drawing.Imaging.ImageFormat.Jpeg);
                        imdf.ScaleAbsoluteHeight(120f);
                        imdf.ScaleAbsoluteWidth(120f);
                        ptableleftSideDetail.AddCell(new PdfPCell(imdf) { Border = 0, PaddingTop = 30, PaddingLeft = 0, PaddingRight = 0 });
                    }
                }
                #endregion

                ptableleftSideDetail.AddCell(new PdfPCell(new Phrase("" + Chunk.NEWLINE + "Department of Labour, Govt. of Punjab" + Chunk.NEWLINE, verdanaRightHeader1)) { Border = 0, PaddingTop = 15, PaddingRight = 25, HorizontalAlignment = PdfPCell.ALIGN_CENTER });

                PdfPTable ptablerightSideDetail = new PdfPTable(1);
                ptablerightSideDetail.DefaultCell.Border = 0;
                ptablerightSideDetail.DefaultCell.Padding = 0;

                //ptablerightSideDetail.AddCell(new PdfPCell(new Phrase("", verdanaBold)) { Border = 0, Padding = 4 });
                ptablerightSideDetail.AddCell(new PdfPCell(new Phrase("FORM-C", verdanaHeader)) { Border = 0, PaddingLeft = 20, PaddingTop = 10, HorizontalAlignment = PdfPCell.ALIGN_CENTER });
                //ptablerightSideDetail.AddCell(new PdfPCell(new Phrase("[See Rule 5]", verdanaHeader)) { Border = 0, PaddingLeft = 20, HorizontalAlignment = PdfPCell.ALIGN_CENTER });
                ptablerightSideDetail.AddCell(new PdfPCell(new Phrase("CERTIFICATE OF REGISTRATION\r\nOF TRADE UNIONS ACT XVI of 1926", impactHeader)) { Border = 0, PaddingTop = 25, PaddingBottom = 20, PaddingLeft = 40, PaddingRight = 30, VerticalAlignment = PdfPCell.ALIGN_MIDDLE, HorizontalAlignment = PdfPCell.ALIGN_CENTER });
                ptablerightSideDetail.AddCell(new PdfPCell(new Phrase("CERTIFICATE OF REGISTRATION", monotype)) { Border = 0, PaddingBottom = 4, PaddingTop = 20, HorizontalAlignment = PdfPCell.ALIGN_CENTER });
                ptablerightSideDetail.AddCell(new PdfPCell(new Phrase("GOVERNMENT OF PUNJAB", monotype)) { Border = 0, PaddingBottom = 4, HorizontalAlignment = PdfPCell.ALIGN_CENTER });
                ptablerightSideDetail.AddCell(new PdfPCell(new Phrase("", verdanaBold)) { Border = 0, Padding = 1, PaddingTop = 20 });
                ptablerightSideDetail.AddCell(new PdfPCell(new Phrase(" Dated: " + DateTime.Now.ToShortDateString() + "                            Licence No. : " + licenceNo.ToString(), verdanaHeader)) { Border = 0, PaddingTop = 20 });


                Paragraph para1 = new Paragraph();
                para1.Add(new Chunk(" It is hereby certified that  ", timesRoman));
                para1.Add(new Chunk(genralDetail.TUName, verdanaBold));
                para1.Add(new Chunk(" Address  ", timesRoman));
                para1.Add(new Chunk(genralDetail.TUAddress + "", verdanaBold));
                para1.Add(new Chunk(Chunk.NEWLINE + "has been registered under the Indian Trade Unions Act, 1926\r\non this ", timesRoman));
                para1.Add(new Chunk(String.Format("{0:dd-MMM-yyyy}", DateTime.Now.ToShortDateString()), verdanaBold));
                PdfPCell shipment1 = new PdfPCell(para1);
                shipment1.SetLeading(2f, 2f);
                ptablerightSideDetail.AddCell(new PdfPCell(shipment1) { Border = 0, HorizontalAlignment = PdfPCell.ALIGN_JUSTIFIED, VerticalAlignment = PdfPCell.ALIGN_JUSTIFIED, FixedHeight = 350, PaddingBottom = 25 });



                var userDetails = await _iAuthService.GetUserFullDetailByUserName(userName);
                string officerSigFilePath = "";
                if (!isDeemedCalling)
                {

                    if (userDetails.ListData.Count() == 0 || userDetails.ListData[0].Signature == null)
                    {
                        userDetails.ListData[0].Signature = "abc.jpg";
                    }
                    officerSigFilePath = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "OfficerSignatures")).Root + userDetails.ListData[0].Signature;
                    iTextSharp.text.Image signature = iTextSharp.text.Image.GetInstance(new FileStream(officerSigFilePath, FileMode.OpenOrCreate, FileAccess.Read));
                    signature.ScaleToFit(120f, 120f);
                    signature.SetAbsolutePosition(630f, 150f);
                    ptablerightSideDetail.AddCell(new PdfPCell(signature) { Border = 0, PaddingTop = 5, HorizontalAlignment = PdfPCell.ALIGN_RIGHT });
                }


                Paragraph SignSeal = new Paragraph();
                SignSeal.Add(new Chunk(Chunk.NEWLINE + "REGISTRAR", verdanaHeader));
                SignSeal.Add(new Chunk(Chunk.NEWLINE + "REGISTRAR, TRADE UNION", verdanaHeader));
                SignSeal.Add(new Chunk(Chunk.NEWLINE + "PUNJAB", verdanaHeader));



                ptablerightSideDetail.AddCell(new PdfPCell(SignSeal) { Border = 0, HorizontalAlignment = PdfPCell.ALIGN_RIGHT });



                Paragraph SignSeal1 = new Paragraph();
                SignSeal1.Add(new Chunk(Chunk.NEWLINE + "Name: ", verdanaHeader));
                SignSeal1.Add(new Chunk(userDetails.ListData[0].UserFullName, SealFontArial));
                SignSeal1.Add(new Chunk(Chunk.NEWLINE + "Desig: ", verdanaHeader));
                SignSeal1.Add(new Chunk("Labour Commissioner Punjab", SealFontArial));


                ptablerightSideDetail.AddCell(new PdfPCell(SignSeal1) { Border = 0, HorizontalAlignment = PdfPCell.ALIGN_LEFT, PaddingLeft = 140 });
                //ptablerightSideDetail.AddCell(new PdfPCell(new Phrase(" Dated " + DateTime.Now.ToShortDateString(), verdanaHeader)) { Border = 0, HorizontalAlignment = PdfPCell.ALIGN_LEFT});

                ptablerightSideDetail.AddCell(new PdfPCell(new Phrase("This is a computer generated document to check & verify please login to https://pblabour.gov.in/Account/ApplicationStatus", footer)) { Border = 1, BorderWidth = 1, Padding = 8 });
                PdfPTable ptableShopDetailLicenceApplied = new PdfPTable(3);
                ptableShopDetailLicenceApplied.DefaultCell.Border = 0;
                ptableShopDetailLicenceApplied.DefaultCell.Padding = 2;
                ptableShopDetailLicenceApplied.SpacingBefore = 1000;
                ptableShopDetailLicenceApplied.AddCell(new PdfPCell(ptableleftSideDetail) { Border = 0 });
                ptableShopDetailLicenceApplied.AddCell(new PdfPCell(ptablerightSideDetail) { Border = 0, Colspan = 2 });
                documents.Add(ptableShopDetailLicenceApplied);
                documents.Close();
                PDFPageEvents pageeventsobj = new PDFPageEvents();

                //ptablerightSideDetail.AddCell(new PdfPCell(para) { Border = 0, VerticalAlignment = PdfPCell.ALIGN_JUSTIFIED, HorizontalAlignment = PdfPCell.ALIGN_JUSTIFIED, FixedHeight = 400 });
            }
            catch (Exception ex)
            {
                genericFormModel.HasException = true;
                genericFormModel.Exceptions = ex.Message;
            }
            return genericFormModel;
        }
        //------epfo notice---

        private async Task<NoticeGenerateServiceResultTemplate> Certificate_EPFO(string epfoid, string userName, Int64 establishmentEPFOLogsRefId, string disputeNo)
        {
            NoticeGenerateServiceResultTemplate genericFormModel = new NoticeGenerateServiceResultTemplate();
            try
            {
                Guid guId = Guid.NewGuid();
                genericFormModel.PdfNameGUID = guId.ToString();
                genericFormModel.EstablishmentEPFOLogsRefId = establishmentEPFOLogsRefId;
                var sfilepath = "";
                sfilepath = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "TempFiles")).Root + guId.ToString() + ".pdf";
                FileInfo file = new FileInfo(sfilepath);
                if (file.Exists)
                {
                    file.Delete();
                }
                iTextSharp.text.Document documents = new iTextSharp.text.Document(iTextSharp.text.PageSize.A4, 5, 5, 30, 30);
                documents.Open();
                #region ClassObjects
                pdfFonts classFonts = new pdfFonts();
                #endregion

                #region CreateObjects
                PdfWriter owriter = PdfWriter.GetInstance(documents, new FileStream(sfilepath, FileMode.Create));
                PDFPageEvents pageEvent = new PDFPageEvents();
                owriter.PageEvent = pageEvent;
                owriter.SetEncryption(PdfWriter.STRENGTH128BITS, "", "", PdfWriter.AllowPrinting);
                #endregion

                var fontName = "Verdana";
                if (!FontFactory.IsRegistered(fontName))
                {
                    var fontPath = "Content/fonts/VERDANA.TTF";
                    FontFactory.Register(fontPath);
                }

                #region Tables
                var establishment = _context.Establishment_EPFO.Where(x => x.EstablishmentId == epfoid).FirstOrDefault();
                #endregion

                string fontpath1 = "Content/fonts/VERDANA.TTF";
                BaseFont customfont = BaseFont.CreateFont(fontpath1, BaseFont.CP1252, BaseFont.EMBEDDED);
                string fontpath2 = "Content/fonts/VERDANA.TTF";
                BaseFont customfontimpact = BaseFont.CreateFont(fontpath2, BaseFont.CP1252, BaseFont.EMBEDDED);
                documents.Open();
                Font verdanaHeader = FontFactory.GetFont("Arial", 9, Font.BOLD);
                Font footer = FontFactory.GetFont("Arial", 9, Font.BOLD, BaseColor.BLUE);
                Font verdana = FontFactory.GetFont("Arial", 8, BaseColor.BLACK);
                Font verdanaBoldItalic = FontFactory.GetFont("Arial", 8, Font.BOLDITALIC);
                Font verdanasmall = FontFactory.GetFont("Arial", 8, Font.NORMAL);
                verdanasmall.Color = BaseColor.WHITE;
                Font verdanaRightHeader = FontFactory.GetFont("Arial", 15, Font.BOLD);
                verdanaRightHeader.Color = BaseColor.WHITE;
                Font verdanaRightHeader1 = FontFactory.GetFont("Arial", 12, Font.BOLD);
                verdanaRightHeader1.Color = BaseColor.WHITE;
                Font arialOldLicenceDetails = FontFactory.GetFont("Arial", 10, Font.BOLD);
                arialOldLicenceDetails.Color = BaseColor.WHITE;
                Font monotypeHeader = new Font(customfont, 20);
                Font monotype = FontFactory.GetFont("Arial", 13, Font.BOLD);
                Font impact = new Font(customfontimpact, 12);
                Font impactHeader = new Font(customfontimpact, 11, Font.BOLD);
                impactHeader.Color = BaseColor.WHITE;
                BaseFont bfTimes = BaseFont.CreateFont(BaseFont.TIMES_ROMAN, BaseFont.CP1252, false);
                Font timesRoman = FontFactory.GetFont("Arial", 10, BaseColor.BLACK);
                Font ArialFont = FontFactory.GetFont("Arial", 8, BaseColor.WHITE);
                Font SealFontArial = FontFactory.GetFont("Arial", 9, BaseColor.BLACK);
                Font verdanaBold = FontFactory.GetFont("Arial", 10, Font.BOLD);
                Font verdanalic = FontFactory.GetFont("Arial", 9, Font.BOLD);
                verdanalic.Color = BaseColor.WHITE;
                var noticePerforma = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "LicensePerformas")).Root + "Notice.jpg";
                iTextSharp.text.Image pageLogo1 = iTextSharp.text.Image.GetInstance(new FileStream(noticePerforma, FileMode.OpenOrCreate, FileAccess.Read));
                pageLogo1.Alignment = iTextSharp.text.Image.ALIGN_CENTER | iTextSharp.text.Image.UNDERLYING;
                pageLogo1.ScaleToFit(800f, 800f);
                pageLogo1.SetAbsolutePosition(10f, 25f);
                pageLogo1.BorderWidth = 1;
                documents.Add(pageLogo1);
                PdfPTable ptableleftSideDetail = new PdfPTable(1);
                #region QR Code Generate
                var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(epfoid);
                string base64String = System.Convert.ToBase64String(plainTextBytes);
                QRCodeGenerator qrGenerator = new QRCodeGenerator();
                QRCodeGenerator.QRCode qrCode = qrGenerator.CreateQrCode(base64String, QRCodeGenerator.ECCLevel.Q);

                //using (System.Drawing.Bitmap bitMap = qrCode.GetGraphic(20))
                //{
                //    using (MemoryStream ms = new MemoryStream())
                //    {
                //        bitMap.Save(ms, ImageFormat.Jpeg);
                //        System.Drawing.Image imgQRCode = System.Drawing.Image.FromStream(ms);
                //        iTextSharp.text.Image imdf = iTextSharp.text.Image.GetInstance(imgQRCode, System.Drawing.Imaging.ImageFormat.Jpeg);
                //        imdf.ScaleAbsoluteHeight(80f);
                //        imdf.ScaleAbsoluteWidth(80f);
                //        ptableleftSideDetail.AddCell(new PdfPCell(imdf) { Border = 0, PaddingTop = 30, PaddingLeft = 0, PaddingRight = 0 });
                //    }
                //}
                #endregion

                PdfPTable ptablerightSideDetail = new PdfPTable(1);
                ptablerightSideDetail.DefaultCell.Border = 0;
                ptablerightSideDetail.DefaultCell.Padding = 0;

                
                ptablerightSideDetail.AddCell(new PdfPCell(new Phrase(disputeNo, verdanaHeader))
                {
                    Border = 0,
                    PaddingLeft = 210,
                    PaddingTop = 115
                });

                ptablerightSideDetail.AddCell(new PdfPCell(new Phrase(DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss"), verdanaHeader))
                {
                    Border = 0,
                    PaddingLeft = 210,
                    PaddingTop = 4
                });

                

                ptablerightSideDetail.AddCell(new PdfPCell(new Phrase(establishment.EstablishmentId + "  ", verdanaHeader))
                {
                    Border = 0,
                    PaddingLeft = 85,  
                    PaddingTop = -6,   
                    FixedHeight = 16,
         
                });

                ptablerightSideDetail.AddCell(new PdfPCell(new Phrase(establishment.EstablishmentName, verdanaHeader))
                {
                    Border = 0,
                    PaddingLeft = 75,
                    PaddingTop = -4,
                    FixedHeight = 16
                });

                ptablerightSideDetail.AddCell(new PdfPCell(new Phrase(establishment.EstablishmentType, verdanaHeader))
                {
                    Border = 0,
                    PaddingLeft = 75,
                    PaddingTop = -4,
                    FixedHeight = 16
                });

                ptablerightSideDetail.AddCell(new PdfPCell(new Phrase(establishment.EstablishmentAddress1, verdanaHeader))
                {
                    Border = 0,
                    PaddingLeft = 75,
                    PaddingTop = -4,
                    FixedHeight = 16
                });

                ptablerightSideDetail.AddCell(new PdfPCell(new Phrase(establishment.City + " , " + establishment.District + " , " + establishment.PinCode, verdanaHeader))
                {
                    Border = 0,
                    PaddingLeft = 75,
                    PaddingTop = -4,
                    FixedHeight = 16
                });
                string officerSigFilePath = "";
                var userDetails = await _iAuthService.GetUserFullDetailByUserName(userName);
                officerSigFilePath = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "OfficerSignatures")).Root + userDetails.ListData[0].Signature;
                iTextSharp.text.Image signature = iTextSharp.text.Image.GetInstance(new FileStream(officerSigFilePath, FileMode.OpenOrCreate, FileAccess.Read));
                signature.ScaleToFit(90f, 150f);
                PdfPTable ptableShopDetailLicenceApplied = new PdfPTable(3);
                ptableShopDetailLicenceApplied.DefaultCell.Border = 0;
                ptableShopDetailLicenceApplied.DefaultCell.Padding = 2;
                ptableShopDetailLicenceApplied.SpacingBefore = 1000;
                ptableShopDetailLicenceApplied.AddCell(new PdfPCell(ptableleftSideDetail) { Border = 0 });
                ptableShopDetailLicenceApplied.AddCell(new PdfPCell(ptablerightSideDetail) { Border = 0, Colspan = 2 });
                documents.Add(ptableShopDetailLicenceApplied);
                documents.Close();
                PDFPageEvents pageeventsobj = new PDFPageEvents();

            }
            catch (Exception ex)
            {
                genericFormModel.HasException = true;
                genericFormModel.Exceptions = ex.Message;
            }
            return genericFormModel;
        }

        private async Task<GeneratePdfServiceResultTemplate> Certificate_Inspection_FactoryWing(Int64 inspectionRefId, int inspectionType)
        {
            GeneratePdfServiceResultTemplate genericFormModel = new GeneratePdfServiceResultTemplate();
            try
            {
                Guid guId = Guid.NewGuid();
                genericFormModel.PdfNameGUID = guId.ToString();
                var sfilepath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Inspections", "Inspection_" + inspectionRefId + "_" + inspectionType + ".pdf");


                using (iTextSharp.text.Document documents = new iTextSharp.text.Document(iTextSharp.text.PageSize.A4, 30, 30, 50, 50))
                {
                    PdfWriter owriter = PdfWriter.GetInstance(documents, new FileStream(sfilepath, FileMode.Create));
                    owriter.SetEncryption(PdfWriter.STRENGTH128BITS, "", "", PdfWriter.AllowPrinting);


                    documents.Open();

                    var inspectionMasterData = _context.Inspection_Master.Where(x => x.InspectionId == inspectionRefId).FirstOrDefault();
                    var establishmentDetails = await _iInspectionService.GetInspectionEstablishmentBasicDetails(inspectionMasterData.LicenceNumber);
                    documents.Add(new Paragraph("Inspection Report(Factory-Wing)", FontFactory.GetFont("Times-Roman", 20, Font.BOLD)) { Alignment = Element.ALIGN_CENTER });
                    if (inspectionMasterData.InspectionSubmited_Factory_Wing_FactoryExistenceType == InspectionFactoryExistenceTypeEnum.OPEN)
                    {
                        // Load inspection data

                        var inspectionData = await _iInspectionService.GetIncpectionFormFactoryPartIGeneralDetail(inspectionRefId);
                        var pairs = ModelOps.GetTitleAndValues<Inspection_Form_Factory_Part_I_General>(inspectionData.FormModel);

                        // Create a table for displaying inspection data
                        PdfPTable inspectionTable = new PdfPTable(2)
                        {
                            WidthPercentage = 100
                        };
                        foreach (var item in pairs)
                        {
                            inspectionTable.AddCell(CreateCell(item.Title, 10, true));
                            inspectionTable.AddCell(CreateCell(item.Value, 10, false));

                            inspectionTable.AddCell(new PdfPCell(new Phrase(""))
                            {
                                FixedHeight = 10, // Adjust height for spacing
                                Border = PdfPCell.NO_BORDER // No border for spacing cell
                            });

                            // Add another empty cell to create a gap
                            inspectionTable.AddCell(new PdfPCell(new Phrase(""))
                            {
                                FixedHeight = 10,
                                Border = PdfPCell.NO_BORDER
                            });
                        }
                        documents.Add(new Chunk(" "));
                        documents.Add(new Paragraph("Inspection Form Part-I", FontFactory.GetFont("Times-Roman", 16, Font.BOLD)) { Alignment = Element.ALIGN_CENTER });
                        documents.Add(new Chunk(""));
                        documents.Add(inspectionTable);

                        var inspectionForm2Data = await _iInspectionService.GetIncpectionFormFactoryPartIIFactoryDetail(inspectionRefId);
                        var pairs2 = ModelOps.GetTitleAndValues<Inspection_Form_Factory_Part_II_FactoryDetail>(inspectionForm2Data.FormModel);

                        // Create a table for displaying inspection data
                        PdfPTable inspectionForm2Table = new PdfPTable(2)
                        {
                            WidthPercentage = 100
                        };
                        foreach (var item in pairs2)
                        {
                            inspectionForm2Table.AddCell(CreateCell(item.Title, 10, true));
                            inspectionForm2Table.AddCell(CreateCell(item.Value, 10, false));

                            inspectionForm2Table.AddCell(new PdfPCell(new Phrase(""))
                            {
                                FixedHeight = 10, // Adjust height for spacing
                                Border = PdfPCell.NO_BORDER // No border for spacing cell
                            });

                            // Add another empty cell to create a gap
                            inspectionForm2Table.AddCell(new PdfPCell(new Phrase(""))
                            {
                                FixedHeight = 10,
                                Border = PdfPCell.NO_BORDER
                            });
                        }
                        documents.Add(new Chunk(" ")); // Add space
                        documents.Add(new Paragraph("Inspection Form Part-II", FontFactory.GetFont("Times-Roman", 16, Font.BOLD)) { Alignment = Element.ALIGN_CENTER });
                        documents.Add(new Chunk(""));
                        documents.Add(inspectionForm2Table);


                        var inspectionForm3ReportData = await _iInspectionService.GetIncpectionFormFactoryPartIIIInspectionReport(inspectionRefId);
                        var pairs3IR = ModelOps.GetTitleAndValues<Inspection_Form_Factory_Part_III_InspectionReport>(inspectionForm3ReportData.FormModel);

                        // Create a table for displaying inspection data
                        PdfPTable inspectionForm3IRTable = new PdfPTable(2)
                        {
                            WidthPercentage = 100
                        };
                        foreach (var item in pairs3IR)
                        {
                            inspectionForm3IRTable.AddCell(CreateCell(item.Title, 10, true));
                            inspectionForm3IRTable.AddCell(CreateCell(item.Value, 10, false));

                            inspectionForm3IRTable.AddCell(new PdfPCell(new Phrase(""))
                            {
                                FixedHeight = 10, // Adjust height for spacing
                                Border = PdfPCell.NO_BORDER // No border for spacing cell
                            });

                            // Add another empty cell to create a gap
                            inspectionForm3IRTable.AddCell(new PdfPCell(new Phrase(""))
                            {
                                FixedHeight = 10,
                                Border = PdfPCell.NO_BORDER
                            });
                        }
                        documents.Add(new Chunk(" ")); // Add space
                        documents.Add(new Paragraph("Inspection Form Part-III Main Report", FontFactory.GetFont("Times-Roman", 16, Font.BOLD)) { Alignment = Element.ALIGN_CENTER });
                        documents.Add(new Chunk(""));
                        documents.Add(inspectionForm3IRTable);


                        var inspectionForm3MustorRollData = await _iInspectionService.GetIncpectionFormFactoryPartIIIMusterRoll(inspectionRefId);

                        PdfPTable inspectionForm3MustorRoll1Table = new PdfPTable(8)
                        {
                            WidthPercentage = 100

                        };
                        PdfPTable inspectionForm3MustorRoll2Table = new PdfPTable(8)
                        {
                            WidthPercentage = 100

                        };
                        if (inspectionForm3MustorRollData.ResponseDataModel.Where(x => x.InspectionMusterRollType == InspectionMusterRollTypeEnum.On_INSPECTION_TIME).Count() > 0)
                        {
                            inspectionForm3MustorRoll1Table.AddCell(CreateCell("ShiftTime_From", 10, true));
                            inspectionForm3MustorRoll1Table.AddCell(CreateCell("ShiftTime_To", 10, true));
                            inspectionForm3MustorRoll1Table.AddCell(CreateCell("Adult(Male)", 10, true));
                            inspectionForm3MustorRoll1Table.AddCell(CreateCell("Adult(Female)", 10, true));
                            inspectionForm3MustorRoll1Table.AddCell(CreateCell("Adolescent(Male)", 10, true));
                            inspectionForm3MustorRoll1Table.AddCell(CreateCell("Adolescent(Female)", 10, true));
                            inspectionForm3MustorRoll1Table.AddCell(CreateCell("Children(Male)", 10, true));
                            inspectionForm3MustorRoll1Table.AddCell(CreateCell("Children(Female)", 10, true));

                            foreach (var item in inspectionForm3MustorRollData.ResponseDataModel.Where(x => x.InspectionMusterRollType == InspectionMusterRollTypeEnum.On_INSPECTION_TIME).ToList())
                            {
                                inspectionForm3MustorRoll1Table.AddCell(CreateCell(item.ShiftTime_From, 10, false));
                                inspectionForm3MustorRoll1Table.AddCell(CreateCell(item.ShiftTime_To, 10, false));
                                inspectionForm3MustorRoll1Table.AddCell(CreateCell(item.Count_Adult_Male.ToString(), 10, false));
                                inspectionForm3MustorRoll1Table.AddCell(CreateCell(item.Count_Adult_FeMale.ToString(), 10, false));
                                inspectionForm3MustorRoll1Table.AddCell(CreateCell(item.Count_Adolescent_Male.ToString(), 10, false));
                                inspectionForm3MustorRoll1Table.AddCell(CreateCell(item.Count_Adolescent_FeMale.ToString(), 10, false));
                                inspectionForm3MustorRoll1Table.AddCell(CreateCell(item.Count_Children_Male.ToString(), 10, false));
                                inspectionForm3MustorRoll1Table.AddCell(CreateCell(item.Count_Children_FeMale.ToString(), 10, false));

                            }
                            documents.Add(new Chunk(" ")); // Add space
                            documents.Add(new Paragraph("Workers Details A. Details as per muster roll (On Inspection Time)", FontFactory.GetFont("Times-Roman", 16, Font.BOLD)) { Alignment = Element.ALIGN_CENTER });
                            documents.Add(new Chunk(""));

                        }
                        documents.Add(inspectionForm3MustorRoll1Table);
                        if (inspectionForm3MustorRollData.ResponseDataModel.Where(x => x.InspectionMusterRollType == InspectionMusterRollTypeEnum.ON_LICENSE).Count() > 0)
                        {
                            inspectionForm3MustorRoll2Table.AddCell(CreateCell("ShiftTime_From", 10, true));
                            inspectionForm3MustorRoll2Table.AddCell(CreateCell("ShiftTime_To", 10, true));
                            inspectionForm3MustorRoll2Table.AddCell(CreateCell("Adult(Male)", 10, true));
                            inspectionForm3MustorRoll2Table.AddCell(CreateCell("Adult(Female)", 10, true));
                            inspectionForm3MustorRoll2Table.AddCell(CreateCell("Adolescent(Male)", 10, true));
                            inspectionForm3MustorRoll2Table.AddCell(CreateCell("Adolescent(Female)", 10, true));
                            inspectionForm3MustorRoll2Table.AddCell(CreateCell("Children(Male)", 10, true));
                            inspectionForm3MustorRoll2Table.AddCell(CreateCell("Children(Female)", 10, true));
                            foreach (var item in inspectionForm3MustorRollData.ResponseDataModel.Where(x => x.InspectionMusterRollType == InspectionMusterRollTypeEnum.ON_LICENSE).ToList())
                            {
                                inspectionForm3MustorRoll2Table.AddCell(CreateCell(item.ShiftTime_From, 10, true));
                                inspectionForm3MustorRoll2Table.AddCell(CreateCell(item.ShiftTime_To, 10, false));
                                inspectionForm3MustorRoll2Table.AddCell(CreateCell(item.Count_Adult_Male.ToString(), 10, true));
                                inspectionForm3MustorRoll2Table.AddCell(CreateCell(item.Count_Adult_FeMale.ToString(), 10, false));
                                inspectionForm3MustorRoll2Table.AddCell(CreateCell(item.Count_Adolescent_Male.ToString(), 10, true));
                                inspectionForm3MustorRoll2Table.AddCell(CreateCell(item.Count_Adolescent_FeMale.ToString(), 10, false));
                                inspectionForm3MustorRoll2Table.AddCell(CreateCell(item.Count_Children_Male.ToString(), 10, true));
                                inspectionForm3MustorRoll2Table.AddCell(CreateCell(item.Count_Children_FeMale.ToString(), 10, false));

                            }
                            documents.Add(new Chunk(" ")); // Add space
                            documents.Add(new Paragraph("Workers Details A. Details as per muster roll(On License)", FontFactory.GetFont("Times-Roman", 16, Font.BOLD)) { Alignment = Element.ALIGN_CENTER });
                            documents.Add(new Chunk(""));

                        }
                        //else
                        //{

                        //}
                        documents.Add(inspectionForm3MustorRoll2Table);

                        var inspectionFormHealthData = await _iInspectionService.GetIncpectionFormFactoryPartIIIHealth(inspectionRefId);
                        var pairsHealth = ModelOps.GetTitleAndValues<Inspection_Form_Factory_Part_III_Health>(inspectionFormHealthData.FormModel);

                        // Create a table for displaying inspection data
                        PdfPTable inspectionForm3HealthTable = new PdfPTable(2)
                        {
                            WidthPercentage = 100
                        };
                        foreach (var item in pairsHealth)
                        {
                            inspectionForm3HealthTable.AddCell(CreateCell(item.Title, 10, true));
                            inspectionForm3HealthTable.AddCell(CreateCell(item.Value, 10, false));

                            inspectionForm3HealthTable.AddCell(new PdfPCell(new Phrase(""))
                            {
                                FixedHeight = 10, // Adjust height for spacing
                                Border = PdfPCell.NO_BORDER // No border for spacing cell
                            });

                            // Add another empty cell to create a gap
                            inspectionForm3HealthTable.AddCell(new PdfPCell(new Phrase(""))
                            {
                                FixedHeight = 10,
                                Border = PdfPCell.NO_BORDER
                            });
                        }
                        documents.Add(new Chunk(" ")); // Add space
                        documents.Add(new Paragraph("Health", FontFactory.GetFont("Times-Roman", 16, Font.BOLD)) { Alignment = Element.ALIGN_CENTER });
                        documents.Add(new Chunk(""));
                        documents.Add(inspectionForm3HealthTable);

                        var inspectionFormSafetyData = await _iInspectionService.GetIncpectionFormFactoryPartIIISafety(inspectionRefId);
                        var pairsSafety = ModelOps.GetTitleAndValues<Inspection_Form_Factory_Part_III_Safety>(inspectionFormSafetyData.FormModel);

                        // Create a table for displaying inspection data
                        PdfPTable inspectionFormSafetyTable = new PdfPTable(2)
                        {
                            WidthPercentage = 100
                        };
                        foreach (var item in pairsSafety)
                        {
                            inspectionFormSafetyTable.AddCell(CreateCell(item.Title, 10, true));
                            inspectionFormSafetyTable.AddCell(CreateCell(item.Value, 10, false));

                            inspectionFormSafetyTable.AddCell(new PdfPCell(new Phrase(""))
                            {
                                FixedHeight = 10, // Adjust height for spacing
                                Border = PdfPCell.NO_BORDER // No border for spacing cell
                            });

                            // Add another empty cell to create a gap
                            inspectionFormSafetyTable.AddCell(new PdfPCell(new Phrase(""))
                            {
                                FixedHeight = 10,
                                Border = PdfPCell.NO_BORDER
                            });
                        }
                        documents.Add(new Chunk(" ")); // Add space
                        documents.Add(new Paragraph("Safety", FontFactory.GetFont("Times-Roman", 16, Font.BOLD)) { Alignment = Element.ALIGN_CENTER });
                        documents.Add(new Chunk(""));
                        documents.Add(inspectionFormSafetyTable);

                        var inspectionFormWelfareData = await _iInspectionService.GetIncpectionGetFormFactoryPartIIIWelfare(inspectionRefId);
                        var pairsWelfare = ModelOps.GetTitleAndValues<Inspection_Form_Factory_Part_III_Welfare>(inspectionFormWelfareData.FormModel);

                        // Create a table for displaying inspection data
                        PdfPTable inspectionFormWelfareTable = new PdfPTable(2)
                        {
                            WidthPercentage = 100
                        };
                        foreach (var item in pairsWelfare)
                        {
                            inspectionFormWelfareTable.AddCell(CreateCell(item.Title, 10, true));
                            inspectionFormWelfareTable.AddCell(CreateCell(item.Value, 10, false));

                            inspectionFormWelfareTable.AddCell(new PdfPCell(new Phrase(""))
                            {
                                FixedHeight = 10, // Adjust height for spacing
                                Border = PdfPCell.NO_BORDER // No border for spacing cell
                            });

                            // Add another empty cell to create a gap
                            inspectionFormWelfareTable.AddCell(new PdfPCell(new Phrase(""))
                            {
                                FixedHeight = 10,
                                Border = PdfPCell.NO_BORDER
                            });
                        }
                        documents.Add(new Chunk(" ")); // Add space
                        documents.Add(new Paragraph("Welfare", FontFactory.GetFont("Times-Roman", 16, Font.BOLD)) { Alignment = Element.ALIGN_CENTER });
                        documents.Add(new Chunk(""));
                        documents.Add(inspectionFormWelfareTable);

                        var inspectionFormGeneralData = await _iInspectionService.GetIncpectionGetFormFactoryPartIIIGeneral(inspectionRefId);
                        var pairsGeneral = ModelOps.GetTitleAndValues<Inspection_Form_Factory_Part_III_General>(inspectionFormGeneralData.FormModel);

                        // Create a table for displaying inspection data
                        PdfPTable inspectionFormGeneralTable = new PdfPTable(2)
                        {
                            WidthPercentage = 100
                        };
                        foreach (var item in pairsGeneral)
                        {
                            inspectionFormGeneralTable.AddCell(CreateCell(item.Title, 10, true));
                            inspectionFormGeneralTable.AddCell(CreateCell(item.Value, 10, false));

                            inspectionFormGeneralTable.AddCell(new PdfPCell(new Phrase(""))
                            {
                                FixedHeight = 10, // Adjust height for spacing
                                Border = PdfPCell.NO_BORDER // No border for spacing cell
                            });

                            // Add another empty cell to create a gap
                            inspectionFormGeneralTable.AddCell(new PdfPCell(new Phrase(""))
                            {
                                FixedHeight = 10,
                                Border = PdfPCell.NO_BORDER
                            });
                        }
                        documents.Add(new Chunk(" ")); // Add space
                        documents.Add(new Paragraph("General", FontFactory.GetFont("Times-Roman", 16, Font.BOLD)) { Alignment = Element.ALIGN_CENTER });
                        documents.Add(new Chunk(""));
                        documents.Add(inspectionFormGeneralTable);

                        var inspectionFormMajorAccidentData = await _iInspectionService.GetIncpectionGetFormFactoryPartIIIMajorAccidentHazard(inspectionRefId);
                        var pairsMajorAccident = ModelOps.GetTitleAndValues<Inspection_Form_Factory_Part_III_MajorAccidentHazard>(inspectionFormMajorAccidentData.FormModel);

                        // Create a table for displaying inspection data
                        PdfPTable inspectionFormMajorAccidentTable = new PdfPTable(2)
                        {
                            WidthPercentage = 100
                        };
                        foreach (var item in pairsMajorAccident)
                        {
                            inspectionFormMajorAccidentTable.AddCell(CreateCell(item.Title, 10, true));
                            inspectionFormMajorAccidentTable.AddCell(CreateCell(item.Value, 10, false));

                            inspectionFormMajorAccidentTable.AddCell(new PdfPCell(new Phrase(""))
                            {
                                FixedHeight = 10, // Adjust height for spacing
                                Border = PdfPCell.NO_BORDER // No border for spacing cell
                            });

                            // Add another empty cell to create a gap
                            inspectionFormMajorAccidentTable.AddCell(new PdfPCell(new Phrase(""))
                            {
                                FixedHeight = 10,
                                Border = PdfPCell.NO_BORDER
                            });
                        }
                        documents.Add(new Chunk(" ")); // Add space
                        documents.Add(new Paragraph("Major Accident Hazard", FontFactory.GetFont("Times-Roman", 16, Font.BOLD)) { Alignment = Element.ALIGN_CENTER });
                        documents.Add(new Chunk(""));
                        documents.Add(inspectionFormMajorAccidentTable);

                        var inspectionFormDangerousData = await _iInspectionService.GetForm_Factory_Part_III_Dangerousoperation(inspectionRefId);
                        var pairsDangerous = ModelOps.GetTitleAndValues<Inspection_Form_Factory_Part_III_DangerousOperation>(inspectionFormDangerousData.FormModel);


                        // Create a table for displaying inspection data
                        PdfPTable inspectionFormDangerousTable = new PdfPTable(2)
                        {
                            WidthPercentage = 100
                        };
                        foreach (var item in pairsDangerous)
                        {
                            inspectionFormDangerousTable.AddCell(CreateCell(item.Title, 10, true));
                            inspectionFormDangerousTable.AddCell(CreateCell(item.Value, 10, false));

                        }
                        documents.Add(new Chunk(" ")); // Add space
                        documents.Add(new Paragraph("Dangerous Operation", FontFactory.GetFont("Times-Roman", 16, Font.BOLD)) { Alignment = Element.ALIGN_CENTER });
                        documents.Add(new Chunk(""));
                        documents.Add(inspectionFormDangerousTable);
                    }
                    else
                    {

                        // Create a table for displaying inspection data
                        PdfPTable inspectionTable = new PdfPTable(2)
                        {
                            WidthPercentage = 100
                        };

                        inspectionTable.AddCell(CreateCell("Factory Name", 10, true));
                        inspectionTable.AddCell(CreateCell(establishmentDetails.ResponseDataModel.EstablishmentName, 10, false));

                        inspectionTable.AddCell(CreateCell("Factory Address", 10, true));
                        inspectionTable.AddCell(CreateCell(establishmentDetails.ResponseDataModel.EstablishmentAddress, 10, false));

                        inspectionTable.AddCell(CreateCell("Factory Status", 10, true));
                        inspectionTable.AddCell(CreateCell(EnumOps.GetEnumDescriptionName<InspectionFactoryExistenceTypeEnum>(Enum.GetName(typeof(InspectionFactoryExistenceTypeEnum), inspectionMasterData.InspectionSubmited_Factory_Wing_FactoryExistenceType)), 10, false));

                        inspectionTable.AddCell(CreateCell("Factory De-Registration Number", 10, true));
                        inspectionTable.AddCell(CreateCell(inspectionMasterData.FactoryDeRegistrationNo, 10, false));

                        inspectionTable.AddCell(CreateCell("Inspection Date", 10, true));
                        inspectionTable.AddCell(CreateCell(inspectionMasterData.InspectionDoneOn_Factory_Wing.ToString(), 10, false));

                        inspectionTable.AddCell(CreateCell("Remarks", 10, true));
                        inspectionTable.AddCell(CreateCell(inspectionMasterData.Remarks.ToString(), 10, false));

                        inspectionTable.AddCell(new PdfPCell(new Phrase(""))
                        {
                            FixedHeight = 10, // Adjust height for spacing
                            Border = PdfPCell.NO_BORDER // No border for spacing cell
                        });

                        // Add another empty cell to create a gap
                        inspectionTable.AddCell(new PdfPCell(new Phrase(""))
                        {
                            FixedHeight = 10,
                            Border = PdfPCell.NO_BORDER
                        });

                        documents.Add(new Chunk(" "));
                        documents.Add(inspectionTable);
                    }

                    var officerDetails = _context.Inspection_Master.Where(x => x.InspectionId == inspectionRefId).FirstOrDefault();
                    var officerUserRole = _context.Users.Where(x => x.Id == officerDetails.InspectionSubmited_Factory_Wing_UserId && x.IsEnabled == true).Include(x => x.UserRoles).ThenInclude(x => x.Role).FirstOrDefault();
                    var circleName = _context.FactoryCircles.Where(x => x.FactoryCircleId == officerDetails.FactoryCircleId && x.Version == 2);
                    //var userDetails = await _iAuthService.GetUserFullDetailByUserName(officerUserRole.UserName);
                    var userDetails = await _iAuthService.GetUserProfileByProfileId(officerDetails.InspectionSubmited_Factory_Wing_ProfileId);
                    string officerSigFilePath = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "OfficerSignatures")).Root + userDetails.Signature;

                    iTextSharp.text.Image signature = iTextSharp.text.Image.GetInstance(officerSigFilePath);
                    signature.ScaleToFit(120f, 120f);
                    signature.SetAbsolutePosition(450f, 150f); // Adjust position as needed
                    Font timesRoman = FontFactory.GetFont("Times-Roman", 9, Font.BOLD);

                    Paragraph SignSeal = new Paragraph()
                    {
                        Alignment = Element.ALIGN_RIGHT, // Justify the text
                        SpacingBefore = 10f, // Margin-top before the text
                        SpacingAfter = 10f, // Margin-bottom after the text
                        IndentationLeft = 10f,   // Padding left
                        IndentationRight = 10f   // Padding right
                    };
                    SignSeal.Add(new Chunk("Name: ", timesRoman));
                    SignSeal.Add(new Chunk(userDetails.FirstName + " " + userDetails.LastName, timesRoman));
                    SignSeal.Add(new Chunk(Chunk.NEWLINE + "Desig: ", timesRoman));
                    SignSeal.Add(new Chunk(officerUserRole.UserRoles.FirstOrDefault().Role.NormalizedName, timesRoman));
                    SignSeal.Add(new Chunk(Chunk.NEWLINE + "Circle Name: ", timesRoman));
                    SignSeal.Add(new Chunk(circleName.FirstOrDefault().FactoryCircleName, timesRoman));

                    //PdfPTable ptablerightSideDetail = new PdfPTable(1);
                    //ptablerightSideDetail.DefaultCell.Border = 0;
                    //ptablerightSideDetail.DefaultCell.Padding = 0;

                    //ptablerightSideDetail.AddCell(new PdfPCell(signature) { Border = 0, PaddingTop = 5, HorizontalAlignment = PdfPCell.ALIGN_RIGHT });

                    // Create a table to align signature and details
                    PdfPTable officerTable = new PdfPTable(2)
                    {
                        WidthPercentage = 100
                    };
                    documents.Add(SignSeal);
                    documents.Close();
                    PDFPageEvents pageeventsobj = new PDFPageEvents();
                    string filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Inspections", "Inspection_" + inspectionRefId + "_" + inspectionType + ".pdf");

                    string pdfFilePath = sfilepath;
                    Byte[] bytes = File.ReadAllBytes(sfilepath);
                    string file = System.Convert.ToBase64String(bytes);
                    genericFormModel.PdfContent = file;
                    genericFormModel.FileNo = "Inspection_" + inspectionRefId + "_" + inspectionType;
                }
            }
            catch (Exception ex)
            {
                genericFormModel.HasException = true;
                genericFormModel.Exceptions = ex.Message;
            }
            return genericFormModel;
        }

        private async Task<GeneratePdfServiceResultTemplate> Certificate_Inspection_LabourWing(Int64 inspectionRefId, int inspectionType)
        {
            GeneratePdfServiceResultTemplate genericFormModel = new GeneratePdfServiceResultTemplate();
            try
            {
                Guid guId = Guid.NewGuid();
                genericFormModel.PdfNameGUID = guId.ToString();
                var sfilepath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Inspections", "Inspection_" + inspectionRefId + "_" + inspectionType + ".pdf");

                // Ensure the file is deleted if it already exists
                if (File.Exists(sfilepath))
                {
                    File.Delete(sfilepath);
                }

                using (iTextSharp.text.Document documents = new iTextSharp.text.Document(iTextSharp.text.PageSize.A4, 30, 30, 50, 50))
                {
                    PdfWriter owriter = PdfWriter.GetInstance(documents, new FileStream(sfilepath, FileMode.Create));
                    owriter.SetEncryption(PdfWriter.STRENGTH128BITS, "", "", PdfWriter.AllowPrinting);
                    documents.Open();
                    var inspectionMasterData = _context.Inspection_Master.Where(x => x.InspectionId == inspectionRefId).FirstOrDefault();
                    var establishmentDetails = await _iInspectionService.GetInspectionEstablishmentBasicDetails(inspectionMasterData.LicenceNumber);
                    documents.Add(new Paragraph("Inspection Report(Labour-Wing)", FontFactory.GetFont("Times-Roman", 20, Font.BOLD)) { Alignment = Element.ALIGN_CENTER });
                    if (inspectionMasterData.InspectionSubmited_Labour_Wing_FactoryExistenceType == InspectionFactoryExistenceTypeEnum.OPEN)
                    {

                        // Load inspection data
                        var inspectionData = await _iInspectionService.GetInspectionFormLabourPartIGeneralDetail(inspectionRefId);
                        var pairs = ModelOps.GetTitleAndValues<Inspection_Form_Labour_Part_I_General>(inspectionData.FormModel);

                        // Create a table for displaying inspection data
                        PdfPTable inspectionTable = new PdfPTable(2)
                        {
                            WidthPercentage = 100
                        };
                        foreach (var item in pairs)
                        {
                            inspectionTable.AddCell(CreateCell(item.Title, 10, true));
                            inspectionTable.AddCell(CreateCell(item.Value, 10, false));
                        }
                        documents.Add(new Chunk(" "));
                        documents.Add(new Paragraph("Inspection Form Part-I", FontFactory.GetFont("Times-Roman", 16, Font.BOLD)) { Alignment = Element.ALIGN_CENTER });
                        documents.Add(new Chunk(""));
                        documents.Add(inspectionTable);



                        var inspectionForm2Data = await _iInspectionService.GetInspectionFormLabourPartIIFactoryDetail(inspectionRefId);
                        var pairs2 = ModelOps.GetTitleAndValues<Inspection_Form_Labour_Part_II_FactoryDetail>(inspectionForm2Data.FormModel);

                        // Create a table for displaying inspection data
                        PdfPTable inspectionForm2Table = new PdfPTable(2)
                        {
                            WidthPercentage = 100
                        };
                        foreach (var item in pairs2)
                        {
                            inspectionForm2Table.AddCell(CreateCell(item.Title, 10, true));
                            inspectionForm2Table.AddCell(CreateCell(item.Value, 10, false));
                        }
                        documents.Add(new Chunk(" ")); // Add space
                        documents.Add(new Paragraph("Inspection Form Part-II", FontFactory.GetFont("Times-Roman", 16, Font.BOLD)) { Alignment = Element.ALIGN_CENTER });
                        documents.Add(new Chunk(""));
                        documents.Add(inspectionForm2Table);

                        var inspectionForm3MustorRollData = await _iInspectionService.GetInspectionFormLabourPartIIIMusterRoll(inspectionRefId);

                        PdfPTable inspectionForm3MustorRoll1Table = new PdfPTable(8)
                        {
                            WidthPercentage = 100

                        };
                        PdfPTable inspectionForm3MustorRoll2Table = new PdfPTable(8)
                        {
                            WidthPercentage = 100

                        };
                        if (inspectionForm3MustorRollData.ResponseDataModel.Where(x => x.InspectionMusterRollType == InspectionMusterRollTypeEnum.On_INSPECTION_TIME).Count() > 0)
                        {
                            inspectionForm3MustorRoll1Table.AddCell(CreateCell("ShiftTime_From", 10, true));
                            inspectionForm3MustorRoll1Table.AddCell(CreateCell("ShiftTime_To", 10, true));
                            inspectionForm3MustorRoll1Table.AddCell(CreateCell("Adult(Male)", 10, true));
                            inspectionForm3MustorRoll1Table.AddCell(CreateCell("Adult(Female)", 10, true));
                            inspectionForm3MustorRoll1Table.AddCell(CreateCell("Adolescent(Male)", 10, true));
                            inspectionForm3MustorRoll1Table.AddCell(CreateCell("Adolescent(Female)", 10, true));
                            inspectionForm3MustorRoll1Table.AddCell(CreateCell("Children(Male)", 10, true));
                            inspectionForm3MustorRoll1Table.AddCell(CreateCell("Children(Female)", 10, true));

                            foreach (var item in inspectionForm3MustorRollData.ResponseDataModel.Where(x => x.InspectionMusterRollType == InspectionMusterRollTypeEnum.On_INSPECTION_TIME).ToList())
                            {
                                inspectionForm3MustorRoll1Table.AddCell(CreateCell(item.ShiftTime_From, 10, false));
                                inspectionForm3MustorRoll1Table.AddCell(CreateCell(item.ShiftTime_To, 10, false));
                                inspectionForm3MustorRoll1Table.AddCell(CreateCell(item.Count_Adult_Male.ToString(), 10, false));
                                inspectionForm3MustorRoll1Table.AddCell(CreateCell(item.Count_Adult_FeMale.ToString(), 10, false));
                                inspectionForm3MustorRoll1Table.AddCell(CreateCell(item.Count_Adolescent_Male.ToString(), 10, false));
                                inspectionForm3MustorRoll1Table.AddCell(CreateCell(item.Count_Adolescent_FeMale.ToString(), 10, false));
                                inspectionForm3MustorRoll1Table.AddCell(CreateCell(item.Count_Children_Male.ToString(), 10, false));
                                inspectionForm3MustorRoll1Table.AddCell(CreateCell(item.Count_Children_FeMale.ToString(), 10, false));

                            }
                            documents.Add(new Chunk(" ")); // Add space
                            documents.Add(new Paragraph("Workers Details A. Details as per muster roll (On Inspection Time)", FontFactory.GetFont("Times-Roman", 16, Font.BOLD)) { Alignment = Element.ALIGN_CENTER });
                            documents.Add(new Chunk(""));

                        }
                        documents.Add(inspectionForm3MustorRoll1Table);
                        if (inspectionForm3MustorRollData.ResponseDataModel.Where(x => x.InspectionMusterRollType == InspectionMusterRollTypeEnum.ON_LICENSE).Count() > 0)
                        {
                            inspectionForm3MustorRoll2Table.AddCell(CreateCell("ShiftTime_From", 10, true));
                            inspectionForm3MustorRoll2Table.AddCell(CreateCell("ShiftTime_To", 10, true));
                            inspectionForm3MustorRoll2Table.AddCell(CreateCell("Adult(Male)", 10, true));
                            inspectionForm3MustorRoll2Table.AddCell(CreateCell("Adult(Female)", 10, true));
                            inspectionForm3MustorRoll2Table.AddCell(CreateCell("Adolescent(Male)", 10, true));
                            inspectionForm3MustorRoll2Table.AddCell(CreateCell("Adolescent(Female)", 10, true));
                            inspectionForm3MustorRoll2Table.AddCell(CreateCell("Children(Male)", 10, true));
                            inspectionForm3MustorRoll2Table.AddCell(CreateCell("Children(Female)", 10, true));
                            foreach (var item in inspectionForm3MustorRollData.ResponseDataModel.Where(x => x.InspectionMusterRollType == InspectionMusterRollTypeEnum.ON_LICENSE).ToList())
                            {
                                inspectionForm3MustorRoll2Table.AddCell(CreateCell(item.ShiftTime_From, 10, true));
                                inspectionForm3MustorRoll2Table.AddCell(CreateCell(item.ShiftTime_To, 10, false));
                                inspectionForm3MustorRoll2Table.AddCell(CreateCell(item.Count_Adult_Male.ToString(), 10, true));
                                inspectionForm3MustorRoll2Table.AddCell(CreateCell(item.Count_Adult_FeMale.ToString(), 10, false));
                                inspectionForm3MustorRoll2Table.AddCell(CreateCell(item.Count_Adolescent_Male.ToString(), 10, true));
                                inspectionForm3MustorRoll2Table.AddCell(CreateCell(item.Count_Adolescent_FeMale.ToString(), 10, false));
                                inspectionForm3MustorRoll2Table.AddCell(CreateCell(item.Count_Children_Male.ToString(), 10, true));
                                inspectionForm3MustorRoll2Table.AddCell(CreateCell(item.Count_Children_FeMale.ToString(), 10, false));

                            }
                            documents.Add(new Chunk(" ")); // Add space
                            documents.Add(new Paragraph("Workers Details A. Details as per muster roll(On License)", FontFactory.GetFont("Times-Roman", 16, Font.BOLD)) { Alignment = Element.ALIGN_CENTER });
                            documents.Add(new Chunk(""));

                        }
                        documents.Add(inspectionForm3MustorRoll2Table);

                        var inspectionFormEqualEnumerationAct = await _iInspectionService.GetInspectionFormLabourPartIIIEqualEnumerationAct(inspectionRefId);
                        var pairsEnumerationAct = ModelOps.GetTitleAndValues<Inspection_Form_Labour_Part_III_EqualEnumerationAct>(inspectionFormEqualEnumerationAct.FormModel);

                        // Create a table for displaying inspection data
                        PdfPTable inspectionEqualEnumerationAct = new PdfPTable(2)
                        {
                            WidthPercentage = 100
                        };
                        foreach (var item in pairsEnumerationAct)
                        {
                            inspectionEqualEnumerationAct.AddCell(CreateCell(item.Title, 10, true));
                            inspectionEqualEnumerationAct.AddCell(CreateCell(item.Value, 10, false));
                        }
                        documents.Add(new Chunk(" ")); // Add space
                        documents.Add(new Paragraph("Equal Enumeration Act", FontFactory.GetFont("Times-Roman", 16, Font.BOLD)) { Alignment = Element.ALIGN_CENTER });
                        documents.Add(new Chunk(""));
                        documents.Add(inspectionEqualEnumerationAct);

                        var inspectionFormLabourIIIMinimumWageAct = await _iInspectionService.GetInspectionFormLabourPartIIIMinimumWagesAct(inspectionRefId);
                        var pairsMinimumWageAct = ModelOps.GetTitleAndValues<Inspection_Form_Labour_III_MinimumWageAct>(inspectionFormLabourIIIMinimumWageAct.FormModel);

                        // Create a table for displaying inspection data
                        PdfPTable inspectionMinimumWageAct = new PdfPTable(2)
                        {
                            WidthPercentage = 100
                        };
                        foreach (var item in pairsMinimumWageAct)
                        {
                            inspectionMinimumWageAct.AddCell(CreateCell(item.Title, 10, true));
                            inspectionMinimumWageAct.AddCell(CreateCell(item.Value, 10, false));
                        }
                        documents.Add(new Chunk(" ")); // Add space
                        documents.Add(new Paragraph("Minimum Wage Act", FontFactory.GetFont("Times-Roman", 16, Font.BOLD)) { Alignment = Element.ALIGN_CENTER });
                        documents.Add(new Chunk(""));
                        documents.Add(inspectionMinimumWageAct);

                        var inspectionFormLabourIIIPaymentWagesAct = await _iInspectionService.GetInspectionFormLabourPartIIIPaymentWagesAct(inspectionRefId);
                        var pairsPaymentWagesAct = ModelOps.GetTitleAndValues<Inspection_Form_Labour_III_PaymentWagesAct>(inspectionFormLabourIIIPaymentWagesAct.FormModel);

                        // Create a table for displaying inspection data
                        PdfPTable inspectionPaymentWagesAct = new PdfPTable(2)
                        {
                            WidthPercentage = 100
                        };
                        foreach (var item in pairsPaymentWagesAct)
                        {
                            inspectionPaymentWagesAct.AddCell(CreateCell(item.Title, 10, true));
                            inspectionPaymentWagesAct.AddCell(CreateCell(item.Value, 10, false));
                        }
                        documents.Add(new Chunk(" ")); // Add space
                        documents.Add(new Paragraph("Payment Wages Act", FontFactory.GetFont("Times-Roman", 16, Font.BOLD)) { Alignment = Element.ALIGN_CENTER });
                        documents.Add(new Chunk(""));
                        documents.Add(inspectionPaymentWagesAct);

                        var inspectionFormLabourIIIPaymentBonusActStatutoryReport = await _iInspectionService.GetInspectionFormLabourPartIIIPaymentBonusAct_StatutoryReport(inspectionRefId);
                        var pairsPaymentBonusActStatutoryReport = ModelOps.GetTitleAndValues<Inspection_Form_Labour_III_PaymentBonusAct_StatutoryReport>(inspectionFormLabourIIIPaymentBonusActStatutoryReport.FormModel);

                        // Create a table for displaying inspection data
                        PdfPTable inspectionPaymentBonusActStatutoryReport = new PdfPTable(2)
                        {
                            WidthPercentage = 100
                        };
                        foreach (var item in pairsPaymentBonusActStatutoryReport)
                        {
                            inspectionPaymentBonusActStatutoryReport.AddCell(CreateCell(item.Title, 10, true));
                            inspectionPaymentBonusActStatutoryReport.AddCell(CreateCell(item.Value, 10, false));
                        }
                        documents.Add(new Chunk(" ")); // Add space
                        documents.Add(new Paragraph("Payment BonusAct Statutory Report", FontFactory.GetFont("Times-Roman", 16, Font.BOLD)) { Alignment = Element.ALIGN_CENTER });
                        documents.Add(new Chunk(""));
                        documents.Add(inspectionPaymentBonusActStatutoryReport);

                        var inspectionFormLabourIIIChildAndAdolescentLabourAct = await _iInspectionService.GetInspectionFormLabourPartIIIChildAndAdolescentLabourAct(inspectionRefId);
                        var pairsPaymentChildAndAdolescentLabourAct = ModelOps.GetTitleAndValues<Inspection_Form_Labour_III_ChildAndAdolescentLabourAct>(inspectionFormLabourIIIChildAndAdolescentLabourAct.FormModel);

                        // Create a table for displaying inspection data
                        PdfPTable inspectionChildAndAdolescentLabourAct = new PdfPTable(2)
                        {
                            WidthPercentage = 100
                        };
                        foreach (var item in pairsPaymentChildAndAdolescentLabourAct)
                        {
                            inspectionChildAndAdolescentLabourAct.AddCell(CreateCell(item.Title, 10, true));
                            inspectionChildAndAdolescentLabourAct.AddCell(CreateCell(item.Value, 10, false));
                        }
                        documents.Add(new Chunk(" ")); // Add space
                        documents.Add(new Paragraph("Child And Adolescent Labour Act", FontFactory.GetFont("Times-Roman", 16, Font.BOLD)) { Alignment = Element.ALIGN_CENTER });
                        documents.Add(new Chunk(""));
                        documents.Add(inspectionChildAndAdolescentLabourAct);

                        var inspectionFormLabourIIINationalAndFestivalHolidays = await _iInspectionService.GetInspectionFormLabourPartIIINationalAndFestivalHolidays(inspectionRefId);
                        var pairsNationalAndFestivalHolidays = ModelOps.GetTitleAndValues<Inspection_Form_Labour_III_NationalAndFestivalHolidays>(inspectionFormLabourIIINationalAndFestivalHolidays.FormModel);

                        // Create a table for displaying inspection data
                        PdfPTable inspectionNationalAndFestivalHolidays = new PdfPTable(2)
                        {
                            WidthPercentage = 100
                        };
                        foreach (var item in pairsNationalAndFestivalHolidays)
                        {
                            inspectionNationalAndFestivalHolidays.AddCell(CreateCell(item.Title, 10, true));
                            inspectionNationalAndFestivalHolidays.AddCell(CreateCell(item.Value, 10, false));
                        }
                        documents.Add(new Chunk(" ")); // Add space
                        documents.Add(new Paragraph("National And Festival Holidays", FontFactory.GetFont("Times-Roman", 16, Font.BOLD)) { Alignment = Element.ALIGN_CENTER });
                        documents.Add(new Chunk(""));
                        documents.Add(inspectionNationalAndFestivalHolidays);

                        var inspectionFormLabourIIIMaternityBenefitAct = await _iInspectionService.GetInspectionFormLabourPartIIIMaternityBenefitAct(inspectionRefId);
                        var pairsMaternityBenefitAct = ModelOps.GetTitleAndValues<Inspection_Form_Labour_III_MaternityBenefitAct>(inspectionFormLabourIIIMaternityBenefitAct.FormModel);

                        // Create a table for displaying inspection data
                        PdfPTable inspectionMaternityBenefitAct = new PdfPTable(2)
                        {
                            WidthPercentage = 100
                        };
                        foreach (var item in pairsMaternityBenefitAct)
                        {
                            inspectionMaternityBenefitAct.AddCell(CreateCell(item.Title, 10, true));
                            inspectionMaternityBenefitAct.AddCell(CreateCell(item.Value, 10, false));
                        }
                        documents.Add(new Chunk(" ")); // Add space
                        documents.Add(new Paragraph("Maternity-Benefit Act", FontFactory.GetFont("Times-Roman", 16, Font.BOLD)) { Alignment = Element.ALIGN_CENTER });
                        documents.Add(new Chunk(""));
                        documents.Add(inspectionMaternityBenefitAct);

                        var inspectionFormLabourIIIContractLabourAct = await _iInspectionService.GetInspectionFormLabourPartIIIContractLabourAct(inspectionRefId);
                        var pairsContractLabourAct = ModelOps.GetTitleAndValues<Inspection_Form_Labour_III_ContractLabourAct>(inspectionFormLabourIIIContractLabourAct.FormModel);

                        // Create a table for displaying inspection data
                        PdfPTable inspectionContractLabourAct = new PdfPTable(2)
                        {
                            WidthPercentage = 100
                        };
                        foreach (var item in pairsContractLabourAct)
                        {
                            inspectionContractLabourAct.AddCell(CreateCell(item.Title, 10, true));
                            inspectionContractLabourAct.AddCell(CreateCell(item.Value, 10, false));
                        }
                        documents.Add(new Chunk(" ")); // Add space
                        documents.Add(new Paragraph("Contract-Labour Act", FontFactory.GetFont("Times-Roman", 16, Font.BOLD)) { Alignment = Element.ALIGN_CENTER });
                        documents.Add(new Chunk(""));
                        documents.Add(inspectionContractLabourAct);

                        var inspectionFormLabourIIIInterStateMigrantWorkmenAct = await _iInspectionService.GetInspectionFormLabourPartIIIInterStateMigrantWorkmenAct(inspectionRefId);
                        var pairsInterStateMigrantWorkmenAct = ModelOps.GetTitleAndValues<Inspection_Form_Labour_III_InterStateMigrantWorkmenAct>(inspectionFormLabourIIIInterStateMigrantWorkmenAct.FormModel);

                        // Create a table for displaying inspection data
                        PdfPTable inspectionInterStateMigrantWorkmenAct = new PdfPTable(2)
                        {
                            WidthPercentage = 100
                        };
                        foreach (var item in pairsInterStateMigrantWorkmenAct)
                        {
                            inspectionInterStateMigrantWorkmenAct.AddCell(CreateCell(item.Title, 10, true));
                            inspectionInterStateMigrantWorkmenAct.AddCell(CreateCell(item.Value, 10, false));
                        }
                        documents.Add(new Chunk(" ")); // Add space
                        documents.Add(new Paragraph("InterState Migrant Workmen Act", FontFactory.GetFont("Times-Roman", 16, Font.BOLD)) { Alignment = Element.ALIGN_CENTER });
                        documents.Add(new Chunk(""));
                        documents.Add(inspectionInterStateMigrantWorkmenAct);

                        var inspectionFormLabourIIILabourWelfareFundAct = await _iInspectionService.GetInspectionFormLabourPartIIILabourWelfareFund_Act(inspectionRefId);
                        var pairsLabourWelfareFundAct = ModelOps.GetTitleAndValues<Inspection_Form_Labour_III_LabourWelfareFund_Act>(inspectionFormLabourIIILabourWelfareFundAct.FormModel);

                        // Create a table for displaying inspection data
                        PdfPTable inspectionLabourWelfareFundAct = new PdfPTable(2)
                        {
                            WidthPercentage = 100
                        };
                        foreach (var item in pairsLabourWelfareFundAct)
                        {
                            inspectionLabourWelfareFundAct.AddCell(CreateCell(item.Title, 10, true));
                            inspectionLabourWelfareFundAct.AddCell(CreateCell(item.Value, 10, false));
                        }
                        documents.Add(new Chunk(" ")); // Add space
                        documents.Add(new Paragraph("Labour Welfare Fund Act", FontFactory.GetFont("Times-Roman", 16, Font.BOLD)) { Alignment = Element.ALIGN_CENTER });
                        documents.Add(new Chunk(""));
                        documents.Add(inspectionLabourWelfareFundAct);

                        var inspectionFormLabourIIIGratuityAct = await _iInspectionService.GetInspectionFormLabourPartIIIGratuityAct(inspectionRefId);
                        var pairsGratuityAct = ModelOps.GetTitleAndValues<Inspection_Form_Labour_III_GratuityAct>(inspectionFormLabourIIIGratuityAct.FormModel);

                        // Create a table for displaying inspection data
                        PdfPTable inspectionGratuityAct = new PdfPTable(2)
                        {
                            WidthPercentage = 100
                        };
                        foreach (var item in pairsGratuityAct)
                        {
                            inspectionGratuityAct.AddCell(CreateCell(item.Title, 10, true));
                            inspectionGratuityAct.AddCell(CreateCell(item.Value, 10, false));
                        }
                        documents.Add(new Chunk(" ")); // Add space
                        documents.Add(new Paragraph("Gratuity Act", FontFactory.GetFont("Times-Roman", 16, Font.BOLD)) { Alignment = Element.ALIGN_CENTER });
                        documents.Add(new Chunk(""));
                        documents.Add(inspectionGratuityAct);

                        var inspectionFormLabourIIIIndustrialEmploymentAct = await _iInspectionService.GetInspectionFormLabourPartIIIIndustrialEmploymentAct(inspectionRefId);
                        var pairsIndustrialEmploymentAct = ModelOps.GetTitleAndValues<Inspection_Form_Labour_III_IndustrialEmploymentAct>(inspectionFormLabourIIIIndustrialEmploymentAct.FormModel);

                        // Create a table for displaying inspection data
                        PdfPTable inspectionIndustrialEmploymentAct = new PdfPTable(2)
                        {
                            WidthPercentage = 100
                        };
                        foreach (var item in pairsIndustrialEmploymentAct)
                        {
                            inspectionIndustrialEmploymentAct.AddCell(CreateCell(item.Title, 10, true));
                            inspectionIndustrialEmploymentAct.AddCell(CreateCell(item.Value, 10, false));
                        }
                        documents.Add(new Chunk(" ")); // Add space
                        documents.Add(new Paragraph("Industrial Employment Act", FontFactory.GetFont("Times-Roman", 16, Font.BOLD)) { Alignment = Element.ALIGN_CENTER });
                        documents.Add(new Chunk(""));
                        documents.Add(inspectionIndustrialEmploymentAct);

                        var inspectionFormLabourIIIBOCWAct = await _iInspectionService.GetInspectionFormLabourPartIIIBOCW_Act(inspectionRefId);
                        var pairsBOCWAct = ModelOps.GetTitleAndValues<Inspection_Form_Labour_III_BOCW_Act>(inspectionFormLabourIIIBOCWAct.FormModel);

                        // Create a table for displaying inspection data
                        PdfPTable inspectionBOCWAct = new PdfPTable(2)
                        {
                            WidthPercentage = 100
                        };
                        foreach (var item in pairsBOCWAct)
                        {
                            inspectionBOCWAct.AddCell(CreateCell(item.Title, 10, true));
                            inspectionBOCWAct.AddCell(CreateCell(item.Value, 10, false));
                        }
                        documents.Add(new Chunk(" ")); // Add space
                        documents.Add(new Paragraph("BOCW Act", FontFactory.GetFont("Times-Roman", 16, Font.BOLD)) { Alignment = Element.ALIGN_CENTER });
                        documents.Add(new Chunk(""));
                        documents.Add(inspectionBOCWAct);


                        if (inspectionMasterData.InspectionSubmitOn_Labour_Wing < DateTime.Parse("2024-10-10"))
                        {
                            var inspectionFormLabourIIIShopAct = await _iInspectionService.GetInspectionFormLabourPartIIIShopAct(inspectionRefId);
                            var pairsShopAct = ModelOps.GetTitleAndValues<Inspection_Form_Labour_III_ShopAct>(inspectionFormLabourIIIShopAct.FormModel);

                            // Create a table for displaying inspection data
                            PdfPTable inspectionShopAct = new PdfPTable(2)
                            {
                                WidthPercentage = 100
                            };
                            foreach (var item in pairsShopAct)
                            {
                                inspectionShopAct.AddCell(CreateCell(item.Title, 10, true));
                                inspectionShopAct.AddCell(CreateCell(item.Value, 10, false));

                            }
                            documents.Add(new Chunk(" ")); // Add space
                            documents.Add(new Paragraph("Shop Act", FontFactory.GetFont("Times-Roman", 16, Font.BOLD)) { Alignment = Element.ALIGN_CENTER });
                            documents.Add(new Chunk(""));
                            documents.Add(inspectionShopAct);
                        }

                        var inspectionFormLabourIIIObservations = await _iInspectionService.GetInspectionFormLabourPartIIIObservations(inspectionRefId);
                        var pairsObservations = ModelOps.GetTitleAndValues<Inspection_Form_Labour_III_Observations>(inspectionFormLabourIIIObservations.FormModel);

                        // Create a table for displaying inspection data
                        PdfPTable inspectionObservations = new PdfPTable(2)
                        {
                            WidthPercentage = 100
                        };
                        foreach (var item in pairsObservations)
                        {
                            inspectionObservations.AddCell(CreateCell(item.Title, 10, true));
                            inspectionObservations.AddCell(CreateCell(item.Value, 10, false));
                        }
                        documents.Add(new Chunk(" ")); // Add space
                        documents.Add(new Paragraph("Observations", FontFactory.GetFont("Times-Roman", 16, Font.BOLD)) { Alignment = Element.ALIGN_CENTER });
                        documents.Add(new Chunk(""));
                        documents.Add(inspectionObservations);
                    }
                    else
                    {

                        // Create a table for displaying inspection data
                        PdfPTable inspectionTable = new PdfPTable(2)
                        {
                            WidthPercentage = 100
                        };

                        inspectionTable.AddCell(CreateCell("Factory Name", 10, true));
                        inspectionTable.AddCell(CreateCell(establishmentDetails.ResponseDataModel.EstablishmentName, 10, false));

                        inspectionTable.AddCell(CreateCell("Factory Address", 10, true));
                        inspectionTable.AddCell(CreateCell(establishmentDetails.ResponseDataModel.EstablishmentAddress, 10, false));

                        inspectionTable.AddCell(CreateCell("Factory Status", 10, true));
                        inspectionTable.AddCell(CreateCell(EnumOps.GetEnumDescriptionName<InspectionFactoryExistenceTypeEnum>(Enum.GetName(typeof(InspectionFactoryExistenceTypeEnum), inspectionMasterData.InspectionSubmited_Labour_Wing_FactoryExistenceType)), 10, false));

                        inspectionTable.AddCell(CreateCell("Factory De-Registration Number", 10, true));
                        inspectionTable.AddCell(CreateCell(inspectionMasterData.FactoryDeRegistrationNo, 10, false));

                        inspectionTable.AddCell(CreateCell("Inspection Date", 10, true));
                        inspectionTable.AddCell(CreateCell(inspectionMasterData.InspectionDoneOn_Labour_Wing.ToString(), 10, false));

                        inspectionTable.AddCell(CreateCell("Remarks", 10, true));
                        inspectionTable.AddCell(CreateCell(inspectionMasterData.Remarks.ToString(), 10, false));

                        inspectionTable.AddCell(new PdfPCell(new Phrase(""))
                        {
                            FixedHeight = 10, // Adjust height for spacing
                            Border = PdfPCell.NO_BORDER // No border for spacing cell
                        });

                        // Add another empty cell to create a gap
                        inspectionTable.AddCell(new PdfPCell(new Phrase(""))
                        {
                            FixedHeight = 10,
                            Border = PdfPCell.NO_BORDER
                        });

                        documents.Add(new Chunk(" "));
                        documents.Add(inspectionTable);
                    }

                    var officerDetails = _context.Inspection_Master.Where(x => x.InspectionId == inspectionRefId).FirstOrDefault();
                    var officerUserRole = _context.Users.Where(x => x.Id == officerDetails.InspectionSubmited_Labour_Wing_UserId && x.IsEnabled == true).Include(x => x.UserRoles).ThenInclude(x => x.Role).FirstOrDefault();
                    var circleName = _context.LabourCircles.Where(x => x.LabourCircleId == officerDetails.LabourCircleRefId && x.Version == 2);
                    //var userDetails = await _iAuthService.GetUserFullDetailByUserName(officerUserRole.UserName);
                    var userDetails = await _iAuthService.GetUserProfileByProfileId(officerDetails.InspectionSubmited_Labour_Wing_ProfileId);
                    //string officerSigFilePath = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "OfficerSignatures")).Root + userDetails.ListData[0].Signature;

                    //iTextSharp.text.Image signature = iTextSharp.text.Image.GetInstance(officerSigFilePath);
                    //signature.ScaleToFit(120f, 120f);
                    //signature.SetAbsolutePosition(450f, 150f); // Adjust position as needed

                    PdfPTable officerDetailsTable = new PdfPTable(2)
                    {
                        WidthPercentage = 100
                    };
                    Font timesRoman = FontFactory.GetFont("Times-Roman", 9, Font.BOLD);
                    Paragraph SignSeal = new Paragraph()
                    {
                        Alignment = Element.ALIGN_RIGHT, // Justify the text
                        SpacingBefore = 10f, // Margin-top before the text
                        SpacingAfter = 10f, // Margin-bottom after the text
                        IndentationLeft = 10f,   // Padding left
                        IndentationRight = 10f   // Padding right
                    };
                    SignSeal.Add(new Chunk("Name: ", timesRoman));
                    SignSeal.Add(new Chunk(userDetails.FirstName + " " + userDetails.LastName, timesRoman));
                    SignSeal.Add(new Chunk(Chunk.NEWLINE + "Desig: ", timesRoman));
                    SignSeal.Add(new Chunk(officerUserRole.UserRoles.FirstOrDefault().Role.NormalizedName, timesRoman));
                    SignSeal.Add(new Chunk(Chunk.NEWLINE + "Circle Name: ", timesRoman));
                    SignSeal.Add(new Chunk(circleName.FirstOrDefault().LabourCircleName, timesRoman));


                    //PdfPTable ptablerightSideDetail = new PdfPTable(1);
                    //ptablerightSideDetail.DefaultCell.Border = 0;
                    //ptablerightSideDetail.DefaultCell.Padding = 0;

                    //ptablerightSideDetail.AddCell(new PdfPCell(signature) { Border = 0, PaddingTop = 5, HorizontalAlignment = PdfPCell.ALIGN_RIGHT });

                    // Create a table to align signature and details
                    PdfPTable officerTable = new PdfPTable(2)
                    {
                        WidthPercentage = 100
                    };
                    //documents.Add(ptablerightSideDetail);
                    documents.Add(SignSeal);
                    documents.Close();
                    // Construct the file path
                    string filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Inspections", "Inspection_" + inspectionRefId + "_" + inspectionType + ".pdf");

                    string pdfFilePath = filePath;
                    Byte[] bytes = File.ReadAllBytes(filePath);
                    string file = System.Convert.ToBase64String(bytes);
                    genericFormModel.PdfContent = file;
                    genericFormModel.FileNo = "Inspection_" + inspectionRefId + "_" + inspectionType;
                }
            }
            catch (Exception ex)
            {
                genericFormModel.HasException = true;
                genericFormModel.Exceptions = ex.Message;
            }
            return genericFormModel;
        }

        public async Task<GeneratePdfServiceResultTemplate> Inspection_ViolationReport_FactoryWing(Int64 inspectionRefId, int inspectionType)
        {
            GeneratePdfServiceResultTemplate genericFormModel = new GeneratePdfServiceResultTemplate();
            try
            {
                Guid guId = Guid.NewGuid();
                genericFormModel.PdfNameGUID = guId.ToString();
                var sfilepath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Inspections", "Violation_" + inspectionRefId + "_" + inspectionType + ".pdf");
                Font timesRoman = FontFactory.GetFont("Times-Roman", 9, Font.BOLD);

                using (iTextSharp.text.Document documents = new iTextSharp.text.Document(iTextSharp.text.PageSize.A4, 30, 30, 50, 50))
                {
                    PdfWriter owriter = PdfWriter.GetInstance(documents, new FileStream(sfilepath, FileMode.Create));
                    owriter.SetEncryption(PdfWriter.STRENGTH128BITS, "", "", PdfWriter.AllowPrinting);


                    documents.Open();

                    var inspectionMasterData = _context.Inspection_Master.Where(x => x.InspectionId == inspectionRefId).FirstOrDefault();
                    var establishmentDetails = await _iInspectionService.GetInspectionEstablishmentBasicDetails(inspectionMasterData.LicenceNumber);
                    var titleFont = FontFactory.GetFont("Times-Roman", 20, Font.BOLD, new BaseColor(0, 51, 102)); // Dark Blue color
                    var titleParagraph = new Paragraph("Violation Report(Factory-Wing)", titleFont)
                    {
                        Alignment = Element.ALIGN_CENTER
                    };

                    titleParagraph.SpacingBefore = 20;  // Adjust as needed

                    titleParagraph.SpacingAfter = 10;  // Adjust as needed
                    documents.Add(titleParagraph);

                    if (inspectionMasterData.InspectionSubmited_Factory_Wing_FactoryExistenceType == InspectionFactoryExistenceTypeEnum.OPEN)
                    {

                        var inspectionData = await _iInspectionService.GetIncpectionFormFactoryPartIGeneralDetail(inspectionRefId);
                        var pairs = ModelOps.GetTitleAndValues<Inspection_Form_Factory_Part_I_General>(inspectionData.FormModel);

                        PdfPTable inspectionFactoryTable = new PdfPTable(2)
                        {
                            WidthPercentage = 100
                        };

                        float[] columnWidths = new float[] { 1f, 2f };
                        inspectionFactoryTable.SetWidths(columnWidths);

                        PdfPCell headerCell = new PdfPCell(new Phrase("General Details", FontFactory.GetFont("Helvetica", 14, Font.BOLD)))
                        {
                            Colspan = 2,
                            HorizontalAlignment = Element.ALIGN_CENTER,
                            Border = Rectangle.BOTTOM_BORDER,
                            BackgroundColor = new BaseColor(90, 90, 90),
                            PaddingTop = 5,  // Adjust top padding to reduce space
                            PaddingBottom = 5,  // Adjust bottom padding to reduce space
                            Phrase = new Phrase("General Details", FontFactory.GetFont("Helvetica", 14, Font.BOLD, BaseColor.WHITE)),
                            FixedHeight = 30  // Set a fixed height for the header cell (adjust as needed)
                        };

                        inspectionFactoryTable.AddCell(headerCell);

                        bool isEvenRow = true;
                        foreach (var item in pairs)
                        {
                            PdfPCell titleCell = CreateCell(item.Title, 10, true);
                            titleCell.Border = Rectangle.BOX;
                            titleCell.Padding = 10;
                            titleCell.BackgroundColor = isEvenRow ? new BaseColor(245, 245, 245) : BaseColor.WHITE;
                            inspectionFactoryTable.AddCell(titleCell);

                            PdfPCell valueCell = CreateCell(item.Value, 10, false);
                            valueCell.Border = Rectangle.BOX;
                            valueCell.Padding = 10;
                            valueCell.BackgroundColor = isEvenRow ? new BaseColor(245, 245, 245) : BaseColor.WHITE;
                            inspectionFactoryTable.AddCell(valueCell);

                            isEvenRow = !isEvenRow;
                        }

                        documents.Add(new Chunk(" "));
                        documents.Add(inspectionFactoryTable);

                        var inspectionForm2Data = await _iInspectionService.GetIncpectionFormFactoryPartIIFactoryDetail(inspectionRefId);
                        var pairs2 = ModelOps.GetTitleAndValues<Inspection_Form_Factory_Part_II_FactoryDetail>(inspectionForm2Data.FormModel);

                        PdfPTable inspectionForm2Table = new PdfPTable(2)
                        {
                            WidthPercentage = 100
                        };

                        float[] columnWidths2 = new float[] { 1f, 2f };
                        inspectionForm2Table.SetWidths(columnWidths2);

                        PdfPCell headerCell2 = new PdfPCell(new Phrase("Factory Details", FontFactory.GetFont("Helvetica", 14, Font.BOLD)))
                        {
                            Colspan = 2,
                            HorizontalAlignment = Element.ALIGN_CENTER,
                            Border = Rectangle.BOTTOM_BORDER,
                            BackgroundColor = new BaseColor(90, 90, 90),
                            PaddingTop = 5,  // Adjust top padding to reduce space
                            PaddingBottom = 5,  // Adjust bottom padding to reduce space
                            Phrase = new Phrase("Factory Details", FontFactory.GetFont("Helvetica", 14, Font.BOLD, BaseColor.WHITE)),
                            FixedHeight = 30  // Set a fixed height for the header cell (adjust as needed)
                        };

                        inspectionForm2Table.AddCell(headerCell2);

                        bool isEvenRow2 = true;
                        foreach (var item in pairs2)
                        {
                            PdfPCell titleCell2 = CreateCell(item.Title, 10, true);
                            titleCell2.Border = Rectangle.BOX;
                            titleCell2.Padding = 10;
                            titleCell2.BackgroundColor = isEvenRow2 ? new BaseColor(245, 245, 245) : BaseColor.WHITE;
                            inspectionForm2Table.AddCell(titleCell2);

                            PdfPCell valueCell2 = CreateCell(item.Value, 10, false);
                            valueCell2.Border = Rectangle.BOX;
                            valueCell2.Padding = 10;
                            valueCell2.BackgroundColor = isEvenRow2 ? new BaseColor(245, 245, 245) : BaseColor.WHITE;
                            inspectionForm2Table.AddCell(valueCell2);

                            isEvenRow2 = !isEvenRow2;
                        }

                        documents.Add(new Chunk(" "));
                        documents.Add(inspectionForm2Table);

                        var inspectionFormHealthData = await _iInspectionService.GetIncpectionFormFactoryPartIIIHealth(inspectionRefId);
                        var pairsHealth = ModelOps.GetTitleAndValues<Inspection_Form_Factory_Part_III_Health>(inspectionFormHealthData.FormModel);

                        var propNames = pairsHealth.Where(x => x.PropertyName.EndsWith("ViolationExist") && x.Value == "HAS_VIOLATION")
                            .Select(x => x.PropertyName.Replace("ViolationExist", "")).ToArray();

                        if (propNames.Count() > 0)
                        {
                            PdfPTable inspectionForm3HealthTable = new PdfPTable(2)
                            {
                                WidthPercentage = 100
                            };

                            // Set column widths where the first column (title) takes double the width of the second column (remarks)
                            float[] columnWidths3 = new float[] { 2f, 1f };
                            inspectionForm3HealthTable.SetWidths(columnWidths3);

                            PdfPCell headerCell3 = new PdfPCell(new Phrase("Health", FontFactory.GetFont("Helvetica", 14, Font.BOLD)))
                            {
                                Colspan = 2,
                                HorizontalAlignment = Element.ALIGN_CENTER,
                                Border = Rectangle.BOTTOM_BORDER,
                                BackgroundColor = new BaseColor(90, 90, 90),
                                PaddingTop = 5,  // Adjust top padding to reduce space
                                PaddingBottom = 5,  // Adjust bottom padding to reduce space
                                Phrase = new Phrase("Health", FontFactory.GetFont("Helvetica", 14, Font.BOLD, BaseColor.WHITE)),
                                FixedHeight = 30  // Set a fixed height for the header cell (adjust as needed)
                            };
                            inspectionForm3HealthTable.AddCell(headerCell3);

                            bool isEvenRow3 = true;  // To alternate row colors
                            foreach (var propName in propNames)
                            {
                                var title = pairsHealth.Where(x => x.PropertyName == propName + "Selection").Select(x => x.Title).FirstOrDefault();
                                var remarks = pairsHealth.Where(x => x.PropertyName == propName + "Remarks").Select(x => x.Value).FirstOrDefault();

                                PdfPCell titleCell3 = CreateCell(title, 10, true);
                                titleCell3.Border = Rectangle.BOX;
                                titleCell3.Padding = 10;
                                titleCell3.BackgroundColor = isEvenRow3 ? new BaseColor(245, 245, 245) : BaseColor.WHITE;
                                inspectionForm3HealthTable.AddCell(titleCell3);

                                PdfPCell valueCell3 = CreateCell(remarks, 10, false);
                                valueCell3.Border = Rectangle.BOX;
                                valueCell3.Padding = 10;
                                valueCell3.BackgroundColor = isEvenRow3 ? new BaseColor(245, 245, 245) : BaseColor.WHITE;
                                inspectionForm3HealthTable.AddCell(valueCell3);

                                isEvenRow3 = !isEvenRow3;

                                inspectionForm3HealthTable.AddCell(new PdfPCell(new Phrase(""))
                                {
                                    FixedHeight = 10,
                                    Border = PdfPCell.NO_BORDER
                                });

                                inspectionForm3HealthTable.AddCell(new PdfPCell(new Phrase(""))
                                {
                                    FixedHeight = 10,
                                    Border = PdfPCell.NO_BORDER
                                });
                            }

                            documents.Add(new Chunk(" "));
                            documents.Add(inspectionForm3HealthTable);
                        }


                        var inspectionFormSafetyData = await _iInspectionService.GetIncpectionFormFactoryPartIIISafety(inspectionRefId);
                        var pairsSafety = ModelOps.GetTitleAndValues<Inspection_Form_Factory_Part_III_Safety>(inspectionFormSafetyData.FormModel);

                        var propNamesSafety = pairsSafety.Where(x => x.PropertyName.EndsWith("ViolationExist") && x.Value == "HAS_VIOLATION").Select(x => x.PropertyName.Replace("ViolationExist", "")).ToArray();

                        if (propNamesSafety.Count() > 0)
                        {
                            PdfPTable inspectionFormSafetyTable = new PdfPTable(2)
                            {
                                WidthPercentage = 100
                            };

                            float[] columnWidthsSafety = new float[] { 2f, 1f };
                            inspectionFormSafetyTable.SetWidths(columnWidthsSafety);

                            PdfPCell headerCellSafety = new PdfPCell(new Phrase("Safety", FontFactory.GetFont("Helvetica", 14, Font.BOLD)))
                            {
                                Colspan = 2,
                                HorizontalAlignment = Element.ALIGN_CENTER,
                                Border = Rectangle.BOTTOM_BORDER,
                                BackgroundColor = new BaseColor(90, 90, 90),
                                PaddingTop = 5,  // Adjust top padding to reduce space
                                PaddingBottom = 5,  // Adjust bottom padding to reduce space
                                Phrase = new Phrase("Safety", FontFactory.GetFont("Helvetica", 14, Font.BOLD, BaseColor.WHITE)),
                                FixedHeight = 30  // Set a fixed height for the header cell (adjust as needed)
                            };
                            inspectionFormSafetyTable.AddCell(headerCellSafety);

                            bool isEvenRowSafety = true;
                            foreach (var propName in propNamesSafety)
                            {
                                var title = pairsSafety.Where(x => x.PropertyName == propName + "Selection").Select(x => x.Title).FirstOrDefault();
                                var remarks = pairsSafety.Where(x => x.PropertyName == propName + "Remarks").Select(x => x.Value).FirstOrDefault();

                                PdfPCell titleCellSafety = CreateCell(title, 10, true);
                                titleCellSafety.Border = Rectangle.BOX;
                                titleCellSafety.Padding = 10;
                                titleCellSafety.BackgroundColor = isEvenRowSafety ? new BaseColor(245, 245, 245) : BaseColor.WHITE;
                                inspectionFormSafetyTable.AddCell(titleCellSafety);

                                PdfPCell valueCellSafety = CreateCell(remarks, 10, false);
                                valueCellSafety.Border = Rectangle.BOX;
                                valueCellSafety.Padding = 10;
                                valueCellSafety.BackgroundColor = isEvenRowSafety ? new BaseColor(245, 245, 245) : BaseColor.WHITE;
                                inspectionFormSafetyTable.AddCell(valueCellSafety);

                                isEvenRowSafety = !isEvenRowSafety;

                                inspectionFormSafetyTable.AddCell(new PdfPCell(new Phrase(""))
                                {
                                    FixedHeight = 10,
                                    Border = PdfPCell.NO_BORDER
                                });

                                inspectionFormSafetyTable.AddCell(new PdfPCell(new Phrase(""))
                                {
                                    FixedHeight = 10,
                                    Border = PdfPCell.NO_BORDER
                                });
                            }

                            documents.Add(new Chunk(" "));
                            documents.Add(inspectionFormSafetyTable);
                        }

                        var inspectionFormWelfareData = await _iInspectionService.GetIncpectionGetFormFactoryPartIIIWelfare(inspectionRefId);
                        var pairsWelfare = ModelOps.GetTitleAndValues<Inspection_Form_Factory_Part_III_Welfare>(inspectionFormWelfareData.FormModel);
                        var propNamesWelfare = pairsWelfare.Where(x => x.PropertyName.EndsWith("ViolationExist") && x.Value == "HAS_VIOLATION").Select(x => x.PropertyName.Replace("ViolationExist", "")).ToArray();

                        if (propNamesWelfare.Count() > 0)
                        {
                            PdfPTable inspectionFormWelfareTable = new PdfPTable(2)
                            {
                                WidthPercentage = 100
                            };

                            float[] columnWidthsWelfare = new float[] { 2f, 1f };
                            inspectionFormWelfareTable.SetWidths(columnWidthsWelfare);

                            PdfPCell headerCellWelfare = new PdfPCell(new Phrase("Welfare", FontFactory.GetFont("Helvetica", 14, Font.BOLD)))
                            {
                                Colspan = 2,
                                HorizontalAlignment = Element.ALIGN_CENTER,
                                Border = Rectangle.BOTTOM_BORDER,
                                BackgroundColor = new BaseColor(90, 90, 90),
                                PaddingTop = 5,  // Adjust top padding to reduce space
                                PaddingBottom = 5,  // Adjust bottom padding to reduce space
                                Phrase = new Phrase("Welfare", FontFactory.GetFont("Helvetica", 14, Font.BOLD, BaseColor.WHITE)),
                                FixedHeight = 30  // Set a fixed height for the header cell (adjust as needed)
                            };
                            inspectionFormWelfareTable.AddCell(headerCellWelfare);

                            bool isEvenRowWelfare = true;
                            foreach (var propName in propNamesWelfare)
                            {
                                var title = pairsWelfare.Where(x => x.PropertyName == propName + "Selection").Select(x => x.Title).FirstOrDefault();
                                var remarks = pairsWelfare.Where(x => x.PropertyName == propName + "Remarks").Select(x => x.Value).FirstOrDefault();

                                PdfPCell titleCellWelfare = CreateCell(title, 10, true);
                                titleCellWelfare.Border = Rectangle.BOX;
                                titleCellWelfare.Padding = 10;
                                titleCellWelfare.BackgroundColor = isEvenRowWelfare ? new BaseColor(245, 245, 245) : BaseColor.WHITE;
                                inspectionFormWelfareTable.AddCell(titleCellWelfare);

                                PdfPCell valueCellWelfare = CreateCell(remarks, 10, false);
                                valueCellWelfare.Border = Rectangle.BOX;
                                valueCellWelfare.Padding = 10;
                                valueCellWelfare.BackgroundColor = isEvenRowWelfare ? new BaseColor(245, 245, 245) : BaseColor.WHITE;
                                inspectionFormWelfareTable.AddCell(valueCellWelfare);

                                isEvenRowWelfare = !isEvenRowWelfare;

                                inspectionFormWelfareTable.AddCell(new PdfPCell(new Phrase(""))
                                {
                                    FixedHeight = 10,
                                    Border = PdfPCell.NO_BORDER
                                });

                                inspectionFormWelfareTable.AddCell(new PdfPCell(new Phrase(""))
                                {
                                    FixedHeight = 10,
                                    Border = PdfPCell.NO_BORDER
                                });
                            }

                            documents.Add(new Chunk(" "));
                            documents.Add(inspectionFormWelfareTable);
                        }

                        var inspectionFormGeneralData = await _iInspectionService.GetIncpectionGetFormFactoryPartIIIGeneral(inspectionRefId);
                        var pairsGeneral = ModelOps.GetTitleAndValues<Inspection_Form_Factory_Part_III_General>(inspectionFormGeneralData.FormModel);
                        var propNamesGeneral = pairsGeneral.Where(x => x.PropertyName.EndsWith("ViolationExist") && x.Value == "HAS_VIOLATION").Select(x => x.PropertyName.Replace("ViolationExist", "")).ToArray();

                        if (propNamesGeneral.Count() > 0)
                        {
                            PdfPTable inspectionFormGeneralTable = new PdfPTable(2)
                            {
                                WidthPercentage = 100
                            };

                            float[] columnWidthsGeneral = new float[] { 2f, 1f };
                            inspectionFormGeneralTable.SetWidths(columnWidthsGeneral);

                            PdfPCell headerCellGeneral = new PdfPCell(new Phrase("General", FontFactory.GetFont("Helvetica", 14, Font.BOLD)))
                            {
                                Colspan = 2,
                                HorizontalAlignment = Element.ALIGN_CENTER,
                                Border = Rectangle.BOTTOM_BORDER,
                                BackgroundColor = new BaseColor(90, 90, 90),
                                PaddingTop = 5,  // Adjust top padding to reduce space
                                PaddingBottom = 5,  // Adjust bottom padding to reduce space
                                Phrase = new Phrase("General", FontFactory.GetFont("Helvetica", 14, Font.BOLD, BaseColor.WHITE)),
                                FixedHeight = 30  // Set a fixed height for the header cell (adjust as needed)
                            };
                            inspectionFormGeneralTable.AddCell(headerCellGeneral);

                            bool isEvenRowGeneral = true;
                            foreach (var propName in propNamesGeneral)
                            {
                                var title = pairsGeneral.Where(x => x.PropertyName == propName + "Selection").Select(x => x.Title).FirstOrDefault();
                                var remarks = pairsGeneral.Where(x => x.PropertyName == propName + "Remarks").Select(x => x.Value).FirstOrDefault();

                                PdfPCell titleCellGeneral = CreateCell(title, 10, true);
                                titleCellGeneral.Border = Rectangle.BOX;
                                titleCellGeneral.Padding = 10;
                                titleCellGeneral.BackgroundColor = isEvenRowGeneral ? new BaseColor(245, 245, 245) : BaseColor.WHITE;
                                inspectionFormGeneralTable.AddCell(titleCellGeneral);

                                PdfPCell valueCellGeneral = CreateCell(remarks, 10, false);
                                valueCellGeneral.Border = Rectangle.BOX;
                                valueCellGeneral.Padding = 10;
                                valueCellGeneral.BackgroundColor = isEvenRowGeneral ? new BaseColor(245, 245, 245) : BaseColor.WHITE;
                                inspectionFormGeneralTable.AddCell(valueCellGeneral);

                                isEvenRowGeneral = !isEvenRowGeneral;

                                inspectionFormGeneralTable.AddCell(new PdfPCell(new Phrase(""))
                                {
                                    FixedHeight = 10,
                                    Border = PdfPCell.NO_BORDER
                                });

                                inspectionFormGeneralTable.AddCell(new PdfPCell(new Phrase(""))
                                {
                                    FixedHeight = 10,
                                    Border = PdfPCell.NO_BORDER
                                });
                            }

                            documents.Add(new Chunk(" "));
                            documents.Add(inspectionFormGeneralTable);
                        }

                        var inspectionFormMajorAccidentData = await _iInspectionService.GetIncpectionGetFormFactoryPartIIIMajorAccidentHazard(inspectionRefId);
                        var pairsMajorAccident = ModelOps.GetTitleAndValues<Inspection_Form_Factory_Part_III_MajorAccidentHazard>(inspectionFormMajorAccidentData.FormModel);
                        var propNamesMajorAccident = pairsMajorAccident.Where(x => x.PropertyName.EndsWith("ViolationExist") && x.Value == "HAS_VIOLATION").Select(x => x.PropertyName.Replace("_ViolationExist", "")).ToArray();



                        if (propNamesMajorAccident.Count() > 0)
                        {
                            PdfPTable inspectionFormMajorAccidentTable = new PdfPTable(2)
                            {
                                WidthPercentage = 100
                            };
                            float[] columnWidthsMajorAccident = new float[] { 2f, 1f };
                            inspectionFormMajorAccidentTable.SetWidths(columnWidthsMajorAccident);

                            PdfPCell headerCellMajorAccident = new PdfPCell(new Phrase("Major Accident", FontFactory.GetFont("Helvetica", 14, Font.BOLD)))
                            {
                                Colspan = 2,
                                HorizontalAlignment = Element.ALIGN_CENTER,
                                Border = Rectangle.BOTTOM_BORDER,
                                BackgroundColor = new BaseColor(90, 90, 90),
                                PaddingTop = 5,  // Adjust top padding to reduce space
                                PaddingBottom = 5,  // Adjust bottom padding to reduce space
                                Phrase = new Phrase("Major Accident ", FontFactory.GetFont("Helvetica", 14, Font.BOLD, BaseColor.WHITE)),
                                FixedHeight = 30  // Set a fixed height for the header cell (adjust as needed)
                            };
                            inspectionFormMajorAccidentTable.AddCell(headerCellMajorAccident);

                            bool isEvenRowMajorAccident = true;
                            foreach (var propName in propNamesMajorAccident)
                            {
                                var title = pairsMajorAccident.Where(x => x.PropertyName == propName).Select(x => x.Title).FirstOrDefault();
                                var remarks = pairsMajorAccident.Where(x => x.PropertyName == propName).Select(x => x.Value).FirstOrDefault();

                                PdfPCell titleCellMajorAccident = CreateCell(title, 10, true);
                                titleCellMajorAccident.Border = Rectangle.BOX;
                                titleCellMajorAccident.Padding = 10;
                                titleCellMajorAccident.BackgroundColor = isEvenRowMajorAccident ? new BaseColor(245, 245, 245) : BaseColor.WHITE;
                                inspectionFormMajorAccidentTable.AddCell(titleCellMajorAccident);

                                PdfPCell valueCellMajorAccident = CreateCell(remarks, 10, false);
                                valueCellMajorAccident.Border = Rectangle.BOX;
                                valueCellMajorAccident.Padding = 10;
                                valueCellMajorAccident.BackgroundColor = isEvenRowMajorAccident ? new BaseColor(245, 245, 245) : BaseColor.WHITE;
                                inspectionFormMajorAccidentTable.AddCell(valueCellMajorAccident);

                                isEvenRowMajorAccident = !isEvenRowMajorAccident;

                                inspectionFormMajorAccidentTable.AddCell(new PdfPCell(new Phrase(""))
                                {
                                    FixedHeight = 10,
                                    Border = PdfPCell.NO_BORDER
                                });

                                inspectionFormMajorAccidentTable.AddCell(new PdfPCell(new Phrase(""))
                                {
                                    FixedHeight = 10,
                                    Border = PdfPCell.NO_BORDER
                                });
                            }

                            documents.Add(new Chunk(" "));
                            documents.Add(inspectionFormMajorAccidentTable);
                        }
                    }
                    else
                    {

                        // Create a table for displaying inspection data
                        PdfPTable inspectionTable = new PdfPTable(2)
                        {
                            WidthPercentage = 100
                        };

                        inspectionTable.AddCell(CreateCell("Factory Name", 10, true));
                        inspectionTable.AddCell(CreateCell(establishmentDetails.ResponseDataModel.EstablishmentName, 10, false));

                        inspectionTable.AddCell(CreateCell("Factory Address", 10, true));
                        inspectionTable.AddCell(CreateCell(establishmentDetails.ResponseDataModel.EstablishmentAddress, 10, false));

                        inspectionTable.AddCell(CreateCell("Factory Status", 10, true));
                        inspectionTable.AddCell(CreateCell(EnumOps.GetEnumDescriptionName<InspectionFactoryExistenceTypeEnum>(Enum.GetName(typeof(InspectionFactoryExistenceTypeEnum), inspectionMasterData.InspectionSubmited_Factory_Wing_FactoryExistenceType)), 10, false));

                        inspectionTable.AddCell(CreateCell("Factory De-Registration Number", 10, true));
                        inspectionTable.AddCell(CreateCell(inspectionMasterData.FactoryDeRegistrationNo, 10, false));

                        inspectionTable.AddCell(CreateCell("Inspection Date", 10, true));
                        inspectionTable.AddCell(CreateCell(inspectionMasterData.InspectionDoneOn_Factory_Wing.ToString(), 10, false));

                        inspectionTable.AddCell(CreateCell("Remarks", 10, true));
                        inspectionTable.AddCell(CreateCell(inspectionMasterData.Remarks.ToString(), 10, false));


                        inspectionTable.AddCell(new PdfPCell(new Phrase(""))
                        {
                            FixedHeight = 10, // Adjust height for spacing
                            Border = PdfPCell.NO_BORDER // No border for spacing cell
                        });

                        // Add another empty cell to create a gap
                        inspectionTable.AddCell(new PdfPCell(new Phrase(""))
                        {
                            FixedHeight = 10,
                            Border = PdfPCell.NO_BORDER
                        });

                        documents.Add(new Chunk(" "));
                        documents.Add(inspectionTable);

                        Paragraph SignSeal2 = new Paragraph()
                        {
                            Alignment = Element.ALIGN_CENTER, // Justify the text
                            SpacingBefore = 10f, // Margin-top before the text
                            SpacingAfter = 10f, // Margin-bottom after the text
                            IndentationLeft = 10f,   // Padding left
                            IndentationRight = 10f   // Padding right

                        };
                        Font redFont = FontFactory.GetFont("Times-Roman", 14, Font.NORMAL, BaseColor.RED);

                        // Add the text with the specified font
                        SignSeal2.Add(new Chunk("No Violation found..!!", redFont));
                        documents.Add(SignSeal2);
                    }

                    var officerDetails = _context.Inspection_Master.Where(x => x.InspectionId == inspectionRefId).FirstOrDefault();
                    var officerUserRole = _context.Users.Where(x => x.Id == officerDetails.InspectionSubmited_Factory_Wing_UserId && x.IsEnabled == true).Include(x => x.UserRoles).ThenInclude(x => x.Role).FirstOrDefault();
                    var circleName = _context.FactoryCircles.Where(x => x.FactoryCircleId == officerDetails.FactoryCircleId && x.Version == 2);
                    //var userDetails = await _iAuthService.GetUserFullDetailByUserName(officerUserRole.UserName);
                    var userDetails = await _iAuthService.GetUserProfileByProfileId(officerDetails.InspectionSubmited_Factory_Wing_ProfileId);
                    //string officerSigFilePath = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "OfficerSignatures")).Root + userDetails.ListData[0].Signature;

                    //iTextSharp.text.Image signature = iTextSharp.text.Image.GetInstance(officerSigFilePath);
                    //signature.ScaleToFit(120f, 120f);
                    //signature.SetAbsolutePosition(450f, 150f); // Adjust position as needed


                    Paragraph SignSeal = new Paragraph()
                    {
                        Alignment = Element.ALIGN_RIGHT, // Justify the text
                        SpacingBefore = 10f, // Margin-top before the text
                        SpacingAfter = 10f, // Margin-bottom after the text
                        IndentationLeft = 10f,   // Padding left
                        IndentationRight = 10f   // Padding right
                    };
                    SignSeal.Add(new Chunk("Name: ", timesRoman));
                    SignSeal.Add(new Chunk(userDetails.FirstName + " " + userDetails.LastName, timesRoman));
                    SignSeal.Add(new Chunk(Chunk.NEWLINE + "Desig: ", timesRoman));
                    SignSeal.Add(new Chunk(officerUserRole.UserRoles.FirstOrDefault().Role.NormalizedName, timesRoman));
                    SignSeal.Add(new Chunk(Chunk.NEWLINE + "Circle Name: ", timesRoman));
                    SignSeal.Add(new Chunk(circleName.FirstOrDefault().FactoryCircleName, timesRoman));

                    //PdfPTable ptablerightSideDetail = new PdfPTable(1);
                    //ptablerightSideDetail.DefaultCell.Border = 0;
                    //ptablerightSideDetail.DefaultCell.Padding = 0;

                    //ptablerightSideDetail.AddCell(new PdfPCell(signature) { Border = 0, PaddingTop = 5, HorizontalAlignment = PdfPCell.ALIGN_RIGHT });

                    // Create a table to align signature and details
                    PdfPTable officerTable = new PdfPTable(2)
                    {
                        WidthPercentage = 100
                    };
                    documents.Add(SignSeal);
                    documents.Close();
                    PDFPageEvents pageeventsobj = new PDFPageEvents();
                    string filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Inspections", "Violation_" + inspectionRefId + "_" + inspectionType + ".pdf");

                    string pdfFilePath = sfilepath;
                    Byte[] bytes = File.ReadAllBytes(sfilepath);
                    string file = System.Convert.ToBase64String(bytes);
                    genericFormModel.PdfContent = file;
                    genericFormModel.FileNo = "Violation_" + inspectionRefId + "_" + inspectionType;
                }
            }
            catch (Exception ex)
            {
                genericFormModel.HasException = true;
                genericFormModel.Exceptions = ex.Message;
            }
            return genericFormModel;
        }

        private async Task<GeneratePdfServiceResultTemplate> Inspection_ViolationReport_LabourWing(Int64 inspectionRefId, int inspectionType)
        {
            GeneratePdfServiceResultTemplate genericFormModel = new GeneratePdfServiceResultTemplate();
            try
            {
                Guid guId = Guid.NewGuid();
                genericFormModel.PdfNameGUID = guId.ToString();
                var sfilepath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Inspections", "Violation_" + inspectionRefId + "_" + inspectionType + ".pdf");

                // Ensure the file is deleted if it already exists
                if (File.Exists(sfilepath))
                {
                    File.Delete(sfilepath);
                }

                using (iTextSharp.text.Document documents = new iTextSharp.text.Document(iTextSharp.text.PageSize.A4, 30, 30, 50, 50))
                {
                    PdfWriter owriter = PdfWriter.GetInstance(documents, new FileStream(sfilepath, FileMode.Create));
                    owriter.SetEncryption(PdfWriter.STRENGTH128BITS, "", "", PdfWriter.AllowPrinting);
                    documents.Open();
                    var inspectionMasterData = _context.Inspection_Master.Where(x => x.InspectionId == inspectionRefId).FirstOrDefault();
                    var establishmentDetails = await _iInspectionService.GetInspectionEstablishmentBasicDetails(inspectionMasterData.LicenceNumber);
                    documents.Add(new Paragraph("Violation Report(Labour-Wing)", FontFactory.GetFont("Times-Roman", 20, Font.BOLD)) { Alignment = Element.ALIGN_CENTER });
                    if (inspectionMasterData.InspectionSubmited_Labour_Wing_FactoryExistenceType == InspectionFactoryExistenceTypeEnum.OPEN)
                    {
                        var inspectionData = await _iInspectionService.GetInspectionFormLabourPartIGeneralDetail(inspectionRefId);
                        var pairs = ModelOps.GetTitleAndValues<Inspection_Form_Labour_Part_I_General>(inspectionData.FormModel);

                        PdfPTable inspectionFactoryTable = new PdfPTable(2)
                        {
                            WidthPercentage = 100
                        };

                        float[] columnWidths = new float[] { 1f, 2f };
                        inspectionFactoryTable.SetWidths(columnWidths);

                        PdfPCell headerCell = new PdfPCell(new Phrase("General Details", FontFactory.GetFont("Helvetica", 14, Font.BOLD)))
                        {
                            Colspan = 2,
                            HorizontalAlignment = Element.ALIGN_CENTER,
                            Border = Rectangle.BOTTOM_BORDER,
                            BackgroundColor = new BaseColor(90, 90, 90),
                            PaddingTop = 5,  // Adjust top padding to reduce space
                            PaddingBottom = 5,  // Adjust bottom padding to reduce space
                            Phrase = new Phrase("General Details", FontFactory.GetFont("Helvetica", 14, Font.BOLD, BaseColor.WHITE)),
                            FixedHeight = 30  // Set a fixed height for the header cell (adjust as needed)
                        };

                        inspectionFactoryTable.AddCell(headerCell);

                        bool isEvenRow = true;
                        foreach (var item in pairs)
                        {
                            PdfPCell titleCell = CreateCell(item.Title, 10, true);
                            titleCell.Border = Rectangle.BOX;
                            titleCell.Padding = 10;
                            titleCell.BackgroundColor = isEvenRow ? new BaseColor(245, 245, 245) : BaseColor.WHITE;
                            inspectionFactoryTable.AddCell(titleCell);

                            PdfPCell valueCell = CreateCell(item.Value, 10, false);
                            valueCell.Border = Rectangle.BOX;
                            valueCell.Padding = 10;
                            valueCell.BackgroundColor = isEvenRow ? new BaseColor(245, 245, 245) : BaseColor.WHITE;
                            inspectionFactoryTable.AddCell(valueCell);

                            isEvenRow = !isEvenRow;
                        }

                        documents.Add(new Chunk(" "));
                        documents.Add(inspectionFactoryTable);

                        var inspectionForm2Data = await _iInspectionService.GetInspectionFormLabourPartIIFactoryDetail(inspectionRefId);
                        var pairs2 = ModelOps.GetTitleAndValues<Inspection_Form_Labour_Part_II_FactoryDetail>(inspectionForm2Data.FormModel);

                        PdfPTable inspectionForm2Table = new PdfPTable(2)
                        {
                            WidthPercentage = 100
                        };

                        float[] columnWidths2 = new float[] { 1f, 2f };
                        inspectionForm2Table.SetWidths(columnWidths2);

                        PdfPCell headerCell2 = new PdfPCell(new Phrase("Factory Details", FontFactory.GetFont("Helvetica", 14, Font.BOLD)))
                        {
                            Colspan = 2,
                            HorizontalAlignment = Element.ALIGN_CENTER,
                            Border = Rectangle.BOTTOM_BORDER,
                            BackgroundColor = new BaseColor(90, 90, 90),
                            PaddingTop = 5,  // Adjust top padding to reduce space
                            PaddingBottom = 5,  // Adjust bottom padding to reduce space
                            Phrase = new Phrase("Factory Details", FontFactory.GetFont("Helvetica", 14, Font.BOLD, BaseColor.WHITE)),
                            FixedHeight = 30  // Set a fixed height for the header cell (adjust as needed)
                        };

                        inspectionForm2Table.AddCell(headerCell2);

                        bool isEvenRow2 = true;
                        foreach (var item in pairs2)
                        {
                            PdfPCell titleCell2 = CreateCell(item.Title, 10, true);
                            titleCell2.Border = Rectangle.BOX;
                            titleCell2.Padding = 10;
                            titleCell2.BackgroundColor = isEvenRow2 ? new BaseColor(245, 245, 245) : BaseColor.WHITE;
                            inspectionForm2Table.AddCell(titleCell2);

                            PdfPCell valueCell2 = CreateCell(item.Value, 10, false);
                            valueCell2.Border = Rectangle.BOX;
                            valueCell2.Padding = 10;
                            valueCell2.BackgroundColor = isEvenRow2 ? new BaseColor(245, 245, 245) : BaseColor.WHITE;
                            inspectionForm2Table.AddCell(valueCell2);

                            isEvenRow2 = !isEvenRow2;
                        }

                        documents.Add(new Chunk(" "));
                        documents.Add(inspectionForm2Table);


                        var inspectionFormEqualEnumerationAct = await _iInspectionService.GetInspectionFormLabourPartIIIEqualEnumerationAct(inspectionRefId);
                        var pairsEnumerationAct = ModelOps.GetTitleAndValues<Inspection_Form_Labour_Part_III_EqualEnumerationAct>(inspectionFormEqualEnumerationAct.FormModel);
                        var propNames = pairsEnumerationAct.Where(x => x.PropertyName.EndsWith("ViolationExist") && x.Value == "HAS_VIOLATION")
                                                            .Select(x => x.PropertyName.Replace("_ViolationExist", "")).ToArray();

                        // Create a table for displaying inspection data
                        PdfPTable inspectionEqualEnumerationAct = new PdfPTable(2)
                        {
                            WidthPercentage = 100
                        };

                        // Set column widths (renamed to avoid conflict)
                        float[] columnWidthsForEqualEnumeration = new float[] { 1f, 2f };
                        inspectionEqualEnumerationAct.SetWidths(columnWidthsForEqualEnumeration);

                        // Create the header cell for "Equal Enumeration Act" (renamed to avoid conflict)
                        PdfPCell headerCellEqualEnumeration = new PdfPCell(new Phrase("Equal Enumeration Act", FontFactory.GetFont("Helvetica", 14, Font.BOLD)))
                        {
                            Colspan = 2,
                            HorizontalAlignment = Element.ALIGN_CENTER,
                            Border = Rectangle.BOTTOM_BORDER,
                            BackgroundColor = new BaseColor(90, 90, 90),
                            PaddingTop = 5,  // Adjust top padding to reduce space
                            PaddingBottom = 5,  // Adjust bottom padding to reduce space
                            Phrase = new Phrase("Equal Enumeration Act", FontFactory.GetFont("Helvetica", 14, Font.BOLD, BaseColor.WHITE)),
                            FixedHeight = 30  // Set a fixed height for the header cell (adjust as needed)
                        };

                        inspectionEqualEnumerationAct.AddCell(headerCellEqualEnumeration);

                        // Renaming the row indicator variable to avoid conflict
                        bool isEvenRowForEqualEnumeration = true;
                        if (propNames.Count() > 0)
                        {
                            foreach (var propName in propNames)
                            {
                                var title = pairsEnumerationAct.Where(x => x.PropertyName == propName).Select(x => x.Title).FirstOrDefault();
                                var remarks = pairsEnumerationAct.Where(x => x.PropertyName == propName + "_Remarks").Select(x => x.Value).FirstOrDefault();

                                PdfPCell titleCell = CreateCell(title, 10, true);
                                titleCell.Border = Rectangle.BOX;
                                titleCell.Padding = 10;
                                titleCell.BackgroundColor = isEvenRowForEqualEnumeration ? new BaseColor(245, 245, 245) : BaseColor.WHITE;
                                inspectionEqualEnumerationAct.AddCell(titleCell);

                                PdfPCell remarksCell = CreateCell(remarks, 10, false);
                                remarksCell.Border = Rectangle.BOX;
                                remarksCell.Padding = 10;
                                remarksCell.BackgroundColor = isEvenRowForEqualEnumeration ? new BaseColor(245, 245, 245) : BaseColor.WHITE;
                                inspectionEqualEnumerationAct.AddCell(remarksCell);

                                isEvenRowForEqualEnumeration = !isEvenRowForEqualEnumeration;
                            }

                            // Add the table to the document
                            documents.Add(new Chunk(" "));
                            documents.Add(inspectionEqualEnumerationAct);
                        }




                        var inspectionFormLabourIIIMinimumWageAct = await _iInspectionService.GetInspectionFormLabourPartIIIMinimumWagesAct(inspectionRefId);
                        var pairsMinimumWageAct = ModelOps.GetTitleAndValues<Inspection_Form_Labour_III_MinimumWageAct>(inspectionFormLabourIIIMinimumWageAct.FormModel);
                        var propNamesMinimumWageAct = pairsMinimumWageAct.Where(x => x.PropertyName.EndsWith("ViolationExist") && x.Value == "HAS_VIOLATION").Select(x => x.PropertyName.Replace("_ViolationExist", "")).ToArray();
                        // Create a table for displaying inspection data
                        PdfPTable inspectionMinimumWageAct = new PdfPTable(2)
                        {
                            WidthPercentage = 100
                        };
                        if (propNamesMinimumWageAct.Count() > 0)
                        {
                            foreach (var propName in propNamesMinimumWageAct)
                            {
                                var title = pairsMinimumWageAct.Where(x => x.PropertyName == propName).Select(x => x.Title).FirstOrDefault();
                                var remarks = pairsMinimumWageAct.Where(x => x.PropertyName == propName + "_Remarks").Select(x => x.Value).FirstOrDefault();

                                inspectionMinimumWageAct.AddCell(CreateCell(title, 10, true));
                                inspectionMinimumWageAct.AddCell(CreateCell(remarks, 10, false));

                                inspectionMinimumWageAct.AddCell(new PdfPCell(new Phrase(""))
                                {
                                    FixedHeight = 10, // Adjust height for spacing
                                    Border = PdfPCell.NO_BORDER // No border for spacing cell
                                });

                                // Add another empty cell to create a gap
                                inspectionMinimumWageAct.AddCell(new PdfPCell(new Phrase(""))
                                {
                                    FixedHeight = 10,
                                    Border = PdfPCell.NO_BORDER
                                });

                            }

                            documents.Add(new Chunk(" ")); // Add space
                            documents.Add(new Paragraph("Minimum Wage Act", FontFactory.GetFont("Times-Roman", 16, Font.BOLD)) { Alignment = Element.ALIGN_CENTER });
                            documents.Add(new Chunk(""));
                            documents.Add(inspectionMinimumWageAct);
                        }


                        var inspectionFormLabourIIIPaymentWagesAct = await _iInspectionService.GetInspectionFormLabourPartIIIPaymentWagesAct(inspectionRefId);
                        var pairsPaymentWagesAct = ModelOps.GetTitleAndValues<Inspection_Form_Labour_III_PaymentWagesAct>(inspectionFormLabourIIIPaymentWagesAct.FormModel);
                        var propNamesPaymentWagesAct = pairsPaymentWagesAct.Where(x => x.PropertyName.EndsWith("ViolationExist") && x.Value == "HAS_VIOLATION").Select(x => x.PropertyName.Replace("_ViolationExist", "")).ToArray();
                        // Create a table for displaying inspection data
                        PdfPTable inspectionPaymentWagesAct = new PdfPTable(2)
                        {
                            WidthPercentage = 100
                        };
                        if (propNamesPaymentWagesAct.Count() > 0)
                        {
                            foreach (var propName in propNamesPaymentWagesAct)
                            {
                                var title = pairsMinimumWageAct.Where(x => x.PropertyName == propName).Select(x => x.Title).FirstOrDefault();
                                var remarks = pairsMinimumWageAct.Where(x => x.PropertyName == propName + "_Remarks").Select(x => x.Value).FirstOrDefault();

                                inspectionMinimumWageAct.AddCell(CreateCell(title, 10, true));
                                inspectionMinimumWageAct.AddCell(CreateCell(remarks, 10, false));

                                inspectionMinimumWageAct.AddCell(new PdfPCell(new Phrase(""))
                                {
                                    FixedHeight = 10, // Adjust height for spacing
                                    Border = PdfPCell.NO_BORDER // No border for spacing cell
                                });

                                // Add another empty cell to create a gap
                                inspectionMinimumWageAct.AddCell(new PdfPCell(new Phrase(""))
                                {
                                    FixedHeight = 10,
                                    Border = PdfPCell.NO_BORDER
                                });

                            }

                            documents.Add(new Chunk(" ")); // Add space
                            documents.Add(new Paragraph("Payment Wages Act", FontFactory.GetFont("Times-Roman", 16, Font.BOLD)) { Alignment = Element.ALIGN_CENTER });
                            documents.Add(new Chunk(""));
                            documents.Add(inspectionPaymentWagesAct);
                        }


                        var inspectionFormLabourIIIPaymentBonusActStatutoryReport = await _iInspectionService.GetInspectionFormLabourPartIIIPaymentBonusAct_StatutoryReport(inspectionRefId);
                        var pairsPaymentBonusActStatutoryReport = ModelOps.GetTitleAndValues<Inspection_Form_Labour_III_PaymentBonusAct_StatutoryReport>(inspectionFormLabourIIIPaymentBonusActStatutoryReport.FormModel);
                        var propNamesPaymentBonusActStatutoryReport = pairsPaymentBonusActStatutoryReport.Where(x => x.PropertyName.EndsWith("ViolationExist") && x.Value == "HAS_VIOLATION").Select(x => x.PropertyName.Replace("_ViolationExist", "")).ToArray();
                        // Create a table for displaying inspection data
                        PdfPTable inspectionPaymentBonusActStatutoryReport = new PdfPTable(2)
                        {
                            WidthPercentage = 100
                        };
                        if (propNamesPaymentBonusActStatutoryReport.Count() > 0)
                        {
                            foreach (var propName in propNamesPaymentBonusActStatutoryReport)
                            {
                                var title = pairsPaymentBonusActStatutoryReport.Where(x => x.PropertyName == propName).Select(x => x.Title).FirstOrDefault();
                                var remarks = pairsPaymentBonusActStatutoryReport.Where(x => x.PropertyName == propName + "_Remarks").Select(x => x.Value).FirstOrDefault();

                                inspectionPaymentBonusActStatutoryReport.AddCell(CreateCell(title, 10, true));
                                inspectionPaymentBonusActStatutoryReport.AddCell(CreateCell(remarks, 10, false));

                                inspectionPaymentBonusActStatutoryReport.AddCell(new PdfPCell(new Phrase(""))
                                {
                                    FixedHeight = 10, // Adjust height for spacing
                                    Border = PdfPCell.NO_BORDER // No border for spacing cell
                                });

                                // Add another empty cell to create a gap
                                inspectionPaymentBonusActStatutoryReport.AddCell(new PdfPCell(new Phrase(""))
                                {
                                    FixedHeight = 10,
                                    Border = PdfPCell.NO_BORDER
                                });
                            }
                            documents.Add(new Chunk(" ")); // Add space
                            documents.Add(new Paragraph("Payment BonusAct Statutory Report", FontFactory.GetFont("Times-Roman", 16, Font.BOLD)) { Alignment = Element.ALIGN_CENTER });
                            documents.Add(new Chunk(""));
                            documents.Add(inspectionPaymentBonusActStatutoryReport);
                        }


                        var inspectionFormLabourIIIChildAndAdolescentLabourAct = await _iInspectionService.GetInspectionFormLabourPartIIIChildAndAdolescentLabourAct(inspectionRefId);
                        var pairsPaymentChildAndAdolescentLabourAct = ModelOps.GetTitleAndValues<Inspection_Form_Labour_III_ChildAndAdolescentLabourAct>(inspectionFormLabourIIIChildAndAdolescentLabourAct.FormModel);
                        var propNamesPaymentChildAndAdolescentLabourAct = pairsPaymentChildAndAdolescentLabourAct.Where(x => x.PropertyName.EndsWith("ViolationExist") && x.Value == "HAS_VIOLATION").Select(x => x.PropertyName.Replace("_ViolationExist", "")).ToArray();
                        // Create a table for displaying inspection data
                        PdfPTable inspectionChildAndAdolescentLabourAct = new PdfPTable(2)
                        {
                            WidthPercentage = 100
                        };
                        if (propNamesPaymentChildAndAdolescentLabourAct.Count() > 0)
                        {
                            foreach (var propName in propNamesPaymentChildAndAdolescentLabourAct)
                            {
                                var title = pairsPaymentChildAndAdolescentLabourAct.Where(x => x.PropertyName == propName).Select(x => x.Title).FirstOrDefault();
                                var remarks = pairsPaymentChildAndAdolescentLabourAct.Where(x => x.PropertyName == propName + "_Remarks").Select(x => x.Value).FirstOrDefault();

                                inspectionChildAndAdolescentLabourAct.AddCell(CreateCell(title, 10, true));
                                inspectionChildAndAdolescentLabourAct.AddCell(CreateCell(remarks, 10, false));

                                inspectionChildAndAdolescentLabourAct.AddCell(new PdfPCell(new Phrase(""))
                                {
                                    FixedHeight = 10, // Adjust height for spacing
                                    Border = PdfPCell.NO_BORDER // No border for spacing cell
                                });

                                // Add another empty cell to create a gap
                                inspectionChildAndAdolescentLabourAct.AddCell(new PdfPCell(new Phrase(""))
                                {
                                    FixedHeight = 10,
                                    Border = PdfPCell.NO_BORDER
                                });
                            }
                            documents.Add(new Chunk(" ")); // Add space
                            documents.Add(new Paragraph("Child And Adolescent Labour Act", FontFactory.GetFont("Times-Roman", 16, Font.BOLD)) { Alignment = Element.ALIGN_CENTER });
                            documents.Add(new Chunk(""));
                            documents.Add(inspectionChildAndAdolescentLabourAct);
                        }


                        var inspectionFormLabourIIINationalAndFestivalHolidays = await _iInspectionService.GetInspectionFormLabourPartIIINationalAndFestivalHolidays(inspectionRefId);
                        var pairsNationalAndFestivalHolidays = ModelOps.GetTitleAndValues<Inspection_Form_Labour_III_NationalAndFestivalHolidays>(inspectionFormLabourIIINationalAndFestivalHolidays.FormModel);
                        var propNamesNationalAndFestivalHolidays = pairsNationalAndFestivalHolidays.Where(x => x.PropertyName.EndsWith("ViolationExist") && x.Value == "HAS_VIOLATION").Select(x => x.PropertyName.Replace("_ViolationExist", "")).ToArray();
                        // Create a table for displaying inspection data
                        PdfPTable inspectionNationalAndFestivalHolidays = new PdfPTable(2)
                        {
                            WidthPercentage = 100
                        };
                        if (propNamesNationalAndFestivalHolidays.Count() > 0)
                        {
                            foreach (var propName in propNamesNationalAndFestivalHolidays)
                            {
                                var title = pairsNationalAndFestivalHolidays.Where(x => x.PropertyName == propName).Select(x => x.Title).FirstOrDefault();
                                var remarks = pairsNationalAndFestivalHolidays.Where(x => x.PropertyName == propName + "_Remarks").Select(x => x.Value).FirstOrDefault();

                                inspectionNationalAndFestivalHolidays.AddCell(CreateCell(title, 10, true));
                                inspectionNationalAndFestivalHolidays.AddCell(CreateCell(remarks, 10, false));

                                inspectionNationalAndFestivalHolidays.AddCell(new PdfPCell(new Phrase(""))
                                {
                                    FixedHeight = 10, // Adjust height for spacing
                                    Border = PdfPCell.NO_BORDER // No border for spacing cell
                                });

                                // Add another empty cell to create a gap
                                inspectionNationalAndFestivalHolidays.AddCell(new PdfPCell(new Phrase(""))
                                {
                                    FixedHeight = 10,
                                    Border = PdfPCell.NO_BORDER
                                });
                            }
                            documents.Add(new Chunk(" ")); // Add space
                            documents.Add(new Paragraph("National And Festival Holidays", FontFactory.GetFont("Times-Roman", 16, Font.BOLD)) { Alignment = Element.ALIGN_CENTER });
                            documents.Add(new Chunk(""));
                            documents.Add(inspectionNationalAndFestivalHolidays);
                        }


                        var inspectionFormLabourIIIMaternityBenefitAct = await _iInspectionService.GetInspectionFormLabourPartIIIMaternityBenefitAct(inspectionRefId);
                        var pairsMaternityBenefitAct = ModelOps.GetTitleAndValues<Inspection_Form_Labour_III_MaternityBenefitAct>(inspectionFormLabourIIIMaternityBenefitAct.FormModel);
                        var propNamesMaternityBenefitAct = pairsMaternityBenefitAct.Where(x => x.PropertyName.EndsWith("ViolationExist") && x.Value == "HAS_VIOLATION").Select(x => x.PropertyName.Replace("_ViolationExist", "")).ToArray();
                        // Create a table for displaying inspection data
                        PdfPTable inspectionMaternityBenefitAct = new PdfPTable(2)
                        {
                            WidthPercentage = 100
                        };
                        if (propNamesMaternityBenefitAct.Count() > 0)
                        {
                            foreach (var propName in propNamesMaternityBenefitAct)
                            {
                                var title = pairsMaternityBenefitAct.Where(x => x.PropertyName == propName).Select(x => x.Title).FirstOrDefault();
                                var remarks = pairsMaternityBenefitAct.Where(x => x.PropertyName == propName + "_Remarks").Select(x => x.Value).FirstOrDefault();

                                inspectionMaternityBenefitAct.AddCell(CreateCell(title, 10, true));
                                inspectionMaternityBenefitAct.AddCell(CreateCell(remarks, 10, false));

                                inspectionMaternityBenefitAct.AddCell(new PdfPCell(new Phrase(""))
                                {
                                    FixedHeight = 10, // Adjust height for spacing
                                    Border = PdfPCell.NO_BORDER // No border for spacing cell
                                });

                                // Add another empty cell to create a gap
                                inspectionMaternityBenefitAct.AddCell(new PdfPCell(new Phrase(""))
                                {
                                    FixedHeight = 10,
                                    Border = PdfPCell.NO_BORDER
                                });
                            }
                            documents.Add(new Chunk(" ")); // Add space
                            documents.Add(new Paragraph("Maternity-Benefit Act", FontFactory.GetFont("Times-Roman", 16, Font.BOLD)) { Alignment = Element.ALIGN_CENTER });
                            documents.Add(new Chunk(""));
                            documents.Add(inspectionMaternityBenefitAct);
                        }


                        var inspectionFormLabourIIIContractLabourAct = await _iInspectionService.GetInspectionFormLabourPartIIIContractLabourAct(inspectionRefId);
                        var pairsContractLabourAct = ModelOps.GetTitleAndValues<Inspection_Form_Labour_III_ContractLabourAct>(inspectionFormLabourIIIContractLabourAct.FormModel);
                        var propNamesContractLabourAct = pairsContractLabourAct.Where(x => x.PropertyName.EndsWith("ViolationExist") && x.Value == "HAS_VIOLATION").Select(x => x.PropertyName.Replace("_ViolationExist", "")).ToArray();
                        // Create a table for displaying inspection data
                        PdfPTable inspectionContractLabourAct = new PdfPTable(2)
                        {
                            WidthPercentage = 100
                        };
                        if (propNamesContractLabourAct.Count() > 0)
                        {
                            foreach (var propName in propNamesContractLabourAct)
                            {
                                var title = pairsContractLabourAct.Where(x => x.PropertyName == propName).Select(x => x.Title).FirstOrDefault();
                                var remarks = pairsContractLabourAct.Where(x => x.PropertyName == propName + "_Remarks").Select(x => x.Value).FirstOrDefault();

                                inspectionContractLabourAct.AddCell(CreateCell(title, 10, true));
                                inspectionContractLabourAct.AddCell(CreateCell(remarks, 10, false));

                                inspectionContractLabourAct.AddCell(new PdfPCell(new Phrase(""))
                                {
                                    FixedHeight = 10, // Adjust height for spacing
                                    Border = PdfPCell.NO_BORDER // No border for spacing cell
                                });

                                // Add another empty cell to create a gap
                                inspectionContractLabourAct.AddCell(new PdfPCell(new Phrase(""))
                                {
                                    FixedHeight = 10,
                                    Border = PdfPCell.NO_BORDER
                                });
                            }
                            documents.Add(new Chunk(" ")); // Add space
                            documents.Add(new Paragraph("Contract-Labour Act", FontFactory.GetFont("Times-Roman", 16, Font.BOLD)) { Alignment = Element.ALIGN_CENTER });
                            documents.Add(new Chunk(""));
                            documents.Add(inspectionContractLabourAct);
                        }


                        var inspectionFormLabourIIIInterStateMigrantWorkmenAct = await _iInspectionService.GetInspectionFormLabourPartIIIInterStateMigrantWorkmenAct(inspectionRefId);
                        var pairsInterStateMigrantWorkmenAct = ModelOps.GetTitleAndValues<Inspection_Form_Labour_III_InterStateMigrantWorkmenAct>(inspectionFormLabourIIIInterStateMigrantWorkmenAct.FormModel);
                        var propNamesInterStateMigrantWorkmenAct = pairsInterStateMigrantWorkmenAct.Where(x => x.PropertyName.EndsWith("ViolationExist") && x.Value == "HAS_VIOLATION").Select(x => x.PropertyName.Replace("_ViolationExist", "")).ToArray();
                        // Create a table for displaying inspection data
                        PdfPTable inspectionInterStateMigrantWorkmenAct = new PdfPTable(2)
                        {
                            WidthPercentage = 100
                        };
                        if (propNamesInterStateMigrantWorkmenAct.Count() > 0)
                        {
                            foreach (var propName in propNamesInterStateMigrantWorkmenAct)
                            {
                                var title = pairsInterStateMigrantWorkmenAct.Where(x => x.PropertyName == propName).Select(x => x.Title).FirstOrDefault();
                                var remarks = pairsInterStateMigrantWorkmenAct.Where(x => x.PropertyName == propName + "_Remarks").Select(x => x.Value).FirstOrDefault();

                                inspectionInterStateMigrantWorkmenAct.AddCell(CreateCell(title, 10, true));
                                inspectionInterStateMigrantWorkmenAct.AddCell(CreateCell(remarks, 10, false));

                                inspectionInterStateMigrantWorkmenAct.AddCell(new PdfPCell(new Phrase(""))
                                {
                                    FixedHeight = 10, // Adjust height for spacing
                                    Border = PdfPCell.NO_BORDER // No border for spacing cell
                                });

                                // Add another empty cell to create a gap
                                inspectionInterStateMigrantWorkmenAct.AddCell(new PdfPCell(new Phrase(""))
                                {
                                    FixedHeight = 10,
                                    Border = PdfPCell.NO_BORDER
                                });

                            }
                            documents.Add(new Chunk(" ")); // Add space
                            documents.Add(new Paragraph("InterState Migrant Workmen Act", FontFactory.GetFont("Times-Roman", 16, Font.BOLD)) { Alignment = Element.ALIGN_CENTER });
                            documents.Add(new Chunk(""));
                            documents.Add(inspectionInterStateMigrantWorkmenAct);
                        }


                        var inspectionFormLabourIIILabourWelfareFundAct = await _iInspectionService.GetInspectionFormLabourPartIIILabourWelfareFund_Act(inspectionRefId);
                        var pairsLabourWelfareFundAct = ModelOps.GetTitleAndValues<Inspection_Form_Labour_III_LabourWelfareFund_Act>(inspectionFormLabourIIILabourWelfareFundAct.FormModel);
                        var propNamesLabourWelfareFundAct = pairsLabourWelfareFundAct.Where(x => x.PropertyName.EndsWith("ViolationExist") && x.Value == "HAS_VIOLATION").Select(x => x.PropertyName.Replace("_ViolationExist", "")).ToArray();
                        // Create a table for displaying inspection data
                        PdfPTable inspectionLabourWelfareFundAct = new PdfPTable(2)
                        {
                            WidthPercentage = 100
                        };
                        if (propNamesLabourWelfareFundAct.Count() > 0)
                        {
                            foreach (var propName in propNamesLabourWelfareFundAct)
                            {
                                var title = pairsLabourWelfareFundAct.Where(x => x.PropertyName == propName).Select(x => x.Title).FirstOrDefault();
                                var remarks = pairsLabourWelfareFundAct.Where(x => x.PropertyName == propName + "_Remarks").Select(x => x.Value).FirstOrDefault();

                                inspectionLabourWelfareFundAct.AddCell(CreateCell(title, 10, true));
                                inspectionLabourWelfareFundAct.AddCell(CreateCell(remarks, 10, false));

                                inspectionLabourWelfareFundAct.AddCell(new PdfPCell(new Phrase(""))
                                {
                                    FixedHeight = 10, // Adjust height for spacing
                                    Border = PdfPCell.NO_BORDER // No border for spacing cell
                                });

                                // Add another empty cell to create a gap
                                inspectionLabourWelfareFundAct.AddCell(new PdfPCell(new Phrase(""))
                                {
                                    FixedHeight = 10,
                                    Border = PdfPCell.NO_BORDER
                                });

                            }
                            documents.Add(new Chunk(" ")); // Add space
                            documents.Add(new Paragraph("Labour Welfare Fund Act", FontFactory.GetFont("Times-Roman", 16, Font.BOLD)) { Alignment = Element.ALIGN_CENTER });
                            documents.Add(new Chunk(""));
                            documents.Add(inspectionLabourWelfareFundAct);
                        }


                        var inspectionFormLabourIIIGratuityAct = await _iInspectionService.GetInspectionFormLabourPartIIIGratuityAct(inspectionRefId);
                        var pairsGratuityAct = ModelOps.GetTitleAndValues<Inspection_Form_Labour_III_GratuityAct>(inspectionFormLabourIIIGratuityAct.FormModel);
                        var propNamesGratuityAct = pairsGratuityAct.Where(x => x.PropertyName.EndsWith("ViolationExist") && x.Value == "HAS_VIOLATION").Select(x => x.PropertyName.Replace("_ViolationExist", "")).ToArray();
                        // Create a table for displaying inspection data
                        PdfPTable inspectionGratuityAct = new PdfPTable(2)
                        {
                            WidthPercentage = 100
                        };
                        if (propNamesGratuityAct.Count() > 0)
                        {
                            foreach (var propName in propNamesGratuityAct)
                            {
                                var title = pairsGratuityAct.Where(x => x.PropertyName == propName).Select(x => x.Title).FirstOrDefault();
                                var remarks = pairsGratuityAct.Where(x => x.PropertyName == propName + "_Remarks").Select(x => x.Value).FirstOrDefault();

                                inspectionGratuityAct.AddCell(CreateCell(title, 10, true));
                                inspectionGratuityAct.AddCell(CreateCell(remarks, 10, false));

                                inspectionGratuityAct.AddCell(new PdfPCell(new Phrase(""))
                                {
                                    FixedHeight = 10, // Adjust height for spacing
                                    Border = PdfPCell.NO_BORDER // No border for spacing cell
                                });

                                // Add another empty cell to create a gap
                                inspectionGratuityAct.AddCell(new PdfPCell(new Phrase(""))
                                {
                                    FixedHeight = 10,
                                    Border = PdfPCell.NO_BORDER
                                });

                            }
                            documents.Add(new Chunk(" ")); // Add space
                            documents.Add(new Paragraph("Gratuity Act", FontFactory.GetFont("Times-Roman", 16, Font.BOLD)) { Alignment = Element.ALIGN_CENTER });
                            documents.Add(new Chunk(""));
                            documents.Add(inspectionGratuityAct);
                        }


                        var inspectionFormLabourIIIIndustrialEmploymentAct = await _iInspectionService.GetInspectionFormLabourPartIIIIndustrialEmploymentAct(inspectionRefId);
                        var pairsIndustrialEmploymentAct = ModelOps.GetTitleAndValues<Inspection_Form_Labour_III_IndustrialEmploymentAct>(inspectionFormLabourIIIIndustrialEmploymentAct.FormModel);
                        var propIndustrialEmploymentAct = pairsIndustrialEmploymentAct.Where(x => x.PropertyName.EndsWith("ViolationExist") && x.Value == "HAS_VIOLATION").Select(x => x.PropertyName.Replace("_ViolationExist", "")).ToArray();
                        // Create a table for displaying inspection data
                        PdfPTable inspectionIndustrialEmploymentAct = new PdfPTable(2)
                        {
                            WidthPercentage = 100
                        };
                        if (propIndustrialEmploymentAct.Count() > 0)
                        {
                            foreach (var propName in propIndustrialEmploymentAct)
                            {
                                var title = pairsIndustrialEmploymentAct.Where(x => x.PropertyName == propName).Select(x => x.Title).FirstOrDefault();
                                var remarks = pairsIndustrialEmploymentAct.Where(x => x.PropertyName == propName + "_Remarks").Select(x => x.Value).FirstOrDefault();

                                inspectionIndustrialEmploymentAct.AddCell(CreateCell(title, 10, true));
                                inspectionIndustrialEmploymentAct.AddCell(CreateCell(remarks, 10, false));

                                inspectionIndustrialEmploymentAct.AddCell(new PdfPCell(new Phrase(""))
                                {
                                    FixedHeight = 10, // Adjust height for spacing
                                    Border = PdfPCell.NO_BORDER // No border for spacing cell
                                });

                                // Add another empty cell to create a gap
                                inspectionIndustrialEmploymentAct.AddCell(new PdfPCell(new Phrase(""))
                                {
                                    FixedHeight = 10,
                                    Border = PdfPCell.NO_BORDER
                                });

                            }
                            documents.Add(new Chunk(" ")); // Add space
                            documents.Add(new Paragraph("Industrial Employment Act", FontFactory.GetFont("Times-Roman", 16, Font.BOLD)) { Alignment = Element.ALIGN_CENTER });
                            documents.Add(new Chunk(""));
                            documents.Add(inspectionIndustrialEmploymentAct);
                        }


                        var inspectionFormLabourIIIBOCWAct = await _iInspectionService.GetInspectionFormLabourPartIIIBOCW_Act(inspectionRefId);
                        var pairsBOCWAct = ModelOps.GetTitleAndValues<Inspection_Form_Labour_III_BOCW_Act>(inspectionFormLabourIIIBOCWAct.FormModel);
                        var propBOCWAct = pairsBOCWAct.Where(x => x.PropertyName.EndsWith("ViolationExist") && x.Value == "HAS_VIOLATION").Select(x => x.PropertyName.Replace("_ViolationExist", "")).ToArray();
                        // Create a table for displaying inspection data
                        PdfPTable inspectionBOCWAct = new PdfPTable(2)
                        {
                            WidthPercentage = 100
                        };
                        if (propBOCWAct.Count() > 0)
                        {
                            foreach (var propName in propBOCWAct)
                            {
                                var title = pairsBOCWAct.Where(x => x.PropertyName == propName).Select(x => x.Title).FirstOrDefault();
                                var remarks = pairsBOCWAct.Where(x => x.PropertyName == propName + "_Remarks").Select(x => x.Value).FirstOrDefault();

                                inspectionBOCWAct.AddCell(CreateCell(title, 10, true));
                                inspectionBOCWAct.AddCell(CreateCell(remarks, 10, false));

                                inspectionBOCWAct.AddCell(new PdfPCell(new Phrase(""))
                                {
                                    FixedHeight = 10, // Adjust height for spacing
                                    Border = PdfPCell.NO_BORDER // No border for spacing cell
                                });

                                // Add another empty cell to create a gap
                                inspectionBOCWAct.AddCell(new PdfPCell(new Phrase(""))
                                {
                                    FixedHeight = 10,
                                    Border = PdfPCell.NO_BORDER
                                });

                            }
                            documents.Add(new Chunk(" ")); // Add space
                            documents.Add(new Paragraph("BOCW Act", FontFactory.GetFont("Times-Roman", 16, Font.BOLD)) { Alignment = Element.ALIGN_CENTER });
                            documents.Add(new Chunk(""));
                            documents.Add(inspectionBOCWAct);
                        }

                        if (inspectionMasterData.InspectionSubmitOn_Labour_Wing < DateTime.Parse("2024-10-10"))
                        {
                            var inspectionFormLabourIIIShopAct = await _iInspectionService.GetInspectionFormLabourPartIIIShopAct(inspectionRefId);
                            var pairsShopAct = ModelOps.GetTitleAndValues<Inspection_Form_Labour_III_ShopAct>(inspectionFormLabourIIIShopAct.FormModel);

                            // Create a table for displaying inspection data
                            PdfPTable inspectionShopAct = new PdfPTable(2)
                            {
                                WidthPercentage = 100
                            };
                            foreach (var item in pairsShopAct)
                            {
                                inspectionShopAct.AddCell(CreateCell(item.Title, 10, true));
                                inspectionShopAct.AddCell(CreateCell(item.Value, 10, false));

                            }
                            documents.Add(new Chunk(" ")); // Add space
                            documents.Add(new Paragraph("Shop Act", FontFactory.GetFont("Times-Roman", 16, Font.BOLD)) { Alignment = Element.ALIGN_CENTER });
                            documents.Add(new Chunk(""));
                            documents.Add(inspectionShopAct);
                        }

                        var inspectionFormLabourIIIObservations = await _iInspectionService.GetInspectionFormLabourPartIIIObservations(inspectionRefId);
                        var pairsObservations = ModelOps.GetTitleAndValues<Inspection_Form_Labour_III_Observations>(inspectionFormLabourIIIObservations.FormModel);
                        var propObservationsAct = pairsObservations.Where(x => x.PropertyName.EndsWith("ViolationExist") && x.Value == "HAS_VIOLATION").Select(x => x.PropertyName.Replace("_ViolationExist", "")).ToArray();

                        // Create a table for displaying inspection data
                        PdfPTable inspectionObservations = new PdfPTable(2)
                        {
                            WidthPercentage = 100
                        };

                        if (propObservationsAct.Count() > 0)
                        {
                            foreach (var propName in propObservationsAct)
                            {
                                var title = pairsObservations.Where(x => x.PropertyName == propName).Select(x => x.Title).FirstOrDefault();
                                var remarks = pairsObservations.Where(x => x.PropertyName == propName + "_Remarks").Select(x => x.Value).FirstOrDefault();

                                inspectionObservations.AddCell(CreateCell(title, 10, true));
                                inspectionObservations.AddCell(CreateCell(remarks, 10, false));

                                inspectionObservations.AddCell(new PdfPCell(new Phrase(""))
                                {
                                    FixedHeight = 10, // Adjust height for spacing
                                    Border = PdfPCell.NO_BORDER // No border for spacing cell
                                });

                                // Add another empty cell to create a gap
                                inspectionObservations.AddCell(new PdfPCell(new Phrase(""))
                                {
                                    FixedHeight = 10,
                                    Border = PdfPCell.NO_BORDER
                                });

                            }
                            documents.Add(new Chunk(" ")); // Add space
                            documents.Add(new Paragraph("Observations", FontFactory.GetFont("Times-Roman", 16, Font.BOLD)) { Alignment = Element.ALIGN_CENTER });
                            documents.Add(new Chunk(""));
                            documents.Add(inspectionObservations);
                        }
                    }
                    else
                    {

                        // Create a table for displaying inspection data
                        PdfPTable inspectionTable = new PdfPTable(2)
                        {
                            WidthPercentage = 100
                        };

                        inspectionTable.AddCell(CreateCell("Factory Name", 10, true));
                        inspectionTable.AddCell(CreateCell(establishmentDetails.ResponseDataModel.EstablishmentName, 10, false));

                        inspectionTable.AddCell(CreateCell("Factory Address", 10, true));
                        inspectionTable.AddCell(CreateCell(establishmentDetails.ResponseDataModel.EstablishmentAddress, 10, false));

                        inspectionTable.AddCell(CreateCell("Factory Status", 10, true));
                        inspectionTable.AddCell(CreateCell(EnumOps.GetEnumDescriptionName<InspectionFactoryExistenceTypeEnum>(Enum.GetName(typeof(InspectionFactoryExistenceTypeEnum), inspectionMasterData.InspectionSubmited_Labour_Wing_FactoryExistenceType)), 10, false));

                        inspectionTable.AddCell(CreateCell("Factory De-Registration Number", 10, true));
                        inspectionTable.AddCell(CreateCell(inspectionMasterData.FactoryDeRegistrationNo, 10, false));

                        inspectionTable.AddCell(CreateCell("Inspection Date", 10, true));
                        inspectionTable.AddCell(CreateCell(inspectionMasterData.InspectionDoneOn_Labour_Wing.ToString(), 10, false));

                        inspectionTable.AddCell(CreateCell("Remarks", 10, true));
                        inspectionTable.AddCell(CreateCell(inspectionMasterData.Remarks.ToString(), 10, false));

                        inspectionTable.AddCell(new PdfPCell(new Phrase(""))
                        {
                            FixedHeight = 10, // Adjust height for spacing
                            Border = PdfPCell.NO_BORDER // No border for spacing cell
                        });

                        // Add another empty cell to create a gap
                        inspectionTable.AddCell(new PdfPCell(new Phrase(""))
                        {
                            FixedHeight = 10,
                            Border = PdfPCell.NO_BORDER
                        });

                        documents.Add(new Chunk(" "));
                        documents.Add(inspectionTable);

                        Paragraph SignSeal2 = new Paragraph()
                        {
                            Alignment = Element.ALIGN_CENTER, // Justify the text
                            SpacingBefore = 10f, // Margin-top before the text
                            SpacingAfter = 10f, // Margin-bottom after the text
                            IndentationLeft = 10f,   // Padding left
                            IndentationRight = 10f   // Padding right

                        };
                        Font redFont = FontFactory.GetFont("Times-Roman", 14, Font.NORMAL, BaseColor.RED);

                        // Add the text with the specified font
                        SignSeal2.Add(new Chunk("No Violation found..!!", redFont));
                        documents.Add(SignSeal2);
                    }

                    var officerDetails = _context.Inspection_Master.Where(x => x.InspectionId == inspectionRefId).FirstOrDefault();
                    var officerUserRole = _context.Users.Where(x => x.Id == officerDetails.InspectionSubmited_Labour_Wing_UserId && x.IsEnabled == true).Include(x => x.UserRoles).ThenInclude(x => x.Role).FirstOrDefault();
                    var circleName = _context.LabourCircles.Where(x => x.LabourCircleId == officerDetails.LabourCircleRefId && x.Version == 2);
                    //var userDetails = await _iAuthService.GetUserFullDetailByUserName(officerUserRole.UserName);
                    var userDetails = await _iAuthService.GetUserProfileByProfileId(officerDetails.InspectionSubmited_Labour_Wing_ProfileId);
                    // string officerSigFilePath = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "OfficerSignatures")).Root + userDetails.ListData[0].Signature;

                    //iTextSharp.text.Image signature = iTextSharp.text.Image.GetInstance(officerSigFilePath);
                    //signature.ScaleToFit(120f, 120f);
                    //signature.SetAbsolutePosition(450f, 150f); // Adjust position as needed

                    PdfPTable officerDetailsTable = new PdfPTable(2)
                    {
                        WidthPercentage = 100
                    };
                    Font timesRoman = FontFactory.GetFont("Times-Roman", 9, Font.BOLD);
                    Paragraph SignSeal = new Paragraph()
                    {
                        Alignment = Element.ALIGN_RIGHT, // Justify the text
                        SpacingBefore = 10f, // Margin-top before the text
                        SpacingAfter = 10f, // Margin-bottom after the text
                        IndentationLeft = 10f,   // Padding left
                        IndentationRight = 10f   // Padding right
                    };
                    SignSeal.Add(new Chunk("Name: ", timesRoman));
                    SignSeal.Add(new Chunk(userDetails.FirstName + " " + userDetails.LastName, timesRoman));
                    SignSeal.Add(new Chunk(Chunk.NEWLINE + "Desig: ", timesRoman));
                    SignSeal.Add(new Chunk(officerUserRole.UserRoles.FirstOrDefault().Role.NormalizedName, timesRoman));
                    SignSeal.Add(new Chunk(Chunk.NEWLINE + "Circle Name: ", timesRoman));
                    SignSeal.Add(new Chunk(circleName.FirstOrDefault().LabourCircleName, timesRoman));


                    //PdfPTable ptablerightSideDetail = new PdfPTable(1);
                    //ptablerightSideDetail.DefaultCell.Border = 0;
                    //ptablerightSideDetail.DefaultCell.Padding = 0;

                    //ptablerightSideDetail.AddCell(new PdfPCell(signature) { Border = 0, PaddingTop = 5, HorizontalAlignment = PdfPCell.ALIGN_RIGHT });

                    // Create a table to align signature and details
                    PdfPTable officerTable = new PdfPTable(2)
                    {
                        WidthPercentage = 100
                    };
                    //documents.Add(ptablerightSideDetail);
                    documents.Add(SignSeal);
                    documents.Close();
                    // Construct the file path
                    string filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Inspections", "Violation_" + inspectionRefId + "_" + inspectionType + ".pdf");

                    string pdfFilePath = filePath;
                    Byte[] bytes = File.ReadAllBytes(filePath);
                    string file = System.Convert.ToBase64String(bytes);
                    genericFormModel.PdfContent = file;
                    genericFormModel.FileNo = "Violation_" + inspectionRefId + "_" + inspectionType;
                }
            }
            catch (Exception ex)
            {
                genericFormModel.HasException = true;
                genericFormModel.Exceptions = ex.Message;
            }
            return genericFormModel;
        }

        private async Task<CertificateGenerateServiceResultTemplate> Certificate_AppForm_39(Int64 appRefId, string CertificatePerformaURL, ApplicationTypeEnum applicationType, string userName, string licenceNo, bool isDeemedCalling)
        {
            CertificateGenerateServiceResultTemplate genericFormModel = new CertificateGenerateServiceResultTemplate();
            try
            {
                Guid guId = Guid.NewGuid();
                genericFormModel.PdfNameGUID = guId.ToString();
                var sfilepath = "";
                sfilepath = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "TempFiles")).Root + guId.ToString() + ".pdf";
                FileInfo file = new FileInfo(sfilepath);
                if (file.Exists)
                {
                    file.Delete();
                }
                iTextSharp.text.Document documents = new iTextSharp.text.Document(iTextSharp.text.PageSize.A4, 5, 5, 30, 30);
                documents.Open();
                #region ClassObjects
                pdfFonts classFonts = new pdfFonts();
                #endregion

                #region CreateObjects
                PdfWriter owriter = PdfWriter.GetInstance(documents, new FileStream(sfilepath, FileMode.Create));
                PDFPageEvents pageEvent = new PDFPageEvents();
                owriter.PageEvent = pageEvent;
                owriter.SetEncryption(PdfWriter.STRENGTH128BITS, "", "", PdfWriter.AllowPrinting);
                #endregion

                var fontName = "Verdana";
                if (!FontFactory.IsRegistered(fontName))
                {
                    var fontPath = "Content/fonts/VERDANA.TTF";
                    FontFactory.Register(fontPath);
                }

                #region Tables

                var parentWithChildObject = await _iGR_Licence_PE_ISM_GeneralDetail
                       .GetAsync(x => x.AppRefId == appRefId,  //Conditions         
                         null,                //Orders
                         x => x.Application, x => x.Application.ApplicationAction, x => x.Licence_PE_ISM_Contrators)      //Includes
                       .ConfigureAwait(false);
                var principalEmployerLicence = parentWithChildObject.FirstOrDefault();
                var projectSite = _context.ProjectSites.Where(x => x.ProjectSiteId == principalEmployerLicence.Application.ProjectSiteRefId).FirstOrDefault();
                var alcCircle = _context.ALCCircles.Where(x => x.ALCCircleId == projectSite.AlcCircleRefId).FirstOrDefault();
                #endregion

                #region Licence Validity Year
                int licenceValidUpto = 0;
                DofLicenceDetailsViewModel dofDetails = new DofLicenceDetailsViewModel();
                if (principalEmployerLicence.Application.ApplicationPurposeType == ApplicationPurposeTypeEnum.GRANT_LICENCE)
                {
                    DateTime registrationDate = DateTime.MinValue;
                    licenceValidUpto = 1;
                }
                #endregion

                string fontpath1 = "Content/fonts/VERDANA.TTF";
                BaseFont customfont = BaseFont.CreateFont(fontpath1, BaseFont.CP1252, BaseFont.EMBEDDED);
                string fontpath2 = "Content/fonts/VERDANA.TTF";
                BaseFont customfontimpact = BaseFont.CreateFont(fontpath2, BaseFont.CP1252, BaseFont.EMBEDDED);
                documents.Open();
                Font verdanaHeader = FontFactory.GetFont("Arial", 9, Font.BOLD);
                Font footer = FontFactory.GetFont("Arial", 9, Font.BOLD, BaseColor.BLUE);
                Font verdana = FontFactory.GetFont("Arial", 8, BaseColor.BLACK);
                Font verdanaBoldItalic = FontFactory.GetFont("Arial", 8, Font.BOLDITALIC);
                Font verdanasmall = FontFactory.GetFont("Arial", 8, Font.NORMAL);
                verdanasmall.Color = BaseColor.WHITE;
                Font verdanaRightHeader = FontFactory.GetFont("Arial", 15, Font.BOLD);
                verdanaRightHeader.Color = BaseColor.WHITE;
                Font verdanaRightHeader1 = FontFactory.GetFont("Arial", 12, Font.BOLD);
                verdanaRightHeader1.Color = BaseColor.WHITE;
                Font arialOldLicenceDetails = FontFactory.GetFont("Arial", 10, Font.BOLD);
                arialOldLicenceDetails.Color = BaseColor.WHITE;
                Font monotypeHeader = new Font(customfont, 20);
                Font monotype = FontFactory.GetFont("Arial", 13, Font.BOLD);
                Font impact = new Font(customfontimpact, 12);
                Font impactHeader = new Font(customfontimpact, 11, Font.BOLD);
                impactHeader.Color = BaseColor.WHITE;
                BaseFont bfTimes = BaseFont.CreateFont(BaseFont.TIMES_ROMAN, BaseFont.CP1252, false);
                Font timesRoman = FontFactory.GetFont("Arial", 10, BaseColor.BLACK);
                Font ArialFont = FontFactory.GetFont("Arial", 8, BaseColor.WHITE);
                Font SealFontArial = FontFactory.GetFont("Arial", 9, BaseColor.BLACK);
                Font verdanaBold = FontFactory.GetFont("Arial", 10, Font.BOLD);
                Font verdanalic = FontFactory.GetFont("Arial", 9, Font.BOLD);
                verdanalic.Color = BaseColor.WHITE;
                var principalEmployerPerforma = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "LicensePerformas")).Root + "CertificatePerforma_Principal_Employer.jpg";
                iTextSharp.text.Image pageLogo1 = iTextSharp.text.Image.GetInstance(new FileStream(principalEmployerPerforma, FileMode.OpenOrCreate, FileAccess.Read));

                pageLogo1.Alignment = iTextSharp.text.Image.ALIGN_CENTER | iTextSharp.text.Image.UNDERLYING;
                pageLogo1.ScaleToFit(800f, 800f);
                pageLogo1.SetAbsolutePosition(10f, 25f);
                pageLogo1.BorderWidth = 1;
                documents.Add(pageLogo1);
                PdfPTable ptableleftSideDetail = new PdfPTable(1);
                ptableleftSideDetail.DefaultCell.Border = 0;
                ptableleftSideDetail.DefaultCell.Padding = 0;
                ptableleftSideDetail.HorizontalAlignment = (Element.ALIGN_JUSTIFIED);
                ptableleftSideDetail.DefaultCell.VerticalAlignment = (Element.ALIGN_JUSTIFIED);
                ptableleftSideDetail.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
                ptableleftSideDetail.AddCell(new PdfPCell(new Phrase("", verdanaBold)) { Border = 0, PaddingTop = 10 });


                ptableleftSideDetail.AddCell(new PdfPCell(new Phrase("Registration No. : " + licenceNo.ToString(), verdanalic)) { Border = 0, PaddingTop = 300, PaddingLeft = -18 }); ;
                ptableleftSideDetail.AddCell(new PdfPCell(new Phrase("Registration Issued On : " + String.Format("{0:dd-MMM-yyyy}", DateTime.Now.ToShortDateString()), verdanalic)) { Border = 0, PaddingTop = 10, PaddingLeft = -18 });
                Font verdanaBoldBP = new Font(BaseFont.CreateFont(BaseFont.TIMES_ROMAN, BaseFont.CP1252, false), 8, Font.BOLD | Font.UNDERLINE, BaseColor.WHITE);
                Phrase phrase = new Phrase();

                ptableleftSideDetail.AddCell(new PdfPCell(phrase) { Border = 0, PaddingTop = 10, PaddingRight = 20 });

                #region QR Code Generate
                var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(principalEmployerLicence.Application.PublicAppRefNum);
                //string base64String = System.Convert.ToBase64String(plainTextBytes);
                var qrCodeDetails = "Establishment Name : " + projectSite.EstablishmentName + "|" + "Establishment Address : " + projectSite.Address + "|" + "IPIN : " + principalEmployerLicence.Application.InvestPunjab_Ipin + "|" + "Contact No : " + projectSite.ContactPersonFirstName + " " + projectSite.ContactPersonLastName + "|" + projectSite.ContactPersonMobileNo + "|" + "Licence No : " + licenceNo + "|" + "File No : " + principalEmployerLicence.Application.PublicAppRefNum;
                QRCodeGenerator qrGenerator = new QRCodeGenerator();
                QRCodeGenerator.QRCode qrCode = qrGenerator.CreateQrCode(qrCodeDetails, QRCodeGenerator.ECCLevel.Q);

                using (System.Drawing.Bitmap bitMap = qrCode.GetGraphic(20))
                {
                    using (MemoryStream ms = new MemoryStream())
                    {
                        bitMap.Save(ms, ImageFormat.Jpeg);
                        System.Drawing.Image imgQRCode = System.Drawing.Image.FromStream(ms);
                        iTextSharp.text.Image imdf = iTextSharp.text.Image.GetInstance(imgQRCode, System.Drawing.Imaging.ImageFormat.Jpeg);
                        imdf.ScaleAbsoluteHeight(120f);
                        imdf.ScaleAbsoluteWidth(120f);
                        ptableleftSideDetail.AddCell(new PdfPCell(imdf) { Border = 0, PaddingTop = 30, PaddingLeft = 0, PaddingRight = 0 });
                    }
                }
                #endregion

                ptableleftSideDetail.AddCell(new PdfPCell(new Phrase("" + Chunk.NEWLINE + "Department of Labour, Govt. of Punjab" + Chunk.NEWLINE, verdanaRightHeader1)) { Border = 0, PaddingTop = 15, PaddingRight = 25, HorizontalAlignment = PdfPCell.ALIGN_CENTER });

                PdfPTable ptablerightSideDetail = new PdfPTable(1);
                ptablerightSideDetail.DefaultCell.Border = 0;
                ptablerightSideDetail.DefaultCell.Padding = 0;

                //ptablerightSideDetail.AddCell(new PdfPCell(new Phrase("", verdanaBold)) { Border = 0, Padding = 4 });
                ptablerightSideDetail.AddCell(new PdfPCell(new Phrase("FORM-II", verdanaHeader)) { Border = 0, PaddingLeft = 20, PaddingTop = 10, HorizontalAlignment = PdfPCell.ALIGN_CENTER });
                ptablerightSideDetail.AddCell(new PdfPCell(new Phrase("[(See Rule-4 (1))]", verdanaHeader)) { Border = 0, PaddingLeft = 20, HorizontalAlignment = PdfPCell.ALIGN_CENTER });
                ptablerightSideDetail.AddCell(new PdfPCell(new Phrase("REGISTRATION OF PRINCIPAL EMPLOYER INTER-STATE MIGRANT WORKMEN UNDER THE INTER STATE MIGRANT WORKMEN ACT, 1979 ", impactHeader)) { Border = 0, PaddingTop = 12, PaddingBottom = 20, PaddingLeft = 40, PaddingRight = 30, VerticalAlignment = PdfPCell.ALIGN_MIDDLE, HorizontalAlignment = PdfPCell.ALIGN_CENTER });
                ptablerightSideDetail.AddCell(new PdfPCell(new Phrase("CERTIFICATE OF REGISTRATION", monotype)) { Border = 0, PaddingBottom = 4, PaddingTop = 3, HorizontalAlignment = PdfPCell.ALIGN_CENTER });
                ptablerightSideDetail.AddCell(new PdfPCell(new Phrase("GOVERNMENT OF PUNJAB", monotype)) { Border = 0, PaddingBottom = 4, HorizontalAlignment = PdfPCell.ALIGN_CENTER });
                ptablerightSideDetail.AddCell(new PdfPCell(new Phrase("", verdanaBold)) { Border = 0, Padding = 1, PaddingTop = 6 });
                ptablerightSideDetail.AddCell(new PdfPCell(new Phrase(" Dated: " + DateTime.Now.ToShortDateString() + "                            Registration No. : " + licenceNo.ToString(), verdanaHeader)) { Border = 0 });

                Paragraph para1 = new Paragraph();
                para1.Add(new Chunk("        A Certificate of Registration containing the following particulars is hereby granted under clause (a) of sub-section (2) of section 4 of The Inter-State Migrant Workmen (Regulation of the Employment and conditions of Service) Act, 1979 and the Rules made thereunder to  - ", timesRoman));
                para1.Add(new Chunk(projectSite.EstablishmentName + " ," + projectSite.Address, verdanaBold));

                /*para1.Add(new Chunk(" having the following particulars subject to the conditions laid down in Annexure:-", timesRoman));
                para1.Add(new Chunk(Chunk.NEWLINE + "", verdanaBoldItalic));*/
                para1.Add(new Chunk(Chunk.NEWLINE + "1.  Nature of work carried on in the establishment: :  ", timesRoman));
                para1.Add(new Chunk(principalEmployerLicence.NatureOfWork, verdanaBold));

                para1.Add(new Chunk(Chunk.NEWLINE + "2. Name and Addresses of the Principal Employer :  ", timesRoman));
                para1.Add(new Chunk(principalEmployerLicence.PE_Name + " ," + principalEmployerLicence.PE_Address, verdanaBold));

                para1.Add(new Chunk(Chunk.NEWLINE + "3. Nature of the work in which contract labour is employed or is to be employed: ", timesRoman));
                para1.Add(new Chunk(" (Enclosed  Annexure)", verdanaBold));
                para1.Add(new Chunk(Chunk.NEWLINE + "4. Maximum  number of contract labour to be employed on any day through each contractor: ", timesRoman));
                para1.Add(new Chunk(" (Enclosed  Annexure)", verdanaBold));
                para1.Add(new Chunk(Chunk.NEWLINE + "5. Other particulars relevant to the employment of contract labour.", timesRoman));
                para1.Add(new Chunk(principalEmployerLicence.NatureOfWork, verdanaBold));
                PdfPCell shipment1 = new PdfPCell(para1);
                shipment1.SetLeading(1f, 2f);
                ptablerightSideDetail.AddCell(new PdfPCell(shipment1) { Border = 0, HorizontalAlignment = PdfPCell.ALIGN_JUSTIFIED, VerticalAlignment = PdfPCell.ALIGN_JUSTIFIED, FixedHeight = 420, PaddingBottom = 15 });
                var userDetails = await _iAuthService.GetUserFullDetailByUserName(userName);
                string officerSigFilePath = "";
                if (!isDeemedCalling)
                {

                    if (userDetails.ListData.Count() == 0 || userDetails.ListData[0].Signature == null)
                    {
                        userDetails.ListData[0].Signature = "abc.jpg";
                    }
                    officerSigFilePath = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "OfficerSignatures")).Root + userDetails.ListData[0].Signature;
                    iTextSharp.text.Image signature = iTextSharp.text.Image.GetInstance(new FileStream(officerSigFilePath, FileMode.OpenOrCreate, FileAccess.Read));
                    signature.ScaleToFit(120f, 120f);
                    signature.SetAbsolutePosition(630f, 150f);
                    ptablerightSideDetail.AddCell(new PdfPCell(signature) { Border = 0, PaddingTop = 5, HorizontalAlignment = PdfPCell.ALIGN_RIGHT });
                }

                Paragraph SignSeal = new Paragraph();
                SignSeal.Add(new Chunk(Chunk.NEWLINE + "Circle Name: ", verdanaHeader));
                SignSeal.Add(new Chunk(alcCircle.ALCCircleName, SealFontArial));
                SignSeal.Add(new Chunk(Chunk.NEWLINE + "District Name: ", verdanaHeader));
                SignSeal.Add(new Chunk(alcCircle.ALCCircleName, SealFontArial));
                SignSeal.Add(new Chunk(Chunk.NEWLINE + "Date: ", verdanaHeader));
                SignSeal.Add(new Chunk(DateTime.Now.ToShortDateString(), SealFontArial));
                SignSeal.Add(new Chunk(Chunk.NEWLINE + "Time: ", verdanaHeader));
                SignSeal.Add(new Chunk(DateTime.Now.ToShortTimeString(), SealFontArial));

                ptablerightSideDetail.AddCell(new PdfPCell(SignSeal) { Border = 0, HorizontalAlignment = PdfPCell.ALIGN_LEFT, PaddingLeft = 200 });
                //ptablerightSideDetail.AddCell(new PdfPCell(new Phrase(" Dated " + DateTime.Now.ToShortDateString(), verdanaHeader)) { Border = 0, HorizontalAlignment = PdfPCell.ALIGN_LEFT});

                ptablerightSideDetail.AddCell(new PdfPCell(new Phrase("This is a computer generated document to check & verify please login to https://pblabour.gov.in/Account/ApplicationStatus", footer)) { Border = 1, BorderWidth = 1, Padding = 8 });
                PdfPTable ptablePrincipalDetailLicenceApplied = new PdfPTable(3);
                ptablePrincipalDetailLicenceApplied.DefaultCell.Border = 0;
                ptablePrincipalDetailLicenceApplied.DefaultCell.Padding = 2;
                ptablePrincipalDetailLicenceApplied.SpacingBefore = 1000;
                ptablePrincipalDetailLicenceApplied.AddCell(new PdfPCell(ptableleftSideDetail) { Border = 0 });
                ptablePrincipalDetailLicenceApplied.AddCell(new PdfPCell(ptablerightSideDetail) { Border = 0, Colspan = 2 });
                documents.Add(ptablePrincipalDetailLicenceApplied);
                documents.NewPage();
                PdfPTable myTable = new PdfPTable(6);
                myTable.SpacingAfter = 10;
                myTable.AddCell(new PdfPCell(new Phrase(" Dated: " + DateTime.Now.ToShortDateString(), verdanaHeader)) { Border = 0, Padding = 5, PaddingTop = 30, Colspan = 3, HorizontalAlignment = PdfPCell.ALIGN_LEFT, });
                myTable.AddCell(new PdfPCell(new Phrase(" Registration No.: " + licenceNo.ToString(), verdanaHeader)) { Border = 0, Padding = 5, PaddingTop = 30, Colspan = 3, HorizontalAlignment = PdfPCell.ALIGN_RIGHT, });
                if (principalEmployerLicence.Licence_PE_ISM_Contrators != null)
                {
                    PdfPCell Annheader = new PdfPCell(new Phrase(" Annexure (Detail of Contractors)", verdanaHeader));
                    Annheader.Colspan = 6;
                    Annheader.Border = 0;
                    Annheader.Padding = 20;
                    Annheader.Left = 50;
                    Annheader.HorizontalAlignment = PdfPCell.ALIGN_JUSTIFIED;
                    myTable.AddCell(Annheader);
                    Annheader.HorizontalAlignment = Element.ALIGN_CENTER;
                    PdfPCell header = new PdfPCell(new Phrase("Detail of Contractors of Principal Employer under The Inter-State Migrant Workmen Act, 1979", verdanaHeader));
                    header.Colspan = 6;
                    myTable.AddCell(header);
                    header.HorizontalAlignment = Element.ALIGN_CENTER;
                    PdfPCell CellTwoHdr = new PdfPCell(new Phrase("Name of Contractor", verdanaHeader));
                    myTable.AddCell(CellTwoHdr);
                    PdfPCell CellThreeHdr = new PdfPCell(new Phrase(" Address of Contractor", verdanaHeader));
                    myTable.AddCell(CellThreeHdr);
                    PdfPCell CellFourHdr = new PdfPCell(new Phrase("Nature of work in which contract labourer is employed or is to be employed", verdanaHeader));
                    myTable.AddCell(CellFourHdr);
                    PdfPCell CelltweHdr = new PdfPCell(new Phrase(" Maximum no. of contract labourer to be employed on any day through each contractor ", verdanaHeader));
                    myTable.AddCell(CelltweHdr);
                    PdfPCell CellTwo = new PdfPCell(new Phrase("Estimated Date of commencement of each contract work under each contractor", verdanaHeader));
                    myTable.AddCell(CellTwo);
                    PdfPCell CellEightHdr = new PdfPCell(new Phrase("Estimated Date of termination of employment of contract labourer under contractor", verdanaHeader));
                    myTable.AddCell(CellEightHdr);
                    var Contractors = principalEmployerLicence.Licence_PE_ISM_Contrators.ToList();
                    foreach (var v in Contractors)
                    {
                        PdfPCell CellThree = new PdfPCell(new Phrase(v.Name, verdana));
                        myTable.AddCell(CellThree);
                        PdfPCell CellEight = new PdfPCell(new Phrase(v.Address, verdana));
                        myTable.AddCell(CellEight);
                        PdfPCell CellNine = new PdfPCell(new Phrase(v.NatureOfWork, verdana));
                        myTable.AddCell(CellNine);
                        PdfPCell CellTen = new PdfPCell(new Phrase(v.MaxLabourWorkerEmployed.ToString(), verdana));
                        myTable.AddCell(CellTen);
                        PdfPCell Cellele = new PdfPCell(new Phrase(String.Format("{0:dd-MMM-yyyy}", v.DateOfCommencement), verdana));
                        myTable.AddCell(Cellele);
                        PdfPCell CellSeven = new PdfPCell(new Phrase(String.Format("{0:dd-MMM-yyyy}", v.DateOfTermination), verdana));
                        myTable.AddCell(CellSeven);
                    }
                   

                }
                documents.Add(myTable);
                documents.Close();
                PDFPageEvents pageeventsobj = new PDFPageEvents();



                //ptablerightSideDetail.AddCell(new PdfPCell(para) { Border = 0, VerticalAlignment = PdfPCell.ALIGN_JUSTIFIED, HorizontalAlignment = PdfPCell.ALIGN_JUSTIFIED, FixedHeight = 400 });
            }
            catch (Exception ex)
            {
                genericFormModel.HasException = true;
                genericFormModel.Exceptions = ex.Message;
            }
            return genericFormModel;
        }


        // Helper method to create a cell with specific formatting
        private PdfPCell CreateCell(string text, float fontSize, bool isBold)
        {
            Font font = FontFactory.GetFont("Times-Roman", fontSize, isBold ? Font.BOLD : Font.NORMAL);
            PdfPCell cell = new PdfPCell(new Phrase(text, font))
            {
                Padding = 5,
                Border = Rectangle.BOTTOM_BORDER,
                BorderColor = BaseColor.GRAY
            };
            return cell;
        }


        #region Convert Amount To Word
        public class NumberToWords
        {
            public String[] units = { "Zero", "One", "Two", "Three",
                                            "Four", "Five", "Six", "Seven", "Eight", "Nine", "Ten", "Eleven",
                                            "Twelve", "Thirteen", "Fourteen", "Fifteen", "Sixteen",
                                            "Seventeen", "Eighteen", "Nineteen" };

            public String[] tens = { "", "", "Twenty", "Thirty", "Forty",
                                            "Fifty", "Sixty", "Seventy", "Eighty", "Ninety" };
        }
        public static String ConvertAmount(double amount)
        {
            try
            {
                Int64 amount_int = (Int64)amount;
                Int64 amount_dec = (Int64)Math.Round((amount - (double)(amount_int)) * 100);
                if (amount_dec == 0)
                {
                    return Convert(amount_int) + "Rupees" + " Only";
                }
                else
                {
                    return Convert(amount_int) + " Point " + Convert(amount_dec) + " Only";
                }
            }
            catch (Exception e)
            {
                // TODO: handle exception  
            }
            return "";
        }

        public static String Convert(Int64 i)
        {
            NumberToWords numberToWords = new NumberToWords();
            if (i < 20)
            {
                return numberToWords.units[i];
            }
            if (i < 100)
            {
                return numberToWords.tens[i / 10] + ((i % 10 > 0) ? " " + Convert(i % 10) : "");
            }
            if (i < 1000)
            {
                return numberToWords.units[i / 100] + " Hundred"
                        + ((i % 100 > 0) ? " And " + Convert(i % 100) : "");
            }
            if (i < 100000)
            {
                return Convert(i / 1000) + " Thousand "
                + ((i % 1000 > 0) ? " " + Convert(i % 1000) : "");
            }
            if (i < 10000000)
            {
                return Convert(i / 100000) + " Lakh "
                        + ((i % 100000 > 0) ? " " + Convert(i % 100000) : "");
            }
            if (i < 1000000000)
            {
                return Convert(i / 10000000) + " Crore "
                        + ((i % 10000000 > 0) ? " " + Convert(i % 10000000) : "");
            }
            return Convert(i / 1000000000) + " Arab "
                    + ((i % 1000000000 > 0) ? " " + Convert(i % 1000000000) : "");
        }
        #endregion


        private string Convert24HrsTo12Hrs(string time)
        {
            var timeJson = JsonConvert.DeserializeObject<TimeFormatViewModal>(time);
            var hours = timeJson.hour;
            var minutes = timeJson.minute;
            var amPmDesignator = "AM";
            if (hours == 0)
                hours = 12;
            else if (hours == 12)
                amPmDesignator = "PM";
            else if (hours > 12)
            {
                hours -= 12;
                amPmDesignator = "PM";
            }
            return String.Format("{0}:{1:00} {2}", hours, minutes, amPmDesignator);
        }

        public async Task<GenerateLicenceNoViewModel> GenerateLicenceNo(Int64 appId)
        {
            GenerateLicenceNoViewModel generateLicenceNo = new GenerateLicenceNoViewModel();
            {
                try
                {
                    var application = _context.Applications.Where(x => x.AppId == appId && x.IsDeleted == false).FirstOrDefault();
                    var projectSite = _context.ProjectSiteLogs.Where(x => x.ProjectSiteRefId == application.ProjectSiteRefId && x.ProjectSiteVersion == application.ProjectSiteVersion).FirstOrDefault();
                    var licenceNo = "";
                    string districtAliasName = _context.Districts.Where(x => x.DistrictLgdId == projectSite.DistrictRefId).Select(x => x.DistrictAliasName).FirstOrDefault();
                    if (application != null)
                    {
                        string appType = "";
                        if (application.ApplicationType == ApplicationTypeEnum.REG_ESTB_OSH)
                        {
                            appType = "01";
                        }
                        else if (application.ApplicationType == ApplicationTypeEnum.CONTRACT_LABOUR_OSH)
                        {
                            appType = "02";
                        }
                        else if (application.ApplicationType == ApplicationTypeEnum.BUILDING_PLAN_OSH)
                        {
                            appType = "03";
                        }
                        else if (application.ApplicationType == ApplicationTypeEnum.COMMON_LICENCE_OSH)
                        {
                            appType = "04";
                        }
                        else if (application.ApplicationType == ApplicationTypeEnum.BUILDING_PLAN_HUD)
                        {
                            appType = "05";
                        }
                        else if (application.ApplicationType == ApplicationTypeEnum.SHOP_LICENCE)
                        {
                            appType = "06";
                        }
                        else if (application.ApplicationType == ApplicationTypeEnum.FACTORY_LICENCE)
                        {
                            appType = "70";
                        }
                        else if (application.ApplicationType == ApplicationTypeEnum.WOMEN_NIGHT_SHIFT_SHOP)
                        {
                            appType = "61";
                        }

                        else if (application.ApplicationType == ApplicationTypeEnum.WOMEN_NIGHT_SHIFT_FACTORY)
                        {
                            appType = "62";
                        }
                        else if (application.ApplicationType == ApplicationTypeEnum.BOCW_ESTABLISHMENT_ACT)
                        {
                            appType = "35";
                        }
                        else if (application.ApplicationType == ApplicationTypeEnum.CONTRACT_LABOUR)
                        {
                            appType = "38";
                        }
                        else if (application.ApplicationType == ApplicationTypeEnum.ISM_PRINCIPAL_EMPLOYER)
                        {
                            appType = "39";
                        }
                        else if (application.ApplicationType == ApplicationTypeEnum.PRINCIPAL_EMPLOYER)
                        {
                            appType = "37";
                        }
                        else if (application.ApplicationType == ApplicationTypeEnum.MOTOR_TRANSPORT)
                        {
                            appType = "36";
                        }
                        else if (application.ApplicationType == ApplicationTypeEnum.ISM_CONTRACT_LABOUR)
                        {
                            appType = "40";
                        }


                        if (application.ApplicationType == ApplicationTypeEnum.BUILDING_PLAN_PROPOSED || application.ApplicationType == ApplicationTypeEnum.BUILDING_PLAN_EXISTING || application.ApplicationType == ApplicationTypeEnum.BUILDING_PLAN_ADDITION_AMENDMENT || application.ApplicationType == ApplicationTypeEnum.BUILDING_PLAN_PSIEC)
                        {
                            licenceNo = "DOFPB" + projectSite.ProjectSiteRefId.ToString().PadLeft(8, '0');
                        }
                        else
                        {
                            // Call Stored Procedure here
                            var licence = await _iApplicationManagementService.GetLicenceNoDetailsByProjectSiteRefId(appId);
                            if (licence.ResponseDataModel != null && licence.ResponseDataModel != "" && licence.ResponseDataModel != null && licence.ResponseDataModel != "NA")
                            {
                                licenceNo = licence.ResponseDataModel;
                            }
                            else
                            {
                                licenceNo = districtAliasName + '/' + 'N' + appType + '/' + projectSite.ProjectSiteRefId.ToString("D8");
                            }
                        }

                        ApplicationLicenceNoMapping applicationLicenceNo = new ApplicationLicenceNoMapping()
                        {
                            AppRefId = appId,
                            LicenceNumber = licenceNo
                        };

                        if (!_context.ApplicationLicenceNoMapping.Any(x => x.AppRefId == appId))
                        {
                            await _context.ApplicationLicenceNoMapping.AddAsync(applicationLicenceNo);
                            await _context.SaveChangesAsync();
                            //_context.ApplicationLicenceNoMapping.Update(applicationLicenceNo);
                            //await _context.SaveChangesAsync();
                        }
                        else
                        {
                            //await _context.ApplicationLicenceNoMapping.AddAsync(applicationLicenceNo);
                            //await _context.SaveChangesAsync();
                        }
                        generateLicenceNo.LicenceNo = licenceNo;
                    }
                }
                catch (Exception ex)
                {
                    generateLicenceNo = new GenerateLicenceNoViewModel()
                    {
                        LicenceNo = string.Empty
                    };
                }
            }
            return generateLicenceNo;
        }


        public async Task<GenerateDisputeNoViewModel> GenerateDisputeNo(Establishment_EPFO_Logs formModel)
        {
            GenerateDisputeNoViewModel generateDisputeNo = new GenerateDisputeNoViewModel();
            {
                try
                {

                    formModel = GenericModelOps<Establishment_EPFO_Logs>.SetNullAllNevigationProperties(formModel);
                    formModel.SentDate = DateTime.Now;
                    formModel.HasSent = false;
                    _iGR_Establishment_EPFO.Insert(formModel);
                    await _iGR_Establishment_EPFO.SavechangeAsync();
                    var establishmentEPFOLogsId = formModel.EstablishmentEPFOLogsId;

                    int lastid = _context.ApplicationDisputeNoMapping.Count();

                    var disputeno = "";
                    var disputeid = lastid + 1;
                    var abbre = "DISPUTE";
                    disputeno = abbre + '/' + '2' + '/' + disputeid.ToString().PadLeft(6, '0');

                    ApplicationDisputeNoMapping applicationDisputeNo = new ApplicationDisputeNoMapping()
                    {
                        EstablishmentId = formModel.EstablishmentRefId,
                        DisputeNumber = disputeno,
                        SentDateTime = DateTime.Now,
                        IsNotSent = 0,
                        EstablishmentEPFOLogsId = establishmentEPFOLogsId,

                    };

                    await _context.ApplicationDisputeNoMapping.AddAsync(applicationDisputeNo);
                    await _context.SaveChangesAsync();

                    generateDisputeNo.DisputeNo = disputeno;
                    generateDisputeNo.EstablishmentEPFOLogsRefId = establishmentEPFOLogsId;


                }
                catch (Exception ex)
                {
                    generateDisputeNo = new GenerateDisputeNoViewModel()
                    {
                        DisputeNo = string.Empty
                    };
                }
            }
            return generateDisputeNo;
        }

        public async Task<CertificateGenerateServiceResultTemplate> Generate_Form_V(Int64 appRefid, Int64 id, string userName, int isFormVGenerate)
        {
            CertificateGenerateServiceResultTemplate genericFormModel = new CertificateGenerateServiceResultTemplate();
            try
            {
                //GenerateDisputeNoViewModel generateDispute = await GenerateDisputeNo(formModel);
                string CertificatePerformaURL = string.Empty;

                if (isFormVGenerate == 1)
                {
                    var contractor = _context.Licence_CL_PE_Contrator.Where(x => x.Id == id).FirstOrDefault();
                    contractor.IsFormVGenerate = 1;
                    contractor.FormVGenerateDate = DateTime.Now;
                    _context.Licence_CL_PE_Contrator.Update(contractor);
                    _context.SaveChanges();
                }

                genericFormModel = await Certificate_FormV(appRefid, id);

            }
            catch (Exception ex)
            {
                genericFormModel.HasException = true;
                genericFormModel.Exceptions = ex.Message;
            }
            return genericFormModel;
        }

        private async Task<CertificateGenerateServiceResultTemplate> Certificate_FormV(Int64 appRefId, Int64 Id)
        {
            CertificateGenerateServiceResultTemplate genericFormModel = new CertificateGenerateServiceResultTemplate();
            try
            {
                var app = _context.Applications.Where(x => x.AppId == appRefId).FirstOrDefault();
                if (app.ApplicationType == ApplicationTypeEnum.PRINCIPAL_EMPLOYER)
                {
                    Guid guId = Guid.NewGuid();
                    genericFormModel.PdfNameGUID = guId.ToString();
                    var sfilepath = "";
                    sfilepath = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "TempFiles")).Root + guId.ToString() + ".pdf";
                    FileInfo file = new FileInfo(sfilepath);
                    if (file.Exists)
                    {
                        file.Delete();
                    }
                    iTextSharp.text.Document documents = new iTextSharp.text.Document(iTextSharp.text.PageSize.A4, 5, 5, 30, 30);
                    documents.Open();
                    #region ClassObjects
                    pdfFonts classFonts = new pdfFonts();
                    #endregion

                    #region CreateObjects
                    PdfWriter owriter = PdfWriter.GetInstance(documents, new FileStream(sfilepath, FileMode.Create));
                    PDFPageEvents pageEvent = new PDFPageEvents();
                    owriter.PageEvent = pageEvent;
                    owriter.SetEncryption(PdfWriter.STRENGTH128BITS, "", "", PdfWriter.AllowPrinting);
                    #endregion

                    var fontName = "Verdana";
                    if (!FontFactory.IsRegistered(fontName))
                    {
                        var fontPath = "Content/fonts/VERDANA.TTF";
                        FontFactory.Register(fontPath);
                    }

                    #region Tables

                    var parentWithChildObject = await _iGR_Licence_CL_PE_GeneralDetail
                        .GetAsync(x => x.AppRefId == appRefId,  //Conditions         
                        null,                //Orders
                        x => x.Application, x => x.Application.ApplicationAction, x => x.Licence_CL_PE_Contrators)      //Includes
                        .ConfigureAwait(false);
                    var principalEmployerLicence = parentWithChildObject.FirstOrDefault();

                    var projectSite = _context.ProjectSites.Where(x => x.ProjectSiteId == principalEmployerLicence.Application.ProjectSiteRefId).FirstOrDefault();
                    var cldeails = _context.Licence_CL_PE_Contrator.Where(x => x.Id == Id).FirstOrDefault();
  

                    #endregion

                    #region Licence Validity Year
                    int licenceValidUpto = 0;
                    DofLicenceDetailsViewModel dofDetails = new DofLicenceDetailsViewModel();
                    if (principalEmployerLicence.Application.ApplicationPurposeType == ApplicationPurposeTypeEnum.GRANT_LICENCE)
                    {
                        DateTime registrationDate = DateTime.MinValue;
                        licenceValidUpto = 1;
                    }
                    #endregion

                    string fontpath1 = "Content/fonts/VERDANA.TTF";
                    BaseFont customfont = BaseFont.CreateFont(fontpath1, BaseFont.CP1252, BaseFont.EMBEDDED);
                    string fontpath2 = "Content/fonts/VERDANA.TTF";
                    BaseFont customfontimpact = BaseFont.CreateFont(fontpath2, BaseFont.CP1252, BaseFont.EMBEDDED);
                    documents.Open();
                    Font verdanaHeader = FontFactory.GetFont("Arial", 15, Font.BOLD);
                    Font footer = FontFactory.GetFont("Arial", 9, Font.BOLD, BaseColor.BLUE);
                    Font verdana = FontFactory.GetFont("Arial", 11, BaseColor.BLACK);
                    Font verdanaBoldItalic = FontFactory.GetFont("Arial", 8, Font.BOLDITALIC);
                    Font verdanasmall = FontFactory.GetFont("Arial", 8, Font.NORMAL);
                    verdanasmall.Color = BaseColor.WHITE;
                    Font verdanaRightHeader = FontFactory.GetFont("Arial", 15, Font.BOLD);
                    verdanaRightHeader.Color = BaseColor.WHITE;
                    Font verdanaRightHeader1 = FontFactory.GetFont("Arial", 12, Font.BOLD);
                    verdanaRightHeader1.Color = BaseColor.WHITE;
                    Font arialOldLicenceDetails = FontFactory.GetFont("Arial", 10, Font.BOLD);
                    arialOldLicenceDetails.Color = BaseColor.WHITE;
                    Font monotypeHeader = new Font(customfont, 20);
                    Font monotype = FontFactory.GetFont("Arial", 13, Font.BOLD);
                    Font impact = new Font(customfontimpact, 12);
                    Font impactHeader = new Font(customfontimpact, 11, Font.BOLD);
                    impactHeader.Color = BaseColor.WHITE;
                    BaseFont bfTimes = BaseFont.CreateFont(BaseFont.TIMES_ROMAN, BaseFont.CP1252, false);
                    Font timesRoman = FontFactory.GetFont("Arial", 13, BaseColor.BLACK);
                    Font ArialFont = FontFactory.GetFont("Arial", 8, BaseColor.WHITE);
                    Font SealFontArial = FontFactory.GetFont("Arial", 9, BaseColor.BLACK);
                    Font verdanaBold = FontFactory.GetFont("Arial", 14, Font.BOLD);
                    Font verdanalic = FontFactory.GetFont("Arial", 9, Font.BOLD);
                    verdanalic.Color = BaseColor.WHITE;
                    var factoryPerforma = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "LicensePerformas")).Root + "letter_head.jpg";
                    iTextSharp.text.Image pageLogo1 = iTextSharp.text.Image.GetInstance(new FileStream(factoryPerforma, FileMode.OpenOrCreate, FileAccess.Read));

                    pageLogo1.Alignment = iTextSharp.text.Image.ALIGN_CENTER | iTextSharp.text.Image.UNDERLYING;
                    pageLogo1.ScaleToFit(750f, 750f);
                    pageLogo1.SetAbsolutePosition(30f, 75f);
                    pageLogo1.BorderWidth = 1;
                    documents.Add(pageLogo1);


                    #region Headers
                    Paragraph pAppSubHeadingSpace = new Paragraph(" ", classFonts.SubHeader());
                    pAppSubHeadingSpace.Alignment = Element.ALIGN_CENTER;
                    documents.Add(new Paragraph(pAppSubHeadingSpace));

                    Paragraph AppHeading = new Paragraph("FORM - V", verdanaHeader);
                    AppHeading.Alignment = Element.ALIGN_CENTER;
                    AppHeading.SpacingBefore = 40f;
                    documents.Add(new Paragraph(AppHeading));

                    Paragraph Heading = new Paragraph("Under Contract Labour (Regulation & Abolition) Act, 1970", verdanaHeader);
                    Heading.Alignment = Element.ALIGN_CENTER;
                    documents.Add(new Paragraph(Heading));

                    Paragraph deptHeading = new Paragraph("[Rule 21(2)]", verdanaHeader);
                    deptHeading.Alignment = Element.ALIGN_CENTER;
                    documents.Add(new Paragraph(deptHeading));


                    pAppSubHeadingSpace.Alignment = Element.ALIGN_CENTER;
                    documents.Add(new Paragraph(pAppSubHeadingSpace));
                    #endregion


                    #region AppPoints
                    PdfPTable ptableAppPoints = new PdfPTable(3);
                    ptableAppPoints.DefaultCell.Border = 0;
                    #endregion


                    PdfPTable ptableAmount = new PdfPTable(3);
                    ptableAmount.DefaultCell.Border = 0;

                    //Font timesRoman = FontFactory.GetFont("Arial", 13, Color.BLACK);

                    Paragraph para1 = new Paragraph
                    {
                        Alignment = Element.ALIGN_JUSTIFIED // Justify the text
                    };

                    para1.Add(new Chunk(Chunk.NEWLINE + "             Certified that we have engaged the applicant ", timesRoman));
                    para1.Add(new Chunk(cldeails.Name + ", " + cldeails.Address + ".", verdanaBold));
                    para1.Add(new Chunk(Chunk.NEWLINE + "Contractor in our establishment. We undertake to ", timesRoman));
                    para1.Add(new Chunk("be bounded by all the provisions of the Contract labour (Regulation and Abolition) Act, 1970, and ", timesRoman));
                    para1.Add(new Chunk("the punjab contract labour (Regulation and Abolition) Act, 1973 in so far as the provisions are applicable to us in respect", timesRoman));
                    para1.Add(new Chunk("of the employment of contract labour by the applicant in our establishment. ", timesRoman));
                    ptableAmount.AddCell(new PdfPCell(new Phrase(para1)) { Border = 0, Colspan = 3, PaddingTop = 70 });

                    var phraseDeclaration = new Phrase();
                    phraseDeclaration.Add(new Chunk("Declaration: ", verdanaBold));
                    phraseDeclaration.Add(new Chunk("I hereby declare that the particulars given above are true to the best of my knowledge and belief.", timesRoman));
                    ptableAmount.AddCell(new PdfPCell(new Phrase(phraseDeclaration)) { Border = 0, Colspan = 3, PaddingTop = 80 });

                    var phrasePlace = new Phrase();
                    phrasePlace.Add(new Chunk("Place: ", verdanaBold));
                    phrasePlace.Add(new Chunk(projectSite.Address, timesRoman));
                    ptableAmount.AddCell(new PdfPCell(new Phrase(phrasePlace)) { Border = 0, Colspan = 3, PaddingTop = 10 });

                    var phraseDate = new Phrase();
                    phraseDate.Add(new Chunk("Date: ", verdanaBold));
                    phraseDate.Add(new Chunk(cldeails.FormVGenerateDate.ToLongDateString(), timesRoman));
                    ptableAmount.AddCell(new PdfPCell(new Phrase(phraseDate)) { Border = 0, Colspan = 3, PaddingTop = 10 });

                    var usersignature = "";
                    usersignature = _context.ApplicationDocuments.Where(x => x.AppRefId == appRefId && x.DocumentRefId == 50033).Select(x => x.AttachmentName).FirstOrDefault();
                    usersignature = string.IsNullOrEmpty(usersignature) ? "0fa4bd76-19ea-4906-a218-dae789094a58.jpg" : usersignature;
                    if (usersignature.Count() == 0 || usersignature == null)
                    {
                        usersignature = "0fa4bd76-19ea-4906-a218-dae789094a58.jpg";
                    }

                    string officerSigFilePath = "0fa4bd76-19ea-4906-a218-dae789094a58.jpg";
                    officerSigFilePath = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "AppFiles")).Root + usersignature;
                    iTextSharp.text.Image signature = iTextSharp.text.Image.GetInstance(new FileStream(officerSigFilePath, FileMode.OpenOrCreate, FileAccess.Read));
                    signature.ScaleToFit(120f, 120f);
                    signature.SetAbsolutePosition(630f, 150f);
                    ptableAmount.AddCell(new PdfPCell(signature) { Border = 0, PaddingTop = 10, Colspan = 3, HorizontalAlignment = PdfPCell.ALIGN_RIGHT });


                    var phraseApplicent = new Phrase();
                    phraseApplicent.Add(new Chunk("Applicent Name: ", verdanaBold));
                    phraseApplicent.Add(new Chunk(projectSite.ContactPersonFirstName + " " + projectSite.ContactPersonMiddleName + " " + projectSite.ContactPersonLastName, timesRoman));
                    ptableAmount.AddCell(new PdfPCell(new Phrase(phraseApplicent)) { Border = 0, Colspan = 3, PaddingTop = 15, HorizontalAlignment = PdfPCell.ALIGN_RIGHT });

                    var phraseAddress = new Phrase();
                    phraseAddress.Add(new Chunk("Address: ", verdanaBold));
                    phraseAddress.Add(new Chunk(projectSite.Address + ", " + projectSite.VillageOrTown, timesRoman));
                    ptableAmount.AddCell(new PdfPCell(new Phrase(phraseAddress)) { Border = 0, Colspan = 3, PaddingTop = 5, HorizontalAlignment = PdfPCell.ALIGN_RIGHT });




                    documents.Add(ptableAmount);


                    documents.Close();
                    PDFPageEvents pageeventsobj = new PDFPageEvents();
                }
                else
                {
                    Guid guId = Guid.NewGuid();
                    genericFormModel.PdfNameGUID = guId.ToString();
                    var sfilepath = "";
                    sfilepath = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "TempFiles")).Root + guId.ToString() + ".pdf";
                    FileInfo file = new FileInfo(sfilepath);
                    if (file.Exists)
                    {
                        file.Delete();
                    }
                    iTextSharp.text.Document documents = new iTextSharp.text.Document(iTextSharp.text.PageSize.A4, 5, 5, 30, 30);
                    documents.Open();
                    #region ClassObjects
                    pdfFonts classFonts = new pdfFonts();
                    #endregion

                    #region CreateObjects
                    PdfWriter owriter = PdfWriter.GetInstance(documents, new FileStream(sfilepath, FileMode.Create));
                    PDFPageEvents pageEvent = new PDFPageEvents();
                    owriter.PageEvent = pageEvent;
                    owriter.SetEncryption(PdfWriter.STRENGTH128BITS, "", "", PdfWriter.AllowPrinting);
                    #endregion

                    var fontName = "Verdana";
                    if (!FontFactory.IsRegistered(fontName))
                    {
                        var fontPath = "Content/fonts/VERDANA.TTF";
                        FontFactory.Register(fontPath);
                    }

                    #region Tables
            
                        var parentWithChildObject = await _iGR_Licence_PE_ISM_GeneralDetail
                          .GetAsync(x => x.AppRefId == appRefId,  //Conditions         
                            null,                //Orders
                            x => x.Application, x => x.Application.ApplicationAction, x => x.Licence_PE_ISM_Contrators)      //Includes
                          .ConfigureAwait(false);
                    var principalEmployerLicence = parentWithChildObject.FirstOrDefault();

                    var projectSite = _context.ProjectSites.Where(x => x.ProjectSiteId == principalEmployerLicence.Application.ProjectSiteRefId).FirstOrDefault();
                    var cldeails = _context.Licence_PE_ISM_Contrators.Where(x => x.Id == Id).FirstOrDefault();
        
                    #endregion

                    #region Licence Validity Year
                    int licenceValidUpto = 0;
                    DofLicenceDetailsViewModel dofDetails = new DofLicenceDetailsViewModel();
                    if (principalEmployerLicence.Application.ApplicationPurposeType == ApplicationPurposeTypeEnum.GRANT_LICENCE)
                    {
                        DateTime registrationDate = DateTime.MinValue;
                        licenceValidUpto = 1;
                    }
                    #endregion

                    string fontpath1 = "Content/fonts/VERDANA.TTF";
                    BaseFont customfont = BaseFont.CreateFont(fontpath1, BaseFont.CP1252, BaseFont.EMBEDDED);
                    string fontpath2 = "Content/fonts/VERDANA.TTF";
                    BaseFont customfontimpact = BaseFont.CreateFont(fontpath2, BaseFont.CP1252, BaseFont.EMBEDDED);
                    documents.Open();
                    Font verdanaHeader = FontFactory.GetFont("Arial", 15, Font.BOLD);
                    Font footer = FontFactory.GetFont("Arial", 9, Font.BOLD, BaseColor.BLUE);
                    Font verdana = FontFactory.GetFont("Arial", 11, BaseColor.BLACK);
                    Font verdanaBoldItalic = FontFactory.GetFont("Arial", 8, Font.BOLDITALIC);
                    Font verdanasmall = FontFactory.GetFont("Arial", 8, Font.NORMAL);
                    verdanasmall.Color = BaseColor.WHITE;
                    Font verdanaRightHeader = FontFactory.GetFont("Arial", 15, Font.BOLD);
                    verdanaRightHeader.Color = BaseColor.WHITE;
                    Font verdanaRightHeader1 = FontFactory.GetFont("Arial", 12, Font.BOLD);
                    verdanaRightHeader1.Color = BaseColor.WHITE;
                    Font arialOldLicenceDetails = FontFactory.GetFont("Arial", 10, Font.BOLD);
                    arialOldLicenceDetails.Color = BaseColor.WHITE;
                    Font monotypeHeader = new Font(customfont, 20);
                    Font monotype = FontFactory.GetFont("Arial", 13, Font.BOLD);
                    Font impact = new Font(customfontimpact, 12);
                    Font impactHeader = new Font(customfontimpact, 11, Font.BOLD);
                    impactHeader.Color = BaseColor.WHITE;
                    BaseFont bfTimes = BaseFont.CreateFont(BaseFont.TIMES_ROMAN, BaseFont.CP1252, false);
                    Font timesRoman = FontFactory.GetFont("Arial", 13, BaseColor.BLACK);
                    Font ArialFont = FontFactory.GetFont("Arial", 8, BaseColor.WHITE);
                    Font SealFontArial = FontFactory.GetFont("Arial", 9, BaseColor.BLACK);
                    Font verdanaBold = FontFactory.GetFont("Arial", 14, Font.BOLD);
                    Font verdanalic = FontFactory.GetFont("Arial", 9, Font.BOLD);
                    verdanalic.Color = BaseColor.WHITE;
                    var factoryPerforma = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "LicensePerformas")).Root + "letter_head.jpg";
                    iTextSharp.text.Image pageLogo1 = iTextSharp.text.Image.GetInstance(new FileStream(factoryPerforma, FileMode.OpenOrCreate, FileAccess.Read));

                    pageLogo1.Alignment = iTextSharp.text.Image.ALIGN_CENTER | iTextSharp.text.Image.UNDERLYING;
                    pageLogo1.ScaleToFit(750f, 750f);
                    pageLogo1.SetAbsolutePosition(30f, 75f);
                    pageLogo1.BorderWidth = 1;
                    documents.Add(pageLogo1);


                    #region Headers
                    Paragraph pAppSubHeadingSpace = new Paragraph(" ", classFonts.SubHeader());
                    pAppSubHeadingSpace.Alignment = Element.ALIGN_CENTER;
                    documents.Add(new Paragraph(pAppSubHeadingSpace));

                    Paragraph AppHeading = new Paragraph("FORM - V", verdanaHeader);
                    AppHeading.Alignment = Element.ALIGN_CENTER;
                    AppHeading.SpacingBefore = 40f;
                    documents.Add(new Paragraph(AppHeading));


                    Paragraph Heading = new Paragraph("Migrant Workmen (Contractor) under the Inter-State Migrant Workmen(Regulation & Abolition) Act, 1979", verdanaHeader);
                    Heading.Alignment = Element.ALIGN_CENTER;
                    documents.Add(new Paragraph(Heading));

                    Paragraph deptHeading = new Paragraph("[Rule 21(2)]", verdanaHeader);
                    deptHeading.Alignment = Element.ALIGN_CENTER;
                    documents.Add(new Paragraph(deptHeading));


                    pAppSubHeadingSpace.Alignment = Element.ALIGN_CENTER;
                    documents.Add(new Paragraph(pAppSubHeadingSpace));
                    #endregion


                    #region AppPoints
                    PdfPTable ptableAppPoints = new PdfPTable(3);
                    ptableAppPoints.DefaultCell.Border = 0;
                    #endregion


                    PdfPTable ptableAmount = new PdfPTable(3);
                    ptableAmount.DefaultCell.Border = 0;

                    //Font timesRoman = FontFactory.GetFont("Arial", 13, Color.BLACK);

                    Paragraph para1 = new Paragraph
                    {
                        Alignment = Element.ALIGN_JUSTIFIED // Justify the text
                    };

                    para1.Add(new Chunk(Chunk.NEWLINE + "             Certified that we have engaged the applicant ", timesRoman));
                    para1.Add(new Chunk(cldeails.Name + ", " + cldeails.Address + ".", verdanaBold));
                    para1.Add(new Chunk(Chunk.NEWLINE + "Contractor in our establishment. We undertake to ", timesRoman));
                    para1.Add(new Chunk("be bounded by all the provisions of the Contract labour (Regulation and Abolition) Act, 1970, and ", timesRoman));
                    para1.Add(new Chunk("the punjab contract labour (Regulation and Abolition) Act, 1973 in so far as the provisions are applicable to us in respect", timesRoman));
                    para1.Add(new Chunk("of the employment of contract labour by the applicant in our establishment. ", timesRoman));
                    ptableAmount.AddCell(new PdfPCell(new Phrase(para1)) { Border = 0, Colspan = 3, PaddingTop = 70 });

                    var phraseDeclaration = new Phrase();
                    phraseDeclaration.Add(new Chunk("Declaration: ", verdanaBold));
                    phraseDeclaration.Add(new Chunk("I hereby declare that the particulars given above are true to the best of my knowledge and belief.", timesRoman));
                    ptableAmount.AddCell(new PdfPCell(new Phrase(phraseDeclaration)) { Border = 0, Colspan = 3, PaddingTop = 80 });

                    var phrasePlace = new Phrase();
                    phrasePlace.Add(new Chunk("Place: ", verdanaBold));
                    phrasePlace.Add(new Chunk(projectSite.Address, timesRoman));
                    ptableAmount.AddCell(new PdfPCell(new Phrase(phrasePlace)) { Border = 0, Colspan = 3, PaddingTop = 10 });

                    var phraseDate = new Phrase();
                    phraseDate.Add(new Chunk("Date: ", verdanaBold));
                    phraseDate.Add(new Chunk(cldeails.FormVGenerateDate.ToLongDateString(), timesRoman));
                    ptableAmount.AddCell(new PdfPCell(new Phrase(phraseDate)) { Border = 0, Colspan = 3, PaddingTop = 10 });




                    var usersignature = "";
                    usersignature = _context.ApplicationDocuments.Where(x => x.AppRefId == appRefId && x.DocumentRefId == 50033).FirstOrDefault().AttachmentName;
                    if (usersignature.Count() == 0 || usersignature == null)
                    {
                        usersignature = "0fa4bd76-19ea-4906-a218-dae789094a58.jpeg";
                    }
                    string officerSigFilePath = "0fa4bd76-19ea-4906-a218-dae789094a58.jpeg";
                    officerSigFilePath = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "AppFiles")).Root + usersignature;
                    iTextSharp.text.Image signature = iTextSharp.text.Image.GetInstance(new FileStream(officerSigFilePath, FileMode.OpenOrCreate, FileAccess.Read));
                    signature.ScaleToFit(120f, 120f);
                    signature.SetAbsolutePosition(630f, 150f);
                    ptableAmount.AddCell(new PdfPCell(signature) { Border = 0, PaddingTop = 10, Colspan = 3, HorizontalAlignment = PdfPCell.ALIGN_RIGHT });


                    var phraseApplicent = new Phrase();
                    phraseApplicent.Add(new Chunk("Applicent Name: ", verdanaBold));
                    phraseApplicent.Add(new Chunk(projectSite.ContactPersonFirstName + " " + projectSite.ContactPersonMiddleName + " " + projectSite.ContactPersonLastName, timesRoman));
                    ptableAmount.AddCell(new PdfPCell(new Phrase(phraseApplicent)) { Border = 0, Colspan = 3, PaddingTop = 15, HorizontalAlignment = PdfPCell.ALIGN_RIGHT });

                    var phraseAddress = new Phrase();
                    phraseAddress.Add(new Chunk("Address: ", verdanaBold));
                    phraseAddress.Add(new Chunk(projectSite.Address + ", " + projectSite.VillageOrTown, timesRoman));
                    ptableAmount.AddCell(new PdfPCell(new Phrase(phraseAddress)) { Border = 0, Colspan = 3, PaddingTop = 5, HorizontalAlignment = PdfPCell.ALIGN_RIGHT });




                    documents.Add(ptableAmount);


                    documents.Close();
                    PDFPageEvents pageeventsobj = new PDFPageEvents();
                }

            }
            catch (Exception ex)
            {
                genericFormModel.HasException = true;
                genericFormModel.Exceptions = ex.Message;
            }
            return genericFormModel;
        }


        private async Task<CertificateGenerateServiceResultTemplate> Certificate_AppForm_37(Int64 appRefId, string CertificatePerformaURL, ApplicationTypeEnum applicationType, string userName, string licenceNo, bool isDeemedCalling)
        {
            CertificateGenerateServiceResultTemplate genericFormModel = new CertificateGenerateServiceResultTemplate();
            try
            {
                Guid guId = Guid.NewGuid();
                genericFormModel.PdfNameGUID = guId.ToString();
                var sfilepath = "";
                sfilepath = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "TempFiles")).Root + guId.ToString() + ".pdf";
                FileInfo file = new FileInfo(sfilepath);
                if (file.Exists)
                {
                    file.Delete();
                }
                iTextSharp.text.Document documents = new iTextSharp.text.Document(iTextSharp.text.PageSize.A4, 5, 5, 30, 30);
                documents.Open();
                #region ClassObjects
                pdfFonts classFonts = new pdfFonts();
                #endregion

                #region CreateObjects
                PdfWriter owriter = PdfWriter.GetInstance(documents, new FileStream(sfilepath, FileMode.Create));
                PDFPageEvents pageEvent = new PDFPageEvents();
                owriter.PageEvent = pageEvent;
                owriter.SetEncryption(PdfWriter.STRENGTH128BITS, "", "", PdfWriter.AllowPrinting);
                #endregion

                var fontName = "Verdana";
                if (!FontFactory.IsRegistered(fontName))
                {
                    var fontPath = "Content/fonts/VERDANA.TTF";
                    FontFactory.Register(fontPath);
                }

                #region Tables

                var parentWithChildObject = await _iGR_Licence_CL_PE_GeneralDetail
                       .GetAsync(x => x.AppRefId == appRefId,  //Conditions         
                         null,                //Orders
                         x => x.Application, x => x.Application.ApplicationAction, x => x.Licence_CL_PE_Contrators)      //Includes
                       .ConfigureAwait(false);
                var principalEmployerLicence = parentWithChildObject.FirstOrDefault();
                var projectSite = _context.ProjectSites.Where(x => x.ProjectSiteId == principalEmployerLicence.Application.ProjectSiteRefId).FirstOrDefault();
                var alcCircle = _context.ALCCircles.Where(x => x.ALCCircleId == projectSite.AlcCircleRefId).FirstOrDefault();
                var applicationtype = "";
                if (principalEmployerLicence.Application.ApplicationPurposeType == ApplicationPurposeTypeEnum.GRANT_LICENCE)
                {
                    applicationtype = "(Registration)";
                }
                else if (principalEmployerLicence.Application.ApplicationPurposeType == ApplicationPurposeTypeEnum.RENEWAL_LICENCE)
                {
                    applicationtype = "(Renewal)";
                }
                else if (principalEmployerLicence.Application.ApplicationPurposeType == ApplicationPurposeTypeEnum.AMENDMENT_LICENCE)
                {
                    applicationtype = "(Amendment)";
                }

                #endregion

                #region Licence Validity Year
                int licenceValidUpto = 0;
                DofLicenceDetailsViewModel dofDetails = new DofLicenceDetailsViewModel();
                if (principalEmployerLicence.Application.ApplicationPurposeType == ApplicationPurposeTypeEnum.GRANT_LICENCE)
                {
                    DateTime registrationDate = DateTime.MinValue;
                    licenceValidUpto = 1;
                }
                #endregion

                string fontpath1 = "Content/fonts/VERDANA.TTF";
                BaseFont customfont = BaseFont.CreateFont(fontpath1, BaseFont.CP1252, BaseFont.EMBEDDED);
                string fontpath2 = "Content/fonts/VERDANA.TTF";
                BaseFont customfontimpact = BaseFont.CreateFont(fontpath2, BaseFont.CP1252, BaseFont.EMBEDDED);
                documents.Open();
                Font verdanaHeader = FontFactory.GetFont("Arial", 9, Font.BOLD);
                Font footer = FontFactory.GetFont("Arial", 9, Font.BOLD, BaseColor.BLUE);
                Font verdana = FontFactory.GetFont("Arial", 8, BaseColor.BLACK);
                Font verdanaBoldItalic = FontFactory.GetFont("Arial", 8, Font.BOLDITALIC);
                Font verdanasmall = FontFactory.GetFont("Arial", 8, Font.NORMAL);
                verdanasmall.Color = BaseColor.WHITE;
                Font verdanaRightHeader = FontFactory.GetFont("Arial", 15, Font.BOLD);
                verdanaRightHeader.Color = BaseColor.WHITE;
                Font verdanaRightHeader1 = FontFactory.GetFont("Arial", 12, Font.BOLD);
                verdanaRightHeader1.Color = BaseColor.WHITE;
                Font arialOldLicenceDetails = FontFactory.GetFont("Arial", 10, Font.BOLD);
                arialOldLicenceDetails.Color = BaseColor.WHITE;
                Font monotypeHeader = new Font(customfont, 20);
                Font monotype = FontFactory.GetFont("Arial", 13, Font.BOLD);
                Font impact = new Font(customfontimpact, 12);
                Font impactHeader = new Font(customfontimpact, 11, Font.BOLD);
                impactHeader.Color = BaseColor.WHITE;
                BaseFont bfTimes = BaseFont.CreateFont(BaseFont.TIMES_ROMAN, BaseFont.CP1252, false);
                Font timesRoman = FontFactory.GetFont("Arial", 10, BaseColor.BLACK);
                Font ArialFont = FontFactory.GetFont("Arial", 8, BaseColor.WHITE);
                Font SealFontArial = FontFactory.GetFont("Arial", 9, BaseColor.BLACK);
                Font verdanaBold = FontFactory.GetFont("Arial", 10, Font.BOLD);
                Font verdanalic = FontFactory.GetFont("Arial", 9, Font.BOLD);
                verdanalic.Color = BaseColor.WHITE;
                var principalEmployerPerforma = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "LicensePerformas")).Root + "CertificatePerforma_Principal_Employer.jpg";
                iTextSharp.text.Image pageLogo1 = iTextSharp.text.Image.GetInstance(new FileStream(principalEmployerPerforma, FileMode.OpenOrCreate, FileAccess.Read));

                pageLogo1.Alignment = iTextSharp.text.Image.ALIGN_CENTER | iTextSharp.text.Image.UNDERLYING;
                pageLogo1.ScaleToFit(800f, 800f);
                pageLogo1.SetAbsolutePosition(10f, 25f);
                pageLogo1.BorderWidth = 1;
                documents.Add(pageLogo1);
                PdfPTable ptableleftSideDetail = new PdfPTable(1);
                ptableleftSideDetail.DefaultCell.Border = 0;
                ptableleftSideDetail.DefaultCell.Padding = 0;
                ptableleftSideDetail.HorizontalAlignment = (Element.ALIGN_JUSTIFIED);
                ptableleftSideDetail.DefaultCell.VerticalAlignment = (Element.ALIGN_JUSTIFIED);
                ptableleftSideDetail.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
                ptableleftSideDetail.AddCell(new PdfPCell(new Phrase("", verdanaBold)) { Border = 0, PaddingTop = 10 });


                ptableleftSideDetail.AddCell(new PdfPCell(new Phrase("Registration No. : " + licenceNo.ToString(), verdanalic)) { Border = 0, PaddingTop = 300, PaddingLeft = -18 }); ;
                ptableleftSideDetail.AddCell(new PdfPCell(new Phrase("Registration Issued On : " + DateTime.Now.ToString("dd-MM-yyyy"), verdanalic)) { Border = 0, PaddingTop = 10, PaddingLeft = -18 });
                Font verdanaBoldBP = new Font(BaseFont.CreateFont(BaseFont.TIMES_ROMAN, BaseFont.CP1252, false), 8, Font.BOLD | Font.UNDERLINE, BaseColor.WHITE);
                Phrase phrase = new Phrase();

                ptableleftSideDetail.AddCell(new PdfPCell(phrase) { Border = 0, PaddingTop = 10, PaddingRight = 20 });

                #region QR Code Generate
                var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(principalEmployerLicence.Application.PublicAppRefNum);
                //string base64String = System.Convert.ToBase64String(plainTextBytes);
                var qrCodeDetails = "Establishment Name : " + projectSite.EstablishmentName + "|" + "Establishment Address : " + projectSite.Address + "|" + "IPIN : " + principalEmployerLicence.Application.InvestPunjab_Ipin + "|" + "Contact No : " + projectSite.ContactPersonFirstName + " " + projectSite.ContactPersonLastName + "|" + projectSite.ContactPersonMobileNo + "|" + "Licence No : " + licenceNo + "|" + "File No : " + principalEmployerLicence.Application.PublicAppRefNum;
                QRCodeGenerator qrGenerator = new QRCodeGenerator();
                QRCodeGenerator.QRCode qrCode = qrGenerator.CreateQrCode(qrCodeDetails, QRCodeGenerator.ECCLevel.Q);

                using (System.Drawing.Bitmap bitMap = qrCode.GetGraphic(20))
                {
                    using (MemoryStream ms = new MemoryStream())
                    {
                        bitMap.Save(ms, ImageFormat.Jpeg);
                        System.Drawing.Image imgQRCode = System.Drawing.Image.FromStream(ms);
                        iTextSharp.text.Image imdf = iTextSharp.text.Image.GetInstance(imgQRCode, System.Drawing.Imaging.ImageFormat.Jpeg);
                        imdf.ScaleAbsoluteHeight(120f);
                        imdf.ScaleAbsoluteWidth(120f);
                        ptableleftSideDetail.AddCell(new PdfPCell(imdf) { Border = 0, PaddingTop = 30, PaddingLeft = 0, PaddingRight = 0 });
                    }
                }
                #endregion

                ptableleftSideDetail.AddCell(new PdfPCell(new Phrase("" + Chunk.NEWLINE + "Department of Labour, Govt. of Punjab" + Chunk.NEWLINE, verdanaRightHeader1)) { Border = 0, PaddingTop = 15, PaddingRight = 25, HorizontalAlignment = PdfPCell.ALIGN_CENTER });

                PdfPTable ptablerightSideDetail = new PdfPTable(1);
                ptablerightSideDetail.DefaultCell.Border = 0;
                ptablerightSideDetail.DefaultCell.Padding = 0;

                //ptablerightSideDetail.AddCell(new PdfPCell(new Phrase("", verdanaBold)) { Border = 0, Padding = 4 });
                ptablerightSideDetail.AddCell(new PdfPCell(new Phrase("FORM-II", verdanaHeader)) { Border = 0, PaddingLeft = 20, PaddingTop = 10, HorizontalAlignment = PdfPCell.ALIGN_CENTER });
                ptablerightSideDetail.AddCell(new PdfPCell(new Phrase("[See Rule 18(1)]", verdanaHeader)) { Border = 0, PaddingLeft = 20, HorizontalAlignment = PdfPCell.ALIGN_CENTER });
                ptablerightSideDetail.AddCell(new PdfPCell(new Phrase("REGISTRATION CERTIFICATE OF PRINCIPAL EMPLOYER UNDER THE CONTRACT LABOUR (R & A) ACT, 1970 ", impactHeader)) { Border = 0, PaddingTop = 12, PaddingBottom = 20, PaddingLeft = 40, PaddingRight = 30, VerticalAlignment = PdfPCell.ALIGN_MIDDLE, HorizontalAlignment = PdfPCell.ALIGN_CENTER });
                ptablerightSideDetail.AddCell(new PdfPCell(new Phrase("CERTIFICATE OF REGISTRATION", monotype)) { Border = 0, PaddingBottom = 4, PaddingTop = 3, HorizontalAlignment = PdfPCell.ALIGN_CENTER });
                ptablerightSideDetail.AddCell(new PdfPCell(new Phrase("GOVERNMENT OF PUNJAB", monotype)) { Border = 0, PaddingBottom = 4, HorizontalAlignment = PdfPCell.ALIGN_CENTER });
                ptablerightSideDetail.AddCell(new PdfPCell(new Phrase(applicationtype, monotype)) { Border = 0, PaddingBottom = 4, HorizontalAlignment = PdfPCell.ALIGN_CENTER });
                ptablerightSideDetail.AddCell(new PdfPCell(new Phrase("", verdanaBold)) { Border = 0, Padding = 1, PaddingTop = 6 });
                ptablerightSideDetail.AddCell(new PdfPCell(new Phrase(" Dated: " + DateTime.Now.ToString("dd-MM-yyyy") + "                            Registration No. : " + licenceNo.ToString(), verdanaHeader)) { Border = 0 });

                Paragraph para1 = new Paragraph();
                para1.Add(new Chunk("        A Certificate of Registration containing the following particulars is hereby granted under sub-section (2) of section 7 of Contract Labour (Regulation and Abolition) Act, 1970 and the rules made thereunder, to . ", timesRoman));
                para1.Add(new Chunk(projectSite.EstablishmentName + "," + projectSite.Address, verdanaBold));

                para1.Add(new Chunk(Chunk.NEWLINE + "1. Nature of the work carried on in the establishment: ", timesRoman));
                para1.Add(new Chunk(principalEmployerLicence.NatureOfWork, verdanaBold));

                para1.Add(new Chunk(Chunk.NEWLINE + "2. Name and Address of the Owner / Principal Employer:  ", timesRoman));
                para1.Add(new Chunk(principalEmployerLicence.PE_Name + " ," + principalEmployerLicence.PE_Address, verdanaBold));

                para1.Add(new Chunk(Chunk.NEWLINE + "3. Name and Permanent Address of Manager:  ", timesRoman));
                para1.Add(new Chunk(principalEmployerLicence.Manager_Name + " ," + principalEmployerLicence.Manager_Address, verdanaBold));

                para1.Add(new Chunk(Chunk.NEWLINE + "4. Nature of the work in which contract labour is employed or is to be employed: ", timesRoman));
                para1.Add(new Chunk(" (Enclosed  Annexure)", verdanaBold));

                para1.Add(new Chunk(Chunk.NEWLINE + "5. Maximum  number of contract labour to be employed on any day through each contractor: ", timesRoman));
                para1.Add(new Chunk(" (Enclosed  Annexure)", verdanaBold));

                para1.Add(new Chunk(Chunk.NEWLINE + "6. Other particulars relevant to the employment of contract labour.", timesRoman));
                PdfPCell shipment1 = new PdfPCell(para1);
                shipment1.SetLeading(1f, 2f);
                ptablerightSideDetail.AddCell(new PdfPCell(shipment1) { Border = 0, HorizontalAlignment = PdfPCell.ALIGN_JUSTIFIED, VerticalAlignment = PdfPCell.ALIGN_JUSTIFIED, FixedHeight = 420, PaddingBottom = 15 });
                var userDetails = await _iAuthService.GetUserFullDetailByUserName(userName);
                string officerSigFilePath = "";
                if (!isDeemedCalling)
                {

                    if (userDetails.ListData.Count() == 0 || userDetails.ListData[0].Signature == null)
                    {
                        userDetails.ListData[0].Signature = "abc.jpg";
                    }
                    officerSigFilePath = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "OfficerSignatures")).Root + userDetails.ListData[0].Signature;
                    iTextSharp.text.Image signature = iTextSharp.text.Image.GetInstance(new FileStream(officerSigFilePath, FileMode.OpenOrCreate, FileAccess.Read));
                    signature.ScaleToFit(120f, 120f);
                    signature.SetAbsolutePosition(630f, 150f);
                    ptablerightSideDetail.AddCell(new PdfPCell(signature) { Border = 0, PaddingTop = 5, HorizontalAlignment = PdfPCell.ALIGN_RIGHT });
                }

                Paragraph SignSeal = new Paragraph();
                SignSeal.Add(new Chunk(Chunk.NEWLINE + "Circle Name: ", verdanaHeader));
                SignSeal.Add(new Chunk(alcCircle.ALCCircleName, SealFontArial));
                SignSeal.Add(new Chunk(Chunk.NEWLINE + "District Name: ", verdanaHeader));
                SignSeal.Add(new Chunk(alcCircle.ALCCircleName, SealFontArial));
                SignSeal.Add(new Chunk(Chunk.NEWLINE + "Date: ", verdanaHeader));
                SignSeal.Add(new Chunk(DateTime.Now.ToShortDateString(), SealFontArial));
                SignSeal.Add(new Chunk(Chunk.NEWLINE + "Time: ", verdanaHeader));
                SignSeal.Add(new Chunk(DateTime.Now.ToShortTimeString(), SealFontArial));
                ptablerightSideDetail.AddCell(new PdfPCell(SignSeal) { Border = 0, HorizontalAlignment = PdfPCell.ALIGN_LEFT, PaddingLeft = 200 });
                //ptablerightSideDetail.AddCell(new PdfPCell(new Phrase(" Dated " + DateTime.Now.ToShortDateString(), verdanaHeader)) { Border = 0, HorizontalAlignment = PdfPCell.ALIGN_LEFT});

                ptablerightSideDetail.AddCell(new PdfPCell(new Phrase("This is a computer generated document to check & verify please login to https://pblabour.gov.in/Account/ApplicationStatus", footer)) { Border = 1, BorderWidth = 1, Padding = 8 });
                PdfPTable ptablePrincipalDetailLicenceApplied = new PdfPTable(3);
                ptablePrincipalDetailLicenceApplied.DefaultCell.Border = 0;
                ptablePrincipalDetailLicenceApplied.DefaultCell.Padding = 2;
                ptablePrincipalDetailLicenceApplied.SpacingBefore = 1000;
                ptablePrincipalDetailLicenceApplied.AddCell(new PdfPCell(ptableleftSideDetail) { Border = 0 });
                ptablePrincipalDetailLicenceApplied.AddCell(new PdfPCell(ptablerightSideDetail) { Border = 0, Colspan = 2 });
                documents.Add(ptablePrincipalDetailLicenceApplied);
                documents.NewPage();
                PdfPTable myTable = new PdfPTable(6);
                myTable.SpacingAfter = 10;
                myTable.AddCell(new PdfPCell(new Phrase(" Dated: " + DateTime.Now.ToShortDateString(), verdanaHeader)) { Border = 0, Padding = 5, PaddingTop = 30, Colspan = 3, HorizontalAlignment = PdfPCell.ALIGN_LEFT, });
                myTable.AddCell(new PdfPCell(new Phrase(" Registration No.: " + licenceNo.ToString(), verdanaHeader)) { Border = 0, Padding = 5, PaddingTop = 30, Colspan = 3, HorizontalAlignment = PdfPCell.ALIGN_RIGHT, });
                if (principalEmployerLicence.Licence_CL_PE_Contrators != null)
                {
                    PdfPCell Annheader = new PdfPCell(new Phrase(" Annexure (Detail of Contractors)", verdanaHeader));
                    Annheader.Colspan = 6;
                    Annheader.Border = 0;
                    Annheader.Padding = 20;
                    Annheader.Left = 50;
                    Annheader.HorizontalAlignment = PdfPCell.ALIGN_JUSTIFIED;
                    myTable.AddCell(Annheader);
                    Annheader.HorizontalAlignment = Element.ALIGN_CENTER;
                    PdfPCell header = new PdfPCell(new Phrase("Detail of Contractors of Principal Employer under Contract Labour (R&A) Act, 1970", verdanaHeader));
                    header.Colspan = 6;
                    myTable.AddCell(header);
                    header.HorizontalAlignment = Element.ALIGN_CENTER;
                    PdfPCell CellTwoHdr = new PdfPCell(new Phrase("Name of Contractor", verdanaHeader));
                    myTable.AddCell(CellTwoHdr);
                    PdfPCell CellThreeHdr = new PdfPCell(new Phrase(" Address of Contractor", verdanaHeader));
                    myTable.AddCell(CellThreeHdr);
                    PdfPCell CellFourHdr = new PdfPCell(new Phrase("Nature of work in which contract labourer is employed or is to be employed", verdanaHeader));
                    myTable.AddCell(CellFourHdr);
                    PdfPCell CelltweHdr = new PdfPCell(new Phrase(" Maximum no. of contract labourer to be employed on any day through each contractor ", verdanaHeader));
                    myTable.AddCell(CelltweHdr);
                    PdfPCell CellTwo = new PdfPCell(new Phrase("Estimated Date of commencement of each contract work under each contractor", verdanaHeader));
                    myTable.AddCell(CellTwo);
                    PdfPCell CellEightHdr = new PdfPCell(new Phrase("Estimated Date of termination of employment of contract labourer under contractor", verdanaHeader));
                    myTable.AddCell(CellEightHdr);
                    var Contractors = principalEmployerLicence.Licence_CL_PE_Contrators.ToList();
                    foreach (var v in Contractors)
                    {
                        PdfPCell CellThree = new PdfPCell(new Phrase(v.Name, verdana));
                        myTable.AddCell(CellThree);
                        PdfPCell CellEight = new PdfPCell(new Phrase(v.Address, verdana));
                        myTable.AddCell(CellEight);
                        PdfPCell CellNine = new PdfPCell(new Phrase(v.NatureOfWork, verdana));
                        myTable.AddCell(CellNine);
                        PdfPCell CellTen = new PdfPCell(new Phrase(v.MaxLabourWorkerEmployed.ToString(), verdana));
                        myTable.AddCell(CellTen);
                        PdfPCell Cellele = new PdfPCell(new Phrase(String.Format("{0:dd-MMM-yyyy}", v.DateOfCommencement), verdana));
                        myTable.AddCell(Cellele);
                        PdfPCell CellSeven = new PdfPCell(new Phrase(String.Format("{0:dd-MMM-yyyy}", v.DateOfTermination), verdana));
                        myTable.AddCell(CellSeven);
                    }
                    //if (officerSigFilePath != null)
                    //{
                    //    myTable.AddCell(new PdfPCell(signature) { Border = 0, HorizontalAlignment = Element.ALIGN_RIGHT, Colspan = 6, PaddingTop = 30 });
                    //}

                }
                documents.Add(myTable);
                documents.Close();
                PDFPageEvents pageeventsobj = new PDFPageEvents();



                //ptablerightSideDetail.AddCell(new PdfPCell(para) { Border = 0, VerticalAlignment = PdfPCell.ALIGN_JUSTIFIED, HorizontalAlignment = PdfPCell.ALIGN_JUSTIFIED, FixedHeight = 400 });
            }
            catch (Exception ex)
            {
                genericFormModel.HasException = true;
                genericFormModel.Exceptions = ex.Message;
            }
            return genericFormModel;
        }

        private async Task<CertificateGenerateServiceResultTemplate> Certificate_AppForm_38(Int64 appRefId, string CertificatePerformaURL, ApplicationTypeEnum applicationType, string userName, string licenceNo, bool isDeemedCalling)
        {
            CertificateGenerateServiceResultTemplate genericFormModel = new CertificateGenerateServiceResultTemplate();
            try
            {
                Guid guId = Guid.NewGuid();
                genericFormModel.PdfNameGUID = guId.ToString();
                var sfilepath = "";
                sfilepath = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "TempFiles")).Root + guId.ToString() + ".pdf";
                FileInfo file = new FileInfo(sfilepath);
                if (file.Exists)
                {
                    file.Delete();
                }
                iTextSharp.text.Document documents = new iTextSharp.text.Document(iTextSharp.text.PageSize.A4, 5, 5, 30, 30);
                documents.Open();
                #region ClassObjects
                pdfFonts classFonts = new pdfFonts();
                #endregion

                #region CreateObjects
                PdfWriter owriter = PdfWriter.GetInstance(documents, new FileStream(sfilepath, FileMode.Create));
                PDFPageEvents pageEvent = new PDFPageEvents();
                owriter.PageEvent = pageEvent;
                owriter.SetEncryption(PdfWriter.STRENGTH128BITS, "", "", PdfWriter.AllowPrinting);
                #endregion

                var fontName = "Verdana";
                if (!FontFactory.IsRegistered(fontName))
                {
                    var fontPath = "Content/fonts/VERDANA.TTF";
                    FontFactory.Register(fontPath);
                }

                #region Tables

                var parentWithChildObject = await _iGR_Licence_Contract_Labour_GeneralDetail
                       .GetAsync(x => x.AppRefId == appRefId,  //Conditions         
                         null,                //Orders
                         x => x.Application, x => x.Application.ApplicationAction)      //Includes
                       .ConfigureAwait(false);
                var contractLabourLicence = parentWithChildObject.FirstOrDefault();
                var projectSite = _context.ProjectSites.Where(x => x.ProjectSiteId == contractLabourLicence.Application.ProjectSiteRefId).FirstOrDefault();
                var alcCircle = _context.ALCCircles.Where(x => x.ALCCircleId == projectSite.AlcCircleRefId).FirstOrDefault();
                var applicationtype = "";
                if (contractLabourLicence.Application.ApplicationPurposeType == ApplicationPurposeTypeEnum.GRANT_LICENCE)
                {
                    applicationtype = "(Registration)";
                }
                else if (contractLabourLicence.Application.ApplicationPurposeType == ApplicationPurposeTypeEnum.RENEWAL_LICENCE)
                {
                    applicationtype = "(Renewal)";
                }
                else if (contractLabourLicence.Application.ApplicationPurposeType == ApplicationPurposeTypeEnum.AMENDMENT_LICENCE)
                {
                    applicationtype = "(Amendment)";
                }
                #endregion


                string formattedLastDate = "";

                if (contractLabourLicence.Application.ApplicationPurposeType == ApplicationPurposeTypeEnum.GRANT_LICENCE)
                {
                    int approveldate = (int)contractLabourLicence.LicenceForYear;
                    DateTime lastDateOfCurrentYear = new DateTime(approveldate, 12, 31);
                    formattedLastDate = lastDateOfCurrentYear.ToString("dd MMMM yyyy");
                }

                else if (contractLabourLicence.Application.ApplicationPurposeType == ApplicationPurposeTypeEnum.RENEWAL_LICENCE)
                {
                    int approveldate = (int)contractLabourLicence.LicenceForYear;
                    DateTime lastDateOfCurrentYear = new DateTime(approveldate, 12, 31);
                    formattedLastDate = lastDateOfCurrentYear.ToString("dd MMMM yyyy");
                }
                else if (contractLabourLicence.Application.ApplicationPurposeType == ApplicationPurposeTypeEnum.AMENDMENT_LICENCE)
                {

                    int approveldate = (int)contractLabourLicence.LicenceForYear;
                    DateTime lastDateOfCurrentYear = new DateTime(approveldate, 12, 31);
                    formattedLastDate = lastDateOfCurrentYear.ToString("dd MMMM yyyy");
                }


                string fontpath1 = "Content/fonts/VERDANA.TTF";
                BaseFont customfont = BaseFont.CreateFont(fontpath1, BaseFont.CP1252, BaseFont.EMBEDDED);
                string fontpath2 = "Content/fonts/VERDANA.TTF";
                BaseFont customfontimpact = BaseFont.CreateFont(fontpath2, BaseFont.CP1252, BaseFont.EMBEDDED);
                documents.Open();
                Font verdanaHeader = FontFactory.GetFont("Arial", 9, Font.BOLD);
                Font footer = FontFactory.GetFont("Arial", 9, Font.BOLD, BaseColor.BLUE);
                Font verdana = FontFactory.GetFont("Arial", 8, BaseColor.BLACK);
                Font verdanaBoldItalic = FontFactory.GetFont("Arial", 8, Font.BOLDITALIC);
                Font verdanasmall = FontFactory.GetFont("Arial", 8, Font.NORMAL);
                verdanasmall.Color = BaseColor.WHITE;
                Font verdanaRightHeader = FontFactory.GetFont("Arial", 15, Font.BOLD);
                verdanaRightHeader.Color = BaseColor.WHITE;
                Font verdanaRightHeader1 = FontFactory.GetFont("Arial", 12, Font.BOLD);
                verdanaRightHeader1.Color = BaseColor.WHITE;
                Font arialOldLicenceDetails = FontFactory.GetFont("Arial", 10, Font.BOLD);
                arialOldLicenceDetails.Color = BaseColor.WHITE;
                Font monotypeHeader = new Font(customfont, 20);
                Font monotype = FontFactory.GetFont("Arial", 13, Font.BOLD);
                Font impact = new Font(customfontimpact, 12);
                Font impactHeader = new Font(customfontimpact, 11, Font.BOLD);
                impactHeader.Color = BaseColor.WHITE;
                BaseFont bfTimes = BaseFont.CreateFont(BaseFont.TIMES_ROMAN, BaseFont.CP1252, false);
                Font timesRoman = FontFactory.GetFont("Arial", 10, BaseColor.BLACK);
                Font ArialFont = FontFactory.GetFont("Arial", 8, BaseColor.WHITE);
                Font SealFontArial = FontFactory.GetFont("Arial", 9, BaseColor.BLACK);
                Font verdanaBold = FontFactory.GetFont("Arial", 10, Font.BOLD);
                Font verdanalic = FontFactory.GetFont("Arial", 9, Font.BOLD);
                verdanalic.Color = BaseColor.WHITE;
                var principalEmployerPerforma = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "LicensePerformas")).Root + "CertificatePerforma_Principal_Employer.jpg";
                iTextSharp.text.Image pageLogo1 = iTextSharp.text.Image.GetInstance(new FileStream(principalEmployerPerforma, FileMode.OpenOrCreate, FileAccess.Read));

                pageLogo1.Alignment = iTextSharp.text.Image.ALIGN_CENTER | iTextSharp.text.Image.UNDERLYING;
                pageLogo1.ScaleToFit(800f, 800f);
                pageLogo1.SetAbsolutePosition(10f, 25f);
                pageLogo1.BorderWidth = 1;
                documents.Add(pageLogo1);
                PdfPTable ptableleftSideDetail = new PdfPTable(1);
                ptableleftSideDetail.DefaultCell.Border = 0;
                ptableleftSideDetail.DefaultCell.Padding = 0;
                ptableleftSideDetail.HorizontalAlignment = (Element.ALIGN_JUSTIFIED);
                ptableleftSideDetail.DefaultCell.VerticalAlignment = (Element.ALIGN_JUSTIFIED);
                ptableleftSideDetail.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
                ptableleftSideDetail.AddCell(new PdfPCell(new Phrase("", verdanaBold)) { Border = 0, PaddingTop = 10 });


                ptableleftSideDetail.AddCell(new PdfPCell(new Phrase("Licence No. : " + licenceNo.ToString(), verdanalic)) { Border = 0, PaddingTop = 300, PaddingLeft = -18 }); ;
                ptableleftSideDetail.AddCell(new PdfPCell(new Phrase("Licence Issued On : " + String.Format("{0:dd-MMM-yyyy}", DateTime.Now.ToString("dd/MM/yyyy")), verdanalic)) { Border = 0, PaddingTop = 10, PaddingLeft = -18 });
                ptableleftSideDetail.AddCell(new PdfPCell(new Phrase("Valid Upto : " + String.Format("{0:dd-MMM-yyyy}", formattedLastDate), verdanalic)) { Border = 0, PaddingTop = 10, PaddingLeft = -18 });
                Font verdanaBoldBP = new Font(BaseFont.CreateFont(BaseFont.TIMES_ROMAN, BaseFont.CP1252, false), 8, Font.BOLD | Font.UNDERLINE, BaseColor.WHITE);
                Phrase phrase = new Phrase();

                ptableleftSideDetail.AddCell(new PdfPCell(phrase) { Border = 0, PaddingTop = 10, PaddingRight = 20 });

                #region QR Code Generate
                var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(contractLabourLicence.Application.PublicAppRefNum);
                //string base64String = System.Convert.ToBase64String(plainTextBytes);
                var qrCodeDetails = "Establishment Name : " + projectSite.EstablishmentName + "|" + "Establishment Address : " + projectSite.Address + "|" + "IPIN : " + contractLabourLicence.Application.InvestPunjab_Ipin + "|" + "Contact No : " + projectSite.ContactPersonFirstName + " " + projectSite.ContactPersonLastName + "|" + projectSite.ContactPersonMobileNo + "|" + "Licence No : " + licenceNo + "|" + "File No : " + contractLabourLicence.Application.PublicAppRefNum;
                QRCodeGenerator qrGenerator = new QRCodeGenerator();
                QRCodeGenerator.QRCode qrCode = qrGenerator.CreateQrCode(qrCodeDetails, QRCodeGenerator.ECCLevel.Q);

                using (System.Drawing.Bitmap bitMap = qrCode.GetGraphic(20))
                {
                    using (MemoryStream ms = new MemoryStream())
                    {
                        bitMap.Save(ms, ImageFormat.Jpeg);
                        System.Drawing.Image imgQRCode = System.Drawing.Image.FromStream(ms);
                        iTextSharp.text.Image imdf = iTextSharp.text.Image.GetInstance(imgQRCode, System.Drawing.Imaging.ImageFormat.Jpeg);
                        imdf.ScaleAbsoluteHeight(120f);
                        imdf.ScaleAbsoluteWidth(120f);
                        ptableleftSideDetail.AddCell(new PdfPCell(imdf) { Border = 0, PaddingTop = 30, PaddingLeft = 0, PaddingRight = 0 });
                    }
                }
                #endregion

                ptableleftSideDetail.AddCell(new PdfPCell(new Phrase("" + Chunk.NEWLINE + "Department of Labour, Govt. of Punjab" + Chunk.NEWLINE, verdanaRightHeader1)) { Border = 0, PaddingTop = 15, PaddingRight = 25, HorizontalAlignment = PdfPCell.ALIGN_CENTER });

                PdfPTable ptablerightSideDetail = new PdfPTable(1);
                ptablerightSideDetail.DefaultCell.Border = 0;
                ptablerightSideDetail.DefaultCell.Padding = 0;

                //ptablerightSideDetail.AddCell(new PdfPCell(new Phrase("", verdanaBold)) { Border = 0, Padding = 4 });
                ptablerightSideDetail.AddCell(new PdfPCell(new Phrase("FORM-VI", verdanaHeader)) { Border = 0, PaddingLeft = 20, PaddingTop = 10, HorizontalAlignment = PdfPCell.ALIGN_CENTER });
                ptablerightSideDetail.AddCell(new PdfPCell(new Phrase("[See Rule 25 (1)]", verdanaHeader)) { Border = 0, PaddingLeft = 20, HorizontalAlignment = PdfPCell.ALIGN_CENTER });
                ptablerightSideDetail.AddCell(new PdfPCell(new Phrase("LICENSING OF CONTRACTOR UNDER THE CONTRACT LABOUR (R & A) ACT, 1970 ", impactHeader)) { Border = 0, PaddingTop = 17, PaddingBottom = 20, PaddingLeft = 40, PaddingRight = 30, VerticalAlignment = PdfPCell.ALIGN_MIDDLE, HorizontalAlignment = PdfPCell.ALIGN_CENTER });
                ptablerightSideDetail.AddCell(new PdfPCell(new Phrase("CERTIFICATE OF REGISTRATION", monotype)) { Border = 0, PaddingBottom = 4, PaddingTop = 14, HorizontalAlignment = PdfPCell.ALIGN_CENTER });
                ptablerightSideDetail.AddCell(new PdfPCell(new Phrase("GOVERNMENT OF PUNJAB", monotype)) { Border = 0, PaddingBottom = 4, HorizontalAlignment = PdfPCell.ALIGN_CENTER });
                ptablerightSideDetail.AddCell(new PdfPCell(new Phrase("", verdanaBold)) { Border = 0, Padding = 1, PaddingTop = 10 });
                ptablerightSideDetail.AddCell(new PdfPCell(new Phrase(" Dated: " + DateTime.Now.ToString("dd/MM/yyyy") + "                                Licence No. : " + licenceNo.ToString(), verdanaHeader)) { Border = 0 });

                Paragraph para1 = new Paragraph();
                para1.Add(new Chunk(Chunk.NEWLINE + "", verdanaBoldItalic));
                para1.Add(new Chunk(Chunk.NEWLINE + "1. Licence is hereby granted to ", timesRoman));
                para1.Add(new Chunk(contractLabourLicence.ContractorName, verdanaBold).SetUnderline(0.1f, -2f));
                para1.Add(new Chunk(Chunk.NEWLINE + "work under ", timesRoman));
                para1.Add(new Chunk(contractLabourLicence.EstablishmentName, verdanaBold).SetUnderline(0.1f, -2f));
                para1.Add(new Chunk(Chunk.NEWLINE + "under section 12(1) of the Contract Labour (Regulation and Abolition) Act, 1970, subject to the conditions specified in Annexure). ", timesRoman));
                para1.Add(new Chunk("", timesRoman));
                para1.Add(new Chunk("", timesRoman));
                para1.Add(new Chunk(Chunk.NEWLINE + "2. This licence is for doing the work of ", timesRoman));
                para1.Add(new Chunk(contractLabourLicence.NatureOfWork, verdanaBold).SetUnderline(0.1f, -2f));
                para1.Add(new Chunk("(nature of work to be indicated) in the establishment of ", timesRoman));
                para1.Add(new Chunk(contractLabourLicence.PrincipalEmployerName, verdanaBold).SetUnderline(0.1f, -2f));
                para1.Add(new Chunk(" (name of principal employer to be indicated) of ", timesRoman));
                para1.Add(new Chunk(contractLabourLicence.PrincipalEmployerAddress, verdanaBold).SetUnderline(0.1f, -2f));
                para1.Add(new Chunk("(place of work to be indicated).", timesRoman));

                para1.Add(new Chunk("", timesRoman));
                para1.Add(new Chunk(Chunk.NEWLINE + "3. The Licence shall remain in force till ", timesRoman));
                para1.Add(new Chunk(formattedLastDate, verdanaBold).SetUnderline(0.1f, -2f));
                para1.Add(new Chunk(Chunk.NEWLINE + "4. The Licence valid for number of workers upto ", timesRoman));
                para1.Add(new Chunk(contractLabourLicence.MaximumNumberOfEmployee.ToString(), timesRoman));
                PdfPCell shipment1 = new PdfPCell(para1);
                shipment1.SetLeading(1f, 2f);
                ptablerightSideDetail.AddCell(new PdfPCell(shipment1) { Border = 0, HorizontalAlignment = PdfPCell.ALIGN_JUSTIFIED, VerticalAlignment = PdfPCell.ALIGN_JUSTIFIED, FixedHeight = 430, PaddingBottom = 15 });
                var userDetails = await _iAuthService.GetUserFullDetailByUserName(userName);
                string officerSigFilePath = "";
                if (!isDeemedCalling)
                {

                    if (userDetails.ListData.Count() == 0 || userDetails.ListData[0].Signature == null)
                    {
                        userDetails.ListData[0].Signature = "abc.jpg";
                    }
                    officerSigFilePath = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "OfficerSignatures")).Root + userDetails.ListData[0].Signature;
                    iTextSharp.text.Image signature = iTextSharp.text.Image.GetInstance(new FileStream(officerSigFilePath, FileMode.OpenOrCreate, FileAccess.Read));
                    signature.ScaleToFit(120f, 120f);
                    signature.SetAbsolutePosition(630f, 150f);
                    ptablerightSideDetail.AddCell(new PdfPCell(signature) { Border = 0, PaddingTop = 5, HorizontalAlignment = PdfPCell.ALIGN_RIGHT });
                }

                Paragraph SignSeal = new Paragraph();
                SignSeal.Add(new Chunk("Name: ", verdanaHeader));
                SignSeal.Add(new Chunk(userDetails.ListData[0].UserFullName, SealFontArial));
                SignSeal.Add(new Chunk(Chunk.NEWLINE + "Circle Name: ", verdanaHeader));
                SignSeal.Add(new Chunk(alcCircle.ALCCircleName, SealFontArial));
                //SignSeal.Add(new Chunk(Chunk.NEWLINE + "District Name: ", verdanaHeader));
                //SignSeal.Add(new Chunk(labourCircle.LabourCircleName, SealFontArial));
                SignSeal.Add(new Chunk(Chunk.NEWLINE + "Date: ", verdanaHeader));
                SignSeal.Add(new Chunk(DateTime.Now.ToShortDateString(), SealFontArial));
                SignSeal.Add(new Chunk(Chunk.NEWLINE + "Time: ", verdanaHeader));
                SignSeal.Add(new Chunk(DateTime.Now.ToString("dd/MM/yyyy"), SealFontArial));

                ptablerightSideDetail.AddCell(new PdfPCell(SignSeal) { Border = 0, HorizontalAlignment = PdfPCell.ALIGN_LEFT, PaddingLeft = 200 });
                //ptablerightSideDetail.AddCell(new PdfPCell(new Phrase(" Dated " + DateTime.Now.ToShortDateString(), verdanaHeader)) { Border = 0, HorizontalAlignment = PdfPCell.ALIGN_LEFT});

                ptablerightSideDetail.AddCell(new PdfPCell(new Phrase("This is a computer generated document to check & verify please login to https://pblabour.gov.in/Account/ApplicationStatus", footer)) { Border = 1, BorderWidth = 1, Padding = 5 });
                PdfPTable ptablePrincipalDetailLicenceApplied = new PdfPTable(3);
                ptablePrincipalDetailLicenceApplied.DefaultCell.Border = 0;
                ptablePrincipalDetailLicenceApplied.DefaultCell.Padding = 2;
                ptablePrincipalDetailLicenceApplied.SpacingBefore = 1000;
                ptablePrincipalDetailLicenceApplied.AddCell(new PdfPCell(ptableleftSideDetail) { Border = 0 });
                ptablePrincipalDetailLicenceApplied.AddCell(new PdfPCell(ptablerightSideDetail) { Border = 0, Colspan = 2 });
                documents.Add(ptablePrincipalDetailLicenceApplied);
                documents.NewPage();
                PdfPTable ptableLabourAnnexure = new PdfPTable(7);
                ptableLabourAnnexure.DefaultCell.Border = 0;
                ptableLabourAnnexure.DefaultCell.Padding = 2;
                int sno = 1;
                ptableLabourAnnexure.AddCell(new PdfPCell(new Phrase("ANNEXURE", verdanaHeader)) { Padding = 20, PaddingLeft = 220, Colspan = 7, Border = 0, Left = 100 });
                ptableLabourAnnexure.AddCell(new PdfPCell(new Phrase("The licence is subject to the following conditions :-", verdana)) { Padding = 20, Colspan = 7, Border = 0, Left = 40 });
                ptableLabourAnnexure.AddCell(new PdfPCell(new Phrase((sno++).ToString(), verdana)) { Border = 1, BorderWidthTop = 1, BorderWidthBottom = 1, BorderWidthLeft = 1, BorderWidthRight = 1 });
                ptableLabourAnnexure.AddCell(new PdfPCell(new Phrase("The licence shall be non-transferable.", verdana)) { Border = 1, BorderWidthTop = 1, BorderWidthBottom = 1, BorderWidthRight = 1, Colspan = 6, Padding = 6 });
                ptableLabourAnnexure.AddCell(new PdfPCell(new Phrase((sno++).ToString(), verdana)) { Border = 1, BorderWidthTop = 1, BorderWidthBottom = 1, BorderWidthLeft = 1, BorderWidthRight = 1 });
                ptableLabourAnnexure.AddCell(new PdfPCell(new Phrase("The number of workmen employed as contract labour in the establishment shall not, on any day exceed " + contractLabourLicence.MaximumNumberOfEmployee, verdana)) { Border = 1, BorderWidthTop = 1, BorderWidthBottom = 1, BorderWidthRight = 1, Colspan = 6, Padding = 6 });
                ptableLabourAnnexure.AddCell(new PdfPCell(new Phrase((sno++).ToString(), verdana)) { Border = 1, BorderWidthTop = 0, BorderWidthBottom = 1, BorderWidthLeft = 1, BorderWidthRight = 1 });
                ptableLabourAnnexure.AddCell(new PdfPCell(new Phrase("Except as provided in the rules the fees paid for the grant or as the case may be, for renewal of the licence shall be non-refundable .", verdana)) { Border = 1, BorderWidthTop = 0, BorderWidthBottom = 1, BorderWidthRight = 1, Colspan = 6, Padding = 6 });
                ptableLabourAnnexure.AddCell(new PdfPCell(new Phrase((sno++).ToString(), verdana)) { Border = 1, BorderWidthTop = 0, BorderWidthBottom = 1, BorderWidthLeft = 1, BorderWidthRight = 1 });
                ptableLabourAnnexure.AddCell(new PdfPCell(new Phrase("The rates of wages payble to the workmen by the contractor shall not be less than the rates prescribed for the Schedule of employment under the Minimum Wages Act, 1948, where applicable, and where the rates have been fixed by agreement, settlement or award, not less than the  rates fixed.", verdana)) { Border = 1, BorderWidthTop = 0, BorderWidthBottom = 1, BorderWidthRight = 1, Colspan = 6, Padding = 6 });
                ptableLabourAnnexure.AddCell(new PdfPCell(new Phrase((sno++).ToString(), verdana)) { Border = 1, BorderWidthTop = 0, BorderWidthBottom = 1, BorderWidthLeft = 1, BorderWidthRight = 1 });
                ptableLabourAnnexure.AddCell(new PdfPCell(new Phrase("In case where the workmen employed by the contractor perform the same or similar kind of work as the workmen directly employed by the principal employer of the establishment, the wages rates, holidays, hours of work and other conditions of service of the workmen of the contractor shall be the same as applicable to the workmen directly employed by the principal employer of the establishment on the same or similar kind of work: Provided that in the case of any disagreement with regard to the type of work the same shall be decided by the [Deputy Chief Labour Commissioner (Central)] whose decision shall be final.", verdana)) { Border = 1, BorderWidthTop = 0, BorderWidthBottom = 1, BorderWidthRight = 1, Colspan = 6, Padding = 6 });
                ptableLabourAnnexure.AddCell(new PdfPCell(new Phrase("", verdana)) { Border = 1, BorderWidthTop = 0, BorderWidthBottom = 1, BorderWidthLeft = 1, BorderWidthRight = 1 });
                ptableLabourAnnexure.AddCell(new PdfPCell(new Phrase("  Provided that in the case of any disagreement with regard to the type of work the same shall be decided by the [Deputy Chief Labour Commissioner (Central)] whose decision shall be final.", verdana)) { Border = 1, BorderWidthTop = 0, BorderWidthBottom = 1, BorderWidthRight = 1, Colspan = 6, Padding = 6 });
                ptableLabourAnnexure.AddCell(new PdfPCell(new Phrase((sno++).ToString(), verdana)) { Border = 1, BorderWidthTop = 0, BorderWidthBottom = 1, BorderWidthLeft = 1, BorderWidthRight = 1 });
                ptableLabourAnnexure.AddCell(new PdfPCell(new Phrase("In other case the wages rates, holidays, hours of work and condistions of service of the workmen of the contractor shall be such as may be specified in this  behalf by the [Deputy Chief Labour Commissioner (Central)].", verdana)) { Border = 1, BorderWidthTop = 0, BorderWidthBottom = 1, BorderWidthRight = 1, Colspan = 6, Padding = 6 });
                ptableLabourAnnexure.AddCell(new PdfPCell(new Phrase((sno++).ToString(), verdana)) { Border = 1, BorderWidthTop = 0, BorderWidthBottom = 1, BorderWidthLeft = 1, BorderWidthRight = 1 });
                ptableLabourAnnexure.AddCell(new PdfPCell(new Phrase("In every establishment where 20  or more women are ordinarily employed as contract labour there shall be provided 2 rooms of reasonable dimension for the use of their children under the age of six years. One of such rooms would be used as a play room for the children and the other as bed room for the children. For this purpose the contractor shall supply adequate number of toys and games in the play room and sufficient number of cots and beddings in the sleeping rooms. The standard of construction and maintenance of the creches may be such as may be specified in this behalf by the [Deputy Chief Labour Commissioner (Central)].", verdana)) { Border = 1, BorderWidthTop = 0, BorderWidthBottom = 1, BorderWidthRight = 1, Colspan = 6, Padding = 6 });
                ptableLabourAnnexure.AddCell(new PdfPCell(new Phrase((sno++).ToString(), verdana)) { Border = 1, BorderWidthTop = 0, BorderWidthBottom = 1, BorderWidthLeft = 1, BorderWidthRight = 1 });
                ptableLabourAnnexure.AddCell(new PdfPCell(new Phrase("The license shall notify any change in the number of workmen or the condistions of work to the Licensing Officer.", verdana)) { Border = 1, BorderWidthTop = 0, BorderWidthBottom = 1, BorderWidthRight = 1, Colspan = 6, Padding = 6 });
                ptableLabourAnnexure.AddCell(new PdfPCell(new Phrase((sno++).ToString(), verdana)) { Border = 1, BorderWidthTop = 0, BorderWidthBottom = 1, BorderWidthLeft = 1, BorderWidthRight = 1 });
                ptableLabourAnnexure.AddCell(new PdfPCell(new Phrase("[A copy of the licence shall be displayed prominently at the premises where the contract work is being carried on ]", verdana)) { Border = 1, BorderWidthTop = 0, BorderWidthBottom = 1, BorderWidthRight = 1, Colspan = 6, Padding = 6 });
                documents.Add(ptableLabourAnnexure);
                documents.Close();
                PDFPageEvents pageeventsobj = new PDFPageEvents();



                //ptablerightSideDetail.AddCell(new PdfPCell(para) { Border = 0, VerticalAlignment = PdfPCell.ALIGN_JUSTIFIED, HorizontalAlignment = PdfPCell.ALIGN_JUSTIFIED, FixedHeight = 400 });
            }
            catch (Exception ex)
            {
                genericFormModel.HasException = true;
                genericFormModel.Exceptions = ex.Message;
            }
            return genericFormModel;
        }
        private async Task<CertificateGenerateServiceResultTemplate> Certificate_AppForm_36(Int64 appRefId, string CertificatePerformaURL, ApplicationTypeEnum applicationType, string userName, string licenceNo, bool isDeemedCalling)
        {
            CertificateGenerateServiceResultTemplate genericFormModel = new CertificateGenerateServiceResultTemplate();
            try
            {
                Guid guId = Guid.NewGuid();
                genericFormModel.PdfNameGUID = guId.ToString();
                var sfilepath = "";
                sfilepath = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "TempFiles")).Root + guId.ToString() + ".pdf";
                FileInfo file = new FileInfo(sfilepath);
                if (file.Exists)
                {
                    file.Delete();
                }
                iTextSharp.text.Document documents = new iTextSharp.text.Document(iTextSharp.text.PageSize.A4, 5, 5, 30, 30);
                documents.Open();
                #region ClassObjects
                pdfFonts classFonts = new pdfFonts();
                #endregion

                #region CreateObjects
                PdfWriter owriter = PdfWriter.GetInstance(documents, new FileStream(sfilepath, FileMode.Create));
                PDFPageEvents pageEvent = new PDFPageEvents();
                owriter.PageEvent = pageEvent;
                owriter.SetEncryption(PdfWriter.STRENGTH128BITS, "", "", PdfWriter.AllowPrinting);
                #endregion

                var fontName = "Verdana";
                if (!FontFactory.IsRegistered(fontName))
                {
                    var fontPath = "Content/fonts/VERDANA.TTF";
                    FontFactory.Register(fontPath);
                }

                #region Tables
                var parentWithChildObject = await _iGR_Licence_MotorTransport
                      .GetAsync(x => x.AppRefId == appRefId,  //Conditions         
                        null,                //Orders          
                        x => x.Application, x => x.Application.ApplicationAction, x => x.Application.AppFeeDetails)      //Includes
                      .ConfigureAwait(false);
                var genralDetail = parentWithChildObject.FirstOrDefault();
                var projectSite = _context.ProjectSites.Where(x => x.ProjectSiteId == genralDetail.Application.ProjectSiteRefId).FirstOrDefault();
                var alcCircle = _context.ALCCircles.Where(x => x.ALCCircleId == projectSite.AlcCircleRefId).FirstOrDefault();
                #endregion

                string formattedLastDate = "";

                if (genralDetail.Application.ApplicationPurposeType == ApplicationPurposeTypeEnum.GRANT_LICENCE)
                {
                    int approveldate = (int)genralDetail.LicenceForYear;
                    DateTime lastDateOfCurrentYear = new DateTime(approveldate, 12, 31);
                    formattedLastDate = lastDateOfCurrentYear.ToString("dd MMMM yyyy");
                }

                else if (genralDetail.Application.ApplicationPurposeType == ApplicationPurposeTypeEnum.RENEWAL_LICENCE)
                {
                    int approveldate = (int)genralDetail.LicenceForYear;
                    DateTime lastDateOfCurrentYear = new DateTime(approveldate, 12, 31);
                    formattedLastDate = lastDateOfCurrentYear.ToString("dd MMMM yyyy");
                }
                else if (genralDetail.Application.ApplicationPurposeType == ApplicationPurposeTypeEnum.AMENDMENT_LICENCE)
                {

                    int approveldate = (int)genralDetail.LicenceForYear;
                    DateTime lastDateOfCurrentYear = new DateTime(approveldate, 12, 31);
                    formattedLastDate = lastDateOfCurrentYear.ToString("dd MMMM yyyy");
                }


                string fontpath1 = "Content/fonts/VERDANA.TTF";
                BaseFont customfont = BaseFont.CreateFont(fontpath1, BaseFont.CP1252, BaseFont.EMBEDDED);
                string fontpath2 = "Content/fonts/VERDANA.TTF";
                BaseFont customfontimpact = BaseFont.CreateFont(fontpath2, BaseFont.CP1252, BaseFont.EMBEDDED);
                documents.Open();
                Font verdanaHeader = FontFactory.GetFont("Arial", 9, Font.BOLD);
                Font footer = FontFactory.GetFont("Arial", 9, Font.BOLD, BaseColor.BLUE);
                Font verdana = FontFactory.GetFont("Arial", 8, BaseColor.BLACK);
                Font verdanaBoldItalic = FontFactory.GetFont("Arial", 8, Font.BOLDITALIC);
                Font verdanasmall = FontFactory.GetFont("Arial", 8, Font.NORMAL);
                verdanasmall.Color = BaseColor.WHITE;
                Font verdanaRightHeader = FontFactory.GetFont("Arial", 15, Font.BOLD);
                verdanaRightHeader.Color = BaseColor.WHITE;
                Font verdanaRightHeader1 = FontFactory.GetFont("Arial", 12, Font.BOLD);
                verdanaRightHeader1.Color = BaseColor.WHITE;
                Font arialOldLicenceDetails = FontFactory.GetFont("Arial", 10, Font.BOLD);
                arialOldLicenceDetails.Color = BaseColor.WHITE;
                Font monotypeHeader = new Font(customfont, 20);
                Font monotype = FontFactory.GetFont("Arial", 13, Font.BOLD);
                Font impact = new Font(customfontimpact, 12);
                Font impactHeader = new Font(customfontimpact, 11, Font.BOLD);
                impactHeader.Color = BaseColor.WHITE;
                BaseFont bfTimes = BaseFont.CreateFont(BaseFont.TIMES_ROMAN, BaseFont.CP1252, false);
                Font timesRoman = FontFactory.GetFont("Arial", 10, BaseColor.BLACK);
                Font ArialFont = FontFactory.GetFont("Arial", 8, BaseColor.WHITE);
                Font SealFontArial = FontFactory.GetFont("Arial", 9, BaseColor.BLACK);
                Font verdanaBold = FontFactory.GetFont("Arial", 10, Font.BOLD);
                Font verdanalic = FontFactory.GetFont("Arial", 9, Font.BOLD);
                verdanalic.Color = BaseColor.WHITE;
                var factoryPerforma = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "LicensePerformas")).Root + "CertificatePerforma_MotorTransport.jpg";
                iTextSharp.text.Image pageLogo1 = iTextSharp.text.Image.GetInstance(new FileStream(factoryPerforma, FileMode.OpenOrCreate, FileAccess.Read));

                pageLogo1.Alignment = iTextSharp.text.Image.ALIGN_CENTER | iTextSharp.text.Image.UNDERLYING;
                pageLogo1.ScaleToFit(800f, 800f);
                pageLogo1.SetAbsolutePosition(10f, 25f);
                pageLogo1.BorderWidth = 1;
                documents.Add(pageLogo1);
                PdfPTable ptableleftSideDetail = new PdfPTable(1);
                ptableleftSideDetail.DefaultCell.Border = 0;
                ptableleftSideDetail.DefaultCell.Padding = 0;
                ptableleftSideDetail.HorizontalAlignment = (Element.ALIGN_JUSTIFIED);
                ptableleftSideDetail.DefaultCell.VerticalAlignment = (Element.ALIGN_JUSTIFIED);
                ptableleftSideDetail.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
                ptableleftSideDetail.AddCell(new PdfPCell(new Phrase("", verdanaBold)) { Border = 0, PaddingTop = 10 });



                ptableleftSideDetail.AddCell(new PdfPCell(new Phrase("Registration No. : " + licenceNo.ToString(), verdanalic)) { Border = 0, PaddingTop = 300, PaddingLeft = -18 }); ;
                ptableleftSideDetail.AddCell(new PdfPCell(new Phrase("Registration Issued On : " + String.Format("{0:dd-MMM-yyyy}", DateTime.Now.ToShortDateString()), verdanalic)) { Border = 0, PaddingTop = 10, PaddingLeft = -18 });
                ptableleftSideDetail.AddCell(new PdfPCell(new Phrase("Valid upto  : " + formattedLastDate, verdanalic)) { Border = 0, PaddingTop = 10, PaddingLeft = -18 });

                Font verdanaBoldBP = new Font(BaseFont.CreateFont(BaseFont.TIMES_ROMAN, BaseFont.CP1252, false), 8, Font.BOLD | Font.UNDERLINE, BaseColor.WHITE);
                Phrase phrase = new Phrase();

                ptableleftSideDetail.AddCell(new PdfPCell(phrase) { Border = 0, PaddingTop = 10, PaddingRight = 20 });

                #region QR Code Generate
                var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(genralDetail.Application.PublicAppRefNum);
                string base64String = System.Convert.ToBase64String(plainTextBytes);
                QRCodeGenerator qrGenerator = new QRCodeGenerator();
                QRCodeGenerator.QRCode qrCode = qrGenerator.CreateQrCode(base64String, QRCodeGenerator.ECCLevel.Q);

                using (System.Drawing.Bitmap bitMap = qrCode.GetGraphic(20))
                {
                    using (MemoryStream ms = new MemoryStream())
                    {
                        bitMap.Save(ms, ImageFormat.Jpeg);
                        System.Drawing.Image imgQRCode = System.Drawing.Image.FromStream(ms);
                        iTextSharp.text.Image imdf = iTextSharp.text.Image.GetInstance(imgQRCode, System.Drawing.Imaging.ImageFormat.Jpeg);
                        imdf.ScaleAbsoluteHeight(120f);
                        imdf.ScaleAbsoluteWidth(120f);
                        ptableleftSideDetail.AddCell(new PdfPCell(imdf) { Border = 0, PaddingTop = 30, PaddingLeft = 0, PaddingRight = 0 });
                    }
                }
                #endregion

                ptableleftSideDetail.AddCell(new PdfPCell(new Phrase("" + Chunk.NEWLINE + "Department of Labour, Govt. of Punjab" + Chunk.NEWLINE, verdanaRightHeader1)) { Border = 0, PaddingTop = 15, PaddingRight = 25, HorizontalAlignment = PdfPCell.ALIGN_CENTER });

                PdfPTable ptablerightSideDetail = new PdfPTable(1);
                ptablerightSideDetail.DefaultCell.Border = 0;
                ptablerightSideDetail.DefaultCell.Padding = 0;

                //ptablerightSideDetail.AddCell(new PdfPCell(new Phrase("", verdanaBold)) { Border = 0, Padding = 4 });
                ptablerightSideDetail.AddCell(new PdfPCell(new Phrase("FORM-II", verdanaHeader)) { Border = 0, PaddingLeft = 20, PaddingTop = 10, HorizontalAlignment = PdfPCell.ALIGN_CENTER });
                ptablerightSideDetail.AddCell(new PdfPCell(new Phrase("[See Rule 5]", verdanaHeader)) { Border = 0, PaddingLeft = 20, HorizontalAlignment = PdfPCell.ALIGN_CENTER });
                ptablerightSideDetail.AddCell(new PdfPCell(new Phrase("REGISTRATION CERTIFICATE OF MOTOR TRANSPORT WORKERS UNDER THE MOTOR TRANSPORT WORKERS ACT, 1961", impactHeader)) { Border = 0, PaddingTop = 25, PaddingBottom = 20, PaddingLeft = 40, PaddingRight = 30, VerticalAlignment = PdfPCell.ALIGN_MIDDLE, HorizontalAlignment = PdfPCell.ALIGN_CENTER });
                ptablerightSideDetail.AddCell(new PdfPCell(new Phrase("CERTIFICATE OF REGISTRATION", monotype)) { Border = 0, PaddingBottom = 4, PaddingTop = 10, HorizontalAlignment = PdfPCell.ALIGN_CENTER });
                ptablerightSideDetail.AddCell(new PdfPCell(new Phrase("GOVERNMENT OF PUNJAB", monotype)) { Border = 0, PaddingBottom = 4, HorizontalAlignment = PdfPCell.ALIGN_CENTER });
                ptablerightSideDetail.AddCell(new PdfPCell(new Phrase("", verdanaBold)) { Border = 0, Padding = 1, PaddingTop = 6 });
                ptablerightSideDetail.AddCell(new PdfPCell(new Phrase(" Dated: " + DateTime.Now.ToShortDateString() + "                            Registration No. : " + licenceNo.ToString(), verdanaHeader)) { Border = 0 });


                Paragraph para1 = new Paragraph();
                para1.Add(new Chunk(" Certificate  of  Registration  is  hereby  granted ", timesRoman));
                para1.Add(new Chunk(genralDetail.TransportServiceName, verdanaBold));

                para1.Add(new Chunk(" to operate motor transport services employing not more than", timesRoman));
                para1.Add(new Chunk(genralDetail.MaxTransportWorkers + "", verdanaBoldItalic));
                para1.Add(new Chunk(Chunk.NEWLINE + "persons on any one day during the year subject to provisions of the Motor Transport Workers Act, 1961 and the Rules made thereunder. The certificate of Registration shall remain in force till the 31st Day of December ", timesRoman));
                para1.Add(new Chunk(formattedLastDate, verdanaBold));
                PdfPCell shipment1 = new PdfPCell(para1);
                shipment1.SetLeading(2f, 2f);
                ptablerightSideDetail.AddCell(new PdfPCell(shipment1) { Border = 0, HorizontalAlignment = PdfPCell.ALIGN_JUSTIFIED, VerticalAlignment = PdfPCell.ALIGN_JUSTIFIED, FixedHeight = 350, PaddingBottom = 25 });



                var userDetails = await _iAuthService.GetUserFullDetailByUserName(userName);
                string officerSigFilePath = "";
                if (!isDeemedCalling)
                {

                    if (userDetails.ListData.Count() == 0 || userDetails.ListData[0].Signature == null)
                    {
                        userDetails.ListData[0].Signature = "abc.jpg";
                    }
                    officerSigFilePath = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "OfficerSignatures")).Root + userDetails.ListData[0].Signature;
                    iTextSharp.text.Image signature = iTextSharp.text.Image.GetInstance(new FileStream(officerSigFilePath, FileMode.OpenOrCreate, FileAccess.Read));
                    signature.ScaleToFit(120f, 120f);
                    signature.SetAbsolutePosition(630f, 150f);
                    ptablerightSideDetail.AddCell(new PdfPCell(signature) { Border = 0, PaddingTop = 5, HorizontalAlignment = PdfPCell.ALIGN_RIGHT });
                }

                Paragraph SignSeal = new Paragraph();
                //SignSeal.Add(new Chunk("Name: ", verdanaHeader));
                //SignSeal.Add(new Chunk(userDetails.ListData[0].UserFullName, SealFontArial));
                //SignSeal.Add(new Chunk(Chunk.NEWLINE + "Desig: ", verdanaHeader));
                SignSeal.Add(new Chunk(Chunk.NEWLINE + "Circle Name: ", verdanaHeader));
                SignSeal.Add(new Chunk(alcCircle.ALCCircleName, SealFontArial));
                SignSeal.Add(new Chunk(Chunk.NEWLINE + "District Name: ", verdanaHeader));
                SignSeal.Add(new Chunk(alcCircle.ALCCircleName, SealFontArial));
                SignSeal.Add(new Chunk(Chunk.NEWLINE + "Date: ", verdanaHeader));
                SignSeal.Add(new Chunk(DateTime.Now.ToShortDateString(), SealFontArial));
                SignSeal.Add(new Chunk(Chunk.NEWLINE + "Time: ", verdanaHeader));
                SignSeal.Add(new Chunk(DateTime.Now.ToShortTimeString(), SealFontArial));

                ptablerightSideDetail.AddCell(new PdfPCell(SignSeal) { Border = 0, HorizontalAlignment = PdfPCell.ALIGN_LEFT, PaddingLeft = 200 });
                //ptablerightSideDetail.AddCell(new PdfPCell(new Phrase(" Dated " + DateTime.Now.ToShortDateString(), verdanaHeader)) { Border = 0, HorizontalAlignment = PdfPCell.ALIGN_LEFT});

                ptablerightSideDetail.AddCell(new PdfPCell(new Phrase("This is a computer generated document to check & verify please login to https://pblabour.gov.in/Account/ApplicationStatus", footer)) { Border = 1, BorderWidth = 1, Padding = 8 });
                PdfPTable ptableShopDetailLicenceApplied = new PdfPTable(3);
                ptableShopDetailLicenceApplied.DefaultCell.Border = 0;
                ptableShopDetailLicenceApplied.DefaultCell.Padding = 2;
                ptableShopDetailLicenceApplied.SpacingBefore = 1000;
                ptableShopDetailLicenceApplied.AddCell(new PdfPCell(ptableleftSideDetail) { Border = 0 });
                ptableShopDetailLicenceApplied.AddCell(new PdfPCell(ptablerightSideDetail) { Border = 0, Colspan = 2 });
                documents.Add(ptableShopDetailLicenceApplied);
                documents.Close();
                PDFPageEvents pageeventsobj = new PDFPageEvents();

                //ptablerightSideDetail.AddCell(new PdfPCell(para) { Border = 0, VerticalAlignment = PdfPCell.ALIGN_JUSTIFIED, HorizontalAlignment = PdfPCell.ALIGN_JUSTIFIED, FixedHeight = 400 });
            }
            catch (Exception ex)
            {
                genericFormModel.HasException = true;
                genericFormModel.Exceptions = ex.Message;
            }
            return genericFormModel;
        }
        public async Task<CertificateGenerateServiceResultTemplate> AllActGenerateCertificate(Int64 appRefId, ApplicationTypeEnum applicationType, string userName, bool isDeemedCalling)
        {
            CertificateGenerateServiceResultTemplate genericFormModel = new CertificateGenerateServiceResultTemplate();
            try
            {
                GenerateLicenceNoViewModel licenceNo = await GenerateLicenceNo(appRefId);
                string CertificatePerformaURL = string.Empty;
                if (applicationType == ApplicationTypeEnum.PRINCIPAL_EMPLOYER) // Principal Employer
                {
                    genericFormModel = await Certificate_AppForm_37(appRefId, CertificatePerformaURL, applicationType, userName, licenceNo.LicenceNo, isDeemedCalling);
                }
                else if (applicationType == ApplicationTypeEnum.CONTRACT_LABOUR) // Contract Labour
                {
                    genericFormModel = await Certificate_AppForm_38(appRefId, CertificatePerformaURL, applicationType, userName, licenceNo.LicenceNo, isDeemedCalling);
                }
                else if (applicationType == ApplicationTypeEnum.ISM_PRINCIPAL_EMPLOYER) //Inter State Principal Employer
                {
                    genericFormModel = await Certificate_AppForm_39(appRefId, CertificatePerformaURL, applicationType, userName, licenceNo.LicenceNo, isDeemedCalling);
                }
                else if (applicationType == ApplicationTypeEnum.ISM_CONTRACT_LABOUR) //Inter State Contract Labour
                {
                    genericFormModel = await Certificate_AppForm_40(appRefId, CertificatePerformaURL, applicationType, userName, licenceNo.LicenceNo, isDeemedCalling);
                }
                else if (applicationType == ApplicationTypeEnum.SHOP_LICENCE) //Shop Act Licence
                {
                    genericFormModel = await Certificate_AppForm_6(appRefId, CertificatePerformaURL, applicationType, userName, licenceNo.LicenceNo, isDeemedCalling);
                }
                else if (applicationType == ApplicationTypeEnum.BUILDING_PLAN_HUD) //Combined Proposed Approval of Building Plans (HUD and Factories)
                {
                    genericFormModel = await All_Act_Certificate_AppForm_5(appRefId, CertificatePerformaURL, applicationType, userName, licenceNo.LicenceNo, isDeemedCalling);
                }
                else if (applicationType == ApplicationTypeEnum.BUILDING_PLAN_PROPOSED || applicationType == ApplicationTypeEnum.BUILDING_PLAN_EXISTING || applicationType == ApplicationTypeEnum.BUILDING_PLAN_ADDITION_AMENDMENT) //Building Plan Under Factory Act
                {
                    genericFormModel = await All_Act_Certificate_AppForm_71_72_73(appRefId, CertificatePerformaURL, applicationType, userName, licenceNo.LicenceNo, isDeemedCalling);
                }
                else if (applicationType == ApplicationTypeEnum.BUILDING_PLAN_PSIEC) //Combined Building Plans (PSIEC and Factories)
                {
                    genericFormModel = await All_Act_Certificate_AppForm_81(appRefId, CertificatePerformaURL, applicationType, userName, licenceNo.LicenceNo, isDeemedCalling);
                }

            }
            catch (Exception ex)
            {
                genericFormModel.HasException = true;
                genericFormModel.Exceptions = ex.Message;
            }
            return genericFormModel;
        }

        private async Task<CertificateGenerateServiceResultTemplate> Certificate_AppForm_40(Int64 appRefId, string CertificatePerformaURL, ApplicationTypeEnum applicationType, string userName, string licenceNo, bool isDeemedCalling)
        {
            CertificateGenerateServiceResultTemplate genericFormModel = new CertificateGenerateServiceResultTemplate();
            try
            {
                Guid guId = Guid.NewGuid();
                genericFormModel.PdfNameGUID = guId.ToString();
                var sfilepath = "";
                sfilepath = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "TempFiles")).Root + guId.ToString() + ".pdf";
                FileInfo file = new FileInfo(sfilepath);
                if (file.Exists)
                {
                    file.Delete();
                }
                iTextSharp.text.Document documents = new iTextSharp.text.Document(iTextSharp.text.PageSize.A4, 5, 5, 30, 30);
                documents.Open();
                #region ClassObjects
                pdfFonts classFonts = new pdfFonts();
                #endregion

                #region CreateObjects
                PdfWriter owriter = PdfWriter.GetInstance(documents, new FileStream(sfilepath, FileMode.Create));
                PDFPageEvents pageEvent = new PDFPageEvents();
                owriter.PageEvent = pageEvent;
                owriter.SetEncryption(PdfWriter.STRENGTH128BITS, "", "", PdfWriter.AllowPrinting);
                #endregion

                var fontName = "Verdana";
                if (!FontFactory.IsRegistered(fontName))
                {
                    var fontPath = "Content/fonts/VERDANA.TTF";
                    FontFactory.Register(fontPath);
                }

                #region Tables

                var parentWithChildObject = await _iGR_Licence_ISM_ContractLabour_GeneralDetail
                       .GetAsync(x => x.AppRefId == appRefId,  //Conditions         
                         null,                //Orders
                         x => x.Application, x => x.Application.ApplicationAction)      //Includes
                       .ConfigureAwait(false);
                var contractlabourLicence = parentWithChildObject.FirstOrDefault();
                var projectSite = _context.ProjectSites.Where(x => x.ProjectSiteId == contractlabourLicence.Application.ProjectSiteRefId).FirstOrDefault();
                var alcCircle = _context.ALCCircles.Where(x => x.ALCCircleId == projectSite.AlcCircleRefId).FirstOrDefault();
                #endregion

                #region Licence Validity Year
                int licenceValidUpto = 0;
                int year = DateTime.Now.Year;
                string ValidUpto = new DateTime(year, 12, 31).ToString("dd/MM/yyyy");

                DofLicenceDetailsViewModel dofDetails = new DofLicenceDetailsViewModel();
                if (contractlabourLicence.Application.ApplicationPurposeType == ApplicationPurposeTypeEnum.GRANT_LICENCE)
                {
                    DateTime registrationDate = DateTime.MinValue;
                    ValidUpto = new DateTime(year, 12, 31).ToString("dd/MM/yyyy");
                    licenceValidUpto = 1;
                }
                else if (contractlabourLicence.Application.ApplicationPurposeType == ApplicationPurposeTypeEnum.RENEWAL_LICENCE)
                {
                    DateTime registrationDate = DateTime.MinValue;
                    ValidUpto = new DateTime(year + 1, 12, 31).ToString("dd/MM/yyyy");
                    licenceValidUpto = 1;
                }
                #endregion

                string fontpath1 = "Content/fonts/VERDANA.TTF";
                BaseFont customfont = BaseFont.CreateFont(fontpath1, BaseFont.CP1252, BaseFont.EMBEDDED);
                string fontpath2 = "Content/fonts/VERDANA.TTF";
                BaseFont customfontimpact = BaseFont.CreateFont(fontpath2, BaseFont.CP1252, BaseFont.EMBEDDED);
                documents.Open();
                Font verdanaHeader = FontFactory.GetFont("Arial", 9, Font.BOLD);
                Font footer = FontFactory.GetFont("Arial", 9, Font.BOLD, BaseColor.BLUE);
                Font verdana = FontFactory.GetFont("Arial", 8, BaseColor.BLACK);
                Font verdanaBoldItalic = FontFactory.GetFont("Arial", 8, Font.BOLDITALIC);
                Font verdanasmall = FontFactory.GetFont("Arial", 8, Font.NORMAL);
                verdanasmall.Color = BaseColor.WHITE;
                Font verdanaRightHeader = FontFactory.GetFont("Arial", 15, Font.BOLD);
                verdanaRightHeader.Color = BaseColor.WHITE;
                Font verdanaRightHeader1 = FontFactory.GetFont("Arial", 12, Font.BOLD);
                verdanaRightHeader1.Color = BaseColor.WHITE;
                Font arialOldLicenceDetails = FontFactory.GetFont("Arial", 10, Font.BOLD);
                arialOldLicenceDetails.Color = BaseColor.WHITE;
                Font monotypeHeader = new Font(customfont, 20);
                Font monotype = FontFactory.GetFont("Arial", 13, Font.BOLD);
                Font impact = new Font(customfontimpact, 12);
                Font impactHeader = new Font(customfontimpact, 11, Font.BOLD);
                impactHeader.Color = BaseColor.WHITE;
                BaseFont bfTimes = BaseFont.CreateFont(BaseFont.TIMES_ROMAN, BaseFont.CP1252, false);
                Font timesRoman = FontFactory.GetFont("Arial", 10, BaseColor.BLACK);
                Font ArialFont = FontFactory.GetFont("Arial", 8, BaseColor.WHITE);
                Font SealFontArial = FontFactory.GetFont("Arial", 9, BaseColor.BLACK);
                Font verdanaBold = FontFactory.GetFont("Arial", 10, Font.BOLD);
                Font verdanalic = FontFactory.GetFont("Arial", 9, Font.BOLD);
                verdanalic.Color = BaseColor.WHITE;
                var principalEmployerPerforma = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "LicensePerformas")).Root + "CertificatePerforma_Principal_Employer.jpg";
                iTextSharp.text.Image pageLogo1 = iTextSharp.text.Image.GetInstance(new FileStream(principalEmployerPerforma, FileMode.OpenOrCreate, FileAccess.Read));

                pageLogo1.Alignment = iTextSharp.text.Image.ALIGN_CENTER | iTextSharp.text.Image.UNDERLYING;
                pageLogo1.ScaleToFit(800f, 800f);
                pageLogo1.SetAbsolutePosition(10f, 25f);
                pageLogo1.BorderWidth = 1;
                documents.Add(pageLogo1);
                PdfPTable ptableleftSideDetail = new PdfPTable(1);
                ptableleftSideDetail.DefaultCell.Border = 0;
                ptableleftSideDetail.DefaultCell.Padding = 0;
                ptableleftSideDetail.HorizontalAlignment = (Element.ALIGN_JUSTIFIED);
                ptableleftSideDetail.DefaultCell.VerticalAlignment = (Element.ALIGN_JUSTIFIED);
                ptableleftSideDetail.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
                ptableleftSideDetail.AddCell(new PdfPCell(new Phrase("", verdanaBold)) { Border = 0, PaddingTop = 10 });


                ptableleftSideDetail.AddCell(new PdfPCell(new Phrase("Licence No. : " + licenceNo.ToString(), verdanalic)) { Border = 0, PaddingTop = 300, PaddingLeft = -18 }); ;
                ptableleftSideDetail.AddCell(new PdfPCell(new Phrase("Licence Issued On : " + String.Format("{0:dd-MMM-yyyy}", DateTime.Now.ToString("dd/MM/yyyy")), verdanalic)) { Border = 0, PaddingTop = 10, PaddingLeft = -18 });
                ptableleftSideDetail.AddCell(new PdfPCell(new Phrase("Valid Upto : " + String.Format("{0:dd-MMM-yyyy}", ValidUpto), verdanalic)) { Border = 0, PaddingTop = 10, PaddingLeft = -18 });
                Font verdanaBoldBP = new Font(BaseFont.CreateFont(BaseFont.TIMES_ROMAN, BaseFont.CP1252, false), 8, Font.BOLD | Font.UNDERLINE, BaseColor.WHITE);
                Phrase phrase = new Phrase();

                ptableleftSideDetail.AddCell(new PdfPCell(phrase) { Border = 0, PaddingTop = 10, PaddingRight = 20 });

                #region QR Code Generate
                var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(contractlabourLicence.Application.PublicAppRefNum);
                //string base64String = System.Convert.ToBase64String(plainTextBytes);
                var qrCodeDetails = "Establishment Name : " + projectSite.EstablishmentName + "|" + "Establishment Address : " + projectSite.Address + "|" + "IPIN : " + contractlabourLicence.Application.InvestPunjab_Ipin + "|" + "Contact No : " + projectSite.ContactPersonFirstName + " " + projectSite.ContactPersonLastName + "|" + projectSite.ContactPersonMobileNo + "|" + "Licence No : " + licenceNo + "|" + "File No : " + contractlabourLicence.Application.PublicAppRefNum;
                QRCodeGenerator qrGenerator = new QRCodeGenerator();
                QRCodeGenerator.QRCode qrCode = qrGenerator.CreateQrCode(qrCodeDetails, QRCodeGenerator.ECCLevel.Q);

                using (System.Drawing.Bitmap bitMap = qrCode.GetGraphic(20))
                {
                    using (MemoryStream ms = new MemoryStream())
                    {
                        bitMap.Save(ms, ImageFormat.Jpeg);
                        System.Drawing.Image imgQRCode = System.Drawing.Image.FromStream(ms);
                        iTextSharp.text.Image imdf = iTextSharp.text.Image.GetInstance(imgQRCode, System.Drawing.Imaging.ImageFormat.Jpeg);
                        imdf.ScaleAbsoluteHeight(120f);
                        imdf.ScaleAbsoluteWidth(120f);
                        ptableleftSideDetail.AddCell(new PdfPCell(imdf) { Border = 0, PaddingTop = 30, PaddingLeft = 0, PaddingRight = 0 });
                    }
                }
                #endregion

                ptableleftSideDetail.AddCell(new PdfPCell(new Phrase("" + Chunk.NEWLINE + "Department of Labour, Govt. of Punjab" + Chunk.NEWLINE, verdanaRightHeader1)) { Border = 0, PaddingTop = 15, PaddingRight = 25, HorizontalAlignment = PdfPCell.ALIGN_CENTER });

                PdfPTable ptablerightSideDetail = new PdfPTable(1);
                ptablerightSideDetail.DefaultCell.Border = 0;
                ptablerightSideDetail.DefaultCell.Padding = 0;

                ptablerightSideDetail.AddCell(new PdfPCell(new Phrase("FORM-VIII", verdanaHeader)) { Border = 0, PaddingLeft = 20, PaddingTop = 10, HorizontalAlignment = PdfPCell.ALIGN_CENTER });
                ptablerightSideDetail.AddCell(new PdfPCell(new Phrase("[See Rule 11 (1)]", verdanaHeader)) { Border = 0, PaddingLeft = 20, HorizontalAlignment = PdfPCell.ALIGN_CENTER });
                ptablerightSideDetail.AddCell(new PdfPCell(new Phrase("LICENSING OF CONTRACTOR UNDER THE INTER STATE MIGRANT WORKMEN ACT, 1979 ", impactHeader)) { Border = 0, PaddingTop = 17, PaddingBottom = 20, PaddingLeft = 40, PaddingRight = 30, VerticalAlignment = PdfPCell.ALIGN_MIDDLE, HorizontalAlignment = PdfPCell.ALIGN_CENTER });
                ptablerightSideDetail.AddCell(new PdfPCell(new Phrase("CERTIFICATE OF REGISTRATION", monotype)) { Border = 0, PaddingBottom = 4, PaddingTop = 14, HorizontalAlignment = PdfPCell.ALIGN_CENTER });
                ptablerightSideDetail.AddCell(new PdfPCell(new Phrase("GOVERNMENT OF PUNJAB", monotype)) { Border = 0, PaddingBottom = 4, HorizontalAlignment = PdfPCell.ALIGN_CENTER });
                ptablerightSideDetail.AddCell(new PdfPCell(new Phrase("", verdanaBold)) { Border = 0, Padding = 1, PaddingTop = 10 });
                ptablerightSideDetail.AddCell(new PdfPCell(new Phrase(" Dated: " + DateTime.Now.ToString("dd/MM/yyyy") + "                                Licence No. : " + licenceNo.ToString(), verdanaHeader)) { Border = 0 });

                Paragraph para1 = new Paragraph();
                para1.Add(new Chunk(Chunk.NEWLINE + "", verdanaBoldItalic));
                para1.Add(new Chunk(Chunk.NEWLINE + "1. Licence is hereby granted to ", timesRoman));
                para1.Add(new Chunk(contractlabourLicence.ContractorName, verdanaBold).SetUnderline(0.1f, -2f));
                para1.Add(new Chunk(Chunk.NEWLINE + "work under ", timesRoman));
                para1.Add(new Chunk(projectSite.EstablishmentName, verdanaBold).SetUnderline(0.1f, -2f));
                para1.Add(new Chunk(Chunk.NEWLINE + "section 8(1) of the Inter-State Migrant Workmen (Regulation of Employment and Conditions of Service) Act, 1979, subject to the conditions specified in the Annexure). ", timesRoman));
                para1.Add(new Chunk("", timesRoman));
                para1.Add(new Chunk("", timesRoman));
                para1.Add(new Chunk(Chunk.NEWLINE + "2. This licence is for doing the work of ", timesRoman));
                para1.Add(new Chunk(contractlabourLicence.NatureOfWork, verdanaBold).SetUnderline(0.1f, -2f));
                para1.Add(new Chunk("(nature of work to be indicated) in the establishment of ", timesRoman));
                para1.Add(new Chunk(contractlabourLicence.PrincipalEmployerName, verdanaBold).SetUnderline(0.1f, -2f));
                para1.Add(new Chunk(" (name of principal employer to be indicated) of ", timesRoman));
                para1.Add(new Chunk(projectSite.Address, verdanaBold).SetUnderline(0.1f, -2f));
                para1.Add(new Chunk("(place of work to be indicated).", timesRoman));

                para1.Add(new Chunk("", timesRoman));
                para1.Add(new Chunk(Chunk.NEWLINE + "3. The Licence shall remain in force till ", timesRoman));
                para1.Add(new Chunk(ValidUpto, verdanaBold).SetUnderline(0.1f, -2f));
                para1.Add(new Chunk(Chunk.NEWLINE + "4. The Licence valid for number of workers upto ", timesRoman));
                para1.Add(new Chunk(contractlabourLicence.MaximumNumberOfEmployee.ToString(), timesRoman));
                PdfPCell shipment1 = new PdfPCell(para1);
                shipment1.SetLeading(1f, 2f);
                ptablerightSideDetail.AddCell(new PdfPCell(shipment1) { Border = 0, HorizontalAlignment = PdfPCell.ALIGN_JUSTIFIED, VerticalAlignment = PdfPCell.ALIGN_JUSTIFIED, FixedHeight = 430, PaddingBottom = 15 });
                var userDetails = await _iAuthService.GetUserFullDetailByUserName(userName);
                string officerSigFilePath = "";
                if (!isDeemedCalling)
                {

                    if (userDetails.ListData.Count() == 0 || userDetails.ListData[0].Signature == null)
                    {
                        userDetails.ListData[0].Signature = "abc.jpg";
                    }
                    officerSigFilePath = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "OfficerSignatures")).Root + userDetails.ListData[0].Signature;
                    iTextSharp.text.Image signature = iTextSharp.text.Image.GetInstance(new FileStream(officerSigFilePath, FileMode.OpenOrCreate, FileAccess.Read));
                    signature.ScaleToFit(120f, 120f);
                    signature.SetAbsolutePosition(630f, 150f);
                    ptablerightSideDetail.AddCell(new PdfPCell(signature) { Border = 0, PaddingTop = 5, HorizontalAlignment = PdfPCell.ALIGN_RIGHT });
                }

                Paragraph SignSeal = new Paragraph();
                SignSeal.Add(new Chunk("Name: ", verdanaHeader));
                SignSeal.Add(new Chunk(userDetails.ListData[0].UserFullName, SealFontArial));
                SignSeal.Add(new Chunk(Chunk.NEWLINE + "Circle Name: ", verdanaHeader));
                SignSeal.Add(new Chunk(alcCircle.ALCCircleName, SealFontArial));
                SignSeal.Add(new Chunk(Chunk.NEWLINE + "District Name: ", verdanaHeader));
                SignSeal.Add(new Chunk(alcCircle.ALCCircleName, SealFontArial));
                SignSeal.Add(new Chunk(Chunk.NEWLINE + "Date: ", verdanaHeader));
                SignSeal.Add(new Chunk(DateTime.Now.ToShortDateString(), SealFontArial));
                SignSeal.Add(new Chunk(Chunk.NEWLINE + "Time: ", verdanaHeader));
                SignSeal.Add(new Chunk(DateTime.Now.ToString("dd/MM/yyyy"), SealFontArial));
                ptablerightSideDetail.AddCell(new PdfPCell(SignSeal) { Border = 0, HorizontalAlignment = PdfPCell.ALIGN_LEFT, PaddingLeft = 200 });
                //ptablerightSideDetail.AddCell(new PdfPCell(new Phrase(" Dated " + DateTime.Now.ToShortDateString(), verdanaHeader)) { Border = 0, HorizontalAlignment = PdfPCell.ALIGN_LEFT});

                ptablerightSideDetail.AddCell(new PdfPCell(new Phrase("This is a computer generated document to check & verify please login to https://pblabour.gov.in/Account/ApplicationStatus", footer)) { Border = 1, BorderWidth = 1, Padding = 5 });
                PdfPTable ptablePrincipalDetailLicenceApplied = new PdfPTable(3);
                ptablePrincipalDetailLicenceApplied.DefaultCell.Border = 0;
                ptablePrincipalDetailLicenceApplied.DefaultCell.Padding = 2;
                ptablePrincipalDetailLicenceApplied.SpacingBefore = 1000;
                ptablePrincipalDetailLicenceApplied.AddCell(new PdfPCell(ptableleftSideDetail) { Border = 0 });
                ptablePrincipalDetailLicenceApplied.AddCell(new PdfPCell(ptablerightSideDetail) { Border = 0, Colspan = 2 });
                documents.Add(ptablePrincipalDetailLicenceApplied);
                documents.NewPage();
                PdfPTable ptableLabourAnnexure = new PdfPTable(7);
                ptableLabourAnnexure.DefaultCell.Border = 0;
                ptableLabourAnnexure.DefaultCell.Padding = 2;
                int sno = 1;
                ptableLabourAnnexure.AddCell(new PdfPCell(new Phrase("ANNEXURE", verdanaHeader)) { Padding = 20, PaddingLeft = 220, Colspan = 7, Border = 0, Left = 100 });
                ptableLabourAnnexure.AddCell(new PdfPCell(new Phrase("The licence is subject to the following conditions :-", verdana)) { Padding = 20, Colspan = 7, Border = 0, Left = 40 });
                ptableLabourAnnexure.AddCell(new PdfPCell(new Phrase((sno++).ToString(), verdana)) { Border = 1, BorderWidthTop = 1, BorderWidthBottom = 1, BorderWidthLeft = 1, BorderWidthRight = 1 });
                ptableLabourAnnexure.AddCell(new PdfPCell(new Phrase("The licence shall be non-transferable.", verdana)) { Border = 1, BorderWidthTop = 1, BorderWidthBottom = 1, BorderWidthRight = 1, Colspan = 6, Padding = 6 });
                ptableLabourAnnexure.AddCell(new PdfPCell(new Phrase((sno++).ToString(), verdana)) { Border = 1, BorderWidthTop = 1, BorderWidthBottom = 1, BorderWidthLeft = 1, BorderWidthRight = 1 });
                ptableLabourAnnexure.AddCell(new PdfPCell(new Phrase("The number of workmen employed as contract labour in the establishment shall not, on any day exceed " + contractlabourLicence.MaximumNumberOfEmployee, verdana)) { Border = 1, BorderWidthTop = 1, BorderWidthBottom = 1, BorderWidthRight = 1, Colspan = 6, Padding = 6 });
                ptableLabourAnnexure.AddCell(new PdfPCell(new Phrase((sno++).ToString(), verdana)) { Border = 1, BorderWidthTop = 0, BorderWidthBottom = 1, BorderWidthLeft = 1, BorderWidthRight = 1 });
                ptableLabourAnnexure.AddCell(new PdfPCell(new Phrase("Except as provided in the rules the fees paid for the grant or as the case may be, for renewal of the licence shall be non-refundable .", verdana)) { Border = 1, BorderWidthTop = 0, BorderWidthBottom = 1, BorderWidthRight = 1, Colspan = 6, Padding = 6 });
                ptableLabourAnnexure.AddCell(new PdfPCell(new Phrase((sno++).ToString(), verdana)) { Border = 1, BorderWidthTop = 0, BorderWidthBottom = 1, BorderWidthLeft = 1, BorderWidthRight = 1 });
                ptableLabourAnnexure.AddCell(new PdfPCell(new Phrase("The rates of wages payble to the workmen by the contractor shall not be less than the rates prescribed for the Schedule of employment under the Minimum Wages Act, 1948, where applicable, and where the rates have been fixed by agreement, settlement or award, not less than the  rates fixed.", verdana)) { Border = 1, BorderWidthTop = 0, BorderWidthBottom = 1, BorderWidthRight = 1, Colspan = 6, Padding = 6 });
                ptableLabourAnnexure.AddCell(new PdfPCell(new Phrase((sno++).ToString(), verdana)) { Border = 1, BorderWidthTop = 0, BorderWidthBottom = 1, BorderWidthLeft = 1, BorderWidthRight = 1 });
                ptableLabourAnnexure.AddCell(new PdfPCell(new Phrase("In case where the workmen employed by the contractor perform the same or similar kind of work as the workmen directly employed by the principal employer of the establishment, the wages rates, holidays, hours of work and other conditions of service of the workmen of the contractor shall be the same as applicable to the workmen directly employed by the principal employer of the establishment on the same or similar kind of work: Provided that in the case of any disagreement with regard to the type of work the same shall be decided by the [Deputy Chief Labour Commissioner (Central)] whose decision shall be final.", verdana)) { Border = 1, BorderWidthTop = 0, BorderWidthBottom = 1, BorderWidthRight = 1, Colspan = 6, Padding = 6 });
                ptableLabourAnnexure.AddCell(new PdfPCell(new Phrase("", verdana)) { Border = 1, BorderWidthTop = 0, BorderWidthBottom = 1, BorderWidthLeft = 1, BorderWidthRight = 1 });
                ptableLabourAnnexure.AddCell(new PdfPCell(new Phrase("  Provided that in the case of any disagreement with regard to the type of work the same shall be decided by the [Deputy Chief Labour Commissioner (Central)] whose decision shall be final.", verdana)) { Border = 1, BorderWidthTop = 0, BorderWidthBottom = 1, BorderWidthRight = 1, Colspan = 6, Padding = 6 });
                ptableLabourAnnexure.AddCell(new PdfPCell(new Phrase((sno++).ToString(), verdana)) { Border = 1, BorderWidthTop = 0, BorderWidthBottom = 1, BorderWidthLeft = 1, BorderWidthRight = 1 });
                ptableLabourAnnexure.AddCell(new PdfPCell(new Phrase("In other case the wages rates, holidays, hours of work and condistions of service of the workmen of the contractor shall be such as may be specified in this  behalf by the [Deputy Chief Labour Commissioner (Central)].", verdana)) { Border = 1, BorderWidthTop = 0, BorderWidthBottom = 1, BorderWidthRight = 1, Colspan = 6, Padding = 6 });
                ptableLabourAnnexure.AddCell(new PdfPCell(new Phrase((sno++).ToString(), verdana)) { Border = 1, BorderWidthTop = 0, BorderWidthBottom = 1, BorderWidthLeft = 1, BorderWidthRight = 1 });
                ptableLabourAnnexure.AddCell(new PdfPCell(new Phrase("In every establishment where 20  or more women are ordinarily employed as contract labour there shall be provided 2 rooms of reasonable dimension for the use of their children under the age of six years. One of such rooms would be used as a play room for the children and the other as bed room for the children. For this purpose the contractor shall supply adequate number of toys and games in the play room and sufficient number of cots and beddings in the sleeping rooms. The standard of construction and maintenance of the creches may be such as may be specified in this behalf by the [Deputy Chief Labour Commissioner (Central)].", verdana)) { Border = 1, BorderWidthTop = 0, BorderWidthBottom = 1, BorderWidthRight = 1, Colspan = 6, Padding = 6 });
                ptableLabourAnnexure.AddCell(new PdfPCell(new Phrase((sno++).ToString(), verdana)) { Border = 1, BorderWidthTop = 0, BorderWidthBottom = 1, BorderWidthLeft = 1, BorderWidthRight = 1 });
                ptableLabourAnnexure.AddCell(new PdfPCell(new Phrase("The license shall notify any change in the number of workmen or the condistions of work to the Licensing Officer.", verdana)) { Border = 1, BorderWidthTop = 0, BorderWidthBottom = 1, BorderWidthRight = 1, Colspan = 6, Padding = 6 });
                ptableLabourAnnexure.AddCell(new PdfPCell(new Phrase((sno++).ToString(), verdana)) { Border = 1, BorderWidthTop = 0, BorderWidthBottom = 1, BorderWidthLeft = 1, BorderWidthRight = 1 });
                ptableLabourAnnexure.AddCell(new PdfPCell(new Phrase("[A copy of the licence shall be displayed prominently at the premises where the contract work is being carried on ]", verdana)) { Border = 1, BorderWidthTop = 0, BorderWidthBottom = 1, BorderWidthRight = 1, Colspan = 6, Padding = 6 });
                documents.Add(ptableLabourAnnexure);
                documents.Close();
                PDFPageEvents pageeventsobj = new PDFPageEvents();



            }
            catch (Exception ex)
            {
                genericFormModel.HasException = true;
                genericFormModel.Exceptions = ex.Message;
            }
            return genericFormModel;
        }

        private async Task<CertificateGenerateServiceResultTemplate> All_Act_Certificate_AppForm_5(Int64 appRefId, string CertificatePerformaURL, ApplicationTypeEnum applicationType, string userName, string licenceNo, bool isDeemedCalling)
        {

            CertificateGenerateServiceResultTemplate genericFormModel = new CertificateGenerateServiceResultTemplate();
            try
            {

                Guid guId = Guid.NewGuid();
                genericFormModel.PdfNameGUID = guId.ToString();
                var sfilepath = "";
                sfilepath = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "TempFiles")).Root + guId.ToString() + ".pdf";
                FileInfo file = new FileInfo(sfilepath);
                if (file.Exists)
                {
                    file.Delete();
                }
                iTextSharp.text.Document documents = new iTextSharp.text.Document(iTextSharp.text.PageSize.A4, 5, 5, 5, 5);
                float a4Width = iTextSharp.text.PageSize.A4.Width;
                float a4Height = iTextSharp.text.PageSize.A4.Height;
                documents.Open();
                #region ClassObjects
                pdfFonts classFonts = new pdfFonts();
                #endregion

                #region CreateObjects
                PdfWriter owriter = PdfWriter.GetInstance(documents, new FileStream(sfilepath, FileMode.Create));
                PDFPageEvents pageEvent = new PDFPageEvents();
                owriter.PageEvent = pageEvent;
                owriter.SetEncryption(PdfWriter.STRENGTH128BITS, "", "", PdfWriter.AllowPrinting);
                #endregion

                var fontName = "Verdana";
                if (!FontFactory.IsRegistered(fontName))
                {
                    var fontPath = "Content/fonts/VERDANA.TTF";
                    FontFactory.Register(fontPath);
                }

                #region Tables
                var application = _context.Applications.Where(x => x.AppId == appRefId).Include(x => x.ProjectSites).FirstOrDefault();
                var projectsites = _context.ProjectSites.Where(x => x.ProjectSiteId == application.ProjectSiteRefId).FirstOrDefault();
                var district = _context.Districts.Where(x => x.DistrictLgdId == projectsites.DistrictRefId).FirstOrDefault();
                #endregion

                string fontpath1 = "Content/fonts/VERDANA.TTF";
                BaseFont customfont = BaseFont.CreateFont(fontpath1, BaseFont.CP1252, BaseFont.EMBEDDED);
                string fontpath2 = "Content/fonts/VERDANA.TTF";
                BaseFont customfontimpact = BaseFont.CreateFont(fontpath2, BaseFont.CP1252, BaseFont.EMBEDDED);
                documents.Open();
                Font verdanaHeader = FontFactory.GetFont("Arial", 11, Font.BOLD);
                Font footer = FontFactory.GetFont("Arial", 9, Font.BOLD, BaseColor.BLUE);
                Font verdana = FontFactory.GetFont("Arial", 8, BaseColor.BLACK);
                Font verdanaBoldItalic = FontFactory.GetFont("Arial", 8, Font.BOLDITALIC);
                Font verdanasmall = FontFactory.GetFont("Arial", 8, Font.NORMAL);
                verdanasmall.Color = BaseColor.WHITE;
                Font verdanaRightHeader = FontFactory.GetFont("Arial", 15, Font.BOLD);
                verdanaRightHeader.Color = BaseColor.WHITE;
                Font verdanaRightHeader1 = FontFactory.GetFont("Arial", 12, Font.BOLD);
                verdanaRightHeader1.Color = BaseColor.WHITE;
                Font arialOldLicenceDetails = FontFactory.GetFont("Arial", 10, Font.BOLD);
                arialOldLicenceDetails.Color = BaseColor.WHITE;
                Font monotypeHeader = new Font(customfont, 20);
                Font monotype = FontFactory.GetFont("Arial", 13, Font.BOLD);
                Font impact = new Font(customfontimpact, 12);
                Font impactHeader = new Font(customfontimpact, 11, Font.BOLD);
                impactHeader.Color = BaseColor.WHITE;
                BaseFont bfTimes = BaseFont.CreateFont(BaseFont.TIMES_ROMAN, BaseFont.CP1252, false);
                Font timesRoman = FontFactory.GetFont("Arial", 10, BaseColor.BLACK);
                Font ArialFont = FontFactory.GetFont("Arial", 8, BaseColor.WHITE);
                Font SealFontArial = FontFactory.GetFont("Arial", 9, BaseColor.BLACK);
                Font verdanaBold = FontFactory.GetFont("Arial", 10, Font.BOLD);
                Font verdanalic = FontFactory.GetFont("Arial", 9, Font.BOLD);
                verdanalic.Color = BaseColor.WHITE;
                var noticePerforma = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "LicensePerformas")).Root + "Deemed_HUD_1.jpg";
                iTextSharp.text.Image pageLogo1 = iTextSharp.text.Image.GetInstance(new FileStream(noticePerforma, FileMode.OpenOrCreate, FileAccess.Read));
                pageLogo1.Alignment = iTextSharp.text.Image.ALIGN_CENTER | iTextSharp.text.Image.UNDERLYING;
                pageLogo1.ScaleToFit(a4Width, a4Height);
                documents.Add(pageLogo1);


                var attachment1 = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "LicensePerformas")).Root + "Deemed_HUD_2.jpg";
                iTextSharp.text.Image page1 = iTextSharp.text.Image.GetInstance(new FileStream(attachment1, FileMode.OpenOrCreate, FileAccess.Read));
                page1.Alignment = iTextSharp.text.Image.ALIGN_CENTER | iTextSharp.text.Image.UNDERLYING;
                page1.ScaleToFit(800f, 800f);
                page1.SetAbsolutePosition(10f, 25f);
                page1.BorderWidth = 1;


                var attachment2 = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "LicensePerformas")).Root + "Deemed_HUD_3.jpg";
                iTextSharp.text.Image page2 = iTextSharp.text.Image.GetInstance(new FileStream(attachment2, FileMode.OpenOrCreate, FileAccess.Read));
                page2.Alignment = iTextSharp.text.Image.ALIGN_CENTER | iTextSharp.text.Image.UNDERLYING;
                page2.ScaleToFit(800f, 800f);
                page2.SetAbsolutePosition(10f, 25f);
                page2.BorderWidth = 1;



                var attachment3 = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "LicensePerformas")).Root + "Deemed_HUD_4.jpg";
                iTextSharp.text.Image page3 = iTextSharp.text.Image.GetInstance(new FileStream(attachment3, FileMode.OpenOrCreate, FileAccess.Read));
                page3.Alignment = iTextSharp.text.Image.ALIGN_CENTER | iTextSharp.text.Image.UNDERLYING;
                page3.ScaleToFit(800f, 800f);
                page3.SetAbsolutePosition(10f, 25f);
                page3.BorderWidth = 1;


                PdfPTable ptableleftSideDetail = new PdfPTable(1);


                PdfPTable ptablerightSideDetail = new PdfPTable(1);
                ptablerightSideDetail.DefaultCell.Border = 0;
                ptablerightSideDetail.DefaultCell.Padding = 0;
                ptablerightSideDetail.AddCell(new PdfPCell(new Phrase(application.PublicAppRefNum + "                                                                             " + DateTime.Now.ToString("dd-MM-yyyy"), verdanaHeader)) { Border = 0, PaddingLeft = -160, PaddingTop = 156 });
                ptablerightSideDetail.AddCell(new PdfPCell(new Phrase(projectsites.EstablishmentName, verdanaHeader)) { Border = 0, PaddingLeft = -115, PaddingTop = 13 }); ;
                ptablerightSideDetail.AddCell(new PdfPCell(new Phrase(projectsites.Address, verdanaHeader)) { Border = 0, PaddingLeft = -130, PaddingTop = 13 });


                ptablerightSideDetail.AddCell(new PdfPCell(new Phrase(application.CreatedOnDate.ToString("dd-MM-yyyy"), verdanaHeader)) { Border = 0, PaddingLeft = -160, PaddingTop = 125 });


                ptablerightSideDetail.AddCell(new PdfPCell(new Phrase(application.InvestPunjab_Ipin, verdanaHeader)) { Border = 0, PaddingLeft = -85, PaddingTop = 128 });
                ptablerightSideDetail.AddCell(new PdfPCell(new Phrase(application.InvestPunjab_AppId.ToString(), verdanaHeader)) { Border = 0, PaddingLeft = -85, PaddingTop = 15 });
                ptablerightSideDetail.AddCell(new PdfPCell(new Phrase(district.DistrictName, verdanaHeader)) { Border = 0, PaddingLeft = -85, PaddingTop = 15 });
                ptablerightSideDetail.AddCell(new PdfPCell(new Phrase(projectsites.ProjectPurpose, verdanaHeader)) { Border = 0, PaddingLeft = -85, PaddingTop = 15 });
                PdfPTable ptableShopDetailLicenceApplied = new PdfPTable(3);
                ptableShopDetailLicenceApplied.DefaultCell.Border = 0;
                ptableShopDetailLicenceApplied.DefaultCell.Padding = 2;
                ptableShopDetailLicenceApplied.SpacingBefore = 1000;
                ptableShopDetailLicenceApplied.AddCell(new PdfPCell(ptableleftSideDetail) { Border = 0 });
                ptableShopDetailLicenceApplied.AddCell(new PdfPCell(ptablerightSideDetail) { Border = 0, Colspan = 2 });
                documents.Add(ptableShopDetailLicenceApplied);
                documents.NewPage();
                documents.Add(page1);
                documents.NewPage();
                documents.Add(page2);
                documents.NewPage();
                documents.Add(page3);
                documents.Close();
                PDFPageEvents pageeventsobj = new PDFPageEvents();


            }
            catch (Exception ex)
            {
                genericFormModel.HasException = true;
                genericFormModel.Exceptions = ex.Message;
            }
            return genericFormModel;
        }

        private async Task<CertificateGenerateServiceResultTemplate> All_Act_Certificate_AppForm_71_72_73(Int64 appRefId, string CertificatePerformaURL, ApplicationTypeEnum applicationType, string userName, string licenceNo, bool isDeemedCalling)
        {

            CertificateGenerateServiceResultTemplate genericFormModel = new CertificateGenerateServiceResultTemplate();
            try
            {

                Guid guId = Guid.NewGuid();
                genericFormModel.PdfNameGUID = guId.ToString();
                var sfilepath = "";
                sfilepath = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "TempFiles")).Root + guId.ToString() + ".pdf";
                FileInfo file = new FileInfo(sfilepath);
                if (file.Exists)
                {
                    file.Delete();
                }
                iTextSharp.text.Document documents = new iTextSharp.text.Document(iTextSharp.text.PageSize.A4, 5, 5, 5, 5);
                float a4Width = iTextSharp.text.PageSize.A4.Width;
                float a4Height = iTextSharp.text.PageSize.A4.Height;
                documents.Open();
                #region ClassObjects
                pdfFonts classFonts = new pdfFonts();
                #endregion

                #region CreateObjects
                PdfWriter owriter = PdfWriter.GetInstance(documents, new FileStream(sfilepath, FileMode.Create));
                PDFPageEvents pageEvent = new PDFPageEvents();
                owriter.PageEvent = pageEvent;
                owriter.SetEncryption(PdfWriter.STRENGTH128BITS, "", "", PdfWriter.AllowPrinting);
                #endregion

                var fontName = "Verdana";
                if (!FontFactory.IsRegistered(fontName))
                {
                    var fontPath = "Content/fonts/VERDANA.TTF";
                    FontFactory.Register(fontPath);
                }

                #region Tables
                var application = _context.Applications.Where(x => x.AppId == appRefId).Include(x => x.ProjectSites).FirstOrDefault();
                var projectsites = _context.ProjectSites.Where(x => x.ProjectSiteId == application.ProjectSiteRefId).FirstOrDefault();
                var district = _context.Districts.Where(x => x.DistrictLgdId == projectsites.DistrictRefId).FirstOrDefault();
                #endregion

                string fontpath1 = "Content/fonts/VERDANA.TTF";
                BaseFont customfont = BaseFont.CreateFont(fontpath1, BaseFont.CP1252, BaseFont.EMBEDDED);
                string fontpath2 = "Content/fonts/VERDANA.TTF";
                BaseFont customfontimpact = BaseFont.CreateFont(fontpath2, BaseFont.CP1252, BaseFont.EMBEDDED);
                documents.Open();
                Font verdanaHeader = FontFactory.GetFont("Arial", 11, Font.BOLD);
                Font footer = FontFactory.GetFont("Arial", 9, Font.BOLD, BaseColor.BLUE);
                Font verdana = FontFactory.GetFont("Arial", 8, BaseColor.BLACK);
                Font verdanaBoldItalic = FontFactory.GetFont("Arial", 8, Font.BOLDITALIC);
                Font verdanasmall = FontFactory.GetFont("Arial", 8, Font.NORMAL);
                verdanasmall.Color = BaseColor.WHITE;
                Font verdanaRightHeader = FontFactory.GetFont("Arial", 15, Font.BOLD);
                verdanaRightHeader.Color = BaseColor.WHITE;
                Font verdanaRightHeader1 = FontFactory.GetFont("Arial", 12, Font.BOLD);
                verdanaRightHeader1.Color = BaseColor.WHITE;
                Font arialOldLicenceDetails = FontFactory.GetFont("Arial", 10, Font.BOLD);
                arialOldLicenceDetails.Color = BaseColor.WHITE;
                Font monotypeHeader = new Font(customfont, 20);
                Font monotype = FontFactory.GetFont("Arial", 13, Font.BOLD);
                Font impact = new Font(customfontimpact, 12);
                Font impactHeader = new Font(customfontimpact, 11, Font.BOLD);
                impactHeader.Color = BaseColor.WHITE;
                BaseFont bfTimes = BaseFont.CreateFont(BaseFont.TIMES_ROMAN, BaseFont.CP1252, false);
                Font timesRoman = FontFactory.GetFont("Arial", 10, BaseColor.BLACK);
                Font ArialFont = FontFactory.GetFont("Arial", 8, BaseColor.WHITE);
                Font SealFontArial = FontFactory.GetFont("Arial", 9, BaseColor.BLACK);
                Font verdanaBold = FontFactory.GetFont("Arial", 10, Font.BOLD);
                Font verdanalic = FontFactory.GetFont("Arial", 9, Font.BOLD);
                verdanalic.Color = BaseColor.WHITE;
                var noticePerforma = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "LicensePerformas")).Root + "Deemed_71_1.jpg";
                iTextSharp.text.Image pageLogo1 = iTextSharp.text.Image.GetInstance(new FileStream(noticePerforma, FileMode.OpenOrCreate, FileAccess.Read));
                pageLogo1.Alignment = iTextSharp.text.Image.ALIGN_CENTER | iTextSharp.text.Image.UNDERLYING;
                pageLogo1.ScaleToFit(a4Width, a4Height);
                documents.Add(pageLogo1);


                var attachment1 = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "LicensePerformas")).Root + "Deemed_71_2.jpg";
                iTextSharp.text.Image page1 = iTextSharp.text.Image.GetInstance(new FileStream(attachment1, FileMode.OpenOrCreate, FileAccess.Read));
                page1.Alignment = iTextSharp.text.Image.ALIGN_CENTER | iTextSharp.text.Image.UNDERLYING;
                page1.ScaleToFit(800f, 800f);
                page1.SetAbsolutePosition(10f, 25f);
                page1.BorderWidth = 1;


                PdfPTable ptableleftSideDetail = new PdfPTable(1);


                PdfPTable ptablerightSideDetail = new PdfPTable(1);
                ptablerightSideDetail.DefaultCell.Border = 0;
                ptablerightSideDetail.DefaultCell.Padding = 0;
                ptablerightSideDetail.AddCell(new PdfPCell(new Phrase(application.PublicAppRefNum + "                                                                           " + DateTime.Now.ToString("dd-MM-yyyy"), verdanaHeader)) { Border = 0, PaddingLeft = -160, PaddingTop = 156 });
                ptablerightSideDetail.AddCell(new PdfPCell(new Phrase(projectsites.EstablishmentName, verdanaHeader)) { Border = 0, PaddingLeft = -120, PaddingTop = 13 }); ;
                ptablerightSideDetail.AddCell(new PdfPCell(new Phrase(projectsites.Address, verdanaHeader)) { Border = 0, PaddingLeft = -130, PaddingTop = 13 });


                ptablerightSideDetail.AddCell(new PdfPCell(new Phrase(application.CreatedOnDate.ToString("dd-MM-yyyy"), verdanaHeader)) { Border = 0, PaddingLeft = -160, PaddingTop = 105 });



                ptablerightSideDetail.AddCell(new PdfPCell(new Phrase(application.InvestPunjab_Ipin, verdanaHeader)) { Border = 0, PaddingLeft = -85, PaddingTop = 98 });
                ptablerightSideDetail.AddCell(new PdfPCell(new Phrase(application.InvestPunjab_AppId.ToString(), verdanaHeader)) { Border = 0, PaddingLeft = -85, PaddingTop = 15 });
                ptablerightSideDetail.AddCell(new PdfPCell(new Phrase(district.DistrictName, verdanaHeader)) { Border = 0, PaddingLeft = -85, PaddingTop = 15 });
                ptablerightSideDetail.AddCell(new PdfPCell(new Phrase(projectsites.ProjectPurpose, verdanaHeader)) { Border = 0, PaddingLeft = -85, PaddingTop = 15 });
                PdfPTable ptableShopDetailLicenceApplied = new PdfPTable(3);
                ptableShopDetailLicenceApplied.DefaultCell.Border = 0;
                ptableShopDetailLicenceApplied.DefaultCell.Padding = 2;
                ptableShopDetailLicenceApplied.SpacingBefore = 1000;
                ptableShopDetailLicenceApplied.AddCell(new PdfPCell(ptableleftSideDetail) { Border = 0 });
                ptableShopDetailLicenceApplied.AddCell(new PdfPCell(ptablerightSideDetail) { Border = 0, Colspan = 2 });
                documents.Add(ptableShopDetailLicenceApplied);
                documents.NewPage();
                documents.Add(page1);
                documents.NewPage();
                documents.Close();
                PDFPageEvents pageeventsobj = new PDFPageEvents();


            }
            catch (Exception ex)
            {
                genericFormModel.HasException = true;
                genericFormModel.Exceptions = ex.Message;
            }
            return genericFormModel;
        }
        private async Task<CertificateGenerateServiceResultTemplate> All_Act_Certificate_AppForm_81(Int64 appRefId, string CertificatePerformaURL, ApplicationTypeEnum applicationType, string userName, string licenceNo, bool isDeemedCalling)
        {

            CertificateGenerateServiceResultTemplate genericFormModel = new CertificateGenerateServiceResultTemplate();
            try
            {

                Guid guId = Guid.NewGuid();
                genericFormModel.PdfNameGUID = guId.ToString();
                var sfilepath = "";
                sfilepath = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "TempFiles")).Root + guId.ToString() + ".pdf";
                FileInfo file = new FileInfo(sfilepath);
                if (file.Exists)
                {
                    file.Delete();
                }
                iTextSharp.text.Document documents = new iTextSharp.text.Document(iTextSharp.text.PageSize.A4, 5, 5, 5, 5);
                float a4Width = iTextSharp.text.PageSize.A4.Width;
                float a4Height = iTextSharp.text.PageSize.A4.Height;
                documents.Open();
                #region ClassObjects
                pdfFonts classFonts = new pdfFonts();
                #endregion

                #region CreateObjects
                PdfWriter owriter = PdfWriter.GetInstance(documents, new FileStream(sfilepath, FileMode.Create));
                PDFPageEvents pageEvent = new PDFPageEvents();
                owriter.PageEvent = pageEvent;
                owriter.SetEncryption(PdfWriter.STRENGTH128BITS, "", "", PdfWriter.AllowPrinting);
                #endregion

                var fontName = "Verdana";
                if (!FontFactory.IsRegistered(fontName))
                {
                    var fontPath = "Content/fonts/VERDANA.TTF";
                    FontFactory.Register(fontPath);
                }

                #region Tables
                var application = _context.Applications.Where(x => x.AppId == appRefId).Include(x => x.ProjectSites).FirstOrDefault();
                var projectsites = _context.ProjectSites.Where(x => x.ProjectSiteId == application.ProjectSiteRefId).FirstOrDefault();
                var district = _context.Districts.Where(x => x.DistrictLgdId == projectsites.DistrictRefId).FirstOrDefault();
                #endregion

                string fontpath1 = "Content/fonts/VERDANA.TTF";
                BaseFont customfont = BaseFont.CreateFont(fontpath1, BaseFont.CP1252, BaseFont.EMBEDDED);
                string fontpath2 = "Content/fonts/VERDANA.TTF";
                BaseFont customfontimpact = BaseFont.CreateFont(fontpath2, BaseFont.CP1252, BaseFont.EMBEDDED);
                documents.Open();
                Font verdanaHeader = FontFactory.GetFont("Arial", 11, Font.BOLD);
                Font footer = FontFactory.GetFont("Arial", 9, Font.BOLD, BaseColor.BLUE);
                Font verdana = FontFactory.GetFont("Arial", 8, BaseColor.BLACK);
                Font verdanaBoldItalic = FontFactory.GetFont("Arial", 8, Font.BOLDITALIC);
                Font verdanasmall = FontFactory.GetFont("Arial", 8, Font.NORMAL);
                verdanasmall.Color = BaseColor.WHITE;
                Font verdanaRightHeader = FontFactory.GetFont("Arial", 15, Font.BOLD);
                verdanaRightHeader.Color = BaseColor.WHITE;
                Font verdanaRightHeader1 = FontFactory.GetFont("Arial", 12, Font.BOLD);
                verdanaRightHeader1.Color = BaseColor.WHITE;
                Font arialOldLicenceDetails = FontFactory.GetFont("Arial", 10, Font.BOLD);
                arialOldLicenceDetails.Color = BaseColor.WHITE;
                Font monotypeHeader = new Font(customfont, 20);
                Font monotype = FontFactory.GetFont("Arial", 13, Font.BOLD);
                Font impact = new Font(customfontimpact, 12);
                Font impactHeader = new Font(customfontimpact, 11, Font.BOLD);
                impactHeader.Color = BaseColor.WHITE;
                BaseFont bfTimes = BaseFont.CreateFont(BaseFont.TIMES_ROMAN, BaseFont.CP1252, false);
                Font timesRoman = FontFactory.GetFont("Arial", 10, BaseColor.BLACK);
                Font ArialFont = FontFactory.GetFont("Arial", 8, BaseColor.WHITE);
                Font SealFontArial = FontFactory.GetFont("Arial", 9, BaseColor.BLACK);
                Font verdanaBold = FontFactory.GetFont("Arial", 10, Font.BOLD);
                Font verdanalic = FontFactory.GetFont("Arial", 9, Font.BOLD);
                verdanalic.Color = BaseColor.WHITE;
                var noticePerforma = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "LicensePerformas")).Root + "Deemed_81_1.jpg";
                iTextSharp.text.Image pageLogo1 = iTextSharp.text.Image.GetInstance(new FileStream(noticePerforma, FileMode.OpenOrCreate, FileAccess.Read));
                pageLogo1.Alignment = iTextSharp.text.Image.ALIGN_CENTER | iTextSharp.text.Image.UNDERLYING;
                pageLogo1.ScaleToFit(a4Width, a4Height);
                documents.Add(pageLogo1);


                var attachment1 = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "LicensePerformas")).Root + "Deemed_81_2.jpg";
                iTextSharp.text.Image page1 = iTextSharp.text.Image.GetInstance(new FileStream(attachment1, FileMode.OpenOrCreate, FileAccess.Read));
                page1.Alignment = iTextSharp.text.Image.ALIGN_CENTER | iTextSharp.text.Image.UNDERLYING;
                page1.ScaleToFit(800f, 800f);
                page1.SetAbsolutePosition(10f, 25f);
                page1.BorderWidth = 1;

                var attachment2 = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "LicensePerformas")).Root + "Deemed_81_3.jpg";
                iTextSharp.text.Image page2 = iTextSharp.text.Image.GetInstance(new FileStream(attachment2, FileMode.OpenOrCreate, FileAccess.Read));
                page2.Alignment = iTextSharp.text.Image.ALIGN_CENTER | iTextSharp.text.Image.UNDERLYING;
                page2.ScaleToFit(800f, 800f);
                page2.SetAbsolutePosition(10f, 25f);
                page2.BorderWidth = 1;



                PdfPTable ptableleftSideDetail = new PdfPTable(1);


                PdfPTable ptablerightSideDetail = new PdfPTable(1);
                ptablerightSideDetail.DefaultCell.Border = 0;
                ptablerightSideDetail.DefaultCell.Padding = 0;
                ptablerightSideDetail.AddCell(new PdfPCell(new Phrase(application.PublicAppRefNum + "                                                                           " + DateTime.Now.ToString("dd-MM-yyyy"), verdanaHeader)) { Border = 0, PaddingLeft = -160, PaddingTop = 156 });
                ptablerightSideDetail.AddCell(new PdfPCell(new Phrase(projectsites.EstablishmentName, verdanaHeader)) { Border = 0, PaddingLeft = -120, PaddingTop = 13 }); ;
                ptablerightSideDetail.AddCell(new PdfPCell(new Phrase(projectsites.Address, verdanaHeader)) { Border = 0, PaddingLeft = -140, PaddingTop = 13 });


                ptablerightSideDetail.AddCell(new PdfPCell(new Phrase(application.CreatedOnDate.ToString("dd-MM-yyyy"), verdanaHeader)) { Border = 0, PaddingLeft = -160, PaddingTop = 105 });



                ptablerightSideDetail.AddCell(new PdfPCell(new Phrase(application.InvestPunjab_Ipin, verdanaHeader)) { Border = 0, PaddingLeft = -85, PaddingTop = 120 });
                ptablerightSideDetail.AddCell(new PdfPCell(new Phrase(application.InvestPunjab_AppId.ToString(), verdanaHeader)) { Border = 0, PaddingLeft = -85, PaddingTop = 15 });
                ptablerightSideDetail.AddCell(new PdfPCell(new Phrase(district.DistrictName, verdanaHeader)) { Border = 0, PaddingLeft = -85, PaddingTop = 15 });
                ptablerightSideDetail.AddCell(new PdfPCell(new Phrase(projectsites.ProjectPurpose, verdanaHeader)) { Border = 0, PaddingLeft = -85, PaddingTop = 15 });

                PdfPTable ptableShopDetailLicenceApplied = new PdfPTable(3);
                ptableShopDetailLicenceApplied.DefaultCell.Border = 0;
                ptableShopDetailLicenceApplied.DefaultCell.Padding = 2;
                ptableShopDetailLicenceApplied.SpacingBefore = 1000;
                ptableShopDetailLicenceApplied.AddCell(new PdfPCell(ptableleftSideDetail) { Border = 0 });
                ptableShopDetailLicenceApplied.AddCell(new PdfPCell(ptablerightSideDetail) { Border = 0, Colspan = 2 });
                documents.Add(ptableShopDetailLicenceApplied);
                documents.NewPage();
                documents.Add(page1);
                documents.NewPage();
                documents.Add(page2);
                documents.NewPage();
                documents.Close();
                PDFPageEvents pageeventsobj = new PDFPageEvents();


            }
            catch (Exception ex)
            {
                genericFormModel.HasException = true;
                genericFormModel.Exceptions = ex.Message;
            }
            return genericFormModel;
        }

        private async Task<CertificateGenerateServiceResultTemplate> Certificate_AppForm_81(Int64 appRefId, string CertificatePerformaURL, ApplicationTypeEnum applicationType, string userName, string licenceNo, bool isDeemedCalling)
        {
            CertificateGenerateServiceResultTemplate genericFormModel = new CertificateGenerateServiceResultTemplate();
            try
            {
                Guid guId = Guid.NewGuid();
                genericFormModel.PdfNameGUID = guId.ToString();
                genericFormModel.HasException = false;
                var SaveDirectoryPath = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "TempFiles")).Root + guId.ToString() + ".pdf";
                iTextSharp.text.Document documents = new iTextSharp.text.Document(iTextSharp.text.PageSize.A4, 5, 5, 30, 30);

                PdfWriter owriter = PdfWriter.GetInstance(documents, new FileStream(SaveDirectoryPath, FileMode.Create));
                PDFPageEvents pageEvent = new PDFPageEvents();
                owriter.PageEvent = pageEvent;
                owriter.SetEncryption(PdfWriter.STRENGTH128BITS, "", "", PdfWriter.AllowPrinting);

                string fontpath = "Content/fonts/Times-New-Roman.TTF";
                BaseFont baseFont = BaseFont.CreateFont(fontpath, BaseFont.CP1252, BaseFont.EMBEDDED);
                Font font = new Font(baseFont, 12);
                Font fontBold = new Font(baseFont, 12, Font.BOLD);
                Font smallFont = new Font(baseFont, 8);
                documents.Open();

                Paragraph paragraph1 = new Paragraph("Director of Factories, Punjab", fontBold)
                {
                    Alignment = Element.ALIGN_CENTER
                };
                documents.Add(paragraph1);

                Paragraph paragraph2 = new Paragraph("Kirat Bhawan(Model Welfare Centre) Near 10-11 Lights\nSec 64 (Phase 10),S.A.S Nagar (Mohali),Pin-160062", font)
                {
                    Alignment = Element.ALIGN_CENTER
                };
                documents.Add(paragraph2);
                documents.Add(new Paragraph("\n", font));

                var licenceDetails = _context.Applications.Where(x => x.AppId == appRefId && x.IsDeleted == false).Include(x => x.ProjectSites).FirstOrDefault();
                string dofNumber = "DOFPB" + licenceDetails.ProjectSites.ProjectSiteId.ToString().PadLeft(8, '0');
                var feeDetails = _context.Payments_RaisedFee.Where(x => x.AppRefId == appRefId).ToList();

                if (licenceDetails?.ProjectSites != null)
                {
                    Paragraph establishmentParagraph = new Paragraph(licenceDetails.ProjectSites.EstablishmentName, font)
                    {
                        Alignment = Element.ALIGN_JUSTIFIED,
                        IndentationLeft = 10f,
                        IndentationRight = 10f
                    };
                    documents.Add(establishmentParagraph);

                    Paragraph addressParagraph = new Paragraph(licenceDetails.ProjectSites.Address, font)
                    {
                        Alignment = Element.ALIGN_JUSTIFIED,
                        IndentationLeft = 10f,
                        IndentationRight = 10f
                    };
                    documents.Add(addressParagraph);
                }

                if (dofNumber != null)
                {
                    Paragraph dofNoParagraph = new Paragraph(dofNumber, fontBold)
                    {
                        Alignment = Element.ALIGN_JUSTIFIED,
                        IndentationLeft = 10f,
                        IndentationRight = 10f
                    };
                    documents.Add(dofNoParagraph);

                    Paragraph dateParagraph = new Paragraph("Date :" + DateTime.Now, fontBold)
                    {
                        Alignment = Element.ALIGN_JUSTIFIED,
                        IndentationLeft = 10f,
                        IndentationRight = 10f
                    };
                    documents.Add(dateParagraph);

                    Paragraph fileNoParagraph = new Paragraph("File No :" + licenceDetails.PublicAppRefNum, fontBold)
                    {
                        Alignment = Element.ALIGN_JUSTIFIED,
                        IndentationLeft = 10f,
                        IndentationRight = 10f
                    };
                    documents.Add(fileNoParagraph);
                }

                Paragraph subject = new Paragraph("\nSubject: Acceptance of Application for Approval of Combined Building Plan For PSIEC And Factories.", fontBold)
                {
                    Alignment = Element.ALIGN_JUSTIFIED,
                    IndentationLeft = 10f,
                    IndentationRight = 10f
                };
                documents.Add(subject);
                string establishmentName = licenceDetails.ProjectSites.EstablishmentName;
                string address = licenceDetails.ProjectSites.Address;

                Chunk normalChunk1 = new Chunk("\nThe proposed building plans of ", font);
                Chunk boldChunk1 = new Chunk(establishmentName + ", ", fontBold);
                Chunk normalChunk2 = new Chunk(address + ", Punjab (India) ", fontBold);
                Chunk normalChunk3 = new Chunk("have been found correct in accordance with the provisions of (Rule 3-A) of the Punjab Factories Rule, 1952 & ", font);
                Chunk normalChunk4 = new Chunk("got accepted under Factories Act, 1948 by the Chief Inspector of Factories, Punjab, subject to the following conditions: -", font);

                Phrase paragraphPhrase = new Phrase();
                paragraphPhrase.Add(normalChunk1);
                paragraphPhrase.Add(boldChunk1);
                paragraphPhrase.Add(normalChunk2);
                paragraphPhrase.Add(normalChunk3);
                paragraphPhrase.Add(normalChunk4);

                Paragraph body = new Paragraph(paragraphPhrase)
                {
                    Alignment = Element.ALIGN_JUSTIFIED,
                    IndentationLeft = 10f,
                    IndentationRight = 10f
                };
                documents.Add(body);

                documents.Add(new Paragraph("\n", font));

                //  string[] conditions = new[]
                //  {
                //    "Amended Factory Building Plans shall be submitted to Chief Inspector of Factories, Punjab in case any change in area, building, plant & machinery and manufacturing process in future.",
                //    "Applicant shall obtain any other permission required under building bye-law of Housing Department of Punjab Land Authority or any other Act at his own.",
                //    "The Building Cess Rs. " + feeDetails.Where(x=> x.FeeHeaderRefId == 19).Select(x=> x.AmountRaised) + " has been deposited by the applicant under the Building & other construction worker's Welfare Cess Act, 1996. However, the applicant shall deposit the difference of amount, if any, found at any point of time.",
                //    "The PSIEC fee Rs. " + feeDetails.Where(x=> x.FeeHeaderRefId == 39).Select(x=> x.AmountRaised) + " has been deposited by the applicant under the Scrutiny Fee. However, the applicant shall deposit the difference of amount, if any, found at any point of time.",
                //    "The occupier shall submit form no. 1-F (Stability Certificate for building/revised building and after expiry of 5 years) after the completion of building and installation of plant and machinery duly certified by the Competent Person.",
                //    "The occupier shall submit the application in Form No.2 to get Factory Licence after the completion of plant and machinery before start of manufacturing process.",
                //    "The Concerned Party will make sure that all rules and regulations are followed and that NOCs/ approvals as required to be taken from other departments/ offices/ agencies be taken accordingly. The concerned party shall be solely responsible for any kind of violations/ deviations/ diversions made by it. Strict action shall be taken accordingly as per the rules.",
                //    "Occupier shall be liable to obtain NOC from Fire Department"
                //};

                var buildingCess = feeDetails.Where(x => x.FeeHeaderRefId == 48).Sum(x => x.AmountRaised);
                var psiecFee = feeDetails.Where(x => x.FeeHeaderRefId == 39).Sum(x => x.AmountRaised);
                string[] conditions = new[]
                {
                    "Amended Factory Building Plans shall be submitted to Chief Inspector of Factories, Punjab in case any change in area, building, plant & machinery and manufacturing process in future.",
                    "Applicant shall obtain any other permission required under building bye-law of Housing Department of Punjab Land Authority or any other Act at his own.",
                    $"The Building Cess Rs. {buildingCess:N2} has been deposited by the applicant under the Building & other construction worker's Welfare Cess Act, 1996. However, the applicant shall deposit the difference of amount, if any, found at any point of time.",
                    $"The PSIEC fee Rs. {psiecFee:N2} has been deposited by the applicant under the Scrutiny Fee. However, the applicant shall deposit the difference of amount, if any, found at any point of time.",
                    "The occupier shall submit form no. 1-F (Stability Certificate for building/revised building and after expiry of 5 years) after the completion of building and installation of plant and machinery duly certified by the Competent Person.",
                    "The occupier shall submit the application in Form No.2 to get Factory Licence after the completion of plant and machinery before start of manufacturing process.",
                    "The Concerned Party will make sure that all rules and regulations are followed and that NOCs/ approvals as required to be taken from other departments/ offices/ agencies be taken accordingly. The concerned party shall be solely responsible for any kind of violations/ deviations/ diversions made by it. Strict action shall be taken accordingly as per the rules.",
                    "Occupier shall be liable to obtain NOC from Fire Department"
                };

                int index = 1;
                foreach (var condition in conditions)
                {
                    Paragraph conditionParagraph = new Paragraph($"{index}. {condition}", font)
                    {
                        Alignment = Element.ALIGN_JUSTIFIED,
                        IndentationLeft = 10f,
                        IndentationRight = 10f
                    };
                    documents.Add(conditionParagraph);
                    index++;
                }

                Paragraph additionalText = new Paragraph("\nAdditional Director of Factories, Punjab\nFor Director of Factories (Chief Inspector of Factories), Punjab", fontBold)
                {
                    Alignment = Element.ALIGN_RIGHT,
                    SpacingBefore = 10f,
                    SpacingAfter = 10f,
                    IndentationLeft = 10f,
                    IndentationRight = 10f
                };
                documents.Add(additionalText);

                PdfPTable footerTable = new PdfPTable(1)
                {
                    TotalWidth = PageSize.A4.Width - 80,
                    LockedWidth = true,
                    SpacingBefore = 0,
                    SpacingAfter = 0
                };

                PdfPCell footerCell = new PdfPCell(new Phrase("This letter is electronically generated, so does not require any signature.", smallFont))
                {
                    Border = PdfPCell.NO_BORDER,
                    HorizontalAlignment = Element.ALIGN_CENTER,
                    VerticalAlignment = Element.ALIGN_BOTTOM,
                    PaddingLeft = 0,
                    PaddingRight = 0,
                    PaddingTop = 0,
                    PaddingBottom = 0
                };
                footerTable.AddCell(footerCell);
                footerTable.WriteSelectedRows(0, -1, 0, 40, owriter.DirectContent);

                //documents.Close();
                //return genericFormModel;


                documents.Close();

                // Merge with existing PDF
                var factoryWingApproval = await _context.ApplicationDocuments.Where(x => x.AppRefId == appRefId && x.DocumentRefId == 60034).OrderByDescending(x => x.AppDocId).Select(x => x.AttachmentName).FirstOrDefaultAsync();
                var psiecApproval = await _context.ApplicationDocuments.Where(x => x.AppRefId == appRefId && x.DocumentRefId == 60035).OrderByDescending(x => x.AppDocId).Select(x => x.AttachmentName).FirstOrDefaultAsync();

                if (factoryWingApproval != null || psiecApproval != null)
                {
                    string mergedFileName = $"Merged_{guId}.pdf";
                    string mergedPdfPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "TempFiles", mergedFileName);

                    var filesToMerge = new List<string>();
                    filesToMerge.Add(SaveDirectoryPath);

                    // Add 60034 PDF if exists
                    if (factoryWingApproval != null)
                    {
                        string existingPdf = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "AppFiles", factoryWingApproval);
                        filesToMerge.Add(existingPdf);
                    }

                    // Add 60035 PDF if exists
                    if (psiecApproval != null)
                    {
                        string existingPdf2 = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "AppFiles", psiecApproval);
                        filesToMerge.Add(existingPdf2);
                    }

                    PdfReader.unethicalreading = true;
                    MergePdfFiles(mergedPdfPath, filesToMerge);
                    genericFormModel.PdfNameGUID = mergedFileName;
                }
                return genericFormModel;
            }
            catch (Exception ex)
            {
                genericFormModel.HasException = true;
                genericFormModel.Exceptions = ex.Message;
                return genericFormModel;
            }
        }

        public void MergePdfFiles(string outputFilePath, List<string> pdfFiles)
            {
                using (FileStream stream = new FileStream(outputFilePath, FileMode.Create))
                {
                    Document document = new Document();
                    PdfCopy pdf = new PdfCopy(document, stream);
                    document.Open();

                    foreach (string file in pdfFiles)
                    {
                        if (!File.Exists(file)) continue;
                        using (PdfReader reader = new PdfReader(file))
                        {
                            for (int i = 1; i <= reader.NumberOfPages; i++)
                            {
                                pdf.AddPage(pdf.GetImportedPage(reader, i));
                            }
                            pdf.FreeReader(reader);
                            reader.Close();
                        }
                    }
                    document.Close();
                }
            }

        public async Task<CertificateGenerateServiceResultTemplate> Certificate_AppForm_76(long appRefId, ApplicationTypeEnum applicationType,  string userName, bool isDeemedCalling)

        {
            CertificateGenerateServiceResultTemplate result = new CertificateGenerateServiceResultTemplate();

            try
            {
                List<StoreProcedureParm> storeProcedureParms = new List<StoreProcedureParm>()
        {
            new StoreProcedureParm()
            {
                ParmName = "AppId", ParmValue = appRefId.ToString(), isNumber = true
            }
        };

                var spResult = await _iGeneric_SP_Repository.CallStoreProcedureReaderAsync<StabiltyAcknoweldgementReceiptViewModel>( "sp_GetStabiltyAcknowDetailByAppRefId", storeProcedureParms);

                var ackSlipData = spResult.FirstOrDefault();

                if (ackSlipData == null)
                {
                    result.HasException = true;
                    result.Exceptions = "No data found for this application.";
                    return result;
                }

                string folderPath = Path.Combine(
                    Directory.GetCurrentDirectory(),
                    "wwwroot",
                    "License",
                    "AppForm_BP_DECLARATION_STABILITY_CERTIFICATE");

                if (!Directory.Exists(folderPath))
                    Directory.CreateDirectory(folderPath);

                string pdfName = $"{ackSlipData.FileNumber}.pdf";
                string savePath = Path.Combine(folderPath, pdfName);
                if (File.Exists(savePath))
                {
                    result.PdfNameGUID = pdfName;   
                    return result;                  
                }

                using (Document doc = new Document(PageSize.A4, 30, 30, 30, 30))
                {
                    PdfWriter writer = PdfWriter.GetInstance(doc, new FileStream(savePath, FileMode.Create));

                    writer.SetEncryption(
                        PdfWriter.STRENGTH128BITS,
                        "",
                        "",
                        PdfWriter.AllowPrinting);

                    doc.Open();

                    float leftMargin = doc.LeftMargin;
                    float rightMargin = doc.RightMargin;
                    float topMargin = doc.TopMargin;
                    float bottomMargin = doc.BottomMargin;

                    doc.SetMargins(0, 0, 0, 0);
                    doc.NewPage();

                    BaseColor lightBlue = new BaseColor(204, 229, 255);

                    PdfPTable header = new PdfPTable(3);
                    header.WidthPercentage = 100;
                    header.SetWidths(new float[] { 15f, 70f, 15f });
                    header.DefaultCell.Padding = 0;

                    string leftLogoPath = Path.Combine(Directory.GetCurrentDirectory(), "content/images/punjablogo.png");
                    Image leftLogo = Image.GetInstance(leftLogoPath);
                    leftLogo.ScaleAbsolute(55f, 55f);

                    PdfPCell leftCell = new PdfPCell(leftLogo)
                    {
                        BackgroundColor = lightBlue,
                        Border = Rectangle.NO_BORDER,
                        Padding = 0,
                        HorizontalAlignment = Element.ALIGN_CENTER,
                        VerticalAlignment = Element.ALIGN_MIDDLE
                    };

                    PdfPCell centerCell = new PdfPCell()
                    {
                        BackgroundColor = lightBlue,
                        Border = Rectangle.NO_BORDER,
                        PaddingTop = 12,
                        PaddingBottom = 12,
                        HorizontalAlignment = Element.ALIGN_CENTER,
                        VerticalAlignment = Element.ALIGN_MIDDLE
                    };

                    centerCell.AddElement(new Paragraph("ACKNOWLEDGEMENT",
                        FontFactory.GetFont("Times-Roman", 20, Font.BOLD))
                    { Alignment = Element.ALIGN_CENTER });

                    centerCell.AddElement(new Paragraph("SUBMISSION OF STABILITY CERTIFICATE",
                        FontFactory.GetFont("Times-Roman", 14, Font.BOLD))
                    { Alignment = Element.ALIGN_CENTER });

                    string rightLogoPath = Path.Combine(Directory.GetCurrentDirectory(), "content/images/logoLabour.png");
                    Image rightLogo = Image.GetInstance(rightLogoPath);
                    rightLogo.ScaleAbsolute(55f, 55f);

                    PdfPCell rightCell = new PdfPCell(rightLogo)
                    {
                        BackgroundColor = lightBlue,
                        Border = Rectangle.NO_BORDER,
                        Padding = 0,
                        HorizontalAlignment = Element.ALIGN_CENTER,
                        VerticalAlignment = Element.ALIGN_MIDDLE
                    };

                    header.AddCell(leftCell);
                    header.AddCell(centerCell);
                    header.AddCell(rightCell);
                    doc.Add(header);

                    doc.SetMargins(leftMargin, rightMargin, topMargin, bottomMargin);
                    doc.Add(new Paragraph("\n"));

                    PdfPTable table = new PdfPTable(2);
                    table.WidthPercentage = 100;

                   
                    Func<string, PdfPCell> H = (txt) => new PdfPCell(new Phrase(
                        txt,
                        FontFactory.GetFont("Times-Roman", 11, Font.BOLD)))
                    {
                        Padding = 6,
                        BackgroundColor = new BaseColor(235, 235, 235)
                    };

                    
                    Func<string, PdfPCell> N = (txt) => new PdfPCell(new Phrase(
                        txt,
                        FontFactory.GetFont("Times-Roman", 11, Font.NORMAL)))
                    {
                        Padding = 6,
                        BackgroundColor = BaseColor.WHITE
                    };

                    table.AddCell(H("Acknowledgement No."));
                    table.AddCell(N(ackSlipData.FileNumber));

                    table.AddCell(H("Applicant Name"));
                    table.AddCell(N(ackSlipData.ApplicantName));

                    table.AddCell(H("Establishment Name"));
                    table.AddCell(N(ackSlipData.EstablishmentName));

                    table.AddCell(H("Address"));
                    table.AddCell(N($"{ackSlipData.Address}, {ackSlipData.VillageOrTown}, {ackSlipData.TehsilName}, {ackSlipData.DistrictName}, {ackSlipData.PinCode}"));

                    table.AddCell(H("Mobile"));
                    table.AddCell(N(ackSlipData.MobileNo));

                    table.AddCell(H("Email"));
                    table.AddCell(N(ackSlipData.Email));

                    table.AddCell(H("Application Type"));
                    table.AddCell(N(ackSlipData.ApplicationType.ToString()));

                    table.AddCell(H("Purpose"));
                    table.AddCell(N(ackSlipData.ApplicationPurposeType.ToString()));

                    table.AddCell(H("Approved Date"));
                    table.AddCell(N(ackSlipData.ApprovedDate.ToString("dd-MM-yyyy")));

                    doc.Add(table);

                  
                    if (ackSlipData.IsStabilityApproved == 1)
                    {
                        doc.Add(new Paragraph("\nCompetent Person Details",
                            FontFactory.GetFont("Times-Roman", 14, Font.BOLD)));

                        PdfPTable cp = new PdfPTable(2);
                        cp.WidthPercentage = 100;

                        cp.AddCell(H("Name"));
                        cp.AddCell(N(ackSlipData.CompetentPersonName));

                        cp.AddCell(H("Contact No."));
                        cp.AddCell(N(ackSlipData.CompetentPersonContactNo));

                        cp.AddCell(H("Email"));
                        cp.AddCell(N(ackSlipData.CompetentPersonEmail));

                        doc.Add(cp);
                    }

                 
                    if (ackSlipData.IsStabilityApproved == 3)
                    {
                        doc.Add(new Paragraph("\nEngineer Details",
                            FontFactory.GetFont("Times-Roman", 14, Font.BOLD)));

                        PdfPTable eng = new PdfPTable(2);
                        eng.WidthPercentage = 100;

                        eng.AddCell(H("Name"));
                        eng.AddCell(N(ackSlipData.EngineerName));

                        eng.AddCell(H("Contact No."));
                        eng.AddCell(N(ackSlipData.EngineerContactNo));

                        eng.AddCell(H("Email"));
                        eng.AddCell(N(ackSlipData.EngineerEmail));

                        doc.Add(eng);
                    }

                    doc.Add(new Paragraph("\n"));

                   
                    PdfPTable noteTable = new PdfPTable(1);
                    noteTable.WidthPercentage = 100;

                    noteTable.AddCell(new PdfPCell(new Phrase(
                        "NOTE: This acknowledgement is system-generated and requires no physical signature.",
                        FontFactory.GetFont("Times-Roman", 12, Font.BOLD, new BaseColor(200, 0, 0))))
                    {
                        Padding = 10,
                        BackgroundColor = new BaseColor(255, 204, 204),
                        BorderColor = new BaseColor(255, 0, 0),
                        BorderWidth = 1.5f
                    });

                    doc.Add(noteTable);

                    doc.Close();
                }

                result.PdfNameGUID = pdfName;
            }
            catch (Exception ex)
            {
                result.HasException = true;
                result.Exceptions = ex.Message;
            }

            return result;
        }








       
        
       

    }
}