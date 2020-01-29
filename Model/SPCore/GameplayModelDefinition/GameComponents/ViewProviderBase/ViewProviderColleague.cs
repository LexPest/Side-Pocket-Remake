namespace Model.SPCore.GameplayModelDefinition.GameComponents.ViewProviderBase
{
  /// <summary>
  /// "Сотрудник" для взаимодействия между моделью и отображением
  /// </summary>
  public abstract class ViewProviderColleague
  {
    /// <summary>
    /// Стандартный конструктор
    /// </summary>
    /// <param name="parActualViewProviderMediator">"Посредник" для осуществления взаимодействия</param>
    public ViewProviderColleague(ViewProviderMediator parActualViewProviderMediator)
    {
      ActualViewProviderMediator = parActualViewProviderMediator;
    }

    /// <summary>
    /// Получение сообщения о создании компонента
    /// </summary>
    /// <param name="parTargetComponent">Созданный компонент</param>
    public abstract void ReceiveNotificationCreated(ViewProviderComponent parTargetComponent);

    /// <summary>
    /// Получение сообщения об удалении компонента
    /// </summary>
    /// <param name="parTargetComponent">Удаляемый компонент</param>
    public abstract void ReceiveNotificationRemoved(ViewProviderComponent parTargetComponent);

    /// <summary>
    /// Получение сообщения об обновлении компонента
    /// </summary>
    /// <param name="parTargetComponent">Обновленный компонент</param>
    public abstract void ReceiveNotificationUpdated(ViewProviderComponent parTargetComponent);

    /// <summary>
    /// Текущий посредник
    /// </summary>
    public ViewProviderMediator ActualViewProviderMediator { get; private set; }
  }
}