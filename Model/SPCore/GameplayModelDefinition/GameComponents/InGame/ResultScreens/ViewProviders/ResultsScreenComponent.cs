using System;
using System.Linq;
using CoreUtil.Pool;
using Model.SPCore.DS;
using Model.SPCore.GameplayModelDefinition.GameComponents.ViewProviderBase;
using Model.SPCore.GameplayModelDefinition.ObjectModel;
using Model.SPCore.GameplayModelDefinition.SidePocketGame.Data;
using Model.SPCore.Managers.Serialization;
using Model.SPCore.Managers.Sound.Data;
using Model.SPCore.MGameActions;
using Model.SPCore.MPlayers;

namespace Model.SPCore.GameplayModelDefinition.GameComponents.InGame.ResultScreens.ViewProviders
{
  /// <summary>
  /// Экран результата игры - уровень пройден
  /// </summary>
  public class ResultsScreenComponent : ViewProviderComponent
  {
    /// <summary>
    /// Текущая строка вводимого имени игрока
    /// </summary>
    public string CurrentPlayerName { get; set; }
    /// <summary>
    /// Текущее состояние экрана результатов
    /// </summary>
    public EResultsScreenStage CurrentStage { get; set; }
    /// <summary>
    /// Выбранное игроком дальнейшее действие
    /// </summary>
    public EResultsScreenFinalChoice FinalChoice { get; set; }

    /// <summary>
    /// Активен ли экран результатов
    /// </summary>
    public bool IsAlive { get; set; }

    /// <summary>
    /// Выбрана ли сейчас опция выхода и сохранения очков
    /// </summary>
    public bool IsBankAndExitActionSelected { get; set; } = false;
    
    /// <summary>
    /// Ссылка на главного управляющейго игрока
    /// </summary>
    private MPlayer _mainPlayer;

    /// <summary>
    /// Текущие актуальные игровые данные игрока
    /// </summary>
    public Player PlayerActualData { get; set; }

    /// <summary>
    /// Ссылка на менеджер игроков
    /// </summary>
    private MPlayersManager _playersManager;

    /// <summary>
    /// Конструктор для поддержки системы пулинга объектов
    /// </summary>
    /// <param name="parSupportData">Данные для работы с пулом</param>
    public ResultsScreenComponent(ObjectPoolSupportData parSupportData) : base(parSupportData)
    {
    }

    /// <summary>
    /// Событие завершения работы экрана результатов
    /// </summary>
    public event Action ResultsScreenClosing;

    /// <summary>
    /// Замена конструктора, процедура инициализации компонента
    /// </summary>
    /// <param name="parEntGameObject">Родительский игровой объект</param>
    public ResultsScreenComponent Init(GameObject parEntGameObject, Player parPlayer)
    {
      base.Init(parEntGameObject, false, true);

      parEntGameObject.LinkedAppModel.GetSoundManager().PlayBgMusic(EAppMusicAssets.LevelResults, true);

      CurrentStage = EResultsScreenStage.ChooseNextAction;
      FinalChoice = EResultsScreenFinalChoice.None;
      IsBankAndExitActionSelected = false;
      CurrentPlayerName = "";

      _playersManager = parEntGameObject.LinkedAppModel.GetPlayersManager();
      _mainPlayer = _playersManager.Player1;

      PlayerActualData = parPlayer;

      IsAlive = true;

      return this;
    }

    /// <summary>
    /// Сигнал фиксированного обновления модели (используется преимущественно для обработки физики и ввода)
    /// </summary>
    /// <param name="parFixedDeltaTime">Время шага фиксированного обновления в секундах</param>
    public override void FixedUpdate(double parFixedDeltaTime)
    {
      switch (CurrentStage)
      {
        case EResultsScreenStage.ChooseNextAction:
        {
          if (_playersManager.IsActionButtonPressed(_mainPlayer))
          {
            if (IsBankAndExitActionSelected)
            {
              //check if can get into the records
              if (ParentGameObject.LinkedAppModel.GetRecordsData().PlayerRecordsInfo.Last().PointsEarned <
                  PlayerActualData.Score)
              {
                //can be added to the records
                _playersManager.ClearLastPressedKeyboardKey();
                CurrentStage = EResultsScreenStage.ChooseNameForTheRecords;
              }
              else
              {
                ScreenClose(EResultsScreenFinalChoice.BankAndExit);
              }
            }
            else
            {
              ScreenClose(EResultsScreenFinalChoice.ContinuePlaying);
            }
          }
          else
          {
            if (_playersManager.IsButtonPressed(_mainPlayer, EGameActionButton.Dpad_Menu_Up) ||
                _playersManager.IsButtonPressed(_mainPlayer, EGameActionButton.Dpad_Menu_Down))
            {
              IsBankAndExitActionSelected = !IsBankAndExitActionSelected;
            }
          }

          break;
        }

        case EResultsScreenStage.ChooseNameForTheRecords:
        {
          if (_playersManager.IsButtonPressed(_mainPlayer, EGameActionButton.Button_Start))
          {
            //confirm name and place to the records
            ParentGameObject.LinkedAppModel.GetRecordsData()
              .TryAddRecord(new RecordPlayerInfo(PlayerActualData.Score, CurrentPlayerName));
            AppSerializationManager.SaveDataToFile(ParentGameObject.LinkedAppModel.GetRecordsData(),
              ParentGameObject.LinkedAppModel.GetRecordsDataPath());
            ScreenClose(EResultsScreenFinalChoice.BankAndExit);
          }
          else
          {
            if (_playersManager.LastPressedKeyboardKey != "")
            {
              if (_playersManager.LastPressedKeyboardKey == MPlayersManager.SPECIAL_BACKSPACE_SIGNATURE)
              {
                if (CurrentPlayerName.Length > 0)
                {
                  CurrentPlayerName = CurrentPlayerName.Remove(CurrentPlayerName.Length - 1, 1);
                }
              }
              else
              {
                if (CurrentPlayerName.Length < RecordPlayerInfo.MAX_CHARS_IN_NAME)
                {
                  CurrentPlayerName = CurrentPlayerName + _playersManager.LastPressedKeyboardKey;
                }
              }

              _playersManager.ClearLastPressedKeyboardKey();
            }
          }

          break;
        }

        default:
          throw new ArgumentOutOfRangeException();
      }
    }

    /// <summary>
    /// Осущесвтляет закрытие экрана результатов
    /// </summary>
    /// <param name="parChoice">Итоговое выбранное игроком действие на выходе</param>
    private void ScreenClose(EResultsScreenFinalChoice parChoice)
    {
      IsAlive = false;
      FinalChoice = parChoice;
      ResultsScreenClosing?.Invoke();
    }
  }
}