using PokemonTypeMovesetDataTool;

namespace PokemonTypeMoveset.DataTool
{
    public static class CommandHandlers
    {
        public static async Task DownloadPokemonMoveset(string pokemonName)
        {
            var moveNames = await new PokemonLearnsetHtml().GetMoveNames(pokemonName);
            FileDataProvider.PokemonLearnsets[pokemonName] = moveNames;
            FileDataProvider.PersistLearnsetsDatastore();
        }

        public static void ParsePokemonList(string source)
        {
            var pokemonListHtmlTable = new PokemonListHtmlTable(source);
            pokemonListHtmlTable.ToJson();
        }
    }
}
