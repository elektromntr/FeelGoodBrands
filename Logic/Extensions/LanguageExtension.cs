using System;
using System.Collections.Generic;
using System.Text;
using Data.Enums;

namespace Logic.Extensions
{
    public static class LanguageExtension
    {
        private static readonly Language defaultLanguage = Language.English;
        
        public static Data.Enums.Language GetLanguage(string language)
        {
            var resultLanguage = defaultLanguage; //resultLanguage is set to default on beginning
            if (Enum.TryParse(language, true, out Language parsed))
                resultLanguage = parsed;
            return resultLanguage;
        }

        public static string SessionLanguageKey() => "Language";
    }
}
