namespace ViewOpenTK.OpenGL.DisplayUpdateStrategy
{
  /// <summary>
  /// Интерфейс стратегии обновления дисплея для отрисовки OpenGL
  /// </summary>
  public interface IDisplayUpdateStrategy
  {
    /// <summary>
    /// Получить полную информацию о дисплее приложения для отрисовки OpenGL
    /// </summary>
    /// <param name="parWindowWidth">Ширина окна</param>
    /// <param name="parWindowHeight">Высота окна</param>
    /// <param name="parViewportWidth">Ширина вида</param>
    /// <param name="parViewportHeight">Высота вида</param>
    /// <param name="outDisplayViewportGlUnitsX1">Выходной параметр координаты экрана OpenGL для дисплея X1</param>
    /// <param name="outDisplayViewportGlUnitsY1">Выходной параметр координаты экрана OpenGL для дисплея Y1</param>
    /// <param name="outDisplayViewportGlUnitsX2">Выходной параметр координаты экрана OpenGL для дисплея X2</param>
    /// <param name="outDisplayViewportGlUnitsY2">Выходной параметр координаты экрана OpenGL для дисплея Y2</param>
    /// <param name="outGlobalRescale">Выходной параметр фактора масштабирования</param>
    void GetDisplayData(double parWindowWidth, double parWindowHeight, double parViewportWidth, double parViewportHeight,
      out double outDisplayViewportGlUnitsX1, out double outDisplayViewportGlUnitsY1,
      out double outDisplayViewportGlUnitsX2, out double outDisplayViewportGlUnitsY2, out double outGlobalRescale);
  }
}