using System.Collections.Generic;

namespace ThreeAPI.scene.geometry
{
  public interface IPolygon
  {
    IEnumerable<IVertex> Vertices { get; }
  }
}