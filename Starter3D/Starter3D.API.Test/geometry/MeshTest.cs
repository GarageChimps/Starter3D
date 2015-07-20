using System.Linq;
using Moq;
using NUnit.Framework;
using OpenTK;
using Starter3D;

namespace ThreeAPI.Test.geometry
{
  [TestFixture()]
  public class MeshTest
  {
    [Test()]
    public void GetTriangles_ThreeVerticesOneTriFace_AssertOneTriangle()
    {
      var mesh = CreateMesh();
      mesh.AddVertex(new Vertex(new Vector3(1, 1, 0), new Vector3(1, 1, 1).Normalized(), new Vector2(0, 0)));
      mesh.AddVertex(new Vertex(new Vector3(1, -1, 0), new Vector3(1, 1, 1).Normalized(), new Vector2(0, 1)));
      mesh.AddVertex(new Vertex(new Vector3(-1, -1, 0), new Vector3(1, 1, 1).Normalized(), new Vector2(1, 1)));
      mesh.AddFace(new Face(0, 1, 2));
      var triangles = mesh.GetTriangles();

      Assert.AreEqual(1, triangles.Count());
    }

    [Test()]
    public void GetTriangles_ThreeVerticesOneTriFace_AssertCorrectTriangle()
    {
      var mesh = CreateMesh();
      mesh.AddVertex(new Vertex(new Vector3(1, 1, 0), new Vector3(1, 1, 1).Normalized(), new Vector2(0, 0)));
      mesh.AddVertex(new Vertex(new Vector3(1, -1, 0), new Vector3(1, 1, 1).Normalized(), new Vector2(0, 1)));
      mesh.AddVertex(new Vertex(new Vector3(-1, -1, 0), new Vector3(1, 1, 1).Normalized(), new Vector2(1, 1)));
      mesh.AddFace(new Face(0, 1, 2));
      var triangles = mesh.GetTriangles();

      Assert.AreEqual(new Vertex(new Vector3(1, 1, 0), new Vector3(1, 1, 1).Normalized(), new Vector2(0, 0)), triangles.ElementAt(0).Vertices.ElementAt(0));
      Assert.AreEqual(new Vertex(new Vector3(1, -1, 0), new Vector3(1, 1, 1).Normalized(), new Vector2(0, 1)), triangles.ElementAt(0).Vertices.ElementAt(1));
      Assert.AreEqual(new Vertex(new Vector3(-1, -1, 0), new Vector3(1, 1, 1).Normalized(), new Vector2(1, 1)), triangles.ElementAt(0).Vertices.ElementAt(2));
    }

    [Test()]
    public void GetTriangles_ThreeVerticesOneQuadFace_AssertOneTriangle()
    {
      var mesh = CreateMesh();
      mesh.AddVertex(new Vertex(new Vector3(1, 1, 0), new Vector3(1, 1, 1).Normalized(), new Vector2(0, 0)));
      mesh.AddVertex(new Vertex(new Vector3(1, -1, 0), new Vector3(1, 1, 1).Normalized(), new Vector2(0, 1)));
      mesh.AddVertex(new Vertex(new Vector3(-1, -1, 0), new Vector3(1, 1, 1).Normalized(), new Vector2(1, 1)));
      mesh.AddVertex(new Vertex(new Vector3(-1, 1, 0), new Vector3(1, 1, 1).Normalized(), new Vector2(1, 0)));
      mesh.AddFace(new Face(0, 1, 2, 3));
      var triangles = mesh.GetTriangles();

      Assert.AreEqual(2, triangles.Count());
    }

    [Test()]
    public void GetTriangles_ThreeVerticesOneQuadFace_AssertCorrectTriangle()
    {
      var mesh = CreateMesh();
      mesh.AddVertex(new Vertex(new Vector3(1, 1, 0), new Vector3(1, 1, 1).Normalized(), new Vector2(0, 0)));
      mesh.AddVertex(new Vertex(new Vector3(1, -1, 0), new Vector3(1, 1, 1).Normalized(), new Vector2(0, 1)));
      mesh.AddVertex(new Vertex(new Vector3(-1, -1, 0), new Vector3(1, 1, 1).Normalized(), new Vector2(1, 1)));
      mesh.AddVertex(new Vertex(new Vector3(-1, 1, 0), new Vector3(1, 1, 1).Normalized(), new Vector2(1, 0)));
      mesh.AddFace(new Face(0, 1, 2, 3));
      var triangles = mesh.GetTriangles();

      Assert.AreEqual(new Vertex(new Vector3(1, 1, 0), new Vector3(1, 1, 1).Normalized(), new Vector2(0, 0)), triangles.ElementAt(0).Vertices.ElementAt(0));
      Assert.AreEqual(new Vertex(new Vector3(1, -1, 0), new Vector3(1, 1, 1).Normalized(), new Vector2(0, 1)), triangles.ElementAt(0).Vertices.ElementAt(1));
      Assert.AreEqual(new Vertex(new Vector3(-1, -1, 0), new Vector3(1, 1, 1).Normalized(), new Vector2(1, 1)), triangles.ElementAt(0).Vertices.ElementAt(2));
      Assert.AreEqual(new Vertex(new Vector3(-1, -1, 0), new Vector3(1, 1, 1).Normalized(), new Vector2(1, 1)), triangles.ElementAt(1).Vertices.ElementAt(0));
      Assert.AreEqual(new Vertex(new Vector3(-1, 1, 0), new Vector3(1, 1, 1).Normalized(), new Vector2(1, 0)), triangles.ElementAt(1).Vertices.ElementAt(1));
      Assert.AreEqual(new Vertex(new Vector3(1, 1, 0), new Vector3(1, 1, 1).Normalized(), new Vector2(0, 0)), triangles.ElementAt(1).Vertices.ElementAt(2));
    }

    [Test()]
    public void GenerateMissingNormals_AllMisingFromOneFaces_AllNormalsAreZMinus()
    {
      var mesh = CreateMesh();
      mesh.AddVertex(new Vertex(new Vector3(1, 1, 0), new Vector3(), new Vector2(0, 0)));
      mesh.AddVertex(new Vertex(new Vector3(1, -1, 0), new Vector3(), new Vector2(0, 1)));
      mesh.AddVertex(new Vertex(new Vector3(-1, -1, 0), new Vector3(), new Vector2(1, 1)));
      mesh.AddFace(new Face(0, 1, 2));

      mesh.GenerateMissingNormals();

      Assert.AreEqual(new Vector3(0, 0, -1), mesh.Vertices.ElementAt(0).Normal);
      Assert.AreEqual(new Vector3(0, 0, -1), mesh.Vertices.ElementAt(1).Normal);
      Assert.AreEqual(new Vector3(0, 0, -1), mesh.Vertices.ElementAt(2).Normal);
    }

    [Test()]
    public void GenerateMissingNormals_AllMisingFromOneFaceReverseOrderOfVertices_AllNormalsAreZPlus()
    {
      var mesh = CreateMesh();
      mesh.AddVertex(new Vertex(new Vector3(1, 1, 0), new Vector3(), new Vector2(0, 0)));
      mesh.AddVertex(new Vertex(new Vector3(-1, 1, 0), new Vector3(), new Vector2(0, 1)));
      mesh.AddVertex(new Vertex(new Vector3(1, -1, 0), new Vector3(), new Vector2(1, 1)));
      mesh.AddFace(new Face(0, 1, 2));

      mesh.GenerateMissingNormals();

      Assert.AreEqual(new Vector3(0, 0, 1), mesh.Vertices.ElementAt(0).Normal);
      Assert.AreEqual(new Vector3(0, 0, 1), mesh.Vertices.ElementAt(1).Normal);
      Assert.AreEqual(new Vector3(0, 0, 1), mesh.Vertices.ElementAt(2).Normal);
    }

    [Test()]
    public void GenerateMissingNormals_OneMisingFromOneFace_OnNormalsIsZPlusOthersStayTheSame()
    {
      var mesh = CreateMesh();
      mesh.AddVertex(new Vertex(new Vector3(1, 1, 0), new Vector3(), new Vector2(0, 0)));
      mesh.AddVertex(new Vertex(new Vector3(-1, 1, 0), new Vector3(1, 1, 1).Normalized(), new Vector2(0, 1)));
      mesh.AddVertex(new Vertex(new Vector3(1, -1, 0), new Vector3(1, 0, 1).Normalized(), new Vector2(1, 1)));
      mesh.AddFace(new Face(0, 1, 2));

      mesh.GenerateMissingNormals();

      Assert.AreEqual(new Vector3(0, 0, 1), mesh.Vertices.ElementAt(0).Normal);
      Assert.AreEqual(new Vector3(1, 1, 1).Normalized(), mesh.Vertices.ElementAt(1).Normal);
      Assert.AreEqual(new Vector3(1, 0, 1).Normalized(), mesh.Vertices.ElementAt(2).Normal);
    }

    [Test()]
    public void GenerateMissingNormals_AllMisingFromTwoFaces_AllNormalsAreCorrect()
    {
      var mesh = CreateMesh();
      mesh.AddVertex(new Vertex(new Vector3(1, 1, 0), new Vector3(), new Vector2(0, 0)));
      mesh.AddVertex(new Vertex(new Vector3(-1, 1, 0), new Vector3(), new Vector2(0, 1)));
      mesh.AddVertex(new Vertex(new Vector3(1, -1, 0), new Vector3(), new Vector2(1, 1)));
      mesh.AddVertex(new Vertex(new Vector3(1, 1, 1), new Vector3(), new Vector2(1, 1)));
      mesh.AddFace(new Face(0, 1, 2));
      mesh.AddFace(new Face(0, 3, 2));

      mesh.GenerateMissingNormals();

      Assert.AreEqual(new Vector3(1, 0, 1).Normalized(), mesh.Vertices.ElementAt(0).Normal);
      Assert.AreEqual(new Vector3(0, 0, 1).Normalized(), mesh.Vertices.ElementAt(1).Normal);
      Assert.AreEqual(new Vector3(1, 0, 1).Normalized(), mesh.Vertices.ElementAt(2).Normal);
      Assert.AreEqual(new Vector3(1, 0, 0).Normalized(), mesh.Vertices.ElementAt(3).Normal);
    }

    private static Geometry CreateMesh()
    {
      var meshLoaderMock = new Mock<IGeometryLoader>();
      var mesh = new Geometry(meshLoaderMock.Object);
      return mesh;
    }
  }
}
