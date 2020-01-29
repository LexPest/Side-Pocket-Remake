using CoreUtil.Math;

namespace Model.SPCore.GameplayModelDefinition.SidePocketGame.Data
{
  /// <summary>
  /// Луза
  /// </summary>
  public class Pocket
  {
    /// <summary>
    /// Центр лузы
    /// </summary>
    public SpVector3 Center { get; }
    
    /// <summary>
    /// Радиус лузы
    /// </summary>
    public double Radius { get; } = 11;

    /// <summary>
    /// Стандартный конструктор
    /// </summary>
    /// <param name="parCenter">Центр лузы</param>
    public Pocket(SpVector3 parCenter)
    {
      Center = parCenter;
    }
  }
}