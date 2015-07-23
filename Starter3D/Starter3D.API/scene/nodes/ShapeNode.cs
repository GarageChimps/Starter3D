﻿using System;
using System.IO;
using OpenTK;
using Starter3D.API.geometry;
using Starter3D.API.geometry.factories;
using Starter3D.API.renderer;
using Starter3D.API.resources;
using Starter3D.API.scene.persistence;
using Starter3D.API.utils;

namespace Starter3D.API.scene.nodes
{
  public class ShapeNode : BaseSceneNode
  {
    private IShape _shape;
    private readonly IShapeFactory _shapeFactory;
    private readonly IResourceManager _resourceManager;
    private bool _hasTransform;
    private Vector3 _position;
    private Vector3 _orientationAxis;
    private float _orientationAngle;
    private Vector3 _scale;

    public IShape Shape
    {
      get { return _shape; }
    }

    public ShapeNode(IShape shape, IShapeFactory shapeFactory, IResourceManager resourceManager, Vector3 scale = default(Vector3), Vector3 position = default(Vector3), 
      Vector3 orientationAxis = default(Vector3), float orientationAngle = 0)
      : this(shapeFactory, resourceManager, scale, position, orientationAxis, orientationAngle)
    {
      _shape = shape;
    }

    public ShapeNode(IShapeFactory shapeFactory, IResourceManager resourceManager, Vector3 scale = default(Vector3), Vector3 position = default(Vector3),
      Vector3 orientationAxis = default(Vector3), float orientationAngle = 0)
    {
      _shapeFactory = shapeFactory;
      _resourceManager = resourceManager;
    }

    private void Init(Vector3 scale = default(Vector3), Vector3 position = default(Vector3),
      Vector3 orientationAxis = default(Vector3), float orientationAngle = 0)
    {
      _scale = scale;
      _position = position;
      _orientationAxis = orientationAxis;
      _orientationAngle = orientationAngle;
    }

    public override void Load(ISceneDataNode sceneDataNode)
    {
      var scale = new Vector3();
      var position = new Vector3();
      var orientationAxis = new Vector3();
      var orientationAngle = 0.0f;
      if (sceneDataNode.HasParameter("sx"))
      {
        _hasTransform = true;
        float sx = float.Parse(sceneDataNode.ReadParameter("sx"));
        float sy = float.Parse(sceneDataNode.ReadParameter("sy"));
        float sz = float.Parse(sceneDataNode.ReadParameter("sz"));
        scale = new Vector3(sx, sy, sz);
      } 
      if (sceneDataNode.HasParameter("tx"))
      {
        _hasTransform = true;
        float tx = float.Parse(sceneDataNode.ReadParameter("tx"));
        float ty = float.Parse(sceneDataNode.ReadParameter("ty"));
        float tz = float.Parse(sceneDataNode.ReadParameter("tz"));
        position = new Vector3(tx, ty, tz);
      }
      if (sceneDataNode.HasParameter("rx"))
      {
        _hasTransform = true;
        float rx = float.Parse(sceneDataNode.ReadParameter("rx"));
        float ry = float.Parse(sceneDataNode.ReadParameter("ry"));
        float rz = float.Parse(sceneDataNode.ReadParameter("rz"));
        orientationAxis = new Vector3(rx, ry, rz);
        orientationAngle = float.Parse(sceneDataNode.ReadParameter("angle"));
      }
      Init(scale, position, orientationAxis, orientationAngle);

      var shapeTypeString = sceneDataNode.ReadParameter("shapeType");
      var name = sceneDataNode.ReadParameter("shapeName");
      var filePath = sceneDataNode.ReadParameter("filePath");
      var fileTypeString = Path.GetExtension(filePath).TrimStart('.');

      var shapeType = (ShapeType)Enum.Parse(typeof(ShapeType), shapeTypeString);
      var fileType = (FileType) Enum.Parse(typeof (FileType), fileTypeString);
      _shape = _shapeFactory.CreateShape(shapeType, fileType, name);
      _shape.Load(filePath);

      var materialKey = sceneDataNode.ReadParameter("material");
      _shape.Material = _resourceManager.GetMaterial(materialKey);
    }

    public override void Configure(IRenderer renderer)
    {
      _shape.Configure(renderer);
    }
    
    public override void Render(IRenderer renderer)
    {
      var modelTransform = GetModelTransform();
      _shape.Render(renderer, modelTransform);
    }

    private Matrix4 GetModelTransform()
    {
      if (_hasTransform)
      {
        var translation = Matrix4.CreateTranslation(_position);
        var rotation = Matrix4.Identity;
        if (_orientationAngle != 0)
          rotation = Matrix4.CreateFromAxisAngle(_orientationAxis, _orientationAngle.ToRadians());
        var scale = Matrix4.CreateScale(_scale);
        var matrix = translation * rotation * scale;
        return matrix;
      }
      return ComposeTransform();
    }
  }
}