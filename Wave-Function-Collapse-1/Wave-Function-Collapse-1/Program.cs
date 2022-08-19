using System.Diagnostics;
using Wave_Function_Collapse_1.Config;
using Wave_Function_Collapse_1.Wfc;

namespace Wave_Function_Collapse_1;

public static class Program
{
    public static void Main(string[] args)
    {
        Console.WriteLine("Press any key to start");
        Console.ReadKey();
        
        var config = new WfcConfig();
        config.LoadConfigFromFile(@"D:\Projects\Wave-Function-Collapse\Wave-Function-Collapse-1\Wave-Function-Collapse-1\Config\FirstCharSet.json");
        WfcAlgorithm wfcAlgorithm = new(config);

        char regenerate;
        do
        {
            Console.Clear();
            var sw = new Stopwatch();
            sw.Start();
            Console.CursorVisible = false;
            var collapsedFunction = wfcAlgorithm.Run();
            sw.Stop();
            WfcDraw.DrawCollapsedFunction(collapsedFunction);
            Console.CursorVisible = true;
            Console.WriteLine($"[{sw.ElapsedMilliseconds} ms] Regenerate? [y / n]");
            regenerate = Console.ReadKey().KeyChar;
        } while (regenerate == 'y');
    }
}