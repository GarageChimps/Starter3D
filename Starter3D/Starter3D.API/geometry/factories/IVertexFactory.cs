using OpenTK;

namespace Starter3D
{
  public interface IVertexFactory
  {
    IVertex CreateVertex(Vector3 position, Vector3 normal, Vector2 textureCoordinates);
  }
}