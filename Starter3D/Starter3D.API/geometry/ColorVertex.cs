using System.Collections.Generic;
using OpenTK;
using Starter3D.API.renderer;

namespace Starter3D.API.geometry
{
  public class ColorVertex : Vertex
  {
    private Vector3 _color;

    public Vector3 Color
    {
      get { return _color; }
      set { _color = value; }
    }

    public ColorVertex(Vector3 position, Vector3 color)
      : base(position, new Vector3(), new Vector3())
    {
      _color = color;
    }

    public override void AppendData(List<Vector3> vertexData)
    {
      vertexData.Add(_position);
      vertexData.Add(_color);
    }

    public override void Configure(string objectName, string shaderName, IRenderer renderer)
    {
      renderer.SetVertexAttribute(objectName, shaderName, 0, "inPosition", 3 * Vector3.SizeInBytes, 0);
      renderer.SetVertexAttribute(objectName, shaderName, 1, "inColor", 3 * Vector3.SizeInBytes, Vector3.SizeInBytes);
    }
    
  }
}