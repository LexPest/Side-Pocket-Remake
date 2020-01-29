using CoreUtil.Math;

namespace Model.SPCore.GameplayModelDefinition.SidePocketGame.Data.Setup
{
  /// <summary>
  /// Стартовая конфигурация шара
  /// </summary>
  public class BallStartConfig
  {
    /// <summary>
    /// Определенный тип шара
    /// </summary>
    public EBallType BallType { get; }

    /// <summary>
    /// Стартовая позиция на столе
    /// </summary>
    public SpVector3 StartCenterPosition { get; }

    /// <summary>
    /// Стандартный конструктор
    /// </summary>
    /// <param name="parBallType">Определенный тип шара</param>
    /// <param name="parStartCenterPosition">Стартовая позиция на столе</param>
    public BallStartConfig(EBallType parBallType, SpVector3 parStartCenterPosition)
    {
      BallType = parBallType;
      StartCenterPosition = parStartCenterPosition;
    }
  }
}