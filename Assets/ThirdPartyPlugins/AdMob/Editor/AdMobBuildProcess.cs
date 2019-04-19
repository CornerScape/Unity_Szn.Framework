using System.IO;
using System.Xml;
using UnityEditor;
using UnityEditor.Build;
using UnityEngine;

namespace SznFramework.AdMob
{
    public class AdMobBuildProcess : IPreprocessBuild, IPostprocessBuild
    {
        private const string ANDROID_MANIFEST_FILE_PATH =
            "Plugins/Android/GoogleMobileAdsPlugin/AndroidManifest.xml";

        public int callbackOrder
        {
            get { return 0; }
        }

        public void OnPreprocessBuild(BuildTarget InTarget, string InPath)
        {
            if (InTarget == BuildTarget.Android)
            {
                string androidManifestPath = Path.Combine(Application.dataPath, ANDROID_MANIFEST_FILE_PATH);
                if (File.Exists(androidManifestPath))
                {
                    XmlDocument androidManifestDoc = new XmlDocument();
                    androidManifestDoc.Load(androidManifestPath);
                    XmlNode node = androidManifestDoc.SelectSingleNode("manifest");
                    if (null == node)
                    {
                        Debug.LogError("AndroidManifest.xml can not be empty.");
                    }
                    else
                    {
                        XmlNode usesSdkNode = node.SelectSingleNode("uses-sdk");
                        if (null != usesSdkNode && null != usesSdkNode.Attributes)
                        {
                            usesSdkNode.Attributes["android:minSdkVersion"].Value = "16";
                            usesSdkNode.Attributes["android:targetSdkVersion"].Value = "26";
                        }
                        else
                        {
                            Debug.LogWarning("Node uses-sdk Not Found.");
                        }

                        XmlNode metaDataNode = node.SelectSingleNode("application/meta-data");
                        if (null != metaDataNode && null != metaDataNode.Attributes)
                        {
                            metaDataNode.Attributes["android:value"].Value = AdMobConfig.AD_MOB_APP_ID;
                        }
                        else
                        {
                            Debug.LogWarning("Node application/meta-data Not Found.");
                        }
                        // 保存
                        androidManifestDoc.Save(androidManifestPath);
                        AssetDatabase.Refresh();
                    }
                }
                else
                {
                    Debug.LogError("AdMob AndroidManifest.xml does not exist with the '"
                                   + ANDROID_MANIFEST_FILE_PATH
                                   + "' directory, please modify the location. ");
                }
            }
        }

        public void OnPostprocessBuild(BuildTarget InTarget, string InPath)
        {
            if (InTarget == BuildTarget.iOS)
            {
#if UNITY_IOS
                string plistPath = Path.Combine(InPath, "Info.plist");
                UnityEditor.iOS.Xcode.PlistDocument plist = new UnityEditor.iOS.Xcode.PlistDocument();

                plist.ReadFromFile(plistPath);
                plist.root.SetString("GADApplicationIdentifier", AdMobConfig.AD_MOB_APP_ID);
                File.WriteAllText(plistPath, plist.WriteToString());
#endif
            }
        }
    }
}