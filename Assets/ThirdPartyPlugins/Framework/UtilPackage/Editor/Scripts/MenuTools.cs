using SznFramework.Editor.ScriptingDefine;
using UnityEditor;
using UnityEngine;

namespace SznFramework.Editor.Menu
{
    public enum MenuPriority
    {
        LocalData = 50,
        OpenFile = 100,
        RuntimeMode = 200,
        DataBase = 300,
        Test = 1000
    }

    public static class MenuTools
    {
        public const  string MENU_ROOT_NAME = "Framework/";
        #region Local data
        [MenuItem(MENU_ROOT_NAME +"Local Player Data/Clear PlayerPrefs", priority = (int)MenuPriority.LocalData)]
        static void ClearPlayPrefs()
        {
            PlayerPrefs.DeleteAll();
        }

        //[MenuItem(MENU_ROOT_NAME + "Local Player Data/Clear EditorPrefs", priority = (int)MenuPriority.LocalData)]
        //static void ClearEditorPrefs()
        //{
        //    EditorPrefs.DeleteKey(ScriptingDefineConfig.ALL_DEFINE_PREFS_KEY_S);
        //    EditorPrefs.DeleteKey(ScriptingDefineConfig.DEFINE_GROUP_PREFS_KEY_S);
        //}
        #endregion


        #region Open folder
        private const string OPEN_FILE_MENU_NAME_S = MENU_ROOT_NAME + "Open File/";

        [MenuItem(OPEN_FILE_MENU_NAME_S  + "PersistentData Folder", priority = (int)MenuPriority.OpenFile)]
        static void OpenPersistentData()
        {
            System.Diagnostics.Process.Start(Application.persistentDataPath);
        }

        [MenuItem(OPEN_FILE_MENU_NAME_S + "Assets Folder", priority = (int)MenuPriority.OpenFile + 1)]
        static void OpenAssets()
        {
            System.Diagnostics.Process.Start(Application.dataPath);
        }

        [MenuItem(OPEN_FILE_MENU_NAME_S  +"StreamingAssets Folder", priority = (int)MenuPriority.OpenFile + 2)]
        static void OpenStreamingAssets()
        {
            System.Diagnostics.Process.Start(Application.streamingAssetsPath);
        }
        #endregion
    }
}