using System;
using System.Collections.Generic;
using System.Linq;
using Model.SPCore.Consts;
using Model.SPCore.MGameActions;

namespace Model.SPCore.MPlayers
{
    /// <summary>
    /// Менеджер ввода игроков
    /// </summary>
    public class MPlayersManager
    {
        /// <summary>
        /// Специальное обозначение клавиши Backspace для обработки событий ввода текста на клавиатуре
        /// </summary>
        public const string SPECIAL_BACKSPACE_SIGNATURE = "BackSpace";

        /// <summary>
        /// Текущий список игроков в игре
        /// </summary>
        private List<MPlayer> _players = new List<MPlayer>();

        /// <summary>
        /// Событие добавления нового игрока
        /// </summary>
        public event Action<MPlayer> OnPlayerAdded;

        /// <summary>
        /// Событие удаления игрока из игры
        /// </summary>
        public event Action<MPlayer> OnPlayerRemoved;

        /// <summary>
        /// Обновление данных о последней нажатой текстовой клавиатурной клавише
        /// </summary>
        /// <param name="parKey">Строка последней нажатой текстовой клавиатурной клавише (обычно буква или BackSpace)</param>
        public void UpdateLastPressedKeyboardKey(string parKey)
        {
            LastPressedKeyboardKey = parKey;
        }

        /// <summary>
        /// Очистка данных о последней нажатой текстовой клавиатурной клавише
        /// </summary>
        public void ClearLastPressedKeyboardKey()
        {
            LastPressedKeyboardKey = "";
        }

        /// <summary>
        /// Добавить игрока
        /// </summary>
        /// <param name="parNewPlayer">Данные о новом игроке</param>
        public void AddPlayer(MPlayer parNewPlayer)
        {
            _players.Add(parNewPlayer);
            OnPlayerAdded?.Invoke(parNewPlayer);
        }

        /// <summary>
        /// Удалить игрока из игры
        /// </summary>
        /// <param name="parPlayerId">Данные об удаляемом игроке</param>
        public void RemovePlayer(string parPlayerId)
        {
            MPlayer foundPlayer = _players.FirstOrDefault(parX => parX.PlayerId == parPlayerId);
            if (foundPlayer == null)
            {
                return;
            }

            _players.Remove(foundPlayer);
            OnPlayerRemoved?.Invoke(foundPlayer);
        }

        /// <summary>
        /// Является ли игрок активным в текущий момент
        /// </summary>
        /// <param name="parPlayerId">Идентификатор игрока</param>
        /// <returns>True, если игрок активен</returns>
        public bool IsPlayerActive(string parPlayerId)
        {
            MPlayer foundPlayer = _players.FirstOrDefault(parX => parX.PlayerId == parPlayerId);
            if (foundPlayer != null)
            {
                return foundPlayer.IsActive;
            }

            return false;
        }

        /// <summary>
        /// Получить состояние игровой оси для определенного игрока
        /// </summary>
        /// <param name="parPlayerId">Идентификатор игрока</param>
        /// <param name="parActionId">Тип игровой оси</param>
        /// <returns></returns>
        public MGameActionAxis GetGameActionAxis(string parPlayerId, EGameActionAxis parActionId)
        {
            MPlayer foundPlayer = _players.FirstOrDefault(parX => parX.PlayerId == parPlayerId);
            if (foundPlayer != null)
            {
                if (foundPlayer.IsActive)
                {
                    return foundPlayer.GetGameActionAxis(parActionId);
                }
            }

            return new MGameActionAxis(AppInfoConsts.AxisDefaultValue);
        }

        /// <summary>
        /// Получить состояние игровой кнопки для определенного игрока
        /// </summary>
        /// <param name="parPlayerId">Идентификатор игрока</param>
        /// <param name="parActionId">Тип игровой кнопки</param>
        /// <returns></returns>
        public MGameActionButton GetGameActionButton(string parPlayerId, EGameActionButton parActionId)
        {
            MPlayer foundPlayer = _players.FirstOrDefault(parX => parX.PlayerId == parPlayerId);
            if (foundPlayer != null)
            {
                if (foundPlayer.IsActive)
                {
                    return foundPlayer.GetGameActionButton(parActionId);
                }
            }

            return new MGameActionButton();
        }

        /// <summary>
        /// Нажата ли сейчас игровая кнопка игроком? Нажатие кнопки является событие перехода кнопки
        /// из состояния "не нажата" в состояние "зажата"
        /// </summary>
        /// <param name="parPlayerRef">Ссылка на объект информации об игроке</param>
        /// <param name="parButtonId">Тип игровой кнопки</param>
        /// <returns></returns>
        public bool IsButtonPressed(MPlayer parPlayerRef, EGameActionButton parButtonId)
        {
            return (parPlayerRef.GetGameActionButton(parButtonId).IsPressedFrame);
        }


        /// <summary>
        /// Отпущена ли сейчас игровая кнопка игроком? Отпускание кнопки является событие перехода кнопки
        /// из состояния "зажата" в состояние "не нажата"
        /// </summary>
        /// <param name="parPlayerRef">Ссылка на объект информации об игроке</param>
        /// <param name="parButtonId">Тип игровой кнопки</param>
        /// <returns></returns>
        public bool IsButtonReleased(MPlayer parPlayerRef, EGameActionButton parButtonId)
        {
            return (parPlayerRef.GetGameActionButton(parButtonId).IsReleasedFrame);
        }

        /// <summary>
        /// Удерживает ли игрок определенную игровую кнопку?
        /// </summary>
        /// <param name="parPlayerRef">Ссылка на объект информации об игроке</param>
        /// <param name="parButtonId">Тип игровой кнопки</param>
        /// <returns>True, если удерживает</returns>
        public bool IsButtonHolding(MPlayer parPlayerRef, EGameActionButton parButtonId)
        {
            return (parPlayerRef.GetGameActionButton(parButtonId).IsHolding);
        }

        /// <summary>
        /// Нажата ли хотя бы одна из кнопок, отвечающая за подтверждение или осуществление игрового действия игроком
        /// </summary>
        /// <param name="parPlayerRef">Ссылка на объект информации об игроке</param>
        /// <returns></returns>
        public bool IsActionButtonPressed(MPlayer parPlayerRef)
        {
            return IsButtonPressed(parPlayerRef, EGameActionButton.Button_A) ||
                   IsButtonPressed(parPlayerRef, EGameActionButton.Button_B) ||
                   IsButtonPressed(parPlayerRef, EGameActionButton.Button_X) ||
                   IsButtonPressed(parPlayerRef, EGameActionButton.Button_Y);
        }

        /// <summary>
        /// Текущий первый игрок в системе
        /// </summary>
        public MPlayer Player1 { get; set; }

        /// <summary>
        /// Текущий второй игрок в системе
        /// </summary>
        public MPlayer Player2 { get; set; }

        /// <summary>
        /// Данные о последней нажатой текстовой клавиатурной клавише
        /// </summary>
        public string LastPressedKeyboardKey { get; private set; }
    }
}