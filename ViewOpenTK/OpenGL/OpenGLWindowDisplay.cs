using System;
using ViewOpenTK.OpenGL.DisplayUpdateStrategy;
using ViewOpenTK.SPCore.ViewProvider;

namespace ViewOpenTK.OpenGL
{
  /// <summary>
  /// Информация о дисплее приложения в окне OpenGL
  /// </summary>
  public class OpenGlWindowDisplay
  {
    /// <summary>
    /// Стандартная стратегия обновления информации о дисплее приложения
    /// </summary>
    private static readonly IDisplayUpdateStrategy DefaultDisplayUpdateStrategy =
      new StandardCenteredDisplayUpdateStrategy(false);

    /// <summary>
    /// Режим рендеринга приложения
    /// </summary>
    private EOpenGlAppRenderingMode _appRenderingMode = EOpenGlAppRenderingMode.UsingFramebuffer;

    /// <summary>
    /// X1 координата дисплея приложения в системе координат OpenGL
    /// </summary>
    private double _displayViewportGlUnitsX1;

    /// <summary>
    /// X2 координата дисплея приложения в системе координат OpenGL
    /// </summary>
    private double _displayViewportGlUnitsX2;

    /// <summary>
    /// Y1 координата дисплея приложения в системе координат OpenGL
    /// </summary>
    private double _displayViewportGlUnitsY1;

    /// <summary>
    /// Y2 координата дисплея приложения в системе координат OpenGL
    /// </summary>
    private double _displayViewportGlUnitsY2;

    /// <summary>
    /// Фактор масштабирования дисплея приложения
    /// </summary>
    private double _globalRescale;

    /// <summary>
    /// Текущая выбранная стратегия обновления информации о дисплее приложения
    /// </summary>
    public IDisplayUpdateStrategy CurrentDisplayUpdateStrategy = DefaultDisplayUpdateStrategy;

    /// <summary>
    /// Событие изменения настроек рендеринга дисплея приложения
    /// </summary>
    public event Action RenderDisplayChanged;

    /// <summary>
    /// Обновить дисплей-вид приложения
    /// </summary>
    /// <param name="parWidth">Новая ширина</param>
    /// <param name="parHeight">Новая высота</param>
    public void UpdateViewport(double parWidth, double parHeight)
    {
      ViewportWidth = parWidth;
      ViewportHeight = parHeight;

      UpdateDisplay(WindowWidth, WindowHeight);
    }

    /// <summary>
    /// Обновить дисплей-вид приложения
    /// </summary>
    /// <param name="parWidth">Новая ширина</param>
    /// <param name="parHeight">Новая высота</param>
    /// <param name="parDisplayUpdateStrategy">Новая стратегия обновления дисплея приложения</param>
    public void UpdateDisplay(double parWidth, double parHeight, IDisplayUpdateStrategy parDisplayUpdateStrategy = null)
    {
      WindowWidth = parWidth;
      WindowHeight = parHeight;

      if (parDisplayUpdateStrategy != null)
      {
        CurrentDisplayUpdateStrategy = parDisplayUpdateStrategy;
      }

      CurrentDisplayUpdateStrategy.GetDisplayData(WindowWidth, WindowHeight, ViewportWidth, ViewportHeight,
        out _displayViewportGlUnitsX1, out _displayViewportGlUnitsY1, out _displayViewportGlUnitsX2,
        out _displayViewportGlUnitsY2, out _globalRescale);

      DisplayViewportGlUnitsWidth = DisplayViewportGlUnitsX2 - DisplayViewportGlUnitsX1;
      DisplayViewportGlUnitsHeight = DisplayViewportGlUnitsY2 - DisplayViewportGlUnitsY1;

      RenderDisplayChanged?.Invoke();
    }

    /// <summary>
    /// Получить данные о границах дисплея в нормализованном пространстве OpenGL
    /// </summary>
    /// <param name="parPosX">Позиция X</param>
    /// <param name="parPosY">Позиция Y</param>
    /// <param name="parWidth">Ширина</param>
    /// <param name="parHeight">Высота</param>
    /// <returns></returns>
    /// <exception cref="ArgumentOutOfRangeException"></exception>
    public OpenGlScreenNormalizedRect GetCurrentRenderingModeNormalizedRect(double parPosX, double parPosY,
      double parWidth, double parHeight)
    {
      switch (AppRenderingMode)
      {
        case EOpenGlAppRenderingMode.UsingFramebuffer:
          return GetCurrentViewportNormalizedRect(parPosX, parPosY, parWidth, parHeight);
        case EOpenGlAppRenderingMode.UsingGlobalRescale:
          return GetCurrentDisplayNormalizedRect(parPosX, parPosY, parWidth, parHeight);
        default:
          throw new ArgumentOutOfRangeException();
      }
    }

    /// <summary>
    /// (Устаревшее) Получить информацию о позиции объекта на дисплее, обновляемым с помощью фактора масштабирования
    /// </summary>
    /// <param name="parPosX">Позиция X</param>
    /// <param name="parPosY">Позиция Y</param>
    /// <param name="parWidth">Ширина</param>
    /// <param name="parHeight">Высота</param>
    /// <returns></returns>
    [Obsolete]
    private OpenGlScreenNormalizedRect GetCurrentDisplayNormalizedRect(double parPosX, double parPosY, double parWidth,
      double parHeight)
    {
      double x1 = Math.Max(DisplayViewportGlUnitsX1 +
                           OpenGlUtil.GetNormalizedWidthCoordValue(parPosX * GlobalRescale, WindowWidth),
        DisplayViewportGlUnitsX1);

      double x2 = Math.Min(x1 + OpenGlUtil.GetNormalizedWidthCoordValue(parWidth * GlobalRescale, WindowWidth),
        DisplayViewportGlUnitsX2);

      double y1 = Math.Min(DisplayViewportGlUnitsY1 -
                           OpenGlUtil.GetNormalizedHeightCoordValue(parPosY * GlobalRescale, WindowHeight),
        DisplayViewportGlUnitsY1);

      double y2 = Math.Max(y1 - OpenGlUtil.GetNormalizedHeightCoordValue(parHeight * GlobalRescale, WindowHeight),
        DisplayViewportGlUnitsY2);

      return new OpenGlScreenNormalizedRect()
      {
        X1 = x1,
        X2 = x2,
        Y1 = y1,
        Y2 = y2
      };
    }

    /// <summary>
    /// Получить информацию о позиции объекта на дисплее, обновляемым с помощью буфера кадров
    /// </summary>
    /// <param name="parPosX">Позиция X</param>
    /// <param name="parPosY">Позиция Y</param>
    /// <param name="parWidth">Ширина</param>
    /// <param name="parHeight">Высота</param>
    /// <returns></returns>
    private OpenGlScreenNormalizedRect GetCurrentViewportNormalizedRect(double parPosX, double parPosY, double parWidth,
      double parHeight)
    {
      double x1 = Math.Max(-1 + OpenGlUtil.GetNormalizedWidthCoordValue(parPosX, ViewportWidth), -1);
      double x2 = Math.Min(x1 + OpenGlUtil.GetNormalizedWidthCoordValue(parWidth, ViewportWidth), 1);
      double y1 = Math.Min(1 - OpenGlUtil.GetNormalizedHeightCoordValue(parPosY, ViewportHeight), 1);
      double y2 = Math.Max(y1 - OpenGlUtil.GetNormalizedHeightCoordValue(parHeight, ViewportHeight), -1);

      return new OpenGlScreenNormalizedRect()
      {
        X1 = x1,
        X2 = x2,
        Y1 = y1,
        Y2 = y2
      };
    }

    /// <summary>
    /// Фактор масштабирования дисплея
    /// </summary>
    public double GlobalRescale
    {
      get { return _globalRescale; }
      set { _globalRescale = value; }
    }

    /// <summary>
    /// Ширина окна OpenGL
    /// </summary>
    public double WindowWidth { get; private set; }

    /// <summary>
    /// Высота окна OpenGL
    /// </summary>
    public double WindowHeight { get; private set; }

    /// <summary>
    /// Ширина вида-дисплея
    /// </summary>
    public double ViewportWidth { get; private set; } = ViewBehaviourConsts.BASE_SURFACE_WIDTH;

    /// <summary>
    /// Высота вида-дисплея
    /// </summary>
    public double ViewportHeight { get; private set; } = ViewBehaviourConsts.BASE_SURFACE_HEIGHT;

    /// <summary>
    /// X1 координата дисплея приложения в системе координат OpenGL
    /// </summary>
    public double DisplayViewportGlUnitsX1 => _displayViewportGlUnitsX1;

    /// <summary>
    /// Y1 координата дисплея приложения в системе координат OpenGL
    /// </summary>
    public double DisplayViewportGlUnitsY1 => _displayViewportGlUnitsY1;

    /// <summary>
    /// Ширина дисплея в системе координат OpenGL
    /// </summary>
    public double DisplayViewportGlUnitsWidth { get; private set; }

    /// <summary>
    /// Высота дисплея в системе координат OpenGL
    /// </summary>
    public double DisplayViewportGlUnitsHeight { get; private set; }

    /// <summary>
    /// X2 координата дисплея приложения в системе координат OpenGL
    /// </summary>
    public double DisplayViewportGlUnitsX2 => _displayViewportGlUnitsX2;

    /// <summary>
    /// Y2 координата дисплея приложения в системе координат OpenGL
    /// </summary>
    public double DisplayViewportGlUnitsY2 => _displayViewportGlUnitsY2;

    /// <summary>
    /// Текущий режим рендеринга приложения
    /// </summary>
    public EOpenGlAppRenderingMode AppRenderingMode
    {
      get { return _appRenderingMode; }
      set
      {
        _appRenderingMode = value;
        RenderDisplayChanged?.Invoke();
      }
    }

    #region Singleton

    private static OpenGlWindowDisplay _instance;

    public static OpenGlWindowDisplay Instance
    {
      get
      {
        if (_instance == null)
        {
          _instance = new OpenGlWindowDisplay();
        }

        return _instance;
      }
    }

    private OpenGlWindowDisplay()
    {
    }

    #endregion
  }
}