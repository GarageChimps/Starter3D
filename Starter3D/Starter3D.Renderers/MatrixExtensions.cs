namespace Starter3D.Renderers
{
  public static class MatrixExtensions
  {
    public static SlimDX.Matrix ToSlimDXMatrix(this OpenTK.Matrix4 matrix)
    {
      var matrixDX = new SlimDX.Matrix();
      for (int row = 0; row < 4; row++)
      {
        for (int column = 0; column < 4; column++)
        {
          matrixDX[row, column] = matrix[row, column];
        }
      }
      return matrixDX;
    }
  }
}