using System;
using Starter3D.API.renderer;

namespace Starter3D.API.resources
{
  public class Shader : IShader
  {
    private string _shaderName;
    private string _vertexShader;
    private string _fragmentShader;

    public string Name
    {
      get { return _shaderName; }
    }

    public void Configure(IRenderer renderer)
    {
      renderer.LoadShaders(_shaderName, _vertexShader, _fragmentShader);
    }

    public void Render(IRenderer renderer)
    {
      renderer.UseShader(_shaderName);
    }

    public void Load(IDataNode dataNode)
    {
      _shaderName = dataNode.ReadParameter("key");
      _vertexShader = dataNode.ReadParameter("vertex");
      _fragmentShader = dataNode.ReadParameter("fragment");
    }
  }
}