using PokemonTypeMoveset.DataTool;
using PokemonTypeMovesetAnalyzer;
using static PokemonTypeMoveset.DataTool.FileDataProvider;


foreach (var pokemonName in PokemonLearnsets.Keys.Where(pokemonName => PokemonLearnsets[pokemonName].Any()))
{
    var movesets = MoveSetAnalyzer.AnalyzeMoves(pokemonName, PokemonLearnsets[pokemonName]
        .Where(moveName => MovesByName.ContainsKey(moveName))
        .Select(moveName => MovesByName[moveName]));
    if (movesets.Any())
    {
        var movesetsByTypeAdvances = movesets.ToGroupedDictionary(moveset => moveset.TypeAdvantages.ToListString());
        foreach (var movesetTypeAdvantage in movesetsByTypeAdvances.Keys)
        {
            var maxDamageMoveset = movesetsByTypeAdvances[movesetTypeAdvantage].OrderByDescending(moveset => moveset.Damage).First();
            Console.Out.WriteLine(maxDamageMoveset.ToCsv());
        }
    }
}