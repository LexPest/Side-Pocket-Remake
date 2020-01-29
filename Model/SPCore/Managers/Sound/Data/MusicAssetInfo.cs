namespace Model.SPCore.Managers.Sound.Data
{
  /// <summary>
  /// Структура данных информации о музыкальном ассете
  /// </summary>
  public struct MusicAssetInfo
  {
    /// <summary>
    /// Имя пакета ресурсов
    /// </summary>
    public string AssetPackName { get; private set; }
    
    /// <summary>
    /// Имя ассета
    /// </summary>
    public string AssetName { get; private set; }

    /// <summary>
    /// Стандартный конструктор
    /// </summary>
    /// <param name="parAssetPackName">Имя пакета ресурсов</param>
    /// <param name="parAssetName">Имя ассета</param>
    public MusicAssetInfo(string parAssetPackName, string parAssetName)
    {
      AssetPackName = parAssetPackName;
      AssetName = parAssetName;
    }
  }
}