using System.Xml.Linq;
using Starter3D.API.scene.nodes.factories;

namespace Starter3D.API.scene.persistence.factories
{
  public class DataNodeFactory : IDataNodeFactory
  {
    private readonly ISceneNodeFactory _sceneNodeFactory;

    public DataNodeFactory(ISceneNodeFactory sceneNodeFactory)
    {
      _sceneNodeFactory = sceneNodeFactory;
    }

    public IDataNode CreateXmlDataNode(XElement element)
    {
      return new XMLDataNode(element, _sceneNodeFactory, this);
    }
  }
}