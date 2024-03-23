using System.IO;

namespace Branta.Automation.Wallets;

public class Sparrow : BaseWallet
{
    public override Dictionary<string, string> CheckSums => new()
    {
        { "1.8.4", "b27babb2b7e6024f58dcf975081f0285" },
        { "1.8.3", "5a4087e257ec0b2a40b4b40a8e2ed58d" },
        { "1.8.2", "25bdc8beb04642cabfef30b3529c375c" },
        { "1.8.1", "6b7e17b96e840aea32a40a3e73f1ba86" },
        { "1.8.0", "416e0ea8b3b6dffe097b5c3b9bd71aa6" },
        { "1.7.9", "dae54bdff194bc5aadc17b89ca50fe39" },
        { "1.7.8", "718f7b8293545395a568bbbc55671939" }
    };

    public override Dictionary<string, string> InstallerHashes => new()
    {
        { "Sparrow-1.8.4-aarch64.dmg", "76187ef7b52e22a6b45840fd493c7aea6affce42ee6ae6b9399cc99f65e632cb" },
        { "Sparrow-1.8.4-x86_64.dmg", "e13a325b66b722d64c563b06b92ac0559e7b9e7927c46b48ab19df1e99317937" },
        { "Sparrow-1.8.4.exe", "60c3ac1fe44957baca902d1f4297cbd35a154ffa740dafe271ef553c1c3bb692" },
        { "Sparrow-1.8.4.zip", "4464689617842288078df66afbb6dfa994e900b091c6c39a90c0680259a1906b" },
        { "sparrow-1.8.4-1.aarch64.rpm", "0b8cb74ad81c8c7f29b1cb7816a866b9f03bac154e87a7bfb17e9046dcdf2b1f" },
        { "sparrow-1.8.4-1.x86_64.rpm", "a39be262a6437003216759d20c2c9e5a1e3cf22d8156f490810f263662226d9c" },
        { "sparrow-1.8.4-aarch64.tar.gz", "6b050131368f639c33e52a88eb6fb03678acc009f8e464eccfbe353e3cac8156" },
        { "sparrow-1.8.4-x86_64.tar.gz", "a8ed2e84455747df5cff7870790423e9e20e06662bd5e232be8e5f9d3841df70" },
        { "sparrow-server-1.8.4-1.aarch64.rpm", "456d7f8fce2409f07a3f1850ff2b4343e4d74b45939d780ec6e4586f8f63c3b2" },
        { "sparrow-server-1.8.4-1.x86_64.rpm", "8a933394289db6a7a511924ccd23586d7a8e5e6b33206742ecc7d89bbce7af7a" },
        { "sparrow-server-1.8.4-aarch64.tar.gz", "e41ee194decef351ac1fe43faf36d5c4afe20146e80ba551e4cd19e9642d49a7" },
        { "sparrow-server-1.8.4-x86_64.tar.gz", "e627ed0ba6a2a135c135901dff65f68034d03aa206b1a955a9505b3e36efc760" },
        { "sparrow-server_1.8.4-1_amd64.deb", "633f8cf81cd3719490f94ce8bebb7e9f30c4a5c73605f1283fa2db41905f5dd1" },
        { "sparrow-server_1.8.4-1_arm64.deb", "bf79e57e165da48c789668e6a60e6da4e655c4c01edc05a0eb4a7f8917e0f311" },
        { "sparrow_1.8.4-1_amd64.deb", "61d6508b41d1b7f233b7d619025acac6dc1816c9871ddf4b3cdb963050427897" },
        { "sparrow_1.8.4-1_arm64.deb", "f9a80b54f0e0dc49660df929138910338fa4b9caa8377351e649e6597f83199b" }
    };

    public Sparrow() : base("Sparrow")
    {
    }

    public override string GetPath()
    {
        var localPath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);

        return Path.Join(localPath, "Sparrow");
    }
}
