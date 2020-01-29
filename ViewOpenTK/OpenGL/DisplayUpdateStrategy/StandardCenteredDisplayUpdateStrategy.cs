using System;

namespace ViewOpenTK.OpenGL.DisplayUpdateStrategy
{
  /// <summary>
  /// Реализация стандартной стратегии обновления дисплея приложения с упором на рендеринг в центре экрана
  /// не растянутого изображения
  /// </summary>
  public class StandardCenteredDisplayUpdateStrategy : IDisplayUpdateStrategy
  {
    /// <summary>
    /// Стандартный конструктор
    /// </summary>
    /// <param name="parShouldRescaleBeWholeNumber">Должен ли фактор масштабирования быть обязательно целочисленным (для Pixel-Perfect)</param>
    public StandardCenteredDisplayUpdateStrategy(bool parShouldRescaleBeWholeNumber)
    {
      ShouldRescaleBeWholeNumber = parShouldRescaleBeWholeNumber;
    }

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
    public void GetDisplayData(double parWindowWidth, double parWindowHeight, double parViewportWidth, double parViewportHeight,
      out double outDisplayViewportGlUnitsX1, out double outDisplayViewportGlUnitsY1, out double outDisplayViewportGlUnitsX2,
      out double outDisplayViewportGlUnitsY2, out double outGlobalRescale)
    {
      double byWidthRescale = (double) parWindowWidth / parViewportWidth;
      double byHeightRescale = (double) parWindowHeight / parViewportHeight;

      outGlobalRescale = (Math.Min(byWidthRescale, byHeightRescale));

      if (ShouldRescaleBeWholeNumber)
      {
        outGlobalRescale = Math.Floor(outGlobalRescale);
      }

      double rescaledWidth = parViewportWidth * outGlobalRescale;
      double rescaledHeight = parViewportHeight * outGlobalRescale;

      // расчет координат середины экрана
      double middleX = parWindowWidth / 2;
      double middleY = parWindowHeight / 2;

      double leftupperX = middleX - rescaledWidth / 2;
      double leftupperY = middleY - rescaledHeight / 2;

      double rightbottomX = leftupperX + rescaledWidth;
      double rightbottomY = leftupperY + rescaledHeight;

      outDisplayViewportGlUnitsX1 = OpenGlUtil.GetOpenGlCoordX(leftupperX, parWindowWidth);
      outDisplayViewportGlUnitsY1 = OpenGlUtil.GetOpenGlCoordY(leftupperY, parWindowHeight);

      outDisplayViewportGlUnitsX2 = OpenGlUtil.GetOpenGlCoordX(rightbottomX, parWindowWidth);
      outDisplayViewportGlUnitsY2 = OpenGlUtil.GetOpenGlCoordY(rightbottomY, parWindowHeight);
    }

    public bool ShouldRescaleBeWholeNumber { get; set; }
  }
}