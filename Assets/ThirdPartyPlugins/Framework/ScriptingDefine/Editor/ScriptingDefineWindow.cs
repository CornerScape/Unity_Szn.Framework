using System.Collections.Generic;
using System.Text;
using SznFramework.UtilPackage.Editor;
using UnityEditor;
using UnityEngine;

namespace SznFramework.Editor.ScriptingDefine
{
    public class ScriptingDefineWindow : EditorWindow
    {
        private static EditorWindow scriptingDefineWindow;
        
        public static void Init()
        {
            scriptingDefineWindow = CreateInstance<ScriptingDefineWindow>();
            //window.LoadExcel(Application.dataPath);
            scriptingDefineWindow.titleContent = new GUIContent("Runtime Mode", "Change Scripting GroupName Symbols.");
            scriptingDefineWindow.minSize = new Vector2(555, 600);
            scriptingDefineWindow.maxSize = new Vector2(555, 2000);
            scriptingDefineWindow.ShowUtility();
        }

        private class MacroDefine
        {
            public string Define;
            public bool IsSelected;
            public readonly bool IsCustomer;

            public MacroDefine(string InDefine, bool InIsCustomer)
            {
                Define = InDefine;
                IsSelected = false;
                IsCustomer = InIsCustomer;
            }

            public static MacroDefine Convert(string InStr)
            {
                string[] item = InStr.Split('&');
                return new MacroDefine(item[0], bool.Parse(item[2])) { IsSelected = bool.Parse(item[1]) };
            }

            public override string ToString()
            {
                return string.Format("{0}&{1}&{2}", Define, IsSelected, IsCustomer);
            }
        }

        private class MacroDefineGroup
        {
            public string GroupName;
            public readonly List<MacroDefine> DefineGroup;
            public readonly bool IsCustomer, CanEdit;

            public MacroDefineGroup(string InGroupName)
            {
                GroupName = InGroupName;
                DefineGroup = new List<MacroDefine>(16);
                IsCustomer = true;
                CanEdit = true;
            }

            private MacroDefineGroup(string InGroupName, bool InIsCustomer, bool InCanEdit)
            {
                GroupName = InGroupName;
                DefineGroup = new List<MacroDefine>(16);
                IsCustomer = InIsCustomer;
                CanEdit = InCanEdit;
            }

            public MacroDefineGroup(string InGroupName, List<MacroDefine> InDefineGroup, bool InIsCustomer, bool InCanEdit)
            {
                GroupName = InGroupName;
                DefineGroup = InDefineGroup;
                IsCustomer = InIsCustomer;
                CanEdit = InCanEdit;
            }

            public string GetDefineGroup()
            {
                if (null != DefineGroup)
                {
                    int count = DefineGroup.Count;
                    if (0 != count)
                    {
                        StringBuilder sb = new StringBuilder(count * 16);
                        sb.Append(DefineGroup[0].Define);
                        for (int i = 1; i < count; i++)
                        {
                            sb.Append(", ").Append(DefineGroup[i].Define);
                        }

                        return sb.ToString();
                    }
                }

                return string.Empty;
            }

            public static MacroDefineGroup Convert(string InStr)
            {
                string[] item = InStr.Split('@');

                if (string.IsNullOrEmpty(item[1])) return new MacroDefineGroup(item[0], bool.Parse(item[2]), bool.Parse(item[3]));
                else
                {
                    string[] defines = item[1].Split('|');
                    int len = defines.Length;
                    List<MacroDefine> defineGroup = new List<MacroDefine>(len > 32 ? len : 32);
                    for (int i = 0; i < len; i++)
                    {
                        defineGroup.Add(MacroDefine.Convert(defines[i]));
                    }
                    return new MacroDefineGroup(item[0], defineGroup, bool.Parse(item[2]), bool.Parse(item[3]));
                }
            }

            public override string ToString()
            {
                int count = null == DefineGroup ? 0 : DefineGroup.Count;
                StringBuilder sb = new StringBuilder(count * 16 + 32);
                sb.Append(GroupName)
                    .Append("@");
                if (0 != count)
                {
                    // ReSharper disable once PossibleNullReferenceException
                    sb.Append(DefineGroup[0]);
                    for (int i = 1; i < count; i++)
                    {
                        sb.Append("|").Append(DefineGroup[i]);
                    }
                }

                sb.Append("@")
                    .Append(IsCustomer)
                    .Append("@")
                    .Append(CanEdit);

                return sb.ToString();
            }
        }

        private Dictionary<int, MacroDefineGroup> defineGroup;
        private List<MacroDefine> allDefine;
        private string currentDefineGroupName;
        private bool isGroupEditing, isDefineEditing;

        private GUIContent headContent, editHeadContent;
        private GUIStyle headStyle;
        private GUIStyle defineLabelStyle;
        private GUIStyle previewStyle;

        private void OnEnable()
        {
            Load();
            GetCurrentDefineGroupName();

            headContent = new GUIContent("Scripting Define Symbols Group", "Locally saved scripting define symbols.");
            editHeadContent = new GUIContent("Edit Scripting Define Symbols", "Edit selected scripting define symbols.");
            headStyle = new GUIStyle
            {
                fontStyle = FontStyle.Bold,
                fontSize = 20,
                normal = { textColor = Color.white },
                alignment = TextAnchor.MiddleCenter
            };

            defineLabelStyle = new GUIStyle
            {
                normal = { textColor = new Color(0.8f, 0.8f, 0.8f) },
                alignment = TextAnchor.LowerCenter,
                contentOffset = new Vector2(0, 4)
            };

            previewStyle = new GUIStyle
            {
                normal = { textColor = new Color(0.8f, 0.8f, 0.8f) },
                alignment = TextAnchor.LowerLeft,
                contentOffset = new Vector2(0, 6)
            };

            //string dirPath = Path.GetDirectoryName(AssetDatabase.GetAssetPath(MonoScript.FromScriptableObject(this)));
            //if (!string.IsNullOrEmpty(dirPath))
            //{
            //    scriptingDefineGroupSkin =
            //        AssetDatabase.LoadAssetAtPath<GUISkin>(Path.Combine(dirPath, "DefaultSkin.guiskin"));
            //    scriptingDefineGroupSkin.label.alignment = TextAnchor.MiddleCenter;
            //    scriptingDefineGroupSkin.textField.alignment = TextAnchor.MiddleCenter;
            //}
        }

        private int removeKey = -1;
        private int editorKey = -1;
        private bool needEditInit = true;
        private readonly List<int> addedIndexes = new List<int>(16);
        private void OnGUI()
        {
            GUILayout.BeginVertical("box");
            GUI.skin.textField.alignment = TextAnchor.LowerCenter;

            #region Scripting Define Group
            if (-1 == editorKey)
            {
                GUILayout.Label(headContent, headStyle);
                
                int index = 0;
                foreach (KeyValuePair<int, MacroDefineGroup> itor in defineGroup)
                {
                    GUILayout.BeginVertical("Box");
                    GUILayout.BeginHorizontal();
                    GUILayout.Label(index.ToString(), GUILayout.Width(30));
                    if (itor.Value.IsCustomer)
                        itor.Value.GroupName = GUILayout.TextField(itor.Value.GroupName, GUILayout.Width(300));
                    else GUILayout.Label(itor.Value.GroupName, defineLabelStyle, GUILayout.Width(300));

                    GUI.enabled = currentDefineGroupName != itor.Value.GroupName;
                    if (GUILayout.Button("Select", GUILayout.Width(60)))
                    {
                        if (isGroupEditing) Dialog.Error("Please finish editing first.");
                        else
                        {
                            ScriptingDefineTools.SetScriptingDefineSymbols(itor.Value.GetDefineGroup());
                            scriptingDefineWindow.Close();
                        }
                    }

                    GUI.enabled = itor.Value.CanEdit;

                    if (GUILayout.Button("Edit", GUILayout.Width(60)))
                    {
                        if (isGroupEditing) Dialog.Error("Please finish editing first.");
                        else
                        {
                            needEditInit = true;
                            editorKey = itor.Key;
                        }
                    }

                    GUI.enabled = itor.Value.IsCustomer;
                    if (GUILayout.Button("Remove", GUILayout.Width(60)))
                    {
                        if (isGroupEditing) Dialog.Error("Please finish editing first.");
                        else removeKey = itor.Key;
                    }

                    GUI.enabled = true;
                    GUILayout.EndHorizontal();
                    GUILayout.Space(4);
                    GUILayout.BeginHorizontal();
                    GUILayout.Label("Preview:", previewStyle, GUILayout.Width(56));
                    GUILayout.BeginHorizontal("box");
                    GUILayout.Label(itor.Value.GetDefineGroup());
                    GUILayout.EndHorizontal();
                    GUILayout.EndHorizontal();
                    GUILayout.Space(4);
                    GUILayout.EndVertical();
                    ++index;
                }

                if (isGroupEditing)
                {
                    GUILayout.BeginHorizontal();
                    if (GUILayout.Button("Back", GUILayout.Width(260)))
                    {
                        int count = addedIndexes.Count;
                        for (int i = 0; i < count; i++)
                        {
                            defineGroup.Remove(addedIndexes[i]);
                        }
                        addedIndexes.Clear();
                        isGroupEditing = false;
                    }

                    if (GUILayout.Button("Save", GUILayout.Width(260)))
                    {
                        if (CheckDefineGroup())
                        {
                            Save();
                            addedIndexes.Clear();
                            isGroupEditing = false;
                        }
                    }
                    GUILayout.EndVertical();
                }
                else
                {
                    GUILayout.BeginVertical();
                    if (GUILayout.Button("Add"))
                    {
                        isGroupEditing = true;
                        int count = defineGroup.Count;
                        defineGroup.Add(count, new MacroDefineGroup("Customize Model"));
                        addedIndexes.Add(count);
                    }

                    GUILayout.Space(280);
                    if (GUILayout.Button("Reset To Default"))
                    {
                        if (EditorUtility.DisplayDialog("Tips",
                            "Confirm that you want to delete the locally saved macro definition?", "Ok", "Cancel"))
                        {
                            EditorPrefs.DeleteKey(ScriptingDefineConfig.ALL_DEFINE_PREFS_KEY_S);
                            EditorPrefs.DeleteKey(ScriptingDefineConfig.DEFINE_GROUP_PREFS_KEY_S);

                            Load();
                        }
                    }
                    GUILayout.EndVertical();
                }
            }
            #endregion

            #region Remove Scripting Define Group
            if (-1 != removeKey)
            {
                Debug.LogError(removeKey);
                int count = defineGroup.Count - 1;
                if (removeKey != count)
                {
                    for (int i = removeKey; i < count; i++)
                    {
                        defineGroup[i] = defineGroup[i + 1];
                    }
                }

                defineGroup.Remove(count);
                removeKey = -1;
            }
            #endregion

            #region Editor Scripting Define Group
            if (-1 != editorKey)
            {
                GUILayout.Label(editHeadContent, headStyle);
                GUILayout.BeginVertical("box");

                GUILayout.BeginHorizontal("box");
                GUILayout.Label("Scripting Define Group Name:", GUILayout.Width(180));
                GUI.enabled = false;
                GUILayout.TextField(defineGroup[editorKey].GroupName, GUILayout.Width(360));
                GUI.enabled = true;
                GUILayout.EndHorizontal();

                List<MacroDefine> selectedDefines = defineGroup[editorKey].DefineGroup;
                int selectedCount = selectedDefines.Count;

                int count = allDefine.Count;

                for (int i = 0; i < count; i++)
                {
                    GUILayout.BeginHorizontal("box");
                    GUILayout.Label(i.ToString(), GUILayout.Width(30));
                    allDefine[i].Define = GUILayout.TextField(allDefine[i].Define, GUILayout.Width(300));

                    if (needEditInit)
                    {
                        allDefine[i].IsSelected = false;
                        if (0 != selectedCount)
                        {
                            for (int j = 0; j < selectedCount; j++)
                            {
                                if (allDefine[i].Define == selectedDefines[j].Define)
                                {
                                    allDefine[i].IsSelected = true;
                                    break;
                                }
                            }
                        }
                    }

                    GUI.enabled = allDefine[i].IsCustomer;
                    allDefine[i].IsSelected = GUILayout.Toggle(allDefine[i].IsSelected, "Select", GUILayout.Width(60));

                    GUI.enabled = allDefine[i].IsCustomer;
                    if (GUILayout.Button("Remove", GUILayout.Width(60)))
                    {
                        bool canRemove = true;
                        string currentDefine = allDefine[i].Define;
                        StringBuilder sb = new StringBuilder(defineGroup.Count * 16);
                        foreach (KeyValuePair<int, MacroDefineGroup> itor in defineGroup)
                        {
                            foreach (MacroDefine macroDefine in itor.Value.DefineGroup)
                            {
                                if (macroDefine.Define == currentDefine)
                                {
                                    sb.Append("<").Append(itor.Value.GroupName).Append(">");
                                    canRemove = false;
                                    break;
                                }
                            }
                        }

                        if (canRemove)
                        {
                            allDefine.RemoveAt(i);
                        }
                        else
                        {
                            Dialog.Error("The select to be deleted is being used by \n" + sb + "\nso it cannot be deleted.");
                        }
                    }
                    GUI.enabled = true;

                    GUI.enabled = i != 0;
                    if (GUILayout.Button("^", GUILayout.Width(30)))
                    {
                        MacroDefine temp = allDefine[i];
                        allDefine[i] = allDefine[i - 1];
                        allDefine[i - 1] = temp;
                    }

                    GUI.enabled = i != count - 1;
                    if (GUILayout.Button("v", GUILayout.Width(30)))
                    {
                        MacroDefine temp = allDefine[i];
                        allDefine[i] = allDefine[i + 1];
                        allDefine[i + 1] = temp;
                    }

                    GUI.enabled = true;
                    GUILayout.EndHorizontal();
                }

                if (isDefineEditing)
                {
                    GUILayout.BeginHorizontal();
                    if (GUILayout.Button("Back", GUILayout.Width(260)))
                    {
                        allDefine.RemoveAt(allDefine.Count - 1);
                        isDefineEditing = false;
                    }

                    if (GUILayout.Button("Save", GUILayout.Width(260)))
                    {
                        if (CheckAllDefine())
                        {
                            isDefineEditing = false;
                            Save();
                            if (currentDefineGroupName == defineGroup[editorKey].GroupName) GetCurrentDefineGroupName();
                        }
                    }
                    GUILayout.EndVertical();
                }
                else
                {
                    GUILayout.BeginHorizontal();
                    if (GUILayout.Button("Back", GUILayout.Width(174)))
                    {
                        if (CheckAllDefine())
                        {
                            isDefineEditing = false;
                            editorKey = -1;
                        }
                    }

                    if (GUILayout.Button("Save", GUILayout.Width(174)))
                    {
                        if (CheckAllDefine())
                        {
                            SaveDefineGroup(editorKey);
                            Save();
                            isDefineEditing = false;
                            editorKey = -1;
                        }
                    }

                    if (GUILayout.Button("Add", GUILayout.Width(174)))
                    {
                        isDefineEditing = true;
                        allDefine.Add(new MacroDefine(string.Empty, true));
                    }
                    GUILayout.EndHorizontal();
                }

                needEditInit = false;
            }

            #endregion


            GUILayout.EndVertical();
        }

        private bool CheckAllDefine()
        {
            int count = allDefine.Count;
            for (int i = 0; i < count; i++)
            {
                if (string.IsNullOrEmpty(allDefine[i].Define))
                {
                    Dialog.Error("Scripting Define Symbol Can Not be Empty.");
                    return false;
                }
            }

            count = allDefine.Count;
            for (int i = 0; i < count; i++)
            {
                for (int j = i + 1; j < count; j++)
                {
                    if (allDefine[i].Define == allDefine[j].Define)
                    {
                        Dialog.Error("Scripting Define Symbols Can Not Repeat. \nPlease check the values in the " + i + " and " + j);
                        return false;
                    }
                }
            }
            return true;
        }

        private bool CheckDefineGroup()
        {
            foreach (KeyValuePair<int, MacroDefineGroup> itor in defineGroup)
            {
                if (string.IsNullOrEmpty(itor.Value.GroupName))
                {
                    Dialog.Error("Macro Define Group Name Can not be empty.");
                    return false;
                }

                int time = 0;
                foreach (KeyValuePair<int, MacroDefineGroup> pair in defineGroup)
                {
                    if (itor.Value.GroupName == pair.Value.GroupName) ++time;
                }

                if (1 != time)
                {
                    Dialog.Error("Macro Define Group Name Can not repeat.");
                    return false;
                }
            }

            return true;
        }

        private void SaveDefineGroup(int InEditorKey)
        {
            MacroDefineGroup group = defineGroup[InEditorKey];
            group.DefineGroup.Clear();
            int count = allDefine.Count;
            for (int i = 0; i < count; i++)
            {
                if (allDefine[i].IsSelected) group.DefineGroup.Add(allDefine[i]);
            }

            defineGroup[InEditorKey] = group;
        }

        private void Save()
        {
            int count = allDefine.Count;
            StringBuilder sb = new StringBuilder(count * 48);
            for (int i = 0; i < count; i++)
            {
                sb.Append(allDefine[i]).Append(",");
            }
            EditorPrefs.SetString(ScriptingDefineConfig.ALL_DEFINE_PREFS_KEY_S, sb.ToString());

            count = defineGroup.Count;
            sb = new StringBuilder(count * 64);
            foreach (KeyValuePair<int, MacroDefineGroup> itor in defineGroup)
            {
                sb.Append(itor.Value).Append(",");
            }
            EditorPrefs.SetString(ScriptingDefineConfig.DEFINE_GROUP_PREFS_KEY_S, sb.ToString());
        }

        private void Load()
        {
            string allDefineStr;
            if (string.IsNullOrEmpty(allDefineStr = EditorPrefs.GetString(ScriptingDefineConfig.ALL_DEFINE_PREFS_KEY_S, string.Empty)))
            {
                //ADS; AD_DEBUG; UNIT_TEST
                allDefine = new List<MacroDefine>(32)
                {
                    new MacroDefine(ScriptingDefineConfig.TEST_MACRO_DEFINE_S, false),
                    new MacroDefine(ScriptingDefineConfig.DEBUG_MACRO_DEFINE_S, false)
                };

                defineGroup = new Dictionary<int, MacroDefineGroup>(16)
                {
                    {
                        0,
                        new MacroDefineGroup(ScriptingDefineConfig.EMPTY_MODE_NAME_S,
                            null,
                            false,
                            false)
                    },
                    {
                        1,
                        new MacroDefineGroup(ScriptingDefineConfig.TEST_MODE_NAME_S,
                            new List<MacroDefine>(16){allDefine[0]},
                            false,
                            true)
                    },
                    {
                        2,
                        new MacroDefineGroup(ScriptingDefineConfig.DEBUG_MODE_NAME_S,
                            new List<MacroDefine>(16){allDefine[1]},
                            false,
                            true)
                    },
                    {
                        3,
                        new MacroDefineGroup(ScriptingDefineConfig.RELEASE_MODE_NAME_S,
                            new List<MacroDefine>(16),
                            false,
                            true)
                    }
                };

                Save();
            }
            else
            {
                string[] allDefineItem = allDefineStr.Split(',');
                int len = allDefineItem.Length - 1;
                allDefine = new List<MacroDefine>(len > 32 ? len : 32);
                for (int i = 0; i < len; i++)
                {
                    allDefine.Add(MacroDefine.Convert(allDefineItem[i]));
                }

                string[] defineGroupItem = EditorPrefs.GetString(ScriptingDefineConfig.DEFINE_GROUP_PREFS_KEY_S).Split(',');
                len = defineGroupItem.Length - 1;
                defineGroup = new Dictionary<int, MacroDefineGroup>(len > 16 ? len : 16);
                for (int i = 0; i < len; i++)
                {
                    defineGroup.Add(i, MacroDefineGroup.Convert(defineGroupItem[i]));
                }
            }
        }

        private void GetCurrentDefineGroupName()
        {
            currentDefineGroupName = string.Empty;
            string currentDefineSymbols = ScriptingDefineTools.GetScriptingDefineSymbols();

            foreach (KeyValuePair<int, MacroDefineGroup> itor in defineGroup)
            {
                if (itor.Value.GetDefineGroup() == currentDefineSymbols)
                {
                    currentDefineGroupName = itor.Value.GroupName;
                    break;
                }
            }
        }
    }
}

