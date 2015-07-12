using System.Collections.Generic;

namespace Starter3D.API.geometry.factories
{
  public interface IFaceFactory
  {
    IFace CreateFace(List<int> indices);
  }
}