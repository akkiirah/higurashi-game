using System;
using System.Diagnostics;
using Higurashi_When_They_Cry_Hashiru.Graphics;
using Higurashi_When_They_Cry_Hashiru.System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Higurashi_When_They_Cry_Hashiru.Entities;

public class Keiichi : IGameEntity
{
    private const int IDLE_SPRITE_POS_X = 0;
    private const int IDLE_SPRITE_POS_Y = 0;
    private const int WALK_SPRITE_POS_Y = 113*2;
    private const int JUMP_SPRITE_POS_Y = 113*3;
    private const int SPRITE_WIDTH = 85;
    private const int SPRITE_HEIGHT = 113;
    private const float IDLE_ANIMATION_TIME = 1 / 8f;
    private const float RUN_ANIMATION_TIME = 1 / 12f;
    private const float JUMP_ANIMATION_TIME = 1 / 10f;
    private const int IDLE_ANIMATION_TOTAL_FRAMES = 4;
    private const int WALK_ANIMATION_TOTAL_FRAMES = 6;
    private const int JUMP_ANIMATION_TOTAL_FRAMES = 6;
    private const float GRAVITY = 1600f;
    private const float JUMP_START_VELOCITY = -680f;
    private const float DROP_VELOCITY = JUMP_START_VELOCITY * -2f;
    private const float CANCEL_JUMP_VELOCITY = -240f;
    private const float MIN_JUMP_HEIGHT = 20f;

    public event EventHandler jumpComplete;
    private float _dropVelocity;
    private float _verticalVelocity;
    private float _startPosY;
    private Vector2 Position { get; set; }
    public PlayerState State { get; private set; }

    
    public Sprite[] _idleSprites;
    private SpriteAnimation _idle;
    public Sprite[] _walkSprites;
    private SpriteAnimation _walk;
    public Sprite[] _jumpSprites;
    private SpriteAnimation _jump;
    
    
    public int DrawOrder { get; set; }

    public Keiichi(Texture2D spriteSheet, Vector2 position)
    {
        Position = new Vector2(position.X, position.Y - SPRITE_HEIGHT);
        State = PlayerState.Idle;
        
        _idleSprites = new Sprite[IDLE_ANIMATION_TOTAL_FRAMES];
        _idle = new SpriteAnimation();
        _walkSprites = new Sprite[WALK_ANIMATION_TOTAL_FRAMES];
        _walk = new SpriteAnimation();
        _jumpSprites = new Sprite[JUMP_ANIMATION_TOTAL_FRAMES];
        _jump = new SpriteAnimation();
        
        for (int i = 0; i < IDLE_ANIMATION_TOTAL_FRAMES; i++)
        {
            int posX = i == 0 ? 0 : SPRITE_WIDTH * i;
            _idleSprites[i] = new Sprite(spriteSheet, posX, IDLE_SPRITE_POS_Y, SPRITE_WIDTH, SPRITE_HEIGHT);
        }
        
        for (int i = 0; i < WALK_ANIMATION_TOTAL_FRAMES; i++)
        {
            int posX = i == 0 ? 0 : SPRITE_WIDTH * i;
            _walkSprites[i] = new Sprite(spriteSheet, posX, WALK_SPRITE_POS_Y, SPRITE_WIDTH, SPRITE_HEIGHT);
        }
        
        for (int i = 0; i < JUMP_ANIMATION_TOTAL_FRAMES; i++)
        {
            int posX = i == 0 ? 0 : SPRITE_WIDTH * i;
            _jumpSprites[i] = new Sprite(spriteSheet, posX, JUMP_SPRITE_POS_Y, SPRITE_WIDTH, SPRITE_HEIGHT);
        }
        
        _startPosY = Position.Y;

        CreateAnimation(_idle, _idleSprites, IDLE_ANIMATION_TIME);
        CreateAnimation(_walk, _walkSprites, RUN_ANIMATION_TIME);
        CreateAnimation(_jump, _jumpSprites, JUMP_ANIMATION_TIME);
        _jump.ShouldLoop = false;      
        _idle.Play();
        _walk.Play();
        _jump.Play();
    }

    public void Update(GameTime gameTime)
    {
        KeyboardState keyboard = Keyboard.GetState();
        
        Debug.WriteLine(State);
        
        switch (State)
        {
            case PlayerState.Idle:
                _idle.Update(gameTime);
                if (keyboard.IsKeyDown(Keys.D))
                    State = PlayerState.Walking;
                break;
            case PlayerState.Walking:
                _walk.Update(gameTime);
                if(!keyboard.IsKeyDown(Keys.D))
                    State = PlayerState.Idle;
                break;
            case PlayerState.Jumping:
            case PlayerState.Falling:
                _jump.Update(gameTime);
                UpdateJumping(gameTime);
                break;
        }
    }

    public void Draw(SpriteBatch spriteBatch, GameTime gameTime)
    {
        switch (State)
        {
            case PlayerState.Idle:
                _idle.Draw(spriteBatch, Position);
                break;
            case PlayerState.Walking:
                _walk.Draw(spriteBatch, Position);
                break; 
            case PlayerState.Falling:
            case PlayerState.Jumping:
                _jump.Draw(spriteBatch, Position);
                break; 
        }
    }

    private void CreateAnimation(SpriteAnimation spriteAnimation, Sprite[] sprites, float animTime)
    {
        spriteAnimation.Clear();
        
        for (int i = 0; i < sprites.Length; i++)
        {
            float time = i == 0 ? 0 : animTime * i;
            spriteAnimation.AddFrame(sprites[i], time);
        }
    }
    
    private void UpdateJumping(GameTime gameTime)
    {
        Position = new Vector2(Position.X, Position.Y + _verticalVelocity * (float)gameTime.ElapsedGameTime.TotalSeconds
                                                      + _dropVelocity * (float)gameTime.ElapsedGameTime.TotalSeconds);
        _verticalVelocity += GRAVITY * (float)gameTime.ElapsedGameTime.TotalSeconds;

        if (_verticalVelocity >= 0)
            State = PlayerState.Falling;

        if (Position.Y >= _startPosY)
        {
            Position = new Vector2(Position.X, _startPosY);
            _verticalVelocity = 0;
            _jump.ResetAnim();
            State = PlayerState.Idle;
        }
    }
    
    public bool BeginJump()
    {
        if (State == PlayerState.Jumping || State == PlayerState.Falling)
            return false;

        State = PlayerState.Jumping;
        _jump.Play();

        _verticalVelocity = JUMP_START_VELOCITY;
        
        return true;
    }

    public bool CancelJump()
    {
        if (State != PlayerState.Jumping || (_startPosY - Position.Y) < MIN_JUMP_HEIGHT)
            return false;
        
        _verticalVelocity = _verticalVelocity < CANCEL_JUMP_VELOCITY ? CANCEL_JUMP_VELOCITY : 0;
        
        return true;
    }
    
    public bool Duck()
    {
        if (State == PlayerState.Jumping || State == PlayerState.Falling)
            return false;

        State = PlayerState.Ducking;
        return true;
    }
    public bool GetUp()
    {
        if (State != PlayerState.Ducking)
            return false;

        State = PlayerState.Running;
        return true;
    }
    public bool Drop()
    {
        if (State != PlayerState.Jumping && State != PlayerState.Falling)
            return false;

        State = PlayerState.Falling;

        _dropVelocity = DROP_VELOCITY;
        
        return true;
    }

}