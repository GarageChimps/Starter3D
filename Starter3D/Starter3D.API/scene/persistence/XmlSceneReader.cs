using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using OpenTK;
using Starter3D.API.scene.nodes;
using Starter3D.API.scene.persistence.factories;
using Starter3D.API.utils;

namespace Starter3D.API.scene.persistence
{
  public class XmlSceneReader: ISceneReader
  {
    private readonly ISceneDataNodeFactory _sceneDataNodeFactory;
    
    public XmlSceneReader(ISceneDataNodeFactory sceneDataNodeFactory)
    {
      _sceneDataNodeFactory = sceneDataNodeFactory;
    }

    public IScene Read(string filePath)
    {
      if(!File.Exists(filePath))
        return new Scene(new BaseSceneNode());
      var xmlDoc = XDocument.Load(filePath);
      var element = xmlDoc.Elements("Scene").First();
      var node = _sceneDataNodeFactory.CreateXmlDataNode(element);
      var sceneGraph = node.Load();
      
      //AE August 2015: Light Nodes are special: they need an index that depends on the global number on light, so this is set after loading the complete scene. The indexing is independent for each type of light
      var lights = sceneGraph.GetNodes<LightNode>();
      var lightCountDictionary = new Dictionary<Type, int>(); 
      foreach (var light in lights)
      {
        if (!lightCountDictionary.ContainsKey(light.GetType()))
          lightCountDictionary[light.GetType()] = 0;
        light.Index = lightCountDictionary[light.GetType()];
        lightCountDictionary[light.GetType()] += 1;
      }
      return new Scene(sceneGraph);
    }

    
  }
}