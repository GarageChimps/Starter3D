namespace ThreeAPI.scene
{
  public enum SceneNodeType
  {
    Scene = 0,
    Translate,
    Rotate,
    Scale,
    Shape,
    PerspectiveCamera,
    OrthographicCamera
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