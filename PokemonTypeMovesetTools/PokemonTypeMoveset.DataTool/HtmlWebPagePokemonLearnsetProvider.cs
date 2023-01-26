namespace PokemonTypeMoveset.DataTool
{
    public interface IPokemonLearnsetProvider
    {
        public Task<IEnumerable<string>> GetMoveNames(string pokemonName);
    }
}
