namespace Branta.Automation.Wallets;

public abstract class BaseWallet
{
    public abstract string Name { get; }

    public abstract Dictionary<string, string> CheckSums { get; }

    public abstract string GetPath();

    public abstract string GetVersion();
}
