using System.Linq;
using System.Xml.Linq;
using ThreeAPI.scene.nodes;
using ThreeAPI.scene.persistence.factories;
using ThreeAPI.scene.nodes.factories;
using ThreeAPI.geometry.factories;
using ThreeAPI.resources;

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

    public static XMLDataNodeReader CreateReader(IResourceManager resourceManager){
      var vertexFactory = new VertexFactory();
      var faceFactory = new FaceFactory();
      var meshLoaderFactory = new MeshLoaderFactory(vertexFactory, faceFactory);
      var meshFactory = new MeshFactory(meshLoaderFactory);
      var shapeFactory = new ShapeFactory(meshFactory);
      var sceneNodeFactory = new SceneNodeFactory(shapeFactory, resourceManager);
      var dataNodeFactory = new DataNodeFactory(sceneNodeFactory);
      var xmlReader = new XMLDataNodeReader(dataNodeFactory);
      return xmlReader;
    }
  }
}