using System.Collections.Generic;

namespace CoreUtil.ResourceManager.AssetData.AssetDatabaseUpdateStrategies
{
  /// <summary>
  /// Интерфейс стратегии обновления базы данных игровых ассетов
  /// </summary>
  public interface IAssetDatabaseUpdateStrategy
  {
    /// <summary>
    /// Метод для получения базы данных ассетов определенной директории
    /// </summary>
    /// <param name="parDatabaseDir">Корневая директория будущей базы</param>
    /// <returns></returns>
    Dictionary<string, AssetPack> GetAssetsDatabase(string parDatabaseDir);
  }
}