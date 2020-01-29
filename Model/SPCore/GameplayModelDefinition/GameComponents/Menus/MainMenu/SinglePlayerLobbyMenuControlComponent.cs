using System;
using System.Collections.Generic;
using System.Linq;
using CoreUtil.Pool;
using Model.SPCore.Consts;
using Model.SPCore.GameplayModelDefinition.GameComponents.Universal.DS;
using Model.SPCore.GameplayModelDefinition.ObjectModel;
using Model.SPCore.GameplayModelDefinition.SidePocketGame.Data.Setup;
using Model.SPCore.Managers.Sound.Data;
using Model.SPCore.MGameActions;

namespace Model.SPCore.GameplayModelDefinition.GameComponents.Menus.MainMenu
{
  /// <summary>
  /// Компонент, ответственный за подменю лобби старта однопользовательского режима игры
  /// </summary>
  public class SinglePlayerLobbyMenuControlComponent : StandardMenuControlComponent
  {
    /// <summary>
    /// Текущий выбранный игровой уровень
    /// </summary>
    private GameLevel _currentSelectedLevel;

    /// <summary>
    /// Индекс текущего выбранного игрового уровня
    /// </summary>
    private int _gameLevelIndex;

    /// <summary>
    /// Конструктор для поддержки системы пулинга объектов
    /// </summary>
    /// <param name="parSupportData">Данные для работы с пулом</param>
    public SinglePlayerLobbyMenuControlComponent(ObjectPoolSupportData parSupportData) : base(parSupportData)
    {
    }

    /// <summary>
    /// Событие о начале подготовки к переходу на игровой уровень со стороны View
    /// </summary>
    public event Action TransitionToTheGameStageStarted;
    
    /// <summary>
    /// Событие о завершении подготовки к переходу на игровой уровень со стороны View
    /// </summary>
    public event Action ViewEndedTransitionToTheGameStage;

    /// <summary>
    /// Замена конструктора, процедура инициализации компонента
    /// </summary>
    /// <param name="parEntGameObject">Родительский игровой объект</param>
    public new SinglePlayerLobbyMenuControlComponent Init(GameObject parEntGameObject)
    {
      base.Init(parEntGameObject);

      AvailableLevels = GameConsts.AvailableGameLevels;

      CurrentSelectedLevel = AvailableLevels.First();

      return this;
    }

    /// <summary>
    /// Процедура активации подменю
    /// </summary>
    public override void Activate()
    {
      //начинаем проигрывать музыкальный трек лобби
      ParentGameObject.LinkedAppModel.GetSoundManager().PlaySfx(EAppSfxAssets.BallPocketed, false);

      ParentGameObject.LinkedAppModel.GetSoundManager().PlayBgMusic(EAppMusicAssets.LoungeBeforeLevelStart, true);

      //заполнение кнопок меню
      MenuUiElements = new LinkedList<UiElement>();

      LinkedList<KeyListenerData> buttonKeyListenerData = new LinkedList<KeyListenerData>();

      buttonKeyListenerData.AddLast(new KeyListenerData(EGameActionButton.Button_A,
        PlayerRef, StartGameLevel, true));
      buttonKeyListenerData.AddLast(new KeyListenerData(EGameActionButton.Button_B,
        PlayerRef, StartGameLevel, true));
      buttonKeyListenerData.AddLast(new KeyListenerData(EGameActionButton.Button_X,
        PlayerRef, StartGameLevel, true));
      buttonKeyListenerData.AddLast(new KeyListenerData(EGameActionButton.Button_Start,
        PlayerRef, StartGameLevel, true));
      buttonKeyListenerData.AddLast(new KeyListenerData(EGameActionButton.Dpad_Menu_Left,
        PlayerRef, SelectPrevLevel, false));
      buttonKeyListenerData.AddLast(new KeyListenerData(EGameActionButton.Dpad_Menu_Right,
        PlayerRef, SelectNextLevel, false));

      SelectStartLevelButton = new UiElementButton(buttonKeyListenerData, ParentGameObject);
      MenuUiElements.AddLast(SelectStartLevelButton);

      ReturnBackButton = CreateStandardButton(ReturnBack);
      MenuUiElements.AddLast(ReturnBackButton);

      SelectUiElement(MenuUiElements.First.Value);

      //активация управления

      DefineStdMenuHandlingControls();


      base.Activate();
    }

    /// <summary>
    /// Действия по началу игрового уровня (с пробросом в компонент главного меню)
    /// </summary>
    private void StartGameLevel()
    {
      ParentGameObject.LinkedAppModel.GetSoundManager().StopMusic();

      MainMenuControlComponent mainMenuControlComponent =
        ParentGameObject.GetComponent<MainMenuControlComponent>(typeof(MainMenuControlComponent));

      //disable all button listeners
      if (MenuHandlingControls != null)
      {
        foreach (var keyListenerComponent in MenuHandlingControls)
        {
          keyListenerComponent.DisableAndSendToPool();
        }
      }

      SelectUiElement(null);


      ViewEndedTransitionToTheGameStage += () => mainMenuControlComponent.StartGameHandle(CurrentSelectedLevel);

      TransitionToTheGameStageStarted?.Invoke();
    }

    /// <summary>
    /// Вид сообщает, что он готов к переходу на игровой уровень
    /// </summary>
    public void ViewNotifiesThatTransitionEnded()
    {
      ViewEndedTransitionToTheGameStage?.Invoke();
    }

    /// <summary>
    /// Осуществление выбора следующего игрового уровня
    /// </summary>
    private void SelectNextLevel()
    {
      int possibleNextLevel = _gameLevelIndex + 1;
      if (possibleNextLevel >= AvailableLevels.Count)
      {
        possibleNextLevel = 0;
      }

      CurrentSelectedLevel = AvailableLevels[possibleNextLevel];

      ViewUpdateSignal(0.0);
    }

    /// <summary>
    /// Осуществление выбора предыдущего игрового уровня
    /// </summary>
    private void SelectPrevLevel()
    {
      int possibleNextLevel = _gameLevelIndex - 1;
      if (possibleNextLevel < 0)
      {
        possibleNextLevel = AvailableLevels.Count - 1;
      }

      CurrentSelectedLevel = AvailableLevels[possibleNextLevel];

      ViewUpdateSignal(0.0);
    }

    /// <summary>
    /// Возврат в предыдущее меню
    /// </summary>
    private void ReturnBack()
    {
      //включаем музыку главного меню обратно
      ParentGameObject.LinkedAppModel.GetSoundManager().PlayBgMusic(EAppMusicAssets.MainMenu, true);
      CloseSubmenu();
    }

    /// <summary>
    /// Совмещенная кнопка выбора и старта уровня
    /// </summary>
    public UiElementButton SelectStartLevelButton { get; private set; }

    /// <summary>
    /// Кнопка возврата в предыдущее меню
    /// </summary>
    public UiElementButton ReturnBackButton { get; private set; }

    /// <summary>
    /// Текущий выбранный игровой уровень
    /// </summary>
    public GameLevel CurrentSelectedLevel
    {
      get => _currentSelectedLevel;
      private set
      {
        _currentSelectedLevel = value;
        _gameLevelIndex = AvailableLevels.IndexOf(value);
      }
    }

    /// <summary>
    /// Список доступных для выбора игровых уровней
    /// </summary>
    private List<GameLevel> AvailableLevels { get; set; }
  }
}