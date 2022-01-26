using FrontEnd.Models;
using System;
using System.Configuration;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Web.Mvc;

namespace FrontEnd.Controllers
{
    public class FileController : Controller
    {

        private static int MAX_HEIGHT = 738;
        private static int MAX_WIDTH = 1080;
        private static int QUALITY_IMAGE = 90;
        private static string FILE_PATH = ConfigurationManager.AppSettings["GetLocalPathFile"];

        internal static AttachmentModel PathFile_PATH(AttachmentModel model, string FILE_PATHDATA)
        {
            var file = model.Base64.Split(',')[1];
            var data = Convert.FromBase64String(file);
            DateTime now = DateTime.Now;
            string guid = Guid.NewGuid().ToString();
            string fileLocation = now.Year.ToString();

            model.Code = guid;
            model.FileSize = data.LongLength;
            model.Path = Path.Combine(FILE_PATHDATA, fileLocation);
            model.Base64 = file;
            return model;
        }

        internal static void SaveFile_PATH(AttachmentModel model, string FILE_PATHDATA)
        {
            var data = Convert.FromBase64String(model.Base64);
            if (IsValidImage(data))
            {
                data = ReduceImageSize(data, QUALITY_IMAGE);
            }
            DateTime now = DateTime.Now;
            string extension = Path.GetExtension(model.FileName);
            string guid = model.Code != "" && model.Code != null ? model.Code : Guid.NewGuid().ToString();
            string name = Path.GetFileNameWithoutExtension(model.FileName);
            string newFileName = string.Format("{0}{1}", guid, extension);
            string fileLocation = now.Year.ToString();

            FileManager.SaveFile(Path.Combine(FILE_PATH + FILE_PATHDATA, fileLocation), newFileName, data);
        }

        private static bool IsValidImage(byte[] bytes)
        {
            try
            {
                using (MemoryStream ms = new MemoryStream(bytes))
                    Image.FromStream(ms);
            }
            catch (ArgumentException)
            {
                return false;
            }
            return true;
        }

        public static byte[] ReduceImageSize(byte[] inputBytes, int quality)
        {
            if (quality == 0)
            {
                quality = 80;
            }
            if (inputBytes.Length > 0)
            {
                inputBytes = ResizeImage(inputBytes);
                Image image;
                using (var inputStream = new MemoryStream(inputBytes))
                {
                    image = Image.FromStream(inputStream);
                    var jpegEncoder = ImageCodecInfo.GetImageDecoders().First(c => c.FormatID == ImageFormat.Jpeg.Guid);
                    var encoderParameters = new EncoderParameters(1);
                    encoderParameters.Param[0] = new EncoderParameter(Encoder.Quality, quality);
                    Byte[] outputBytes;
                    using (var outputStream = new MemoryStream())
                    {
                        image.Save(outputStream, jpegEncoder, encoderParameters);
                        outputBytes = outputStream.ToArray();
                    }
                    return outputBytes;
                }
            }
            return inputBytes;
        }

        public static byte[] ResizeImage(byte[] inputBytes)
        {
            byte[] outputBytes = null;
            using (var ms = new MemoryStream(inputBytes))
            {
                using (var img = Image.FromStream(ms))
                {
                    if ((img.Width > img.Height) && (img.Width > MAX_WIDTH))
                    {
                        using (var newImg = BitmapResize(img, new Size(MAX_WIDTH, (MAX_WIDTH * img.Height) / img.Width)))
                        {
                            using (var m = new MemoryStream())
                            {
                                newImg.Save(m, ImageFormat.Jpeg);
                                outputBytes = m.ToArray();
                            }
                        }
                    }
                    else if ((img.Width < img.Height) && (img.Height > MAX_HEIGHT))
                    {
                        using (var newImg = BitmapResize(img, new Size((MAX_HEIGHT * img.Width) / img.Height, MAX_HEIGHT)))
                        {
                            using (var m = new MemoryStream())
                            {
                                newImg.Save(m, ImageFormat.Jpeg);
                                outputBytes = m.ToArray();
                            }
                        }
                    }
                    else
                    {
                        using (var m = new MemoryStream())
                        {
                            img.Save(m, ImageFormat.Jpeg);
                            outputBytes = m.ToArray();
                        }
                    }
                }
            }

            return outputBytes;
        }

        internal static Image BitmapResize(Image img, Size size)
        {
            return (Image)(new Bitmap(img, size));
        }
    }

    public static class FileManager
    {
        private static readonly object padlock = new object();

        public static byte[] ReadFile(string filePath, string fileName)
        {
            string p = Path.Combine(System.Configuration.ConfigurationManager.AppSettings["GetLocalPathFile"], filePath, fileName);
            return File.ReadAllBytes(p);
        }

        public static void SaveFile(string filePath, string fileName, byte[] data)
        {


            //var dir = Path.Combine(System.Configuration.ConfigurationManager.AppSettings["GetLocalPathFile"], filePath);
            var dir = filePath;
            if (!Directory.Exists(filePath))
            {
                lock (padlock)
                {
                    if (!Directory.Exists(dir))
                    {
                        Directory.CreateDirectory(dir);
                    }
                }
            }

            var stream = new MemoryStream(data);
            stream.Position = 0;

            using (FileStream file = new FileStream(Path.Combine(dir, fileName), FileMode.Create, System.IO.FileAccess.Write))
            {
                stream.CopyTo(file);
            }
        }

        public static void DeleteFile(string filePath, string fileName)
        {
            string file = Path.Combine(System.Configuration.ConfigurationManager.AppSettings["GetLocalPathFile"], filePath, fileName);
            if (File.Exists(file))
            {
                File.Delete(file);
            }
        }
    }

}