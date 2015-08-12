using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using Flaxen.SlimDXControlLib;
using Starter3D.API.controller;
using Starter3D.API.renderer;
using Starter3D.API.utils;
using Starter3D.Renderers;
using KeyEventArgs = System.Windows.Input.KeyEventArgs;
using MouseEventArgs = System.Windows.Input.MouseEventArgs;

namespace Starter3D.Application.ui
{
  public partial class Direct3DWindow
  {
    private readonly Direct3DRenderingAdapter _renderingAdapter;
    private readonly Direct3DRenderer _renderer;
    private readonly IController _controller;
    private int _lastMousePositionX;
    private int _lastMousePositionY;

    public Direct3DWindow(IController controller, IRenderer renderer, double frameRate)
    {
      if (controller == null) throw new ArgumentNullException("controller");
      if (renderer == null) throw new ArgumentNullException("renderer");
      _controller = controller;
      _renderer = (Direct3DRenderer) renderer;
      _renderingAdapter = new Direct3DRenderingAdapter(_controller, _renderer, _renderer.Direct3DDevice, frameRate);

      Width = controller.Width;
      Height = controller.Height;
      if (_controller.IsFullScreen)
        WindowState = WindowState.Maximized;
      Title = controller.Name;

      InitializeComponent();
      if (_controller.HasUserInterface)
      {
        if (_controller.CentralView != null)
          MainGrid.Children.Add((UIElement) _controller.CentralView);
        if (_controller.LeftView != null)
          LeftGrid.Children.Add((UIElement) _controller.LeftView);
        if (_controller.RightView != null)
          RightGrid.Children.Add((UIElement) _controller.RightView);
        if (_controller.TopView != null)
          TopGrid.Children.Add((UIElement) _controller.TopView);
        if (_controller.BottomView != null)
          BottomGrid.Children.Add((UIElement) _controller.BottomView);

      }
      Loaded += OnLoaded;
      Closing += OnClosing;
      KeyDown += OnKeyPress;
    }
  

    private void OnClosing(object sender, CancelEventArgs e)
    {
      _renderingAdapter.Dispose();
    }

    private void OnLoaded(object sender, RoutedEventArgs e)
    {
      direct3DControl.RegisterRenderer(_renderingAdapter, (int)ActualWidth, (int)ActualHeight);
      direct3DControl.Loaded += OnDirect3DControlLoaded;
      direct3DControl.SizeChanged += OnSizeChanged;
      direct3DControl.MouseLeftButtonDown += OnMouseLeftButtonDown;
      direct3DControl.MouseRightButtonDown += OnMouseRightButtonDown;
      direct3DControl.MouseLeftButtonUp += OnMouseLeftButtonUp;
      direct3DControl.MouseRightButtonUp += OnMouseRightButtonUp;
      direct3DControl.MouseMove += OnMouseMove;
      direct3DControl.MouseWheel += OnMouseWheel;

      _controller.Load();
    }
    
    private void OnDirect3DControlLoaded(object sender, RoutedEventArgs e)
    {
      _renderingAdapter.Reinitialize((int)direct3DControl.ActualWidth, (int)direct3DControl.ActualHeight);
      _controller.UpdateSize(direct3DControl.ActualWidth, direct3DControl.ActualHeight);
    }

    private void OnSizeChanged(object sender, SizeChangedEventArgs e)
    {
      _controller.UpdateSize(direct3DControl.ActualWidth, direct3DControl.ActualHeight);
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
      var formsKey = (Keys)KeyInterop.VirtualKeyFromKey(e.Key);
      _controller.KeyDown((int)formsKey);
    }

  }
}
