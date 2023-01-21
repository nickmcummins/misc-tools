using System.Reflection;
using static PokemonTypeMovesetAnalyzer.Data;

namespace PokemonTypeMovesetAnalyzer.Models
{
    public class Pokemon
    {
        public string Name { get;  }
        public IEnumerable<Move> Moves { get; }
        public Pokemon(string name)
        {
            Name = name;
            var moveNames = Assembly.GetExecutingAssembly().ReadResource<IDictionary<string, IEnumerable<string>>>($"{Name}.json")["Moves"];
            Moves = moveNames.Select(moveName => MovesByName.TryGetValue(moveName, out var move) ? move : new Move(moveName));
        }
    }
}
