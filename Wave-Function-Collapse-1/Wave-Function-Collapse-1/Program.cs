using Wave_Function_Collapse_1.Config;

namespace Wave_Function_Collapse_1;

public static class Program
{
    public static void Main(string[] args)
    {
        Wfc wfc = new(new WfcConfig(@"D:\Projects\Wave-Function-Collapse\Wave-Function-Collapse-1\Wave-Function-Collapse-1\Config\FirstCharSet.json"));
        char regenerate;
        do
        {
            wfc.Run();
            Console.WriteLine("Regenerate? [y / n]");
            regenerate = Console.ReadKey().KeyChar;
        } while (regenerate == 'y');
    }
}