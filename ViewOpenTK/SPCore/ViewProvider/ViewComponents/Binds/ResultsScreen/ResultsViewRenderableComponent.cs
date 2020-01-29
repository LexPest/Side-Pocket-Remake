using System;
using System.Collections.Generic;
using System.Drawing;
using CoreUtil.Pool;
using Model.SPCore.DS;
using Model.SPCore.GameplayModelDefinition.GameComponents.InGame.ResultScreens;
using Model.SPCore.GameplayModelDefinition.GameComponents.InGame.ResultScreens.ViewProviders;
using Model.SPCore.GameplayModelDefinition.GameComponents.ViewProviderBase;
using ViewOpenTK.SPCore.ViewProvider.InternalGraphicalDataStructures;
using ViewOpenTK.SPCore.ViewProvider.RenderingData;

namespace ViewOpenTK.SPCore.ViewProvider.ViewComponents.Binds.ResultsScreen
{
  /// <summary>
  /// Компонент для рендеринга данных экрана результатов успешного прохождения уровня
  /// </summary>
  public class ResultsViewRenderableComponent : ViewRenderableComponent
  {
    /// <summary>
    /// Название спрайта заднего фона
    /// </summary>
    private const string RESULTS_BG_PATH = "/stage_results_screen/spr_stage_results_background.png";
    
    /// <summary>
    /// Позиция X для текста
    /// </summary>
    private const int TEXT_MIDDLE_X = 160;

    /// <summary>
    /// Данные для рендеринга заднего фона
    /// </summary>
    private RenderingData.RenderingData? _backgroundRenderingData;

    /// <summary>
    /// Компонент на стороне модели
    /// </summary>
    private ResultsScreenComponent _modelSideComponent;
    
    /// <summary>
    /// Данные для рендеринга текста
    /// </summary>
    private List<RenderingData.RenderingData?> _textsRenderingData;

    /// <summary>
    /// Конструктор для поддержки системы пулинга объектов
    /// </summary>
    /// <param name="parSupportData">Данные для работы с пулом</param>
    public ResultsViewRenderableComponent(ObjectPoolSupportData parSupportData) : base(parSupportData)
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
      _modelSideComponent = (ResultsScreenComponent) parModelSideProviderComponent;
    }

    /// <summary>
    /// Является ли компонент автоматически обновляемым?
    /// </summary>
    public override bool IsUpdatable()
    {
      return true;
    }

    /// <summary>
    /// Обновление данных компонента отображения
    /// </summary>
    /// <param name="parDeltaTime">Время кадра</param>
    /// <returns>Флаг необходимости перерисовки</returns>
    public override bool ViewUpdate(double parDeltaTime)
    {
      UpdateRenderingData();
      return true;
    }

    /// <summary>
    /// Обновить данные для рендеринга
    /// </summary>
    public override void UpdateRenderingData()
    {
      if (_modelSideComponent.IsAlive)
      {
        if (_backgroundRenderingData == null)
        {
          _backgroundRenderingData =
            new RenderingData.RenderingData(new RenderingSprite(ActualSubassetsDataLibrary.GetSprite(RESULTS_BG_PATH),
              0,
              0,
              0, Color.White, -30));
        }

        if (_textsRenderingData == null)
        {
          _textsRenderingData = new List<RenderingData.RenderingData?>();
        }

        _textsRenderingData.Clear();

        _textsRenderingData.Add(new RenderingData.RenderingData(new RenderingString(-35, TEXT_MIDDLE_X, 120,
          $"YOUR CURRENT SCORE: {_modelSideComponent.PlayerActualData.Score}",
          ActualSubassetsDataLibrary.GetFont(ViewBehaviourConsts.DEFAULT_APP_FONT), Color.White, 1, 1,
          EHorizontalAlign.Middle, EVerticalAlign.Middle)));

        void ProcessButton(bool parIsActiveButton, int parX, int parY, string parCaption)
        {
          string chosenFontKey = parIsActiveButton
            ? ViewBehaviourConsts.DEFAULT_APP_FONT
            : ViewBehaviourConsts
              .MENU_DARK_INACTIVE_APP_FONT;

          _textsRenderingData.Add(new RenderingData.RenderingData(new RenderingString(-35, parX, parY,
            parCaption, ActualSubassetsDataLibrary.GetFont(chosenFontKey), Color.White, 1, 1,
            EHorizontalAlign.Middle, EVerticalAlign.Middle)));
        }

        switch (_modelSideComponent.CurrentStage)
        {
          case EResultsScreenStage.ChooseNextAction:
          {
            ProcessButton(!_modelSideComponent.IsBankAndExitActionSelected, TEXT_MIDDLE_X, 160, "CONTINUE");
            ProcessButton(_modelSideComponent.IsBankAndExitActionSelected, TEXT_MIDDLE_X, 180,
              "BANK AND EXIT");
            break;
          }

          case EResultsScreenStage.ChooseNameForTheRecords:
          {
            ProcessButton(true, TEXT_MIDDLE_X, 140,
              $"{ViewBehaviourConsts.ESCAPE_CHARACTER_SEQUENCE_START}KING_1{ViewBehaviourConsts.ESCAPE_CHARACTER_SEQUENCE_END} ENTER NAME FOR RECORDS LIST {ViewBehaviourConsts.ESCAPE_CHARACTER_SEQUENCE_START}KING_1{ViewBehaviourConsts.ESCAPE_CHARACTER_SEQUENCE_END}");
            ProcessButton(true, TEXT_MIDDLE_X, 160, "8 CHARACTERS MAX, THEN PRESS ENTER");
            string playerName = _modelSideComponent.CurrentPlayerName;
            if (playerName.Length < RecordPlayerInfo.MAX_CHARS_IN_NAME)
            {
              playerName = playerName + "-";
            }

            ProcessButton(true, TEXT_MIDDLE_X, 180, playerName);
            break;
          }

          default:
            throw new ArgumentOutOfRangeException();
        }
      }
    }

    /// <summary>
    /// Получить данные для рендеринга
    /// </summary>
    public override List<RenderingData.RenderingData?> GetRenderingData()
    {
      List<RenderingData.RenderingData?> retData = new List<RenderingData.RenderingData?>();
      if (_backgroundRenderingData != null)
      {
        retData.Add(_backgroundRenderingData);
      }

      if (_textsRenderingData != null)
      {
        if (_textsRenderingData.Count > 0)
        {
          retData.AddRange(_textsRenderingData);
        }
      }

      return retData;
    }
  }
}