using System.Linq;
using System.Xml.Linq;
using ThreeAPI.scene.nodes;
using ThreeAPI.scene.persistence.factories;
using ThreeAPI.scene.nodes.factories;
using ThreeAPI.geometry.factories;
using ThreeAPI.resources;

namespace ThreeAPI.scene.persistence
{
  public class XMLDataNodeReader: ISceneNodeReader
  {
    private readonly IDataNodeFactory _dataNodeFactory;

    public XMLDataNodeReader(IDataNodeFactory dataNodeFactory)
    {
      _dataNodeFactory = dataNodeFactory;
    }

    public ISceneNode Read(string filePath)
    {
      var xmlDoc = XDocument.Load(filePath);
      var element = xmlDoc.Elements("Scene").First();
      var node = _dataNodeFactory.CreateXmlDataNode(element);
      return node.Load();
    }

    
  }
}