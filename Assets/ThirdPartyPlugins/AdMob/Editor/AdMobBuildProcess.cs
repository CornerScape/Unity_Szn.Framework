//using System.Collections;
//using System.Collections.Generic;
//using System.IO;
//using System.Xml;
//using UnityEditor;
//using UnityEditor.Build;
//using UnityEditor.Callbacks;
//using UnityEngine;

//namespace SznFramework.AdMob
//{
//    public class AdMobBuildProcess : IPreprocessBuild, IPostprocessBuild
//    {
//        public int callbackOrder
//        {
//            get { return 0; }
//        }

//        public void OnPreprocessBuild(BuildTarget InTarget, string InPath)
//        {
//            Debug.LogError("OnPreprocessBuild Target = " + InTarget + "- Path = " + InPath);

//            DirectoryInfo rootInfo = new DirectoryInfo(Application.dataPath);

//            string xmlPath = Application.dataPath + "/Plugins/Android/AndroidManifest.xml";
//            XmlDocument xmlDoc = new XmlDocument();
//            xmlDoc.Load(xmlPath);
//            // 包名
//            XmlNode node = xmlDoc.SelectSingleNode("/manifest");
//            node.Attributes["package"].Value = jd["basic"]["app_package_name"].ToString();

//            // scheme
//            node = FindNode(xmlDoc, "/manifest/application/activity/intent-filter/data", "android:name", "appscheme");
//            node.Attributes["android:scheme"].Value = jd["android"]["android_manifest"]["url_schemes_name"].ToString();
//            // 百度-paraches
//            node = FindNode(xmlDoc, "/manifest/application/activity/intent-filter/data", "android:scheme", "paraches");
//            node.Attributes["android:host"].Value = jd["android"]["android_manifest"]["baidu_paraches"].ToString();
//            // 百度-API_KEY
//            node = FindNode(xmlDoc, "/manifest/application/meta-data", "android:name", "com.baidu.lbsapi.API_KEY");
//            node.Attributes["android:value"].Value = jd["android"]["android_manifest"]["baidu_app_key"].ToString();
//            // 云娃-YvImSdkAppId
//            node = FindNode(xmlDoc, "/manifest/application/meta-data", "android:name", "YvImSdkAppId");
//            node.Attributes["android:value"].Value = jd["android"]["android_manifest"]["yunwa_app_id"].ToString();
//            // bugly appid
//            node = FindNode(xmlDoc, "/manifest/application/meta-data", "android:name", "BUGLY_APPID");
//            node.Attributes["android:value"].Value = jd["android"]["android_manifest"]["bugly_app_id"].ToString();

//            // 自定义
//            string xpath = "/manifest/application/meta-data";
//            string key = "android:name";
//            string value = "android:value";
//            node = FindNode(xmlDoc, xpath, key, "wx_app_id");
//            node.Attributes[value].Value = jd["android"]["android_manifest"]["wx_app_id"].ToString();
//            node = FindNode(xmlDoc, xpath, key, "wx_app_secret");
//            node.Attributes[value].Value = jd["android"]["android_manifest"]["wx_app_secret"].ToString();
//            node = FindNode(xmlDoc, xpath, key, "xianliao_app_id");
//            node.Attributes[value].Value = jd["android"]["android_manifest"]["xianliao_app_id"].ToString();
//            node = FindNode(xmlDoc, xpath, key, "yunwa_app_id");
//            node.Attributes[value].Value = jd["android"]["android_manifest"]["yunwa_app_id"].ToString();
//            node = FindNode(xmlDoc, xpath, key, "baidu_app_key");
//            node.Attributes[value].Value = jd["android"]["android_manifest"]["baidu_app_key"].ToString();
//            node = FindNode(xmlDoc, xpath, key, "bugly_app_id");
//            node.Attributes[value].Value = jd["android"]["android_manifest"]["bugly_app_id"].ToString();
//            node = FindNode(xmlDoc, xpath, key, "url_schemes_name");
//            node.Attributes[value].Value = jd["android"]["android_manifest"]["url_schemes_name"].ToString();

//            // 保存
//            xmlDoc.Save(xmlPath);
//            AssetDatabase.Refresh();
//        }

//        public void OnPostprocessBuild(BuildTarget InTarget, string InPath)
//        {
//            Debug.LogError("OnPostprocessBuild Target = " + InTarget + "- Path = " + InPath);
//        }

//        static XmlNode FindNode(XmlDocument xmlDoc, string xpath, string attributeName, string attributeValue)
//        {
//            XmlNodeList nodes = xmlDoc.SelectNodes(xpath);
//            //Debug.Log(nodes.Count);
//            for (int i = 0; i < nodes.Count; i++)
//            {
//                XmlNode node = nodes.Item(i);
//                string _attributeValue = node.Attributes[attributeName].Value;
//                if (_attributeValue == attributeValue)
//                {
//                    return node;
//                }
//            }
//            return null;
//        }
//    }
//}

//using System.Collections;
//using System.Collections.Generic;
//using System.IO;
//using System.Xml;
//using UnityEditor;
//using UnityEditor.Build;
//using UnityEditor.Callbacks;
//using UnityEngine;

//namespace SznFramework.AdMob
//{
//    public class AdMobBuildProcess : IPreprocessBuild, IPostprocessBuild
//    {
//        public int callbackOrder
//        {
//            get { return 0; }
//        }

//        public void OnPreprocessBuild(BuildTarget InTarget, string InPath)
//        {
//            Debug.LogError("OnPreprocessBuild Target = " + InTarget + "- Path = " + InPath);

//            DirectoryInfo rootInfo = new DirectoryInfo(Application.dataPath);

//            string xmlPath = Application.dataPath + "/Plugins/Android/AndroidManifest.xml";
//            XmlDocument xmlDoc = new XmlDocument();
//            xmlDoc.Load(xmlPath);
//            // 包名
//            XmlNode node = xmlDoc.SelectSingleNode("/manifest");
//            node.Attributes["package"].Value = jd["basic"]["app_package_name"].ToString();

//            // scheme
//            node = FindNode(xmlDoc, "/manifest/application/activity/intent-filter/data", "android:name", "appscheme");
//            node.Attributes["android:scheme"].Value = jd["android"]["android_manifest"]["url_schemes_name"].ToString();
//            // 百度-paraches
//            node = FindNode(xmlDoc, "/manifest/application/activity/intent-filter/data", "android:scheme", "paraches");
//            node.Attributes["android:host"].Value = jd["android"]["android_manifest"]["baidu_paraches"].ToString();
//            // 百度-API_KEY
//            node = FindNode(xmlDoc, "/manifest/application/meta-data", "android:name", "com.baidu.lbsapi.API_KEY");
//            node.Attributes["android:value"].Value = jd["android"]["android_manifest"]["baidu_app_key"].ToString();
//            // 云娃-YvImSdkAppId
//            node = FindNode(xmlDoc, "/manifest/application/meta-data", "android:name", "YvImSdkAppId");
//            node.Attributes["android:value"].Value = jd["android"]["android_manifest"]["yunwa_app_id"].ToString();
//            // bugly appid
//            node = FindNode(xmlDoc, "/manifest/application/meta-data", "android:name", "BUGLY_APPID");
//            node.Attributes["android:value"].Value = jd["android"]["android_manifest"]["bugly_app_id"].ToString();

//            // 自定义
//            string xpath = "/manifest/application/meta-data";
//            string key = "android:name";
//            string value = "android:value";
//            node = FindNode(xmlDoc, xpath, key, "wx_app_id");
//            node.Attributes[value].Value = jd["android"]["android_manifest"]["wx_app_id"].ToString();
//            node = FindNode(xmlDoc, xpath, key, "wx_app_secret");
//            node.Attributes[value].Value = jd["android"]["android_manifest"]["wx_app_secret"].ToString();
//            node = FindNode(xmlDoc, xpath, key, "xianliao_app_id");
//            node.Attributes[value].Value = jd["android"]["android_manifest"]["xianliao_app_id"].ToString();
//            node = FindNode(xmlDoc, xpath, key, "yunwa_app_id");
//            node.Attributes[value].Value = jd["android"]["android_manifest"]["yunwa_app_id"].ToString();
//            node = FindNode(xmlDoc, xpath, key, "baidu_app_key");
//            node.Attributes[value].Value = jd["android"]["android_manifest"]["baidu_app_key"].ToString();
//            node = FindNode(xmlDoc, xpath, key, "bugly_app_id");
//            node.Attributes[value].Value = jd["android"]["android_manifest"]["bugly_app_id"].ToString();
//            node = FindNode(xmlDoc, xpath, key, "url_schemes_name");
//            node.Attributes[value].Value = jd["android"]["android_manifest"]["url_schemes_name"].ToString();

//            // 保存
//            xmlDoc.Save(xmlPath);
//            AssetDatabase.Refresh();
//        }

//        public void OnPostprocessBuild(BuildTarget InTarget, string InPath)
//        {
//            Debug.LogError("OnPostprocessBuild Target = " + InTarget + "- Path = " + InPath);
//        }

//        static XmlNode FindNode(XmlDocument xmlDoc, string xpath, string attributeName, string attributeValue)
//        {
//            XmlNodeList nodes = xmlDoc.SelectNodes(xpath);
//            //Debug.Log(nodes.Count);
//            for (int i = 0; i < nodes.Count; i++)
//            {
//                XmlNode node = nodes.Item(i);
//                string _attributeValue = node.Attributes[attributeName].Value;
//                if (_attributeValue == attributeValue)
//                {
//                    return node;
//                }
//            }
//            return null;
//        }
//    }
//}

