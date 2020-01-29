namespace ViewOpenTK.SPCore.ViewProvider
{
  /// <summary>
  /// Некоторые строки для рендеринга
  /// </summary>
  public static class ViewAppStrings
  {
    /// <summary>
    /// Строка о копирайте SEGA
    /// </summary>
    public static string SegaScreenNotice = "ORIGINALLY PUBLISHED ON SEGA MEGA DRIVE";
    
    /// <summary>
    /// Строка о копирайте DATA EAST
    /// </summary>
    public static string DataEastScreenNotice = "ORIGINALLY DEVELOPED BY DATA EAST";

    /// <summary>
    /// Строка о копирайте автора
    /// </summary>
    public static string PressStartScreenCopyright =
      $"{ViewBehaviourConsts.ESCAPE_CHARACTER_SEQUENCE_START}COPYRIGHT{ViewBehaviourConsts.ESCAPE_CHARACTER_SEQUENCE_END} 2019 ALEXEY MIHAILOV GR. 5413";
  }
}