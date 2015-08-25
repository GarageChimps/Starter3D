using Starter3D.API.scene.nodes;

namespace Starter3D.Plugin.MaterialEditor
{
  public class ShapeViewModel : ViewModelBase
  {
    private readonly ShapeNode _shape;


    public ShapeNode Shape
    {
      get { return _shape; }
    }

    public ShapeViewModel(ShapeNode shape)
    {
      _shape = shape;
    }

    
  }
}