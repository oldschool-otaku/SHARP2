namespace SHARP2;

public class Program
{

    private static void Main(string[] args)
    {
        Operation.CreateOperations();
        
        void RewriteLine(string caret, List<char> buffer)
        {
            Console.SetCursorPosition(0, Console.CursorTop);
            Console.Write(new string(' ', Console.WindowWidth - 1));
            Console.SetCursorPosition(0, Console.CursorTop);
            Console.Write(caret);
            Console.Write(buffer.ToArray());
        }

        int selectLine(IReadOnlyList<string> text)
        {
            Console.WriteLine("Select column: ");
            int columnSelected = 0;
            Console.Clear();
            for (int i = 0; i < text.Count; i++)
            {
                Console.WriteLine($"{(i == columnSelected ? "> " : "  ")}{text[i]}");
            }
            ConsoleKeyInfo keyInfo = Console.ReadKey(true);
            while (keyInfo.Key != ConsoleKey.Enter)
            {
                switch (keyInfo.Key)
                {
                    case ConsoleKey.DownArrow:
                        columnSelected = columnSelected + 1 >= text.Count ? columnSelected : columnSelected + 1;
                        break;
                                
                    case ConsoleKey.UpArrow:
                        columnSelected = columnSelected - 1 <= 0 ? 0 : columnSelected - 1;
                        break;
                }
                Console.Clear();
                for (int i = 0; i < text.Count; i++)
                {
                    Console.WriteLine($"{(i == columnSelected ? "> " : "  ")}{text[i]}");
                }
                            
                keyInfo = Console.ReadKey(true);
            }

            return columnSelected;
        }

        void Func()
        {
            string path =
                @$"{Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory)}\\{DateTime.Now.ToLongDateString()}.txt";
            if (!File.Exists(path))
            {
                File.CreateText(path).Close();
            }
            
            List<string> text = File.ReadLines(path).ToList();

            while (true)
            {
                Console.Clear();
                foreach (string i in text)
                {
                    Console.WriteLine(i);
                }
                Console.WriteLine("Input command >");
                string? input = Console.ReadLine()?.ToLower();

                switch (input)
                {
                    
                    case "save":
                        break;
                    
                    case "edit":
                        int columnSelected = selectLine(text);
                        Console.Clear();
                        string caret = "> ";
                        string defaultValue = text[columnSelected];
                        int stroke = 0;

                        List<char> buffer = defaultValue.ToCharArray().Take(Console.WindowWidth - caret.Length - 1)
                            .ToList();
                        List<List<char>> textbuffer = text.Select(x => x.ToCharArray().ToList()).ToList();
                        Console.Write(caret);
                        Console.Write(buffer.ToArray());
                        Console.SetCursorPosition(Console.CursorLeft, Console.CursorTop);
                        ConsoleKeyInfo keyInfo = Console.ReadKey(true);
                        
                        while (keyInfo.Key != ConsoleKey.Enter)
                        {
                            switch (keyInfo.Key)
                            {
                                case ConsoleKey.LeftArrow:
                                    Console.SetCursorPosition(Math.Max(Console.CursorLeft - 1, caret.Length),
                                        Console.CursorTop);
                                    break;
                                case ConsoleKey.RightArrow:
                                    Console.SetCursorPosition(
                                        Math.Min(Console.CursorLeft + 1, caret.Length + buffer.Count),
                                        Console.CursorTop);
                                    break;
                                // case ConsoleKey.UpArrow:
                                //     textbuffer[stroke] = buffer;
                                //     stroke = stroke - 1 <= 0 ? 0 : stroke - 1;
                                //     buffer = textbuffer[stroke].Take(Console.WindowWidth - caret.Length - 1).ToList();
                                //     break;
                                // case ConsoleKey.DownArrow:
                                //     textbuffer[stroke] = buffer;
                                //     stroke = stroke + 1 > textbuffer.Count - 1 ? textbuffer.Count - 1 : stroke + 1;
                                //     buffer = textbuffer[stroke].Take(Console.WindowWidth - caret.Length - 1).ToList();
                                //     break;
                                case ConsoleKey.Enter:
                                    Console.SetCursorPosition(
                                        Math.Min(Console.CursorLeft, caret.Length + buffer.Count),
                                        Console.CursorTop);
                                    textbuffer[stroke] = buffer;
                                    stroke = stroke + 1 > textbuffer.Count - 1 ? textbuffer.Count - 1 : stroke + 1;
                                    textbuffer.Insert(stroke, new List<char>());
                                    buffer = textbuffer[stroke].Take(Console.WindowWidth - caret.Length - 1).ToList();
                                    break;
                                    
                                case ConsoleKey.Home:
                                    Console.SetCursorPosition(caret.Length, Console.CursorTop);
                                    break;
                                case ConsoleKey.End:
                                    Console.SetCursorPosition(caret.Length + buffer.Count, Console.CursorTop);
                                    break;
                                case ConsoleKey.Backspace:
                                    if (Console.CursorLeft <= caret.Length)
                                    {
                                        // textbuffer.RemoveAt(stroke);
                                        // stroke = stroke - 1 <= 0 ? 0 : stroke - 1;
                                        // buffer = textbuffer[stroke].Take(Console.WindowWidth - caret.Length - 1).ToList();
                                        break;
                                    }

                                    int cursorColumnAfterBackspace = Math.Max(Console.CursorLeft - 1, caret.Length);
                                    buffer.RemoveAt(Console.CursorLeft - caret.Length - 1);
                                    RewriteLine(caret, buffer);
                                    Console.SetCursorPosition(cursorColumnAfterBackspace, Console.CursorTop);
                                    break;
                                case ConsoleKey.Delete:
                                    if (Console.CursorLeft >= caret.Length + buffer.Count)
                                        break;
                                    

                                    int cursorColumnAfterDelete = Console.CursorLeft;
                                    buffer.RemoveAt(Console.CursorLeft - caret.Length);
                                    RewriteLine(caret, buffer);
                                    Console.SetCursorPosition(cursorColumnAfterDelete, Console.CursorTop);
                                    break;
                                default:
                                    char character = keyInfo.KeyChar;
                                    if (character < 32)
                                        break;
                                    int cursorAfterNewChar = Console.CursorLeft + 1;
                                    if (cursorAfterNewChar > Console.WindowWidth ||
                                        caret.Length + buffer.Count >= Console.WindowWidth - 1)
                                        break;
     

                                    buffer.Insert(Console.CursorLeft - caret.Length, character);
                                    RewriteLine(caret, buffer);
                                    Console.SetCursorPosition(cursorAfterNewChar, Console.CursorTop);
                                    break;
                            }

                            keyInfo = Console.ReadKey(true);
                        }

                        Console.Write(Environment.NewLine);

                        text[columnSelected] = new string(buffer.ToArray());
                        continue;

                    default:
                        Console.Clear();
                        text = Operation.List.Any(x => x.Trigger == input) ? Operation.List.Find(x => x.Trigger == input).Fn(text) : text;
                        continue;
                }

                break;
            }
            File.WriteAllLines(path, text);
        }
        Func();
    }
}