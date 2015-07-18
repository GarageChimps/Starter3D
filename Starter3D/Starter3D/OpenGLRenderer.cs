using System;
using System.Collections.Generic;
using System.IO;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using Starter3D.API.renderer;

namespace Starter3D
{
  public class OpenGLRenderer : IRenderer
  {
    private const string ShaderBasePath = "Shaders";
    private const string FragmentShaderExtension = "Fragment.glsl";
    private const string VertexShaderExtension = "Vertex.glsl";
    
    private readonly Dictionary<string, int> _shaderHandleDictionary = new Dictionary<string, int>();
    private readonly Dictionary<string, int> _objectsHandleDictionary = new Dictionary<string, int>();


    public void DrawTriangles(string name, int triangleCount)
    {
      if (!_objectsHandleDictionary.ContainsKey(name))
        throw new ApplicationException("Object must be added to the renderer before drawing");
      GL.BindVertexArray(_objectsHandleDictionary[name]);
      GL.DrawElements(BeginMode.Triangles, triangleCount, DrawElementsType.UnsignedInt, IntPtr.Zero);
    }

    
    public void LoadShaders(string shaderName)
    {
      if (_shaderHandleDictionary.ContainsKey(shaderName))
        return;
      var fragmentShaderSource = File.ReadAllText(Path.Combine(ShaderBasePath, shaderName + FragmentShaderExtension));
      var vertexShaderSource = File.ReadAllText(Path.Combine(ShaderBasePath, shaderName + VertexShaderExtension));
      var programHandle = CreateProgram(vertexShaderSource, fragmentShaderSource);
      _shaderHandleDictionary.Add(shaderName, programHandle);
      GL.UseProgram(_shaderHandleDictionary[shaderName]);
    }

    public void UseShader(string shaderName)
    {
      if (!_shaderHandleDictionary.ContainsKey(shaderName))
        throw new ApplicationException("Shader must be loaded before using it");
      GL.UseProgram(_shaderHandleDictionary[shaderName]);
    }

    public void AddObject(string objectName)
    {
      if (_objectsHandleDictionary.ContainsKey(objectName))
        return;
      int objHandle;
      GL.GenVertexArrays(1, out  objHandle);
      _objectsHandleDictionary.Add(objectName, objHandle);
      GL.BindVertexArray(_objectsHandleDictionary[objectName]);
    }

    public void AddMatrixParameter(string name, Matrix4 matrix)
    {
      foreach (var shaderHandle in _shaderHandleDictionary.Values)
      {
        int location = GL.GetUniformLocation(shaderHandle, name);
        if( location != -1)
          GL.UniformMatrix4(location, false, ref matrix);
      }
    }

    public void AddVectorParameter(string name, Vector3 vector)
    {
      foreach (var shaderHandle in _shaderHandleDictionary.Values)
      {
        int location = GL.GetUniformLocation(shaderHandle, name);
        if (location != -1)
          GL.Uniform3(location, vector);
      }
    }

    public void AddNumberParameter(string name, float number)
    {
      foreach (var shaderHandle in _shaderHandleDictionary.Values)
      {
        int location = GL.GetUniformLocation(shaderHandle, name);
        if (location != -1)
          GL.Uniform1(location, number);
      }
    }

    public void SetVerticesData(List<Vector3> data)
    {
      var verticesArray = data.ToArray();
      int vboHandle;
      GL.GenBuffers(1, out vboHandle);
      GL.BindBuffer(BufferTarget.ArrayBuffer, vboHandle);
      GL.BufferData(BufferTarget.ArrayBuffer, new IntPtr(verticesArray.Length * Vector3.SizeInBytes), verticesArray, BufferUsageHint.StaticDraw);
    }
    
    public void SetFacesData(List<int> data)
    {
      var indicesArray = new uint[data.Count];
      for (int i = 0; i < data.Count; i++)
      {
        indicesArray[i] = (uint) data[i];
      }
      int indicesVboHandle;
      GL.GenBuffers(1, out indicesVboHandle);
      GL.BindBuffer(BufferTarget.ElementArrayBuffer, indicesVboHandle);
      GL.BufferData(BufferTarget.ElementArrayBuffer, new IntPtr(indicesArray.Length * sizeof(uint)), indicesArray, BufferUsageHint.StaticDraw);
    }


    public void SetVertexAttribute(int index, string name, int stride, int offset)
    {
      foreach (var shaderHandle in _shaderHandleDictionary.Values)
      {
        int location = GL.GetAttribLocation(shaderHandle, name);
        if (location != -1)
        {
          //GL.BindAttribLocation(_programHandle, index, name);
          GL.EnableVertexAttribArray(location);
          GL.VertexAttribPointer(location, 3, VertexAttribPointerType.Float, false, stride,
            IntPtr.Add(IntPtr.Zero, offset));
        }
      }
    }

    private int CreateShader(string shaderSource, ShaderType type)
    {
      int shaderHandle = GL.CreateShader(type);
      GL.ShaderSource(shaderHandle, shaderSource);
      GL.CompileShader(shaderHandle);
      Console.WriteLine(GL.GetShaderInfoLog(shaderHandle));
      return shaderHandle;
    }

    private int CreateVertexShader(string shaderSource)
    {
      return CreateShader(shaderSource, ShaderType.VertexShader);
    }

    private int CreateFragmentShader(string shaderSource)
    {
      return CreateShader(shaderSource, ShaderType.FragmentShader);
    }

    private int CreateProgram(string vertexShaderSource, string fragmentShaderSource)
    {
      int vsHandle = CreateVertexShader(vertexShaderSource);
      int fsHandle = CreateFragmentShader(fragmentShaderSource);
      return CreateProgram(vsHandle, fsHandle);
    }

    private int CreateProgram(int vertexShaderHandle, int fragmentShaderHandle)
    {

      int shaderProgramHandle = GL.CreateProgram();

      GL.AttachShader(shaderProgramHandle, vertexShaderHandle);
      GL.AttachShader(shaderProgramHandle, fragmentShaderHandle);

      GL.LinkProgram(shaderProgramHandle);

      Console.WriteLine(GL.GetProgramInfoLog(shaderProgramHandle));
      return shaderProgramHandle;
    }

    
  }
}
