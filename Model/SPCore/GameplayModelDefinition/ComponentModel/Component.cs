using CoreUtil.Pool;
using Model.SPCore.GameplayModelDefinition.ObjectModel;

namespace Model.SPCore.GameplayModelDefinition.ComponentModel
{
  /// <summary>
  /// Базовый класс для игровых компонентов компонентно-ориентированной системы проектирования
  /// </summary>
  public abstract class Component : PoolSupportedObject, IUpdatable, IFixedUpdatable
  {
    /// <summary>
    /// Конструктор для поддержки системы пулинга объектов
    /// </summary>
    /// <param name="parSupportData">Данные для работы с пулом</param>
    public Component(ObjectPoolSupportData parSupportData) : base(parSupportData)
    {
    }

    /// <summary>
    /// Сигнал фиксированного обновления модели (используется преимущественно для обработки физики и ввода)
    /// </summary>
    /// <param name="parFixedDeltaTime">Время шага фиксированного обновления в секундах</param>
    public virtual void FixedUpdate(double parFixedDeltaTime)
    {
    }

    /// <summary>
    /// Является ли активным объект
    /// </summary>
    /// <returns></returns>
    bool IFixedUpdatable.IsActive()
    {
      return IsFixedUpdatable;
    }

    /// <summary>
    /// Сигнал обновления модели
    /// </summary>
    /// <param name="parDeltaTime">Время кадра в секундах</param>
    public virtual void Update(double parDeltaTime)
    {
    }

    /// <summary>
    /// Является ли активным объект
    /// </summary>
    /// <returns></returns>
    bool IUpdatable.IsActive()
    {
      return IsUpdatable;
    }

    /// <summary>
    /// Активация владения компонентом другим игровым объектом
    /// </summary>
    /// <param name="parGameObject"></param>
    public void OwnComponent(GameObject parGameObject)
    {
      ParentGameObject = parGameObject;
    }

    /// <summary>
    /// Замена конструктора, процедура инициализации компонента
    /// </summary>
    /// <param name="parEntGameObject">Родительский игровой объект</param>
    /// <param name="parIsUpdatable">Будет ли получать сигналы обновления модели</param>
    /// <param name="parIsFixedUpdatable">Будет ли получать сигналы фиксированного обновления модели</param>
    protected virtual void Init(GameObject parEntGameObject, bool parIsUpdatable, bool parIsFixedUpdatable)
    {
      ParentGameObject = parEntGameObject;
      IsUpdatable = parIsUpdatable;
      IsFixedUpdatable = parIsFixedUpdatable;


      if (parIsUpdatable)
      {
        parEntGameObject.LinkedAppModel.RegisterUpdatableObject(this);
      }

      if (parIsFixedUpdatable)
      {
        parEntGameObject.LinkedAppModel.RegisterFixedUpdatableObject(this);
      }
    }

    /// <summary>
    /// Выполнить действия перед отправкой в пул
    /// </summary>
    public override void OnBeforeResetPerform()
    {
      ParentGameObject?.RemoveComponent(this);

      if (IsUpdatable)
      {
        ParentGameObject.LinkedAppModel.UnregisterUpdatableObject(this);
      }

      if (IsFixedUpdatable)
      {
        ParentGameObject.LinkedAppModel.UnregisterFixedUpdatableObject(this);
      }

      base.OnBeforeResetPerform();
    }

    /// <summary>
    /// Родительский игровой объект
    /// </summary>
    public GameObject ParentGameObject { get; private set; }

    /// <summary>
    /// Получает ли сигналы обновления модели
    /// </summary>
    private bool IsUpdatable { get; set; }
    
    /// <summary>
    /// Получает ли сигналы фиксированного обновления модели
    /// </summary>
    private bool IsFixedUpdatable { get; set; }
  }
}