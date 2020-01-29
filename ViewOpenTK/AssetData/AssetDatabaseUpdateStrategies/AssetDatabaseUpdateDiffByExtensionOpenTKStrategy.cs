using System.Collections.Generic;
using CoreUtil.ResourceManager;
using CoreUtil.ResourceManager.AssetData.AssetDatabaseUpdateStrategies;

namespace ViewOpenTK.AssetData.AssetDatabaseUpdateStrategies
{
  /// <summary>
  /// Класс реализует стратегию обновления базы данных ресурсов приложения, базирующейся
  /// на разделении на типы ассетов по расширению файлов
  /// </summary>
  public class AssetDatabaseUpdateDiffByExtensionOpenTkStrategy : AssetDatabaseUpdateDiffByExtensionStrategy
  {
    /// <summary>
    /// Стандартный конструктор
    /// </summary>
    /// <param name="parExtensionToActualAssetType">Словарь соответствия расширение-основной ассета приложения</param>
    /// <param name="parAssetExtensionToActualOpenTkAssetType">Словарь соответствия расширение-тип ассета OpenTK</param>
    public AssetDatabaseUpdateDiffByExtensionOpenTkStrategy(
      Dictionary<string, EAssetType> parExtensionToActualAssetType,
      Dictionary<string, EOpenTkAssetType> parAssetExtensionToActualOpenTkAssetType) : base(
      parExtensionToActualAssetType)
    {
      AssetExtensionToActualOpenTkAssetType = parAssetExtensionToActualOpenTkAssetType;
    }

    /// <summary>
    /// Переопределенная процедура добавления метаданных ресурса в пакет ресурсов 
    /// </summary>
    /// <param name="parFilePath">Путь к файлу ресурса</param>
    /// <param name="parDirectory">Директория ресурса</param>
    /// <param name="parFileExtension">Расширение файла</param>
    /// <param name="parAvailableAssetsInPack">Ссылка на словарь текущих доступных пакетов ресурсов для заполнения</param>
    protected override void AddAssetMetadataToPack(string parFilePath, string parDirectory, string parFileExtension,
      Dictionary<string, AssetMetadata> parAvailableAssetsInPack)
    {
      if (AssetExtensionToActualOpenTkAssetType.TryGetValue(parFileExtension, out EOpenTkAssetType openTkAssetType))
      {
        EAssetType targetAssetType = EAssetType.Binary;
        if (openTkAssetType == EOpenTkAssetType.TextMetadata)
        {
          targetAssetType = EAssetType.Text;
        }

        parAvailableAssetsInPack.Add(parFilePath.Replace(parDirectory, "").Replace("\\", "/"),
          new AssetMetadataOpenTk(targetAssetType, parFilePath, openTkAssetType));
      }
      else
      {
        base.AddAssetMetadataToPack(parFilePath, parDirectory, parFileExtension, parAvailableAssetsInPack);
      }
    }

    /// <summary>
    /// Текущий установленный словарь соответствия расширение-тип ассета OpenTK
    /// </summary>
    public Dictionary<string, EOpenTkAssetType> AssetExtensionToActualOpenTkAssetType { get; private set; }
  }
}