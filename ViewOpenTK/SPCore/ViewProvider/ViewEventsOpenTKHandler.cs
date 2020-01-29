using System;
using System.Collections.Generic;
using CoreUtil.Pool;
using Model.SPCore.GameplayModelDefinition.GameComponents.ViewProviderBase;
using Model.SPCore.GameplayModelDefinition.ObjectModel;
using ViewOpenTK.AssetData.DataTypes.Subassets;
using ViewOpenTK.OpenGL;
using ViewOpenTK.SPCore.ViewProvider.ViewComponents;

namespace ViewOpenTK.SPCore.ViewProvider
{
  /// <summary>
  /// Обработчик событий отображения OpenTK
  /// </summary>
  public class ViewEventsOpenTkHandler : IDisposable
  {
    /// <summary>
    /// Активные компоненты рендеринга
    /// </summary>
    private Dictionary<ViewProviderComponent, ViewRenderableComponent> _activeRenderableComponents =
      new Dictionary<ViewProviderComponent, ViewRenderableComponent>();

    /// <summary>
    /// Активные обновляемые компоненты рендеринга
    /// </summary>
    private List<ViewRenderableComponent> _updatableRenderableComponents = new List<ViewRenderableComponent>();

    /// <summary>
    /// Стандартный конструктор
    /// </summary>
    /// <param name="parTypesLinker">Объект для связывания компонентов модели и отображения</param>
    /// <param name="parAppModel">Модель приложения</param>
    /// <param name="parLinkedView">Отображение приложения</param>
    public ViewEventsOpenTkHandler(ViewBehaviourOpenTkHandlersLinker parTypesLinker,
      IAppModel parAppModel, AppViewOpenTk parLinkedView)
    {
      TypesLinker = parTypesLinker;
      ViewSideProviderColleague = new ViewProviderOpenTkViewSideColleague(parAppModel.GetViewProviderMediator(), this);
      AppModelRef = parAppModel;
      LinkedView = parLinkedView;

      OpenGlWindowDisplay.Instance.RenderDisplayChanged += OnDisplayChanged;
    }

    /// <summary>
    /// Деструктор
    /// </summary>
    public void Dispose()
    {
      OpenGlWindowDisplay.Instance.RenderDisplayChanged -= OnDisplayChanged;
    }

    /// <summary>
    /// Действия при создании компонента на стороне модели
    /// </summary>
    /// <param name="parTargetComponent">Компонент на стороне модели</param>
    public void OnComponentCreated(ViewProviderComponent parTargetComponent)
    {
      CheckSubassetsDataLibrary();

      Type renderableComponentType = TypesLinker.GetLinkedViewComponentType(parTargetComponent.GetType());

      ViewRenderableComponent newRenderableComponent =
        (ViewRenderableComponent) OpenTkViewComponentsPoolManager.GetObject(renderableComponentType);

      _activeRenderableComponents.Add(parTargetComponent, newRenderableComponent);

      if (newRenderableComponent.IsUpdatable())
      {
        _updatableRenderableComponents.Add(newRenderableComponent);
      }

      newRenderableComponent.InitAndLink(parTargetComponent, this);

      newRenderableComponent.UpdateRenderingData();

      RendereringDataWasUpdatedFlag = true;
    }

    /// <summary>
    /// Действия при удалении компонента на стороне модели
    /// </summary>
    /// <param name="parTargetComponent">Компонент на стороне модели</param>
    public void OnComponentRemoved(ViewProviderComponent parTargetComponent)
    {
      ViewRenderableComponent targetRenderableComponent = _activeRenderableComponents[parTargetComponent];

      if (targetRenderableComponent.IsUpdatable())
      {
        _updatableRenderableComponents.Remove(targetRenderableComponent);
      }

      targetRenderableComponent.DisableAndSendToPool();
      _activeRenderableComponents.Remove(parTargetComponent);

      RendereringDataWasUpdatedFlag = true;
    }

    /// <summary>
    /// Действия при обновлении компонента на стороне модели
    /// </summary>
    /// <param name="parTargetComponent">Компонент на стороне модели</param>
    public void OnComponentUpdated(ViewProviderComponent parTargetComponent)
    {
      _activeRenderableComponents[parTargetComponent].UpdateRenderingData();

      RendereringDataWasUpdatedFlag = true;
    }

    /// <summary>
    /// Сигнал обновления компонентов отображения
    /// </summary>
    /// <param name="parDeltaTime">Время кадра</param>
    public void ViewUpdate(double parDeltaTime)
    {
      List<ViewRenderableComponent> safeRenderableComponents =
        new List<ViewRenderableComponent>(_updatableRenderableComponents);

      foreach (var renderableComponent in safeRenderableComponents)
      {
        if (renderableComponent.IsUpdatable())
        {
          if (renderableComponent.ViewUpdate(parDeltaTime))
          {
            RendereringDataWasUpdatedFlag = true;
          }
        }
      }
    }

    /// <summary>
    /// Проверка библиотеки производных ассетов
    /// </summary>
    public void CheckSubassetsDataLibrary()
    {
      if (ActualSubassetsDataLibrary == null)
      {
        ActualSubassetsDataLibrary = new SubassetsDataLibrary();
        ActualSubassetsDataLibrary.UpdateCollectionsData(
          ViewBehaviourConsts.DefaultUpdatingSubassetsLibStrategy, AppModelRef.GetResourcesManager());
        //TODO Maybe change while(){} to WaitUntil...?
        while (!OpenGlCommandsInternalHandler.AreAllActionsPerformed())
        {
          Console.WriteLine("Waiting for actions");
        }
      }
    }

    /// <summary>
    /// Получить данные для рендеринга
    /// </summary>
    /// <returns></returns>
    public List<RenderingData.RenderingData?> GetRenderingData()
    {
      List<RenderingData.RenderingData?> renderingData = new List<RenderingData.RenderingData?>();
      foreach (var activeRenderableComponent in _activeRenderableComponents)
      {
        var rendData = activeRenderableComponent.Value.GetRenderingData();
        if (rendData != null)
        {
          renderingData.AddRange(rendData);
        }
      }

      return renderingData;
    }

    /// <summary>
    /// Действия при изменении настроек дисплея приложения
    /// </summary>
    public void OnDisplayChanged()
    {
      foreach (var activeRenderableComponent in _activeRenderableComponents)
      {
        activeRenderableComponent.Value.UpdateRenderingData();
      }

      RendereringDataWasUpdatedFlag = true;
    }

    /// <summary>
    /// Объект для связывания компонентов модели и отображения
    /// </summary>
    public ViewBehaviourOpenTkHandlersLinker TypesLinker { get; private set; }

    /// <summary>
    /// "Сотрудник" для взаимодействия с моделью через посредника
    /// </summary>
    public ViewProviderOpenTkViewSideColleague ViewSideProviderColleague { get; private set; }

    /// <summary>
    /// Пул менеджер компонентов OpenTK
    /// </summary>
    public PoolManager OpenTkViewComponentsPoolManager { get; private set; } = new PoolManager();

    /// <summary>
    /// Отображение приложения
    /// </summary>
    public AppViewOpenTk LinkedView { get; private set; }

    /// <summary>
    /// Модель приложения
    /// </summary>
    private IAppModel AppModelRef { get; set; }

    /// <summary>
    /// Текущая библиотека производных ассетов
    /// </summary>
    public SubassetsDataLibrary ActualSubassetsDataLibrary { get; private set; } = null;

    /// <summary>
    /// Флаг, сигнализирующий о том, что данные для рендеринга были изменены
    /// </summary>
    public bool RendereringDataWasUpdatedFlag { get; set; }
  }
}