using AngleSharp.Dom;
using AngleSharp.Html.Parser;
using System.Text.Json;
using TowerSoft.HtmlToExcel;

namespace PokemonTypeMoveset.DataTool
{
    public class PokemonLearnsetHtml : IPokemonLearnsetProvider
    {
        private static readonly string LearnsetByLevelUp = "Learnset by Leveling Up";
        private static readonly string LearnsetByTM = "Learnset by TM";
        private static readonly string LearnsetByEggGroup = "Learnset by Egg Group";
        private static readonly IEnumerable<string> LearnsetTables = new[] { LearnsetByLevelUp, LearnsetByTM, LearnsetByEggGroup };
        private static readonly HttpClient HttpClient = new HttpClient();

        public async Task<IEnumerable<string>> GetMoveNames(string pokemonName)
        {
            var moveNames = new HashSet<string>();
            Console.Out.WriteLine($"Downloading moves for {pokemonName} from {FileDataProvider.Pokemon[pokemonName]}.");
            var html = new HtmlParser().ParseDocument(await HttpClient.GetStringAsync(FileDataProvider.Pokemon[pokemonName]));
            var movesTablesH3 = html.QuerySelectorAll("h3").Where(h3 => LearnsetTables.Contains(h3.TextContent));
            foreach (var movesTableH3 in movesTablesH3)
            {
                var movesTableElement = movesTableH3.GetNextSiblingElementByTagname("table");
                var movesTableJson = new MovesHtmlTable(movesTableElement).ToJson();
                var tableMoveNames = JsonSerializer.Deserialize<IEnumerable<IDictionary<string, string>>>(movesTableJson)
                    .Select(moveObj => moveObj["Move"]);
                moveNames.AddRange(tableMoveNames);
                Console.Out.WriteLine($"\tAdded {tableMoveNames.Count()} moves.");
            }

            return moveNames;
        }

        class MovesHtmlTable : HtmlTable
        {
            private readonly IElement _movesTable;
            
            public MovesHtmlTable(IElement movesTable)
            {
                _movesTable = movesTable;
            }


            public override string ToXlsx()
            {
                var xlsxFilename = Path.GetTempFileName();
                byte[] xlsxData;
                using (WorkbookBuilder workbookBuilder = new WorkbookBuilder())
                {
                    workbookBuilder.AddSheet("Sheet1", _movesTable.OuterHtml);
                    xlsxData = workbookBuilder.GetAsByteArray();
                }

                File.WriteAllBytes(xlsxFilename, xlsxData);
                Console.Out.WriteLine($"Wrote {xlsxFilename} with size {xlsxData.Length} bytes.");
                return xlsxFilename;
            }
        }
    }

}
