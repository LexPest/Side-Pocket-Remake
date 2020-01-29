namespace CoreUtil.Math
{
  /// <summary>
  /// Работа с углами
  /// </summary>
  public static class Angle
  {
    /// <summary>
    /// Из градусов в радианы
    /// </summary>
    /// <param name="parDegrees">Угол в градусах</param>
    /// <returns>Угол в радианах</returns>
    public static double DegreesToRadians(double parDegrees)
    {
      return parDegrees * System.Math.PI / 180;
    }

    /// <summary>
    /// От радиан к вектору
    /// </summary>
    /// <param name="parRadians">Угол в радианах</param>
    /// <returns>Пространственный вектор</returns>
    public static SpVector3 RadiansToVector(double parRadians)
    {
      return new SpVector3(System.Math.Cos(parRadians), System.Math.Sin(parRadians));
    }

    /// <summary>
    /// Повернуть точки вокруг опоры
    /// </summary>
    /// <param name="parRotationDegreesAngle">Величина поворота в градусах</param>
    /// <param name="refPoints">Точки для поворота</param>
    /// <param name="parOrigin">Точка опоры</param>
    public static void RotateAroundPivot(double parRotationDegreesAngle, ref SpVector3[] refPoints, SpVector3 parOrigin)
    {
      double rotationRadians = DegreesToRadians(parRotationDegreesAngle);

      double s = System.Math.Sin(rotationRadians);
      double c = System.Math.Cos(rotationRadians);

      for (var index = 0; index < refPoints.Length; index++)
      {
        var refPoint = refPoints[index];

        //переместим
        double x = refPoint.X - parOrigin.X;
        double y = refPoint.Y - parOrigin.Y;

        //повернем
        double xRot = x * c - y * s;
        double yRot = x * s + y * c;

        //переместим обратно
        double xRes = xRot + parOrigin.X;
        double yRes = yRot + parOrigin.Y;

        refPoints[index] = new SpVector3(xRes, yRes);
      }
    }
  }
}