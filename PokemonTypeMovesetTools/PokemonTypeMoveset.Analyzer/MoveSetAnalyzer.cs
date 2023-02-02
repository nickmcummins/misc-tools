using PokemonTypeMoveset.DataTool;
using PokemonTypeMovesetAnalyzer.Models;

namespace PokemonTypeMovesetAnalyzer
{
    public class MoveSetAnalyzer
    {
        public static IList<MoveSet> AnalyzeMoves(string pokemonName, IEnumerable<Move> pokemonMoves)
        {
            var damagingMoves = pokemonMoves.Where(move => move.Power >= 70);
            return damagingMoves.CombinationsOfK(4)
                .Select(moves => new MoveSet(pokemonName, moves))
                .OrderByDescending(moveSet => moveSet.TypeAdvantages.Count())
                .ToList();
        }

        public static IList<MoveSet> AnalyzeMoves(Pokemon pokemon)
        {
            return AnalyzeMoves(pokemon.Name, pokemon.Moves);
        }
    }
}
