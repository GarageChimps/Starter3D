using System.Collections.ObjectModel;
using System.Linq;
using Starter3D.API.ui;

namespace Starter3D.Plugin.PixelShader
{
  public class PixelShaderUserInterface : ViewModelBase, IUserInterface
  {
    private readonly PixelShaderController _controller;
    private readonly PixelShaderView _view;

    private readonly  ObservableCollection<ShaderViewModel> _shaders = new ObservableCollection<ShaderViewModel>();
    private ShaderViewModel _currentShader;
    public object View
    {
      get { return _view; }
    }

    public ObservableCollection<ShaderViewModel> Shaders
    {
      get { return _shaders; }
    }

    public ShaderViewModel CurrentShader
    {
      get { return _currentShader; }
      set
      {
        if (_currentShader != value)
        {
          _currentShader = value;
          OnCurrentShaderChanged();
          OnPropertyChanged(() => CurrentShader);
        }
      }
    }

    public PixelShaderUserInterface(PixelShaderController controller)
    {
      _controller = controller;
      _view = new PixelShaderView();
      _view.DataContext = this;

      var shaders = controller.GetShaders();
      foreach (var shader in shaders)
      {
        _shaders.Add(new ShaderViewModel(shader));
      }
      CurrentShader = _shaders.First();
    }

    private void OnCurrentShaderChanged()
    {
      _controller.SetCurrentShader(_currentShader.Shader);
    }
  }
}