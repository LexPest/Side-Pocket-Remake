using System;
using CoreUtil.Pool;

namespace UnitTest
{
  /// <summary>
  /// Класс объекта для совмещенного тестирования
  /// </summary>
  public class PoolSupportedObjectCombined : PoolSupportedObject
  {
    /// <summary>
    /// Тестовое поле 1
    /// </summary>
    private int _backFieldValueProp;

    /// <summary>
    /// Тестовое поле 2
    /// </summary>
    private int _backFieldValuePropProtected;

    /// <summary>
    /// Тестовое поле 3
    /// </summary>
    private int _backFieldValuePropPublic;

    /// <summary>
    /// Тестовое поле 4
    /// </summary>
    private TestReferenceType _testReferenceTypeField;

    /// <summary>
    /// Тестовое поле 5
    /// </summary>
    protected TestReferenceType TestReferenceTypeProtectedField;

    /// <summary>
    /// Тестовое поле 6
    /// </summary>
    public TestReferenceType TestReferenceTypeWrongAndPublic;

    /// <summary>
    /// Тестовое поле 7
    /// </summary>
    private double _valueField;

    /// <summary>
    /// Тестовое поле 8
    /// </summary>
    protected double ValueFieldProtected;

    /// <summary>
    /// Тестовое поле 9
    /// </summary>
    public double ValueFieldWrongAndPublic;

    /// <summary>
    /// Конструктор для поддержки системы пулинга объектов
    /// </summary>
    /// <param name="parSupportData">Данные для работы с пулом</param>
    public PoolSupportedObjectCombined(ObjectPoolSupportData parSupportData) : base(parSupportData)
    {
    }

    /// <summary>
    /// Тестовое свойство 1
    /// </summary>
    private double ValueProp { get; set; }

    /// <summary>
    /// Тестовое свойство 2
    /// </summary>
    protected double ValuePropProtected { get; set; }

    /// <summary>
    /// Тестовое свойство 3
    /// </summary>
    public double ValuePropPublic { get; set; }

    /// <summary>
    /// Тестовое свойство 4
    /// </summary>
    private int BackFieldValueProp
    {
      get { return _backFieldValueProp; }
      set { _backFieldValueProp = value; }
    }

    /// <summary>
    /// Тестовое свойство 5
    /// </summary>
    protected int BackFieldValuePropProtected
    {
      get => _backFieldValuePropProtected;
      set => _backFieldValuePropProtected = value;
    }

    /// <summary>
    /// Тестовое свойство 6
    /// </summary>
    public int BackFieldValuePropPublic
    {
      get => _backFieldValuePropPublic;
      set => _backFieldValuePropPublic = value;
    }

    /// <summary>
    /// Тестовое свойство 7
    /// </summary>
    private TestReferenceType TestReferenceType { get; set; }

    /// <summary>
    /// Тестовое свойство 8
    /// </summary>
    protected TestReferenceType TestReferenceTypeProtected { get; set; }

    /// <summary>
    /// Тестовое свойство 9
    /// </summary>
    public TestReferenceType TestReferenceTypePublic { get; set; }

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
    public PoolSupportedObjectCombined Init()
    {
      Event1 += ExampleInvokableMethod1;
      Event1 += ExampleInvokableMethod2;
      Event2 += ExampleInvokableMethod1;
      Event2 += ExampleInvokableMethod2;
      Event3 += ExampleInvokableMethod1;
      Event3 += ExampleInvokableMethod2;

      _valueField = ValueFieldProtected = ValueFieldWrongAndPublic = 5;
      _testReferenceTypeField = new TestReferenceType();
      TestReferenceTypeProtectedField = new TestReferenceType();
      TestReferenceTypeWrongAndPublic = new TestReferenceType();

      ValueProp = ValuePropProtected = ValuePropPublic =
        BackFieldValuePropPublic = BackFieldValuePropProtected = BackFieldValueProp = 5;
      TestReferenceType = new TestReferenceType();
      TestReferenceTypeProtected = new TestReferenceType();
      TestReferenceTypePublic = new TestReferenceType();

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

    /// <summary>
    /// Присвоены ли полям и свойствам значения по умолчанию?
    /// </summary>
    /// <returns>True, если всем полям и свойствам объекта присвоены значения по умолчанию</returns>
    public bool IsEverythingDefault()
    {
      return _valueField == 0 && ValueFieldProtected == 0 && ValueFieldWrongAndPublic == 0 &&
             _testReferenceTypeField == null && TestReferenceTypeProtectedField == null &&
             TestReferenceTypeWrongAndPublic == null && ValueProp == 0 && ValuePropProtected == 0 &&
             ValuePropPublic == 0 &&
             BackFieldValuePropPublic == 0 && BackFieldValuePropProtected == 0 && BackFieldValueProp == 0 &&
             TestReferenceType == null && TestReferenceTypeProtected == null &&
             TestReferenceTypePublic == null;
    }
  }
}