using System.Collections.Generic;
using System.IO;

namespace Szn.Framework.UtilPackage
{
    public static class FileTools
    {
        public static List<FileInfo> GetAllFileInfos(string InRootDirectory)
        {
            if (string.IsNullOrEmpty(InRootDirectory)) return null;
            DirectoryInfo directoryInfo = new DirectoryInfo(InRootDirectory);
            List<FileInfo> fileInfoList = new List<FileInfo>();
            fileInfoList.AddRange(directoryInfo.GetFiles());
            DirectoryInfo[] directoryInfos = directoryInfo.GetDirectories();
            int len = directoryInfos.Length;
            for (int i = 0; i < len; i++)
            {
                fileInfoList.AddRange(GetAllFileInfos(directoryInfos[i]));
            }

            return fileInfoList;
        }

        public static List<FileInfo> GetAllFileInfos(DirectoryInfo InRootDirectory)
        {
            List<FileInfo> fileInfoList = new List<FileInfo>();
            fileInfoList.AddRange(InRootDirectory.GetFiles());
            DirectoryInfo[] directoryInfos = InRootDirectory.GetDirectories();
            int len = directoryInfos.Length;
            for (int i = 0; i < len; i++)
            {
                fileInfoList.AddRange(GetAllFileInfos(directoryInfos[i]));
            }

            return fileInfoList;
        }
    }
}