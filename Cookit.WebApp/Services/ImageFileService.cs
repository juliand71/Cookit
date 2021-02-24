using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using TinifyAPI;

namespace Cookit.WebApp.Services
{
    public class ImageFileService
    {
        private readonly IWebHostEnvironment _env;
        private readonly string _defaultDir = "D:\\CookitRecipeImages\\";
        public ImageFileService(IWebHostEnvironment env)
        {
            _env = env;
        }

        public string GetNewFileName(string fileName, string suffix = null)
        {
            string fileType = Path.GetExtension(fileName);
            string newFileNameNoExtension = Path.GetFileNameWithoutExtension(Path.GetRandomFileName());

            if (suffix != null)
            {
                newFileNameNoExtension = newFileNameNoExtension + suffix;
            }

            return newFileNameNoExtension + fileType;
        }

        public string GetFullPath(string imgFileName)
        {
            return Path.Combine(_defaultDir, imgFileName);
        }

        public bool IsValidFileType(string fileName)
        {
            string fileType = Path.GetExtension(fileName);
            if (fileType != ".jpg" && fileType != ".png")
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        public void SaveImageFile(IFormFile formFile, string newFileName)
        {         
            var fullImagePath = GetFullPath(newFileName);
            var fileStream = new FileStream(fullImagePath, FileMode.Create, FileAccess.Write);

            formFile.CopyToAsync(fileStream);
        }

        public void OptimizeImageFile(string fileName)
        {
            Tinify.Key = "h2Ht4N5B4gNDm8rwDjlBhJC6T1hd8MY5";

            string fullFilePath = GetFullPath(fileName);
            var source = Tinify.FromFile(fullFilePath);
            var resized = source.Resize(new
            {
                method = "fit",
                width = 400,
                height = 300
            });

            resized.ToFile(fullFilePath);
        }
    }
}
