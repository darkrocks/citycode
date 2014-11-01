using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Guide.Services.Contracts;

namespace Guide.Services
{
	public class TransliterationService : ITransliterationService
	{
			private Dictionary<char, string> Words
			{
				get
				{
					return new Dictionary<char, string>
                    {
                        {'а', "a"},
                        {'б', "b"},
                        {'в', "v"},
                        {'г', "g"},
                        {'д', "d"},
                        {'е', "e"},
                        {'ё', "yo"},
                        {'ж', "zh"},
                        {'з', "z"},
                        {'и', "i"},
                        {'й', "y"},
                        {'к', "k"},
                        {'л', "l"},
                        {'м', "m"},
                        {'н', "n"},
                        {'о', "o"},
                        {'п', "p"},
                        {'р', "r"},
                        {'с', "s"},
                        {'т', "t"},
                        {'у', "u"},
                        {'ф', "f"},
                        {'х', "h"},
                        {'ц', "c"},
                        {'ч', "ch"},
                        {'ш', "sh"},
                        {'щ', "sch"},
                        {'ъ', "j"},
                        {'ы', "y"},
                        {'ь', "j"},
                        {'э', "e"},
                        {'ю', "yu"},
                        {'я', "ya"},

                        {' ', "-"},
                        {'-', "-"},
                        {'0', "0"},
                        {'1', "1"},
                        {'2', "2"},
                        {'3', "3"},
                        {'4', "4"},
                        {'5', "5"},
                        {'6', "6"},
                        {'7', "7"},
                        {'8', "8"},
                        {'9', "9"},

                        {'a', "a"},
                        {'b', "b"},
                        {'c', "c"},
                        {'d', "d"},
                        {'e', "e"},
                        {'f', "f"},
                        {'g', "g"},
                        {'h', "h"},
                        {'i', "i"},
                        {'j', "j"},
                        {'k', "k"},
                        {'l', "l"},
                        {'m', "m"},
                        {'n', "n"},
                        {'o', "o"},
                        {'p', "p"},
                        {'r', "r"},
                        {'s', "s"},
                        {'t', "t"},
                        {'u', "u"},
                        {'v', "v"},
                        {'w', "w"},
                        {'x', "x"},
                        {'y', "y"},
                        {'z', "z"}
                    };
				}
			}

			public string Transliterate(string text)
			{
				text = text.ToLower();
				return text.Where(c => Words.ContainsKey(c)).Aggregate("", (current, c) => current + Words[c]);
			}
		}
}
