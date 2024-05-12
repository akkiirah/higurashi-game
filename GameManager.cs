using System.Diagnostics;
using Higurashi_Game.Entities;
using Higurashi_Game.System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Higurashi_Game;

public class GameManager : Game
{
    public const int VIRTUAL_WINDOW_WIDTH = 1600;
    public const int VIRTUAL_WINDOW_HEIGHT = 900;
    public const int WINDOW_WIDTH = 1600;
    public const int WINDOW_HEIGHT = 900;
    public float scaleX;
    public float scaleY;
    private Matrix matrix;

    private const string ASSETNAME_SPRITESHEET = "spritesheet";
    private const string ASSETNAME_TEST = "test1 - Kopie";
    private const string ASSETNAME_BG = "bgtest";
    
    public const int CHARACTER_START_POS_Y = VIRTUAL_WINDOW_HEIGHT - 188;
    public const int KEIICHI_START_POS_X = 1;

    private Texture2D _spritesheet;
    private Texture2D _test;
    private Texture2D _bg;
    
    private GameState _gameState;
    private EntityManager _entityManager;
    private Keiichi _keiichi;
    private InputManager _inputManager;
    
    private GraphicsDeviceManager _graphics;
    private SpriteBatch _spriteBatch;
    

    

    public GameManager()
    {
        _graphics = new GraphicsDeviceManager(this);
        Content.RootDirectory = "Content";
        IsMouseVisible = true;
        
    }

    protected override void Initialize()
    {
        _graphics.PreferredBackBufferWidth = WINDOW_WIDTH;
        _graphics.PreferredBackBufferHeight = WINDOW_HEIGHT;
        _graphics.ApplyChanges();

        scaleX = (float)GraphicsDevice.Viewport.Width    / VIRTUAL_WINDOW_WIDTH;
        scaleY = (float)GraphicsDevice.Viewport.Height  / VIRTUAL_WINDOW_HEIGHT;
        matrix = Matrix.CreateScale(scaleX, scaleY, 1.0f);
        
        base.Initialize();
    }

    protected override void LoadContent()
    {
        Vector2 keiichiStartPos = new Vector2(KEIICHI_START_POS_X, CHARACTER_START_POS_Y);
        
        _spriteBatch = new SpriteBatch(GraphicsDevice);
        _spritesheet = Content.Load<Texture2D>(ASSETNAME_SPRITESHEET);
        _test = Content.Load<Texture2D>(ASSETNAME_TEST);
        _bg = Content.Load<Texture2D>(ASSETNAME_BG);
        
        _entityManager = new EntityManager();
        _gameState = GameState.Menu;
        
        _keiichi = new Keiichi(_test , keiichiStartPos);
        //_keiichi.jumpComplete += keiichi_JumpComplete;
        
        _inputManager = new InputManager(_keiichi);
        
        _entityManager.AddEntity(_keiichi);
    }

    protected override void Update(GameTime gameTime)
    {
        Debug.WriteLine((float)GraphicsDevice.Viewport.Width);
        KeyboardState keyboard = Keyboard.GetState();
        
        if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed ||
            Keyboard.GetState().IsKeyDown(Keys.Escape))
            Exit();

        switch (_gameState)
        {
            case GameState.Menu:
                _gameState = keyboard.IsKeyDown(Keys.Q) ? GameState.InGame : _gameState;
                break;
            case GameState.InGame:
                if (keyboard.IsKeyDown(Keys.E))
                _gameState = GameState.Menu;
                _inputManager.ProcessControls(gameTime);
                _entityManager.Update(gameTime);
                break;
            
        }


        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {

        switch (_gameState)
        {
            case GameState.Menu:
                GraphicsDevice.Clear(Color.Wheat);
                break;
            case GameState.Transition: 
                break;
            case GameState.InGame:
                GraphicsDevice.Clear(Color.Black);
                _spriteBatch.Begin(transformMatrix: matrix);
                _spriteBatch.Draw(_bg, new Rectangle(0, 0, VIRTUAL_WINDOW_WIDTH, VIRTUAL_WINDOW_HEIGHT), Color.White);
                _entityManager.Draw(_spriteBatch, gameTime);
                _spriteBatch.End();
                break;
        }

        base.Draw(gameTime);
    }
}


