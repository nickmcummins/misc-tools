using System.Text.RegularExpressions;

namespace XdgIconResourceUtils
{
    public static class StringExtensions
    {
        private static readonly Regex IconSizeFolderPattern = new Regex(@"([0-9]+)x([0-9]+)");
        
        public static int GetIconSize(this string sizeFolder)
        {
            return int.Parse(IconSizeFolderPattern.Match(sizeFolder).Groups[1].Value);
        }

        public static string QuoteIfNeeded(this string str)
        {
            if (str.Contains(' '))
            {
                return $"\"{str}\"";
            }
            return str;
        }
    }
}