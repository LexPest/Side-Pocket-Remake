using CoreUtil.Math;

namespace Model.SPCore.GameplayModelDefinition.SidePocketGame.Data
{
  /// <summary>
  /// Данные модели шара
  /// </summary>
  public class BallModelData
  {
    /// <summary>
    /// Определенный тип шара
    /// </summary>
    public EBallType BallType { get; }

    /// <summary>
    /// Местоположение центра шара
    /// </summary>
    public SpVector3 Center;

    /// <summary>
    /// Радиус шара
    /// </summary>
    public double Radius = 4;

    /// <summary>
    /// Значение скорости шара
    /// </summary>
    public double Speed;

    /// <summary>
    /// Вектор направления скорости шара
    /// </summary>
    public SpVector3 Velocity;

    /// <summary>
    /// Стандартный конструктор
    /// </summary>
    /// <param name="parBallType">Определенный тип шара</param>
    public BallModelData(EBallType parBallType)
    {
      BallType = parBallType;
    }

    /// <summary>
    /// Останавливает шар
    /// </summary>
    public void ResetMovement()
    {
      Velocity = new SpVector3(0, 0);
      Speed = 0;
    }

    /// <summary>
    /// Перемещается ли шар в данный момент
    /// </summary>
    public bool IsMoving => Speed > 0;
  }
}