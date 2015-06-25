using OpenTK;

namespace ThreeAPI.scene
{
  public class Vertex : IVertex
  {
    private Vector3 _position;
    private Vector3 _normal;
    private Vector2 _textureCoords;

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

    public Vector2 TextureCoords
    {
      get { return _textureCoords; }
      set { _textureCoords = value; }
    }

    public bool HasValidNormal()
    {
      return !(_normal.X == 0 && _normal.Y == 0 && _normal.Z == 0);
    }

    public Vertex(Vector3 position, Vector3 normal, Vector2 textureCoords)
    {
      _position = position;
      _normal = normal;
      _textureCoords = textureCoords;
    }

    public Vertex()
    {
      
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