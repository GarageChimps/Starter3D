using System;
using System.Collections.Generic;
using OpenTK;
using GL = OpenTK.Graphics.OpenGL.GL;
using OpenTK.Graphics.OpenGL;

namespace ThreeAPI.renderer
{
  public static class OpenGLHelper
  {

    public static int CreateShader(string shaderSource, ShaderType type){
      int shaderHandle = GL.CreateShader( type );
      GL.ShaderSource( shaderHandle, shaderSource );
      GL.CompileShader( shaderHandle );
      Console.WriteLine(GL.GetShaderInfoLog(shaderHandle));
      return shaderHandle;
    }

    public static int CreateVertexShader(string shaderSource){
      return CreateShader (shaderSource, ShaderType.VertexShader);
    }

    public static int CreateFragmentShader(string shaderSource){
      return CreateShader (shaderSource, ShaderType.FragmentShader);
    }

    public static int CreateProgram(string vertexShaderSource, string fragmentShaderSource){
      int vsHandle = CreateVertexShader (vertexShaderSource);
      int fsHandle = CreateFragmentShader (fragmentShaderSource);
      return CreateProgram (vsHandle, fsHandle);
    }

    public static int CreateProgram(int vertexShaderHandle, int fragmentShaderHandle){
      
      int shaderProgramHandle = GL.CreateProgram();

      GL.AttachShader( shaderProgramHandle, vertexShaderHandle );
      GL.AttachShader( shaderProgramHandle, fragmentShaderHandle );

      GL.LinkProgram( shaderProgramHandle );

      Console.WriteLine( GL.GetProgramInfoLog( shaderProgramHandle) );
      return shaderProgramHandle;
    }

    public static void LoadVertexPositions(int shaderProgramHandle, Vector3[] positionVboData)
    {
      int positionVboHandle;
      GL.GenBuffers( 1, out positionVboHandle );
      GL.BindBuffer( BufferTarget.ArrayBuffer, positionVboHandle );
      GL.BufferData<Vector3>( BufferTarget.ArrayBuffer,
        new IntPtr( positionVboData.Length * Vector3.SizeInBytes ),
        positionVboData, BufferUsageHint.StaticDraw );


      GL.EnableVertexAttribArray( 0 );
      GL.BindAttribLocation( shaderProgramHandle, 0, "inPosition" );
      GL.VertexAttribPointer( 0, 3, VertexAttribPointerType.Float, false, Vector3.SizeInBytes, 0 );            
    }

    public static void LoadIndexer(int shaderProgramHandle, uint [] indicesVboData)
    {
      int indicesVboHandle;
      GL.GenBuffers( 1, out indicesVboHandle );
      GL.BindBuffer( BufferTarget.ElementArrayBuffer, indicesVboHandle );
      GL.BufferData( BufferTarget.ElementArrayBuffer, 
        new IntPtr( indicesVboData.Length * sizeof(uint) ),
        indicesVboData, BufferUsageHint.StaticDraw );
    }
  }
}

