using System;
using System.IO;
using OpenTK;
using ThreeAPI.geometry;
using ThreeAPI.materials;
using ThreeAPI.renderer;
using OpenTK;
using GL = OpenTK.Graphics.OpenGL.GL;
using OpenTK.Graphics.OpenGL;

namespace Starter3D.API.OpenGLRendering
{
  public class OpenGLRenderer : IRenderer
  {
    private int _programHandle;
    private int _objHandle;

    public void ConfigureMesh(IMesh mesh)
    {
      ConfigureMaterial(mesh.Material);
      GL.GenVertexArrays(1, out  _objHandle);
      GL.BindVertexArray(_objHandle);
      LoadVertexPositions(_programHandle, GetVertexPositions(mesh));
      LoadIndexer(_programHandle, GetFaceIndices(mesh));
    }

    private void ConfigureMaterial(IMaterial material)
    {
      string fragmentShaderSource = File.ReadAllText(material.FragmentShader);
      string vertexShaderSource = File.ReadAllText(material.VertexShader);
      _programHandle = CreateProgram(vertexShaderSource, fragmentShaderSource);
      GL.UseProgram(_programHandle);
    }

    public void Render(IMesh mesh)
    {
      GL.BindVertexArray(_objHandle);
      GL.DrawElements(BeginMode.Triangles, GetTriangleCount(mesh), DrawElementsType.UnsignedInt, IntPtr.Zero);
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

    private void LoadVertexPositions(int shaderProgramHandle, Vector3[] positionVboData)
    {
      int positionVboHandle;
      GL.GenBuffers(1, out positionVboHandle);
      GL.BindBuffer(BufferTarget.ArrayBuffer, positionVboHandle);
      GL.BufferData<Vector3>(BufferTarget.ArrayBuffer,
        new IntPtr(positionVboData.Length * Vector3.SizeInBytes),
        positionVboData, BufferUsageHint.StaticDraw);


      GL.EnableVertexAttribArray(0);
      GL.BindAttribLocation(shaderProgramHandle, 0, "inPosition");
      GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, Vector3.SizeInBytes, 0);
    }

    private void LoadIndexer(int shaderProgramHandle, uint[] indicesVboData)
    {
      int indicesVboHandle;
      GL.GenBuffers(1, out indicesVboHandle);
      GL.BindBuffer(BufferTarget.ElementArrayBuffer, indicesVboHandle);
      GL.BufferData(BufferTarget.ElementArrayBuffer,
        new IntPtr(indicesVboData.Length * sizeof(uint)),
        indicesVboData, BufferUsageHint.StaticDraw);
    }

    private Vector3[] GetVertexPositions(IMesh mesh)
    {

      Vector3[] positions = new Vector3[mesh.VerticesCount];
      int i = 0;
      foreach (IVertex v in mesh.Vertices)
      {
        positions[i] = v.Position;
        i++;
      }
      return positions;
    }

    private uint[] GetFaceIndices(IMesh mesh)
    {

      uint[] faces = new uint[mesh.FacesCount * 3];
      int i = 0;
      foreach (IFace f in mesh.Faces)
      {
        foreach (int indx in f.VertexIndices)
        {
          faces[i] = (uint)indx;
          i++;
        }
      }
      return faces;
    }

    private int GetTriangleCount(IMesh mesh)
    {
      return mesh.FacesCount * 3;
    }
  }
}
