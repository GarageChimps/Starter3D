using System.Collections.Generic;

namespace ThreeAPI.geometry
{
  public interface IFace
  {
    bool IsQuad { get; }
    IEnumerable<int> VertexIndices { get; }
    bool HasVertex(int vertexIndex);
    void AppendData(List<int> verticesIndices);
  }
}