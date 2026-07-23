using IconTool.Commands;
using System.CommandLine;

namespace IconTool
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var rootCommand = new RootCommand("IconTool");
            
            var fillDefaultIcoSizes = new Option<bool>("--include-all-default-ico-sizes");
            
            var svgToIcoCommand = new Command("svg2ico", "Convert SVG to ICO");
            var svgArgument = new Argument<string>("svgfile");
            var icoArgument = new Argument<string>("icofile");
            svgToIcoCommand.Arguments.Add(svgArgument);
            svgToIcoCommand.Arguments.Add(icoArgument);
            svgToIcoCommand.Options.Add(fillDefaultIcoSizes);
            svgToIcoCommand.SetAction(parseResult =>
            {
                new SvgToIcoCommand().Handle((parseResult.GetValue(svgArgument), parseResult.GetValue(icoArgument), parseResult.GetValue(fillDefaultIcoSizes)));
            });
            rootCommand.Subcommands.Add(svgToIcoCommand);

            var bulkSvgToIcoCommand = new Command("bulksvg2ico", "Bulk convert SVGs to ICOs");
            var svgFolderArgument = new Argument<string>("svgfolder");
            var icoOutputFolderArgument = new Argument<string>("icooutputfolder");
            bulkSvgToIcoCommand.Arguments.Add(svgFolderArgument);
            bulkSvgToIcoCommand.Arguments.Add(icoOutputFolderArgument);
            bulkSvgToIcoCommand.Options.Add(fillDefaultIcoSizes);
            bulkSvgToIcoCommand.SetAction(parseResult =>
            {
                new BulkSvgToIcoCommand().Handle((parseResult.GetValue(svgFolderArgument), parseResult.GetValue(icoOutputFolderArgument), parseResult.GetValue(fillDefaultIcoSizes)));
            });
            rootCommand.Subcommands.Add(bulkSvgToIcoCommand);


            rootCommand.Parse(args).Invoke();
        }
    }
}
