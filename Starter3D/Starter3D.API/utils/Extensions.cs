using System;

namespace Starter3D.API.utils
{
  public static class Extensions
  {
    public static float ToRadians(this float angle)
    {
      return (float)(Math.PI * (angle / 180.0f));
    }

    public static float ToDegrees(this float angle)
    {
      return (float)(180.0f * (angle / Math.PI));
    }
  }
}
