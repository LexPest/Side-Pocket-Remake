using System;
using CoreUtil.Pool;

namespace UnitTest
{
  /// <summary>
  /// Класс объекта для тестирования механизма отписывания от событий
  /// </summary>
  internal class PoolSupportedObjectWithEvents : PoolSupportedObject
  {
    /// <summary>
    /// Конструктор для поддержки системы пулинга объектов
    /// </summary>
    /// <param name="parSupportData">Данные для работы с пулом</param>
    public PoolSupportedObjectWithEvents(ObjectPoolSupportData supportData) : base(supportData)
    {
    }

    /// <summary>
    /// Тестовое событие 1
    /// </summary>
    public event Action Event1;

    /// <summary>
    /// Тестовое событие 2
    /// </summary>
    protected event Action Event2;

    /// <summary>
    /// Тестовое событие 3
    /// </summary>
    private event Action Event3;

    /// <summary>
    /// Замена конструктора, процедура инициализации объекта
    /// </summary>
    public PoolSupportedObjectWithEvents Init()
    {
      Event1 += ExampleInvokableMethod1;
      Event1 += ExampleInvokableMethod2;
      Event2 += ExampleInvokableMethod1;
      Event2 += ExampleInvokableMethod2;
      Event3 += ExampleInvokableMethod1;
      Event3 += ExampleInvokableMethod2;

      return this;
    }

    /// <summary>
    /// Тестовый вызываемый по событию метод 1
    /// </summary>
    private void ExampleInvokableMethod1()
    {
      Console.WriteLine("Example method 1 called");
    }

    /// <summary>
    /// Тестовый вызываемый по событию метод 2
    /// </summary>
    private void ExampleInvokableMethod2()
    {
      Console.WriteLine("Example method 2 called");
    }

    /// <summary>
    /// Есть ли подписчик хотя бы у одного из событий?
    /// </summary>
    /// <returns>True, если есть подписчик хотя бы у одного из событий</returns>
    public bool IsAtLeastOneEventInvoked()
    {
      if (Event1 != null || Event2 != null || Event3 != null)
      {
        return true;
      }

      return false;
    }
  }
}