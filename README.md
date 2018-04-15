BarcodeGenerator - EAN-8  for now...
This utility will generate barcode labels in A4 page which you can print on any A4 sticky/paper.

use 'input.csv' as per following format for your input to generate bulk barcodes.

barcode, price, count

example input.csv file:

12345, 12.50, 7
45878, 45, 8


The utility will workout the checksum digit automatilly by default.
Price will be rounded to 2 decimal point.
