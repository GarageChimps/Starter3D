using System;
using Flaxen.SlimDXControlLib;
using SlimDX;
using Starter3D.API.controller;
using SlimDX.Direct3D10;
using Device = SlimDX.Direct3D10_1.Device1;

namespace Starter3D.Application.ui
{
  public class Direct3DRenderingAdapter : SimpleRenderEngine
  {
    private readonly IController _controller;
    private readonly double _frameRate;
    private TimeSpan _lastRenderingTime = TimeSpan.Zero;
    private TimeSpan _lastUpdateTime = TimeSpan.Zero;
    
    public Direct3DRenderingAdapter(IController controller, Device device, double frameRate)
    {
      if (controller == null) throw new ArgumentNullException("controller");
      if (device == null) throw new ArgumentNullException("device");
      _controller = controller;
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

      if (deltaRenderingSeconds > 1.0/_frameRate)
      {
        Device.OutputMerger.SetTargets(SampleDepthView, SampleRenderView);
        Device.Rasterizer.SetViewports(ViewPort);

        Device.ClearDepthStencilView(SampleDepthView, DepthStencilClearFlags.Depth | DepthStencilClearFlags.Stencil,
          1.0f, 0);
        Device.ClearRenderTargetView(SampleRenderView, new SlimDX.Color4(new Vector3(0.9f, 0.9f, 1)));

        _controller.Render(deltaRenderingSeconds);

        Device.Flush();
        _lastRenderingTime = elapsedTime;
      }
      _lastUpdateTime = elapsedTime;
    }
  }

}
