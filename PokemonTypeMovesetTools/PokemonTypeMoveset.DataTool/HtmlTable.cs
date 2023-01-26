using Rochas.ExcelToJson;

namespace PokemonTypeMoveset.DataTool
{
    public abstract class HtmlTable
    {

        public abstract string ToXlsx();

        public string ToJson()
        {
            var xlsxFilename = ToXlsx();
            return ExcelToJsonParser.GetJsonStringFromTabular(new FileStream(xlsxFilename, FileMode.OpenOrCreate));
        }
    }
}
