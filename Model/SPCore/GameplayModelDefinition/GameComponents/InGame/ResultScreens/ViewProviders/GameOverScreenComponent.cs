using System;
using CoreUtil.Pool;
using Model.SPCore.GameplayModelDefinition.GameComponents.ViewProviderBase;
using Model.SPCore.GameplayModelDefinition.ObjectModel;
using Model.SPCore.Managers.Sound.Data;

namespace Model.SPCore.GameplayModelDefinition.GameComponents.InGame.ResultScreens.ViewProviders
{
  /// <summary>
  /// Экран результата игры - проигрыш
  /// </summary>
  public class GameOverScreenComponent : ViewProviderComponent
  {
    /// <summary>
    /// Конструктор для поддержки системы пулинга объектов
    /// </summary>
    /// <param name="parSupportData">Данные для работы с пулом</param>
    public GameOverScreenComponent(ObjectPoolSupportData parSupportData) : base(parSupportData)
    {
    }
    /// <summary>
    /// Событие завершения работы экрана результатов
    /// </summary>
    public event Action OnScreenConfirmed;

    /// <summary>
    /// Замена конструктора, процедура инициализации компонента
    /// </summary>
    /// <param name="parEntGameObject">Родительский игровой объект</param>
    public GameOverScreenComponent Init(GameObject parEntGameObject)
    {
      base.Init(parEntGameObject, false, true);
      ParentGameObject.LinkedAppModel.GetSoundManager().PlayBgMusic(EAppMusicAssets.GameOver, true);
      return this;
    }

    /// <summary>
    /// Сигнал фиксированного обновления модели (используется преимущественно для обработки физики и ввода)
    /// </summary>
    /// <param name="parFixedDeltaTime">Время шага фиксированного обновления в секундах</param>
    public override void FixedUpdate(double parFixedDeltaTime)
    {
      base.FixedUpdate(parFixedDeltaTime);
      if (ParentGameObject.LinkedAppModel.GetPlayersManager()
        .IsActionButtonPressed(ParentGameObject.LinkedAppModel.GetPlayersManager().Player1))
      {
        ParentGameObject.LinkedAppModel.GetSoundManager().StopMusic();
        OnScreenConfirmed?.Invoke();
      }
    }
  }
}