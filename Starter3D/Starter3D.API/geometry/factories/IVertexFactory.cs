using OpenTK;

namespace Starter3D.API.geometry.factories
{
  public interface IVertexFactory
  {
    IVertex CreateVertex(Vector3 position, Vector3 normal, Vector3 textureCoordinates);
  }
}