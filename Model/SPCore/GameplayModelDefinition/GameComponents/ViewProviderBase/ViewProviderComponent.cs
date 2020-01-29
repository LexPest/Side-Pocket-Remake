using CoreUtil.Pool;
using Model.SPCore.GameplayModelDefinition.ComponentModel;
using Model.SPCore.GameplayModelDefinition.ObjectModel;

namespace Model.SPCore.GameplayModelDefinition.GameComponents.ViewProviderBase
{
  /// <summary>
  /// Базовый класс компонента для всех компонентов, которые должны быть связаны
  /// с соответствующими компонентами отображения. Они необходимы для предоставления данных
  /// модели отображению и связываются автоматически.
  /// </summary>
  public abstract class ViewProviderComponent : Component
  {
    /// <summary>
    /// Конструктор для поддержки системы пулинга объектов
    /// </summary>
    /// <param name="parSupportData">Данные для работы с пулом</param>
    protected ViewProviderComponent(ObjectPoolSupportData parSupportData) : base(parSupportData)
    {
    }

    /// <summary>
    /// Замена конструктора, процедура инициализации компонента
    /// </summary>
    /// <param name="parEntGameObject">Родительский игровой объект</param>
    /// <param name="parIsUpdatable">Должен ли компонент получать сигналы обновления модели</param>
    /// <param name="parIsFixedUpdatable">Должен ли компонент получать сигралы фиксированного обновления модели</param>
    protected override void Init(GameObject parEntGameObject, bool parIsUpdatable, bool parIsFixedUpdatable)
    {
      base.Init(parEntGameObject, parIsUpdatable, parIsFixedUpdatable);

      ProviderColleague =
        new ViewProviderModelSideStandardColleague(parEntGameObject.LinkedAppModel.GetViewProviderMediator());

      ProviderColleague.ActualViewProviderMediator.SendCreatedNotification(
        ProviderColleague.ActualViewProviderMediator.ViewProviderViewSide, this);
    }

    
    /// <summary>
    /// Отправить сигнал об обновлении компонента
    /// </summary>
    public void ViewUpdateSignal(double parDeltaTime)
    {
      ProviderColleague.ActualViewProviderMediator.SendUpdatedNotification(
        ProviderColleague.ActualViewProviderMediator.ViewProviderViewSide, this);
    }

    /// <summary>
    /// Действия перед отправкой в пул
    /// </summary>
    public override void OnBeforeResetPerform()
    {
      ProviderColleague.ActualViewProviderMediator.SendRemovedNotification(
        ProviderColleague.ActualViewProviderMediator.ViewProviderViewSide, this);

      base.OnBeforeResetPerform();
    }
    
    /// <summary>
    /// Связанный с компонентом коллега
    /// </summary>
    protected ViewProviderColleague ProviderColleague { get; private set; }
  }
}