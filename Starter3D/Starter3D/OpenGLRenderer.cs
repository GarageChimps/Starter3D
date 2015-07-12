using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using ThreeAPI.geometry;
using ThreeAPI.renderer;

namespace Starter3D
{
  public class OpenGLRenderer : IRenderer
  {
    private int _programHandle;
    private int _objHandle;

    public void Render(IMesh mesh)
    {
      GL.BindVertexArray(_objHandle);
      GL.DrawElements(BeginMode.Triangles, GetTriangleCount(mesh), DrawElementsType.UnsignedInt, IntPtr.Zero);
    }

    public void SetShaders(string vertexShaderPath, string fragmentShaderPath)
    {
      string fragmentShaderSource = File.ReadAllText(fragmentShaderPath);
      string vertexShaderSource = File.ReadAllText(vertexShaderPath);
      _programHandle = CreateProgram(vertexShaderSource, fragmentShaderSource);
      GL.UseProgram(_programHandle);
    }

    public void AddMesh(IMesh mesh)
    {
      GL.GenVertexArrays(1, out  _objHandle);
      GL.BindVertexArray(_objHandle);
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
      GL.EnableVertexAttribArray(index);
      //int location = GL.GetAttribLocation(_programHandle, name);
      GL.BindAttribLocation(_programHandle, index, name);
      GL.VertexAttribPointer(index, 3, VertexAttribPointerType.Float, false, stride, IntPtr.Add(IntPtr.Zero, offset));
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

    private int GetTriangleCount(IMesh mesh)
    {
      return mesh.FacesCount * 3;
    }
  }
}
