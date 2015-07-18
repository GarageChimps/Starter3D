using System.Collections.Generic;
using OpenTK;
using Starter3D.API.renderer;

namespace Starter3D.API.geometry
{
  public class Vertex : IVertex
  {
    private Vector3 _position;
    private Vector3 _normal;
    private Vector3 _textureCoords;

    public Vector3 Position
    {
      get { return _position; }
      set { _position = value; }
    }

    public Vector3 Normal
    {
      get { return _normal; }
      set { _normal = value; }
    }

    public Vector3 TextureCoords
    {
      get { return _textureCoords; }
      set { _textureCoords = value; }
    }

    public Vertex(Vector3 position, Vector3 normal, Vector2 textureCoords)
      : this(position, normal, new Vector3(textureCoords))
    {
    }

    public Vertex(Vector3 position, Vector3 normal, Vector3 textureCoords)
    {
      _position = position;
      _normal = normal;
      _textureCoords = textureCoords;
    }

    public Vertex()
    {

    }

    public bool HasValidNormal()
    {
      return !(_normal.X == 0 && _normal.Y == 0 && _normal.Z == 0);
    }

    public void AppendData(List<Vector3> vertexData)
    {
      vertexData.Add(_position);
      vertexData.Add(_normal);
      vertexData.Add(_textureCoords);
    }

    public void Configure(string objectName, string shaderName, IRenderer renderer)
    {
      renderer.SetVertexAttribute(objectName, shaderName, 0, "inPosition", 3 * Vector3.SizeInBytes, 0);
      renderer.SetVertexAttribute(objectName, shaderName, 1, "inNormal", 3 * Vector3.SizeInBytes, Vector3.SizeInBytes);
      renderer.SetVertexAttribute(objectName, shaderName, 2, "inTextureCoords", 3 * Vector3.SizeInBytes, 2 * Vector3.SizeInBytes);
    }

    public override bool Equals(object obj)
    {
      // If parameter cannot be cast to Point return false.
      var other = obj as Vertex;
      if (other == null)
      {
        return false;
      }

      // Return true if the fields match:
      var areEqual = (_position == other.Position) && (_normal == other.Normal) && (_textureCoords == other.TextureCoords);
      return areEqual;
    }

    public bool Equals(Vertex other)
    {
      // If parameter is null return false:
      if (other == null)
      {
        return false;
      }

      // Return true if the fields match:
      return (_position == other.Position) && (_normal == other.Normal) && (_textureCoords == other.TextureCoords);
    }

    public override int GetHashCode()
    {
      return _position.GetHashCode() ^ _normal.GetHashCode() ^ _textureCoords.GetHashCode();
    }
  }
}