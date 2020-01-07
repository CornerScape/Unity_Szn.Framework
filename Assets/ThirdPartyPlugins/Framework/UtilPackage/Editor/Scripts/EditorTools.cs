using UnityEditor;
using UnityEngine;

namespace Szn.Framework.UtilPackage.Editor
{
    public static class EditorTools
    {
        public static string GetCompanyName()
        {
            string companyName = Application.companyName;
            if (companyName == "DefaultCompany")
            {
                string[] dataPath = Application.dataPath.Replace("\\", "/").Split('/');
                int len = dataPath.Length;
                if (len > 2) companyName = dataPath[len - 2];
            }

            return companyName;
        }

        public static string OpenAssetsFolder(string InTitle, string InDefaultPath = null, string InDefaultName = null)
        {
            string dataPath = Application.dataPath;
            if (string.IsNullOrEmpty(InDefaultPath)) InDefaultPath = dataPath;
            string path = EditorUtility.OpenFolderPanel(InTitle, InDefaultPath, InDefaultName);
            if (string.IsNullOrEmpty(path))
                path = null;
            else
            {
                if (path.Contains(dataPath))
                {
                    path = path.Replace(dataPath, "Assets") + "/";
                }
                else
                {
                    Dialog.Error("Can not open the folder out of the project path!");
                    path = null;
                }
            }

            return path;
        }

        public static string SaveAssetsFolder(string InTitle, string InDefaultPath = null, string InDefaultName = null)
        {
            string dataPath = Application.dataPath;
            if (string.IsNullOrEmpty(InDefaultPath)) InDefaultPath = dataPath;
            string path = EditorUtility.SaveFolderPanel(InTitle, InDefaultPath, InDefaultName);
            if (string.IsNullOrEmpty(path))
                path = null;
            else
            {
                if (path.Contains(dataPath))
                {
                    path = path.Replace(dataPath, "Assets") + "/";
                }
                else
                {
                    Dialog.Error("Can not save the folder out of the project path!");
                    path = null;
                }
            }

            return path;
        }

        public static string OpenAssetsFile(string InTitle, string InDirectory = null, string InExtension = null)
        {
            string dataPath = Application.dataPath;
            if (string.IsNullOrEmpty(InDirectory)) InDirectory = dataPath;
            if (!string.IsNullOrEmpty(InExtension) && InExtension.Contains("."))
                InExtension = InExtension.Replace(".", "");
            string path = EditorUtility.OpenFilePanel(InTitle, InDirectory, InExtension);
            if (string.IsNullOrEmpty(path))
                path = null;
            else
            {
                if (path.Contains(dataPath))
                {
                    path = path.Replace(dataPath, "Assets");
                }
                else
                {
                    Dialog.Error("Can not open the file out of the project path!");
                    path = null;
                }
            }

            return path;
        }


        public static string SaveAssetsFile(string InTitle, string InDirectory = null, string InDefaultName = null,
            string InExtension = null)
        {
            string dataPath = Application.dataPath;
            if (string.IsNullOrEmpty(InDirectory)) InDirectory = dataPath;
            if (!string.IsNullOrEmpty(InExtension) && InExtension.Contains("."))
                InExtension = InExtension.Replace(".", "");
            string path = EditorUtility.SaveFilePanel(InTitle, InDirectory, InDefaultName, InExtension);
            if (string.IsNullOrEmpty(path))
                path = null;
            else
            {
                if (path.Contains(dataPath))
                {
                    path = path.Replace(dataPath, "Assets");
                }
                else
                {
                    Dialog.Error("Can not save the file out of the project path!");
                    path = null;
                }
            }

            return path;
        }
    }

}

