﻿using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Restaurantly_backend.Helpers;

namespace Restaurantly_backend.Helpers
{
    public static class Extension
    {
        //Check File Size
        public static bool CheckFileSize(this IFormFile file, int kb)
        {
            return file.Length / 1024 <= kb;
        }
        //Check File Type
        public static bool CheckFileType(this IFormFile file, string type)
        {
            return file.ContentType.Contains(type);
        }
        //Save File
        public async static Task<string> SaveFileAsync(this IFormFile file, string root,params string[] folders)
        {
            var fileName = Guid.NewGuid().ToString() + file.FileName;
            var path = Path.Combine(Helper.GetPath(root, folders), fileName);
            using(FileStream fileStream = new FileStream(path, FileMode.Create))
            {
                await file.CopyToAsync(fileStream);
            }
            return fileName;
        }
    }
}
