using System;
using ThreeAPI.examples;

namespace ThreeDU
{
  public class ShaderExample
  {
    public static int WindowWidth = 512;
    public static int WindowHeight = 512;
    public static float FrameRate = 60;

    public ShaderExample ()
    {
    }

    [STAThread]
    public static void Main()
    {
      using (var window = new PlainWindow(WindowWidth, WindowHeight))
      {
        window.Run(FrameRate);
      }
    }
  }
}

