using System;
using System.CommandLine;
using XdgIconResourceUtils.Models;

namespace XdgIconResourceUtils
{
    class Program
    {
        static void Main(string[] args)
        {
            var xdgIconResourceUtilsCommand = new RootCommand();

            var themeOption = new Option<string>("theme", "--theme");            
            var sizeOption = new Option<string>("size", "--size");

            var listCommand = new Command("list");
            listCommand.Options.Add(themeOption);
            listCommand.Options.Add(sizeOption);

            listCommand.SetAction((parseResult) =>
            {
                var theme = parseResult.GetValue(themeOption);
                var size = parseResult.GetValue(sizeOption);
                var iconTheme = new InstalledIconTheme(theme);
                Console.Out.WriteLine(iconTheme);
            });
            xdgIconResourceUtilsCommand.Add(listCommand);

            var uninstallCommand = new Command("uninstall");
            uninstallCommand.Options.Add(themeOption);
            uninstallCommand.Options.Add(sizeOption);

            uninstallCommand.SetAction((parseResult) =>
            {
                var theme = parseResult.GetValue(themeOption);
                var iconTheme = new InstalledIconTheme(theme);
                var icons = iconTheme.Icons.Values;
                foreach (var icon in icons)
                {
                    foreach (var iconVariant in icon.Variants.Values)
                    {
                        Console.Out.WriteLine($"xdg-icon-resource uninstall --size {iconVariant.Size} --context {iconVariant.Context} {icon.IconFilename.QuoteIfNeeded()}");
                    }
                }
            });
            xdgIconResourceUtilsCommand.Add(uninstallCommand);

            xdgIconResourceUtilsCommand.Parse(args).Invoke();
        }       
    }
}
