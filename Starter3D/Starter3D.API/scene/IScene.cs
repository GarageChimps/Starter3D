using System.Collections.Generic;
using Starter3D.API.renderer;
using Starter3D.API.scene.nodes;

namespace Starter3D.API.scene
{
  /// <summary>
  /// Interface for a scene that contains all the necessary elements that will be used for an interactive rendering application
  /// @author Alejandro Echeverria
  /// @data July-2015   
  /// </summary>
  public interface IScene
  {
    CameraNode CurrentCamera { get; }
    IEnumerable<CameraNode> Cameras { get; }
    IEnumerable<ShapeNode> Shapes { get; }
    IEnumerable<LightNode> Lights { get; }

    void AddShape(ShapeNode shape);
    void AddLight(LightNode light);
    void AddCamera(CameraNode camera);
    void NextCamera();

    void ClearShapes();
    void ClearLights();
    void ClearCameras();
    
    void Configure(IRenderer renderer);
    void Render(IRenderer renderer);
    
  }
}
