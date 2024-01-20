![Branta](Branta/Assets/goldwhitecropped.png)

*Branta software for Windows*

## About
Branta is a one stop shop for all things bitcoin security.

#### Languages and Tools
<div>
    <img src="https://raw.githubusercontent.com/devicons/devicon/master/icons/csharp/csharp-original.svg" height="40" />
    <img src="https://pic4.zhimg.com/50/v2-06f957e72756783fd7d73ff3e1b04a85_qhd.jpg" height="40" />
</div>

### Learn more
https://branta.pro

## Features
 - [X] Wallet Verification: Automatically verifies supported wallets against PGP verified SHA-256 checksums
 - [X] Clipboard Guardian: Get notified of bitcoin-related activity on your clipboard

#### Supported Wallets
 - [X] Sparrow
 - [ ] Ledger
 - [ ] Trezor
 - [X] Wasabi
 - [ ] Blockstream Green
 - [ ] Whirlpool

## Getting Started

Clone the repo
```
git clone https://github.com/BrantaOps/branta-windows.git
```

Create a standalone executable
```
dotnet publish -c Release --self-contained -r win-x64 -p:PublishSingleFile=true -p:IncludeNativeLibrariesForSelfExtract=true
```
Executable can be found in `Branta\bin\Release\net8.0-windows\win-x64\publish\`
