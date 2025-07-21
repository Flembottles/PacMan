using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Audio;
using System;
using System.IO;

namespace pacman;

public class Game1 : Game
{
    private GraphicsDeviceManager _graphics;
    private SpriteBatch _spriteBatch;
    Texture2D t_maze;
    Texture2D t_sheet;
    SoundEffect dot1;
    SoundEffect dot2;
    Tilesystem tileSystem;
    Player player;
    Vector2 origin;

    public Game1()
    {
        _graphics = new GraphicsDeviceManager(this);
        Content.RootDirectory = "Content";
        IsMouseVisible = true;
    }

    protected override void Initialize()
    {
        // TODO: Add your initialization logic here
        _graphics.PreferredBackBufferWidth = 224;
        _graphics.PreferredBackBufferHeight = 288;
        _graphics.ApplyChanges();
        tileSystem = new Tilesystem();
        tileSystem.LoadGrids();

        player = new Player(tileSystem);
        origin = new Vector2(player.frame.Width / 2, player.frame.Height / 2);
        base.Initialize();
    }

    protected override void LoadContent()
    {
        _spriteBatch = new SpriteBatch(GraphicsDevice);

        // TODO: use this.Content to load your game content here
        t_maze = Content.Load<Texture2D>("PacMaze");
        t_sheet = Content.Load<Texture2D>("PacSheet");

        dot1 = Content.Load<SoundEffect>("eat_dot_0");
        dot2 = Content.Load<SoundEffect>("eat_dot_1");
        player.SetSound(dot1, dot2);
    }

    protected override void Update(GameTime gameTime)
    {
        if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
            Exit();

        // TODO: Add your update logic here
        player.Update();
        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.Black);

        // TODO: Add your drawing code here
        _spriteBatch.Begin();
        _spriteBatch.Draw(t_maze, new Vector2(0, 24), Color.White);

        _spriteBatch.Draw(t_sheet, player.position, player.frame, Color.White, player.GetRotation(player.direction), origin, 1.0f, SpriteEffects.None, 0f);
        for (int row = 0; row < 31; row++)
        {
            for (int col = 0; col < 28; col++)
            {
                if (tileSystem.tiles[row, col].tileType == Tilesystem.TileType.DOT)
                {
                    _spriteBatch.Draw(t_sheet, new Vector2(tileSystem.tiles[row, col].position.X + 3, tileSystem.tiles[row, col].position.Y + 4), new Rectangle(607, 23, 2, 2), Color.White);
                }
            }
        }        
        _spriteBatch.End();
        base.Draw(gameTime);
    }
}
