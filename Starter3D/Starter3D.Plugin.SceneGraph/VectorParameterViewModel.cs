using System;
using OpenTK;
using Starter3D.API.scene.nodes;

namespace Starter3D.Plugin.SceneGraph
{
  public class VectorParameterViewModel : ViewModelBase
  {
    private InteractiveShapeNode _shape;
    private readonly string _name;
    private readonly Func<InteractiveShapeNode, Vector3> _paramFunc;
    private readonly Action<InteractiveShapeNode, Vector3> _setParamFunc;
    private float _x;
    private float _y;
    private float _z;

    public string Name
    {
      get { return _name; }
    }

    public float X
    {
      get { return _x; }
      set
      {
        if (_x != value)
        {
          _x = value;
          OnValueChanged();
          OnPropertyChanged(() => X);
        }
      }
    }

    public float Y
    {
      get { return _y; }
      set
      {
        if (_y != value)
        {
          _y = value;
          OnValueChanged();
          OnPropertyChanged(() => Y);
        }
      }
    }

    public float Z
    {
      get { return _z; }
      set
      {
        if (_z != value)
        {
          _z = value;
          OnValueChanged();
          OnPropertyChanged(() => Z);
        }
      }
    }

    public VectorParameterViewModel(InteractiveShapeNode shape, string name, Func<InteractiveShapeNode, Vector3> paramFunc, Action<InteractiveShapeNode, Vector3> setParamFunc)
    {
      _name = name;
      _paramFunc = paramFunc;
      _setParamFunc = setParamFunc;

      SetShape(shape);
    }

    private void SetShape(InteractiveShapeNode shape)
    {
      _shape = shape;
      var vector = _paramFunc(_shape);
      _x = vector.X;
      _y = vector.Y;
      _z = vector.Z;
    }

    private void OnValueChanged()
    {
      _setParamFunc(_shape, new Vector3(_x, _y, _z));
    }

   
  }
}