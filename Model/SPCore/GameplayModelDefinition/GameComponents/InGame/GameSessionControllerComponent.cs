using System;
using System.Collections;
using CoreUtil.Coroutine;
using CoreUtil.Pool;
using Model.SPCore.Consts;
using Model.SPCore.GameplayModelDefinition.ComponentModel;
using Model.SPCore.GameplayModelDefinition.GameComponents.InGame.ResultScreens;
using Model.SPCore.GameplayModelDefinition.GameComponents.InGame.ResultScreens.ViewProviders;
using Model.SPCore.GameplayModelDefinition.GameComponents.InGame.ViewProviders;
using Model.SPCore.GameplayModelDefinition.ObjectModel;
using Model.SPCore.GameplayModelDefinition.SidePocketGame.Data;
using Model.SPCore.GameplayModelDefinition.SidePocketGame.Data.Setup;
using Model.SPCore.Managers.Sound.Data;

namespace Model.SPCore.GameplayModelDefinition.GameComponents.InGame
{
  /// <summary>
  /// Компонент, управляющий игровой сессией
  /// </summary>
  public class GameSessionControllerComponent : Component
  {
    /// <summary>
    /// Бонусные жизни за успешное прохождение уровня
    /// </summary>
    private const int LIVES_PER_LEVEL = 2;

    /// <summary>
    /// Стартовое количество жизней у игрока
    /// </summary>
    private const int LIVES_COUNT_START = 16;
    
    /// <summary>
    /// Основной игрок
    /// </summary>
    private Player _actualPlayer;

    /// <summary>
    /// Текущий игровой объект текущего запущенного игрового уровня
    /// </summary>
    private GameObject _currentLevelGameObject;
    /// <summary>
    /// Текущий компонент запущенного игрового уровня
    /// </summary>
    private PocketGameViewProvider _currentPocketViewProvider;
    /// <summary>
    /// Флаг: можем ли перейти к экрану результатов
    /// </summary>
    private bool _flagCanProceedToResultsScreen = false;

    /// <summary>
    /// Ссылка на актуальный компонент экрана проигрыша
    /// </summary>
    private GameOverScreenComponent _gameOverComponent;

    /// <summary>
    /// Флаг: начат ли уровень
    /// </summary>
    private bool _levelStarted = false;
    /// <summary>
    /// Ссылка на актуальный компонент экрана результатов в случае прохождения уровня
    /// </summary>
    private ResultsScreenComponent _resultsScreenComponent;
    private GameLevel _startingLevel;

    /// <summary>
    /// Конструктор для поддержки системы пулинга объектов
    /// </summary>
    /// <param name="parSupportData">Данные для работы с пулом</param>
    public GameSessionControllerComponent(ObjectPoolSupportData parSupportData) : base(parSupportData)
    {
    }

    /// <summary>
    /// Событие полного завершения игровой сессии
    /// </summary>
    public event Action OnGameSessionEnd;

    /// <summary>
    /// Замена конструктора, процедура инициализации компонента
    /// </summary>
    /// <param name="parEntGameObject">Родительский игровой объект</param>
    /// <param name="parStartLevel">Стартовый уровень для начала игры</param>
    public new GameSessionControllerComponent Init(GameObject parEntGameObject, GameLevel parStartLevel)
    {
      base.Init(parEntGameObject, false, true);
      Console.WriteLine("Starting game session...");
      _actualPlayer = new Player(parEntGameObject.LinkedAppModel.GetPlayersManager().Player1, 0, LIVES_COUNT_START);
      //StartLevel(startLevel);
      _startingLevel = parStartLevel;
      return this;
    }

    /// <summary>
    /// Сигнал фиксированного обновления модели (используется преимущественно для обработки физики и ввода)
    /// </summary>
    /// <param name="parFixedDeltaTime">Время шага фиксированного обновления в секундах</param>
    public override void FixedUpdate(double parFixedDeltaTime)
    {
      base.FixedUpdate(parFixedDeltaTime);
      if (!_levelStarted)
      {
        StartLevel(_startingLevel);
        _levelStarted = true;
      }

      if (_flagCanProceedToResultsScreen)
      {
        _flagCanProceedToResultsScreen = false;
        ProceedToResultsScreen();
      }
    }

    /// <summary>
    /// Осуществляет старт игрового уровня
    /// </summary>
    /// <param name="parLevel">Данные об игровом уровне</param>
    public void StartLevel(GameLevel parLevel)
    {
      _currentLevelGameObject = ActualLinkedObjectPoolSupportData.LinkedPoolManager
        .GetObject<GameObject>(typeof(GameObject)).Init(ParentGameObject.LinkedAppModel);

      _currentPocketViewProvider = ActualLinkedObjectPoolSupportData.LinkedPoolManager
        .GetObject<PocketGameViewProvider>(typeof(PocketGameViewProvider))
        .Init(_currentLevelGameObject, parLevel.ActualLevelLevelConfig, _actualPlayer);

      _currentPocketViewProvider.GameLevelEnd += OnGameLevelEnd;

      _startingLevel = parLevel;

      _currentPocketViewProvider.StartGameLevel();
    }

    /// <summary>
    /// Обработчик события окончания игрового уровня
    /// </summary>
    public void OnGameLevelEnd()
    {
      _currentPocketViewProvider.GameLevelEnd -= OnGameLevelEnd;
      Console.WriteLine("Game level ended");
      ParentGameObject.LinkedAppModel.GetSoundManager().StopMusic();

      if (_currentPocketViewProvider.EndLevelResult == ELevelEndResult.Lose)
      {
        //экран проигрыша
        PerformOnLose();
      }
      else if (_currentPocketViewProvider.EndLevelResult == ELevelEndResult.Win)
      {
        //экран победы

        //попробуем проиграть музыку успешного окончания уровня
        if (ParentGameObject.LinkedAppModel.GetGameplaySettingsData().IsMusicEnabled)
        {
          CoroutineManager.Instance.StartCoroutine(PlayVictoryMusicAndProceed());
        }
        else
        {
          _flagCanProceedToResultsScreen = true;
        }
      }
    }

    /// <summary>
    /// Короутина для проигрывания музыки успешного окончания уровня
    /// </summary>
    /// <returns></returns>
    private IEnumerator PlayVictoryMusicAndProceed()
    {
      ParentGameObject.LinkedAppModel.GetSoundManager().PlayBgMusic(EAppMusicAssets.SPLevelEnd, false);
      yield return new WaitForSeconds(7.25);
      _flagCanProceedToResultsScreen = true;
    }

    /// <summary>
    /// Действия в случае проигрыша
    /// </summary>
    private void PerformOnLose()
    {
      DestroySessionData();

      _gameOverComponent =
        ActualLinkedObjectPoolSupportData.LinkedPoolManager.GetObject<GameOverScreenComponent>(
          typeof(GameOverScreenComponent)).Init(ParentGameObject);

      _gameOverComponent.OnScreenConfirmed += EndGameSession;
    }

    /// <summary>
    /// Действия по переходу к экрану результатов
    /// </summary>
    private void ProceedToResultsScreen()
    {
      _resultsScreenComponent = ActualLinkedObjectPoolSupportData.LinkedPoolManager.GetObject<ResultsScreenComponent>(
        typeof(ResultsScreenComponent)).Init(ParentGameObject, _actualPlayer);

      _resultsScreenComponent.ResultsScreenClosing += ResultsScreenEnd;
    }

    /// <summary>
    /// Обработчик окончания работы экрана результатов в случае успешного прохождения уровня
    /// </summary>
    private void ResultsScreenEnd()
    {
      if (_resultsScreenComponent.FinalChoice == EResultsScreenFinalChoice.ContinuePlaying)
      {
        ProceedToTheNextLevel();
      }
      else
      {
        BankAndEndSession();
      }
    }

    /// <summary>
    /// Уничтожение экрана результатов в случае успешного прохождения уровня
    /// </summary>
    private void DestroyResultsScreen()
    {
      if (_resultsScreenComponent != null)
      {
        _resultsScreenComponent.ResultsScreenClosing -= ResultsScreenEnd;
        _resultsScreenComponent.DisableAndSendToPool();
        _resultsScreenComponent = null;
      }
    }

    /// <summary>
    /// Переход к следующему уровню
    /// </summary>
    private void ProceedToTheNextLevel()
    {
      _actualPlayer.LifeCount += LIVES_PER_LEVEL;
      DestroySessionData();
      DestroyResultsScreen();

      StartLevel(GetNextLevel(_startingLevel));
    }

    /// <summary>
    /// Получить следующий уровень из ротации
    /// </summary>
    /// <param name="parPrevious">Данные предыдущего уровня</param>
    /// <returns></returns>
    private GameLevel GetNextLevel(GameLevel parPrevious)
    {
      int index = GameConsts.AvailableGameLevels.IndexOf(parPrevious) + 1;
      if (index >= GameConsts.AvailableGameLevels.Count)
      {
        index = 0;
      }

      return GameConsts.AvailableGameLevels[index];
    }

    /// <summary>
    /// Действия по полному завершению игровой сессии с сохранением результата игрока
    /// </summary>
    private void BankAndEndSession()
    {
      DestroySessionData();

      EndGameSession();
    }

    /// <summary>
    /// Уничтожение данных игровой сессии
    /// </summary>
    private void DestroySessionData()
    {
      if (_currentLevelGameObject != null)
      {
        _currentPocketViewProvider?.DisableAndSendToPool();
        _currentLevelGameObject.DisableAndSendToPool();
        _currentLevelGameObject = null;
      }
    }

    /// <summary>
    /// Полное завершение игровой сессии для последующего перехода обратно в главное меню
    /// </summary>
    private void EndGameSession()
    {
      if (_gameOverComponent != null)
      {
        _gameOverComponent.OnScreenConfirmed -= EndGameSession;
        _gameOverComponent.DisableAndSendToPool();
        _gameOverComponent = null;
      }

      //same disposal for the results screen
      DestroyResultsScreen();


      OnGameSessionEnd?.Invoke();
    }
  }
}