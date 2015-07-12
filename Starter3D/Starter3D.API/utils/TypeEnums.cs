namespace Starter3D.API.utils
{
  public enum SceneNodeType
  {
    Scene = 0,
    Translate,
    Rotate,
    Scale,
    Shape,
    PerspectiveCamera,
    OrthographicCamera,
    PointLight,
    DirectionalLight
  }

  public enum ShapeType
  {
    Mesh = 0,
    PointCloud,
    Curve
  }

  public enum FileType
  {
    obj = 0,

  }
}