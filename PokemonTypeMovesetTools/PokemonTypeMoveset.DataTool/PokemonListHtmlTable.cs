using HtmlAgilityPack;
using PokemonTypeMoveset.DataTool;
using TowerSoft.HtmlToExcel;

namespace PokemonTypeMovesetDataTool
{
    public class PokemonListHtmlTable : HtmlTable
    {
        private readonly string _htmlFilename;
        private string _html;

        public PokemonListHtmlTable(string htmlFilename)
        {
            _htmlFilename = htmlFilename;
            _html = File.ReadAllText(_htmlFilename);
            Preprocess();
        }

        private void Preprocess()
        {
            var htmlDoc = new HtmlDocument();
            htmlDoc.LoadHtml(_html);

            var theadTr = htmlDoc.DocumentNode.SelectNodes("//thead/tr").First();
            theadTr.AppendChild(HtmlNode.CreateNode("<th role='columnheader'>Url</th>"));

            foreach (var tr in htmlDoc.DocumentNode.SelectNodes("//tbody/tr"))
            {
                var linkUrl = tr.GetDescendantsByTagName("a").First().Attributes["href"].Value;
                tr.AppendChild(HtmlNode.CreateNode($"<td>{linkUrl}</td>"));
            }
            var sw = new StringWriter();
            htmlDoc.Save(sw);
            _html = sw.GetStringBuilder().ToString();
        }

        public override string ToXlsx()
        {
            var xlsxFilename = _htmlFilename.Replace(".html", ".xlsx");
            byte[] xlsxData;
            using (WorkbookBuilder workbookBuilder = new WorkbookBuilder())
            {
                workbookBuilder.AddSheet("Sheet1", _html);
                xlsxData = workbookBuilder.GetAsByteArray();
            }

            File.WriteAllBytes(xlsxFilename, xlsxData);
            Console.Out.WriteLine($"Wrote {xlsxFilename} with size {xlsxData.Length} bytes.");
            return xlsxFilename;
        }
    }
}
