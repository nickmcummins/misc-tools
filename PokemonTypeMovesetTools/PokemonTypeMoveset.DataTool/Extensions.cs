using AngleSharp.Dom;
using System.Reflection;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace PokemonTypeMoveset.DataTool
{
    public static class Extensions
    {
        private static JsonSerializerOptions _jsonOptions;
        public static JsonSerializerOptions JsonOptions
        {
            get
            {
                if (_jsonOptions == null)
                {
                    _jsonOptions = new JsonSerializerOptions();
                    _jsonOptions.Converters.Add(new JsonStringEnumConverter());
                }

                return _jsonOptions;
            }
        }

        public static IElement GetNextSiblingElementByTagname(this IElement element, string tagName)
        {
            IElement? currentSibling = null; 
            do
            {
                currentSibling = element.NextElementSibling;
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


        public static T ReadResource<T>(this Assembly assembly, string resourceFilename) => JsonSerializer.Deserialize<T>(assembly.ReadResourceAsString(resourceFilename), JsonOptions);

        public static string ReadResourceAsString(this Assembly assembly, string resourceFilename)
        {
            var fullResourcePath = assembly.GetManifestResourceNames()
                .FirstOrDefault(resourceName => resourceName.EndsWith(resourceFilename));

            using var streamReader = new StreamReader(assembly.GetManifestResourceStream(fullResourcePath));
            return streamReader.ReadToEnd();
        }
    }
}
