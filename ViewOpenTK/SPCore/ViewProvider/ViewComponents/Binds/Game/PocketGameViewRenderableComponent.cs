using System;
using System.Collections.Generic;
using System.Drawing;
using CoreUtil.Math;
using CoreUtil.Pool;
using Model.SPCore.GameplayModelDefinition.GameComponents.InGame;
using Model.SPCore.GameplayModelDefinition.GameComponents.InGame.ViewProviders;
using Model.SPCore.GameplayModelDefinition.GameComponents.ViewProviderBase;
using ViewOpenTK.SPCore.ViewProvider.InternalGraphicalDataStructures;
using ViewOpenTK.SPCore.ViewProvider.RenderingData;

namespace ViewOpenTK.SPCore.ViewProvider.ViewComponents.Binds.Game
{
  /// <summary>
  /// Компонент для рендеринга данных игровой сессии
  /// </summary>
  public class PocketGameViewRenderableComponent : ViewRenderableComponent
  {
    /// <summary>
    /// Название спрайта игрового поля
    /// </summary>
    private const string NORMAL_GAMEFIELD_BG_SPRITE = "/game_field/spr_gamefield_main.png";

    /// <summary>
    /// Название анимации выбора силы удара кием
    /// </summary>
    private const string ANIM_INFOBAR_SHOT = "/infobar_shot_anim/spr_infobar_shot_anim.png";

    /// <summary>
    /// Название спрайта индикатора жизни игрока
    /// </summary>
    private const string LIFE_INDICATOR = "/balls_lives_indicator/spr_alive_ball_indicator.png";

    /// <summary>
    /// Название спрайта индикатора половинки жизни игрока
    /// </summary>
    private const string LIFE_INDICATOR_HALF = "/balls_lives_indicator/spr_alive_ball_indicator_half.png";

    /// <summary>
    /// Название анимации удара кием по шару
    /// </summary>
    private const string SHOT_HAND_CUE_ANIM = "/hand_shot_anim/spr_hand_shot_anim.png";

    /// <summary>
    /// Коэффициент подстройки отображения угла удара
    /// </summary>
    private const double ANGLE_ADJUST_TOLERANCE = 6;

    /// <summary>
    /// Позиция X первой линии счетчика жизней
    /// </summary>
    private const int LIFECOUNTER_LINE1_START_MIDDLE_X = 28;

    /// <summary>
    /// Позиция Y первой линии счетчика жизней
    /// </summary>
    private const int LIFECOUNTER_LINE1_START_MIDDLE_Y = 36;

    /// <summary>
    /// Расстояние между элементами счетчика жизней по оси X
    /// </summary>
    private const int LIFECOUNTER_X_ELEM_OFFSET = 8;

    /// <summary>
    /// Расстояние между элементами счетчика жизней по оси Y
    /// </summary>
    private const int LIFECOUNTER_Y_ELEM_OFFSET = 8;

    /// <summary>
    /// Количество элементов счетчика жизней на одной линии
    /// </summary>
    private const int LIFES_ON_ONE_LINE = 8;

    /// <summary>
    /// Позиция X счетчика очков
    /// </summary>
    private const int SCORE_RIGHT_X_OFFSET = 80;

    /// <summary>
    /// Позиция Y счетчика очков
    /// </summary>
    private const int SCORE_TOP_Y_OFFSET = 20;

    /// <summary>
    /// Максимальный кадр анимации выбора силы удара
    /// </summary>
    private const int ANIM_INFOBAR_MAX_FRAME = 65;

    /// <summary>
    /// Максимальный кадр анимации удара кием по шару
    /// </summary>
    private const int ANIM_HANDSHOT_CUE_MAX_FRAME = 60;

    /// <summary>
    /// Данные рендеринга шаров
    /// </summary>
    private LinkedList<RenderingData.RenderingData?> _ballsRenderingData;

    /// <summary>
    /// Данные о проигрываемой анимации выбора силы удара
    /// </summary>
    private PlayableAnimationObject _choosingShotAnimationInfobar;

    /// <summary>
    /// Данные о проигрываемой анимации удара кием по шару
    /// </summary>
    private PlayableAnimationObject _choosingShotPlayerCueAnimation;

    /// <summary>
    /// Данные рендеринга заднего фона
    /// </summary>
    private RenderingData.RenderingData? _currentRenderingBg;

    /// <summary>
    /// Компонент на стороне модели
    /// </summary>
    private PocketGameViewProvider _modelProviderComponent;

    /// <summary>
    /// Данные рендеринга кия и прицела игрока
    /// </summary>
    private LinkedList<RenderingData.RenderingData?> _playerCueRenderingData;

    /// <summary>
    /// Данные рендеринга данных об игроке
    /// </summary>
    private LinkedList<RenderingData.RenderingData?> _playerScoreAndLifeCounter;

    /// <summary>
    /// Данные для рендеринга панели с выбором силы удара игроком (оверлей)
    /// </summary>
    private RenderingData.RenderingData? _upperShotChoosePanelOverlay;

    /// <summary>
    /// Конструктор для поддержки системы пулинга объектов
    /// </summary>
    /// <param name="parSupportData">Данные для работы с пулом</param>
    public PocketGameViewRenderableComponent(ObjectPoolSupportData parSupportData) : base(parSupportData)
    {
    }

    /// <summary>
    /// Замена конструктора, процедура инициализации компонента отображения
    /// </summary>
    /// <param name="parModelSideProviderComponent">Связанный компонент на стороне модели</param>
    /// <param name="parLinkedEventsHandler">Связанный обработчик событий отображения</param>
    public override void InitAndLink(ViewProviderComponent parModelSideProviderComponent,
      ViewEventsOpenTkHandler parLinkedEventsHandler)
    {
      base.InitAndLink(parModelSideProviderComponent, parLinkedEventsHandler);
      _modelProviderComponent = (PocketGameViewProvider) parModelSideProviderComponent;

      _choosingShotAnimationInfobar =
        new PlayableAnimationObject(ActualSubassetsDataLibrary.GetAnimation(ANIM_INFOBAR_SHOT));

      _choosingShotPlayerCueAnimation =
        new PlayableAnimationObject(ActualSubassetsDataLibrary.GetAnimation(SHOT_HAND_CUE_ANIM));
    }

    /// <summary>
    /// Является ли компонент автоматически обновляемым?
    /// </summary>
    public override bool IsUpdatable()
    {
      return false;
    }

    /// <summary>
    /// Обновление данных компонента отображения
    /// </summary>
    /// <param name="parDeltaTime">Время кадра</param>
    /// <returns>Флаг необходимости перерисовки</returns>
    public override bool ViewUpdate(double parDeltaTime)
    {
      throw new System.NotImplementedException();
    }

    /// <summary>
    /// Обновить данные для рендеринга
    /// </summary>
    public override void UpdateRenderingData()
    {
      if (_modelProviderComponent != null)
      {
        if (_modelProviderComponent.GameReady)
        {
          //всегда есть задний фон, шары, очки и счетчик жизней
          if (_currentRenderingBg == null)
          {
            _currentRenderingBg =
              new RenderingData.RenderingData(
                new RenderingSprite(ActualSubassetsDataLibrary.GetSprite(NORMAL_GAMEFIELD_BG_SPRITE), 0,
                  0,
                  0,
                  Color.White, 5));
          }

          if (_ballsRenderingData == null)
          {
            _ballsRenderingData = new LinkedList<RenderingData.RenderingData?>();
          }

          _ballsRenderingData.Clear();

          foreach (var modelBall in _modelProviderComponent.BallsInGame)
          {
            BallGraphicsBind graphicsBind = GameFieldViewConsts.BallGraphicsBinds[modelBall.BallType];
            _ballsRenderingData.AddLast(new RenderingData.RenderingData(new RenderingSprite(
              ActualSubassetsDataLibrary.GetSprite(graphicsBind.StillSpriteKey),
              GameFieldViewConsts.FIELD_MODEL_RENDER_LEFT_TOP_OFFSET_X + modelBall.Center.X,
              GameFieldViewConsts.FIELD_MODEL_RENDER_LEFT_TOP_OFFSET_Y +
              (GameFieldViewConsts.FIELD_HEIGHT - modelBall.Center.Y),
              0, graphicsBind.BlendColor, 1, 1, 1, EHorizontalAlign.Middle, EVerticalAlign.Middle)));
          }

          if (_playerScoreAndLifeCounter == null)
          {
            _playerScoreAndLifeCounter = new LinkedList<RenderingData.RenderingData?>();
          }

          _playerScoreAndLifeCounter.Clear();

          //очки
          _playerScoreAndLifeCounter.AddLast(new RenderingData.RenderingData(new RenderingString(1,
            SCORE_RIGHT_X_OFFSET,
            SCORE_TOP_Y_OFFSET, _modelProviderComponent.ActualPlayer.Score.ToString(),
            ActualSubassetsDataLibrary.GetFont(ViewBehaviourConsts.RED_SHADOW_APP_FONT), Color.White, 1, 1,
            EHorizontalAlign.Right, EVerticalAlign.Top)));

          //счетчик жизней
          int line1Lifes = 0;
          int line2Lifes = 0;

          int actualPlayerLifesNumber = _modelProviderComponent.ActualPlayer.LifeCount / 2 +
                                        _modelProviderComponent.ActualPlayer.LifeCount % 2;
          bool isLastLifeHalfed = _modelProviderComponent.ActualPlayer.LifeCount % 2 == 1;

          if (actualPlayerLifesNumber > 2 * LIFES_ON_ONE_LINE)
          {
            line1Lifes = LIFES_ON_ONE_LINE;
            line2Lifes = LIFES_ON_ONE_LINE;
          }
          else
          {
            if (actualPlayerLifesNumber >= LIFES_ON_ONE_LINE)
            {
              line1Lifes = LIFES_ON_ONE_LINE;
              line2Lifes = actualPlayerLifesNumber - LIFES_ON_ONE_LINE;
            }
            else
            {
              line1Lifes = actualPlayerLifesNumber;
              line2Lifes = 0;
            }
          }

          int currentY = LIFECOUNTER_LINE1_START_MIDDLE_Y;
          int currentX = LIFECOUNTER_LINE1_START_MIDDLE_X;
          for (int i = 0; i < line1Lifes; i++)
          {
            string chosenSpriteKey = LIFE_INDICATOR;

            if (i == line1Lifes - 1)
            {
              if (line2Lifes == 0)
              {
                if (isLastLifeHalfed)
                {
                  chosenSpriteKey = LIFE_INDICATOR_HALF;
                }
              }
            }


            _playerScoreAndLifeCounter.AddLast(new RenderingData.RenderingData(new RenderingSprite(
              ActualSubassetsDataLibrary.GetSprite(chosenSpriteKey), currentX, currentY, 0, Color.White,
              0,
              1, 1,
              EHorizontalAlign.Middle, EVerticalAlign.Middle)));
            currentX += LIFECOUNTER_X_ELEM_OFFSET;
          }


          currentY += LIFECOUNTER_Y_ELEM_OFFSET;
          currentX = LIFECOUNTER_LINE1_START_MIDDLE_X;
          for (int i = 0; i < line2Lifes; i++)
          {
            string chosenSpriteKey = LIFE_INDICATOR;

            if (i == line2Lifes - 1)
            {
              if (isLastLifeHalfed)
              {
                chosenSpriteKey = LIFE_INDICATOR_HALF;
              }
            }

            _playerScoreAndLifeCounter.AddLast(new RenderingData.RenderingData(new RenderingSprite(
              ActualSubassetsDataLibrary.GetSprite(chosenSpriteKey), currentX, currentY, 0, Color.White,
              0,
              1, 1,
              EHorizontalAlign.Middle, EVerticalAlign.Middle)));
            currentX += LIFECOUNTER_X_ELEM_OFFSET;
          }

          //дальнейший рендеринг зависит от текущего состояния игры
          if (_playerCueRenderingData == null)
          {
            _playerCueRenderingData = new LinkedList<RenderingData.RenderingData?>();
          }

          _playerCueRenderingData.Clear();

          if (_modelProviderComponent.CurrentGameState == EPocketGameState.Aiming)
          {
            _upperShotChoosePanelOverlay = null;

            //прицел
            SpVector3 whiteBallPosOrigin = _modelProviderComponent.PlayerWhiteBall.Center;
            whiteBallPosOrigin.X += GameFieldViewConsts.FIELD_MODEL_RENDER_LEFT_TOP_OFFSET_X;
            whiteBallPosOrigin.Y = GameFieldViewConsts.FIELD_MODEL_RENDER_LEFT_TOP_OFFSET_Y +
                                   (GameFieldViewConsts.FIELD_HEIGHT - whiteBallPosOrigin.Y);


            double additionalRotOffset = 0;
            if (_modelProviderComponent.PlayerCurrentAimingAngle > 10 &&
                _modelProviderComponent.PlayerCurrentAimingAngle < 80)
            {
              additionalRotOffset = ANGLE_ADJUST_TOLERANCE;
            }
            else if (_modelProviderComponent.PlayerCurrentAimingAngle <= 10 &&
                     _modelProviderComponent.PlayerCurrentAimingAngle > 0)
            {
              additionalRotOffset =
                CustomLerp.GetInterpolatedValue(_modelProviderComponent.PlayerCurrentAimingAngle, 0, 10, 0,
                  ANGLE_ADJUST_TOLERANCE);
            }
            else if (_modelProviderComponent.PlayerCurrentAimingAngle >= 80 &&
                     _modelProviderComponent.PlayerCurrentAimingAngle < 90)
            {
              additionalRotOffset =
                CustomLerp.GetInterpolatedValue(_modelProviderComponent.PlayerCurrentAimingAngle, 80, 90,
                  ANGLE_ADJUST_TOLERANCE,
                  0);
            }
            else if (_modelProviderComponent.PlayerCurrentAimingAngle > 100 &&
                     _modelProviderComponent.PlayerCurrentAimingAngle < 170)
            {
              additionalRotOffset = -ANGLE_ADJUST_TOLERANCE;
            }
            else if (_modelProviderComponent.PlayerCurrentAimingAngle > 90 &&
                     _modelProviderComponent.PlayerCurrentAimingAngle <= 100)
            {
              additionalRotOffset = -
                CustomLerp.GetInterpolatedValue(_modelProviderComponent.PlayerCurrentAimingAngle, 90, 100, 0,
                  ANGLE_ADJUST_TOLERANCE);
            }
            else if (_modelProviderComponent.PlayerCurrentAimingAngle >= 170 &&
                     _modelProviderComponent.PlayerCurrentAimingAngle < 180)
            {
              additionalRotOffset = -
                CustomLerp.GetInterpolatedValue(_modelProviderComponent.PlayerCurrentAimingAngle, 170, 180,
                  ANGLE_ADJUST_TOLERANCE,
                  0);
            }
            else if (_modelProviderComponent.PlayerCurrentAimingAngle > 190 &&
                     _modelProviderComponent.PlayerCurrentAimingAngle < 260)
            {
              additionalRotOffset = ANGLE_ADJUST_TOLERANCE;
            }
            else if (_modelProviderComponent.PlayerCurrentAimingAngle > 180 &&
                     _modelProviderComponent.PlayerCurrentAimingAngle <= 190)
            {
              additionalRotOffset =
                CustomLerp.GetInterpolatedValue(_modelProviderComponent.PlayerCurrentAimingAngle, 180, 190, 0,
                  ANGLE_ADJUST_TOLERANCE);
            }
            else if (_modelProviderComponent.PlayerCurrentAimingAngle >= 260 &&
                     _modelProviderComponent.PlayerCurrentAimingAngle < 270)
            {
              additionalRotOffset =
                CustomLerp.GetInterpolatedValue(_modelProviderComponent.PlayerCurrentAimingAngle, 260, 270,
                  ANGLE_ADJUST_TOLERANCE,
                  0);
            }
            else if (_modelProviderComponent.PlayerCurrentAimingAngle > 280 &&
                     _modelProviderComponent.PlayerCurrentAimingAngle < 350)
            {
              additionalRotOffset = -ANGLE_ADJUST_TOLERANCE;
            }
            else if (_modelProviderComponent.PlayerCurrentAimingAngle > 270 &&
                     _modelProviderComponent.PlayerCurrentAimingAngle <= 280)
            {
              additionalRotOffset = -
                CustomLerp.GetInterpolatedValue(_modelProviderComponent.PlayerCurrentAimingAngle, 270, 280, 0,
                  ANGLE_ADJUST_TOLERANCE);
            }
            else if (_modelProviderComponent.PlayerCurrentAimingAngle >= 350 &&
                     _modelProviderComponent.PlayerCurrentAimingAngle < 360)
            {
              additionalRotOffset = -
                CustomLerp.GetInterpolatedValue(_modelProviderComponent.PlayerCurrentAimingAngle, 350, 360,
                  ANGLE_ADJUST_TOLERANCE,
                  0);
            }


            _playerCueRenderingData.AddLast(new RenderingData.RenderingData(whiteBallPosOrigin.X + 8,
              whiteBallPosOrigin.Y, 100, 1, Color.Bisque, -1,
              _modelProviderComponent.GetAngleApproximated() + additionalRotOffset, whiteBallPosOrigin.X,
              whiteBallPosOrigin.Y));
          }
          else if (_modelProviderComponent.CurrentGameState == EPocketGameState.ChooseShotPower)
          {
            _choosingShotAnimationInfobar.CurrentFrame =
              _modelProviderComponent.GetShotForcePercent() * ANIM_INFOBAR_MAX_FRAME;

            _choosingShotPlayerCueAnimation.CurrentFrame =
              _modelProviderComponent.GetShotForcePercent() * ANIM_HANDSHOT_CUE_MAX_FRAME;

            //панель с информацией о силе удара

            _upperShotChoosePanelOverlay = new RenderingData.RenderingData(new RenderingSprite(
              _choosingShotAnimationInfobar.GetCurrentAnimSpriteFrame(), 145, 1, 0, Color.White, -5, 1,
              1));

            //анимация кия

            SpVector3 whiteBallPosOrigin = _modelProviderComponent.PlayerWhiteBall.Center;
            whiteBallPosOrigin.X += GameFieldViewConsts.FIELD_MODEL_RENDER_LEFT_TOP_OFFSET_X;
            whiteBallPosOrigin.Y = GameFieldViewConsts.FIELD_MODEL_RENDER_LEFT_TOP_OFFSET_Y +
                                   (GameFieldViewConsts.FIELD_HEIGHT - whiteBallPosOrigin.Y);

            _playerCueRenderingData.AddLast(new RenderingData.RenderingData(new RenderingSprite(
              _choosingShotPlayerCueAnimation.GetCurrentAnimSpriteFrame(), whiteBallPosOrigin.X + 8,
              whiteBallPosOrigin.Y + 4, _modelProviderComponent.PlayerCurrentAimingAngle - 180,
              Color.White,
              -6, 1, 1, EHorizontalAlign.Left, EVerticalAlign.Middle, whiteBallPosOrigin.X,
              whiteBallPosOrigin.Y)));
          }
        }
      }
    }

    /// <summary>
    /// Получить данные для рендеринга
    /// </summary>
    public override List<RenderingData.RenderingData?> GetRenderingData()
    {
      List<RenderingData.RenderingData?> retRenderingData = new List<RenderingData.RenderingData?>();

      if (_currentRenderingBg != null)
      {
        retRenderingData.Add(_currentRenderingBg);
      }

      if (_upperShotChoosePanelOverlay != null)
      {
        retRenderingData.Add(_upperShotChoosePanelOverlay);
      }

      if (_playerScoreAndLifeCounter != null)
      {
        retRenderingData.AddRange(_playerScoreAndLifeCounter);
      }

      if (_ballsRenderingData != null)
      {
        retRenderingData.AddRange(_ballsRenderingData);
      }

      if (_playerCueRenderingData != null)
      {
        if (_playerCueRenderingData.Count > 0)
        {
          retRenderingData.AddRange(_playerCueRenderingData);
        }
      }

      return retRenderingData;
    }
  }
}