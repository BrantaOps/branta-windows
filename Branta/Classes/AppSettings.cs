namespace Branta.Classes;

public class AppSettings
{
    public CountlySettings Countly { get; set; }
}

public class CountlySettings
{
    public string Url { get; set; }

    public string ApiKey { get; set; }
}