using System.Text.Json.Serialization;

namespace PokemonTypeMovesetAnalyzer.Models
{
    public class Move
    {
        [JsonPropertyName("Move")]
        public string Name { get; set; }

        [JsonPropertyName("Type")]
        public PokemonType MoveType { get; set; }

        public short? Power { get; set; }
        public short Accuracy { get; set; }
        public short PP { get; set; }
        public string Effect { get; set; }
        public MoveCategory Category { get; set; }

        public Move() { }
        public Move(string name) { Name = name; }

        public override string ToString() => $"Move(name={Name},type={MoveType},power={Power},category={Category})";

    }
}
