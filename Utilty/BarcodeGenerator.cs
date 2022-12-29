using System;
using System.Drawing;

namespace Core.Infrastructure.Utilty
{

    public interface IBarcodeGenerator
    {
        Bitmap CreateBarcodeImage(string barcode);
        byte[] CreateBarcodeByte(string barcode);
        string CreateBarcode(string supplier, string category, string brand, string product);

    }


    public class BarcodeGenerator : IBarcodeGenerator
    {
        public Bitmap CreateBarcodeImage(string barcode)
        {
            var ean13 = new Ean13();
            try
            {
                ean13.CountryCode = barcode.Substring(0, 2);
                ean13.ManufacturerCode = barcode.Substring(2, 5);
                ean13.ProductCode = barcode.Substring(7, 5);
                ean13.Scale = (float)Convert.ToDecimal(0.8);
            }
            catch
            {
            }
            Bitmap bmp = ean13.CreateBitmap();

            return bmp;

        }

        public byte[] CreateBarcodeByte(string barcode)
        {
            var bmp = CreateBarcodeImage(barcode);
            byte[] imageData = (byte[])System.ComponentModel.TypeDescriptor.GetConverter(bmp).ConvertTo(bmp, typeof(byte[]));
            return imageData;

        }

        public string CreateBarcode(string supplier, string category, string brand, string product)
        {
            string strCountry, strManufacture, strProductCd;
            Ean13 ean13 = new Ean13(); //Added by Mahfuj

            string flag = "", supp = "", grp = "", prd = "", cat = "", ChkSum = "";
            try
            {
                supp = supplier.Substring(8, 2);
                cat = category.Substring(8, 2);
                grp = brand.Substring(8, 2);
                prd = product.Substring(4, 6);


                flag = supp + grp + cat + prd;

                strCountry = flag.Substring(0, 2);
                strManufacture = flag.Substring(2, 5);
                strProductCd = flag.Substring(7, 5);
                ean13.CalculateChecksumDigitAddi(strCountry, strManufacture, strProductCd);

                ChkSum = ean13.ChecksumDigit_sa;
                flag = flag + ChkSum;
                ean13.CountryCode = flag.Substring(0, 2);
                ean13.ManufacturerCode = flag.Substring(2, 5);
                ean13.ProductCode = flag.Substring(7, 5);
            }
            catch { }
            return flag;

        }
    }
}
