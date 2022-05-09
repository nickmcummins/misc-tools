using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Text.Json;
using System.Xml;

namespace DecompiledAssemblySourcesTool
{
    public static class Extensions
    {
        public  static readonly string Comma = ", ";
        public static readonly string Space = " ";

        public static IEnumerable<XmlNode> Select(this XmlNodeList nodeList)
        {
            var list = new List<XmlNode>();
            for (var i = 0; i < nodeList.Count; i++)
            {
                list.Add(nodeList.Item(i));
            }
            return list;
        }

        public static IEnumerable<T> Select<T>(this XmlNodeList nodeList, Func<XmlNode, T> func)
        {
            var list = new List<XmlNode>();
            for (var i = 0; i < nodeList.Count; i++)
            {
                list.Add(nodeList.Item(i));
            }
            return list.Select(func);
        }

        public static IEnumerable<T> Flatten<T>(this T node, Func<T, IEnumerable<T>> toChild)
        {
            var nodes = new List<T>();
            var children = toChild.Invoke(node);
            if (children != null)
            {
                foreach (var child in children)
                {
                    nodes.AddRange(child.Flatten(toChild));
                }
            }
            nodes.Add(node);
            return nodes;
        }

        public static string ToString<T>(this IEnumerable<T> ts, Func<T, string>? toStr = null)
        { 
            if (ts == null)
            {
                return $"[]";
            }
            if (toStr != null)
            {
                return $"[{string.Join(Comma, ts.Select(t => toStr(t)))}]";
            }
            else if (typeof(T).GetMethod("ToString").DeclaringType != typeof(object))
            {
                return $"[{string.Join(Comma, ts.Select(t => t.ToString()))}]";
            }
            return JsonSerializer.Serialize(ts);
        }

        public static IDictionary<TKey, IList<T>> ToGroupedDictionary<TKey, T>(this IEnumerable<T> ts, Func<T, TKey> toKey)
        {
            var dict = new Dictionary<TKey, IList<T>>();
            foreach (var t in ts)
            {
                IList<T> groupList;
                if (dict.TryGetValue(toKey.Invoke(t), out var existingList))
                {
                    groupList = existingList;
                }
                else
                {
                    groupList = new List<T>();
                    dict[toKey.Invoke(t)] = groupList;
                }
                groupList.Add(t);
            }
            return dict;
        }

        public static IList<T> Add<T>(this IList<T> ilist, IEnumerable<T> otherList)
        {
            var list = ilist as List<T>;
            list.AddRange(otherList);
            return list;
        }
    }
}
