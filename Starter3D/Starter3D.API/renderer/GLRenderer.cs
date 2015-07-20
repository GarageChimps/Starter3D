using System;
using System.Drawing;
using System.Collections;
using System.Collections.Generic;

using OpenTK;
using OpenTK.Input;
using GL = OpenTK.Graphics.OpenGL.GL;
using EnableCap = OpenTK.Graphics.OpenGL.EnableCap;
using OpenTK.Graphics.OpenGL;

using ThreeAPI.renderer;

namespace Starter3D
{
  public class GLRenderer
  {
    Dictionary<IGeometry, int> glObjects;

    public GLRenderer ()
    {
    }

    public void SetSize(float width, float height){
    }

    public void Render(Scene scene, Camera camera){
      // TODO: i imagen this will get split later
      // adding processing of lights, sprites, 
      // opactity ... etc
      // For now lets just render the objects
      RenderObjects(scene, camera);
    }

    public void RenderObjects(Scene scene, Camera camera){
      Matrix4 transform = Matrix4.Identity;
      Stack mStack = new Stack();

      // we are recalculating the matrix in every frame
      // ideally we should have it precalculated and only re-
      // calculate on demand.

      Action<ISceneNode> visit = delegate(ISceneNode node) {
        mStack.Push(transform);
        transform *= node.Transform;
        RenderNode(node, transform, camera);
      };

      Action<ISceneNode> finish = delegate(ISceneNode node) {
        mStack.Pop ();
      };

      scene.Traverse(visit, finish);
    }
    public void RenderNode(ISceneNode node, Matrix4 nodeTransfor, Camera camera){
      if (node is IMesh) {
        IMesh mesh = (IMesh)node;
        IGeometry geometry = mesh.Geometry;

        int objHandle;
        if (!glObjects.TryGetValue (mesh.Geometry, out objHandle)) {
          objHandle = CreateHandle (geometry); 
          glObjects.Add (geometry, objHandle);
        }
          
        GL.BindVertexArray(objHandle);
        GL.DrawElements( BeginMode.Triangles, OpenGLMesh.triangleCount(geometry),
        DrawElementsType.UnsignedInt, IntPtr.Zero );
        
      }
    }
    public int CreateHandle(Geometry geometry){
      int objHandle;
      GL.GenVertexArrays(1, out  objHandle);
      GL.BindVertexArray(objHandle);

      OpenGLHelper.CreateBuffer(OpenGLMesh.vertexPositions(geometry));
      OpenGLHelper.CreateIndexer(OpenGLMesh.faceIndices(geometry));

    }
  }
}

