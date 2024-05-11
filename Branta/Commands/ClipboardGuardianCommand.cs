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
    private readonly Settings _settings;

    private const int SeedWordMin = 12;
    private const int SeedWordMax = 24;

    private string LastClipboardContent { get; set; }
    private HashSet<string> Bip39Words { get; set; }

    public ClipboardGuardianCommand(ClipboardGuardianViewModel viewModel, NotificationCenter notificationCenter, Settings settings)
    {
        _viewModel = viewModel;
        _notificationCenter = notificationCenter;
        _settings = settings;
    }

    public override void Execute(object parameter)
    {
        string clipBoardContent = Application.Current.Dispatcher.Invoke(() =>
        {
            return Clipboard.GetText().Trim();
        });

        if (clipBoardContent == LastClipboardContent)
        {
            return;
        }

        LastClipboardContent = clipBoardContent;

        var clipboardItem = Process(clipBoardContent) ?? new ClipboardItem();

        _viewModel.ClipboardItem = new ClipboardItemViewModel(clipboardItem);

        if (clipboardItem?.Notification != null)
        {
            _notificationCenter.Notify(clipboardItem.Notification);
        }
    }

    private ClipboardItem Process(string clipBoardContent)
    {
        if (CheckForBitcoinAddress(clipBoardContent))
        {
            return new ClipboardItem
            {
                Name = "Bitcoin Address",
                Value = clipBoardContent,
                Notification = _settings.ClipboardGuardian.BitcoinAddressesEnabled ? new Notification()
                {
                    Title = "New Address in Clipboard.",
                    Message = "Bitcoin detected.",
                } : null
            };
        }

        if (CheckForSeedPhrase(clipBoardContent))
        {
            return new ClipboardItem
            {
                Name = "Seed Phrase Detected",
                Notification = _settings.ClipboardGuardian.SeedPhraseEnabled ? new Notification()
                {
                    Title = "Seed Phrase in clipboard detected.",
                    Message = "Never share your seed phrase with anyone. Your seed phrase IS your money."
                } : null
            };
        }

        if (CheckForXPub(clipBoardContent))
        {
            return new ClipboardItem
            {
                Name = "Bitcoin Public Key Detected",
                Value = clipBoardContent,
                Notification = _settings.ClipboardGuardian.ExtendedPublicKeyEnabled ? new Notification()
                {
                    Title = "Bitcoin Extended Public Key in Clipboard.",
                    Message = "Sharing your XPUB can lead to loss of privacy."
                } : null
            };
        }

        if (CheckForXPrv(clipBoardContent))
        {
            return new ClipboardItem
            {
                Name = "Bitcoin Private Key Detected",
                Notification = _settings.ClipboardGuardian.PrivateKeyEnabled ? new Notification()
                {
                    Title = "Bitcoin Private Key in Clipboard.",
                    Message = "Never share your private key with others."
                } : null
            };
        }

        if (CheckForNPub(clipBoardContent))
        {
            return new ClipboardItem
            {
                Name = "Nostr Public Key Detected",
                Value = clipBoardContent,
                Notification = _settings.ClipboardGuardian.NostrPublicKeyEnabled ? new Notification()
                {
                    Title = "New Nostr Public Key in Clipboard."
                } : null
            };
        }

        if (CheckForNPrv(clipBoardContent))
        {
            return new ClipboardItem
            {
                Name = "Nostr Private Key Detected",
                Notification = _settings.ClipboardGuardian.NostrPrivateKeyEnabled ? new Notification()
                {
                    Title = "New Nostr Private Key in Clipboard.",
                    Message = "Never share your private key with others."
                } : null
            };
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
