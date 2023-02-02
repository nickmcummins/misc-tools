using AngleSharp.Dom;
using PokemonTypeMoveset.DataTool.Text.Json;
using System.Reflection;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace PokemonTypeMoveset.DataTool
{
    public static class Extensions
    {
        private static readonly string Comma = ",";

        private static JsonSerializerOutputFormatOptions _jsonOptions;
        public static JsonSerializerOutputFormatOptions JsonOptions
        {
            get
            {
                if (_jsonOptions == null)
                {
                    _jsonOptions = new JsonSerializerOutputFormatOptions(new JsonSerializerOptions());
                    _jsonOptions.SerializerOptions.Converters.Add(new JsonStringEnumConverter());
                    _jsonOptions.ListOnSingleLine = true;
                }

                return _jsonOptions;
            }
        }

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

        public static void AddRange<T>(this ISet<T> set, IEnumerable<T> additionalItems)
        {
            foreach (var additionalItem in additionalItems)
            {
                set.Add(additionalItem);
            }
        }


        public static T ReadResource<T>(this Assembly assembly, string resourceFilename) => JsonSerializer.Deserialize<T>(assembly.ReadResourceAsString(resourceFilename), JsonOptions.SerializerOptions);

        public static string ReadResourceAsString(this Assembly assembly, string resourceFilename)
        {
            var fullResourcePath = assembly.GetManifestResourceNames()
                .FirstOrDefault(resourceName => resourceName.EndsWith(resourceFilename));

            using var streamReader = new StreamReader(assembly.GetManifestResourceStream(fullResourcePath));
            return streamReader.ReadToEnd();
        }

        public static string ToString(this byte[] bytes, Encoding encoding) => encoding.GetString(bytes);

        public static string ToJson<T>(this T obj, JsonSerializerOutputFormatOptions options = null)
        {
            options = options ?? JsonOptions;
            var jsonStr = Encoding.UTF8.GetString(JsonSerializer.SerializeToUtf8Bytes(obj, options.SerializerOptions));
            if (options.ListOnSingleLine)
            {
                var lines = jsonStr.Split("\n").Select(line => line.Contains("]") ? $"{line}\n" : line);
                jsonStr = string.Join(string.Empty, lines);
            }
            return jsonStr;
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

    }
}
