using UnityEngine;

namespace SznFramework.UtilPackage
{
    public sealed class DeviceInfoTools
    {
        private static DeviceInfoTools instance;

        public static DeviceInfoTools Instance
        {
            get { return instance ?? (instance = new DeviceInfoTools()); }
        }

        
#if UNITY_EDITOR
#elif UNITY_ANDROID
        public AndroidJavaClass UnityPlayer { get; private set; }
        private readonly AndroidJavaClass toast;
        public AndroidJavaObject UnityActivity { get; private set; }
        public AndroidJavaObject UnityContext { get; private set; }
        private readonly int showToastTime;
#endif

        public string UniqueId { get; private set; }
        public string Country { get; private set; }
        public string Language { get; private set; }
        public string OperatingSystem { get; private set; }
        public string Model { get; private set; }
        public string ScreenSize { get; private set; }
        public string Memory { get; private set; }

        private const string IS_GET_DEVICE_INFO = "IS_GET_DEVICE_INFO";
        private const string UNIQUE_ID = "UniqueId";
        private const string COUNTRY = "Country";
        private const string LANGUAGE = "Language";
        private const string OPERATING_SYSTEM = "OperatingSystem";
        private const string MODEL = "Model";
        private const string SCREEN_SIZE = "ScreenSize";
        private const string MEMORY = "Memory";

        public DeviceInfoTools()
        {
#if UNITY_EDITOR
#elif UNITY_ANDROID
            UnityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
            UnityActivity = UnityPlayer.GetStatic<AndroidJavaObject>("currentActivity");
            UnityContext = UnityActivity.Call<AndroidJavaObject>("getApplicationContext");
            toast = new AndroidJavaClass("android.widget.Toast");
            showToastTime = toast.GetStatic<int>("LENGTH_SHORT");
#endif

            if (PlayerPrefs.GetInt(IS_GET_DEVICE_INFO, 0) == 0) GetDeviceInfo();
            UniqueId = PlayerPrefs.GetString(UNIQUE_ID);
            Country = PlayerPrefs.GetString(COUNTRY);
            Language = PlayerPrefs.GetString(LANGUAGE);
            OperatingSystem = PlayerPrefs.GetString(OPERATING_SYSTEM);
            Model = PlayerPrefs.GetString(MODEL);
            ScreenSize = PlayerPrefs.GetString(SCREEN_SIZE);
            Memory = PlayerPrefs.GetString(MEMORY);
        }

        private void GetDeviceInfo()
        {
            if (PlayerPrefs.GetInt(IS_GET_DEVICE_INFO, 0) == 0)
            {
                try
                {
#if UNITY_EDITOR
                    PlayerPrefs.SetString(UNIQUE_ID, SystemInfo.deviceUniqueIdentifier);
                    PlayerPrefs.SetString(SCREEN_SIZE, string.Format("{0}x{1}", Screen.width, Screen.height));
                    PlayerPrefs.SetString(MODEL, SystemInfo.deviceModel);
                    PlayerPrefs.SetString(OPERATING_SYSTEM, SystemInfo.operatingSystem);
                    PlayerPrefs.SetString(MEMORY, SystemInfo.systemMemorySize.ToString());
#elif UNITY_ANDROID
                    using (AndroidJavaClass secure = new AndroidJavaClass("android.provider.Settings$Secure"))
                    {
                        PlayerPrefs.SetString(UNIQUE_ID, secure.CallStatic<string>("getString",
                            UnityContext.Call<AndroidJavaObject>("getContentResolver"),
                            secure.GetStatic<string>("ANDROID_ID")));
                    }

                    using (AndroidJavaClass locale = new AndroidJavaClass("java.util.Locale"))
                    {
                        using (AndroidJavaObject defaultLocale = locale.CallStatic<AndroidJavaObject>("getDefault"))
                        {
                            PlayerPrefs.SetString(COUNTRY, defaultLocale.Call<string>("getCountry"));
                            PlayerPrefs.SetString(LANGUAGE, defaultLocale.Call<string>("getLanguage"));
                        }
                    }
#endif
                    PlayerPrefs.SetInt(IS_GET_DEVICE_INFO, 1);
                }
                catch (System.Exception exception)
                {
                    PlayerPrefs.SetInt(IS_GET_DEVICE_INFO, 0);
                    Debug.LogError(string.Format("Msg:\n{0}\nStack Trace:\n{1}", exception.Message,
                        exception.StackTrace));
                }
            }
        }

        public void ShowDeviceInfo()
        {
            Debug.LogError(string.Format(
                "Unique Id = {0}\nCountry = {1}\nLanguage = {2}\nOperatingSystem = {3}\nModel = {4}\nScreenSize = {5}\nMemory = {6}",
                PlayerPrefs.GetString(UNIQUE_ID),
                PlayerPrefs.GetString(COUNTRY),
                PlayerPrefs.GetString(LANGUAGE),
                PlayerPrefs.GetString(OPERATING_SYSTEM),
                PlayerPrefs.GetString(MODEL),
                PlayerPrefs.GetString(SCREEN_SIZE),
                PlayerPrefs.GetString(MEMORY)));
        }

        public void ShowToast(string InContent)
        {
#if !UNITY_EDITOR && UNITY_ANDROID
            UnityActivity.Call("runOnUiThread", new AndroidJavaRunnable(() =>
            {
                toast.CallStatic<AndroidJavaObject>("makeText",
                    UnityContext,
                    InContent,
                    showToastTime).Call("show");
            }));
#endif
        }
    }
}
