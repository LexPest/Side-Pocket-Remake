using System;
using System.Collections.Generic;
using System.Linq;

namespace ViewOpenTK.SPCore.ViewProvider
{
  /// <summary>
  /// Класс объекта, реализующего функционал связывания компонентов модели и отображения
  /// </summary>
  public class ViewBehaviourOpenTkHandlersLinker
  {
    /// <summary>
    /// Стандартный конструктор
    /// </summary>
    /// <param name="parLinkageData">Данные о типах для связывания</param>
    public ViewBehaviourOpenTkHandlersLinker(Dictionary<Type, Type> parLinkageData)
    {
      LinkageComponentMetaDataModelToView = parLinkageData;
    }

    /// <summary>
    /// Получить тип компонента модели для типа компонента отображения
    /// </summary>
    /// <param name="parViewComponentType">Тип компонента отображения</param>
    /// <returns></returns>
    public Type GetLinkedModelComponentType(Type parViewComponentType)
    {
      return LinkageComponentMetaDataModelToView.Values.FirstOrDefault(parX => parX == parViewComponentType);
    }

    /// <summary>
    /// Получить тип компонента отображения для типа компонента модели
    /// </summary>
    /// <param name="parModelComponentType">Тип компонента модели</param>
    /// <returns></returns>
    public Type GetLinkedViewComponentType(Type parModelComponentType)
    {
      return LinkageComponentMetaDataModelToView.TryGetValue(parModelComponentType, out Type viewComponentType)
        ? viewComponentType
        : null;
    }

    /// <summary>
    /// Данные о типах для связывания
    /// </summary>
    private Dictionary<Type, Type> LinkageComponentMetaDataModelToView { get; set; }
  }
}