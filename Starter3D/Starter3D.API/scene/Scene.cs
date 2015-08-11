using System.Collections.Generic;
using System.Linq;
using Starter3D.API.renderer;
using Starter3D.API.scene.nodes;

namespace Starter3D.API.scene
{
  public class Scene : IScene
  {
    private int _currentCameraIndex;
    private readonly List<CameraNode> _cameras;
    private readonly List<ShapeNode> _shapes;
    private readonly List<LightNode> _lights;

    private bool _isDirty = true;

    public CameraNode CurrentCamera { get { return _cameras[_currentCameraIndex]; } }

    public IEnumerable<CameraNode> Cameras
    {
      get { return _cameras; }
    }

    public IEnumerable<ShapeNode> Shapes
    {
      get { return _shapes; }
    }

    public IEnumerable<LightNode> Lights
    {
      get { return _lights; }
    }

    public Scene(ISceneNode rootNode)
    {
      _cameras = rootNode.GetNodes<CameraNode>().OrderBy(c => c.Order).ToList();
      _lights = rootNode.GetNodes<LightNode>().ToList();
      _shapes = rootNode.GetNodes<ShapeNode>().ToList();
    }

    public Scene(List<CameraNode> cameras, List<ShapeNode> shapes, List<LightNode> lights)
    {
      _cameras = cameras;
      _lights = lights;
      _shapes = shapes;
    }

    public void AddShape(ShapeNode shape)
    {
      _shapes.Add(shape);
    }

    public void AddLight(LightNode light)
    {
      _lights.Add(light);
      _isDirty = true;
    }

    public void AddCamera(CameraNode camera)
    {
      _cameras.Add(camera);
    }


    public void ClearCameras()
    {
      _cameras.Clear();
    }

    public void ClearShapes()
    {
      _shapes.Clear();
    }

    public void ClearLights()
    {
      _lights.Clear();
      _isDirty = true;
    }

    public void Configure(IRenderer renderer)
    {
      CurrentCamera.Configure(renderer);
      _lights.ForEach(l => l.Configure(renderer));
      _shapes.ForEach(s => s.Configure(renderer));
    }

    public void Render(IRenderer renderer)
    {
      //AE August 2015: We need to tell the shaders
      if (_isDirty)
      {
        var pointLights = _lights.Where(l => l.GetType() == typeof(PointLight));
        var directionalLights = _lights.Where(l => l.GetType() == typeof(DirectionalLight));
        renderer.SetNumericParameter("activeNumberOfPointLights", pointLights.Count());
        renderer.SetNumericParameter("activeNumberOfDirectionalLights", directionalLights.Count());
        _isDirty = false;
      }

      CurrentCamera.Render(renderer);
      _lights.ForEach(l => l.Render(renderer));
      _shapes.ForEach(s => s.Render(renderer));
    }

    public void NextCamera()
    {
      _currentCameraIndex = (_currentCameraIndex + 1) % _cameras.Count;
    }


  }
}