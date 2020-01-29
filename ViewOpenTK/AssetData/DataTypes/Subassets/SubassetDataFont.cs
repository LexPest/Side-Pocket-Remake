using System.Collections.Generic;
using System.Text;
using ViewOpenTK.SPCore.ViewProvider;

namespace ViewOpenTK.AssetData.DataTypes.Subassets
{
  /// <summary>
  /// Производный ассет - шрифт
  /// </summary>
  public class SubassetDataFont : SubassetData
  {
    /// <summary>
    /// Стандартный конструктор
    /// </summary>
    /// <param name="parSymbolsToSprites">Словарь соответствия символам спрайтов</param>
    public SubassetDataFont(Dictionary<string, SubassetDataSprite> parSymbolsToSprites)
    {
      SymbolsToSprites = parSymbolsToSprites;
    }

    /// <summary>
    /// Получить спрайты для текстовой строки
    /// </summary>
    /// <param name="parText">Целевая текстовая строка</param>
    /// <returns></returns>
    public LinkedList<SubassetDataSprite> GetSymbolsSprites(string parText)
    {
      LinkedList<SubassetDataSprite> resultSprites = new LinkedList<SubassetDataSprite>();

      for (int i = 0; i < parText.Length; i++)
      {
        void ProcessEscapeSequence(string parEscSequenceStart, string parEscSequenceEnd)
        {
          i += parEscSequenceStart.Length;
          StringBuilder sb = new StringBuilder();

          for (; i < parText.Length; i++)
          {
            if (CheckEscapeSequence(parEscSequenceEnd))
            {
              break;
            }
            else
            {
              sb.Append(parText[i]);
            }
          }

          i += parEscSequenceEnd.Length - 1;
          TryAddByKey(sb.ToString());
        }

        bool CheckEscapeSequence(string parEscSequence)
        {
          if (i + parEscSequence.Length <= parText.Length)
          {
            for (int j = i; j < i + parEscSequence.Length; j++)
            {
              if (parText[j] != parEscSequence[j - i])
              {
                return false;
              }
            }

            return true;
          }

          return false;
        }

        void TryAddByKey(string parActualKey)
        {
          if (SymbolsToSprites.ContainsKey(parActualKey))
          {
            resultSprites.AddLast(SymbolsToSprites[parActualKey]);
          }
          else
          {
            resultSprites.AddLast(SymbolsToSprites[ViewBehaviourConsts.DEFAULT_SYMBOL]);
          }
        }


        char currentChar = parText[i];
        if (CheckEscapeSequence(ViewBehaviourConsts.ESCAPE_CHARACTER_SEQUENCE_START))
        {
          ProcessEscapeSequence(ViewBehaviourConsts.ESCAPE_CHARACTER_SEQUENCE_START,
            ViewBehaviourConsts.ESCAPE_CHARACTER_SEQUENCE_END);
        }
        else
        {
          TryAddByKey(currentChar.ToString());
        }
      }

      return resultSprites;
    }

    /// <summary>
    /// Словарь соответствия символам спрайтов
    /// </summary>
    private Dictionary<string, SubassetDataSprite> SymbolsToSprites { get; set; }
  }
}