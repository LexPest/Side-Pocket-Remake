namespace Model.SPCore.GameplayModelDefinition.GameComponents.ViewProviderBase
{
  /// <summary>
  /// Класс реализует шаблон "Посредник" для взаимодействия и передачи данных между компонентами модели и отображения
  /// </summary>
  public class ViewProviderMediator
  {
    /// <summary>
    /// Стандартный конструктор без параметров
    /// </summary>
    public ViewProviderMediator()
    {
    }

    /// <summary>
    /// Отправить сообщение о создании компонента
    /// </summary>
    /// <param name="parColleague">Целевой "сотрудник"</param>
    /// <param name="parTargetComponent">Созданный компонент</param>
    public void SendCreatedNotification(ViewProviderColleague parColleague, ViewProviderComponent parTargetComponent)
    {
      parColleague.ReceiveNotificationCreated(parTargetComponent);
    }

    /// <summary>
    /// Отправить сообщение об удалении компонента
    /// </summary>
    /// <param name="parColleague">Целевой "сотрудник"</param>
    /// <param name="parTargetComponent">Удаляемый компонент</param>
    public void SendRemovedNotification(ViewProviderColleague parColleague, ViewProviderComponent parTargetComponent)
    {
      parColleague.ReceiveNotificationRemoved(parTargetComponent);
    }

    /// <summary>
    /// Отправить сообщение об обновлении компонента
    /// </summary>
    /// <param name="parColleague">Целевой "сотрудник"</param>
    /// <param name="parTargetComponent">Обновленный компонент</param>
    public void SendUpdatedNotification(ViewProviderColleague parColleague, ViewProviderComponent parTargetComponent)
    {
      parColleague.ReceiveNotificationUpdated(parTargetComponent);
    }

    /// <summary>
    /// "Сотрудник" со стороны модели
    /// </summary>
    public ViewProviderColleague ViewProviderModelSide { get; set; }
    
    /// <summary>
    /// "Сотрудник" со стороны отображения
    /// </summary>
    public ViewProviderColleague ViewProviderViewSide { get; set; }
  }
}