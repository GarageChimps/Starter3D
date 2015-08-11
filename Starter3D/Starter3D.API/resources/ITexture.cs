using System.Drawing;

namespace Starter3D.API.resources
{
  public interface ITexture : IResource
  {
    string Name { get; }
    Bitmap Image { get; }
  }
}