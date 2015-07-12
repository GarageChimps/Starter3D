using System.Collections.Generic;

namespace Starter3D.API.geometry
{
  public class Triangle : IPolygon
  {
    private readonly List<IVertex> _vertices = new List<IVertex>();
 
    public IEnumerable<IVertex> Vertices
    {
      get { return _vertices; }
    }

    public void AddVertex(IVertex vertex)
    {
      _vertices.Add(vertex);
    }
    
  }
}