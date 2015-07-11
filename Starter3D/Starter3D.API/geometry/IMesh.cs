using System.Collections.Generic;
using ThreeAPI.materials;

namespace ThreeAPI.geometry
{
  public interface IMesh : IShape
  {
    IEnumerable<IVertex> Vertices { get; }
    IEnumerable<IFace> Faces { get; }
    IEnumerable<IPolygon> GetTriangles();
    void AddVertex(IVertex vertex);
    void AddFace(IFace face);
    void GenerateMissingNormals();
    int VerticesCount{ get; }
    int FacesCount{ get; }
    IMaterial Material { get; }
  }
}