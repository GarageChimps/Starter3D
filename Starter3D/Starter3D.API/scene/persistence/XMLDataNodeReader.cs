using System.Linq;
using System.Xml.Linq;
using ThreeAPI.scene.nodes;
using ThreeAPI.scene.persistence.factories;

namespace ThreeAPI.scene.persistence
{
  public class XMLDataNodeReader: IXmlDataNodeReader
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