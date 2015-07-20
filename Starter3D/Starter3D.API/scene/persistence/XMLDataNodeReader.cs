using System.Linq;
using System.Xml.Linq;

namespace Starter3D
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

    public static XMLDataNodeReader CreateReader(){
      var vertexFactory = new VertexFactory();
      var faceFactory = new FaceFactory();
      var meshLoaderFactory = new MeshLoaderFactory(vertexFactory, faceFactory);
      var meshFactory = new MeshFactory(meshLoaderFactory);
      var shapeFactory = new ShapeFactory(meshFactory);
      var sceneNodeFactory = new SceneNodeFactory(shapeFactory);
      var dataNodeFactory = new DataNodeFactory(sceneNodeFactory);
      var xmlReader = new XMLDataNodeReader(dataNodeFactory);
      return xmlReader;
    }
  }
}