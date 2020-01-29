using System;
using System.Collections.Generic;
using System.Drawing;
using CoreUtil.Pool;
using Model.SPCore.DS;
using Model.SPCore.GameplayModelDefinition.GameComponents.Menus;
using Model.SPCore.GameplayModelDefinition.GameComponents.Menus.MainMenu;
using Model.SPCore.GameplayModelDefinition.GameComponents.ViewProviderBase;
using ViewOpenTK.SPCore.ViewProvider.InternalGraphicalDataStructures;
using ViewOpenTK.SPCore.ViewProvider.RenderingData;

namespace ViewOpenTK.SPCore.ViewProvider.ViewComponents.Binds.Menus
{
  /// <summary>
  /// Компонент для рендеринга данных меню таблицы рекордов
  /// </summary>
  public class RecordsMenuControlComponentViewRenderableComponent : ViewRenderableComponent
  {
    /// <summary>
    /// Данные для рендеринга элементов меню
    /// </summary>
    private List<RenderingData.RenderingData?> _currentMenuElements;
    
    /// <summary>
    /// Компонент на стороне модели
    /// </summary>
    private RecordsMenuControlComponent _modelProviderComponent;

    /// <summary>
    /// Конструктор для поддержки системы пулинга объектов
    /// </summary>
    /// <param name="parSupportData">Данные для работы с пулом</param>
    public RecordsMenuControlComponentViewRenderableComponent(ObjectPoolSupportData parSupportData) : base(parSupportData)
    {
    }
    
    /// <summary>
    /// Является ли компонент автоматически обновляемым?
    /// </summary>
    public override bool IsUpdatable()
    {
      return false;
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
      _modelProviderComponent = (RecordsMenuControlComponent) parModelSideProviderComponent;

      _modelProviderComponent.OnBecomeActive += OnActivatedPerform;
      _modelProviderComponent.OnDeactivated += OnDeactivatedPerform;
    }

    /// <summary>
    /// Действия при активации меню
    /// </summary>
    private void OnActivatedPerform()
    {
      ProcessUiElementsList();
      Console.WriteLine("Records Menu activated");
    }

    /// <summary>
    /// Действия при деактвации меню
    /// </summary>
    private void OnDeactivatedPerform()
    {
      _currentMenuElements = null;
      Console.WriteLine("Records Menu disabled");
    }

    /// <summary>
    /// Обновление данных компонента отображения
    /// </summary>
    /// <param name="parDeltaTime">Время кадра</param>
    /// <returns>Флаг необходимости перерисовки</returns>
    public override bool ViewUpdate(double parDeltaTime)
    {
      return false;
    }

    /// <summary>
    /// Обработка состояния элементов интерфейса
    /// </summary>
    private void ProcessUiElementsList()
    {
      //добавить фон
      _currentMenuElements = new List<RenderingData.RenderingData?>();
      _currentMenuElements.Add(new RenderingData.RenderingData(
        new RenderingSprite(
          ActualSubassetsDataLibrary.GetSprite(MainMenuControlComponentViewRenderableComponent.MAIN_MENU_BG),
          0, 0, 0, Color.White, 1)));

      //добавить псевдо-кнопки рекордов с текущими значениями
      int currentMenuY = MainMenuControlComponentViewRenderableComponent.Y_OFFSET_MENU_START;

      if (_modelProviderComponent.MenuUiElements != null)
      {
        void ProcessButton(UiElementButton parButton, int parY, string parCaptionPrimary, string parCaptionSecondary)
        {
          string chosenFontKey = parButton.IsHovered
            ? ViewBehaviourConsts.DEFAULT_APP_FONT
            : ViewBehaviourConsts
              .MENU_DARK_INACTIVE_APP_FONT;

          bool isCentered = parCaptionSecondary == null;

          if (isCentered)
          {
            _currentMenuElements.Add(new RenderingData.RenderingData(new RenderingString(-1,
              MainMenuControlComponentViewRenderableComponent.X_CENTERED_OFFSET_MENU, parY,
              parCaptionPrimary, ActualSubassetsDataLibrary.GetFont(chosenFontKey), Color.White, 1, 1,
              EHorizontalAlign.Middle, EVerticalAlign.Top)));
          }
          else
          {
            _currentMenuElements.Add(new RenderingData.RenderingData(new RenderingString(-1,
              OptionsMenuControlComponentViewRenderableComponent.MENU_LEFT_SIDE_X_OFFSET, parY,
              parCaptionPrimary, ActualSubassetsDataLibrary.GetFont(chosenFontKey), Color.White, 1, 1,
              EHorizontalAlign.Left, EVerticalAlign.Top)));
            _currentMenuElements.Add(new RenderingData.RenderingData(new RenderingString(-1,
              OptionsMenuControlComponentViewRenderableComponent.MENU_RIGHT_SIDE_X_OFFSET, parY,
              parCaptionSecondary, ActualSubassetsDataLibrary.GetFont(chosenFontKey), Color.White, 1, 1,
              EHorizontalAlign.Right, EVerticalAlign.Top)));
          }
        }

        RecordsData recordsData = _modelProviderComponent.ParentGameObject.LinkedAppModel.GetRecordsData();
        int currentRecordId = 0;
        ProcessButton(_modelProviderComponent.RecordOne, currentMenuY,
          $"{recordsData.PlayerRecordsInfo[currentRecordId].PlayerName}",
          recordsData.PlayerRecordsInfo[currentRecordId].PointsEarned.ToString());
        currentRecordId++;

        currentMenuY += MainMenuControlComponentViewRenderableComponent.Y_OFFSET_MENU_ELEMENTS;

        ProcessButton(_modelProviderComponent.RecordTwo, currentMenuY,
          $"{recordsData.PlayerRecordsInfo[currentRecordId].PlayerName}",
          recordsData.PlayerRecordsInfo[currentRecordId].PointsEarned.ToString());
        currentRecordId++;

        currentMenuY += MainMenuControlComponentViewRenderableComponent.Y_OFFSET_MENU_ELEMENTS;

        ProcessButton(_modelProviderComponent.RecordThree, currentMenuY,
          $"{recordsData.PlayerRecordsInfo[currentRecordId].PlayerName}",
          recordsData.PlayerRecordsInfo[currentRecordId].PointsEarned.ToString());
        currentRecordId++;

        currentMenuY += MainMenuControlComponentViewRenderableComponent.Y_OFFSET_MENU_ELEMENTS;

        ProcessButton(_modelProviderComponent.RecordsClearButton, currentMenuY, "Clear", null);

        currentMenuY += MainMenuControlComponentViewRenderableComponent.Y_OFFSET_MENU_ELEMENTS;

        ProcessButton(_modelProviderComponent.ReturnBackButton, currentMenuY, "Back", null);
      }
    }

    /// <summary>
    /// Обновить данные для рендеринга
    /// </summary>
    public override void UpdateRenderingData()
    {
      if (_currentMenuElements != null)
      {
        ProcessUiElementsList();
      }
    }

    /// <summary>
    /// Получить данные для рендеринга
    /// </summary>
    public override List<RenderingData.RenderingData?> GetRenderingData()
    {
      if (_modelProviderComponent.IsActive)
      {
        List<RenderingData.RenderingData?> retList = new List<RenderingData.RenderingData?>();
        if (_currentMenuElements != null)
          retList.AddRange(_currentMenuElements);
        return retList;
      }
      else
      {
        return null;
      }
    }

    /// <summary>
    /// Выполнить действия перед отправкой в пул
    /// </summary>
    public override void OnBeforeResetPerform()
    {
      base.OnBeforeResetPerform();

      if (_modelProviderComponent != null)
      {
        _modelProviderComponent.OnBecomeActive -= OnActivatedPerform;
        _modelProviderComponent.OnDeactivated -= OnDeactivatedPerform;
      }
    }
  }
}