using System;
using OpenTK.Audio;
using OpenTK.Audio.OpenAL;
using ViewOpenTK.AssetData.DataTypes;

namespace ViewOpenTK.OpenAL.BasicManager
{
  /// <summary>
  /// Менеджер-обертка OpenAL
  /// </summary>
  public sealed class OpenAlManager
  {
    /// <summary>
    /// Приватный конструктор
    /// </summary>
    private OpenAlManager()
    {
      //инициализация подсистемы OpenAL
      CheckForError("OpenAL Pre-Init Error Occured");

      AudioContext = new AudioContext();

      CheckForError("OpenAL Init Error Occured",
        $"OpenAL initialized, default device is: {AudioContext.DefaultDevice}");
    }

    /// <summary>
    /// Проверить на ошибочное состояние OpenAL
    /// </summary>
    /// <param name="parMessageErr">Префикс сообщения в случае ошибки</param>
    /// <param name="parMessageOk">Сообщение в случае отсутствия ошибки</param>
    /// <returns>True, если ошибка отсутствует</returns>
    public static bool CheckForError(string parMessageErr, string parMessageOk = null)
    {
      ALError error = AL.GetError();
      if (error != ALError.NoError)
      {
        Console.WriteLine($"AL: {parMessageErr}: {error}");
        return false;
      }
      else if (parMessageOk != null)
      {
        Console.WriteLine($"AL: {parMessageOk}");
      }

      return true;
    }

    /// <summary>
    /// Воспроизвести
    /// </summary>
    /// <param name="parWaveSound">Аудио ассет OpenTK</param>
    /// <param name="parIsLooped">Зациклить воспроизведение?</param>
    public void Play(AssetDataOpenTkWaveSound parWaveSound, bool parIsLooped = false)
    {
      parWaveSound.AudioFormatProvider.Play(parIsLooped);
    }

    /// <summary>
    /// Приостановить воспроизведение
    /// </summary>
    /// <param name="parWaveSound">Аудио ассет OpenTK</param>
    public void Pause(AssetDataOpenTkWaveSound parWaveSound)
    {
      parWaveSound.AudioFormatProvider.Pause();
    }

    /// <summary>
    /// Остановить воспроизведение
    /// </summary>
    /// <param name="parWaveSound">Аудио ассет OpenTK</param>
    public void Stop(AssetDataOpenTkWaveSound parWaveSound)
    {
      parWaveSound.AudioFormatProvider.Stop();
    }

    /// <summary>
    /// Сбросить и перемотать воспроизведение
    /// </summary>
    /// <param name="parWaveSound">Аудио ассет OpenTK</param>
    public void Rewind(AssetDataOpenTkWaveSound parWaveSound)
    {
      parWaveSound.AudioFormatProvider.Reset();
    }

    /// <summary>
    /// Воспроизводится ли сейчас?
    /// </summary>
    /// <param name="parWaveSound">Аудио ассет OpenTK</param>
    public bool IsPlaying(AssetDataOpenTkWaveSound parWaveSound)
    {
      return parWaveSound.AudioFormatProvider.IsPlaying();
    }

    /// <summary>
    /// На паузе ли сейчас воспроизведение?
    /// </summary>
    /// <param name="parWaveSound">Аудио ассет OpenTK</param>
    public bool IsPaused(AssetDataOpenTkWaveSound parWaveSound)
    {
      return parWaveSound.AudioFormatProvider.IsPaused();
    }

    /// <summary>
    /// Установить флаг-признак зацикливания воспроизведения
    /// </summary>
    /// <param name="parWaveSound">Аудио ассет OpenTK</param>
    /// <param name="parIsLooped">Значение признака зацикливания воспроизведения</param>
    public void SetIsLooped(AssetDataOpenTkWaveSound parWaveSound, bool parIsLooped)
    {
      parWaveSound.AudioFormatProvider.SetIsLooped(parIsLooped);
    }

    /// <summary>
    /// Аудио контекст OpenAL
    /// </summary>
    private AudioContext AudioContext { get; set; }

    #region Singleton

    private static OpenAlManager _instance;

    public static OpenAlManager Instance
    {
      get
      {
        if (_instance == null)
        {
          _instance = new OpenAlManager();
        }

        return _instance;
      }
    }

    public static OpenAlManager GetInstance()
    {
      return Instance;
    }

    #endregion
  }
}