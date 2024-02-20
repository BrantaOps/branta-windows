<picture>
  <source media="(prefers-color-scheme: dark)" srcset="Branta/Assets/goldwhitecropped.png">
  <source media="(prefers-color-scheme: light)" srcset="Branta/Assets/goldblackcropped.jpg">
  <img alt="Branta" src="Branta/Assets/goldblackcropped.jpg">
</picture>

![.NET Workflow Badge](https://github.com/BrantaOps/branta-windows/actions/workflows/dotnet.yml/badge.svg)


#### Languages and Tools
<div>
    <img src="https://raw.githubusercontent.com/devicons/devicon/master/icons/dotnetcore/dotnetcore-original.svg" height="40" title=".NET" />
    <img src="https://pic4.zhimg.com/50/v2-06f957e72756783fd7d73ff3e1b04a85_qhd.jpg" height="40" title="Windows Presentation Foundation" />
    <img src="https://external-content.duckduckgo.com/iu/?u=https%3A%2F%2Favatars1.githubusercontent.com%2Fu%2F2678858%3Fs%3D280%26v%3D4&f=1&nofb=1&ipt=10a8c0955262d6d9d7d6b62176e7faf2027a66de81e99e8537167b340351b1a4&ipo=images" height="40" title="NUnit"/>
</div>


## Features
 - ✅ Wallet Verification: Automatically verifies supported wallets against PGP verified SHA-256 checksums
 - ✅ Clipboard Guardian: Get notified of bitcoin-related activity on your clipboard
 - 🔳 Focus Automation: Verify wallets upon launch
 - 🔳 Installer Automation


![IMAGE 2024-02-04 10:53:51](https://github.com/BrantaOps/branta-windows/assets/74844722/aad0859b-4f9e-47c2-a302-f602df05e1cc)




#### Supported Wallets
 - ✅ Sparrow
 - ✅ Wasabi
 - ✅ Trezor
 - ✅ Blockstream Green
 - 🔳 Ledger
 - 🔳 Whirlpool


## Known Limitations

- Spoof Wallets likely install to custom paths. Branta needs to have more flexible path scanning to catch these.
- Ongoing wallet support - Branta doesn't know about new releases unless we manually input them.


## Building

Prerequisites 
 - [.NET 8.0](https://dotnet.microsoft.com/en-us/download/dotnet/8.0)

Clone the repo
```
git clone https://github.com/BrantaOps/branta-windows.git
```

Create a standalone executable
```
dotnet publish Branta/Branta.csproj -c Release --self-contained -r win-x64 -p:PublishSingleFile=true -p:IncludeNativeLibrariesForSelfExtract=true
```
Executable can be found in `Branta\bin\Release\net8.0-windows\win-x64\publish\`

*OR*

Run from Source
```
dotnet run
```

## Feature Requests & Bug Reporting

Open a [new issue](https://github.com/BrantaOps/branta-windows/issues/new) on Github and we'll reply as soon as we can.

## Policy on Altcoins/Altchains

Branta is Bitcoin-only. Enduring Bitcoin products are difficult to build; adding support for the unstable, poorly designed altcoin ecosystem is a lose-lose proposition.

## Licensing

The code in this project is licensed under the [MIT license](LICENSE.txt).
