using System;
using ToDoApp.Views;

namespace ToDoApp.Controllers
{
    public static class MainMenuController
    {
        public static void Show()
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine(LanguageController.T("main_menu_title"));
                Console.WriteLine(LanguageController.T("main_menu_notes"));
                Console.WriteLine(LanguageController.T("main_menu_language"));
                Console.WriteLine(LanguageController.T("main_menu_exit"));
                Console.Write(LanguageController.T("prompt_choice"));
                string input = Console.ReadLine();

                if (input == "1")
                    NotesMenuController.Show();
                else if (input == "2")
                    LanguageMenuController.Show();
                else if (input == "0")
                    Environment.Exit(0);
            }
        }
    }
}