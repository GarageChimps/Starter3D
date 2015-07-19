using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using Starter3D.API.renderer;
using PixelFormat = OpenTK.Graphics.OpenGL.PixelFormat;

namespace Starter3D.OpenGL
{
  public class OpenGLRenderer : IRenderer
  {
    #region Fields
    private const string ShaderBasePath = "Shaders";
    private const string FragmentShaderExtension = "Fragment.glsl";
    private const string VertexShaderExtension = "Vertex.glsl";

    private readonly Dictionary<string, int> _shaderHandleDictionary = new Dictionary<string, int>();
    private readonly Dictionary<string, int> _objectsHandleDictionary = new Dictionary<string, int>();
    private readonly Dictionary<string, int> _objectsVertexBufferDictionary = new Dictionary<string, int>();
    private readonly Dictionary<string, int> _objectsIndexBufferDictionary = new Dictionary<string, int>();
    private readonly Dictionary<string, Tuple<int, TextureUnit>> _textureHandleDictionary = new Dictionary<string, Tuple<int, TextureUnit>>(); 
    #endregion

    #region Public Methods
    public void DrawTriangles(string name, int triangleCount)
    {
      if (!_objectsHandleDictionary.ContainsKey(name))
        throw new ApplicationException("Object must be added to the renderer before drawing");
      if (!_objectsVertexBufferDictionary.ContainsKey(name))
        throw new ApplicationException("Vertices of object must be added to the renderer before drawing");
      if (!_objectsIndexBufferDictionary.ContainsKey(name))
        throw new ApplicationException("Faces of object must be added to the renderer before drawing");
      GL.BindVertexArray(_objectsHandleDictionary[name]);
      GL.DrawElements(BeginMode.Triangles, triangleCount, DrawElementsType.UnsignedInt, IntPtr.Zero);
    }

    public void AddTexture(string name, int index, Bitmap texture)
    {
      if (_textureHandleDictionary.ContainsKey(name))
        return;
      var unit = TextureUnit.Texture0 + index;
      var textureHandle = CreateTexture(texture, TextureUnit.Texture0 + index, TextureMinFilter.Linear,
        TextureMagFilter.Linear);
      _textureHandleDictionary.Add(name, new Tuple<int, TextureUnit>(textureHandle, unit));
      AddNumberParameter(name, index);
    }

    public void UseTexture(string textureName)
    {
      if (!_textureHandleDictionary.ContainsKey(textureName))
        throw new ApplicationException("Texture has to be added before using");
      var textureInfo = _textureHandleDictionary[textureName];
      GL.ActiveTexture(textureInfo.Item2);
      GL.BindTexture(TextureTarget.Texture2D, textureInfo.Item1);
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
        if (location != -1)
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

    public void AddBooleanParameter(string name, bool value)
    {
      foreach (var shaderHandle in _shaderHandleDictionary.Values)
      {
        int location = GL.GetUniformLocation(shaderHandle, name);
        if (location != -1)
          GL.Uniform1(location, value ? 1 : 0);
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

    public void SetVerticesData(string name, List<Vector3> data)
    {
      if (_objectsVertexBufferDictionary.ContainsKey(name))
        return;
      var verticesArray = data.ToArray();
      int vboHandle;
      GL.GenBuffers(1, out vboHandle);
      GL.BindBuffer(BufferTarget.ArrayBuffer, vboHandle);
      GL.BufferData(BufferTarget.ArrayBuffer, new IntPtr(verticesArray.Length * Vector3.SizeInBytes), verticesArray, BufferUsageHint.StaticDraw);
      _objectsVertexBufferDictionary.Add(name, vboHandle);
    }

    public void SetFacesData(string name, List<int> data)
    {
      if (_objectsIndexBufferDictionary.ContainsKey(name))
        return;
      var indicesArray = new uint[data.Count];
      for (int i = 0; i < data.Count; i++)
      {
        indicesArray[i] = (uint)data[i];
      }
      int indicesVboHandle;
      GL.GenBuffers(1, out indicesVboHandle);
      GL.BindBuffer(BufferTarget.ElementArrayBuffer, indicesVboHandle);
      GL.BufferData(BufferTarget.ElementArrayBuffer, new IntPtr(indicesArray.Length * sizeof(uint)), indicesArray, BufferUsageHint.StaticDraw);
      _objectsIndexBufferDictionary.Add(name, indicesVboHandle);
    }
    
    public void SetVertexAttribute(string objectName, string shaderName, int index, string vertexPropertyName, int stride, int offset)
    {
      int location = GL.GetAttribLocation(_shaderHandleDictionary[shaderName], vertexPropertyName);
      if (location != -1)
      {
        GL.EnableVertexAttribArray(index);
        GL.VertexAttribPointer(index, 3, VertexAttribPointerType.Float, false, stride, IntPtr.Add(IntPtr.Zero, offset));
      }
    }

    #endregion

    #region Private Methods
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

    private int CreateTexture(Bitmap texture, TextureUnit unit, TextureMinFilter minFilter, TextureMagFilter magFilter)
    {
      int textureId = GL.GenTexture();
      GL.ActiveTexture(unit);
      GL.BindTexture(TextureTarget.Texture2D, textureId);
      GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)minFilter);
      GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)magFilter);
      GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapS, (int)TextureWrapMode.Repeat);
      GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapT, (int)TextureWrapMode.Repeat);
      GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba, texture.Width, texture.Height, 0,
        PixelFormat.Bgra, PixelType.UnsignedByte, IntPtr.Zero);

      var bmpData = texture.LockBits(new Rectangle(0, 0, texture.Width, texture.Height),
        ImageLockMode.ReadOnly, System.Drawing.Imaging.PixelFormat.Format32bppArgb);

      GL.TexSubImage2D(TextureTarget.Texture2D, 0, 0, 0, texture.Width, texture.Height, PixelFormat.Bgra,
        PixelType.UnsignedByte, bmpData.Scan0);

      texture.UnlockBits(bmpData);

      GL.BindTexture(TextureTarget.Texture2D, 0);

      return textureId;
    }
    #endregion

  }
}
