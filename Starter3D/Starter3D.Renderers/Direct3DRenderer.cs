using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using OpenTK;
using SlimDX;
using SlimDX.D3DCompiler;
using SlimDX.Direct3D10;
using SlimDX.Direct3D10_1;
using SlimDX.DXGI;
using Starter3D.API.renderer;
using Color4 = OpenTK.Graphics.Color4;
using TextureMagFilter = OpenTK.Graphics.OpenGL.TextureMagFilter;
using TextureMinFilter = OpenTK.Graphics.OpenGL.TextureMinFilter;

using Device = SlimDX.Direct3D10_1.Device1;
using Vector3 = OpenTK.Vector3;
using Vector4 = SlimDX.Vector4;

namespace Starter3D.Renderers
{
  public class Direct3DRenderer : IRenderer
  {
    class RenderObject
    {
      public List<InputElement> InputElements = new List<InputElement>();
      public SlimDX.Direct3D10.Buffer VertexBuffer;
      public SlimDX.Direct3D10.Buffer IndexBuffer;
      public int IndexCount;
    }

    class ShaderProgram
    {
      public VertexShader VertexShader;
      public PixelShader PixelShader;
      public ShaderSignature Signature;
      public Effect Effect;

      public ShaderProgram(Effect effect)
      {
        Effect = effect;
      }

      public ShaderProgram(VertexShader vertexShader, PixelShader pixelShader, ShaderSignature signature)
      {
        VertexShader = vertexShader;
        PixelShader = pixelShader;
        Signature = signature;
      }
    }

   
    private const string ShaderBasePath = @"shaders/direct3d";
    private const string ShaderExtension = ".hlsl";
    private const string EffectExtension = ".fx";
    private const string ShaderVersion = "fx_4_0";

    private string _currentShader;
    private readonly Dictionary<string, ShaderProgram> _shaderHandleDictionary = new Dictionary<string, ShaderProgram>();
    private readonly Dictionary<string, RenderObject> _objectsHandleDictionary = new Dictionary<string, RenderObject>();
    private readonly Dictionary<string, ShaderResourceView> _textureHandleDictionary = new Dictionary<string, ShaderResourceView>();
    private readonly Dictionary<string, List<SlimDX.Vector4>> _vectorArrayShaderParameterDictionary = new Dictionary<string, List<SlimDX.Vector4>>();

    private readonly Dictionary<string, string> _semanticsTable = new Dictionary<string, string>();

    private readonly Device _device;

    public Device Direct3DDevice
    {
      get { return _device; }
    }


    public Direct3DRenderer()
    {
      _device = new Device(DriverType.Hardware, DeviceCreationFlags.BgraSupport, FeatureLevel.Level_10_0);
      _semanticsTable.Add("inPosition", "POSITION");
      _semanticsTable.Add("inNormal", "NORMAL");
      _semanticsTable.Add("inTextureCoords", "TEXCOORD");
    }

    public void LoadObject(string objectName)
    {
      if (_objectsHandleDictionary.ContainsKey(objectName))
        return;
      _objectsHandleDictionary[objectName] = new RenderObject();
    }

    public void DrawTriangles(string objectName, int triangleCount)
    {
      if (!_objectsHandleDictionary.ContainsKey(objectName))
        throw new ApplicationException("Object must be added to the renderer before drawing");
      var effect = _shaderHandleDictionary[_currentShader].Effect;
      var technique = effect.GetTechniqueByIndex(0);
      var pass = technique.GetPassByIndex(0);
      _device.InputAssembler.SetInputLayout(new InputLayout(_device, pass.Description.Signature, _objectsHandleDictionary[objectName].InputElements.ToArray()));
      _device.InputAssembler.SetPrimitiveTopology(PrimitiveTopology.TriangleList);
      _device.InputAssembler.SetVertexBuffers(0, new VertexBufferBinding(_objectsHandleDictionary[objectName].VertexBuffer, Vector3.SizeInBytes * _objectsHandleDictionary[objectName].InputElements.Count, 0));
      _device.InputAssembler.SetIndexBuffer(_objectsHandleDictionary[objectName].IndexBuffer, Format.R32_UInt, 0);
      _device.DrawIndexed(_objectsHandleDictionary[objectName].IndexCount, 0, 0);
    }

    public void SetVerticesData(string objectName, List<Vector3> data)
    {
      if (!_objectsHandleDictionary.ContainsKey(objectName))
        throw new ApplicationException("Object must be added to the renderer before setting its vertex data");
      var verticesStream = new DataStream(data.Count * Vector3.SizeInBytes, true, true);
      foreach (var vector in data)
      {
        verticesStream.Write(new SlimDX.Vector3(vector.X, vector.Y, vector.Z));
      }
      verticesStream.Position = 0;
      var bufferDesc = new BufferDescription
      {
        BindFlags = BindFlags.VertexBuffer,
        CpuAccessFlags = CpuAccessFlags.None,
        OptionFlags = ResourceOptionFlags.None,
        SizeInBytes = data.Count * Vector3.SizeInBytes,
        Usage = ResourceUsage.Default
      };
      var vertexBuffer = new SlimDX.Direct3D10.Buffer(_device, verticesStream, bufferDesc);
      _objectsHandleDictionary[objectName].VertexBuffer = vertexBuffer;
    }

    public void SetFacesData(string objectName, List<int> indices)
    {
      if (!_objectsHandleDictionary.ContainsKey(objectName))
        throw new ApplicationException("Object must be added to the renderer before setting its index data");
      var indicesStream = new DataStream(indices.Count * sizeof(int), true, true);
      foreach (var index in indices)
      {
        indicesStream.Write(index);
      }
      indicesStream.Position = 0;
      var bufferDesc = new BufferDescription
      {
        BindFlags = BindFlags.IndexBuffer,
        CpuAccessFlags = CpuAccessFlags.None,
        OptionFlags = ResourceOptionFlags.None,
        SizeInBytes = indices.Count * sizeof(int),
        Usage = ResourceUsage.Default
      };
      var indexBuffer = new SlimDX.Direct3D10.Buffer(_device, indicesStream, bufferDesc);
      _objectsHandleDictionary[objectName].IndexBuffer = indexBuffer;
      _objectsHandleDictionary[objectName].IndexCount = indices.Count;
    }

    public void SetVertexAttribute(string objectName, string shaderName, int index, string vertexPropertyName, int stride, int offset)
    {
      if (!_objectsHandleDictionary.ContainsKey(objectName))
        throw new ApplicationException("Object must be added to the renderer before setting its index data");
      var inputElement = new InputElement(_semanticsTable[vertexPropertyName], 0, Format.R32G32B32A32_Float, offset, 0);
      _objectsHandleDictionary[objectName].InputElements.Add(inputElement);
    }
   
    public void LoadShaders(string shaderName, string vertexShaderFileName, string fragmentShaderFileName)
    {
      if (_shaderHandleDictionary.ContainsKey(shaderName))
        return;
      VertexShader vertexShader;
      PixelShader pixelShader;
      ShaderSignature shaderSignature;
      try
      {

        //using (
        //  var bytecode = ShaderBytecode.CompileFromFile(Path.Combine(ShaderBasePath, vertexShaderFileName + ShaderExtension), "VShader", "vs_4_0", ShaderFlags.None,
        //    EffectFlags.None))
        //{
        //  vertexShader = new VertexShader(_device, bytecode);
        //  shaderSignature = ShaderSignature.GetInputSignature(bytecode);
        //}
        //using (
        //  var bytecode = ShaderBytecode.CompileFromFile(Path.Combine(ShaderBasePath, fragmentShaderFileName + ShaderExtension), "FShader", "ps_4_0", ShaderFlags.None,
        //    EffectFlags.None))
        //{
        //  pixelShader = new PixelShader(_device, bytecode);
        //}
        //_shaderHandleDictionary.Add(shaderName, new ShaderProgram(vertexShader, pixelShader, shaderSignature));
        var shaderText = File.ReadAllText(Path.Combine(ShaderBasePath, fragmentShaderFileName + EffectExtension));
        var shaderEffect = Effect.FromString(_device, shaderText, ShaderVersion, ShaderFlags.None, EffectFlags.None, null, null, null);
        _shaderHandleDictionary.Add(shaderName, new ShaderProgram(shaderEffect));
      }
      catch (Exception e)
      {
        Console.WriteLine(e.Message);
      }
     
    }

    public void UseShader(string shaderName)
    {
      if (!_shaderHandleDictionary.ContainsKey(shaderName))
        throw new ApplicationException("Shader must be loaded before using it");
      //_device.VertexShader.Set(_shaderHandleDictionary[shaderName].VertexShader);
      //_device.PixelShader.Set(_shaderHandleDictionary[shaderName].PixelShader);
      _currentShader = shaderName;
      var effect = _shaderHandleDictionary[shaderName].Effect;
      var technique = effect.GetTechniqueByIndex(0);
      var pass = technique.GetPassByIndex(0);
      pass.Apply();
    }

    public void EnableZBuffer(bool enable)
    {
      //throw new NotImplementedException();
    }

    public void SetBackgroundColor(Color4 color)
    {
      //throw new NotImplementedException();
    }

    
    public void SetNumericParameter(string name, float number)
    {
      foreach (var shaderProgram in _shaderHandleDictionary)
      {
        var effect = shaderProgram.Value.Effect;
        var variable = effect.GetVariableByName(name).AsScalar();
        if (variable != null)
          variable.Set(number);
      }
    }

    public void SetNumericParameter(string name, float number, string shader)
    {
      var effect = _shaderHandleDictionary[shader].Effect;
      var variable = effect.GetVariableByName(name).AsScalar();
      if (variable != null)
        variable.Set(number);
    }

    public void SetVectorParameter(string name, Vector3 vector, string shader)
    {
      var effect = _shaderHandleDictionary[shader].Effect;
      var variable = effect.GetVariableByName(name).AsVector();
      if (variable != null)
        variable.Set(vector.ToSlimDXVector3());
    }

    public void SetVectorParameter(string name, Vector3 vector)
    {
      foreach (var shaderProgram in _shaderHandleDictionary)
      {
        var effect = shaderProgram.Value.Effect;
        var variable = effect.GetVariableByName(name).AsVector();
        if (variable != null)
          variable.Set(vector.ToSlimDXVector3());
      }
    }

    public void SetVectorArrayParameter(string name, int index, Vector3 vector, string shader)
    {
      if (!_vectorArrayShaderParameterDictionary.ContainsKey(name))
        _vectorArrayShaderParameterDictionary[name] = new List<Vector4>();
      _vectorArrayShaderParameterDictionary[name].Insert(index, vector.ToSlimDXVector4());
      var effect = _shaderHandleDictionary[shader].Effect;
      var variable = effect.GetVariableByName(name).AsVector();
      if (variable != null)
        variable.Set(_vectorArrayShaderParameterDictionary[name].ToArray());
    }


    public void SetVectorArrayParameter(string name, int index, Vector3 vector)
    {
      if(!_vectorArrayShaderParameterDictionary.ContainsKey(name))
        _vectorArrayShaderParameterDictionary[name] = new List<Vector4>();
      _vectorArrayShaderParameterDictionary[name].Insert(index, vector.ToSlimDXVector4());
        
      foreach (var shaderProgram in _shaderHandleDictionary)
      {
        var effect = shaderProgram.Value.Effect;
        var variable = effect.GetVariableByName(name).AsVector();
        if (variable != null)
          variable.Set(_vectorArrayShaderParameterDictionary[name].ToArray());
      }
    }

   

    public void SetMatrixParameter(string name, Matrix4 matrix, string shader)
    {
      var effect = _shaderHandleDictionary[shader].Effect;
      var variable = effect.GetVariableByName(name).AsMatrix();
      if(variable != null)
        variable.SetMatrix(matrix.ToSlimDXMatrix());
    }

    public void SetMatrixParameter(string name, Matrix4 matrix)
    {
      foreach (var shaderProgram in _shaderHandleDictionary)
      {
        var effect = shaderProgram.Value.Effect;
        var variable = effect.GetVariableByName(name).AsMatrix();
        if (variable != null)
          variable.SetMatrix(matrix.ToSlimDXMatrix());
      }
    }

    public void LoadTexture(string textureName, int index, Bitmap texture, TextureMinFilter minFilter, TextureMagFilter magFilter)
    {
      if (_textureHandleDictionary.ContainsKey(textureName))
        return;
      var texture2D = CreateTexture(texture.Width, texture.Height);
      LoadBitmapInTexture(texture, texture2D);
      var textureResource = CreateTextureResource(texture2D);
      _textureHandleDictionary.Add(textureName, textureResource);
    }

    public void UseTexture(string textureName, string shader, string uniformName)
    {
      if (!_textureHandleDictionary.ContainsKey(textureName))
        throw new ApplicationException("Texture has to be added before using");
      var effect = _shaderHandleDictionary[shader].Effect;
      var variable = effect.GetVariableByName(uniformName).AsResource();
      if (variable != null)
        variable.SetResource(_textureHandleDictionary[textureName]);
    }

    private Texture2D CreateTexture(int width, int height)
    {
      var desc2 = new Texture2DDescription
      {
        SampleDescription = new SlimDX.DXGI.SampleDescription(1, 0),
        Width = width,
        Height = height,
        MipLevels = 1,
        ArraySize = 1,
        Format = SlimDX.DXGI.Format.R8G8B8A8_UNorm,
        Usage = ResourceUsage.Dynamic,
        BindFlags = BindFlags.ShaderResource,
        CpuAccessFlags = CpuAccessFlags.Write
      };
      return new Texture2D(_device, desc2);
    }

    private void LoadBitmapInTexture(Bitmap bitmap, Texture2D texture)
    {
      try
      {
        var rect = texture.Map(0, MapMode.WriteDiscard, SlimDX.Direct3D10.MapFlags.None);
        if (rect.Data.CanWrite)
        {
          for (int j = 0; j < texture.Description.Height; j++)
          {
            int rowStart = j * rect.Pitch;
            rect.Data.Seek(rowStart, System.IO.SeekOrigin.Begin);
            for (int i = 0; i < texture.Description.Width; i++)
            {
              var color = bitmap.GetPixel(i, j);
              rect.Data.WriteByte((byte)color.R);
              rect.Data.WriteByte((byte)color.G);
              rect.Data.WriteByte((byte)color.B);
              rect.Data.WriteByte((byte)color.A);
            }
          }
        }
        texture.Unmap(0);


      }
      catch (Exception)
      {

        //Do nothing if texture creation fails
      }
    }

    private ShaderResourceView CreateTextureResource(Texture2D texture)
    {
      var desc = new ShaderResourceViewDescription
      {
        Format = texture.Description.Format,
        Dimension = ShaderResourceViewDimension.Texture2D,
        MostDetailedMip = 0,
        MipLevels = 1
      };
      return new ShaderResourceView(_device, texture, desc);
    }

  }
}