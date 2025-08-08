namespace ToDoApp.Controllers
{
    public static class LanguageMenuController
    {
        public static void Show()
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine(LanguageController.T("lang_menu_title"));

                var langs = LanguageController.GetAvailableLanguages();
                for (int i = 0; i < langs.Count; i++)
                {
                    Console.WriteLine($"{i + 1}. {langs[i]}{(langs[i] == LanguageController.CurrentLanguage ? " ✅" : "")}");
                }
                Console.WriteLine(LanguageController.T("lang_menu_back"));
                Console.Write(LanguageController.T("prompt_choice"));

                string input = Console.ReadLine();
                if (input == "0") return;

                if (int.TryParse(input, out int choice) && choice >= 1 && choice <= langs.Count)
                {
                    LanguageController.LoadLanguage(langs[choice - 1]);
                }
            }
        }
    }
}
