using System;

namespace Model.SPCore.DS
{
  /// <summary>
  /// Состояние приложения
  /// </summary>
  [Serializable]
  public sealed class AppState
  {
    /// <summary>
    /// Определенное состояние приложения
    /// </summary>
    public EBaseAppStates CurrentBaseAppState;
  }
}