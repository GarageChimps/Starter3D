using OpenTK;

namespace ThreeAPI.scene.geometry.factories
{
  public class VertexFactory : IVertexFactory
  {
    public IVertex CreateVertex(Vector3 position, Vector3 normal, Vector2 textureCoordinates)
    {
      return new Vertex(position, normal, textureCoordinates);
    }
  }
}