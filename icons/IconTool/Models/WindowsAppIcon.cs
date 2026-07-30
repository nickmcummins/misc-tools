using System.Text.RegularExpressions;

namespace IconTool.Models
{
    public class WindowsAppIcon : IIconFile
    {
        public static readonly Regex FilenamePattern = new(@"^(?<prefix>.+?)\.targetsize-(?<size>\d+)_(?<suffix>[^.]+)\.(?<ext>\w+)$", RegexOptions.Compiled);

        public string FilePath { get; }

        public string Pattern { get; }

        public string Size { get; }

        public WindowsAppIcon(string filePath)
        {
            FilePath = filePath;
            var fileName = Path.GetFileNameWithoutExtension(filePath);
            var match = FilenamePattern.Match(fileName);
            Size = match.Groups["size"].Value;

            var prefix = match.Groups["prefix"].Value;
            var suffix = match.Groups["suffix"].Value;
            Pattern = $"{prefix}.targetsize-%SIZE%{suffix}";
        }

        public override string ToString() => $"WindowsAppIcon {FilePath}\n\tSize={Size},\n\tPattern={Pattern}";
    }
}
