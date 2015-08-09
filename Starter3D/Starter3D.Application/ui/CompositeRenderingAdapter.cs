using System;
using Flaxen.SlimDXControlLib;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using Starter3D.API.controller;
using SlimDX.Direct3D10;
using Device = SlimDX.Direct3D10_1.Device1;
using Vector3 = SlimDX.Vector3;

namespace Starter3D.Application.ui
{
  public class CompositeRenderingAdapter : SimpleRenderEngine
  {
    private readonly IController _controller;
    private readonly GLControl _glControl;

    public CompositeRenderingAdapter(IController controller, Device device, GLControl glControl)
    {
      _controller = controller;
      _glControl = glControl;
      Device = device;
    }

    public override void Render(TimeSpan elapsedTime)
    {
      Device.OutputMerger.SetTargets(SampleDepthView, SampleRenderView);
      Device.Rasterizer.SetViewports(ViewPort);

      Device.ClearDepthStencilView(SampleDepthView, DepthStencilClearFlags.Depth | DepthStencilClearFlags.Stencil, 1.0f, 0);
      Device.ClearRenderTargetView(SampleRenderView, new SlimDX.Color4(new Vector3(0.9f, 0.9f, 1)));

      GL.Viewport(0, 0, (int)_glControl.Width, (int)_glControl.Height);
      GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

      _controller.Render(elapsedTime.Ticks);

      GL.Flush();
      _glControl.SwapBuffers();

      Device.Flush();
    }
  }
}