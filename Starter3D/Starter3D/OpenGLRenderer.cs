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
    private const string ShaderExtension = ".glsl";

    private readonly Dictionary<string, int> _shaderHandleDictionary = new Dictionary<string, int>();
    private readonly Dictionary<string, int> _objectsHandleDictionary = new Dictionary<string, int>();
    private readonly Dictionary<string, Tuple<int, int>> _textureHandleDictionary = new Dictionary<string, Tuple<int, int>>();
    #endregion

    #region Public Methods
    public void LoadObject(string objectName)
    {
      if (_objectsHandleDictionary.ContainsKey(objectName))
        return;
      int objHandle;
      GL.GenVertexArrays(1, out  objHandle);
      _objectsHandleDictionary.Add(objectName, objHandle);
      GL.BindVertexArray(_objectsHandleDictionary[objectName]);
    }
    
    public void DrawTriangles(string name, int triangleCount)
    {
      if (!_objectsHandleDictionary.ContainsKey(name))
        throw new ApplicationException("Object must be added to the renderer before drawing");
      GL.BindVertexArray(_objectsHandleDictionary[name]);
      GL.DrawElements(BeginMode.Triangles, triangleCount, DrawElementsType.UnsignedInt, IntPtr.Zero);
    }

    public void SetVerticesData(string name, List<Vector3> data)
    {
      var verticesArray = data.ToArray();
      int vboHandle;
      GL.GenBuffers(1, out vboHandle);
      GL.BindBuffer(BufferTarget.ArrayBuffer, vboHandle);
      GL.BufferData(BufferTarget.ArrayBuffer, new IntPtr(verticesArray.Length * Vector3.SizeInBytes), verticesArray, BufferUsageHint.StaticDraw);
    }

    public void SetFacesData(string name, List<int> data)
    {
      var indicesArray = new uint[data.Count];
      for (int i = 0; i < data.Count; i++)
      {
        indicesArray[i] = (uint)data[i];
      }
      int indicesVboHandle;
      GL.GenBuffers(1, out indicesVboHandle);
      GL.BindBuffer(BufferTarget.ElementArrayBuffer, indicesVboHandle);
      GL.BufferData(BufferTarget.ElementArrayBuffer, new IntPtr(indicesArray.Length * sizeof(uint)), indicesArray,
        BufferUsageHint.StaticDraw);
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

    public void LoadTexture(string name, string shader, int index, Bitmap texture)
    {
      if (_textureHandleDictionary.ContainsKey(name + shader))
        return;
      var unit = TextureUnit.Texture0 + index;
      var textureHandle = CreateTexture(texture, unit, TextureMinFilter.Linear,
        TextureMagFilter.Linear);
      _textureHandleDictionary.Add(name + shader, new Tuple<int, int>(textureHandle, index));
      SetNumberParameter(name + shader, index);
    }

    public void UseTexture(string textureName, string shader)
    {
      if (!_textureHandleDictionary.ContainsKey(textureName + shader))
        throw new ApplicationException("Texture has to be added before using");
      GL.UseProgram(_shaderHandleDictionary[shader]);
      var textureInfo = _textureHandleDictionary[textureName + shader];
      GL.ActiveTexture(TextureUnit.Texture0 + textureInfo.Item2);
      GL.BindTexture(TextureTarget.Texture2D, textureInfo.Item1);
      int location = GL.GetUniformLocation(_shaderHandleDictionary[shader], textureName);
      if (location != -1)
      {
        int textureUnitIndex = textureInfo.Item2;
        GL.Uniform1(location, textureUnitIndex);
      }
    }

    public void LoadShaders(string shaderName, string vertexShader, string fragmentShader)
    {
      if (_shaderHandleDictionary.ContainsKey(shaderName))
        return;
      var fragmentShaderSource = File.ReadAllText(Path.Combine(ShaderBasePath, fragmentShader + ShaderExtension));
      var vertexShaderSource = File.ReadAllText(Path.Combine(ShaderBasePath, vertexShader + ShaderExtension));
      var programHandle = CreateProgram(vertexShaderSource, fragmentShaderSource);
      _shaderHandleDictionary.Add(shaderName, programHandle);
    }

    public void UseShader(string shaderName)
    {
      if (!_shaderHandleDictionary.ContainsKey(shaderName))
        throw new ApplicationException("Shader must be loaded before using it");
      GL.UseProgram(_shaderHandleDictionary[shaderName]);
    }
    
    public void SetMatrixParameter(string name, Matrix4 matrix)
    {
      foreach (var shaderHandle in _shaderHandleDictionary.Values)
      {
        GL.UseProgram(shaderHandle);
        int location = GL.GetUniformLocation(shaderHandle, name);
        if (location != -1)
          GL.UniformMatrix4(location, false, ref matrix);
      }
    }

    public void SetMatrixParameter(string name, Matrix4 matrix, string shader)
    {
      GL.UseProgram(_shaderHandleDictionary[shader]);
      int location = GL.GetUniformLocation(_shaderHandleDictionary[shader], name);
      if (location != -1)
        GL.UniformMatrix4(location, false, ref matrix);
    }

    public void SetVectorParameter(string name, Vector3 vector)
    {
      foreach (var shaderHandle in _shaderHandleDictionary.Values)
      {
        GL.UseProgram(shaderHandle);
        int location = GL.GetUniformLocation(shaderHandle, name);
        if (location != -1)
          GL.Uniform3(location, vector);
      }
    }

    public void SetVectorParameter(string name, Vector3 vector, string shader)
    {
      GL.UseProgram(_shaderHandleDictionary[shader]);
      int location = GL.GetUniformLocation(_shaderHandleDictionary[shader], name);
      if (location != -1)
        GL.Uniform3(location, vector);
    }

    public void SetBooleanParameter(string name, bool value)
    {
      foreach (var shaderHandle in _shaderHandleDictionary.Values)
      {
        GL.UseProgram(shaderHandle);
        int location = GL.GetUniformLocation(shaderHandle, name);
        if (location != -1)
          GL.Uniform1(location, value ? 1 : 0);
      }
    }

    public void SetBooleanParameter(string name, bool value, string shader)
    {
      GL.UseProgram(_shaderHandleDictionary[shader]);
      int location = GL.GetUniformLocation(_shaderHandleDictionary[shader], name);
      if (location != -1)
        GL.Uniform1(location, value ? 1 : 0);
    }

    public void SetNumberParameter(string name, float number)
    {
      foreach (var shaderHandle in _shaderHandleDictionary.Values)
      {
        GL.UseProgram(shaderHandle);
        int location = GL.GetUniformLocation(shaderHandle, name);
        if (location != -1)
          GL.Uniform1(location, number);
      }
    }

    public void SetNumberParameter(string name, float number, string shader)
    {
      GL.UseProgram(_shaderHandleDictionary[shader]);
      int location = GL.GetUniformLocation(_shaderHandleDictionary[shader], name);
      if (location != -1)
        GL.Uniform1(location, number);
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
