using System.Text.Json;

namespace ToDoApp.Controllers
{
    public static class LanguageController
    {
        private static Dictionary<string, string> _translations = new();
        public static string CurrentLanguage { get; private set; } = "en";

        private static readonly string ConfigFilePath = Path.Combine("Config", "settings.json");

        public static void LoadLanguage(string langCode)
        {
            string path = Path.Combine("Views", "lang", $"{langCode}.json");
            if (!File.Exists(path))
            {
                Console.WriteLine($"Language file '{langCode}' not found, defaulting to 'en'");
                langCode = "en";
                path = Path.Combine("Views", "lang", $"{langCode}.json");
            }

            string json = File.ReadAllText(path);
            _translations = JsonSerializer.Deserialize<Dictionary<string, string>>(json)
                           ?? new Dictionary<string, string>();

            CurrentLanguage = langCode;
            SaveLanguageSetting(langCode);
        }

        public static string T(string key)
        {
            if (_translations.ContainsKey(key))
                return _translations[key];
            return key;
        }

        public static List<string> GetAvailableLanguages()
        {
            string langDir = Path.Combine("Views", "lang");
            return Directory.GetFiles(langDir, "*.json")
                            .Select(f => Path.GetFileNameWithoutExtension(f))
                            .ToList();
        }

        public static void LoadSavedLanguage()
        {
            try
            {
                if (File.Exists(ConfigFilePath))
                {
                    var settings = JsonSerializer.Deserialize<Dictionary<string, string>>(File.ReadAllText(ConfigFilePath));
                    if (settings != null && settings.ContainsKey("language"))
                    {
                        LoadLanguage(settings["language"]);
                        return;
                    }
                }
            }
            catch{}

            LoadLanguage("en");
        }

        private static void SaveLanguageSetting(string langCode)
        {
            Directory.CreateDirectory("Config");
            var settings = new Dictionary<string, string>
            {
                { "language", langCode }
            };
            File.WriteAllText(ConfigFilePath, JsonSerializer.Serialize(settings, new JsonSerializerOptions { WriteIndented = true }));
        }
    }
}
