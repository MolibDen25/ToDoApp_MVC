using System;
using ToDoApp.Controllers;

namespace ToDoApp
{
    class Program
    {
        static void Main()
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            Console.InputEncoding = System.Text.Encoding.UTF8;
            LanguageController.LoadSavedLanguage();
            DirectoryController.EnsureNotesDirectory();
            MainMenuController.Show();
        }
    }
}