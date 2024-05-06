using CountlySDK;
using CountlySDK.Entities;

namespace Branta.Classes;

public static class Analytics
{
    public static void Init()
    {
        var cc = new CountlyConfig
        {
            serverUrl = "https://branta-0dc12e4ffb389.flex.countly.com",
            appKey = "ccc4eb59a850e5f3bdf640b8d36284c3bce03f12",
            appVersion = Helper.GetBrantaVersionWithoutCommitHash()
        };

        Countly.Instance.Init(cc);
        Countly.Instance.SessionBegin();
    }
}