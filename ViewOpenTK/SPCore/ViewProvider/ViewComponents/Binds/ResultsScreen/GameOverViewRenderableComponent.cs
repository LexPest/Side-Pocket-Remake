using System.Collections.Generic;
using System.Drawing;
using CoreUtil.Pool;
using Model.SPCore.GameplayModelDefinition.GameComponents.ViewProviderBase;
using ViewOpenTK.SPCore.ViewProvider.InternalGraphicalDataStructures;
using ViewOpenTK.SPCore.ViewProvider.RenderingData;

namespace ViewOpenTK.SPCore.ViewProvider.ViewComponents.Binds.ResultsScreen
{
  /// <summary>
  /// Компонент для рендеринга данных экрана проигрыша
  /// </summary>
  public class GameOverViewRenderableComponent : ViewRenderableComponent
  {
    /// <summary>
    /// Позиция X для рендеринга текста
    /// </summary>
    private const int RENDERING_TEXT_X_MIDDLE = 160;
    /// <summary>
    /// Позиция Y для рендеринга текста
    /// </summary>
    private const int RENDERING_TEXT_Y_MIDDLE = 120;

    /// <summary>
    /// Конструктор для поддержки системы пулинга объектов
    /// </summary>
    /// <param name="parSupportData">Данные для работы с пулом</param>
    public GameOverViewRenderableComponent(ObjectPoolSupportData parSupportData) : base(parSupportData)
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
    }

    /// <summary>
    /// Получить данные для рендеринга
    /// </summary>
    public override List<RenderingData.RenderingData?> GetRenderingData()
    {
      List<RenderingData.RenderingData?> retRenderingData = new List<RenderingData.RenderingData?>();
      retRenderingData.Add(new RenderingData.RenderingData(new RenderingFillColorScreen(-100, Color.Black)));
      retRenderingData.Add(new RenderingData.RenderingData(new RenderingString(-105, RENDERING_TEXT_X_MIDDLE,
        RENDERING_TEXT_Y_MIDDLE, "GAME OVER",
        ActualSubassetsDataLibrary.GetFont(ViewBehaviourConsts.RED_SHADOW_APP_FONT), Color.White, 1, 1,
        EHorizontalAlign.Middle, EVerticalAlign.Middle)));
      return retRenderingData;
    }
  }
}