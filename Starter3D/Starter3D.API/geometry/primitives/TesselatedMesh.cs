namespace Starter3D.API.geometry.primitives
{
  public abstract class TesselatedMesh : Mesh
  {
    protected float _minU;
    protected float _maxU;
    protected float _minV;
    protected float _maxV;

    protected int _numU;
    protected int _numV;

    public int NumU
    {
      get { return _numU; }
      set { _numU = value; }
    }

    public int NumV
    {
      get { return _numV; }
      set { _numV = value; }
    }

    protected TesselatedMesh(int numU, int numV, float minU, float maxU, float minV, float maxV)
    {
      _minU = minU;
      _maxU = maxU;
      _minV = minV;
      _maxV = maxV;
      Tesselate(numU, numV);

    }

    protected abstract IVertex GetVertex(float u, float v);

    public void Tesselate(int numU, int numV)
    {
      _numU = numU;
      _numV = numV;
      Reset();
      float dU = (_maxU - _minU) / _numU;
      float dV = (_maxV - _minV) / _numV;

      for (int iu = 0; iu <= _numU; iu++)
      {
        float u = _minU + iu * dU;
        for (int iv = 0; iv <= _numV; iv++)
        {
          float v = _minV + iv * dV;
          var vertex = GetVertex(u, v);
          AddVertex(vertex);
        }
      }

      for (int iu = 0; iu < _numU; iu++)
      {
        for (int iv = 0; iv < _numV; iv++)
        {
          var index00 = GetIndex(iu, iv);
          var index01 = GetIndex(iu, iv + 1);
          var index10 = GetIndex(iu + 1, iv);
          var index11 = GetIndex(iu + 1, iv + 1);
          AddFace(new Face(index00, index11, index01));
          AddFace(new Face(index00, index10, index11));
        }
      }
    }

    private int GetIndex(int iu, int iv)
    {
      return iu * _numV + iv;
    }

  }
}