using System.IO;
using System.Linq;
using NUnit.Framework;
using OpenTK;
using Starter3D.API.geometry;
using Starter3D.API.geometry.factories;
using Starter3D.API.geometry.loaders;

namespace ThreeAPI.Test.geometry
{
  [TestFixture()]
  public class ObjMeshLoaderTest
  {
    [Test()]
    public void LoadObjFile_FileIsEmpty_AssertZeroVerticesZeroFaces()
    {
      var testObj = @"";
      File.WriteAllText("test.obj", testObj);

      var mesh = new Mesh(CreateObjMeshLoader());
      mesh.Load("test.obj");

      Assert.AreEqual(0, mesh.Vertices.Count());
      Assert.AreEqual(0, mesh.Faces.Count());
    }

    [Test()]
    public void LoadObjFile_SquareWithOnlyPositionsMadeOfTriangles_AssertFourVerticesTwoFaces()
    {
      WriteSquareWithOnlyPositionsMadeOfTrianglesObj();

      var mesh = new Mesh(CreateObjMeshLoader());
      mesh.Load("test.obj");

      Assert.AreEqual(4, mesh.Vertices.Count());
      Assert.AreEqual(2, mesh.Faces.Count());
    }

    [Test()]
    public void LoadObjFile_SquareWithOnlyPositionsMadeOfTriangles_AssertTwoTriangles()
    {
      WriteSquareWithOnlyPositionsMadeOfTrianglesObj();

      var mesh = new Mesh(CreateObjMeshLoader());
      mesh.Load("test.obj");
      var triangles = mesh.GetTriangles();

      Assert.AreEqual(2, triangles.Count());
    }

    [Test()]
    public void LoadObjFile_SquareWithOnlyPositionsMadeOfTriangles_AssertCorrectVertices()
    {
      WriteSquareWithOnlyPositionsMadeOfTrianglesObj();

      var mesh = new Mesh(CreateObjMeshLoader());
      mesh.Load("test.obj");

      Assert.AreEqual(new Vertex(new Vector3(1, 1, 0), new Vector3(0, 0, -1), new Vector2()), mesh.Vertices.ElementAt(0));
      Assert.AreEqual(new Vertex(new Vector3(1, -1, 0), new Vector3(0, 0, -1), new Vector2()), mesh.Vertices.ElementAt(1));
      Assert.AreEqual(new Vertex(new Vector3(-1, -1, 0), new Vector3(0, 0, -1), new Vector2()), mesh.Vertices.ElementAt(2));
      Assert.AreEqual(new Vertex(new Vector3(-1, 1, 0), new Vector3(0, 0, -1), new Vector2()), mesh.Vertices.ElementAt(3));
    }

    [Test()]
    public void LoadObjFile_SquareWithOnlyPositionsMadeOfTriangles_AssertCorrectFaces()
    {
      WriteSquareWithOnlyPositionsMadeOfTrianglesObj();

      var mesh = new Mesh(CreateObjMeshLoader());
      mesh.Load("test.obj");

      Assert.AreEqual(new Face(0, 1, 2), mesh.Faces.ElementAt(0));
      Assert.AreEqual(new Face(2, 3, 0), mesh.Faces.ElementAt(1));
    }

    [Test()]
    public void LoadObjFile_SquareWithOnlyPositionsMadeOfTriangles_AssertCorrectTriangles()
    {
      WriteSquareWithOnlyPositionsMadeOfTrianglesObj();

      var mesh = new Mesh(CreateObjMeshLoader());
      mesh.Load("test.obj");
      var triangles = mesh.GetTriangles();

      Assert.AreEqual(new Vertex(new Vector3(1, 1, 0), new Vector3(0, 0, -1), new Vector2()), triangles.ElementAt(0).Vertices.ElementAt(0));
      Assert.AreEqual(new Vertex(new Vector3(1, -1, 0), new Vector3(0, 0, -1), new Vector2()), triangles.ElementAt(0).Vertices.ElementAt(1));
      Assert.AreEqual(new Vertex(new Vector3(-1, -1, 0), new Vector3(0, 0, -1), new Vector2()), triangles.ElementAt(0).Vertices.ElementAt(2));

      Assert.AreEqual(new Vertex(new Vector3(-1, -1, 0), new Vector3(0, 0, -1), new Vector2()), triangles.ElementAt(1).Vertices.ElementAt(0));
      Assert.AreEqual(new Vertex(new Vector3(-1, 1, 0), new Vector3(0, 0, -1), new Vector2()), triangles.ElementAt(1).Vertices.ElementAt(1));
      Assert.AreEqual(new Vertex(new Vector3(1, 1, 0), new Vector3(0, 0, -1), new Vector2()), triangles.ElementAt(1).Vertices.ElementAt(2));
    }

    [Test()]
    public void LoadObjFile_SquareWithOnlyPositionsMadeOfQuads_AssertFourVerticesOneFace()
    {
      WriteSquareWithOnlyPositionsMadeOfQuadsObj();

      var mesh = new Mesh(CreateObjMeshLoader());
      mesh.Load("test.obj");

      Assert.AreEqual(4, mesh.Vertices.Count());
      Assert.AreEqual(1, mesh.Faces.Count());
    }

    [Test()]
    public void LoadObjFile_SquareWithOnlyPositionsMadeOfQuads_AssertTwoTriangles()
    {
      WriteSquareWithOnlyPositionsMadeOfQuadsObj();

      var mesh = new Mesh(CreateObjMeshLoader());
      mesh.Load("test.obj");
      var triangles = mesh.GetTriangles();

      Assert.AreEqual(2, triangles.Count());
    }

    [Test()]
    public void LoadObjFile_SquareWithOnlyPositionsMadeOfQuads_AssertCorrectVertices()
    {
      WriteSquareWithOnlyPositionsMadeOfQuadsObj();

      var mesh = new Mesh(CreateObjMeshLoader());
      mesh.Load("test.obj");

      Assert.AreEqual(new Vertex(new Vector3(1, 1, 0), new Vector3(0, 0, -1), new Vector2()), mesh.Vertices.ElementAt(0));
      Assert.AreEqual(new Vertex(new Vector3(1, -1, 0), new Vector3(0, 0, -1), new Vector2()), mesh.Vertices.ElementAt(1));
      Assert.AreEqual(new Vertex(new Vector3(-1, -1, 0), new Vector3(0, 0, -1), new Vector2()), mesh.Vertices.ElementAt(2));
      Assert.AreEqual(new Vertex(new Vector3(-1, 1, 0), new Vector3(0, 0, -1), new Vector2()), mesh.Vertices.ElementAt(3));
    }

    [Test()]
    public void LoadObjFile_SquareWithOnlyPositionsMadeOfQuads_AssertCorrectFaces()
    {
      WriteSquareWithOnlyPositionsMadeOfQuadsObj();

      var mesh = new Mesh(CreateObjMeshLoader());
      mesh.Load("test.obj");

      Assert.AreEqual(new Face(0, 1, 2, 3), mesh.Faces.ElementAt(0));
    }

    [Test()]
    public void LoadObjFile_SquareWithOnlyPositionsMadeOfQuads_AssertCorrectTriangles()
    {
      WriteSquareWithOnlyPositionsMadeOfQuadsObj();

      var mesh = new Mesh(CreateObjMeshLoader());
      mesh.Load("test.obj");
      var triangles = mesh.GetTriangles();

      Assert.AreEqual(new Vertex(new Vector3(1, 1, 0), new Vector3(0, 0, -1), new Vector2()), triangles.ElementAt(0).Vertices.ElementAt(0));
      Assert.AreEqual(new Vertex(new Vector3(1, -1, 0), new Vector3(0, 0, -1), new Vector2()), triangles.ElementAt(0).Vertices.ElementAt(1));
      Assert.AreEqual(new Vertex(new Vector3(-1, -1, 0), new Vector3(0, 0, -1), new Vector2()), triangles.ElementAt(0).Vertices.ElementAt(2));

      Assert.AreEqual(new Vertex(new Vector3(-1, -1, 0), new Vector3(0, 0, -1), new Vector2()), triangles.ElementAt(1).Vertices.ElementAt(0));
      Assert.AreEqual(new Vertex(new Vector3(-1, 1, 0), new Vector3(0, 0, -1), new Vector2()), triangles.ElementAt(1).Vertices.ElementAt(1));
      Assert.AreEqual(new Vertex(new Vector3(1, 1, 0), new Vector3(0, 0, -1), new Vector2()), triangles.ElementAt(1).Vertices.ElementAt(2));
    }

    [Test()]
    public void LoadObjFile_SquareWithPositionsAndTextureCoordinatesMadeOfTriangles_AssertFourVerticesTwoFaces()
    {
      WriteSquareWithPositionsAndTextureCoordinatesMadeOfTrianglesObj();

      var mesh = new Mesh(CreateObjMeshLoader());
      mesh.Load("test.obj");

      Assert.AreEqual(4, mesh.Vertices.Count());
      Assert.AreEqual(2, mesh.Faces.Count());
    }

    [Test()]
    public void LoadObjFile_SquareWithPositionsAndTextureCoordinatesMadeOfTriangles_AssertCorrectVertices()
    {
      WriteSquareWithPositionsAndTextureCoordinatesMadeOfTrianglesObj();

      var mesh = new Mesh(CreateObjMeshLoader());
      mesh.Load("test.obj");

      Assert.AreEqual(new Vertex(new Vector3(1, 1, 0), new Vector3(0, 0, -1), new Vector2(0, 0)), mesh.Vertices.ElementAt(0));
      Assert.AreEqual(new Vertex(new Vector3(1, -1, 0), new Vector3(0, 0, -1), new Vector2(0, 1)), mesh.Vertices.ElementAt(1));
      Assert.AreEqual(new Vertex(new Vector3(-1, -1, 0), new Vector3(0, 0, -1), new Vector2(1, 1)), mesh.Vertices.ElementAt(2));
      Assert.AreEqual(new Vertex(new Vector3(-1, 1, 0), new Vector3(0, 0, -1), new Vector2(1, 0)), mesh.Vertices.ElementAt(3));
    }

    [Test()]
    public void LoadObjFile_SquareWithPositionsTextureCoordinatesAndNormalsMadeOfTriangles_AssertFourVerticesTwoFaces()
    {
      WriteSquareWithPositionsTextureCoordinatesAndNormalsMadeOfTrianglesObj();

      var mesh = new Mesh(CreateObjMeshLoader());
      mesh.Load("test.obj");

      Assert.AreEqual(4, mesh.Vertices.Count());
      Assert.AreEqual(2, mesh.Faces.Count());
    }

    [Test()]
    public void LoadObjFile_SquareWithPositionsTextureCoordinatesAndNormalsMadeOfTriangles_AssertCorrectVertices()
    {
      WriteSquareWithPositionsTextureCoordinatesAndNormalsMadeOfTrianglesObj();

      var mesh = new Mesh(CreateObjMeshLoader());
      mesh.Load("test.obj");

      Assert.AreEqual(new Vertex(new Vector3(1, 1, 0), new Vector3(1, 1, 1).Normalized(), new Vector2(0, 0)), mesh.Vertices.ElementAt(0));
      Assert.AreEqual(new Vertex(new Vector3(1, -1, 0), new Vector3(1, 1, 1).Normalized(), new Vector2(0, 1)), mesh.Vertices.ElementAt(1));
      Assert.AreEqual(new Vertex(new Vector3(-1, -1, 0), new Vector3(1, 1, 1).Normalized(), new Vector2(1, 1)), mesh.Vertices.ElementAt(2));
      Assert.AreEqual(new Vertex(new Vector3(-1, 1, 0), new Vector3(1, 1, 1).Normalized(), new Vector2(1, 0)), mesh.Vertices.ElementAt(3));
    }

    [Test()]
    public void LoadObjFile_SquareWithPositionsAndNormalsMadeOfTriangles_AssertFourVerticesTwoFaces()
    {
      WriteSquareWithPositionsAndNormalsMadeOfTrianglesObj();

      var mesh = new Mesh(CreateObjMeshLoader());
      mesh.Load("test.obj");

      Assert.AreEqual(4, mesh.Vertices.Count());
      Assert.AreEqual(2, mesh.Faces.Count());
    }

    [Test()]
    public void LoadObjFile_SquareWithPositionsAndNormalsMadeOfTriangles_AssertCorrectVertices()
    {
      WriteSquareWithPositionsAndNormalsMadeOfTrianglesObj();

      var mesh = new Mesh(CreateObjMeshLoader());
      mesh.Load("test.obj");

      Assert.AreEqual(new Vertex(new Vector3(1, 1, 0), new Vector3(1, 1, 1).Normalized(), new Vector2()), mesh.Vertices.ElementAt(0));
      Assert.AreEqual(new Vertex(new Vector3(1, -1, 0), new Vector3(1, 1, 1).Normalized(), new Vector2()), mesh.Vertices.ElementAt(1));
      Assert.AreEqual(new Vertex(new Vector3(-1, -1, 0), new Vector3(1, 1, 1).Normalized(), new Vector2()), mesh.Vertices.ElementAt(2));
      Assert.AreEqual(new Vertex(new Vector3(-1, 1, 0), new Vector3(1, 1, 1).Normalized(), new Vector2()), mesh.Vertices.ElementAt(3));
    }

    private static void WriteSquareWithOnlyPositionsMadeOfTrianglesObj()
    {
      var testObj = @"v 1.0 1.0 0.0
                      v 1.0 -1.0 0.0
                      v -1.0 -1.0 0.0
                      v -1.0 1.0 0.0
                      f 1 2 3
                      f 3 4 1
                    ";
      File.WriteAllText("test.obj", testObj);
    }

    private static void WriteSquareWithPositionsAndTextureCoordinatesMadeOfTrianglesObj()
    {
      var testObj = @"v 1.0 1.0 0.0
                      v 1.0 -1.0 0.0
                      v -1.0 -1.0 0.0
                      v -1.0 1.0 0.0
                      vt 0 0
                      vt 0 1
                      vt 1 1
                      vt 1 0
                      f 1/1 2/2 3/3
                      f 3/3 4/4 1/1
                    ";
      File.WriteAllText("test.obj", testObj);
    }

    private static void WriteSquareWithPositionsTextureCoordinatesAndNormalsMadeOfTrianglesObj()
    {
      var testObj = @"v 1.0 1.0 0.0
                      v 1.0 -1.0 0.0
                      v -1.0 -1.0 0.0
                      v -1.0 1.0 0.0
                      vt 0 0
                      vt 0 1
                      vt 1 1
                      vt 1 0
                      vn 1 1 1
                      f 1/1/1 2/2/1 3/3/1
                      f 3/3/1 4/4/1 1/1/1
                    ";
      File.WriteAllText("test.obj", testObj);
    }

    private static void WriteSquareWithPositionsAndNormalsMadeOfTrianglesObj()
    {
      var testObj = @"v 1.0 1.0 0.0
                      v 1.0 -1.0 0.0
                      v -1.0 -1.0 0.0
                      v -1.0 1.0 0.0
                      vn 1 1 1
                      f 1//1 2//1 3//1
                      f 3//1 4//1 1//1
                    ";
      File.WriteAllText("test.obj", testObj);
    }

    private static void WriteSquareWithOnlyPositionsMadeOfQuadsObj()
    {
      var testObj = @"v 1.0 1.0 0.0
                      v 1.0 -1.0 0.0
                      v -1.0 -1.0 0.0
                      v -1.0 1.0 0.0
                      f 1 2 3 4
                    ";
      File.WriteAllText("test.obj", testObj);
    }

    private static ObjMeshLoader CreateObjMeshLoader()
    {
      var vertexFactory = new VertexFactory();
      var faceFactory = new FaceFactory();
      var meshLoader = new ObjMeshLoader(vertexFactory, faceFactory);
      return meshLoader;
    }
  }
}
