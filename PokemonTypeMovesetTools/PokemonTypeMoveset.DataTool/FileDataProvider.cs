using PokemonTypeMovesetAnalyzer.Models;
using System.Reflection;
using System.Text.Json;
using static System.IO.Path;

namespace PokemonTypeMoveset.DataTool
{
    public class FileDataProvider : IPokemonLearnsetProvider
    {
        public static readonly IDictionary<string, Move> MovesByName = Assembly.GetExecutingAssembly()
            .ReadResource<IEnumerable<Move>>("moves.json")
            .ToDictionary(move => move.Name);

        public static readonly IDictionary<string, string> Pokemon = Assembly.GetExecutingAssembly()
            .ReadResource<IEnumerable<IDictionary<string, string>>>("pokemon-list.json")
            .ToDictionary(pokemon => pokemon["name"], pokemon => pokemon["url"]);

        public static readonly IDictionary<string, IEnumerable<string>> PokemonLearnsets = Assembly.GetExecutingAssembly()
            .ReadResource<IDictionary<string, IEnumerable<string>>>("pokemon-learnsets.json");
            

        public static IPokemonLearnsetProvider Instance = new FileDataProvider();

        public async Task<IEnumerable<string>> GetMoveNames(string pokemonName)
        {
            return await Task.Run(() => PokemonLearnsets[pokemonName]);
        }

        public static void PersistLearnsetsDatastore()
        {
            var learnsetDatastoreFilepath = Path.Combine(new[] { GetDirectoryName(Assembly.GetExecutingAssembly().Location), "data", "pokemon-learnsets.json" });
            Console.Out.WriteLine($"Attempting to update {learnsetDatastoreFilepath}.");
            var learnsetJson = PokemonLearnsets.ToJson();
            File.WriteAllText(learnsetDatastoreFilepath, learnsetJson);
            Console.Out.WriteLine($"Wrote {learnsetDatastoreFilepath} with size {learnsetJson.Count()} bytes.");
        }
    }
}
