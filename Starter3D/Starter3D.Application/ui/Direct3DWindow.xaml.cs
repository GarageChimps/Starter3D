using System;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using Flaxen.SlimDXControlLib;
using Starter3D.API.controller;
using Starter3D.API.renderer;
using Starter3D.Renderers;

namespace Starter3D.Application.ui
{
  public partial class Direct3DWindow
  {
    private readonly Direct3DRenderingAdapter _renderingAdapter;
    private readonly Direct3DRenderer _renderer;
    private readonly IController _controller;

    private TimeSpan _lastTime = TimeSpan.Zero;
    private int _lastMousePositionX;
    private int _lastMousePositionY;

    public Direct3DWindow(IController controller, IRenderer renderer)
    {
      if (controller == null) throw new ArgumentNullException("controller");
      _controller = controller;
      _renderer = (Direct3DRenderer) renderer;
      _renderingAdapter = new Direct3DRenderingAdapter(_controller, _renderer.Direct3DDevice);

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

    }

    private void OnLoaded(object sender, RoutedEventArgs e)
    {
      direct3DControl.RegisterRenderer(_renderingAdapter, (int)ActualWidth, (int)ActualHeight);

      direct3DControl.SizeChanged += OnSizeChanged;
      direct3DControl.MouseDown += OnMouseDown;
      direct3DControl.MouseUp += OnMouseUp;
      direct3DControl.MouseMove += OnMouseMove;
      direct3DControl.MouseWheel += OnMouseWheel;
      direct3DControl.KeyDown += OnKeyPress;

      _controller.Load();
    }

    private void OnSizeChanged(object sender, SizeChangedEventArgs e)
    {
      _controller.UpdateSize(direct3DControl.Width, direct3DControl.Height);
    }
    

    private void OnMouseDown(object sender, MouseButtonEventArgs e)
    {
      var button = GetButton(e);
      _controller.MouseDown(button, (int)e.GetPosition(this).X, (int)e.GetPosition(this).X);
    }

    private static ControllerMouseButton GetButton(MouseButtonEventArgs e)
    {
      var button = ControllerMouseButton.Left;
      if (e.LeftButton == MouseButtonState.Pressed)
        button = ControllerMouseButton.Left;
      else if (e.MiddleButton == MouseButtonState.Pressed)
        button = ControllerMouseButton.Middle;
      else if (e.RightButton == MouseButtonState.Pressed)
        button = ControllerMouseButton.Right;
      return button;
    }

    private void OnMouseUp(object sender, MouseButtonEventArgs e)
    {
      var button = GetButton(e);
      _controller.MouseUp(button, (int)e.GetPosition(this).X, (int)e.GetPosition(this).X);
    }

    private void OnMouseMove(object sender, MouseEventArgs e)
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

    private void OnKeyPress(object sender, KeyEventArgs e)
    {
      _controller.KeyDown((int)e.Key);
    }

  }
}
