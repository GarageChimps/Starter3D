using System.Collections.Generic;

namespace ThreeAPI.scene.geometry
{
  public interface IMesh : IShape
  {
    IEnumerable<IVertex> Vertices { get; }
    IEnumerable<IFace> Faces { get; }
    IEnumerable<IPolygon> GetTriangles();
    void AddVertex(IVertex vertex);
    void AddFace(IFace face);
    void GenerateMissingNormals();
  }
}