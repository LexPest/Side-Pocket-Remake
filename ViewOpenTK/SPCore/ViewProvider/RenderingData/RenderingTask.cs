using System.Drawing;
using CoreUtil.Math;

namespace ViewOpenTK.SPCore.ViewProvider.RenderingData
{
  /// <summary>
  /// Структура, хранящая информацию задачи рендеринга OpenGL
  /// </summary>
  public struct RenderingTask
  {
    /// <summary>
    /// Тип задачи для рендеринга
    /// </summary>
    public ERenderingTaskType RenderingDataType;

    /// <summary>
    /// Вершины
    /// </summary>
    public SpVector3[] Vertices;
    /// <summary>
    /// Текстурные вершины
    /// </summary>
    public SpVector3[] TexVertices;

    /// <summary>
    /// Цвет
    /// </summary>
    public Color BlendColor;

    /// <summary>
    /// Связанный идентификатор текстуры OpenGL
    /// </summary>
    public int GlTexture;
  }
}