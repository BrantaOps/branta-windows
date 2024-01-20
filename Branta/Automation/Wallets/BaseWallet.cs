namespace Branta.Automation.Wallets;

public abstract class BaseWallet
{
    public string Name { get; }

    public abstract Dictionary<string, string> CheckSums { get; }

    protected BaseWallet(string name)
    {
        Name = name;
    }

    public abstract string GetPath();

    public abstract string GetVersion();
}
