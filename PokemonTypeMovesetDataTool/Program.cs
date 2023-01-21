using PokemonTypeMovesetDataTool;

var htmlTableFilename = args[0];

var json = new HtmlTable(htmlTableFilename).ToJson();

Console.Out.WriteLine(json);