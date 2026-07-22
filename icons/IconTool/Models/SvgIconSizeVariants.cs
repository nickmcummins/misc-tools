using IconTool.Commands;

namespace IconTool.Models
{
    public class SvgIconSizeVariants
    {
        public string IconFilename { get; }

        public SortedDictionary<string, SvgIcon> Sizes { get; }

        public SvgIconSizeVariants(SvgIcon svgIcon)
        {
            IconFilename = Path.GetFileName(svgIcon.FilePath);
            Sizes = GetSizeVariants(svgIcon);
        }

        private static SortedDictionary<string, SvgIcon> GetSizeVariants(SvgIcon svgIcon)
        {
            var sizeVariants = new SortedDictionary<string, SvgIcon>(SvgIconSizeVariantsComparer.Instance);
            sizeVariants[svgIcon.Size] = svgIcon;
            var iconFilename = Path.GetFileName(svgIcon.FilePath);
            foreach (var sizeDirectory in Directory.GetDirectories(svgIcon.SizeParentDirectory).Except([svgIcon.SizeDirectory]).Where(dirName => !dirName.EndsWith("@2x")))
            {
                var sizedSvgFilePath = Path.Combine(sizeDirectory, svgIcon.Context.ToString().ToLower(), iconFilename);
                if (Path.Exists(sizedSvgFilePath))
                {
                    var sizedSvgIcon = new SvgIcon(sizedSvgFilePath);
                    sizeVariants[sizedSvgIcon.Size] = sizedSvgIcon;
                }
            }
            return sizeVariants;
        }

        public override string ToString()
        {
            return $"SvgIconSizeVariants\n{string.Join('\n', Sizes.Values.Select(v => string.Concat('\t', v.ToString())))}";
        }
    }
}
