using System.Collections.Generic;

namespace Starter3D.API.geometry
{
  /// <summary>
  /// Represent a face of a mesh, referencing to the indices of vertex of the mesh that are part of the face
  /// @author Alejandro Echeverria
  /// @data July-2015   
  /// </summary>
  public interface IFace
  {
    bool IsQuad { get; }                        //True if the face has 4 vertices
    IEnumerable<int> VertexIndices { get; }     //Indices of vertex that are part of the face
    
    /// <summary>
    /// Returns true if the vertex queried is in the face
    /// </summary>
    /// <param name="vertexIndex">Index of vertex</param>
    /// <returns>True or false</returns>
    bool HasVertex(int vertexIndex);
    
    /// <summary>
    /// Appends this face indices to the list provided as parameters
    /// </summary>
    /// <param name="verticesIndices">List where the vertex will be appended (at the end of it)</param>
    void AppendData(List<int> verticesIndices);
  }
}