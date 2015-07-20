using System.Xml.Linq;


namespace Starter3D
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