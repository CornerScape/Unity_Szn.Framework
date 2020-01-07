#if UNITY_EDITOR
using UnityEditor;

namespace Szn.Framework.UtilPackage.Editor
{
    public static class Dialog
    {
        public static void Info(string InMsg)
        {
            EditorUtility.DisplayDialog("Info", InMsg, "Ok");
        }

        public static void Warning(string InMsg)
        {
            EditorUtility.DisplayDialog("Warning", InMsg, "Ok");
        }

        public static void Error(string InMsg)
        {
            EditorUtility.DisplayDialog("Error", InMsg, "Ok");
        }
    }
}
#endif