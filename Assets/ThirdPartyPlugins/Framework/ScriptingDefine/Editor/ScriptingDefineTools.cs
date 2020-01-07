using UnityEditor;

namespace Szn.Framework.Editor.ScriptingDefine
{
    public static class ScriptingDefineTools
    {
        public static string GetScriptingDefineSymbols()
        {
            BuildTargetGroup buildTargetGroup = BuildTargetGroup.Standalone;
            switch (EditorUserBuildSettings.activeBuildTarget)
            {
                case BuildTarget.Android:
                    buildTargetGroup = BuildTargetGroup.Android;
                    break;

                case BuildTarget.iOS:
                    buildTargetGroup = BuildTargetGroup.iOS;
                    break;
            }

            return PlayerSettings.GetScriptingDefineSymbolsForGroup(buildTargetGroup);
        }

        public static void SetScriptingDefineSymbols(string InScriptingDefineSymbols)
        {
            BuildTargetGroup buildTargetGroup = BuildTargetGroup.Standalone;
            switch (EditorUserBuildSettings.activeBuildTarget)
            {
                case BuildTarget.Android:
                    buildTargetGroup = BuildTargetGroup.Android;
                    break;

                case BuildTarget.iOS:
                    buildTargetGroup = BuildTargetGroup.iOS;
                    break;
            }

            //EditorUserBuildSettings.activeScriptCompilationDefines
            PlayerSettings.SetScriptingDefineSymbolsForGroup(buildTargetGroup, InScriptingDefineSymbols);
        }
    }
}