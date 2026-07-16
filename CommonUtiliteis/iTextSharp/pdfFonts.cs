using iTextSharp.text;
using System.Drawing;

namespace pbsamadhannetcoreapi.Services
{
    public class pdfFonts
    {
        // BaseFont bfTimes = BaseFont.CreateFont(BaseFont.HELVETICA , BaseFont.CP1252, false);


        public pdfFonts()
        {
            //
            // TODO: Add constructor logic here
            //
        }

        /// <summary>
        /// Summary description for Font for Top Header
        /// </summary>
        public iTextSharp.text.Font TopHeader()
        {
            iTextSharp.text.Font fontTopHeader = FontFactory.GetFont(FontFactory.COURIER, 13, iTextSharp.text.Font.BOLD);
            return fontTopHeader;
        }

        /// <summary>
        /// Summary description for Font for Application Header
        /// </summary>
        public iTextSharp.text.Font AppHeader()
        {
            iTextSharp.text.Font fontAppHeader = FontFactory.GetFont(FontFactory.HELVETICA, 11, iTextSharp.text.Font.BOLD);
            return fontAppHeader;
        }

        /// <summary>
        /// Summary description for Font for Acknowledgement Application Header
        /// </summary>
        public iTextSharp.text.Font AckAppHeader()
        {
            iTextSharp.text.Font fontAppHeader = FontFactory.GetFont(FontFactory.HELVETICA, 11, iTextSharp.text.Font.BOLD, BaseColor.BLACK);
            return fontAppHeader;
        }

        /// <summary>
        /// Summary description for Font for Acknowledgement Application Header
        /// </summary>
        public iTextSharp.text.Font SocietyHeader()
        {
            iTextSharp.text.Font fontAppHeader = FontFactory.GetFont(FontFactory.HELVETICA, 25, iTextSharp.text.Font.NORMAL, BaseColor.RED);
            return fontAppHeader;
        }

        /// <summary>
        /// Summary description for Font for Acknowledgement Application Header
        /// </summary>
        public iTextSharp.text.Font SocietySubHeader()
        {
            iTextSharp.text.Font fontAppHeader = FontFactory.GetFont(FontFactory.HELVETICA, 15, iTextSharp.text.Font.BOLD, new BaseColor(103, 109, 165));
            return fontAppHeader;
        }
        /// <summary>
        /// Summary description for Font for Super Script
        /// </summary>
        public iTextSharp.text.Font SuperScript()
        {
            iTextSharp.text.Font fontAppHeader = FontFactory.GetFont(FontFactory.HELVETICA, 9, iTextSharp.text.Font.NORMAL, new BaseColor(103, 109, 165));
            return fontAppHeader;
        }

        /// <summary>
        /// Summary description for Font for Acknowledgement Application Header
        /// </summary>
        public iTextSharp.text.Font AckAppSubHeader()
        {
            iTextSharp.text.Font fontAppHeader = FontFactory.GetFont(FontFactory.HELVETICA, 9, iTextSharp.text.Font.UNDERLINE, BaseColor.BLACK);
            return fontAppHeader;
        }
        /// <summary>
        /// Summary description for Font for UnderLine
        /// </summary>
        public iTextSharp.text.Font UnderLine()
        {
            iTextSharp.text.Font fontUnderLine = FontFactory.GetFont(FontFactory.HELVETICA, 11, iTextSharp.text.Font.UNDERLINE, BaseColor.BLACK);
            return fontUnderLine;
        }

        /// <summary>
        /// Summary description for Font for Cell Header
        /// </summary>
        public iTextSharp.text.Font CellHeader()
        {
            iTextSharp.text.Font fontCellHeader = FontFactory.GetFont(FontFactory.COURIER, 12, iTextSharp.text.Font.BOLD);
            return fontCellHeader;
        }

        /// <summary>
        /// Summary description for Font for Dynamic Data
        /// </summary>
        public iTextSharp.text.Font DynamicData()
        {
            iTextSharp.text.Font fontDynamicData = FontFactory.GetFont(FontFactory.HELVETICA, 10, iTextSharp.text.Font.NORMAL);
            return fontDynamicData;
        }

        /// <summary>
        /// Summary description for Font for Table Heading
        /// </summary>
        public iTextSharp.text.Font TableHeader()
        {
            iTextSharp.text.Font fontTableHeader = FontFactory.GetFont(FontFactory.HELVETICA, 12, iTextSharp.text.Font.BOLD);
            return fontTableHeader;
        }

        /// <summary>
        /// Summary description for Font for App Sub Heading
        /// </summary>
        public iTextSharp.text.Font SubHeader()
        {
            iTextSharp.text.Font fontSubHeader = FontFactory.GetFont(FontFactory.HELVETICA, 10, iTextSharp.text.Font.BOLD);
            return fontSubHeader;
        }

        /// <summary>
        /// Summary description for Font for App Sub Sub Heading
        /// </summary>
        public iTextSharp.text.Font SubSubHeader()
        {
            iTextSharp.text.Font fontSubSubHeader = FontFactory.GetFont(FontFactory.HELVETICA, 8, iTextSharp.text.Font.NORMAL);
            return fontSubSubHeader;
        }

        /// <summary>
        /// Summary description for Font for Grid Type Heading
        /// </summary>
        public iTextSharp.text.Font PointsFontNormal()
        {
            iTextSharp.text.Font fontGridTypeHeader = FontFactory.GetFont(FontFactory.HELVETICA, 9, iTextSharp.text.Font.NORMAL);
            return fontGridTypeHeader;
        }
        /// <summary>
        /// Summary description for Font for Grid Type Heading
        /// </summary>
        public iTextSharp.text.Font SubPointsFontNormal()
        {
            iTextSharp.text.Font fontGridTypeHeader = FontFactory.GetFont(FontFactory.HELVETICA, 7, iTextSharp.text.Font.NORMAL);
            return fontGridTypeHeader;
        }

        /// <summary>
        /// Summary description for Points
        /// </summary>
        public iTextSharp.text.Font PointsFont()
        {
            iTextSharp.text.Font fontPoints = FontFactory.GetFont(FontFactory.HELVETICA, 9, iTextSharp.text.Font.BOLD);
            return fontPoints;
        }
        /// <summary>
        /// Summary description for Society Words
        /// </summary>
        public iTextSharp.text.Font SocietyFontUnderLine()
        {
            iTextSharp.text.Font fontPoints = FontFactory.GetFont(FontFactory.HELVETICA, 15, iTextSharp.text.Font.BOLD, new BaseColor(103, 109, 165));
            return fontPoints;
        }
        /// <summary>
        /// Summary description for Society Words
        /// </summary>
        public iTextSharp.text.Font SocietyFont()
        {
            iTextSharp.text.Font fontAppHeader = FontFactory.GetFont(FontFactory.HELVETICA, 15, iTextSharp.text.Font.NORMAL, new BaseColor(103, 109, 165));
            return fontAppHeader;
        }
        /// <summary>
        /// Summary description for Header Bold Points
        /// </summary>
        public iTextSharp.text.Font HeaderPointsBoldFont()
        {
            iTextSharp.text.Font fontPoints = FontFactory.GetFont(FontFactory.HELVETICA, 8, iTextSharp.text.Font.BOLD);
            return fontPoints;
        }

        /// <summary>
        /// Summary description for Header Normal Points
        /// </summary>
        public iTextSharp.text.Font HeaderPointsNormalFont()
        {

            iTextSharp.text.Font fontPoints = FontFactory.GetFont(FontFactory.HELVETICA, 9, iTextSharp.text.Font.NORMAL);
            return fontPoints;
        }

        /// <summary>
        /// Summary description for Small Points
        /// </summary>
        public iTextSharp.text.Font SmallPointsNormalFont()
        {

            iTextSharp.text.Font fontPoints = FontFactory.GetFont(FontFactory.HELVETICA, 7, iTextSharp.text.Font.NORMAL);
            return fontPoints;
        }

        /// <summary>
        /// Summary description for Grid Font
        /// </summary>
        public iTextSharp.text.Font GridFont()
        {
            iTextSharp.text.Font fontPoints = FontFactory.GetFont(FontFactory.HELVETICA, 8, iTextSharp.text.Font.NORMAL);
            return fontPoints;
        }

        /// <summary>
        /// Summary description for Grid Font Ack
        /// </summary>
        public iTextSharp.text.Font GridFontAck()
        {
            iTextSharp.text.Font fontPoints = FontFactory.GetFont(FontFactory.HELVETICA, 9, iTextSharp.text.Font.NORMAL);
            return fontPoints;
        }

        /// <summary>
        /// Summary description for Grid Font Bold
        /// </summary>
        public iTextSharp.text.Font SocietyFontUnderLineBold()
        {
            iTextSharp.text.Font fontPoints = FontFactory.GetFont(FontFactory.HELVETICA, 10, iTextSharp.text.Font.BOLD);
            return fontPoints;
        }

        /// <summary>
        /// Summary description for Grid Font Bold
        /// </summary>
        public iTextSharp.text.Font GridFontBold()
        {
            iTextSharp.text.Font fontPoints = FontFactory.GetFont(FontFactory.HELVETICA, 8, iTextSharp.text.Font.BOLD);
            return fontPoints;
        }
        /// <summary>
        /// Summary description for Points Underline
        /// </summary>
        public iTextSharp.text.Font PointsUnderLineFont()
        {
            iTextSharp.text.Font fontUnderLineFont = FontFactory.GetFont(FontFactory.HELVETICA, 9, iTextSharp.text.Font.UNDERLINE);
            return fontUnderLineFont;
        }

        /// <summary>
        /// Summary description for Without Points Underline
        /// </summary>
        public iTextSharp.text.Font WithoutPointsUnderLineFont()
        {
            iTextSharp.text.Font fontWithoutUnderLineFont = FontFactory.GetFont(FontFactory.HELVETICA, 12, iTextSharp.text.Font.UNDERLINE);
            return fontWithoutUnderLineFont;
        }

        public iTextSharp.text.Font NormalFont()
        {
            // FontFactory ff = new FontFactory();


            iTextSharp.text.Font fontPoints = FontFactory.GetFont(FontFactory.HELVETICA, 9, iTextSharp.text.Font.NORMAL);
            return fontPoints;
        }
    }
}