#define AD_DEBUG
public static class AdMobConfig
{
#if AD_DEBUG

#if UNITY_ANDROID
    public static readonly string AD_MOB_APP_ID = "ca-app-pub-3940256099942544~3347511713";
#elif UNITY_IOS
    public static readonly string AD_MOB_APP_ID = "ca-app-pub-3940256099942544~3347511713";
#endif

#elif AD_RELEASE

#if UNITY_ANDROID
    public static readonly string AD_MOB_APP_ID = "ca-app-pub-3940256099942544~3347511713";
#elif UNITY_IOS
    public static readonly string AD_MOB_APP_ID = "ca-app-pub-3940256099942544~3347511713";
#endif

#endif
}
