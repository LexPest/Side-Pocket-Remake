using System.Collections.Generic;
using System.IO;

namespace CoreUtil.ResourceManager.AssetData.AssetDatabaseUpdateStrategies
{
  /// <summary>
  /// Реализация стратегии обновления базы ресурсов и ассетов, обновляющая их по признаку расширения файлов
  /// </summary>
  public class AssetDatabaseUpdateDiffByExtensionStrategy : IAssetDatabaseUpdateStrategy
  {
    /// <summary>
    /// Стандартный конструктор
    /// </summary>
    /// <param name="parExtensionToActualAssetType">Словарь соответствия расширения файла и типа ассета</param>
    public AssetDatabaseUpdateDiffByExtensionStrategy(Dictionary<string, EAssetType> parExtensionToActualAssetType)
    {
      ExtensionToActualAssetType = parExtensionToActualAssetType;
    }

    /// <summary>
    /// Текущий словарь соответствия расширения файла и типа ассета
    /// </summary>
    public Dictionary<string, EAssetType> ExtensionToActualAssetType { get; private set; }

    /// <summary>
    /// Составить базу данных ассетов и ресурсов из директории
    /// </summary>
    /// <param name="parDatabaseDir">Целевая директория для анализа</param>
    /// <returns>База данных ассетов</returns>
    public virtual Dictionary<string, AssetPack> GetAssetsDatabase(string parDatabaseDir)
    {
      Dictionary<string, AssetPack> retAssetDatabase = new Dictionary<string, AssetPack>();

      //load assets database
      string fullPathToInitializationDir = Path.GetFullPath(parDatabaseDir);

      foreach (var directory in Directory.GetDirectories(fullPathToInitializationDir))
      {
        Dictionary<string, AssetMetadata> availableAssetsInPack = new Dictionary<string, AssetMetadata>();

        List<string> filePaths = DirSearch(directory);

        foreach (var filePath in filePaths)
        {
          //save in database only supported assets
          string fileExtension = Path.GetExtension(filePath);

          AddAssetMetadataToPack(filePath, directory, fileExtension, availableAssetsInPack);
        }

        retAssetDatabase.Add(directory.Replace(fullPathToInitializationDir, "").Replace('\\', '/'),
          new AssetPack(availableAssetsInPack));
      }

      return retAssetDatabase;
    }

    /// <summary>
    /// Рекурсивный алгоритм поиска файла по вложенным директориям
    /// </summary>
    /// <param name="parDirPath">Путь к директории поиска</param>
    /// <returns>Список найденных файлов</returns>
    protected List<string> DirSearch(string parDirPath)
    {
      List<string> files = new List<string>();
      foreach (string file in Directory.GetFiles(parDirPath))
      {
        files.Add(Path.GetFullPath(file));
      }

      foreach (string subDirectory in Directory.GetDirectories($"{parDirPath}"))
      {
        files.AddRange(DirSearch(subDirectory));
      }

      return files;
    }

    /// <summary>
    /// Вспомогательный метод для добавления информации об ассете к пакету ресурсов
    /// </summary>
    /// <param name="parFilePath">Путь к файлу ассета</param>
    /// <param name="parDirectory">Директория ассета</param>
    /// <param name="parFileExtension">Расширение файла</param>
    /// <param name="parAvailableAssetsInPack">Текущая база данных ассетов</param>
    protected virtual void AddAssetMetadataToPack(string parFilePath, string parDirectory, string parFileExtension,
      Dictionary<string, AssetMetadata> parAvailableAssetsInPack)
    {
      if (ExtensionToActualAssetType.TryGetValue(parFileExtension, out EAssetType assetType))
      {
        parAvailableAssetsInPack.Add(parFilePath.Replace(parDirectory, "").Replace('\\', '/'),
          new AssetMetadata(assetType, parFilePath));
      }
    }
  }
}