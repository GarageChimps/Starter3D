using System.Collections.Generic;
using OpenTK;
using Starter3D.API.renderer;

namespace Starter3D.API.geometry
{
  /// <summary>
  /// Interface for a generic vertex, that contains position, normal and texcoords data
  /// @author Alejandro Echeverria
  /// @data July-2015   
  /// </summary>
  public interface IVertex
  {
    Vector3 Position { get; }     
    Vector3 Normal { get; set; }
    Vector3 TextureCoords { get; }

    /// <summary>
    /// An invalid normal is one that has all components in 0
    /// </summary>
    /// <returns>True if the normal is valid, false otherwise</returns>
    bool HasValidNormal();

    /// <summary>
    /// Appends this vertex data to the provdided list
    /// </summary>
    /// <param name="vertexData">List of vectors where this vertex data will be appended</param>
    void AppendData(List<Vector3> vertexData);

    /// <summary>
    /// Configures this vertex for rendering
    /// </summary>
    /// <param name="objectName">Name of shape that contains this vertex</param>
    /// <param name="shaderName">Name of shader that will be used for rendering this vertex</param>
    /// <param name="renderer">Renderer</param>
    void Configure(string objectName, string shaderName, IRenderer renderer);
  }
}