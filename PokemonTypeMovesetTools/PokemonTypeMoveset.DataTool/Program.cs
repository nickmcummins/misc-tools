using System.CommandLine;
using static PokemonTypeMoveset.DataTool.CommandHandlers;



var rootCommand = new RootCommand("Pokemon type movesest data tool");

var downloadPokemonMovesetCommand = new Command("download-moveset", "Download a Pokemon's list of moves.");
var pokemonNameOption = new Option<string?>(name: "--name", description: "The name of the Pokemon to retrieve learnset.");
downloadPokemonMovesetCommand.AddOption(pokemonNameOption);
downloadPokemonMovesetCommand.SetHandler(DownloadPokemonMoveset, pokemonNameOption);
rootCommand.AddCommand(downloadPokemonMovesetCommand);

var downloadPokemonListCommand = new Command("parse-list", "Download list of Pokemon.");
var pokemonListLocationOption = new Option<string>(name: "--list", "");
downloadPokemonListCommand.AddOption(pokemonListLocationOption);
downloadPokemonListCommand.SetHandler(ParsePokemonList, pokemonListLocationOption);
rootCommand.AddCommand(downloadPokemonListCommand);



var output = await rootCommand.InvokeAsync(args);
Console.Out.WriteLine(output);
return output;