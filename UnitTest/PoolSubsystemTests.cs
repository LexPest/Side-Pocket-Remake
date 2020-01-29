using System;
using System.Linq;
using CoreUtil.Pool;
using NUnit.Framework;


namespace UnitTest
{
  /// <summary>
  /// Модульные тесты для подсистемы пулинга
  /// </summary>
  [TestFixture]
  public class PoolSubsystemTests
  {
    /// <summary>
    /// Метод для экспериментального вызова во время тестов
    /// </summary>
    private void ExampleInvokableMethodOutside()
    {
      Console.WriteLine("Example method outside called");
    }

    /// <summary>
    /// Совмещенное тестирование
    /// </summary>
    [Test]
    public void CombinedCleanupTest()
    {
      PoolManager poolManager = new PoolManager();

      PoolSupportedObjectCombined poolSupportedObjectCombined =
        poolManager.GetObject<PoolSupportedObjectCombined>(typeof(PoolSupportedObjectCombined));

      poolSupportedObjectCombined.Init();

      poolSupportedObjectCombined.Event1 += ExampleInvokableMethodOutside;

      Assert.That(!poolSupportedObjectCombined.IsEverythingDefault());

      if (!poolSupportedObjectCombined.IsAtLeastOneEventInvoked())
      {
        Assert.Fail("Problems with events invokation inside of pool supported object");
      }

      poolSupportedObjectCombined.BackFieldValuePropPublic = 12;

      poolSupportedObjectCombined.DisableAndSendToPool();

      Assert.That(poolSupportedObjectCombined.IsEverythingDefault());
      Assert.That(!poolSupportedObjectCombined.IsAtLeastOneEventInvoked());
    }

/// <summary>
/// Тестирование механизма отписывания от событий
/// </summary>
    [Test]
    public void EventsCleanupTest()
    {
      PoolManager poolManager = new PoolManager();

      PoolSupportedObjectWithEvents poolSupportedObjectWithEvents =
        poolManager.GetObject<PoolSupportedObjectWithEvents>(typeof(PoolSupportedObjectWithEvents));

      //Assert.That(poolSupportedObjectWithEvents.IsAtLeastOneEventInvoked);

      poolSupportedObjectWithEvents.Init();
      poolSupportedObjectWithEvents.Event1 += ExampleInvokableMethodOutside;

      if (!poolSupportedObjectWithEvents.IsAtLeastOneEventInvoked())
      {
        Assert.Fail("Problems with events invokation inside of pool supported object");
      }

      poolSupportedObjectWithEvents.DisableAndSendToPool();

      Assert.That(!poolSupportedObjectWithEvents.IsAtLeastOneEventInvoked());
    }

    /// <summary>
    /// Тестирование присвоения полям значений по умолчанию
    /// </summary>
    [Test]
    public void FieldsCleanupTest()
    {
      PoolManager poolManager = new PoolManager();

      PoolSupportedObjectWithFields poolSupportedObjectWithFields =
        poolManager.GetObject<PoolSupportedObjectWithFields>(typeof(PoolSupportedObjectWithFields));

      poolSupportedObjectWithFields.Init();

      Assert.That(!poolSupportedObjectWithFields.IsEverythingDefault());

      poolSupportedObjectWithFields.DisableAndSendToPool();

      Assert.That(poolSupportedObjectWithFields.IsEverythingDefault());
    }

    /// <summary>
    /// Общее тестирование хранения объектов в пуле
    /// </summary>
    [Test]
    public void PoolingGeneralObjectStorageTest()
    {
      PoolManager poolManager = new PoolManager();

      Assert.That(poolManager.PoolContents.Count == 0);

      PoolSupportedObjectWithEvents poolSupportedObjectWithEvents =
        poolManager.GetObject<PoolSupportedObjectWithEvents>(typeof(PoolSupportedObjectWithEvents));

      Assert.That(poolManager.PoolContents.Count == 0);

      PoolSupportedObjectWithFields poolSupportedObjectWithFields =
        poolManager.GetObject<PoolSupportedObjectWithFields>(typeof(PoolSupportedObjectWithFields));

      Assert.That(poolManager.PoolContents.Count == 0);

      PoolSupportedObjectWithProperties poolSupportedObjectWithProperties =
        poolManager.GetObject<PoolSupportedObjectWithProperties>(typeof(PoolSupportedObjectWithProperties));

      Assert.That(poolManager.PoolContents.Count == 0);

      poolSupportedObjectWithEvents.DisableAndSendToPool();

      Assert.That(poolManager.PoolContents.Count == 1);
      Assert.That(poolManager.PoolContents.First().Value.Count == 1);

      PoolSupportedObjectWithEvents poolSupportedObjectWithEventsDublicated =
        poolManager.GetObject<PoolSupportedObjectWithEvents>(typeof(PoolSupportedObjectWithEvents));

      Assert.That(poolManager.PoolContents.First().Value.Count == 0);
      Assert.AreEqual(poolSupportedObjectWithEvents, poolSupportedObjectWithEventsDublicated);

      poolSupportedObjectWithFields.DisableAndSendToPool();
      poolSupportedObjectWithProperties.DisableAndSendToPool();

      Assert.That(poolManager.PoolContents.Count == 3);

      int objectsOperationsCount = 100;

      PoolSupportedObject[] supportedObjects = new PoolSupportedObject[objectsOperationsCount];

      for (int i = 0; i < objectsOperationsCount; i++)
      {
        supportedObjects[i] = poolManager.GetObject<PoolSupportedObject>(typeof(PoolSupportedObjectWithEvents));
        supportedObjects[i].DisableAndSendToPool();
      }

      Assert.That(poolManager.PoolContents.First().Value.Count == 1);
    }

    /// <summary>
    /// Тестирование корректности заполнения и хранения кэшированных метаданных типов
    /// </summary>
    [Test]
    public void PoolingGeneralTypesInfoGenerationTest()
    {
      PoolManager poolManager = new PoolManager();

      Assert.That(poolManager.PoolContents.Count == 0);
      Assert.That(poolManager.TypesSupportData.Count == 0);

      PoolSupportedObjectWithEvents poolSupportedObjectWithEvents =
        poolManager.GetObject<PoolSupportedObjectWithEvents>(typeof(PoolSupportedObjectWithEvents));

      Assert.That(poolManager.PoolContents.Count == 0);
      Assert.That(poolManager.TypesSupportData.Count == 1);
      Assert.That(poolManager.TypesSupportData.First().Key == poolSupportedObjectWithEvents.GetType());

      PoolSupportedObjectWithProperties poolSupportedObjectWithProperties =
        poolManager.GetObject<PoolSupportedObjectWithProperties>(typeof(PoolSupportedObjectWithProperties));
      Assert.That(poolManager.PoolContents.Count == 0);
      Assert.That(poolManager.TypesSupportData.Count == 2);

      poolSupportedObjectWithProperties.DisableAndSendToPool();
      poolSupportedObjectWithEvents.DisableAndSendToPool();

      Assert.That(poolManager.PoolContents.Count != 0);
      Assert.That(poolManager.TypesSupportData.Count == 2);
    }

    /// <summary>
    /// Тестирование присвоения свойствам значений по умолчанию
    /// </summary>
    [Test]
    public void PropertiesCleanupTest()
    {
      PoolManager poolManager = new PoolManager();

      PoolSupportedObjectWithProperties poolSupportedObjectWithProperties =
        poolManager.GetObject<PoolSupportedObjectWithProperties>(typeof(PoolSupportedObjectWithProperties));

      poolSupportedObjectWithProperties.Init();

      Assert.That(!poolSupportedObjectWithProperties.IsEverythingDefault());

      poolSupportedObjectWithProperties.DisableAndSendToPool();

      Assert.That(poolSupportedObjectWithProperties.IsEverythingDefault());
    }
  }
}