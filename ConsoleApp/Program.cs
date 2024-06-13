namespace ConsoleApp;

using SourceGenerators;

public partial class UserClass
{
    public static void Main()
    {
        Console.WriteLine(AddCopy(1, 1));
    }

    [GenerateCopyMethod]
    public static double Add(double a, double b)
    {
        return a + b;
    }
}
