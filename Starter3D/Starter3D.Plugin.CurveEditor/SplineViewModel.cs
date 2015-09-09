namespace Starter3D.Plugin.CurveEditor
{
  public class SplineViewModel : ViewModelBase
  {
    private readonly ISpline _spline;
    
    public ISpline Spline
    {
      get { return _spline; }
    }
    
    public SplineViewModel(ISpline spline)
    {
      _spline = spline;
    }

    
  }
}