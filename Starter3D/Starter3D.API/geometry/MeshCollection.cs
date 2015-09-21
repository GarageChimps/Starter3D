using System.Collections.Generic;
using OpenTK;
using Starter3D.API.math;
using Starter3D.API.renderer;
using Starter3D.API.resources;

namespace Starter3D.API.geometry
{
  public class MeshCollection : IMeshCollection
  {
    private IMaterial _material;
    private readonly string _name;
    private IMesh _mesh;

    private List<Matrix4> _instanceMatrices = new List<Matrix4>(); 
    
    public string Name
    {
      get { return _name; }
    }

    public IMaterial Material
    {
      get { return _material; }
      set { _material = value; }
    }

    public MeshCollection(string name, IMesh mesh)
    {
      _name = name;
      _mesh = mesh;
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
      _mesh.Configure(renderer);
      renderer.SetInstanceData(_name, _instanceMatrices);
      renderer.SetInstanceAttribute(_name, _material.Name, 0, "instanceMatrix", Vector4.SizeInBytes, Vector4.SizeInBytes);
    }

    public void Render(IRenderer renderer, Matrix4 transform)
    {
      _material.Render(renderer);
      renderer.SetMatrixParameter("modelMatrix", transform, _material.Shader.Name);
      renderer.DrawMeshCollection(_name, _mesh.GetTriangleCount(), _instanceMatrices.Count);
    }

    public bool Intersects(Ray ray)
    {
      throw new System.NotImplementedException();
    }

    public IShape Clone()
    {
      throw new System.NotImplementedException();
    }

    public void AddInstance(Matrix4 instanceMatrix)
    {
      _instanceMatrices.Add(instanceMatrix);
    }
  }
}