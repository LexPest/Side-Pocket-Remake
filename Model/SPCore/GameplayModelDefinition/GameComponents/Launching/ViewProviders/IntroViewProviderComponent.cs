using System;
using CoreUtil.Pool;
using Model.SPCore.GameplayModelDefinition.GameComponents.ViewProviderBase;
using Model.SPCore.GameplayModelDefinition.ObjectModel;

namespace Model.SPCore.GameplayModelDefinition.GameComponents.Launching.ViewProviders
{
  /// <summary>
  /// Компонент для обработки стартовой заставки игры
  /// </summary>
  public class IntroViewProviderComponent : ViewProviderComponent
  {
    /// <summary>
    /// Делегат по завершению стартовой заставки
    /// </summary>
    public Action OnIntroCutsceneEndedPerform;

    /// <summary>
    /// Конструктор для поддержки системы пулинга объектов
    /// </summary>
    /// <param name="parSupportData">Данные для работы с пулом</param>
    public IntroViewProviderComponent(ObjectPoolSupportData parSupportData) : base(parSupportData)
    {
    }

    /// <summary>
    /// Замена конструктора, процедура инициализации компонента
    /// </summary>
    /// <param name="parEntGameObject">Родительский игровой объект</param>
    public IntroViewProviderComponent Init(GameObject parEntGameObject)
    {
      base.Init(parEntGameObject, false, false);
      OnIntroCutsceneEndedPerform += AfterIntroCutscenePerform;
      return this;
    }

    /// <summary>
    /// Дополнительное отладочное действие после игровой заставки
    /// </summary>
    private void AfterIntroCutscenePerform()
    {
      Console.WriteLine("Intro cutscene ended!");
    }
  }
}