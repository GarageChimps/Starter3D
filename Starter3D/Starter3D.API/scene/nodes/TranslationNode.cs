using System.Globalization;
using OpenTK;
using Starter3D.API.scene.persistence;

namespace Starter3D.API.scene.nodes
{
  public class TranslationNode : TransformNode
  {
    private Vector3 _translation = new Vector3();

    public float X
    {
      get { return _translation.X; }
    }

    public float Y
    {
      get { return _translation.Y; }
    }

    public float Z
    {
      get { return _translation.Z; }
    }

    public Vector3 Translation
    {
      get { return _translation; }
    }

    public TranslationNode(float x, float y, float z)
    {
      Init(x, y, z);
    }

    private void Init(float x, float y, float z)
    {
      _translation.X = x;
      _translation.Y = y;
      _translation.Z = z;
      _transform = Matrix4.CreateTranslation(_translation);
    }

    public TranslationNode()
    {
      
    }

    public override void Load(ISceneDataNode sceneDataNode)
    {
      float x = sceneDataNode.ReadFloatParameter("x");
      float y = sceneDataNode.ReadFloatParameter("y");
      float z = sceneDataNode.ReadFloatParameter("z");
      Init(x,y,z);
    }

    public override void Save(ISceneDataNode sceneDataNode)
    {
      sceneDataNode.WriteParameter("x", X.ToString(CultureInfo.InvariantCulture));
      sceneDataNode.WriteParameter("y", Y.ToString(CultureInfo.InvariantCulture));
      sceneDataNode.WriteParameter("z", Z.ToString(CultureInfo.InvariantCulture));
    }
  }
}