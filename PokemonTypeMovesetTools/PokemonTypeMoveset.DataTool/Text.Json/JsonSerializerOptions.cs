namespace PokemonTypeMoveset.DataTool.Text.Json
{
    public class JsonSerializerOutputFormatOptions
    {
        public System.Text.Json.JsonSerializerOptions SerializerOptions { get; }
        public bool ListOnSingleLine { get; set; }

        public JsonSerializerOutputFormatOptions(System.Text.Json.JsonSerializerOptions jsonSerializerOptions)
        {
            SerializerOptions = jsonSerializerOptions;
        }
    }
}
