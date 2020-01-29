using System;

namespace ViewOpenTK.AssetData.DataTypes.AudioFormatProviders
{
  /// <summary>
  /// Интерфейс для реализаций работы с различными форматами аудио
  /// </summary>
  public interface IAudioFormatProvider : IDisposable
  {
    /// <summary>
    /// Инициализация и первоначальная загрузка аудио данных
    /// </summary>
    /// <param name="parLinkedOpenTkSound">Аудио данные OpenTK</param>
    void InitAudioData(AssetDataOpenTkWaveSound parLinkedOpenTkSound);

    /// <summary>
    /// Воспроизведение
    /// </summary>
    /// <param name="parIsLooped">Зациклить воспроизведение?</param>
    void Play(bool parIsLooped);

    /// <summary>
    /// Приостановить воспроизведение
    /// </summary>
    void Pause();

    /// <summary>
    /// Остановить воспроизведение
    /// </summary>
    void Stop();

    /// <summary>
    /// Сбросить воспроизведение
    /// </summary>
    void Reset();

    /// <summary>
    /// Воспроизводится ли сейчас?
    /// </summary>
    /// <returns>True, если воспроизводится</returns>
    bool IsPlaying();

    /// <summary>
    /// На паузе ли сейчас?
    /// </summary>
    /// <returns>True, если на паузе</returns>
    bool IsPaused();

    /// <summary>
    /// Установка параметра зацикленности воспроизведения
    /// </summary>
    /// <param name="parIsLooped">Значение параметра зацикленности воспроизведения</param>
    void SetIsLooped(bool parIsLooped);
  }
}