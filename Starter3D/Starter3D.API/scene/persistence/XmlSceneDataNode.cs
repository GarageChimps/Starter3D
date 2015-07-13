using System;
using System.Xml.Linq;
using Starter3D.API.scene.nodes;
using Starter3D.API.scene.nodes.factories;
using Starter3D.API.scene.persistence.factories;
using Starter3D.API.utils;

namespace Starter3D.API.scene.persistence
{
  public class XmlSceneDataNode : ISceneDataNode
  {
    private readonly XElement _element;
    private readonly ISceneNodeFactory _sceneNodeFactory;
    private readonly ISceneDataNodeFactory _sceneDataNodeFactory;

    public XmlSceneDataNode(XElement element, ISceneNodeFactory sceneNodeFactory, ISceneDataNodeFactory sceneDataNodeFactory)
    {
      _element = element;
      _sceneNodeFactory = sceneNodeFactory;
      _sceneDataNodeFactory = sceneDataNodeFactory;
    }

    public ISceneNode Load()
    {
      var type = (SceneNodeType)Enum.Parse(typeof(SceneNodeType), _element.Name.ToString());
      var sceneNode = _sceneNodeFactory.CreateSceneNode(type);
      sceneNode.Load(this);
      foreach (var childElement in _element.Elements())
      {
        var childDataNode = _sceneDataNodeFactory.CreateXmlDataNode(childElement);
        var childSceneNode = childDataNode.Load();
        sceneNode.AddChild(childSceneNode);
      }
      return sceneNode;
    }

    public void WriteParameter(string parameterName, string parameterValue)
    {
      throw new System.NotImplementedException();
    }

    public bool HasParameter(string parameterName)
    {
      return _element.Attribute(parameterName) != null;
    }

    public string ReadParameter(string parameterName)
    {
      return _element.Attribute(parameterName).Value;
    }
  }
}