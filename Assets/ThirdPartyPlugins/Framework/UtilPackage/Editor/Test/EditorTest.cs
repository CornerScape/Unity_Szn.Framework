#if UNIT_TEST && UNITY_EDITOR
using SznFramework.UtilPackage.Editor;
using UnityEditor;
using UnityEngine;

public class EditorTest : MonoBehaviour
{
    [MenuItem("Framework/Test/Company Name")]
    public static void TestCompanyName()
    {
        Dialog.Info(EditorTools.GetCompanyName());
    }

    [MenuItem("Framework/Test/Open Assets Floder")]
    public static void TestOpenAssetsFloder()
    {
        Dialog.Info(EditorTools.OpenAssetsFolder("Open"));
    }

    [MenuItem("Framework/Test/Save Assets Floder")]
    public static void TestSaveAssetsFloder()
    {
        Dialog.Info(EditorTools.SaveAssetsFolder("Save"));
    }

    [MenuItem("Framework/Test/Open Assets File")]
    public static void TestOpenAssetsFile()
    {
        Dialog.Info(EditorTools.OpenAssetsFile("Open"));
    }

    [MenuItem("Framework/Test/Save Assets File")]
    public static void TestSaveAssetsFile()
    {
        Dialog.Info(EditorTools.SaveAssetsFile("Save", Application.dataPath, "Plugin", ".meat"));
    }
}

#endif