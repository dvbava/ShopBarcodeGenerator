using MessagingToolkit.Barcode;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;

namespace BarcodeGen
{
    public class LabelGenerator
    {
        private Generator creator;

        public LabelGenerator()
        {
            creator = new Generator();
        }

        public string GenerateBarcode(string number, bool appendCheckSum, int pageHeight, int pageWidth, string currencySign, decimal price, string tagLine)
        {
            if (appendCheckSum)
            {
                int checkDigit = GetCheckSum(number);
                number = number.ToString().PadLeft(7, '0') + checkDigit;
            }

            if (!Directory.Exists("out-barcode"))
                Directory.CreateDirectory("out-barcode");

            int barWidth = pageWidth;
            int barHeight = pageHeight - 30;
            Generator creator = new Generator();

            var bmpi = creator.CreateBarcode(number, BarcodeFormat.Ean8, barWidth, barHeight);

            Bitmap bmp = bmpi.Clone(new Rectangle(0, 0, barWidth, barHeight), PixelFormat.Format32bppPArgb);

            var fullHeight = pageHeight + 75;
            Bitmap bmp2 = new Bitmap(pageWidth, fullHeight);

            Graphics g = Graphics.FromImage(bmp2);

            g.SmoothingMode = SmoothingMode.AntiAlias;
            g.InterpolationMode = InterpolationMode.HighQualityBicubic;
            g.PixelOffsetMode = PixelOffsetMode.HighQuality;
            g.FillRegion(Brushes.White, new Region(new Rectangle(0, 0, pageWidth, fullHeight)));

            g.DrawString(currencySign + " " + String.Format("{0:0.00}", price), new Font("Tahoma", 15, FontStyle.Bold), Brushes.Black, new PointF(130, 0));
            g.DrawImage(bmp, new RectangleF(-2, 28, pageWidth, barHeight));
            g.DrawString(number, new Font("Tahoma", 12), Brushes.Black, new PointF(130, barHeight + 26));
            g.DrawString(tagLine, new Font("Tahoma", 15), Brushes.Black, new PointF(70, barHeight + 40));

            g.Flush();
            var fileName = "out-barcode\\" + number + ".jpg";
            bmp2.Save(fileName, ImageFormat.Jpeg);

            return fileName;
        }

        public string GenerateEmpty()
        {
            Bitmap bmp = new Bitmap(10, 10);

            Graphics g = Graphics.FromImage(bmp);

            g.SmoothingMode = SmoothingMode.AntiAlias;
            g.InterpolationMode = InterpolationMode.HighQualityBicubic;
            g.PixelOffsetMode = PixelOffsetMode.HighQuality;
            g.FillRegion(Brushes.White, new Region(new Rectangle(0, 0, 10, 10)));
            g.Flush();
            var fileName = "out-barcode\\empty.jpg";
            bmp.Save(fileName, ImageFormat.Jpeg);

            return fileName;

        }

        public static int GetCheckSum(string number)
        {
            var sumEven = 0;
            var sumOdd = 0;

            var chars = number.ToArray();
            for (int i = 1; i <= chars.Length; i++)
            {
                if (i % 2 == 0)
                    sumEven += int.Parse(chars[i - 1].ToString());
                else
                    sumOdd += (int.Parse(chars[i - 1].ToString()) * 3);
            }

            var sumOddEven = sumOdd + sumEven;

            if (sumOddEven % 10 == 0) return 0;

            return 10 - sumOddEven % 10;
        }
    }
}
