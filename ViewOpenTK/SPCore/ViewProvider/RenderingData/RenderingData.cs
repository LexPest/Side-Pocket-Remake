using System.Drawing;
using CoreUtil.Math;
using ViewOpenTK.OpenGL;

namespace ViewOpenTK.SPCore.ViewProvider.RenderingData
{
  /// <summary>
  /// Структура, хранящая данные о рендеринге для OpenGL
  /// </summary>
  public struct RenderingData
  {
    /// <summary>
    /// Задания для рендеринга OpenGL
    /// </summary>
    public RenderingTask[] RenderingTasks;

    /// <summary>
    /// Параметр глубины (меньше - ближе)
    /// </summary>
    public double Depth { get; private set; }

    /// <summary>
    /// Конструктор данных о рендеринге для примитива
    /// </summary>
    /// <param name="parRectX">Координата X</param>
    /// <param name="parRectY">Координата Y</param>
    /// <param name="parRectWidth">Ширина</param>
    /// <param name="parRectHeight">Высота</param>
    /// <param name="parColor">Цвет</param>
    /// <param name="parDepth">Глубина</param>
    /// <param name="parRotationDegreesAngle">Поворот в градусах</param>
    /// <param name="parPivotX">Точка опоры для поворота, координата X</param>
    /// <param name="parPivotY">Точка опоры для поворота, координата Y</param>
    public RenderingData(double parRectX, double parRectY, double parRectWidth, double parRectHeight, Color parColor,
      double parDepth, double parRotationDegreesAngle = 0, double parPivotX = 0, double parPivotY = 0)
    {
      Depth = parDepth;

      OpenGlScreenNormalizedRect normalizedRectData =
        OpenGlWindowDisplay.Instance.GetCurrentRenderingModeNormalizedRect(parRectX, parRectY,
          parRectWidth, parRectHeight);

      OpenGlScreenNormalizedRect normalizedPivotData =
        OpenGlWindowDisplay.Instance.GetCurrentRenderingModeNormalizedRect(parPivotX, parPivotY, 1, 1);

      RenderingTask task = new RenderingTask();

      task.Vertices = new SpVector3[]
      {
        new SpVector3(normalizedRectData.X1, normalizedRectData.Y1),
        new SpVector3(normalizedRectData.X2, normalizedRectData.Y1),
        new SpVector3(normalizedRectData.X2, normalizedRectData.Y2),
        new SpVector3(normalizedRectData.X1, normalizedRectData.Y2),
      };

      Angle.RotateAroundPivot(parRotationDegreesAngle, ref task.Vertices,
        new SpVector3(normalizedPivotData.X1, normalizedPivotData.Y1));

      task.RenderingDataType = ERenderingTaskType.Primitive;
      
      task.GlTexture = 0;
      task.BlendColor = parColor;
      task.TexVertices = null;
      task.RenderingDataType = ERenderingTaskType.Primitive;

      RenderingTasks = new[] {task};
    }

    /// <summary>
    /// Конструктор данных о рендеринге для спрайта
    /// </summary>
    /// <param name="parRenderingSprite">Спрайт</param>
    public RenderingData(RenderingSprite parRenderingSprite)
    {
      Depth = parRenderingSprite.Depth;

      OpenGlScreenNormalizedRect normalizedRectData =
        OpenGlWindowDisplay.Instance.GetCurrentRenderingModeNormalizedRect(parRenderingSprite.X,
          parRenderingSprite.Y,
          parRenderingSprite.Sprite.Width * parRenderingSprite.ScaleX,
          parRenderingSprite.Sprite.Height * parRenderingSprite.ScaleY);

      RenderingTask task = new RenderingTask();

      task.Vertices = new SpVector3[]
      {
        new SpVector3(normalizedRectData.X1, normalizedRectData.Y1),
        new SpVector3(normalizedRectData.X2, normalizedRectData.Y1),
        new SpVector3(normalizedRectData.X2, normalizedRectData.Y2),
        new SpVector3(normalizedRectData.X1, normalizedRectData.Y2),
      };

      OpenGlScreenNormalizedRect normalizedPivotData =
        OpenGlWindowDisplay.Instance.GetCurrentRenderingModeNormalizedRect(parRenderingSprite.PivotX,
          parRenderingSprite.PivotY, 1, 1);

      Angle.RotateAroundPivot(parRenderingSprite.RotationDegrees, ref task.Vertices,
        new SpVector3(normalizedPivotData.X1, normalizedPivotData.Y1));


      task.BlendColor = parRenderingSprite.BlendColor;
      task.GlTexture = parRenderingSprite.Sprite.LinkedAssetDataTexture.GlTextureId;
      task.RenderingDataType = ERenderingTaskType.Sprite;
      task.TexVertices = new SpVector3[]
      {
        new SpVector3(parRenderingSprite.Sprite.OpenTkTextureRect.X1, parRenderingSprite.Sprite.OpenTkTextureRect.Y1),
        new SpVector3(parRenderingSprite.Sprite.OpenTkTextureRect.X2, parRenderingSprite.Sprite.OpenTkTextureRect.Y1),
        new SpVector3(parRenderingSprite.Sprite.OpenTkTextureRect.X2, parRenderingSprite.Sprite.OpenTkTextureRect.Y2),
        new SpVector3(parRenderingSprite.Sprite.OpenTkTextureRect.X1, parRenderingSprite.Sprite.OpenTkTextureRect.Y2)
      };
      RenderingTasks = new[] {task};
    }

    /// <summary>
    /// Конструктор данных о рендеринге для текстовой строки
    /// </summary>
    /// <param name="parRenderingString">Данные о строке для рендеринга</param>
    public RenderingData(RenderingString parRenderingString)
    {
      Depth = parRenderingString.Depth;

      RenderingTasks = new RenderingTask[parRenderingString.SpritesToRender.Count];

      int index = 0;
      foreach (var renderingSprite in parRenderingString.SpritesToRender)
      {
        RenderingData renderingDataForSprite = new RenderingData(renderingSprite);
        RenderingTasks[index] = renderingDataForSprite.RenderingTasks[0];
        index++;
      }
    }

    /// <summary>
    /// Конструктор данных о рендеринге для заполняющего экран одноцветного примитива
    /// </summary>
    /// <param name="parRenderingFillColorScreen">Заполняющий экран одноцветный примитив</param>
    public RenderingData(RenderingFillColorScreen parRenderingFillColorScreen)
    {
      Depth = parRenderingFillColorScreen.Depth;
      RenderingTasks = new RenderingTask[1];
      RenderingTasks[0] = (new RenderingData(0, 0, OpenGlWindowDisplay.Instance.ViewportWidth,
          OpenGlWindowDisplay.Instance.ViewportHeight, parRenderingFillColorScreen.Color, Depth))
        .RenderingTasks[0];
    }
  }
}