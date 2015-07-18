using System;
using System.Collections.Generic;
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
    private readonly ISceneNode _sceneGraph;
    private readonly IEnumerable<ShapeNode> _objects;
    private readonly IEnumerable<LightNode> _lights;
    private readonly IEnumerable<CameraNode> _cameras;
    private readonly CameraNode _camera;
    private readonly List<ISceneNode> _sceneElements = new List<ISceneNode>(); 

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
      _sceneGraph = _sceneReader.Read(configuration.ScenePath);
      _objects = _sceneGraph.GetNodes<ShapeNode>();
      _lights = _sceneGraph.GetNodes<LightNode>();
      _cameras = _sceneGraph.GetNodes<CameraNode>();
      _camera = _cameras.First();
      _sceneElements.AddRange(_objects);
      _sceneElements.AddRange(_lights);
      _sceneElements.Add(_camera);
    }

    protected override void OnRenderFrame(FrameEventArgs e)
    {
      GL.Viewport(0, 0, this.Width, this.Height);
      GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

      foreach (var sceneElement in _sceneElements)
      {
        sceneElement.Update(_renderer);
        sceneElement.Render(_renderer);
      }

      GL.Flush();
      SwapBuffers();
    }

    protected override void OnLoad(EventArgs e)
    {
      foreach (var sceneElement in _sceneElements)
      {
        sceneElement.Configure(_renderer);
      }

      GL.Enable(EnableCap.DepthTest);
      GL.ClearColor(Color.AliceBlue);

    }

  }
}

