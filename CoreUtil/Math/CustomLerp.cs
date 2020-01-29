namespace CoreUtil.Math
{
  public static class CustomLerp
  {
    /// <summary>
    /// Получить интерполированное значение из одного периода в другой
    /// </summary>
    /// <param name="parKnownValue">Значение из первого периода</param>
    /// <param name="parKnownValuePeriodStart">Начало первого периода</param>
    /// <param name="parKnownValuePeriodEnd">Конец первого периода</param>
    /// <param name="parNeededValuePeriodStart">Начало второго периода</param>
    /// <param name="parNeededValuePeriodEnd">Конец второго периода</param>
    /// <returns></returns>
    public static double GetInterpolatedValue(double parKnownValue, double parKnownValuePeriodStart,
      double parKnownValuePeriodEnd, double parNeededValuePeriodStart, double parNeededValuePeriodEnd)
    {
      double percent = (parKnownValue - parKnownValuePeriodStart) / (parKnownValuePeriodEnd - parKnownValuePeriodStart);
      return (parNeededValuePeriodEnd - parNeededValuePeriodStart) * percent + parNeededValuePeriodStart;
    }
  }
}