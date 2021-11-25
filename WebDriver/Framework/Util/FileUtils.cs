using System.IO;
using WebdriverFramework.Framework.WebDriver;

namespace WebdriverFramework.Framework.Util
{
    /// <summary>
    /// usefull file utils 
    /// </summary>
    public static class FileUtils
    {
        /// <summary>
        /// true Is File Exist
        /// </summary>
        public static bool IsFileExist(string filePath)
        {
            var isFileExist = File.Exists(filePath);
            Logger.Instance.Info($"Checking that the file exists along the path: {filePath}. File visible state: {isFileExist}");
            return isFileExist;
        }

        /// <summary>
        /// delete file
        /// </summary>
        public static void DeleteFile(string filePath)
        {
            Logger.Instance.Info($"Deleting the file along the path: {filePath}");
            File.Delete(filePath);
        }

        /// <summary>
        /// Clean Directory
        /// </summary>
        public static void CleanDirectory(string directory)
        {
            Logger.Instance.Info($"Cleaning the directory along the path: {directory}");
            foreach (var file in Directory.GetFiles(directory))
            {
                DeleteFile(file);
            }
        }
    }
}