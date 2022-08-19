using Wave_Function_Collapse_1.Config;

namespace Wave_Function_Collapse_1.Wfc;

public class WfcAlgorithm
{
    private static WfcConfig _config = null!;

    private readonly Random _randomObject = new();

    public WfcAlgorithm(WfcConfig config)
    {
        _config = config;
    }
    
    public char[,] Run()
    {
        var grid = InitializeGrid(_config.GridSize);
        (int, int) currentTile;
        do
        {
            currentTile = GetTile(grid);
            if (currentTile == (-1, -1)) continue;
            SetTile(currentTile, ref grid);
            UpdateTile(currentTile, _config.GridSize, ref grid);
        } while (currentTile != (-1, -1));

        return GetFormattedResult(grid, _config.GridSize);
    }

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
    private char[,] GetFormattedResult(char[,][] grid, (int x, int y) size)
    {
        var response = new char[size.x, size.y];
        for (var x = 0; x < size.x; x++)
        {
            for (var y = 0; y < size.y; y++)
            {
                response[x, y] = grid[x, y].First();
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
        WfcDraw.WriteAt(tile, response);
        Thread.Sleep(_config.StepsSpeed);
    }
    private List<NearTile> GetJsonTiles(char[] selectedChars, (int x, int y) tile, (int x, int y) grid)
    {
        var response = new List<NearTile>();
        if(tile.x - 1 >= 0) response.Add(new NearTile(selectedChars, (tile.x - 1, tile.y), Direction.Up));
        if(tile.x + 1 < grid.x) response.Add(new NearTile(selectedChars, (tile.x + 1, tile.y), Direction.Down));
        if(tile.y - 1 >= 0) response.Add(new NearTile(selectedChars, (tile.x, tile.y - 1), Direction.Left));
        if(tile.y + 1 < grid.y) response.Add(new NearTile(selectedChars, (tile.x, tile.y + 1), Direction.Right));
        return response;
    }
    // If return (-1, -1) then the tail is not been updated
    private bool UpdateTileCharSet(NearTile nearTile, char[] tileChars, ref char[,][] grid)
    {
        var x = nearTile.TilePosition.Item1;
        var y = nearTile.TilePosition.Item2;
        if (grid[x, y].Length is 1) return false;
        var availableTails = GetFitTiles(tileChars, nearTile.TileFitDirection);
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
    private char[] GetFitTiles(char[] chars, Direction direction)
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
    private class NearTile
    {
        public NearTile(char[] fatherTileSet, (int, int) tilePosition, Direction tileFitDirection)
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