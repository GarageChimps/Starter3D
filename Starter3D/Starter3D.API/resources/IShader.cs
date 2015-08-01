using System.Drawing;
using OpenTK;
using Starter3D.API.renderer;

namespace Starter3D.API.resources
{
  public interface IShader
  {
    string Name { get; }
    void Configure(IRenderer renderer);
    void Render(IRenderer renderer);
    void Load(IDataNode dataNode);
    void SetVectorParameter(string name, Vector3 vector);
    void SetNumericParameter(string name, float number);
    void SetTextureParameter(string name, Bitmap texture);
  }
}