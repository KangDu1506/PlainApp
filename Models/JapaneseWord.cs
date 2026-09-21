using PlainApp.Services.JapaneseServices;

namespace PlainApp.Models
{
    public class JapaneseWord
    {
        public string Kanji { get; private set; } = string.Empty;
        public string Kana { get; private set; } = string.Empty;
        public string Romaji => JapaneseHelper.ConvertToRomaji(Kana);
        public JapaneseWord(string kanji, string kana)
        {
            Kanji = kanji;
            Kana = kana;
        }
    }
}
