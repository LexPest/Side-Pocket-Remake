using System;
using CoreUtil.Pool;
using Model.SPCore.GameplayModelDefinition.GameComponents.ViewProviderBase;

namespace Model.SPCore.GameplayModelDefinition.GameComponents.Menus
{
  /// <summary>
  /// Базовый класс, определяющий общую функциональность и элементы контракта для меню и подменю
  /// </summary>
  public abstract class MenuControlComponent : ViewProviderComponent
  {
    /// <summary>
    /// Конструктор для поддержки системы пулинга объектов
    /// </summary>
    /// <param name="parSupportData">Данные для работы с пулом</param>
    protected MenuControlComponent(ObjectPoolSupportData parSupportData) : base(parSupportData)
    {
    }

    /// <summary>
    /// События закрытия меню или подменю
    /// </summary>
    public event Action MenuClosedEvent;

    /// <summary>
    /// Событие активации меню/подменю
    /// </summary>
    public event Action OnBecomeActive;

    /// <summary>
    /// Событие деактивации меню/подменю
    /// </summary>
    public event Action OnDeactivated;

    /// <summary>
    /// Процедура активации меню/подменю
    /// </summary>
    public virtual void Activate()
    {
      IsActive = true;
      OnBecomeActive?.Invoke();
    }

    /// <summary>
    /// Процедура деактивации меню/подменю
    /// </summary>
    public virtual void Deactivate()
    {
      IsActive = false;
      OnDeactivated?.Invoke();
    }

    /// <summary>
    /// Вызов события закрытия меню
    /// </summary>
    protected void RaiseMenuClosed()
    {
      MenuClosedEvent?.Invoke();
    }

    /// <summary>
    /// Является ли меню активным в данный момент
    /// </summary>
    public bool IsActive { get; private set; }
  }
}