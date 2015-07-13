using System.Globalization;
using System.Xml.Linq;
using OpenTK;

namespace Starter3D.API.resources
{
  public class XmlDataNode : IDataNode
  {
    private readonly XElement _element;

    public XmlDataNode(XElement element)
    {
      _element = element;
    }

    public void WriteParameter(string parameterName, string parameterValue)
    {
      throw new System.NotImplementedException();
    }

    public bool HasParameter(string parameterName)
    {
      return _element.Attribute(parameterName) != null;
    }

    public string ReadParameter(string parameterName)
    {
      return _element.Attribute(parameterName).Value;
    }

    public float ReadFloatParameter(string parameterName)
    {
      var parameter = ReadParameter(parameterName);
      return float.Parse(parameter, CultureInfo.InvariantCulture);
    }

    public Vector3 ReadVectorParameter(string parameterName)
    {
      var parameter = ReadParameter(parameterName);
      var splitParamters = parameter.Split(',');
      return new Vector3(float.Parse(splitParamters[0], CultureInfo.InvariantCulture),
        float.Parse(splitParamters[1], CultureInfo.InvariantCulture), float.Parse(splitParamters[2], CultureInfo.InvariantCulture));
    }
  }
}