using System.Collections.Generic;
using OpenTK;

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
    List<Vector3> GetVerticesData();
    List<int> GetFaceData();
  }
}