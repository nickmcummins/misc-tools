using IconTool.ImageMagick;
using IconTool.Inkscape;
using IconTool.Models;

namespace IconTool.Commands
{
    public class SvgToIcoCommand : IIconToolCommand<(string svgFile, string icoFile, bool includeAllDefaultIcoSizes)>
    {
        public static readonly int[] IcoSizes = [16, 24, 32, 48, 64, 128, 256];

        private static readonly IDictionary<int, int> ResizeSizes = new Dictionary<int, int>()
        {
            { 32, 24 },
            { 20, 24 }
        };

        private static readonly IDictionary<string, int> ObjectGeometryCache = new Dictionary<string, int>();

        public void Handle((string svgFile, string icoFile, bool includeAllDefaultIcoSizes) args)
        {
            var svgIcon = new SvgIcon(args.svgFile);
            var svgIconSizes = new SvgIconSizeVariants(svgIcon);

            var exportedPngs = new SortedDictionary<int, string>();
            var tempDirectory = Directory.CreateTempSubdirectory();
            foreach (var sizeKvp in svgIconSizes.Sizes)
            {
                int width;
                int height;
                if (!int.TryParse(sizeKvp.Key, out var size))
                {
                    var objectGeometry = InkscapeCli.QueryObjectGeometry(svgIcon.FilePath, [QueryObjectGeometryProperty.Width, QueryObjectGeometryProperty.Height]);
                    width = objectGeometry.Width.Value;
                    height = objectGeometry.Height.Value;
                } 
                else
                {
                    width = size;
                    height = size;
                }

                var exportedFilename = InkscapeCli.ExportFile(sizeKvp.Value.FilePath, args.icoFile, width, height, tempDirectory.FullName, ExportFileType.Png);
                exportedPngs[width] = exportedFilename;
            }

            if (args.includeAllDefaultIcoSizes)
            {
                foreach (var icoSize in IcoSizes.Where(icoSize => !exportedPngs.ContainsKey(icoSize)))
                {
                    var sourceSizeSvg = GetSizeSourceSvg(icoSize, svgIconSizes);
                    var exportedFilename = InkscapeCli.ExportFile(sourceSizeSvg, args.icoFile, icoSize, icoSize, tempDirectory.FullName, ExportFileType.Png);
                    exportedPngs[icoSize] = exportedFilename;
                }
                
            }
            
            ImageMagickCli.Convert(exportedPngs.Values, args.icoFile);
        }

        private static string GetSizeSourceSvg(int iconSize, SvgIconSizeVariants svgIconSizes)
        {
            var availableSizes = svgIconSizes.Sizes.Keys.ToDictionary(size =>
            {
                if (!int.TryParse(size, out var intSize) && !ObjectGeometryCache.TryGetValue(svgIconSizes.Sizes[size].FilePath, out intSize))
                {
                    var objectGeometry = InkscapeCli.QueryObjectGeometry(svgIconSizes.Sizes[size].FilePath,
                        [QueryObjectGeometryProperty.Width, QueryObjectGeometryProperty.Height]);
                    intSize = objectGeometry.Width.Value;
                    ObjectGeometryCache[svgIconSizes.Sizes[size].FilePath] = intSize;
                }

                return intSize;
            }, size => svgIconSizes.Sizes[size.ToString()].FilePath);

            int sourceSize;
            if (ResizeSizes.TryGetValue(iconSize, out var resizeSourceSize) && availableSizes.ContainsKey(resizeSourceSize))
            {
                sourceSize = resizeSourceSize;
            }
            else
            {
                sourceSize = availableSizes.Keys.FindFirstItemLargerThan(iconSize);
            }
            return availableSizes[sourceSize];
        }
    }
}
