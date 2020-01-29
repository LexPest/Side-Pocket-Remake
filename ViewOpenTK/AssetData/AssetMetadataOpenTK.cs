using CoreUtil.ResourceManager;

namespace ViewOpenTK.AssetData
{
  /// <summary>
  /// Метаданные об ассете OpenTK
  /// </summary>
  public class AssetMetadataOpenTk : AssetMetadata
  {
    /// <summary>
    /// Стандартный конструктор
    /// </summary>
    /// <param name="parAssetType">Базовый тип ассета</param>
    /// <param name="parFilePath">Путь к файлу ассета</param>
    /// <param name="parOpenTkAssetType">Тип ассета OpenTK</param>
    public AssetMetadataOpenTk(EAssetType parAssetType, string parFilePath, EOpenTkAssetType parOpenTkAssetType) : base(
      parAssetType, parFilePath)
    {
      OpenTkAssetType = parOpenTkAssetType;
    }

    /// <summary>
    /// Тип ассета OpenTK
    /// </summary>
    public EOpenTkAssetType OpenTkAssetType { get; private set; }
  }
}