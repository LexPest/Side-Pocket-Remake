namespace Model.SPCore.GameplayModelDefinition.GameComponents.Menus
{
  /// <summary>
  /// Базовый класс для определения контракта и функциональности для
  /// стандартных элементов пользовательского интерфеса в меню и подменю
  /// </summary>
  public abstract class UiElement
  {
    /// <summary>
    /// Действия при активации
    /// </summary>
    public virtual void Hover()
    {
      IsHovered = true;
    }
    
    /// <summary>
    /// Действия при деактивации
    /// </summary>
    public virtual void Unhover()
    {
      IsHovered = false;
    }

    /// <summary>
    /// Активен ли сейчас этот элемент
    /// </summary>
    public bool IsHovered { get; private set; }
  }
}