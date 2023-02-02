using AngleSharp.Dom;
using HtmlAgilityPack;
using PokemonTypeMoveset.DataTool.Text.Json;
using System.Reflection;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text.RegularExpressions;

namespace PokemonTypeMoveset.DataTool
{
    public static class Extensions
    {
        private static readonly string Comma = ",";
        private static readonly Regex MultiSpace = new Regex(@"\s\s+", RegexOptions.Compiled);
        private static JsonSerializerOutputFormatOptions _jsonOptions;
        public static JsonSerializerOutputFormatOptions JsonOptions
        {
            get
            {
                if (_jsonOptions == null)
                {
                    _jsonOptions = new JsonSerializerOutputFormatOptions(new JsonSerializerOptions() { WriteIndented = true });
                    _jsonOptions.SerializerOptions.Converters.Add(new JsonStringEnumConverter());
                    _jsonOptions.ListOnSingleLine = true;
                }

                return _jsonOptions;
            }
        }

        #region XML/HTML Element
        public static IElement GetNextSiblingElementByTagname(this IElement element, string tagName)
        {
            do
            {
                IElement? currentSibling = element.NextElementSibling;
                if (currentSibling.TagName.ToLower() == tagName)
                {
                    return currentSibling;
                }
            } while (element.NextElementSibling != null);

            throw new Exception($"Could not find sibling {tagName} for <{element.TagName}>{element.TextContent}</{element.TagName}>.");
        }

        public static IEnumerable<HtmlNode> GetDescendantsByTagName(this HtmlNode htmlNode, string tagName)
        {
            return htmlNode.Descendants()
                .Where(descendant => descendant.NodeType == HtmlNodeType.Element && descendant.Name == tagName);
        }
        #endregion

        #region IEnumerable
        public static void AddRange<T>(this ISet<T> set, IEnumerable<T> additionalItems)
        {
            foreach (var additionalItem in additionalItems)
            {
                set.Add(additionalItem);
            }
        }

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

        public static IEnumerable<T> Select<T>(this IEnumerable<T> list, Func<T, T> selectorFirst, Func<T, T> selector, Func<T, T>? selectorLast = null)
        {
            var i = 0;
            var length = list.Count();
            return list.Select(item =>
            {
                T selected;
                if (i == 0) selected = selectorFirst(item);
                else if (selectorLast == null || i < length - 1) selected = selector(item);
                else selected = selectorLast(item);
                i++;
                return selected;
            });
        }

        public static IDictionary<TKey, IEnumerable<TSource>> ToGroupedDictionary<TSource, TKey>(this IEnumerable<TSource> source, Func<TSource, TKey> keySelector)
        {
            var grouping = source.GroupBy(keySelector);

            var dict = new Dictionary<TKey, IEnumerable<TSource>>();
            foreach (var group in grouping)
            {
                var groupKey = keySelector.Invoke(group.First());
                dict[groupKey] = group.Select(item => item).ToList();
            }
            return dict;
        }
        #endregion

        #region Assembly
        public static T ReadResource<T>(this Assembly assembly, string resourceFilename) => JsonSerializer.Deserialize<T>(assembly.ReadResourceAsString(resourceFilename), JsonOptions.SerializerOptions);

        public static string ReadResourceAsString(this Assembly assembly, string resourceFilename)
        {
            var fullResourcePath = assembly.GetManifestResourceNames()
                .FirstOrDefault(resourceName => resourceName.EndsWith(resourceFilename));

            using var streamReader = new StreamReader(assembly.GetManifestResourceStream(fullResourcePath));
            return streamReader.ReadToEnd();
        }
        #endregion

        #region String/JSON
        public static string ToString(this byte[] bytes, Encoding encoding) => encoding.GetString(bytes);

        public static string Replace(this string s, Regex regex, string replacement) => regex.Replace(s, replacement);

        public static string ToJson<T>(this T obj, JsonSerializerOutputFormatOptions? options = null)
        {
            options = options ?? JsonOptions;
            var jsonStr = Encoding.UTF8.GetString(JsonSerializer.SerializeToUtf8Bytes(obj, options.SerializerOptions));
            if (options.ListOnSingleLine)
            {
                var lines = jsonStr.Split("\r\n").Select(
                    firstLine => $"{firstLine}\n", 
                    line => line.Contains("]") ? $"{line}\n" : "\t" + line.Replace(MultiSpace, " ").Remove("\t").Trim(),
                    lastLine => lastLine);
                jsonStr = string.Join(string.Empty, lines);
            }
            return jsonStr;
        }

        public static string Remove(this string s, string substr)
        {
            return s.Replace(substr, string.Empty);
        }
        #endregion
    }
}
