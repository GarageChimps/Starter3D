using OpenTK;

namespace Starter3D.API.geometry.factories
{
  public class VertexFactory : IVertexFactory
  {
    public IVertex CreateVertex(Vector3 position, Vector3 normal, Vector3 textureCoordinates)
    {
      return new Vertex(position, normal, textureCoordinates);
    }
  }
}