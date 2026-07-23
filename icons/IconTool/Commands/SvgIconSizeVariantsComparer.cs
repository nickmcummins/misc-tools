namespace IconTool.Commands
{
    class SvgIconSizeVariantsComparer : IComparer<string>
    {
        public static readonly IComparer<string> Instance = new SvgIconSizeVariantsComparer();

        public int Compare(string x, string y)
        {
            if (!int.TryParse(x, out var intX))
            {
                intX = int.MaxValue;
            }

            if (!int.TryParse(y, out var intY))
            {
                intY = int.MaxValue;
            }
            return intX.CompareTo(intY);
        }
    }
}
