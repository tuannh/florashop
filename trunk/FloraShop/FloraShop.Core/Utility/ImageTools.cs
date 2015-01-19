using FloraShop.Core.Logs;
using System;
using System.Linq;
using System.Collections;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Runtime.InteropServices;
using System.Drawing.Drawing2D;
using FloraShop.Core.Extensions;

namespace FloraShop.Core.Utility
{
    public static class ImageTools
    {
        #region Support methods

        /// <summary>
        /// Determines whether [is image extension] [the specified extension]. Ex: .jpg, .bmp
        /// </summary>
        /// <param name="extension">The extension.</param>
        /// <returns>
        ///   <c>true</c> if [is image extension] [the specified extension]; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsImageExtension(string extension)
        {
            switch (extension.ToLower())
            {
                case ".jpg":
                case ".jpeg":
                case ".gif":
                case ".png":
                case ".bmp":
                    return true;
            }
            return false;
        }

        /// <summary>
        /// Gets the image extension.
        /// </summary>
        /// <param name="format">The format.</param>
        /// <returns></returns>
        public static string GetImageExtension(ImageFormat format)
        {
            return ImageCodecInfo.GetImageEncoders()
                                 .FirstOrDefault(x => x.FormatID == format.Guid)
                                 .FilenameExtension;
        }

        /// <summary>
        /// Converts to image format. Ex: .jpg, .png
        /// </summary>
        /// <param name="extension">The extension.</param>
        /// <returns></returns>
        public static ImageFormat ConvertToImageFormat(string extension)
        {
            switch (extension.ToLower())
            {
                case ".jpg":
                case ".jpeg":
                    return ImageFormat.Jpeg;
                case ".gif":
                    return ImageFormat.Gif;
                case ".png":
                    return ImageFormat.Png;
                case ".bmp":
                default:
                    return ImageFormat.Bmp;
            }
        }

        /// <summary>
        /// Validates the image.
        /// </summary>
        /// <param name="filePath">The file path.</param>
        /// <returns></returns>
        public static bool ValidateImage(string filePath)
        {
            try
            {
                var sourceImage = System.Drawing.Image.FromFile(filePath);
                sourceImage.Dispose();
                return true;
            }
            catch
            {
                // This is not a valid image. Do nothing.
                return false;
            }
        }

        /// <summary>
        /// Gets the JPG codec.
        /// </summary>
        /// <returns></returns>
        private static ImageCodecInfo GetJpgCodec()
        {
            ImageCodecInfo[] aCodecs = ImageCodecInfo.GetImageEncoders();
            ImageCodecInfo oCodec = null;

            for (int i = 0; i < aCodecs.Length; i++)
            {
                if (aCodecs[i].MimeType.Equals("image/jpeg"))
                {
                    oCodec = aCodecs[i];
                    break;
                }
            }

            return oCodec;
        }

        /// <summary>
        /// Gets the codec by extension.
        /// </summary>
        /// <param name="fileExt">The file ext. Ex: .jpg, .png</param>
        /// <returns></returns>
        public static ImageCodecInfo GetCodecByExt(string fileExt)
        {
            fileExt = fileExt.ToLower();
            var encoders = ImageCodecInfo.GetImageEncoders();

            for (var i = 0; i < encoders.Length; i++)
            {
                if (encoders[i].FilenameExtension.ToLower().Contains(fileExt))
                    return encoders[i];
            }
            return GetJpgCodec();
        }

        #endregion

        #region CropImage

        /// <summary>
        /// Resizes the image.
        /// </summary>
        /// <param name="sourceFile">The source file.</param>
        /// <param name="targetFile">The target file.</param>
        /// <param name="maxWidth">Width of the max.</param>
        /// <param name="maxHeight">Height of the max.</param>
        /// <param name="preserverAspectRatio">if set to <c>true</c> [preserver aspect ratio].</param>
        /// <param name="quality">The quality.</param>
        /// <returns></returns>
        /// <summary>
        /// Crops the image.
        /// </summary>
        /// <param name="sourceFilePath">The source file path.</param>
        /// <param name="targetFilePath">The target file path.</param>
        /// <param name="x">The x.</param>
        /// <param name="y">The y.</param>
        /// <param name="width">The width.</param>
        /// <param name="height">The height.</param>
        /// <returns></returns>
        public static bool CropImage(string sourceFilePath, string targetFilePath, int x, int y, int width, int height)
        {
            var extension = Path.GetExtension(sourceFilePath);
            var imageFormat = ConvertToImageFormat(extension);
            using (FileStream fs = new FileStream(sourceFilePath, FileMode.Open, FileAccess.Read, FileShare.Read))
            {
                using (FileStream target = new FileStream(targetFilePath, FileMode.OpenOrCreate, FileAccess.Write, FileShare.None))
                {
                    CropImage(fs, target, imageFormat, x, y, width, height);
                }
            }

            return true;
        }

        /// <summary>
        /// Crops the image.
        /// </summary>
        /// <param name="sourceStream">The source stream.</param>
        /// <param name="targetStream">The target stream.</param>
        /// <param name="imageFormat">The image format.</param>
        /// <param name="x">The x.</param>
        /// <param name="y">The y.</param>
        /// <param name="width">The width.</param>
        /// <param name="height">The height.</param>
        /// <returns></returns>
        public static bool CropImage(Stream sourceStream, Stream targetStream, ImageFormat imageFormat, int x, int y, int width, int height)
        {
            Rectangle r = new Rectangle(x, y, width, height);

            using (Bitmap src = Image.FromStream(sourceStream) as Bitmap)
            {
                Bitmap target = new Bitmap(width, height);


                using (Graphics g = Graphics.FromImage(target))
                {
                    g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBilinear;
                    g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;

                    g.DrawImage(src, new Rectangle(0, 0, target.Width, target.Height), r, GraphicsUnit.Pixel);
                }

                target.Save(targetStream, imageFormat);

            }

            return true;
        }

        #endregion

        #region CreateThumbnail

        /// Creates the thumbnail to fix and nice image
        /// </summary>
        /// <param name="sourceFile">The source file.</param>
        /// <param name="targetFile">The target file.</param>
        /// <param name="thumbWith">The thumb with.</param>
        /// <param name="thumbHeight">Height of the thumb.</param>
        /// <param name="bgColor">Color of the bg.</param>
        /// <param name="quality">The quality.</param>
        /// <returns></returns>
        public static bool CreateThumbnail(string sourceFile, string targetFile, int thumbWidth, int thumbHeight, Color bgColor, int quality = 80)
        {
            try
            {
                using (var srcImg = System.Drawing.Image.FromFile(sourceFile))
                {
                    // Calculate the new width and height
                    int width = srcImg.Width;
                    int height = srcImg.Height;

                    var pasteX = 0;
                    var pasteY = 0;

                    var newWidth = width;
                    var newHeight = height;

                    var tmpWidth = thumbWidth;
                    float tmpHeight = (float)(height * thumbWidth) / width;
                    var intTmpHeight = (int)tmpHeight;

                    // thumb is ratio scale with src image
                    if (tmpHeight == (float)intTmpHeight && intTmpHeight == thumbHeight)
                    {
                        newWidth = thumbWidth;
                        newHeight = thumbHeight;
                    }
                    // width ratio great than height ratio. Height is fix scale
                    else if (width > thumbWidth && height > thumbHeight)
                    {
                        var ratio = (float)width / (float)height;

                        if (ratio >= 1) // fix height
                        {
                            newHeight = thumbHeight;
                            newWidth = (int)Math.Floor(newHeight * ratio);

                            // recheck new with
                            if (newWidth < thumbWidth)
                            {
                                newWidth = thumbWidth;
                                newHeight = (int)Math.Floor(newWidth / ratio);
                                pasteY = (thumbHeight - newHeight) / 2;
                            }
                            else
                            {
                                pasteX = (thumbWidth - newWidth) / 2;
                            }
                        }
                        else
                        {
                            newWidth = thumbWidth;
                            newHeight = (int)Math.Floor(newWidth / ratio);

                            if (newHeight < thumbHeight)
                            {
                                newHeight = thumbHeight;
                                newWidth = (int)Math.Floor(newHeight * ratio);
                                pasteX = (thumbWidth - newWidth) / 2;
                            }
                            else
                                pasteY = (thumbHeight - newHeight) / 2;
                        }
                    }
                    else
                    {
                        pasteX = (thumbWidth - newWidth) / 2;
                        pasteY = (thumbHeight - newHeight) / 2;
                    }

                    #region make dest image

                    using (var destImage = new System.Drawing.Bitmap(thumbWidth, thumbHeight))
                    {
                        using (var graphic = Graphics.FromImage(destImage))
                        {
                            graphic.FillRectangle(new SolidBrush(bgColor), new Rectangle(0, 0, thumbWidth, thumbHeight));
                            //graphic.PixelOffsetMode = PixelOffsetMode.Half;
                            //graphic.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic; /* new way */

                            graphic.DrawImage(srcImg, pasteX, pasteY, newWidth, newHeight);

                            var ext = Path.GetExtension(targetFile);
                            var codec = GetCodecByExt(ext);

                            var eps = new EncoderParameters(1);
                            eps.Param[0] = new EncoderParameter(Encoder.Quality, quality);

                            destImage.Save(targetFile, codec, eps);

                            graphic.Dispose();
                            destImage.Dispose();
                            srcImg.Dispose();
                        }
                    }

                    #endregion

                }
            }
            catch (Exception exp)
            {
                exp.Log();
                return false;
            }

            return true;
        }

        /// <summary>
        /// Creates the thumbnail with white background and quality is 80%
        /// </summary>
        /// <param name="sourceFile">The source file.</param>
        /// <param name="targetFile">The target file.</param>
        /// <param name="thumbWidth">The thumb with.</param>
        /// <param name="thumbHeight">Height of the thumb.</param>
        /// <returns></returns>
        public static bool CreateThumbnail(string sourceFile, string targetFile, int thumbWidth, int thumbHeight)
        {
            return CreateThumbnail(sourceFile, targetFile, thumbWidth, thumbHeight, Color.White, 80);
        }

        /// <summary>
        /// Creates the thumbnail to fix and nice image
        /// </summary>
        /// <param name="sourceFile">The source file.</param>
        /// <param name="thumbWidth">Width of the thumb.</param>
        /// <param name="thumbHeight">Height of the thumb.</param>
        /// <param name="bgColor">Color of the bg.</param>
        /// <param name="quality">The quality.</param>
        /// <returns></returns>
        public static Bitmap CreateThumbnail(string sourceFile, int thumbWidth, int thumbHeight, Color bgColor, int quality = 80)
        {
            if (!string.IsNullOrEmpty(sourceFile))
            {
                var tmpPath = string.Format(@"{0}\{1}{2}", Path.GetDirectoryName(sourceFile), Guid.NewGuid(), Path.GetFileName(sourceFile));
                if (CreateThumbnail(sourceFile, tmpPath, thumbWidth, thumbHeight, bgColor, quality))
                {
                    using (var image = Image.FromFile(tmpPath))
                    {
                        // var copy = new Bitmap(image);
                        var copy = CopyHelper.DeepCopy<Image>(image) as Bitmap;

                        image.Dispose();

                        try
                        {
                            File.Delete(tmpPath);
                        }
                        catch (Exception exp)
                        {
                            exp.Log();
                        }

                        return copy;
                    }
                }
            }

            return null;
        }

        /// <summary>
        /// Creates the thumbnail to fix and nice image
        /// </summary>
        /// <param name="sourceFile">The source file.</param>
        /// <param name="thumbWidth">Width of the thumb.</param>
        /// <param name="thumbHeight">Height of the thumb.</param>
        /// <returns></returns>
        public static Bitmap CreateThumbnail(string sourceFile, int thumbWidth, int thumbHeight)
        {
            return CreateThumbnail(sourceFile, thumbWidth, thumbHeight, Color.White, 80);
        }

        #endregion

        #region FixResizeImage
        /// <summary>
        /// Fix resize image
        /// </summary>
        /// <param name="sourceFile">The source file.</param>
        /// <param name="targetFile">The target file.</param>
        /// <param name="width">The width.</param>
        /// <param name="height">The height.</param>
        /// <param name="bgColor">Color of the background.</param>
        /// <param name="quality">The quality.</param>
        /// <returns>An image with fix width x height</returns>
        public static bool FixResizeImage(string sourceFile, string targetFile, int width, int height, Color bgColor, int quality = 80)
        {
            try
            {
                using (var srcImg = System.Drawing.Image.FromFile(sourceFile))
                {
                    // Calculate the new width and height
                    int srcWidth = srcImg.Width;
                    int srcHeight = srcImg.Height;

                    var pasteX = 0;
                    var pasteY = 0;

                    var newWidth = srcWidth;
                    var newHeight = srcHeight;

                    var ext = Path.GetExtension(targetFile);
                    var codec = GetCodecByExt(ext);

                    var eps = new EncoderParameters(1);
                    eps.Param[0] = new EncoderParameter(Encoder.Quality, quality);

                    var tmpWidth = width;
                    float tmpHeight = (float)(width * srcHeight) / srcWidth;
                    var intTmpHeight = (int)tmpHeight;

                    // thumb is ratio scale with src image
                    if (tmpHeight == (float)intTmpHeight && intTmpHeight == height)
                    {
                        newWidth = width;
                        newHeight = height;
                    }
                    else if (srcWidth == width && srcHeight == height)
                    {
                        var fileInfo = new FileInfo(sourceFile);
                        var size = fileInfo.Length;

                        if (size / 1024 > 100) // file size > 100Kb
                            srcImg.Save(targetFile, codec, eps);
                        else
                            srcImg.Save(targetFile);

                        srcImg.Dispose();
                        return true;
                    }
                    else if (srcWidth > width || srcHeight > height)
                    {
                        var ratio = (float)srcWidth / srcHeight;
                        var ratioX = (float)srcWidth / width;
                        var ratioY = (float)srcHeight / height;

                        if (ratioX > ratioY)
                        {
                            newWidth = width;
                            newHeight = (int)Math.Floor(newWidth / ratio);
                            pasteY = (height - newHeight) / 2;
                        }
                        else
                        {
                            newHeight = height;
                            newWidth = (int)Math.Floor(newHeight * ratio);
                            pasteX = (width - newWidth) / 2;
                        }
                    }
                    else
                    {
                        if (srcWidth > width)
                        {
                            newWidth = width;
                            pasteY = Math.Abs((newHeight - height) / 2);
                        }
                        else if (srcHeight > height)
                        {
                            pasteX = Math.Abs((newWidth - width) / 2);
                            newHeight = height;
                        }
                        else
                        {
                            pasteX = Math.Abs((newWidth - width) / 2);
                            pasteY = Math.Abs((newHeight - height) / 2);
                        }
                    }

                    #region make dest image

                    using (var destImage = new System.Drawing.Bitmap(width, height))
                    {
                        using (var graphic = Graphics.FromImage(destImage))
                        {
                            graphic.FillRectangle(new SolidBrush(bgColor), new Rectangle(0, 0, width, height));
                            graphic.DrawImage(srcImg, pasteX, pasteY, newWidth, newHeight);

                            destImage.Save(targetFile, codec, eps);

                            graphic.Dispose();
                            destImage.Dispose();
                            srcImg.Dispose();
                        }
                    }

                    #endregion

                }
            }
            catch (Exception exp)
            {
                exp.Log();

                return false;
            }

            return true;
        }

        /// <summary>
        /// Fix resize image with default background is white and default quality is 80%
        /// </summary>
        /// <param name="sourceFile">The source file.</param>
        /// <param name="targetFile">The target file.</param>
        /// <param name="width">The width.</param>
        /// <param name="height">The height.</param>
        /// <returns>An image with fix width x height</returns>
        public static bool FixResizeImage(string sourceFile, string targetFile, int width, int height)
        {
            return FixResizeImage(sourceFile, targetFile, width, height, Color.White, 80);
        }

        /// <summary>
        /// Fixes the resize image.
        /// </summary>
        /// <param name="sourceFile">The source file.</param>
        /// <param name="width">The width.</param>
        /// <param name="height">The height.</param>
        /// <param name="bgColor">Color of the bg.</param>
        /// <param name="quality">The quality.</param>
        /// <returns></returns>
        public static Bitmap FixResizeImage(string sourceFile, int width, int height, Color bgColor, int quality = 80)
        {
            if (!string.IsNullOrEmpty(sourceFile))
            {
                var tmpPath = string.Format(@"{0}\{1}{2}", Path.GetDirectoryName(sourceFile), Guid.NewGuid(), Path.GetFileName(sourceFile));
                if (FixResizeImage(sourceFile, tmpPath, width, height, bgColor, quality))
                {
                    using (var image = Image.FromFile(tmpPath))
                    {
                        // var copy = new Bitmap(image);
                        var copy = CopyHelper.DeepCopy<Image>(image) as Bitmap;

                        image.Dispose();

                        try
                        {
                            File.Delete(tmpPath);
                        }
                        catch (Exception exp)
                        {
                            exp.Log();
                        }

                        return copy;
                    }
                }
            }

            return null;
        }

        /// <summary>
        /// Fix resize image with default background is white and default quality is 80%
        /// </summary>
        /// <param name="sourceFile">The source file.</param>
        /// <param name="width">The width.</param>
        /// <param name="height">The height.</param>
        /// <returns></returns>
        public static Bitmap FixResizeImage(string sourceFile, int width, int height)
        {
            return FixResizeImage(sourceFile, width, height, Color.White, 80);
        }

        #endregion

        #region ResizeImage

        /// <summary>
        /// Resizes the image. 
        /// </summary>
        /// <param name="sourceFile">The source file.</param>
        /// <param name="targetFile">The target file.</param>
        /// <param name="maxWidth">The maximum width.</param>
        /// <param name="maxHeight">The maximum height.</param>
        /// <param name="bgColor">Color of the bg.</param>
        /// <param name="quality">The quality.</param>
        /// <returns>Return an image with width less than or equal maxWidth, height less than or equal maxHeight</returns>
        public static bool ResizeImage(string sourceFile, string targetFile, int maxWidth, int maxHeight, Color bgColor, int quality = 80)
        {
            try
            {
                using (var srcImg = System.Drawing.Image.FromFile(sourceFile))
                {
                    // Calculate the new width and height
                    int srcWidth = srcImg.Width;
                    int srcHeight = srcImg.Height;

                    var newWidth = srcWidth;
                    var newHeight = srcHeight;

                    var ext = Path.GetExtension(targetFile);
                    var codec = GetCodecByExt(ext);

                    var eps = new EncoderParameters(1);
                    eps.Param[0] = new EncoderParameter(Encoder.Quality, quality);

                    if (srcWidth > maxWidth || srcHeight > maxHeight)
                    {
                        var ratio = (float)srcWidth / srcHeight;
                        var ratioX = (float)srcWidth / maxWidth;
                        var ratioY = (float)srcHeight / maxHeight;

                        if (ratioX > ratioY)
                        {
                            newWidth = srcWidth > maxWidth ? maxWidth : srcWidth;
                            newHeight = (int)Math.Floor(newWidth / ratio);
                        }
                        else
                        {
                            newHeight = srcWidth > maxHeight ? maxHeight : srcHeight;
                            newWidth = (int)Math.Floor(newHeight * ratio);
                        }

                        #region make dest image

                        using (var destImage = new System.Drawing.Bitmap(newWidth, newHeight))
                        {
                            using (var graphic = Graphics.FromImage(destImage))
                            {
                                graphic.FillRectangle(new SolidBrush(bgColor), new Rectangle(0, 0, newWidth, newHeight));

                                graphic.DrawImage(srcImg, 0, 0, newWidth, newHeight);

                                destImage.Save(targetFile, codec, eps);

                                graphic.Dispose();
                                destImage.Dispose();
                                srcImg.Dispose();
                            }
                        }

                        #endregion
                    }
                    else
                    {
                        var fileInfo = new FileInfo(sourceFile);
                        var size = fileInfo.Length;

                        if (size / 1024 > 100) // file size > 100Kb
                            srcImg.Save(targetFile, codec, eps);
                        else
                            srcImg.Save(targetFile);

                        srcImg.Dispose();
                    }
                }
            }
            catch (Exception exp)
            {
                exp.Log();

                return false;
            }

            return true;
        }

        /// <summary>
        /// Resize the image with default background is white and default quality is 80%
        /// </summary>
        /// <param name="sourceFile">The source file.</param>
        /// <param name="targetFile">The target file.</param>
        /// <param name="maxWidth">The maximum width.</param>
        /// <param name="maxHeight">The maximum height.</param>
        /// <returns>Return an image with width less than or equal maxWidth, height less than or equal maxHeight</returns>
        public static bool ResizeImage(string sourceFile, string targetFile, int maxWidth, int maxHeight)
        {
            return ResizeImage(sourceFile, targetFile, maxWidth, maxHeight, Color.White, 80);
        }

        /// <summary>
        /// Resizes the image.
        /// </summary>
        /// <param name="sourceFile">The source file.</param>
        /// <param name="targetFile">The target file.</param>
        /// <param name="maxWidth">The maximum width.</param>
        /// <param name="maxHeight">The maximum height.</param>
        /// <param name="bgColor">Color of the bg.</param>
        /// <param name="quality">The quality.</param>
        /// <returns></returns>
        public static Bitmap ResizeImage(string sourceFile, int maxWidth, int maxHeight, Color bgColor, int quality = 80)
        {
            if (!string.IsNullOrEmpty(sourceFile))
            {
                var tmpPath = string.Format(@"{0}\{1}{2}", Path.GetDirectoryName(sourceFile), Guid.NewGuid(), Path.GetFileName(sourceFile));
                if (ResizeImage(sourceFile, tmpPath, maxWidth, maxHeight, bgColor, quality))
                {
                    using (var image = Image.FromFile(tmpPath))
                    {
                        // var copy = new Bitmap(image);
                        var copy = CopyHelper.DeepCopy<Image>(image) as Bitmap;

                        image.Dispose();

                        try
                        {
                            File.Delete(tmpPath);
                        }
                        catch (Exception exp)
                        {
                            exp.Log();
                        }

                        return copy;
                    }
                }
            }

            return null;
        }

        /// <summary>
        /// Resize the image with default background is white and default quality is 80%
        /// </summary>
        /// <param name="sourceFile">The source file.</param>
        /// <param name="maxWidth">The maximum width.</param>
        /// <param name="maxHeight">The maximum height.</param>
        /// <param name="bgColor">Color of the bg.</param>
        /// <param name="quality">The quality.</param>
        /// <returns></returns>
        public static Bitmap ResizeImage(string sourceFile, int maxWidth, int maxHeight)
        {
            return ResizeImage(sourceFile, maxWidth, maxHeight, Color.White, 80);
        }

        #endregion

        #region Save image

        /// <summary>
        /// Saves the image and auto compress if the file >= 100Kb
        /// </summary>
        /// <param name="sourceFile">The source file.</param>
        /// <param name="targetFile">The target file.</param>
        /// <param name="quality">The quality.</param>
        /// <returns></returns>
        public static bool SaveImage(string sourceFile, string targetFile, int quality = 80)
        {
            try
            {
                using (var srcImg = System.Drawing.Image.FromFile(sourceFile))
                {
                    int width = srcImg.Width;
                    int height = srcImg.Height;

                    var ext = Path.GetExtension(targetFile);
                    var codec = ImageTools.GetCodecByExt(ext);

                    var eps = new EncoderParameters(1);
                    eps.Param[0] = new EncoderParameter(Encoder.Quality, quality);

                    var fileInfo = new FileInfo(sourceFile);
                    var size = fileInfo.Length;

                    if (size / 1024 > 100) // file size > 100Kb
                        srcImg.Save(targetFile, codec, eps);
                    else
                        srcImg.Save(targetFile);

                    srcImg.Dispose();

                }
            }
            catch (Exception exp)
            {
                exp.Log();

                return false;
            }

            return true;
        }

        #endregion
    }
}
