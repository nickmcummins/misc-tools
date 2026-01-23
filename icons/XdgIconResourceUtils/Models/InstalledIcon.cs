using System.Collections.Generic;
using System.IO;

namespace XdgIconResourceUtils.Models
{
    public class InstalledIcon
    {
        public string Filename { get; init; }

        public IDictionary<int, string> Sizes { get; init; }

        public InstalledIcon(string iconFilepath)
        {
            Filename = Path.GetFileName(iconFilepath);
            Sizes = new Dictionary<int, string>();
            AddSize(iconFilepath);
        }

        public void AddSize(string iconFilepath)
        {
            Sizes[iconFilepath.GetIconSize()] =  iconFilepath;
        }

        public override string ToString()
        {
            return $"{Filename} [sizes: {string.Join(',', Sizes.Keys)}]";
        }
    }
}