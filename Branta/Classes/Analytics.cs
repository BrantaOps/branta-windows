using CountlySDK;
using CountlySDK.Entities;

namespace Branta.Classes;

public static class Analytics
{
    public static void Init(AppSettings appSettings)
    {
        var cc = new CountlyConfig
        {
            serverUrl = appSettings.Countly.Url,
            appKey = appSettings.Countly.ApiKey,
            appVersion = Helper.GetBrantaVersionWithoutCommitHash()
        };

        Countly.Instance.Init(cc);
        Countly.Instance.SessionBegin();
    }
}