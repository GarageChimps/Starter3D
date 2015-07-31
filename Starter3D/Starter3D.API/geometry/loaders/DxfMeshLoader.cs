using System;
using System.Collections.Generic;
using System.IO;
using netDxf;
using Starter3D.API.geometry.factories;

namespace Starter3D.API.geometry.loaders
{
  public class DxfMeshLoader : IMeshLoader
  {
    private readonly IVertexFactory _vertexFactory;
    private readonly IFaceFactory _faceFactory;

    public DxfMeshLoader(IVertexFactory vertexFactory, IFaceFactory faceFactory)
    {
      if (vertexFactory == null) throw new ArgumentNullException("vertexFactory");
      if (faceFactory == null) throw new ArgumentNullException("faceFactory");
      _vertexFactory = vertexFactory;
      _faceFactory = faceFactory;
    }

    public void Load(IMesh mesh, string filePath)
    {
      var dxfLoad = DxfDocument.Load(filePath);
      var vertices = new List<IVertex>();
      var faces = new List<IFace>();
      var verticesIndexMap = new Dictionary<IVertex, int>();
      int currentVertexIndex = 0;
      foreach (var face3D in dxfLoad.Faces3d)
      {
        var v1 = face3D.FirstVertex;
        var v2 = face3D.SecondVertex;
        var v3 = face3D.ThirdVertex;
        var v4 = face3D.FourthVertex;
        currentVertexIndex = CreateTriangleFace(v1, v2, v3, verticesIndexMap, currentVertexIndex, vertices, faces);
        currentVertexIndex = CreateTriangleFace(v3, v4, v1, verticesIndexMap, currentVertexIndex, vertices, faces);
      }
      foreach (var vertex in vertices)
      {
        mesh.AddVertex(vertex);
      }
      foreach (var face in faces)
      {
        mesh.AddFace(face);
      }
    }

    private int CreateTriangleFace(Vector3 v1, Vector3 v2, Vector3 v3, Dictionary<IVertex, int> verticesIndexMap, int currentVertexIndex,
      List<IVertex> vertices, List<IFace> faces)
    {
      var dxfVertices = new List<Vector3>()
      {
        v1,
        v2,
        v3,
      };
      var faceIndices = new List<int>();
      foreach (var v in dxfVertices)
      {
        var vertex = _vertexFactory.CreateVertex(new OpenTK.Vector3((float) v.X, (float) v.Y, (float) v.Z),
          new OpenTK.Vector3(), new OpenTK.Vector3());
        if (!verticesIndexMap.ContainsKey(vertex))
        {
          verticesIndexMap.Add(vertex, currentVertexIndex);
          currentVertexIndex++;
          vertices.Add(vertex);
        }
        faceIndices.Add(verticesIndexMap[vertex]);
      }
      var face = _faceFactory.CreateFace(faceIndices);
      faces.Add(face);
      return currentVertexIndex;
    }
  }
}