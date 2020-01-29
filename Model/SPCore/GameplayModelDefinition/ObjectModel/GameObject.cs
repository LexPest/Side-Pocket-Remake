using System;
using System.Collections.Generic;
using CoreUtil.Pool;
using Model.SPCore.GameplayModelDefinition.ComponentModel;

namespace Model.SPCore.GameplayModelDefinition.ObjectModel
{
  /// <summary>
  /// Игровой объект, представляющий собой специальный объект, к которому прикрепляются компоненты для
  /// реализации компонентно-ориентированного подхода. В конкретной реализации игровые объекты чаще используется
  /// для контекстуального разделения компонентов.
  /// </summary>
  public class GameObject : PoolSupportedObject
  {
    /// <summary>
    /// Конструктор для поддержки системы пулинга объектов
    /// </summary>
    /// <param name="parSupportData">Данные для работы с пулом</param>
    public GameObject(ObjectPoolSupportData parSupportData) : base(parSupportData)
    {
    }

    /// <summary>
    /// Замена конструктора, процедура инициализации игрового объекта
    /// </summary>
    /// <param name="parModel">Модель</param>
    public GameObject Init(IAppModel parModel)
    {
      LinkedAppModel = parModel;
      //Test = 3;
      Components = new List<Component>();

      return this;
    }

    /// <summary>
    /// Добавить компонент к игровому объекту
    /// </summary>
    /// <param name="parComponent">Добавляемый компонент</param>
    public void AddComponent(Component parComponent)
    {
      parComponent.OwnComponent(this);
      Components.Add(parComponent);
    }

    /// <summary>
    /// Добавить компонент к игровому объекту и вернуть его
    /// </summary>
    /// <param name="parComponent">Добавляемый компонент</param>
    /// <typeparam name="T">Тип компонента</typeparam>
    /// <returns>Добавленный компонент</returns>
    public T AddComponent<T>(Component parComponent) where T : Component
    {
      AddComponent(parComponent);
      return parComponent as T;
    }

    /// <summary>
    /// Удалить компонент из игрового объекта
    /// </summary>
    /// <param name="parComponent">Удаляемый компонент</param>
    public void RemoveComponent(Component parComponent)
    {
      Components.Remove(parComponent);
    }

    /// <summary>
    /// Получить компонент определенного типа, который находится
    /// на этом игровом объекте
    /// </summary>
    /// <param name="parT">Тип компонента</param>
    /// <typeparam name="T">Желаемый тип преобразования</typeparam>
    /// <returns>Компонент, если он был найден</returns>
    public T GetComponent<T>(Type parT) where T : class
    {
      return Components.Find(parX => parX is T) as T;
    }

    /// <summary>
    /// Получить все компоненты определенного типа, которые находятся
    /// на этом игровом объекте
    /// </summary>
    /// <param name="parT">Тип компонентов</param>
    /// <typeparam name="T">Желаемый тип преобразования</typeparam>
    /// <returns>Динамический массив компонентов</returns>
    public List<T> GetComponents<T>(Type parT) where T : class
    {
      return Components.FindAll(parX => parX is T) as List<T>;
    }

    /// <summary>
    /// Действия перед отправкой объекта в пул
    /// </summary>
    public override void OnBeforeResetPerform()
    {
      while (Components.Count > 0)
      {
        Components[0].DisableAndSendToPool();
      }


      base.OnBeforeResetPerform();
    }

    /// <summary>
    /// Связанная модель
    /// </summary>
    public IAppModel LinkedAppModel { get; private set; }

    /// <summary>
    /// Динамический массив находящийся на этом игровом объекте компонентов
    /// </summary>
    private List<Component> Components { get; set; }
  }
}