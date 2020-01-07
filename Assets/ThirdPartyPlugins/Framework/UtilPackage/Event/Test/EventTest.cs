#if UNIT_TEST
using System;
using Szn.Framework.UtilPackage.Event;
using UnityEngine;

public class EventTest : MonoBehaviour
{
    // Use this for initialization
    void Start()
    {
        Action none = () => Debug.LogError("None");
        Action<string> one = InS =>
        {
            Debug.LogError(InS + "-0");
        };
        Action<string> one1  = InS =>
        {
            Debug.LogError(InS + "-1");
        };
        Action<string> one2  = InS =>
        {
            Debug.LogError(InS + "-2");
        };
        Action<string> one3  = InS =>
        {
            Debug.LogError(InS + "-3");
        };
        Action<string> one4  = InS =>
        {
            Debug.LogError(InS + "-4");
        };
        
        Action<string, string> two = (InS, InS1) => Debug.LogError(InS + "+" + InS1);
        Action<string, string, string> three = (InS, InS1, InS2) => Debug.LogError(InS + "+" + InS1 + "+" + InS2);
        Action<string, string, string, string> four = (InS, InS1, InS2, InS3) =>
            Debug.LogError(InS + "+" + InS1 + "+" + InS2 + "+" + InS3);

        EventDispatcher.Instance.Register(EventKey.None, none, true);
        EventDispatcher.Instance.Trigger(EventKey.None);
        
        EventDispatcher.Instance.Register(EventKey.One, one, true);
        EventDispatcher.Instance.Trigger(EventKey.One, "0");
        
        EventDispatcher.Instance.Register(EventKey.One, one1, true);
        EventDispatcher.Instance.Trigger(EventKey.One, "1");
        
        EventDispatcher.Instance.Register(EventKey.One, one2);
        EventDispatcher.Instance.Trigger(EventKey.One, "2");
        
        EventDispatcher.Instance.Register(EventKey.One, one3);
        EventDispatcher.Instance.Trigger(EventKey.One, "3");
        
        EventDispatcher.Instance.Register(EventKey.One, one4);
        EventDispatcher.Instance.Trigger(EventKey.One, "4");
        
        EventDispatcher.Instance.Unregister(EventKey.One, one2);
        EventDispatcher.Instance.Unregister(EventKey.One, one3);
        
        EventDispatcher.Instance.Trigger(EventKey.One, "5");
        

        EventDispatcher.Instance.Register(EventKey.Two, two, true);
        EventDispatcher.Instance.Trigger(EventKey.Two, "one", "two");

        EventDispatcher.Instance.Register(EventKey.Three, three, true);
        EventDispatcher.Instance.Trigger(EventKey.Three, "one", "two", "three");

        EventDispatcher.Instance.Register(EventKey.Four, four, true);
        EventDispatcher.Instance.Trigger(EventKey.Four, "one", "two", "three", "Four");
    }
}
#endif