using PokemonTypeMovesetDataTool;
using System.Linq;

namespace PokemonTypeMoveset.DataTool
{
    public static class CommandHandlers
    {
        public static async Task DownloadPokemonMoveset(string? pokemonName = null, bool redownloadExisting = false)
        {
            var pokemonList = pokemonName != null ? new[] { pokemonName } : FileDataProvider.Pokemon.Keys;
            var i = 0; 
            foreach (var pokemon in pokemonList.Where(pokemon => redownloadExisting || !FileDataProvider.PokemonLearnsets.ContainsKey(pokemon)))
            {
                var moveNames = await new PokemonLearnsetHtml().GetMoveNames(pokemon);
                FileDataProvider.PokemonLearnsets[pokemon] = moveNames;
                if (i % 10 == 0)
                {
                    FileDataProvider.PersistLearnsetsDatastore("pokemon-learnsets.json.tmp");
                }
                i++;
            }
            FileDataProvider.PersistLearnsetsDatastore();
        }

        public static void ParsePokemonList(string source)
        {
            var pokemonListHtmlTable = new PokemonListHtmlTable(source);
            pokemonListHtmlTable.ToJson();
        }
    }
}
