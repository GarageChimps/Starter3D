using System.Collections.Generic;
using OpenTK;

namespace Starter3D.API.resources
{
  public interface IMaterial : IResource
  {
    IShader Shader { get; set;  }
    string Name { get; }
    void SetParameter(string name, float value);
    void SetParameter(string name, Vector3 value);
    void SetTexture(string name, ITexture texture);
    IEnumerable<KeyValuePair<string, float>> NumericParameters { get; }
    IEnumerable<KeyValuePair<string, Vector3>> VectorParameters { get; }
    IEnumerable<KeyValuePair<string, ITexture>> Textures { get; }
  }
}
