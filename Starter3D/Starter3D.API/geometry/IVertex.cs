﻿using System.Collections.Generic;
using OpenTK;
using Starter3D.API.renderer;

namespace Starter3D.API.geometry
{
  public interface IVertex
  {
    Vector3 Position { get; }
    Vector3 Normal { get; set; }
    Vector3 TextureCoords { get; }
    bool HasValidNormal();
    void AppendData(List<Vector3> vertexData);
    void Configure(string objectName, string shaderName, IRenderer renderer);
  }
}