using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using MugStore.Common;
using MugStore.Services.Common;
using MugStore.Services.Data;
using SixLabors.ImageSharp;
using System;
using System.Globalization;
using System.IO;
using System.Linq;

namespace MugStore.Web.Controllers
{
    public class ImageController : BaseController
    {
        private readonly IImagesService images;
        private readonly IHostingEnvironment hostingEnvironment;
        private readonly ILoggerService loggerService;

        public ImageController(IImagesService images, IHostingEnvironment hostingEnvironment, ILoggerService loggerService)
        {
            this.images = images;
            this.hostingEnvironment = hostingEnvironment;
            this.loggerService = loggerService;
        }

        public IActionResult Index(string name)
        {
            var image = this.images.Get(name);
            if (image == null)
            {
                return NotFound(name);
            }

            return DownloadImage(image.ContentType, Path.Combine(GlobalConstants.PathToUploadImages, image.Path), image.Name);
        }

        public IActionResult ProductImage(string name)
        {
            var image = this.images.GetProductImage(name);
            if (image == null)
            {
                return NotFound(name);
            }

            this.HttpContext.Response.Headers.Add("Cache-Control", "public,max-age=31536000");
            this.HttpContext.Response.Headers.Add("Expires", DateTime.UtcNow.AddYears(1).ToString("R", CultureInfo.InvariantCulture));
            return DownloadImage(image.ContentType, Path.Combine(GlobalConstants.PathToGalleryImages, image.Path), image.Name);
        }

        [RequestSizeLimit(8_000_000)]
        public IActionResult Upload()
        {
            try
            {
                var imagesPath = Path.Combine(this.hostingEnvironment.ContentRootPath, GlobalConstants.PathToUploadImages.FixOsPath());
                var datePath = string.Format(@"{0}\{1}\".FixOsPath(), DateTime.Today.Year, DateTime.Today.Month);
                var path = Path.Combine(imagesPath, datePath);
                int width, height = default(int);
                double dpi = default(double);

                var file = this.Request.Form?.Files.FirstOrDefault();

                if (file == null || file.Length == 0)
                {
                    throw new InvalidDataException("Upload 1 file");
                }

                var type = file.ContentType.Split('/');
                var fileName = Guid.NewGuid().ToString();

                using (var sharpImage = Image.Load(file.OpenReadStream()))
                {
                    width = sharpImage.Width;
                    height = sharpImage.Height;
                    dpi = sharpImage.MetaData.HorizontalResolution; // This value is not always correct !!!
                }

                if (width == default(int) || height == default(int))
                {
                    throw new InvalidDataException("Image width or height is 0");
                }

                if (dpi == default(double))
                {
                    throw new InvalidDataException("Image dpi is 0");
                }

                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }

                using (var stream = new FileStream(path + fileName + "." + type[1], FileMode.Create))
                {
                    file.CopyTo(stream);
                }

                var image = new Data.Models.Image()
                {
                    Name = fileName,
                    OriginalName = file.FileName,
                    ContentType = file.ContentType,
                    Path = datePath,
                    Width = width,
                    Height = height,
                    Dpi = (int)Math.Ceiling(dpi)
                };

                this.images.Add(image);

                var result = new
                {
                    success = true,
                    filename = image.Name,
                    width = image.Width,
                    height = image.Height,
                    dpi = image.Dpi
                };

                return this.Json(result);
            }
            catch (Exception ex)
            {
                var result = new
                {
                    success = false,
                    message = ex.Message
                };

                this.loggerService.Log(Data.Models.LogLevel.Error, ex.Message, "500");

                return this.Json(result);
            }
        }

        private IActionResult DownloadImage(string contentType, string imagePath, string imageName)
        {
            var type = contentType.Split('/');
            var path = Path.Combine(this.hostingEnvironment.ContentRootPath, imagePath.FixOsPath(), imageName.FixOsPath() + "." + type[1]);

            var memory = new MemoryStream();
            using (var stream = new FileStream(path, FileMode.Open))
            {
                stream.CopyTo(memory);
            }

            memory.Position = 0;
            return this.File(memory, contentType);
        }
    }
}
