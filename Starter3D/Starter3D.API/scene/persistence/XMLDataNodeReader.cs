using System.Linq;
using System.Xml.Linq;
using Starter3D.API.scene.nodes;
using Starter3D.API.scene.persistence.factories;

namespace Starter3D.API.scene.persistence
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