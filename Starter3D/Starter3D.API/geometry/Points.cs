using System.Collections.Generic;
using System.Linq;
using OpenTK;
using Starter3D.API.math;
using Starter3D.API.renderer;
using Starter3D.API.resources;

namespace Starter3D.API.geometry
{
  public class Points : IPoints
  {
    private string _name;
    private IMaterial _material;
    private float _size;

    private readonly List<IVertex> _vertices = new List<IVertex>();

    public string Name
    {
      get { return _name; }
    }

    public IMaterial Material
    {
      get { return _material; }
      set { _material = value; }
    }

    public float Size
    {
      get { return _size; }
      set { _size = value; }
    }

    public Points(string name, float size)
    {
      _name = name;
      _size = size;
    }

    public void Load(string filePath)
    {
      throw new System.NotImplementedException();
    }

    public void Save(string filePath)
    {
      throw new System.NotImplementedException();
    }

    public void Configure(IRenderer renderer)
    {
      _material.Configure(renderer);
      renderer.LoadObject(_name);
      renderer.SetVerticesData(_name, GetVerticesData());
      renderer.SetFacesData(_name, GetFaceData());
      if (_vertices.Count > 0)
        _vertices.First().Configure(_name, _material.Shader.Name, renderer);
    }

    public void Render(IRenderer renderer, Matrix4 modelTransform)
    {
      if (_vertices.Count > 0)
      {
        _material.Render(renderer);
        renderer.DrawPoints(_name, _vertices.Count, _size);
      }
    }

    public bool Intersects(Ray ray)
    {
      throw new System.NotImplementedException();
    }

    public void AddPoint(Vector3 position)
    {
      _vertices.Add(new Vertex(position, new Vector3(), new Vector3()));
    }

    public void Clear()
    {
      _vertices.Clear();
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
      var faces = new List<int>();
      for (int i = 0; i < _vertices.Count; i++)
      {
        faces.Add(i);
      }
      return faces;
    }
  }
}