using System;
using System.Collections.Generic;
using System.Linq;
using CoreUtil.Math;
using Model.SPCore.GameplayModelDefinition.SidePocketGame.Data;
using Model.SPCore.GameplayModelDefinition.SidePocketGame.Data.Setup;
using Model.SPCore.Managers.Sound.Data;

namespace Model.SPCore.Consts
{
  /// <summary>
  /// Константы, связанные с игровым процессом
  /// </summary>
  public static class GameConsts
  {
    /// <summary>
    /// Стандартное расстояние между шарами для расстановки
    /// </summary>
    public const int BALLS_SETUP_BASE_DISTANCE = 9;

    /// <summary>
    /// Удвоенное стандартное расстояние между шарами для расстановки
    /// </summary>
    public const int DOUBLED_BALLS_SETUP_BASE_DISTANCE = BALLS_SETUP_BASE_DISTANCE * 2;

    /// <summary>
    /// Ширина игрового стола
    /// </summary>
    public static readonly int FieldWidth = 268;

    /// <summary>
    /// Длина игрового стола
    /// </summary>
    public static readonly int FieldHeight = 160;

    /// <summary>
    /// Стандартная стартовая позиция белого шара игрока
    /// </summary>
    public static readonly SpVector3 StartWhiteBallPlayerPos = new SpVector3(189, FieldHeight - 79, 0);

    /// <summary>
    /// Стандартный игровой стол
    /// </summary>
    public static readonly GameField StandardGameField;

    /// <summary>
    /// Доступные игровые уровни
    /// </summary>
    public static readonly List<GameLevel> AvailableGameLevels = new List<GameLevel>();

    /// <summary>
    /// Статический конструктор
    /// </summary>
    static GameConsts()
    {
      LinkedList<CollisionLine> collisionLines = new LinkedList<CollisionLine>();

      //низ, левая сторона
      SpVector3 lineNormalBottomLeftSide = new SpVector3(0, 1, 0).Normalize();
      Func<BallModelData, bool> lineCheckFuncBottomLeftSide = parData =>
      {
        if (parData.Center.X + parData.Radius < 32 && parData.Center.X - parData.Radius > 122)
        {
          return false;
        }

        if (parData.Center.Y - parData.Radius > 20)
        {
          return false;
        }

        return true;
      };
      collisionLines.AddLast(new CollisionLine(lineNormalBottomLeftSide, lineCheckFuncBottomLeftSide));

      SpVector3 lineNormalBottomRightSide = new SpVector3(0, 1, 0).Normalize();
      Func<BallModelData, bool> lineCheckFuncBottomRightSide = parData =>
      {
        if (parData.Center.X + parData.Radius < 145 && parData.Center.X - parData.Radius > 235)
        {
          return false;
        }

        if (parData.Center.Y - parData.Radius > 20)
        {
          return false;
        }

        return true;
      };
      collisionLines.AddLast(new CollisionLine(lineNormalBottomRightSide, lineCheckFuncBottomRightSide));

      SpVector3 lineNormalTopLeftSide = new SpVector3(0, -1, 0).Normalize();
      Func<BallModelData, bool> lineCheckFuncTopLeftSide = parData =>
      {
        if (parData.Center.X + parData.Radius < 32 && parData.Center.X - parData.Radius > 122)
        {
          return false;
        }

        if (parData.Center.Y + parData.Radius < FieldHeight - 20)
        {
          return false;
        }

        return true;
      };
      collisionLines.AddLast(new CollisionLine(lineNormalTopLeftSide, lineCheckFuncTopLeftSide));

      SpVector3 lineNormalTopRightSide = new SpVector3(0, -1, 0).Normalize();
      Func<BallModelData, bool> lineCheckFuncTopRightSide = parData =>
      {
        if (parData.Center.X + parData.Radius < 145 && parData.Center.X - parData.Radius > 235)
        {
          return false;
        }

        if (parData.Center.Y + parData.Radius < FieldHeight - 20)
        {
          return false;
        }

        return true;
      };
      collisionLines.AddLast(new CollisionLine(lineNormalTopRightSide, lineCheckFuncTopRightSide));

      SpVector3 lineNormalLeftSide = new SpVector3(1, 0, 0).Normalize();
      Func<BallModelData, bool> lineCheckFuncLeftSide = parData =>
      {
        if (parData.Center.X - parData.Radius > 21)
        {
          return false;
        }

        if (parData.Center.Y + parData.Radius < 32 && parData.Center.Y - parData.Radius > 128)
        {
          return false;
        }

        return true;
      };
      collisionLines.AddLast(new CollisionLine(lineNormalLeftSide, lineCheckFuncLeftSide));

      SpVector3 lineNormalRightSide = new SpVector3(-1, 0, 0).Normalize();
      Func<BallModelData, bool> lineCheckFuncRightSide = parData =>
      {
        if (parData.Center.X + parData.Radius < FieldWidth - 21)
        {
          return false;
        }

        if (parData.Center.Y + parData.Radius < 32 && parData.Center.Y - parData.Radius > 128)
        {
          return false;
        }

        return true;
      };
      collisionLines.AddLast(new CollisionLine(lineNormalRightSide, lineCheckFuncRightSide));


      //низ

      SpVector3 bottomLeftPocketBottomBorderStart = new SpVector3(27, FieldHeight - 143, 0);
      SpVector3 bottomLeftPocketBottomBorderEnd = new SpVector3(32, FieldHeight - 138, 0);
      collisionLines.AddLast(new CollisionLine(bottomLeftPocketBottomBorderStart,
        bottomLeftPocketBottomBorderEnd, false, true));

      SpVector3 bottomLeftPocketTopBorderStart = new SpVector3(16, FieldHeight - 132, 0);
      SpVector3 bottomLeftPocketTopBorderEnd = new SpVector3(21, FieldHeight - 127, 0);
      collisionLines.AddLast(new CollisionLine(bottomLeftPocketTopBorderStart,
        bottomLeftPocketTopBorderEnd, true, false));

      SpVector3 bottomMiddlePocketRightBorderStart = new SpVector3(142, FieldHeight - 143, 0);
      SpVector3 bottomMiddlePocketRightBorderEnd = new SpVector3(145, FieldHeight - 138, 0);
      collisionLines.AddLast(new CollisionLine(bottomMiddlePocketRightBorderStart,
        bottomMiddlePocketRightBorderEnd, false, true));

      SpVector3 bottomMiddlePocketLeftBorderStart = new SpVector3(125, FieldHeight - 143, 0);
      SpVector3 bottomMiddlePocketLeftBorderEnd = new SpVector3(122, FieldHeight - 138, 0);
      collisionLines.AddLast(new CollisionLine(bottomMiddlePocketLeftBorderStart,
        bottomMiddlePocketLeftBorderEnd, true, true));

      SpVector3 bottomRightPocketBottomBorderStart = new SpVector3(240, FieldHeight - 143, 0);
      SpVector3 bottomRightPocketBottomBorderEnd = new SpVector3(235, FieldHeight - 138, 0);
      collisionLines.AddLast(new CollisionLine(bottomRightPocketBottomBorderStart,
        bottomRightPocketBottomBorderEnd, true, true));

      SpVector3 bottomRightPocketTopBorderStart = new SpVector3(251, FieldHeight - 132, 0);
      SpVector3 bottomRightPocketTopBorderEnd = new SpVector3(246, FieldHeight - 127, 0);
      collisionLines.AddLast(new CollisionLine(bottomRightPocketTopBorderStart,
        bottomRightPocketTopBorderEnd, false, false));


      //верх

      SpVector3 topLeftPocketBottomBorderStart = new SpVector3(21, FieldHeight - 32, 0);
      SpVector3 topLeftPocketBottomBorderEnd = new SpVector3(15, FieldHeight - 26, 0);
      collisionLines.AddLast(new CollisionLine(topLeftPocketBottomBorderStart,
        topLeftPocketBottomBorderEnd, true, true));

      SpVector3 topLeftPocketTopBorderStart = new SpVector3(32, FieldHeight - 21, 0);
      SpVector3 topLeftPocketTopBorderEnd = new SpVector3(26, FieldHeight - 15, 0);
      collisionLines.AddLast(new CollisionLine(topLeftPocketTopBorderStart,
        topLeftPocketTopBorderEnd, false, false));

      SpVector3 topMiddlePocketRightBorderStart = new SpVector3(145, FieldHeight - 21, 0);
      SpVector3 topMiddlePocketRightBorderEnd = new SpVector3(142, FieldHeight - 15, 0);
      collisionLines.AddLast(new CollisionLine(topMiddlePocketRightBorderStart,
        topMiddlePocketRightBorderEnd, false, false));

      SpVector3 topMiddlePocketLeftBorderStart = new SpVector3(122, FieldHeight - 21, 0);
      SpVector3 topMiddlePocketLeftBorderEnd = new SpVector3(125, FieldHeight - 15, 0);
      collisionLines.AddLast(new CollisionLine(topMiddlePocketLeftBorderStart,
        topMiddlePocketLeftBorderEnd, true, false));

      SpVector3 topRightPocketBottomBorderStart = new SpVector3(246, FieldHeight - 32, 0);
      SpVector3 topRightPocketBottomBorderEnd = new SpVector3(252, FieldHeight - 26, 0);
      collisionLines.AddLast(new CollisionLine(topRightPocketBottomBorderStart,
        topRightPocketBottomBorderEnd, false, true));

      SpVector3 topRightPocketTopBorderStart = new SpVector3(235, FieldHeight - 21, 0);
      SpVector3 topRightPocketTopBorderEnd = new SpVector3(241, FieldHeight - 15, 0);
      collisionLines.AddLast(new CollisionLine(topRightPocketTopBorderStart,
        topRightPocketTopBorderEnd, true, false));


      StandardGameField = new GameField(268, 160, new Pocket[]
      {
        new Pocket(new SpVector3(16, 16, 0)), new Pocket(new SpVector3(134, 11, 0)),
        new Pocket(new SpVector3(252, 16, 0)),
        new Pocket(new SpVector3(16, 144, 0)), new Pocket(new SpVector3(134, 148, 0)),
        new Pocket(new SpVector3(252, 144, 0))
      }, collisionLines.ToArray());


      //добавляем игровые уровни

      AvailableGameLevels = new List<GameLevel>()
      {
        new GameLevel("Los Angeles", new LevelStartConfig(StandardGameField, new BallStartConfig[]
          {
            new BallStartConfig(EBallType.Yellow, new SpVector3(90, FieldHeight - 80)),
            new BallStartConfig(EBallType.Blue,
              new SpVector3(90 - BALLS_SETUP_BASE_DISTANCE * 1,
                FieldHeight - (80 + BALLS_SETUP_BASE_DISTANCE / 2 * 1))),
            new BallStartConfig(EBallType.Red,
              new SpVector3(90 - BALLS_SETUP_BASE_DISTANCE * 1,
                FieldHeight - (80 - BALLS_SETUP_BASE_DISTANCE / 2 * 1))),
            new BallStartConfig(EBallType.Purple,
              new SpVector3(90 - BALLS_SETUP_BASE_DISTANCE * 2,
                FieldHeight - (80 + BALLS_SETUP_BASE_DISTANCE * 1))),
            new BallStartConfig(EBallType.Orange,
              new SpVector3(90 - BALLS_SETUP_BASE_DISTANCE * 2, FieldHeight - (80))),
            new BallStartConfig(EBallType.Green,
              new SpVector3(90 - BALLS_SETUP_BASE_DISTANCE * 2,
                FieldHeight - (80 - BALLS_SETUP_BASE_DISTANCE * 1)))
          },
          new BallStartConfig(EBallType.WhitePlayerBall, StartWhiteBallPlayerPos),
          EAppMusicAssets.SPLevel1
        )),
        new GameLevel("Las Vegas", new LevelStartConfig(StandardGameField, new BallStartConfig[]
          {
            new BallStartConfig(EBallType.Yellow, new SpVector3(100, FieldHeight - 80)),
            new BallStartConfig(EBallType.Blue,
              new SpVector3(100 - BALLS_SETUP_BASE_DISTANCE * 1,
                FieldHeight - (80 + BALLS_SETUP_BASE_DISTANCE / 2 * 1))),
            new BallStartConfig(EBallType.Red,
              new SpVector3(100 - BALLS_SETUP_BASE_DISTANCE * 1,
                FieldHeight - (80 - BALLS_SETUP_BASE_DISTANCE / 2 * 1))),
            new BallStartConfig(EBallType.Purple,
              new SpVector3(100 - BALLS_SETUP_BASE_DISTANCE * 2,
                FieldHeight - (80 + BALLS_SETUP_BASE_DISTANCE * 1))),
            new BallStartConfig(EBallType.Orange,
              new SpVector3(100 - BALLS_SETUP_BASE_DISTANCE * 2, FieldHeight - (80))),
            new BallStartConfig(EBallType.Green,
              new SpVector3(100 - BALLS_SETUP_BASE_DISTANCE * 2,
                FieldHeight - (80 - BALLS_SETUP_BASE_DISTANCE * 1))),
            new BallStartConfig(EBallType.Brown,
              new SpVector3(100 - BALLS_SETUP_BASE_DISTANCE * 3,
                FieldHeight - (80 + BALLS_SETUP_BASE_DISTANCE / 2 * 1))),
            new BallStartConfig(EBallType.Black,
              new SpVector3(100 - BALLS_SETUP_BASE_DISTANCE * 3,
                FieldHeight - (80 - BALLS_SETUP_BASE_DISTANCE / 2 * 1))),
            new BallStartConfig(EBallType.Yellow9Ball,
              new SpVector3(100 - BALLS_SETUP_BASE_DISTANCE * 4,
                FieldHeight - 80))
          },
          new BallStartConfig(EBallType.WhitePlayerBall, StartWhiteBallPlayerPos),
          EAppMusicAssets.SPLevel2)),
        new GameLevel("San Francisco", new LevelStartConfig(StandardGameField, new BallStartConfig[]
          {
            new BallStartConfig(EBallType.Yellow, new SpVector3(110, FieldHeight - 80)),
            new BallStartConfig(EBallType.Blue,
              new SpVector3(110 - BALLS_SETUP_BASE_DISTANCE * 1,
                FieldHeight - (80 + BALLS_SETUP_BASE_DISTANCE / 2 * 1))),
            new BallStartConfig(EBallType.Red,
              new SpVector3(110 - BALLS_SETUP_BASE_DISTANCE * 1,
                FieldHeight - (80 - BALLS_SETUP_BASE_DISTANCE / 2 * 1))),
            new BallStartConfig(EBallType.Purple,
              new SpVector3(110 - BALLS_SETUP_BASE_DISTANCE * 2,
                FieldHeight - (80 + BALLS_SETUP_BASE_DISTANCE * 1))),
            new BallStartConfig(EBallType.Orange,
              new SpVector3(110 - BALLS_SETUP_BASE_DISTANCE * 2, FieldHeight - (80))),
            new BallStartConfig(EBallType.Green,
              new SpVector3(110 - BALLS_SETUP_BASE_DISTANCE * 2,
                FieldHeight - (80 - BALLS_SETUP_BASE_DISTANCE * 1)))
          },
          new BallStartConfig(EBallType.WhitePlayerBall, StartWhiteBallPlayerPos), EAppMusicAssets.SPLevel3)),
        new GameLevel("New York", new LevelStartConfig(StandardGameField, new BallStartConfig[]
          {
            new BallStartConfig(EBallType.Yellow, new SpVector3(80, FieldHeight - 80)),
            new BallStartConfig(EBallType.Blue,
              new SpVector3(80 - DOUBLED_BALLS_SETUP_BASE_DISTANCE * 1,
                FieldHeight - (80 + DOUBLED_BALLS_SETUP_BASE_DISTANCE / 2 * 1))),
            new BallStartConfig(EBallType.Red,
              new SpVector3(80 - DOUBLED_BALLS_SETUP_BASE_DISTANCE * 1,
                FieldHeight - (80 - DOUBLED_BALLS_SETUP_BASE_DISTANCE / 2 * 1))),
            new BallStartConfig(EBallType.Purple,
              new SpVector3(80 - DOUBLED_BALLS_SETUP_BASE_DISTANCE * 2,
                FieldHeight - (80 + DOUBLED_BALLS_SETUP_BASE_DISTANCE * 1))),
            new BallStartConfig(EBallType.Orange,
              new SpVector3(80 - DOUBLED_BALLS_SETUP_BASE_DISTANCE * 2, FieldHeight - (80))),
            new BallStartConfig(EBallType.Green,
              new SpVector3(80 - DOUBLED_BALLS_SETUP_BASE_DISTANCE * 2,
                FieldHeight - (80 - DOUBLED_BALLS_SETUP_BASE_DISTANCE * 1)))
          },
          new BallStartConfig(EBallType.WhitePlayerBall, StartWhiteBallPlayerPos), EAppMusicAssets.SPLevel4)),

        new GameLevel("Atlantic City", new LevelStartConfig(StandardGameField, new BallStartConfig[]
          {
            new BallStartConfig(EBallType.Yellow, new SpVector3(100, FieldHeight - 80)),
            new BallStartConfig(EBallType.Blue,
              new SpVector3(100 - BALLS_SETUP_BASE_DISTANCE * 1,
                FieldHeight - (80 + BALLS_SETUP_BASE_DISTANCE / 2 * 1))),
            new BallStartConfig(EBallType.Red,
              new SpVector3(100 - BALLS_SETUP_BASE_DISTANCE * 1,
                FieldHeight - (80 - BALLS_SETUP_BASE_DISTANCE / 2 * 1))),
            new BallStartConfig(EBallType.Purple,
              new SpVector3(100 - BALLS_SETUP_BASE_DISTANCE * 2,
                FieldHeight - (80 + BALLS_SETUP_BASE_DISTANCE * 1))),
            new BallStartConfig(EBallType.Orange,
              new SpVector3(100 - BALLS_SETUP_BASE_DISTANCE * 2, FieldHeight - (80))),
            new BallStartConfig(EBallType.Green,
              new SpVector3(100 - BALLS_SETUP_BASE_DISTANCE * 2,
                FieldHeight - (80 - BALLS_SETUP_BASE_DISTANCE * 1))),

            new BallStartConfig(EBallType.Brown,
              new SpVector3(100 - BALLS_SETUP_BASE_DISTANCE * 3,
                FieldHeight - (80 + BALLS_SETUP_BASE_DISTANCE * 2))),
            new BallStartConfig(EBallType.Black,
              new SpVector3(100 - BALLS_SETUP_BASE_DISTANCE * 3, FieldHeight - (80 + BALLS_SETUP_BASE_DISTANCE * 1))),
            new BallStartConfig(EBallType.Yellow9Ball,
              new SpVector3(100 - BALLS_SETUP_BASE_DISTANCE * 3,
                FieldHeight - (80 - BALLS_SETUP_BASE_DISTANCE * 1))),
            new BallStartConfig(EBallType.Blue10Ball,
              new SpVector3(100 - BALLS_SETUP_BASE_DISTANCE * 3,
                FieldHeight - (80 - BALLS_SETUP_BASE_DISTANCE * 2))),
          },
          new BallStartConfig(EBallType.WhitePlayerBall, StartWhiteBallPlayerPos),
          EAppMusicAssets.SPLevel5)),
        new GameLevel("Test Area", new LevelStartConfig(StandardGameField, new BallStartConfig[]
        {
          new BallStartConfig(EBallType.Yellow, new SpVector3(16, 16, 0)),
        }, new BallStartConfig(EBallType.WhitePlayerBall, StartWhiteBallPlayerPos), EAppMusicAssets.SPLevel1))
      };
    }
  }
}