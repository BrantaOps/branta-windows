namespace Branta.Classes;

public class CheckSums
{
    public Dictionary<string, VersionInfo> BlockstreamGreen { get; set; }
    public Dictionary<string, VersionInfo> Ledger { get; set; }
    public Dictionary<string, VersionInfo> Sparrow { get; set; }
    public Dictionary<string, VersionInfo> Trezor { get; set; }
    public Dictionary<string, VersionInfo> Wasabi { get; set; }
}
