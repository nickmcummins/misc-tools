using PokemonTypeMovesetAnalyzer.Models;
using System.Reflection;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace PokemonTypeMovesetAnalyzer
{
    public static class Extensions
    {
        private static readonly string Comma = ",";

        public static string ToString(this byte[] bytes, Encoding encoding) => encoding.GetString(bytes);

        public static string ToJson<T>(this T obj) => JsonSerializer.SerializeToUtf8Bytes(obj).ToString(Encoding.UTF8);

        public static string ToListString<T>(this IEnumerable<T> list, Func<T, string> toStrFunc = null) => $"[{string.Join(Comma, list.Select(item => toStrFunc != null ? toStrFunc(item) : item.ToString()))}]";

        public static IEnumerable<IEnumerable<T>> CombinationsOfK<T>(this IEnumerable<T> data, int k)
        {
            int size = data.Count();

            IEnumerable<IEnumerable<T>> Runner(IEnumerable<T> list, int n)
            {
                int skip = 1;
                foreach (var headList in list.Take(size - k + 1).Select(h => new T[] { h }))
                {
                    if (n == 1)
                        yield return headList;
                    else
                    {
                        foreach (var tailList in Runner(list.Skip(skip), n - 1))
                        {
                            yield return headList.Concat(tailList);
                        }
                        skip++;
                    }
                }
            }

            return Runner(data, k);
        }
    }
}
