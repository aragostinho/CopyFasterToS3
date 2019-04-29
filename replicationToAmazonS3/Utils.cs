using replicationToAmazonS3.Core;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace System
{
    public static class Utils
    {
        public static string RemoveLastSlash(this string text)
        {
            if (text.EndsWith(@"\"))
                return text.Remove(text.Length, 1);

            return text;
        }

        public static string RemoveLastFolder(this string text)
        {
            text = text.RemoveLastSlash();

            if (text.IndexOf(@"\") == -1)
                return string.Empty;

            var fullPathWithoutLastFolder = text.Substring(0, text.LastIndexOf(@"\") + 1);

            return fullPathWithoutLastFolder;
        }
        public static string RemoveRootUnit(this string text)
        {
            if (text.Contains(@":\"))
                return text.Substring(2, text.Length - 2);

            return text;
        }


        public static string ToFullS3KeyName(this string keyname, string keyNameDestination)
        {
            if (keyname.IsNullOrEmpty())
                return string.Empty;

            if (keyNameDestination.IsNullOrEmpty())
                return keyname;

            return $"{keyNameDestination}/{keyname}";

        }

        public static string ToS3KeyName(this string text)
        {
            string returnText = string.Empty;

            if (text.StartsWith(@"\"))
                returnText = text.Substring(1, text.Length - 1);

            returnText = returnText.Replace(@"\", "/");

            return returnText;
        }

        public static bool IsNullOrEmpty(this string pString)
        {
            return string.IsNullOrEmpty(pString);
        }

        public static int ToInt(this string pString)
        {
            return int.Parse(pString);
        }

        public static bool ToBoolean(this string text)
        {
            if (text.IsNullOrEmpty())
                return false;

            bool parsedToBool = false;
            var result = bool.TryParse(text, out parsedToBool);

            if (parsedToBool == false)
                return false;

            return result;
        }

        public static string[] ToFolderList(this string text)
        {
            if (text.IsNullOrEmpty())
                return null;

            if (text.IndexOf(',') == -1)
                return null;

            return text.Split(',');
        }


        public static void CatchErrorLog(Exception exception, string currentFile = null, string currentKeyName = null)
        {
            var folderLogs = $@"{AppDomain.CurrentDomain.BaseDirectory}\logs";

            if (!Directory.Exists(folderLogs))
                Directory.CreateDirectory(folderLogs);

            var log = File.CreateText($@"{folderLogs}\{Guid.NewGuid().ToString()}.txt");

            if (!currentFile.IsNullOrEmpty() && !currentKeyName.IsNullOrEmpty())
            {
                log.WriteLine($"FILE OBJECT *****************************");
                log.WriteLine($"FilePath: {currentFile.ToString()}");
                log.WriteLine($"FilePath: {currentKeyName.ToString()}");
            } 

            log.WriteLine($"ERROR DETAILS *****************************");
            log.WriteLine($"Date: {DateTime.Now.ToString()}");
            log.WriteLine($"Error message: {exception?.Message}");
            log.WriteLine($"InnerException: {exception?.InnerException}");
            log.WriteLine($"Source: {exception?.Source}");
            log.WriteLine($"TargetSite: {exception?.TargetSite}");
            log.WriteLine($"StackTrace: {exception?.StackTrace}");
            log.Close();

        }

        public static void CatchErrorLog(ErrorSaveObjectResult objectResult, string currentFile = null, string currentKeyName = null)
        {
            var folderLogs = $@"{AppDomain.CurrentDomain.BaseDirectory}\logs";

            if (!Directory.Exists(folderLogs))
                Directory.CreateDirectory(folderLogs);

            var log = File.CreateText($@"{folderLogs}\{Guid.NewGuid().ToString()}.txt");

            if (!currentFile.IsNullOrEmpty() && !currentKeyName.IsNullOrEmpty())
            {
                log.WriteLine($"FILE OBJECT *****************************");
                log.WriteLine($"FilePath: {currentFile.ToString()}");
                log.WriteLine($"FilePath: {currentKeyName.ToString()}");
            }

            log.WriteLine($"ERROR DETAILS *****************************");
            log.WriteLine($"Date: {DateTime.Now.ToString()}");
            log.WriteLine($"Error message: {objectResult?.Message}");
            log.WriteLine($"InnerException: {objectResult.Exception?.InnerException}");
            log.WriteLine($"Source: {objectResult.Exception?.Source}");
            log.WriteLine($"TargetSite: {objectResult.Exception?.TargetSite}");
            log.WriteLine($"StackTrace: {objectResult.Exception?.StackTrace}");
            log.Close();

        }

    }
}
