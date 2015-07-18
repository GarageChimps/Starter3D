using OpenTK;
using Starter3D.API.renderer;

namespace Starter3D.API.resources
{
  public class BlinnPhongMaterial : Material
  {
    private Vector3 _ambientLight;
    private Vector3 _diffuseColor;
    private Vector3 _specularColor;
    private float _shininess;

    public BlinnPhongMaterial(string shaderName, Vector3 ambientLight, Vector3 diffuseColor, Vector3 specularColor, float shininess)
      : base(shaderName)
    {
      _ambientLight = ambientLight;
      _diffuseColor = diffuseColor;
      _specularColor = specularColor;
      _shininess = shininess;
    }

    public BlinnPhongMaterial()
    {

    }

    public override void Configure(IRenderer renderer)
    {
      base.Configure(renderer);
      renderer.AddVectorParameter("ambientLight", _ambientLight);
      renderer.AddVectorParameter("diffuseColor", _diffuseColor);
      renderer.AddVectorParameter("specularColor", _specularColor);
      renderer.AddNumberParameter("shininess", _shininess);
    }

    public override void Load(IDataNode dataNode)
    {
      base.Load(dataNode);
      _ambientLight = dataNode.ReadVectorParameter("ambientLight");
      _diffuseColor = dataNode.ReadVectorParameter("diffuseColor");
      _specularColor = dataNode.ReadVectorParameter("specularColor");
      _shininess = dataNode.ReadFloatParameter("shininess");
    }
  }
}