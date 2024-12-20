﻿using AllupMVC.Utilities.Enums;
using Microsoft.AspNetCore.Http;
using System.Text.RegularExpressions;

namespace AllupMVC.Utilities.Extensions
{
    public static class FileValidator
    {
        public static string CheckName(this string name)
        {
            if (name.Any(char.IsDigit))
            {
                return null;
            }
            name = name.Trim();
            return char.ToUpper(name[0]) + name.Substring(1).ToLower(); ;
        }

        public static bool CheckEmail(this string email)
        {
            Regex regex = new Regex(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$");
            Match match = regex.Match(email);
            if (match.Success)
            {
                return true;
            }
            return false;

        }
        public static string FilePath(this string fileName,params string[]roots)
        {
            string path=string.Empty;
            for(int i = 0; i < roots.Length; i++)
            {
                path=Path.Combine(path,roots[i]);
            }
            path=Path.Combine(path,fileName);
            return path;
        }

        public static bool ValidateType(this IFormFile file,string type)
        {
            if(file.ContentType.Contains(type))
            {
                return true;
            }
            return false;
        }

        public static bool ValidateSize(this IFormFile file,FileSize filesize,int size)
        {
          switch(filesize)
            {
                case FileSize.KB:
                    return file.Length <= size * 1024;
                case FileSize.MB:
                    return file.Length <= size * 1024 * 1024;
                case FileSize.GB:
                    return file.Length <= size * 1024 * 1024 * 1024;
            }
            return false;
        }

        public static async Task<string> CreateFileAsync(this IFormFile file,params string[]roots)
        {
            string fileName = string.Concat(Guid.NewGuid().ToString(), file.FileName.Substring(file.FileName.LastIndexOf('.')));
            string path = fileName.FilePath(roots);

            using (FileStream filestream = new(path, FileMode.Create))
            {
                await file.CopyToAsync(filestream);
            }
            return fileName;
        }

        public static void DeleteFile(this string fileName,params string[]roots)
        {
            string path=fileName.FilePath(roots);
            if(File.Exists(path))
            {
                File.Delete(path);
            }
        }
    }
}
