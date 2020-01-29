using System;
using System.Collections.Generic;
using CoreUtil.Pool;
using Model.SPCore.GameplayModelDefinition.ObjectModel;
using Model.SPCore.GameplayModelDefinition.SidePocketGame.Data.Setup;

namespace Model.SPCore.GameplayModelDefinition.GameComponents.Menus.MainMenu
{
  /// <summary>
  /// Компонент, ответственный за главное меню игры
  /// </summary>
  public class MainMenuControlComponent : StandardMenuControlComponent
  {
    /// <summary>
    /// Конструктор для поддержки системы пулинга объектов
    /// </summary>
    /// <param name="parSupportData">Данные для работы с пулом</param>
    public MainMenuControlComponent(ObjectPoolSupportData parSupportData) : base(parSupportData)
    {
    }

    /// <summary>
    /// Событие старта игровой сессии
    /// </summary>
    public event Action<GameLevel> OnGameStart;

    /// <summary>
    /// Замена конструктора, процедура инициализации компонента
    /// </summary>
    /// <param name="parEntGameObject">Родительский игровой объект</param>
    public new MainMenuControlComponent Init(GameObject parEntGameObject)
    {
      base.Init(parEntGameObject);

      return this;
    }

    /// <summary>
    /// Вызов активации события старта игровой сессии
    /// </summary>
    /// <param name="parGameLevel"></param>
    public void StartGameHandle(GameLevel parGameLevel)
    {
      OnGameStart?.Invoke(parGameLevel);
    }

    /// <summary>
    /// Процедура активации главного меню
    /// </summary>
    public override void Activate()
    {
      //заполенение подменю
      SubMenus = new List<MenuControlComponent>();

      SinglePlayerLobbySubMenu =
        ParentGameObject.AddComponent<SinglePlayerLobbyMenuControlComponent>(
          ActualLinkedObjectPoolSupportData.LinkedPoolManager
            .GetObject<SinglePlayerLobbyMenuControlComponent>(typeof(SinglePlayerLobbyMenuControlComponent))
            .Init(ParentGameObject));

      SubMenus.Add(SinglePlayerLobbySubMenu);

      OptionsSubMenu =
        ParentGameObject.AddComponent<OptionsMenuControlComponent>(
          ActualLinkedObjectPoolSupportData.LinkedPoolManager
            .GetObject<OptionsMenuControlComponent>(typeof(OptionsMenuControlComponent))
            .Init(ParentGameObject));

      SubMenus.Add(OptionsSubMenu);

      RecordsSubMenu =
        ParentGameObject.AddComponent<RecordsMenuControlComponent>(
          ActualLinkedObjectPoolSupportData.LinkedPoolManager
            .GetObject<RecordsMenuControlComponent>(typeof(RecordsMenuControlComponent))
            .Init(ParentGameObject));

      SubMenus.Add(RecordsSubMenu);

      JukeboxSubMenu =
        ParentGameObject.AddComponent<JukeboxMenuControlComponent>(
          ActualLinkedObjectPoolSupportData.LinkedPoolManager
            .GetObject<JukeboxMenuControlComponent>(typeof(JukeboxMenuControlComponent))
            .Init(ParentGameObject));

      SubMenus.Add(JukeboxSubMenu);


      //заполнение кнопок меню
      MenuUiElements = new LinkedList<UiElement>();

      NewGame1PPocketButton = CreateStandardButton(() => GoToSubmenu(SinglePlayerLobbySubMenu));

      MenuUiElements.AddLast(NewGame1PPocketButton); //лобби


      OptionsButton = CreateStandardButton(() => GoToSubmenu(OptionsSubMenu));

      MenuUiElements.AddLast(OptionsButton); //настройки


      RecordsButton = CreateStandardButton(() => GoToSubmenu(RecordsSubMenu));

      MenuUiElements.AddLast(RecordsButton); //рекорды


      JukeboxButton = CreateStandardButton(() => GoToSubmenu(JukeboxSubMenu));

      MenuUiElements.AddLast(JukeboxButton); //музыкальный плеер


      ExitButton = CreateStandardButton(ExitAction);

      MenuUiElements.AddLast(ExitButton); //выход


      SelectUiElement(MenuUiElements.First.Value);

      //Активация управления меню

      DefineStdMenuHandlingControls();

      base.Activate();
    }

    /// <summary>
    /// Процедура деактивации подменю
    /// </summary>
    public override void Deactivate()
    {
      base.Deactivate();
    }

    /// <summary>
    /// Вызов действия выхода из главного меню
    /// </summary>
    private void ExitAction()
    {
      CloseSubmenu();
    }

    /// <summary>
    /// Компонент подменю лобби одиночного режима игры
    /// </summary>
    private MenuControlComponent SinglePlayerLobbySubMenu { get; set; }

    /// <summary>
    /// Компонент подменю настроек
    /// </summary>
    private MenuControlComponent OptionsSubMenu { get; set; }

    /// <summary>
    /// Компонент подменю экрана рекордов
    /// </summary>
    private MenuControlComponent RecordsSubMenu { get; set; }

    /// <summary>
    /// Компонент подменю экрана музыкального проигрывателя саундтрека
    /// </summary>
    private MenuControlComponent JukeboxSubMenu { get; set; }

    /// <summary>
    /// Кнопка вызова подменю лобби одиночного режима игры
    /// </summary>
    public UiElementButton NewGame1PPocketButton { get; private set; }

    /// <summary>
    /// Кнопка вызова подменю настроек
    /// </summary>
    public UiElementButton OptionsButton { get; private set; }

    /// <summary>
    /// Кнопка вызова подменю экрана рекордов
    /// </summary>
    public UiElementButton RecordsButton { get; private set; }

    /// <summary>
    /// Кнопка вызова подменю экрана музыкального проигрывателя саундтрека
    /// </summary>
    public UiElementButton JukeboxButton { get; private set; }

    /// <summary>
    /// Кнопка вызова команды выхода
    /// </summary>
    public UiElementButton ExitButton { get; private set; }
  }
}