namespace Model.SPCore.GameplayModelDefinition.GameComponents.ViewProviderBase
{
  /// <summary>
  /// Класс реализует "сотрудника" со стороны модели
  /// </summary>
  public class ViewProviderModelSideStandardColleague : ViewProviderColleague
  {
    /// <summary>
    /// Стандартный конструктор
    /// </summary>
    /// <param name="parActualViewProviderMediator">Посредник</param>
    public ViewProviderModelSideStandardColleague(ViewProviderMediator parActualViewProviderMediator) : base(
      parActualViewProviderMediator)
    {
    }

    /// <summary>
    /// Получение сообщения о создании компонента (не используется в текущей версии, но необходимо в будущем)
    /// </summary>
    /// <param name="parTargetComponent">Созданный компонент</param>
    public override void ReceiveNotificationCreated(ViewProviderComponent parTargetComponent)
    {
      throw new System.NotImplementedException("This feature is not supported in this version");
    }

    /// <summary>
    /// Получение сообщения об удалении компонента (не используется в текущей версии, но необходимо в будущем)
    /// </summary>
    /// <param name="parTargetComponent">Удаляемый компонент</param>
    public override void ReceiveNotificationRemoved(ViewProviderComponent parTargetComponent)
    {
      throw new System.NotImplementedException("This feature is not supported in this version");
    }

    /// <summary>
    /// Получение сообщения об обновлении компонента (не используется в текущей версии, но необходимо в будущем)
    /// </summary>
    /// <param name="parTargetComponent">Обновленный компонент</param>
    public override void ReceiveNotificationUpdated(ViewProviderComponent parTargetComponent)
    {
      throw new System.NotImplementedException("This feature is not supported in this version");
    }
  }
}