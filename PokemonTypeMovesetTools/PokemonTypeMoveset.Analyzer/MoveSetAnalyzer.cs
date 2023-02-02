using PokemonTypeMoveset.DataTool;
using PokemonTypeMovesetAnalyzer.Models;

namespace PokemonTypeMovesetAnalyzer
{
    public class MoveSetAnalyzer
    {
        public static IList<MoveSet> AnalyzeMoves(Pokemon pokemon)
        {
            var damagingMoves = pokemon.Moves.Where(move => move.Power >= 70);
            return damagingMoves.CombinationsOfK(4)
                .Select(moves => new MoveSet(pokemon.Name, moves))
                .OrderByDescending(moveSet => moveSet.TypeAdvantages.Count())
                .ToList();
        }
    }
}
