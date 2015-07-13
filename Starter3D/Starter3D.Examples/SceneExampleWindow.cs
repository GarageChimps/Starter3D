using System;
using System.Drawing;
using System.Linq;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using Starter3D.API.geometry;
using Starter3D.API.renderer;
using Starter3D.API.resources;
using Starter3D.API.scene.nodes;
using Starter3D.API.scene.persistence;
using Starter3D.API.utils;
using GL = OpenTK.Graphics.OpenGL.GL;
using EnableCap = OpenTK.Graphics.OpenGL.EnableCap;

namespace Starter3D.Examples
{
  public class SceneExampleWindow : GameWindow
  {
    public static int WindowWidth = 512;
    public static int WindowHeight = 512;
    public static float FrameRate = 60;

    private readonly IRenderer _renderer;
    private readonly ISceneReader _sceneReader;
    private readonly IResourceManager _resourceManager;
    private ISceneNode _scene;
    private IMesh _mesh;

    public SceneExampleWindow(int width, int height, IRenderer renderer, ISceneReader sceneReader, IResourceManager resourceManager, IConfiguration configuration)
      : base(width, height,
        new OpenTK.Graphics.GraphicsMode(), "Starter3D", GameWindowFlags.Default,
        DisplayDevice.Default, 3, 0,
        OpenTK.Graphics.GraphicsContextFlags.ForwardCompatible | OpenTK.Graphics.GraphicsContextFlags.Debug)
    {
      _renderer = renderer;
      _sceneReader = sceneReader;
      _resourceManager = resourceManager;
      _resourceManager.Load(configuration.ResourcesPath);
      _scene = _sceneReader.Read(configuration.ScenePath);
      _mesh = _scene.GetNodes<ShapeNode>().First().Shape as IMesh;
      if (_mesh.HasNoValidNormal())
        _mesh.GenerateMissingNormals();
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
      _scene.ConfigureRenderer(_renderer);

      GL.Enable(EnableCap.DepthTest);
      GL.ClearColor(Color.AliceBlue);

    }

  }
}

