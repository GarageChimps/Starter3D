using Starter3D.API.renderer;

namespace Starter3D.API.resources
{
  public class Material : IMaterial
  {
    private string _vertexShader;
    private string _fragmentShader;
    
    public string VertexShader
    {
      get { return _vertexShader; }
    }

    public string FragmentShader
    {
      get { return _fragmentShader; }
    }
    
    public Material()
    {
      
    }

    public Material(string vertexShader, string fragmentShader)
    {
      _vertexShader = vertexShader;
      _fragmentShader = fragmentShader;
    }


    public virtual void ConfigureRenderer(IRenderer renderer)
    {
      renderer.SetShaders(_vertexShader, _fragmentShader);
    }

    public virtual void Load(IDataNode dataNode)
    {
      _vertexShader = dataNode.ReadParameter("vertexShader");
      _fragmentShader = dataNode.ReadParameter("fragmentShader");
    }
  }
}