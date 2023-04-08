using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Serialization;
using ColossalFramework.Globalization;

namespace ThemeMixer.TranslationFramework
{
    public delegate void LanguageChangedEventHandler(string languageIdentifier);

    /// <summary>
    /// Handles localization for a mod.
    /// </summary>
    public class Translation
    {

        protected List<Language> Languages = new List<Language>();
        protected Language CurrentLanguage;
        protected bool LanguagesLoaded;
        protected bool LoadLanguageAutomatically;
        private const string FallbackLanguage = "en";

        private static Translation _instance;

        public static Translation Instance => _instance ?? (_instance = new Translation());

        private Translation(bool loadLanguageAutomatically = true)
        {
            LoadLanguageAutomatically = loadLanguageAutomatically;
            LocaleManager.eventLocaleChanged += SetCurrentLanguage;
        }

        private void SetCurrentLanguage()
        {
            if (Languages == null || Languages.Count == 0 || !LocaleManager.exists)
                return;
            CurrentLanguage = Languages.Find(lang => lang._uniqueName == LocaleManager.instance.language) ??
                               Languages.Find(lang => lang._uniqueName == FallbackLanguage);
        }


        /// <summary>
        /// Loads all languages up if not already loaded.
        /// </summary>
        public void LoadLanguages()
        {
            if (LanguagesLoaded || !LoadLanguageAutomatically) return;
            RefreshLanguages();
            SetCurrentLanguage();
            LanguagesLoaded = true;
        }

        /// <summary>
        /// Forces a reload of the languages, even if they're already loaded
        /// </summary>
        public void RefreshLanguages()
        {
            Languages.Clear();
            string basePath = TranslationUtil.AssemblyPath;

            if (basePath != "")
            {
                string languagePath = Path.Combine(basePath, "Locale");

                if (Directory.Exists(languagePath))
                {
                    string[] languageFiles = Directory.GetFiles(languagePath);

                    foreach (string languageFile in languageFiles)
                    {
                        using (StreamReader reader = new StreamReader(languageFile))
                        {
                            XmlSerializer xmlSerializer = new XmlSerializer(typeof(Language));
                            if (xmlSerializer.Deserialize(reader) is Language loadedLanguage)
                                Languages.Add(loadedLanguage);

                            else UnityEngine.Debug.LogWarning($"Failed to Deserialize {languageFile}!");
                        }
                    }
                }
                else
                {
                    UnityEngine.Debug.LogWarning("Locale Directory not found!");
                }
            }
            else UnityEngine.Debug.LogWarning("Mod Path was empty!");
        }

        /// <summary>
        /// Returns a list of languages which are available to the mod. This will return readable languages for use on the UI
        /// </summary>
        /// <returns>A list of languages available.</returns>
        public List<string> AvailableLanguagesReadable()
        {
            LoadLanguages();

            return Languages.Select(availableLanguage => availableLanguage._readableName).ToList();
        }

        /// <summary>
        /// Returns a list of languages which are available to the mod. This will return language IDs for searching.
        /// </summary>
        /// <returns>A list of languages available.</returns>
        public List<string> AvailableLanguages()
        {
            LoadLanguages();

            return Languages.Select(availableLanguage => availableLanguage._uniqueName).ToList();
        }

        /// <summary>
        /// Returns a list of Language unique IDs that have the name
        /// </summary>
        /// <param name="name">The name of the language to get IDs for</param>
        /// <returns>A list of IDs that match</returns>
        public List<string> GetLanguageIDsFromName(string name)
        {
            return (from availableLanguage in Languages where availableLanguage._readableName == name select availableLanguage._uniqueName).ToList();
        }

        /// <summary>
        /// Returns whether you can translate into a specific translation ID
        /// </summary>
        /// <param name="translationId">The ID of the translation to check</param>
        /// <returns>Whether a translation into this ID is possible</returns>
        public bool HasTranslation(string translationId)
        {
            LoadLanguages();
            return CurrentLanguage != null && CurrentLanguage._conversionDictionary.ContainsKey(translationId);
        }

        /// <summary>
        /// Gets a translation for a specific translation ID
        /// </summary>
        /// <param name="translationId">The ID to return the translation for</param>
        /// <returns>A translation of the translationId</returns>
        public string GetTranslation(string translationId)
        {
            LoadLanguages();
            string translatedText = translationId;

            if (CurrentLanguage != null)
            {
                if (HasTranslation(translationId))
                    translatedText = CurrentLanguage._conversionDictionary[translationId];
                else UnityEngine.Debug.LogWarning("Returned translation for language \"" + CurrentLanguage._uniqueName + "\" doesn't contain a suitable translation for \"" + translationId + "\"");
            }
            else UnityEngine.Debug.LogWarning("Can't get a translation for \"" + translationId + "\" as there is not a language defined");

            return translatedText;
        }
    }
}
