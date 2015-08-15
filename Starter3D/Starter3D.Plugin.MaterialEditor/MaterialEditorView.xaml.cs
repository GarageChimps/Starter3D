#if WIN64
using System.Windows.Controls;

namespace Starter3D.Plugin.MaterialEditor
{
  /// <summary>
  /// Interaction logic for MaterialEditorUI.xaml
  /// </summary>
  public partial class MaterialEditorView : UserControl
  {
    public MaterialEditorView(MaterialEditorController controller)
    {
      InitializeComponent();
      this.DataContext = controller;

    }
  }
}
#else
namespace Starter3D.Plugin.MaterialEditor
{
  /// <summary>
  /// Interaction logic for MaterialEditorUI.xaml
  /// </summary>
  public partial class MaterialEditorView 
  {
    public MaterialEditorView(MaterialEditorController controller)
    {
      
    }
  }
}
#endif
