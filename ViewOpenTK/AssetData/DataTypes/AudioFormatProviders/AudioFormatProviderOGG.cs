using System;
using System.Threading;
using System.Threading.Tasks;
using DragonOgg.MediaPlayer;

namespace ViewOpenTK.AssetData.DataTypes.AudioFormatProviders
{
  /// <summary>
  /// Реализация работы с форматом аудио OGG Vorbis
  /// </summary>
  public class AudioFormatProviderOgg : IAudioFormatProvider
  {
    /// <summary>
    /// Флаг-признак окончания задачи зацикленного воспроизведения
    /// </summary>
    private bool _loopTaskEnded = false;

    /// <summary>
    /// Инициализация и первоначальная загрузка аудио данных
    /// </summary>
    /// <param name="parLinkedOpenTkSound">Аудио данные OpenTK</param>
    public void InitAudioData(AssetDataOpenTkWaveSound parLinkedOpenTkSound)
    {
      OggPlayerInstance = new OggPlayerFBN(); // создаем проигрыватель
      Console.WriteLine($"Setting file path {parLinkedOpenTkSound.ActualAssetMetadata.FilePath}");
      OggPlayerInstance.SetCurrentFile(parLinkedOpenTkSound.ActualAssetMetadata.FilePath);
    }

    /// <summary>
    /// Воспроизведение
    /// </summary>
    /// <param name="parIsLooped">Зациклить воспроизведение?</param>
    public void Play(bool parIsLooped)
    {
      IsLooped = parIsLooped;
      OggPlayerInstance.Play();
      UpdateLoopedTask();
    }

    /// <summary>
    /// Приостановить воспроизведение
    /// </summary>
    public void Pause()
    {
      TaskCancellationFlag = true;
      OggPlayerInstance.Pause();
    }

    /// <summary>
    /// Остановить воспроизведение
    /// </summary>
    public void Stop()
    {
      Console.WriteLine("Stop OGG called");
      TaskCancellationFlag = true;
      OggPlayerInstance.Stop();
      _loopTaskEnded = true;
    }

    /// <summary>
    /// Сбросить воспроизведение
    /// </summary>
    public void Reset()
    {
      Console.WriteLine("Reset OGG called");
      TaskCancellationFlag = true;
      OggPlayerInstance.Stop();
      IsLooped = false;
      _loopTaskEnded = true;
    }

    /// <summary>
    /// Воспроизводится ли сейчас?
    /// </summary>
    /// <returns>True, если воспроизводится</returns>
    public bool IsPlaying()
    {
      return OggPlayerInstance.PlayerState == OggPlayerStatus.Playing ||
             OggPlayerInstance.PlayerState == OggPlayerStatus.Buffering;
    }

    /// <summary>
    /// На паузе ли сейчас?
    /// </summary>
    /// <returns>True, если на паузе</returns>
    public bool IsPaused()
    {
      return OggPlayerInstance.PlayerState == OggPlayerStatus.Paused;
    }

    /// <summary>
    /// Установка параметра зацикленности воспроизведения
    /// </summary>
    /// <param name="parIsLooped">Значение параметра зацикленности воспроизведения</param>
    public void SetIsLooped(bool parIsLooped)
    {
      IsLooped = parIsLooped;
    }

    /// <summary>
    /// Деструктор
    /// </summary>
    public void Dispose()
    {
      //tru
      Console.WriteLine("OGG disposing!");
      TaskCancellationFlag = true;
      Stop();

      while (!_loopTaskEnded)
      {
      }

      while (OggPlayerInstance.PlayerState == OggPlayerStatus.Playing)
      {
      }

      Console.WriteLine("OGG disposed!");
      OggPlayerInstance.Dispose();
    }

    /// <summary>
    /// Обновить состояние задачи по зацикливанию воспроизведения трека
    /// </summary>
    private void UpdateLoopedTask()
    {
      TaskCancellationFlag = false;
      if (IsLooped)
      {
        LoopTask = Task.Run(() =>
          {
            while (OggPlayerInstance.PlayerState == OggPlayerStatus.Playing ||
                   OggPlayerInstance.PlayerState == OggPlayerStatus.Buffering)
            {
              if (TaskCancellationFlag)
              {
                Console.WriteLine("OGG task cancelled!");
                _loopTaskEnded = true;
                return;
              }

              Thread.Sleep(10);
            }

            if (TaskCancellationFlag)
            {
              Console.WriteLine("OGG task cancelled!");
              _loopTaskEnded = true;
              return;
            }

            Play(IsLooped);
          }
        );
      }
      else
      {
        _loopTaskEnded = true;
      }
    }

    /// <summary>
    /// Ссылка на текущий плеер OGG Vorbis
    /// </summary>
    private OggPlayerFBN OggPlayerInstance { get; set; }

    /// <summary>
    /// Флаг-признак отмены выполнения задачи по зацикливанию воспроизведения
    /// </summary>
    private bool TaskCancellationFlag { get; set; } = false;

    /// <summary>
    /// Текущий экземпляр задачи по зацикливанию воспроизведения
    /// </summary>
    private Task LoopTask { get; set; }

    /// <summary>
    /// Зациклено ли воспроизведение?
    /// </summary>
    private bool IsLooped { get; set; }
  }
}