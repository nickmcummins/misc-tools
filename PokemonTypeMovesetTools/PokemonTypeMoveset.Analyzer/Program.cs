using PokemonTypeMoveset.DataTool;
using PokemonTypeMovesetAnalyzer;
using PokemonTypeMovesetAnalyzer.Models;

Pokemon goodra = null; // new Pokemon("Goodra", await FileDataProvider.Instance.GetMoves("Goodra"));
var movesets = MoveSetAnalyzer.AnalyzeMoves(goodra);

Console.Out.WriteLine(movesets.Count());