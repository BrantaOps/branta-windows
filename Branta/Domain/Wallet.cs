using Branta.Enums;

namespace Branta.Domain;

public class Wallet
{
    public string Name { get; set; }

    public string Version { get; set; }

    public WalletStatus Status { get; set; }
}