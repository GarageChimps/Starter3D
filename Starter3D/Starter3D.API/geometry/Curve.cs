using System.Collections.Generic;
using System.Linq;
using OpenTK;
using Starter3D.API.math;
using Starter3D.API.renderer;
using Starter3D.API.resources;

namespace Starter3D.API.geometry
{
  public class Curve : ICurve
  {
    private string _name;
    private IMaterial _material;

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

    public List<IVertex> Vertices
    {
      get { return _vertices; }
    }

    public Curve(string name)
    {
      _name = name;
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
      if(_vertices.Count > 0)
        _vertices.First().Configure(_name, _material.Shader.Name, renderer);
    }

    public void Render(IRenderer renderer, Matrix4 modelTransform)
    {
      _material.Render(renderer);
      renderer.DrawLines(_name, _vertices.Count);
    }

    public bool Intersects(Ray ray)
    {
      throw new System.NotImplementedException();
    }

    public void AddPoint(Vector3 position)
    {
      _vertices.Add(new Vertex(position, new Vector3(), new Vector3() ));
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
      var data = Enumerable.Range(0, _vertices.Count).ToList();
      return data;
    }
  }
}