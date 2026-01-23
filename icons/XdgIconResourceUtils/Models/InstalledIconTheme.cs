using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace XdgIconResourceUtils.Models
{
    public class InstalledIconTheme
    {
        public string Folder { get; init; }

        public IDictionary<string, InstalledIcon> Icons { get; init; }

        public InstalledIconTheme(string folder)
        {
            Folder = folder;
            Icons  = new  Dictionary<string, InstalledIcon>();
            GetIcons(folder);
        }

        private void GetIcons(string iconThemeFolder)
        {
            var sizeFolders = Directory.GetDirectories(iconThemeFolder).Where(directoryName => Regex.IsMatch(Path.GetFileName(directoryName), @"^([0-9]+)x([0-9]+)$")).ToList();
            foreach (var sizeFolder in sizeFolders)
            {
                Console.Out.WriteLine(sizeFolder);
                foreach (var iconCategory in Directory.GetDirectories(sizeFolder))
                {
                    foreach (var sizedIconFile in Directory.GetFiles(iconCategory))
                    {
                        var iconFilename = Path.GetFileName(sizedIconFile);
                        if (!Icons.TryGetValue(iconFilename, out var installedIcon))
                        {
                            installedIcon = new InstalledIcon(sizedIconFile);
                            Icons[iconFilename] = installedIcon;
                        }
                        else
                        {
                            installedIcon.AddSize(sizedIconFile);
                        }
                    }
                }
            }
        }
    }
}