using System.Linq;
using Starter3D.API.resources;

namespace Starter3D.Plugin.Tessellation
{
  public class NumericParameterViewModel : ViewModelBase
  {
    private readonly IMaterial _material;
    private readonly string _name;
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

    public NumericParameterViewModel(IMaterial material, string name)
    {
      _material = material;
      _name = name;
      _value = _material.NumericParameters.First(kv => kv.Key == _name).Value;
    }

    private void OnValueChanged()
    {
      _material.SetParameter(_name, _value);
    }
    
  }
}