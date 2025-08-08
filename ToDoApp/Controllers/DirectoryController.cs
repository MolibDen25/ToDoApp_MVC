using System.IO;

namespace ToDoApp.Controllers
{
    public static class DirectoryController
    {
        public static string NotesDirectory => "notes";

        public static void EnsureNotesDirectory()
        {
            if (!Directory.Exists(NotesDirectory))
                Directory.CreateDirectory(NotesDirectory);
        }
    }
}