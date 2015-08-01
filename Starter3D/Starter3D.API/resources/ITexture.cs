using System.Drawing;
using Starter3D.API.renderer;

namespace Starter3D.API.resources
{
  public interface ITexture : IResource
  {
    string Name { get; }
    Bitmap Image { get; }
    void Configure(IRenderer renderer, string shaderName, string uniformName, int index);
  }
}