using System.Xml.Linq;
using ThreeAPI.scene.nodes.factories;

namespace ThreeAPI.scene.persistence.factories
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