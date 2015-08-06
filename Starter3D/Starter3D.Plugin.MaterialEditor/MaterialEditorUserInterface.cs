using Starter3D.API.ui;

namespace Starter3D.Plugin.MaterialEditor
{
  public class MaterialEditorUserInterface : IUserInterface
  {
    private readonly MaterialEditorView _view;

    public object View
    {
      get { return _view; }
    }


    public MaterialEditorUserInterface()
    {
      _view = new MaterialEditorView();
    }
  }
}
