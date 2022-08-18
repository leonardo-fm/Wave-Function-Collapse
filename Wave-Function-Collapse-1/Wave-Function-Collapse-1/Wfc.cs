using System.Text;
using Wave_Function_Collapse_1.Config;

namespace Wave_Function_Collapse_1;

public class Wfc
{
    private static WfcConfig _config;

    private readonly Random _randomObject = new();
    private readonly TileSet _currentSet = new();

    private static int _origRow;
    private static int _origCol;
    
    public Wfc(WfcConfig config)
    {
        _config = config;
    }
    
    public void Run()
    {
        var grid = InitializeGrid(_config.GridSize); 
        (int, int) currentTile;
        if(_config.ShowSteps)
        {
            DrawFullResult(grid);
            _origRow = Console.CursorTop;
            _origCol = Console.CursorLeft;
        }
        do
        {
            currentTile = GetTile(grid);
            if (currentTile == (-1, -1)) continue;
            SetTile(currentTile, ref grid);
            UpdateTile(currentTile, _config.GridSize, ref grid);
        } while (currentTile != (-1, -1));

        DrawFullResult(grid);
    }

    // private List<Tile> LoadTile()
    // {
    //     // new[] {'┐', '└', '┴', '┬', '├', '─', '┤', '│', '┼', '┘', '┌', ' '},
    //     var response = new List<Tile>
    //     {
    //         new Tile(
    //             '┐', 
    //             new[] {'└', '┴', '┬', '├', '─', '┼', '┌'},
    //             new[] {'└', '├', '│', '┌', ' '},
    //             new[] {'└', '┴', '─', '┘', ' '},
    //             new[] {'└', '┴', '├', '┤', '│', '┼', '┘'}
    //         ),
    //         new Tile(
    //             '└',
    //             new[] {'┐', '┤', '│', '┘', ' '},
    //             new[] {'┐', '┴', '┬', '─', '┤', '┼', '┘'},
    //             new[] {'┐', '┬', '├', '┤', '│', '┼', '┌'},
    //             new[] {'┐', '┬', '─', '┌', ' '}
    //         ),
    //         new Tile(
    //             '┴',
    //             new[] {'└', '┴', '┬', '├', '─', '┼', '┌'},
    //             new[] {'┐', '┴', '┬', '─', '┤', '┼', '┘'},
    //             new[] {'┐', '┬', '├', '┤', '│', '┼', '┌'},
    //             new[] {'┐', '┬', '─', '┌', ' '}
    //         ),
    //         new Tile(
    //             '┬',
    //             new[] {'└', '┴', '┬', '├', '─', '┼', '┌'},
    //             new[] {'┐', '┴', '┬', '─', '┤', '┼', '┘'},
    //             new[] {'└', '┴', '─', '┘', ' '},
    //             new[] {'└', '┴', '├', '┤', '│', '┼', '┘'}
    //         ),
    //         new Tile(
    //             '├',
    //             new[] {'┐', '┤', '│', '┘', ' '},
    //             new[] {'┐', '┴', '┬', '─', '┤', '┼', '┘'},
    //             new[] {'┐', '┬', '├', '┤', '│', '┼', '┌'},
    //             new[] {'└', '┴', '├', '┤', '│', '┼', '┘'}
    //         ),
    //         new Tile(
    //             '─',
    //             new[] {'└', '┴', '┬', '├', '─', '┼', '┌'},
    //             new[] {'┐', '┴', '┬', '─', '┤', '┼', '┘'},
    //             new[] {'└', '┴', '─', '┘', ' '},
    //             new[] {'┐', '┬', '─', '┌', ' '}
    //         ),
    //         new Tile(
    //             '┤',
    //             new[] {'└', '┴', '┬', '├', '─', '┼', '┌'},
    //             new[] {'└', '├', '│', '┌', ' '},
    //             new[] {'┐', '┬', '├', '┤', '│', '┼', '┌'},
    //             new[] {'└', '┴', '├', '┤', '│', '┼', '┘'}
    //         ),
    //         new Tile(
    //             '│',
    //             new[] {'┐', '┤', '│', '┘', ' '},
    //             new[] {'└', '├', '│', '┌', ' '},
    //             new[] {'┐', '┬', '├', '┤', '│', '┼', '┌'},
    //             new[] {'└', '┴', '├', '┤', '│', '┼', '┘'}
    //         ),
    //         new Tile(
    //             '┼',
    //             new[] {'└', '┴', '┬', '├', '─', '┼', '┌'},
    //             new[] {'┐', '┴', '┬', '─', '┤', '┼', '┘'},
    //             new[] {'┐', '┬', '├', '┤', '│', '┼', '┌'},
    //             new[] {'└', '┴', '├', '┤', '│', '┼', '┘'}
    //         ),
    //         new Tile(
    //             '┘',
    //             new[] {'└', '┴', '┬', '├', '─', '┼', '┌'},
    //             new[] {'└', '├', '│', '┌', ' '},
    //             new[] {'┐', '┬', '├', '┤', '│', '┼', '┌'},
    //             new[] {'┐', '┬', '─', '┌', ' '}
    //         ),
    //         new Tile(
    //             '┌',
    //             new[] {'┐', '┤', '│', '┘', ' '},
    //             new[] {'┐', '┴', '┬', '─', '┤', '┼', '┘'},
    //             new[] {'└', '┴', '─', '┘', ' '},
    //             new[] {'└', '┴', '├', '┤', '│', '┼', '┘'}
    //         ),
    //         new Tile(
    //             ' ',
    //             new[] {'┐', '┤', '│', '┘', ' '},
    //             new[] {'└', '├', '│', '┌', ' '},
    //             new[] {'└', '┴', '─', '┘', ' '},
    //             new[] {'┐', '┬', '─', '┌', ' '}
    //         )
    //     };
    //
    //     return response;
    // }
    private char[,][] InitializeGrid((int x, int y) size)
    {
        var response = new char[size.x, size.y][];
        for (var x = 0; x < size.x; x++)
        {
            for (var y = 0; y < size.y; y++)
            {
                response[x, y] = _config.CharsSet;
            }
        }
        
        return response;
    }

    // If return (-1, -1) then no more tile have to be updated
    private (int x, int y) GetTile(char[,][] grid)
    {
        var minFitTiles = int.MaxValue;
        var response = (-1, -1);
        
        for (var x = 0; x < grid.GetLength(0); x++)
        {
            for (var y = 0; y < grid.GetLength(1); y++)
            {
                if(grid[x, y].Length >= minFitTiles || grid[x, y].Length <= 1) continue;

                minFitTiles = grid[x, y].Length;
                response = (x, y);
            }
        }

        return response;
    }
    private void SetTile((int x, int y) tile, ref char[,][] grid)
    {
        var randomChar = _randomObject.Next(grid[tile.x, tile.y].Length);
        var response = grid[tile.x, tile.y][randomChar];
        grid[tile.x, tile.y] = new[] { response };
        if (!_config.ShowSteps) return;
        // Show steps
        WriteAt(tile, response);
        Thread.Sleep(_config.StepsSpeed);
    }
    private List<JsonTile> GetJsonTiles(char[] selectedChars, (int x, int y) tile, (int x, int y) grid)
    {
        var response = new List<JsonTile>();
        if(tile.x - 1 >= 0) response.Add(new JsonTile(selectedChars, (tile.x - 1, tile.y), Direction.Up));
        if(tile.x + 1 < grid.x) response.Add(new JsonTile(selectedChars, (tile.x + 1, tile.y), Direction.Down));
        if(tile.y - 1 >= 0) response.Add(new JsonTile(selectedChars, (tile.x, tile.y - 1), Direction.Left));
        if(tile.y + 1 < grid.y) response.Add(new JsonTile(selectedChars, (tile.x, tile.y + 1), Direction.Right));
        return response;
    }
    // If return (-1, -1) then the tail is not been updated
    private bool UpdateTileCharSet(JsonTile jsonTile, char[] tileChars, ref char[,][] grid)
    {
        var x = jsonTile.TilePosition.Item1;
        var y = jsonTile.TilePosition.Item2;
        if (grid[x, y].Length is 1) return false;
        var availableTails = _currentSet.GetFitTiles(tileChars, jsonTile.TileFitDirection);
        if (availableTails.Length == _config.CharsSet.Length) return false;
        grid[x, y] = grid[x, y].Intersect(availableTails).ToArray();
        return true;
    }
    private void UpdateTile((int x, int y) tile, (int x, int y) _gridSize, ref char[,][] grid)
    {
        var jsonTiles = GetJsonTiles(grid[tile.x, tile.y], tile, _gridSize);
        foreach (var jsonTile in jsonTiles)
        {
            var hasBeenUpdated = UpdateTileCharSet(jsonTile, grid[tile.x, tile.y], ref grid);
            if(hasBeenUpdated is false) continue;
            UpdateTile(jsonTile.TilePosition, _gridSize, ref grid);
        }
    }
    private string SelectResult(char[,][] grid)
    {
        var response = new StringBuilder();
        for (var x = 0; x < grid.GetLength(0); x++)
        {
            for (var y = 0; y < grid.GetLength(1); y++)
            {
                var charToDraw = grid[x, y].Length == 1 ? grid[x, y].First() : ' ';
                response.Append(charToDraw);
            }

            response.Append('\n');
        }

        return response.ToString();
    }
    private void DrawFullResult(char[,][] grid)
    {
        Console.Clear();
        var finalGrid = SelectResult(grid);
        Console.Write(finalGrid);
    }

    private static void WriteAt((int x, int y) position, char charToDraw)
    {
        try
        {
            Console.SetCursorPosition(_origCol + position.x, _origRow + position.y);
            Console.Write(charToDraw);
        }
        catch (ArgumentOutOfRangeException e)
        {
            Console.Clear();
            Console.WriteLine(e.Message);
        }
    }

    private class TileSet
    {
        public char[] GetFitTiles(char[] chars, Direction direction)
        {
            var response = new List<char>();
            foreach (var taleChar in chars)
            {
                var currentTile = _config.CharsRule.First(t => t.CurrentTile == taleChar);
                var availableTails = currentTile.GetAvailableTails(direction);
                foreach (var tail in availableTails)
                {
                    if(response.Contains(tail)) continue;
                    response.Add(tail);
                }
                
                if(response.Count == _config.CharsSet.Length) break;
            }

            return response.ToArray();
        }
    }
    public class Tile
    {
        public Tile(char currentTile, char[] left, char[] right, char[] up, char[] down)
        {
            CurrentTile = currentTile;
            Left = left;
            Right = right;
            Up = up;
            Down = down;
        }

        public char CurrentTile { get; set; }
        public char[] Left { get; set; }
        public char[] Right { get; set; }
        public char[] Up { get; set; }
        public char[] Down { get; set; }

        public char[] GetAvailableTails(Direction direction)
        {
            var availableTails = direction switch
            {
                Direction.Left => Left,
                Direction.Right => Right,
                Direction.Up => Up,
                Direction.Down => Down,
                _ => throw new ArgumentOutOfRangeException(nameof(direction), direction, null)
            };

            return availableTails;
        }
    } 
    public enum Direction
    {
        Left,
        Right,
        Up,
        Down
    }
    private class JsonTile
    {
        public JsonTile(char[] fatherTileSet, (int, int) tilePosition, Direction tileFitDirection)
        {
            FatherTileSet = fatherTileSet;
            TilePosition = tilePosition;
            TileFitDirection = tileFitDirection;
        }
        public char[] FatherTileSet { get; set; }
        public (int, int) TilePosition { get; set; }
        public Direction TileFitDirection { get; set; }
    }
}