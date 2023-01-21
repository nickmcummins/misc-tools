using PokemonTypeMovesetAnalyzer.Models;
using System.Reflection;
using static PokemonTypeMovesetAnalyzer.Models.PokemonType;

namespace PokemonTypeMovesetAnalyzer
{
    public class Data
    {
        public static ISet<PokemonType> Types = new HashSet<PokemonType>(Enum.GetValues<PokemonType>().ToList());

        public static readonly IDictionary<PokemonType, IEnumerable<PokemonType>> Strengths = new Dictionary<PokemonType, IEnumerable<PokemonType>>(new KeyValuePair<PokemonType, IEnumerable<PokemonType>>[] {
            new(Bug, new [] { Grass, Dark, Psychic }),
            new(Dark, new[] { Ghost, Psychic }),
            new(Dragon, new[] { Dragon }),
            new(Electric, new[] { Flying, Water }),
            new(Fairy, new[] { Fighting, Dark, Dragon }),
            new(Fighting, new[] { Dark, Ice, Normal, Rock, Steel }),
            new(Fire, new[] { Bug, Grass, Ice, Steel }),
            new(Flying, new[] { Bug, Fighting, Grass }),
            new(Ghost, new[] { Ghost, Psychic }),
            new(Grass, new[] { Ground, Rock, Water }),
            new(Ground, new[] {Electric, Fire, Poison, Rock, Steel }),
            new(Ice, new[] { Dragon, Flying, Grass, Ground }),
            new(Normal, Enumerable.Empty<PokemonType>()),
            new(Poison, new[] { Fairy, Grass }),
            new(Psychic, new[] { Fighting, Poison }),
            new(Rock, new[] { Bug, Fire, Flying, Ice }),
            new(Steel, new[] { Fairy, Ice, Rock }),
            new(Water, new[] { Fire, Ground, Rock })
        });

        public static readonly IDictionary<PokemonType, IEnumerable<PokemonType>> Weaknesses = new Dictionary<PokemonType, IEnumerable<PokemonType>>(new KeyValuePair<PokemonType, IEnumerable<PokemonType>>[] {
            new(Bug, new [] { Fire, Flying, Rock }),
            new(Dark, new[] { Bug, Fairy, Fighting }),
            new(Dragon, new[] { Dragon, Fairy, Ice }),
            new(Electric, new[] { Ground }),
            new(Fairy, new[] { Poison, Steel }),
            new(Fighting, new[] { Fairy, Flying, Psychic }),
            new(Fire, new[] { Ground, Rock, Water }),
            new(Flying, new[] { Electric, Ice, Rock }),
            new(Ghost, new[] { Dark, Ghost }),
            new(Grass, new[] { Bug, Fire, Flying, Ice, Poison }),
            new(Ground, new[] { Grass, Ice, Water }),
            new(Ice, new[] { Fighting, Fire, Rock, Steel }),
            new(Normal, new[] { Fighting }),
            new(Poison, new[] { Ground, Psychic }),
            new(Psychic, new[] { Bug, Dark, Ghost }),
            new(Rock, new[] { Fighting, Grass, Ground, Steel, Water }),
            new(Steel, new[] { Fighting, Fire, Ground }),
            new(Water, new[] { Electric, Grass })
        });


        public static readonly IDictionary<string, Move> MovesByName = Assembly.GetExecutingAssembly()
            .ReadResource<IEnumerable<Move>>("VioletScarletMoves.json")
            .ToDictionary(move => move.Name);
    }
}
