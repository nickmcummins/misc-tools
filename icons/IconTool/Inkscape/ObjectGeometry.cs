using System.Reflection;

namespace IconTool.Inkscape
{
    public class ObjectGeometry
    {
        public static readonly IDictionary<QueryObjectGeometryProperty, PropertyInfo> Properties = typeof(ObjectGeometry).GetProperties().ToDictionary(p => Enum.Parse<QueryObjectGeometryProperty>(p.Name));

        public int? Width { get; set; }
        public int? Height { get; set; }
    }
}
