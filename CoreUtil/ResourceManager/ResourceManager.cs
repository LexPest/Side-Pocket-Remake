using System;
using System.Collections.Generic;
using System.Linq;
using CoreUtil.ResourceManager.AssetData.AssetDatabaseUpdateStrategies;
using CoreUtil.ResourceManager.AssetData.Builders;
using CoreUtil.ResourceManager.AssetData.DataTypes;

namespace CoreUtil.ResourceManager
{
  /// <summary>
  /// Менеджер ресурсов
  /// </summary>
  public sealed class ResourceManager
  {
    /// <summary>
    /// Стандартный словарь сопоставления расширения файла и типа ассета
    /// </summary>
    public static readonly Dictionary<string, EAssetType> StandardExtensionToAssetType =
      new Dictionary<string, EAssetType>()
      {
        {".txt", EAssetType.Text},
        {".bin", EAssetType.Binary}
      };

    /// <summary>
    /// Стандартный конструктор
    /// </summary>
    /// <param name="parAssetDatabaseDir">Директория для составления начальной базы данных ассетов</param>
    /// <param name="parInitialAssetBuilder">"Строитель"-загрузчик ресурсов</param>
    /// <param name="parAssetDatabaseUpdateStrategy">Стратегия составления начальной базы данных ассетов</param>
    public ResourceManager(string parAssetDatabaseDir, AssetDataAbstractBuilder parInitialAssetBuilder = null,
      IAssetDatabaseUpdateStrategy parAssetDatabaseUpdateStrategy = null)
    {
      AssetDatabaseDir = parAssetDatabaseDir;
      AvailableAssetPacks = new Dictionary<string, AssetPack>();
      LoadedAssetPacks = new Dictionary<string, AssetPackLoadedData>();
      StandardAssetBuilder = parInitialAssetBuilder ?? new AssetDataStandardBuilder();
      StandardAssetDatabaseUpdateStrategy = parAssetDatabaseUpdateStrategy ??
                                            new AssetDatabaseUpdateDiffByExtensionStrategy(
                                              StandardExtensionToAssetType);

      UpdateAssetsAvailableDatabase(StandardAssetDatabaseUpdateStrategy);
    }

    /// <summary>
    /// Обновление базы данных доступных ассетов
    /// </summary>
    /// <param name="parAssetDatabaseUpdateStrategy"></param>
    public void UpdateAssetsAvailableDatabase(IAssetDatabaseUpdateStrategy parAssetDatabaseUpdateStrategy)
    {
      AvailableAssetPacks = parAssetDatabaseUpdateStrategy.GetAssetsDatabase(AssetDatabaseDir);
    }

    /// <summary>
    /// Загружает пакет ресурсов в память
    /// </summary>
    /// <param name="parAssetPackName">Имя пакета ресурсов</param>
    /// <param name="parCustomAssetBuilder">Определенный строитель-загрузчик</param>
    /// <exception cref="ArgumentException">Пакет ресурсов не был найден</exception>
    public void LoadAssetPack(string parAssetPackName, AssetDataAbstractBuilder parCustomAssetBuilder = null)
    {
      if (AvailableAssetPacks.TryGetValue(parAssetPackName, out AssetPack assetPack))
      {
        LoadAssetPack(assetPack, parCustomAssetBuilder, parAssetPackName);
      }
      else
      {
        throw new ArgumentException("Asset pack with the specified name was not found in database");
      }
    }

    /// <summary>
    /// Загружает пакет ресурсов в память
    /// </summary>
    /// <param name="parAssetPack">Пакет ресурсов</param>
    /// <param name="parCustomAssetBuilder">Определенный строитель-загрузчик</param>
    /// <param name="parAssetPackName">Имя пакета ресурсов</param>
    public void LoadAssetPack(AssetPack parAssetPack, AssetDataAbstractBuilder parCustomAssetBuilder = null,
      string parAssetPackName = null)
    {
      AssetDataAbstractBuilder actualAssetDataBuilder = parCustomAssetBuilder ?? StandardAssetBuilder;

      //проверим, если этот пакет уже загружен
      parAssetPackName = parAssetPackName ?? AvailableAssetPacks.FirstOrDefault(parX => parX.Value == parAssetPack).Key;
      if (LoadedAssetPacks.ContainsKey(parAssetPackName))
      {
        //выгрузим его сначала
        UnloadAssetPack(LoadedAssetPacks[parAssetPackName]);
        Console.WriteLine("Unloading asset pack because it has been already loaded in memory");
      }

      //а затем снова загрузим
      Dictionary<string, AssetDataParent> assetPackData = new Dictionary<string, AssetDataParent>();

      foreach (var contentData in parAssetPack.Content)
      {
        assetPackData.Add(contentData.Key, actualAssetDataBuilder.LoadAssetData<AssetDataParent>(contentData.Value));
        Console.WriteLine($"Loaded data {contentData.Key} in asset pack {parAssetPackName}");
      }

      LoadedAssetPacks.Add(parAssetPackName, new AssetPackLoadedData(assetPackData));
    }

    /// <summary>
    /// Выгрузить пакет ресурсов из памяти
    /// </summary>
    /// <param name="parAssetPackName">Название пакета ресурсов</param>
    /// <exception cref="ArgumentException">Пакет ресурсов не был загружен</exception>
    public void UnloadAssetPack(string parAssetPackName)
    {
      if (LoadedAssetPacks.TryGetValue(parAssetPackName, out AssetPackLoadedData assetPack))
      {
        UnloadAssetPack(assetPack, parAssetPackName);
      }
      else
      {
        throw new ArgumentException("Asset pack with the specified name is not loaded");
      }
    }

    /// <summary>
    /// Выгрузить пакет ресурсов из памяти
    /// </summary>
    /// <param name="parAssetPackLoadedData">Данные о загруженном в память пакете ресурсов</param>
    /// <param name="parAssetPackName">Название пакета ресурсов</param>
    public void UnloadAssetPack(AssetPackLoadedData parAssetPackLoadedData, string parAssetPackName = null)
    {
      parAssetPackName = parAssetPackName ??
                         LoadedAssetPacks.FirstOrDefault(parX => parX.Value == parAssetPackLoadedData).Key;

      parAssetPackLoadedData.Dispose();

      LoadedAssetPacks.Remove(parAssetPackName);

      Console.WriteLine($"Asset pack {parAssetPackName} was unloaded");
    }

    /// <summary>
    /// Получить данные ресурса
    /// </summary>
    /// <param name="parAssetPackName">Название пакета ресурсов</param>
    /// <param name="parAssetName">Название нужного ресурса</param>
    /// <typeparam name="T">Желаемый преобразованный тип ресурса</typeparam>
    /// <returns>Данные ресурса</returns>
    /// <exception cref="ArgumentException">Ресурс не может быть найден в базе загруженных ресурсов</exception>
    public T GetAssetData<T>(string parAssetPackName, string parAssetName) where T : AssetDataParent
    {
      if (LoadedAssetPacks.TryGetValue(parAssetPackName, out var assetPack))
      {
        if (assetPack.Content.TryGetValue(parAssetName, out var targetAsset))
        {
          return targetAsset as T;
        }
        else
        {
          throw new ArgumentException(
            $"Cannot find asset {parAssetName} in the asset pack in database: {parAssetPackName}");
        }
      }
      else
      {
        throw new ArgumentException($"Cannot find asset pack in database: {parAssetPackName}");
      }
    }

    /// <summary>
    /// Получить данные ресурса
    /// </summary>
    /// <param name="parAssetMetadata">Метаданные ассета</param>
    /// <typeparam name="T">Желаемый преобразованный тип ресурса</typeparam>
    /// <returns>Данные ресурса</returns>
    /// <exception cref="ArgumentException">Ресурс не может быть найден в базе загруженных ресурсов</exception>
    public T GetAssetData<T>(AssetMetadata parAssetMetadata) where T : AssetDataParent
    {
      foreach (var loadedAssetPack in LoadedAssetPacks)
      {
        AssetDataParent possibleResult = loadedAssetPack.Value.Content
          .FirstOrDefault(parX => parX.Value.ActualAssetMetadata == parAssetMetadata).Value;
        if (possibleResult != null)
        {
          return possibleResult as T;
        }
      }

      throw new ArgumentException($"This asset is not loaded");
    }

    /// <summary>
    /// Получить метаданные об ассете
    /// </summary>
    /// <param name="parAssetPackName">Имя пакета ресурсов нужного ассета</param>
    /// <param name="parAssetName">Имя нужного ассета</param>
    /// <returns></returns>
    public AssetMetadata GetAssetMetadata(string parAssetPackName, string parAssetName)
    {
      if (AvailableAssetPacks.TryGetValue(parAssetPackName, out var assetPack))
      {
        if (assetPack.Content.TryGetValue(parAssetName, out var targetAsset))
        {
          return targetAsset;
        }
      }

      //не найден
      return null;
    }

    /// <summary>
    /// Получить данные о ресурсе
    /// </summary>
    /// <param name="parAssetMetadata">Метаданные об ассете</param>
    /// <param name="outAssetPack">Название пакета ресурсов</param>
    /// <param name="outAssetName">Название ресурса</param>
    public void GetAssetInfo(AssetMetadata parAssetMetadata, out string outAssetPack, out string outAssetName)
    {
      outAssetPack = null;
      outAssetName = null;
      foreach (var availableAssetPack in AvailableAssetPacks)
      {
        outAssetName = availableAssetPack.Value.Content
          .FirstOrDefault(parX => parX.Value == parAssetMetadata).Key;
        if (outAssetName != null)
        {
          outAssetPack = availableAssetPack.Key;
          return;
        }
      }
    }

    /// <summary>
    /// Возвращает загруженные данные ресурсов пакета, загружает пакет если необходимо
    /// </summary>
    /// <param name="parAssetPackKey">Имя пакета ресурсов</param>
    /// <returns></returns>
    public Dictionary<string, AssetDataParent> GetLoadedAssetPackContent(string parAssetPackKey)
    {
      if (!LoadedAssetPacks.ContainsKey(parAssetPackKey))
      {
        LoadAssetPack(parAssetPackKey);
      }

      return LoadedAssetPacks[parAssetPackKey].Content;
    }

    /// <summary>
    /// Доступные для загрузки пакеты ресурсов
    /// </summary>
    private Dictionary<string, AssetPack> AvailableAssetPacks { get; set; }

    /// <summary>
    /// Загруженные в память пакеты ресурсов
    /// </summary>
    private Dictionary<string, AssetPackLoadedData> LoadedAssetPacks { get; set; }

    /// <summary>
    /// Строитель-загрузчик ресурсов по умолчанию
    /// </summary>
    private AssetDataAbstractBuilder StandardAssetBuilder { get; set; }

    /// <summary>
    /// Стратегия обновления базы данных ресурсов по умолчанию
    /// </summary>
    private IAssetDatabaseUpdateStrategy StandardAssetDatabaseUpdateStrategy { get; set; }

    /// <summary>
    /// Корневая директория ресурсов игры по умолчанию
    /// </summary>
    private string AssetDatabaseDir { get; set; }
  }
}