using PokemonTypeMoveset.DataTool;
using static PokemonTypeMoveset.DataTool.FileDataProvider;
using static PokemonTypeMovesetAnalyzer.TypesChart;

namespace PokemonTypeMovesetAnalyzer.Models
{
    public class MoveSet
    {
        public string Pokemon { get; }
        public IEnumerable<Move> Moves { get; }
        public ISet<PokemonType> TypeAdvantages { get; }
        public IEnumerable<PokemonType> MissingTypeAdvantages { get; }
        public int Damage { get; }

        public MoveSet(string pokemon, IEnumerable<string> moveNames) : this(pokemon, moveNames.Select(move => MovesByName[move])) { }

        public MoveSet(string pokemon, IEnumerable<Move> moves)
        {
            Pokemon = pokemon;
            Moves = moves;
            TypeAdvantages = new SortedSet<PokemonType>(Moves.SelectMany(move => Strengths[move.MoveType]));
            MissingTypeAdvantages = Types.Where(t => !TypeAdvantages.Contains(t));
            Damage = Moves.Sum(move => move.Power.GetValueOrDefault());
        }

        public override string ToString() => $"MoveSet(pokemon={Pokemon},moves={Moves.ToListString(move => move.Name)},typeAdvantages({TypeAdvantages.Count()})={TypeAdvantages.ToListString()},missingTypeAdvantages({MissingTypeAdvantages.Count()})={MissingTypeAdvantages.ToListString()})";

        public string ToCsv() => $"{Pokemon}\t{Moves.ToListString(move => move.Name)}\t{Damage}\t{TypeAdvantages.Count()}\t{TypeAdvantages.ToListString()}\t{MissingTypeAdvantages.Count()}\t{MissingTypeAdvantages.ToListString()}";
    }
}
