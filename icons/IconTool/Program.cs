using IconTool.Models;
using System.CommandLine;

namespace IconTool
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var rootCommand = new RootCommand("IconTool");
            var svgToIcoCommand = new Command("svg2ico", "Convert SVG to ICO");

            var svgArgument = new Argument<string>("svgfile");
            var icoArgument = new Argument<string>("icofile");

            svgToIcoCommand.Arguments.Add(svgArgument);
            svgToIcoCommand.Arguments.Add(icoArgument);

            var keyOption = new Option<string>("--key")
            {
                Description = "Configuration key"
            };

            svgToIcoCommand.Options.Add(keyOption);
            svgToIcoCommand.SetAction(parseResult =>
            {
                var svgIcon = new SvgIcon(parseResult.GetValue(svgArgument));
                Console.Out.WriteLine(svgIcon);
                var svgIconSizes = new SvgIconSizeVariants(svgIcon);
                Console.Out.WriteLine(svgIconSizes);
            });
            
            rootCommand.Subcommands.Add(svgToIcoCommand);

            rootCommand.Parse(args).Invoke();
        }
    }
}
