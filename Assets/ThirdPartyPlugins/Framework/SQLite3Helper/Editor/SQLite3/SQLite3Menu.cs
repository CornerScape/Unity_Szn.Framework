﻿using SznFramework.Editor.Menu;
using UnityEditor;

namespace SznFramework.Editor.SQLite3Creator
{
    public static class SQLite3Menu
    {
        [MenuItem(MenuTools.MENU_ROOT_NAME + "Database/SQLite3 Window %&z", priority = (int)MenuPriority.DataBase)]
        static void OpenPersistentData()
        {
            SQLite3Window.Init();
        }
    }
}