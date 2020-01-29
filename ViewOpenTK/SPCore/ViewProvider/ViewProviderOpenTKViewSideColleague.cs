using Model.SPCore.GameplayModelDefinition.GameComponents.ViewProviderBase;

namespace ViewOpenTK.SPCore.ViewProvider
{
  /// <summary>
  /// Реализация "сотрудника" для взаимодействия между моделью и отображением
  /// </summary>
  public class ViewProviderOpenTkViewSideColleague : ViewProviderColleague
  {
    /// <summary>
    /// Стандартный конструктор
    /// </summary>
    /// <param name="parActualViewProviderMediator">"Посредник" для осуществления взаимодействия</param>
    /// <param name="parViewEventsHandler">Обработчик данных для рендеринга и событий OpenTK</param>
    public ViewProviderOpenTkViewSideColleague(ViewProviderMediator parActualViewProviderMediator,
      ViewEventsOpenTkHandler parViewEventsHandler) : base(parActualViewProviderMediator)
    {
      ActualViewEventsHandler = parViewEventsHandler;
    }

    /// <summary>
    /// Получение сообщения о создании компонента
    /// </summary>
    /// <param name="parTargetComponent">Созданный компонент</param>
    public override void ReceiveNotificationCreated(ViewProviderComponent parTargetComponent)
    {
      ActualViewEventsHandler.OnComponentCreated(parTargetComponent);
    }

    /// <summary>
    /// Получение сообщения об удалении компонента
    /// </summary>
    /// <param name="parTargetComponent">Удаляемый компонент</param>
    public override void ReceiveNotificationRemoved(ViewProviderComponent parTargetComponent)
    {
      ActualViewEventsHandler.OnComponentRemoved(parTargetComponent);
    }

    /// <summary>
    /// Получение сообщения об обновлении компонента
    /// </summary>
    /// <param name="parTargetComponent">Обновленный компонент</param>
    public override void ReceiveNotificationUpdated(ViewProviderComponent parTargetComponent)
    {
      ActualViewEventsHandler.OnComponentUpdated(parTargetComponent);
    }

    /// <summary>
    /// Обработчик данных для рендеринга и событий OpenTK
    /// </summary>
    private ViewEventsOpenTkHandler ActualViewEventsHandler { get; set; }
  }
}