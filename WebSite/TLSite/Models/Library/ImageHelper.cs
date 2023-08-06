using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;
using System.Drawing.Imaging;
using System.Drawing.Drawing2D;
using System.Drawing;
using System.Web.Hosting;

namespace TLSite.Models.Library
{
    #region ImageCropModel
    public class GiftImageSize
    {
        public int p_width { get; set; }
        public int p_height { get; set; }
    }

    public static class ScreenResolution
    {
        public const string SCREEN_320_480 = "320,480";
        public const string SCREEN_480_800 = "480,800"; // 
        public const string SCREEN_640_960 = "640,960"; //iPhone 4 resolution
        public const string SCREEN_640_1136 = "640,1136";   //iPhone 5 resolution
        public const string SCREEN_720_1280 = "720,1280";   //SamSung 
    }

    public static class ImageRatios
    {
        public const string twoToOne = "2:1";
        public const string oneToOne = "1:1";
        public const string fullHeight = "0:0";
        //        public const string fullHeight = "1.4:1";
    }

    public static class CropImageSizes
    {
        public const string X = "X";
        public const string XX = "XX";
        public const string XXX = "XXX";
        public const string BACKGROUND = "BACKGROUND";
    };

    public static class UpImageCategory //Set the Content/UploadFiles/ directory name
    {
        public const string GIFT = "gift";
        public const string ANNOUNCE = "announces";
        public const string COMPINFO = "compinfo";
        public const string EMPLOYEE = "employees";
        public const string NEWCAR = "newcars";
        public const string PHONEBGK = "background";
    };

    public class DefaultCropSizes
    {
        public readonly Dictionary<string, int> DefaultValues;
        public DefaultCropSizes()
        {
            DefaultValues = GetDefaultValues();
        }

        public static Dictionary<string, int> GetDefaultValues()
        {
            Dictionary<string, int> myDictionary = new Dictionary<string, int>();

            myDictionary.Add(ScreenResolution.SCREEN_320_480 + "X", 100);
            myDictionary.Add(ScreenResolution.SCREEN_320_480 + "XX", 150);
            myDictionary.Add(ScreenResolution.SCREEN_320_480 + "XXX", 300);
            myDictionary.Add(ScreenResolution.SCREEN_320_480 + "BACKGROUND", 480);

            myDictionary.Add(ScreenResolution.SCREEN_480_800 + "X", 160);
            myDictionary.Add(ScreenResolution.SCREEN_480_800 + "XX", 240);
            myDictionary.Add(ScreenResolution.SCREEN_480_800 + "XXX", 480);
            myDictionary.Add(ScreenResolution.SCREEN_480_800 + "BACKGROUND", 800);

            myDictionary.Add(ScreenResolution.SCREEN_640_960 + "X", 210);
            myDictionary.Add(ScreenResolution.SCREEN_640_960 + "XX", 315);
            myDictionary.Add(ScreenResolution.SCREEN_640_960 + "XXX", 630);
            myDictionary.Add(ScreenResolution.SCREEN_640_960 + "BACKGROUND", 960);

            myDictionary.Add(ScreenResolution.SCREEN_640_1136 + "X", 210);
            myDictionary.Add(ScreenResolution.SCREEN_640_1136 + "XX", 315);
            myDictionary.Add(ScreenResolution.SCREEN_640_1136 + "XXX", 630);
            myDictionary.Add(ScreenResolution.SCREEN_640_1136 + "BACKGROUND", 1136);

            myDictionary.Add(ScreenResolution.SCREEN_720_1280 + "X", 240);
            myDictionary.Add(ScreenResolution.SCREEN_720_1280 + "XX", 360);
            myDictionary.Add(ScreenResolution.SCREEN_720_1280 + "XXX", 720);
            myDictionary.Add(ScreenResolution.SCREEN_720_1280 + "BACKGROUND", 1280);

            return myDictionary;
        }
    }
    #endregion

    public class ImageHelper
    {
        private void saveJpeg(string path, Bitmap img, long quality)
        {
            // Encoder parameter for image quality
            EncoderParameter qualityParam = new EncoderParameter(Encoder.Quality, quality);

            // Jpeg image codec
            ImageCodecInfo jpegCodec = this.getEncoderInfo("image/jpeg");

            if (jpegCodec == null)
                return;

            EncoderParameters encoderParams = new EncoderParameters(1);
            encoderParams.Param[0] = qualityParam;

            img.Save(path, jpegCodec, encoderParams);
        }

        private ImageCodecInfo getEncoderInfo(string mimeType)
        {
            // Get image codecs for all image formats
            ImageCodecInfo[] codecs = ImageCodecInfo.GetImageEncoders();

            // Find the correct image codec
            for (int i = 0; i < codecs.Length; i++)
                if (codecs[i].MimeType == mimeType)
                    return codecs[i];
            return null;
        }

        public static void ResizeAndCrop(string basepath, string orgfile, string screensize, string ratio, string imgsize)
        {
            string file_prefix = "";
            int width = 0, height = 0;

            int orgWidth = 0, orgHeight = 0;
            int tgtWidth = 0, tgtHeight = 0;
            decimal resize_ratio = 1;

            DefaultCropSizes defsizes = new DefaultCropSizes();
            string[] ratios = ratio.Split(':');

            if (ratio == ImageRatios.fullHeight)
            {
                string[] resolutions = screensize.Split(',');
                height = (int)decimal.Parse(resolutions[1]);
                width = (int)decimal.Parse(resolutions[0]);
                //                 height = (int)decimal.Parse(resolutions[1]);
                //                 width = (int)(Math.Round(decimal.Parse(resolutions[0]) * decimal.Parse(ratios[0])));
            }
            else
            {
                width = defsizes.DefaultValues[screensize + imgsize];
                height = (int)(Math.Round(width * (decimal.Parse(ratios[1]) / decimal.Parse(ratios[0]))));
            }

            file_prefix = screensize.Replace(",", "") + "_" + ratio.Replace(":", "") + "_" + imgsize + "_";

            //first get original image bitmap.
            Image orgimg = Image.FromFile(basepath + orgfile);

            if (orgimg == null) return;

            orgWidth = orgimg.Width;
            orgHeight = orgimg.Height;

            /* Get ratio to fit target image to original image size. */
            if ((orgHeight / (decimal)orgWidth) <= (height / (decimal)width))
            {
                resize_ratio = orgHeight / (decimal)height;
            }
            else
            {
                resize_ratio = orgWidth / (decimal)width;
            }

            /* Fit target image size to orginal image size using computed ratio. */
            tgtWidth = (int)(Math.Round(width * resize_ratio));
            tgtHeight = (int)(Math.Round(height * resize_ratio));

            int startx = (orgWidth - tgtWidth) / 2;
            int starty = (orgHeight - tgtHeight) / 2;

            Bitmap srcBmp = orgimg as Bitmap;

            Rectangle cropSection = new Rectangle(startx,
                                      starty,
                                      (startx == 0) ? orgWidth : tgtWidth,
                                      (starty == 0) ? orgHeight : tgtHeight);

            try
            {
                //Bitmap cropBmp = new Bitmap((startx == 0) ? orgWidth : tgtWidth, 
                //                            (starty == 0) ? orgHeight : tgtHeight);

                //Graphics cropGra = Graphics.FromImage((Image)cropBmp);
                //cropGra.DrawImage(srcBmp, 0,0, cropSection, GraphicsUnit.Pixel);

                /* Make fited and cropped image from original image. */
                using (Bitmap cropBmp = srcBmp.Clone(cropSection, srcBmp.PixelFormat))
                {
                    using (Bitmap targetBmp = new Bitmap(width, height))
                    {
                        /* Resize image to target size and save image to file. */
                        using (Graphics g = Graphics.FromImage((Image)targetBmp))
                        {
                            g.InterpolationMode = InterpolationMode.HighQualityBicubic;
                            g.SmoothingMode = SmoothingMode.AntiAlias;
                            g.CompositingQuality = CompositingQuality.HighQuality;
                            g.PixelOffsetMode = PixelOffsetMode.HighQuality;
                            g.DrawImage(cropBmp, 0, 0, width, height);

                            string savepath = basepath + "small_" + orgfile;
                            try
                            {
                                targetBmp.Save(savepath, ImageFormat.Jpeg);
                            }
                            catch (Exception e)
                            {
                                //CommonModel.WriteLogFile(this.GetType().Name, "ResizeAndCrop()", e.ToString());
                            }
                        }
                    }
                }
            }
            catch (System.Exception ex)
            {
                CommonModel.WriteLogFile("ImageHelper", "ResizeAndCrop()", ex.ToString());
            }

            orgimg.Dispose();
        }
    }
}