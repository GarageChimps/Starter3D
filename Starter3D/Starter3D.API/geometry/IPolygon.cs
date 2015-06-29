using System.Collections.Generic;

namespace ThreeAPI.geometry
{
  public interface IPolygon
  {
    IEnumerable<IVertex> Vertices { get; }
  }
}