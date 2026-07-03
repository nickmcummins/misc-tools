namespace IconTool.Models
{
    public class SvgIconSizeVariants
    {
        public static readonly int[] IcoSizes = [16, 24, 32, 48, 64, 128, 256];

        public string IconFilename { get; }

        public IDictionary<string, SvgIcon> Sizes { get; }

        public SvgIconSizeVariants(SvgIcon svgIcon)
        {
            IconFilename = Path.GetFileName(svgIcon.FilePath);
            Sizes = GetSizeVariants(svgIcon);
        }

        private static IDictionary<string, SvgIcon> GetSizeVariants(SvgIcon svgIcon)
        {
            var sizeVariants = new Dictionary<string, SvgIcon>();
            sizeVariants[svgIcon.Size] = svgIcon;
            var iconFilename = Path.GetFileName(svgIcon.FilePath);
            foreach (var sizeDirectory in Directory.GetDirectories(svgIcon.SizeParentDirectory).Except([svgIcon.SizeDirectory]))
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
