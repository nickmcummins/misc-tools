namespace IconTool
{
    public static class EnumerableExtensions
    {
        public static void Sort<T>(this IList<T> list, IComparer<T>? comparer = null)
        {
            if (list is List<T> listTypeList)
            {
                listTypeList.Sort(comparer);
            }
            else if (list is T[] array)
            {
                Array.Sort(array, comparer);
            }
            else
            {
                throw new NotSupportedException($"{nameof(EnumerableExtensions)}.{nameof(Sort)} is not supported for type {list.GetType().Name}.");
            }
        }
    }
}
