namespace Starter3D.API.controller
{
  public enum ControllerMouseButton
  {
    Left = 0,
    Middle,
    Right
  }

  public interface IController
  {
    int Width { get; }
    int Height { get; }
    bool IsFullScreen { get; }
    object View { get; }
    bool HasUserInterface { get; }
    string Name { get; }

    void Load();
    void Render(double time);
    void Update(double time);
    void MouseDown(ControllerMouseButton button, int x, int y);
    void MouseUp(ControllerMouseButton button, int x, int y);
    void MouseWheel(int delta, int x, int y);
    void MouseMove(int x, int y, int deltaX, int deltaY);
    void KeyDown(int key);
    void UpdateSize(double width, double height);
  }
}
