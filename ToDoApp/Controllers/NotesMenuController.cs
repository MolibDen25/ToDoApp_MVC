using ToDoApp.Views;

namespace ToDoApp.Controllers;

public static class NotesMenuController
{
    static string notesDir = DirectoryController.NotesDirectory;

    public static void Show()
    {
        while (true)
        {
            Console.Clear();
            var files = GetSortedNoteFiles();

            Console.WriteLine(LanguageController.T("note_menu_title"));
            for (int i = 0; i < files.Count; i++)
            {
                var lines = File.ReadAllLines(files[i]);
                string title = lines.Length > 0 ? lines[0] : LanguageController.T("note_menu_untitle");
                bool isCrossed = title.StartsWith("~");
                title = title.TrimStart('-', '~');
                Console.WriteLine($"{i + 1}. {(isCrossed ? "[✓] " : "")}{title}");
            }

            Console.WriteLine(LanguageController.T("note_menu_add"));
            Console.WriteLine(LanguageController.T("note_menu_cross"));
            Console.WriteLine(LanguageController.T("note_menu_delete"));
            Console.WriteLine(LanguageController.T("note_menu_back"));
            Console.Write(LanguageController.T("prompt_choice"));
            string input = Console.ReadLine();

            if (input == "0") return;
            else if (input == "+") AddNote();
            else if (input.StartsWith("~")) ToggleCrossNote(input.Substring(1), files);
            else if (input.StartsWith("-")) DeleteNote(input.Substring(1), files);
            else if (int.TryParse(input, out int index) && index >= 1 && index <= files.Count)
                OpenNoteMenuController.Show(files[index - 1]);
        }
    }

    static void AddNote()
    {
        Console.Clear();
        Console.Write(LanguageController.T("prompt_enter_note_title"));
        string title = Console.ReadLine() ?? LanguageController.T("note_menu_untitle");
        var files = GetSortedNoteFiles();
        string path = Path.Combine(notesDir, $"{files.Count + 1}.txt");
        File.WriteAllText(path, "-" + title + Environment.NewLine);
    }

    static void ToggleCrossNote(string number, List<string> files)
    {
        if (int.TryParse(number, out int index) && index >= 1 && index <= files.Count)
        {
            var lines = File.ReadAllLines(files[index - 1]);
            if (lines.Length > 0)
            {
                if (lines[0].StartsWith("~"))
                    lines[0] = "-" + lines[0][1..];
                else
                    lines[0] = "~" + lines[0][1..];
                File.WriteAllLines(files[index - 1], lines);
            }
        }
    }

    static void DeleteNote(string number, List<string> files)
    {
        if (int.TryParse(number, out int index) && index >= 1 && index <= files.Count)
        {
            File.Delete(files[index - 1]);
            for (int i = index; i < files.Count; i++)
            {
                File.Move(files[i], Path.Combine(DirectoryController.NotesDirectory, $"{i}.txt"));
            }
        }
    }

    static List<string> GetSortedNoteFiles()
    {
        var files = new List<string>(Directory.GetFiles(notesDir, "*.txt"));
        files.Sort((a, b) =>
        {
            int na = int.Parse(Path.GetFileNameWithoutExtension(a));
            int nb = int.Parse(Path.GetFileNameWithoutExtension(b));
            return na.CompareTo(nb);
        });
        return files;
    }
}
