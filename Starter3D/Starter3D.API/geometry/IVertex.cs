using OpenTK;

namespace ThreeAPI.geometry
{
  public interface IVertex
  {
    Vector3 Position { get; }
    Vector3 Normal { get; set; }
    Vector2 TextureCoords { get; }
    bool HasValidNormal();
  }
}