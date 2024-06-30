using Branta.Features.ClipboardGuardian;

namespace Branta.Tests.Commands;

public class ClipboardGuardianTest
{
    [TestCase("bc1qxp78h48nre66s2m529ey3u8lt39hesjveqtkap", true)]
    [TestCase("bc1qep2un4cvwmhf6kxjgp6kzqcpzzyl98l5zkndl2", true)]
    [TestCase("bc1qep/un4cvwmhf6kxjgp6kzqcpzzyl98l5zkndl2", false)]
    public void CheckForBitcoinAddress_Should(string clipboardContent, bool isValid)
    {
        Assert.That(isValid, Is.EqualTo(ClipboardGuardianCommand.CheckForBitcoinAddress(clipboardContent)));
    }

    [TestCase("abandon ability able about above absent absorb abstract absurd abuse access accident", true)]
    [TestCase("abandon\r\nability\r\nable\r\nabout\r\nabove\r\nabsent\r\nabsorb\r\nabstract\r\nabsurd\r\nabuse\r\naccess\r\naccident", true)]
    [TestCase("abandon ability able about above absent absorb abstract absurd abuse access", false)]
    [TestCase("abandon ability able about above absent absorb abstract absurd abuse access account accuse achieve acid acoustic acquire across act action actor actress actual adapt add", false)]
    public void CheckForSeedPhrase_Should(string clipboardContent, bool isValid)
    {
        var clipboardGuardian = new ClipboardGuardianCommand(null, null, null, null, null);

        Assert.That(isValid, Is.EqualTo(clipboardGuardian.CheckForSeedPhrase(clipboardContent)));
    }

    [TestCase("xpub661MyMwAqRbcFtXgS5sYJABqqG9YLmC4Q1Rdap9gSE8NqtwybGhePY2gZ29ESFjqJoCu1Rupje8YtGqsefD265TMg7usUDFdp6W1EGMcet8", true)]
    public void CheckForXPub_Should(string clipboardContent, bool isValid)
    {
        Assert.That(isValid, Is.EqualTo(ClipboardGuardianCommand.CheckForXPub(clipboardContent)));
    }

    [TestCase("xprv9s21ZrQH143K3QTDL4LXw2F7HEK3wJUD2nW2nRk4stbPy6cq3jPPqjiChkVvvNKmPGJxWUtg6LnF5kejMRNNU3TGtRBeJgk33yuGBxrMPHi", true)]
    public void CheckForXPrv_Should(string clipboardContent, bool isValid)
    {
        Assert.That(isValid, Is.EqualTo(ClipboardGuardianCommand.CheckForXPrv(clipboardContent)));

    }

    [TestCase("npub180cvv07tjdrrgpa0j7j7tmnyl2yr6yr7l8j4s3evf6u64th6gkwsyjh6w6", true)]
    public void CheckForNPub_Should(string clipboardContent, bool isValid)
    {
        Assert.That(isValid, Is.EqualTo(ClipboardGuardianCommand.CheckForNPub(clipboardContent)));
    }

    [TestCase("nsec1vl029mgpspedva04g90vltkh6fvh240zqtv9k0t9af8935ke9laqsnlfe5", true)]
    public void CheckForNPrv_Should(string clipboardContent, bool isValid)
    {
        Assert.That(isValid, Is.EqualTo(ClipboardGuardianCommand.CheckForNPrv(clipboardContent)));
    }
}
