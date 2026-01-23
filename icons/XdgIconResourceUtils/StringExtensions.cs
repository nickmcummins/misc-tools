using System.IO;
using System.Text.RegularExpressions;

namespace XdgIconResourceUtils
{
    public static class StringExtensions
    {
        private static readonly Regex IconSizeFolderPattern = new Regex(@"([0-9]+)x([0-9]+)");
        
        public static int GetIconSize(this string iconFilepath)
        {
            var sizeFolder = Path.GetFileName(Path.GetDirectoryName(Path.GetDirectoryName(iconFilepath)));
            return int.Parse(IconSizeFolderPattern.Match(sizeFolder).Groups[1].Value);
        }
    }
}