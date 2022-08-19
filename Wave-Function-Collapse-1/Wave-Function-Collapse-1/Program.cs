using System.Diagnostics;
using Wave_Function_Collapse_1.Config;
using Wave_Function_Collapse_1.Wfc;

namespace Wave_Function_Collapse_1;

public static class Program
{
    public static void Main(string[] args)
    {
        var config = new WfcConfig();
        config.LoadConfigFromFile(@"D:\Projects\Wave-Function-Collapse\Wave-Function-Collapse-1\Wave-Function-Collapse-1\Config\FirstCharSet.json");
        WfcAlgorithm wfcAlgorithm = new(config);

        char regenerate;
        do
        {
            var sw = new Stopwatch();
            sw.Start();
            var collapsedFunction = wfcAlgorithm.Run();
            sw.Stop();
            WfcDraw.DrawCollapsedFunction(collapsedFunction);
            Console.WriteLine($"[{sw.ElapsedMilliseconds} ms] Regenerate? [y / n]");
            regenerate = Console.ReadKey().KeyChar;
        } while (regenerate == 'y');
    }
}