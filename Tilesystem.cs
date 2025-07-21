using System;
using System.Data;
using System.IO;
using System.Linq;
using System.Numerics;

namespace pacman;

public class Tilesystem
{
    public string data;
    public string raw;
    public enum TileType { WALL, DOT, PATH, POWER, TUNNEL }

    public struct Tile
    {
        public TileType tileType;
        public Vector2 position;
    }

    public Tile[,] tiles;

    public Tilesystem()
    {
        tiles = new Tile[31, 28];
    }
    public Vector2 SetPosition(Vector2 pos)
    {
        return pos;
    }
    public void LoadGrids()
    {
        raw = File.ReadAllText("Content/PacmanGrid.txt");
        data = new string(raw.Where(c => !char.IsWhiteSpace(c)).ToArray());
        int dataIndex = 0;
        Vector2 setPosition = new Vector2(0, 24);

        for (int row = 0; row < 31; row++)
        {
            for (int col = 0; col < 28; col++)
            {
                Tile tile = new Tile();
                tile.position = setPosition;
                switch (data[dataIndex])
                {
                    case 'W':
                        tile.tileType = TileType.WALL;
                        break;
                    case 'D':
                        tile.tileType = TileType.DOT;
                        break;
                    case 'P':
                        tile.tileType = TileType.POWER;
                        break;
                    case 'T':
                        tile.tileType = TileType.TUNNEL;
                        break;
                    case 'O':
                        tile.tileType = TileType.PATH;
                        break;
                }
                tiles[row, col] = tile;
                dataIndex++;
                setPosition.X += 8;
            }
            setPosition.Y += 8;
            setPosition.X = 0;
        }
    }
}