using System;
using System.IO;
using CoreUtil.ResourceManager.AssetData.DataTypes;

namespace CoreUtil.ResourceManager.AssetData.Builders
{
  /// <summary>
  /// Реализация стандартного "строителя"-загрузчика игровых ресурсов
  /// </summary>
  public class AssetDataStandardBuilder : AssetDataAbstractBuilder
  {
    /// <summary>
    /// Загрузить и подготовить данные игрового ресурса
    /// </summary>
    /// <param name="parAssetMetadata">Метаданные ресурса</param>
    /// <typeparam name="T">Тип ресурса</typeparam>
    /// <returns>Обработчик данных игрового ресурса</returns>
    /// <exception cref="NotImplementedException">Неизвестный тип ресурса</exception>
    public override T LoadAssetData<T>(AssetMetadata parAssetMetadata)
    {
      switch (parAssetMetadata.AssetType)
      {
        case EAssetType.Binary:
          return new AssetDataBinary(parAssetMetadata, File.ReadAllBytes(parAssetMetadata.FilePath)) as T;
        case EAssetType.Text:
          return new AssetDataText(parAssetMetadata, File.ReadAllLines(parAssetMetadata.FilePath)) as T;
        default:
          throw new NotImplementedException("Loader not specified for that type of the asset!");
      }
    }
  }
}