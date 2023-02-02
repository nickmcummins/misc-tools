using System.CommandLine;
using static PokemonTypeMoveset.DataTool.CommandHandlers;



var rootCommand = new RootCommand("Pokemon type movesest data tool");

var downloadPokemonMovesetCommand = new Command("download-moveset", "Download a Pokemon's list of moves.");
var pokemonNameOption = new Option<string?>(name: "--name", description: "The name of the Pokemon to retrieve learnset.");
var allPokemonOption = new Option<bool?>(name: "--all", description: "Download learnsets for all Pokemon.");
var redownloadExistingOption = new Option<bool?>(name: "--redownload-existing", description: "Redownload learnset for existing.");

downloadPokemonMovesetCommand.AddOption(pokemonNameOption);
downloadPokemonMovesetCommand.AddOption(allPokemonOption);
downloadPokemonMovesetCommand.SetHandler(async (pokemonName, redownloadExisting) => { await DownloadPokemonMoveset(pokemonName, redownloadExisting.GetValueOrDefault()); }, pokemonNameOption, redownloadExistingOption);
rootCommand.AddCommand(downloadPokemonMovesetCommand);

var downloadPokemonListCommand = new Command("parse-list", "Download list of Pokemon.");
var pokemonListLocationOption = new Option<string>(name: "--list", "");
downloadPokemonListCommand.AddOption(pokemonListLocationOption);
downloadPokemonListCommand.SetHandler(ParsePokemonList, pokemonListLocationOption);
rootCommand.AddCommand(downloadPokemonListCommand);



var output = await rootCommand.InvokeAsync(args);
Console.Out.WriteLine(output);
return output;