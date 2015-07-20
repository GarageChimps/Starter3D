using System;
using System.Drawing;
using System.IO;
using System.Linq;

using OpenTK;
using OpenTK.Input;
using GL = OpenTK.Graphics.OpenGL.GL;
using EnableCap = OpenTK.Graphics.OpenGL.EnableCap;
using OpenTK.Graphics.OpenGL;
using Starter3D;
using ThreeAPI.renderer;

namespace ThreeAPI.examples
{
  public class PlainWindow: GameWindow
  {
    public static int WindowWidth = 512;
    public static int WindowHeight = 512;
    public static float FrameRate = 60;

    private string PixelShaderFilePath = Path.Combine("Shaders", "PositionFragmentShader.glsl");
    private string VertexShaderFilePath = Path.Combine("Shaders", "PositionVertexShader.glsl");
    private int _programHandle;
    private int objHandle;

    private IGeometry mesh;

    public PlainWindow (int width, int height)
      : base(width, height,
        new OpenTK.Graphics.GraphicsMode(), "Starter3D", GameWindowFlags.Default,
        DisplayDevice.Default, 3, 0,
        OpenTK.Graphics.GraphicsContextFlags.ForwardCompatible | OpenTK.Graphics.GraphicsContextFlags.Debug)
    {
      var xmlReader = XMLDataNodeReader.CreateReader();
      var scene = xmlReader.Read("scenes/testMeshScene.xml");
      mesh = (IGeometry)((ShapeNode)scene.Children.First ()).Shape;
    }

    protected override void OnRenderFrame(FrameEventArgs e)
    {
      GL.Viewport(0, 0, this.Width, this.Height);
      GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

      // draw object
      GL.BindVertexArray(objHandle);
      GL.DrawElements( BeginMode.Triangles, OpenGLMesh.triangleCount(mesh),
        DrawElementsType.UnsignedInt, IntPtr.Zero );

      GL.Flush();
      SwapBuffers();
    }

    protected override void OnLoad(EventArgs e)
    {
      string fs_src = File.ReadAllText(PixelShaderFilePath);
      string vs_src= File.ReadAllText(VertexShaderFilePath);
      _programHandle = OpenGLHelper.CreateProgram (vs_src, fs_src);

      GL.UseProgram( _programHandle );

      GL.GenVertexArrays(1, out  objHandle);
      GL.BindVertexArray(objHandle);

      OpenGLHelper.LoadVertexPositions(_programHandle, OpenGLMesh.vertexPositions(mesh));
      OpenGLHelper.LoadIndexer(_programHandle, OpenGLMesh.faceIndices(mesh));

      GL.Enable( EnableCap.DepthTest );
      GL.ClearColor(Color.AliceBlue);

    }

  }
}

