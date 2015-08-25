using System;
using System.Collections.Generic;
using System.Linq;
using OpenTK;
using Starter3D.API.geometry.loaders;
using Starter3D.API.renderer;
using Starter3D.API.resources;

namespace Starter3D.API.geometry
{
  public class Mesh : IMesh
  {
    private readonly IMeshLoader _meshLoader;
    private readonly List<IVertex> _vertices = new List<IVertex>();
    private readonly List<IFace> _faces = new List<IFace>();
    private IMaterial _material;
    private readonly string _name;

    public Mesh(IMeshLoader meshLoader, string name="default")
    {
      _meshLoader = meshLoader;
      _name = name;
    }

    public Mesh(string name = "default")
    {
      _name = name;
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

    public string Name
    {
      get { return _name; }
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

    public bool HasNoValidNormal()
    {
      foreach (var vertex in _vertices)
      {
        if (vertex.HasValidNormal())
          return false;
      }
      return true;
    }

    public void GenerateMissingNormals()
    {
      var vertexToFacesMap = new Dictionary<IVertex, List<IFace>>();
      foreach (var face in _faces)
      {
        foreach (var vertexIndex in face.VertexIndices)
        {
          var vertex = _vertices[vertexIndex];
          if (!vertexToFacesMap.ContainsKey(vertex))
            vertexToFacesMap.Add(vertex, new List<IFace>());
          vertexToFacesMap[vertex].Add(face);
        }
      }

      foreach (var keyPair in vertexToFacesMap)
      {
        if(!keyPair.Key.HasValidNormal())
          GenerateNormal(keyPair.Key, keyPair.Value);
      }
    }

    public void Load(string filePath)
    {
      _meshLoader.Load(this, filePath);
      if (HasNoValidNormal())
        GenerateMissingNormals();
    }

    public void Save(string filePath)
    {
      throw new NotImplementedException();
    }

    public void Configure(IRenderer renderer)
    {
      _material.Configure(renderer);
      renderer.LoadObject(_name);
      renderer.SetVerticesData(_name, GetVerticesData());
      renderer.SetFacesData(_name, GetFaceData());
      _vertices.First().Configure(_name, _material.Shader.Name, renderer); //We use the first vertex as representatve to configure the vertex info of the renderer
    }

    public void Render(IRenderer renderer, Matrix4 transform)
    {
      _material.Render(renderer);
      renderer.SetMatrixParameter("modelMatrix", transform, _material.Shader.Name);
      renderer.DrawTriangles(_name, GetTriangleCount());
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

    private int GetTriangleCount()
    {
      return FacesCount * 3;
    }

   
  }
}