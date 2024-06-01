<picture>
  <source media="(prefers-color-scheme: dark)" srcset="Branta/Assets/goldwhitecropped.png">
  <source media="(prefers-color-scheme: light)" srcset="Branta/Assets/goldblackcropped.jpg">
  <img alt="Branta" src="Branta/Assets/goldblackcropped.jpg">
</picture>

![Branta Screenshot](https://github.com/BrantaOps/branta-windows/assets/110685100/b2548673-3f08-46c5-8f10-428e0394e16f)


## Docs
https://docs.branta.pro/

![.NET Workflow Badge](https://github.com/BrantaOps/branta-windows/actions/workflows/dotnet.yml/badge.svg)

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

## Releases

To Create the MSI installer Build the Branta.Setup project in Release

Note: For the installer to work the ProductCode and ProductVersion in `Branta.Setup.vdproj` must be updated.

## Donate

Branta is free open source software. If you can, consider donating on [Geyser](https://geyser.fund/project/branta). All funds go towards improving and automating security for your stack.


## Feature Requests & Bug Reporting

Open a [new issue](https://github.com/BrantaOps/branta-windows/issues/new) on Github and we'll reply as soon as we can.


## Licensing

The code in this project is licensed under the [MIT license](LICENSE.txt).
