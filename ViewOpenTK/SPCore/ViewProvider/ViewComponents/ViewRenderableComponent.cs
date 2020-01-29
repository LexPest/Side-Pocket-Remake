using System.Collections.Generic;
using CoreUtil.Pool;
using Model.SPCore.GameplayModelDefinition.GameComponents.ViewProviderBase;
using ViewOpenTK.AssetData.DataTypes.Subassets;

namespace ViewOpenTK.SPCore.ViewProvider.ViewComponents
{
  /// <summary>
  /// Базовый класс компонента отображения для рендеринга
  /// </summary>
  public abstract class ViewRenderableComponent : PoolSupportedObject
  {
    /// <summary>
    /// Доступная библиотека производных ассетов
    /// </summary>
    protected SubassetsDataLibrary ActualSubassetsDataLibrary;
    /// <summary>
    /// Связанный обработчик событий отображения
    /// </summary>
    protected ViewEventsOpenTkHandler LinkedViewEventsHandler;

    /// <summary>
    /// Конструктор для поддержки системы пулинга объектов
    /// </summary>
    /// <param name="parSupportData">Данные для работы с пулом</param>
    protected ViewRenderableComponent(ObjectPoolSupportData parSupportData) : base(parSupportData)
    {
    }

    /// <summary>
    /// Является ли компонент автоматически обновляемым?
    /// </summary>
    /// <returns></returns>
    public abstract bool IsUpdatable();

    /// <summary>
    /// Замена конструктора, процедура инициализации компонента отображения
    /// </summary>
    /// <param name="parModelSideProviderComponent">Связанный компонент на стороне модели</param>
    /// <param name="parLinkedEventsHandler">Связанный обработчик событий отображения</param>
    public virtual void InitAndLink(
      ViewProviderComponent parModelSideProviderComponent, ViewEventsOpenTkHandler parLinkedEventsHandler)
    {
      ActualSubassetsDataLibrary = parLinkedEventsHandler.ActualSubassetsDataLibrary;
      LinkedViewEventsHandler = parLinkedEventsHandler;
    }

    /// <summary>
    /// Обновление данных компонента отображения
    /// </summary>
    /// <param name="parDeltaTime">Время кадра</param>
    /// <returns>Флаг необходимости перерисовки</returns>
    public abstract bool ViewUpdate(double parDeltaTime);

    /// <summary>
    /// Обновить данные для рендеринга
    /// </summary>
    public abstract void UpdateRenderingData();
    
    /// <summary>
    /// Получить данные для рендеринга
    /// </summary>
    /// <returns></returns>
    public abstract List<RenderingData.RenderingData?> GetRenderingData();
  }
}