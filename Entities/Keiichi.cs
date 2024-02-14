using System.Collections.Generic;
using System.Diagnostics;
using Higurashi_When_They_Cry_Hashiru.Graphics;
using Higurashi_When_They_Cry_Hashiru.System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Trex_Runner_Game.Graphics;

namespace Higurashi_When_They_Cry_Hashiru.Entities;

public class Keiichi : IGameEntity
{
    private const int IDLE_SPRITE_POS_X = 0;
    private const int IDLE_SPRITE_POS_Y = 0;
    private const int WALK_SPRITE_POS_Y = 113*2;
    private const int SPRITE_WIDTH = 85;
    private const int SPRITE_HEIGHT = 113;
    private const float IDLE_ANIMATION_TIME = 1 / 12f;
    private const int IDLE_ANIMATION_TOTAL_FRAMES = 4;
    private const int WALK_ANIMATION_TOTAL_FRAMES = 6;

    private PlayerState _playerState;
    public int DrawOrder { get; }
    
    public Sprite[] _idleSprites;
    private SpriteAnimation _idle;
    
    public Sprite[] _walkSprites;
    private SpriteAnimation _walk;

    public Keiichi(Texture2D spriteSheet)
    {
        _idleSprites = new Sprite[IDLE_ANIMATION_TOTAL_FRAMES];
        _idle = new SpriteAnimation();
        _walkSprites = new Sprite[WALK_ANIMATION_TOTAL_FRAMES];
        _walk = new SpriteAnimation();
        
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

        CreateAnimation(_idle, _idleSprites, IDLE_ANIMATION_TIME);
        CreateAnimation(_walk, _walkSprites, IDLE_ANIMATION_TIME);
    }
    
    public void Update(GameTime gameTime)
    {
        KeyboardState keyboard = Keyboard.GetState();
        
        switch (_playerState)
        {
            case PlayerState.Idle:
                if (!_idle.IsPlaying)
                {
                    CreateAnimation(_idle, _idleSprites, IDLE_ANIMATION_TIME);
                    _idle.Play();
                }
                _idle.Update(gameTime);
                if (keyboard.IsKeyDown(Keys.F))
                    _playerState = PlayerState.Walking;
                break;
            case PlayerState.Walking:
                if (!_walk.IsPlaying)
                {
                    CreateAnimation(_walk, _walkSprites, IDLE_ANIMATION_TIME);
                    _walk.Play();
                }
                _walk.Update(gameTime);
                break;
        }
    }

    public void Draw(SpriteBatch spriteBatch, GameTime gameTime)
    {
        switch (_playerState)
        {
            case PlayerState.Idle:
                _idle.Draw(spriteBatch, new Vector2(0,0));
                break;
            case PlayerState.Walking:
                _walk.Draw(spriteBatch, new Vector2(0,0));
                break; 
        }
    }

    private void CreateAnimation(SpriteAnimation spriteAnimation, Sprite[] sprites, float animTime)
    {
        spriteAnimation.Clear();
        
        for (int i = 0; i < sprites.Length; i++)
        {
            Debug.WriteLine(i);
            float time = i == 0 ? 0 : animTime * i;
            Debug.WriteLine(time);
            spriteAnimation.AddFrame(sprites[i], time);
        }

    }
}