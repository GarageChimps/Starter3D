using System.Collections.Generic;

namespace Starter3D
{
  public interface IFace
  {
    bool IsQuad { get; }
    IEnumerable<int> VertexIndices { get; }
    bool HasVertex(int vertexIndex);
  }
}