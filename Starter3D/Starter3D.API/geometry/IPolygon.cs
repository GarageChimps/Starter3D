using System.Collections.Generic;

namespace Starter3D
{
  public interface IPolygon
  {
    IEnumerable<IVertex> Vertices { get; }
  }
}