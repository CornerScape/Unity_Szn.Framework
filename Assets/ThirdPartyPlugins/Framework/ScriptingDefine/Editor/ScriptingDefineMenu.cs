using System.Text;
using SznFramework.Editor.Menu;
using UnityEditor;
using UnityEngine;

namespace SznFramework.Editor.ScriptingDefine
{
    public static class ScriptingDefineMenu
    {
        private const string RUNTIME_MODE_ITEM_NAME_S = "Runtime Mode/";
        private const int RUNTIME_MODE_PRIORITY_I = (int)MenuPriority.RuntimeMode;

        [MenuItem(MenuTools.MENU_ROOT_NAME + RUNTIME_MODE_ITEM_NAME_S + "Empty",
            priority = RUNTIME_MODE_PRIORITY_I)]
        public static void SwitchEmptyMode()
        {
            SwitchMode(ScriptingDefineConfig.EMPTY_MODE_NAME_S);
        }

        [MenuItem(MenuTools.MENU_ROOT_NAME + RUNTIME_MODE_ITEM_NAME_S + "Test",
            priority = RUNTIME_MODE_PRIORITY_I )]
        public static void SwitchTestMode()
        {
            SwitchMode(ScriptingDefineConfig.TEST_MODE_NAME_S);
        }

        [MenuItem(MenuTools.MENU_ROOT_NAME + RUNTIME_MODE_ITEM_NAME_S + "Debug",
            priority = RUNTIME_MODE_PRIORITY_I + 1)]
        public static void SwitchDebugMode()
        {
            SwitchMode(ScriptingDefineConfig.DEBUG_MODE_NAME_S);
        }

        [MenuItem(MenuTools.MENU_ROOT_NAME + RUNTIME_MODE_ITEM_NAME_S + "Release",
            priority = RUNTIME_MODE_PRIORITY_I + 2)]
        public static void SwitchReleaseMode()
        {
            SwitchMode(ScriptingDefineConfig.RELEASE_MODE_NAME_S);
        }

        [MenuItem(MenuTools.MENU_ROOT_NAME + RUNTIME_MODE_ITEM_NAME_S + "Other",
            priority = RUNTIME_MODE_PRIORITY_I + 100)]
        public static void OpenScriptingDefineWindow()
        {
            ScriptingDefineWindow.Init();
        }

        private static void SwitchMode(string InModeName)
        {
            string defineGroupStr;
            if (string.IsNullOrEmpty(defineGroupStr = EditorPrefs.GetString(ScriptingDefineConfig.DEFINE_GROUP_PREFS_KEY_S, string.Empty)))
            {
                switch (InModeName)
                {
                    case ScriptingDefineConfig.EMPTY_MODE_NAME_S:
                        ScriptingDefineTools.SetScriptingDefineSymbols(string.Empty);
                        break;

                    case ScriptingDefineConfig.TEST_MODE_NAME_S:
                        ScriptingDefineTools.SetScriptingDefineSymbols(ScriptingDefineConfig.TEST_MACRO_DEFINE_S);
                        break;

                    case ScriptingDefineConfig.DEBUG_MODE_NAME_S:
                        ScriptingDefineTools.SetScriptingDefineSymbols(ScriptingDefineConfig.DEBUG_MACRO_DEFINE_S);
                        break;

                    case ScriptingDefineConfig.RELEASE_MODE_NAME_S:
                        ScriptingDefineTools.SetScriptingDefineSymbols(string.Empty);
                        break;
                }
            }
            else
            {
                string[] allDefineItem = defineGroupStr.Split(',');
                int len = allDefineItem.Length - 1;
                for (int i = 0; i < len; i++)
                {
                    if (allDefineItem[i].StartsWith(InModeName))
                    {
                        string[] defineItems = allDefineItem[i].Split('@')[1].Split('|');
                        len = defineItems.Length;
                        if(len == 0) ScriptingDefineTools.SetScriptingDefineSymbols(string.Empty);
                        else
                        {
                            StringBuilder sb = new StringBuilder(len * 16);
                            for (int j = 0; j < len; j++)
                            {
                                sb.Append(defineItems[j].Split('&')[0]).Append(",");
                            }
                            ScriptingDefineTools.SetScriptingDefineSymbols(sb.ToString());
                        }

                        break;
                    }
                }
            }
        }
    }
}