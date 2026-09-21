using System.Text;

namespace PlainApp.Services.JapaneseServices
{
    public static class JapaneseHelper
    {
        public static string CleanJapaneseText(string text)
        {
            if (string.IsNullOrWhiteSpace(text))
            {
                return string.Empty;
            }
            
            string cleanedText = text.Trim();

            cleanedText = cleanedText.Normalize(System.Text.NormalizationForm.FormC);

            return cleanedText;
        }

        public static string ConvertToHiragana(string text)
        {
            if (string.IsNullOrWhiteSpace(text))
            {
                return string.Empty;
            }

            var hiraganaText = new System.Text.StringBuilder();
            foreach (char c in text)
            {
                if (c >= 0x30A1 && c <= 0x30F4)
                {
                    hiraganaText.Append((char)(c - 0x60));
                }
                else
                {
                    hiraganaText.Append(c);
                }
            }
            return hiraganaText.ToString();
        }

        public static string ConvertToRomaji(string text)
        {
            if (string.IsNullOrWhiteSpace(text))
            {
                return string.Empty;
            }

            string cleanedText = CleanJapaneseText(text);

            var result = new StringBuilder();
            int i = 0;

            while (i < cleanedText.Length)
            {
                if (cleanedText[i] == 'っ' || cleanedText[i] == 'ッ')
                {
                    if (i + 1 < cleanedText.Length)
                    {
                        string nextRomaji = PeekRomaji(cleanedText, i + 1);
                        if (!string.IsNullOrEmpty(nextRomaji))
                        {
                            result.Append(nextRomaji[0]);
                        }
                    }
                    i++;
                    continue;
                }

                if (i + 1 < cleanedText.Length)
                {
                    string pair = cleanedText.Substring(i, 2);
                    if (KanaTable.HiraganaTable.TryGetValue(pair, out var romajiPair))
                    {
                        result.Append(romajiPair);
                        i += 2;
                        continue;
                    }
                }

                string single = cleanedText[i].ToString();
                if (KanaTable.HiraganaTable.TryGetValue(single, out var romajiSingle))
                {
                    result.Append(romajiSingle);
                }
                else
                {
                    result.Append(single);
                }

                i++;
            }

            return result.ToString();
        }

        private static string PeekRomaji(string text, int index)
        {
            if (index + 1 < text.Length)
            {
                string pair = text.Substring(index, 2);
                if (KanaTable.HiraganaTable.TryGetValue(pair, out var rPair)) return rPair;
            }

            string single = text[index].ToString();
            if (KanaTable.HiraganaTable.TryGetValue(single, out var rSingle)) return rSingle;

            return string.Empty;
        }
    }
}
