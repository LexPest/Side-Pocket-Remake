using System;
using CoreUtil.Pool;
using Model.SPCore.GameplayModelDefinition.ComponentModel;
using Model.SPCore.GameplayModelDefinition.GameComponents.InGame;
using Model.SPCore.GameplayModelDefinition.GameComponents.Launching.ViewProviders;
using Model.SPCore.GameplayModelDefinition.GameComponents.Menus.MainMenu;
using Model.SPCore.GameplayModelDefinition.GameComponents.Universal;
using Model.SPCore.GameplayModelDefinition.GameComponents.Universal.DS;
using Model.SPCore.GameplayModelDefinition.ObjectModel;
using Model.SPCore.GameplayModelDefinition.SidePocketGame.Data.Setup;
using Model.SPCore.Managers.Sound.Data;
using Model.SPCore.MGameActions;

namespace Model.SPCore.GameplayModelDefinition.GameComponents.Launching
{
  /// <summary>
  /// Компонент, отвечающий за старт приложения
  /// </summary>
  public class AppStartComponent : Component
  {
    /// <summary>
    /// Ссылка на текущий компонент, управляющий игровой сессией
    /// </summary>
    private GameSessionControllerComponent _currentGameSessionControlComponent;

    /// <summary>
    /// Текущий игровой объект игровой сессии
    /// </summary>
    private GameObject _currentGameSessionGameObject;
    /// <summary>
    /// Ссылка на текущий компонент, управляющий главным меню
    /// </summary>
    private MainMenuControlComponent _currentMainMenuControlComponent;

    /// <summary>
    /// Текущий игровой объект главного меню
    /// </summary>
    private GameObject _currentMainMenuMainGameObject;

    /// <summary>
    /// Флаг - сигнал начала игры
    /// </summary>
    private bool _gameStartSignal;
    
    /// <summary>
    /// Выбранный стартовый игровой уровень
    /// </summary>
    private GameLevel _selectedLevel;

    /// <summary>
    /// Конструктор для поддержки системы пулинга объектов
    /// </summary>
    /// <param name="parSupportData">Данные для работы с пулом</param>
    public AppStartComponent(ObjectPoolSupportData parSupportData) : base(parSupportData)
    {
    }

    /// <summary>
    /// Замена конструктора, процедура инициализации компонента
    /// </summary>
    /// <param name="parEntGameObject">Родительский игровой объект</param>
    public AppStartComponent Init(GameObject parEntGameObject)
    {
      base.Init(parEntGameObject, true, true);
      Console.WriteLine("Application launch performing...");
      IsFirstUpdate = true;
      return this;
    }

    /// <summary>
    /// Сигнал обновления модели
    /// </summary>
    /// <param name="parDeltaTime">Время кадра в секундах</param>
    public override void Update(double parDeltaTime)
    {
      base.Update(parDeltaTime);
      if (IsFirstUpdate)
      {
        IsFirstUpdate = false;
        if (ParentGameObject.LinkedAppModel.GetGameplaySettingsData().IsIntroDisabled)
        {
          //пропускаем заставку
          ToMainMenu();
        }
        else
        {
          WatchingCutsceneProviderComponent = ParentGameObject.AddComponent<IntroViewProviderComponent>(
            ActualLinkedObjectPoolSupportData.LinkedPoolManager
              .GetObject<IntroViewProviderComponent>(typeof(IntroViewProviderComponent))
              .Init(ParentGameObject));

          ParentGameObject.LinkedAppModel.GetSoundManager().PlaySfx(EAppSfxAssets.IntroCutscene, false);

          WatchingCutsceneProviderComponent.OnIntroCutsceneEndedPerform += OnCutsceneEnd;
        }
      }
      else if (_gameStartSignal)
      {
        //уничтожаем меню

        _currentMainMenuControlComponent.MenuClosedEvent -= ApplicationExit;
        _currentMainMenuControlComponent.OnGameStart -= StartGame;

        _currentMainMenuControlComponent.DisableAndSendToPool();
        _currentMainMenuMainGameObject.DisableAndSendToPool();


        _currentMainMenuMainGameObject = null;
        _currentMainMenuControlComponent = null;

        Console.WriteLine("Game is starting...");
        //переходим к выбранному игровому уровню
        _currentGameSessionGameObject = ActualLinkedObjectPoolSupportData.LinkedPoolManager
          .GetObject<GameObject>(typeof(GameObject)).Init(ParentGameObject.LinkedAppModel);

        _currentGameSessionControlComponent = ActualLinkedObjectPoolSupportData.LinkedPoolManager
          .GetObject<GameSessionControllerComponent>(typeof(GameSessionControllerComponent))
          .Init(_currentGameSessionGameObject, _selectedLevel);
        _currentGameSessionGameObject.AddComponent(_currentGameSessionControlComponent);

        _currentGameSessionControlComponent.OnGameSessionEnd += EndGameSession;

        _gameStartSignal = false;
      }
    }

    /// <summary>
    /// Обработчик завершения стартовой заставки
    /// </summary>
    private void OnCutsceneEnd()
    {
      Console.WriteLine("Intro cutscene ending handled!");
      //dispose watcher and player
      WatchingCutsceneProviderComponent?.DisableAndSendToPool();
      WatchingCutsceneProviderComponent = null;

      //add listener for main menu transition
      WatchingKeyComponent = ParentGameObject.AddComponent<KeyListenerComponent>(ActualLinkedObjectPoolSupportData
          .LinkedPoolManager.GetObject<KeyListenerComponent>(typeof(KeyListenerComponent)))
        .Init(ParentGameObject);

      WatchingKeyComponent.WatchData.Add(new KeyListenerData(EGameActionButton.Button_Start,
        ParentGameObject.LinkedAppModel.GetPlayersManager().Player1,
        ToMainMenu, true));

      //transitioning to main menu
    }

    /// <summary>
    /// Действия по переходу в главное меню игры
    /// </summary>
    private void ToMainMenu()
    {
      Console.WriteLine("To main menu transition performed!");

      WatchingKeyComponent?.DisableAndSendToPool();

      ParentGameObject.LinkedAppModel.GetSoundManager().EnsureBgMusicIsPlaying(EAppMusicAssets.MainMenu, true);

      _currentMainMenuMainGameObject =
        ActualLinkedObjectPoolSupportData.LinkedPoolManager.GetObject<GameObject>(typeof(GameObject))
          .Init(ParentGameObject.LinkedAppModel);

      _currentMainMenuControlComponent =
        ActualLinkedObjectPoolSupportData.LinkedPoolManager.GetObject<MainMenuControlComponent>(
          typeof(MainMenuControlComponent)).Init(_currentMainMenuMainGameObject);
      _currentMainMenuMainGameObject.AddComponent(_currentMainMenuControlComponent);

      _currentMainMenuControlComponent.MenuClosedEvent += ApplicationExit;
      _currentMainMenuControlComponent.OnGameStart += StartGame;

      _currentMainMenuControlComponent.Activate();
    }

    /// <summary>
    /// Действия по началу игровой сессии
    /// </summary>
    /// <param name="parGameLevel">Игровой уровень</param>
    private void StartGame(GameLevel parGameLevel)
    {
      _selectedLevel = parGameLevel;
      _gameStartSignal = true;
    }

    /// <summary>
    /// Действия по завершению игровой сесиии
    /// </summary>
    private void EndGameSession()
    {
      _currentGameSessionControlComponent.OnGameSessionEnd -= EndGameSession;

      _currentGameSessionGameObject.DisableAndSendToPool();

      _currentGameSessionControlComponent = null;
      _currentGameSessionGameObject = null;

      ToMainMenu();
    }

    /// <summary>
    /// Действия по выходу из приложения с помощью кнопки
    /// </summary>
    private void ApplicationExit()
    {
      ParentGameObject.LinkedAppModel.ExitApp();
    }


    /// <summary>
    /// Ссылка на текущий компонент, ответственный за стартовую заставку
    /// </summary>
    private IntroViewProviderComponent WatchingCutsceneProviderComponent { get; set; }

    /// <summary>
    /// Ссылка на текущий компонент, ответственный за отслеживание ввода игрока
    /// </summary>
    private KeyListenerComponent WatchingKeyComponent { get; set; }

    /// <summary>
    /// Специальный флаг, обозначающий является ли текущий цикл обновления первым.
    /// Необходим, чтобы начать выполнять действия на втором цикле обновления игры, т.к. на первом
    /// может быть не до конца завершена инициализация.
    /// </summary>
    private bool IsFirstUpdate { get; set; } = true;
  }
}