using System;
using OpenTK;
using ThreeAPI.geometry;

namespace ThreeAPI
{
  public static class OpenGLMesh
  {
    public static Vector3[] vertexPositions(IMesh mesh){

      Vector3[] positions = new Vector3[mesh.VerticesCount];
      int i = 0;
      foreach(IVertex v in mesh.Vertices) {
        positions [i] = v.Position;
        i++;
      }
      return positions;
    }

    public static uint[] faceIndices(IMesh mesh){

      uint[] faces = new uint[mesh.FacesCount * 3];
      int i = 0;
      foreach (IFace f in mesh.Faces) {
        foreach (int indx in f.VertexIndices) {
          faces [i] = (uint)indx;
          i++;
        }
      }
      return faces;
    }
    public static int triangleCount(IMesh mesh){
      return mesh.FacesCount * 3;
    }
  }
}

