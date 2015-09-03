using System;
using System.Linq;
using Starter3D.API.resources;
using Starter3D.API.scene.nodes;

namespace Starter3D.Plugin.SceneGraph
{
  public class NumericParameterViewModel : ViewModelBase
  {
    private InteractiveShapeNode _shape;
    private readonly string _name;
    private readonly Func<InteractiveShapeNode, float> _paramFunc;
    private readonly Action<InteractiveShapeNode, float> _setParamFunc;
    private float _value;

    public string Name
    {
      get { return _name; }
    }

    public float Value
    {
      get { return _value; }
      set
      {
        if (_value != value)
        {
          _value = value;
          OnValueChanged();
          OnPropertyChanged(() => Value);
        }
      }
    }

    public NumericParameterViewModel(InteractiveShapeNode shape, string name, Func<InteractiveShapeNode, float> paramFunc, Action<InteractiveShapeNode, float> setParamFunc)
    {
      _name = name;
      _paramFunc = paramFunc;
      _setParamFunc = setParamFunc;
      SetShape(shape);
    }

    private void SetShape(InteractiveShapeNode shape)
    {
      _shape = shape;
      _value = _paramFunc(_shape);
    }

    private void OnValueChanged()
    {
      _setParamFunc(_shape, _value);
    }
    
  }
}