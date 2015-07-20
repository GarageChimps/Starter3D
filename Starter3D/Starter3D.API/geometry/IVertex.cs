using OpenTK;

namespace Starter3D
{
  public interface IVertex
  {
    Vector3 Position { get; }
    Vector3 Normal { get; set; }
    Vector2 TextureCoords { get; }
    bool HasValidNormal();
  }
}