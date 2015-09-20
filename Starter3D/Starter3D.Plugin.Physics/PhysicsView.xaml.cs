#if WIN64
using System.Windows.Controls;

namespace Starter3D.Plugin.Physics
{
  /// <summary>
  /// Interaction logic for MaterialEditorUI.xaml
  /// </summary>
  public partial class PhysicsView : UserControl
  {
    public PhysicsView(PhysicsController controller)
    {
      InitializeComponent();
      this.DataContext = controller;

    }
  }
}
#else
namespace Starter3D.Plugin.SceneGraph
{
  /// <summary>
  /// Interaction logic for MaterialEditorUI.xaml
  /// </summary>
  public partial class SceneGraphView 
  {
    public SceneGraphView(SceneGraphController controller)
    {
      
    }
  }
}
#endif
