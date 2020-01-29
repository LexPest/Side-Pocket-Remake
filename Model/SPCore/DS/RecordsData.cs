using System;
using System.Collections.Generic;
using System.Linq;

namespace Model.SPCore.DS
{
  /// <summary>
  /// Данные о рекордах
  /// </summary>
  [Serializable]
  public sealed class RecordsData
  {
    /// <summary>
    /// Максимум позиций игроков в рекордах
    /// </summary>
    public const int MAXIMUM_PLAYERS_INFOS = 3;

    /// <summary>
    /// Текущая информация о рекордах
    /// </summary>
    private List<RecordPlayerInfo> _playerRecordsInfo;

    /// <summary>
    /// Стандартный конструктор без параметров
    /// </summary>
    public RecordsData()
    {
    }

    /// <summary>
    /// Установить стандартные значения
    /// </summary>
    public void SetDefaultValues()
    {
      PlayerRecordsInfo = new List<RecordPlayerInfo>();
      TryAddRecord(new RecordPlayerInfo(0, "Mike")); //Новичок
      TryAddRecord(new RecordPlayerInfo(1500, "Nikolas")); //Мастер
      TryAddRecord(new RecordPlayerInfo(3000, "Lisette")); //Про
    }

    /// <summary>
    /// Получить информацию о рекордах по умолчанию
    /// </summary>
    /// <returns></returns>
    public static RecordsData GetStandardRecords()
    {
      RecordsData retRecords = new RecordsData();
      retRecords.SetDefaultValues();
      return retRecords;
    }

    /// <summary>
    /// Вспомогательный метод для актуализации информации о рекордах и обновлении данных
    /// </summary>
    /// <param name="parNoSort">Флаг отсутствия необходимости сортировки</param>
    private void ActualizeRecordsInfo(bool parNoSort = false)
    {
      if (PlayerRecordsInfo == null)
      {
        PlayerRecordsInfo = new List<RecordPlayerInfo>();
      }
      else
      {
        if (!parNoSort)
        {
          PlayerRecordsInfo = PlayerRecordsInfo.OrderByDescending(parInfo => parInfo.PointsEarned).ToList();
        }

        while (PlayerRecordsInfo.Count > MAXIMUM_PLAYERS_INFOS)
        {
          PlayerRecordsInfo.RemoveAt(MAXIMUM_PLAYERS_INFOS);
        }
      }
    }

    /// <summary>
    /// Проверить и применить ограничения имени для всех игроков
    /// </summary>
    public void CheckAndApplyConstraints()
    {
      foreach (var recordPlayerInfo in PlayerRecordsInfo)
      {
        recordPlayerInfo.NameConstraintsApply();
      }
    }

    /// <summary>
    /// Вспомогательный метод для получения индекса вставки в динамический массив информации о рекорде
    /// </summary>
    /// <param name="parPointsEarned">Количество заработанных очков</param>
    /// <returns></returns>
    private int GetInsertIndex(long parPointsEarned)
    {
      int i = 0;
      for (i = 0; i < PlayerRecordsInfo.Count; i++)
      {
        if (PlayerRecordsInfo[i].PointsEarned < parPointsEarned)
        {
          return i;
        }
      }

      return i;
    }

    /// <summary>
    /// Попробовать осуществить добавление информации о рекорде
    /// </summary>
    /// <param name="parPlayerInfo">Информация о рекорде игрока</param>
    /// <returns>True если добавление было произведено успешно</returns>
    public bool TryAddRecord(RecordPlayerInfo parPlayerInfo)
    {
      ActualizeRecordsInfo();
      int insertIndex = GetInsertIndex(parPlayerInfo.PointsEarned);
      if (insertIndex >= MAXIMUM_PLAYERS_INFOS)
      {
        return false;
      }
      else
      {
        if (insertIndex > PlayerRecordsInfo.Count)
        {
          PlayerRecordsInfo.Add(parPlayerInfo);
        }
        else
        {
          PlayerRecordsInfo.Insert(insertIndex, parPlayerInfo);
          ActualizeRecordsInfo(true);
        }

        return true;
      }
    }

    /// <summary>
    /// Текущая информация о рекордах
    /// </summary>
    public List<RecordPlayerInfo> PlayerRecordsInfo
    {
      get { return _playerRecordsInfo; }
      set { _playerRecordsInfo = value; }
    }
  }
}