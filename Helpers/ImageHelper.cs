using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Cook_Book_API.Interfaces;

namespace Cook_Book_API.Helpers
{
    public class ImageHelper : IImageHelper
    {
        private readonly IConfiguration _config;
        private readonly IHostEnvironment _hostEnvironment;
        private readonly ILogger _logger;

        private readonly string[] acceptedExtensions = { ".jpg",".jpeg",".png","gif"};

        public ImageHelper(IConfiguration config, IHostEnvironment hostEnvironment, ILogger<ImageHelper> logger)
        {
            _config = config;
            _hostEnvironment = hostEnvironment;
            _logger = logger;
        }

        public string GetImagePath(string imageName)
        {
            string output = "";

            try
            {
                var imagesPhotoPath = _config["ImagePath"];
                var rootFolderPath = _hostEnvironment.ContentRootPath + "\\wwwroot";
                var relativePath = imagesPhotoPath + imageName;
                var path = rootFolderPath + relativePath;
                output = path;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Got exception.");
            }
            return output;
        }

        public bool CheckCorrectExtension(string extension)
        {
            bool output = false; 
            try
            {
                if (acceptedExtensions.Any(extension.Contains))
                {
                    output = true;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Got exception.");
            }

            return output;
        }
    }
}
