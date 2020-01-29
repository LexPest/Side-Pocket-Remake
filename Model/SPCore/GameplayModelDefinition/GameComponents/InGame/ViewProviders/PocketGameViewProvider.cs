using System;
using System.Collections.Generic;
using System.Linq;
using CoreUtil.Math;
using CoreUtil.Pool;
using Model.SPCore.GameplayModelDefinition.GameComponents.ViewProviderBase;
using Model.SPCore.GameplayModelDefinition.ObjectModel;
using Model.SPCore.GameplayModelDefinition.SidePocketGame.Data;
using Model.SPCore.GameplayModelDefinition.SidePocketGame.Data.Setup;
using Model.SPCore.Managers.Sound.Data;
using Model.SPCore.MGameActions;
using Model.SPCore.MPlayers;

namespace Model.SPCore.GameplayModelDefinition.GameComponents.InGame.ViewProviders
{
  public class PocketGameViewProvider : ViewProviderComponent
  {
    /// <summary>
    /// Стандартная скорость изменения положения прицела
    /// </summary>
    private const double CHANGE_AIM_ANGLE_SPEED_DEFAULT = 70;

    /// <summary>
    /// Скорость изменения положения прицела в режиме с повышенной точностью
    /// </summary>
    private const double CHANGE_AIM_ANGLE_SPEED_PRECISE = 25;

    /// <summary>
    /// Угол вверх
    /// </summary>
    private const double AIM_ANGLE_UP = 90;

    /// <summary>
    /// Угол влево
    /// </summary>
    private const double AIM_ANGLE_LEFT = 180;

    /// <summary>
    /// Угол вниз
    /// </summary>
    private const double AIM_ANGLE_BOTTOM = 270;

    /// <summary>
    /// Угол вправо
    /// </summary>
    private const double AIM_ANGLE_RIGHT = 0;

    /// <summary>
    /// Погрешность при сравнении чисел с плавающей запятой
    /// </summary>
    public const double COMPARISON_TOLERANCE = 1E-04;

    /// <summary>
    /// Переходная константа угла через 360/0 градусов
    /// </summary>
    private const double AIM_ANGLE_CONVERTION_THRESHOLD = 359;

    /// <summary>
    /// Максимальная сила удара кием по шару
    /// </summary>
    public const double POWER_MAX = 400;

    /// <summary>
    /// Минимальная сила удара кием по шару
    /// </summary>
    public const double POWER_MIN = 20;

    /// <summary>
    /// Скорость изменения силы удара кием по шару
    /// </summary>
    public const double POWER_CHANGE_SPEED = 350;

    /// <summary>
    /// Величина округления и приближения угла
    /// </summary>
    public const double ANGLE_APPROXIMATION = 2.5;

    /// <summary>
    /// Начальный выбранный угол удара игроком во время прицеливания 
    /// </summary>
    public const double START_PLAYER_ANGLE = 180;

    /// <summary>
    /// Текущий выбранный угол удара игроком во время прицеливания
    /// </summary>
    private double _playerCurrentAimingAngle = 180;

    /// <summary>
    /// Основной игрок
    /// </summary>
    public Player ActualPlayer;

    /// <summary>
    /// Шары на столе
    /// </summary>
    public List<BallModelData> BallsInGame;

    /// <summary>
    /// Шары в лузах
    /// </summary>
    public List<BallModelData> BallsPocketed;

    /// <summary>
    /// Текущее состояние игровой сессии
    /// </summary>
    public EPocketGameState CurrentGameState;

    /// <summary>
    /// Результат окончания игрового уровня
    /// </summary>
    public ELevelEndResult EndLevelResult;

    /// <summary>
    /// Игровой стол
    /// </summary>
    public GameField Field;

    /// <summary>
    /// Флаг - хотя бы один шар был забит во время последнего удара
    /// </summary>
    private bool _flagAtLeastOneBallIsPocketed = false;

    /// <summary>
    /// Для изменения текущей силы удара: в обратном ли направлении она сейчас меняется?
    /// </summary>
    private bool _forceBackwardsDirection = false;

    /// <summary>
    /// Признак готовности игровой сессии к началу игры
    /// </summary>
    public bool GameReady = false;

    /// <summary>
    /// Стартовая конфигурация игрового уровня
    /// </summary>
    private LevelStartConfig _levelConfig;

    /// <summary>
    /// Выбранная сила удара игроком
    /// </summary>
    public double PlayerChosenShotForce = 0;

    /// <summary>
    /// Ссылка на менеджер ввода игроков
    /// </summary>
    private MPlayersManager _playersInputManager;

    /// <summary>
    /// Белый шар игрока
    /// </summary>
    public BallModelData PlayerWhiteBall;

    /// <summary>
    /// Конструктор для поддержки системы пулинга объектов
    /// </summary>
    /// <param name="parSupportData">Данные для работы с пулом</param>
    public PocketGameViewProvider(ObjectPoolSupportData parSupportData) : base(parSupportData)
    {
    }

    /// <summary>
    /// Получить силу удара в процентах
    /// </summary>
    /// <returns></returns>
    public double GetShotForcePercent()
    {
      return (PlayerChosenShotForce - POWER_MIN) / (POWER_MAX - POWER_MIN);
    }

    /// <summary>
    /// Получить округленный, приближенный угол
    /// </summary>
    /// <returns></returns>
    public double GetAngleApproximated()
    {
      return Math.Floor(PlayerCurrentAimingAngle / ANGLE_APPROXIMATION) * ANGLE_APPROXIMATION;
    }

    /// <summary>
    /// События завершения игрового уровня
    /// </summary>
    public event Action GameLevelEnd;

    /// <summary>
    /// Замена конструктора, процедура инициализации компонента
    /// </summary>
    /// <param name="parEntGameObject">Родительский игровой объект</param>
    /// <param name="parLevelStartConfig">Стартовая конфигурация игрового уровня</param>
    public PocketGameViewProvider Init(GameObject parEntGameObject, LevelStartConfig parLevelStartConfig,
      Player parPlayerData)
    {
      base.Init(parEntGameObject, false, true);

      _levelConfig = parLevelStartConfig;

      Field = parLevelStartConfig.LevelGameField;

      BallsPocketed = new List<BallModelData>();
      BallsInGame = new List<BallModelData>();

      foreach (var ball in parLevelStartConfig.LevelBalls)
      {
        BallsInGame.Add(new BallModelData(ball.BallType)
        {
          Center = ball.StartCenterPosition
        });
      }

      PlayerWhiteBall = new BallModelData(parLevelStartConfig.PlayerWhiteBall.BallType)
      {
        Center = parLevelStartConfig.PlayerWhiteBall.StartCenterPosition
      };

      BallsInGame.Add(PlayerWhiteBall);

      ActualPlayer = parPlayerData;

      PlayerCurrentAimingAngle = START_PLAYER_ANGLE;
      PlayerChosenShotForce = POWER_MIN;

      CurrentGameState = EPocketGameState.Init;

      _playersInputManager = parEntGameObject.LinkedAppModel.GetPlayersManager();

      EndLevelResult = ELevelEndResult.None;

      GameReady = true;
      return this;
    }

    /// <summary>
    /// Начать игру на уровне
    /// </summary>
    public void StartGameLevel()
    {
      Console.WriteLine("Game level started");
      CurrentGameState = EPocketGameState.Aiming;
      ParentGameObject.LinkedAppModel.GetSoundManager().PlayBgMusic(_levelConfig.LevelBackgroundMusic, true);
    }

    /// <summary>
    /// Сигнал фиксированного обновления модели (используется преимущественно для обработки физики и ввода)
    /// </summary>
    /// <param name="parFixedDeltaTime">Время шага фиксированного обновления в секундах</param>
    public override void FixedUpdate(double parFixedDeltaTime)
    {
      base.FixedUpdate(parFixedDeltaTime);

      switch (CurrentGameState)
      {
        case EPocketGameState.Init:
        {
          break;
        }

        case EPocketGameState.Aiming:
        {
          _forceBackwardsDirection = false;

          double currentAimChangeSpeed =
            _playersInputManager.IsButtonHolding(ActualPlayer.PlayerInput, EGameActionButton.Button_Bumper_Shift)
              ? CHANGE_AIM_ANGLE_SPEED_PRECISE
              : CHANGE_AIM_ANGLE_SPEED_DEFAULT;

          //вверх, влево, вниз, вправо для изменения угла удара
          if (_playersInputManager.IsButtonHolding(ActualPlayer.PlayerInput, EGameActionButton.Dpad_Menu_Up))
          {
            //наверх
            if (Math.Abs(PlayerCurrentAimingAngle - AIM_ANGLE_UP) < COMPARISON_TOLERANCE)
            {
              // готово
            }
            else if (PlayerCurrentAimingAngle > AIM_ANGLE_UP &&
                     PlayerCurrentAimingAngle <= AIM_ANGLE_BOTTOM)
            {
              PlayerCurrentAimingAngle -= parFixedDeltaTime * currentAimChangeSpeed;
              if (PlayerCurrentAimingAngle < AIM_ANGLE_UP)
              {
                PlayerCurrentAimingAngle = AIM_ANGLE_UP;
              }
            }
            else if (PlayerCurrentAimingAngle < AIM_ANGLE_UP)
            {
              PlayerCurrentAimingAngle += parFixedDeltaTime * currentAimChangeSpeed;
              if (PlayerCurrentAimingAngle > AIM_ANGLE_UP)
              {
                PlayerCurrentAimingAngle = AIM_ANGLE_UP;
              }
            }
            else if (PlayerCurrentAimingAngle > AIM_ANGLE_BOTTOM &&
                     PlayerCurrentAimingAngle <= AIM_ANGLE_CONVERTION_THRESHOLD)
            {
              PlayerCurrentAimingAngle = PlayerCurrentAimingAngle + parFixedDeltaTime * currentAimChangeSpeed;
            }
          }
          else if (_playersInputManager.IsButtonHolding(ActualPlayer.PlayerInput,
            EGameActionButton.Dpad_Menu_Left))
          {
            //налево
            if (Math.Abs(PlayerCurrentAimingAngle - AIM_ANGLE_LEFT) < COMPARISON_TOLERANCE)
            {
              //готово
            }
            else if (PlayerCurrentAimingAngle < AIM_ANGLE_LEFT)
            {
              PlayerCurrentAimingAngle += parFixedDeltaTime * currentAimChangeSpeed;
              if (PlayerCurrentAimingAngle > AIM_ANGLE_LEFT)
              {
                PlayerCurrentAimingAngle = AIM_ANGLE_LEFT;
              }
            }
            else if (PlayerCurrentAimingAngle > AIM_ANGLE_LEFT)
            {
              PlayerCurrentAimingAngle -= parFixedDeltaTime * currentAimChangeSpeed;
              if (PlayerCurrentAimingAngle < AIM_ANGLE_LEFT)
              {
                PlayerCurrentAimingAngle = AIM_ANGLE_LEFT;
              }
            }
          }
          else if (_playersInputManager.IsButtonHolding(ActualPlayer.PlayerInput,
            EGameActionButton.Dpad_Menu_Down))
          {
            //вниз
            if (Math.Abs(PlayerCurrentAimingAngle - AIM_ANGLE_BOTTOM) < COMPARISON_TOLERANCE)
            {
              //готово
            }
            else if (PlayerCurrentAimingAngle < AIM_ANGLE_BOTTOM &&
                     PlayerCurrentAimingAngle >= AIM_ANGLE_UP)
            {
              PlayerCurrentAimingAngle += parFixedDeltaTime * currentAimChangeSpeed;
              if (PlayerCurrentAimingAngle > AIM_ANGLE_BOTTOM)
              {
                PlayerCurrentAimingAngle = AIM_ANGLE_BOTTOM;
              }
            }
            else if (PlayerCurrentAimingAngle > AIM_ANGLE_BOTTOM &&
                     PlayerCurrentAimingAngle < AIM_ANGLE_CONVERTION_THRESHOLD)
            {
              PlayerCurrentAimingAngle -= parFixedDeltaTime * currentAimChangeSpeed;
              if (PlayerCurrentAimingAngle < AIM_ANGLE_BOTTOM)
              {
                PlayerCurrentAimingAngle = AIM_ANGLE_BOTTOM;
              }
            }
            else if (PlayerCurrentAimingAngle < AIM_ANGLE_UP)
            {
              PlayerCurrentAimingAngle -= parFixedDeltaTime * currentAimChangeSpeed;
            }
          }
          else if (_playersInputManager.IsButtonHolding(ActualPlayer.PlayerInput,
            EGameActionButton.Dpad_Menu_Right))
          {
            //вправо
            if (Math.Abs(PlayerCurrentAimingAngle - AIM_ANGLE_RIGHT) < COMPARISON_TOLERANCE)
            {
              //готово
            }
            else if (PlayerCurrentAimingAngle <= AIM_ANGLE_LEFT)
            {
              PlayerCurrentAimingAngle -= parFixedDeltaTime * currentAimChangeSpeed;
              if (PlayerCurrentAimingAngle >= AIM_ANGLE_BOTTOM)
              {
                PlayerCurrentAimingAngle = AIM_ANGLE_RIGHT;
              }
            }
            else if (PlayerCurrentAimingAngle > AIM_ANGLE_LEFT)
            {
              PlayerCurrentAimingAngle += parFixedDeltaTime * currentAimChangeSpeed;
              if (_playerCurrentAimingAngle <= AIM_ANGLE_UP)
              {
                PlayerCurrentAimingAngle = AIM_ANGLE_RIGHT;
              }
            }
          }
          else if (_playersInputManager.IsActionButtonPressed(ActualPlayer.PlayerInput))
          {
            PlayerChosenShotForce = POWER_MIN;
            //теперь начинаем выбирать силу удара
            CurrentGameState = EPocketGameState.ChooseShotPower;
          }

          break;
        }

        case EPocketGameState.ChooseShotPower:
        {
          if (!_forceBackwardsDirection)
          {
            if (PlayerChosenShotForce < POWER_MAX)
            {
              PlayerChosenShotForce += parFixedDeltaTime * POWER_CHANGE_SPEED;
              if (PlayerChosenShotForce > POWER_MAX)
              {
                PlayerChosenShotForce = POWER_MAX;
                _forceBackwardsDirection = !_forceBackwardsDirection;
              }
            }
          }
          else
          {
            PlayerChosenShotForce -= parFixedDeltaTime * POWER_CHANGE_SPEED;
            if (PlayerChosenShotForce < POWER_MIN)
            {
              PlayerChosenShotForce = POWER_MIN;
              _forceBackwardsDirection = !_forceBackwardsDirection;
            }
          }

          if (_playersInputManager.IsActionButtonPressed(ActualPlayer.PlayerInput))
          {
            //совершаем удар
            PlayerWhiteBall.Speed = PlayerChosenShotForce;
            PlayerWhiteBall.Velocity =
              Angle.RadiansToVector(Angle.DegreesToRadians(GetAngleApproximated()));
            _flagAtLeastOneBallIsPocketed = false;
            ParentGameObject.LinkedAppModel.GetSoundManager().PlaySfx(EAppSfxAssets.BallShotByCue, false);
            CurrentGameState = EPocketGameState.BallsMovingInProgress;
          }

          break;
        }

        case EPocketGameState.BallsMovingInProgress:
        {
          //подождем пока все завершат свое движение
          if (!UpdateGameField(parFixedDeltaTime))
          {
            //посмотрим, вдруг мы уже выиграли или проиграли
            if (!_flagAtLeastOneBallIsPocketed)
            {
              //уменьшим счетчик жизней
              ActualPlayer.LifeCount--;
            }

            if (ActualPlayer.LifeCount <= 0)
            {
              //мы проиграли...
              EndLevelResult = ELevelEndResult.Lose;
              Console.WriteLine("Player lost!");
            }

            if (BallsInGame.Count == 1)
            {
              if (BallsInGame.First() == PlayerWhiteBall)
              {
                //мы выиграли!
                EndLevelResult = ELevelEndResult.Win;
                Console.WriteLine("Player completed the level!");
              }
              else
              {
                throw new ApplicationException("Game Session state machine is broken!");
              }
            }

            if (EndLevelResult != ELevelEndResult.None)
            {
              CurrentGameState = EPocketGameState.LevelEnd;
            }
            else
            {
              CurrentGameState = EPocketGameState.Aiming;
            }

            GC.Collect();
          }

          break;
        }

        case EPocketGameState.Paused:
        {
          break;
        }

        case EPocketGameState.LevelEnd:
        {
          GameLevelEnd?.Invoke();
          CurrentGameState = EPocketGameState.Init;
          break;
        }

        default:
          throw new ArgumentOutOfRangeException();
      }

      if (CurrentGameState != EPocketGameState.Init)
      {
        ViewUpdateSignal(parFixedDeltaTime);
      }
    }

    /// <summary>
    /// Сигнал обновления модели игрового поля / стола
    /// </summary>
    /// <param name="parFixedDeltaTime">Время шага фиксированного обновления в секундах</param>
    /// <returns></returns>
    internal bool UpdateGameField(double parFixedDeltaTime)
    {
      bool atLeastOneBallIsMoving = false;

      foreach (var ballModelData in BallsInGame)
      {
        if (ballModelData.IsMoving)
        {
          MoveBall(ballModelData, parFixedDeltaTime);
          atLeastOneBallIsMoving = true;
        }
      }

      CheckCollisions();

      return atLeastOneBallIsMoving;
    }

    /// <summary>
    /// Перемещение шара
    /// </summary>
    /// <param name="parBall">Шар</param>
    /// <param name="parFixedDeltaTime">Время шага фиксированного обновления в секундах</param>
    internal void MoveBall(BallModelData parBall, double parFixedDeltaTime)
    {
      parBall.Center.X += parBall.Velocity.X * parBall.Speed * parFixedDeltaTime;
      parBall.Center.Y += parBall.Velocity.Y * parBall.Speed * parFixedDeltaTime;

      double modifierDecreasor = 0.3 * parFixedDeltaTime;
      double modifierActual = 1.00 - modifierDecreasor;

      parBall.Velocity.X = parBall.Velocity.X * modifierActual;
      parBall.Velocity.Y = parBall.Velocity.Y * modifierActual;

      parBall.Speed = parBall.Speed * modifierActual;

      if (Math.Abs(parBall.Velocity.X) < 0.1 && Math.Abs(parBall.Velocity.Y) < 0.1)
      {
        parBall.Velocity.X = 0;
        parBall.Velocity.Y = 0;

        parBall.Speed = 0;
        // ball.IsMoving = false;
      }
    }

    /// <summary>
    /// Обнаружение столкновения между шарами
    /// </summary>
    /// <param name="parBall1">Шар первый</param>
    /// <param name="parBall2">Шар второй</param>
    /// <returns>True если есть столкновение</returns>
    private bool DetectCollision(BallModelData parBall1, BallModelData parBall2)
    {
      SpVector3 ballsDistanceVector = parBall2.Center - parBall1.Center;
      double centersDistance = ballsDistanceVector.Length();

      return (centersDistance <= parBall1.Radius + parBall2.Radius);
    }

    /// <summary>
    /// Обнаружение столкновения с коллайдером лузы
    /// </summary>
    /// <param name="parBall">Шар</param>
    /// <param name="parPocket">Луза</param>
    /// <returns>True если есть столкновение</returns>
    private bool IsPocketCollision(BallModelData parBall, Pocket parPocket)
    {
      SpVector3 ballDistanceVector = parPocket.Center - parBall.Center;
      double centersDistance = ballDistanceVector.Length();

      return (centersDistance <= parBall.Radius + parPocket.Radius);
    }

    /// <summary>
    /// Запуск проверки всех столкновений на всем столе
    /// </summary>
    private void CheckCollisions()
    {
      for (int i = 0; i < BallsInGame.Count; i++)
      {
        BallModelData currentBall = BallsInGame[i];

        if (CheckCollisionsWithBorders(currentBall))
        {
          ParentGameObject.LinkedAppModel.GetSoundManager()
            .PlaySfx(EAppSfxAssets.BallCollisionWallSolid, false);
        }

        for (int j = i + 1; j < BallsInGame.Count; j++)
        {
          if (CheckCollisionsWithBalls(currentBall, BallsInGame[j]))
          {
            ParentGameObject.LinkedAppModel.GetSoundManager().PlaySfx(EAppSfxAssets.BallCollision, false);
          }
        }

        if (CheckIfIsPocketed(currentBall))
        {
          ParentGameObject.LinkedAppModel.GetSoundManager().PlaySfx(EAppSfxAssets.BallPocketed, false);

          if (currentBall != PlayerWhiteBall)
          {
            BallsPocketed.Add(currentBall);
            BallsInGame.Remove(currentBall);

            _flagAtLeastOneBallIsPocketed = true;
            ActualPlayer.Score += 100;
            i--;
          }
          else
          {
            currentBall.ResetMovement();
            currentBall.Center = _levelConfig.PlayerWhiteBall.StartCenterPosition;
            ActualPlayer.Score -= 200;
            ActualPlayer.LifeCount -= 2;
          }
        }
      }
    }

    /// <summary>
    /// Проверка столкновения с бортом
    /// </summary>
    /// <param name="parBall">Шар</param>
    /// <returns></returns>
    private bool CheckCollisionsWithBorders(BallModelData parBall)
    {
      foreach (var fieldCollisionLine in Field.CollisionLines)
      {
        if (fieldCollisionLine.CheckCollisionFunc(parBall))
        {
          //столкновение произошло
          SpVector3 ballVelocity = parBall.Velocity;

          parBall.Velocity = 2 * ballVelocity +
                             2 * fieldCollisionLine.Normal * (-ballVelocity.DotProduct(fieldCollisionLine.Normal)) -
                             ballVelocity;

          do
          {
            parBall.Center = parBall.Center + (fieldCollisionLine.Normal);
          } while (fieldCollisionLine.CheckCollisionFunc(parBall));

          return true;
        }
      }

      return false;
    }

    /// <summary>
    /// Обработка столкновения двух шаров
    /// </summary>
    /// <param name="parBall1">Шар 1</param>
    /// <param name="parBall2">Шар 2</param>
    /// <returns>True, если столкновение имело место быть</returns>
    private bool CheckCollisionsWithBalls(BallModelData parBall1, BallModelData parBall2)
    {
      if (DetectCollision(parBall1, parBall2))
      {
        SpVector3 tmpVelocity = ChangeVelocities(parBall1, parBall2);
        parBall2.Velocity = ChangeVelocities(parBall2, parBall1);
        parBall1.Velocity = tmpVelocity;

        double tmpBallSpeed = Math.Max(parBall2.Speed * 0.8, parBall1.Speed * 0.8);

        double intersectionRadius = -(parBall2.Center - parBall1.Center).Length() + parBall2.Radius + parBall1.Radius;

        if (intersectionRadius <= 0)
        {
          intersectionRadius = 1;
        }

        double posModifier = intersectionRadius / 2;
        parBall1.Center = parBall1.Center + (parBall1.Center - parBall2.Center).Normalize() * (posModifier + 1);
        parBall2.Center = parBall2.Center + (parBall2.Center - parBall1.Center).Normalize() * (posModifier + 1);


        parBall1.Speed = tmpBallSpeed;
        parBall2.Speed = tmpBallSpeed;

        return true;
      }

      return false;
    }

    /// <summary>
    /// Вспомогательный метод для изменения векторов скоростей столкнувшихся шаров
    /// </summary>
    /// <param name="parBall1">Шар 1</param>
    /// <param name="parBall2">Шар 2</param>
    /// <returns>Новый вектор скорости для Шара 1</returns>
    private SpVector3 ChangeVelocities(BallModelData parBall1, BallModelData parBall2)
    {
      SpVector3 centersVector = parBall2.Center - parBall1.Center;
      SpVector3 ballOnePerpendicular = centersVector.PerpendicularComponent(parBall1.Velocity);
      SpVector3 ballTwoPerpendicular = centersVector.PerpendicularComponent(parBall2.Velocity);
      SpVector3 ballTwoParallel = centersVector.ParralelComponent(parBall2.Velocity);

      SpVector3 newBall1Velocity = ballTwoParallel + ballOnePerpendicular;
      return newBall1Velocity;
    }

    /// <summary>
    /// Проверка, не попал ли шар в лузу
    /// </summary>
    /// <param name="parBall">Проверяемый шар</param>
    /// <returns></returns>
    private bool CheckIfIsPocketed(BallModelData parBall)
    {
      foreach (var fieldPocket in Field.Pockets)
      {
        if (IsPocketCollision(parBall, fieldPocket))
        {
          return true;
        }
      }

      return false;
    }

    /// <summary>
    /// Текущий выбранный угол удара игроком во время прицеливания
    /// </summary>
    public double PlayerCurrentAimingAngle
    {
      get { return _playerCurrentAimingAngle; }
      set
      {
        if (value > AIM_ANGLE_CONVERTION_THRESHOLD)
        {
          _playerCurrentAimingAngle = value - AIM_ANGLE_CONVERTION_THRESHOLD;
        }
        else if (value < 0)
        {
          _playerCurrentAimingAngle = AIM_ANGLE_CONVERTION_THRESHOLD + value;
        }
        else
        {
          _playerCurrentAimingAngle = value;
        }
      }
    }
  }
}