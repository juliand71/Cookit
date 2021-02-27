using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Hosting;
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

        private string _defaultDir;
        public ImageFileService(IWebHostEnvironment env)
        {
            _env = env;

            if (_env.IsDevelopment())
            {
                _defaultDir = "D:\\CookitRecipeImages\\";
            }
            else
            {
                _defaultDir = "E:\\CookitRecipeImages\\";
            }
        }

        // helper method to create a new random file name
        // it is not considered secure to use the same file name that a user provides when uploading
        // it's also probably not secure to not do any kind of scan on the file, but that's a problem
        // for another day
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

        /**
         * Simple Helper function to append the file name to the end of the default image directory
         */
        public string GetFullPath(string imgFileName)
        {
            return Path.Combine(_defaultDir, imgFileName);
        }

        /**
         * Helper method to check if a user provided file is a valid image file
         * Currently just checks for jpg and png, may add other file types in the future
         */
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

        /**
         * Here's where the actual work happens
         * Copies the data from the uploaded file to a location with a new file name
         */
        public void SaveImageFile(IFormFile formFile, string newFileName)
        {         
            var fullImagePath = GetFullPath(newFileName);
            var fileStream = new FileStream(fullImagePath, FileMode.Create, FileAccess.Write);

            formFile.CopyToAsync(fileStream);
        }

        /**
         * Use the TinyPNG / TinyJPG API to optimize the image file
         * 
         * This is currently broken, and not used anywhere. Probably because I am missing something
         * with all the async / awaits going on with the Tinify Library and the OnPost method also
         * being async
         */
        //public void OptimizeImageFile(string fileName)
        //{
        //    Tinify.Key = "h2Ht4N5B4gNDm8rwDjlBhJC6T1hd8MY5";

        //    string fullFilePath = GetFullPath(fileName);
        //    var source = Tinify.FromFile(fullFilePath);
        //    var resized = source.Resize(new
        //    {
        //        method = "fit",
        //        width = 400,
        //        height = 300
        //    });

        //    resized.ToFile(fullFilePath);
        //}
    }
}
