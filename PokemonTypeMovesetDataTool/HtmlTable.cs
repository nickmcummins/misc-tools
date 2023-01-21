using AngleSharp.Html.Dom;
using HtmlAgilityPack;
using Rochas.ExcelToJson;
using TowerSoft.HtmlToExcel;

namespace PokemonTypeMovesetDataTool
{
    public class HtmlTable
    {
        private readonly string _htmlFilename;
        private string _html;
        private string _xlsxFilename;

        public HtmlTable(string htmlFilename)
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
                var linkUrl = tr.SelectSingleNode("//a").Attributes["href"].Value;
                tr.AppendChild(HtmlNode.CreateNode($"<td>{linkUrl}</td>"));
            }
            var sw = new StringWriter();
            htmlDoc.Save(sw);
            _html = sw.GetStringBuilder().ToString();
        }

        public void ToXlsx()
        {
            _xlsxFilename = _htmlFilename.Replace(".html", ".xlsx");
            byte[] xlsxData;
            using (WorkbookBuilder workbookBuilder = new WorkbookBuilder())
            {
                workbookBuilder.AddSheet("Sheet1", _html);
                xlsxData = workbookBuilder.GetAsByteArray();
            }

            File.WriteAllBytes(_xlsxFilename, xlsxData);
            Console.Out.WriteLine($"Wrote {_xlsxFilename} with size {xlsxData.Length} bytes.");
        }

        public string ToJson()
        {
            ToXlsx();
            return ExcelToJsonParser.GetJsonStringFromTabular(new FileStream(_xlsxFilename, FileMode.OpenOrCreate));
        }
    }
}
