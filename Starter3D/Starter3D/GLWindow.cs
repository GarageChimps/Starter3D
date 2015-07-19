using System;
using System.Drawing;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Input;
using Starter3D.API.controller;
using ClearBufferMask = OpenTK.Graphics.OpenGL.ClearBufferMask;
using EnableCap = OpenTK.Graphics.OpenGL.EnableCap;
using GL = OpenTK.Graphics.OpenGL.GL;

namespace Starter3D.OpenGL
{
  public class GLWindow : GameWindow
  {
    private IController _controller;

    public GLWindow(int width, int height, IController controller)
      : base(width, height,
        new GraphicsMode(), "Starter3D.OpenGL", GameWindowFlags.Default,
        DisplayDevice.Default, 3, 0,
        GraphicsContextFlags.ForwardCompatible | GraphicsContextFlags.Debug)
    {
      _controller = controller;
    }

    protected override void OnLoad(EventArgs e)
    {
      _controller.Load();
      GL.Enable(EnableCap.DepthTest);
      GL.ClearColor(Color.AliceBlue);
    }

    protected override void OnRenderFrame(FrameEventArgs e)
    {
      GL.Viewport(0, 0, this.Width, this.Height);
      GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

      _controller.Render(e.Time);
      

      GL.Flush();
      SwapBuffers();
    }

    protected override void OnUpdateFrame(FrameEventArgs e)
    {
      _controller.Update(e.Time);
    }

    protected override void OnMouseDown(MouseButtonEventArgs e)
    {
      var button = ControllerMouseButton.Left;
      if(e.Button == MouseButton.Left)
        button = ControllerMouseButton.Left;
      else if (e.Button == MouseButton.Middle)
        button = ControllerMouseButton.Middle;
      else if (e.Button == MouseButton.Right)
        button = ControllerMouseButton.Right;
      _controller.MouseDown(button, e.X, e.Y);
    }

    protected override void OnMouseWheel(MouseWheelEventArgs e)
    {
      _controller.MouseWheel(e.Delta, e.X, e.Y);
      
    }

    protected override void OnMouseMove(MouseMoveEventArgs e)
    {
      _controller.MouseMove(e.X, e.Y);
    }

    protected override void OnKeyDown(KeyboardKeyEventArgs e)
    {
      _controller.KeyDown((int)e.Key);
    }
  }
}

