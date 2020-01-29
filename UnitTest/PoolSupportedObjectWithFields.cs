using CoreUtil.Pool;

namespace UnitTest
{
  /// <summary>
  /// Класс объекта для тестирования механизма присвоения полям значений по умолчанию
  /// </summary>
  public class PoolSupportedObjectWithFields : PoolSupportedObject
  {
    /// <summary>
    /// Тестовое поле 1
    /// </summary>
    private TestReferenceType _testReferenceType;

    /// <summary>
    /// Тестовое поле 2
    /// </summary>
    protected TestReferenceType TestReferenceTypeProtected;

    /// <summary>
    /// Тестовое поле 3
    /// </summary>
    public TestReferenceType TestReferenceTypeWrongAndPublic;


    /// <summary>
    /// Тестовое поле 4
    /// </summary>
    private double _valueField;

    /// <summary>
    /// Тестовое поле 5
    /// </summary>
    protected double ValueFieldProtected;

    /// <summary>
    /// Тестовое поле 6
    /// </summary>
    public double ValueFieldWrongAndPublic;


    /// <summary>
    /// Конструктор для поддержки системы пулинга объектов
    /// </summary>
    /// <param name="parSupportData">Данные для работы с пулом</param>
    public PoolSupportedObjectWithFields(ObjectPoolSupportData supportData) : base(supportData)
    {
    }

    /// <summary>
    /// Замена конструктора, процедура инициализации объекта
    /// </summary>
    public PoolSupportedObjectWithFields Init()
    {
      _valueField = ValueFieldProtected = ValueFieldWrongAndPublic = 5;
      _testReferenceType = new TestReferenceType();
      TestReferenceTypeProtected = new TestReferenceType();
      TestReferenceTypeWrongAndPublic = new TestReferenceType();

      return this;
    }

    /// <summary>
    /// Присвоены ли полям и свойствам значения по умолчанию?
    /// </summary>
    /// <returns>True, если всем полям и свойствам объекта присвоены значения по умолчанию</returns>
    public bool IsEverythingDefault()
    {
      return _valueField == 0 && ValueFieldProtected == 0 && ValueFieldWrongAndPublic == 0 &&
             _testReferenceType == null && TestReferenceTypeProtected == null &&
             TestReferenceTypeWrongAndPublic == null;
    }
  }
}