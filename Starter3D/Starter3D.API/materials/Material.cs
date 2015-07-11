namespace ThreeAPI.materials
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
    

  }
}