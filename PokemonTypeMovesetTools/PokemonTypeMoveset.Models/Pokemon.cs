using System.Collections.Generic;

namespace PokemonTypeMovesetAnalyzer.Models
{
    public class Pokemon
    {
        public string Name { get;  }
        public IEnumerable<Move> Moves { get; }

        public Pokemon(string name, IEnumerable<Move> moves)
        {
            Name = name;
            Moves = moves;
        }
    }
}
