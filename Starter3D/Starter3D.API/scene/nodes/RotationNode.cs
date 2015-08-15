using System.Globalization;
using OpenTK;
using Starter3D.API.scene.persistence;
using Starter3D.API.utils;

namespace Starter3D.API.scene.nodes
{
  public class RotationNode : TransformNode
  {
    private Vector3 _axis = new Vector3();
    private float _angle;

    public float X
    {
      get { return _axis.X; }
    }

    public float Y
    {
      get { return _axis.Y; }
    }

    public float Z
    {
      get { return _axis.Z; }
    }

    public float Angle
    {
      get { return _angle; }
    }

    public RotationNode()
    {
      
    }

    public RotationNode(float x, float y, float z, float angle)
    {
      Init(x, y, z, angle);
    }
    
    private void Init(float x, float y, float z, float angle)
    {
      _axis.X = x;
      _axis.Y = y;
      _axis.Z = z;
      _angle = angle;
      _transform = Matrix4.CreateFromAxisAngle(_axis, angle);
    }

    public override void Load(ISceneDataNode sceneDataNode)
    {
      float x = sceneDataNode.ReadFloatParameter("x");
      float y = sceneDataNode.ReadFloatParameter("y");
      float z = sceneDataNode.ReadFloatParameter("z");
      float angle = sceneDataNode.ReadFloatParameter("angle").ToRadians();
      Init(x, y, z, angle);
    }

    public override void Save(ISceneDataNode sceneDataNode)
    {
      sceneDataNode.WriteParameter("x", X.ToString(CultureInfo.InvariantCulture));
      sceneDataNode.WriteParameter("y", Y.ToString(CultureInfo.InvariantCulture));
      sceneDataNode.WriteParameter("z", Z.ToString(CultureInfo.InvariantCulture));
      sceneDataNode.WriteParameter("angle", _angle.ToDegrees().ToString(CultureInfo.InvariantCulture));
    }
  }
}