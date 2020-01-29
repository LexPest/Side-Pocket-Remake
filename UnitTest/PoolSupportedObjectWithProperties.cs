using CoreUtil.Pool;

namespace UnitTest
{
  /// <summary>
  /// Класс объекта для тестирования механизма присвоения свойствам значений по умолчанию
  /// </summary>
  public class PoolSupportedObjectWithProperties : PoolSupportedObject
  {
    /// <summary>
    /// Тестовое поле для свойства 1
    /// </summary>
    private int _backFieldValueProp;
    
    /// <summary>
    /// Тестовое поле для свойства 2
    /// </summary>
    private int _backFieldValuePropProtected;
    
    /// <summary>
    /// Тестовое поле для свойства 3
    /// </summary>
    private int _backFieldValuePropPublic;

    
    /// <summary>
    /// Конструктор для поддержки системы пулинга объектов
    /// </summary>
    /// <param name="parSupportData">Данные для работы с пулом</param>
    public PoolSupportedObjectWithProperties(ObjectPoolSupportData supportData) : base(supportData)
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
    /// Замена конструктора, процедура инициализации объекта
    /// </summary>
    public PoolSupportedObjectWithProperties Init()
    {
      ValueProp = ValuePropProtected = ValuePropPublic =
        BackFieldValuePropPublic = BackFieldValuePropProtected = BackFieldValueProp = 5;
      TestReferenceType = new TestReferenceType();
      TestReferenceTypeProtected = new TestReferenceType();
      TestReferenceTypePublic = new TestReferenceType();

      return this;
    }

    /// <summary>
    /// Присвоены ли полям и свойствам значения по умолчанию?
    /// </summary>
    /// <returns>True, если всем полям и свойствам объекта присвоены значения по умолчанию</returns>
    public bool IsEverythingDefault()
    {
      return ValueProp == 0 && ValuePropProtected == 0 && ValuePropPublic == 0 &&
             BackFieldValuePropPublic == 0 && BackFieldValuePropProtected == 0 && BackFieldValueProp == 0 &&
             TestReferenceType == null && TestReferenceTypeProtected == null &&
             TestReferenceTypePublic == null;
    }
  }
}