using System;
using System.Drawing;
using System.IO;
using System.Linq;

using OpenTK;
using OpenTK.Input;
using GL = OpenTK.Graphics.OpenGL.GL;
using EnableCap = OpenTK.Graphics.OpenGL.EnableCap;
using OpenTK.Graphics.OpenGL;
using Starter3D.API.OpenGLRendering;
using ThreeAPI.geometry;
using ThreeAPI.renderer;
using ThreeAPI.scene.nodes;
using ThreeAPI.scene.persistence;

namespace ThreeAPI.examples
{
  public class PlainWindow: GameWindow
  {
    public static int WindowWidth = 512;
    public static int WindowHeight = 512;
    public static float FrameRate = 60;
    
    private IMesh _mesh;
    private IRenderer _renderer;
    private ISceneNode _scene;

    public PlainWindow (int width, int height)
      : base(width, height,
        new OpenTK.Graphics.GraphicsMode(), "Starter3D", GameWindowFlags.Default,
        DisplayDevice.Default, 3, 0,
        OpenTK.Graphics.GraphicsContextFlags.ForwardCompatible | OpenTK.Graphics.GraphicsContextFlags.Debug)
    {
      _renderer = new OpenGLRenderer();
      var xmlReader = XMLDataNodeReader.CreateReader();
      _scene = xmlReader.Read("scenes/testMeshScene.xml");
      _mesh = (IMesh)((ShapeNode)_scene.Children.First()).Shape;
    }

    protected override void OnRenderFrame(FrameEventArgs e)
    {
      GL.Viewport(0, 0, this.Width, this.Height);
      GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

      // draw object
     _renderer.Render(_mesh);

      GL.Flush();
      SwapBuffers();
    }

    protected override void OnLoad(EventArgs e)
    {
      _renderer.ConfigureMesh(_mesh);

      GL.Enable( EnableCap.DepthTest );
      GL.ClearColor(Color.AliceBlue);

    }

  }
}

