using UnityEngine;
using UnityEditor;
using System.IO;

namespace SznFramework
{
    public class ReporterEditor : Editor
    {
        [MenuItem("Reporter/Create")]
        public static void CreateReporter()
        {
            const int reporterExecOrder = -12000;
            GameObject reporterObj = new GameObject("Reporter");
            Reporter reporter = reporterObj.AddComponent<Reporter>();

            // Register root object for undo.
            Undo.RegisterCreatedObjectUndo(reporterObj, "Create Reporter Object");

            MonoScript reporterScript = MonoScript.FromMonoBehaviour(reporter);
            string reporterPath = Path.GetDirectoryName(AssetDatabase.GetAssetPath(reporterScript));

            if (MonoImporter.GetExecutionOrder(reporterScript) != reporterExecOrder)
            {
                MonoImporter.SetExecutionOrder(reporterScript, reporterExecOrder);
                //Debug.Log("Fixing exec order for " + reporterScript.name);
            }

            reporter.ReporterImages = new ReporterImages
            {
                CustomerImage = (Texture2D)AssetDatabase.LoadAssetAtPath(Path.Combine(reporterPath, "ReporterImages/customer.png"),
                    typeof(Texture2D)),
                ClearImage =
                    (Texture2D)AssetDatabase.LoadAssetAtPath(Path.Combine(reporterPath, "ReporterImages/clear.png"),
                        typeof(Texture2D)),
                CollapseImage = (Texture2D)AssetDatabase.LoadAssetAtPath(Path.Combine(reporterPath, "ReporterImages/collapse.png"),
                    typeof(Texture2D)),
                ClearOnNewSceneImage =
                    (Texture2D)AssetDatabase.LoadAssetAtPath(Path.Combine(reporterPath, "ReporterImages/clearOnSceneLoaded.png"),
                        typeof(Texture2D)),
                ShowTimeImage = (Texture2D)AssetDatabase.LoadAssetAtPath(Path.Combine(reporterPath, "ReporterImages/timer_1.png"),
                    typeof(Texture2D)),
                ShowSceneImage =
                    (Texture2D)AssetDatabase.LoadAssetAtPath(Path.Combine(reporterPath, "ReporterImages/UnityIcon.png"),
                        typeof(Texture2D)),
                UserImage = (Texture2D)AssetDatabase.LoadAssetAtPath(Path.Combine(reporterPath, "ReporterImages/user.png"),
                    typeof(Texture2D)),
                ShowMemoryImage = (Texture2D)AssetDatabase.LoadAssetAtPath(Path.Combine(reporterPath, "ReporterImages/memory.png"),
                    typeof(Texture2D)),
                SoftwareImage = (Texture2D)AssetDatabase.LoadAssetAtPath(Path.Combine(reporterPath, "ReporterImages/software.png"),
                    typeof(Texture2D)),
                DateImage = (Texture2D)AssetDatabase.LoadAssetAtPath(Path.Combine(reporterPath, "ReporterImages/date.png"),
                    typeof(Texture2D)),
                ShowFpsImage =
                    (Texture2D)AssetDatabase.LoadAssetAtPath(Path.Combine(reporterPath, "ReporterImages/fps.png"),
                        typeof(Texture2D)),
                InfoImage = (Texture2D)AssetDatabase.LoadAssetAtPath(Path.Combine(reporterPath, "ReporterImages/info.png"),
                    typeof(Texture2D)),
                SearchImage = (Texture2D)AssetDatabase.LoadAssetAtPath(Path.Combine(reporterPath, "ReporterImages/search.png"),
                    typeof(Texture2D)),
                CloseImage =
                    (Texture2D)AssetDatabase.LoadAssetAtPath(Path.Combine(reporterPath, "ReporterImages/close.png"),
                        typeof(Texture2D)),
                BuildFromImage =
                    (Texture2D)AssetDatabase.LoadAssetAtPath(Path.Combine(reporterPath, "ReporterImages/buildFrom.png"),
                        typeof(Texture2D)),
                SystemInfoImage =
                    (Texture2D)AssetDatabase.LoadAssetAtPath(Path.Combine(reporterPath, "ReporterImages/ComputerIcon.png"),
                        typeof(Texture2D)),
                GraphicsInfoImage =
                    (Texture2D)AssetDatabase.LoadAssetAtPath(Path.Combine(reporterPath, "ReporterImages/graphicCard.png"),
                        typeof(Texture2D)),
                BackImage = (Texture2D)AssetDatabase.LoadAssetAtPath(Path.Combine(reporterPath, "ReporterImages/back.png"),
                    typeof(Texture2D)),
                LogImage = (Texture2D)AssetDatabase.LoadAssetAtPath(Path.Combine(reporterPath, "ReporterImages/log_icon.png"),
                    typeof(Texture2D)),
                WarningImage =
                    (Texture2D)AssetDatabase.LoadAssetAtPath(Path.Combine(reporterPath, "ReporterImages/warning_icon.png"),
                        typeof(Texture2D)),
                ErrorImage = (Texture2D)AssetDatabase.LoadAssetAtPath(Path.Combine(reporterPath, "ReporterImages/error_icon.png"),
                    typeof(Texture2D)),
                BarImage = (Texture2D)AssetDatabase.LoadAssetAtPath(Path.Combine(reporterPath, "ReporterImages/bar.png"),
                    typeof(Texture2D)),
                ButtonActiveImage =
                    (Texture2D)AssetDatabase.LoadAssetAtPath(Path.Combine(reporterPath, "ReporterImages/button_active.png"),
                        typeof(Texture2D)),
                EvenLogImage = (Texture2D)AssetDatabase.LoadAssetAtPath(Path.Combine(reporterPath, "ReporterImages/even_log.png"),
                    typeof(Texture2D)),
                OddLogImage = (Texture2D)AssetDatabase.LoadAssetAtPath(Path.Combine(reporterPath, "ReporterImages/odd_log.png"),
                    typeof(Texture2D)),
                SelectedImage = (Texture2D)AssetDatabase.LoadAssetAtPath(Path.Combine(reporterPath, "ReporterImages/selected.png"),
                    typeof(Texture2D)),
                ReporterScrollSkin =
                    (GUISkin)AssetDatabase.LoadAssetAtPath(
                        Path.Combine(reporterPath, "ReporterImages/reporterScrollerSkin.guiskin"), typeof(GUISkin))
            };
        }
    }
}