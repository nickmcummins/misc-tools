using PokemonTypeMovesetAnalyzer;
using PokemonTypeMovesetAnalyzer.Models;

var goodra = new Pokemon("Goodra");
var movesets = MoveSetAnalyzer.AnalyzeMoves(goodra);

Console.Out.WriteLine(movesets.Count());