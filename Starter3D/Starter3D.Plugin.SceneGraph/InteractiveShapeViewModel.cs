using Starter3D.API.scene.nodes;

namespace Starter3D.Plugin.SceneGraph
{
  public class InteractiveShapeViewModel : ViewModelBase
  {
    private InteractiveShapeNode _interactiveShape;
    private VectorParameterViewModel _translation;
    private VectorParameterViewModel _scaling;
    private VectorParameterViewModel _orientationAxis;
    private NumericParameterViewModel _angle;

    public InteractiveShapeViewModel(InteractiveShapeNode interactiveShape)
    {
      _interactiveShape = interactiveShape;
      _translation = new VectorParameterViewModel(_interactiveShape, "Translation", s => s.Position, (s, p) => s.Position = p);
      _scaling = new VectorParameterViewModel(_interactiveShape, "Scaling", s => s.Scale, (s, p) => s.Scale = p);
    }

    public VectorParameterViewModel Translation
    {
      get { return _translation; }
    }

    public VectorParameterViewModel Scaling
    {
      get { return _scaling; }
    }
  }
}