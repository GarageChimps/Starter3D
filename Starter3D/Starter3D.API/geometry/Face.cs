using System.Collections.Generic;
using System.Linq;

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
