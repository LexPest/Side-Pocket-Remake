using System;
using CoreUtil.ResourceManager;
using CoreUtil.ResourceManager.AssetData.Builders;
using CoreUtil.ResourceManager.AssetData.DataTypes;
using ViewOpenTK.AssetData.DataTypes;

namespace ViewOpenTK.AssetData.Builders
{
  /// <summary>
  /// Класс для реализации шаблона "строитель" для обработки ассетов OpenTK
  /// </summary>
  public class AssetDataOpenTkBuilder : AssetDataStandardBuilder
  {
    /// <summary>
    /// Переопределение процесса загрузки данных ресурсов для ассетов OpenTK
    /// </summary>
    /// <param name="parAssetMetadata">Метаданные загружаемого ассета</param>
    /// <typeparam name="T">Приведенный тип результирующего объекта загруженного ресурса</typeparam>
    /// <returns></returns>
    /// <exception cref="NotImplementedException">Тип ассета не поддерживается этим загрузчиком</exception>
    public override T LoadAssetData<T>(AssetMetadata parAssetMetadata)
    {
      if (parAssetMetadata is AssetMetadataOpenTk assetMetadataOpenTk)
      {
        switch (assetMetadataOpenTk.OpenTkAssetType)
        {
          case EOpenTkAssetType.NotSpecial:
            return base.LoadAssetData<T>(assetMetadataOpenTk);
          case EOpenTkAssetType.Texture:
            return new AssetDataOpenTkTexture(assetMetadataOpenTk,
              base.LoadAssetData<AssetDataBinary>(assetMetadataOpenTk).BinaryData) as T;
          case EOpenTkAssetType.WaveSound:
            return new AssetDataOpenTkWaveSound(assetMetadataOpenTk,
              base.LoadAssetData<AssetDataBinary>(assetMetadataOpenTk).BinaryData) as T;
          case EOpenTkAssetType.TextMetadata:
            return base.LoadAssetData<AssetDataText>(assetMetadataOpenTk) as T;
          default:
            throw new NotImplementedException("Loader not specified for that type of the asset!");
        }
      }
      else
      {
        return base.LoadAssetData<T>(parAssetMetadata);
      }
    }
  }
}