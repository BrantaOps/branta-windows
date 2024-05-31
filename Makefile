run:
	dotnet run --project=Branta

publish:
	dotnet publish Branta/Branta.csproj -c Release --self-contained -r win-x64 -p:PublishSingleFile=true -p:IncludeNativeLibrariesForSelfExtract=true
