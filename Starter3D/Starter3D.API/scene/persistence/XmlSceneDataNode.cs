using System;
using System.Globalization;
using System.Xml.Linq;
using OpenTK;
using OpenTK.Graphics;
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

    public float ReadFloatParameter(string parameterName)
    {
      var parameter = ReadParameter(parameterName);
      return float.Parse(parameter, CultureInfo.InvariantCulture);
    }

    public Vector3 ReadVectorParameter(string parameterName)
    {
      var parameter = ReadParameter(parameterName);
      var splitParamters = parameter.Split(',');
      return new Vector3(float.Parse(splitParamters[0], CultureInfo.InvariantCulture),
        float.Parse(splitParamters[1], CultureInfo.InvariantCulture), float.Parse(splitParamters[2], CultureInfo.InvariantCulture));
    }

    public Color4 ReadColorParameter(string parameterName)
    {
      var parameter = ReadParameter(parameterName);
      var splitParamters = parameter.Split(',');
      return new Color4(float.Parse(splitParamters[0], CultureInfo.InvariantCulture),
        float.Parse(splitParamters[1], CultureInfo.InvariantCulture), float.Parse(splitParamters[2], CultureInfo.InvariantCulture), 1);
    }
  }
}