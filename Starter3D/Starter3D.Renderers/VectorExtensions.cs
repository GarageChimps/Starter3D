namespace Starter3D.Renderers
{
  public static class VectorExtensions
  {
    public static SlimDX.Vector3 ToSlimDXVector3(this OpenTK.Vector3 vector)
    {
      return new SlimDX.Vector3(vector.X, vector.Y, vector.Z);
    }

    public static SlimDX.Vector4 ToSlimDXVector4(this OpenTK.Vector3 vector)
    {
      return new SlimDX.Vector4(vector.X, vector.Y, vector.Z, 0);
    }
  }
}
