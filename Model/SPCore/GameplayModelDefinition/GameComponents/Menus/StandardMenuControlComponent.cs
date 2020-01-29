using System;
using System.Collections.Generic;
using CoreUtil.Pool;
using Model.SPCore.GameplayModelDefinition.GameComponents.Universal;
using Model.SPCore.GameplayModelDefinition.GameComponents.Universal.DS;
using Model.SPCore.GameplayModelDefinition.ObjectModel;
using Model.SPCore.MGameActions;
using Model.SPCore.MPlayers;

namespace Model.SPCore.GameplayModelDefinition.GameComponents.Menus
{
  /// <summary>
  /// Базовый класс, определяющий функциональность и элементы контракта
  /// для меню и подменю, имеющий общий стандартизированный дизайн и тип
  /// </summary>
  public abstract class StandardMenuControlComponent : MenuControlComponent
  {
    /// <summary>
    /// Компоненты, следящие за вводом игрока для управления меню/подменю
    /// </summary>
    protected List<KeyListenerComponent> MenuHandlingControls;
    /// <summary>
    /// Доступные в этом меню подменю
    /// </summary>
    protected List<MenuControlComponent> SubMenus;

    /// <summary>
    /// Конструктор для поддержки системы пулинга объектов
    /// </summary>
    /// <param name="parSupportData">Данные для работы с пулом</param>
    protected StandardMenuControlComponent(ObjectPoolSupportData parSupportData) : base(parSupportData)
    {
    }

    /// <summary>
    /// Замена конструктора, процедура инициализации компонента
    /// </summary>
    /// <param name="parEntGameObject">Родительский игровой объект</param>
    public void Init(GameObject parEntGameObject)
    {
      base.Init(parEntGameObject, false, false);

      PlayerRef = ParentGameObject.LinkedAppModel.GetPlayersManager().Player1;
    }

    /// <summary>
    /// Процедура деактивации меню/подменю
    /// </summary>
    public override void Deactivate()
    {
      base.Deactivate();

      if (MenuHandlingControls != null)
      {
        foreach (var keyListenerComponent in MenuHandlingControls)
        {
          keyListenerComponent.DisableAndSendToPool();
        }
      }

      MenuHandlingControls = null;

      SelectUiElement(null);
    }

    /// <summary>
    /// Выбор текущего активного элемента пользовательского интерфейса
    /// </summary>
    /// <param name="parSelectedElement">Новый выбранный элемент UI</param>
    protected void SelectUiElement(UiElement parSelectedElement)
    {
      CurrentHoveredElement?.Unhover();

      CurrentHoveredElement = parSelectedElement;
      CurrentHoveredElement?.Hover();
      ViewUpdateSignal(0.0);
    }

    /// <summary>
    /// Действия по переходу во вложенное в это меню подменю
    /// </summary>
    /// <param name="parTargetSubmenu">Вложенное подменю для перехода</param>
    protected void GoToSubmenu(MenuControlComponent parTargetSubmenu)
    {
      Deactivate();
      parTargetSubmenu.Activate();
      parTargetSubmenu.MenuClosedEvent += OnSubmenuClosed;
      CurrentSubMenu = parTargetSubmenu;
    }

    /// <summary>
    /// Обработчик закрытия вложенного подменю
    /// </summary>
    protected void OnSubmenuClosed()
    {
      if (CurrentSubMenu != null)
      {
        CurrentSubMenu.MenuClosedEvent -= OnSubmenuClosed;
        CurrentSubMenu = null;
      }

      Activate();
    }

    /// <summary>
    /// Закрытие меню/подменю и возврат на уровень выше
    /// </summary>
    protected void CloseSubmenu()
    {
      Deactivate();
      if (SubMenus != null)
      {
        foreach (var menuControlComponent in SubMenus)
        {
          menuControlComponent.DisableAndSendToPool();
        }
      }

      RaiseMenuClosed();
    }

    /// <summary>
    /// Добавление стандартных действий для обработки нажатия на кнопки в меню
    /// </summary>
    /// <param name="parButtonKeyListenerData">Обработчик ввода игрока</param>
    /// <param name="parOnPressedAction">Действие при нажатии</param>
    protected void AddToButtonKeyListenerDataConfirmationAction(LinkedList<KeyListenerData> parButtonKeyListenerData,
      Action parOnPressedAction)
    {
      parButtonKeyListenerData.AddLast(new KeyListenerData(EGameActionButton.Button_A,
        PlayerRef, parOnPressedAction, false));
      parButtonKeyListenerData.AddLast(new KeyListenerData(EGameActionButton.Button_B,
        PlayerRef, parOnPressedAction, false));
      parButtonKeyListenerData.AddLast(new KeyListenerData(EGameActionButton.Button_X,
        PlayerRef, parOnPressedAction, false));
      parButtonKeyListenerData.AddLast(new KeyListenerData(EGameActionButton.Button_Start,
        PlayerRef, parOnPressedAction, false));
    }

    /// <summary>
    /// Создает стандартную кнопку
    /// </summary>
    /// <param name="parOnPressedAction">Действие при нажатии на кнопку</param>
    /// <returns>Созданную кнопку</returns>
    protected UiElementButton CreateStandardButton(Action parOnPressedAction)
    {
      LinkedList<KeyListenerData> buttonKeyListenerData = new LinkedList<KeyListenerData>();
      AddToButtonKeyListenerDataConfirmationAction(buttonKeyListenerData,
        parOnPressedAction);
      return new UiElementButton(buttonKeyListenerData, ParentGameObject);
    }

    /// <summary>
    /// Перемещение по элементам пользовательского интерфейса меню вверх
    /// </summary>
    protected void StdMenuMoveUp()
    {
      LinkedListNode<UiElement> linkedListNode =
        (MenuUiElements.Find((UiElementButton) CurrentHoveredElement)?.Previous);
      if (linkedListNode != null)
        SelectUiElement(linkedListNode?.Value);
      //ViewUpdateSignal(0.0);
    }

    /// <summary>
    /// Перемещение по элементам пользовательского интерфейса меню вниз
    /// </summary>
    protected void StdMenuMoveDown()
    {
      LinkedListNode<UiElement> linkedListNode =
        MenuUiElements.Find((UiElementButton) CurrentHoveredElement)?.Next;
      if (linkedListNode != null)
        SelectUiElement(linkedListNode?.Value);
      //ViewUpdateSignal(0.0);
    }

    /// <summary>
    /// Определяет основные стандартные обработчики ввода для осуществления операций перемешения по элементам
    /// пользовательского интерфейса меню
    /// </summary>
    protected void DefineStdMenuHandlingControls()
    {
      MenuHandlingControls = new List<KeyListenerComponent>();

      KeyListenerData menuMoveUp =
        new KeyListenerData(EGameActionButton.Dpad_Menu_Up, PlayerRef, StdMenuMoveUp, false);
      KeyListenerData menuMoveDown =
        new KeyListenerData(EGameActionButton.Dpad_Menu_Down, PlayerRef, StdMenuMoveDown, false);

      KeyListenerComponent menuControlComponent =
        ActualLinkedObjectPoolSupportData.LinkedPoolManager
          .GetObject<KeyListenerComponent>(typeof(KeyListenerComponent)).Init(ParentGameObject);

      menuControlComponent.WatchData.Add(menuMoveUp);
      menuControlComponent.WatchData.Add(menuMoveDown);

      MenuHandlingControls.Add(ParentGameObject.AddComponent<KeyListenerComponent>(menuControlComponent));
    }

    /// <summary>
    /// Текущий выделенный активный элемент меню
    /// </summary>
    public UiElement CurrentHoveredElement { get; protected set; }
    
    /// <summary>
    /// Управляющий меню игрок
    /// </summary>
    protected MPlayer PlayerRef { get; set; }
    
    /// <summary>
    /// Элементы пользовательского интерфейса меню/подменю
    /// </summary>
    public LinkedList<UiElement> MenuUiElements { get; protected set; }

    /// <summary>
    /// Текущее активное для этого меню подменю (если был совершен такой переход)
    /// </summary>
    private MenuControlComponent CurrentSubMenu { get; set; }
  }
}