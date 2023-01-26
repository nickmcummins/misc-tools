using PokemonTypeMoveset.DataTool;
using PokemonTypeMovesetDataTool;
using System.CommandLine;



var pokemonNameOption = new Option<string?>(
            name: "--name",
            description: "The name of the Pokemon to retrieve learnset.");

var rootCommand = new RootCommand("Sample app for System.CommandLine");
rootCommand.AddOption(pokemonNameOption);

rootCommand.SetHandler(async (name) => 
{ 
    var moveNames = await new PokemonLearnsetHtml().GetMoveNames(name);
    FileDataProvider.PokemonLearnsets[name] = moveNames;
    FileDataProvider.PersistLearnsetsDatastore();

}, pokemonNameOption);

var output = await rootCommand.InvokeAsync(args);
Console.Out.WriteLine(output);
return output;