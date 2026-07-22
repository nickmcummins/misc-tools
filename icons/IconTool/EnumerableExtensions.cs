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

        public static T FindFirstItemLargerThan<T>(this IEnumerable<T> list, T item) where T : IComparable<T>
        {
            T listItem;
            int i;
            for (i = 0; i < list.Count(); i++)
            {
                listItem = list.ElementAt(i);
                
                if (listItem.CompareTo(item) > 0)
                {
                    return listItem;
                }
            }

            listItem = list.ElementAt(i - 1);

            return listItem;
        }
    }
}
