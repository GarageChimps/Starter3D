using System.Collections.Generic;

namespace ThreeAPI.scene
{
  public interface IFace
  {
    bool IsQuad { get; }
    IEnumerable<int> VertexIndices { get; }
    bool HasVertex(int vertexIndex);
  }
}