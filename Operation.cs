namespace SHARP2;

public class Operation
{
    public readonly string Trigger;
    public readonly Func<List<string>, List<string>> Fn;
    public static readonly List<Operation> List = new();

    private Operation(string trigger, Func<List<string>, List<string>> fn)
    {
        Trigger = trigger;
        Fn = fn;
        List.Add(this);
    }

    public static void CreateOperations()
    {

        List<string> ToLower(List<string> var)
        {
            for (int i = 0; i < var.Count; i++)
            {
                var[i] = var[i].ToLower();
            }
            return var;
        }
        
        List<string> ToUpper(List<string> var)
        {
            for (int i = 0; i < var.Count; i++)
            {
                var[i] = var[i].ToUpper();
            }
            return var;
        }

        List<string> Replace(List<string> var)
        {
            Console.WriteLine("Replace > ");
            string? oldValue = Console.ReadLine();
            Console.WriteLine("With > ");
            string newValue = Console.ReadLine()!;
            for (int i = 0; i < var.Count; i++)
            {
                var[i] = var[i].Replace(oldValue!, newValue);
            }

            return var;
        }

        new Operation("tolower", ToLower);
        new Operation("toupper", ToUpper);
        new Operation("replace", Replace);
    }
}