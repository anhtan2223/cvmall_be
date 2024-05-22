using System.Globalization;
using System.Text;

namespace Framework.Core.Extensions
{
    public static class StringExtensions
    {
        public static string RemoveAccents(this string text)
        {
            if (string.IsNullOrWhiteSpace(text))
                return text;

            text = text.Normalize(NormalizationForm.FormD);
            char[] chars = text.Where(predicate: c => CharUnicodeInfo.GetUnicodeCategory(c) != UnicodeCategory.NonSpacingMark).ToArray();

            return new string(chars).Normalize(NormalizationForm.FormC);
        }

        public static bool Like(this string source, string target)
        {
            return RemoveAccents(source).Contains(RemoveAccents(target));
        }
        public static string ReplaceText(this string text)
        {
            return text.Replace('.', ',');
        }
    }
}
