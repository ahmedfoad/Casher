using System;
using System.IO;
using DevExpress.XtraPrinting;
using Casher.Reports.Product;
using Casher.Models;
using System.Collections.Generic;

namespace Casher.GlobalClass.Reports
{
    public class Rpt_Product
    {
        internal string ExportBarCodeReport(List<Cls_Product> Cls_Product)
        {

                string _path = System.Web.HttpContext.Current.Server.MapPath(@"~/Reports/pdf"); 
                Random random = new Random();
                string tick = DateTime.Now.Ticks.ToString();
                string reportPath = Path.Combine(_path, "BarCodeReport.pdf");

                BarCodeReport report = new BarCodeReport();
                report.DataSource = Cls_Product;

                PdfExportOptions pdfOptions = report.ExportOptions.Pdf;
                pdfOptions.Compressed = true;
                pdfOptions.ImageQuality = PdfJpegImageQuality.Low;
                pdfOptions.NeverEmbeddedFonts = "Tahoma;Courier New";
                pdfOptions.DocumentOptions.Application = "Human Resources Application";
                pdfOptions.DocumentOptions.Author = "مؤسسة حوران الشامل لقنية الحاسب الآلي والإنترنت";
                pdfOptions.DocumentOptions.Subject = "باركود الصنف";
                pdfOptions.DocumentOptions.Title = "باركود الصنف";

                report.ExportToPdf(reportPath);
                return "BarCodeReport";
             
        }
    }
}