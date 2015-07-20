using System.Collections.Generic;

namespace Starter3D
{
  public interface IGeometry : IShape
  {
    IEnumerable<IVertex> Vertices { get; }
    IEnumerable<IFace> Faces { get; }
    IEnumerable<IPolygon> GetTriangles();
    void AddVertex(IVertex vertex);
    void AddFace(IFace face);
    void GenerateMissingNormals();
    int VerticesCount{ get; }
    int FacesCount{ get; }
  }
}