namespace Starter3D.API.utils
{
  public enum ControllerMouseButton
  {
    Left = 0,
    Middle,
    Right
  }

  public enum CullMode
  {
    None = 0,
    Back,
    Front
  }

  public enum MaterialType
  {
    BaseMaterial = 0,
  }

  public enum ShaderResourceType
  {
    BaseShader = 0,
  }

  public enum TextureType
  {
    BaseTexture = 0,
  }

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
    DirectionalLight,
    AmbientLight
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
    dxf

  }
}