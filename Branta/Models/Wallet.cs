using Branta.Enums;

namespace Branta.Models;

public class Wallet
{
    public string Name { get; set; }

    public string Version { get; set; }

    public DateTime LastScanned { get; set; }

    public WalletStatus Status { get; set; }

    public string ExeName { get; set; }
}