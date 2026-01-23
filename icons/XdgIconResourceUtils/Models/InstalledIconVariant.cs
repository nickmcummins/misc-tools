using System.IO;

namespace XdgIconResourceUtils.Models
{
    public class InstalledIconVariant
    {
        public FileInfo File { get; set; }

        public int Size { get; set; }

        public string Context { get; set; }

        public InstalledIconVariant(string iconFilepath, int size, string context) 
        {
            File = new FileInfo(iconFilepath);
            Size = size;
            Context = context;
        }

        public override string ToString()
        {
            return $"{File.Name} --size {Size} --context {Context}";
        }
    }
}