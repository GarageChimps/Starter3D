using System;
using Flaxen.SlimDXControlLib;
using SlimDX;
using Starter3D.API.controller;
using SlimDX.Direct3D10;
using Device = SlimDX.Direct3D10_1.Device1;

namespace Starter3D.Application.ui
{
  /// <summary>
  /// Class description
  /// </summary>
  /// <history>
  /// 11/21/2012 8:43:17 AM - Alejandro : Class creation
  /// </history>
  public class Direct3DRenderingAdapter : SimpleRenderEngine
  {
    private readonly IController _controller;

    public Direct3DRenderingAdapter(IController controller, Device device)
    {
      _controller = controller;
      Device = device;
    }

    public override void Render(TimeSpan elapsedTime)
    {
      Device.OutputMerger.SetTargets(SampleDepthView, SampleRenderView);
      Device.Rasterizer.SetViewports(ViewPort);

      Device.ClearDepthStencilView(SampleDepthView, DepthStencilClearFlags.Depth | DepthStencilClearFlags.Stencil, 1.0f, 0);
      Device.ClearRenderTargetView(SampleRenderView, new SlimDX.Color4(new Vector3(0.9f,0.9f,1)));

      _controller.Render(elapsedTime.Ticks);

      Device.Flush();
    }
  }

}
