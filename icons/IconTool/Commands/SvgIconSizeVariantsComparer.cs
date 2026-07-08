namespace IconTool.Commands
{
    class SvgIconSizeVariantsComparer : IComparer<string>
    {
        public static readonly IComparer<string> Instance = new SvgIconSizeVariantsComparer();

        public int Compare(string x, string y)
        {
            return int.Parse(x).CompareTo(int.Parse(y));
        }
    }
}
