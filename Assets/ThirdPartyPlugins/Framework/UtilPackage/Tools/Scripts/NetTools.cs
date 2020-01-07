using System;
using System.Collections;
using UnityEngine;

namespace Szn.Framework.UtilPackage
{
    public class NetTools : MonoSingleton<NetTools>
    {
        public void GetWWWFrom(string InURL, Action<WWW> InAction)
        {
            StartCoroutine(GetNetEnumerator(InURL, InAction));
        }

        public void GetTextFromNet(string InURL, Action<string> InAction)
        {
            StartCoroutine(GetTextFromNetEnumerator(InURL, InAction));
        }

        public void GetBytesFromNet(string InURL, Action<byte[]> InAction)
        {
            StartCoroutine(GetBytesFromNetEnumerator(InURL, InAction));
        }

        public void GetAssetBundleFromNet(string InURL, Action<AssetBundle> InAction)
        {
            StartCoroutine(GetAssetBundleFromNetEnumerator(InURL, InAction));
        }

        public static IEnumerator GetNetEnumerator(string InURL, Action<WWW> InAction)
        {
            if (InAction == null) yield break;

            using (WWW www = new WWW(InURL))
            {
                yield return www;

                if (www.isDone && string.IsNullOrEmpty(www.error))
                {
                    InAction(www);
                }
                else
                {
                    Debug.LogError(www.error);
                }
            }
        }

        public static IEnumerator GetTextFromNetEnumerator(string InURL, Action<string> InAction)
        {
            if (InAction == null) yield break;

            using (WWW www = new WWW(InURL))
            {
                yield return www;

                if (www.isDone && string.IsNullOrEmpty(www.error))
                {
                    InAction(www.text);
                }
                else
                {
                    Debug.LogError(www.error);
                }
            }
        }

        public static IEnumerator GetBytesFromNetEnumerator(string InURL, Action<byte[]> InAction)
        {
            if (InAction == null) yield break;

            using (WWW www = new WWW(InURL))
            {
                yield return www;

                if (www.isDone && string.IsNullOrEmpty(www.error))
                {
                    InAction(www.bytes);
                }
                else
                {
                    Debug.LogError(www.error);
                }
            }
        }

        public static IEnumerator GetAssetBundleFromNetEnumerator(string InURL, Action<AssetBundle> InAction)
        {
            if (InAction == null) yield break;
            using (WWW www = new WWW(InURL))
            {
                yield return www;

                if (www.isDone && string.IsNullOrEmpty(www.error))
                {
                    InAction.Invoke(www.assetBundle);
                }
                else
                {
                    Debug.LogError(www.error);
                }
            }
        }
    }
}