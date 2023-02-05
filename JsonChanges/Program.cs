//
// This code is based on Nick Chapsas Video JSON support gets a major missing feature in .NET 7
// https://www.youtube.com/watch?v=2zUQwVD7T_E&ab_channel=NickChapsas
// This example shows how to work with polymorphism when serializing and deserializing in JSON
//

using JsonChanges;
using System.Text.Json;

Point2D point2d = new Point2D { X = 4, Y = 20 };
string point2dAsText = JsonSerializer.Serialize(point2d);
Point2D point2dAdaing = JsonSerializer.Deserialize<Point2D>(point2dAsText)!;

Point2D point3d = new Point3D { X = 4, Y = 20, Z = 30 };
string point3dAsText = JsonSerializer.Serialize(point3d);
Point3D point3dAdaing = JsonSerializer.Deserialize<Point3D>(point3dAsText)!;
