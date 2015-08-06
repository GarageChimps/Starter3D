using System.IO;
using System.Linq;
using System.Xml.Linq;
using Starter3D.API.scene.nodes;
using Starter3D.API.scene.persistence.factories;

namespace Starter3D.API.scene.persistence
{
  public class XmlSceneReader: ISceneReader
  {
    private readonly ISceneDataNodeFactory _sceneDataNodeFactory;
    
    public XmlSceneReader(ISceneDataNodeFactory sceneDataNodeFactory)
    {
      _sceneDataNodeFactory = sceneDataNodeFactory;
    }

    public ISceneNode Read(string filePath)
    {
      if(!File.Exists(filePath))
        return new BaseSceneNode();
      var xmlDoc = XDocument.Load(filePath);
      var element = xmlDoc.Elements("Scene").First();
      var node = _sceneDataNodeFactory.CreateXmlDataNode(element);
      return node.Load();
    }

    
  }
}