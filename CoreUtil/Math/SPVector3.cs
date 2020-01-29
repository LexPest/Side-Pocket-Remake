namespace CoreUtil.Math
{
  /// <summary>
  /// Пространственный вектор с тремя компонентами
  /// </summary>
  public struct SpVector3
  {
    /// <summary>
    /// Компонента вектора
    /// </summary>
    private double _x, _y, _z;

    /// <summary>
    /// Компонента апликат
    /// </summary>
    public double Z
    {
      get { return _z; }
      set { _z = value; }
    }

    /// <summary>
    /// Компонента ординат
    /// </summary>
    public double Y
    {
      get { return _y; }
      set { _y = value; }
    }

    /// <summary>
    /// Компонента абсцисс
    /// </summary>
    public double X
    {
      get { return _x; }
      set { _x = value; }
    }


    /// <summary>
    /// Конструктор, принимающий 3 компоненты
    /// </summary
    /// <param name="parX1">Компонента абсцисс</param>
    /// <param name="parY1">Компонента ординат</param>
    /// <param name="parZ1">Компонента апликат</param>
    public SpVector3(double parX1, double parY1, double parZ1)
    {
      _x = parX1;
      _y = parY1;
      _z = parZ1;
    }

    /// <summary>
    /// Конструктор, принимающий 2 компоненты
    /// </summary>
    /// <param name="parX">Компонента абсцисс</param>
    /// <param name="parY">Компонента ординат</param>
    public SpVector3(double parX, double parY)
    {
      this._x = parX;
      this._y = parY;
      this._z = 0;
    }

    /// <summary>
    /// Получить нормализованный вектор
    /// </summary>
    /// <returns></returns>
    public SpVector3 Normalize()
    {
      double len = Length();
      return new SpVector3(_x /= len, _y /= len, _z /= len);
    }

    /// <summary>
    /// Конструктор, принимающий другой вектор
    /// </summary>
    /// <param name="parV">Другой вектор</param>
    public SpVector3(SpVector3 parV)
    {
      _x = parV._x;
      _y = parV._y;
      _z = parV._z;
    }

    /// <summary>
    /// Длина вектора
    /// </summary>
    /// <returns></returns>
    public double Length()
    {
      return System.Math.Sqrt(_x * _x + _y * _y + _z * _z);
    }

    /// <summary>
    /// Равен ли вектор другому
    /// </summary>
    /// <param name="parV2">Другой вектор</param>
    /// <returns></returns>
    public bool Equal(SpVector3 parV2)
    {
      if (_x == parV2.X && _y == parV2.Y && _z == parV2.Z
      ) //true if all components in one vector is equal to components in another.
        return true;
      else
        return false;
    }

    /// <summary>
    /// Скалярное произведение векторов
    /// </summary>
    /// <param name="parV2">Второй вектор</param>
    /// <returns></returns>
    public double DotProduct(SpVector3 parV2)
    {
      return (_x * parV2.X + _y * parV2.Y + _z * parV2.Z);
    }

    /// <summary>
    /// Перекрестное произведение векторов
    /// </summary>
    /// <param name="parV2">Второй вектор</param>
    /// <returns></returns>
    public SpVector3 CrossProduct(SpVector3 parV2)
    {
      return new SpVector3(_y * parV2.Z - _z * parV2.Y, _z * parV2.X - _x * parV2.Z, _x * parV2.Y - _y * parV2.X);
    }

    /// <summary>
    /// Компонент переноса
    /// </summary>
    /// <param name="parV2">Второй вектор</param>
    /// <returns></returns>
    public SpVector3 ParralelComponent(SpVector3 parV2)
    {
      double lengthSquared, dotProduct, scale;
      lengthSquared = Length() * Length();
      dotProduct = DotProduct(parV2);
      if (lengthSquared != 0)
        scale = dotProduct / lengthSquared;
      else
        return new SpVector3(0, 0);
      return new SpVector3(this.Scale(scale));
    }

    /// <summary>
    /// Компонент перпендикуляра
    /// </summary>
    /// <param name="parV2"></param>
    /// <returns></returns>
    public SpVector3 PerpendicularComponent(SpVector3 parV2)
    {
      return new SpVector3(parV2 - this.ParralelComponent(parV2));
    }

    /// <summary>
    /// Умножить на скаляр
    /// </summary>
    /// <param name="parScale">Скаляр</param>
    /// <returns></returns>
    public SpVector3 Scale(double parScale)
    {
      return new SpVector3(parScale * _x, parScale * _y, parScale * _z); //Multiplys by scalar
    }

    /// <summary>
    /// Оператор умножения числа на вектор
    /// </summary>
    /// <param name="parK">Число</param>
    /// <param name="parV1">Вектор</param>
    /// <returns></returns>
    public static SpVector3 operator *(double parK, SpVector3 parV1)
    {
      return new SpVector3(parK * parV1._x, parK * parV1._y, parK * parV1._z);
    }

    /// <summary>
    /// Оператор умножения вектора на число
    /// </summary>
    /// <param name="parV1">Вектор</param>
    /// <param name="parK">Число</param>
    /// <returns></returns>
    public static SpVector3 operator *(SpVector3 parV1, double parK)
    {
      return new SpVector3(parK * parV1._x, parK * parV1._y, parK * parV1._z);
    }

    /// <summary>
    /// Оператор сложения векторов
    /// </summary>
    /// <param name="parV1">Вектор 1</param>
    /// <param name="parV2">Вектор 2</param>
    /// <returns></returns>
    public static SpVector3 operator +(SpVector3 parV1, SpVector3 parV2)
    {
      return new SpVector3(parV1.X + parV2.X, parV1.Y + parV2.Y, parV1.Z + parV2.Z); //adds
    }

    /// <summary>
    /// Оператор вычитания векторов
    /// </summary>
    /// <param name="parV1">Вектор 1</param>
    /// <param name="parV2">Вектор 2</param>
    /// <returns></returns>
    public static SpVector3 operator -(SpVector3 parV1, SpVector3 parV2)
    {
      return new SpVector3(parV1.X - parV2.X, parV1.Y - parV2.Y, parV1.Z - parV2.Z); //subtracts
    }

    /// <summary>
    /// Получить нормаль к линии в двухмерном пространстве
    /// </summary>
    /// <param name="parStart">Точка начала линии</param>
    /// <param name="parEnd">Точка конца линии</param>
    /// <param name="parIsClockwise">Нормаль нужна по часовой стрелке?</param>
    /// <returns></returns>
    public static SpVector3 GetNormal2DOnly(SpVector3 parStart, SpVector3 parEnd, bool parIsClockwise)
    {
      SpVector3 resultVector = parEnd - parStart;
      if (parIsClockwise)
      {
        return new SpVector3(resultVector.Y, -resultVector.X, resultVector.Z).Normalize();
      }
      else
      {
        return new SpVector3(-resultVector.Y, resultVector.X, resultVector.Z).Normalize();
      }
    }

    /// <summary>
    /// Унарный оператор "минус" для вектора
    /// </summary>
    /// <param name="parV1">Вектор</param>
    /// <returns></returns>
    public static SpVector3 operator -(SpVector3 parV1)
    {
      return new SpVector3(-parV1._x, -parV1._y, -parV1._z);
    }
  }
}