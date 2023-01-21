using static PokemonTypeMovesetAnalyzer.Data;

namespace PokemonTypeMovesetAnalyzer.Models
{
    public class MoveSet
    {
        public string Pokemon { get; }
        public IEnumerable<Move> Moves { get; }
        public ISet<PokemonType> TypeAdvantages { get; }
        public IEnumerable<PokemonType> MissingTypeAdvantages { get; }

        public MoveSet(string pokemon, IEnumerable<string> moveNames) : this(pokemon, moveNames.Select(move => MovesByName[move])) { }

        public MoveSet(string pokemon, IEnumerable<Move> moves)
        {
            Pokemon = pokemon;
            Moves = moves;
            TypeAdvantages = new HashSet<PokemonType>(Moves.SelectMany(move => Strengths[move.MoveType]));
            MissingTypeAdvantages = Types.Where(t => !TypeAdvantages.Contains(t));
        }

        public override string ToString() => $"MoveSet(pokemon={Pokemon},moves={Moves.ToListString(move => move.Name)},typeAdvantages={TypeAdvantages.ToListString()},missingTypeAdvantages={MissingTypeAdvantages.ToListString()})";
    }
}
