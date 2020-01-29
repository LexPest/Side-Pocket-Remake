using System.Collections.Generic;
using CoreUtil.Pool;
using Model.SPCore.GameplayModelDefinition.ComponentModel;
using Model.SPCore.GameplayModelDefinition.GameComponents.Universal.DS;
using Model.SPCore.GameplayModelDefinition.ObjectModel;

namespace Model.SPCore.GameplayModelDefinition.GameComponents.Universal
{
  /// <summary>
  /// Вспомогательный компонент, ответственный за отслеживание ввода игрока и его корректную обработку
  /// </summary>
  public class KeyListenerComponent : Component
  {
    /// <summary>
    /// Конструктор для поддержки системы пулинга объектов
    /// </summary>
    /// <param name="parSupportData">Данные для работы с пулом</param>
    public KeyListenerComponent(ObjectPoolSupportData parSupportData) : base(parSupportData)
    {
    }

    /// <summary>
    /// Замена конструктора, процедура инициализации компонента
    /// </summary>
    /// <param name="parEntGameObject">Родительский игровой объект</param>
    public KeyListenerComponent Init(GameObject parEntGameObject)
    {
      base.Init(parEntGameObject, false, true);
      WatchData = new List<KeyListenerData>();
      return this;
    }

    /// <summary>
    /// Сигнал фиксированного обновления модели (используется преимущественно для обработки физики и ввода)
    /// </summary>
    /// <param name="parFixedDeltaTime">Время шага фиксированного обновления в секундах</param>
    public override void FixedUpdate(double parFixedDeltaTime)
    {
      base.FixedUpdate(parFixedDeltaTime);
      for (var index = 0; index < WatchData.Count; index++)
      {
        var keyListenerData = WatchData[index];
        if (ParentGameObject.LinkedAppModel.GetPlayersManager()
          .IsButtonPressed(keyListenerData.PlayerToCheck, keyListenerData.ButtonToCheck))
        {
          keyListenerData.PerformOnSuccess();

          if (WatchData == null)
          {
            return;
          }

          if (keyListenerData.AutoRemove)
          {
            WatchData.Remove(keyListenerData);
            index--;
          }
        }
      }
    }

    /// <summary>
    /// Данные об отслеживаемых игровых действиях и реакциях на них
    /// </summary>
    public List<KeyListenerData> WatchData { get; private set; }
  }
}