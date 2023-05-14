using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.Helper
{
    public static class TextHelper
    {
        public static string Codify(string text)
        {
            if (string.IsNullOrWhiteSpace(text))
                return "";

            StringBuilder newText = new StringBuilder(text.Length * 2);
            newText.Append(text[0]);

            for (int i = 1; i < text.Length; i++)
            {
                if (text[i] == ' ')
                    break;

                if (char.IsUpper(text[i]) && text[i - 1] == ' ')
                    newText.Append('_');

                newText.Append(text[i]);
            }

            return newText.ToString();
        }

        public static string Labelize(string text)
        {
            if (string.IsNullOrWhiteSpace(text))
                return "";

            StringBuilder newText = new StringBuilder(text.Length * 2);
            newText.Append(text[0].ToString().ToUpper());

            for (int i = 1; i < text.Length; i++)
            {
                if (char.IsUpper(text[i]))
                    newText.Append(' ');
                newText.Append(text[i]);
            }

            return newText.ToString();
        }

    }
}
