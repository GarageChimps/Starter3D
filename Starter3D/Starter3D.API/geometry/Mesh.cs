using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using OpenTK;
using ThreeAPI.geometry.loaders;
using ThreeAPI.renderer;
using ThreeAPI.resources;

namespace ThreeAPI.geometry
{
  public class Mesh : IMesh
  {
    private readonly IMeshLoader _meshLoader;
    private readonly List<IVertex> _vertices = new List<IVertex>();
    private readonly List<IFace> _faces = new List<IFace>();
    private IMaterial _material;

    public Mesh()
    {
    }

    public Mesh(IMeshLoader meshLoader)
    {
      _meshLoader = meshLoader;
    }

    public IEnumerable<IVertex> Vertices
    {
      get { return _vertices; }
    }

    public int VerticesCount{
      get { return _vertices.Count; }
    }

    public IEnumerable<IFace> Faces
    {
      get { return _faces; }
    }

    public int FacesCount{
      get{ return _faces.Count; }
    }

    public IMaterial Material
    {
      get { return _material; }
      set { _material = value; }
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

    public void ConfigureRenderer(IRenderer renderer)
    {
      _material.ConfigureRenderer(renderer);
      renderer.AddMesh(this);
      renderer.SetVerticesData(GetVerticesData());
      renderer.SetFacesData(GetFaceData());
      _vertices.First().ConfigureRenderer(renderer); //We use the first vertex as representatve to configure the vertex info of the renderer
      
    }

    public List<Vector3> GetVerticesData()
    {
      var data = new List<Vector3>();
      foreach (var vertex in _vertices)
      {
        vertex.AppendData(data);
      }
      return data;
    }

    public List<int> GetFaceData()
    {
      var data = new List<int>();
      foreach (var face in _faces)
      {
        face.AppendData(data);
      }
      return data;
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