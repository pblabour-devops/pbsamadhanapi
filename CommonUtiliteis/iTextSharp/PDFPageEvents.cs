using iTextSharp.text;
using iTextSharp.text.pdf;
using System;
/// <summary>
/// Summary description for PDFPageEvents
/// </summary>
public class PDFPageEvents : PdfPageEventHelper
{
    protected PdfTemplate total;
    // This is the contentbyte object of the writer
    PdfContentByte cb;
    // we will put the final number of pages in a template
    PdfTemplate template;
    // this is the BaseFont we are going to use for the header / footer
    BaseFont bf = null;
    // This keeps track of the creation time
    DateTime PrintTime = DateTime.Now;
    public PDFPageEvents()
    {
        //
        // TODO: Add constructor logic here
        //
    }

    #region Properties

    /// <summary>
    /// To get or set Title (Type string)
    /// </summary>
    public string Title { get; set; }

    /// <summary>
    /// To get or set HeaderLeft (Type string)
    /// </summary>
    public string HeaderLeft { get; set; }

    /// <summary>
    /// To get or set HeaderRight (Type string)
    /// </summary>
    public string HeaderRight { get; set; }

    /// <summary>
    /// To get or set HeaderFont (Type string)
    /// </summary>
    public Font HeaderFont { get; set; }

    /// <summary>
    /// To get or set FooterFont (Type string)
    /// </summary>
    public Font FooterFont { get; set; }


    #endregion

    // To override the onOpenDocument method
    //Function Name : - OnOpenDocument 
    //Purpose       : - This function will overrides the Existing open document property of the pdf writer 
    //  and will set the Base Font,ContentBytes and its size for the Page
    //Author        : - Kanwaljeet Singh

    public override void OnOpenDocument(PdfWriter writer, Document document)
    {
        try
        {
            PrintTime = DateTime.Now;
            bf = BaseFont.CreateFont(BaseFont.HELVETICA, BaseFont.CP1252, BaseFont.NOT_EMBEDDED);
            cb = writer.DirectContent;
            template = cb.CreateTemplate(50, 50);
        }
        catch// (DocumentException de)
        {

        }
        //catch// (System.IO.IOException ioe)
        //{

        //}
    }
    //Function Name : - OnStartPage 
    //Purpose       : - This function will add the Content Bytes to the PDF Table which is then added to the PDF
    //Author        : - Kanwaljeet Singh
    public override void OnStartPage(PdfWriter writer, Document document)
    {
        base.OnStartPage(writer, document);

        Rectangle pageSize = document.PageSize;
        Title = "";
        if (Title != string.Empty)
        {
            cb.BeginText();
            cb.SetFontAndSize(bf, 15);
            //cb.SetRGBColorFill(50, 50, 200);//Used to the Default Font for the PDF Page
            cb.SetTextMatrix(pageSize.GetLeft(40), pageSize.GetTop(40));
            cb.ShowText(Title);
            cb.EndText();
        }

        if (HeaderLeft + HeaderRight != string.Empty)
        {
            PdfPTable HeaderTable = new PdfPTable(2);
            HeaderTable.DefaultCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            HeaderTable.TotalWidth = pageSize.Width - 80;
            HeaderTable.SetWidthPercentage(new float[] { 45, 45 }, pageSize);

            PdfPCell HeaderLeftCell = new PdfPCell(new Phrase(8, HeaderLeft, HeaderFont));
            HeaderLeftCell.Padding = 5;
            HeaderLeftCell.PaddingBottom = 0;
            HeaderLeftCell.BorderWidthRight = 0;
            HeaderTable.AddCell(HeaderLeftCell);

            PdfPCell HeaderRightCell = new PdfPCell(new Phrase(8, HeaderRight, HeaderFont));
            HeaderRightCell.HorizontalAlignment = PdfPCell.ALIGN_RIGHT;
            HeaderRightCell.Padding = 5;
            HeaderRightCell.PaddingBottom = 0;
            HeaderRightCell.BorderWidthLeft = 0;
            HeaderTable.AddCell(HeaderRightCell);

            // cb.SetRGBColorFill(0, 0, 0);
            HeaderTable.WriteSelectedRows(0, -1, pageSize.GetLeft(40), pageSize.GetTop(10), cb);
        }
    }
    //Function Name : - OnEndPage 
    //Purpose       : - This function will add the calculated total no of pages and the page no in the Content Bytes
    //Author        : - Kanwaljeet Singh
    public override void OnEndPage(PdfWriter writer, Document document)
    {
        //base.OnEndPage(writer, document);

        //int pageN = writer.PageNumber;
        //String text = "";
        ////String text = "Page " + pageN + " of ";
        //float len = bf.GetWidthPoint(text, 8);

        //Rectangle pageSize = document.PageSize;

        ////cb.SetRGBColorFill(100, 100, 100);

        //cb.BeginText();
        //cb.SetFontAndSize(bf, 8);
        //cb.SetTextMatrix(pageSize.GetLeft(540), pageSize.GetBottom(2));
        //cb.ShowText(text);
        //cb.EndText();
        ////Template to Add Total No Pages which is calculated in the Close Document Function
        //cb.AddTemplate(template, pageSize.GetLeft(540) + len, pageSize.GetBottom(2));

        //cb.BeginText();
        //cb.SetFontAndSize(bf, 8);
        //cb.ShowTextAligned(PdfContentByte.ALIGN_RIGHT,
        //    "Printed On " + PrintTime.ToString(),
        //    pageSize.GetRight(40),
        //    pageSize.GetBottom(830), 0);
        //cb.EndText();
    }
    //Function Name : - OnCloseDosument 
    //Purpose       : - This function will Count the Total No PDF Pages
    //Author        : - Kanwaljeet Singh
    public override void OnCloseDocument(PdfWriter writer, Document document)
    {
        base.OnCloseDocument(writer, document);

        template.BeginText();
        template.SetFontAndSize(bf, 8);
        template.SetTextMatrix(0, 0);
        template.ShowText("" + (writer.PageNumber - 1));
        template.EndText();
    }

    //Function Name : - DownloadPDF 
    //Purpose       : - This function will provide the Common Function for downloading the pdf file in the browser
    //Author        : - Kanwaljeet Singh
    public void DownloadPDF(string FileName)
    {
        //HttpContext.Current.Response.Cache.SetCacheability(HttpCacheability.NoCache);
        //HttpContext.Current.Response.ClearHeaders();
        //HttpContext.Current.Response.Clear();
        //HttpContext.Current.Response.ContentType = "Application/pdf";
        //HttpContext.Current.Response.WriteFile(FileName);
        //HttpContext.Current.Response.End();
    }
}
