using Svg;

namespace IconTool.Models
{
    public class SvgIcon
    {
        private readonly string[] _directoryPath;

        public string FilePath { get; }
        public IconContext Context { get; }
        public string Size { get; }
        public string SizeDirectory { get; }
        public string SizeParentDirectory { get; }

        private SvgDocument _svgDocument;
        public SvgDocument SvgDocument { 
            get { if (_svgDocument == null) _svgDocument = Svg.SvgDocument.Open(FilePath); return _svgDocument; } 
        }

        public SvgIcon(string filePath)
        {
            FilePath = filePath;
            _directoryPath = Path.GetDirectoryName(filePath).Split(Path.DirectorySeparatorChar);
            if (_directoryPath[_directoryPath.Length - 1] == "scalable" || int.TryParse(_directoryPath[_directoryPath.Length - 1], out _))
            {
                Context = Enum.TryParse<IconContext>(_directoryPath[_directoryPath.Length - 2], true, out var context) ? context : IconContext.Default;
                Size = _directoryPath[_directoryPath.Length - 1];
                SizeDirectory = string.Join(Path.DirectorySeparatorChar, _directoryPath.Take(_directoryPath.Length));
                SizeParentDirectory = string.Join(Path.DirectorySeparatorChar, _directoryPath.Take(_directoryPath.Length - 1));
            }
            else
            {
                Context = Enum.TryParse<IconContext>(_directoryPath[_directoryPath.Length - 1], true, out var context) ? context : IconContext.Default;
                Size = _directoryPath[_directoryPath.Length - 2];
                SizeDirectory = string.Join(Path.DirectorySeparatorChar, _directoryPath.Take(_directoryPath.Length - 1));
                SizeParentDirectory = string.Join(Path.DirectorySeparatorChar, _directoryPath.Take(_directoryPath.Length - 2));
            }
        }

        public override string ToString() => $"SvgIcon {FilePath}\n\tContext={Context}\n\tSize={Size},\n\t{nameof(SizeParentDirectory)}={SizeParentDirectory}";
    }
}
