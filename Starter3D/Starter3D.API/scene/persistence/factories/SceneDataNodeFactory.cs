using System.Xml.Linq;
using Starter3D.API.scene.nodes.factories;

namespace Starter3D.API.scene.persistence.factories
{
  public class SceneDataNodeFactory : ISceneDataNodeFactory
  {
    private readonly ISceneNodeFactory _sceneNodeFactory;

    public SceneDataNodeFactory(ISceneNodeFactory sceneNodeFactory)
    {
      _sceneNodeFactory = sceneNodeFactory;
    }

    public ISceneDataNode CreateXmlDataNode(XElement element)
    {
      return new XmlSceneDataNode(element, _sceneNodeFactory, this);
    }
  }
}