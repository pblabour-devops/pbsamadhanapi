using ClosedXML.Excel;
using Microsoft.AspNetCore.Mvc.Rendering;
using pbsamadhannetcoreapi.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography.X509Certificates;
using iTextSharp.text.pdf;
using System.IO;
using Org.BouncyCastle.Security;
using Org.BouncyCastle.X509;
using X509Certificate = Org.BouncyCastle.X509.X509Certificate;
using iTextSharp.text.pdf.security;
using System.Security.Cryptography;

namespace pbsamadhannetcoreapi.CommonUtiliteis
{
    public static class ExcelOperations<T> where T : class
    {
        public static void GenerateExcelUsingModel(string workSheetName)
        {
            using (var workbook = new XLWorkbook())
            {
                //workbook.Properties.Author = "TestAuther";
                //https://www.codeproject.com/Articles/1089368/Apply-Data-Validation-to-Excel-Cells-in-Csharp
                //https://github.com/ClosedXML/ClosedXML-wiki/blob/master/Data-Validation.md
                PropertyInfo[] properties = typeof(T).GetProperties();
                var worksheet = workbook.Worksheets.Add(workSheetName);
                int colCount = 1;
                int enulListColIndex = 100;
                foreach (PropertyInfo property in properties)
                {
                    //1. Add column
                    
                    
                    //2. Set Column Datatype
                    if (property.PropertyType.Name.ToLower().StartsWith("int") || property.PropertyType.Name.ToLower().StartsWith("decimal"))
                    {
                        worksheet.Cell(1, colCount).DataType = XLDataType.Number;
                    }
                    else if (property.PropertyType.Name.ToLower().StartsWith("string"))
                    {
                        worksheet.Cell(1, colCount).DataType = XLDataType.Text;
                    }
                    else if (property.PropertyType.Name.ToLower().StartsWith("datetime"))
                    {
                        worksheet.Cell(1, colCount).DataType = XLDataType.DateTime;
                    }
                    else if (property.PropertyType.Name.ToLower().StartsWith("bool"))
                    {
                        worksheet.Cell(1, colCount).DataType = XLDataType.Boolean;
                    }
                    else if (property.PropertyType.IsEnum)
                    {
                        worksheet.Cell(1, colCount).DataType = XLDataType.Text;
                        
                        SelectList enumList = EnumOps.GetEnumAsSelectList<AppActionTypeEnum>();
                        int enumIndex = 2;
                        foreach (var enumItem in enumList)
                        {
                            worksheet.Cell(enumIndex, enulListColIndex).Value = enumItem.Text;
                            enumIndex++;
                            //worksheet.Column(100).Style.Protection.Hidden = true;

                        }
                        worksheet.Column(colCount).SetDataValidation().List(worksheet.Range(2, enulListColIndex, enumIndex - 1, enulListColIndex));
                        enulListColIndex++;

                    }
                    worksheet.Cell(1, colCount).Value = property.Name;
                    //3. Add validations
                    IEnumerable<Attribute> propAttributes = property.GetCustomAttributes();
                    StringBuilder errorMsgStr = new StringBuilder();
                    string currErrMsg = "";
                    foreach (var propAttribute in propAttributes)
                    {
                        if (propAttribute.GetType().Name == "RequiredAttribute")
                        {
                            errorMsgStr.Append(propAttributes.OfType<RequiredAttribute>().Where(a => a.ErrorMessage == a.ErrorMessage).ToDictionary(a => a.GetType().Name, a => a).First().Value.ErrorMessage);
                        }
                        else if (propAttribute.GetType().Name == "StringLengthAttribute")
                        {
                            currErrMsg = propAttributes.OfType<StringLengthAttribute>().Where(a => a.ErrorMessage == a.ErrorMessage).ToDictionary(a => a.GetType().Name, a => a).First().Value.ErrorMessage;
                            
                            //Validations
                            worksheet.Column(colCount).SetDataValidation().TextLength.EqualOrLessThan(propAttributes.OfType<StringLengthAttribute>().Where(a => a.MaximumLength == a.MaximumLength).ToDictionary(a => a.GetType().Name, a => a).First().Value.MaximumLength);
                            worksheet.Column(colCount).SetDataValidation().ErrorMessage = currErrMsg;

                            if (errorMsgStr.ToString().Length > 0)
                                errorMsgStr.Append(" and ");
                            errorMsgStr.Append(currErrMsg);


                        }
                        else if (propAttribute.GetType().Name == "RangeAttribute")
                        {
                            currErrMsg = propAttributes.OfType<RangeAttribute>().Where(a => a.ErrorMessage == a.ErrorMessage).ToDictionary(a => a.GetType().Name, a => a).First().Value.ErrorMessage;

                            //Validations
                            worksheet.Column(colCount).SetDataValidation().WholeNumber.Between(
                                Convert.ToInt32(propAttributes.OfType<RangeAttribute>().Where(a => a.Minimum == a.Minimum).ToDictionary(a => a.GetType().Name, a => a).First().Value.Minimum),
                                Convert.ToInt32(propAttributes.OfType<RangeAttribute>().Where(a => a.Maximum == a.Maximum).ToDictionary(a => a.GetType().Name, a => a).First().Value.Maximum));
                            worksheet.Column(colCount).SetDataValidation().ErrorMessage = currErrMsg;

                            if (errorMsgStr.ToString().Length > 0)
                                errorMsgStr.Append(" and ");
                            errorMsgStr.Append(currErrMsg);
                        }
                    }
                    worksheet.Column(colCount).SetDataValidation().InputMessage = errorMsgStr.ToString();
                    errorMsgStr.Clear();

                    colCount++;
                }
                workbook.SaveAs(@"D:\repos\text.xlsx");
            }
        }
    }

    public static class DigitalSignatureOperations
    {
        public static void StempDigitalSignatureToFile()
        {
            //Create a variable for X509Certificate2 
            X509Certificate2 certClient = null;

            //Get all the DSC users registered to local store and for current user using X509Store, PFB
            //509Store "st" will collect all the certificates
            X509Store st = new X509Store(StoreName.My, StoreLocation.CurrentUser);
            st.Open(OpenFlags.MaxAllowed);
            X509Certificate2Collection collection = st.Certificates;

            //Collection will get all the certificates.
            //Select the certificate.
            //If we have multiple certificates registered in local, and if we want to pick the valid certificates using the name, please follow the below process:
            //Send "Name" as the DSC token owner name. You can check if the DSC token is expired or not.After all these checks, assign the value to "certClient".

            for (int i = 0; i < collection.Count; i++)
            {
                foreach (X509Certificate2 cert in collection)
                {
                    certClient = cert;
                    //username = certClient.Subject;
                    //ErrorLogs(username);
                    //startdate = certClient.GetEffectiveDateString();
                    //enddate = certClient.GetExpirationDateString();
                    if (collection[i].Subject.Contains("ac59b47f54011b990b928bde9a8b8b96"))
                    {
                        certClient = collection[i];
                    }
                }
            }
            st.Close();

            //Close the X509Store.


            //Get Certificate Chain    
            IList<X509Certificate> chain = new List<X509Certificate>();
            X509Chain x509Chain = new X509Chain();
            x509Chain.Build(certClient);
            foreach (X509ChainElement x509ChainElement in x509Chain.ChainElements)
            {
                chain.Add(DotNetUtilities.FromX509Certificate(x509ChainElement.Certificate));
            }

            //Select the file to be signed(input file)
            string filename = @"E:\Ebook\timothywilliams.pdf";

            //Assign the file to a PDF reader to edit the file.
            PdfReader inputPdf = new PdfReader(filename);

            //Create a new file to save the signed pdf.
            FileStream signedPdf = new FileStream(@"E:\Ebook\Signed.pdf", FileMode.Create);

            //PdfStamper will create the stamp on the "inputPdf" and create a new "signedPdf"
            PdfStamper pdfStamper = PdfStamper.CreateSignature(inputPdf, signedPdf, '\0');

            AsymmetricAlgorithm algorithm;
            //algorithm.p
            AsymmetricAlgorithm privateKey = certClient.PrivateKey;
            
            //certClient.PrivateKey = algorithm.;
            //Signature is encrypted using SHA-256
            IExternalSignature externalSignature = new X509Certificate2Signature(certClient, "sha256RSA");

            //This will create a signature text in the input pdf
            PdfSignatureAppearance signatureAppearance = pdfStamper.SignatureAppearance;

            //Other information can be added and displayed in the signature section with signature.

            signatureAppearance.Reason = "My Signature";
            signatureAppearance.SetVisibleSignature(new iTextSharp.text.Rectangle(0, 00, 200, 100), inputPdf.NumberOfPages, "Signature");
            signatureAppearance.SignatureRenderingMode = PdfSignatureAppearance.RenderingMode.DESCRIPTION;
            MakeSignature.SignDetached(signatureAppearance, externalSignature, chain, null, null, null, 0, CryptoStandard.CMS);

            //PdfReader and pdfStamper will be closed after the signature is done.
            inputPdf.Close();
            pdfStamper.Close();

        }

    }

    public class SecurityHelper
    {
        public static string GenerateSalt(int nSalt)
        {
            var saltBytes = new byte[nSalt];

            using (var provider = new RNGCryptoServiceProvider())
            {
                provider.GetNonZeroBytes(saltBytes);
            }

            return Convert.ToBase64String(saltBytes);
        }

        public static string HashPassword(string password, string salt)
        {
            var saltBytes = Convert.FromBase64String(salt);

            using (var rfc2898DeriveBytes = new Rfc2898DeriveBytes(password, saltBytes, iterations:1000))
            {
                return Convert.ToBase64String(rfc2898DeriveBytes.GetBytes(cb: 32));
            }
        }
    }
}
