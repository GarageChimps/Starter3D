using System.Collections.Generic;
using OpenTK;

namespace Starter3D.API.geometry
{
  /// <summary>
  /// Interface for a mesh, i.e. a collection of vertex and faces
  /// @author Alejandro Echeverria
  /// @data July-2015   
  /// </summary>
  public interface IMesh : IShape
  {
    IEnumerable<IVertex> Vertices { get; } //Vertices in this mesh
    IEnumerable<IFace> Faces { get; }      //Faces in this mesh
    int VerticesCount { get; }             //Number of vertices
    int FacesCount { get; }                //Number if faces
    
    /// <summary>
    /// Provides a list of all the triangles in this mesh. If a given face is a quad, two triangles will be emitted for that face
    /// </summary>
    /// <returns>List of triangles</returns>
    IEnumerable<IPolygon> GetTriangles();

    /// <summary>
    /// Adds a new vertex
    /// </summary>
    /// <param name="vertex">Vertex</param>
    void AddVertex(IVertex vertex);

    /// <summary>
    /// Adds a new face
    /// </summary>
    /// <param name="face">Face</param>
    void AddFace(IFace face);

    /// <summary>
    /// An invalid normal is one that has all components in 0
    /// </summary>
    /// <returns>True if the normal is valid, false otherwise</returns>
    bool HasNoValidNormal();
    
    /// <summary>
    /// Generates normals for all vertex that have invalid normals, by averaging the normals of the faces where the vertex belongs
    /// </summary>
    void GenerateMissingNormals();

    /// <summary>
    /// Returns a list of Vector3 with the information contained on all vertices of the mesh, vertex by vertex
    /// </summary>
    /// <returns>List with vector information for all vertices</returns>
    List<Vector3> GetVerticesData();

    /// <summary>
    /// Returs a list of all the vertex indices of all the faces in the mesh
    /// </summary>
    /// <returns></returns>
    List<int> GetFaceData();
  }
}