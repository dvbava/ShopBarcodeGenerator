using iTextSharp.text;
using iTextSharp.text.pdf;
using System;
using System.Collections.Generic;
using System.IO;

namespace BarcodeGen
{
    public class LabelPrinter
    {
        public string Print(List<string> labelList)
        {

            if (!Directory.Exists("out-labels-A4"))
                Directory.CreateDirectory("out-labels-A4");

            labelList = AdjustCount(labelList);

            var outputPath = string.Format("{0}\\{1:dd_MM_yyyy_hh_mm_ss}.pdf", "out-labels-A4", DateTime.Now);

            Document document = new Document(PageSize.A4, 25, 25, 65, 0);
            var writer = PdfWriter.GetInstance(document, new FileStream(outputPath, FileMode.Create));
            document.AddTitle("Barcode generator");
            document.AddAuthor("Dipak Baba");
            document.AddCreator("http://www.sangeetacollection.com");
            document.Open();


            PdfDestination pdfDest = new PdfDestination(PdfDestination.XYZ, 0, 0, 0.92f);
            PdfAction action = PdfAction.GotoLocalPage(1, pdfDest, writer);
            writer.SetOpenAction(action);

           
            PdfPTable table = new PdfPTable(4);
            table.HorizontalAlignment = Element.ALIGN_LEFT;
            table.WidthPercentage = 100;
            table.DefaultCell.Border = Rectangle.NO_BORDER;

            foreach (var item in labelList)
            {
                table.AddCell(CreateImageCell(item));
            }

            document.Add(table);
            document.Close();

            return outputPath;
        }

        public static PdfPCell CreateImageCell(String path)
        {
            Image img = Image.GetInstance(path);
            img.BorderWidth = 0;

            PdfPCell cell = new PdfPCell(img, true);

            cell.Border = Rectangle.NO_BORDER;
            cell.PaddingBottom = 2;
            cell.PaddingLeft = 2;
            cell.PaddingRight = 2;
            cell.PaddingTop = 2;

            return cell;
        }

        private List<string> AdjustCount(List<string> labelList)
        {
            if (labelList.Count % 4 == 0) return labelList;

            var gaps = 4 - labelList.Count % 4;

            LabelGenerator labelGenerator = new LabelGenerator();
            var emptyImage = labelGenerator.GenerateEmpty();

            for (int i = 0; i < gaps; i++)
            {
                labelList.Add(emptyImage);
            }

            return labelList;
        }
    }
}
