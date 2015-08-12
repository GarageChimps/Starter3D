using System;
using Flaxen.SlimDXControlLib;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using Starter3D.API.controller;
using SlimDX.Direct3D10;
using Starter3D.Renderers;
using Device = SlimDX.Direct3D10_1.Device1;
using Vector3 = SlimDX.Vector3;

namespace Starter3D.Application.ui
{
  public class CompositeRenderingAdapter : SimpleRenderEngine
  {
    private readonly IController _controller;
    private readonly Direct3DRenderer _renderer;
    private readonly GLControl _glControl;
    private readonly double _frameRate;
    private TimeSpan _lastRenderingTime = TimeSpan.Zero;
    private TimeSpan _lastUpdateTime = TimeSpan.Zero;

    public CompositeRenderingAdapter(IController controller, Direct3DRenderer renderer, Device device, GLControl glControl, double frameRate)
    {
      if (controller == null) throw new ArgumentNullException("controller");
      if (renderer == null) throw new ArgumentNullException("renderer");
      if (device == null) throw new ArgumentNullException("device");
      if (glControl == null) throw new ArgumentNullException("glControl");
      _controller = controller;
      _renderer = renderer;
      _glControl = glControl;
      _frameRate = frameRate;
      Device = device;
    }

    public override void Render(TimeSpan elapsedTime)
    {
      if (elapsedTime == _lastUpdateTime)
        return;

      var deltaUpdateSeconds = (elapsedTime - _lastUpdateTime).TotalSeconds;
      var deltaRenderingSeconds = (elapsedTime - _lastRenderingTime).TotalSeconds;
      _controller.Update(deltaUpdateSeconds);

      if (deltaRenderingSeconds > 1.0 / _frameRate)
      {
        Device.OutputMerger.SetTargets(SampleDepthView, SampleRenderView);
        Device.Rasterizer.SetViewports(ViewPort);

        Device.ClearDepthStencilView(SampleDepthView, DepthStencilClearFlags.Depth | DepthStencilClearFlags.Stencil, 1.0f, 0);
        Device.ClearRenderTargetView(SampleRenderView, _renderer.Background);

        GL.Viewport(0, 0, (int)_glControl.Width, (int)_glControl.Height);
        GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

        _controller.Render(deltaRenderingSeconds);

        GL.Flush();
        _glControl.SwapBuffers();

        Device.Flush();
        _lastRenderingTime = elapsedTime;
      }
      _lastUpdateTime = elapsedTime;
    }

    protected override void DisposeManaged()
    {
      _renderer.Dispose();
      if (SharedTexture != null)
      {
        SharedTexture.Dispose();
        SharedTexture = null;
      }
      if (SampleRenderView != null)
      {
        SampleRenderView.Dispose();
        SampleRenderView = null;
      }

      if (SampleDepthView != null)
      {
        SampleDepthView.Dispose();
        SampleDepthView = null;
      }
      if (Device != null)
      {
        Device.Dispose();
        Device = null;
      }
    }
  }
}