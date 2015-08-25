using System;
using System.Collections.ObjectModel;
using Starter3D.API.resources;

namespace Starter3D.Plugin.MaterialEditor
{
  public class MaterialViewModel : ViewModelBase
  {
    private readonly IMaterial _material;
    private readonly ObservableCollection<NumericParameterViewModel> _numericParameters = new ObservableCollection<NumericParameterViewModel>();
    private readonly ObservableCollection<VectorParameterViewModel> _vectorParameters = new ObservableCollection<VectorParameterViewModel>(); 

    public IMaterial Material
    {
      get { return _material; }
    }

    public ObservableCollection<NumericParameterViewModel> NumericParameters
    {
      get { return _numericParameters; }
    }

    public ObservableCollection<VectorParameterViewModel> VectorParameters
    {
      get { return _vectorParameters; }
    }

    public MaterialViewModel(IMaterial material)
    {
      if (material == null) throw new ArgumentNullException("material");
      _material = material;
      foreach (var numericParameter in _material.NumericParameters)
      {
        _numericParameters.Add(new NumericParameterViewModel(_material, numericParameter.Key));
      }
      foreach (var vectorParameter in _material.VectorParameters)
      {
        _vectorParameters.Add(new VectorParameterViewModel(_material, vectorParameter.Key));
      }
    }
  }
}