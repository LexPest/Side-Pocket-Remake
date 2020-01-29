namespace ViewOpenTK.OpenGL
{
  /// <summary>
  /// Режим рендеринга приложения OpenGL
  /// </summary>
  public enum EOpenGlAppRenderingMode
  {
    /// <summary>
    /// Используя кадровый буфер
    /// </summary>
    UsingFramebuffer,
    /// <summary>
    /// Используя технику фактора масштабирования
    /// </summary>
    UsingGlobalRescale
  }
}