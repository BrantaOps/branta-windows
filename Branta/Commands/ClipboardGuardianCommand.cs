using Branta.Classes;
using Branta.Models;
using Branta.ViewModels;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Windows;

namespace Branta.Commands;

public partial class ClipboardGuardianCommand : BaseCommand
{
    private readonly ClipboardGuardianViewModel _viewModel;
    private readonly NotificationCenter _notificationCenter;
    private readonly ResourceDictionary _resourceDictionary;
    private readonly Settings _settings;

    private const int SeedWordMin = 12;
    private const int SeedWordMax = 24;

    private string LastClipboardContent { get; set; }
    private HashSet<string> Bip39Words { get; set; }

    public ClipboardGuardianCommand(ClipboardGuardianViewModel viewModel, NotificationCenter notificationCenter,
        Settings settings,ResourceDictionary resourceDictionary)
    {
        _viewModel = viewModel;
        _notificationCenter = notificationCenter;
        _settings = settings;
        _resourceDictionary = resourceDictionary;
    }

    public override void Execute(object parameter)
    {
        var clipBoardContent = Application.Current.Dispatcher.Invoke(() => Clipboard.GetText().Trim());

        if (clipBoardContent == LastClipboardContent)
        {
            return;
        }

        LastClipboardContent = clipBoardContent;

        var clipboardItem = Process(clipBoardContent) ?? new ClipboardItem
        {
            Value = "No Bitcoin/Nostr content detected.",
            IsDefault = true
        };

        _viewModel.ClipboardItem = new ClipboardItemViewModel(clipboardItem, _resourceDictionary);

        if (clipboardItem?.Notification != null)
        {
            _notificationCenter.Notify(clipboardItem.Notification);
        }
    }

    private ClipboardItem Process(string clipBoardContent)
    {
        if (CheckForBitcoinAddress(clipBoardContent))
        {
            return new ClipboardItem(_settings.ClipboardGuardian.BitcoinAddressesEnabled,
                                     _resourceDictionary,
                                     "BitcoinAddress",
                                     clipBoardContent);
        }

        if (CheckForSeedPhrase(clipBoardContent))
        {
            return new ClipboardItem(_settings.ClipboardGuardian.SeedPhraseEnabled,
                                     _resourceDictionary,
                                     "SeedPhrase");
        }

        if (CheckForXPub(clipBoardContent))
        {
            return new ClipboardItem(_settings.ClipboardGuardian.ExtendedPublicKeyEnabled,
                                     _resourceDictionary,
                                     "BitcoinPublicKey");
        }

        if (CheckForXPrv(clipBoardContent))
        {
            return new ClipboardItem(_settings.ClipboardGuardian.PrivateKeyEnabled,
                                     _resourceDictionary,
                                     "BitcoinPrivateKey");
        }

        if (CheckForNPub(clipBoardContent))
        {
            return new ClipboardItem(_settings.ClipboardGuardian.NostrPublicKeyEnabled,
                                     _resourceDictionary,
                                     "NostrPublicKey");
        }

        if (CheckForNPrv(clipBoardContent))
        {

            return new ClipboardItem(_settings.ClipboardGuardian.NostrPrivateKeyEnabled,
                                     _resourceDictionary,
                                     "NostrPrivateKey");
        }

        return null;
    }

    public static bool CheckForBitcoinAddress(string value)
    {
        return BitcoinAddressRegex().IsMatch(value) || SegwitAddressRegex().IsMatch(value);
    }

    public bool CheckForSeedPhrase(string content)
    {
        if (string.IsNullOrWhiteSpace(content))
        {
            return false;
        }

        if (!content.Contains(' ') && !content.Contains(Environment.NewLine))
        {
            return false;
        }

        var spaced = content.Split(' ');
        var newLined = content.Split(Environment.NewLine);

        var userSeedWords = new HashSet<string>();

        if (spaced.Length >= newLined.Length)
        {
            userSeedWords = new HashSet<string>(spaced);
        }
        else if (newLined.Length > spaced.Length)
        {
            userSeedWords = new HashSet<string>(newLined);
        }

        // TODO - handle the case where the line broke, and it's a valid seed with \n on a line. Strip each word of \n.

        if (userSeedWords.Count < SeedWordMin || userSeedWords.Count > SeedWordMax)
        {
            return false;
        }

        if (Bip39Words == null)
        {
            try
            {
                var assembly = Assembly.GetExecutingAssembly();
                const string resourceName = "Branta.Assets.bip39wordlist.txt";

                using var stream = assembly.GetManifestResourceStream(resourceName);
                using var reader = new StreamReader(stream!);

                var words = reader.ReadToEnd();
                Bip39Words = words.Split(Environment.NewLine).ToHashSet();
            }
            catch (Exception ex)
            {
                Trace.Listeners.Add(new TextWriterTraceListener("log.txt"));
                Trace.WriteLine(ex);
                Trace.Flush();
            }
        }

        return !userSeedWords.Select(w => w.ToLower()).Except(Bip39Words!).Any();
    }

    public static bool CheckForXPub(string value)
    {
        var isXPub = value.StartsWith("xpub") || value.StartsWith("ypub") || value.StartsWith("zpub");

        return isXPub && value.Length > 10;
    }

    public static bool CheckForXPrv(string value)
    {
        return value.StartsWith("xprv") && value.Length > 10;
    }

    public static bool CheckForNPub(string value)
    {
        return NPubAddressRegex().IsMatch(value);
    }

    public static bool CheckForNPrv(string value)
    {
        return NPrvAddressRegex().IsMatch(value);
    }

    [GeneratedRegex("^[13][a-km-zA-HJ-NP-Z1-9]{25,34}$")]
    private static partial Regex BitcoinAddressRegex();

    [GeneratedRegex("^bc1[0-9a-zA-Z]{25,39}$")]
    private static partial Regex SegwitAddressRegex();

    [GeneratedRegex("^npub[0-9a-z]{58,65}$")]
    private static partial Regex NPubAddressRegex();

    [GeneratedRegex("^nsec[0-9a-z]{58,65}$")]
    private static partial Regex NPrvAddressRegex();
}
