using System.Collections.Generic;
using Model.SPCore.GameplayModelDefinition.GameComponents.Universal;
using Model.SPCore.GameplayModelDefinition.GameComponents.Universal.DS;
using Model.SPCore.GameplayModelDefinition.ObjectModel;

namespace Model.SPCore.GameplayModelDefinition.GameComponents.Menus
{
  /// <summary>
  /// Стандартный элемент пользовательского интерфейса меню кнопка
  /// </summary>
  public class UiElementButton : UiElement
  {
    /// <summary>
    /// Созданный обработчик ввода игрока
    /// </summary>
    protected KeyListenerComponent GeneratedKeyListener;

    /// <summary>
    /// Данные для обработчика ввода игрока
    /// </summary>
    protected LinkedList<KeyListenerData> ProvidedKeyListenerData;

    /// <summary>
    /// Целевой игровой объект
    /// </summary>
    protected GameObject TargetGameObject;

    /// <summary>
    /// Стандартный конструктор
    /// </summary>
    /// <param name="parProvidedKeyListenerData">Данные для обработчика ввода игрока</param>
    /// <param name="parTargetGameObject">Целевой игровой объект</param>
    public UiElementButton(LinkedList<KeyListenerData> parProvidedKeyListenerData, GameObject parTargetGameObject)
    {
      ProvidedKeyListenerData = parProvidedKeyListenerData;
      TargetGameObject = parTargetGameObject;
    }

    /// <summary>
    /// Действия при активации
    /// </summary>
    public override void Hover()
    {
      base.Hover();
      GeneratedKeyListener = TargetGameObject
        .AddComponent<KeyListenerComponent>(
          new KeyListenerComponent(TargetGameObject.ActualLinkedObjectPoolSupportData))
        .Init(TargetGameObject);
      GeneratedKeyListener.WatchData.AddRange(ProvidedKeyListenerData);
    }

    /// <summary>
    /// Действия при деактивации
    /// </summary>
    public override void Unhover()
    {
      base.Unhover();
      if (GeneratedKeyListener != null)
      {
        GeneratedKeyListener.DisableAndSendToPool();
        GeneratedKeyListener = null;
      }
    }
  }
}