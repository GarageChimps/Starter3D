namespace Starter3D.API.resources
{
  public interface IMaterial : IResource
  {
    IShader Shader { get; set;  }
    string Name { get; }
  }
}
