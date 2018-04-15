using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Windows;

namespace BarcodeGen
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        LabelGenerator generator;
        LabelPrinter printer;

        public MainWindow()
        {
            InitializeComponent();
            generator = new LabelGenerator();
            printer = new LabelPrinter();
        }

        private void Generate_Click(object sender, RoutedEventArgs e)
        {
            int pageWidth = txtWidth.Text.Length > 0 ? int.Parse(txtWidth.Text) : 360;
            int pageHeight = txtHeight.Text.Length > 0 ? int.Parse(txtHeight.Text) : 110; ;

            var lines = File.ReadAllLines(inputFile.Text);

            List<string> barcodeList = new List<string>();

            foreach (var line in lines)
            {
                var tokens = line.Split(new[] { ',' });
                var filePath = generator.GenerateBarcode(tokens[0].Trim(), (bool)appendCheckSum.IsChecked, pageHeight, pageWidth, txtCurrency.Text, decimal.Parse(tokens[1].Trim()), txtTagLine.Text);

                var count = tokens.Length > 2 ? int.Parse(tokens[2].Trim()) : 1;
                for (int i = 0; i < count; i++)
                    barcodeList.Add(filePath);

            }

            var outFile = printer.Print(barcodeList);
            Process.Start("explorer.exe", Path.Combine(Environment.CurrentDirectory, outFile));
        }

        private void Close_Click(object sender, RoutedEventArgs e)
        {
            Environment.Exit(0);
        }
    }
}
