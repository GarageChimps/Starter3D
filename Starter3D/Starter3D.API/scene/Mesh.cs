using System;
using System.Collections.Generic;
using System.Linq;
using OpenTK;

namespace ThreeAPI.scene
{
  public class Mesh : IMesh
  {
    private readonly IMeshLoader _meshLoader;
    private readonly List<IVertex> _vertices = new List<IVertex>();
    private readonly List<IFace> _faces = new List<IFace>();

    public IEnumerable<IVertex> Vertices
    {
      get { return _vertices; }
    }

    public IEnumerable<IFace> Faces
    {
      get { return _faces; }
    }
    
    public Mesh(IMeshLoader meshLoader)
    {
      _meshLoader = meshLoader;
    }

    public IEnumerable<IPolygon> GetTriangles()
    {
      var triangles = new List<IPolygon>();
      foreach (var face in _faces)
      {
        var triangle = new Triangle();
        triangle.AddVertex(_vertices[face.VertexIndices.ElementAt(0)]);
        triangle.AddVertex(_vertices[face.VertexIndices.ElementAt(1)]);
        triangle.AddVertex(_vertices[face.VertexIndices.ElementAt(2)]);
        triangles.Add(triangle);
        if (face.IsQuad)
        {
          var triangle2 = new Triangle();
          triangle2.AddVertex(_vertices[face.VertexIndices.ElementAt(2)]);
          triangle2.AddVertex(_vertices[face.VertexIndices.ElementAt(3)]);
          triangle2.AddVertex(_vertices[face.VertexIndices.ElementAt(0)]);
          triangles.Add(triangle2);
        }
      }
      return triangles;
    }

    public void AddVertex(IVertex vertex)
    {
      _vertices.Add(vertex);
    }

    public void AddFace(IFace face)
    {
      _faces.Add(face);
    }

    public void GenerateMissingNormals()
    {
      var vertexToFacesMap = new Dictionary<IVertex, List<IFace>>();
      int index = 0;
      foreach (var vertex in _vertices)
      {
        if (!vertex.HasValidNormal())
        {
          foreach (var face in _faces)
          {
            if (face.HasVertex(index))
            {
              if(!vertexToFacesMap.ContainsKey(vertex))
                vertexToFacesMap.Add(vertex, new List<IFace>());
              vertexToFacesMap[vertex].Add(face);
            }
          }
        }
        index++;
      }

      foreach (var keyPair in vertexToFacesMap)
      {
        GenerateNormal(keyPair.Key, keyPair.Value);
      }
    }

    public void Load(string filePath)
    {
      _meshLoader.Load(this, filePath);
    }

    public void Save(string filePath)
    {
      throw new NotImplementedException();
    }

    private void GenerateNormal(IVertex vertex, List<IFace> faces)
    {
      var accumulatedNormal = new Vector3();
      foreach (var face in faces)
      {
        accumulatedNormal += GetFaceNormal(face);
      }
      vertex.Normal = accumulatedNormal.Normalized();
    }


    private Vector3 GetFaceNormal(IFace face)
    {
      var v10 = _vertices[face.VertexIndices.ElementAt(1)].Position - _vertices[face.VertexIndices.ElementAt(0)].Position;
      var v20 = _vertices[face.VertexIndices.ElementAt(2)].Position - _vertices[face.VertexIndices.ElementAt(0)].Position;
      var cross = Vector3.Cross(v10, v20);
      cross.Normalize();
      return cross;
    }

   
  }
}