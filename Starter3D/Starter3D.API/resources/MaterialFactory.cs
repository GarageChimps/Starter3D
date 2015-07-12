﻿namespace Starter3D.API.resources
{
  public class MaterialFactory : IMaterialFactory
  {
    public IMaterial CreateMaterial(string vertexShader, string fragmentShader)
    {
      return new Material(vertexShader, fragmentShader);
    }
  }
}