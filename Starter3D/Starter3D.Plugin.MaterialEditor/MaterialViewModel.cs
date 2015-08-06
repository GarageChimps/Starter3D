using Starter3D.API.resources;

namespace Starter3D.Plugin.MaterialEditor
{
  public class MaterialViewModel : ViewModelBase
  {
    private readonly IMaterial _material;

    public IMaterial Material
    {
      get { return _material; }
    }

    public MaterialViewModel(IMaterial material)
    {
      _material = material;
    }

    
  }
}