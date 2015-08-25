using System.Linq;
using OpenTK;

namespace Starter3D.API.geometry.primitives
{
  /// <summary>
  /// A 1x1x1 cube centered at 0,0,0
  /// </summary>
  public class Cube : Mesh
  {
    public Cube(string name="cube", float x = 0f, float y = 0f, float z = 0f, float sizeX = 1f, float sizeY = 1f, float sizeZ = 1f)
      : base(name)
    {
      CreateCube(x, y, z, sizeX, sizeY, sizeZ);
    }

    private void CreateCube(float x, float y, float z, float sizeX, float sizeY, float sizeZ)
    {
      //Top vertexes
      var point1Bottom = new Vector3(x, z, y);
      var point2Bottom = new Vector3(x + sizeX, z, y);
      var point3Bottom = new Vector3(x + sizeX, z, y + sizeY);
      var point4Bottom = new Vector3(x, z, y + sizeY);

      //Bottom vertexes
      var point1Top = new Vector3(x, z + sizeZ, y);
      var point2Top = new Vector3(x + sizeX, z + sizeZ, y);
      var point3Top = new Vector3(x + sizeX, z + sizeZ, y + sizeY);
      var point4Top = new Vector3(x, z + sizeZ, y + sizeY);

      //Normals
      var topNormal = new Vector3(0, 1, 0);
      var bottomNormal = new Vector3(0, -1, 0);
      var leftNormal = new Vector3(-1, 0, 0);
      var rightNormal = new Vector3(1, 0, 0);
      var farNormal = new Vector3(0, 0, -1);
      var nearNormal = new Vector3(0, 0, 1);

      CreateCubeFace(point4Top, point1Top, point2Top, point3Top, topNormal);
      CreateCubeFace(point1Bottom, point4Bottom, point3Bottom, point2Bottom, bottomNormal);
      CreateCubeFace(point1Top, point1Bottom, point2Bottom, point2Top, farNormal);
      CreateCubeFace(point3Top, point3Bottom, point4Bottom, point4Top, nearNormal);
      CreateCubeFace(point4Top, point4Bottom, point1Bottom, point1Top, leftNormal);
      CreateCubeFace(point2Top, point2Bottom, point3Bottom, point3Top, rightNormal);
    }

    private void CreateCubeFace(Vector3 point1, Vector3 point2, Vector3 point3, Vector3 point4, Vector3 normal)
    {
      var index3 = Vertices.Count();
      AddVertex(new Vertex(point3, normal, new Vector2()));
      var index1 = Vertices.Count();
      AddVertex(new Vertex(point1, normal, new Vector2()));
      var index2 = Vertices.Count();
      AddVertex(new Vertex(point2, normal, new Vector2()));
      var index4 = Vertices.Count();
      AddVertex(new Vertex(point4, normal, new Vector2()));

      AddFace(new Face(index3, index1, index2));
      AddFace(new Face(index1, index3, index4));
      
    }
  }
}
