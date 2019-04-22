#define AD_DEBUG
public static class AdMobConfig
{
#if AD_DEBUG

#if UNITY_ANDROID
    public static readonly string AD_MOB_APP_ID = "ca-app-pub-3940256099942544~3347511713";
    public static readonly string BANNER_AD_ID = "ca-app-pub-3940256099942544/6300978111";
    public static readonly string INTERSTITIAL_AD_ID = "ca-app-pub-3940256099942544/1033173712";
    public static readonly string REWARD_AD_ID = "ca-app-pub-3940256099942544/5224354917";
#elif UNITY_IOS
    public static readonly string AD_MOB_APP_ID = "ca-app-pub-3940256099942544~1458002511";
    public static readonly string BANNER_AD_ID = "ca-app-pub-3940256099942544/2934735716";
    public static readonly string INTERSTITIAL_AD_ID = "ca-app-pub-3940256099942544/4411468910";
    public static readonly string REWARD_AD_ID = "ca-app-pub-3940256099942544/1712485313";
#endif

#elif AD_RELEASE

#if UNITY_ANDROID
    public static readonly string AD_MOB_APP_ID = "ca-app-pub-3940256099942544~3347511713";
    public static readonly string BANNER_AD_ID = "ca-app-pub-3940256099942544/6300978111";
    public static readonly string INTERSTITIAL_AD_ID = "ca-app-pub-3940256099942544/1033173712";
    public static readonly string REWARD_AD_ID = "ca-app-pub-3940256099942544/5224354917";
#elif UNITY_IOS
    public static readonly string AD_MOB_APP_ID = "ca-app-pub-3940256099942544~1458002511";
    public static readonly string BANNER_AD_ID = "ca-app-pub-3940256099942544/2934735716";
    public static readonly string INTERSTITIAL_AD_ID = "ca-app-pub-3940256099942544/4411468910";
    public static readonly string REWARD_AD_ID = "ca-app-pub-3940256099942544/1712485313";
#endif

#endif
}
