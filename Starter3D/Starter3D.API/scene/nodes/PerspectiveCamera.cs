﻿using System.Globalization;
using OpenTK;
using Starter3D.API.scene.persistence;
using Starter3D.API.utils;

namespace Starter3D.API.scene.nodes
{
  public class PerspectiveCamera : CameraNode
  {
    private float _fieldOfView;
    private float _aspectRatio;

    public float FieldOfView
    {
      get { return _fieldOfView; }
    }

    public float AspectRatio
    {
      get { return _aspectRatio; }
    }


    public PerspectiveCamera(float nearClip, float farClip, int order, float fieldOfView, float aspectRatio)
      : base(nearClip, farClip, order)
    {
      Init(fieldOfView, aspectRatio);
    }

    public PerspectiveCamera()
    {

    }

   
    private void Init(float fieldOfView, float aspectRatio)
    {
     _fieldOfView = fieldOfView;
      _aspectRatio = aspectRatio;
    }

    public override void Load(ISceneDataNode sceneDataNode)
    {
      base.Load(sceneDataNode);
      float fov = float.Parse(sceneDataNode.ReadParameter("fieldOfView")).ToRadians();
      float aspectRatio = float.Parse(sceneDataNode.ReadParameter("aspectRatio"));
      Init(fov, aspectRatio);
    }

    public override void Save(ISceneDataNode sceneDataNode)
    {
      base.Save(sceneDataNode);
      sceneDataNode.WriteParameter("fieldOfView", _fieldOfView.ToDegrees().ToString(CultureInfo.InvariantCulture));
      sceneDataNode.WriteParameter("aspectRatio", _aspectRatio.ToString(CultureInfo.InvariantCulture));
    }

    protected override Matrix4 CreateProjectionMatrix()
    {
      var perspectiveMatrix = Matrix4.CreatePerspectiveFieldOfView(_fieldOfView, _aspectRatio, NearClip, FarClip);
      return perspectiveMatrix;
    }
  }
}