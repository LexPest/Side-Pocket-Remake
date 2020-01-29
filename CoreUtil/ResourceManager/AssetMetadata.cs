namespace CoreUtil.ResourceManager
{
  /// <summary>
  /// Метаданные ресурса
  /// </summary>
  public class AssetMetadata
  {
    /// <summary>
    /// Основной тип ассета
    /// </summary>
    public EAssetType AssetType { get; private set; }

    /// <summary>
    /// Путь к файлу ассета
    /// </summary>
    public string FilePath { get; private set; }

    /// <summary>
    /// Стандартный конструктор
    /// </summary>
    /// <param name="parAssetType">Основной тип ассета</param>
    /// <param name="parFilePath">Путь к файлу ассета</param>
    public AssetMetadata( /*string nameId,*/ EAssetType parAssetType, string parFilePath)
    {
      //NameId = nameId;

      AssetType = parAssetType;
      FilePath = parFilePath;
    }
  }
}