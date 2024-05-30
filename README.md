# Rainier Card Definition Fetcher
![logo](logo.png)

This is a small library and standalone tool used for both Omukade development and fetching card and rule data used to run the Omukade family of TCGL servers.

**NOTE: As of v1.0.15, this no longer requires seperate installation, and is included with Omukade servers.**

## Requirements
* [.NET 6 Runtime or SDK](https://dotnet.microsoft.com/en-us/download/dotnet/6.0) for your platform
* A set of current TCGL assemblies and updated by PAR
* Supports Windows x64 and Linux x64 + ARM64
* For developing, Visual Studio 2022 (any edition) with C#, and [Procedual Assembly Rewriter](https://github.com/Hastwell/Omukade.ProcedualAssemblyRewriter)

## Standalone Usage

In addition to being an included library in server applications, this tool can be run independently for advanced scenarios. For basic use of Omukade software,
this should usually not be required.

Before running this command, populate the file `secrets.json` in the app's directory with your Pokemon Trainer's Club account. This account is used to fetch
data from the TCGL servers. eg:
```json
{"username": "mysigninname", "password": "abc123"}
```

**The account used must have previously logged into TCGL** as this app cannot deal with any of the first-signin "authorize TCGL to access your account" stuff.

This application uses AutoPAR to load the TCGL binaries. Although they are usually automatically fetched without issue, if issues do arise, one of the following can be used to supply the needed TCGL binaries:
* Windows Only, Recommended: Install Pokemon TCG Live. It will be auto-detected by AutoPAR and used for this application.
* Add the setting `autopar-search-folder` with the location of your TCGL install directory to secrets.json. Backslashes and quotes must be escaped (`\\` and `\"` respectively).
* Copy the TCGL assemblies from your TCGL install directory (`C:\Install\Folder\Pokémon Trading Card Game Live\Pokemon TCG Live_Data\Managed`) to the folder `autopar` under the app's folder.
  Alternatively, the config setting `autopar-search-folder` can be used to set any other name for this directory if prefered. *You must manually update this folder whenever the game updates!*

### Arguments
```
Fetch arguments (as many as desired can be specified):
--fetch-itemdb          Fetches the database of items.
--fetch-cardactions     Fetches the localized list of card actions.
--fetch-carddb          Fetches the list of cards for display in-game (not implementations, see --fetch-carddefinitions)
--fetch-carddefinitions / --fetch-carddefs Fetches all card implementations.
--ignore-invalid-ids-file   By default, --fetch-carddefinitions will create a file of known-bad card IDs that can be skipped
                            for significantly faster performance on subsequent executions (minutes vs hours).
                            These entries may become stale as skipped cards become implemented; --fetch-carddefinitions should
                            probably be run monthly with this flag.

                            If the invalid IDs file doesn't already exist, this flag will have no effect.

--fetch-rules           Fetches the game rules.
--fetch-aidecks         Fetches a selection of decks used by the AI.
--interactive           Enters an interactive prompt allowing arbitrary documents to be downloaded, given their name.

Omukade Cheyenne servers typically only need the results of --fetch-carddefinitions --fetch-rules

Output arguments:
--output-folder (/foo/bar)  Writes all fetched data to this folder. By default, the current working directory is used for output.
                            Directories will be created under this folder with all retrieved information.

Other arguments:
-h / --help             This help text. Will also appear automatically if run with no fetch arguments, or no arguments at all.
--no-update-check       Skips checking for Rainier updates (eg, if started from another program that also did this check)
--quiet                 Supresses all non-error messages.
--token=abc123          Manually specify the token to use, instead of automatically logging in.
```

## Compiling

### Rainier Dependencies with AutoPAR
When checking out a project using AutoPAR for the first time, you may see errors related to types and assemblies not found.
The `Omukade.AutoPAR.BuildPipeline.Rainier` package will fetch and prepare these dependencies for you when attempting a build.
No manual action should be needed, although client updates may require changing referenced libraries (eg, old libs removed, new ones added that are now required)

### Building
* Use Visual Studio 2022 or later, build the project.
* With the .NET 6 SDK, `dotnet build Omukade.CardDefinitionFetcher.sln`

## License
This software is licensed under the terms of the [GNU AGPL v3.0](https://www.gnu.org/licenses/agpl-3.0.en.html)