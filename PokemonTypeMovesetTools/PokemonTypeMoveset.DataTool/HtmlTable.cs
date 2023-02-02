using Rochas.ExcelToJson;

namespace PokemonTypeMoveset.DataTool
{
    public abstract class HtmlTable
    {

        public abstract string ToXlsx();

        public string ToJson()
        {
            var xlsxFilename = ToXlsx();
            var jsonFilename = xlsxFilename.Replace(".xlsx", ".json");
            var jsonStr = ExcelToJsonParser.GetJsonStringFromTabular(new FileStream(xlsxFilename, FileMode.OpenOrCreate));
            File.WriteAllText(jsonFilename, jsonStr);
            Console.Out.WriteLine($"Wrote {jsonFilename} of length {jsonStr.Length}.");

            return jsonStr;
        }
    }
}
