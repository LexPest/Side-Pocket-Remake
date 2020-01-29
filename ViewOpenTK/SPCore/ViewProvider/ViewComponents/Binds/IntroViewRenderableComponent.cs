using System;
using System.Collections.Generic;
using System.Drawing;
using CoreUtil.Pool;
using Model.SPCore.GameplayModelDefinition.GameComponents.Launching.ViewProviders;
using Model.SPCore.GameplayModelDefinition.GameComponents.ViewProviderBase;
using Model.SPCore.Managers.Sound.Data;
using ViewOpenTK.OpenGL;
using ViewOpenTK.SPCore.ViewProvider.InternalGraphicalDataStructures;
using ViewOpenTK.SPCore.ViewProvider.RenderingData;

namespace ViewOpenTK.SPCore.ViewProvider.ViewComponents.Binds
{
  /// <summary>
  /// Компонент для рендеринга данных стартовой заставки игры
  /// </summary>
  public class IntroViewRenderableComponent : ViewRenderableComponent
  {
    /// <summary>
    /// Название префикса анимации стартовой заставки
    /// </summary>
    private const string ANIM_NAME_INTRO_CUTSCENE_START = "/intro_anim/spr_intro_anim_tex";

    /// <summary>
    /// Стартовый индекс номера анимации
    /// </summary>
    private const int ANIM_NAME_INTRO_CUTSCENE_INDEX_FROM = 0;

    /// <summary>
    /// Конечный индекс номера анимации
    /// </summary>
    private const int ANIM_NAME_INTRO_CUTSCENE_INDEX_TO = 12;

    /// <summary>
    /// Постфикс названия анимации стартовой заставки
    /// </summary>
    private const string ANIM_NAME_INTRO_CUTSCENE_END = ".png";

    /// <summary>
    /// Кадр начала экрана "SEGA"
    /// </summary>
    private const int FRAME_SEGA_START = 0;

    /// <summary>
    /// Кадр конца экрана "SEGA"
    /// </summary>
    private const int FRAME_SEGA_END = 132;

    /// <summary>
    /// Кадр начала экрана "DATA EAST"
    /// </summary>
    private const int FRAME_DATA_EAST_FULL_START = 210;

    /// <summary>
    /// Кадр конца экрана "DATA EAST"
    /// </summary>
    private const int FRAME_DATA_EAST_FULL_END = 406;

    /// <summary>
    /// Кадр начала экрана "PRESS START BUTTON"
    /// </summary>
    private const int FRAME_DATA_PRESS_START_BUTTON_START = 584;

    /// <summary>
    /// Y-смещение позиции текста копирайта
    /// </summary>
    private const int BASE_COPYRIGHT_NOTICE_Y_BOTTOM_OFFSET = 40;

    /// <summary>
    /// X-смещение позиции текста PRESS START BUTTOn
    /// </summary>
    private const int PRESS_START_COPYRIGHT_NOTICE_X_FROM_LEFT = 28;

    /// <summary>
    /// Y-смещение позиции текста PRESS START BUTTOn
    /// </summary>
    private const int PRESS_START_COPYRIGHT_NOTICE_Y_FROM_BOTTOM = 12;

    /// <summary>
    /// Текущий индекс анимации
    /// </summary>
    private int _currentAnimSheetIndex = ANIM_NAME_INTRO_CUTSCENE_INDEX_FROM;

    /// <summary>
    /// Количество кадров в текущей анимации
    /// </summary>
    private int _currentFrameTotal;

    /// <summary>
    /// Текущие данные для рендеринга спрайтов
    /// </summary>
    private RenderingData.RenderingData? _currentSpriteRenderingData;

    /// <summary>
    /// Текущие данные для рендеринга текста
    /// </summary>
    private List<RenderingData.RenderingData?> _currentStringsRenderingData;

    /// <summary>
    /// Текущая анимация стартовой заставки
    /// </summary>
    private PlayableAnimationObject _introCutsceneAnimation;

    /// <summary>
    /// Воспроизводится ли анимация
    /// </summary>
    private bool _isAnimInProgress = false;

    /// <summary>
    /// Компонент на стороне модели
    /// </summary>
    private IntroViewProviderComponent _modelProviderComponent;

    /// <summary>
    /// Конструктор для поддержки системы пулинга объектов
    /// </summary>
    /// <param name="parSupportData">Данные для работы с пулом</param>
    public IntroViewRenderableComponent(ObjectPoolSupportData parSupportData) : base(parSupportData)
    {
    }

    /// <summary>
    /// Является ли компонент автоматически обновляемым?
    /// </summary>
    public override bool IsUpdatable()
    {
      return true;
    }

    /// <summary>
    /// Замена конструктора, процедура инициализации компонента отображения
    /// </summary>
    /// <param name="parModelSideProviderComponent">Связанный компонент на стороне модели</param>
    /// <param name="parLinkedEventsHandler">Связанный обработчик событий отображения</param>
    public override void InitAndLink(
      ViewProviderComponent parModelSideProviderComponent, ViewEventsOpenTkHandler parLinkedEventsHandler)
    {
      base.InitAndLink(parModelSideProviderComponent, parLinkedEventsHandler);

      _modelProviderComponent = (IntroViewProviderComponent) parModelSideProviderComponent;

      _currentAnimSheetIndex = ANIM_NAME_INTRO_CUTSCENE_INDEX_FROM;
      // TODO null setting is not nessesary
      _currentSpriteRenderingData = null;
      _introCutsceneAnimation = null;

      ProcessNextAnimationSheet();
    }

    /// <summary>
    /// Обработать следующую анимацию
    /// </summary>
    private void ProcessNextAnimationSheet()
    {
      void PlayNextSheet()
      {
        string animationAssetName =
          $"{ANIM_NAME_INTRO_CUTSCENE_START}{_currentAnimSheetIndex}{ANIM_NAME_INTRO_CUTSCENE_END}";
        _introCutsceneAnimation =
          new PlayableAnimationObject(ActualSubassetsDataLibrary.GetAnimation(animationAssetName), 60);
        _introCutsceneAnimation.EventFullAnimCompleted += ProcessNextAnimationSheet;
      }

      _isAnimInProgress = true;
      if (_introCutsceneAnimation == null)
      {
        PlayNextSheet();
      }
      else
      {
        _currentFrameTotal += _introCutsceneAnimation.FrameCount;
        _introCutsceneAnimation.EventFullAnimCompleted -= ProcessNextAnimationSheet;
        _introCutsceneAnimation = null;

        _currentAnimSheetIndex++;


        if (_currentAnimSheetIndex > ANIM_NAME_INTRO_CUTSCENE_INDEX_TO)
        {
          _isAnimInProgress = false;
          _modelProviderComponent.OnIntroCutsceneEndedPerform?.Invoke();
        }
        else
        {
          PlayNextSheet();
        }
      }
    }

    /// <summary>
    /// Обновление данных компонента отображения
    /// </summary>
    /// <param name="parDeltaTime">Время кадра</param>
    /// <returns>Флаг необходимости перерисовки</returns>
    public override bool ViewUpdate(double parDeltaTime)
    {
      if (_isAnimInProgress)
      {
        _introCutsceneAnimation.ProceedAnimationStep(parDeltaTime);
        UpdateRenderingData();
        return true;
      }

      return false;
    }

    /// <summary>
    /// Обновить данные для рендеринга
    /// </summary>
    public override void UpdateRenderingData()
    {
      if (_isAnimInProgress)
      {
        _currentSpriteRenderingData =
          new RenderingData.RenderingData(new RenderingSprite(_introCutsceneAnimation.GetCurrentAnimSpriteFrame(), 0, 0,
            0,
            Color.White, -10));

        int currentActualFrame = _currentFrameTotal + (int) Math.Floor(_introCutsceneAnimation.CurrentFrame);

        _currentStringsRenderingData = new List<RenderingData.RenderingData?>();

        if (currentActualFrame >= FRAME_SEGA_START && currentActualFrame <= FRAME_SEGA_END)
        {
          _currentStringsRenderingData.Add(new RenderingData.RenderingData(new RenderingString(-12,
            OpenGlWindowDisplay.Instance.ViewportWidth / 2,
            OpenGlWindowDisplay.Instance.ViewportHeight - BASE_COPYRIGHT_NOTICE_Y_BOTTOM_OFFSET,
            ViewAppStrings.SegaScreenNotice,
            ActualSubassetsDataLibrary.GetFont(ViewBehaviourConsts.DEFAULT_APP_FONT), Color.White, 1, 1,
            EHorizontalAlign.Middle, EVerticalAlign.Bottom)));
        }
        else if (currentActualFrame >= FRAME_DATA_EAST_FULL_START &&
                 currentActualFrame <= FRAME_DATA_EAST_FULL_END)
        {
          _currentStringsRenderingData.Add(new RenderingData.RenderingData(new RenderingString(-12,
            OpenGlWindowDisplay.Instance.ViewportWidth / 2,
            OpenGlWindowDisplay.Instance.ViewportHeight - BASE_COPYRIGHT_NOTICE_Y_BOTTOM_OFFSET,
            ViewAppStrings.DataEastScreenNotice,
            ActualSubassetsDataLibrary.GetFont(ViewBehaviourConsts.DEFAULT_APP_FONT), Color.White, 1, 1,
            EHorizontalAlign.Middle, EVerticalAlign.Bottom)));
        }
        else if (currentActualFrame >= FRAME_DATA_EAST_FULL_END &&
                 currentActualFrame < FRAME_DATA_PRESS_START_BUTTON_START)
        {
          if (!_modelProviderComponent.ParentGameObject.LinkedAppModel.GetSoundManager().IsMusicPlaying())
          {
            _modelProviderComponent.ParentGameObject.LinkedAppModel.GetSoundManager()
              .PlayBgMusic(EAppMusicAssets.MainMenu, true);
          }
        }
        else if (currentActualFrame >= FRAME_DATA_PRESS_START_BUTTON_START)
        {
          _currentStringsRenderingData.Add(new RenderingData.RenderingData(new RenderingString(-12,
            PRESS_START_COPYRIGHT_NOTICE_X_FROM_LEFT,
            OpenGlWindowDisplay.Instance.ViewportHeight - PRESS_START_COPYRIGHT_NOTICE_Y_FROM_BOTTOM,
            ViewAppStrings.PressStartScreenCopyright,
            ActualSubassetsDataLibrary.GetFont(ViewBehaviourConsts.DEFAULT_APP_FONT), Color.White, 1, 1,
            EHorizontalAlign.Left, EVerticalAlign.Bottom)));
        }
        else
        {
          _currentStringsRenderingData = null;
        }
      }
      else
      {
        _currentSpriteRenderingData = null;
        _currentStringsRenderingData = null;
      }
    }

    /// <summary>
    /// Получить данные для рендеринга
    /// </summary>
    public override List<RenderingData.RenderingData?> GetRenderingData()
    {
      List<RenderingData.RenderingData?> retRenderingDataList = new List<RenderingData.RenderingData?>();
      retRenderingDataList.Add(_currentSpriteRenderingData);
      if (_currentStringsRenderingData != null)
      {
        retRenderingDataList.AddRange(_currentStringsRenderingData);
      }

      return retRenderingDataList;
    }
  }
}