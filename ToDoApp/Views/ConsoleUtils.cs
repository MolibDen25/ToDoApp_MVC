using System;
using System.Text;

namespace ToDoApp.Views
{
    public static class ConsoleUtils
    {
        public static string ReadLineWithEsc(out bool wasEsc)
        {
            wasEsc = false;
            var sb = new StringBuilder();
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
}