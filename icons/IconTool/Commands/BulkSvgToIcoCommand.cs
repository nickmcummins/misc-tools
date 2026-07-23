namespace IconTool.Commands;

public class BulkSvgToIcoCommand : IIconToolCommand<(string svgFolder, string outputFolder, bool includeAllDefaultIcoSizes)>
{
    private static readonly SvgToIcoCommand SvgToIco = new();
    
    public void Handle((string svgFolder, string outputFolder, bool includeAllDefaultIcoSizes) args)
    {
        if (!Directory.Exists(args.outputFolder))
        {
            Console.Out.WriteLine($"Output folder {args.outputFolder} does not exist, creating it.");
            Directory.CreateDirectory(args.outputFolder);
        }
        
        foreach (var svgIcon in new DirectoryInfo(args.svgFolder).EnumerateFiles("*.svg", SearchOption.AllDirectories).Where(fileinfo => !fileinfo.IsSymlink()))
        {
            Console.Out.WriteLine($"Converting icon {svgIcon}.");
            var iconName = Path.GetFileNameWithoutExtension(svgIcon.Name);
            SvgToIco.Handle((svgIcon.FullName, $"{args.outputFolder}{Path.DirectorySeparatorChar}{iconName}{iconName}.ico", args.includeAllDefaultIcoSizes));
        }
    }
}