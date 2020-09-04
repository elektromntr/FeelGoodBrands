using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Data.Enums;
using Newtonsoft.Json;
// ReSharper disable ClassNeverInstantiated.Local
#pragma warning disable 649

namespace Logic.Extensions
{
    public static class LanguageExtension
    {
        private static readonly Language defaultLanguage = Language.English;
        private const string SessionLanguageKeyName = "Language";
        
        public static Data.Enums.Language GetLanguage(string language)
        {
            var resultLanguage = defaultLanguage; //resultLanguage is set to default on beginning
            if (Enum.TryParse(language, true, out Language parsed))
                resultLanguage = parsed;
            return resultLanguage;
        }

        public static string SessionLanguageKey() => SessionLanguageKeyName;

        public static string GetTranslatedString(Language language, string key)
        {
            using StreamReader r = new StreamReader(@"translator\dictionary.json");
            List<DictionaryItem> items = JsonConvert.DeserializeObject<List<DictionaryItem>>(r.ReadToEnd());
            var itemToTranslate = items.First(k => k.Name == key).Translations;
            string translation = "";
            switch (language)
            {
                case Language.Polish:
                    translation = itemToTranslate.Polish;
                    break;
                case Language.English:
                    translation = itemToTranslate.English;
                    break;
            }
            return translation;
        }

        private class DictionaryItem
        {
            public string Name;
            public Translation Translations;
        }

        private class Translation
        {
            public string English;
            public string Polish;
        }
    }
}