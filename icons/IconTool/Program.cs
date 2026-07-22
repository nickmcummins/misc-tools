using IconTool.Commands;
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

            var fillDefaultIcoSizes = new Option<bool>("--include-all-default-ico-sizes");
            svgToIcoCommand.Options.Add(fillDefaultIcoSizes);

            svgToIcoCommand.SetAction(parseResult =>
            {
                //var svgIcon = new SvgIcon(parseResult.GetValue(svgArgument));
                //Console.Out.WriteLine(svgIcon);
                //var svgIconSizes = new SvgIconSizeVariants(svgIcon);
                //Console.Out.WriteLine(svgIconSizes);
                new SvgToIcoCommand().Handle((parseResult.GetValue(svgArgument), parseResult.GetValue(icoArgument), parseResult.GetValue(fillDefaultIcoSizes)));
            });
            
            rootCommand.Subcommands.Add(svgToIcoCommand);

            rootCommand.Parse(args).Invoke();
        }
    }
}
