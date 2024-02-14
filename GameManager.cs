using Higurashi_When_They_Cry_Hashiru.Entities;
using Higurashi_When_They_Cry_Hashiru.System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Higurashi_When_They_Cry_Hashiru;

public class GameManager : Game
{
    private const string ASSETNAME_SPRITESHEET = "spritesheet";

    private Texture2D _spritesheet;
    private GameState _gameState;
    private EntityManager _entityManager;
    private Keiichi _keiichi;
    
    
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

        base.Initialize();
    }

    protected override void LoadContent()
    {
        _spriteBatch = new SpriteBatch(GraphicsDevice);
        _spritesheet = Content.Load<Texture2D>(ASSETNAME_SPRITESHEET);
        
        _entityManager = new EntityManager();
        _gameState = GameState.Menu;
        _keiichi = new Keiichi(_spritesheet);
        
        _entityManager.AddEntity(_keiichi);
    }

    protected override void Update(GameTime gameTime)
    {
        KeyboardState keyboard = Keyboard.GetState();
        
        if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed ||
            Keyboard.GetState().IsKeyDown(Keys.Escape))
            Exit();

        switch (_gameState)
        {
            case GameState.Menu:
                if (keyboard.IsKeyDown(Keys.D))
                    _gameState = GameState.InGame;
                break;
            case GameState.InGame:
                if (keyboard.IsKeyDown(Keys.A))
                _gameState = GameState.Menu;
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
                _spriteBatch.Begin();
                _entityManager.Draw(_spriteBatch, gameTime);
                _spriteBatch.End();
                break;
        }

        base.Draw(gameTime);
    }
}