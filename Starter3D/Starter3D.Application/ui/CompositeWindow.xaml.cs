using System;
using System.Runtime.InteropServices;
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
using Starter3D.API.renderer;
using Starter3D.Renderers;
using KeyEventArgs = System.Windows.Forms.KeyEventArgs;
using KeyPressEventArgs = System.Windows.Forms.KeyPressEventArgs;
using MouseEventArgs = System.Windows.Forms.MouseEventArgs;
using WindowState = System.Windows.WindowState;

namespace Starter3D.Application.ui
{
  public partial class CompositeWindow
  {
    private CompositeRenderingAdapter _renderingAdapter;
    private GLControl _glControl;
    private readonly IController _controller;
    private readonly IRenderer _renderer;

    private TimeSpan _lastTime = TimeSpan.Zero;
    private int _lastMousePositionX;
    private int _lastMousePositionY;

    public CompositeWindow(IController controller, IRenderer renderer)
    {
      if (controller == null) throw new ArgumentNullException("controller");
      if (renderer == null) throw new ArgumentNullException("renderer");
      _controller = controller;
      _renderer = renderer;
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
      Loaded += OnLoaded;
      SizeChanged += OnSizeChanged;
      
    }

    private void OnLoaded(object sender, RoutedEventArgs e)
    {
      if (_glControl != null)
      {
        _renderingAdapter = new CompositeRenderingAdapter(_controller,
          ((CompositeRenderer) _renderer).D3DRenderer.Direct3DDevice, _glControl);
        direct3DControl.RegisterRenderer(_renderingAdapter, (int) ActualWidth, (int) ActualHeight);
      }
      direct3DControl.Loaded += OnDirect3DControlLoaded;
      direct3DControl.SizeChanged += OnSizeChanged;
      direct3DControl.MouseLeftButtonDown += OnMouseLeftButtonDown;
      direct3DControl.MouseRightButtonDown += OnMouseRightButtonDown;
      direct3DControl.MouseLeftButtonUp += OnMouseLeftButtonUp;
      direct3DControl.MouseRightButtonUp += OnMouseRightButtonUp;
      direct3DControl.MouseMove += OnMouseMoveD3D;
      direct3DControl.MouseWheel += OnMouseWheel;
      direct3DControl.KeyDown += OnKeyPress;

      //_controller.Load();
    }

    private void OnDirect3DControlLoaded(object sender, RoutedEventArgs e)
    {
      if (_renderingAdapter != null)
        _renderingAdapter.Reinitialize((int)direct3DControl.ActualWidth, (int)direct3DControl.ActualHeight);
      _controller.UpdateSize(direct3DControl.ActualWidth, direct3DControl.ActualHeight);
    }

    private void OnSizeChanged(object sender, SizeChangedEventArgs e)
    {
      if(_renderingAdapter != null)
        _renderingAdapter.Reinitialize((int)direct3DControl.ActualWidth, (int)direct3DControl.ActualHeight);
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
      _glControl.KeyPress += OnKeyPress;
      _glControl.Dock = DockStyle.Fill;
      (sender as WindowsFormsHost).Child = _glControl;

      //_renderingAdapter = new CompositeRenderingAdapter(_controller, ((CompositeRenderer)_renderer).D3DRenderer.Direct3DDevice, _glControl);
      //direct3DControl.RegisterRenderer(_renderingAdapter, (int)ActualWidth, (int)ActualHeight);
      

      _controller.Load();

      //CompositionTarget.Rendering += Render;
    }

    private void Render(object sender, EventArgs e)
    {
      RenderingEventArgs args = (RenderingEventArgs)e;
      if (args.RenderingTime == _lastTime)
        return;

      GL.Viewport(0, 0, (int)_glControl.Width, (int)_glControl.Height);
      GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

      _controller.Render((args.RenderingTime - _lastTime).TotalSeconds);


      GL.Flush();
      _glControl.SwapBuffers();

      _lastTime = args.RenderingTime;

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

    private void OnKeyPress(object sender, KeyPressEventArgs e)
    {
      _controller.KeyDown(e.KeyChar);
    }

    private void OnMouseRightButtonUp(object sender, MouseButtonEventArgs e)
    {
      _controller.MouseUp(ControllerMouseButton.Right, (int)e.GetPosition(this).X, (int)e.GetPosition(this).X);
    }

    private void OnMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
    {
      _controller.MouseUp(ControllerMouseButton.Left, (int)e.GetPosition(this).X, (int)e.GetPosition(this).X);
    }

    private void OnMouseRightButtonDown(object sender, MouseButtonEventArgs e)
    {
      _controller.MouseDown(ControllerMouseButton.Right, (int)e.GetPosition(this).X, (int)e.GetPosition(this).X);
    }

    private void OnMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
    {
      _controller.MouseDown(ControllerMouseButton.Left, (int)e.GetPosition(this).X, (int)e.GetPosition(this).X);
    }


    private void OnMouseMoveD3D(object sender, System.Windows.Input.MouseEventArgs e)
    {
      int deltaX = (int)((int)e.GetPosition(this).X - _lastMousePositionX);
      int deltaY = (int)((int)e.GetPosition(this).Y - _lastMousePositionY);
      _controller.MouseMove((int)e.GetPosition(this).X, (int)e.GetPosition(this).Y, deltaX, deltaY);
      _lastMousePositionX = (int)e.GetPosition(this).X;
      _lastMousePositionY = (int)e.GetPosition(this).Y;
    }

    private void OnMouseWheel(object sender, MouseWheelEventArgs e)
    {
      _controller.MouseWheel(e.Delta / 100, (int)e.GetPosition(this).X, (int)e.GetPosition(this).X);
    }

    private void OnKeyPress(object sender, System.Windows.Input.KeyEventArgs e)
    {
      _controller.KeyDown((int)e.Key);
    }
  }
}
