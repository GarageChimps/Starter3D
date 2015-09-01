using System;
using System.Collections.Generic;
using System.Linq;
using OpenTK;
using Starter3D.API.math;

namespace Starter3D.API.geometry
{
  public class Face : IFace
  {
    private readonly List<int> _vertexIndices = new List<int>();

    public bool IsQuad { get { return _vertexIndices.Count == 4; } }

    public IEnumerable<int> VertexIndices { get { return _vertexIndices;} }
    
    public Face(int index0, int index1, int index2)
    {
      _vertexIndices.Add(index0);
      _vertexIndices.Add(index1);
      _vertexIndices.Add(index2);
    }

    public Face(int index0, int index1, int index2, int index3)
      : this(index0, index1, index2)
    {
      _vertexIndices.Add(index3);
    }

    public bool HasVertex(int vertexIndex)
    {
      foreach (var index in _vertexIndices)
      {
        if (vertexIndex == index)
          return true;
      }
      return false;
    }

    public void AppendData(List<int> verticesIndices)
    {
      verticesIndices.AddRange(_vertexIndices);
    }

    public bool Intersects(Ray ray, List<IVertex> vertices)
    {
      if(IsQuad)
        throw new NotImplementedException();

      float xa = vertices[_vertexIndices[0]].Position.X;
      float xb = vertices[_vertexIndices[1]].Position.X;
      float xc = vertices[_vertexIndices[2]].Position.X;
      float ya = vertices[_vertexIndices[0]].Position.Y;
      float yb = vertices[_vertexIndices[1]].Position.Y;
      float yc = vertices[_vertexIndices[2]].Position.Y;
      float za = vertices[_vertexIndices[0]].Position.Z;
      float zb = vertices[_vertexIndices[1]].Position.Z;
      float zc = vertices[_vertexIndices[2]].Position.Z;
      float xd = ray.Direction.X;
      float yd = ray.Direction.Y;
      float zd = ray.Direction.Z;
      float xe = ray.Position.X;
      float ye = ray.Position.Y;
      float ze = ray.Position.Z;

      float detA = Determinant(xa - xb, xa - xc, xd, ya - yb, ya - yc, yd, za - zb, za - zc, zd);
      float t = Determinant(xa - xb, xa - xc, xa - xe, ya - yb, ya - yc, ya - ye, za - zb, za - zc, za - ze) / detA;
      if (t < 0)
      {
        return false;
      }
      float gamma = Determinant(xa - xb, xa - xe, xd, ya - yb, ya - ye, yd, za - zb, za - ze, zd) / detA;
      if (gamma < 0 || gamma > 1)
        return false;
      float beta = Determinant(xa - xe, xa - xc, xd, ya - ye, ya - yc, yd, za - ze, za - zc, zd) / detA;
      if ((beta < 0) || (beta > (1 - gamma)))
        return false;

      if (t < ray.IntersectionDistance)
      {
        ray.IntersectionDistance = t;
      }
      return true;
    }

    private static float Determinant(float a, float b, float c, float d, float e, float f, float g, float h, float i)
    {
      return a * e * i - a * f * h - b * d * i + c * d * h + b * f * g - c * e * g;
    }

    public override bool Equals(object obj)
    {
      // If parameter cannot be cast to Point return false.
      var other = obj as Face;
      if (other == null)
      {
        return false;
      }

      if (this.VertexIndices.Count() != other.VertexIndices.Count())
        return false;

      // Return true if the fields match:
      bool areEqual = true;
      for (int i = 0; i < this.VertexIndices.Count(); i++)
      {
        areEqual &= VertexIndices.ElementAt(i) == other.VertexIndices.ElementAt(i);
      }
      return areEqual;
    }

    public bool Equals(Face other)
    {
      // If parameter is null return false:
      if (other == null)
      {
        return false;
      }

      if (this.VertexIndices.Count() != other.VertexIndices.Count())
        return false;

      // Return true if the fields match:
      bool areEqual = true;
      for (int i = 0; i < this.VertexIndices.Count(); i++)
      {
        areEqual &= VertexIndices.ElementAt(i) == other.VertexIndices.ElementAt(i);
      }
      return areEqual;
    }

    public override int GetHashCode()
    {
      int code = VertexIndices.ElementAt(0);
      for (int i = 1; i < this.VertexIndices.Count(); i++)
      {
        code ^= VertexIndices.ElementAt(i);
      }
      return code;
    }
  }
}
