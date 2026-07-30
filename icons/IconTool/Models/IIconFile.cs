namespace IconTool.Models
{
    public interface IIconFile
    {
        string FilePath { get; }
        string Size { get; }

        string ToString();
    }
}