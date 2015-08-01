using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Linq;

namespace Starter3D.API.utils
{
  public class Configuration : IConfiguration
  {
    private const string ConfigurationPath = "config.xml";
    private readonly Dictionary<string, string> _parameters = new Dictionary<string, string>(); 
    

    public Configuration()
    {
      Load();
    }

    private void Load()
    {
      if (!File.Exists(ConfigurationPath))
        return;

      var xmlDoc = XDocument.Load(ConfigurationPath);
      var xmlConfig = xmlDoc.Elements("Configuration").First();
      foreach (var xAttribute in xmlConfig.Attributes())
      {
        _parameters.Add(xAttribute.Name.ToString(), xAttribute.Value);
      }

    }

    public bool HasParameter(string parameterName)
    {
      return _parameters.ContainsKey(parameterName);
    }

    public string GetParameter(string parameterName)
    {
      return _parameters[parameterName];
    }
  }
}