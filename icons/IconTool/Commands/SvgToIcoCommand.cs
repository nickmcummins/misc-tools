using IconTool.Inkscape;
using IconTool.Models;

namespace IconTool.Commands
{
    public class SvgToIcoCommand : IIconToolCommand<(string svgFile, string icoFile)>
    {
        public static readonly int[] IcoSizes = [16, 24, 32, 48, 64, 128, 256];

        public void Handle((string svgFile, string icoFile) args)
        {
            var svgIcon = new SvgIcon(args.svgFile);
            var svgIconSizes = new SvgIconSizeVariants(svgIcon);

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

                InkscapeCli.ExportFile(svgIcon.FilePath, args.icoFile, width, height, tempDirectory.FullName, ExportFileType.Png);
            }
        }
    }
}
