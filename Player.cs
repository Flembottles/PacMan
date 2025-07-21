using System;
using System.Data;
using System.Threading;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;

namespace pacman;

public class Player
{
    public Rectangle frame;
    private int eatAnimFrameSize = 16;
    private Tilesystem tileSystem;
    private int frameTime;
    int reverseAnim = 1;
    int alternateDotSound = 1;
    int frames = 3;
    public Vector2 position;
    public int gridPositionX;
    public int gridPositionY;
    public Vector2 positionOnTile;
    public bool keyPressed = false;
    int nextTileX = 0;
    int nextTileY = 0;
    public float unitsToSnap = 4f;
    public bool onWall = false;

    SoundEffectInstance dot1Instance;
    SoundEffectInstance dot2Instance;
    public float rotation = 0f;

    public enum Direction
    {
        Right,
        Up,
        Left,
        Down
    }
    public Direction direction;
    public Player(Tilesystem tileSystem)
    {
        this.tileSystem = tileSystem;
        frameTime = 1;
        // X Frame Movement = 17
        frame = new Rectangle(488, 0, 14, 14);
        direction = Direction.Right;
        position = new Vector2(114, 212);
    }
    public void SetSound(SoundEffect dot1, SoundEffect dot2)
    {
        dot1Instance = dot1.CreateInstance();
        dot2Instance = dot2.CreateInstance();
    }
    public float GetRotation(Direction direction)
    {
        switch (direction)
        {
            case Direction.Right:
                return 0f;
            case Direction.Down:
                return MathHelper.PiOver2;
            case Direction.Left:
                return MathHelper.Pi;
            case Direction.Up:
                return 3 * MathHelper.PiOver2;
            default:
                return 0f;
        }
    }

    public void SetGridPosition()
    {
        if (position.X < 224 && position.X > 0)
        {
            gridPositionX = ((int)position.X) / 8;
            gridPositionY = ((int)position.Y - 28) / 8;
            positionOnTile.X = position.X % 8;
            positionOnTile.Y = (int)position.Y % 8;
        }
        
        if (tileSystem.tiles[gridPositionY, gridPositionX].tileType == Tilesystem.TileType.DOT)
        {
            alternateDotSound *= -1;
            tileSystem.tiles[gridPositionY, gridPositionX].tileType = Tilesystem.TileType.PATH;
            if (alternateDotSound == 1)
            {
                dot1Instance.Play();
            }
            else
            {
                dot2Instance.Play();
            }
        }
    }

    public void Update()
    {
        SetGridPosition();

        if (Keyboard.GetState().IsKeyDown(Keys.Up) && (tileSystem.tiles[gridPositionY - 1, gridPositionX].tileType != Tilesystem.TileType.WALL) && (int)positionOnTile.X == 4)
        {
            nextTileY = gridPositionY - 1;
            if (tileSystem.tiles[nextTileY, gridPositionX].tileType != Tilesystem.TileType.WALL)
            {
                onWall = false;
                unitsToSnap = 6f;
                direction = Direction.Up;
            }
        }
        if (Keyboard.GetState().IsKeyDown(Keys.Down) && (tileSystem.tiles[gridPositionY + 1, gridPositionX].tileType != Tilesystem.TileType.WALL) && (int)positionOnTile.X == 4)
        {
            
            nextTileY = gridPositionY + 1;
            if (tileSystem.tiles[nextTileY, gridPositionX].tileType != Tilesystem.TileType.WALL)
            {
                onWall = false;
                unitsToSnap = 6f;
                direction = Direction.Down;
            }
        }
        if (Keyboard.GetState().IsKeyDown(Keys.Left) && nextTileX > 0 && (tileSystem.tiles[gridPositionY, gridPositionX - 1].tileType != Tilesystem.TileType.WALL) && ((int)positionOnTile.Y == 4))
        {
            nextTileX = gridPositionX - 1;
            if (tileSystem.tiles[gridPositionY, nextTileX].tileType != Tilesystem.TileType.WALL)
            {
                onWall = false;
                unitsToSnap = 6f;
                direction = Direction.Left;
            }
        }
        if (Keyboard.GetState().IsKeyDown(Keys.Right) && nextTileX <= 24 && (tileSystem.tiles[gridPositionY, gridPositionX + 1].tileType != Tilesystem.TileType.WALL) && ((int)positionOnTile.Y == 4))
        { 
            nextTileX = gridPositionX + 1;
            if (tileSystem.tiles[gridPositionY, nextTileX].tileType != Tilesystem.TileType.WALL)
            {
                onWall = false;
                unitsToSnap = 6f;
                direction = Direction.Right;
            }
        }
        switch (direction)
        {
            case Direction.Right:
                if (position.X < 214 && position.X > 8)
                {
                    nextTileX = gridPositionX + 1;
                }
                nextTileY = gridPositionY;
                unitsToSnap -= 1f;
                if (unitsToSnap <= 0 && !onWall)
                {
                    unitsToSnap = 6f;
                }
                if (tileSystem.tiles[gridPositionY, nextTileX].tileType != Tilesystem.TileType.WALL)
                {
                    position.X += 1f;
                }
                else
                {
                    if (positionOnTile.X <= 3)
                    {
                        position.X += 1f;
                    }
                }
                if (position.X > 234)
                {
                    position.X = 0;
                }
                break;
            case Direction.Left:
                if (position.X < 214 && position.X > 8)
                {
                    nextTileX = gridPositionX - 1;
                }
                nextTileY = gridPositionY;
                unitsToSnap -= 1f;
                if (unitsToSnap <= 0 && !onWall)
                {
                    unitsToSnap = 6f;
                }
                if (tileSystem.tiles[gridPositionY, nextTileX].tileType != Tilesystem.TileType.WALL)
                {
                    position.X -= 1f;
                }
                else
                {
                    if (positionOnTile.X >= 5)
                    {
                        position.X -= 1f;
                    }
                }
                if (position.X < -10)
                {
                    position.X = 224;
                }
                break;
            case Direction.Up:
                nextTileX = gridPositionX;
                nextTileY = gridPositionY - 1;
                unitsToSnap -= 1f;
                if (unitsToSnap <= 0 && !onWall)
                {
                    unitsToSnap = 4f;
                }
                if (tileSystem.tiles[nextTileY, gridPositionX].tileType != Tilesystem.TileType.WALL)
                {
                    position.Y -= 1f;
                }
                else
                {
                    if ((int)positionOnTile.Y != 4)
                    {
                        position.Y -= 1f;
                    }
                }
                
                break;
            case Direction.Down:
                nextTileX = gridPositionX;
                nextTileY = gridPositionY + 1;
                unitsToSnap -= 1f;
                if (unitsToSnap <= 0 && !onWall)
                {
                    unitsToSnap = 6f;
                }
                if (tileSystem.tiles[nextTileY, gridPositionX].tileType != Tilesystem.TileType.WALL)
                {
                    position.Y += 1f;
                }
                else
                {
                    if ((int)positionOnTile.Y != 4)
                    {
                        position.Y -= 1f;
                    }
                }
                break;
        }

        if (frameTime > 0)
        {
            frameTime--;
        }
        else
        {
            frames--;
            if (frames == 0)
            {
                reverseAnim *= -1;
                frames = 2;
            }
            frameTime = 1;
            frame.X -= eatAnimFrameSize * reverseAnim;
        }

    }
}