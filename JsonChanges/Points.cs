using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace JsonChanges
{
    [JsonPolymorphic(UnknownDerivedTypeHandling = JsonUnknownDerivedTypeHandling.FailSerialization, TypeDiscriminatorPropertyName = "Type")]
    [JsonDerivedType(typeof(Point3D), "point3d")]
    [JsonDerivedType(typeof(Point2D), "point2d")]
    public class Point2D
    {
        public double X { get; set; }

        public double Y { get; set; }
    }

    public class Point3D : Point2D
    {
        public double Z { get; set; }
    }
}
