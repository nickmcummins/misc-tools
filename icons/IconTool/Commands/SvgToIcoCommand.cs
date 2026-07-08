using IconTool.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IconTool.Commands
{
    public class SvgToIcoCommand : IIconToolCommand<(string svgFile, string icoFile)>
    {
        public static readonly int[] IcoSizes = [16, 24, 32, 48, 64, 128, 256];

        public void Handle((string svgFile, string icoFile) args)
        {
            var svgIcon = new SvgIcon(args.svgFile);
            var svgIconSizes = new SvgIconSizeVariants(svgIcon);
            var sizes = new SortedList<string>(0, SvgIconSizeVariantsComparer.Instance);
            sizes.UnionWith(svgIconSizes.Sizes.Keys);
            sizes.Sort();

        }
    }
}
