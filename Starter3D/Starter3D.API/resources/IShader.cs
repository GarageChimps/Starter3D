using OpenTK;

namespace Starter3D.API.resources
{
  public interface IShader : IResource
  {
    string Name { get; }
    void SetVectorParameter(string name, Vector3 vector);
    void SetNumericParameter(string name, float number);
    void SetTextureParameter(string name, ITexture texture);
  }
}