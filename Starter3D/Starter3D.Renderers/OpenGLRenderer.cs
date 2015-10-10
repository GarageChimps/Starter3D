using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using OpenTK;
using OpenTK.Graphics;
using Starter3D.API.renderer;
using Starter3D.API.utils;
using BeginMode = OpenTK.Graphics.OpenGL.BeginMode;
using BufferAccess = OpenTK.Graphics.OpenGL.BufferAccess;
using BufferTarget = OpenTK.Graphics.OpenGL.BufferTarget;
using BufferUsageHint = OpenTK.Graphics.OpenGL.BufferUsageHint;
using CullFaceMode = OpenTK.Graphics.OpenGL.CullFaceMode;
using DrawElementsType = OpenTK.Graphics.OpenGL.DrawElementsType;
using EnableCap = OpenTK.Graphics.OpenGL.EnableCap;
using GL = OpenTK.Graphics.OpenGL.GL;
using MaterialFace = OpenTK.Graphics.OpenGL.MaterialFace;
using PixelFormat = OpenTK.Graphics.OpenGL.PixelFormat;
using PixelInternalFormat = OpenTK.Graphics.OpenGL.PixelInternalFormat;
using PixelType = OpenTK.Graphics.OpenGL.PixelType;
using ShaderType = OpenTK.Graphics.OpenGL.ShaderType;
using TextureMagFilter = OpenTK.Graphics.OpenGL.TextureMagFilter;
using TextureMinFilter = OpenTK.Graphics.OpenGL.TextureMinFilter;
using TextureParameterName = OpenTK.Graphics.OpenGL.TextureParameterName;
using TextureTarget = OpenTK.Graphics.OpenGL.TextureTarget;
using TextureUnit = OpenTK.Graphics.OpenGL.TextureUnit;
using TextureWrapMode = OpenTK.Graphics.OpenGL.TextureWrapMode;
using VertexAttribPointerType = OpenTK.Graphics.OpenGL.VertexAttribPointerType;

namespace Starter3D.Renderers
{
  public class OpenGLRenderer : IRenderer
  {
    #region Structs

    struct TextureInfo
    {
      public int Handle { get; set; }
      public int Unit { get; set; }
    }

    class RenderObject
    {
      public int ObjectHandle { get; set; }
      public int VertexBufferHandle { get; set; }
      public int IndexBufferHandle { get; set; }
      public int InstanceBufferHandle { get; set; }
      public int VertexBufferSize { get; set; }
      public int IndexBufferSize { get; set; }
      public int InstanceBufferSize { get; set; }
    }
    #endregion

    #region Fields
    private const string ShaderBasePath = @"shaders/opengl";
    private const string ShaderExtension = ".glsl";

    private readonly Dictionary<string, int> _shaderHandleDictionary = new Dictionary<string, int>();
    private readonly Dictionary<string, RenderObject> _objectsHandleDictionary = new Dictionary<string, RenderObject>();
    private readonly Dictionary<string, TextureInfo> _textureHandleDictionary = new Dictionary<string, TextureInfo>();

    #endregion

    #region Public Methods
    public void LoadObject(string objectName)
    {
      if (_objectsHandleDictionary.ContainsKey(objectName))
        return;
      int objHandle;
      GL.GenVertexArrays(1, out  objHandle);
      _objectsHandleDictionary.Add(objectName, new RenderObject() { ObjectHandle = objHandle });
      GL.BindVertexArray(_objectsHandleDictionary[objectName].ObjectHandle);
    }

    public void DrawTriangles(string name, int triangleCount)
    {
      if (!_objectsHandleDictionary.ContainsKey(name))
        throw new ApplicationException("Object must be added to the renderer before drawing");
      GL.BindVertexArray(_objectsHandleDictionary[name].ObjectHandle);
      GL.DrawElements(BeginMode.Triangles, triangleCount, DrawElementsType.UnsignedInt, IntPtr.Zero);
    }

    public void DrawMeshCollection(string objectName, int triangleCount, int instanceCount)
    {
      if (!_objectsHandleDictionary.ContainsKey(objectName))
        throw new ApplicationException("Object must be added to the renderer before drawing");
      GL.BindVertexArray(_objectsHandleDictionary[objectName].ObjectHandle);
      GL.DrawElementsInstanced(BeginMode.Triangles, triangleCount, DrawElementsType.UnsignedInt, IntPtr.Zero, instanceCount);
    }

    public void DrawLines(string name, int lineCount, float lineWidth)
    {
      if (!_objectsHandleDictionary.ContainsKey(name))
        throw new ApplicationException("Object must be added to the renderer before drawing");
      GL.BindVertexArray(_objectsHandleDictionary[name].ObjectHandle);
      GL.LineWidth(lineWidth);
      GL.DrawElements(BeginMode.LineStrip, lineCount, DrawElementsType.UnsignedInt, IntPtr.Zero);
    }

    public void DrawPoints(string name, int pointCount, float pointSize)
    {
      if (!_objectsHandleDictionary.ContainsKey(name))
        throw new ApplicationException("Object must be added to the renderer before drawing");
      GL.BindVertexArray(_objectsHandleDictionary[name].ObjectHandle);
      GL.PointSize(pointSize);
      GL.DrawElements(BeginMode.Points, pointCount, DrawElementsType.UnsignedInt, IntPtr.Zero);
    }

    public void SetVerticesData(string name, List<Vector3> data, bool isDynamic = false)
    {
      GL.BindVertexArray(_objectsHandleDictionary[name].ObjectHandle);
      int vboHandle;
      GL.GenBuffers(1, out vboHandle);
      _objectsHandleDictionary[name].VertexBufferHandle = vboHandle;
      CreateVerticesData(name, data, isDynamic, data.Count);
    }

    private void CreateVerticesData(string name, List<Vector3> data, bool isDynamic, int bufferSize)
    {
      GL.BindVertexArray(_objectsHandleDictionary[name].ObjectHandle);
      var verticesArray = data.ToArray();
      GL.BindBuffer(BufferTarget.ArrayBuffer, _objectsHandleDictionary[name].VertexBufferHandle);
      GL.BufferData(BufferTarget.ArrayBuffer, new IntPtr(bufferSize * Vector3.SizeInBytes), verticesArray,
        isDynamic ? BufferUsageHint.DynamicDraw : BufferUsageHint.StaticDraw);
      _objectsHandleDictionary[name].VertexBufferSize = bufferSize;

    }

    public void UpdateVerticesData(string name, List<Vector3> data)
    {
      if (data.Count > _objectsHandleDictionary[name].VertexBufferSize)
      {
        CreateVerticesData(name, data, true, _objectsHandleDictionary[name].VertexBufferSize * 2);
      }
      else
      {
        GL.BindVertexArray(_objectsHandleDictionary[name].ObjectHandle);
        var vertexValuesArray = new float[data.Count * 3];
        var index = 0;
        foreach (var vertex in data)
        {
          vertexValuesArray[index++] = vertex.X;
          vertexValuesArray[index++] = vertex.Y;
          vertexValuesArray[index++] = vertex.Z;
        }
        var memoryPointer = GL.MapBuffer(BufferTarget.ArrayBuffer, BufferAccess.WriteOnly);
        unsafe
        {
          fixed (float* systemMemory = &vertexValuesArray[0])
          {
            float* videoMemory = (float*)memoryPointer.ToPointer();
            for (int i = 0; i < vertexValuesArray.Length; i++)
              videoMemory[i] = systemMemory[i]; // simulate what GL.BufferData would do
          }
        }
        GL.UnmapBuffer(BufferTarget.ArrayBuffer);
      }
    }

    public void SetIndexData(string name, List<int> indices, bool isDynamic = false)
    {
      GL.BindVertexArray(_objectsHandleDictionary[name].ObjectHandle);
      int indicesVboHandle;
      GL.GenBuffers(1, out indicesVboHandle);
      _objectsHandleDictionary[name].IndexBufferHandle = indicesVboHandle;
      CreateIndexData(name, indices, isDynamic, indices.Count);
    }

    private void CreateIndexData(string name, List<int> indices, bool isDynamic, int bufferSize)
    {
      GL.BindVertexArray(_objectsHandleDictionary[name].ObjectHandle);
      var indicesArray = new uint[indices.Count];
      for (int i = 0; i < indices.Count; i++)
      {
        indicesArray[i] = (uint)indices[i];
      }
      
      GL.BindBuffer(BufferTarget.ElementArrayBuffer, _objectsHandleDictionary[name].IndexBufferHandle);
      GL.BufferData(BufferTarget.ElementArrayBuffer, new IntPtr(bufferSize * sizeof(uint)), indicesArray,
        isDynamic ? BufferUsageHint.DynamicDraw : BufferUsageHint.StaticDraw);
      _objectsHandleDictionary[name].IndexBufferSize = bufferSize;
      
    }

    public void UpdateIndexData(string name, List<int> indices)
    {
      if (indices.Count > _objectsHandleDictionary[name].IndexBufferSize)
      {
        CreateIndexData(name, indices, true, _objectsHandleDictionary[name].IndexBufferSize * 2);
      }
      else
      {
        GL.BindVertexArray(_objectsHandleDictionary[name].ObjectHandle);
        var indicesArray = new uint[indices.Count];
        for (int i = 0; i < indices.Count; i++)
        {
          indicesArray[i] = (uint)indices[i];
        }
        var memoryPointer = GL.MapBuffer(BufferTarget.ElementArrayBuffer, BufferAccess.WriteOnly);
        unsafe
        {
          fixed (uint* systemMemory = &indicesArray[0])
          {
            uint* videoMemory = (uint*)memoryPointer.ToPointer();
            for (int i = 0; i < indicesArray.Length; i++)
              videoMemory[i] = systemMemory[i]; // simulate what GL.BufferData would do
          }
        }
        GL.UnmapBuffer(BufferTarget.ElementArrayBuffer);
      }
    }



    public void SetVertexAttribute(string objectName, string shaderName, int index, string vertexPropertyName, int stride, int offset)
    {
      GL.BindVertexArray(_objectsHandleDictionary[objectName].ObjectHandle);
      int location = GL.GetAttribLocation(_shaderHandleDictionary[shaderName], vertexPropertyName);
      if (location != -1)
      {
        GL.EnableVertexAttribArray(location);
        GL.VertexAttribPointer(location, 3, VertexAttribPointerType.Float, false, stride, IntPtr.Add(IntPtr.Zero, offset));
        GL.VertexAttribDivisor(location, 0);
      }
    }

    public void SetInstanceAttribute(string objectName, string shaderName, int index, string instancePropertyName, int stride, int offset)
    {
      GL.BindVertexArray(_objectsHandleDictionary[objectName].ObjectHandle);
      int location = GL.GetAttribLocation(_shaderHandleDictionary[shaderName], instancePropertyName);
      if (location != -1)
      {
        for (int i = 0; i < 4; i++)
        {
          GL.EnableVertexAttribArray(location + i);
          GL.VertexAttribPointer(location + i, 4, VertexAttribPointerType.Float, false, stride, IntPtr.Add(IntPtr.Zero, i * offset));
          GL.VertexAttribDivisor(location + i, 1);
        }
      }
    }

    public void SetInstanceData(string name, List<Matrix4> instanceData, bool isDynamic = false)
    {
      GL.BindVertexArray(_objectsHandleDictionary[name].ObjectHandle);
      int instanceHandle;
      GL.GenBuffers(1, out instanceHandle);
      _objectsHandleDictionary[name].InstanceBufferHandle = instanceHandle;
      CreateInstanceData(name, instanceData, isDynamic, instanceData.Count);
      
    }

    private void CreateInstanceData(string name, List<Matrix4> instanceData, bool isDynamic, int bufferSize)
    {
      GL.BindVertexArray(_objectsHandleDictionary[name].ObjectHandle);
      var instancesArray = instanceData.ToArray();
      GL.BindBuffer(BufferTarget.ArrayBuffer, _objectsHandleDictionary[name].InstanceBufferHandle);
      GL.BufferData(BufferTarget.ArrayBuffer, new IntPtr(bufferSize * 4 * Vector4.SizeInBytes), instancesArray,
        isDynamic ? BufferUsageHint.DynamicDraw : BufferUsageHint.StaticDraw);
      _objectsHandleDictionary[name].InstanceBufferSize = bufferSize;
      
    }

    public void UpdateInstanceData(string name, List<Matrix4> instanceData)
    {
      if (instanceData.Count > _objectsHandleDictionary[name].InstanceBufferSize)
      {
        CreateInstanceData(name, instanceData, true, _objectsHandleDictionary[name].InstanceBufferSize * 2);
      }
      else
      {
        GL.BindVertexArray(_objectsHandleDictionary[name].ObjectHandle);
        var instancesValuesArray = new float[instanceData.Count * 16];
        var index = 0;
        foreach (var instance in instanceData)
        {
          instancesValuesArray[index++] = instance.Column0.X;
          instancesValuesArray[index++] = instance.Column0.Y;
          instancesValuesArray[index++] = instance.Column0.Z;
          instancesValuesArray[index++] = instance.Column0.W;

          instancesValuesArray[index++] = instance.Column1.X;
          instancesValuesArray[index++] = instance.Column1.Y;
          instancesValuesArray[index++] = instance.Column1.Z;
          instancesValuesArray[index++] = instance.Column1.W;

          instancesValuesArray[index++] = instance.Column2.X;
          instancesValuesArray[index++] = instance.Column2.Y;
          instancesValuesArray[index++] = instance.Column2.Z;
          instancesValuesArray[index++] = instance.Column2.W;

          instancesValuesArray[index++] = instance.Column3.X;
          instancesValuesArray[index++] = instance.Column3.Y;
          instancesValuesArray[index++] = instance.Column3.Z;
          instancesValuesArray[index++] = instance.Column3.W;
        }
        var memoryPointer = GL.MapBuffer(BufferTarget.ArrayBuffer, BufferAccess.WriteOnly);
        unsafe
        {
          fixed (float* systemMemory = &instancesValuesArray[0])
          {
            float* videoMemory = (float*)memoryPointer.ToPointer();
            for (int i = 0; i < instancesValuesArray.Length; i++)
              videoMemory[i] = systemMemory[i]; // simulate what GL.BufferData would do
          }
        }
        GL.UnmapBuffer(BufferTarget.ArrayBuffer);
      }
    }

    public void LoadTexture(string textureName, int index, Bitmap texture, TextureMinFilter minFilter, TextureMagFilter magFilter)
    {
      if (_textureHandleDictionary.ContainsKey(textureName))
        return;
      var unit = TextureUnit.Texture0 + index;
      var textureHandle = CreateTexture(texture, unit, minFilter, magFilter);
      _textureHandleDictionary.Add(textureName, new TextureInfo { Handle = textureHandle, Unit = index });
    }

    public void UseTexture(string textureName, string shader, string uniformName)
    {
      if (!_textureHandleDictionary.ContainsKey(textureName))
        throw new ApplicationException("Texture has to be added before using");
      GL.UseProgram(_shaderHandleDictionary[shader]);
      var textureInfo = _textureHandleDictionary[textureName];
      GL.ActiveTexture(TextureUnit.Texture0 + textureInfo.Unit);
      GL.BindTexture(TextureTarget.Texture2D, textureInfo.Handle);
      int location = GL.GetUniformLocation(_shaderHandleDictionary[shader], uniformName);
      if (location != -1)
      {
        int textureUnitIndex = textureInfo.Unit;
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

    public void SetVectorArrayParameter(string name, int index, Vector3 vector)
    {
      foreach (var shaderHandle in _shaderHandleDictionary.Values)
      {
        GL.UseProgram(shaderHandle);
        int location = GL.GetUniformLocation(shaderHandle, name + "[" + index + "]");
        if (location != -1)
          GL.Uniform3(location, vector);
      }
    }

    public void SetVectorArrayParameter(string name, int index, Vector3 vector, string shader)
    {
      GL.UseProgram(_shaderHandleDictionary[shader]);
      int location = GL.GetUniformLocation(_shaderHandleDictionary[shader], name + "[" + index + "]");
      if (location != -1)
        GL.Uniform3(location, vector);
    }

    public void SetNumericParameter(string name, float number)
    {
      foreach (var shaderHandle in _shaderHandleDictionary.Values)
      {
        GL.UseProgram(shaderHandle);
        int location = GL.GetUniformLocation(shaderHandle, name);
        if (location != -1)
          GL.Uniform1(location, number);
      }
    }

    public void SetNumericParameter(string name, float number, string shader)
    {
      GL.UseProgram(_shaderHandleDictionary[shader]);
      int location = GL.GetUniformLocation(_shaderHandleDictionary[shader], name);
      if (location != -1)
        GL.Uniform1(location, number);
    }

    public void SetBackgroundColor(float r, float g, float b)
    {
      GL.ClearColor(new Color4(r, g, b, 1));
    }

    public void EnableZBuffer(bool enable)
    {
      if (enable)
        GL.Enable(EnableCap.DepthTest);
      else
        GL.Disable(EnableCap.DepthTest);
    }

    public void EnableWireframe(bool enable)
    {
      var polygonMode = enable ? OpenTK.Graphics.OpenGL.PolygonMode.Line : OpenTK.Graphics.OpenGL.PolygonMode.Fill;
      GL.PolygonMode(MaterialFace.FrontAndBack, polygonMode);
    }

    public void SetCullMode(CullMode cullMode)
    {
      switch (cullMode)
      {
        case CullMode.None:
          GL.Disable(EnableCap.CullFace);
          break;
        case CullMode.Back:
          GL.Enable(EnableCap.CullFace);
          GL.CullFace(CullFaceMode.Back);
          break;
        case CullMode.Front:
          GL.Enable(EnableCap.CullFace);
          GL.CullFace(CullFaceMode.Front);
          break;
        default:
          throw new ArgumentOutOfRangeException("cullMode");
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
