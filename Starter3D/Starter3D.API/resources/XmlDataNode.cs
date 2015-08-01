using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
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

    public void ReadAllParameters(Dictionary<string, Vector3> vectorParameters, Dictionary<string, float> numericParameters, 
      Dictionary<string, string> textureParameters)
    {
      foreach (var attribute in _element.Attributes())
      {
        var name = attribute.Name.ToString();
        var value = attribute.Value;
        var splitParamters = value.Split(',');
        float floatValue;
        if (splitParamters.Length == 3)
        {
          float x, y, z;
          if (!float.TryParse(splitParamters[0], out x))
            continue;
          if (!float.TryParse(splitParamters[1], out y))
            continue;
          if (!float.TryParse(splitParamters[2], out z))
            continue;
          vectorParameters.Add(name, new Vector3(x, y, z));

        }
        else if (float.TryParse(value, out floatValue))
        {
          numericParameters.Add(name, floatValue);
        }
        else
        {
          textureParameters.Add(name, value);
        }
      }
    }

    public IEnumerable<string> ReadParameterList(string parameterName)
    {
      var parameter = ReadParameter(parameterName);
      var splitParameters = parameter.Split(',').ToList();
      splitParameters.ForEach(s => s.Trim());
      return splitParameters;
    }
  }
}