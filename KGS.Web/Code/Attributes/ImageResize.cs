using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Web;

namespace KGS.Web.Code.Attributes
{
    public class ImageResize
    {
        #region [RESIZE UPLOADED IMAGE]
        public static byte[] ResizeImage(Stream InputStream)
        {
            Image uploaded = Image.FromStream(InputStream);
            int originalWidth = uploaded.Width;
            int originalHeight = uploaded.Height;
            float percentWidth = (float)256 / (float)originalWidth;
            float percentHeight = (float)256 / (float)originalHeight;
            float percent = percentHeight < percentWidth ? percentHeight : percentWidth;
            int newWidth = (int)(originalWidth * percent);
            int newHeight = (int)(originalHeight * percent);
            Image newImage = new Bitmap(newWidth, newHeight);
            using (Graphics g = Graphics.FromImage(newImage))
            {
                g.InterpolationMode = InterpolationMode.HighQualityBicubic;
                g.DrawImage(uploaded, 0, 0, newWidth, newHeight);
            }
            byte[] results;
            using (MemoryStream ms = new MemoryStream())
            {
                ImageCodecInfo codec = ImageCodecInfo.GetImageEncoders().FirstOrDefault(c => c.FormatID == ImageFormat.Jpeg.Guid);
                EncoderParameters jpegParms = new EncoderParameters(1);
                jpegParms.Param[0] = new EncoderParameter(Encoder.Quality, 95L);
                newImage.Save(ms, codec, jpegParms);
                results = ms.ToArray();
            }
            return results;
        }
        #endregion 
    }
}