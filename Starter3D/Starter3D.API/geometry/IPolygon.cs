using System.Collections.Generic;

namespace Starter3D.API.geometry
{
  public interface IPolygon
  {
    IEnumerable<IVertex> Vertices { get; }
  }
}