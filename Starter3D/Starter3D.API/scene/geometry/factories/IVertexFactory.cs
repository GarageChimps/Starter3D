using OpenTK;

namespace ThreeAPI.scene.geometry.factories
{
  public interface IVertexFactory
  {
    IVertex CreateVertex(Vector3 position, Vector3 normal, Vector2 textureCoordinates);
  }
}