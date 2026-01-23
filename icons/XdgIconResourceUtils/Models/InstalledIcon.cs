using System.Collections.Generic;
using System.Linq;

namespace XdgIconResourceUtils.Models
{
    public class InstalledIcon
    {
        public string IconFilename { get; init; }
        public IDictionary<(int size, string context), InstalledIconVariant> Variants { get; init; }

        public InstalledIcon(string iconFilename)
        {
            IconFilename = iconFilename;
            Variants = new Dictionary<(int size, string context), InstalledIconVariant>();
        }

        public void AddVariant(InstalledIconVariant iconVariant)
        {

            Variants[(iconVariant.Size, iconVariant.Context)] = iconVariant;
        }

        public override string ToString()
        {
            return string.Concat($"{IconFilename}\n", string.Join("\n", Variants.Values.Select(variant => string.Concat("\t", variant.ToString()))));
        }
    }
}
