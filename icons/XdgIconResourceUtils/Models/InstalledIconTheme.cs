using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace XdgIconResourceUtils.Models
{
    public class InstalledIconTheme
    {
        private static readonly Regex IconThemeSizeFolderPattern = new Regex(@"^([0-9]+)x([0-9]+)$");

        public string Folder { get; init; }

        public IDictionary<string, InstalledIcon> Icons { get; init; }

        public InstalledIconTheme(string folder)
        {
            Folder = folder;
            Icons  = new Dictionary<string, InstalledIcon>();
            GetIcons(folder);
        }

        private void GetIcons(string iconThemeFolder)
        {
            foreach (var sizeFolder in Directory.GetDirectories(iconThemeFolder).Where(directoryName => IconThemeSizeFolderPattern.IsMatch(Path.GetFileName(directoryName))))
            {
                var size = sizeFolder.GetIconSize();
                foreach (var iconCategory in Directory.GetDirectories(sizeFolder))
                {
                    var context = Path.GetFileName(iconCategory);
                    foreach (var sizedIconFile in Directory.GetFiles(iconCategory))
                    {
                        var iconFilename = Path.GetFileName(sizedIconFile);
                        if (!Icons.TryGetValue(iconFilename, out var installedIcon))
                        {
                            installedIcon = new InstalledIcon(iconFilename);
                            Icons[iconFilename] = installedIcon;
                        }
                        installedIcon.AddVariant(new InstalledIconVariant(sizedIconFile, size, context));
                    }
                }
            }
        }

        public override string ToString()
        {
            return $"{Folder}\n{string.Join('\n', Icons.Values.Select(icon => icon.ToString()))}";
        }
    }
}