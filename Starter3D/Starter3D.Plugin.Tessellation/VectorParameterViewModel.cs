using System.Linq;
using OpenTK;
using Starter3D.API.resources;

namespace Starter3D.Plugin.Tessellation
{
  public class VectorParameterViewModel : ViewModelBase
  {
    private readonly IMaterial _material;
    private readonly string _name;
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

    public VectorParameterViewModel(IMaterial material, string name)
    {
      _material = material;
      _name = name;
      _x = material.VectorParameters.First(kv => kv.Key == name).Value.X;
      _y = material.VectorParameters.First(kv => kv.Key == name).Value.Y;
      _z = material.VectorParameters.First(kv => kv.Key == name).Value.Z;
    }
    
    private void OnValueChanged()
    {
      _material.SetParameter(_name, new Vector3(_x, _y, _z));
    }

   
  }
}