using System.Collections.Generic;

namespace Starter3D.API.geometry
{
  /// <summary>
  /// Interface for polygons
  /// @author Alejandro Echeverria
  /// @data July-2015   
  /// </summary>
  public interface IPolygon
  {
    IEnumerable<IVertex> Vertices { get; }    //The vertices of this polygon
  }
}