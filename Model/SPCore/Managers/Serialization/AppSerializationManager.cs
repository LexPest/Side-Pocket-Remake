using System;
using System.IO;
using Newtonsoft.Json;

namespace Model.SPCore.Managers.Serialization
{
  /// <summary>
  /// Вспомогательный класс, который содержит методы для сериализации и десериализации данных
  /// </summary>
  public static class AppSerializationManager
  {
    /// <summary>
    /// Загрузить данные из файла
    /// </summary>
    /// <param name="parPath">Путь к файлу</param>
    /// <typeparam name="T">Тип десериализуемых данных</typeparam>
    /// <returns>Данные запрошенного типа в случае успешной загрузки</returns>
    /// <exception cref="ArgumentException">Путь некорректен или файл не существует</exception>
    /// <exception cref="ApplicationException">Произошла ошибка во время загрузки файла</exception>
    public static T LoadDataFromFile<T>(string parPath)
    {
      if (parPath == null)
      {
        throw new ArgumentException("Path cannot be null!");
      }

      if (parPath == String.Empty)
      {
        throw new ArgumentException("Path cannot be empty!");
      }

      if (!File.Exists(parPath))
      {
        throw new ArgumentException("File not exists!");
      }

      try
      {
        string deserializedSettingsText = File.ReadAllText(parPath);
        return JsonConvert.DeserializeObject<T>(deserializedSettingsText);
      }
      catch (Exception e)
      {
        throw new ApplicationException("File cannot be loaded properly!");
      }
    }

    /// <summary>
    /// Безопасно загрузить данные из файла
    /// </summary>
    /// <param name="parPath">Путь к файлу</param>
    /// <param name="outResult">Выходной результат</param>
    /// <typeparam name="T">Тип десериализуемых данных</typeparam>
    /// <returns>True, если загрузка была успешна</returns>
    public static bool LoadDataFromFileSafe<T>(string parPath, out T outResult)
    {
      outResult = default(T);
      try
      {
        outResult = LoadDataFromFile<T>(parPath);
        return true;
      }
      catch (Exception e)
      {
        return false;
      }
    }


    /// <summary>
    /// Сохраняет/сериализует данные в файл
    /// </summary>
    /// <param name="parSerializationData">Данные для сохранения/сериализации</param>
    /// <param name="parPath">Путь для сохранения файла</param>
    public static void SaveDataToFile<T>(T parSerializationData, string parPath)
    {
      if (!Directory.Exists(Path.Combine(parPath, "../")))
      {
        Directory.CreateDirectory(Path.Combine(parPath, "../"));
      }

      string serializedSettings =
        JsonConvert.SerializeObject(parSerializationData, typeof(T), new JsonSerializerSettings() { });
      File.WriteAllText(parPath, serializedSettings);
      Console.WriteLine($"Serialized data!");
    }
  }
}