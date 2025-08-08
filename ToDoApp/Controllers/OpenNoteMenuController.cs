using System.Text;
using ToDoApp.Views;

namespace ToDoApp.Controllers;

public static class OpenNoteMenuController
{
    static string notesDir = DirectoryController.NotesDirectory;

    public static void Show(string filepath)
    {
        while (true)
        {
            Console.Clear();
            var lines = new List<string>(File.ReadAllLines(filepath));
            string noteTitle = lines.Count > 0 ? lines[0].TrimStart('-', '~') : LanguageController.T("note_menu_untitle");
            Console.WriteLine($"{LanguageController.T("note_title")}: '{noteTitle}' ===");
            Console.WriteLine();
            for (int i = 1; i < lines.Count; i++)
            {
                string marker = lines[i].StartsWith("~") ? "[✓]" : "[ ]";
                Console.WriteLine($"{i}. {marker} {lines[i].TrimStart('-', '~')}");
            }

            Console.WriteLine(LanguageController.T("note_title_change"));
            Console.WriteLine(LanguageController.T("note_add"));
            Console.WriteLine(LanguageController.T("note_cross"));
            Console.WriteLine(LanguageController.T("note_delete"));
            Console.WriteLine(LanguageController.T("note_back"));
            Console.Write(LanguageController.T("prompt_choice"));
            string input = Console.ReadLine();

            if (input == "0") return;
            else if (input == "%0")
            {
                Console.WriteLine(LanguageController.T("prompt_current_note_title") + noteTitle);
                Console.Write(LanguageController.T("prompt_new_note_title"));
                string newTitle = ReadLineWithEsc(out bool wasEsc);
                if (!wasEsc && !string.IsNullOrWhiteSpace(newTitle))
                {
                    char prefix = lines[0].Length > 0 ? lines[0][0] : '-';
                    lines[0] = prefix + newTitle;
                    File.WriteAllLines(filepath, lines);
                }
            }
            else if (input == "+")
            {
                Console.Write(LanguageController.T("prompt_enter_text"));
                string text = Console.ReadLine();
                lines.Add("-" + text);
                File.WriteAllLines(filepath, lines);
            }
            else if (input.StartsWith("~") && int.TryParse(input.Substring(1), out int cross) && cross >= 1 && cross < lines.Count)
            {
                if (lines[cross].StartsWith("~"))
                    lines[cross] = "-" + lines[cross].Substring(1);
                else
                    lines[cross] = "~" + lines[cross].Substring(1);
                File.WriteAllLines(filepath, lines);
            }
            else if (input.StartsWith("-") && int.TryParse(input.Substring(1), out int del) && del >= 1 && del < lines.Count)
            {
                lines.RemoveAt(del);
                File.WriteAllLines(filepath, lines);
            }
            else if (int.TryParse(input, out int edit) && edit >= 1 && edit < lines.Count)
            {
                Console.WriteLine(LanguageController.T("prompt_current_text") + lines[edit].Substring(1));
                Console.Write(LanguageController.T("prompt_new_text"));
                string newText = ReadLineWithEsc(out bool wasEsc);
                if (!wasEsc)
                {
                    char prefix = lines[edit][0];
                    lines[edit] = prefix + newText;
                    File.WriteAllLines(filepath, lines);
                }
            }
        }
    }

    static string ReadLineWithEsc(out bool wasEsc)
    {
        wasEsc = false;
        StringBuilder sb = new StringBuilder();
        ConsoleKeyInfo key;
        while ((key = Console.ReadKey(true)).Key != ConsoleKey.Enter)
        {
            if (key.Key == ConsoleKey.Escape)
            {
                wasEsc = true;
                Console.WriteLine();
                return "";
            }
            else if (key.Key == ConsoleKey.Backspace && sb.Length > 0)
            {
                sb.Remove(sb.Length - 1, 1);
                Console.Write(" ");
            }
            else if (!char.IsControl(key.KeyChar))
            {
                sb.Append(key.KeyChar);
                Console.Write(key.KeyChar);
            }
        }
        Console.WriteLine();
        return sb.ToString();
    }
}
