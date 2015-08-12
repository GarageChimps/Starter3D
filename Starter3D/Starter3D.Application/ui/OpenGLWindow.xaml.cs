using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Forms;
using System.Windows.Forms.Integration;
using System.Windows.Input;
using System.Windows.Media;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using Starter3D.API.controller;
using Starter3D.API.utils;
using KeyEventArgs = System.Windows.Input.KeyEventArgs;
using KeyPressEventArgs = System.Windows.Forms.KeyPressEventArgs;
using MouseEventArgs = System.Windows.Forms.MouseEventArgs;
using WindowState = System.Windows.WindowState;

namespace Starter3D.Application.ui
{
  public partial class OpenGLWindow
  {
    private GLControl _glControl;
    private readonly IController _controller;

    private readonly double _frameRate; 
    private TimeSpan _lastRenderingTime = TimeSpan.Zero;
    private TimeSpan _lastUpdateTime = TimeSpan.Zero;
    private int _lastMousePositionX;
    private int _lastMousePositionY;

    public OpenGLWindow(IController controller, double frameRate)
    {
      if (controller == null) throw new ArgumentNullException("controller");
      _controller = controller;
      _frameRate = frameRate;
      Width = controller.Width;
      Height = controller.Height;
      if (_controller.IsFullScreen)
        WindowState = WindowState.Maximized;
      Title = controller.Name;

      InitializeComponent();
      if (_controller.HasUserInterface)
      {
        if(_controller.CentralView != null)
          MainGrid.Children.Add((UIElement)_controller.CentralView);
        if (_controller.LeftView != null)
          LeftGrid.Children.Add((UIElement)_controller.LeftView);
        if (_controller.RightView != null)
          RightGrid.Children.Add((UIElement)_controller.RightView);
        if (_controller.TopView != null)
          TopGrid.Children.Add((UIElement)_controller.TopView);
        if (_controller.BottomView != null)
          BottomGrid.Children.Add((UIElement)_controller.BottomView);
        
      }
      SizeChanged += OnSizeChanged;
      KeyDown += OnKeyPress;
    }

    private void OnSizeChanged(object sender, SizeChangedEventArgs e)
    {
      _controller.UpdateSize(_glControl.Width, _glControl.Height);
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
      _glControl.Dock = DockStyle.Fill;
      (sender as WindowsFormsHost).Child = _glControl;

      _controller.Load();

      CompositionTarget.Rendering += Render;
    }

    private void Render(object sender, EventArgs e)
    {
      RenderingEventArgs args = (RenderingEventArgs)e;
      if (args.RenderingTime == _lastUpdateTime)
        return;

      var deltaUpdateSeconds = (args.RenderingTime - _lastUpdateTime).TotalSeconds;
      var deltaRenderingSeconds = (args.RenderingTime - _lastRenderingTime).TotalSeconds;
      _controller.Update(deltaUpdateSeconds);

      if (deltaRenderingSeconds > 1.0/_frameRate)
      {

        GL.Viewport(0, 0, (int) _glControl.Width, (int) _glControl.Height);
        GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

        _controller.Render(deltaRenderingSeconds);

        GL.Flush();
        _glControl.SwapBuffers();

        _lastRenderingTime = args.RenderingTime;
      }
      _lastUpdateTime = args.RenderingTime;

    }

    private void OnMouseDown(object sender, MouseEventArgs e)
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

    private void OnMouseUp(object sender, MouseEventArgs e)
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

    private void OnMouseMove(object sender, MouseEventArgs e)
    {
      int deltaX = (int)(e.X - _lastMousePositionX);
      int deltaY = (int)(e.Y - _lastMousePositionY);
      _controller.MouseMove(e.X, e.Y, deltaX, deltaY);
      _lastMousePositionX = e.X;
      _lastMousePositionY = e.Y;
    }

    private void OnMouseWheel(object sender, MouseEventArgs e)
    {
      _controller.MouseWheel(e.Delta / 100, e.X, e.Y);
    }

    private void OnKeyPress(object sender, KeyEventArgs e)
    {
      var formsKey = (Keys)KeyInterop.VirtualKeyFromKey(e.Key);
      _controller.KeyDown((int)formsKey);
    }

  }
}
