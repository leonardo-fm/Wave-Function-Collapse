using System.Text;

namespace Wave_Function_Collapse_1.Wfc;

public class WfcDraw
{
    public static void DrawCollapsedFunction(char[,] grid)
    {
        Console.Clear();
        var finalGrid = SelectResult(grid);
        Console.Write(finalGrid);
    }
    private static string SelectResult(char[,] grid)
    {
        var response = new StringBuilder();
        for (var x = 0; x < grid.GetLength(0); x++)
        {
            for (var y = 0; y < grid.GetLength(1); y++)
            {
                response.Append(grid[x, y]);
            }

            response.Append('\n');
        }

        return response.ToString();
    }
    public static void WriteAt((int x, int y) position, char charToDraw)
    {
        try
        {
            Console.SetCursorPosition(position.y, position.x);
            Console.Write(charToDraw);
        }
        catch (ArgumentOutOfRangeException e)
        {
            Console.Clear();
            Console.WriteLine(e.Message);
        }
    }
}