using System;
using CoreUtil.Math;

namespace Model.SPCore.GameplayModelDefinition.SidePocketGame.Data
{
  /// <summary>
  /// Линия коллизии (использует формат обычной XOY-системы координат)
  /// </summary>
  public class CollisionLine
  {
    /// <summary>
    /// Функция для проверки коллизии с линией
    /// </summary>
    public Func<BallModelData, bool> CheckCollisionFunc;

    /// <summary>
    /// Нормаль
    /// </summary>
    public SpVector3 Normal;

    /// <summary>
    /// Стандартный конструктор
    /// </summary>
    /// <param name="parNormal">Нормаль</param>
    /// <param name="parCheckCollisionFunc">Функция для проверки коллизии с линией</param>
    public CollisionLine(SpVector3 parNormal, Func<BallModelData, bool> parCheckCollisionFunc)
    {
      CheckCollisionFunc = parCheckCollisionFunc;
      Normal = parNormal;
    }

    /// <summary>
    /// Дополнительный конструктор, генерирующий нормаль и функцию проверки коллизии самостоятельно
    /// </summary>
    /// <param name="parStart">Стартовая точка линии</param>
    /// <param name="parEnd">Конечная точка линии</param>
    /// <param name="parIsClockwiseNormal">Поиск нормали должен производится по часовой стрелке?</param>
    /// <param name="parIsCollisionCheckPreciseBottom">Точная проверка столкновения будет являтся пройденной,
    ///                                                если точка соприкосновения окажется снизу от линии?</param>
    public CollisionLine(SpVector3 parStart, SpVector3 parEnd, bool parIsClockwiseNormal,
      bool parIsCollisionCheckPreciseBottom)
    {
      Normal = SpVector3.GetNormal2DOnly(parEnd,
        parStart, parIsClockwiseNormal);

      CheckCollisionFunc = GenFunctionForPocketBorder(parStart, parEnd, parIsCollisionCheckPreciseBottom, Normal);
    }

    /// <summary>
    /// Получает коэффициенты уравнения прямой y = kx + b
    /// </summary>
    /// <param name="parStart">Первая точка</param>
    /// <param name="parEnd">Вторая точка</param>
    /// <param name="parEquationK">Коэффициент уравнения k</param>
    /// <param name="parEquationB">Коэффициент уравнения b</param>
    static void GetEquationKandB(SpVector3 parStart, SpVector3 parEnd, out double parEquationK, out double parEquationB)
    {
      double dY = parEnd.Y - parStart.Y;
      double dX = parEnd.X - parStart.X;

      parEquationK = dY / dX;
      parEquationB = parStart.Y - (parEquationK * parStart.X);
    }

    /// <summary>
    /// Проверка столкновения по модели прямоугольников AABB
    /// </summary>
    /// <param name="parBallData">Шар</param>
    /// <param name="parStart">Стартовая точка линии</param>
    /// <param name="parEnd">Конечная точка линии</param>
    /// <returns>True, если столкновение возможно</returns>
    static bool IsRectCollided(BallModelData parBallData, SpVector3 parStart, SpVector3 parEnd)
    {
      if (parBallData.Center.Y - parBallData.Radius > parEnd.Y)
      {
        return false;
      }

      if (parBallData.Center.Y + parBallData.Radius < parStart.Y)
      {
        return false;
      }

      // }
      double maxX = Math.Max(parStart.X, parEnd.X);
      double minX = Math.Min(parStart.X, parEnd.X);

      if (parBallData.Center.X - parBallData.Radius > maxX)
      {
        return false;
      }

      if (parBallData.Center.X + parBallData.Radius < minX)
      {
        return false;
      }

      return true;
    }

    /// <summary>
    /// Генерирует и возвращает функцию для проверки столкновения для заданной линии
    /// </summary>
    /// <param name="parStart">Начало линии</param>
    /// <param name="parEnd">Конец линии</param>
    /// <param name="parShouldCheckBottom">Точная проверка столкновения будет являтся пройденной,
    ///                                    если точка соприкосновения окажется снизу от линии?</param>
    /// <param name="parNormal">Нормаль</param>
    /// <returns></returns>
    static Func<BallModelData, bool> GenFunctionForPocketBorder(SpVector3 parStart, SpVector3 parEnd,
      bool parShouldCheckBottom,
      SpVector3 parNormal)
    {
      GetEquationKandB(parStart, parEnd,
        out double k, out double b);

      Func<BallModelData, bool> retFunc = parData =>
      {
        if (IsRectCollided(parData, parStart, parEnd))
        {
          SpVector3 pointToCheck = parData.Center + (-parNormal) * parData.Radius;

          //необходима более точная проверка
          if (parShouldCheckBottom)
          {
            return pointToCheck.Y < k * pointToCheck.X + b;
          }
          else
          {
            return pointToCheck.Y > k * pointToCheck.X + b;
          }
        }
        else
        {
          return false;
        }
      };

      return retFunc;
    }
  }
}