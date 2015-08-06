using Starter3D.API.ui;

namespace Starter3D.Plugin.MaterialEditor
{
  public class MaterialEditorUserInterface : IUserInterface
  {
    private readonly MaterialEditorController _controller;
    private readonly MaterialEditorView _view;

    public object View
    {
      get { return _view; }
    }


    public MaterialEditorUserInterface(MaterialEditorController controller)
    {
      _controller = controller;
      _view = new MaterialEditorView();
      _view.DataContext = this;
    }
  }
}
