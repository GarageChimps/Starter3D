using System;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Forms.Integration;
using System.Windows.Input;
using System.Windows.Media;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using Starter3D.API.controller;
using Color = System.Drawing.Color;
using KeyEventArgs = System.Windows.Input.KeyEventArgs;
using KeyPressEventArgs = System.Windows.Forms.KeyPressEventArgs;
using MouseEventArgs = System.Windows.Input.MouseEventArgs;

namespace Starter3D.Application.ui
{
  public partial class OpenGLWindow
  {
    private GLControl _glControl;
    private readonly IController _controller;

    private TimeSpan _lastTime = TimeSpan.Zero;
    private int _lastMousePositionX;
    private int _lastMousePositionY;

    public OpenGLWindow(int width, int height, IController controller)
    {
      if (controller == null) throw new ArgumentNullException("controller");
      _controller = controller;
      Width = width;
      Height = height;
      InitializeComponent();
    }

    private void WindowsFormsHostInitialized(object sender, EventArgs e)
    {
      var flags = GraphicsContextFlags.Default;
      _glControl = new GLControl(new GraphicsMode(32, 24), 2, 0, flags);
      _glControl.MakeCurrent();
      //_glControl.Paint += Paint;
      _glControl.MouseDown += OnMouseDown;
      _glControl.MouseUp += OnMouseUp;
      _glControl.MouseMove += OnMouseMove;
      _glControl.MouseWheel += OnMouseWheel;
      _glControl.KeyPress += OnKeyPress;
      _glControl.Dock = DockStyle.Fill;
      (sender as WindowsFormsHost).Child = _glControl;

      _controller.Load();
      GL.Enable(EnableCap.DepthTest);
      GL.ClearColor(Color.AliceBlue);

      CompositionTarget.Rendering += Render;
    }

    private void Render(object sender, EventArgs e)
    {
      RenderingEventArgs args = (RenderingEventArgs)e;
      if (args.RenderingTime == _lastTime)
        return;
      
      GL.Viewport(0, 0, (int)this.Width, (int)this.Height);
      GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

      _controller.Render((args.RenderingTime - _lastTime).TotalSeconds);


      GL.Flush();
      _glControl.SwapBuffers();

      _lastTime = args.RenderingTime;

    }

    private void OnMouseDown(object sender, System.Windows.Forms.MouseEventArgs e)
    {
      var button = ControllerMouseButton.Left;
      if (e.Button == MouseButtons.Left)
        button = ControllerMouseButton.Left;
      else if (e.Button == MouseButtons.Middle)
        button = ControllerMouseButton.Middle;
      else if (e.Button == MouseButtons.Right)
        button = ControllerMouseButton.Right;
      _controller.MouseDown(button, e.X, e.Y);
    }

    private void OnMouseUp(object sender, System.Windows.Forms.MouseEventArgs e)
    {
      var button = ControllerMouseButton.Left;
      if (e.Button == MouseButtons.Left)
        button = ControllerMouseButton.Left;
      else if (e.Button == MouseButtons.Middle)
        button = ControllerMouseButton.Middle;
      else if (e.Button == MouseButtons.Right)
        button = ControllerMouseButton.Right;
      _controller.MouseUp(button, e.X, e.Y);
    }

    private void OnMouseMove(object sender, System.Windows.Forms.MouseEventArgs e)
    {
      int deltaX = (int) (e.X - _lastMousePositionX);
      int deltaY = (int)(e.Y - _lastMousePositionY);
      _controller.MouseMove(e.X, e.Y, deltaX, deltaY);
      _lastMousePositionX = e.X;
      _lastMousePositionY = e.Y;
    }

    private void OnMouseWheel(object sender, System.Windows.Forms.MouseEventArgs e)
    {
      _controller.MouseWheel(e.Delta/100, e.X, e.Y);
    }

    private void OnKeyPress(object sender, KeyPressEventArgs e)
    {
      _controller.KeyDown(e.KeyChar);
    }

  }
}
